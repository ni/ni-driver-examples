'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
'4. Configure Trigger Type and Trigger Parameters.
'5. Configure Carrier Bandwidth.
'6. Select Downlink as Link Direction.
'7. Configure Downlink Test Model. 
'8. Select ModAcc measurement and enable Traces.
'9. Configure Averaging Parameters for ModAcc measurement.
'10. Select Frame as Synchronization Mode and configure Measurement Interval.
'11. Configure EVM Unit.
'12. Initiate the Measurement.
'13. Fetch ModAcc Measurements and Traces.
'14. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.LteMX

Namespace NationalInstruments.Examples.RFmxLteNBIoTDLModAcc
    Public Class RFmxLteNBIoTDLModAcc
        Private instrSession As RFmxInstrMX
        Private lte As RFmxLteMX
        Private resourceName As String

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

        Private measurementOffset As Integer
        Private measurementLength As Integer

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
        Private meanRmsNpssEvm As Double
        Private meanRmsNsssEvm As Double
        Private meanRmsNrsEvm As Double
        Private qpskConstellation As ComplexSingle()
        Private meanRmsEvmPerSubcarrier As AnalogWaveform(Of Single)

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
            instrSession = New RFmxInstrMX(resourceName, "")
        End Sub

        Private Sub InitializeVariables()
            resourceName = "RFSA"

            frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
            frequencyReferenceFrequency = 10000000.0
            ' (Hz) 

            centerFrequency = 2140000000.0
            ' (Hz) 
            referenceLevel = 0.0
            ' (dBm) 
            externalAttenuation = 0.0
            ' (dB) 

            enableTrigger = False
            digitalEdgeTriggerSource = RFmxLteMXConstants.Pfi0
            digitalEdgeTriggerEdge = RFmxLteMXDigitalEdgeTriggerEdge.Rising
            triggerDelay = 0.0
            ' (s) 

            averagingEnabled = RFmxLteMXModAccAveragingEnabled.[False]
            averagingCount = 10

            measurementOffset = 0
            '(slots) 
            measurementLength = 20
            '(slots) 

            timeout = 10.0
            ' (s) 
        End Sub

        Private Sub ConfigureLte()
            lte = instrSession.GetLteSignalConfiguration()
            ' Create a new RFmx Session 
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
            lte.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
            lte.ConfigureDigitalEdgeTrigger("", digitalEdgeTriggerSource, digitalEdgeTriggerEdge, triggerDelay, enableTrigger)
            lte.ComponentCarrier.Configure("", 200000.0, 0.0, 0)
            lte.ConfigureLinkDirection("", RFmxLteMXLinkDirection.Downlink)
            lte.SelectMeasurements("", RFmxLteMXMeasurementTypes.ModAcc, True)
            lte.ModAcc.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount)
            lte.ModAcc.Configuration.ConfigureSynchronizationModeAndInterval("", RFmxLteMXModAccSynchronizationMode.Frame, measurementOffset, measurementLength)
            lte.ModAcc.Configuration.ConfigureEvmUnit("", RFmxLteMXModAccEvmUnit.Percentage)
            lte.ComponentCarrier.SetNBIoTDownlinkChannelConfigurationMode("", RFmxLteMXNBIoTDownlinkChannelConfigurationMode.TestModel)
            lte.Initiate("", "")
        End Sub

        Private Sub RetrieveResults()
            lte.ModAcc.Results.FetchCompositeEvm("", timeout, meanRmsCompositeEvm, maxPeakCompositeEvm, meanFrequencyError, peakCompositeEvmSymbolIndex,
                peakCompositeEvmSubcarrierIndex, peakCompositeEvmSlotIndex)
            lte.ModAcc.Results.FetchIQImpairments("", timeout, meanIQOriginOffset, meanIQGainImbalance, meanIQQuadratureError)
            lte.ModAcc.Results.GetNpdschMeanRmsEvm("", meanRmsEvm)
            lte.ModAcc.Results.GetNpdschMeanRmsQpskEvm("", meanRmsQpskEvm)
            lte.ModAcc.Results.GetMeanRmsNpssEvm("", meanRmsNpssEvm)
            lte.ModAcc.Results.GetMeanRmsNsssEvm("", meanRmsNsssEvm)
            lte.ModAcc.Results.GetMeanRmsNrsEvm("", meanRmsNrsEvm)
            lte.ModAcc.Results.FetchNpdschQpskConstellation("", timeout, qpskConstellation)
            lte.ModAcc.Results.FetchEvmPerSubcarrierTrace("", timeout, meanRmsEvmPerSubcarrier)
        End Sub

        Private Sub PrintResults()
            Console.WriteLine("------------------Measurement---------------")
            Console.WriteLine("Mean RMS Composite EVM  (% or dB)    : {0}", meanRmsCompositeEvm)
            Console.WriteLine("Max Peak Composite EVM  (% or dB)    : {0}", maxPeakCompositeEvm)
            Console.WriteLine("NPDSCH Mean RMS  EVM  (% or dB)      : {0}", meanRmsEvm)
            Console.WriteLine("NPDSCH Mean RMS QPSK EVM  (% or dB)  : {0}", meanRmsQpskEvm)
            Console.WriteLine("Mean RMS NPSS EVM  (% or dB)         : {0}", meanRmsNpssEvm)
            Console.WriteLine("Mean RMS NSSS EVM  (% or dB)         : {0}", meanRmsNsssEvm)
            Console.WriteLine("Mean RMS NRS EVM (% or dB)           : {0}", meanRmsNrsEvm)
            Console.WriteLine("Mean Frequency Error  (Hz)           : {0}", meanFrequencyError)
            Console.WriteLine("Mean IQ Origin Offset  (dBc)         : {0}", meanIQOriginOffset)
        End Sub

        Private Shared Sub DisplayError(ex As Exception)
            Console.WriteLine("ERROR:" & vbLf & Convert.ToString(ex.[GetType]()) & ": " & ex.Message)
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

    End Class
End Namespace
