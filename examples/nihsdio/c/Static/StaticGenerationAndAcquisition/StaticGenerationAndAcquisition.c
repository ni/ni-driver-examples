/************************************************************************
*
*  Example Program:
*    StaticGenerationAndAcquisition.c
*
*  Description:
*    Writes static data to specified channels and reads back the state
*    of the channels afterwards.
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
   /* Execution parameters */
   ViRsrc deviceID = "PXI1Slot2";
   ViConstString channelList = "0-15";

   ViUInt32 writeData = 0x4321;
   ViUInt32 channelMask = 0xFFFF; /* all channels */
   ViInt32  genVoltageLogicFamily = NIHSDIO_VAL_3_3V_LOGIC;

   ViUInt32 readData = 0;
   ViInt32 acqVoltageLogicFamily = NIHSDIO_VAL_3_3V_LOGIC;

   /* Context parameters */
   ViSession oSes = VI_NULL;
   ViSession iSes = VI_NULL;
   ViStatus error = VI_SUCCESS;
   ViChar oErrDesc[1024];
   ViChar iErrDesc[1024];


   /* Initialize both sessions */
   checkErr(niHSDIO_InitGenerationSession(
            deviceID, VI_FALSE, VI_FALSE, VI_NULL, &oSes));

   checkErr(niHSDIO_InitAcquisitionSession(
            deviceID, VI_FALSE, VI_FALSE, VI_NULL, &iSes));

   /* Assign channels for static operation */
   checkErr(niHSDIO_AssignStaticChannels(oSes, channelList));
   checkErr(niHSDIO_AssignStaticChannels(iSes, channelList));

   /* Configure generation voltage */
   checkErr(niHSDIO_ConfigureDataVoltageLogicFamily(
            oSes, channelList, genVoltageLogicFamily));

   /* Configure acquisition voltage */
   checkErr(niHSDIO_ConfigureDataVoltageLogicFamily(
            iSes, channelList, acqVoltageLogicFamily));

   /* Write static data with channel mask */
   checkErr(niHSDIO_WriteStaticU32(oSes, writeData, channelMask));

   /* Read back static data */
   checkErr(niHSDIO_ReadStaticU32(iSes, &readData));

Error:

   if (error == VI_SUCCESS)
   {
      /* print result */
      printf("Done without error.\n");
      printf("Data written (channel mask 0x%.4X) = 0x%X \n", channelMask, writeData);
      printf("Data read = 0x%X \n", readData);
   }
   else
   {
      /* Get error description and print */
      niHSDIO_GetError(oSes, &error, sizeof(oErrDesc)/sizeof(ViChar), oErrDesc);
      niHSDIO_GetError(iSes, &error, sizeof(iErrDesc)/sizeof(ViChar), iErrDesc);

      printf("\nError encountered\n===================\n%s%s\n", oErrDesc, iErrDesc);
   }

   /* close both sessions */
   niHSDIO_close(iSes);
   niHSDIO_close(oSes);

   /* prompt to exit (for pop-up console windows) */
   printf("\nHit <Enter> to continue...\n");
   getchar();

   return error;
}
