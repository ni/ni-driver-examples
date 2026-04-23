/*****************************************************************************/
/* National Instruments Function Generator                                   */
/* Triggers Example source file                                              */
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
#include <math.h>
#include <time.h>

#define WFM_SIZE 128
#define SEQ_SIZE 3

enum {
   NO_TRIGGER,
   SOFTWARE_TRIGGER,
   DIGITAL_TRIGGER 
};

int main(int argc, char *argv[]) {
   ViChar Resource[256];
   ViStatus error = VI_SUCCESS;
   ViSession vi=VI_NULL;
   ViReal64 sampleRate;
   ViReal64 gain;
   ViInt32  triggerMode;
   ViChar   triggerSource[256];
   ViInt32  tempTriggerSource;
   ViInt32  tempTriggerLine;
   ViReal64 sine[WFM_SIZE];
   ViReal64 square[WFM_SIZE];
   ViReal64 ramp[WFM_SIZE];
   ViInt32  handles[SEQ_SIZE];
   ViInt32  seqHandle;
   ViInt32  loopCounts[SEQ_SIZE];
   ViInt32  i;
   ViChar   input = 0;
   ViInt32  triggered = 0;
   ViInt32  hardwareState;
   time_t   startTime;
   ViReal64 waitTimeInSeconds = 8.0;
   ViInt32  triggerType;

   if(argc == 1) {
      /*- Prompt for parameters -----------------------------------------------*/
      #define BUFSIZE 256
      ViChar inputLine[BUFSIZE];

      // set default values
      strcpy(Resource, "PXI1Slot2");
      sampleRate = 40000000.0;
      gain = 1;
      triggerMode = 2;              // Continuous
      tempTriggerSource = 2;        // Software trigger
      tempTriggerLine = 0;          // Trigger line (RTSI or PFI)

      // Optionally override defaults from the command line.
      // If user simply hits enter, inputLine is an empty string, and sscanf will not
      // update its target variable (so the default value is preserved)

      printf("\nSpecify Inputs (Enter to accept default)\n");

      printf("Resource Name (%s): ", Resource);
      if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%s", Resource);

      printf("Sample Rate (%f): ", sampleRate);
      if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%lf", &sampleRate);

      printf("Gain (%f): ", gain);
      if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%lf", &gain);

      printf("Trigger Mode {1=Single | 2=Continuous | 3=Stepped | 4=Burst} (%d): ", triggerMode);
      if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%d", &triggerMode);

      printf("Trigger Source {0=Immediate | 1=External | 2=Software | 3=RTSI | 4=PFI | 5=PXI Star} (%d): ", tempTriggerSource);
      if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%d", &tempTriggerSource);

      // Figure out the right constant to use for trigger source based on user input
      switch(tempTriggerSource) {
         case 0:
            triggerType = NO_TRIGGER;
            break;
         case 1:
            triggerType = DIGITAL_TRIGGER;
            strcpy(triggerSource, "External");
            break;
         case 2:
            triggerType = SOFTWARE_TRIGGER;
            break;
         case 3: 
            triggerType = DIGITAL_TRIGGER;
            printf("\tRTSI line: {0-7} (%d): ", tempTriggerLine);
            if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%d", &tempTriggerLine);
            sprintf(triggerSource, "RTSI%d", tempTriggerLine);
            break;
         case 4:
            triggerType = DIGITAL_TRIGGER;
            printf("\tPFI line: {0-1} (%d): ", tempTriggerLine);
            if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%d", &tempTriggerLine);
            sprintf(triggerSource, "PFI%d", tempTriggerLine);
            break;
         case 5:
            triggerType = DIGITAL_TRIGGER;
            strcpy(triggerSource, "PXIStar");
            break;
      }

      // Print newline(s) to visually separate output from input
      printf("\n\n");
   }
   else if(argc == 5 || argc == 6) {
      /*- Get parameters from the command line ------------------------------*/
      strcpy(Resource, argv[1]);
      sampleRate = strtod(argv[2], NULL);
      triggerMode = strtol(argv[3], NULL, 0);
      tempTriggerSource = strtol(argv[4], NULL, 0);
      if (argc == 6) tempTriggerLine = strtol(argv[5], NULL, 0);

      // Figure out the right constant to use for trigger source based on user input
      switch(tempTriggerSource) {
         case 0:
            triggerType = NO_TRIGGER;
            break;
         case 1:
            triggerType = DIGITAL_TRIGGER;
            strcpy(triggerSource, "External");
            break;
         case 2:
            triggerType = SOFTWARE_TRIGGER;
            break;
         case 3: 
            triggerType = DIGITAL_TRIGGER;
            sprintf(triggerSource, "RTSI%d", tempTriggerLine);
            break;
         case 4:
            triggerType = DIGITAL_TRIGGER;
            sprintf(triggerSource, "PFI%d", tempTriggerLine);
            break;
         case 5:
            triggerType = DIGITAL_TRIGGER;
            strcpy(triggerSource, "PXIStar");
            break;
      }

   }
   else {
      /*- Show usage --------------------------------------------------------*/
      printf("Usage: %s <resource> <sample rate> <trigger mode> <trigger source> <trigger line>\n", argv[0]);
      return -1;
   }
   // clear the input buffer.
   fflush( stdin );

    /*- Create some waveform data --------------------------------------------*/
   // Sine:
   for (i = 0; i < WFM_SIZE; i++)
       sine[i] = sin(((ViReal64)i/WFM_SIZE)*2*3.141596);

   // Square
   for (i = 0; i < WFM_SIZE; i++) {
       if (i < WFM_SIZE/2) square[i] = .9;
       else square[i] = -.9;
   }
    
   // Ramp
   for (i = 0; i < WFM_SIZE; i++)
       ramp[i] = (ViReal64)i/WFM_SIZE - .5;

   /*- Initialize the session -----------------------------------------------*/
   printf("Initializing %s\n", Resource);
   checkErr(niFgen_init(Resource, VI_TRUE, VI_TRUE, &vi));

   /*- Configure the active channels for the session -----------------------*/
   checkErr(niFgen_ConfigureChannels(vi, "0"));
  
   /*- Configure output for sequence mode ----------------------------------*/    
   checkErr(niFgen_ConfigureOutputMode(vi, NIFGEN_VAL_OUTPUT_SEQ));

   /*- Create and download all of the waveforms for the sequence -----------*/
   checkErr(niFgen_CreateArbWaveform(vi, WFM_SIZE, sine, &(handles[0])));
   loopCounts[0] = 1;
   checkErr(niFgen_CreateArbWaveform(vi, WFM_SIZE, square, &(handles[1])));
   loopCounts[1] = 1;
   checkErr(niFgen_CreateArbWaveform(vi, WFM_SIZE, ramp, &(handles[2])));
   loopCounts[2] = 1;

   /*- Create the arbitrary sequence ---------------------------------------*/
   checkErr(niFgen_CreateArbSequence(vi, SEQ_SIZE, handles, loopCounts, &seqHandle));

   /*- Select the arbitrary sequence to generate ---------------------------*/
   checkErr(niFgen_ConfigureArbSequence(vi, "0", seqHandle, gain, 0.0)); 
   
   /*- Configure sample clock mode and rate --------------------------------*/    
   checkErr(niFgen_ConfigureSampleRate(vi, sampleRate));

   /*- Configure trigger mode and source -----------------------------------*/    
   checkErr(niFgen_ConfigureTriggerMode(vi, "0", triggerMode));
   if (triggerType == NO_TRIGGER) {
      checkErr(niFgen_DisableStartTrigger(vi));
   } else if (triggerType == SOFTWARE_TRIGGER) {
      checkErr(niFgen_ConfigureSoftwareEdgeStartTrigger(vi));
   } else {
      checkErr(niFgen_ConfigureDigitalEdgeStartTrigger(vi, triggerSource, NIFGEN_VAL_RISING_EDGE));
   }

   /*- Enable output and start generating ----------------------------------*/    
   checkErr(niFgen_ConfigureOutputEnabled(vi, "0", VI_TRUE));
   checkErr(niFgen_InitiateGeneration(vi));

   printf("Waiting %.2f seconds for trigger.\n", waitTimeInSeconds);
   startTime = time(0);
   
   do 
   {
      checkErr(niFgen_GetHardwareState(vi, &hardwareState));
      if (hardwareState == NIFGEN_VAL_WAITING_FOR_START_TRIGGER && triggered == 1) {
         triggered = 0;
         printf("Waiting for trigger...\n");
      } else if (hardwareState != NIFGEN_VAL_WAITING_FOR_START_TRIGGER && triggered == 0) {
         triggered = 1;
         printf("Trigger received, generating sequence at %lf Hz\n", sampleRate);
      }      
   }
   while (difftime(time(0), startTime) < waitTimeInSeconds);

   if (!triggered) 
   {
      printf("Time expired without hardware trigger.  Sending software trigger.\n");
      checkErr(niFgen_SendSoftwareEdgeTrigger(vi, NIFGEN_VAL_START_TRIGGER, ""));        

   }

   // Continue to output the now-triggered waveform a little longer
   while (difftime(time(0), startTime) < waitTimeInSeconds * 1.5);

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

