/*
// This example reads and writes signals on the same FlexRay port.
// This is used to demonstrate that you can open a signal single point input
// session and a single point output session on the same port.
// For more information about these types of sessions, please consult the NI-XNET
// manual.
// This example uses hardcoded signal names that use the NIXNET_example database.
// To use your own database, you need to add an alias to your database file using
// the NI-XNET Database Editor and then modify the database name and signals used here.
// Please make sure that the bus is properly terminated as this example does not
// enable the on-board termination.
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
   int l_TypedChar = 0;
   char *l_pSelectedInterface = "FlexRay1";
   char *l_pSelectedDatabase = "NIXNET_example";
   char *l_pSelectedCluster = "FlexRay_Cluster";
   char *l_pSelectedInputSignalList = "FlexRayEventSignal1,FlexRayCyclicSignal1";
   char *l_pSelectedOutputSignalList = "FlexRayCyclicSignal3,FlexRayCyclicSignal4";
   f64 l_OutputValueBuffer[NUM_SIGNALS_OUT];
   f64 l_InputValueBuffer[NUM_SIGNALS_IN];
   nxTimestamp_t l_TimestampBuffer[NUM_SIGNALS_IN];
   u32 l_KeySlotId = 2;
   nxStatus_t l_Status = 0;

   // Display parameters that will be used for the example.
   printf("Interface: %s\nDatabase: %s\n", l_pSelectedInterface,
      l_pSelectedDatabase);
   printf("Input Signal List: %s\nOutput Signal List: %s\n",
      l_pSelectedInputSignalList, l_pSelectedOutputSignalList);
   printf("KeyslotID: %d\n", l_KeySlotId);

   // Create an XNET session in SignalInSinglePoint mode
   l_Status = nxCreateSession(l_pSelectedDatabase, l_pSelectedCluster,
      l_pSelectedInputSignalList, l_pSelectedInterface, nxMode_SignalInSinglePoint,
      &m_InputSessionRef);
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
      l_pSelectedOutputSignalList, l_pSelectedInterface, nxMode_SignalOutSinglePoint,
      &m_OutputSessionRef);
   if (nxSuccess == l_Status)
   {
      printf("Output session created successfully.\n");
   }
   else
   {
      DisplayErrorAndExit(l_Status, "nxCreateSession");
   }

   // Set the Key Slot
   l_Status = nxSetProperty(m_OutputSessionRef,
      nxPropSession_IntfFlexRayKeySlotID, sizeof(l_KeySlotId), &l_KeySlotId);
   if (nxSuccess != l_Status)
   {
      DisplayErrorAndExit(l_Status, "nxSetProperty");
   }

   printf("Press 't' to transmit and 'r' to receive. Press 'q' to quit.\n");

   // Main loop
   while ('q' != (l_TypedChar = tolower(_getch())))
   {
      switch (l_TypedChar)
      {
      case 'r':
         // Read the latest signal data. The first read will also auto-start the
         // session and normally return the default values defined in the database.
         // Use the nxStart function to start the session before the initial read.
         l_Status = nxReadSignalSinglePoint(m_InputSessionRef,
            l_InputValueBuffer, sizeof(l_InputValueBuffer),
            l_TimestampBuffer, sizeof(l_TimestampBuffer));
         if (nxSuccess == l_Status)
         {
            // Print the values ignoring timestamps
            printf("Signals received:\n");
            printf("Signal 1: %f\n", l_InputValueBuffer[0]);
            printf("Signal 2: %f\n\n", l_InputValueBuffer[1]);
         }
         else
         {
            DisplayErrorAndExit(l_Status, "nxReadSignalSinglePoint");
         }
         break;

      case 't':
         l_OutputValueBuffer[0] = (f64)i;
         l_OutputValueBuffer[1] = (f64)(i * 10);

         // Update the signal data
         l_Status = nxWriteSignalSinglePoint(m_OutputSessionRef,
            l_OutputValueBuffer, sizeof(l_OutputValueBuffer));
         if (nxSuccess == l_Status)
         {
            printf("Signals sent:\n");
            printf("Signal 1: %f\n", l_OutputValueBuffer[0]);
            printf("Signal 2: %f\n\n", l_OutputValueBuffer[1]);
            if (++i > 10)
            {
               i = 0;
            }
         }
         else
         {
            DisplayErrorAndExit(l_Status, "nxWriteSignalSinglePoint");
         }
         break;

      default:
         printf("No action.\n");
         break;
      }
   }

   // Clear the XNET output session
   l_Status = nxClear(m_OutputSessionRef);
   if (nxSuccess == l_Status)
   {
      printf("Output session cleared successfully!\n");
   }
   else
   {
      DisplayErrorAndExit(l_Status, "nxClear");
   }

   // Clear the XNET input session
   l_Status = nxClear(m_InputSessionRef);
   if (nxSuccess == l_Status)
   {
      printf("Input session cleared successfully!\n");
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

   nxClear(m_InputSessionRef);
   nxClear(m_OutputSessionRef);

   _getch();
   exit(1);
}
