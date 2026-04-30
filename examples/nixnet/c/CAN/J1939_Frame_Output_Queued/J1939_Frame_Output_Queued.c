/*
// This example demonstrates the transmission of large J1939 payloads with a Frame Output
// Queued session. It writes a J1939 frame out when any key (except 'q') is pressed.
// This example is designed to be run with J1939 Frame Input Queued example, ensure
// that it is running if you intend to use this example.
// For more information about this type of session, please consult the NI-XNET manual.
// This example uses hard coded frame names that use the NIXNET_example database.
// To use your own database, you need to add an alias to your database file using
// the NI-XNET Database Editor and then modify the database name and signals used here.
// Please make sure that the bus is properly terminated as this example does not
// enable the on-board termination. Also ensure that the transceivers are
// externally powered when using C Series modules.
*/

#include "../../example_support.h" // Include file for Sleep, _getch
#include <nixnet.h>                // Include file for NI-XNET functions and constants
#include <stdlib.h>                // Include file for various functions
#include <stdio.h>                 // Include file for printf
#include <ctype.h>                 // Include file for tolower

#define PAYLOAD_SIZE_1 16

//=============================================================================
// Static global variables
//=============================================================================
static nxSessionRef_t m_SessionRef = 0;

//=============================================================================
// Global functions declarations
//=============================================================================
void DisplayErrorAndExit(nxStatus_t Status, char *Source);
nxStatus_t Claim_by_Address(nxSessionRef_t SessionRef, u64 NodeName,
   u32 InNodeAddr, u32 *OutNodeAddr);

//=============================================================================
// Main function
//=============================================================================
int main(void)
{
   // Declare all variables for the function
   unsigned int i = 0;
   unsigned int j = 0;
   char *l_pSelectedInterface = "CAN1";
   char *l_pSelectedDatabase = "NIXNET_example";
   char *l_pSelectedCluster = "J1939_Over_CAN";
   char *l_pSelectedFrame = "J1939_Transport_BAM";
   u32 l_NumberOfBytesForFrames = 0;
   u8 l_Buffer[sizeof(nxFrameFixed_t(PAYLOAD_SIZE_1))];
   nxFrameVar_t *l_pFrame = NULL;
   nxStatus_t l_Status = 0;

   // The selected frame, J1939_Transport_BAM, is defined
   // to be a global frame in the database.

   // J1939 variables
   u64 l_NodeName = 2;         // J1939 Node Name
   u32 l_InAddress = 2;        // Proposed Address
   u32 l_OutAddress = 0;       // Assigned Address by Claiming

   u16 l_PayloadLength = PAYLOAD_SIZE_1;   // Payload Length
   // Note that the payload is larger than the usual CAN frame (non-FD) allows.
   // A payload of 16 bytes will transmit in 3 frames (each has 7 bytes max.)

   // Display parameters that will be used for the example.
   printf("Interface: %s\nDatabase: %s\nFrame: %s\n", l_pSelectedInterface,
      l_pSelectedDatabase, l_pSelectedFrame);

   // Create an XNET session in FrameOutQueued mode
   l_Status = nxCreateSession(l_pSelectedDatabase, l_pSelectedCluster,
      l_pSelectedFrame, l_pSelectedInterface, nxMode_FrameOutQueued,
      &m_SessionRef);
   if (nxSuccess == l_Status)
   {
      printf("Session created successfully.\n");
   }
   else
   {
      DisplayErrorAndExit(l_Status, "nxCreateSession");
   }

   // Claim By Address Function, will also start the session
   l_Status = Claim_by_Address(m_SessionRef, l_NodeName, l_InAddress,
      &l_OutAddress);
   if (nxSuccess != l_Status)
   {
      DisplayErrorAndExit(l_Status, "Claim_by_Address");
   }

   // Here we set each of the frames parameters.
   // There are ways to programmatically get these parameters using
   // the object properties.
   l_pFrame = (nxFrameVar_t *)l_Buffer;
   l_pFrame->Type = nxFrameType_J1939_Data;

   // Use this helper function to set the extended J1939 payload length
   nxFrameSetPayloadLength(l_pFrame, l_PayloadLength);

   for (i = 0; i < l_PayloadLength; ++i)
   {
      l_pFrame->Payload[i] = (u8)i;
   }

   // Get the actual size of the buffer used
   l_pFrame = nxFrameIterate(l_pFrame);
   l_NumberOfBytesForFrames = (u32)((u8 *)l_pFrame - (u8 *)l_Buffer);

   printf("\nPress any key to write one J1939 Queued Frame. (q to quit)\n");
   printf("No. of J1939 Frames transmitted:\n");

   // Main loop
   while ('q' != tolower(_getch()))
   {
      // Write the J1939 Frame Queued
      l_Status = nxWriteFrame(m_SessionRef, l_Buffer,
         l_NumberOfBytesForFrames, 10.0);
      if (nxSuccess != l_Status)
      {
         DisplayErrorAndExit(l_Status, "nxWriteFrame");
      }

      printf("\r%d", ++j);
   }

   // Clear the XNET session
   l_Status = nxClear(m_SessionRef);
   if (nxSuccess == l_Status)
   {
      printf("\nSession cleared successfully!\n");
   }
   else
   {
      DisplayErrorAndExit(l_Status, "nxClear");
   }

   return 0;
}

//=============================================================================
// Display Error Function
//=============================================================================
void DisplayErrorAndExit(nxStatus_t Status, char *Source)
{
   char l_StatusString[1024];
   nxStatusToString(Status, sizeof(l_StatusString), l_StatusString);

   printf("\n\nERROR at %s!\n%s\n", Source, l_StatusString);
   printf("\nExecution stopped.\nPress any key to quit\n");

   nxClear(m_SessionRef);

   _getch();
   exit(1);
}

//=============================================================================
//
//                   J1939 Claim By Address Function
//
//   This function claims an address on the network by setting the J1939/Node
//   Name and J1939/Node Address properties. After setting the Node Address
//   (to a value less than 254), XNET sends an address claimed message and
//   waits 300 ms for the response from the network. If no other node is
//   using this address, there is no response to the message; after the
//   timeout, the address is granted to the session and the session can
//   transmit frames on the network. During the claiming procedure, the node
//   address property returns the null address (254), so we will poll
//   this address until it gets a valid value.
//
//=============================================================================
nxStatus_t Claim_by_Address(nxSessionRef_t SessionRef, u64 NodeName, u32 InNodeAddr, u32 *OutNodeAddr)
{
   nxStatus_t l_Status = 0;
   u16 l_Count = 0;

   // Setting the J1939 Node Name
   l_Status = nxSetProperty(SessionRef, nxPropSession_J1939Name, sizeof(NodeName), &NodeName);
   if (nxSuccess != l_Status)
   {
      printf("J1939 Node Name Setting Failed\n");
      DisplayErrorAndExit(l_Status, "nxSetProperty");
   }

   // Setting the J1939 Node Address
   l_Status = nxSetProperty(SessionRef, nxPropSession_J1939Address, sizeof(InNodeAddr), &InNodeAddr);
   if (nxSuccess != l_Status)
   {
      printf("J1939 Node Address Setting Failed\n");
      DisplayErrorAndExit(l_Status, "nxSetProperty");
   }

   // Break loop when the requested time is elapsed, a time out error happens
   // or a valid address (not 254 or 255) has been assigned
   do
   {
      Sleep(5);
      // Poll to check if the J1939 node address has been set by the driver
      l_Status = nxGetProperty(SessionRef, nxPropSession_J1939Address, sizeof(*OutNodeAddr), OutNodeAddr);
      if (nxSuccess != l_Status)
      {
         printf("J1939 Node Address getting has failed\n");
         DisplayErrorAndExit(l_Status, "nxGetProperty");
      }
   }
   while (!(*OutNodeAddr < 254) && (++l_Count <= 300) && (l_Status == nxSuccess));

   // Reporting of Missing Address Error
   if (!(*OutNodeAddr < 254))
   {
      l_Status = nxErrJ1939MissingAddress;
   }

   return l_Status;
}
