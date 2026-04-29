// Easy Pattern Matching.h : main header file for the EASY PATTERN MATCHING application
//

#if !defined(AFX_EASYPATTERNMATCHING_H__91FA8D44_6FB4_4C73_AE0D_332B36002AC2__INCLUDED_)
#define AFX_EASYPATTERNMATCHING_H__91FA8D44_6FB4_4C73_AE0D_332B36002AC2__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#ifndef __AFXWIN_H__
	#error include 'stdafx.h' before including this file for PCH
#endif

#include "resource.h"		// main symbols

/////////////////////////////////////////////////////////////////////////////
// CEasyPatternMatchingApp:
// See Easy Pattern Matching.cpp for the implementation of this class
//

class CEasyPatternMatchingApp : public CWinApp
{
public:
	CEasyPatternMatchingApp();

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CEasyPatternMatchingApp)
	public:
	virtual BOOL InitInstance();
	//}}AFX_VIRTUAL

// Implementation
	//{{AFX_MSG(CEasyPatternMatchingApp)
		// NOTE - the ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};


/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_EASYPATTERNMATCHING_H__91FA8D44_6FB4_4C73_AE0D_332B36002AC2__INCLUDED_)
