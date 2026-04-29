#include <stdlib.h>
#include <stdio.h>
#include "nise.h"
#include "niseErrHandler.h"

#if defined _CVI_
 #include "UsingFindRoute.h"
#elif defined _MSC_VER
 #include <conio.h>
 #include <ctype.h>
#else
 #error "Unsupported compiler. Please adjust the console i/o to run this example"
#endif

void GetNamesFromUI(NISEBuffer* deviceName, NISEBuffer* channel1, NISEBuffer* channel2);

#define PREALLOCATED_ROUTE_SIZE 1024

int main()
{
    NISESession session = 0;
    NISEStatus  status = NISE_ERROR_NONE;
    NISEBuffer  deviceName[256] = "SwitchExecutiveExample";
    NISEBuffer  channel1[256] = "DCPower";
    NISEBuffer  channel2[256] = "UUT_Vcc";
    NISEBuffer  *route = NULL;
    NISEInt32   routeSize = PREALLOCATED_ROUTE_SIZE; //    initial buffer size for the route
    NISEInt32   routeCapability;

    GetNamesFromUI(deviceName, channel1, channel2);

    //  Open a sesssion to the virtual device
    niseCheckErr(niSE_OpenSession(deviceName, "", &session));

    //  Find route function returns the route string in the 'route' parameter
    //  First, assume the route string length of 1024 characters.
    //  FindRoute function attempts to find the route between requested end points,
    //  and it returns the result in the "route" buffer if it is big enough.
    //  If the buffer is not big enough, reallocate the buffer and call the FindRoute 
    //  function again.  A typical, although not the fastest usage would be to call
    //  this function once with zero size and null buffer, in order to discover
    //  the required size.
    //  Assuming some reasonable size for the "route" buffer up front can increase
    //  the performance of the code if the assumed value is bigger than the actual
    //  route string size. This way we do not have to call the searching algorithm 
    //  twice. This example uses this preallocation method.

    route = (NISEBuffer*)malloc(routeSize);
    niseCheckErr(niSE_FindRoute(session, channel1, channel2, route, &routeSize, &routeCapability));
    
    //  if the return string was too large, reallocate and try again
    if (routeSize > PREALLOCATED_ROUTE_SIZE)
    {
        route = (NISEBuffer*)realloc(route, routeSize);
        niseCheckErr(niSE_FindRoute(session, channel1, channel2, route, &routeSize, &routeCapability));
    }
    
    //  Connect a route
    if (routeCapability == NISE_VAL_PATH_AVAILABLE)
    {
        static char message[2*PREALLOCATED_ROUTE_SIZE];
        niseCheckErr(niSE_Connect(session, route, NISE_VAL_NO_MULTICONNECT, NISE_TRUE));
        sprintf(message, "FindRoute returned \"%s\"\nConnect and Disconnect succeeded!", route);
        ReportStatus("Success", message);
    }
    else
    {
        ReportStatus("FindRoute Example", "Unable to connect those two channels at this time.");
    }
    
    //  This would be a logical place to perform measurement operations
    
    //  Disconnect the route group
    niseCheckErr(niSE_Disconnect(session, route));
Error:
    //  Error handling:
    if (status<NISE_ERROR_NONE)
    {
        HandleError(session, status);
    }

    if (session != 0)
    {
        niSE_CloseSession(session);
    }
    return 0;
}








//  Implementation of the National Instruments LabWindows/CVI User Interface interaction
#if defined _CVI_

void GetNamesFromUI(NISEBuffer* deviceName, NISEBuffer* channel1, NISEBuffer* channel2)
{
    int panelHandle = LoadPanel (0, "UsingFindRoute.uir", PANEL);
    
    SetCtrlVal (panelHandle, PANEL_DEVICE, deviceName);
    SetCtrlVal (panelHandle, PANEL_CHANNEL1, channel1);
    SetCtrlVal (panelHandle, PANEL_CHANNEL2, channel2);
    DisplayPanel (panelHandle);
    SetCtrlAttribute (panelHandle, PANEL_DEVICE, ATTR_CALLBACK_DATA, deviceName);
    SetCtrlAttribute (panelHandle, PANEL_CHANNEL1, ATTR_CALLBACK_DATA, channel1);
    SetCtrlAttribute (panelHandle, PANEL_CHANNEL2, ATTR_CALLBACK_DATA, channel2);
    RunUserInterface();
    DiscardPanel (panelHandle);
}

int CVICALLBACK cbConnect (int panel, int control, int event,
        void *callbackData, int eventData1, int eventData2)
{
    if (event == EVENT_COMMIT)
    {
        NISEBuffer *deviceName, *channel1, *channel2;
        GetCtrlAttribute (panel, PANEL_DEVICE, ATTR_CALLBACK_DATA, &deviceName);
        GetCtrlVal (panel, PANEL_DEVICE, deviceName);
        GetCtrlAttribute (panel, PANEL_CHANNEL1, ATTR_CALLBACK_DATA, &channel1);
        GetCtrlVal (panel, PANEL_CHANNEL1, channel1);
        GetCtrlAttribute (panel, PANEL_CHANNEL2, ATTR_CALLBACK_DATA, &channel2);
        GetCtrlVal (panel, PANEL_CHANNEL2, channel2);
        QuitUserInterface(0);
    }
    return 0;
}

//  Implementation of the Microsoft Visual C++ User Interface interaction
#elif defined _MSC_VER

void GetNamesFromUI(NISEBuffer* deviceName, NISEBuffer* channel1, NISEBuffer* channel2)
{
    printf ("\nEnter niSE Virtual Device Name (SwitchExecutiveExample):");
    myScanf(deviceName);
    printf ("\nEnter the name of the left-hand terminal (DCPower):");
    myScanf(channel1);
    printf ("\nEnter the name of the right-hand terminal (UUT_Vcc):");
    myScanf(channel2);
}

#endif
