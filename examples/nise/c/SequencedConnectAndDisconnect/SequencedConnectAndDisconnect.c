#include <stdlib.h>
#include <stdio.h>
#include "nise.h"
#include "niseErrHandler.h"

#if defined _CVI_
 #include "SequencedConnectAndDisconnect.h"
#elif defined _MSC_VER
#else
 #error "Unsupported compiler. Please adjust the console i/o to run this example"
#endif

int ShowUI(NISEConstString deviceName);
void UIReport(int handle, NISEConstString message);
void CloseUI(int handle);


int main()
{
    NISESession session = 0;
    NISEStatus  status = NISE_ERROR_NONE;
    NISEConstString  deviceName = "SwitchExecutiveExample";
    NISEInt32   i;
    NISEConstString  routes[] = 
            {
                "",
                "DCPowerToUUT_Vcc_Leg1",
                "ArbToInput & ScopeToOutput",
                "[Arb->SampleMatrix1/r0->SampleMatrix2/r0->UUT_ADC_IN]",
                "FreqResponseTest",
                ""
            };
    int handle;
    
    handle = ShowUI(deviceName);

    UIReport (handle, "Opening a session...\n\n");
    //  Open a sesssion to the virtual device
    niseCheckErr(niSE_OpenSession(deviceName, "", &session));

    //  Walk through each array element, connecting the current 
    //  index and disconnecting the previous one
    for (i=1; i<sizeof(routes)/sizeof(NISEConstString); i++)
    {
        niseCheckErr(niSE_ConnectAndDisconnect(session,                     //  device session
                                               routes[i],                   //  route to connect
                                               routes[i - 1],               //  route to disconnect
                                               NISE_VAL_USE_DEFAULT_MODE,   //  multiconnect mode
                                               NISE_VAL_BREAK_BEFORE_MAKE,  //  operation order
                                               NISE_TRUE));                 //  wait for debounce
        //  This would be a logical place to perform measurement operations
        {
            NISEBuffer  message[256];
            sprintf(message, "Step %d\n    Connected    :%s\n"
                             "    Disconnected :%s\n", i, routes[i], routes[i-1]);
            UIReport (handle, message);
        }
    }
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
    CloseUI(handle);
    return 0;
}

#if defined _CVI_

int ShowUI(NISEConstString deviceName)
{
    int handle = LoadPanel (0, "SequencedConnectAndDisconnect.uir", PANEL);
    SetCtrlVal (handle, PANEL_DEVICE, deviceName);
    DisplayPanel(handle);
    return handle;
}

void UIReport(int handle, NISEConstString message)
{
    SetCtrlVal (handle, PANEL_C_AND_D, message);
}

void CloseUI(int handle)
{
    SetInputMode (handle, PANEL_OK, 1);
    RunUserInterface();
    DiscardPanel(handle);
}

int CVICALLBACK cbQuit (int panel, int control, int event,
        void *callbackData, int eventData1, int eventData2)
{
    if (event == EVENT_COMMIT)
    {
        QuitUserInterface(0);
    }
    return 0;
}

#elif defined _MSC_VER
int ShowUI(NISEConstString deviceName)
{
    system("cls");
    printf ("\nniSE Virtual Device Name: %s\n\n", deviceName);
    return 0;
}

void UIReport(int handle, NISEConstString message)
{
    printf("%s", message);
}

void CloseUI(int handle)
{
}
#endif
