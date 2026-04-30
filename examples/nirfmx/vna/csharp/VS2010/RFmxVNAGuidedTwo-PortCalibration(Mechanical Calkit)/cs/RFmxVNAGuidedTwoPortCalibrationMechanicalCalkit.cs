//Steps:
//1.Open a new RFmx session.
//2. Configure Frequency Reference.
//3. Configure sweep settings: Sweep Type, Start Frequency, Stop Frequency, Number of Frequency Points, IF Bandwidth, and Power Level and Test Rx Attenuation with different port names.
//4. Select S-Parameter measurement. 
//5. Configure Calibration Ports, Calibration Method and Thru.
//6. Import Calkit File
//7. Configure Connector type & Mechanical Calkit Name for each VNA port.
//8. Initiate Calibration.
//9. Acquire Calibration data after user confirmation.
//10. Save Calibration data.
//11. Save Calset data to a file.
//12. Get Calset Frequency Grid.
//13. Get Calset Error Terms.
//13a.Calculate Magnitude of Error Terms
//14. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.VnaMX;

namespace NationalInstruments.Examples.RFmxVnaGuidedTwoPortCalibrationMechanicalCalkit
{
    public class RFmxVnaGuidedTwoPortCalibrationMechanicalCalkit
    {
        RFmxInstrMX instrSession;
        RFmxVnaMX vna;
        string resourceName;

        double frequencyStart;
        double frequencyEnd;
        int numberOfFrequencyPoints;
        double port1PowerLevel;
        double port2PowerLevel;
        double port1TestReceiverAttenuation;
        double port2TestReceiverAttenuation;
        double IFBandwidth;
        string frequencyReferenceSource;
        double frequencyReferenceFrequency;
        RFmxVnaMXCorrectionCalibrationMethod CorrectionCalibrationMethod;
        RFmxVnaMXCorrectionCalibrationThruMethod CorrectionCalibrationThruMethod;
        string[] calibrationPorts;
        string MechanicalCalkitName;
        string connectorType;
        double CalibrationTimeout;
        double ThruCoaxDelay;
        int CalStepCount;
        double CalibrationEstimatedThruDelay;
        string CalStepDescription;
        string[] calkitFilePath = { "" };
        string calsetFilePath;
        string portSelectorString;
        string calstepSelectorString;
        double[] frequencyGrid;
        ComplexSingle[][] errorTerm = new ComplexSingle[10][];

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
            numberOfFrequencyPoints = 251;
            port1PowerLevel = -10.0;                                                             /* (dBm) */
            port2PowerLevel = -10.0;                                                             /* (dBm) */
            port1TestReceiverAttenuation = 0.0;                                                  /* (dB) */
            port2TestReceiverAttenuation = 0.0;                                                  /* (dB) */
            IFBandwidth = 100e3;                                                                 /* (Hz) */
            frequencyReferenceSource = RFmxInstrMXConstants.PxiClock;
            frequencyReferenceFrequency = 100e6;                                                 /* (Hz) */
            CorrectionCalibrationMethod = RFmxVnaMXCorrectionCalibrationMethod.Solt;
            CorrectionCalibrationThruMethod = RFmxVnaMXCorrectionCalibrationThruMethod.Auto;
            CalibrationTimeout = 100.000;                                                        /* Seconds */
            ThruCoaxDelay = double.NaN;                                                          /* Seconds */
            calibrationPorts = new string[] { "port1", "port2" };
            MechanicalCalkitName = "";
            connectorType = "3.5mm female"; 
            calsetFilePath = "";
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
            vna.SetNumberOfPoints("", numberOfFrequencyPoints);
            vna.SetIFBandwidth("", IFBandwidth);
            portSelectorString = RFmxVnaMX.BuildPortString("", "port1");
            vna.SetPowerLevel(portSelectorString, port1PowerLevel);
            vna.SetTestReceiverAttenuation(portSelectorString, port1TestReceiverAttenuation);
            portSelectorString = RFmxVnaMX.BuildPortString("", "port2");
            vna.SetPowerLevel(portSelectorString, port2PowerLevel);
            vna.SetTestReceiverAttenuation(portSelectorString, port2TestReceiverAttenuation);
            vna.SelectMeasurements("", RFmxVnaMXMeasurementTypes.SParams, false);
            vna.SetCorrectionCalibrationPorts("", calibrationPorts);
            vna.SetCorrectionCalibrationMethod("", CorrectionCalibrationMethod);
            vna.SetCorrectionCalibrationThruMethod("", CorrectionCalibrationThruMethod);

            if (!double.IsNaN(ThruCoaxDelay))
            {
                vna.SetCorrectionCalibrationThruCoaxDelay("", ThruCoaxDelay);
            }
            for (int i = 0; i < calkitFilePath.Length; i++)
            {
                vna.CalkitManagerImportCalkit("", calkitFilePath[i]);
            }
            foreach (string portSelector in calibrationPorts)
            {
                string portSelectorString = RFmxVnaMX.BuildPortString("", portSelector);
                vna.SetCorrectionCalibrationCalkitType(portSelectorString, RFmxVnaMXCorrectionCalibrationCalkitType.Mechanical);
                vna.SetCorrectionCalibrationConnectorType(portSelectorString, connectorType);
                vna.SetCorrectionCalibrationCalkitMechanicalName(portSelectorString, MechanicalCalkitName);
            }
            vna.CalibrationInitiate("");
            vna.GetCorrectionCalibrationStepCount("", out CalStepCount);
            for (int i = 0; i < CalStepCount; i++)
            {
                calstepSelectorString = RFmxVnaMX.BuildCalstepString("", i);
                vna.GetCorrectionCalibrationStepDescription(calstepSelectorString, out CalStepDescription);
                Console.WriteLine($"CalStep {i + 1} Description: {CalStepDescription}");
                Console.WriteLine("Press any key for Next Cal Step...");
                Console.ReadKey(true);
                Console.Clear();
                vna.CalibrationAcquire(calstepSelectorString, CalibrationTimeout);
            }
            vna.CalibrationSave("", "");
            vna.CalsetSaveToFile("", "", calsetFilePath);
            vna.GetCorrectionCalibrationEstimatedThruDelay("", out CalibrationEstimatedThruDelay);
            Console.WriteLine($"Calibration Estimated Thru Delay: {CalibrationEstimatedThruDelay}");
        }

        void RetrieveResults()
        {
            vna.CalsetGetFrequencyGrid("", "", RFmxVnaMXCalFrequencyGrid.Directivity, ref frequencyGrid);
            for (int i = 0; i < 10; i++)
            {
                vna.CalsetGetErrorTerm("", "", GetCalErrorTermType(i), GetCalPort1(i), GetCalPort2(i), ref errorTerm[i]);
                CalculateMagnitude(ref errorTerm[i]);
            }
        }

        private RFmxVnaMXCalErrorTerm GetCalErrorTermType(int index)
        {
            switch (index)
            {
                case 0:
                case 1:
                    return RFmxVnaMXCalErrorTerm.Directivity;
                case 2:
                case 3:
                    return RFmxVnaMXCalErrorTerm.SourceMatch;
                case 4:
                case 5:
                    return RFmxVnaMXCalErrorTerm.ReflectionTracking;
                case 6:
                case 7:
                    return RFmxVnaMXCalErrorTerm.TransmissionTracking;
                case 8:
                case 9:
                    return RFmxVnaMXCalErrorTerm.LoadMatch;
                default:
                    throw new InvalidOperationException("Invalid index");
            }
        }

        private string GetCalPort1(int index)
        {
            return (index % 2 == 0) ? "port1" : "port2";
        }

        private string GetCalPort2(int index)
        {
            return (index % 2 == 0) ? "port2" : "port1";
        }

        private void CalculateMagnitude(ref ComplexSingle[] complexArray)
        {
            for (int i = 0; i < complexArray.Length; i++)
            {
                double absValue = Math.Sqrt(complexArray[i].Real * complexArray[i].Real + complexArray[i].Imaginary * complexArray[i].Imaginary);
                float magnitude = (float)(20 * Math.Log10(absValue));
                complexArray[i] = new ComplexSingle(magnitude, 0);
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
