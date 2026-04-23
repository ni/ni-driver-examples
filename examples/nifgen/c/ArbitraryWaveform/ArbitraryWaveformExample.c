/*****************************************************************************/
/* National Instruments Function Generator                                   */
/* Arbitrary Waveform Example source file                                    */
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
   ViChar Resource[256], DataFile[256];
   const ViChar * ChannelName = "0";
   ViReal64 SampleRate, Gain, DCOffset, ActualSampleRate;
   ViInt32  Filter, ClockMode, SampleClock;
   ViChar SampleClockSource[256];
   ViStatus error = VI_SUCCESS;
   ViSession vi=VI_NULL;
   ViInt32 wfmHandle;
   ViBoolean filterEnable;

   if(argc == 1) {
      /*- Prompt for parameters -----------------------------------------------*/
      #define BUFSIZE 256
      ViChar inputLine[BUFSIZE];

      // set default values
      strcpy(Resource, "PXI1Slot2");
      strcpy(DataFile, "sine.bin");
      SampleRate = 20e+6;
      Gain = 1.0;
      DCOffset = 0.0;
      Filter = 0;
      ClockMode = NIFGEN_VAL_AUTOMATIC;
      SampleClock = 0;

      // Optionally override defaults from the command line.
      // If user simply hits enter, inputLine is an empty string, and sscanf will not
      // update its target variable (so the default value is preserved)

      printf("\nSpecify Inputs (Enter to accept default)\n");

      printf("Resource Name (%s): ", Resource);
      if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%s", Resource);

      printf("i16 Binary Data File (assumes Big Endian) (%s): ", DataFile);
      if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%s", DataFile);

      printf("Sample Rate in Hz (%lf): ", SampleRate);
      if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%lf", &SampleRate);

      printf("Gain (%lf): ", Gain);
      if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%lf", &Gain);

      printf("DC Offset (%lf): ", DCOffset);
      if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%lf", &DCOffset);

      printf("Filter {1=true | 0=false} (%d): ", Filter);
      if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%d", &Filter);

      printf("Clock Mode {0=HiRes | 1=DivDown | 2=Automatic} (%d): ", ClockMode);
      if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%d", &ClockMode);

      printf("Sample Clock Source {0=internal | 1=external} (%d): ", SampleClock);
      if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%d", &SampleClock);

      // Print newline(s) to visually separate output from input
      printf("\n\n");
   }
   else if(argc == 9) {
      /*- Get parameters from the command line ------------------------------*/
      strcpy(Resource, argv[1]);
      strcpy(DataFile, argv[2]);
      SampleRate = strtod(argv[3], NULL);
      Gain = strtod(argv[4], NULL);
      DCOffset = strtod(argv[5], NULL);
      Filter = strtol(argv[6], NULL, 0);
      ClockMode = strtol(argv[7], NULL, 0);
      SampleClock = strtol(argv[8], NULL, 0);
   }
   else {
      /*- Show usage --------------------------------------------------------*/
      printf("Usage: %s <resource> <data file (i16 binary)> <sample rate>"
            " <gain> <dc offset> <filters enabled (1 or 0)> <clock mode (0=hi res, 1=div down, 2=automatic)> "
            " <sample clock source (1=external, 0=internal)>\n", argv[0]);
      printf("The data file is big-endian to be compatible with the Waveform Editor.\n");
      return -1;
   }
   

   /*- Convert input to constants -------------------------------------------*/
   filterEnable = ((Filter == 1) ? VI_TRUE : VI_FALSE);
   if (SampleClock == 0) strcpy(SampleClockSource, "OnboardClock");
   else strcpy(SampleClockSource, "External");

   
   /*- Initialize the session -----------------------------------------------*/
   printf("Initializing %s\n", Resource);
   checkErr(niFgen_init(Resource, VI_TRUE, VI_TRUE, &vi));

   /*- Configure the active channels for the session ------------------------*/
   checkErr(niFgen_ConfigureChannels(vi, "0"));

   /*- Configure the device for arb mode ------------------------------------*/  
   checkErr(niFgen_ConfigureOutputMode(vi, NIFGEN_VAL_OUTPUT_ARB));

   /*- Create the waveform and download the data ----------------------------*/
   checkErr(niFgen_CreateWaveformFromFileI16 (vi, ChannelName, 
                                              DataFile,
                                              NIFGEN_VAL_BIG_ENDIAN,
                                              &wfmHandle));

   /*- Select the waveform to generate --------------------------------------*/
   checkErr(niFgen_ConfigureArbWaveform(vi, ChannelName, wfmHandle, Gain,
                                        DCOffset)); 

   /*- Configure the sample clock -------------------------------------------*/
   checkErr(niFgen_ConfigureSampleClockSource(vi, SampleClockSource));
   checkErr(niFgen_ConfigureSampleRate(vi, SampleRate));
   checkErr(niFgen_ConfigureClockMode(vi, ClockMode));

   /*- Get the coerced sample rate (coerced in divide down clock mode only) -*/

   checkErr(niFgen_GetAttributeViReal64 (vi, VI_NULL,
                                         NIFGEN_ATTR_ACTUAL_ARB_SAMPLE_RATE,
                                         &ActualSampleRate));  

   /*- Turn filters on or off -----------------------------------------------*/ 
   checkErr(niFgen_SetAttributeViBoolean(vi, ChannelName,
                                         NIFGEN_ATTR_DIGITAL_FILTER_ENABLED,
                                         filterEnable)); 
   checkErr(niFgen_SetAttributeViBoolean(vi, ChannelName,
                                         NIFGEN_ATTR_ANALOG_FILTER_ENABLED,
                                         filterEnable)); 

   /*- Enable output and generate  ------------------------------------------*/
   checkErr(niFgen_ConfigureOutputEnabled(vi, ChannelName, VI_TRUE));
   checkErr(niFgen_InitiateGeneration(vi));

   printf("Generating %s at %lf Hz\n", DataFile, ActualSampleRate);

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

