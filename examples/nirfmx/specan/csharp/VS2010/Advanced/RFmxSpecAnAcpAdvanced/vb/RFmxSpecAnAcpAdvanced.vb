'Steps:
'1. Open a new RFmx session
'2. Configure Selected Ports
'3. Configure the Center Frequency
'4. Configure the basic instrument properties (Clock Source, Clock Frequency)
'5. Configure the basic signal properties  (Reference Level, External Attenuation and RF Attenuation)
'6. Select ACP measurement and enable the traces
'7. Configure ACP Measurement Method, Power Units and Averaging Parameters
'8. Configure ACP FFT
'9. Configure ACP RBW Filter
'10. Configure ACP Sweep Time
'11. Configure ACP Noise Compensation
'12. Configure ACP Number of Carrier Channels
'13. Configure ACP Carrier Channel Settings (Integration BW, Carrier Mode, RRC Filter, Carrier Offset)
'14. Configure ACP Number of Offset Channels
'15. Configure ACP Offset Channel Settings (Integration BW, Offset Frequency, Offset Power Reference,
'    Relative Attenuation, RRC Filter)
'16. Initiate Measurement
'17. Fetch ACP Measurements and Traces
'18. Close the RFmx Session

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.SpecAnMX

Structure CarrierChannel
   Public mode As RFmxSpecAnMXAcpCarrierMode
   Public carrierFrequency As Double
   Public integrationBandwidth As Double
   Public rrcFilterEnabled As RFmxSpecAnMXAcpCarrierRrcFilterEnabled
   Public rrcFilterAlpha As Double
End Structure

Structure CarrierMeasurement
   Public absolutePower As Double
   Public totalRelativePower As Double
   Public resCarrierFrequency As Double
   Public resIntegrationBandwidth As Double
End Structure

Structure OffsetChannel
   Public enabled As RFmxSpecAnMXAcpOffsetEnabled()
   Public frequencySideband As RFmxSpecAnMXAcpOffsetSideband()
   Public frequencyOffset As Double()
   Public referenceCarrier As RFmxSpecAnMXAcpOffsetPowerReferenceCarrier()
   Public referenceSpecific As Integer()
   Public rrcFilterEnabled As RFmxSpecAnMXAcpOffsetRrcFilterEnabled()
   Public integrationBandwidth As Double()
   Public relativeAttenuation As Double()
   Public rrcFilterAlpha As Double()
   Public frequencyDefinition As RFmxSpecAnMXAcpOffsetFrequencyDefinition()

   Public Sub New(numOfOffsets As Int32)
      enabled = New RFmxSpecAnMXAcpOffsetEnabled(numOfOffsets - 1) {}
      frequencySideband = New RFmxSpecAnMXAcpOffsetSideband(numOfOffsets - 1) {}
      frequencyOffset = New Double(numOfOffsets - 1) {}
      referenceCarrier = New RFmxSpecAnMXAcpOffsetPowerReferenceCarrier(numOfOffsets - 1) {}
      referenceSpecific = New Integer(numOfOffsets - 1) {}
      rrcFilterEnabled = New RFmxSpecAnMXAcpOffsetRrcFilterEnabled(numOfOffsets - 1) {}
      integrationBandwidth = New Double(numOfOffsets - 1) {}
      relativeAttenuation = New Double(numOfOffsets - 1) {}
      rrcFilterAlpha = New Double(numOfOffsets - 1) {}
      frequencyDefinition = New RFmxSpecAnMXAcpOffsetFrequencyDefinition(numOfOffsets - 1) {}
   End Sub
End Structure

Structure OffsetChannelMeasurement
   Public lowerRelativePower As Double()
   Public upperRelativePower As Double()
   Public lowerAbsolutePower As Double()
   Public upperAbsolutePower As Double()
   Public Sub New(numOfOffsets As Integer)
      lowerRelativePower = New Double(numOfOffsets - 1) {}
      upperRelativePower = New Double(numOfOffsets - 1) {}
      lowerAbsolutePower = New Double(numOfOffsets - 1) {}
      upperAbsolutePower = New Double(numOfOffsets - 1) {}
   End Sub
End Structure

Class RFmxSpecAnAcpAdvanced
   Private instrSession As RFmxInstrMX
   Private specAn As RFmxSpecAnMX
   Private resourceName As String, frequencySource As String
   Private selectedPorts As String
   Private centerFrequency As Double, referenceLevel As Double, externalAttenuation As Double,
           rfAttenuation As Double, frequency As Double
   Private averagingCount As Integer
   Private noiseCompensationEnabled As RFmxSpecAnMXAcpNoiseCompensationEnabled
   Private fftWindow As RFmxSpecAnMXAcpFftWindow
   Private rbwAuto As RFmxSpecAnMXAcpRbwAutoBandwidth
   Private rbwFilterType As RFmxSpecAnMXAcpRbwFilterType
   Private sweepTimeAuto As RFmxSpecAnMXAcpSweepTimeAuto
   Private rbw As Double, sweepTimeInterval As Double, fftPadding As Double,
           totalCarrierPower As Double, timeout As Double
   Private averagingEnabled As RFmxSpecAnMXAcpAveragingEnabled
   Private averagingType As RFmxSpecAnMXAcpAveragingType
   Private measurementMethod As RFmxSpecAnMXAcpMeasurementMethod
   Private enableAllTraces As Boolean = True
   Private rfAttenuationAuto As RFmxInstrMXRFAttenuationAuto
   Const NumberOfCarriers As Integer = 1
   Const NumberOfOffsets As Integer = 2
   Private powerUnits As RFmxSpecAnMXAcpPowerUnits
   Private carrierChannelInput As CarrierChannel() = New CarrierChannel(NumberOfCarriers - 1) {}
   Private carrierChannelOutput As CarrierMeasurement() = New CarrierMeasurement(NumberOfCarriers - 1) {}
   Dim offsetChannelInput As New OffsetChannel(NumberOfOffsets)
   Dim offsetChannelOutput As New OffsetChannelMeasurement(NumberOfOffsets)
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
      centerFrequency = 1000000000.0
      ' Hz 
      referenceLevel = 0.0
      ' dBm 
      externalAttenuation = 0.0
      ' dB 
      timeout = 10.0
      ' seconds 

      rfAttenuationAuto = RFmxInstrMXRFAttenuationAuto.[True]
      rfAttenuation = 10.0
      ' dB 

      frequencySource = RFmxInstrMXConstants.OnboardClock
      frequency = 10000000.0
      ' Hz 

      powerUnits = RFmxSpecAnMXAcpPowerUnits.dBm
      measurementMethod = RFmxSpecAnMXAcpMeasurementMethod.Normal
      noiseCompensationEnabled = RFmxSpecAnMXAcpNoiseCompensationEnabled.[False]

      ' Sweep Time
      sweepTimeAuto = RFmxSpecAnMXAcpSweepTimeAuto.[True]
      sweepTimeInterval = 0.001
      ' seconds 

      ' RBW Filter
      rbwFilterType = RFmxSpecAnMXAcpRbwFilterType.Gaussian
      rbwAuto = RFmxSpecAnMXAcpRbwAutoBandwidth.[True]
      rbw = 10000.0
      ' Hz 

      ' FFT
      fftWindow = RFmxSpecAnMXAcpFftWindow.FlatTop
      fftPadding = -1.0

      'Averaging 
      averagingEnabled = RFmxSpecAnMXAcpAveragingEnabled.[False]
      averagingCount = 10
      averagingType = RFmxSpecAnMXAcpAveragingType.Rms

      ' Carrier Channels
      For i As Integer = 0 To NumberOfCarriers - 1
         carrierChannelInput(i).mode = RFmxSpecAnMXAcpCarrierMode.Active
         carrierChannelInput(i).carrierFrequency = 0.0
         carrierChannelInput(i).integrationBandwidth = 1000000.0
         ' Hz 
         carrierChannelInput(i).rrcFilterEnabled = RFmxSpecAnMXAcpCarrierRrcFilterEnabled.[False]
         carrierChannelInput(i).rrcFilterAlpha = 0.22
      Next

      ' Offset Channels

      For i As Integer = 0 To NumberOfOffsets - 1
         offsetChannelInput.enabled(i) = RFmxSpecAnMXAcpOffsetEnabled.[True]
         If i = 0 Then
            ' For offset 0, set frequency offset = 1 MHz 
            offsetChannelInput.frequencyOffset(i) = 1000000.0
         Else
            ' For offset 1, set frequency offset = 2 MHz 
            offsetChannelInput.frequencyOffset(i) = 2000000.0
         End If
         offsetChannelInput.frequencySideband(i) = RFmxSpecAnMXAcpOffsetSideband.Both
         offsetChannelInput.referenceCarrier(i) = RFmxSpecAnMXAcpOffsetPowerReferenceCarrier.Closest
         offsetChannelInput.referenceSpecific(i) = 0
         offsetChannelInput.integrationBandwidth(i) = 1000000.0
         offsetChannelInput.relativeAttenuation(i) = 0.0
         offsetChannelInput.rrcFilterEnabled(i) = RFmxSpecAnMXAcpOffsetRrcFilterEnabled.[False]
         offsetChannelInput.rrcFilterAlpha(i) = 0.22
         offsetChannelInput.frequencyDefinition(i) = RFmxSpecAnMXAcpOffsetFrequencyDefinition.CarrierCenterToOffsetCenter
      Next
   End Sub

   Private Sub InitializeInstr()
      ' Create a new RFmx Session 

      instrSession = New RFmxInstrMX(resourceName, "")
   End Sub

   Private Sub ConfigureSpecAn()
      Dim offsetString As String
      Dim carrierString As String
      ' Get SpecAn signal 

      specAn = instrSession.GetSpecAnSignalConfiguration()

      ' Configure measurement 

      instrSession.ConfigureFrequencyReference("", frequencySource, frequency)
      specAn.SetSelectedPorts("", selectedPorts)
      specAn.ConfigureFrequency("", centerFrequency)
      specAn.ConfigureReferenceLevel("", referenceLevel)
      specAn.ConfigureExternalAttenuation("", externalAttenuation)
      instrSession.ConfigureRFAttenuation("", rfAttenuationAuto, rfAttenuation)
      specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Acp, enableAllTraces)
      specAn.Acp.Configuration.ConfigureMeasurementMethod("", measurementMethod)
      specAn.Acp.Configuration.ConfigurePowerUnits("", powerUnits)
      specAn.Acp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)
      specAn.Acp.Configuration.ConfigureFft("", fftWindow, fftPadding)
      specAn.Acp.Configuration.ConfigureRbwFilter("", rbwAuto, rbw, rbwFilterType)
      specAn.Acp.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval)
      specAn.Acp.Configuration.ConfigureNoiseCompensationEnabled("", noiseCompensationEnabled)

      specAn.Acp.Configuration.ConfigureNumberOfCarriers("", NumberOfCarriers)
      For i As Integer = 0 To NumberOfCarriers - 1
         carrierString = RFmxSpecAnMX.BuildCarrierString2("", i)
         specAn.Acp.Configuration.ConfigureCarrierIntegrationBandwidth(carrierString,
                                                                       carrierChannelInput(i).integrationBandwidth)
         specAn.Acp.Configuration.ConfigureCarrierMode(carrierString,
                                                       carrierChannelInput(i).mode)
         specAn.Acp.Configuration.ConfigureCarrierRrcFilter(carrierString,
                                                            carrierChannelInput(i).rrcFilterEnabled,
                                                            carrierChannelInput(i).rrcFilterAlpha)
         specAn.Acp.Configuration.ConfigureCarrierFrequency(carrierString,
                                                         carrierChannelInput(i).carrierFrequency)
      Next
      specAn.Acp.Configuration.ConfigureNumberOfOffsets("", NumberOfOffsets)

      For i As Integer = 0 To NumberOfOffsets - 1
         offsetString = RFmxSpecAnMX.BuildOffsetString2("", i)
         specAn.Acp.Configuration.ConfigureOffsetFrequencyDefinition(offsetString, offsetChannelInput.frequencyDefinition(i))
      Next

      specAn.Acp.Configuration.ConfigureOffsetArray("", offsetChannelInput.frequencyOffset, offsetChannelInput.frequencySideband,
                                                    offsetChannelInput.enabled)
      specAn.Acp.Configuration.ConfigureOffsetIntegrationBandwidthArray("", offsetChannelInput.integrationBandwidth)
      specAn.Acp.Configuration.ConfigureOffsetPowerReferenceArray("", offsetChannelInput.referenceCarrier,
                                                                  offsetChannelInput.referenceSpecific)
      specAn.Acp.Configuration.ConfigureOffsetRelativeAttenuationArray("", offsetChannelInput.relativeAttenuation)
      specAn.Acp.Configuration.ConfigureOffsetRrcFilterArray("", offsetChannelInput.rrcFilterEnabled,
                                                             offsetChannelInput.rrcFilterAlpha)

      specAn.Initiate("", "")
   End Sub

   Private Sub RetrieveResults()
      ' Retrieve results 

      Dim carrierString As String
      Dim spectrum As Spectrum(Of Single) = Nothing

      specAn.Acp.Results.FetchOffsetMeasurementArray("", timeout, offsetChannelOutput.lowerRelativePower,
         offsetChannelOutput.upperRelativePower, offsetChannelOutput.lowerAbsolutePower, offsetChannelOutput.upperAbsolutePower)

      For i As Integer = 0 To NumberOfCarriers - 1
         carrierString = RFmxSpecAnMX.BuildCarrierString2("", i)
         specAn.Acp.Results.FetchCarrierMeasurement(carrierString, timeout,
                                                    carrierChannelOutput(i).absolutePower,
                                                    carrierChannelOutput(i).totalRelativePower,
                                                    carrierChannelOutput(i).resCarrierFrequency,
                                                    carrierChannelOutput(i).resIntegrationBandwidth)
      Next
      specAn.Acp.Results.FetchTotalCarrierPower("", timeout, totalCarrierPower)

      specAn.Acp.Results.FetchSpectrum("", timeout, spectrum)
   End Sub

   Private Sub PrintResults()
      ' Display the results 


      Console.WriteLine("Total Carrier Power (dBm or dBm/Hz)  {0}" & vbLf, totalCarrierPower)
      Console.WriteLine("Carrier Measurements: " & vbLf)
      For i As Integer = 0 To NumberOfCarriers - 1
         Console.WriteLine("Carrier {0}:", i)
         Console.WriteLine("Abosulte Power (dBm or dBm/Hz)       {0}", carrierChannelOutput(i).absolutePower)
         Console.WriteLine("Total Relative Power (dB)            {0}", carrierChannelOutput(i).totalRelativePower)
         Console.WriteLine("Carrier Offset (Hz)                  {0}", carrierChannelOutput(i).resCarrierFrequency)
         Console.WriteLine("Integration Bandwidth (Hz)           {0}", carrierChannelOutput(i).resIntegrationBandwidth)
         Console.WriteLine("---------------------------------------------------" & vbLf)
      Next

      Console.WriteLine("Offset Channel Measurements: " & vbLf)
      For i As Integer = 0 To NumberOfOffsets - 1
         Console.WriteLine("Offset {0}:", i)
         Console.WriteLine("Lower Relative Power (dB)            {0}", offsetChannelOutput.lowerRelativePower(i))
         Console.WriteLine("Upper Relative Power (dB)            {0}", offsetChannelOutput.upperRelativePower(i))
         Console.WriteLine("Lower Absolute Power (dBm or dBm/Hz) {0}", offsetChannelOutput.lowerAbsolutePower(i))
         Console.WriteLine("Upper Absolute Power (dBm or dBm/Hz) {0}", offsetChannelOutput.upperAbsolutePower(i))
         Console.WriteLine("-------------------------------------------------" & vbLf)
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

   Private Shared Sub DisplayError(message As String)
      Console.WriteLine("ERROR:" & vbLf & message)
   End Sub
End Class
