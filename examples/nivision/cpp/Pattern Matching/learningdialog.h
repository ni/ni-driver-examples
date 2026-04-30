#if !defined(AFX_LEARNINGDIALOG_H__51F925E8_B77B_4728_B28E_08E254D61302__INCLUDED_)
#define AFX_LEARNINGDIALOG_H__51F925E8_B77B_4728_B28E_08E254D61302__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// CLearningDialog.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CLearningDialog dialog

class CLearningDialog : public CDialog
{
// Construction
public:
	CLearningDialog(CWnd* pParent = NULL);   // standard constructor

// Dialog Data
	//{{AFX_DATA(CLearningDialog)
	enum { IDD = IDD_LEARNING };
	CProgressCtrl	m_progress;
	//}}AFX_DATA


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CLearningDialog)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:
    Image* templateImage;
    int increment;
    enum {
        minVal = 0,
        maxVal = 20
    };    
    static void __cdecl LearnThread(void* mainDialog);
    // Generated message map functions
	//{{AFX_MSG(CLearningDialog)
	afx_msg void OnShowWindow(BOOL bShow, UINT nStatus);
	afx_msg void OnTimer(UINT_PTR nIDEvent);
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_LEARNINGDIALOG_H__51F925E8_B77B_4728_B28E_08E254D61302__INCLUDED_)
