'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure basic signal properties(Center Frequency, Reference Level and External Attenuation).
'4. Configure Trigger Type and Trigger Parameters.
'5. Configure Packet Type.
'6. Configure Data Rate.
'7. Configure Payload Length.
'8. Select ACP measurement and enable Traces.
'9. Configure ACP Burst Sync Type.
'10. Configure Averaging Parameters for ACP measurement.
'11. Configure Number of Offsets Or Channel Number depending on Offset Channel Mode.
'12. Initiate the Measurement.
'13. Fetch ACP Measurements and Trace.
'14. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.BTMX

Namespace NationalInstruments.Examples.RFmxBTAcp
    Public Class RFmxBTAcp
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
        Private dataRate As Integer

        Private payloadLengthMode As RFmxBTMXPayloadLengthMode
        Private payloadLength As Integer

        Private burstSynchronizationType As RFmxBTMXAcpBurstSynchronizationType

        Private measurement As RFmxBTMXMeasurementTypes
        Private enableAllTraces As Boolean

        Private averagingEnabled As RFmxBTMXAcpAveragingEnabled
        Private averagingCount As Integer

        Private numberOfOffsets As Integer
        Private offsetChannelMode As RFmxBTMXAcpOffsetChannelMode
        Private channelNumber As Integer

        Private measurementStatus As RFmxBTMXAcpResultsMeasurementStatus
        Private referenceChannelPower As Double

        Private timeout As Double
        Private lowerAbsolutePower As Double()
        Private upperAbsolutePower As Double()
        Private lowerRelativePower As Double()
        Private upperRelativePower As Double()
        Private lowerMargin As Double()
        Private upperMargin As Double()

        Private limitWithExceptionMask As Spectrum(Of Single)
        Private limitWithoutExceptionMask As Spectrum(Of Single)

        Private absolutePower As Spectrum(Of Single)

        Private spectrum As Spectrum(Of Single)

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

            packetType = RFmxBTMXPacketType.PacketTypeDH1
            dataRate = 1000000                                                          '(bps)

            payloadLengthMode = RFmxBTMXPayloadLengthMode.Auto
            payloadLength = 10                                                          '(bytes)

            burstSynchronizationType = RFmxBTMXAcpBurstSynchronizationType.Preamble

            measurement = RFmxBTMXMeasurementTypes.Acp
            enableAllTraces = True

            averagingEnabled = RFmxBTMXAcpAveragingEnabled.False
            averagingCount = 10

            numberOfOffsets = 5
            offsetChannelMode = RFmxBTMXAcpOffsetChannelMode.Symmetric
            channelNumber = 0

            timeout = 10.0                                                              '(seconds )

            measurementStatus = RFmxBTMXAcpResultsMeasurementStatus.Fail
            referenceChannelPower = 0.0                                                 '(dBm)
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
			   BT.ConfigureDataRate("", dataRate)
			   BT.ConfigurePayloadLength("", payloadLengthMode, payloadLength)
            BT.SelectMeasurements("", measurement, enableAllTraces)
            BT.Acp.Configuration.ConfigureBurstSynchronizationType("", burstSynchronizationType)
            BT.Acp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount)
            BT.Acp.Configuration.ConfigureOffsetChannelMode("", offsetChannelMode)
            If offsetChannelMode = RFmxBTMXAcpOffsetChannelMode.Symmetric Then
                BT.Acp.Configuration.ConfigureNumberOfOffsets("", numberOfOffsets)
            ElseIf offsetChannelMode = RFmxBTMXAcpOffsetChannelMode.InBand Then
                BT.ConfigureChannelNumber("", channelNumber)
            End If
            BT.Initiate("", "")
        End Sub

        Private Sub RetrieveResults()
            BT.Acp.Results.FetchMeasurementStatus("", timeout, measurementStatus)
            BT.Acp.Results.FetchReferenceChannelPower("", timeout, referenceChannelPower)
            BT.Acp.Results.FetchOffsetMeasurementArray("", timeout, lowerAbsolutePower, upperAbsolutePower, lowerRelativePower, upperRelativePower, lowerMargin, upperMargin)
            BT.Acp.Results.FetchMaskTrace("", timeout, limitWithExceptionMask, limitWithoutExceptionMask)
            BT.Acp.Results.FetchAbsolutePowerTrace("", timeout, absolutePower)
            BT.Acp.Results.FetchSpectrum("", timeout, spectrum)
        End Sub

        Private Sub PrintResults()
            Console.WriteLine("------------------ACP------------------")
            Console.WriteLine("Measurement Status                : {0}", measurementStatus)
            Console.WriteLine("Reference Channel Power (dBm)     : {0}", referenceChannelPower)
            Console.WriteLine()

            Console.WriteLine("------------------Offset Measuremensts------------------")
            For i As Integer = 0 To lowerAbsolutePower.Length - 1
                Console.WriteLine("Offset " + i.ToString())
                Console.WriteLine("Lower Absolute Powers (dBm)       : {0} ", lowerAbsolutePower(i))
                Console.WriteLine("Upper Absolute Powers (dBm)       : {0} ", upperAbsolutePower(i))
                Console.WriteLine("Lower Relative Powers (dB)        : {0} ", lowerRelativePower(i))
                Console.WriteLine("Upper Relative Powers (dB)        : {0} ", upperRelativePower(i))
                Console.WriteLine("Lower Margin (dB)                 : {0} ", lowerMargin(i))
                Console.WriteLine("Upper Margin (dB)                 : {0} ", upperMargin(i))
                Console.WriteLine()
            Next
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
