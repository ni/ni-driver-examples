//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5[A-F]. Configure Subblock Parameters.
//5A. Configure Number of Subblocks.
//5B. Configure subblock Frequency.
//5C. Configure Component Carrier Spacing.
//5D. Configure Number of Component Carriers.
//5E. Configure Component Carriers (Component Carrier Frequency and Component Carrier Bandwidth).
//5F. Configure Component Carrier Maximum Output Power for Downlink Link Direction.
//6. Configure Link Direction.
//7. Select SEM measurement and enable Traces.
//8. Configure Sweep Time Parameters.
//9. Configure Averaging Parameters for SEM measurement.
//10. Configure Uplink Mask Type, or Downlink Mask, eNodeB Category.
//11. Initiate the Measurement.
//12. Fetch SEM Measurements and Traces.
//13. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.LteMX;

namespace NationalInstruments.Examples.RFmxLteSemNonContiguousMultiCarrier
{
    /* Input: Subblock inputs structure */
    struct SubblockInput
    {
	    public double subblockFrequency;                           /*(Hz) */
        public RFmxLteMXComponentCarrierSpacingType componentCarrierSpacingType;
        public int componentCarrierAtCenterFrequency;
        public double[] componentCarrierBandwidth;                 /*(Hz) */
        public double[] componentCarrierFrequency;                 /*(Hz) */
        public double[] componentCarrierMaximumOutputPower;        /*(dBm) */
    }

    /* Input: Subblock measurement outputs structure */
    struct SubblockMeasurement
    {
        public double subblockPower;
        public double integrationBandwidth;
        public double subblockFrequency;

        public double[] lowerOffsetMarginRelativePower;             /*(dBm) */
        public double[] lowerOffsetMarginAbsolutePower;             /*(dBm) */
        public double[] lowerOffsetMargin;
        public double[] lowerOffsetMarginFrequency;                 /*(Hz) */
        public RFmxLteMXSemLowerOffsetMeasurementStatus[] lowerOffsetMeasurementStatus;

        public double[] upperOffsetMarginRelativePower;             /*(dBm) */
        public double[] upperOffsetMarginAbsolutePower;             /*(dBm) */
        public double[] upperOffsetMargin;
        public double[] upperOffsetMarginFrequency;                 /*(Hz) */
        public RFmxLteMXSemUpperOffsetMeasurementStatus[] upperOffsetMeasurementStatus;
    }

    public class RFmxLteSemNonContiguousMultiCarrier
    {
        RFmxInstrMX instrSession;
        RFmxLteMX lte;
        string rfsaResourceName;

        const int NumberOfComponentCarriers = 1;
        const int NumberOfSubblocks = 2;

        string frequencyReferenceSource;
        double frequencyReferenceFrequency;

        double centerFrequency;
        double referenceLevel;
        double externalAttenuation;

        bool enableTrigger;
        string digitalEdgeSource;
        RFmxLteMXDigitalEdgeTriggerEdge digitalEdge;
        double triggerDelay;

        RFmxLteMXLinkDirection linkDirection;
        RFmxLteMXSemUplinkMaskType uplinkMaskType;
        RFmxLteMXeNodeBCategory eNodeBCategory;
        RFmxLteMXSemDownlinkMaskType downlinkMaskType;
        double deltaFMaximum;
        double aggregatedMaximumPower;

        RFmxLteMXSemSweepTimeAuto sweepTimeAuto;
        double sweepTimeInterval;

        RFmxLteMXSemAveragingEnabled averagingEnabled;
        int averagingCount;

        RFmxLteMXSemAveragingType averagingType;
        double timeout;

        string subblockString;

        SubblockInput[] subblocks;
        SubblockMeasurement[] subblocksMsr;

        Spectrum<float> spectrum;
        Spectrum<float> absoluteMask;
        double totalAggregatedPower;
        RFmxLteMXSemMeasurementStatus measurementStatus;

        void CloseSession()
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

        void InitializeInstr()
        {
            instrSession = new RFmxInstrMX(rfsaResourceName, "");
        }

        void InitializeVariables()
        {
            rfsaResourceName = "RFSA";

            frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
            frequencyReferenceFrequency = 10e6;                     /* (Hz) */

            centerFrequency = 1.95e9;                               /* (Hz) */
            referenceLevel = 0.00;                                  /* (dBm) */
            externalAttenuation = 0.0;                              /* (dBm) */

            enableTrigger = false;
            digitalEdgeSource = RFmxLteMXConstants.Pfi0;
            digitalEdge = RFmxLteMXDigitalEdgeTriggerEdge.Rising;
            triggerDelay = 0.0;                                     /* (s) */

            linkDirection = RFmxLteMXLinkDirection.Uplink;

            uplinkMaskType = RFmxLteMXSemUplinkMaskType.General_NS01;
            eNodeBCategory = RFmxLteMXeNodeBCategory.WideAreaBaseStationCategoryA;
            deltaFMaximum = 15.00e6;                                /* (Hz) */
            aggregatedMaximumPower = 0.00;                          /* (dBm) */
            downlinkMaskType = RFmxLteMXSemDownlinkMaskType.ENodeBCategoryBased;

            sweepTimeAuto = RFmxLteMXSemSweepTimeAuto.True;
            sweepTimeInterval = 0.001;                              /* (s) */

            averagingEnabled = RFmxLteMXSemAveragingEnabled.False;
            averagingCount = 10;
            averagingType = RFmxLteMXSemAveragingType.Rms;

            timeout = 10.0;                                         /* (s) */

            subblocksMsr = new SubblockMeasurement[NumberOfSubblocks];
            subblocks = new SubblockInput[NumberOfSubblocks] {
                                                                new SubblockInput {
                                                                                    subblockFrequency = 0.0,
                                                                                    componentCarrierSpacingType = RFmxLteMXComponentCarrierSpacingType.Nominal,
                                                                                    componentCarrierAtCenterFrequency = -1,
                                                                                    componentCarrierBandwidth = new double[NumberOfComponentCarriers] {20e6},
                                                                                    componentCarrierFrequency = new double[NumberOfComponentCarriers] {0.0},
                                                                                    componentCarrierMaximumOutputPower =new double[NumberOfComponentCarriers]{0.00}
                                                                                  },
                                                                new SubblockInput {
                                                                                    subblockFrequency = 30e6, 
                                                                                    componentCarrierSpacingType = RFmxLteMXComponentCarrierSpacingType.Nominal,
                                                                                    componentCarrierAtCenterFrequency = -1,
                                                                                    componentCarrierBandwidth = new double[NumberOfComponentCarriers] {20e6},
                                                                                    componentCarrierFrequency =  new double[NumberOfComponentCarriers] {0.0},
                                                                                    componentCarrierMaximumOutputPower =new double[NumberOfComponentCarriers]{0.00},
                                                                                  }
                                                            };
        }

        void ConfigureLte()
        {
            lte = instrSession.GetLteSignalConfiguration();     /* Create a new RFmx Session */
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
            lte.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
            lte.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger);
            lte.ConfigureNumberOfSubblocks("", NumberOfSubblocks);
            for (int i = 0; i < NumberOfSubblocks; i++)
            {
                subblockString = RFmxLteMX.BuildSubblockString("", i);
                lte.SetSubblockFrequency(subblockString, subblocks[i].subblockFrequency);
                lte.ComponentCarrier.ConfigureSpacing(subblockString, subblocks[i].componentCarrierSpacingType,
                                                      subblocks[i].componentCarrierAtCenterFrequency);
                lte.ConfigureNumberOfComponentCarriers(subblockString, NumberOfComponentCarriers);
                lte.ComponentCarrier.ConfigureArray(subblockString, subblocks[i].componentCarrierBandwidth,
                                                    subblocks[i].componentCarrierFrequency, null);
               if (linkDirection == RFmxLteMXLinkDirection.Downlink)
                  lte.Sem.Configuration.ComponentCarrier.ConfigureMaximumOutputPowerArray(subblockString, subblocks[i].componentCarrierMaximumOutputPower);
            }
            lte.ConfigureLinkDirection("", linkDirection);
            lte.SelectMeasurements("", RFmxLteMXMeasurementTypes.Sem, true);
            lte.Sem.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval);
            lte.Sem.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);

            if (linkDirection == RFmxLteMXLinkDirection.Uplink)
                lte.Sem.Configuration.ConfigureUplinkMaskType("", uplinkMaskType);
            else
            {
                lte.ConfigureeNodeBCategory("", eNodeBCategory);
                lte.Sem.Configuration.ConfigureDownlinkMask("", downlinkMaskType, deltaFMaximum, aggregatedMaximumPower);
            }
            lte.Initiate("", "");
        }

        void RetrieveResults()
        {
            subblocksMsr = new SubblockMeasurement[NumberOfSubblocks];
            for (int i = 0; i < NumberOfSubblocks; i++)
            {
                subblockString = RFmxLteMX.BuildSubblockString("", i);

                lte.Sem.Results.FetchSubblockMeasurement(subblockString, timeout, out subblocksMsr[i].subblockPower,
                  out subblocksMsr[i].integrationBandwidth, out subblocksMsr[i].subblockFrequency);

                lte.Sem.Results.FetchUpperOffsetMarginArray(subblockString, timeout, ref subblocksMsr[i].upperOffsetMeasurementStatus,
                  ref subblocksMsr[i].upperOffsetMargin, ref subblocksMsr[i].upperOffsetMarginFrequency,
                  ref subblocksMsr[i].upperOffsetMarginAbsolutePower, ref subblocksMsr[i].upperOffsetMarginRelativePower);

                lte.Sem.Results.FetchLowerOffsetMarginArray(subblockString, timeout, ref subblocksMsr[i].lowerOffsetMeasurementStatus,
                  ref subblocksMsr[i].lowerOffsetMargin, ref subblocksMsr[i].lowerOffsetMarginFrequency,
                  ref subblocksMsr[i].lowerOffsetMarginAbsolutePower, ref subblocksMsr[i].lowerOffsetMarginRelativePower);
            }

            lte.Sem.Results.FetchTotalAggregatedPower("", timeout, out totalAggregatedPower);
            lte.Sem.Results.FetchMeasurementStatus("", timeout, out measurementStatus);
            lte.Sem.Results.FetchSpectrum("", timeout, ref spectrum, ref absoluteMask);
        }

        void PrintResults()
        {
            Console.WriteLine("Total Aggregated Power (dBm)    :{0}", totalAggregatedPower);
            Console.WriteLine("Measurement Status              :{0}", measurementStatus);
            Console.WriteLine("--------------------Subblock Measurements--------------------");
            for (var i = 0; i < subblocksMsr.Length; i++)
            {
                Console.WriteLine("\nSubblock {0}\n", i);

                Console.WriteLine("Subblock Power (dBm)            :{0}", subblocksMsr[i].subblockPower);
                Console.WriteLine("Integration Bandwidth (Hz)      :{0}", subblocksMsr[i].integrationBandwidth);
                Console.WriteLine("Frequency (Hz)                  :{0}", subblocksMsr[i].subblockFrequency);

                Console.WriteLine("\nOffset Segment Measurements \n");
                for (int j = 0; j < subblocksMsr[i].lowerOffsetMargin.Length; j++)
                {
                    Console.WriteLine("\nLower Offset Segement Measurement {0}", j);

                    Console.WriteLine("Measurement Status              :{0}", subblocksMsr[i].lowerOffsetMeasurementStatus[j]);
                    Console.WriteLine("Margin (dB)                     :{0}", subblocksMsr[i].lowerOffsetMargin[j]);
                    Console.WriteLine("Margin Frequency (Hz)           :{0}", subblocksMsr[i].lowerOffsetMarginFrequency[j]);
                    Console.WriteLine("Margin Absolute Power (dBm)     :{0}", subblocksMsr[i].lowerOffsetMarginAbsolutePower[j]);

                    Console.WriteLine("\nUpper Offset Segement Measurement {0}", j);

                    Console.WriteLine("Measurement Status              :{0}", subblocksMsr[i].upperOffsetMeasurementStatus[j]);
                    Console.WriteLine("Margin (dB)                     :{0}", subblocksMsr[i].upperOffsetMargin[j]);
                    Console.WriteLine("Margin Frequency (Hz)           :{0}", subblocksMsr[i].upperOffsetMarginFrequency[j]);
                    Console.WriteLine("Margin Absolute Power (dBm)     :{0}", subblocksMsr[i].upperOffsetMarginAbsolutePower[j]);
                }
            }
        }

        static void DisplayError(Exception ex)
        {
            Console.WriteLine("ERROR:\n" + ex.GetType() + ": " + ex.Message);
        }

    }
}
