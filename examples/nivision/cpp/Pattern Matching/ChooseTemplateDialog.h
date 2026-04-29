#if !defined(AFX_CHOOSETEMPLATEDIALOG_H__0EC78B03_E09C_49E5_A15D_92B1BF99DF05__INCLUDED_)
#define AFX_CHOOSETEMPLATEDIALOG_H__0EC78B03_E09C_49E5_A15D_92B1BF99DF05__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// ChooseTemplateDialog.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CChooseTemplateDialog dialog

class CChooseTemplateDialog : public CDialog
{
// Construction
public:
	CChooseTemplateDialog(CWnd* pParent = NULL);   // standard constructor

// Dialog Data
	//{{AFX_DATA(CChooseTemplateDialog)
	enum { IDD = IDD_CHOOSETEMPLATE };
	CButton	m_ok;
	BOOL	m_rotation;
	//}}AFX_DATA


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CChooseTemplateDialog)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

public:
    void SelectRect(Rect rect);

// Implementation
protected:
    Image* templateImage;
	// Generated message map functions
	//{{AFX_MSG(CChooseTemplateDialog)
	afx_msg void OnShowWindow(BOOL bShow, UINT nStatus);
	afx_msg void OnOK();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_CHOOSETEMPLATEDIALOG_H__0EC78B03_E09C_49E5_A15D_92B1BF99DF05__INCLUDED_)
