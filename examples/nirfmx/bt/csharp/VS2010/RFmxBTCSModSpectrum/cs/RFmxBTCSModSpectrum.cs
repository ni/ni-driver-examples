/*Steps:
1. Open a new RFmx Session.
2. Configure Frequency Reference.
3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
4. Configure Trigger Type and Trigger Parameters.
5. Configure Packet Type.
6. Configure Data Rate.
7. Configure CS Packet Format, CS SYNC Sequence, Payload Length Mode and bytes.
8. Select ModSpectrum measurement
9. Configure Burst Synchronization Type 
10. Configure Averaging Parameters for ModSpectrum measurement.
11. Initiate the Measurement.
12. Fetch ModSpectrum Measurements and Traces.
13. Close RFmx Session.*/

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.BTMX;

namespace NationalInstruments.Examples.RFmxBTCSModSpectrum

{

 public class RFmxBTCSModSpectrum

 {
  RFmxInstrMX instrSession;
  RFmxBTMX BT;
  string resourceName;

  string frequencyReferenceSource;
  double frequencyReferenceFrequency;

  double centerFrequency;
  double referenceLevel;
  double externalAttenuation;

  bool enableTrigger;
  double iqPowerEdgeTriggerLevel;
  RFmxBTMXTriggerMinimumQuietTimeMode minimumQuiteTimeMode;
  double minimumQuietTime;
  double triggerDelay;

  RFmxBTMXPacketType packetType;
  int leDataRate;

  RFmxBTMXPayloadLengthMode payloadLengthMode;
  int payloadLength;

  RFmxBTMXModSpectrumAveragingEnabled averagingEnabled;
  int averagingCount;

  double resultsBandwidth;                                                /* Hz */
  double resultsHighFrequency;                                            /* Hz */
  double resultsLowFrequency;                                             /* Hz */

  Spectrum<float> resultSpectrum;
  double timeout;

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
   resourceName = "RFSA";

   frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
   frequencyReferenceFrequency = 10e6;                                    /* (Hz) */

   centerFrequency = 2.402e9;                                             /* (Hz) */
   referenceLevel = 0.00;                                                 /* (dBm) */
   externalAttenuation = 0.0;                                             /* (dB) */

   enableTrigger = true;
   iqPowerEdgeTriggerLevel = -20.0;                                       /* (dB) */
   minimumQuiteTimeMode = RFmxBTMXTriggerMinimumQuietTimeMode.Auto;
   minimumQuietTime = 100e-6;                                             /*(seconds) */
   triggerDelay = 0.0;                                                    /*(seconds) */

   packetType = RFmxBTMXPacketType.PacketTypeLECS;
   leDataRate = 1000000;                                                  /*(bps) */
   payloadLengthMode = RFmxBTMXPayloadLengthMode.Auto;
   payloadLength = 10;                                                    /*bytes*/

   averagingEnabled = RFmxBTMXModSpectrumAveragingEnabled.False;
   averagingCount = 10;
   timeout = 10;                                                          /*(seconds) */
  }

  void InitializeInstr()
  {
   instrSession = new RFmxInstrMX(resourceName, "");
  }

  void ConfigureBT()
  {
   BT = instrSession.GetBTSignalConfiguration();       /* Create a new RFmx Session */
   instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
   BT.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
   BT.ConfigureIQPowerEdgeTrigger("", "0", RFmxBTMXIQPowerEdgeTriggerSlope.Rising, iqPowerEdgeTriggerLevel,
       triggerDelay, minimumQuiteTimeMode, minimumQuietTime, RFmxBTMXIQPowerEdgeTriggerLevelType.Relative,
       enableTrigger);
   BT.ConfigurePacketType("", packetType);
   BT.ConfigureDataRate("", leDataRate);
   BT.SetChannelSoundingPacketFormat("", RFmxBTMXChannelSoundingPacketFormat.Sync);
   BT.SetChannelSoundingSyncSequence("", RFmxBTMXChannelSoundingSyncSequence.PayloadPattern);
   BT.SetPayloadLengthMode("", payloadLengthMode);
   BT.SetPayloadLength("", payloadLength);
   BT.SelectMeasurements("", RFmxBTMXMeasurementTypes.ModSpectrum, true);
   BT.ModSpectrum.Configuration.ConfigureBurstSynchronizationType("", RFmxBTMXModSpectrumBurstSynchronizationType.Preamble);
   BT.ModSpectrum.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount);
   BT.Initiate("", "");
  }

  void RetrieveResults()
  {
   BT.ModSpectrum.Results.GetBandwidth("", out resultsBandwidth);
   BT.ModSpectrum.Results.GetHighFrequency("", out resultsHighFrequency);
   BT.ModSpectrum.Results.GetLowFrequency("", out resultsLowFrequency);
   BT.ModSpectrum.Results.FetchSpectrum("", timeout, ref resultSpectrum);
  }

  void PrintResults()
  {
   Console.WriteLine("------------------------ModSpectrum--------------------------------");
   Console.WriteLine("ModSpectrum Results Bandwidth (Hz)       : {0} \n", resultsBandwidth);
   Console.WriteLine("ModSpectrum Results High Freq (Hz)       : {0} \n", resultsHighFrequency);
   Console.WriteLine("ModSpectrum Results Low Freq (Hz)        : {0} \n", resultsLowFrequency);
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