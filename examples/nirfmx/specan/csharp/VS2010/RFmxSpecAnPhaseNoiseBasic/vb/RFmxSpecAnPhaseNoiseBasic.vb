'Steps:
'1. Open a new RFmx session
'2. Configure the basic instrument properties (Clock Source and Clock Frequency)
'3. Configure Selected Ports
'4. Configure instrument RF Attenuation
'5. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
'6. Configure PhaseNoise measurement and enable the traces
'7. Configure Auto Range
'8. Configure Averaging Multiplier
'9. Configure Smoothing
'10. Initiate Measurement
'11. Fetch PhaseNoise Measurements and Traces
'12. Close the RFmx Session

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.SpecAnMX

Class RFmxSpecAnPhaseNoiseBasic
   Private instrSession As RFmxInstrMX
   Private specAn As RFmxSpecAnMX
   Private selectedPorts As String
   Private centerFrequency As Double, frequencyReferenceFrequency As Double, referenceLevel As Double, externalAttenuation As Double, rbwPercentage As Double, rfAttenuation As Double
   Private startFrequency As Double, stopFrequency As Double, carrierFrequency As Double, smoothingPercentage As Double, timeout As Double, bandwidth As Double,
    measurementInterval As Double

   Private resourceName As String, frequencyReferenceSource As String
   Private enableAllTraces As Boolean
   Private rfAttenuationAuto As RFmxInstrMXRFAttenuationAuto
   Private autolevel As Boolean
   Private smoothingType As RFmxSpecAnMXPhaseNoiseSmoothingType
   Private averagingMultiplier As Integer
   Private carrierPower As Double

   Private integratedPhaseNoise As Double(), residualPMInRadian As Double(), residualPMInDegree As Double(), residualFM As Double(), jitter As Double()
   Private measuredFrequency As Single(), measuredPhaseNoise As Single(), smoothedFrequency As Single(), smoothedPhaseNoise As Single()

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

      frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
      frequencyReferenceFrequency = 10000000.0
      ' Hz 
      enableAllTraces = True

      autolevel = True
      rfAttenuationAuto = RFmxInstrMXRFAttenuationAuto.[True]
      rfAttenuation = 10.0
      ' dB 
      bandwidth = 200000.0
      ' Hz 
      measurementInterval = 0.01
      ' seconds 

      'Auto Ranges
      startFrequency = 1000.0
      ' Hz 
      stopFrequency = 1000000.0
      ' Hz 
      rbwPercentage = 10.0
      ' % 

      averagingMultiplier = 1

      ' Smoothing 

      smoothingType = RFmxSpecAnMXPhaseNoiseSmoothingType.Logarithmic
      smoothingPercentage = 2.0
      ' % 
   End Sub

   Private Sub InitializeInstr()
      ' Create a new RFmx Session 

      instrSession = New RFmxInstrMX(resourceName, "")
   End Sub

   Private Sub ConfigureSpecAn()
      ' Get SpecAn signal 

      specAn = instrSession.GetSpecAnSignalConfiguration()
      instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
      specAn.SetSelectedPorts("", selectedPorts)
      instrSession.ConfigureRFAttenuation("", rfAttenuationAuto, rfAttenuation)
      specAn.ConfigureFrequency("", centerFrequency)
      specAn.ConfigureExternalAttenuation("", externalAttenuation)
      If autolevel Then
         specAn.AutoLevel("", bandwidth, measurementInterval, referenceLevel)
         Console.WriteLine("Reference level(dBm)         : {0}", referenceLevel)
      Else
         specAn.ConfigureReferenceLevel("", referenceLevel)
      End If
      specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.PhaseNoise, enableAllTraces)

      specAn.PhaseNoise.Configuration.ConfigureAutoRange("", startFrequency, stopFrequency, rbwPercentage)

      specAn.PhaseNoise.Configuration.ConfigureAveragingMultiplier("", averagingMultiplier)
      specAn.PhaseNoise.Configuration.ConfigureSmoothing("", smoothingType, smoothingPercentage)
      specAn.Initiate("", "")
   End Sub

   Private Sub RetrieveResults()
      ' Retrieve results 

      specAn.PhaseNoise.Results.FetchCarrierMeasurement("", timeout, carrierFrequency, carrierPower)
      specAn.PhaseNoise.Results.FetchIntegratedNoise("", timeout, integratedPhaseNoise, residualPMInRadian, residualPMInDegree,
                                                     residualFM, jitter)
      specAn.PhaseNoise.Results.FetchMeasuredLogPlotTrace("", timeout, measuredFrequency, measuredPhaseNoise)
      specAn.PhaseNoise.Results.FetchSmoothedLogPlotTrace("", timeout, smoothedFrequency, smoothedPhaseNoise)
   End Sub

   Private Sub PrintResults()
      ' Display the results 

      Console.WriteLine(vbLf & "Carrier Measurement" & vbLf)
      Console.WriteLine("Carrier Frequency(Hz)        : {0}", carrierFrequency)
      Console.WriteLine("Carrier Power(dBm)           : {0}", carrierPower)

      Console.WriteLine(vbLf & "Integrated Noise" & vbLf)

      Console.WriteLine("Integrated Phase Noise(dBc)  : {0}", integratedPhaseNoise(0))
      Console.WriteLine("Residual PM(rad)             : {0}", residualPMInRadian(0))
      Console.WriteLine("Residual PM(deg)             : {0}", residualPMInDegree(0))
      Console.WriteLine("Residual FM(Hz)              : {0}", residualFM(0))
      Console.WriteLine("Jitter(s)                    : {0}", jitter(0))
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
