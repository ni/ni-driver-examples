'Steps:
'1. Open a new RFmx session
'2. Configure the basic instrument properties (Clock Source and Clock Frequency)
'3. Configure Selected Ports
'4. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
'5. Configure IQ Power Edge Trigger properties (Trigger Delay, IQ Power Edge Level, Min Quiet Time)
'6. Configure IQ measurement
'7. Configure Acquisition parameters
'8. Initiate Measurement
'9. Fetch IQ Data
'10. Mean Power calculation - Power in dBm = 10* log (((I^2+Q^2)/(2*R))/1mW), where R=50 Ohms
'11. Close the RFmx Session

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.SpecAnMX

Public Class RFmxSpecAnIQ
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

      Dim frequencySource As String = RFmxInstrMXConstants.OnboardClock
      Dim frequency As Double = 10000000.0
      ' Hz 
      Dim iqPowerEdgeEnabled As Boolean = False
      Dim iqPowerEdgeLevel As Double = -20.0
      ' dBm
      Dim triggerDelay As Double = 0.0
      ' seconds
      Dim minimumQuietTime As Double = 0.0
      ' seconds

      Dim sampleRate As Double = 10000000.0
      ' samples per second
      Dim acquisitionTime As Double = 0.001
      ' seconds

      Dim meanPower As Double, meanPowerInDbm As Double

      Dim timeout As Double = 10
      ' seconds 
      Dim recordToFetch As Integer = 0
      Dim samplesToRead As Long = -1

      Dim data As ComplexWaveform(Of ComplexSingle) = Nothing
      Dim sum As Double = 0.0
      Dim realArray As Double()
      Dim imaginaryArray As Double()

      Try
         ' Create a new RFmx Session 

         instrSession = New RFmxInstrMX(resourceName, "")

         ' Get SpecAn signal 

         specAn = instrSession.GetSpecAnSignalConfiguration()

         ' Configure measurement 

         instrSession.ConfigureFrequencyReference("", frequencySource, frequency)
         specAn.SetSelectedPorts("", selectedPorts)
         specAn.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
         specAn.ConfigureIQPowerEdgeTrigger("", "0", iqPowerEdgeLevel, RFmxSpecAnMXIQPowerEdgeTriggerSlope.Rising,
            triggerDelay, RFmxSpecAnMXTriggerMinimumQuietTimeMode.Manual, minimumQuietTime, iqPowerEdgeEnabled)
         specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.IQ, False)
         specAn.IQ.Configuration.ConfigureAcquisition("", sampleRate, 1, acquisitionTime, 0)
         specAn.Initiate("", "")

         ' Retrieve results 

         specAn.IQ.Results.FetchData("", timeout, recordToFetch, samplesToRead, data)

         realArray = data.GetRealDataArray(False)
         imaginaryArray = data.GetImaginaryDataArray(False)

         For i As Integer = 0 To data.SampleCount - 1
            sum += (realArray(i) * realArray(i)) + (imaginaryArray(i) * imaginaryArray(i))
         Next

         meanPower = sum / CDbl(data.SampleCount)
         meanPowerInDbm = 10 * Math.Log10(meanPower / (2 * 50) / 0.001)

         Console.WriteLine("Mean Power (dBm) {0}", meanPowerInDbm)
      Catch ex As Exception
         DisplayError(ex)
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
         DisplayError(ex)
      End Try
   End Sub

   Private Sub DisplayError(ex As Exception)
      Console.WriteLine("ERROR:" & vbLf & Convert.ToString(ex.[GetType]()) & ": " & ex.Message)
   End Sub
End Class
