//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Component Carrier Spacing.
//6. Configure Component Carriers.
//7. Select PvT measurement and enable Traces.
//8. Configure Duplex Scheme.
//9. Configure Measurement Method.
//10. Configure Averaging Parameters for PvT measurement.
//11. Initiate the Measurement.
//12. Fetch PvT Measurements and Traces.
//13. Close RFmx Session. 

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.LteMX;

namespace NationalInstruments.Examples.RFmxLteULPvtContiguousMultiCarrier
{
    public class RFmxLteULPvtContiguousMultiCarrier
    {
        RFmxInstrMX instrSession;
        RFmxLteMX lte;

        string resourceName = "RFSA";
        string frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
        string iqPowerEdgeTriggerSource = "0";

        double frequencyReferenceFrequency = 10e6;                                    /* Hz */
        double centerFrequency = 1.95e+9;                                             /* Hz */
        double referenceLevel = 0.0;                                                  /* dBm */
        double externalAttenuation = 0.0;                                             /* dB */
        double iqPowerEdgeTriggerLevel = -20.0;                                       /* dB */
        double triggerDelay = 0.0;                                                    /* seconds */
        double minimumQuietTimeDuration = 50.0e-6;                                     /* seconds */
        double timeout = 10;                                                          /* seconds */
        bool enableTrigger = true;
        int averagingCount = 10;
        int componentCarrierAtCenterFrequency = -1;

        const int NumberOfComponentCarriers = 2;
        double[] componentCarrierBandwidth = { 20e6, 20e6 };
        double[] componentCarrierFrequency = { -9.9e6, 9.9e6 };

        double offPowerExclusionBefore = 0.0, offPowerExclusionAfter = 0.0;

        RFmxLteMXUplinkDownlinkConfiguration uplinkDownlinkConfiguration = RFmxLteMXUplinkDownlinkConfiguration.Configuration0;
        RFmxLteMXComponentCarrierSpacingType componentCarrierSpacingType = RFmxLteMXComponentCarrierSpacingType.Nominal;
        RFmxLteMXPvtMeasurementMethod measurementMethod = RFmxLteMXPvtMeasurementMethod.Normal;
        RFmxLteMXPvtAveragingEnabled averagingEnabled = RFmxLteMXPvtAveragingEnabled.False;
        RFmxLteMXPvtAveragingType averagingType = RFmxLteMXPvtAveragingType.Rms;
        RFmxLteMXTriggerMinimumQuietTimeMode minimumQuietTimeMode = RFmxLteMXTriggerMinimumQuietTimeMode.Auto;
        RFmxLteMXIQPowerEdgeTriggerLevelType iqPowerEdgeTriggerLevelType = RFmxLteMXIQPowerEdgeTriggerLevelType.Relative;
        RFmxLteMXIQPowerEdgeTriggerSlope iqPowerEdgeTriggerSlope = RFmxLteMXIQPowerEdgeTriggerSlope.Rising;
        RFmxLteMXDuplexScheme duplexScheme = RFmxLteMXDuplexScheme.Tdd;

        RFmxLteMXPvtMeasurementStatus[] measurementStatus;
        double[] meanAbsoluteOFFPowerBefore, meanAbsoluteOFFPowerAfter, meanAbsoluteONPower, burstWidth;
        AnalogWaveform<float> signalPower = null, absoluteLimit = null;

        string subblockCarrierString;

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

            lte.ComponentCarrier.ConfigureSpacing("", componentCarrierSpacingType,
                                                  componentCarrierAtCenterFrequency);

            lte.ConfigureNumberOfComponentCarriers("", NumberOfComponentCarriers);

            lte.ComponentCarrier.ConfigureArray("", componentCarrierBandwidth,
                                                componentCarrierFrequency, null);

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

            lte.Pvt.Results.FetchMeasurementArray("", timeout, ref measurementStatus, ref meanAbsoluteOFFPowerBefore,
                                                         ref meanAbsoluteOFFPowerAfter, ref meanAbsoluteONPower,
                                                         ref burstWidth);
            for (int i = 0; i < NumberOfComponentCarriers; i++)
            {
                subblockCarrierString = RFmxLteMX.BuildCarrierString("", i);
                lte.Pvt.Results.FetchSignalPowerTrace(subblockCarrierString, timeout,
                    ref signalPower, ref absoluteLimit);
            }

        }

        private void PrintResults()
        {
            Console.WriteLine("\n********** Measurement **********");

            for (int i = 0; i < NumberOfComponentCarriers; i++)
            {
                Console.WriteLine("\nCarrier  : {0}", i);
                Console.WriteLine("Status                               : {0}", measurementStatus[i]);
                Console.WriteLine("Mean Absolute OFF Power Before (dBm) : {0}", meanAbsoluteOFFPowerBefore[i]);
                Console.WriteLine("Mean Absolute OFF Power After (dBm)  : {0}", meanAbsoluteOFFPowerAfter[i]);
                Console.WriteLine("Mean Absolute ON Power (dBm)         : {0}", meanAbsoluteONPower[i]);
                Console.WriteLine("Burst Width (s)                      : {0}", burstWidth[i]);
                Console.WriteLine("---------------------------------------------");
            }
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
