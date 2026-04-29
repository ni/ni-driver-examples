//Steps:
//1. Open a new RFmx session.
//2. Configure Frequency Reference.
//3. Configure sweep settings: Sweep Type, Start Frequency, Stop Frequency, Number of Frequency Points, IF Bandwidth, and Power Level and Test Rx Attenuation with different port names.
//4. Select S-Parameter measurement. 
//5. Configure Calibration Ports and Calibration Method.
//6. Configure Connector type & vCal Resource Name for each VNA port.
//7. Detect vCal ports connected to the VNA ports
//8. Initiate Calibration.
//9. Acquire Calibration data after user confirmation.
//10. Save Calibration data.
//11. Save Calset data to a file.
//12. Get Calset Error Terms.
//12a. Calculate Magnitude of Error Terms
//13. Close RFmx Session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include <math.h>

#include "niRFmxVNA.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION                4096
/* Maximum size of a selector string */
#define MAX_SELECTOR_STRING                  256
#define NUMBER_OF_SPARAMS                    4
#define ERROR_TERM_IDENTIFIERS               5
#define AUTO_DETECT_OFF                      0
#define AUTO_DETECT_ON                       1

void concatenateArrays(NIComplexSingle arr1[], int len1, NIComplexSingle arr2[], int len2, NIComplexSingle result[])
{
    // Copy the first array into result
    for (int i = 0; i < len1; i++) {
        result[i] = arr1[i];
    }

    // Copy the second array into result
    for (int i = 0; i < len2; i++) {
        result[len1 + i] = arr2[i];
    }
}

int main(int argc, char *argv[])
{
   char *resourceName = "VNA";
   niRFmxInstrHandle instrumentHandle = NULL;

   char errorMessage[MAX_ERROR_DESCRIPTION] = { 0 };
   int32 error = 0;
   int32 lastErrorCode = 0;

   int32 actualArraySize = 0;
   int32 errorTermPort1ArraySize = 0;
   int32 errorTermPort2ArraySize = 0;
   float64 frequencyStart = 1.0e9;                                                                       /* (Hz) */
   float64 frequencyEnd = 26.0e9;                                                                        /* (Hz) */
   int32 numberOfFrequencyPoints = 251;
   float64 port1PowerLevel = -10.0;                                                                      /* (dBm) */
   float64 port2PowerLevel = -10.0;                                                                      /* (dBm) */
   float64 port1TestReceiverAttenuation = 0.0;                                                           /* (dB) */
   float64 port2TestReceiverAttenuation = 0.0;                                                           /* (dB) */
   float64 IFBandwidth = 100.0e3;                                                                        /* (Hz) */

   char *frequencyReferenceSource = RFMXVNA_VAL_PXI_CLK_STR;
   float64 frequencyReferenceFrequency = 100.0e6;                                                        /* (Hz) */

   char *vCalResourceName = "vCal";
   char *connectorType = "3.5 mm female";
   float64 calibrationTimeout = 100.0;                                                                   /*(seconds) */
   int32 autoDetectvCalOrientation = AUTO_DETECT_ON;
   char vCalOrientation[MAX_SELECTOR_STRING] = "portA:port1,portB:port2";
   
   int32 errorTermIdentifier[ERROR_TERM_IDENTIFIERS] = {
       RFMXVNA_VAL_CAL_ERROR_TERM_DIRECTIVITY,
       RFMXVNA_VAL_CAL_ERROR_TERM_SOURCE_MATCH,
       RFMXVNA_VAL_CAL_ERROR_TERM_REFLECTION_TRACKING,
       RFMXVNA_VAL_CAL_ERROR_TERM_TRANSMISSION_TRACKING,
       RFMXVNA_VAL_CAL_ERROR_TERM_LOAD_MATCH};

   char* calsetFilePath = NULL;
   float64* frequencyGridResult = NULL;
   NIComplexSingle* errorTermPort1Data = NULL;
   NIComplexSingle* errorTermPort2Data = NULL;
   NIComplexSingle* errorTermData = NULL;
   float64 calculateMagnitude;

   char portSelectorString[MAX_SELECTOR_STRING];

   int32 index = 0;

   float64 timeout = 10.0;                                                                               /*(seconds) */

   /* Initialize a session */
   RFmxCheckWarn(RFmxVNA_Initialize(resourceName, "", &instrumentHandle, NULL));

   /* Configure the session */
   RFmxCheckWarn(RFmxVNA_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, frequencyReferenceFrequency));
   RFmxCheckWarn(RFmxVNA_SetSweepType(instrumentHandle, "", RFMXVNA_VAL_SWEEP_TYPE_LINEAR));
   RFmxCheckWarn(RFmxVNA_SetStartFrequency(instrumentHandle, "", frequencyStart));
   RFmxCheckWarn(RFmxVNA_SetStopFrequency(instrumentHandle, "", frequencyEnd));
   RFmxCheckWarn(RFmxVNA_SetNumberOfPoints(instrumentHandle, "", numberOfFrequencyPoints));
   RFmxCheckWarn(RFmxVNA_SetIFBandwidth(instrumentHandle, "", IFBandwidth));
   RFmxCheckWarn(RFmxVNA_BuildPortString("", "port1", MAX_SELECTOR_STRING, portSelectorString));
   RFmxCheckWarn(RFmxVNA_SetPowerLevel(instrumentHandle, portSelectorString, port1PowerLevel));
   RFmxCheckWarn(RFmxVNA_SetTestReceiverAttenuation(instrumentHandle, portSelectorString, port1TestReceiverAttenuation));
   RFmxCheckWarn(RFmxVNA_BuildPortString("", "port2", MAX_SELECTOR_STRING, portSelectorString));
   RFmxCheckWarn(RFmxVNA_SetPowerLevel(instrumentHandle, portSelectorString, port2PowerLevel));
   RFmxCheckWarn(RFmxVNA_SetTestReceiverAttenuation(instrumentHandle, portSelectorString, port2TestReceiverAttenuation));
   RFmxCheckWarn(RFmxVNA_SelectMeasurements(instrumentHandle, "", RFMXVNA_VAL_SPARAMS, RFMXVNA_VAL_FALSE));

   RFmxCheckWarn(RFmxVNA_SetCorrectionCalibrationPorts(instrumentHandle, "", "port1,port2"));
   RFmxCheckWarn(RFmxVNA_SetCorrectionCalibrationMethod(instrumentHandle, "", RFMXVNA_VAL_CORRECTION_CALIBRATION_METHOD_SOLT));

   RFmxCheckWarn(RFmxVNA_SetCorrectionCalibrationConnectorType(instrumentHandle, "port::all", connectorType));
   RFmxCheckWarn(RFmxVNA_SetCorrectionCalibrationCalkitElectronicResourceName(instrumentHandle, "port::all", vCalResourceName));

   if (autoDetectvCalOrientation)
   {
       RFmxCheckWarn(RFmxVNA_AutoDetectvCalOrientation(instrumentHandle, ""));
       RFmxCheckWarn(RFmxVNA_GetCorrectionCalibrationCalkitElectronicOrientation(instrumentHandle, "", MAX_SELECTOR_STRING, vCalOrientation));
       printf("vCal Orientation                        : %s\n", vCalOrientation);
   }
   else
   {
       RFmxCheckWarn(RFmxVNA_SetCorrectionCalibrationCalkitElectronicOrientation(instrumentHandle, "", vCalOrientation));
   }
   
   RFmxCheckWarn(RFmxVNA_CalibrationInitiate(instrumentHandle, ""));
   RFmxCheckWarn(RFmxVNA_CalibrationAcquire(instrumentHandle, "", calibrationTimeout));
   RFmxCheckWarn(RFmxVNA_CalibrationSave(instrumentHandle, "", ""));
   _getch();

   RFmxCheckWarn(RFmxVNA_CalsetSaveToFile(instrumentHandle, "", "", calsetFilePath));

   RFmxCheckWarn(RFmxVNA_CalsetGetFrequencyGrid(instrumentHandle, "", "", RFMXVNA_VAL_CAL_ERROR_TERM_DIRECTIVITY, NULL, 0, &actualArraySize));
   if (actualArraySize > 0) {
       frequencyGridResult = (float64*)malloc(sizeof(float64) * actualArraySize);
       if (frequencyGridResult) {
           RFmxCheckWarn(RFmxVNA_CalsetGetFrequencyGrid(instrumentHandle, "", "", RFMXVNA_VAL_CAL_ERROR_TERM_DIRECTIVITY, frequencyGridResult, actualArraySize, NULL));
       } else {
           printf("malloc failed.\n");
           goto Error;
       }
   }
   for (int i = 0; i < ERROR_TERM_IDENTIFIERS; i++) {
       errorTermPort1ArraySize = 0;
       RFmxCheckWarn(RFmxVNA_CalsetGetErrorTerm(instrumentHandle, "", "", errorTermIdentifier[i], "port1", "port2", NULL, 0, &errorTermPort1ArraySize));
       if (errorTermPort1ArraySize > 0) {
           errorTermPort1Data = (NIComplexSingle*)malloc(sizeof(NIComplexSingle) * errorTermPort1ArraySize);
           if (errorTermPort1Data) {
               RFmxCheckWarn(RFmxVNA_CalsetGetErrorTerm(instrumentHandle, "", "", errorTermIdentifier[i], "port1", "port2", errorTermPort1Data, errorTermPort1ArraySize, NULL));
           } else {
               printf("malloc failed.\n");
               goto Error;
           }
       }
       errorTermPort2ArraySize = 0;
       RFmxCheckWarn(RFmxVNA_CalsetGetErrorTerm(instrumentHandle, "", "", errorTermIdentifier[i], "port2", "port1", NULL, 0, &errorTermPort2ArraySize));
       if (errorTermPort2ArraySize > 0) {
           errorTermPort2Data = (NIComplexSingle*)malloc(sizeof(NIComplexSingle) * errorTermPort2ArraySize);
           if (errorTermPort2Data) {
               RFmxCheckWarn(RFmxVNA_CalsetGetErrorTerm(instrumentHandle, "", "", errorTermIdentifier[i], "port2", "port1", errorTermPort2Data, errorTermPort2ArraySize, NULL));
           } else {
               printf("malloc failed.\n");
               goto Error;
           }
       }
       actualArraySize = errorTermPort1ArraySize + errorTermPort2ArraySize;
       errorTermData = (NIComplexSingle*)malloc(sizeof(NIComplexSingle) * actualArraySize);
       concatenateArrays(errorTermPort1Data, errorTermPort1ArraySize, errorTermPort2Data, errorTermPort1ArraySize, errorTermData);
       calculateMagnitude = (20.0 * log10(sqrt(errorTermData->real * errorTermData->real + errorTermData->imaginary * errorTermData->imaginary)));
   }
  
   Error:
   if (error)
   {
      RFmxVNA_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
      if (error < 0)
         printf("ERROR: %s\n", errorMessage);
      else
         printf("WARNING: %s %d\n", errorMessage, error);
   }

   if (instrumentHandle)
   {
      RFmxVNA_Close(instrumentHandle, RFMXVNA_VAL_FALSE);
   }

   /* Free allocated memory */
   if (frequencyGridResult) {
       free(frequencyGridResult);
   }
   if (errorTermPort1Data) {
       free(errorTermPort1Data);
   }
   if (errorTermPort2Data) {
       free(errorTermPort2Data);
   }
   if (errorTermData) {
       free(errorTermData);
   }

   printf("Press any key to exit\n");
   _getch();

   return error;
}