/*
// This example sends a frame out every 100 ms. This is used to demonstrate a frame
// stream output session. For more information about this type of session, please
// consult the NI-XNET manual.
// This example uses hardcoded frames that use the :memory: database.
// To use your own database, you need to add an alias to your
// database file using the NI-XNET Database Editor and then modify the database
// name and frames used here.
// Please make sure that the bus is properly terminated as this example does not
// enable the on-board termination. Also ensure that the transceivers are
// externally powered when using C Series modules.
*/

#include "../../example_support.h" // Include file for Sleep, _getch, _kbhit
#include <nixnet.h>                // Include file for NI-XNET functions and constants
#include <stdlib.h>                // Include file for various functions
#include <stdio.h>                 // Include file for printf
#include <ctype.h>                 // Include file for tolower

#define NUM_FRAMES 2

//=============================================================================
// Static global variables
//=============================================================================
static nxSessionRef_t g_SessionRef = 0;

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
   char *l_pSelectedInterface = "CAN1";
   char *l_pSelectedDatabase = ":memory:";
   char *l_pSelectedCluster = "";
   char *l_pSelectedFrameList = "";
   u64 l_BaudRate = 125000;
   u32 l_NumberOfBytesForFrames = 0;
   u8 l_Buffer[NUM_FRAMES * sizeof(nxFrameCAN_t)];
   nxFrameVar_t *l_pFrame = NULL;
   nxStatus_t l_Status = 0;

   // Display parameters that will be used for the example.
   printf("Interface: %s\nDatabase: %s\nBaudrate: %llu\n", l_pSelectedInterface,
      l_pSelectedDatabase, l_BaudRate);

   // Create an XNET session in FrameOutStream mode
   l_Status = nxCreateSession(l_pSelectedDatabase, l_pSelectedCluster,
      l_pSelectedFrameList, l_pSelectedInterface, nxMode_FrameOutStream,
      &g_SessionRef);
   if (nxSuccess == l_Status)
   {
      printf("Session created successfully.\n");
   }
   else
   {
      DisplayErrorAndExit(l_Status, "nxCreateSession");
   }

   // We are not using a predefined database, so we need to set the Baud Rate
   l_Status = nxSetProperty(g_SessionRef, nxPropSession_IntfBaudRate64,
      sizeof(u64), &l_BaudRate);
   if (nxSuccess == l_Status)
   {
      printf("Properties set successfully.\n");
   }
   else
   {
      DisplayErrorAndExit(l_Status, "nxSetProperty");
   }

   // Get a pointer to the first frame's memory
   l_pFrame = (nxFrameVar_t *)l_Buffer;

   // Set the first frame's parameters (skipping the payload for now).
   // There are ways to programmatically get these parameters
   // using the objects properties. Here we hardcode them.
   l_pFrame->Timestamp = 0;
   l_pFrame->Flags = 0;
   l_pFrame->Identifier = 4;
   l_pFrame->Type = nxFrameType_CAN_Data;
   l_pFrame->PayloadLength = 8;

   // Iterate the pointer to the next frame
   l_pFrame = nxFrameIterate(l_pFrame);

   // Set the second frame's parameters (skipping the payload for now).
   l_pFrame->Timestamp = 0;
   l_pFrame->Flags = 0;
   l_pFrame->Identifier = 5;
   l_pFrame->Type = nxFrameType_CAN_Data;
   l_pFrame->PayloadLength = 2;

   // Get the actual size of the buffer used
   l_pFrame = nxFrameIterate(l_pFrame);
   l_NumberOfBytesForFrames = (u32)((u8 *)l_pFrame - (u8 *)l_Buffer);

   printf("Press q to quit.\n");

   // Main loop
   i = 0;
   do
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

      // Write the frame buffer
      l_Status = nxWriteFrame(g_SessionRef, l_Buffer,
         l_NumberOfBytesForFrames, 10.0);
      if (nxSuccess != l_Status)
      {
         DisplayErrorAndExit(l_Status, "nxWriteFrame");
      }
      printf("\rSent frames with payloads starting with %02X.", i);

      if (++i > 0xFF)
      {
         i = 0;
      }

      Sleep(100); // Wait 100 ms

      if (_kbhit())
      {
         l_TypedChar = _getch();
      }
   }
   while ('q' != tolower(l_TypedChar));

   // Clear the XNET session
   l_Status = nxClear(g_SessionRef);
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

   nxClear(g_SessionRef);

   _getch();
   exit(1);
}
