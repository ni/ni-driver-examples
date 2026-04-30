// Steps:
// 1. Open a new RFmx Session.
// 2. Configure Frequency Reference.
// 3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
// 4. Configure Trigger Type and Trigger Parameters.
// 5. Configure Component Carrier Spacing.
// 6. Configure Component Carriers.
// 7. Configure Duplex Scheme.
// 8. Select SlotPhase measurement and enable Traces.
// 9. Configure Measurement Method.
// 10. Initiate the Measurement.
// 11. Fetch SlotPhase Measurements and Traces.
// 12. Close RFmx Session.  

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.LteMX;

namespace NationalInstruments.Examples.RFmxLteULSlotPhaseContiguousMultiCarrier
{
    public class RFmxLteULSlotPhaseContiguousMultiCarrier
    {
        RFmxInstrMX instrSession;
        RFmxLteMX lte;
        string resourceName;
        RFmxLteMXMeasurementTypes measurement;
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
        const int numberOfComponentCarriers = 2;

        RFmxLteMXDuplexScheme duplexScheme;
        RFmxLteMXSlotPhaseSynchronizationMode synchronizationMode;
        int measurementOffset;
        int measurementLength;

        RFmxLteMXComponentCarrierSpacingType componentCarrierSpacingType;
        int componentCarrierAtCenterFrequency;

        double[] componentCarrierBandwidth = { 20e6, 20e6 };                     /*(Hz) */
        double[] componentCarrierFrequency = { -9.9e6, 9.9e6 };                  /*(Hz) */
        int[] cellId = { 0, 1 };

        RFmxLteMXUplinkDownlinkConfiguration uplinkDownlinkConfiguraiton;
        string subblockCarrierString;
        double timeout;

        double[] maximumPhaseDiscontinuity;
        double[][] slotPhaseDiscontinuity = new double[numberOfComponentCarriers][];
        AnalogWaveform<float>[] samplePhaseError = new AnalogWaveform<float>[numberOfComponentCarriers];
        AnalogWaveform<float>[] samplePhaseErrorLinearFit = new AnalogWaveform<float>[numberOfComponentCarriers];


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
            instrSession = new RFmxInstrMX(resourceName, "");
        }

        void InitializeVariables()
        {
            resourceName = "RFSA";
            measurement = RFmxLteMXMeasurementTypes.SlotPhase;
            frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
            frequencyReferenceFrequency = 10e6;                                  /* (Hz) */

            centerFrequency = 1.95e9;                                            /* (Hz) */
            referenceLevel = 0.00;                                               /* (dBm) */
            externalAttenuation = 0.0;                                           /* (dB) */

            enableTrigger = false;
            digitalEdgeTriggerSource = RFmxLteMXConstants.Pfi0;
            digitalEdgeTriggerEdge = RFmxLteMXDigitalEdgeTriggerEdge.Rising;
            triggerDelay = 0.0;                                                  /* (s) */

            duplexScheme = RFmxLteMXDuplexScheme.Fdd;
            synchronizationMode = RFmxLteMXSlotPhaseSynchronizationMode.Slot;
            measurementOffset = 0;                                               /* slots */
            measurementLength = numberOfSlots;                                   /* slots */

            componentCarrierSpacingType = RFmxLteMXComponentCarrierSpacingType.Nominal;
            componentCarrierAtCenterFrequency = -1;
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
            lte.ComponentCarrier.ConfigureSpacing("", componentCarrierSpacingType, componentCarrierAtCenterFrequency);
            lte.ConfigureNumberOfComponentCarriers("", numberOfComponentCarriers);
            lte.ComponentCarrier.ConfigureArray("", componentCarrierBandwidth, componentCarrierFrequency, cellId);
            lte.ConfigureDuplexScheme("", duplexScheme, uplinkDownlinkConfiguraiton);
            lte.SelectMeasurements("", measurement, true);
            lte.SlotPhase.Configuration.ConfigureSynchronizationModeAndInterval("", synchronizationMode, measurementOffset, 
                                                                                measurementLength);
            lte.Initiate("", "");
        }

        private void RetrieveResults()
        {
            /* Retrieve results */
            lte.SlotPhase.Results.FetchMaximumPhaseDiscontinuityArray("", timeout, ref maximumPhaseDiscontinuity);

            for (int i = 0; i < numberOfComponentCarriers; i++)
            {
                subblockCarrierString = RFmxLteMX.BuildCarrierString("", i);
                lte.SlotPhase.Results.FetchPhaseDiscontinuities(subblockCarrierString, timeout, ref slotPhaseDiscontinuity[i]);
                lte.SlotPhase.Results.FetchSamplePhaseError(subblockCarrierString, timeout, ref samplePhaseError[i]);
                lte.SlotPhase.Results.FetchSamplePhaseErrorLinearFitTrace(subblockCarrierString, timeout, 
                                                                          ref samplePhaseErrorLinearFit[i]);

            }
        }

        private void PrintResults()
        {
            /* Retrieve results */
            for (int i = 0; i < numberOfComponentCarriers; i++)
            {
                Console.WriteLine("\nCarrier {0}", i);
                Console.WriteLine("Maximum  Phase Discontinuity (deg)   : {0}", maximumPhaseDiscontinuity[i]);
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
