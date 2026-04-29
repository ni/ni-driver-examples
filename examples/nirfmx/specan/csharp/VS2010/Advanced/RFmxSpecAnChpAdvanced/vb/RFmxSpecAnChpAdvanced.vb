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

Class RFmxSpecAnChpAdvanced
   Private instrSession As RFmxInstrMX
   Private specAn As RFmxSpecAnMX
   Private resourceName As String, frequencySource As String
   Private selectedPorts As String
   Private centerFrequency As Double, referenceLevel As Double, externalAttenuation As Double, frequency As Double,
      span As Double, rbw As Double, sweepTimeInterval As Double, fftPadding As Double, timeout As Double
   Private enableAllTraces As Boolean
   Private rbwFilterType As RFmxSpecAnMXChpRbwFilterType
   Private rbwAuto As RFmxSpecAnMXChpRbwAutoBandwidth
   Private sweepTimeAuto As RFmxSpecAnMXChpSweepTimeAuto
   Private averagingEnabled As RFmxSpecAnMXChpAveragingEnabled
   Private averagingCount As Integer
   Private averagingType As RFmxSpecAnMXChpAveragingType
   Private fftWindow As RFmxSpecAnMXChpFftWindow

   Const NumberOfCarriers As Integer = 1

   Private totalCarrierPower As Double = 0.0

   Private Structure CarrierChannel
      Public carrierFrequency As Double, integrationBandwidth As Double, rrcFilterAlpha As Double
      Public rrcFilterEnabled As RFmxSpecAnMXChpCarrierRrcFilterEnabled
   End Structure

   Private Structure CarrierMeasurement
      Public absolutePower As Double
      Public psd As Double
      Public relativePower As Double
   End Structure

   Private carrierCh As CarrierChannel() = New CarrierChannel(NumberOfCarriers - 1) {}
   Private carrierMeas As CarrierMeasurement() = New CarrierMeasurement(NumberOfCarriers - 1) {}

   Friend Sub Run()
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
      ' Initialize input variables 

      resourceName = "RFSA"
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

      'Sweep time
      sweepTimeAuto = RFmxSpecAnMXChpSweepTimeAuto.[True]
      sweepTimeInterval = 0.001
      ' seconds 

      'Averaging
      averagingEnabled = RFmxSpecAnMXChpAveragingEnabled.[False]
      averagingCount = 10
      averagingType = RFmxSpecAnMXChpAveragingType.Rms

      'RBW Filter
      rbwFilterType = RFmxSpecAnMXChpRbwFilterType.Gaussian
      rbwAuto = RFmxSpecAnMXChpRbwAutoBandwidth.[True]
      rbw = 10000.0
      ' Hz

      'FFT
      fftWindow = RFmxSpecAnMXChpFftWindow.FlatTop
      fftPadding = -1.0

      enableAllTraces = True

      'Set up the carrier channel inputs
      For i As Integer = 0 To NumberOfCarriers - 1
         carrierCh(i).carrierFrequency = 0.0
         ' Hz
         carrierCh(i).integrationBandwidth = 1000000.0
         ' Hz
         carrierCh(i).rrcFilterEnabled = RFmxSpecAnMXChpCarrierRrcFilterEnabled.[False]
         carrierCh(i).rrcFilterAlpha = 0.22
      Next
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
      specAn.Chp.Configuration.ConfigureSpan("", span)
      specAn.Chp.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval)
      specAn.Chp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)
      specAn.Chp.Configuration.ConfigureRbwFilter("", rbwAuto, rbw, rbwFilterType)
      specAn.Chp.Configuration.ConfigureFft("", fftWindow, fftPadding)
      specAn.Chp.Configuration.ConfigureNumberOfCarriers("", NumberOfCarriers)

      Dim selectorString As String
      For i As Integer = 0 To NumberOfCarriers - 1
         selectorString = RFmxSpecAnMX.BuildCarrierString2("", i)
         specAn.Chp.Configuration.ConfigureCarrierOffset(selectorString, carrierCh(i).carrierFrequency)
         specAn.Chp.Configuration.ConfigureIntegrationBandwidth(selectorString, carrierCh(i).integrationBandwidth)
         specAn.Chp.Configuration.ConfigureRrcFilter(selectorString, carrierCh(i).rrcFilterEnabled,
                                                     carrierCh(i).rrcFilterAlpha)
      Next

      specAn.Initiate("", "")
   End Sub

   Private Sub RetrieveResults()
      ' Retrieve results 

      Dim spectrum As Spectrum(Of Single) = Nothing
      specAn.Chp.Results.FetchSpectrum("", timeout, spectrum)
      specAn.Chp.Results.FetchTotalCarrierPower("", timeout, totalCarrierPower)

      Dim selectorString As String
      For i As Integer = 0 To NumberOfCarriers - 1
         selectorString = RFmxSpecAnMX.BuildCarrierString2("", i)
         specAn.Chp.Results.FetchCarrierMeasurement(selectorString, timeout, carrierMeas(i).absolutePower,
                                                    carrierMeas(i).psd, carrierMeas(i).relativePower)
      Next
   End Sub

   Private Sub PrintResults()
      Console.WriteLine("Total Carrier Power (dBm) {0}", totalCarrierPower)
      Console.WriteLine(vbLf & "Carrier Measurements")
      For i As Integer = 0 To NumberOfCarriers - 1
         Console.WriteLine(vbLf & "Carrier : {0}", i)
         Console.WriteLine("Absolute power (dBm)     {0}", carrierMeas(i).absolutePower)
         Console.WriteLine("PSD (dBm/Hz)             {0}", carrierMeas(i).psd)
         Console.WriteLine("Relative Power (dB)      {0}", carrierMeas(i).relativePower)
         Console.WriteLine("-----------------------------------------------------------")
      Next
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
