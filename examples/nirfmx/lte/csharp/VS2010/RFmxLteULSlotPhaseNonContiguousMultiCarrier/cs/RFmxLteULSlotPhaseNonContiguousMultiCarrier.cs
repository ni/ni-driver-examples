// Steps:
// 1.Open a new RFmx Session.
// 2.Configure Frequency Reference.
// 3.Configure basic signal properties (Reference Level and External Attenuation).
// 4.Configure Trigger Type and Trigger Parameters.
// 5. Configure Subblock Configurations.
// 5A.Configure Number of Subblocks.
// 5B.Configure Subblock Frequency.
// 5C.Configure Component Carrier Spacing.
// 5D.Configure Number of Component Carriers.
// 5E.Configure Component Carriers (Component Carrier Frequency and Component Carrier Bandwidth).
// 6.Configure Duplex Scheme.
// 7.Select SlotPhase measurement and enable Traces.
// 8.Configure Measurement Interval.
// 9.Initiate the Measurement.
// 10. Fetch SlotPhase Measurements and Traces.
// 11. Close RFmx Session. 

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.LteMX;

namespace NationalInstruments.Examples.RFmxLteULSlotPhaseNonContiguousMultiCarrier
{
    public class RFmxLteULSlotPhaseNonContiguousMultiCarrier
    {
        RFmxInstrMX instrSession;
        RFmxLteMX lte;
        string resourceName;
        string frequencyReferenceSource;
        double frequencyReferenceFrequency;

        double centerFrequency;
        double referenceLevel;
        double externalAttenuation;

        bool enableTrigger;
        string digitalEdgeTriggerSource;
        RFmxLteMXDigitalEdgeTriggerEdge digitalEdgeTriggerEdge;
        double triggerDelay;

        const int numberOfSlots = 20;
        const int numberOfSubblocks = 2;
        const int numberOfComponentCarriers = 1;

        RFmxLteMXDuplexScheme duplexScheme;
        RFmxLteMXSlotPhaseSynchronizationMode synchronizationMode;
        int measurementOffset;
        int measurementLength;

        /* Subblock inputs structure */
        struct SubblockInput
        {
	        public double subblockFrequency;                                     /*(Hz) */
            public RFmxLteMXComponentCarrierSpacingType componentCarrierSpacingType;
            public int componentCarrierAtCenterFrequency;
            public double[] componentCarrierBandwidth;                           /*(Hz) */
            public double[] componentCarrierFrequency;                           /*(Hz) */
            public int[] cellID;
        };

        /* Subblock measurement outputs structure */
        struct SubblockMeasurement
        {
            public double[] maximumPhaseDiscontinuity;                           /*(deg)*/
            public double[] slotPhaseDiscontinuity;                                  /*(dBm) */
            public AnalogWaveform<float> samplePhaseError;                       /* deg */
            public AnalogWaveform<float> samplePhaseErrorLinearFit;              /* deg */
        };

        SubblockInput[] subblockInput = new SubblockInput[numberOfSubblocks] {
                                                                    new SubblockInput { 
                                                                                            subblockFrequency = 0.0,
                                                                                            componentCarrierSpacingType = RFmxLteMXComponentCarrierSpacingType.Nominal, 
                                                                                            componentCarrierAtCenterFrequency = -1, 
                                                                                            componentCarrierBandwidth = new double[numberOfComponentCarriers] {20e6}, 
                                                                                            componentCarrierFrequency = new double[numberOfComponentCarriers] {0.0},
                                                                                            cellID = new int[numberOfComponentCarriers]{0},
                                                                                      },
                                                                    new SubblockInput {
                                                                                            subblockFrequency = 30e6,
                                                                                            componentCarrierSpacingType = RFmxLteMXComponentCarrierSpacingType.Nominal, 
                                                                                            componentCarrierAtCenterFrequency = -1, 
                                                                                            componentCarrierBandwidth = new double[numberOfComponentCarriers] {20e6}, 
                                                                                            componentCarrierFrequency =  new double[numberOfComponentCarriers] {0.0},
                                                                                            cellID = new int[numberOfComponentCarriers]{1},
                                                                                      }
                                                                };
        SubblockMeasurement[] subblockMsr = new SubblockMeasurement[numberOfSubblocks];

        string[] subblockString = new string[numberOfSubblocks], subblockCarrierString = new string[numberOfSubblocks];
        RFmxLteMXUplinkDownlinkConfiguration uplinkDownlinkConfiguraiton;
        double timeout;


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

        private void InitializeInstr()
        {
            /* Create a new RFmx Session */
            instrSession = new RFmxInstrMX(resourceName, "");
        }

        void InitializeVariables()
        {
            resourceName = "RFSA";
            frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
            frequencyReferenceFrequency = 10e6;                                  /* (Hz) */

            centerFrequency = 1.95e9;                                            /* (Hz) */
            referenceLevel = 0.00;                                               /* (dBm) */
            externalAttenuation = 0.0;                                           /* (dBm) */

            enableTrigger = false;
            digitalEdgeTriggerSource = RFmxLteMXConstants.Pfi0;
            digitalEdgeTriggerEdge = RFmxLteMXDigitalEdgeTriggerEdge.Rising;
            triggerDelay = 0.0;                                                  /* (s) */

            duplexScheme = RFmxLteMXDuplexScheme.Fdd;
            synchronizationMode = RFmxLteMXSlotPhaseSynchronizationMode.Slot;
            measurementOffset = 0;                                               /* slots */
            measurementLength = numberOfSlots;                                   /* slots */

            uplinkDownlinkConfiguraiton = RFmxLteMXUplinkDownlinkConfiguration.Configuration0;
            timeout = 10.0;                                                      /* (s) */
        }

        private void ConfigureLte()
        {
            /* Get Lte signal */
            lte = instrSession.GetLteSignalConfiguration();

            /* Configure measurement */
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
            lte.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
            lte.ConfigureDigitalEdgeTrigger("", digitalEdgeTriggerSource, digitalEdgeTriggerEdge, triggerDelay, enableTrigger);
            lte.ConfigureNumberOfSubblocks("", numberOfSubblocks);

            for (int i = 0; i < numberOfSubblocks; i++)
            {
                subblockString[i] = RFmxLteMX.BuildSubblockString("", i);
                lte.SetSubblockFrequency(subblockString[i], subblockInput[i].subblockFrequency);
                lte.ComponentCarrier.ConfigureSpacing(subblockString[i], subblockInput[i].componentCarrierSpacingType,
                                                      subblockInput[i].componentCarrierAtCenterFrequency);
                lte.ConfigureNumberOfComponentCarriers(subblockString[i], numberOfComponentCarriers);
                lte.ComponentCarrier.ConfigureArray(subblockString[i], subblockInput[i].componentCarrierBandwidth,
                                                    subblockInput[i].componentCarrierFrequency,
                                                     subblockInput[i].cellID);
            }

            lte.ConfigureDuplexScheme("", duplexScheme, uplinkDownlinkConfiguraiton);
            lte.SelectMeasurements("", RFmxLteMXMeasurementTypes.SlotPhase, true);
            lte.SlotPhase.Configuration.ConfigureSynchronizationModeAndInterval("", synchronizationMode, measurementOffset, measurementLength);
            lte.Initiate("", "");
        }

        private void RetrieveResults()
        {
            /* Retrieve results */
            for (int i = 0; i < numberOfSubblocks; i++)
            {
                lte.SlotPhase.Results.FetchMaximumPhaseDiscontinuityArray(subblockString[i], timeout, ref subblockMsr[i].maximumPhaseDiscontinuity);

                subblockCarrierString[i] = RFmxLteMX.BuildCarrierString(subblockString[i], 0);
                lte.SlotPhase.Results.FetchPhaseDiscontinuities(subblockCarrierString[i], timeout, ref subblockMsr[i].slotPhaseDiscontinuity);
                lte.SlotPhase.Results.FetchSamplePhaseError(subblockCarrierString[i], timeout, ref subblockMsr[i].samplePhaseError);
                lte.SlotPhase.Results.FetchSamplePhaseErrorLinearFitTrace(subblockCarrierString[i], timeout, ref subblockMsr[i].samplePhaseErrorLinearFit);
            }
        }

        private void PrintResults()
        {
            Console.WriteLine("********** Subblock Measurements ********** \n");
            for (int i = 0; i < numberOfSubblocks; i++)
            {
                Console.WriteLine("Subblock                             : {0}", i);
                for (int j = 0; j < numberOfComponentCarriers; j++)
                {
                    Console.WriteLine("Carrier                              : {0}", j);
                    Console.WriteLine("Maximum  Phase Discontinuity (deg)   : {0}\n", subblockMsr[i].maximumPhaseDiscontinuity[j]);
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
