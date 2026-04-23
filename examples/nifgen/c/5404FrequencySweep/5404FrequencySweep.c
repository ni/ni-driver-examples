/*
 * Example of using FGEN with the 5404 to produce a frequency sweep.
 * A logarithmic scale is used to determine the frequencies to sweep over.
 * 7-10-2002
 */

#include "niFgen.h"
#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <math.h>
#include <time.h>

int main(int argc, char* argv[]) {

   ViStatus error = VI_SUCCESS;
   ViSession instrHandle = VI_NULL;

   ViChar resource[256];
   ViReal64 amplitude, startFreq, stopFreq, logStart, logFactor, freqAry[512];
   int numSteps, i;
   ViReal64 timeDelayInMilliseconds, timeDelayInSeconds, elapsedCPUTimeInSeconds;
   clock_t startTime;

   // user gave comman line arguments, but not the correct number
   // output usage message
   if ((argc != 1) && (argc != 7)) {
      printf("usage: %s <resource> <amplitude (Vp-p> <starting frequency>\
         <stopping frequency> <num of steps> <time delay between each step (ms)>", argv[0]);
      return -1;
   }

   // correct number of arguments (hopefully in correct order)
   // assign arguments to appropriate variables
   if (argc == 7) {
      strcpy(resource, argv[1]);
      amplitude = strtod(argv[2], NULL);
      startFreq = strtod(argv[3], NULL);
      stopFreq = strtod(argv[4], NULL);
      numSteps = atoi(argv[5]);
      timeDelayInMilliseconds = strtoul(argv[6], NULL, 10);
   }

   printf("\n\n-- frequency sweep example --\n");

   // prompt for and read in necessary variables (if not given in command line)
   if (argc == 1) {
      #define BUFSIZE 256
      ViChar inputLine[BUFSIZE];

      // set default values
      strcpy(resource, "PXI1Slot2");
      amplitude = 2.0;
      startFreq = 1E+6;
      stopFreq = 10E+6;
      numSteps = 10;
      timeDelayInMilliseconds = 100.0;

      // Optionally override defaults from the command line.
      // If user simply hits enter, inputLine is an empty string, and sscanf will not
      // update its target variable (so the default value is preserved)

      printf("\nSpecify Inputs (Enter to accept default)\n");

      printf("Resource Name (%s): ", resource);
      if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%s", resource);

      printf("Amplitude in Vp-p (%lf): ", amplitude);
      if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%lf", &amplitude);

      printf("Start Frequency in Hz (%lf): ", startFreq);
      if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%lf", &startFreq);

      printf("Stop Frequency in Hz (%lf): ", stopFreq);
      if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%lf", &stopFreq);

      printf("Number of Frequency Steps (%d): ", numSteps);
      if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%d", &numSteps);

      printf("Duration in ms / Step (%lf): ", timeDelayInMilliseconds);
      if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%lf", &timeDelayInMilliseconds);
   }

   // initialize a session with the 5404.
   printf("Initializing %s... ", resource);
   checkErr(niFgen_init(resource, VI_TRUE, VI_TRUE, &instrHandle));
   printf("Done\n");

   printf("Calculating array...");
   logStart = log10(startFreq);
   logFactor = (log10(stopFreq) - logStart) / numSteps;
   for (i = 0; i<=numSteps; i++)
      freqAry[i] = pow(10, logFactor * i + logStart);
   printf("Done\n");

   // configure a sine wave with the amplitude and the starting frequency given.
   // InitiateGeneration() actually starts the generation of the wave 
   // and the output must be turned on (ConfigureOutputEnabled() with a VI_TRUE argument)
   // in order for the board to actually output the signal
   checkErr(niFgen_ConfigureStandardWaveform(instrHandle, "0",
                                             NIFGEN_VAL_WFM_SINE, amplitude,
                                             0.00, startFreq, 0.00));
   checkErr(niFgen_InitiateGeneration(instrHandle));
   checkErr(niFgen_ConfigureOutputEnabled(instrHandle, "0", VI_TRUE));

   // step through the frequency array and delay before proceeding
   // to the next interation
   printf("Sweeping...");
   timeDelayInSeconds = timeDelayInMilliseconds / 1000.0;
         
   for (i=0; i<=numSteps; i++) {
      checkErr(niFgen_SetAttributeViReal64(instrHandle, "",
                                           NIFGEN_ATTR_FUNC_FREQUENCY, freqAry[i]));
                                           
      // The clock() function, which returns a process's CPU time, is used here only for 
      // portability. Replace the clock() function with a timing mechanism appropriate 
      // to your operating system and needs (for example, a sleeping function for long 
      // delays or a high-precision timer for short delays).
      
      startTime = clock();      
      do
         elapsedCPUTimeInSeconds = ((double) (clock() - startTime)) / CLOCKS_PER_SEC;
      while (elapsedCPUTimeInSeconds < timeDelayInSeconds);
                  
   }
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

