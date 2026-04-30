//////////////////////////////////////////////////////////////////////////////
//
//  Title     : Stop Trigger Hardware Synchronized.c
//  Project   : NI-IMAQ
//  Created   : 12/20/2004 @ 09:46:42
//  Author    : National Instruments
//  Platforms : All
//  Purpose   : This sample demonstrates how to use a hardware stop trigger to
//              stop an acquisition when an event occurs.
//
//////////////////////////////////////////////////////////////////////////////


//============================================================================
//  Includes
//============================================================================
#include <windows.h>
#include <mmsystem.h>
#include <stdio.h>
#include "Stop Trigger Hardware Synchronized.h"
#define _NIWIN  
#include "niimaq.h"


//============================================================================
//  Error checking macro
//============================================================================
#define errChk(fCall) if (error = (fCall), error < 0) {goto Error;} else


//============================================================================
//  Defines
//============================================================================
#define NUM_TRIG_TYPES 3


//============================================================================
//  Function declarations
//============================================================================
LRESULT CALLBACK ImaqSmplProc(HWND hWnd, WPARAM iMessage, WPARAM wParam, LPARAM lParam);
void __cdecl FreeResources(void);
void DisplayIMAQError(Int32 error);
int OnRing (void);
int OnStop (void);
void CALLBACK OnTimer(UINT uID,	UINT uMsg, DWORD dwUser, DWORD dw1, DWORD dw2);
uInt32	ImaqCallback(SESSION_ID sid, IMG_ERR err, IMG_SIGNAL_TYPE signal, uInt32 signalIdentifier,void* userdata);
void PlotDisplay(BOOL preTrig, uInt32 index);
void UpdateScrollPos(HWND scrollBar, int newPos);
uInt32 ConvertSetIndexToBufferListIndex(BOOL preTrig, uInt32 setIndex);


//============================================================================
//  Globals - Windows GUI
//============================================================================
static HINSTANCE    hInst;
static HWND    	    ImaqSmplHwnd;
static HWND         HStop, HRing, HQuit, HIntfName, HFrameRate, HBufNum, HTrigSrcType, HTrigSrcNumber, HNumPreTrigBufs, HNumPostTrigBufs, HPreTrigScroll, HPostTrigScroll, HPreTrigBufsIndex, HPostTrigBufsIndex;
static UINT         ImaqTimerId;


//============================================================================
//  Globals - NI-IMAQ
//============================================================================
static SESSION_ID   Sid = 0;
static BUFLIST_ID   Bid = 0;
static INTERFACE_ID Iid = 0;
static Int8         **ImaqBuffers = NULL;      // acquisiton buffer
static Int32        CanvasWidth = 248;         // width of the display area
static Int32        CanvasHeight = 384;        // height of the display area
static Int32        CanvasTop = 30;            // top of the display area
static Int32        PreTrigCanvasLeft = 10;    // left of the pre-trigger buffers display area
static Int32        PostTrigCanvasLeft = 266;  // left of the post-trigger buffers display area
static Int32        AcqWinWidth;               // acquisition window width
static Int32        AcqWinHeight;              // acquisition window height
static uInt32       numRingBuffers;            // number of buffers in the ring
static uInt32       acquiredPreTrigBufs;       // number of acquired pre-trigger buffers
static uInt32       acquiredPostTrigBufs;      // number of acquired post-trigger buffers
static uInt32       oldestBufferIndex;         // oldest acquired buffer index
static unsigned int plotFlag; 
char TriggerSource [NUM_TRIG_TYPES][10] =
{	
	"External",
	"RTSI",
	"ISO In"
};
int TriggerMap [NUM_TRIG_TYPES] =
{
	IMG_SIGNAL_EXTERNAL,
	IMG_SIGNAL_RTSI,
	IMG_SIGNAL_ISO_IN
};


//////////////////////////////////////////////////////////////////////////////
//
//  WinMain
//
//  Description:
//      The main program function.
//
//////////////////////////////////////////////////////////////////////////////
int WINAPI WinMain (HINSTANCE hInstance, HINSTANCE hPrevInstance,
					   LPSTR lpszCmdLine, int nCmdShow)
{
	CHAR        ImaqSmplClassName[] = "Imaq Sample";
    WNDCLASS  	ImaqSmplClass;
	MSG			msg;
    HWND        hTemp;
	int			i;

    // register the main window
	hInst = hInstance;
	if (!hPrevInstance)
    {
		ImaqSmplClass.style         = CS_HREDRAW | CS_VREDRAW;
		ImaqSmplClass.lpfnWndProc   = (WNDPROC) ImaqSmplProc;
		ImaqSmplClass.cbClsExtra    = 0;
		ImaqSmplClass.cbWndExtra    = 0;
		ImaqSmplClass.hInstance     = hInstance;
		ImaqSmplClass.hIcon         = LoadIcon(NULL, IDI_APPLICATION);
		ImaqSmplClass.hCursor       = LoadCursor (NULL, IDC_ARROW);
		ImaqSmplClass.hbrBackground = GetStockObject(LTGRAY_BRUSH);
		ImaqSmplClass.lpszMenuName  = 0;
		ImaqSmplClass.lpszClassName = ImaqSmplClassName;
	
		if (!RegisterClass (&ImaqSmplClass))
           	return (0);
	}

	// Register the cleanup function.
	atexit(FreeResources);

    // creates the main window
	ImaqSmplHwnd = CreateWindow(ImaqSmplClassName, "Stop Trigger Hardware Synchronized", WS_OVERLAPPEDWINDOW | WS_VISIBLE,
							CW_USEDEFAULT, CW_USEDEFAULT, 750, 540, NULL, NULL, hInstance, NULL);

    
    // creates the interface name label
    if (!(hTemp = CreateWindow("Static","Interface Name",ES_LEFT | WS_CHILD | WS_VISIBLE,
                                540,10,100,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // creates the frame rate label
    if (!(hTemp = CreateWindow("Static","Frames/Second",ES_LEFT | WS_CHILD | WS_VISIBLE,
                                540,197,140,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // creates the buffer number label
    if (!(hTemp = CreateWindow("Static","Buffer Number",ES_LEFT | WS_CHILD | WS_VISIBLE,
                                540,247,140,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

	// creates the Trigger Source Type label
    if (!(hTemp = CreateWindow("Static","Trigger Source Type",SS_LEFT | WS_CHILD | WS_VISIBLE,
                                540,297,160,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // creates the Trigger Source Number label
    if (!(hTemp = CreateWindow("Static","Trigger Source Number",SS_LEFT | WS_CHILD | WS_VISIBLE,
                                540,352,160,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // creates the Num Pre-Trigger Buffers label
    if (!(hTemp = CreateWindow("Static","Num Pre-Trigger Buffers",SS_LEFT | WS_CHILD | WS_VISIBLE,
                                540,407,180,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // creates the Num Post-Trigger Buffers label
    if (!(hTemp = CreateWindow("Static","Num Post-Trigger Buffers",SS_LEFT | WS_CHILD | WS_VISIBLE,
                                540,462,180,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // creates the Pre-Trigger Buffers label
    if (!(hTemp = CreateWindow("Static","Pre-Trigger Buffers",SS_LEFT | WS_CHILD | WS_VISIBLE,
                                PreTrigCanvasLeft,10,180,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // creates the Post-Trigger Buffers label
    if (!(hTemp = CreateWindow("Static","Post-Trigger Buffers",SS_LEFT | WS_CHILD | WS_VISIBLE,
                                PostTrigCanvasLeft,10,180,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

	// create Trigger Source Type combobox
    if (!(HTrigSrcType = CreateWindow("ComboBox","External", CBS_DROPDOWNLIST | WS_CHILD | WS_VISIBLE | WS_BORDER,
                                540,317,100,200,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // create Trigger Source Number edit box
    if (!(HTrigSrcNumber = CreateWindow("Edit","0", ES_LEFT | ES_NUMBER | WS_CHILD | WS_VISIBLE | WS_BORDER,
                                540,372,100,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // create Num Pre-Trigger Buffers edit box
    if (!(HNumPreTrigBufs = CreateWindow("Edit","5", ES_LEFT | ES_NUMBER | WS_CHILD | WS_VISIBLE | WS_BORDER,
                                540,427,100,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // create Num Post-Trigger Buffers edit box
    if (!(HNumPostTrigBufs = CreateWindow("Edit","10", ES_LEFT | ES_NUMBER | WS_CHILD | WS_VISIBLE | WS_BORDER,
                                540,482,100,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

	// create the pre-trigger buffers scroll bar
	if (!(HPreTrigScroll = CreateWindow("ScrollBar", "PreTrigScroll", WS_CHILD | WS_VISIBLE | WS_BORDER | WS_DISABLED,
		                        10, 427, 248, 20, ImaqSmplHwnd, (HMENU)SB_PRETRIG, hInstance, NULL)))
        return (FALSE);

	// create the post-trigger buffers scroll bar
	if (!(HPostTrigScroll = CreateWindow("ScrollBar", "PostTrigScroll", WS_CHILD | WS_VISIBLE | WS_BORDER | WS_DISABLED,
		                        266, 427, 248, 20, ImaqSmplHwnd, (HMENU)SB_POSTTRIG, hInstance, NULL)))
        return (FALSE);

	// create the pre-trigger buffers index
	if (!(HPreTrigBufsIndex = CreateWindow("Edit", "0", ES_LEFT | ES_NUMBER | WS_CHILD | WS_VISIBLE | WS_BORDER | WS_DISABLED,
		                        10, 457, 100, 20, ImaqSmplHwnd, (HMENU)-1, hInstance, NULL)))
		return (FALSE);

	// create the post-trigger buffers index
	if (!(HPostTrigBufsIndex = CreateWindow("Edit", "0", ES_LEFT | ES_NUMBER | WS_CHILD | WS_VISIBLE | WS_BORDER | WS_DISABLED,
		                        266, 457, 100, 20, ImaqSmplHwnd, (HMENU)-1, hInstance, NULL)))
		return (FALSE);

    // fill the image rep combo box
    for (i = 0; i < NUM_TRIG_TYPES; i++)
		SendMessage(HTrigSrcType, CB_ADDSTRING, 0, (LPARAM) TriggerSource[i]);
    // select first element
	SendMessage(HTrigSrcType, CB_SETCURSEL, (WPARAM) 0, 0);

  
    // creates the interface name edit box
    if (!(HIntfName = CreateWindow("Edit","img0", ES_LEFT | WS_CHILD | WS_VISIBLE | WS_BORDER,
                                540,30,100,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // creates the frame rate edit box
    if (!(HFrameRate = CreateWindow("Edit","0",ES_LEFT | WS_CHILD | WS_VISIBLE | WS_BORDER,
                                540,217,100,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // creates the buffer number edit box
    if (!(HBufNum = CreateWindow("Edit","0",ES_LEFT | WS_CHILD | WS_VISIBLE | WS_BORDER,
                                540,267,100,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // creates the Ring button
    if (!(HRing = CreateWindow("Button","Ring",BS_PUSHBUTTON | WS_CHILD | WS_VISIBLE | WS_BORDER,
                                550,62,80,40,ImaqSmplHwnd,(HMENU)PB_RING,hInstance,NULL)))
        return(FALSE);

    // creates the stop button
    if (!(HStop = CreateWindow("Button","Stop",BS_PUSHBUTTON | WS_CHILD | WS_VISIBLE | WS_BORDER,
                                550,102,80,40,ImaqSmplHwnd,(HMENU)PB_STOP,hInstance,NULL)))
        return(FALSE);

    EnableWindow(HStop, FALSE);

    // creates the quit application button
    if (!(HQuit = CreateWindow("Button","Quit",BS_DEFPUSHBUTTON | WS_CHILD | WS_VISIBLE,
                                550,142,80,40,ImaqSmplHwnd,(HMENU)PB_QUIT,hInstance,NULL)))
      return(FALSE);
	
    // Display the main window
	ShowWindow(ImaqSmplHwnd, SW_SHOW);
    UpdateWindow(ImaqSmplHwnd);

    	
	while (GetMessage (&msg, NULL, 0, 0))
    {
        TranslateMessage (&msg) ;
        DispatchMessage (&msg) ;
    }

    return (int)(msg.wParam);
}


//////////////////////////////////////////////////////////////////////////////
//
//  ImaqSmplProc
//
//  Description:
//      The messaging function.
//
//////////////////////////////////////////////////////////////////////////////
LRESULT CALLBACK ImaqSmplProc(HWND hWnd, WPARAM iMessage, WPARAM wParam, LPARAM lParam)
{
	WORD            wmId;	
	
	switch (iMessage)
	{
		case WM_COMMAND:
			wmId    = LOWORD(wParam);
			switch (wmId)
            {
                case PB_QUIT:
                    // Quit button has been hitten
                    PostQuitMessage(0);
                    break;
				case PB_RING:
                    // Ring button has been hitten
					OnRing();
					break;
                case PB_STOP:
                    // Stop button has been hitten
					OnStop();
					break;
			}
			break;
		case WM_HSCROLL:
		{
			HWND scrollBar = (HWND)lParam;
			if (scrollBar) {
                // We received a message from one of our scrollbars.  Determine how to process it.
                // The only type of event that we will process is a change of position.
				UINT msgId = LOWORD(wParam);
				int newPos;
				SCROLLINFO scrollInfo;
				scrollInfo.cbSize = sizeof(SCROLLINFO);
				scrollInfo.fMask = SIF_RANGE | SIF_POS;
				GetScrollInfo(scrollBar, SB_CTL, &scrollInfo);
				newPos = scrollInfo.nPos;
				switch (msgId)
				{
					case SB_LINELEFT:
					case SB_PAGELEFT:
					{
						newPos = (scrollInfo.nPos == scrollInfo.nMin) ? scrollInfo.nMin : (scrollInfo.nPos - 1);
						break;
					}
					case SB_LINERIGHT:
					case SB_PAGERIGHT:
					{
						newPos = (scrollInfo.nPos == scrollInfo.nMax) ? scrollInfo.nMax : (scrollInfo.nPos + 1);
						break;
					}
					case SB_THUMBPOSITION:
					{
						newPos = HIWORD(wParam);
						break;
					}
				}
                // The new position is different than the old position.  Update the scrollbar position
                // and plot the image the new position corresponds to.
				if (newPos != scrollInfo.nPos) {
                    BOOL preTrig = scrollBar == HPreTrigScroll;
					UpdateScrollPos(scrollBar, newPos);
					PlotDisplay(preTrig, ConvertSetIndexToBufferListIndex(preTrig, newPos));
				}
			}
			break;
		 }
		case WM_DESTROY:
            OnStop();
			PostQuitMessage(0);

		default:
			return DefWindowProc(hWnd, iMessage, wParam, lParam);   
			break;
	}
	return 0;
}



//////////////////////////////////////////////////////////////////////////////
//
//  UpdateScrollPos
//
//  Description:
//      Updates one of the scrollbars with the given new position.  The new
//      position must be within the scrollbar's range.
//
//  Parameters:
//      scrollBar - The scrollBar to update
//      newPos    - The new position
//
//////////////////////////////////////////////////////////////////////////////
void UpdateScrollPos(HWND scrollBar, int newPos) {
	// Update the scrollbar position and the corresponding edit window that
    // displays the current scrollbar position index.
    char indexStr[64];
	SCROLLINFO scrollInfo;
	scrollInfo.cbSize = sizeof(SCROLLINFO);
	scrollInfo.fMask = SIF_POS;
	scrollInfo.nPos = newPos;
	SetScrollInfo(scrollBar, SB_CTL, &scrollInfo, TRUE);
	sprintf(indexStr, "%d", newPos);
	if (scrollBar == HPreTrigScroll) {
		SetWindowText(HPreTrigBufsIndex, indexStr);
	}
	else if (scrollBar == HPostTrigScroll) {
		SetWindowText(HPostTrigBufsIndex, indexStr);
	}
}


//////////////////////////////////////////////////////////////////////////////
//
//  OnRing
//
//  Description:
//      Function to be called when the ring button is clicked.
//
//////////////////////////////////////////////////////////////////////////////
int OnRing (void)
{
	int 	        error;
    char	        intfName[64];
	unsigned long   i;
	unsigned long 	trigSrcType;
    unsigned long   trigSrcNumber;
	unsigned long   numPreTrigBufs;
	unsigned long   numPostTrigBufs;
	unsigned int	bitsPerPixel;
    char            tempString[64];
	
	// Get the interface name
	GetWindowText(HIntfName, intfName, 64);

	// Get the trigger type
    trigSrcType = TriggerMap[SendMessage(HTrigSrcType, CB_GETCURSEL, 0, 0)];

    // Get the trigger number
	GetWindowText(HTrigSrcNumber, tempString, 64);
    trigSrcNumber = atoi(tempString);

	// Get the number of buffers to be acquired both before and after the trigger asserts
    // The number of pre- and post-trigger buffers must be non-zero.
	GetWindowText(HNumPreTrigBufs, tempString, 64);
	numPreTrigBufs = atoi(tempString);
	GetWindowText(HNumPostTrigBufs, tempString, 64);
	numPostTrigBufs = atoi(tempString);
    if (numPreTrigBufs == 0 && numPostTrigBufs == 0) {
        MessageBox(ImaqSmplHwnd, "The number of pre- and post-trigger buffers cannot both be zero.", "Error", MB_OK);
        return 0;
    }

	// The previous interface may already be open.  Free any allocated resources.
	FreeResources();

	// Set the scrollbar positions back to 0 and disable them.
	UpdateScrollPos(HPreTrigScroll, 0);
	UpdateScrollPos(HPostTrigScroll, 0);
	EnableWindow(HPreTrigScroll, FALSE);
	EnableWindow(HPostTrigScroll, FALSE);

    // It is invalid to change the number of pre-trigger buffers, number of post-trigger
    // buffers, trigger type, or trigger number during an acquisition, so disable the windows.
    EnableWindow(HNumPreTrigBufs, FALSE);
    EnableWindow(HNumPostTrigBufs, FALSE);
    EnableWindow(HTrigSrcType, FALSE);
    EnableWindow(HTrigSrcNumber, FALSE);

	// Open an interface and a session
	errChk(imgInterfaceOpen (intfName, &Iid));
	errChk(imgSessionOpen (Iid, &Sid));
	
	// Let's check that the Acquisition window is not smaller than the Canvas
	errChk(imgGetAttribute (Sid, IMG_ATTR_ROI_WIDTH, &AcqWinWidth));
	errChk(imgGetAttribute (Sid, IMG_ATTR_ROI_HEIGHT, &AcqWinHeight));
	
	if(CanvasWidth < AcqWinWidth)
		AcqWinWidth = CanvasWidth;
	if(CanvasHeight < AcqWinHeight)
		AcqWinHeight = CanvasHeight;
		
	// get the pixel depth of the camera.
	errChk(imgGetAttribute (Sid, IMG_ATTR_BITSPERPIXEL, &bitsPerPixel));
	
	switch(bitsPerPixel)
	{
	case 10:
		plotFlag = IMGPLOT_MONO_10;
		break;
	case 12:
		plotFlag = IMGPLOT_MONO_12;
		break;
	case 14:
		plotFlag = IMGPLOT_MONO_14;
		break;
	case 16:
		plotFlag = IMGPLOT_MONO_16;
		break;
	case 24:
	case 32:
		// assumes that a 24 bits camera is a color camera.
		// in this mode, even if the camera is 24 bits the board returns 32 bits values
		plotFlag = IMGPLOT_COLOR_RGB32;
		break;
	default:
		plotFlag = IMGPLOT_MONO_8;
		break;
	}

	// Set the ROI to the size of the Canvas so that it will fit nicely
	errChk(imgSetAttribute2 (Sid, IMG_ATTR_ROI_WIDTH, AcqWinWidth));
	errChk(imgSetAttribute2 (Sid, IMG_ATTR_ROI_HEIGHT, AcqWinHeight));
	errChk(imgSetAttribute2 (Sid, IMG_ATTR_ROWPIXELS, AcqWinWidth)); 

	// The number of buffers in the ring should be the summation of the number of
	// buffers to be acquired before and after the stop trigger.
	numRingBuffers = numPreTrigBufs + numPostTrigBufs;
	ImaqBuffers = malloc(numRingBuffers * sizeof(*ImaqBuffers));
	
	// We let the driver automatically allocate the memory for us
	for(i=0; i< numRingBuffers; i++)
		ImaqBuffers[i] = NULL; 

	// Configure the stop trigger.
	errChk(imgSessionTriggerConfigure2 (Sid, trigSrcType, trigSrcNumber, IMG_TRIG_POLAR_ACTIVEH, 1000, IMG_TRIG_ACTION_STOP));
	errChk(imgSetAttribute2 (Sid, IMG_ATTR_NUM_POST_TRIGGER_BUFFERS, numPostTrigBufs));

	// Setup and launch the ring acquisition
	errChk(imgRingSetup (Sid, numRingBuffers, (void**)ImaqBuffers, 0, TRUE));
					   
    // Set the timer
    ImaqTimerId = timeSetEvent(33, 0, OnTimer, 0, TIME_PERIODIC);

    EnableWindow(HStop, TRUE);
    EnableWindow(HRing, FALSE);
    EnableWindow(HQuit, FALSE);
Error :
    if(error<0) {
        EnableWindow(HNumPreTrigBufs, TRUE);
        EnableWindow(HNumPostTrigBufs, TRUE);
        EnableWindow(HTrigSrcType, TRUE);
        EnableWindow(HTrigSrcNumber, TRUE);
        DisplayIMAQError(error);
    }

   	return 0;
}


//////////////////////////////////////////////////////////////////////////////
//
//  ConvertSetIndexToBufferListIndex
//
//  Description:
//      Converts a pre- or post-trigger buffer set index into a buffer list
//      index.
//
//  Parameters:
//      preTrig  - Indicates which set (pre- or post-trigger) we are converting from
//      setIndex - The set index
//
//  Return Value:
//      The converted buffer list index.
//
//////////////////////////////////////////////////////////////////////////////
uInt32 ConvertSetIndexToBufferListIndex(BOOL preTrig, uInt32 setIndex) {
    return (preTrig ? (oldestBufferIndex + setIndex) : (oldestBufferIndex + acquiredPreTrigBufs + setIndex)) % numRingBuffers;
}


//////////////////////////////////////////////////////////////////////////////
//
//  PlotDisplay
//
//  Description:
//      Plots one of the buffer displays with the given buffer list index.  The
//      display to update is specified by the preTrig parameter.  If TRUE,
//      we will update the pre-trigger display; if FALSE, we update the post-
//      trigger display.
//
//  Parameters:
//      updatePreTrig - Indicates which display to update
//      index         - The buffer list index
//
//////////////////////////////////////////////////////////////////////////////
void PlotDisplay(BOOL preTrig, uInt32 index) {
	// Display it using imgPlot
	// Note that if you are using a board or camera with a bitdepth greater
	// that 8 bits, you need to set the flag parameter of imgPlot to match
	// the bit depth of the camera. See the "snap imgPlot" sample. 
	imgPlot2 (ImaqSmplHwnd, ImaqBuffers[index], 0, 0, AcqWinWidth, AcqWinHeight, preTrig ? PreTrigCanvasLeft : PostTrigCanvasLeft, CanvasTop, plotFlag);
}


//////////////////////////////////////////////////////////////////////////////
//
//  OnTimer
//
//  Description:
//      Timer function used to monitor the progress of the acquisition.
//
//////////////////////////////////////////////////////////////////////////////
void CALLBACK OnTimer(UINT uID,	UINT uMsg, DWORD dwUser, DWORD dw1, DWORD dw2)
{
    static int nbFrame = 0, error;  
	static int t1, t2, currBufNum, lastBufNum = 0xFFFFFFFF, acqInProgress;
    char    buffer[32];
	
	if(uID == ImaqTimerId)
	{
		t2 = GetTickCount();
		
		// If the acquisition is no longer in progress, quit and update the stop button.
		errChk(imgGetAttribute (Sid, IMG_ATTR_ACQ_IN_PROGRESS, &acqInProgress));

		if (acqInProgress == 0) {
			OnStop();
			return;
		}

		// Wait at least for the first valid frame
		errChk(imgGetAttribute (Sid, IMG_ATTR_LAST_VALID_BUFFER, &currBufNum));
			
		if((currBufNum == lastBufNum) || (currBufNum == 0xFFFFFFFF))
			return;

		sprintf(buffer, "%d", currBufNum);
		SetWindowText (HBufNum, buffer);
		
        // Display the buffer index.  Note that the buffer data is unprotected from
        // being re-acquired into during the plot operation.  If you want to guarantee
        // data protection, you should use imgSessionExamineBuffer.
        PlotDisplay(TRUE, currBufNum);

		lastBufNum = currBufNum;
		
		// Calculate the number of frame per seconds every 10 frames
		nbFrame++;
		if (nbFrame>10)
		{
			sprintf(buffer, "%.2f", 1000.0 * (double)nbFrame / (double)(t2-t1));
			SetWindowText (HFrameRate, buffer);
			t1 = t2;
			nbFrame=0;
		}

Error:
		if(error<0)
		{
			OnStop();
			DisplayIMAQError(error);
		}
	}

    return;
}


//////////////////////////////////////////////////////////////////////////////
//
//  OnStop
//
//  Description:
//      Stops the timer and the acquisition.
//
//////////////////////////////////////////////////////////////////////////////
int OnStop(void)
{
    int error;
	uInt32 lastAcquiredNum;
	SCROLLINFO scrollInfo;
    
    // Stop the timer
    timeKillEvent(ImaqTimerId);

	// If Sid is NULL, then there is no acquisition to stop.
	if (Sid == 0)
		return 0;
    
    // stop the acquisition
    errChk(imgSessionStopAcquisition (Sid));

	// Calculate the number of acquired pre-trigger buffers, post-trigger buffers, and the
	// oldest buffer index (the first pre-triggered buffer).
	errChk(imgGetAttribute(Sid, IMG_ATTR_LAST_VALID_FRAME, &lastAcquiredNum));
	errChk(imgGetAttribute(Sid, IMG_ATTR_NUM_POST_TRIGGER_BUFFERS, &acquiredPostTrigBufs));
	if (lastAcquiredNum + 1 < numRingBuffers) {
		oldestBufferIndex = 0;
		acquiredPreTrigBufs = (lastAcquiredNum + 1) - acquiredPostTrigBufs;
	}
	else {
		oldestBufferIndex = (lastAcquiredNum + 1) % numRingBuffers;
		acquiredPreTrigBufs = numRingBuffers - acquiredPostTrigBufs;
	}

	// Enable the scrollbars and update the range of each.  The ranges of each should
	// be equal to the number of pre- and post-trigger buffers.
	EnableWindow(HPreTrigScroll, TRUE);
	EnableWindow(HPostTrigScroll, TRUE);
	scrollInfo.cbSize = sizeof(scrollInfo);
	scrollInfo.fMask = SIF_RANGE | SIF_POS;
	scrollInfo.nPos = 0;
	scrollInfo.nMin = 0;
	scrollInfo.nMax = acquiredPreTrigBufs - 1;
	SetScrollInfo(HPreTrigScroll, SB_CTL, &scrollInfo, TRUE);
	scrollInfo.nMax = acquiredPostTrigBufs - 1;
	SetScrollInfo(HPostTrigScroll, SB_CTL, &scrollInfo, TRUE);

    // Also plot the buffer displays with the first buffer index of each
    // pre- and post-trigger buffer data set.
	PlotDisplay(TRUE, ConvertSetIndexToBufferListIndex(TRUE, 0));
	PlotDisplay(FALSE, ConvertSetIndexToBufferListIndex(FALSE, 0));

    // Re-enable the edit windows we disabled earlier.
    EnableWindow(HNumPreTrigBufs, TRUE);
    EnableWindow(HNumPostTrigBufs, TRUE);
    EnableWindow(HTrigSrcType, TRUE);
    EnableWindow(HTrigSrcNumber, TRUE);
    		
Error:
    if(error<0)
    {
	    DisplayIMAQError(error);
    }

    EnableWindow(HStop, FALSE);
    EnableWindow(HRing, TRUE);
    EnableWindow(HQuit, TRUE);

    return 0;
}


//////////////////////////////////////////////////////////////////////////////
//
//  DisplayIMAQError
//
//  Description:
//      In case of an error, this function will display a dialog box with
//      the error message.
//
//  Parameters:
//      error - The error code.
//
//////////////////////////////////////////////////////////////////////////////
void DisplayIMAQError(Int32 error)
{
    static Int8 ErrorMessage[256];

    memset(ErrorMessage, 0x00, sizeof(ErrorMessage));

    // converts error code to a message
    imgShowError(error, ErrorMessage);

    MessageBox(ImaqSmplHwnd, ErrorMessage, "Imaq Sample", MB_OK);
}


//////////////////////////////////////////////////////////////////////////////
//
//  FreeResources
//
//  Description:
//      Frees any NI-IMAQ resources this application may have allocated.
//
//////////////////////////////////////////////////////////////////////////////
void __cdecl FreeResources(void) {
	// Dispose the buffer pointer array.
	if (ImaqBuffers) {
		free(ImaqBuffers);
		ImaqBuffers = NULL;
	}

  	// Close the interface and the session
    if(Sid != 0) {
	    imgClose (Sid, TRUE);
		Sid = 0;
	}
    if(Iid != 0) {
	    imgClose (Iid, TRUE);
		Iid = 0;
	}

}




