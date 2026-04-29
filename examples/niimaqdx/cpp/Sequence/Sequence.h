//////////////////////////////////////////////////////////////////////////////
//
//  Title     : Sequence.h
//  Project   : NI-IMAQdx
//  Created   : 8/24/2006 @ 22:50:02
//  Platforms : All
//  Access    : Public
//  Purpose   : Demonstrates a simple, high-level acquisition of a sequence 
//				of images, built using MSVC.
//
//////////////////////////////////////////////////////////////////////////////

#if !defined(AFX_SEQUENCE_H__421A8E71_62D2_4629_82E0_E3A5103B1A7A__INCLUDED_)
#define AFX_SEQUENCE_H__421A8E71_62D2_4629_82E0_E3A5103B1A7A__INCLUDED_

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
//  Class CSequenceApp
//============================================================================

class CSequenceApp : public CWinApp
{
public:
	//------------------------------------------------------------------------
    //  Constructor
    //------------------------------------------------------------------------
	CSequenceApp();

    //------------------------------------------------------------------------
    //  ClassWizard generated virtual function overrides
    //------------------------------------------------------------------------
	//{{AFX_VIRTUAL(CSequenceApp)
	public:
	virtual BOOL InitInstance();
	//}}AFX_VIRTUAL
	//{{AFX_MSG(CSequenceApp)
		// NOTE - the ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_SEQUENCE_H__421A8E71_62D2_4629_82E0_E3A5103B1A7A__INCLUDED_)
