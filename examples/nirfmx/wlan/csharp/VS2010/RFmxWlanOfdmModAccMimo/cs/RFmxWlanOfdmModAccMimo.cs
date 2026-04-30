//Steps:
//1. Open a New RFmx session.
//2. Configure the Frequency Reference properties (Clock Source And Clock Frequency).
//3. Configure Number of Frequency Segment And Receive Chain.
//4. Configure Center Frequency for each Segment.
//5.Configure Selected Port.
//6. Configure the basic signal port specific properties ( Reference Level And External Attenuation).
//7. Configure IQ Power Edge Trigger properties (Trigger Delay, IQ Power Edge Level, Min Quiet Time).
//8. Configure Standard And Channel Bandwidth Properties.
//9. Select OFDMModAcc measurement And enable the traces. 
//10. Configure the Measurement Interval.
//11. Configure Frequency Error Estimation Method.
//12. Configure Amplitude Tracking Enabled.
//13. Configure Phase Tracking Enabled.
//14. Configure Symbol Clock Error Correction Enabled.
//15. Configure Channel Estimation Type.
//16. Configure Averaging parameters.
//17. Configure Channel Matrix Power Enabled.
//18. Initiate Measurement.
//19. Fetch OFDMModAcc Measurements.
//19. Fetch User Specific Results based on the PPDU Type. 
//20. Close the RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.WlanMX;

namespace NationalInstruments.Examples.RFmxWlanOfdmModAccMimo
{
   public class RFmxWlanOfdmModAccMimo
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

      int measurementOffset;
      int maximumMeasurementLength;

      RFmxWlanMXOfdmModAccFrequencyErrorEstimationMethod frequencyErrorEstimationMethod;
      RFmxWlanMXOfdmModAccAmplitudeTrackingEnabled amplitudeTrackingEnabled;
      RFmxWlanMXOfdmModAccPhaseTrackingEnabled phaseTrackingEnabled;
      RFmxWlanMXOfdmModAccSymbolClockErrorCorrectionEnabled symbolClockErrorCorrectionEnabled;
      RFmxWlanMXOfdmModAccChannelEstimationType channelEstimationType;
      RFmxWlanMXOfdmModAccChannelMatrixPowerEnabled channelMatrixPowerEnabled;

      RFmxWlanMXOfdmModAccAveragingEnabled averagingEnabled;
      int averagingCount;

      double timeout;

      double compositeRmsEvmMean;
      double compositeDataRmsEvmMean;
      double compositePilotRmsEvmMean;
      int numberOfSymbolsUsed;

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

         measurementOffset = 0;                                                  /* (symbols) */
         maximumMeasurementLength = 16;                                          /* (symbols) */

         frequencyErrorEstimationMethod = RFmxWlanMXOfdmModAccFrequencyErrorEstimationMethod.PreambleAndPilots;
         amplitudeTrackingEnabled = RFmxWlanMXOfdmModAccAmplitudeTrackingEnabled.False;
         phaseTrackingEnabled = RFmxWlanMXOfdmModAccPhaseTrackingEnabled.True;
         symbolClockErrorCorrectionEnabled = RFmxWlanMXOfdmModAccSymbolClockErrorCorrectionEnabled.True;
         channelEstimationType = RFmxWlanMXOfdmModAccChannelEstimationType.Reference;
         channelMatrixPowerEnabled = RFmxWlanMXOfdmModAccChannelMatrixPowerEnabled.True;

         averagingEnabled = RFmxWlanMXOfdmModAccAveragingEnabled.False;
         averagingCount = 10;

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
         wlan.SelectMeasurements("", RFmxWlanMXMeasurementTypes.OfdmModAcc, true);
         wlan.OfdmModAcc.Configuration.ConfigureMeasurementLength("", measurementOffset, maximumMeasurementLength);
         wlan.OfdmModAcc.Configuration.ConfigureFrequencyErrorEstimationMethod("", frequencyErrorEstimationMethod);
         wlan.OfdmModAcc.Configuration.ConfigureAmplitudeTrackingEnabled("", amplitudeTrackingEnabled);
         wlan.OfdmModAcc.Configuration.ConfigurePhaseTrackingEnabled("", phaseTrackingEnabled);
         wlan.OfdmModAcc.Configuration.ConfigureSymbolClockErrorCorrectionEnabled("",
              symbolClockErrorCorrectionEnabled);
         wlan.OfdmModAcc.Configuration.ConfigureChannelEstimationType("", channelEstimationType);
         wlan.OfdmModAcc.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount);
         wlan.OfdmModAcc.Configuration.SetChannelMatrixPowerEnabled("", channelMatrixPowerEnabled);
         wlan.Initiate("", "");
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
            for (int i=0; i<numberOfUsers;++i)
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
            wlan.OfdmModAcc.Results.FetchFrequencyErrorMean("", timeout, out frequencyErrorMean[i]);
            wlan.OfdmModAcc.Results.FetchSymbolClockErrorMean("", timeout, out symbolClockErrorMean[i]);
            for (int j=0; j<numberOfStreamResults; ++j)
            {
               streamString = RFmxWlanMX.BuildStreamString(segmentString, j);
               wlan.OfdmModAcc.Results.FetchStreamRmsEvm(streamString, timeout, out streamDataRmsEvmMean[i,j], 
                  out streamDataRmsEvmMean[i,j], out streamPilotRmsEvmMean[i,j]);
               wlan.OfdmModAcc.Results.FetchStreamRmsEvmPerSubcarrierMeanTrace(streamString, timeout, 
                  ref streamRmsEvmPerSubcarrierMean[i,j]);
               wlan.OfdmModAcc.Results.FetchPilotConstellationTrace(streamString, timeout, ref pilotConstellation[i,j]);
               wlan.OfdmModAcc.Results.FetchDataConstellationTrace(streamString, timeout, ref dataConstellation[i, j]);
            }
            for (int j = 0; j < numberOfReceiveChains; ++j)
            {
               chainString = RFmxWlanMX.BuildChainString(segmentString, j);
               wlan.OfdmModAcc.Results.FetchCrossPower(segmentString, timeout, out crossPowerMean[i,j]);
               wlan.OfdmModAcc.Results.FetchIQImpairments(segmentString, timeout, out relativeIQOriginOffsetMean[i,j],
                  out iqGainImbalanceMean[i,j], out iqQuadratureErrorMean[i,j], out absoluteIQOriginOffsetMean[i,j],
                  out iqTimingSkewMean[i,j]);
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
         for (int i = 0; i < numberOfFrequencySegments; ++i)
         {
            segmentString = RFmxWlanMX.BuildSegmentString("", i);
            Console.WriteLine("------------Measurements for {0}-------------\n", segmentString);
            Console.WriteLine("Frequency Error Mean (Hz)               : {0}", frequencyErrorMean[i]);
            Console.WriteLine("Symbol Clock Error Mean (ppm)           : {0}\n", symbolClockErrorMean[i]);
            for (int j = 0; j < numberOfStreamResults; ++j)
            {
               streamString = RFmxWlanMX.BuildStreamString(segmentString, j);
               Console.WriteLine("\n---------Measurements for {0}--------", streamString);
               Console.WriteLine("Stream RMS EVM Mean (dB)                 : {0}", streamDataRmsEvmMean[i,j]);
               Console.WriteLine("Stream Pilot RMS EVM Mean (dB)           : {0}", streamPilotRmsEvmMean[i, j]);
               Console.WriteLine("Stream Data RMS EVM Mean (dB)            : {0}\n", streamDataRmsEvmMean[i, j]);
            }
            for (int j = 0; j < numberOfReceiveChains; ++j)
            {
               chainString = RFmxWlanMX.BuildChainString(segmentString, j);
               Console.WriteLine("\n---------Measurements for {0}---------", chainString);
               Console.WriteLine("Cross Power Mean (dB)                   : {0}", crossPowerMean[i,j]);
               Console.WriteLine("\n------------------IQ Impairments------------------");
               Console.WriteLine("Relative I/Q Origin Offset Mean (dB)    : {0}", relativeIQOriginOffsetMean[i,j]);
               Console.WriteLine("Absolute I/Q Origin Offset Mean (dBm)   : {0}", absoluteIQOriginOffsetMean[i,j]);
               Console.WriteLine("I/Q Gain Imbalance Mean (dB)            : {0}", iqGainImbalanceMean[i,j]);
               Console.WriteLine("I/Q Quadrature Error Mean (deg)         : {0}", iqQuadratureErrorMean[i,j]);
               Console.WriteLine("I/Q Timing Skew Mean (s)                : {0}\n", iqTimingSkewMean[i,j]);
            }
            Console.WriteLine("\n--------------------------------------------------\n\n");
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
