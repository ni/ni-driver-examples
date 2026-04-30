//////////////////////////////////////////////////////////////////////////////
//
//  Title     : Grab.cpp
//  Project   : NI-IMAQdx
//  Created   : 8/25/2006 @ 09:34:58
//  Platforms : All
//  Access    : Public
//  Purpose   : Illustrates how to perform a simple, high-level grab acquisition,
//              built using MSVC.
//
//////////////////////////////////////////////////////////////////////////////


//============================================================================
//  Includes
//============================================================================
#include "stdafx.h"
#include "Grab.h"
#include "GrabDlg.h"

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
BEGIN_MESSAGE_MAP(CGrabApp, CWinApp)
	//{{AFX_MSG_MAP(CGrabApp)
		// NOTE - the ClassWizard will add and remove mapping macros here.
		//    DO NOT EDIT what you see in these blocks of generated code!
	//}}AFX_MSG
	ON_COMMAND(ID_HELP, CWinApp::OnHelp)
END_MESSAGE_MAP()

//============================================================================
//  The CGrabApp object
//============================================================================
CGrabApp theApp;

//////////////////////////////////////////////////////////////////////////////
//
//  CGrabApp::CGrabApp
//
//  Description:
//      Constructs the CGrabApp.
//
//////////////////////////////////////////////////////////////////////////////
CGrabApp::CGrabApp()
{
}

//////////////////////////////////////////////////////////////////////////////
//
//  CGrabApp::InitInstance
//
//  Description:
//      Initializes our application instance.
//
//////////////////////////////////////////////////////////////////////////////
BOOL CGrabApp::InitInstance()
{
    //------------------------------------------------------------------------
    //  Enable the controls, create the dialog, and let it run.
    //------------------------------------------------------------------------
	Enable3dControls();			// Call this when using MFC in a shared DLL
	CGrabDlg dlg;
	m_pMainWnd = &dlg;
	dlg.DoModal();
	return FALSE;
}
