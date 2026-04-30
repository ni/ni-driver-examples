'Steps:
'1. Open a new RFmx session.
'2. Configure the instrument properties Clock Source and Clock Frequency.
'3. Configure Selected Ports.
'4. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
'5. Select Harmonics measurement and enable the traces.
'6. Configure RBW Filter parameters.
'7. Configure Measurement Interval of the Fundamental.
'8. Configure Auto Harmonics setup.
'9A. If Harmonics Setup is Auto, configure Number of Harmonics.
'9B. If Harmonics Setup is Manual, configure Order, BW and Measurement Interval for each Harmonic using Selector String.
'10. Configure Measurement Method and Noise Compensation Enabled.
'11. Configure Averaging parameters.
'12. Initiate Harmonics Measurment.
'13. Fetch Total Harmonic Distortion[THD].
'14. Fetch Harmonic Measurement results and Power Trace for all the Harmonics using Selector String.
'15. Close the RFmx Session.

Imports NationalInstruments
Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.SpecAnMX

Namespace NationalInstruments.Examples.RFmxSpecAnHarmDynamicRange
   Public Class RFmxSpecAnHarmDynamicRange
      Private instrSession As RFmxInstrMX
      Private specAn As RFmxSpecAnMX
      Const NumberOfHarmonics As Integer = 3
      Private resourceName As String, frequencySource As String, harmonicString As String
      Private selectedPorts As String
      Private referenceLevel As Double, externalAttenuation As Double, centerFrequency As Double,
         frequency As Double, rbw As Double, rrcAlpha As Double, measurementInterval As Double, timeout As Double
      Private rbwFilterType As RFmxSpecAnMXHarmRbwFilterType
      Private autoHarmonicsSetup As RFmxSpecAnMXHarmAutoHarmonicsSetupEnabled
      Private measurementMethod As RFmxSpecAnMXHarmMeasurementMethod
      Private noiseCompensationEnabled As RFmxSpecAnMXHarmNoiseCompensationEnabled
      Private averagingEnabled As RFmxSpecAnMXHarmAveragingEnabled
      Private averagingType As RFmxSpecAnMXHarmAveragingType
      Private averagingCount As Integer

      Private harmEnabled As RFmxSpecAnMXHarmHarmonicEnabled() = New RFmxSpecAnMXHarmHarmonicEnabled(NumberOfHarmonics - 1) {}
      Private harmOrder As Integer() = New Integer(NumberOfHarmonics - 1) {}
      Private harmBandwidth As Double() = New Double(NumberOfHarmonics - 1) {}
      Private harmMeasurementInterval As Double() = New Double(NumberOfHarmonics - 1) {}

      Private totalHarmonicDistortion As Double, averageFundamentalPower As Double, fundamentalFrequency As Double
      Private harmAverageRelativePower As Double() = New Double(NumberOfHarmonics - 1) {}
      Private harmAverageAbsolutePower As Double() = New Double(NumberOfHarmonics - 1) {}
      Private harmRbw As Double() = New Double(NumberOfHarmonics - 1) {}
      Private harmFrequency As Double() = New Double(NumberOfHarmonics - 1) {}

      Private power As AnalogWaveform(Of Single)() = New AnalogWaveform(Of Single)(NumberOfHarmonics - 1) {}

      Public Sub Run()
         Try
            InitializeVariables()
            InitializeInstr()
            ConfigureSpecAn()
            RetreiveResults()
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

         frequencySource = RFmxInstrMXConstants.OnboardClock
         frequency = 10000000.0
         ' Hz

         rbwFilterType = RFmxSpecAnMXHarmRbwFilterType.Gaussian
         rbw = 100000.0
         ' Hz
         rrcAlpha = 0.01
         measurementInterval = 0.001
         ' seconds

         autoHarmonicsSetup = RFmxSpecAnMXHarmAutoHarmonicsSetupEnabled.True

         For i As Integer = 0 To NumberOfHarmonics - 1
            harmEnabled(i) = RFmxSpecAnMXHarmHarmonicEnabled.True
            harmOrder(i) = i + 1
            harmBandwidth(i) = 100000.0
            ' Hz
            harmMeasurementInterval(i) = 0.001
            ' seconds
         Next

         measurementMethod = RFmxSpecAnMXHarmMeasurementMethod.DynamicRange
         noiseCompensationEnabled = RFmxSpecAnMXHarmNoiseCompensationEnabled.True

         averagingEnabled = RFmxSpecAnMXHarmAveragingEnabled.False
         averagingCount = 10
         averagingType = RFmxSpecAnMXHarmAveragingType.Rms

         timeout = 10.0
         ' seconds
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
         specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Harm, True)
         specAn.Harm.Configuration.ConfigureFundamentalRbw("", rbw, rbwFilterType, rrcAlpha)
         specAn.Harm.Configuration.ConfigureFundamentalMeasurementInterval("", measurementInterval)
         specAn.Harm.Configuration.ConfigureAutoHarmonics("", autoHarmonicsSetup)
         If autoHarmonicsSetup = RFmxSpecAnMXHarmAutoHarmonicsSetupEnabled.True Then
            specAn.Harm.Configuration.ConfigureNumberOfHarmonics("", NumberOfHarmonics)
         Else
            specAn.Harm.Configuration.ConfigureNumberOfHarmonics("", NumberOfHarmonics)
            specAn.Harm.Configuration.ConfigureHarmonicArray("", harmOrder, harmBandwidth, harmEnabled,
               harmMeasurementInterval)
         End If
         specAn.Harm.Configuration.SetMeasurementMethod("", measurementMethod)
         specAn.Harm.Configuration.SetNoiseCompensationEnabled("", noiseCompensationEnabled)
         specAn.Harm.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)
         specAn.Initiate("", "")
      End Sub

      Private Sub RetreiveResults()
         specAn.Harm.Results.FetchTotalHarmonicDistortion("", timeout, totalHarmonicDistortion,
            averageFundamentalPower, fundamentalFrequency)

         For i As Integer = 0 To NumberOfHarmonics - 1
            harmonicString = RFmxSpecAnMX.BuildHarmonicString2("", i)
            specAn.Harm.Results.FetchHarmonicPowerTrace(harmonicString, timeout, power(i))
         Next

         specAn.Harm.Results.FetchHarmonicMeasurementArray("", timeout, harmAverageRelativePower,
            harmAverageAbsolutePower, harmRbw, harmFrequency)
      End Sub

      Private Sub PrintResults()
         Console.WriteLine("Measurement" & vbLf)
         Console.WriteLine("Total Harmonic Distoration (%)     : {0}", totalHarmonicDistortion)
         Console.WriteLine("Average Fundamental Power (dBm)    : {0}", averageFundamentalPower)
         Console.WriteLine("Fundamental Frequency (Hz)         : {0}", fundamentalFrequency)

         Console.WriteLine(vbLf & "----------------Harmonics----------------------" & vbLf)
         For i As Integer = 0 To NumberOfHarmonics - 1
            Console.WriteLine("Harmonic {0}:", i + 1)
            Console.WriteLine("Harmonics Frequency    (Hz)       : {0}", harmFrequency(i))
            Console.WriteLine("Harmonics RBW          (Hz)       : {0}", harmRbw(i))
            Console.WriteLine("Average Absolute Power (dBm)      : {0}", harmAverageAbsolutePower(i))
            Console.WriteLine("Average Relative Power (dB)       : {0}", harmAverageRelativePower(i))
            Console.WriteLine("---------------------------------------------" & vbLf)
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
