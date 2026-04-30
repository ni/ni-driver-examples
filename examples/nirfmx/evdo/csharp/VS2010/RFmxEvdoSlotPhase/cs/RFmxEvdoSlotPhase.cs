//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Channel Configuration Mode.
//6. Configure Physical Layer Subtype.
//7. Configure Uplink Data Modulation Type. 
//8. Configure Uplink Spreading Parameters.
//9. Select SlotPhase measurement and enable Traces. 
//10. Configure Synchronization Mode and Interval
//11. Initiate the Measurement.
//12. Fetch SlotPhase Measurements and Traces.
//13. Close RFmx Session.  

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.EvdoMX;

namespace NationalInstruments.Examples.RFmxEvdoSlotPhase
{
    public class RFmxEvdoSlotPhase
    {
        RFmxInstrMX instrSession;
        RFmxEvdoMX evdo;

        string resourceName;
        RFmxEvdoMXMeasurementTypes measurement;
        string frequencyReferenceSource;
        double frequencyReferenceFrequency;                                /* Hz */
        double centerFrequency;                                            /* Hz */
        double externalAttenuation;                                        /* dB */

        string digitalEdgeSource;
        RFmxEvdoMXDigitalEdgeTriggerEdge digitalEdge;
        double triggerDelay;                                               /* seconds */
        double referenceLevel;                                             /* dBm */

        RFmxEvdoMXChannelConfigurationMode channelConfigurationMode;
        RFmxEvdoMXSlotPhaseSynchronizationMode synchronizationMode;
        int measurementOffset;
        int measurementLength;
        RFmxEvdoMXPhysicalLayerSubtype physicalLayerSubtype;
        RFmxEvdoMXUplinkDataModulationType uplinkDataModulationType;
        long uplinkSpreadingIMask;
        long uplinkSpreadingQMask;

        bool enableAllTraces;
        bool enableTrigger;
        double timeout;

        double maximumHalfSlotPhaseDiscontinuity;
        double[] halfSlotPhaseDiscontinuity = null;
        AnalogWaveform<float> chipPhaseError = null;
        AnalogWaveform<float> chipPhaseErrorLinearFit = null;

        public void Run()
        {
            try
            {
                InitializeVariables();
                InitializeInstr();
                ConfigureEvdo();
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
            measurement = RFmxEvdoMXMeasurementTypes.SlotPhase;
            frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
            frequencyReferenceFrequency = 10.0e+6;                                  /* Hz */
            centerFrequency = 833.49e+6;                                            /* Hz */
            externalAttenuation = 0.000000;                                         /* dB */

            digitalEdgeSource = RFmxEvdoMXConstants.Pfi0;
            digitalEdge = RFmxEvdoMXDigitalEdgeTriggerEdge.Rising;
            triggerDelay = 0.000000;                                                /* seconds */
            referenceLevel = 0.000000;                                              /* dBm */

            channelConfigurationMode = RFmxEvdoMXChannelConfigurationMode.AutoDetect;
            synchronizationMode = RFmxEvdoMXSlotPhaseSynchronizationMode.Slot;
            measurementOffset = 0;                                                  /* slots */
            measurementLength = 16;                                                 /* slots */
            physicalLayerSubtype = RFmxEvdoMXPhysicalLayerSubtype.Subtype0_1;
            uplinkDataModulationType = RFmxEvdoMXUplinkDataModulationType.Auto;
            uplinkSpreadingIMask = 0;
            uplinkSpreadingQMask = 0;

            enableAllTraces = true;
            enableTrigger = false;
            timeout = 10.0;
        }

        void InitializeInstr()
        {
            /* Create a new RFmx Session */
            instrSession = new RFmxInstrMX(resourceName, "");
        }

        void ConfigureEvdo()
        {
            evdo = instrSession.GetEvdoSignalConfiguration();
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
            evdo.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
            evdo.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger);
            evdo.ConfigureChannelConfigurationMode("", channelConfigurationMode);
            evdo.ConfigurePhysicalLayerSubtype("", physicalLayerSubtype);
            evdo.ConfigureUplinkDataModulationType("", uplinkDataModulationType);
            evdo.SelectMeasurements("", measurement, enableAllTraces);
            evdo.ConfigureUplinkSpreading("", uplinkSpreadingIMask, uplinkSpreadingQMask);
            evdo.SlotPhase.Configuration.ConfigureSynchronizationModeAndInterval("", synchronizationMode,
                measurementOffset, measurementLength);
            evdo.Initiate("", "");
        }

        void RetrieveResults()
        {
            evdo.SlotPhase.Results.FetchMaximumHalfSlotPhaseDiscontinuity("", timeout, out maximumHalfSlotPhaseDiscontinuity);
            evdo.SlotPhase.Results.FetchPhaseDiscontinuities("", timeout, ref halfSlotPhaseDiscontinuity);
            evdo.SlotPhase.Results.FetchChipPhaseErrorTrace("", timeout, ref chipPhaseError);
            evdo.SlotPhase.Results.FetchChipPhaseErrorLinearFitTrace("", timeout, ref chipPhaseErrorLinearFit);
        }

        void PrintResults()
        {
            Console.WriteLine("\n-------------- Slot Phase Results --------------\n");
            Console.WriteLine("Maximum Half Slot Phase Discontinuity (deg)         : {0}\n", maximumHalfSlotPhaseDiscontinuity);

            for (int i = 0; i < 2*measurementLength; i++)
            {
                Console.WriteLine("\nSlot Number {0}", i);
                Console.WriteLine("Half Slot Phase Discontinuity (deg)                 : {0}", halfSlotPhaseDiscontinuity[i]);
            }

        }

        void CloseSession()
        {
            if (evdo != null)
            {
                evdo.Dispose();
                evdo = null;
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