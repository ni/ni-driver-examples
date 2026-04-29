/*****************************************************************************/
/*		This sample demonstrates how to use a function callback to stop		 */
/*		acquistion when an event occurs ....							     */
/*****************************************************************************/

#include <windows.h>
#include <mmsystem.h>
#include <stdio.h>
#include "stop trigger.h"
#define _NIWIN  
#include "niimaq.h"

// error checking macro
#define errChk(fCall) if (error = (fCall), error < 0) {goto Error;} else


// number of buffers used in the ring
#define NUM_RING_BUFFERS 5


// Window proc
LRESULT CALLBACK ImaqSmplProc(HWND hWnd, WPARAM iMessage, WPARAM wParam, LPARAM lParam);
// Error display function
void DisplayIMAQError(Int32 error);
// Callbacks
int OnRing (void);
// Our Callback function, it is installed later using imgSessionWaitSignalAsync2
uInt32	ImaqCallback(SESSION_ID sid, IMG_ERR err, IMG_SIGNAL_TYPE signal, uInt32 signalIdentifier,void* userdata);

// Threads
DWORD ImaqThread (LPDWORD lpdwParam);
DWORD StopThread (LPDWORD lpdwParam);
static HANDLE HThread;
static HANDLE HStopThread, HStopEvent;

// windows GUI globals
static HINSTANCE    hInst;
static HWND    	    ImaqSmplHwnd;
static HWND         HStop, HRing, HQuit, HIntfName, HFrameRate, HBufNum;
static HWND         HTrigSrcType, HTrigSrcNumber, HStopBuffer;

// Imaq globals
static SESSION_ID   Sid = 0;
static BUFLIST_ID   Bid = 0;
static INTERFACE_ID Iid = 0;
static Int8         *ImaqBuffers[NUM_RING_BUFFERS];   // acquisiton buffer
static Int32        CanvasWidth = 512;  // width of the display area
static Int32        CanvasHeight = 384; // height of the display area
static Int32        CanvasTop = 10;     // top of the display area
static Int32        CanvasLeft = 10;    // left of the display area
static Int32        AcqWinWidth;
static Int32        AcqWinHeight;
static unsigned int plotFlag; 
volatile static BOOL StopAcquisition;


char* TriggerSource[] =
{	
    {   "External"  },
    {   "RTSI"      },
    {   "ISO In"    },
};

const int NumberTriggerSource = sizeof(TriggerSource) / sizeof(TriggerSource[0]);

int TriggerMap[] =
{
	IMG_SIGNAL_EXTERNAL,
	IMG_SIGNAL_RTSI,
	IMG_SIGNAL_ISO_IN,
};


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

    // creates the main window
	ImaqSmplHwnd = CreateWindow(ImaqSmplClassName, "Stop Trigger", WS_OVERLAPPEDWINDOW | WS_VISIBLE,
							CW_USEDEFAULT, CW_USEDEFAULT, 700, 500, NULL, NULL, hInstance, NULL);

    
    // creates the interface name label
    if (!(hTemp = CreateWindow("Static","Interface Name",ES_LEFT | WS_CHILD | WS_VISIBLE,
                                540,10,100,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // creates the frame rate label
    if (!(hTemp = CreateWindow("Static","Frames/Second",ES_LEFT | WS_CHILD | WS_VISIBLE,
                                540,197,120,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // creates the buffer number label
    if (!(hTemp = CreateWindow("Static","Buffer Number",ES_LEFT | WS_CHILD | WS_VISIBLE,
                                540,247,100,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

	// creates the Trigger Source Type label
    if (!(hTemp = CreateWindow("Static","Trigger Source Type",SS_LEFT | WS_CHILD | WS_VISIBLE,
                                540,297,160,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // creates the Trigger Source Number label
    if (!(hTemp = CreateWindow("Static","Trigger Source Number",SS_LEFT | WS_CHILD | WS_VISIBLE,
                                540,352,160,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

	// create Trigger Source Type combobox
    if (!(HTrigSrcType = CreateWindow("ComboBox","External", CBS_DROPDOWNLIST | WS_CHILD | WS_VISIBLE | WS_BORDER,
                                540,317,100,200,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // create Trigger Source Number edit box
    if (!(HTrigSrcNumber = CreateWindow("Edit","0", ES_LEFT | ES_NUMBER | WS_CHILD | WS_VISIBLE | WS_BORDER,
                                540,372,100,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // create the Stopped Buffer label
    if (!(hTemp = CreateWindow("Static","Stop Buffer Index",SS_LEFT | WS_CHILD | WS_VISIBLE,
                                540,402,120,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // create Stopped Buffer edit box
    if (!(HStopBuffer = CreateWindow("Edit","0", ES_LEFT | ES_NUMBER | WS_CHILD | WS_VISIBLE | WS_BORDER,
                                540,422,100,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // fill the image rep combo box
    for (i = 0; i < NumberTriggerSource; i++)
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

    // Wait for the stop thread to complete before returning
    WaitForSingleObject(HStopThread, INFINITE);

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
                    // Quit button has been pressed
                    PostQuitMessage(0);
                    break;
				case PB_RING:
                    // Ring button has been pressed
					OnRing();
					break;
                case PB_STOP:
                    // Stop button has been pressed
					SetEvent(HStopEvent);
					break;
			}
			break;
		case WM_DESTROY:
            SetEvent(HStopEvent);
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
	unsigned long 	trigSrcType;
    unsigned long   trigSrcNumber;
	unsigned int	bitsPerPixel;
    char            trigSrcNumberString[64];
    DWORD			dwThreadId;
	
    // Create the event that needs to be signaled when we
    // wish to stop the acquisition.
    HStopEvent = CreateEvent(NULL, TRUE, FALSE, NULL);
    if (!HStopEvent)
        return 0;

    // Create the thread that is responsible for shutting
    // down the acquisition
    HStopThread = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE) StopThread, (LPDWORD) &HStopEvent, 0, &dwThreadId);
    if (!HStopThread)
        return 0;
	
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
	for(i=0; i<NUM_RING_BUFFERS; i++)
		ImaqBuffers[i] = NULL; 

    // Get the trigger type
    trigSrcType = TriggerMap[SendMessage(HTrigSrcType, CB_GETCURSEL, 0, 0)];

    // Get the trigger number
	GetWindowText(HTrigSrcNumber, trigSrcNumberString, 64);
    trigSrcNumber = atoi(trigSrcNumberString);

	// Install the callback function
    errChk(imgSessionWaitSignalAsync2(Sid, trigSrcType, trigSrcNumber, 
                                      IMG_TRIG_POLAR_ACTIVEH, ImaqCallback, NULL));

    // Set the stopped flag
    StopAcquisition = 0;

	// Setup and launch the ring acquisition
	errChk(imgRingSetup (Sid, NUM_RING_BUFFERS, (void**)ImaqBuffers, 0, TRUE));
				   
    // Start the acquisition thread
	HThread = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE) ImaqThread, (LPDWORD*)&StopAcquisition, 0, &dwThreadId);               
    if (HThread == NULL) 
        return 0;

    EnableWindow(HStop, TRUE);
    EnableWindow(HRing, FALSE);
    EnableWindow(HQuit, FALSE);
Error :
    if(error<0) {
        DisplayIMAQError(error);
        PostMessage(ImaqSmplHwnd, WM_COMMAND, PB_STOP, 0);
    }

   	return 0;
}


// This is the thread used to monitor the progress of the acquisition
DWORD ImaqThread(LPDWORD lpdwParam)
{
    int nbFrame = 0, error = 0;  
	int t1 = 0, t2;
    int requestedBuffer = 0, currentBuffer;
    void* bufferAddress = NULL;
    char    text[32];
	
	// the thread stop when StopRing goes to TRUE or there is an error
	while(!StopAcquisition && !error)
	{
		t2 = GetTickCount();
		
		// Hold the buffer whose index is requestedBuffer. This is a cumulative buffer
		// index, if the buffer has not been acquired yet this function will block
		// until it is available. If the buffer has been overwritten this function
		// will return the last available buffer. Once the buffer is hold the buffer
		// won't be overwritten until it is released with imgSessionReleaseBuffer.
		errChk(imgSessionExamineBuffer2 (Sid, requestedBuffer, &currentBuffer, &bufferAddress));
		
		// Display it using imgPlot
		// Note that if you are using a board or camera with a bitdepth greater
		// that 8 bits, you need to set the flag parameter of imgPlot to match
		// the bit depth of the camera. See the "snap imgPlot" sample. 
		errChk(imgPlot2 (ImaqSmplHwnd, bufferAddress, 0, 0, AcqWinWidth, AcqWinHeight,
						 CanvasLeft, CanvasTop, plotFlag));

        // Reinsert the buffer back in the ring
		errChk(imgSessionReleaseBuffer (Sid));

        // Update the user interface
        sprintf(text, "%d", currentBuffer);
		SetWindowText (HBufNum, text);
		
        // Get the next buffer
        requestedBuffer++;

		// Calculate the number of frame per seconds every 10 frames
		nbFrame++;
		if (nbFrame>10)
		{
			sprintf(text, "%.2f", 1000.0 * (double)nbFrame / (double)(t2-t1));
			SetWindowText (HFrameRate, text);
			t1 = t2;
			nbFrame=0;
		}

Error:
		if(error<0 && !StopAcquisition)
		{
			DisplayIMAQError(error);
            PostMessage(ImaqSmplHwnd, WM_COMMAND, PB_STOP, 0);
		}
	}

    return 0;
}


// Waits for the stop event to occur, then stops the acquisition.
DWORD StopThread(LPDWORD lpdwParam) {
    DWORD dwResult;

    // Get a handle to the stop event
    HANDLE event = *((HANDLE*) lpdwParam);

    // Wait for the done event to occur
    dwResult = WaitForSingleObject(event, INFINITE);
    if (dwResult != WAIT_FAILED) {
        CloseHandle(event);
        event = NULL;
    }

    // Set the stopped flag
    StopAcquisition = 1;

    // Wait for the thread to end and kill it otherwise
	dwResult = WaitForSingleObject(HThread, 2000);
	if (dwResult == WAIT_TIMEOUT)
		TerminateThread(HThread, 0);
    
    // stop the acquisition
    imgSessionStopAcquisition (Sid);

    	// Close the interface and the session
    if(Sid != 0)
	    imgClose (Sid, TRUE);
    if(Iid != 0)
	    imgClose (Iid, TRUE);

    EnableWindow(HStop, FALSE);
    EnableWindow(HRing, TRUE);
    EnableWindow(HQuit, TRUE);

    return 0;
}


// This function gets called when the signal occurs.  Because the
// acquisition is still running, we need to abort the acquisition
// as soon as we get the callback.
uInt32	ImaqCallback(SESSION_ID sid, IMG_ERR err, IMG_SIGNAL_TYPE signal, uInt32 signalIdentifier, void* userdata)
{
	uInt32	bufferNumber;
    char    text[64];

    // If the acquisition is still running, stop it and post a
    // message to shut down the rest of the aquisition.
    if (!StopAcquisition) {
        // Stop the acquisition thread
        StopAcquisition = TRUE;

	    // Stop the acquisition
	    imgSessionAbort (Sid, &bufferNumber);

        // Update the user interface to show what buffer
        // the acquisition stopped on.
        sprintf(text, "%d", bufferNumber);
        SetWindowText(HStopBuffer, text);
	
        // Stop the timer and close the session
	    PostMessage(ImaqSmplHwnd, WM_COMMAND, PB_STOP, 0);
    }

    // if the callback returns TRUE it is rearmed automatically.
    return FALSE;
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




