//////////////////////////////////////////////////////////////////////////////
//
//  Title     : GrabBasicAttr.cpp
//  Project   : NI-IMAQdx
//  Created   : 2/12/2008 @ 10:19:26
//  Platforms : All
//  Access    : Public
//  Purpose   : Illustrates how to perform a simple, high-level grab acquisition,
//              with basic attribute support built using MSVC.
//
//////////////////////////////////////////////////////////////////////////////


//============================================================================
//  Includes
//============================================================================
#include "stdafx.h"
#include "GrabBasicAttr.h"
#include "GrabBasicAttrDlg.h"

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
BEGIN_MESSAGE_MAP(CGrabBasicAttrApp, CWinApp)
	//{{AFX_MSG_MAP(CGrabBasicAttrApp)
		// NOTE - the ClassWizard will add and remove mapping macros here.
		//    DO NOT EDIT what you see in these blocks of generated code!
	//}}AFX_MSG
	ON_COMMAND(ID_HELP, CWinApp::OnHelp)
END_MESSAGE_MAP()

//============================================================================
//  The CGrabBasicAttrApp object
//============================================================================
CGrabBasicAttrApp theApp;

//////////////////////////////////////////////////////////////////////////////
//
//  CGrabBasicAttrApp::CGrabBasicAttrApp
//
//  Description:
//      Constructs the CGrabBasicAttrApp.
//
//////////////////////////////////////////////////////////////////////////////
CGrabBasicAttrApp::CGrabBasicAttrApp()
{
}

//////////////////////////////////////////////////////////////////////////////
//
//  CGrabBasicAttrApp::InitInstance
//
//  Description:
//      Initializes our application instance.
//
//////////////////////////////////////////////////////////////////////////////
BOOL CGrabBasicAttrApp::InitInstance()
{
    //------------------------------------------------------------------------
    //  Enable the controls, create the dialog, and let it run.
    //------------------------------------------------------------------------
	Enable3dControls();			// Call this when using MFC in a shared DLL
	CGrabBasicAttrDlg dlg;
	m_pMainWnd = &dlg;
	dlg.DoModal();
	return FALSE;
}
