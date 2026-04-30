/*
// This example writes a PDU out on the bus when a keyboard character is
// pressed. This is used to demonstrate a PDU single point output session.
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
   char *l_pSelectedInterface = "FlexRay1";
   char *l_pSelectedDatabase = "NIXNET_example";
   char *l_pSelectedCluster = "FlexRay_Cluster";
   char *l_pSelectedPDUList = "FlexRayCyclicFrame1_pdu,FlexRayCyclicFrame2_pdu";
   u32 l_NumberOfBytesForPDUs = 0;
   u8 l_Buffer[sizeof(nxFrameFixed_t(PAYLOAD_SIZE_1)) + sizeof(nxFrameFixed_t(PAYLOAD_SIZE_2))];
   nxFrameVar_t *l_pFrame = NULL;
   u32 l_KeySlotId = 2;
   u32 l_Param = 0;
   nxStatus_t l_Status = 0;

   // Display parameters that will be used for the example.
   printf("Interface: %s\nDatabase: %s\n", l_pSelectedInterface, l_pSelectedDatabase);
   printf("Frames: %s\nKeyslotID: %d\n", l_pSelectedPDUList, l_KeySlotId);

   // Create an XNET session in FrameOutSinglePoint mode
   // Since we are passing PDU names, NI-XNET will open a PDU session.
   l_Status = nxCreateSession(l_pSelectedDatabase, l_pSelectedCluster,
      l_pSelectedPDUList, l_pSelectedInterface, nxMode_FrameOutSinglePoint,
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

   // Start the session
   l_Status = nxStart(m_SessionRef, nxStartStop_Normal);
   if (nxSuccess != l_Status)
   {
      DisplayErrorAndExit(l_Status, "nxStart");
   }

   // Wait for integration
   l_Status = nxWait(m_SessionRef, nxCondition_IntfCommunicating,
      0, 10.0, &l_Param);
   if (nxSuccess != l_Status)
   {
      DisplayErrorAndExit(l_Status, "nxWait");
   }

   l_pFrame = (nxFrameVar_t *)l_Buffer;

   // Set the first frame's parameters.
   // Since we are using PDUs, only the payload matters.
   // There are ways to programmatically get these parameters
   // using the objects properties.
   // FlexRayCyclicFrame1_pdu has a length of 4.
   l_pFrame->PayloadLength = 4;
   for (i = 0; i < l_pFrame->PayloadLength; ++i)
   {
      l_pFrame->Payload[i] = (u8)i;
   }

   // Iterate the pointer to the next frame
   l_pFrame = nxFrameIterate(l_pFrame);

   // Set the second frame's parameters.
   // FlexRayCyclicFrame2_pdu has a length of 2
   l_pFrame->PayloadLength = 2;
   for (i = 0; i < l_pFrame->PayloadLength; ++i)
   {
      l_pFrame->Payload[i] = (u8)i;
   }

   // Get the actual size of the buffer used
   l_pFrame = nxFrameIterate(l_pFrame);
   l_NumberOfBytesForPDUs = (u32)((u8 *)l_pFrame - (u8 *)l_Buffer);

   printf("Press any key to write the frame value. (q to quit)\n");

   // Main loop
   while ('q' != tolower(_getch()))
   {
      // Update the frame data
      l_Status = nxWriteFrame(m_SessionRef, l_Buffer,
         l_NumberOfBytesForPDUs, 10.0);
      if (nxSuccess != l_Status)
      {
         DisplayErrorAndExit(l_Status, "nxWriteFrame");
      }
      else
      {
         printf("Frame written.\n");
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
