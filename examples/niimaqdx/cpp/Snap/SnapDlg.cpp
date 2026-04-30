//////////////////////////////////////////////////////////////////////////////
//
//  Title     : SnapDlg.cpp
//  Project   : NI-IMAQdx
//  Created   : 8/24/2006 @ 22:27:48
//  Platforms : All
//  Access    : Public
//  Purpose   : Demonstrates a simple, high-level snap acquisition, built using
//              MSVC.
//
//////////////////////////////////////////////////////////////////////////////


//============================================================================
//  Includes
//============================================================================
#include "stdafx.h"
#include "Snap.h"
#include "SnapDlg.h"

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
//  CSnapDlg::CSnapDlg
//
//  Description:
//      Constructor for the main dialog.
//
//////////////////////////////////////////////////////////////////////////////
CSnapDlg::CSnapDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CSnapDlg::IDD, pParent),
	image (NULL),
	session (NULL)
{
	//{{AFX_DATA_INIT(CSnapDlg)
	camName = _T("cam0");
	//}}AFX_DATA_INIT
	// Note that LoadIcon does not require a subsequent DestroyIcon in Win32
	m_hIcon = AfxGetApp()->LoadIcon(IDR_IMAQDX);
}


//////////////////////////////////////////////////////////////////////////////
//
//  CSnapDlg::DoDataExchange
//
//  Description:
//      Exchanges data between dialog and our member variables.
//
//////////////////////////////////////////////////////////////////////////////

void CSnapDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CSnapDlg)
	DDX_Text(pDX, IDC_CAMNAME, camName);
	//}}AFX_DATA_MAP
}

//============================================================================
//  The message map
//============================================================================
BEGIN_MESSAGE_MAP(CSnapDlg, CDialog)
	//{{AFX_MSG_MAP(CSnapDlg)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_BN_CLICKED(ID_SNAP, OnSnap)
	ON_BN_CLICKED(ID_QUIT, OnQuit)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()


//////////////////////////////////////////////////////////////////////////////
//
//  CSnapDialog::OnInitDialog
//
//  Description:
//      Called when it's time to initialize the dialog.
//
//////////////////////////////////////////////////////////////////////////////
BOOL CSnapDlg::OnInitDialog()
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
//  CHLSnapDlg::OnSnap
//
//  Description:
//      Event handler method which is called in reponse to the user single-
//      clicking on the Snap button.
//
//////////////////////////////////////////////////////////////////////////////
void CSnapDlg::OnSnap() 
{
	//--------------------------------------------------------------------
	//  Dispose any existing image
	//--------------------------------------------------------------------
	if(image)
	{
		imaqDispose(image);
		image = NULL;
	}

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
		//  We need an image for display purposes.  
		//--------------------------------------------------------------------
		image = imaqCreateImage (IMAQ_IMAGE_U8, 0);
		if (!image)
			throw VisionError();

		//-----------------------------------------------------------------------
		//  Now it's time to snap and display an image.  
		//-----------------------------------------------------------------------
		status = IMAQdxSnap (session, image);
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
    //  Close the session to the camera
    //-----------------------------------------------------------------------
	IMAQdxCloseCamera (session);
	session = 0;
}

//////////////////////////////////////////////////////////////////////////////
//
//  CSnapDlg::OnQuit
//
//  Description:
//      Message handler which is called when the user single-clicks the "Quit"
//      button.
//
//////////////////////////////////////////////////////////////////////////////
void CSnapDlg::OnQuit() 
{
    //------------------------------------------------------------------------
    //  The only cleanup we need to do is to close the display window and
    //  free the image associated with it.
    //------------------------------------------------------------------------
	if(image)
	{
		imaqDispose(image);
		image = NULL;
	}
    imaqCloseWindow(IMAQ_ALL_WINDOWS);
    EndDialog(0);
}
