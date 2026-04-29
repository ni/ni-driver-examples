'Steps:
'1. Open a new RFmx session
'2. Configure Selected Ports
'3. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
'4. Select ACP,TXP measurements in the measurements Array (to perform composite measurement)
'5. Configure ACP Averaging
'6. Configure ACP Integration BW, Number of Offset Channels and Channel Spacing
'This function configures a Carrier Channel with Offset Channels as specified by the Number of Offsets
'Refer to VI help for more information.
'7. Configure ACP Measurement Interval and RBW
'8. Configure TXP Averaging
'9. Initiate Measurement
'10. Fetch ACP Carrier and Offset Measurements
'11. Fetch TXP Measurement
'12. Close the RFmx Session

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.SpecAnMX

Class RFmxSpecAnAcpTxpComposite
   Private instrSession As RFmxInstrMX
   Private specAn As RFmxSpecAnMX
   Private resourceName As String
   Const NumberOfOffsetChannels As Integer = 2
   Private selectedPorts As String
   Private centerFrequency As Double, referenceLevel As Double, externalAttenuation As Double,
           rrcAlpha As Double, integrationBandwidth As Double, channelSpacing As Double,
           timeout As Double, measurementInterval As Double, rbw As Double
   Private acpAveragingCount As Integer, txpAveragingCount As Integer
   Private acpAveragingEnabled As RFmxSpecAnMXAcpAveragingEnabled
   Private txpAveragingEnabled As RFmxSpecAnMXTxpAveragingEnabled
   Private acpAveragingType As RFmxSpecAnMXAcpAveragingType
   Private txpAveragingType As RFmxSpecAnMXTxpAveragingType
   Private rbwFilterType As RFmxSpecAnMXTxpRbwFilterType
   Private enableAllTraces As Boolean

   Private carrierAbsolutePower As Double, offCh0LowerRelativePower As Double,
           offCh0UpperRelativePower As Double, offCh1LowerRelativePower As Double,
           offCh1UpperRelativePower As Double, averageMeanPower As Double,
           peakToAverageRatio As Double, maxPower As Double, minPower As Double
   Private relativePower As Double, carrierFrequency As Double, resIntegrationBandwidth As Double,
           offCh1UpperAbsolutePower As Double, offCh1LowerAbsolutePower As Double,
           offCh0UpperAbsolutePower As Double, offCh0LowerAbsolutePower As Double


   Friend Sub Run()
      Try
         InitializeVariable()
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

   Private Sub InitializeVariable()
      resourceName = "RFSA"
      selectedPorts = ""
      centerFrequency = 1000000000.0
      ' Hz 
      referenceLevel = 0.0
      ' dBm 
      externalAttenuation = 0.0
      ' dB 
      enableAllTraces = True
      timeout = 10
      ' seconds 

      ' ACP
      integrationBandwidth = 1000000.0
      ' Hz 
      channelSpacing = 1000000.0
      ' Hz 
      acpAveragingEnabled = RFmxSpecAnMXAcpAveragingEnabled.[False]
      acpAveragingCount = 10
      acpAveragingType = RFmxSpecAnMXAcpAveragingType.Rms

      ' Txp
      measurementInterval = 0.001
      ' seconds 
      rbw = 100000.0
      ' Hz 
      rbwFilterType = RFmxSpecAnMXTxpRbwFilterType.Gaussian
      txpAveragingEnabled = RFmxSpecAnMXTxpAveragingEnabled.[False]
      txpAveragingCount = 10
      txpAveragingType = RFmxSpecAnMXTxpAveragingType.Rms
      rrcAlpha = 0.1
   End Sub

   Private Sub InitializeInstr()
      ' Create a new RFmx Session 

      instrSession = New RFmxInstrMX(resourceName, "")
   End Sub

   Private Sub ConfigureSpecAn()
      ' Get SpecAn signal 

      specAn = instrSession.GetSpecAnSignalConfiguration()

      ' Configure measurements 

      specAn.SetSelectedPorts("", selectedPorts)
      specAn.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
      specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Acp Or RFmxSpecAnMXMeasurementTypes.Txp,
                                enableAllTraces)

      'ACP
      specAn.Acp.Configuration.ConfigureAveraging("", acpAveragingEnabled, acpAveragingCount,
                                                  acpAveragingType)
      specAn.Acp.Configuration.ConfigureCarrierAndOffsets("", integrationBandwidth,
                                                          NumberOfOffsetChannels,
                                                          channelSpacing)

      'TXP
      specAn.Txp.Configuration.ConfigureMeasurementInterval("", measurementInterval)
      specAn.Txp.Configuration.ConfigureRbwFilter("", rbw, rbwFilterType, rrcAlpha)
      specAn.Txp.Configuration.ConfigureAveraging("", txpAveragingEnabled,
                                                  txpAveragingCount,
                                                  txpAveragingType)

      specAn.Initiate("", "")
   End Sub

   Private Sub RetrieveResults()
      ' Retrieve results 

      'ACP
      Dim selectorString As String = RFmxSpecAnMX.BuildCarrierString2("", 0)
      specAn.Acp.Results.FetchCarrierMeasurement(selectorString, timeout,
                                                 carrierAbsolutePower,
                                                 relativePower, carrierFrequency,
                                                 resIntegrationBandwidth)

      selectorString = RFmxSpecAnMX.BuildOffsetString2("", 0)
      specAn.Acp.Results.FetchOffsetMeasurement(selectorString, timeout,
                                                offCh0LowerRelativePower,
                                                offCh0UpperRelativePower,
                                                offCh0LowerAbsolutePower,
                                                offCh0UpperAbsolutePower)

      selectorString = RFmxSpecAnMX.BuildOffsetString2("", 1)
      specAn.Acp.Results.FetchOffsetMeasurement(selectorString, timeout,
                                                offCh1LowerRelativePower,
                                                offCh1UpperRelativePower,
                                                offCh1LowerAbsolutePower,
                                                offCh1UpperAbsolutePower)

      'TXP
      specAn.Txp.Results.FetchMeasurement("", timeout, averageMeanPower,
                                          peakToAverageRatio, maxPower,
                                          minPower)
   End Sub

   Private Sub PrintResults()
      Console.WriteLine("------------------------ACP-----------------------------" & vbLf)
      Console.WriteLine("Carrier Abs Power (dBm or dBm/Hz)        {0}", carrierAbsolutePower)
      Console.WriteLine("Offset ch0 Lower Relative Power (dB)     {0}", offCh0LowerRelativePower)
      Console.WriteLine("Offset ch0 Upper Relative Power (dB)     {0}", offCh0UpperRelativePower)
      Console.WriteLine("Offset ch1 Lower Relative Power (dB)     {0}", offCh1LowerRelativePower)
      Console.WriteLine("Offset ch1 Upper Relative Power (dB)     {0}", offCh1UpperRelativePower)

      Console.WriteLine(vbLf & "------------------------TXP-----------------------------" & vbLf)
      Console.WriteLine("Average Mean Power    (dBm)              {0}", averageMeanPower)
      Console.WriteLine("Peak to Average Ratio (dB)               {0}", peakToAverageRatio)
      Console.WriteLine("Maximum Power         (dBm)              {0}", maxPower)
      Console.WriteLine("Minimum Power         (dBm)              {0}", minPower)

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
