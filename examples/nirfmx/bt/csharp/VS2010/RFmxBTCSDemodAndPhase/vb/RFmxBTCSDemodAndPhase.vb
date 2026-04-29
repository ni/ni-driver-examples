'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure basic signal properties(Center Frequency, Reference Level and External Attenuation).
'4. Configure Trigger Type and Trigger Parameters.
'5. Configure Packet Type.
'6. Configure Data Rate.
'7. Configure Channel Sounding properties (CS Packet Format, CS Sync Sequence, CS Phase Measurement Period, CS Tone Extension Slot).
'8. Select ModAcc measurement and enable Traces.
'9. Configure ModAcc Burst Synchronization Type.
'10. Configure Averaging Parameters for ModAcc measurement.
'11. Initiate the Measurement.
'12. Fetch ModAcc Measurements and Trace.
'13. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.BTMX

Namespace NationalInstruments.Examples.RFmxBTCSDemodAndPhase

    Public Class RFmxBTCSDemodAndPhase
        Private instrSession As RFmxInstrMX
        Private BT As RFmxBTMX
        Private rfsaResourceName As String

        Private frequencyReferenceSource As String
        Private frequencyReferenceFrequency As Double

        Private centerFrequency As Double
        Private referenceLevel As Double
        Private externalAttenuation As Double

        Private enableTrigger As Boolean
        Private iqPowerEdgeLevel As Double
        Private minimumQuietTimeMode As RFmxBTMXTriggerMinimumQuietTimeMode
        Private minimumQuietTime As Double
        Private triggerDelay As Double

        Private packetType As RFmxBTMXPacketType
        Private LEDataRate As Integer

        Private channelSoundingPacketFormat As RFmxBTMXChannelSoundingPacketFormat
        Private channelSoundingSyncSequence As RFmxBTMXChannelSoundingSyncSequence
        Private channelSoundingPhaseMeasurementPeriod As Double
        Private channelSoundingToneExtensionSlot As RFmxBTMXChannelSoundingToneExtensionSlot

        Private averagingEnabled As RFmxBTMXModAccAveragingEnabled
        Private averagingCount As Integer

        Private timeout As Double

        Private peakFrequencyErrorMaximum As Double                                '(Hz)
        Private initialFrequencyDriftMaximum As Double                             '(Hz)
        Private peakFrequencyDriftMaximum As Double                                '(Hz)
        Private peakFrequencyDriftRateMaximum As Double                            '(Hz)
        Private clockDriftMean As Double                                           '(ppm)
        Private preambleStartTimeMean As Double                                    '(seconds)

        Private CSDetrendedTrace As AnalogWaveform(Of Single)
        Private CSToneAmplitude As AnalogWaveform(Of Single)
        Private CSTonePhase As AnalogWaveform(Of Single)

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
            rfsaResourceName = "RFSA"

            frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
            frequencyReferenceFrequency = 10000000.0                               '(Hz)

            centerFrequency = 2402000000.0                                         '(Hz)
            referenceLevel = 0.0                                                   '(dBm)
            externalAttenuation = 0.0                                              '(dB)

            enableTrigger = True
            iqPowerEdgeLevel = -20.0                                               'dB
            minimumQuietTimeMode = RFmxBTMXTriggerMinimumQuietTimeMode.Auto
            minimumQuietTime = 0.0001                                              'seconds
            triggerDelay = 0.0                                                     'seconds

            packetType = RFmxBTMXPacketType.PacketTypeLECS
            LEDataRate = 1000000                                                   'bps

            channelSoundingPacketFormat = RFmxBTMXChannelSoundingPacketFormat.Sync
            channelSoundingSyncSequence = RFmxBTMXChannelSoundingSyncSequence.None
            channelSoundingPhaseMeasurementPeriod = 0.00001
            channelSoundingToneExtensionSlot = RFmxBTMXChannelSoundingToneExtensionSlot.Disabled

            averagingEnabled = RFmxBTMXModAccAveragingEnabled.False
            averagingCount = 10

            timeout = 10.0                                                         'seconds
        End Sub

        Private Sub InitializeInstr()
            instrSession = New RFmxInstrMX(rfsaResourceName, "")
        End Sub

        Private Sub ConfigureBT()
            BT = instrSession.GetBTSignalConfiguration()       ' Create a new RFmx Session 
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
            BT.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
            BT.ConfigureIQPowerEdgeTrigger("", "0", RFmxBTMXIQPowerEdgeTriggerSlope.Rising, iqPowerEdgeLevel,
            triggerDelay, minimumQuietTimeMode, minimumQuietTime, RFmxBTMXIQPowerEdgeTriggerLevelType.Relative,
            enableTrigger)
            BT.ConfigurePacketType("", packetType)
            BT.ConfigureDataRate("", LEDataRate)
            BT.SetChannelSoundingPacketFormat("", channelSoundingPacketFormat)
            BT.SetChannelSoundingSyncSequence("", channelSoundingSyncSequence)
            BT.SetChannelSoundingPhaseMeasurementPeriod("", channelSoundingPhaseMeasurementPeriod)
            BT.SetChannelSoundingToneExtensionSlot("", channelSoundingToneExtensionSlot)
            BT.SelectMeasurements("", RFmxBTMXMeasurementTypes.ModAcc, True)
            BT.ModAcc.Configuration.ConfigureBurstSynchronizationType("", RFmxBTMXModAccBurstSynchronizationType.Preamble)
            BT.ModAcc.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount)
            BT.Initiate("", "")
        End Sub

        Private Sub RetrieveResults()
            BT.ModAcc.Results.GetClockDriftMean("", clockDriftMean)
            BT.ModAcc.Results.GetPreambleStartTimeMean("", preambleStartTimeMean)
            BT.ModAcc.Results.FetchFrequencyErrorLE("", timeout, peakFrequencyErrorMaximum,
                initialFrequencyDriftMaximum, peakFrequencyDriftMaximum, peakFrequencyDriftRateMaximum)
            BT.ModAcc.Results.FetchCSToneTrace("", timeout, CSToneAmplitude, CSTonePhase)
            BT.ModAcc.Results.FetchCSDetrendedPhaseTrace("", timeout, CSDetrendedTrace)
        End Sub

        Private Sub PrintResults()
            Console.WriteLine("------------------ Channel Sounding Measurements ------------------")
            Console.WriteLine("Peak Frequency Error Maximum (Hz)               : {0}", peakFrequencyErrorMaximum)
            Console.WriteLine("Clock Drift Mean (ppm)                          : {0}", clockDriftMean)
            Console.WriteLine("Preamble Start Time Mean (seconds)              : {0}", preambleStartTimeMean)

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
