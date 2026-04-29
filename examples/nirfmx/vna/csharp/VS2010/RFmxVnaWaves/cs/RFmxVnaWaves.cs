//Steps:
//1. Open a new RFmx session.
//2. Configure Frequency Reference.
//3. Configure Sweep settings: Frequency List, IF Bandwidth, and Power Level and Test Rx Attenuation with different port names.
//4. Configure Averaging.
//5. Select Wave measurement.
//6. Configure Number of Waves.
//7. Configure each Wave and Format.
//8. Configure Magnitude Units & Phase Trace Type.
//9. Initiate the Measurement.
//10. Read the Num Waves.
//11. Fetch Waves X data.
//12. Fetch Waves Y data for each Wave.
//13. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.VnaMX;

namespace NationalInstruments.Examples.RFmxVnaWaves
{
    public class RFmxVnaWaves
    {
        RFmxInstrMX instrSession;
        RFmxVnaMX vna;
        string resourceName;

        RFmxVnaMXSweepType sweepType;
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

        int numberOfWaves;
        string[] waves;
        RFmxVnaMXWavesFormat[] wavesFormats;

        RFmxVnaMXWavesMagnitudeUnits magnitudeUnits;
        RFmxVnaMXWavesPhaseTraceType phaseTraceType;

        RFmxVnaMXAveragingEnabled averagingEnabled;
        int averagingCount;

        RFmxVnaMXMeasurementTypes measurement;
        bool enableAllTraces;

        string waveSelectorString;
        string portSelectorString;

        double timeout;

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

            sweepType = RFmxVnaMXSweepType.Linear;
            startFrequency = 1e9;                                                                /* (Hz) */
            stopFrequency = 10e9;                                                                /* (Hz) */
            frequencyPoints = 10;
            port1PowerLevel = -10.0;                                                             /* (dBm) */
            port2PowerLevel = -10.0;                                                             /* (dBm) */
            port1TestReceiverAttenuation = 0.0;                                                  /* (dB) */
            port2TestReceiverAttenuation = 0.0;                                                  /* (dB) */
            IFBandwidth = 100.0e3;                                                                  /* (Hz) */

            frequencyReferenceSource = RFmxInstrMXConstants.PxiClock;
            frequencyReferenceFrequency = 100e6;                                                 /* (Hz) */

            numberOfWaves = 4;
            waves = new string[] { "a1_1", "b1_1", "a1_2", "b1_2" };
            wavesFormats = new RFmxVnaMXWavesFormat[] { RFmxVnaMXWavesFormat.Magnitude,
                RFmxVnaMXWavesFormat.Phase, RFmxVnaMXWavesFormat.Magnitude, RFmxVnaMXWavesFormat.Phase };

            magnitudeUnits = RFmxVnaMXWavesMagnitudeUnits.dBm;
            phaseTraceType = RFmxVnaMXWavesPhaseTraceType.Wrapped;

            averagingEnabled = RFmxVnaMXAveragingEnabled.False;
            averagingCount = 10;

            measurement = RFmxVnaMXMeasurementTypes.Waves;
            enableAllTraces = false;

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
            vna.SelectMeasurements("", measurement, enableAllTraces);
            vna.Waves.Configuration.SetNumberOfWaves("", numberOfWaves);
            for (int i = 0; i < numberOfWaves; i++)
            {
                waveSelectorString = RFmxVnaMX.BuildWaveString("", i);
                vna.Waves.Configuration.ConfigureWave(waveSelectorString, waves[i]);
                vna.Waves.Configuration.SetFormat(waveSelectorString, wavesFormats[i]);
            }
            vna.Waves.Configuration.SetMagnitudeUnits("", magnitudeUnits);
            vna.Waves.Configuration.SetPhaseTraceType("", phaseTraceType);
            vna.Initiate("", "");
        }

        void RetrieveResults()
        {
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
