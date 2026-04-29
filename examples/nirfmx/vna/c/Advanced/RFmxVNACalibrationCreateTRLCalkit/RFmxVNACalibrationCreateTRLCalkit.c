//Steps:
//1. Open a new RFmx session.
//2. Create a new Calkit.
//3. Define Male and Female Connectors for the Calkit.
//4. Create Calibration Elements for Reflect calibration standard with Male and Female Connector.
//5. Create Calibration Element for Thru calibration standard with Male-Female Connectors.
//6. Create Calibration Elements for Line calibration standards with Male-Female Connectors.
//7. Configure TRL Options.
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
#define NUM_LINES      2

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

   char* calkitID = "Example TRL Calkit";
   char* calkitFilePath = "..\\..\\Support\\Example TRL Calkit.nckt";
   char* connectorID[NUM_CONNECTORS] = {"3.5 mm Male", "3.5 mm Female"};
   int32 connectorGender[NUM_CONNECTORS] = { RFMXVNA_VAL_CALKIT_MANAGER_CALKIT_CONNECTOR_GENDER_MALE, RFMXVNA_VAL_CALKIT_MANAGER_CALKIT_CONNECTOR_GENDER_FEMALE };
   char* calibrationElementIDReflect[NUM_CONNECTORS] = {"Reflect-M", "Reflect-F"};
   char* connectorIDTwoPort = "3.5 mm Male,3.5 mm Female";
   char* calibrationElementIDThru = "Insertable Thru";
   char* calibrationElementIDLine[NUM_LINES] = {"Line 1", "Line 2"};
   float64 calibrationElementLineDelaySec[NUM_LINES] = {55e-12, 15e-12};
   float64 calibrationElementLineMinimumFrequencyHz[NUM_LINES] = {2e09, 7e09};
   float64 calibrationElementLineMaximumFrequencyHz[NUM_LINES] = {7e09, 26.5e09};

   /* Initialize */
   RFmxCheckWarn(RFmxVNA_Initialize(resourceName, "", &instrumentHandle, NULL));
   
   /* Create Calkit */
   RFmxCheckWarn(RFmxVNA_CalkitManagerCreateCalkit(instrumentHandle, "", calkitID));
   RFmxCheckWarn(RFmxVNA_BuildCalkitString("", calkitID, MAX_SELECTOR_STRING, calkitSelectorString));
   RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitSetDescription(instrumentHandle, calkitSelectorString, "Example TRL Calkit"));
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
   /* Create Reflect calibration standard for each connector */
   for (int i = 0; i < NUM_CONNECTORS; i++)
   {
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitAddCalibrationElement(instrumentHandle, calkitSelectorString, calibrationElementIDReflect[i]));
      RFmxCheckWarn(RFmxVNA_BuildCalibrationElementString(calkitSelectorString, calibrationElementIDReflect[i], MAX_SELECTOR_STRING, calElementSelectorString));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementSetTypes(instrumentHandle, calElementSelectorString, (int32[]) { RFMXVNA_VAL_CALKIT_MANAGER_CALKIT_CALIBRATION_ELEMENT_TYPE_REFLECT }, 1));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementSetPortConnectors(instrumentHandle, calElementSelectorString, connectorID[i], strlen(connectorID[i])));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementSetDescription(instrumentHandle, calElementSelectorString, "Reflect (Short)"));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementSetMinimumFrequency(instrumentHandle, calElementSelectorString, 0));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementSetMaximumFrequency(instrumentHandle, calElementSelectorString, 100.0e9));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementSetSParameterDefinition(instrumentHandle, calElementSelectorString, RFMXVNA_VAL_CALKIT_MANAGER_CALKIT_CALIBRATION_ELEMENT_SPARAMETER_DEFINITION_REFLECT_MODEL));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementReflectModelSetModelType(instrumentHandle, calElementSelectorString, RFMXVNA_VAL_CALKIT_MANAGER_CALKIT_CALIBRATION_ELEMENT_REFLECT_MODEL_TYPE_REFLECT_SHORT));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementReflectModelSetC0(instrumentHandle, calElementSelectorString, 0));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementReflectModelSetC1(instrumentHandle, calElementSelectorString, 0));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementReflectModelSetC2(instrumentHandle, calElementSelectorString, 0));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementReflectModelSetC3(instrumentHandle, calElementSelectorString, 0));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementReflectModelSetOffsetDelay(instrumentHandle, calElementSelectorString, 0));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementReflectModelSetOffsetLoss(instrumentHandle, calElementSelectorString, 0));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementReflectModelSetOffsetZ0(instrumentHandle, calElementSelectorString, 50));
      RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementReflectModelSetSParamAvailability(instrumentHandle, calElementSelectorString, RFMXVNA_VAL_CALKIT_MANAGER_CALKIT_CALIBRATION_ELEMENT_REFLECT_MODEL_SPARAMETER_AVAILABILITY_ESTIMATE));
   }
   /* Create Thru calibration standard */
   RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitAddCalibrationElement(instrumentHandle, calkitSelectorString, calibrationElementIDThru));
   RFmxCheckWarn(RFmxVNA_BuildCalibrationElementString(calkitSelectorString, calibrationElementIDThru, MAX_SELECTOR_STRING, calElementSelectorString));
   RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementSetTypes(instrumentHandle, calElementSelectorString, (int32[]) { RFMXVNA_VAL_CALKIT_MANAGER_CALKIT_CALIBRATION_ELEMENT_TYPE_THRU }, 1));
   RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementSetPortConnectors(instrumentHandle, calElementSelectorString, connectorIDTwoPort, strlen(connectorIDTwoPort)));
   RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementSetDescription(instrumentHandle, calElementSelectorString, "Insertable Thru"));
   RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementSetMinimumFrequency(instrumentHandle, calElementSelectorString, 0));
   RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementSetMaximumFrequency(instrumentHandle, calElementSelectorString, 100.0e9));
   RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementSetSParameterDefinition(instrumentHandle, calElementSelectorString, RFMXVNA_VAL_CALKIT_MANAGER_CALKIT_CALIBRATION_ELEMENT_SPARAMETER_DEFINITION_DELAY_MODEL));
   RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementDelayModelSetDelay(instrumentHandle, calElementSelectorString, 0));
   /* Create Line standards */
   for (int i = 0; i < NUM_LINES; i++)
   {
       RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitAddCalibrationElement(instrumentHandle, calkitSelectorString, calibrationElementIDLine[i]));
       RFmxCheckWarn(RFmxVNA_BuildCalibrationElementString(calkitSelectorString, calibrationElementIDLine[i], MAX_SELECTOR_STRING, calElementSelectorString));
       RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementSetTypes(instrumentHandle, calElementSelectorString, (int32[]) { RFMXVNA_VAL_CALKIT_MANAGER_CALKIT_CALIBRATION_ELEMENT_TYPE_LINE }, 1));
       RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementSetPortConnectors(instrumentHandle, calElementSelectorString, connectorIDTwoPort, strlen(connectorIDTwoPort)));
       RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementSetDescription(instrumentHandle, calElementSelectorString, calibrationElementIDLine[i]));
       RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementSetMinimumFrequency(instrumentHandle, calElementSelectorString, calibrationElementLineMinimumFrequencyHz[i]));
       RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementSetMaximumFrequency(instrumentHandle, calElementSelectorString, calibrationElementLineMaximumFrequencyHz[i]));
       RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementSetSParameterDefinition(instrumentHandle, calElementSelectorString, RFMXVNA_VAL_CALKIT_MANAGER_CALKIT_CALIBRATION_ELEMENT_SPARAMETER_DEFINITION_DELAY_MODEL));
       RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitCalibrationElementDelayModelSetDelay(instrumentHandle, calElementSelectorString, calibrationElementLineDelaySec[i]));
   }
   /* TRL Options */
   RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitSetTRLReferencePlane(instrumentHandle, calkitSelectorString, RFMXVNA_VAL_CALKIT_MANAGER_CALKIT_TRL_REFERENCE_PLANE_THRU));
   RFmxCheckWarn(RFmxVNA_CalkitManagerCalkitSetLRLLineAutoChar(instrumentHandle, calkitSelectorString, 0));

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