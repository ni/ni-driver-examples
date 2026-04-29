' Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
'4. Configure Trigger Type and Trigger Parameters.
'5. Configure Component Carrier Spacing.
'6. Configure operating Band.
'7. Configure Duplex Mode.
'8. Configure Component Carriers.
'9. Configure Auto DMRS Detection Enabled.
'10. Select ACP,CHP,ModAcc,OBW And SEM measurements And enable Traces.
'11. Configure Averaging Parameters for ModAcc.
'12. Configure Averaging Parameters for ACP.
'13. Configure Averaging Parameters for CHP.
'14. Configure Averaging Parameters for OBW.
'15. Configure Averaging Parameters for SEM.
'16. Configure ACP Sweep Time.
'17. Configure CHP Sweep Time.
'18. Configure OBW Sweep Time.
'19. Configure SEM Sweep Time.
'20. Configure Uplink Mask Type for SEM.
'21. Configure Synchronization Mode And Measurement Interval.
'22. Initiate the Measurement.
'23. Fetch SEM Measurements.
'24. Fetch OBW Measurements.
'25. Fetch CHP Measurements.
'26. Fetch ACP Measurements.
'27. Fetch ModAcc Measurements.
'28. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.LteMX

Public Class RFmxLteULEvmAcpChpObwSemCompositeContiguousMultiCarrier
   Private instrSession As RFmxInstrMX
   Private lte As RFmxLteMX

   Private resourceName As String, frequencyReferenceSource As String, digitalEdgeSource As String

   Private frequencyReferenceFrequency As Double, centerFrequency As Double, referenceLevel As Double
   Private externalAttenuation As Double, triggerDelay As Double, sweepTimeInterval As Double
   Private timeout As Double

   Private enableTrigger As Boolean

   Private band As Integer, measurementOffset As Integer, measurementLength As Integer
   Private averagingCount As Integer, componentCarrierAtCenterFrequency As Integer

   Const NumberOfComponentCarriers As Integer = 2
   Private componentCarrierFrequency As Double() = {-9225000.0, 2475000.0}
   Private componentCarrierBandwidth As Double() = {5000000.0, 20000000.0}
   Private cellID As Integer() = {0, 1}

   Private digitalEdge As RFmxLteMXDigitalEdgeTriggerEdge
   Private duplexScheme As RFmxLteMXDuplexScheme
   Private componentCarrierSpacingType As RFmxLteMXComponentCarrierSpacingType
   Private synchronizationMode As RFmxLteMXModAccSynchronizationMode
   Private uplinkMaskType As RFmxLteMXSemUplinkMaskType
   Private uplinkDownlinkConfiguration As RFmxLteMXUplinkDownlinkConfiguration

   Private autoDmrsDetectionEnabled As RFmxLteMXAutoDmrsDetectionEnabled

   Private acpTotalAggregatedPower As Double, chpTotalAggregatedPower As Double, semTotalAggregatedPower As Double
   Private obwOccupiedBandwidth As Double, obwAbsolutePower As Double, obwStopFrequency As Double
   Private obwStartFrequency As Double

   Private acpLowerAbsolutePower As Double(), acpUpperAbsolutePower As Double(), acpLowerRelativePower As Double()
   Private acpUpperRelativePower As Double(), acpAbsolutePower As Double(), acpRelativePower As Double()
   Private chpAbsolutePower As Double(), chpRelativePower As Double(), semAbsoluteIntegratedPower As Double()
   Private semRelativeIntegratedPower As Double(), semLowerOffsetMargin As Double()
   Private semLowerOffsetMarginFrequency As Double(), semLowerOffsetMarginAbsolutePower As Double()
   Private semLowerOffsetMarginRelativePower As Double(), semUpperOffsetMargin As Double()
   Private semUpperOffsetMarginFrequency As Double(), semUpperOffsetMarginAbsolutePower As Double()
   Private semUpperOffsetMarginRelativePower As Double(), modAccMeanRmsCompositeEvm As Double()
   Private modAccMaxPeakCompositeEvm As Double(), modAccMeanFrequencyError As Double()
   Private modAccMeanIQOriginOffset As Double(), modAccMeanIQGainImbalance As Double()
   Private modAccMeanIQQuadratureError As Double()

   Private modAccPeakCompositeEvmSlotIndex As Integer(), modAccPeakCompositeEvmSymbolIndex As Integer()
   Private modAccPeakCompositeEvmSubcarrierIndex As Integer()

   Private semMeasurementStatus As RFmxLteMXSemMeasurementStatus
   Private semLowerOffsetMeasurementStatus As RFmxLteMXSemLowerOffsetMeasurementStatus()
   Private semUpperOffsetMeasurementStatus As RFmxLteMXSemUpperOffsetMeasurementStatus()

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
         ' Close session

         CloseSession()
         Console.WriteLine("Press any key to exit")
         Console.ReadKey()
      End Try
   End Sub

   Private Sub InitializeVariables()
      ' Initialize input variables

      resourceName = "RFSA"

      centerFrequency = 1950000000.0
      ' Hz
      referenceLevel = 0.0
      ' dBm
      externalAttenuation = 0.0
      ' dB

      frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
      frequencyReferenceFrequency = 10000000.0
      ' Hz

      enableTrigger = False
      digitalEdgeSource = RFmxLteMXConstants.Pfi0
      digitalEdge = RFmxLteMXDigitalEdgeTriggerEdge.Rising
      triggerDelay = 0.0
      ' seconds

      duplexScheme = RFmxLteMXDuplexScheme.Fdd

      band = 1

      componentCarrierSpacingType = RFmxLteMXComponentCarrierSpacingType.Nominal
      componentCarrierAtCenterFrequency = -1

      synchronizationMode = RFmxLteMXModAccSynchronizationMode.Slot
      measurementOffset = 0
      measurementLength = 1

      uplinkMaskType = RFmxLteMXSemUplinkMaskType.General_NS01
      uplinkDownlinkConfiguration = RFmxLteMXUplinkDownlinkConfiguration.Configuration0

      sweepTimeInterval = 0.001
      ' seconds

      averagingCount = 10

      autoDmrsDetectionEnabled = RFmxLteMXAutoDmrsDetectionEnabled.True

      timeout = 10.0
      ' seconds
   End Sub

   Private Sub InitializeInstr()
      ' Create a new RFmx Session

      instrSession = New RFmxInstrMX(resourceName, "")
   End Sub

   Private Sub ConfigureLte()
      ' Get Lte signal 

      lte = instrSession.GetLteSignalConfiguration()

      ' Configure measurement 

      instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)

      lte.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)

      lte.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger)

      lte.ComponentCarrier.ConfigureSpacing("", componentCarrierSpacingType, componentCarrierAtCenterFrequency)

      lte.ConfigureBand("", band)

      lte.ConfigureDuplexScheme("", duplexScheme, uplinkDownlinkConfiguration)

      lte.ConfigureNumberOfComponentCarriers("", NumberOfComponentCarriers)

      lte.ComponentCarrier.ConfigureArray("", componentCarrierBandwidth, componentCarrierFrequency, cellID)

      lte.ConfigureAutoDmrsDetectionEnabled("", autoDmrsDetectionEnabled)

      lte.SelectMeasurements("", RFmxLteMXMeasurementTypes.Acp Or RFmxLteMXMeasurementTypes.Chp Or
                             RFmxLteMXMeasurementTypes.ModAcc Or RFmxLteMXMeasurementTypes.Obw Or
                             RFmxLteMXMeasurementTypes.Sem, True)

      lte.ModAcc.Configuration.ConfigureAveraging("", RFmxLteMXModAccAveragingEnabled.[False], averagingCount)

      lte.Acp.Configuration.ConfigureAveraging("", RFmxLteMXAcpAveragingEnabled.[False], averagingCount,
                                               RFmxLteMXAcpAveragingType.Rms)

      lte.Chp.Configuration.ConfigureAveraging("", RFmxLteMXChpAveragingEnabled.[False], averagingCount,
                                               RFmxLteMXChpAveragingType.Rms)

      lte.Obw.Configuration.ConfigureAveraging("", RFmxLteMXObwAveragingEnabled.[False], averagingCount,
                                               RFmxLteMXObwAveragingType.Rms)

      lte.Sem.Configuration.ConfigureAveraging("", RFmxLteMXSemAveragingEnabled.[False], averagingCount,
                                               RFmxLteMXSemAveragingType.Rms)

      lte.Acp.Configuration.ConfigureSweepTime("", RFmxLteMXAcpSweepTimeAuto.[True], sweepTimeInterval)

      lte.Chp.Configuration.ConfigureSweepTime("", RFmxLteMXChpSweepTimeAuto.[True], sweepTimeInterval)

      lte.Obw.Configuration.ConfigureSweepTime("", RFmxLteMXObwSweepTimeAuto.[True], sweepTimeInterval)

      lte.Sem.Configuration.ConfigureSweepTime("", RFmxLteMXSemSweepTimeAuto.[True], sweepTimeInterval)

      lte.Sem.Configuration.ConfigureUplinkMaskType("", uplinkMaskType)

      lte.ModAcc.Configuration.ConfigureSynchronizationModeAndInterval("", synchronizationMode, measurementOffset,
                                                                       measurementLength)

      lte.Initiate("", "")
   End Sub

   Private Sub RetrieveResults()
      ' Retrieve results

      lte.Sem.Results.FetchLowerOffsetMarginArray("", timeout, semLowerOffsetMeasurementStatus, semLowerOffsetMargin,
                                                  semLowerOffsetMarginFrequency, semLowerOffsetMarginAbsolutePower,
                                                  semLowerOffsetMarginRelativePower)

      lte.Sem.Results.FetchUpperOffsetMarginArray("", timeout, semUpperOffsetMeasurementStatus, semUpperOffsetMargin,
                                                  semUpperOffsetMarginFrequency, semUpperOffsetMarginAbsolutePower,
                                                  semUpperOffsetMarginRelativePower)

      lte.Sem.Results.ComponentCarrier.FetchMeasurementArray("", timeout, semAbsoluteIntegratedPower,
                                                             semRelativeIntegratedPower)

      lte.Sem.Results.FetchMeasurementStatus("", timeout, semMeasurementStatus)

      lte.Sem.Results.FetchTotalAggregatedPower("", timeout, semTotalAggregatedPower)

      lte.Obw.Results.FetchMeasurement("", timeout, obwOccupiedBandwidth, obwAbsolutePower, obwStartFrequency,
                                       obwStopFrequency)

      lte.Chp.Results.ComponentCarrier.FetchMeasurementArray("", timeout, chpAbsolutePower, chpRelativePower)

      lte.Chp.Results.FetchTotalAggregatedPower("", timeout, chpTotalAggregatedPower)

      lte.Acp.Results.FetchOffsetMeasurementArray("", timeout, acpLowerRelativePower, acpUpperRelativePower,
                                                  acpLowerAbsolutePower, acpUpperAbsolutePower)

      lte.Acp.Results.ComponentCarrier.FetchMeasurementArray("", timeout, acpAbsolutePower, acpRelativePower)

      lte.Acp.Results.FetchTotalAggregatedPower("", timeout, acpTotalAggregatedPower)

      lte.ModAcc.Results.FetchCompositeEvmArray("", timeout, modAccMeanRmsCompositeEvm, modAccMaxPeakCompositeEvm,
                                                modAccMeanFrequencyError, modAccPeakCompositeEvmSymbolIndex,
                                                modAccPeakCompositeEvmSubcarrierIndex, modAccPeakCompositeEvmSlotIndex)

      lte.ModAcc.Results.FetchIQImpairmentsArray("", timeout, modAccMeanIQOriginOffset, modAccMeanIQGainImbalance,
                                                 modAccMeanIQQuadratureError)
   End Sub

   Private Sub PrintResults()
      Console.WriteLine("************************* ModAcc *************************" & vbLf)
      Console.WriteLine("---------------- Measurements ----------------")
      For i As Integer = 0 To modAccMeanRmsCompositeEvm.Length - 1
         Console.WriteLine("Carrier  : {0}" & vbLf, i)
         Console.WriteLine("Mean RMS Composite EVM  (%)    : {0}", modAccMeanRmsCompositeEvm(i))
         Console.WriteLine("Max Peak Composite EVM  (% )   : {0}", modAccMaxPeakCompositeEvm(i))
         Console.WriteLine("Mean Frequency Error    (Hz)   : {0}", modAccMeanFrequencyError(i))
         Console.WriteLine("Mean IQ Origin Offset   (dBc)  : {0}", modAccMeanIQOriginOffset(i))
      Next

      Console.WriteLine(vbLf & "************************* CHP *************************" & vbLf)
      Console.WriteLine("Total Aggregated Power  (dBm)    : {0}", chpTotalAggregatedPower)
      Console.WriteLine(vbLf & "----- Component Carrier Measurements -----")
      For i As Integer = 0 To chpAbsolutePower.Length - 1
         Console.WriteLine(vbLf & " Carrier  :{0}", i)
         Console.WriteLine("Absolute Power  (dBm)  : {0}", chpAbsolutePower(i))
         Console.WriteLine("Relative Power  (dB)   : {0}", chpRelativePower(i))
      Next

      Console.WriteLine(vbLf & "************************* ACP *************************" & vbLf)
      Console.WriteLine("Total Aggregated Power  (dBm)    : {0}", acpTotalAggregatedPower)
      Console.WriteLine(vbLf & "----- Component Carrier Measurements -----")
      For i As Integer = 0 To acpAbsolutePower.Length - 1
         Console.WriteLine(vbLf & " Carrier  :{0}", i)
         Console.WriteLine("Absolute Power  (dBm)  : {0}", acpAbsolutePower(i))
         Console.WriteLine("Relative Power  (dB)   : {0}", acpRelativePower(i))
      Next

      Console.WriteLine(vbLf & "------ Offset Channel Measurements -------")
      For i As Integer = 0 To acpLowerRelativePower.Length - 1
         Console.WriteLine(vbLf & "Offset  :{0}", i)
         Console.WriteLine("Lower Relative Power (dB)  : {0}", acpLowerRelativePower(i))
         Console.WriteLine("Upper Relative Power (dB)  : {0}", acpUpperRelativePower(i))
         Console.WriteLine("Lower Absolute Power (dBm) : {0}", acpLowerAbsolutePower(i))
         Console.WriteLine("Upper Absolute Power (dBm) : {0}", acpUpperAbsolutePower(i))
      Next

      Console.WriteLine(vbLf & "************************* OBW *************************" & vbLf)
      Console.WriteLine("---------------- Measurement ----------------")
      Console.WriteLine("Occupied Bandwidth  (Hz)  : {0}", obwOccupiedBandwidth)
      Console.WriteLine("Absolute Power      (dBm) : {0}", obwAbsolutePower)
      Console.WriteLine("Start Frequency     (Hz)  : {0}", obwStartFrequency)
      Console.WriteLine("Stop Frequency      (Hz)  : {0}", obwStopFrequency)

      Console.WriteLine(vbLf & "************************* SEM *************************" & vbLf)
      Console.WriteLine("Measurement Status               : {0}", semMeasurementStatus)
      Console.WriteLine("Total Aggregated Power  (dBm)    : {0}", semTotalAggregatedPower)
      Console.WriteLine(vbLf & "-----Component Carrier Measurements ------")
      For i As Integer = 0 To semAbsoluteIntegratedPower.Length - 1
         Console.WriteLine("Carrier  : {0}", i)
         Console.WriteLine("Absolute Integrated Power  (dBm)  : {0}", semAbsoluteIntegratedPower(i))
         Console.WriteLine("Relative Integrated Power  (dB)   : {0}", semRelativeIntegratedPower(i))
      Next

      Console.WriteLine(vbLf & "---- Lower Offset Segment Measurements ---- ")
      For i As Integer = 0 To semLowerOffsetMargin.Length - 1
         Console.WriteLine(vbLf & "Offset  : {0}", i)
         Console.WriteLine("Measurement Status            : {0}", semLowerOffsetMeasurementStatus(i))
         Console.WriteLine("Margin                 (dB)   : {0}", semLowerOffsetMargin(i))
         Console.WriteLine("Margin Frequency       (Hz)   : {0}", semLowerOffsetMarginFrequency(i))

         Console.WriteLine("Margin Absolute Power  (dBm)  : {0}", semLowerOffsetMarginAbsolutePower(i))
      Next
      Console.WriteLine(vbLf & "---- Upper Offset Segment Measurements ---- ")
      For i As Integer = 0 To semUpperOffsetMargin.Length - 1
         Console.WriteLine(vbLf & "Offset  :{0}", i)
         Console.WriteLine("Measurement Status            : {0}", semUpperOffsetMeasurementStatus(i))
         Console.WriteLine("Margin                 (dB)   : {0}", semUpperOffsetMargin(i))
         Console.WriteLine("Margin Frequency       (Hz)   : {0}", semUpperOffsetMarginFrequency(i))
         Console.WriteLine("Margin Absolute Power  (dBm)  : {0}", semUpperOffsetMarginAbsolutePower(i))
      Next
   End Sub

   Private Sub CloseSession()
      Try
         If lte IsNot Nothing Then
            lte.Dispose()
            lte = Nothing
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
