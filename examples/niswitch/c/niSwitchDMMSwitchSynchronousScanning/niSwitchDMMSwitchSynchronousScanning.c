/*

 National Instruments - NI-SWITCH - CVI Examples

 Synchronous Scanning

 This example demonstrates how to scan a series of channels on a switch module and take measurements with an NI digital multimeter using synchronous scanning.
 Refer to the NI Switches Help to determine if your switch supports scanning, the scan list syntax, and the valid channel names and valid resource names for your switch module.

 NI-SWITCH functions utilized:

 niSwitch_InitWithTopology
 niSwitch_ConfigureScanList
 niSwitch_ConfigureScanTrigger
 niSwitch_SetContinuousScan
 niSwitch_InitiateScan
 niSwitch_GetAttributeViBoolean
 niSwitch_GetAttributeViReal64
 niSwitch_AbortScan
 niSwitch_close

 NI-DMM functions utilized:

 niDMM_init
 niDMM_ConfigureMeasurement
 niDMM_ConfigureMultiPoint
 niDMM_ConfigureMeasCompleteDest
 niDMM_ConfigureMeasCompleteSlope
 niDMM_Initiate
 niDMM_FetchMultiPoint
 niDMM_close

*/


#include <stdio.h>
#include "niswitch.h"
#include "niSwitchErrorHandler.h"
#include "niDMMErrorHandler.h"
#include "nidmm.h"

#ifdef   _CVI_
   #include <utility.h>   //for Delay()
#elif defined _MSC_VER
   #include <windows.h>   // for Sleep()
#elif defined __linux__
   #include <unistd.h>    // for sleep()
#endif

ViStatus calculateSwitchTimeOut(ViSession switchSession, ViReal64* timeOutInMilliSeconds);


int main (void)
{
   ViSession switchSession = VI_NULL;
   ViReal64 switchTimeOutInMilliSeconds;
   ViRsrc switchResourceName = "Dev1";
   ViConstString scanList = "ch0:9->com0;";
   ViStatus switchError = VI_SUCCESS;

   ViSession dmmSession = VI_NULL;
   ViRsrc dmmResourceName = "Dev2";
   ViInt32 dmmSampleCount = 10;
   ViReal64 dmmSamplesArray[10];
   ViInt32 actualNumberOfPoints;
   ViConstString topology = NISWITCH_TOPOLOGY_CONFIGURED_TOPOLOGY;
   ViBoolean isWaitingForTrigger = VI_FALSE;
   ViStatus dmmError = VI_SUCCESS;
   ViInt32 i;


   //Programming the DMM

   //Open a session to the DMM.
   niDMMCheckErr(niDMM_init(dmmResourceName, VI_TRUE, VI_TRUE, &dmmSession));

   //Configure the function, range, and resolution of the measurement.
   niDMMCheckErr(niDMM_ConfigureMeasurement(
      dmmSession,
      NIDMM_VAL_DC_VOLTS,
      10.0,     //range
      0.0001)); //resolution

   //Configures a multipoint acquisition.
   niDMMCheckErr(niDMM_ConfigureMultiPoint(
      dmmSession,
      1,                  //triggerCount
      dmmSampleCount,
      NIDMM_VAL_INTERVAL, //sampleTrigger
      0.10));             //sampleInterval

   //Configures the destination of the DMM output trigger (Measurement Complete).
   //This should match the switch module input trigger.
   niDMMCheckErr(niDMM_ConfigureMeasCompleteDest(dmmSession, NIDMM_VAL_EXTERNAL));

   //Configures the slope of the DMM output trigger.
   niDMMCheckErr(niDMM_ConfigureMeasCompleteSlope(dmmSession, NIDMM_VAL_NEGATIVE));

   //Programming the Switch

   //Open a session to the switch module and set the topology.
   niSwitchCheckErr(niSwitch_InitWithTopology(
      switchResourceName,
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

   //Configures the input trigger of the switch module.
   //This should match the output trigger of the DMM.
   niSwitchCheckErr(niSwitch_ConfigureScanTrigger(
      switchSession,
      0.0,                        // scanDelay
      NISWITCH_VAL_REARCONNECTOR, //triggerInput
      NISWITCH_VAL_NONE));        //scanAdvancedOutput

   //Configures the switch to loop continuously through the scan list until
   //niSwitch_Abort is called.
   niSwitchCheckErr(niSwitch_SetContinuousScan(
      switchSession,
      VI_TRUE));  //continuousScan

   //Calculates a delay for the switch module based on settling time and
   //scan delay
   niSwitchCheckErr(calculateSwitchTimeOut(
      switchSession,
      &switchTimeOutInMilliSeconds));

   //Initiates the scan usings the configured scan list and triggers.
   niSwitchCheckErr(niSwitch_InitiateScan(switchSession));

   //Query an attribute value to determine if the switch is ready for
   //a trigger.
   niSwitchCheckErr(niSwitch_GetAttributeViBoolean(
      switchSession,
      VI_NULL,
      NISWITCH_ATTR_IS_WAITING_FOR_TRIG,
      &isWaitingForTrigger));

   for (i = 0; ( i < (switchTimeOutInMilliSeconds/10))  && !isWaitingForTrigger; i++)
   {

      // wait for 10ms
      #ifdef _CVI_
         Delay (0.01);
      #elif defined _MSC_VER
         Sleep (10);
      #elif defined __linux__
         sleep (0.01);
      #endif

      //Query an attribute value to determine if the switch is ready for a trigger.
      niSwitchCheckErr(niSwitch_GetAttributeViBoolean(
         switchSession,
         VI_NULL,
         NISWITCH_ATTR_IS_WAITING_FOR_TRIG,
         &isWaitingForTrigger));
   }

   //Initiate the DMM measurement.
   niDMMCheckErr(niDMM_Initiate(dmmSession));

   //Download data from the DMM.
   niDMMCheckErr(niDMM_FetchMultiPoint(
      dmmSession,
      NIDMM_VAL_TIME_LIMIT_AUTO,
      dmmSampleCount,
      dmmSamplesArray,
      &actualNumberOfPoints));

   //Halts the scan.
   niSwitchCheckErr(niSwitch_AbortScan(switchSession));

   for (i = 0; i < dmmSampleCount; i++)
   {
      printf("Measurements[%d] = %f \n", i, dmmSamplesArray[i]);
   }

Error:

   if (switchError < VI_SUCCESS)
   {
      //Display errors (if any).
      niSwitch_ErrorHandler(switchSession, switchError);
   }
   if (dmmError < VI_SUCCESS)
   {
      //Display errors (if any).
      niDMM_ErrorHandler(dmmSession, dmmError);
   }
   if (dmmSession != VI_NULL)
   {
      //Close the session to DMM.
      niDMM_close(dmmSession);
   }
   if (switchSession != VI_NULL)
   {
      //Close the session to switch module.
      niSwitch_close(switchSession);
   }

   return 0;

}


//Calculates a delay for the switch module based on settling time and scan delay.
ViStatus calculateSwitchTimeOut(ViSession switchSession, ViReal64* timeOutInMilliSeconds)
{
   ViReal64 settlingTime, scanDelay;
   ViStatus switchError = VI_TRUE;

   //Query an attribute value to determine the switch module settling time.
   niSwitchCheckErr(niSwitch_GetAttributeViReal64(
      switchSession,
      VI_NULL,
      NISWITCH_ATTR_SETTLING_TIME,
      &settlingTime));

   //Query an attribte value to determine any delay during scanning.
   niSwitchCheckErr(niSwitch_GetAttributeViReal64(
      switchSession,
      VI_NULL,
      NISWITCH_ATTR_SCAN_DELAY,
      &scanDelay));

   *timeOutInMilliSeconds = (settlingTime + scanDelay) * 1000 * 10;

Error:
   return switchError;
}
