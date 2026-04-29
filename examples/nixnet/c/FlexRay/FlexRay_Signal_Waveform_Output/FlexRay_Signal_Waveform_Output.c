/*
// This example writes a waveform signal using the default resample rate.
// This is used to demonstrate a signal waveform output session.
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

#define NUM_SAMP 300
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
   unsigned int i = 0;
   int l_TypedChar = 0;
   f64 l_MinOut = 0.0;
   f64 l_MaxOut = 10.0;
   f64 l_IncrementOut = 0.0;
   char *l_pSelectedInterface = "FlexRay1";
   char *l_pSelectedDatabase = "NIXNET_example";
   char *l_pSelectedCluster = "FlexRay_Cluster";
   char *l_pSelectedSignalList = "FlexRayCyclicSignal1,FlexRayCyclicSignal2";
   f64 l_ValueBuffer[NUM_SIGNALS][NUM_SAMP];
   u32 l_KeySlotId = 2;
   nxStatus_t l_Status = 0;

   // Display parameters that will be used for the example.
   printf("Interface: %s\nDatabase: %s\n", l_pSelectedInterface,
      l_pSelectedDatabase);
   printf("Signal List: %s\n", l_pSelectedSignalList);
   printf("KeyslotID: %d\n", l_KeySlotId);

   // Create an XNET session in SignalOutWaveform mode
   // Default resample rate (1000 Hz) will be used
   l_Status = nxCreateSession(l_pSelectedDatabase, l_pSelectedCluster,
      l_pSelectedSignalList, l_pSelectedInterface, nxMode_SignalOutWaveform,
      &m_SessionRef);
   if (nxSuccess == l_Status)
   {
      printf("Session created successfully.\n");
   }
   else
   {
      DisplayErrorAndExit(l_Status, "nxCreateSession");
   }

   l_Status = nxSetProperty(m_SessionRef, nxPropSession_IntfFlexRayKeySlotID,
      sizeof(l_KeySlotId), &l_KeySlotId);
   if (nxSuccess != l_Status)
   {
      DisplayErrorAndExit(l_Status, "nxSetProperty");
   }

   printf("Values are incremented from %f to %f in this example.\n",
      l_MinOut, l_MaxOut);

   // We generate the output waveforms as a ramp from l_MinOut to l_MaxOut.
   // Calculate the increment for each sample.
   // Each channel is 1/2 waveform out of phase from the other.
   l_IncrementOut = (l_MaxOut - l_MinOut) / NUM_SAMP;
   for (i = 0; i < NUM_SAMP; ++i)
   {
      l_ValueBuffer[0][i] = l_MinOut + (l_IncrementOut * i);
      l_ValueBuffer[1][i] = l_MinOut + (l_IncrementOut *
         ((i + (NUM_SAMP / 2)) % NUM_SAMP));
   }

   printf("Press q to quit\n");

   // Main loop
   i = 0;
   do
   {
      // Write the waveform data
      l_Status = nxWriteSignalWaveform(m_SessionRef, 10.0,
         (f64 *)l_ValueBuffer, sizeof(l_ValueBuffer));
      if (nxSuccess == l_Status)
      {
         printf("\rLoop %d", ++i);
      }
      else
      {
         DisplayErrorAndExit(l_Status, "nxWriteSignalWaveform");
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
