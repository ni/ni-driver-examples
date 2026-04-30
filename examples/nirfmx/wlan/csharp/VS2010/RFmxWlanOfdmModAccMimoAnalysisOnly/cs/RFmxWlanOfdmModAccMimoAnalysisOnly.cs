//Steps:
//1. Open a new RFmx session.
//     Note: To configure for more than 4 waveform, set the maxnumwfms in the option string
//                to that many number of waveform. 
//2. Configure Number of Frequency Segment and Receive Chain.
//3. Configure Center Frequency for each Segment.
//4. Configure Standard and Channel Bandwidth Properties.
//5. Select OFDMModAcc measurement and enable all the traces. 
//6. Configure the Measurement Interval.
//7. Configure Frequency Error Estimation Method.
//8. Configure Amplitude Tracking Enabled.
//9. Configure Phase Tracking Enabled.
//10. Configure Symbol Clock Error Correction Enabled.
//11. Configure Channel Estimation Type.
//12. Read the MIMO Waveform from the .tdms file. 
//13. Wire the waveforms to RFmx Analyze N Wfms (IQ).API for performing the measurement. 
//14. Fetch OFDMModAcc Measurements.
//14. Fetch the User specific results based on PPDU Type. 
//15. Close the RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.WlanMX;
using NationalInstruments.ModularInstruments.NIRfsgPlayback;

namespace NationalInstruments.Examples.RFmxWlanOfdmModAccMimoAnalysisOnly
{
    public class RFmxWlanOfdmModAccMimoAnalysisOnly
    {
        RFmxInstrMX instrSession;
        RFmxWlanMX wlan;

        int numberOfFrequencySegments;
        int numberOfReceiveChains;

        string segmentString;
        string chainString;

        double[] centerFrequency;
        ComplexWaveform<ComplexSingle>[] IQ;

        RFmxWlanMXStandard standard;

        double channelBandwidth;

        int measurementOffset;
        int maximumMeasurementLength;

        RFmxWlanMXOfdmModAccFrequencyErrorEstimationMethod frequencyErrorEstimationMethod;
        RFmxWlanMXOfdmModAccAmplitudeTrackingEnabled amplitudeTrackingEnabled;
        RFmxWlanMXOfdmModAccPhaseTrackingEnabled phaseTrackingEnabled;
        RFmxWlanMXOfdmModAccSymbolClockErrorCorrectionEnabled symbolClockErrorCorrectionEnabled;
        RFmxWlanMXOfdmModAccChannelEstimationType channelEstimationType;

        double timeout;

        double compositeRmsEvmMean;
        double compositeDataRmsEvmMean;
        double compositePilotRmsEvmMean;
        int numberOfSymbolsUsed;
        int numberOfWaveforms;

        RFmxWlanMXOfdmPpduType ppduType;
        RFmxWlanMXOfdmGuardIntervalType guardIntervalType;
        RFmxWlanMXOfdmModAccLSigParityCheckStatus lSigParityCheckStatus;
        RFmxWlanMXOfdmModAccSigCrcStatus sigCrcStatus;
        RFmxWlanMXOfdmModAccSigBCrcStatus sigBCrcStatus;

        int[] mcsIndex;
        int[] numberOfSpaceTimeStreams;

        int numberOfUsers;
        string userString;
        int numberOfStreamResults;

        double[] frequencyErrorMean;
        double[] symbolClockErrorMean;

        string streamString;
        string filePath;

        double[,] streamRmsEvmMean;
        double[,] streamDataRmsEvmMean;
        double[,] streamPilotRmsEvmMean;

        AnalogWaveform<float>[,] streamRmsEvmPerSubcarrierMean;
        ComplexSingle[,][] pilotConstellation;
        ComplexSingle[,][] dataConstellation;

        double[,] crossPowerMean;

        double[,] relativeIQOriginOffsetMean;
        double[,] iqGainImbalanceMean;
        double[,] iqQuadratureErrorMean;
        double[,] absoluteIQOriginOffsetMean;
        double[,] iqTimingSkewMean;

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
            numberOfWaveforms = 2;

            filePath = @"WLAN_80211n_20MHz_1Seg_2Chain_MIMO.tdms";

            numberOfFrequencySegments = 1;
            numberOfReceiveChains = 2;

            centerFrequency = new double[] { 5.18e9, 5.26e9 };              /* (Hz) */

            standard = RFmxWlanMXStandard.Standard802_11n;

            channelBandwidth = 20e6;                                                /*(Hz) */

            measurementOffset = 0;                                                  /* (symbols) */
            maximumMeasurementLength = 16;                                          /* (symbols) */

            frequencyErrorEstimationMethod = RFmxWlanMXOfdmModAccFrequencyErrorEstimationMethod.PreambleAndPilots;
            amplitudeTrackingEnabled = RFmxWlanMXOfdmModAccAmplitudeTrackingEnabled.False;
            phaseTrackingEnabled = RFmxWlanMXOfdmModAccPhaseTrackingEnabled.True;
            symbolClockErrorCorrectionEnabled = RFmxWlanMXOfdmModAccSymbolClockErrorCorrectionEnabled.True;
            channelEstimationType = RFmxWlanMXOfdmModAccChannelEstimationType.Reference;

            timeout = 10.0;                                                         /* (s) */

            ppduType = RFmxWlanMXOfdmPpduType.NonHT;
            guardIntervalType = RFmxWlanMXOfdmGuardIntervalType.OneByFour;
            lSigParityCheckStatus = RFmxWlanMXOfdmModAccLSigParityCheckStatus.NotApplicable;
            sigCrcStatus = RFmxWlanMXOfdmModAccSigCrcStatus.NotApplicable;
            sigBCrcStatus = RFmxWlanMXOfdmModAccSigBCrcStatus.NotApplicable;

            numberOfStreamResults = Int32.MinValue;

            frequencyErrorMean = new double[numberOfFrequencySegments];
            symbolClockErrorMean = new double[numberOfFrequencySegments];

            crossPowerMean = new double[numberOfFrequencySegments, numberOfReceiveChains];

            relativeIQOriginOffsetMean = new double[numberOfFrequencySegments, numberOfReceiveChains];
            iqGainImbalanceMean = new double[numberOfFrequencySegments, numberOfReceiveChains];
            iqQuadratureErrorMean = new double[numberOfFrequencySegments, numberOfReceiveChains];
            absoluteIQOriginOffsetMean = new double[numberOfFrequencySegments, numberOfReceiveChains];
            iqTimingSkewMean = new double[numberOfFrequencySegments, numberOfReceiveChains];
        }

        void InitializeInstr()
        {
            instrSession = new RFmxInstrMX("", "Analysisonly=1;MaxNumWfms:8");
        }

        void ConfigureWlan()
        {
            wlan = instrSession.GetWlanSignalConfiguration();     /* Create a new RFmx Session */
            wlan.ConfigureNumberOfFrequencySegmentsAndReceiveChains("", numberOfFrequencySegments, numberOfReceiveChains);
            for (int i = 0; i < numberOfFrequencySegments; ++i)
            {
                segmentString = RFmxWlanMX.BuildSegmentString("", i);
                wlan.ConfigureFrequency(segmentString, centerFrequency[i]);
            }

            wlan.ConfigureStandard("", standard);
            wlan.ConfigureChannelBandwidth("", channelBandwidth);
            wlan.SelectMeasurements("", RFmxWlanMXMeasurementTypes.OfdmModAcc, true);
            wlan.OfdmModAcc.Configuration.ConfigureMeasurementLength("", measurementOffset, maximumMeasurementLength);
            wlan.OfdmModAcc.Configuration.ConfigureFrequencyErrorEstimationMethod("", frequencyErrorEstimationMethod);
            wlan.OfdmModAcc.Configuration.ConfigureAmplitudeTrackingEnabled("", amplitudeTrackingEnabled);
            wlan.OfdmModAcc.Configuration.ConfigurePhaseTrackingEnabled("", phaseTrackingEnabled);
            wlan.OfdmModAcc.Configuration.ConfigureSymbolClockErrorCorrectionEnabled("",
                 symbolClockErrorCorrectionEnabled);
            wlan.OfdmModAcc.Configuration.ConfigureChannelEstimationType("", channelEstimationType);
            NIRfsgPlayback.ReadWaveformsFromFileComplex(filePath, numberOfWaveforms, ref IQ);
            wlan.AnalyzeNWaveformsIQ("", "", IQ, true);
        }

        void RetrieveResults()
        {
            wlan.OfdmModAcc.Results.FetchCompositeRmsEvm("", timeout, out compositeRmsEvmMean, out compositeDataRmsEvmMean,
               out compositePilotRmsEvmMean);
            wlan.OfdmModAcc.Results.FetchNumberOfSymbolsUsed("", timeout, out numberOfSymbolsUsed);
            wlan.OfdmModAcc.Results.FetchPpduType("", timeout, out ppduType);
            wlan.OfdmModAcc.Results.FetchGuardIntervalType("", timeout, out guardIntervalType);
            wlan.OfdmModAcc.Results.FetchLSigParityCheckStatus("", timeout, out lSigParityCheckStatus);
            wlan.OfdmModAcc.Results.FetchSigCrcStatus("", timeout, out sigCrcStatus);
            wlan.OfdmModAcc.Results.FetchSigBCrcStatus("", timeout, out sigBCrcStatus);
            if (ppduType == RFmxWlanMXOfdmPpduType.MU)
            {
                wlan.OfdmModAcc.Results.FetchNumberOfUsers("", timeout, out numberOfUsers);
                mcsIndex = new int[numberOfUsers];
                numberOfSpaceTimeStreams = new int[numberOfUsers];
                for (int i = 0; i < numberOfUsers; ++i)
                {
                    userString = RFmxWlanMX.BuildUserString("", i);
                    wlan.OfdmModAcc.Results.FetchMcsIndex(userString, timeout, out mcsIndex[i]);
                    wlan.OfdmModAcc.Results.FetchNumberOfSpaceTimeStreams(userString, timeout, out numberOfSpaceTimeStreams[i]);
                    int tempOffset;
                    wlan.OfdmModAcc.Results.GetSpaceTimeStreamOffset(userString, out tempOffset);
                    tempOffset = tempOffset + numberOfSpaceTimeStreams[i];
                    if (tempOffset > numberOfStreamResults)
                        numberOfStreamResults = tempOffset;
                }
            }
            else
            {
                mcsIndex = new int[1];
                numberOfSpaceTimeStreams = new int[1];
                wlan.OfdmModAcc.Results.FetchMcsIndex("", timeout, out mcsIndex[0]);
                wlan.OfdmModAcc.Results.FetchNumberOfSpaceTimeStreams("", timeout, out numberOfSpaceTimeStreams[0]);
                numberOfStreamResults = numberOfSpaceTimeStreams[0];
            }
            streamRmsEvmMean = new double[numberOfFrequencySegments, numberOfStreamResults];
            streamDataRmsEvmMean = new double[numberOfFrequencySegments, numberOfStreamResults];
            streamPilotRmsEvmMean = new double[numberOfFrequencySegments, numberOfStreamResults];
            streamRmsEvmPerSubcarrierMean = new AnalogWaveform<float>[numberOfFrequencySegments, numberOfStreamResults];
            pilotConstellation = new ComplexSingle[numberOfFrequencySegments, numberOfStreamResults][];
            dataConstellation = new ComplexSingle[numberOfFrequencySegments, numberOfStreamResults][];
            for (int i = 0; i < numberOfFrequencySegments; ++i)
            {
                segmentString = RFmxWlanMX.BuildSegmentString("", i);
                wlan.OfdmModAcc.Results.FetchFrequencyErrorMean(segmentString, timeout, out frequencyErrorMean[i]);
                wlan.OfdmModAcc.Results.FetchSymbolClockErrorMean(segmentString, timeout, out symbolClockErrorMean[i]);
                for (int j = 0; j < numberOfStreamResults; ++j)
                {
                    streamString = RFmxWlanMX.BuildStreamString(segmentString, j);
                    wlan.OfdmModAcc.Results.FetchStreamRmsEvm(streamString, timeout, out streamDataRmsEvmMean[i, j],
                       out streamDataRmsEvmMean[i, j], out streamPilotRmsEvmMean[i, j]);
                    wlan.OfdmModAcc.Results.FetchStreamRmsEvmPerSubcarrierMeanTrace(streamString, timeout,
                       ref streamRmsEvmPerSubcarrierMean[i, j]);
                    wlan.OfdmModAcc.Results.FetchPilotConstellationTrace(streamString, timeout, ref pilotConstellation[i, j]);
                    wlan.OfdmModAcc.Results.FetchDataConstellationTrace(streamString, timeout, ref dataConstellation[i, j]);
                }
                for (int j = 0; j < numberOfReceiveChains; ++j)
                {
                    chainString = RFmxWlanMX.BuildChainString(segmentString, j);
                    wlan.OfdmModAcc.Results.FetchCrossPower(chainString, timeout, out crossPowerMean[i, j]);
                    wlan.OfdmModAcc.Results.FetchIQImpairments(chainString, timeout, out relativeIQOriginOffsetMean[i, j],
                       out iqGainImbalanceMean[i, j], out iqQuadratureErrorMean[i, j], out absoluteIQOriginOffsetMean[i, j],
                       out iqTimingSkewMean[i, j]);
                }
            }
        }

        void PrintResults()
        {
            Console.WriteLine("-----------------------EVM-----------------------\n");
            Console.WriteLine("------------------Composite EVM------------------");
            Console.WriteLine("RMS EVM Mean (dB)                       : {0}", compositeRmsEvmMean);
            Console.WriteLine("Data RMS EVM Mean (dB)                  : {0}", compositeDataRmsEvmMean);
            Console.WriteLine("Pilot RMS EVM Mean (dB)                 : {0}\n", compositePilotRmsEvmMean);
            Console.WriteLine("Number of Symbols Used                  : {0}\n", numberOfSymbolsUsed);
            Console.WriteLine("\n--------------------------------------------------\n\n");

            for (int i = 0; i < numberOfFrequencySegments; ++i)
            {
                segmentString = RFmxWlanMX.BuildSegmentString("", i);
                Console.WriteLine("------------Measurements for {0}-------------\n", segmentString);
                for (int j = 0; j < numberOfStreamResults; ++j)
                {
                    streamString = RFmxWlanMX.BuildStreamString(segmentString, j);
                    Console.WriteLine("\n---------Measurements for {0}--------", streamString);
                    Console.WriteLine("Stream RMS EVM Mean (dB)                 : {0}", streamDataRmsEvmMean[i, j]);
                    Console.WriteLine("Stream Pilot RMS EVM Mean (dB)           : {0}", streamPilotRmsEvmMean[i, j]);
                    Console.WriteLine("Stream Data RMS EVM Mean (dB)            : {0}\n", streamDataRmsEvmMean[i, j]);
                }
                for (int j = 0; j < numberOfReceiveChains; ++j)
                {
                    chainString = RFmxWlanMX.BuildChainString(segmentString, j);
                    Console.WriteLine("\n---------Measurements for {0}---------", chainString);
                    Console.WriteLine("Cross Power Mean (dB)                   : {0}", crossPowerMean[i, j]);
                }


            }
            Console.WriteLine("\n--------------------------------------------------\n\n");

            Console.WriteLine("---------------------Impairments & PPDU Info--------------------");
            for (int i = 0; i < numberOfFrequencySegments; ++i)
            {
                segmentString = RFmxWlanMX.BuildSegmentString("", i);
                Console.WriteLine("\n---------Measurements for {0}---------\n", segmentString);
                Console.WriteLine("Frequency Error Mean (Hz)               : {0}", frequencyErrorMean[i]);
                Console.WriteLine("Symbol Clock Error Mean (ppm)           : {0}\n", symbolClockErrorMean[i]);
                for (int j = 0; j < numberOfReceiveChains; ++j)
                {
                    chainString = RFmxWlanMX.BuildChainString(segmentString, j);
                    Console.WriteLine("\n------------------IQ Impairments for {0}----------", chainString);
                    Console.WriteLine("Relative I/Q Origin Offset Mean (dB)    : {0}", relativeIQOriginOffsetMean[i, j]);
                    Console.WriteLine("Absolute I/Q Origin Offset Mean (dBm)   : {0}", absoluteIQOriginOffsetMean[i, j]);
                    Console.WriteLine("I/Q Gain Imbalance Mean (dB)            : {0}", iqGainImbalanceMean[i, j]);
                    Console.WriteLine("I/Q Quadrature Error Mean (deg)         : {0}", iqQuadratureErrorMean[i, j]);
                    Console.WriteLine("I/Q Timing Skew Mean (s)                : {0}\n", iqTimingSkewMean[i, j]);
                }
            }
            Console.WriteLine("\n--------------------------------------------------\n\n");

            Console.WriteLine("---------------------PPDU Info--------------------");
            Console.WriteLine("PPDU Type                               : {0}", ppduType);
            if (ppduType == RFmxWlanMXOfdmPpduType.MU)
            {
                for (int i = 0; i < numberOfUsers; ++i)
                {
                    Console.WriteLine("\nNSTS {0}                                  : {0}", i, numberOfSpaceTimeStreams[i]);
                    Console.WriteLine("MCS Index {0}                             : {0}\n", i, mcsIndex[i]);
                }
            }
            else
            {
                Console.WriteLine("NSTS                                    : {0}", numberOfSpaceTimeStreams[0]);
                Console.WriteLine("MCS Index                               : {0}", mcsIndex[0]);
            }
            Console.WriteLine("Guard Interval Type                     : {0}", guardIntervalType);
            Console.WriteLine("L-SIG Parity Check Status               : {0}", lSigParityCheckStatus);
            Console.WriteLine("SIG CRC Status                          : {0}", sigCrcStatus);
            Console.WriteLine("SIG-B CRC Status                        : {0}", sigBCrcStatus);

            Console.WriteLine("\n--------------------------------------------------\n\n");

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
