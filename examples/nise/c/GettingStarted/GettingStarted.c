#include "nise.h"
#include "niseErrHandler.h"

#if defined _CVI_
 #include "GettingStarted.h"
#elif defined _MSC_VER
 #include <stdio.h>
 #include <conio.h>
 #include <ctype.h>
#else
 #error "Unsupported compiler. Please adjust the console i/o to run this example"
#endif

void GetNamesFromUI(NISEBuffer* deviceName, NISEBuffer* routeName);

int main()
{
    NISESession session = 0;
    NISEStatus  status = NISE_ERROR_NONE;
    NISEBuffer  deviceName[256] = "SwitchExecutiveExample";
    NISEBuffer  routeName[256] = "FreqResponseTest";

    GetNamesFromUI(deviceName, routeName);

    //  Open a sesssion to the virtual device
    niseCheckErr(niSE_OpenSession(deviceName, "", &session));
    
    //  Connect a route
    niseCheckErr(niSE_Connect(session, routeName, NISE_VAL_MULTICONNECT_ROUTES, NISE_TRUE));
    
    
    //  This would be a logical place to perform measurement operations
    
    //  Disconnect the route group
    niseCheckErr(niSE_Disconnect(session, routeName));
Error:
    //  Error handling:
    if (status<NISE_ERROR_NONE)
    {
        HandleError(session, status);
    }
    else
    {
        ReportStatus("Success", "Connect and Disconnect succeeded!");
    }

    if (session != 0)
    {
        niSE_CloseSession(session);
    }
    return 0;
}








//  Implementation of the National Instruments LabWindows/CVI User Interface interaction
#if defined _CVI_

void GetNamesFromUI(NISEBuffer* deviceName, NISEBuffer* routeName)
{
    int panelHandle = LoadPanel (0, "GettingStarted.uir", PANEL);
    DisplayPanel (panelHandle);
    SetCtrlAttribute (panelHandle, PANEL_DEVICE, ATTR_CALLBACK_DATA, deviceName);
    SetCtrlAttribute (panelHandle, PANEL_ROUTE, ATTR_CALLBACK_DATA, routeName);
    RunUserInterface();
    DiscardPanel (panelHandle);
}

int CVICALLBACK cbConnect (int panel, int control, int event,
        void *callbackData, int eventData1, int eventData2)
{
    if (event == EVENT_COMMIT)
    {
        NISEBuffer *deviceName, *routeName;
        GetCtrlAttribute (panel, PANEL_DEVICE, ATTR_CALLBACK_DATA, &deviceName);
        GetCtrlVal (panel, PANEL_DEVICE, deviceName);
        GetCtrlAttribute (panel, PANEL_ROUTE, ATTR_CALLBACK_DATA, &routeName);
        GetCtrlVal (panel, PANEL_ROUTE, routeName);
        QuitUserInterface(0);
    }
    return 0;
}


//  Implementation of the Microsoft Visual C++ User Interface interaction
#elif defined _MSC_VER

void GetNamesFromUI(NISEBuffer* deviceName, NISEBuffer* routeName)
{
    printf("\nEnter niSE Virtual Device Name (%s):", deviceName);
    myScanf(deviceName);
    printf("\nEnter the name of a route (%s):", routeName);
    myScanf(routeName);
}

#endif
