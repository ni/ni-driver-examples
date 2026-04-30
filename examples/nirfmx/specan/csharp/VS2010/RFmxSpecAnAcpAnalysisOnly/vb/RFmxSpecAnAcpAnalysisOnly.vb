'Steps:
'1. Open a new RFmx session.
'2. Configure Center Frequency.
'3. Select ACP measurement and enable the traces.
'4. Configure RBW filter parameters.
'5. Configure Sweep Time.
'6. Configure Integration BW of the Carrier and Offset Channels, Number of Offset Channels and Channel Spacing between the Offset channels. This function configures a Carrier Channel with Offset Channels as specified by the Number of Offsets. Refer to method help for more information.
'7. Configure Averaging Parameters.
'8. Commit the settings on the RFmx session and read the acquisition parameters (Span and Number of Records)from the RFmxInstr Property node.

'9. Open a new NI-RFSA session.
'10. Configure the NI-RFSA device reference clock.
'11. Configure the Center Frequency of the RFSA hardware for the 2 possible Acquisition types - IQ and Spectrum acquisition.
'12. Configure the External gain (in dB) of a device or cable connected before the RF IN connector of the NI-RFSA.
'13. Configure the Reference Level in dBm.
'14. Read the maximum instantaneous bandwidth of the RFSA device and compare with the Span requested by the measurement to decide the type of acquisition to be performed by RFSA.

'15. Configure the Acquisition Type to Spectrum.
'16. Read the Spectrum Acquisition parameters (Resolution Bandwidth and FFT Window Type) from the RFmx session and pass the same settings to the NI-RFSA session.
'17. Read the Power Spectrum from RFSA and pass it to RFmx AnalyzeSpectrum function for performing the measurement.
'    Note: NI-RFSA returns the power spectrum centered at the configured Center Frequency, but RFmx AnalyzeSpectrum function expects the spectrum to be at baseband i.e f0 is relative to 0Hz.

'18. Configure the Acquisition Type to IQ.
'19. Read the IQ Acquisition parameters (Sampling Rate and Acquisition Time) from the RFmx session and pass the same settings to the NI-RFSA session.
'20. Initiate the acquisition.
'21. Read the IQ Waveform from RFSA and pass it to RFmx AnalyzeIQ function for performing the measurement.
'    Note: In a multi-record acquisition case, the 'reset' parameter of the RFmxSpecAn AnalyzeIQ function is true for the first iteration and false for the remaining iterations.

'22. Fetch ACP Measurements and Traces.
'23. Close the RFmx and NI-RFSA Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.SpecAnMX
Imports NationalInstruments.ModularInstruments.NIRfsa

Public Class RFmxSpecAnAcpAnalysisOnly
   Private rfsaSession As NIRfsa
   Private instrSession As RFmxInstrMX
   Private specAn As RFmxSpecAnMX

   Const NumberOfOffsets As Integer = 1

   Private rfsaResourceName As String = "RFSA"
   Private centerFrequency As Double = 1000000000.0
   Private updatedCenterFrequency As Double
   ' Hz 
   Private referenceLevel As Double = 0.0
   ' dBm 
   Private externalAttenuation As Double = 0.0
   ' dB 

   Private referenceClockSource As RfsaReferenceClockSource = RfsaReferenceClockSource.OnboardClock
   Private referenceClockRate As Double = 10000000.0
   ' Hz 

   Private integrationBandwidth As Double = 1000000.0
   ' Hz 
   Private channelSpacing As Double = 1000000.0
   ' Hz 

   Private rbwAuto As RFmxSpecAnMXAcpRbwAutoBandwidth = RFmxSpecAnMXAcpRbwAutoBandwidth.[True]
   Private rbw As Double = 10000.0
   ' Hz 
   Private rbwFilterType As RFmxSpecAnMXAcpRbwFilterType = RFmxSpecAnMXAcpRbwFilterType.Gaussian

   Private sweepTimeAuto As RFmxSpecAnMXAcpSweepTimeAuto = RFmxSpecAnMXAcpSweepTimeAuto.[True]
   Private sweepTimeInterval As Double = 1000000.0
   ' s 

   Private averagingCount As Integer = 10
   Private averagingEnabled As RFmxSpecAnMXAcpAveragingEnabled = RFmxSpecAnMXAcpAveragingEnabled.[False]
   Private averagingType As RFmxSpecAnMXAcpAveragingType = RFmxSpecAnMXAcpAveragingType.Rms

   Private timeout As Double = 10
   ' s
   Private numberOfRecords As Integer
   Private numberOfSamples As Long
   Private maxDeviceInstantaneousBandwidth As Double, spectralAcquisitionSpan As Double
   Private reset As Boolean
   Private reserved As Long = 0

   Private absolutePower As Double
   Private lowerRelativePower As Double()
   Private upperRelativePower As Double()
   Private lowerAbsolutePower As Double()
   Private upperAbsolutePower As Double()

   Friend Sub Run()
      Try
         ConfigureRfsaAndRFmx()
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

   Private Sub ConfigureRfsaAndRFmx()
      ' Create a new RFSA Session 

      rfsaSession = New NIRfsa(rfsaResourceName, True, False)

      rfsaSession.Configuration.ReferenceClock.Configure(referenceClockSource, referenceClockRate)
      rfsaSession.Configuration.IQ.CarrierFrequency = centerFrequency
      rfsaSession.Configuration.Spectrum.CenterFrequency = centerFrequency
      rfsaSession.Configuration.Vertical.Advanced.ExternalGain = -externalAttenuation
      rfsaSession.Configuration.Vertical.ReferenceLevel = referenceLevel

      ' Create a new RFmx Session 

      instrSession = New RFmxInstrMX("", "AnalysisOnly=1")

      ' Get SpecAn signal 

      specAn = instrSession.GetSpecAnSignalConfiguration()

      ' Configure measurement 

      specAn.ConfigureFrequency("", centerFrequency)
      specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Acp, True)
      specAn.Acp.Configuration.ConfigureRbwFilter("", rbwAuto, rbw, rbwFilterType)
      specAn.Acp.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval)
      specAn.Acp.Configuration.ConfigureCarrierAndOffsets("", integrationBandwidth, NumberOfOffsets, channelSpacing)
      specAn.Acp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)
      specAn.Commit("")

      maxDeviceInstantaneousBandwidth = rfsaSession.DeviceCharacteristics.MaxInstantaneousBandwidth
      instrSession.GetRecommendedNumberOfRecords("", numberOfRecords)
      instrSession.GetRecommendedSpectralAcquisitionSpan("", spectralAcquisitionSpan)

      If (maxDeviceInstantaneousBandwidth >= spectralAcquisitionSpan) Then
         ' IQ Acquisition.
         rfsaSession.Configuration.AcquisitionType = RfsaAcquisitionType.IQ

         Dim acquisitionTime As Double, minimumSampleRate As Double
         instrSession.GetRecommendedIQAcquisitionTime("", acquisitionTime)
         instrSession.GetRecommendedIQMinimumSampleRate("", minimumSampleRate)

         numberOfSamples = CLng(Math.Ceiling(minimumSampleRate * acquisitionTime))
         rfsaSession.Configuration.IQ.IQRate = minimumSampleRate
         rfsaSession.Configuration.IQ.NumberOfSamples = numberOfSamples
         rfsaSession.Configuration.IQ.NumberOfRecordsIsFinite = True
         rfsaSession.Configuration.IQ.NumberOfRecords = numberOfRecords
         rfsaSession.Acquisition.IQ.Initiate()

         For i As Integer = 0 To numberOfRecords - 1
            reset = If((i = 0), True, False)
            Dim iqInfo As RfsaWaveformInfo
            Dim iqData As ComplexDouble() = rfsaSession.Acquisition.IQ.FetchIQSingleRecordComplex(Of ComplexDouble)(i, numberOfSamples, New PrecisionTimeSpan(timeout), iqInfo)
            Dim iq As ComplexWaveform(Of ComplexSingle) = CreateWaveform(iqData, iqInfo)
            specAn.AnalyzeIQ1Waveform("", "", iq, reset, reserved)
         Next
      Else
         ' Spectral Acquisition.
         rfsaSession.Configuration.AcquisitionType = RfsaAcquisitionType.Spectrum

         Dim recommendedFftWindow As RFmxInstrMXRecommendedSpectralFftWindow
         Dim fftWindowType As RfsaFftWindowType
         Dim resolutionBandwidth As Double

         instrSession.GetRecommendedSpectralFftWindow("", recommendedFftWindow)
         instrSession.GetRecommendedSpectralResolutionBandwidth("", resolutionBandwidth)
         Select Case recommendedFftWindow
            Case RFmxInstrMXRecommendedSpectralFftWindow.FlatTop
               fftWindowType = RfsaFftWindowType.FlatTop
               Exit Select
            Case RFmxInstrMXRecommendedSpectralFftWindow.Hanning
               fftWindowType = RfsaFftWindowType.Hanning
               Exit Select
            Case RFmxInstrMXRecommendedSpectralFftWindow.Hamming
               fftWindowType = RfsaFftWindowType.Hamming
               Exit Select
            Case RFmxInstrMXRecommendedSpectralFftWindow.Gaussian
               fftWindowType = RfsaFftWindowType.Gaussian
               Exit Select
            Case RFmxInstrMXRecommendedSpectralFftWindow.Blackman
               fftWindowType = RfsaFftWindowType.Blackman
               Exit Select
            Case RFmxInstrMXRecommendedSpectralFftWindow.BlackmanHarris
               fftWindowType = RfsaFftWindowType.BlackmanHarris
               Exit Select
            Case RFmxInstrMXRecommendedSpectralFftWindow.KaiserBessel
               fftWindowType = RfsaFftWindowType.KaiserBessel
               Exit Select
            Case Else
               fftWindowType = RfsaFftWindowType.Uniform
               Exit Select
         End Select

         updatedCenterFrequency = rfsaSession.Configuration.Spectrum.CenterFrequency
         rfsaSession.Configuration.Spectrum.ResolutionBandwidth = resolutionBandwidth
         rfsaSession.Configuration.Spectrum.FftWindowType = fftWindowType
         rfsaSession.Configuration.Spectrum.ResolutionBandwidthType = RfsaResolutionBandwidthType.RbwBinWidth
         rfsaSession.Configuration.Spectrum.PowerSpectrumUnits = RfsaPowerSpectrumUnits.dBm
         rfsaSession.Configuration.Spectrum.Span = spectralAcquisitionSpan

         For i As Integer = 0 To numberOfRecords - 1
            reset = If((i = 0), True, False)
            Dim spectrumInfo As RfsaSpectrumInfo
            Dim spectrumData As Double() = rfsaSession.Acquisition.Spectrum.ReadPowerSpectrum(New PrecisionTimeSpan(timeout), spectrumInfo)
            Dim powerSpectrum As Spectrum(Of Single) = CreateSpectrum(spectrumData, spectrumInfo)
            powerSpectrum.StartFrequency = powerSpectrum.StartFrequency - updatedCenterFrequency
            specAn.AnalyzeSpectrum1Waveform("", "", powerSpectrum, reset, reserved)
         Next
      End If
   End Sub

   Private Function CreateWaveform(data As ComplexDouble(), info As RfsaWaveformInfo) As ComplexWaveform(Of ComplexSingle)
      Dim target As New ComplexWaveform(Of ComplexSingle)(data.Length)
      Dim targetBuffer = target.GetWritableBuffer()
      For i As Integer = 0 To data.Length - 1
         targetBuffer(i) = New ComplexSingle(Convert.ToSingle(data(i).Real), Convert.ToSingle(data(i).Imaginary))
      Next
      target.PrecisionTiming = PrecisionWaveformTiming.CreateWithRegularInterval(New PrecisionTimeSpan(info.XIncrement), New PrecisionTimeSpan(info.AbsoluteInitialX))
      Return target
   End Function

   Private Function CreateSpectrum(data As Double(), info As RfsaSpectrumInfo) As Spectrum(Of Single)
      Dim target As New Spectrum(Of Single)(data.Length)
      Dim targetBuffer = target.GetWritableBuffer()
      For i As Integer = 0 To data.Length - 1
         targetBuffer(i) = Convert.ToSingle(data(i))
      Next
      target.StartFrequency = info.InitialFrequency
      target.FrequencyIncrement = info.FrequencyIncrement
      Return target
   End Function

   Private Sub RetrieveResults()
      ' Retrieve results 


      Dim spectrum As Spectrum(Of Single) = Nothing
      Dim totalRelativePower As Double, carrierFrequency As Double

      specAn.Acp.Results.FetchOffsetMeasurementArray("", timeout, lowerRelativePower, upperRelativePower, lowerAbsolutePower, upperAbsolutePower)

      specAn.Acp.Results.FetchCarrierMeasurement("", timeout, absolutePower, totalRelativePower, carrierFrequency, integrationBandwidth)

      specAn.Acp.Results.FetchSpectrum("", timeout, spectrum)
   End Sub

   Private Sub PrintResults()
      Console.WriteLine("-----------------Carrier Measurements-----------------" & vbLf)
      Console.WriteLine("Absolute Power (dBm)       {0}", absolutePower)

      Console.WriteLine(vbLf & "--------------Offset Channel Measurements-------------" & vbLf)
      For i As Integer = 0 To NumberOfOffsets - 1
         Console.WriteLine("----Offset {0}" & vbLf, i)
         Console.WriteLine("Lower Relative Power (dB)            {0}", lowerRelativePower(i))
         Console.WriteLine("Upper Relative Power (dB)            {0}", upperRelativePower(i))
         Console.WriteLine("Lower Absolute Power (dBm)           {0}", lowerAbsolutePower(i))
         Console.WriteLine("Upper Absolute Power (dBm)           {0}", upperAbsolutePower(i))
      Next
      Console.WriteLine("-------------------------------------------------" & vbLf)
   End Sub

   Private Sub CloseSession()
      Try
         If specAn IsNot Nothing Then
            specAn.Dispose()
            specAn = Nothing
         End If

         If instrSession IsNot Nothing Then
            instrSession.Close()
            instrSession = Nothing
         End If

         If rfsaSession IsNot Nothing Then
            rfsaSession.Dispose()
            rfsaSession = Nothing
         End If
      Catch ex As Exception
         DisplayError(ex)
      End Try
   End Sub

   Private Sub DisplayError(ex As Exception)
      Console.WriteLine("ERROR:" & vbLf & Convert.ToString(ex.[GetType]()) & ": " & ex.Message)
   End Sub
End Class
