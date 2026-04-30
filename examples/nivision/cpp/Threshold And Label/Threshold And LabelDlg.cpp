// Threshold And LabelDlg.cpp : implementation file
//

#include "stdafx.h"
#include "Threshold And Label.h"
#include "Threshold And LabelDlg.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

#define BOTTOM_FUDGE_FACTOR -3

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
// CThresholdAndLabelDlg dialog

CThresholdAndLabelDlg::CThresholdAndLabelDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CThresholdAndLabelDlg::IDD, pParent)
{
	//{{AFX_DATA_INIT(CThresholdAndLabelDlg)
	m_particleCount = _T("");
	m_threshMaxText = _T("");
	m_threshMinText = _T("");
	//}}AFX_DATA_INIT
	// Note that LoadIcon does not require a subsequent DestroyIcon in Win32
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CThresholdAndLabelDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CThresholdAndLabelDlg)
	DDX_Control(pDX, IDC_CONNECTIVITY_4, m_connectivity4);
	DDX_Control(pDX, IDC_CONNECTIVITY_8, m_connectivity8);
	DDX_Control(pDX, IDC_THRESH_MIN, m_threshMinSlider);
	DDX_Control(pDX, IDC_THRESH_MAX, m_threshMaxSlider);
	DDX_Control(pDX, IDC_HISTOGRAM, m_histogram);
	DDX_Text(pDX, IDC_PARTICLE_COUNT, m_particleCount);
	DDX_Text(pDX, IDC_THRESH_MAX_DISPLAY, m_threshMaxText);
	DDX_Text(pDX, IDC_THRESH_MIN_DISPLAY, m_threshMinText);
	//}}AFX_DATA_MAP
}

BEGIN_MESSAGE_MAP(CThresholdAndLabelDlg, CDialog)
	//{{AFX_MSG_MAP(CThresholdAndLabelDlg)
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_BN_CLICKED(IDC_BROWSE, OnBrowse)
	ON_WM_HSCROLL()
	ON_BN_CLICKED(IDC_CONNECTIVITY_4, OnConnectivity)
	ON_BN_CLICKED(IDC_CONNECTIVITY_8, OnConnectivity)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CThresholdAndLabelDlg message handlers

BOOL CThresholdAndLabelDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	// Set the icon for this dialog.  The framework does this automatically
	//  when the application's main window is not a dialog
	SetIcon(m_hIcon, TRUE);			// Set big icon
	SetIcon(m_hIcon, FALSE);		// Set small icon
	
	//-----------------------------------------------------------------------
    //  IMAQ:  Our initialization
	//-----------------------------------------------------------------------
    image = NULL;    
    labelledImage = imaqCreateImage(IMAQ_IMAGE_U8, 2);
    m_threshMinSlider.SetRange(0, 255);
    m_threshMaxSlider.SetRange(0, 255);
    m_threshMinSlider.SetPos(100);
    m_threshMaxSlider.SetPos(200);
    m_threshMin = 100;
    m_threshMax = 200;
    m_threshMinText = "100";
    m_threshMaxText = "200";
    m_connectivity4.SetCheck(1);
    m_connectivity8.SetCheck(0);
    UpdateData(FALSE);
    Histogram();
    
	return TRUE;  // return TRUE  unless you set the focus to a control
}

// If you add a minimize button to your dialog, you will need the code below
//  to draw the icon.  For MFC applications using the document/view model,
//  this is automatically done for you by the framework.

void CThresholdAndLabelDlg::OnPaint() 
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
HCURSOR CThresholdAndLabelDlg::OnQueryDragIcon()
{
	return (HCURSOR) m_hIcon;
}

//---------------------------------------------------------------------------
//  IMAQ:  All code beyond this point is IMAQ code.
//---------------------------------------------------------------------------

void CThresholdAndLabelDlg::OnBrowse() 
{
    //-----------------------------------------------------------------------
    //  Query the user for the name of the file to open.
    //-----------------------------------------------------------------------
    CFileDialog fileDialog(TRUE, NULL, NULL, OFN_HIDEREADONLY | OFN_OVERWRITEPROMPT, "Image Files (*.jpg;*.png;*.bmp;*.tif;*.tiff;*.apd)|*.jpg;*.png;*.bmp;*.tif;*.tiff;*.apd|All Files(*.*)|*.*||");
    fileDialog.m_ofn.lpstrInitialDir = GetImageDirectory();
    if (fileDialog.DoModal() == IDOK) {
        //-------------------------------------------------------------------
        //  Now load the file.  If there's no problems with loading it,
        //  do the histogram and thresholding.
        //-------------------------------------------------------------------
        Image* loadedImage = LoadImage(fileDialog.GetPathName(), fileDialog.GetFileName());
        if (loadedImage) {
            imaqDispose(image);
            image = loadedImage;
            Histogram();
            ThresholdAndLabel();
        }
    }
}


Image* CThresholdAndLabelDlg::LoadImage(const CString& file, const CString& title) {
    Image* loadedImage;
    //-----------------------------------------------------------------------
    //  Get information about the file.
    //-----------------------------------------------------------------------
    int width, height;
    if (!imaqGetFileInfo((const char*) file, NULL, NULL, NULL, &width, &height, NULL)) {
        char* err = imaqGetErrorText(imaqGetLastError());
        MessageBox(err, "Error Reading the File", MB_OK);
        imaqDispose(err);
        return NULL;
    }
    //-----------------------------------------------------------------------
    //  Build the image.
    //-----------------------------------------------------------------------
    loadedImage = imaqCreateImage(IMAQ_IMAGE_U8, 2);
    if (!imaqSetImageSize(loadedImage, width, height)) {
        char* err = imaqGetErrorText(imaqGetLastError());
        MessageBox(err, "Error Creating Image", MB_OK);
        imaqDispose(err);
        imaqDispose(loadedImage);
        return NULL;
    }
    //-----------------------------------------------------------------------
    //  Read in the image.
    //-----------------------------------------------------------------------
    if (!imaqReadFile(loadedImage, (const char*) file, NULL, NULL)) {
        char* err = imaqGetErrorText(imaqGetLastError());
        MessageBox(err, "Error Reading the File", MB_OK);
        imaqDispose(err);
        imaqDispose(loadedImage);
        return NULL;
    }
    imaqSetWindowTitle(IMAGE_WINDOW, title);
    return loadedImage;
}


void CThresholdAndLabelDlg::Histogram() {
    UpdateData(TRUE);
    int max = 1;
    //-----------------------------------------------------------------------
    //  We need to make a metafile with the picture of the histogram.
    //  Start off by creating our device context.  We want the attributes of
    //  our DC so set up the attribute DC.  Fill in the
    //-----------------------------------------------------------------------
    CMetaFileDC dc;
    dc.CreateEnhanced(NULL, NULL, NULL, NULL);
    dc.m_hAttribDC = dc.m_hDC;
    //-----------------------------------------------------------------------
    //  If we have an image, we'll do the histogram
    //-----------------------------------------------------------------------
    if (image) {
        int i;
        //-------------------------------------------------------------------
        //  Do the histogram.  Figure out the maximum number of pixels in a bin.
        //-------------------------------------------------------------------
        HistogramReport* report = imaqHistogram(image, 256, 0, 255, NULL);
		if(!report)
		{
			char* err = imaqGetErrorText(imaqGetLastError());
			MessageBox(err, "Runtime Error", MB_OK);
			imaqDispose(err);
			return;
		}
        for (i = 0; i < 256; ++i) {
            if (max < report->histogram[i])
                max = report->histogram[i];
        }    
        //-------------------------------------------------------------------
        //  Make an array of points.  The x coordinate is simply the array
        //  index.  The y coordinate is the value.  Well, not exactly.  Since
        //  the DC has 0 at the top, we'll subtract the value from the max
        //-------------------------------------------------------------------
        CPoint points[256];
        for (i = 0; i < 256; ++i) {
            points[i].x = i;
            points[i].y = max - report->histogram[i];
        }
        imaqDispose(report);
        //-------------------------------------------------------------------
        //  OK.  Fill the background with back, then draw the line in red.
        //-------------------------------------------------------------------
        CBrush blackBrush((COLORREF) 0x00000000);
        CRect rect(0, BOTTOM_FUDGE_FACTOR, 257, max + 1);
        dc.FillRect(rect, &blackBrush);
        CPen redPen(PS_SOLID, 1, 0x004040FF);
        dc.SelectObject(redPen);
        dc.Polyline(points, 256);
    }
    else {
        //-------------------------------------------------------------------
        //  No histogram.  Just fill the entire area with black.
        //-------------------------------------------------------------------
        CBrush blackBrush((COLORREF) 0x00000000);
        CRect rect(0, BOTTOM_FUDGE_FACTOR, 257, max + 1);
        dc.FillRect(rect, &blackBrush);
    }
    //-----------------------------------------------------------------------
    //  Draw lines showing the min and max of the threshold, in green.
    //-----------------------------------------------------------------------
    CPen greenPen(PS_SOLID, 1, 0x0040FF40);
    dc.SelectObject(greenPen);
    dc.MoveTo(m_threshMin, BOTTOM_FUDGE_FACTOR);
    dc.LineTo(m_threshMin, max + 1);
    dc.MoveTo(m_threshMax, BOTTOM_FUDGE_FACTOR);
    dc.LineTo(m_threshMax, max + 1);
    //-----------------------------------------------------------------------
    //  We are done drawing.  Close the metafile, and set the histogram to
    //  the metafile
    //-----------------------------------------------------------------------
    HENHMETAFILE emf = dc.CloseEnhanced();
    m_histogram.SetEnhMetaFile(emf);
}


void CThresholdAndLabelDlg::ThresholdAndLabel() {
    if (!image)
        return;
    //-----------------------------------------------------------------------
    //  Do the threshold and labelling.  Redisplay the image and update the
    //  count control.
    //-----------------------------------------------------------------------
    UpdateData(TRUE);
    int numParticles;
    imaqThreshold(labelledImage, image, (float) m_threshMin, (float) m_threshMax, 1, 1);
    imaqLabel(labelledImage, labelledImage, m_connectivity8.GetCheck(), &numParticles);
    imaqMoveWindow(IMAGE_WINDOW, imaqMakePoint(0, 0));
    imaqSetWindowPalette(IMAGE_WINDOW, IMAQ_PALETTE_BINARY, NULL, 0);
    imaqDisplayImage(labelledImage, IMAGE_WINDOW, TRUE);
    m_particleCount.Format("Particle Count: %d", numParticles);
    UpdateData(FALSE);
}

void CThresholdAndLabelDlg::OnHScroll(UINT nSBCode, UINT nPos, CScrollBar* pScrollBar) 
{
    //-----------------------------------------------------------------------
    //  One of the sliders changed.  Cache the new values.
    //-----------------------------------------------------------------------
    m_threshMin = m_threshMinSlider.GetPos();
    m_threshMax = m_threshMaxSlider.GetPos();
    if (m_threshMin > m_threshMax) {
        int temp = m_threshMin;
        m_threshMin = m_threshMax;
        m_threshMax = temp;
    }
    //-----------------------------------------------------------------------
    //  Now redo the histogram and threshold & label.
    //-----------------------------------------------------------------------
    m_threshMinText.Format("%d", m_threshMinSlider.GetPos());
    m_threshMaxText.Format("%d", m_threshMaxSlider.GetPos());
    UpdateData(FALSE);
    Histogram();
    ThresholdAndLabel();
}

void CThresholdAndLabelDlg::OnConnectivity() 
{
    //-----------------------------------------------------------------------
    //  Redo the histogram and threshold & label.
    //-----------------------------------------------------------------------
    UpdateData(FALSE);
    Histogram();
    ThresholdAndLabel();
}
