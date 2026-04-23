/*

 National Instruments - NI-SWITCH - CVI Examples

 Handshaking

 This example demonstrates how to scan a series of channels on a switch module and take measurements with an NI digital multimeter using handshaking.

 Refer to the NI Switches Help to determine if your switch supports scanning, the scan list syntax, and the valid channel names and valid resource names for your switch module.

 NI-SWITCH functions utilized:

 niSwitch_InitWithTopology
 niSwitch_ConfigureScanList
 niSwitch_ConfigureScanTrigger
 niSwitch_SetContinuousScan
 niSwitch_SetAttributeViInt32
 niSwitch_InitiateScan
 niSwitch_GetAttributeViBoolean
 niSwitch_GetAttributeViReal64
 niSwitch_AbortScan
 niSwitch_close

 NI-DMM functions utilized:

 niDMM_init
 niDMM_ConfigureMeasurement
 niDMM_ConfigureTrigger
 niDMM_ConfigureTriggerSlope
 niDMM_ConfigureMultiPoint
 niDMM_ConfigureSampleTriggerSlope
 niDMM_ConfigureMeasCompleteDest
 niDMM_ConfigureMeasCompleteSlope
 niDMM_GetAttributeViString
 niDMM_Initiate
 niDMM_FetchMultiPoint
 niDMM_close

*/

#include <stdio.h>
#include "niswitch.h"
#include "niSwitchErrorHandler.h"
#include "niDMMErrorHandler.h"
#include "nidmm.h"

int main (void)
{

   ViSession switchSession = VI_NULL;
   ViRsrc switchResourceName = "Dev1";
   ViConstString scanList = "ch0:9->com0;";
   ViStatus switchError = VI_SUCCESS;
   ViConstString topology = NISWITCH_TOPOLOGY_CONFIGURED_TOPOLOGY;
   ViBoolean isWaitingForTrigger = VI_FALSE;
   ViInt32 i;

   ViSession dmmSession = VI_NULL;
   ViRsrc dmmResourceName = "Dev2";
   ViInt32 dmmSampleCount = 10;
   ViReal64 dmmSamplesArray[10];
   ViInt32 actualNumberOfPoints;
   ViStatus dmmError = VI_SUCCESS;
   ViChar dmmInstrumentModel[256];

   //Programming the DMM

   //Open a session to the DMM.
   niDMMCheckErr(niDMM_init(dmmResourceName, VI_TRUE, VI_TRUE, &dmmSession));

   //Configure the function, range, and resolution of the measurement.
   niDMMCheckErr(niDMM_ConfigureMeasurement(
      dmmSession,
      NIDMM_VAL_DC_VOLTS,
      10.0,     //range
      0.0001)); //resolution

   //Configure the input trigger for the DMM.
   //This should match the output trigger (Scan Advanced Output) of the switch.
   niDMMCheckErr(niDMM_ConfigureTrigger(
      dmmSession,
      NIDMM_VAL_EXTERNAL,     //trigSource,
      NIDMM_VAL_AUTO_DELAY)); //triggerDelay

   //Configures the slope of the DMM input trigger.
   niDMMCheckErr(niDMM_ConfigureTriggerSlope(
      dmmSession,
      NIDMM_VAL_NEGATIVE)); //polarity

   //Configure a multipoint acquisition.
   niDMMCheckErr(niDMM_ConfigureMultiPoint(
      dmmSession,
      1,                  //triggerCount
      dmmSampleCount,
      NIDMM_VAL_EXTERNAL, //sampleTrigger
      -1));               //sampleInterval

   //Configures the slope of the secondary DMM input trigger (Sample Trigger).
   niDMMCheckErr(niDMM_ConfigureSampleTriggerSlope(
      dmmSession,
      NIDMM_VAL_NEGATIVE));

   //Configures the destination of the DMM output trigger (Measurement Complete).
   //This should match the switch module input trigger.
   niDMMCheckErr(niDMM_ConfigureMeasCompleteDest(
      dmmSession,
      NIDMM_VAL_EXTERNAL));

   //Configures the slope of the DMM output trigger.
   niDMMCheckErr(niDMM_ConfigureMeasCompleteSlope(
      dmmSession,
      NIDMM_VAL_NEGATIVE));

   //Queries an attribute value to determine the instrument module of the DMM.
   niDMMCheckErr(niDMM_GetAttributeViString(
      dmmSession,
      VI_NULL,
      NIDMM_ATTR_INSTRUMENT_MODEL,
      256,
      dmmInstrumentModel));

   //Initiate the DMM measurement.
   niDMMCheckErr(niDMM_Initiate(dmmSession));


   //Programming the Switch

   //Open a session to the switch module and set the topology.
   niSwitchCheckErr(niSwitch_InitWithTopology(
      switchResourceName,
      topology,
      VI_FALSE,
      VI_TRUE,
      &switchSession));

   //Configures the switch module for scanning. Refer to NI Switches Help for
   //scan list syntax.
   niSwitchCheckErr(niSwitch_ConfigureScanList(
      switchSession,
      scanList,
      NISWITCH_VAL_BREAK_BEFORE_MAKE));  //scanMode

   //Configures the input trigger of the switch module. This should match the
   //output trigger of the DMM.
   niSwitchCheckErr(niSwitch_ConfigureScanTrigger(
      switchSession,
      0.0, // scanDelay
      NISWITCH_VAL_FRONTCONNECTOR,   //triggerInput
      NISWITCH_VAL_FRONTCONNECTOR)); //scanAdvancedOutput

   //Configures the switch to loop continuously through the scan list until
   //niSwitch_Abort is called.
   niSwitchCheckErr(niSwitch_SetContinuousScan(
      switchSession,
      VI_TRUE)); //continuousScan

   //Sets the polarity and edge of the switch input trigger.
   niSwitchCheckErr(niSwitch_SetAttributeViInt32(
      switchSession,
      VI_NULL,
      NISWITCH_ATTR_TRIGGER_INPUT_POLARITY,
      NISWITCH_VAL_FALLING_EDGE));

   //Sets the polarity and edge of the switch output trigger.
   niSwitchCheckErr(niSwitch_SetAttributeViInt32(
      switchSession,
      VI_NULL,
      NISWITCH_ATTR_SCAN_ADVANCED_POLARITY,
      NISWITCH_VAL_FALLING_EDGE));

   //Initiates the scan usings the configured scan list and triggers.
   niSwitchCheckErr(niSwitch_InitiateScan(switchSession));

   //Download data from the DMM.
   niDMMCheckErr(niDMM_FetchMultiPoint(
      dmmSession,
      NIDMM_VAL_TIME_LIMIT_AUTO, //maxTime
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

