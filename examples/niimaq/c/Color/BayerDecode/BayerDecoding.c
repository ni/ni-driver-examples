/*****************************************************************************/
/*		This sample demonstrates how to continuously acquire and decode      */  
/*      bayer encoded images in multiple buffers using a high level ring     */
/*      operation                                                            */
/*****************************************************************************/     

#include <windows.h>
#include <mmsystem.h>
#include <stdio.h>
#include "BayerDecoding.h"
#define _NIWIN  
#include "niimaq.h"

// error checking macro
#define errChk(fCall) if (error = (fCall), error < 0) {goto Error;} else


// number of buffers used in the ring
#define NUM_RING_BUFFERS 5
// number of possible bayer patterns
#define NB_BAYER_PATTERN 4


// Window proc
LRESULT CALLBACK ImaqSmplProc(HWND hWnd, WPARAM iMessage, WPARAM wParam, LPARAM lParam);
// Error display function
void DisplayIMAQError(Int32 error);
// Callbacks
int OnRing (void);
int OnStop (void);
DWORD ImaqThread(LPDWORD lpdwParam);    



// windows GUI globals
static HINSTANCE    hInst;
static HWND         ImaqSmplHwnd;
static HWND         HStop, HRing, HQuit, HIntfName, HRedGain, HGreenGain, HBlueGain, HBayerPattern;
static HANDLE       HThread;


// Imaq globals
static SESSION_ID   Sid = 0;
static BUFLIST_ID   Bid = 0;
static INTERFACE_ID Iid = 0;
static Int8         *ImaqBuffers[NUM_RING_BUFFERS];   // acquisiton buffer
static Int8         *RGBBuffer=NULL;    // Bayer decoded RGB image buffer
static Int32        CanvasWidth = 512;  // width of the display area
static Int32        CanvasHeight = 384; // height of the display area
static Int32        CanvasTop = 10;     // top of the display area
static Int32        CanvasLeft = 10;    // left of the display area
static Int32        AcqWinWidth;
static Int32        AcqWinHeight;
static uInt32     	bitsPerPixel;
static uInt8        bayerPattern;
static uInt32		BufNum;
static BOOL			StopRing = FALSE;
static unsigned int plotFlag;
uInt32              redLUT[65536];      
uInt32              greenLUT[65536];    
uInt32              blueLUT[65536];   

// Strings for the ComboBox initialisation
char BayerPattern [NB_BAYER_PATTERN][10]=
{	
	"GBGB_RGRG",
	"GRGR_BGBG",
	"BGBG_GRGR", 
	"RGRG_GBGB",
};

int BayerPatternMap [4]=
{
	IMG_BAYER_PATTERN_GBGB_RGRG,
	IMG_BAYER_PATTERN_GRGR_BGBG,
	IMG_BAYER_PATTERN_BGBG_GRGR,
	IMG_BAYER_PATTERN_RGRG_GBGB,
};  


int WINAPI WinMain (HINSTANCE hInstance, HINSTANCE hPrevInstance,
					   LPSTR lpszCmdLine, int nCmdShow)
{
	CHAR        ImaqSmplClassName[] = "Imaq Sample";
    WNDCLASS  	ImaqSmplClass;
	MSG			msg;
    HWND        hTemp;
    int         i;

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

    // creates the main window
	ImaqSmplHwnd = CreateWindow(ImaqSmplClassName, "HLRing with Bayer Decoding", WS_OVERLAPPEDWINDOW | WS_VISIBLE,
							CW_USEDEFAULT, CW_USEDEFAULT, 680, 440, NULL, NULL, hInstance, NULL);

    
    // creates the interface name label
    if (!(hTemp = CreateWindow("Static","Interface name",ES_LEFT | WS_CHILD | WS_VISIBLE,
                                540,14,100,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // creates the Bayer Pattern label
    if (!(hTemp = CreateWindow("Static","Bayer Pattern",ES_LEFT | WS_CHILD | WS_VISIBLE,
                                540,197,100,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // creates the Red Gain label
    if (!(hTemp = CreateWindow("Static","Red Gain",ES_LEFT | WS_CHILD | WS_VISIBLE,
                                540,252,140,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // creates the Green Gain label
    if (!(hTemp = CreateWindow("Static","Green Gain",ES_LEFT | WS_CHILD | WS_VISIBLE,
                                540,302,140,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // creates the Blue Gain label
    if (!(hTemp = CreateWindow("Static","Blue Gain",ES_LEFT | WS_CHILD | WS_VISIBLE,
                                540,352,140,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // creates the interface name edit box
    if (!(HIntfName = CreateWindow("Edit","img0",ES_LEFT | WS_CHILD | WS_VISIBLE | WS_BORDER,
                                540,34,100,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // creates the red gain edit box
    if (!(HRedGain = CreateWindow("Edit","1.0",ES_LEFT | WS_CHILD | WS_VISIBLE | WS_BORDER,
                                540,272,100,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // creates the green gain edit box
    if (!(HGreenGain = CreateWindow("Edit","1.0",ES_LEFT | WS_CHILD | WS_VISIBLE | WS_BORDER,
                                540,322,100,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // creates the blue gain edit box
    if (!(HBlueGain = CreateWindow("Edit","1.0",ES_LEFT | WS_CHILD | WS_VISIBLE | WS_BORDER,
                                540,372,100,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // creates the Grab button
    if (!(HRing = CreateWindow("Button","Ring",BS_PUSHBUTTON | WS_CHILD | WS_VISIBLE | WS_BORDER,
                                550,67,80,40,ImaqSmplHwnd,(HMENU)PB_RING,hInstance,NULL)))
        return(FALSE);

    // creates the stop button
    if (!(HStop = CreateWindow("Button","Stop",BS_PUSHBUTTON | WS_CHILD | WS_VISIBLE | WS_BORDER,
                                550,107,80,40,ImaqSmplHwnd,(HMENU)PB_STOP,hInstance,NULL)))
        return(FALSE);

    EnableWindow(HStop, FALSE);

    // creates the quit application button
    if (!(HQuit = CreateWindow("Button","Quit",BS_DEFPUSHBUTTON | WS_CHILD | WS_VISIBLE,
                                550,147,80,40,ImaqSmplHwnd,(HMENU)PB_QUIT,hInstance,NULL)))
      return(FALSE);

    // create Bayer Pattern combobox
    if (!(HBayerPattern = CreateWindow("ComboBox","GBGB_RGRG", CBS_DROPDOWNLIST | WS_CHILD | WS_VISIBLE | WS_BORDER,
                                530,217,120,200,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // fill the Bayer Pattenr combo box
    for (i=0; i<NB_BAYER_PATTERN; i++)
		SendMessage(HBayerPattern, CB_ADDSTRING, 0, (LPARAM) BayerPattern[i]);
    // select first element
	SendMessage(HBayerPattern, CB_SETCURSEL, (WPARAM) 0, 0);
	
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


// Message proc
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
		case WM_DESTROY:
            OnStop();
			PostQuitMessage(0);

		default:
			return DefWindowProc(hWnd, iMessage, wParam, lParam);   
			break;
	}
	return 0;
}



// Function executed when the snap button is clicked
int OnRing (void)
{
	int 	        i, error;
    char	        intfName[64];
	DWORD			dwThreadId;
    int 	        bufSize;
    static char		redGainBuffer[32];
    static char		greenGainBuffer[32];
    static char		blueGainBuffer[32];
    double          redGain;
    double          greenGain;
    double          blueGain;
	
	
	// Get the interface name
	GetWindowText(HIntfName, intfName, 64);

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

    // Set the ROI to the size of the Canvas so that it will fit nicely
	errChk(imgSetAttribute2 (Sid, IMG_ATTR_ROI_WIDTH, AcqWinWidth));
	errChk(imgSetAttribute2 (Sid, IMG_ATTR_ROI_HEIGHT, AcqWinHeight));
	errChk(imgSetAttribute2 (Sid, IMG_ATTR_ROWPIXELS, AcqWinWidth)); 

	// get the information for the gain LUTs for Bayer decoding
	errChk(imgGetAttribute (Sid, IMG_ATTR_BITSPERPIXEL, &bitsPerPixel));
    GetWindowText(HRedGain, redGainBuffer, 32);
    GetWindowText(HGreenGain, greenGainBuffer, 32);
    GetWindowText(HBlueGain, blueGainBuffer, 32);
    redGain = atof(redGainBuffer);
    greenGain = atof(greenGainBuffer);
    blueGain = atof(blueGainBuffer);
    // Get the Bayer Pattern
    bayerPattern = BayerPatternMap[SendMessage(HBayerPattern, CB_GETCURSEL, 0, 0)];
    // calculate the gain LUTs for Bayer decoding
    errChk(imgCalculateBayerColorLUT(redGain, greenGain, blueGain, redLUT, greenLUT, blueLUT, bitsPerPixel));

    // compute the size of the required RGB image buffer
	bufSize = AcqWinWidth * AcqWinHeight * 4;
    // create the RGB image buffer
    errChk(imgCreateBuffer(Sid, FALSE, bufSize, &RGBBuffer));
		
	// We let the driver automatically allocate the memory for each acquisition buffer
	for(i=0; i<NUM_RING_BUFFERS; i++)
		ImaqBuffers[i] = NULL; 
	// Setup and launch the ring acquisition
	errChk(imgRingSetup (Sid, NUM_RING_BUFFERS, (void**)ImaqBuffers, 0, TRUE));

	// Set the Buffer index to zero
	BufNum = 0;
	StopRing = FALSE;
					   
    // Start the acquisition thread
	HThread = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE) ImaqThread, (LPDWORD*)&StopRing, 0, &dwThreadId);               
    if (HThread == NULL) 
        return 0;

	EnableWindow(HStop, TRUE);
    EnableWindow(HRing, FALSE);
    EnableWindow(HQuit, FALSE);
    EnableWindow(HRedGain, FALSE);
    EnableWindow(HGreenGain, FALSE);
    EnableWindow(HBlueGain, FALSE);
    EnableWindow(HBayerPattern, FALSE);
Error :
    if(error<0) {
        DisplayIMAQError(error);

        // Close the interface and the session
        if(Sid != 0)
	        imgClose (Sid, TRUE);
        if(Iid != 0)
	        imgClose (Iid, TRUE);
    }

   	return 0;
}


// Timer function used to monitor the progress of the acquisition
DWORD ImaqThread(LPDWORD lpdwParam)  
{
    static int		error;  
	static int		t1, t2, currBufNum;
    void* bufAddr = NULL;
				
	// the thread stop when StopRing goes to TRUE
	while(*((BOOL*)lpdwParam) == FALSE)
	{
		t2 = GetTickCount();
		
		// Hold the buffer whose index is BufNum. This is a cumulative buffer
		// index, if the buffer has not been acquired yet this function will block
		// until it is available. If the buffer has been overwritten this function
		// will return the last available buffer. Once the buffer is hold the buffer
		// won't be overwritten until it is released with imgSessionReleaseBuffer.
		errChk(imgSessionExamineBuffer2 (Sid, BufNum, &currBufNum, &bufAddr));

        // Decode the RBG information from the image
        errChk(imgBayerColorDecode(RGBBuffer, bufAddr, AcqWinHeight, AcqWinWidth, AcqWinWidth, AcqWinWidth, redLUT, greenLUT, blueLUT, bayerPattern, bitsPerPixel, 0));
								 
		// Display it using imgPlot
		errChk(imgPlot2 (ImaqSmplHwnd, RGBBuffer, 0, 0, AcqWinWidth, 
				AcqWinHeight, CanvasLeft, CanvasTop, IMGPLOT_COLOR_RGB32));
		
		// reinsert the buffer back in the ring
		errChk(imgSessionReleaseBuffer (Sid));
		
		// Now get next buffer
		BufNum ++;

	Error:
		if(error<0)
		{
			OnStop();
			DisplayIMAQError(error);
		}

		Sleep(0);
	}

	return 0;
}


// Stop the timer and the ongoing acquisition
int OnStop(void)
{
    int		error;
	DWORD	dwResult;
	    
	// Stop the thread, but only stop it once
    if (!StopRing) {
	    StopRing = TRUE;

	    // Wait for the thread to end and kill it otherwise
	    dwResult = WaitForSingleObject(HThread, 2000);
	    if (dwResult == WAIT_TIMEOUT)
		    TerminateThread(HThread, 0);

	     // stop the acquisition
        errChk(imgSessionStopAcquisition (Sid));
        		
    Error:
        if(error<0)
        {
	        DisplayIMAQError(error);
        }

        // Close the interface and the session
        if(Sid != 0)
	        imgClose (Sid, TRUE);
        if(Iid != 0)
	        imgClose (Iid, TRUE);

        EnableWindow(HStop, FALSE);
        EnableWindow(HRing, TRUE);
        EnableWindow(HQuit, TRUE);
        EnableWindow(HRedGain, TRUE);
        EnableWindow(HGreenGain, TRUE);
        EnableWindow(HBlueGain, TRUE);
        EnableWindow(HBayerPattern, TRUE);

	    CloseHandle (HThread);
    }
	return 0;
}



// in case of error this function will display a dialog box
// with the error message
void DisplayIMAQError(Int32 error)
{
    static Int8 ErrorMessage[256];

    memset(ErrorMessage, 0x00, sizeof(ErrorMessage));

    // converts error code to a message
    imgShowError(error, ErrorMessage);

    MessageBox(ImaqSmplHwnd, ErrorMessage, "Imaq Sample", MB_OK);
}


