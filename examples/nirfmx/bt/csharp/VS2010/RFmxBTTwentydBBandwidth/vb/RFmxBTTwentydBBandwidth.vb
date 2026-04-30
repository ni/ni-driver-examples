'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
'4. Configure Trigger Type and Trigger Parameters.
'5. Select TwentydBBandwidth measurement and enable Traces.
'6. Configure Averaging Parameters for TwentydBBandwidth measurement.
'7. Initiate the Measurement.
'8. Fetch TwentydBBandwidth Measurements and Trace.
'9. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.BTMX

Namespace NationalInstruments.Examples.RFmxBTTwentydBBandwidth
    Public Class RFmxBTTwentydBBandwidth
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

        Private measurement As RFmxBTMXMeasurementTypes
        Private enableAllTraces As Boolean

        Private averagingEnabled As RFmxBTMXTwentydBBandwidthAveragingEnabled
        Private averagingCount As Integer

        Private timeout As Double
        Private peakPower As Double
        Private bandwidth As Double
        Private highFrequemcy As Double
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
            iqPowerEdgeLevel = -20.0                                                     '(dB)
            minimumQuiteTimeMode = RFmxBTMXTriggerMinimumQuietTimeMode.Auto
            minimumQuietTime = 0.0001                                                    '(seconds)
            iqPowerEdgeTriggerLevelType = RFmxBTMXIQPowerEdgeTriggerLevelType.Relative
            triggerDelay = 0.0                                                           '(seconds)

            measurement = RFmxBTMXMeasurementTypes.TwentydBBandwidth
            enableAllTraces = True

            averagingEnabled = RFmxBTMXTwentydBBandwidthAveragingEnabled.False
            averagingCount = 10

            timeout = 10.0                                                               '(seconds)
            peakPower = 0.0                                                              '(dBm)
            bandwidth = 0.0                                                              '(Hz)
            highFrequemcy = 0.0                                                          '(Hz)
            lowFrequency = 0.0                                                           '(Hz)
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
            BT.TwentydBBandwidth.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount)
            BT.Initiate("", "")
        End Sub

        Private Sub RetrieveResults()
            BT.TwentydBBandwidth.Results.FetchMeasurement("", timeout, peakPower, bandwidth, highFrequemcy, lowFrequency)
            BT.TwentydBBandwidth.Results.FetchSpectrum("", timeout, spectrum)
        End Sub

        Private Sub PrintResults()
            Console.WriteLine("------------------Measurement------------------")
            Console.WriteLine("Peak Power (dBm)                                : {0}", peakPower)
            Console.WriteLine("Bandwidth (Hz)                                  : {0}", bandwidth)
            Console.WriteLine("High Frequency (Hz)                             : {0}", highFrequemcy)
            Console.WriteLine("Low Frequency (Hz)                              : {0}", lowFrequency)
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
