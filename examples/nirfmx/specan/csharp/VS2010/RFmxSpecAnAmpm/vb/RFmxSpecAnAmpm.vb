' Steps
'1. Open RFSG session.
'2. Configure RFSG frequency reference, generation Mode to Script, power level type and Pre-filter Gain.
'3. Configure marker0 to be generated from RFSG on the specified output terminal.
'4. Configure frequency and power level of RF output signal.
'5. Set RFSG External Gain. #4 and #5 ensure that the average power of the signal at
'   the input Of the DUT matches the user configured DUT Average Input Power.
'6. a. Read waveform from file and download Waveform from file to RFSG
'   b.Set Waveform Runtime Scaling to the negative of the desired Pre-filter Gain.
'   c.Read waveform sample rate, multiply by 0.8 and set the result to the signal bandwidth.
'   d.Write script to generate the waveform specified in the script.
'   e.This script Is programmed To generate waveform continuously, with marker0 aligned to sample index 0.
'7. Initiate RFSG generation as per the selected script.
'8. Open RFmx session.
'9. Configure frequency reference of the analyser.
'10. Configure Selected Ports.
'11. Configure trigger to use as reference for signal acquisition.
'12. Configure center frequency and external attenuation.
'13. Select AMPM measurement, configure the reference waveform and power of this signal at the input of the DUT.
'14. Set the measurement sample rate and the measurement interval to use for analysis.
'15. Set threshold.
'16. Configure Reference Power Type.
'17. Set Reference Level Or perform Auto Level to compute an approximate reference level to use by the analyser.
'18. Initiate and fetch AMPM results.
'19. Close RFmx session.
'20. Close RFSG session.
'It Is recommended to clear the waveform before closing RFSG session.


Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.SpecAnMX
Imports NationalInstruments.DataInfrastructure
Imports NationalInstruments.ModularInstruments.NIRfsg
Imports NationalInstruments.ModularInstruments.NIRfsgPlayback
Imports System.IO

Public Class RFmxSpecAnAmpm
   Private instrSession As RFmxInstrMX
   Private specAn As RFmxSpecAnMX
   Private rfsgSession As NIRfsg

   Private rfsaResourceName As String = "RFSA"
   Private rfsgResourceName As String = "RFSG"

   Private frequencyReferenceSource As String = RFmxInstrMXConstants.OnboardClock
   Private frequencyReferenceFrequency As Double = 10000000.0
   ' Hz
   Private triggerType As RFmxSpecAnMXTriggerType = RFmxSpecAnMXTriggerType.DigitalEdge
   Private triggerDelay As Double = 0
   ' seconds
   Private enableTrigger As Boolean = True
   Private digitalEdgeTriggerSource As String = RFmxSpecAnMXConstants.PxiTriggerLine0
   Private markerEventExportedOutputTerminal As String = RfsgMarkerEventExportedOutputTerminal.PxiTriggerLine0
   Private digitalEdgeTriggerEdge As RFmxSpecAnMXDigitalEdgeTriggerEdge = RFmxSpecAnMXDigitalEdgeTriggerEdge.Rising
   Private iqPowerEdgeTriggerSource As String = "0"
   Private iqPowerEdgeTriggerLevel As Double = -20
   Private iqPowerEdgeSlope As RFmxSpecAnMXIQPowerEdgeTriggerSlope = RFmxSpecAnMXIQPowerEdgeTriggerSlope.Rising
   Private minimumQuietTimeMode As RFmxSpecAnMXTriggerMinimumQuietTimeMode = RFmxSpecAnMXTriggerMinimumQuietTimeMode.Manual
   Private minimumQuietTimeDuration As Double = 0
   ' seconds
   Private selectedPorts As String = ""
   Private centerFrequency As Double = 1000000000.0
   ' Hz
   Private rfsaExternalAttenuation As Double = 0.0
   ' dB
   Private rfsgExternalAttenuation As Double = 0.0
   ' dB
   Private autoLevel As Boolean = True
   Private referenceLevel As Double = -14.0
   ' dBm
   Private signalBandwidth As Double = 20000000.0
   ' Hz
   Private autoLevelMeasurementInterval As Double = 0.0001
   ' seconds
   Private autoLevelReferenceLevel As Double
   Private preFilterGain As Double = -4.0
   ' dB
   Private runtimeScaling As Double
   Private dutAverageInputPower As Double = -20
   ' dBm
   Private sampleRateMode As RFmxSpecAnMXAmpmMeasurementSampleRateMode = RFmxSpecAnMXAmpmMeasurementSampleRateMode.ReferenceWaveform
   Private sampleRate As Double = 120000000.0
   ' S/s
   Private measurementInterval As Double = 0.0001
   ' seconds
   Private thresholdEnabled As RFmxSpecAnMXAmpmThresholdEnabled = RFmxSpecAnMXAmpmThresholdEnabled.[True]
   Private thresholdLevel As Double = -20
   ' dB or dBm
   Private thresholdType As RFmxSpecAnMXAmpmThresholdType = RFmxSpecAnMXAmpmThresholdType.Relative
   Private referencePowerType As RFmxSpecAnMXAmpmReferencePowerType = RFmxSpecAnMXAmpmReferencePowerType.Input
   Private referenceWaveformSingle As ComplexWaveform(Of ComplexSingle)

   Private waveformFileName As String = "LTE20MHz Waveform (Two Subframes).tdms"
   Private idleDurationPresent As RFmxSpecAnMXAmpmReferenceWaveformIdleDurationPresent = RFmxSpecAnMXAmpmReferenceWaveformIdleDurationPresent.[False]
   Private signalType As RFmxSpecAnMXAmpmSignalType = RFmxSpecAnMXAmpmSignalType.Modulated
   Private timeout As Double = 10
   ' seconds

   Private referenceClockSource As RfsgFrequencyReferenceSource = RfsgFrequencyReferenceSource.OnboardClock
   Private referenceClockRate As Double = 10000000.0
   Private scriptName As String = "AMPMScript"
   Private waveformName As String = "Wfm"
   Private markerNumber As Integer = 0
   Private waveformScript As String

   Private meanLinearGain As Double, onedBCompressionPoint As Double, meanRmsEvm As Double, gainErrorRange As Double, phaseErrorRange As Double, meanPhaseError As Double,
      amToAMResidual As Double, amToPMResidual As Double

   Private referencePowersAMToAM As Single()
   Private measuredAMToAM As Single()
   Private curveFitAMToAM As Single()
   Private referencePowersAMToPM As Single()
   Private measuredAMToPM As Single()
   Private curveFitAMToPM As Single()

   Friend Sub Run()
      Try
         ReadWaveformFromTdmsFile()
         ConfigureRfsg()
         ConfigureRFmx()
         RetrieveResults()
         PrintResults()
      Catch ex As Exception
         DisplayError(ex)
      Finally
         CloseSessions()
         Console.WriteLine("Press any key to exit.....")
         Console.ReadKey()
      End Try
   End Sub

   Private Sub ConfigureRfsg()
      ' Configure RFSG

      rfsgSession = New NIRfsg(rfsgResourceName, True, True)
      rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.Script
      rfsgSession.RF.PowerLevelType = RfsgRFPowerLevelType.PeakPower
      rfsgSession.FrequencyReference.Configure(referenceClockSource, referenceClockRate)
      rfsgSession.DeviceEvents.MarkerEvents(markerNumber).ExportedOutputTerminal = markerEventExportedOutputTerminal
      rfsgSession.RF.Configure(centerFrequency, dutAverageInputPower)
      waveformScript = "script " & scriptName & vbLf & vbTab & vbTab & "Repeat forever" & vbLf & vbTab & vbTab & vbTab & "Generate " & waveformName & " marker" & markerNumber & "(0)" & vbLf & vbTab & vbTab & "end repeat" & vbLf & "end script"
      rfsgSession.RF.ExternalGain = -rfsgExternalAttenuation
      rfsgSession.Arb.ReadAndDownloadWaveformFromFileTdms(waveformName, waveformFileName, 0)
      sampleRate = rfsgSession.Arb.Waveforms(waveformName).IQRate
      runtimeScaling = preFilterGain
      rfsgSession.Arb.PreFilterGain = runtimeScaling
      rfsgSession.Arb.SignalBandwidth = 0.8 * sampleRate
      rfsgSession.Arb.Scripting.WriteScript(waveformScript)
      rfsgSession.Initiate()
   End Sub

   Private Sub ConfigureRFmx()
      ' Configure RFmx

      instrSession = New RFmxInstrMX(rfsaResourceName, "")
      specAn = instrSession.GetSpecAnSignalConfiguration()
      instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
      specAn.SetSelectedPorts("", selectedPorts)

      If triggerType.Equals(RFmxSpecAnMXTriggerType.DigitalEdge) Then
         specAn.ConfigureDigitalEdgeTrigger("", digitalEdgeTriggerSource, digitalEdgeTriggerEdge, triggerDelay, enableTrigger)
      ElseIf triggerType.Equals(RFmxSpecAnMXTriggerType.IQPowerEdge) Then
         specAn.ConfigureIQPowerEdgeTrigger("", iqPowerEdgeTriggerSource, iqPowerEdgeTriggerLevel, iqPowerEdgeSlope, triggerDelay, minimumQuietTimeMode,
            minimumQuietTimeDuration, enableTrigger)
      End If

      specAn.ConfigureRF("", centerFrequency, referenceLevel, rfsaExternalAttenuation)

      specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Ampm, True)
      specAn.Ampm.Configuration.ConfigureDutAverageInputPower("", dutAverageInputPower)
      specAn.Ampm.Configuration.ConfigureReferenceWaveform("", referenceWaveformSingle, idleDurationPresent, signalType)
      specAn.Ampm.Configuration.ConfigureMeasurementSampleRate("", sampleRateMode, sampleRate)
      specAn.Ampm.Configuration.ConfigureMeasurementInterval("", measurementInterval)
      specAn.Ampm.Configuration.ConfigureThreshold("", thresholdEnabled, thresholdLevel, thresholdType)
      specAn.Ampm.Configuration.ConfigureReferencePowerType("", referencePowerType)

      If autoLevel Then
         specAn.AutoLevel("", signalBandwidth, autoLevelMeasurementInterval, autoLevelReferenceLevel)
         Console.WriteLine("Reference Level(dBm): {0}" & vbLf, autoLevelReferenceLevel)
      Else
         specAn.ConfigureReferenceLevel("", referenceLevel)
      End If

      specAn.Initiate("", "")
   End Sub

   Private Sub RetrieveResults()
      specAn.Ampm.Results.FetchDutCharacteristics("", timeout, meanLinearGain, onedBCompressionPoint, meanRmsEvm)
      specAn.Ampm.Results.FetchError("", timeout, gainErrorRange, phaseErrorRange, meanPhaseError)
      specAn.Ampm.Results.FetchCurveFitResidual("", timeout, amToAMResidual, amToPMResidual)
      specAn.Ampm.Results.FetchAMToAMTrace("", timeout, referencePowersAMToAM, measuredAMToAM, curveFitAMToAM)
      specAn.Ampm.Results.FetchAMToPMTrace("", timeout, referencePowersAMToPM, measuredAMToPM, curveFitAMToPM)

      rfsgSession.Abort()
      rfsgSession.Arb.ClearWaveform(waveformName)
   End Sub

   Private Sub PrintResults()
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

   Private Sub ReadWaveformFromTdmsFile()
      NIRfsgPlayback.ReadWaveformFromFileComplex(waveformFileName, referenceWaveformSingle)
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

   Private Shared Function ComplexDoubleWaveformToComplexSingleWaveform(waveform As ComplexWaveform(Of ComplexDouble)) As ComplexWaveform(Of ComplexSingle)
      Dim retWaveForm As ComplexWaveform(Of ComplexSingle)
      Dim tempArrayDouble As ComplexDouble() = waveform.GetRawData()
      Dim tempArraySingle As ComplexSingle() = New ComplexSingle(tempArrayDouble.Length - 1) {}
      For i As Integer = 0 To tempArraySingle.Length - 1
         tempArraySingle(i).Real = CSng(tempArrayDouble(i).Real)
         tempArraySingle(i).Imaginary = CSng(tempArrayDouble(i).Imaginary)
      Next
      retWaveForm = ComplexWaveform(Of ComplexSingle).FromArray1D(tempArraySingle)
      retWaveForm.PrecisionTiming = PrecisionWaveformTiming.CreateWithRegularInterval(New PrecisionTimeSpan(waveform.PrecisionTiming.SampleInterval.TotalSeconds), New PrecisionTimeSpan(waveform.PrecisionTiming.TimeOffset.TotalSeconds))

      Return retWaveForm
   End Function

End Class
