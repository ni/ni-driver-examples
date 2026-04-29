'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure basic signal properties (Reference Level and External Attenuation).
'4. Configure Trigger Type and Trigger Parameters.
'5. Configure Duplex Mode.
'6. Configure Link Direction as Downlink.
'7[A-F]. Configure Subblock Parameters.
'7A. Configure Number of Subblocks.
'7B. Configure subblock Frequency.
'7C. Configure Component Carrier Spacing.
'7D. Configure Band.
'7E. Configure Number of Component Carriers.
'7F. Configure Component Carriers (Component Carrier Frequency and Component Carrier Bandwidth).
'8. Configure Downlink Test Model.
'9. Select ModAcc measurement and enable Traces.
'10. Configure Averaging Parameters for ModAcc measurement.
'11. Select Frame as Synchronization Mode and configure Measurement Interval.
'12. Configure EVM Unit.
'13. Initiate the Measurement
'14[A-F]. Fetch ModAcc Measurements and Traces
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
   Public downlinkTestModel As RFmxLteMXDownlinkTestModel()
End Structure

' Input: Subblock measurement outputs structure

Structure SubblockMeasurement
   Public meanRmsCompositeEvm As Double()
   '(dBm)
   Public maximumPeakCompositeEvm As Double()
   '(dBm)
   Public peakCompositeEvmSlotIndex As Integer()
   '(dB)
   Public peakCompositeEvmSymbolIndex As Integer()
   Public peakCompositeEvmSubcarrierIndex As Integer()
   Public meanRmsEvm As Double()
   '(% or dB)
   Public meanRmsQpskEvm As Double()
   '(% or dB)
   Public meanRms16QamEvm As Double()
   '(% or dB)
   Public meanRms64QamEvm As Double()
   '(% or dB)
   Public meanRms256QamEvm As Double()
   '(% or dB)
   Public meanRms1024QamEvm As Double()
   '(% or dB)
   Public meanFrequencyError As Double()
   '(Hz)
   Public meanIQOriginOffset As Double()
   Public meanIQGainImbalance As Double()
   Public meanIQQuadratureError As Double()
   Public qpskConstellation As ComplexSingle()()
   Public meanRmsEvmPerSubcarrier As AnalogWaveform(Of Single)()
End Structure

Public Class RFmxLteDLModAccNonContiguousMultiCarrier
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
   Private digitalEdgeTriggerSource As String
   Private digitalEdgeTriggerEdge As RFmxLteMXDigitalEdgeTriggerEdge
   Private triggerDelay As Double

   Private averagingEnabled As RFmxLteMXModAccAveragingEnabled
   Private averagingCount As Integer

   Private timeout As Double

   Private subblockString As String()

   Private subblocks As SubblockInput()
   Private subblocksMsr As SubblockMeasurement()

   Private linkDirection As RFmxLteMXLinkDirection
   Private duplexScheme As RFmxLteMXDuplexScheme
   Private uplinkDownlinkConfiguration As RFmxLteMXUplinkDownlinkConfiguration
   Private synchronizationMode As RFmxLteMXModAccSynchronizationMode
   Private measurementOffset As Integer
   Private measurementLength As Integer
   Private evmUnit As RFmxLteMXModAccEvmUnit
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

      centerFrequency = 2140000000.0
      ' (Hz) 
      referenceLevel = 0.0
      ' (dBm)
      externalAttenuation = 0.0
      ' (dBm)

      enableTrigger = False
      digitalEdgeTriggerSource = RFmxLteMXConstants.Pfi0
      digitalEdgeTriggerEdge = RFmxLteMXDigitalEdgeTriggerEdge.Rising
      triggerDelay = 0.0
      ' (s)

      averagingEnabled = RFmxLteMXModAccAveragingEnabled.[False]
      averagingCount = 10

      timeout = 10.0
      ' (s)

      linkDirection = RFmxLteMXLinkDirection.Downlink
      uplinkDownlinkConfiguration = RFmxLteMXUplinkDownlinkConfiguration.Configuration0
      duplexScheme = RFmxLteMXDuplexScheme.Fdd

      synchronizationMode = RFmxLteMXModAccSynchronizationMode.Frame
      measurementOffset = 0
      measurementLength = 1

      evmUnit = RFmxLteMXModAccEvmUnit.Percentage

        subblocks = New SubblockInput(numberOfSubblocks - 1) {New SubblockInput() With {
        .subblockFrequency = 0.0,
        .componentCarrierSpacingType = RFmxLteMXComponentCarrierSpacingType.Nominal,
        .componentCarrierAtCenterFrequency = -1,
        .componentCarrierBandwidth = New Double(numberOfComponentCarriers - 1) {20000000.0},
        .componentCarrierFrequency = New Double(numberOfComponentCarriers - 1) {0.0},
        .componentCarrierCellId = New Integer(numberOfComponentCarriers - 1) {0},
        .band = 1,
        .downlinkTestModel = New RFmxLteMXDownlinkTestModel(numberOfComponentCarriers - 1) {RFmxLteMXDownlinkTestModel.TM1_1}
      }, New SubblockInput() With {
        .subblockFrequency = 30000000.0,
        .componentCarrierSpacingType = RFmxLteMXComponentCarrierSpacingType.Nominal,
        .componentCarrierAtCenterFrequency = -1,
        .componentCarrierBandwidth = New Double(numberOfComponentCarriers - 1) {20000000.0},
        .componentCarrierFrequency = New Double(numberOfComponentCarriers - 1) {0.0},
        .componentCarrierCellId = New Integer(numberOfComponentCarriers - 1) {0},
        .band = 1,
        .downlinkTestModel = New RFmxLteMXDownlinkTestModel(numberOfComponentCarriers - 1) {RFmxLteMXDownlinkTestModel.TM1_1}
      }}

        subblocksMsr = New SubblockMeasurement(numberOfSubblocks - 1) {}
   End Sub

   Private Sub ConfigureLte()
      ' Create a new RFmx Session

      lte = instrSession.GetLteSignalConfiguration()
      instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)

      lte.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
      lte.ConfigureDigitalEdgeTrigger("", digitalEdgeTriggerSource, digitalEdgeTriggerEdge, triggerDelay, enableTrigger)
      lte.ConfigureDuplexScheme("", duplexScheme, uplinkDownlinkConfiguration)
      lte.ConfigureLinkDirection("", linkDirection)
      lte.ConfigureNumberOfSubblocks("", numberOfSubblocks)

      subblockString = New String(numberOfSubblocks - 1) {}
      For i As Integer = 0 To numberOfSubblocks - 1
         subblockString(i) = RFmxLteMX.BuildSubblockString("", i)
         lte.SetSubblockFrequency(subblockString(i), subblocks(i).subblockFrequency)
         lte.ComponentCarrier.ConfigureSpacing(subblockString(i), subblocks(i).componentCarrierSpacingType, subblocks(i).componentCarrierAtCenterFrequency)
         lte.ConfigureBand(subblockString(i), subblocks(i).band)
         lte.ConfigureNumberOfComponentCarriers(subblockString(i), numberOfComponentCarriers)
         lte.ComponentCarrier.ConfigureArray(subblockString(i), subblocks(i).componentCarrierBandwidth, subblocks(i).componentCarrierFrequency, subblocks(i).componentCarrierCellId)
         lte.ComponentCarrier.ConfigureDownlinkTestModelArray(subblockString(i), subblocks(i).downlinkTestModel)
      Next

      lte.SelectMeasurements("", RFmxLteMXMeasurementTypes.ModAcc, True)
      lte.ModAcc.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount)
      lte.ModAcc.Configuration.ConfigureSynchronizationModeAndInterval("", synchronizationMode, measurementOffset, measurementLength)
      lte.ModAcc.Configuration.ConfigureEvmUnit("", evmUnit)
      lte.Initiate("", "")
   End Sub

   Private Sub RetrieveResults()
      subblocksMsr = New SubblockMeasurement(numberOfSubblocks - 1) {}
      For i As Integer = 0 To numberOfSubblocks - 1
         subblocksMsr(i).qpskConstellation = New ComplexSingle(numberOfComponentCarriers - 1)() {}
         subblocksMsr(i).meanRmsEvmPerSubcarrier = New AnalogWaveform(Of Single)(numberOfComponentCarriers - 1) {}
         lte.ModAcc.Results.FetchCompositeEvmArray(subblockString(i), timeout, subblocksMsr(i).meanRmsCompositeEvm, subblocksMsr(i).maximumPeakCompositeEvm, subblocksMsr(i).meanFrequencyError, subblocksMsr(i).peakCompositeEvmSymbolIndex,
          subblocksMsr(i).peakCompositeEvmSubcarrierIndex, subblocksMsr(i).peakCompositeEvmSlotIndex)
         lte.ModAcc.Results.FetchIQImpairmentsArray(subblockString(i), timeout, subblocksMsr(i).meanIQOriginOffset, subblocksMsr(i).meanIQGainImbalance, subblocksMsr(i).meanIQQuadratureError)
         lte.ModAcc.Results.FetchPdschEvmArray(subblockString(i), timeout, subblocksMsr(i).meanRmsEvm, subblocksMsr(i).meanRmsQpskEvm, subblocksMsr(i).meanRms16QamEvm, subblocksMsr(i).meanRms64QamEvm,
             subblocksMsr(i).meanRms256QamEvm)
         lte.ModAcc.Results.FetchPdsch1024QamEvmArray(subblockString(i), timeout, subblocksMsr(i).meanRms1024QamEvm)

         For j As Integer = 0 To numberOfComponentCarriers - 1
            subblockCarrierString = RFmxLteMX.BuildCarrierString(subblockString(i), j)
            lte.ModAcc.Results.FetchPdschQpskConstellation(subblockCarrierString, timeout, subblocksMsr(i).qpskConstellation(j))
            lte.ModAcc.Results.FetchEvmPerSubcarrierTrace(subblockCarrierString, timeout, subblocksMsr(i).meanRmsEvmPerSubcarrier(j))

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
            Console.WriteLine("Mean Rms Evm  (%  or dB)                : {0}", subblocksMsr(i).meanRmsEvm(j))
            Console.WriteLine("Mean Rms Qpsk Evm  (%  or dB)           : {0}", subblocksMsr(i).meanRmsQpskEvm(j))
            Console.WriteLine("Mean Rms 16Qam Evm  (%  or dB)          : {0}", subblocksMsr(i).meanRms16QamEvm(j))
            Console.WriteLine("Mean Rms 64Qam Evm  (%  or dB)          : {0}", subblocksMsr(i).meanRms64QamEvm(j))
            Console.WriteLine("Mean Rms 256Qam Evm  (%  or dB)         : {0}", subblocksMsr(i).meanRms256QamEvm(j))
            Console.WriteLine("Mean Rms 1024Qam Evm  (%  or dB)        : {0}", subblocksMsr(i).meanRms1024QamEvm(j))
            Console.WriteLine("Mean Frequency Error  (Hz)              : {0}", subblocksMsr(i).meanFrequencyError(j))
            Console.WriteLine("Mean IQ Origin Offset  (dBc)            : {0}", subblocksMsr(i).meanIQOriginOffset(j))
            Console.WriteLine("Mean IQ Gain Imbalance  (dB)            : {0}", subblocksMsr(i).meanIQGainImbalance(j))
            Console.WriteLine("Mean IQ Quadrature Error  (deg)         : {0}", subblocksMsr(i).meanIQQuadratureError(j))
         Next
      Next
   End Sub

   Private Shared Sub DisplayError(ex As Exception)
      Console.WriteLine("ERROR:" & vbLf & Convert.ToString(ex.[GetType]()) & ": " & ex.Message)
   End Sub

End Class
