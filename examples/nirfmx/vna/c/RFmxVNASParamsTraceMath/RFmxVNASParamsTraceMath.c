//Steps:
//1. Open a new RFmx session.
//2. Configure Frequency Reference.
//3. Configure sweep settings: Sweep Type, Start Frequency, Stop Frequency, Number of Frequency Points, IF Bandwidth, and Power Level and Test Rx Attenuation with different port names.
//4. Configure Averaging.
//5. Select S-Parameter measurement.
//6. Configure S-Parameter and format for selected S-Parameters.
//7. Configure Magnitude Units & Phase Trace Type.
//8. Initiate the Measurement.
//9. Copy Measurement Data to Memory and Get Memory Data.
//10. Configure Math Function.
//11. Initiate the Measurement for Applying Math Function.
//12. Read X-Axis Values (aggregated frequency list).
//13. Fetch Math Applied S-Parameter Y data.
//14. Close RFmx Session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
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

   float64 startFrequency = 1.0e9;
   float64 stopFrequency = 26.0e9;
   int32 numberOfFrequencyPoints = 251;
   float64 port1PowerLevel = -10.0;                                                                      /* (dBm) */
   float64 port2PowerLevel = -10.0;                                                                      /* (dBm) */
   float64 port1TestReceiverAttenuation = 0.0;                                                           /* (dB) */
   float64 port2TestReceiverAttenuation = 0.0;                                                           /* (dB) */
   float64 IFBandwidth = 100.0e3;                                                                        /* (Hz) */

   char* sParamsSParameter = "S11";
   int32 sParamsFormats = RFMXVNA_VAL_SPARAMS_FORMAT_MAGNITUDE;
   char* memoryName = "Memory0";
   int32 mathFunction = RFMXVNA_VAL_SPARAMS_MATH_FUNCTION_DIVIDE;
   int32 averagingEnabled = RFMXVNA_VAL_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;

   float64 timeout = 10.0;                                                                               /*(seconds) */

   /* Variables to store measurement data */
   
   int32 actualArraySize = 0;
   float64* sParamsXDataResult = NULL;
   float32* sParamsY1DataResult = NULL;
   float32* sParamsY2DataResult = NULL;
   float64* sParamsMemoryXDataResult = NULL;
   float32* sParamsMemoryY1DataResult = NULL;
   float32* sParamsMemoryY2DataResult = NULL;

   char portSelectorString[MAX_SELECTOR_STRING];
   char sParamSelectorString[MAX_SELECTOR_STRING];
   char measurementMemorySelectorString[MAX_SELECTOR_STRING];

   /* Initialize a session */
   RFmxCheckWarn(RFmxVNA_Initialize(resourceName, "", &instrumentHandle, NULL));

   /* Configure the session */
   RFmxCheckWarn(RFmxVNA_CfgFrequencyReference(instrumentHandle, "", RFMXVNA_VAL_PXI_CLK_STR, 100.0e6));
   RFmxCheckWarn(RFmxVNA_SetSweepType(instrumentHandle, "", RFMXVNA_VAL_SWEEP_TYPE_LINEAR));
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
   RFmxCheckWarn(RFmxVNA_SetAveragingEnabled(instrumentHandle, "", averagingEnabled));
   RFmxCheckWarn(RFmxVNA_SetAveragingCount(instrumentHandle, "", averagingCount));
   RFmxCheckWarn(RFmxVNA_SelectMeasurements(instrumentHandle, "", RFMXVNA_VAL_SPARAMS, RFMXVNA_VAL_FALSE));
   RFmxCheckWarn(RFmxVNA_BuildSParameterString("", 0, MAX_SELECTOR_STRING, sParamSelectorString));
   RFmxCheckWarn(RFmxVNA_SParamsCfgSParameter(instrumentHandle, sParamSelectorString, sParamsSParameter));
   RFmxCheckWarn(RFmxVNA_SParamsSetFormat(instrumentHandle, sParamSelectorString, sParamsFormats));
  
   RFmxCheckWarn(RFmxVNA_SParamsSetMagnitudeUnits(instrumentHandle, "", RFMXVNA_VAL_SPARAMS_MAGNITUDE_UNITS_DB));
   RFmxCheckWarn(RFmxVNA_SParamsSetPhaseTraceType(instrumentHandle, "", RFMXVNA_VAL_SPARAMS_PHASE_TRACE_TYPE_WRAPPED));
   RFmxCheckWarn(RFmxVNA_Initiate(instrumentHandle, "", ""));

   
   RFmxCheckWarn(RFmxVNA_CopyDataToMeasurementMemory(instrumentHandle, sParamSelectorString, memoryName));
   RFmxCheckWarn(RFmxVNA_BuildMeasurementMemoryString(sParamSelectorString, memoryName, MAX_SELECTOR_STRING, measurementMemorySelectorString));

   /* Get Memory results */
   RFmxCheckWarn(RFmxVNA_GetMeasurementMemoryXData(instrumentHandle, measurementMemorySelectorString, NULL, 0, &actualArraySize));
   if (actualArraySize > 0)
   {
       sParamsMemoryXDataResult = (float64*)malloc(sizeof(float64) * actualArraySize);
       sParamsMemoryY1DataResult = (float32*)malloc(sizeof(float32) * actualArraySize);
       sParamsMemoryY2DataResult = (float32*)malloc(sizeof(float32) * actualArraySize);
       if (sParamsMemoryXDataResult && sParamsMemoryY1DataResult && sParamsMemoryY2DataResult)
       {
           RFmxCheckWarn(RFmxVNA_GetMeasurementMemoryXData(instrumentHandle, measurementMemorySelectorString, sParamsMemoryXDataResult, actualArraySize, NULL));
           RFmxCheckWarn(RFmxVNA_GetMeasurementMemoryYData(instrumentHandle, measurementMemorySelectorString, sParamsMemoryY1DataResult, sParamsMemoryY2DataResult, actualArraySize, NULL));
       }
       else
       {
           printf("malloc failed.\n");
           goto Error;
       }
   }

   RFmxCheckWarn(RFmxVNA_SParamsSetMathFunction(instrumentHandle, sParamSelectorString, mathFunction));
   RFmxCheckWarn(RFmxVNA_Initiate(instrumentHandle, "", ""));

   /* Fetch Math Applied results */
   actualArraySize = 0;
   RFmxCheckWarn(RFmxVNA_SParamsFetchXData(instrumentHandle, "", timeout, NULL, 0, &actualArraySize));
   if (actualArraySize > 0)
   {
      sParamsXDataResult = (float64*)malloc(sizeof(float64) * actualArraySize);
      sParamsY1DataResult = (float32*)malloc(sizeof(float32) * actualArraySize);
      sParamsY2DataResult = (float32*)malloc(sizeof(float32) * actualArraySize);
      if (sParamsXDataResult && sParamsY1DataResult && sParamsY2DataResult)
      {
          RFmxCheckWarn(RFmxVNA_SParamsFetchXData(instrumentHandle, "", timeout, sParamsXDataResult, actualArraySize, NULL));
          RFmxCheckWarn(RFmxVNA_SParamsFetchYData(instrumentHandle, sParamSelectorString, timeout, sParamsY1DataResult, sParamsY2DataResult, actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
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
   if (sParamsXDataResult) 
   {
      free(sParamsXDataResult);
   }
   if (sParamsY1DataResult) 
   {
      free(sParamsY1DataResult);
   }
   if (sParamsY2DataResult)
   {
       free(sParamsY2DataResult);
   }
   if (sParamsMemoryXDataResult)
   {
       free(sParamsMemoryXDataResult);
   }
   if (sParamsMemoryY1DataResult)
   {
       free(sParamsMemoryY1DataResult);
   }
   if (sParamsMemoryY2DataResult)
   {
       free(sParamsMemoryY2DataResult);
   }

   printf("Press any key to exit\n");
   _getch();

   return error;
}
