'Steps:
'1. Open a new RFmx session
'2. Configure the basic instrument properties (Clock Source, Clock Frequency)
'3. Configure Selected Ports
'4. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
'5. Select SEM measurement and enable the traces
'6. Configure SEM Power Units and Reference Type
'7. Configure SEM Averaging
'8. Configure SEM Integration BW
'9. Configure SEM RBW Filter
'10. Configure SEM RRC Filter
'11. Configure SEM Number of Offsets
'12. Configure SEM Offset Frequency
'Use Array API's to configure all offset parameters as an array
'13. Configure SEM Offset Absolute Limit
'14. Configure SEM Offset Relative Limit
'15. Configure Offset RBW Filter
'16. Configure Offset Limit Fail Mask
'17. Initiate Measurement
'18. Fetch SEM Measurements and Traces
'19. Close the RFmx Session

Imports NationalInstruments
Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.SpecAnMX

Public Class RFmxSpecAnSem
   Private instrSession As RFmxInstrMX
   Private specAn As RFmxSpecAnMX
   Private resourceName As [String], frequencySource As [String]
   Private selectedPorts As String
   Private centerFrequency As Double, referenceLevel As Double, externalAttenuation As Double,
           frequency As Double, integrationBandwidth As Double, rbw As Double,
           rrcFilterAlpha As Double
   Private rbwAuto As RFmxSpecAnMXSemCarrierRbwAutoBandwidth
   Private rbwFilterType As RFmxSpecAnMXSemCarrierRbwFilterType
   Private rrcFilterEnabled As RFmxSpecAnMXSemCarrierRrcFilterEnabled
   Private referenceType As RFmxSpecAnMXSemReferenceType
   Private powerUnits As RFmxSpecAnMXSemPowerUnits
   Private averagingCount As Integer
   Private averagingEnabled As RFmxSpecAnMXSemAveragingEnabled
   Private averagingType As RFmxSpecAnMXSemAveragingType
   Private limitFailMask As RFmxSpecAnMXSemOffsetLimitFailMask
   Private timeout As Double = 10.0

   Const NumberOfOffsets As Integer = 2

   Private offsetEnabled As RFmxSpecAnMXSemOffsetEnabled() =
           New RFmxSpecAnMXSemOffsetEnabled(NumberOfOffsets - 1) {}
   Private offsetFrequencySideband As RFmxSpecAnMXSemOffsetSideband() =
           New RFmxSpecAnMXSemOffsetSideband(NumberOfOffsets - 1) {}
   Private offsetRbwAuto As RFmxSpecAnMXSemOffsetRbwAutoBandwidth() =
           New RFmxSpecAnMXSemOffsetRbwAutoBandwidth(NumberOfOffsets - 1) {}
   Private offsetRbwFilterType As RFmxSpecAnMXSemOffsetRbwFilterType() =
           New RFmxSpecAnMXSemOffsetRbwFilterType(NumberOfOffsets - 1) {}
   Private offsetAbsoluteLimitMode As RFmxSpecAnMXSemOffsetAbsoluteLimitMode() =
           New RFmxSpecAnMXSemOffsetAbsoluteLimitMode(NumberOfOffsets - 1) {}
   Private offsetRelativeLimitMode As RFmxSpecAnMXSemOffsetRelativeLimitMode() =
           New RFmxSpecAnMXSemOffsetRelativeLimitMode(NumberOfOffsets - 1) {}
   Private offsetStartFrequency As Double() = New Double(NumberOfOffsets - 1) {}
   Private offsetStopFrequecny As Double() = New Double(NumberOfOffsets - 1) {}
   Private offsetRbw As Double() = New Double(NumberOfOffsets - 1) {}
   Private offsetAbsoluteLimitStart As Double() = New Double(NumberOfOffsets - 1) {}
   Private offsetAbsoluteLimitStop As Double() = New Double(NumberOfOffsets - 1) {}
   Private offsetRelativeLimitStart As Double() = New Double(NumberOfOffsets - 1) {}
   Private offsetRelativeLimitStop As Double() = New Double(NumberOfOffsets - 1) {}

   'Output values
   Private compositeMeasurementStatus As RFmxSpecAnMXSemCompositeMeasurementStatus
   Private absolutePower As Double
   Private peakAbsolutePower As Double
   Private peakFrequency As Double
   Private totalRelativePower As Double

   Private lowerOffsetTotalAbsolutePower As Double()
   Private lowerOffsetTotalRelativePower As Double()
   Private lowerOffsetPeakAbsolutePower As Double()
   Private lowerOffsetPeakFrequency As Double()
   Private lowerOffsetPeakRelativePower As Double()
   Private lowerOffsetMargin As Double()
   Private lowerOffsetMarginAbsolutePower As Double()
   Private lowerOffsetMarginRelativePower As Double()
   Private lowerOffsetMarginFrequency As Double()
   Private lowerOffsetMeasurementStatus As RFmxSpecAnMXSemLowerOffsetMeasurementStatus()

   Private upperOffsetTotalAbsolutePower As Double()
   Private upperOffsetTotalRelativePower As Double()
   Private upperOffsetPeakAbsolutePower As Double()
   Private upperOffsetPeakFrequency As Double()
   Private upperOffsetPeakRelativePower As Double()
   Private upperOffsetMargin As Double()
   Private upperOffsetMarginAbsolutePower As Double()
   Private upperOffsetMarginRelativePower As Double()
   Private upperOffsetMarginFrequency As Double()
   Private upperOffsetMeasurementStatus As RFmxSpecAnMXSemUpperOffsetMeasurementStatus()

   Public Sub Run()
      Try
         InitializeVariables()
         InitializeInstr()
         ConfigureSpecAn()
         RetrieveResults()
         PrintResults()
      Catch ex As Exception
         DisplayError(ex.Message)
      Finally
         ' Close session 

         CloseSession()
         Console.WriteLine("Press any key to exit.....")
         Console.ReadKey()
      End Try
   End Sub

   Private Sub InitializeVariables()
      resourceName = "RFSA"

      selectedPorts = ""
      centerFrequency = 1000000000.0 ' Hz 
      referenceLevel = 0.0 ' dBm 
      externalAttenuation = 0.0 ' dB 

      frequencySource = RFmxInstrMXConstants.OnboardClock
      frequency = 10000000.0 ' Hz 

      integrationBandwidth = 2000000.0 ' Hz 
      rbwAuto = RFmxSpecAnMXSemCarrierRbwAutoBandwidth.[False]
      rbwFilterType = RFmxSpecAnMXSemCarrierRbwFilterType.Gaussian
      rbw = 10000.0

      rrcFilterEnabled = RFmxSpecAnMXSemCarrierRrcFilterEnabled.[False]
      rrcFilterAlpha = 0.22

      referenceType = RFmxSpecAnMXSemReferenceType.Integration
      powerUnits = RFmxSpecAnMXSemPowerUnits.dBm

      averagingEnabled = RFmxSpecAnMXSemAveragingEnabled.[False]
      averagingCount = 10
      averagingType = RFmxSpecAnMXSemAveragingType.Rms

      limitFailMask = RFmxSpecAnMXSemOffsetLimitFailMask.Absolute

      For i As Integer = 0 To NumberOfOffsets - 1
         If i = 0 Then
            offsetStartFrequency(i) = 1000000.0 ' Hz 
            offsetStopFrequecny(i) = 2000000.0 ' Hz 
            offsetRelativeLimitMode(i) = RFmxSpecAnMXSemOffsetRelativeLimitMode.Manual
            offsetRelativeLimitStart(i) = -10.0
            offsetRelativeLimitStop(i) = -30.0

         ElseIf i = 1 Then
            offsetStartFrequency(i) = 2000000.0 ' Hz 
            offsetStopFrequecny(i) = 3000000.0 ' Hz 
            offsetRelativeLimitMode(i) = RFmxSpecAnMXSemOffsetRelativeLimitMode.Couple
            offsetRelativeLimitStart(i) = -30.0
            offsetRelativeLimitStop(i) = -30.0
         End If

         offsetRbwAuto(i) = RFmxSpecAnMXSemOffsetRbwAutoBandwidth.[True]
         offsetRbwFilterType(i) = RFmxSpecAnMXSemOffsetRbwFilterType.Gaussian
         offsetRbw(i) = 10000.0 ' Hz 
         offsetAbsoluteLimitMode(i) = RFmxSpecAnMXSemOffsetAbsoluteLimitMode.Couple
         offsetAbsoluteLimitStart(i) = -10.0
         offsetAbsoluteLimitStop(i) = -10.0
         offsetEnabled(i) = RFmxSpecAnMXSemOffsetEnabled.[True]
         offsetFrequencySideband(i) = RFmxSpecAnMXSemOffsetSideband.Both
      Next
   End Sub

   Private Sub InitializeInstr()
      ' Create a new RFmx Session 

      instrSession = New RFmxInstrMX(resourceName, "")
   End Sub

   Private Sub ConfigureSpecAn()
      ' Get SpecAn signal 

      specAn = instrSession.GetSpecAnSignalConfiguration()

      ' Configure measurement 

      instrSession.ConfigureFrequencyReference("", frequencySource, frequency)
      specAn.SetSelectedPorts("", selectedPorts)
      specAn.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)

      specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Sem, True)
      specAn.Sem.Configuration.ConfigurePowerUnits("", powerUnits)
      specAn.Sem.Configuration.ConfigureReferenceType("", referenceType)
      specAn.Sem.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount,
                                                  averagingType)
      specAn.Sem.Configuration.ConfigureCarrierIntegrationBandwidth("", integrationBandwidth)
      specAn.Sem.Configuration.ConfigureCarrierRbwFilter("", rbwAuto, rbw, rbwFilterType)
      specAn.Sem.Configuration.ConfigureCarrierRrcFilter("", rrcFilterEnabled, rrcFilterAlpha)
      specAn.Sem.Configuration.ConfigureNumberOfOffsets("", NumberOfOffsets)
      specAn.Sem.Configuration.ConfigureOffsetFrequencyArray("", offsetStartFrequency,
                                                             offsetStopFrequecny, offsetEnabled,
                                                             offsetFrequencySideband)
      specAn.Sem.Configuration.ConfigureOffsetAbsoluteLimitArray("", offsetAbsoluteLimitMode,
                                                                 offsetAbsoluteLimitStart,
                                                                 offsetAbsoluteLimitStop)
      specAn.Sem.Configuration.ConfigureOffsetRelativeLimitArray("", offsetRelativeLimitMode,
                                                                 offsetRelativeLimitStart,
                                                                 offsetRelativeLimitStop)
      specAn.Sem.Configuration.ConfigureOffsetRbwFilterArray("", offsetRbwAuto, offsetRbw,
                                                             offsetRbwFilterType)
      specAn.Sem.Configuration.ConfigureOffsetLimitFailMask("offset::all", limitFailMask)

      specAn.Initiate("", "")
   End Sub

   Private Sub RetrieveResults()
      ' Retrieve results 

      Dim absoluteMaskTrace As Spectrum(Of Single) = Nothing
      Dim relativeMaskTrace As Spectrum(Of Single) = Nothing
      Dim spectrum As Spectrum(Of Single) = Nothing

      specAn.Sem.Results.FetchLowerOffsetPowerArray("", timeout, lowerOffsetTotalAbsolutePower,
                                                    lowerOffsetTotalRelativePower,
                                                    lowerOffsetPeakAbsolutePower,
                                                    lowerOffsetPeakFrequency,
                                                    lowerOffsetPeakRelativePower)

      specAn.Sem.Results.FetchLowerOffsetMarginArray("", timeout, lowerOffsetMeasurementStatus,
                                                     lowerOffsetMargin, lowerOffsetMarginFrequency,
                                                     lowerOffsetMarginAbsolutePower,
                                                     lowerOffsetMarginRelativePower)

      specAn.Sem.Results.FetchUpperOffsetPowerArray("", timeout, upperOffsetTotalAbsolutePower,
                                                    upperOffsetTotalRelativePower,
                                                    upperOffsetPeakAbsolutePower,
                                                    upperOffsetPeakFrequency,
                                                    upperOffsetPeakRelativePower)

      specAn.Sem.Results.FetchUpperOffsetMarginArray("", timeout, upperOffsetMeasurementStatus,
                                                     upperOffsetMargin, upperOffsetMarginFrequency,
                                                     upperOffsetMarginAbsolutePower,
                                                     upperOffsetMarginRelativePower)

      specAn.Sem.Results.FetchCarrierMeasurement("", timeout, absolutePower, peakAbsolutePower,
                                                 peakFrequency, totalRelativePower)

      specAn.Sem.Results.FetchAbsoluteMaskTrace("", timeout, absoluteMaskTrace)

      specAn.Sem.Results.FetchRelativeMaskTrace("", timeout, relativeMaskTrace)

      specAn.Sem.Results.FetchSpectrum("", timeout, spectrum)

      specAn.Sem.Results.FetchCompositeMeasurementStatus("", timeout, compositeMeasurementStatus)
   End Sub

   Private Sub PrintResults()
      Dim status As [String] = "Fail"
      If compositeMeasurementStatus = RFmxSpecAnMXSemCompositeMeasurementStatus.Pass Then
         status = "Pass"
      End If

      Console.WriteLine("Composite measurement status  : {0}" & vbLf, status)

      Console.WriteLine(vbLf & "--------------Carrier Measurements----------------------------" & vbLf)
      Console.WriteLine("Absolute Power (dBm or dBm/Hz):      {0}", absolutePower)
      Console.WriteLine("Peak Absolute Power (dBm or dbm/Hz): {0}", peakAbsolutePower)
      Console.WriteLine("Peak Frquency:                       {0}", peakFrequency)

      Console.WriteLine(vbLf & "--------------Offset segment measurements ---------------------------" & vbLf)
      For i As Integer = 0 To NumberOfOffsets - 1
         Console.WriteLine("Offset {0}" & vbLf, i)

         Console.WriteLine("Lower offset : Total Absolute Power (dBm or dBm/Hz):  {0}",
                           lowerOffsetTotalAbsolutePower(i))
         Console.WriteLine("Lower offset : Total Relative Power (dB):             {0}",
                           lowerOffsetTotalRelativePower(i))
         Console.WriteLine("Lower Offset : Peak Absolute Power (dBm or dBm/Hz):   {0}",
                           lowerOffsetPeakAbsolutePower(i))
         Console.WriteLine("Lower offset : Peak Frequency (Hz):                   {0}",
                           lowerOffsetPeakFrequency(i))
         Console.WriteLine("Lower offset : Peak Relative Power (dB):              {0}",
                           lowerOffsetPeakRelativePower(i))
         Console.WriteLine("Lower Offset : Margin (dB):                           {0}",
                           lowerOffsetMargin(i))
         Console.WriteLine("Lower offset : Margin Absolute Power (dBm or dBm/Hz): {0}",
                           lowerOffsetMarginAbsolutePower(i))
         Console.WriteLine("Lower offset : Margin Relative Power (dB):            {0}",
                           lowerOffsetMarginRelativePower(i))
         Console.WriteLine("Lower offset : Margin Frequency (Hz):                 {0}",
                           lowerOffsetMarginFrequency(i))

         status = "Fail"
         If lowerOffsetMeasurementStatus(i).Equals(RFmxSpecAnMXSemLowerOffsetMeasurementStatus.Pass) Then
            status = "Pass"
         End If
         Console.WriteLine("Lower offset : Measurement Status : {0}" & vbLf, status)

         Console.WriteLine(vbLf & "Upper offset : Total Absolute Power (dBm or dBm/Hz): {0}",
                           upperOffsetTotalAbsolutePower(i))
         Console.WriteLine("Upper offset : Total Relative Power (dB):              {0}",
                           upperOffsetTotalRelativePower(i))
         Console.WriteLine("Upper Offset : Peak Absolute Power (dBm or dBm/Hz):    {0}",
                           upperOffsetPeakAbsolutePower(i))
         Console.WriteLine("Upper offset : Peak Frequency (Hz):                    {0}",
                           upperOffsetPeakFrequency(i))
         Console.WriteLine("Upper offset : Peak Relative Power (dB):               {0}",
                           upperOffsetPeakRelativePower(i))
         Console.WriteLine("Upper Offset : Margin (dB):                            {0}",
                           upperOffsetMargin(i))
         Console.WriteLine("Upper offset : Margin Absolute Power (dBm or dBm/Hz):  {0}",
                           upperOffsetMarginAbsolutePower(i))
         Console.WriteLine("Upper offset : Margin Relative Power (dB):             {0}",
                           upperOffsetMarginRelativePower(i))
         Console.WriteLine("Upper offset : Margin Frequency (Hz):                  {0}",
                           upperOffsetMarginFrequency(i))

         status = "Fail"
         If upperOffsetMeasurementStatus(i).Equals(RFmxSpecAnMXSemUpperOffsetMeasurementStatus.Pass) Then
            status = "Pass"
         End If
         Console.WriteLine("Upper offset : Measurement Status : {0}" & vbLf, status)
         Console.WriteLine("-----------------------------------------------------------------------" & vbLf)
      Next
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
