//////////////////////////////////////////////////////////////////////////////
//
//  Title     : SnapDlg.h
//  Project   : NI-IMAQdx
//  Created   : 8/24/2006 @ 22:42:23
//  Platforms : All
//  Access    : Public
//  Purpose   : Demonstrates a simple, high-level snap acquisition, built using
//              MSVC.
//
//////////////////////////////////////////////////////////////////////////////

#if !defined(AFX_SNAPDLG_H__1C251765_37FE_409A_B8ED_4A36E6811E9E__INCLUDED_)
#define AFX_SNAPDLG_H__1C251765_37FE_409A_B8ED_4A36E6811E9E__INCLUDED_

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
//  Class CSnapDlg
//============================================================================
class CSnapDlg : public CDialog
{
public:
    //------------------------------------------------------------------------
    //  Constructor
    //------------------------------------------------------------------------
	CSnapDlg(CWnd* pParent = NULL);	// standard constructor

    //------------------------------------------------------------------------
    //  Generated data members
    //------------------------------------------------------------------------
	//{{AFX_DATA(CSnapDlg)
	enum { IDD = IDD_SNAP_DIALOG };
	CString	camName;
	//}}AFX_DATA

    //------------------------------------------------------------------------
    //  ClassWizard generated virtual function overrides
    //------------------------------------------------------------------------
	//{{AFX_VIRTUAL(CSnapDlg)
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
	//{{AFX_MSG(CSnapDlg)
	virtual BOOL OnInitDialog();
	afx_msg void OnSnap();
	afx_msg void OnQuit();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

private:
    //------------------------------------------------------------------------
    //  Members
    //------------------------------------------------------------------------
	SESSION_ID session;
	Image* image;

};


//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_SNAPDLG_H__1C251765_37FE_409A_B8ED_4A36E6811E9E__INCLUDED_)
