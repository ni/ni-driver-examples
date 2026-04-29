/************************************************************************
*
*  Example Program:
*    DiodeDetection.c
*
*  Description:
*   This example demonstrates how to use voltage configuration on NI 655x devices
*   to detect the presence of clamp diodes in an I/O pin of a device under test.
*
*   The voltage configuration of this VI assumes that there is a diode connected
*   to a Vcc of 3.3 and another diode connected to ground.
*   5 V and -2 V signals are generated to activate the diodes,
*   and data interpretation is used to determine whether the diode was activated.
*
*   For more information on diode detection with 655x devices please visit
*   http://www.ni.com/zone and search for HSDIO diode.
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

   ViUInt32 channelMask = 0xFFFFF; /* all channels */
   ViReal64 genVoltageHigh = 5.0;
   ViReal64 genVoltageLow = -2.0;
   ViReal64 acqVoltageHigh = 4.0;
   ViReal64 acqVoltageLow = -1.25;


   ViUInt32 diodeResultsVcc = 0;
   ViUInt32 diodeResultsGnd = 0;

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

   /* Configure generation voltage for Vcc diode */
   checkErr(niHSDIO_ConfigureDataVoltageCustomLevels (oSes, channelList, 0, genVoltageHigh));

   /* Configure acquisition voltage */
   checkErr(niHSDIO_ConfigureDataVoltageCustomLevels (iSes, channelList, acqVoltageLow, acqVoltageHigh));

   /* Configure Data Interpretation to valid/invalid to check diode operation */
   checkErr(niHSDIO_ConfigureDataInterpretation (iSes, channelList, NIHSDIO_VAL_VALID_OR_INVALID));

   /* Write all ones to drive high voltage and activate Vcc diodes */
   checkErr(niHSDIO_WriteStaticU32(oSes, 0xFFFFF, channelMask));

   /* Read back static data to check Vcc diode operation */
   checkErr(niHSDIO_ReadStaticU32(iSes, &diodeResultsVcc));

   /* Reconfigure generation voltage to negative value */
   checkErr(niHSDIO_ConfigureDataVoltageCustomLevels (oSes, channelList, genVoltageLow, 0));

   /* Write all zeros to drive low voltage and activate Gnd diodes */
   checkErr(niHSDIO_WriteStaticU32(oSes, 0x0, channelMask));

   /* Read back static data to check Gnd diode operation */
   checkErr(niHSDIO_ReadStaticU32(iSes, &diodeResultsGnd));


Error:

   if (error == VI_SUCCESS)
   {
      /* print result */
      printf("Done without error.\n");
      printf("Vcc Diode Results = 0x%X \n", diodeResultsVcc);
      printf("Gnd Diode Results = 0x%X \n", diodeResultsGnd);
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
