/*
 * Example of using FGEN to synchronize two 5404 boards
 * 6-19-2002
 */

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include "niFgen.h"

int main(int argc, char *argv[]) {

   ViChar resource1[256], resource2[256];
   ViReal64 frequency, amplitude, phase2;
   ViStatus error = VI_SUCCESS;
   ViSession instrHandle1 = VI_NULL;
   ViSession instrHandle2 = VI_NULL;

   // user gave command line arguments, but not the correct number
   // output usage message
   if ((argc != 1) && (argc != 6)) {
      printf("usage: %s <resource 1> <resource 2> <frequency (Hz)> <amplitude> <start phase>\n", argv[0]);
      return -1;
   }

   // correct number of arguments (hopefully in correct order) given
   // assign arguments to appropriate variables
   if (argc == 6) {
      strcpy(resource1, argv[1]);
      strcpy(resource2, argv[2]);
      frequency = strtod(argv[3], NULL);
      amplitude = strtod(argv[4], NULL);
      phase2 = strtod(argv[5], NULL);
   }

   printf("\n\n-- synchronization example --\n");

   // prompt for and read in necessary waveform attributes if not given in command line
   if (argc == 1) {
      #define BUFSIZE 256
      ViChar inputLine[BUFSIZE];

      // set default values
      strcpy(resource1, "PXI1Slot2");
      strcpy(resource2, "PXI1Slot3");
      frequency = 40e+6;
      amplitude = 2.0;
      phase2 = 0.0;

      // Optionally override defaults from the command line.
      // If user simply hits enter, inputLine is an empty string, and sscanf will not
      // update its target variable (so the default value is preserved)

      printf("\nSpecify Inputs (Enter to accept default)\n");

      printf("Resource 1 (%s): ", resource1);
      if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%s", resource1);

      printf("Resource 2 (%s): ", resource2);
      if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%s", resource2);

      printf("Both Devices Frequency in Hz (%lf): ", frequency);
      if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%lf", &frequency);

      printf("Both Devices Amplitude in Vp-p (%lf): ", amplitude);
      if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%lf", &amplitude);

      printf("Resource 2 Start Phase in degrees (%lf): ", phase2);
      if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%lf", &phase2);
   }

   // initialize sessions on each of the two 5404 boards.
   printf("Initializing device 1 (%s)... ", resource1);
   checkErr(niFgen_init(resource1, VI_TRUE, VI_TRUE, &instrHandle1));
   printf("Done\n");

   printf("Initializing device 2 (%s)... ", resource2);
   checkErr(niFgen_init(resource2, VI_TRUE, VI_TRUE, &instrHandle2));
   printf("Done\n");

   // configure the boards with sine waves each with the same given frequency and amplitude.  Device 2
   // also gets initialized with a starting phase value.
   printf("Configuring two sine waves... ");
   checkErr(niFgen_ConfigureStandardWaveform(instrHandle1, VI_NULL, NIFGEN_VAL_WFM_SINE, amplitude, 0, frequency, 0.00));
   checkErr(niFgen_ConfigureStandardWaveform(instrHandle2, VI_NULL, NIFGEN_VAL_WFM_SINE, amplitude, 0, frequency, phase2));
   printf("Done\n");

   // the following instructions are used to synchronize the two boards.
   // first, configure the reference clock source for each board
   printf("Synchronizing reference clocks and triggers... ");
   checkErr(niFgen_ConfigureReferenceClock(instrHandle1, "PXI_Clk10", 10e6));
   checkErr(niFgen_ConfigureReferenceClock(instrHandle2, "PXI_Clk10", 10e6));
   

   // device 1 is set to start on a software trigger.  This trigger is also routed to RTSI 0
   // and the second device is told to wait for a trigger on this line (RTSI 0)
   checkErr(niFgen_ConfigureSoftwareEdgeStartTrigger(instrHandle1));
   checkErr(niFgen_ExportSignal(instrHandle1, NIFGEN_VAL_START_TRIGGER, VI_NULL, "RTSI0"));
   checkErr(niFgen_ConfigureDigitalEdgeStartTrigger(instrHandle2, "RTSI0", NIFGEN_VAL_RISING_EDGE));
   printf("Done\n");

   // This would normally cause the boards to start outputting the sine waves.  But because of
   // the triggers, it doesn't.  Here we set everything up so that they're both just waiting on the trigger.
   printf("Generating waves... ");
   checkErr(niFgen_InitiateGeneration(instrHandle2));
   checkErr(niFgen_ConfigureOutputEnabled(instrHandle2, "0", VI_TRUE));
   checkErr(niFgen_InitiateGeneration(instrHandle1));
   checkErr(niFgen_ConfigureOutputEnabled(instrHandle1, "0", VI_TRUE));
   printf("Done\n");

   // once the trigger is fired, both boards start outputting at the same time.
   printf("Firing trigger... ");
   checkErr(niFgen_SendSoftwareEdgeTrigger(instrHandle1, NIFGEN_VAL_START_TRIGGER, VI_NULL));
   printf("Done\n");

   /**
    * NOTE TO USER:
    *    At this point, in order to synchronize correctly, to bring the two waveforms in or out of phase,
    * it would be necessary to add an on-the-fly correction for phase shifts.  By changing the phase of
    * the second device (remember, we're only in control of the second one), we can alter the phase difference
    * of the two waveforms.
    */
   printf("\nNOTE TO USER:\nAt this point, in order to synchronize correctly, to bring the \
two waveforms in or out of phase, it would be necessary to add an on-the-fly correction \
for phase shifts.  By changing the phase of the second device (remember, we're only in \
control of the second one), we can alter the phase difference of the two waveforms.\n");

Error:
   // error handler function that the checkErr macro directs control to when error != 0 after a niFgen function call
   if (error != VI_SUCCESS) {
      ViChar errorMessage[512];
      niFgen_ErrorHandler(instrHandle1, error, errorMessage);
      if (strcmp(errorMessage, ""))
         niFgen_ErrorHandler(instrHandle2, error, errorMessage);
      printf("\n\nError %x: %s\n", error, errorMessage);
   }

   // close all sessions before termination of program
   if (instrHandle1 != VI_NULL)
      niFgen_close(instrHandle1);
   if (instrHandle2 != VI_NULL)
      niFgen_close(instrHandle2);

   return 0;
}

