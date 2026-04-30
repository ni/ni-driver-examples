'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
'4. Configure Trigger Type and Trigger Parameters.
'5. Select FrequencyRange measurement and enable Traces.
'6. Configure Averaging Parameters for FrequencyRange measurement.
'7. Configure Span.
'8. Initiate the Measurement.
'9. Fetch FrequencyRange Measurements and Trace.
'10. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.BTMX

Namespace NationalInstruments.Examples.RFmxBTFrequencyRange

    Public Class RFmxBTFrequencyRange
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

        Private span As Double

        Private measurement As RFmxBTMXMeasurementTypes
        Private enableAllTraces As Boolean

        Private averagingEnabled As RFmxBTMXFrequencyRangeAveragingEnabled
        Private averagingCount As Integer

        Private timeout As Double
        Private highFrequency As Double
        Private lowFrequency As Double

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
            rfsaResourceName = "RFSA"

            frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
            frequencyReferenceFrequency = 10000000.0                                     ' (Hz)

            centerFrequency = 2402000000.0                                               ' (Hz)
            referenceLevel = 0.0                                                         ' (dBm)
            externalAttenuation = 0.0                                                    ' (dB)

            enableTrigger = True
            iqPowerEdgeTriggerSlope = RFmxBTMXIQPowerEdgeTriggerSlope.Rising
            iqPowerEdgeLevel = -20.0                                                     ' (dB)
            minimumQuiteTimeMode = RFmxBTMXTriggerMinimumQuietTimeMode.Auto
            minimumQuietTime = 0.0001                                                    ' (seconds)
            iqPowerEdgeTriggerLevelType = RFmxBTMXIQPowerEdgeTriggerLevelType.Relative
            triggerDelay = 0.0                                                           ' (seconds)

            span = 10000000.0                                                            ' (Hz)

            measurement = RFmxBTMXMeasurementTypes.FrequencyRange
            enableAllTraces = True

            averagingEnabled = RFmxBTMXFrequencyRangeAveragingEnabled.False
            averagingCount = 10

            timeout = 10.0                                                               ' (seconds)
            highFrequency = 0.0                                                          ' (dBm)
            lowFrequency = 0.0                                                           ' (dBm)
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
            BT.SelectMeasurements("", measurement, enableAllTraces)
            BT.FrequencyRange.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount)
            BT.FrequencyRange.Configuration.ConfigureSpan("", span)
            BT.Initiate("", "")
        End Sub

        Private Sub RetrieveResults()
            BT.FrequencyRange.Results.FetchMeasurement("", timeout, highFrequency, lowFrequency)
            BT.FrequencyRange.Results.FetchSpectrum("", timeout, spectrum)
        End Sub

        Private Sub PrintResults()
            Console.WriteLine("------------------Measurement------------------")
            Console.WriteLine("High Frequency (Hz)                      : {0}", highFrequency)
            Console.WriteLine("Low Frequency (Hz)                       : {0}", lowFrequency)
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
