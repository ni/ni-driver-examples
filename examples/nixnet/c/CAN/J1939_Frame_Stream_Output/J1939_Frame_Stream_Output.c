/*
// This example demonstrates how to send J1939 messages using a Frame Output Stream
// session. It writes a very large J1939 frame out every 3 seconds.
// This example is designed to be run with J1939 Frame Input Stream example, ensure
// that it is running if you intend to use this example.
// For more information about this type of session, please consult the NI-XNET manual.
// This example uses hard coded frames that use the :can_j1939: in-memory database.
// To use your own database, you need to add an alias to your database file using
// the NI-XNET Database Editor and then modify the database name and frames used here.
// Please make sure that the bus is properly terminated as this example does not
// enable the on-board termination. Also ensure that the transceivers are
// externally powered when using C Series modules.
*/

#include "../../example_support.h" // Include file for Sleep, _getch, _kbhit
#include <nixnet.h>                // Include file for NI-XNET functions and constants
#include <stdlib.h>                // Include file for various functions
#include <stdio.h>                 // Include file for printf
#include <ctype.h>                 // Include file for tolower

#define PAYLOAD_SIZE_1 256

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
   int l_TypedChar = 0;
   char *l_pSelectedInterface = "CAN1";
   char *l_pSelectedCluster = "";
   u8 l_Buffer[sizeof(nxFrameFixed_t(PAYLOAD_SIZE_1))];
   u64 l_BaudRate = 250000;
   u32 l_NumberOfBytesForFrames = 0;
   nxFrameVar_t *l_pFrame = NULL;
   nxStatus_t l_Status = 0;

   char *l_pSelectedDatabase = ":can_j1939:";
   // We are using the special :can_j1939: in-memory database.
   // This enables J1939 for the session. Normally, we would need to
   // enable J1939 inside the database file.

   // J1939 variables
   u64 l_NodeName = 4;        // J1939 Node Name
   u32 l_InAddress = 4;       // Proposed Address
   u32 l_OutAddress = 0;      // Assigned Address by Claiming

   u16 l_PayloadLength = PAYLOAD_SIZE_1; // Payload Length
   // Use a very large (e.g. 256 bytes) payload (maximum is 1785 bytes).
   // Payloads over 8 bytes will transmit in multiple frames with 7 bytes each
   // with a reduced rate of 20 Hz.
   // Please make sure that the wait time in the sender loop is larger than the
   // duration of the transmission when changing the payload size.

   // Display parameters that will be used for the example.
   printf("Interface: %s\nDatabase: %s\nBaudrate: %llu\n", l_pSelectedInterface,
      l_pSelectedDatabase, l_BaudRate);

   // Create an XNET session in FrameOutStream mode
   l_Status = nxCreateSession(l_pSelectedDatabase, l_pSelectedCluster, "",
      l_pSelectedInterface, nxMode_FrameOutStream, &m_SessionRef);
   if (nxSuccess == l_Status)
   {
      printf("Session created successfully.\n");
   }
   else
   {
      DisplayErrorAndExit(l_Status, "nxCreateSession");
   }

   // Set the Baud rate for the interface
   l_Status = nxSetProperty(m_SessionRef, nxPropSession_IntfBaudRate64,
      sizeof(u64), &l_BaudRate);
   if (nxSuccess != l_Status)
   {
      DisplayErrorAndExit(l_Status, "nxSetProperty");
   }

   // Claim By Address Function, will also start the session
   l_Status = Claim_by_Address(m_SessionRef, l_NodeName, l_InAddress,
      &l_OutAddress);
   if (nxSuccess != l_Status)
   {
      DisplayErrorAndExit(l_Status, "Claim_by_Address");
   }

   // Here we set each of the frame's parameters.
   l_pFrame = (nxFrameVar_t *)l_Buffer;
   l_pFrame->Timestamp = 0;
   l_pFrame->Flags = 0;
   l_pFrame->Type = nxFrameType_J1939_Data;

   l_pFrame->Identifier = 0x18FAFF00;
   // The identifier consists of:
   // -Prio = 6  (Low priority, which is actually ignored for frames with payload > 8 bytes)
   // -EDP = 0   (Always 0 for J1939)
   // -DP = 0    (Extended Datapage)
   // -PF = 0xFA (Identifier. This one is of global destination type, since it is equal or larger than 240)
   // -PS = 0xFF (Global message group extension or destination address for destination-specific messages)
   // -SA = 0x00 (Source address, will be overwritten by XNET during transmission)
   // The first three items combine to 0x18. Please consult the XNET documentation for more information.

   // Use this helper function to set the extended J1939 payload length
   nxFrameSetPayloadLength(l_pFrame, l_PayloadLength);

   for (i = 0; i < l_PayloadLength; ++i)
   {
      l_pFrame->Payload[i] = (u8)i;
   }

   // Move the pointer to where the next frame would start to get the end pointer
   l_pFrame = nxFrameIterate(l_pFrame);
   l_NumberOfBytesForFrames = (u32)((u8 *)l_pFrame - (u8 *)l_Buffer);

   printf("\nWriting J1939 Frames with %d bytes payload. Press q to quit\n",
      l_PayloadLength);

   printf("No of J1939 Frames transmitted:\n");

   // Main loop
   do
   {
      // Write the J1939 Frame stream
      l_Status = nxWriteFrame(m_SessionRef, l_Buffer,
         l_NumberOfBytesForFrames, 10.0);
      if (nxSuccess != l_Status)
      {
         DisplayErrorAndExit(l_Status, "nxWriteFrame");
      }

      // Transmitting these large frames takes a long time because the single
      // frames transmit with a limited rate of 20 Hz by default
      Sleep(3000);
      printf("\r%d", ++j);

      if (_kbhit())
      {
         l_TypedChar = _getch();
      }
   }
   while ('q' != tolower(l_TypedChar));

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
