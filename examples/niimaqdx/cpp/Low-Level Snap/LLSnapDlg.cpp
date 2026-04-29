//////////////////////////////////////////////////////////////////////////////
//
//  Title     : LLSnapDlg.cpp
//  Project   : NI-IMAQdx
//  Created   : 8/29/2006 @ 13:29:21
//  Platforms : All
//  Access    : Public
//  Purpose   : Illustrates how to perform a simple, low-level snap acquisition,
//              built using MSVC.
//
//////////////////////////////////////////////////////////////////////////////


//============================================================================
//  Includes
//============================================================================
#include "stdafx.h"
#include "LLSnap.h"
#include "LLSnapDlg.h"

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
//  CLLSnapDlg::CLLSnapDlg
//
//  Description:
//      Constructs the main CLLSnapDlg.
//
//////////////////////////////////////////////////////////////////////////////
CLLSnapDlg::CLLSnapDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CLLSnapDlg::IDD, pParent),
	image (NULL),
	session (NULL)
{
	//{{AFX_DATA_INIT(CLLSnapDlg)
	camName = _T("cam0");
	//}}AFX_DATA_INIT
	// Note that LoadIcon does not require a subsequent DestroyIcon in Win32
	m_hIcon = AfxGetApp()->LoadIcon(IDR_IMAQDX);
}

//////////////////////////////////////////////////////////////////////////////
//
//  CLLSnapDlg::DoDataExchange
//
//  Description:
//      Exchanges data between our member variables and the dialog.
//
//////////////////////////////////////////////////////////////////////////////
void CLLSnapDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CLLSnapDlg)
	DDX_Text(pDX, IDC_CAMNAME, camName);
	//}}AFX_DATA_MAP
}

//============================================================================
//  Message map
//============================================================================
BEGIN_MESSAGE_MAP(CLLSnapDlg, CDialog)
	//{{AFX_MSG_MAP(CLLSnapDlg)
	ON_BN_CLICKED(ID_SNAP, OnSnap)
	ON_BN_CLICKED(ID_QUIT, OnQuit)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//////////////////////////////////////////////////////////////////////////////
//
//  CLLSnapDlg::OnInitDialog
//
//  Description:
//      Message handler which is called when it's time to initialize the dialog.
//
//////////////////////////////////////////////////////////////////////////////
BOOL CLLSnapDlg::OnInitDialog()
{
    //------------------------------------------------------------------------
    //  Let the base method run & initialize our big & small icons.  Also,
    //  start out with the Stop button disabled and the Start button enabled.
    //------------------------------------------------------------------------
	CDialog::OnInitDialog();
	SetIcon(m_hIcon, TRUE);			// Set big icon
	SetIcon(m_hIcon, FALSE);		// Set small icon
	return TRUE;  // return TRUE  unless you set the focus to a control
}

//////////////////////////////////////////////////////////////////////////////
//
//  CLLSnapDlg::OnSnap
//
//  Description:
//      Message handler which is called when the user single-clicks the "Snap"
//      button.  Here, we'll start the grab.
//
//////////////////////////////////////////////////////////////////////////////
void CLLSnapDlg::OnSnap() 
{
	unsigned long bufferNumber;

	//--------------------------------------------------------------------
	//  Dispose of any image that already exists
	//--------------------------------------------------------------------
	if(image)
		imaqDispose(image);
	
	//-----------------------------------------------------------------------
    //  Get the camera name from the panel
    //-----------------------------------------------------------------------
	UpdateData(TRUE);
	
	try
	{
		//-----------------------------------------------------------------------
		//  Open a session to the camera
		//-----------------------------------------------------------------------
		IMAQdxError status = IMAQdxOpenCamera (camName, IMAQdxCameraControlModeController, &session);
		if (status)
			throw status;
		
		//--------------------------------------------------------------------
		//  Create an image
		//--------------------------------------------------------------------
		image = imaqCreateImage (IMAQ_IMAGE_U8, 0);
		if (!image)
			throw VisionError();

		//-----------------------------------------------------------------------
		//  Now it's time to snap and display an image.  The driver will automatically
		//  allocate memory for the snapped image. 
		//-----------------------------------------------------------------------
		status = IMAQdxConfigureAcquisition (session, FALSE, 1);
		if (status)
			throw status;

        status = IMAQdxStartAcquisition (session);
		if (status)
			throw status;

        status = IMAQdxGetImage (session, image, IMAQdxBufferNumberModeBufferNumber, 0, &bufferNumber);
		if (status)
			throw status;

		//-----------------------------------------------------------------------
		//  Display the image
		//-----------------------------------------------------------------------
		imaqDisplayImage(image, 0, true);
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
    //  Stop and unconfigure the acquisition and close the session
    //-----------------------------------------------------------------------
	IMAQdxStopAcquisition (session);
	IMAQdxUnconfigureAcquisition (session);
	IMAQdxCloseCamera (session);
	session = 0;
}

//////////////////////////////////////////////////////////////////////////////
//
//  CLLSnapDlg::OnQuit
//
//  Description:
//      Message handler which is called when the user single-clicks the "Quit"
//      button.
//
//////////////////////////////////////////////////////////////////////////////
void CLLSnapDlg::OnQuit() 
{
    //------------------------------------------------------------------------
    //  The only cleanup we need to do is to close the display window and
    //  free the image associated with it.
    //------------------------------------------------------------------------
    if (image)
	{
		imaqDispose(image);
		image = NULL;
	}
    imaqCloseWindow(IMAQ_ALL_WINDOWS);
    EndDialog(0);
}
