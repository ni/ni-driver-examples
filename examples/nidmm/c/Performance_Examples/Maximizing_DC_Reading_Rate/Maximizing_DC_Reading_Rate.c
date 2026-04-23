/******************************************************************************/
/*                                                                            */
/* National Instruments - NI-DMM - C Examples                                 */
/*                                                                            */
/* Maximizing DC Reading Rate                                                 */
/*                                                                            */
/* This example demonstrates how to maximize DC reading rates for             */
/* National Instruments Multimeters by specifying the aperture time.          */
/* Generally NI-DMM selects an aperture to achieve the specified resolution,  */
/* but in this example, you directly set the aperture time.                   */
/* The acquired measurements are then used to calculate the noise-free digits */
/* of resolution.                                                             */
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

/****************************************************\

        Recommended Aperture Time Settings

Function / Range  |  Resolution  |  Aperture Time (s)
                  |  (digits)    |
-----------------------------------------------------
DCV / 1V             6             10m
                     5             300u
                     4             20u

DCV / 10V            6             3.33m
                     5             200u
                     4             20u

DCV / 100V           6             10m
                     5             350u
                     4             50u

DCV / 300V           6             10m
                     5             500u
                     4             50u
-----------------------------------------------------

Res / 1kohm          6             10m
                     5             300u
                     4             45u

Res / 10kohm         6             10m
                     5             300u
                     4             45u

Res / 100kohm        6             10m
                     5             300u
                     4             45u

Res / 1Mohm          6             10m
                     5             300u
                     4             45u

Res / 10Mohm         6             10m
                     5             300u
                     4             45u

Res / 100Mohm        6             10m
                     5             300u
                     4             45u

\****************************************************/

#define MAXPTSTOREAD 5000
#define MAXPTSTOANALYZE 100

#include "nidmm.h"
#include <stdio.h>
#include <string.h>
#include <math.h>
#include <malloc.h>

ViBoolean  RequestToStop = VI_FALSE;

ViReal64 CalculateResolutionFromData (ViReal64 range,ViReal64 measurements[],
                                      ViInt32 measurementsIndex, ViInt32 totalPointsAcquired);

void StdDev (ViReal64* samples, ViInt32 sampleSize, ViReal64 *measurementMean, ViReal64 *measurementStdDev);

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
   ViReal64  apertureTime        = 20.0e-6;

   ViReal64  startTime           = 0.0;
   ViReal64  currentTime         = 0.0;
   ViReal64  timeInterval        = 0.0;
   ViInt32   readingRate         = 0;
   ViReal64  effectiveResolution = 0.0;
   ViReal64  measurementPeriod   = 0.0;
   ViInt32   minNumPointsToRead  = 0;
   ViInt32   totalPointsAcquired = 0;
   ViInt32   numPointsRead       = 0;
   ViInt32   ptsAvailable        = 0;
   ViInt16   acqStatus           = 0;
   ViInt32   dcNoiseRejection    = NIDMM_VAL_DCNR_NORMAL;

   ViReal64  measurements[MAXPTSTOREAD] = {0.0};
   ViInt32   measurementsIndex     = 0;
   ViReal64  currentMeasurement  = 0.0;
   ViInt32   effectiveSize       = 0;

   ViChar    modeString[20]      = "";
   ViChar    rangeString[20]     = "";
   ViChar    dataString[20]      = "";

   /*- Ask user to specify device --------------------------------------*/
   promptForResourceName(resourceName);

   /*- Initialize the DMM and create a new session --------------------------*/
   checkErr(niDMM_init(resourceName, idQuery, reset, &vi));

   /*- Configure measurement attributes -------------------------------------*/
   checkErr(niDMM_SetAttributeViInt32 (vi, "", NIDMM_ATTR_FUNCTION, measurementType));

   checkErr(niDMM_SetAttributeViReal64 (vi, "", NIDMM_ATTR_RANGE, range));

   /*- Set sample count to zero to do continous measurements-----------------*/
   checkErr(niDMM_SetAttributeViInt32 (vi, "", NIDMM_ATTR_SAMPLE_COUNT, 0));

   /*- Set reading rate enhancing attributes --------------------------------*/
   checkErr(niDMM_SetAttributeViInt32 (vi, "", NIDMM_ATTR_APERTURE_TIME_UNITS, NIDMM_VAL_SECONDS));

   checkErr(niDMM_SetAttributeViReal64 (vi, "", NIDMM_ATTR_APERTURE_TIME, apertureTime));

   /*- Set reading rate enhancing attributes --------------------------------*/
   checkErr(niDMM_SetAttributeViInt32 (vi, "", NIDMM_ATTR_DC_NOISE_REJECTION, dcNoiseRejection));

   /*- Find out measurement rate so we can report it -*/
   checkErr(niDMM_GetMeasurementPeriod(vi,&measurementPeriod));

   /*- Wait for 1/20 of a second's worth of data before fetching as long as the
       number is between 2 and 1000 points -----------------------------------*/
   minNumPointsToRead = (ViInt32) (1 / (20 * apertureTime));
   if ( minNumPointsToRead < 2 )
      minNumPointsToRead = 2;
   else if ( minNumPointsToRead > 1000 )
      minNumPointsToRead = 1000;

   /*- Initiate -------------------------------------------------------------*/
   checkErr(niDMM_Initiate (vi));

   RequestToStop = VI_FALSE;

   while(!RequestToStop)
   {
      /*- Make sure there are enough points to achieve high reading rate -------- */
      checkErr(niDMM_ReadStatus (vi, &ptsAvailable, &acqStatus));

      /*- Make sure a predefined least number of point are available -------------*/
      if(ptsAvailable >= minNumPointsToRead)
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
               checkErr(niDMM_FetchMultiPoint(vi, NIDMM_VAL_TIME_LIMIT_AUTO, effectiveSize , &measurements[measurementsIndex], &numPointsRead));

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
         checkErr(niDMM_FetchMultiPoint(vi, NIDMM_VAL_TIME_LIMIT_AUTO, ptsAvailable, &measurements[measurementsIndex], &numPointsRead));

         currentMeasurement = measurements[measurementsIndex];

         /*- Update the total numbers of points acquired -------------------------*/
         totalPointsAcquired += numPointsRead;

         measurementsIndex += numPointsRead;

         effectiveResolution = CalculateResolutionFromData(range , measurements, measurementsIndex, totalPointsAcquired);

         checkErr(niDMM_FormatMeas(measurementType, range, effectiveResolution,
                  currentMeasurement, modeString, rangeString, dataString));
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
      printf("sample rate = %5.2f\n",1.0/measurementPeriod);
      printf("modeString = %s\n", modeString);
      printf("rangeString = %s\n", rangeString);
      printf("dataString = %s\n", dataString);
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

ViReal64 CalculateResolutionFromData (ViReal64 range, ViReal64 measurements[],
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
