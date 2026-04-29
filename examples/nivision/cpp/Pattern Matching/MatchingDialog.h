#if !defined(AFX_MATCHINGDIALOG_H__879F77A7_1168_4EC8_9C02_48C3E0DD418A__INCLUDED_)
#define AFX_MATCHINGDIALOG_H__879F77A7_1168_4EC8_9C02_48C3E0DD418A__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// MatchingDialog.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CMatchingDialog dialog

class CMatchingDialog : public CDialog
{
// Construction
public:
	CMatchingDialog(CWnd* pParent = NULL);   // standard constructor

// Dialog Data
	//{{AFX_DATA(CMatchingDialog)
	enum { IDD = IDD_MATCHING };
	CButton	m_rotation;
	CListCtrl	m_resultsList;
	CSpinButtonCtrl	m_matchesSpin;
	CSpinButtonCtrl	m_contrastSpin;
	CSpinButtonCtrl	m_scoreSpin;
	UINT	m_matchesValue;
	UINT	m_scoreValue;
	UINT	m_contrastValue;
	BOOL	m_subpixel;
	CString	m_matchesFound;
	//}}AFX_DATA


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CMatchingDialog)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:
    void UpdateMatchesFound(int);
    void UpdateResultsList(PatternMatch*, int);
    void UpdateOverlay(PatternMatch*, int);
    void SizeColumns();
	// Generated message map functions
	//{{AFX_MSG(CMatchingDialog)
	virtual BOOL OnInitDialog();
	afx_msg void OnShowWindow(BOOL bShow, UINT nStatus);
	afx_msg void PerformSearch();
	afx_msg void OnNewimage();
	afx_msg void OnTimer(UINT_PTR nIDEvent);
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_MATCHINGDIALOG_H__879F77A7_1168_4EC8_9C02_48C3E0DD418A__INCLUDED_)
