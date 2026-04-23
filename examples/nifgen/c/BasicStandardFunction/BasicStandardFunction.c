/*****************************************************************************/
/* National Instruments Function Generator                                   */
/* Basic Standard Function Example source file                               */
/*                                                                           */
/* National Instruments, Austin Texas                                        */
/* PH. (800)433-3488   Fax (512)794-5678                                     */
/* Original Release: 12-03                                                   */
/*                                                                           */
/* Modification History:                                                     */
/*     Date    Initials  Description                                         */
/*     10-03    DC        Created                                            */
/*****************************************************************************/
#include "niFgen.h"
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

int main(int argc, char *argv[]) {
   ViChar Resource[256];
   ViStatus error = VI_SUCCESS;
   ViSession vi=VI_NULL;
   ViInt32 wfmType;
   ViChar tempWfmType[20];
   ViReal64 amplitude;
   ViReal64 frequency;

   if(argc == 1) {
      /*- Prompt for parameters -----------------------------------------------*/
      #define BUFSIZE 256
      ViChar inputLine[BUFSIZE];

      // set default values
      strcpy(Resource, "PXI1Slot2");
      strcpy(tempWfmType, "sine");
      amplitude = 2;
      frequency = 1000000.0;

      // Optionally override defaults from the command line.
      // If user simply hits enter, inputLine is an empty string, and sscanf will not
      // update its target variable (so the default value is preserved)

      printf("\nSpecify Inputs (Enter to accept default)\n");

      printf("Resource Name (%s): ", Resource);
      if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%s", Resource);

      printf("Waveform [sine|square|triangle|up|down|dc|noise] (%s): ", tempWfmType);
      if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%s", tempWfmType);

      printf("Frequency (%lf): ", frequency);
      if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%lf", &frequency);

      printf("Amplitude (%lf): ", amplitude);
      if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%lf", &amplitude);

      // Print newline(s) to visually separate output from input
      printf("\n\n");
   }
   else if(argc == 5) {
      /*- Get parameters from the command line ------------------------------*/
      strcpy(Resource, argv[1]);
      strcpy(tempWfmType, argv[2]);
      frequency = strtod(argv[3], NULL);
      amplitude = strtod(argv[4], NULL);
   }
   else {
      /*- Show usage --------------------------------------------------------*/
      printf("Usage: %s <resource> [sine|square|triangle|up|down|dc|noise]", argv[0]);
      printf("<waveform> <frequency> <amplitude>\n");
      return -1;
   }

   /*- Convert strings to constants ----------------------------------------*/
   if     (strcmp(tempWfmType, "sine") == 0)    wfmType = NIFGEN_VAL_WFM_SINE;
   else if(strcmp(tempWfmType, "square") == 0)  wfmType = NIFGEN_VAL_WFM_SQUARE;
   else if(strcmp(tempWfmType, "triangle") == 0)wfmType = NIFGEN_VAL_WFM_TRIANGLE;
   else if(strcmp(tempWfmType, "up") == 0)      wfmType = NIFGEN_VAL_WFM_RAMP_UP;
   else if(strcmp(tempWfmType, "down") == 0)    wfmType = NIFGEN_VAL_WFM_RAMP_DOWN;
   else if(strcmp(tempWfmType, "dc") == 0)      wfmType = NIFGEN_VAL_WFM_DC;
   else if(strcmp(tempWfmType, "noise") == 0)   wfmType = NIFGEN_VAL_WFM_NOISE;
   else wfmType = 0;

   /*- Initialize the session -----------------------------------------------*/
   printf("Initializing %s\n", Resource);
   checkErr(niFgen_init(Resource, VI_TRUE, VI_TRUE, &vi));

   /*- Configure the active channels for the session ------------------------*/
   checkErr(niFgen_ConfigureChannels(vi, "0"));

   /*- Configure output for standard function mode --------------------------*/
   checkErr(niFgen_ConfigureOutputMode(vi, NIFGEN_VAL_OUTPUT_FUNC));

   /*- Configure the standard function to generate --------------------------*/ 
   checkErr(niFgen_ConfigureStandardWaveform(vi, "0", wfmType, amplitude, 0, frequency, 0));

   /*- Initiate generation --------------------------------------------------*/ 
   checkErr(niFgen_InitiateGeneration(vi));

   // clear the input buffer, then wait for user input before closing the session.
   // (DAQmx devices will quit generating the output when the session is closed).
   fflush( stdin );
   printf("Generating a %s wave. Press Enter to continue...\n", tempWfmType);
   getchar();

Error:
   /*- Process any errors ---------------------------------------------------*/
   if(error != VI_SUCCESS) {
      ViChar errMsg[256];
      niFgen_ErrorHandler(vi, error, errMsg);
      printf("Error %x: %s\n", error, errMsg);
   }

   /*- Abort generation and close the session -------------------------------*/
   if (vi) {
      niFgen_AbortGeneration(vi);
      niFgen_close (vi);
   }
   return 0;
}

