//Steps:
//1. Open a new RFmx session.
//2. Configure Frequency Reference.
//3. Create a named signal instance.
//4. Configure sweep settings: Frequency List, IF Bandwidth, and Power Level and Test Rx Attenuation with different port names.
//5. Select S-Parameter measurement.
//6. Configure number of S-Parameters.
//7. Configure each S-Parameter and format.
//8. Configure Magnitude Units & Phase Trace Type.
//9. Load one or more Calset data from files(s) to create a global pool of named calsets.
//10. Select a named calset from the global pool to set as active calset for the specified signal. 
//11. Enable Correction
//12. Initiate the Measurement after user confirmation
//13. Read Number of SParams and X-Axis Values (aggregated frequency list).
//14. Fetch S-Parameter X data. 
//15. Fetch S-Parameter Y data for each S-Parameter.
//16. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.VnaMX;

namespace NationalInstruments.Examples.RFmxVnaSParamsCorrectedWithNamedCalsetLoad
{
    public class RFmxVnaSParamsCorrectedWithNamedCalsetLoad
    {
        RFmxInstrMX instrSession;
        RFmxVnaMX vna, vnaSignal1;
        string resourceName;

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

        int numberOfCalsets;
        string[] calsetFilePath;
        string[] calsetName;

        string sParamSelectorString;
        string portSelectorString;

        double timeout;

        int numberOfSParamsResult;
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

            numberOfCalsets = 3;
            calsetFilePath = new string[] { "", "", "" };
            calsetName = new string[] { "Calset1", "Calset2", "Calset3" };

            timeout = 10.0;                                                                      /*seconds */
        }

        void InitializeInstr()
        {
            instrSession = new RFmxInstrMX(resourceName, "");
        }

        void ConfigureVna()
        {
            vna = instrSession.GetVnaSignalConfiguration();                                      /* Create a new RFmx Session */
            vna.CloneSignalConfiguration("Signal1", out vnaSignal1);                             /* Create a named signal instance */
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
            vnaSignal1.SetSweepType("", RFmxVnaMXSweepType.Linear);
            vnaSignal1.SetStartFrequency("", frequencyStart);
            vnaSignal1.SetStopFrequency("", frequencyStop);
            vnaSignal1.SetNumberOfPoints("", frequencyListSize);
            vnaSignal1.SetIFBandwidth("", IFBandwidth);
            portSelectorString = RFmxVnaMX.BuildPortString("", "port1");
            vnaSignal1.SetPowerLevel(portSelectorString, port1PowerLevel);
            vnaSignal1.SetTestReceiverAttenuation(portSelectorString, port1TestReceiverAttenuation);
            portSelectorString = RFmxVnaMX.BuildPortString("", "port2");
            vnaSignal1.SetPowerLevel(portSelectorString, port2PowerLevel);
            vnaSignal1.SetTestReceiverAttenuation(portSelectorString, port2TestReceiverAttenuation);

            vnaSignal1.SelectMeasurements("", RFmxVnaMXMeasurementTypes.SParams, false);

            vnaSignal1.SParams.Configuration.SetNumberOfSParameters("", numberOfSParams);

            for (int i = 0; i < numberOfSParams; i++)
            {
                sParamSelectorString = RFmxVnaMX.BuildSParameterString("", i);
                vnaSignal1.SParams.Configuration.ConfigureSParameter(sParamSelectorString, sParamsSParameters[i]);
                vnaSignal1.SParams.Configuration.SetFormat(sParamSelectorString, sParamsFormats[i]);
            }

            vnaSignal1.SParams.Configuration.SetMagnitudeUnits("", magnitudeUnits);
            vnaSignal1.SParams.Configuration.SetPhaseTraceType("", phaseTraceType);

            for (int i = 0; i < numberOfCalsets; i++)
            {
                vna.CalsetLoadFromFile("", calsetName[i], calsetFilePath[i]);
            }
            vnaSignal1.SelectActiveCalset("", "Calset1", RFmxVnaMXRestoreConfiguration.None);

            vnaSignal1.SetCorrectionEnabled("", RFmxVnaMXCorrectionEnabled.True);
            vnaSignal1.Initiate("", "");
        }

        void RetrieveResults()
        {
            vnaSignal1.SParams.Configuration.GetNumberOfSParameters("", out numberOfSParamsResult);
            vnaSignal1.SParams.Results.FetchXData("", timeout, ref sParamsXDataResult);
            sParamsY1DataResult = new float[numberOfSParamsResult][];
            sParamsY2DataResult = new float[numberOfSParamsResult][];
            for (int i = 0; i < numberOfSParamsResult; i++)
            {
                sParamSelectorString = RFmxVnaMX.BuildSParameterString("", i);
                vnaSignal1.SParams.Results.FetchYData(sParamSelectorString, timeout, ref sParamsY1DataResult[i], ref sParamsY2DataResult[i]);
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
