// ChooseTemplateDialog.cpp : implementation file
//

#include "stdafx.h"
#include "Pattern Matching.h"
#include "ChooseTemplateDialog.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

#include <nivision.h>

//---------------------------------------------------------------------------
//  IMAQ:  Local Variables & Functions
//---------------------------------------------------------------------------
static CChooseTemplateDialog* thisDialog;
static void RectDrawnProc(WindowEventType, int, Tool, Rect);


/////////////////////////////////////////////////////////////////////////////
// CChooseTemplateDialog dialog


CChooseTemplateDialog::CChooseTemplateDialog(CWnd* pParent /*=NULL*/)
	: CDialog(CChooseTemplateDialog::IDD, pParent)
{
	//{{AFX_DATA_INIT(CChooseTemplateDialog)
	m_rotation = TRUE;
	//}}AFX_DATA_INIT
}


void CChooseTemplateDialog::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CChooseTemplateDialog)
	DDX_Control(pDX, IDC_SEARCH, m_ok);
	DDX_Check(pDX, IDC_ROTATION, m_rotation);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CChooseTemplateDialog, CDialog)
	//{{AFX_MSG_MAP(CChooseTemplateDialog)
	ON_WM_SHOWWINDOW()
	ON_BN_CLICKED(IDC_SEARCH, OnOK)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CChooseTemplateDialog message handlers

void CChooseTemplateDialog::OnShowWindow(BOOL bShow, UINT nStatus) 
{
	CDialog::OnShowWindow(bShow, nStatus);
	
    //-----------------------------------------------------------------------
    //  IMAQ:  If the window is being shown, then show the tool palette next
    //  to the image window and select the rectangle tool.  Set up the event
    //  callback to call us when there is a draw event.
    //-----------------------------------------------------------------------
    if (bShow) {
        templateImage = NULL;
        m_ok.EnableWindow(FALSE);
        ToolWindowOptions twOptions;
        memset(&twOptions, 0, sizeof(ToolWindowOptions));
        twOptions.showRectangleTool = TRUE;
        imaqSetupToolWindow(TRUE, 4, &twOptions);
        Point imagePos;
        int imageWidth;
        imaqGetWindowPos(IMAGE_WINDOW, &imagePos);
        imaqGetWindowSize(IMAGE_WINDOW, &imageWidth, NULL);
        imagePos.x += imageWidth;
        imaqMoveToolWindow(imagePos);
        imaqShowToolWindow(TRUE);
        imaqSetCurrentTool(IMAQ_RECTANGLE_TOOL);
        thisDialog = this;
        imaqSetEventCallback(RectDrawnProc, TRUE);
    }    
    else {
        imaqDispose(templateImage);
        templateImage = NULL;
    }
}


void CChooseTemplateDialog::SelectRect(Rect rect) {
    //-----------------------------------------------------------------------
    //  IMAQ:  Set the template area in the main dialog, then enable our OK
    //  button and show the template.
    //-----------------------------------------------------------------------
    GetParent()->SendMessage(WM_SETTEMPLATEAREA, (WPARAM) &rect);
    imaqDispose(templateImage);
    Image* originalTemplate = (Image*) (GetParent()->SendMessage(WM_GETTEMPLATEIMAGE));
    PlaceImageOnDialog(this, IDC_IMAGE_AREA, originalTemplate);
    m_ok.EnableWindow(TRUE);
}


static void RectDrawnProc(WindowEventType event, int windowNumber, Tool, Rect selectionArea) {
    //-----------------------------------------------------------------------
    //  IMAQ:  If we get a draw event on the image window, then let the
    //  ChooseTemplateDialog know.
    //-----------------------------------------------------------------------
    if (event == IMAQ_DRAW_EVENT && windowNumber == IMAGE_WINDOW) {
        thisDialog->SelectRect(selectionArea);
    }
}

void CChooseTemplateDialog::OnOK() 
{
    //-----------------------------------------------------------------------
    //  IMAQ:  Turn off the tools palette & get rid of the window ROI, then 
    //  let the  parent dialog know we like our choice.    
    //-----------------------------------------------------------------------
    UpdateData(TRUE);
    imaqSetEventCallback(NULL, FALSE);
    imaqShowToolWindow(FALSE);
    imaqSetWindowROI(IMAGE_WINDOW, NULL);
    GetParent()->SendMessage(WM_TEMPLATESELECTED, m_rotation);
}
