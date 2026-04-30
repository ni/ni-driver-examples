// Load And DisplayDlg.cpp : implementation file
//

#include "stdafx.h"
#include "Load And Display.h"
#include "Load And DisplayDlg.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

#define PREVIEW_WINDOW 0
#define FLOATING_WINDOW 1

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
// CLoadAndDisplayDlg dialog

CLoadAndDisplayDlg::CLoadAndDisplayDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CLoadAndDisplayDlg::IDD, pParent)
{
	//{{AFX_DATA_INIT(CLoadAndDisplayDlg)
	m_fileName = _T("");
	//}}AFX_DATA_INIT
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CLoadAndDisplayDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CLoadAndDisplayDlg)
	DDX_Text(pDX, IDC_FILENAME, m_fileName);
	//}}AFX_DATA_MAP
}

BEGIN_MESSAGE_MAP(CLoadAndDisplayDlg, CDialog)
	//{{AFX_MSG_MAP(CLoadAndDisplayDlg)
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_BN_CLICKED(IDOK, OnLoad)
	ON_BN_CLICKED(IDB_BROWSE, OnBrowse)
	ON_BN_CLICKED(IDB_LOAD, OnLoad)
	ON_BN_CLICKED(IDCANCEL, OnQuit)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CLoadAndDisplayDlg message handlers

BOOL CLoadAndDisplayDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	SetIcon(m_hIcon, TRUE);			// Set big icon
	SetIcon(m_hIcon, FALSE);		// Set small icon

    //-----------------------------------------------------------------------
    //  IMAQ:  Since we start out without an image,  set image to NULL
    //  Look for the default image directory.
    //-----------------------------------------------------------------------
    image = NULL;
	return TRUE;  // return TRUE  unless you set the focus to a control
}

// If you add a minimize button to your dialog, you will need the code below
//  to draw the icon.  For MFC applications using the document/view model,
//  this is automatically done for you by the framework.

void CLoadAndDisplayDlg::OnPaint() 
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

HCURSOR CLoadAndDisplayDlg::OnQueryDragIcon()
{
	return (HCURSOR) m_hIcon;
}

//---------------------------------------------------------------------------
//  IMAQ:  All code beyond this point is IMAQ code.
//---------------------------------------------------------------------------


void CLoadAndDisplayDlg::OnLoad() 
{
    UpdateData(FALSE);
    //-----------------------------------------------------------------------
    //  Get information about the file.
    //-----------------------------------------------------------------------
    int width, height;
    ImageType type;
    if (!imaqGetFileInfo((const char*) m_fileName, NULL, NULL, NULL, &width, &height, &type)) {
        char* err = imaqGetErrorText(imaqGetLastError());
        MessageBox(err, "Error Reading the File", MB_OK);
        imaqDispose(err);
        return;
    }
    //-----------------------------------------------------------------------
    //  Dispose any current image and build up a new one.
    //-----------------------------------------------------------------------
    imaqDispose(image);
    image = imaqCreateImage(type, 0);
    if (!imaqSetImageSize(image, width, height)) {
        char* err = imaqGetErrorText(imaqGetLastError());
        MessageBox(err, "Error Creating Image", MB_OK);
        imaqDispose(err);
        return;
    }
    //-----------------------------------------------------------------------
    //  Read in the image, and display it.  Note that an 8 bit image might 
    //  have been saved with a color palette.  Load in the palette and
    //  use it.
    //-----------------------------------------------------------------------
    RGBValue palette[256];
    int numColors = 0;
    if (!imaqReadFile(image, (const char*) m_fileName, palette, &numColors)) {
        char* err = imaqGetErrorText(imaqGetLastError());
        MessageBox(err, "Error Reading the File", MB_OK);
        imaqDispose(err);
        return;
    }
    imaqDisplayImage(image, FLOATING_WINDOW, TRUE);
    if (type == IMAQ_IMAGE_U8 && numColors > 0) {
        imaqSetWindowPalette(FLOATING_WINDOW, IMAQ_PALETTE_USER, palette, numColors);
    }
}

void CLoadAndDisplayDlg::OnBrowse() 
{
    //-----------------------------------------------------------------------
    //  Query the user for the name of the file to open.  If the user hit
    //  OK, then update the edit box on the dialog with the new name.
    //-----------------------------------------------------------------------
    CFileDialog fileDialog(TRUE, NULL, NULL, OFN_HIDEREADONLY | OFN_OVERWRITEPROMPT, "Image Files (*.jpg;*.png;*.bmp;*.tif;*.tiff;*.apd)|*.jpg;*.png;*.bmp;*.tif;*.tiff;*.apd|All Files(*.*)|*.*||");
    fileDialog.m_ofn.lpstrInitialDir = GetImageDirectory();
    if (fileDialog.DoModal() == IDOK) {
        UpdateData(TRUE);
        m_fileName = fileDialog.GetPathName();
        UpdateData(FALSE);
    }
}

void CLoadAndDisplayDlg::OnQuit() 
{
    //-----------------------------------------------------------------------
    //  We are done with the dialog.  Clean things up, then end the dialog.
    //-----------------------------------------------------------------------
    imaqDispose(image);
    image = NULL;
    imaqCloseWindow(FLOATING_WINDOW);
    EndDialog(IDOK);
}
