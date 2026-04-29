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

using System;
using System.IO;
using System.Reflection;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.VnaMX;

namespace NationalInstruments.Examples.RFmxVNACalibrationCreateTRLCalkit
{
    public class RFmxVNACalibrationCreateTRLCalkit
    {
        RFmxInstrMX instrSession;
        RFmxVnaMX vna;
        string resourceName;
        string calkitID;
        string[] connectorID;
        string calkitSelectorString;
        string connectorSelectorString;
        string calElementSelectorString;
        string[] calibrationElementIDReflect;
        string[] connectorIDTwoPort;
        string calibrationElementIDThru;
        string[] calibrationElementIDLine;
        double[] calibrationElementLineDelaySec;
        double[] calibrationElementLineMinimumFrequencyHz;
        double[] calibrationElementLineMaximumFrequencyHz;
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
            calkitID = "Example TRL Calkit";
            calkitFilePath = "..\\..\\..\\..\\..\\Support\\Example TRL Calkit.nckt";
            connectorID = new string[] { "3.5 mm Male", "3.5 mm Female" };
            calibrationElementIDReflect = new string[] { "Reflect-M", "Reflect-F" };
            connectorIDTwoPort = new string[] { connectorID[0], connectorID[1] };
            calibrationElementIDThru = "Insertable Thru";
            calibrationElementIDLine = new string[] { "Line 1", "Line 2" };
            calibrationElementLineDelaySec = new double[] { 55e-12, 15e-12 };
            calibrationElementLineMinimumFrequencyHz = new double[] { 2e09, 7e09 };
            calibrationElementLineMaximumFrequencyHz = new double[] { 7e09, 26.5e09 };
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
            vna.CalkitManagerCalkitSetDescription(calkitSelectorString, "Example TRL Calkit");
            vna.CalkitManagerCalkitSetVersion(calkitSelectorString, "1.0.0");
            /* Create connectors */
            for (int i = 0; i < connectorID.Length; i++)
            {
                vna.CalkitManagerCalkitAddConnector(calkitSelectorString, connectorID[i]);
                connectorSelectorString = RFmxVnaMX.BuildConnectorString(calkitSelectorString, connectorID[i]);
                vna.CalkitManagerCalkitConnectorSetGender(connectorSelectorString, GetConnectorGender(i));
                vna.CalkitManagerCalkitConnectorSetType(connectorSelectorString, "3.5 mm");
                vna.CalkitManagerCalkitConnectorSetDescription(connectorSelectorString, "Connector Description");
                vna.CalkitManagerCalkitConnectorSetMinimumFrequency(connectorSelectorString, 0);
                vna.CalkitManagerCalkitConnectorSetMaximumFrequency(connectorSelectorString, 100.0e9);
                vna.CalkitManagerCalkitConnectorSetImpedance(connectorSelectorString, 50);
            }
            /* Create Reflect calibration standard for each connector */
            for (int i = 0; i < connectorID.Length; i++)
            {
                vna.CalkitManagerCalkitAddCalibrationElement(calkitSelectorString, calibrationElementIDReflect[i]);
                calElementSelectorString = RFmxVnaMX.BuildCalibrationElementString(calkitSelectorString, calibrationElementIDReflect[i]);
                vna.CalkitManagerCalkitCalibrationElementSetTypes(calElementSelectorString, new RFmxVnaMXCalkitManagerCalkitCalibrationElementType[] {RFmxVnaMXCalkitManagerCalkitCalibrationElementType.Reflect});
                vna.CalkitManagerCalkitCalibrationElementSetPortConnectors(calElementSelectorString, new string[] {connectorID[i]});
                vna.CalkitManagerCalkitCalibrationElementSetDescription(calElementSelectorString, "Reflect (Short)");
                vna.CalkitManagerCalkitCalibrationElementSetMinimumFrequency(calElementSelectorString, 0);
                vna.CalkitManagerCalkitCalibrationElementSetMaximumFrequency(calElementSelectorString, 100.0e9);
                vna.CalkitManagerCalkitCalibrationElementSetSParameterDefinition(calElementSelectorString, RFmxVnaMXCalkitManagerCalkitCalibrationElementSParameterDefinition.ReflectModel);
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetModelType(calElementSelectorString, RFmxVnaMXCalkitManagerCalkitCalibrationElementReflectModelType.ReflectShort);
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetC0(calElementSelectorString, 0);
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetC1(calElementSelectorString, 0);
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetC2(calElementSelectorString, 0);
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetC3(calElementSelectorString, 0);
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetOffsetDelay(calElementSelectorString, 0);
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetOffsetLoss(calElementSelectorString, 0);
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetOffsetZ0(calElementSelectorString, 50);
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetSParamAvailability(calElementSelectorString, RFmxVnaMXCalkitManagerCalkitCalibrationElementReflectModelSParameterAvailability.Estimate);
            }
            /* Create Thru calibration standard */
            vna.CalkitManagerCalkitAddCalibrationElement(calkitSelectorString, calibrationElementIDThru);
            calElementSelectorString = RFmxVnaMX.BuildCalibrationElementString(calkitSelectorString, calibrationElementIDThru);
            vna.CalkitManagerCalkitCalibrationElementSetTypes(calElementSelectorString, new RFmxVnaMXCalkitManagerCalkitCalibrationElementType[] {RFmxVnaMXCalkitManagerCalkitCalibrationElementType.Thru});
            vna.CalkitManagerCalkitCalibrationElementSetPortConnectors(calElementSelectorString, connectorIDTwoPort);
            vna.CalkitManagerCalkitCalibrationElementSetDescription(calElementSelectorString, "Insertable Thru");
            vna.CalkitManagerCalkitCalibrationElementSetMinimumFrequency(calElementSelectorString, 0);
            vna.CalkitManagerCalkitCalibrationElementSetMaximumFrequency(calElementSelectorString, 100.0e9);
            vna.CalkitManagerCalkitCalibrationElementSetSParameterDefinition(calElementSelectorString, RFmxVnaMXCalkitManagerCalkitCalibrationElementSParameterDefinition.DelayModel);            
            vna.CalkitManagerCalkitCalibrationElementDelayModelSetDelay(calElementSelectorString, 0);
            /* Create Line standards */
            for (int i = 0; i < calibrationElementIDLine.Length; i++)
            {
                vna.CalkitManagerCalkitAddCalibrationElement(calkitSelectorString, calibrationElementIDLine[i]);
                calElementSelectorString = RFmxVnaMX.BuildCalibrationElementString(calkitSelectorString, calibrationElementIDLine[i]);
                vna.CalkitManagerCalkitCalibrationElementSetTypes(calElementSelectorString, new RFmxVnaMXCalkitManagerCalkitCalibrationElementType[] {RFmxVnaMXCalkitManagerCalkitCalibrationElementType.Line});
                vna.CalkitManagerCalkitCalibrationElementSetPortConnectors(calElementSelectorString, connectorIDTwoPort);
                vna.CalkitManagerCalkitCalibrationElementSetDescription(calElementSelectorString, calibrationElementIDLine[i]);
                vna.CalkitManagerCalkitCalibrationElementSetMinimumFrequency(calElementSelectorString, calibrationElementLineMinimumFrequencyHz[i]);
                vna.CalkitManagerCalkitCalibrationElementSetMaximumFrequency(calElementSelectorString, calibrationElementLineMaximumFrequencyHz[i]);
                vna.CalkitManagerCalkitCalibrationElementSetSParameterDefinition(calElementSelectorString, RFmxVnaMXCalkitManagerCalkitCalibrationElementSParameterDefinition.DelayModel);            
                vna.CalkitManagerCalkitCalibrationElementDelayModelSetDelay(calElementSelectorString, calibrationElementLineDelaySec[i]);
            }
            /* TRL Options */
            vna.CalkitManagerCalkitSetTrlReferencePlane(calkitSelectorString, RFmxVnaMXCalkitManagerCalkitTrlReferencePlane.Thru);
            vna.CalkitManagerCalkitSetLrlLineAutoChar(calkitSelectorString, false);
 
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
