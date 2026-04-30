//Steps:
//1. Open a new RFmx session.
//2. Configure the Frequency Reference properties (Clock Source and Clock Frequency).
//3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure IQ Power Edge Trigger properties (Trigger Delay, IQ Power Edge Level, Minimum Quiet Time).
//5. Configure Standard to 802.11ax and Channel Bandwidth properties.
//6. Configure MCS Index, RU Size, RU Offset, RU Type, Distribution Bandwidth, Guard Interval Type, LTF Size and PE Disambiguity.
//7. Select OFDMModAcc measurement and enable the traces.
//8. Configure Measurement Interval.
//9. Configure Unused Tone Error Mask Reference.
//10. Configure Averaging parameters.
//11. Initiate Measurement.
//12. Fetch OFDMModAcc Measurements.
//13. Close the RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.WlanMX;

namespace NationalInstruments.Examples.RFmxWlanOfdmModAccTriggerBasedPpdu
{
   public class RFmxWlanOfdmModAccTriggerBasedPpdu
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
      double distributionBandwidth;

      int mcsIndex;
      int ruSize;
      int ruOffsetMruIndex;
      RFmxWlanMXOfdmRUType ruType;
      RFmxWlanMXOfdmGuardIntervalType guardIntervalType;
      RFmxWlanMXOfdmLtfSize ltfSize;
      int peDisambiguity;

      int measurementOffset;
      int maximumMeasurementLength;

      RFmxWlanMXOfdmModAccUnusedToneErrorMaskReference unusedToneErrorMaskReference;

      RFmxWlanMXOfdmModAccAveragingEnabled averagingEnabled;
      int averagingCount;

      double timeout;

      double compositeRmsEvmMean;
      double compositeDataRmsEvmMean;
      double compositePilotRmsEvmMean;

      double unusedToneErrorMargin;
      int unusedToneErrorMarginRUIndex;

      double[] unusedToneErrorMarginPerRU;
      double frequencyErrorMean;
      double frequencyErrorCcdf10Percent;
      double symbolClockErrorMean;
      RFmxWlanMXOfdmPpduType ppduType;

      ComplexSingle[] pilotConstellation;
      ComplexSingle[] dataConstellation;
      AnalogWaveform<float> unusedToneError;
      AnalogWaveform<float> unusedToneErrorMask;

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

         standard = RFmxWlanMXStandard.Standard802_11ax;

         channelBandwidth = 20e6;                                                /*(Hz) */
         distributionBandwidth = 20e6;                                           /*(Hz) */

         mcsIndex = 0;
         ruSize = 26;
         ruOffsetMruIndex = 0;
         ruType = RFmxWlanMXOfdmRUType.Rru;
         guardIntervalType = RFmxWlanMXOfdmGuardIntervalType.OneByFour;
         ltfSize = RFmxWlanMXOfdmLtfSize.LtfSize4x;
         peDisambiguity = 0;

         measurementOffset = 0;                                                  /* (symbols) */
         maximumMeasurementLength = 16;                                          /* (symbols) */

         unusedToneErrorMaskReference = RFmxWlanMXOfdmModAccUnusedToneErrorMaskReference.Limit1;

         averagingEnabled = RFmxWlanMXOfdmModAccAveragingEnabled.False;
         averagingCount = 10;

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
         wlan.SetOfdmMcsIndex("", mcsIndex);
         wlan.SetOfdmRUSize("", ruSize);
         wlan.SetOfdmRUOffsetMruIndex("", ruOffsetMruIndex);
         wlan.SetOfdmRUType("", ruType);
         wlan.SetOfdmDistributionBandwidth("", distributionBandwidth);
         wlan.SetOfdmGuardIntervalType("", guardIntervalType);
         wlan.SetOfdmLtfSize("", ltfSize);
         wlan.SetOfdmPEDisambiguity("", peDisambiguity);
         wlan.SelectMeasurements("", RFmxWlanMXMeasurementTypes.OfdmModAcc, true);
         wlan.OfdmModAcc.Configuration.ConfigureMeasurementLength("", measurementOffset, maximumMeasurementLength);
         wlan.OfdmModAcc.Configuration.SetUnusedToneErrorMaskReference("", unusedToneErrorMaskReference);
         wlan.OfdmModAcc.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount);
         wlan.Initiate("", "");
      }

      void RetrieveResults()
      {
         wlan.OfdmModAcc.Results.FetchCompositeRmsEvm("", timeout, out compositeRmsEvmMean, out compositeDataRmsEvmMean, out compositePilotRmsEvmMean);
         wlan.OfdmModAcc.Results.FetchUnusedToneError("", timeout, out unusedToneErrorMargin, out unusedToneErrorMarginRUIndex);
         wlan.OfdmModAcc.Results.FetchUnusedToneErrorMarginPerRU("", timeout, ref unusedToneErrorMarginPerRU);
         wlan.OfdmModAcc.Results.FetchFrequencyErrorMean("", timeout, out frequencyErrorMean);
         wlan.OfdmModAcc.Results.FetchFrequencyErrorCcdf10Percent("", timeout, out frequencyErrorCcdf10Percent);
         wlan.OfdmModAcc.Results.FetchSymbolClockErrorMean("", timeout, out symbolClockErrorMean);
         wlan.OfdmModAcc.Results.FetchPpduType("", timeout, out ppduType);
         wlan.OfdmModAcc.Results.FetchPilotConstellationTrace("", timeout, ref pilotConstellation);
         wlan.OfdmModAcc.Results.FetchDataConstellationTrace("", timeout, ref dataConstellation);
         wlan.OfdmModAcc.Results.FetchUnusedToneErrorMeanTrace("", timeout, ref unusedToneError, ref unusedToneErrorMask);
      }

      void PrintResults()
      {
         Console.WriteLine("------------------EVM & Impairments------------------\n");
         Console.WriteLine("RMS EVM Mean (dB)                       :{0}", compositeRmsEvmMean);
         Console.WriteLine("Frequency Error Mean (Hz)               :{0}", frequencyErrorMean);
         Console.WriteLine("Frequency Error CCDF 10 % (Hz)          :{0}", frequencyErrorCcdf10Percent);
         Console.WriteLine("Symbol Clock Error Mean (ppm)           :{0}", symbolClockErrorMean);
         Console.WriteLine("PPDU Type                               :{0}", ppduType);
         Console.WriteLine("\n------------------Unused Tone Error------------------\n");
         Console.WriteLine("Margin (dB)                             :{0}", unusedToneErrorMargin);
         Console.WriteLine("Margin RU Index                         :{0}", unusedToneErrorMarginRUIndex);
         for (int i = 0; i < unusedToneErrorMarginPerRU.Length; i++)
         {
            Console.WriteLine("Unused Tone Error Margin per RU(dB)     :{0}", unusedToneErrorMarginPerRU[i]);
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
