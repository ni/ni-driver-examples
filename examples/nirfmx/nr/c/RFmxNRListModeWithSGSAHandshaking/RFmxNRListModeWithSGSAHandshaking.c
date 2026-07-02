//Steps:
//1. Open NI-RFSG session.
//2. Configure RFSG Selected Ports.
//3. Configure RFSG frequency reference.
//4. Configure RFSG configuration settled event to the device scriptTrigger0, to make sure generation starts only after
//   RFSG configuration settled.
//5. Configure RFSG to advance upon receipt of RFSA Ready for Advance event through PXI trigger line.
//6. Configure frequency and external gain of RF output signal.
//7. Get terminal name for marker0 and assign to the RFSA Reference Trigger Digital Edge source.
//8. Create a Configuration List. Pass Power Level in the Configuration List Properties parameter to be able to configure
//   Power Level in each step that we create. The Set As Active List parameter in this VI defaults to true, this will set the
//   Active Configuration List property to the name of the created configuration list.Once the Active Configuration List
//   is set, using a property node to access Power Level will modify the property for this configuration list.
//9. Create a Configuration List Step.The Set As Active Step parameter in this VI defaults to true, this will set the Active
//   Configuration List Step property to the created configuration list step index.Once the Active Configuration List
//   Step is set, using a property node to access Power Level will modify the property for this configuration list step in
//   the configuration list indicated by the Active Configuration List property.
//10. Configure the Power Level for the Active Configuration List Step in the Active Configuration List.
//11. Read waveform from file and download Waveform from file to RFSG.
//12. Retrieve waveform PAPR and add the same to the RFSA reference level while configuring to every list step.
//13. Set Automatic SG SA Shared LO to Enabled.
//14. Set LO Offset Mode to Auto while performing an in-band ModAcc measurement. This causes the RFSG LO to be
//    placed outside the signal, if signal bandwidth is less than half of the device instantaneous bandwidth; otherwise,
//    the LO is placed at the center of the signal.
//15. Write script to generate the waveform specified in the script. This script is programmed to generate waveform
//    continuously and generate a marker at the start of the waveform (sample 0).
//16. Open a new RFmx Session.
//17. Configure the Frequency Reference properties (Clock Source and Clock Frequency).
//18. Export RFSA Ready for Advance event to PXI trigger line.
//19. Create RFmx NR List.
//20. Create RFmx NR List Step.
//21. Configure the Reference Level for the Specified List Step in the NR List.
//22. Configure personality and measurement parameters for a List Step.
//23. Configure Center Frequency, Selected Ports, External Attenuation, Trigger Type and Trigger Parameters for all List Step.
//24. Configure Link Direction, Frequency Range, CC bandwidth, Cell ID, Band, and BWP Subcarrier Spacing for all List Step.
//25. Set LO Leakage Avoidance Enabled to True and Automatic SG SA Shared LO to Enabled. Enabling LO Leakage Avoidance causes RFmx
//    to place the SA LO outside the measurement bandwidth, if the measurement bandwidth is less than half of the device instantaneous
//    bandwidth; otherwise, the LO is placed at the center of the signal.
//26. Select ModAcc measurement and disable traces for all List Step.
//27. Initiate ModAcc measurement for List.
//28. Initiate signal generation.
//29. Wait for Acquisition to complete.
//30. Fetch ModAcc measurement Results for all Configuration List Steps one by one.
//31. Stop signal generation.
//32. Delete the Configuration List to avoid committing the Configuration List to the hardware with subsequent
//    calls to Commit. Deleting the list will reset the Active Configuration List.
//33. Delete RFmx NR List.
//34. Close the RFmx Session.
//35. Close the RFSG session. 
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

   float64 centerFrequency = 3.5e9;                                              /* (Hz) */

   ViRsrc RFSGResourceName = "RFSG";
   ViConstString RFSGSelectedPorts = "";
   ViConstString waveformFilePath = "..\\Support\\NR_FR2_UL_SISO_CC-1_BW-50MHz_SCS-120kHz.tdms";
   ViConstString waveformName = "Wfm";

   ViReal64 RFSGExternalAttenuation = 0.0;                                      /* (dB) */
   ViConstString RFSGFrequencyReferenceSource = NIRFSG_VAL_ONBOARD_CLOCK_STR;
   
   ViReal64 RFSGFrequency = 10.0e6;                                             /* (Hz) */

   char *RFSAResourceName = "RFSA";
   char *RFSASelectedPorts = "";
   float64 RFSAExternalAttenuation = 0.0;                                       /* (dB) */

   char *RFSAFrequencyReferenceSource = RFMXINSTR_VAL_ONBOARD_CLOCK_STR;
   float64 RFSAFrequency = 10.0e6;                                              /* (Hz) */

   char listName[] = "ModAcc_List";
   char listSelectorString[MAX_SELECTOR_STRING];
   char listStepSelectorString[MAX_SELECTOR_STRING];

   int32 digitalEdge = RFMXNR_VAL_DIGITAL_EDGE_RISING_EDGE;
   float64 triggerDelay = 0.0;                                                  /* (s) */

   int32 numberOfSteps = 10;
   int32 stepIndex;

   float64 startReferenceLevel = -20.0;                                         /* (dBm) */
   float64 stopReferenceLevel = 0.0;                                            /* (dBm) */

   int32 frequencyRange = RFMXNR_VAL_FREQUENCY_RANGE_RANGE2;
   float64 carrierBandwidth = 50e6;                                             /* (Hz) */
   float64 subcarrierSpacing = 120e3;                                           /* (Hz) */
   int32 band = 257;
   int32 cellID = 0;

   float64 *rampPattern = NULL;
   float64 timeout = 10.0;                                                      /* (s) */
   ViChar script[MAX_SELECTOR_STRING];
   ViReal64 papr;
   ViChar configurationSettledEvenTerminalName[MAX_SELECTOR_STRING];
   ViChar markerEventTerminalName[MAX_SELECTOR_STRING];
   ViReal64 externalGain;

   ViConstString RFSGListName = "PowerLevelList";
   ViInt32 numberOfAttributes = 1;
   ViAttr configurationListAttributes[] = { NIRFSG_ATTR_POWER_LEVEL };

   float64 *compositeRmsEvmMean = NULL;
   float64 *compositePeakEvmMaximum = NULL;

   RfsgCheckWarn(niRFSG_init(RFSGResourceName, VI_TRUE, VI_FALSE, &RFSGSession));
   RfsgCheckWarn(niRFSG_SetAttributeViString(RFSGSession, "", NIRFSG_ATTR_SELECTED_PORTS, RFSGSelectedPorts));
   RfsgCheckWarn(niRFSG_ConfigureRefClock(RFSGSession, RFSGFrequencyReferenceSource, RFSGFrequency));
   RfsgCheckWarn(niRFSG_GetAttributeViString(RFSGSession, "", NIRFSG_ATTR_CONFIGURATION_SETTLED_EVENT_TERMINAL_NAME,
      MAX_SELECTOR_STRING, configurationSettledEvenTerminalName));
   RfsgCheckWarn(niRFSG_ConfigureDigitalEdgeScriptTrigger(RFSGSession, NIRFSG_VAL_SCRIPT_TRIGGER0, configurationSettledEvenTerminalName, NIRFSG_VAL_RISING_EDGE));
   RfsgCheckWarn(niRFSG_ConfigureDigitalEdgeConfigurationListStepTrigger(RFSGSession, NIRFSG_VAL_PXI_TRIG0_STR, NIRFSG_VAL_RISING_EDGE));
   externalGain = -1 * RFSGExternalAttenuation;
   RfsgCheckWarn(niRFSG_SetAttributeViReal64(RFSGSession, "", NIRFSG_ATTR_EXTERNAL_GAIN, externalGain));
   RfsgCheckWarn(niRFSG_SetAttributeViReal64(RFSGSession, "", NIRFSG_ATTR_FREQUENCY, centerFrequency));
   RfsgCheckWarn(niRFSG_GetTerminalName(RFSGSession, NIRFSG_VAL_MARKER_EVENT, NIRFSG_VAL_MARKER_EVENT0, MAX_SELECTOR_STRING, markerEventTerminalName));
   RfsgCheckWarn(niRFSG_CreateConfigurationList(RFSGSession, RFSGListName, numberOfAttributes, configurationListAttributes, VI_TRUE));

   rampPattern = (float64*)malloc(sizeof(float64) * numberOfSteps);
   linearRampPattern(startReferenceLevel, stopReferenceLevel, numberOfSteps, 0, rampPattern);
   for (i = 0; i < numberOfSteps; i++)
   {
      RfsgCheckWarn(niRFSG_CreateConfigurationListStep(RFSGSession, VI_TRUE));
      RfsgCheckWarn(niRFSG_SetAttributeViReal64(RFSGSession, "", NIRFSG_ATTR_POWER_LEVEL, rampPattern[i]));
   }

   playbackCheckWarn(niRFSGPlayback_ReadAndDownloadWaveformFromFile(RFSGSession, waveformFilePath, waveformName));
   playbackCheckWarn(niRFSGPlayback_RetrieveWaveformPAPR(RFSGSession, waveformName, &papr));
   playbackCheckWarn(niRFSGPlayback_StoreAutomaticSGSASharedLO(RFSGSession, "", NIRFSGPLAYBACK_VAL_AUTOMATIC_SG_SA_SHARED_LO_ENABLED));
   playbackCheckWarn(niRFSGPlayback_StoreWaveformLOOffsetMode(RFSGSession, waveformName, NIRFSGPLAYBACK_VAL_LO_OFFSET_MODE_AUTO));
   sprintf(script, "script GenerateWaveform\n  repeat forever\n    generate %s marker0(0)\n \
      wait until scripttrigger0\n   end repeat\n  end script", waveformName);
   playbackCheckWarn(niRFSGPlayback_SetScriptToGenerateSingleRFSG(RFSGSession, script));

   /* Initialize a session */
   RFmxCheckWarn(RFmxNR_Initialize(RFSAResourceName, "", &instrumentHandle, NULL));

   RFmxCheckWarn(RFmxInstr_CfgFrequencyReference(instrumentHandle, "", RFSAFrequencyReferenceSource, RFSAFrequency));
   RFmxCheckWarn(RFmxInstr_ExportSignal(instrumentHandle, RFMXINSTR_VAL_READY_FOR_ADVANCE_EVENT, RFMXINSTR_VAL_PXI_TRIG0_STR));

   RFmxCheckWarn(RFmxNR_CreateList(instrumentHandle, listName));
   RFmxCheckWarn(RFmxNR_BuildListString(listName, "", MAX_SELECTOR_STRING, listSelectorString));

   for (i = 0; i < numberOfSteps; i++)
   {
      RFmxCheckWarn(RFmxNR_CreateListStep(instrumentHandle, listSelectorString, &stepIndex));
      RFmxCheckWarn(RFmxNR_BuildListStepString(listName, "", stepIndex, MAX_SELECTOR_STRING, listStepSelectorString));
      RFmxCheckWarn(RFmxNR_SetReferenceLevel(instrumentHandle, listStepSelectorString, papr + rampPattern[i]));
   }

   RFmxCheckWarn(RFmxNR_BuildListStepString(listName, "", -1, MAX_SELECTOR_STRING, listStepSelectorString));

   RFmxCheckWarn(RFmxNR_CfgFrequency(instrumentHandle, listStepSelectorString, centerFrequency));
   RFmxCheckWarn(RFmxNR_SetSelectedPorts(instrumentHandle, listStepSelectorString, RFSASelectedPorts));
   RFmxCheckWarn(RFmxNR_CfgDigitalEdgeTrigger(instrumentHandle, listStepSelectorString, markerEventTerminalName, 
      digitalEdge, triggerDelay, 1));
   RFmxCheckWarn(RFmxNR_CfgExternalAttenuation(instrumentHandle, listStepSelectorString, RFSAExternalAttenuation));
   RFmxCheckWarn(RFmxNR_SetLinkDirection(instrumentHandle, listStepSelectorString, RFMXNR_VAL_LINK_DIRECTION_UPLINK));
   RFmxCheckWarn(RFmxNR_SetFrequencyRange(instrumentHandle, listStepSelectorString, frequencyRange));
   RFmxCheckWarn(RFmxNR_SetComponentCarrierBandwidth(instrumentHandle, listStepSelectorString, carrierBandwidth));
   RFmxCheckWarn(RFmxNR_SetCellID(instrumentHandle, listStepSelectorString, cellID));
   RFmxCheckWarn(RFmxNR_SetBand(instrumentHandle, listStepSelectorString, band));
   RFmxCheckWarn(RFmxNR_SetBandwidthPartSubcarrierSpacing(instrumentHandle, listStepSelectorString, subcarrierSpacing));

   RFmxCheckWarn(RFmxInstr_SetLOSource(instrumentHandle, "", RFMXINSTR_VAL_LO_SOURCE_AUTOMATIC_SG_SA_SHARED));
   RFmxCheckWarn(RFmxInstr_SetLOLeakageAvoidanceEnabled(instrumentHandle, "", RFMXINSTR_VAL_LO_LEAKAGE_AVOIDANCE_ENABLED_TRUE));

   RFmxCheckWarn(RFmxNR_SelectMeasurements(instrumentHandle, listStepSelectorString, RFMXNR_VAL_MODACC, RFMXNR_VAL_FALSE));

   RFmxCheckWarn(RFmxNR_Initiate(instrumentHandle, listSelectorString, ""));
   RfsgCheckWarn(niRFSG_Initiate(RFSGSession));
   RFmxCheckWarn(RFmxInstr_WaitForAcquisitionComplete(instrumentHandle, timeout));

   compositeRmsEvmMean = (float64*)malloc(sizeof(float64) * numberOfSteps);
   compositePeakEvmMaximum = (float64*)malloc(sizeof(float64) * numberOfSteps);

   for (i = 0; i < numberOfSteps; i++)
   {
      RFmxCheckWarn(RFmxNR_BuildListStepString(listName, "", i, MAX_SELECTOR_STRING, listStepSelectorString));
      RFmxCheckWarn(RFmxNR_ModAccGetResultsCompositeRMSEVMMean(instrumentHandle, listStepSelectorString, &compositeRmsEvmMean[i]));
      RFmxCheckWarn(RFmxNR_ModAccGetResultsCompositePeakEVMMaximum(instrumentHandle, listStepSelectorString, &compositePeakEvmMaximum[i]));
   }

   printf("------------------Measurements------------------\n\n");
   printf("Composite RMS EVM Mean (%%)\n");
   for (i = 0; i < numberOfSteps; i++)
   {
      RFmxCheckWarn(RFmxNR_BuildListStepString(listName, "", i, MAX_SELECTOR_STRING, listStepSelectorString));
      printf("Step%d    : %f\n", i, compositeRmsEvmMean[i]);
   }
   printf("\nComposite Peak EVM Maximum (%%)\n");
   for (i = 0; i < numberOfSteps; i++)
   {
      RFmxCheckWarn(RFmxNR_BuildListStepString(listName, "", i, MAX_SELECTOR_STRING, listStepSelectorString));
      printf("Step%d    : %f\n", i, compositePeakEvmMaximum[i]);
   }

   RfsgCheckWarn(niRFSG_Abort(RFSGSession));
   RfsgCheckWarn(niRFSG_DeleteConfigurationList(RFSGSession, RFSGListName));

   RFmxCheckWarn(RFmxNR_DeleteList(instrumentHandle, listSelectorString));
   RFmxCheckWarn(RFmxNR_Close(instrumentHandle, RFMXNR_VAL_FALSE));

   playbackCheckWarn(niRFSGPlayback_ClearWaveform(RFSGSession, waveformName));

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
   if (compositeRmsEvmMean)
      free(compositeRmsEvmMean);
   if (compositePeakEvmMaximum)
      free(compositePeakEvmMaximum);

   printf("\nPress any key to exit\n");
   _getch();

   return errorOccured;
}