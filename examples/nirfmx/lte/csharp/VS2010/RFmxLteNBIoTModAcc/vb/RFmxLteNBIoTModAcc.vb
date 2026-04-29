'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
'4. Configure Trigger Type and Trigger Parameters.
'5. Configure Carrier Bandwidth.
'6. Configure NB-IoT Component Carrier.
'7. Configure NPUSCH Format.
'8. Configure Auto NPUSCH Channel Detection Enabled.
'9. Configure NPUSCH Starting Slot.
'10. Configure NPUSCH DMRS.
'11. Select ModAcc measurement and enable Traces.
'12. Configure Measurement Interval.
'13. Configure EVM Unit.
'14. Initiate the Measurement.
'15. Fetch ModAcc Measurements and Traces.
'16. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.LteMX

Public Class RFmxLteNBIoTModAcc
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

   Private nCellID As Integer
   Private uplinkSubcarrierSpacing As RFmxLteMXNBIoTUplinkSubcarrierSpacing
   Private nPuschFormat As Integer
   Private nPuschStartingSlot As Integer

   Private baseSequenceMode As RFmxLteMXNPuschDmrsBaseSequenceMode
   Private baseSequenceIndex As Integer
   Private cyclicShift As Integer
   Private groupHoppingEnabled As RFmxLteMXNPuschDmrsGroupHoppingEnabled
   Private deltaSS As Integer

   Private synchronizationMode As RFmxLteMXModAccSynchronizationMode
   Private measurementOffset As Integer
   Private measurementLength As Integer

   Private evmUnit As RFmxLteMXModAccEvmUnit

   Private componentCarrierBandwidth As Double
   Private componentCarrierFrequency As Double
   Private cellID As Integer

   Private autoNPuschChannelDetectionEnabled As RFmxLteMXAutoNPuschChannelDetectionEnabled

   Private timeout As Double

   Private meanRmsCompositeEvm As Double
   Private maxPeakCompositeEvm As Double
   Private meanFrequencyError As Double
   Private peakCompositeEvmSlotIndex As Integer
   Private peakCompositeEvmSymbolIndex As Integer
   Private peakCompositeEvmSubcarrierIndex As Integer
   Private meanIQOriginOffset As Double
   Private meanIQGainImbalance As Double
   Private meanIQQuadratureError As Double
   Private inBandEmissionMargin As Double
   Private dataConstellation As ComplexSingle(), dmrsDataConstellation As ComplexSingle()
   Private rmsEvmPerSymbol As AnalogWaveform(Of Single)

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

      nCellID = 0
      uplinkSubcarrierSpacing = RFmxLteMXNBIoTUplinkSubcarrierSpacing.SubcarrierSpacing15kHz
      nPuschFormat = 1
      nPuschStartingSlot = 0

      baseSequenceMode = RFmxLteMXNPuschDmrsBaseSequenceMode.Auto
      baseSequenceIndex = 0
      cyclicShift = 0
      groupHoppingEnabled = RFmxLteMXNPuschDmrsGroupHoppingEnabled.False
      deltaSS = 0

      synchronizationMode = RFmxLteMXModAccSynchronizationMode.Slot
      measurementOffset = 0
      '(slots)
      measurementLength = 1
      '(slots)

      evmUnit = RFmxLteMXModAccEvmUnit.Percentage

      componentCarrierBandwidth = 200000.0
      '(Hz)
      componentCarrierFrequency = 0.0
      '(Hz)
      cellID = 0

      autoNPuschChannelDetectionEnabled = RFmxLteMXAutoNPuschChannelDetectionEnabled.True

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
      lte.ComponentCarrier.Configure("", componentCarrierBandwidth, componentCarrierFrequency, cellID)
      lte.ComponentCarrier.ConfigureNBIoTComponentCarrier("", nCellID, uplinkSubcarrierSpacing)
      lte.ComponentCarrier.ConfigureNPuschFormat("", nPuschFormat)
      lte.ComponentCarrier.ConfigureAutoNPuschChannelDetectionEnabled("", autoNPuschChannelDetectionEnabled)
      lte.ComponentCarrier.ConfigureNPuschStartingSlot("", nPuschStartingSlot)
      lte.ComponentCarrier.ConfigureNPuschDmrs("", baseSequenceMode, baseSequenceIndex, cyclicShift,
         groupHoppingEnabled, deltaSS)
      lte.SelectMeasurements("", RFmxLteMXMeasurementTypes.ModAcc, True)
      lte.ModAcc.Configuration.ConfigureSynchronizationModeAndInterval("", synchronizationMode,
         measurementOffset, measurementLength)
      lte.ModAcc.Configuration.ConfigureEvmUnit("", evmUnit)
      lte.Initiate("", "")
   End Sub

   Private Sub RetrieveResults()
      lte.ModAcc.Results.FetchCompositeEvm("", timeout, meanRmsCompositeEvm, maxPeakCompositeEvm, meanFrequencyError,
       peakCompositeEvmSymbolIndex, peakCompositeEvmSubcarrierIndex, peakCompositeEvmSlotIndex)
      lte.ModAcc.Results.FetchIQImpairments("", timeout, meanIQOriginOffset, meanIQGainImbalance,
                                            meanIQQuadratureError)
      lte.ModAcc.Results.FetchInBandEmissionMargin("", timeout, inBandEmissionMargin)
      lte.ModAcc.Results.FetchNPuschConstellationTrace("", timeout, dataConstellation, dmrsDataConstellation)
      lte.ModAcc.Results.FetchEvmPerSubcarrierTrace("", timeout, rmsEvmPerSymbol)
   End Sub

   Private Sub PrintResults()
      Console.WriteLine("------------------Measurements------------------")
      Console.WriteLine("Mean RMS Composite EVM  (% or dB)       : {0}", meanRmsCompositeEvm)
      Console.WriteLine("Max Peak Composite EVM  (% or dB)       : {0}", maxPeakCompositeEvm)
      Console.WriteLine("Peak Composite EVM Slot Index           : {0}", peakCompositeEvmSlotIndex)
      Console.WriteLine("Peak Composite EVM Symbol Index         : {0}", peakCompositeEvmSymbolIndex)
      Console.WriteLine("Peak Composite EVM Subcarrier Index     : {0}", peakCompositeEvmSubcarrierIndex)
      Console.WriteLine("Mean Frequency Error  (Hz)              : {0}", meanFrequencyError)
      Console.WriteLine("Mean IQ Origin Offset  (dBc)            : {0}", meanIQOriginOffset)
      Console.WriteLine("Mean IQ Gain Imbalance  (dB)            : {0}", meanIQGainImbalance)
      Console.WriteLine("Mean IQ Quadrature Error  (deg)         : {0}", meanIQQuadratureError)
      Console.WriteLine("In-Band Emission Margin  (dB)           : {0}", inBandEmissionMargin)
   End Sub

   Private Shared Sub DisplayError(ex As Exception)
      Console.WriteLine("ERROR:" & vbLf & Convert.ToString(ex.[GetType]()) & ": " & ex.Message)
   End Sub

End Class
