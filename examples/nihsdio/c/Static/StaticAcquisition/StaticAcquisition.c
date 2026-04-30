/************************************************************************
*
*  Example Program:
*    StaticAcquisition.c
*
*  Description:
*    Acquires static data from specified channels.
*
*  Pin Connection Information:
*    None.
*
************************************************************************/


/* Includes */
#include <stdio.h>
#include "niHSDIO.h"

int main(void)
{

   ViRsrc deviceID = "PXI1Slot2";
   ViConstString channelList = "0-15";

   ViSession vi = VI_NULL;
   ViStatus error = VI_SUCCESS;
   ViChar errDesc[1024];
   ViUInt32 readData = 0;


   /* Initialize acquisition session */
   checkErr(niHSDIO_InitAcquisitionSession(
            deviceID, VI_FALSE, VI_FALSE, VI_NULL, &vi));

   /* Assign channels for static acquisition */
   checkErr(niHSDIO_AssignStaticChannels(vi, channelList));


   /* Read static data */
   checkErr(niHSDIO_ReadStaticU32(vi, &readData));


Error:

   if (error == VI_SUCCESS)
   {
      /* print result */
      printf("Done without error.\n");
      printf("Data read = 0x%X \n", readData);
   }
   else
   {
      /* Get error description and print */
      niHSDIO_GetError(vi, &error, sizeof(errDesc)/sizeof(ViChar), errDesc);

      printf("\nError encountered\n===================\n%s\n", errDesc);
   }

   /* close the session */
   niHSDIO_close(vi);

   /* prompt to exit (for pop-up console windows) */
   printf("\nHit <Enter> to continue...\n");
   getchar();

   return error;
}










