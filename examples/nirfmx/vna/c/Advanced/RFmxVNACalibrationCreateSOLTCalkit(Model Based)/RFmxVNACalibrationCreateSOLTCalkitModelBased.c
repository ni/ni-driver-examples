//Steps:
//1. Open a new RFmx session.
//2. Create a new Calkit
//3. Define Male and Female Connectors for the Calkit
//4. Create Calibration Elements for Short calibration standard with Male and Female Connector
//5. Create Calibration Elements for Open calibration standard with Male and Female Connector
//6. Create Calibration Elements for Load calibration standard with Male and Female Connector
//7. Create Calibration Element for Thru calibration standard with Male-Male, Female-Female, and Male-Female Connectors
//8. Export the Calkit to file.
//9. Close RFmx Session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxVNA.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION                4096
/* Maximum size of a selector string */
#define MAX_SELECTOR_STRING                  256
#define NUM_CONNECTORS 2
#define NUM_THRU       3

int main(int argc, char *argv[])
{
   char *resourceName = "VNA";
   niRFmxInstrHandle instrumentHandle = NULL;

   char errorMessage[MAX_ERROR_DESCRIPTION] = { 0 };
   int32 error = 0;
   int32 lastErrorCode = 0;
   
   char calkitSelectorString[MAX_SELECTOR_STRING];
   char connectorSelectorString[MAX_SELECTOR_STRING];
   char calElementSelectorString[MAX_SELECTOR_STRING];

   char* calkitID = "Example SOLT Calkit (Model Based)";
   char* calkitFilePath = "..\\..\\Support\\Example SOLT Calkit (Model Based).nckt";
   char* connectorID[NUM_CONNECTORS] = {"3.5 mm Male", "3.5 mm Female"};
   int32 connectorGender[NUM_CONNECTORS] = { RFMXVNA_VAL_CALKIT_MANAGER_CALKIT_CONNECTOR_GENDER_MALE, RFMXVNA_VAL_CALKIT_MANAGER_CALKIT_CONNECTOR_GENDER_FEMALE };
   char* calibrationElementIDShort[NUM_CONNECTORS] = {"Short-M", "Short-F"};
   char* calibrationElementIDOpen[NUM_CONNECTORS] = {"Open-M", "Open-F"};
   char* calibrationElementIDLoad[NUM_CONNECTORS] = {"Load-M", "Load-F"};
   char* connectorIDThru[NUM_THRU] = {"3.5 mm Male,3.5 mm Male", "3.5 mm Female,3.5 mm Female", "3.5 mm Male,3.5 mm Female"};
   char* calibrationElementIDThru[NUM_THRU] = {"Thru-M-M", "Thru-F-F", "Thru-M-F"};

   /* Initialize */
   RFmxCheckWarn(RFmxVNA_Initialize(resourceName, "", &instrumentHandle, NULL));
   
   /* Create Calkit */
   RFmxCheckWarn(RFmxVNA_CalkitManagerCreateCalkit(instrumentHandle, "", calkitID));
   RFmxCheckWarn(RFmxVNA_BuildCalkitString("", calkitID, MAX_SELECTOR_STRING, calkitSelectorString));
   RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitSetDescription(instrumentHandle, calkitSelectorString, "Example SOLT Calkit"));
   RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitSetVersion(instrumentHandle, calkitSelectorString, "1.0.0"));

   /* Create connectors */
   for (int i = 0; i < NUM_CONNECTORS; i++)
   {
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitAddConnector(instrumentHandle, calkitSelectorString, connectorID[i]));
      RFmxCheckWarn(RFmxVNA_BuildConnectorString(calkitSelectorString, connectorID[i], MAX_SELECTOR_STRING, connectorSelectorString));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitConnectorSetGender(instrumentHandle, connectorSelectorString, connectorGender[i]));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitConnectorSetType(instrumentHandle, connectorSelectorString, "3.5 mm"));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitConnectorSetDescription(instrumentHandle, connectorSelectorString, "Connector Description"));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitConnectorSetMinimumFrequency(instrumentHandle, connectorSelectorString, 0));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitConnectorSetMaximumFrequency(instrumentHandle, connectorSelectorString, 100.0e9));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitConnectorSetImpedance(instrumentHandle, connectorSelectorString, 50));
   }
   /* Create Short calibration standard for each connector */
   for (int i = 0; i < NUM_CONNECTORS; i++)
   {
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitAddCalibrationElement(instrumentHandle, calkitSelectorString, calibrationElementIDShort[i]));
      RFmxCheckWarn(RFmxVNA_BuildCalibrationElementString(calkitSelectorString, calibrationElementIDShort[i], MAX_SELECTOR_STRING, calElementSelectorString));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementSetTypes(instrumentHandle, calElementSelectorString, (int32[]) { RFMXVNA_VAL_CALKIT_MANAGER_CALKIT_CALIBRATION_ELEMENT_TYPE_SHORT }, 1));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementSetPortConnectors(instrumentHandle, calElementSelectorString, connectorID[i], strlen(connectorID[i])));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementSetDescription(instrumentHandle, calElementSelectorString, "Example Short"));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementSetMinimumFrequency(instrumentHandle, calElementSelectorString, 1e09));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementSetMaximumFrequency(instrumentHandle, calElementSelectorString, 99.0e9));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementSetSParameterDefinition(instrumentHandle, calElementSelectorString, RFMXVNA_VAL_CALKIT_MANAGER_CALKIT_CALIBRATION_ELEMENT_SPARAMETER_DEFINITION_REFLECT_MODEL));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementReflectModelSetModelType(instrumentHandle, calElementSelectorString, RFMXVNA_VAL_CALKIT_MANAGER_CALKIT_CALIBRATION_ELEMENT_REFLECT_MODEL_TYPE_REFLECT_SHORT));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementReflectModelSetC0(instrumentHandle, calElementSelectorString, 2e-12));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementReflectModelSetC1(instrumentHandle, calElementSelectorString, 1e-22));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementReflectModelSetC2(instrumentHandle, calElementSelectorString, 2e-33));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementReflectModelSetC3(instrumentHandle, calElementSelectorString, 1e-44));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementReflectModelSetOffsetDelay(instrumentHandle, calElementSelectorString, 3e-11));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementReflectModelSetOffsetLoss(instrumentHandle, calElementSelectorString, 2e-09));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementReflectModelSetOffsetZ0(instrumentHandle, calElementSelectorString, 50));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementReflectModelSetSParamAvailability(instrumentHandle, calElementSelectorString, RFMXVNA_VAL_CALKIT_MANAGER_CALKIT_CALIBRATION_ELEMENT_REFLECT_MODEL_SPARAMETER_AVAILABILITY_KNOWN));
   }
   /* Create Open calibration standard for each connector */
   for (int i = 0; i < NUM_CONNECTORS; i++)
   {
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitAddCalibrationElement(instrumentHandle, calkitSelectorString, calibrationElementIDOpen[i]));
      RFmxCheckWarn(RFmxVNA_BuildCalibrationElementString(calkitSelectorString, calibrationElementIDOpen[i], MAX_SELECTOR_STRING, calElementSelectorString));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementSetTypes(instrumentHandle, calElementSelectorString, (int32[]) { RFMXVNA_VAL_CALKIT_MANAGER_CALKIT_CALIBRATION_ELEMENT_TYPE_OPEN }, 1));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementSetPortConnectors(instrumentHandle, calElementSelectorString, connectorID[i], strlen(connectorID[i])));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementSetDescription(instrumentHandle, calElementSelectorString, "Example Open"));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementSetMinimumFrequency(instrumentHandle, calElementSelectorString, 1e09));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementSetMaximumFrequency(instrumentHandle, calElementSelectorString, 99.0e9));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementSetSParameterDefinition(instrumentHandle, calElementSelectorString, RFMXVNA_VAL_CALKIT_MANAGER_CALKIT_CALIBRATION_ELEMENT_SPARAMETER_DEFINITION_REFLECT_MODEL));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementReflectModelSetModelType(instrumentHandle, calElementSelectorString, RFMXVNA_VAL_CALKIT_MANAGER_CALKIT_CALIBRATION_ELEMENT_REFLECT_MODEL_TYPE_REFLECT_OPEN));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementReflectModelSetC0(instrumentHandle, calElementSelectorString, 5e-14));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementReflectModelSetC1(instrumentHandle, calElementSelectorString, 3e-25));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementReflectModelSetC2(instrumentHandle, calElementSelectorString, 2e-35));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementReflectModelSetC3(instrumentHandle, calElementSelectorString, 1e-46));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementReflectModelSetOffsetDelay(instrumentHandle, calElementSelectorString, 3e-11));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementReflectModelSetOffsetLoss(instrumentHandle, calElementSelectorString, 2e-09));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementReflectModelSetOffsetZ0(instrumentHandle, calElementSelectorString, 50));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementReflectModelSetSParamAvailability(instrumentHandle, calElementSelectorString, RFMXVNA_VAL_CALKIT_MANAGER_CALKIT_CALIBRATION_ELEMENT_REFLECT_MODEL_SPARAMETER_AVAILABILITY_KNOWN));
   }
   /* Create Load calibration standard for each connector */
   for (int i = 0; i < NUM_CONNECTORS; i++)
   {
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitAddCalibrationElement(instrumentHandle, calkitSelectorString, calibrationElementIDLoad[i]));
      RFmxCheckWarn(RFmxVNA_BuildCalibrationElementString(calkitSelectorString, calibrationElementIDLoad[i], MAX_SELECTOR_STRING, calElementSelectorString));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementSetTypes(instrumentHandle, calElementSelectorString, (int32[]) { RFMXVNA_VAL_CALKIT_MANAGER_CALKIT_CALIBRATION_ELEMENT_TYPE_LOAD }, 1));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementSetPortConnectors(instrumentHandle, calElementSelectorString, connectorID[i], strlen(connectorID[i])));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementSetDescription(instrumentHandle, calElementSelectorString, "Example Load"));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementSetMinimumFrequency(instrumentHandle, calElementSelectorString, 1e09));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementSetMaximumFrequency(instrumentHandle, calElementSelectorString, 99.0e9));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementSetSParameterDefinition(instrumentHandle, calElementSelectorString, RFMXVNA_VAL_CALKIT_MANAGER_CALKIT_CALIBRATION_ELEMENT_SPARAMETER_DEFINITION_REFLECT_MODEL));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementReflectModelSetModelType(instrumentHandle, calElementSelectorString, RFMXVNA_VAL_CALKIT_MANAGER_CALKIT_CALIBRATION_ELEMENT_REFLECT_MODEL_TYPE_LOAD));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementReflectModelSetSParamAvailability(instrumentHandle, calElementSelectorString, RFMXVNA_VAL_CALKIT_MANAGER_CALKIT_CALIBRATION_ELEMENT_REFLECT_MODEL_SPARAMETER_AVAILABILITY_KNOWN));
   }
   /* Create Thru calibration standards */
   for (int i = 0; i < NUM_THRU; i++)
   {
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitAddCalibrationElement(instrumentHandle, calkitSelectorString, calibrationElementIDThru[i]));
      RFmxCheckWarn(RFmxVNA_BuildCalibrationElementString(calkitSelectorString, calibrationElementIDThru[i], MAX_SELECTOR_STRING, calElementSelectorString));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementSetTypes(instrumentHandle, calElementSelectorString, (int32[]) { RFMXVNA_VAL_CALKIT_MANAGER_CALKIT_CALIBRATION_ELEMENT_TYPE_THRU }, 1));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementSetPortConnectors(instrumentHandle, calElementSelectorString, connectorIDThru[i], strlen(connectorIDThru[i])));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementSetDescription(instrumentHandle, calElementSelectorString, "Example Thru"));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementSetMinimumFrequency(instrumentHandle, calElementSelectorString, 1e09));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementSetMaximumFrequency(instrumentHandle, calElementSelectorString, 99.0e9));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementSetSParameterDefinition(instrumentHandle, calElementSelectorString, RFMXVNA_VAL_CALKIT_MANAGER_CALKIT_CALIBRATION_ELEMENT_SPARAMETER_DEFINITION_DELAY_MODEL));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementDelayModelSetDelay(instrumentHandle, calElementSelectorString, 1e-11));
   }

   RFmxCheckWarn(RFmxVNA_CalkitManagerExportCalkit(instrumentHandle, "", calkitID, calkitFilePath));

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

   printf("Press any key to exit\n");
   _getch();

   return error;
}