//Steps:
//1.  Open NI - RFSG session. 
//2.  Configure RFSG generation mode to Script. This allows for writing a script that determines the behavior of the generation.
//3.  Configure RFSG Frequency Reference (Clock Source and Clock Frequency).
//4.  Configure RFSG Selected Ports, External Gain and Frequency of RF output signal.
//5.  Get the terminal name for marker0. Use this as the source to configure RFSG List to advance upon receipt of a 
//    marker event.
//6.  Read waveform from file and download waveform from file to RFSG.
//7.  Retrieve waveform sample rate from the waveform file.
//8.  Retrieve the value of PAPR from the waveform file.
//9.  Write script to generate a waveform. This script is programmed to continuously generate a waveform of length
//    equal to the RFmx list step duration and generate marker0 at the end of list step acquisition.
//10.  Create a Configuration List. Pass Power Level in the Configuration List Properties parameter to be able to configure
//    Power Level in each step that we create.The Set As Active List parameter in this function defaults to true, this will set the
//    Active Configuration List property to the name of the created configuration list. Once the Active Configuration List
//    is set, using a property node to access Power Level will modify the property for this configuration list.
//11. Create a Configuration List Step.The Set As Active Step parameter in this function defaults to true, this will set the Active
//    Configuration List Step property to the created configuration list step index. Once the Active Configuration List
//    Step is set, using a property node to access Power Level will modify the property for this configuration list step in
//    the configuration list indicated by the Active Configuration List property.
//12. Configure the Power Level for the Active Configuration List Step in the Active Configuration List.
//13. Open a new RFmx Session.
//14. Configure the Frequency Reference properties (Clock Source and Clock Frequency).
//15. Create RFmx NR List.
//16. Create RFmx NR List Step.
//17. Configure the Reference Level for the Specified List Step in the NR List.
//18. Configure Trigger Parameters for IQ Power Edge Trigger on List Step0.
//19. Configure Trigger Parameters for Digital Edge Time Trigger on List Step1 to N - 1.
//20. Configure List Step Timer Offset for List Step1 to N - 1
//21. Configure List Step Timer Duration for all List steps.
//22. Create List Step string which can be used to configure personality and measurement parameters for a List Step.
//    Providing -1 on Build List Step function would create selector string with list name and step as "all". Example:"list::<listname>/step::all"
//23. Configure Center Frequency, Selected Portand External Attenuation for all List Step.
//24. Configure List Step Timer Unit as Time for all List Step.
//25. Configure Link Direction, Frequency Range, Carrier Bandwidthand Subcarrier Spacing for all List Step.
//26. Configure Sweep Time Parameters for all List Step.
//27. Select ACP measurement and enable Traces for all List Step.
//28. Initiate ACP measurement for NR List.
//29. Initiate signal generation.
//30. Wait for Acquisition to complete.
//31. Use Build List Step String to obtain selector string for a List step.
//32. Fetch ACP measurement Results for all Configuration List Steps one by one.
//33. Build List Step String for Trace step number. Fetch ACP Traces for the desired List Step.
//34. Stop signal generation.
//35. Delete the Configuration List to avoid committing the Configuration List to the hardware with subsequent
//    calls to Commit. Deleting the list will reset the Active Configuration List.
//36. Delete RFmx NR List.
//37. Close the RFmx Session.
//38. Close the RFSG session. 
//    It is recommended to clear the waveform before closing RFSG session.


#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxNR.h"
#include "niRFSG.h"
#include "niRFSGPlayback.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION                   4096

/* Maximum size of a selector string */
#define MAX_SELECTOR_STRING                     256

/* Number of offsets */
#define NUMBER_OF_OFFSETS                       3

/* CheckWarn macro for RFSG and Playback API calls*/
#define RfsgCheckWarn(fCall)     if (1) {ViStatus _code_; if (_code_ = (fCall), _code_ < 0)    \
                                    {RFSGError = _code_;goto Error;}        \
                                    else RFSGError = (RFSGError==0)?_code_:RFSGError;}    \
                                    else RFSGError = RFSGError

#define playbackCheckWarn(fCall) if (1) {ViStatus _code_; if (_code_ = (fCall), _code_ < 0)    \
                                 {playbackError = _code_;goto Error;}        \
                                 else playbackError = (playbackError==0)?_code_:playbackError;} \
                                 else playbackError = playbackError

int32 error = 0, errorOccured = 0, playbackError = 0, lastErrorCode = 0;
char errorMessage[MAX_ERROR_DESCRIPTION];

void linearRampPattern(float64 start, float64 end, int samples, int32 includeEnd, float64 *rampPattern)
{
   int m, i;
   double delta;
   m = includeEnd ? samples : (samples - 1);
   delta = (end - start) / m;
   for (i = 0; i < samples; i++)
   {
      rampPattern[i] = start + (i * delta);
   }
}

int main(int argc, char *argv[])
{
   niRFmxInstrHandle instrumentHandle = NULL;
   ViSession RFSGSession;

   char errorMessage[MAX_ERROR_DESCRIPTION] = { 0 };
   int32 error = 0, lastErrorCode = 0, errorOccured = 0;
   int32 RFSGError = 0, playbackError = 0;
   int i = 0;
   int j = 0;

   float64 centerFrequency = 3.5e9;                                              /* (Hz) */

   ViRsrc RFSGResourceName = "RFSG";
   ViConstString RFSGSelectedPorts = "";
   ViConstString waveformFilePath = "..\\Support\\NR_FR1_UL_BW-100MHz_SCS-30kHz.tdms";
   ViConstString waveformName = "Wfm";
   ViConstString waveformChannelName = "waveform::Wfm";


   ViReal64 RFSGExternalAttenuation = 0.0;                                      /* (dB) */
   ViConstString RFSGFrequencyReferenceSource = NIRFSG_VAL_ONBOARD_CLOCK_STR;
   
   ViReal64 RFSGFrequency = 10.0e6;                                             /* (Hz) */

   char *RFSAResourceName = "RFSA";
   char *RFSASelectedPorts = "";
   float64 RFSAExternalAttenuation = 0.0;                                       /* (dB) */

   char *RFSAFrequencyReferenceSource = RFMXINSTR_VAL_ONBOARD_CLOCK_STR;
   float64 RFSAFrequency = 10.0e6;                                              /* (Hz) */

   float64 triggerDelay = 0.0;                                                  /* (s) */
   float64 step0IQPowerEdgeLevel = -10;                                         /* (dBm) */
   float64 step0MinimumQuietTime = 0;                                           /* (s) */

   int32 numberOfSteps = 10;
   int32 stepIndex;

   float64 startReferenceLevel = -20.0;                                         /* (dBm) */
   float64 stopReferenceLevel = 0.0;                                            /* (dBm) */


   float64 listStepTimerDuration = 1.50e-3;                                     
   float64 listStepTimerOffset = 0;                                             

   char listName[] = "ModAcc_List";
   char listSelectorString[MAX_SELECTOR_STRING];
   char listStepSelectorString[MAX_SELECTOR_STRING];

   int32 linkDirection = RFMXNR_VAL_LINK_DIRECTION_UPLINK;
   int32 frequencyRange = RFMXNR_VAL_FREQUENCY_RANGE_RANGE1;
   float64 carrierBandwidth = 100e6;                                            /* (Hz) */
   float64 subcarrierSpacing = 30e3;                                            /* (Hz) */

   float64 sweepTimeInterval = 1.00e-3;                                         /* (s) */

   int32 traceStepNumber = 0;

   float64 *rampPattern = NULL;
   float64 timeout = 10.0;                                                      /* (s) */
   ViChar script[MAX_SELECTOR_STRING];
   ViChar markerEventTerminalName[MAX_SELECTOR_STRING];
   ViReal64 sampleRate;
   int32 numberOfSamples;
   int32 markerLocation;
   ViReal64 PAPR;
   ViReal64 externalGain;

   ViConstString RFSGListName = "PowerLevelList";
   ViInt32 numberOfAttributes = 1;
   ViAttr configurationListAttributes[] = { NIRFSG_ATTR_POWER_LEVEL };

   /* Variables to store the results */
   float64 absolutePower = 0.0;                                                 /* (dBm) */

   float64 lowerRelativePower[NUMBER_OF_OFFSETS] = { 0 };                       /* (dB) */
   float64 upperRelativePower[NUMBER_OF_OFFSETS] = { 0 };                       /* (dB) */
   float64 lowerAbsolutePower[NUMBER_OF_OFFSETS] = { 0 };                       /* (dBm) */
   float64 upperAbsolutePower[NUMBER_OF_OFFSETS] = { 0 };                       /* (dBm) */

   int32 actualArraySize = 0, arraySize = 0;
   float64 x0 = 0.0, dx = 0.0;
   float32* relativePowersTrace = NULL;
   float32* spectrum = NULL;

   /* Initialize an RFSG Session */
   RfsgCheckWarn(niRFSG_init(RFSGResourceName, VI_TRUE, VI_FALSE, &RFSGSession));
   RfsgCheckWarn(niRFSG_ConfigureGenerationMode(RFSGSession, NIRFSG_VAL_SCRIPT));
   RfsgCheckWarn(niRFSG_ConfigureRefClock(RFSGSession, RFSGFrequencyReferenceSource, RFSGFrequency));

   externalGain = -1 * RFSGExternalAttenuation;
   RfsgCheckWarn(niRFSG_SetAttributeViReal64(RFSGSession, "", NIRFSG_ATTR_EXTERNAL_GAIN, externalGain));
   RfsgCheckWarn(niRFSG_SetAttributeViString(RFSGSession, "", NIRFSG_ATTR_SELECTED_PORTS, RFSGSelectedPorts));
   RfsgCheckWarn(niRFSG_SetAttributeViReal64(RFSGSession, "", NIRFSG_ATTR_FREQUENCY, centerFrequency));

   RfsgCheckWarn(niRFSG_GetTerminalName(RFSGSession, NIRFSG_VAL_MARKER_EVENT, NIRFSG_VAL_MARKER_EVENT0, MAX_SELECTOR_STRING, markerEventTerminalName));
   RfsgCheckWarn(niRFSG_ConfigureDigitalEdgeConfigurationListStepTrigger(RFSGSession, markerEventTerminalName, NIRFSG_VAL_RISING_EDGE));

   RfsgCheckWarn(niRFSG_ReadAndDownloadWaveformFromFileTDMS(RFSGSession, waveformName, waveformFilePath, 0));
   RfsgCheckWarn(niRFSG_GetAttributeViReal64(RFSGSession, waveformChannelName, NIRFSG_ATTR_WAVEFORM_IQ_RATE, &sampleRate));
   RfsgCheckWarn(niRFSG_GetAttributeViReal64(RFSGSession, waveformChannelName, NIRFSG_ATTR_WAVEFORM_PAPR, &PAPR));
   numberOfSamples = (int32)(sampleRate * listStepTimerDuration);
   markerLocation = (int32)(sampleRate * sweepTimeInterval);
   sprintf(script, "script GenerateWaveform\n  repeat forever\n    generate %s subset(0, %d) marker0(%d)\n \
      end repeat\n  end script", waveformName, numberOfSamples, markerLocation);
   RfsgCheckWarn(niRFSG_WriteScript(RFSGSession, script));

   RfsgCheckWarn(niRFSG_CreateConfigurationList(RFSGSession, RFSGListName, numberOfAttributes, configurationListAttributes, VI_TRUE));

   rampPattern = (float64*)malloc(sizeof(float64) * numberOfSteps);
   linearRampPattern(startReferenceLevel, stopReferenceLevel, numberOfSteps, 0, rampPattern);
   for (i = 0; i < numberOfSteps; i++)
   {
       RfsgCheckWarn(niRFSG_CreateConfigurationListStep(RFSGSession, VI_TRUE));
       RfsgCheckWarn(niRFSG_SetAttributeViReal64(RFSGSession, "", NIRFSG_ATTR_POWER_LEVEL, rampPattern[i]));
   }

   /* Initialize an RFmx session */
   RFmxCheckWarn(RFmxNR_Initialize(RFSAResourceName, "", &instrumentHandle, NULL));

   RFmxCheckWarn(RFmxInstr_CfgFrequencyReference(instrumentHandle, "", RFSAFrequencyReferenceSource, RFSAFrequency));

   RFmxCheckWarn(RFmxNR_CreateList(instrumentHandle, listName));
   RFmxCheckWarn(RFmxNR_BuildListString(listName, "", MAX_SELECTOR_STRING, listSelectorString));

   for (i = 0; i < numberOfSteps; i++)
   {
       RFmxCheckWarn(RFmxNR_CreateListStep(instrumentHandle, listSelectorString, &stepIndex));
       RFmxCheckWarn(RFmxNR_BuildListStepString(listName, "", stepIndex, MAX_SELECTOR_STRING, listStepSelectorString));
       RFmxCheckWarn(RFmxNR_SetReferenceLevel(instrumentHandle, listStepSelectorString, rampPattern[i] + PAPR));
       if (i == 0)
       {
           RFmxCheckWarn(RFmxNR_CfgIQPowerEdgeTrigger(instrumentHandle, listStepSelectorString, "0",
               RFMXNR_VAL_IQ_POWER_EDGE_RISING_SLOPE, step0IQPowerEdgeLevel, triggerDelay,
               RFMXNR_VAL_TRIGGER_MINIMUM_QUIET_TIME_MODE_MANUAL, step0MinimumQuietTime, 0, 1));
       }
       else
       {
           RFmxCheckWarn(RFmxNR_CfgDigitalEdgeTrigger(instrumentHandle, listStepSelectorString,
               RFMXNR_VAL_TIMER_EVENT_STR, RFMXNR_VAL_DIGITAL_EDGE_RISING_EDGE, triggerDelay, 1));
           RFmxCheckWarn(RFmxNR_SetListStepTimerOffset(instrumentHandle, listStepSelectorString, listStepTimerOffset));
       }
       RFmxCheckWarn(RFmxNR_SetListStepTimerDuration(instrumentHandle, listStepSelectorString, listStepTimerDuration));
   }

   RFmxCheckWarn(RFmxNR_BuildListStepString(listName, "", -1, MAX_SELECTOR_STRING, listStepSelectorString));

   RFmxCheckWarn(RFmxNR_SetSelectedPorts(instrumentHandle, listStepSelectorString, RFSASelectedPorts));
   RFmxCheckWarn(RFmxNR_CfgFrequency(instrumentHandle, listStepSelectorString, centerFrequency));
   RFmxCheckWarn(RFmxNR_CfgExternalAttenuation(instrumentHandle, listStepSelectorString, RFSAExternalAttenuation));
   RFmxCheckWarn(RFmxNR_SetListStepTimerUnit(instrumentHandle, listStepSelectorString, RFMXNR_VAL_LIST_STEP_TIMER_UNIT_TIME));
   RFmxCheckWarn(RFmxNR_SetLinkDirection(instrumentHandle, listStepSelectorString, linkDirection));
   RFmxCheckWarn(RFmxNR_SetFrequencyRange(instrumentHandle, listStepSelectorString, frequencyRange));
   RFmxCheckWarn(RFmxNR_SetComponentCarrierBandwidth(instrumentHandle, listStepSelectorString, carrierBandwidth));
   RFmxCheckWarn(RFmxNR_SetBandwidthPartSubcarrierSpacing(instrumentHandle, listStepSelectorString, subcarrierSpacing));

   RFmxCheckWarn(RFmxNR_ACPCfgSweepTime(instrumentHandle, listStepSelectorString, RFMXNR_VAL_ACP_SWEEP_TIME_AUTO_FALSE, sweepTimeInterval));

   RFmxCheckWarn(RFmxNR_SelectMeasurements(instrumentHandle, listStepSelectorString, RFMXNR_VAL_ACP, RFMXNR_VAL_TRUE));

   RFmxCheckWarn(RFmxNR_Initiate(instrumentHandle, listSelectorString, ""));
   RfsgCheckWarn(niRFSG_Initiate(RFSGSession));
   RFmxCheckWarn(RFmxInstr_WaitForAcquisitionComplete(instrumentHandle, timeout));

   for (i = 0; i < numberOfSteps; i++)
   {
       RFmxCheckWarn(RFmxNR_BuildListStepString(listName, "", i, MAX_SELECTOR_STRING, listStepSelectorString));
       printf("\n\n----------- Measurements for %s -----------\n\n", listStepSelectorString);

       RFmxCheckWarn(RFmxNR_ACPFetchOffsetMeasurementArray(instrumentHandle, listStepSelectorString, timeout,
           lowerRelativePower, upperRelativePower,
           lowerAbsolutePower, upperAbsolutePower,
           NUMBER_OF_OFFSETS, NULL));
       RFmxCheckWarn(RFmxNR_ACPFetchComponentCarrierMeasurement(instrumentHandle, listStepSelectorString, timeout,
           &absolutePower, NULL));

       printf("Carrier Measurements: \n");
       printf("Absolute Power (dBm or dBm/Hz)          : %f\n", absolutePower);
       printf("---------------------------------------------------\n");

       printf("Offset Channel Measurements: \n");
       for (j = 0; j < NUMBER_OF_OFFSETS; j++)
       {
           printf("Offset  :  %d\n", j);
           printf("Lower Relative Power (dB)               : %f\n", lowerRelativePower[j]);
           printf("Upper Relative Power (dB)               : %f\n", upperRelativePower[j]);
           printf("Lower Absolute Power (dBm or dBm/Hz)    : %f\n", lowerAbsolutePower[j]);
           printf("Upper Absolute Power (dBm or dBm/Hz)    : %f\n", upperAbsolutePower[j]);
           printf("-------------------------------------------------\n");
       }
   }

   RFmxCheckWarn(RFmxNR_BuildListStepString(listName, "", traceStepNumber, MAX_SELECTOR_STRING, listStepSelectorString));
   for (i = 0; i < arraySize; i++)
   {
       actualArraySize = 0;
       RFmxCheckWarn(RFmxNR_ACPFetchRelativePowersTrace(instrumentHandle, listStepSelectorString, timeout, i,
           NULL, NULL, NULL, 0, &actualArraySize));
       if (actualArraySize > 0)
       {
           relativePowersTrace = (float32*)malloc(sizeof(float32) * actualArraySize);
           if (relativePowersTrace)
               RFmxCheckWarn(RFmxNR_ACPFetchRelativePowersTrace(instrumentHandle, listStepSelectorString, timeout,
                   i, &x0, &dx, relativePowersTrace, actualArraySize, NULL));
       }
   }

   actualArraySize = 0;
   RFmxCheckWarn(RFmxNR_ACPFetchSpectrum(instrumentHandle, listStepSelectorString, timeout, NULL, NULL, NULL, 0, &actualArraySize));
   if (actualArraySize > 0)
   {
       spectrum = (float32*)malloc(sizeof(float32) * actualArraySize);
       if (spectrum)
       {
           RFmxCheckWarn(RFmxNR_ACPFetchSpectrum(instrumentHandle, listStepSelectorString, timeout, &x0, &dx, spectrum,
               actualArraySize, NULL));
       }
       else
       {
           printf("malloc failed.\n");
           goto Error;
       }
   }


   RfsgCheckWarn(niRFSG_Abort(RFSGSession));
   RfsgCheckWarn(niRFSG_DeleteConfigurationList(RFSGSession, RFSGListName));

   RFmxCheckWarn(RFmxNR_DeleteList(instrumentHandle, listSelectorString));
   RFmxCheckWarn(RFmxNR_Close(instrumentHandle, RFMXNR_VAL_FALSE));

   RfsgCheckWarn(niRFSG_ClearArbWaveform(RFSGSession, waveformName));
   
   RfsgCheckWarn(niRFSG_close(RFSGSession));

Error:
   if (error)
   {
      errorOccured = error;
      RFmxNR_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
      if (error < 0)
         printf("ERROR: %s\n", errorMessage);
      else
         printf("WARNING: %s\n", errorMessage);
   }

   if (RFSGError)
   {
      errorOccured = RFSGError;
      niRFSG_GetError(RFSGSession, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
      if (RFSGError < 0)
         printf("ERROR: %s\n", errorMessage);
      else
         printf("WARNING: %s\n", errorMessage);
   }

   if (playbackError)
   {
      errorOccured = playbackError;
      niRFSGPlayback_GetError(&lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
      if (playbackError < 0)
         printf("ERROR: %s\n", errorMessage);
      else
         printf("WARNING: %s\n", errorMessage);
   }

   if (instrumentHandle)
   {
      RFmxNR_Close(instrumentHandle, RFMXNR_VAL_FALSE);
   }
   if (RFSGSession)
   {
      niRFSG_Abort(RFSGSession);
      niRFSGPlayback_ClearWaveform(RFSGSession, waveformName);
      niRFSG_close(RFSGSession);
   }

   /* Free allocated memory */
   if (rampPattern)
      free(rampPattern);
   if (relativePowersTrace)
      free(relativePowersTrace);
   if (spectrum)
      free(spectrum);

   printf("\nPress any key to exit\n");
   _getch();

   return errorOccured;
}