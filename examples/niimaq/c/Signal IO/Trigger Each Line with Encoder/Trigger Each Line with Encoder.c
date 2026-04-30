/*****************************************************************************/
/*		This sample demonstrates how to continuously acquire pictures        */
/*      in multiple buffers using low level functions				         */
/*****************************************************************************/  

#include <windows.h>
#include <mmsystem.h>
#include <stdio.h>
#include "Trigger Each Line with Encoder.h"
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
DWORD ImaqThread(LPDWORD lpdwParam);
DWORD ImaqEncoderPosition(LPDWORD lpdwParam);
DWORD StopThread(LPDWORD lpdwParam);
// Thread objects
static HANDLE HThread;
static HANDLE HStopThread, HStopEvent;
static HANDLE HEncoderPositionThread;


// windows GUI globals
static HINSTANCE    hInst;
static HWND         ImaqSmplHwnd;
static HWND         HStop, HRing, HQuit, HIntfName, HFrameRate, HBufNum, HEncoderPosition;
static HWND         HPhaseAPolarity, HPhaseBPolarity, HDivideFactor, HFilter;


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
static BOOL			StopRing = FALSE;
static unsigned int	plotFlag; 


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
	ImaqSmplHwnd = CreateWindow(ImaqSmplClassName, "Trigger Each Line with Encoder", WS_OVERLAPPEDWINDOW | WS_VISIBLE,
							CW_USEDEFAULT, CW_USEDEFAULT, 680, 600, NULL, NULL, hInstance, NULL);

    // creates the interface name label
    if (!(hTemp = CreateWindow("Static","Interface Name",ES_LEFT | WS_CHILD | WS_VISIBLE,
                                540,14,100,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // creates the interface name edit box
    if (!(HIntfName = CreateWindow("Edit","img0",ES_LEFT | WS_CHILD | WS_VISIBLE | WS_BORDER,
                                540,34,100,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // creates the Grab button
    if (!(HRing = CreateWindow("Button","Ring",BS_PUSHBUTTON | WS_CHILD | WS_VISIBLE | WS_BORDER,
                                550,72,80,40,ImaqSmplHwnd,(HMENU)PB_RING,hInstance,NULL)))
        return(FALSE);

    // creates the stop button
    if (!(HStop = CreateWindow("Button","Stop",BS_PUSHBUTTON | WS_CHILD | WS_VISIBLE | WS_BORDER,
                                550,112,80,40,ImaqSmplHwnd,(HMENU)PB_STOP,hInstance,NULL)))
        return(FALSE);

    // creates the quit application button
    if (!(HQuit = CreateWindow("Button","Quit",BS_DEFPUSHBUTTON | WS_CHILD | WS_VISIBLE,
                                550,152,80,40,ImaqSmplHwnd,(HMENU)PB_QUIT,hInstance,NULL)))
        return(FALSE);

    // create the Phase A Polarity label
    if (!(hTemp = CreateWindow("Static","Phase A Polarity",ES_LEFT | WS_CHILD | WS_VISIBLE,
                                540,202,140,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // create the Phase A Polarity combobox
    if (!(HPhaseAPolarity = CreateWindow("ComboBox","External", CBS_DROPDOWNLIST | WS_CHILD | WS_VISIBLE,
                                540,222,120,200,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // create the Phase B Polarity label
    if (!(hTemp = CreateWindow("Static","Phase B Polarity",ES_LEFT | WS_CHILD | WS_VISIBLE,
                                540,252,140,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // create the Phase B Polarity combobox
    if (!(HPhaseBPolarity = CreateWindow("ComboBox","External", CBS_DROPDOWNLIST | WS_CHILD | WS_VISIBLE,
                                540,272,120,200,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // creates the divide factor label
    if (!(hTemp = CreateWindow("Static","Divide Factor",ES_LEFT | WS_CHILD | WS_VISIBLE,
                                540,302,140,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // creates the divide factor edit box
    if (!(HDivideFactor = CreateWindow("Edit","2",ES_LEFT | WS_CHILD | WS_VISIBLE | WS_BORDER,
                                540,322,100,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // creates the filter checkbox
    if (!(HFilter = CreateWindow("Button","Filter?",BS_AUTOCHECKBOX | WS_CHILD | WS_VISIBLE,
                                540,352,80,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // creates the frame rate label
    if (!(hTemp = CreateWindow("Static","Frame Rate",ES_LEFT | WS_CHILD | WS_VISIBLE,
                                540,382,140,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // creates the frame rate edit box
    if (!(HFrameRate = CreateWindow("Edit","0",ES_LEFT | WS_CHILD | WS_VISIBLE | WS_BORDER,
                                540,402,100,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // creates the buffer number label
    if (!(hTemp = CreateWindow("Static","Buffer Number",ES_LEFT | WS_CHILD | WS_VISIBLE,
                                540,432,140,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // creates the buffer number edit box
    if (!(HBufNum = CreateWindow("Edit","0",ES_LEFT | WS_CHILD | WS_VISIBLE | WS_BORDER,
                                540,452,100,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // creates the encoder position label
    if (!(hTemp = CreateWindow("Static","Encoder Position",ES_LEFT | WS_CHILD | WS_VISIBLE,
                                540,482,140,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // creates the buffer number edit box
    if (!(HEncoderPosition = CreateWindow("Edit","0",ES_LEFT | WS_CHILD | WS_VISIBLE | WS_BORDER,
                                540,502,100,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // disable the appropriate user interface elements
    EnableWindow(HStop, FALSE);
    EnableWindow(HBufNum, FALSE);
    EnableWindow(HFrameRate, FALSE);
    EnableWindow(HEncoderPosition, FALSE);

    // fill in the polarity combo boxes
    SendMessage(HPhaseAPolarity, CB_ADDSTRING, 0, (LPARAM)"Active High");
    SendMessage(HPhaseAPolarity, CB_ADDSTRING, 0, (LPARAM)"Active Low");
    SendMessage(HPhaseBPolarity, CB_ADDSTRING, 0, (LPARAM)"Active High");
    SendMessage(HPhaseBPolarity, CB_ADDSTRING, 0, (LPARAM)"Active Low");

    // set the data associated with the combo box data
    SendMessage(HPhaseAPolarity, CB_SETITEMDATA, 0, (LPARAM)IMG_TRIG_POLAR_ACTIVEH);
    SendMessage(HPhaseAPolarity, CB_SETITEMDATA, 1, (LPARAM)IMG_TRIG_POLAR_ACTIVEL);
	SendMessage(HPhaseBPolarity, CB_SETITEMDATA, 0, (LPARAM)IMG_TRIG_POLAR_ACTIVEH);
    SendMessage(HPhaseBPolarity, CB_SETITEMDATA, 1, (LPARAM)IMG_TRIG_POLAR_ACTIVEL);

    // select the default item in the polarity combo box
    SendMessage(HPhaseAPolarity, CB_SETCURSEL, 0, 0);
    SendMessage(HPhaseBPolarity, CB_SETCURSEL, 0, 0);

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
	unsigned int	bitsPerPixel;
    unsigned int    phaseAPolarity, phaseBPolarity, divideFactor, filter;
    char            intfName[64], displayString[64];
	DWORD			dwThreadId;
    BOOL            canGetEncoderPosition;
	
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

    // Not all IMAQ devices support reading the encoder position.  Determine whether this device does.
    // Note that we temporarily disable breaking on library errors because we want to manually handle
    // the potential error we get from calling imgGetAttribute.
    {
        uInt64 encoderPosition;
        canGetEncoderPosition = imgGetAttribute(Sid, IMG_ATTR_ENCODER_POSITION, &encoderPosition) == IMG_ERR_GOOD;
    }

    // The following unsigned int casts are safe because we only ever expect 32-bit values.
    // Get the encoder polarity settings
    phaseAPolarity = (unsigned int)SendMessage(HPhaseAPolarity, CB_GETCURSEL, 0, 0);
    phaseBPolarity = (unsigned int)SendMessage(HPhaseBPolarity, CB_GETCURSEL, 0, 0);
    // Get the divide factor and convert it to an integer
	GetWindowText(HDivideFactor, displayString, 64);
    divideFactor = atoi(displayString);
    // Get the filter setting
    filter = (unsigned int)SendMessage(HFilter, BM_GETCHECK, 0, 0);

    // Configure the encoder parameters.  These parameters are used to derive the scaled
    errChk(imgSetAttribute2(Sid, IMG_ATTR_ENCODER_PHASE_A_POLARITY, phaseAPolarity));
    errChk(imgSetAttribute2(Sid, IMG_ATTR_ENCODER_PHASE_B_POLARITY, phaseBPolarity));
    errChk(imgSetAttribute2(Sid, IMG_ATTR_ENCODER_DIVIDE_FACTOR, divideFactor));
    errChk(imgSetAttribute2(Sid, IMG_ATTR_ENCODER_FILTER, filter));

    // Tell the acquisition to use a line trigger, which should be derived from the scaled encoder signal.
    errChk(imgSessionLineTrigSource2(Sid, IMG_SIGNAL_SCALED_ENCODER, 0, IMG_TRIG_POLAR_ACTIVEH, 0));

	// configure the session to use this buffer list
	errChk(imgSessionConfigure(Sid, Bid));

	// start the acquisition, asynchronous
	errChk(imgSessionAcquire(Sid, TRUE, NULL));

    // Set the text controls to a beginning state
    sprintf(displayString, "0");
    SetWindowText(HFrameRate, displayString);
    SetWindowText(HBufNum, displayString);
    SetWindowText(HEncoderPosition, displayString);

    StopRing = FALSE;

    // Start the acquisition thread
	HThread = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE) ImaqThread, (LPDWORD*)&StopRing, 0, &dwThreadId);               
    if (HThread == NULL) 
        PostMessage(ImaqSmplHwnd, WM_COMMAND, PB_STOP, 0);

    // Start the encoder position thread if this board is supported
    if(canGetEncoderPosition)
        HEncoderPositionThread = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)ImaqEncoderPosition, (LPDWORD*)&StopRing, 0, &dwThreadId);

    // Enable the stop button and text displays. Disable the other controls.
    EnableWindow(HStop, TRUE);
    EnableWindow(HRing, FALSE);
    EnableWindow(HQuit, FALSE);
    EnableWindow(HBufNum, TRUE);
    EnableWindow(HFrameRate, TRUE);
    EnableWindow(HPhaseAPolarity, FALSE);
    EnableWindow(HPhaseBPolarity, FALSE);
    EnableWindow(HDivideFactor, FALSE);
    EnableWindow(HFilter, FALSE);

Error :
    if(error<0) {
        DisplayIMAQError(error);
        PostMessage(ImaqSmplHwnd, WM_COMMAND, PB_STOP, 0);
    }

   	return 0;
}



DWORD ImaqThread(LPDWORD lpdwParam)
{
    int nbFrame = 0, error = 0;  
	int t1 = 0, t2 = 0;
    int bufferIndex = 0, currBufNum;
    void* bufAddr = NULL;
    char displayString[32];
	
    // Create a pointer to the stop boolean. This needs to be
    // volatile because the value can change at any time.
    BOOL* volatile stop = (BOOL*)lpdwParam;

	// the thread stop when StopRing goes to TRUE or there is an error
	while(!*stop && !error)
	{
		t2 = GetTickCount();
		
		// Hold the buffer whose index is bufferIndex. This is a cumulative buffer
		// index, if the buffer has not been acquired yet this function will block
		// until it is available. If the buffer has been overwritten this function
		// will return the last available buffer. Once the buffer is hold the buffer
		// won't be overwritten until it is released with imgSessionReleaseBuffer.
		errChk(imgSessionExamineBuffer2 (Sid, bufferIndex, &currBufNum, &bufAddr));
								 
		// Display the index of the last acquired buffer
		sprintf(displayString, "%d", currBufNum);
		SetWindowText (HBufNum, displayString);
								 
		// Display it using imgPlot
		// Note that if you are using a board or camera with a bitdepth greater
		// that 8 bits, you need to set the flag parameter of imgPlot to match
		// the bit depth of the camera. See the "snap imgPlot" sample. 
		errChk(imgPlot2 (ImaqSmplHwnd, bufAddr, 0, 0, AcqWinWidth, 
				AcqWinHeight, CanvasLeft, CanvasTop, plotFlag));
		
		// reinsert the buffer back in the ring
		errChk(imgSessionReleaseBuffer (Sid));
		
		// Now get next buffer
		bufferIndex ++;
		
		// Calculate the frame rate every 10 frames
		nbFrame++;
		if (nbFrame>10)
		{
			sprintf(displayString, "%.2f", 1000.0 * (double)nbFrame / (double)(t2-t1));
			SetWindowText (HFrameRate, displayString);
			t1 = t2;
			nbFrame=0;
		}

Error:
        // if there is an error, stop the acquisition and display the error
        if(error < 0) {
            DisplayIMAQError(error);
            PostMessage(ImaqSmplHwnd, WM_COMMAND, PB_STOP, 0);
        }
	}

    return 0;
}



DWORD ImaqEncoderPosition(LPDWORD lpdwParam) {
    char encoderPositionString[256];
    uInt64 encoderPosition;
    IMG_ERR error = IMG_ERR_GOOD;

    // the thread stops when StopRing goes to TRUE or an error occurs
    while(*((BOOL*)lpdwParam) == FALSE && !error) {
        // since this is fairly low priority, sleep for awhile
        Sleep(50);

        // get the current encoder position
        error = imgGetAttribute(Sid, IMG_ATTR_ENCODER_POSITION, &encoderPosition);

        // update the text
        sprintf(encoderPositionString, "%d", (uInt32)encoderPosition);
        SetWindowText(HEncoderPosition, encoderPositionString);
    }

    // if there is an error, stop the acquisition and display the error
    if(error < 0) {
        DisplayIMAQError(error);
        PostMessage(ImaqSmplHwnd, WM_COMMAND, PB_STOP, 0);
    }
    
    // return normally
    return 0;
}


// Waits for the stop event to occur, then stops the acquisition.
DWORD StopThread(LPDWORD lpdwParam) {
    int i;
    DWORD dwResult;

    // Get a handle to the stop event
    HANDLE event = *((HANDLE*) lpdwParam);

    // Wait for the done event to occur
    dwResult = WaitForSingleObject(event, INFINITE);
    if (dwResult != WAIT_FAILED) {
        CloseHandle(event);
        event = NULL;
    }
    
    // Stop the threads
	StopRing = TRUE;

	// Wait for the thread to end and kill it otherwise
	dwResult = WaitForSingleObject(HThread, 5000);
	if (dwResult == WAIT_TIMEOUT)
		TerminateThread(HThread, 0);
    CloseHandle (HThread);

    // Wait for the encoder position thread to end
    dwResult = WaitForSingleObject(HEncoderPositionThread, 5000);
    if (dwResult == WAIT_TIMEOUT)
        TerminateThread(HEncoderPositionThread, 0);
    CloseHandle(HEncoderPositionThread);
    
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

    // Set the user interface components to their startup states.
    EnableWindow(HStop, FALSE);
    EnableWindow(HRing, TRUE);
    EnableWindow(HQuit, TRUE);
    EnableWindow(HBufNum, FALSE);
    EnableWindow(HFrameRate, FALSE);
    EnableWindow(HPhaseAPolarity, TRUE);
    EnableWindow(HPhaseBPolarity, TRUE);
    EnableWindow(HDivideFactor, TRUE);
    EnableWindow(HFilter, TRUE);

    // Reset the session variables.
    Sid = Bid = Iid = 0;

    return 0;
}



// in case of error this function will display a dialog box
// with the error message
void DisplayIMAQError(Int32 error)
{
    Int8 ErrorMessage[256];
    memset(ErrorMessage, 0, sizeof(ErrorMessage));

    // converts error code to a message
    imgShowError(error, ErrorMessage);

    MessageBox(ImaqSmplHwnd, ErrorMessage, "Imaq Sample", MB_OK);
}


