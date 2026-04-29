/*
// This example reads the latest frame when a keyboard character is pressed.
// This is used to demonstrate a frame single point input session.
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
   unsigned int l_NumBytes = 0;
   char *l_pSelectedInterface = "LIN2";
   char *l_pSelectedDatabase = "NIXNET_exampleLDF";
   char *l_pSelectedCluster = "Cluster";
   char *l_pSelectedFrameList = "MasterFrame1,MasterFrame2";
   u8 l_Buffer[NUM_FRAMES * sizeof(nxFrameLIN_t)];
   nxFrameVar_t *l_pFrame = NULL;
   nxStatus_t l_Status = 0;

   // Change this to set the interface to master mode
   u32 l_IsMaster  = 0;

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

   // Create an XNET session in FrameInSinglePoint mode
   l_Status = nxCreateSession(l_pSelectedDatabase, l_pSelectedCluster,
      l_pSelectedFrameList, l_pSelectedInterface, nxMode_FrameInSinglePoint,
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

   printf("Press any key to read latest frame value. (q to quit)\n");

   // Main loop
   while ('q' != tolower(_getch()))
   {
      // Read the latest frame data. The first read will also auto-start the
      // session and normally return the default payload values.
      // Use the nxStart function to start the session before the initial read.
      l_Status = nxReadFrame(m_SessionRef, l_Buffer, sizeof(l_Buffer),
         nxTimeout_None, &l_NumBytes);
      if (nxSuccess != l_Status)
      {
         DisplayErrorAndExit(l_Status, "nxReadFrame");
      }
      else
      {
         l_pFrame = (nxFrameVar_t *)l_Buffer;
         // FramePtr iterates to each frame. When it increments
         // past the number of bytes returned, we reached the end.
         while ((u8 *)l_pFrame < (u8 *)l_Buffer + l_NumBytes)
         {
            // Print the frame's ID
            printf("Frame ID: %d, ", l_pFrame->Identifier);

            // Print the frame's payload
            printf("Payload Bytes: ");
            for (i = 0; i < l_pFrame->PayloadLength; ++i)
            {
               printf("%02X ", l_pFrame->Payload[i]);
            }
            printf("\n");

            // Go to next variable-payload frame.
            l_pFrame = nxFrameIterate(l_pFrame);
         }
         printf("\n");
      }
   }

   // Clear the XNET session
   l_Status = nxClear(m_SessionRef);
   if (nxSuccess == l_Status)
   {
      printf("Session cleared successfully!\n");
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
