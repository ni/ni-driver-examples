/*****************************************************************************/
/*  This sample demonstrates how to asynchronously and continuously          */
/*  acquire images in multiple buffers from multiple ports using             */
/*  low-level functions.  The cameras attached to each port are not          */
/*  required to be synchronized -- each will run at its respective frame     */
/*  rate.  Image acquisition and display for each port is managed by a       */
/*  separate thread that is spawned at configuration time.                   */
/*                                                                           */
/*  NOTE:  This example requires an IMAQ device that supports multiple       */
/*  ports.                                                                   */
/*****************************************************************************/

//-----------------------------------------------------------------------------
//  Includes
//-----------------------------------------------------------------------------
#include <windows.h>
#include <mmsystem.h>
#include <stdio.h>
#include "LLRing (Dual Port).h"
#define _NIWIN  
#include "niimaq.h"


//-----------------------------------------------------------------------------
//  Defines
//-----------------------------------------------------------------------------
#define NUM_RING_BUFFERS    5
#define NUM_PORTS           2
#define MAX_STRING_LENGTH   64
// Defines for UI controls
#define CANVAS_WIDTH        600
#define CANVAS_HEIGHT       350
#define CTRL_LEFT           650
#define CTRL_HEIGHT         20
#define CTRL_OFFSET         10
// Error checking macro
#define errChk(fCall) if (error = (fCall), error < 0) {goto Error;} else


//-----------------------------------------------------------------------------
//  Typedefs
//-----------------------------------------------------------------------------
typedef struct {
	BUFLIST_ID      Bid;
	SESSION_ID      Sid;
	INTERFACE_ID    Iid;
    BOOL            configured;
	void*           buffers[NUM_RING_BUFFERS];
	int             CanvasWidth, CanvasHeight, CanvasTop, CanvasLeft;
	int             AcqWinWidth, AcqWinHeight;
	int             plotFlag;
	HANDLE          HAcquisitionThread;
	HWND            HFrameRate, HBufNum;
} AcquisitionInfo;


//-----------------------------------------------------------------------------
//  Function Prototypes
//-----------------------------------------------------------------------------
LRESULT CALLBACK ImaqSmplProc(HWND hWnd, WPARAM iMessage, WPARAM wParam, LPARAM lParam);
void DisplayIMAQError(Int32 error);
int OnRing (void);
void ConfigureRing(const char* interfaceName, uInt32 port);
DWORD ImaqAcquisitionThread(LPDWORD lpdwParam);
DWORD StopAcquisitionThread(LPDWORD lpdwParam);


//-----------------------------------------------------------------------------
//  Global Variables
//-----------------------------------------------------------------------------
static HINSTANCE        hInst;
static HWND             ImaqSmplHwnd;
static HWND             HInterfaceName, HStop, HRing, HQuit;
static volatile BOOL   StopAcquisition;
static HANDLE           HStopThread, HStopEvent;
static AcquisitionInfo  acquisitionInfo[NUM_PORTS];


int WINAPI WinMain (HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpszCmdLine, int nCmdShow)
{
	CHAR        ImaqSmplClassName[] = "Imaq Sample";
    WNDCLASS  	ImaqSmplClass;
	MSG			msg;
    HWND        hTemp;
    uInt32      i, position;
    CHAR        displayString[MAX_STRING_LENGTH];

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

    // Create the main windoww
	ImaqSmplHwnd = CreateWindow(ImaqSmplClassName, "LLRing", WS_OVERLAPPEDWINDOW | WS_VISIBLE,
							CW_USEDEFAULT, CW_USEDEFAULT, 800, 800, NULL, NULL, hInstance, NULL);

    // Create the interface name label
    if (!(hTemp = CreateWindow("Static","Interface Name",ES_LEFT | WS_CHILD | WS_VISIBLE,
                                CTRL_LEFT,14,100,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // Create the interface name edit box
    if (!(HInterfaceName = CreateWindow("Edit","img0",ES_LEFT | WS_CHILD | WS_VISIBLE | WS_BORDER,
                                CTRL_LEFT,34,100,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // Create the Grab button
    if (!(HRing = CreateWindow("Button","Ring",BS_PUSHBUTTON | WS_CHILD | WS_VISIBLE | WS_BORDER,
                                CTRL_LEFT,72,80,40,ImaqSmplHwnd,(HMENU)PB_RING,hInstance,NULL)))
        return(FALSE);

    // Create the Stop button
    if (!(HStop = CreateWindow("Button","Stop",BS_PUSHBUTTON | WS_CHILD | WS_VISIBLE | WS_BORDER,
                                CTRL_LEFT,112,80,40,ImaqSmplHwnd,(HMENU)PB_STOP,hInstance,NULL)))
        return(FALSE);

    // Create the Quit button
    if (!(HQuit = CreateWindow("Button","Quit",BS_DEFPUSHBUTTON | WS_CHILD | WS_VISIBLE,
                                CTRL_LEFT,152,80,40,ImaqSmplHwnd,(HMENU)PB_QUIT,hInstance,NULL)))
        return(FALSE);


    // Create the frame rate and buffer number display
    // for the individual ports.
    position = 202;
    for(i = 0; i < NUM_PORTS; ++i )
    {
        AcquisitionInfo* acquisition = &acquisitionInfo[i];

        // Create the image label
        sprintf(displayString, "Port %d", i);
        hTemp = CreateWindow("Static", displayString, ES_LEFT | WS_CHILD | WS_VISIBLE,
                             10, i * (CANVAS_HEIGHT + CTRL_HEIGHT + CTRL_OFFSET) + CTRL_OFFSET, 40, CTRL_HEIGHT, ImaqSmplHwnd, (HMENU)-1, hInstance, NULL);
        if (!hTemp)
            return FALSE;

        // Create the frame rate label
        sprintf(displayString, "Frame Rate Port %d", i);
        hTemp = CreateWindow("Static", displayString, ES_LEFT | WS_CHILD | WS_VISIBLE,
                             CTRL_LEFT, position, 120, CTRL_HEIGHT, ImaqSmplHwnd, (HMENU)-1, hInstance, NULL);
        if (!hTemp)
            return FALSE;

        // Increment the position
        position += CTRL_HEIGHT;

        // Create the frame rate text box
        acquisition->HFrameRate = CreateWindow("Edit", "Unavailable", ES_LEFT | WS_CHILD | WS_VISIBLE | WS_BORDER,
                                               CTRL_LEFT, position, 100, CTRL_HEIGHT, ImaqSmplHwnd, (HMENU)-1, hInstance, NULL);
        if (!acquisition->HFrameRate)
            return FALSE;

        // Increment the position
        position += CTRL_HEIGHT;

        // Create the buffer number label
        sprintf(displayString, "Buffer Number Port %d", i);
        hTemp = CreateWindow("Static", displayString, ES_LEFT | WS_CHILD | WS_VISIBLE,
                             CTRL_LEFT, position, 140, CTRL_HEIGHT, ImaqSmplHwnd, (HMENU)-1, hInstance, NULL);
        if (!hTemp)
            return FALSE;

        // Increment the position
        position += CTRL_HEIGHT;

        // Create the bufer number text box
        acquisition->HBufNum = CreateWindow("Edit", "Unavailable", ES_LEFT | WS_CHILD | WS_VISIBLE | WS_BORDER,
                                            CTRL_LEFT, position, 100, CTRL_HEIGHT, ImaqSmplHwnd, (HMENU)-1, hInstance, NULL);
        if (!acquisition->HBufNum)
            return FALSE;

        // Disable the text boxes
        EnableWindow(acquisition->HFrameRate, FALSE);
        EnableWindow(acquisition->HBufNum, FALSE);

        // Increment the position
        position += CTRL_HEIGHT + 20;
    }

    // Disable the Stop button
    EnableWindow(HStop, FALSE);

    // Display the main window
	ShowWindow(ImaqSmplHwnd, SW_SHOW);
    UpdateWindow(ImaqSmplHwnd);

    // Start getting window messages
	while (GetMessage (&msg, NULL, 0, 0))
    {
        TranslateMessage (&msg) ;
        DispatchMessage (&msg) ;
    }

    // Wait for the stop thread to complete before returning
    WaitForSingleObject(HStopThread, INFINITE);

    return (int)(msg.wParam);
}


//-----------------------------------------------------------------------------
//  ImaqSmplProc
//  This function interprets messages and calls the appropriate function.
//-----------------------------------------------------------------------------
LRESULT CALLBACK ImaqSmplProc (HWND hWnd, WPARAM iMessage, WPARAM wParam, LPARAM lParam)
{
	WORD            wmId;	
	
	switch (iMessage)
	{
		case WM_COMMAND:
			wmId    = LOWORD(wParam);
			switch (wmId)
            {
                case PB_QUIT:
                    // Quit button has been pushed
                    PostQuitMessage(0);
                    break;
				case PB_RING:
                    // Grab button has been pushed
					OnRing();
					break;
                case PB_STOP:
                    // Grab button has been pushed
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


//-----------------------------------------------------------------------------
//  ConfigureRing
//  Configures the interface and port for a ring acquisition.
//-----------------------------------------------------------------------------
void ConfigureRing(const char* interfaceName, uInt32 port) {
    uInt32              i, error;
    char                decoratedInterfaceName[MAX_STRING_LENGTH], displayString[MAX_STRING_LENGTH];
    uInt32              bitsPerPixel, bytesPerPixel, bufferCommand, bufferSize;
    AcquisitionInfo*    acquisition = &acquisitionInfo[port];

    // Create the interface with the port number
    sprintf(decoratedInterfaceName, "%s::%d", interfaceName, port);

    // Open an interface and a session
	errChk(imgInterfaceOpen (decoratedInterfaceName, &acquisition->Iid));
	errChk(imgSessionOpen (acquisition->Iid, &acquisition->Sid));

    // Set the dimensions and location of the canvas
    acquisition->CanvasWidth = CANVAS_WIDTH;
    acquisition->CanvasHeight = CANVAS_HEIGHT;
    acquisition->CanvasLeft = 10;
    // Move the canvas up and down based on the port number.  This
    // means that port 0 will display at the top of the window and
    // port n will display at the bottom of the window.
    acquisition->CanvasTop = port * (CANVAS_HEIGHT + CTRL_HEIGHT + CTRL_OFFSET) + CTRL_HEIGHT + CTRL_OFFSET;

    // Make sure the acquisition window is not larger than the canvas
	errChk(imgGetAttribute (acquisition->Sid, IMG_ATTR_ROI_WIDTH, &acquisition->AcqWinWidth));
	errChk(imgGetAttribute (acquisition->Sid, IMG_ATTR_ROI_HEIGHT, &acquisition->AcqWinHeight));
	
	if(acquisition->CanvasWidth < acquisition->AcqWinWidth)
		acquisition->AcqWinWidth = acquisition->CanvasWidth;
	if(acquisition->CanvasHeight < acquisition->AcqWinHeight)
		acquisition->AcqWinHeight = acquisition->CanvasHeight;
		
	// Get the pixel depth of the acquisition
	errChk(imgGetAttribute (acquisition->Sid, IMG_ATTR_BITSPERPIXEL, &bitsPerPixel));
	
    // Set the corresponding flag for how we will draw images
	switch (bitsPerPixel)
	{
	case 10:
		acquisition->plotFlag = IMGPLOT_MONO_10;
		break;
	case 12:
		acquisition->plotFlag = IMGPLOT_MONO_12;
		break;
	case 14:
		acquisition->plotFlag = IMGPLOT_MONO_14;
		break;
	case 16:
		acquisition->plotFlag = IMGPLOT_MONO_16;
		break;
	case 24:
        // Assume that a 24 bit camera is a 32 bit camera because
        // the driver will still return 32 bits of data.
	case 32:
		acquisition->plotFlag = IMGPLOT_COLOR_RGB32;
		break;
	default:
		acquisition->plotFlag = IMGPLOT_MONO_8;
		break;
	}

	// Set the ROI to the size of the Canvas so that it will fit nicely
	errChk(imgSetAttribute2 (acquisition->Sid, IMG_ATTR_ROI_WIDTH, acquisition->AcqWinWidth));
	errChk(imgSetAttribute2 (acquisition->Sid, IMG_ATTR_ROI_HEIGHT, acquisition->AcqWinHeight));
	errChk(imgSetAttribute2 (acquisition->Sid, IMG_ATTR_ROWPIXELS, acquisition->AcqWinWidth));
	
	// Create a buffer list with one element
    errChk(imgCreateBufList (NUM_RING_BUFFERS, &acquisition->Bid));
    
    // Compute the size of the required buffer
	errChk(imgGetAttribute (acquisition->Sid, IMG_ATTR_BYTESPERPIXEL, &bytesPerPixel));
	bufferSize = acquisition->AcqWinWidth * acquisition->AcqWinHeight * bytesPerPixel;

	/* the following configuration assigns the following to buffer list 
	   element 0:

		    1) buffer pointer that will contain image
		    2) size of the buffer for buffer element 0
		    3) command to loop when this element is reached
    
	 */
	for (i = 0; i < NUM_RING_BUFFERS; i++)
	{
		errChk(imgCreateBuffer(acquisition->Sid, FALSE, bufferSize, &acquisition->buffers[i]));
		errChk(imgSetBufferElement2(acquisition->Bid, i, IMG_BUFF_ADDRESS, acquisition->buffers[i]));
		errChk(imgSetBufferElement2(acquisition->Bid, i, IMG_BUFF_SIZE, bufferSize));
		bufferCommand = (i == (NUM_RING_BUFFERS - 1)) ? IMG_CMD_LOOP : IMG_CMD_NEXT;
		errChk(imgSetBufferElement2(acquisition->Bid, i, IMG_BUFF_COMMAND, bufferCommand));
	}

	// Lock down the buffers contained in the buffer list
	errChk(imgMemLock(acquisition->Bid));

	// Configure the session to use this buffer list
	errChk(imgSessionConfigure(acquisition->Sid, acquisition->Bid));

    // Set the text controls to a beginning state
    sprintf(displayString, "0");
    SetWindowText(acquisition->HFrameRate, displayString);
    SetWindowText(acquisition->HBufNum, displayString);

    // If there is no error, this acquisition is configured
    acquisition->configured = error == 0;

    // See if there were any errors
Error:
    if (error < 0) {
        DisplayIMAQError(error);
        PostMessage(ImaqSmplHwnd, WM_COMMAND, PB_STOP, 0);
    }
}
//-----------------------------------------------------------------------------
//  OnRing
//  The function that gets called when a user clicks Ring.
//-----------------------------------------------------------------------------
int OnRing (void)
{
    uInt32          i;
    char            interfaceName[MAX_STRING_LENGTH];
	DWORD			dwThreadId, threadsStarted = 0;
	
    // Create the event that needs to be signaled when we
    // wish to stop the acquisition.
    HStopEvent = CreateEvent(NULL, TRUE, FALSE, NULL);
    if (!HStopEvent)
        return 0;

    // Create the thread that is responsible for shutting
    // down the acquisition
    HStopThread = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)StopAcquisitionThread, (LPDWORD)&HStopEvent, 0, &dwThreadId);
    if (!HStopThread)
        return 0;

    // Get the interface name
	GetWindowText(HInterfaceName, interfaceName, MAX_STRING_LENGTH);

    // Iterate over the number of ports to configure the acquisition
    for (i = 0; i < NUM_PORTS; ++i)
        ConfigureRing(interfaceName, i);

    // Configure the stop condition for the acquisition
    StopAcquisition = FALSE;

    // Configure the UI elements
    EnableWindow(HStop, TRUE);
    EnableWindow(HRing, FALSE);
    EnableWindow(HQuit, FALSE);

    // After the ports have been configured, start the acquisition threads
    for (i = 0; i < NUM_PORTS; ++i)
    {
        AcquisitionInfo* acquisition = &acquisitionInfo[i];
        if (acquisition->configured)
        {
            acquisition->HAcquisitionThread = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)ImaqAcquisitionThread, (LPDWORD*)acquisition, 0, &dwThreadId);
            ++threadsStarted;
        }
    }

    // If no threads were started, display a dialog and stop the acquisitions
    if (threadsStarted == 0)
    {
        MessageBox(ImaqSmplHwnd, "No acquisitions were started", "Example Error", MB_OK | MB_ICONERROR);
        PostMessage(ImaqSmplHwnd, WM_COMMAND, PB_STOP, 0);
    }

   	return 0;
}


//-----------------------------------------------------------------------------
//  ImaqAcquisitionThread
//  Starts an acquisition.
//-----------------------------------------------------------------------------
DWORD ImaqAcquisitionThread(LPDWORD lpdwParam)
{
    int                 frameNumber = 0, error = 0;  
	int                 startTime = 0, endTime = 0;
    int                 bufferIndex;
    char                displayString[MAX_STRING_LENGTH];
    AcquisitionInfo*    acquisition = (AcquisitionInfo*)lpdwParam;

    // Start the acquisition
    error = imgSessionAcquire(acquisition->Sid, TRUE, NULL);

    // Enable the display windows
    EnableWindow(acquisition->HFrameRate, TRUE);
    EnableWindow(acquisition->HBufNum, TRUE);

    bufferIndex = 0;
	// Acquire until StopAcquisition goes to TRUE or there is an error
	while (!StopAcquisition && !error)
	{
        uInt32 lastAcquiredBuffer; 
        void* bufferAddress = NULL;

        // Get the start time
		endTime = GetTickCount();
		
		// Hold the buffer whose index is bufferIndex. This is a cumulative buffer
		// index, if the buffer has not been acquired yet this function will block
		// until it is available. If the buffer has been overwritten this function
		// will return the last available buffer. Once the buffer is hold the buffer
		// won't be overwritten until it is released with imgSessionReleaseBuffer.
		error = imgSessionExamineBuffer2(acquisition->Sid, bufferIndex, &lastAcquiredBuffer, &bufferAddress);
        if (error)
            break;

		// Display the index of the last acquired buffer
		sprintf(displayString, "%d", lastAcquiredBuffer);
		SetWindowText(acquisition->HBufNum, displayString);

		// Display it using imgPlot
		// Note that if you are using a board or camera with a bitdepth greater
		// that 8 bits, you need to set the flag parameter of imgPlot to match
		// the bit depth of the camera. See the "snap imgPlot" sample. 
		imgPlot2(ImaqSmplHwnd, bufferAddress, 0, 0,
                        acquisition->AcqWinWidth, acquisition->AcqWinHeight,
                        acquisition->CanvasLeft, acquisition->CanvasTop, acquisition->plotFlag);
		
		// Release the buffer
		error = imgSessionReleaseBuffer(acquisition->Sid);
        if (error)
            break;
		
		// Now get the next buffer
		bufferIndex++;
		
		// Calculate the frame rate every 10 frames
		frameNumber++;
		if (frameNumber > 10)
		{
			sprintf(displayString, "%.2f", 1000.0 * (double)frameNumber / (double)(endTime - startTime));
			SetWindowText (acquisition->HFrameRate, displayString);
			startTime = endTime;
			frameNumber = 0;
		}
	}

    // If there was an error and the acquisition hasn't been stopped,
    // display the error and stop the acquisition.
    if (error < 0 && !StopAcquisition)
    {
        DisplayIMAQError(error);
        PostMessage(ImaqSmplHwnd, WM_COMMAND, PB_STOP, 0);
    }

    return 0;
}


//-----------------------------------------------------------------------------
//  StopAcquisitionThread
//  The function that gets called when a user clicks Stop.
//-----------------------------------------------------------------------------
DWORD StopAcquisitionThread(LPDWORD lpdwParam)
{
    uInt32 port, bufferNumber;
    DWORD result;
    
    // Get a handle to the stop event
    HANDLE event = *((HANDLE*) lpdwParam);

    // Wait for the done event to occur
    result = WaitForSingleObject(event, INFINITE);
    if (result != WAIT_FAILED) {
        CloseHandle(event);
        event = NULL;
    }

    // Stop the threads
	StopAcquisition = TRUE;

    // To stop the acquisition:
    //   1) Stop the threads
    //   2) Stop the acquisition
    //   3) Cleanup the image buffers
    //   4) Close the session
    for(port = 0; port < NUM_PORTS; ++port) {
        AcquisitionInfo* acquisition = &acquisitionInfo[port];

        // Cleanup the threads
        if (acquisition->HAcquisitionThread != INVALID_HANDLE_VALUE)
        {
            // Wait for the thread to finish
	        DWORD waitResult = WaitForSingleObject(acquisition->HAcquisitionThread, 5000);
            // If the wait timed out, terminate the thread
	        if (waitResult == WAIT_TIMEOUT)
		        TerminateThread(acquisition->HAcquisitionThread, 0);
            // If the wait was successful, close the thread handle
            if (waitResult != WAIT_FAILED)
                CloseHandle(acquisition->HAcquisitionThread);
        }

        // Stop the acquisition
        imgSessionAbort(acquisition->Sid, &bufferNumber);

        // Unlock the buffer list and dispose it
        imgMemUnlock(acquisition->Bid);
        imgDisposeBufList(acquisition->Bid, TRUE);

        // Close the session and interface
	    imgClose(acquisition->Sid, TRUE);
	    imgClose(acquisition->Iid, TRUE);

        // Reset the session variables
        acquisition->Bid = acquisition->Sid = acquisition->Iid = 0;

        // Set the user interface components to their startup states.
        EnableWindow(acquisition->HBufNum, FALSE);
        EnableWindow(acquisition->HFrameRate, FALSE);
    }

    // Set the user interface components to their startup states.
    EnableWindow(HStop, FALSE);
    EnableWindow(HRing, TRUE);
    EnableWindow(HQuit, TRUE);

    // Return
    return 0;
}



//-----------------------------------------------------------------------------
//  DisplayIMAQError
//  If the value passed in is an error, displays the error description.
//-----------------------------------------------------------------------------
void DisplayIMAQError(Int32 error)
{
    Int8 message[256];
    memset(message, 0, sizeof(message));

    // Get the description associated with the error code
    imgShowError(error, message);

    // Display a message box with the error description
    MessageBox(NULL, message, "Imaq Sample", MB_OK | MB_ICONERROR);
}


