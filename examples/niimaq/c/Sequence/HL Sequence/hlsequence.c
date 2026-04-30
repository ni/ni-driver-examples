/*****************************************************************************/
/*		This sample demonstrates how to acquire a sequence of picture        */
/*      using a high level sequence operation    						     */
/*****************************************************************************/ 

#include <windows.h>
#include <mmsystem.h>
#include <stdio.h>
#include "HLsequence.h"
#define _NIWIN  
#include "niimaq.h"

// error checking macro
#define errChk(fCall) if (error = (fCall), error < 0) {goto Error;} else


// number of buffers used in the ring
#define NUM_SEQUENCE_BUFFERS 10


// Window proc
LRESULT CALLBACK ImaqSmplProc(HWND hWnd, WPARAM iMessage, WPARAM wParam, LPARAM lParam);
// Error display function
void DisplayIMAQError(Int32 error);
// Sequence Callback
int OnSequence (void);
int OnStop (void);
// The prototype for this callback is different between Win32 and Win64.
// We do not use dwUser, dw1, or dw2 during this callback.
#ifdef _WIN64
void CALLBACK OnTimer(UINT uID,	UINT uMsg, DWORD_PTR dwUser, DWORD_PTR dw1, DWORD_PTR dw2);
#else
void CALLBACK OnTimer(UINT uID,	UINT uMsg, DWORD dwUser, DWORD dw1, DWORD dw2);
#endif


// windows GUI globals
static HINSTANCE    hInst;
static HWND         ImaqSmplHwnd;
static HWND         HStop, HSequ, HQuit, HIntfName, HStatus, HBufNum;
static UINT         ImaqTimerId;


// Imaq globals
static SESSION_ID   Sid = 0;
static BUFLIST_ID   Bid = 0;
static INTERFACE_ID Iid = 0;
static Int8         *ImaqBuffers[NUM_SEQUENCE_BUFFERS];   // acquisiton buffer
static Int32        CanvasWidth = 512;  // width of the display area
static Int32        CanvasHeight = 384; // height of the display area
static Int32        CanvasTop = 10;     // top of the display area
static Int32        CanvasLeft = 10;    // left of the display area
static Int32        AcqWinWidth;
static Int32        AcqWinHeight;
static unsigned int plotFlag; 


int WINAPI WinMain (HINSTANCE hInstance, HINSTANCE hPrevInstance,
					   LPSTR lpszCmdLine, int nCmdShow)
{
	CHAR        ImaqSmplClassName[] = "Imaq Sample";
    WNDCLASS  	ImaqSmplClass;
	MSG			msg;
    HWND        hTemp;

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
	ImaqSmplHwnd = CreateWindow(ImaqSmplClassName, "HLSequence", WS_OVERLAPPEDWINDOW | WS_VISIBLE,
							CW_USEDEFAULT, CW_USEDEFAULT, 680, 440, NULL, NULL, hInstance, NULL);

    
    // creates the interface name label
    if (!(hTemp = CreateWindow("Static","Interface name",ES_LEFT | WS_CHILD | WS_VISIBLE,
                                540,14,100,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // creates the status label
    if (!(hTemp = CreateWindow("Static","Status",ES_LEFT | WS_CHILD | WS_VISIBLE,
                                540,232,140,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // creates the buffer number label
    if (!(hTemp = CreateWindow("Static","Buffer number",ES_LEFT | WS_CHILD | WS_VISIBLE,
                                540,292,140,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // creates the interface name edit box
    if (!(HIntfName = CreateWindow("Edit","img0",ES_LEFT | WS_CHILD | WS_VISIBLE | WS_BORDER,
                                540,34,100,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // creates the status edit box
    if (!(HStatus = CreateWindow("Edit","0",ES_LEFT | WS_CHILD | WS_VISIBLE | WS_BORDER,
                                540,252,100,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // creates the buffer number edit box
    if (!(HBufNum = CreateWindow("Edit","0",ES_LEFT | WS_CHILD | WS_VISIBLE | WS_BORDER,
                                540,312,100,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // creates the Grab button
    if (!(HSequ = CreateWindow("Button","Sequence",BS_PUSHBUTTON | WS_CHILD | WS_VISIBLE | WS_BORDER,
                                550,72,80,40,ImaqSmplHwnd,(HMENU)PB_SEQU,hInstance,NULL)))
        return(FALSE);

    // creates the stop button
    if (!(HStop = CreateWindow("Button","Stop",BS_PUSHBUTTON | WS_CHILD | WS_VISIBLE | WS_BORDER,
                                550,112,80,40,ImaqSmplHwnd,(HMENU)PB_STOP,hInstance,NULL)))
        return(FALSE);

    // disable stop button
    EnableWindow(HStop, FALSE);
    EnableWindow(HStatus, FALSE);
    EnableWindow(HBufNum, FALSE);

    // creates the quit application button
    if (!(HQuit = CreateWindow("Button","Quit",BS_DEFPUSHBUTTON | WS_CHILD | WS_VISIBLE,
                                550,152,80,40,ImaqSmplHwnd,(HMENU)PB_QUIT,hInstance,NULL)))
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
				case PB_SEQU:
                    // Grab button has been hitten
					OnSequence();
					break;
                case PB_STOP:
                    // Grab button has been hitten
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



// Function executed when the sequence button is clicked
int OnSequence (void)
{
	int 	        i, error;
    unsigned int	skippedBuffers[NUM_SEQUENCE_BUFFERS] = {0};
    unsigned int	bitsPerPixel;
    char	        intfName[64];     
	
	
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
	
	
	// We let the driver automatically allocate the memory for us
	for(i=0; i<NUM_SEQUENCE_BUFFERS; i++)
		ImaqBuffers[i] = NULL;
	
	// Setup and launch the sequence acquisition asynchronously
	// if the sequence was launched synchronously, we wouldn't need
	// the timer loop to check the progress of the acquisition
	errChk(imgSequenceSetup (Sid, NUM_SEQUENCE_BUFFERS, (void **)ImaqBuffers,
					  skippedBuffers, TRUE, TRUE));
		
    // Set the timer
    ImaqTimerId = timeSetEvent(33, 0, OnTimer, 0, TIME_PERIODIC);

    EnableWindow(HStop, TRUE);
    EnableWindow(HSequ, FALSE);
    EnableWindow(HQuit, FALSE);
Error :
    if(error<0)
        DisplayIMAQError(error);

   	return 0;
}


// Timer function used to monitor the progress of the acquisition.
// The prototype for this callback is different between Win32 and Win64.
// We do not use dwUser, dw1, or dw2 during this callback.
#ifdef _WIN64
void CALLBACK OnTimer(UINT uID,	UINT uMsg, DWORD_PTR dwUser, DWORD_PTR dw1, DWORD_PTR dw2)
#else
void CALLBACK OnTimer(UINT uID,	UINT uMsg, DWORD dwUser, DWORD dw1, DWORD dw2)
#endif
{
    static int error, status, i;  
	unsigned int currBufNum;
    char    buffer[32];
	

	if(uID == ImaqTimerId)
	{
		// Check the progress of the acquisition
		errChk(imgSessionStatus (Sid, &status, &currBufNum));
			
		
		if(status)
			SetWindowText (HStatus, "Acquiring...");
		else
			SetWindowText (HStatus, "Finished");

		sprintf(buffer, "%d", currBufNum);
		SetWindowText (HBufNum, buffer);
		
		// Display it using imgPlot
		// Note that if you are using a board or camera with a bitdepth greater
		// that 8 bits, you need to set the flag parameter of imgPlot to match
		// the bit depth of the camera. See the "snap imgPlot" sample.
		if(currBufNum != 0xFFFFFFFF)
			errChk(imgPlot2 (ImaqSmplHwnd, ImaqBuffers[currBufNum], 0, 0, 
							AcqWinWidth, AcqWinHeight, CanvasLeft, CanvasTop, plotFlag));
    
		
		if(!status) // acquisition is finished
		{
			// Stop the timer
			timeKillEvent(ImaqTimerId);
        
			// Close the interface and the session
			imgClose (Sid, TRUE);
			imgClose (Iid, TRUE);

			EnableWindow(HStop, FALSE);
			EnableWindow(HSequ, TRUE);
			EnableWindow(HQuit, TRUE);
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


// When the user hit the stop button, stop the timer and the acquistion
int OnStop(void)
{
    int error;
    
    // Stop the timer
    timeKillEvent(ImaqTimerId);

    // stop the acquisition
    errChk(imgSessionStopAcquisition(Sid));
    		
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
    EnableWindow(HSequ, TRUE);
    EnableWindow(HQuit, TRUE);

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


