//Steps:
//1. Open a new RFmx session.
//2. Create a new Calkit
//3. Define Male and Female Connectors for the Calkit
//4. Create Calibration Elements for One-Port standards
//5. Create Calibration Elements for Two-Port Thru's
//6. Export the Calkit to file.
//7. Close RFmx Session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxVNA.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION                4096
/* Maximum size of a selector string */
#define MAX_SELECTOR_STRING                  256
#define NUM_CONNECTORS                       2

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

   char* calkitID = "Example SOLT Calkit (SnP Data Based)";
   char* calkitFilePath = "..\\..\\Support\\Example SOLT Calkit (SnP Data Based).nckt";
   char* connectorID[NUM_CONNECTORS] = {"3.5 mm Male", "3.5 mm Female"};
   int32 connectorGender[NUM_CONNECTORS] = { RFMXVNA_VAL_CALKIT_MANAGER_CALKIT_CONNECTOR_GENDER_MALE, RFMXVNA_VAL_CALKIT_MANAGER_CALKIT_CONNECTOR_GENDER_FEMALE };
   
   struct CalibrationStandards
   {
      char* elementID;
      char* connectorID;
      char* fileName;
   };
   struct CalibrationStandards onePortCalibrationStandards[] = {
      {"Short-M",       "3.5 mm Male",   "..\\..\\Support\\Short(m).s1p"},
      {"Open-M",        "3.5 mm Male",   "..\\..\\Support\\Open(m).s1p"},
      {"Load-M",        "3.5 mm Male",   "..\\..\\Support\\Load(m).s1p"},
      {"OffsetShort-M", "3.5 mm Male",   "..\\..\\Support\\OffsetShort(m).s1p"},
      {"Short-F",       "3.5 mm Female", "..\\..\\Support\\Short(f).s1p"},
      {"Open-F",        "3.5 mm Female", "..\\..\\Support\\Open(f).s1p"},
      {"Load-F",        "3.5 mm Female", "..\\..\\Support\\Load(f).s1p"},
      {"OffsetShort-F", "3.5 mm Female", "..\\..\\Support\\OffsetShort(f).s1p"}
   };
   struct CalibrationStandards twoPortCalibrationStandards[] = {
      {"Thru-M-M", "3.5 mm Male,3.5 mm Male",     "..\\..\\Support\\Thru(m-m).s2p"},
      {"Thru-F-F", "3.5 mm Female,3.5 mm Female", "..\\..\\Support\\Thru(f-f).s2p"}
   };

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
   /* Create 1-port standards */
   for (int i = 0; i < sizeof(onePortCalibrationStandards)/sizeof(onePortCalibrationStandards[0]); i++)
   {
      struct CalibrationStandards* standard = &onePortCalibrationStandards[i];
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitAddCalibrationElement(instrumentHandle, calkitSelectorString, (*standard).elementID));
      RFmxCheckWarn(RFmxVNA_BuildCalibrationElementString(calkitSelectorString, (*standard).elementID, MAX_SELECTOR_STRING, calElementSelectorString));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementSetTypes(instrumentHandle, calElementSelectorString, (int32[]) { RFMXVNA_VAL_CALKIT_MANAGER_CALKIT_CALIBRATION_ELEMENT_TYPE_TERMINATION }, 1));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementSetPortConnectors(instrumentHandle, calElementSelectorString, (*standard).connectorID, strlen((*standard).connectorID)));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementSetDescription(instrumentHandle, calElementSelectorString, (*standard).elementID));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementSetMinimumFrequency(instrumentHandle, calElementSelectorString, 1e09));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementSetMaximumFrequency(instrumentHandle, calElementSelectorString, 26.0e9));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementSetSParameterDefinition(instrumentHandle, calElementSelectorString, RFMXVNA_VAL_CALKIT_MANAGER_CALKIT_CALIBRATION_ELEMENT_SPARAMETER_DEFINITION_SPARAMETER));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementReflectModelSetSParamAvailability(instrumentHandle, calElementSelectorString, RFMXVNA_VAL_CALKIT_MANAGER_CALKIT_CALIBRATION_ELEMENT_REFLECT_MODEL_SPARAMETER_AVAILABILITY_KNOWN));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementSParameterSetFromFile(instrumentHandle, calElementSelectorString, (*standard).fileName));
   }
   /* Create Thru standards */
   for (int i = 0; i < sizeof(twoPortCalibrationStandards) / sizeof(twoPortCalibrationStandards[0]); i++)
   {
      struct CalibrationStandards* standard = &twoPortCalibrationStandards[i];
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitAddCalibrationElement(instrumentHandle, calkitSelectorString, (*standard).elementID));
      RFmxCheckWarn(RFmxVNA_BuildCalibrationElementString(calkitSelectorString, (*standard).elementID, MAX_SELECTOR_STRING, calElementSelectorString));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementSetTypes(instrumentHandle, calElementSelectorString, (int32[]) { RFMXVNA_VAL_CALKIT_MANAGER_CALKIT_CALIBRATION_ELEMENT_TYPE_THRU }, 1));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementSetPortConnectors(instrumentHandle, calElementSelectorString, (*standard).connectorID, strlen((*standard).connectorID)));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementSetDescription(instrumentHandle, calElementSelectorString, (*standard).elementID));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementSetMinimumFrequency(instrumentHandle, calElementSelectorString, 1e09));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementSetMaximumFrequency(instrumentHandle, calElementSelectorString, 26.0e9));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementSetSParameterDefinition(instrumentHandle, calElementSelectorString, RFMXVNA_VAL_CALKIT_MANAGER_CALKIT_CALIBRATION_ELEMENT_SPARAMETER_DEFINITION_SPARAMETER));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementReflectModelSetSParamAvailability(instrumentHandle, calElementSelectorString, RFMXVNA_VAL_CALKIT_MANAGER_CALKIT_CALIBRATION_ELEMENT_REFLECT_MODEL_SPARAMETER_AVAILABILITY_KNOWN));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementSParameterSetFromFile(instrumentHandle, calElementSelectorString, (*standard).fileName));
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