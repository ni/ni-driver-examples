/*****************************************************************************/
/* National Instruments Function Generator                                   */
/* Sweep Generator Example source file                                       */
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

ViStatus ReadFreqFile(ViString File, ViInt32 *Length, ViReal64 **frequencies,
                      ViReal64 **amplitudes);

int main(int argc, char *argv[]) {
   ViChar Resource[256], Type[256], File[256];
   const ViChar * ChannelName = "0";
   ViReal64 Amplitude, StartPhase, DCOffset;
   ViStatus error = VI_SUCCESS;
   ViSession vi = VI_NULL;
   ViInt32 wfmType;
   ViInt32 Length;
   ViReal64 *frequencies=VI_NULL, *durations=VI_NULL;
   ViInt32 fListHandle;

   if(argc == 1) {
      /*- Prompt for parameters -----------------------------------------------*/
      #define BUFSIZE 256
      ViChar inputLine[BUFSIZE];

      // set default values
      strcpy(Resource, "PXI1Slot2");
      strcpy(Type, "sine");
      strcpy(File, "freqlist.txt");
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

      printf("Frequency List File (%s): ", File);
      if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%s", File);

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
      strcpy(File, argv[3]);
      Amplitude = strtod(argv[4], NULL);
      StartPhase = strtod(argv[5], NULL);
      DCOffset = strtod(argv[6], NULL);
   }
   else {
      /*- Show usage ----------------------------------------------------------*/
      printf( "Usage: %s <resource> [sine|square|triangle|up|down|dc|noise]"
           " <frequency list file> <amplitude> <start phase> <DC offset>\n", argv[0]);
      printf ("The frequency list file is a text file with 2 numbers per line\n"
           "separated by white space.  The first is the frequency in Hz, and the\n"
           "second is the duration in seconds.");
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
   
   /*- Read in the file ----------------------------------------------------*/
   checkErr(ReadFreqFile(File, &Length, &frequencies, &durations));
   
   /*- Initialize the session ----------------------------------------------*/
   printf("Initializing %s\n", Resource);
   checkErr(niFgen_init(Resource, VI_TRUE, VI_TRUE, &vi));

   /*- Configure the active channels for the session -----------------------*/
   checkErr(niFgen_ConfigureChannels(vi, "0"));

   /*- Configure output for frequency list mode ----------------------------*/
   checkErr(niFgen_ConfigureOutputMode(vi, NIFGEN_VAL_OUTPUT_FREQ_LIST));

   /*- Create the frequency list -------------------------------------------*/
   checkErr(niFgen_CreateFreqList(vi, wfmType, Length, frequencies,
                                  durations, &fListHandle)); 

   /*- Select frequency list to generate -----------------------------------*/
   checkErr(niFgen_ConfigureFreqList(vi, ChannelName, fListHandle, Amplitude,
                                     DCOffset, StartPhase)); 
   
   /*- Enable output and start generation ----------------------------------*/
   checkErr(niFgen_ConfigureOutputEnabled(vi, ChannelName, VI_TRUE));
   checkErr(niFgen_InitiateGeneration(vi));

   /*- clear the input buffer, wait for user input before closing session. -*/
   fflush( stdin );
   printf("Generating a %s wave with frequencies from %s.  Press Enter to continue...\n", Type, File);
   getchar();

Error:
   /*- Process any errors ---------------------------------------------------*/
   if(error != VI_SUCCESS) {
      ViChar errMsg[256];
      niFgen_ErrorHandler(vi, error, errMsg);
      printf("Error %x: %s\n", error, errMsg);
   }

   /*- Free allocated memory ------------------------------------------------*/
   if(frequencies != VI_NULL) free(frequencies);
   if(durations != VI_NULL) free(durations);

   /*- Abort generation and close the session -------------------------------*/
   if (vi) {
      niFgen_AbortGeneration(vi);
      niFgen_close (vi);
   }
   return 0;
}

   /*- Read in the file ----------------------------------------------------*/
ViStatus ReadFreqFile(ViString File, ViInt32 *Length, ViReal64 **frequencies,
                      ViReal64 **durations) {
   FILE * file;
   ViInt32 Lines, temp, i;
   ViReal64 t1, t2;
   ViStatus error = VI_SUCCESS;

   printf("Reading file: %s\n", File);
   if(!(file = fopen(File, "r"))) {
      printf ("Failed to open file %s\n", File);
      printf("Use a full or relative path if the file is not in the current directory.\n");
      checkErr(-1);
   }
   for(Lines=0; ; Lines++) {
      temp = fscanf(file, "%lf %lf\n", &t1, &t2);
      if(temp != 2) break;
   }
   if(Lines == 0) {
      printf("Invalid input file %s\n", File);
      checkErr(-1);
   }
   rewind(file);
   checkAlloc(*frequencies = (ViReal64*) malloc(Lines * sizeof(ViReal64)));
   checkAlloc(*durations = (ViReal64*) malloc(Lines * sizeof(ViReal64)));
   for(i=0; i<Lines; i++) {
      temp = fscanf(file, "%lf %lf", &t1, &t2);
      (*frequencies)[i] = t1;
      (*durations)[i] = t2;
      if(temp != 2) break;
   }
   *Length = i;

Error:
   if (file) fclose(file);

   return error;
}



