//////////////////////////////////////////////////////////////////////////////
//
//  Title     : TriggeredGrab.cpp
//  Project   : NI-IMAQdx
//  Created   : 8/25/2006 @ 09:34:58
//  Platforms : All
//  Access    : Public
//  Purpose   : Illustrates how to perform a triggered, high-level grab acquisition,
//              built using MSVC.
//
//////////////////////////////////////////////////////////////////////////////


//============================================================================
//  Includes
//============================================================================
#include "stdafx.h"
#include "TriggeredGrab.h"
#include "TriggeredGrabDlg.h"

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
BEGIN_MESSAGE_MAP(CTriggeredGrabApp, CWinApp)
	//{{AFX_MSG_MAP(CTriggeredGrabApp)
		// NOTE - the ClassWizard will add and remove mapping macros here.
		//    DO NOT EDIT what you see in these blocks of generated code!
	//}}AFX_MSG
	ON_COMMAND(ID_HELP, CWinApp::OnHelp)
END_MESSAGE_MAP()

//============================================================================
//  The CTriggeredGrabApp object
//============================================================================
CTriggeredGrabApp theApp;

//////////////////////////////////////////////////////////////////////////////
//
//  CTriggeredGrabApp::CTriggeredGrabApp
//
//  Description:
//      Constructs the CTriggeredGrabApp.
//
//////////////////////////////////////////////////////////////////////////////
CTriggeredGrabApp::CTriggeredGrabApp()
{
}

//////////////////////////////////////////////////////////////////////////////
//
//  CTriggeredGrabApp::InitInstance
//
//  Description:
//      Initializes our application instance.
//
//////////////////////////////////////////////////////////////////////////////
BOOL CTriggeredGrabApp::InitInstance()
{
	Enable3dControls();			// Call this when using MFC in a shared DLL
	CTriggeredGrabDlg dlg;
	m_pMainWnd = &dlg;
	dlg.DoModal();
	return FALSE;
}
