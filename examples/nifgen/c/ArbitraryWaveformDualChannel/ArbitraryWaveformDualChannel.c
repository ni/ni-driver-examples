/*****************************************************************************/
/* National Instruments Function Generator                                   */
/* Arbitrary Waveform Example source file                                    */
/*                                                                           */
/* National Instruments, Austin Texas                                        */
/* PH. (800)433-3488   Fax (512)794-5678                                     */
/* Original Release: 4-08                                                    */
/*                                                                           */
/* Modification History:                                                     */
/*     Date    Initials  Description                                         */
/*     4-08    LD        Created                                             */
/*     4-17    BA        Write Waveforms Interleaved                         */
/*****************************************************************************/
#include "niFgen.h"
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <math.h>

#define WFM_SIZE 256

int main(int argc, char *argv[]) {
   ViChar Resource[256];
   const ViChar * ChannelName = "0,1";
   ViReal64 SampleRate, Gain, ActualSampleRate;
   ViReal64 DCOffset = 0.0;
   ViInt32  SampleClock;
   ViChar SampleClockSource[256];
   ViStatus error = VI_SUCCESS;
   ViSession vi=VI_NULL;
   ViInt32 wfmHandle;
   ViReal64 sineWfm[WFM_SIZE];
   ViReal64 squareWfm[WFM_SIZE];
   ViReal64 interleavedWfm[WFM_SIZE * 2];
   ViInt32 i;

   if(argc == 1) {
      /*- Prompt for parameters -----------------------------------------------*/
      #define BUFSIZE 256
      ViChar inputLine[BUFSIZE];

      // set default values
      strcpy(Resource, "PXI1Slot2");
      SampleRate = 20e+6;
      Gain = 0.5;
      SampleClock = 0;

      // Optionally override defaults from the command line.
      // If user simply hits enter, inputLine is an empty string, and sscanf will not
      // update its target variable (so the default value is preserved)

      printf("\nSpecify Inputs (Enter to accept default)\n");

      printf("Resource Name (%s): ", Resource);
      if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%s", Resource);

      printf("Sample Rate in Hz (%lf): ", SampleRate);
      if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%lf", &SampleRate);

      printf("Gain (%lf): ", Gain);
      if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%lf", &Gain);

      printf("Sample Clock Source {0=internal | 1=external} (%d): ", SampleClock);
      if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%d", &SampleClock);

      // Print newline(s) to visually separate output from input
      printf("\n\n");
   }
   else if(argc == 5) {
      /*- Get parameters from the command line ------------------------------*/
      strcpy(Resource, argv[1]);
      SampleRate = strtod(argv[2], NULL);
      Gain = strtod(argv[3], NULL);
      SampleClock = strtol(argv[4], NULL, 0);
   }
   else {
      /*- Show usage --------------------------------------------------------*/
      printf("Usage: %s <resource> <sample rate> <gain>"
             " <sample clock source (1=external, 0=internal)>\n", argv[0]);
      return -1;
   }


   /*- Convert input to constants -------------------------------------------*/
   if (SampleClock == 0) strcpy(SampleClockSource, "OnboardClock");
   else strcpy(SampleClockSource, "External");

   /*- Create some waveform data --------------------------------------------*/
   // Sinewave:
   for(i = 0; i < WFM_SIZE; ++i)
       sineWfm[i] = sin(((ViReal64)i/WFM_SIZE)*2*3.141596);

   // Squarewave:
   for(i = 0; i < WFM_SIZE / 2; ++i)
      squareWfm[i] = 1.0;
   for(i = WFM_SIZE/2; i < WFM_SIZE; ++i)
      squareWfm[i] = -1.0;

   // Interleave the waveform data
   for(i = 0; i < WFM_SIZE; ++i)
   {
      // sine on ch0, square on ch1
      interleavedWfm[i * 2] = sineWfm[i];
      interleavedWfm[i * 2 + 1] = squareWfm[i];
   }

   /*- Initialize the session -----------------------------------------------*/
   printf("Initializing %s\n", Resource);
   checkErr(niFgen_init(Resource, VI_TRUE, VI_TRUE, &vi));

   /*- Configure the device for arb mode ------------------------------------*/
   checkErr(niFgen_ConfigureOutputMode(vi, NIFGEN_VAL_OUTPUT_ARB));

   /*- Allocate the waveform buffer. */
   checkErr(niFgen_AllocateWaveform(vi, ChannelName, WFM_SIZE, &wfmHandle));

   /*- Write the waveform for both channels, interleaved */
   checkErr(niFgen_WriteWaveform(vi, ChannelName, wfmHandle, WFM_SIZE * 2, interleavedWfm));

   /*- Select the waveform handle to generate --------------------------------------*/
   checkErr(niFgen_ConfigureArbWaveform(vi, ChannelName, wfmHandle, Gain,
                                        DCOffset));

   /*- Configure the sample clock -------------------------------------------*/
   checkErr(niFgen_ConfigureSampleClockSource(vi, SampleClockSource));
   checkErr(niFgen_ConfigureSampleRate(vi, SampleRate));

   /*- Get the coerced sample rate (coerced in divide down clock mode only) -*/

   checkErr(niFgen_GetAttributeViReal64 (vi, VI_NULL,
                                         NIFGEN_ATTR_ACTUAL_ARB_SAMPLE_RATE,
                                         &ActualSampleRate));

   /*- Enable output and generate  ------------------------------------------*/
   checkErr(niFgen_ConfigureOutputEnabled(vi, ChannelName, VI_TRUE));
   checkErr(niFgen_InitiateGeneration(vi));

   printf("Generating at %lf Hz\n", ActualSampleRate);

   // clear the input buffer, then wait for user input before closing the session.
   // (DAQmx devices will quit generating the output when the session is closed).
   fflush( stdin );
   printf("Press Enter to continue...\n");
   getchar();

Error:
   /*- Process any errors ---------------------------------------------------*/
   if(error != VI_SUCCESS) {
      ViChar errMsg[256];
      niFgen_ErrorHandler(vi, error, errMsg);
      printf("Error %x: %s\n", error, errMsg);
   }

   /*- Close the session ----------------------------------------------------*/
   if (vi) niFgen_close (vi);
   return 0;
}

