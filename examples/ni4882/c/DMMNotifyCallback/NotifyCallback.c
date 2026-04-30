
/*
 * Filename - NotifyCallback.c
 *
 * This application demonstrates how to serial poll the Fluke 45 Dual
 * Display Multimeter using GPIB.
 *
 * A user can configure devices to request service from the GPIB
 * controller. A device asserts the SRQ signal to request service. All
 * devices share a single SRQ signal. A GPIB controller can determine
 * which device (or devices) are requesting service by enabling
 * autopolling and using an ibnotify callback. The callback will be
 * invoked in a separate thread from the rest of the application.
 * Because of this, the global status functions should not be used
 * and instead the thread safe versions should be used: ThreadIbsta(),
 * ThreadIbcnt(), ThreadIberr().
 *
 * In this application, the multimeter is brought online and set up
 * to supply a reading at regular intervals. When the multimeter has
 * an output, it asserts the SRQ line. The ibnotify callback function
 * is invoked and the status byte from the autopoll is examined to see
 * if the instrument requested service. If it did, the measurement
 * is read and printed. This is repeated ten times.
 */

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <conio.h>

#include "ni4882.h"

#if _MSC_VER
// Disable warning C4100, which can be issued by Visual C++ 2008 and later, due
// to unreferenced parameters on the ibnotify callback.
#pragma warning(disable:4100)
#endif

static int __stdcall MyCallback (int LocalUd,
                          unsigned int LocalIbsta,
                          unsigned int LocalIberr,
                          unsigned int LocalIbcnt,
                          void *RefData);

#define TRUE 1
#define FALSE 0
#define MAXREADINGS 10

static int  ReadingsTaken = 0;
static int  DeviceError;
static char expectedResponse = 0x50;

int __cdecl main()
{

   int ud;

   // Assign a unique identifier to the device and store it in the
   // variable ud. ibdev opens an available device and assigns it to
   // access GPIB0 with a primary address of 1, a secondary address of 0,
   // a timeout of 10 seconds, the END message enabled, and the EOS mode
   // disabled. If ud is less than zero, then print an error message
   // that the call failed and exit the program.
   ud = ibdev   (0,     // connect board
                 1,     // primary address of GPIB device
                 0,     // secondary address of GPIB device
                 T10s,  // 10 second I/O timeout
                 1,     // EOT mode turned on
                 0);    // EOS mode disabled

   if (ud < 0)
   {
      printf ("ibdev failed.\n");
      return 0;
   }

   // set up the asynchronous event notification on RQS
   // If the ERR bit is set in Ibsta, then print an error message that the
   // call failed and exit the program.
   ibnotify (ud, RQS, MyCallback, NULL);
   if (Ibsta() & ERR)
   {
      printf ("ibnotify call failed.\n");
      return 0;
   }

   // *RST requests that the Fluke 45 resets itself
   // *SRE 16 issues a request to the device to assert RQS when data is available.
   // VAL1? requests a measurement.
   ibwrt (ud, "*RST; *SRE 16; VAL1?", 20L);
   if (Ibsta() & ERR)
   {
      printf ("unable to write to device.\n");
      return 0;
   }

   while ((ReadingsTaken < MAXREADINGS) && !(DeviceError))
   {
      // Your application does useful work here. For example, it
      // might process the device readings or do any other useful work.
   }

   // disable notification
   ibnotify (ud, 0, NULL, NULL);

   // Call the ibonl function to disable the hardware and software.
   ibonl (ud, 0);
   return 1;

}

int __stdcall MyCallback (int  LocalUd,
                          unsigned int LocalIbsta,
                          unsigned int LocalIberr,
                          unsigned int LocalIbcnt,
                          void * RefData
                          )
{
   char SpollByte;
   char ReadBuffer[40];

   // If the ERR bit is set in LocalIbsta, then print an error message
   // and return.
   if (LocalIbsta & ERR)
   {
      printf ("GPIB error %d has occurred. No more callbacks.\n",
         LocalIberr);
      DeviceError = TRUE;
      return 0;
   }

   // Read the serial poll byte from the device. If the ERR bit is set
   // in LocalIbsta, then print an error message and return.
   ibrsp (LocalUd, &SpollByte);
   if (ThreadIbsta() & ERR)
   {
      printf ("ibrsp failed. No more callbacks.\n");
      DeviceError = TRUE;
      return 0;
   }

   // If the returned status byte equals the expected response, then
   // the device has valid data to send; otherwise it has a fault
   // condition to report.
   if (SpollByte != expectedResponse)
   {
      printf ("Device returned invalid response. Status byte = 0x%x\n",
            SpollByte);
      DeviceError = TRUE;
      return 0;
   }

   // Read the data from the device. If the ERR bit is set in LocalIbsta,
   // then print an error message and return.
   ibrd (LocalUd, ReadBuffer, 40L);
   if (ThreadIbsta() & ERR)
   {
      printf ("ibrd failed. No more callbacks.\n");
      DeviceError = TRUE;
      return 0;
   }

   // The string returned by ibrd is a binary string whose length is
   // specified by the byte count in Ibcnt. However, many GPIB
   // instruments return ASCII data strings and this example makes this
   // assumption. Because of this, it is possible to add a NULL
   // character to the end of the data received and use the printf()
   // function to display the ASCII data. The following code
   // illustrates that. ThreadIbcnt is used because the callback is
   // executing in a separate thread from the rest of the application.
   ReadBuffer[ThreadIbcnt()] = '\0';

   // Display the data.
   printf ("Reading : %s\n", ReadBuffer);

   ReadingsTaken += 1;

   // If all expected readings were received, return from the callback
   // without rearming the notification.
   if (ReadingsTaken >= MAXREADINGS)
   {
      return 0;
   }
   else
   {

      // Issue a request to the device to send the data and rearm
      // callback on RQS.
      ibwrt (LocalUd, "VAL1?", 5L);
      if (ThreadIbsta() & ERR)
      {
         printf ("ibwrt failed. No more callbacks.\n");
         DeviceError = TRUE;
         return 0;
      }
      else
      {
         return RQS;
      }
   }
}
