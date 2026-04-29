/*
// This example reads a waveform signal. Selected samples from the waveform are
// displayed together with its StartTime and sample rate.
// This is used to demonstrate a signal waveform input session.
// For more information about this type of session, please consult the NI-XNET
// manual. This example uses hardcoded signal names that use the NIXNET_example
// database. To use your own database, you need to add an alias to your
// database file using the NI-XNET Database Editor and then modify the database
// name and signals used here.
// Please make sure that the bus is properly terminated as this example does not
// enable the on-board termination.
*/

#include "../../example_support.h" // Include file for Sleep, _getch, _kbhit, PrintTimestamp
#include <nixnet.h>                // Include file for NI-XNET functions and constants
#include <stdlib.h>                // Include file for various functions
#include <stdio.h>                 // Include file for printf
#include <ctype.h>                 // Include file for tolower

#define NUM_SAMP 100
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
   char *l_pSelectedInterface = "FlexRay2";
   char *l_pSelectedDatabase = "NIXNET_example";
   char *l_pSelectedCluster = "FlexRay_Cluster";
   char *l_pSelectedSignalList = "FlexRayCyclicSignal1,FlexRayCyclicSignal2";
   u32 l_NumRead = 0;
   nxTimestamp_t l_StartTime = 0;
   f64 l_DeltaTime = 0.0;
   f64 l_ValueBuffer[NUM_SIGNALS][NUM_SAMP];
   u32 l_KeySlotId = 1;
   nxStatus_t l_Status = 0;

   // Display parameters that will be used for the example.
   printf("Interface: %s\nDatabase: %s\n", l_pSelectedInterface,
      l_pSelectedDatabase);
   printf("Signal List: %s\n", l_pSelectedSignalList);
   printf("KeyslotID: %d\n", l_KeySlotId);

   // Create an XNET session in SignalInWaveform mode
   // Default resample rate (1000 Hz) will be used
   l_Status = nxCreateSession(l_pSelectedDatabase, l_pSelectedCluster,
      l_pSelectedSignalList, l_pSelectedInterface, nxMode_SignalInWaveform,
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

   printf("Press q to quit\n");

   // Main loop
   i = 0;
   do
   {
      // Read the waveform data
      l_Status = nxReadSignalWaveform(m_SessionRef, 1.0, &l_StartTime,
         &l_DeltaTime, (f64 *)l_ValueBuffer, sizeof(l_ValueBuffer),
         &l_NumRead);
      if (nxSuccess == l_Status)
      {
         // For each loop, print the StartTime of the waveform, then
         // a few selected samples for each channel.
         printf("Loop %d: StartTime = ", ++i);
         PrintTimestamp(&l_StartTime);
         printf(" DeltaTime = %f us\n", l_DeltaTime);
         printf("\tSignal1:  [0] = %2.3f ... [50] = %2.3f, [99] = %2.3f\n",
         l_ValueBuffer[0][0], l_ValueBuffer[0][50], l_ValueBuffer[0][99]);
         printf("\tSignal2:  [0] = %2.3f ... [50] = %2.3f, [99] = %2.3f\n",
         l_ValueBuffer[1][0], l_ValueBuffer[1][50], l_ValueBuffer[1][99]);
         printf("\n");
      }
      else
      {
         DisplayErrorAndExit(l_Status, "nxReadSignalWaveform");
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

