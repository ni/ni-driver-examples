//Steps:
//1. Open a new RFmx session.
//2. Configure Frequency Reference.
//3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters..
//5. Select PvT measurement and enable traces.
//6. Configure Midamble.
//7. Configure Measurement Method.
//8. Configure Averaging.
//9. Initiate the Measurement.
//10. Fetch PvT Measurements and Traces.
//11. Close the RFmx session.


using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.TdscdmaMX;

namespace NationalInstruments.Examples.RFmxTdscdmaPvt
{
    public class RFmxTdscdmaPvt
    {
        RFmxInstrMX instrSession;
        RFmxTdscdmaMX tdscdma;

        string resourceName, iqPowerEdgeTriggerSource, frequencySource;
        double centerFrequency, referenceLevel, externalAttenuation, minimumQuietTimeDuration, frequencyReferenceFrequency,
               triggerDelay, iqPowerEdgeTriggerLevel;
        double timeout;

        RFmxTdscdmaMXPvtAveragingEnabled averagingEnabled;
        RFmxTdscdmaMXPvtAveragingType averagingType;
        RFmxTdscdmaMXPvtMeasurementMethod measurementMethod;
        RFmxTdscdmaMXTriggerMinimumQuietTimeMode minimumQuietTimeMode;
        RFmxTdscdmaMXMidambleAutoDetectionMode midambleAutoDetectionMode;
        bool enableTrigger;
        RFmxTdscdmaMXIQPowerEdgeTriggerLevelType iqPowerEdgeTriggerLevelType;
        RFmxTdscdmaMXIQPowerEdgeTriggerSlope iqPowerEdgeTriggerSlope;

        RFmxTdscdmaMXPvtSegmentStatus[] segmentStatus;
        RFmxTdscdmaMXPvtMeasurementStatus measurementStatus;
        int averagingCount;
        int maximumNumberOfUsers, midambleShift;


        double meanAbsoluteONPower, meanAbsoluteOFFPower;

        double[] segmentMargin;
        double[] segmentMarginTime;
        double[] segmentMeanAbsolutePower;
        double[] segmentMaximumAbsolutePower;
        double[] segmentMinimumAbsolutePower;

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
                Console.WriteLine("Press any key to exit.....");
                Console.ReadKey();
            }
        }

        private void InitializeVariables()
        {
            resourceName = "RFSA";
            frequencySource = RFmxInstrMXConstants.OnboardClock;
            frequencyReferenceFrequency = 10E+6;         /* Hz */
            centerFrequency = 1.91E+9;                   /* Hz */
            referenceLevel = 0.00;                       /* dBm */
            externalAttenuation = 0.00;                  /* dB */
            timeout = 10.00;                             /* seconds */
            enableTrigger = true;
            iqPowerEdgeTriggerSource = "0";
            iqPowerEdgeTriggerSlope = RFmxTdscdmaMXIQPowerEdgeTriggerSlope.Rising;
            iqPowerEdgeTriggerLevel = -20.00;            /*dB*/
            iqPowerEdgeTriggerLevelType = RFmxTdscdmaMXIQPowerEdgeTriggerLevelType.Relative;
            triggerDelay = 0.00;                         /* seconds */
            minimumQuietTimeMode = RFmxTdscdmaMXTriggerMinimumQuietTimeMode.Auto;
            minimumQuietTimeDuration = 16E-6;            /* seconds */
            averagingEnabled = RFmxTdscdmaMXPvtAveragingEnabled.False;
            averagingCount = 10;
            averagingType = RFmxTdscdmaMXPvtAveragingType.Rms;
            measurementMethod = RFmxTdscdmaMXPvtMeasurementMethod.Normal;
            midambleAutoDetectionMode = RFmxTdscdmaMXMidambleAutoDetectionMode.MidambleShift;
            maximumNumberOfUsers = 16;
            midambleShift = 8;
        }

        private void InitializeInstr()
        {
            /* Create a new RFmx Session */
            instrSession = new RFmxInstrMX(resourceName, "");
        }

        private void ConfigureTdscdma()
        {
            /* Get Tdscdma signal */
            tdscdma = instrSession.GetTdscdmaSignalConfiguration();

            /* Configure measurement */
            instrSession.ConfigureFrequencyReference("", frequencySource, frequencyReferenceFrequency);
            tdscdma.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
            tdscdma.ConfigureIQPowerEdgeTrigger("", iqPowerEdgeTriggerSource, iqPowerEdgeTriggerSlope,
                iqPowerEdgeTriggerLevel, triggerDelay, minimumQuietTimeMode, minimumQuietTimeDuration,
                iqPowerEdgeTriggerLevelType, enableTrigger);
            tdscdma.SelectMeasurements("", RFmxTdscdmaMXMeasurementTypes.Pvt, true);
            tdscdma.ConfigureMidambleShift("", midambleAutoDetectionMode, maximumNumberOfUsers, midambleShift);
            tdscdma.Pvt.Configuration.ConfigureMeasurementMethod("", measurementMethod);
            tdscdma.Pvt.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);
            tdscdma.Initiate("", "");
        }

        private void RetrieveResults()
        {
            AnalogWaveform<float> signalPower = null;
            AnalogWaveform<float> absoluteLimit = null;
            tdscdma.Pvt.Results.FetchMeasurementStatus("", timeout, out measurementStatus);
            tdscdma.Pvt.Results.FetchPowers("", timeout, out meanAbsoluteONPower, out meanAbsoluteOFFPower);
            tdscdma.Pvt.Results.FetchSegmentMeasurementArray("", timeout, ref segmentStatus, ref segmentMargin, ref segmentMarginTime, ref segmentMeanAbsolutePower, ref segmentMaximumAbsolutePower,
                                                                  ref segmentMinimumAbsolutePower);
            tdscdma.Pvt.Results.FetchSignalPowerTrace("", timeout, ref signalPower, ref absoluteLimit);
        }

        private void PrintResults()
        {
            Console.WriteLine("--------------------PVT  Results--------------------");

            for (int i = 0; i < segmentMargin.Length; i++)
            {
                Console.WriteLine("\nSegment                                {0}", i);

                Console.WriteLine("Measurement Status                     {0}", segmentStatus[i]);

                Console.WriteLine("Margin (dB)                            {0}", segmentMargin[i]);

                Console.WriteLine("Margin Time(sec)                       {0}", segmentMarginTime[i]);

                Console.WriteLine("Mean Absolute Power (dBm)              {0}", segmentMeanAbsolutePower[i]);

                Console.WriteLine("Maximum Absolute Power (dBm)           {0}", segmentMaximumAbsolutePower[i]);

                Console.WriteLine("Minimum Absolute Power (dBm)           {0}", segmentMinimumAbsolutePower[i]);

            }


            Console.WriteLine("\nMeasurement Status                         {0}", measurementStatus);


            Console.WriteLine("\n--------------------Absolute Powers--------------------");

            Console.WriteLine("Mean Absolute ON Power (dBm)           {0}", meanAbsoluteONPower);

            Console.WriteLine("Mean Absolute OFF Power (dBm)          {0}", meanAbsoluteOFFPower);
        }

        private void CloseSession()
        {
            try
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