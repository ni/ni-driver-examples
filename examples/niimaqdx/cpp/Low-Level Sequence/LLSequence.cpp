//////////////////////////////////////////////////////////////////////////////
//
//  Title     : LLSequence.cpp
//  Project   : NI-IMAQdx
//  Created   : 8/29/2006 @ 14:33:13
//  Platforms : All
//  Access    : Public
//  Purpose   : Demonstrates a simple, low-level acquisition of a sequence 
//				of images, built using MSVC.
//
//////////////////////////////////////////////////////////////////////////////


//============================================================================
//  Includes
//============================================================================
#include "stdafx.h"
#include "LLSequence.h"
#include "LLSequenceDlg.h"

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
BEGIN_MESSAGE_MAP(CLLSequenceApp, CWinApp)
	//{{AFX_MSG_MAP(CLLSequenceApp)
		// NOTE - the ClassWizard will add and remove mapping macros here.
		//    DO NOT EDIT what you see in these blocks of generated code!
	//}}AFX_MSG
	ON_COMMAND(ID_HELP, CWinApp::OnHelp)
END_MESSAGE_MAP()

//============================================================================
//  The CLLSequenceApp object.
//============================================================================
CLLSequenceApp theApp;

//////////////////////////////////////////////////////////////////////////////
//
//  CLLSequenceApp::CLLSequenceApp
//
//  Description:
//      Constructs the main application context.
//
//////////////////////////////////////////////////////////////////////////////
CLLSequenceApp::CLLSequenceApp()
{
}

//////////////////////////////////////////////////////////////////////////////
//
//  CLLSequenceApp::InitInstance
//
//  Description:
//      Main initialization routine of the application.
//
//////////////////////////////////////////////////////////////////////////////
BOOL CLLSequenceApp::InitInstance()
{
	Enable3dControls();			// Call this when using MFC in a shared DLL
	CLLSequenceDlg dlg;
	m_pMainWnd = &dlg;
	dlg.DoModal();
	return FALSE;
}
