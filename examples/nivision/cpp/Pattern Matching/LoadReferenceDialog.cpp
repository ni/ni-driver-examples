// LoadReferenceDialog.cpp : implementation file
//

#include "stdafx.h"
#include "Pattern Matching.h"
#include "LoadReferenceDialog.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CLoadReferenceDialog dialog


CLoadReferenceDialog::CLoadReferenceDialog(CWnd* pParent /*=NULL*/)
	: CDialog(CLoadReferenceDialog::IDD, pParent)
{
	//{{AFX_DATA_INIT(CLoadReferenceDialog)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT
}


void CLoadReferenceDialog::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CLoadReferenceDialog)
		// NOTE: the ClassWizard will add DDX and DDV calls here
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CLoadReferenceDialog, CDialog)
	//{{AFX_MSG_MAP(CLoadReferenceDialog)
	ON_BN_CLICKED(IDB_BROWSE, OnBrowse)
	ON_WM_CANCELMODE()
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CLoadReferenceDialog message handlers

void CLoadReferenceDialog::OnBrowse() 
{
    if (GetParent()->SendMessage(WM_FILEDIALOG)) {
        GetParent()->PostMessage(WM_REFERENCEFILEREADY);
    }
}

BOOL CLoadReferenceDialog::OnInitDialog() 
{
	CDialog::OnInitDialog();
	// Add extra initialization here
	
	return TRUE;  // return TRUE unless you set the focus to a control
	              // EXCEPTION: OCX Property Pages should return FALSE
}

void CLoadReferenceDialog::OnCancelMode() 
{
	CDialog::OnCancelMode();
	
	// Add your message handler code here
	
}
