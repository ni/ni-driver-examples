'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure Selected Ports.
'4. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
'5. Configure Trigger Parameters for IQ Power Edge Trigger.
'6. Configure Link Direction, Frequency Range, Carrier Bandwidth and BWP Subcarrier Spacing.
'7. Select SEM measurement and enable Traces.
'8. Configure Uplink Mask Type, or Downlink Mask, Band, gNodeB Category, Delta F_Max(Hz) and
'  Component Carrier Rated Output Power based on Link Direction.
'9. Configure Sweep Time Parameters.
'10. Configure Averaging Parameters for SEM measurement.
'11. Initiate the Measurement.
'12. Fetch SEM Measurements and Traces.
'13. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.NRMX

Namespace NationalInstruments.Examples.RFmxNRSemSingleCarrier
   Public Class RFmxNRSemSingleCarrier
      Private instrSession As RFmxInstrMX
      Private NR As RFmxNRMX
      Private resourceName As String

      Private selectedPorts As String
      Private centerFrequency As Double
      Private referenceLevel As Double
      Private externalAttenuation As Double

      Private frequencyReferenceSource As String
      Private frequencyReferenceFrequency As Double

      Private iqPowerEdgeEnabled As Boolean
      Private iqPowerEdgeLevel As Double
      Private triggerDelay As Double
      Private minimumQuietTimeMode As RFmxNRMXTriggerMinimumQuietTimeMode
      Private minimumQuietTime As Double

      Private linkDirection As RFmxNRMXLinkDirection

      Private frequencyRange As RFmxNRMXFrequencyRange

      Private uplinkMaskType As RFmxNRMXSemUplinkMaskType

      Private gNodeBCategory As RFmxNRMXgNodeBCategory
      Private downlinkMaskType As RFmxNRMXSemDownlinkMaskType
      Private deltaFMaximum As Double
      Private componentCarrierRatedOutputPower As Double
      Private band As Integer

      Private carrierBandwidth As Double
      Private subcarrierSpacing As Double

      Private sweepTimeAuto As RFmxNRMXSemSweepTimeAuto
      Private sweepTimeInterval As Double

      Private averagingEnabled As RFmxNRMXSemAveragingEnabled
      Private averagingCount As Integer
      Private averagingType As RFmxNRMXSemAveragingType

      Private timeout As Double

      Private measurementStatus As RFmxNRMXSemMeasurementStatus

      Private absolutePower As Double
      ' (dBm)
      Private peakAbsolutePower As Double
      ' (dBm)
      Private peakFrequency As Double
      ' (Hz)
      Private relativePower As Double
      ' (dB)

      Private upperOffsetMeasurementStatus As RFmxNRMXSemUpperOffsetMeasurementStatus()
      Private upperOffsetMargin As Double()
      ' (dB)
      Private upperOffsetMarginFrequency As Double()
      ' (Hz)
      Private upperOffsetMarginAbsolutePower As Double()
      ' (dBm)
      Private upperOffsetMarginRelativePower As Double()
      ' (dB)

      Private lowerOffsetMeasurementStatus As RFmxNRMXSemLowerOffsetMeasurementStatus()
      Private lowerOffsetMargin As Double()
      ' (dB)
      Private lowerOffsetMarginFrequency As Double()
      ' (Hz)
      Private lowerOffsetMarginAbsolutePower As Double()
      ' (dBm)
      Private lowerOffsetMarginRelativePower As Double()
      ' (dB)

      Private spectrum As Spectrum(Of Single)
      Private compositeMask As Spectrum(Of Single)

      Public Sub Run()
         Try
            InitializeVariables()
            InitializeInstr()
            ConfigureNR()
            RetrieveResults()
            PrintResults()
         Catch ex As Exception
            DisplayError(ex)
         Finally
            ' Close session
            CloseSession()
            Console.WriteLine("Press any key to exit")
            Console.ReadKey()
         End Try
      End Sub

      Private Sub InitializeVariables()
         resourceName = "RFSA"

         selectedPorts = ""
         centerFrequency = 3500000000.0
         ' (Hz)
         referenceLevel = 0.0
         ' (dBm)
         externalAttenuation = 0.0
         ' (dB)

         frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
         frequencyReferenceFrequency = 10000000.0
         ' (Hz)

         iqPowerEdgeEnabled = False
         iqPowerEdgeLevel = -20.0
         ' (dB or dBm)
         triggerDelay = 0.0
         ' (s)
         minimumQuietTimeMode = RFmxNRMXTriggerMinimumQuietTimeMode.Auto
         minimumQuietTime = 0.000008
         ' (s)

         linkDirection = RFmxNRMXLinkDirection.Uplink

         frequencyRange = RFmxNRMXFrequencyRange.Range1

         uplinkMaskType = RFmxNRMXSemUplinkMaskType.General

         gNodeBCategory = RFmxNRMXgNodeBCategory.WideAreaBaseStationCategoryA
         downlinkMaskType = RFmxNRMXSemDownlinkMaskType.Standard
         deltaFMaximum = 15000000.0
         ' (Hz)
         componentCarrierRatedOutputPower = 0.0
         ' (dBm)
         band = 78

         carrierBandwidth = 100000000.0
         ' (Hz)
         subcarrierSpacing = 30000.0
         ' (Hz)

         sweepTimeAuto = RFmxNRMXSemSweepTimeAuto.True
         sweepTimeInterval = 0.001
         ' (s)

         averagingEnabled = RFmxNRMXSemAveragingEnabled.False
         averagingCount = 10
         averagingType = RFmxNRMXSemAveragingType.Rms

         timeout = 10.0
         ' (s)
      End Sub

      Private Sub InitializeInstr()
         instrSession = New RFmxInstrMX(resourceName, "")
      End Sub

      Private Sub ConfigureNR()
         NR = instrSession.GetNRSignalConfiguration()
         ' Create a new RFmx Session
         instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
         NR.SetSelectedPorts("", selectedPorts)
         NR.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
         NR.ConfigureIQPowerEdgeTrigger("", "0", RFmxNRMXIQPowerEdgeTriggerSlope.Rising, iqPowerEdgeLevel,
            triggerDelay, minimumQuietTimeMode, minimumQuietTime, RFmxNRMXIQPowerEdgeTriggerLevelType.Relative,
            iqPowerEdgeEnabled)

         NR.SetLinkDirection("", linkDirection)
         NR.SetFrequencyRange("", frequencyRange)
         NR.ComponentCarrier.SetBandwidth("", carrierBandwidth)
         NR.ComponentCarrier.SetBandwidthPartSubcarrierSpacing("", subcarrierSpacing)

         NR.SelectMeasurements("", RFmxNRMXMeasurementTypes.Sem, True)

         If linkDirection = RFmxNRMXLinkDirection.Uplink Then
            NR.Sem.Configuration.ConfigureUplinkMaskType("", uplinkMaskType)
         Else
            NR.ConfiguregNodeBCategory("", gNodeBCategory)
            NR.SetBand("", band)
            NR.Sem.Configuration.SetDownlinkMaskType("", downlinkMaskType)
            NR.Sem.Configuration.SetDeltaFMaximum("", deltaFMaximum)
            NR.Sem.Configuration.ComponentCarrier.ConfigureRatedOutputPower("", componentCarrierRatedOutputPower)
         End If

         NR.Sem.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval)
         NR.Sem.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)

         NR.Initiate("", "")
      End Sub

      Private Sub RetrieveResults()
         NR.Sem.Results.FetchUpperOffsetMarginArray("", timeout, upperOffsetMeasurementStatus, upperOffsetMargin,
            upperOffsetMarginFrequency, upperOffsetMarginAbsolutePower, upperOffsetMarginRelativePower)

         NR.Sem.Results.FetchLowerOffsetMarginArray("", timeout, lowerOffsetMeasurementStatus, lowerOffsetMargin,
            lowerOffsetMarginFrequency, lowerOffsetMarginAbsolutePower, lowerOffsetMarginRelativePower)

         NR.Sem.Results.ComponentCarrier.FetchMeasurement("", timeout, absolutePower, peakAbsolutePower,
            peakFrequency, relativePower)

         NR.Sem.Results.FetchMeasurementStatus("", timeout, measurementStatus)

         NR.Sem.Results.FetchSpectrum("", timeout, spectrum, compositeMask)
      End Sub

      Private Sub PrintResults()
         Console.WriteLine("Measurement Status                       : {0}", measurementStatus)
         Console.WriteLine("Carrier Absolute Integrated Power (dBm)  : {0}", absolutePower)

         Console.WriteLine(vbLf & "----------Lower Offset Segment Measurements----------" & vbLf)
         For i As Integer = 0 To lowerOffsetMargin.Length - 1
            Console.WriteLine("Offset  {0}", i)
            Console.WriteLine("Measurement Status                       : {0}", lowerOffsetMeasurementStatus(i))
            Console.WriteLine("Margin (dB)                              : {0}", lowerOffsetMargin(i))
            Console.WriteLine("Margin Frequency (Hz)                    : {0}", lowerOffsetMarginFrequency(i))
            Console.WriteLine("Margin Absolute Power (dBm)              : {0}" & vbLf, lowerOffsetMarginAbsolutePower(i))
         Next

         Console.WriteLine(vbLf & "----------Upper Offset Segment Measurements----------" & vbLf)
         For i As Integer = 0 To upperOffsetMargin.Length - 1
            Console.WriteLine("Offset  {0}", i)
            Console.WriteLine("Measurement Status                       : {0}", upperOffsetMeasurementStatus(i))
            Console.WriteLine("Margin (dB)                              : {0}", upperOffsetMargin(i))
            Console.WriteLine("Margin Frequency (Hz)                    : {0}", upperOffsetMarginFrequency(i))
            Console.WriteLine("Margin Absolute Power (dBm)              : {0}" & vbLf, upperOffsetMarginAbsolutePower(i))
         Next
      End Sub

      Private Sub CloseSession()
         If NR IsNot Nothing Then
            NR.Dispose()
            NR = Nothing
         End If
         If instrSession IsNot Nothing Then
            instrSession.Close()
            instrSession = Nothing
         End If
      End Sub

      Private Shared Sub DisplayError(ex As Exception)
         Console.WriteLine("ERROR:" & vbLf & Convert.ToString(ex.[GetType]()) & ": " & ex.Message)
      End Sub

   End Class
End Namespace
