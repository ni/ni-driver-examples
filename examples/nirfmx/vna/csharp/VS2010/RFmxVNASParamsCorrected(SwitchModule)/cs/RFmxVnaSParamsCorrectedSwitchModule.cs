//Steps:
//1.Open a new RFmx session.
//2. Configure Frequency Reference.
/*3. Configure sweep settings: Sweep Type, Start Frequency, Stop Frequency, Number of Frequency Points,
IF Bandwidth, and Power Level and Test Rx Attenuation with different port names.*/
//4. Configure Averaging.
//5. Select S-Parameter measurement.
//6. Configure number of S-Parameters.
//7. Configure each S-Parameter and format.
//8. Configure Magnitude Units & Phase Trace Type.
//9. Configure Calibration Ports and Calibration Method
//10. Configure Connector type & vCal Resource Name for each VNA port
//11. Initiate Calibration
//12. Read the Calstep Description for connection information.
//13. Acquire Calibration data after user confirmation 
//14. Save Calibration data
//15. Enable Correction
//16. Initiate the Measurement after user confirmation
//17. Read Number of SParams.
//18. Fetch S-Parameter X data.
//19. Fetch S-Parameter Y data for each S-Parameter.
//20. Fetch S-Parameter Correction State.
//21. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.VnaMX;

namespace NationalInstruments.Examples.RFmxVnaSParamsCorrectedSwitchModule
{
    public class RFmxVnaSParamsCorrectedSwitchModule
    {
        RFmxInstrMX instrSession;
        RFmxVnaMX vna;
        string resourceName;
        double frequencyStart;
        double frequencyEnd;
        int numberOfFrequencyPoints;
        string[] portNames;
        double[] powerLevel;
        double[] testReceiverAttenuation;
        double IFBandwidth;
        string frequencyReferenceSource;
        double frequencyReferenceFrequency;
        int numberOfSParams;
        string sParamSelectorString;
        string[] sParamsReceiverPorts;
        string[] sParamsSourcePorts;
        RFmxVnaMXSParamsFormat[] sParamsFormats;
        string[] calibrationPorts;
        string vCalOrientation;
        string vCalResourceName;
        string connectorType;
        double calibrationTimeout;
        string connectionInstruction;
        RFmxVnaMXSParamsMagnitudeUnits magnitudeUnits;
        RFmxVnaMXSParamsPhaseTraceType phaseTraceType;
        RFmxVnaMXAveragingEnabled averagingEnabled;
        int averagingCount;
        string portSelectorString;
        double timeout;
        int numberOfSParamsResult;
        RFmxVnaMXSParamsCorrectionState correctionStateResult;
        double[] sParamsXDataResult;
        float[][] sParamsY1DataResult;
        float[][] sParamsY2DataResult;

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
            portNames = new string[] { "rmm0/port0", "rmm0/port1" };
            powerLevel = new double[] { -10.0, -10.0 };                                          /* (dBm) */
            testReceiverAttenuation = new double[] { 0.0, 0.0 };                                 /* (dB) */
            IFBandwidth = 100e3;                                                                 /* (Hz) */

            frequencyReferenceSource = RFmxInstrMXConstants.PxiClock;
            frequencyReferenceFrequency = 100e6;                                                 /* (Hz) */
            numberOfSParams = 4;
            sParamsReceiverPorts = new string[] { "rmm0/port0", "rmm0/port0", "rmm0/port1", "rmm0/port1" };
            sParamsSourcePorts = new string[] { "rmm0/port0", "rmm0/port1", "rmm0/port0", "rmm0/port1" };
            sParamsFormats = new RFmxVnaMXSParamsFormat[]
            {
                RFmxVnaMXSParamsFormat.Magnitude,
                RFmxVnaMXSParamsFormat.Magnitude,
                RFmxVnaMXSParamsFormat.Magnitude,
                RFmxVnaMXSParamsFormat.Magnitude
            };

            calibrationPorts = new string[] { "rmm0/port0", "rmm0/port1" };
            vCalOrientation = "portA:rmm0/port0,portB:rmm0/port1";
            vCalResourceName = "vCal";
            connectorType = "3.5 mm female";
            calibrationTimeout = 100.0;                                                          /*seconds */
            magnitudeUnits = RFmxVnaMXSParamsMagnitudeUnits.dB;
            phaseTraceType = RFmxVnaMXSParamsPhaseTraceType.Wrapped;
            averagingEnabled = RFmxVnaMXAveragingEnabled.False;
            averagingCount = 10;
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
            vna.SetStopFrequency ("", frequencyEnd);
            vna.SetNumberOfPoints("", numberOfFrequencyPoints);
            vna.SetIFBandwidth("", IFBandwidth);
            for(int i = 0; i < portNames.Length; i++)
            {
                portSelectorString = RFmxVnaMX.BuildPortString("", portNames[i]);
                vna.SetPowerLevel(portSelectorString, powerLevel[i]);
                vna.SetTestReceiverAttenuation(portSelectorString, testReceiverAttenuation[i]);
            }

            vna.SetAveragingEnabled("", averagingEnabled);
            vna.SetAveragingCount("", averagingCount);

            vna.SelectMeasurements("", RFmxVnaMXMeasurementTypes.SParams, false);

            vna.SParams.Configuration.SetNumberOfSParameters("", numberOfSParams);

            for (int i = 0; i < numberOfSParams; i++)
            {
                sParamSelectorString = RFmxVnaMX.BuildSParameterString("", i);
                vna.SParams.Configuration.SetReceiverPort(sParamSelectorString, sParamsReceiverPorts[i]);
                vna.SParams.Configuration.SetSourcePort(sParamSelectorString, sParamsSourcePorts[i]);
                vna.SParams.Configuration.SetFormat(sParamSelectorString, sParamsFormats[i]);
            }

            vna.SParams.Configuration.SetMagnitudeUnits("", magnitudeUnits);
            vna.SParams.Configuration.SetPhaseTraceType("", phaseTraceType);

            vna.SetCorrectionCalibrationPorts("", calibrationPorts);
            vna.SetCorrectionCalibrationMethod("", RFmxVnaMXCorrectionCalibrationMethod.Solt);
            vna.SetCorrectionCalibrationCalkitElectronicOrientation("", vCalOrientation);

            vna.SetCorrectionCalibrationConnectorType("port::all", connectorType);
            vna.SetCorrectionCalibrationCalkitElectronicResourceName("port::all", vCalResourceName);

            vna.CalibrationInitiate("");
            vna.GetCorrectionCalibrationStepDescription("", out connectionInstruction);
            Console.WriteLine(connectionInstruction);
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
            vna.CalibrationAcquire("", calibrationTimeout);
            vna.CalibrationSave("", "");
            Console.WriteLine("Connect DUT across the specified measurement ports");
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();

            vna.SetCorrectionEnabled("", RFmxVnaMXCorrectionEnabled.True);
            vna.Initiate("", "");
        }

        void RetrieveResults()
        {
            vna.SParams.Configuration.GetNumberOfSParameters("", out numberOfSParamsResult);
            vna.SParams.Results.FetchXData("", timeout, ref sParamsXDataResult);
            sParamsY1DataResult = new float[numberOfSParamsResult][];
            sParamsY2DataResult = new float[numberOfSParamsResult][];
            for (int i = 0; i < numberOfSParamsResult; i++)
            {
                sParamSelectorString = RFmxVnaMX.BuildSParameterString("", i);
                vna.SParams.Results.FetchYData(sParamSelectorString, timeout, ref sParamsY1DataResult[i], ref sParamsY2DataResult[i]);
            }
            vna.SParams.Results.GetCorrectionState("", out correctionStateResult);
            Console.WriteLine($"Correction State: {correctionStateResult}");
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
