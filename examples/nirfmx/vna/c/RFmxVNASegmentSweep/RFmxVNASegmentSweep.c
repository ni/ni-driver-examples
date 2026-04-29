//Steps:
//1. Open a new RFmx session.
//2. Configure Frequency Reference.
//3. Configure sweep settings: IF Bandwidth, Power Level and Test Rx Attenuation with different port names.
//4a. Configure Sweep Type as Segment. Configure Number of Segments and Independent Settings enabled per segment. 
//4b. Configure per segment settings like Segment Enabled, Start and Stop Frequencies, Number of Frequency points, Segment IF Bandwidth, Segment Dwell Time, Segment Power Level and Segment Test Receiver Attenuation.
//
//For the properties where the Segment <property> Enabled was set to True in 4a, values configured in 4b are used. If they were set to False, values configured in 3 are used.
//
//5. Configure Trigger settings.
//6. Select S-Parameter measurement.
//7. Configure number of S-Parameters.
//8. Configure each S-Parameter and format.
//9. Configure Magnitude Units & Phase Trace Type.
//10. Configure Calibration Ports and Calibration Method
//11. Configure Connector type & vCal Resource Name for each VNA port
//12. Initiate Calibration
//13. Acquire Calibration data after user confirmation 
//14. Save Calibration data
//15. Enable Correction
//16. Initiate the Measurement after user confirmation
//17. Read Number of SParams.
//18. Fetch S-Parameter X data.
//19. Fetch S-Parameter Y data for each S-Parameter.
//20. Fetch S-Parameter Correction State. 
//21. Close RFmx Session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxVNA.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION                4096
/* Maximum size of a selector string */
#define MAX_SELECTOR_STRING                  256
#define NUMBER_OF_SPARAMS                    4
#define NUMBER_OF_SEGMENTS                   5

int main(int argc, char *argv[])
{
   char *resourceName = "VNA";
   niRFmxInstrHandle instrumentHandle = NULL;

   char errorMessage[MAX_ERROR_DESCRIPTION] = { 0 };
   int32 error = 0;
   int32 lastErrorCode = 0;

   float64 IFBandwidth = 100.0e3;                                                                        /* (Hz) */
   float64 port1PowerLevel = -10.0;                                                                      /* (dBm) */
   float64 port2PowerLevel = -10.0;                                                                      /* (dBm) */
   float64 port1TestReceiverAttenuation = 0.0;                                                           /* (dB) */
   float64 port2TestReceiverAttenuation = 0.0;                                                           /* (dB) */
   int32 numberOfSegment = NUMBER_OF_SEGMENTS;
   int32 segmentPowerLevelEnabled = RFMXVNA_VAL_SEGMENT_POWER_LEVEL_ENABLED_TRUE;
   int32 segmentIFBandwidthEnabled = RFMXVNA_VAL_SEGMENT_IF_BANDWIDTH_ENABLED_TRUE;
   int32 segmentTestReceiverAttenuationEnabled = RFMXVNA_VAL_SEGMENT_TEST_RECEIVER_ATTENUATION_ENABLED_TRUE;
   int32 segmentDwellTimeEnabled = RFMXVNA_VAL_SEGMENT_DWELL_TIME_ENABLED_TRUE;
   int32 segmentEnabled[NUMBER_OF_SEGMENTS] = {RFMXVNA_VAL_SEGMENT_ENABLED_TRUE, RFMXVNA_VAL_SEGMENT_ENABLED_TRUE, RFMXVNA_VAL_SEGMENT_ENABLED_TRUE, RFMXVNA_VAL_SEGMENT_ENABLED_TRUE, RFMXVNA_VAL_SEGMENT_ENABLED_TRUE};
   float64 segmentStartFrequency[NUMBER_OF_SEGMENTS] = { 1.0e9, 5.1e9, 10.1e9, 15.1e9, 20.1e9 };         /* (Hz) */
   float64 segmentStopFrequency[NUMBER_OF_SEGMENTS] = { 5.0e9, 10.0e9, 15.0e9, 20.0e9, 26.5e9 };         /* (Hz) */
   int32 segmentNumberOfFrequencyPoints[NUMBER_OF_SEGMENTS] = {41, 50, 50, 50, 65};
   float64 segmentIFBandwidth[NUMBER_OF_SEGMENTS] = { 100.0e3, 1.0e6, 100.0e3, 10.0e3, 100.0e3 };        /* (Hz) */
   float64 segmentDwellTime[NUMBER_OF_SEGMENTS] = { 0, 0, 0, 0, 0 };
   float64 segmentPort1PowerLevel[NUMBER_OF_SEGMENTS] = { -10.0, -10.0, -10.0, -10.0, -10.0 };           /* (dBm) */
   float64 segmentPort1TestReceiverAttenuation[NUMBER_OF_SEGMENTS] = { 0, 0, 0, 0, 0 };                  /* (dBm) */
   float64 segmentPort2PowerLevel[NUMBER_OF_SEGMENTS] = { -10.0, -10.0, -10.0, -10.0, -10.0 };           /* (dBm) */
   float64 segmentPort2TestReceiverAttenuation[NUMBER_OF_SEGMENTS] = { 0, 0, 0, 0, 0 };                  /* (dBm) */

   int32 triggerType = RFMXVNA_VAL_TRIGGER_TYPE_NONE;
   int32 triggerMode = RFMXVNA_VAL_TRIGGER_MODE_SEGMENT;
   double triggerDelay = 0.0; /*(seconds)*/

   char* sParamsSParameters[NUMBER_OF_SPARAMS] = {"S11", "S12", "S21", "S22"};
   int32 sParamsFormats[NUMBER_OF_SPARAMS] =
   {
       RFMXVNA_VAL_SPARAMS_FORMAT_MAGNITUDE,
       RFMXVNA_VAL_SPARAMS_FORMAT_MAGNITUDE,
       RFMXVNA_VAL_SPARAMS_FORMAT_MAGNITUDE,
       RFMXVNA_VAL_SPARAMS_FORMAT_MAGNITUDE
   };
   char* calibrationPorts = "port1,port2";
   char* vCalResourceName = "vCal";
   char* connectorType = "3.5 mm female";
   float64 calibrationTimeout = 100.0;                                                                  /*(seconds) */
   int32 magnitudeUnits = RFMXVNA_VAL_SPARAMS_MAGNITUDE_UNITS_DB;
   int32 phaseTraceType = RFMXVNA_VAL_SPARAMS_PHASE_TRACE_TYPE_WRAPPED;
   char sParamSelectorString[MAX_SELECTOR_STRING];
   char portSelectorString[MAX_SELECTOR_STRING];
   char segmentSelectorString[MAX_SELECTOR_STRING];

   int32 index = 0;
   float64 timeout = 10.0;                                                                              /*(seconds) */
   int32 actualArraySize = 0;
   int32 numberOfSParamsResult = 0;
   int32 correctionStateResult = 0;
   float64* sParamsXDataResult = NULL;
   float32** sParamsY1DataResult = NULL;
   float32** sParamsY2DataResult = NULL;

   /* Initialize a session */
   RFmxCheckWarn(RFmxVNA_Initialize(resourceName, "", &instrumentHandle, NULL));

   /* Configure the session */
   RFmxCheckWarn(RFmxVNA_CfgFrequencyReference(instrumentHandle, "", RFMXVNA_VAL_PXI_CLK_STR, 100.0e6));
   RFmxCheckWarn(RFmxVNA_SetSweepType(instrumentHandle, "", RFMXVNA_VAL_SWEEP_TYPE_SEGMENT));
   RFmxCheckWarn(RFmxVNA_SetIFBandwidth(instrumentHandle, "", IFBandwidth));
   RFmxCheckWarn(RFmxVNA_BuildPortString("", "port1", MAX_SELECTOR_STRING, portSelectorString));
   RFmxCheckWarn(RFmxVNA_SetPowerLevel(instrumentHandle, portSelectorString, port1PowerLevel));
   RFmxCheckWarn(RFmxVNA_SetTestReceiverAttenuation(instrumentHandle, portSelectorString, port1TestReceiverAttenuation));
   RFmxCheckWarn(RFmxVNA_BuildPortString("", "port2", MAX_SELECTOR_STRING, portSelectorString));
   RFmxCheckWarn(RFmxVNA_SetPowerLevel(instrumentHandle, portSelectorString, port2PowerLevel));
   RFmxCheckWarn(RFmxVNA_SetTestReceiverAttenuation(instrumentHandle, portSelectorString, port2TestReceiverAttenuation));
   RFmxCheckWarn(RFmxVNA_SetNumberOfSegments(instrumentHandle, "", numberOfSegment));
   RFmxCheckWarn(RFmxVNA_SetSegmentPowerLevelEnabled(instrumentHandle, "", segmentPowerLevelEnabled));
   RFmxCheckWarn(RFmxVNA_SetSegmentIFBandwidthEnabled(instrumentHandle, "", segmentIFBandwidthEnabled));
   RFmxCheckWarn(RFmxVNA_SetSegmentTestReceiverAttenuationEnabled(instrumentHandle, "", segmentTestReceiverAttenuationEnabled));
   RFmxCheckWarn(RFmxVNA_SetSegmentDwellTimeEnabled(instrumentHandle, "", segmentDwellTimeEnabled));
    for (index = 0; index < NUMBER_OF_SEGMENTS; index++)
   {
      RFmxCheckWarn(RFmxVNA_BuildSegmentString("", index, MAX_SELECTOR_STRING, segmentSelectorString));
      RFmxCheckWarn(RFmxVNA_SetSegmentEnabled(instrumentHandle, segmentSelectorString, segmentEnabled[index]));
      RFmxCheckWarn(RFmxVNA_SetSegmentStartFrequency(instrumentHandle, segmentSelectorString, segmentStartFrequency[index]));
      RFmxCheckWarn(RFmxVNA_SetSegmentStopFrequency(instrumentHandle, segmentSelectorString, segmentStopFrequency[index]));
      RFmxCheckWarn(RFmxVNA_SetSegmentNumberOfFrequencyPoints(instrumentHandle, segmentSelectorString, segmentNumberOfFrequencyPoints[index]));
      RFmxCheckWarn(RFmxVNA_SetSegmentIFBandwidth(instrumentHandle, segmentSelectorString, segmentIFBandwidth[index]));
      RFmxCheckWarn(RFmxVNA_SetSegmentDwellTime(instrumentHandle, segmentSelectorString, segmentDwellTime[index]));
      RFmxCheckWarn(RFmxVNA_BuildPortString(segmentSelectorString, "port1", MAX_SELECTOR_STRING, portSelectorString));
      RFmxCheckWarn(RFmxVNA_SetSegmentPowerLevel(instrumentHandle, portSelectorString, segmentPort1PowerLevel[index]));
      RFmxCheckWarn(RFmxVNA_SetSegmentTestReceiverAttenuation(instrumentHandle, portSelectorString, segmentPort1TestReceiverAttenuation[index]));
      RFmxCheckWarn(RFmxVNA_BuildPortString(segmentSelectorString, "port2", MAX_SELECTOR_STRING, portSelectorString));
      RFmxCheckWarn(RFmxVNA_SetSegmentPowerLevel(instrumentHandle, portSelectorString, segmentPort2PowerLevel[index]));
      RFmxCheckWarn(RFmxVNA_SetSegmentTestReceiverAttenuation(instrumentHandle, portSelectorString, segmentPort2TestReceiverAttenuation[index]));
   }
   RFmxCheckWarn(RFmxVNA_SetTriggerType(instrumentHandle, "", triggerType));
   RFmxCheckWarn(RFmxVNA_SetTriggerMode(instrumentHandle, "", triggerMode));
   RFmxCheckWarn(RFmxVNA_SetTriggerDelay(instrumentHandle, "", triggerDelay));
   RFmxCheckWarn(RFmxVNA_SelectMeasurements(instrumentHandle, "", RFMXVNA_VAL_SPARAMS, RFMXVNA_VAL_FALSE));
   RFmxCheckWarn(RFmxVNA_SParamsSetNumberOfSParameters(instrumentHandle, "", NUMBER_OF_SPARAMS));
   for (index = 0; index < NUMBER_OF_SPARAMS; index++)
   {
      RFmxCheckWarn(RFmxVNA_BuildSParameterString("", index, MAX_SELECTOR_STRING, sParamSelectorString));
      RFmxCheckWarn(RFmxVNA_SParamsCfgSParameter(instrumentHandle, sParamSelectorString, sParamsSParameters[index]));
      RFmxCheckWarn(RFmxVNA_SParamsSetFormat(instrumentHandle, sParamSelectorString, sParamsFormats[index]));
   }
   RFmxCheckWarn(RFmxVNA_SParamsSetMagnitudeUnits(instrumentHandle, "", magnitudeUnits));
   RFmxCheckWarn(RFmxVNA_SParamsSetPhaseTraceType(instrumentHandle, "", phaseTraceType));
   RFmxCheckWarn(RFmxVNA_SetCorrectionCalibrationPorts(instrumentHandle, "", calibrationPorts));
   RFmxCheckWarn(RFmxVNA_SetCorrectionCalibrationMethod(instrumentHandle, "", RFMXVNA_VAL_CORRECTION_CALIBRATION_METHOD_SOLT));
   RFmxCheckWarn(RFmxVNA_SetCorrectionCalibrationConnectorType(instrumentHandle, "port::all", connectorType));
   RFmxCheckWarn(RFmxVNA_SetCorrectionCalibrationCalkitElectronicResourceName(instrumentHandle, "port::all", vCalResourceName));
   RFmxCheckWarn(RFmxVNA_CalibrationInitiate(instrumentHandle, ""));
   printf("Connect Port A of NI CAL-5501 to Port 1 of NI PXIe-5633,  and Port B of NI CAL-5501 to Port 2 of NI PXIe-5633.\n");
   printf("Press any key to continue.\n");
   _getch();
   RFmxCheckWarn(RFmxVNA_CalibrationAcquire(instrumentHandle, "", calibrationTimeout));
   RFmxCheckWarn(RFmxVNA_CalibrationSave(instrumentHandle, "", ""));
   printf("Connect DUT across port1 and port2 of NI PXIe-5633.\n");
   printf("Press any key to continue.\n");
   _getch();
   RFmxCheckWarn(RFmxVNA_SetCorrectionEnabled(instrumentHandle, "", RFMXVNA_VAL_CORRECTION_ENABLED_TRUE));

   RFmxCheckWarn(RFmxVNA_Initiate(instrumentHandle, "", ""));

   /* Fetch results */
   RFmxCheckWarn(RFmxVNA_SParamsGetNumberOfSParameters(instrumentHandle, "", &numberOfSParamsResult));

    RFmxCheckWarn(RFmxVNA_SParamsFetchXData(instrumentHandle, "", timeout, NULL, 0, &actualArraySize));
    if (actualArraySize > 0)
    {
        sParamsXDataResult = (float64*)malloc(sizeof(float64) * actualArraySize);
        if (sParamsXDataResult)
        {
            RFmxCheckWarn(RFmxVNA_SParamsFetchXData(instrumentHandle, "", timeout, sParamsXDataResult, actualArraySize, NULL));
        }
        else
        {
            printf("malloc failed.\n");
            goto Error;
        }
    }

    sParamsY1DataResult = (float32**)malloc(sizeof(float32*) * numberOfSParamsResult);
    sParamsY2DataResult = (float32**)malloc(sizeof(float32*) * numberOfSParamsResult);
    if (sParamsY1DataResult && sParamsY2DataResult)
    {
        for (index = 0; index < numberOfSParamsResult; index++)
        {
            actualArraySize = 0;
            RFmxCheckWarn(RFmxVNA_BuildSParameterString("", index, MAX_SELECTOR_STRING, sParamSelectorString));
            RFmxCheckWarn(RFmxVNA_SParamsFetchYData(instrumentHandle, sParamSelectorString, timeout, NULL, NULL, 0, &actualArraySize));
            if (actualArraySize > 0)
            {
                sParamsY1DataResult[index] = (float32*)malloc(sizeof(float32) * actualArraySize);
                sParamsY2DataResult[index] = (float32*)malloc(sizeof(float32) * actualArraySize);
                if (sParamsY1DataResult[index] && sParamsY2DataResult[index])
                {
                    RFmxCheckWarn(RFmxVNA_SParamsFetchYData(instrumentHandle, sParamSelectorString, timeout, sParamsY1DataResult[index], sParamsY2DataResult[index], actualArraySize, NULL));
                }
                else
                {
                    printf("malloc failed.\n");
                    goto Error;
                }
            }
        }
    }
   else
   {
      printf("malloc failed.\n");
      goto Error;
   }
   RFmxCheckWarn(RFmxVNA_SParamsGetResultsCorrectionState(instrumentHandle, "", &correctionStateResult));

Error:
   if (error)
   {
      RFmxVNA_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
      if (error < 0)
         printf("ERROR: %s\n", errorMessage);
      else
         printf("WARNING: %s %d\n", errorMessage, error);
   }

   if (instrumentHandle)
   {
      RFmxVNA_Close(instrumentHandle, RFMXVNA_VAL_FALSE);
   }

    /* Free allocated memory */
    if (sParamsXDataResult)
    {
        free(sParamsXDataResult);
    }
    for (index = 0; index < numberOfSParamsResult; index++)
    {
        if (sParamsY1DataResult[index])
        {
            free(sParamsY1DataResult[index]);
        }
        if (sParamsY2DataResult[index])
        {
            free(sParamsY2DataResult[index]);
        }
    }
    if (sParamsY1DataResult)
    {
        free(sParamsY1DataResult);
    }
    if (sParamsY2DataResult)
    {
        free(sParamsY2DataResult);
    }

   printf("Press any key to exit\n");
   _getch();

   return error;
}