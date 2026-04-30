'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure basic signal properties (Reference Level and External Attenuation).
'4. Configure Trigger Type and Trigger Parameters.
'5. Configure Duplex Mode.
'6[A-F]. Configure Subblock Parameters.
'6A. Configure Number of Subblocks.
'6B. Configure subblock Frequency.
'6C. Configure Component Carrier Spacing.
'6D. Configure Band.
'6E. Configure Number of Component Carriers.
'6F. Configure Component Carriers (Component Carrier Frequency and Component Carrier Bandwidth).
'7. Configure Auto DMRS Detection Enabled.
'8. Select ModAcc measurement And enable Traces.
'9. Configure Synchronization Mode And Measurement Interval.
'10. Configure EVM Unit.
'11. Configure In-Band Emission Mask Type.
'12. Configure Averaging Parameters for ModAcc measurement.
'13. Initiate the Measurement.
'14. Fetch ModAcc Measurements And Traces
'15. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.LteMX

' Input: Subblock inputs structure

Structure SubblockInput
   Public subblockFrequency As Double
   '(Hz)
   Public componentCarrierSpacingType As RFmxLteMXComponentCarrierSpacingType
   Public componentCarrierAtCenterFrequency As Integer
   Public band As Integer
   Public componentCarrierBandwidth As Double()
   '(Hz)
   Public componentCarrierFrequency As Double()
   '(Hz)
   Public componentCarrierCellId As Integer()
   '(Hz)
End Structure

' Input: Subblock measurement outputs structure

Structure SubblockMeasurement
   Public meanRmsCompositeEvm As Double()
   ' (dBm)
   Public maximumPeakCompositeEvm As Double()
   ' (dBm)
   Public meanFrequencyError As Double()
   ' (dB)
   Public peakCompositeEvmSlotIndex As Integer()
   ' (dB)
   Public peakCompositeEvmSymbolIndex As Integer()
   Public peakCompositeEvmSubcarrierIndex As Integer()
   Public meanIQOriginOffset As Double()
   Public meanIQGainImbalance As Double()
   Public meanIQQuadratureError As Double()
   Public inBandEmissionMargin As Double()
   Public dataConstellationTraces As ComplexSingle()()
   Public dmrsDataConstellationTraces As ComplexSingle()()
   Public meanRmsEvmPerSubcarrier As AnalogWaveform(Of Single)()
End Structure

Public Class RFmxLteULModAccNonContiguousMultiCarrier
   Private instrSession As RFmxInstrMX
   Private lte As RFmxLteMX
   Private uplinkDownlinkConfiguration As RFmxLteMXUplinkDownlinkConfiguration

   Private rfsaResourceName As String

   Private Const NumberOfComponentCarriers As Integer = 1
   Private Const NumberOfSubblocks As Integer = 2

   Private frequencyReferenceSource As String
   Private frequencyReferenceFrequency As Double

   Private centerFrequency As Double
   Private referenceLevel As Double
   Private externalAttenuation As Double

   Private enableTrigger As Boolean
   Private digitalEdgeSource As String
   Private digitalEdge As RFmxLteMXDigitalEdgeTriggerEdge
   Private triggerDelay As Double

   Private componentCarrierBandwidth As Double()
   Private componentCarrierFrequency As Double()

   Private averagingEnabled As RFmxLteMXModAccAveragingEnabled
   Private averagingCount As Integer

   Private autoDmrsDetectionEnabled As RFmxLteMXAutoDmrsDetectionEnabled

   Private timeout As Double

   Private subblockString As String()

   Private subblocks As SubblockInput()
   Private subblocksMsr As SubblockMeasurement()

   Private duplexScheme As RFmxLteMXDuplexScheme
   Private componentCarrierCellId As Integer()
   Private synchronizationMode As RFmxLteMXModAccSynchronizationMode
   Private measurementOffset As Integer
   Private measurementLength As Integer
   Private evmUnit As RFmxLteMXModAccEvmUnit
   Private inBandEmissionMaskType As RFmxLteMXModAccInBandEmissionMaskType
   Private subblockCarrierString As String

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

   Private Sub InitializeInstr()
      instrSession = New RFmxInstrMX(rfsaResourceName, "")
   End Sub

   Private Sub InitializeVariables()
      rfsaResourceName = "RFSA"

      frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
      frequencyReferenceFrequency = 10000000.0
      ' (Hz)

      centerFrequency = 1950000000.0
      ' (Hz) 
      referenceLevel = 0.0
      ' (dBm)
      externalAttenuation = 0.0
      ' (dBm)

      enableTrigger = False
      digitalEdgeSource = RFmxLteMXConstants.Pfi0
      digitalEdge = RFmxLteMXDigitalEdgeTriggerEdge.Rising
      triggerDelay = 0.0
      ' (s)

      averagingEnabled = RFmxLteMXModAccAveragingEnabled.[False]
      averagingCount = 10

      timeout = 10.0
      ' (s)

      uplinkDownlinkConfiguration = RFmxLteMXUplinkDownlinkConfiguration.Configuration0
      duplexScheme = RFmxLteMXDuplexScheme.Fdd

      synchronizationMode = RFmxLteMXModAccSynchronizationMode.Slot
      measurementOffset = 0
      measurementLength = 1

      evmUnit = RFmxLteMXModAccEvmUnit.Percentage

      inBandEmissionMaskType = RFmxLteMXModAccInBandEmissionMaskType.Release11Onwards

      autoDmrsDetectionEnabled = RFmxLteMXAutoDmrsDetectionEnabled.True

      subblocksMsr = New SubblockMeasurement(NumberOfSubblocks - 1) {}
        subblocks = New SubblockInput() {
         New SubblockInput() With {
                                    .subblockFrequency = 0.0,
                                    .componentCarrierSpacingType = RFmxLteMXComponentCarrierSpacingType.Nominal,
                                    .componentCarrierAtCenterFrequency = -1,
                                    .band = 1,
                                    .componentCarrierBandwidth = New Double(NumberOfComponentCarriers - 1) {20000000.0},
                                    .componentCarrierFrequency = New Double(NumberOfComponentCarriers - 1) {0.0},
                                    .componentCarrierCellId = New Integer(NumberOfComponentCarriers - 1) {0}
                                    },
         New SubblockInput() With {
                                    .subblockFrequency = 30000000.0,
                                    .componentCarrierSpacingType = RFmxLteMXComponentCarrierSpacingType.Nominal,
                                    .componentCarrierAtCenterFrequency = -1,
                                    .band = 1,
                                    .componentCarrierBandwidth = New Double(NumberOfComponentCarriers - 1) {20000000.0},
                                    .componentCarrierFrequency = New Double(NumberOfComponentCarriers - 1) {0.0},
                                    .componentCarrierCellId = New Integer(NumberOfComponentCarriers - 1) {0}
                                 }
                                      }
    End Sub

   Private Sub ConfigureLte()
      lte = instrSession.GetLteSignalConfiguration()
      ' Create a new RFmx Session
      instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)

      lte.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
      lte.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger)
      lte.ConfigureDuplexScheme("", duplexScheme, uplinkDownlinkConfiguration)
      lte.ConfigureNumberOfSubblocks("", NumberOfSubblocks)

      subblockString = New String(NumberOfSubblocks - 1) {}
      For i As Integer = 0 To NumberOfSubblocks - 1
         subblockString(i) = RFmxLteMX.BuildSubblockString("", i)
         lte.SetSubblockFrequency(subblockString(i), subblocks(i).subblockFrequency)
         lte.ComponentCarrier.ConfigureSpacing(subblockString(i), subblocks(i).componentCarrierSpacingType,
                                               subblocks(i).componentCarrierAtCenterFrequency)
         lte.ConfigureBand(subblockString(i), subblocks(i).band)
         lte.ConfigureNumberOfComponentCarriers(subblockString(i), NumberOfComponentCarriers)
         lte.ComponentCarrier.ConfigureArray(subblockString(i), subblocks(i).componentCarrierBandwidth,
                                             subblocks(i).componentCarrierFrequency,
                                             subblocks(i).componentCarrierCellId)
      Next
      lte.ConfigureAutoDmrsDetectionEnabled("", autoDmrsDetectionEnabled)

      lte.SelectMeasurements("", RFmxLteMXMeasurementTypes.ModAcc, True)
      lte.ModAcc.Configuration.ConfigureSynchronizationModeAndInterval("", synchronizationMode, measurementOffset,
                                                                       measurementLength)
      lte.ModAcc.Configuration.ConfigureEvmUnit("", evmUnit)
      lte.ModAcc.Configuration.ConfigureInBandEmissionMaskType("", inBandEmissionMaskType)
      lte.ModAcc.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount)
      lte.Initiate("", "")
   End Sub

   Private Sub RetrieveResults()
      subblocksMsr = New SubblockMeasurement(NumberOfSubblocks - 1) {}
      For i As Integer = 0 To NumberOfSubblocks - 1
         subblocksMsr(i).dataConstellationTraces = New ComplexSingle(NumberOfComponentCarriers - 1)() {}
         subblocksMsr(i).dmrsDataConstellationTraces = New ComplexSingle(NumberOfComponentCarriers - 1)() {}
         subblocksMsr(i).meanRmsEvmPerSubcarrier = New AnalogWaveform(Of Single)(NumberOfComponentCarriers - 1) {}
         lte.ModAcc.Results.FetchCompositeEvmArray(subblockString(i), timeout, subblocksMsr(i).meanRmsCompositeEvm,
                                                   subblocksMsr(i).maximumPeakCompositeEvm,
                                                   subblocksMsr(i).meanFrequencyError,
                                                   subblocksMsr(i).peakCompositeEvmSymbolIndex,
                                                   subblocksMsr(i).peakCompositeEvmSubcarrierIndex,
                                                   subblocksMsr(i).peakCompositeEvmSlotIndex)
         lte.ModAcc.Results.FetchIQImpairmentsArray(subblockString(i), timeout, subblocksMsr(i).meanIQOriginOffset,
                                                    subblocksMsr(i).meanIQGainImbalance,
                                                    subblocksMsr(i).meanIQQuadratureError)
         lte.ModAcc.Results.FetchInBandEmissionMarginArray(subblockString(i), timeout,
                                                           subblocksMsr(i).inBandEmissionMargin)

         For j As Integer = 0 To NumberOfComponentCarriers - 1
            subblockCarrierString = RFmxLteMX.BuildCarrierString(subblockString(i), j)
            lte.ModAcc.Results.FetchEvmPerSubcarrierTrace(subblockCarrierString, timeout,
                                                          subblocksMsr(i).meanRmsEvmPerSubcarrier(j))
            lte.ModAcc.Results.FetchPuschConstellationTrace(subblockCarrierString, timeout,
                                                            subblocksMsr(i).dataConstellationTraces(j),
                                                            subblocksMsr(i).dmrsDataConstellationTraces(j))
         Next
      Next
   End Sub

   Private Sub PrintResults()
      Console.WriteLine("----------------------Measurements--------------------")
      For i As Integer = 0 To NumberOfSubblocks - 1
         Console.WriteLine(vbLf & "Subblock Number {0}", i)

         Console.WriteLine("-----------Component Carrier Measurements----------------", i)
         For j As Integer = 0 To NumberOfComponentCarriers - 1
            Console.WriteLine("Carrier {0}", j)
            Console.WriteLine("Mean Rms Composite Evm  (%  or dB)      : {0}", subblocksMsr(i).meanRmsCompositeEvm(j))
            Console.WriteLine("Max Peak Composite Evm  (%  or dB)      : {0}", subblocksMsr(i).maximumPeakCompositeEvm(j))
            Console.WriteLine("Peak Composite Evm Slot Index           : {0}", subblocksMsr(i).peakCompositeEvmSlotIndex(j))
            Console.WriteLine("Peak Composite Evm Symbol Index         : {0}", subblocksMsr(i).peakCompositeEvmSymbolIndex(j))
            Console.WriteLine("Peak Composite Evm Subcarrier Index     : {0}", subblocksMsr(i).peakCompositeEvmSubcarrierIndex(j))
            Console.WriteLine("Mean Frequency Error  (Hz)              : {0}", subblocksMsr(i).meanFrequencyError(j))
            Console.WriteLine("Mean IQ Origin Offset  (dBc)            : {0}", subblocksMsr(i).meanIQOriginOffset(j))
            Console.WriteLine("Mean IQ Gain Imbalance  (dB)            : {0}", subblocksMsr(i).meanIQGainImbalance(j))
            Console.WriteLine("Mean IQ Quadrature Error  (deg)         : {0}", subblocksMsr(i).meanIQQuadratureError(j))
            Console.WriteLine("In Band Emission Margin  (dB)           : {0}", subblocksMsr(i).inBandEmissionMargin(j))
            Console.WriteLine("-------------------------------------------------" & vbLf)
         Next
      Next
   End Sub

   Private Shared Sub DisplayError(ex As Exception)
      Console.WriteLine("ERROR:" & vbLf & Convert.ToString(ex.[GetType]()) & ": " & ex.Message)
   End Sub

End Class
