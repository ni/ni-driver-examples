// Easy Pattern MatchingDlg.h : header file
//

#if !defined(AFX_EASYPATTERNMATCHINGDLG_H__FF740A12_7F14_46C6_B211_69C8FDBFAF83__INCLUDED_)
#define AFX_EASYPATTERNMATCHINGDLG_H__FF740A12_7F14_46C6_B211_69C8FDBFAF83__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

/////////////////////////////////////////////////////////////////////////////
// CEasyPatternMatchingDlg dialog

class CEasyPatternMatchingDlg : public CDialog
{
// Construction
public:
	CEasyPatternMatchingDlg(CWnd* pParent = NULL);	// standard constructor

// Dialog Data
	//{{AFX_DATA(CEasyPatternMatchingDlg)
	enum { IDD = IDD_EASYPATTERNMATCHING_DIALOG };
	CStatic	m_learningText;
	CButton	m_doSearchButton;
	CButton	m_learnTemplateButton;
	CButton	m_loadImagesButton;
	CString	m_matchScore;
	BOOL	m_rotationInvariance;
	CString	m_maxMatches;
	CString	m_matchesText;
	//}}AFX_DATA

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CEasyPatternMatchingDlg)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:
	HICON m_hIcon;
   Image* templateImage;
   Image* searchImage;

   void DrawBoxAroundMatches(const PatternMatch* report, int numMatches, RGBValue boxColor, int windowNumber);
	// Generated message map functions
	//{{AFX_MSG(CEasyPatternMatchingDlg)
	virtual BOOL OnInitDialog();
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	afx_msg void OnLoadImages();
	afx_msg void OnLearnTemplate();
	afx_msg void OnDoSearch();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_EASYPATTERNMATCHINGDLG_H__FF740A12_7F14_46C6_B211_69C8FDBFAF83__INCLUDED_)
