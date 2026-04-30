'Steps:
'1. Open a new RFmx session.
'2. Configure Frequency Reference.
'3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
'4. Configure Trigger Type and Trigger Parameters..
'5. Select PvT measurement and enable traces.
'6. Configure Midamble.
'7. Configure Measurement Method.
'8. Configure Averaging.
'9. Initiate the Measurement.
'10. Fetch PvT Measurements and Traces.
'11. Close the RFmx session.

Imports NationalInstruments
Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.TdscdmaMX

Namespace NationalInstruments.Examples.RFmxTdscdmaPvt
    Public Class RFmxTdscdmaPvt
        Private instrSession As RFmxInstrMX
        Private tdscdma As RFmxTdscdmaMX

        Private resourceName As String, iqPowerEdgeTriggerSource As String, frequencySource As String
        Private centerFrequency As Double, referenceLevel As Double, externalAttenuation As Double, minimumQuietTimeDuration As Double, frequencyReferenceFrequency As Double, triggerDelay As Double, _
         iqPowerEdgeTriggerLevel As Double
        Private timeout As Double

        Private averagingEnabled As RFmxTdscdmaMXPvtAveragingEnabled
        Private averagingType As RFmxTdscdmaMXPvtAveragingType
        Private measurementMethod As RFmxTdscdmaMXPvtMeasurementMethod
        Private minimumQuietTimeMode As RFmxTdscdmaMXTriggerMinimumQuietTimeMode
        Private midambleAutoDetectionMode As RFmxTdscdmaMXMidambleAutoDetectionMode
        Private enableTrigger As Boolean
        Private iqPowerEdgeTriggerLevelType As RFmxTdscdmaMXIQPowerEdgeTriggerLevelType
        Private iqPowerEdgeTriggerSlope As RFmxTdscdmaMXIQPowerEdgeTriggerSlope

        Private segmentStatus As RFmxTdscdmaMXPvtSegmentStatus()
        Private measurementStatus As RFmxTdscdmaMXPvtMeasurementStatus
        Private averagingCount As Integer
        Private maximumNumberOfUsers As Integer, midambleShift As Integer


        Private meanAbsoluteONPower As Double, meanAbsoluteOFFPower As Double

        Private segmentMargin As Double()
        Private segmentMarginTime As Double()
        Private segmentMeanAbsolutePower As Double()
        Private segmentMaximumAbsolutePower As Double()
        Private segmentMinimumAbsolutePower As Double()

        Public Sub Run()
            Try
                InitializeVariables()
                InitializeInstr()
                ConfigureTdscdma()
                RetrieveResults()
                PrintResults()
            Catch ex As Exception
                DisplayError(ex)
            Finally
                ' Close session 

                CloseSession()
                Console.WriteLine("Press any key to exit.....")
                Console.ReadKey()
            End Try
        End Sub

        Private Sub InitializeVariables()
            resourceName = "RFSA"
            frequencySource = RFmxInstrMXConstants.OnboardClock
            frequencyReferenceFrequency = 10000000.0
            ' Hz 
            centerFrequency = 1910000000.0
            ' Hz 
            referenceLevel = 0.0
            ' dBm 
            externalAttenuation = 0.0
            ' dB 
            timeout = 10.0
            ' seconds 
            enableTrigger = True
            iqPowerEdgeTriggerSource = "0"
            iqPowerEdgeTriggerSlope = RFmxTdscdmaMXIQPowerEdgeTriggerSlope.Rising
            iqPowerEdgeTriggerLevel = -20.0
            'dB
            iqPowerEdgeTriggerLevelType = RFmxTdscdmaMXIQPowerEdgeTriggerLevelType.Relative
            triggerDelay = 0.0
            ' seconds 
            minimumQuietTimeMode = RFmxTdscdmaMXTriggerMinimumQuietTimeMode.Auto
            minimumQuietTimeDuration = 0.000016
            ' seconds 
            averagingEnabled = RFmxTdscdmaMXPvtAveragingEnabled.False
            averagingCount = 10
            averagingType = RFmxTdscdmaMXPvtAveragingType.Rms
            measurementMethod = RFmxTdscdmaMXPvtMeasurementMethod.Normal
            midambleAutoDetectionMode = RFmxTdscdmaMXMidambleAutoDetectionMode.MidambleShift
            maximumNumberOfUsers = 16
            midambleShift = 8
            measurementStatus = 0
            meanAbsoluteONPower = 0.0
            ' dBm 
            meanAbsoluteOFFPower = 0.0
            ' dBm 
        End Sub

        Private Sub InitializeInstr()
            ' Create a new RFmx Session 

            instrSession = New RFmxInstrMX(resourceName, "")
        End Sub

        Private Sub ConfigureTdscdma()
            ' Get Tdscdma signal 

            tdscdma = instrSession.GetTdscdmaSignalConfiguration()

            ' Configure measurement 

            instrSession.ConfigureFrequencyReference("", frequencySource, frequencyReferenceFrequency)
            tdscdma.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
            tdscdma.ConfigureIQPowerEdgeTrigger("", iqPowerEdgeTriggerSource, iqPowerEdgeTriggerSlope, iqPowerEdgeTriggerLevel, triggerDelay, minimumQuietTimeMode, _
             minimumQuietTimeDuration, iqPowerEdgeTriggerLevelType, enableTrigger)
            tdscdma.SelectMeasurements("", RFmxTdscdmaMXMeasurementTypes.Pvt, True)
            tdscdma.ConfigureMidambleShift("", midambleAutoDetectionMode, maximumNumberOfUsers, midambleShift)
            tdscdma.Pvt.Configuration.ConfigureMeasurementMethod("", measurementMethod)
            tdscdma.Pvt.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)
            tdscdma.Initiate("", "")
        End Sub

        Private Sub RetrieveResults()
            Dim signalPower As AnalogWaveform(Of Single) = Nothing
            Dim absoluteLimit As AnalogWaveform(Of Single) = Nothing
            ' Retrieve results

            tdscdma.Pvt.Results.FetchMeasurementStatus("", timeout, measurementStatus)
            tdscdma.Pvt.Results.FetchPowers("", timeout, meanAbsoluteONPower, meanAbsoluteOFFPower)
            tdscdma.Pvt.Results.FetchSegmentMeasurementArray("", timeout, segmentStatus, segmentMargin, segmentMarginTime, segmentMeanAbsolutePower,
                                                             segmentMaximumAbsolutePower, segmentMinimumAbsolutePower)
            tdscdma.Pvt.Results.FetchSignalPowerTrace("", timeout, signalPower, absoluteLimit)


        End Sub

        Private Sub PrintResults()
            Console.WriteLine("--------------------PVT  Results--------------------")

            For i As Integer = 0 To segmentMargin.Length - 1

                Console.WriteLine(vbCrLf & "Segment                                     {0}", i)

                Console.WriteLine("Measurement Status                          {0}", segmentStatus(i))

                Console.WriteLine("Margin (dB)                                 {0}", segmentMargin(i))

                Console.WriteLine("Margin Time(sec)                            {0}", segmentMarginTime(i))

                Console.WriteLine("Mean Absolute Power (dBm)                   {0}", segmentMeanAbsolutePower(i))

                Console.WriteLine("Maximum Absolute Power (dBm)                {0}", segmentMaximumAbsolutePower(i))

                Console.WriteLine("Minimum Absolute Power (dBm)                {0}", segmentMinimumAbsolutePower(i))
            Next

            Console.WriteLine(vbCrLf & "Measurement Status                     {0}", measurementStatus)

            Console.WriteLine(vbCrLf & "--------------------Absolute Powers--------------------")

            Console.WriteLine("Mean Absolute ON Power (dBm)                  {0}", meanAbsoluteONPower)

            Console.WriteLine("Mean Absolute OFF Power (dBm)                 {0}", meanAbsoluteOFFPower)

        End Sub

        Private Sub CloseSession()
            Try
                If tdscdma IsNot Nothing Then
                    tdscdma.Dispose()
                    tdscdma = Nothing
                End If

                If instrSession IsNot Nothing Then
                    instrSession.Close()
                    instrSession = Nothing
                End If
            Catch ex As Exception
                DisplayError(ex)
            End Try
        End Sub

        Private Shared Sub DisplayError(ex As Exception)
            Console.WriteLine("ERROR:" & vbLf & Convert.ToString(ex.[GetType]()) & ": " & ex.Message)
        End Sub
    End Class
End Namespace
