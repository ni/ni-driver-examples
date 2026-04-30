/*****************************************************************************/
/*		This sample demonstrates how to continuously acquire pictures        */
/*      in multiple buffers using low level functions				         */
/*****************************************************************************/  

#define _NIWIN

#include <windows.h>
#include <mmsystem.h>
#include <stdio.h>
#include "VHA Ring.h"
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
DWORD ImaqThread(LPDWORD lpdwParam);
DWORD StopThread(LPDWORD lpdwParam);
// Thread objects
static HANDLE HThread;
static HANDLE HStopThread, HStopEvent;


// windows GUI globals
static HINSTANCE    hInst;
static HWND         ImaqSmplHwnd;
static HWND         HStop, HRing, HQuit, HIntfName, HFrameRate, HBufNum, HImageHeight;


// Imaq globals
static SESSION_ID   Sid = 0;
static BUFLIST_ID   Bid = 0;
static INTERFACE_ID Iid = 0;
static Int8         *ImaqBuffers[NUM_RING_BUFFERS];   // acquisiton buffer
static Int32        CanvasWidth = 450;  // width of the display area
static Int32        CanvasHeight = 550; // height of the display area
static Int32        CanvasTop = 10;     // top of the display area
static Int32        CanvasLeft = 10;    // left of the display area
static Int32        AcqWinWidth;
static Int32        AcqWinHeight;
static uInt32		BufNum;
static BOOL		StopRing = FALSE; 
static Int32		ActualHeight = 0;
static uInt8		zeroArray[1024*5000];	// This array is used to clear the display between
											// acquisitions. Arbitrarily set to 1024 X 5000


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
	ImaqSmplHwnd = CreateWindow(ImaqSmplClassName, "VHA Ring", WS_OVERLAPPEDWINDOW | WS_VISIBLE,
							CW_USEDEFAULT, CW_USEDEFAULT, 640, 600, NULL, NULL, hInstance, NULL);

    
    // creates the interface name label
    if (!(hTemp = CreateWindow("Static","Interface Name",ES_LEFT | WS_CHILD | WS_VISIBLE,
                                525,14,100,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // creates the frame rate label
    if (!(hTemp = CreateWindow("Static","Frame Rate",ES_LEFT | WS_CHILD | WS_VISIBLE,
                                525,232,140,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // creates the buffer number label
    if (!(hTemp = CreateWindow("Static","Buffer Number",ES_LEFT | WS_CHILD | WS_VISIBLE,
                                525,292,140,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // creates the image height label
    if (!(hTemp = CreateWindow("Static","Image Height",ES_LEFT | WS_CHILD | WS_VISIBLE,
                                525,352,140,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // creates the interface name edit box
    if (!(HIntfName = CreateWindow("Edit","img0",ES_LEFT | WS_CHILD | WS_VISIBLE | WS_BORDER,
                                525,34,100,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // creates the frame rate edit box
    if (!(HFrameRate = CreateWindow("Edit","0",ES_LEFT | WS_CHILD | WS_VISIBLE | WS_BORDER,
                                525,252,100,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // creates the image height edit box
    if (!(HImageHeight = CreateWindow("Edit","0",ES_LEFT | WS_CHILD | WS_VISIBLE | WS_BORDER,
                                525,372,100,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);
   // creates the buffer number edit box
    if (!(HBufNum = CreateWindow("Edit","0",ES_LEFT | WS_CHILD | WS_VISIBLE | WS_BORDER,
                                525,312,100,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // creates the Grab button
    if (!(HRing = CreateWindow("Button","Ring",BS_PUSHBUTTON | WS_CHILD | WS_VISIBLE | WS_BORDER,
                                525,72,80,40,ImaqSmplHwnd,(HMENU)PB_RING,hInstance,NULL)))
        return(FALSE);

    // creates the stop button
    if (!(HStop = CreateWindow("Button","Stop",BS_PUSHBUTTON | WS_CHILD | WS_VISIBLE | WS_BORDER,
                                525,112,80,40,ImaqSmplHwnd,(HMENU)PB_STOP,hInstance,NULL)))
        return(FALSE);

    EnableWindow(HStop, FALSE);
    EnableWindow(HBufNum, FALSE);
    EnableWindow(HFrameRate, FALSE);

    // creates the quit application button
    if (!(HQuit = CreateWindow("Button","Quit",BS_DEFPUSHBUTTON | WS_CHILD | WS_VISIBLE,
                                525,152,80,40,ImaqSmplHwnd,(HMENU)PB_QUIT,hInstance,NULL)))
      return(FALSE);
	
    // Display the main window
	ShowWindow(ImaqSmplHwnd, SW_SHOW);
    UpdateWindow(ImaqSmplHwnd);
	
	// Fill empty array with zeroes. This will be used to clear the display
	// later
	for (i = 0; i < (1024*5000); i++)
		zeroArray[i] = 255;
    	
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
                    // Quit button has been hitten
                    PostQuitMessage(0);
                    break;
				case PB_RING:
                    // Grab button has been hitten
					OnRing();
					break;
                case PB_STOP:
                    // Grab button has been hitten
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
    unsigned int    bufCmd, bufSize, bytesPerPixel;
    char	        intfName[64];
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
	
	// Let's get the size of the acquision width and max allowable height
	errChk(imgGetAttribute (Sid, IMG_ATTR_ROI_WIDTH, &AcqWinWidth));
	errChk(imgGetAttribute (Sid, IMG_ATTR_ROI_HEIGHT, &AcqWinHeight));
	
	// create a buffer list with one element
    errChk(imgCreateBufList(NUM_RING_BUFFERS, &Bid));
    
    // compute the size of the required buffer
	errChk(imgGetAttribute (Sid, IMG_ATTR_BYTESPERPIXEL, &bytesPerPixel));
	bufSize = AcqWinWidth * AcqWinHeight * bytesPerPixel;

	/* the following configuration assigns the following to buffer list 
	   element 0:

		    1) buffer pointer that will contain image
		    2) size of the buffer for buffer element 0
		    3) command to loop when this element is reached
    
	 */
	for (i = 0; i < NUM_RING_BUFFERS; i++)
	{
		errChk(imgCreateBuffer(Sid, FALSE, bufSize, &ImaqBuffers[i]));
		errChk(imgSetBufferElement2(Bid, i, IMG_BUFF_ADDRESS, ImaqBuffers[i]));
		errChk(imgSetBufferElement2(Bid, i, IMG_BUFF_SIZE, bufSize));
		bufCmd = (i == (NUM_RING_BUFFERS - 1)) ? IMG_CMD_LOOP : IMG_CMD_NEXT;
		errChk(imgSetBufferElement2(Bid, i, IMG_BUFF_COMMAND, bufCmd));
	}

	// lock down the buffers contained in the buffer list
	errChk(imgMemLock(Bid));

    	
	/* Setup board for variable line acquisition. This is done by setting
    the VHA attribute and then configuring the board to trigger each buffer.
    In VHA mode the board will begin acquistion when the trigger is asserted
    and will terminate the current buffer when the trigger is unasserted. When 
    the trigger line is asserted again, acquistion into the next buffer will
    begin.*/
	    
	imgSetAttribute2 (Sid, IMG_ATTR_VHA_MODE, TRUE);
    imgSessionTriggerConfigure2(Sid, IMG_SIGNAL_EXTERNAL, 
                                0, IMG_TRIG_POLAR_ACTIVEH, 
                                5000, IMG_TRIG_ACTION_BUFFER);
	
	// configure the session to use this buffer list
	errChk(imgSessionConfigure(Sid, Bid));

	// start the acquisition, asynchronous
	errChk(imgSessionAcquire(Sid, TRUE, NULL));
					   
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
Error :
    if(error<0) {
        DisplayIMAQError(error);
        PostMessage(ImaqSmplHwnd, WM_COMMAND, PB_STOP, 0);
    }

   	return 0;
}



DWORD ImaqThread(LPDWORD lpdwParam)
{
    static int nbFrame = 0, error;  
	static int t1, t2, currBufNum;
    void* bufAddr = NULL;
    char    buffer[32];
	static int topOffset, leftOffset;
	static int	displayHeight, displayWidth;
	
    // Create a pointer to the stop boolean. This needs to be
    // volatile because the value can change at any time.
    BOOL* volatile stop = (BOOL*)lpdwParam;

	// the thread stop when StopRing goes to TRUE or there is an error
	while(!*stop && !error)
	{
		t2 = GetTickCount();
		
		// Hold the buffer whose index is BufNum. This is a cumulative buffer
		// index, if the buffer has not been acquired yet this function will block
		// until it is available. If the buffer has been overwritten this function
		// will return the last available buffer. Once the buffer is hold the buffer
		// won't be overwritten until it is released with imgSessionReleaseBuffer.
		errChk(imgSessionExamineBuffer2 (Sid, BufNum, &currBufNum, &bufAddr));
								 
		// Clear display (use ActualHeight value from previous iteration
		errChk(imgPlot2 (ImaqSmplHwnd, zeroArray, 0, 0, CanvasWidth, 
				CanvasHeight, CanvasLeft, CanvasTop, FALSE));

		/* Get the actual height of the image and display it. You must pass this
		function the actual index of the buffer rather than the cumulative buffer
		number for the second parameter.  This is done with the mod operator (%) */
		imgGetBufferElement(Bid,(currBufNum % NUM_RING_BUFFERS), 
                                   IMG_BUFF_ACTUALHEIGHT,
                                   &ActualHeight);				 

		
		// Display height of image
		sprintf(buffer, "%d", ActualHeight);
		SetWindowText (HImageHeight, buffer);

		// Display the index of the last acquired buffer
		sprintf(buffer, "%d", currBufNum);
		SetWindowText (HBufNum, buffer);

		// Display it using imgPlot
		// Note that if you are using a board or camera with a bitdepth greater
		// that 8 bits, you need to set the flag parameter of imgPlot to match
		// the bit depth of the camera. See the "snap imgPlot" sample. 

		// If either image dimension is too large to fit on the canvas, shift the
		// image using top and left offset so that it will fit. This will cause
		// the image to be"cropped" at the top and left for display, but the full
		// image will still exist im memory
		if (ActualHeight > CanvasHeight) {
			topOffset = ActualHeight - CanvasHeight;
			displayHeight = CanvasHeight;
		}
		else {
			topOffset = 0;
			displayHeight = ActualHeight;
		}

		if (AcqWinWidth > CanvasWidth)	{
			leftOffset = AcqWinWidth - CanvasWidth;
			displayWidth = CanvasWidth;
		}
		else {
			leftOffset = 0;
			displayWidth = AcqWinWidth;
		}

		errChk(imgPlot2 (ImaqSmplHwnd, bufAddr, leftOffset,
			topOffset, displayWidth, displayHeight, CanvasLeft, CanvasTop, IMGPLOT_MONO_8));
		
		// reinsert the buffer back in the ring
		errChk(imgSessionReleaseBuffer (Sid));
		
		// Now get next buffer
		BufNum ++;
		
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
	StopRing = TRUE;

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
	for (i = 0; i < NUM_RING_BUFFERS; i++)
		if (ImaqBuffers[i] != NULL)
		    imgDisposeBuffer(ImaqBuffers[i]);

	// close this buffer list
	if (Bid != 0)
		imgDisposeBufList(Bid, FALSE);

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


