/*
 * Filename - SerialPoll.c
 *
 * This application demonstrates how to serial poll the Fluke 45 Dual
 * Display Multimeter using GPIB.
 *
 * A user can configure devices to request service from the GPIB
 * controller. A device asserts the SRQ signal to request service. All
 * devices share a single SRQ signal. A GPIB controller can determine
 * which device (or devices) are requesting service by serially polling
 * all of the devices. Serial polling a device does the following:
 *
 *  1. Retrieves seven bits of status information from the device.
 *  2. Retrieves a bit indicating whether the device is requesting
 *     service.
 *  3. Instructs the device to stop requesting service until a new
 *     reason to request service occurs.
 *
 * In this application, the multimeter is brought online and set up
 * to supply a reading at regular intervals. When the multimeter has
 * an output, it asserts the SRQ line. The device is then serial
 * polled to see if it requested service. If it did, the measurement
 * is read and printed. This is repeated until the escape key (<ESC>)
 * is pressed.
 */

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
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

// Response byte after serial polling device.
#define SERIALPOLLRESPONSE    0x60

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
   int  Dev;
   int  Done = 0;
   char ResByte;

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
    * '*RST' resets the Fluke 45.
    */
   ibwrt(Dev, "*RST\n", 5L);
   if (Ibsta() & ERR)
   {
      GPIBCleanup(Dev, "Unable to reset the multimeter");
      return 1;
   }

   /*
    * '*ESE 1' sets the OPC bit in the Event Status Enable Register.
    */
   ibwrt(Dev, "*ESE 1\n", 7L);
   if (Ibsta() & ERR)
   {
      GPIBCleanup(Dev, "Unable to set ESE");
      return 1;
   }

   /*
    * '*SRE 32' sets the ESB bit in the Service Request Enable
    * Register. This enables the device to request service.
    */
   ibwrt(Dev, "*SRE 32\n", 8L);
   if (Ibsta() & ERR)
   {
      GPIBCleanup(Dev, "Unable to set SRE");
      return 1;
   }

   /*
    * The measurement rate is set using the command 'RATE S'. 'S'
    * indicates that the multimeter will supply a reading every
    * 2.5 seconds.
    */
   ibwrt(Dev, "RATE S\n", 7L);
   if (Ibsta() & ERR)
   {
      GPIBCleanup(Dev, "Unable to set rate of measurement");
      return 1;
   }

   printf("Press escape when ready to quit\n");

   /*
    * The while loop is executed until the user presses <esc>.
    */
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
       * The '*CLS' command clears the multimeter status data
       * structures.
       */
      ibwrt(Dev, "*CLS\n", 5L);
      if (Ibsta() & ERR)
      {
         GPIBCleanup(Dev, "Unable to clear Event Registers");
         return 1;
      }

      /*
       * The '*OPC' command instructs the multimeter to assert the
       * Operation Complete bit in the Standard Event Status Register.
       * When the multimeter is ready to supply a reading, it is this
       * command that causes it to assert the GPIB SRQ line.
       */
      ibwrt(Dev, "*OPC\n", 5L);
      if (Ibsta() & ERR)
      {
         GPIBCleanup(Dev, "Unable to set Operation Complete Bit");
         return 1;
      }

      /*
       * The application then uses ibwait to wait for either the
       * multimeter to request service or the call to timeout. The wait
       * mask supplied to ibwait contains two bits: TIMO and RQS. The
       * TIMO bit limits the wait period to the timeout value that was
       * specified in the ibdev call; the RQS bit instructs the
       * application to wait until the device has requested service.
       * The ibwait call is completed when one or more of the specified
       * wait conditions becomes true.
       */
      ibwait(Dev, TIMO | RQS);
      if (Ibsta() & ERR)
      {
         GPIBCleanup(Dev, "Error occurred while waiting for Service Request");
         return 1;
      }

      /*
       * After the wait has completed, the global variable ibsta is
       * checked to ensure that the wait ended because the device
       * requested service (for example, RQS is set in ibsta) and not
       * because the timeout period has elapsed.
       */
      if (!(Ibsta() & RQS))
      {
         GPIBCleanup(Dev, "Multimeter did not request service");
         return 1;
      }

      /*
       * If the multimeter has requested service, it is serial polled,
       * using ibrsp. The serial poll response byte is returned in the
       * parameter ResByte.
       */
      ibrsp(Dev, &ResByte);
      if (Ibsta() & ERR)
      {
         GPIBCleanup(Dev, "Unable to serial poll multimeter");
         return 1;
      }

      /*
       * If the value of ResByte is not 0x60, the multimeter did not
       * request service as a result of the output queue being full.
       * In this case, the device is taken offline and the program
       * exited.
       */
      if (ResByte != SERIALPOLLRESPONSE)
      {
         printf("Error : Unexpected serial poll response. Expected : 0x%x"
                "Received : 0x%x\n", SERIALPOLLRESPONSE, ResByte);
         printf("Cleanup: Taking device off-line\n");
         ibonl(Dev, 0);
         return 1;
      }

      /*
       * The application writes the 'VAL1?' command to the multimeter.
       * The 'VAL1?' command causes the multimeter to generate a string
       * that contains the primary display value.
       */
      ibwrt(Dev, "VAL1?\n", 6L);
      if (Ibsta() & ERR)
      {
         GPIBCleanup(Dev, "Unable to request data from multimeter");
         return 1;
      }

      /*
       * The application reads the ASCII string from the multimeter
       * into the variable ValueStr.
       */
      ibrd(Dev, ValueStr, ARRAYSIZE);
      if (Ibsta() & ERR)
      {
         GPIBCleanup(Dev, "Unable to read from multimeter");
         return 1;
      }

      /*
       * The string returned by ibrd is a binary string whose length
       * is specified by the byte count in ibcntl. However, the
       * multimeter sends measurements in the form of ASCII strings.
       * Because of this, it is possible to add a NULL character to
       * the end of the data received and use the printf function to
       * display the ASCII response. The following code illustrates
       * that.
       */
      ValueStr[Ibcnt() - 1] = '\0';

      printf("Value read is %s \n", ValueStr);
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
      ibonl(Dev, 0);
   }
}
