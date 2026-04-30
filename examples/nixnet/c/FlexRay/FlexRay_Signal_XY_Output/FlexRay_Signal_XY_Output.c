/*
// This example transmits a sequence of signal values without alterations.
// This is used to demonstrate a signal XY output session.
// For more information about this type of session, please consult the NI-XNET
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

#define NUM_SAMP 50
#define NUM_SIGNALS 1

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
   f64 l_MinOut = 0.0;
   f64 l_MaxOut = 40.0;
   f64 l_IncrementOut = 0.0;
   char *l_pSelectedInterface = "FlexRay1";
   char *l_pSelectedDatabase = "NIXNET_example";
   char *l_pSelectedCluster = "FlexRay_Cluster";
   char *l_pSelectedSignalList  = "FlexRayCyclicSignal1";
   f64 l_ValueBuffer[NUM_SIGNALS][NUM_SAMP];
   u32 l_NumPairsBuffer[NUM_SIGNALS];
   u32 l_KeySlotId = 2;
   nxStatus_t l_Status = 0;

   // Display parameters that will be used for the example.
   printf("Interface: %s\nDatabase: %s\n", l_pSelectedInterface,
      l_pSelectedDatabase);
   printf("Signal List: %s\n", l_pSelectedSignalList);
   printf("KeyslotID: %d\n", l_KeySlotId);

   // Create an XNET session in SignalOutXY mode
   l_Status = nxCreateSession(l_pSelectedDatabase, l_pSelectedCluster,
      l_pSelectedSignalList, l_pSelectedInterface, nxMode_SignalOutXY,
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
   l_Status = nxSetProperty(m_SessionRef,
      nxPropSession_IntfFlexRayKeySlotID, sizeof(l_KeySlotId), &l_KeySlotId);
   if (nxSuccess != l_Status)
   {
      DisplayErrorAndExit(l_Status, "nxSetProperty");
   }

   printf("Values are incremented from %f to approx. %f in this example.\n",
      l_MinOut, l_MaxOut);

   // We need to tell the driver how many timestamp/value pairs
   // we are sending for each (just one in this example) signal.
   l_NumPairsBuffer[0] = NUM_SAMP;

   // We generate the signal sequence as a ramp from MinOut to MaxOut.
   l_IncrementOut = (l_MaxOut - l_MinOut) / NUM_SAMP;
   for (i = 0; i < NUM_SAMP; ++i)
   {
      l_ValueBuffer[0][i] = l_MinOut +
         (l_IncrementOut * ((i + (NUM_SAMP / 2)) % NUM_SAMP));
   }

   printf("Press q to quit\n");

   // Main loop
   i = 0;
   do
   {
      // Write the XY signal data
      l_Status = nxWriteSignalXY(m_SessionRef, 10.0,
         (f64 *)l_ValueBuffer, sizeof(l_ValueBuffer),
         NULL, 0,
         (u32 *)l_NumPairsBuffer, sizeof(l_NumPairsBuffer));
      if (nxSuccess == l_Status)
      {
         printf("\rLoop %d", ++i);
      }
      else
      {
         DisplayErrorAndExit(l_Status, "nxWriteSignalXY");
      }

      if (_kbhit())
      {
         l_TypedChar = _getch();
      }
   }
   while ('q' != tolower(l_TypedChar));

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
