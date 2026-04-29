'Steps:
'1. Open a new RFmx session
'2. Configure the instrument properties Clock Source and Clock Frequency
'3. Configure Selected Ports
'4. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
'5. Select FCnt measurement and enable the traces
'6. Configure FCnt Measurement Interval
'7. Configure FCnt RBW filter
'8. Configure FCnt Averaging
'9. Configure FCnt Threshold
'10. Initiate Measurment
'11. Fetch FCnt Measurements and Traces
'12. Close the RFmx Session

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.SpecAnMX

Public Class RFmxSpecAnFcnt
   Private instrSession As RFmxInstrMX
   Private specAn As RFmxSpecAnMX

   Private selectedPorts As String
   Private centerFrequency As Double, referenceLevel As Double, externalAttenuation As Double,
           frequency As Double, measurementInterval As Double, rbw As Double,
           timeout As Double, rrcAlpha As Double, thresholdLevel As Double
   Private resourceName As String, frequencySource As String
   Private averagingEnabled As RFmxSpecAnMXFcntAveragingEnabled
   Private averagingCount As Integer
   Private averagingType As RFmxSpecAnMXFcntAveragingType
   Private rbwFilterType As RFmxSpecAnMXFcntRbwFilterType
   Private thresholdEnabled As RFmxSpecAnMXFcntThresholdEnabled
   Private thresholdType As RFmxSpecAnMXFcntThresholdType

   Private averageRelativeFrequency As Double ' Hz 
   Private averageAbsoluteFrequency As Double ' Hz 
   Private meanPhase As Double

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
      centerFrequency = 1000000000.0 ' Hz 
      referenceLevel = 0.0 ' dBm 
      externalAttenuation = 0.0 ' dB 

      frequencySource = RFmxInstrMXConstants.OnboardClock
      frequency = 10000000.0 ' Hz 

      measurementInterval = 0.001 ' seconds 
      timeout = 10 ' seconds 

      'RBW Filter
      rbw = 100000.0 ' Hz 
      rbwFilterType = RFmxSpecAnMXFcntRbwFilterType.None
      rrcAlpha = 0.1

      'Averaging
      averagingType = RFmxSpecAnMXFcntAveragingType.Mean
      averagingEnabled = RFmxSpecAnMXFcntAveragingEnabled.[False]
      averagingCount = 10

      'Threshold
      thresholdEnabled = RFmxSpecAnMXFcntThresholdEnabled.[False]
      thresholdType = RFmxSpecAnMXFcntThresholdType.Relative
      thresholdLevel = -20.0 ' dB or dBm
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
      specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Fcnt, True)
      specAn.Fcnt.Configuration.ConfigureRbwFilter("", rbw, rbwFilterType, rrcAlpha)
      specAn.Fcnt.Configuration.ConfigureMeasurementInterval("", measurementInterval)
      specAn.Fcnt.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount,
                                                   averagingType)
      specAn.Fcnt.Configuration.ConfigureThreshold("", thresholdEnabled, thresholdLevel,
                                                   thresholdType)
      specAn.Initiate("", "")

   End Sub

   Private Sub RetrieveResults()
      ' Retrieve results 

      Dim frequencyTrace As AnalogWaveform(Of Single) = Nothing
      specAn.Fcnt.Results.FetchFrequencyTrace("", timeout, frequencyTrace)
      specAn.Fcnt.Results.FetchMeasurement("", timeout, averageRelativeFrequency,
                                           averageAbsoluteFrequency, meanPhase)
   End Sub

   Private Sub PrintResults()
      Console.WriteLine("Average Relative Frequency (Hz) {0}", averageRelativeFrequency)
      Console.WriteLine("Average Absolute Frequency (Hz) {0}", averageAbsoluteFrequency)
      Console.WriteLine("Mean Phase (deg)                {0}", meanPhase)
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
