/*
 * Filename - Bdquery.c
 *
 * This sample application is comprised of three basic parts:
 *
 *  1. Initialization
 *  2. Main Body
 *  3. Cleanup
 *
 * The Initialization portion consists of getting a handle to a
 * board and then clearing a device.
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
#include <malloc.h>

#include "ni4882.h"

#define ARRAYSIZE          1024     // Size of read buffer

static int  Bd;                            // Board handle
static char ReadBuffer[ARRAYSIZE + 1];     // Read Buffer
static char ErrorMnemonic[29][5] = {
                              "EDVR", "ECIC", "ENOL", "EADR", "EARG",
                              "ESAC", "EABO", "ENEB", "EDMA", "",
                              "EOIP", "ECAP", "EFSO", "",     "EBUS",
                              "ESTB", "ESRQ", "",     "",      "",
                              "ETAB", "ELCK", "EARM", "EHDL",  "",
                              "",     "EWIP", "ERST", "EPWR" };


static void GPIBCleanup(int Bd, const char * ErrorMsg);


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
    * The application initializes the board using ibfind. A handle, Bd,
    * is returned and is used in all subsequent calls to the interface
    * board.
    */
   Bd = ibfind("GPIB0");
   if (Ibsta() & ERR)
   {
      GPIBCleanup(Bd, "Unable to open board");
      return 1;
   }

   /*
    * Your board needs to be the Controller-In-Charge in order to send
    * GPIB commands.  To accomplish this, the function ibsic is called.
    * ibsic also initializes the GPIB and asserts the GPIB interfaces
    * clear (IFC) line. If the error bit ERR is set in ibsta, call
    * GPIBCleanup with an error message.
    */
   ibsic(Bd);
   if (Ibsta() & ERR)
   {
      GPIBCleanup(Bd, "Unable to assert interface clear");
      return 1;
   }

   /*
    * Prior to issuing a device clear acommand or an ibwrt call, you
    * first need to set up the GPIB bus. This is accomplished using
    * ibcmd with the IEEE 488 command messages. A table of the command
    * messages can be found in either the online help or in Appendix A,
    * Multiline Interface Messages, in the "NI-488.2 Function Reference
    * Manual".
    *
    * To make board 0 a talker and a device at primary address 1 a
    * listener, we send the command message "@!" where '@' is the ASCII
    * equivalent of MTA0 (My Talker Address 0) and '!' is the ASCII
    * equivalent of MLA1 (My Listen Address 1).
    */
   ibcmd(Bd, "@!", 2L);
   if (Ibsta() & ERR)
   {
      GPIBCleanup(Bd, "Unable to send command");
      return 1;
   }

   /*
    * Send the GPIB Selected Device Clear (SDC) message (hex 04) to the
    * device to clear the internal device functions.  If the error bit
    * ERR is set in ibsta, call GPIBCleanup with an error message.
    */
   ibcmd(Bd, "\04", 1L);
   if (Ibsta() & ERR)
   {
      GPIBCleanup(Bd, "Unable to clear device");
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
   ibwrt(Bd, "*IDN?\n", 6L);
   if (Ibsta() & ERR)
   {
      GPIBCleanup(Bd, "Unable to write to the device");
      return 1;
   }

   /*
    * Before issuing an ibrd call, the GPIB bus needs to be set up ahead
    * of time. To make board 0 a listener and a device at primary address
    * 1 a talker, we send the command message " A" where ' ' (or space)
    * is the ASCII equivalent of MLA0 (My Listener Address 0) and 'A' is
    * the ASCII equivalent of MTA1 (My Talker Address 1).
    */
   ibcmd(Bd, " A", 2L);
   if (Ibsta() & ERR)
   {
      GPIBCleanup(Bd, "Unable to send command");
      return 1;
   }

   /*
    * Read the identification code by calling ibrd. If the ERR bit is
    * set in ibsta, call GPIBCleanup with an error message.
    */
   ibrd(Bd, ReadBuffer, ARRAYSIZE);
   if (Ibsta() & ERR)
   {
      GPIBCleanup(Bd, "Unable to read data from device");
      return 1;
   }

   /*
    * Assume that the returned string contains ASCII data. NULL terminate
    * the string using the value in ibcntl which is the number of bytes
    * read in. Use printf to display the string.
    */
   ReadBuffer[Ibcnt()] = '\0';

   printf("Data read: %s", ReadBuffer);

   /*
    * ========================================================================
    *
    * CLEANUP SECTION
    *
    * ========================================================================
    */

   /*
    * The board is taken offline.
    */
   ibonl(Bd, 0);

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
   printf("Error : %s\nibsta = 0x%x iberr = %d (%s)\n",
          ErrorMsg, Ibsta(), Iberr(), ErrorMnemonic[Iberr()]);
   if (ud != -1)
   {
      printf("Cleanup: Taking board offline\n");
      ibonl(ud, 0);
   }
}
