// Pattern MatchingDlg.h : header file
//

#if !defined(AFX_PATTERNMATCHINGDLG_H__353D4789_4795_4762_981F_BBF16CFD9B1E__INCLUDED_)
#define AFX_PATTERNMATCHINGDLG_H__353D4789_4795_4762_981F_BBF16CFD9B1E__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "LoadReferenceDialog.h"
#include "ChooseTemplateDialog.h"
#include "LearningDialog.h"
#include "MatchingDialog.h"


/////////////////////////////////////////////////////////////////////////////
// CPatternMatchingDlg dialog

class CPatternMatchingDlg : public CDialog
{
// Construction
public:
	CPatternMatchingDlg(CWnd* pParent = NULL);	// standard constructor

// Dialog Data
	//{{AFX_DATA(CPatternMatchingDlg)
	enum { IDD = IDD_PATTERNMATCHING_DIALOG };
		// NOTE: the ClassWizard will add data members here
	//}}AFX_DATA

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CPatternMatchingDlg)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support
	//}}AFX_VIRTUAL
    Image* LoadAndDisplay(const char* path, const char* name);

// Implementation
protected:
	HICON m_hIcon;
    CLoadReferenceDialog m_loadReference;
    CChooseTemplateDialog m_chooseTemplate;
    CLearningDialog m_learning;
    CMatchingDialog m_matching;
    CRect m_subdialogRect;
    Image* searchImage;
    Image* templateImage;
    int learnRotation;
	// Generated message map functions
	//{{AFX_MSG(CPatternMatchingDlg)
	virtual BOOL OnInitDialog();
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
    afx_msg LRESULT OnReferenceFileReady(WPARAM, LPARAM);
    afx_msg LRESULT OnTemplateSelected(WPARAM, LPARAM);
    afx_msg LRESULT OnDoneLearning(WPARAM, LPARAM);
    afx_msg LRESULT OnGetTemplateImage(WPARAM, LPARAM);
    afx_msg LRESULT OnGetSearchImage(WPARAM, LPARAM);
    afx_msg LRESULT OnFileDialog(WPARAM, LPARAM);
    afx_msg LRESULT OnSetTemplateArea(WPARAM, LPARAM);
    afx_msg LRESULT OnGetLearnRotation(WPARAM, LPARAM);
	afx_msg void OnQuit();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_PATTERNMATCHINGDLG_H__353D4789_4795_4762_981F_BBF16CFD9B1E__INCLUDED_)
