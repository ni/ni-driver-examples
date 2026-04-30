'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
'4. Configure Trigger Type and Trigger Parameters.
'5. Configure Packet Type.
'6. Configure Payload Length.
'7. Select ModAcc measurement and enable Traces.
'8. Configure ModAcc Burst Synchronization Mode.
'9. Configure Averaging Parameters for ModAcc measurement.
'10. Initiate the Measurement.
'11. Fetch ModAcc Measurements and Trace.
'12. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.BTMX

Namespace NationalInstruments.Examples.RFmxBTDevm

    Public Class RFmxBTDevm
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
        Private iqPowerEdgeTriggerLevel As Double
        Private minimumQuiteTimeMode As RFmxBTMXTriggerMinimumQuietTimeMode
        Private minimumQuietTime As Double
        Private iqPowerEdgeTriggerLevelType As RFmxBTMXIQPowerEdgeTriggerLevelType
        Private triggerDelay As Double

        Private packetType As RFmxBTMXPacketType

        Private payloadLengthMode As RFmxBTMXPayloadLengthMode
        Private payloadLength As Integer

        Private burstSynchronizationType As RFmxBTMXModAccBurstSynchronizationType

        Private measurement As RFmxBTMXMeasurementTypes
        Private enableAllTraces As Boolean

        Private averagingEnabled As RFmxBTMXModAccAveragingEnabled
        Private averagingCount As Integer

        Private timeout As Double
        Private peakRmsDevmMaximum As Double
        Private peakDevmMaximum As Double
        Private ninetyninePercentDevm As Double

        Private headerFrequencyErrorWiMaximum As Double
        Private peakFrequencyErrorWiPlusW0Maximum As Double
        Private peakFrequencyErrorW0Maximum As Double

        Private time As Single()
        Private frequencyErrorWiPlusW0 As Single()

        Private devmPerSymbol As Single()
        Private constellation As ComplexSingle()

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

            packetType = RFmxBTMXPacketType.PacketType2DH1

            payloadLengthMode = RFmxBTMXPayloadLengthMode.Auto
            payloadLength = 10                                                           '(bytes)

            burstSynchronizationType = RFmxBTMXModAccBurstSynchronizationType.Preamble

            measurement = RFmxBTMXMeasurementTypes.ModAcc
            enableAllTraces = True

            averagingEnabled = RFmxBTMXModAccAveragingEnabled.False
            averagingCount = 10

            timeout = 10.0                                                              '(seconds )
            peakRmsDevmMaximum = 0                                                      '(%)
            peakDevmMaximum = 0                                                         '(%)
            ninetyninePercentDevm = 0.0                                                 '(%)
            headerFrequencyErrorWiMaximum = 0.0                                         '(Hz)
            peakFrequencyErrorWiPlusW0Maximum = 0.0                                     '(Hz)
            peakFrequencyErrorW0Maximum = 0.0                                           '(Hz)
        End Sub

        Private Sub InitializeInstr()
            instrSession = New RFmxInstrMX(rfsaResourceName, "")
        End Sub

        Private Sub ConfigureBT()
            BT = instrSession.GetBTSignalConfiguration()       ' Create a new RFmx Session
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
            BT.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
            BT.ConfigureIQPowerEdgeTrigger("", "0", iqPowerEdgeTriggerSlope, iqPowerEdgeTriggerLevel,
            triggerDelay, minimumQuiteTimeMode, minimumQuietTime, iqPowerEdgeTriggerLevelType,
            enableTrigger)
            BT.ConfigurePacketType("", packetType)
            BT.ConfigurePayloadLength("", payloadLengthMode, payloadLength)
            BT.SelectMeasurements("", measurement, enableAllTraces)
            BT.ModAcc.Configuration.ConfigureBurstSynchronizationType("", burstSynchronizationType)
            BT.ModAcc.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount)
            BT.Initiate("", "")
        End Sub

        Private Sub RetrieveResults()
            BT.ModAcc.Results.FetchDevm("", timeout, peakRmsDevmMaximum, peakDevmMaximum, ninetyninePercentDevm)
            BT.ModAcc.Results.FetchFrequencyErrorEdr("", timeout, headerFrequencyErrorWiMaximum, peakFrequencyErrorWiPlusW0Maximum,
            peakFrequencyErrorW0Maximum)
            BT.ModAcc.Results.FetchDevmPerSymbolTrace("", timeout, devmPerSymbol)
            BT.ModAcc.Results.FetchFrequencyErrorWiPlusW0TraceEdr("", timeout, time, frequencyErrorWiPlusW0)
            BT.ModAcc.Results.FetchConstellationTrace("", timeout, constellation)
        End Sub

        Private Sub PrintResults()
            Console.WriteLine("------------------DEVM------------------")
            Console.WriteLine("Peak Rms Devm Maximum (%)                : {0}", peakRmsDevmMaximum)
            Console.WriteLine("Peak Devm Maximum (%)                    : {0}", peakDevmMaximum)
            Console.WriteLine("99% Devm (%)                             : {0}", ninetyninePercentDevm)
            Console.WriteLine("------------------EDE Frequency Error------------------")
            Console.WriteLine("Header Frequency Error wi Maximum (Hz)   : {0}", headerFrequencyErrorWiMaximum)
            Console.WriteLine("Peak Frequency Error wi+w0 Maximum (Hz)  : {0}", peakFrequencyErrorWiPlusW0Maximum)
            Console.WriteLine("Peak Frequency Error w0 Maximum (Hz)     : {0}", peakFrequencyErrorW0Maximum)
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
