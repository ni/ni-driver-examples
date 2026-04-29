'Steps:
'1. Open a new RFmx session
'2. Configure Selected Ports
'3. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
'4. Configure Spectrum Span
'5. Configure Spectrum RBW filter
'6. Configure Spectrum Averaging
'7. Read Spectrum Measurement Results
'8. Close the RFmx Session

Imports NationalInstruments
Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.SpecAnMX

Public Class RFmxSpecAnSpectrumBasic
   Private instrSession As RFmxInstrMX
   Private specAn As RFmxSpecAnMX

   Public Sub Run()
      Dim resourceName As String = "RFSA"
      Dim selectedPorts As String = ""
      Dim centerFrequency As Double = 1000000000.0 ' Hz 
      Dim referenceLevel As Double = 0.0 ' dBm 
      Dim externalAttenuation As Double = 0.0 ' dB 
      Dim timeout As Double = 10 ' seconds 
      Dim span As Double = 1000000.0 ' Hz 

      'RBW Filter
      Dim rbwAuto As RFmxSpecAnMXSpectrumRbwAutoBandwidth = RFmxSpecAnMXSpectrumRbwAutoBandwidth.[True]
      Dim rbw As Double = 100000.0
      Dim rbwFilterType As RFmxSpecAnMXSpectrumRbwFilterType = RFmxSpecAnMXSpectrumRbwFilterType.Gaussian

      'Averaging 
      Dim averagingEnabled As RFmxSpecAnMXSpectrumAveragingEnabled = RFmxSpecAnMXSpectrumAveragingEnabled.[False]
      Dim averagingCount As Integer = 10
      Dim averagingType As RFmxSpecAnMXSpectrumAveragingType = RFmxSpecAnMXSpectrumAveragingType.Rms

      Dim spectrum As Spectrum(Of Single) = Nothing

      Try
         ' Create a new RFmx Session 

         instrSession = New RFmxInstrMX(resourceName, "")

         ' Get SpecAn signal 

         specAn = instrSession.GetSpecAnSignalConfiguration()

         ' Configure measurement 

         specAn.SetSelectedPorts("", selectedPorts)
         specAn.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
         specAn.Spectrum.Configuration.ConfigureSpan("", span)
         specAn.Spectrum.Configuration.ConfigureRbwFilter("", rbwAuto, rbw, rbwFilterType)
         specAn.Spectrum.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount,
                                                          averagingType)

         ' Retrieve results 

         specAn.Spectrum.Results.Read("", timeout, spectrum)

         Console.WriteLine("Start Frequency(Hz)       {0}", spectrum.StartFrequency)
         Console.WriteLine("Frequency Increment(Hz)   {0}", spectrum.FrequencyIncrement)
         Console.WriteLine("Sample Count              {0}", spectrum.SampleCount)

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
