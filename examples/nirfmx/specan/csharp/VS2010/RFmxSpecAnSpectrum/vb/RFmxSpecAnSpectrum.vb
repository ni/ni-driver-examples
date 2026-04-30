'Steps:
'1. Open a New RFmx session.
'2. Configure the basic instrument properties(Clock Source, Clock Frequency).
'3. Configure Selected Ports.
'4. Configure the Center Frequency, Span Or Start, Stop Frequency based on the Tab Selection.
'5. Configure the basic signal properties(Reference Level, External Attenuation).
'6. Select Spectrum measurement And enable the traces.
'7. Configure Measurement Method
'8. Configure RBW Filter Parameters.
'9. Configure Power Units.
'10. Configure Sweep Time.
'11. Configure Spectrum Averaging.
'12. Configure FFT parameters.
'13. Configure Noise Compensation Enabled.
'14. Configure Detectors.
'15. Configure VBW Filter Parameters.
'16. If Measurement Method Is Sequential FFT, Configure Sequential FFT Parameters. 
'17. Configure Cleaner Spectrum.
'18. Initiate Measurement.
'19. Fetch Spectrum Traces And Measurements.
'20. Close the RFmx Session.

Imports NationalInstruments
Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.SpecAnMX

Public Class RFmxSpecAnSpectrum
   Private instrSession As RFmxInstrMX
   Private specAn As RFmxSpecAnMX
   Private resourceName As String, frequencySource As String
   Private isStartStopFreq As Boolean
   Private startFrequency As Double, endFrequency As Double
   Private selectedPorts As String
   Private centerFrequency As Double, referenceLevel As Double, externalAttenuation As Double, frequency As Double,
      rbw As Double, sweepTimeInterval As Double, span As Double, fftPadding As Double, fftOverlapPercent As Double, timeout As Double
   Private vbw As Double, vbwToRbwRatio As Double
   Private cleanerSpectrum As RFmxInstrMXCleanerSpectrum
   Private measurementMethod As RFmxSpecAnMXSpectrumMeasurementMethod
   Private powerUnits As RFmxSpecAnMXSpectrumPowerUnits
   Private noiseCompensationEnabled As RFmxSpecAnMXSpectrumNoiseCompensationEnabled
   Private rbwFilterType As RFmxSpecAnMXSpectrumRbwFilterType
   Private rbwAuto As RFmxSpecAnMXSpectrumRbwAutoBandwidth
   Private sweepTimeAuto As RFmxSpecAnMXSpectrumSweepTimeAuto
   Private averagingEnabled As RFmxSpecAnMXSpectrumAveragingEnabled
   Private averagingCount As Integer
   Private detectorPoints As Integer
   Private sequentialFftSize As Integer
   Private averagingType As RFmxSpecAnMXSpectrumAveragingType
   Private fftWindow As RFmxSpecAnMXSpectrumFftWindow
   Private fftOverlapMode As RFmxSpecAnMXSpectrumFftOverlapMode
   Private fftOverlapType As RFmxSpecAnMXSpectrumFftOverlapType
   Private vbwAuto As RFmxSpecAnMXSpectrumVbwFilterAutoBandwidth
   Private detectorType As RFmxSpecAnMXSpectrumDetectorType

   Private peakAmplitude As Double, peakFrequency As Double, frequencyResolution As Double

   Public Sub Run()
      Try
         InitializeVariables()
         InitializeInstr()
         ConfigureSpecAn()
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

      isStartStopFreq = False
      startFrequency = 995000000.0
      ' Hz
      endFrequency = 1005000000.0
      ' Hz

      selectedPorts = ""
      centerFrequency = 1000000000.0
      ' Hz
      referenceLevel = 0.0
      ' dBm
      externalAttenuation = 0.0
      ' dB

      timeout = 10.0
      ' seconds

      frequencySource = RFmxInstrMXConstants.OnboardClock
      frequency = 10000000.0
      ' Hz

      span = 1000000.0
      ' Hz

      measurementMethod = RFmxSpecAnMXSpectrumMeasurementMethod.Normal
	  powerUnits = RFmxSpecAnMXSpectrumPowerUnits.dBm

      noiseCompensationEnabled = RFmxSpecAnMXSpectrumNoiseCompensationEnabled.[False]
	  cleanerSpectrum = RFmxInstrMXCleanerSpectrum.Disabled

      'RBW Filter
      rbwFilterType = RFmxSpecAnMXSpectrumRbwFilterType.Gaussian
      rbwAuto = RFmxSpecAnMXSpectrumRbwAutoBandwidth.[True]
      rbw = 10000.0
      ' Hz

      vbwAuto = RFmxSpecAnMXSpectrumVbwFilterAutoBandwidth.[True]
      vbw = 30000.0
      ' Hz
      vbwToRbwRatio = 3

      detectorType = RFmxSpecAnMXSpectrumDetectorType.None
      detectorPoints = 1001

      'Sweep Time
      sweepTimeAuto = RFmxSpecAnMXSpectrumSweepTimeAuto.[True]
      sweepTimeInterval = 0.001
      ' seconds

      'Averaging
      averagingEnabled = RFmxSpecAnMXSpectrumAveragingEnabled.[False]
      averagingCount = 10
      averagingType = RFmxSpecAnMXSpectrumAveragingType.Rms

      'FFT
      fftWindow = RFmxSpecAnMXSpectrumFftWindow.FlatTop
      fftPadding = -1.0
	  fftOverlapMode = RFmxSpecAnMXSpectrumFftOverlapMode.Disabled
	  fftOverlapPercent = 0
	  fftOverlapType = RFmxSpecAnMXSpectrumFftOverlapType.Rms
      sequentialFftSize = 512
   End Sub

   Private Sub InitializeInstr()
      ' Create a new RFmx Session

      instrSession = New RFmxInstrMX(resourceName, "")
   End Sub

   Private Sub ConfigureSpecAn()
      ' Get SpecAn signal

      specAn = instrSession.GetSpecAnSignalConfiguration()

      ' Configure measurement

      instrSession.ConfigureFrequencyReference("", frequencySource, frequency)
      specAn.SetSelectedPorts("", selectedPorts)
      If isStartStopFreq Then
         specAn.Spectrum.Configuration.ConfigureFrequencyStartStop("", startFrequency, endFrequency)
      Else
         'Configure span
         specAn.ConfigureFrequency("", centerFrequency)
         specAn.Spectrum.Configuration.ConfigureSpan("", span)
      End If
      specAn.ConfigureReferenceLevel("", referenceLevel)
      specAn.ConfigureExternalAttenuation("", externalAttenuation)
      specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Spectrum, True)
	  specAn.Spectrum.Configuration.ConfigureMeasurementMethod("", measurementMethod)
      specAn.Spectrum.Configuration.ConfigureRbwFilter("", rbwAuto, rbw, rbwFilterType)
      specAn.Spectrum.Configuration.ConfigurePowerUnits("", powerUnits)
      specAn.Spectrum.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval)
      specAn.Spectrum.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)
      specAn.Spectrum.Configuration.ConfigureFft("", fftWindow, fftPadding)
      specAn.Spectrum.Configuration.ConfigureNoiseCompensationEnabled("", noiseCompensationEnabled)
      specAn.Spectrum.Configuration.ConfigureDetector("", detectorType, detectorPoints)
      specAn.Spectrum.Configuration.ConfigureVbwFilter("", vbwAuto, vbw, vbwToRbwRatio)
	  specAn.Spectrum.Configuration.SetFftOverlapMode("", fftOverlapMode)
	  specAn.Spectrum.Configuration.SetFftOverlap("", fftOverlapPercent)
	  specAn.Spectrum.Configuration.SetFftOverlapType("", fftOverlapType)
	  specAn.Spectrum.Configuration.SetSequentialFftSize("", sequentialFftSize)
	  instrSession.SetCleanerSpectrum("", cleanerSpectrum)
      specAn.Initiate("", "")
   End Sub

   Private Sub RetrieveResults()
      ' Retrieve results

      Dim spectrum As Spectrum(Of Single) = Nothing
      specAn.Spectrum.Results.FetchSpectrum("", timeout, spectrum)

      specAn.Spectrum.Results.FetchMeasurement("", timeout, peakAmplitude, peakFrequency, frequencyResolution)
   End Sub

   Private Sub PrintResults()
      Console.WriteLine("Peak Amplitude (dBm)             {0}", peakAmplitude)
      Console.WriteLine("Peak Frequency (Hz)              {0}", peakFrequency)
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
      Catch ex As Exception
         DisplayError(ex)
      End Try
   End Sub

   Private Shared Sub DisplayError(ex As Exception)
      Console.WriteLine("ERROR:" & vbLf & Convert.ToString(ex.[GetType]()) & ": " & ex.Message)
   End Sub
End Class
