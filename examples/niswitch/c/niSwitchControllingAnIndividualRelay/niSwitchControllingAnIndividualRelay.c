/*

 National Instruments - NI-SWITCH - CVI Examples

 Controlling an Individual Relay

 This example demonstrates how to control an individual relay on a switch module.

 Refer to the NI Switches Help for the valid relay names for your switch module.

 NI-SWITCH functions utilized:

 niSwitch_InitWithTopology
 niSwitch_RelayControl
 niSwitch_WaitForDebounce
 niSwitch_close

*/


#include "niswitch.h"
#include "niSwitchErrorHandler.h"

int main (void)
{
   ViSession switchSession = VI_NULL;
   ViStatus switchError = VI_SUCCESS;
   ViRsrc resourceName = "Dev1";
   ViConstString relayName = "b1r0c0";
   ViInt32 relayAction = NISWITCH_VAL_CLOSE_RELAY; //NISWITCH_VAL_OPEN_RELAY to open relay.
   ViConstString topology = NISWITCH_TOPOLOGY_CONFIGURED_TOPOLOGY;

   //Open a session to the switch module and set the topology.
   niSwitchCheckErr(niSwitch_InitWithTopology(
      resourceName,
      topology,
      VI_FALSE,
      VI_TRUE,
      &switchSession));

   //Open or Close the relay.
   niSwitchCheckErr(niSwitch_RelayControl(
      switchSession,
      relayName,
      relayAction));

   //Wait for the relay to activate and debounce.
   niSwitchCheckErr(niSwitch_WaitForDebounce(switchSession, 5000));

Error:

    if (switchError < VI_SUCCESS)
    {
       //Display errors (if any).
       niSwitch_ErrorHandler(switchSession, switchError);
    }
    if (switchSession != VI_NULL)
    {
       //Close the session to switch module.
       niSwitch_close(switchSession);
    }

    return 0;
}

