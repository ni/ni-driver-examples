'Steps:
'1. Open a new RFmx session
'2. Configure Selected Ports
'3. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
'4. Configure ACP Averaging Parameters
'5. Configure ACP Integration BW, Number of Offset Channels and Channel Spacing
'This function configures a Carrier Channel with Offset Channels as specified by the Number of Offsets
'6. Read ACP Measurement Results
'This function returns Absolute Power for the Carrier Channel and Relative Powers for two Offset Channels
'7. Close the RFmx Session

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.SpecAnMX

Public Class RFmxSpecAnAcpBasic
   Private instrSession As RFmxInstrMX
   Private specAn As RFmxSpecAnMX

   Friend Sub Run()
      Dim resourceName As String = "RFSA"
      Dim selectedPorts As String = ""
      Dim centerFrequency As Double = 1000000000.0
      ' Hz 
      Dim referenceLevel As Double = 0.0
      ' dBm 
      Dim externalAttenuation As Double = 0.0
      ' dB 

      Dim integrationBandwidth As Double = 1000000.0
      ' Hz 
      Dim numberOfOffsetChannels As Integer = 2
      Dim channelSpacing As Double = 1000000.0
      ' Hz 

      Dim averagingEnabled As RFmxSpecAnMXAcpAveragingEnabled = RFmxSpecAnMXAcpAveragingEnabled.[False]
      Dim averagingType As RFmxSpecAnMXAcpAveragingType = RFmxSpecAnMXAcpAveragingType.Rms
      Dim averagingCount As Integer = 10
      Dim timeout As Double = 10
      ' seconds 

      Dim carrierAbsolutePower As Double, offCh0LowerRelativePower As Double,
          offCh0UpperRelativePower As Double, offCh1LowerRelativePower As Double,
          offCh1UpperRelativePower As Double

      Try
         ' Create a new RFmx Session 

         instrSession = New RFmxInstrMX(resourceName, "")

         ' Get SpecAn signal 

         specAn = instrSession.GetSpecAnSignalConfiguration()

         ' Configure measurement 

         specAn.SetSelectedPorts("", selectedPorts)
         specAn.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
         specAn.Acp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount,
                                                     averagingType)
         specAn.Acp.Configuration.ConfigureCarrierAndOffsets("", integrationBandwidth,
                                                             numberOfOffsetChannels, channelSpacing)

         ' Retrieve results 

         specAn.Acp.Results.Read("", timeout, carrierAbsolutePower, offCh0LowerRelativePower,
                                 offCh0UpperRelativePower, offCh1LowerRelativePower,
                                 offCh1UpperRelativePower)

         Console.WriteLine("Carrier Absolute Power (dBm or dBm/Hz) {0}", carrierAbsolutePower)
         Console.WriteLine("Offset ch0 Lower Relative Power (dB)   {0}", offCh0LowerRelativePower)
         Console.WriteLine("Offset ch0 Upper Relative Power(dB)    {0}", offCh0UpperRelativePower)
         Console.WriteLine("Offset ch1 Lower Relative Power(dB)    {0}", offCh1LowerRelativePower)
         Console.WriteLine("Offset ch1 Upper Relative Power(dB)    {0}", offCh1UpperRelativePower)
      Catch ex As Exception
         DisplayError(ex.Message)
      Finally
         ' Close session 

         CloseSession()
         Console.WriteLine("Press any key to exit.....")
         Console.ReadKey()
      End Try
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

   Private Sub DisplayError(message As String)
      Console.WriteLine("ERROR:" & vbLf & message)
   End Sub
End Class
