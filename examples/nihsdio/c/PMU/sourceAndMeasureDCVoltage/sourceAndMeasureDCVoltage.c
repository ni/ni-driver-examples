/************************************************************************
*
*  Example Program:
*    sourceAndMeasureDCVoltage.c
*
*  Description:
*   This example demonstrates how to source and measure DC voltage.
*
*  Pin Connection Information:
*    None.
*
************************************************************************/

/* Includes */
#include <stdio.h>
#include <time.h>
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
   ViReal64 timeToWaitInSeconds =  4.0e-6;

   ViSession generationViSession = VI_NULL;
   ViConstString channelList = "23-0, PFI1";

   /* STPMU parameters */
   ViReal64 sourceVoltageLevelInVolts = 2.0;
   ViReal64 sourceCurrentRangeInAmps = 2.0e-6;
   ViReal64 measureAperatureTimeInSec = 1.0e-3;
   ViInt32 numberOfMeasurements = 0;
   ViReal64 voltageMeasurementsArray[TOTAL_CHANNEL_COUNT];

   /* Initialize an NI-HSDIO session.  For STPMU operations, you may use
      either a generation session or an acquisition session.*/
   checkErr(niHSDIO_InitGenerationSession (deviceID, VI_FALSE, VI_FALSE, VI_NULL, &generationViSession));

   /* Source the specified voltage level to all channels in the channelList */
   checkErr(niHSDIO_STPMU_SourceVoltage (generationViSession,
      channelList, sourceVoltageLevelInVolts, NIHSDIO_VAL_STPMU_LOCAL_SENSE, sourceCurrentRangeInAmps));

   /* Allow the sourced voltage to stabilize */
   time(&startTime);
   do
   {
      time(&currentTime);
      timeElapsedInSeconds = difftime(currentTime, startTime);
   }while(timeElapsedInSeconds < timeToWaitInSeconds);

   /* Measure the voltages on the channels specified in channelList.
      The voltageMeasurementsArray will be populated in the same order
      as the specified channelList.  In this case, channel 23's
      measured voltage will be the first measurement in the returned
      array. */
   checkErr(niHSDIO_STPMU_MeasureVoltage (generationViSession,
      channelList, measureAperatureTimeInSec, NIHSDIO_VAL_STPMU_LOCAL_SENSE,
      voltageMeasurementsArray, &numberOfMeasurements));

Error:

   /* Disabling the STPMU functionality on the channelList.
      Here, we have chosen to return the channels to their
      previous digital state. */
   error = niHSDIO_STPMU_DisablePMU (generationViSession,
      channelList, NIHSDIO_VAL_STPMU_RETURN_TO_PREVIOUS);

   if (error == VI_SUCCESS)
   {
      /* print result */
      printf("Done without error.\n");
      for(loopIndex = 0; loopIndex < numberOfMeasurements; loopIndex++)
      {
         printf("Measurement %2u  Voltage: %f volts\n", loopIndex + 1, voltageMeasurementsArray[loopIndex]);
      }
   }
   else
   {
      /* Get error description and print */
      niHSDIO_GetError(generationViSession, &error, sizeof(errDesc)/sizeof(ViChar), errDesc);

      printf("\nError encountered\n===================\n%s\n", errDesc);
   }

   /* close the session */
   niHSDIO_close(generationViSession);

   /* prompt to exit (for pop-up console windows) */
   printf("\nHit <Enter> to continue...\n");
   getchar();

   return error;
}
