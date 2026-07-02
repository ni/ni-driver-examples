' Steps:
'1. Open RFSG session.
'2. Configure RFSG frequency reference.
'3. Configure marker0 to be generated from RFSG on the specified output terminal.
'4. Configure frequency and power level of RF output signal.
'5. Set RFSG External Gain. #4 and #5 ensure that the average power of the signal at the input of the DUT
'   matches the user configured DUT Average Input Power.
'6. Open RFmx session.
'7. Configure frequency reference of the analyser.
'8. Configure Selected Ports.
'9. Configure trigger to use as reference for signal acquisition.
'10. Configure center frequency and external attenuation.
'11. Select IDPD measurement, configure the reference waveform.
'12. Equalizer Mode is fixed to Train Mode.
'13. Configure power of reference signal at the input of the DUT.
'      Set Equalizer mode and filter length.
'      Set Sample rate and it's mode.
'      Set Averaging Mode and count. 
'      Set Start and Stop for Impairment estimation. 
'14. Commit IDPD Measurement to get Equalizer Training waveform. Equalizer training waveform is 
'      multitone waveform with same spectral occupancy as the reference waveform. This is because 
'      equalizer training waveform is multitone wavaform to ensure that obtained equalizer coefficients 
'      can be used for wider set of test waveform configurations.
'15. Get Equalizer Training waveform and  scale from -1 to 1 as RFSG power level type is peak power.
'16. Write Equalizer Training waveform to RFSG
'      Set Waveform Runtime Scaling to the desired Pre-filter Gain.
'      Read waveform sample rate, multiply by 0.8 and set the result to the signal bandwidth.
'      Write script to generate the waveform specified in the script. This script is programmed 
'      to generate waveform continuously, with marker0 aligned to sample index 0.
'      Initiate RFSG generation. 
'17. Perform Auto Level to compute an approximate reference level to be used by the analyser.
'18. Initiates IDPD measurement.
'19. Fetch Trained Equalizer Coefficients.
'20. Close RFmx session.
'21. Close RFSG session. 
'It is recommended to clear the waveform before closing RFSG session.


Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.SpecAnMX
Imports NationalInstruments.ModularInstruments.NIRfsg
Imports NationalInstruments.ModularInstruments.NIRfsgPlayback

Namespace NationalInstruments.Examples.RFmxSpecAnIdpdEqualizerTrain
    Public Class RFmxSpecAnIdpdEqualizerTrain
        Private instrSession As RFmxInstrMX
        Private specAn As RFmxSpecAnMX
        Private rfsgSession As NIRfsg
        Private instrumentHandle As IntPtr

        Private rfsaResourceName As String = "RFSA"
        Private rfsgResourceName As String = "RFSG"

        Private referenceWaveformFile As String = "LTE20MHz Waveform (Two Subframes).tdms"

        Private referenceClockSource As RfsgFrequencyReferenceSource = RfsgFrequencyReferenceSource.OnboardClock
        Private referenceClockRate As Double = 10000000.0
        Private triggerDelay As Double = 0
        ' seconds 
        Private dutAverageInputPower As Double = -20
        ' dBm 
        Private selectedPorts As String = ""
        Private centerFrequency As Double = 1000000000.0
        ' Hz 
        Private referenceLevel As Double = 0.0
        ' dBm 
        Private rfsaExternalAttenuation As Double = 0.0
        ' dB 
        Private rfsgExternalAttenuation As Double = 0.0
        ' dB 
        Private preFilterGain As Double = -1.5
        ' dB 
        Private runtimeScaling As Double
        Private papr As Double = 0.0

        Private frequencyReferenceSource As String = RFmxInstrMXConstants.OnboardClock
        Private frequencyReferenceFrequency As Double = 10000000.0
        ' Hz 
        Private enableTrigger As Boolean = True
        Private triggerType As RFmxSpecAnMXTriggerType = RFmxSpecAnMXTriggerType.DigitalEdge

        Private sampleRateMode As RFmxSpecAnMXIdpdMeasurementSampleRateMode = RFmxSpecAnMXIdpdMeasurementSampleRateMode.ReferenceWaveform
        Private sampleRate As Double = 120000000.0
        ' S/s 
        Private idleDurationPresent As RFmxSpecAnMXIdpdReferenceWaveformIdleDurationPresent = RFmxSpecAnMXIdpdReferenceWaveformIdleDurationPresent.[False]
        Private signalType As RFmxSpecAnMXIdpdSignalType = RFmxSpecAnMXIdpdSignalType.Modulated

        Private equalizerMode As RFmxSpecAnMXIdpdEqualizerMode = RFmxSpecAnMXIdpdEqualizerMode.Train

        Private signalBandwidth As Double = 20000000.0
        ' Hz 
        Private autoLevelMeasurementInterval As Double = 0.0001
        ' seconds 
        Private autoLevelReferenceLevel As Double

        Private timeout As Double = 10
        ' seconds 

        Private scriptName As String = "IDPDScript"
        Private waveformName As String = "Wfm"
        Private waveformSize As Integer
        Private rfsgIqRate As Double
        Private markerNumber As Integer = 0
        Private waveformScript As String

        Private idpdMeanRmsEvm As Double()
        Private equalizerWaveform As ComplexWaveform(Of ComplexSingle), normalizedEqualizerWaveform As ComplexWaveform(Of ComplexSingle)
        Private referenceWaveformComplexSingle As ComplexWaveform(Of ComplexSingle)
        Private equalizerCoefficients As ComplexWaveform(Of ComplexSingle)
        Friend Sub Run()
            Try
                ReadWaveformFromTdmsFile()
                ReadWaveFormSizeFromTdmsFile()
                OpenSession()
                ConfigureRfsg()
                ConfigureRFmx()
                RetrieveResults()
                DisplayResults()
            Catch ex As Exception
                DisplayError(ex)
            Finally
                CloseSessions()
                Console.WriteLine("Press any key to exit.....")
                Console.ReadKey()
            End Try
        End Sub

        Private Sub ReadWaveformFromTdmsFile()
            NIRfsgPlayback.ReadWaveformFromFileComplex(referenceWaveformFile, referenceWaveformComplexSingle)
        End Sub

        Private Sub ReadWaveFormSizeFromTdmsFile()
            NIRfsgPlayback.ReadWaveformSizeFromFile(referenceWaveformFile, 0, waveformSize)
        End Sub

        Private Sub OpenSession()
            instrSession = New RFmxInstrMX(rfsaResourceName, "")
            specAn = instrSession.GetSpecAnSignalConfiguration()
        End Sub

        Private Sub ConfigureRfsg()
            rfsgSession = New NIRfsg(rfsgResourceName, False, True)
            rfsgSession.FrequencyReference.Configure(referenceClockSource, referenceClockRate)
            rfsgSession.DeviceEvents.MarkerEvents(markerNumber).ExportedOutputTerminal = RfsgMarkerEventExportedOutputTerminal.PxiTriggerLine0
            rfsgSession.RF.Configure(centerFrequency, dutAverageInputPower)
            rfsgSession.RF.PowerLevelType = RfsgRFPowerLevelType.PeakPower
            rfsgSession.RF.ExternalGain = -rfsgExternalAttenuation
            rfsgSession.RF.Upconverter.FrequencyOffsetMode = UpconverterFrequencyOffsetMode.Auto

            instrumentHandle = rfsgSession.GetInstrumentHandle().DangerousGetHandle()
        End Sub

        Private Sub ConfigureRFmx()
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
            specAn.SetSelectedPorts("", selectedPorts)
            If triggerType = RFmxSpecAnMXTriggerType.DigitalEdge Then
                specAn.ConfigureDigitalEdgeTrigger("", RFmxInstrMXConstants.PxiTriggerLine0, RFmxSpecAnMXDigitalEdgeTriggerEdge.Rising, triggerDelay, enableTrigger)
            ElseIf triggerType = RFmxSpecAnMXTriggerType.IQPowerEdge Then
                specAn.ConfigureIQPowerEdgeTrigger("", "0", -20.0, RFmxSpecAnMXIQPowerEdgeTriggerSlope.Rising, triggerDelay, RFmxSpecAnMXTriggerMinimumQuietTimeMode.Manual,
                0.0, enableTrigger)
            End If
            specAn.ConfigureRF("", centerFrequency, referenceLevel, rfsaExternalAttenuation)
            specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Idpd, True)
            specAn.Idpd.Configuration.ConfigureReferenceWaveform("", referenceWaveformComplexSingle, idleDurationPresent, signalType)
            specAn.Idpd.Configuration.SetEqualizerMode("", equalizerMode)
            specAn.Idpd.Configuration.SetDutAverageInputPower("", dutAverageInputPower)
            specAn.Idpd.Configuration.SetMeasurementSampleRateMode("", sampleRateMode)
            specAn.Idpd.Configuration.SetMeasurementSampleRate("", sampleRate)
            specAn.Commit("")
        End Sub

        Private Sub RetrieveResults()
            specAn.Idpd.Results.GetEqualizerReferenceWaveform("", equalizerWaveform, papr)

            ' Storing the predistorted waveform again to normalize it 

            Dim x0 As Double = equalizerWaveform.PrecisionTiming.TimeOffset.TotalSeconds
            Dim dx As Double = equalizerWaveform.PrecisionTiming.SampleInterval.TotalSeconds
            normalizedEqualizerWaveform = New ComplexWaveform(Of ComplexSingle)(0)
            normalizedEqualizerWaveform.Append(equalizerWaveform.GetRawData())
            Dim precisionTiming As PrecisionWaveformTiming = PrecisionWaveformTiming.CreateWithRegularInterval(New PrecisionTimeSpan(dx), New PrecisionTimeSpan(x0))
            normalizedEqualizerWaveform.PrecisionTiming = precisionTiming

            'Normalization

            Dim equalizerWaveformRawData As ComplexSingle() = normalizedEqualizerWaveform.GetRawData()
            Dim maxElement As Single = equalizerWaveformRawData(0).Magnitude

            For k As Integer = 1 To equalizerWaveformRawData.Length - 1
                If equalizerWaveformRawData(k).Magnitude > maxElement Then
                    maxElement = equalizerWaveformRawData(k).Magnitude
                End If
            Next

            For j As Integer = 0 To equalizerWaveformRawData.Length - 1
                equalizerWaveformRawData(j) /= New ComplexSingle(maxElement, 0)
            Next

            normalizedEqualizerWaveform = New ComplexWaveform(Of ComplexSingle)(0)
            normalizedEqualizerWaveform.Append(equalizerWaveformRawData)
            normalizedEqualizerWaveform.PrecisionTiming = precisionTiming

            rfsgIqRate = 1 / normalizedEqualizerWaveform.PrecisionTiming.SampleInterval.TotalSeconds
            rfsgSession.Arb.WriteWaveform(waveformName, normalizedEqualizerWaveform)
            NIRfsgPlayback.StoreWaveformRuntimeScaling(instrumentHandle, waveformName, runtimeScaling)
            NIRfsgPlayback.StoreWaveformSampleRate(instrumentHandle, waveformName, rfsgIqRate)
            NIRfsgPlayback.StoreWaveformPapr(instrumentHandle, waveformName, papr)
            NIRfsgPlayback.StoreWaveformSignalBandwidth(instrumentHandle, waveformName, 0.8 * rfsgIqRate)
            waveformScript = [String].Format("script {0}{1}repeat forever{1}generate {2} marker{3}(0){1}end repeat{1}end script", scriptName, Environment.NewLine, waveformName, markerNumber)
            NIRfsgPlayback.SetScriptToGenerateSingleRfsg(instrumentHandle, waveformScript)
            rfsgSession.Initiate()

            specAn.AutoLevel("", signalBandwidth, autoLevelMeasurementInterval, autoLevelReferenceLevel)
            specAn.Initiate("", "")
            specAn.Idpd.Results.FetchEqualizerCoefficients("", timeout, equalizerCoefficients)
        End Sub

        Private Sub DisplayResults()
            Console.WriteLine("x0 : {0}", equalizerCoefficients.PrecisionTiming.TimeOffset.TotalSeconds)
            Console.WriteLine("dx : {0}", equalizerCoefficients.PrecisionTiming.SampleInterval.TotalSeconds)
            Dim coefficientArray As ComplexSingle() = equalizerCoefficients.GetRawData()
            Console.WriteLine("Equalizer Coefficients: ")
            For i As Integer = 0 To coefficientArray.Length - 1
                Console.WriteLine(coefficientArray(i))
            Next
        End Sub

        Private Sub CloseSessions()
            Try
                If specAn IsNot Nothing Then
                    specAn.Dispose()
                    specAn = Nothing
                End If

                If instrSession IsNot Nothing Then
                    instrSession.Close()
                    instrSession = Nothing
                End If

                If rfsgSession IsNot Nothing Then
                    rfsgSession.Abort()
                    NIRfsgPlayback.ClearWaveform(instrumentHandle, waveformName)
                    rfsgSession.Close()
                    rfsgSession = Nothing

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
