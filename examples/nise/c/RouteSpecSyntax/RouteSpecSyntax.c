#include <stdlib.h>
#include "nise.h"
#include "niseErrHandler.h"

#if defined _CVI_
 #include "RouteSpecSyntax.h"
#elif defined _MSC_VER
 #include <stdio.h>
 #include <conio.h>
 #include <ctype.h>
#else
 #error "Unsupported compiler. Please adjust the console i/o to run this example"
#endif

int ShowUI(NISEConstString deviceName);
void GetStyleFromUI(int handle, NISEInt32* style);
void CloseUI(int handle);

NISEConstString  routeSpecificationStyles[] = 
        {
            "Single Route Group",
            "Multiple Routes By Name",
            "Combining Routes and Route Groups",
            "Connect and Disconnect Disorder",
            "Full Route Specification",
            "Combining Styles",
            "Exit example"
        };

int main()
{
    NISESession session = 0;
    NISEStatus  status = NISE_ERROR_NONE;
    NISEBuffer  *deviceName = "SwitchExecutiveExample";
    NISEInt32   style = 0;

    int handle = ShowUI(deviceName);
    //  Open a sesssion to the virtual device
    niseCheckErr(niSE_OpenSession(deviceName, "", &session));

    while (style != 6)
    {
        GetStyleFromUI(handle, &style);
        switch (style)
        {
        case 0: //  Single Route Group
            //  Using a route group name to control a set of switches.  
            //  Route groups are useful for grouping large sets of switching 
            //  together to accomodate all switching necessary for a given test.
            
            niseCheckErr(niSE_Connect(session, "FreqResponseTest", 
                                      NISE_VAL_MULTICONNECT_ROUTES, NISE_TRUE));
            
            niseCheckErr(niSE_Disconnect(session, "FreqResponseTest"));
            
            ReportStatus("Single Route Group", 
                         "Connected and Disconnected \"FreqResponseTest\"");
            break;
        case 1: //  Multiple Routes by Name
            //  Using multiple routes in a single connection call.  
            //  Use ampersand ("&") to combine multiple routes 
            //  in a single route specification string.

            niseCheckErr(niSE_Connect(session, "ArbToInput & ScopeToOutput", 
                                      NISE_VAL_MULTICONNECT_ROUTES, NISE_TRUE));
            
            niseCheckErr(niSE_Disconnect(session, "ArbToInput & ScopeToOutput"));
            
            ReportStatus("Multiple Routes by Name", 
                         "Connected and Disconnected\n  \"ArbToInput & ScopeToOutput\"");
            break;
        case 2: //  Combining Routes and Route Groups
            //  Using both routes and route groups in a single connection call.
            //  Use ampersand ("&") to combine multiple routes in a single 
            //  route specification string.

            niseCheckErr(niSE_Connect(session, "ArbToInput & PowerUUT", 
                                      NISE_VAL_MULTICONNECT_ROUTES, NISE_TRUE));

            niseCheckErr(niSE_Disconnect(session, "ArbToInput & PowerUUT"));

            ReportStatus("Combining Routes and Route Groups", 
                         "Connected and Disconnected \"ArbToInput & PowerUUT\"");
            break;
        case 3: //  Connect and Disconnect disorder
            //  Connects and disconnects do not have to be in the same order 
            //  or grouping as they were originally called.  In fact, 
            //  even route groups may be split up into multiple disconnections.
            niseCheckErr(niSE_Connect(session, "ArbToInput", 
                                      NISE_VAL_MULTICONNECT_ROUTES, NISE_TRUE));

            niseCheckErr(niSE_Connect(session, "ScopeToOutput & PowerUUT", 
                                      NISE_VAL_MULTICONNECT_ROUTES, NISE_TRUE));

            niseCheckErr(niSE_Disconnect(session, 
                                         "ArbToInput & ScopeToOutput & DMMToUUT_Vcc_Leg1"));

            niseCheckErr(niSE_Disconnect(session, "DCPowerToUUT_Vcc_Leg1"));
            ReportStatus("Connect and Disconnect Disorder", 
                         "Connected:\n"
                         "  \"ArbToInput\"\n"
                         "  \"ScopeToOutput & PowerUUT\"\n"
                         "Disconnected:\n"
                         "  \"ArbToInput & ScopeToOutput & DMMToUUT_Vcc_Leg1\"");
            break;
        case 4: //  Full Route Specification
            //  Routes may be specified without using preconfigured routes or 
            //  route groups.  When using full path specification, the path 
            //  should be enlosed in square brackets ("[", "]").  Each path 
            //  leg is divided by a "->" delimiter.  Each leg must contain either:
            //      a) a channel alias name
            //      b) a unique channel as denoted by "device/channel"
            niseCheckErr(niSE_Connect(session, "[Scope->SampleMatrix1/r1->SampleMatrix2/r1->UUT_DAC_OUT]", 
                                      NISE_VAL_MULTICONNECT_ROUTES, NISE_TRUE));

            niseCheckErr(niSE_Disconnect(session, "[Scope->SampleMatrix1/r1->SampleMatrix2/r1->UUT_DAC_OUT]"));
            
            ReportStatus("Full Route Specification", 
                          "Connected and Disconnected \"[Scope->SampleMatrix1/r1->SampleMatrix2/r1->UUT_DAC_OUT]\"");
        
            //  Other ways to specify this route:
            //      "[SampleMatrix1/c1->SampleMatrix1/r1->SampleMatrix2/r1->SampleMatrix2/c2]"
            //      "ScopeToOutput"
            //      "[Scope->SampleMatrix1/r1->SampleMatrix2/r1->SampleMatrix2/c2]"
            //      "[SampleMatrix1/c1->SampleMatrix1/r1->SampleMatrix2/r1->UUT_DAC_OUT]"
            break;
        case 5: //  Combining Styles
            //  Different combinations of specification styles can be 
            //  used within a single route string.
            niseCheckErr(niSE_Connect(session, 
                                      "[Scope->SampleMatrix1/r1->SampleMatrix2/r1->UUT_DAC_OUT] & ArbToInput & PowerUUT", 
                                      NISE_VAL_MULTICONNECT_ROUTES, NISE_TRUE));

            niseCheckErr(niSE_Disconnect(session, 
                                         "[Scope->SampleMatrix1/r1->SampleMatrix2/r1->UUT_DAC_OUT] & ArbToInput & PowerUUT"));
            ReportStatus("Combining Styles", 
                         "Connected and Disconnected "
                         "\"[Scope->SampleMatrix1/r1->SampleMatrix2/r1->UUT_DAC_OUT] & ArbToInput & PowerUUT\"");
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
    return 0;
}













//  Implementation of the National Instruments LabWindows/CVI User Interface interaction
#if defined _CVI_
int ShowUI(NISEConstString deviceName)
{
    int panelHandle = LoadPanel (0, "RouteSpecSyntax.uir", PANEL);
    DisplayPanel (panelHandle);
    SetCtrlVal (panelHandle, PANEL_DEVICE, deviceName);
    SetCtrlAttribute (panelHandle, PANEL_STYLE, ATTR_VISIBLE, 0);
    SetPanelAttribute(panelHandle, ATTR_HEIGHT, 91);
    ProcessDrawEvents();
    return panelHandle;
}

void GetStyleFromUI(int handle, NISEInt32* style)
{
    int i;
    SetPanelAttribute(handle, ATTR_HEIGHT, 285);
    SetCtrlAttribute (handle, PANEL_TEXTMSG, ATTR_VISIBLE, 0);
    SetCtrlAttribute(handle, PANEL_STYLE, ATTR_CALLBACK_DATA, style);
    ClearListCtrl (handle, PANEL_STYLE);
    for (i=0; i<sizeof(routeSpecificationStyles)/sizeof(NISEConstString)-1; i++)
    {
        InsertListItem (handle, PANEL_STYLE, i, routeSpecificationStyles[i], i);
    }
    
    SetCtrlAttribute (handle, PANEL_STYLE, ATTR_VISIBLE, 1);
    RunUserInterface();
}

void CloseUI(int handle)
{
    DiscardPanel(handle);
}

int CVICALLBACK cbConnect (int panel, int control, int event,
        void *callbackData, int eventData1, int eventData2)
{
    if (event == EVENT_COMMIT)
    {
        NISEInt32* style;
        GetCtrlAttribute (panel, PANEL_STYLE, ATTR_CALLBACK_DATA, &style);
        GetCtrlVal (panel, PANEL_STYLE, style);
        QuitUserInterface(0);
    }
    return 0;
}
int CVICALLBACK cbQuit (int panel, int control, int event,
        void *callbackData, int eventData1, int eventData2)
{
    if (event == EVENT_COMMIT)
    {
        NISEInt32* style;
        GetCtrlAttribute (panel, PANEL_STYLE, ATTR_CALLBACK_DATA, &style);
        *style = (sizeof(routeSpecificationStyles)/sizeof(NISEConstString)-1);
        QuitUserInterface(0);
    }
    return 0;
}


//  Implementation of the Microsoft Visual C++ User Interface interaction
#elif defined _MSC_VER

int ShowUI(NISEConstString deviceName)
{
    system("cls");
    printf ("\nniSE Virtual Device Name: %s\nOpening session...\n\n", deviceName);
    return 0;
}
void GetStyleFromUI(int handle, NISEInt32* style)
{
    NISEInt32 i;
    system("cls");
    printf ("\nChoose a route specification style from the list:\n");
    for (i=0; i<sizeof(routeSpecificationStyles)/sizeof(NISEConstString); i++)
    {
        printf ("\t%d)\t%s\n", i, routeSpecificationStyles[i]);
    }
    do
    {
        *style = getch() - '0';
    }while (*style<0 || *style>=sizeof(routeSpecificationStyles)/sizeof(NISEConstString));
}

void CloseUI(int handle)
{
}

#endif
