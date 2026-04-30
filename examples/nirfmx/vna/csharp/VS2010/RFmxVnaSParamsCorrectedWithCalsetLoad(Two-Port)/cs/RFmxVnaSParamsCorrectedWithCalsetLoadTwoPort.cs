//Steps:
//1. Open a new RFmx session.
//2. Configure Frequency Reference.
//3. Configure sweep settings: Frequency List, IF Bandwidth, and Power Level and Test Rx Attenuation with different port names.
//4. Select S-Parameter measurement.
//5. Configure number of S-Parameters.
//6. Configure each S-Parameter and format.
//7. Configure Magnitude Units & Phase Trace Type.
//8. Load Calset data from a file.
//9. Enable Correction.
//10. Initiate the Measurement after user confirmation.
//11. Read Number of SParams.
//12. Fetch S-Parameter Correction State.
//13. Fetch S-Parameter X data.
//14. Fetch S-Parameter Y data for each S-Parameter.
//15. Set SnP Export attributes (can be accessed and written before or after measurement initiate) and save S-Parameter data to file.
//16. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.VnaMX;

namespace NationalInstruments.Examples.RFmxVnaSParamsCorrectedWithCalsetLoadTwoPort
{
    public class RFmxVnaSParamsCorrectedWithCalsetLoadTwoPort
    {
        RFmxInstrMX instrSession;
        RFmxVnaMX vna;
        string resourceName;

        RFmxVnaMXSweepType sweepType;
        int frequencyListSize;
        double frequencyStart;
        double frequencyStop;
        double port1PowerLevel;
        double port2PowerLevel;
        double port1TestReceiverAttenuation;
        double port2TestReceiverAttenuation;
        double IFBandwidth;

        string frequencyReferenceSource;
        double frequencyReferenceFrequency;

        int numberOfSParams;
        string[] sParamsSParameters;
        RFmxVnaMXSParamsFormat[] sParamsFormats;

        RFmxVnaMXSParamsMagnitudeUnits magnitudeUnits;
        RFmxVnaMXSParamsPhaseTraceType phaseTraceType;

        string calsetFilePath;
        string snpFilePath;

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

            sweepType = RFmxVnaMXSweepType.Linear;
            frequencyListSize = 251;
            frequencyStart = 1e9;
            frequencyStop = 26e9;

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

            magnitudeUnits = RFmxVnaMXSParamsMagnitudeUnits.dB;
            phaseTraceType = RFmxVnaMXSParamsPhaseTraceType.Wrapped;

            calsetFilePath = "";
            snpFilePath = "";

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
            vna.SetSweepType("", sweepType);
            vna.SetStartFrequency("", frequencyStart);
            vna.SetStopFrequency("", frequencyStop);
            vna.SetNumberOfPoints("", frequencyListSize);
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

            vna.CalsetLoadFromFile("", "", calsetFilePath);

            vna.SetCorrectionEnabled("", RFmxVnaMXCorrectionEnabled.True);
            vna.Initiate("", "");
        }

        void RetrieveResults()
        {
            vna.SParams.Configuration.GetNumberOfSParameters("", out numberOfSParamsResult);
            vna.SParams.Results.GetCorrectionState("", out correctionStateResult);
            vna.SParams.Results.FetchXData("", timeout, ref sParamsXDataResult);
            sParamsY1DataResult = new float[numberOfSParamsResult][];
            sParamsY2DataResult = new float[numberOfSParamsResult][];
            for (int i = 0; i < numberOfSParamsResult; i++)
            {
                sParamSelectorString = RFmxVnaMX.BuildSParameterString("", i);
                vna.SParams.Results.FetchYData(sParamSelectorString, timeout, ref sParamsY1DataResult[i], ref sParamsY2DataResult[i]);
            }

            vna.SParams.Configuration.SetSnPDataFormat("", RFmxVnaMXSParamsSnPDataFormat.Auto);
            vna.SParams.Configuration.SetSnPPorts("", "port1,port2");
            vna.SParams.Configuration.ExportToSnPFile("", snpFilePath);
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
