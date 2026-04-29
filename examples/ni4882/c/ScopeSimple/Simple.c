/*
 * Filename - Simple.c
 *
 * This application demonstrates how to read from and write to the
 * Tektronix TDS 210 Two Channel Digital Real-Time Oscilloscope using
 * GPIB.
 *
 * This sample application is comprised of three basic parts:
 *
 * 1. Initialization
 * 2. Main Body
 * 3. Cleanup
 *
 * The Initialization portion consists of getting a handle to a
 * device and then clearing the device.
 *
 * In the Main Body, this application queries a device for its
 * identification code by issuing the '*IDN?' command. Many
 * instruments respond to this command with an identification string.
 * Note, 488.2 compliant devices are required to respond to this
 * command.
 *
 * The last step, Cleanup, takes the device offline.
 */

#include <stdio.h>
#include <stdlib.h>
#include <malloc.h>

#include "ni4882.h"

#define ARRAYSIZE          1024     // Size of read buffer

#define BDINDEX               0     // Board Index
#define PRIMARY_ADDR_OF_SCOPE 1     // Primary address of device
#define NO_SECONDARY_ADDR     0     // Secondary address of device
#define TIMEOUT               T10s  // Timeout value = 10 seconds
#define EOTMODE               1     // Enable the END message
#define EOSMODE               0     // Disable the EOS mode

static int  Dev;
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
   /*
    * ========================================================================
    *
    * INITIALIZATION SECTION
    *
    * ========================================================================
    */

   /*
    * The application brings the oscilloscope online using ibdev. A
    * device handle, Dev, is returned and is used in all subsequent
    * calls to the device.
    */
   Dev = ibdev(BDINDEX, PRIMARY_ADDR_OF_SCOPE, NO_SECONDARY_ADDR,
               TIMEOUT, EOTMODE, EOSMODE);
   if (Ibsta() & ERR)
   {
      printf("Unable to open device\nibsta = 0x%x iberr = %d\n",
             Ibsta(), Iberr());
      return 1;
   }

   /*
    * Clear the internal or device functions of the device.  If the
    * error bit ERR is set in ibsta, call GPIBCleanup with an error
    * message.
    */
   ibclr(Dev);
   if (Ibsta() & ERR)
   {
      GPIBCleanup(Dev, "Unable to clear device");
      return 1;
   }

   /*
    * ========================================================================
    *
    * MAIN BODY SECTION
    *
    * In this application, the Main Body communicates with the instrument
    * by writing a command to it and reading its response. This would be
    * the right place to put other instrument communication.
    *
    * ========================================================================
    */

   /*
    * The application issues the '*IDN?' command to the oscilloscope.
    */
   ibwrt(Dev, "*IDN?\n", 6L);
   if (Ibsta() & ERR)
   {
      GPIBCleanup(Dev, "Unable to set oscilloscope");
      return 1;
   }

   /*
    * The application reads the identification code in the form of an
    * ASCII string from the oscilloscope into the ValueStr variable.
    */
   ibrd(Dev, ValueStr, ARRAYSIZE);
   if (Ibsta() & ERR)
   {
      GPIBCleanup(Dev, "Unable to read data from oscilloscope");
      return 1;
   }

   /*
    * Assume that the returned string contains ASCII data. NULL
    * terminate the string using the value in ibcntl which is the
    * number of bytes read in. Use printf to display the string.
    */
   ValueStr[Ibcnt() - 1] = '\0';

   printf("Data read: %s\n", ValueStr);

   /*
    * ========================================================================
    *
    * CLEANUP SECTION
    *
    * ========================================================================
    */

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
