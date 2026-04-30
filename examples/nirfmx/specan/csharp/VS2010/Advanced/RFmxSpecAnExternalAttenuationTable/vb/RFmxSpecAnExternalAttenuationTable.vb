'Steps:
'1. Open a new RFmx session
'2. Configure the basic instrument properties (Clock Source, Clock Frequency)
'3. Configure Selected Ports
'4. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
'5. Configure External Attenuation Table
'6. Select SEM measurement and enable the traces
'7. Configure SEM Power Units and Reference Type
'8. Configure Averaging parameters
'9. Configure Integration BW of the carrier
'10. Configure RBW Filter parameters
'11. Configure RRC Filter to be applied on the acquired carrier channel
'12. Configure Number of Offsets
'13. Configure Frequency Ranges of Offset channels
'Use Array API's to configure all offset parameters as an array
'14. Configure Absolute Limit mask for Offset Channels
'15. Configure Relative Limit mask for Offset Channels
'16. Configure RBW Filter parameters for the Offset channels
'17. Configure Offset Limit Fail Mask
'18. Configure Amplitude Correction Type

Imports NationalInstruments
Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.SpecAnMX

Public Class RFmxSpecAnExternalAttenuationTable
   Private instrSession As RFmxInstrMX
   Private specAn As RFmxSpecAnMX

   Private selectedPorts As String
   Private resourceName As [String]
   Private offsetString As String
   Private portString As String
   Private centerFrequency As Double, referenceLevel As Double, externalAttenuation As Double
   Private frequencyReferenceSource As [String]
   Private frequencyReferenceFrequency As Double

   Const TableSize As Integer = 3
   Const NumberOfOffsets As Integer = 4

   Private frequency As Double() = New Double(TableSize - 1) {997000000.0, 1000000000.0, 1003000000.0}
   Private attenuation As Double() = New Double(TableSize - 1) {0.2, 0.5, 0.3}

   Private integrationBandwidth As Double

   Private rbwAuto As RFmxSpecAnMXSemCarrierRbwAutoBandwidth
   Private rbwFilterType As RFmxSpecAnMXSemCarrierRbwFilterType
   Private rbw As Double

   Private rrcFilterEnabled As RFmxSpecAnMXSemCarrierRrcFilterEnabled
   Private rrcAlpha As Double

   Private amplitudeCorrectionType As RFmxSpecAnMXSemAmplitudeCorrectionType

   Private averagingEnabled As RFmxSpecAnMXSemAveragingEnabled
   Private averagingCount As Integer
   Private averagingType As RFmxSpecAnMXSemAveragingType

   Private referenceType As RFmxSpecAnMXSemReferenceType
   Private powerUnits As RFmxSpecAnMXSemPowerUnits

   Private offsetEnabled As RFmxSpecAnMXSemOffsetEnabled() = New RFmxSpecAnMXSemOffsetEnabled(NumberOfOffsets - 1) {RFmxSpecAnMXSemOffsetEnabled.[True], RFmxSpecAnMXSemOffsetEnabled.[True], RFmxSpecAnMXSemOffsetEnabled.[True], RFmxSpecAnMXSemOffsetEnabled.[True]}

   Private offsetSideband As RFmxSpecAnMXSemOffsetSideband() = New RFmxSpecAnMXSemOffsetSideband(NumberOfOffsets - 1) {RFmxSpecAnMXSemOffsetSideband.Both, RFmxSpecAnMXSemOffsetSideband.Both, RFmxSpecAnMXSemOffsetSideband.Both, RFmxSpecAnMXSemOffsetSideband.Both}

   Private offsetStartFrequency As Double() = New Double(NumberOfOffsets - 1) {2515000.0, 4000000.0, 7500000.0, 8500000.0}
   ' Hz
   Private offsetStopFrequency As Double() = New Double(NumberOfOffsets - 1) {3485000.0, 7500000.0, 8500000.0, 12000000.0}
   ' Hz

   Private offsetRbwFilterType As RFmxSpecAnMXSemOffsetRbwFilterType() = New RFmxSpecAnMXSemOffsetRbwFilterType(NumberOfOffsets - 1) {RFmxSpecAnMXSemOffsetRbwFilterType.Gaussian, RFmxSpecAnMXSemOffsetRbwFilterType.Gaussian, RFmxSpecAnMXSemOffsetRbwFilterType.Gaussian, RFmxSpecAnMXSemOffsetRbwFilterType.Gaussian}
   Private offsetRbwAuto As RFmxSpecAnMXSemOffsetRbwAutoBandwidth() = New RFmxSpecAnMXSemOffsetRbwAutoBandwidth(NumberOfOffsets - 1) {RFmxSpecAnMXSemOffsetRbwAutoBandwidth.[False], RFmxSpecAnMXSemOffsetRbwAutoBandwidth.[False], RFmxSpecAnMXSemOffsetRbwAutoBandwidth.[False], RFmxSpecAnMXSemOffsetRbwAutoBandwidth.[False]}
   Private offsetRbw As Double() = New Double(NumberOfOffsets - 1) {30000.0, 500000.0, 1000000.0, 1000000.0}
   ' Hz

   Private absoluteLimitMode As RFmxSpecAnMXSemOffsetAbsoluteLimitMode() = New RFmxSpecAnMXSemOffsetAbsoluteLimitMode(NumberOfOffsets - 1) {RFmxSpecAnMXSemOffsetAbsoluteLimitMode.Couple, RFmxSpecAnMXSemOffsetAbsoluteLimitMode.Couple, RFmxSpecAnMXSemOffsetAbsoluteLimitMode.Couple, RFmxSpecAnMXSemOffsetAbsoluteLimitMode.Couple}
   Private absoluteLimitStart As Double() = New Double(NumberOfOffsets - 1) {-69.6, -54.3, -54.3, -54.3}
   ' dBm
   Private absoluteLimitStop As Double() = New Double(NumberOfOffsets - 1) {-69.6, -54.3, -54.3, -54.3}
   ' dBm

   Private relativeLimitMode As RFmxSpecAnMXSemOffsetRelativeLimitMode() = New RFmxSpecAnMXSemOffsetRelativeLimitMode(NumberOfOffsets - 1) {RFmxSpecAnMXSemOffsetRelativeLimitMode.Manual, RFmxSpecAnMXSemOffsetRelativeLimitMode.Manual, RFmxSpecAnMXSemOffsetRelativeLimitMode.Manual, RFmxSpecAnMXSemOffsetRelativeLimitMode.Couple}
   Private relativeLimitStart As Double() = New Double(NumberOfOffsets - 1) {-33.73, -34.0, -37.5, -47.5}
   ' dBm
   Private relativeLimitStop As Double() = New Double(NumberOfOffsets - 1) {-48.27, -37.5, -47.5, -47.5}
   ' dBm

   Private offsetLimitFailMask As RFmxSpecAnMXSemOffsetLimitFailMask
   Private timeout As Double

   ' Output Variables

   Private compositeMeasurementStatus As RFmxSpecAnMXSemCompositeMeasurementStatus
   Private carrierAbsolutePower As Double, peakAbsolutePower As Double, peakFrequency As Double, totalRelativePower As Double

   Public lowerOffsetMargin As Double()
   Public lowerOffsetMarginAbsolutePower As Double()
   Public lowerOffsetMarginRelativePower As Double()
   Public lowerOffsetMarginFrequency As Double()
   Public lowerOffsetMeasurementStatus As RFmxSpecAnMXSemLowerOffsetMeasurementStatus()

   Public upperOffsetMargin As Double()
   Public upperOffsetMarginAbsolutePower As Double()
   Public upperOffsetMarginRelativePower As Double()
   Public upperOffsetMarginFrequency As Double()
   Public upperOffsetMeasurementStatus As RFmxSpecAnMXSemUpperOffsetMeasurementStatus()

   Private spectrum As Spectrum(Of Single)
   Private absoluteMask As Spectrum(Of Single)
   Private relativeMask As Spectrum(Of Single)

   Public Sub Run()
      Try
         InitializeVariables()
         InitializeInstr()
         ConfigureSpecAn()
         RetrieveResults()
         PrintResults()
      Catch ex As Exception
         DisplayError(ex)
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

      centerFrequency = 1000000000.0
      ' Hz
      referenceLevel = 0.0
      ' dBm
      externalAttenuation = 0.0
      ' dB

      frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
      frequencyReferenceFrequency = 10000000.0
      ' Hz

      integrationBandwidth = 3840000.0

      rbwAuto = RFmxSpecAnMXSemCarrierRbwAutoBandwidth.[False]
      rbwFilterType = RFmxSpecAnMXSemCarrierRbwFilterType.Gaussian
      rbw = 30000.0
      ' Hz

      rrcFilterEnabled = RFmxSpecAnMXSemCarrierRrcFilterEnabled.[True]
      rrcAlpha = 0.22

      amplitudeCorrectionType = RFmxSpecAnMXSemAmplitudeCorrectionType.RFCenterFrequency

      averagingEnabled = RFmxSpecAnMXSemAveragingEnabled.[False]
      averagingCount = 10
      averagingType = RFmxSpecAnMXSemAveragingType.Rms

      referenceType = RFmxSpecAnMXSemReferenceType.Integration
      powerUnits = RFmxSpecAnMXSemPowerUnits.dBm

      offsetLimitFailMask = RFmxSpecAnMXSemOffsetLimitFailMask.AbsoluteAndRelative

      absoluteMask = Nothing
      relativeMask = Nothing
      spectrum = Nothing

      timeout = 10.0
      ' seconds
   End Sub

   Private Sub InitializeInstr()
      ' Create a new RFmx Session

      instrSession = New RFmxInstrMX(resourceName, "")
   End Sub

   Private Sub ConfigureSpecAn()
      ' Get SpecAn signal

      specAn = instrSession.GetSpecAnSignalConfiguration()

      ' Configure measurement

      instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
      specAn.SetSelectedPorts("", selectedPorts)
      specAn.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
      portString = RFmxInstrMX.BuildPortString2("", selectedPorts, "", 0)
      instrSession.ConfigureExternalAttenuationTable(portString, "", frequency, attenuation)
      specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Sem, True)
      specAn.Sem.Configuration.ConfigurePowerUnits("", powerUnits)
      specAn.Sem.Configuration.ConfigureReferenceType("", referenceType)
      specAn.Sem.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)
      specAn.Sem.Configuration.ConfigureCarrierIntegrationBandwidth("", integrationBandwidth)
      specAn.Sem.Configuration.ConfigureCarrierRbwFilter("", rbwAuto, rbw, rbwFilterType)
      specAn.Sem.Configuration.ConfigureCarrierRrcFilter("", rrcFilterEnabled, rrcAlpha)
      specAn.Sem.Configuration.ConfigureNumberOfOffsets("", NumberOfOffsets)
      specAn.Sem.Configuration.ConfigureOffsetFrequencyArray("", offsetStartFrequency, offsetStopFrequency, offsetEnabled, offsetSideband)
      specAn.Sem.Configuration.ConfigureOffsetAbsoluteLimitArray("", absoluteLimitMode, absoluteLimitStart, absoluteLimitStop)
      specAn.Sem.Configuration.ConfigureOffsetRelativeLimitArray("", relativeLimitMode, relativeLimitStart, relativeLimitStop)
      specAn.Sem.Configuration.ConfigureOffsetRbwFilterArray("", offsetRbwAuto, offsetRbw, offsetRbwFilterType)
      offsetString = RFmxSpecAnMX.BuildOffsetString2("", -1)
      specAn.Sem.Configuration.ConfigureOffsetLimitFailMask(offsetString, offsetLimitFailMask)
      specAn.Sem.Configuration.SetAmplitudeCorrectionType("", amplitudeCorrectionType)
      specAn.Initiate("", "")
   End Sub

   Private Sub RetrieveResults()
      ' Retrieve results

      specAn.Sem.Results.FetchLowerOffsetMarginArray("", timeout, lowerOffsetMeasurementStatus, lowerOffsetMargin,
                                                       lowerOffsetMarginFrequency, lowerOffsetMarginAbsolutePower,
                                                       lowerOffsetMarginRelativePower)
      specAn.Sem.Results.FetchUpperOffsetMarginArray("", timeout, upperOffsetMeasurementStatus, upperOffsetMargin,
                                                       upperOffsetMarginFrequency, upperOffsetMarginAbsolutePower,
                                                       upperOffsetMarginRelativePower)
      specAn.Sem.Results.FetchCarrierMeasurement("", timeout, carrierAbsolutePower, peakAbsolutePower, peakFrequency, totalRelativePower)
      specAn.Sem.Results.FetchAbsoluteMaskTrace("", timeout, absoluteMask)
      specAn.Sem.Results.FetchRelativeMaskTrace("", timeout, relativeMask)
      specAn.Sem.Results.FetchSpectrum("", timeout, spectrum)
      specAn.Sem.Results.FetchCompositeMeasurementStatus("", timeout, compositeMeasurementStatus)
   End Sub

   Private Sub PrintResults()
      Console.WriteLine("Measurement status                     :  {0}", compositeMeasurementStatus)
      Console.WriteLine("Carrier Absolute Power (dBm or dBm/Hz) :  {0}" & vbLf, carrierAbsolutePower)

      Console.WriteLine("---------------Lower Offset---------------" & vbLf)
      Console.WriteLine("Lower Offset Segment Measurements" & vbLf)
      For i As Integer = 0 To NumberOfOffsets - 1
         Console.WriteLine("Offset : {0}" & vbLf, i)
         Console.WriteLine("Margin (dB)                              :  {0}", lowerOffsetMargin(i))
         Console.WriteLine("Margin Absolute Power (dBm)              :  {0}", lowerOffsetMarginAbsolutePower(i))
         Console.WriteLine("Margin Relative Power (dB)               :  {0}", lowerOffsetMarginRelativePower(i))
         Console.WriteLine("Margin Frequency (Hz)                    :  {0}", lowerOffsetMarginFrequency(i))
         Console.WriteLine("Measurement Status                       :  {0}", lowerOffsetMeasurementStatus(i))
      Next

      Console.WriteLine("---------------Upper Offset---------------" & vbLf)
      Console.WriteLine("Upper Offset Segment Measurements" & vbLf)
      For i As Integer = 0 To NumberOfOffsets - 1
         Console.WriteLine("Offset : {0}" & vbLf, i)
         Console.WriteLine("Margin (dB)                              :  {0}", upperOffsetMargin(i))
         Console.WriteLine("Margin Absolute Power (dBm)              :  {0}", upperOffsetMarginAbsolutePower(i))
         Console.WriteLine("Margin Relative Power (dB)               :  {0}", upperOffsetMarginRelativePower(i))
         Console.WriteLine("Margin Frequency (Hz)                    :  {0}", upperOffsetMarginFrequency(i))
         Console.WriteLine("Measurement Status                       :  {0}", upperOffsetMeasurementStatus(i))
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
         DisplayError(ex)
      End Try
   End Sub

   Private Shared Sub DisplayError(ex As Exception)
      Console.WriteLine("ERROR:" & vbLf & Convert.ToString(ex.[GetType]()) & ": " & ex.Message)
   End Sub
End Class
