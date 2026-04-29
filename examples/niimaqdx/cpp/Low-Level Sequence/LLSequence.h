//////////////////////////////////////////////////////////////////////////////
//
//  Title     : LLSequence.h
//  Project   : NI-IMAQdx
//  Created   : 8/29/2006 @ 14:31:02
//  Platforms : All
//  Access    : Public
//  Purpose   : Demonstrates a simple, low-level acquisition of a sequence 
//				of images, built using MSVC.
//
//////////////////////////////////////////////////////////////////////////////
#if !defined(AFX_LLSEQUENCE_H__DABE04D8_3730_4B48_98A7_DADBA40FB4E0__INCLUDED_)
#define AFX_LLSEQUENCE_H__DABE04D8_3730_4B48_98A7_DADBA40FB4E0__INCLUDED_

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
//  Class CLLSequenceApp
//============================================================================

class CLLSequenceApp : public CWinApp
{
public:
	//------------------------------------------------------------------------
    //  Constructor
    //------------------------------------------------------------------------
	CLLSequenceApp();

    //------------------------------------------------------------------------
    //  ClassWizard generated virtual function overrides
    //------------------------------------------------------------------------
	//{{AFX_VIRTUAL(CLLSequenceApp)
	public:
	virtual BOOL InitInstance();
	//}}AFX_VIRTUAL
	//{{AFX_MSG(CLLSequenceApp)
		// NOTE - the ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};


/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_LLSEQUENCE_H__DABE04D8_3730_4B48_98A7_DADBA40FB4E0__INCLUDED_)
