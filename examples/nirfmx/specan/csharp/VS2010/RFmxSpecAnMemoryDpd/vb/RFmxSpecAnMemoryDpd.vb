' Steps:
'1. Open RFSG session.
'2. Configure RFSG frequency reference.
'3. Configure marker0 to be generated from RFSG on the specified output terminal.
'4. Configure frequency and power level of RF output signal.
'5. Set RFSG External Gain. #4 and #5 ensure that the average power of the signal at the input of the DUT
'   matches the user configured DUT Average Input Power.
'6. Read waveform from file and download Waveform from file to RFSG.
'   Set Waveform Runtime Scaling to the desired Pre-filter Gain.
'   Read waveform sample rate, multiply by 0.8 and set the result to the signal bandwidth.
'   Write script to generate the waveform specified in the script. This script is programmed
'   to generate waveform continuously, with marker0 aligned to sample index 0.
'7. Initiate generation.
'8. Open RFmx session.
'9. Configure frequency reference of the analyser.
'10. Configure Selected Ports.
'11. Configure trigger to use as reference for signal acquisition.
'12. Configure center frequency and external attenuation.
'13. Select DPD measurement, configure the reference waveform
'    and power of this signal at the input of the DUT.
'14. Select and configure the Memory polynomial or Generalized memory polynomial model
'    to estimate the predistotor.
'15. Set the measurement sample rate and the measurement interval to use for analysis.
'16. Enable iterative DPD.
'17. Perform Auto Level to compute an approximate reference level to use by the analyser.
'18. Configure the Memory models Correction type.
'19. Initiates DPD measurement and then applies the DPD polynomial to
'    remove the effects of memory and nonlinearity introduced by the DUT.
'20. Set the previous iteration polynomial, in case DPD is measured iteratively.
'21. Fetch DPD Polynomial.
'22. Abort RFSG generation and write a new waveform that is predistorted by applying LUT.
'    Set Waveform Runtime Scaling to the negative of the desired Pre-filter Gain.
'    Set the sample rate computed from Apply Digital Predistortion.
'    Set the final PAPR to the sum of the actual PAPR and the Power Offset as computed
'    by Apply Digital Predistortion.
'    Set the Signal Bandwidth.
'    Initiate RFSG generation using the script that was selected earlier.
'23. Perform Auto Level to compute an approximate reference level while generating the predistorted waveform.
'24. Select and configure AMPM measurement in RFmx after DPD measurement is complete.
'    AMPM measurement is used to inspect the measure the AM-AM and AM-PM response of the DUT.
'25. Initiate and fetch AMPM results.
'26. Close RFmx session.
'27. Close RFSG session.
'It is recommended to clear the waveform before closing RFSG session.


Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.SpecAnMX
Imports NationalInstruments.DataInfrastructure
Imports NationalInstruments.ModularInstruments.NIRfsg
Imports NationalInstruments.ModularInstruments.NIRfsgPlayback
Imports System.IO

Public Class RFmxSpecAnMemoryDpd
   Private instrSession As RFmxInstrMX
   Private specAn As RFmxSpecAnMX
   Private rfsgSession As NIRfsg
   Private instrumentHandle As IntPtr

   Private rfsaResourceName As String = "RFSA"
   Private rfsgResourceName As String = "RFSG"
   Private enableTrigger As Boolean = True
   Private frequencyReferenceSource As String = RFmxInstrMXConstants.OnboardClock
   Private frequencyReferenceFrequency As Double = 10000000.0
   ' Hz

   Private triggerType As RFmxSpecAnMXTriggerType = RFmxSpecAnMXTriggerType.DigitalEdge
   Private triggerDelay As Double = 0
   ' seconds
   Private selectedPorts As String = ""
   Private centerFrequency As Double = 1000000000.0
   ' Hz
   Private referenceLevel As Double = 0.0
   ' dBm
   Private rfsaExternalAttenuation As Double = 0.0
   ' dB
   Private rfsgExternalAttenuation As Double = 0.0
   ' dB
   Private preFilterGain As Double = -4.0
   ' dB
   Private runtimeScaling As Double
   Private papr As Double = 0.0
   Private signalBandwidth As Double = 20000000.0
   ' Hz
   Private autoLevelMeasurementInterval As Double = 0.0001
   ' seconds
   Private autoLevelReferenceLevel As Double
   Private dutAverageInputPower As Double = -20
   ' dBm
   Private sampleRateMode As RFmxSpecAnMXDpdMeasurementSampleRateMode = RFmxSpecAnMXDpdMeasurementSampleRateMode.ReferenceWaveform
   Private sampleRate As Double = 120000000.0
   ' S/s
   Private measurementInterval As Double = 0.0001
   ' seconds

   Private thresholdLevel As Double = -20
   ' dB or dBm
   Private referenceWaveformComplexSingle As ComplexWaveform(Of ComplexSingle), waveformWithDpdComplexSingle As ComplexWaveform(Of ComplexSingle)

   Private referenceWaveformFile As String = "LTE20MHz Waveform (Two Subframes).tdms"
   Private idleDurationPresent As RFmxSpecAnMXDpdReferenceWaveformIdleDurationPresent = RFmxSpecAnMXDpdReferenceWaveformIdleDurationPresent.[False]
   Private signalType As RFmxSpecAnMXDpdSignalType = RFmxSpecAnMXDpdSignalType.Modulated
   Private timeout As Double = 10
   ' seconds
   Private numberOfIterations As Integer = 3
   Private memoryModelCorrectionType As RFmxSpecAnMXDpdApplyDpdMemoryModelCorrectionType = RFmxSpecAnMXDpdApplyDpdMemoryModelCorrectionType.MagnitudeAndPhase
   Private iterativeDpdEnabled As RFmxSpecAnMXDpdIterativeDpdEnabled = RFmxSpecAnMXDpdIterativeDpdEnabled.[False]
   Private referenceClockSource As RfsgFrequencyReferenceSource = RfsgFrequencyReferenceSource.OnboardClock
   Private dpdModel As RFmxSpecAnMXDpdModel = RFmxSpecAnMXDpdModel.MemoryPolynomial
   Private referenceClockRate As Double = 10000000.0
   Private scriptName As String = "DPDScript"
   Private waveformName As String = "Wfm"
   Private referencePowerType As RFmxSpecAnMXAmpmReferencePowerType = RFmxSpecAnMXAmpmReferencePowerType.Input
   Private rfsgIqRate As Double
   Private markerNumber As Integer = 0
   Private waveformScript As String
   Private memoryPolynomialOrder As Integer = 3, memoryPolynomialDepth As Integer = 2
   Private memoryPolynomialLeadOrder As Integer = 2, memoryPolynomialLagOrder As Integer = 2, memoryPolynomialLeadMemoryDepth As Integer = 2, memoryPolynomialLagMemoryDepth As Integer = 2, memoryPolynomialMaxLead As Integer = 2, memoryPolynomialMaxLag As Integer = 2
   Private meanLinearGain As Double, onedBCompressionPoint As Double, meanRmsEvm As Double, gainErrorRange As Double, phaseErrorRange As Double, meanPhaseError As Double,
      amToAMResidual As Double, amToPMResidual As Double, powerOffset As Double

   Private referencePowersAMToAM As Single()
   Private measuredAMToAM As Single()
   Private curveFitAMToAM As Single()
   Private referencePowersAMToPM As Single()
   Private measuredAMToPM As Single()
   Private curveFitAMToPM As Single()
   Private dpdPolynomial As ComplexSingle()

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
      rfsgIqRate = 1 / referenceWaveformComplexSingle.PrecisionTiming.SampleInterval.TotalSeconds
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
      waveformScript = [String].Format("script {0}{1}repeat forever{1}generate {2} marker{3}(0){1}end repeat{1}end script", scriptName, Environment.NewLine, waveformName, markerNumber)
      rfsgSession.RF.ExternalGain = -rfsgExternalAttenuation
      instrumentHandle = rfsgSession.GetInstrumentHandle().DangerousGetHandle()
      NIRfsgPlayback.ReadAndDownloadWaveformFromFile(instrumentHandle, referenceWaveformFile, waveformName)
      runtimeScaling = preFilterGain
      NIRfsgPlayback.StoreWaveformRuntimeScaling(instrumentHandle, waveformName, runtimeScaling)
      NIRfsgPlayback.RetrieveWaveformSampleRate(instrumentHandle, waveformName, sampleRate)
      NIRfsgPlayback.StoreWaveformSignalBandwidth(instrumentHandle, waveformName, 0.8 * sampleRate)
      NIRfsgPlayback.SetScriptToGenerateSingleRfsg(instrumentHandle, waveformScript)
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
      specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Dpd, True)
      specAn.Dpd.Configuration.ConfigureReferenceWaveform("", referenceWaveformComplexSingle, idleDurationPresent, signalType)
      specAn.Dpd.Configuration.ConfigureDutAverageInputPower("", dutAverageInputPower)
      specAn.Dpd.Configuration.ConfigureDpdModel("", dpdModel)
      specAn.Dpd.Configuration.ConfigureMemoryPolynomial("", memoryPolynomialOrder, memoryPolynomialDepth)
      specAn.Dpd.Configuration.ConfigureGeneralizedMemoryPolynomialCrossTerms("", memoryPolynomialLeadOrder, memoryPolynomialLagOrder, memoryPolynomialLeadMemoryDepth, memoryPolynomialLagMemoryDepth, memoryPolynomialMaxLead,
         memoryPolynomialMaxLag)
      specAn.Dpd.Configuration.ConfigureMeasurementSampleRate("", sampleRateMode, sampleRate)
      specAn.Dpd.Configuration.ConfigureMeasurementInterval("", measurementInterval)
      specAn.Dpd.Configuration.ConfigureIterativeDpdEnabled("", iterativeDpdEnabled)
      specAn.AutoLevel("", signalBandwidth, autoLevelMeasurementInterval, autoLevelReferenceLevel)
      specAn.Dpd.ApplyDpd.ConfigureMemoryModelCorrectionType("", memoryModelCorrectionType)
   End Sub

   Private Sub RetrieveResults()

      If iterativeDpdEnabled = RFmxSpecAnMXDpdIterativeDpdEnabled.[False] Then
         numberOfIterations = 1
      Else
         numberOfIterations = 3
      End If
      For i As Integer = 0 To numberOfIterations - 1
         specAn.Dpd.Configuration.ConfigurePreviousDpdPolynomial("", dpdPolynomial)
         specAn.Initiate("", "")
         specAn.Dpd.ApplyDpd.ApplyDigitalPredistortion("", referenceWaveformComplexSingle, RFmxSpecAnMXDpdApplyDpdIdleDurationPresent.[False], timeout, waveformWithDpdComplexSingle, papr,
            powerOffset)
         rfsgSession.Abort()
         NIRfsgPlayback.ClearWaveform(instrumentHandle, waveformName)
         rfsgIqRate = 1 / waveformWithDpdComplexSingle.PrecisionTiming.SampleInterval.TotalSeconds
         rfsgSession.Arb.WriteWaveform(waveformName, waveformWithDpdComplexSingle)
         NIRfsgPlayback.StoreWaveformRuntimeScaling(instrumentHandle, waveformName, runtimeScaling)
         NIRfsgPlayback.StoreWaveformSampleRate(instrumentHandle, waveformName, rfsgIqRate)
         NIRfsgPlayback.StoreWaveformPapr(instrumentHandle, waveformName, (papr + powerOffset))
         NIRfsgPlayback.StoreWaveformSignalBandwidth(instrumentHandle, waveformName, 0.8 * rfsgIqRate)
         NIRfsgPlayback.SetScriptToGenerateSingleRfsg(instrumentHandle, waveformScript)
         rfsgSession.Initiate()

         specAn.Dpd.Results.FetchDpdPolynomial("", timeout, dpdPolynomial)
      Next

      specAn.AutoLevel("", signalBandwidth, autoLevelMeasurementInterval, autoLevelReferenceLevel)
      specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Ampm, True)
      specAn.Ampm.Configuration.ConfigureMeasurementSampleRate("", RFmxSpecAnMXAmpmMeasurementSampleRateMode.ReferenceWaveform, sampleRate)
      specAn.Ampm.Configuration.ConfigureMeasurementInterval("", measurementInterval)
      specAn.Ampm.Configuration.ConfigureReferenceWaveform("", referenceWaveformComplexSingle, RFmxSpecAnMXAmpmReferenceWaveformIdleDurationPresent.[False], RFmxSpecAnMXAmpmSignalType.Modulated)
      specAn.Ampm.Configuration.ConfigureDutAverageInputPower("", dutAverageInputPower)
      specAn.Ampm.Configuration.ConfigureThreshold("", RFmxSpecAnMXAmpmThresholdEnabled.[True], thresholdLevel, RFmxSpecAnMXAmpmThresholdType.Relative)
      specAn.Ampm.Configuration.ConfigureReferencePowerType("", referencePowerType)

      specAn.Initiate("", "")

      specAn.Ampm.Results.FetchDutCharacteristics("", timeout, meanLinearGain, onedBCompressionPoint, meanRmsEvm)
      specAn.Ampm.Results.FetchError("", timeout, gainErrorRange, phaseErrorRange, meanPhaseError)
      specAn.Ampm.Results.FetchCurveFitResidual("", timeout, amToAMResidual, amToPMResidual)
      specAn.Ampm.Results.FetchAMToAMTrace("", timeout, referencePowersAMToAM, measuredAMToAM, curveFitAMToAM)
      specAn.Ampm.Results.FetchAMToPMTrace("", timeout, referencePowersAMToPM, measuredAMToPM, curveFitAMToPM)

      rfsgSession.Abort()
      NIRfsgPlayback.ClearWaveform(instrumentHandle, waveformName)
   End Sub

   Private Sub DisplayResults()
      Console.WriteLine("-----------------Measurement-----------------" & vbLf)
      Console.WriteLine("Mean Linear Gain (dB)            {0}", meanLinearGain)
      Console.WriteLine("Mean Phase Error (deg)           {0}", meanPhaseError)
      Console.WriteLine("Mean RMS EVM (%)                 {0}", meanRmsEvm)
      Console.WriteLine("AM to AM Residual (dB)           {0}", amToAMResidual)
      Console.WriteLine("AM to PM Residual (deg)          {0}", amToPMResidual)
      Console.WriteLine("Gain Error Range (dB)            {0}", gainErrorRange)
      Console.WriteLine("Phase Error Range (deg)          {0}", phaseErrorRange)
      Console.WriteLine("1 dB Compression Point (dBm)     {0}", onedBCompressionPoint)
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
