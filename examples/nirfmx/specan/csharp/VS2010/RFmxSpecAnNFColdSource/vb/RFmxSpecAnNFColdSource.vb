'Steps:
'1. Open a new RFmx session.
'2. Configure Selected Ports.
'3. Select NF measurement.
'4. Configure Measurement Method.
'5. Configure measurement frequencies
'       5.1. Specify Start Frequency, Stop Frequency and  Frequency Step Size.
'       5.2. Specify Start Frequency, Stop Frequency and Frequency Points.
'       5.3. Specify Frequency List.
'6. Configure Measurement Bandwidth.
'7. Configure Measurement Interval.
'8. Configure  Averaging.
'9. Configure Calibration Loss.
'10. Configure DUT Input Loss.
'11. Configure DUT Output Loss.
'12. Configure Cold Source Mode.
'13. Configure Cold Source DUT S-Parameters.
'14. Configure Reference Level
'         14.1. Let measurement recommend a Reference Level.
'         14.2. Manually configure Reference Level.
'15. Initiate the measurement.
'16. Fetch NF Measurements and Create Graphs.
'17. Close RFmx Session.

Imports NationalInstruments
Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.SpecAnMX

Public Enum FrequencyListConfigurationType
   [Step] = CInt(0)
   Points = CInt(1)
   Frequency = CInt(2)
End Enum

Class RFmxSpecAnNFColdSource
   Private instrSession As RFmxInstrMX
   Private specAn As RFmxSpecAnMX
   Private resourceName As [String]
   Private selectedPorts As String
   Private referenceLevel As Double, dutMaxGain As Double, dutMaxNoiseFigure As Double
   Private timeout As Double
   Private averagingCount As Integer
   Private averagingEnabled As RFmxSpecAnMXNFAveragingEnabled

   Private startFrequency As Double
   Private stopFrequency As Double

   Private stepSize As Double
   Private numberOfPoints As Integer
   Private frequencyList As Double() = Nothing

   Private measurementMethod As RFmxSpecAnMXNFMeasurementMethod
   Private coldSourceMode As RFmxSpecAnMXNFColdSourceMode
   Private measurementBandwidth As Double, measurementInterval As Double

   Private DutInputLossCompEnabled As RFmxSpecAnMXNFDutInputLossCompensationEnabled
   Private DutInputLossTemperature As Double
   Private DutInputLoss As Double(), DutInputLossFrequency As Double()

   Private DutOutputLossCompEnabled As RFmxSpecAnMXNFDutOutputLossCompensationEnabled
   Private DutOutputLossTemperature As Double
   Private DutOutputLoss As Double(), DutOutputLossFrequency As Double()

   Private calibrationLossCompensationEnabled As RFmxSpecAnMXNFCalibrationLossCompensationEnabled
   Private calibrationLossTemperature As Double
   Private calibrationLoss As Double(), calibrationLossFrequency As Double()

   Private sParamFrequency As Double()
   Private s11 As Double(), s12 As Double(), s21 As Double(), s22 As Double()  'dB

   Private frequencyListConfiguration As FrequencyListConfigurationType
   Private manualReferenceLevel As Boolean

   'Result variables

   Private coldSourcePower As Double()       'dBm
   Private dutGain As Double()             'dB
   Private dutNoiseFigure As Double()      'dB
   Private dutNoiseTemperature As Double() 'K
   Private frequencyListOut As Double()

   Private analyserNoiseFigure As Double() 'dB
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
      selectedPorts = ""

      startFrequency = 1000000000.0       ' Hz 
      stopFrequency = 2000000000.0        ' Hz 

      stepSize = 100000000.0              ' Hz 
      numberOfPoints = 10

      dutMaxGain = 0.0                    'dB
      dutMaxNoiseFigure = 0.0             'dB
      referenceLevel = -55.0              'dBm 

      frequencyListConfiguration = FrequencyListConfigurationType.[Step]
      manualReferenceLevel = True

      measurementMethod = RFmxSpecAnMXNFMeasurementMethod.ColdSource
      coldSourceMode = RFmxSpecAnMXNFColdSourceMode.Measure
      measurementBandwidth = 100000.0      'Hz
      measurementInterval = 0.001          'seconds

      DutInputLossCompEnabled = RFmxSpecAnMXNFDutInputLossCompensationEnabled.[False]
      DutInputLossTemperature = 297         'K
      DutInputLoss = InlineAssignHelper(DutInputLossFrequency, Nothing)

      DutOutputLossCompEnabled = RFmxSpecAnMXNFDutOutputLossCompensationEnabled.[False]
      DutOutputLossTemperature = 297       'K
      DutOutputLoss = InlineAssignHelper(DutOutputLossFrequency, Nothing)

      calibrationLossCompensationEnabled = RFmxSpecAnMXNFCalibrationLossCompensationEnabled.[False]
      calibrationLossTemperature = 297  'K
      calibrationLoss = InlineAssignHelper(calibrationLossFrequency, Nothing)

      averagingCount = 10
      averagingEnabled = RFmxSpecAnMXNFAveragingEnabled.[False]

      sParamFrequency = InlineAssignHelper(s11, InlineAssignHelper(s12, InlineAssignHelper(s21, InlineAssignHelper(s22, Nothing))))

      timeout = 10.0                   'seconds 
   End Sub

   Private Sub InitializeInstr()
      ' Create a new RFmx Session 

      instrSession = New RFmxInstrMX(resourceName, "")
   End Sub

   Private Sub ConfigureSpecAn()
      ' Get SpecAn signal 

      specAn = instrSession.GetSpecAnSignalConfiguration()

      ' Configure measurement 
      specAn.SetSelectedPorts("", selectedPorts)

      specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.NF, False)

      specAn.NF.Configuration.ConfigureMeasurementMethod("", measurementMethod)

      If frequencyListConfiguration = FrequencyListConfigurationType.[Step] Then
         specAn.NF.Configuration.ConfigureFrequencyListStartStopStep("", startFrequency, stopFrequency, stepSize)
      ElseIf frequencyListConfiguration = FrequencyListConfigurationType.Points Then
         specAn.NF.Configuration.ConfigureFrequencyListStartStopPoints("", startFrequency, stopFrequency, numberOfPoints)
      ElseIf frequencyListConfiguration = FrequencyListConfigurationType.Frequency Then
         specAn.NF.Configuration.ConfigureFrequencyList("", frequencyList)
      End If

      specAn.NF.Configuration.ConfigureMeasurementBandwidth("", measurementBandwidth)
      specAn.NF.Configuration.ConfigureMeasurementInterval("", measurementInterval)
      specAn.NF.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount)
      specAn.NF.Configuration.ConfigureCalibrationLoss("", calibrationLossCompensationEnabled, calibrationLossFrequency, calibrationLoss, calibrationLossTemperature)
      specAn.NF.Configuration.ConfigureDutInputLoss("", DutInputLossCompEnabled, DutInputLossFrequency, DutInputLoss, DutInputLossTemperature)
      specAn.NF.Configuration.ConfigureDutOutputLoss("", DutOutputLossCompEnabled, DutOutputLossFrequency, DutOutputLoss, DutOutputLossTemperature)
      specAn.NF.Configuration.ConfigureColdSourceMode("", coldSourceMode)
      specAn.NF.Configuration.ConfigureColdSourceDutSParameters("", sParamFrequency, s21, s12, s11, s22)

      If Not manualReferenceLevel Then
         specAn.NF.Configuration.RecommendReferenceLevel("", dutMaxGain, dutMaxNoiseFigure, referenceLevel)
         Console.WriteLine("Reference Level :        {0}", referenceLevel)
         Console.WriteLine()
      Else
         specAn.ConfigureReferenceLevel("", referenceLevel)
      End If
      specAn.Initiate("", "")
   End Sub

   Private Sub RetrieveResults()
      ' Retrieve results 

      specAn.NF.Results.FetchColdSourcePower("", timeout, coldSourcePower)
      specAn.NF.Results.FetchAnalyzerNoiseFigure("", timeout, analyserNoiseFigure)
      specAn.NF.Results.FetchDutNoiseFigureAndGain("", timeout, dutNoiseFigure, dutNoiseTemperature, dutGain)
      specAn.NF.Configuration.GetFrequencyList("", frequencyListOut)
   End Sub

   Private Sub PrintResults()
      Dim resultSize As Integer = 0
      resultSize = If((coldSourcePower Is Nothing), 0, coldSourcePower.Length)
      Console.WriteLine(vbLf & "Results" & vbLf)
      For i As Integer = 0 To resultSize - 1
         Console.WriteLine(vbLf & "Result {0}:" & vbLf, i)
         Console.WriteLine("Frequency (Hz)             :      {0}", frequencyListOut(i))
         Console.WriteLine("DUT Noise Figure (dB)      :      {0}", dutNoiseFigure(i))
         Console.WriteLine("DUT Noise Temperature (K)  :      {0}", dutNoiseTemperature(i))
         Console.WriteLine("DUT Gain (dB)              :      {0}", dutGain(i))
         Console.WriteLine("Analyser Noise Figure(dB)  :      {0}", analyserNoiseFigure(i))
         Console.WriteLine("Measured Power (dBm)       :      {0}", coldSourcePower(i))
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

   Private Sub DisplayError(ex As Exception)
      Console.WriteLine("ERROR:" & vbLf & Convert.ToString(ex.[GetType]()) & ": " & ex.Message)
   End Sub
   Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
      target = value
      Return value
   End Function

End Class
