/************************************************************************
*
*  Example Program:
*    sourceDCCurrentAndMeasure.c
*
*  Description:
*   This example demonstrates how to source current and then measure
*   voltage and measure current on the same set of channels.  This
*   example configures and applies the specified current on the channels
*   in the channel list.  The PMU attempts to force the specified current
*   while limiting the voltage to the specified upper and lower limits.
*   The Measure Current and Measure Voltage function calls return
*   an array of measurements in the same order as the channels specified
*   in the channel list.
*
*  Pin Connection Information:
*    None.
*
************************************************************************/

/* Includes */
#include <time.h>
#include <stdio.h>
#include "niHSDIO.h"

#define TOTAL_CHANNEL_COUNT 28

int main(void)
{
   ViRsrc deviceID = "PXI1Slot2";
   ViStatus error = VI_SUCCESS;
   ViChar errDesc[1024];
   ViInt32 loopIndex = 0;

   /* Timing parameters */
   time_t startTime = 0;
   time_t currentTime = 0;
   ViReal64 timeElapsedInSeconds = 0;
   ViReal64 timeToWaitInSeconds = 4.0e-6;

   ViSession acquisitionViSession = VI_NULL;
   ViConstString channelList = "0-23, PFI1, PFI2, PFI4, PFI5";

   /* STPMU parameters */
   ViReal64 sourceCurrentLevelInAmps = 0.020;
   ViReal64 sourceCurrentRangeInAmps = 0.032;
   ViReal64 sourceUpperVoltageLimitInVolts = 3.3;
   ViReal64 sourceLowerVoltageLimitInVolts = 0.0;
   ViReal64 measureAperatureTimeInSec = 1.0e-3;
   ViInt32 numberOfCurrentMeasurements = 0;
   ViReal64 currentMeasurementsArray[TOTAL_CHANNEL_COUNT];
   ViInt32 numberOfVoltageMeasurements = 0;
   ViReal64 voltageMeasurementsArray[TOTAL_CHANNEL_COUNT];

   /* Initialize an NI-HSDIO session.  For STPMU operations, you may use
      either a generation session or an acquisition session.*/
   checkErr(niHSDIO_InitAcquisitionSession (deviceID, VI_FALSE, VI_FALSE, VI_NULL, &acquisitionViSession));

   /* Source the specified current level to all channels in the channelList.
      Use the voltage limits to restrain output voltage levels.*/
   checkErr(niHSDIO_STPMU_SourceCurrent (acquisitionViSession,
      channelList, sourceCurrentLevelInAmps, sourceCurrentRangeInAmps,
      sourceLowerVoltageLimitInVolts, sourceUpperVoltageLimitInVolts));

   /* Allow the sourced current to stabilize */
   time(&startTime);
   do
   {
      time(&currentTime);
      timeElapsedInSeconds = difftime(currentTime, startTime);
   }while(timeElapsedInSeconds < timeToWaitInSeconds);

   /* Measure the currents on the channels specified in channelList.
      Note: In order to measure current, you must source either voltage or current. */
   checkErr(niHSDIO_STPMU_MeasureCurrent (acquisitionViSession,
      channelList, measureAperatureTimeInSec, currentMeasurementsArray,
      &numberOfCurrentMeasurements));

   /* Measure the voltages on the channels specified in channelList.*/
   checkErr(niHSDIO_STPMU_MeasureVoltage (acquisitionViSession,
      channelList, measureAperatureTimeInSec, NIHSDIO_VAL_STPMU_LOCAL_SENSE,
      voltageMeasurementsArray, &numberOfVoltageMeasurements));

Error:

   /* Disabling the STPMU functionality on the channelList.
      Here, we have chosen to return the channels to their
      previous digital state. */
   error = niHSDIO_STPMU_DisablePMU (acquisitionViSession,
      channelList, NIHSDIO_VAL_STPMU_RETURN_TO_PREVIOUS);

   if (error == VI_SUCCESS)
   {
      /* print result */
      printf("Done without error.\n");
      for
      (
         loopIndex = 0;
         loopIndex < numberOfCurrentMeasurements && loopIndex < numberOfVoltageMeasurements;
         loopIndex++
      )
      {
         printf("Measurement %2u:\n  Voltage: %f volts Current: %f amps\n",
            loopIndex + 1, voltageMeasurementsArray[loopIndex], currentMeasurementsArray[loopIndex]);
      }
   }
   else
   {
      /* Get error description and print */
      niHSDIO_GetError(acquisitionViSession, &error, sizeof(errDesc)/sizeof(ViChar), errDesc);

      printf("\nError encountered\n===================\n%s\n", errDesc);
   }

   /* close the session */
   niHSDIO_close(acquisitionViSession);

   /* prompt to exit (for pop-up console windows) */
   printf("\nHit <Enter> to continue...\n");
   getchar();

   return error;
}
