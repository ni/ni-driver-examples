/*
// This example reads the latest signal values when a keyboard character is
// pressed. This is used to demonstrate a signal single point input session.
// For more information about this type of session, please consult the NI-XNET
// manual.
// This example uses hardcoded signal names that use the NIXNET_exampleLDF database.
// To use your own database, you need to add an alias to your database file using
// the NI-XNET Database Editor and then modify the database name and signals used here.
// Please make sure that the bus is properly terminated as this example does not
// enable the on-board termination. Also ensure that the transceivers are
// externally powered when using C Series modules.
*/

#include "../../example_support.h" // Include file for Sleep, _getch
#include <nixnet.h>                // Include file for NI-XNET functions and constants
#include <stdlib.h>                // Include file for various functions
#include <stdio.h>                 // Include file for printf
#include <ctype.h>                 // Include file for tolower

#define NUM_SIGNALS 2

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
   char *l_pSelectedInterface = "LIN2";
   char *l_pSelectedDatabase = "NIXNET_exampleLDF";
   char *l_pSelectedCluster = "Cluster";
   char *l_pSelectedSignalList = "MasterSignal1_U16,MasterSignal2_U16";
   f64 l_ValueBuffer[NUM_SIGNALS];
   nxTimestamp_t l_TimestampBuffer[NUM_SIGNALS];
   nxStatus_t l_Status = 0;

   // Change this to set the interface to master mode
   u32 l_IsMaster  = 0;

   // The schedule is identified by its index. The index is mapped to
   // the schedules in the database. Index 0 is the first schedule
   // displayed in the Database Editor.
   u32 l_ScheduleIndex = 0;

   // Display parameters that will be used for the example.
   printf("Interface: %s\nDatabase: %s\nSignal List: %s\n",
      l_pSelectedInterface, l_pSelectedDatabase, l_pSelectedSignalList);

   if (0 != l_IsMaster)
   {
      printf("Master?: Yes\n");
   }
   else
   {
      printf("Master?: No\n");
   }

   // Create an XNET session in SignalInSinglePoint mode
   l_Status = nxCreateSession(l_pSelectedDatabase, l_pSelectedCluster,
      l_pSelectedSignalList, l_pSelectedInterface, nxMode_SignalInSinglePoint,
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
         printf("Session Created Successfully.\n");
      }

   printf("Press any key to read latest signal value. (q to quit)\n");

   // Main loop
   while ('q' != tolower(_getch()))
   {
      // Read the latest signal data. The first read will also auto-start the
      // session and normally return the default values defined in the database.
      // Use the nxStart function to start the session before the initial read.
      l_Status = nxReadSignalSinglePoint(m_SessionRef, l_ValueBuffer,
         sizeof(l_ValueBuffer), l_TimestampBuffer,
         sizeof(l_TimestampBuffer));
      if (nxSuccess == l_Status)
      {
         // Print the values ignoring the timestamps
         printf("Signals received:\n");
         printf("Signal 1: %f\n", l_ValueBuffer[0]);
         printf("Signal 2: %f\n\n", l_ValueBuffer[1]);
      }
      else
      {
         DisplayErrorAndExit(l_Status, "nxReadSignalSinglePoint");
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
