//Steps:
//1. Open a new RFmx session
//2. Configure the basic instrument properties (Clock Source and Clock Frequency)
//3. Configure Selected Ports
//4. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//5. Configure TXP measurement
//6. Configure the Measurement Interval
//7. Configure RBW filter parameters
//8. Configure Averaging parameters
//9. Create a Queue for producer-consumer setup
//10. Producer loop - Configure frequency step, Initiate Measurement, enqueue selector string and wait for the acquisiton to be complete
//11. Consumer loop - Dequeue selector string and Fetch TXP reults
//12. Release Queue
//13. Close the RFmx Session

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include <windows.h>
#include "niRFmxSpecAn.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096

#define MAX_SELECTOR_STRING_LEN     256
#define RESULT_NAME_LEN             50

#define NUM_OF_MEASUREMENTS         2

HANDLE hThread;
DWORD threadId;

typedef struct
{
   char resultNames[NUM_OF_MEASUREMENTS][MAX_SELECTOR_STRING_LEN];
   int32 frontEnd, rearEnd;
   CRITICAL_SECTION lock;

}queue_t;

queue_t queue;

void InitializeQueue(int numOfMeasurements)
{
   int i = 0;

   for (i = 0; i < numOfMeasurements; i++)
      strcpy_s(queue.resultNames[i], MAX_SELECTOR_STRING_LEN, "");

   queue.frontEnd = 0;
   queue.rearEnd = 0;
   InitializeCriticalSection(&queue.lock);
}

void AddToQueue(char *selectorString)
{
   EnterCriticalSection(&queue.lock);
   if (queue.rearEnd < NUM_OF_MEASUREMENTS)
      strcpy_s(queue.resultNames[queue.rearEnd++], MAX_SELECTOR_STRING_LEN, selectorString);

   LeaveCriticalSection(&queue.lock);
}

char dequeueElement[MAX_SELECTOR_STRING_LEN] = { '\0' };

char * GetNextResultName()
{
   int32 isQueueEmpty = 0; //Assuming queue is non empty
   int32 timeout = 10000;  //10 seconds

   do
   {
      EnterCriticalSection(&queue.lock);
      isQueueEmpty = queue.frontEnd == queue.rearEnd;
      LeaveCriticalSection(&queue.lock);
      if (isQueueEmpty)
      {
         Sleep(10);
         timeout -= 10;
      }
   } while (isQueueEmpty && timeout);

   EnterCriticalSection(&queue.lock);
   strcpy_s(dequeueElement, MAX_SELECTOR_STRING_LEN, queue.resultNames[queue.frontEnd]);
   strcpy_s(queue.resultNames[queue.frontEnd], MAX_SELECTOR_STRING_LEN, "");
   queue.frontEnd++;
   LeaveCriticalSection(&queue.lock);

   return dequeueElement;
}

char *selectedPorts = "";
float64 centerFrequency = 1e+9;              /* Hz */
int32 errorFlag = RFMXSPECAN_VAL_FALSE;      /* flag to check the error condition */

DWORD WINAPI CfgAndInitiate(LPVOID lpParam)
{
   niRFmxInstrHandle instrumentHandle = (niRFmxInstrHandle)(lpParam);
   int32 error = 0, lastErrorCode = 0;
   char errorMessage[MAX_ERROR_DESCRIPTION];
   float64 frequencyOffset = 0.0;
   char resultName[RESULT_NAME_LEN] = { '\0' };
   char suffix[5] = { '\0' };
   char selectorString[MAX_SELECTOR_STRING_LEN] = { '\0' };
   int i = 0;

   for (i = 0; i < NUM_OF_MEASUREMENTS; i++)
   {
      frequencyOffset = i * 1e6;
      RFmxCheckWarn(RFmxSpecAn_CfgFrequency(instrumentHandle, "", centerFrequency + frequencyOffset));

      strcpy_s(resultName, RESULT_NAME_LEN, "TXP_Result");
      _itoa_s(i, suffix, 5, 10);
      strcat_s(resultName, RESULT_NAME_LEN, suffix);

      RFmxSpecAn_BuildSignalString("", resultName, MAX_SELECTOR_STRING_LEN, selectorString);
      RFmxCheckWarn(RFmxSpecAn_Initiate(instrumentHandle, selectorString, ""));
      AddToQueue(selectorString);
      RFmxCheckWarn(RFmxSpecAn_WaitForAcquisitionComplete(instrumentHandle, 10));
   }
Error:
   if (error)
   {
      errorFlag = RFMXSPECAN_VAL_TRUE;
      RFmxSpecAn_GetError(NULL, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
      if (error < 0)
         printf("Thread ID : %d, ERROR: %s\n", GetCurrentThreadId(), errorMessage);
      else
         printf("Thread ID : %d, WARNING: %s\n", GetCurrentThreadId(), errorMessage);
   }
   return error;
}

/* Arrays to store results */
float64 minPower[NUM_OF_MEASUREMENTS] = { 0.0 };         /* dBm */
float64 avgMeanPower[NUM_OF_MEASUREMENTS] = { 0.0 };     /* dBm */
float64 peakToAvgPower[NUM_OF_MEASUREMENTS] = { 0.0 };   /* dB */
float64 maxPower[NUM_OF_MEASUREMENTS] = { 0.0 };         /* dBm */

DWORD WINAPI Fetch(LPVOID lpParam)
{
   niRFmxInstrHandle instrumentHandle = (niRFmxInstrHandle)(lpParam);
   int32 error = 0, lastErrorCode = 0;
   char errorMessage[MAX_ERROR_DESCRIPTION];
   char selectorString[MAX_SELECTOR_STRING_LEN] = { '\0' };
   int i = 0;

   for (i = 0; i < NUM_OF_MEASUREMENTS; i++)
   {
      strcpy_s(selectorString, MAX_SELECTOR_STRING_LEN, GetNextResultName());
      RFmxCheckWarn(RFmxSpecAn_TXPFetchMeasurement(instrumentHandle, selectorString, 10.0,
         &avgMeanPower[i], &peakToAvgPower[i], &maxPower[i], &minPower[i]));
   }
Error:
   if (error)
   {
      errorFlag = RFMXSPECAN_VAL_TRUE;
      RFmxSpecAn_GetError(NULL, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
      if (error < 0)
         printf("Thread ID : %d, ERROR: %s\n", GetCurrentThreadId(), errorMessage);
      else
         printf("Thread ID : %d, WARNING: %s\n", GetCurrentThreadId(), errorMessage);
   }
   return error;
}

int main(int argc, char *argv[])
{
   niRFmxInstrHandle instrumentHandle = NULL;
   int32 error = 0, lastErrorCode = 0, i = 0;
   char errorMessage[MAX_ERROR_DESCRIPTION];

   char *resourceName = "RFSA";
   char *frequencySource = RFMXINSTR_VAL_ONBOARD_CLOCK_STR;
   float64 referenceLevel = 0.00;            /* dBm */
   float64 externalAttenuation = 0.00;       /* dB */
   float64 frequency = 10.0e+6;              /* Hz */


   float64 measInterval = 1e-3;              /* seconds */

   /* RBW Filter */
   int32 RBWFilterType = RFMXSPECAN_VAL_TXP_RBW_FILTER_TYPE_GAUSSIAN;
   float64 RBW = 100e+3;                     /* Hz */
   float64 RRCAlpha = 0.010;

   /* Averaging */
   int32 averagingEnabled = RFMXSPECAN_VAL_TXP_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;
   int32 averagingType = RFMXSPECAN_VAL_TXP_AVERAGING_TYPE_RMS;

   /* Create a new RFmx Session */
   RFmxCheckWarn(RFmxSpecAn_Initialize(resourceName, "", &instrumentHandle, NULL));

   /* Configure TXP parameters */
   RFmxCheckWarn(RFmxSpecAn_CfgFrequencyReference(instrumentHandle, "", frequencySource, frequency));
   RFmxCheckWarn(RFmxSpecAn_SetSelectedPorts(instrumentHandle, "", selectedPorts));
   RFmxCheckWarn(RFmxSpecAn_CfgFrequency(instrumentHandle, "", centerFrequency));
   RFmxCheckWarn(RFmxSpecAn_CfgReferenceLevel(instrumentHandle, "", referenceLevel));
   RFmxCheckWarn(RFmxSpecAn_CfgExternalAttenuation(instrumentHandle, "", externalAttenuation));
   RFmxCheckWarn(RFmxSpecAn_SelectMeasurements(instrumentHandle, "", RFMXSPECAN_VAL_TXP, RFMXSPECAN_VAL_FALSE));
   RFmxCheckWarn(RFmxSpecAn_TXPCfgMeasurementInterval(instrumentHandle, "", measInterval));
   RFmxCheckWarn(RFmxSpecAn_TXPCfgRBWFilter(instrumentHandle, "", RBW, RBWFilterType, RRCAlpha));
   RFmxCheckWarn(RFmxSpecAn_TXPCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));

   InitializeQueue(NUM_OF_MEASUREMENTS);

   hThread = CreateThread(NULL, 0, Fetch, instrumentHandle, 0, &threadId);//Create a new thread for Consumer loop

   CfgAndInitiate(instrumentHandle);//Start Producer loop

   WaitForSingleObject(hThread, INFINITE);

   if (!errorFlag)
   {
      /* Display results */
      for (i = 0; i < NUM_OF_MEASUREMENTS; i++)
      {
         printf("--------Measurement %d--------\n", i + 1);
         printf("Average Mean Power(dBm)   %f\n", avgMeanPower[i]);
         printf("Peak to Average Ratio(dB) %f\n", peakToAvgPower[i]);
         printf("Maximum Power(dBm)        %f\n", maxPower[i]);
         printf("Minimum Power(dBm)        %f\n\n", minPower[i]);
      }
   }

Error:
   if (error)
   {
      RFmxSpecAn_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
      if (error < 0)
         printf("ERROR: %s\n", errorMessage);
      else
         printf("WARNING: %s\n", errorMessage);
   }
   if (hThread)
      CloseHandle(hThread);
   if (instrumentHandle)
   {
      RFmxSpecAn_Close(instrumentHandle, RFMXSPECAN_VAL_FALSE);
   }

   printf("Press any key to exit\n");
   _getch();

   return error;
}
