// Pattern Matching.h : main header file for the PATTERN MATCHING application
//

#if !defined(AFX_PATTERNMATCHING_H__84BD1450_4CF1_4BF1_8258_D2AD93219201__INCLUDED_)
#define AFX_PATTERNMATCHING_H__84BD1450_4CF1_4BF1_8258_D2AD93219201__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#ifndef __AFXWIN_H__
	#error include 'stdafx.h' before including this file for PCH
#endif

#include "resource.h"		// main symbols

/////////////////////////////////////////////////////////////////////////////
// CPatternMatchingApp:
// See Pattern Matching.cpp for the implementation of this class
//

class CPatternMatchingApp : public CWinApp
{
public:
	CPatternMatchingApp();

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CPatternMatchingApp)
	public:
	virtual BOOL InitInstance();
	//}}AFX_VIRTUAL

// Implementation

	//{{AFX_MSG(CPatternMatchingApp)
		// NOTE - the ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};


/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_PATTERNMATCHING_H__84BD1450_4CF1_4BF1_8258_D2AD93219201__INCLUDED_)
