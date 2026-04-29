/*
// This example reads the latest PDU when a keyboard character is pressed.
// This is used to demonstrate a PDU single point input session.
// For more information about this type of session, please consult the NI-XNET
// manual.
// This example uses hardcoded PDU names that use the NIXNET_example database.
// To use your own database, you need to add an alias to your database file using
// the NI-XNET Database Editor and then modify the database name and PDUs used here.
// Please make sure that the bus is properly terminated as this example does not
// enable the on-board termination.
*/

#include "../../example_support.h" // Include file for Sleep, _getch
#include <nixnet.h>                // Include file for NI-XNET functions and constants
#include <stdlib.h>                // Include file for various functions
#include <stdio.h>                 // Include file for printf
#include <ctype.h>                 // Include file for tolower

#define PAYLOAD_SIZE_1 4
#define PAYLOAD_SIZE_2 2

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
   char *l_pSelectedInterface = "FlexRay2";
   char *l_pSelectedDatabase = "NIXNET_example";
   char *l_pSelectedCluster = "FlexRay_Cluster";
   char *l_pSelectedPDUList = "FlexRayCyclicFrame1_pdu,FlexRayCyclicFrame2_pdu";
   u8 l_Buffer[sizeof(nxFrameFixed_t(PAYLOAD_SIZE_1)) + sizeof(nxFrameFixed_t(PAYLOAD_SIZE_2))];
   nxFrameVar_t *l_pFrame = NULL;
   u32 l_NumBytes = 0;
   u32 l_KeySlotId = 1;
   nxStatus_t l_Status = 0;

   // Display parameters that will be used for the example.
   printf("Interface: %s\nDatabase: %s\n", l_pSelectedInterface, l_pSelectedDatabase);
   printf("Frames: %s\nKeyslotID: %d\n", l_pSelectedPDUList, l_KeySlotId);

   // Create an XNET session in FrameInSinglePoint mode
   // Since we are passing PDU names, NI-XNET will open a PDU session.
   l_Status = nxCreateSession(l_pSelectedDatabase, l_pSelectedCluster,
      l_pSelectedPDUList, l_pSelectedInterface, nxMode_FrameInSinglePoint,
      &m_SessionRef);
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

   printf("Press any key to read latest signal value. (q to quit)\n");

   // Main loop
   while ('q' != tolower(_getch()))
   {
      // Read the latest frame data. The first read will also auto-start the
      // session and normally return the default payload values.
      // Use the nxStart function to start the session before the initial read.
      l_Status = nxReadFrame(m_SessionRef, l_Buffer, sizeof(l_Buffer),
         nxTimeout_None, &l_NumBytes);
      if (nxSuccess == l_Status)
      {
         l_pFrame = (nxFrameVar_t *)l_Buffer;
         // l_pFrame iterates to each frame. When it increments
         // past the number of bytes returned, we reached the end.
         while ((u8 *)l_pFrame < l_NumBytes + (u8 *)l_Buffer)
         {
            // Print Frame ID and payload
            printf("Frame ID: %d, ", l_pFrame->Identifier);
            printf("Payload Bytes: ");
            for (i = 0; i < l_pFrame->PayloadLength; ++i)
            {
               printf("%02X ", l_pFrame->Payload[i]);
            }
            printf("\n");

            // Go to next variable-payload frame.
            l_pFrame = nxFrameIterate(l_pFrame);
         }
      }
      else
      {
         DisplayErrorAndExit(l_Status, "nxReadFrame");
      }
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
