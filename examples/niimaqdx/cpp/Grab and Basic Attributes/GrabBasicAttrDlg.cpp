//////////////////////////////////////////////////////////////////////////////
//
//  Title     : GrabBasicAttrDlg.cpp
//  Project   : NI-IMAQdx
//  Created   : 2/12/2008 @ 10:19:26
//  Platforms : All
//  Access    : Public
//  Purpose   : Illustrates how to perform a simple, high-level grab acquisition,
//              with basic attribute support built using MSVC.
//
//////////////////////////////////////////////////////////////////////////////


//============================================================================
//  Includes
//============================================================================
#include "stdafx.h"
#include "GrabBasicAttr.h"
#include "GrabBasicAttrDlg.h"

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
const char* InitialBufferNumber = "0";
const char* DefaultAttributeRoot = "CameraAttributes";
const IMAQdxAttributeVisibility DefaultAttributeVisibility = IMAQdxAttributeVisibilitySimple;


//////////////////////////////////////////////////////////////////////////////
//
//  CGrabBasicAttrDlg::CGrabBasicAttrDlg
//
//  Description:
//      Constructs the main CGrabBasicAttrDlg.
//
//////////////////////////////////////////////////////////////////////////////
CGrabBasicAttrDlg::CGrabBasicAttrDlg(CWnd* pParent) :
    CDialog(CGrabBasicAttrDlg::IDD, pParent),
    session(0),
    image(NULL),
    grabThread(NULL),
    stopThread(false)
{
	//{{AFX_DATA_INIT(CGrabBasicAttrDlg)
    camName = _T("cam0");
	//}}AFX_DATA_INIT
	// Note that LoadIcon does not require a subsequent DestroyIcon in Win32
	m_hIcon = AfxGetApp()->LoadIcon(IDR_IMAQDX);
    imaqSetWindowThreadPolicy(IMAQ_SEPARATE_THREAD);

}

//////////////////////////////////////////////////////////////////////////////
//
//  CGrabBasicAttrDlg::DoDataExchange
//
//  Description:
//      Exchanges data between our member variables and the dialog.
//
//////////////////////////////////////////////////////////////////////////////
void CGrabBasicAttrDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CGrabBasicAttrDlg)
	DDX_Control(pDX, ID_SET, setButton);
	DDX_Control(pDX, IDC_FRAMESPERSEC, frameRateControl);
	DDX_Control(pDX, IDC_BUFFERNUM, bufferNumberControl);
	DDX_Control(pDX, IDC_ATTRIBUTEVALUE, attributeValueControl);
	DDX_Control(pDX, IDC_ATTRIBUTENAME, attributeNameControl);
	DDX_Control(pDX, ID_STOP, stopButton);
	DDX_Control(pDX, ID_GRAB, grabButton);
	DDX_Control(pDX, IDC_CAMNAME, camNameControl);
    DDX_Text(pDX, IDC_CAMNAME, camName);
	//}}AFX_DATA_MAP
}

//============================================================================
//  Message map
//============================================================================
BEGIN_MESSAGE_MAP(CGrabBasicAttrDlg, CDialog)
	//{{AFX_MSG_MAP(CGrabBasicAttrDlg)
	ON_BN_CLICKED(ID_QUIT, OnQuit)
	ON_BN_CLICKED(ID_GRAB, OnGrab)
	ON_BN_CLICKED(ID_STOP, OnStop)
	ON_CBN_SELCHANGE(IDC_ATTRIBUTENAME, OnChangeAttributeName)
	ON_BN_CLICKED(ID_SET, OnSetAttributeValue)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//////////////////////////////////////////////////////////////////////////////
//
//  CGrabBasicAttrDlg::OnInitDialog
//
//  Description:
//      Message handler which is called when it's time to initialize the dialog.
//
//////////////////////////////////////////////////////////////////////////////
BOOL CGrabBasicAttrDlg::OnInitDialog()
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
    frameRateControl.EnableWindow(FALSE);
    frameRateControl.SetWindowText(InitialFrameRate);
    bufferNumberControl.EnableWindow(FALSE);
    bufferNumberControl.SetWindowText(InitialBufferNumber);
    attributeNameControl.EnableWindow(FALSE);
    attributeValueControl.EnableWindow(FALSE);
    setButton.EnableWindow(FALSE);
	return TRUE;  // return TRUE  unless you set the focus to a control
}

//////////////////////////////////////////////////////////////////////////////
//
//  CGrabBasicAttrDlg::OnGrab
//
//  Description:
//      Message handler which is called when the user single-clicks the "Grab"
//      button.  Here, we'll start the GrabBasicAttr.
//
//////////////////////////////////////////////////////////////////////////////
void CGrabBasicAttrDlg::OnGrab() 
{
    UpdateData (TRUE);
    bool startedSuccessfully = false;
    frameRateControl.SetWindowText(InitialFrameRate);
    bufferNumberControl.SetWindowText(InitialBufferNumber);

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
        camNameControl.GetWindowText(camName);
        IMAQdxError status = IMAQdxOpenCamera (camName, IMAQdxCameraControlModeController, &session);
        if (status)
            throw status;

        //--------------------------------------------------------------------
        //  Now it's time to start the GrabBasicAttr.  If the acquisition starts
        //  properly, go ahead and start the grab thread.
        //--------------------------------------------------------------------
        status = IMAQdxConfigureGrab (session);
        if (status)
            throw status;

        //--------------------------------------------------------------------
        //  Format attribute name control.
        //--------------------------------------------------------------------
        status = FormatAttributeNameControl();
        if (status)
            throw status;

        
        stopThread = false;
        grabThread = AfxBeginThread (CGrabBasicAttrDlg::GrabThreadFunc, this);
        camNameControl.EnableWindow (FALSE);
        grabButton.EnableWindow (FALSE);
        stopButton.EnableWindow (TRUE);
        attributeNameControl.EnableWindow (TRUE);
        attributeValueControl.EnableWindow (TRUE);
        setButton.EnableWindow (FALSE);
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
//  CGrabBasicAttrDlg::GrabThreadFunc
//
//  Description:
//      This is the main grab worker thread.  Its job is to get images from
//      the driver and display them.
//
//////////////////////////////////////////////////////////////////////////////
UINT CGrabBasicAttrDlg::GrabThreadFunc (LPVOID param) 
{
     return reinterpret_cast<CGrabBasicAttrDlg*>(param)->GrabThreadFuncInternal ();
}

UINT CGrabBasicAttrDlg::GrabThreadFuncInternal ()
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
        status = IMAQdxGrab (session, image, TRUE, &bufferNumber);
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
            CString frameRateString, bufferNumberString;
            double frameRate = ((double)(bufferNumber - lastBufferNumber) / (double)(newTick - lastTick)) * 1000.0;
            frameRateString.Format("%.6f", frameRate);
            lastTick = newTick;
            lastBufferNumber = bufferNumber;
            bufferNumberString.Format("%u", bufferNumber);
            frameRateControl.SetWindowText(frameRateString);
            bufferNumberControl.SetWindowText(bufferNumberString);
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
//  CGrabBasicAttrDlg::OnStop
//
//  Description:
//      Message handler which is called when the user single-clicks the "Stop"
//      button.  Here, we'll stop the GrabBasicAttr.
//
//////////////////////////////////////////////////////////////////////////////
void CGrabBasicAttrDlg::OnStop() 
{
    //------------------------------------------------------------------------
    //  Stop the grab thread.  Stop the acquisition and close the session.
    //  Dispose of the image.
    //------------------------------------------------------------------------
    if (grabThread)
        StopGrabThread();
    if (session) 
	{
		IMAQdxCloseCamera (session);
        session = 0;
    }
    
	if (image)
	{
	    imaqDispose (image);
		image = NULL;
	}

	camNameControl.EnableWindow(TRUE);
    grabButton.EnableWindow(TRUE);
    stopButton.EnableWindow(FALSE);
    attributeNameControl.EnableWindow(FALSE);
    attributeValueControl.EnableWindow(FALSE);
    setButton.EnableWindow(FALSE);
}

//////////////////////////////////////////////////////////////////////////////
//
//  CGrabBasicAttrDlg::OnQuit
//
//  Description:
//      Message handler which is called when the user single-clicks the "Quit"
//      button.
//
//////////////////////////////////////////////////////////////////////////////
void CGrabBasicAttrDlg::OnQuit() 
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
//  CGrabBasicAttrDlg::StopGrabThread
//
//  Description:
//      Stops the grab thread and destroys the object associated with it.
//      You should only call this function if the thread was properly created.
//
//////////////////////////////////////////////////////////////////////////////
void CGrabBasicAttrDlg::StopGrabThread() {
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
//  CGrabBasicAttrDlg::DisplayNIIMAQdxError
//
//  Description:
//      Displays a message box containing the NI-IMAQdx error information.
//
//  Parameters:
//      error - The error code
//
//////////////////////////////////////////////////////////////////////////////
void CGrabBasicAttrDlg::DisplayNIIMAQdxError(IMAQdxError error) {
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
//  CGrabBasicAttrDlg::DisplayNIVisionError
//
//  Description:
//      Displays a message box containing the NI Vision error information.
//
//  Parameters:
//      error - The error code
//
//////////////////////////////////////////////////////////////////////////////
void CGrabBasicAttrDlg::DisplayNIVisionError(int error) {
    //--------------------------------------------------------------------
    //  Display a dialog containing the NI Vision error we encountered.
    //--------------------------------------------------------------------
    char* errorText = imaqGetErrorText(error);
    char dialogText[512];
    sprintf(dialogText, "Error Code = 0x%08X\n\n%s", error, errorText);
    MessageBox(dialogText, "NI Vision Error");
    imaqDispose(errorText);
}


//////////////////////////////////////////////////////////////////////////////
//
//  CGrabBasicAttrDlg::FormatAttributeNameControl
//
//  Description:
//      Format attribute name control with enumerated attributes
//
//  Return Value:
//      Error code
//
//////////////////////////////////////////////////////////////////////////////
IMAQdxError CGrabBasicAttrDlg::FormatAttributeNameControl() {
    IMAQdxError status = IMAQdxErrorSuccess;
    //--------------------------------------------------------------------
    //  Clear current combo box
    //--------------------------------------------------------------------
    attributeNameControl.ResetContent();
    //--------------------------------------------------------------------
    //  Enumerate available camera attributes
    //--------------------------------------------------------------------
    uInt32 attributeCount = 0;
    IMAQdxAttributeInformation* attributeArray = NULL;
    status = IMAQdxEnumerateAttributes2(session, NULL, &attributeCount, DefaultAttributeRoot, DefaultAttributeVisibility);
    if (status)
        return status;
    attributeArray = new IMAQdxAttributeInformation[attributeCount];
    if (!attributeArray)
        return IMAQdxErrorSystemMemoryFull;
    status = IMAQdxEnumerateAttributes2(session, attributeArray, &attributeCount, DefaultAttributeRoot, DefaultAttributeVisibility);
    if (status)
        return status;
    //--------------------------------------------------------------------
    //  Add camera attribute names to combo box
    //--------------------------------------------------------------------
    for (uInt32 i = 0; i < attributeCount; ++i) {
        attributeNameControl.AddString(attributeArray[i].Name);
    }
    //--------------------------------------------------------------------
    //  Select the first item
    //--------------------------------------------------------------------
    if (attributeCount) {
        attributeNameControl.SetCurSel(0);
        OnChangeAttributeName();
    }
    //--------------------------------------------------------------------
    //  Delete attribute array
    //--------------------------------------------------------------------
    delete [] attributeArray;
    return status;
}


//////////////////////////////////////////////////////////////////////////////
//
//  CGrabBasicAttrDlg::OnChangeAttributeName
//
//  Description:
//      Event handler for selecting a new attribute name.
//      Query attribute value and update corresponding control.
//
//////////////////////////////////////////////////////////////////////////////
void CGrabBasicAttrDlg::OnChangeAttributeName() 
{
    CString attributeName;
    attributeNameControl.GetWindowText(attributeName);
    //--------------------------------------------------------------------
    //  Query read/write access
    //--------------------------------------------------------------------
    bool32 readable = false, writable = false;
    IMAQdxIsAttributeReadable(session, attributeName, &readable);
    IMAQdxIsAttributeWritable(session, attributeName, &writable);
    //--------------------------------------------------------------------
    //  Query current value
    //--------------------------------------------------------------------
    char buffer[IMAQDX_MAX_API_STRING_LENGTH] = { 0 };
    if (readable) {
        IMAQdxGetAttribute(session, attributeName, IMAQdxValueTypeString, buffer);
    }
    //--------------------------------------------------------------------
    //  Update attribute value control
    //--------------------------------------------------------------------
    attributeValueControl.EnableWindow(readable && writable);
    attributeValueControl.SetWindowText(buffer);
    setButton.EnableWindow(writable);
}


//////////////////////////////////////////////////////////////////////////////
//
//  CGrabBasicAttrDlg::OnSetAttributeValue
//
//  Description:
///     Event handler for changing attribute value.
//      
//////////////////////////////////////////////////////////////////////////////
void CGrabBasicAttrDlg::OnSetAttributeValue() 
{
    CString attributeName, attributeValue;
    attributeNameControl.GetWindowText(attributeName);
    attributeValueControl.GetWindowText(attributeValue);
    //--------------------------------------------------------------------
    //  Set current value
    //--------------------------------------------------------------------
    IMAQdxError status = IMAQdxSetAttribute(session, attributeName, IMAQdxValueTypeString, (const char*)attributeValue);
    if (status) {
        DisplayNIIMAQdxError(status);
    }
    //--------------------------------------------------------------------
    //  Query coerced value
    //--------------------------------------------------------------------
    char buffer[IMAQDX_MAX_API_STRING_LENGTH] = { 0 };
    IMAQdxGetAttribute(session, attributeName, IMAQdxValueTypeString, buffer);
    attributeValueControl.SetWindowText(buffer);
}
