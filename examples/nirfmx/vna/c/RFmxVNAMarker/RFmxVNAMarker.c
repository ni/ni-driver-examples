//Steps:
// 1. Open a new RFmx session.
// 2. Configure Frequency Reference.
// 3. Configure sweep settings: Sweep Type, Start Frequency, Stop Frequency, Number of Frequency Points, IF Bandwidth, and Power Level and Test Rx Attenuation with different port names.
// 4. Configure Averaging.
// 5. Select S-Parameter measurement.
// 6. Configure S-Parameter and format for selected S-Parameters.
// 7. Configure Magnitude Units & Phase Trace Type.
// 8. Initiate the Measurement.
// 9. Read X-Axis Values (aggregated frequency list).
// 10. Fetch S-Parameter Y data.
// 11. Configure Marker Type as Normal.
// 12. Configure Marker Data Source as 'sparam0' to be used by the Marker.
// 13. Configure Marker Peak Threshold.
// 14. Configure Marker Peak Excursion. 
// 15. Perform Peak Search on the configured data source.
// 16. Fetch X value of the Marker.
// 17. Fetch Y value of the Marker.
// 18. Based on the user selection move the Marker to Next Peak, Next Left Peak and Next Right Peak  position and Fetch X and Y value of the Marker after moving the Marker to new position. Stop the Loop on Error or if the user has pressed the Stop button
// 19. Close RFmx Session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include "niRFmxVNA.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION                4096
/* Maximum size of a selector string */
#define MAX_SELECTOR_STRING                  256

void displayMarkerMenu(void);
void displayMarkerData(float64 aMarkerX, float64 aMarkerY1, float64 aMarkerY2);

int main(int argc, char *argv[])
{
   char *resourceName = "VNA";
   niRFmxInstrHandle instrumentHandle = NULL;

   char errorMessage[MAX_ERROR_DESCRIPTION] = { 0 };
   int32 error = 0;
   int32 lastErrorCode = 0;
   int ch = 0;
   int menuBrkFlg = 0, donotFetchFlg = 0;
   int32 searchMode = 0;

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

   int32 averagingEnabled = RFMXVNA_VAL_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;

   /* Peak threshold */
   int32 thresholdEnabled = RFMXVNA_VAL_MARKER_PEAK_SEARCH_THRESHOLD_ENABLED_FALSE;
   int32 threshold = -100; 

   /* Peak Excursion */
   int32 excursionEnabled = RFMXVNA_VAL_MARKER_PEAK_SEARCH_EXCURSION_ENABLED_FALSE;
   float64 excursion = 3;

   /* Variables to store measurement data */
   float64 timeout = 10.0;                                                                              /*(seconds) */
   int32 actualArraySize = 0;
   float64* sParamsXDataResult = NULL;
   float32* sParamsY1DataResult = NULL;
   float32* sParamsY2DataResult = NULL;

   /* Variables to store marker data */
   float64 markerX = 0;
   float64 markerY1 = 0;
   float64 markerY2 = 0;

   char portSelectorString[MAX_SELECTOR_STRING];

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

   RFmxCheckWarn(RFmxVNA_SParamsCfgSParameter(instrumentHandle, "", sParamsSParameter));
   RFmxCheckWarn(RFmxVNA_SParamsSetFormat(instrumentHandle, "", sParamsFormats));
  
   RFmxCheckWarn(RFmxVNA_SParamsSetMagnitudeUnits(instrumentHandle, "", RFMXVNA_VAL_SPARAMS_MAGNITUDE_UNITS_DB));
   RFmxCheckWarn(RFmxVNA_SParamsSetPhaseTraceType(instrumentHandle, "", RFMXVNA_VAL_SPARAMS_PHASE_TRACE_TYPE_WRAPPED));
   RFmxCheckWarn(RFmxVNA_Initiate(instrumentHandle, "", ""));

   /* Fetch results */
   
   RFmxCheckWarn(RFmxVNA_SParamsFetchXData(instrumentHandle, "", timeout, NULL, 0, &actualArraySize));
   if (actualArraySize > 0)
   {
      sParamsXDataResult = (float64*)malloc(sizeof(float64) * actualArraySize);
      sParamsY1DataResult = (float32*)malloc(sizeof(float32) * actualArraySize);
      sParamsY2DataResult = (float32*)malloc(sizeof(float32) * actualArraySize);
      if (sParamsXDataResult && sParamsY1DataResult && sParamsY1DataResult)
      {
          RFmxCheckWarn(RFmxVNA_SParamsFetchXData(instrumentHandle, "", timeout, sParamsXDataResult, actualArraySize, NULL));
          RFmxCheckWarn(RFmxVNA_SParamsFetchYData(instrumentHandle, "", timeout, sParamsY1DataResult, sParamsY2DataResult, actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

    /* Configure Marker settings */
   RFmxCheckWarn(RFmxVNA_MarkerCfgType(instrumentHandle, "", RFMXVNA_VAL_MARKER_TYPE_NORMAL));
   RFmxCheckWarn(RFmxVNA_MarkerCfgDataSource(instrumentHandle, "", "sparam0"));
   RFmxCheckWarn(RFmxVNA_MarkerCfgPeakSearchThreshold(instrumentHandle, "", thresholdEnabled, threshold));
   RFmxCheckWarn(RFmxVNA_MarkerCfgPeakSearchExcursion(instrumentHandle, "", excursionEnabled, excursion));
   RFmxCheckWarn(RFmxVNA_MarkerSearch(instrumentHandle, "", RFMXVNA_VAL_MARKER_SEARCH_MODE_PEAK));
   RFmxCheckWarn(RFmxVNA_MarkerFetchX(instrumentHandle, "", &markerX));
   RFmxCheckWarn(RFmxVNA_MarkerFetchY(instrumentHandle, "", &markerY1, &markerY2));

   displayMarkerData(markerX, markerY1, markerY2);

    /* Menu to scan through different peaks of configured data source */
   displayMarkerMenu();
   menuBrkFlg = 0;
   while (1)
   {
       ch = _getch();
       donotFetchFlg = 0;
       switch (ch)
       {
           case 'l':
           case 'L':
               searchMode = RFMXVNA_VAL_MARKER_SEARCH_MODE_NEXT_LEFT_PEAK;
               break;

           case 'r':
           case 'R':
               searchMode = RFMXVNA_VAL_MARKER_SEARCH_MODE_NEXT_RIGHT_PEAK;
               break;

           case 'h':
           case 'H':
               searchMode = RFMXVNA_VAL_MARKER_SEARCH_MODE_NEXT_PEAK;
               break;

           case 's':
           case 'S':
               menuBrkFlg = 1;
               donotFetchFlg = 1;
               break;
           default:
               donotFetchFlg = 1;
               displayMarkerMenu();
               break;
       }

       if (donotFetchFlg == 0) /* Fetch marker data */
       {
           error = RFmxVNA_MarkerSearch(instrumentHandle, "", searchMode);
           if (error == 0)
           {
               error = RFmxVNA_MarkerFetchX(instrumentHandle, "", &markerX);
               error = RFmxVNA_MarkerFetchY(instrumentHandle, "", &markerY1, &markerY2);
               if (error == 0)
                   displayMarkerData(markerX, markerY1, markerY2);
               else
                   menuBrkFlg = 1;
           }
           else
               menuBrkFlg = 1;
       }
       if (menuBrkFlg)
       {
           printf("Exiting menu. . .\n");
           break;
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

   printf("Press any key to continue\n");
   _getch();

   return error;
}

void displayMarkerMenu(void)
{
    printf("To read different points, use the following keys\n");
    printf("l/L - Next Left Peak\n");
    printf("r/R - Next Right Peak\n");
    printf("h/H - Next Peak\n");
    printf("s/S - Stop or Exit\n");
}

void displayMarkerData(float64 aMarkerX, float64 aMarkerY1, float64 aMarkerY2)
{
    printf("\n---------------------------------------------------\n");
    printf("Marker X  : %f\n", aMarkerX);
    printf("Marker Y1 : %f\n", aMarkerY1);
    printf("Marker Y2 : %f\n", aMarkerY2);
    printf("\n----------------------------------------------------\n");
}