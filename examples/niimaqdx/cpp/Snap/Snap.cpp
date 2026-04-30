//////////////////////////////////////////////////////////////////////////////
//
//  Title     : Snap.cpp
//  Project   : NI-IMAQdx
//  Created   : 8/24/2006 @ 22:52:13
//  Platforms : All
//  Access    : Public
//  Purpose   : Demonstrates a simple, high-level snap acquisition, built using
//              MSVC.
//
//////////////////////////////////////////////////////////////////////////////


//============================================================================
//  Includes
//============================================================================
#include "stdafx.h"
#include "Snap.h"
#include "SnapDlg.h"

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
BEGIN_MESSAGE_MAP(CSnapApp, CWinApp)
	//{{AFX_MSG_MAP(CSnapApp)
		// NOTE - the ClassWizard will add and remove mapping macros here.
		//    DO NOT EDIT what you see in these blocks of generated code!
	//}}AFX_MSG
	ON_COMMAND(ID_HELP, CWinApp::OnHelp)
END_MESSAGE_MAP()

//============================================================================
//  The CSnapApp object.
//============================================================================
CSnapApp::CSnapApp()
{
	// TODO: add construction code here,
	// Place all significant initialization in InitInstance
}

//////////////////////////////////////////////////////////////////////////////
//
//  CSnapApp::CSnapApp
//
//  Description:
//      Constructs the main application context.
//
//////////////////////////////////////////////////////////////////////////////
CSnapApp theApp;

//////////////////////////////////////////////////////////////////////////////
//
//  CHLSnapApp::InitInstance
//
//  Description:
//      Main initialization routine of the application.
//
//////////////////////////////////////////////////////////////////////////////
BOOL CSnapApp::InitInstance()
{

    //------------------------------------------------------------------------
    //  Enable the controls, create the dialog, and let it run.
    //------------------------------------------------------------------------
	Enable3dControls();	
	CSnapDlg dlg;
	m_pMainWnd = &dlg;
	dlg.DoModal();
	return FALSE;
}
