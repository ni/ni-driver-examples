//////////////////////////////////////////////////////////////////////////////
//
//  Title     : LLGrab.cpp
//  Project   : NI-IMAQdx
//  Created   : 8/29/2006 @ 10:30:58
//  Platforms : All
//  Access    : Public
//  Purpose   : Illustrates how to perform a simple, low-level grab acquisition,
//              built using MSVC.
//
//////////////////////////////////////////////////////////////////////////////


//============================================================================
//  Includes
//============================================================================
#include "stdafx.h"
#include "LLGrab.h"
#include "LLGrabDlg.h"

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
BEGIN_MESSAGE_MAP(CLLGrabApp, CWinApp)
	//{{AFX_MSG_MAP(CLLGrabApp)
		// NOTE - the ClassWizard will add and remove mapping macros here.
		//    DO NOT EDIT what you see in these blocks of generated code!
	//}}AFX_MSG
	ON_COMMAND(ID_HELP, CWinApp::OnHelp)
END_MESSAGE_MAP()

//============================================================================
//  The CLLGrabApp object
//============================================================================
CLLGrabApp theApp;

//////////////////////////////////////////////////////////////////////////////
//
//  CLLGrabApp::CLLGrabApp
//
//  Description:
//      Constructs the CLLGrabApp.
//
//////////////////////////////////////////////////////////////////////////////
CLLGrabApp::CLLGrabApp()
{
}

//////////////////////////////////////////////////////////////////////////////
//
//  CLLGrabApp::InitInstance
//
//  Description:
//      Initializes our application instance.
//
//////////////////////////////////////////////////////////////////////////////
BOOL CLLGrabApp::InitInstance()
{
    //------------------------------------------------------------------------
    //  Enable the controls, create the dialog, and let it run.
    //------------------------------------------------------------------------
	Enable3dControls();			// Call this when using MFC in a shared DLL
	CLLGrabDlg dlg;
	m_pMainWnd = &dlg;
	dlg.DoModal();
	return FALSE;
}
