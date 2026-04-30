/*
 * Filename - ClearTrigger.c
 *
 * This application demonstrates how to clear and trigger the
 * Tektronix TDS 210 Two Channel Digital Real-Time Oscilloscope. The
 * oscilloscope is brought online and cleared. The command ibclr
 * resets the device's message processing parts (that is, clears the
 * input and output queues, discards unprocessed commands and so on).
 * The command '*RST' resets the internal device functions like the
 * current display setting. Next, the oscilloscope is set up to
 * acquire a waveform when it receives a GPIB trigger. Each time the
 * oscilloscope is triggered, the acquired waveform is read and
 * printed out to the application window. This is repeated until the
 * escape key (<ESC>) is pressed. Finally, the device is taken
 * offline.
 */

#include <stdio.h>
#include <stdlib.h>
#include <malloc.h>
#include <conio.h>
#include <string.h>

#include "ni4882.h"

#if _MSC_VER
// Redefine kbhit to _kbhit to avoid warnings in Microsoft C.
#define kbhit _kbhit
#endif

#define  ARRAYSIZE         2048     // Size of read buffer

#define BDINDEX               0     // Board Index
#define PRIMARY_ADDR_OF_SCOPE 1     // Primary address of device
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


int __cdecl main(void)
{
   int  Dev;
   int  LoopCount = 0;
   int  Done      = 0;
   char ScopeConfigString[] = "DAT:SOU CH1;:DAT:ENC ASCII;"
                              ":DAT:WID 1;:DAT:STAR 1;"
                              ":DAT:STOP 500;:HOR:MAIN:SCALE 5e-4\n";
   char CommandsWhenTriggeredString[] = "*DDT 'SEL:CH1 ON;:ACQ:STATE ON;:CURVE?'\n";

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
    * The application resets the internal device functions of the
    * oscilloscope by writing the command '*RST'.
    */
   ibwrt(Dev, "*RST\n", 5L);
   if (Ibsta() & ERR)
   {
      GPIBCleanup(Dev, "Unable to reset oscilloscope");
      return 1;
   }

   /*
    * The application resets the GPIB portion of the oscilloscope by
    * calling ibclr.
    */
   ibclr(Dev);
   if (Ibsta() & ERR)
   {
      GPIBCleanup(Dev, "Unable to clear oscilloscope");
      return 1;
   }

   /*
    * To be able to read the waveform, the oscilloscope's
    * characteristics are set using the commands contained in the
    * ScopeConfigString variable. The commands are combined using a
    * semicolon.
    *
    * ScopeConfigString contains :
    *
    * 'DAT:SOU CH1'         Sets the source of the waveform to be
    *                       read as channel 1.
    * 'DATA:ENC ASCII'      Indicates that the data is to be read in
    *                       ASCII format.
    * 'DAT:WID 1'           Specifies that one byte is to be read per
    *                       data point.
    * 'DAT:STAR 1'          Sets the first point in the waveform to be
    *                       transferred to 1.
    * 'DAT:STOP 500'        Sets the last point in the waveform to be
    *                       transferred to 500.
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
    * To acquire a waveform each time the oscilloscope is triggered,
    * the commands to acquire and read the waveform are stored using
    * the CommandsWhenTriggeredString.
    *
    * The CommandsWhenTriggeredString contains:
    *
    * '*DDT'         Instructs the oscilloscope to store a list of
    *                commands to execute every time the oscilloscope
    *                is triggered.
    * 'SEL:CH1 ON'   Selects channel 1 for the acquisition.
    * 'ACQ:STATE ON' Begins the acquisition.
    * 'CURVE?'       Requests the waveform reading from the
    *                oscilloscope.
    */
   ibwrt(Dev, CommandsWhenTriggeredString,
         strlen(CommandsWhenTriggeredString));
   if (Ibsta() & ERR)
   {
      GPIBCleanup(Dev, "Unable to set DDT string");
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
       * The oscilloscope is triggered using the command ibtrg. The
       * trigger causes the commands stored in
       * CommandsWhenTriggeredString to be executed.
       */
      ibtrg(Dev);
      if (Ibsta() & ERR)
      {
         GPIBCleanup(Dev, "Unable to trigger oscilloscope");
         return 1;
      }

      /*
       * The application reads the waveform as an ASCII string into
       * the ValueStr variable.
       */
      ibrd(Dev, ValueStr, ARRAYSIZE);
      if (Ibsta() & ERR)
      {
         GPIBCleanup(Dev, "Unable to read curve data");
         return 1;
      }

      LoopCount++;

      /*
       * The string returned by ibrd is a binary string whose length
       * is specified by the byte count in ibcntl. However, the
       * oscilloscope sends measurements in the form of ASCII strings.
       * Because of this, it is possible to add a NULL character to
       * the end of the data received and use the printf function to
       * display the ASCII response. The following code illustrates
       * that.
       */
      ValueStr[Ibcnt() - 1] = '\0';

      printf("\nWaveform No: %d\n\n", LoopCount);
      printf("%s", ValueStr);
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
