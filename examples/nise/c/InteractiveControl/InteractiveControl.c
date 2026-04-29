#include <stdlib.h>
#include <stdio.h>
#include "nise.h"
#include "niseErrHandler.h"

#if defined _CVI_
 #include "InteractiveControl.h"
#elif defined _MSC_VER
 #include <string.h>
 #include <conio.h>
 #include <ctype.h>
#else
 #error "Unsupported compiler. Please adjust the console i/o to run this example"
#endif

int ShowUI(NISEBuffer* deviceName);
void GetActionParamsFromUI(int handle, NISEConstString deviceName, NISEInt32* action, 
                           NISEBuffer* ch1, NISEBuffer* ch2, NISEBuffer* route, 
                           NISEInt32* multiconnectMode);
void CloseUI(int handle);

NISEConstString  interactiveControlOptions[] = 
        {
            "Find Route Between Channels",
            "Connect Route",
            "Disconnect Route",
            "Exit example"
        };

NISEConstString  findRouteResults[] = 
        {
            "0 - invalid",
            "1 - Path Available",
            "2 - Path Exists",
            "3 - Path Unsupported",
            "4 - Resource in Use",
            "5 - Source Conflict",
            "6 - Channel not Available",
            "7 - Channels are Hardwired"
        };

int main()
{
    NISESession session = 0;
    NISEStatus  status = NISE_ERROR_NONE;
    NISEBuffer  deviceName[256] = "SwitchExecutiveExample";
    NISEInt32   action = 0;
    NISEBuffer  ch1[256]="Scope";
    NISEBuffer  ch2[256]="UUT_DAC_OUT";
    NISEBuffer  *route = NULL;
    NISEInt32   routeSize = 256;
    NISEInt32   routeCapability;
    NISEInt32   multiconnectMode = NISE_VAL_USE_DEFAULT_MODE;
    int handle;

    route = malloc(routeSize);
    *route = '\0';

    handle = ShowUI(deviceName);
    //  Open a sesssion to the virtual device
    niseCheckErr(niSE_OpenSession(deviceName, "", &session));

    while (action != 3)
    {
        GetActionParamsFromUI(handle, deviceName, &action, ch1, ch2, route, &multiconnectMode);

        switch (action)
        {
        case 0: //  Find Route between channels
            //  see if a route is available between the two channels
            niseCheckErr(niSE_FindRoute(session, ch1, ch2, route, &routeSize, &routeCapability));
            if (routeCapability == NISE_VAL_PATH_AVAILABLE)
            {
                //  If a route is available, dynamically size a string to 
                //  hold the route and then retrieve it
                route = realloc (route, routeSize);
                niseCheckErr(niSE_FindRoute(session, ch1, ch2, route, 
                                            &routeSize, &routeCapability));
                ReportStatus("Found Route", route);
            }
            else
            {
                ReportStatus("Find Route Result", findRouteResults[routeCapability]);
            }
            break;
        case 1: //  Connect
            //  connect the route specified in "route"
            niseCheckErr(niSE_Connect(session, route, multiconnectMode, NISE_TRUE));
            ReportStatus("Connected Route", route);
            break;
        case 2: //  Disconnect
            //  disconnect the route specified in "route"
            niseCheckErr(niSE_Disconnect(session, route));
            ReportStatus("Disconnected Route", route);
            break;
        }
    }
Error:
    CloseUI(handle);
    //  Error handling:
    if (status<NISE_ERROR_NONE)
    {
        HandleError(session, status);
    }
    if (session != 0)
    {
        niSE_CloseSession(session);
    }
    free(route);
    return 0;
}










//  Implementation of the National Instruments LabWindows/CVI User Interface interaction
#if defined _CVI_

int ShowUI(NISEBuffer* deviceName)
{
    int panelHandle = LoadPanel (0, "InteractiveControl.uir", PANEL);
    SetCtrlAttribute (panelHandle, PANEL_DEVICE, ATTR_CALLBACK_DATA, deviceName);
    SetCtrlVal (panelHandle, PANEL_DEVICE, deviceName);
    DisplayPanel (panelHandle);
    SetCtrlAttribute(panelHandle, PANEL_DECORATION_1, ATTR_VISIBLE, 0);
    SetCtrlAttribute(panelHandle, PANEL_CHANNEL2, ATTR_VISIBLE, 0);
    SetCtrlAttribute(panelHandle, PANEL_CHANNEL1, ATTR_VISIBLE, 0);
    SetCtrlAttribute(panelHandle, PANEL_FIND, ATTR_VISIBLE, 0);
    SetCtrlAttribute(panelHandle, PANEL_DECORATION_2, ATTR_VISIBLE, 0);
    SetCtrlAttribute(panelHandle, PANEL_ROUTE, ATTR_VISIBLE, 0);
    SetCtrlAttribute(panelHandle, PANEL_MULTICONNECT, ATTR_VISIBLE, 0);
    SetCtrlAttribute(panelHandle, PANEL_DISCONNECT, ATTR_VISIBLE, 0);
    SetCtrlAttribute(panelHandle, PANEL_CONNECT, ATTR_VISIBLE, 0);
    SetCtrlAttribute(panelHandle, PANEL_OK, ATTR_VISIBLE, 0);
   SetActiveCtrl (panelHandle, PANEL_OK_1);
    RunUserInterface();
    SetCtrlAttribute(panelHandle, PANEL_DEVICE, ATTR_CTRL_MODE, VAL_INDICATOR);
    SetCtrlAttribute(panelHandle, PANEL_OK_1, ATTR_VISIBLE, 0);
    SetCtrlAttribute(panelHandle, PANEL_DECORATION_1, ATTR_VISIBLE, 1);
    SetCtrlAttribute(panelHandle, PANEL_CHANNEL2, ATTR_VISIBLE, 1);
    SetCtrlAttribute(panelHandle, PANEL_CHANNEL1, ATTR_VISIBLE, 1);
    SetCtrlAttribute(panelHandle, PANEL_FIND, ATTR_VISIBLE, 1);
    SetCtrlAttribute(panelHandle, PANEL_DECORATION_2, ATTR_VISIBLE, 1);
    SetCtrlAttribute(panelHandle, PANEL_ROUTE, ATTR_VISIBLE, 1);
    SetCtrlAttribute(panelHandle, PANEL_MULTICONNECT, ATTR_VISIBLE, 1);
    SetCtrlAttribute(panelHandle, PANEL_DISCONNECT, ATTR_VISIBLE, 1);
    SetCtrlAttribute(panelHandle, PANEL_CONNECT, ATTR_VISIBLE, 1);
    SetCtrlAttribute(panelHandle, PANEL_OK, ATTR_VISIBLE, 1);
   SetActiveCtrl (panelHandle, PANEL_FIND);
    SetPanelAttribute(panelHandle, ATTR_HEIGHT, 314);
    return panelHandle;
}

void GetActionParamsFromUI(int handle, NISEConstString deviceName, NISEInt32* action, NISEBuffer* ch1, NISEBuffer* ch2, NISEBuffer* route, NISEInt32* multiconnectMode)
{
    SetCtrlVal(handle, PANEL_CHANNEL1, ch1);
    SetCtrlVal(handle, PANEL_CHANNEL2, ch2);
    SetCtrlVal(handle, PANEL_MULTICONNECT, *multiconnectMode);
    SetCtrlVal(handle, PANEL_ROUTE, route);
    SetCtrlAttribute(handle, PANEL_ROUTE, ATTR_CALLBACK_DATA, route);
    SetCtrlAttribute(handle, PANEL_CHANNEL2, ATTR_CALLBACK_DATA, ch2);
    SetCtrlAttribute(handle, PANEL_MULTICONNECT, ATTR_CALLBACK_DATA, multiconnectMode);
    SetCtrlAttribute(handle, PANEL_CHANNEL1, ATTR_CALLBACK_DATA, ch1);
    SetPanelAttribute(handle, ATTR_CALLBACK_DATA, action);
    RunUserInterface();
}

void CloseUI(int handle)
{
    DiscardPanel(handle);
}

int CVICALLBACK cbCandD (int panel, int control, int event,
        void *callbackData, int eventData1, int eventData2)
{
    if (event == EVENT_COMMIT)
    {
        NISEInt32* action;
        GetPanelAttribute(panel, ATTR_CALLBACK_DATA, &action);
        if (control == PANEL_CONNECT || control == PANEL_DISCONNECT)
        {
            NISEBuffer *route;
            NISEInt32 *multiconnectMode;
            GetCtrlAttribute (panel, PANEL_ROUTE, ATTR_CALLBACK_DATA, &route);
            GetCtrlVal (panel, PANEL_ROUTE, route);
            GetCtrlAttribute (panel, PANEL_MULTICONNECT, ATTR_CALLBACK_DATA, &multiconnectMode);
            GetCtrlVal (panel, PANEL_MULTICONNECT, multiconnectMode);
            *action = (control == PANEL_CONNECT)?1:2;
        }
        else if (control == PANEL_FIND)
        {
            NISEBuffer *channel1, *channel2;
            GetCtrlAttribute (panel, PANEL_CHANNEL1, ATTR_CALLBACK_DATA, &channel1);
            GetCtrlVal (panel, PANEL_CHANNEL1, channel1);
            GetCtrlAttribute (panel, PANEL_CHANNEL2, ATTR_CALLBACK_DATA, &channel2);
            GetCtrlVal (panel, PANEL_CHANNEL2, channel2);
            *action = 0;
        }
        QuitUserInterface(0);
    }
    return 0;
}

int CVICALLBACK cbOk (int panel, int control, int event,
        void *callbackData, int eventData1, int eventData2)
{
    if (event == EVENT_COMMIT)
    {
        NISEBuffer *deviceName;
        GetCtrlAttribute (panel, PANEL_DEVICE, ATTR_CALLBACK_DATA, &deviceName);
        GetCtrlVal (panel, PANEL_DEVICE, deviceName);
        QuitUserInterface(0);
    }
    return 0;
}

int CVICALLBACK cbQuit (int panel, int control, int event,
        void *callbackData, int eventData1, int eventData2)
{
    if (event == EVENT_COMMIT)
    {
        NISEInt32* action;
        GetPanelAttribute(panel, ATTR_CALLBACK_DATA, &action);
        *action = sizeof(interactiveControlOptions)/sizeof(NISEConstString) - 1;
        QuitUserInterface(0);
    }
    return 0;
}

//  Implementation of the Microsoft Visual C++ User Interface interaction
#elif defined _MSC_VER

int ShowUI(NISEBuffer* deviceName)
{
    printf("\nEnter niSE Virtual Device Name (%s):", deviceName);
    myScanf(deviceName);
    return 0;
}

void CloseUI(int handle)
{
}

void GetActionParamsFromUI(int handle, NISEConstString deviceName, NISEInt32* action, NISEBuffer* ch1, NISEBuffer* ch2, NISEBuffer* route, NISEInt32* multiconnectMode)
{
    NISEInt32 i;
    char c;

    system("cls");
    printf ("\nniSE Device:%s\n", deviceName);
    printf ("\nChoose an action to perform from the list:\n");
    for (i=0; i<sizeof(interactiveControlOptions)/sizeof(NISEConstString); i++)
    {
        printf ("\t%d)\t%s\n", i, interactiveControlOptions[i]);
    }
    do
    {
        *action = getch() - '0';
    }while (*action<0 || *action>=sizeof(interactiveControlOptions)/sizeof(NISEConstString));
    if (*action != (sizeof(interactiveControlOptions)/sizeof(NISEConstString) - 1))
    {
        char modes[4] = "dny";
        system("cls");
        printf ("\nniSE Device:%s\n", deviceName);
        switch (*action)
        {
        case 0:
            printf("\nFind Route between channels:\n\tChannel 1(%s):", ch1);
            myScanf(ch1);
            printf("\n\tChannel 2(%s):", ch2);
            myScanf(ch2);
            break;
        case 1:
            printf("\nMulticonnect Routes y/n/d(use default) (%c)?", modes[*multiconnectMode+1]);
            do
            {
                c = tolower(getch());
            }while (c != 'y' && c != 'n' && c != 'd' && c != '\r');
            switch (c)
            {
            case 'd':
                *multiconnectMode = NISE_VAL_USE_DEFAULT_MODE;
                break;
            case 'y':
                *multiconnectMode = NISE_VAL_MULTICONNECT_ROUTES;
                break;
            case 'n':
                *multiconnectMode = NISE_VAL_NO_MULTICONNECT;
                break;
            }
        case 2:
            printf("\nEnter Route (%s):", route);
            myScanf(route);
            break;
        }
    }
}

#endif
