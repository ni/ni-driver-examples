//Steps:
//1. Open a new RFmx session
//2. Configure the instrument properties Clock Source and Clock Frequency
//3. Configure Selected Ports
//4. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//5. Select Spectrum measurement
//6. Configure Spectrum RBW
//7. Configure Spectrum Span
//8. Configure Spectrum Averaging
//9.  Initiate Measurement
//10. Fetch Spectrum Trace
//11. Configure Marker Peak Threshold
//12. Configure Marker Type as Normal
//13. Configure "Spectrum" as the trace to be used by the Marker
//14. Use Marker Peak Search to detect the Number of Peaks in the Spectrum
//15. Fetch XY Location of the Marker
//16. Based on the user selection move the Marker to Next Highest, Next Left and Next Right position
//and Fetch XY Location of the Marker after moving the Marker to new position
//Stop the Loop on Error or if the user has pressed the Stop button
//17. Close the RFmx Session

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include "niRFmxSpecAn.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096


void displayMarkerMenu(void);
void displayMarkerData(int32 aNumberOfPeaks, int32 aPeakFound, float64 aMarkerXLocation, float64 aMarkerYLocation);

int main(int argc, char *argv[])
{
   int ch = 0;
   int menuBrkFlg = 0, donotFetchFlg = 0;
   int32 peakFound = 0;
   int32 peakToFetch = 0;

   niRFmxInstrHandle instrumentHandle = NULL;
   int32 error = 0, lastErrorCode = 0;
   char errorMessage[MAX_ERROR_DESCRIPTION] = { '\0' };

   char *resourceName = "RFSA";
   char *frequencySource = RFMXINSTR_VAL_ONBOARD_CLOCK_STR;
   char *selectedPorts = "";
   float64 centerFrequency = 1e+9;        /* Hz */
   float64 referenceLevel = 0.00;         /* dBm */
   float64 externalAttenuation = 0.00;    /* dB */
   float64 frequency = 10.0e+6;           /* Hz */

   float64 timeout = 10.0;                /* seconds */

   /* Span */
   float64 span = 1.0e+6;                 /* Hz */

   /* RBW */
   int32 RBWFilterType = RFMXSPECAN_VAL_SPECTRUM_RBW_FILTER_TYPE_GAUSSIAN;
   int32 RBWAuto = RFMXSPECAN_VAL_SPECTRUM_RBW_AUTO_FALSE;
   float64 RBW = 10.0e+3;                 /* Hz */

   /* Averaging */
   int32 averagingEnabled = RFMXSPECAN_VAL_SPECTRUM_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;
   int32 averagingType = RFMXSPECAN_VAL_SPECTRUM_AVERAGING_TYPE_RMS;

   int32 markerType = RFMXSPECAN_VAL_MARKER_TYPE_NORMAL;
   int32 markerTrace = RFMXSPECAN_VAL_MARKER_TRACE_SPECTRUM;

   /* Peak threshold */
   int32 thresholdEnabled = RFMXSPECAN_VAL_MARKER_THRESHOLD_ENABLED_FALSE;
   int32 threshold = -90;                 /* Abs units */

   /* Peak Excursion */
   int32 excursionEnabled = RFMXSPECAN_VAL_MARKER_PEAK_EXCURSION_ENABLED_FALSE;
   float64 excursion = 6;                 /*  Rel units */

   /* Variables to store measurement data */
   int32 numberOfPeaks = 0;
   float64 markerXLocation = 0;
   float64 markerYLocation = 0;

   float64 x0 = 0.0, dx = 0.0;
   float32 *spectrum = (float32 *)NULL;
   int32 actualArraySize = 0;

   /* Create a new RFmx Session */
   RFmxCheckWarn(RFmxSpecAn_Initialize(resourceName, "", &instrumentHandle, NULL));

   /* Configure Spectrum settings */
   RFmxCheckWarn(RFmxSpecAn_CfgFrequencyReference(instrumentHandle, "", frequencySource, frequency));
   RFmxCheckWarn(RFmxSpecAn_SetSelectedPorts(instrumentHandle, "", selectedPorts));
   RFmxCheckWarn(RFmxSpecAn_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
   RFmxCheckWarn(RFmxSpecAn_SelectMeasurements(instrumentHandle, "", RFMXSPECAN_VAL_SPECTRUM, RFMXSPECAN_VAL_TRUE));
   RFmxCheckWarn(RFmxSpecAn_SpectrumCfgRBWFilter(instrumentHandle, "", RBWAuto, RBW, RBWFilterType));
   RFmxCheckWarn(RFmxSpecAn_SpectrumCfgSpan(instrumentHandle, "", span));
   RFmxCheckWarn(RFmxSpecAn_SpectrumCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));
   RFmxCheckWarn(RFmxSpecAn_Initiate(instrumentHandle, "", ""));

   /* Retrieve Spectrum data */
   RFmxCheckWarn(RFmxSpecAn_SpectrumFetchSpectrum(instrumentHandle, "", timeout, NULL, NULL, NULL, 0,
      &actualArraySize));
   if (actualArraySize > 0)
   {
      spectrum = (float32 *)malloc(sizeof(float32)*actualArraySize);

      if (spectrum)
      {
         RFmxCheckWarn(RFmxSpecAn_SpectrumFetchSpectrum(instrumentHandle, "", timeout, &x0, &dx, spectrum,
            actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   /* Configure Marker settings */
   RFmxCheckWarn(RFmxSpecAn_MarkerCfgThreshold(instrumentHandle, "", thresholdEnabled, threshold));
   RFmxCheckWarn(RFmxSpecAn_MarkerCfgPeakExcursion(instrumentHandle, "", excursionEnabled, excursion));
   RFmxCheckWarn(RFmxSpecAn_MarkerCfgType(instrumentHandle, "marker0", markerType));
   RFmxCheckWarn(RFmxSpecAn_MarkerCfgTrace(instrumentHandle, "marker0", markerTrace));
   RFmxCheckWarn(RFmxSpecAn_MarkerPeakSearch(instrumentHandle, "marker0", &numberOfPeaks));
   RFmxCheckWarn(RFmxSpecAn_MarkerFetchXY(instrumentHandle, "marker0", &markerXLocation, &markerYLocation));

   displayMarkerData(numberOfPeaks, peakFound, markerXLocation, markerYLocation);

   /* Menu to scan through different peaks of spectrum trace */
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
         peakToFetch = RFMXSPECAN_VAL_MARKER_NEXT_PEAK_NEXT_LEFT;
         break;

      case 'r':
      case 'R':
         peakToFetch = RFMXSPECAN_VAL_MARKER_NEXT_PEAK_NEXT_RIGHT;
         break;

      case 'h':
      case 'H':
         peakToFetch = RFMXSPECAN_VAL_MARKER_NEXT_PEAK_NEXT_HIGHEST;
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
         error = RFmxSpecAn_MarkerNextPeak(instrumentHandle, "marker0", peakToFetch, &peakFound);
         if (error == 0)
         {
            error = RFmxSpecAn_MarkerFetchXY(instrumentHandle, "marker0", &markerXLocation,
               &markerYLocation);
            if (error == 0)
               displayMarkerData(numberOfPeaks, peakFound, markerXLocation, markerYLocation);
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
      RFmxSpecAn_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
      if (error < 0)
         printf("ERROR: %s\n", errorMessage);
      else
         printf("WARNING: %s\n", errorMessage);
   }
   if (instrumentHandle)
   {
      RFmxSpecAn_Close(instrumentHandle, RFMXSPECAN_VAL_FALSE);
   }
   /* Free allocated memory */
   if (spectrum)
      free(spectrum);
   printf("Press any key to continue\n");
   _getch();

   return error;
}

void displayMarkerMenu(void)
{
   printf("To read different points, use the following keys\n");
   printf("l/L - Next Left\n");
   printf("r/R - Next Right\n");
   printf("h/H - Next Highest\n");
   printf("s/S - Stop or Exit\n");
}

void displayMarkerData(int32 aNumberOfPeaks, int32 aPeakFound, float64 aMarkerXLocation, float64 aMarkerYLocation)
{
   printf("\n---------------------------------------------------\n");
   printf("Number of peaks          : %d\n", aNumberOfPeaks);
   printf("Next peak found?         : %d\n", aPeakFound);
   printf("Marker X Location (Hz)   : %f\n", aMarkerXLocation);
   printf("Marker Y Location (dBm)  : %f\n", aMarkerYLocation);
   printf("\n----------------------------------------------------\n");
}
