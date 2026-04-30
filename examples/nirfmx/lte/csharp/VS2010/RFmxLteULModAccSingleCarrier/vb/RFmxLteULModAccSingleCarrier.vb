'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
'4. Configure Trigger Type and Trigger Parameters.
'5. Configure Carrier Bandwidth.
'6. Configure operating Band.
'7. Configure Duplex Mode.
'8. Configure Auto DMRS Detection Enabled.
'9. Select ModAcc measurement And enable Traces.
'10. Configure Synchronization Mode And Measurement Interval.
'11. Configure EVM Unit.
'12. Configure In-Band Emission Mask Type.
'13. Configure Averaging Parameters for ModAcc measurement.
'14. Initiate the Measurement.
'15. Fetch ModAcc Measurements And Traces.
'16. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.LteMX

Public Class RFmxLteULModAccSingleCarrier
   Private instrSession As RFmxInstrMX
   Private lte As RFmxLteMX
   Private rfsaResourceName As String

   Private frequencyReferenceSource As String
   Private frequencyReferenceFrequency As Double

   Private centerFrequency As Double
   Private referenceLevel As Double
   Private externalAttenuation As Double

   Private enableTrigger As Boolean
   Private digitalEdgeSource As String
   Private digitalEdge As RFmxLteMXDigitalEdgeTriggerEdge
   Private triggerDelay As Double

   Private componentCarrierBandwidth As Double
   Private componentCarrierFrequency As Double
   Private cellID As Integer

   Private averagingEnabled As RFmxLteMXModAccAveragingEnabled
   Private averagingCount As Integer

   Private synchronizationMode As RFmxLteMXModAccSynchronizationMode
   Private measurementOffset As Integer
   Private measurementLength As Integer
   Private evmUnit As RFmxLteMXModAccEvmUnit
   Private inBandEmissionMaskType As RFmxLteMXModAccInBandEmissionMaskType
   Private band As Integer
   Private duplexScheme As RFmxLteMXDuplexScheme
   Private uplinkDownlinkConfiguration As RFmxLteMXUplinkDownlinkConfiguration

   Private autoDmrsDetectionEnabled As RFmxLteMXAutoDmrsDetectionEnabled

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
   Private evmPerSubcarrier As AnalogWaveform(Of Single)

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
      '(dBm)

      enableTrigger = False
      digitalEdgeSource = RFmxLteMXConstants.Pfi0
      digitalEdge = RFmxLteMXDigitalEdgeTriggerEdge.Rising
      triggerDelay = 0.0
      '(s)

      componentCarrierBandwidth = 10000000.0
      '(Hz)
      componentCarrierFrequency = 0.0
      '(Hz)
      cellID = 0

      averagingEnabled = RFmxLteMXModAccAveragingEnabled.[False]
      averagingCount = 10

      synchronizationMode = RFmxLteMXModAccSynchronizationMode.Slot
      measurementOffset = 0
      '(slots)
      measurementLength = 1
      '(slots)

      uplinkDownlinkConfiguration = RFmxLteMXUplinkDownlinkConfiguration.Configuration0
      duplexScheme = RFmxLteMXDuplexScheme.Fdd

      band = 1

      evmUnit = RFmxLteMXModAccEvmUnit.Percentage

      inBandEmissionMaskType = RFmxLteMXModAccInBandEmissionMaskType.Release11Onwards

      autoDmrsDetectionEnabled = RFmxLteMXAutoDmrsDetectionEnabled.True

      timeout = 10.0
      '(s)
   End Sub

   Private Sub ConfigureLte()
      lte = instrSession.GetLteSignalConfiguration()
      'Create a new RFmx Session
      instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
      lte.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
      lte.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger)
      lte.ComponentCarrier.Configure("", componentCarrierBandwidth, componentCarrierFrequency, cellID)
      lte.ConfigureBand("", band)
      lte.ConfigureDuplexScheme("", duplexScheme, uplinkDownlinkConfiguration)
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
      lte.ModAcc.Results.FetchCompositeEvm("", timeout, meanRmsCompositeEvm, maxPeakCompositeEvm, meanFrequencyError,
       peakCompositeEvmSymbolIndex, peakCompositeEvmSubcarrierIndex, peakCompositeEvmSlotIndex)
      lte.ModAcc.Results.FetchIQImpairments("", timeout, meanIQOriginOffset, meanIQGainImbalance,
                                            meanIQQuadratureError)
      lte.ModAcc.Results.FetchInBandEmissionMargin("", timeout, inBandEmissionMargin)
      lte.ModAcc.Results.FetchPuschConstellationTrace("", timeout, dataConstellation, dmrsDataConstellation)
      lte.ModAcc.Results.FetchEvmPerSubcarrierTrace("", timeout, evmPerSubcarrier)
   End Sub

   Private Sub PrintResults()
      Console.WriteLine("------------------Measurement------------------")
      Console.WriteLine("Mean RMS Composite EVM  (% or dB)       : {0}", meanRmsCompositeEvm)
      Console.WriteLine("Max Peak Composite EVM  (% or dB)       : {0}", maxPeakCompositeEvm)
      Console.WriteLine("Peak Composite EVM Slot Index           : {0}", peakCompositeEvmSlotIndex)
      Console.WriteLine("Peak Composite EVM Symbol Index         : {0}", peakCompositeEvmSymbolIndex)
      Console.WriteLine("Peak Composite EVM Subcarrier Index     : {0}", peakCompositeEvmSubcarrierIndex)
      Console.WriteLine("Mean Frequency Error  (Hz)              : {0}", meanFrequencyError)
      Console.WriteLine("Mean IQ Origin Offset  (dBc)            : {0}", meanIQOriginOffset)
      Console.WriteLine("Mean IQ Gain Imbalance  (dB)            : {0}", meanIQGainImbalance)
      Console.WriteLine("Mean IQ Quadrature Error  (deg)         : {0}", meanIQQuadratureError)
      Console.WriteLine("In Band Emission Margin  (dB)           : {0}", inBandEmissionMargin)
   End Sub

   Private Shared Sub DisplayError(ex As Exception)
      Console.WriteLine("ERROR:" & vbLf & Convert.ToString(ex.[GetType]()) & ": " & ex.Message)
   End Sub

End Class
