//Steps:
//1. Open a new RFmx session.
//2. Configure the Frequency Reference properties (Clock Source and Clock Frequency).
//3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure IQ Power Edge Trigger properties (Trigger Delay, IQ Power Edge Level, Minimum Quiet Time).
//5. Configure Standard and Channel Bandwidth Properties.
//6. Disable auto PPDU type detection and header decoding. Configure PPDU Type and properties that are otherwise decoded from Header.
//7. Select OFDMModAcc measurement and disable the traces.
//8. Configure the Measurement Interval. Make sure that the input signal has number of symbols at least equal to the specified Maximum Measurement Length.
//9. Configure Frequency Error Estimation Method. You may set Frequency Error Estimation Method to disabled to optimize speed of the measurement
//   when there is no frequency error between transmitter and receiver.
//10. Configure Amplitude Tracking Enabled.
//11. Configure  Symbol Clock Error Correction Enabled. You may set Symbol Clock Correction Enabled to False to optimize speed of the measurement
//    when there is no symbol clock error between transmitter and receiver.
//12. Configure Averaging parameters.
//13. Disable burst start detection and I/Q impairments estimation.
//14. Initiate Measurement.
//15. Fetch OFDMModAcc Measurements.
//16. Close the RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.WlanMX;

namespace NationalInstruments.Examples.RFmxWlanOfdmModAccFemTestSpeedOptimized
{
   public class RFmxWlanOfdmModAccFemTestSpeedOptimized
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

      RFmxWlanMXOfdmAutoPpduTypeDetectionEnabled autoPpduTypeDetectionEnabled;
      RFmxWlanMXOfdmPpduType ppduType;
      RFmxWlanMXOfdmHeaderDecodingEnabled headerDecodingEnabled;
      int mcsIndex;
      RFmxWlanMXOfdmGuardIntervalType guardIntervalType;
      RFmxWlanMXOfdmLtfSize ltfSize;
      int RUSize;
      int numberOfSigSymbols;

      RFmxWlanMXOfdmModAccBurstStartDetectionEnabled burstStartDetectionEnabled;
      RFmxWlanMXOfdmModAccIQImpairmentsEstimationEnabled iqImpairmentsEstimationEnabled;

      RFmxWlanMXOfdmModAccAveragingEnabled averagingEnabled;
      int averagingCount;

      int measurementOffset;
      int maximumMeasurementLength;
      RFmxWlanMXOfdmModAccFrequencyErrorEstimationMethod frequencyErrorEstimationMethod;

      RFmxWlanMXOfdmModAccAmplitudeTrackingEnabled amplitudeTrackingEnabled;
      RFmxWlanMXOfdmModAccSymbolClockErrorCorrectionEnabled symbolClockErrorCorrectionEnabled;

      double timeout;

      double compositeRmsEvmMean;
      double compositeDataRmsEvmMean;
      double compositePilotRmsEvmMean;
      int numberOfSymbolsUsed;
      double frequencyErrorMean;
      double symbolClockErrorMean;

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

         autoPpduTypeDetectionEnabled = RFmxWlanMXOfdmAutoPpduTypeDetectionEnabled.False;
         ppduType = RFmxWlanMXOfdmPpduType.NonHT;
         headerDecodingEnabled = RFmxWlanMXOfdmHeaderDecodingEnabled.False;
         mcsIndex = 0;
         guardIntervalType = RFmxWlanMXOfdmGuardIntervalType.OneByFour;
         ltfSize = RFmxWlanMXOfdmLtfSize.LtfSize4x;
         RUSize = 26;
         numberOfSigSymbols = 1;

         burstStartDetectionEnabled = RFmxWlanMXOfdmModAccBurstStartDetectionEnabled.False;
         iqImpairmentsEstimationEnabled = RFmxWlanMXOfdmModAccIQImpairmentsEstimationEnabled.False;

         amplitudeTrackingEnabled = RFmxWlanMXOfdmModAccAmplitudeTrackingEnabled.False;
         symbolClockErrorCorrectionEnabled = RFmxWlanMXOfdmModAccSymbolClockErrorCorrectionEnabled.False;

         averagingEnabled = RFmxWlanMXOfdmModAccAveragingEnabled.False;
         averagingCount = 10;

         measurementOffset = 0;                                                  /* (symbols) */
         maximumMeasurementLength = 16;                                          /* (symbols) */
         frequencyErrorEstimationMethod = RFmxWlanMXOfdmModAccFrequencyErrorEstimationMethod.Disabled;

         timeout = 10.0;                                                         /* (s) */

         compositeRmsEvmMean = 0.0;                                              /* (dB) */
         compositeDataRmsEvmMean = 0.0;                                          /* (dB) */
         compositePilotRmsEvmMean = 0.0;                                         /* (dB) */
         numberOfSymbolsUsed = 0;
         frequencyErrorMean = 0.0;                                               /* (Hz) */
         symbolClockErrorMean = 0.0;                                             /* (ppm) */
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
         wlan.SetOfdmAutoPpduTypeDetectionEnabled("", autoPpduTypeDetectionEnabled);
         wlan.SetOfdmPpduType("", ppduType);
         wlan.SetOfdmHeaderDecodingEnabled("", headerDecodingEnabled);
         wlan.SetOfdmMcsIndex("", mcsIndex);
         wlan.SetOfdmGuardIntervalType("", guardIntervalType);
         wlan.SetOfdmLtfSize("", ltfSize);
         wlan.SetOfdmRUSize("", RUSize);
         wlan.SetOfdmNumberOfSigSymbols("", numberOfSigSymbols);
         wlan.SelectMeasurements("", RFmxWlanMXMeasurementTypes.OfdmModAcc, true);
         wlan.OfdmModAcc.Configuration.ConfigureMeasurementLength("", measurementOffset, maximumMeasurementLength);
         wlan.OfdmModAcc.Configuration.ConfigureFrequencyErrorEstimationMethod("", frequencyErrorEstimationMethod);
         wlan.OfdmModAcc.Configuration.ConfigureAmplitudeTrackingEnabled("", amplitudeTrackingEnabled);
         wlan.OfdmModAcc.Configuration.ConfigureSymbolClockErrorCorrectionEnabled("",
              symbolClockErrorCorrectionEnabled);
         wlan.OfdmModAcc.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount);
         wlan.OfdmModAcc.Configuration.SetBurstStartDetectionEnabled("", burstStartDetectionEnabled);
         wlan.OfdmModAcc.Configuration.SetIQImpairmentsEstimationEnabled("", iqImpairmentsEstimationEnabled);
         wlan.Initiate("", "");
      }

      void RetrieveResults()
      {
         wlan.OfdmModAcc.Results.FetchCompositeRmsEvm("", timeout, out compositeRmsEvmMean, out compositeDataRmsEvmMean, out compositePilotRmsEvmMean);
         wlan.OfdmModAcc.Results.FetchNumberOfSymbolsUsed("", timeout, out numberOfSymbolsUsed);
         wlan.OfdmModAcc.Results.FetchFrequencyErrorMean("", timeout, out frequencyErrorMean);
         wlan.OfdmModAcc.Results.FetchSymbolClockErrorMean("", timeout, out symbolClockErrorMean);
      }

      void PrintResults()
      {
         Console.WriteLine("------------------Composite EVM------------------");
         Console.WriteLine("RMS EVM Mean (dB)                       :{0}", compositeRmsEvmMean);
         Console.WriteLine("Data RMS EVM Mean (dB)                  :{0}", compositeDataRmsEvmMean);
         Console.WriteLine("Pilot RMS EVM Mean (dB)                 :{0}\n", compositePilotRmsEvmMean);
         Console.WriteLine("Number of Symbols Used                  :{0}", numberOfSymbolsUsed);
         Console.WriteLine("Frequency Error Mean(Hz)                :{0}", frequencyErrorMean);
         Console.WriteLine("Symbol Clock Error Mean(ppm)            :{0}", symbolClockErrorMean);
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
