//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Select SlotPhase measurement and enable Traces.
//6. Configure Radio Configuration and Uplink Spreading Long code mask.
//7. Configure Synchronization Mode and Interval
//8. Initiate the Measurement.
//9. Fetch SlotPhase Measurements and Traces.
//10. Close RFmx Session.  

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.Cdma2kMX;

namespace NationalInstruments.Examples.RFmxCdma2kSlotPhase
{
    public class RFmxCdma2kSlotPhase
    {
        RFmxInstrMX instrSession;
        RFmxCdma2kMX cdma2k;

        string resourceName;
        RFmxCdma2kMXMeasurementTypes measurement;
        string frequencyReferenceSource;
        double frequencyReferenceFrequency;                                /* Hz */
        double centerFrequency;                                            /* Hz */
        double externalAttenuation;                                        /* dB */

        string digitalEdgeSource;
        RFmxCdma2kMXDigitalEdgeTriggerEdge digitalEdge;
        double triggerDelay;                                               /* seconds */
        double referenceLevel;                                             /* dBm */

        RFmxCdma2kMXRadioConfiguration radioConfiguration;
        int uplinkSpreadingLongCodeMask;
        RFmxCdma2kMXSlotPhaseSynchronizationMode synchronizationMode;
        int measurementOffset;                                             /*slots*/
        int measurementLength;                                             /*slots*/


        bool enableAllTraces;
        bool enableTrigger;

        double timeout;

        double maximumPhaseDiscontinuity;
        double[] slotPhaseDiscontinuity = null;
        AnalogWaveform<float> chipPhaseError = null;
        AnalogWaveform<float> chipPhaseErrorLinearFit = null;



        public void Run()
        {
            try
            {
                InitializeVariables();
                InitializeInstr();
                ConfigureCdma2k();
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

        private void InitializeVariables()
        {
            /* Initialize input variables */

            resourceName = "RFSA";
            measurement = RFmxCdma2kMXMeasurementTypes.SlotPhase;
            frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
            frequencyReferenceFrequency = 10.0e+6;                                  /* Hz */
            centerFrequency = 833.49e+6;                                            /* Hz */
            externalAttenuation = 0.000000;                                         /* dB */

            digitalEdgeSource = RFmxCdma2kMXConstants.Pfi0;
            digitalEdge = RFmxCdma2kMXDigitalEdgeTriggerEdge.Rising;
            triggerDelay = 0.000000;                                                /* seconds */
            referenceLevel = 0.000000;                                              /* dBm */

            radioConfiguration = RFmxCdma2kMXRadioConfiguration.RC3;
            uplinkSpreadingLongCodeMask = 0;
            synchronizationMode = RFmxCdma2kMXSlotPhaseSynchronizationMode.Slot;
            measurementOffset = 0;                                                  /*slots*/
            measurementLength = 16;                                                 /*slots*/


            enableAllTraces = true;
            enableTrigger = false;

            timeout = 10.0;

        }

        void InitializeInstr()
        {
            /* Create a new RFmx Session */
            instrSession = new RFmxInstrMX(resourceName, "");
        }

        void ConfigureCdma2k()
        {
            cdma2k = instrSession.GetCdma2kSignalConfiguration();
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
            cdma2k.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
            cdma2k.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger);
            cdma2k.ConfigureRadioConfiguration("", radioConfiguration);
            cdma2k.ConfigureUplinkSpreading("", uplinkSpreadingLongCodeMask);
            cdma2k.SelectMeasurements("", measurement, enableAllTraces);                 
            cdma2k.SlotPhase.Configuration.ConfigureSynchronizationModeAndInterval("", synchronizationMode,
                measurementOffset, measurementLength);
            cdma2k.Initiate("", "");
        }

        void RetrieveResults()
        {
            cdma2k.SlotPhase.Results.FetchMaximumPhaseDiscontinuity("", timeout, out maximumPhaseDiscontinuity);
            cdma2k.SlotPhase.Results.FetchPhaseDiscontinuities("", timeout, ref slotPhaseDiscontinuity);
            cdma2k.SlotPhase.Results.FetchChipPhaseErrorTrace("", timeout, ref chipPhaseError);
            cdma2k.SlotPhase.Results.FetchChipPhaseErrorLinearFitTrace("", timeout, ref chipPhaseErrorLinearFit);
        }

        void PrintResults()
        {
            Console.WriteLine("Maximum Phase Discontinuity (deg)         : {0}\n", maximumPhaseDiscontinuity);

            for (int i = 0; i < measurementLength; i++)
            {
                Console.WriteLine("\nSlot Number                               : {0}", i);
                Console.WriteLine("Slot Phase Discontinuity (deg)            : {0}", slotPhaseDiscontinuity[i]);
            }

        }

        void CloseSession()
        {
            if (cdma2k != null)
            {
                cdma2k.Dispose();
                cdma2k = null;
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