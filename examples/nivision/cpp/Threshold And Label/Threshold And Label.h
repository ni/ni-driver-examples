// Threshold And Label.h : main header file for the THRESHOLD AND LABEL application
//

#if !defined(AFX_THRESHOLDANDLABEL_H__26EEC73C_B66F_47C6_BE82_23CE2A7B3D46__INCLUDED_)
#define AFX_THRESHOLDANDLABEL_H__26EEC73C_B66F_47C6_BE82_23CE2A7B3D46__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#ifndef __AFXWIN_H__
	#error include 'stdafx.h' before including this file for PCH
#endif

#include "resource.h"		// main symbols

/////////////////////////////////////////////////////////////////////////////
// CThresholdAndLabelApp:
// See Threshold And Label.cpp for the implementation of this class
//

class CThresholdAndLabelApp : public CWinApp
{
public:
	CThresholdAndLabelApp();

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CThresholdAndLabelApp)
	public:
	virtual BOOL InitInstance();
	//}}AFX_VIRTUAL

// Implementation
	//{{AFX_MSG(CThresholdAndLabelApp)
		// NOTE - the ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};


/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_THRESHOLDANDLABEL_H__26EEC73C_B66F_47C6_BE82_23CE2A7B3D46__INCLUDED_)
