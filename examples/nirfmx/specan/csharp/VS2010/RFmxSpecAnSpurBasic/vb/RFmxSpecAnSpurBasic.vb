'Steps:
'1. Open a new RFmx session
'2. Configure Selected Ports
'3. Configure the basic signal properties  (Reference Level and External Attenuation)
'4. Select Spur measurement 
'5. Configure Spur Number of Ranges
'6. Configure Spur Start frequency, Stop frequency, RBW filter and Absolute Limit Start for all the ranges
'7. Initiate Measurement
'8. Read Spur Measurement Status
'9. Close the RFmx Session

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.SpecAnMX

Public Class RFmxSpecAnSpurBasic
   Private instrSession As RFmxInstrMX
   Private specAn As RFmxSpecAnMX

   Public Sub Run()
      Dim resourceName As String = "RFSA"
      Dim selectedPorts As String = ""
      Dim centerFrequency As Double = 0.0 ' Hz 
      Dim referenceLevel As Double = 0.0 ' dBm 
      Dim externalAttenuation As Double = 0.0 ' dB 

      'Range list
      Dim rangeListSize As Integer = 1
      Dim startFrequency() As Double = {1000000000.0} ' Hz 
      Dim stopFrequency() As Double = {1500000000.0} ' Hz 
      Dim rangeEnabled() As RFmxSpecAnMXSpurRangeEnabled = {RFmxSpecAnMXSpurRangeEnabled.[True]}

      'RBW Filter
      Dim rbwFilterAutoBandwidth() As RFmxSpecAnMXSpurRbwAutoBandwidth = {RFmxSpecAnMXSpurRbwAutoBandwidth.[False]}
      Dim rbwFilterBandwidth() As Double = {30000.0} ' Hz 
      Dim rbwFilterType() As RFmxSpecAnMXSpurRbwFilterType = {RFmxSpecAnMXSpurRbwFilterType.Gaussian}

      Dim limit() As Double = {-10.0} ' dBm 
      Dim timeout As Double = 10 ' seconds 

      Dim measurementStatus As RFmxSpecAnMXSpurMeasurementStatus

      Try
         ' Create a new RFmx Session 

         instrSession = New RFmxInstrMX(resourceName, "")

         ' Get SpecAn signal 

         specAn = instrSession.GetSpecAnSignalConfiguration()

         ' Configure measurement 
         specAn.SetSelectedPorts("", selectedPorts)
         specAn.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
         specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Spur, True)
         specAn.Spur.Configuration.ConfigureNumberOfRanges("", rangeListSize)
         specAn.Spur.Configuration.ConfigureRangeFrequencyArray("", startFrequency,
                                                               stopFrequency, rangeEnabled)
         specAn.Spur.Configuration.ConfigureRangeRbwArray("",
                                                               rbwFilterAutoBandwidth,
                                                               rbwFilterBandwidth,
                                                               rbwFilterType)
         specAn.Spur.Configuration.ConfigureRangeAbsoluteLimitArray("", Nothing, limit, Nothing)
         specAn.Initiate("", "")

         ' Retrieve results 

         specAn.Spur.Results.FetchMeasurementStatus("", timeout, measurementStatus)


         Console.WriteLine("Measurement Status: {0}" & vbLf, measurementStatus)

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
