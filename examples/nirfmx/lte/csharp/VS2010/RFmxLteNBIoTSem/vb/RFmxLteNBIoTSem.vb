'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
'4. Configure Trigger Type and Trigger Parameters.
'5. Select Carrier Bandwidth as 200k.
'6. Configure Uplink Subcarrier Spacing.
'7. Select SEM measurement and enable Traces.
'8. Configure Sweep Time Parameters.
'9. Configure Averaging Parameters for SEM measurement.
'10. Initiate the Measurement.
'11. Fetch SEM Measurements and Traces.
'12. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.LteMX

Public Class RFmxLteNBIoTSem
   Private instrSession As RFmxInstrMX
   Private lte As RFmxLteMX
   Private rfsaResourceName As String

   Private frequencyReferenceSource As String
   Private frequencyReferenceFrequency As Double

   Private centerFrequency As Double
   Private referenceLevel As Double
   Private externalAttenuation As Double

   Private enableTrigger As Boolean
   Private iqPowerEdgeTriggerSlope As RFmxLteMXIQPowerEdgeTriggerSlope
   Private iqPowerEdgeTriggerLevel As Double
   Private minimumQuiteTimeMode As RFmxLteMXTriggerMinimumQuietTimeMode
   Private minimumQuietTime As Double
   Private iqPowerEdgeTriggerLevelType As RFmxLteMXIQPowerEdgeTriggerLevelType
   Private triggerDelay As Double
   Private linkDirection As RFmxLteMXLinkDirection

   Private uplinkSubcarrierSpacing As RFmxLteMXNBIoTUplinkSubcarrierSpacing
   Private eNodeBCategory As RFmxLteMXeNodeBCategory
   Private downlinkMaskType As RFmxLteMXSemDownlinkMaskType
   Private deltaFMaximum As Double
   Private aggregatedMaximumPower As Double
   Private maximumOutputPower As Double

   Private sweepTimeAuto As RFmxLteMXSemSweepTimeAuto
   Private sweepTimeInterval As Double

   Private averagingEnabled As RFmxLteMXSemAveragingEnabled
   Private averagingCount As Integer
   Private averagingType As RFmxLteMXSemAveragingType

   Private componentCarrierBandwidth As Double
   Private componentCarrierFrequency As Double
   Private cellID As Integer

   Private nCellID As Integer

   Private timeout As Double

   Private measurementStatus As RFmxLteMXSemMeasurementStatus

   Private absoluteIntegratedPower As Double
   '(dBm)
   Private relativeIntegratedPower As Double
   '(dBm)

   Private upperOffsetMeasurementStatus As RFmxLteMXSemUpperOffsetMeasurementStatus()
   Private upperOffsetMargin As Double()
   '(dB)
   Private upperOffsetMarginFrequency As Double()
   '(Hz)
   Private upperOffsetMarginAbsolutePower As Double()
   '(dBm)
   Private upperOffsetMarginRelativePower As Double()
   '(dBm)

   Private lowerOffsetMeasurementStatus As RFmxLteMXSemLowerOffsetMeasurementStatus()
   Private lowerOffsetMargin As Double()
   '(dB)
   Private lowerOffsetMarginFrequency As Double()
   '(Hz)
   Private lowerOffsetMarginAbsolutePower As Double()
   '(dBm)
   Private lowerOffsetMarginRelativePower As Double()
   '(dBm)

   Private spectrum As Spectrum(Of Single)
   Private compositeMask As Spectrum(Of Single)

   Private Sub CloseSession()
      If lte IsNot Nothing Then
         lte.Dispose()
         lte = Nothing
      End If
      If instrSession IsNot Nothing Then
         instrSession.Close()
         instrSession = Nothing
      End If
   End Sub

   Public Sub Run()
      Try
         InitializeVariables()
         InitializeInstr()
         ConfigureLte()
         RetrieveResults()
         PrintResults()
      Catch ex As Exception
         DisplayError(ex)
      Finally
         'Close session

         CloseSession()
         Console.WriteLine("Press any key to exit")
         Console.ReadKey()
      End Try
   End Sub

   Private Sub InitializeInstr()
      instrSession = New RFmxInstrMX(rfsaResourceName, "")
   End Sub

   Private Sub InitializeVariables()
      rfsaResourceName = "RFSA"

      frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
      frequencyReferenceFrequency = 10000000.0
      '(Hz)

      centerFrequency = 1950000000.0
      '(Hz)
      referenceLevel = 0.0
      '(dBm)
      externalAttenuation = 0.0
      '(dB)

      enableTrigger = True
      iqPowerEdgeTriggerSlope = RFmxLteMXIQPowerEdgeTriggerSlope.Rising
      iqPowerEdgeTriggerLevel = -20.0
      '(dB)
      minimumQuiteTimeMode = RFmxLteMXTriggerMinimumQuietTimeMode.Auto
      minimumQuietTime = 0.0001
      '(s)
      iqPowerEdgeTriggerLevelType = RFmxLteMXIQPowerEdgeTriggerLevelType.Relative
      triggerDelay = 0.0
      '(s)
      linkDirection = RFmxLteMXLinkDirection.Uplink
      uplinkSubcarrierSpacing = RFmxLteMXNBIoTUplinkSubcarrierSpacing.SubcarrierSpacing15kHz
	  
	  eNodeBCategory = RFmxLteMXeNodeBCategory.WideAreaBaseStationCategoryA
         downlinkMaskType = RFmxLteMXSemDownlinkMaskType.ENodeBCategoryBased
         deltaFMaximum = 15000000.0
         ' (Hz)
		 aggregatedMaximumPower = 0.0
         ' (dBm)
         maximumOutputPower = 0.0
         ' (dBm)

      sweepTimeAuto = RFmxLteMXSemSweepTimeAuto.True
      sweepTimeInterval = 0.001
      '(s)

      averagingEnabled = RFmxLteMXSemAveragingEnabled.False
      averagingCount = 10
      averagingType = RFmxLteMXSemAveragingType.Rms

      componentCarrierBandwidth = 200000.0
      '(Hz)
      componentCarrierFrequency = 0.0
      '(Hz)

      cellID = 0

      nCellID = 0

      timeout = 10.0
      '(s)
   End Sub

   Private Sub ConfigureLte()
      lte = instrSession.GetLteSignalConfiguration()
      'Create a new RFmx Session
      instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
      lte.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
      lte.ConfigureIQPowerEdgeTrigger("", "0", iqPowerEdgeTriggerSlope, iqPowerEdgeTriggerLevel, triggerDelay,
         minimumQuiteTimeMode, minimumQuietTime, iqPowerEdgeTriggerLevelType, enableTrigger)
	  lte.ConfigureLinkDirection("", linkDirection)
      lte.ComponentCarrier.Configure("", componentCarrierBandwidth, componentCarrierFrequency, cellID)
      lte.SelectMeasurements("", RFmxLteMXMeasurementTypes.Sem, True)
      lte.Sem.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval)
      lte.Sem.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)
	  If linkDirection = RFmxLteMXLinkDirection.Uplink Then
         lte.ComponentCarrier.ConfigureNBIoTComponentCarrier("", nCellID, uplinkSubcarrierSpacing)
      ElseIf linkDirection = RFmxLteMXLinkDirection.Downlink Then
         lte.ConfigureeNodeBCategory("", eNodeBCategory)
         lte.Sem.Configuration.ConfigureDownlinkMask("", downlinkMaskType, deltaFMaximum, aggregatedMaximumPower)
         lte.Sem.Configuration.ComponentCarrier.ConfigureMaximumOutputPower("", maximumOutputPower)
	  End If	 
      lte.Initiate("", "")
   End Sub

   Private Sub RetrieveResults()
      lte.Sem.Results.FetchUpperOffsetMarginArray("", timeout, upperOffsetMeasurementStatus, upperOffsetMargin,
        upperOffsetMarginFrequency, upperOffsetMarginAbsolutePower, upperOffsetMarginRelativePower)
      lte.Sem.Results.FetchLowerOffsetMarginArray("", timeout, lowerOffsetMeasurementStatus, lowerOffsetMargin,
        lowerOffsetMarginFrequency, lowerOffsetMarginAbsolutePower, lowerOffsetMarginRelativePower)
      lte.Sem.Results.ComponentCarrier.FetchMeasurement("", timeout, absoluteIntegratedPower, relativeIntegratedPower)
      lte.Sem.Results.FetchMeasurementStatus("", timeout, measurementStatus)
      lte.Sem.Results.FetchSpectrum("", timeout, spectrum, compositeMask)
   End Sub

   Private Sub PrintResults()
      Console.WriteLine("Measurement Status                         :{0}", measurementStatus)
      Console.WriteLine("Carrier Absolute Integrated Power (dBm)    :{0}", absoluteIntegratedPower)

      Console.WriteLine(vbLf & vbLf & "----------Lower Offset Segment Measurements----------" & vbLf)
      For i As Integer = 0 To lowerOffsetMargin.Length - 1
         Console.WriteLine("Offset {0}", i)
         Console.WriteLine("Measurement Status              :{0}", lowerOffsetMeasurementStatus(i))
         Console.WriteLine("Margin (dB)                     :{0}", lowerOffsetMargin(i))
         Console.WriteLine("Margin Frequency (Hz)           :{0}", lowerOffsetMarginFrequency(i))
         Console.WriteLine("Margin Absolute Power (dBm)     :{0}" & vbLf, lowerOffsetMarginAbsolutePower(i))
      Next

      Console.WriteLine(vbLf & vbLf & "----------Upper Offset Segment Measurements----------" & vbLf)
      For i As Integer = 0 To upperOffsetMargin.Length - 1
         Console.WriteLine("Offset {0}", i)
         Console.WriteLine("Measurement Status              :{0}", upperOffsetMeasurementStatus(i))
         Console.WriteLine("Margin (dB)                     :{0}", upperOffsetMargin(i))
         Console.WriteLine("Margin Frequency (Hz)           :{0}", upperOffsetMarginFrequency(i))
         Console.WriteLine("Margin Absolute Power (dBm)     :{0}" & vbLf, upperOffsetMarginAbsolutePower(i))
      Next

   End Sub

   Private Shared Sub DisplayError(ex As Exception)
      Console.WriteLine("ERROR:" & vbLf & Convert.ToString(ex.[GetType]()) & ": " & ex.Message)
   End Sub

End Class
