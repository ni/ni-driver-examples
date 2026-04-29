/*
// This example converts frames to signals, and then converts the signal values
// back to the frame payloads using a conversion session.
// For more information about this type of session, please consult the NI-XNET
// manual.
// This example uses hardcoded signal names that use the NIXNET_example database.
// To use your own database, you need to add an alias to your
// database file using the NI-XNET Database Editor and then modify the database
// name and signals used here.
*/

#include "../../example_support.h" // Include file for Sleep, _getch
#include <nixnet.h>                // Include file for NI-XNET functions and constants
#include <stdlib.h>                // Include file for various functions
#include <stdio.h>                 // Include file for printf
#include <ctype.h>                 // Include file for tolower

#define NUM_SIGNALS 2
#define PAYLOAD_SIZE_1 8
#define PAYLOAD_SIZE_2 8

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
   char *l_pSelectedDatabase = "NIXNET_example";
   char *l_pSelectedCluster = "FlexRay_Cluster";
   char *l_pSelectedSignalList = "FlexRayCyclicSignal3,FlexRayEventSignal1";
   u32 l_NumberOfBytesForFrames = 0;
   u32 l_NumberOfBytesReturnedConv = 0;
   u8 l_Buffer[sizeof(nxFrameFixed_t(PAYLOAD_SIZE_1)) + sizeof(nxFrameFixed_t(PAYLOAD_SIZE_2))];
   u8 l_BufferConv[sizeof(nxFrameFixed_t(PAYLOAD_SIZE_1)) + sizeof(nxFrameFixed_t(PAYLOAD_SIZE_2))];
   nxFrameVar_t *l_pFrame = NULL;
   nxFrameVar_t *l_pFrameConv = NULL;
   f64 l_ValueBuffer[NUM_SIGNALS];
   nxTimestamp_t l_TimestampBuffer[NUM_SIGNALS];
   nxStatus_t l_Status = 0;

   // Display parameters that will be used for the example.
   printf("Database: %s\nCluster: %s\nSignal List: %s\n",
      l_pSelectedDatabase, l_pSelectedCluster, l_pSelectedSignalList);

   // Create an XNET session in SignalConversionSinglePoint mode
   l_Status = nxCreateSession(l_pSelectedDatabase, l_pSelectedCluster,
      l_pSelectedSignalList, "", nxMode_SignalConversionSinglePoint,
      &m_SessionRef);
   if (nxSuccess == l_Status)
   {
      printf("Session created successfully.\n");
   }
   else
   {
      DisplayErrorAndExit(l_Status, "nxCreateSession");
   }

   printf("\nFrames to Convert\nSlot Id\tData\t\n");
   // Here we set each of the frames parameters.
   // There are ways to programmatically get these parameters using
   // the objects properties

   l_pFrame = (nxFrameVar_t *)l_Buffer;
   l_pFrame->Timestamp = 0;
   l_pFrame->Flags = nxFrameFlags_FlexRay_ChA | nxFrameFlags_FlexRay_ChB;
   l_pFrame->Identifier = 4;
   l_pFrame->Type = nxFrameType_FlexRay_Data;
   l_pFrame->PayloadLength = 8;
   for (i = 0; i < l_pFrame->PayloadLength; ++i)
   {
      l_pFrame->Payload[i] = (u8)(i + 1);
   }
   // Print the frame we just created
   printf("%d\t ", l_pFrame->Identifier);
   for (i = 0; i < l_pFrame->PayloadLength; ++i)
   {
      printf("%02X ", l_pFrame->Payload[i]);
   }
   printf("\n");

   // Iterate the pointer to the next frame
   l_pFrame = nxFrameIterate(l_pFrame);
   l_pFrame->Timestamp = 0;
   l_pFrame->Flags = nxFrameFlags_FlexRay_ChA | nxFrameFlags_FlexRay_ChB;
   l_pFrame->Identifier = 5;
   l_pFrame->Type = nxFrameType_FlexRay_Data;
   l_pFrame->PayloadLength = 8;
   for (i = 0; i < l_pFrame->PayloadLength; ++i)
   {
      l_pFrame->Payload[i] = (u8)(i + 9);
   }
   // Print the frame we just created
   printf("%d\t ", l_pFrame->Identifier);
   for (i = 0; i < l_pFrame->PayloadLength; ++i)
   {
      printf("%02X ", l_pFrame->Payload[i]);
   }
   printf("\n");

   // Get the actual size of the buffer used
   l_pFrame = nxFrameIterate(l_pFrame);
   l_NumberOfBytesForFrames = (u32)((u8 *)l_pFrame - (u8 *)l_Buffer);

   // Convert the frames to signal values
   l_Status = nxConvertFramesToSignalsSinglePoint(m_SessionRef, l_Buffer,
      l_NumberOfBytesForFrames, l_ValueBuffer, sizeof(l_ValueBuffer),
      l_TimestampBuffer, sizeof(l_TimestampBuffer));
   if (nxSuccess == l_Status)
   {
      // Print the values ignoring timestamps
      printf("\nConverted signal values\n");
      printf("Signal 1: %f\n", l_ValueBuffer[0]);
      printf("Signal 2: %f\n", l_ValueBuffer[1]);
   }
   else
   {
      DisplayErrorAndExit(l_Status, "nxConvertFramesToSignalsSinglePoint");
   }

   // Convert the signal values back to frames
   l_Status = nxConvertSignalsToFramesSinglePoint(m_SessionRef, l_ValueBuffer,
      sizeof(l_ValueBuffer), l_BufferConv, sizeof(l_BufferConv),
      &l_NumberOfBytesReturnedConv);
   if (nxSuccess == l_Status)
   {
      printf("\nFrames converted from signal values\nID\tData\n");

      l_pFrameConv = (nxFrameVar_t *)l_BufferConv;

      // l_pFrameConv iterates to each frame. When it increments
      // past the number of bytes returned, we reached the end.
      while ((u8 *)l_pFrameConv < (u8 *)l_BufferConv + l_NumberOfBytesReturnedConv)
      {
         // Print timestamp, ID and payload
         printf("%d\t", l_pFrameConv->Identifier);

         for (i = 0; i < l_pFrameConv->PayloadLength; ++i)
         {
            printf("%02X ", l_pFrameConv->Payload[i]);
         }
         printf("\n");

         // Go to next variable-payload frame.
         l_pFrameConv = nxFrameIterate(l_pFrameConv);
      }
   }
   else
   {
      DisplayErrorAndExit(l_Status, "nxConvertSignalsToFramesSinglePoint");
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

   printf("\nPress any key to quit\n");
   _getch();

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
