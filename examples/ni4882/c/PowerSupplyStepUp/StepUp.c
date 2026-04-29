/*
 * Filename - StepUp.c
 *
 * This application demonstrates how to step up the current supplied
 * by the Tektronix PS2520G Programmable Power Supply. The current
 * supplied is initially set to zero and then increased by a fixed
 * step value until the maximum value is reached. At each stage, the
 * current reading is obtained and printed to the application window.
 */

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <malloc.h>

#include "ni4882.h"

#define MaxCurrent          1.5
#define Step               0.25
#define ARRAYSIZE          1024     // Size of read buffer

#define BDINDEX               0     // Board Index
#define PRIMARY_ADDR_OF_PPS   1     // Primary address of device
#define NO_SECONDARY_ADDR     0     // Secondary address of device
#define TIMEOUT               T10s  // Timeout value = 10 seconds
#define EOTMODE               1     // Enable the END message
#define EOSMODE               0     // Disable the EOS mode

static char ValueStr[ARRAYSIZE + 1];
static char ErrorMnemonic[29][5] = {
                              "EDVR", "ECIC", "ENOL", "EADR", "EARG",
                              "ESAC", "EABO", "ENEB", "EDMA", "",
                              "EOIP", "ECAP", "EFSO", "",     "EBUS",
                              "ESTB", "ESRQ", "",     "",      "",
                              "ETAB", "ELCK", "EARM", "EHDL",  "",
                              "",     "EWIP", "ERST", "EPWR" };


static void GPIBCleanup(int Dev, const char * ErrorMsg);


int __cdecl main(void)
{
   int    Dev;
   double Current = 0;
   char   CurrentValueString[30];

   /*
    * The application brings the power supply online using ibdev. A
    * device handle, Dev, is returned and is used in all subsequent
    * calls to the device.
    */
   Dev = ibdev(BDINDEX, PRIMARY_ADDR_OF_PPS, NO_SECONDARY_ADDR,
               TIMEOUT, EOTMODE, EOSMODE);
   if (Ibsta() & ERR)
   {
      printf("Unable to open device\nibsta = 0x%x iberr = %d\n",
             Ibsta(), Iberr());
      return 1;
   }

   /*
    * The application selects output 1 using the command
    * 'INST:NSEL 1'.
    */
   ibwrt(Dev, "INST:NSEL 1\n", 12L);
   if (Ibsta() & ERR)
   {
      GPIBCleanup(Dev, "Unable to select output 1");
      return 1;
   }

   /*
    * Initial Value of the Current Variable.
    */
   sprintf(CurrentValueString, "SOURCE:CURRENT 0\n");

   while (Current <= MaxCurrent)
   {
      /*
       * The application sets the value of the actual output current
       * using the command stored in the CurrentValueString variable.
       * CurrentValueString contains the command 'SOURCE:CURRENT'
       * Current which sets the value of the actual output current to
       * the value of the variable Current.
       */
      ibwrt(Dev, CurrentValueString, strlen(CurrentValueString));
      if (Ibsta() & ERR)
      {
         GPIBCleanup(Dev, "Unable to set Power Supply Current");
         return 1;
      }

      /*
       * The power supply is requested to return the value of the
       * actual output current.
       */
      ibwrt(Dev, "CURRENT?\n", 9L);
      if (Ibsta() & ERR)
      {
         GPIBCleanup(Dev, "Unable to set Power Supply");
         return 1;
      }

      /*
       * The value of the actual output current is read into the
       * ValueStr variable.
       */
      ibrd(Dev, ValueStr, ARRAYSIZE);
      if (Ibsta() & ERR)
      {
         GPIBCleanup(Dev, "Unable to read data from Power supply");
         return 1;
      }

      /*
       * The string returned by ibrd is a binary string whose length
       * is specified by the byte count in ibcntl. However, the
       * device sends measurements in the form of ASCII strings.
       * Because of this, it is possible to add a NULL character to
       * the end of the data received and use the printf function to
       * display the ASCII response. The following code illustrates
       * that.
       */
      ValueStr[Ibcnt() - 1] = '\0';

      printf("Data read : %s\n", ValueStr);

      /*
       * The value of the Current variable is increased by Step.
       */
      Current += Step;
      sprintf(CurrentValueString, "SOURCE:CURRENT %f\n", Current);
   }

   /*
    * The device is taken offline.
    */
   ibonl(Dev, 0);

   return 0;
}

/*
 * After each GPIB call, the application checks whether the call
 * succeeded. If an NI-488.2 call fails, the GPIB driver sets the
 * corresponding bit in the global status variable. If the call
 * failed, this procedure prints an error message, takes the device
 * offline and exits.
 */
void GPIBCleanup(int Dev, const char * ErrorMsg)
{
    printf("Error : %s\nibsta = 0x%x iberr = %d (%s)\n",
           ErrorMsg, Ibsta(), Iberr(), ErrorMnemonic[Iberr()]);
    if (Dev != -1)
    {
       printf("Cleanup: Taking device offline\n");
       ibonl(Dev, 0);
    }
}
