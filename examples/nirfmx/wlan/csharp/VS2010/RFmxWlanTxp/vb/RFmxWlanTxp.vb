'Steps:
'1. Open a new RFmx session.
'2. Configure the frequency reference properties (Clock Source and Clock Frequency).
'3. Configure the basic signal properties (Center Frequency and External Attenuation).
'4. Configure IQ Power Edge Trigger properties (Trigger Delay, IQ Power Edge Level, Minimum Quiet Time).
'5. Configure Standard and Channel Bandwidth Properties.
'6. Configure Reference Level.
'7. Select TXP measurement and enable the traces.
'8. Configure the Measurement Interval.
'9. Configure Averaging parameters.
'10. Initiate Measurement.
'11. Fetch TXP Traces and Measurements.
'12. Close the RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.WlanMX

Namespace NationalInstruments.Examples.RFmxWlanTxp

    Public Class RFmxWlanTxp
        Private instrSession As RFmxInstrMX
        Private wlan As RFmxWlanMX
        Private resourceName As String

        Private centerFrequency As Double
        Private referenceLevel As Double
        Private externalAttenuation As Double
        Private autoLevel As Boolean
        Private measurementInterval As Double

        Private frequencyReferenceSource As String
        Private frequencyReferenceFrequency As Double

        Private iqPowerEdgeEnabled As Boolean
        Private iqPowerEdgeLevel As Double
        Private triggerDelay As Double
        Private minimumQuietTimeMode As RFmxWlanMXTriggerMinimumQuietTimeMode
        Private minimumQuietTime As Double

        Private standard As RFmxWlanMXStandard

        Private channelBandwidth As Double

        Private averagingEnabled As RFmxWlanMXTxpAveragingEnabled
        Private averagingCount As Integer

        Private timeout As Double

        Private maximumMeasurementInterval As Double

        Private power As AnalogWaveform(Of Single)

        Private averagePowerMean As Double, peakPowerMaximum As Double

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
            autoLevel = True
            measurementInterval = 0.01
            ' (s)

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

            standard = RFmxWlanMXStandard.Standard802_11ag

            channelBandwidth = 20000000.0
            '(Hz)

            averagingEnabled = RFmxWlanMXTxpAveragingEnabled.[False]
            averagingCount = 10

            maximumMeasurementInterval = 0.001
            ' (s)

            timeout = 10.0
            ' (s)

            averagePowerMean = 0
            ' (dBm)
            peakPowerMaximum = 0
            ' (dBm)
        End Sub

        Private Sub InitializeInstr()
            instrSession = New RFmxInstrMX(resourceName, "")
        End Sub

        Private Sub ConfigureWlan()
            wlan = instrSession.GetWlanSignalConfiguration()
            ' Create a new RFmx Session
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
            wlan.ConfigureFrequency("", centerFrequency)
            wlan.ConfigureExternalAttenuation("", externalAttenuation)
            wlan.ConfigureIQPowerEdgeTrigger("", "0", RFmxWlanMXIQPowerEdgeTriggerSlope.Rising, iqPowerEdgeLevel, triggerDelay, minimumQuietTimeMode,
               minimumQuietTime, RFmxWlanMXIQPowerEdgeTriggerLevelType.Relative, iqPowerEdgeEnabled)
            wlan.ConfigureStandard("", standard)
            wlan.ConfigureChannelBandwidth("", channelBandwidth)
            If autoLevel Then
                wlan.AutoLevel("", measurementInterval)
            Else
                wlan.ConfigureReferenceLevel("", referenceLevel)
            End If
            wlan.SelectMeasurements("", RFmxWlanMXMeasurementTypes.Txp, True)
            wlan.Txp.Configuration.ConfigureMaximumMeasurementInterval("", maximumMeasurementInterval)
            wlan.Txp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount)
            wlan.Initiate("", "")
        End Sub

        Private Sub RetrieveResults()
            wlan.Txp.Results.FetchPowerTrace("", timeout, power)
            wlan.Txp.Results.FetchMeasurement("", timeout, averagePowerMean, peakPowerMaximum)
        End Sub

        Private Sub PrintResults()
            Console.WriteLine(vbLf & "----------Measurement----------" & vbLf)
            Console.WriteLine("Average Power Mean (dBm)         :{0}", averagePowerMean)
            Console.WriteLine("Peak Power Maximum (dBm)         :{0}", peakPowerMaximum)
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
