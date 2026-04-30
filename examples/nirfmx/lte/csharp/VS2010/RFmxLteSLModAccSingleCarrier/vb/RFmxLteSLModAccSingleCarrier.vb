'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
'4. Configure Trigger Type and Trigger Parameters.
'5. Configure Carrier Bandwidth.
'6. Configure Link Direction to Sidelink.
'7. Configure operating Band to 47.
'8. Configure Auto Resource Block Detection Enabled to True.
'9. Configure Auto DMRS Detection Enabled to True.
'10. Select ModAcc measurement and enable Traces.
'11. Configure Synchronization Mode and Measurement Interval.
'12. Configure EVM Unit.
'13. Initiate the Measurement.
'14. Fetch ModAcc Measurements and Traces.
'15. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.LteMX

Namespace NationalInstruments.Examples.RFmxLteSLModAccSingleCarrier
   Public Class RFmxLteSLModAccSingleCarrier
      Private instrSession As RFmxInstrMX
      Private lte As RFmxLteMX

      Private resourceName As String
      Private centerFrequency As Double
      Private referenceLevel As Double
      Private externalAttenuation As Double

      Private frequencyReferenceSource As String
      Private frequencyReferenceFrequency As Double

      Private iqPowerEdgeEnabled As Boolean
      Private iqPowerEdgeLevel As Double
      Private triggerDelay As Double
      Private minimumQuietTimeMode As RFmxLteMXTriggerMinimumQuietTimeMode
      Private minimumQuietTime As Double

      Private carrierBandwidth As Double

      Private measurementOffset As Integer
      Private measurementLength As Integer

      Private evmUnit As RFmxLteMXModAccEvmUnit

      Private timeout As Double

      Private meanRmsCompositeEvm As Double
      ' (% or dB)
      Private maxPeakCompositeEvm As Double
      ' (% or dB)
      Private meanFrequencyError As Double
      ' (Hz)
      Private peakCompositeEvmSlotIndex As Integer
      Private peakCompositeEvmSymbolIndex As Integer
      Private peakCompositeEvmSubcarrierIndex As Integer
      Private meanIQOriginOffset As Double
      ' (dBc)
      Private meanIQGainImbalance As Double
      ' (dB)
      Private meanIQQuadratureError As Double
      ' (deg)
      Private inBandEmissionMargin As Double
      ' (dB)
      Private dataConstellation As ComplexSingle(), dmrsConstellation As ComplexSingle()
      Private evmPerSubcarrier As AnalogWaveform(Of Single)
      ' (% or dB)

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
         centerFrequency = 5890000000.0
         ' (Hz)
         referenceLevel = 0.0
         ' (dBm)
         externalAttenuation = 0.0
         ' (dB)

         frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
         frequencyReferenceFrequency = 10000000.0
         ' (Hz)

         iqPowerEdgeEnabled = True
         iqPowerEdgeLevel = -20.0
         '(dB)
         triggerDelay = 0.0
         ' (s)
         minimumQuietTimeMode = RFmxLteMXTriggerMinimumQuietTimeMode.Auto
         minimumQuietTime = 0.00005
         ' (s)

         carrierBandwidth = 10000000.0
         ' (Hz)

         measurementOffset = 0
         ' (slots)
         measurementLength = 1
         ' (slots)

         evmUnit = RFmxLteMXModAccEvmUnit.Percentage

         timeout = 10.0
         ' (s)
      End Sub

      Private Sub InitializeInstr()
         instrSession = New RFmxInstrMX(resourceName, "")
      End Sub

      Private Sub ConfigureLte()
         lte = instrSession.GetLteSignalConfiguration()
         ' Create a new RFmx Session
         instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
         lte.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
         lte.ConfigureIQPowerEdgeTrigger("", "0", RFmxLteMXIQPowerEdgeTriggerSlope.Rising, iqPowerEdgeLevel,
            triggerDelay, minimumQuietTimeMode, minimumQuietTime, RFmxLteMXIQPowerEdgeTriggerLevelType.Relative,
            iqPowerEdgeEnabled)
         lte.ComponentCarrier.Configure("", carrierBandwidth, 0.0, 0)
         lte.ConfigureLinkDirection("", RFmxLteMXLinkDirection.Sidelink)
         lte.ConfigureBand("", 47)
         lte.ComponentCarrier.ConfigureAutoResourceBlockDetectionEnabled("", RFmxLteMXAutoResourceBlockDetectionEnabled.True)
         lte.ConfigureAutoDmrsDetectionEnabled("", RFmxLteMXAutoDmrsDetectionEnabled.True)
         lte.SelectMeasurements("", RFmxLteMXMeasurementTypes.ModAcc, True)
         lte.ModAcc.Configuration.ConfigureSynchronizationModeAndInterval("", RFmxLteMXModAccSynchronizationMode.Slot,
            measurementOffset, measurementLength)
         lte.ModAcc.Configuration.ConfigureEvmUnit("", evmUnit)
         lte.Initiate("", "")
      End Sub

      Private Sub RetrieveResults()
         lte.ModAcc.Results.FetchCompositeEvm("", timeout, meanRmsCompositeEvm, maxPeakCompositeEvm, meanFrequencyError,
            peakCompositeEvmSymbolIndex, peakCompositeEvmSubcarrierIndex, peakCompositeEvmSlotIndex)
         lte.ModAcc.Results.FetchIQImpairments("", timeout, meanIQOriginOffset, meanIQGainImbalance, meanIQQuadratureError)
         lte.ModAcc.Results.FetchInBandEmissionMargin("", timeout, inBandEmissionMargin)
         lte.ModAcc.Results.FetchPsschConstellationTrace("", timeout, dataConstellation, dmrsConstellation)
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

      Private Shared Sub DisplayError(ex As Exception)
         Console.WriteLine("ERROR:" & vbLf & Convert.ToString(ex.[GetType]()) & ": " & ex.Message)
      End Sub

   End Class
End Namespace