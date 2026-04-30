/************************************************************************
*
*  Example Program:
*    DynamicGenAndAcqWithPerPinVoltageConfiguration.c
*
*  Description:
*  This example demonstrates how to configure per-pin data,
*  clock, event, and trigger voltages.  External connection is required
*  for this example.
*
*  Pin Connection Information:
*    To ensure source synchronicity, please use equal length cable
*    to make the following connections on the terminal block:
*    (1) Connect the exported Sample clock (default: DDC CLK OUT)
*        from the generation device to the clock input terminal
*        (default: STROBE) on the acquisition device.
*    (2) Connect the Data Active event (default PFI 1) to the
*        acquisition Start trigger (default PFI 2).
*    (3) Connect data lines 0-7 to data lines 8-15.(Line 0 to line 8, etc.)
*
************************************************************************/

/* Includes */
#include <stdio.h>
#include "niHSDIO.h"

#define kWaveformSize 256

int main(void)
{
   ViRsrc deviceID = "PXI1Slot2";
   ViStatus error = VI_SUCCESS;
   ViChar errDesc[1024];
   ViInt32 loopIndex = 0;

   /* Create a waveform to generate */
   ViConstString waveformName = "myWfm";
   ViUInt32 waveformDataU32[kWaveformSize];

   /* Acquisition parameters */
   ViSession acquisitionViSession = VI_NULL;
   ViConstString acquisitionChannelList = "8-15";
   /* Acquisition data input voltages */
   ViConstString acquisitionChannelA = "8";
   ViReal64 acquisitionDataVoltageHighChannelA = 1.7;
   ViReal64 acquisitionDataVoltageLowChannelA = 1.6;
   ViConstString acquisitionChannelB = "9";
   ViReal64 acquisitionDataVoltageHighChannelB = 1.8;
   ViReal64 acquisitionDataVoltageLowChannelB = 1.55;
   /* Acquisition trigger voltages */
   ViReal64 acquisitionTriggerVoltageHigh = 1.9;
   ViReal64 acquisitionTriggerVoltageLow = 0.0;
   /* Acquisition input clock voltages */
   ViReal64 acquisitionInputClockVoltageHigh = 1.7;
   ViReal64 acquisitionInputClockVoltageLow = 1.6;
   ViUInt32 acquiredWaveformDataU32[kWaveformSize];
   ViInt32 numberOfSamplesRead = 0;

   /* Generation parameters */
   ViSession generationViSession = VI_NULL;
   ViReal64 sampleClockRateInHz = 50.0e6;
   ViConstString generationChannelList = "0-7";
   /* Generation data output voltages */
   ViConstString generationChannelA = "0";
   ViReal64 generationDataVoltageHighChannelA = 3.3;
   ViReal64 generationDataVoltageLowChannelA = 0.0;
   ViConstString generationChannelB = "1";
   ViReal64 generationDataVoltageHighChannelB = 3.2;
   ViReal64 generationDataVoltageLowChannelB = 0.3;
   /* Generation event voltages */
   ViReal64 generationEventVoltageHigh = 3.3;
   ViReal64 generationEventVoltageLow = 0.0;
   /* Generation output clock voltages */
   ViReal64 generationOutputClockVoltageHigh = 3.3;
   ViReal64 generationOutputClockVoltageLow = 0.0;

   /* Populate waveform with ramp data */
   for (loopIndex = 0; loopIndex < kWaveformSize; loopIndex++)
   {
      waveformDataU32[loopIndex] = loopIndex;
   }

   /* Initialize and configure generation session */
   {
      checkErr(niHSDIO_InitGenerationSession (deviceID, VI_FALSE, VI_FALSE, VI_NULL, &generationViSession ));

      checkErr(niHSDIO_AssignDynamicChannels (generationViSession,
         generationChannelList));

      /* Program generation voltage levels */
      {
         /* Program generation channel A voltage levels */
         checkErr(niHSDIO_ConfigureDataVoltageCustomLevels (generationViSession,
            generationChannelA, generationDataVoltageLowChannelA,
            generationDataVoltageHighChannelA));

         /* Program generation channel B voltage levels */
         checkErr(niHSDIO_ConfigureDataVoltageCustomLevels (generationViSession,
            generationChannelB, generationDataVoltageLowChannelB,
            generationDataVoltageHighChannelB));

         /* Program generation event voltage levels */
         checkErr(niHSDIO_ConfigureEventVoltageCustomLevels (generationViSession,
            generationEventVoltageLow, generationEventVoltageHigh));

         /* Program generation output clock voltage levels */
         checkErr(niHSDIO_SetAttributeViReal64 (generationViSession,
            "", NIHSDIO_ATTR_CLOCK_VOLTAGE_HIGH_LEVEL, generationOutputClockVoltageHigh));

         checkErr(niHSDIO_SetAttributeViReal64 (generationViSession,
            "", NIHSDIO_ATTR_CLOCK_VOLTAGE_LOW_LEVEL, generationOutputClockVoltageLow));
      }

      checkErr(niHSDIO_ConfigureSampleClock (generationViSession,
         NIHSDIO_VAL_ON_BOARD_CLOCK_STR, sampleClockRateInHz));

      /* Export the sample clock to share it with the acquisition session */
      checkErr(niHSDIO_ExportSignal (generationViSession,
         NIHSDIO_VAL_SAMPLE_CLOCK, VI_NULL, NIHSDIO_VAL_DDC_CLK_OUT_STR));

      /* Export the data active event in order to trigger the acquisition session */
      checkErr(niHSDIO_ExportSignal (generationViSession,
         NIHSDIO_VAL_DATA_ACTIVE_EVENT, VI_NULL, NIHSDIO_VAL_PFI1_STR));

      checkErr(niHSDIO_WriteNamedWaveformU32 (generationViSession,
         waveformName, kWaveformSize, waveformDataU32));
   }

   /* Initialize and configure acquisition session */
   {
      checkErr(niHSDIO_InitAcquisitionSession (deviceID, VI_FALSE, VI_FALSE, VI_NULL, &acquisitionViSession));

      checkErr(niHSDIO_AssignDynamicChannels (acquisitionViSession,
         acquisitionChannelList));

      /* Program acquisition voltage levels */
      {
         /* Program acquisition channel A voltage levels */
         checkErr(niHSDIO_ConfigureDataVoltageCustomLevels (acquisitionViSession ,
            acquisitionChannelA, acquisitionDataVoltageLowChannelA,
            acquisitionDataVoltageHighChannelA));

         /* Program acquisition channel B voltage levels */
         checkErr(niHSDIO_ConfigureDataVoltageCustomLevels (acquisitionViSession ,
            acquisitionChannelB, acquisitionDataVoltageLowChannelB,
            acquisitionDataVoltageHighChannelB));

         /* Program acquisition trigger voltage levels */
         checkErr(niHSDIO_ConfigureTriggerVoltageCustomLevels (acquisitionViSession,
            acquisitionTriggerVoltageLow, acquisitionTriggerVoltageHigh));

         /* Program generation output clock voltage levels */
         checkErr(niHSDIO_SetAttributeViReal64 (acquisitionViSession,
            "", NIHSDIO_ATTR_CLOCK_VOLTAGE_HIGH_LEVEL, acquisitionInputClockVoltageHigh));

         checkErr(niHSDIO_SetAttributeViReal64 (acquisitionViSession,
            "", NIHSDIO_ATTR_CLOCK_VOLTAGE_LOW_LEVEL, acquisitionInputClockVoltageLow));
      }

      /* Configure a single record acquisition of the ramp waveform */
      checkErr(niHSDIO_ConfigureAcquisitionSize (acquisitionViSession,
         kWaveformSize, 1));

      /* Use the sample clock exported from the generation session */
      checkErr(niHSDIO_ConfigureSampleClock (acquisitionViSession,
         NIHSDIO_VAL_STROBE_STR, sampleClockRateInHz));

      /* Wait for the generation session's exported signal to begin acquiring */
      checkErr(niHSDIO_ConfigureDigitalEdgeStartTrigger (acquisitionViSession,
         NIHSDIO_VAL_PFI2_STR, NIHSDIO_VAL_RISING_EDGE));
   }

   /* Initiate the acquistion session, which will wait for its start trigger
      from the generation session. */
   checkErr(niHSDIO_Initiate (acquisitionViSession));

   /* Initiate the generation session, which will export an event that will
      trigger the acquisition session to begin acquiring data. */
   checkErr(niHSDIO_Initiate (generationViSession));

   checkErr(niHSDIO_FetchWaveformU32 (acquisitionViSession,
      kWaveformSize, 10000, &numberOfSamplesRead, acquiredWaveformDataU32));

   /* Stop exporting the Data Active Event. */
   checkErr(niHSDIO_ExportSignal (generationViSession,
      NIHSDIO_VAL_DATA_ACTIVE_EVENT, "", NIHSDIO_VAL_DO_NOT_EXPORT_STR));

   /* Stop exporting the Sample Clock. */
   checkErr(niHSDIO_ExportSignal (generationViSession,
      NIHSDIO_VAL_SAMPLE_CLOCK, "", NIHSDIO_VAL_DO_NOT_EXPORT_STR));

Error:

   if (error == VI_SUCCESS)
   {
      /* Shifting the acquired data to the right by 8 bits because binary data is organized such that
         a bit's position corresponds to its channel number. Here, we shift 8 bits, assuming the
         acquisition channels are 8-15. */
      printf("\nExpected sample 1: %x Acquired sample 1: %x\n", waveformDataU32[0], acquiredWaveformDataU32[0] >> 8);
      printf("\nExpected sample 2: %x Acquired sample 2: %x\n", waveformDataU32[1], acquiredWaveformDataU32[1] >> 8);
      printf("\nExpected sample 3: %x Acquired sample 3: %x\n", waveformDataU32[2], acquiredWaveformDataU32[2] >> 8);
      printf("\nExpected sample 4: %x Acquired sample 4: %x\n", waveformDataU32[3], acquiredWaveformDataU32[3] >> 8);
   }
   else
   {
      printf("Error encountered\n===================\n");

      /*  Get errors from NI-HSDIO sessions. Calling niHSDIO_GetError returns
          the error and clears the error for that session. */
      niHSDIO_GetError(generationViSession, &error, sizeof(errDesc)/sizeof(ViChar), errDesc);
      if (error != VI_SUCCESS)
         printf("Device ID: %s\nError Description: %s\n", deviceID, errDesc);

      niHSDIO_GetError(acquisitionViSession, &error, sizeof(errDesc)/sizeof(ViChar), errDesc);
      if (error != VI_SUCCESS)
         printf("Device ID: %s\nError Description: %s\n", deviceID, errDesc);
   }

   /* Close the sessions */
   niHSDIO_close(acquisitionViSession);
   niHSDIO_close(generationViSession);

   /* Prompt to exit (for pop-up console windows) */
   printf("\nHit <Enter> to continue...\n");
   getchar();

   return error;
}
