// Easy Acquire.h : main header file for the EASY ACQUIRE application
//

#if !defined(AFX_EASYACQUIRE_H__7307C29B_C10E_4825_9A53_130EA3FF0C5C__INCLUDED_)
#define AFX_EASYACQUIRE_H__7307C29B_C10E_4825_9A53_130EA3FF0C5C__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#ifndef __AFXWIN_H__
	#error include 'stdafx.h' before including this file for PCH
#endif

#include "resource.h"		// main symbols

/////////////////////////////////////////////////////////////////////////////
// CEasyAcquireApp:
// See Easy Acquire.cpp for the implementation of this class
//

class CEasyAcquireApp : public CWinApp
{
public:
	CEasyAcquireApp();

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CEasyAcquireApp)
	public:
	virtual BOOL InitInstance();
	//}}AFX_VIRTUAL

// Implementation

	//{{AFX_MSG(CEasyAcquireApp)
		// NOTE - the ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};


/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_EASYACQUIRE_H__7307C29B_C10E_4825_9A53_130EA3FF0C5C__INCLUDED_)
