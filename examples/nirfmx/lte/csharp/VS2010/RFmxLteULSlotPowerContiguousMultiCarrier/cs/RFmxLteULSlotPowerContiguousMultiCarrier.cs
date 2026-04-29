//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Component Carrier Spacing.
//6. Configure Component Carriers.
//7. Configure Duplex Scheme.
//8. Select SlotPower measurement and enable Traces.
//9. Configure Measurement Method.
//10. Initiate the Measurement.
//11. Fetch SlotPower Measurements and Traces.
//12. Close RFmx Session. 

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.LteMX;

namespace NationalInstruments.Examples.RFmxLteULSlotPowerContiguousMultiCarrier
{
    public class RFmxLteULSlotPowerContiguousMultiCarrier
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

        const int numberOfComponentCarriers = 2;

        RFmxLteMXDuplexScheme duplexScheme;
        int measurementOffset;
        int measurementLength;

        RFmxLteMXComponentCarrierSpacingType componentCarrierSpacingType;
        int componentCarrierAtCenterFrequency;

        double[] componentCarrierBandwidth = { 20e6, 20e6 };                     /*(Hz) */
        double[] componentCarrierFrequency = { -9.9e6, 9.9e6 };                  /*(Hz) */
        int[] cellId = { 0, 1 };

        RFmxLteMXUplinkDownlinkConfiguration uplinkDownlinkConfiguraiton;
        string carrierString;
        double timeout;

        double[][] subframePower = new double[numberOfComponentCarriers][];
        double[][] subframePowerDelta = new double[numberOfComponentCarriers][];

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

            measurementOffset = 0;                                               /* subframes */
            measurementLength = 10;                                              /* subframes */

            componentCarrierSpacingType = RFmxLteMXComponentCarrierSpacingType.Nominal;
            componentCarrierAtCenterFrequency = -1;
            uplinkDownlinkConfiguraiton = RFmxLteMXUplinkDownlinkConfiguration.Configuration0;
            timeout = 10.0;                                                      /* (s) */
        }

        void InitializeInstr()
        {
            /* Create a new RFmx Session */
            instrSession = new RFmxInstrMX(resourceName, "");
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
            lte.SelectMeasurements("", RFmxLteMXMeasurementTypes.SlotPower, true);
            lte.SlotPower.Configuration.ConfigureMeasurementInterval("", measurementOffset, measurementLength);
            lte.Initiate("", "");
        }

        private void RetrieveResults()
        {
            /* Retrieve results */

            for (int i = 0; i < numberOfComponentCarriers; i++)
            {
                carrierString = RFmxLteMX.BuildCarrierString("", i);
                lte.SlotPower.Results.FetchPowers(carrierString, timeout, ref subframePower[i], ref subframePowerDelta[i]);
            }
        }

        private void PrintResults()
        {
            /* Retrieve results */
            for (int i = 0; i < numberOfComponentCarriers; i++)
            {
                Console.WriteLine("\n\nCC {0} Trace", i);
                Console.WriteLine("Subframe Power(dBm)");
                PrintArray(subframePower[i]);
                Console.WriteLine();
                Console.WriteLine("\nSubframe Power Delta(dB)");
                PrintArray(subframePowerDelta[i]);
            }
        }

        private void PrintArray(double[] arrayToPrint)
        {
            int i = 0, length = arrayToPrint.Length;
            for (i = 0; i < length; i++)
            {
                if (i == (length - 1))
                {
                    Console.Write(arrayToPrint[i]);
                }
                else
                {
                    Console.Write(arrayToPrint[i] + ",");
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
