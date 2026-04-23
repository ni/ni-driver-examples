/*
 * Example of using FGEN with the 5404 to produce a sine wave
 * 6-19-2002
 */
#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include "niFgen.h"

int main(int argc, char* argv[]) {

   ViChar resource[256];
   ViReal64 frequency, amplitude, startPhase;
   ViStatus error = VI_SUCCESS;
   ViSession instrHandle = VI_NULL;

   // user gave command line arguments, but not the correct number
   // output usage message
   if ((argc != 1) && (argc != 5)) {
      printf("usage: %s <resource> <frequency (Hz)> <amplitude (Vp-p)> <start phase>", argv[0]);
      return -1;
   }

   // correct number of arguments (hopefully in correct order)
   // assign arguments to appropriate variables
   if (argc == 5) {
      strcpy(resource, argv[1]);
      frequency = strtod(argv[2], NULL);
      amplitude = strtod(argv[3], NULL);
      startPhase = strtod(argv[4], NULL);
   }

   printf("\n\n-- sine wave generation example --\n");

   // prompt for and read in necessary waveform attributes
   if (argc == 1) {
      #define BUFSIZE 256
      ViChar inputLine[BUFSIZE];

      // set default values
      strcpy(resource, "PXI1Slot2");
      frequency = 40E+6;
      amplitude = 2.0;
      startPhase = 0.0;

      // Optionally override defaults from the command line.
      // If user simply hits enter, inputLine is an empty string, and sscanf will not
      // update its target variable (so the default value is preserved)

      printf("\nSpecify Inputs (Enter to accept default)\n");

      printf("Resource Name (%s): ", resource);
      if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%s", resource);

      printf("Frequency in Hz (%lf): ", frequency);
      if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%lf", &frequency);

      printf("Amplitude in Vp-p (%lf): ", amplitude);
      if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%lf", &amplitude);

      printf("Start Phase in degrees (%lf): ", startPhase);
      if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%lf", &startPhase);
   }
   
   // initialize a session with the 5404.
   printf("Initializing %s... ", resource);
   checkErr(niFgen_init(resource, VI_TRUE, VI_TRUE, &instrHandle));
   printf("Done\n");
   
   // configure a sine wave with given attributes.  InitiateGeneration() actually starts the generation
   // of the wave and the output must be turned on (ConfigureOutputEnabled() with a VI_TRUE argument)
   // in order for the board to actually output the signal
   printf("Generating sine wave... ");
   checkErr(niFgen_ConfigureStandardWaveform (instrHandle, VI_NULL, NIFGEN_VAL_WFM_SINE, amplitude, 0, frequency, startPhase));
   checkErr(niFgen_InitiateGeneration (instrHandle));
   checkErr(niFgen_ConfigureOutputEnabled(instrHandle, "0", VI_TRUE));
   printf("Done\n");

   // error handler function that the checkErr macro directs control to if error != 0 after a niFgen function call
Error:
   if (error != VI_SUCCESS) {
      ViChar errorMessage[512];
      niFgen_ErrorHandler(instrHandle, error, errorMessage);
      printf("\n\nError %x: %s\n", error, errorMessage);
   }

   // close the session before termination of program
   if (instrHandle != VI_NULL)
      niFgen_close(instrHandle);

   return 0;
}
