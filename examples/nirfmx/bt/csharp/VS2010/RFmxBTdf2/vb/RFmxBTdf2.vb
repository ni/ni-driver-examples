'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure basic signal properties(Center Frequency, Reference Level and External Attenuation).
'4. Configure Trigger Type and Trigger Parameters.
'5. Configure Packet Type.
'6. Configure Data Rate.
'7. Configure Payload Bit pattern
'8. Configure Payload Length.
'9. Configure Direction Finding.
'10. Select ModAcc measurement and enable Traces.
'11. Configure ModAcc Burst Synchronization Type.
'12. Configure Averaging Parameters for ModAcc measurement.
'13. Initiate the Measurement.
'14. Fetch ModAcc Measurements and Trace.
'15. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.BTMX

Namespace NationalInstruments.Examples.RFmxBTdf2

    Public Class RFmxBTdf2
        Private instrSession As RFmxInstrMX
        Private BT As RFmxBTMX
        Private rfsaResourceName As String

        Private frequencyReferenceSource As String
        Private frequencyReferenceFrequency As Double

        Private centerFrequency As Double
        Private referenceLevel As Double
        Private externalAttenuation As Double

        Private enableTrigger As Boolean
        Private iqPowerEdgeTriggerSlope As RFmxBTMXIQPowerEdgeTriggerSlope
        Private iqPowerEdgeLevel As Double
        Private minimumQuiteTimeMode As RFmxBTMXTriggerMinimumQuietTimeMode
        Private minimumQuietTime As Double
        Private iqPowerEdgeTriggerLevelType As RFmxBTMXIQPowerEdgeTriggerLevelType
        Private triggerDelay As Double

        Private packetType As RFmxBTMXPacketType
        Private LEDataRate As Integer

        Private payloadBitPattern As RFmxBTMXPayloadBitPattern
        Private payloadLengthMode As RFmxBTMXPayloadLengthMode
        Private payloadLength As Integer

        Private directionFindingMode As RFmxBTMXDirectionFindingMode
        Private cteLength As Double
	Private cteSlotDuration As Double

        Private burstSynchronizationType As RFmxBTMXModAccBurstSynchronizationType

        Private measurement As RFmxBTMXMeasurementTypes
        Private enableAllTraces As Boolean

        Private averagingEnabled As RFmxBTMXModAccAveragingEnabled
        Private averagingCount As Integer

        Private timeout As Double
        Private df2avgMinimum As Double
        Private percentageOfSymbolsAbouveDf2maxThreshold As Double

        Private initialFrequencyErrorMaximum As Double
        Private BRPeakFrequencyDriftMaximum As Double
        Private BRPeakFrequencyDriftRateMaximum As Double

        Private peakFrequencyErrorMaximum As Double
        Private initialFrequencyDriftMaximum As Double
        Private LEPeakFrequencyDriftMaximum As Double
        Private LEPeakFrequencyDriftRateMaximum As Double

        Private df2max As Single()
        Private time As Single()
        Private timeBR As Single()
        Private frequencyErrorBR As Single()
        Private timeLE As Single()
        Private frequencyErrorLE As Single()

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
            frequencyReferenceFrequency = 10000000.0                               ' (Hz)

            centerFrequency = 2402000000.0                                         ' (Hz)
            referenceLevel = 0.0                                                   ' (dBm)
            externalAttenuation = 0.0                                              ' (dB)

            enableTrigger = True
            iqPowerEdgeTriggerSlope = RFmxBTMXIQPowerEdgeTriggerSlope.Rising
            iqPowerEdgeLevel = -20.0                                               'dB
            minimumQuiteTimeMode = RFmxBTMXTriggerMinimumQuietTimeMode.Auto
            minimumQuietTime = 0.0001                                              'seconds
            iqPowerEdgeTriggerLevelType = RFmxBTMXIQPowerEdgeTriggerLevelType.Relative
            triggerDelay = 0.0                                                     'seconds

            packetType = RFmxBTMXPacketType.PacketTypeDH1
            LEDataRate = 1000000                                                   'bps

            payloadBitPattern = RFmxBTMXPayloadBitPattern.Pattern10101010
            payloadLengthMode = RFmxBTMXPayloadLengthMode.Auto
            payloadLength = 10                                                      'bytes

            directionFindingMode = RFmxBTMXDirectionFindingMode.Disabled
            cteLength = 0.00016                                                    'seconds
	    cteSlotDuration = 0.000001                                             'seconds

            burstSynchronizationType = RFmxBTMXModAccBurstSynchronizationType.Preamble

            measurement = RFmxBTMXMeasurementTypes.ModAcc
            enableAllTraces = True

            averagingEnabled = RFmxBTMXModAccAveragingEnabled.False
            averagingCount = 10

            timeout = 10.0                                                         'seconds
            df2avgMinimum = 0                                                      '(Hz)
            percentageOfSymbolsAbouveDf2maxThreshold = 0                           '(Hz)

            initialFrequencyErrorMaximum = 0.0                                     '(Hz)
            BRPeakFrequencyDriftMaximum = 0.0                                      '(Hz)
            BRPeakFrequencyDriftRateMaximum = 0.0                                  '(Hz)

            peakFrequencyErrorMaximum = 0.0                                        '(Hz)
            initialFrequencyDriftMaximum = 0.0                                     '(Hz)
            LEPeakFrequencyDriftMaximum = 0.0                                      '(Hz)
            LEPeakFrequencyDriftRateMaximum = 0.0                                  '(Hz)
        End Sub

        Private Sub InitializeInstr()
            instrSession = New RFmxInstrMX(rfsaResourceName, "")
        End Sub

        Private Sub ConfigureBT()
            BT = instrSession.GetBTSignalConfiguration()       ' Create a new RFmx Session
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
            BT.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
            BT.ConfigureIQPowerEdgeTrigger("", "0", iqPowerEdgeTriggerSlope, iqPowerEdgeLevel,
            triggerDelay, minimumQuiteTimeMode, minimumQuietTime, iqPowerEdgeTriggerLevelType,
            enableTrigger)
            BT.ConfigurePacketType("", packetType)
            BT.ConfigureDataRate("", LEDataRate)
            BT.ConfigurePayloadBitPattern("", payloadBitPattern)
            BT.ConfigurePayloadLength("", payloadLengthMode, payloadLength)
            BT.ConfigureLEDirectionFinding("", directionFindingMode, cteLength, cteSlotDuration)
            BT.SelectMeasurements("", measurement, enableAllTraces)
            BT.ModAcc.Configuration.ConfigureBurstSynchronizationType("", burstSynchronizationType)
            BT.ModAcc.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount)
            BT.Initiate("", "")
        End Sub

        Private Sub RetrieveResults()
            BT.ModAcc.Results.FetchDf2("", timeout, df2avgMinimum, percentageOfSymbolsAbouveDf2maxThreshold)
            BT.ModAcc.Results.FetchFrequencyErrorBR("", timeout, initialFrequencyErrorMaximum, BRPeakFrequencyDriftMaximum,
            BRPeakFrequencyDriftRateMaximum)
            BT.ModAcc.Results.FetchFrequencyErrorLE("", timeout, peakFrequencyErrorMaximum, initialFrequencyDriftMaximum,
            LEPeakFrequencyDriftMaximum, LEPeakFrequencyDriftRateMaximum)
            BT.ModAcc.Results.FetchDf2maxTrace("", timeout, time, df2max)
            BT.ModAcc.Results.FetchFrequencyErrorTraceBR("", timeout, timeBR, frequencyErrorBR)
            BT.ModAcc.Results.FetchFrequencyErrorTraceLE("", timeout, timeLE, frequencyErrorLE)
        End Sub

        Private Sub PrintResults()
            Console.WriteLine("------------------df2 Measurement------------------")
            Console.WriteLine("df2avg Minimum (Hz)                             : {0}", df2avgMinimum)
            Console.WriteLine("Percentage of Symbols above df2max Threshold (%) : {0}", percentageOfSymbolsAbouveDf2maxThreshold)
            Console.WriteLine("------------------BR Frequency Error------------------")
            Console.WriteLine("Initial Frequency Error Maximum (Hz)            : {0}", initialFrequencyErrorMaximum)
            Console.WriteLine("Peak Frequency Drift Maximum (Hz)               : {0}", BRPeakFrequencyDriftMaximum)
            Console.WriteLine("Peak Frequency Drift Rate Maximum (Hz)          : {0}", BRPeakFrequencyDriftRateMaximum)
            Console.WriteLine("------------------LE Frequency Error------------------")
            Console.WriteLine("Peak Frequency Error Maximum (Hz)               : {0}", peakFrequencyErrorMaximum)
            Console.WriteLine("Initial Frequency Drift Maximum (Hz)            : {0}", initialFrequencyDriftMaximum)
            Console.WriteLine("Peak Frequency Drift Maximum (Hz)               : {0}", LEPeakFrequencyDriftMaximum)
            Console.WriteLine("Peak Frequency Drift Rate Maximum (Hz)          : {0}", LEPeakFrequencyDriftRateMaximum)
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
