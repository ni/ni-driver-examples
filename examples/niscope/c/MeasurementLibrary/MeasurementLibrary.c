/********************************************************************************
 * Multi Record Example, uses GenericMeasurementLibrary.h generic file
 *******************************************************************************/

#include "asciiPlot.h"
#include <locale.h>
#include <stdio.h>
#include <conio.h>
#include <time.h>
#include "GenericMeasurementLibrary.h"

#define MAX_STRING_SIZE 50
clock_t initialTime;
int askedOnce;

void main()
{
   setlocale(LC_ALL, "");

   // Initial values for the example
   initialTime = clock();
   printf("This example will fetch until any key is pressed.\n");
   printf("The screen will be updated everysecond.\n");
   askedOnce = 0;

   // Call the generic function to perform a multi record acquisition
   niScope_GenericMeasurementLibrary();
   // Wait and exit
   printf("Press any key to exit.\n");
   _getch();
}

// Obtain the resource name of the device from the user
int GetResourceNameFromGUI (ViRsrc resourceName)
{
   //Get the device name from the user
   printf("Type the device name (e.g., Dev1, PXI1Slot2, Digitizer1, ...) from Measurement & Automation Explorer: ");
   scanf("%s", resourceName);

   return 0;
}

// Obtain the necessary parameters
int GetParametersFromGUI (ViChar* channelName,
                          ViReal64* verticalRange,
                          ViInt32* verticalCoupling,
                          ViReal64* minSampleRate,
                          ViInt32* minRecordLength,
                          ViInt32* refLevelUnits,
                          ViReal64* chanBasedLowRef,
                          ViReal64* chanBasedMidRef,
                          ViReal64* chanBasedHighRef,
                          ViInt32* scalarMeasurement,
                          ViBoolean* clearStatistics,
                          ViInt32* triggerType)
{
   //////////////////////////////////////////////////////////////////////////////////////////////////////
   // NOTE: Some devices may require configuring input impedance, vertical coupling and vertical range.
   //////////////////////////////////////////////////////////////////////////////////////////////////////

   int option;
   // Default parameters
   strcpy(channelName,"0");
   *verticalRange = 10.0;
   *verticalCoupling = NISCOPE_VAL_DC;
   *minSampleRate = 10000000;
   *minRecordLength = 1000;
   *refLevelUnits = NISCOPE_VAL_MEAS_PERCENTAGE;
   *chanBasedLowRef = 10;
   *chanBasedMidRef = 50;
   *chanBasedHighRef = 90;
   *clearStatistics = VI_FALSE;
   *triggerType = 0;

   // Since we don't have a GUI, only display the menu the first time this function runs
   if (askedOnce == 0)
   {
      printf("Available Measurements:\n");
      printf("1 - Frequency\n");
      printf("2 - Average Frequency\n");
      printf("3 - FFT Frequency\n");
      printf("4 - Period\n");
      printf("5 - Average Period\n");
      printf("6 - Rise Time\n");
      printf("7 - Fall Time\n");
      printf("8 - Rising Slew Rate\n");
      printf("9 - Falling Slew Rate\n");
      printf("10 - Overshoot\n");
      printf("11 - Preshoot\n");
      printf("12 - Voltage RMS\n");
      printf("13 - Voltage Cycle RMS\n");
      printf("14 - AC Estimate\n");
      printf("15 - FFT Amplitude\n");
      printf("16 - Voltage Average\n");
      printf("17 - Voltage Cycle Average\n");
      printf("18 - DC Estimate\n");
      printf("19 - Voltage Max\n");
      printf("20 - Voltage Min\n");
      printf("21 - Voltage Peak-to-Peak\n");
      printf("22 - Voltage High\n");
      printf("23 - Voltage Low\n");
      printf("24 - Voltage Amplitude\n");
      printf("25 - Voltage Top\n");
      printf("26 - Voltage Base\n");
      printf("27 - Voltage Base-to-Top\n");
      printf("28 - Negative Width\n");
      printf("29 - Positive Width\n");
      printf("30 - Negative Duty Cycle\n");
      printf("31 - Positive Duty Cycle\n");
      printf("32 - Integral\n");
      printf("33 - Area\n");
      printf("34 - Cycle Area\n");
      printf("35 - Time Delay\n");
      printf("36 - Phase Delay\n");
      printf("37 - Low Ref Volts\n");
      printf("38 - Mid Ref Volts\n");
      printf("39 - High Ref Volts\n");
      printf("40 - Volt. Hist. Mean\n");
      printf("41 - Volt. Hist. Stdev\n");
      printf("42 - Volt. Hist. Median\n");
      printf("43 - Volt. Hist. Mode\n");
      printf("44 - Volt. Hist. Max\n");
      printf("45 - Volt. Hist. Min\n");
      printf("46 - Volt. Hist. Peak-to-Peak\n");
      printf("47 - Volt. Hist. Mean + Stdev\n");
      printf("48 - Volt. Hist. Mean + 2 Stdev\n");
      printf("49 - Volt. Hist. Mean + 3 Stdev\n");
      printf("50 - Volt. Hist. Hits\n");
      printf("51 - Volt. Hist. New Hits\n");
      printf("52 - Time Hist. Mean\n");
      printf("53 - Time Hist. Stdev\n");
      printf("54 - Time Hist. Median\n");
      printf("55 - Time Hist. Mode\n");
      printf("56 - Time Hist. Max\n");
      printf("57 - Time Hist. Min\n");
      printf("58 - Time Hist. Peak-to-Peak\n");
      printf("59 - Time Hist. Mean + Stdev\n");
      printf("60 - Time Hist. Mean + 2 Stdev\n");
      printf("61 - Time Hist. Mean + 3 Stdev\n");
      printf("62 - Time Hist. Hits\n");
      printf("63 - Time Hist. New Hits\n\n");

      printf("Choose desired measurement : ");
      scanf("%d",&option);

      // Choose the adecuate NI-SCOPE constat value
      switch (option)
      {
      case 1: *scalarMeasurement = NISCOPE_VAL_FREQUENCY;
         break;
      case 2: *scalarMeasurement = NISCOPE_VAL_AVERAGE_FREQUENCY;
         break;
      case 3: *scalarMeasurement = NISCOPE_VAL_FFT_FREQUENCY;
         break;
      case 4: *scalarMeasurement = NISCOPE_VAL_PERIOD;
         break;
      case 5: *scalarMeasurement = NISCOPE_VAL_AVERAGE_PERIOD;
         break;
      case 6: *scalarMeasurement = NISCOPE_VAL_RISE_TIME;
         break;
      case 7: *scalarMeasurement = NISCOPE_VAL_FALL_TIME;
         break;
      case 8: *scalarMeasurement = NISCOPE_VAL_RISE_SLEW_RATE;
         break;
      case 9: *scalarMeasurement = NISCOPE_VAL_FALL_SLEW_RATE;
         break;
      case 10: *scalarMeasurement = NISCOPE_VAL_OVERSHOOT;
         break;
      case 11: *scalarMeasurement = NISCOPE_VAL_PRESHOOT;
         break;
      case 12: *scalarMeasurement = NISCOPE_VAL_VOLTAGE_RMS;
         break;
      case 13: *scalarMeasurement = NISCOPE_VAL_VOLTAGE_CYCLE_RMS;
         break;
      case 14: *scalarMeasurement = NISCOPE_VAL_AC_ESTIMATE;
         break;
      case 15: *scalarMeasurement = NISCOPE_VAL_FFT_AMPLITUDE;
         break;
      case 16: *scalarMeasurement = NISCOPE_VAL_VOLTAGE_AVERAGE;
         break;
      case 17: *scalarMeasurement = NISCOPE_VAL_VOLTAGE_CYCLE_AVERAGE;
         break;
      case 18: *scalarMeasurement = NISCOPE_VAL_DC_ESTIMATE;
         break;
      case 19: *scalarMeasurement = NISCOPE_VAL_VOLTAGE_MAX;
         break;
      case 20: *scalarMeasurement = NISCOPE_VAL_VOLTAGE_MIN;
         break;
      case 21: *scalarMeasurement = NISCOPE_VAL_VOLTAGE_PEAK_TO_PEAK;
         break;
      case 22: *scalarMeasurement = NISCOPE_VAL_VOLTAGE_HIGH;
         break;
      case 23: *scalarMeasurement = NISCOPE_VAL_VOLTAGE_LOW;
         break;
      case 24: *scalarMeasurement = NISCOPE_VAL_AMPLITUDE;
         break;
      case 25: *scalarMeasurement = NISCOPE_VAL_VOLTAGE_TOP;
         break;
      case 26: *scalarMeasurement = NISCOPE_VAL_VOLTAGE_BASE;
         break;
      case 27: *scalarMeasurement = NISCOPE_VAL_VOLTAGE_BASE_TO_TOP;
         break;
      case 28: *scalarMeasurement = NISCOPE_VAL_WIDTH_NEG;
         break;
      case 29: *scalarMeasurement = NISCOPE_VAL_WIDTH_POS;
         break;
      case 30: *scalarMeasurement = NISCOPE_VAL_DUTY_CYCLE_NEG;
         break;
      case 31: *scalarMeasurement = NISCOPE_VAL_DUTY_CYCLE_POS;
         break;
      case 32: *scalarMeasurement = NISCOPE_VAL_INTEGRAL;
         break;
      case 33: *scalarMeasurement = NISCOPE_VAL_AREA;
         break;
      case 34: *scalarMeasurement = NISCOPE_VAL_CYCLE_AREA;
         break;
      case 35: *scalarMeasurement = NISCOPE_VAL_TIME_DELAY;
         break;
      case 36: *scalarMeasurement = NISCOPE_VAL_PHASE_DELAY;
         break;
      case 37: *scalarMeasurement = NISCOPE_VAL_LOW_REF_VOLTS;
         break;
      case 38: *scalarMeasurement = NISCOPE_VAL_MID_REF_VOLTS;
         break;
      case 39: *scalarMeasurement = NISCOPE_VAL_HIGH_REF_VOLTS;
         break;
      case 40: *scalarMeasurement = NISCOPE_VAL_VOLTAGE_HISTOGRAM_MEAN;
         break;
      case 41: *scalarMeasurement = NISCOPE_VAL_VOLTAGE_HISTOGRAM_STDEV;
         break;
      case 42: *scalarMeasurement = NISCOPE_VAL_VOLTAGE_HISTOGRAM_MEDIAN;
         break;
      case 43: *scalarMeasurement = NISCOPE_VAL_VOLTAGE_HISTOGRAM_MODE;
         break;
      case 44: *scalarMeasurement = NISCOPE_VAL_VOLTAGE_HISTOGRAM_MAX;
         break;
      case 45: *scalarMeasurement = NISCOPE_VAL_VOLTAGE_HISTOGRAM_MIN;
         break;
      case 46: *scalarMeasurement = NISCOPE_VAL_VOLTAGE_HISTOGRAM_PEAK_TO_PEAK;
         break;
      case 47: *scalarMeasurement = NISCOPE_VAL_VOLTAGE_HISTOGRAM_MEAN_PLUS_STDEV;
         break;
      case 48: *scalarMeasurement = NISCOPE_VAL_VOLTAGE_HISTOGRAM_MEAN_PLUS_2_STDEV;
         break;
      case 49: *scalarMeasurement = NISCOPE_VAL_VOLTAGE_HISTOGRAM_MEAN_PLUS_3_STDEV;
         break;
      case 50: *scalarMeasurement = NISCOPE_VAL_VOLTAGE_HISTOGRAM_HITS;
         break;
      case 51: *scalarMeasurement = NISCOPE_VAL_VOLTAGE_HISTOGRAM_NEW_HITS;
         break;
      case 52: *scalarMeasurement = NISCOPE_VAL_TIME_HISTOGRAM_MEAN;
         break;
      case 53: *scalarMeasurement = NISCOPE_VAL_TIME_HISTOGRAM_STDEV;
         break;
      case 54: *scalarMeasurement = NISCOPE_VAL_TIME_HISTOGRAM_MEDIAN;
         break;
      case 55: *scalarMeasurement = NISCOPE_VAL_TIME_HISTOGRAM_MODE;
         break;
      case 56: *scalarMeasurement = NISCOPE_VAL_TIME_HISTOGRAM_MAX;
         break;
      case 57: *scalarMeasurement = NISCOPE_VAL_TIME_HISTOGRAM_MIN;
         break;
      case 58: *scalarMeasurement = NISCOPE_VAL_TIME_HISTOGRAM_PEAK_TO_PEAK;
         break;
      case 59: *scalarMeasurement = NISCOPE_VAL_TIME_HISTOGRAM_MEAN_PLUS_STDEV;
         break;
      case 60: *scalarMeasurement = NISCOPE_VAL_TIME_HISTOGRAM_MEAN_PLUS_2_STDEV;
         break;
      case 61: *scalarMeasurement = NISCOPE_VAL_TIME_HISTOGRAM_MEAN_PLUS_3_STDEV;
         break;
      case 62: *scalarMeasurement = NISCOPE_VAL_TIME_HISTOGRAM_HITS;
         break;
      case 63: *scalarMeasurement = NISCOPE_VAL_TIME_HISTOGRAM_NEW_HITS;
         break;
      }
   }

   // Make sure we don't display the menu again
   askedOnce = 1;

   return 0;
}

// Display message - do a printf
int DisplayErrorMessageInGUI (ViInt32 error,
                              ViConstString errorMessage)
{

   printf("%s\n",errorMessage);
   return 0;
}

// Plot waveforms, uses ascii plot to display the waveforms in ascii
int PlotWfms (ViInt32 numWaveforms,
              ViReal64 *waveformPtr,
              struct niScope_wfmInfo *wfmInfoPtr,
              ViReal64 *measurementResult,
              ViReal64 *mean,
              ViReal64 *stdev,
              ViReal64 *min,
              ViReal64 *max,
              ViInt32 *numInStats)
{
   clock_t currentTime;
   currentTime = clock();
   // Plot only every second
   if ((double)(currentTime - initialTime) / CLOCKS_PER_SEC > 1)
   {
      initialTime = currentTime;
      // Display waveform
      if (waveformPtr && wfmInfoPtr)
      {
         asciiPlot(waveformPtr,wfmInfoPtr[0].actualSamples);
      }
      // Display the measurement values
      printf("Measurement Result: %.2f\n",*measurementResult);
      printf("Mean: %.2f\n", *mean);
      printf("Stdev: %.2f\n", *stdev);
      printf("Min: %.2f\n", *min);
      printf("Max: %.2f\n", *max);
      printf("NumInStats: %d\n", *numInStats);
   }
   return 0;
}

// Return true to stop after the first acquisition
int ProcessEvent (int *stopPtr)
{
   // Stop when keyboard is pressed
   if (_kbhit())
   {
      *stopPtr = VI_TRUE;
      _getch();
   }
   else
      *stopPtr = VI_FALSE;
   return 0;
}


/*************************************************************************************\

                              End of example

\*************************************************************************************/
