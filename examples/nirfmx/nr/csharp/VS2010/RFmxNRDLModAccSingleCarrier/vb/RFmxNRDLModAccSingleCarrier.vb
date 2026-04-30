'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure Selected Ports.
'4. Configure basic signal properties(Center Frequency, Reference Level and External Attenuation).
'5. Configure Trigger Type and Trigger Parameters.
'6. Configure Link Direction as Downlink, Frequency Range, Carrier Bandwidth and BWP Subcarrier Spacing.
'7. Configure DL Test Model and DL Test Model Duplex Scheme.
'8. Select ModAcc measurement and enable Traces.
'9. Configure Synchronization Mode and Averaging Parameters for ModAcc measurement.
'10. Configure Measurement Interval.
'11. Initiate the Measurement.
'12. Fetch ModAcc Measurements and Traces.
'13. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.NRMX

Namespace NationalInstruments.Examples.RFmxNRDLModAccSingleCarrier
   Public Class RFmxNRDLModAccSingleCarrier
      Private instrSession As RFmxInstrMX
      Private NR As RFmxNRMX
      Private resourceName As String

      Private selectedPorts As String
      Private centerFrequency As Double
      Private referenceLevel As Double
      Private externalAttenuation As Double

      Private frequencyReferenceSource As String
      Private frequencyReferenceFrequency As Double

      Private enableTrigger As Boolean
      Private digitalEdgeSource As String
      Private digitalEdge As RFmxNRMXDigitalEdgeTriggerEdge
      Private triggerDelay As Double

      Private frequencyRange As RFmxNRMXFrequencyRange
      Private carrierBandwidth As Double
      Private subcarrierSpacing As Double

      Private downlinkTestModelDuplexScheme As RFmxNRMXDownlinkTestModelDuplexScheme
      Private downlinkTestModel As RFmxNRMXDownlinkTestModel

      Private synchronizationMode As RFmxNRMXModAccSynchronizationMode

      Private averagingEnabled As RFmxNRMXModAccAveragingEnabled
      Private averagingCount As Integer

      Private measurementLengthUnit As RFmxNRMXModAccMeasurementLengthUnit
      Private measurementOffset As Double
      Private measurementLength As Double

      Private timeout As Double

      Private compositeRmsEvmMean As Double
      ' (%)
      Private compositePeakEvmMaximum As Double
      ' (%)
      Private compositePeakEvmSlotIndex As Integer
      Private compositePeakEvmSymbolIndex As Integer
      Private compositePeakEvmSubcarrierIndex As Integer

      Private pdschRmsEvmMean As Double
      ' (%)

      Private componentCarrierFrequencyErrorMean As Double
      ' (Hz)
      Private componentCarrierIQOriginOffsetMean As Double
      ' (dBc)
      Private componentCarrierIQGainImbalanceMean As Double
      ' (dB)
      Private componentCarrierQuadratureErrorMean As Double
      ' (deg)

      Private pdschConstellation As ComplexSingle()

      Private rmsEvmPerSubcarrierMean As AnalogWaveform(Of Single)
      Private rmsEvmPerSymbolMean As AnalogWaveform(Of Single)

      Public Sub Run()
         Try
            InitializeVariables()
            InitializeInstr()
            ConfigureNR()
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

         selectedPorts = ""
         centerFrequency = 3500000000.0
         ' (Hz)
         referenceLevel = 0.0
         ' (dBm)
         externalAttenuation = 0.0
         ' (dB)

         frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
         frequencyReferenceFrequency = 10000000.0
         ' (Hz)

         enableTrigger = False
         digitalEdgeSource = RFmxNRMXConstants.PxiTriggerLine0
         digitalEdge = RFmxNRMXDigitalEdgeTriggerEdge.Rising
         triggerDelay = 0.0
         ' (s)

         frequencyRange = RFmxNRMXFrequencyRange.Range1
         carrierBandwidth = 100000000.0
         ' (Hz)
         subcarrierSpacing = 30000.0
         ' (Hz)

         downlinkTestModel = RFmxNRMXDownlinkTestModel.TM1_1
         downlinkTestModelDuplexScheme = RFmxNRMXDownlinkTestModelDuplexScheme.Fdd

         synchronizationMode = RFmxNRMXModAccSynchronizationMode.Slot

         averagingEnabled = RFmxNRMXModAccAveragingEnabled.False
         averagingCount = 10

         measurementLengthUnit = RFmxNRMXModAccMeasurementLengthUnit.Slot
         measurementOffset = 0.0
         measurementLength = 1

         timeout = 10.0
         ' (s)
      End Sub

      Private Sub InitializeInstr()
         instrSession = New RFmxInstrMX(resourceName, "")
      End Sub

      Private Sub ConfigureNR()
         NR = instrSession.GetNRSignalConfiguration()
         ' Create a new RFmx Session
         instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
         NR.SetSelectedPorts("", selectedPorts)
         NR.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
         NR.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger)

         NR.SetLinkDirection("", RFmxNRMXLinkDirection.Downlink)
         NR.SetFrequencyRange("", frequencyRange)
         NR.ComponentCarrier.SetBandwidth("", carrierBandwidth)
         NR.ComponentCarrier.SetBandwidthPartSubcarrierSpacing("", subcarrierSpacing)

         NR.ComponentCarrier.SetDownlinkTestModel("", downlinkTestModel)
         NR.ComponentCarrier.SetDownlinkTestModelDuplexScheme("", downlinkTestModelDuplexScheme)

         NR.SelectMeasurements("", RFmxNRMXMeasurementTypes.ModAcc, True)

         NR.ModAcc.Configuration.SetSynchronizationMode("", synchronizationMode)
         NR.ModAcc.Configuration.SetAveragingEnabled("", averagingEnabled)
         NR.ModAcc.Configuration.SetAveragingCount("", averagingCount)

         NR.ModAcc.Configuration.SetMeasurementLengthUnit("", measurementLengthUnit)
         NR.ModAcc.Configuration.SetMeasurementOffset("", measurementOffset)
         NR.ModAcc.Configuration.SetMeasurementLength("", measurementLength)

         NR.Initiate("", "")
      End Sub

      Private Sub RetrieveResults()
         NR.ModAcc.Results.GetCompositeRmsEvmMean("", compositeRmsEvmMean)
         NR.ModAcc.Results.GetCompositePeakEvmMaximum("", compositePeakEvmMaximum)
         NR.ModAcc.Results.GetCompositePeakEvmSlotIndex("", compositePeakEvmSlotIndex)
         NR.ModAcc.Results.GetCompositePeakEvmSymbolIndex("", compositePeakEvmSymbolIndex)
         NR.ModAcc.Results.GetCompositePeakEvmSubcarrierIndex("", compositePeakEvmSubcarrierIndex)

         NR.ModAcc.Results.GetComponentCarrierFrequencyErrorMean("", componentCarrierFrequencyErrorMean)
         NR.ModAcc.Results.GetComponentCarrierIQOriginOffsetMean("", componentCarrierIQOriginOffsetMean)
         NR.ModAcc.Results.GetComponentCarrierIQGainImbalanceMean("", componentCarrierIQGainImbalanceMean)
         NR.ModAcc.Results.GetComponentCarrierQuadratureErrorMean("", componentCarrierQuadratureErrorMean)

         Select Case downlinkTestModel

            Case RFmxNRMXDownlinkTestModel.TM1_1, RFmxNRMXDownlinkTestModel.TM1_2, RFmxNRMXDownlinkTestModel.TM3_3
               NR.ModAcc.Results.GetPdschQpskRmsEvmMean("", pdschRmsEvmMean)
               NR.ModAcc.Results.FetchPdschQpskConstellationTrace("", timeout, pdschConstellation)

            Case RFmxNRMXDownlinkTestModel.TM2, RFmxNRMXDownlinkTestModel.TM3_1
               NR.ModAcc.Results.GetPdsch64QamRmsEvmMean("", pdschRmsEvmMean)
               NR.ModAcc.Results.FetchPdsch64QamConstellationTrace("", timeout, pdschConstellation)

            Case RFmxNRMXDownlinkTestModel.TM2a, RFmxNRMXDownlinkTestModel.TM3_1a
               NR.ModAcc.Results.GetPdsch256QamRmsEvmMean("", pdschRmsEvmMean)
               NR.ModAcc.Results.FetchPdsch256QamConstellationTrace("", timeout, pdschConstellation)

            Case RFmxNRMXDownlinkTestModel.TM3_2
               NR.ModAcc.Results.GetPdsch16QamRmsEvmMean("", pdschRmsEvmMean)
               NR.ModAcc.Results.FetchPdsch16QamConstellationTrace("", timeout, pdschConstellation)

         End Select

         NR.ModAcc.Results.FetchRmsEvmPerSubcarrierMeanTrace("", timeout, rmsEvmPerSubcarrierMean)
         NR.ModAcc.Results.FetchRmsEvmPerSymbolMeanTrace("", timeout, rmsEvmPerSymbolMean)
      End Sub

      Private Sub PrintResults()
         Console.WriteLine("------------------Measurement------------------" & vbLf)
         Console.WriteLine("Composite RMS EVM Mean (%)                     : {0}", compositeRmsEvmMean)
         Console.WriteLine("Composite Peak EVM Maximum (%)                 : {0}", compositePeakEvmMaximum)
         Console.WriteLine("Composite Peak EVM Slot Index                  : {0}", compositePeakEvmSlotIndex)
         Console.WriteLine("Composite Peak EVM Symbol Index                : {0}", compositePeakEvmSymbolIndex)
         Console.WriteLine("Composite Peak EVM Subcarrier Index            : {0}", compositePeakEvmSubcarrierIndex)
         Console.WriteLine("PDSCH RMS EVM Mean (%)                         : {0}", pdschRmsEvmMean)
         Console.WriteLine("Component Carrier Frequency Error Mean (Hz)    : {0}", componentCarrierFrequencyErrorMean)
         Console.WriteLine("Component Carrier IQ Origin Offset Mean (dBc)  : {0}", componentCarrierIQOriginOffsetMean)
         Console.WriteLine("Component Carrier IQ Gain Imbalance Mean (dB)  : {0}", componentCarrierIQGainImbalanceMean)
         Console.WriteLine("Component Carrier Quadrature Error Mean (deg)  : {0}" & vbLf, componentCarrierQuadratureErrorMean)
      End Sub

      Private Sub CloseSession()
         If NR IsNot Nothing Then
            NR.Dispose()
            NR = Nothing
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
