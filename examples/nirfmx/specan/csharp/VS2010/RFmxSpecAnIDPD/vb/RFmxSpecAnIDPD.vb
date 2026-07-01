' Steps:
'1. Open RFSG session.
'2. Configure RFSG frequency reference, generation Mode to Script, Power Level Type and Upconverter Frequency Offset Mode
'3. Configure marker0 to be generated from RFSG on the specified output terminal.
'4. Configure frequency and power level of RF output signal.
'5. Set RFSG External Gain. #4 and #5 ensure that the average power of the signal at the input of the DUT
'   matches the user configured DUT Average Input Power.
'6. Read waveform from file and download Waveform from file to RFSG.
'   Set Waveform Runtime Scaling to the desired Pre-filter Gain.
'   Read waveform sample rate, multiply by 0.8 and set the result to the signal bandwidth.
'   Write script to generate the waveform specified in the script. This script is programmed
'to generate waveform continuously, with marker0 aligned to sample index 0.
'7. Initiate generation.
'8. Open RFmx session.
'9. Configure frequency reference of the analyser.
'10. Configure Selected Ports.
'11. Configure trigger to use as reference for signal acquisition.
'12. Configure center frequency and external attenuation.
'13. Select IDPD measurement, configure the reference waveform.
'14. Configure the equalizer coefficients for 'Hold' mode of Equalizer.
'15. Set power of configured reference signal at the input of the DUT.
'      Set the measurement sample rate and the measurement interval to use for analysis.
'      Set equalizer mode.
'      Configure averaging and EVM Enabled with EVM unit.
'      Set start and stop for impairment estimation and synchronization estimation.
'      Configure gain expansion (dB) and power linearity tradeoff (%).
'16. Perform Auto Level to compute an approximate reference level to use by the analyzer and adjust
'      the reference level to account for PAPR changes after applying DPD.
'17. Configure predistorted waveform obtained from previous iteration.  
'18. Initiates IDPD measurement.
'19. Fetch predistorted waveform and scale from -1 to 1 as RFSG power level type is peak power and
'      fetch RMS EVM.
'20. Abort RFSG generation and write a new Predistorted Waveform.
'       Set Waveform Runtime Scaling to the desired Pre-filter Gain.
'       Set the sample rate computed from Predistorted Waveform.
'       Set the final PAPR to the sum of the actual PAPR and the Power Offset as computed from
'       Predistorted Waveform.
'       Set the Signal Bandwidth.
'       Initiate RFSG generation using the script that was selected earlier
'21. Configure appropriate trigger delay for AMPM measurement based on burst location in the Waveform.
'22. Perform Auto Level to compute an approximate reference level while generating the predistorted waveform.
'23. Select and configure AMPM measurement in RFmx after IDPD measurement is complete.
'      AMPM measurement is used to measure the AM-AM and AM-PM response of the DUT.
'24. Initiate and fetch AMPM results.
'25. Close RFmx session.
'26. Close RFSG session.
'It is recommended to clear the waveform before closing RFSG session.


Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.SpecAnMX
Imports NationalInstruments.ModularInstruments.NIRfsg
Imports NationalInstruments.ModularInstruments.NIRfsgPlayback


Namespace NationalInstruments.Examples.RFmxSpecAnIdpd
    Public Class RFmxSpecAnIdpd
        Private instrSession As RFmxInstrMX
        Private specAn As RFmxSpecAnMX
        Private rfsgSession As NIRfsg

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
        Private numberOfIterations As Integer = 10
        Private gainExpansion As Double = 3.0
        ' dB 
        Private powerLinearityTradeoff As Double = 50.0
        ' % 
        Private equalizerMode As RFmxSpecAnMXIdpdEqualizerMode = RFmxSpecAnMXIdpdEqualizerMode.Off
        Private equalizerCoefficients As ComplexSingle()
        Private startTime As Double = 0.0
        ' seconds 
        Private stopTime As Double = 0.0001
        ' seconds 
        Private idpdEvmEnabled As RFmxSpecAnMXIdpdEvmEnabled = RFmxSpecAnMXIdpdEvmEnabled.[True]
        Private evmUnit As RFmxSpecAnMXIdpdEvmUnit = RFmxSpecAnMXIdpdEvmUnit.dB
        Private targetGain As Double = 20
        ' dB 
        Private gain As Double = 0

        Private signalBandwidth As Double = 20000000.0
        ' Hz 
        Private autoLevelMeasurementInterval As Double = 0.0001
        ' seconds 
        Private autoLevelReferenceLevel As Double

        Private ampmIdleDurationPresent As RFmxSpecAnMXAmpmReferenceWaveformIdleDurationPresent = RFmxSpecAnMXAmpmReferenceWaveformIdleDurationPresent.[False]
        Private ampmSignalType As RFmxSpecAnMXAmpmSignalType = RFmxSpecAnMXAmpmSignalType.Modulated
        Private referencePowerType As RFmxSpecAnMXAmpmReferencePowerType = RFmxSpecAnMXAmpmReferencePowerType.Input
        Private thresholdEnabled As RFmxSpecAnMXAmpmThresholdEnabled = RFmxSpecAnMXAmpmThresholdEnabled.[True]

        Private timeout As Double = 10
        ' seconds 

        Private scriptName As String = "IDPDScript"
        Private waveformName As String = "Wfm"
        Private rfsgIqRate As Double
        Private markerNumber As Integer = 0
        Private waveformScript As String

        Private idpdMeanRmsEvm As Double()
        Private predistortedWaveform As ComplexWaveform(Of ComplexSingle), normalizePredistortedWaveform As ComplexWaveform(Of ComplexSingle)
        Private referenceWaveformComplexSingle As ComplexWaveform(Of ComplexSingle)

        Private meanLinearGain As Double, onedBCompressionPoint As Double, meanRmsEvm As Double, gainErrorRange As Double, phaseErrorRange As Double, meanPhaseError As Double,
        amToAMResidual As Double, amToPMResidual As Double, powerOffset As Double

        Private referencePowersAMToAM As Single()
        Private measuredAMToAM As Single()
        Private curveFitAMToAM As Single()
        Private referencePowersAMToPM As Single()
        Private measuredAMToPM As Single()
        Private curveFitAMToPM As Single()

        Friend Sub Run()
            Try
                ReadWaveformFromTdmsFile()
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

        Private Sub OpenSession()
            instrSession = New RFmxInstrMX(rfsaResourceName, "")
            specAn = instrSession.GetSpecAnSignalConfiguration()
        End Sub

        Private Sub ConfigureRfsg()
            rfsgSession = New NIRfsg(rfsgResourceName, False, True)
            rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.Script
            rfsgSession.RF.PowerLevelType = RfsgRFPowerLevelType.PeakPower
            rfsgSession.FrequencyReference.Configure(referenceClockSource, referenceClockRate)
            rfsgSession.DeviceEvents.MarkerEvents(markerNumber).ExportedOutputTerminal = RfsgMarkerEventExportedOutputTerminal.PxiTriggerLine0
            rfsgSession.RF.Configure(centerFrequency, dutAverageInputPower)
            rfsgSession.RF.Upconverter.FrequencyOffsetMode = UpconverterFrequencyOffsetMode.Auto
            rfsgSession.RF.ExternalGain = -rfsgExternalAttenuation
            rfsgSession.Arb.ReadAndDownloadWaveformFromFileTdms(waveformName, referenceWaveformFile, 0)
            rfsgIqRate = rfsgSession.Arb.Waveforms(waveformName).IQRate
            runtimeScaling = preFilterGain
            rfsgSession.Arb.PreFilterGain = runtimeScaling
            rfsgSession.Arb.SignalBandwidth = 0.8 * rfsgIqRate
            waveformScript = [String].Format("script {0}{1}repeat forever{1}generate {2} marker{3}(0){1}end repeat{1}end script", scriptName, Environment.NewLine, waveformName, markerNumber)
            rfsgSession.Arb.Scripting.WriteScript(waveformScript)
            rfsgSession.Initiate()
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
            specAn.Idpd.Configuration.ConfigureEqualizerCoefficients("", 0.0, 0.0, equalizerCoefficients)
            specAn.Idpd.Configuration.SetDutAverageInputPower("", dutAverageInputPower)
            specAn.Idpd.Configuration.SetMeasurementSampleRateMode("", sampleRateMode)
            specAn.Idpd.Configuration.SetMeasurementSampleRate("", sampleRate)
            specAn.Idpd.Configuration.SetEqualizerMode("", equalizerMode)
            specAn.Idpd.Configuration.SetEvmEnabled("", idpdEvmEnabled)
            specAn.Idpd.Configuration.SetEvmUnit("", evmUnit)
            specAn.Idpd.Configuration.SetGainExpansion("", gainExpansion)
            specAn.Idpd.Configuration.SetPowerLinearityTradeoff("", powerLinearityTradeoff)
            specAn.Idpd.Configuration.SetImpairmentEstimationStart("", startTime)
            specAn.Idpd.Configuration.SetSynchronizationEstimationStart("", startTime)
            specAn.Idpd.Configuration.SetImpairmentEstimationStop("", stopTime)
            specAn.Idpd.Configuration.SetSynchronizationEstimationStop("", stopTime)

            specAn.AutoLevel("", signalBandwidth, autoLevelMeasurementInterval, autoLevelReferenceLevel)
        End Sub

        Private Sub RetrieveResults()
            idpdMeanRmsEvm = New Double(numberOfIterations - 1) {}
            For i As Integer = 0 To numberOfIterations - 1
                specAn.Idpd.Configuration.ConfigurePredistortedWaveform("", predistortedWaveform, targetGain)
                specAn.Initiate("", "")
                specAn.Idpd.Results.FetchPredistortedWaveform("", timeout, predistortedWaveform, papr, powerOffset, gain)
                specAn.Idpd.Results.GetMeanRmsEvm("", idpdMeanRmsEvm(i))

                ' Storing the predistorted waveform again to normalize it 

                Dim x0 As Double = predistortedWaveform.PrecisionTiming.TimeOffset.TotalSeconds
                Dim dx As Double = predistortedWaveform.PrecisionTiming.SampleInterval.TotalSeconds
                normalizePredistortedWaveform = New ComplexWaveform(Of ComplexSingle)(0)
                normalizePredistortedWaveform.Append(predistortedWaveform.GetRawData())
                Dim precisionTiming As PrecisionWaveformTiming = PrecisionWaveformTiming.CreateWithRegularInterval(New PrecisionTimeSpan(dx), New PrecisionTimeSpan(x0))
                normalizePredistortedWaveform.PrecisionTiming = precisionTiming

                'Normalization

                Dim predistortedWaveformRawData As ComplexSingle() = normalizePredistortedWaveform.GetRawData()
                Dim maxElement As Single = predistortedWaveformRawData(0).Magnitude

                For k As Integer = 1 To predistortedWaveformRawData.Length - 1
                    If predistortedWaveformRawData(k).Magnitude > maxElement Then
                        maxElement = predistortedWaveformRawData(k).Magnitude
                    End If
                Next

                For j As Integer = 0 To predistortedWaveformRawData.Length - 1
                    predistortedWaveformRawData(j) /= New ComplexSingle(maxElement, 0)
                Next

                normalizePredistortedWaveform = New ComplexWaveform(Of ComplexSingle)(0)
                normalizePredistortedWaveform.Append(predistortedWaveformRawData)
                normalizePredistortedWaveform.PrecisionTiming = precisionTiming

                targetGain = gain

                rfsgSession.Abort()
                rfsgSession.Arb.ClearWaveform(waveformName)
                rfsgIqRate = 1 / normalizePredistortedWaveform.PrecisionTiming.SampleInterval.TotalSeconds
                rfsgSession.Arb.WriteWaveform(waveformName, normalizePredistortedWaveform)
                rfsgSession.Arb.PreFilterGain = runtimeScaling
                rfsgSession.Arb.IQRate = rfsgIqRate
                rfsgSession.Arb.Waveforms(waveformName).Papr = (papr + powerOffset)
                rfsgSession.Arb.SignalBandwidth = 0.8 * rfsgIqRate
                rfsgSession.Arb.Scripting.WriteScript(waveformScript)
                rfsgSession.Initiate()
            Next

            specAn.SetTriggerDelay("", triggerDelay)
            specAn.AutoLevel("", signalBandwidth, autoLevelMeasurementInterval, autoLevelReferenceLevel)
            specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Ampm, True)
            specAn.Ampm.Configuration.ConfigureReferenceWaveform("", referenceWaveformComplexSingle, ampmIdleDurationPresent, ampmSignalType)
            specAn.Ampm.Configuration.SetDutAverageInputPower("", dutAverageInputPower)
            specAn.Ampm.Configuration.SetMeasurementSampleRateMode("", RFmxSpecAnMXAmpmMeasurementSampleRateMode.ReferenceWaveform)
            specAn.Ampm.Configuration.SetMeasurementSampleRate("", sampleRate)
            specAn.Ampm.Configuration.SetMeasurementInterval("", stopTime - startTime)
            specAn.Ampm.Configuration.SetReferencePowerType("", referencePowerType)
            specAn.Ampm.Configuration.SetThresholdEnabled("", thresholdEnabled)

            specAn.Initiate("", "")

            specAn.Ampm.Results.FetchDutCharacteristics("", timeout, meanLinearGain, onedBCompressionPoint, meanRmsEvm)
            specAn.Ampm.Results.FetchError("", timeout, gainErrorRange, phaseErrorRange, meanPhaseError)
            specAn.Ampm.Results.FetchCurveFitResidual("", timeout, amToAMResidual, amToPMResidual)
            specAn.Ampm.Results.FetchAMToAMTrace("", timeout, referencePowersAMToAM, measuredAMToAM, curveFitAMToAM)
            specAn.Ampm.Results.FetchAMToPMTrace("", timeout, referencePowersAMToPM, measuredAMToPM, curveFitAMToPM)

            rfsgSession.Abort()
            rfsgSession.Arb.ClearWaveform(waveformName)
        End Sub

        Private Sub DisplayResults()
            Console.WriteLine("-----------------AMPM Measurement-----------------" & vbLf)
            Console.WriteLine("Mean Linear Gain (dB)            {0}", meanLinearGain)
            Console.WriteLine("Mean Phase Error (deg)           {0}", meanPhaseError)
            Console.WriteLine("Mean RMS EVM (%)                 {0}", meanRmsEvm)
            Console.WriteLine("AM to AM Residual (dB)           {0}", amToAMResidual)
            Console.WriteLine("AM to PM Residual (deg)          {0}", amToPMResidual)
            Console.WriteLine("Gain Error Range (dB)            {0}", gainErrorRange)
            Console.WriteLine("Phase Error Range (deg)          {0}", phaseErrorRange)
            Console.WriteLine("1 dB Compression Point (dBm)     {0}", onedBCompressionPoint)
            Console.WriteLine(vbLf & "-----------------IDPD Measurement-----------------" & vbLf)
            Console.WriteLine("IDPD RMS EVM Mean (% or dB) per iteration:")
            For i As Integer = 0 To idpdMeanRmsEvm.Length - 1
                Console.WriteLine(idpdMeanRmsEvm(i))
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
