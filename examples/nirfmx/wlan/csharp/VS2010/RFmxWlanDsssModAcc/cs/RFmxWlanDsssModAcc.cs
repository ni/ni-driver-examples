//Steps:
//1. Open a new RFmx session.
//2.  Configure the frequency reference properties (Clock Source and Clock Frequency).
//3. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation).
//4. Configure IQ Power Edge Trigger properties (Trigger Delay, IQ Power Edge Level, Minimum Quiet Time).
//5. Configure Standard to 802.11b.
//6. Select DSSSModAcc measurement and enable the traces.
//7. Configure Measurement Length.
//8. Configure Pulse Shaping Filter Type and Parameter.
//9. Configure EVM unit.
//10. Configure Averaging parameters.
//11. Initiate Measurement.
//12. Fetch DSSSModAcc Traces and Measurements.
//13. Close the RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.WlanMX;

namespace NationalInstruments.Examples.RFmxWlanDsssModAcc
{
   public class RFmxWlanDsssModAcc
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

      int measurementOffset;
      int maximumMeasurementLength;

      RFmxWlanMXDsssModAccPulseShapingFilterType pulseShapingFilterType;
      double pulseShapingFilterParameter;

      RFmxWlanMXDsssModAccEvmUnit evmUnit;

      RFmxWlanMXDsssModAccAveragingEnabled averagingEnabled;
      int averagingCount;

      double timeout;

      double rmsEvmMean;
      double peakEvm80211_2016Maximum;
      double peakEvm80211_2007Maximum;
      double peakEvm80211_1999Maximum;
      double frequencyErrorMean;
      double chipClockErrorMean;
      int numberOfChipsUsed;

      RFmxWlanMXDsssModAccDataModulationFormat dataModulationFormat;
      int payloadLength;
      RFmxWlanMXDsssModAccPreambleType preambleType;
      int lockedClocksBit;
      RFmxWlanMXDsssModAccPayloadHeaderCrcStatus headerCrcStatus;
      RFmxWlanMXDsssModAccPsduCrcStatus psduCrcStatus;

      double iqOriginOffsetMean;
      double iqGainImbalanceMean;
      double iqQuadratureErrorMean;

      AnalogWaveform<float> evmPerChipMean;
      ComplexSingle[] constellation;

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

         standard = RFmxWlanMXStandard.Standard802_11b;

         measurementOffset = 0;                                                  /*(chips)*/
         maximumMeasurementLength = 1000;                                        /*(chips)*/

         pulseShapingFilterType = RFmxWlanMXDsssModAccPulseShapingFilterType.Rectangular;
         pulseShapingFilterParameter = 0.50;

         evmUnit = RFmxWlanMXDsssModAccEvmUnit.Percentage;

         averagingEnabled = RFmxWlanMXDsssModAccAveragingEnabled.False;
         averagingCount = 10;

         timeout = 10.0;                                                         /* (s) */

         rmsEvmMean = 0.0;                                                       /*(% or dB) */
         peakEvm80211_2016Maximum = 0.0;                                         /*(% or dB) */
         peakEvm80211_2007Maximum = 0.0;                                         /*(% or dB) */
         peakEvm80211_1999Maximum = 0.0;                                         /*(% or dB) */
         frequencyErrorMean = 0.0;                                               /*(Hz) */
         chipClockErrorMean = 0.0;                                               /*(ppm) */
         numberOfChipsUsed = 0;

         dataModulationFormat = RFmxWlanMXDsssModAccDataModulationFormat.Dsss1Mbps;
         payloadLength = 0;                                                      /*(byte) */
         preambleType = RFmxWlanMXDsssModAccPreambleType.Long;
         lockedClocksBit = 0;
         headerCrcStatus = RFmxWlanMXDsssModAccPayloadHeaderCrcStatus.Fail;
         psduCrcStatus = RFmxWlanMXDsssModAccPsduCrcStatus.Fail;

         iqOriginOffsetMean = 0.0;                                               /*(dB) */
         iqGainImbalanceMean = 0.0;                                              /*(dB) */
         iqQuadratureErrorMean = 0.0;                                            /*(deg) */
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
         wlan.SelectMeasurements("", RFmxWlanMXMeasurementTypes.DsssModAcc, true);
         wlan.DsssModAcc.Configuration.ConfigureMeasurementLength("", measurementOffset, maximumMeasurementLength);
         wlan.DsssModAcc.Configuration.SetPulseShapingFilterType("", pulseShapingFilterType);
         wlan.DsssModAcc.Configuration.SetPulseShapingFilterParameter("", pulseShapingFilterParameter);
         wlan.DsssModAcc.Configuration.ConfigureEvmUnit("", evmUnit);
         wlan.DsssModAcc.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount);
         wlan.Initiate("", "");
      }

      void RetrieveResults()
      {
         wlan.DsssModAcc.Results.FetchEvm("", timeout, out rmsEvmMean, out peakEvm80211_2016Maximum, out peakEvm80211_2007Maximum,
             out peakEvm80211_1999Maximum, out frequencyErrorMean, out chipClockErrorMean, out numberOfChipsUsed);
         wlan.DsssModAcc.Results.FetchPpduInformation("", timeout, out dataModulationFormat, out payloadLength,
              out preambleType, out lockedClocksBit, out headerCrcStatus, out psduCrcStatus);
         wlan.DsssModAcc.Results.FetchIQImpairments("", timeout, out iqOriginOffsetMean, out iqGainImbalanceMean, out iqQuadratureErrorMean);
         wlan.DsssModAcc.Results.FetchEvmPerChipMeanTrace("", timeout, ref evmPerChipMean);
         wlan.DsssModAcc.Results.FetchConstellationTrace("", timeout, ref constellation);
      }

      void PrintResults()
      {
         Console.WriteLine("\n---------------EVM---------------\n");
         Console.WriteLine("RMS EVM Mean (% or dB)                     :{0}", rmsEvmMean);
         Console.WriteLine("Peak EVM (802.11-2016) Maximum (% or dB)   :{0}", peakEvm80211_2016Maximum);
         Console.WriteLine("Peak EVM (802.11-2007) Maximum (% or dB)   :{0}", peakEvm80211_2007Maximum);
         Console.WriteLine("Peak EVM (802.11-1999) Maximum (% or dB)   :{0}", peakEvm80211_1999Maximum);
         Console.WriteLine("Number of Chips Used                       :{0}", numberOfChipsUsed);
         Console.WriteLine("\n---------------Impairments & PPDU Info---------------\n");
         Console.WriteLine("Frequency Error Mean (Hz)                  :{0}", frequencyErrorMean);
         Console.WriteLine("Chip Clock Error Mean (ppm)                :{0}", chipClockErrorMean);

         Console.WriteLine("\n---------------IQ Impairments---------------\n");
         Console.WriteLine("I/Q Origin Offset Mean (dB)                :{0}", iqOriginOffsetMean);
         Console.WriteLine("I/Q Gain Imbalance Mean (dB)               :{0}", iqGainImbalanceMean);
         Console.WriteLine("I/Q Quadrature Error Mean (deg)            :{0}", iqQuadratureErrorMean);

         Console.WriteLine("\n---------------PPDU Information---------------\n");
         Console.WriteLine("Data Modulation Format                     :{0}", dataModulationFormat);
         Console.WriteLine("Payload Length (bytes)                     :{0}", payloadLength);
         Console.WriteLine("Preamble Type                              :{0}", preambleType);
         Console.WriteLine("Locked Clock Bit                           :{0}", lockedClocksBit);
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
