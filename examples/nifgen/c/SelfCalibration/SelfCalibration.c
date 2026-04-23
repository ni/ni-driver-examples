/*****************************************************************************/
/* National Instruments Function Generator                                   */
/* Self Calibration Example source file                                      */
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
   ViBoolean isSelfCalSupported;
   ViChar    userInfo[100];
   ViChar    oldUserInfo[5];
   ViInt32   minute, hour, day, month, year;
   ViReal64  temperature;

   if(argc == 1) {
      /*- Prompt for parameters -----------------------------------------------*/
      #define BUFSIZE 256
      ViChar inputLine[BUFSIZE];

      // set default values
      strcpy(Resource, "PXI1Slot2");
      strcpy(userInfo, "-1");
      
      // Optionally override defaults from the command line.
      // If user simply hits enter, inputLine is an empty string, and sscanf will not
      // update its target variable (so the default value is preserved)

      printf("\nSpecify Inputs (Enter to accept default)\n");

      printf("Resource Name (%s): ", Resource);
      if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%s", Resource);

      printf("User defined info [enter -1 to leave as is] (%s): ", userInfo);
      if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%s", userInfo);

      // Print newline(s) to visually separate output from input
      printf("\n\n");
   }
   else if(argc == 3) {
      /*- Get parameters from the command line ------------------------------*/
      strcpy(Resource, argv[1]);
      strcpy(userInfo, argv[2]);
   }
   else {
      /*- Show usage --------------------------------------------------------*/
      printf("Usage: %s <resource> <user info>\n", argv[0]);
      return -1;
   }

   /*- Initialize the session -----------------------------------------------*/
   printf("Initializing %s\n", Resource);
   checkErr(niFgen_init(Resource, VI_TRUE, VI_TRUE, &vi));

   /*- Find out if self calibration is supported for this device ------------*/  
   checkErr(niFgen_GetSelfCalSupported(vi, &isSelfCalSupported));

   /*- Error if self cal is not supported, otherwise perform self cal -------*/
   if (isSelfCalSupported) {
      // Get the previous self calibration info and display
      checkErr(niFgen_GetCalUserDefinedInfo(vi, oldUserInfo));
      checkErr(niFgen_GetSelfCalLastDateAndTime(vi, &year, &month, &day, &hour, &minute));
      checkErr(niFgen_GetSelfCalLastTemp(vi, &temperature));
      printf("Last self calibration on %d/%d/%d at %d:%.2d.  Board temperature: %.2f,  User info: %s\n",
         month, day, year, hour, minute, temperature, oldUserInfo);

      // Perform the self calibration
      if (strcmp(userInfo, "-1")) checkErr(niFgen_SetCalUserDefinedInfo(vi, userInfo));
      printf("Performing self calibration...\n");
      checkErr(niFgen_SelfCal(vi));

      // Get the current self calibration info and display
      checkErr(niFgen_GetCalUserDefinedInfo(vi, oldUserInfo));
      checkErr(niFgen_GetSelfCalLastDateAndTime(vi, &year, &month, &day, &hour, &minute));
      checkErr(niFgen_GetSelfCalLastTemp(vi, &temperature));
      printf("Self calibration successful on %d/%d/%d at %d:%.2d.  Board temperature: %.2f,  User info: %s\n",
         month, day, year, hour, minute, temperature, oldUserInfo);
   } else printf("Self calibration is not supported for this device.");

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

