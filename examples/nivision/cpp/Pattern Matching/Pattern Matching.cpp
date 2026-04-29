// Pattern Matching.cpp : Defines the class behaviors for the application.
//

#include "stdafx.h"
#include "Pattern Matching.h"
#include "Pattern MatchingDlg.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CPatternMatchingApp

BEGIN_MESSAGE_MAP(CPatternMatchingApp, CWinApp)
	//{{AFX_MSG_MAP(CPatternMatchingApp)
		// NOTE - the ClassWizard will add and remove mapping macros here.
		//    DO NOT EDIT what you see in these blocks of generated code!
	//}}AFX_MSG
	ON_COMMAND(ID_HELP, CWinApp::OnHelp)
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CPatternMatchingApp construction

CPatternMatchingApp::CPatternMatchingApp()
{
	// Add construction code here,
	// Place all significant initialization in InitInstance
}

/////////////////////////////////////////////////////////////////////////////
// The one and only CPatternMatchingApp object

CPatternMatchingApp theApp;

/////////////////////////////////////////////////////////////////////////////
// CPatternMatchingApp initialization

BOOL CPatternMatchingApp::InitInstance()
{
	// Standard initialization
	// If you are not using these features and wish to reduce the size
	//  of your final executable, you should remove from the following
	//  the specific initialization routines you do not need.

#ifdef _AFXDLL
	Enable3dControls();			// Call this when using MFC in a shared DLL
#else
	Enable3dControlsStatic();	// Call this when linking to MFC statically
#endif

	CPatternMatchingDlg dlg;
	m_pMainWnd = &dlg;
	int nResponse = dlg.DoModal();
	if (nResponse == IDOK)
	{
		// Place code here to handle when the dialog is
		// dismissed with OK
	}
	else if (nResponse == IDCANCEL)
	{
		// Place code here to handle when the dialog is
		// dismissed with Cancel
	}

	// Since the dialog has been closed, return FALSE so that we exit the
	//  application, rather than start the application's message pump.
	return FALSE;
}


Image* PlaceImageOnDialog(CWnd* wnd, int areaID, Image* originalImage) {
    //-------------------------------------------------------------------
    //  Find out how big an area we have and how big the image is.
    //  big the template is.
    //-------------------------------------------------------------------
    CRect imageArea;
    wnd->GetDlgItem(areaID)->GetWindowRect(imageArea);
    wnd->ScreenToClient(imageArea);
    int width, height;
    imaqGetImageSize(originalImage, &width, &height);
    //-------------------------------------------------------------------
    //  Make a copy of the image to display.  If the image is smaller than 
    //  the display area, we can just use a copy of it, else scale.
    //-------------------------------------------------------------------
    Image* displayImage = imaqCreateImage(IMAQ_IMAGE_U8, 0);
    if (width <= imageArea.Width() && height <= imageArea.Height()) {
        imaqDuplicate(displayImage, originalImage);
    }
    else {
        double horzScaling = (double) imageArea.Width() / width;
        double vertScaling = (double) imageArea.Height() / height;
        double scaling = (horzScaling < vertScaling) ? horzScaling : vertScaling;
        width = (int) (width * scaling);
        height = (int) (height * scaling);
        imaqResample(displayImage, originalImage, width, height, IMAQ_BILINEAR, IMAQ_NO_RECT);
    }
    //-------------------------------------------------------------------
    //  Now that we have a new height & width for the image, center it.
    //-------------------------------------------------------------------
    imageArea.OffsetRect((imageArea.Width() - width) / 2, (imageArea.Height() - height) / 2);
    imageArea.right = imageArea.left + width;
    imageArea.bottom = imageArea.top + height;
    //-------------------------------------------------------------------
    //  Make an image display window, but make it a child of this dialog.
    //-------------------------------------------------------------------
    imaqShowWindow(TEMPLATE_WINDOW, FALSE);
    imaqSetupWindow(TEMPLATE_WINDOW, 0);
    ::SetParent((HWND) imaqGetSystemWindowHandle(TEMPLATE_WINDOW), wnd->m_hWnd);
    ::MoveWindow((HWND) imaqGetSystemWindowHandle(TEMPLATE_WINDOW), imageArea.left, imageArea.top, imageArea.Width(), imageArea.Height(), false);
    imaqShowWindow(TEMPLATE_WINDOW, FALSE);
    imaqDisplayImage(displayImage, TEMPLATE_WINDOW, TRUE);
    return displayImage;
}
