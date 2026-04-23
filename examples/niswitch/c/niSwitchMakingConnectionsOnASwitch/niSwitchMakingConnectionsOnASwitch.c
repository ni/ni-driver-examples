/*

 National Instruments - NI-SWITCH - CVI Examples

 Making Connections on a Switch

 This example demonstrates how to connect channels on a switch module.

 Refer to the NI Switches Help for the valid channel names for your switch module.

 NI-SWITCH functions utilized:

 niSwitch_InitWithTopology
 niSwitch_Connect
 niSwitch_WaitForDebounce
 niSwitch_close

*/


#include "niswitch.h"
#include "niSwitchErrorHandler.h"

int main (void)
{
   ViSession switchSession = VI_NULL;
   ViRsrc resourceName = "Dev1" ;
   ViConstString channel1 = "r0";
   ViConstString channel2 = "c0";
   ViConstString channel3 = "r1";
   ViConstString channel4 = "c1";
   ViStatus switchError = VI_SUCCESS;
   ViConstString topology  = NISWITCH_TOPOLOGY_CONFIGURED_TOPOLOGY;

   //Open a session to the switch module and set the topology.
   niSwitchCheckErr(niSwitch_InitWithTopology(
      resourceName,
      topology,
      VI_FALSE,
      VI_TRUE,
      &switchSession));

   //Connect channel1 and channel2.
   niSwitchCheckErr(niSwitch_Connect(switchSession, channel1, channel2));

   //Wait for any relays to activate and debounce.
   niSwitchCheckErr(niSwitch_WaitForDebounce(switchSession,5000));

   //Connect channel3 and channel4
   niSwitchCheckErr(niSwitch_Connect(switchSession, channel3, channel4));

   //Wait for any relays to activate and debounce.
   niSwitchCheckErr(niSwitch_WaitForDebounce(switchSession,5000));


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
