'Steps :
'RFmxSpecAn IM Example
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
'11. Configure Frequency definition
'12. Configure Measurement Method
'13. Configure Fundamental tones
'14. Configure Auto Intermods Setup Enabled
'15. Configure Number of Intermods
'16. Configure Intermod (Array)
'17. Initiate Measurement
'18. Fetch IM Measurements and Trace
'19. Close RFmx Session

Imports NationalInstruments
Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.SpecAnMX

Namespace NationalInstruments.Examples.RFmxSpecAnIMAdvanced

   Class RFmxSpecAnIMAdvanced
      Private instrSession As RFmxInstrMX
      Private specAn As RFmxSpecAnMX
      Const numberOfIntermods As Integer = 1
      Private selectedPorts As String
      Private centerFrequency As Double, frequency As Double, referenceLevel As Double, externalAttenuation As Double, rfAttenuation As Double, rbw As Double
      Private sweepTimeInterval As Double, fftPadding As Double, lowerToneFrequency As Double, upperToneFrequency As Double, timeout As Double
      Private lowerTonePower As Double, upperTonePower As Double
      Private resourceName As String, frequencySource As String
      Private enableAllTraces As Boolean
      Private rfAttenuationAuto As RFmxInstrMXRFAttenuationAuto

      Private averagingEnabled As RFmxSpecAnMXIMAveragingEnabled
      Private averagingType As RFmxSpecAnMXIMAveragingType
      Private averagingCount As Integer

      Private rbwAuto As RFmxSpecAnMXIMRbwFilterAutoBandwidth
      Private rbwFilterType As RFmxSpecAnMXIMRbwFilterType
      Private frequencyDefinition As RFmxSpecAnMXIMFrequencyDefinition

      Private sweepTimeAuto As RFmxSpecAnMXIMSweepTimeAuto
      Private fftWindow As RFmxSpecAnMXIMFftWindow
      Private measurementMethod As RFmxSpecAnMXIMMeasurementMethod

      Private autoIntermodsSetupEnabled As RFmxSpecAnMXIMAutoIntermodsSetupEnabled
      Private maximumIntermodOrder As Integer
      Private actualNumberOfIntermods As Integer

      Private order As Integer() = New Integer(numberOfIntermods - 1) {}
      Private side As RFmxSpecAnMXIMIntermodSide() = New RFmxSpecAnMXIMIntermodSide(numberOfIntermods - 1) {}
      Private enabled As RFmxSpecAnMXIMIntermodEnabled() = New RFmxSpecAnMXIMIntermodEnabled(numberOfIntermods - 1) {}
      Private lowerIntermodFrequency As Double() = New Double(numberOfIntermods - 1) {}
      Private upperIntermodFrequency As Double() = New Double(numberOfIntermods - 1) {}

      Private intermodOrder As Integer() = New Integer(numberOfIntermods - 1) {}
      Private lowerIntermodPower As Double() = New Double(numberOfIntermods - 1) {}
      Private upperIntermodPower As Double() = New Double(numberOfIntermods - 1) {}
      Private worstCaseOutputInterceptPower As Double() = New Double(numberOfIntermods - 1) {}
      Private lowerOutputInterceptPower As Double() = New Double(numberOfIntermods - 1) {}
      Private upperOutputInterceptPower As Double() = New Double(numberOfIntermods - 1) {}

      Private spectrum As Spectrum(Of Single)()
      Private numberOfSpectrums As Integer

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
         centerFrequency = 1000000000.0      ' Hz 
         referenceLevel = 0.0                ' dBm 
         externalAttenuation = 0.0           ' dB 
         timeout = 10.0                      ' seconds 

         frequencySource = RFmxInstrMXConstants.OnboardClock
         frequency = 10000000.0              ' Hz 

         rfAttenuationAuto = RFmxInstrMXRFAttenuationAuto.[True]
         rfAttenuation = 10.0                ' dB 

         enableAllTraces = True

         'Averaging 
         averagingEnabled = RFmxSpecAnMXIMAveragingEnabled.[False]
         averagingCount = 10
         averagingType = RFmxSpecAnMXIMAveragingType.Rms

         ' RBW Filter
         rbwFilterType = RFmxSpecAnMXIMRbwFilterType.Gaussian
         rbwAuto = RFmxSpecAnMXIMRbwFilterAutoBandwidth.[True]
         rbw = 10000.0                       ' Hz 

         ' Sweep Time
         sweepTimeAuto = RFmxSpecAnMXIMSweepTimeAuto.[True]
         sweepTimeInterval = 0.001           ' seconds 

         ' FFT
         fftWindow = RFmxSpecAnMXIMFftWindow.FlatTop
         fftPadding = -1.0

         'Frequency Definition
         frequencyDefinition = RFmxSpecAnMXIMFrequencyDefinition.Relative

         'Measurement Method
         measurementMethod = RFmxSpecAnMXIMMeasurementMethod.Normal

         'Fundamental Tones
         lowerToneFrequency = -1000000.0         ' Hz 
         upperToneFrequency = 1000000.0          ' Hz 

         'Auto Intermods Setup
         autoIntermodsSetupEnabled = RFmxSpecAnMXIMAutoIntermodsSetupEnabled.[True]
         maximumIntermodOrder = 3

         'Intermods
         For i As Integer = 0 To numberOfIntermods - 1
            enabled(i) = RFmxSpecAnMXIMIntermodEnabled.[True]
            order(i) = 3
            side(i) = RFmxSpecAnMXIMIntermodSide.Both
            lowerIntermodFrequency(i) = -3000000.0              ' Hz 
            upperIntermodFrequency(i) = 3000000.0              ' Hz
         Next
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
         specAn.IM.Configuration.ConfigureFrequencyDefinition("", frequencyDefinition)
         specAn.IM.Configuration.ConfigureMeasurementMethod("", measurementMethod)
         specAn.IM.Configuration.ConfigureFundamentalTones("", lowerToneFrequency, upperToneFrequency)
         specAn.IM.Configuration.ConfigureAutoIntermodsSetup("", autoIntermodsSetupEnabled, maximumIntermodOrder)

         If autoIntermodsSetupEnabled = RFmxSpecAnMXIMAutoIntermodsSetupEnabled.[False] Then
            specAn.IM.Configuration.ConfigureNumberOfIntermods("", numberOfIntermods)


            specAn.IM.Configuration.ConfigureIntermodArray("", order, lowerIntermodFrequency, upperIntermodFrequency, side, enabled)
         End If

         specAn.Initiate("", "")
      End Sub

      Private Sub RetrieveResults()
         ' Retrieve results 

         specAn.IM.Configuration.GetNumberOfIntermods("", actualNumberOfIntermods)
         If measurementMethod = RFmxSpecAnMXIMMeasurementMethod.Normal Then
            numberOfSpectrums = 1
         Else
            numberOfSpectrums = 2 * actualNumberOfIntermods + 2
         End If

         spectrum = New Spectrum(Of Single)(numberOfSpectrums - 1) {}
         specAn.IM.Results.FetchFundamentalMeasurement("", timeout, lowerTonePower, upperTonePower)
         specAn.IM.Results.FetchIntermodMeasurementArray("", timeout, intermodOrder, lowerIntermodPower, upperIntermodPower)
         specAn.IM.Results.FetchInterceptPowerArray("", timeout, intermodOrder, worstCaseOutputInterceptPower, lowerOutputInterceptPower, upperOutputInterceptPower)
         For spectrumIndex As Integer = 0 To numberOfSpectrums - 1
            specAn.IM.Results.FetchSpectrum("", timeout, spectrumIndex, spectrum(spectrumIndex))
         Next
      End Sub

      Private Sub PrintResults()
         ' Display the results 

         Console.WriteLine("Fundamental Tone Measurement              " & vbLf)
         Console.WriteLine("Lower Tone Power(dBm)                    : {0}", lowerTonePower)
         Console.WriteLine("Upper Tone Power(dBm)                    : {0}", upperTonePower)

         Console.WriteLine(vbLf & "Intermod Measurements                     " & vbLf)

         For i As Integer = 0 To actualNumberOfIntermods - 1
            Console.WriteLine(vbLf & "Intermod Measurement                     : {0}", i)
            Console.WriteLine("Order                                    : {0}", intermodOrder(i))
            Console.WriteLine("Lower Intermod Power(dBm)                : {0}", lowerIntermodPower(i))
            Console.WriteLine("Upper Intermod Power(dBm)                : {0}", upperIntermodPower(i))
            Console.WriteLine("Lower Output Intercept Power(dBm)        : {0}", lowerOutputInterceptPower(i))
            Console.WriteLine("Upper Output Intercept Power(dBm)        : {0}", upperOutputInterceptPower(i))
            Console.WriteLine("Worst Case Output Intercept Power(dBm)   : {0}", worstCaseOutputInterceptPower(i))
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
End Namespace
