'Steps:
'RFmxSpecAn IM TOI Example
'1. Open a new RFmx session
'2. Configure Selected Ports
'3. Configure the Center Frequency
'4. Configure the basic instrument properties (Clock Source, Clock Frequency)
'5. Configure the basic signal properties  (Reference Level, External Attenuation and RF Attenuation)
'6. Select IM measurement and enable the traces
'7. Configure Averaging
'8. Configure RBW Filter parameters
'9. Configure Sweep Time
'10. Configure FFT
'11. Configure Measurement Method
'12. Configure Fundamental tones
'13. Configure auto setup of third order intermod frequencies
'14. Initiate Measurement
'15. Fetch IM Measurements and Trace
'16. Close RFmx Session

Imports NationalInstruments
Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.SpecAnMX

Namespace NationalInstruments.Examples.RFmxSpecAnTOI
   Class RFmxSpecAnTOI
      Private instrSession As RFmxInstrMX
      Private specAn As RFmxSpecAnMX
      Const maximumNumberOfSpectrums As Integer = 4
      Private selectedPorts As String
      Private centerFrequency As Double, frequency As Double, referenceLevel As Double,
          externalAttenuation As Double, rfAttenuation As Double, rbw As Double
      Private sweepTimeInterval As Double, fftPadding As Double, lowerToneFrequency As Double,
          upperToneFrequency As Double, timeout As Double
      Private lowerTonePower As Double, upperTonePower As Double, lowerIntermodPower As Double,
          upperIntermodPower As Double, worstCaseOutputInterceptPower As Double, lowerOutputInterceptPower As Double,
       upperOutputInterceptPower As Double
      Private resourceName As String, frequencySource As String
      Private enableAllTraces As Boolean
      Private rfAttenuationAuto As RFmxInstrMXRFAttenuationAuto

      Private averagingEnabled As RFmxSpecAnMXIMAveragingEnabled
      Private averagingType As RFmxSpecAnMXIMAveragingType
      Private averagingCount As Integer, intermodOrder As Integer

      Private rbwAuto As RFmxSpecAnMXIMRbwFilterAutoBandwidth
      Private rbwFilterType As RFmxSpecAnMXIMRbwFilterType

      Private sweepTimeAuto As RFmxSpecAnMXIMSweepTimeAuto
      Private fftWindow As RFmxSpecAnMXIMFftWindow
      Private measurementMethod As RFmxSpecAnMXIMMeasurementMethod

      Private autoIntermodsSetupEnabled As RFmxSpecAnMXIMAutoIntermodsSetupEnabled

      Private spectrum As Spectrum(Of Single)()
      Private numberOfSpectrums As Integer
      Private maximumIntermodOrder As Integer


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
         centerFrequency = 1000000000.0          ' Hz 
         referenceLevel = 0.0                    ' dBm 
         externalAttenuation = 0.0               ' dB 
         timeout = 10.0                          ' seconds 

         frequencySource = RFmxInstrMXConstants.OnboardClock
         frequency = 10000000.0                  ' Hz 

         rfAttenuationAuto = RFmxInstrMXRFAttenuationAuto.[True]
         rfAttenuation = 10.0                    ' dB 

         enableAllTraces = True

         'Averaging 
         averagingEnabled = RFmxSpecAnMXIMAveragingEnabled.[False]
         averagingCount = 10
         averagingType = RFmxSpecAnMXIMAveragingType.Rms

         ' RBW Filter
         rbwFilterType = RFmxSpecAnMXIMRbwFilterType.Gaussian
         rbwAuto = RFmxSpecAnMXIMRbwFilterAutoBandwidth.[True]
         rbw = 10000.0                          ' Hz 

         ' Sweep Time
         sweepTimeAuto = RFmxSpecAnMXIMSweepTimeAuto.[True]
         sweepTimeInterval = 0.001              ' seconds 

         ' FFT
         fftWindow = RFmxSpecAnMXIMFftWindow.FlatTop
         fftPadding = -1.0

         'Measurement Method
         measurementMethod = RFmxSpecAnMXIMMeasurementMethod.Normal

         'Fundamental Tones
         lowerToneFrequency = -1000000.0          ' Hz 
         upperToneFrequency = 1000000.0           ' Hz 

         'Auto Intermods Setup
         autoIntermodsSetupEnabled = RFmxSpecAnMXIMAutoIntermodsSetupEnabled.[True]
         maximumIntermodOrder = 3

         If measurementMethod = RFmxSpecAnMXIMMeasurementMethod.Normal Then
            numberOfSpectrums = 1
         Else
            numberOfSpectrums = maximumNumberOfSpectrums
         End If
      End Sub

      Private Sub InitializeInstr()
         ' Create a new RFmx Session 

         instrSession = New RFmxInstrMX(resourceName, "")
      End Sub

      Private Sub ConfigureSpecAn()
         ' Get SpecAn signal 

         specAn = instrSession.GetSpecAnSignalConfiguration()

         specAn.ConfigureFrequency("", centerFrequency)
         instrSession.ConfigureFrequencyReference("", frequencySource, frequency)
         specAn.SetSelectedPorts("", selectedPorts)
         specAn.ConfigureReferenceLevel("", referenceLevel)
         specAn.ConfigureExternalAttenuation("", externalAttenuation)
         instrSession.ConfigureRFAttenuation("", rfAttenuationAuto, rfAttenuation)
         specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.IM, enableAllTraces)

         specAn.IM.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)
         specAn.IM.Configuration.ConfigureRbwFilter("", rbwAuto, rbw, rbwFilterType)
         specAn.IM.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval)
         specAn.IM.Configuration.ConfigureFft("", fftWindow, fftPadding)
         specAn.IM.Configuration.ConfigureMeasurementMethod("", measurementMethod)
         specAn.IM.Configuration.ConfigureFundamentalTones("", lowerToneFrequency, upperToneFrequency)
         specAn.IM.Configuration.ConfigureAutoIntermodsSetup("", autoIntermodsSetupEnabled, maximumIntermodOrder)

         specAn.Initiate("", "")
      End Sub

      Private Sub RetrieveResults()
         ' Retrieve results 

         specAn.IM.Results.FetchFundamentalMeasurement("", timeout, lowerTonePower, upperTonePower)
         specAn.IM.Results.FetchIntermodMeasurement("", timeout, intermodOrder, lowerIntermodPower, upperIntermodPower)
         specAn.IM.Results.FetchInterceptPower("", timeout, intermodOrder, worstCaseOutputInterceptPower,
                                               lowerOutputInterceptPower, upperOutputInterceptPower)

         spectrum = New Spectrum(Of Single)(numberOfSpectrums - 1) {}
         For spectrumIndex As Integer = 0 To numberOfSpectrums - 1
            specAn.IM.Results.FetchSpectrum("", timeout, spectrumIndex, spectrum(spectrumIndex))
         Next
      End Sub

      Private Sub PrintResults()
         ' Display the results 

         Console.WriteLine("Fundamental Tone Measurement " & vbLf)
         Console.WriteLine("Lower Tone Power(dBm)      :{0}", lowerTonePower)
         Console.WriteLine("Upper Tone Power(dBm)      :{0}", upperTonePower)

         Console.WriteLine(vbLf & "Intermod Measurement         " & vbLf)
         Console.WriteLine("Lower Intermod Power(dBm)  :{0}", lowerIntermodPower)
         Console.WriteLine("Upper Intermod Power(dBm)  :{0}", upperIntermodPower)
         Console.WriteLine("Lower TOI(dBm)             :{0}", lowerOutputInterceptPower)
         Console.WriteLine("Upper TOI(dBm)             :{0}", upperOutputInterceptPower)
         Console.WriteLine("Worst Case TOI(dBm)        :{0}", worstCaseOutputInterceptPower)
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
End Namespace
