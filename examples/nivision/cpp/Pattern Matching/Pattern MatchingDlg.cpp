// Pattern MatchingDlg.cpp : implementation file
//

#include "stdafx.h"
#include "Pattern Matching.h"
#include "Pattern MatchingDlg.h"
#include <process.h>

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

//---------------------------------------------------------------------------
//  IMAQ:  GetImageDirectory sets up a logical default directory.  If you
//  move your executable, you will want to change this function.
//---------------------------------------------------------------------------
static const char* GetImageDirectory() {
    static char dir[512];
    if (*dir)
        return dir;
    GetModuleFileName(NULL, dir, 512);
    char* endOfPath = strrchr(dir, '\\') + 1;
    strcpy(endOfPath, "..\\Images");
    return dir;
}


/////////////////////////////////////////////////////////////////////////////
// CPatternMatchingDlg dialog

CPatternMatchingDlg::CPatternMatchingDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CPatternMatchingDlg::IDD, pParent)
{
	//{{AFX_DATA_INIT(CPatternMatchingDlg)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT
	// Note that LoadIcon does not require a subsequent DestroyIcon in Win32
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CPatternMatchingDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CPatternMatchingDlg)
		// NOTE: the ClassWizard will add DDX and DDV calls here
	//}}AFX_DATA_MAP
}

BEGIN_MESSAGE_MAP(CPatternMatchingDlg, CDialog)
	//{{AFX_MSG_MAP(CPatternMatchingDlg)
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
    ON_MESSAGE(WM_REFERENCEFILEREADY, OnReferenceFileReady)
    ON_MESSAGE(WM_TEMPLATESELECTED, OnTemplateSelected)
    ON_MESSAGE(WM_DONELEARNING, OnDoneLearning)
    ON_MESSAGE(WM_GETTEMPLATEIMAGE, OnGetTemplateImage)
    ON_MESSAGE(WM_GETSEARCHIMAGE, OnGetSearchImage)
    ON_MESSAGE(WM_GETLEARNROTATION, OnGetLearnRotation)
    ON_MESSAGE(WM_FILEDIALOG, OnFileDialog)
    ON_MESSAGE(WM_SETTEMPLATEAREA, OnSetTemplateArea)
	ON_BN_CLICKED(IDQUIT, OnQuit)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CPatternMatchingDlg message handlers

BOOL CPatternMatchingDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	// Set the icon for this dialog.  The framework does this automatically
	//  when the application's main window is not a dialog
	SetIcon(m_hIcon, TRUE);			// Set big icon
	SetIcon(m_hIcon, FALSE);		// Set small icon
    
    //-----------------------------------------------------------------------
    // IMAQ:  Initialize the image pointers to NULL.
    // There is a combo box that is where we want our subdialogs to go.
    // Find out its position.
    //-----------------------------------------------------------------------
    searchImage = NULL;
    templateImage = NULL;
    GetDlgItem(IDC_SUBDIALOG_AREA)->GetWindowRect(m_subdialogRect);
    ScreenToClient(m_subdialogRect);
    //-----------------------------------------------------------------------
    // IMAQ:  Create the subdialogs and move them to the desired position.
    // Make the first subdialog visible.
    //-----------------------------------------------------------------------
    m_loadReference.Create(IDD_LOADREFERENCE, this);
    m_loadReference.MoveWindow(m_subdialogRect);
    m_chooseTemplate.Create(IDD_CHOOSETEMPLATE, this);
    m_chooseTemplate.MoveWindow(m_subdialogRect);
    m_learning.Create(IDD_LEARNING, this);
    m_learning.MoveWindow(m_subdialogRect);
    m_matching.Create(IDD_MATCHING, this);
    m_matching.MoveWindow(m_subdialogRect);
    m_loadReference.ShowWindow(SW_SHOW);

	return TRUE;  // return TRUE  unless you set the focus to a control
}

// If you add a minimize button to your dialog, you will need the code below
//  to draw the icon.  For MFC applications using the document/view model,
//  this is automatically done for you by the framework.

void CPatternMatchingDlg::OnPaint() 
{
	if (IsIconic())
	{
		CPaintDC dc(this); // device context for painting

		SendMessage(WM_ICONERASEBKGND, (WPARAM) dc.GetSafeHdc(), 0);

		// Center icon in client rectangle
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// Draw the icon
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialog::OnPaint();
	}
}

// The system calls this to obtain the cursor to display while the user drags
//  the minimized window.
HCURSOR CPatternMatchingDlg::OnQueryDragIcon()
{
	return (HCURSOR) m_hIcon;
}

//---------------------------------------------------------------------------
//  IMAQ:  All code below this point is new.
//---------------------------------------------------------------------------


LRESULT CPatternMatchingDlg::OnReferenceFileReady(WPARAM, LPARAM) {
    //-----------------------------------------------------------------------
    //  Put up the next subdialog.
    //-----------------------------------------------------------------------
    m_loadReference.ShowWindow(SW_HIDE);
    m_chooseTemplate.ShowWindow(SW_SHOW);
    return 1;
}



Image* CPatternMatchingDlg::LoadAndDisplay(const char* path, const char* name) {
    //-----------------------------------------------------------------------
    //  Create a new image.  Since pattern matching works only on 8 bit images
    //  we'll make an 8 bit image.
    //-----------------------------------------------------------------------
    Image* image = imaqCreateImage(IMAQ_IMAGE_U8, 0);
    if (!image) {
        char* err = imaqGetErrorText(imaqGetLastError());
        MessageBox(err, "Error Creating Image", MB_OK);
        imaqDispose(err);
        imaqDispose(image);
        return NULL;
    }
    //-----------------------------------------------------------------------
    //  Read in the image.
    //-----------------------------------------------------------------------
    if (!imaqReadFile(image, path, NULL, NULL)) {
        char* err = imaqGetErrorText(imaqGetLastError());
        MessageBox(err, "Error Reading the File", MB_OK);
        imaqDispose(err);
        imaqDispose(image);
        return NULL;
    }
    //-----------------------------------------------------------------------
    // Display it.
    //-----------------------------------------------------------------------
    imaqMoveWindow(IMAGE_WINDOW, imaqMakePoint(0, 0));
    imaqDisplayImage(image, IMAGE_WINDOW, TRUE);
    imaqSetWindowTitle(IMAGE_WINDOW, name);
    //-----------------------------------------------------------------------
    //  Make it our search image.
    //-----------------------------------------------------------------------
    imaqDispose(searchImage);
    searchImage = image;
    return searchImage;
}


LRESULT CPatternMatchingDlg::OnSetTemplateArea(WPARAM area, LPARAM) {
    Rect templateArea = *reinterpret_cast<Rect*>(area);
    //-----------------------------------------------------------------------
    //  The template area is ready for us.  Make the template image.
    //-----------------------------------------------------------------------
    imaqDispose(templateImage);
    templateImage = imaqCreateImage(IMAQ_IMAGE_U8, 0);
    if (!templateImage || !imaqSetImageSize(templateImage, templateArea.width, templateArea.height)) {
        char* err = imaqGetErrorText(imaqGetLastError());
        MessageBox(err, "Error Making Template Image", MB_OK);
        imaqDispose(err);
        return 0;
    }
    imaqCopyRect(templateImage, searchImage, templateArea, imaqMakePoint(0, 0));
    return 1;
}


LRESULT CPatternMatchingDlg::OnTemplateSelected(WPARAM rotation, LPARAM) {
    //-----------------------------------------------------------------------
    //  Put up the next subdialog.
    //-----------------------------------------------------------------------
    learnRotation = rotation;
    m_chooseTemplate.ShowWindow(SW_HIDE);
    m_learning.ShowWindow(SW_SHOW);
    return 0;
}


LRESULT CPatternMatchingDlg::OnDoneLearning(WPARAM success, LPARAM) {
    //-----------------------------------------------------------------------
    //  On success, put up the next dialog.  On failure, ask for a different
    //  template.
    //-----------------------------------------------------------------------
    if (success) {
        m_learning.ShowWindow(SW_HIDE);
        m_matching.ShowWindow(SW_SHOW);
    }
    else {
        m_learning.ShowWindow(SW_HIDE);
        m_chooseTemplate.ShowWindow(SW_SHOW);
    }
    return 0;
}


LRESULT CPatternMatchingDlg::OnGetTemplateImage(WPARAM, LPARAM) {
    return (LRESULT) templateImage;
}


LRESULT CPatternMatchingDlg::OnGetSearchImage(WPARAM, LPARAM) {
    return (LRESULT) searchImage;
}

LRESULT CPatternMatchingDlg::OnGetLearnRotation(WPARAM, LPARAM) {
    return (LRESULT) learnRotation;
}



LRESULT CPatternMatchingDlg::OnFileDialog(WPARAM, LPARAM) {
    CString path;
    CString file;
    //-----------------------------------------------------------------------
    //  Query the user for the name of the file.  If the user hit OK.
    //  OK, then store the name of the selected file, and notify the parent
    //  dialog we have a file name now.
    //-----------------------------------------------------------------------
    CFileDialog fileDialog(TRUE, NULL, NULL, OFN_HIDEREADONLY | OFN_OVERWRITEPROMPT, "Image Files (*.jpg;*.png;*.bmp;*.tif;*.tiff;*.apd)|*.jpg;*.png;*.bmp;*.tif;*.tiff;*.apd|All Files(*.*)|*.*||");
    fileDialog.m_ofn.lpstrInitialDir = GetImageDirectory();
    if (fileDialog.DoModal() == IDOK) {
        path  = fileDialog.GetPathName();
        file  = fileDialog.GetFileName();
        return LoadAndDisplay(path, file) != NULL;
    }
    else {
        return 0;
    }
}


void CPatternMatchingDlg::OnQuit() 
{
    EndDialog(TRUE);
}
