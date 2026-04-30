'Steps:
'1. Open a new RFmx session
'2. Configure Selected Ports
'3. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
'4. Configure TXP RBW
'5. Configure TXP Measurement Interval
'6. Configure TXP Averaging
'7. Read TXP Measurement Results
'8. Close the RFmx Session

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.SpecAnMX

Public Class RFmxSpecAnTxpBasic
   Private instrSession As RFmxInstrMX
   Private specAn As RFmxSpecAnMX

   Public Sub Run()
      Dim resourceName As String = "RFSA"
      Dim selectedPorts As String = ""
      Dim centerFrequency As Double = 1000000000.0 ' Hz 
      Dim referenceLevel As Double = 0.0 ' dBm 
      Dim externalAttenuation As Double = 0.0 ' dB 
      Dim measurementInterval As Double = 0.001 ' seconds 
      Dim timeout As Double = 10 ' seconds 

      'RBW Filter
      Dim rbw As Double = 100000.0
      Dim rbwFilterType As RFmxSpecAnMXTxpRbwFilterType = RFmxSpecAnMXTxpRbwFilterType.Gaussian
      Dim rrcAlpha As Double = 0.1

      'Averaging
      Dim averagingEnabled As RFmxSpecAnMXTxpAveragingEnabled = RFmxSpecAnMXTxpAveragingEnabled.[False]
      Dim averagingCount As Integer = 10
      Dim averagingType As RFmxSpecAnMXTxpAveragingType = RFmxSpecAnMXTxpAveragingType.Rms

      Dim averageMeanPower As Double, peakToAverageRatio As Double, maximumPower As Double,
          minimumPower As Double

      Try
         ' Create a new RFmx Session 

         instrSession = New RFmxInstrMX(resourceName, "")

         ' Get SpecAn signal 

         specAn = instrSession.GetSpecAnSignalConfiguration()

         ' Configure measurement 

         specAn.SetSelectedPorts("", selectedPorts)
         specAn.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
         specAn.Txp.Configuration.ConfigureMeasurementInterval("", measurementInterval)
         specAn.Txp.Configuration.ConfigureRbwFilter("", rbw, rbwFilterType, rrcAlpha)
         specAn.Txp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount,
                                                     averagingType)

         ' Retrieve results 

         specAn.Txp.Results.Read("", timeout, averageMeanPower, peakToAverageRatio,
                                 maximumPower, minimumPower)

         Console.WriteLine("Average Mean Frequency (Hz)  " & averageMeanPower)
         Console.WriteLine("Mean Phase (deg)             " & peakToAverageRatio)
         Console.WriteLine("Maximum Power (dBm)          " & maximumPower)

         Console.WriteLine("Minimum Power (dBm)          " & minimumPower)
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
