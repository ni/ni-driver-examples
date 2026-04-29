'''/ Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
'4. Configure Trigger Type and Trigger Parameters.
'5. Configure Component Carrier to 200k.
'6. Configure NB-IoT Component Carrier.
'7. Configure NPUSCH Format.
'8. Configure Auto NPUSCH Channel Detection Enabled.
'9. Configure NPUSCH Starting Slot.
'10. Configure NPUSCH DMRS.
'11. Select ACP,ModAcc,OBW,CHP and SEM measurements and enable Traces.
'12. Configure Averaging Parameters for ModAcc.
'13. Configure Averaging Parameters for ACP.
'14. Configure Averaging Parameters for CHP.
'15. Configure Averaging Parameters for OBW.
'16. Configure Averaging Parameters for SEM.
'17. Configure ACP Sweep Time.
'18. Configure CHP Sweep Time.
'19. Configure OBW Sweep Time.
'20. Configure SEM Sweep Time.
'21. Configure ModAcc Synchronization Mode and Measurement Interval.
'22. Initiate the Measurement.
'23. Fetch ModAcc Measurements.
'24. Fetch ACP Measurements.
'25. Fetch SEM Measurements.
'26. Fetch OBW Measurement.
'27. Fetch CHP Measurement.
'28. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.LteMX

Public Class RFmxLteNBIoTModAccAcpChpObwSemCompositeSingleCarrier
   Private instrSession As RFmxInstrMX
   Private lte As RFmxLteMX

   Private resourceName As String, frequencyReferenceSource As String, iqPowerEdgeTriggerSource As String

   Private frequencyReferenceFrequency As Double, centerFrequency As Double, referenceLevel As Double
   Private externalAttenuation As Double, triggerDelay As Double, sweepTimeInterval As Double, timeout As Double

   Private enableTrigger As Boolean

   Private measurementOffset As Integer, measurementLength As Integer, averagingCount As Integer

   Private componentCarrierFrequency As Double
   Private componentCarrierBandwidth As Double
   Private cellID As Integer, nCellID As Integer
   Private nPuschFormat As Integer

   Private synchronizationMode As RFmxLteMXModAccSynchronizationMode
   Private uplinkSubcarrierSpacing As RFmxLteMXNBIoTUplinkSubcarrierSpacing
   Private iqPowerEdgeTriggerSlope As RFmxLteMXIQPowerEdgeTriggerSlope
   Private minimumQuietTimeMode As RFmxLteMXTriggerMinimumQuietTimeMode
   Private iqPowerEdgeTriggerLevel As Double, minimumQuietTimeDuration As Double
   Private iqPowerEdgeTriggerLevelType As RFmxLteMXIQPowerEdgeTriggerLevelType
   Private obwOccupiedBandwidth As Double, obwAbsolutePower As Double, obwStopFrequency As Double
   Private obwStartFrequency As Double

   Private acpLowerAbsolutePower As Double(), acpUpperAbsolutePower As Double(), acpLowerRelativePower As Double()
   Private acpUpperRelativePower As Double(), semLowerOffsetMargin As Double()
   Private semLowerOffsetMarginFrequency As Double(), semLowerOffsetMarginAbsolutePower As Double()
   Private semLowerOffsetMarginRelativePower As Double(), semUpperOffsetMargin As Double()
   Private semUpperOffsetMarginFrequency As Double(), semUpperOffsetMarginAbsolutePower As Double()
   Private semUpperOffsetMarginRelativePower As Double()
   Private modAccMeanRmsCompositeEvm As Double, modAccMaxPeakCompositeEvm As Double
   Private modAccMeanFrequencyError As Double, modAccMeanIQOriginOffset As Double, modAccMeanIQGainImbalance As Double
   Private modAccMeanIQQuadratureError As Double, modAccInBandEmissionMargin As Double
   Private semAbsoluteIntegratedPower As Double, semRelativeIntegratedPower As Double, chpAbsolutePower As Double
   Private chpRelativePower As Double, acpAbsolutePower As Double, acpRelativePower As Double

   Private modAccPeakCompositeEvmSlotIndex As Integer, modAccPeakCompositeEvmSymbolIndex As Integer
   Private modAccPeakCompositeEvmSubcarrierIndex As Integer

   Private nPuschDmrsBaseSequenceIndex As Integer, nPuschDmrsCyclicShift As Integer, nPuschDmrsDeltaSS As Integer

   Private nPuschStartingSlot As Integer

   Private autoNPuschChannelDetectionEnabled As RFmxLteMXAutoNPuschChannelDetectionEnabled

   Private nPuschDmrsBaseSequenceMode As RFmxLteMXNPuschDmrsBaseSequenceMode
   Private nPuschDmrsGroupHoppingEnabled As RFmxLteMXNPuschDmrsGroupHoppingEnabled

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

      componentCarrierFrequency = 0.0
      componentCarrierBandwidth = 200000.0
      cellID = 0
      frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
      frequencyReferenceFrequency = 10000000.0
      ' Hz

      iqPowerEdgeTriggerSource = "0"
      iqPowerEdgeTriggerSlope = RFmxLteMXIQPowerEdgeTriggerSlope.Rising
      iqPowerEdgeTriggerLevel = -20.0

      enableTrigger = True
      triggerDelay = 0.0
      ' seconds
      minimumQuietTimeDuration = 0.0001
      ' seconds

      minimumQuietTimeMode = RFmxLteMXTriggerMinimumQuietTimeMode.Auto
      iqPowerEdgeTriggerLevelType = RFmxLteMXIQPowerEdgeTriggerLevelType.Relative

      nPuschStartingSlot = 0

      nPuschDmrsBaseSequenceIndex = 0
      nPuschDmrsCyclicShift = 0
      nPuschDmrsDeltaSS = 0

      nPuschDmrsBaseSequenceMode = RFmxLteMXNPuschDmrsBaseSequenceMode.Auto
      nPuschDmrsGroupHoppingEnabled = RFmxLteMXNPuschDmrsGroupHoppingEnabled.False

      nPuschFormat = 1
      uplinkSubcarrierSpacing = RFmxLteMXNBIoTUplinkSubcarrierSpacing.SubcarrierSpacing15kHz
      nCellID = 0
      synchronizationMode = RFmxLteMXModAccSynchronizationMode.Slot
      measurementOffset = 0
      ' Slots
      measurementLength = 1
      ' Slots

      sweepTimeInterval = 0.001
      ' seconds

      averagingCount = 10

      autoNPuschChannelDetectionEnabled = RFmxLteMXAutoNPuschChannelDetectionEnabled.True

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

      lte.ConfigureIQPowerEdgeTrigger("", iqPowerEdgeTriggerSource, iqPowerEdgeTriggerSlope, iqPowerEdgeTriggerLevel,
         triggerDelay, minimumQuietTimeMode, minimumQuietTimeDuration, iqPowerEdgeTriggerLevelType, enableTrigger)

      lte.ComponentCarrier.Configure("", componentCarrierBandwidth, componentCarrierFrequency, cellID)

      lte.ComponentCarrier.ConfigureNBIoTComponentCarrier("", nCellID, uplinkSubcarrierSpacing)

      lte.ComponentCarrier.ConfigureNPuschFormat("", nPuschFormat)

      lte.ComponentCarrier.ConfigureAutoNPuschChannelDetectionEnabled("", autoNPuschChannelDetectionEnabled)

      lte.ComponentCarrier.ConfigureNPuschStartingSlot("", nPuschStartingSlot)

      lte.ComponentCarrier.ConfigureNPuschDmrs("", nPuschDmrsBaseSequenceMode, nPuschDmrsBaseSequenceIndex,
                                               nPuschDmrsCyclicShift, nPuschDmrsGroupHoppingEnabled, nPuschDmrsDeltaSS)

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

      lte.ModAcc.Configuration.ConfigureSynchronizationModeAndInterval("", synchronizationMode,
                                                                       measurementOffset, measurementLength)

      lte.Initiate("", "")
   End Sub

   Private Sub RetrieveResults()
      ' Retrieve results

      lte.ModAcc.Results.FetchCompositeEvm("", timeout, modAccMeanRmsCompositeEvm, modAccMaxPeakCompositeEvm,
                                           modAccMeanFrequencyError, modAccPeakCompositeEvmSymbolIndex,
                                           modAccPeakCompositeEvmSubcarrierIndex, modAccPeakCompositeEvmSlotIndex)

      lte.ModAcc.Results.FetchIQImpairments("", timeout, modAccMeanIQOriginOffset, modAccMeanIQGainImbalance,
                                            modAccMeanIQQuadratureError)

      lte.ModAcc.Results.FetchInBandEmissionMargin("", timeout, modAccInBandEmissionMargin)

      lte.Acp.Results.FetchOffsetMeasurementArray("", timeout, acpLowerRelativePower, acpUpperRelativePower,
                                                  acpLowerAbsolutePower, acpUpperAbsolutePower)

      lte.Acp.Results.ComponentCarrier.FetchMeasurement("", timeout, acpAbsolutePower, acpRelativePower)

      lte.Sem.Results.FetchLowerOffsetMarginArray("", timeout, semLowerOffsetMeasurementStatus, semLowerOffsetMargin,
                                                  semLowerOffsetMarginFrequency, semLowerOffsetMarginAbsolutePower,
                                                  semLowerOffsetMarginRelativePower)

      lte.Sem.Results.FetchUpperOffsetMarginArray("", timeout, semUpperOffsetMeasurementStatus, semUpperOffsetMargin,
                                                  semUpperOffsetMarginFrequency, semUpperOffsetMarginAbsolutePower,
                                                  semUpperOffsetMarginRelativePower)

      lte.Sem.Results.ComponentCarrier.FetchMeasurement("", timeout, semAbsoluteIntegratedPower,
                                                        semRelativeIntegratedPower)

      lte.Sem.Results.FetchMeasurementStatus("", timeout, semMeasurementStatus)

      lte.Obw.Results.FetchMeasurement("", timeout, obwOccupiedBandwidth, obwAbsolutePower, obwStartFrequency,
                                       obwStopFrequency)

      lte.Chp.Results.ComponentCarrier.FetchMeasurement("", timeout, chpAbsolutePower, chpRelativePower)
   End Sub

   Private Sub PrintResults()
      Console.WriteLine("************************* ModAcc *************************" & vbLf)
      Console.WriteLine("---------------- Measurements ----------------")
      Console.WriteLine("Mean RMS Composite EVM (% or dB)         : {0}", modAccMeanRmsCompositeEvm)
      Console.WriteLine("Maximum Peak Composite EVM (% or dB)     : {0}", modAccMaxPeakCompositeEvm)
      Console.WriteLine("Peak Composite EVM Slot Index            : {0}", modAccPeakCompositeEvmSlotIndex)
      Console.WriteLine("Peak Composite EVM Symbol Index          : {0}", modAccPeakCompositeEvmSymbolIndex)
      Console.WriteLine("Peak Composite EVM Subcarrier Index      : {0}", modAccPeakCompositeEvmSubcarrierIndex)
      Console.WriteLine("Mean Frequency Error    (Hz)             : {0}", modAccMeanFrequencyError)
      Console.WriteLine("Mean IQ Origin Offset   (dBc)            : {0}", modAccMeanIQOriginOffset)
      Console.WriteLine("Mean IQ Gain Imbalance (dB)              : {0}", modAccMeanIQGainImbalance)
      Console.WriteLine("Mean IQ Quadrature Error (deg)           : {0}", modAccMeanIQQuadratureError)
      Console.WriteLine("In-Band Emission Margin (dB)             : {0}", modAccInBandEmissionMargin)

      Console.WriteLine(vbLf & "************************* ACP *************************" & vbLf)
      Console.WriteLine("Carrier Absolute Power (dBm)    : {0}", acpAbsolutePower)

      Console.WriteLine(vbLf & "------ Offset Channel Measurements -------")
      For i As Integer = 0 To acpLowerRelativePower.Length - 1
         Console.WriteLine(vbLf & "Offset  :{0}", i)
         Console.WriteLine("Lower Relative Power (dB)  : {0}", acpLowerRelativePower(i))
         Console.WriteLine("Upper Relative Power (dB)  : {0}", acpUpperRelativePower(i))
         Console.WriteLine("Lower Absolute Power (dBm) : {0}", acpLowerAbsolutePower(i))
         Console.WriteLine("Upper Absolute Power (dBm) : {0}", acpUpperAbsolutePower(i))
      Next

      Console.WriteLine(vbLf & "************************* SEM *************************" & vbLf)
      Console.WriteLine("Measurement Status                      : {0}", semMeasurementStatus)
      Console.WriteLine("Carrier Absolute Integrated Power (dBm) : {0}", semAbsoluteIntegratedPower)

      Console.WriteLine(vbLf & "---- Lower Offset Segment Measurements ---- ")
      For i As Integer = 0 To semLowerOffsetMarginAbsolutePower.Length - 1
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

      Console.WriteLine(vbLf & "************************* OBW *************************" & vbLf)
      Console.WriteLine("---------------- Measurement ----------------")
      Console.WriteLine("Occupied Bandwidth  (Hz)  : {0}", obwOccupiedBandwidth)
      Console.WriteLine("Absolute Power      (dBm) : {0}", obwAbsolutePower)
      Console.WriteLine("Start Frequency     (Hz)  : {0}", obwStartFrequency)
      Console.WriteLine("Stop Frequency      (Hz)  : {0}", obwStopFrequency)


      Console.WriteLine(vbLf & "************************* CHP *************************" & vbLf)
      Console.WriteLine("Carrier Absolute Power (dBm)   : {0}", chpAbsolutePower)
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
