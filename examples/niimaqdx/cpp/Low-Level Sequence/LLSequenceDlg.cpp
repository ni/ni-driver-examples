//////////////////////////////////////////////////////////////////////////////
//
//  Title     : LLSequenceDlg.cpp
//  Project   : NI-IMAQdx
//  Created   : 8/29/2006 @ 14:38:48
//  Platforms : All
//  Access    : Public
//  Purpose   : Demonstrates a simple, low-level acquisition of a sequence 
//				of images, built using MSVC.
//
//////////////////////////////////////////////////////////////////////////////

//============================================================================
//  Includes
//============================================================================
#include "stdafx.h"
#include "LLSequence.h"
#include "LLSequenceDlg.h"

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
//  CLLSequenceDlg::CLLSequenceDlg
//
//  Description:
//      Constructor for the main dialog.
//
//////////////////////////////////////////////////////////////////////////////
CLLSequenceDlg::CLLSequenceDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CLLSequenceDlg::IDD, pParent),
	imageArray (NULL),
	session (NULL)
{
	//{{AFX_DATA_INIT(CLLSequenceDlg)
	camName = _T("cam0");
	ImageToDisplay = 0;
	imageArraySize = 5;
	//}}AFX_DATA_INIT
	// Note that LoadIcon does not require a subsequent DestroyIcon in Win32
	m_hIcon = AfxGetApp()->LoadIcon(IDR_IMAQDX);
}

//////////////////////////////////////////////////////////////////////////////
//
//  CLLSequenceDlg::DoDataExchange
//
//  Description:
//      Exchanges data between dialog and our member variables.
//
//////////////////////////////////////////////////////////////////////////////
void CLLSequenceDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CLLSequenceDlg)
	DDX_Control(pDX, IDC_DISPLAYED_IMAGE, DisplayImageCtrl);
	DDX_Text(pDX, IDC_CAMNAME, camName);
	DDX_Slider(pDX, IDC_DISPLAYED_IMAGE, ImageToDisplay);
	DDX_Text(pDX, IDC_NB_IMAGES, imageArraySize);
	//}}AFX_DATA_MAP
}

//============================================================================
//  The message map
//============================================================================
BEGIN_MESSAGE_MAP(CLLSequenceDlg, CDialog)
	//{{AFX_MSG_MAP(CLLSequenceDlg)
	ON_BN_CLICKED(ID_START, OnStart)
	ON_BN_CLICKED(ID_QUIT, OnQuit)
	ON_NOTIFY(NM_RELEASEDCAPTURE, IDC_DISPLAYED_IMAGE, OnDisplayedImage)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//////////////////////////////////////////////////////////////////////////////
//
//  CLLSequenceDlg::OnInitDialog
//
//  Description:
//      Called when it's time to initialize the dialog.
//
//////////////////////////////////////////////////////////////////////////////
BOOL CLLSequenceDlg::OnInitDialog()
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
//  CLLSequenceDlg::OnStart
//
//  Description:
//      Event handler method which is called in reponse to the user single-
//      clicking on the Start button.
//
//////////////////////////////////////////////////////////////////////////////
void CLLSequenceDlg::OnStart() 
{
	unsigned long	bufferNumber = 0;
	int	i;
	
	//-----------------------------------------------------------------------
	//  Dispose of any images that were previously created
	//-----------------------------------------------------------------------
	if(imageArray)
	{
		for (i=0; i<imageArraySize; i++)
			imaqDispose (imageArray[i]);
				
		delete [] imageArray;
		imageArray = NULL;

	}
			
	//-----------------------------------------------------------------------
    //  Get the camera name from the panel
    //-----------------------------------------------------------------------
	UpdateData(TRUE);

	try
	{
		//-----------------------------------------------------------------------
		//  Create an array of images for the sequence acquisition
		//-----------------------------------------------------------------------
		imageArray = new Image*[imageArraySize];

		for (i=0; i<imageArraySize; i++)
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
		status = IMAQdxConfigureAcquisition (session, 0, imageArraySize);
		if (status)
			throw status;

        status = IMAQdxStartAcquisition (session);
		if (status)
			throw status;

		for(i=0; i<imageArraySize; i++)
		{
		    //----------------------------------------------------------------
			//  Get the next frame.
			//----------------------------------------------------------------
			status = IMAQdxGetImage (session, imageArray[i], IMAQdxBufferNumberModeBufferNumber, i, &bufferNumber);
		}
		
		if (!status)
			imaqDisplayImage (imageArray[0], 0, TRUE);
		else
			throw status;
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
	IMAQdxStopAcquisition (session);
	IMAQdxUnconfigureAcquisition (session);
	IMAQdxCloseCamera (session);
	session = 0;

}

//////////////////////////////////////////////////////////////////////////////
//
//  CLLSequenceDlg::OnQuit
//
//  Description:
//      Message handler which is called when the user single-clicks the "Quit"
//      button.
//
//////////////////////////////////////////////////////////////////////////////
void CLLSequenceDlg::OnQuit() 
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
		imageArray = NULL;
	}
    imaqCloseWindow(IMAQ_ALL_WINDOWS);
    EndDialog(0);
}

//////////////////////////////////////////////////////////////////////////////
//
//  CLLSequenceDlg::OnDisplayedImage
//
//  Description:
//      Message handler which is called when the user changes the value of the
//		Image to Display slider.
//
//////////////////////////////////////////////////////////////////////////////
void CLLSequenceDlg::OnDisplayedImage(NMHDR* pNMHDR, LRESULT* pResult) 
{
	//-----------------------------------------------------------------------
    //  Display image selected by Displayed Image control using NI Vision
    //-----------------------------------------------------------------------
	UpdateData (TRUE);
	imaqDisplayImage (imageArray[ImageToDisplay], 0, TRUE);

	*pResult = 0;
}
