'Steps:
'1. Open a new RFmx session
'2. Configure the instrument properties Clock Source and Clock Frequency
'3. Configure Selected Ports
'4. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
'5. Select CCDF measurement and enable the traces
'6. Configure CCDF Number of Records and Measurement Interval
'7. Configure CCDF RBW filter
'8. Configure CCDF Threshold
'9. Initiate Measurement
'10. Fetch CCDF Measurements and Traces
'11. Close the RFmx Session

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.SpecAnMX

Class RFmxSpecAnCcdf
   Private instrSession As RFmxInstrMX
   Private specAn As RFmxSpecAnMX
   Private resourceName As String, frequencySource As String
   Private selectedPorts As String
   Private centerFrequency As Double, referenceLevel As Double, externalAttenuation As Double,
           measurementInterval As Double, rbw As Double, rrcAlpha As Double,
           thresholdLevel As Double, frequency As Double
   Private measuredSamplesCount As Integer, numOfRecords As Integer
   Private enableAllTraces As Boolean
   Private rbwFilterType As RFmxSpecAnMXCcdfRbwFilterType
   Private thresholdEnabled As RFmxSpecAnMXCcdfThresholdEnabled
   Private thresholdType As RFmxSpecAnMXCcdfThresholdType

   Private tenPercentPower As Double, onePercentPower As Double, oneTenthPercentPower As Double,
           oneHundredthPercentPower As Double, oneThousandthPercentPower As Double, oneTenThousandthPercentPower As Double,
           meanPower As Double, meanPowerPercentile As Double, peakPower As Double, timeout As Double
   Private gaussianProbabilitiesWaveform As AnalogWaveform(Of Single), probabilitiesWaveform As AnalogWaveform(Of Single)

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

      frequencySource = RFmxInstrMXConstants.OnboardClock
      frequency = 10000000.0 ' Hz 

      numOfRecords = 1
      measurementInterval = 0.001
      ' seconds 

      rbwFilterType = RFmxSpecAnMXCcdfRbwFilterType.None
      rbw = 100000.0 ' Hz 
      rrcAlpha = 0.01

      thresholdEnabled = RFmxSpecAnMXCcdfThresholdEnabled.[False]
      thresholdType = RFmxSpecAnMXCcdfThresholdType.Relative
      thresholdLevel = -20.0 ' dB or dBm

      enableAllTraces = True
      timeout = 10.0 ' seconds 
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
      specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Ccdf, enableAllTraces)
      specAn.Ccdf.Configuration.ConfigureNumberOfRecords("", numOfRecords)
      specAn.Ccdf.Configuration.ConfigureMeasurementInterval("", measurementInterval)
      specAn.Ccdf.Configuration.ConfigureRbwFilter("", rbw, rbwFilterType, rrcAlpha)
      specAn.Ccdf.Configuration.ConfigureThreshold("", thresholdEnabled, thresholdLevel,
                                                   thresholdType)
      specAn.Initiate("", "")
   End Sub

   Private Sub RetrieveResults()
      ' Retrieve results 


      specAn.Ccdf.Results.FetchPower("", timeout, meanPower, meanPowerPercentile,
                                     peakPower, measuredSamplesCount)
      specAn.Ccdf.Results.FetchBasicPowerProbabilities("", timeout, tenPercentPower,
                                                       onePercentPower,
                                                       oneTenthPercentPower,
                                                       oneHundredthPercentPower,
                                                       oneThousandthPercentPower,
                                                       oneTenThousandthPercentPower)
      specAn.Ccdf.Results.FetchGaussianProbabilitiesTrace("", timeout,
                                                          gaussianProbabilitiesWaveform)
      specAn.Ccdf.Results.FetchProbabilitiesTrace("", timeout, probabilitiesWaveform)
   End Sub

   Private Sub PrintResults()
      Console.WriteLine("--------------Power----------------------" & vbLf)
      Console.WriteLine(" Mean Power (dBm)           {0}", meanPower)
      Console.WriteLine(" Mean Power Percentile (%)  {0}", meanPowerPercentile)
      Console.WriteLine(" Peak Power (dB)            {0}", peakPower)
      Console.WriteLine(" Measured Samples Count     {0}", measuredSamplesCount)

      Console.WriteLine("--------------Power Probabilities-------------" & vbLf)
      Console.WriteLine(" 10 % Power (dB)            {0}", tenPercentPower)
      Console.WriteLine(" 1 % Power (dB)             {0}", onePercentPower)
      Console.WriteLine(" 0.1 % Power (dB)           {0}", oneTenthPercentPower)
      Console.WriteLine(" 0.01 % Power (dB)          {0}", oneHundredthPercentPower)
      Console.WriteLine(" 0.001 % Power (dB)         {0}", oneThousandthPercentPower)
      Console.WriteLine(" 0.0001 % Power (dB)        {0}", oneTenThousandthPercentPower)
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
