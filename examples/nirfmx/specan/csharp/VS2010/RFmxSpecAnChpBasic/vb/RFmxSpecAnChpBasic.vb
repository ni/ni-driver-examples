'Steps:
'1. Open a new RFmx session
'2. Configure Selected Ports
'3. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
'4. Configure CHP Integration BW
'5. Configure CHP Averaging
'6. Read CHP Measurement Results
'7. Close the RFmx Session

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.SpecAnMX

Public Class RFmxSpecAnChpBasic
   Private instrSession As RFmxInstrMX
   Private specAn As RFmxSpecAnMX

   Friend Sub Run()
      Dim resourceName As String = "RFSA"
      Dim selectedPorts As String = ""
      Dim centerFrequency As Double = 1000000000.0 ' Hz 
      Dim referenceLevel As Double = 0.0 ' dBm 
      Dim externalAttenuation As Double = 0.0 ' dB 
      Dim timeout As Double = 10.0 ' seconds 
      Dim integrationBandwidth As Double = 1000000.0 ' Hz 
      Dim averagingEnabled As RFmxSpecAnMXChpAveragingEnabled = RFmxSpecAnMXChpAveragingEnabled.[False]
      Dim averagingCount As Integer = 10
      Dim absolutePower As Double ' dBm 
      Dim psd As Double ' dBm/Hz 

      Try
         ' Create a new RFmx Session 

         instrSession = New RFmxInstrMX(resourceName, "")

         ' Get SpecAn signal 

         specAn = instrSession.GetSpecAnSignalConfiguration()

         ' Configure measurement 

         specAn.SetSelectedPorts("", selectedPorts)
         specAn.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
         specAn.Chp.Configuration.ConfigureIntegrationBandwidth("", integrationBandwidth)
         specAn.Chp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount,
                                                     RFmxSpecAnMXChpAveragingType.Rms)

         ' Retrieve results 

         specAn.Chp.Results.Read("", timeout, absolutePower, psd)

         Console.WriteLine("Absolute Power (dBm) {0}", absolutePower)

         Console.WriteLine("PSD (dBm/Hz)         {0}", psd)
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
