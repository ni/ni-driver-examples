'Steps:
'1. Open a new RFmx session
'2. Configure the instrument properties Clock Source and Clock Frequency
'3. Configure Selected Ports
'4. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
'5. Select OBW measurement and enable the traces
'6. Configure OBW Bandwidth Percentange, Span and Sweep Time
'7. Configure OBW Averaging
'8. Configure OBW RBW filter
'9. Configure OBW FFT
'10. Configure OBW Power Units
'11. Initiate Measurement
'12. Fetch OBW Measurement and Traces
'13. Close the RFmx Session

Imports NationalInstruments
Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.SpecAnMX

Public Class RFmxSpecAnObw
   Private instrSession As RFmxInstrMX
   Private specAn As RFmxSpecAnMX
   Private resourceName As String, frequencySource As String
   Private selectedPorts As String
   Private centerFrequency As Double, referenceLevel As Double, externalAttenuation As Double,
           fftPadding As Double, rbw As Double, timeOut As Double, frequency As Double,
           span As Double, bandWidthPercentage As Double, sweepTimeInterval As Double
   Private averagingEnabled As RFmxSpecAnMXObwAveragingEnabled
   Private averagingType As RFmxSpecAnMXObwAveragingType
   Private rbwFilterType As RFmxSpecAnMXObwRbwFilterType
   Private rbwAutoBandwidth As RFmxSpecAnMXObwRbwAutoBandwidth
   Private powerUnits As RFmxSpecAnMXObwPowerUnits
   Private fftWindow As RFmxSpecAnMXObwFftWindow
   Private sweepTimeAuto As RFmxSpecAnMXObwSweepTimeAuto
   Private averagingCount As Integer

   Private stopFrequency As Double, startFrequency As Double, occupiedBandwidth As Double,
           averagePower As Double, frequencyResolution As Double

   Public Sub Run()
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
      centerFrequency = 1000000000.0      ' Hz 
      referenceLevel = 0.0                ' dBm 
      externalAttenuation = 0.0           ' dB 

      frequencySource = RFmxInstrMXConstants.OnboardClock
      frequency = 10000000.0              ' Hz 

      span = 1000000.0                    ' Hz 
      bandWidthPercentage = 99.0
      powerUnits = RFmxSpecAnMXObwPowerUnits.dBm

      'RBW Filter
      rbwFilterType = RFmxSpecAnMXObwRbwFilterType.Gaussian
      rbw = 10000.0
      rbwAutoBandwidth = RFmxSpecAnMXObwRbwAutoBandwidth.[True]

      'Sweep Time
      sweepTimeAuto = RFmxSpecAnMXObwSweepTimeAuto.[True]
      sweepTimeInterval = 0.001
      ' seconds 

      'Averaging
      averagingEnabled = RFmxSpecAnMXObwAveragingEnabled.[False]
      averagingCount = 10
      averagingType = RFmxSpecAnMXObwAveragingType.Rms

      'FFT
      fftWindow = RFmxSpecAnMXObwFftWindow.FlatTop
      fftPadding = -1.0

      timeOut = 10                    ' seconds 
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
      specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Obw, True)
      specAn.Obw.Configuration.ConfigureBandwidthPercentage("", bandWidthPercentage)
      specAn.Obw.Configuration.ConfigureSpan("", span)
      specAn.Obw.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval)
      specAn.Obw.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount,
                                                  averagingType)
      specAn.Obw.Configuration.ConfigureRbwFilter("", rbwAutoBandwidth, rbw, rbwFilterType)
      specAn.Obw.Configuration.ConfigureFft("", fftWindow, fftPadding)
      specAn.Obw.Configuration.ConfigurePowerUnits("", powerUnits)

      specAn.Initiate("", "")
   End Sub

   Private Sub RetrieveResults()
      ' Retrieve results 

      Dim spectrum As Spectrum(Of Single) = Nothing
      specAn.Obw.Results.FetchSpectrumTrace("", timeOut, spectrum)
      specAn.Obw.Results.FetchMeasurement("", timeOut, occupiedBandwidth, averagePower,
                                          frequencyResolution, startFrequency,
                                          stopFrequency)
   End Sub

   Private Sub PrintResults()
      Console.WriteLine("Occupied Bandwidth (Hz)       {0}", occupiedBandwidth)
      Console.WriteLine("Average Power (dBm or dBm/Hz) {0}", averagePower)
      Console.WriteLine("Frequency Resolution (Hz)     {0}", frequencyResolution)
      Console.WriteLine("Start Frequency (Hz)          {0}", startFrequency)
      Console.WriteLine("Stop Frequency (Hz)           {0}", stopFrequency)
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
