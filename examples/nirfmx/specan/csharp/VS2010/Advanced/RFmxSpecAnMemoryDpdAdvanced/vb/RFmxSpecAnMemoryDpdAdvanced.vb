' Steps:
'1. Open RFSG session.
'2. Configure RFSG frequency reference, generation mode to Script.
'3. Configure marker0 to be generated from RFSG on the specified output terminal.
'4. Configure frequency and power level of RF output signal.
'5. Configure power level type.
'6. Set RFSG External Gain. #4 and #5 ensure that the average power of the signal at the input of the DUT
'   matches the user configured DUT Average Input Power.
'7. a. Read waveform from.
'   b. Write input waveform on RFSG device.
'      Set the waveform sample rate.
'      Store waveform PAPR.
'      Set Waveform Runtime Scaling to the desired Pre-filter Gain.
'      Read waveform sample rate, multiply by 0.8 and set the result to the signal bandwidth.
'      Write script to generate the waveform specified in the script. This script is programmed
'      to generate waveform continuously, with marker0 aligned to sample index 0.
'8. Initiate generation.
'9. Open RFmx session.
'10. Configure frequency reference of the analyser.
'11. Configure Selected Ports.
'12. Configure trigger to use as reference for signal acquisition.
'13. Configure center frequency and external attenuation.
'14. Select DPD measurement.
'15. Configure pre-DPD CFR.
'16. Configure waveform settings for pre-DPD CFR with filtering.
'17. Apply pre-DPD CFR.
'18. configure the reference waveform.
'19. Configure power of the signal at the input of the DUT. Select and configure the Memory
'    polynomial or Generalized memory polynomial model and its parameters to estimate the predistotor.
'20. Set the measurement sample rate and the measurement interval to use for analysis.
'21. Enable iterative DPD.
'22. Configure DPD NMSE Enabled.
'23. Configure the Memory models Correction type.
'24. Configure apply DPD CFR settings before calling RFmx initiate.
'    This is because these settings are used by measurement when performing iterative DPD.
'25. Perform Auto Level to compute an approximate reference level to use by the analyser.
'26. Set the previous iteration polynomial, in case DPD is measured iteratively.
'27. Initiates DPD measurement and then configure Apply Digital Predistortion to remove the
'    effects of memory and nonlinearity introduced by the DUT.
'28. a. Fetch DPD Polynomial.
'    b. Fetch NMSE (dB).
'29. Abort RFSG generation and write a new waveform that is predistorted by applying momory polynomial coefficients.
'    Set Waveform Runtime Scaling to desired Pre-filter Gain.
'    Set the sample rate computed from Apply Digital Predistortion.
'    Set the final PAPR to the sum of the actual PAPR and the Power Offset as computed
'    by Apply Digital Predistortion.
'    Set the Signal Bandwidth.
'    Initiate RFSG generation Using the script that was selected earlier.
'30. Perform Auto Level to compute an approximate reference level while generating the predistorted waveform.
'31. Select and configure AMPM measurement in RFmx after DPD measurement is complete.
'    AMPM measurement is used to inspect the measure the AM-AM and AM-PM response of the DUT.
'32. Initiate and fetch AMPM results.
'33. Close RFmx session.
'34. Close RFSG session.
'It is recommended to clear the waveform before closing RFSG session.*/

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.SpecAnMX
Imports NationalInstruments.DataInfrastructure
Imports NationalInstruments.ModularInstruments.NIRfsg
Imports NationalInstruments.ModularInstruments.NIRfsgPlayback
Imports System.IO

Public Class RFmxSpecAnMemoryDpdAdvanced
   Private instrSession As RFmxInstrMX
   Private specAn As RFmxSpecAnMX
   Private rfsgSession As NIRfsg

   Private rfsaResourceName As String = "RFSA"
   Private rfsgResourceName As String = "RFSG"
   Private enableTrigger As Boolean = True
   Private digitalEdgeSource As String = RFmxInstrMXConstants.PxiTriggerLine0
   Private digitalEdgeTriggerEdge As RFmxSpecAnMXDigitalEdgeTriggerEdge = RFmxSpecAnMXDigitalEdgeTriggerEdge.Rising
   Private iqPowerEdgeTriggerSlope As RFmxSpecAnMXIQPowerEdgeTriggerSlope = RFmxSpecAnMXIQPowerEdgeTriggerSlope.Rising
   Private triggerMinimumQuietTimeMode As RFmxSpecAnMXTriggerMinimumQuietTimeMode =
      RFmxSpecAnMXTriggerMinimumQuietTimeMode.Manual
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
   Private powerLevelType As RfsgRFPowerLevelType = RfsgRFPowerLevelType.PeakPower
   Private sampleRateMode As RFmxSpecAnMXDpdMeasurementSampleRateMode =
      RFmxSpecAnMXDpdMeasurementSampleRateMode.ReferenceWaveform
   Private sampleRate As Double = 120000000.0
   ' S/s
   Private measurementInterval As Double = 0.0001
   ' seconds

   Private thresholdLevel As Double = -20
   ' dB or dBm
   Private referenceWaveformComplexSingle As ComplexWaveform(Of ComplexSingle)
   Private waveformWithDpdComplexSingle As ComplexWaveform(Of ComplexSingle)
   Private preDpdWaveformWithComplexSingle As ComplexWaveform(Of ComplexSingle)

   Private referenceWaveformFile As String = "LTE20MHz Waveform (Two Subframes).tdms"
   Private idleDurationPresent As RFmxSpecAnMXDpdReferenceWaveformIdleDurationPresent =
     RFmxSpecAnMXDpdReferenceWaveformIdleDurationPresent.[False]
   Private dpdApplyDpdIdleDurationPresent As RFmxSpecAnMXDpdApplyDpdIdleDurationPresent =
      RFmxSpecAnMXDpdApplyDpdIdleDurationPresent.[False]
   Private signalType As RFmxSpecAnMXDpdSignalType = RFmxSpecAnMXDpdSignalType.Modulated
   Private timeout As Double = 10
   ' seconds
   Private numberOfIterations As Integer = 3
   Private memoryModelCorrectionType As RFmxSpecAnMXDpdApplyDpdMemoryModelCorrectionType =
      RFmxSpecAnMXDpdApplyDpdMemoryModelCorrectionType.MagnitudeAndPhase
   Private iterativeDpdEnabled As RFmxSpecAnMXDpdIterativeDpdEnabled = RFmxSpecAnMXDpdIterativeDpdEnabled.[False]
   Private referenceClockSource As RfsgFrequencyReferenceSource = RfsgFrequencyReferenceSource.OnboardClock
   Private dpdModel As RFmxSpecAnMXDpdModel = RFmxSpecAnMXDpdModel.MemoryPolynomial
   Private referenceClockRate As Double = 10000000.0
   Private scriptName As String = "DPDScript"
   Private waveformName As String = "Wfm"
   Private rfsgIqRate As Double
   Private markerNumber As Integer = 0
   Private waveformScript As String
   Private memoryPolynomialOrder As Integer = 3, memoryPolynomialDepth As Integer = 2
   Private crossTermsLeadOrder As Integer = 2, crossTermsLagOrder As Integer = 2,
      crossTermsLeadMemoryDepth As Integer = 2, crossTermsLagMemoryDepth As Integer = 2,
      crossTermsMaximumLead As Integer = 2, crossTermsMaximumLag As Integer = 2
   Private nmseEnabled As RFmxSpecAnMXDpdNmseEnabled = RFmxSpecAnMXDpdNmseEnabled.False
   Private referencePowerType As RFmxSpecAnMXAmpmReferencePowerType = RFmxSpecAnMXAmpmReferencePowerType.Input

   Private preDpdCfrEnabled As RFmxSpecAnMXDpdPreDpdCfrEnabled = RFmxSpecAnMXDpdPreDpdCfrEnabled.False
   Private preDpdCfrMethod As RFmxSpecAnMXDpdPreDpdCfrMethod = RFmxSpecAnMXDpdPreDpdCfrMethod.Clipping
   Private preDpdCfrMaximumIterations As Integer = 10
   Private preDpdCfrTargetPapr As Double = 8.0
   '(dB)
   Private preDpdCfrWindowType As RFmxSpecAnMXDpdPreDpdCfrWindowType = RFmxSpecAnMXDpdPreDpdCfrWindowType.KaiserBessel
   Private preDpdCfrWindowLength As Integer = 10
   Private preDpdCfrShapingFactor As Double = 5.0
   Private preDpdCfrShapingThreshold As Double = -5.0
   '(dB)
   Private preDpdCfrFilterEnabled As RFmxSpecAnMXDpdPreDpdCfrFilterEnabled = RFmxSpecAnMXDpdPreDpdCfrFilterEnabled.False
   Const NumberOfCarriers As Integer = 1
   Private preDpdCarrierOffsets As Double() = New Double(NumberOfCarriers - 1) {0.0}
   ' (Hz)
   Private preDpdCarrierBandwidths As Double() = New Double(NumberOfCarriers - 1) {20000000.0}
   ' (Hz)

   Private applyDpdCfrEnabled As RFmxSpecAnMXDpdApplyDpdCfrEnabled = RFmxSpecAnMXDpdApplyDpdCfrEnabled.False
   Private applyDpdCfrMethod As RFmxSpecAnMXDpdApplyDpdCfrMethod = RFmxSpecAnMXDpdApplyDpdCfrMethod.Clipping
   Private applyDpdCfrMaximumIterations As Integer = 10
   Private applyDpdCfrTargetPaprType As RFmxSpecAnMXDpdApplyDpdCfrTargetPaprType = RFmxSpecAnMXDpdApplyDpdCfrTargetPaprType.InputPapr
   Private applyDpdCfrTargetPapr As Double = 8.0
   '(dB)
   Private applyDpdCfrWindowType As RFmxSpecAnMXDpdApplyDpdCfrWindowType = RFmxSpecAnMXDpdApplyDpdCfrWindowType.KaiserBessel
   Private applyDpdCfrWindowLength As Integer = 10
   Private applyDpdCfrShapingFactor As Double = 5.0
   Private applyDpdCfrShapingThreshold As Double = -5.0
   '(dB)

   Private carrierString As String

   Private meanLinearGain As Double, onedBCompressionPoint As Double, meanRmsEvm As Double, gainErrorRange As Double,
      phaseErrorRange As Double, meanPhaseError As Double, amToAMResidual As Double, amToPMResidual As Double,
      powerOffset As Double, nmse As Double

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
         ConfigureRfsgAndRFmx()
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

   Private Sub ConfigureRfsgAndRFmx()
      rfsgSession = New NIRfsg(rfsgResourceName, False, True)
      rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.Script
      rfsgSession.FrequencyReference.Configure(referenceClockSource, referenceClockRate)
      rfsgSession.DeviceEvents.MarkerEvents(markerNumber).ExportedOutputTerminal = RfsgMarkerEventExportedOutputTerminal.PxiTriggerLine0
      rfsgSession.RF.Configure(centerFrequency, dutAverageInputPower)
      rfsgSession.RF.PowerLevelType = powerLevelType
      waveformScript = [String].Format("script {0}{1}repeat forever{1}generate {2} marker{3}(0){1}end repeat{1}end script", scriptName, Environment.NewLine, waveformName, markerNumber)
      rfsgSession.RF.ExternalGain = -rfsgExternalAttenuation

      instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
      specAn.SetSelectedPorts("", selectedPorts)

      If triggerType = RFmxSpecAnMXTriggerType.IQPowerEdge Then
         specAn.ConfigureIQPowerEdgeTrigger("", "0", -20.0, RFmxSpecAnMXIQPowerEdgeTriggerSlope.Rising, triggerDelay,
            RFmxSpecAnMXTriggerMinimumQuietTimeMode.Manual, 0.0, enableTrigger)
      Else
         specAn.ConfigureDigitalEdgeTrigger("", RFmxInstrMXConstants.PxiTriggerLine0,
            RFmxSpecAnMXDigitalEdgeTriggerEdge.Rising, triggerDelay, enableTrigger)
      End If
      specAn.ConfigureRF("", centerFrequency, referenceLevel, rfsaExternalAttenuation)
      specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Dpd, True)
      If preDpdCfrEnabled = RFmxSpecAnMXDpdPreDpdCfrEnabled.True Then
         specAn.Dpd.PreDpd.SetCfrEnabled("", preDpdCfrEnabled)
         specAn.Dpd.PreDpd.SetCfrMethod("", preDpdCfrMethod)
         specAn.Dpd.PreDpd.SetCfrMaximumIterations("", preDpdCfrMaximumIterations)
         specAn.Dpd.PreDpd.SetCfrTargetPapr("", preDpdCfrTargetPapr)
         specAn.Dpd.PreDpd.SetCfrWindowType("", preDpdCfrWindowType)
         specAn.Dpd.PreDpd.SetCfrWindowLength("", preDpdCfrWindowLength)
         specAn.Dpd.PreDpd.SetCfrShapingFactor("", preDpdCfrShapingFactor)
         specAn.Dpd.PreDpd.SetCfrShapingThreshold("", preDpdCfrShapingThreshold)
         specAn.Dpd.PreDpd.SetCfrFilterEnabled("", preDpdCfrFilterEnabled)
         specAn.Dpd.PreDpd.SetCfrNumberOfCarriers("", NumberOfCarriers)
         For i As Integer = 0 To NumberOfCarriers - 1
            carrierString = RFmxSpecAnMX.BuildCarrierString2("", i)
            specAn.Dpd.PreDpd.SetCarrierOffset(carrierString, preDpdCarrierOffsets(i))
            specAn.Dpd.PreDpd.SetCarrierBandwidth(carrierString, preDpdCarrierBandwidths(i))
         Next
         specAn.Dpd.PreDpd.ApplyPreDpdSignalConditioning("", referenceWaveformComplexSingle, dpdApplyDpdIdleDurationPresent,
            preDpdWaveformWithComplexSingle, papr)
      End If
      If preDpdCfrEnabled = RFmxSpecAnMXDpdPreDpdCfrEnabled.[True] Then
         rfsgSession.Arb.WriteWaveform(waveformName, preDpdWaveformWithComplexSingle)
         sampleRate = 1 / preDpdWaveformWithComplexSingle.PrecisionTiming.SampleInterval.TotalSeconds
         rfsgSession.Arb.IQRate = sampleRate
      Else
         rfsgSession.Arb.ReadAndDownloadWaveformFromFileTdms(waveformName, referenceWaveformFile, 0)
         sampleRate = rfsgSession.Arb.Waveforms(waveformName).IQRate
         papr = rfsgSession.Arb.Waveforms(waveformName).Papr
      End If
      rfsgSession.Arb.Waveforms(waveformName).Papr = papr
      runtimeScaling = preFilterGain
      rfsgSession.Arb.PreFilterGain = runtimeScaling
      rfsgSession.Arb.SignalBandwidth = 0.8 * sampleRate
      rfsgSession.Arb.Scripting.WriteScript(waveformScript)
      rfsgSession.Initiate()

      If preDpdCfrEnabled = RFmxSpecAnMXDpdPreDpdCfrEnabled.True Then
         specAn.Dpd.Configuration.ConfigureReferenceWaveform("", preDpdWaveformWithComplexSingle, idleDurationPresent, signalType)
      Else
         specAn.Dpd.Configuration.ConfigureReferenceWaveform("", referenceWaveformComplexSingle, idleDurationPresent, signalType)
      End If
      specAn.Dpd.Configuration.ConfigureDutAverageInputPower("", dutAverageInputPower)
      specAn.Dpd.Configuration.ConfigureDpdModel("", dpdModel)
      specAn.Dpd.Configuration.ConfigureMemoryPolynomial("", memoryPolynomialOrder, memoryPolynomialDepth)
      specAn.Dpd.Configuration.ConfigureGeneralizedMemoryPolynomialCrossTerms("", crossTermsLeadOrder, crossTermsLagOrder,
         crossTermsLeadMemoryDepth, crossTermsLagMemoryDepth, crossTermsMaximumLead, crossTermsMaximumLag)
      specAn.Dpd.Configuration.ConfigureMeasurementSampleRate("", sampleRateMode, sampleRate)
      specAn.Dpd.Configuration.ConfigureMeasurementInterval("", measurementInterval)
      specAn.Dpd.Configuration.ConfigureIterativeDpdEnabled("", iterativeDpdEnabled)
      If iterativeDpdEnabled = RFmxSpecAnMXDpdIterativeDpdEnabled.False Then
         numberOfIterations = 1
      End If
      specAn.Dpd.Configuration.SetNmseEnabled("", nmseEnabled)
      specAn.Dpd.ApplyDpd.ConfigureMemoryModelCorrectionType("", memoryModelCorrectionType)
      specAn.Dpd.ApplyDpd.SetCfrEnabled("", applyDpdCfrEnabled)
      specAn.Dpd.ApplyDpd.SetCfrMethod("", applyDpdCfrMethod)
      specAn.Dpd.ApplyDpd.SetCfrMaximumIterations("", applyDpdCfrMaximumIterations)
      specAn.Dpd.ApplyDpd.SetCfrTargetPaprType("", applyDpdCfrTargetPaprType)
      specAn.Dpd.ApplyDpd.SetCfrTargetPapr("", applyDpdCfrTargetPapr)
      specAn.Dpd.ApplyDpd.SetCfrWindowType("", applyDpdCfrWindowType)
      specAn.Dpd.ApplyDpd.SetCfrWindowLength("", applyDpdCfrWindowLength)
      specAn.Dpd.ApplyDpd.SetCfrShapingFactor("", applyDpdCfrShapingFactor)
      specAn.Dpd.ApplyDpd.SetCfrShapingThreshold("", applyDpdCfrShapingThreshold)
      specAn.AutoLevel("", signalBandwidth, autoLevelMeasurementInterval, autoLevelReferenceLevel)
   End Sub

   Private Sub RetrieveResults()
      For i As Integer = 0 To numberOfIterations - 1
         specAn.Dpd.Configuration.ConfigurePreviousDpdPolynomial("", dpdPolynomial)
         specAn.Initiate("", "")
         If preDpdCfrEnabled = RFmxSpecAnMXDpdPreDpdCfrEnabled.True Then
            specAn.Dpd.ApplyDpd.ApplyDigitalPredistortion("", preDpdWaveformWithComplexSingle, dpdApplyDpdIdleDurationPresent,
               timeout, waveformWithDpdComplexSingle, papr, powerOffset)
         Else
            specAn.Dpd.ApplyDpd.ApplyDigitalPredistortion("", referenceWaveformComplexSingle, dpdApplyDpdIdleDurationPresent,
               timeout, waveformWithDpdComplexSingle, papr, powerOffset)
         End If
         specAn.Dpd.Results.FetchDpdPolynomial("", timeout, dpdPolynomial)
         specAn.Dpd.Results.FetchNmse("", timeout, nmse)
         Console.WriteLine("NMSE            {0}", nmse)

         rfsgSession.Abort()
         rfsgSession.Arb.ClearWaveform(waveformName)
         rfsgIqRate = 1 / waveformWithDpdComplexSingle.PrecisionTiming.SampleInterval.TotalSeconds
         rfsgSession.Arb.WriteWaveform(waveformName, waveformWithDpdComplexSingle)
         rfsgSession.Arb.PreFilterGain = runtimeScaling
         rfsgSession.Arb.IQRate = rfsgIqRate
         rfsgSession.Arb.Waveforms(waveformName).Papr = (papr + powerOffset)
         rfsgSession.Arb.SignalBandwidth = 0.8 * rfsgIqRate
         rfsgSession.Arb.Scripting.WriteScript(waveformScript)
         rfsgSession.Initiate()
      Next

      specAn.AutoLevel("", signalBandwidth, autoLevelMeasurementInterval, autoLevelReferenceLevel)
      specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Ampm, True)
      specAn.Ampm.Configuration.ConfigureMeasurementSampleRate("", RFmxSpecAnMXAmpmMeasurementSampleRateMode.ReferenceWaveform, sampleRate)
      specAn.Ampm.Configuration.ConfigureMeasurementInterval("", measurementInterval)
      If preDpdCfrEnabled = RFmxSpecAnMXDpdPreDpdCfrEnabled.True Then
         specAn.Ampm.Configuration.ConfigureReferenceWaveform("", preDpdWaveformWithComplexSingle,
             RFmxSpecAnMXAmpmReferenceWaveformIdleDurationPresent.[False], RFmxSpecAnMXAmpmSignalType.Modulated)
      Else
         specAn.Ampm.Configuration.ConfigureReferenceWaveform("", referenceWaveformComplexSingle,
             RFmxSpecAnMXAmpmReferenceWaveformIdleDurationPresent.[False], RFmxSpecAnMXAmpmSignalType.Modulated)
      End If
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
