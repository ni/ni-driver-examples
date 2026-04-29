/*
// This example reads all the frames on the bus and displays them in table
// format. This is used to demonstrate a frame input stream session.
// For more information about this type of session, please consult the NI-XNET
// manual.
// This example uses the NIXNET_example database.
// To use your own database, you need to add an alias to your
// database file using the NI-XNET Database Editor and then modify the database
// name used here.
// Please make sure that the bus is properly terminated as this example does not
// enable the on-board termination.
*/

#include "../../example_support.h" // Include file for Sleep, _getch, _kbhit, PrintTimestamp
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

//=============================================================================
// Main function
//=============================================================================
int main(void)
{
   // Declare all variables for the function
   unsigned int i = 0;
   unsigned int j = 0;
   int l_TypedChar = 0;
   char *l_pSelectedInterface = "FlexRay2";
   char *l_pSelectedDatabase = "NIXNET_example";
   char *l_pSelectedCluster = "FlexRay_Cluster";
   char *l_pSelectedFrameList = NULL;
   char l_FrameChannelA = '-';
   char l_FrameChannelB = '-';
   u8 l_Buffer[10000]; // Use a large buffer for stream input
   nxFrameVar_t *l_pFrame = NULL;
   u32 l_NumBytes = 0;
   u32 l_KeySlotId = 1;
   nxStatus_t l_Status = 0;

   // Display parameters that will be used for the example.
   printf("Interface: %s\nDatabase: %s\n", l_pSelectedInterface, l_pSelectedDatabase);
   printf("KeyslotID: %d\n", l_KeySlotId);

   // Create an XNET session in FrameInStream mode
   l_Status = nxCreateSession(l_pSelectedDatabase, l_pSelectedCluster,
      l_pSelectedFrameList, l_pSelectedInterface, nxMode_FrameInStream, &m_SessionRef);
   if (nxSuccess == l_Status)
   {
      printf("Session created successfully.\n");
   }
   else
   {
      DisplayErrorAndExit(l_Status, "nxCreateSession");
   }

   // Set the Key Slot
   l_Status = nxSetProperty(m_SessionRef, nxPropSession_IntfFlexRayKeySlotID,
      sizeof(l_KeySlotId), &l_KeySlotId);
   if (nxSuccess != l_Status)
   {
      DisplayErrorAndExit(l_Status, "nxSetProperty");
   }

   printf("Displaying all frames received. Press q to quit\n");
   printf("Timestamp\t\tID\tChannel\tData\n");

   // Main loop
   do
   {
      // Read the received frames
      l_Status = nxReadFrame(m_SessionRef, l_Buffer, sizeof(l_Buffer),
         nxTimeout_None, &l_NumBytes);
      if (nxSuccess == l_Status)
      {
         l_pFrame = (nxFrameVar_t *)l_Buffer;

         // l_pFrame iterates to each frame. When it increments
         // past the number of bytes returned, we reached the end.
         while ((u8 *)l_pFrame < l_NumBytes + (u8 *)l_Buffer)
         {
            // Print timestamp, ID and payload
            PrintTimestamp(&l_pFrame->Timestamp);

            if (l_pFrame->Flags & nxFrameFlags_FlexRay_ChA)
            {
               l_FrameChannelA = 'A';
            }
            else
            {
               l_FrameChannelA = '-';
            }

            if (l_pFrame->Flags & nxFrameFlags_FlexRay_ChB)
            {
               l_FrameChannelB = 'B';
            }
            else
            {
               l_FrameChannelB = '-';
            }

            // Print Frame ID
            printf("\t%d\t%c%c\t", l_pFrame->Identifier, l_FrameChannelA, l_FrameChannelB);

            j = 0;
            for (i = 0; i < l_pFrame->PayloadLength; ++i)
            {
               if (j++ >= 8)
               {
                  printf("\n\t\t\t\t\t"); // Break line after 8 bytes
                  j = 1;
               }
               printf("%02X ", l_pFrame->Payload[i]);
            }
            printf("\n");

            // Go to next variable-payload frame
            l_pFrame = nxFrameIterate(l_pFrame);
         }
         Sleep(10);
      }
      else
      {
         DisplayErrorAndExit(l_Status, "nxReadFrame");
      }

      if (_kbhit())
      {
         l_TypedChar = _getch();
      }
   }
   while ('q' != tolower(l_TypedChar));

   printf("Data aquisition stopped.\n");

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

