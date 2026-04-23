/*

 National Instruments - NI-SWITCH - CVI Examples

 Software Scanning

 This example demonstrates how to scan a series of channels on a switch using software scanning.

 Refer to the NI Switches Help to determine if your switch supports scanning, the scan list syntax, and the valid channel names and valid resource names for your switch module.

 NI-SWITCH functions utilized:

 niSwitch_InitWithTopology
 niSwitch_ConfigureScanList
 niSwitch_ConfigureScanTrigger
 niSwitch_SetContinuousScan
 niSwitch_InitiateScan
 niSwitch_SendSoftwareTrigger
 niSwitch_AbortScan
 niSwitch_close

*/


#include "niswitch.h"
#include "niSwitchErrorHandler.h"

#ifdef   _CVI_
   #include <utility.h>   //for Delay()
#elif defined _MSC_VER
   #include <windows.h>   // for Sleep()
#elif defined __linux__
   #include <unistd.h>    // for sleep()
#endif

int main (void)
{
   ViSession switchSession = VI_NULL;
   ViRsrc resourceName = "Dev1" ;
   ViConstString topology = NISWITCH_TOPOLOGY_CONFIGURED_TOPOLOGY;
   ViConstString scanList = "ch0:4->com0;";
   ViInt32 numberOfTriggers = 5;
   ViStatus switchError = VI_SUCCESS;
   ViInt32 i;

   //Open a session to the switch module and set the topology.
   niSwitchCheckErr(niSwitch_InitWithTopology(
      resourceName,
      topology,
      VI_FALSE,
      VI_TRUE,
      &switchSession));

   //Configures the switch module for scanning.
   //Refer to NI Switches Help for scan list syntax.
   niSwitchCheckErr(niSwitch_ConfigureScanList(
      switchSession,
      scanList,
      NISWITCH_VAL_BREAK_BEFORE_MAKE));  //scanMode

   //Configures the trigger to be software trigger.
   niSwitchCheckErr(niSwitch_ConfigureScanTrigger(
      switchSession,
      0.0,                        // scanDelay
      NISWITCH_VAL_SOFTWARE_TRIG, //triggerInput
      NISWITCH_VAL_NONE));        //scanAdvancedOutput


   //Configures the switch to loop continuously through the scan list until
   //niSwitch_Abort is called.
   niSwitchCheckErr(niSwitch_SetContinuousScan(
      switchSession,
      VI_TRUE));

   //Initiates the scan usings the configured scan list and triggers.
   niSwitchCheckErr(niSwitch_InitiateScan(switchSession));

   for (i=0; i<numberOfTriggers; i++)
   {

      //wait for 500 ms
      #ifdef _CVI_
         Delay (0.5);
      #elif defined _MSC_VER
         Sleep (500);
      #elif defined __linux__
         sleep (0.5);
      #endif

      //Triggers the switch module through software.
      niSwitchCheckErr(niSwitch_SendSoftwareTrigger(switchSession));
   }

   //Halts the scan.
   niSwitchCheckErr(niSwitch_AbortScan(switchSession));

Error:

   if (switchError < VI_SUCCESS)
   {
      //Display errors (if any).
      niSwitch_ErrorHandler(switchSession, switchError);
   }
   if (switchSession != VI_NULL)
   {
      //Close the session to the switch module.
      niSwitch_close(switchSession);
   }

   return 0;

}

