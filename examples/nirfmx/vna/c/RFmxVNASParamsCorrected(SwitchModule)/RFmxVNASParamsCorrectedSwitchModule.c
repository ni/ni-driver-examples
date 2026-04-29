//Steps:
//1. Open a new RFmx session.
//2. Configure Frequency Reference.
/*3. Configure sweep settings: Sweep Type, Start Frequency, Stop Frequency, Number of Frequency Points,
     IF Bandwidth, and Power Level and Test Rx Attenuation with different port names.*/
//4. Configure Averaging.
//5. Select S-Parameter measurement.
//6. Configure number of S-Parameters.
//7. Configure each S-Parameter and format.
//8. Configure Magnitude Units & Phase Trace Type.
//9. Configure Calibration Ports and Calibration Method
//10. Configure Connector type & vCal Resource Name for each VNA port
//11. Initiate Calibration
//12. Read the Calstep Description for connection information.
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
/* Maximum size of calstep description*/
#define MAX_CALSTEP_DESCRIPTION              2048

int main(int argc, char *argv[])
{
   char *resourceName = "VNA";
   niRFmxInstrHandle instrumentHandle = NULL;
   char errorMessage[MAX_ERROR_DESCRIPTION] = { 0 };
   int32 error = 0;
   int32 lastErrorCode = 0;
   float64 frequencyStart = 1.0e9;                                                                       /* (Hz) */
   float64 frequencyEnd = 26.0e9;                                                                        /* (Hz) */
   int32 numberOfFrequencyPoints = 251;
   char* measurementPorts = "rmm0/port0,rmm0/port1";
   int32 measurementPortSize = 2;
   char* portNames[] = {"rmm0/port0", "rmm0/port1"};
   float64 portPowerLevelArray[] = {-10.0, -10.0}; /* (dBm) */
   float64 portTestReceiverAttenuationArray[] = {0.0, 0.0}; /* (dB) */
   float64 IFBandwidth = 100.0e3;                                                                        /* (Hz) */

   char *frequencyReferenceSource = RFMXVNA_VAL_PXI_CLK_STR;
   float64 frequencyReferenceFrequency = 100.0e6;                                                        /* (Hz) */

   char* sParamsReceiverPort[NUMBER_OF_SPARAMS] = {"rmm0/port0", "rmm0/port0", "rmm0/port1", "rmm0/port1"};
   char* sParamsSourcePort[NUMBER_OF_SPARAMS] = {"rmm0/port0", "rmm0/port1", "rmm0/port0", "rmm0/port1"};

   int32 sParamsFormats[NUMBER_OF_SPARAMS] =
   {
      RFMXVNA_VAL_SPARAMS_FORMAT_MAGNITUDE,
      RFMXVNA_VAL_SPARAMS_FORMAT_MAGNITUDE,
      RFMXVNA_VAL_SPARAMS_FORMAT_MAGNITUDE,
      RFMXVNA_VAL_SPARAMS_FORMAT_MAGNITUDE
   };

   char *calibrationPorts = "rmm0/port0,rmm0/port1";
   char* vCalOrientation = "PortA:rmm0/port0,PortB:rmm0/port1";
   char *vCalResourceName = "vCal";
   char *connectorType = "3.5 mm female";
   char connectionInstruction[MAX_ERROR_DESCRIPTION];
   float64 calibrationTimeout = 100.0;                                                                   /*(seconds) */

   int32 magnitudeUnits = RFMXVNA_VAL_SPARAMS_MAGNITUDE_UNITS_DB;
   int32 phaseTraceType = RFMXVNA_VAL_SPARAMS_PHASE_TRACE_TYPE_WRAPPED;
   int32 averagingEnabled = RFMXVNA_VAL_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;
   char sParamSelectorString[MAX_SELECTOR_STRING];
   char portSelectorString[MAX_SELECTOR_STRING];

   int32 index = 0;
   float64 timeout = 10.0;                                                                               /*(seconds) */

   int32 actualArraySize = 0;
   int32 numberOfSParamsResult = 0;
   int32 correctionStateResult = 0;
   float64* sParamsXDataResult = NULL;
   float32** sParamsY1DataResult = NULL;
   float32** sParamsY2DataResult = NULL;

   /* Initialize a session */
   RFmxCheckWarn(RFmxVNA_Initialize(resourceName, "", &instrumentHandle, NULL));

   /* Configure the session */
   RFmxCheckWarn(RFmxVNA_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, frequencyReferenceFrequency));
   RFmxCheckWarn(RFmxVNA_SetSweepType(instrumentHandle, "", RFMXVNA_VAL_SWEEP_TYPE_LINEAR));
   RFmxCheckWarn(RFmxVNA_SetStartFrequency(instrumentHandle, "", frequencyStart));
   RFmxCheckWarn(RFmxVNA_SetStopFrequency(instrumentHandle, "", frequencyEnd));
   RFmxCheckWarn(RFmxVNA_SetNumberOfPoints(instrumentHandle, "", numberOfFrequencyPoints));
   RFmxCheckWarn(RFmxVNA_SetIFBandwidth(instrumentHandle, "", IFBandwidth));
   for (index = 0; index < measurementPortSize; index++) {
       RFmxCheckWarn(RFmxVNA_BuildPortString("", portNames[index], MAX_SELECTOR_STRING, portSelectorString));
       RFmxCheckWarn(RFmxVNA_SetPowerLevel(instrumentHandle, portSelectorString, portPowerLevelArray[index]));
       RFmxCheckWarn(RFmxVNA_SetTestReceiverAttenuation(instrumentHandle, portSelectorString, portTestReceiverAttenuationArray[index]));
   }

   RFmxCheckWarn(RFmxVNA_SetAveragingEnabled(instrumentHandle, "", averagingEnabled));
   RFmxCheckWarn(RFmxVNA_SetAveragingCount(instrumentHandle, "", averagingCount));

   RFmxCheckWarn(RFmxVNA_SelectMeasurements(instrumentHandle, "", RFMXVNA_VAL_SPARAMS, RFMXVNA_VAL_FALSE));

   RFmxCheckWarn(RFmxVNA_SParamsSetNumberOfSParameters(instrumentHandle, "", NUMBER_OF_SPARAMS));
   for (index = 0; index < NUMBER_OF_SPARAMS; index++)
   {
      RFmxCheckWarn(RFmxVNA_BuildSParameterString("", index, MAX_SELECTOR_STRING, sParamSelectorString));
      RFmxCheckWarn(RFmxVNA_SParamsSetReceiverPort(instrumentHandle, sParamSelectorString, sParamsReceiverPort[index]));
      RFmxCheckWarn(RFmxVNA_SParamsSetSourcePort(instrumentHandle, sParamSelectorString, sParamsSourcePort[index]));
      RFmxCheckWarn(RFmxVNA_SParamsSetFormat(instrumentHandle, sParamSelectorString, sParamsFormats[index]));
   }

   RFmxCheckWarn(RFmxVNA_SParamsSetMagnitudeUnits(instrumentHandle, "", magnitudeUnits));
   RFmxCheckWarn(RFmxVNA_SParamsSetPhaseTraceType(instrumentHandle, "", phaseTraceType));

   RFmxCheckWarn(RFmxVNA_SetCorrectionCalibrationPorts(instrumentHandle, "", calibrationPorts));
   RFmxCheckWarn(RFmxVNA_SetCorrectionCalibrationMethod(instrumentHandle, "", RFMXVNA_VAL_CORRECTION_CALIBRATION_METHOD_SOLT));
   RFmxCheckWarn(RFmxVNA_SetCorrectionCalibrationCalkitElectronicOrientation(instrumentHandle, "", vCalOrientation));

   RFmxCheckWarn(RFmxVNA_SetCorrectionCalibrationConnectorType(instrumentHandle, "port::all", connectorType));
   RFmxCheckWarn(RFmxVNA_SetCorrectionCalibrationCalkitElectronicResourceName(instrumentHandle, "port::all", vCalResourceName));

   RFmxCheckWarn(RFmxVNA_CalibrationInitiate(instrumentHandle, ""));
   RFmxCheckWarn(RFmxVNA_GetCorrectionCalibrationStepDescription(instrumentHandle, "", MAX_CALSTEP_DESCRIPTION, connectionInstruction));
   printf("Calstep Description: %s\n", connectionInstruction);
   printf("Press any key to continue.\n");
   _getch();
   RFmxCheckWarn(RFmxVNA_CalibrationAcquire(instrumentHandle, "", calibrationTimeout));
   RFmxCheckWarn(RFmxVNA_CalibrationSave(instrumentHandle, "", ""));
   printf("Connect DUT across the specified measurement ports.\n");
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
   RFmxVNA_SParamsGetResultsCorrectionState(instrumentHandle, "", &correctionStateResult);
   printf("Correction State: %d\n", correctionStateResult);
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