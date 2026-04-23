/*****************************************************************************/
/* National Instruments Function Generator                                   */
/* T Clock Synchronization Example source file                               */
/*                                                                           */
/* National Instruments, Austin Texas                                        */
/* PH. (800)433-3488   Fax (512)794-5678                                     */
/* Original Release: 10-03                                                   */
/*                                                                           */
/* Modification History:                                                     */
/*     Date    Initials  Description                                         */
/*     10-03   DC        Created                                             */
/*****************************************************************************/
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <math.h>
#include "niFgen.h"
#include "niTClk.h"

#define WFM_SIZE 16
#define NIFGEN_ERROR_SIZE 4096

int main(int argc, char *argv[]) {
   ViChar ResourceInput[4096];
   ViChar* currentResource = VI_NULL;
   ViUInt32 numResources;
   ViReal64 sampleRate;
   ViStatus error = VI_SUCCESS;
   ViSession* vi = VI_NULL;
   ViInt32 wfmHandle;
   ViReal64 wfmData[WFM_SIZE];
   ViUInt32 i;

   if(argc == 1) {
      /*- Prompt for parameters -----------------------------------------------*/
      #define BUFSIZE 256
      ViChar inputLine[BUFSIZE];

      // set default values
      strcpy(ResourceInput, "PXI1Slot2,PXI1Slot3");
      sampleRate = 20e+6;

      // Optionally override defaults from the command line.
      // If user simply hits enter, inputLine is an empty string, and sscanf will not
      // update its target variable (so the default value is preserved)

      printf("\nSpecify Inputs (Enter to accept default)\n");

      printf("Resource Names (comma separated) (%s): ", ResourceInput);
      if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%s", ResourceInput);

      printf("Sample Rate in Hz (%lf): ", sampleRate);
      if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%lf", &sampleRate);

      // Print newline(s) to visually separate output from input
      printf("\n\n");
   }
   else if(argc == 3) {
      /*- Get parameters from the command line ------------------------------*/
      strcpy(ResourceInput, argv[1]);
      sampleRate = strtod(argv[2], NULL);
   }
   else {
      /*- Show usage --------------------------------------------------------*/
      printf("Usage: %s <resource1,resource2,etc.> <sample rate> \n", argv[0]);
      return -1;
   }

   /*- Create some waveform data --------------------------------------------*/
   // Sine:
   for (i = 0; i < WFM_SIZE; i++)
       wfmData[i] = 0.0;
   wfmData[0] = 1.0;
  
   /*- Parse the resource input string --------------------------------------*/
   numResources = 1;
   for (i = 0; i < strlen(ResourceInput); i++)
      if (ResourceInput[i] == ',') numResources++;
   currentResource = strtok(ResourceInput, ",");
   vi = (ViSession *) malloc(numResources * sizeof(ViSession));

   for (i = 0; i < numResources; i++)
   {
      /*- Initialize the sessions ----------------------------------------------*/
      printf("Initializing %s\n", currentResource);
      vi[i] = VI_NULL;
      checkErr(niFgen_init(currentResource, VI_TRUE, VI_TRUE, &vi[i]));

      /*- Configure the active channels for the session ----------------------*/
      checkErr(niFgen_ConfigureChannels(vi[i], "0"));
      
      /*- Set the sample rate ------------------------------------------------*/
      checkErr(niFgen_ConfigureSampleRate(vi[i], sampleRate));

      /*- Create and download the waveform -----------------------------------*/
      checkErr(niFgen_CreateArbWaveform(vi[i], WFM_SIZE, wfmData, &wfmHandle));    

      currentResource = strtok(NULL, ",");
   }

   /*- Configure for homogeneous triggers for niTClk ----------------------*/
   checkErr(niTClk_ConfigureForHomogeneousTriggers(numResources, vi));

   /*- Synchronize arbs and start generation ------------------------------*/
   checkErr(niTClk_Synchronize(numResources, vi, 0));
   checkErr(niTClk_Initiate(numResources, vi));

   printf("Generating square wave at %lf Hz\n", sampleRate/WFM_SIZE);

   // clear the input buffer, then wait for user input before closing the session.
   // (DAQmx devices will quit generating the output when the session is closed).
   fflush( stdin );
   printf("Press Enter to continue...\n");
   getchar();

Error:
   /*- Process any errors ---------------------------------------------------*/
   if(error != VI_SUCCESS) {
      ViChar errMsg[NIFGEN_ERROR_SIZE];
      if (i >= numResources)
         niTClk_GetExtendedErrorInfo((ViChar*) &errMsg, NIFGEN_ERROR_SIZE);
      else 
         niFgen_GetError(vi[i], &error, NIFGEN_ERROR_SIZE, errMsg);
      printf("Error %x: %s\n", error, errMsg);
   }

   /*- Close the sessions ---------------------------------------------------*/
   if (vi != VI_NULL) {
      for (i = 0; i < numResources; i++)
         if (vi[i] != VI_NULL) niFgen_close (vi[i]);
      free(vi);
   }
   return 0;
}

