// Threshold And LabelDlg.h : header file
//

#if !defined(AFX_THRESHOLDANDLABELDLG_H__64EA501B_9636_4858_95EE_5888F6CD2739__INCLUDED_)
#define AFX_THRESHOLDANDLABELDLG_H__64EA501B_9636_4858_95EE_5888F6CD2739__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

/////////////////////////////////////////////////////////////////////////////
// CThresholdAndLabelDlg dialog

class CThresholdAndLabelDlg : public CDialog
{
// Construction
public:
	CThresholdAndLabelDlg(CWnd* pParent = NULL);	// standard constructor

// Dialog Data
	//{{AFX_DATA(CThresholdAndLabelDlg)
	enum { IDD = IDD_THRESHOLDANDLABEL_DIALOG };
	CButton	m_connectivity4;
	CButton	m_connectivity8;
	CSliderCtrl	m_threshMinSlider;
	CSliderCtrl	m_threshMaxSlider;
	CStatic	m_histogram;
	CString	m_particleCount;
	CString	m_threshMaxText;
	CString	m_threshMinText;
	//}}AFX_DATA

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CThresholdAndLabelDlg)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:
	HICON m_hIcon;
    int m_threshMin;
    int m_threshMax;
    Image* image;
    Image* labelledImage;
    Image* LoadImage(const CString& file, const CString& title);
    void Histogram();
    void ThresholdAndLabel();
	// Generated message map functions
	//{{AFX_MSG(CThresholdAndLabelDlg)
	virtual BOOL OnInitDialog();
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	afx_msg void OnBrowse();
	afx_msg void OnHScroll(UINT nSBCode, UINT nPos, CScrollBar* pScrollBar);
	afx_msg void OnConnectivity();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_THRESHOLDANDLABELDLG_H__64EA501B_9636_4858_95EE_5888F6CD2739__INCLUDED_)
