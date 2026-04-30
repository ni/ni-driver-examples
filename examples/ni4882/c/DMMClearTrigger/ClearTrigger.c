/*
 * Filename - ClearTrigger.c
 *
 * This application demonstrates how to clear and trigger the Fluke 45
 * Dual Display Multimeter. The multimeter is brought online and
 * cleared. The clear resets the device's message processing parts
 * (that is, clears the input and output queues, discards unprocessed
 * commands and so on), but it does not reset internal device functions
 * (like the current display setting). Next, the multimeter is set up
 * receive a trigger. Each time the multimeter is triggered, the
 * acquired waveform is read and printed out to the application window.
 * This is repeated until the escape key (<ESC>) is pressed. Finally,
 * the device is taken offline.
 */

#include <stdio.h>
#include <stdlib.h>
#include <malloc.h>
#include <conio.h>

#include "ni4882.h"

#if _MSC_VER
// Redefine kbhit to _kbhit to avoid warnings in Microsoft C.
#define kbhit _kbhit
#endif

#define ARRAYSIZE          1024     // Size of read buffer

#define BDINDEX               0     // Board Index
#define PRIMARY_ADDR_OF_DMM   1     // Primary address of device
#define NO_SECONDARY_ADDR     0     // Secondary address of device
#define TIMEOUT               T10s  // Timeout value = 10 seconds
#define EOTMODE               1     // Enable the END message
#define EOSMODE               0     // Disable the EOS mode

#define ESCAPE             0x1b

static char ValueStr[ARRAYSIZE + 1];
static char ErrorMnemonic[29][5] = {
                              "EDVR", "ECIC", "ENOL", "EADR", "EARG",
                              "ESAC", "EABO", "ENEB", "EDMA", "",
                              "EOIP", "ECAP", "EFSO", "",     "EBUS",
                              "ESTB", "ESRQ", "",     "",      "",
                              "ETAB", "ELCK", "EARM", "EHDL",  "",
                              "",     "EWIP", "ERST", "EPWR" };


static void GPIBCleanup(int Dev, const char * ErrorMsg);


int __cdecl main()
{
    int Dev;
    int Done = 0;

   /*
    * The application brings the multimeter online using ibdev. A
    * device handle, Dev, is returned and is used in all subsequent
    * calls to the device.
    */
   Dev = ibdev(BDINDEX, PRIMARY_ADDR_OF_DMM, NO_SECONDARY_ADDR,
               TIMEOUT, EOTMODE, EOSMODE);
   if (Ibsta() & ERR)
   {
      printf("Unable to open Device\nibsta = 0x%x iberr = %d\n",
             Ibsta(), Iberr());
      return 1;
   }

   /*
    * The application resets the GPIB portion of the device by
    * calling ibclr.
    */
   ibclr(Dev);
   if (Ibsta() & ERR)
   {
      GPIBCleanup(Dev, "Unable to clear multimeter");
      return 1;
   }

   /*
    * The type of trigger is set using the command 'TRIGGER 5'. The
    * number 5 indicates that when the multimeter is triggered, the
    * IEEE-488 Group Execute Trigger message will be sent to it.
    */
   ibwrt(Dev, "TRIGGER 5\n", 10L);
   if (Ibsta() & ERR)
   {
      GPIBCleanup(Dev, "Unable to set trigger type");
      return 1;
   }

   printf("Press escape when ready to quit\n");

   while (!Done)
   {
      if (kbhit())
      {
         if (_getch() == ESCAPE)
         {
            Done = 1;
            continue;
         }
      }

      /*
       * The application triggers the multimeter using the command
       * ibtrg.
       */
      ibtrg(Dev);
      if (Ibsta() & ERR)
      {
         GPIBCleanup(Dev, "Unable to trigger multimeter");
         return 1;
      }

      /*
       * The application writes the 'VAL1?' command to the multimeter.
       * The 'VAL1?' command causes the multimeter to generate a
       * string that contains the primary display value.
       */
      ibwrt(Dev, "VAL1?\n", 6L);
      if (Ibsta() & ERR)
      {
         GPIBCleanup(Dev, "Unable to set multimeter");
         return 1;
      }

      /*
       * The application reads the ASCII string from the multimeter
       * into the variable ValueStr.
       */
      ibrd(Dev, ValueStr, ARRAYSIZE);
      if (Ibsta() & ERR)
      {
         GPIBCleanup(Dev, "Unable to read data from multimeter");
         return 1;
      }

      /*
       * The string returned by ibrd is a binary string whose length is
       * specified by the byte count in ibcntl. However, the multimeter
       * sends measurements in the form of ASCII strings. Because of
       * this, it is possible to add a NULL character to the end of the
       * data received and use the printf function to display the ASCII
       * response. The following code illustrates that.
       */
      ValueStr[Ibcnt() - 1] = '\0';

      printf("Data read after triggering: %s\n", ValueStr);
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
      printf("Cleanup: Taking device off-line\n");
      ibonl (Dev, 0);
   }
}
