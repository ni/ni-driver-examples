#if !defined(AFX_LOADREFERENCEDIALOG_H__E8F2BA8B_FB8D_42EF_8958_EAC89A00EFEA__INCLUDED_)
#define AFX_LOADREFERENCEDIALOG_H__E8F2BA8B_FB8D_42EF_8958_EAC89A00EFEA__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// LoadReferenceDialog.h : header file
//


/////////////////////////////////////////////////////////////////////////////
// CLoadReferenceDialog dialog

class CLoadReferenceDialog : public CDialog
{
// Construction
public:
	CLoadReferenceDialog(CWnd* pParent = NULL);   // standard constructor

// Dialog Data
	//{{AFX_DATA(CLoadReferenceDialog)
	enum { IDD = IDD_LOADREFERENCE };
		// NOTE: the ClassWizard will add data members here
	//}}AFX_DATA


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CLoadReferenceDialog)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:
    CString m_fileName;
	// Generated message map functions
	//{{AFX_MSG(CLoadReferenceDialog)
	afx_msg void OnBrowse();
	virtual BOOL OnInitDialog();
	afx_msg void OnCancelMode();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_LOADREFERENCEDIALOG_H__E8F2BA8B_FB8D_42EF_8958_EAC89A00EFEA__INCLUDED_)
