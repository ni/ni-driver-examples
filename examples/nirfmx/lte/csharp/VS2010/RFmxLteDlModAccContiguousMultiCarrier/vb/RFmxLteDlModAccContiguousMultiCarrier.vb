'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
'4. Configure Trigger Type and Trigger Parameters.
'5. Configure Component Carrier Spacing.
'6. Configure operating Band.
'7. Configure Duplex Mode.
'8. Select Downlink as Link Direction.
'9. Configure Component Carriers.
'10. Configure Downlink Test Model.
'11. Select ModAcc measurement and enable Traces.
'12. Configure Averaging Parameters for ModAcc measurement.
'13. Select Frame as Synchronization Mode and configure Measurement Interval.
'14. Configure EVM Unit.
'15. Initiate the Measurement.
'16. Fetch ModAcc Measurements and Traces.
'17. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.LteMX

Public Class RFmxLteDLModAccContiguousMultiCarrier
   Private instrSession As RFmxInstrMX
   Private lte As RFmxLteMX
   Private rfsaResourceName As String

   Private frequencyReferenceSource As String
   Private frequencyReferenceFrequency As Double

   Private centerFrequency As Double
   Private referenceLevel As Double
   Private externalAttenuation As Double

   Private enableTrigger As Boolean
   Private digitalEdgeTriggerSource As String
   Private digitalEdgeTriggerEdge As RFmxLteMXDigitalEdgeTriggerEdge
   Private triggerDelay As Double

   Const numberOfComponentCarriers As Integer = 2
   Private componentCarrierBandwidth As Double() = {20000000.0, 20000000.0}
   Private componentCarrierFrequency As Double() = {-9900000.0, 9900000.0}
   Private componentCarrierCellId As Integer() = {0, 0}
   Private downlinkTestModel As RFmxLteMXDownlinkTestModel() = {RFmxLteMXDownlinkTestModel.TM1_1, RFmxLteMXDownlinkTestModel.TM1_1}

   Private averagingEnabled As RFmxLteMXModAccAveragingEnabled
   Private averagingCount As Integer

   Private timeout As Double

   Private componentSpacingType As RFmxLteMXComponentCarrierSpacingType
   Private componentCarrierAtCenterFrequency As Integer

   Private band As Integer
   Private duplexScheme As RFmxLteMXDuplexScheme
   Private uplinkDownlinkConfiguration As RFmxLteMXUplinkDownlinkConfiguration
   Private linkDirection As RFmxLteMXLinkDirection

   Private synchronizationMode As RFmxLteMXModAccSynchronizationMode
   Private measurementOffset As Integer
   Private measurementLength As Integer

   Private evmUnit As RFmxLteMXModAccEvmUnit

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

   Private meanRmsEvm As Double()
   Private meanRmsQpskEvm As Double()
   Private meanRms16QamEvm As Double()
   Private meanRms64QamEvm As Double()
   Private meanRms256QamEvm As Double()
   Private meanRms1024QamEvm As Double()

   Private qpskConstellation As ComplexSingle()()
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

      componentSpacingType = RFmxLteMXComponentCarrierSpacingType.Nominal
      componentCarrierAtCenterFrequency = -1

      band = 1
      duplexScheme = RFmxLteMXDuplexScheme.Fdd
      uplinkDownlinkConfiguration = RFmxLteMXUplinkDownlinkConfiguration.Configuration0
      linkDirection = RFmxLteMXLinkDirection.Downlink

      qpskConstellation = New ComplexSingle(numberOfComponentCarriers - 1)() {}

      meanRmsEvmPerSubcarrier = New AnalogWaveform(Of Single)(numberOfComponentCarriers - 1) {}

      averagingEnabled = RFmxLteMXModAccAveragingEnabled.[False]
      averagingCount = 10

      synchronizationMode = RFmxLteMXModAccSynchronizationMode.Frame
      measurementOffset = 0
      measurementLength = 1

      evmUnit = RFmxLteMXModAccEvmUnit.Percentage

      timeout = 10.0
      ' (s) 
   End Sub

   Private Sub ConfigureLte()
      ' Create a new RFmx Session 

      lte = instrSession.GetLteSignalConfiguration()
      instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)

      lte.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
      lte.ConfigureDigitalEdgeTrigger("", digitalEdgeTriggerSource, digitalEdgeTriggerEdge, triggerDelay, enableTrigger)
      lte.ComponentCarrier.ConfigureSpacing("", componentSpacingType, componentCarrierAtCenterFrequency)
      lte.ConfigureBand("", band)
      lte.ConfigureDuplexScheme("", duplexScheme, uplinkDownlinkConfiguration)
      lte.ConfigureLinkDirection("", linkDirection)
      lte.ConfigureNumberOfComponentCarriers("", numberOfComponentCarriers)
      lte.ComponentCarrier.ConfigureArray("", componentCarrierBandwidth, componentCarrierFrequency, componentCarrierCellId)
      lte.ComponentCarrier.ConfigureDownlinkTestModelArray("", downlinkTestModel)
      lte.SelectMeasurements("", RFmxLteMXMeasurementTypes.ModAcc, True)
      lte.ModAcc.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount)
      lte.ModAcc.Configuration.ConfigureSynchronizationModeAndInterval("", synchronizationMode, measurementOffset, measurementLength)
      lte.ModAcc.Configuration.ConfigureEvmUnit("", evmUnit)
      lte.Initiate("", "")
   End Sub

   Private Sub RetrieveResults()
      lte.ModAcc.Results.FetchCompositeEvmArray("", timeout, meanRmsCompositeEvm, maximumPeakCompositeEvm, meanFrequencyError, peakCompositeEvmSymbolIndex,
         peakCompositeEvmSubcarrierIndex, peakCompositeEvmSlotIndex)
      lte.ModAcc.Results.FetchIQImpairmentsArray("", timeout, meanIQOriginOffset, meanIQGainImbalance, meanIQQuadratureError)
      lte.ModAcc.Results.FetchPdschEvmArray("", timeout, meanRmsEvm, meanRmsQpskEvm, meanRms16QamEvm, meanRms64QamEvm,
         meanRms256QamEvm)
      lte.ModAcc.Results.FetchPdsch1024QamEvmArray("", timeout, meanRms1024QamEvm)
      For i As Integer = 0 To numberOfComponentCarriers - 1
         subblockCarrierString = RFmxLteMX.BuildCarrierString("", i)
         lte.ModAcc.Results.FetchPdschQpskConstellation(subblockCarrierString, timeout, qpskConstellation(i))
         lte.ModAcc.Results.FetchEvmPerSubcarrierTrace(subblockCarrierString, timeout, meanRmsEvmPerSubcarrier(i))
      Next
   End Sub

   Private Sub PrintResults()
      Console.WriteLine("------------------------Measurements------------------------")
      For i As Integer = 0 To numberOfComponentCarriers - 1
         Console.WriteLine(vbLf & "Carrier {0}" & vbLf, i)
         Console.WriteLine("Mean RMS Composite EVM  (% or dB)    : {0}", meanRmsCompositeEvm(i))
         Console.WriteLine("Mean RMS EVM  (% or dB)              : {0}", meanRmsEvm(i))
         Console.WriteLine("Mean RMS QPSK EVM  (% or dB)         : {0}", meanRmsQpskEvm(i))
         Console.WriteLine("Mean RMS 16QAM EVM  (% or dB)        : {0}", meanRms16QamEvm(i))
         Console.WriteLine("Mean RMS 64QAM EVM  (% or dB)        : {0}", meanRms64QamEvm(i))
         Console.WriteLine("Mean RMS 256QAM EVM  (% or dB)       : {0}", meanRms256QamEvm(i))
         Console.WriteLine("Mean RMS 1024QAM EVM  (% or dB)      : {0}", meanRms1024QamEvm(i))
         Console.WriteLine("Mean Frequency Error  (Hz)           : {0}", meanFrequencyError(i))
         Console.WriteLine("Mean IQ Gain Imbalance  (dB)         : {0}", meanIQGainImbalance(i))
         Console.WriteLine("Mean IQ Origin Offset  (dBc)         : {0}", meanIQOriginOffset(i))
         Console.WriteLine("Mean IQ Quadrature Error  (deg)      : {0}", meanIQQuadratureError(i))
      Next
   End Sub

   Private Sub InitializeInstr()
      instrSession = New RFmxInstrMX(rfsaResourceName, "")
   End Sub

   Private Shared Sub DisplayError(ex As Exception)
      Console.WriteLine("ERROR:" & vbLf & Convert.ToString(ex.[GetType]()) & ": " & ex.Message)
   End Sub

End Class
