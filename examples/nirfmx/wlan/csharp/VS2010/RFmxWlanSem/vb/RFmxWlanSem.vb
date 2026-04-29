'Steps:
'1. Open a new RFmx session.
'2. Configure the frequency reference properties(Clock Source and Clock Frequency).
'3. Configure the basic signal properties(Center Frequency, Reference Level and External Attenuation).
'4. Configure IQ Power Edge Trigger properties(Trigger Delay, IQ Power Edge Level, Minimum Quiet Time).
'5. Configure Standard and Channel Bandwidth Properties.
'6. Select SEM measurement and enable the traces.
'7. Configure Averaging parameters.
'8. Configure Sweep Time and Span parameters.
'9. Initiate Measurement.
'10. Fetch SEM Traces and Measurements.
'11. Close the RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.WlanMX

Namespace NationalInstruments.Examples.RFmxWlanSem

    Public Class RFmxWlanSem
        Private instrSession As RFmxInstrMX
        Private wlan As RFmxWlanMX
        Private resourceName As String

        Private frequencyReferenceSource As String
        Private frequencyReferenceFrequency As Double

        Private centerFrequency As Double
        Private referenceLevel As Double
        Private externalAttenuation As Double

        Private iqPowerEdgeEnabled As Boolean
        Private iqPowerEdgeLevel As Double
        Private triggerDelay As Double
        Private minimumQuietTimeMode As RFmxWlanMXTriggerMinimumQuietTimeMode
        Private minimumQuietTime As Double

        Private standard As RFmxWlanMXStandard

        Private channelBandwidth As Double

        Private averagingEnabled As RFmxWlanMXSemAveragingEnabled
        Private averagingCount As Integer
        Private averagingType As RFmxWlanMXSemAveragingType

        Private spanAuto As RFmxWlanMXSemSpanAuto
        Private span As Double

        Private sweepTimeAuto As RFmxWlanMXSemSweepTimeAuto
        Private sweepTime As Double

        Private maskType As RFmxWlanMXSemMaskType

        Private timeout As Double

        Private measurementStatus As RFmxWlanMXSemMeasurementStatus

        Private absolutePower As Double
        ' (dBm)
        Private relativePower As Double
        ' (dBm)

        Private upperOffsetMeasurementStatus As RFmxWlanMXSemUpperOffsetMeasurementStatus()
        Private upperOffsetMargin As Double()
        '(dB)
        Private upperOffsetMarginFrequency As Double()
        '(Hz)
        Private upperOffsetMarginAbsolutePower As Double()
        '(dBm)
        Private upperOffsetMarginRelativePower As Double()
        '(dBm)

        Private lowerOffsetMeasurementStatus As RFmxWlanMXSemLowerOffsetMeasurementStatus()
        Private lowerOffsetMargin As Double()
        '(dB)
        Private lowerOffsetMarginFrequency As Double()
        '(Hz)
        Private lowerOffsetMarginAbsolutePower As Double()
        '(dBm)
        Private lowerOffsetMarginRelativePower As Double()
        '(dBm)

        Private spectrum As Spectrum(Of Single)
        Private compositeMask As Spectrum(Of Single)

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
            ' (dB)
            triggerDelay = 0.0
            ' (s)
            minimumQuietTimeMode = RFmxWlanMXTriggerMinimumQuietTimeMode.Auto
            minimumQuietTime = 0.000005
            ' (s)

            standard = RFmxWlanMXStandard.Standard802_11ag

            channelBandwidth = 20000000.0
            ' (Hz)

            averagingEnabled = RFmxWlanMXSemAveragingEnabled.False
            averagingCount = 10
            averagingType = RFmxWlanMXSemAveragingType.Rms

            spanAuto = RFmxWlanMXSemSpanAuto.True
            span = 66000000.0
            ' (Hz)

            sweepTimeAuto = RFmxWlanMXSemSweepTimeAuto.True
            sweepTime = 0.001
            ' (s)

            maskType = RFmxWlanMXSemMaskType.Standard
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
            wlan.ConfigureIQPowerEdgeTrigger("", "0", RFmxWlanMXIQPowerEdgeTriggerSlope.Rising, iqPowerEdgeLevel,
            triggerDelay, minimumQuietTimeMode, minimumQuietTime, RFmxWlanMXIQPowerEdgeTriggerLevelType.Relative,
            iqPowerEdgeEnabled)
            wlan.ConfigureStandard("", standard)
            wlan.ConfigureChannelBandwidth("", channelBandwidth)
            wlan.SelectMeasurements("", RFmxWlanMXMeasurementTypes.Sem, True)
            wlan.Sem.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)
            wlan.Sem.Configuration.ConfigureMaskType("", maskType)
            wlan.Sem.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTime)
            wlan.Sem.Configuration.ConfigureSpan("", spanAuto, span)
            wlan.Initiate("", "")
        End Sub

        Private Sub RetrieveResults()
            wlan.Sem.Results.FetchMeasurementStatus("", timeout, measurementStatus)
            wlan.Sem.Results.FetchCarrierMeasurement("", timeout, absolutePower,
         relativePower)
            wlan.Sem.Results.FetchLowerOffsetMarginArray("", timeout, lowerOffsetMeasurementStatus, lowerOffsetMargin,
         lowerOffsetMarginFrequency, lowerOffsetMarginAbsolutePower, lowerOffsetMarginRelativePower)
            wlan.Sem.Results.FetchUpperOffsetMarginArray("", timeout, upperOffsetMeasurementStatus, upperOffsetMargin,
         upperOffsetMarginFrequency, upperOffsetMarginAbsolutePower, upperOffsetMarginRelativePower)
            wlan.Sem.Results.FetchSpectrum("", timeout, spectrum, compositeMask)
        End Sub

        Private Sub PrintResults()
            Console.WriteLine("Measurement Status                       :{0}", measurementStatus)
            Console.WriteLine("Carrier Absolute Power (dBm)             :{0}", absolutePower)

            Console.WriteLine(vbLf & "----------Lower Offset Measurements----------" & vbLf)
            For i As Integer = 0 To lowerOffsetMargin.Length - 1
                Console.WriteLine("Offset {0}", i)
                Console.WriteLine("Measurement Status              :{0}", lowerOffsetMeasurementStatus(i))
                Console.WriteLine("Margin (dB)                     :{0}", lowerOffsetMargin(i))
                Console.WriteLine("Margin Frequency (Hz)           :{0}", lowerOffsetMarginFrequency(i))
                Console.WriteLine("Margin Absolute Power (dBm)     :{0}", lowerOffsetMarginAbsolutePower(i) & vbLf)
            Next

            Console.WriteLine(vbLf & "----------Upper Offset Measurements----------" & vbLf)
            For i As Integer = 0 To upperOffsetMargin.Length - 1
                Console.WriteLine("Offset {0}", i)
                Console.WriteLine("Measurement Status              :{0}", upperOffsetMeasurementStatus(i))
                Console.WriteLine("Margin (dB)                     :{0}", upperOffsetMargin(i))
                Console.WriteLine("Margin Frequency (Hz)           :{0}", upperOffsetMarginFrequency(i))
                Console.WriteLine("Margin Absolute Power (dBm)     :{0}", upperOffsetMarginAbsolutePower(i) & vbLf)
            Next

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
