//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Select SlotPower measurement and enable Traces.
//6. Configure Measurement Length.
//7. Initiate the Measurement.
//8. Fetch SlotPower Measurement.
//9. Close the RFmx session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.TdscdmaMX;

namespace NationalInstruments.Examples.RFmxTdscdmaSlotPower
{
    public class RFmxTdscdmaSlotPower
    {
        RFmxInstrMX instrSession;
        RFmxTdscdmaMX tdscdma;

        string resourceName;
        RFmxTdscdmaMXMeasurementTypes measurement;
        string frequencyReferenceSource, iqPowerEdgeTriggerSource;
        double frequencyReferenceFrequency;                                 /* Hz */
        double centerFrequency;                                             /* Hz */
        double externalAttenuation;                                         /* dB */
        double iqPowerEdgeTriggerLevel, triggerDelay, minimumQuietTimeDuration;
        double referenceLevel;                                              /* dBm */
        bool enableTrigger;
        RFmxTdscdmaMXIQPowerEdgeTriggerSlope iqPowerEdgeTriggerSlope;
        RFmxTdscdmaMXTriggerMinimumQuietTimeMode minimumQuietTimeMode;
        RFmxTdscdmaMXIQPowerEdgeTriggerLevelType iqPowerEdgeTriggerLevelType;

        int measurementLength;                                              /*slots*/
        bool enableAllTraces;
        double timeout;

        double[] slotPower = null;                                          /* dBm */
        double[] slotPowerDelta = null;                                     /* dB */

        public void Run()
        {
            try
            {
                InitializeVariables();
                InitializeInstr();
                ConfigureTdscdma();
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
            measurement = RFmxTdscdmaMXMeasurementTypes.SlotPower;
            frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
            frequencyReferenceFrequency = 10.0e+6;                          /* Hz */
            centerFrequency = 1.91e+9;                                      /* Hz */
            externalAttenuation = 0.000000;                                 /* dB */

            iqPowerEdgeTriggerSource = "0";
            iqPowerEdgeTriggerSlope = RFmxTdscdmaMXIQPowerEdgeTriggerSlope.Rising;
            iqPowerEdgeTriggerLevel = -20.00;                               /*dB*/
            triggerDelay = 0.00;                                            /* seconds */
            minimumQuietTimeMode = RFmxTdscdmaMXTriggerMinimumQuietTimeMode.Auto;
            iqPowerEdgeTriggerLevelType = RFmxTdscdmaMXIQPowerEdgeTriggerLevelType.Relative;
            enableTrigger = true;
            minimumQuietTimeDuration = 16E-6;                               /* seconds */

            referenceLevel = 0.000000;                                      /* dBm */
            measurementLength = 28;                                         /*slots*/

            enableAllTraces = true;
            timeout = 10.0;
        }

        void InitializeInstr()
        {
            /* Create a new RFmx Session */
            instrSession = new RFmxInstrMX(resourceName, "");
        }

        void ConfigureTdscdma()
        {
            tdscdma = instrSession.GetTdscdmaSignalConfiguration();
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
            tdscdma.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);

            tdscdma.ConfigureIQPowerEdgeTrigger("", iqPowerEdgeTriggerSource, iqPowerEdgeTriggerSlope, iqPowerEdgeTriggerLevel, triggerDelay,
                minimumQuietTimeMode, minimumQuietTimeDuration, iqPowerEdgeTriggerLevelType, enableTrigger);

            tdscdma.SelectMeasurements("", measurement, enableAllTraces);

            tdscdma.SlotPower.Configuration.ConfigureMeasurementLength("", measurementLength);
            tdscdma.Initiate("","");
        }

        void RetrieveResults()
        {
            tdscdma.SlotPower.Results.FetchPowers("", timeout, ref slotPower, ref slotPowerDelta);
        }

        void PrintResults()
        {
            Console.WriteLine("------------Slot Powers ------------\n");
            for (int i = 0; i < slotPower.Length; i++)
            {
                Console.WriteLine("Slot Number           : {0}", i);
                Console.WriteLine("Slot Power (dBm)      : {0}", slotPower[i]);
                Console.WriteLine("Slot Power Delta (dB) : {0}", slotPowerDelta[i]);
            }
        }

        void CloseSession()
        {
            if (tdscdma != null)
            {
                tdscdma.Dispose();
                tdscdma = null;
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