'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure basic signal properties (Reference Level and External Attenuation).
'4. Configure Trigger Type and Trigger Parameters.
'5. Configure Number of DUT Antennas.
'6. Configure Transmit Antenna to Analyze.
'7. Configure Duplex Mode.
'8[A-G]. Configure Subblock Parameters.
'8A. Configure Number of Subblocks.
'8B. Configure subblock Frequency.
'8C. Configure Component Carrier Spacing.
'8D. Configure Band.
'8E. Configure Number of Component Carriers.
'8F. Configure Component Carriers (Component Carrier Frequency and Component Carrier Bandwidth).
'8G. Configure DMRS OCC Enabled and Cyclic Shift Field.
'9. Configure Auto DMRS Detection Enabled.
'10. Select ModAcc measurement And enable Traces.
'11. Configure Synchronization Mode And Measurement Interval.
'12. Configure EVM Unit.
'13. Configure In-Band Emission Mask Type.
'14. Configure Averaging Parameters for ModAcc measurement.
'15. Initiate the Measurement.
'16. Fetch ModAcc Measurements And Traces.
'17. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.LteMX

Namespace NationalInstruments.Examples.RFmxLteULModAccMimo
   ' Input: Subblock inputs structure

   Structure SubblockInput
   Public subblockFrequency As Double
   '(Hz)
      Public componentCarrierSpacingType As RFmxLteMXComponentCarrierSpacingType
      Public componentCarrierAtCenterFrequency As Integer
      Public band As Integer
      'Component Carrier Settings
      Public componentCarrierBandwidth As Double()
      '(Hz)
      Public componentCarrierFrequency As Double()
      '(Hz)
      Public componentCarrierCellId As Integer()
   End Structure

   ' Input: Subblock measurement outputs structure

   Structure SubblockMeasurement
      Public meanRmsCompositeEvm As Double()
      '(% or dB)
      Public maximumPeakCompositeEvm As Double()
      '(% or dB)
      Public peakCompositeEvmSlotIndex As Integer()
      Public peakCompositeEvmSymbolIndex As Integer()
      Public peakCompositeEvmSubcarrierIndex As Integer()
      Public meanFrequencyError As Double()
      '(Hz)
      Public meanIQOriginOffset As Double()
      '(dBc)
      Public meanIQGainImbalance As Double()
      '(dB)
      Public meanIQQuadratureError As Double()
      '(deg)
      Public inBandEmissionMargin As Double()
      '(dB)
      Public dataConstellationTraces As ComplexSingle()()
      Public dmrsDataConstellationTraces As ComplexSingle()()
      Public meanRmsEvmPerSubcarrier As AnalogWaveform(Of Single)()
      '(% or dB)
   End Structure

   Public Class RFmxLteULModAccMimo
      Private instrSession As RFmxInstrMX
      Private lte As RFmxLteMX

      Private rfsaResourceName As String

      Const numberOfComponentCarriers As Integer = 1
      Const numberOfSubblocks As Integer = 2

      Private frequencyReferenceSource As String
      Private frequencyReferenceFrequency As Double

      Private centerFrequency As Double
      Private referenceLevel As Double
      Private externalAttenuation As Double

      Private enableTrigger As Boolean
      Private digitalEdgeSource As String
      Private digitalEdge As RFmxLteMXDigitalEdgeTriggerEdge
      Private triggerDelay As Double

      Private duplexScheme As RFmxLteMXDuplexScheme
      Private uplinkDownlinkConfiguration As RFmxLteMXUplinkDownlinkConfiguration
      Private numberOfDUTAntennas As Integer
      Private transmitAntennaToAnalyze As Integer

      ' EVM Unit
      Private evmUnit As RFmxLteMXModAccEvmUnit

      ' InBandEmissionMaskType
      Private inBandEmissionMaskType As RFmxLteMXModAccInBandEmissionMaskType

      ' Measurement Interval
      Private synchronizationMode As RFmxLteMXModAccSynchronizationMode
      Private measurementOffset As Integer
      Private measurementLength As Integer

      ' Averaging
      Private averagingEnabled As RFmxLteMXModAccAveragingEnabled
      Private averagingCount As Integer

      Private autoDmrsDetectionEnabled As RFmxLteMXAutoDmrsDetectionEnabled

      Private timeout As Double
      Private subblockString As String()
      Private subblocks As SubblockInput()
      Private subblocksMsr As SubblockMeasurement()

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

         uplinkDownlinkConfiguration = RFmxLteMXUplinkDownlinkConfiguration.Configuration0
         duplexScheme = RFmxLteMXDuplexScheme.Fdd
         numberOfDUTAntennas = 2
         transmitAntennaToAnalyze = 0

         evmUnit = RFmxLteMXModAccEvmUnit.Percentage

         inBandEmissionMaskType = RFmxLteMXModAccInBandEmissionMaskType.Release11Onwards

         synchronizationMode = RFmxLteMXModAccSynchronizationMode.Slot
         measurementOffset = 0
         measurementLength = 1

         averagingEnabled = RFmxLteMXModAccAveragingEnabled.False
         averagingCount = 10

         autoDmrsDetectionEnabled = RFmxLteMXAutoDmrsDetectionEnabled.True

         timeout = 10.0
            ' (s)

            subblocks = New SubblockInput(numberOfSubblocks - 1) {New SubblockInput() With {
          .subblockFrequency = 0.0,
          .componentCarrierSpacingType = RFmxLteMXComponentCarrierSpacingType.Nominal,
          .componentCarrierAtCenterFrequency = -1,
          .band = 1,
          .componentCarrierBandwidth = New Double(numberOfComponentCarriers - 1) {20000000.0},
          .componentCarrierFrequency = New Double(numberOfComponentCarriers - 1) {0.0},
          .componentCarrierCellId = New Integer(numberOfComponentCarriers - 1) {0}
         }, New SubblockInput() With {
          .subblockFrequency = 30000000.0,
          .componentCarrierSpacingType = RFmxLteMXComponentCarrierSpacingType.Nominal,
          .componentCarrierAtCenterFrequency = -1,
          .band = 1,
          .componentCarrierBandwidth = New Double(numberOfComponentCarriers - 1) {20000000.0},
          .componentCarrierFrequency = New Double(numberOfComponentCarriers - 1) {0.0},
          .componentCarrierCellId = New Integer(numberOfComponentCarriers - 1) {0}
         }}

            subblocksMsr = New SubblockMeasurement(numberOfSubblocks - 1) {}
      End Sub

      Private Sub ConfigureLte()
         lte = instrSession.GetLteSignalConfiguration()
         ' Create a new RFmx Session
         instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)

         lte.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
         lte.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger)
         lte.ConfigureNumberOfDutAntennas("", numberOfDUTAntennas)
         lte.ConfigureTransmitAntennaToAnalyze("", transmitAntennaToAnalyze)
         lte.ConfigureDuplexScheme("", duplexScheme, uplinkDownlinkConfiguration)
         lte.ConfigureNumberOfSubblocks("", numberOfSubblocks)

         subblockString = New String(numberOfSubblocks - 1) {}
         For i As Integer = 0 To numberOfSubblocks - 1
            subblockString(i) = RFmxLteMX.BuildSubblockString("", i)
            lte.SetSubblockFrequency(subblockString(i), subblocks(i).subblockFrequency)
            lte.ComponentCarrier.ConfigureSpacing(subblockString(i), subblocks(i).componentCarrierSpacingType,
                                                  subblocks(i).componentCarrierAtCenterFrequency)
            lte.ConfigureBand(subblockString(i), subblocks(i).band)
            lte.ConfigureNumberOfComponentCarriers(subblockString(i), numberOfComponentCarriers)
            lte.ComponentCarrier.ConfigureArray(subblockString(i), subblocks(i).componentCarrierBandwidth,
                                                subblocks(i).componentCarrierFrequency,
                                                subblocks(i).componentCarrierCellId)
         Next
         lte.ConfigureAutoDmrsDetectionEnabled("", autoDmrsDetectionEnabled)

         lte.SelectMeasurements("", RFmxLteMXMeasurementTypes.ModAcc, True)
         lte.ModAcc.Configuration.ConfigureSynchronizationModeAndInterval("", synchronizationMode,
                                                                          measurementOffset, measurementLength)
         lte.ModAcc.Configuration.ConfigureEvmUnit("", evmUnit)
         lte.ModAcc.Configuration.ConfigureInBandEmissionMaskType("", inBandEmissionMaskType)
         lte.ModAcc.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount)
         lte.Initiate("", "")
      End Sub

      Private Sub RetrieveResults()
         For i As Integer = 0 To numberOfSubblocks - 1
            subblocksMsr(i).dataConstellationTraces = New ComplexSingle(numberOfComponentCarriers - 1)() {}
            subblocksMsr(i).dmrsDataConstellationTraces = New ComplexSingle(numberOfComponentCarriers - 1)() {}
            subblocksMsr(i).meanRmsEvmPerSubcarrier = New AnalogWaveform(Of Single)(numberOfComponentCarriers - 1) {}
            lte.ModAcc.Results.FetchCompositeEvmArray(subblockString(i), timeout, subblocksMsr(i).meanRmsCompositeEvm,
                                                      subblocksMsr(i).maximumPeakCompositeEvm,
                                                      subblocksMsr(i).meanFrequencyError,
                                                      subblocksMsr(i).peakCompositeEvmSymbolIndex,
             subblocksMsr(i).peakCompositeEvmSubcarrierIndex, subblocksMsr(i).peakCompositeEvmSlotIndex)
            lte.ModAcc.Results.FetchIQImpairmentsArray(subblockString(i), timeout,
                                                       subblocksMsr(i).meanIQOriginOffset,
                                                       subblocksMsr(i).meanIQGainImbalance,
                                                       subblocksMsr(i).meanIQQuadratureError)
            lte.ModAcc.Results.FetchInBandEmissionMarginArray(subblockString(i), timeout,
                                                              subblocksMsr(i).inBandEmissionMargin)

            For j As Integer = 0 To numberOfComponentCarriers - 1
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
         For i As Integer = 0 To numberOfSubblocks - 1
            Console.WriteLine(vbLf & "Subblock Number {0}", i)

            Console.WriteLine("-----------Component Carrier Measurements----------------", i)
            For j As Integer = 0 To numberOfComponentCarriers - 1
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
End Namespace
