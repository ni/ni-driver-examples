//////////////////////////////////////////////////////////////////////////////
//
//  Title     : LLGrabDlg.cpp
//  Project   : NI-IMAQdx
//  Created   : 8/29/2006 @ 10:33:21
//  Platforms : All
//  Access    : Public
//  Purpose   : Illustrates how to perform a simple, low-level grab acquisition,
//              built using MSVC.
//
//////////////////////////////////////////////////////////////////////////////


//============================================================================
//  Includes
//============================================================================
#include "stdafx.h"
#include "LLGrab.h"
#include "LLGrabDlg.h"

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
//  CLLGrabDlg::CLLGrabDlg
//
//  Description:
//      Constructs the main CLLGrabDlg.
//
//////////////////////////////////////////////////////////////////////////////
CLLGrabDlg::CLLGrabDlg(CWnd* pParent) :
    CDialog(CLLGrabDlg::IDD, pParent),
    session(0),
    image(NULL),
    grabThread(NULL),
    stopThread(false)
{
	//{{AFX_DATA_INIT(CLLGrabDlg)
	camName = _T("cam0");
	FrameRateString = _T(InitialFrameRate);
	actualBufferNumber = 0;
	//}}AFX_DATA_INIT
	// Note that LoadIcon does not require a subsequent DestroyIcon in Win32
	m_hIcon = AfxGetApp()->LoadIcon(IDR_IMAQDX);
    imaqSetWindowThreadPolicy(IMAQ_SEPARATE_THREAD);

}

//////////////////////////////////////////////////////////////////////////////
//
//  CLLGrabDlg::DoDataExchange
//
//  Description:
//      Exchanges data between our member variables and the dialog.
//
//////////////////////////////////////////////////////////////////////////////
void CLLGrabDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CLLGrabDlg)
	DDX_Control(pDX, IDC_CAMNAME, camNameControl);
	DDX_Control(pDX, ID_STOP, stopButton);
	DDX_Control(pDX, ID_GRAB, grabButton);
	DDX_Text(pDX, IDC_CAMNAME, camName);
	DDX_Text(pDX, IDC_FRAMESPERSEC, FrameRateString);
	DDX_Text(pDX, IDC_BUFFERNUM, actualBufferNumber);
	//}}AFX_DATA_MAP
}

//============================================================================
//  Message map
//============================================================================
BEGIN_MESSAGE_MAP(CLLGrabDlg, CDialog)
	//{{AFX_MSG_MAP(CLLGrabDlg)
	ON_BN_CLICKED(ID_GRAB, OnGrab)
	ON_BN_CLICKED(ID_STOP, OnStop)
	ON_BN_CLICKED(ID_QUIT, OnQuit)
    ON_MESSAGE(WM_UPDATEDIALOG, OnUpdateDialog)

	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//////////////////////////////////////////////////////////////////////////////
//
//  CLLGrabDlg::OnInitDialog
//
//  Description:
//      Message handler which is called when it's time to initialize the dialog.
//
//////////////////////////////////////////////////////////////////////////////
BOOL CLLGrabDlg::OnInitDialog()
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
//  CLLGrabDlg::OnGrab
//
//  Description:
//      Message handler which is called when the user single-clicks the "Grab"
//      button.  Here, we'll start the grab.
//
//////////////////////////////////////////////////////////////////////////////
void CLLGrabDlg::OnGrab() 
{
    bool startedSuccessfully = false;
    UpdateData (TRUE);
    FrameRateString = InitialFrameRate;
    UpdateData (FALSE);
    try 
	{
 		//--------------------------------------------------------------------
		//  We need an image for display purposes.  
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
        //  Now it's time to configure and start the grab.  If the acquisition
        //  starts properly, go ahead and start the grab thread.
        //--------------------------------------------------------------------
        status = IMAQdxConfigureAcquisition (session, TRUE, 3);
		if (status)
            throw status;

        status = IMAQdxStartAcquisition (session);
		if (status)
            throw status;

        stopThread = false;
        grabThread = AfxBeginThread (CLLGrabDlg::GrabThreadFunc, this);
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
        if (grabThread)
            StopGrabThread();
        IMAQdxCloseCamera (session);
        session = 0;
    }
}

//////////////////////////////////////////////////////////////////////////////
//
//  CLLGrabDlg::GrabThreadFunc
//
//  Description:
//      This is the main grab worker thread.  Its job is to get images from
//      the driver and display them.
//
//////////////////////////////////////////////////////////////////////////////
UINT CLLGrabDlg::GrabThreadFunc (LPVOID param) 
{
     return reinterpret_cast<CLLGrabDlg*>(param)->GrabThreadFuncInternal();
}

UINT CLLGrabDlg::GrabThreadFuncInternal()
{
	//------------------------------------------------------------------------
    //  Start grabbing images until either the user tells us to stop or we
    //  encounter an error.
    //------------------------------------------------------------------------
    IMAQdxError status = IMAQdxErrorSuccess;
    uInt32 lastBufferNumber = -1;
    DWORD lastTick = GetTickCount();
    while (!stopThread) 
	{
        uInt32 bufferNumber;
		status = IMAQdxGetImage (session, image, IMAQdxBufferNumberModeNext, 0, &bufferNumber);
		if (status)
			break;
        imaqDisplayImage(image, 0, true);

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
    }
    //------------------------------------------------------------------------
    //  Well, it's time to stop.  If we got an error, stop the acquisition.
    //  Afterwards tell the outside world that our thread is done.
    //------------------------------------------------------------------------
    if (status) 
	{
        DisplayNIIMAQdxError(status);
        SetActiveWindow();
        stopButton.PostMessage(BM_CLICK);
    }
    return 0;
}

//////////////////////////////////////////////////////////////////////////////
//
//  CLLGrabDlg::OnStop
//
//  Description:
//      Message handler which is called when the user single-clicks the "Stop"
//      button.  Here, we'll stop the grab.
//
//////////////////////////////////////////////////////////////////////////////
void CLLGrabDlg::OnStop() 
{
    //------------------------------------------------------------------------
    //  Stop the grab thread.  Stop the acquisition and close the session.
    //  Dispose of the image.
    //------------------------------------------------------------------------
    if (grabThread)
        StopGrabThread();
    
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
//  CLLGrabDlg::OnQuit
//
//  Description:
//      Message handler which is called when the user single-clicks the "Quit"
//      button.
//
//////////////////////////////////////////////////////////////////////////////
void CLLGrabDlg::OnQuit() 
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
//  CLLGrabDlg::OnUpdateDialog
//
//  Description:
//      Message handler used to update the dialog with the latest data.
//
//////////////////////////////////////////////////////////////////////////////
LRESULT CLLGrabDlg::OnUpdateDialog(WPARAM, LPARAM) {
    UpdateData(FALSE);
    return 0;
}

//////////////////////////////////////////////////////////////////////////////
//
//  CLLGrabDlg::StopGrabThread
//
//  Description:
//      Stops the grab thread and destroys the object associated with it.
//      You should only call this function if the thread was properly created.
//
//////////////////////////////////////////////////////////////////////////////
void CLLGrabDlg::StopGrabThread() {
    //------------------------------------------------------------------------
    //  Tell the thread to stop and wait for it to do so.  Note that the timeout
    //  should be longer than your frame time to guarantee that the thread
    //  stops by the time we exit this function.
    //------------------------------------------------------------------------
    stopThread = true;
    WaitForSingleObject(grabThread->m_hThread, 5000);
    grabThread = NULL;
}

//////////////////////////////////////////////////////////////////////////////
//
//  CLLGrabDlg::DisplayNIIMAQdxError
//
//  Description:
//      Displays a message box containing the NI-IMAQdx error information.
//
//  Parameters:
//      error - The error code
//
//////////////////////////////////////////////////////////////////////////////
void CLLGrabDlg::DisplayNIIMAQdxError(IMAQdxError error) {
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
//  CLLGrabDlg::DisplayNIVisionError
//
//  Description:
//      Displays a message box containing the NI Vision error information.
//
//  Parameters:
//      error - The error code
//
//////////////////////////////////////////////////////////////////////////////
void CLLGrabDlg::DisplayNIVisionError(int error) {
    //--------------------------------------------------------------------
    //  Display a dialog containing the NI Vision error we encountered.
    //--------------------------------------------------------------------
    char* errorText = imaqGetErrorText(error);
    char dialogText[512];
    sprintf(dialogText, "Error Code = 0x%08X\n\n%s", error, errorText);
    MessageBox(dialogText, "NI Vision Error");
    imaqDispose(errorText);
}
