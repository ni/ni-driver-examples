'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
'4. Configure Trigger Type and Trigger Parameters.
'5. Configure Component Carrier.
'6. Configure operating Band.
'7. Configure Duplex Mode.
'8. Configure Auto DMRS Detection Enabled.
'9. Select ACP,CHP,ModAcc,OBW And SEM measurements And enable Traces.
'10. Configure Averaging Parameters for ModAcc.
'11. Configure Averaging Parameters for ACP.
'12. Configure Averaging Parameters for CHP.
'13. Configure Averaging Parameters for OBW.
'14. Configure Averaging Parameters for SEM.
'15. Configure ACP Sweep Time.
'16. Configure CHP Sweep Time.
'17. Configure OBW Sweep Time.
'18. Configure SEM Sweep Time.
'19. Configure Standard Mask Type for SEM.
'20. Configure Synchronization Mode And Measurement Interval.
'21. Initiate the Measurement.
'22. Fetch SEM Measurements.
'23. Fetch OBW Measurements.
'24. Fetch CHP Measurements.
'25. Fetch ACP Measurements.
'26. Fetch ModAcc Measurements.
'27. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.LteMX

Public Class RFmxLteModAccAcpChpObwSemCompositeSingleCarrier
   Private instrSession As RFmxInstrMX
   Private lte As RFmxLteMX

   Private resourceName As String, frequencyReferenceSource As String, digitalEdgeSource As String

   Private frequencyReferenceFrequency As Double, centerFrequency As Double, referenceLevel As Double
   Private externalAttenuation As Double, triggerDelay As Double, componentCarrierFrequency As Double
   Private componentCarrierBandwidth As Double, sweepTimeInterval As Double, timeout As Double

   Private enableTrigger As Boolean

   Private band As Integer, cellID As Integer, measurementOffset As Integer, measurementLength As Integer
   Private averagingCount As Integer

   Private digitalEdge As RFmxLteMXDigitalEdgeTriggerEdge
   Private duplexScheme As RFmxLteMXDuplexScheme
   Private synchronizationMode As RFmxLteMXModAccSynchronizationMode
   Private uplinkMaskType As RFmxLteMXSemUplinkMaskType
   Private uplinkDownlinkConfiguration As RFmxLteMXUplinkDownlinkConfiguration

   Private autoDmrsDetectionEnabled As RFmxLteMXAutoDmrsDetectionEnabled

   Private acpAbsolutePower As Double, acpRelativePower As Double, chpAbsolutePower As Double
   Private chpRelativePower As Double, modAccMeanRmsCompositeEvm As Double, modAccMaxPeakCompositeEvm As Double
   Private modAccMeanFrequencyError As Double, modAccMeanIQOriginOffset As Double, modAccMeanIQGainImbalance As Double
   Private modAccMeanIQQuadratureError As Double, obwOccupiedBandwidth As Double, obwAbsolutePower As Double
   Private obwStopFrequency As Double, obwStartFrequency As Double, semAbsoluteIntegratedPower As Double
   Private semRelativeIntegratedPower As Double

   Private acpLowerAbsolutePower As Double(), acpUpperAbsolutePower As Double(), acpLowerRelativePower As Double()
   Private acpUpperRelativePower As Double(), semLowerOffsetMargin As Double()
   Private semLowerOffsetMarginFrequency As Double(), semLowerOffsetMarginAbsolutePower As Double()
   Private semLowerOffsetMarginRelativePower As Double(), semUpperOffsetMargin As Double()
   Private semUpperOffsetMarginFrequency As Double(), semUpperOffsetMarginAbsolutePower As Double()
   Private semUpperOffsetMarginRelativePower As Double()

   Private modAccPeakCompositeEvmSlotIndex As Integer, modAccPeakCompositeEvmSymbolIndex As Integer
   Private modAccPeakCompositeEvmSubcarrierIndex As Integer

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
         Console.WriteLine("Press any key to exit.....")
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

      uplinkDownlinkConfiguration = RFmxLteMXUplinkDownlinkConfiguration.Configuration0
      duplexScheme = RFmxLteMXDuplexScheme.Fdd

      band = 1

      componentCarrierBandwidth = 10000000.0
      ' Hz
      componentCarrierFrequency = 0.0
      ' Hz
      cellID = 0

      synchronizationMode = RFmxLteMXModAccSynchronizationMode.Slot
      measurementOffset = 0
      measurementLength = 1

      uplinkMaskType = RFmxLteMXSemUplinkMaskType.General_NS01

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

      lte.ComponentCarrier.Configure("", componentCarrierBandwidth, componentCarrierFrequency, cellID)

      lte.ConfigureBand("", band)

      lte.ConfigureDuplexScheme("", duplexScheme, uplinkDownlinkConfiguration)

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

      lte.Sem.Results.ComponentCarrier.FetchMeasurement("", timeout, semAbsoluteIntegratedPower,
                                                        semRelativeIntegratedPower)

      lte.Sem.Results.FetchMeasurementStatus("", timeout, semMeasurementStatus)

      lte.Obw.Results.FetchMeasurement("", timeout, obwOccupiedBandwidth, obwAbsolutePower, obwStartFrequency,
                                       obwStopFrequency)

      lte.Chp.Results.ComponentCarrier.FetchMeasurement("", timeout, chpAbsolutePower, chpRelativePower)

      lte.Acp.Results.FetchOffsetMeasurementArray("", timeout, acpLowerRelativePower, acpUpperRelativePower,
                                                  acpLowerAbsolutePower, acpUpperAbsolutePower)

      lte.Acp.Results.ComponentCarrier.FetchMeasurement("", timeout, acpAbsolutePower, acpRelativePower)

      lte.ModAcc.Results.FetchCompositeEvm("", timeout, modAccMeanRmsCompositeEvm, modAccMaxPeakCompositeEvm,
                                           modAccMeanFrequencyError, modAccPeakCompositeEvmSymbolIndex,
       modAccPeakCompositeEvmSubcarrierIndex, modAccPeakCompositeEvmSlotIndex)

      lte.ModAcc.Results.FetchIQImpairments("", timeout, modAccMeanIQOriginOffset, modAccMeanIQGainImbalance,
                                            modAccMeanIQQuadratureError)
   End Sub

   Private Sub PrintResults()
      Console.WriteLine("************************* ModAcc *************************" & vbLf)
      Console.WriteLine("Mean RMS Composite EVM  (%)             : {0}", modAccMeanRmsCompositeEvm)
      Console.WriteLine("Max Peak Composite EVM  (% )            : {0}", modAccMaxPeakCompositeEvm)
      Console.WriteLine("Mean FrequencyError     (Hz)            : {0}", modAccMeanFrequencyError)
      Console.WriteLine("Mean IQ Origin Offset   (dBc)           : {0}", modAccMeanIQOriginOffset)

      Console.WriteLine(vbLf & "************************* CHP *************************" & vbLf)
      Console.WriteLine("Carrier Absolute Power  (dBm)           : {0}", chpAbsolutePower)

      Console.WriteLine(vbLf & "************************* ACP *************************" & vbLf)
      Console.WriteLine("Carrier Absolute Power  (dBm)           : {0}", acpAbsolutePower)
      Console.WriteLine(vbLf & "------- Offset Channel Measurements -------")
      For i As Integer = 0 To acpLowerRelativePower.Length - 1
         Console.WriteLine(vbLf & "Offset  : {0}", i)
         Console.WriteLine("Lower Relative Power (dB)               : {0}", acpLowerRelativePower(i))
         Console.WriteLine("Upper Relative Power (dB)               : {0}", acpUpperRelativePower(i))
         Console.WriteLine("Lower Absolute Power (dBm)              : {0}", acpLowerAbsolutePower(i))
         Console.WriteLine("Upper Absolute Power (dBm)              : {0}", acpUpperAbsolutePower(i))
      Next

      Console.WriteLine(vbLf & "************************* OBW *************************" & vbLf)
      Console.WriteLine("Occupied Bandwidth  (Hz)                : {0}", obwOccupiedBandwidth)
      Console.WriteLine("Absolute Power      (dBm)               : {0}", obwAbsolutePower)
      Console.WriteLine("Start Frequency     (Hz)                : {0}", obwStartFrequency)
      Console.WriteLine("Stop Frequency      (Hz)                : {0}", obwStopFrequency)

      Console.WriteLine(vbLf & "************************* SEM *************************" & vbLf)
      Console.WriteLine("Measurement Status                      : {0}", semMeasurementStatus)
      Console.WriteLine("Carrier Absolute Integrated Power  (dBm): {0}", semAbsoluteIntegratedPower)
      Console.WriteLine(vbLf & "----- Lower Offset Segment Measurements -----")
      For i As Integer = 0 To semLowerOffsetMargin.Length - 1
         Console.WriteLine(vbLf & "Offset  : {0}", i)
         Console.WriteLine("Measurement Status                      : {0}", semLowerOffsetMeasurementStatus(i))
         Console.WriteLine("Margin                 (dB)             : {0}", semLowerOffsetMargin(i))
         Console.WriteLine("Margin Frequency       (Hz)             : {0}", semLowerOffsetMarginFrequency(i))

         Console.WriteLine("Margin Absolute Power  (dBm)            : {0}", semLowerOffsetMarginAbsolutePower(i))
      Next
      Console.WriteLine(vbLf & "----- Upper Offset Segment Measurements -----")
      For i As Integer = 0 To semUpperOffsetMargin.Length - 1
         Console.WriteLine(vbLf & "Offset  : {0}", i)
         Console.WriteLine("Measurement Status                      : {0}", semUpperOffsetMeasurementStatus(i))
         Console.WriteLine("Margin                 (dB)             : {0}", semUpperOffsetMargin(i))
         Console.WriteLine("Margin Frequency       (Hz)             : {0}", semUpperOffsetMarginFrequency(i))
         Console.WriteLine("Margin Absolute Power  (dBm)            : {0}", semUpperOffsetMarginAbsolutePower(i))
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
