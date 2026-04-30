/*
// This example reads an XY signal waveform. The XY session returns an array
// of values with corresponding timestamps.
// This is used to demonstrate a signal XY input session.
// For more information about this type of session, please consult the NI-XNET
// manual. This example uses hardcoded signal names that use the NIXNET_example
// database. To use your own database, you need to add an alias to your
// database file using the NI-XNET Database Editor and then modify the database
// name and signals used here.
// Please make sure that the bus is properly terminated as this example does not
// enable the on-board termination. Also ensure that the transceivers are
// externally powered when using C Series modules.
*/

#include "../../example_support.h" // Include file for Sleep, _getch, PrintTimestamp
#include <nixnet.h>                // Include file for NI-XNET functions and constants
#include <stdlib.h>                // Include file for various functions
#include <stdio.h>                 // Include file for printf
#include <ctype.h>                 // Include file for tolower

#define NUM_SAMP 100
#define NUM_SIGNALS 2
#define NUM_DISPLAYCOLUMNS 2

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
   unsigned int l_Column = 1;
   int l_TypedChar = 0;
   char *l_pSelectedInterface = "CAN2";
   char *l_pSelectedDatabase = "NIXNET_example";
   char *l_pSelectedCluster = "CAN_Cluster";
   char *l_pSelectedSignalList = "CANCyclicSignal1,CANCyclicSignal2";
   f64 l_ValueBuffer[NUM_SIGNALS][NUM_SAMP];
   nxTimestamp_t l_TimeStampBuffer[NUM_SIGNALS][NUM_SAMP];
   u32 l_NumPairsBuffer[NUM_SIGNALS];
   nxStatus_t l_Status = 0;

   // Display parameters that will be used for the example.
   printf("Interface: %s\nDatabase: %s\nSignal List: %s\n",
      l_pSelectedInterface, l_pSelectedDatabase, l_pSelectedSignalList);

   // Initialize all values to 0, or we will think we have samples!
   for (i = 0; i < NUM_SIGNALS; ++i)
   {
      l_NumPairsBuffer[i] = 0;
   }

   // Create an XNET session in SignalInXY mode
   l_Status = nxCreateSession(l_pSelectedDatabase, l_pSelectedCluster,
      l_pSelectedSignalList, l_pSelectedInterface, nxMode_SignalInXY,
      &m_SessionRef);
   if (nxSuccess == l_Status)
   {
      printf("Session created successfully.\n");
   }
   else
   {
      DisplayErrorAndExit(l_Status, "nxCreateSession");
   }

   printf("Press q to quit.\n");

   // Main loop
   do
   {
      // Read the XY signal data
      l_Status = nxReadSignalXY(m_SessionRef, NULL,
         (f64 *)l_ValueBuffer, sizeof(l_ValueBuffer),
         (nxTimestamp_t *)l_TimeStampBuffer, sizeof(l_TimeStampBuffer),
         (u32 *)l_NumPairsBuffer, sizeof(l_NumPairsBuffer));
      if (nxSuccess == l_Status)
      {
         // For each loop, print the x values paired with y values
         for (i = 0; i < NUM_SIGNALS; ++i)
         {
            printf("\nSignal %d\n", i + 1);
            if (l_NumPairsBuffer[i] == 0)
            {
               printf("No values available.\n");
            }
            else
            {
               l_Column = 1;
               for (j = 0; j < l_NumPairsBuffer[i]; ++j)
               {
                  PrintTimestamp(&l_TimeStampBuffer[i][j]);
                  printf(" %f", l_ValueBuffer[i][j]);

                  if (++l_Column <= NUM_DISPLAYCOLUMNS)
                  {
                     printf("\t");
                  }
                  else
                  {
                     printf("\n");
                     l_Column = 1;
                  }
               }
            }
         }
      }
      else
      {
         DisplayErrorAndExit(l_Status, "nxReadSignalXY");
      }

      Sleep(100);

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

