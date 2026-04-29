/************************************************************************
*
*  Example Program:
*    StaticGenerationWithTristate.c
*
*  Description:
*    Demonstrates how to tristate specified channels.
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
   ViConstString tristateChannelList = "3,7,11,15";
   ViUInt32 writeData = 0xFFFFF;
   ViUInt32 channelMask = 0xFFFFF; /* all channels */

   ViSession vi = VI_NULL;
   ViStatus error = VI_SUCCESS;
   ViChar errDesc[1024];


   /* Initialize generation session */
   checkErr(niHSDIO_InitGenerationSession(
            deviceID, VI_FALSE, VI_FALSE, VI_NULL, &vi));

   /* Assign channels for static generation */
   checkErr(niHSDIO_AssignStaticChannels(vi, channelList));


   /* Tristate channels */
   checkErr(niHSDIO_TristateChannels(vi, tristateChannelList, VI_TRUE));

   /* Write static data with channel mask */
   checkErr(niHSDIO_WriteStaticU32(vi, writeData, channelMask));

   /* Re-enable tristated channels.  Only channels that have been
      previously tristated in this session can be untristated  */
   checkErr(niHSDIO_TristateChannels(vi, tristateChannelList, VI_FALSE));


Error:

   if (error == VI_SUCCESS)
   {
      /* print result */
      printf("Done without error.\n");
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
