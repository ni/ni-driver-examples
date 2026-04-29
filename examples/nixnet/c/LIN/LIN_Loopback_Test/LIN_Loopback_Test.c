/*
// This example can be used to test LIN communication between two ports.
// A signal value is sent on one port and is received on the other port.
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

#define NUM_SIGNALS_OUT 2
#define NUM_SIGNALS_IN 2

//=============================================================================
// Static global variables
//=============================================================================
static nxSessionRef_t m_InputSessionRef = 0;
static nxSessionRef_t m_OutputSessionRef = 0;

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
   char *l_pSelectedInterface1 = "LIN1";
   char *l_pSelectedInterface2 = "LIN2";
   char *l_pSelectedDatabase = "NIXNET_exampleLDF";
   char *l_pSelectedCluster = "Cluster";
   char *l_pSelectedInputSignalList = "MasterSignal1_U16,MasterSignal2_U16";
   char *l_pSelectedOutputSignalList = "MasterSignal1_U16,MasterSignal2_U16";
   f64 l_OutputValueBuffer[NUM_SIGNALS_OUT];
   f64 l_InputValueBuffer[NUM_SIGNALS_IN];
   nxTimestamp_t l_TimestampBuffer[NUM_SIGNALS_IN];
   nxStatus_t l_Status = 0;

   // The schedule needs to be an index. The index is mapped to the
   // schedules in the database. Index 0 is the first schedule.
   u32 l_ScheduleIndex = 0;

   // Display parameters that will be used for the example.
   printf("Interfaces: %s and %s\nDatabase: %s\n", l_pSelectedInterface1,
      l_pSelectedInterface2, l_pSelectedDatabase);
   printf("Input Signal List: %s\nOutput Signal List: %s\n",
      l_pSelectedInputSignalList, l_pSelectedOutputSignalList);

   // Create an XNET session in SignalInSinglePoint mode
   l_Status = nxCreateSession(l_pSelectedDatabase, l_pSelectedCluster,
      l_pSelectedInputSignalList, l_pSelectedInterface1,
      nxMode_SignalInSinglePoint, &m_InputSessionRef);
   if (nxSuccess == l_Status)
   {
      printf("Input session created successfully.\n");
   }
   else
   {
      DisplayErrorAndExit(l_Status, "nxCreateSession");
   }

   // Create an XNET session in SignalOutSinglePoint mode
   l_Status = nxCreateSession(l_pSelectedDatabase, l_pSelectedCluster,
      l_pSelectedOutputSignalList, l_pSelectedInterface2,
      nxMode_SignalOutSinglePoint, &m_OutputSessionRef);
   if (nxSuccess != l_Status)
   {
      DisplayErrorAndExit(l_Status, "nxCreateSession");
   }
   else
   {
      // Set the schedule - this will also automatically enable master mode
      l_Status = nxWriteState(m_OutputSessionRef, nxState_LINScheduleChange,
         sizeof(l_ScheduleIndex), &l_ScheduleIndex);
      if (nxSuccess != l_Status)
      {
         DisplayErrorAndExit(l_Status, "nxWriteState");
      }
      printf("Output session created successfully.\n");
   }

   // Start the input session manually to make sure that the first
   // signal value sent before the initial read will be received.
   l_Status = nxStart(m_InputSessionRef, nxStartStop_Normal);
   if (nxSuccess == l_Status)
   {
      printf("Input session started manually.\n");
   }
   else
   {
      DisplayErrorAndExit(l_Status, "nxStart");
   }

   printf("Press any key to transmit (q to quit).\n");
   printf("The same values should be received.\n");

   // Main loop
   i = 1;
   while ('q' != tolower(_getch()))
   {
      // Write out values incrementing them each time
      l_OutputValueBuffer[0] = (f64)i;
      l_OutputValueBuffer[1] = (f64)(i * 10);

      // Update the signal values
      l_Status = nxWriteSignalSinglePoint(m_OutputSessionRef,
         l_OutputValueBuffer, sizeof(l_OutputValueBuffer));
      if (nxSuccess == l_Status)
      {
         printf("Signals sent:\n");
         printf("Signal 1: %f\n", l_OutputValueBuffer[0]);
         printf("Signal 2: %f\n", l_OutputValueBuffer[1]);
         if (++i > 10)
         {
            i = 0;
         }
      }
      else
      {
         DisplayErrorAndExit(l_Status, "nxWriteSignalSinglePoint");
      }

      // Wait 100ms and then read back the values.
      // They should be the same as the ones sent.
      Sleep(100);

      l_Status = nxReadSignalSinglePoint(m_InputSessionRef,
         l_InputValueBuffer, sizeof(l_InputValueBuffer),
         l_TimestampBuffer, sizeof(l_TimestampBuffer));
      if (nxSuccess == l_Status)
      {
         // Print the values ignoring the timestamps
         printf("Signals received:\n");
         printf("Signal 1: %f\n", l_InputValueBuffer[0]);
         printf("Signal 2: %f\n\n", l_InputValueBuffer[1]);
      }
      else
      {
         DisplayErrorAndExit(l_Status, "nxReadSignalSinglePoint");
      }
   }

   // Clear the XNET sessions
   l_Status = nxClear(m_OutputSessionRef);
   if (nxSuccess != l_Status)
   {
      DisplayErrorAndExit(l_Status, "nxClear");
   }

   l_Status = nxClear(m_InputSessionRef);
   if (nxSuccess != l_Status)
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

   nxClear(m_OutputSessionRef);
   nxClear(m_InputSessionRef);

   _getch();
   exit(1);
}
