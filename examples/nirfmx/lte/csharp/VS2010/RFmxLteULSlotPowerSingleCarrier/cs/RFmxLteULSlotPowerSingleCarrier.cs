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

namespace NationalInstruments.Examples.RFmxLteULSlotPowerSingleCarrier
{
    public class RFmxLteULSlotPowerSingleCarrier
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

        string subblockString, subblockCarrierString;
        RFmxLteMXDuplexScheme duplexScheme;
        RFmxLteMXUplinkDownlinkConfiguration uplinkDownlinkConfiguration;
        double componentCarrierBandwidth;
        double componentCarrierFrequency;
        int cellID, i;
        const int numberOfSlots = 20;

        int measurementOffset;
        int measurementLength;

        double timeout;

        double[] subFramePower, subFramePowerDelta;


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
            measurementOffset = 0;                                               /* subframes */
            measurementLength = numberOfSlots;                                   /* subframes */

        }

        private void ConfigureLte()
        {
            /* Get Lte signal */
            lte = instrSession.GetLteSignalConfiguration();

            /* Configure measurement */
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
            lte.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
            lte.ConfigureDigitalEdgeTrigger("", digitalEdgeTriggerSource, digitalEdgeTriggerEdge, triggerDelay, enableTrigger);
            subblockString = RFmxLteMX.BuildSubblockString("", 0);
            subblockCarrierString = RFmxLteMX.BuildCarrierString(subblockString, 0);

            lte.ComponentCarrier.Configure(subblockCarrierString, componentCarrierBandwidth,
                                                componentCarrierFrequency, cellID);
            lte.ConfigureDuplexScheme("", duplexScheme, uplinkDownlinkConfiguration);
            lte.SelectMeasurements("", RFmxLteMXMeasurementTypes.SlotPower, true);
            lte.SlotPower.Configuration.ConfigureMeasurementInterval("", measurementOffset, measurementLength);
            lte.Initiate("", "");
        }

        private void RetrieveResults()
        {
            /* Retrieve results */
            lte.SlotPower.Results.FetchPowers("", timeout, ref subFramePower, ref subFramePowerDelta);
        }

        private void PrintResults()
        {
            /* Retrieve results */
            Console.WriteLine("Subframe Power(dBm): \n");

            for (i = 0; i < subFramePower.Length; i++)
            {
                if (i == subFramePower.Length - 1)
                    Console.Write(subFramePower[i]);
                else
                    Console.Write("{0}, ", subFramePower[i]);
            }

            Console.WriteLine("\n\nSubframe Power Delta(dB): \n");

            for (i = 0; i < subFramePowerDelta.Length; i++)
            {
                if (i == subFramePowerDelta.Length - 1)
                    Console.Write(subFramePowerDelta[i]);
                else
                    Console.Write("{0}, ", subFramePowerDelta[i]);
            }
            Console.WriteLine("\n");
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
