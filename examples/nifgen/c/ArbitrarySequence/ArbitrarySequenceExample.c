/*****************************************************************************/
/* National Instruments Function Generator                                   */
/* Arbitrary Sequence Example source file                                    */
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

typedef struct Stage_s {
   ViChar wfmFileName[256];
   ViInt32 wfmHandle;
   ViInt32 loopCount;
   ViInt32 markerPosition;
} Stage;

// This program assumes a sequence with 3 stages
#define numberOfSequenceStages 3

int main(int argc, char *argv[]) {
   ViChar resource[256];
   const ViChar * channelName = "0";
   ViReal64 sampleRate, gain, dcOffset;
   ViInt32  filter, clockMode, updateClock;
   ViBoolean filterEnable;
   ViInt32 i, sequenceHandle;
   ViStatus error = VI_SUCCESS;
   ViSession vi = VI_NULL;
   ViInt32 wfmHandles[numberOfSequenceStages];
   ViInt32 loopCounts[numberOfSequenceStages];
   ViInt32 markers[numberOfSequenceStages];
   Stage sequence[numberOfSequenceStages];

   if(argc == 1) {
      /*- Prompt for parameters -----------------------------------------------*/
      #define BUFSIZE 256
      ViChar inputLine[BUFSIZE];

      // set default values
      strcpy(resource, "PXI1Slot2");
      strcpy(sequence[0].wfmFileName, "sine.bin");
      strcpy(sequence[1].wfmFileName, "triangle.bin");
      strcpy(sequence[2].wfmFileName, "square.bin");
      for (i = 0; i < numberOfSequenceStages; i++){
         sequence[i].loopCount = i + 1; 
         sequence[i].markerPosition = 0;
      }
      sampleRate = 20e+6;
      gain = 1.0;
      dcOffset = 0.0;
      filter = 0;
      clockMode = NIFGEN_VAL_AUTOMATIC;
      updateClock = 0;

      // Optionally override defaults from the command line.
      // If user simply hits enter, inputLine is an empty string, and sscanf will not
      // update its target variable (so the default value is preserved)

      printf("\nSpecify Inputs (Enter to accept default)\n");

      printf("Resource Name (%s): ", resource);
      if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%s", resource);

      /*- Initialize the session ----------------------------------------------*/
      printf("Initializing %s\n", resource);
      checkErr(niFgen_init(resource, VI_TRUE, VI_TRUE, &vi));

      /*- Configure the active channels for the session -----------------------*/
      checkErr(niFgen_ConfigureChannels(vi, "0"));

      /*- Configure output for sequence mode ----------------------------------*/    
      checkErr(niFgen_ConfigureOutputMode(vi, NIFGEN_VAL_OUTPUT_SEQ));


      printf("Configuring a %d stage stage sequence...\n", numberOfSequenceStages);
      for (i = 0; i < numberOfSequenceStages; i++){
         printf("Stage %d:\n", i+1);
         
         printf("-- File with waveform data (assumes Big Endian) (%s): ", sequence[i].wfmFileName);
         if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%s", sequence[i].wfmFileName);

         /*- Create wfm from file ---------------------------------------------*/    
         checkErr(
            niFgen_CreateWaveformFromFileI16(
               vi,
               channelName,
               sequence[i].wfmFileName,
               NIFGEN_VAL_BIG_ENDIAN,
               &(sequence[i].wfmHandle)
            )
         );

         printf("-- Number of loops (%d): ", sequence[i].loopCount);
         if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%d", &(sequence[i].loopCount));

         printf("-- Marker position (%d): ", sequence[i].markerPosition);
         if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%d", &(sequence[i].markerPosition));

   
      }

      /*- Create the handle, loop, and marker arrays from the stage array -----*/
      for(i = 0; i < numberOfSequenceStages; i++) {
         wfmHandles[i] = sequence[i].wfmHandle;
         loopCounts[i] = sequence[i].loopCount;
         markers[i] = sequence[i].markerPosition;
      }

      /*- Create the arbitrary sequence ---------------------------------------*/
      checkErr(niFgen_CreateAdvancedArbSequence(
         vi,
         numberOfSequenceStages,
         wfmHandles, 
         loopCounts,
         VI_NULL,
         markers,
         VI_NULL,
         &sequenceHandle));

      printf("Sample Rate in Hz (%lf): ", sampleRate);
      if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%lf", &sampleRate);

      printf("gain (%lf): ", gain);
      if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%lf", &gain);

      printf("DC Offset (%lf): ", dcOffset);
      if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%lf", &dcOffset);

      printf("Filter {1=true | 0=false} (%d): ", filter);
      if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%d", &filter);

      printf("Clock Mode {0=HiRes | 1=DivDown | 2=Automatic} (%d): ", clockMode);
      if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%d", &clockMode);

      printf("Update Clock Source {0=internal | 1=external} (%d): ", updateClock);
      if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%d", &updateClock);

      // Print newline(s) to visually separate output from input
      printf("\n\n");
   }else {
      /*- Show usage ----------------------------------------------------------*/
      printf("Usage:\n");
      printf("Run executable without passing in any parameters.");
      return -1;
   }
   
   /*- Convert input to constants ------------------------------------------*/
   filterEnable = ((filter == 1) ? VI_TRUE : VI_FALSE);
   updateClock = ((updateClock == 1) ? NIFGEN_VAL_EXTERNAL : NIFGEN_VAL_INTERNAL);


   /*- Select the arbitrary sequence to generate ---------------------------*/
   checkErr(niFgen_ConfigureArbSequence(vi, channelName, sequenceHandle, gain, dcOffset)); 
   
   /*- Configure sample clock mode and rate --------------------------------*/    
   checkErr(niFgen_ConfigureClockMode(vi, clockMode));
   checkErr(niFgen_ConfigureSampleRate(vi, sampleRate));
   checkErr(niFgen_GetAttributeViReal64 (vi, VI_NULL,
                                           NIFGEN_ATTR_ARB_SAMPLE_RATE,
                                           &sampleRate));  
   
   /*- Configure filters ---------------------------------------------------*/    
   checkErr(niFgen_SetAttributeViBoolean(vi, channelName,
                                         NIFGEN_ATTR_DIGITAL_FILTER_ENABLED,
                                         filterEnable)); 
   checkErr(niFgen_SetAttributeViBoolean(vi, channelName,
                                         NIFGEN_ATTR_ANALOG_FILTER_ENABLED,
                                         filterEnable)); 
   /*- Enable output and start generating ----------------------------------*/    
   checkErr(niFgen_ConfigureOutputEnabled(vi, channelName, VI_TRUE));
   checkErr(niFgen_InitiateGeneration(vi));
   
   
   printf("Generating sequence at %lf Hz\n", sampleRate);

Error:
   /*- Process any errors ---------------------------------------------------*/
   if(error != VI_SUCCESS) {
      ViChar errMsg[256];
      niFgen_ErrorHandler(vi, error, errMsg);
      printf("Error %x: %s\n", error, errMsg);
   }

   // clear the input buffer, then wait for user input before closing the session.
   // (DAQmx devices will quit generating the output when the session is closed).
   fflush( stdin );
   printf("Press Enter to continue...\n");
   getchar();

   /*- Close the session ----------------------------------------------------*/
   if (vi) niFgen_close (vi);
   return 0;
}

