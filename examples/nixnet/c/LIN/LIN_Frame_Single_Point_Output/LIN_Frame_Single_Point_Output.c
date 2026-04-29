/*
// This example writes a frame out on the bus when a keyboard character is
// pressed. This is used to demonstrate a frame single point output session.
// For more information about this type of session, please consult the NI-XNET
// manual.
// This example uses hardcoded frame names that use the NIXNET_exampleLDF database.
// To use your own database, you need to add an alias to your database file using
// the NI-XNET Database Editor and then modify the database name and frames used here.
// Please make sure that the bus is properly terminated as this example does not
// enable the on-board termination. Also ensure that the transceivers are
// externally powered when using C Series modules.
*/

#include "../../example_support.h" // Include file for Sleep, _getch
#include <nixnet.h>                // Include file for NI-XNET functions and constants
#include <stdlib.h>                // Include file for various functions
#include <stdio.h>                 // Include file for printf
#include <ctype.h>                 // Include file for tolower

#define NUM_FRAMES 2

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
   char *l_pSelectedInterface = "LIN1";
   char *l_pSelectedDatabase = "NIXNET_exampleLDF";
   char *l_pSelectedCluster = "Cluster";
   char *l_pSelectedFrameList = "MasterFrame1,MasterFrame2";
   u8 l_Buffer[NUM_FRAMES * sizeof(nxFrameLIN_t)];
   u32 l_NumberOfBytesForFrames = 0;
   nxFrameVar_t *l_pFrame = NULL;
   nxStatus_t l_Status = 0;

   // Change this to set the interface to slave mode
   u32 l_IsMaster  = 1;

   // The schedule is identified by its index. The index is mapped to
   // the schedules in the database. Index 0 is the first schedule
   // displayed in the Database Editor.
   u32 l_ScheduleIndex = 0;

   // Display parameters that will be used for the example.
   printf("Interface: %s\nDatabase: %s\nFrame List: %s\n", l_pSelectedInterface,
      l_pSelectedDatabase, l_pSelectedFrameList);

   if (0 != l_IsMaster)
   {
      printf("Master?: Yes\n");
   }
   else
   {
      printf("Master?: No\n");
   }

   // Create an XNET session in FrameOutSinglePoint mode
   l_Status = nxCreateSession(l_pSelectedDatabase, l_pSelectedCluster,
      l_pSelectedFrameList, l_pSelectedInterface, nxMode_FrameOutSinglePoint,
      &m_SessionRef);
   if (nxSuccess != l_Status)
   {
      DisplayErrorAndExit(l_Status, "nxCreateSession");
   }
   else
   {
      if (0 != l_IsMaster)
      {
         // Set the schedule - this will also automatically enable master mode
         l_Status = nxWriteState(m_SessionRef, nxState_LINScheduleChange,
            sizeof(l_ScheduleIndex), &l_ScheduleIndex);
         if (nxSuccess != l_Status)
         {
            DisplayErrorAndExit(l_Status, "nxWriteState");
         }
      }
   }
   printf("Session Created Successfully.\n");

   // Get a pointer to the first frame's memory
   l_pFrame = (nxFrameVar_t *)l_Buffer;

   // Set each of the first frame's parameters (skipping the payload for now).
   // There are ways to programmatically get these parameters
   // using the objects properties. Here we hardcode them.
   l_pFrame->Timestamp = 0;
   l_pFrame->Flags = 0;
   l_pFrame->Identifier = 66;
   l_pFrame->Type = nxFrameType_LIN_Data;
   l_pFrame->PayloadLength = 8;

   // Iterate the pointer to the next frame
   l_pFrame = nxFrameIterate(l_pFrame);

   // Set the second frame's parameters (skipping the payload for now).
   l_pFrame->Timestamp = 0;
   l_pFrame->Flags = 0;
   l_pFrame->Identifier = 67;
   l_pFrame->Type = nxFrameType_LIN_Data;
   l_pFrame->PayloadLength = 2;

   // Get the actual size of the buffer used
   l_pFrame = nxFrameIterate(l_pFrame);
   l_NumberOfBytesForFrames = (u32)((u8 *)l_pFrame - (u8 *)l_Buffer);

   printf("Press any key to update the frame's value or .q to quit\n\n");

   // Main loop
   while ('q' != tolower(_getch()))
   {
      // Update the payload of the first frame
      l_pFrame = (nxFrameVar_t *)l_Buffer;
      for (j = 0; j < l_pFrame->PayloadLength; ++j)
      {
         l_pFrame->Payload[j]= (u8)(j + i);
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
