'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
'4. Configure Trigger Type and Trigger Parameters.
'5. Configure Component Carrier Spacing.
'6. Configure operating Band to 46.
'7. Configure Duplex Mode to LAA.
'8. Configure Component Carriers settings.
'9. Configure Auto DMRS Detection Enabled.
'10. Select ModAcc measurement And enable Traces.
'11. Configure Synchronization Mode And Measurement Interval.
'12. Configure EVM Unit.
'13. Configure Averaging Parameters for ModAcc measurement.
'14. Initiate the Measurement.
'15. Fetch ModAcc Measurements And Traces.
'16. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.LteMX

Public Class RFmxLteULLaaModAccMultiCarrier
   Private instrSession As RFmxInstrMX
   Private lte As RFmxLteMX
   Private resourceName As String

   Private frequencyReferenceSource As String
   Private frequencyReferenceFrequency As Double

   Private centerFrequency As Double
   Private referenceLevel As Double
   Private externalAttenuation As Double

   Private iqPowerEdgeTriggerSource As String
   Private enableTrigger As Boolean
   Private iqPowerEdgeTriggerLevel As Double
   Private triggerDelay As Double
   Private minimumQuietTimeDuration As Double
   Private minimumQuietTimeMode As RFmxLteMXTriggerMinimumQuietTimeMode
   Private iqPowerEdgeTriggerLevelType As RFmxLteMXIQPowerEdgeTriggerLevelType
   Private iqPowerEdgeTriggerSlope As RFmxLteMXIQPowerEdgeTriggerSlope

   Const numberOfComponentCarriers As Integer = 2
   Private componentCarrierBandwidth As Double()
   Private componentCarrierFrequency As Double()
   Private componentCarrierCellId As Integer()
   Private laaNumberOfSubframes As Integer()
   Private laaStartingSubframe As Integer()
   Private laaUplinkStartPosition As RFmxLteMXLaaUplinkStartPosition()
   Private laaUplinkEndingSymbol As RFmxLteMXLaaUplinkEndingSymbol()

   Private averagingEnabled As RFmxLteMXModAccAveragingEnabled
   Private averagingCount As Integer

   Private timeout As Double

   Private componentSpacingType As RFmxLteMXComponentCarrierSpacingType
   Private componentCarrierAtCenterFrequency As Integer

   Private synchronizationMode As RFmxLteMXModAccSynchronizationMode
   Private uplinkDownlinkConfiguration As RFmxLteMXUplinkDownlinkConfiguration
   Private measurementOffset As Integer
   Private measurementLength As Integer

   Private evmUnit As RFmxLteMXModAccEvmUnit

   Private autoDmrsDetectionEnabled As RFmxLteMXAutoDmrsDetectionEnabled

   Private subblockCarrierString As String

   Private meanRmsCompositeEvm As Double()
   Private maximumPeakCompositeEvm As Double()
   Private meanFrequencyError As Double()
   Private peakCompositeEvmSlotIndex As Integer()
   Private peakCompositeEvmSymbolIndex As Integer()
   Private peakCompositeEvmSubcarrierIndex As Integer()

   Private meanIQOriginOffset As Double()
   Private meanIQGainImbalance As Double()
   Private meanIQQuadratureError As Double()
   Private inBandEmissionMargin As Double()

   Private dataConstellation As ComplexSingle()(), dmrsDataConstellation As ComplexSingle()()

   Private meanRmsEvmPerSubcarrier As AnalogWaveform(Of Single)()

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
         ' Close session

         CloseSession()
         Console.WriteLine("Press any key to exit")
         Console.ReadKey()
      End Try
   End Sub


   Private Sub InitializeVariables()
      resourceName = "RFSA"

      frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
      frequencyReferenceFrequency = 10000000.0
      ' (Hz)

      centerFrequency = 1950000000.0
      ' (Hz)
      referenceLevel = 0.0
      ' (dBm)
      externalAttenuation = 0.0
      ' (dBm)

      iqPowerEdgeTriggerSource = "0"
      enableTrigger = True
      iqPowerEdgeTriggerLevel = -20.0
      '/* (dB) */
      triggerDelay = 0.0
      '/* (s) */
      minimumQuietTimeDuration = 0.00005
      '/* (s) */
      minimumQuietTimeMode = RFmxLteMXTriggerMinimumQuietTimeMode.Auto
      iqPowerEdgeTriggerLevelType = RFmxLteMXIQPowerEdgeTriggerLevelType.Relative
      iqPowerEdgeTriggerSlope = RFmxLteMXIQPowerEdgeTriggerSlope.Rising

      componentCarrierBandwidth = New Double() {20000000.0, 20000000.0}
      componentCarrierFrequency = New Double() {-9900000.0, 9900000.0}
      componentCarrierCellId = New Integer() {0, 0}
      laaNumberOfSubframes = New Integer() {1, 1}
      laaStartingSubframe = New Integer() {0, 0}
      laaUplinkStartPosition = New RFmxLteMXLaaUplinkStartPosition() {RFmxLteMXLaaUplinkStartPosition.StartPosition00,
                                                                  RFmxLteMXLaaUplinkStartPosition.StartPosition00}
      laaUplinkEndingSymbol = New RFmxLteMXLaaUplinkEndingSymbol() {RFmxLteMXLaaUplinkEndingSymbol.EndingSymbol13,
                                                               RFmxLteMXLaaUplinkEndingSymbol.EndingSymbol13}

      componentSpacingType = RFmxLteMXComponentCarrierSpacingType.Nominal
      componentCarrierAtCenterFrequency = -1

      uplinkDownlinkConfiguration = RFmxLteMXUplinkDownlinkConfiguration.Configuration0

      dataConstellation = New ComplexSingle(numberOfComponentCarriers - 1)() {}
      dmrsDataConstellation = New ComplexSingle(numberOfComponentCarriers - 1)() {}

      meanRmsEvmPerSubcarrier = New AnalogWaveform(Of Single)(numberOfComponentCarriers - 1) {}

      averagingEnabled = RFmxLteMXModAccAveragingEnabled.[False]
      averagingCount = 10

      synchronizationMode = RFmxLteMXModAccSynchronizationMode.Slot
      measurementOffset = 0
      measurementLength = 1

      evmUnit = RFmxLteMXModAccEvmUnit.Percentage

      autoDmrsDetectionEnabled = RFmxLteMXAutoDmrsDetectionEnabled.True

      timeout = 10.0
      ' (s)
   End Sub

   Private Sub ConfigureLte()
      lte = instrSession.GetLteSignalConfiguration()
      ' Create a new RFmx Session
      instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)

      lte.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
      lte.ConfigureIQPowerEdgeTrigger("", iqPowerEdgeTriggerSource, iqPowerEdgeTriggerSlope, iqPowerEdgeTriggerLevel,
         triggerDelay, minimumQuietTimeMode, minimumQuietTimeDuration, iqPowerEdgeTriggerLevelType, enableTrigger)
      lte.ComponentCarrier.ConfigureSpacing("", componentSpacingType, componentCarrierAtCenterFrequency)
      lte.ConfigureBand("", 46)
      lte.ConfigureDuplexScheme("", RFmxLteMXDuplexScheme.Laa, uplinkDownlinkConfiguration)
      lte.ConfigureNumberOfComponentCarriers("", numberOfComponentCarriers)
      For i As Integer = 0 To numberOfComponentCarriers - 1
         subblockCarrierString = RFmxLteMX.BuildCarrierString("", i)
         lte.ComponentCarrier.Configure(subblockCarrierString, componentCarrierBandwidth(i),
            componentCarrierFrequency(i), componentCarrierCellId(i))
         lte.ComponentCarrier.SetLaaStartingSubframe(subblockCarrierString, laaStartingSubframe(i))
         lte.ComponentCarrier.SetLaaNumberOfSubframes(subblockCarrierString, laaNumberOfSubframes(i))
         lte.ComponentCarrier.SetLaaUplinkStartPosition(subblockCarrierString, laaUplinkStartPosition(i))
         lte.ComponentCarrier.SetLaaUplinkEndingSymbol(subblockCarrierString, laaUplinkEndingSymbol(i))
      Next
      lte.ConfigureAutoDmrsDetectionEnabled("", autoDmrsDetectionEnabled)
      lte.SelectMeasurements("", RFmxLteMXMeasurementTypes.ModAcc, True)
      lte.ModAcc.Configuration.ConfigureSynchronizationModeAndInterval("", synchronizationMode, measurementOffset,
                                                                       measurementLength)
      lte.ModAcc.Configuration.ConfigureEvmUnit("", evmUnit)
      lte.ModAcc.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount)
      lte.Initiate("", "")
   End Sub

   Private Sub RetrieveResults()
      lte.ModAcc.Results.FetchCompositeEvmArray("", timeout, meanRmsCompositeEvm, maximumPeakCompositeEvm,
                                                meanFrequencyError, peakCompositeEvmSymbolIndex,
                                                peakCompositeEvmSubcarrierIndex, peakCompositeEvmSlotIndex)
      lte.ModAcc.Results.FetchIQImpairmentsArray("", timeout, meanIQOriginOffset, meanIQGainImbalance,
                                                 meanIQQuadratureError)
      lte.ModAcc.Results.FetchInBandEmissionMarginArray("", timeout, inBandEmissionMargin)
      For i As Integer = 0 To numberOfComponentCarriers - 1
         subblockCarrierString = RFmxLteMX.BuildCarrierString("", i)
         lte.ModAcc.Results.FetchPuschConstellationTrace(subblockCarrierString, timeout,
                                                         dataConstellation(i), dmrsDataConstellation(i))
         lte.ModAcc.Results.FetchEvmPerSubcarrierTrace(subblockCarrierString, timeout, meanRmsEvmPerSubcarrier(i))
      Next
   End Sub

   Private Sub PrintResults()
      Console.WriteLine("------------------------Measurements------------------------" & vbLf)
      For i As Integer = 0 To numberOfComponentCarriers - 1
         Console.WriteLine("Carrier  : {0}" & vbLf, i)
         Console.WriteLine("Mean RMS Composite EVM  (% or dB)    : {0}", meanRmsCompositeEvm(i))
         Console.WriteLine("Maximum Peak Composite EVM  (% or dB): {0}", maximumPeakCompositeEvm(i))
         Console.WriteLine("Peak Composite EVM Slot Index        : {0}", peakCompositeEvmSlotIndex(i))
         Console.WriteLine("Peak Composite EVM Symbol Index      : {0}", peakCompositeEvmSymbolIndex(i))
         Console.WriteLine("Peak Composite EVM Subcarrier Index  : {0}", peakCompositeEvmSubcarrierIndex(i))
         Console.WriteLine("Mean Frequency Error  (Hz)           : {0}", meanFrequencyError(i))
         Console.WriteLine("Mean IQ Origin Offset  (dBc)         : {0}", meanIQOriginOffset(i))
         Console.WriteLine("Mean IQ Gain Imbalance  (dB)         : {0}", meanIQGainImbalance(i))
         Console.WriteLine("Mean IQ Quadrature Error  (deg)      : {0}", meanIQQuadratureError(i))
         Console.WriteLine("In Band Emission Margin  (dB)        : {0}", inBandEmissionMargin(i))
         Console.WriteLine("-------------------------------------------------" & vbLf)
      Next
   End Sub

   Private Sub InitializeInstr()
      instrSession = New RFmxInstrMX(resourceName, "")
   End Sub

   Private Shared Sub DisplayError(ex As Exception)
      Console.WriteLine("ERROR:" & vbLf & Convert.ToString(ex.[GetType]()) & ": " & ex.Message)
   End Sub

End Class
