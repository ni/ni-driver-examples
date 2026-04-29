'Steps:
'1. Open a new RFmx session
'2. Configure Selected Ports
'3. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
'4. Configure OBW Span
'5. Configure OBW Averaging
'6. Read OBW Measurement Results
'7. Close the RFmx Session

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.SpecAnMX

Class RFmxSpecAnMXObwBasic
   Private instrSession As RFmxInstrMX
   Private specAn As RFmxSpecAnMX

   Public Sub Run()
      Dim resourceName As String = "RFSA"
      Dim selectedPorts As String = ""
      Dim centerFrequency As Double = 1000000000.0 ' Hz 
      Dim referenceLevel As Double = 0.0 ' dBm 
      Dim externalAttenuation As Double = 0.0 ' dB 
      Dim span As Double = 1000000.0 ' Hz 
      Dim timeout As Double = 10 ' seconds 
      Dim averagingEnabled As RFmxSpecAnMXObwAveragingEnabled = RFmxSpecAnMXObwAveragingEnabled.[False]
      Dim averagingType As RFmxSpecAnMXObwAveragingType = RFmxSpecAnMXObwAveragingType.Rms
      Dim averagingCount As Integer = 10

      Dim stopFrequency As Double, startFrequency As Double, occupiedBandwidth As Double,
          averagePower As Double, frequencyResolution As Double

      Try
         ' Create a new RFmx Session 

         instrSession = New RFmxInstrMX(resourceName, "")

         ' Get SpecAn signal 

         specAn = instrSession.GetSpecAnSignalConfiguration()

         ' Configure measurement 

         specAn.SetSelectedPorts("", selectedPorts)
         specAn.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
         specAn.Obw.Configuration.ConfigureSpan("", span)
         specAn.Obw.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount,
                                                     averagingType)

         ' Retrieve results 

         specAn.Obw.Results.Read("", timeout, occupiedBandwidth, averagePower,
                                 frequencyResolution, startFrequency, stopFrequency)

         Console.WriteLine("Occupied Bandwidth (Hz)   {0}", occupiedBandwidth)
         Console.WriteLine("Average total Power (dBm) {0}", averagePower)
         Console.WriteLine("Frequency Resolution (Hz) {0}", frequencyResolution)
         Console.WriteLine("Start Frequency (Hz)      {0}", startFrequency)
         Console.WriteLine("Stop Frequency (Hz)       {0}", stopFrequency)

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
