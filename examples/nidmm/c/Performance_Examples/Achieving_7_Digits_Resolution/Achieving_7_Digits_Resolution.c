/******************************************************************************/
/*                                                                            */
/* National Instruments - NI-DMM - C Examples                                 */
/*                                                                            */
/* Achieving 7 Digits Resolution                                              */
/*                                                                            */
/* This example describes how to program a National Instruments Multimeter    */
/* to achieve a resolution of 7 noise-free digits.                            */
/*                                                                            */
/* Most traditional DMMs specify digits of resolution based on noise "counts" */
/* on a fixed display that defines the performance of the ADC. A more         */
/* conservative approach is to calculate the effective resolution based on    */
/* noise-free digits. You can use the example's calculation as a guide,       */
/* in addition to the actual measured values, to determine the appropriate    */
/* aperture for a given measurement setup and conditions.                     */
/*                                                                            */
/* For more information, refer to the Examples topic in the NI Digital        */
/* Multimeters Help.                                                          */
/******************************************************************************/

#define MAXPTSTOREAD 5000
#define MAXPTSTOANALYZE 100
#define LEASTNUMPOINTSTOREAD 4

#include "nidmm.h"
#include <stdio.h>
#include <string.h>
#include <math.h>
#include <malloc.h>

ViBoolean  RequestToStop = VI_FALSE;

ViReal64 CalculateResolutionFromData (ViReal64 range,ViReal64 measurements[],
                                      ViInt32 measurementsIndex, ViInt32 totalPointsAcquired);

void StdDev (ViReal64* samples, ViInt32 sampleSize, ViReal64 *measurementMean,
             ViReal64 *measurementStdDev);

int promptForResourceName(ViChar resourceName[] )
{
   //Get the device name from the user
   printf("Type the device name (e.g., DAQ::1, Dev1, PXI0Slot1, ...): ");
   fgets(resourceName, 255, stdin);
   resourceName[strlen(resourceName) - 1] = '\0'; // Remove newline character

   return 0;
}

void ErrorHandler(ViSession vi, ViStatus error)
{
   ViChar   *errorText;
   ViStatus errorCode = VI_SUCCESS;
   ViInt32  bufferSize = 0;

   if (error == VI_SUCCESS)
   {
      // No error, nothing to do
      return;
   }

   // First find the length of the error description.  Pass VI_NULL for the
   // buffer to retrieve the length of the error message.
   bufferSize = niDMM_GetError(vi, &errorCode, 0, VI_NULL);
   if(bufferSize > 0)
   {
      // Return code >0 from first call to GetError represents the size of
      // the description.  Call it again.
      // Ignore incoming IVI error code and return description from the driver
      // (trust that the IVI error code was properly stored in the session
      // by the driver)
      errorText = (ViChar *)malloc(bufferSize);
      niDMM_GetError(vi, &errorCode, bufferSize, errorText);
   }
   else
   {
      // Return code <= 0 from GetError indicates a problem.  This is expected
      // when the session is invalid (IVI spec requires GetError to fail).
      // Use GetErrorMessage instead.  It doesn't require a session.

      // Call niDMM_GetErrorMessage, pass VI_NULL for the buffer in order to retrieve
      // the length of the error message.
      errorCode = bufferSize;
      bufferSize = niDMM_GetErrorMessage(VI_NULL, errorCode, 0, VI_NULL);
      errorText = (ViChar *)malloc(bufferSize);

      // Get the error description
      niDMM_GetErrorMessage(VI_NULL, errorCode, bufferSize, errorText);
   }

   if(errorCode < VI_SUCCESS)
   {
      // Print Error
      printf("Failure; error = %d\n", error);
      printf("error msg: %s\n", errorText);
   }
   else
   {
      // Print Warning
      printf("warning = %d\n", error);
      printf("warning msg: %s\n\n", errorText);
   }

   free(errorText);
   return;
}

int main (int argc, char *argv[])
{
   ViChar    resourceName[256]   = "Dev1";
   ViSession vi                  = VI_NULL;
   ViBoolean idQuery             = VI_TRUE;
   ViBoolean reset               = VI_TRUE;
   ViStatus  error               = VI_SUCCESS;

   ViInt32   measurementType     = NIDMM_VAL_DC_VOLTS;
   ViReal64  range               = 10.0;
   ViReal64  resolution          = 5.5;
   ViReal64  apertureTime        = 0.016777;
   ViInt32   autoZeroMode        = NIDMM_VAL_AUTO_ZERO_ON;
   ViInt32   ADCCal              = NIDMM_VAL_ADC_CALIBRATION_OFF;
   ViInt32   numOfAverages       = 5;

   ViReal64  effectiveResolution = 0.0;
   ViInt32   totalPointsAcquired = 0;
   ViInt32   numPointsRead       = 0;
   ViInt32   ptsAvailable        = 0;
   ViInt16   acqStatus           = 0;

   ViReal64  measurements[MAXPTSTOREAD] = {0.0};
   ViInt32 measurementsIndex     = 0;
   ViInt32   effectiveSize       = 0;

   /*- Ask user to specify device --------------------------------------*/
   promptForResourceName(resourceName);

   /*- Initialize the DMM and create a new session --------------------------*/
   checkErr(niDMM_init(resourceName, idQuery, reset, &vi));

   checkErr(niDMM_ConfigureMeasurementDigits (vi, measurementType, range, resolution));

   /*-Configure Multipoint to get continuos measurement----------------------*/
   checkErr(niDMM_ConfigureMultiPoint (vi, 1, 0,NIDMM_VAL_IMMEDIATE, 0.0));

   /*- Set resolution enhancing attributes ----------------------------------*/
   checkErr(niDMM_SetAttributeViInt32 (vi, "", NIDMM_ATTR_APERTURE_TIME_UNITS, NIDMM_VAL_SECONDS));

   checkErr(niDMM_SetAttributeViReal64 (vi, "", NIDMM_ATTR_APERTURE_TIME, apertureTime));

   checkErr(niDMM_SetAttributeViInt32 (vi, "", NIDMM_ATTR_NUMBER_OF_AVERAGES, numOfAverages));

   /*- Configure autozero mode ----------------------------------------------*/
   checkErr(niDMM_ConfigureAutoZeroMode (vi, autoZeroMode));

   /*- Configure ADC calibration --------------------------------------------*/
   checkErr(niDMM_ConfigureADCCalibration (vi, ADCCal));

   /*- Initiate -------------------------------------------------------------*/
   checkErr(niDMM_Initiate (vi));

   RequestToStop = VI_FALSE;

   while(!RequestToStop)
   {
      /*- Make sure there are enough points to achieve high resolution -------- */
      checkErr(niDMM_ReadStatus (vi, &ptsAvailable, &acqStatus));
      /*- Make sure a predefined least number of point are available -------------*/
      if(ptsAvailable > LEASTNUMPOINTSTOREAD)
      {
         /*- effectiveSize is numbers of data points array can store before it reaches
             it's end -------------------------------------------------------------*/
         effectiveSize = MAXPTSTOREAD - measurementsIndex;

         /*- If array will reach it's end before all the ptsAvailable are do multiple
             FetchMultipoints ----------------------------------------------------*/
         if( ptsAvailable > effectiveSize )
         {
            while ( ptsAvailable > effectiveSize )
            {
               /*- Fetch only the data as much as effective size------------------------*/
               checkErr(niDMM_FetchMultiPoint(vi, NIDMM_VAL_TIME_LIMIT_AUTO, effectiveSize,
                                              &measurements[measurementsIndex], &numPointsRead));

               /*- Update the total numbers of points acquired -------------------------*/
               totalPointsAcquired += numPointsRead;

               /*- Update the number of points available -------------------------------*/
               ptsAvailable -= numPointsRead;

               /*- Reset the array iterator to point to very begining-------------------*/
               measurementsIndex = 0;

               /*- Since we we have reset the array iterator at the beginning of the array,
                   effective size is the size of the array------------------------------*/
               effectiveSize = MAXPTSTOREAD;
            }
         }
         /*- Fetch data---------------------------------------------------------*/
         checkErr(niDMM_FetchMultiPoint(vi, NIDMM_VAL_TIME_LIMIT_AUTO, ptsAvailable,
                                        &measurements[measurementsIndex], &numPointsRead));

         totalPointsAcquired += numPointsRead;
         measurementsIndex += numPointsRead;

         effectiveResolution = CalculateResolutionFromData(range , measurements,
                                            measurementsIndex, totalPointsAcquired);
      }
      if(totalPointsAcquired >= MAXPTSTOANALYZE) RequestToStop = 1;
   }

 Error:

   printf ("\n\n===================================\n\n");

   if (error < VI_SUCCESS)
   {
      ErrorHandler(vi, error);
   }
   else
   {
      if (error)
      {
         // Function call returned a warning, print it first
         ErrorHandler(vi, error);
      }
      printf("totalPointsAcquired = %d\n", totalPointsAcquired);
      printf("effectiveResolution = %lf\n", effectiveResolution);
   }
   printf ("\n\n===================================\n\n");

   if(vi)
      niDMM_close(vi);

   return(0);
}


/******************************************************************************/
/* Definition of function that uses the actual data set to calculate the      */
/* resolution of the measurements.                                            */
/******************************************************************************/

ViReal64 CalculateResolutionFromData (ViReal64 range,ViReal64 measurements[],
                                      ViInt32 measurementsIndex, ViInt32 totalPointsAcquired)
{
   ViReal64  resolution                       = 0;
   ViReal64  measurementMean                  = 0;
   ViReal64  measurementStdDev                = 0;
   ViInt32   sampleSize                       = 0;
   ViReal64  samples[MAXPTSTOANALYZE]         = {0.0};

   ViReal64* PtrToMostRecentPtsToAnalyze;
   ViReal64* samplesItr                       = samples;
   ViReal64* lastMeasurementsIndexPtr         = measurements + MAXPTSTOREAD;
   ViInt32   numPointsFromTopToMeasurenetsIndex = measurementsIndex;
   ViInt32   pointsRemaining                  = 0;

   /*- If the total points acquired till now are less than samples' size, copy them all in
       samples array ---------------------------------------------------------------------*/
   if(totalPointsAcquired < MAXPTSTOANALYZE )
   {
      /*- copy the data ( < MAXPTSTOANALYZE) available into samples array ----------------*/
      memcpy(samples, measurements, totalPointsAcquired*8);
       sampleSize = totalPointsAcquired;
   }

   /*- If there are more points in measurements array than sample size, only copy the most
       recent number of points equal to sample size*/
   else
   {
     /*- copy only latest data points( =MAXPTSTOANALYZE) available into samples array ----*/
     sampleSize = MAXPTSTOANALYZE;

     if(numPointsFromTopToMeasurenetsIndex >= MAXPTSTOANALYZE )
     {
        PtrToMostRecentPtsToAnalyze   = &measurements[measurementsIndex] - MAXPTSTOANALYZE;
        /*- Copy last MAXPTSTOANALYZE points of data from the measurements array ---------*/
        memcpy(samples, PtrToMostRecentPtsToAnalyze , MAXPTSTOANALYZE*8);
     }
     else
     {
        /*- Copy from the beginning of the measurements array ----------------------------*/
        memcpy(samples, measurements , numPointsFromTopToMeasurenetsIndex*8);

        /*- Update samples Iterator ------------------------------------------------------*/
        samplesItr += numPointsFromTopToMeasurenetsIndex;

        /*- Points remaining equals sample array size - num points already copied into
            samples ----------------------------------------------------------------------*/
        pointsRemaining = MAXPTSTOANALYZE - numPointsFromTopToMeasurenetsIndex;

        /*- Copy from the end of the measurements array ----------------------------------*/
        memcpy(samplesItr, lastMeasurementsIndexPtr - pointsRemaining, pointsRemaining*8);
     }
   }

   /*- calculate the mean and the standard deviation for the data set --------------------*/
   StdDev (samples, sampleSize, &measurementMean, &measurementStdDev);

   if(measurementStdDev != 0)
        resolution = log10 ((range*2.0)/(measurementStdDev * sqrt (12.00)));

   return(resolution);
}


/*****************************************************************************\
                           End example source code
\*****************************************************************************/

void StdDev (ViReal64* samples, ViInt32 sampleSize, ViReal64 *measurementMean, ViReal64 *measurementStdDev){
   int i=0;
   ViReal64 mean=0;
   ViReal64 meanSquared=0;
   ViReal64 stddev=0;
   for(i=0; i<sampleSize; i++){
      mean += samples[i];
      meanSquared += samples[i]*samples[i];
   }
   mean /= sampleSize;
   meanSquared /= sampleSize;
   *measurementStdDev = sqrt(meanSquared - mean*mean);
   *measurementMean = mean;
}
