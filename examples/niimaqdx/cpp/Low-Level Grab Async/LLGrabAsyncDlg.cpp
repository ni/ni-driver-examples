//////////////////////////////////////////////////////////////////////////////
//
//  Title     : LLGrabAsyncDlg.cpp
//  Project   : NI-IMAQdx
//  Created   : 8/29/2006 @ 18:10:21
//  Platforms : All
//  Access    : Public
//  Purpose   : Illustrates how to perform an asynchronous, low-level grab
//              acquisition, built using MSVC.
//
//////////////////////////////////////////////////////////////////////////////


//============================================================================
//  Includes
//============================================================================
#include "stdafx.h"
#include "LLGrabAsync.h"
#include "LLGrabAsyncDlg.h"

//============================================================================
//  Debug defines
//============================================================================
#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

//============================================================================
//  VisionError - Type typedef is simply used to differentiate NI Vision errors
//      vs. driver errors during error handling.
//============================================================================
typedef int VisionError;


//============================================================================
//  Constants
//============================================================================
const char* InitialFrameRate = "0.000000";


//============================================================================
//  Custom messages
//============================================================================
#define WM_UPDATEDIALOG (WM_USER + 1)


//////////////////////////////////////////////////////////////////////////////
//
//  CLLGrabAsyncDlg::CLLGrabAsyncDlg
//
//  Description:
//      Constructs the main CLLGrabAsyncDlg.
//
//////////////////////////////////////////////////////////////////////////////
CLLGrabAsyncDlg::CLLGrabAsyncDlg(CWnd* pParent) :
    CDialog(CLLGrabAsyncDlg::IDD, pParent),
    session(0),
    image(NULL)
{
	//{{AFX_DATA_INIT(CLLGrabAsyncDlg)
	actualBufferNumber = 0;
	camName = _T("cam0");
	FrameRateString = _T(InitialFrameRate);
	//}}AFX_DATA_INIT
	// Note that LoadIcon does not require a subsequent DestroyIcon in Win32
	m_hIcon = AfxGetApp()->LoadIcon(IDR_IMAQDX);
    imaqSetWindowThreadPolicy(IMAQ_SEPARATE_THREAD);
}

//////////////////////////////////////////////////////////////////////////////
//
//  CLLGrabAsyncDlg::DoDataExchange
//
//  Description:
//      Exchanges data between our member variables and the dialog.
//
//////////////////////////////////////////////////////////////////////////////
void CLLGrabAsyncDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CLLGrabAsyncDlg)
	DDX_Control(pDX, IDC_CAMNAME, camNameControl);
	DDX_Control(pDX, ID_STOP, stopButton);
	DDX_Control(pDX, ID_GRAB, grabButton);
	DDX_Text(pDX, IDC_BUFFERNUM, actualBufferNumber);
	DDX_Text(pDX, IDC_CAMNAME, camName);
	DDX_Text(pDX, IDC_FRAMESPERSEC, FrameRateString);
	//}}AFX_DATA_MAP
}

//============================================================================
//  Message map
//============================================================================
BEGIN_MESSAGE_MAP(CLLGrabAsyncDlg, CDialog)
	//{{AFX_MSG_MAP(CLLGrabAsyncDlg)
	ON_BN_CLICKED(ID_GRAB, OnGrab)
	ON_BN_CLICKED(ID_STOP, OnStop)
	ON_BN_CLICKED(ID_QUIT, OnQuit)
    ON_MESSAGE(WM_UPDATEDIALOG, OnUpdateDialog)

	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//////////////////////////////////////////////////////////////////////////////
//
//  CLLGrabAsyncDlg::OnInitDialog
//
//  Description:
//      Message handler which is called when it's time to initialize the dialog.
//
//////////////////////////////////////////////////////////////////////////////
BOOL CLLGrabAsyncDlg::OnInitDialog()
{
    //------------------------------------------------------------------------
    //  Let the base method run & initialize our big & small icons.  Also,
    //  start out with the Stop button disabled and the Start button enabled.
    //------------------------------------------------------------------------
	CDialog::OnInitDialog();
	SetIcon(m_hIcon, TRUE);			// Set big icon
	SetIcon(m_hIcon, FALSE);		// Set small icon
    camNameControl.EnableWindow(TRUE);
    grabButton.EnableWindow(TRUE);
    stopButton.EnableWindow(FALSE);
	return TRUE;  // return TRUE  unless you set the focus to a control
}


//////////////////////////////////////////////////////////////////////////////
//
//  CLLGrabAsyncDlg::OnGrab
//
//  Description:
//      Message handler which is called when the user single-clicks the "Grab"
//      button.  Here, we'll start the grab.
//
//////////////////////////////////////////////////////////////////////////////
void CLLGrabAsyncDlg::OnGrab() 
{
    bool startedSuccessfully = false;
    UpdateData (TRUE);
    FrameRateString = InitialFrameRate;
    UpdateData (FALSE);
    try {
		//--------------------------------------------------------------------
		//  Create an image for display purposes.  
		//--------------------------------------------------------------------
		image = imaqCreateImage (IMAQ_IMAGE_U8, 0);
		if (!image)
			throw VisionError();

        //--------------------------------------------------------------------
        //  Open an interface to our board, as given by the interface name.
        //--------------------------------------------------------------------
        IMAQdxError status = IMAQdxOpenCamera (camName, IMAQdxCameraControlModeController, &session);
        if (status)
            throw status;

        //--------------------------------------------------------------------
        //  Now it's time to configure, register the asynchronous callback and 
        //  start the grab.  If the acquisition starts properly, go ahead and 
		//  start the grab thread.
        //--------------------------------------------------------------------
        status = IMAQdxConfigureAcquisition (session, TRUE, 3);
        if (status)
            throw status;

        status = IMAQdxRegisterFrameDoneEvent (session, 1, FrameDoneCallback, this); 
        if (status)
            throw status;

		status = IMAQdxStartAcquisition (session);
		if (status)
            throw status;

        camNameControl.EnableWindow (FALSE);
        grabButton.EnableWindow (FALSE);
        stopButton.EnableWindow (TRUE);
        startedSuccessfully = true;
    }
    catch (IMAQdxError error) {
        DisplayNIIMAQdxError (error);
    }
    catch (VisionError) {
        DisplayNIVisionError (imaqGetLastError());
    }

    //------------------------------------------------------------------------
    //  If the acquisition wasn't started properly, we need to clean up.
    //  Otherwise, just update the state of the buttons and let it run.
    //------------------------------------------------------------------------
    if (!startedSuccessfully) 
	{
        IMAQdxCloseCamera (session);
        session = 0;
    }
}

//////////////////////////////////////////////////////////////////////////////
//
//  CLLGrabAsyncDlg::FrameDoneCallback
//
//  Description:
//      This is the frame done callback.  Its job is to get images from
//      the driver and display them.
//
//////////////////////////////////////////////////////////////////////////////
uInt32 __stdcall CLLGrabAsyncDlg::FrameDoneCallback (IMAQdxSession session, uInt32 bufferNumber, void* callbackData) 
{
	return reinterpret_cast<CLLGrabAsyncDlg*>(callbackData)->FrameDoneCallbackInternal(session, bufferNumber);
}


uInt32 CLLGrabAsyncDlg::FrameDoneCallbackInternal (IMAQdxSession session, uInt32 bufferNumber)
{
	//------------------------------------------------------------------------
    //  Start grabbing images until either the user tells us to stop or we
    //  encounter an error.
    //------------------------------------------------------------------------
    IMAQdxError status = IMAQdxErrorSuccess;
    static uInt32 lastBufferNumber = -1;
    static DWORD lastTick = GetTickCount();

	// Acquire image
	status = IMAQdxGetImage (session, image, IMAQdxBufferNumberModeBufferNumber, bufferNumber, &bufferNumber);
		
	// Display image
	imaqDisplayImage (image, 0, TRUE);

    //--------------------------------------------------------------------
    //  Every so often, update the dialog with the current frame rate and
	//  buffer number.  The frame rate calculation interface is given by 
	//	frameRateInterval, in seconds.  This is a running average of the 
	//	most recent N images within an interval.
    //--------------------------------------------------------------------
    const double frameRateInterval = 0.5;
    DWORD newTick = GetTickCount();
    if ((double)(newTick - lastTick) / 1000.0 >= frameRateInterval) 
	{
        double frameRate = ((double)(bufferNumber - lastBufferNumber) / (double)(newTick - lastTick)) * 1000.0;
        FrameRateString.Format("%.6f", frameRate);
        lastTick = newTick;
        lastBufferNumber = bufferNumber;
		actualBufferNumber = bufferNumber;
        PostMessage(WM_UPDATEDIALOG);
    }

    //------------------------------------------------------------------------
    //  If we got an error, display error and stop the acquisition.
    //------------------------------------------------------------------------
    if (status) 
	{
        DisplayNIIMAQdxError(status);
        SetActiveWindow();
        stopButton.PostMessage(BM_CLICK);
    }

	//------------------------------------------------------------------------
	//	Rearm the callback
	//------------------------------------------------------------------------
    return TRUE;
}

//////////////////////////////////////////////////////////////////////////////
//
//  CLLGrabAsyncDlg::OnStop
//
//  Description:
//      Message handler which is called when the user single-clicks the "Stop"
//      button.  Here, we'll stop the grab.
//
//////////////////////////////////////////////////////////////////////////////
void CLLGrabAsyncDlg::OnStop() 
{
    //------------------------------------------------------------------------
    //  Stop the acquisition and close the session. Dispose of the image.
    //------------------------------------------------------------------------
    if (session) 
	{
		IMAQdxStopAcquisition (session);
		IMAQdxUnconfigureAcquisition (session);
		IMAQdxCloseCamera (session);
        session = 0;
    }
    
	if (image)
	{
	    imaqDispose(image);
		image = NULL;
	}

	camNameControl.EnableWindow(TRUE);
    grabButton.EnableWindow(TRUE);
    stopButton.EnableWindow(FALSE);
}

//////////////////////////////////////////////////////////////////////////////
//
//  CLLGrabAsyncDlg::OnQuit
//
//  Description:
//      Message handler which is called when the user single-clicks the "Quit"
//      button.
//
//////////////////////////////////////////////////////////////////////////////
void CLLGrabAsyncDlg::OnQuit() 
{
    //------------------------------------------------------------------------
    //  Stop the acquisition and clean up the image display.
    //------------------------------------------------------------------------
    OnStop();
    imaqShowWindow(0, false);
	OnCancel();
}

//////////////////////////////////////////////////////////////////////////////
//
//  CLLGrabAsyncDlg::OnUpdateDialog
//
//  Description:
//      Message handler used to update the dialog with the latest data.
//
//////////////////////////////////////////////////////////////////////////////
LRESULT CLLGrabAsyncDlg::OnUpdateDialog(WPARAM, LPARAM) {
    UpdateData(FALSE);
    return 0;
}


//////////////////////////////////////////////////////////////////////////////
//
//  CLLGrabAsyncDlg::DisplayNIIMAQdxError
//
//  Description:
//      Displays a message box containing the NI-IMAQdx error information.
//
//  Parameters:
//      error - The error code
//
//////////////////////////////////////////////////////////////////////////////
void CLLGrabAsyncDlg::DisplayNIIMAQdxError(IMAQdxError error) {
    //--------------------------------------------------------------------
    //  Display a dialog containing the NI-IMAQdx error we encountered.
    //--------------------------------------------------------------------
    Int8 errorText[512];
    sprintf(errorText, "Error Code = 0x%08X\n\n", error);
    IMAQdxGetErrorString (error, errorText + strlen(errorText), uInt32(sizeof(errorText) - strlen(errorText)));
    MessageBox(errorText, "NI-IMAQdx Error");
}


//////////////////////////////////////////////////////////////////////////////
//
//  CLLGrabAsyncDlg::DisplayNIVisionError
//
//  Description:
//      Displays a message box containing the NI Vision error information.
//
//  Parameters:
//      error - The error code
//
//////////////////////////////////////////////////////////////////////////////
void CLLGrabAsyncDlg::DisplayNIVisionError(int error) {
    //--------------------------------------------------------------------
    //  Display a dialog containing the NI Vision error we encountered.
    //--------------------------------------------------------------------
    char* errorText = imaqGetErrorText(error);
    char dialogText[512];
    sprintf(dialogText, "Error Code = 0x%08X\n\n%s", error, errorText);
    MessageBox(dialogText, "NI Vision Error");
    imaqDispose(errorText);
}
