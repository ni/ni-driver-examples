//Steps:
//1. Open a new RFmx session.
//2. Configure Frequency Reference.
//3. Configure sweep settings: Sweep Type, Start Frequency, Stop Frequency, Number of Frequency Points, IF Bandwidth, and Power Level with different port names.
//4. Configure Pulse Settings.
//5. Configure Pulse Generator Settings.
//6. Configure Averaging,
//7. Select S-Parameter measurement.
//8. Configure number of S-Parameters.
//9. Configure each S-Parameter and format.
//10. Initiate the Measurement.
//11. Read Number of SParams.
//12. Fetch S-Parameter X data.
//13. Fetch S-Parameter Y data for each S-Parameter.
//14. Close RFmx Session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxVNA.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION                4096
/* Maximum size of a selector string */
#define MAX_SELECTOR_STRING                  256
#define FREQUENCY_LIST_SIZE                  10
#define NUMBER_OF_SPARAMS                    1
#define NUMBER_OF_PULSE_GENERATORS           4

int main(int argc, char *argv[])
{
   char *resourceName = "VNA";
   niRFmxInstrHandle instrumentHandle = NULL;

   char errorMessage[MAX_ERROR_DESCRIPTION] = { 0 };
   int32 error = 0;
   int32 lastErrorCode = 0;

   char* frequencyReferenceSource = RFMXVNA_VAL_PXI_CLK_STR;
   float64 frequencyReferenceFrequency = 100.0e6;                                                        /* (Hz) */

   float64 startFrequency = 1.0e9;                                                                       /* (Hz) */
   float64 stopFrequency = 1.0e9;                                                                        /* (Hz) */
   int32 numberOfFrequencyPoints = 1;
   float64 port1PowerLevel = -10.0;                                                                      /* (dBm) */
   float64 port2PowerLevel = -10.0;                                                                      /* (dBm) */

   int32 pulseModeEnabled = RFMXVNA_VAL_PULSE_MODE_ENABLED_TRUE;
   float64 pulsePeriod = 1.0e-3;                                                                         /* (seconds) */
   float64 pulseModulatorDealy = 0;                                                                      /* (seconds) */
   float64 pulseModulatorWidth = 100.0e-6;                                                               /* (seconds) */
   int32 pulseAcquistionAuto = RFMXVNA_VAL_PULSE_ACQUISITION_AUTO_TRUE;
   float64 pulseAcquisitionWidth = 46.65e-6;                                                             /* (seconds) */
   float64 pulseAcquisitionDealy = 20.0e-6;                                                              /* (seconds) */

   int32 pulseGeneratorEnabled[NUMBER_OF_PULSE_GENERATORS] = {
       RFMXVNA_VAL_PULSE_GENERATOR_ENABLED_TRUE,
       RFMXVNA_VAL_PULSE_GENERATOR_ENABLED_FALSE,
       RFMXVNA_VAL_PULSE_GENERATOR_ENABLED_FALSE,
       RFMXVNA_VAL_PULSE_GENERATOR_ENABLED_FALSE};
   char* pulseGeneratorExportOutputTerminal[NUMBER_OF_PULSE_GENERATORS] = {
       RFMXINSTR_VAL_PFI0_STR,
       RFMXINSTR_VAL_DO_NOT_EXPORT_STR,
       RFMXINSTR_VAL_DO_NOT_EXPORT_STR,
       RFMXINSTR_VAL_DO_NOT_EXPORT_STR};
   float64 pulseGeneratorDelay[NUMBER_OF_PULSE_GENERATORS] = {0,0,0,0};                                  /* (seconds) */
   float64 pulseGeneratorWidth[NUMBER_OF_PULSE_GENERATORS] = {100.e-6, 100.e-6, 100.e-6, 100.e-6};       /* (seconds) */

   int32 averagingEnabled = RFMXVNA_VAL_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;

   char* sParamsSParameters[NUMBER_OF_SPARAMS] = {"S11"};
   int32 sParamsFormats[NUMBER_OF_SPARAMS] =
   {
      RFMXVNA_VAL_SPARAMS_FORMAT_MAGNITUDE
   };

   char sParamSelectorString[MAX_SELECTOR_STRING];
   char portSelectorString[MAX_SELECTOR_STRING];
   char pulseGenSelectorString[MAX_SELECTOR_STRING];

   int32 index = 0;

   float64 timeout = 10.0;                                                                               /*(seconds) */

   int32 actualArraySize = 0;
   int32 numberOfSParamsResult = 0;
   float64* sParamsXDataResult = NULL;
   float32** sParamsY1DataResult = NULL;
   float32** sParamsY2DataResult = NULL;

   /* Initialize a session */
   RFmxCheckWarn(RFmxVNA_Initialize(resourceName, "", &instrumentHandle, NULL));

   /* Configure the session */
   RFmxCheckWarn(RFmxVNA_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource,
      frequencyReferenceFrequency));
   RFmxCheckWarn(RFmxVNA_SetSweepType(instrumentHandle, "", RFMXVNA_VAL_SWEEP_TYPE_LINEAR));
   RFmxCheckWarn(RFmxVNA_SetStartFrequency(instrumentHandle, "", startFrequency));
   RFmxCheckWarn(RFmxVNA_SetStopFrequency(instrumentHandle, "", stopFrequency));
   RFmxCheckWarn(RFmxVNA_SetNumberOfPoints(instrumentHandle, "", numberOfFrequencyPoints));
   RFmxCheckWarn(RFmxVNA_BuildPortString("", "port1", MAX_SELECTOR_STRING, portSelectorString));
   RFmxCheckWarn(RFmxVNA_SetPowerLevel(instrumentHandle, portSelectorString, port1PowerLevel));
   RFmxCheckWarn(RFmxVNA_BuildPortString("", "port2", MAX_SELECTOR_STRING, portSelectorString));
   RFmxCheckWarn(RFmxVNA_SetPowerLevel(instrumentHandle, portSelectorString, port2PowerLevel));

   RFmxCheckWarn(RFmxVNA_SetPulseModeEnabled(instrumentHandle, "", pulseModeEnabled));
   RFmxCheckWarn(RFmxVNA_SetPulsePeriod(instrumentHandle, "", pulsePeriod));
   RFmxCheckWarn(RFmxVNA_SetPulseModulatorDelay(instrumentHandle, "", pulseModulatorDealy));
   RFmxCheckWarn(RFmxVNA_SetPulseModulatorWidth(instrumentHandle, "", pulseModulatorWidth));
   RFmxCheckWarn(RFmxVNA_SetPulseAcquisitionAuto(instrumentHandle, "", pulseAcquistionAuto));
   RFmxCheckWarn(RFmxVNA_SetPulseAcquisitionWidth(instrumentHandle, "", pulseAcquisitionWidth));
   RFmxCheckWarn(RFmxVNA_SetPulseAcquisitionDelay(instrumentHandle, "", pulseAcquisitionDealy));
   for (index = 0; index < NUMBER_OF_PULSE_GENERATORS; index++) {
       RFmxCheckWarn(RFmxVNA_BuildPulseGeneratorString("", index, MAX_SELECTOR_STRING, pulseGenSelectorString));
       RFmxCheckWarn(RFmxVNA_SetPulseGeneratorEnabled(instrumentHandle, pulseGenSelectorString, pulseGeneratorEnabled[index]));
       RFmxCheckWarn(RFmxVNA_SetPulseGeneratorExportOutputTerminal(instrumentHandle, pulseGenSelectorString, pulseGeneratorExportOutputTerminal[index]));
       RFmxCheckWarn(RFmxVNA_SetPulseGeneratorDelay(instrumentHandle, pulseGenSelectorString, pulseGeneratorDelay[index]));
       RFmxCheckWarn(RFmxVNA_SetPulseGeneratorWidth(instrumentHandle, pulseGenSelectorString, pulseGeneratorWidth[index]));
   }
   RFmxCheckWarn(RFmxVNA_SetAveragingEnabled(instrumentHandle, "", averagingEnabled));
   RFmxCheckWarn(RFmxVNA_SetAveragingCount(instrumentHandle, "", averagingCount));
   RFmxCheckWarn(RFmxVNA_SelectMeasurements(instrumentHandle, "", RFMXVNA_VAL_SPARAMS, RFMXVNA_VAL_FALSE));
   RFmxCheckWarn(RFmxVNA_SParamsSetNumberOfSParameters(instrumentHandle, "", NUMBER_OF_SPARAMS));
   for (index = 0; index < NUMBER_OF_SPARAMS; index++)
   {
      RFmxCheckWarn(RFmxVNA_BuildSParameterString("", index, MAX_SELECTOR_STRING, sParamSelectorString));
      RFmxCheckWarn(RFmxVNA_SParamsCfgSParameter(instrumentHandle, sParamSelectorString, sParamsSParameters[index]));
      RFmxCheckWarn(RFmxVNA_SParamsSetFormat(instrumentHandle, sParamSelectorString, sParamsFormats[index]));
   }
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