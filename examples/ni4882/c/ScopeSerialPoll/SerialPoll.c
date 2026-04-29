/*
 * Filename - SerialPoll.c
 *
 * This application demonstrates how to serial poll the Tektronix TDS
 * 210 Two Channel Digital Real-Time Oscilloscope using GPIB.
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
 * The oscilloscope can be programmed to request service when it is
 * ready to be serviced. In this application, the oscilloscope is
 * brought online and set up to acquire waveforms. The application then
 * waits until the device requests service. The oscilloscope requests
 * service when it completes the waveform acquisition. The device is
 * then serial polled to see if it requested service. If it did, the
 * waveform is read and printed. This is repeated until the escape key
 * (<ESC>) is pressed.
 */

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <malloc.h>
#include <conio.h>

#include "ni4882.h"

#if _MSC_VER
// Redefine kbhit to _kbhit to avoid warnings in Microsoft C.
#define kbhit _kbhit
#endif

#define ARRAYSIZE          2048     // Size of read buffer

#define BDINDEX               0     // Board Index
#define PRIMARY_ADDR_OF_SCOPE 1     // Primary address of device
#define NO_SECONDARY_ADDR     0     // Secondary address of device
#define TIMEOUT               T10s  // Timeout value = 10 seconds
#define EOTMODE               1     // Enable the END message
#define EOSMODE               0     // Disable the EOS mode

// Response byte after serial polling device.
#define SERPOLLRESPONSE    0x50
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


int __cdecl main(void)
{
   int  Dev;
   int  LoopCount = 0;
   int  Done      = 0;
   char ResByte;
   char ScopeConfigString[] = "DAT:SOU CH1;:DAT:ENC ASCII;:DAT:WID 1;"
                              ":DAT:STAR 1;:DAT:STOP 500;"
                              ":HOR:MAIN:SCALE 5e-4\n";
   char AcqWaveformString[] = "SEL:CH1 ON;:ACQ:MOD SAMPLE\n";

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
    * The waveform characteristics are set using the commands contained
    * in the ScopeConfigString character array variable. The commands
    * are combined using a semicolon.
    *
    * ScopeConfigString contains:
    *
    * 'DAT:SOU CH1'         Sets the source of the waveform to be
    *                       read as channel 1
    * 'DATA:ENC ASCII'      Indicates that the data is to be read in
    *                       ASCII format
    * 'DAT:WID 1'           Specifies that one byte is to be read per
    *                       data point
    * 'DAT:STAR 1'          Sets the first point in the waveform to
    *                       be transferred to 1
    * 'DAT:STOP 500'        Sets the last point in the waveform to be
    *                       transferred to 500
    * 'HOR:MAIN SCALE 5e-4' Sets the horizontal scale to 5 x 10-4
    *                       seconds per unit.
    */
   ibwrt(Dev, ScopeConfigString, strlen(ScopeConfigString));
   if (Ibsta() & ERR)
   {
      GPIBCleanup(Dev, "Unable to set waveform characteristics");
      return 1;
   }

   /*
    * The AcqWaveformString string contains the following commands:
    *
    * 'SEL:CH1 ON'     Selects channel 1 of the oscilloscope
    * 'ACQ:MOD SAMPLE' Sets the oscilloscope to display the first
    *                  sampled value that was taken during the
    *                  acquisition.
    */
   ibwrt(Dev, AcqWaveformString, strlen(AcqWaveformString));
   if (Ibsta() & ERR)
   {
      GPIBCleanup(Dev, "Unable to set up acquisition");
      return 1;
   }

   /*
    * '*SRE 16' sets the MAV bit in the Service Request Enable
    * Register. This action enables the oscilloscope to request
    * service and to set the MAV bit in the Status Byte Register when
    * its output queue has data in it.
    */
   ibwrt(Dev, "*SRE 16\n", 8L);
   if (Ibsta() & ERR)
   {
      GPIBCleanup(Dev, "Unable to set SRE");
      return 1;
   }

   /*
    * The acquisition is begun using 'ACQUIRE:STATE ON'.
    */
   ibwrt(Dev, "ACQUIRE:STATE ON\n", 17L);
   if (Ibsta() & ERR)
   {
      GPIBCleanup(Dev, "Unable to set state on");
      return 1;
   }

   /*
    * The application writes the command 'CURVE?' to the oscilloscope.
    * This command requests the oscilloscope to supply the waveform
    * data.
    */
   ibwrt(Dev, "CURVE?\n", 7L);
   if (Ibsta() & ERR)
   {
      GPIBCleanup(Dev, "Unable to request curve data");
      return 1;
   }

   printf("\n\nAcquiring waveform. Press escape to quit.\n");

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
       * The application waits for either the oscilloscope to request
       * service or the call to timeout. The wait mask supplied to
       * ibwait contains two bits: TIMO and RQS. The TIMO bit limits
       * the wait period to the timeout value that was specified in
       * the ibdev call; the RQS bit instructs the application to wait
       * until the device has requested service. The ibwait call is
       * completed when one or more of the specified wait conditions
       * becomes true.
       */
      ibwait(Dev, TIMO | RQS);
      if (Ibsta() & ERR)
      {
         GPIBCleanup(Dev, "Error occurred while waiting for Service Request");
         return 1;
      }

      /*
       * If after the ibwait call, the RQS bit in ibsta is not set,
       * the Oscilloscope did not request sevice.
       */
      if (!(Ibsta() & RQS))
      {
         GPIBCleanup(Dev, "Error : Oscilloscope did not request service");
         return 1;
      }

      /*
       * After the wait has completed, the serial poll response byte
       * is checked.
       */
      ibrsp(Dev, &ResByte);
      if (Ibsta() & ERR)
      {
         GPIBCleanup(Dev, "Unable to serial poll oscilloscope");
         return 1;
      }

      /*
       * If the value is not 0x50, the device did not request service
       * as a result of completing the acquisition. In this case, an
       * error message is printed and the program exited.
       */
      if (ResByte != SERPOLLRESPONSE)
      {
         printf("Error : Unexpected serial poll response. Expected : 0x%x "
                "Received : 0x%x\n", SERPOLLRESPONSE, ResByte);
         printf("Cleanup: Taking device off-line\n");
         ibonl(Dev, 0);
         return 1;
      }

      /*
       * The Curve string from the oscilloscope is read into the
       * character variable ValueStr
       */
      ibrd(Dev, ValueStr, ARRAYSIZE);
      if (Ibsta() & ERR)
      {
         GPIBCleanup(Dev, "Unable to read curve data");
         return 1;
      }

      LoopCount++;

      /*
       * The string returned by ibrd is a binary string whose length is
       * specified by the byte count in ibcntl. However, the oscilloscope
       * sends measurements in the form of ASCII strings. Because of
       * this, it is possible to add a NULL character to the end of the
       * data received and use the printf function to display the ASCII
       * response. The following code illustrates that.
       */
      ValueStr[Ibcnt() - 1] = '\0';

      printf("\nWaveform No: %d\n\n", LoopCount);
      printf("%s", ValueStr);

      /*
       * The application writes the command 'CURVE?' to the oscilloscope.
       * This command requests the oscilloscope to supply the waveform
       * data before serial polling it again.
       */
      ibwrt(Dev, "CURVE?\n", 7L);
      if (Ibsta() & ERR)
      {
         GPIBCleanup(Dev, "Unable to request curve data");
         return 1;
      }
   }

   printf("\n\nAcquisition Done\n\n");

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
