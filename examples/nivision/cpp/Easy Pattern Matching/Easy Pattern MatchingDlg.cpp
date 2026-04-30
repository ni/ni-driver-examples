// Easy Pattern MatchingDlg.cpp : implementation file
//

#include "stdafx.h"
#include "Easy Pattern Matching.h"
#include "Easy Pattern MatchingDlg.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif


//---------------------------------------------------------------------------
//  IMAQ:  GetImageDirectory sets up a logical default directory.  If you
//  move your executable, you will want to change this function.
//---------------------------------------------------------------------------
static const char* GetImageDirectory() {
    static char dir[512];
    if (*dir)
        return dir;
    GetModuleFileName(NULL, dir, 512);
    char* endOfPath = strrchr(dir, '\\') + 1;
    strcpy(endOfPath, "..\\Images");
    return dir;
}



/////////////////////////////////////////////////////////////////////////////
// CEasyPatternMatchingDlg dialog

CEasyPatternMatchingDlg::CEasyPatternMatchingDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CEasyPatternMatchingDlg::IDD, pParent)
{
	//{{AFX_DATA_INIT(CEasyPatternMatchingDlg)
	m_matchScore = _T("750");
	m_rotationInvariance = FALSE;
	m_maxMatches = _T("1");
	m_matchesText = _T("");
	//}}AFX_DATA_INIT
	// Note that LoadIcon does not require a subsequent DestroyIcon in Win32
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CEasyPatternMatchingDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CEasyPatternMatchingDlg)
	DDX_Control(pDX, IDC_LEARNING_TEXT, m_learningText);
	DDX_Control(pDX, ID_DO_SEARCH, m_doSearchButton);
	DDX_Control(pDX, ID_LEARN_TEMPLATE, m_learnTemplateButton);
	DDX_Control(pDX, ID_LOAD_IMAGES, m_loadImagesButton);
	DDX_Text(pDX, IDC_MATCH_SCORE, m_matchScore);
	DDV_MaxChars(pDX, m_matchScore, 4);
	DDX_Check(pDX, IDC_ROTATION_INVARIANCE, m_rotationInvariance);
	DDX_Text(pDX, IDC_MAX_MATCHES, m_maxMatches);
	DDX_Text(pDX, IDC_MATCHES_TEXT, m_matchesText);
	//}}AFX_DATA_MAP
}

BEGIN_MESSAGE_MAP(CEasyPatternMatchingDlg, CDialog)
	//{{AFX_MSG_MAP(CEasyPatternMatchingDlg)
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_BN_CLICKED(ID_LOAD_IMAGES, OnLoadImages)
	ON_BN_CLICKED(ID_LEARN_TEMPLATE, OnLearnTemplate)
	ON_BN_CLICKED(ID_DO_SEARCH, OnDoSearch)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CEasyPatternMatchingDlg message handlers

BOOL CEasyPatternMatchingDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	// Set the icon for this dialog.  The framework does this automatically
	//  when the application's main window is not a dialog
	SetIcon(m_hIcon, TRUE);			// Set big icon
	SetIcon(m_hIcon, FALSE);		// Set small icon
	
	//-----------------------------------------------------------------------
    //  IMAQ:  Initialize our images.
	//-----------------------------------------------------------------------
    templateImage = imaqCreateImage(IMAQ_IMAGE_U8, 0);
    searchImage = imaqCreateImage(IMAQ_IMAGE_U8, 0);
	
	return TRUE;  // return TRUE  unless you set the focus to a control
}

// If you add a minimize button to your dialog, you will need the code below
//  to draw the icon.  For MFC applications using the document/view model,
//  this is automatically done for you by the framework.

void CEasyPatternMatchingDlg::OnPaint() 
{
	if (IsIconic())
	{
		CPaintDC dc(this); // device context for painting

		SendMessage(WM_ICONERASEBKGND, (WPARAM) dc.GetSafeHdc(), 0);

		// Center icon in client rectangle
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// Draw the icon
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialog::OnPaint();
	}
}

// The system calls this to obtain the cursor to display while the user drags
//  the minimized window.
HCURSOR CEasyPatternMatchingDlg::OnQueryDragIcon()
{
	return (HCURSOR) m_hIcon;
}


//-----------------------------------------------------------------------
//  IMAQ:  From here down is new code.
//-----------------------------------------------------------------------

void CEasyPatternMatchingDlg::OnLoadImages() 
{
    //-----------------------------------------------------------------------
    //  Load the template and search images.
    //-----------------------------------------------------------------------
    char templateFile[512];
    char searchFile[512];
    strcpy(templateFile, GetImageDirectory());
    strcat(templateFile, "\\pmtemplate.png");
    strcpy(searchFile, GetImageDirectory());
    strcat(searchFile, "\\pcb03-06.png");
    if (!imaqReadFile(templateImage, templateFile, NULL, NULL)) {
        MessageBox("Unable to Read the Template File", "Error", MB_OK | MB_ICONEXCLAMATION);
        EndDialog(0);
    }    
    if (!imaqReadFile(searchImage, searchFile, NULL, NULL)) {
        MessageBox("Unable to Read the Search File", "Error", MB_OK | MB_ICONEXCLAMATION);
        EndDialog(0);
    }    
    //-----------------------------------------------------------------------
    //  Display them in a nice way.
    //-----------------------------------------------------------------------
    imaqSetWindowSize(TEMPLATE_WINDOW, 200, 100);
    imaqMoveWindow(TEMPLATE_WINDOW, imaqMakePoint(488, 1));
    imaqSetWindowTitle(TEMPLATE_WINDOW, "Template Image");
    imaqDisplayImage(templateImage, TEMPLATE_WINDOW, FALSE);
    imaqMoveWindow(SEARCH_WINDOW, imaqMakePoint(1, 258));
    imaqSetWindowTitle(SEARCH_WINDOW, "Search Image");
    imaqDisplayImage(searchImage, SEARCH_WINDOW, TRUE);
    //-----------------------------------------------------------------------
    //  Make the next button in the sequence active.
    //-----------------------------------------------------------------------
    m_loadImagesButton.EnableWindow(FALSE);
    m_learnTemplateButton.EnableWindow(TRUE);
}


void CEasyPatternMatchingDlg::OnLearnTemplate() 
{
    //-----------------------------------------------------------------------
    //  Learn the template.  Since this takes a while put up text to that
    //  effect and set the wait cursor.
    //-----------------------------------------------------------------------
    HCURSOR oldCursor = GetCursor();
    SetCursor(LoadCursor(NULL, IDC_WAIT));
    m_learningText.ShowWindow(SW_SHOW);
    m_learningText.SetWindowText("Learning.  This may take up to 20 seconds.");
    m_learningText.UpdateWindow();
    UpdateWindow();
    imaqLearnPattern(templateImage, IMAQ_LEARN_ALL);
    //-----------------------------------------------------------------------
    //  Done learning.  Restore the cursor, hide the old text, and enable the
    //  next button in the sequence.
    //-----------------------------------------------------------------------
    SetCursor(oldCursor);
    m_learningText.ShowWindow(SW_HIDE);
    m_learnTemplateButton.EnableWindow(FALSE);
    m_doSearchButton.EnableWindow(TRUE);
}


void CEasyPatternMatchingDlg::OnDoSearch() 
{
    //-----------------------------------------------------------------------
    //  Get the control values, then build the match options with those
    //  values.
    //-----------------------------------------------------------------------
    MatchPatternOptions matchOptions;
    UpdateData(TRUE);
    matchOptions.mode = m_rotationInvariance ? IMAQ_MATCH_ROTATION_INVARIANT : IMAQ_MATCH_SHIFT_INVARIANT;
    matchOptions.minContrast = 10;
    matchOptions.subpixelAccuracy = FALSE;
    matchOptions.numMatchesRequested = atoi(m_maxMatches);
    if (matchOptions.numMatchesRequested < 1) {
        matchOptions.numMatchesRequested = 1;
        m_maxMatches.Format("%d", 1);
    }
    matchOptions.minMatchScore = (float) atof(m_matchScore);
    if (matchOptions.minMatchScore < 0) {
        matchOptions.minMatchScore = 0;
        m_matchScore.Format("%d", 0);
    }
    if (matchOptions.minMatchScore > 1000) {
        matchOptions.minMatchScore = 1000;
        m_matchScore.Format("%d", 1000);
    }
    matchOptions.numRanges = 0;
    matchOptions.angleRanges = NULL;
    matchOptions.matchFactor = 0;
    //-----------------------------------------------------------------------
    //  Now do the match
    //-----------------------------------------------------------------------
    int numMatches;
    PatternMatch* report = imaqMatchPattern(searchImage, templateImage, &matchOptions, IMAQ_NO_RECT, &numMatches);
    if (!report) {
        char* err = imaqGetErrorText(imaqGetLastError());
        MessageBox(err, "Error", MB_ICONEXCLAMATION | MB_OK);
        imaqDispose(err);
        EndDialog(0);
    }
    //-----------------------------------------------------------------------
    //  Put the number of matches found on the dialog & draw the matches on
    //  the image.  After that, we are done with the report so dispose it.
    //-----------------------------------------------------------------------
    m_matchesText.Format("Matches Found: %d", numMatches);
    UpdateData(FALSE);
    RGBValue red = {0x20, 0x20, 0xff, 0};
    DrawBoxAroundMatches(report, numMatches, red, SEARCH_WINDOW);
    imaqDispose(report);
}


void CEasyPatternMatchingDlg::DrawBoxAroundMatches (const PatternMatch* report, int numMatches, RGBValue boxColor, int windowNumber) {
    int i, j;
    Point approxCorner[4];
    Overlay* overlay;
    ContourID boxContour;
    ROI* overlayROI;
    
    //-----------------------------------------------------------------------
    //  Create the ROI we'll make the overlay from, then iterate over each
    //  match.
    //-----------------------------------------------------------------------
    overlayROI = imaqCreateROI();
    for (i = 0; i < numMatches; ++i) {
        //-------------------------------------------------------------------
        // Find the nearest pixel to each corner of the match.
        //-------------------------------------------------------------------
        for (j = 0; j < 4; ++j) {
            approxCorner[j].x = (int) (report[i].corner[j].x + 0.5);
            approxCorner[j].y = (int) (report[i].corner[j].y + 0.5);
        }
        //-------------------------------------------------------------------
        //  Add a contour for that box and make it the desired color.
        //-------------------------------------------------------------------
        boxContour = imaqAddClosedContour (overlayROI, approxCorner, 4);
        imaqSetContourColor (overlayROI, boxContour, &boxColor);
    }
    //-----------------------------------------------------------------------
    //  Now that we have the ROI, make an overlay from it & attach that to
    //  the desired window.
    //-----------------------------------------------------------------------
    overlay = imaqCreateOverlayFromROI(overlayROI);
    imaqSetWindowOverlay(windowNumber, overlay);  
    imaqDispose(overlayROI);
    imaqDispose(overlay);
}
