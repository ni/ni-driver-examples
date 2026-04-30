/*****************************************************************************/
/*		This sample demonstrates how to continuously acquire pictures        */
/*      using a low level grab operation								     */
/*****************************************************************************/    

#include <windows.h>
#include <mmsystem.h>
#include <stdio.h>
#include "llgrab.h"
#define _NIWIN  
#include "niimaq.h"


// error checking macro
#define errChk(fCall) if (error = (fCall), error < 0) {goto Error;} else
// number of buffers used in the grab. Optimally this number is 3.
#define NUM_GRAB_BUFFERS 3

// Window proc
LRESULT CALLBACK ImaqSmplProc(HWND hWnd, WPARAM iMessage, WPARAM wParam, LPARAM lParam);
// Error display function
void DisplayIMAQError(Int32 error);
// Snap Callback
int OnGrab (void);
DWORD ImaqThread(LPDWORD lpdwParam);
DWORD StopThread(LPDWORD lpdwParam);
// Thread objects
static HANDLE HThread;
static HANDLE HStopThread, HStopEvent;


// windows GUI globals
static HINSTANCE    hInst;
static HWND    	    ImaqSmplHwnd;
static HWND         HStop, HGrab, HQuit, HIntfName, HFrameRate;


// Imaq globals
static SESSION_ID   Sid = 0;
static BUFLIST_ID   Bid = 0;
static INTERFACE_ID Iid = 0;
static Int8         *ImaqBuffers[NUM_GRAB_BUFFERS];	//acquisition buffers
static Int8         *CopyBuffer=NULL;   // copied acquisition buffer
static Int32        CanvasWidth = 512;  // width of the display area
static Int32        CanvasHeight = 384; // height of the display area
static Int32        CanvasTop = 10;     // top of the display area
static Int32        CanvasLeft = 10;    // left of the display area
static Int32        AcqWinWidth;
static Int32        AcqWinHeight;
static BOOL			StopGrab = FALSE;
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
	ImaqSmplHwnd = CreateWindow(ImaqSmplClassName, "LLGrab", WS_OVERLAPPEDWINDOW | WS_VISIBLE,
							CW_USEDEFAULT, CW_USEDEFAULT, 680, 440, NULL, NULL, hInstance, NULL);

    
    // creates the interface name label
    if (!(hTemp = CreateWindow("Static","Interface Name",ES_LEFT | WS_CHILD | WS_VISIBLE,
                                540,14,100,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // creates the frame rate label
    if (!(hTemp = CreateWindow("Static","Frame Rate",ES_LEFT | WS_CHILD | WS_VISIBLE,
                                540,232,140,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // creates the interface name edit box
    if (!(HIntfName = CreateWindow("Edit","img0",ES_LEFT | WS_CHILD | WS_VISIBLE | WS_BORDER,
                                540,34,100,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // creates the frame rate edit box
    if (!(HFrameRate = CreateWindow("Edit","0",ES_LEFT | WS_CHILD | WS_VISIBLE | WS_BORDER,
                                540,252,100,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // creates the Grab button
    if (!(HGrab = CreateWindow("Button","Grab",BS_PUSHBUTTON | WS_CHILD | WS_VISIBLE | WS_BORDER,
                                550,72,80,40,ImaqSmplHwnd,(HMENU)PB_GRAB,hInstance,NULL)))
        return(FALSE);

    // creates the stop button
    if (!(HStop = CreateWindow("Button","Stop",BS_PUSHBUTTON | WS_CHILD | WS_VISIBLE | WS_BORDER,
                                550,112,80,40,ImaqSmplHwnd,(HMENU)PB_STOP,hInstance,NULL)))
        return(FALSE);

    EnableWindow(HStop, FALSE);
    EnableWindow(HFrameRate, FALSE);

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
                    PostQuitMessage(0);
                    break;
				case PB_GRAB:
                    // Grab button has been pressed
					OnGrab();
					break;
                case PB_STOP:
                    // Grab button has been pressed
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
int OnGrab (void)
{
	int 	        error, bufSize, bytesPerPixel, i, bufCmd;
    char	        intfName[64];
	unsigned int	bitsPerPixel;
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
	
	
	// create a buffer list with NUM_GRAB_BUFFERS elements
    errChk(imgCreateBufList(NUM_GRAB_BUFFERS, &Bid));
    
    // compute the size of the required buffer
	errChk(imgGetAttribute (Sid, IMG_ATTR_BYTESPERPIXEL, &bytesPerPixel));
	bufSize = AcqWinWidth * AcqWinHeight * bytesPerPixel;

	// alloc our own buffer for storing copy
	CopyBuffer = (Int8 *) malloc(bufSize * sizeof (Int8));
	
	/* the following configuration assigns the following to buffer list 
	   element i:

		    1) buffer pointer that will contain image
		    2) size of the buffer for buffer element i
		    3) command to loop when this element is reached
    
	 */
     for (i = 0; i < NUM_GRAB_BUFFERS; i++)
	{
		errChk(imgCreateBuffer(Sid, FALSE, bufSize, &ImaqBuffers[i]));
		errChk(imgSetBufferElement2(Bid, i, IMG_BUFF_ADDRESS, ImaqBuffers[i]));
		errChk(imgSetBufferElement2(Bid, i, IMG_BUFF_SIZE, bufSize));
		bufCmd = (i == (NUM_GRAB_BUFFERS - 1)) ? IMG_CMD_LOOP : IMG_CMD_NEXT;
		errChk(imgSetBufferElement2(Bid, i, IMG_BUFF_COMMAND, bufCmd));
	}

	// lock down the buffers contained in the buffer list
	errChk(imgMemLock(Bid));

	// configure the session to use this buffer list
	errChk(imgSessionConfigure(Sid, Bid));

	// start the acquisition, asynchronous
	errChk(imgSessionAcquire(Sid, TRUE, NULL));
					   
    StopGrab = FALSE;

    // Start the acquisition thread
	HThread = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE) ImaqThread, (LPDWORD*)&StopGrab, 0, &dwThreadId);               
    if (HThread == NULL) 
        return 0;

    EnableWindow(HStop, TRUE);
    EnableWindow(HGrab, FALSE);
    EnableWindow(HQuit, FALSE);
Error :
    if(error<0) {
        DisplayIMAQError(error);
        PostMessage(ImaqSmplHwnd, WM_COMMAND, PB_STOP, 0);
    }

   	return 0;
}



DWORD ImaqThread(LPDWORD lpdwParam)
{
    static int nbFrame = 0, error = 0;  
	static int t1 = 0, t2 = 0, currBufNum = 0;
	static int actualCopiedBuffer = 0, prevCopiedBuffer = 0;
    char    buffer[32];
	
    // Create a pointer to the stop boolean. This needs to be
    // volatile because the value can change at any time.
    BOOL* volatile stop = (BOOL*)lpdwParam;

	// the thread stop when StopRing goes to TRUE or there is an error
	while(!*stop && !error)
	{
		t2 = GetTickCount();
		
		// Wait at least for the first valid frame
		errChk(imgGetAttribute (Sid, IMG_ATTR_LAST_VALID_FRAME, &currBufNum));	
		currBufNum++;
		
		// Copy the last valid buffer
		errChk(imgSessionCopyBufferByNumber(Sid, currBufNum, CopyBuffer, IMG_OVERWRITE_GET_NEWEST, &actualCopiedBuffer, NULL));
        
		// Display it using imgPlot
		// Note that if you are using a board or camera with a bitdepth greater
		// that 8 bits, you need to set the flag parameter of imgPlot to match
		// the bit depth of the camera. See the "snap imgPlot" sample. 
		errChk(imgPlot2 (ImaqSmplHwnd, CopyBuffer, 0, 0, AcqWinWidth, AcqWinHeight,
						 CanvasLeft, CanvasTop, plotFlag));
		
		// Calculate the number of frame per seconds every 10 frames
		if ((actualCopiedBuffer-prevCopiedBuffer)>10)
		{
			sprintf(buffer, "%.2f", 1000.0 * (double)(actualCopiedBuffer-prevCopiedBuffer) / (double)(t2-t1));
			SetWindowText (HFrameRate, buffer);
			t1 = t2;
			prevCopiedBuffer = actualCopiedBuffer;
		}

	Error:
		if(error<0 && !*stop)
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
    int i;
    
    // Get a handle to the stop event
    HANDLE event = *((HANDLE*) lpdwParam);

    // Wait for the done event to occur
    dwResult = WaitForSingleObject(event, INFINITE);
    if (dwResult != WAIT_FAILED) {
        CloseHandle(event);
        event = NULL;
    }
    
	// Stop the thread
	StopGrab = TRUE;

	// Wait for the thread to end and kill it otherwise
	dwResult = WaitForSingleObject(HThread, 2000);
	if (dwResult == WAIT_TIMEOUT)
		TerminateThread(HThread, 0);
	
    // stop the acquisition
    imgSessionAbort(Sid, NULL);

    // unlock the buffers in the buffer list
	if (Bid != 0)
		imgMemUnlock(Bid);

	// dispose of the buffers 
	for (i = 0; i < NUM_GRAB_BUFFERS; i++)
		if (ImaqBuffers[i] != NULL)
		    imgDisposeBuffer(ImaqBuffers[i]);

	// close this buffer list
	if (Bid != 0)
		imgDisposeBufList(Bid, FALSE);

	// free our copy buffer
	if (CopyBuffer != NULL)
		free(CopyBuffer);

	// Close the interface and the session
    if(Sid != 0)
	    imgClose (Sid, TRUE);
    if(Iid != 0)
	    imgClose (Iid, TRUE);

    EnableWindow(HStop, FALSE);
    EnableWindow(HGrab, TRUE);
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


