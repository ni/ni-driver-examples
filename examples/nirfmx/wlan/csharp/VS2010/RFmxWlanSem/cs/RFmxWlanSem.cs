//Steps:
//1. Open a new RFmx session.
//2. Configure the frequency reference properties(Clock Source and Clock Frequency).
//3. Configure the basic signal properties(Center Frequency, Reference Level and External Attenuation).
//4. Configure IQ Power Edge Trigger properties(Trigger Delay, IQ Power Edge Level, Minimum Quiet Time).
//5. Configure Standard and Channel Bandwidth Properties.
//6. Select SEM measurement and enable the traces.
//7. Configure Averaging parameters.
//8. Configure Sweep Time and Span parameters.
//9. Initiate Measurement.
//10. Fetch SEM Traces and Measurements.
//11. Close the RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.WlanMX;

namespace NationalInstruments.Examples.RFmxWlanSem
{
    public class RFmxWlanSem
    {
        RFmxInstrMX instrSession;
        RFmxWlanMX wlan;
        string resourceName;

        double centerFrequency;
        double referenceLevel;
        double externalAttenuation;

        string frequencyReferenceSource;
        double frequencyReferenceFrequency;

        bool iqPowerEdgeEnabled;
        double iqPowerEdgeLevel;
        double triggerDelay;
        RFmxWlanMXTriggerMinimumQuietTimeMode minimumQuietTimeMode;
        double minimumQuietTime;

        RFmxWlanMXStandard standard;

        double channelBandwidth;

        RFmxWlanMXSemAveragingEnabled averagingEnabled;
        int averagingCount;
        RFmxWlanMXSemAveragingType averagingType;

        RFmxWlanMXSemSpanAuto spanAuto;
        double span;

        RFmxWlanMXSemSweepTimeAuto sweepTimeAuto;
        double sweepTime;

        RFmxWlanMXSemMaskType maskType;

        double timeout;

        RFmxWlanMXSemMeasurementStatus measurementStatus;

        double absolutePower;                                                   /*(dBm) */
        double relativePower;                                                   /*(dBm) */

        RFmxWlanMXSemUpperOffsetMeasurementStatus[] upperOffsetMeasurementStatus;
        double[] upperOffsetMargin;                                             /*(dB) */
        double[] upperOffsetMarginFrequency;                                    /*(Hz) */
        double[] upperOffsetMarginAbsolutePower;                                /*(dBm) */
        double[] upperOffsetMarginRelativePower;                                /*(dBm) */

        RFmxWlanMXSemLowerOffsetMeasurementStatus[] lowerOffsetMeasurementStatus;
        double[] lowerOffsetMargin;                                             /*(dB) */
        double[] lowerOffsetMarginFrequency;                                    /*(Hz) */
        double[] lowerOffsetMarginAbsolutePower;                                /*(dBm) */
        double[] lowerOffsetMarginRelativePower;                                /*(dBm) */

        Spectrum<float> spectrum;
        Spectrum<float> compositeMask;

        public void Run()
        {
            try
            {
                InitializeVariables();
                InitializeInstr();
                ConfigureWlan();
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

            centerFrequency = 2.412e9;                                              /* (Hz) */
            referenceLevel = 0.0;                                                   /* (dBm) */
            externalAttenuation = 0.0;                                              /* (dB) */

            frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
            frequencyReferenceFrequency = 10e6;                                     /* (Hz) */

            iqPowerEdgeEnabled = true;
            iqPowerEdgeLevel = -20.0;                                               /*(dB) */
            triggerDelay = 0.0;                                                     /* (s) */
            minimumQuietTimeMode = RFmxWlanMXTriggerMinimumQuietTimeMode.Auto;
            minimumQuietTime = 5.0e-6;                                              /* (s) */

            standard = RFmxWlanMXStandard.Standard802_11ag;

            channelBandwidth = 20e6;                                                /*(Hz) */

            averagingEnabled = RFmxWlanMXSemAveragingEnabled.False;
            averagingCount = 10;
            averagingType = RFmxWlanMXSemAveragingType.Rms;

            spanAuto = RFmxWlanMXSemSpanAuto.True;
            span = 66.0e6;                                                          /*(Hz) */

            sweepTimeAuto = RFmxWlanMXSemSweepTimeAuto.True;
            sweepTime = 1.0e-3;                                                     /* (s) */

            maskType = RFmxWlanMXSemMaskType.Standard;

            timeout = 10.0;                                                         /* (s) */
        }

        void InitializeInstr()
        {
            instrSession = new RFmxInstrMX(resourceName, "");
        }

        void ConfigureWlan()
        {
            wlan = instrSession.GetWlanSignalConfiguration();     /* Create a new RFmx Session */
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
            wlan.ConfigureFrequency("", centerFrequency);
            wlan.ConfigureReferenceLevel("", referenceLevel);
            wlan.ConfigureExternalAttenuation("", externalAttenuation);
            wlan.ConfigureIQPowerEdgeTrigger("", "0", RFmxWlanMXIQPowerEdgeTriggerSlope.Rising, iqPowerEdgeLevel,
               triggerDelay, minimumQuietTimeMode, minimumQuietTime, RFmxWlanMXIQPowerEdgeTriggerLevelType.Relative,
               iqPowerEdgeEnabled);
            wlan.ConfigureStandard("", standard);
            wlan.ConfigureChannelBandwidth("", channelBandwidth);
            wlan.SelectMeasurements("", RFmxWlanMXMeasurementTypes.Sem, true);
            wlan.Sem.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);
            wlan.Sem.Configuration.ConfigureMaskType("", maskType);
            wlan.Sem.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTime);
            wlan.Sem.Configuration.ConfigureSpan("", spanAuto, span);
            wlan.Initiate("", "");
        }

        void RetrieveResults()
        {
            wlan.Sem.Results.FetchMeasurementStatus("", timeout, out measurementStatus);
            wlan.Sem.Results.FetchCarrierMeasurement("", timeout, out absolutePower, out relativePower);
            wlan.Sem.Results.FetchLowerOffsetMarginArray("", timeout, ref lowerOffsetMeasurementStatus,
               ref lowerOffsetMargin, ref lowerOffsetMarginFrequency, ref lowerOffsetMarginAbsolutePower,
               ref lowerOffsetMarginRelativePower);
            wlan.Sem.Results.FetchUpperOffsetMarginArray("", timeout, ref upperOffsetMeasurementStatus,
               ref upperOffsetMargin, ref upperOffsetMarginFrequency, ref upperOffsetMarginAbsolutePower,
               ref upperOffsetMarginRelativePower);
            wlan.Sem.Results.FetchSpectrum("", timeout, ref spectrum, ref compositeMask);
        }

        void PrintResults()
        {
            Console.WriteLine("Measurement Status                          :{0}", measurementStatus);
            Console.WriteLine("Carrier Absolute Power (dBm)                :{0}", absolutePower);

            Console.WriteLine("\n----------Lower Offset Measurements----------\n");
            for (int i = 0; i < lowerOffsetMargin.Length; i++)
            {
                Console.WriteLine("Offset {0}", i);
                Console.WriteLine("Measurement Status              :{0}",
                     lowerOffsetMeasurementStatus[i]);
                Console.WriteLine("Margin (dB)                     :{0}", lowerOffsetMargin[i]);
                Console.WriteLine("Margin Frequency (Hz)           :{0}", lowerOffsetMarginFrequency[i]);
                Console.WriteLine("Margin Absolute Power (dBm)     :{0}\n", lowerOffsetMarginAbsolutePower[i]);
            }

            Console.WriteLine("\n----------Upper Offset Measurements----------\n");
            for (int i = 0; i < upperOffsetMargin.Length; i++)
            {
                Console.WriteLine("Offset {0}", i);
                Console.WriteLine("Measurement Status              :{0}", upperOffsetMeasurementStatus[i]);
                Console.WriteLine("Margin (dB)                     :{0}", upperOffsetMargin[i]);
                Console.WriteLine("Margin Frequency (Hz)           :{0}", upperOffsetMarginFrequency[i]);
                Console.WriteLine("Margin Absolute Power (dBm)     :{0}\n", upperOffsetMarginAbsolutePower[i]);
            }
        }

        void CloseSession()
        {
            if (wlan != null)
            {
                wlan.Dispose();
                wlan = null;
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
