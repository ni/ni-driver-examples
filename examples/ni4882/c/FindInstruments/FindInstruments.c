/*
 * Filename - Findinstruments.c
 *
 * This sample application initializes the bus and the GPIB interface
 * board so that the GPIB board is Controller-In-Charge (CIC). It
 * then proceeds to find all the instruments on the GPIB bus and
 * print the PAD (Primary Address) and SAD (Secondary Address) of
 * each instrument.
 */

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <conio.h>

#include "ni4882.h"

#define GPIB0        0                 // Board handle

static int        Num_Instruments;            // Number of instruments on GPIB
static int        PAD;                        // Primary address
static int        SAD;                        // Secondary address
static int        loop;                       // Loop counter
static Addr4882_t Instruments[32];            // Array of primary addresses
static Addr4882_t Result[31];                 // Array of listen addresses
static char ErrorMnemonic[29][5] = {
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
    * Your board needs to be the Controller-In-Charge in order to find
    * all instrument on the GPIB.  To accomplish this, the function
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
    * address 0 by default.  This array (Instruments) will be given to
    * the function FindLstn to find all instruments.  The constant
    * NOADDR, defined in NI488.H, signifies the end of the array.
    */
   for (loop = 0; loop < 30; loop++)
   {
      Instruments[loop] = (Addr4882_t)(loop + 1);
   }
   Instruments[30] = NOADDR;

   /*
    * Print message to tell user that the program is searching for all
    * active listeners.  Find all of the instruments on the bus.  Store
    * the instrument addresses in the array Result. Note, the
    * instruments must be powered on and connected with a GPIB cable in
    * order for FindLstn to detect them. If the error bit ERR is set in
    * ibsta, call GPIBCleanup with an error message.
    */
   printf("Finding all instruments on the bus...\n\n");

   FindLstn(GPIB0, Instruments, Result, 31);
   if (Ibsta() & ERR)
   {
      GPIBCleanup(GPIB0, "Unable to issue FindLstn call");
      return 1;
   }

   /*
    * ibcntl contains the actual number of addresses stored in the
    * Result array. Assign the value of ibcntl to the variable
    * Num_Instruments. Print the number of instruments found.
    */
   Num_Instruments = Ibcnt();

   printf("Number of instruments found = %d\n\n", Num_Instruments);

   /*
    * The Result array contains the addresses of all the instruments
    * found by FindLstn. Use the constant NOADDR, as defined in
    * NI488.H, to signify the end of the array.
    */
   Result[Num_Instruments] = NOADDR;

   /*
    * Print out each instrument's PAD and SAD, one at a time.
    *
    * Establish a FOR loop to print out the information. The variable
    * LOOP will serve as a counter for the FOR loop and as the index
    * to the array Result.
    */
   for (loop = 0; loop < Num_Instruments; loop++)
   {
      /*
       * The low byte of the instrument address is the primary
       * address. Assign the variable PAD the primary address of the
       * instrument. The macro GetPAD, defined in NI488.H, returns
       * the low byte of the instrument address.
       */
      PAD = GetPAD(Result[loop]);

      /*
       * The high byte of the instrument address is the secondary
       * address. Assign the variable SAD the primary address of the
       * instrument. The macro GetSAD, defined in NI488.H, returns
       * the high byte of the instrument address.
       */
      SAD = GetSAD(Result[loop]);

      if (SAD == NO_SAD)
      {
         printf("The instrument at Result[%d]: PAD = %d SAD = NONE\n",
                loop, PAD);
      }
      else
      {
         printf("The instrument at Result[%d]: PAD = %d SAD = %d\n",
                loop, PAD, SAD);
      }

   } /* End of FOR loop */


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
    printf("Error : %s\nibsta = 0x%x iberr = %d (%s)\n",
           ErrorMsg, Ibsta(), Iberr(), ErrorMnemonic[Iberr()]);
    printf("Cleanup: Taking board offline\n");
    ibonl(ud, 0);
}
