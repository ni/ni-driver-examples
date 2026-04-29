//Steps:
//1. Open a new RFmx session.
//2. Configure the Frequency Reference properties(Clock Source and Clock Frequency).
//3. Configure Number of Frequency Segment and Receive Chain.
//4. Configure Center Frequency for each Segment.
//5. Configure the basic signal port specific properties(Reference Level and External Attenuation).
//6. Configure Selected Port.
//7. Configure IQ Power Edge Trigger properties(Trigger Delay, IQ Power Edge Level, Min Quiet Time).
//8. Configure Standard and Channel Bandwidth Properties.
//9. Select SEM measurement and enable the traces.
//10. Configure SEM Mask Type.
//11. Configure Averaging parameters.
//12. Configure Sweep Time and Span parameters.
//13. Initiate Measurement.
//14. Fetch SEM Traces and Measurements.
//15. Close the RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.WlanMX;

namespace NationalInstruments.Examples.RFmxWlanSemMimo
{
    public class RFmxWlanSemMimo
    {
        RFmxInstrMX instrSession;
        RFmxWlanMX wlan;
        string[] resourceName;
        int numberOfDevices;

        string[] selectedPorts;

        string frequencyReferenceSource;
        double frequencyReferenceFrequency;

        int numberOfFrequencySegments;
        int numberOfReceiveChains;

        string segmentString;
        string chainString;

        double[] centerFrequency;
        double[] referenceLevel;
        double[] externalAttenuation;

        string[] portString;

        string[] selectedPortsString;

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

        RFmxWlanMXSemSweepTimeAuto sweepTimeAuto;
        double sweepTime;

        RFmxWlanMXSemSpanAuto spanAuto;
        double span;

        double timeout;

        RFmxWlanMXSemMeasurementStatus measurementStatus;

        double[,] absolutePower;                                                   /*(dBm) */
        double[,] relativePower;                                                   /*(dBm) */

        RFmxWlanMXSemUpperOffsetMeasurementStatus[,][] upperOffsetMeasurementStatus;
        double[,][] upperOffsetMargin;                                             /*(dB) */
        double[,][] upperOffsetMarginFrequency;                                    /*(Hz) */
        double[,][] upperOffsetMarginAbsolutePower;                                /*(dBm) */
        double[,][] upperOffsetMarginRelativePower;                                /*(dBm) */

        RFmxWlanMXSemLowerOffsetMeasurementStatus[,][] lowerOffsetMeasurementStatus;
        double[,][] lowerOffsetMargin;                                             /*(dB) */
        double[,][] lowerOffsetMarginFrequency;                                    /*(Hz) */
        double[,][] lowerOffsetMarginAbsolutePower;                                /*(dBm) */
        double[,][] lowerOffsetMarginRelativePower;                                /*(dBm) */

        Spectrum<float>[,] spectrum;
        Spectrum<float>[,] compositeMask;

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
            resourceName = new string[] { "RFSA1", "RFSA2" };
            numberOfDevices = resourceName.GetLength(0);

            selectedPorts = new string[] { "", "" };

            frequencyReferenceSource = RFmxInstrMXConstants.PxiClock;
            frequencyReferenceFrequency = 10e6;                                     /* (Hz) */

            numberOfFrequencySegments = 1;
            numberOfReceiveChains = 2;

            centerFrequency = new double[] { 5.180000e9, 5.260000e9 };              /* (Hz) */
            referenceLevel = new double[] { 0.0, 0.0 };                             /* (dBm) */
            externalAttenuation = new double[] { 0.0, 0.0 };                        /* (dB) */

            portString = new string[numberOfDevices];

            selectedPortsString = new string[numberOfDevices];

            iqPowerEdgeEnabled = true;
            iqPowerEdgeLevel = -20.0;                                               /*(dB) */
            triggerDelay = 0.0;                                                     /* (s) */
            minimumQuietTimeMode = RFmxWlanMXTriggerMinimumQuietTimeMode.Auto;
            minimumQuietTime = 5.0e-6;                                              /* (s) */

            standard = RFmxWlanMXStandard.Standard802_11n;

            channelBandwidth = 20e6;                                                /*(Hz) */

            averagingEnabled = RFmxWlanMXSemAveragingEnabled.False;
            averagingCount = 10;
            averagingType = RFmxWlanMXSemAveragingType.Rms;

            sweepTimeAuto = RFmxWlanMXSemSweepTimeAuto.True;
            sweepTime = 1.0e-3;                                                     /* (s) */

            spanAuto = RFmxWlanMXSemSpanAuto.True;
            span = 66.0e6;                                                          /*(Hz) */

            timeout = 10.0;                                                         /* (s) */

            measurementStatus = new RFmxWlanMXSemMeasurementStatus();

            absolutePower = new double[numberOfFrequencySegments, numberOfReceiveChains];
            relativePower = new double[numberOfFrequencySegments, numberOfReceiveChains];

            upperOffsetMeasurementStatus = new RFmxWlanMXSemUpperOffsetMeasurementStatus[numberOfFrequencySegments,
               numberOfReceiveChains][];
            upperOffsetMargin = new double[numberOfFrequencySegments, numberOfReceiveChains][];
            upperOffsetMarginFrequency = new double[numberOfFrequencySegments, numberOfReceiveChains][];
            upperOffsetMarginAbsolutePower = new double[numberOfFrequencySegments, numberOfReceiveChains][];
            upperOffsetMarginRelativePower = new double[numberOfFrequencySegments, numberOfReceiveChains][];

            lowerOffsetMeasurementStatus = new RFmxWlanMXSemLowerOffsetMeasurementStatus[numberOfFrequencySegments,
               numberOfReceiveChains][];
            lowerOffsetMargin = new double[numberOfFrequencySegments, numberOfReceiveChains][];
            lowerOffsetMarginFrequency = new double[numberOfFrequencySegments, numberOfReceiveChains][];
            lowerOffsetMarginAbsolutePower = new double[numberOfFrequencySegments, numberOfReceiveChains][];
            lowerOffsetMarginRelativePower = new double[numberOfFrequencySegments, numberOfReceiveChains][];

            spectrum = new Spectrum<float>[numberOfFrequencySegments, numberOfReceiveChains];
            compositeMask = new Spectrum<float>[numberOfFrequencySegments, numberOfReceiveChains];
        }

        void InitializeInstr()
        {
            instrSession = new RFmxInstrMX(resourceName, "");
        }

        void ConfigureWlan()
        {
            wlan = instrSession.GetWlanSignalConfiguration();     /* Create a new RFmx Session */
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
            wlan.ConfigureNumberOfFrequencySegmentsAndReceiveChains("", numberOfFrequencySegments, numberOfReceiveChains);
            for (int i = 0; i < numberOfFrequencySegments; ++i)
            {
                segmentString = RFmxWlanMX.BuildSegmentString("", i);
                wlan.ConfigureFrequency(segmentString, centerFrequency[i]);
            }
            for (int i = 0; i < numberOfDevices; ++i)
            {
                selectedPortsString[i] = RFmxInstrMX.BuildPortString2("", selectedPorts[i], resourceName[i], 0);
                portString[i] = RFmxInstrMX.BuildPortString2("", "", resourceName[i], 0);
                wlan.ConfigureReferenceLevel(portString[i], referenceLevel[i]);
                wlan.ConfigureExternalAttenuation(portString[i], externalAttenuation[i]);
            }
            wlan.ConfigureSelectedPortsMultiple("", selectedPortsString);
            wlan.ConfigureIQPowerEdgeTrigger("", "0", RFmxWlanMXIQPowerEdgeTriggerSlope.Rising, iqPowerEdgeLevel,
               triggerDelay, minimumQuietTimeMode, minimumQuietTime, RFmxWlanMXIQPowerEdgeTriggerLevelType.Relative,
               iqPowerEdgeEnabled);
            wlan.ConfigureStandard("", standard);
            wlan.ConfigureChannelBandwidth("", channelBandwidth);
            wlan.SelectMeasurements("", RFmxWlanMXMeasurementTypes.Sem, true);
            wlan.Sem.Configuration.ConfigureMaskType("", RFmxWlanMXSemMaskType.Standard);
            wlan.Sem.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);
            wlan.Sem.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTime);
            wlan.Sem.Configuration.ConfigureSpan("", spanAuto, span);
            wlan.Initiate("", "");
        }

        void RetrieveResults()
        {
            for (int i = 0; i < numberOfFrequencySegments; ++i)
            {
                segmentString = RFmxWlanMX.BuildSegmentString("", i);
                for (int j = 0; j < numberOfReceiveChains; ++j)
                {
                    chainString = RFmxWlanMX.BuildChainString(segmentString, j);
                    wlan.Sem.Results.FetchCarrierMeasurement(chainString, timeout, out absolutePower[i, j], out relativePower[i, j]);
                    wlan.Sem.Results.FetchLowerOffsetMarginArray(chainString, timeout, ref lowerOffsetMeasurementStatus[i, j],
                       ref lowerOffsetMargin[i, j], ref lowerOffsetMarginFrequency[i, j], ref lowerOffsetMarginAbsolutePower[i, j],
                       ref lowerOffsetMarginRelativePower[i, j]);
                    wlan.Sem.Results.FetchUpperOffsetMarginArray(chainString, timeout, ref upperOffsetMeasurementStatus[i, j],
                       ref upperOffsetMargin[i, j], ref upperOffsetMarginFrequency[i, j], ref upperOffsetMarginAbsolutePower[i, j],
                       ref upperOffsetMarginRelativePower[i, j]);
                    wlan.Sem.Results.FetchSpectrum(chainString, timeout, ref spectrum[i, j], ref compositeMask[i, j]);
                }
            }
            wlan.Sem.Results.FetchMeasurementStatus("", timeout, out measurementStatus);

        }

        void PrintResults()
        {
            Console.WriteLine("Measurement Status                          :{0}", measurementStatus);
            for (int i = 0; i < numberOfFrequencySegments; ++i)
            {
                segmentString = RFmxWlanMX.BuildSegmentString("", i);
                for (int j = 0; j < numberOfReceiveChains; ++j)
                {
                    chainString = RFmxWlanMX.BuildChainString(segmentString, j);

                    Console.WriteLine("\n-------Measurement for {0}-------\n\n", chainString);
                    Console.WriteLine("Carrier Absolute Power (dBm)                :{0}", absolutePower[i, j]);

                    Console.WriteLine("\n----------Lower Offset Measurements----------\n");
                    for (int k = 0; k < lowerOffsetMargin[i, j].Length; k++)
                    {
                        Console.WriteLine("Offset {0}", k);
                        Console.WriteLine("Measurement Status              :{0}",
                             lowerOffsetMeasurementStatus[i, j][k]);
                        Console.WriteLine("Margin (dB)                     :{0}", lowerOffsetMargin[i, j][k]);
                        Console.WriteLine("Margin Frequency (Hz)           :{0}", lowerOffsetMarginFrequency[i, j][k]);
                        Console.WriteLine("Margin Absolute Power (dBm)     :{0}\n", lowerOffsetMarginAbsolutePower[i, j][k]);
                    }

                    Console.WriteLine("\n----------Upper Offset Measurements----------\n");
                    for (int k = 0; k < upperOffsetMargin[i, j].Length; k++)
                    {
                        Console.WriteLine("Offset {0}", k);
                        Console.WriteLine("Measurement Status              :{0}", upperOffsetMeasurementStatus[i, j][k]);
                        Console.WriteLine("Margin (dB)                     :{0}", upperOffsetMargin[i, j][k]);
                        Console.WriteLine("Margin Frequency (Hz)           :{0}", upperOffsetMarginFrequency[i, j][k]);
                        Console.WriteLine("Margin Absolute Power (dBm)     :{0}\n", upperOffsetMarginAbsolutePower[i, j][k]);
                    }
                }
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
