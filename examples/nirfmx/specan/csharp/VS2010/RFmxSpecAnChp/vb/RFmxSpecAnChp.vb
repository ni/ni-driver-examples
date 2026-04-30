'Steps:
'1. Open a new RFmx session
'2. Configure the instrument properties Clock Source and Clock Frequency
'3. Configure Selected Ports
'4. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
'5. Select CHP measurement and enable the traces
'6. Configure CHP Integration BW, Span and Sweep Time
'7. Configure CHP Averaging
'8. Configure CHP RBW filter
'9. Configure CHP FFT
'10. Configure CHP RRC Filter
'11. Initiate Measurement
'12. Fetch CHP Measurements and Traces
'13. Close the RFmx Session

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.SpecAnMX

Class RFmxSpecAnChp
   Private instrSession As RFmxInstrMX
   Private specAn As RFmxSpecAnMX
   Private resourceName As String, frequencySource As String
   Private selectedPorts As String
   Private centerFrequency As Double, referenceLevel As Double, externalAttenuation As Double,
           frequency As Double, integrationBandwidth As Double, span As Double, rbw As Double,
           sweepTimeInterval As Double, rrcAlpha As Double, fftPadding As Double
   Private enableAllTraces As Boolean
   Private rbwFilterType As RFmxSpecAnMXChpRbwFilterType
   Private rbwAuto As RFmxSpecAnMXChpRbwAutoBandwidth
   Private sweepTimeAuto As RFmxSpecAnMXChpSweepTimeAuto
   Private averagingEnabled As RFmxSpecAnMXChpAveragingEnabled
   Private averagingCount As Integer
   Private averagingType As RFmxSpecAnMXChpAveragingType
   Private rrcFilterEnabled As RFmxSpecAnMXChpCarrierRrcFilterEnabled
   Private fftWindow As RFmxSpecAnMXChpFftWindow

   Private averagePower As Double, psd As Double,
           relativePower As Double, timeout As Double

   Friend Sub Run()
      Try
         InitializeVariables()
         InitializeInstr()
         ConfigureSpecAn()
         RetrieveResults()

         PrintResults()
      Catch ex As Exception
         DisplayError(ex.Message)
      Finally
         ' Close session 

         CloseSession()
         Console.WriteLine("Press any key to exit.....")
         Console.ReadKey()
      End Try
   End Sub

   Private Sub InitializeVariables()
      ' Initialize input variables 

      resourceName = "RFSA"
      selectedPorts = ""
      centerFrequency = 1000000000.0 ' Hz 
      referenceLevel = 0.0 ' dBm 
      externalAttenuation = 0.0 ' dB 
      timeout = 10.0 ' seconds 

      frequencySource = RFmxInstrMXConstants.OnboardClock
      frequency = 10000000.0 ' Hz 

      integrationBandwidth = 1000000.0 ' Hz 
      span = 1000000.0

      'RBW Filter
      rbwFilterType = RFmxSpecAnMXChpRbwFilterType.Gaussian
      rbwAuto = RFmxSpecAnMXChpRbwAutoBandwidth.[True]
      rbw = 10000.0

      'Sweep time
      sweepTimeAuto = RFmxSpecAnMXChpSweepTimeAuto.[True]
      sweepTimeInterval = 0.001 ' seconds 

      'Averaging
      averagingEnabled = RFmxSpecAnMXChpAveragingEnabled.[False]
      averagingCount = 10
      averagingType = RFmxSpecAnMXChpAveragingType.Rms

      'RRC Filter
      rrcFilterEnabled = RFmxSpecAnMXChpCarrierRrcFilterEnabled.[False]
      rrcAlpha = 0.22

      'FFT
      fftWindow = RFmxSpecAnMXChpFftWindow.FlatTop
      fftPadding = -1.0

      enableAllTraces = True
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
      specAn.ConfigureFrequency("", centerFrequency)
      specAn.ConfigureReferenceLevel("", referenceLevel)
      specAn.ConfigureExternalAttenuation("", externalAttenuation)
      specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Chp, enableAllTraces)
      specAn.Chp.Configuration.ConfigureIntegrationBandwidth("", integrationBandwidth)
      specAn.Chp.Configuration.ConfigureSpan("", span)
      specAn.Chp.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval)
      specAn.Chp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount,
                                                  averagingType)
      specAn.Chp.Configuration.ConfigureRbwFilter("", rbwAuto, rbw, rbwFilterType)
      specAn.Chp.Configuration.ConfigureFft("", fftWindow, fftPadding)
      specAn.Chp.Configuration.ConfigureRrcFilter("", rrcFilterEnabled, rrcAlpha)
      specAn.Initiate("", "")
   End Sub

   Private Sub RetrieveResults()
      ' Retrieve results 

      Dim spectrum As Spectrum(Of Single) = Nothing
      specAn.Chp.Results.FetchSpectrum("", timeout, spectrum)
      specAn.Chp.Results.FetchCarrierMeasurement("", timeout, averagePower,
                                          psd, relativePower)
   End Sub

   Private Sub PrintResults()
      Console.WriteLine("Average Channel Power (dBm)  {0}", averagePower)
      Console.WriteLine("Average Channel PSD (dBm/Hz) {0}", psd)
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
         DisplayError(ex.Message)
      End Try
   End Sub

   Private Shared Sub DisplayError(ByVal message As String)
      Console.WriteLine("ERROR:" & vbLf & message)
   End Sub
End Class
