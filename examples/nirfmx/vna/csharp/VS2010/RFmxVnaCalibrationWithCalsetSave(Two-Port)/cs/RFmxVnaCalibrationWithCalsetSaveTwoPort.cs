//Steps:
//1.Open a new RFmx session.
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

using System;
using System.Linq;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.VnaMX;

namespace NationalInstruments.Examples.RFmxVnaCalibrationWithCalsetSaveTwoPort
{
    public class RFmxVnaCalibrationWithCalsetSaveTwoPort
    {
        RFmxInstrMX instrSession;
        RFmxVnaMX vna;
        string resourceName;

        double frequencyStart;
        double frequencyEnd;
        int numOfFrequencyPoints;
        double port1PowerLevel;
        double port2PowerLevel;
        double port1TestReceiverAttenuation;
        double port2TestReceiverAttenuation;
        double IFBandwidth;

        string frequencyReferenceSource;
        double frequencyReferenceFrequency;

        string[] calibrationPorts;
        string vCalResourceName;
        string connectorType;
        double calibrationTimeout;
        bool autoDetectvCalOrientation;
        string vCalOrientation;

        string calsetFilePath;
        double[] frequencyGrid;
        string portSelectorString;
        double timeout;

        public void Run()
        {
            try
            {
                InitializeVariables();
                InitializeInstr();
                ConfigureVna();
                RetrieveResults();
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

            frequencyStart = 1e9;                                                                /* (Hz) */
            frequencyEnd = 26e9;                                                                 /* (Hz) */
            numOfFrequencyPoints = 251;
            port1PowerLevel = -10.0;                                                             /* (dBm) */
            port2PowerLevel = -10.0;                                                             /* (dBm) */
            port1TestReceiverAttenuation = 0.0;                                                  /* (dB) */
            port2TestReceiverAttenuation = 0.0;                                                  /* (dB) */
            IFBandwidth = 100e3;                                                                 /* (Hz) */

            frequencyReferenceSource = RFmxInstrMXConstants.PxiClock;
            frequencyReferenceFrequency = 100e6;                                                 /* (Hz) */

            vCalResourceName = "vCal";
            connectorType = "3.5 mm female";
            calibrationTimeout = 100.0;                                                          /*seconds */
            autoDetectvCalOrientation = true;
            vCalOrientation = "PortA:Port1,PortB:Port2";

            calsetFilePath = "";
            timeout = 10.0;                                                                      /*seconds */
        }

        void InitializeInstr()
        {
            instrSession = new RFmxInstrMX(resourceName, "");
        }

        void ConfigureVna()
        {
            vna = instrSession.GetVnaSignalConfiguration();                                      /* Create a new RFmx Session */
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
            vna.SetSweepType("", RFmxVnaMXSweepType.Linear);
            vna.SetStartFrequency("", frequencyStart);
            vna.SetStopFrequency("", frequencyEnd);
            vna.SetNumberOfPoints("", numOfFrequencyPoints);
            vna.SetIFBandwidth("", IFBandwidth);
            portSelectorString = RFmxVnaMX.BuildPortString("", "port1");
            vna.SetPowerLevel(portSelectorString, port1PowerLevel);
            vna.SetTestReceiverAttenuation(portSelectorString, port1TestReceiverAttenuation);
            portSelectorString = RFmxVnaMX.BuildPortString("", "port2");
            vna.SetPowerLevel(portSelectorString, port2PowerLevel);
            vna.SetTestReceiverAttenuation(portSelectorString, port2TestReceiverAttenuation);

            vna.SelectMeasurements("", RFmxVnaMXMeasurementTypes.SParams, false);

            vna.SetCorrectionCalibrationPorts("", new string[] { "port1", "port2" });
            vna.SetCorrectionCalibrationMethod("", RFmxVnaMXCorrectionCalibrationMethod.Solt);

            vna.SetCorrectionCalibrationConnectorType("port::all", connectorType);
            vna.SetCorrectionCalibrationCalkitElectronicResourceName("port::all", vCalResourceName);

            if (autoDetectvCalOrientation)
            {
                vna.AutoDetectvCalOrientation("");
                vna.GetCorrectionCalibrationCalkitElectronicOrientation("", out vCalOrientation);
                Console.WriteLine(string.Format("vCal Orientation         :{0}", vCalOrientation));
            }
            else
            {
                vna.SetCorrectionCalibrationCalkitElectronicOrientation("", vCalOrientation);
            }

            vna.CalibrationInitiate("");
            vna.CalibrationAcquire("", calibrationTimeout);
            vna.CalibrationSave("", "");
            vna.CalsetSaveToFile("", "", calsetFilePath);
        }

        void RetrieveResults()
        {
            var errorTermIdentifier = new RFmxVnaMXCalErrorTerm[] {
                RFmxVnaMXCalErrorTerm.Directivity,
                RFmxVnaMXCalErrorTerm.SourceMatch,
                RFmxVnaMXCalErrorTerm.ReflectionTracking,
                RFmxVnaMXCalErrorTerm.TransmissionTracking,
                RFmxVnaMXCalErrorTerm.LoadMatch };
            vna.CalsetGetFrequencyGrid("", "", RFmxVnaMXCalFrequencyGrid.Directivity, ref frequencyGrid);
            var errorTermsPort1 = new ComplexSingle[frequencyGrid.Length];
            var errorTermsPort2 = new ComplexSingle[frequencyGrid.Length];
            for (int i = 0; i < errorTermIdentifier.Length; i++)
            {
                vna.CalsetGetErrorTerm("", "", errorTermIdentifier[i], "port1", "port2", ref errorTermsPort1);
                vna.CalsetGetErrorTerm("", "", errorTermIdentifier[i], "port2", "port1", ref errorTermsPort2);
                var errorTerms = errorTermsPort1.Concat(errorTermsPort2).ToArray();
                for (int j = 0; j < errorTerms.Length; j++)
                {
                    var errorTermsMagnitude = CalculateMagnitude(ref errorTerms);
                }
            }
        }

        private float[] CalculateMagnitude(ref ComplexSingle[] complexArray)
        {
            var magnitude = new float[complexArray.Length];
            for (int i = 0; i < complexArray.Length; i++)
            {
                double absValue = Math.Sqrt(complexArray[i].Real * complexArray[i].Real + complexArray[i].Imaginary * complexArray[i].Imaginary);
                magnitude[i] = (float)(20 * Math.Log10(absValue));
            }
            return magnitude;
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
