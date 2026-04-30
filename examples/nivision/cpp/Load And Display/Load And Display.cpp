// Load And Display.cpp : Defines the class behaviors for the application.
//

#include "stdafx.h"
#include "Load And Display.h"
#include "Load And DisplayDlg.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

#include <nivision.h>

/////////////////////////////////////////////////////////////////////////////
// CLoadAndDisplayApp

BEGIN_MESSAGE_MAP(CLoadAndDisplayApp, CWinApp)
	//{{AFX_MSG_MAP(CLoadAndDisplayApp)
	//}}AFX_MSG
	ON_COMMAND(ID_HELP, CWinApp::OnHelp)
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CLoadAndDisplayApp construction

CLoadAndDisplayApp::CLoadAndDisplayApp()
{
}

/////////////////////////////////////////////////////////////////////////////
// The one and only CLoadAndDisplayApp object

CLoadAndDisplayApp theApp;

/////////////////////////////////////////////////////////////////////////////
// CLoadAndDisplayApp initialization

BOOL CLoadAndDisplayApp::InitInstance()
{
	// Standard initialization

#ifdef _AFXDLL
	Enable3dControls();			// Call this when using MFC in a shared DLL
#else
	Enable3dControlsStatic();	// Call this when linking to MFC statically
#endif
	CLoadAndDisplayDlg dlg;
	m_pMainWnd = &dlg;
	int nResponse = dlg.DoModal();
    CWnd::DeleteTempMap();
	if (nResponse == IDOK)
	{
	}
	else if (nResponse == IDCANCEL)
	{
	}
	// Since the dialog has been closed, return FALSE so that we exit the
	//  application, rather than start the application's message pump.
	return FALSE;
}
