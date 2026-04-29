/*Steps:
1. Open a new RFmx Session.
2. Configure Frequency Reference.
3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
4. Configure Trigger Type and Trigger Parameters.
5. Configure Carrier Bandwidth.
6. Select PvT measurement and enable Traces.
7. Configure Duplex Scheme.
8. Configure Measurement Methods.
9. Configure Averaging Parameters for PvT measurement.
10. Initiate the Measurement.
11. Fetch PvT  Traces and Measurements.
12. Close RFmx Session.*/

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.LteMX;

namespace NationalInstruments.Examples.RFmxLteULPvtSingleCarrier
{
    public class RFmxLteULPvtSingleCarrier
    {
        RFmxInstrMX instrSession;
        RFmxLteMX lte;

        string resourceName = "RFSA";
        string frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
        string iqPowerEdgeTriggerSource = "0";

        double frequencyReferenceFrequency = 10e6;                                             /* Hz */
        double centerFrequency = 1.95e+9;                                                      /* Hz */
        double referenceLevel = 0.0;                                                           /* dBm */
        double externalAttenuation = 0.0;                                                      /* dB */
        double iqPowerEdgeTriggerLevel = -20.0;                                                /* dB */
        double triggerDelay = 0.0;                                                             /* seconds */
        double minimumQuietTimeDuration = 50.0e-6;								               /* seconds */
        double timeout = 10.0;                                                                /* seconds */
        double componentCarrierBandwidth = 10e6;                                               /* Hz */
        double componentCarrierFrequency = 0.0;                                                /* Hz */
        bool enableTrigger = true;
        int averagingCount = 10;
        int cellID = 0;

        double offPowerExclusionBefore = 0.0, offPowerExclusionAfter = 0.0;

        RFmxLteMXUplinkDownlinkConfiguration uplinkDownlinkConfiguration = RFmxLteMXUplinkDownlinkConfiguration.Configuration0;
        RFmxLteMXPvtMeasurementMethod measurementMethod = RFmxLteMXPvtMeasurementMethod.Normal;
        RFmxLteMXPvtAveragingEnabled averagingEnabled = RFmxLteMXPvtAveragingEnabled.False;
        RFmxLteMXPvtAveragingType averagingType = RFmxLteMXPvtAveragingType.Rms;
        RFmxLteMXTriggerMinimumQuietTimeMode minimumQuietTimeMode = RFmxLteMXTriggerMinimumQuietTimeMode.Auto;
        RFmxLteMXIQPowerEdgeTriggerLevelType iqPowerEdgeTriggerLevelType = RFmxLteMXIQPowerEdgeTriggerLevelType.Relative;
        RFmxLteMXIQPowerEdgeTriggerSlope iqPowerEdgeTriggerSlope = RFmxLteMXIQPowerEdgeTriggerSlope.Rising;
        RFmxLteMXDuplexScheme duplexScheme = RFmxLteMXDuplexScheme.Tdd;

        RFmxLteMXPvtMeasurementStatus measurementStatus;
        double meanAbsoluteOFFPowerBefore, meanAbsoluteOFFPowerAfter, meanAbsoluteONPower, burstWidth;
        AnalogWaveform<float> signalPower, absoluteLimit;

        public void Run()
        {
            try
            {
                InitializeInstr();
                ConfigureLte();
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


        private void InitializeInstr()
        {
            /* Create a new RFmx Session */
            instrSession = new RFmxInstrMX(resourceName, "");
        }

        private void ConfigureLte()
        {
            /* Get Lte signal */
            lte = instrSession.GetLteSignalConfiguration();

            /* Configure measurement */
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);

            lte.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);

            lte.ConfigureIQPowerEdgeTrigger("", iqPowerEdgeTriggerSource, iqPowerEdgeTriggerSlope, iqPowerEdgeTriggerLevel,
                triggerDelay, minimumQuietTimeMode, minimumQuietTimeDuration, iqPowerEdgeTriggerLevelType, enableTrigger);

            lte.ComponentCarrier.Configure("", componentCarrierBandwidth,
                                                componentCarrierFrequency, cellID);

            lte.SelectMeasurements("", RFmxLteMXMeasurementTypes.Pvt, true);

            lte.ConfigureDuplexScheme("", duplexScheme, uplinkDownlinkConfiguration);

            lte.Pvt.Configuration.ConfigureMeasurementMethod("", measurementMethod);

            lte.Pvt.Configuration.ConfigureOffPowerExclusionPeriods("", offPowerExclusionBefore, offPowerExclusionAfter);

            lte.Pvt.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);

            lte.Initiate("", "");

        }

        private void RetrieveResults()
        {
            /* Retrieve results */

            lte.Pvt.Results.FetchSignalPowerTrace("", timeout,
                    ref signalPower, ref absoluteLimit);

            lte.Pvt.Results.FetchMeasurement("", timeout, out measurementStatus, out meanAbsoluteOFFPowerBefore,
                                                         out meanAbsoluteOFFPowerAfter, out meanAbsoluteONPower,
                                                         out burstWidth);
        }

        private void PrintResults()
        {
            Console.WriteLine("\n********** Measurement **********");

            Console.WriteLine("Status                               : {0}", measurementStatus);
            Console.WriteLine("Mean Absolute OFF Power Before (dBm) : {0}", meanAbsoluteOFFPowerBefore);
            Console.WriteLine("Mean Absolute OFF Power After (dBm)  : {0}", meanAbsoluteOFFPowerAfter);
            Console.WriteLine("Mean Absolute ON Power (dBm)         : {0}", meanAbsoluteONPower);
            Console.WriteLine("Burst Width (s)                      : {0}", burstWidth);
        }

        private void CloseSession()
        {
            try
            {
                if (lte != null)
                {
                    lte.Dispose();
                    lte = null;
                }

                if (instrSession != null)
                {
                    instrSession.Close();
                    instrSession = null;
                }
            }
            catch (Exception ex)
            {
                DisplayError(ex);
            }
        }

        static private void DisplayError(Exception ex)
        {
            Console.WriteLine("ERROR:\n" + ex.GetType() + ": " + ex.Message);
        }
    }
}
