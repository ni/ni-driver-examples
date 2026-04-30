'Steps:
'1. Open a new RFmx session
'2. Configure Selected Ports
'3. Load configuration from rfmxconfig file
'4. Configure Frequency Reference
'5. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
'6. Initiate Measurement
'7. Fetch ACP Measurements and Traces
'8. Close the RFmx Session

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.SpecAnMX

Public Class RFmxSpecAnAcpFromConfigurationFile
   Private instrSession As RFmxInstrMX
   Private specAn As RFmxSpecAnMX
   Private resourceName As String
   Private configurationFileName As String

   Private selectedPorts As String
   Private centerFrequency As Double, referenceLevel As Double, externalAttenuation As Double
   Private frequency As Double
   Private timeout As Double
   Private frequencySource As String

   'Output values
   Private absolutePower As Double
   Private lowerRelativePower As Double()
   Private upperRelativePower As Double()
   Private lowerAbsolutePower As Double()
   Private upperAbsolutePower As Double()

   Friend Sub Run()
      resourceName = "RFSA"
      selectedPorts = ""
      centerFrequency = 1000000000.0
      ' Hz 
      referenceLevel = 0.0
      ' dBm 
      externalAttenuation = 0.0
      ' dB 
      frequencySource = RFmxInstrMXConstants.OnboardClock
      frequency = 10000000.0
      ' Hz 
      configurationFileName = "SpecAn_Configurations.rfmxconfig"
      timeout = 10.0
      ' seconds 

      Try
         ' Create a new RFmx Session 
         instrSession = New RFmxInstrMX(resourceName, "")
         ' Get SpecAn signal 
         specAn = instrSession.GetSpecAnSignalConfiguration()

         ' Configure measurement 
         instrSession.LoadAllConfigurations(configurationFileName, True)
         instrSession.ConfigureFrequencyReference("", frequencySource, frequency)
         specAn.SetSelectedPorts("", selectedPorts)
         specAn.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)

         specAn.Initiate("", "")
         ' Retrieve results 
         Dim spectrum As Spectrum(Of Single) = Nothing
         Dim totalRelativePower As Double, carrierFrequency As Double, integrationBandwidth As Double

         specAn.Acp.Results.FetchOffsetMeasurementArray("", timeout, lowerRelativePower, upperRelativePower,
                                                        lowerAbsolutePower, upperAbsolutePower)

         specAn.Acp.Results.FetchCarrierMeasurement("", timeout, absolutePower, totalRelativePower,
                                                    carrierFrequency, integrationBandwidth)

         specAn.Acp.Results.FetchSpectrum("", timeout, spectrum)

         Console.WriteLine("-----------------Carrier Measurements-----------------" & vbLf)
         Console.WriteLine("Absolute Power (dBm or dBm/Hz)       {0}", absolutePower)

         Console.WriteLine(vbLf & "--------------Offset Channel Measurements-------------" & vbLf)
         For i As Integer = 0 To lowerRelativePower.Length - 1
            Console.WriteLine("----Offset 0" & vbLf, i)
            Console.WriteLine("Lower Relative Power (dB)            {0}", lowerRelativePower(i))
            Console.WriteLine("Upper Relative Power (dB)            {0}", upperRelativePower(i))
            Console.WriteLine("Lower Absolute Power (dBm or dBm/Hz) {0}", lowerAbsolutePower(i))
            Console.WriteLine("Upper Absolute Power (dBm or dBm/Hz) {0}", upperAbsolutePower(i))
         Next
         Console.WriteLine("-------------------------------------------------" & vbLf)

      Catch ex As Exception
         DisplayError(ex)
      Finally
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

   Private Shared Sub DisplayError(ex As Exception)
      Console.WriteLine("ERROR:" & vbLf & Convert.ToString(ex.[GetType]()) & ": " & ex.Message)
   End Sub
End Class
