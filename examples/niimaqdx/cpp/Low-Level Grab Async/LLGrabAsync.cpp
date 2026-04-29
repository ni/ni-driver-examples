//////////////////////////////////////////////////////////////////////////////
//
//  Title     : LLGrabAsync.cpp
//  Project   : NI-IMAQdx
//  Created   : 8/29/2006 @ 18:08:58
//  Platforms : All
//  Access    : Public
//  Purpose   : Illustrates how to perform an asynchronous, low-level grab 
//              acquisition, built using MSVC.
//
//////////////////////////////////////////////////////////////////////////////


//============================================================================
//  Includes
//============================================================================
#include "stdafx.h"
#include "LLGrabAsync.h"
#include "LLGrabAsyncDlg.h"

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
BEGIN_MESSAGE_MAP(CLLGrabAsyncApp, CWinApp)
	//{{AFX_MSG_MAP(CLLGrabAsyncApp)
		// NOTE - the ClassWizard will add and remove mapping macros here.
		//    DO NOT EDIT what you see in these blocks of generated code!
	//}}AFX_MSG
	ON_COMMAND(ID_HELP, CWinApp::OnHelp)
END_MESSAGE_MAP()

//============================================================================
//  The CLLGrabAsyncApp object
//============================================================================
CLLGrabAsyncApp theApp;

//////////////////////////////////////////////////////////////////////////////
//
//  CLLGrabAsyncApp::CLLGrabAsyncApp
//
//  Description:
//      Constructs the CLLGrabAsyncApp.
//
//////////////////////////////////////////////////////////////////////////////
CLLGrabAsyncApp::CLLGrabAsyncApp()
{
}

//////////////////////////////////////////////////////////////////////////////
//
//  CLLGrabAsyncApp::InitInstance
//
//  Description:
//      Initializes our application instance.
//
//////////////////////////////////////////////////////////////////////////////
BOOL CLLGrabAsyncApp::InitInstance()
{
	Enable3dControls();			// Call this when using MFC in a shared DLL
	CLLGrabAsyncDlg dlg;
	m_pMainWnd = &dlg;
	dlg.DoModal();
	return FALSE;
}
