// Load And Display.h : main header file for the LOAD AND DISPLAY application
//

#if !defined(AFX_LOADANDDISPLAY_H__23B226B7_07DE_474C_B269_722F1FE5DD29__INCLUDED_)
#define AFX_LOADANDDISPLAY_H__23B226B7_07DE_474C_B269_722F1FE5DD29__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#ifndef __AFXWIN_H__
	#error include 'stdafx.h' before including this file for PCH
#endif

#include "resource.h"		// main symbols

/////////////////////////////////////////////////////////////////////////////
// CLoadAndDisplayApp:
// See Load And Display.cpp for the implementation of this class
//

class CLoadAndDisplayApp : public CWinApp
{
public:
	CLoadAndDisplayApp();

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CLoadAndDisplayApp)
	public:
	virtual BOOL InitInstance();
	//}}AFX_VIRTUAL

// Implementation
    
	//{{AFX_MSG(CLoadAndDisplayApp)
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};


/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_LOADANDDISPLAY_H__23B226B7_07DE_474C_B269_722F1FE5DD29__INCLUDED_)
