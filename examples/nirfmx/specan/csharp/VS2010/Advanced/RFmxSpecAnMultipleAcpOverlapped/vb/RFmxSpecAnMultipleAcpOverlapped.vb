'Steps:
'1. Open a new RFmx session
'2. Configure Selected Ports
'3.A. Configure the basic signal properties - Center Frequency and External Attenuation
'3.B. Configure the Reference Level to be used in the first run of ACP Measurement
'4. Select ACP measurement and enable the traces
'5. Configure Averaging parameters for the ACP Measurement
'6. Configure Integration BW of the Carrier channel, Number of Offset Channels, Channel Spacing
'7. Initiate ACP Measurement with a Result name.
'   When Result name is wired to Initiate same result name should be used to retrieve results from session.
'8. Wait for ACP_1 acquisition to complete
'9. Configure the Reference level to be used in the second run of ACP Measurement
'10. Initiate another ACP Measurement with a Result name. Use this result name while retrieving the results from session
'11. Fetch ACP_1 Measurement Results using Result name for Selector String
'12. Fetch ACP_2 Measurement Results using Result name for Selector String
'13. Close the RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.SpecAnMX


Public Class RFmxSpecAnMultipleAcpOverlapped
   Private instrSession As RFmxInstrMX
   Private specAn As RFmxSpecAnMX
   Private resourceName As String, result1Name As String, result2Name As String
   Private selectedPorts As String
   Private centerFrequency As Double, referenceLevel1 As Double, referenceLevel2 As Double, externalAttenuation As Double, integrationBandwidth As Double, channelSpacing As Double,
    timeout As Double
   Private averagingCount As Integer
   Private averagingEnabled As RFmxSpecAnMXAcpAveragingEnabled
   Private averagingType As RFmxSpecAnMXAcpAveragingType

   Const NumberOfOffsetChannels As Integer = 2

   Private Structure AcpMeasurement
      Public carrierAbsolutePower As Double
      Public relativePower As Double
      Public carrierFrequency As Double
      Public resIntegrationBandwidth As Double
      Public lowerRelativePower As Double()
      Public upperRelativePower As Double()
      Public lowerAbsolutePower As Double()
      Public upperAbsolutePower As Double()
   End Structure

   Private acpMeasurement1 As AcpMeasurement, acpMeasurement2 As AcpMeasurement

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
      referenceLevel1 = 0.0
      ' dBm 
      referenceLevel2 = -10.0
      ' dBm 
      externalAttenuation = 0.0
      ' dB 
      timeout = 10
      ' seconds 

      ' ACP
      integrationBandwidth = 1000000.0
      ' Hz 
      channelSpacing = 1000000.0
      ' Hz 
      averagingEnabled = RFmxSpecAnMXAcpAveragingEnabled.[False]
      averagingCount = 10
      averagingType = RFmxSpecAnMXAcpAveragingType.Rms

      acpMeasurement1.lowerRelativePower = New Double(NumberOfOffsetChannels - 1) {}
      acpMeasurement1.upperRelativePower = New Double(NumberOfOffsetChannels - 1) {}

      acpMeasurement2.lowerRelativePower = New Double(NumberOfOffsetChannels - 1) {}
      acpMeasurement2.upperRelativePower = New Double(NumberOfOffsetChannels - 1) {}
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
      specAn.ConfigureFrequency("", centerFrequency)
      specAn.ConfigureExternalAttenuation("", externalAttenuation)
      specAn.ConfigureReferenceLevel("", referenceLevel1)
      specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Acp, False)
      specAn.Acp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)
      specAn.Acp.Configuration.ConfigureCarrierAndOffsets("", integrationBandwidth, NumberOfOffsetChannels, channelSpacing)

      result1Name = RFmxSpecAnMX.BuildResultString("ACP_Results_1")
      specAn.Initiate("", result1Name)
      instrSession.WaitForAcquisitionComplete(timeout)

      result2Name = RFmxSpecAnMX.BuildResultString("ACP_Results_2")
      specAn.ConfigureReferenceLevel("", referenceLevel2)
      specAn.Initiate("", result2Name)
   End Sub

   Private Sub RetrieveResults()
      specAn.Acp.Results.FetchCarrierMeasurement(result1Name, timeout, acpMeasurement1.carrierAbsolutePower,
                                                 acpMeasurement1.relativePower, acpMeasurement1.carrierFrequency,
                                                 acpMeasurement1.resIntegrationBandwidth)

      specAn.Acp.Results.FetchOffsetMeasurementArray(result1Name, timeout, acpMeasurement1.lowerRelativePower,
                                                     acpMeasurement1.upperRelativePower, acpMeasurement1.lowerAbsolutePower,
                                                     acpMeasurement1.upperAbsolutePower)

      specAn.Acp.Results.FetchCarrierMeasurement(result2Name, timeout, acpMeasurement2.carrierAbsolutePower,
                                                 acpMeasurement2.relativePower, acpMeasurement2.carrierFrequency,
                                                 acpMeasurement2.resIntegrationBandwidth)

      specAn.Acp.Results.FetchOffsetMeasurementArray(result2Name, timeout, acpMeasurement2.lowerRelativePower,
                                                     acpMeasurement2.upperRelativePower, acpMeasurement2.lowerAbsolutePower,
                                                     acpMeasurement2.upperAbsolutePower)
   End Sub

   Private Sub PrintResults()
      Console.WriteLine("------------------------ACP Measurement1-----------------------------" & vbLf)
      Console.WriteLine("Carrier Abs Power (dBm or dBm/Hz)   {0}", acpMeasurement1.carrierAbsolutePower)
      For i As Integer = 0 To NumberOfOffsetChannels - 1
         Console.WriteLine(vbLf & "Offset Channel : {0}", i)
         Console.WriteLine("Lower Relative Power (dB)           {0}", acpMeasurement1.lowerRelativePower(i))
         Console.WriteLine("Upper Relative Power (dB)           {0}", acpMeasurement1.upperRelativePower(i))
      Next

      Console.WriteLine(vbLf & "------------------------ACP Measurement2-----------------------------" & vbLf)
      Console.WriteLine("Carrier Abs Power (dBm or dBm/Hz)   {0}", acpMeasurement2.carrierAbsolutePower)
      For i As Integer = 0 To NumberOfOffsetChannels - 1
         Console.WriteLine(vbLf & "Offset Channel : {0}", i)
         Console.WriteLine("Lower Relative Power (dB)           {0}", acpMeasurement2.lowerRelativePower(i))
         Console.WriteLine("Upper Relative Power (dB)           {0}", acpMeasurement2.upperRelativePower(i))
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

   Private Shared Sub DisplayError(ByVal ex As Exception)
      Console.WriteLine("ERROR:" & vbLf & Convert.ToString(ex.[GetType]()) & ": " & ex.Message)
   End Sub
End Class
