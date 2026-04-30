//////////////////////////////////////////////////////////////////////////////
//
//  Title     : TriggeredGrabDlg.cpp
//  Project   : NI-IMAQdx
//  Created   : 8/25/2006 @ 07:26:21
//  Platforms : All
//  Access    : Public
//  Purpose   : Illustrates how to perform a simple, high-level grab acquisition,
//              built using MSVC.
//
//////////////////////////////////////////////////////////////////////////////


//============================================================================
//  Includes
//============================================================================
#include "stdafx.h"
#include "TriggeredGrab.h"
#include "TriggeredGrabDlg.h"

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

#define IMAQdxAttributeTriggerMode "TriggerMode"
#define IMAQdxAttributeTriggerActivation "TriggerActivation"


//////////////////////////////////////////////////////////////////////////////
//
//  CTriggeredGrabDlg::CTriggeredGrabDlg
//
//  Description:
//      Constructs the main CTriggeredGrabDlg.
//
//////////////////////////////////////////////////////////////////////////////
CTriggeredGrabDlg::CTriggeredGrabDlg(CWnd* pParent) :
    CDialog(CTriggeredGrabDlg::IDD, pParent),
    session(NULL),
    image(NULL),
    grabThread(NULL),
    stopThread(false),
	triggerMode(NULL),
	triggerActivation(NULL)
{
	//{{AFX_DATA_INIT(CTriggeredGrabDlg)
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
//  CTriggeredGrabDlg::DoDataExchange
//
//  Description:
//      Exchanges data between our member variables and the dialog.
//
//////////////////////////////////////////////////////////////////////////////
void CTriggeredGrabDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CTriggeredGrabDlg)
	DDX_Control(pDX, IDC_TRIGGER_ACTIVATION, triggerActivationControl);
	DDX_Control(pDX, IDC_TRIGGER_MODE, triggerModeControl);
	DDX_Control(pDX, ID_INITIALIZE, initializeButton);
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
BEGIN_MESSAGE_MAP(CTriggeredGrabDlg, CDialog)
	//{{AFX_MSG_MAP(CTriggeredGrabDlg)
	ON_BN_CLICKED(ID_GRAB, OnGrab)
	ON_BN_CLICKED(ID_STOP, OnStop)
	ON_BN_CLICKED(ID_QUIT, OnQuit)
	ON_CBN_SELCHANGE(IDC_TRIGGER_MODE, OnChangeTrigger)
	ON_BN_CLICKED(ID_INITIALIZE, OnInitialize)
	ON_CBN_SELCHANGE(IDC_TRIGGER_ACTIVATION, OnChangeTrigger)
	ON_EN_CHANGE(IDC_CAMNAME, OnChangeCamname)
	ON_MESSAGE(WM_UPDATEDIALOG, OnUpdateDialog)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//////////////////////////////////////////////////////////////////////////////
//
//  CTriggeredGrabDlg::OnInitDialog
//
//  Description:
//      Message handler which is called when it's time to initialize the dialog.
//
//////////////////////////////////////////////////////////////////////////////
BOOL CTriggeredGrabDlg::OnInitDialog()
{
    //------------------------------------------------------------------------
    //  Let the base method run & initialize our big & small icons.  Also,
    //  start out with the Grab and Stop buttons disabled and the Initialize
	//  button enabled.
    //------------------------------------------------------------------------
	CDialog::OnInitDialog();
	SetIcon(m_hIcon, TRUE);			// Set big icon
	SetIcon(m_hIcon, FALSE);		// Set small icon
    camNameControl.EnableWindow(TRUE);
	initializeButton.EnableWindow(TRUE);
    grabButton.EnableWindow(FALSE);
    stopButton.EnableWindow(FALSE);
	triggerModeControl.EnableWindow (FALSE);
	triggerActivationControl.EnableWindow (FALSE);

	return TRUE;  // return TRUE  unless you set the focus to a control
}

//////////////////////////////////////////////////////////////////////////////
//
//  CTriggeredGrabDlg::OnInitialize
//
//  Description:
//      Message handler which is called when the user single-clicks the 
//      "Initialize" button.  Here, we query the values for trigger mode and
//		trigger activation and populate the enums.
//
//////////////////////////////////////////////////////////////////////////////
void CTriggeredGrabDlg::OnInitialize() 
{
    IMAQdxError status = IMAQdxErrorSuccess;
	unsigned long	triggerModeSize = 0; 
	unsigned long	triggerActivationSize = 0;
	unsigned long   i;
	unsigned int 	currentTriggerMode;
	unsigned int 	currentTriggerActivation;
	unsigned int	currentTriggerModeIndex = 0;
	unsigned int	currentTriggerActivationIndex = 0;

	UpdateData (TRUE);
	
	try
	{
		//--------------------------------------------------------------------
        //  Open an interface to our board, as given by the interface name.
        //--------------------------------------------------------------------
		status = IMAQdxOpenCamera(camName,IMAQdxCameraControlModeController,&session);
		if (status)
			throw status;
		
		//--------------------------------------------------------------------
        //  Get the list of trigger modes supported by the camera
        //--------------------------------------------------------------------
		status = IMAQdxGetAttribute (session, IMAQdxAttributeTriggerMode, IMAQdxValueTypeU32, &currentTriggerMode);       
		if (status)
			throw status;
		
		status = IMAQdxEnumerateAttributeValues (session, IMAQdxAttributeTriggerMode, NULL, &triggerModeSize);
		if (status)
			throw status;
		
		if (triggerMode)
		{
			delete[] triggerMode;
			triggerMode = NULL;
		}
		triggerMode = new IMAQdxEnumItem[triggerModeSize];
		status = IMAQdxEnumerateAttributeValues (session, IMAQdxAttributeTriggerMode, triggerMode, &triggerModeSize);
		if (status)
			throw status;

		//--------------------------------------------------------------------
        //  Determine the index for the current trigger mode
        //--------------------------------------------------------------------
		for (i = 0; i < triggerModeSize; i++)
		{
			if (currentTriggerMode == triggerMode[i].Value)
			{
				currentTriggerModeIndex = i;
				break;
			}
		}		

		//--------------------------------------------------------------------
        //  Clear any values, and populate the new values in the combo box
        //--------------------------------------------------------------------
		triggerModeControl.ResetContent();		   
		
		for(i=0; i<triggerModeSize; i++)
		{
			triggerModeControl.AddString (triggerMode[i].Name);
			triggerModeControl.SetItemData (i, triggerMode[i].Value);
		}
		triggerModeControl.SetCurSel (currentTriggerModeIndex);
		
		//--------------------------------------------------------------------
        //  Get the list of trigger activations supported by the camera
        //--------------------------------------------------------------------
		status = IMAQdxGetAttribute (session, IMAQdxAttributeTriggerActivation, IMAQdxValueTypeU32, &currentTriggerActivation);       
		if (status)
			throw status;

		status = IMAQdxEnumerateAttributeValues (session, IMAQdxAttributeTriggerActivation, NULL, &triggerActivationSize);
		if (status)
			throw status;

		if (triggerActivation)
		{
			delete[] triggerActivation;
			triggerActivation = NULL;
		}
		triggerActivation = new IMAQdxEnumItem[triggerActivationSize];
		status = IMAQdxEnumerateAttributeValues (session, IMAQdxAttributeTriggerActivation, triggerActivation, &triggerActivationSize);
		if (status)
			throw status;

		//--------------------------------------------------------------------
        //  Determine the index for the current trigger activation
        //--------------------------------------------------------------------
		for (i = 0; i < triggerActivationSize; i++)
		{
			if (currentTriggerActivation == triggerActivation[i].Value)
			{
				currentTriggerActivationIndex = i;
				break;
			}
		}		

		//--------------------------------------------------------------------
        //  Clear any values, and populate the new values in the combo box
        //--------------------------------------------------------------------
		triggerActivationControl.ResetContent();		   
		
		for(i=0; i<triggerActivationSize; i++)
		{
			triggerActivationControl.AddString (triggerActivation[i].Name);
			triggerActivationControl.SetItemData (i, triggerActivation[i].Value);
		}
		triggerActivationControl.SetCurSel (currentTriggerActivation);

		
		//--------------------------------------------------------------------
        //  Disable the Initialize button.  Enable the Grab button.
        //--------------------------------------------------------------------
		initializeButton.EnableWindow(FALSE);
		grabButton.EnableWindow(TRUE);
		stopButton.EnableWindow(FALSE);
		triggerModeControl.EnableWindow (TRUE);
		triggerActivationControl.EnableWindow (TRUE);

	}
    catch (IMAQdxError error) {
        DisplayNIIMAQdxError (error);
    }
    catch (VisionError) {
        DisplayNIVisionError (imaqGetLastError());
    }
}

//////////////////////////////////////////////////////////////////////////////
//
//  CTriggeredGrabDlg::OnGrab
//
//  Description:
//      Message handler which is called when the user single-clicks the "Grab"
//      button.  Here, we'll start the grab.
//
//////////////////////////////////////////////////////////////////////////////
void CTriggeredGrabDlg::OnGrab() 
{
    IMAQdxError status = IMAQdxErrorSuccess;
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
        //  Now it's time to start the grab.  If the acquisition starts
        //  properly, go ahead and start the grab thread.
        //--------------------------------------------------------------------
        status = IMAQdxConfigureGrab (session);
        if (status)
            throw status;
        stopThread = false;
        grabThread = AfxBeginThread (CTriggeredGrabDlg::GrabThreadFunc, this);
        camNameControl.EnableWindow (FALSE);
        grabButton.EnableWindow (FALSE);
        stopButton.EnableWindow (TRUE);
		triggerModeControl.EnableWindow (FALSE);
		triggerActivationControl.EnableWindow (FALSE);
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
//  CTriggeredGrabDlg::GrabThreadFunc
//
//  Description:
//      This is the main grab worker thread.  Its job is to get images from
//      the driver and display them.
//
//////////////////////////////////////////////////////////////////////////////
UINT CTriggeredGrabDlg::GrabThreadFunc (LPVOID param) 
{
     return reinterpret_cast<CTriggeredGrabDlg*>(param)->GrabThreadFuncInternal ();
}

UINT CTriggeredGrabDlg::GrabThreadFuncInternal ()
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
//  CTriggeredGrabDlg::OnChangeTrigger
//
//  Description:
//      Message handler which is called when the user changes the Trigger Mode
//      or Trigger Activation.  Here, we unconfigure and reconfigure the
//		acquisition.
//
//////////////////////////////////////////////////////////////////////////////
void CTriggeredGrabDlg::OnChangeTrigger() 
{
    IMAQdxError status = IMAQdxErrorSuccess;
	unsigned int newTriggerMode;
	unsigned int newTriggerActivation;
	unsigned int newTriggerModeIndex;
	unsigned int newTriggerActivationIndex;

	try
	{
		//------------------------------------------------------------------------
		//  Unconfigure the acquisition
		//------------------------------------------------------------------------
		status = IMAQdxUnconfigureAcquisition (session);
		if (status)
			throw status;

		//------------------------------------------------------------------------
		//  Read the new values for trigger mode and activation
		//------------------------------------------------------------------------
		newTriggerModeIndex = triggerModeControl.GetCurSel();
		newTriggerMode = uInt32(triggerModeControl.GetItemData(newTriggerModeIndex));
		newTriggerActivationIndex = triggerActivationControl.GetCurSel();
		newTriggerActivation = uInt32(triggerActivationControl.GetItemData(newTriggerActivationIndex));
		
		//------------------------------------------------------------------------
		//  Set the trigger mode and activation in the driver
		//------------------------------------------------------------------------
		status = IMAQdxSetAttribute (session, IMAQdxAttributeTriggerMode, IMAQdxValueTypeU32, newTriggerMode);
		if (status)
			throw status;
		
		status = IMAQdxSetAttribute (session, IMAQdxAttributeTriggerActivation, IMAQdxValueTypeU32, newTriggerActivation);
		if (status)
			throw status;

		//------------------------------------------------------------------------
		//  Reconfigure the acquisition
		//------------------------------------------------------------------------
		IMAQdxConfigureGrab (session);
	}

	catch (IMAQdxError error) {
        DisplayNIIMAQdxError (error);
    }
    catch (VisionError) {
        DisplayNIVisionError (imaqGetLastError());
    }
}

//////////////////////////////////////////////////////////////////////////////
//
//  CTriggeredGrabDlg::OnChangeCamName
//
//  Description:
//      Enables/disables button status when the camera name changes.
//
//////////////////////////////////////////////////////////////////////////////
void CTriggeredGrabDlg::OnChangeCamname() 
{
	//--------------------------------------------------------------------
    //  If the camera name changes, enable the initialize button and 
	//	disable the grab and stop buttons.
    //--------------------------------------------------------------------
    camNameControl.EnableWindow(TRUE);
	initializeButton.EnableWindow(TRUE);
    grabButton.EnableWindow(FALSE);
    stopButton.EnableWindow(FALSE);

	//--------------------------------------------------------------------
    //  Close the previous session
    //--------------------------------------------------------------------
    if (session) 
	{
		IMAQdxCloseCamera (session);
        session = 0;
    }
}

//////////////////////////////////////////////////////////////////////////////
//
//  CTriggeredGrabDlg::OnStop
//
//  Description:
//      Message handler which is called when the user single-clicks the "Stop"
//      button.  Here, we'll stop the grab.
//
//////////////////////////////////////////////////////////////////////////////
void CTriggeredGrabDlg::OnStop() 
{
    //------------------------------------------------------------------------
    //  Stop the grab thread.  Stop the acquisition and close the session.
    //  Dispose of the image.
    //------------------------------------------------------------------------
    if (grabThread)
        StopGrabThread();

	if (image)
	{
	    imaqDispose (image);
		image = NULL;
	}

	camNameControl.EnableWindow(TRUE);
    grabButton.EnableWindow(TRUE);
    stopButton.EnableWindow(FALSE);
	triggerModeControl.EnableWindow (TRUE);
	triggerActivationControl.EnableWindow (TRUE);

}

//////////////////////////////////////////////////////////////////////////////
//
//  CTriggeredGrabDlg::OnQuit
//
//  Description:
//      Message handler which is called when the user single-clicks the "Quit"
//      button.
//
//////////////////////////////////////////////////////////////////////////////
void CTriggeredGrabDlg::OnQuit() 
{
    //------------------------------------------------------------------------
    //  Stop the acquisition and clean up the image display.
    //------------------------------------------------------------------------
    OnStop();
    imaqShowWindow(0, false);
	OnCancel();

	if (triggerMode)
	{
		delete[] triggerMode;
		triggerMode = NULL;
	}
	if (triggerActivation)
	{
		delete[] triggerActivation;
		triggerActivation = NULL;
	}
}

//////////////////////////////////////////////////////////////////////////////
//
//  CTriggeredGrabDlg::OnUpdateDialog
//
//  Description:
//      Message handler used to update the dialog with the latest data.
//
//////////////////////////////////////////////////////////////////////////////
LRESULT CTriggeredGrabDlg::OnUpdateDialog(WPARAM, LPARAM) 
{
    UpdateData(FALSE);
    return 0;
}

//////////////////////////////////////////////////////////////////////////////
//
//  CTriggeredGrabDlg::StopGrabThread
//
//  Description:
//      Stops the grab thread and destroys the object associated with it.
//      You should only call this function if the thread was properly created.
//
//////////////////////////////////////////////////////////////////////////////
void CTriggeredGrabDlg::StopGrabThread() 
{
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
//  CTriggeredGrabDlg::DisplayNIIMAQdxError
//
//  Description:
//      Displays a message box containing the NI-IMAQdx error information.
//
//  Parameters:
//      error - The error code
//
//////////////////////////////////////////////////////////////////////////////
void CTriggeredGrabDlg::DisplayNIIMAQdxError(IMAQdxError error) 
{
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
//  CTriggeredGrabDlg::DisplayNIVisionError
//
//  Description:
//      Displays a message box containing the NI Vision error information.
//
//  Parameters:
//      error - The error code
//
//////////////////////////////////////////////////////////////////////////////
void CTriggeredGrabDlg::DisplayNIVisionError(int error) 
{
    //--------------------------------------------------------------------
    //  Display a dialog containing the NI Vision error we encountered.
    //--------------------------------------------------------------------
    char* errorText = imaqGetErrorText(error);
    char dialogText[512];
    sprintf(dialogText, "Error Code = 0x%08X\n\n%s", error, errorText);
    MessageBox(dialogText, "NI Vision Error");
    imaqDispose(errorText);
}




