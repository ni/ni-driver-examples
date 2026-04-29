//////////////////////////////////////////////////////////////////////////////
//
//  Title     : LLSnap.cpp
//  Project   : NI-IMAQdx
//  Created   : 8/29/2006 @ 13:26:58
//  Platforms : All
//  Access    : Public
//  Purpose   : Illustrates how to perform a simple, low-level snap acquisition,
//              built using MSVC.
//
//////////////////////////////////////////////////////////////////////////////


//============================================================================
//  Includes
//============================================================================
#include "stdafx.h"
#include "LLSnap.h"
#include "LLSnapDlg.h"

//============================================================================
//  Debug defines
//============================================================================
#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

//============================================================================
//  Message map
//============================================================================
BEGIN_MESSAGE_MAP(CLLSnapApp, CWinApp)
	//{{AFX_MSG_MAP(CLLSnapApp)
		// NOTE - the ClassWizard will add and remove mapping macros here.
		//    DO NOT EDIT what you see in these blocks of generated code!
	//}}AFX_MSG
	ON_COMMAND(ID_HELP, CWinApp::OnHelp)
END_MESSAGE_MAP()

//============================================================================
//  The CLLSnapApp object
//============================================================================
CLLSnapApp theApp;

//////////////////////////////////////////////////////////////////////////////
//
//  CLLSnapApp::CLLSnapApp
//
//  Description:
//      Constructs the CLLSnapApp.
//
//////////////////////////////////////////////////////////////////////////////
CLLSnapApp::CLLSnapApp()
{
}

//////////////////////////////////////////////////////////////////////////////
//
//  CLLSnapApp::InitInstance
//
//  Description:
//      Initializes our application instance.
//
//////////////////////////////////////////////////////////////////////////////
BOOL CLLSnapApp::InitInstance()
{
    //------------------------------------------------------------------------
    //  Enable the controls, create the dialog, and let it run.
    //------------------------------------------------------------------------
	Enable3dControls();			// Call this when using MFC in a shared DLL
	CLLSnapDlg dlg;
	m_pMainWnd = &dlg;
	dlg.DoModal();
	return FALSE;
}
