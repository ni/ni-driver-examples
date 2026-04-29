// Easy AcquireDlg.cpp : implementation file
//

#include "stdafx.h"
#include "Easy Acquire.h"
#include "Easy AcquireDlg.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CEasyAcquireDlg dialog

CEasyAcquireDlg::CEasyAcquireDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CEasyAcquireDlg::IDD, pParent)
{
	//{{AFX_DATA_INIT(CEasyAcquireDlg)
	m_interfaceName = _T("img0");
	//}}AFX_DATA_INIT
	// Note that LoadIcon does not require a subsequent DestroyIcon in Win32
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CEasyAcquireDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CEasyAcquireDlg)
	DDX_Text(pDX, IDC_INTERFACE_NAME, m_interfaceName);
	//}}AFX_DATA_MAP
}

BEGIN_MESSAGE_MAP(CEasyAcquireDlg, CDialog)
	//{{AFX_MSG_MAP(CEasyAcquireDlg)
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_BN_CLICKED(IDB_ACQUIRE, OnAcquire)
	ON_BN_CLICKED(IDQUIT, OnQuit)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CEasyAcquireDlg message handlers

BOOL CEasyAcquireDlg::OnInitDialog()
{
  	CDialog::OnInitDialog();
	// Set the icon for this dialog.  The framework does this automatically
	//  when the application's main window is not a dialog
	SetIcon(m_hIcon, TRUE);			// Set big icon
	SetIcon(m_hIcon, FALSE);		// Set small icon
    
    //-----------------------------------------------------------------------
    //  IMAQ:  Since we don't initially have an image, set the image pointer
    //  to NULL
    //-----------------------------------------------------------------------
    image = NULL;
	
	return TRUE;  // return TRUE  unless you set the focus to a control
}

// If you add a minimize button to your dialog, you will need the code below
//  to draw the icon.  For MFC applications using the document/view model,
//  this is automatically done for you by the framework.

void CEasyAcquireDlg::OnPaint() 
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
HCURSOR CEasyAcquireDlg::OnQueryDragIcon()
{
	return (HCURSOR) m_hIcon;
}


//---------------------------------------------------------------------------
//  All code below here added for IMAQ.
//---------------------------------------------------------------------------

void CEasyAcquireDlg::OnAcquire()  {
    //-----------------------------------------------------------------------
    //  IMAQ:  Update data to read any changes to the interface name.
    //  Get rid of any old image, then acquire a new image on the desired
    //  interface and display it.
    //-----------------------------------------------------------------------
    UpdateData(TRUE);
    imaqDispose(image);
    image = imaqEasyAcquire(m_interfaceName);
    imaqDisplayImage(image, 0, TRUE);
}

void CEasyAcquireDlg::OnQuit()  {
    //-----------------------------------------------------------------------
    //  IMAQ:  Dispose the image, then close the display windows and end
    //  the dialog
    //-----------------------------------------------------------------------
    imaqDispose(image);
    image = NULL;
    imaqCloseWindow(IMAQ_ALL_WINDOWS);
    EndDialog(0);
}
