/*
// This example logs all the frames on the network and saves them in the .pcap file.
// This is used to demonstrate a frame input stream session.
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
void DisplayErrorAndExit(nxStatus_t Status, const char * Source);

//=============================================================================
// Main function
//=============================================================================
int main(void)
{
   // Declare all variables for the function
   int typedChar = 0;
   // An interface with the "/monitor" suffix would configure the input stream session to passively monitor all network traffic.
   // An interface without this suffix would configure the session as an endpoint.
   const char *pSelectedInterface = "ENET4";
   u32 mode = nxEnetLogMode_Log;
   u32 operation = nxEnetLogOperation_CreateOrReplace;
#if defined _WIN64 || defined _WIN32
   char logPath[] = "C:\\Users\\Public\\Documents\\log.pcap";
#elif defined __linux__
   char logPath[] = "/tmp/log.pcap";
#endif
   u64 frameCount = 0;
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
      DisplayErrorAndExit(status, "nxSetProperty for IntfEnetEptReceiveFilter");
   }

    // Set logging properties on the XNET session.
    status = nxSetProperty(m_SessionRef, nxPropSession_EnetLogMode,
      sizeof(u32), &mode); //Enables or disables logging.
   if (nxSuccess != status)
   {
      DisplayErrorAndExit(status, "nxSetProperty for EnetLogMode");
   }

   status = nxSetProperty(m_SessionRef, nxPropSession_EnetLogOperation,
      sizeof(u32), &operation); //Specifies logging operation.
   if (nxSuccess != status)
   {
      DisplayErrorAndExit(status, "nxSetProperty for EnetLogOperation");
   }

   status = nxSetProperty(m_SessionRef, nxPropSession_EnetLogFile,
      sizeof(logPath), &logPath); //Specifies logging file path.
   if (nxSuccess != status)
   {
      DisplayErrorAndExit(status, "nxSetProperty for EnetLogFile");
   }

   // Note: Use nxPropSession_EnetFrameFilter to specify a string to be applied as a filter for incoming frames.

   //Start logging.
   status = nxStart(m_SessionRef, 0);
   if (nxSuccess != status)
   {
      DisplayErrorAndExit(status, "nxStart");
   }
   printf("Logging all received frames. Press q to quit\n");

   do
   {
      // Get number of frames received.
      status = nxGetProperty(m_SessionRef, nxPropSession_EnetNumFramesReceived,
         sizeof(u64), &frameCount);
      if (nxSuccess != status)
      {
         DisplayErrorAndExit(status, "nxGetProperty");
      }
      printf("\rNumber of Frames Received: %llu", frameCount);

      // Wait 500 ms
      Sleep(500);

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
void DisplayErrorAndExit(nxStatus_t Status, const char * Source)
{
   char statusString[1024];
   nxStatusToString(Status, sizeof(statusString), statusString);

   printf("\n\nERROR at %s!\n%s\n", Source, statusString);
   printf("\nExecution stopped.\nPress any key to quit\n");

   nxClear(m_SessionRef);

   _getch();
   exit(1);
}