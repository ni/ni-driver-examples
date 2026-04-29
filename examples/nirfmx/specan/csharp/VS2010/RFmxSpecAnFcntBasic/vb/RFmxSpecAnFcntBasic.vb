'Steps:
'1. Open a new RFmx session
'2. Configure Selected Ports
'3. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
'4. Configure FCnt Measurement Interval
'5. Configure FCnt Averaging
'6. Configure FCnt RBW
'7. Read FCnt Measurement Results
'8. Close the RFmx Session

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.SpecAnMX

Public Class RFmxSpecAnFcntBasic
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

      Dim averagingEnabled As RFmxSpecAnMXFcntAveragingEnabled = RFmxSpecAnMXFcntAveragingEnabled.[False]
      Dim averagingCount As Integer = 10
      Dim averagingType As RFmxSpecAnMXFcntAveragingType = RFmxSpecAnMXFcntAveragingType.Mean

      Dim rbw As Double = 100000.0
      Dim rrcAlpha As Double = 0.1
      Dim rbwFilterType As RFmxSpecAnMXFcntRbwFilterType = RFmxSpecAnMXFcntRbwFilterType.None

      Dim averageRelativeFrequency As Double ' Hz 
      Dim averageAbsoluteFrequency As Double ' Hz 
      Dim meanPhase As Double ' deg 

      Try
         ' Create a new RFmx Session 

         instrSession = New RFmxInstrMX(resourceName, "")

         ' Get SpecAn signal 

         specAn = instrSession.GetSpecAnSignalConfiguration()

         ' Configure measurement 

         specAn.SetSelectedPorts("", selectedPorts)
         specAn.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
         specAn.Fcnt.Configuration.ConfigureMeasurementInterval("", measurementInterval)
         specAn.Fcnt.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount,
                                                      averagingType)
         specAn.Fcnt.Configuration.ConfigureRbwFilter("", rbw, rbwFilterType, rrcAlpha)

         ' Retrieve results 

         specAn.Fcnt.Results.Read("", timeout, averageRelativeFrequency, averageAbsoluteFrequency,
                                  meanPhase)

         Console.WriteLine("Average Relative Frequency (Hz) {0}", averageRelativeFrequency)
         Console.WriteLine("Average Absolute Frequency (Hz) {0}", averageAbsoluteFrequency)
         Console.WriteLine("Mean Phase (deg)                {0}", meanPhase)
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
