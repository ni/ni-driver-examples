//Steps:
//1.Open a new RFmx Session.
//2.Configure Frequency Reference.
//3.Configure basic signal properties (Reference Level and External Attenuation).
//4.Configure Trigger Type and Trigger Parameters.
//5[A-J]. Configure Subblock Parameters.
//6.Select SEM measurement and enable Traces.
//7.Configure Sweep Time Parameters.
//8.Configure Averaging Parameters for SEM measurement.
//9.Configure Standard Mask Type.
//10. Configure Subblock Offset Segments.
//11. Initiate the Measurement.
//12. Fetch SEM Measurements and Traces.
//13. Close RFmx Session. 

//Step 5 :
//A.Configure Number of Subblocks.
//B.Configure subblock Frequency.
//C.Configure Component Carrier Spacing.
//D.Configure Number of Component Carriers.
//F.Configure Component Carriers (Component Carrier Frequency and Component Carrier Bandwidth).
//E.Configure Number of Offsets.
//G.Configure Offset Frequency.
//H.Configure Offset RBW Filter.
//I.Configure Offset Bandwidth Integral.
//J.Configure Offset Absolute Limit.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.LteMX;

namespace NationalInstruments.Examples.RFmxLteSemAdvancedNonContiguousMultiCarrier
{
    /* Input: Subblock inputs structure */
    struct SubblockInput
    {
	    public double subblockFrequency;                        /*(Hz) */
        public RFmxLteMXComponentCarrierSpacingType componentCarrierSpacingType;
        public int componentCarrierAtCenterFrequency;
        public double[] componentCarrierBandwidth;              /*(Hz) */
        public double[] componentCarrierFrequency;              /*(Hz) */
        public double[] componentCarrierMaximumOutputPower;     /*(dBm) */
        public double[] startFrequency;                         /*(Hz) */
        public double[] stopFrequency;                          /*(Hz) */
        public RFmxLteMXSemOffsetSideband[] sideband;
        public double[] RBW;                                    /*(Hz) */
        public RFmxLteMXSemOffsetRbwFilterType[] RBWFilterType;
        public int[] bandwidthIntegral;
        public double[] offsetAbsoluteLimitStart;               /*(dBm) */
        public double[] offsetAbsoluteLimitStop;                /*(dBm) */
		public double[] offsetRelativeLimitStart;               /*(dBm) */
        public double[] offsetRelativeLimitStop;                /*(dBm) */
        public RFmxLteMXSemOffsetLimitFailMask[] offsetLimitFailMask;
    }

    /*  Subblock measurement outputs structure */
    struct SubblockMeasurement
    {
        public double subblockPower;                            /*(dBm) */
        public double integrationBandwidth;                     /*(Hz) */
        public double frequency;                                /*(Hz) */
        public RFmxLteMXSemLowerOffsetMeasurementStatus[] lowerOffsetMeasurementStatus;
        public double[] lowerOffsetMargin;                      /*(dB) */
        public double[] lowerOffsetMarginFrequency;             /*(Hz) */
        public double[] lowerOffsetMarginAbsolutePower;         /*(dBm) */
        public RFmxLteMXSemUpperOffsetMeasurementStatus[] upperOffsetMeasurementStatus;
        public double[] upperOffsetMargin;                      /*(dB) */
        public double[] upperOffsetMarginFrequency;             /*(Hz) */
        public double[] upperOffsetMarginAbsolutePower;         /*(dBm) */
    }

    public class RFmxLteSemAdvancedNonContiguousMultiCarrier
    {
        RFmxInstrMX instrSession;
        RFmxLteMX lte;
        string rfsaResourceName;

        const int NumberOfSubblocks = 2;
        const int NumberOfComponentCarrier = 1;
        const int NumberOfOffsetSegments = 4;

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

        SubblockInput[] subblocks;
        SubblockMeasurement[] subblocksMsr;

        RFmxLteMXSemSweepTimeAuto sweepTimeAuto;
        double sweepTimeInterval;

        RFmxLteMXSemAveragingEnabled averagingEnabled;
        int averagingCount;
        RFmxLteMXSemAveragingType averagingType;
        
        RFmxLteMXSemMeasurementStatus measurementStatus;

        double[] relativePower;

        double timeout;
        double totalAggregatedPower;

        Spectrum<float> spectrum;
        Spectrum<float> absoluteMask;



        string subblockString;

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
            frequencyReferenceFrequency = 10e6;                 /* (Hz) */

            centerFrequency = 1.95e9;                           /* (Hz) */
            referenceLevel = 0.00;                              /* (dBm) */
            externalAttenuation = 0.0;                          /* (dBm) */

            enableTrigger = false;
            digitalEdgeSource = RFmxLteMXConstants.Pfi0;
            digitalEdge = RFmxLteMXDigitalEdgeTriggerEdge.Rising;
            triggerDelay = 0.0;                                 /* (s) */

			linkDirection = RFmxLteMXLinkDirection.Uplink;
            uplinkMaskType = RFmxLteMXSemUplinkMaskType.General_NS01;
            eNodeBCategory = RFmxLteMXeNodeBCategory.WideAreaBaseStationCategoryA;
            deltaFMaximum = 15.00e6;                            /* (Hz) */
            aggregatedMaximumPower = 0.00;                      /* (dBm) */

            downlinkMaskType = RFmxLteMXSemDownlinkMaskType.ENodeBCategoryBased;

            subblocksMsr = new SubblockMeasurement[NumberOfSubblocks];
            subblocks = new SubblockInput[NumberOfSubblocks] {
                                                               new SubblockInput { 
                                                                                   subblockFrequency = 0.0,
                                                                                   componentCarrierSpacingType =  RFmxLteMXComponentCarrierSpacingType.Nominal,
                                                                                   componentCarrierAtCenterFrequency = -1,
                                                                                   componentCarrierBandwidth = new double[NumberOfComponentCarrier] {20e+6},
                                                                                   componentCarrierFrequency = new double[NumberOfComponentCarrier] {0.00},
																				   componentCarrierMaximumOutputPower =new double[NumberOfComponentCarrier]{0.00},
                                                                                   startFrequency = new double[NumberOfOffsetSegments] {15e+3,1.5e+6,5.5e+6,20.5e+6},
                                                                                   stopFrequency = new double[NumberOfOffsetSegments] {985e+3,4.5e+6,19.5e+6,24.5e+6},
                                                                                   sideband = new RFmxLteMXSemOffsetSideband[NumberOfOffsetSegments] {RFmxLteMXSemOffsetSideband.Both,
                                                                                                                                                      RFmxLteMXSemOffsetSideband.Both,
                                                                                                                                                      RFmxLteMXSemOffsetSideband.Both,
                                                                                                                                                      RFmxLteMXSemOffsetSideband.Both},
                                                                                   RBW = new double[NumberOfOffsetSegments] {10e+3,250e+3,250e+3,250e+3},
                                                                                   RBWFilterType = new RFmxLteMXSemOffsetRbwFilterType[NumberOfOffsetSegments] {RFmxLteMXSemOffsetRbwFilterType.Gaussian,
                                                                                                                                                               RFmxLteMXSemOffsetRbwFilterType.Gaussian,
                                                                                                                                                               RFmxLteMXSemOffsetRbwFilterType.Gaussian,
                                                                                                                                                               RFmxLteMXSemOffsetRbwFilterType.Gaussian
                                                                                   
                                                                                   },
                                                                                   bandwidthIntegral = new int[NumberOfOffsetSegments] {3,4,4,4},
                                                                                   offsetAbsoluteLimitStart = new double[NumberOfOffsetSegments] {-19.5,-8.5,-11.5,-23.5},
                                                                                   offsetAbsoluteLimitStop = new double[NumberOfOffsetSegments] {-19.5,-8.5,-11.5,-23.5},
                                                                                   offsetRelativeLimitStart = new double[NumberOfOffsetSegments]{-51.50,-51.50,-51.50,-51.50},
																				   offsetRelativeLimitStop = new double[NumberOfOffsetSegments]{-58.50,-58.50,-58.50,-58.50},
                                                                                   offsetLimitFailMask = new RFmxLteMXSemOffsetLimitFailMask[NumberOfOffsetSegments]{RFmxLteMXSemOffsetLimitFailMask.Absolute,
                                                                                                                                                                     RFmxLteMXSemOffsetLimitFailMask.Absolute,
                                                                                                                                                                     RFmxLteMXSemOffsetLimitFailMask.Absolute,
                                                                                                                                                                     RFmxLteMXSemOffsetLimitFailMask.Absolute}
														                         },
                                                               new SubblockInput { 
                                                                                   subblockFrequency = 30e6, 
                                                                                   componentCarrierSpacingType =  RFmxLteMXComponentCarrierSpacingType.Nominal,
                                                                                   componentCarrierAtCenterFrequency = -1,
                                                                                   componentCarrierBandwidth = new double[NumberOfComponentCarrier] {20e+6},
                                                                                   componentCarrierFrequency = new double[NumberOfComponentCarrier] {0.00},
																				   componentCarrierMaximumOutputPower =new double[NumberOfComponentCarrier]{0.00},
                                                                                   startFrequency = new double[NumberOfOffsetSegments] {15e+3,1.5e+6,5.5e+6,20.5e+6},
                                                                                   stopFrequency = new double[NumberOfOffsetSegments] {985e+3,4.5e+6,19.5e+6,24.5e+6},
                                                                                   sideband = new RFmxLteMXSemOffsetSideband[NumberOfOffsetSegments] {RFmxLteMXSemOffsetSideband.Both,
                                                                                                                                                      RFmxLteMXSemOffsetSideband.Both,
                                                                                                                                                      RFmxLteMXSemOffsetSideband.Both,
                                                                                                                                                      RFmxLteMXSemOffsetSideband.Both},
                                                                                   RBW = new double[NumberOfOffsetSegments] {10e+3,250e+3,250e+3,250e+3},
                                                                                   RBWFilterType = new RFmxLteMXSemOffsetRbwFilterType[NumberOfOffsetSegments] {RFmxLteMXSemOffsetRbwFilterType.Gaussian,
                                                                                                                                                               RFmxLteMXSemOffsetRbwFilterType.Gaussian,
                                                                                                                                                               RFmxLteMXSemOffsetRbwFilterType.Gaussian,
                                                                                                                                                               RFmxLteMXSemOffsetRbwFilterType.Gaussian},
                                                                                   bandwidthIntegral = new int[NumberOfOffsetSegments] {3,4,4,4},
                                                                                   offsetAbsoluteLimitStart = new double[NumberOfOffsetSegments] {-19.5,-8.5,-11.5,-23.5},
                                                                                   offsetAbsoluteLimitStop = new double[NumberOfOffsetSegments] {-19.5,-8.5,-11.5,-23.5},
                                                                                   offsetRelativeLimitStart = new double[NumberOfOffsetSegments]{-51.50,-51.50,-51.50,-51.50},
																				   offsetRelativeLimitStop = new double[NumberOfOffsetSegments]{-58.50,-58.50,-58.50,-58.50},
                                                                                   offsetLimitFailMask = new RFmxLteMXSemOffsetLimitFailMask[NumberOfOffsetSegments]{RFmxLteMXSemOffsetLimitFailMask.Absolute,
                                                                                                                                                                     RFmxLteMXSemOffsetLimitFailMask.Absolute,
                                                                                                                                                                     RFmxLteMXSemOffsetLimitFailMask.Absolute,
                                                                                                                                                                     RFmxLteMXSemOffsetLimitFailMask.Absolute}
														                         },
                                                            };
            sweepTimeAuto = RFmxLteMXSemSweepTimeAuto.True;
            sweepTimeInterval = 0.001;                          /* (s) */
            averagingEnabled = RFmxLteMXSemAveragingEnabled.False;
            averagingCount = 10;
            averagingType = RFmxLteMXSemAveragingType.Rms;
            timeout = 10;
        }


        void ConfigureLte()
        {
            lte = instrSession.GetLteSignalConfiguration();     /* Create a new RFmx Session */
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);

            lte.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);

            lte.ConfigureDigitalEdgeTrigger("", digitalEdgeSource,
                                                         digitalEdge, triggerDelay, enableTrigger);

            lte.ConfigureNumberOfSubblocks("", NumberOfSubblocks);
            for (int i = 0; i < NumberOfSubblocks; i++)
            {
                subblockString = RFmxLteMX.BuildSubblockString("", i);
                lte.SetSubblockFrequency(subblockString, subblocks[i].subblockFrequency);
                lte.ComponentCarrier.ConfigureSpacing(subblockString, subblocks[i].componentCarrierSpacingType, subblocks[i].componentCarrierAtCenterFrequency);
                lte.ConfigureNumberOfComponentCarriers(subblockString, NumberOfComponentCarrier);
                lte.ComponentCarrier.ConfigureArray(subblockString, subblocks[i].componentCarrierBandwidth, subblocks[i].componentCarrierFrequency,
                                                    null);
                lte.Sem.Configuration.ConfigureNumberOfOffsets(subblockString, NumberOfOffsetSegments);
                lte.Sem.Configuration.ConfigureOffsetFrequencyArray(subblockString, subblocks[i].startFrequency, subblocks[i].stopFrequency,
                                                                    subblocks[i].sideband);
                lte.Sem.Configuration.ConfigureOffsetRbwFilterArray(subblockString, subblocks[i].RBW, subblocks[i].RBWFilterType);
                lte.Sem.Configuration.ConfigureOffsetBandwidthIntegralArray(subblockString, subblocks[i].bandwidthIntegral);
                lte.Sem.Configuration.ConfigureOffsetAbsoluteLimitArray(subblockString, subblocks[i].offsetAbsoluteLimitStart,
                                                                        subblocks[i].offsetAbsoluteLimitStop);
                lte.Sem.Configuration.ConfigureOffsetRelativeLimitArray(subblockString, subblocks[i].offsetRelativeLimitStart,
                                                                       subblocks[i].offsetRelativeLimitStop);
                lte.Sem.Configuration.ConfigureOffsetLimitFailMaskArray(subblockString, subblocks[i].offsetLimitFailMask);

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
                for (int i = 0; i < NumberOfSubblocks; i++)
                {
                    subblockString = RFmxLteMX.BuildSubblockString("", i);
                    lte.Sem.Configuration.ComponentCarrier.ConfigureMaximumOutputPowerArray(subblockString, subblocks[i].componentCarrierMaximumOutputPower);
                }
            }
            lte.Initiate("", "");
         }

        void RetrieveResults()
        {
            for (int i = 0; i < NumberOfSubblocks; i++)
            {
                subblockString = RFmxLteMX.BuildSubblockString("", i);
                lte.Sem.Results.FetchUpperOffsetMarginArray(subblockString, timeout, ref subblocksMsr[i].upperOffsetMeasurementStatus,
                                                            ref subblocksMsr[i].upperOffsetMargin, ref subblocksMsr[i].upperOffsetMarginFrequency,
                                                            ref subblocksMsr[i].upperOffsetMarginAbsolutePower, ref relativePower);
                lte.Sem.Results.FetchLowerOffsetMarginArray(subblockString, timeout, ref subblocksMsr[i].lowerOffsetMeasurementStatus,
                                                            ref subblocksMsr[i].lowerOffsetMargin, ref subblocksMsr[i].lowerOffsetMarginFrequency,
                                                            ref subblocksMsr[i].lowerOffsetMarginAbsolutePower, ref relativePower);
                lte.Sem.Results.FetchSubblockMeasurement(subblockString, timeout, out subblocksMsr[i].subblockPower, out subblocksMsr[i].integrationBandwidth,
                                                         out subblocksMsr[i].frequency);
            }
            lte.Sem.Results.FetchTotalAggregatedPower("", timeout, out totalAggregatedPower);
            lte.Sem.Results.FetchMeasurementStatus("", timeout, out measurementStatus);
            lte.Sem.Results.FetchSpectrum("", timeout, ref spectrum, ref absoluteMask);
        }

        void PrintResults()
        {
            Console.WriteLine("Total Aggregated Power (dBm)                  :{0}", totalAggregatedPower);
            Console.WriteLine("Measurement Status                            :{0}", measurementStatus);
            Console.WriteLine("Subblock Measurements\n");
            for (int i = 0; i < subblocksMsr.Length; i++)
            {
                Console.WriteLine("Subblock  {0}\n", i);
                Console.WriteLine("Subblock Power (dBm)                           :{0}", subblocksMsr[i].subblockPower);
                Console.WriteLine("Integration Bandwidth (Hz)                     :{0}", subblocksMsr[i].integrationBandwidth);
                Console.WriteLine("Frequency (Hz)                                 :{0}\n", subblocksMsr[i].frequency);
                for (int j = 0; j < subblocksMsr[i].lowerOffsetMargin.Length; j++)
                {
                    Console.WriteLine("Offset measurement   {0}\n", j);
                    Console.WriteLine("Lower Offset Segement Measurement  ");
                    Console.WriteLine("Measurement Status                             :{0}", subblocksMsr[i].lowerOffsetMeasurementStatus[j]);
                    Console.WriteLine("Margin (dB)                                    :{0}", subblocksMsr[i].lowerOffsetMargin[j]);
                    Console.WriteLine("Margin Frequency (Hz)                          :{0}", subblocksMsr[i].lowerOffsetMarginFrequency[j]);
                    Console.WriteLine("Margin Absolute Power (dBm)                    :{0}", subblocksMsr[i].lowerOffsetMarginAbsolutePower[j]);
                    Console.WriteLine("Upper Offset Segement Measurement  ");
                    Console.WriteLine("Measurement Status                             :{0}", subblocksMsr[i].upperOffsetMeasurementStatus[j]);
                    Console.WriteLine("Margin (dB)                                    :{0}", subblocksMsr[i].upperOffsetMargin[j]);
                    Console.WriteLine("Margin Frequency (Hz)                          :{0}", subblocksMsr[i].upperOffsetMarginFrequency[j]);
                    Console.WriteLine("Margin Absolute Power (dBm)                    :{0}\n", subblocksMsr[i].upperOffsetMarginAbsolutePower[j]);
                }
            }
        }
        static void DisplayError(Exception ex)
        {
            Console.WriteLine("ERROR:\n" + ex.GetType() + ": " + ex.Message);
        }

    }
}








