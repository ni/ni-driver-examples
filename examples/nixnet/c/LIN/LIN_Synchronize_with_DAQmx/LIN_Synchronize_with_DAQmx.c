/*
// This example displays the synchronization capabilities of the
// NI-XNET boards by connecting a clock and a trigger over the RTSI
// interface to a DAQ card using the DAQmx driver.
// For more information about this type of session, please consult the NI-XNET
// manual.
// This example uses hardcoded signal names that use the NIXNET_exampleLDF database.
// To use your own database, you need to add an alias to your database file using
// the NI-XNET Database Editor and then modify the database name and signals used here.
// Please make sure that the bus is properly terminated as this example does not
// enable the on-board termination. Also ensure that the transceivers are
// externally powered when using C Series modules.
*/

#include "../../example_support.h" // Include file for Sleep, _getch, _kbhit, PrintTimestamp
#include <nixnet.h>                // Include file for NI-XNET functions and constants
#include <NIDAQmx.h>               // Include file for NI-DAQmx functions
#include <stdlib.h>                // Include file for various functions
#include <stdio.h>                 // Include file for printf
#include <ctype.h>                 // Include file for tolower

#define NUM_SAMP 100
#define NUM_SIGNAL 2
#define ERRBUFF_SIZE 2048

// Prototype for DAQmx Error Handling
#define DAQmxErrChk(functionCall) if (DAQmxFailed(l_Error=(functionCall))) goto Error; else

//=============================================================================
// Static global variables
//=============================================================================
static nxSessionRef_t m_SessionRef = 0;
static TaskHandle m_DaqTask;

//=============================================================================
// Global variables
//=============================================================================
char g_ErrBuff[ERRBUFF_SIZE] = {'\0'};

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
   int l_Error = 0;
   unsigned int l_NumRead = 0;
   char *l_pSelectedInterface = "LIN2";
   char *l_pSelectedDatabase = "NIXNET_exampleLDF";
   char *l_pSelectedCluster = "Cluster";
   char *l_pSelectedSignalList = "MasterSignal1_U16,MasterSignal2_U16";
   nxTimestamp_t l_StartTime = 0;
   f64 l_DeltaTime = 0.0;
   f64 l_ValueBuffer[NUM_SIGNAL][NUM_SAMP];
   f64 l_DaqValueBuffer[NUM_SIGNAL][NUM_SAMP];
   char *l_pDaqChannelString = "Dev1/ai0";
   u32 l_SampleRate = 1000;
   nxStatus_t l_Status = 0;

   // For this example, the interface is always a slave

   // Display parameters that will be used for the example.
   printf("Interface: %s\nDatabase: %s\nSignal List: %s\n",
      l_pSelectedInterface, l_pSelectedDatabase, l_pSelectedSignalList);

   // Configure DAQ Task, channel and sample clock
   DAQmxErrChk(DAQmxCreateTask("DAQ Task", &m_DaqTask));
   DAQmxErrChk(DAQmxCreateAIVoltageChan(m_DaqTask, l_pDaqChannelString, "",
      DAQmx_Val_Cfg_Default, -10.0, 10.0, DAQmx_Val_Volts, ""));
   DAQmxErrChk(DAQmxCfgSampClkTiming(m_DaqTask, "OnboardClock", l_SampleRate,
      DAQmx_Val_Rising, DAQmx_Val_ContSamps, NUM_SAMP));

   // Export DAQ Signals
   DAQmxErrChk(DAQmxExportSignal(m_DaqTask, DAQmx_Val_10MHzRefClock, "/Dev1/RTSI1"));
   DAQmxErrChk(DAQmxExportSignal(m_DaqTask, DAQmx_Val_StartTrigger, "/Dev1/RTSI0"));

   // Create an XNET session in SignalInWaveform mode
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

   // Import Master Timebase and Start Trigger
   l_Status = nxConnectTerminals(m_SessionRef, "PXI_Trig0", "MasterTimebase");
   if (l_Status != nxSuccess)
   {
      DisplayErrorAndExit(l_Status, "nxConnectTerminals");
   }

   l_Status = nxConnectTerminals(m_SessionRef, "PXI_Trig1", "StartTrigger");
   if (l_Status != nxSuccess)
   {
      DisplayErrorAndExit(l_Status, "nxConnectTerminals");
   }

   // Start the XNET task first so it waits for the DAQ Start trigger
   l_Status = nxStart(m_SessionRef, nxStartStop_Normal);
   if (l_Status != nxSuccess)
   {
      DisplayErrorAndExit(l_Status, "nxStart");
   }

   // Start the DAQ task
   DAQmxErrChk(DAQmxStartTask(m_DaqTask));

   printf("Initialization completed.\n");

   printf("Press q to quit\n");

   // Main loop
   i = 0;
   do
   {
      // Read XNET Waveform
      l_Status = nxReadSignalWaveform(m_SessionRef, 1.0, &l_StartTime,
         &l_DeltaTime, (f64 *)l_ValueBuffer, sizeof(l_ValueBuffer), &l_NumRead);
      if (nxSuccess != l_Status)
      {
         DisplayErrorAndExit(l_Status, "nxReadSignalWaveform");
      }

      // Read DAQ Wavefom
      DAQmxErrChk(DAQmxReadAnalogF64(m_DaqTask, NUM_SAMP, 10,
         DAQmx_Val_GroupByChannel, (f64 *)l_DaqValueBuffer, NUM_SAMP,
         NULL, NULL));

      // For each loop, print the StartTime of the waveform, then
      // a few selected samples for each channel.
      printf("Loop %d: StartTime = ", ++i);
      PrintTimestamp(&l_StartTime);
      printf(" DeltaTime = %f us\n", l_DeltaTime);
      printf("\tLIN Signal1:  [0] = %2.3f ... [50] = %2.3f, [99] = %2.3f\n",
         l_ValueBuffer[0][0], l_ValueBuffer[0][50], l_ValueBuffer[0][99]);
      printf("\tDAQ Signal1:  [0] = %2.3f ... [50] = %2.3f, [99] = %2.3f\n",
         l_DaqValueBuffer[0][0], l_DaqValueBuffer[0][50], l_DaqValueBuffer[0][99]);
      printf("\n");

      if (_kbhit())
      {
         l_TypedChar = _getch();
      }
   }
   while ('q' != tolower(l_TypedChar));

   // Clearing the XNET Task will disconnect all terminals
   l_Status = nxClear(m_SessionRef);
   if (l_Status != nxSuccess)
   {
      DisplayErrorAndExit(l_Status, "nxClear");
   }

   // Stop and Clear DAQ task
   DAQmxErrChk(DAQmxStopTask(m_DaqTask));
   DAQmxErrChk(DAQmxClearTask(m_DaqTask));

   printf("\n\nCompleted successfully.\n\n");

   return 0;

Error:
   if (DAQmxFailed(l_Error))
   {
      DAQmxGetExtendedErrorInfo(g_ErrBuff, ERRBUFF_SIZE);

      if (DAQmxFailed(l_Error))
      {
         printf("DAQmx Status: %s", g_ErrBuff);
         printf("\nPress any key to quit\n");
         _getch();
      }
   }

   return 1;
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

