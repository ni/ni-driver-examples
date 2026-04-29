'Steps:
'1. Open a new RFmx session.
'2. Configure the instrument properties Clock Source and Clock Frequency.
'3. Configure Selected Ports.
'4. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
'5. Select Harmonics measurement and enable the traces.
'6. Configure RBW Filter parameters.
'7. Configure Measurement Interval of the Fundamental signal.
'8. Configure Number of Harmonics to measure and Auto Harmonics setup.
'9. Configure the parameters of the Harmonics (Measurement Interval, Order, BW And Harmonics Enabled).
'10. Configure Averaging parameters.
'11. Initiate Measurement.
'12. Fetch Harm Measurements and Traces.
'13. Close the RFmx Session.

Imports NationalInstruments
Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.SpecAnMX

Namespace NationalInstruments.Examples.RFmxSpecAnHarm
   Public Class RFmxSpecAnHarm
      Private instrSession As RFmxInstrMX
      Private specAn As RFmxSpecAnMX
      Private resourceName As [String], frequencySource As [String]
      Private selectedPorts As String, harmonicString As String
      Private referenceLevel As Double, externalAttenuation As Double, centerFrequency As Double,
              frequency As Double, rbw As Double, rrcAlpha As Double, measurementInterval As Double
      Private rbwFilterType As RFmxSpecAnMXHarmRbwFilterType
      Private autoHarmonicsSetup As RFmxSpecAnMXHarmAutoHarmonicsSetupEnabled
      Private averagingEnabled As RFmxSpecAnMXHarmAveragingEnabled
      Private averagingType As RFmxSpecAnMXHarmAveragingType
      Private averagingCount As Integer

      Const NumberOfHarmonics As Integer = 3

      Private harmonicsEnabled As RFmxSpecAnMXHarmHarmonicEnabled() =
              New RFmxSpecAnMXHarmHarmonicEnabled(NumberOfHarmonics - 1) {}
      Private harmonicsBandwidth As Double() = New Double(NumberOfHarmonics - 1) {}
      Private harmonicsOrder As Integer() = New Integer(NumberOfHarmonics - 1) {}
      Private harmonicsMeasurementInterval As Double() = New Double(NumberOfHarmonics - 1) {}

      Private totalHarmonicDistortion As Double, averageFundamentalPower As Double,
              fundamentalFrequency As Double, timeout As Double
      Private averageRelativePower As Double()
      Private averageAbsolutePower As Double()
      Private harmonicsRbw As Double()
      Private harmonicsFrequency As Double()

      Private power As AnalogWaveform(Of Single)() = New AnalogWaveform(Of Single)(NumberOfHarmonics - 1) {}

      Public Sub Run()
         Try
            InitializeVariables()
            InitializeInstr()
            ConfigureSpecAn()
            RetreiveResults()
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
         resourceName = "RFSA"

         selectedPorts = ""

         centerFrequency = 1000000000.0
         ' Hz
         referenceLevel = 0.0
         ' dBm
         externalAttenuation = 0.0
         ' dB

         frequencySource = "OnboardClock"
         frequency = 10000000.0
         ' Hz

         rbw = 100000.0
         ' Hz
         rbwFilterType = RFmxSpecAnMXHarmRbwFilterType.Gaussian
         rrcAlpha = 0.01
         measurementInterval = 0.001
         ' seconds

         autoHarmonicsSetup = RFmxSpecAnMXHarmAutoHarmonicsSetupEnabled.True

         For i As Integer = 0 To NumberOfHarmonics - 1
            harmonicsEnabled(i) = RFmxSpecAnMXHarmHarmonicEnabled.True
            harmonicsOrder(i) = i + 1
            harmonicsBandwidth(i) = 100000.0
            ' Hz
            harmonicsMeasurementInterval(i) = 0.001
            ' seconds
         Next

         averagingEnabled = RFmxSpecAnMXHarmAveragingEnabled.False
         averagingCount = 10
         averagingType = RFmxSpecAnMXHarmAveragingType.Rms

         timeout = 10.0
         ' second
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
         specAn.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
         specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Harm, True)
         specAn.Harm.Configuration.ConfigureFundamentalRbw("", rbw, rbwFilterType, rrcAlpha)
         specAn.Harm.Configuration.ConfigureFundamentalMeasurementInterval("", measurementInterval)
         specAn.Harm.Configuration.ConfigureAutoHarmonics("", autoHarmonicsSetup)
         If autoHarmonicsSetup = RFmxSpecAnMXHarmAutoHarmonicsSetupEnabled.True Then
            specAn.Harm.Configuration.ConfigureNumberOfHarmonics("", NumberOfHarmonics)
         Else
            specAn.Harm.Configuration.ConfigureNumberOfHarmonics("", NumberOfHarmonics)
            specAn.Harm.Configuration.ConfigureHarmonicArray("", harmonicsOrder,
               harmonicsBandwidth, harmonicsEnabled, harmonicsMeasurementInterval)
         End If

         specAn.Harm.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)
         specAn.Initiate("", "")
      End Sub

      Private Sub RetreiveResults()
         specAn.Harm.Results.FetchTotalHarmonicDistortion("", timeout, totalHarmonicDistortion,
            averageFundamentalPower, fundamentalFrequency)

         specAn.Harm.Results.FetchHarmonicMeasurementArray("", timeout, averageRelativePower,
            averageAbsolutePower, harmonicsRbw, harmonicsFrequency)

         For i As Integer = 0 To NumberOfHarmonics - 1
            harmonicString = RFmxSpecAnMX.BuildHarmonicString2("", i)
            specAn.Harm.Results.FetchHarmonicPowerTrace(harmonicString, timeout, power(i))
         Next
      End Sub

      Private Sub PrintResults()
         Console.WriteLine("Total Harmonic Distoration (%)   : {0}", totalHarmonicDistortion)
         Console.WriteLine("Average Fundamental Power (dBm)  : {0}", averageFundamentalPower)
         Console.WriteLine("Fundamental Frequency (Hz)       : {0}", fundamentalFrequency)

         Console.WriteLine(vbLf & "-----------------Harmonics----------------------" & vbLf)
         For i As Integer = 0 To NumberOfHarmonics - 1
            Console.WriteLine("Harmonic {0}", i + 1)
            Console.WriteLine("Harmonics Frequency    (Hz)  : {0}", harmonicsFrequency(i))
            Console.WriteLine("Harmonics RBW          (Hz)  : {0}", harmonicsRbw(i))
            Console.WriteLine("Average Absolute Power (dBm) : {0}", averageAbsolutePower(i))
            Console.WriteLine("Average Relative Power (dB)  : {0}", averageRelativePower(i))
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
            DisplayError(ex.Message)
         End Try
      End Sub

      Private Sub DisplayError(message As String)
         Console.WriteLine("ERROR:" & vbLf & message)
      End Sub
   End Class
End Namespace
