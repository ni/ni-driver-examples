//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Radio Configuration and Uplink Spreading Long Code Mask.
//6. Select SlotPower measurement.
//7. Configure Synchronization Mode and Interval
//8. Initiate the Measurement.
//9. Fetch SlotPower Measurement.
//10 Close the RFmx Seesion
 

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.Cdma2kMX;

namespace NationalInstruments.Examples.RFmxCdma2kSlotPower
{
    public class RFmxCdma2kSlotPower
    {
        RFmxInstrMX instrSession;
        RFmxCdma2kMX cdma2k;

        string resourceName;
        RFmxCdma2kMXMeasurementTypes measurement;
        string frequencyReferenceSource;
        double frequencyReferenceFrequency;                                  /* Hz */
        double centerFrequency;                                              /* Hz */
        double externalAttenuation;                                          /* dB */

        string digitalEdgeSource;
        RFmxCdma2kMXDigitalEdgeTriggerEdge digitalEdge;
        double triggerDelay;                                                 /* seconds */
        double referenceLevel;                                               /* dBm */

        RFmxCdma2kMXRadioConfiguration radioConfiguration;
        int uplinkSpreadingLongCodeMask;
        RFmxCdma2kMXSlotPowerSynchronizationMode synchronizationMode;
        int measurementOffset;                                               /*slots*/
        int measurementLength;                                               /*slots*/

        bool enableAllTraces;
        bool enableTrigger;

        double timeout;


        double[] slotPower = null;
        double[] slotPowerDelta = null;



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
            measurement = RFmxCdma2kMXMeasurementTypes.SlotPower;
            frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
            frequencyReferenceFrequency = 10.0e+6;                           /* Hz */
            centerFrequency = 833.49e+6;                                     /* Hz */
            externalAttenuation = 0.000000;                                  /* dB */

            digitalEdgeSource = RFmxCdma2kMXConstants.Pfi0;
            digitalEdge = RFmxCdma2kMXDigitalEdgeTriggerEdge.Rising;
            triggerDelay = 0.000000;                                         /* seconds */
            referenceLevel = 0.000000;                                       /* dBm */

            synchronizationMode = RFmxCdma2kMXSlotPowerSynchronizationMode.Slot;
            measurementOffset = 0;                                           /*slots*/
            measurementLength = 16;                                          /*slots*/

            radioConfiguration = RFmxCdma2kMXRadioConfiguration.RC3;
            uplinkSpreadingLongCodeMask = 0;

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
            cdma2k.SlotPower.Configuration.ConfigureSynchronizationModeAndInterval("", synchronizationMode, measurementOffset, measurementLength);
            cdma2k.Initiate("", "");
        }

        void RetrieveResults()
        {
            cdma2k.SlotPower.Results.FetchPowers("", timeout, ref slotPower, ref slotPowerDelta);
        }

        void PrintResults()
        {
            Console.WriteLine("------------Slot Powers------------\n");
            for (int i = 0; i < measurementLength; i++)
            {
                Console.WriteLine("\nSlot Number                       : {0}", i);
                Console.WriteLine("Slot Power (dBm)                  : {0}", slotPower[i]);
                Console.WriteLine("Slot Power Delta (dB)             : {0}", slotPowerDelta[i]);
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