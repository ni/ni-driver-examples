/*
 * Filename - 4882query.c
 *
 * This sample application is comprised of three basic parts:
 *
 *  1. Initialization
 *  2. Main Body
 *  3. Cleanup
 *
 * The Initialization portion consists of initializing the bus and the
 * GPIB interface board so that the GPIB board is Controller-In-Charge
 * (CIC). Next it finds all the listeners and then clears all the
 * devices on the bus.
 *
 * In the Main Body, this application queries a device for its
 * identification code by issuing the '*IDN?' command. Many
 * instruments respond to this command with an identification string.
 * Note, 488.2 compliant devices are required to respond to this
 * command.
 *
 * The last step, Cleanup, takes the board offline.
 */

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <conio.h>

#include "ni4882.h"

#define ARRAYSIZE  100                 // Size of read buffer
#define GPIB0        0                 // Board handle

static int        Num_Listeners;              // Number of listeners on GPIB
static int        loop;                       // Loop counter
static Addr4882_t Instruments[32];            // Array of primary addresses
static Addr4882_t Result[31];                 // Array of listen addresses
static char       ReadBuffer[ARRAYSIZE + 1];  // Read data buffer
static char       ErrorMnemonic[29][5] = {
                              "EDVR", "ECIC", "ENOL", "EADR", "EARG",
                              "ESAC", "EABO", "ENEB", "EDMA", "",
                              "EOIP", "ECAP", "EFSO", "",     "EBUS",
                              "ESTB", "ESRQ", "",     "",      "",
                              "ETAB", "ELCK", "EARM", "EHDL",  "",
                              "",     "EWIP", "ERST", "EPWR" };


static void GPIBCleanup(int ud, const char * ErrorMsg);


int __cdecl main(void)
{
   /*
    * ====================================================================
    *
    *  INITIALIZATION SECTION
    *
    * ====================================================================
    */

   /*
    * Your board needs to be the Controller-In-Charge in order to find
    * all listeners on the GPIB.  To accomplish this, the function
    * SendIFC is called.  If the error bit ERR is set in ibsta, call
    * GPIBCleanup with an error message.
    */
   SendIFC(GPIB0);
   if (Ibsta() & ERR)
   {
      GPIBCleanup(GPIB0, "Unable to open board");
      return 1;
   }

   /*
    * Create an array containing all valid GPIB primary addresses,
    * except for primary address 0.  Your GPIB interface board is at
    * address 0 by default.  This array (Instuments) will be given to
    * the function FindLstn to find all listeners.  The constant NOADDR,
    * defined in NI488.H, signifies the end of the array.
    */
   for (loop = 0; loop < 30; loop++)
   {
      Instruments[loop] = (Addr4882_t)(loop + 1);
   }
   Instruments[30] = NOADDR;

   /*
    * Print message to tell user that the program is searching for all
    * active listeners.  Find all of the listeners on the bus.  Store
    * the listen addresses in the array Result.  Note, the instruments
    * must be powered on and connected with a GPIB cable in order for
    * FindLstn to detect them.If the error bit ERR is set in ibsta, call
    * GPIBCleanup with an error message.
    */
   printf("Finding all listeners on the bus...\n\n");

   FindLstn(GPIB0, Instruments, Result, 31);
   if (Ibsta() & ERR)
   {
      GPIBCleanup(GPIB0, "Unable to issue FindLstn call");
      return 1;
   }

   /*
    * ibcntl contains the actual number of addresses stored in the
    * Result array. Assign the value of ibcntl to the variable
    * Num_Listeners. Print the number of listeners found.
    */
   Num_Listeners = Ibcnt();

   printf("Number of Instruments found = %d\n\n", Num_Listeners);

   /*
    * The Result array contains the addresses of all listening devices
    * found by FindLstn. Use the constant NOADDR, as defined in
    * ni4882.h, to signify the end of the array.
    */
   Result[Num_Listeners] = NOADDR;

   /*
    * DevClearList will send the GPIB Selected Device Clear (SDC)
    * command message to all the devices on the bus.  If the error bit
    * ERR is set in ibsta, call GPIBCleanup with an error message.
    */
   DevClearList(GPIB0, Result);
   if (Ibsta() & ERR)
   {
      GPIBCleanup(GPIB0, "Unable to clear devices");
      return 1;
   }

   /*
    * ====================================================================
    *
    * MAIN BODY SECTION
    *
    * In this application, the Main Body communicates with the
    * instruments by writing a command to them and reading the individual
    * responses. This would be the right place to put other instrument
    * communication.
    *
    * ====================================================================
    */

   /*
    * Send the identification query to each listen address in the array
    * (Result) using SendList.  The constant NLend, defined in NI488.H,
    * instructs the function SendList to append a linefeed character
    * with EOI asserted to the end of the message.  If the error bit ERR
    * is set in ibsta, call GPIBCleanup with an error message.
    */
   SendList(GPIB0, Result, "*IDN?", 5L, NLend);
   if (Ibsta() & ERR)
   {
      GPIBCleanup(GPIB0, "Unable to write to devices");
      return 1;
   }

   /*
    * Read each device's identification code, one at a time.
    *
    * Establish a FOR loop to read each one of the device's
    * identification code. The variable LOOP will serve as a counter
    * for the FOR loop and as the index to the array Result.
    */
   for (loop = 0; loop < Num_Listeners; loop++)
   {
      /*
       * Read the name identification response returned from each
       * device. Store the response in the array ReadBuffer.  The
       * constant STOPend, defined in NI488.H, instructs the
       * function Receive to terminate the read when END is detected.
       * If the error bit ERR is set in ibsta, call GPIBCleanup with
       * an error message.
       */
      Receive(GPIB0, Result[loop], ReadBuffer, ARRAYSIZE, STOPend);
      if (Ibsta() & ERR)
      {
         GPIBCleanup(GPIB0, "Unable to read from a device");
         return 1;
      }

      /*
       * Assume that the returned string contains ASCII data. NULL
       * terminate the string using the value in ibcntl which is
       * the number of bytes read in. Use printf to display the
       * string.
       */
      ReadBuffer[Ibcnt()] = '\0';
      printf("Returned string: %s", ReadBuffer);

   } /* End of FOR loop */

   /*
    * ====================================================================
    *
    * CLEANUP SECTION
    *
    * ====================================================================
    */

   /*
    * Take the board offline.
    */
   ibonl(GPIB0, 0);

   return 0;
}


/*
 * After each GPIB call, the application checks whether the call
 * succeeded. If an NI-488.2 call fails, the GPIB driver sets the
 * corresponding bit in the global status variable. If the call
 * failed, this procedure prints an error message, takes the board
 * offline and exits.
 */
void GPIBCleanup(int ud, const char * ErrorMsg)
{
   printf("Error : %s\nibsta = GPIB0x%x iberr = %d (%s)\n",
          ErrorMsg, Ibsta(), Iberr(), ErrorMnemonic[Iberr()]);
   printf("Cleanup: Taking board offline\n");
   ibonl(ud, 0);
}
