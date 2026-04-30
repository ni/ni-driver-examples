/*
// This example uses the NI-XNET-Helper.c functions to browse through a database
// file. In this example, we write all clusters, frames and signals to a text file.
// This example is used as a base to show how to use the database classes and
// functions.
*/

#define _CRT_SECURE_NO_WARNINGS

#include <nixnet.h>         // Include file for NI-XNET functions
#include <stdlib.h>         // Include file for various functions
#include <stdio.h>          // Include file for various functions
//#include <conio.h>          // Include file for _getch/kbhit
//#include <windows.h>        // Include file for Win32 time functions
#include "NI-XNET-Helper.h" // Include file with "helper" functions for NI-XNET

// Helper macros (only valid for XNET Status variables)
#define IsError(Status) (Status < nxSuccess)
#define IsNoError(Status) (Status >= nxSuccess)

//=============================================================================
// Static global variables
//=============================================================================
static FILE *m_pFileHandle = NULL;

//=============================================================================
// Global functions declerations
//=============================================================================
void DisplayErrorAndExit(nxStatus_t Status, char *Source);

//=============================================================================
// Main function
//=============================================================================
int main(void)
{
   // Declare all variables for the function

   // Counter variables
   unsigned int i = 0;
   unsigned int j = 0;
   unsigned int k = 0;
   unsigned int l = 0;

   // Arrays to store the names
   char **l_pInterfaceNameArray = NULL;
   char **l_pDbFilepathArray = NULL;
   char **l_pDbAliasArray = NULL;
   char **l_pClusterNameArray = NULL;
   char **l_pFrameNameArray = NULL;
   char **l_pSignalNameArray = NULL;
   char **l_pLinScheduleNameArray = NULL;

   // Array item count variables
   u32 l_NumberOfInterfaces = 0;
   u32 l_NumberOfDbFilepaths = 0;
   u32 l_NumberOfDbAliases = 0;
   u32 l_NumberOfClusters = 0;
   u32 l_NumberOfFrames = 0;
   u32 l_NumberOfSignals = 0;
   u32 l_NumberOfLinSchedules = 0;

   const char *l_pFileName = "output.txt";
   nxStatus_t l_Status = 0;

   printf("XNET Database Browser Example\n");
   printf("We are now analyzing your hardware setup and all installed databases.\n");
   printf("The results will be written to the file %s in the project folder.\n", l_pFileName);
   printf("Depending on your databases, this process might take some time, please wait...\n");

   // Open the output file
   m_pFileHandle = fopen(l_pFileName, "w");
   if (NULL == m_pFileHandle)
   {
      printf("\nERROR: Could not open file %s for writing! Aborting...\n", l_pFileName);
      exit(1);
   }

   fprintf(m_pFileHandle, "XNET Database Browser Example Output File\n");
   fprintf(m_pFileHandle, "Note: Only useable, correctly configured objects will be listed.\n\n");

   // First section: Print all the interface names
   fprintf(m_pFileHandle, "Interfaces in the system:\n");

   l_Status = GetAllInterfaces(&l_NumberOfInterfaces, &l_pInterfaceNameArray);
   if (IsError(l_Status))
   {
      DisplayErrorAndExit(l_Status, "GetAllInterfaces");
   }

   // Iterate over all interfaces
   for (i = 0; i < l_NumberOfInterfaces; ++i)
   {
      fprintf(m_pFileHandle, "%s\n", l_pInterfaceNameArray[i]);
      free(l_pInterfaceNameArray[i]);
   }
   if (0 < l_NumberOfInterfaces)
   {
      free(l_pInterfaceNameArray);
   }
   // We need to free the array items (pointers) as well as the array itself
   // because both are malloc'ed by the helper functions.


   // Second section: Search all database aliases and print all its clusters, frames,
   // schedules and signals.
   fprintf(m_pFileHandle, "\nDatabases and objects:");

   // Get a list of all database aliases
   l_Status = nxExampleGetDBAlias(&l_NumberOfDbAliases, &l_pDbAliasArray);
   if (IsError(l_Status))
   {
      DisplayErrorAndExit(l_Status, "nxExampleGetDBAlias");
   }

   // Get a list of all database file paths
   l_Status = nxExampleGetDBFilepaths(&l_NumberOfDbFilepaths, &l_pDbFilepathArray);
   if (IsError(l_Status))
   {
      DisplayErrorAndExit(l_Status, "nxExampleGetDBFilepaths");
   }

   // Iterate over all database aliases
   for (i = 0; i < l_NumberOfDbAliases; ++i)
   {
      // For each database, print the path and alias
      fprintf(m_pFileHandle, "\nDatabase file: %s\n", l_pDbFilepathArray[i]);
      fprintf(m_pFileHandle, "%s  (DB Alias)\n", l_pDbAliasArray[i]);

      // Get the database's cluster names
      l_Status = GetClusterNames(l_pDbAliasArray[i], &l_NumberOfClusters,
         &l_pClusterNameArray);
      if (IsError(l_Status))
      {
         DisplayErrorAndExit(l_Status, "GetClusterNames");
      }

      // Iterate over all clusters of the database
      for (j = 0; j < l_NumberOfClusters; ++j)
      {
         // For each cluster, print the name
         fprintf(m_pFileHandle, "   %s  (Cluster)\n", l_pClusterNameArray[j]);

         // Get the cluster's frame names
         l_Status = GetFrameNames(l_pDbAliasArray[i], l_pClusterNameArray[j],
            &l_NumberOfFrames, &l_pFrameNameArray);
         if (IsError(l_Status))
         {
            DisplayErrorAndExit(l_Status, "GetFrameNames");
         }

         // Iterate over all frames of the cluster
         for (k = 0; k < l_NumberOfFrames; ++k)
         {
            // For each frame, print the name
            fprintf(m_pFileHandle, "      %s  (Frame)\n", l_pFrameNameArray[k]);

            // Get the frame's signal names
            l_Status = GetSignalsNamesOfFrame(l_pDbAliasArray[i], l_pClusterNameArray[j],
               l_pFrameNameArray[k], &l_NumberOfSignals, &l_pSignalNameArray);
            if (IsError(l_Status))
            {
               DisplayErrorAndExit(l_Status, "GetSignalsNamesOfFrame");
            }

            // Iterate over all signals of the frame
            for (l = 0; l < l_NumberOfSignals; ++l)
            {
               fprintf(m_pFileHandle, "         %s  (Signal)\n", l_pSignalNameArray[l]);
               free(l_pSignalNameArray[l]);
            } // End signals
            if (0 < l_NumberOfSignals)
            {
               free(l_pSignalNameArray);
            }

            free(l_pFrameNameArray[k]);
         } // End frames
         if (0 < l_NumberOfFrames)
         {
            free(l_pFrameNameArray);
         }


         // Get the cluster's LIN Schedules
         l_Status = GetLINScheduleNames(l_pDbAliasArray[i], l_pClusterNameArray[j],
            &l_NumberOfLinSchedules, &l_pLinScheduleNameArray);
         if (IsError(l_Status))
         {
            DisplayErrorAndExit(l_Status, "GetLINScheduleNames");
         }

         // Iterate over all the LIN Schedules
         for (k = 0; k < l_NumberOfLinSchedules; ++k)
         {
            fprintf(m_pFileHandle, "      %s  (LIN Schedule)\n", l_pLinScheduleNameArray[k]);
            free(l_pLinScheduleNameArray[k]);
         } // End LIN Schedules
         if (0 < l_NumberOfLinSchedules)
         {
            free(l_pLinScheduleNameArray);
         }

         free(l_pClusterNameArray[j]);
      } // End clusters
      if (0 < l_NumberOfClusters)
      {
         free(l_pClusterNameArray);
      }

      free(l_pDbFilepathArray[i]);
      free(l_pDbAliasArray[i]);
   } // End databases
   if (0 < l_NumberOfDbFilepaths)
   {
      free(l_pDbFilepathArray);
   }
   if (0 < l_NumberOfDbAliases)
   {
      free(l_pDbAliasArray);
   }

   // We need to free the array items (pointers) as well as the array itself
   // because both are malloc'ed by the helper functions.
   fprintf(m_pFileHandle, "\nEnd of file");
   fclose(m_pFileHandle);
   printf("Analysis complete. Open the output file in a text editor to see the results.\n");

   return 0;
}

//=============================================================================
// Display Error Function
//=============================================================================
void DisplayErrorAndExit(nxStatus_t l_status, char *source)
{
   char statusString[1024];
   nxStatusToString(l_status, sizeof(statusString), statusString);

   printf("\n\nError at %s!\n%s\n", source, statusString);
   printf("\nExecution stopped.\nPress any key to quit\n");

   fprintf(m_pFileHandle, "\nCanceled because of an XNET Error or Warning at %s!\n%s\n", source, statusString);
   fclose(m_pFileHandle);

   exit(1);
}
