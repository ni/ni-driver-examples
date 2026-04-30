/*
// This example writes a frame out on the bus when a keyboard character is
// pressed. This is used to demonstrate the frame single point output session.
// For more information about this type of session, please consult the NI-XNET
// manual.
// This example uses hardcoded signal names that use the NIXNET_example database.
// To use your own database, you need to add an alias to your database file using
// the NI-XNET Database Editor and then modify the database name and frames used here.
// Please make sure that the bus is properly terminated as this example does not
// enable the on-board termination.
*/

#include "../../example_support.h" // Include file for Sleep, _getch
#include <nixnet.h>                // Include file for NI-XNET functions and constants
#include <stdlib.h>                // Include file for various functions
#include <stdio.h>                 // Include file for printf
#include <ctype.h>                 // Include file for tolower

#define PAYLOAD_SIZE_1 8
#define PAYLOAD_SIZE_2 8

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
   char *l_pSelectedInterface = "FlexRay1";
   char *l_pSelectedDatabase = "NIXNET_example";
   char *l_pSelectedCluster = "FlexRay_Cluster";
   char *l_pSelectedFrameList = "FlexRayEventFrame1,FlexRayEventFrame2";
   u32 l_NumberOfBytesForFrames = 0;
   u8 l_Buffer[sizeof(nxFrameFixed_t(PAYLOAD_SIZE_1)) + sizeof(nxFrameFixed_t(PAYLOAD_SIZE_2))];
   nxFrameVar_t *l_pFrame = NULL;
   u32 l_KeySlotId = 2;
   nxStatus_t l_Status = 0;

   // Display parameters that will be used for the example.
   printf("Interface: %s\nDatabase: %s\n", l_pSelectedInterface, l_pSelectedDatabase);
   printf("Frames: %s\nKeyslotID: %d\n", l_pSelectedFrameList, l_KeySlotId);

   // Create an XNET session in FrameOutSinglePoint mode
   l_Status = nxCreateSession(l_pSelectedDatabase, l_pSelectedCluster,
      l_pSelectedFrameList, l_pSelectedInterface, nxMode_FrameOutSinglePoint,
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

   l_pFrame = (nxFrameVar_t *)l_Buffer;

   // Set the first frame's parameters (skipping the payload for now).
   // There are ways to programmatically get these parameters
   // using the objects properties.
   l_pFrame->Timestamp = 0;
   l_pFrame->Flags = 0;
   l_pFrame->Identifier = 5;
   l_pFrame->Type = nxFrameType_FlexRay_Data;
   l_pFrame->PayloadLength = PAYLOAD_SIZE_1;

   // Iterate the pointer to the next frame
   l_pFrame = nxFrameIterate(l_pFrame);

   // Set the second frame's parameters (skipping the payload for now).
   l_pFrame->Timestamp = 0;
   l_pFrame->Flags = 0;
   l_pFrame->Identifier = 6;
   l_pFrame->Type = nxFrameType_FlexRay_Data;
   l_pFrame->PayloadLength = PAYLOAD_SIZE_2;

   // Get the actual size of the buffer used
   l_pFrame = nxFrameIterate(l_pFrame);
   l_NumberOfBytesForFrames = (u32)((u8 *)l_pFrame - (u8 *)l_Buffer);

   printf("Press any key to write frame values. (q to quit)\n");

   // Main loop
   while ('q' != tolower(_getch()))
   {
      // Update the payload of the first frame
      l_pFrame = (nxFrameVar_t *)l_Buffer;
      for (j = 0; j < l_pFrame->PayloadLength; ++j)
      {
         l_pFrame->Payload[j] = (u8)(j + i);
      }

      // Update the payload of the second frame
      l_pFrame = nxFrameIterate(l_pFrame);
      for (j = 0; j < l_pFrame->PayloadLength; ++j)
      {
         l_pFrame->Payload[j] = (u8)(j + i);
      }

      // Update the frame data
      l_Status = nxWriteFrame(m_SessionRef, l_Buffer,
         l_NumberOfBytesForFrames, 10.0);
      if (nxSuccess == l_Status)
      {
         printf("Updated the payloads starting with %02X.\n", i);
      }
      else
      {
         DisplayErrorAndExit(l_Status, "nxWriteFrame");
      }

      if (++i > 0xFF)
      {
         i = 0;
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
