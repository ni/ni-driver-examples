'Steps:
'1. Open a new RFmx session
'2. Configure Selected Ports
'3. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
'4. Configure Harm RBW
'5. Configure Harm Measurement Interval
'6. Configure Harm Number of Harmonics
'7. Configure Harm Averaging
'8. Read Harmonics Measurement Results
'9. Close the RFmx Session

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.SpecAnMX

Public Class RFmxSpecAnMXHarmBasic
   Private instrSession As RFmxInstrMX
   Private specAn As RFmxSpecAnMX

   Public Sub Run()
      Dim resourceName As String = "RFSA"
      Dim selectedPorts As String = ""
      Dim centerFrequency As Double = 1000000000.0 ' Hz 
      Dim referenceLevel As Double = 0.0 ' dBm 
      Dim externalAttenuation As Double = 0.0 ' dB 

      ' Fundamental
      Dim rbw As Double = 100000.0 ' Hz 
      Dim measurementInterval As Double = 0.001 ' seconds 
      Dim rbwFilterType As RFmxSpecAnMXHarmRbwFilterType = RFmxSpecAnMXHarmRbwFilterType.Gaussian
      Dim rrcAlpha As Double = 0.1

      Dim numberOfHarmonics As Integer = 3

      'Averaging 
      Dim averagingEnabled As RFmxSpecAnMXHarmAveragingEnabled = RFmxSpecAnMXHarmAveragingEnabled.[False]
      Dim averagingCount As Integer = 10
      Dim averagingType As RFmxSpecAnMXHarmAveragingType = RFmxSpecAnMXHarmAveragingType.Rms

      Dim timeout As Double = 10.0
      ' seconds 

      Dim totalHarmonicDistortion As Double, averageFundamentalPower As Double

      Try
         ' Create a new RFmx Session 

         instrSession = New RFmxInstrMX(resourceName, "")

         ' Get SpecAn signal 

         specAn = instrSession.GetSpecAnSignalConfiguration()

         ' Configure measurement 

         specAn.SetSelectedPorts("", selectedPorts)
         specAn.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
         specAn.Harm.Configuration.ConfigureFundamentalRbw("", rbw, rbwFilterType, rrcAlpha)
         specAn.Harm.Configuration.ConfigureFundamentalMeasurementInterval("", measurementInterval)
         specAn.Harm.Configuration.ConfigureNumberOfHarmonics("", numberOfHarmonics)
         specAn.Harm.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount,
                                                      averagingType)

         ' Retrieve results 

         specAn.Harm.Results.Read("", timeout, totalHarmonicDistortion, averageFundamentalPower)

         Console.WriteLine("Total Harmonic Distoration (%) {0}" & vbLf, totalHarmonicDistortion)

         Console.WriteLine("Average Fundamental Power (dBm) {0}" & vbLf, averageFundamentalPower)

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
