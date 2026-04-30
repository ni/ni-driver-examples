/*Steps:
1.Open a new RFmx Session.
2.Configure Frequency Reference.
3.Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
4.Configure Trigger Type and Trigger Parameters.
5.Configure Component Carrier Spacing.
6.Configure Component Carriers.
7.Select TXP measurement and enable Traces.
8.Configure TXP measurement offset and length.
9.Configure Averaging Parameters for TXP measurement.
10.Initiate the Measurement.
11. Fetch TXP Measurements and Traces.
12. Close RFmx Session.*/

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.LteMX;

namespace NationalInstruments.Examples.RFmxLteTxpContiguousMultiCarrier
{
    public class RFmxLteTxpContiguousMultiCarrier
    {
        RFmxInstrMX instrSession;
        RFmxLteMX lte;

        string resourceName;

        double centerFrequency;
        double referenceLevel;
        double externalAttenuation;

        string frequencyReferenceSource;
        double frequencyReferenceFrequency;

        bool iqPowerEdgeTriggerEnabled;
        string iqPowerEdgeTriggerSource;
        double iqPowerEdgeTriggerLevel;
        double triggerDelay;
        double minimumQuietTimeDuration;
        RFmxLteMXTriggerMinimumQuietTimeMode minimumQuietTimeMode;
        RFmxLteMXIQPowerEdgeTriggerLevelType iqPowerEdgeTriggerLevelType;
        RFmxLteMXIQPowerEdgeTriggerSlope iqPowerEdgeTriggerSlope;

        int componentCarrierAtCenterFrequency;
        RFmxLteMXComponentCarrierSpacingType componentCarrierSpacingType;

        const int numberOfComponentCarriers = 2;
        double[] componentCarrierBandwidth = new double[numberOfComponentCarriers];
        double[] componentCarrierFrequency = new double[numberOfComponentCarriers];
        int[] cellID;

        RFmxLteMXTxpAveragingEnabled averagingEnabled;
        int averagingCount;
        
        double measurementOffset;                                                        
        double measurementLength;

        double timeout;

        double averagePowerMean;                                                            /* (dBm) */
        double peakPowerMaximum;                                                            /* (dBm) */

        AnalogWaveform<float> power;

        
        public void Run()
        {
            try
            {
                InitializeVariables();
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

        void InitializeVariables()
        {
            resourceName = "RFSA";

            centerFrequency = 1.95e9;                                                       /* (Hz) */
            referenceLevel = 0.00;                                                          /* (dBm) */
            externalAttenuation = 0.0;                                                      /* (dBm) */

            frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
            frequencyReferenceFrequency = 10e6;                                             /* (Hz) */

            iqPowerEdgeTriggerEnabled = false;
            iqPowerEdgeTriggerSource = "0";
            iqPowerEdgeTriggerLevel = -20.0;                                                /* dB */
            triggerDelay = 0.0;                                                             /* seconds */
            minimumQuietTimeDuration = 50.0e-6;                                             /* seconds */
            minimumQuietTimeMode = RFmxLteMXTriggerMinimumQuietTimeMode.Auto;
            iqPowerEdgeTriggerLevelType = RFmxLteMXIQPowerEdgeTriggerLevelType.Relative;
            iqPowerEdgeTriggerSlope = RFmxLteMXIQPowerEdgeTriggerSlope.Rising;

            cellID = null;

            componentCarrierBandwidth[0] = 20e6;                                            /* (Hz) */
            componentCarrierBandwidth[1] = 20e6;                                            /* (Hz) */
            componentCarrierFrequency[0] = -9.9e6;                                          /* (Hz) */
            componentCarrierFrequency[1] = 9.9e6;                                           /* (Hz) */

            averagingEnabled = RFmxLteMXTxpAveragingEnabled.False;
            averagingCount = 10;

            measurementOffset = 0.0;                                                        /* seconds */
            measurementLength = 1.0e-3;                                                     /* seconds */

            timeout = 10.0;                                                                 /* seconds */

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
                triggerDelay, minimumQuietTimeMode, minimumQuietTimeDuration, iqPowerEdgeTriggerLevelType, iqPowerEdgeTriggerEnabled);

            lte.ComponentCarrier.ConfigureSpacing("", componentCarrierSpacingType,
                                                 componentCarrierAtCenterFrequency);

            lte.ConfigureNumberOfComponentCarriers("", numberOfComponentCarriers);

            lte.ComponentCarrier.ConfigureArray("", componentCarrierBandwidth,
                                               componentCarrierFrequency, cellID);

            lte.SelectMeasurements("", RFmxLteMXMeasurementTypes.Txp, true);

            lte.Txp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount);

            lte.Txp.Configuration.ConfigureMeasurementOffsetAndInterval("", measurementOffset, measurementLength);
            lte.Initiate("", "");

        }

        private void RetrieveResults()
        {
            /* Retrieve results */

            lte.Txp.Results.FetchMeasurement("", timeout, out averagePowerMean, out peakPowerMaximum);

            lte.Txp.Results.FetchPowerTrace("", timeout, ref power);
        }

        private void PrintResults()
        {
            Console.WriteLine("\n********** Measurement **********");

            Console.WriteLine("Average Power Mean (dBm)      : {0}", averagePowerMean);
            Console.WriteLine("Peak Power Maximum (dBm)      : {0}\n", peakPowerMaximum);
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
