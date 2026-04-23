/*****************************************************************************/
/* National Instruments Function Generator                                   */
/* Simple Function Generator Example source file                             */
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

int main(int argc, char *argv[]) {
   ViChar Resource[256] = "", Type[256] = "";
   const ViChar * ChannelName = "0";
   ViReal64 Frequency, Amplitude, StartPhase, DCOffset;
   ViStatus error = VI_SUCCESS;
   ViSession vi=VI_NULL;
   ViInt32 wfmType;

   if(argc == 1) {
      /*- Prompt for parameters -----------------------------------------------*/
      #define BUFSIZE 256
      ViChar inputLine[BUFSIZE];

      // set default values
      strcpy(Resource, "PXI1Slot2");
      strcpy(Type, "sine");
      Frequency = 1e+6;
      Amplitude = 1.0;
      StartPhase = 0.0;
      DCOffset = 0.0;

      // Optionally override defaults from the command line.
      // If user simply hits enter, inputLine is an empty string, and sscanf will not
      // update its target variable (so the default value is preserved)

      printf("\nSpecify Inputs (Enter to accept default)\n");

      printf("Resource Name (%s): ", Resource);
      if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%s", Resource);

      printf("Waveform Type {sine|square|triangle|up|down|dc|noise} (%s): ", Type);
      if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%s", Type);

      printf("Frequency (%lf): ", Frequency);
      if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%lf", &Frequency);

      printf("Amplitude in Vp-p (%lf): ", Amplitude);
      if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%lf", &Amplitude);

      printf("Start Phase (%lf): ", StartPhase);
      if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%lf", &StartPhase);

      printf("DC Offset (%lf): ", DCOffset);
      if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%lf", &DCOffset);

      // Print newline(s) to visually separate output from input
      printf("\n\n");
   }
   else if(argc == 7) {
      /*- Get parameters from the command line --------------------------------*/
      strcpy(Resource, argv[1]);
      strcpy(Type, argv[2]);
      Frequency = strtod(argv[3], NULL);
      Amplitude = strtod(argv[4], NULL);
      StartPhase = strtod(argv[5], NULL);
      DCOffset = strtod(argv[6], NULL);
   }
   else {
      /*- Show usage ----------------------------------------------------------*/
      printf("Usage: %s <resource> [sine|square|triangle|up|down|dc|noise]"
          " <frequency> <amplitude> <start phase> <DC offset>\n", argv[0]);
      return -1;
   }


   /*- Convert strings to constants ----------------------------------------*/
   if     (strcmp(Type, "sine") == 0)    wfmType = NIFGEN_VAL_WFM_SINE;
   else if(strcmp(Type, "square") == 0)  wfmType = NIFGEN_VAL_WFM_SQUARE;
   else if(strcmp(Type, "triangle") == 0)wfmType = NIFGEN_VAL_WFM_TRIANGLE;
   else if(strcmp(Type, "up") == 0)      wfmType = NIFGEN_VAL_WFM_RAMP_UP;
   else if(strcmp(Type, "down") == 0)    wfmType = NIFGEN_VAL_WFM_RAMP_DOWN;
   else if(strcmp(Type, "dc") == 0)      wfmType = NIFGEN_VAL_WFM_DC;
   else if(strcmp(Type, "noise") == 0)   wfmType = NIFGEN_VAL_WFM_NOISE;
   else wfmType = 0;
   
   /*- Initialize the session ----------------------------------------------*/
   printf("Initializing %s\n", Resource);
   checkErr(niFgen_init(Resource, VI_TRUE, VI_TRUE, &vi));

   /*- Configure the active channels for the session -----------------------*/
   checkErr(niFgen_ConfigureChannels(vi, "0"));
   
   /*- Configure output for standard function mode -------------------------*/
   checkErr(niFgen_ConfigureOutputMode(vi, NIFGEN_VAL_OUTPUT_FUNC));

   /*- Configure the standard function to generate -------------------------*/ 
   checkErr(niFgen_ConfigureStandardWaveform(vi, ChannelName, wfmType,
                                             Amplitude, DCOffset, Frequency,
                                             StartPhase)); 
   
   /*- Enable output and generate ------------------------------------------*/
   checkErr(niFgen_ConfigureOutputEnabled(vi, ChannelName, VI_TRUE));
   checkErr(niFgen_InitiateGeneration(vi));

   printf("Generating a %s wave\n", Type);

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

   /*- Abort generation and close the session -------------------------------*/
   if (vi) {
      niFgen_AbortGeneration(vi);
      niFgen_close (vi);
   }
   return 0;
}

