//Steps:
//1. Open a new RFmx session.
//2. Create a new Calkit.
//3. Define Male and Female Connectors for the Calkit.
//4. Create Calibration Elements for One-port Standards.
//5. Create Calibration Elements for Two-Port Thru's.
//6. Export the Calkit to file
//7. Close RFmx Session.

using System;
using System.IO;
using System.Reflection;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.VnaMX;

namespace NationalInstruments.Examples.RFmxVnaCalibrationCreateSOLTCalkitSnPDataBased
{
    public class RFmxVnaCalibrationCreateSOLTCalkitSnPDataBased
    {
        RFmxInstrMX instrSession;
        RFmxVnaMX vna;
        string resourceName;
        string calkitID;
        string calkitSelectorString;
        string connectorSelectorString;
        string calElementSelectorString;
        string[] onePortCalibrationSParamterfiles;
        string[] twoPortCalibrationSParamterfilesfortheThrus;
        string[] connectorID;
        string[] connectorID_Combined;
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
            calkitID = "Example SOLT CalKit (SnP Data Based)";
            calkitFilePath = "..\\..\\..\\..\\..\\Support\\Example SOLT CalKit (SnP Data Based).nckt";
            connectorID = new string[] { "3.5mm-Male", "3.5mm-Female" };
            connectorID_Combined = new string[] { "3.5mm-Male,3.5mm-Male", "3.5mm-Female,3.5mm-Female", "3.5mm-Male,3.5mm-Female" };
            onePortCalibrationSParamterfiles = new string[] { "Short(m).s1p", "Open(m).s1p", "Load(m).s1p", "OffsetShort(m).s1p", "Short(f).s1p", "Open(f).s1p", "Load(f).s1p", "OffsetShort(f).s1p" };
            twoPortCalibrationSParamterfilesfortheThrus = new string[] { "Thru(m-m).s2p", "Thru(f-f).s2p", "Thru(m-f).s2p" };
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
            for (int j = 0; j < connectorID.Length; j++)
            {
                string connector = connectorID[j];
                int startIndex = (connector == "3.5mm-Male") ? 0 : 4;
                int endIndex = (connector == "3.5mm-Male") ? 4 : onePortCalibrationSParamterfiles.Length;
                for (int i = startIndex; i < endIndex; i++)
                {
                    vna.CalkitManagerCalkitAddCalibrationElement(calkitSelectorString, Path.GetFileNameWithoutExtension(onePortCalibrationSParamterfiles[i]));
                    calElementSelectorString = RFmxVnaMX.BuildCalibrationElementString(calkitSelectorString, Path.GetFileNameWithoutExtension(onePortCalibrationSParamterfiles[i]));
                    vna.CalkitManagerCalkitCalibrationElementSetTypes(calElementSelectorString, new RFmxVnaMXCalkitManagerCalkitCalibrationElementType[] {RFmxVnaMXCalkitManagerCalkitCalibrationElementType.Termination});
                    vna.CalkitManagerCalkitCalibrationElementSetDescription(calElementSelectorString, onePortCalibrationSParamterfiles[i]);
                    vna.CalkitManagerCalkitCalibrationElementSetPortConnectors(calElementSelectorString, new string[] {connector});
                    vna.CalkitManagerCalkitCalibrationElementSetMinimumFrequency(calElementSelectorString, 1.0e9);
                    vna.CalkitManagerCalkitCalibrationElementSetMaximumFrequency(calElementSelectorString, 99.0e9);
                    vna.CalkitManagerCalkitCalibrationElementSetSParameterDefinition(calElementSelectorString, RFmxVnaMXCalkitManagerCalkitCalibrationElementSParameterDefinition.Sparameter);
                    vna.CalkitManagerCalkitCalibrationElementReflectModelSetSParamAvailability(calElementSelectorString, RFmxVnaMXCalkitManagerCalkitCalibrationElementReflectModelSParameterAvailability.Known);
                    vna.CalkitManagerCalkitCalibrationElementSParameterSetFromFile(calElementSelectorString, onePortCalibrationSParamterfiles[i]);
                }
            }
            for (int i = 0; i < connectorID_Combined.Length; i++)
            {
                vna.CalkitManagerCalkitAddCalibrationElement(calkitSelectorString, Path.GetFileNameWithoutExtension(twoPortCalibrationSParamterfilesfortheThrus[i]));
                calElementSelectorString = RFmxVnaMX.BuildCalibrationElementString(calkitSelectorString, Path.GetFileNameWithoutExtension(twoPortCalibrationSParamterfilesfortheThrus[i]));
                vna.CalkitManagerCalkitCalibrationElementSetTypes(calElementSelectorString, new RFmxVnaMXCalkitManagerCalkitCalibrationElementType[] {RFmxVnaMXCalkitManagerCalkitCalibrationElementType.Thru});
                vna.CalkitManagerCalkitCalibrationElementSetDescription(calElementSelectorString, twoPortCalibrationSParamterfilesfortheThrus[i]);
                vna.CalkitManagerCalkitCalibrationElementSetPortConnectors(calElementSelectorString, new string[] {connectorID_Combined[i]});
                vna.CalkitManagerCalkitCalibrationElementSetMinimumFrequency(calElementSelectorString, 1.0e9);
                vna.CalkitManagerCalkitCalibrationElementSetMaximumFrequency(calElementSelectorString, 99.0e9);
                vna.CalkitManagerCalkitCalibrationElementSetSParameterDefinition(calElementSelectorString, RFmxVnaMXCalkitManagerCalkitCalibrationElementSParameterDefinition.Sparameter);
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetSParamAvailability(calElementSelectorString, RFmxVnaMXCalkitManagerCalkitCalibrationElementReflectModelSParameterAvailability.Known);
                vna.CalkitManagerCalkitCalibrationElementSParameterSetFromFile(calElementSelectorString, twoPortCalibrationSParamterfilesfortheThrus[i]);
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
