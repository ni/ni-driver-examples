/*
// This example reads all the frames on the network and displays them in table
// format. This is used to demonstrate a frame input stream session.
// For more information about this type of session, please consult the NI-XNET
// manual.
// Ensure that you have connected the selected interface to another Ethernet interface
// that is transmitting data. This example can be used with any Ethernet write or output example.
// Configure the Port Mode to Tap if you would like to monitor an existing network
// connection. This requires that you connect both ports of the Tap pair. Use NI MAX to
// change the Port Mode setting.
// Check that the PHY State of the selected interface is compatible with the interface
// it is connected to. For example, if the connected interface is in "Master" mode, the
// interface will need to be configured to "Slave". Use NI MAX to change the PHY State setting.
// If the session is in endpoint mode, set the VID and priority controls to configure the Receive Filter.
*/

#include "../../example_support.h"        // Include file for Sleep, _getch, _kbhit, and PrintTimestamp
#include "../EthernetExamplesUtilities.h" // Include struct for ethernet header plus other libraries
#include <stdlib.h>                       // Include file for various functions
#include <ctype.h>                        // Include file for tolower
#include <string.h>                       // memcpy

//=============================================================================
// Static global variables
//=============================================================================
static nxSessionRef_t m_SessionRef = 0;

//=============================================================================
// Global functions declarations
//=============================================================================
void DisplayErrorAndExit(nxStatus_t Status, char *Source);
int PrintTimestamp1ns(nxTimestamp1ns_t Time);

//=============================================================================
// Main function
//=============================================================================
int main(void)
{
   // Declare all variables for the function
   unsigned int i = 0;
   int typedChar = 0;
   // An interface with the "/monitor" suffix would configure the input stream session to passively monitor all network traffic.
   // An interface without this suffix would configure the session as an endpoint.
   const char* pSelectedInterface = "ENET4";
   // Allocate buffer to accommodate 250 frames of maximum size
   u8 buffer[250 * (sizeof(nxFrameEnet_t) + MAX_ENET_FRAME_SIZE + FCS_SIZE)];
   nxFrameEnet_t* pFrame = NULL;
   u32 numBytes = 0;
   nxStatus_t status = 0;

   // Receive filter only applies for endpoint mode.
   nxEptRxFilter_Element_t rxFilter;
   u32 flag = kEnableVidAndPriority;
   u16 vid = kVlanId;
   u8 priority = kPcp;
   nxMACAddress_t mac = "AA:BB:CC:DD:EE:FF"; // MAC Address filter is not enabled. Use random string.

   printf(CHOOSE_MONITOR_OR_ENDPOINT_TEXT);
   if('m' == tolower(getchar()))
   {
      pSelectedInterface = "ENET4/monitor";
   }

   // Display parameters that will be used for the example.
   printf("Interface: %s\n", pSelectedInterface);

   // Create an XNET session in FrameInStream mode
   status = nxCreateSession(NULL, NULL, NULL, pSelectedInterface,
      nxMode_FrameInStream, &m_SessionRef);

   if (nxSuccess == status)
   {
      printf("Session created successfully.\n");
   }
   else
   {
      DisplayErrorAndExit(status, "nxCreateSession");
   }

   // Set Receive Filter (Only applies for endpoint mode)
   rxFilter.UseFlags = flag;
   rxFilter.VID = vid;
   rxFilter.Priority = priority;
   memcpy(rxFilter.DestinationMAC, mac, sizeof(nxMACAddress_t));

   status = nxSetProperty(m_SessionRef, nxPropSession_IntfEnetEptReceiveFilter,
      sizeof(nxEptRxFilter_Element_t), &rxFilter);
   if (nxSuccess != status)
   {
      DisplayErrorAndExit(status, "nxSetProperty");
   }

   // Note: Use nxPropSession_EnetFrameFilter to specify a string to be applied as a filter for incoming frames.

   printf("Reading all received frames. Press q to quit.\n");
   printf("%-23s %-23s Data\n", "Local Timestamp", "Network Timestamp");
   do
   {
      // Read the received frames into the buffer
      status = nxReadFrame(m_SessionRef, buffer, sizeof(buffer), nxTimeout_None, &numBytes);
      if (nxSuccess == status)
      {
         pFrame = (nxFrameEnet_t*)buffer;

         // pFrame iterates to each frame. When it increments
         // past the number of bytes returned, we reached the end.
         while ((u8*)pFrame < (u8*)buffer + numBytes)
         {
            PrintTimestamp1ns(pFrame->DeviceTimestamp); // Print Local Timestamp.
            PrintTimestamp1ns(pFrame->NetworkTimestamp); // Print Network Timestmap.

            // The length of Frame Data (IEEE Std 802.3 frame data) can be computed
            // by subtracting 28 from the Length field, to account for the fields
            // that are specific to NI (and the FCS).
            for (i = 0; i < pFrame->Length - 28UL; ++i)
            {
               printf("%02X ", pFrame->FrameData[i]);
            }
            printf("\n\n");

            // Go to next variable-payload frame.
            pFrame = nxFrameIterateEthernetRead(pFrame);
         }
      }
      else
      {
         DisplayErrorAndExit(status, "nxReadFrame");
      }

      // Wait 1 ms
      Sleep(1);

      if (_kbhit())
      {
         typedChar = _getch();
      }
   } while ('q' != tolower(typedChar));

   printf("\nFrame capture stopped.\n");

   // Clear the XNET session
   status = nxClear(m_SessionRef);
   if (nxSuccess == status)
   {
      printf("\nSession cleared successfully!\n");
   }
   else
   {
      DisplayErrorAndExit(status, "nxClear");
   }
   return 0;
}

//=============================================================================
// Display Error Function
//=============================================================================
void DisplayErrorAndExit(nxStatus_t Status, char *Source)
{
   char statusString[1024];
   nxStatusToString(Status, sizeof(statusString), statusString);

   printf("\n\nERROR at %s!\n%s\n", Source, statusString);
   printf("\nExecution stopped.\nPress any key to quit\n");

   nxClear(m_SessionRef);

   getchar();
   exit(1);
}

//=============================================================================
// Prints a timestamp to stdout
//=============================================================================
int PrintTimestamp1ns(nxTimestamp1ns_t Time)
{
   // Converts nxTimestamp1ns_t (elapsed since 1 January 1970 00:00:00
   // International Atomic Time (TAI)) to nxTimestamp100ns_t (elapsed since 1
   // January 1601 00:00:00 Coordinated Universal Time (UTC)).
   nxStatus_t status = 0;
   nxTimestamp100ns_t l_100nsTime = 0;
   status = nxConvertTimestamp1nsTo100ns(Time, &l_100nsTime);
   if (nxSuccess != status)
   {
      DisplayErrorAndExit(status, "nxConvertTimestamp1nsTo100ns");
   }
   return PrintTimestamp(&l_100nsTime) + printf(" ");
}