//////////////////////////////////////////////////////////////////////////////
//
//  Title     : SequenceDlg.h
//  Project   : NI-IMAQdx
//  Created   : 8/24/2006 @ 22:42:23
//  Platforms : All
//  Access    : Public
//  Purpose   : Demonstrates a simple, high-level acquisition of a sequence 
//				of images, built using MSVC.
//
//////////////////////////////////////////////////////////////////////////////

#if !defined(AFX_SEQUENCEDLG_H__2F2C3DC3_8FB6_4756_BE9B_6FEA49A33C95__INCLUDED_)
#define AFX_SEQUENCEDLG_H__2F2C3DC3_8FB6_4756_BE9B_6FEA49A33C95__INCLUDED_

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
//  Class CSequenceDlg
//============================================================================
class CSequenceDlg : public CDialog
{
// Construction
public:
    //------------------------------------------------------------------------
    //  Constructor
    //------------------------------------------------------------------------
	CSequenceDlg(CWnd* pParent = NULL);	// standard constructor

    //------------------------------------------------------------------------
    //  Generated data members
    //------------------------------------------------------------------------
	//{{AFX_DATA(CSequenceDlg)
	enum { IDD = IDD_SEQUENCE_DIALOG };
	CSliderCtrl	DisplayImageCtrl;
	CString	camName;
	int		ImageToDisplay;
	int		imageArraySize;
	//}}AFX_DATA

    //------------------------------------------------------------------------
    //  ClassWizard generated virtual function overrides
    //------------------------------------------------------------------------
	//{{AFX_VIRTUAL(CSequenceDlg)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support
	//}}AFX_VIRTUAL

protected:
    //------------------------------------------------------------------------
    //  Protected members
    //------------------------------------------------------------------------
	HICON m_hIcon;

    //------------------------------------------------------------------------
    //  Generated message map functions
    //------------------------------------------------------------------------
	//{{AFX_MSG(CSequenceDlg)
	virtual BOOL OnInitDialog();
	afx_msg void OnQuit();
	afx_msg void OnStart();
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

#endif // !defined(AFX_SEQUENCEDLG_H__2F2C3DC3_8FB6_4756_BE9B_6FEA49A33C95__INCLUDED_)
