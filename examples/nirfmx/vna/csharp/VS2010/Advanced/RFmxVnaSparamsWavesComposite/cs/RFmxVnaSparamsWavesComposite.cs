//Steps:
//1.Open a new RFmx session.
//2. Configure Frequency Reference.
//3. Configure sweep settings: Sweep Type, Start Frequency, Stop Frequency, Number of Frequency Points, IF Bandwidth, and Power Level and Test Rx Attenuation with different port names.
//4. Configure Averaging.
//5. Select S-Parameter and Waves measurements.
//6. Configure number of S-Parameters.
//7. Configure each S-Parameter and Format.
//8. Configure Magnitude Units & Phase Trace Type.
//9. Configure Number of Waves.
//10. Configure each Wave and Format
//11. Configure Magnitude Units & Phase Trace Type
//12. Initiate the Measurement.
//13. Read Number of SParams and X-Axis Values (aggregated frequency list).
//14.Fetch S - Parameter X data.
//15.Fetch S - Parameter Y data for each S-Parameter.
//16. Read the Num Waves.
//17. Fetch Waves X data.
//18. Fetch Waves Y Data for each Wave.
//9. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.VnaMX;

namespace NationalInstruments.Examples.RFmxVnaSparamsWavesComposite
{
    public class RFmxVnaSparamsWavesComposite
    {
        RFmxInstrMX instrSession;
        RFmxVnaMX vna;
        string resourceName;

        double startFrequency;
        double stopFrequency;
        int frequencyPoints;
        double port1PowerLevel;
        double port2PowerLevel;
        double port1TestReceiverAttenuation;
        double port2TestReceiverAttenuation;
        double IFBandwidth;

        string frequencyReferenceSource;
        double frequencyReferenceFrequency;

        RFmxVnaMXAveragingEnabled averagingEnabled;
        int averagingCount;

        int numberOfSParams;
        string[] sParamsSParameters;
        RFmxVnaMXSParamsFormat[] sParamsFormats;

        RFmxVnaMXSParamsMagnitudeUnits sParamsMagnitudeUnits;
        RFmxVnaMXSParamsPhaseTraceType sParamsPhaseTraceType;

        int numberOfWaves;
        string[] waves;
        RFmxVnaMXWavesFormat[] wavesFormats;

        RFmxVnaMXWavesMagnitudeUnits wavesMagnitudeUnits;
        RFmxVnaMXWavesPhaseTraceType wavesPhaseTraceType;

        string sParamSelectorString;
        string waveSelectorString;
        string portSelectorString;

        double timeout;

        int numberOfSParamsResult;
        double[] sParamsXDataResult;
        float[][] sParamsY1DataResult;
        float[][] sParamsY2DataResult;

        int numberOfWavesResult;
        double[] wavesXDataResult;
        float[][] wavesY1DataResult;
        float[][] wavesY2DataResult;

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

            startFrequency = 1e9;                                                                /* (Hz) */
            stopFrequency = 26e9;                                                                /* (Hz) */
            frequencyPoints = 251;
            port1PowerLevel = -10.0;                                                             /* (dBm) */
            port2PowerLevel = -10.0;                                                             /* (dBm) */
            port1TestReceiverAttenuation = 0.0;                                                  /* (dB) */
            port2TestReceiverAttenuation = 0.0;                                                  /* (dB) */
            IFBandwidth = 100.0e3;                                                               /* (Hz) */

            frequencyReferenceSource = RFmxInstrMXConstants.PxiClock;
            frequencyReferenceFrequency = 100e6;                                                 /* (Hz) */

            averagingEnabled = RFmxVnaMXAveragingEnabled.False;
            averagingCount = 10;

            numberOfSParams = 4;
            sParamsSParameters = new string[] { "S11", "S12", "S21", "S22" };
            sParamsFormats = new RFmxVnaMXSParamsFormat[] { RFmxVnaMXSParamsFormat.Magnitude,
                RFmxVnaMXSParamsFormat.Magnitude, RFmxVnaMXSParamsFormat.Magnitude, RFmxVnaMXSParamsFormat.Magnitude };

            sParamsMagnitudeUnits = RFmxVnaMXSParamsMagnitudeUnits.dB;
            sParamsPhaseTraceType = RFmxVnaMXSParamsPhaseTraceType.Wrapped;

            numberOfWaves = 4;
            waves = new string[] { "a1_1", "b1_1", "a1_2", "b1_2" };
            wavesFormats = new RFmxVnaMXWavesFormat[] { RFmxVnaMXWavesFormat.Magnitude,
                RFmxVnaMXWavesFormat.Phase, RFmxVnaMXWavesFormat.Magnitude, RFmxVnaMXWavesFormat.Phase };

            wavesMagnitudeUnits = RFmxVnaMXWavesMagnitudeUnits.dBm;
            wavesPhaseTraceType = RFmxVnaMXWavesPhaseTraceType.Wrapped;

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
            vna.SetStartFrequency("", startFrequency);
            vna.SetStopFrequency("", stopFrequency);
            vna.SetNumberOfPoints("", frequencyPoints);
            vna.SetIFBandwidth("", IFBandwidth);
            portSelectorString = RFmxVnaMX.BuildPortString("", "port1");
            vna.SetPowerLevel(portSelectorString, port1PowerLevel);
            vna.SetTestReceiverAttenuation(portSelectorString, port1TestReceiverAttenuation);
            portSelectorString = RFmxVnaMX.BuildPortString("", "port2");
            vna.SetPowerLevel(portSelectorString, port2PowerLevel);
            vna.SetTestReceiverAttenuation(portSelectorString, port2TestReceiverAttenuation);
            vna.SetAveragingEnabled("", averagingEnabled);
            vna.SetAveragingCount("", averagingCount);
            vna.SelectMeasurements("", RFmxVnaMXMeasurementTypes.SParams | RFmxVnaMXMeasurementTypes.Waves, false);
            vna.SParams.Configuration.SetNumberOfSParameters("", numberOfSParams);
            for (int i = 0; i < numberOfSParams; i++)
            {
                sParamSelectorString = RFmxVnaMX.BuildSParameterString("", i);
                vna.SParams.Configuration.ConfigureSParameter(sParamSelectorString, sParamsSParameters[i]);
                vna.SParams.Configuration.SetFormat(sParamSelectorString, sParamsFormats[i]);
            }
            vna.SParams.Configuration.SetMagnitudeUnits("", sParamsMagnitudeUnits);
            vna.SParams.Configuration.SetPhaseTraceType("", sParamsPhaseTraceType);

            vna.Waves.Configuration.SetNumberOfWaves("", numberOfWaves);
            for (int i = 0; i < numberOfWaves; i++)
            {
                waveSelectorString = RFmxVnaMX.BuildWaveString("", i);
                vna.Waves.Configuration.ConfigureWave(waveSelectorString, waves[i]);
                vna.Waves.Configuration.SetFormat(waveSelectorString, wavesFormats[i]);
            }
            vna.Waves.Configuration.SetMagnitudeUnits("", wavesMagnitudeUnits);
            vna.Waves.Configuration.SetPhaseTraceType("", wavesPhaseTraceType);
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

            vna.Waves.Configuration.GetNumberOfWaves("", out numberOfWavesResult);
            vna.Waves.Results.FetchXData("", timeout, ref wavesXDataResult);
            wavesY1DataResult = new float[numberOfWavesResult][];
            wavesY2DataResult = new float[numberOfWavesResult][];
            for (int i = 0; i < numberOfWavesResult; i++)
            {
                waveSelectorString = RFmxVnaMX.BuildWaveString("", i);
                vna.Waves.Results.FetchYData(waveSelectorString, timeout, ref wavesY1DataResult[i], ref wavesY2DataResult[i]);
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
