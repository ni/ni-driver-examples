//Steps:
//1. Open a new RFmx session.
//2. Configure the Frequency Reference properties (Clock Source and Clock Frequency).
//3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure IQ Power Edge Trigger properties (Trigger Delay, IQ Power Edge Level, Minimum Quiet Time).
//5. Configure Standard and Channel Bandwidth Properties.
//6. Select OFDMModAcc measurement and enable the traces.
//7. Configure the Measurement Interval.
//8. Configure Frequency Error Estimation Method.
//9. Configure Amplitude Tracking Enabled.
//10. Configure Phase Tracking Enabled.
//11. Configure Symbol Clock Error Correction Enabled.
//12. Configure Channel Estimation Type.
//13. Configure Averaging parameters.
//14. Initiate Measurement.
//15. Fetch OFDMModAcc Measurements.
//16. Close the RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.WlanMX;

namespace NationalInstruments.Examples.RFmxWlanOfdmModAcc
{
   public class RFmxWlanOfdmModAcc
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

      int measurementOffset;
      int maximumMeasurementLength;

      RFmxWlanMXOfdmModAccFrequencyErrorEstimationMethod frequencyErrorEstimationMethod;
      RFmxWlanMXOfdmModAccChannelEstimationType channelEstimationType;
      RFmxWlanMXOfdmModAccPhaseTrackingEnabled phaseTrackingEnabled;
      RFmxWlanMXOfdmModAccAmplitudeTrackingEnabled amplitudeTrackingEnabled;
      RFmxWlanMXOfdmModAccSymbolClockErrorCorrectionEnabled symbolClockErrorCorrectionEnabled;

      RFmxWlanMXOfdmModAccAveragingEnabled averagingEnabled;
      int averagingCount;

      double timeout;

      double compositeRmsEvmMean;
      double compositeDataRmsEvmMean;
      double compositePilotRmsEvmMean;
      int numberOfSymbolsUsed;
      double frequencyErrorMean;
      double symbolClockErrorMean;

      RFmxWlanMXOfdmPpduType ppduType;
      int mcsIndex;
      RFmxWlanMXOfdmGuardIntervalType guardIntervalType;
      RFmxWlanMXOfdmModAccLSigParityCheckStatus lSigParityCheckStatus;
      RFmxWlanMXOfdmModAccSigCrcStatus sigCrcStatus;
      RFmxWlanMXOfdmModAccSigBCrcStatus sigBCrcStatus;

      double relativeIQOriginOffsetMean;
      double iqGainImbalanceMean;
      double iqQuadratureErrorMean;
      double absoluteIQOriginOffsetMean;
      double iqTimingSkewMean;

      ComplexSingle[] pilotConstellation;
      ComplexSingle[] dataConstellation;
      AnalogWaveform<float> chainRmsEvmPerSubcarrierMean;

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
         iqPowerEdgeLevel = -20.0;                                               /* (dB) */
         triggerDelay = 0.0;                                                     /* (s) */
         minimumQuietTimeMode = RFmxWlanMXTriggerMinimumQuietTimeMode.Auto;
         minimumQuietTime = 5.0e-6;                                              /* (s) */

         standard = RFmxWlanMXStandard.Standard802_11ag;

         channelBandwidth = 20e6;                                                /* (Hz) */

         measurementOffset = 0;                                                  /* (symbols) */
         maximumMeasurementLength = 16;                                          /* (symbols) */

         frequencyErrorEstimationMethod = RFmxWlanMXOfdmModAccFrequencyErrorEstimationMethod.PreambleAndPilots;
         channelEstimationType = RFmxWlanMXOfdmModAccChannelEstimationType.Reference;
         phaseTrackingEnabled = RFmxWlanMXOfdmModAccPhaseTrackingEnabled.True;
         amplitudeTrackingEnabled = RFmxWlanMXOfdmModAccAmplitudeTrackingEnabled.False;
         symbolClockErrorCorrectionEnabled = RFmxWlanMXOfdmModAccSymbolClockErrorCorrectionEnabled.True;

         averagingEnabled = RFmxWlanMXOfdmModAccAveragingEnabled.False;
         averagingCount = 10;

         timeout = 10.0;                                                         /* (s) */

         ppduType = RFmxWlanMXOfdmPpduType.NonHT;
         guardIntervalType = RFmxWlanMXOfdmGuardIntervalType.OneByFour;
         lSigParityCheckStatus = RFmxWlanMXOfdmModAccLSigParityCheckStatus.NotApplicable;
         sigCrcStatus = RFmxWlanMXOfdmModAccSigCrcStatus.NotApplicable;
         sigBCrcStatus = RFmxWlanMXOfdmModAccSigBCrcStatus.NotApplicable;
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
         wlan.SelectMeasurements("", RFmxWlanMXMeasurementTypes.OfdmModAcc, true);
         wlan.OfdmModAcc.Configuration.ConfigureMeasurementLength("", measurementOffset, maximumMeasurementLength);
         wlan.OfdmModAcc.Configuration.ConfigureFrequencyErrorEstimationMethod("", frequencyErrorEstimationMethod);
         wlan.OfdmModAcc.Configuration.ConfigureAmplitudeTrackingEnabled("", amplitudeTrackingEnabled);
         wlan.OfdmModAcc.Configuration.ConfigurePhaseTrackingEnabled("", phaseTrackingEnabled);
         wlan.OfdmModAcc.Configuration.ConfigureSymbolClockErrorCorrectionEnabled("",
              symbolClockErrorCorrectionEnabled);
         wlan.OfdmModAcc.Configuration.ConfigureChannelEstimationType("", channelEstimationType);
         wlan.OfdmModAcc.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount);
         wlan.Initiate("", "");
      }

      void RetrieveResults()
      {
         wlan.OfdmModAcc.Results.FetchCompositeRmsEvm("", timeout, out compositeRmsEvmMean, out compositeDataRmsEvmMean,
            out compositePilotRmsEvmMean);
         wlan.OfdmModAcc.Results.FetchNumberOfSymbolsUsed("", timeout, out numberOfSymbolsUsed);
         wlan.OfdmModAcc.Results.FetchFrequencyErrorMean("", timeout, out frequencyErrorMean);
         wlan.OfdmModAcc.Results.FetchSymbolClockErrorMean("", timeout, out symbolClockErrorMean);
         wlan.OfdmModAcc.Results.FetchIQImpairments("", timeout, out relativeIQOriginOffsetMean,
             out iqGainImbalanceMean, out iqQuadratureErrorMean, out absoluteIQOriginOffsetMean, out iqTimingSkewMean);
         wlan.OfdmModAcc.Results.FetchPpduType("", timeout, out ppduType);
         wlan.OfdmModAcc.Results.FetchMcsIndex("", timeout, out mcsIndex);
         wlan.OfdmModAcc.Results.FetchGuardIntervalType("", timeout, out guardIntervalType);
         wlan.OfdmModAcc.Results.FetchLSigParityCheckStatus("", timeout, out lSigParityCheckStatus);
         wlan.OfdmModAcc.Results.FetchSigCrcStatus("", timeout, out sigCrcStatus);
         wlan.OfdmModAcc.Results.FetchSigBCrcStatus("", timeout, out sigBCrcStatus);
         wlan.OfdmModAcc.Results.FetchPilotConstellationTrace("", timeout, ref pilotConstellation);
         wlan.OfdmModAcc.Results.FetchDataConstellationTrace("", timeout, ref dataConstellation);
         wlan.OfdmModAcc.Results.FetchChainRmsEvmPerSubcarrierMeanTrace("", timeout, ref chainRmsEvmPerSubcarrierMean);
      }

      void PrintResults()
      {
         Console.WriteLine("------------------EVM------------------\n");
         Console.WriteLine("------------------Composite EVM------------------");
         Console.WriteLine("RMS EVM Mean (dB)                       : {0}", compositeRmsEvmMean);
         Console.WriteLine("Data RMS EVM Mean (dB)                  : {0}", compositeDataRmsEvmMean);
         Console.WriteLine("Pilot RMS EVM Mean (dB)                 : {0}\n", compositePilotRmsEvmMean);
         Console.WriteLine("Number of Symbols Used                  : {0}", numberOfSymbolsUsed);
         Console.WriteLine("\n------------------Impairments & PPDU Info------------------\n");
         Console.WriteLine("Frequency Error Mean (Hz)               : {0}", frequencyErrorMean);
         Console.WriteLine("Symbol Clock Error Mean (ppm)           : {0}", symbolClockErrorMean);
         Console.WriteLine("\n------------------IQ Impairments------------------");
         Console.WriteLine("Relative I/Q Origin Offset Mean (dB)    : {0}", relativeIQOriginOffsetMean);
         Console.WriteLine("Absolute I/Q Origin Offset Mean (dBm)   : {0}", absoluteIQOriginOffsetMean);
         Console.WriteLine("I/Q Gain Imbalance Mean (dB)            : {0}", iqGainImbalanceMean);
         Console.WriteLine("I/Q Quadrature Error Mean (deg)         : {0}", iqQuadratureErrorMean);
         Console.WriteLine("I/Q Timing Skew Mean (s)                : {0}", iqTimingSkewMean);
         Console.WriteLine("\n------------------PPDU Info------------------");
         Console.WriteLine("PPDU Type                               : {0}", ppduType);
         Console.WriteLine("MCS Index                               : {0}", mcsIndex);
         Console.WriteLine("Guard Interval Type                     : {0}", guardIntervalType);
         Console.WriteLine("L-SIG Parity Check Status               : {0}", lSigParityCheckStatus);
         Console.WriteLine("SIG CRC Status                          : {0}", sigCrcStatus);
         Console.WriteLine("SIG-B CRC Status                        : {0}\n", sigBCrcStatus);
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
