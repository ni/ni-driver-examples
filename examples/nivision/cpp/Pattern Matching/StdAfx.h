// stdafx.h : include file for standard system include files,
//  or project specific include files that are used frequently, but
//      are changed infrequently
//

#if !defined(AFX_STDAFX_H__7E5459AB_3BB1_4F7C_ABC0_BA977F7568BF__INCLUDED_)
#define AFX_STDAFX_H__7E5459AB_3BB1_4F7C_ABC0_BA977F7568BF__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#define VC_EXTRALEAN		// Exclude rarely-used stuff from Windows headers

#include <afxwin.h>         // MFC core and standard components
#include <afxext.h>         // MFC extensions
#include <afxdtctl.h>		// MFC support for Internet Explorer 4 Common Controls
#ifndef _AFX_NO_AFXCMN_SUPPORT
#include <afxcmn.h>			// MFC support for Windows Common Controls
#endif // _AFX_NO_AFXCMN_SUPPORT



//---------------------------------------------------------------------------
//  IMAQ
//---------------------------------------------------------------------------
#include <nivision.h>
#define IMAGE_WINDOW 0
#define TEMPLATE_WINDOW 1
#define WM_REFERENCEFILEREADY WM_USER+1
#define WM_TEMPLATESELECTED WM_USER+2
#define WM_DONELEARNING WM_USER+3
#define WM_GETTEMPLATEIMAGE WM_USER+4
#define WM_GETSEARCHIMAGE WM_USER+5
#define WM_FILEDIALOG WM_USER+6
#define WM_SETTEMPLATEAREA WM_USER+7
#define WM_GETLEARNROTATION WM_USER+8

Image* PlaceImageOnDialog(CWnd* wnd, int areaID, Image* originalImage);


//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.




#endif // !defined(AFX_STDAFX_H__7E5459AB_3BB1_4F7C_ABC0_BA977F7568BF__INCLUDED_)
