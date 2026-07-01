'*Steps
'1. Open a New RFmx Session.
'2. Configure Frequency Reference.
'3. Configure basic signal properties (Center Frequency, Reference Level And External Attenuation).
'4. Configure Trigger Type And Trigger Parameters.
'5. Configure Packet Type.
'6. Configure Data Rate.
'7. Configure High Data Throughput Packet Format if the packet type is High Data Throughput.
'8. Configure Payload Length if the Payload Length Mode is not set to Auto.
'9. Configure CS Packet Format, CS SYNC Sequence, CS Phase Measurement Period And CS Tone Extension Slot.
'10. Select PowerRamp measurement
'11. Configure Burst Synchronization Type 
'12. Configure Averaging Parameters for PowerRamp measurement.
'13. Initiate the Measurement.
'14.  Fetch PowerRamp Measurements.
'15. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.BTMX

Namespace NationalInstruments.Examples.RFmxBTPowerRamp
 Public Class RFmxBTPowerRamp
  Private instrSession As RFmxInstrMX
  Private BT As RFmxBTMX
  Private resourceName As String

  Private frequencyReferenceSource As String
  Private frequencyReferenceFrequency As Double

  Private centerFrequency As Double
  Private referenceLevel As Double
  Private externalAttenuation As Double

  Private enableTrigger As Boolean
  Private iqPowerEdgeTriggerSlope As RFmxBTMXIQPowerEdgeTriggerSlope
  Private iqPowerEdgeTriggerLevel As Double
  Private minimumQuiteTimeMode As RFmxBTMXTriggerMinimumQuietTimeMode
  Private minimumQuietTime As Double
  Private iqPowerEdgeTriggerLevelType As RFmxBTMXIQPowerEdgeTriggerLevelType
  Private triggerDelay As Double

  Private packetType As RFmxBTMXPacketType
  Private leDataRate As Integer

  Private highDataThroughputPacketFormat As RFmxBTMXHighDataThroughputPacketFormat

  Private payloadLengthMode As RFmxBTMXPayloadLengthMode
  Private payloadLength As Integer

  Private channelSoundingPacketFormat As RFmxBTMXChannelSoundingPacketFormat
  Private channelSoundingSyncSequence As RFmxBTMXChannelSoundingSyncSequence
  Private channelSoundingPhaseMeasurmentPeriod As Double
  Private channelSoundingToneExtensionSlot As RFmxBTMXChannelSoundingToneExtensionSlot

  Private averagingEnabled As RFmxBTMXPowerRampAveragingEnabled
  Private averagingCount As Integer

  Private riseTimeMean As Double                                           '(seconds)
  Private fallTimeMean As Double                                           '(seconds) 
  Private fortydBFallTimeMean As Double                                    '(seconds) 
  Private fortydBRiseTimeMean As Double                                    '(seconds) 


  Public Sub Run()
   Try
    InitializeVariables()
    InitializeInstr()
    ConfigureBT()
    RetrieveResults()
    PrintResults()
   Catch ex As Exception
    DisplayError(ex)
   Finally
    'Close session
    CloseSession()
    Console.WriteLine("Press any key to exit")
    Console.ReadKey()
   End Try
  End Sub

  Private Sub InitializeVariables()
   resourceName = "RFSA"

   frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
   frequencyReferenceFrequency = 10000000.0                                    '(Hz)

   centerFrequency = 2402000000.0                                              '(Hz)
   referenceLevel = 0.0                                                        '(dBm)
   externalAttenuation = 0.0                                                   '(dB)

   enableTrigger = True
   iqPowerEdgeTriggerSlope = RFmxBTMXIQPowerEdgeTriggerSlope.Rising
   iqPowerEdgeTriggerLevel = -20.0                                             '(dB)
   minimumQuiteTimeMode = RFmxBTMXTriggerMinimumQuietTimeMode.Auto
   minimumQuietTime = 0.0001                                                   '(seconds )
   iqPowerEdgeTriggerLevelType = RFmxBTMXIQPowerEdgeTriggerLevelType.Relative
   triggerDelay = 0.0                                                          '(seconds )

   packetType = RFmxBTMXPacketType.PacketTypeLECS
   leDataRate = 1000000                                                          '(bps)

   highDataThroughputPacketFormat = RFmxBTMXHighDataThroughputPacketFormat.Format0

   payloadLengthMode = RFmxBTMXPayloadLengthMode.Auto
   payloadLength = 10                                                           '(bytes)

   channelSoundingPacketFormat = RFmxBTMXChannelSoundingPacketFormat.Sync
   channelSoundingSyncSequence = RFmxBTMXChannelSoundingSyncSequence.SoundingSequence32bit
   channelSoundingPhaseMeasurmentPeriod = 0.00001
   channelSoundingToneExtensionSlot = RFmxBTMXChannelSoundingToneExtensionSlot.Disabled

   averagingEnabled = RFmxBTMXPowerRampAveragingEnabled.False
   averagingCount = 10

  End Sub

  Private Sub InitializeInstr()
   instrSession = New RFmxInstrMX(resourceName, "")
  End Sub

  Private Sub ConfigureBT()
   BT = instrSession.GetBTSignalConfiguration()       ' Create a new RFmx Session
   instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
   BT.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
   BT.ConfigureIQPowerEdgeTrigger("", "0", iqPowerEdgeTriggerSlope, iqPowerEdgeTriggerLevel,
            triggerDelay, minimumQuiteTimeMode, minimumQuietTime, iqPowerEdgeTriggerLevelType,
            enableTrigger)
   BT.ConfigurePacketType("", packetType)
   BT.ConfigureDataRate("", leDataRate)
   BT.SetHighDataThroughputPacketFormat("", highDataThroughputPacketFormat)
   BT.ConfigurePayloadLength("", payloadLengthMode, payloadLength)
   BT.SetChannelSoundingPacketFormat("", channelSoundingPacketFormat)
   BT.SetChannelSoundingSyncSequence("", channelSoundingSyncSequence)
   BT.SetChannelSoundingPhaseMeasurementPeriod("", channelSoundingPhaseMeasurmentPeriod)
   BT.SetChannelSoundingToneExtensionSlot("", channelSoundingToneExtensionSlot)
   BT.SelectMeasurements("", RFmxBTMXMeasurementTypes.PowerRamp, True)
   BT.PowerRamp.Configuration.SetMeasurementEnabled("", True)
   BT.PowerRamp.Configuration.ConfigureBurstSynchronizationType("", RFmxBTMXAcpBurstSynchronizationType.Preamble)
   BT.PowerRamp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount)
   BT.Initiate("", "")
  End Sub

  Private Sub RetrieveResults()
   BT.PowerRamp.Results.GetRiseTimeMean("", riseTimeMean)
   BT.PowerRamp.Results.GetFallTimeMean("", fallTimeMean)
   BT.PowerRamp.Results.Get40dBFallTimeMean("", fortydBFallTimeMean)
   BT.PowerRamp.Results.Get40dBRiseTimeMean("", fortydBRiseTimeMean)
  End Sub

  Private Sub PrintResults()
   Console.WriteLine("-----------------------PowerRamp---------------------------")
   Console.WriteLine("Rise Time Mean (s)                 : {0}", riseTimeMean)
   Console.WriteLine("Fall Time Mean (s)                 : {0}", fallTimeMean)
   Console.WriteLine("40dB Fall Time Mean (s)            : {0}", fortydBFallTimeMean)
   Console.WriteLine("40dB Rise Time Mean (s)            : {0}", fortydBRiseTimeMean)
   Console.WriteLine()
  End Sub

  Private Sub CloseSession()
   If BT IsNot Nothing Then
    BT.Dispose()
    BT = Nothing
   End If
   If instrSession IsNot Nothing Then
    instrSession.Close()
    instrSession = Nothing
   End If
  End Sub

  Private Shared Sub DisplayError(ex As Exception)
   Console.WriteLine("ERROR:" & vbLf & Convert.ToString(ex.[GetType]()) & ": " + ex.Message)
  End Sub
 End Class
End Namespace
