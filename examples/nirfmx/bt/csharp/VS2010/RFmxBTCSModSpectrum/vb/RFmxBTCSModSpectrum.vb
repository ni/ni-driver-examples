' Steps:
' 1. Open a new RFmx Session.
' 2. Configure Frequency Reference.
' 3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
' 4. Configure Trigger Type and Trigger Parameters.
' 5. Configure Packet Type.
' 6. Configure Data Rate.
' 7. Configure CS Packet Format, CS SYNC Sequence, Payload Length Mode and bytes.
' 8. Select ModSpectrum measurement
' 9. Configure Burst Synchronization Type 
' 10. Configure Averaging Parameters for ModSpectrum measurement.
' 11. Initiate the Measurement.
' 12. Fetch ModSpectrum Measurements and Traces.
' 13. Close RFmx Session.

Imports System
Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.BTMX

Namespace NationalInstruments.Examples.RFmxBTCSModSpectrum
 Public Class RFmxBTCSModSpectrum
  Private instrSession As RFmxInstrMX
  Private BT As RFmxBTMX
  Private resourceName As String

  Private frequencyReferenceSource As String
  Private frequencyReferenceFrequency As Double

  Private centerFrequency As Double
  Private referenceLevel As Double
  Private externalAttenuation As Double

  Private enableTrigger As Boolean
  Private iqPowerEdgeTriggerLevel As Double
  Private minimumQuiteTimeMode As RFmxBTMXTriggerMinimumQuietTimeMode
  Private minimumQuietTime As Double
  Private triggerDelay As Double

  Private packetType As RFmxBTMXPacketType
  Private leDataRate As Integer

  Private payloadLengthMode As RFmxBTMXPayloadLengthMode
  Private payloadLength As Integer

  Private averagingEnabled As RFmxBTMXModSpectrumAveragingEnabled
  Private averagingCount As Integer

  Private resultsBandwidth As Double                                                ' Hz 
  Private resultsHighFrequency As Double                                            ' Hz 
  Private resultsLowFrequency As Double                                             ' Hz 

  Private resultSpectrum As Spectrum(Of Single)
  Private timeout As Double

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
    ' Close session 
    CloseSession()
    Console.WriteLine("Press any key to exit")
    Console.ReadKey()
   End Try
  End Sub

  Private Sub InitializeVariables()
   resourceName = "RFSA"

   frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
   frequencyReferenceFrequency = 10000000.0                              ' (Hz) 

   centerFrequency = 2402000000.0                                        ' (Hz) 
   referenceLevel = 0.00                                                 ' (dBm) 
   externalAttenuation = 0.0                                             ' (dB) 

   enableTrigger = True
   iqPowerEdgeTriggerLevel = -20.0                                       ' (dB) 
   minimumQuiteTimeMode = RFmxBTMXTriggerMinimumQuietTimeMode.Auto
   minimumQuietTime = 0.0001                                             ' (seconds) 
   triggerDelay = 0.0                                                    ' (seconds) 

   packetType = RFmxBTMXPacketType.PacketTypeLECS
   leDataRate = 1000000                                                  ' (bps) 
   payloadLengthMode = RFmxBTMXPayloadLengthMode.Auto
   payloadLength = 10                                                    ' bytes

   averagingEnabled = RFmxBTMXModSpectrumAveragingEnabled.False
   averagingCount = 10
   timeout = 10                                                          ' (seconds) 
  End Sub

  Private Sub InitializeInstr()
   instrSession = New RFmxInstrMX(resourceName, "")
  End Sub

  Private Sub ConfigureBT()
   BT = instrSession.GetBTSignalConfiguration()       ' Create a new RFmx Session 
   instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
   BT.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
   BT.ConfigureIQPowerEdgeTrigger("", "0", RFmxBTMXIQPowerEdgeTriggerSlope.Rising, iqPowerEdgeTriggerLevel, triggerDelay, minimumQuiteTimeMode, minimumQuietTime, RFmxBTMXIQPowerEdgeTriggerLevelType.Relative, enableTrigger)
   BT.ConfigurePacketType("", packetType)
   BT.ConfigureDataRate("", leDataRate)
   BT.SetChannelSoundingPacketFormat("", RFmxBTMXChannelSoundingPacketFormat.Sync)
   BT.SetChannelSoundingSyncSequence("", RFmxBTMXChannelSoundingSyncSequence.PayloadPattern)
   BT.SetPayloadLengthMode("", payloadLengthMode)
   BT.SetPayloadLength("", payloadLength)
   BT.SelectMeasurements("", RFmxBTMXMeasurementTypes.ModSpectrum, True)
   BT.ModSpectrum.Configuration.ConfigureBurstSynchronizationType("", RFmxBTMXModSpectrumBurstSynchronizationType.Preamble)
   BT.ModSpectrum.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount)
   BT.Initiate("", "")
  End Sub

  Private Sub RetrieveResults()
   BT.ModSpectrum.Results.GetBandwidth("", resultsBandwidth)
   BT.ModSpectrum.Results.GetHighFrequency("", resultsHighFrequency)
   BT.ModSpectrum.Results.GetLowFrequency("", resultsLowFrequency)
   BT.ModSpectrum.Results.FetchSpectrum("", timeout, resultSpectrum)
  End Sub

  Private Sub PrintResults()
   Console.WriteLine("------------------------ModSpectrum--------------------------------")
   Console.WriteLine("ModSpectrum Results Bandwidth (Hz)    : {0} " & vbLf, resultsBandwidth)
   Console.WriteLine("ModSpectrum Results High Freq (Hz)    : {0} " & vbLf, resultsHighFrequency)
   Console.WriteLine("ModSpectrum Results Low Freq (Hz)     : {0} " & vbLf, resultsLowFrequency)
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
   Console.WriteLine("ERROR:" & vbLf & ex.GetType().ToString() & ": " & ex.Message)
  End Sub
 End Class
End Namespace
