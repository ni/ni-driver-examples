//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Uplink Scrambling.
//6. Select SlotPower measurement and enable Traces.
//7. Configure Synchronization Mode and Interval
//8. Initiate the Measurement.
//9. Fetch SlotPower Measurement.
//10. Close the RFmx session. 

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.WcdmaMX;

namespace NationalInstruments.Examples.RFmxWcdmaSlotPower
{
    public class RFmxWcdmaSlotPower
    {
        RFmxInstrMX instrSession;
        RFmxWcdmaMX wcdma;

        string resourceName;
        RFmxWcdmaMXMeasurementTypes measurement;
        string frequencyReferenceSource;
        double frequencyReferenceFrequency;                                  /* Hz */
        double centerFrequency;                                              /* Hz */
        double externalAttenuation;                                          /* dB */

        string digitalEdgeSource;
        RFmxWcdmaMXDigitalEdgeTriggerEdge digitalEdge;
        double triggerDelay;                                                 /* seconds */
        double referenceLevel;                                               /* dBm */

        RFmxWcdmaMXSlotPowerSynchronizationMode synchronizationMode;
        int measurementOffset;                                               /*slots*/
        int measurementLength;                                               /*slots*/

        RFmxWcdmaMXUplinkScramblingType uplinkScramblingType;
        int uplinkScramblingCode;

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
                ConfigureWcdma();
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
            measurement = RFmxWcdmaMXMeasurementTypes.SlotPower;
            frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
            frequencyReferenceFrequency = 10.0e+6;                         /* Hz */
            centerFrequency = 1.95e+9;                                     /* Hz */
            externalAttenuation = 0.000000;                                /* dB */

            digitalEdgeSource = RFmxWcdmaMXConstants.Pfi0;
            digitalEdge = RFmxWcdmaMXDigitalEdgeTriggerEdge.Rising;
            triggerDelay = 0.000000;                                       /* seconds */
            referenceLevel = 0.000000;                                     /* dBm */

            synchronizationMode = RFmxWcdmaMXSlotPowerSynchronizationMode.Slot;
            measurementOffset = 0;                                            /*slots*/
            measurementLength = 15;                                           /*slots*/

            uplinkScramblingType = RFmxWcdmaMXUplinkScramblingType.Long;
            uplinkScramblingCode = 0;

            enableAllTraces = true;
            enableTrigger = false;
            timeout = 10.0;
        }

        void InitializeInstr()
        {
            /* Create a new RFmx Session */
            instrSession = new RFmxInstrMX(resourceName, "");
        }

        void ConfigureWcdma()
        {
            wcdma = instrSession.GetWcdmaSignalConfiguration();
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
            wcdma.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
            wcdma.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger);
            wcdma.ConfigureUplinkScrambling("", uplinkScramblingCode, uplinkScramblingType);
            wcdma.SelectMeasurements("", measurement, enableAllTraces);
            wcdma.SlotPower.Configuration.ConfigureSynchronizationModeAndInterval("", synchronizationMode, measurementOffset, measurementLength);
            wcdma.Initiate("", "");
        }

        void RetrieveResults()
        {
            wcdma.SlotPower.Results.FetchPowers("", timeout, ref slotPower, ref slotPowerDelta);
        }

        void PrintResults()
        {
            Console.WriteLine("------------Slot Powers------------\n");
            for (int i = 0; i < measurementLength; i++)
            {
                Console.WriteLine("\nSlot Number {0}", i);
                Console.WriteLine("Slot Power (dBm)                  : {0}", slotPower[i]);
                Console.WriteLine("Slot Power Delta (dB)             : {0}", slotPowerDelta[i]);
            }
        }

        void CloseSession()
        {
            if (wcdma != null)
            {
                wcdma.Dispose();
                wcdma = null;
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