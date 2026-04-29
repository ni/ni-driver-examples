// MatchingDialog.cpp : implementation file
//

#include "stdafx.h"
#include "Pattern Matching.h"
#include "MatchingDialog.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif


static const struct {
    char* name;
    RGBValue value;
} colors[] = {
    {"Red", {0, 0, 255, 0}},
    {"Green", {0, 255, 0, 0}},
    {"Blue", {255, 0, 0, 0}},
    {"Orange", {0, 128, 255, 0}},
    {"Yellow", {0, 255, 255, 0}},
    {"Purple", {160, 0, 160, 0}},
    {"Magenta", {255, 128, 255, 0}},
    {"Pink", {202, 202, 255, 0}},
    {"Aqua", {222, 238, 77, 0}},
    {"Brown", {71, 120, 146, 0}}
};

static const int numColors = sizeof(colors) / sizeof(*colors);


/////////////////////////////////////////////////////////////////////////////
// CMatchingDialog dialog


CMatchingDialog::CMatchingDialog(CWnd* pParent /*=NULL*/)
	: CDialog(CMatchingDialog::IDD, pParent)
{
	//{{AFX_DATA_INIT(CMatchingDialog)
	m_matchesValue = 1;
	m_scoreValue = 900;
	m_contrastValue = 10;
	m_subpixel = FALSE;
	m_matchesFound = _T("");
	//}}AFX_DATA_INIT
}


void CMatchingDialog::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CMatchingDialog)
	DDX_Control(pDX, IDC_ROTATION, m_rotation);
	DDX_Control(pDX, IDC_RESULTS, m_resultsList);
	DDX_Control(pDX, IDC_MATCHES_SPIN, m_matchesSpin);
	DDX_Control(pDX, IDC_CONTRAST_SPIN, m_contrastSpin);
	DDX_Control(pDX, IDC_SCORE_SPIN, m_scoreSpin);
	DDX_Text(pDX, IDC_MATCHES_VALUE, m_matchesValue);
	DDV_MinMaxUInt(pDX, m_matchesValue, 0, 999);
	DDX_Text(pDX, IDC_SCORE_VALUE, m_scoreValue);
	DDV_MinMaxUInt(pDX, m_scoreValue, 0, 1000);
	DDX_Text(pDX, IDC_CONTRAST_VALUE, m_contrastValue);
	DDV_MinMaxUInt(pDX, m_contrastValue, 0, 255);
	DDX_Check(pDX, IDC_SUBPIXEL, m_subpixel);
	DDX_Text(pDX, IDC_MATCHESFOUND, m_matchesFound);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CMatchingDialog, CDialog)
	//{{AFX_MSG_MAP(CMatchingDialog)
	ON_WM_SHOWWINDOW()
	ON_BN_CLICKED(IDC_SEARCH, PerformSearch)
	ON_BN_CLICKED(IDC_NEWIMAGE, OnNewimage)
	ON_WM_TIMER()
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CMatchingDialog message handlers


//---------------------------------------------------------------------------
//  IMAQ:  All code below this point is new.
//---------------------------------------------------------------------------
BOOL CMatchingDialog::OnInitDialog() 
{
	CDialog::OnInitDialog();

    //-----------------------------------------------------------------------
    //  Set up the spin controls and the rotation checkbox.
    //-----------------------------------------------------------------------
    m_matchesSpin.SetRange(0, 1000);
	m_contrastSpin.SetRange(0, 255);
	m_scoreSpin.SetRange(0, 1000);
    m_rotation.SetCheck(1);
    //-----------------------------------------------------------------------
    //  Add the columns to the results indicator, then size the columns.
    //-----------------------------------------------------------------------
    m_resultsList.InsertColumn(0, "Color");
    m_resultsList.InsertColumn(1, "Score", LVCFMT_RIGHT);
    m_resultsList.InsertColumn(2, "Rotation", LVCFMT_RIGHT);
    SizeColumns();
	return TRUE;  // return TRUE unless you set the focus to a control
	              // EXCEPTION: OCX Property Pages should return FALSE
}

void CMatchingDialog::OnShowWindow(BOOL bShow, UINT nStatus) 
{
	CDialog::OnShowWindow(bShow, nStatus);
    if (bShow) {
        //-------------------------------------------------------------------
        // Showing.  Enable or disable the rotation flag.  Then queue up a
        // timer request to perform the initial search.
        //-------------------------------------------------------------------
        if (GetParent()->SendMessage(WM_GETLEARNROTATION)) {
            m_rotation.EnableWindow(TRUE);
        }
        else {
            m_rotation.SetCheck(0);
            m_rotation.EnableWindow(FALSE);
        }
        SetTimer(2, 1, NULL);
    }	
}


void CMatchingDialog::PerformSearch() {
    imaqSetWindowOverlay(IMAGE_WINDOW, NULL);
    HCURSOR oldCursor = GetCursor();
    SetCursor(LoadCursor(NULL, IDC_WAIT));
    //-----------------------------------------------------------------------
    //  Get the images from the main dialog
    //-----------------------------------------------------------------------
    Image* templateImage = (Image*) (GetParent()->SendMessage(WM_GETTEMPLATEIMAGE));
    Image* searchImage= (Image*) (GetParent()->SendMessage(WM_GETSEARCHIMAGE));
    //-----------------------------------------------------------------------
    //  Set up the match options and the search rect.
    //-----------------------------------------------------------------------
    UpdateData(TRUE);
    Rect rect = IMAQ_NO_RECT;
    MatchPatternOptions options;
    options.mode = m_rotation.GetCheck() ? IMAQ_MATCH_ROTATION_INVARIANT : IMAQ_MATCH_SHIFT_INVARIANT;
    options.minContrast = m_contrastValue;
    options.subpixelAccuracy = m_subpixel;
    options.angleRanges = NULL;
    options.numRanges = 0;
    options.numMatchesRequested = m_matchesValue;
    options.matchFactor = 0;
    options.minMatchScore = (float) m_scoreValue;
    //-----------------------------------------------------------------------
    //  Perform the matching.
    //-----------------------------------------------------------------------
    int numMatches = 0;
    PatternMatch* matchInfo = imaqMatchPattern(searchImage, templateImage, &options, rect, &numMatches);
    if (!matchInfo) {
        char* err = imaqGetErrorText(imaqGetLastError());
        GetParent()->MessageBox(err, "Error Matching Pattern");
        imaqDispose(err);
    }
    //-----------------------------------------------------------------------
    //  Update the results.  This includes the number of matches indicator,
    //  the report indicator, and drawing the matches on the image.
    //-----------------------------------------------------------------------
    UpdateMatchesFound(numMatches);
    UpdateResultsList(matchInfo, numMatches);
    UpdateOverlay(matchInfo, numMatches);
    imaqDispose(matchInfo);
    SetCursor(oldCursor);
}



void CMatchingDialog::UpdateMatchesFound(int numMatches) {
    m_matchesFound.Format("%d", numMatches);
    UpdateData(FALSE);
}


void CMatchingDialog::UpdateResultsList(PatternMatch* matchInfo, int numMatches) {
    //-----------------------------------------------------------------------
    //  Start off by emptying the list.  Then add each item.  After doing so
    //  resize the columns.
    //-----------------------------------------------------------------------
    m_resultsList.DeleteAllItems();
    for (int i = 0; i < numMatches; ++i) {
        char buf[256];
        m_resultsList.InsertItem(i, colors[i % numColors].name);
        sprintf(buf, "%.0f", matchInfo[i].score);
        m_resultsList.SetItemText(i, 1, buf);
        sprintf(buf, "%.1f", matchInfo[i].rotation);
        m_resultsList.SetItemText(i, 2, buf);
    }    
    SizeColumns();
}


void CMatchingDialog::UpdateOverlay(PatternMatch * matchInfo, int numMatches) {
    ROI* roi = imaqCreateROI();
    //-----------------------------------------------------------------------
    //  Go over each match.  Add a contour for each match into the ROI.
    //-----------------------------------------------------------------------
    for (int i = 0; i < numMatches; ++i) {
        //-------------------------------------------------------------------
        //  Make a two pixel wide rect around each match.
        //-------------------------------------------------------------------
        Point rect[9];
        for (int j = 0; j < 4; ++j) {
            rect[j].x = (int)(matchInfo[i].corner[j].x + 0.5);
            rect[j].y = (int)(matchInfo[i].corner[j].y + 0.5);
        }
        rect[5] = rect[0];
        int delta;
        delta = (rect[0].x < rect[2].x) ? 1 : -1;
        rect[4].x = rect[0].x - delta;
        rect[6].x = rect[2].x + delta;
        delta = (rect[0].y < rect[2].y) ? 1 : -1;
        rect[4].y = rect[0].y - delta;
        rect[6].y = rect[2].y + delta;
        delta = (rect[1].x < rect[3].x) ? 1 : -1;
        rect[5].x = rect[1].x - delta;
        rect[7].x = rect[3].x + delta;
        delta = (rect[1].y < rect[3].y) ? 1 : -1;
        rect[5].y = rect[1].y - delta;
        rect[7].y = rect[3].y + delta;
        //-------------------------------------------------------------------
        //  Add a contour for the box, then change its color to what we want.
        //-------------------------------------------------------------------
        ContourID id = imaqAddClosedContour(roi, rect, 8);
        imaqSetContourColor(roi, id, &(colors[i % numColors].value));
    }
    //-----------------------------------------------------------------------
    //  Now we have the ROI built up.  Change the ROI into an overlay and add
    //  it to the window.
    //-----------------------------------------------------------------------
    Overlay* overlay = imaqCreateOverlayFromROI(roi);
    imaqSetWindowOverlay(IMAGE_WINDOW, overlay);
    imaqDispose(overlay);
    imaqDispose(roi);

}


void CMatchingDialog::SizeColumns() {
    //-----------------------------------------------------------------------
    //  Calculate the total width.
    //-----------------------------------------------------------------------
    m_resultsList.SetColumnWidth(0, LVSCW_AUTOSIZE_USEHEADER);
    m_resultsList.SetColumnWidth(1, LVSCW_AUTOSIZE_USEHEADER);
    m_resultsList.SetColumnWidth(2, LVSCW_AUTOSIZE_USEHEADER);
    int width = 0;
    for (int i = 0; i < 3; ++i)
        width += m_resultsList.GetColumnWidth(i);
    //-----------------------------------------------------------------------
    //  Set the first two columns to one third that value.  Make the third
    //  column what's leftover (must calculate to avoid roundoff problems).
    //-----------------------------------------------------------------------
    m_resultsList.SetColumnWidth(0, width / 3);
    m_resultsList.SetColumnWidth(1, width / 3);
    m_resultsList.SetColumnWidth(2, width - width / 3 - width / 3);
}


void CMatchingDialog::OnNewimage() 
{
    //-----------------------------------------------------------------------
    //  Ask the main dialog to load and display a new file.
    //-----------------------------------------------------------------------
    if (GetParent()->SendMessage(WM_FILEDIALOG))
        PerformSearch();
}

void CMatchingDialog::OnTimer(UINT_PTR nIDEvent) 
{
    //-----------------------------------------------------------------------
	//  Get rid of the timer (to act one-shot) and then match.
    //-----------------------------------------------------------------------
    KillTimer(nIDEvent);
    PerformSearch();
	CDialog::OnTimer(nIDEvent);
}
