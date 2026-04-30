'Steps:
'1. Open a new RFmx session
'2. Configure the basic instrument properties (Clock Source, Clock Frequency)
'3. Configure Selected Ports
'4. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
'5. Select SEM measurement and enable the traces
'6. Configure SEM Sweep Time
'7. Configure SEM Power Units and Reference Type
'8. Configure SEM Averaging
'9. Configure SEM FFT
'10. Configure SEM Number of Carrier Channel
'11. Configure SEM Carrier Offset, Integration BW, RBW Filter for all the Carriers using Selector String
'12. Configure SEM Number of Offsets
'13. Configure SEM Offset Frequency, RBW filter, Limit Fail Mask, Absolute Limit
'for all offsets using Selector String
'14. Initiate Measurement
'15. Fetch SEM Lower Offset Power, Lower Offset Margin for all the offsets
'16. Fetch SEM Upper Offset Power, Upper Offset Margin for all the offsets
'17. Fetch SEM Carrier Measurement for all the Carriers
'18. Fetch SEM Traces
'19. Close the RFmx session

Imports NationalInstruments
Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.SpecAnMX

Public Class RFmxSpecAnSemAdvanced
   Private instrSession As RFmxInstrMX
   Private specAn As RFmxSpecAnMX
   Private resourceName As [String], frequencySource As [String]
   Private selectedPorts As String
   Private centerFrequency As Double, referenceLevel As Double, externalAttenuation As Double, frequency As Double, timeout As Double
   Private averagingCount As Integer
   Private referenceType As RFmxSpecAnMXSemReferenceType
   Private powerUnits As RFmxSpecAnMXSemPowerUnits
   Private sweepTimeAuto As RFmxSpecAnMXSemSweepTimeAuto
   Private averagingEnabled As RFmxSpecAnMXSemAveragingEnabled
   Private averagingType As RFmxSpecAnMXSemAveragingType
   Private fftWindow As RFmxSpecAnMXSemFftWindow
   Private sweepTimeInterval As Double, fftPadding As Double

   Const NumberOfCarriers As Integer = 1
   Const NumberOfOffsets As Integer = 2

   Private Structure CarrierChannel
      Public carrierFrequency As Double, integrationBandwidth As Double, channelBandwidth As Double,
             rbw As Double, rrcFilterAlpha As Double
      Public rbwAuto As RFmxSpecAnMXSemCarrierRbwAutoBandwidth
      Public rbwFilterType As RFmxSpecAnMXSemCarrierRbwFilterType
      Public rrcFilterEnabled As RFmxSpecAnMXSemCarrierRrcFilterEnabled
   End Structure

   Structure OffsetMeasurementPower
      Public offsetSegmentAbsolutepower As Double()
      Public offsetSegmentTotalRelativePower As Double()
      Public offsetSegmentPeakAbsolutePower As Double()
      Public offsetSegmentPeakFrequency As Double()
      Public offsetSegmentPeakRelativeFrequency As Double()
      Public Sub New(numOfOffsets As Int32)
         offsetSegmentAbsolutepower = New Double(numOfOffsets - 1) {}
         offsetSegmentTotalRelativePower = New Double(numOfOffsets - 1) {}
         offsetSegmentPeakAbsolutePower = New Double(numOfOffsets - 1) {}
         offsetSegmentPeakFrequency = New Double(numOfOffsets - 1) {}
         offsetSegmentPeakRelativeFrequency = New Double(numOfOffsets - 1) {}
      End Sub
   End Structure

   Structure OffsetMeasurementMargin
      Public offsetSegmentMargin As Double()
      Public offsetSegmentMarginAbsolutePower As Double()
      Public offsetSegmentMarginRelativePower As Double()
      Public offsetSegmentMarginFrequency As Double()
      Public Sub New(numOfOffsets As Int32)
         offsetSegmentMargin = New Double(numOfOffsets - 1) {}
         offsetSegmentMarginAbsolutePower = New Double(numOfOffsets - 1) {}
         offsetSegmentMarginRelativePower = New Double(numOfOffsets - 1) {}
         offsetSegmentMarginFrequency = New Double(numOfOffsets - 1) {}
      End Sub

   End Structure

   Structure LowerOffsetMeasurement
      Public power As OffsetMeasurementPower
      Public margin As OffsetMeasurementMargin
      Public status As RFmxSpecAnMXSemLowerOffsetMeasurementStatus()
      Public Sub New(numOfOffSets As Int32)
         power = New OffsetMeasurementPower(numOfOffSets)
         margin = New OffsetMeasurementMargin(numOfOffSets)
         status = New RFmxSpecAnMXSemLowerOffsetMeasurementStatus(NumberOfOffsets - 1) {}
      End Sub

   End Structure

   Structure UpperOffsetMeasurement
      Public power As OffsetMeasurementPower
      Public margin As OffsetMeasurementMargin
      Public status As RFmxSpecAnMXSemUpperOffsetMeasurementStatus()
      Public Sub New(numOfOffSets As Int32)
         power = New OffsetMeasurementPower(numOfOffSets)
         margin = New OffsetMeasurementMargin(numOfOffSets)
         status = New RFmxSpecAnMXSemUpperOffsetMeasurementStatus(NumberOfOffsets - 1) {}
      End Sub
   End Structure

   Private Structure CarrierMeasurement
      Public absolutePower As Double
      Public peakAbsolutePower As Double
      Public peakFrequency As Double
      Public totalRelativePower As Double
   End Structure

   'Carrier channels and Offset segments inputs
   Private carrierCh As CarrierChannel() = New CarrierChannel(NumberOfCarriers - 1) {}

   'Carrier channels and Offset segment measurement results        
   Private carrierMsr As CarrierMeasurement() = New CarrierMeasurement(NumberOfCarriers - 1) {}
   Dim lowerOffset As New LowerOffsetMeasurement(NumberOfOffsets)
   Dim upperOffset As New UpperOffsetMeasurement(NumberOfOffsets)

   Private totalCarrierPower As Double
   Private compositeMeasurementStatus As RFmxSpecAnMXSemCompositeMeasurementStatus


   'offset segment inputs

   Public enabled As RFmxSpecAnMXSemOffsetEnabled() = New RFmxSpecAnMXSemOffsetEnabled(NumberOfOffsets - 1) {}
   Public frequencySideband As RFmxSpecAnMXSemOffsetSideband() = New RFmxSpecAnMXSemOffsetSideband(NumberOfOffsets - 1) {}
   Public rbwAuto As RFmxSpecAnMXSemOffsetRbwAutoBandwidth() = New RFmxSpecAnMXSemOffsetRbwAutoBandwidth(NumberOfOffsets - 1) {}
   Public rbwFilterType As RFmxSpecAnMXSemOffsetRbwFilterType() = New RFmxSpecAnMXSemOffsetRbwFilterType(NumberOfOffsets - 1) {}
   Public limitFailMask As RFmxSpecAnMXSemOffsetLimitFailMask() = New RFmxSpecAnMXSemOffsetLimitFailMask(NumberOfOffsets - 1) {}
   Public absoluteLimitMode As RFmxSpecAnMXSemOffsetAbsoluteLimitMode() = New RFmxSpecAnMXSemOffsetAbsoluteLimitMode(NumberOfOffsets - 1) {}
   Public relativeLimitMode As RFmxSpecAnMXSemOffsetRelativeLimitMode() = New RFmxSpecAnMXSemOffsetRelativeLimitMode(NumberOfOffsets - 1) {}
   Public startFrequency As Double() = New Double(NumberOfOffsets - 1) {}
   Public stopFrequecny As Double() = New Double(NumberOfOffsets - 1) {}
   Public rbw As Double() = New Double(NumberOfOffsets - 1) {}
   Public absoluteStartLimit As Double() = New Double(NumberOfOffsets - 1) {}
   Public absoluteStopLimit As Double() = New Double(NumberOfOffsets - 1) {}
   Public relativeStartLimit As Double() = New Double(NumberOfOffsets - 1) {}
   Public relativeStopLimit As Double() = New Double(NumberOfOffsets - 1) {}
   Public frequencyDefinition As RFmxSpecAnMXSemOffsetFrequencyDefinition() = New RFmxSpecAnMXSemOffsetFrequencyDefinition(NumberOfOffsets - 1) {}


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
      centerFrequency = 1000000000.0
      ' Hz 
      referenceLevel = 0.0
      ' dBm 
      externalAttenuation = 0.0
      ' dB 

      frequencySource = RFmxInstrMXConstants.OnboardClock
      frequency = 10000000.0
      ' Hz 

      timeout = 10.0
      ' seconds 

      referenceType = RFmxSpecAnMXSemReferenceType.Integration
      powerUnits = RFmxSpecAnMXSemPowerUnits.dBm

      'Sweep Time
      sweepTimeAuto = RFmxSpecAnMXSemSweepTimeAuto.[True]
      sweepTimeInterval = 0.001

      'Averaging
      averagingEnabled = RFmxSpecAnMXSemAveragingEnabled.[False]
      averagingCount = 10
      averagingType = RFmxSpecAnMXSemAveragingType.Rms

      'FFT Window
      fftWindow = RFmxSpecAnMXSemFftWindow.FlatTop
      fftPadding = -1.0

      'Set up the carrier channel inputs
      For i As Integer = 0 To NumberOfCarriers - 1
         carrierCh(i).carrierFrequency = 0.0
         carrierCh(i).integrationBandwidth = 2000000.0
         carrierCh(i).channelBandwidth = 2000000.0
         carrierCh(i).rbwAuto = RFmxSpecAnMXSemCarrierRbwAutoBandwidth.[True]
         carrierCh(i).rbwFilterType = RFmxSpecAnMXSemCarrierRbwFilterType.Gaussian
         carrierCh(i).rbw = 10000.0
         carrierCh(i).rrcFilterEnabled = RFmxSpecAnMXSemCarrierRrcFilterEnabled.[False]
         carrierCh(i).rrcFilterAlpha = 0.22
      Next

      'Set up the offset segment inputs
      For i As Integer = 0 To NumberOfOffsets - 1
         If i = 0 Then
            startFrequency(i) = 1000000.0
            stopFrequecny(i) = 2000000.0
            relativeLimitMode(i) = RFmxSpecAnMXSemOffsetRelativeLimitMode.Manual
            relativeStartLimit(i) = -10.0
            relativeStopLimit(i) = -30.0
         Else
            startFrequency(i) = 2000000.0
            stopFrequecny(i) = 3000000.0
            relativeLimitMode(i) = RFmxSpecAnMXSemOffsetRelativeLimitMode.Couple
            relativeStartLimit(i) = -30.0
            relativeStopLimit(i) = -30.0
         End If

         enabled(i) = RFmxSpecAnMXSemOffsetEnabled.[True]
         frequencySideband(i) = RFmxSpecAnMXSemOffsetSideband.Both
         rbwAuto(i) = RFmxSpecAnMXSemOffsetRbwAutoBandwidth.[True]
         rbwFilterType(i) = RFmxSpecAnMXSemOffsetRbwFilterType.Gaussian
         rbw(i) = 10000.0
         limitFailMask(i) = RFmxSpecAnMXSemOffsetLimitFailMask.Absolute
         absoluteLimitMode(i) = RFmxSpecAnMXSemOffsetAbsoluteLimitMode.Couple
         absoluteStartLimit(i) = -10.0
         absoluteStopLimit(i) = -10.0
         frequencyDefinition(i) = RFmxSpecAnMXSemOffsetFrequencyDefinition.CarrierCenterToMeasurementBandwidthCenter
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
      specAn.ConfigureFrequency("", centerFrequency)
      specAn.ConfigureReferenceLevel("", referenceLevel)
      specAn.ConfigureExternalAttenuation("", externalAttenuation)
      specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Sem, True)
      specAn.Sem.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval)
      specAn.Sem.Configuration.ConfigurePowerUnits("", powerUnits)
      specAn.Sem.Configuration.ConfigureReferenceType("", referenceType)
      specAn.Sem.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount,
                                                  averagingType)
      specAn.Sem.Configuration.ConfigureFft("", fftWindow, fftPadding)

      Dim carrierString As String, offsetString As String
      specAn.Sem.Configuration.ConfigureNumberOfCarriers("", NumberOfCarriers)
      For i As Integer = 0 To NumberOfCarriers - 1
         carrierString = RFmxSpecAnMX.BuildCarrierString2("", i)
         specAn.Sem.Configuration.ConfigureCarrierFrequency(carrierString,
                                                         carrierCh(i).carrierFrequency)
         specAn.Sem.Configuration.ConfigureCarrierIntegrationBandwidth(carrierString,
                                                                       carrierCh(i).integrationBandwidth)
         specAn.Sem.Configuration.ConfigureCarrierRbwFilter(carrierString,
                                                            carrierCh(i).rbwAuto,
                                                            carrierCh(i).rbw,
                                                            carrierCh(i).rbwFilterType)
         specAn.Sem.Configuration.ConfigureCarrierRrcFilter(carrierString,
                                                            carrierCh(i).rrcFilterEnabled,
                                                            carrierCh(i).rrcFilterAlpha)
         specAn.Sem.Configuration.ConfigureCarrierChannelBandwidth(carrierString,
                                                                   carrierCh(i).channelBandwidth)
      Next

      specAn.Sem.Configuration.ConfigureNumberOfOffsets("", NumberOfOffsets)
      For i As Integer = 0 To NumberOfOffsets - 1
         offsetString = RFmxSpecAnMX.BuildOffsetString2("", i)

         specAn.Sem.Configuration.ConfigureOffsetLimitFailMask(offsetString,
                                                               limitFailMask(i))

         specAn.Sem.Configuration.ConfigureOffsetFrequencyDefinition(offsetString,
                                                                     frequencyDefinition(i))
      Next

      specAn.Sem.Configuration.ConfigureOffsetFrequencyArray("", startFrequency, stopFrequecny, enabled, frequencySideband)
      specAn.Sem.Configuration.ConfigureOffsetRbwFilterArray("", rbwAuto, rbw, rbwFilterType)
      specAn.Sem.Configuration.ConfigureOffsetAbsoluteLimitArray("", absoluteLimitMode, absoluteStartLimit, absoluteStopLimit)
      specAn.Sem.Configuration.ConfigureOffsetRelativeLimitArray("", relativeLimitMode, relativeStartLimit, relativeStopLimit)

      specAn.Initiate("", "")
   End Sub

   Private Sub RetrieveResults()
      ' Retrieve results 


      Dim carrierString As String


      specAn.Sem.Results.FetchLowerOffsetPowerArray("", timeout, lowerOffset.power.offsetSegmentAbsolutepower,
         lowerOffset.power.offsetSegmentTotalRelativePower, lowerOffset.power.offsetSegmentPeakAbsolutePower,
         lowerOffset.power.offsetSegmentPeakFrequency, lowerOffset.power.offsetSegmentPeakRelativeFrequency)

      specAn.Sem.Results.FetchLowerOffsetMarginArray("", timeout, lowerOffset.status, lowerOffset.margin.offsetSegmentMargin,
         lowerOffset.margin.offsetSegmentMarginFrequency, lowerOffset.margin.offsetSegmentMarginAbsolutePower,
         lowerOffset.margin.offsetSegmentMarginRelativePower)

      specAn.Sem.Results.FetchUpperOffsetPowerArray("", timeout, upperOffset.power.offsetSegmentAbsolutepower,
         upperOffset.power.offsetSegmentTotalRelativePower, upperOffset.power.offsetSegmentPeakAbsolutePower,
         upperOffset.power.offsetSegmentPeakFrequency, upperOffset.power.offsetSegmentPeakRelativeFrequency)

      specAn.Sem.Results.FetchUpperOffsetMarginArray("", timeout, upperOffset.status, upperOffset.margin.offsetSegmentMargin,
         upperOffset.margin.offsetSegmentMarginFrequency, upperOffset.margin.offsetSegmentMarginAbsolutePower,
         upperOffset.margin.offsetSegmentMarginRelativePower)


      For i As Integer = 0 To NumberOfCarriers - 1
         carrierString = RFmxSpecAnMX.BuildCarrierString2("", i)
         specAn.Sem.Results.FetchCarrierMeasurement(carrierString, timeout,
                                                    carrierMsr(i).absolutePower,
                                                    carrierMsr(i).peakAbsolutePower,
                                                    carrierMsr(i).peakFrequency,
                                                    carrierMsr(i).totalRelativePower)
      Next

      Dim absoluteMaskTrace As Spectrum(Of Single) = Nothing
      specAn.Sem.Results.FetchAbsoluteMaskTrace("", timeout, absoluteMaskTrace)

      Dim relativeMaskTrace As Spectrum(Of Single) = Nothing
      specAn.Sem.Results.FetchRelativeMaskTrace("", timeout, relativeMaskTrace)

      Dim spectrum As Spectrum(Of Single) = Nothing
      specAn.Sem.Results.FetchSpectrum("", timeout, spectrum)

      specAn.Sem.Results.FetchTotalCarrierPower("", timeout, totalCarrierPower)

      specAn.Sem.Results.FetchCompositeMeasurementStatus("", timeout, compositeMeasurementStatus)
   End Sub

   Private Sub PrintResults()
      Dim status As String = "Fail"
      If compositeMeasurementStatus = RFmxSpecAnMXSemCompositeMeasurementStatus.Pass Then
         status = "Pass"
      End If

      Console.WriteLine("Composite measurement status:       : {0}", status)
      Console.WriteLine("Total Carrier Power (dBm or dBm/Hz) : {0}" & vbLf, totalCarrierPower)

      Console.WriteLine("--------------Carrier Measurements-----------------------------" & vbLf)
      For i As Integer = 0 To NumberOfCarriers - 1
         Console.WriteLine("*** Carrier {0} ***" & vbLf, i)
         Console.WriteLine("Absolute Power (dBm or dBm/Hz)       : {0}", carrierMsr(i).absolutePower)
         Console.WriteLine("Total Relative Power(dB)             : {0}", carrierMsr(i).totalRelativePower)
         Console.WriteLine("Peak Absolute Power (dBm or dbm/Hz)  : {0}", carrierMsr(i).peakAbsolutePower)
         Console.WriteLine("Peak Frquency                        : {0}", carrierMsr(i).peakFrequency)
         Console.WriteLine("-----------------------------------------------------------" & vbLf)
      Next

      Console.WriteLine("--------------Offset segment measurements ---------------------" & vbLf)
      For i As Integer = 0 To NumberOfOffsets - 1
         Console.WriteLine("*** Offset {0} ***" & vbLf, i)
         Console.WriteLine("Lower offset : Total Absolute Power (dBm or dBm/Hz)   : {0}",
                           lowerOffset.power.offsetSegmentAbsolutepower(i))
         Console.WriteLine("Lower offset : Total Relative Power (dB)              : {0}",
                           lowerOffset.power.offsetSegmentTotalRelativePower(i))
         Console.WriteLine("Lower Offset : Peak Absolute Power (dBm or dBm/Hz)    : {0}",
                           lowerOffset.power.offsetSegmentPeakAbsolutePower(i))
         Console.WriteLine("Lower offset : Peak Frequency (Hz)                    : {0}",
                           lowerOffset.power.offsetSegmentPeakFrequency(i))
         Console.WriteLine("Lower offset : Peak Relative Power (dB)               : {0}",
                           lowerOffset.power.offsetSegmentPeakRelativeFrequency(i))
         Console.WriteLine("Lower Offset : Margin (dB)                            : {0}",
                           lowerOffset.margin.offsetSegmentMargin(i))
         Console.WriteLine("Lower offset : Margin Absolute Power (dBm or dBm/Hz)  : {0}",
                           lowerOffset.margin.offsetSegmentMarginAbsolutePower(i))
         Console.WriteLine("Lower offset : Margin Relative Power (dB)             : {0}",
                           lowerOffset.margin.offsetSegmentMarginRelativePower(i))
         Console.WriteLine("Lower offset : Margin Frequency (Hz)                  : {0}",
                           lowerOffset.margin.offsetSegmentMarginFrequency(i))

         status = "Fail"
         If lowerOffset.status(i) = RFmxSpecAnMXSemLowerOffsetMeasurementStatus.Pass Then
            status = "Pass"
         End If
         Console.WriteLine("Lower offset : Measurement Status                     : {0}" & vbLf, status)

         Console.WriteLine(vbLf & "Upper offset : Total Absolute Power (dBm or dBm/Hz)   : {0}",
                           upperOffset.power.offsetSegmentAbsolutepower(i))
         Console.WriteLine("Upper offset : Total Relative Power (dB)              : {0}",
                           upperOffset.power.offsetSegmentTotalRelativePower(i))
         Console.WriteLine("Upper Offset : Peak Absolute Power (dBm or dBm/Hz)    : {0}",
                           upperOffset.power.offsetSegmentPeakAbsolutePower(i))
         Console.WriteLine("Upper offset : Peak Frequency (Hz)                    : {0}",
                           upperOffset.power.offsetSegmentPeakFrequency(i))
         Console.WriteLine("Upper offset : Peak Relative Power (dB)               : {0}",
                           upperOffset.power.offsetSegmentPeakRelativeFrequency(i))
         Console.WriteLine("Upper Offset : Margin (dB)                            : {0}",
                           upperOffset.margin.offsetSegmentMargin(i))
         Console.WriteLine("Upper offset : Margin Absolute Power (dBm or dBm/Hz)  : {0}",
                           upperOffset.margin.offsetSegmentMarginAbsolutePower(i))
         Console.WriteLine("Upper offset : Margin Relative Power (dB)             : {0}",
                           upperOffset.margin.offsetSegmentMarginRelativePower(i))
         Console.WriteLine("Upper offset : Margin Frequency (Hz)                  : {0}",
                           upperOffset.margin.offsetSegmentMarginFrequency(i))

         status = "Fail"
         If upperOffset.status(i) = RFmxSpecAnMXSemUpperOffsetMeasurementStatus.Pass Then
            status = [String].Copy("Pass")
         End If
         Console.WriteLine("Upper offset : Measurement Status                     : {0}", status)
         Console.WriteLine("-----------------------------------------------------------" & vbLf)
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

   Private Shared Sub DisplayError(message As String)
      Console.WriteLine("ERROR:" & vbLf & message)
   End Sub
End Class
