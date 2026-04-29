//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Packet Type.
//6. Configure Payload Length.
//7. Select ModAcc, ACP and TXP measurements.
//8. Configure Averaging Parameters for ModAcc measurement.
//9. Configure Averaging Parameters for ACP measurement.
//10. Configure Averaging Parameters for TXP measurement.
//11. Configure ACP Number of Offsets.
//12. Configure ACP Offset Channel Mode.
//13. Initiate the Measurement.
//14. Fetch ModAcc measurements.
//15. Fetch ACP measurements.
//16. Fetch TXP measurements.
//17. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.BTMX;

namespace NationalInstruments.Examples.RFmxBTModAccAcpTxpEdrComposite
{
   public class RFmxBTModAccAcpTxpEdrComposite
   {
      RFmxInstrMX instrSession;
      RFmxBTMX BT;
      string rfsaResourceName;

      string frequencyReferenceSource;
      double frequencyReferenceFrequency;

      double centerFrequency;
      double referenceLevel;
      double externalAttenuation;

      bool enableTrigger;
      RFmxBTMXIQPowerEdgeTriggerSlope iqPowerEdgeTriggerSlope;
      double iqPowerEdgeTriggerLevel;
      RFmxBTMXTriggerMinimumQuietTimeMode minimumQuiteTimeMode;
      double minimumQuietTime;
      RFmxBTMXIQPowerEdgeTriggerLevelType iqPowerEdgeTriggerLevelType;
      double triggerDelay;

      RFmxBTMXPacketType packetType;

      RFmxBTMXPayloadLengthMode payloadLengthMode;
      int payloadLength;

      RFmxBTMXMeasurementTypes measurements;
      bool enableAllTraces;

      int averagingCount;

      RFmxBTMXAcpOffsetChannelMode offsetChannelMode;
      int numberOfOffsets;

      double timeout;

      double peakRmsDevmMaximum;
      double peakDevmMaximum;
      double ninetyninePercentDevm;

      double headerFrequencyErrorWiMaximum;
      double peakFrequencyErrorWiPlusW0Maximum;
      double peakFrequencyErrorW0Maximum;

      double referenceChannelPower;

      double[] lowerAbsolutePower;
      double[] upperAbsolutePower;
      double[] lowerRelativePower;
      double[] upperRelativePower;
      double[] lowerMargin;
      double[] upperMargin;

      double averagePowerMean;
      double averagePowerMaximum;
      double averagePowerMinimum;
      double peakToAveragePowerRatioMaximum;
      double edrGfskAveragePowerMean;
      double edrDpskAveragePowerMean;
      double edrDpskGfskAveragePowerRatioMean;

      public void Run()
      {
         try
         {
            InitializeVariables();
            InitializeInstr();
            ConfigureBT();
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
         rfsaResourceName = "RFSA";

         frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
         frequencyReferenceFrequency = 10e6;                                     /* (Hz) */

         centerFrequency = 2.402e9;                                              /* (Hz) */
         referenceLevel = 0.00;                                                  /* (dBm) */
         externalAttenuation = 0.0;                                              /* (dB) */

         enableTrigger = true;
         iqPowerEdgeTriggerSlope = RFmxBTMXIQPowerEdgeTriggerSlope.Rising;
         iqPowerEdgeTriggerLevel = -20.0;                                        /* (dB) */
         minimumQuiteTimeMode = RFmxBTMXTriggerMinimumQuietTimeMode.Auto;
         minimumQuietTime = 100e-6;                                              /*(seconds) */
         iqPowerEdgeTriggerLevelType = RFmxBTMXIQPowerEdgeTriggerLevelType.Relative;
         triggerDelay = 0.0;                                                     /*(seconds) */

         packetType = RFmxBTMXPacketType.PacketType2DH1;

         payloadLengthMode = RFmxBTMXPayloadLengthMode.Auto;
         payloadLength = 10;                                                      /*(bytes) */

         measurements = RFmxBTMXMeasurementTypes.ModAcc | RFmxBTMXMeasurementTypes.Acp | RFmxBTMXMeasurementTypes.Txp;
         enableAllTraces = false;

         averagingCount = 10;

         offsetChannelMode = RFmxBTMXAcpOffsetChannelMode.Symmetric;
         numberOfOffsets = 5;
         timeout = 10.0;                                                         /*seconds */

         referenceChannelPower = 0.0;                                            /*(dBm)*/
      }

      void InitializeInstr()
      {
         instrSession = new RFmxInstrMX(rfsaResourceName, "");
      }

      void ConfigureBT()
      {
         BT = instrSession.GetBTSignalConfiguration();       /* Create a new RFmx Session */
         instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
         BT.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
         BT.ConfigureIQPowerEdgeTrigger("", "0", iqPowerEdgeTriggerSlope, iqPowerEdgeTriggerLevel,
             triggerDelay, minimumQuiteTimeMode, minimumQuietTime, iqPowerEdgeTriggerLevelType,
             enableTrigger);
         BT.ConfigurePacketType("", packetType);
         BT.ConfigurePayloadLength("", payloadLengthMode, payloadLength);
         BT.SelectMeasurements("", measurements, enableAllTraces);
         BT.ModAcc.Configuration.ConfigureAveraging("", RFmxBTMXModAccAveragingEnabled.False, averagingCount);
         BT.Acp.Configuration.ConfigureAveraging("", RFmxBTMXAcpAveragingEnabled.False, averagingCount);
         BT.Txp.Configuration.ConfigureAveraging("", RFmxBTMXTxpAveragingEnabled.False, averagingCount);
         BT.Acp.Configuration.ConfigureNumberOfOffsets("", numberOfOffsets);
         BT.Acp.Configuration.ConfigureOffsetChannelMode("", offsetChannelMode);
         BT.Initiate("", "");
      }

      void RetrieveResults()
      {
         BT.ModAcc.Results.FetchDevm("", timeout, out peakRmsDevmMaximum, out peakDevmMaximum, out ninetyninePercentDevm);
         BT.ModAcc.Results.FetchFrequencyErrorEdr("", timeout, out headerFrequencyErrorWiMaximum, out peakFrequencyErrorWiPlusW0Maximum,
            out peakFrequencyErrorW0Maximum);
         BT.Acp.Results.FetchReferenceChannelPower("", timeout, out referenceChannelPower);
         BT.Acp.Results.FetchOffsetMeasurementArray("", timeout, ref lowerAbsolutePower, ref upperAbsolutePower, ref lowerRelativePower, ref upperRelativePower, ref lowerMargin, ref upperMargin);
         BT.Txp.Results.FetchPowers("", timeout, out averagePowerMean, out averagePowerMaximum, out averagePowerMinimum, out peakToAveragePowerRatioMaximum);
         BT.Txp.Results.FetchEdrPowers("", timeout, out edrGfskAveragePowerMean, out edrDpskAveragePowerMean, out edrDpskGfskAveragePowerRatioMean);
      }

      void PrintResults()
      {
         Console.WriteLine("------------------ModAcc------------------");
         Console.WriteLine("------------------DEVM------------------");
         Console.WriteLine("Peak Rms Devm Maximum (%)                     : {0}", peakRmsDevmMaximum);
         Console.WriteLine("Peak Devm Maximum (%)                         : {0}", peakDevmMaximum);
         Console.WriteLine("99% Devm (%)                                  : {0}", ninetyninePercentDevm);
         Console.WriteLine("------------------EDR Frequency Error------------------");
         Console.WriteLine("Header Frequency Error wi Maximum (Hz)        : {0}", headerFrequencyErrorWiMaximum);
         Console.WriteLine("Peak Frequency Error wi+w0 Maximum (Hz)       : {0}", peakFrequencyErrorWiPlusW0Maximum);
         Console.WriteLine("Peak Frequency Error w0 Maximum (Hz)          : {0}\n", peakFrequencyErrorW0Maximum);

         Console.WriteLine("------------------ACP------------------");
         Console.WriteLine("Reference Channel Power (dBm)                 : {0} \n", referenceChannelPower);

         Console.WriteLine("------------------Offset Measuremensts------------------");
         for (int i = 0; i < lowerAbsolutePower.Length; i++)
         {
            Console.WriteLine("Offset " + i);
            Console.WriteLine("Lower Absolute Powers (dBm)                   : {0} ", lowerAbsolutePower[i]);
            Console.WriteLine("Upper Absolute Powers (dBm)                   : {0} ", upperAbsolutePower[i]);
            Console.WriteLine("Lower Relative Powers (dB)                    : {0} ", lowerRelativePower[i]);
            Console.WriteLine("Upper Relative Powers (dB)                    : {0} ", upperRelativePower[i]);
            Console.WriteLine("Lower Margin (dB)                             : {0} ", lowerMargin[i]);
            Console.WriteLine("Upper Margin (dB)                             : {0} \n", upperMargin[i]);
         }

         Console.WriteLine("------------------TXP------------------");
         Console.WriteLine("Average Power Mean (dBm)                      : {0}", averagePowerMean);
         Console.WriteLine("Average Power Maximum (dBm)                   : {0}", averagePowerMaximum);
         Console.WriteLine("Average Power Minimum (dBm)                   : {0}", averagePowerMinimum);
         Console.WriteLine("Peak to Average Power Ratio Maximum (dB)      : {0}", peakToAveragePowerRatioMaximum);
         Console.WriteLine("EDR GFSK Average Power Mean (dBm)             : {0}", edrGfskAveragePowerMean);
         Console.WriteLine("EDR DPSK Average Power Mean (dBm)             : {0}", edrDpskAveragePowerMean);
         Console.WriteLine("EDR DPSK GFSK Average Power Ratio Mean (dB)   : {0}", edrDpskGfskAveragePowerRatioMean);
      }

      void CloseSession()
      {
         if (BT != null)
         {
            BT.Dispose();
            BT = null;
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