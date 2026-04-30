//////////////////////////////////////////////////////////////////////////////
//
//  Title     : LLSequenceDlg.h
//  Project   : NI-IMAQdx
//  Created   : 8/29/2006 @ 14:29:23
//  Platforms : All
//  Access    : Public
//  Purpose   : Demonstrates a simple, low-level acquisition of a sequence 
//				of images, built using MSVC.
//
//////////////////////////////////////////////////////////////////////////////
#if !defined(AFX_LLSEQUENCEDLG_H__6046C708_0034_4A18_B430_4D3C1C681C3E__INCLUDED_)
#define AFX_LLSEQUENCEDLG_H__6046C708_0034_4A18_B430_4D3C1C681C3E__INCLUDED_

//============================================================================
//  Only include this file once.
//============================================================================
#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

//-----------------------------------------------------------------------
//  Header files required by Example
//-----------------------------------------------------------------------
#include "NIIMAQdx.h"
#include "nivision.h"


//============================================================================
//  Class CLLSequenceDlg
//============================================================================
class CLLSequenceDlg : public CDialog
{
public:
    //------------------------------------------------------------------------
    //  Constructor
    //------------------------------------------------------------------------
	CLLSequenceDlg(CWnd* pParent = NULL);	// standard constructor

    //------------------------------------------------------------------------
    //  Generated data members
    //------------------------------------------------------------------------
	//{{AFX_DATA(CLLSequenceDlg)
	enum { IDD = IDD_LLSEQUENCE_DIALOG };
	CSliderCtrl	DisplayImageCtrl;
	CString	camName;
	int		ImageToDisplay;
	long	imageArraySize;
	//}}AFX_DATA

    //------------------------------------------------------------------------
    //  ClassWizard generated virtual function overrides
    //------------------------------------------------------------------------
	//{{AFX_VIRTUAL(CLLSequenceDlg)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:
    //------------------------------------------------------------------------
    //  Protected members
    //------------------------------------------------------------------------
	HICON m_hIcon;

    //------------------------------------------------------------------------
    //  Generated message map functions
    //------------------------------------------------------------------------
	//{{AFX_MSG(CLLSequenceDlg)
	virtual BOOL OnInitDialog();
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	afx_msg void OnStart();
	afx_msg void OnQuit();
	afx_msg void OnDisplayedImage(NMHDR* pNMHDR, LRESULT* pResult);
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

private:
    //------------------------------------------------------------------------
    //  Members
    //------------------------------------------------------------------------
	SESSION_ID session;
	Image** imageArray;

};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_LLSEQUENCEDLG_H__6046C708_0034_4A18_B430_4D3C1C681C3E__INCLUDED_)
