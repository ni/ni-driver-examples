'Steps:
'1. Open a new RFmx session
'2. Configure Selected Ports
'3. Configure Frequency Reference
'4. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
'5. Configure RF Attenuation
'6. Select ACP measurement and enable the traces
'7. Configure ACP Measurement Method, Power Units and Averaging Parameters
'8. Configure ACP RBW filter
'9. Configure ACP Sweep Time
'10. Configure ACP Noise Compensation
'11. Configure ACP Carrier Channel Settings (Integration BW, RRC Filter)
'12. Configure ACP Number of Offset Channels
'13. Configure ACP Offset Channel Settings (Offset Frequencies, Integration BW, RRC Filter)
'Use "offset:all" selector string to set a parameter for all the Offset Channels
'14. Initiate Measurement
'15. Fetch ACP Measurements and Traces
'16. Close the RFmx Session

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.SpecAnMX

Public Class RFmxSpecAnAcpDynamicRangeExample
   Private instrSession As RFmxInstrMX
   Private specAn As RFmxSpecAnMX
   Private resourceName As String

   Const NumberOfOffsets As Integer = 2

   Private selectedPorts As String
   Private centerFrequency As Double, referenceLevel As Double, autoSetReferenceLevel As Double, externalAttenuation As Double, rfAttenuation As Double, rbw As Double,
      frequency As Double, sweepTimeInterval As Double, measurementInterval As Double, timeout As Double
   Private frequencySource As String
   Private autoLevel As Boolean
   Private rfAttenuationAuto As RFmxInstrMXRFAttenuationAuto
   Private powerUnits As RFmxSpecAnMXAcpPowerUnits
   Private measurementMethod As RFmxSpecAnMXAcpMeasurementMethod
   Private noiseCompensationEnabled As RFmxSpecAnMXAcpNoiseCompensationEnabled
   Private rbwFilterType As RFmxSpecAnMXAcpRbwFilterType
   Private rbwAuto As RFmxSpecAnMXAcpRbwAutoBandwidth
   Private sweepTimeAuto As RFmxSpecAnMXAcpSweepTimeAuto
   Private averagingCount As Integer
   Private averagingEnabled As RFmxSpecAnMXAcpAveragingEnabled
   Private averagingType As RFmxSpecAnMXAcpAveragingType

   'Input values
   Private Structure CarrierChannel
      Public integrationBandwidth As Double
      Public rrcFilterEnabled As RFmxSpecAnMXAcpCarrierRrcFilterEnabled
      Public rrcFilterAlpha As Double
   End Structure
   Private carrierChannelInput As CarrierChannel

   Private Structure OffsetChannel
      Public integrationBandwidth As Double
      Public frequencyOffset As Double()
      Public rrcFilterEnabled As RFmxSpecAnMXAcpOffsetRrcFilterEnabled
      Public rrcFilterAlpha As Double
   End Structure
   Private offsetChannelInput As OffsetChannel

   'Output values
   Private absolutePower As Double
   Private lowerRelativePower As Double()
   Private upperRelativePower As Double()
   Private lowerAbsolutePower As Double()
   Private upperAbsolutePower As Double()

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

      rfAttenuationAuto = RFmxInstrMXRFAttenuationAuto.[True]
      rfAttenuation = 10.0
      ' dB 

      frequencySource = RFmxInstrMXConstants.OnboardClock
      frequency = 10000000.0
      ' Hz 

      measurementInterval = 0.01
      ' seconds 
      autoLevel = True

      ' Carrier Channels
      carrierChannelInput.integrationBandwidth = 1000000.0
      carrierChannelInput.rrcFilterEnabled = RFmxSpecAnMXAcpCarrierRrcFilterEnabled.[False]
      carrierChannelInput.rrcFilterAlpha = 0.22

      powerUnits = RFmxSpecAnMXAcpPowerUnits.dBm
      measurementMethod = RFmxSpecAnMXAcpMeasurementMethod.DynamicRange
      noiseCompensationEnabled = RFmxSpecAnMXAcpNoiseCompensationEnabled.[True]

      ' Sweep Time
      sweepTimeAuto = RFmxSpecAnMXAcpSweepTimeAuto.[True]
      sweepTimeInterval = 0.001
      ' seconds 

      ' RBW Filter
      rbwFilterType = RFmxSpecAnMXAcpRbwFilterType.Gaussian
      rbwAuto = RFmxSpecAnMXAcpRbwAutoBandwidth.[True]
      rbw = 10000.0
      ' Hz 

      ' Offset Channels	
      offsetChannelInput.integrationBandwidth = 1000000.0
      ' Hz 
      offsetChannelInput.rrcFilterEnabled = RFmxSpecAnMXAcpOffsetRrcFilterEnabled.[False]
      offsetChannelInput.rrcFilterAlpha = 0.22

      offsetChannelInput.frequencyOffset = New Double(NumberOfOffsets - 1) {}
      offsetChannelInput.frequencyOffset(0) = 1000000.0
      ' Hz 
      offsetChannelInput.frequencyOffset(1) = 2000000.0
      ' Hz 

      'Averaging 
      averagingEnabled = RFmxSpecAnMXAcpAveragingEnabled.[False]
      averagingCount = 10
      averagingType = RFmxSpecAnMXAcpAveragingType.Rms

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
      specAn.ConfigureExternalAttenuation("", externalAttenuation)
      instrSession.ConfigureRFAttenuation("", rfAttenuationAuto, rfAttenuation)

      If autoLevel Then
         specAn.AutoLevel("", carrierChannelInput.integrationBandwidth, measurementInterval, autoSetReferenceLevel)
         Console.WriteLine("Reference Level(dBm): {0}", autoSetReferenceLevel)
      Else
         specAn.ConfigureReferenceLevel("", referenceLevel)
      End If

      specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Acp, True)

      specAn.Acp.Configuration.ConfigureMeasurementMethod("", measurementMethod)
      specAn.Acp.Configuration.ConfigurePowerUnits("", powerUnits)
      specAn.Acp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)
      specAn.Acp.Configuration.ConfigureRbwFilter("", rbwAuto, rbw, rbwFilterType)
      specAn.Acp.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval)
      specAn.Acp.Configuration.ConfigureNoiseCompensationEnabled("", noiseCompensationEnabled)
      specAn.Acp.Configuration.ConfigureCarrierIntegrationBandwidth("", carrierChannelInput.integrationBandwidth)
      specAn.Acp.Configuration.ConfigureCarrierRrcFilter("", carrierChannelInput.rrcFilterEnabled, carrierChannelInput.rrcFilterAlpha)

      specAn.Acp.Configuration.ConfigureNumberOfOffsets("", NumberOfOffsets)

      specAn.Acp.Configuration.ConfigureOffsetArray("", offsetChannelInput.frequencyOffset, Nothing, Nothing)

      specAn.Acp.Configuration.ConfigureOffsetIntegrationBandwidth("offset::all", offsetChannelInput.integrationBandwidth)

      specAn.Acp.Configuration.ConfigureOffsetRrcFilter("offset::all", offsetChannelInput.rrcFilterEnabled, offsetChannelInput.rrcFilterAlpha)

      specAn.Initiate("", "")
   End Sub

   Private Sub RetrieveResults()
      ' Retrieve results 

      Dim spectrum As Spectrum(Of Single) = Nothing
      Dim totalRelativePower As Double, carrierFrequency As Double, integrationBandwidth As Double

      specAn.Acp.Results.FetchOffsetMeasurementArray("", timeout, lowerRelativePower, upperRelativePower, lowerAbsolutePower, upperAbsolutePower)

      specAn.Acp.Results.FetchCarrierMeasurement("", timeout, absolutePower, totalRelativePower, carrierFrequency, integrationBandwidth)

      specAn.Acp.Results.FetchSpectrum("", timeout, spectrum)
   End Sub

   Private Sub PrintResults()
      Console.WriteLine("-----------------Carrier Measurements-----------------" & vbLf)
      Console.WriteLine("Absolute Power (dBm or dBm/Hz)       {0}", absolutePower)

      Console.WriteLine(vbLf & "--------------Offset Channel Measurements-------------" & vbLf)
      For i As Integer = 0 To NumberOfOffsets - 1
         Console.WriteLine("----Offset {0}" & vbLf, i)
         Console.WriteLine("Lower Relative Power (dB)            {0}", lowerRelativePower(i))
         Console.WriteLine("Upper Relative Power (dB)            {0}", upperRelativePower(i))
         Console.WriteLine("Lower Absolute Power (dBm or dBm/Hz) {0}", lowerAbsolutePower(i))
         Console.WriteLine("Upper Absolute Power (dBm or dBm/Hz) {0}", upperAbsolutePower(i))
      Next
      Console.WriteLine("-------------------------------------------------" & vbLf)
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
