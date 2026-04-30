/************************************************************************
*
*  Example Program:
*    measureCurrent.c
*
*  Description:
*   This example demonstrates how to measure current.  To measure
*   current, you must force voltage or current on the same channel
*   on which you are making measurements.  The Measure Current
*   function returns an array of measurements in the same order
*   as the channels in the specified channel list.
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
   ViReal64 sourceVoltageLevelInVolts = 2.0;
   ViReal64 sourceCurrentRangeInAmps = 2.0e-6;
   ViReal64 measureAperatureTimeInSec = 1.0e-3;
   ViInt32 numberOfCurrentMeasurements = 0;
   ViReal64 currentMeasurementsArray[TOTAL_CHANNEL_COUNT];

   /* Initialize an NI-HSDIO session.  For STPMU operations, you may use
      either a generation session or an acquisition session.*/
   checkErr(niHSDIO_InitAcquisitionSession (deviceID, VI_FALSE, VI_FALSE, VI_NULL, &acquisitionViSession));

   /* Source the specified voltage level to all channels in the channelList.
      Now that we are forcing with the PMU, we are able to make current measurements.*/
   checkErr(niHSDIO_STPMU_SourceVoltage (acquisitionViSession,
      channelList, sourceVoltageLevelInVolts, NIHSDIO_VAL_STPMU_LOCAL_SENSE, sourceCurrentRangeInAmps));

   /* Allow the sourced voltage to stabilize */
   time(&startTime);
   do
   {
      time(&currentTime);
      timeElapsedInSeconds = difftime(currentTime, startTime);
   }while(timeElapsedInSeconds < timeToWaitInSeconds);

   /* Measure the currents on the channels specified in channelList.*/
   checkErr(niHSDIO_STPMU_MeasureCurrent (acquisitionViSession,
      channelList, measureAperatureTimeInSec, currentMeasurementsArray,
      &numberOfCurrentMeasurements));

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
      for(loopIndex = 0; loopIndex < numberOfCurrentMeasurements; loopIndex++)
      {
         printf("Measurement %2u  Current: %f amps\n", loopIndex + 1, currentMeasurementsArray[loopIndex]);
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
