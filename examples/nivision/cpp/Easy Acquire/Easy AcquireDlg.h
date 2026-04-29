// Easy AcquireDlg.h : header file
//

#if !defined(AFX_EASYACQUIREDLG_H__C0F5C9DB_A811_425A_8ECA_566F7237F7D3__INCLUDED_)
#define AFX_EASYACQUIREDLG_H__C0F5C9DB_A811_425A_8ECA_566F7237F7D3__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000


#include <nivision.h>

/////////////////////////////////////////////////////////////////////////////
// CEasyAcquireDlg dialog

class CEasyAcquireDlg : public CDialog
{
// Construction
public:
	CEasyAcquireDlg(CWnd* pParent = NULL);	// standard constructor

// Dialog Data
	//{{AFX_DATA(CEasyAcquireDlg)
	enum { IDD = IDD_EASYACQUIRE_DIALOG };
	CString	m_interfaceName;
	//}}AFX_DATA

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CEasyAcquireDlg)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:
	HICON m_hIcon;
    Image* image;   //IMAQ:  This stores our image
	// Generated message map functions
	//{{AFX_MSG(CEasyAcquireDlg)
	virtual BOOL OnInitDialog();
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	afx_msg void OnAcquire();
	afx_msg void OnQuit();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_EASYACQUIREDLG_H__C0F5C9DB_A811_425A_8ECA_566F7237F7D3__INCLUDED_)
