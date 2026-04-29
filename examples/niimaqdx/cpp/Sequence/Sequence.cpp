//////////////////////////////////////////////////////////////////////////////
//
//  Title     : Sequence.cpp
//  Project   : NI-IMAQdx
//  Created   : 8/24/2006 @ 22:52:13
//  Platforms : All
//  Access    : Public
//  Purpose   : Demonstrates a simple, high-level acquisition of a sequence 
//				of images, built using MSVC.
//
//////////////////////////////////////////////////////////////////////////////


//============================================================================
//  Includes
//============================================================================
#include "stdafx.h"
#include "Sequence.h"
#include "SequenceDlg.h"

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
BEGIN_MESSAGE_MAP(CSequenceApp, CWinApp)
	//{{AFX_MSG_MAP(CSequenceApp)
		// NOTE - the ClassWizard will add and remove mapping macros here.
		//    DO NOT EDIT what you see in these blocks of generated code!
	//}}AFX_MSG
	ON_COMMAND(ID_HELP, CWinApp::OnHelp)
END_MESSAGE_MAP()

//============================================================================
//  The CSequenceApp object.
//============================================================================
CSequenceApp theApp;

//////////////////////////////////////////////////////////////////////////////
//
//  CSequenceApp::CSequenceApp
//
//  Description:
//      Constructs the main application context.
//
//////////////////////////////////////////////////////////////////////////////
CSequenceApp::CSequenceApp()
{
}

//////////////////////////////////////////////////////////////////////////////
//
//  CSequenceApp::InitInstance
//
//  Description:
//      Main initialization routine of the application.
//
//////////////////////////////////////////////////////////////////////////////
BOOL CSequenceApp::InitInstance()
{
    //------------------------------------------------------------------------
    //  Enable the controls, create the dialog, and let it run.
    //------------------------------------------------------------------------
	Enable3dControls();			// Call this when using MFC in a shared DLL
	CSequenceDlg dlg;
	m_pMainWnd = &dlg;
	dlg.DoModal();
	return FALSE;
}
