/*****************************************************************************/
/*		This sample demonstrates how to acquire  a picture				     */
/*      using low level functions								     		 */
/*****************************************************************************/    

#include <windows.h>
#include "llsnap.h"
#define _NIWIN  
#include "niimaq.h"


// error checking macro
#define errChk(fCall) if (error = (fCall), error < 0) {goto Error;} else


// Window proc
LRESULT CALLBACK ImaqSmplProc(HWND hWnd, WPARAM iMessage, WPARAM wParam, LPARAM lParam);
// Error display function
void DisplayIMAQError(Int32 error);
// Snap Callback
int OnSnap (void);


// windows GUI globals
static HINSTANCE    hInst;
static HWND    	    ImaqSmplHwnd;
static HWND         HSnap, HQuit, HIntfName;

// Imaq globals
static SESSION_ID   Sid = 0;
static BUFLIST_ID   Bid = 0;
static INTERFACE_ID Iid = 0;
static Int8         *ImaqBuffer=NULL;    // acquisiton buffer
static Int32        CanvasWidth = 512;  // width of the display area
static Int32        CanvasHeight = 384; // height of the display area
static Int32        CanvasTop = 10;     // top of the display area
static Int32        CanvasLeft = 10;    // left of the display area


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
	ImaqSmplHwnd = CreateWindow(ImaqSmplClassName, "LLSnap", WS_OVERLAPPEDWINDOW | WS_VISIBLE,
							CW_USEDEFAULT, CW_USEDEFAULT, 680, 440, NULL, NULL, hInstance, NULL);

    // creates the Snap button
    if (!(HSnap = CreateWindow("Button","Snap",BS_PUSHBUTTON | WS_CHILD | WS_VISIBLE | WS_BORDER,
                                550,72,80,40,ImaqSmplHwnd,(HMENU)PB_SNAP,hInstance,NULL)))
        return(FALSE);

    // creates the interface name label
    if (!(hTemp = CreateWindow("Static","Interface name",ES_LEFT | WS_CHILD | WS_VISIBLE,
                                540,14,100,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // creates the interface name edit box
    if (!(HIntfName = CreateWindow("Edit","img0",ES_LEFT | WS_CHILD | WS_VISIBLE | WS_BORDER,
                                540,34,100,20,ImaqSmplHwnd,(HMENU)-1,hInstance,NULL)))
        return(FALSE);

    // creates the quit application button
    if (!(HQuit = CreateWindow("Button","Quit",BS_DEFPUSHBUTTON | WS_CHILD | WS_VISIBLE,
                                550,112,80,40,ImaqSmplHwnd,(HMENU)PB_QUIT,hInstance,NULL)))
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
                    PostQuitMessage(0);
                    break;
				case PB_SNAP:
                    // Snap button has been hitten
					OnSnap();
					break;
			}
			break;
        case WM_DESTROY:
			PostQuitMessage(0);

		default:
			return DefWindowProc(hWnd, iMessage, wParam, lParam);   
			break;
	}
	return 0;
}


// Function executed when the snap button is clicked
int OnSnap (void)
{
	int 	        error, acqWinWidth, acqWinHeight;
    unsigned int	bufSize, bytesPerPixel;    
	unsigned int	bitsPerPixel, plotFlag;
	char	        intfName[64];     
	
	
	// Get the interface name
	GetWindowText(HIntfName, intfName, 64);

	// Open an interface and a session
	errChk(imgInterfaceOpen (intfName, &Iid));
	errChk(imgSessionOpen (Iid, &Sid));
	
	// Let's check that the Acquisition window is not smaller than the Canvas
	errChk(imgGetAttribute (Sid, IMG_ATTR_ROI_WIDTH, &acqWinWidth));
	errChk(imgGetAttribute (Sid, IMG_ATTR_ROI_HEIGHT, &acqWinHeight));
	
	if(CanvasWidth < acqWinWidth)
		acqWinWidth = CanvasWidth;
	if(CanvasHeight < acqWinHeight)
		acqWinHeight = CanvasHeight;
		
	// Set the ROI to the size of the Canvas so that it will fit nicely
	errChk(imgSetAttribute2 (Sid, IMG_ATTR_ROI_WIDTH, acqWinWidth));
	errChk(imgSetAttribute2 (Sid, IMG_ATTR_ROI_HEIGHT, acqWinHeight));
	errChk(imgSetAttribute2 (Sid, IMG_ATTR_ROWPIXELS, acqWinWidth)); 
	
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
	
	// create a buffer list with one element
    errChk(imgCreateBufList(1, &Bid));
	
	// compute the size of the required buffer
	errChk(imgGetAttribute (Sid, IMG_ATTR_BYTESPERPIXEL, &bytesPerPixel));
	bufSize = acqWinWidth * acqWinHeight * bytesPerPixel;

	// create a buffer and configure the buffer list
	errChk(imgCreateBuffer(Sid, FALSE, bufSize, &ImaqBuffer));

	/* the following configuration assigns the following to buffer list 
	   element 0:

		    1) buffer pointer that will contain image
		    2) size of the buffer for buffer element 0
		    3) command to stop acquisition when this element is reached
    
	 */
	errChk(imgSetBufferElement2(Bid, 0, IMG_BUFF_ADDRESS, ImaqBuffer));
	errChk(imgSetBufferElement2(Bid, 0, IMG_BUFF_SIZE, bufSize));
	errChk(imgSetBufferElement2(Bid, 0, IMG_BUFF_COMMAND, IMG_CMD_STOP));

	// lock down the buffers contained in the buffer list
	errChk(imgMemLock(Bid));

	// configure the session to use this buffer list
	errChk(imgSessionConfigure(Sid, Bid));

	// start the acquisition, synchronous
	errChk(imgSessionAcquire(Sid, FALSE, NULL));
	
	// Display using NI-IMAQ
	// Note that if you are using a 1424 and a camera with a bitdepth greater
	// that 8 bits, you need to set the flag parameter of imgPlot to match
	// the bit depth of the camera. See the "snap imgPlot" sample. 
	errChk(imgPlot2 (ImaqSmplHwnd, ImaqBuffer, 0, 0, acqWinWidth, acqWinHeight,
					   CanvasLeft, CanvasTop, plotFlag));
					   

Error :
    if(error<0)
        DisplayIMAQError(error);

    // unlock the buffers in the buffer list
	if (Bid != 0)
		imgMemUnlock(Bid);

	// dispose of the buffer
	if (ImaqBuffer != NULL)
		imgDisposeBuffer(ImaqBuffer);

	// close this buffer list
	if (Bid != 0)
		imgDisposeBufList(Bid, FALSE);
	
	// Close the interface and the session
    if(Sid != 0)
	    imgClose (Sid, TRUE);

    if(Iid != 0)
	    imgClose (Iid, TRUE);

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


