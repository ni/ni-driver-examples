/*****************************************************************************/
/* National Instruments Function Generator                                   */
/* Basic Arbitrary Waveform Example source file                              */
/*                                                                           */
/* National Instruments, Austin Texas                                        */
/* PH. (800)433-3488   Fax (512)794-5678                                     */
/* Original Release: 4-99                                                    */
/*                                                                           */
/* Modification History:                                                     */
/*     Date    Initials  Description                                         */
/*     4-99    BC        Created                                             */
/*****************************************************************************/
#include "niFgen.h"
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <math.h>

#define WFM_SIZE 64

int main(int argc, char *argv[]) {
   ViChar Resource[256];
   const ViChar * ChannelName = "0";
   ViReal64 SampleRate, Gain, DCOffset;
   ViStatus error = VI_SUCCESS;
   ViSession vi=VI_NULL;
   ViInt32 i;
   ViInt32 wfmHandle;
   ViReal64 sine[WFM_SIZE];

   if(argc == 1) {
      /*- Prompt for parameters -----------------------------------------------*/
      #define BUFSIZE 256
      ViChar inputLine[BUFSIZE];

      // set default values
      strcpy(Resource, "PXI1Slot2");
      SampleRate = 40e+6;
      Gain = 1.0;
      DCOffset = 0.0;

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

      printf("DC Offset (%lf): ", DCOffset);
      if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%lf", &DCOffset);

      // Print newline(s) to visually separate output from input
      printf("\n\n");
   }
   else if(argc == 5) {
      /*- Get parameters from the command line ------------------------------*/
      strcpy(Resource, argv[1]);
      SampleRate = strtod(argv[2], NULL);
      Gain = strtod(argv[3], NULL);
      DCOffset = strtod(argv[4], NULL);
   }
   else {
      /*- Show usage --------------------------------------------------------*/
      printf("Usage: %s <resource> <sample rate> <gain> <dc offset>\n", argv[0]);
      return -1;
   }

   /*- Create some waveform data --------------------------------------------*/
   // Sine:
   for (i = 0; i < WFM_SIZE; i++)
       sine[i] = sin(((ViReal64)i/WFM_SIZE)*2*3.141596);   
  
   /*- Initialize the session -----------------------------------------------*/
   printf("Initializing %s\n", Resource);
   checkErr(niFgen_init(Resource, VI_TRUE, VI_TRUE, &vi));

   /*- Configure the active channels for the session ------------------------*/
   checkErr(niFgen_ConfigureChannels(vi, "0"));

   /*- Configure the device for arb mode ------------------------------------*/  
   checkErr(niFgen_ConfigureOutputMode(vi, NIFGEN_VAL_OUTPUT_ARB));

   /*- Create the waveform and download the data ----------------------------*/
   checkErr(niFgen_CreateArbWaveform(vi, WFM_SIZE, sine, &wfmHandle)); 

   /*- Select the waveform to generate --------------------------------------*/
   checkErr(niFgen_ConfigureArbWaveform(vi, ChannelName, wfmHandle, Gain,
                                        DCOffset)); 

   /*- Configure the sample clock -------------------------------------------*/
   checkErr(niFgen_ConfigureClockMode(vi, NIFGEN_VAL_HIGH_RESOLUTION));
   checkErr(niFgen_ConfigureSampleRate(vi, SampleRate));

   /*- Enable output and generate  ------------------------------------------*/
   checkErr(niFgen_ConfigureOutputEnabled(vi, ChannelName, VI_TRUE));
   checkErr(niFgen_InitiateGeneration(vi));

   printf("Generating sine wave at %lf Hz\n", SampleRate);

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

