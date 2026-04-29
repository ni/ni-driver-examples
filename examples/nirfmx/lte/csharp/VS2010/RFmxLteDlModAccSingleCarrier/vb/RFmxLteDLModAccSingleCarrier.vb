'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
'4. Configure Trigger Type and Trigger Parameters.
'5. Configure Carrier Bandwidth.
'6. Configure operating Band.
'7. Configure Duplex Scheme.
'8. Select Downlink as Link Direction.
'9. Configure Downlink Test Model. 
'10. Select ModAcc measurement and enable Traces.
'11. Configure Averaging Parameters for ModAcc measurement.
'12. Select Frame as Synchronization Mode and configure Measurement Interval.
'13. Configure EVM Unit.
'14. Initiate the Measurement.
'15[A-E]. Fetch ModAcc Measurements and Traces.
'16. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.LteMX

Public Class RFmxLteDLModAccSingleCarrier
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

   Private componentCarrierBandwidth As Double
   Private componentCarrierFrequency As Double
   Private cellID As Integer
   Private downlinkTestModel As RFmxLteMXDownlinkTestModel

   Private averagingEnabled As RFmxLteMXModAccAveragingEnabled
   Private averagingCount As Integer

   Private synchronizationMode As RFmxLteMXModAccSynchronizationMode
   Private measurementOffset As Integer
   Private measurementLength As Integer
   Private evmUnit As RFmxLteMXModAccEvmUnit
   Private band As Integer
   Private linkDirection As RFmxLteMXLinkDirection
   Private duplexScheme As RFmxLteMXDuplexScheme
   Private uplinkDownlinkConfiguration As RFmxLteMXUplinkDownlinkConfiguration

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
   Private meanRmsEvm As Double
   Private meanRmsQpskEvm As Double
   Private meanRms16QamEvm As Double
   Private meanRms64QamEvm As Double
   Private meanRms256QamEvm As Double
   Private meanRms1024QamEvm As Double
   Private qpskConstellation As ComplexSingle(), qam16Constellation As ComplexSingle(), qam64Constellation As ComplexSingle(), qam256Constellation As ComplexSingle(), qam1024Constellation As ComplexSingle()
   Private meanRmsEvmPerSubcarrier As AnalogWaveform(Of Single)

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

      componentCarrierBandwidth = 10000000.0
      ' (Hz) 
      componentCarrierFrequency = 0.0
      ' (Hz) 
      cellID = 0
      downlinkTestModel = RFmxLteMXDownlinkTestModel.TM1_1

      averagingEnabled = RFmxLteMXModAccAveragingEnabled.[False]
      averagingCount = 10

      synchronizationMode = RFmxLteMXModAccSynchronizationMode.Frame
      measurementOffset = 0
      '(slots) 
      measurementLength = 1
      '(slots) 

      linkDirection = RFmxLteMXLinkDirection.Downlink
      uplinkDownlinkConfiguration = RFmxLteMXUplinkDownlinkConfiguration.Configuration0
      duplexScheme = RFmxLteMXDuplexScheme.Fdd

      band = 1

      evmUnit = RFmxLteMXModAccEvmUnit.Percentage

      timeout = 10.0
      ' (s) 
   End Sub

   Private Sub ConfigureLte()
      lte = instrSession.GetLteSignalConfiguration()
      ' Create a new RFmx Session 
      instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
      lte.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
      lte.ConfigureDigitalEdgeTrigger("", digitalEdgeTriggerSource, digitalEdgeTriggerEdge, triggerDelay, enableTrigger)
      lte.ComponentCarrier.Configure("", componentCarrierBandwidth, componentCarrierFrequency, cellID)
      lte.ConfigureBand("", band)
      lte.ConfigureDuplexScheme("", duplexScheme, uplinkDownlinkConfiguration)
      lte.ConfigureLinkDirection("", linkDirection)
      lte.ComponentCarrier.ConfigureDownlinkTestModel("", downlinkTestModel)
      lte.SelectMeasurements("", RFmxLteMXMeasurementTypes.ModAcc, True)
      lte.ModAcc.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount)
      lte.ModAcc.Configuration.ConfigureSynchronizationModeAndInterval("", synchronizationMode, measurementOffset, measurementLength)
      lte.ModAcc.Configuration.ConfigureEvmUnit("", evmUnit)
      lte.Initiate("", "")
   End Sub

   Private Sub RetrieveResults()
      lte.ModAcc.Results.FetchCompositeEvm("", timeout, meanRmsCompositeEvm, maxPeakCompositeEvm, meanFrequencyError, peakCompositeEvmSymbolIndex,
         peakCompositeEvmSubcarrierIndex, peakCompositeEvmSlotIndex)
      lte.ModAcc.Results.FetchIQImpairments("", timeout, meanIQOriginOffset, meanIQGainImbalance, meanIQQuadratureError)
      lte.ModAcc.Results.FetchPdschEvm("", timeout, meanRmsEvm, meanRmsQpskEvm, meanRms16QamEvm, meanRms64QamEvm,
         meanRms256QamEvm)
      lte.ModAcc.Results.FetchPdsch1024QamEvm("", timeout, meanRms1024QamEvm)
      lte.ModAcc.Results.FetchPdschQpskConstellation("", timeout, qpskConstellation)
      lte.ModAcc.Results.FetchPdsch16QamConstellation("", timeout, qam16Constellation)
      lte.ModAcc.Results.FetchPdsch64QamConstellation("", timeout, qam64Constellation)
      lte.ModAcc.Results.FetchPdsch256QamConstellation("", timeout, qam256Constellation)
      lte.ModAcc.Results.FetchPdsch1024QamConstellation("", timeout, qam1024Constellation)
      lte.ModAcc.Results.FetchEvmPerSubcarrierTrace("", timeout, meanRmsEvmPerSubcarrier)
   End Sub

   Private Sub PrintResults()
      Console.WriteLine("------------------Measurement------------------")
      Console.WriteLine("Mean RMS Composite EVM  (% or dB)       : {0}", meanRmsCompositeEvm)
      Console.WriteLine("Mean RMS  EVM  (% or dB)                : {0}", meanRmsEvm)
      Console.WriteLine("Mean RMS QPSK EVM  (% or dB)            : {0}", meanRmsQpskEvm)
      Console.WriteLine("Mean RMS 16QAM EVM  (% or dB)           : {0}", meanRms16QamEvm)
      Console.WriteLine("Mean RMS 64QAM EVM  (% or dB)           : {0}", meanRms64QamEvm)
      Console.WriteLine("Mean RMS 256QAM EVM (% or dB)           : {0}", meanRms256QamEvm)
      Console.WriteLine("Mean RMS 1024QAM EVM (% or dB)          : {0}", meanRms1024QamEvm)
      Console.WriteLine("Mean Frequency Error  (Hz)              : {0}", meanFrequencyError)
      Console.WriteLine("Mean IQ Origin Offset  (dBc)            : {0}", meanIQOriginOffset)
      Console.WriteLine("Mean IQ Gain Imbalance  (dB)            : {0}", meanIQGainImbalance)
      Console.WriteLine("Mean IQ Quadrature Error  (deg)         : {0}", meanIQQuadratureError)
   End Sub

   Private Shared Sub DisplayError(ex As Exception)
      Console.WriteLine("ERROR:" & vbLf & Convert.ToString(ex.[GetType]()) & ": " & ex.Message)
   End Sub

End Class
