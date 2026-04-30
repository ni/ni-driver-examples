/************************************************************************
*
*  Example Program:
*    DeviceDynamicAcqAndGen-SourceSynchronous.c
*
*  Description:
*    This VI demonstrates how to acquire response data from a device
*    under test (DUT) and compare that data to an expected response
*    waveform using the NI-HSDIO hardware comparison features.
*
*
*  Pin Connection Information:
*    The configuraton of this example assumes that an external DUT
*    is providing an 8-bit count-up pattern and a clock. The eight bits
*    should be connected to channels 0-7, and the clock signal should
*    be connected to the STROBE terminal on the Digital Data & Control
*    connector.
*
************************************************************************/


/* Includes */
#include <stdio.h>
#include <stdlib.h>
#include "niHSDIO.h"

/* These two functions were created simply to make the main program easier to read */
ViStatus setupGenerationDevice (ViRsrc genDeviceID, ViConstString genChannelList, ViReal64 maxClockRate,
                        ViConstString waveformName, ViInt32 waveformSize, ViUInt8 *waveformBuffer, ViSession *genViPtr);

ViStatus setupAcquisitionDevice (ViRsrc acqDeviceID, ViConstString acqChannelList, ViConstString sampleClockSource,
                                 ViReal64 expectedDutClockRate, ViInt32 waveformSize, ViSession *acqViPtr);

ViStatus createHardwareCompareWaveform (ViInt32 waveformSize, ViInt32 numberOfGenChannels,
                           ViInt32 numberOfIntroducedErrors, ViUInt8 *waveformBuffer);


#define NUM_CHANNELS 8
#define WFM_SIZE 1024
int main(void)
{
   ViUInt8 waveformBuffer[WFM_SIZE * NUM_CHANNELS];
   ViInt32 numberOfIntroducedErrors = 0;
   ViStatus error = VI_SUCCESS;
   ViChar errDesc[1024];
   ViInt32 numSampleErrors;

   ViRsrc deviceID = "PXI1Slot2";
   /* NUM_CHANNELS should map to this list */
   ViConstString channelList = "0-7";

   /* Generation */
   ViReal64 maxSampleClockRate = 50.0e6;
   ViConstString waveformName = "myWfm";
   ViSession genVi = VI_NULL;

   /* Acquisition */
   ViReal64 expectedDutClockRate = 50.0e6;
   ViConstString sampleClockSource = NIHSDIO_VAL_STROBE_STR;
   ViInt32 msReadTimeout = 10000;
   ViSession acqVi = VI_NULL;

   /* Create the expected response waveform. Assuming the DUT is returning
      a count-up pattern on channels 0-7 */


   checkErr(createHardwareCompareWaveform (WFM_SIZE, NUM_CHANNELS,
                                           numberOfIntroducedErrors, waveformBuffer));

   /* Initialize, configure, and write compare waveform to generation device */
   checkErr(setupGenerationDevice (deviceID, channelList, maxSampleClockRate,
                                   waveformName, WFM_SIZE, waveformBuffer, &genVi));

   /* Initialize and Configure Acquisition Sessions */
   checkErr(setupAcquisitionDevice  (deviceID, channelList, sampleClockSource,
                                     expectedDutClockRate, WFM_SIZE, &acqVi));

   /* First initialize the Generation session */
   checkErr(niHSDIO_Initiate (genVi));

   /* Second, initialize the Acquisition session */
   checkErr(niHSDIO_Initiate (acqVi));

   /* Wait for acquisition and generation to finish */
   checkErr(niHSDIO_WaitUntilDone (acqVi, msReadTimeout));

   /* Get the number of sample errors */
   checkErr(niHSDIO_GetAttributeViInt32 (acqVi, "", NIHSDIO_ATTR_HWC_NUM_SAMPLE_ERRORS, &numSampleErrors));



Error:

   if (error == VI_SUCCESS)
   {
      /* print result */
      printf("Done.\n");
      printf("Number of comparison sample errors = %d.\n", numSampleErrors);
   }
   else
   {
      printf("Error encountered\n===================\n");

      /*  Get errors from NI-HSDIO sessions. Calling niHSDIO_GetError returns
          the error and clears the error for that session. */

      niHSDIO_GetError(genVi, &error, sizeof(errDesc)/sizeof(ViChar), errDesc);
      if (error != VI_SUCCESS)
         printf("Device ID: %s\nError Description: %s\n", deviceID, errDesc);

      niHSDIO_GetError(acqVi, &error, sizeof(errDesc)/sizeof(ViChar), errDesc);
      if (error != VI_SUCCESS)
         printf("Device ID: %s\nError Description: %s\n", deviceID, errDesc);

   }

   /* close the sessions */
   niHSDIO_close(genVi);

   niHSDIO_close(acqVi);

   /* prompt to exit (for pop-up console windows) */
   printf("\nHit <Enter> to continue...\n");
   getchar();

   return error;
}

ViStatus setupGenerationDevice(ViRsrc genDeviceID, ViConstString genChannelList, ViReal64 maxClockRate,
                               ViConstString waveformName, ViInt32 waveformSize, ViUInt8 *waveformBuffer,
                               ViSession *genViPtr)
{

   ViStatus error = VI_SUCCESS;
   ViSession genVi = VI_NULL;

   /* Initialize generation session */
   checkErr(niHSDIO_InitGenerationSession (
            genDeviceID, VI_FALSE, VI_FALSE, VI_NULL, &genVi));

   /* Assign channels for dynamic generation */
   checkErr(niHSDIO_AssignDynamicChannels (genVi, genChannelList));

   /* Configure Sample clock */
   checkErr(niHSDIO_ConfigureSampleClock (
            genVi, NIHSDIO_VAL_ON_BOARD_CLOCK_STR, maxClockRate));

   /* Enable H, L and X to use hardware compare */
   checkErr(niHSDIO_SetAttributeViInt32 (
            genVi, "", NIHSDIO_ATTR_SUPPORTED_DATA_STATES,
            NIHSDIO_VAL_STATES_L_H_X));

   /* Waveform should contain only compare data */
   checkErr(niHSDIO_WriteNamedWaveformWDT (
            genVi, waveformName, waveformSize, NIHSDIO_VAL_GROUP_BY_SAMPLE, waveformBuffer));

Error:

   *genViPtr = genVi;

   return error;
}


ViStatus setupAcquisitionDevice (ViRsrc acqDeviceID, ViConstString acqChannelList, ViConstString sampleClockSource,
                                 ViReal64 expectedDutClockRate, ViInt32 waveformSize, ViSession *acqViPtr)
{
   ViStatus error = VI_SUCCESS;
   ViSession acqVi = VI_NULL;

   /* Initialize acquisition session */
   checkErr(niHSDIO_InitAcquisitionSession(
            acqDeviceID, VI_FALSE, VI_FALSE, VI_NULL, &acqVi));

   /* Assign channels for dynamic acquisition */
   checkErr(niHSDIO_AssignDynamicChannels (acqVi, acqChannelList));

   /* Configure clocking parameters */
   checkErr(niHSDIO_ConfigureSampleClock(
            acqVi, sampleClockSource, expectedDutClockRate));

   /* Cofigure the data to be clocked on the falling edge of the Sample clock */
   checkErr(niHSDIO_ConfigureDataPosition (
            acqVi, acqChannelList, NIHSDIO_VAL_SAMPLE_CLOCK_FALLING_EDGE));

   /* Configure the Start trigger to trigger on a pattern match */
   checkErr(niHSDIO_ConfigurePatternMatchStartTriggerU32 (
            acqVi, acqChannelList, 0, NIHSDIO_VAL_PATTERN_MATCHES));

   /* Enable H, L and X to use hardware compare */
   checkErr(niHSDIO_SetAttributeViInt32 (
            acqVi, "", NIHSDIO_ATTR_SUPPORTED_DATA_STATES,
            NIHSDIO_VAL_STATES_L_H_X));

   /* Commit acquisition settings to hardware */
   checkErr(niHSDIO_CommitDynamic( acqVi ));

Error:

   *acqViPtr = acqVi;

   return error;

}



// Creates an expected response waveform.
ViInt32 createHardwareCompareWaveform ( ViInt32 waveformSize,
                              ViInt32 numberOfGenChannels,
                              ViInt32 numberOfIntroducedErrors,
                              ViUInt8 *waveformBuffer )
{
   int sampleNumber, signalNumber, currentError;
   int bitValue;
   int bitOffset, errorOffset;
   unsigned int *countPattern;
   ViStatus error = VI_SUCCESS;
   /*
   Create Waveform:
      + Create count-up pattern as raw binary
      + Convert raw binary to waveform data type. conveting 0,1,Z values to L,H,X values.
      + Insert known number of errors to data (some H and L values will be toggled)
   */
   if(waveformBuffer == VI_NULL)
   {
      checkErr(-1);
   }

   countPattern = malloc(sizeof(unsigned int)*waveformSize);

   /* Create raw count pattern */
   for (sampleNumber = 0; sampleNumber < waveformSize; sampleNumber++)
      countPattern[sampleNumber] = sampleNumber;

   /* Conver to digital waveform and add compare data */
   for(sampleNumber = 0; sampleNumber < waveformSize; sampleNumber++)
   {
      for(signalNumber = 0; signalNumber < numberOfGenChannels; signalNumber++)
      {
         /* Get location in waveform for current bit */
         bitOffset = sampleNumber * numberOfGenChannels + signalNumber;

         /* Get value of current bit and store as 0 or 1 in waveform */
         waveformBuffer[bitOffset]
            = ((countPattern[sampleNumber] & (1 << signalNumber)) >> signalNumber) + 3;
      }
   }

   /* Introduce errors to the waveform */
   if (numberOfIntroducedErrors > 0)
   {
      errorOffset = (waveformSize * numberOfGenChannels) / numberOfIntroducedErrors;


      for(currentError = 0; currentError < numberOfIntroducedErrors; currentError++)
      {
         bitValue = waveformBuffer[errorOffset * currentError];

         if(bitValue == NI_DIO_1 || bitValue == NI_DIO_H)
            bitValue--;
         else
            bitValue++;

         waveformBuffer[errorOffset * currentError] = bitValue;
      }
   }

Error:
   free(countPattern);
   return error;
}
