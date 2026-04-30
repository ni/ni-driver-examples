/*
// This example demonstrates how to receive J1939 messages using a Frame Input Stream
// session. It displays the received J1939 frames every 100 ms.
// This example can be run together with any J1939 Output example.
// For more information about this type of session, please consult the NI-XNET manual.
// This example uses the special :can_j1939: in-memory database.
// Please make sure that the bus is properly terminated as this example does not
// enable the on-board termination. Also ensure that the transceivers are
// externally powered when using C Series modules.
*/

#include "../../example_support.h" // Include file for Sleep, _getch, _kbhit, and PrintTimestamp
#include <nixnet.h>                // Include file for NI-XNET functions and constants
#include <stdlib.h>                // Include file for various functions
#include <stdio.h>                 // Include file for printf
#include <ctype.h>                 // Include file for tolower

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
   char *l_pSelectedInterface = "CAN2";
   char *l_pSelectedCluster = "";
   u64 l_BaudRate = 250000;
   u32 l_NumBytes = 0;
   u32 l_NumBytesPerFrame = 0;
   u8 l_Buffer[10000]; // Use a large buffer for J1939 stream input
   nxFrameVar_t *l_pFrame = NULL;
   nxStatus_t l_Status = 0;

   char *l_pSelectedDatabase = ":can_j1939:";
   // We are using the special :can_j1939: in-memory database.
   // This enables J1939 for the session. Normally, we would need to
   // enable J1939 inside the database file.

   // J1939 variables
   u64 l_NodeName = 3;       // J1939 Node Name
   u32 l_InAddress = 3;      // Proposed Address
   u32 l_OutAddress = 0;     // Assigned Address by Claiming

   u16 l_PayloadLength = 0;  // Payload Length of received frame

   // Display parameters that will be used for the example.
   printf("Interface: %s\nDatabase: %s\nBaudrate: %llu\n", l_pSelectedInterface,
      l_pSelectedDatabase, l_BaudRate);

   // Create an XNET session in FrameInStream mode
   l_Status = nxCreateSession(l_pSelectedDatabase, l_pSelectedCluster,
      "", l_pSelectedInterface, nxMode_FrameInStream, &m_SessionRef);
   if (nxSuccess == l_Status)
   {
      printf("Session created successfully.\n");
   }
   else
   {
      DisplayErrorAndExit(l_Status, "nxCreateSession");
   }

   printf("Reading J1939 Frames. Press q to quit\n");

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

   // Main loop
   do
   {
      // Read the frame data
      l_Status = nxReadFrame(m_SessionRef, l_Buffer, sizeof(l_Buffer),
         nxTimeout_None, &l_NumBytes);
      if (nxSuccess != l_Status)
      {
         DisplayErrorAndExit(l_Status, "nxReadFrame");
      }

      l_pFrame = (nxFrameVar_t *)l_Buffer;

      // l_pFrame iterates to each frame. When it increments
      // past the number of bytes returned, we reached the end.
      while ((u8 *)l_pFrame < (u8 *)l_Buffer + l_NumBytes)
      {
         printf("\nTimestamp\t\tID\t\tData\n");
         // Print timestamp, ID and payload
         PrintTimestamp(&l_pFrame->Timestamp);
         printf("\t%x    \t", l_pFrame->Identifier);
         l_PayloadLength = (u16)nxFrameGetPayloadLength(l_pFrame);

         if (l_PayloadLength > 8)
         {
            // There are up to 7 bytes of data in a J1939 multi frame
            l_NumBytesPerFrame = 7;
         }
         else
         {
            l_NumBytesPerFrame = 8;
         }

         j = 0;
         for (i = 0; i < l_PayloadLength; ++i)
         {
            if (j++ >= l_NumBytesPerFrame)
            {
               printf("\n\t\t\t\t\t"); // Break line after each frame
               j = 1;
            }
            printf("%02X ", l_pFrame->Payload[i]);
         }

         printf("\n\nReading the latest J1939 Frames... press q to quit\n\n");

         // Go to next variable-payload frame.
         l_pFrame = nxFrameIterate(l_pFrame);
      }

      Sleep(100);
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

