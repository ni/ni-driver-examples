/*Steps:
1. Open a new RFmx Session.
2. Configure Frequency Reference.
3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
4. Configure Trigger Type and Trigger Parameters.
5. Configure Packet Type.
6. Configure Data Rate.
7. Configure CS Packet Format, CS SYNC Sequence, CS Phase Measurement Period and CS Tone Extension Slot.
8. Select PowerRamp measurement
9. Configure Burst Synchronization Type 
10. Configure Averaging Parameters for PowerRamp measurement.
11. Initiate the Measurement.
12.  Fetch PowerRamp Measurements.
13. Close RFmx Session. .*/

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.BTMX;

namespace NationalInstruments.Examples.RFmxBTCSPowerRamp
{
 public class RFmxBTCSPowerRamp
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
  RFmxBTMXIQPowerEdgeTriggerSlope iqPowerEdgeTriggerSlope;
  double iqPowerEdgeTriggerLevel;
  RFmxBTMXTriggerMinimumQuietTimeMode minimumQuiteTimeMode;
  double minimumQuietTime;
  RFmxBTMXIQPowerEdgeTriggerLevelType iqPowerEdgeTriggerLevelType;
  double triggerDelay;

  RFmxBTMXPacketType packetType;
  int leDataRate;

  RFmxBTMXChannelSoundingPacketFormat channelSoundingPacketFormat;
  RFmxBTMXChannelSoundingSyncSequence channelSoundingSyncSequence;
  double channelSoundingPhaseMeasurmentPeriod;
  RFmxBTMXChannelSoundingToneExtensionSlot channelSoundingToneExtensionSlot;

  RFmxBTMXPowerRampAveragingEnabled averagingEnabled;
  int averagingCount;

  double riseTimeMean;                                         /*(seconds) */
  double fallTimeMean;                                         /*(seconds) */
  double fortydBFallTimeMean;                                  /*(seconds) */


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

   packetType = RFmxBTMXPacketType.PacketTypeLECS;
   leDataRate = 1000000;                                                   /*(bps) */

   channelSoundingPacketFormat = RFmxBTMXChannelSoundingPacketFormat.Sync;
   channelSoundingSyncSequence = RFmxBTMXChannelSoundingSyncSequence.None;
   channelSoundingPhaseMeasurmentPeriod = 0.00001;
   channelSoundingToneExtensionSlot = RFmxBTMXChannelSoundingToneExtensionSlot.Disabled;

   averagingEnabled = RFmxBTMXPowerRampAveragingEnabled.False;
   averagingCount = 10;

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
   BT.ConfigureIQPowerEdgeTrigger("", "0", iqPowerEdgeTriggerSlope, iqPowerEdgeTriggerLevel,
       triggerDelay, minimumQuiteTimeMode, minimumQuietTime, iqPowerEdgeTriggerLevelType,
       enableTrigger);
   BT.ConfigurePacketType("", packetType);
   BT.ConfigureDataRate("", leDataRate);
   BT.SetChannelSoundingPacketFormat("", channelSoundingPacketFormat);
   BT.SetChannelSoundingSyncSequence("", channelSoundingSyncSequence);
   BT.SetChannelSoundingPhaseMeasurementPeriod("", channelSoundingPhaseMeasurmentPeriod);
   BT.SetChannelSoundingToneExtensionSlot("", channelSoundingToneExtensionSlot);
   BT.SelectMeasurements("", RFmxBTMXMeasurementTypes.PowerRamp, true);
   BT.PowerRamp.Configuration.ConfigureBurstSynchronizationType("", RFmxBTMXPowerRampBurstSynchronizationType.Preamble);
   BT.PowerRamp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount);
   BT.Initiate("", "");
  }

  void RetrieveResults()
  {
   BT.PowerRamp.Results.GetRiseTimeMean("", out riseTimeMean);
   BT.PowerRamp.Results.GetFallTimeMean("", out fallTimeMean);
   BT.PowerRamp.Results.Get40dBFallTimeMean("", out fortydBFallTimeMean);
  }

  void PrintResults()
  {
   Console.WriteLine("------------------PowerRamp------------------");
   Console.WriteLine("Rise Time Mean (s)      : {0} \n", riseTimeMean);
   Console.WriteLine("Fall Time Mean (s)      : {0} \n", fallTimeMean);
   Console.WriteLine("40dB Fall Time Mean (s) : {0} \n", fortydBFallTimeMean);


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