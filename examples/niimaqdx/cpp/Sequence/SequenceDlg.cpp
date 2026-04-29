//////////////////////////////////////////////////////////////////////////////
//
//  Title     : SequenceDlg.cpp
//  Project   : NI-IMAQdx
//  Created   : 8/24/2006 @ 22:27:48
//  Platforms : All
//  Access    : Public
//  Purpose   : Demonstrates a simple, high-level acquisition of a sequence 
//				of images, built using MSVC.
//
//////////////////////////////////////////////////////////////////////////////

//============================================================================
//  Includes
//============================================================================
#include "stdafx.h"
#include "Sequence.h"
#include "SequenceDlg.h"

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



//////////////////////////////////////////////////////////////////////////////
//
//  CSequenceDlg::CSequenceDlg
//
//  Description:
//      Constructor for the main dialog.
//
//////////////////////////////////////////////////////////////////////////////
CSequenceDlg::CSequenceDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CSequenceDlg::IDD, pParent),
	imageArray (NULL),
	session (NULL)
{
	//{{AFX_DATA_INIT(CSequenceDlg)
	camName = _T("cam0");
	ImageToDisplay = 0;
	imageArraySize = 5;
	//}}AFX_DATA_INIT
	// Note that LoadIcon does not require a subsequent DestroyIcon in Win32
	m_hIcon = AfxGetApp()->LoadIcon(IDR_IMAQDX);
}

//////////////////////////////////////////////////////////////////////////////
//
//  CSequenceDlg::DoDataExchange
//
//  Description:
//      Exchanges data between dialog and our member variables.
//
//////////////////////////////////////////////////////////////////////////////
void CSequenceDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CSequenceDlg)
	DDX_Control(pDX, IDC_DISPLAYED_IMAGE, DisplayImageCtrl);
	DDX_Text(pDX, IDC_CAMNAME, camName);
	DDX_Slider(pDX, IDC_DISPLAYED_IMAGE, ImageToDisplay);
	DDX_Text(pDX, IDC_NB_IMAGES, imageArraySize);
	//}}AFX_DATA_MAP
}

//============================================================================
//  The message map
//============================================================================
BEGIN_MESSAGE_MAP(CSequenceDlg, CDialog)
	//{{AFX_MSG_MAP(CSequenceDlg)
	ON_BN_CLICKED(ID_QUIT, OnQuit)
	ON_BN_CLICKED(ID_START, OnStart)
	ON_BN_CLICKED(ID_QUIT2, OnQuit)
	ON_NOTIFY(NM_RELEASEDCAPTURE, IDC_DISPLAYED_IMAGE, OnDisplayedImage)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//////////////////////////////////////////////////////////////////////////////
//
//  CSequenceDlg::OnInitDialog
//
//  Description:
//      Called when it's time to initialize the dialog.
//
//////////////////////////////////////////////////////////////////////////////
BOOL CSequenceDlg::OnInitDialog()
{
    //------------------------------------------------------------------------
    //  First let the base method run.  Then, set big & small icons for this
    //  dialog.
    //------------------------------------------------------------------------
	CDialog::OnInitDialog();
	SetIcon(m_hIcon, TRUE);			// Set big icon
	SetIcon(m_hIcon, FALSE);		// Set small icon
	return TRUE;  // return TRUE  unless you set the focus to a control
}

//////////////////////////////////////////////////////////////////////////////
//
//  CSequenceDlg::OnStart
//
//  Description:
//      Event handler method which is called in reponse to the user single-
//      clicking on the Start button.
//
//////////////////////////////////////////////////////////////////////////////
void CSequenceDlg::OnStart() 
{
	//-----------------------------------------------------------------------
	//  Create an array of images for the sequence acquisition
	//-----------------------------------------------------------------------
	if(imageArray)
	{
		for (int i=0; i<imageArraySize; i++)
			imaqDispose (imageArray[i]);
				
		delete [] imageArray;
	}
			
	//-----------------------------------------------------------------------
    //  Get the camera name from the panel
    //-----------------------------------------------------------------------
	UpdateData(TRUE);

	try
	{
		imageArray = new Image*[imageArraySize];
		for (int i=0; i<imageArraySize; i++)
		{	
			imageArray[i] = imaqCreateImage (IMAQ_IMAGE_U8, 0);
			if (!imageArray)
				throw VisionError();
		}
				
		//-----------------------------------------------------------------------
		//  Set range for the Displayed Image slider
		//-----------------------------------------------------------------------
		DisplayImageCtrl.SetRange (0, imageArraySize-1, TRUE);
				
		//-----------------------------------------------------------------------
		//  Open a session to the camera
		//-----------------------------------------------------------------------
		IMAQdxError status = IMAQdxOpenCamera (camName, IMAQdxCameraControlModeController, &session);
		if (status)
			throw status;
				
		//-----------------------------------------------------------------------
		//  Configure and start the acquisition
		//-----------------------------------------------------------------------
		status = IMAQdxSequence (session, imageArray, imageArraySize);
		if (status)
			throw status;

		//-----------------------------------------------------------------------
		//  Display the image
		//-----------------------------------------------------------------------
		imaqDisplayImage (imageArray[0], 0, TRUE);
	}

	catch (IMAQdxError error) 
	{
		//--------------------------------------------------------------------
		//  Display a dialog containing the NI-IMAQdx error we encountered.
		//--------------------------------------------------------------------
		Int8 errorText[512];
		sprintf(errorText, "Error Code = 0x%08X\n\n", error);
		IMAQdxGetErrorString (error, errorText + strlen(errorText), uInt32(sizeof(errorText) - strlen(errorText)));
		MessageBox (errorText, "NI-IMAQdx Error");
	}

	catch (VisionError) 
	{
		//--------------------------------------------------------------------
		//  Display a dialog containing the NI Vision error we encountered.
		//--------------------------------------------------------------------
		int error = imaqGetLastError();
		char* errorText = imaqGetErrorText(error);
		char dialogText[512];
		sprintf (dialogText, "Error Code = 0x%08X\n\n%s", error, errorText);
		MessageBox (dialogText, "NI Vision Error");
		imaqDispose (errorText);
	}


	//-----------------------------------------------------------------------
	//  Close the session to the camera
	//-----------------------------------------------------------------------
	IMAQdxCloseCamera (session);
	session = 0;


}

//////////////////////////////////////////////////////////////////////////////
//
//  CSequenceDlg::OnQuit
//
//  Description:
//      Message handler which is called when the user single-clicks the "Quit"
//      button.
//
//////////////////////////////////////////////////////////////////////////////
void CSequenceDlg::OnQuit() 
{
    //------------------------------------------------------------------------
    //  The only cleanup we need to do is to close the display window and
    //  free the image associated with it.
    //------------------------------------------------------------------------
	if(imageArray)
	{
		for (int i=0; i<imageArraySize; i++)
			imaqDispose (imageArray[i]);
				
		delete [] imageArray;
	}
    imaqCloseWindow(IMAQ_ALL_WINDOWS);
    EndDialog(0);
	
}


//////////////////////////////////////////////////////////////////////////////
//
//  CSequenceDlg::OnDisplayedImage
//
//  Description:
//      Message handler which is called when the user changes the value of the
//		Image to Display slider.
//
//////////////////////////////////////////////////////////////////////////////
void CSequenceDlg::OnDisplayedImage(NMHDR* pNMHDR, LRESULT* pResult) 
{
	//-----------------------------------------------------------------------
    //  Display image selected by Displayed Image control using NI Vision
    //-----------------------------------------------------------------------
	UpdateData (TRUE);
	imaqDisplayImage (imageArray[ImageToDisplay], 0, TRUE);

	*pResult = 0;
}
