//Steps:
//1.Open a new RFmx session.
//2. Configure Frequency Reference.
//3. Configure sweep settings: Sweep Type = CW Time, Number of Points, IF Bandwidth, and Power Level and Test Rx Attenuation with different port names.
//4. Select S-Parameter measurement.
//5. Configure number of S-Parameters.
//6. Configure each S-Parameter and format.
//7. Configure Magnitude Units & Phase Trace Type.
//8. Configure Calibration Ports and Calibration Method
//9. Configure Connector type & vCal Resource Name for each VNA port
//10. Initiate Calibration
//11. Acquire Calibration data after user confirmation 
//12. Save Calibration data
//13. Enable Correction
//14. Initiate the Measurement after user confirmation
//15. Read Number of SParams.
//16. Fetch S-Parameter X data.
//17. Fetch S-Parameter Y data for each S-Parameter.
//18. Fetch S-Parameter Correction State.
//19. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.VnaMX;

namespace NationalInstruments.Examples.RFmxVnaCWTimeSweep
{
    public class RFmxVnaCWTimeSweep
    {
        RFmxInstrMX instrSession;
        RFmxVnaMX vna;
        string resourceName;

        double cwfrequency;
        int numberOfPoints;
        double IFBandwidth;
        double port1PowerLevel;
        double port1TestReceiverAttenuation;
        double port2PowerLevel;
        double port2TestReceiverAttenuation;
        

        string frequencyReferenceSource;
        double frequencyReferenceFrequency;

        int numberOfSParams;
        string[] sParamsSParameters;
        RFmxVnaMXSParamsFormat[] sParamsFormats;

        RFmxVnaMXSParamsMagnitudeUnits magnitudeUnits;
        RFmxVnaMXSParamsPhaseTraceType phaseTraceType;

        string[] calibrationPorts;
        string vCalResourceName;
        string connectorType;
        double calibrationTimeout;

        string sParamSelectorString;
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
                PrintResults();
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

            cwfrequency = 1e9;                                                                   /* (Hz) */
            numberOfPoints = 100;
            port1PowerLevel = -10.0;                                                             /* (dBm) */
            port2PowerLevel = -10.0;                                                             /* (dBm) */
            port1TestReceiverAttenuation = 0.0;                                                  /* (dB) */
            port2TestReceiverAttenuation = 0.0;                                                  /* (dB) */
            IFBandwidth = 100e3;                                                                 /* (Hz) */

            frequencyReferenceSource = RFmxInstrMXConstants.PxiClock;
            frequencyReferenceFrequency = 100e6;                                                 /* (Hz) */

            numberOfSParams = 4;
            sParamsSParameters = new string[] { "S11", "S12", "S21", "S22" };
            sParamsFormats = new RFmxVnaMXSParamsFormat[]
            {
                RFmxVnaMXSParamsFormat.Magnitude,
                RFmxVnaMXSParamsFormat.Magnitude,
                RFmxVnaMXSParamsFormat.Magnitude,
                RFmxVnaMXSParamsFormat.Magnitude
            };

            calibrationPorts = new string[] { "port1", "port2" };
            vCalResourceName = "vCal";
            connectorType = "3.5 mm female";
            calibrationTimeout = 100.0;                                                          /*seconds */

            magnitudeUnits = RFmxVnaMXSParamsMagnitudeUnits.dB;
            phaseTraceType = RFmxVnaMXSParamsPhaseTraceType.Wrapped;

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
            vna.SetSweepType("", RFmxVnaMXSweepType.CWTime);
            vna.SetCWFrequency("", cwfrequency);
            vna.SetNumberOfPoints("", numberOfPoints);
            vna.SetIFBandwidth("", IFBandwidth);
            portSelectorString = RFmxVnaMX.BuildPortString("", "port1");
            vna.SetPowerLevel(portSelectorString, port1PowerLevel);
            vna.SetTestReceiverAttenuation(portSelectorString, port1TestReceiverAttenuation);
            portSelectorString = RFmxVnaMX.BuildPortString("", "port2");
            vna.SetPowerLevel(portSelectorString, port2PowerLevel);
            vna.SetTestReceiverAttenuation(portSelectorString, port2TestReceiverAttenuation);

            vna.SelectMeasurements("", RFmxVnaMXMeasurementTypes.SParams, false);

            vna.SParams.Configuration.SetNumberOfSParameters("", numberOfSParams);

            for (int i = 0; i < numberOfSParams; i++)
            {
                sParamSelectorString = RFmxVnaMX.BuildSParameterString("", i);
                vna.SParams.Configuration.ConfigureSParameter(sParamSelectorString, sParamsSParameters[i]);
                vna.SParams.Configuration.SetFormat(sParamSelectorString, sParamsFormats[i]);
            }

            vna.SParams.Configuration.SetMagnitudeUnits("", magnitudeUnits);
            vna.SParams.Configuration.SetPhaseTraceType("", phaseTraceType);

            vna.SetCorrectionCalibrationPorts("", calibrationPorts);
            vna.SetCorrectionCalibrationMethod("", RFmxVnaMXCorrectionCalibrationMethod.Solt);

            vna.SetCorrectionCalibrationConnectorType("port::all", connectorType);
            vna.SetCorrectionCalibrationCalkitElectronicResourceName("port::all", vCalResourceName);

            vna.CalibrationInitiate("");
            Console.WriteLine("Connect Port A of NI CAL-5501 to Port 1 of NI PXIe-5633,  and Port B of NI CAL-5501 to Port 2 of NI PXIe-5633.");
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
            vna.CalibrationAcquire("", calibrationTimeout);
            vna.CalibrationSave("", "");
            Console.WriteLine("Connect DUT across port1 and port2 of NI PXIe-5633.");
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
        }
        private void PrintResults()
        {
            Console.WriteLine("Correction State             {0}", correctionStateResult);
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
