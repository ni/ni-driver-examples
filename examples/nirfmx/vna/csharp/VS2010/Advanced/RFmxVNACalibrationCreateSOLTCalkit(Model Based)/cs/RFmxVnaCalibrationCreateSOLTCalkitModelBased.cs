//Steps:
//1.Open a new RFmx session.
//2. Create a new Calkit
//3.Define Male and Female Connectors for the Calkit
//4. Create Calibration Elements for Short calibration standard with Male and Female Connector
//5. Create Calibration Elements for Open calibration standard with Male and Female Connector
//6. Create Calibration Elements for Load calibration standard with Male and Female Connector
//7. Create Calibration Element for Thru calibration standard with Male-Male, Female-Female, and Male-Female Connectors
//8. Export the Calkit to file.
//9. Close RFmx Session.

using System;
using System.IO;
using System.Reflection;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.VnaMX;

namespace NationalInstruments.Examples.RFmxVnaCalibrationCreateSOLTCalkitModelBased
{
    public class RFmxVnaCalibrationCreateSOLTCalkitModelBased
    {
        RFmxInstrMX instrSession;
        RFmxVnaMX vna;
        string resourceName;
        string calkitID;
        string calkitSelectorString;
        string connectorSelectorString;
        string[] calibrationElementIDShort;
        string[] calibrationElementIDOpen;
        string[] calibrationElementIDLoad;
        string[] calibrationElementIDThru;
        string[] connectorID;
        string[] connectorID_Combined;
        string calElementSelectorString;
        string calkitFilePath;


        public void Run()
        {
            try
            {
                InitializeVariables();
                InitializeInstr();
                ConfigureVna();
            }
            catch (Exception ex)
            {
                DisplayError(ex);
            }
            finally
            {
                /* Close session */
                CloseSession();
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }

        void InitializeVariables()
        {
            resourceName = "VNA";
            calkitID = "Example SOLT CalKit (Model Based)";
            calkitFilePath = "..\\..\\..\\..\\..\\Support\\Example SOLT CalKit (Model Based).nckt";
            calibrationElementIDShort = new string[] { "Short-M", "Short-F" };
            calibrationElementIDOpen = new string[] { "Open-M", "Open-F" };
            calibrationElementIDLoad = new string[] { "Load-M", "Load-F" };
            calibrationElementIDThru = new string[] { "Thru-M-M", "Thru-F-F", "Thru-M-F" };
            connectorID = new string[] { "3.5mm-Male", "3.5mm-Female" };
            connectorID_Combined = new string[] { "3.5mm-Male,3.5mm-Male", "3.5mm-Female,3.5mm-Female", "3.5mm-Male,3.5mm-Female" };
        }

        void InitializeInstr()
        {
            instrSession = new RFmxInstrMX(resourceName, "");
        }

        void ConfigureVna()
        {
            vna = instrSession.GetVnaSignalConfiguration();
            vna.CalkitManagerCreateCalkit("", calkitID);
            calkitSelectorString = RFmxVnaMX.BuildCalkitString("", calkitID);
            vna.CalkitManagerCalkitSetDescription(calkitSelectorString, "Example SOLT Calkit");
            vna.CalkitManagerCalkitSetVersion(calkitSelectorString, "1.0.0");
            for (int i = 0; i < connectorID.Length; i++)
            {
                vna.CalkitManagerCalkitAddConnector(calkitSelectorString, connectorID[i]);
                connectorSelectorString = RFmxVnaMX.BuildConnectorString(calkitSelectorString, connectorID[i]);
                vna.CalkitManagerCalkitConnectorSetGender(connectorSelectorString, GetConnectorGender(i));
                vna.CalkitManagerCalkitConnectorSetType(connectorSelectorString, "3.5mm");
                vna.CalkitManagerCalkitConnectorSetDescription(connectorSelectorString, "Connector Description");
                vna.CalkitManagerCalkitConnectorSetMinimumFrequency(connectorSelectorString, 0);
                vna.CalkitManagerCalkitConnectorSetMaximumFrequency(connectorSelectorString, 100.0e9);
                vna.CalkitManagerCalkitConnectorSetImpedance(connectorSelectorString, 50);
            }


            for (int i = 0; i < connectorID.Length; i++)
            {
                vna.CalkitManagerCalkitAddCalibrationElement(calkitSelectorString, calibrationElementIDShort[i]);
                calElementSelectorString = RFmxVnaMX.BuildCalibrationElementString(calkitSelectorString, calibrationElementIDShort[i]);
                vna.CalkitManagerCalkitCalibrationElementSetTypes(calElementSelectorString, new RFmxVnaMXCalkitManagerCalkitCalibrationElementType[] {RFmxVnaMXCalkitManagerCalkitCalibrationElementType.Short});
                vna.CalkitManagerCalkitCalibrationElementSetPortConnectors(calElementSelectorString, new string[] {connectorID[i]});
                vna.CalkitManagerCalkitCalibrationElementSetDescription(calElementSelectorString, "Example Short");
                vna.CalkitManagerCalkitCalibrationElementSetMinimumFrequency(calElementSelectorString, 1.0e9);
                vna.CalkitManagerCalkitCalibrationElementSetMaximumFrequency(calElementSelectorString, 99.0e9);
                vna.CalkitManagerCalkitCalibrationElementSetSParameterDefinition(calElementSelectorString, RFmxVnaMXCalkitManagerCalkitCalibrationElementSParameterDefinition.ReflectModel);
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetModelType(calElementSelectorString, RFmxVnaMXCalkitManagerCalkitCalibrationElementReflectModelType.ReflectShort);
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetC0(calElementSelectorString, 2.0e-12);
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetC1(calElementSelectorString, 1.0e-22);
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetC2(calElementSelectorString, 2.0e-33);
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetC3(calElementSelectorString, 1.0e-44);
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetOffsetDelay(calElementSelectorString, 3.0e-11);
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetOffsetLoss(calElementSelectorString, 2.0e-9);
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetOffsetZ0(calElementSelectorString, 50);
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetSParamAvailability(calElementSelectorString, RFmxVnaMXCalkitManagerCalkitCalibrationElementReflectModelSParameterAvailability.Known);
            }
            for (int i = 0; i < connectorID.Length; i++)
            {
                vna.CalkitManagerCalkitAddCalibrationElement(calkitSelectorString, calibrationElementIDOpen[i]);
                calElementSelectorString = RFmxVnaMX.BuildCalibrationElementString(calkitSelectorString, calibrationElementIDOpen[i]);
                vna.CalkitManagerCalkitCalibrationElementSetTypes(calElementSelectorString, new RFmxVnaMXCalkitManagerCalkitCalibrationElementType[] {RFmxVnaMXCalkitManagerCalkitCalibrationElementType.Open});
                vna.CalkitManagerCalkitCalibrationElementSetPortConnectors(calElementSelectorString, new string[] {connectorID[i]});
                vna.CalkitManagerCalkitCalibrationElementSetDescription(calElementSelectorString, "Example Open");
                vna.CalkitManagerCalkitCalibrationElementSetMinimumFrequency(calElementSelectorString, 1.0e9);
                vna.CalkitManagerCalkitCalibrationElementSetMaximumFrequency(calElementSelectorString, 99.0e9);
                vna.CalkitManagerCalkitCalibrationElementSetSParameterDefinition(calElementSelectorString, RFmxVnaMXCalkitManagerCalkitCalibrationElementSParameterDefinition.ReflectModel);
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetModelType(calElementSelectorString, RFmxVnaMXCalkitManagerCalkitCalibrationElementReflectModelType.ReflectOpen);
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetC0(calElementSelectorString, 5.0e-14);
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetC1(calElementSelectorString, 3.0e-25);
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetC2(calElementSelectorString, 2.0e-35);
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetC3(calElementSelectorString, 1.0e-46);
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetOffsetDelay(calElementSelectorString, 3.0e-11);
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetOffsetLoss(calElementSelectorString, 2.0e-9);
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetOffsetZ0(calElementSelectorString, 50);
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetSParamAvailability(calElementSelectorString, RFmxVnaMXCalkitManagerCalkitCalibrationElementReflectModelSParameterAvailability.Known);
            }
            for (int i = 0; i < connectorID.Length; i++)
            {
                vna.CalkitManagerCalkitAddCalibrationElement(calkitSelectorString, calibrationElementIDLoad[i]);
                calElementSelectorString = RFmxVnaMX.BuildCalibrationElementString(calkitSelectorString, calibrationElementIDLoad[i]);
                vna.CalkitManagerCalkitCalibrationElementSetTypes(calElementSelectorString, new RFmxVnaMXCalkitManagerCalkitCalibrationElementType[] {RFmxVnaMXCalkitManagerCalkitCalibrationElementType.Load});
                vna.CalkitManagerCalkitCalibrationElementSetPortConnectors(calElementSelectorString, new string[] {connectorID[i]});
                vna.CalkitManagerCalkitCalibrationElementSetDescription(calElementSelectorString, "Example Load");
                vna.CalkitManagerCalkitCalibrationElementSetMinimumFrequency(calElementSelectorString, 1.0e9);
                vna.CalkitManagerCalkitCalibrationElementSetMaximumFrequency(calElementSelectorString, 99.0e9);
                vna.CalkitManagerCalkitCalibrationElementSetSParameterDefinition(calElementSelectorString, RFmxVnaMXCalkitManagerCalkitCalibrationElementSParameterDefinition.ReflectModel);
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetModelType(calElementSelectorString, RFmxVnaMXCalkitManagerCalkitCalibrationElementReflectModelType.Load);
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetSParamAvailability(calElementSelectorString, RFmxVnaMXCalkitManagerCalkitCalibrationElementReflectModelSParameterAvailability.Known);
            }
            for (int i = 0; i < connectorID_Combined.Length; i++)
            {
                vna.CalkitManagerCalkitAddCalibrationElement(calkitSelectorString, calibrationElementIDThru[i]);
                calElementSelectorString = RFmxVnaMX.BuildCalibrationElementString(calkitSelectorString, calibrationElementIDThru[i]);
                vna.CalkitManagerCalkitCalibrationElementSetTypes(calElementSelectorString, new RFmxVnaMXCalkitManagerCalkitCalibrationElementType[] {RFmxVnaMXCalkitManagerCalkitCalibrationElementType.Thru});
                vna.CalkitManagerCalkitCalibrationElementSetPortConnectors(calElementSelectorString, new string[] {connectorID_Combined[i]});
                vna.CalkitManagerCalkitCalibrationElementSetDescription(calElementSelectorString, "Example Thru");
                vna.CalkitManagerCalkitCalibrationElementSetMinimumFrequency(calElementSelectorString, 1.0e9);
                vna.CalkitManagerCalkitCalibrationElementSetMaximumFrequency(calElementSelectorString, 99.0e9);
                vna.CalkitManagerCalkitCalibrationElementSetSParameterDefinition(calElementSelectorString, RFmxVnaMXCalkitManagerCalkitCalibrationElementSParameterDefinition.DelayModel);
                vna.CalkitManagerCalkitCalibrationElementDelayModelSetDelay(calElementSelectorString, 1.0e-11);
            }
            vna.CalkitManagerExportCalkit("", calkitID, calkitFilePath);

        }

        private RFmxVnaMXCalkitManagerCalkitConnectorGender GetConnectorGender(int index)
        {
            switch (index)
            {
                case 0:
                    return RFmxVnaMXCalkitManagerCalkitConnectorGender.Male;
                case 1:
                    return RFmxVnaMXCalkitManagerCalkitConnectorGender.Female;
                default:
                    throw new InvalidOperationException("Invalid index");
            }
        }

        void CloseSession()
        {
            if (vna != null)
            {
                vna.Dispose();
                vna = null;
            }
            if (instrSession != null)
            {
                instrSession.Close();
                instrSession = null;
            }
        }

        static void DisplayError(Exception ex)
        {
            Console.WriteLine("ERROR:\n" + ex.GetType() + ": " + ex.Message);
        }
    }
}
