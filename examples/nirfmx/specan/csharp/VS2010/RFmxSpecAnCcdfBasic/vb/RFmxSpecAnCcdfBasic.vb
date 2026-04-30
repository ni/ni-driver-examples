'Steps:
'1. Open a new RFmx session
'2. Configure Selected Ports
'3. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
'4. Configure CCDF Measurement Interval
'5. Configure CCDF RBW
'6. Read CCDF Measurement Results
'7. Close the RFmx Session

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.SpecAnMX

Public Class RFmxSpecAnCcdfBasic
   Private instrSession As RFmxInstrMX
   Private specAn As RFmxSpecAnMX

   Public Sub Run()
      Dim resourceName As String = "RFSA"
      Dim selectedPorts As String = ""
      Dim centerFrequency As Double = 1000000000.0  'Hz 
      Dim referenceLevel As Double = 0.0            'dBm
      Dim externalAttenuation As Double = 0.0       'dB
      Dim measurementInterval As Double = 0.001     'seconds
      Dim rbw As Double = 100000.0                  'Hz
      Dim enableAllTraces As Boolean = True
      Dim timeout As Double = 10.0                  'seconds
      Dim meanPower As Double                       'dBm
      Dim meanPowerPercentile As Double             '%
      Dim peakPower As Double                       'dB 
      Dim measuredSamplesCount As Integer

      Try
         ' Create a new RFmx Session 

         instrSession = New RFmxInstrMX(resourceName, "")

         ' Get SpecAn signal 

         specAn = instrSession.GetSpecAnSignalConfiguration()

         ' Configure measurement 

         specAn.SetSelectedPorts("", selectedPorts)
         specAn.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
         specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Ccdf, enableAllTraces)
         specAn.Ccdf.Configuration.ConfigureMeasurementInterval("", measurementInterval)
         specAn.Ccdf.Configuration.SetRbwFilterBandwidth("", rbw)

         ' Retrieve results 

         specAn.Ccdf.Results.Read("", timeout, meanPower, meanPowerPercentile, peakPower, measuredSamplesCount)

         Console.WriteLine(" Mean Power (dBm)           {0}", meanPower)
         Console.WriteLine(" Mean Power Percentile (%)  {0}", meanPowerPercentile)
         Console.WriteLine(" Peak Power (dB)            {0}", peakPower)
         Console.WriteLine(" Measured Samples Count     {0}", measuredSamplesCount)
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
