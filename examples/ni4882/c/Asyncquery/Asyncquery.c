/*
 * Filename - Asyncquery.c
 *
 * This sample application is comprised of three basic parts:
 *
 *  1. Initialization
 *  2. Main Body
 *  3. Cleanup
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
#include <string.h>
#include <conio.h>

#include "ni4882.h"


#define ARRAYSIZE 100            // Size of read buffer

static int  Dev;                        // Device handle
static char ReadBuffer[ARRAYSIZE + 1];  // Read data buffer
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
    * Assign a unique identifier to the device and store in the variable
    * Dev.  If the ERR bit is set in ibsta, call GPIBCleanup with an
    * error message. Otherwise, the device handle, Dev, is returned and
    * is used in all subsequent calls to the device.
    */
   #define BDINDEX               0     // Board Index
   #define PRIMARY_ADDR_OF_DMM   1     // Primary address of device
   #define NO_SECONDARY_ADDR     0     // Secondary address of device
   #define TIMEOUT               T10s  // Timeout value = 10 seconds
   #define EOTMODE               1     // Enable the END message
   #define EOSMODE               0     // Disable the EOS mode

   Dev = ibdev(BDINDEX, PRIMARY_ADDR_OF_DMM, NO_SECONDARY_ADDR,
               TIMEOUT, EOTMODE, EOSMODE);
   if (Ibsta() & ERR)
   {
      GPIBCleanup(Dev, "Unable to open device");
      return 1;
   }

   /*
    * Clear the internal or device functions of the device.  If the error
    * bit ERR is set in ibsta, call GPIBCleanup with an error message.
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
    * Request the identification code by sending the instruction '*IDN?'.
    * If the error bit ERR is set in ibsta, call GPIBCleanup with an error
    * message.
    */
   ibwrta(Dev, "*IDN?", 5L);
   if (Ibsta() & ERR)
   {
      GPIBCleanup(Dev, "Unable to write to device");
      return 1;
   }

   while (!(Ibsta() & CMPL))
   {
      /*
       * Call ibwait with a wait mask of zero.  When the wait mask
       * is zero, ibwait returns immediately with the updated ibsta
       * status word.
       */
      ibwait(Dev, 0);
      if (Ibsta() & ERR)
      {
         GPIBCleanup(Dev, "Error occurred while waiting for I/O to complete");
         return 1;
      }

      /*
       * Your application can do other work here while waiting for
       * the asynchronous I/O to complete. For example, it might
       * process the device readings or do any other useful work.
       */
   }

   /*
    * Read the identification code by calling ibrda. If the ERR bit is
    * set in ibsta, call GPIBCleanup with an error message.
    */
   ibrda(Dev, ReadBuffer, ARRAYSIZE);
   if (Ibsta() & ERR)
   {
      GPIBCleanup(Dev, "Unable to read data from device");
      return 1;
   }

   while (!(Ibsta() & CMPL))
   {
      /*
       * Call ibwait with a wait mask of zero.  When the wait mask
       * is zero, ibwait returns immediately with the updated ibsta
       * status word.
       */
      ibwait(Dev, 0);
      if (Ibsta() & ERR)
      {
         GPIBCleanup(Dev, "Error occurred while waiting for I/O to complete");
         return 1;
      }

      /*
       * Your application can do other work here while waiting for
       * the asynchronous I/O to complete. For example, it might
       * process the device readings or do any other useful work.
       */
   }

   /*
    * Assume that the returned string contains ASCII data. NULL terminate
    * the string using the value in ibcntl which is the number of bytes
    * read in. Use printf to display the string.
    */
   ReadBuffer[Ibcnt()] = '\0';
   printf("Returned string:  %s", ReadBuffer);

   /*
    * ========================================================================
    *
    * CLEANUP SECTION
    *
    * ========================================================================
    */

   /*
    * Take the device offline.
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
void GPIBCleanup(int ud, const char * ErrorMsg)
{
   printf("Error : %s\nibsta = 0x%x iberr = %d (%s)\n",
          ErrorMsg, Ibsta(), Iberr(), ErrorMnemonic[Iberr()]);
   if (ud != -1)
   {
      printf("Cleanup: Taking device offline\n");
      ibonl(ud, 0);
   }
}
