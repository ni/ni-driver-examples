/*Steps:
1. Open a new RFmx Session.
2. Configure Frequency Reference.
3. Configure basic signal properties (Reference Level and External Attenuation).
4. Configure Trigger Type and Trigger Parameters.
5. Configure Subblock Configurations.
5A. Configure Number of Subblocks.
5B. Configure subblock Frequency.
5C. Configure Component Carrier Spacing.
5D. Configure Number of Component Carriers.
5E. Configure Component Carriers (Component Carrier Frequency and Component Carrier Bandwidth).
6. Select PvT measurement and enable Traces.
7. Configure Duplex Scheme.
8. Configure Measurements.
9. Configure Averaging Parameters for PvT measurement.
10. Initiate the Measurement.
11. Fetch PvT Measurements and Traces.
12. Close RFmx Session.*/

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.LteMX;

namespace NationalInstruments.Examples.RFmxLteULPvtNonContiguousMultiCarrier
{
    public class RFmxLteULPvtNonContiguousMultiCarrier
    {
        RFmxInstrMX instrSession;
        RFmxLteMX lte;

        string resourceName = "RFSA";
        string frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
        string iqPowerEdgeTriggerSource = "0";

        double frequencyReferenceFrequency = 10e6;                                            /* Hz */
        double iqPowerEdgeTriggerLevel = -20.0;                                               /* dB */
        double triggerDelay = 0.0;
        double minimumQuietTimeDuration = 5.0e-6;								              /* seconds */
        double timeout = 10.0;                                                                /* seconds */
        double centerFrequency = 1.95e9;                                                      /* (Hz) */
        double referenceLevel = 0.00;                                                         /* dBm */
        double externalAttenuation = 0.00;                                                    /* dB */
        bool enableTrigger = true;

        int averagingCount = 10;

        const int NumberOfSubblocks = 2;
        const int NumberOfComponentCarriers = 1;

        double offPowerExclusionBefore = 0.0, offPowerExclusionAfter = 0.0;

        RFmxLteMXUplinkDownlinkConfiguration uplinkDownlinkConfiguration = RFmxLteMXUplinkDownlinkConfiguration.Configuration0;
        RFmxLteMXPvtMeasurementMethod measurementMethod = RFmxLteMXPvtMeasurementMethod.Normal;
        RFmxLteMXPvtAveragingEnabled averagingEnabled = RFmxLteMXPvtAveragingEnabled.False;
        RFmxLteMXPvtAveragingType averagingType = RFmxLteMXPvtAveragingType.Rms;
        RFmxLteMXTriggerMinimumQuietTimeMode minimumQuietTimeMode = RFmxLteMXTriggerMinimumQuietTimeMode.Auto;
        RFmxLteMXIQPowerEdgeTriggerLevelType iqPowerEdgeTriggerLevelType = RFmxLteMXIQPowerEdgeTriggerLevelType.Relative;
        RFmxLteMXIQPowerEdgeTriggerSlope iqPowerEdgeTriggerSlope = RFmxLteMXIQPowerEdgeTriggerSlope.Rising;
        RFmxLteMXDuplexScheme duplexScheme = RFmxLteMXDuplexScheme.Tdd;

        /* Subblock inputs structure */
        struct SubblockInput
        {
	        public double subblockFrequency;                                            /*(Hz) */
            public RFmxLteMXComponentCarrierSpacingType componentCarrierSpacingType;
            public int componentCarrierAtCenterFrequency;
            public double[] componentCarrierBandwidth;                                  /*(Hz) */
            public double[] componentCarrierFrequency;                                  /*(Hz) */
        };

        /* Subblock measurement outputs structure */
        struct SubblockMeasurement
        {
            public RFmxLteMXPvtMeasurementStatus[] measurementStatus;
            public double[] meanAbsoluteOffPowerBefore;                                 /*(dBm) */
            public double[] meanAbsoluteOffPowerAfter;                                  /*(dBm) */
            public double[] meanAbsoluteOnPower;                                        /*(dBm) */
            public double[] burstWidth;                                                 /*(s) */
        };

        SubblockInput[] subblockInput = new SubblockInput[NumberOfSubblocks] {
                                                                    new SubblockInput {
                                                                                            subblockFrequency = 0.0,
                                                                                            componentCarrierSpacingType = RFmxLteMXComponentCarrierSpacingType.Nominal, 
                                                                                            componentCarrierAtCenterFrequency = -1, 
                                                                                            componentCarrierBandwidth = new double[NumberOfComponentCarriers] {20e6}, 
                                                                                            componentCarrierFrequency = new double[NumberOfComponentCarriers] {0.0},
                                                                                      },
                                                                    new SubblockInput {
                                                                                            subblockFrequency = 30e6,
                                                                                            componentCarrierSpacingType = RFmxLteMXComponentCarrierSpacingType.Nominal, 
                                                                                            componentCarrierAtCenterFrequency = -1, 
                                                                                            componentCarrierBandwidth = new double[NumberOfComponentCarriers] {20e6}, 
                                                                                            componentCarrierFrequency =  new double[NumberOfComponentCarriers] {0.0},
                                                                                      }
                                                                };
        SubblockMeasurement[] subblockMsr = new SubblockMeasurement[NumberOfSubblocks];

        AnalogWaveform<float> signalPower = null, absoluteLimit = null;
        string[] subblockString = new string[NumberOfSubblocks], subblockCarrierString = new string[NumberOfSubblocks];

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

            lte.ConfigureNumberOfSubblocks("", NumberOfSubblocks);

            for (int i = 0; i < NumberOfSubblocks; i++)
            {
                subblockString[i] = RFmxLteMX.BuildSubblockString("", i);
            lte.SetSubblockFrequency(subblockString[i], subblockInput[i].subblockFrequency);
                lte.ComponentCarrier.ConfigureSpacing(subblockString[i], subblockInput[i].componentCarrierSpacingType,
                                                      subblockInput[i].componentCarrierAtCenterFrequency);

                lte.ConfigureNumberOfComponentCarriers(subblockString[i], NumberOfComponentCarriers);

                lte.ComponentCarrier.ConfigureArray(subblockString[i], subblockInput[i].componentCarrierBandwidth,
                                                    subblockInput[i].componentCarrierFrequency,
                                                    null);
            }

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

            for (int i = 0; i < NumberOfSubblocks; i++)
            {
                lte.Pvt.Results.FetchMeasurementArray(subblockString[i], timeout, ref subblockMsr[i].measurementStatus,
                    ref subblockMsr[i].meanAbsoluteOffPowerBefore, ref subblockMsr[i].meanAbsoluteOffPowerAfter,
                    ref subblockMsr[i].meanAbsoluteOnPower, ref subblockMsr[i].burstWidth);

                subblockCarrierString[i] = RFmxLteMX.BuildCarrierString(subblockString[i], 0);

                lte.Pvt.Results.FetchSignalPowerTrace(subblockCarrierString[i], timeout, ref signalPower, ref absoluteLimit);
            }
        }

        private void PrintResults()
        {
            Console.WriteLine("\n********** Measurements ********** ");
            for (int i = 0; i < NumberOfSubblocks; i++)
            {
                Console.WriteLine("Subblock                             : {0}", i);
                for (int j = 0; j < NumberOfComponentCarriers; j++)
                {
                    Console.WriteLine("Carrier                              : {0}", j);
                    Console.WriteLine("Status                               : {0}\n", subblockMsr[i].measurementStatus[j]);
                    Console.WriteLine("Mean Absolute OFF Power Before (dBm) : {0}\n", subblockMsr[i].meanAbsoluteOffPowerBefore[j]);
                    Console.WriteLine("Mean Absolute OFF Power After (dBm)  : {0}\n", subblockMsr[i].meanAbsoluteOffPowerAfter[j]);
                    Console.WriteLine("Mean Absolute ON Power (dBm)         : {0}\n", subblockMsr[i].meanAbsoluteOnPower[j]);
                    Console.WriteLine("Burst Width (s)                      : {0}\n", subblockMsr[i].burstWidth[j]);
                    Console.WriteLine("---------------------------------------------\n");
                }
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
