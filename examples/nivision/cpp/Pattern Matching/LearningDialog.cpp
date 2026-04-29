// LearningDialog.cpp : implementation file
//

#include "stdafx.h"
#include "Pattern Matching.h"
#include "LearningDialog.h"
#include <process.h>

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CLearningDialog dialog


CLearningDialog::CLearningDialog(CWnd* pParent /*=NULL*/)
	: CDialog(CLearningDialog::IDD, pParent)
{
	//{{AFX_DATA_INIT(CLearningDialog)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT
}


void CLearningDialog::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CLearningDialog)
	DDX_Control(pDX, IDC_PROGRESS_BAR, m_progress);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CLearningDialog, CDialog)
	//{{AFX_MSG_MAP(CLearningDialog)
	ON_WM_SHOWWINDOW()
	ON_WM_TIMER()
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CLearningDialog message handlers

void CLearningDialog::OnShowWindow(BOOL bShow, UINT nStatus) 
{
	CDialog::OnShowWindow(bShow, nStatus);
    if (bShow) {
        //-------------------------------------------------------------------
        //  IMAQ:  The window is being shown.  Set up the progress bar and
        //  make a timer to keep it changing.
        //-------------------------------------------------------------------
        m_progress.SetRange(minVal, maxVal);
        m_progress.SetPos(minVal);
        SetTimer(1, 125, NULL);
        //-------------------------------------------------------------------
        //  Put the image on the dialog.
        //-------------------------------------------------------------------
        Image* originalTemplate = (Image*) (GetParent()->SendMessage(WM_GETTEMPLATEIMAGE));
        templateImage = PlaceImageOnDialog(this, IDC_IMAGE_AREA, originalTemplate);
        _beginthread(LearnThread, 0, GetParent());
    }
    else {
        //-------------------------------------------------------------------
        //  Hiding the dialog.  Turn off the timer and dispose of our copy
        //  of the template
        //-------------------------------------------------------------------
        KillTimer(1);
        imaqDispose(templateImage);
    }	
}

void CLearningDialog::OnTimer(UINT_PTR nIDEvent) 
{
    //-----------------------------------------------------------------------
    //  IMAQ:  The timer fired.  If we are at the end of the progress bar,
    //  change directions.  Then change the progress bar.
    //-----------------------------------------------------------------------
    int curValue = m_progress.GetPos();
    if (curValue == maxVal)
        increment = -1;
    if (curValue == minVal)
        increment = 1;
    curValue += increment;
    m_progress.SetPos(curValue);
}


void CLearningDialog::LearnThread(void* _dialog) {
    //-----------------------------------------------------------------------
    //  The main dialog was passed to us as a void pointer.  Convert it back,
    //  then get the template image from it and learn the template.  When we
    //  are done, let the dialog know.
    //-----------------------------------------------------------------------
    CWnd* dialog = (CWnd*) _dialog;
    Image* originalTemplate = (Image*) (dialog->SendMessage(WM_GETTEMPLATEIMAGE));
    int rotation = dialog->SendMessage(WM_GETLEARNROTATION);
    int success= imaqLearnPattern(originalTemplate, rotation ? IMAQ_LEARN_ALL : IMAQ_LEARN_SHIFT_INFORMATION);
    if (!success) {
        char* err = imaqGetErrorText(imaqGetLastError());
        dialog->MessageBox(err, "Error Learning Pattern");
        imaqDispose(err);
    }
    dialog->PostMessage(WM_DONELEARNING, success);
}
