//Steps:
//1. Open a new RFmx session.
//2. Configure Frequency Reference.
//3. Create a named signal instance.
//4 . Configure sweep settings: Sweep Type, Start Frequency, Stop Frequency, Number of Frequency Points, IF Bandwidth, and Power Level and Test Rx Attenuation with different port names.
//5. Select S-Parameter measurement.
//6. Configure number of S-Parameters.
//7. Configure each S-Parameter and format.
//8. Configure Magnitude Units & Phase Trace Type.
//9. Load one or more Calset data from files(s) to create a global pool of named calsets.
//10. Select a named calset from the global pool to set as active calset for the specified signal. 
//11. Enable Correction
//12. Initiate the Measurement after user confirmation
//13. Read Number of SParams and X-Axis Values (aggregated frequency list).
//14. Fetch S-Parameter X data. 
//15. Fetch S-Parameter Y data for each S-Parameter.
//16. Close RFmx Session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxVNA.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION                4096
/* Maximum size of a selector string */
#define MAX_SELECTOR_STRING                  256
#define SIGNAL_NAME_LEN                      50
#define NUMBER_OF_SPARAMS                    4
#define NUMBER_OF_CALSETS                    3


int main(int argc, char *argv[])
{
   char *resourceName = "VNA";
   niRFmxInstrHandle instrumentHandle = NULL;
   char vnaSignal1[MAX_SELECTOR_STRING];

   char errorMessage[MAX_ERROR_DESCRIPTION] = { 0 };
   int32 error = 0;
   int32 lastErrorCode = 0;

   int32 index = 0;

   float64 frequencyStart = 1.0e9;                                                                       /* (Hz) */
   float64 frequencyEnd = 26.0e9;                                                                        /* (Hz) */
   int32 numberOfFrequencyPoints = 251;
   float64 port1PowerLevel = -10.0;                                                                      /* (dBm) */
   float64 port2PowerLevel = -10.0;                                                                      /* (dBm) */
   float64 port1TestReceiverAttenuation = 0.0;                                                           /* (dB) */
   float64 port2TestReceiverAttenuation = 0.0;                                                           /* (dB) */
   float64 IFBandwidth = 100.0e3;                                                                        /* (Hz) */

   char *frequencyReferenceSource = RFMXVNA_VAL_PXI_CLK_STR;
   float64 frequencyReferenceFrequency = 100.0e6;                                                        /* (Hz) */

   char* sParamsSParameters[NUMBER_OF_SPARAMS] = {"S11", "S12", "S21", "S22"};
   int32 sParamsFormats[NUMBER_OF_SPARAMS] =
   {
      RFMXVNA_VAL_SPARAMS_FORMAT_MAGNITUDE,
      RFMXVNA_VAL_SPARAMS_FORMAT_MAGNITUDE,
      RFMXVNA_VAL_SPARAMS_FORMAT_MAGNITUDE,
      RFMXVNA_VAL_SPARAMS_FORMAT_MAGNITUDE
   };

   int32 magnitudeUnits = RFMXVNA_VAL_SPARAMS_MAGNITUDE_UNITS_DB;
   int32 phaseTraceType = RFMXVNA_VAL_SPARAMS_PHASE_TRACE_TYPE_WRAPPED;

   char* calsetFilePath[NUMBER_OF_CALSETS] = {NULL, NULL, NULL};
   char* calsetName[NUMBER_OF_CALSETS] = {"Calset1", "Calset2", "Calset3"};

   char sParamSelectorString[MAX_SELECTOR_STRING];
   char portSelectorString[MAX_SELECTOR_STRING];

   float64 timeout = 10.0;                                                                               /*(seconds) */

   int32 actualArraySize = 0;
   int32 numberOfSParamsResult = 0;
   float64* sParamsXDataResult = NULL;
   float32** sParamsY1DataResult = NULL;
   float32** sParamsY2DataResult = NULL;

   /* Initialize a session */
   RFmxCheckWarn(RFmxVNA_Initialize(resourceName, "", &instrumentHandle, NULL));

   /* Configure the session */
   RFmxCheckWarn(RFmxVNA_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, frequencyReferenceFrequency));
   RFmxCheckWarn(RFmxVNA_BuildSignalString("Signal1", "", MAX_SELECTOR_STRING, vnaSignal1));
   RFmxCheckWarn(RFmxVNA_CreateSignalConfiguration(instrumentHandle, vnaSignal1));
   RFmxCheckWarn(RFmxVNA_SetSweepType(instrumentHandle, vnaSignal1, RFMXVNA_VAL_SWEEP_TYPE_LINEAR));
   RFmxCheckWarn(RFmxVNA_SetStartFrequency(instrumentHandle, vnaSignal1, frequencyStart));
   RFmxCheckWarn(RFmxVNA_SetStopFrequency(instrumentHandle, vnaSignal1, frequencyEnd));
   RFmxCheckWarn(RFmxVNA_SetNumberOfPoints(instrumentHandle, vnaSignal1, numberOfFrequencyPoints));
   RFmxCheckWarn(RFmxVNA_SetIFBandwidth(instrumentHandle, vnaSignal1, IFBandwidth));
   RFmxCheckWarn(RFmxVNA_BuildPortString(vnaSignal1, "port1", MAX_SELECTOR_STRING, portSelectorString));
   RFmxCheckWarn(RFmxVNA_SetPowerLevel(instrumentHandle, portSelectorString, port1PowerLevel));
   RFmxCheckWarn(RFmxVNA_SetTestReceiverAttenuation(instrumentHandle, portSelectorString, port1TestReceiverAttenuation));
   RFmxCheckWarn(RFmxVNA_BuildPortString(vnaSignal1, "port2", MAX_SELECTOR_STRING, portSelectorString));
   RFmxCheckWarn(RFmxVNA_SetPowerLevel(instrumentHandle, portSelectorString, port2PowerLevel));
   RFmxCheckWarn(RFmxVNA_SetTestReceiverAttenuation(instrumentHandle, portSelectorString, port2TestReceiverAttenuation));

   RFmxCheckWarn(RFmxVNA_SelectMeasurements(instrumentHandle, vnaSignal1, RFMXVNA_VAL_SPARAMS, RFMXVNA_VAL_FALSE));

   RFmxCheckWarn(RFmxVNA_SParamsSetNumberOfSParameters(instrumentHandle, vnaSignal1, NUMBER_OF_SPARAMS));
   for (index = 0; index < NUMBER_OF_SPARAMS; index++)
   {
       RFmxCheckWarn(RFmxVNA_BuildSParameterString(vnaSignal1, index, MAX_SELECTOR_STRING, sParamSelectorString));
      RFmxCheckWarn(RFmxVNA_SParamsCfgSParameter(instrumentHandle, sParamSelectorString, sParamsSParameters[index]));
      RFmxCheckWarn(RFmxVNA_SParamsSetFormat(instrumentHandle, sParamSelectorString, sParamsFormats[index]));
   }

   RFmxCheckWarn(RFmxVNA_SParamsSetMagnitudeUnits(instrumentHandle, vnaSignal1, magnitudeUnits));
   RFmxCheckWarn(RFmxVNA_SParamsSetPhaseTraceType(instrumentHandle, vnaSignal1, phaseTraceType));

   for (index = 0; index < NUMBER_OF_CALSETS; index++) {
       RFmxCheckWarn(RFmxVNA_CalsetLoadFromFile(instrumentHandle, "", calsetName[index], calsetFilePath[index]));
   }
   RFmxCheckWarn(RFmxVNA_SelectActiveCalset(instrumentHandle, vnaSignal1, "Calset1", RFMXVNA_VAL_RESTORE_CONFIGURATION_NONE));

   RFmxCheckWarn(RFmxVNA_SetCorrectionEnabled(instrumentHandle, vnaSignal1, RFMXVNA_VAL_CORRECTION_ENABLED_TRUE));
   RFmxCheckWarn(RFmxVNA_Initiate(instrumentHandle, vnaSignal1, ""));

   /* Fetch results */
   RFmxCheckWarn(RFmxVNA_SParamsGetNumberOfSParameters(instrumentHandle, vnaSignal1, &numberOfSParamsResult));

   RFmxCheckWarn(RFmxVNA_SParamsFetchXData(instrumentHandle, vnaSignal1, timeout, NULL, 0, &actualArraySize));
   if (actualArraySize > 0)
   {
       sParamsXDataResult = (float64*)malloc(sizeof(float64) * actualArraySize);
       if (sParamsXDataResult)
       {
           RFmxCheckWarn(RFmxVNA_SParamsFetchXData(instrumentHandle, vnaSignal1, timeout, sParamsXDataResult, actualArraySize, NULL));
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
           RFmxCheckWarn(RFmxVNA_BuildSParameterString(vnaSignal1, index, MAX_SELECTOR_STRING, sParamSelectorString));
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