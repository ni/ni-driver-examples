/*
// This example reads all frames from the bus and displays them in table
// format. This is used to demonstrate a frame input stream session.
// For more information about this type of session, please consult the NI-XNET
// manual.
// This example uses the NIXNET_exampleLDF database.
// To use your own database, you need to add an alias to your database file using
// the NI-XNET Database Editor and then modify the database name used here.
// Please make sure that the bus is properly terminated as this example does not
// enable the on-board termination. Also ensure that the transceivers are
// externally powered when using C Series modules.
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
   int l_TypedChar = 0;
   char *l_pSelectedInterface = "LIN2";
   char *l_pSelectedDatabase = "NIXNET_exampleLDF";
   char *l_pSelectedCluster = "Cluster";
   char *l_pSelectedFrameList = "";
   u8 l_Buffer[250 * sizeof(nxFrameLIN_t)]; // Use a large buffer for stream input
   unsigned int l_NumBytes = 0;
   nxFrameVar_t *l_pFrame = NULL;
   nxStatus_t l_Status = 0;

   // Change this to set the interface to master mode
   u32 l_IsMaster  = 0;

   // The schedule is identified by its index. The index is mapped to
   // the schedules in the database. Index 0 is the first schedule
   // displayed in the Database Editor.
   u32 l_ScheduleIndex = 0;

   // Change this if no response frames are not to be logged.
   u8 l_LogNoResponse = 1;

   // Display parameters that will be used for the example.
   printf("Interface: %s\nDatabase: %s\n", l_pSelectedInterface,
      l_pSelectedDatabase);

   if (0 != l_IsMaster)
   {
      printf("Master?: Yes\n");
   }
   else
   {
      printf("Master?: No\n");
   }

   if (0 != l_LogNoResponse)
   {
      printf("Log No-Response Frames?: Yes\n");
   }
   else
   {
      printf("Log No-Response Frames?: No\n");
   }

   // Create an XNET session in FrameInStream mode
   l_Status = nxCreateSession(l_pSelectedDatabase, l_pSelectedCluster, l_pSelectedFrameList,
      l_pSelectedInterface, nxMode_FrameInStream, &m_SessionRef);
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

      if (0 != l_LogNoResponse)
      {
         // Enable No-Response Frame logging
         l_Status = nxSetProperty(m_SessionRef,
            nxPropSession_IntfLINNoResponseToInStrm, sizeof(l_LogNoResponse),
            &l_LogNoResponse);
         if (nxSuccess != l_Status)
         {
            DisplayErrorAndExit(l_Status, "nxSetProperty");
         }
      }
      printf("Session Created Successfully.\n");
   }
   printf("Logging all received frames. Press q to quit\n");
   printf("Timestamp\t\t  ID\tType\tData\n");

   // Main loop
   i = 0;
   do
   {
      // Read the received frames
      l_Status = nxReadFrame(m_SessionRef, l_Buffer, sizeof(l_Buffer),
         nxTimeout_None, &l_NumBytes);
      if (nxSuccess != l_Status)
      {
         DisplayErrorAndExit(l_Status, "nxReadFrame");
      }

      l_pFrame = (nxFrameVar_t *)l_Buffer;

      // FramePtr iterates to each frame. When it increments
      // past the number of bytes returned, we reached the end.
      while ((u8 *)l_pFrame < (u8 *)l_Buffer + l_NumBytes)
      {
         // Print timestamp, ID and payload
         PrintTimestamp(&l_pFrame->Timestamp);
         printf("\t%d\t ", l_pFrame->Identifier);
         // Print the frame type
         switch (l_pFrame->Type)
         {
         case nxFrameType_LIN_Data:
            printf("LIN Data       ");
            break;
         case nxFrameType_LIN_BusError:
            printf("LIN BusError   ");
            break;
         case nxFrameType_LIN_NoResponse:
            printf("LIN NoResponse ");
            break;
         default:
            printf("Other type     ");
            break;
         }
         printf("(%d)\t", l_pFrame->Type);
         for (i = 0; i < l_pFrame->PayloadLength; ++i)
         {
            printf("%02X ", l_pFrame->Payload[i]);
         }
         printf("\n");

         // Go to next variable-payload frame.
         l_pFrame = nxFrameIterate(l_pFrame);
      }

      Sleep(10);
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

