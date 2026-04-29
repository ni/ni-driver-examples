'Steps:
'1. Open a new RFmx session.
'2. Configure the frequency reference properties (Clock Source and Clock Frequency).
'3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
'4. Configure IQ Power Edge Trigger properties (Trigger Delay, IQ Power Edge Level, Minimum Quiet Time).
'5. Configure Standard as 802.11b.
'6. Select PowerRamp measurement and enable the traces.
'7. Configure the Acquisition Length.
'8. Configure Averaging parameters.
'9. Initiate Measurement.
'10. Fetch PowerRamp Traces and Measurements.
'11. Close the RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.WlanMX

Namespace NationalInstruments.Examples.RFmxWlanDsssPowerRamp

    Public Class RFmxWlanDsssPowerRamp
        Private instrSession As RFmxInstrMX
        Private wlan As RFmxWlanMX
        Private resourceName As String

        Private centerFrequency As Double
        Private referenceLevel As Double
        Private externalAttenuation As Double

        Private frequencyReferenceSource As String
        Private frequencyReferenceFrequency As Double

        Private iqPowerEdgeEnabled As Boolean
        Private iqPowerEdgeLevel As Double
        Private triggerDelay As Double
        Private minimumQuietTimeMode As RFmxWlanMXTriggerMinimumQuietTimeMode
        Private minimumQuietTime As Double

        Private standard As RFmxWlanMXStandard

        Private acquisitionLength As Double

        Private averagingEnabled As RFmxWlanMXPowerRampAveragingEnabled
        Private averagingCount As Integer

        Private timeout As Double

        Private fallTimeMean As Double
        Private riseTimeMean As Double

        Private riseTraceRawWaveform As AnalogWaveform(Of Single)
        Private riseTraceProcessesWaveform As AnalogWaveform(Of Single)
        Private riseTraceThreshold As AnalogWaveform(Of Single)
        Private riseTracePowerReference As AnalogWaveform(Of Single)

        Private fallTraceRawWaveform As AnalogWaveform(Of Single)
        Private fallTraceProcessesWaveform As AnalogWaveform(Of Single)
        Private fallTraceThreshold As AnalogWaveform(Of Single)
        Private fallTracePowerReference As AnalogWaveform(Of Single)

        Public Sub Run()
            Try
                InitializeVariables()
                InitializeInstr()
                ConfigureWlan()
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

            centerFrequency = 2412000000.0
            ' (Hz)
            referenceLevel = 0.0
            ' (dBm)
            externalAttenuation = 0.0
            ' (dB)

            frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
            frequencyReferenceFrequency = 10000000.0
            ' (Hz)

            iqPowerEdgeEnabled = True
            iqPowerEdgeLevel = -20.0
            '(dB)
            triggerDelay = 0.0
            ' (s)
            minimumQuietTimeMode = RFmxWlanMXTriggerMinimumQuietTimeMode.Auto
            minimumQuietTime = 0.000005
            ' (s)

            standard = RFmxWlanMXStandard.Standard802_11b

            acquisitionLength = 0.001
            ' (s)

            averagingEnabled = RFmxWlanMXPowerRampAveragingEnabled.[False]
            averagingCount = 10

            timeout = 10.0
            ' (s)
        End Sub

        Private Sub InitializeInstr()
            instrSession = New RFmxInstrMX(resourceName, "")
        End Sub

        Private Sub ConfigureWlan()
            wlan = instrSession.GetWlanSignalConfiguration()
            ' Create a new RFmx Session
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
            wlan.ConfigureFrequency("", centerFrequency)
            wlan.ConfigureReferenceLevel("", referenceLevel)
            wlan.ConfigureExternalAttenuation("", externalAttenuation)
            wlan.ConfigureIQPowerEdgeTrigger("", "0", RFmxWlanMXIQPowerEdgeTriggerSlope.Rising, iqPowerEdgeLevel, triggerDelay, minimumQuietTimeMode,
         minimumQuietTime, RFmxWlanMXIQPowerEdgeTriggerLevelType.Relative, iqPowerEdgeEnabled)
            wlan.ConfigureStandard("", standard)
            wlan.SelectMeasurements("", RFmxWlanMXMeasurementTypes.PowerRamp, True)
            wlan.PowerRamp.Configuration.ConfigureAcquisitionLength("", acquisitionLength)
            wlan.PowerRamp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount)
            wlan.Initiate("", "")
        End Sub

        Private Sub RetrieveResults()
            wlan.PowerRamp.Results.FetchMeasurement("", timeout, riseTimeMean, fallTimeMean)
            wlan.PowerRamp.Results.FetchRiseTrace("", timeout, riseTraceRawWaveform, riseTraceProcessesWaveform, riseTraceThreshold, riseTracePowerReference)
            wlan.PowerRamp.Results.FetchFallTrace("", timeout, fallTraceRawWaveform, fallTraceProcessesWaveform, fallTraceThreshold, fallTracePowerReference)
        End Sub

        Private Sub PrintResults()
            Console.WriteLine(vbLf & "---------------Measurement---------------" & vbLf)
            Console.WriteLine("Rise Time (s)                     :{0}", riseTimeMean)
            Console.WriteLine("Fall Time (s)                     :{0}", fallTimeMean)
        End Sub

        Private Sub CloseSession()
            If wlan IsNot Nothing Then
                wlan.Dispose()
                wlan = Nothing
            End If
            If instrSession IsNot Nothing Then
                instrSession.Close()
                instrSession = Nothing
            End If
        End Sub

        Private Shared Sub DisplayError(ex As Exception)
            Console.WriteLine("ERROR:" & vbLf & Convert.ToString(ex.[GetType]()) & ": " & ex.Message)
        End Sub

    End Class
End Namespace
