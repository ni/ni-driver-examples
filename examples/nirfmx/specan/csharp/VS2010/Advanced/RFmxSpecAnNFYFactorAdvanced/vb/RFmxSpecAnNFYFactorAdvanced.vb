'Steps:
'1. Open a new RFmx session.
'2. Configure RF Attenuation.
'3. Configure Selected Ports.
'4. Select NF measurement.
'5. Configure Measurement Method.
'6. Configure measurement frequencies
'       6.1. Specify Start Frequency, Stop Frequency and  Frequency Step Size.
'       6.2. Specify Start Frequency, Stop Frequency and Frequency Points.
'       6.3. Specify Frequency List.
'7. Configure DUT Type.
'8. Configure Frequency Converter DUT specific properties.
'9. Configure Measurement Bandwidth.
'10. Configure Measurement Interval.
'11. Configure Averaging.
'12. Configure Calibration loss.
'13. Configure DUT Input Loss.
'14. Configure DUT Output Loss.
'15. Configure RF Preamplifier and Preselector.
'16. Configure Y-Factor Mode.
'17. Configure Y-Factor Noise Source ENR.
'18. Configure Y-Factor Noise Source Settling Time.
'19. Configure Y-Factor Noise Source Loss.
'20. Configure Reference Level
'          20.1. Let measurement recommend a Reference Level.
'          20.2. Manually Configure Reference Level.
'21. Intiate the measurement.
'22. Fetch NF Measurements and Create Graphs.
'23. Close RFmx Session.

Imports NationalInstruments
Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.SpecAnMX

Public Enum FrequencyListConfigurationType
   [Step] = CInt(0)
   Points = CInt(1)
   Frequency = CInt(2)
End Enum

Class RFmxSpecAnNFYFactorAdvanced
   Private instrSession As RFmxInstrMX
   Private specAn As RFmxSpecAnMX
   Private resourceName As [String]
   Private selectedPorts As String
   Private referenceLevel As Double, dutMaxGain As Double, dutMaxNoiseFigure As Double
   Private timeout As Double
   Private averagingCount As Integer
   Private averagingEnabled As RFmxSpecAnMXNFAveragingEnabled

   Private rfAttenuationAuto As RFmxInstrMXRFAttenuationAuto
   Private rfAttenuation As Double

   Private startFrequency As Double
   Private stopFrequency As Double

   Private stepSize As Double
   Private numberOfPoints As Integer
   Private frequencyList As Double() = Nothing
   Private preselectorEnabled As RFmxInstrMXDownconverterPreselectorEnabled

   Private dutType As RFmxSpecAnMXNFDutType

   Private LOFrequency As Double
   Private frequencyContext As RFmxSpecAnMXNFFrequencyConverterFrequencyContext
   Private sideband As RFmxSpecAnMXNFFrequencyConverterSideband
   Private imageRejection As Double

   Private measurementMethod As RFmxSpecAnMXNFMeasurementMethod
   Private yFactorMode As RFmxSpecAnMXNFYFactorMode
   Private measurementBandwidth As Double, measurementInterval As Double

   Private dutInputLossCompEnabled As RFmxSpecAnMXNFDutInputLossCompensationEnabled
   Private dutInputLossTemperature As Double
   Private dutInputLoss As Double(), dutInputLossFrequency As Double()

   Private dutOutputLossCompEnabled As RFmxSpecAnMXNFDutOutputLossCompensationEnabled
   Private dutOutputLossTemperature As Double
   Private dutOutputLoss As Double(), dutOutputLossFrequency As Double()

   Private calibrationLossCompensationEnabled As RFmxSpecAnMXNFCalibrationLossCompensationEnabled
   Private calibrationLossTemperature As Double
   Private calibrationLoss As Double(), calibrationLossFrequency As Double()

   Private noiseSourceLossCompensationEnabled As RFmxSpecAnMXNFYFactorNoiseSourceLossCompensationEnabled
   Private noiseSourceLossTemperature As Double
   Private noiseSourceLoss As Double(), noiseSourceLossFrequency As Double()

   Private frequencyListConfiguration As FrequencyListConfigurationType
   Private recommend As Boolean
   Private preamp As RFmxInstrMXPreampEnabled

   Private settlingTime As Double, coldTemperature As Double, offTemperature As Double
   Private enr As Double(), enrFrequency As Double()

   'Result variables

   Private hotPower As Double(), coldPower As Double()   ' dBm
   Private dutGain As Double()                           ' dB
   Private dutNoiseFigure As Double()                    ' dB
   Private dutNoiseTemperature As Double()               ' K
   Private measurementYFactor As Double(), calibrationYFactor As Double()
   Private analyserNoiseFigure As Double()               ' dB

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

      rfAttenuationAuto = RFmxInstrMXRFAttenuationAuto.True
      rfAttenuation = 10                     ' dB

      averagingCount = 10
      averagingEnabled = RFmxSpecAnMXNFAveragingEnabled.False

      startFrequency = 1000000000.0          ' Hz 
      stopFrequency = 2000000000.0           ' Hz 

      stepSize = 100000000.0                 ' Hz 
      numberOfPoints = 10

      dutMaxGain = 0.0                       ' dB
      dutMaxNoiseFigure = 0.0                ' dB
      referenceLevel = -55.0                 ' dBm 

      frequencyListConfiguration = FrequencyListConfigurationType.Step
      recommend = False
      preamp = RFmxInstrMXPreampEnabled.Enabled
      preselectorEnabled = RFmxInstrMXDownconverterPreselectorEnabled.Enabled

      dutType = RFmxSpecAnMXNFDutType.Amplifier

      LOFrequency = 10000000.0               ' Hz 
      frequencyContext = RFmxSpecAnMXNFFrequencyConverterFrequencyContext.RF
      sideband = RFmxSpecAnMXNFFrequencyConverterSideband.Lsb
      imageRejection = 999.99                ' dB

      measurementMethod = RFmxSpecAnMXNFMeasurementMethod.YFactor
      yFactorMode = RFmxSpecAnMXNFYFactorMode.Measure
      measurementBandwidth = 100000.0        ' Hz
      measurementInterval = 0.001            ' seconds

      dutInputLossCompEnabled = RFmxSpecAnMXNFDutInputLossCompensationEnabled.False
      dutInputLossTemperature = 297          ' K
      dutInputLoss = InlineAssignHelper(dutOutputLoss, Nothing)                           ' dB

      dutOutputLossCompEnabled = RFmxSpecAnMXNFDutOutputLossCompensationEnabled.False
      dutOutputLossTemperature = 297         ' K
      dutInputLossFrequency = InlineAssignHelper(dutOutputLossFrequency, Nothing)         ' Hz

      settlingTime = 0.0                     ' seconds
      coldTemperature = 302.8                ' K
      offTemperature = 297.0                 ' K
      enr = Nothing                          ' dB
      enrFrequency = Nothing                 ' Hz

      calibrationLossCompensationEnabled = RFmxSpecAnMXNFCalibrationLossCompensationEnabled.False
      calibrationLossTemperature = 297       ' K
      calibrationLoss = InlineAssignHelper(noiseSourceLoss, Nothing)                      ' dB

      noiseSourceLossCompensationEnabled = RFmxSpecAnMXNFYFactorNoiseSourceLossCompensationEnabled.False
      noiseSourceLossTemperature = 297       ' K
      calibrationLossFrequency = InlineAssignHelper(noiseSourceLossFrequency, Nothing)    ' Hz

      timeout = 10.0                         ' seconds 
   End Sub

   Private Sub InitializeInstr()
      ' Create a new RFmx Session 

      instrSession = New RFmxInstrMX(resourceName, "")
   End Sub

   Private Sub ConfigureSpecAn()
      ' Get SpecAn signal 

      specAn = instrSession.GetSpecAnSignalConfiguration()

      ' Configure measurement 

      instrSession.ConfigureRFAttenuation("", rfAttenuationAuto, rfAttenuation)

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

      specAn.NF.Configuration.SetDutType("", dutType)

      specAn.NF.Configuration.SetFrequencyConverterLOFrequency("", LOFrequency)
      specAn.NF.Configuration.SetFrequencyConverterFrequencyContext("", frequencyContext)
      specAn.NF.Configuration.SetFrequencyConverterSideband("", sideband)
      specAn.NF.Configuration.SetFrequencyConverterImageRejection("", imageRejection)

      specAn.NF.Configuration.ConfigureMeasurementBandwidth("", measurementBandwidth)
      specAn.NF.Configuration.ConfigureMeasurementInterval("", measurementInterval)
      specAn.NF.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount)
      specAn.NF.Configuration.ConfigureCalibrationLoss("", calibrationLossCompensationEnabled, calibrationLossFrequency, calibrationLoss, calibrationLossTemperature)
      specAn.NF.Configuration.ConfigureDutInputLoss("", dutInputLossCompEnabled, dutInputLossFrequency, dutInputLoss, dutInputLossTemperature)
      specAn.NF.Configuration.ConfigureDutOutputLoss("", dutOutputLossCompEnabled, dutOutputLossFrequency, dutOutputLoss, dutOutputLossTemperature)
      instrSession.SetPreampEnabled("", preamp)
      instrSession.SetDownconverterPreselectorEnabled("", preselectorEnabled)
      specAn.NF.Configuration.ConfigureYFactorMode("", yFactorMode)
      specAn.NF.Configuration.ConfigureYFactorNoiseSourceEnr("", enrFrequency, enr, coldTemperature, offTemperature)
      specAn.NF.Configuration.ConfigureYFactorNoiseSourceSettlingTime("", settlingTime)
      specAn.NF.Configuration.ConfigureYFactorNoiseSourceLoss("", noiseSourceLossCompensationEnabled, noiseSourceLossFrequency, noiseSourceLoss, noiseSourceLossTemperature)
      If recommend Then
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

      specAn.NF.Results.FetchYFactors("", timeout, measurementYFactor, calibrationYFactor)
      specAn.NF.Results.FetchYFactorPowers("", timeout, hotPower, coldPower)
      specAn.NF.Results.FetchAnalyzerNoiseFigure("", timeout, analyserNoiseFigure)
      specAn.NF.Results.FetchDutNoiseFigureAndGain("", timeout, dutNoiseFigure, dutNoiseTemperature, dutGain)
      specAn.NF.Configuration.GetFrequencyList("", frequencyList)
   End Sub

   Private Sub PrintResults()
      Dim resultSize As Integer = 0
      resultSize = If((hotPower Is Nothing), 0, hotPower.Length)
      Console.WriteLine(vbLf & "Results" & vbLf)
      For i As Integer = 0 To resultSize - 1
         Console.WriteLine(vbLf & "Result {0}:" & vbLf, i)
         Console.WriteLine("Frequency (Hz)             :      {0}", frequencyList(i))
         Console.WriteLine("DUT Noise Figure (dB)      :      {0}", dutNoiseFigure(i))
         Console.WriteLine("DUT Noise Temperature(k)   :      {0}", dutNoiseTemperature(i))
         Console.WriteLine("DUT Gain (dB)              :      {0}", dutGain(i))
         Console.WriteLine("Analyser Noise Figure(dB)  :      {0}", analyserNoiseFigure(i))
         Console.WriteLine("Hot Power (dBm)            :      {0}", hotPower(i))
         Console.WriteLine("Cold Power (dBm)           :      {0}", coldPower(i))
         Console.WriteLine("Measurement Y-Factor (dB)  :      {0}", measurementYFactor(i))
         Console.WriteLine("Calibration Y-Factor (dB)  :      {0}", calibrationYFactor(i))
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
