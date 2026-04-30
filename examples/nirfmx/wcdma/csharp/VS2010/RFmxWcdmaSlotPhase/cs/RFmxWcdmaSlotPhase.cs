//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure WCDMA centre frequency.
//4. Configure Trigger Type and Trigger Parameters.
//5. Select SlotPhase measurement and enable Traces.
//6. Configure Uplink Scrambling.
//7. Configure Synchronisation Mode and Interval
//8. Initiate the Measurement.
//9. Fetch SlotPhase Measurements and Traces.
//10. Close RFmx Session. 

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.WcdmaMX;

namespace NationalInstruments.Examples.RFmxWcdmaSlotPhase
{
    public class RFmxWcdmaSlotPhase
    {
        RFmxInstrMX instrSession;
        RFmxWcdmaMX wcdma;

        string resourceName;
        RFmxWcdmaMXMeasurementTypes measurement;
        string frequencyReferenceSource;
        double frequencyReferenceFrequency;                         /* Hz */
        double centerFrequency;                                     /* Hz */
        double externalAttenuation;                                /* dB */

        string digitalEdgeSource;
        RFmxWcdmaMXDigitalEdgeTriggerEdge digitalEdge;
        double triggerDelay;                                       /* seconds */
        double referenceLevel;                                     /* dBm */

        RFmxWcdmaMXSlotPhaseSynchronizationMode synchronisationMode;
        int measurementOffset;                                            /*slots*/
        int measurementLength;                                           /*slots*/

        RFmxWcdmaMXUplinkScramblingType uplinkScramblingType;
        int uplinkScramblingCode;

        bool enableAllTraces;
        bool enableTrigger;

        double timeout;

        int discontinuityMinimumDistance;                                 /*slots*/
        int discontinuityCountGreaterThanlimit1;
        int discontinuityCountGreaterThanlimit2;
        double maximumPhaseDiscontinuity;                                /*deg*/

        double[] slotPhaseDiscontinuity = null;
        AnalogWaveform<float> chipPhaseError = null;
        AnalogWaveform<float> chipPhaseErrorLinearFit = null;



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
            measurement = RFmxWcdmaMXMeasurementTypes.SlotPhase;
            frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
            frequencyReferenceFrequency = 10.0e+6;                         /* Hz */
            centerFrequency = 1.95e+9;                                     /* Hz */
            externalAttenuation = 0.000000;                                /* dB */

            digitalEdgeSource = RFmxWcdmaMXConstants.Pfi0;
            digitalEdge = RFmxWcdmaMXDigitalEdgeTriggerEdge.Rising;
            triggerDelay = 0.000000;                                       /* seconds */
            referenceLevel = 0.000000;                                     /* dBm */

            synchronisationMode = RFmxWcdmaMXSlotPhaseSynchronizationMode.Slot;
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

            wcdma.SelectMeasurements("", measurement, enableAllTraces);

            wcdma.ConfigureUplinkScrambling("", uplinkScramblingCode, uplinkScramblingType);
            wcdma.SlotPhase.Configuration.ConfigureSynchronizationModeAndInterval("", synchronisationMode,
                measurementOffset, measurementLength);
            wcdma.Initiate("", "");
        }

        void RetrieveResults()
        {
            wcdma.SlotPhase.Results.FetchMeasurement("", timeout, out maximumPhaseDiscontinuity,
                out discontinuityCountGreaterThanlimit1, out discontinuityCountGreaterThanlimit2, out discontinuityMinimumDistance);
            wcdma.SlotPhase.Results.FetchPhaseDiscontinuities("", timeout, ref slotPhaseDiscontinuity);
            wcdma.SlotPhase.Results.FetchChipPhaseErrorTrace("", timeout, ref chipPhaseError);
            wcdma.SlotPhase.Results.FetchChipPhaseErrorLinearFitTrace("", timeout, ref chipPhaseErrorLinearFit);
        }

        void PrintResults()
        {
            Console.WriteLine("------------Measurement------------\n");
            Console.WriteLine("Maximum Phase Discontinuity (deg)        : {0}\n", maximumPhaseDiscontinuity);
            Console.WriteLine("Discontinuity Count > Limit1             : {0}\n", discontinuityCountGreaterThanlimit1);
            Console.WriteLine("Discontinuity Count > Limit2             : {0}\n", discontinuityCountGreaterThanlimit2);
            Console.WriteLine("Discontinuity Minimum Distance (slots)   : {0}\n", discontinuityMinimumDistance);

            Console.WriteLine("Slot Phase Discontinuity (deg)           :");
            for (int i = 0; i < measurementLength; i++)
                Console.WriteLine(i + ": {0}", slotPhaseDiscontinuity[i]);
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