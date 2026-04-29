//////////////////////////////////////////////////////////////////////////////
//
//  Title     : Snap.h
//  Project   : NI-IMAQdx
//  Created   : 8/24/2006 @ 22:50:02
//  Platforms : All
//  Access    : Public
//  Purpose   : Demonstrates a simple, high-level snap acquisition, built using
//              MSVC.
//
//////////////////////////////////////////////////////////////////////////////

#if !defined(AFX_SNAP_H__1C418DB7_35E0_40D2_9722_04D2E51B6D4C__INCLUDED_)
#define AFX_SNAP_H__1C418DB7_35E0_40D2_9722_04D2E51B6D4C__INCLUDED_

//============================================================================
//  Only include this file once
//============================================================================
#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#ifndef __AFXWIN_H__
	#error include 'stdafx.h' before including this file for PCH
#endif

#include "resource.h"		// main symbols

//============================================================================
//  Class CSnapApp
//============================================================================

class CSnapApp : public CWinApp
{
public:
    //------------------------------------------------------------------------
    //  Constructor
    //------------------------------------------------------------------------
	CSnapApp();

    //------------------------------------------------------------------------
    //  ClassWizard generated virtual function overrides
    //------------------------------------------------------------------------
	//{{AFX_VIRTUAL(CSnapApp)
	public:
	virtual BOOL InitInstance();
	//}}AFX_VIRTUAL
	//{{AFX_MSG(CSnapApp)
		// NOTE - the ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};


//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_SNAP_H__1C418DB7_35E0_40D2_9722_04D2E51B6D4C__INCLUDED_)
