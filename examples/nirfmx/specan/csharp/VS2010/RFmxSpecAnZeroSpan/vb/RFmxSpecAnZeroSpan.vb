'Steps:
'1. Open a new RFmx session
'2. Configure the basic instrument properties (Clock Source and Clock Frequency)
'3. Configure Selected Ports
'4. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
'5. Select Spectrum measurement and enable the traces
'6. Configure Spectrum RBW filter
'7. Configure Spectrum Span to Zero
'8. Configure Spectrum Sweep Time Interval
'9. Configure Spectrum Averaging
'10. Initiate Measurement
'11. Fetch Spectrum Power Trace and Measurement
'12. Close the RFmx Session

Imports NationalInstruments
Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.SpecAnMX

Public Class RFmxSpecAnZeroSpan
   Private instrSession As RFmxInstrMX
   Private specAn As RFmxSpecAnMX

   Public Sub Run()
      Dim resourceName As String = "RFSA"
      Dim selectedPorts As String = ""
      Dim centerFrequency As Double = 1000000000.0 ' Hz 
      Dim referenceLevel As Double = 0.0 ' dBm 
      Dim externalAttenuation As Double = 0.0 ' dB 
      Dim frequency As Double = 10000000.0 ' Hz 
      Dim timeout As Double = 10.0 ' seconds 
      Dim frequencySource As String = RFmxInstrMXConstants.OnboardClock

      'RBW Filter
      Dim rbwFilterType As RFmxSpecAnMXSpectrumRbwFilterType = RFmxSpecAnMXSpectrumRbwFilterType.Gaussian
      Dim rbw As Double = 10000.0

      'Averaging 
      Dim averagingEnabled As RFmxSpecAnMXSpectrumAveragingEnabled = RFmxSpecAnMXSpectrumAveragingEnabled.[False]
      Dim averagingCount As Integer = 10
      Dim averagingType As RFmxSpecAnMXSpectrumAveragingType = RFmxSpecAnMXSpectrumAveragingType.Rms

      ' Sweep time 

      Dim sweepTimeInterval As Double = 0.001     ' seconds 

      Dim power As AnalogWaveform(Of Single) = Nothing

      Try
         ' Create a new RFmx Session 

         instrSession = New RFmxInstrMX(resourceName, "")

         ' Get SpecAn signal 

         specAn = instrSession.GetSpecAnSignalConfiguration()

         ' Configure measurement 

         instrSession.ConfigureFrequencyReference("", frequencySource, frequency)
         specAn.SetSelectedPorts("", selectedPorts)
         specAn.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
         specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Spectrum, True)
         specAn.Spectrum.Configuration.ConfigureRbwFilter("", RFmxSpecAnMXSpectrumRbwAutoBandwidth.False, rbw, rbwFilterType)
         specAn.Spectrum.Configuration.ConfigureSpan("", 0.0)
         'Zero Span
         specAn.Spectrum.Configuration.ConfigureSweepTime("", RFmxSpecAnMXSpectrumSweepTimeAuto.False, sweepTimeInterval)
         specAn.Spectrum.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount,
                                                          averagingType)
         specAn.Initiate("", "")

         ' Retrieve results 

         specAn.Spectrum.Results.FetchPowerTrace("", timeout, power)

         Console.WriteLine("Measurement Complete." & vbLf)
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
