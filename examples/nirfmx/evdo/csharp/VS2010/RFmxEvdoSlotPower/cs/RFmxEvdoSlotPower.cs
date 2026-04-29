//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Uplink Spreading Parameters.
//6. Select SlotPower measurement.
//7. Configure Synchronization Mode and Interval
//8. Initiate the Measurement.
//9. Fetch SlotPower Measurement.
//10 Close the RFmx Seesion


 
using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.EvdoMX;

namespace NationalInstruments.Examples.RFmxEvdoSlotPower
{
    public class RFmxEvdoSlotPower
    {
        RFmxInstrMX instrSession;
        RFmxEvdoMX evdo;

        string resourceName;
        RFmxEvdoMXMeasurementTypes measurement;
        string frequencyReferenceSource;
        double frequencyReferenceFrequency;                                  
        double centerFrequency;                                              
        double externalAttenuation;                                          

        string digitalEdgeSource;
        RFmxEvdoMXDigitalEdgeTriggerEdge digitalEdge;
        double triggerDelay;                                                 
        double referenceLevel;                                              

        RFmxEvdoMXSlotPowerSynchronizationMode synchronizationMode;
        int measurementOffset;                                               
        int measurementLength;                                              
        long uplinkSpreadingIMask;
        long uplinkSpreadingQMask;

        bool enableAllTraces;
        bool enableTrigger;
        double timeout;

        double[] halfSlotPower = null;
        double[] halfSlotPowerDelta = null;

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
            measurement = RFmxEvdoMXMeasurementTypes.SlotPower;
            frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
            frequencyReferenceFrequency = 10.0e+6;                             /* Hz */
            centerFrequency = 833.49e+6;                                       /* Hz */
            externalAttenuation = 0.000000;                                    /* dB */

            digitalEdgeSource = RFmxEvdoMXConstants.Pfi0;
            digitalEdge = RFmxEvdoMXDigitalEdgeTriggerEdge.Rising;
            triggerDelay = 0.000000;                                           /* seconds */
            referenceLevel = 0.000000;                                         /* dBm */

            synchronizationMode = RFmxEvdoMXSlotPowerSynchronizationMode.Slot;
            measurementOffset = 0;                                             /*slots*/
            measurementLength = 16;                                            /*slots*/
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
            evdo.ConfigureUplinkSpreading("", uplinkSpreadingIMask, uplinkSpreadingQMask);
            evdo.SelectMeasurements("", measurement, enableAllTraces);
            evdo.SlotPower.Configuration.ConfigureSynchronizationModeAndInterval("", synchronizationMode, measurementOffset, measurementLength);
            evdo.Initiate("", "");
        }

        void RetrieveResults()
        {
            evdo.SlotPower.Results.FetchPowers("", timeout, ref halfSlotPower, ref halfSlotPowerDelta);
        }

        void PrintResults()
        {
            Console.WriteLine("------------Half Slot Powers------------\n");
            for (int i = 0; i < halfSlotPower.Length; i++)
            {
                Console.WriteLine("\nHalf Slot Number {0}", i);
                Console.WriteLine("Half Slot Power (dBm)                    : {0}", halfSlotPower[i]);
                Console.WriteLine("Half Slot Power Delta (dB)               : {0}", halfSlotPowerDelta[i]);
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