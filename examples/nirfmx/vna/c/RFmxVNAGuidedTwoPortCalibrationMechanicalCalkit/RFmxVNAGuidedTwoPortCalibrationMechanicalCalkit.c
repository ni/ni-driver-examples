//Steps:
//1. Open a new RFmx session.
//2. Configure Frequency Reference.
//3. Configure sweep settings: Sweep Type, Start Frequency, Stop Frequency, Number of Frequency Points, IF Bandwidth, and Power Level and Test Rx Attenuation with different port names.
//4. Select S-Parameter measurement. 
//5. Configure Calibration Ports, Calibration Method and Thru.
//6. Import Calkit File
//7. Configure Connector type & Mechanical Calkit Name for each VNA port.
//8. Initiate Calibration.
//9. Acquire Calibration data after user confirmation.
//10. Save Calibration data.
//11. Save Calset data to a file.
//12. Get Calset Frequency Grid.
//13. Get Calset Error Terms.
//13a. Calculate Magnitude of Error Terms
//14. Close RFmx Session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include <math.h>

#include "niRFmxVNA.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION                4096
/* Maximum size of a selector string */
#define MAX_SELECTOR_STRING                  256

int main(int argc, char *argv[])
{
   char *resourceName = "VNA";
   niRFmxInstrHandle instrumentHandle = NULL;

   char errorMessage[MAX_ERROR_DESCRIPTION] = { 0 };
   int32 error = 0;
   int32 lastErrorCode = 0;

   int32 sweepType = RFMXVNA_VAL_SWEEP_TYPE_LINEAR;
   float64 startFrequency = 1.0e9;                                                                       /* (Hz) */
   float64 stopFrequency = 26.0e9;                                                                       /* (Hz) */
   int32 numberOfFrequencyPoints = 251;
   float64 port1PowerLevel = -10.0;                                                                      /* (dBm) */
   float64 port2PowerLevel = -10.0;                                                                      /* (dBm) */
   float64 port1TestReceiverAttenuation = 0.0;                                                           /* (dB) */
   float64 port2TestReceiverAttenuation = 0.0;                                                           /* (dB) */
   float64 IFBandwidth = 100.0e3;                                                                        /* (Hz) */

   char *frequencyReferenceSource = RFMXVNA_VAL_PXI_CLK_STR;
   float64 frequencyReferenceFrequency = 100.0e6;                                                        /* (Hz) */

   char* calkitFilePath = "";
   int32 calkitType = RFMXVNA_VAL_CORRECTION_CALIBRATION_CALKIT_TYPE_MECHANICAL;
   char* calsetFilePath = "..\\Support\\Calset_SOLT_Port1_Port2.ncst";

   char* calkitName = "";
   char *connectorType = "3.5 mm female";
   char *calibrationPorts = "port1,port2";
   int32 calMethod = RFMXVNA_VAL_CORRECTION_CALIBRATION_METHOD_SOLT;
   int32 thruMethod = RFMXVNA_VAL_CORRECTION_CALIBRATION_THRU_METHOD_AUTO;
   static float64 zero = 0.0;
   float64 thruCoaxDelay = zero/zero;                                                                    /*(seconds) */
   float64 calibrationTimeout = 100.0;                                                                   /*(seconds) */

   int32 errorTermIdentifier[5] = {RFMXVNA_VAL_CAL_ERROR_TERM_DIRECTIVITY, RFMXVNA_VAL_CAL_ERROR_TERM_SOURCE_MATCH, RFMXVNA_VAL_CAL_ERROR_TERM_REFLECTION_TRACKING, RFMXVNA_VAL_CAL_ERROR_TERM_TRANSMISSION_TRACKING, RFMXVNA_VAL_CAL_ERROR_TERM_LOAD_MATCH};

   char sParamSelectorString[MAX_SELECTOR_STRING];
   char calstepSelectorString[MAX_SELECTOR_STRING];
   char portSelectorString[MAX_SELECTOR_STRING];
   char calstepDescription[MAX_SELECTOR_STRING];

   int32* calStepCount = 0;
   float64* calEstThruDelay = NULL;
   float64* frequencyGrid = NULL;

   NIComplexSingle* errorTerms = NULL;
   float64 calculateMagnitude = 0.0;

   int32 index = 0;

   int32 actualArraySize = 0;

   /* Initialize a session */
   RFmxCheckWarn(RFmxVNA_Initialize(resourceName, "", &instrumentHandle, NULL));

   /* Configure the session */
   RFmxCheckWarn(RFmxVNA_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource,
      frequencyReferenceFrequency));
   RFmxCheckWarn(RFmxVNA_SetSweepType(instrumentHandle, "", sweepType));
   RFmxCheckWarn(RFmxVNA_SetStartFrequency(instrumentHandle, "", startFrequency));
   RFmxCheckWarn(RFmxVNA_SetStopFrequency(instrumentHandle, "", stopFrequency));
   RFmxCheckWarn(RFmxVNA_SetNumberOfPoints(instrumentHandle, "", numberOfFrequencyPoints));
   RFmxCheckWarn(RFmxVNA_SetIFBandwidth(instrumentHandle, "", IFBandwidth));
   RFmxCheckWarn(RFmxVNA_BuildPortString("", "port1", MAX_SELECTOR_STRING, portSelectorString));
   RFmxCheckWarn(RFmxVNA_SetPowerLevel(instrumentHandle, portSelectorString, port1PowerLevel));
   RFmxCheckWarn(RFmxVNA_SetTestReceiverAttenuation(instrumentHandle, portSelectorString, port1TestReceiverAttenuation));
   RFmxCheckWarn(RFmxVNA_BuildPortString("", "port2", MAX_SELECTOR_STRING, portSelectorString));
   RFmxCheckWarn(RFmxVNA_SetPowerLevel(instrumentHandle, portSelectorString, port2PowerLevel));
   RFmxCheckWarn(RFmxVNA_SetTestReceiverAttenuation(instrumentHandle, portSelectorString, port2TestReceiverAttenuation));
   RFmxCheckWarn(RFmxVNA_SelectMeasurements(instrumentHandle, "", RFMXVNA_VAL_SPARAMS, RFMXVNA_VAL_FALSE));
   RFmxCheckWarn(RFmxVNA_SetCorrectionCalibrationPorts(instrumentHandle, "", calibrationPorts));
   RFmxCheckWarn(RFmxVNA_SetCorrectionCalibrationMethod(instrumentHandle, "", calMethod));
   RFmxCheckWarn(RFmxVNA_SetCorrectionCalibrationThruMethod(instrumentHandle, "", thruMethod));
   if (!isnan(thruCoaxDelay))
   {
       RFmxCheckWarn(RFmxVNA_SetCorrectionCalibrationThruCoaxDelay(instrumentHandle, "", thruCoaxDelay));
   }
   RFmxCheckWarn(RFmxVNA_CalkitManagerImportCalkit(instrumentHandle, "", calkitFilePath));
   RFmxCheckWarn(RFmxVNA_SetCorrectionCalibrationCalkitType(instrumentHandle, "port::all", RFMXVNA_VAL_CORRECTION_CALIBRATION_CALKIT_TYPE_MECHANICAL));
   RFmxCheckWarn(RFmxVNA_SetCorrectionCalibrationConnectorType(instrumentHandle, "port::all", connectorType));
   RFmxCheckWarn(RFmxVNA_SetCorrectionCalibrationCalkitMechanicalName(instrumentHandle, "port::all", calkitName));

   RFmxCheckWarn(RFmxVNA_CalibrationInitiate(instrumentHandle, ""));

   RFmxCheckWarn(RFmxVNA_GetCorrectionCalibrationStepCount(instrumentHandle, "", &calStepCount));
   for (index = 0; index < calStepCount; index++)
   {
       RFmxCheckWarn(RFmxVNA_BuildCalstepString("", index, MAX_SELECTOR_STRING, calstepSelectorString));
       RFmxCheckWarn(RFmxVNA_GetCorrectionCalibrationStepDescription(instrumentHandle, calstepSelectorString, MAX_SELECTOR_STRING, calstepDescription));
       printf(calstepDescription);
       printf("Press any key to continue.\n");
       _getch();
       RFmxCheckWarn(RFmxVNA_CalibrationAcquire(instrumentHandle, calstepSelectorString, calibrationTimeout));
   }
   RFmxCheckWarn(RFmxVNA_CalibrationSave(instrumentHandle, "", ""));
   RFmxCheckWarn(RFmxVNA_GetCorrectionCalibrationEstimatedThruDelay(instrumentHandle, "", &calEstThruDelay));
   RFmxCheckWarn(RFmxVNA_CalsetSaveToFile(instrumentHandle, "", "", calsetFilePath));
   RFmxCheckWarn(RFmxVNA_CalsetGetFrequencyGrid(instrumentHandle, "", "", errorTermIdentifier[0], NULL, 0, &actualArraySize));
   if (actualArraySize > 0)
   {
       frequencyGrid = (float64*)malloc(sizeof(float64) * actualArraySize);
       if (frequencyGrid)
       {
           RFmxCheckWarn(RFmxVNA_CalsetGetFrequencyGrid(instrumentHandle, "", "", RFMXVNA_VAL_CAL_ERROR_TERM_DIRECTIVITY, frequencyGrid, actualArraySize, NULL));
       }
       else
       {
           printf("malloc failed.\n");
           goto Error;
       }
   }
   
   for (index = 0; index < sizeof(errorTermIdentifier); index++)
   {
       RFmxCheckWarn(RFmxVNA_CalsetGetErrorTerm(instrumentHandle, "", "", errorTermIdentifier[index], "port1", "port2", NULL, 0, &actualArraySize));
       if (actualArraySize > 0)
       {
            errorTerms = (NIComplexSingle*)malloc(sizeof(NIComplexSingle) * actualArraySize);
            if (errorTerms)
            {
                RFmxCheckWarn(RFmxVNA_CalsetGetErrorTerm(instrumentHandle, "", "", errorTermIdentifier[index], "port1", "port2", errorTerms, actualArraySize, NULL));
                calculateMagnitude = (20.0 * log10(sqrt(errorTerms->real * errorTerms->real + errorTerms->imaginary * errorTerms->imaginary)));
            }
            else
            {
                printf("malloc failed.\n");
                goto Error;
            }
       }
       RFmxCheckWarn(RFmxVNA_CalsetGetErrorTerm(instrumentHandle, "", "", errorTermIdentifier[index], "port2", "port1", NULL, 0, &actualArraySize));
       if (actualArraySize > 0)
       {
            errorTerms = (NIComplexSingle*)malloc(sizeof(NIComplexSingle) * actualArraySize);
            if (errorTerms)
            {
                RFmxCheckWarn(RFmxVNA_CalsetGetErrorTerm(instrumentHandle, "", "", errorTermIdentifier[index], "port2", "port1", errorTerms, actualArraySize, NULL));
                calculateMagnitude = (20.0 * log10(sqrt(errorTerms->real * errorTerms->real + errorTerms->imaginary * errorTerms->imaginary)));
            }
            else
            {
                printf("malloc failed.\n");
                goto Error;
            }
       }
   }

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
   if (frequencyGrid)
   {
        free(frequencyGrid);
   }
   if (errorTerms)
   {
        free(errorTerms);
   }
   printf("Calibration Estimated Thru Delay : %f", calEstThruDelay);
   printf("Press any key to exit\n");
   _getch();

   return error;
}