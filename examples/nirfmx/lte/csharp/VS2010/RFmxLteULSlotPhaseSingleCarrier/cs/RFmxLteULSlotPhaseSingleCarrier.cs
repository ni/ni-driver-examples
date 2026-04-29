//Steps:
//1.Open a new RFmx Session.
//2.Configure Frequency Reference.
//3.Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4.Configure Trigger Type and Trigger Parameters.
//5.Configure Carrier Bandwidth.
//6.Configure Duplex Scheme.
//7.Select SlotPhase measurement and enable Traces.
//8.Configure Synchronization Mode and Interval
//9. Initiate the Measurement.
//10. Fetch SlotPhase  Traces and Measurements.
//11. Close RFmx Session.  

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.LteMX;

namespace NationalInstruments.Examples.RFmxLteULSlotPhaseSingleCarrier
{
    public class RFmxLteULSlotPhaseSingleCarrier
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

        RFmxLteMXDuplexScheme duplexScheme;
        RFmxLteMXUplinkDownlinkConfiguration uplinkDownlinkConfiguration;
        double componentCarrierBandwidth;
        double componentCarrierFrequency;
        int cellID;
        const int numberOfSlots = 20;

        RFmxLteMXSlotPhaseSynchronizationMode synchronizationMode;
        int measurementOffset;
        int measurementLength;

        double timeout;

        double maximumPhaseDiscontinuity;
        double[] slotPhaseDiscontinuity;
        AnalogWaveform<float> samplePhaseError;
        AnalogWaveform<float> samplePhaseErrorLinearFit;


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
            uplinkDownlinkConfiguration = RFmxLteMXUplinkDownlinkConfiguration.Configuration0;
            componentCarrierBandwidth = 10e6;                                    /* (Hz) */
            componentCarrierFrequency = 0.0;                                     /* (Hz) */
            cellID = 0;

            timeout = 10.0;                                                      /* (s) */
            measurementOffset = 0;                                               /* slots */
            measurementLength = numberOfSlots;                                   /* slots */
            synchronizationMode = RFmxLteMXSlotPhaseSynchronizationMode.Slot;

        }

        private void ConfigureLte()
        {
            /* Get Lte signal */
            lte = instrSession.GetLteSignalConfiguration();

            /* Configure measurement */
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
            lte.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
            lte.ConfigureDigitalEdgeTrigger("", digitalEdgeTriggerSource, digitalEdgeTriggerEdge, triggerDelay, enableTrigger);
            lte.ComponentCarrier.Configure("", componentCarrierBandwidth,
                                                componentCarrierFrequency, cellID);
            lte.SelectMeasurements("", measurement, true);
            lte.ConfigureDuplexScheme("", duplexScheme, uplinkDownlinkConfiguration);
            lte.SlotPhase.Configuration.ConfigureSynchronizationModeAndInterval("", synchronizationMode, measurementOffset, measurementLength);
            lte.Initiate("", "");
        }

        private void RetrieveResults()
        {
            /* Retrieve results */
            lte.SlotPhase.Results.FetchPhaseDiscontinuities("", timeout, ref slotPhaseDiscontinuity);
            lte.SlotPhase.Results.FetchMaximumPhaseDiscontinuity("", timeout, out maximumPhaseDiscontinuity);
            lte.SlotPhase.Results.FetchSamplePhaseError("", timeout, ref samplePhaseError);
            lte.SlotPhase.Results.FetchSamplePhaseErrorLinearFitTrace("", timeout, ref samplePhaseErrorLinearFit);
        }

        private void PrintResults()
        {
            /* Retrieve results */
            Console.WriteLine("Maximum  Phase Discontinuity (deg) : {0}", maximumPhaseDiscontinuity);

            for (int i = 0; i < measurementLength; i++)
            {
                Console.WriteLine("\nSlot Number {0}", i);
                Console.WriteLine("Slot Phase Discontinuities (deg)   : {0}", slotPhaseDiscontinuity[i]);
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
