'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure Selected Ports.
'4. Configure basic signal pproperties (Center Frequency, Reference Level and External Attenuation).
'5. Configure Trigger Type and Trigger Parameters.
'6. Configure Link Direction as Downlink and Number of Subblocks.
'7. Configure Sublocks and Carriers .
'8. Configure BWP Subcarrier Spacing, DL Test Model Duplex Scheme and DL Test Model for all carriers in each subblock.
'9. Select ModAcc measurement and enable Traces.
'10. Configure Synchronization Mode for ModAcc measurement.
'11. Configure Measurement Interval.
'12. Initiate the Measurement.
'13. Fetch ModAcc Measurements and Traces.
'14. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.NRMX

Namespace NationalInstruments.Examples.RFmxNRDLModAccNonContiguousMultiCarrier
Public Class RFmxNRDLModAccNonContiguousMultiCarrier
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

	Const NumberOfSubblocks As Integer = 2
	Const NumberOfComponentCarriers As Integer = 2

	Private frequencyRange As RFmxNRMXFrequencyRange

	Private subblockFrequency As Double() = New Double(NumberOfSubblocks - 1) {}
	Private componentCarrierSpacingType As RFmxNRMXComponentCarrierSpacingType() = New RFmxNRMXComponentCarrierSpacingType(NumberOfSubblocks - 1) {}
	Private channelRaster As Double() = New Double(NumberOfSubblocks - 1) {}
	Private componentCarrierAtCenterFrequency As Integer() = New Integer(NumberOfSubblocks - 1) {}

	Private componentCarrierBandwidth As Double(,) = New Double(NumberOfSubblocks - 1, NumberOfComponentCarriers - 1) {}
	Private componentCarrierFrequency As Double(,) = New Double(NumberOfSubblocks - 1, NumberOfComponentCarriers - 1) {}

	Private subcarrierSpacing As Double

	Private downlinkTestModel As RFmxNRMXDownlinkTestModel

	Private downlinkTestModelDuplexScheme As RFmxNRMXDownlinkTestModelDuplexScheme

	Private synchronizationMode As RFmxNRMXModAccSynchronizationMode

	Private measurementLengthUnit As RFmxNRMXModAccMeasurementLengthUnit
	Private measurementOffset As Double
	Private measurementLength As Double

	Private subblockString As String
	Private carrierString As String

	Private timeout As Double

	Private compositeRmsEvmMean As Double(,) = New Double(NumberOfSubblocks - 1, NumberOfComponentCarriers - 1) {}
	' (%) 
	Private compositePeakEvmMaximum As Double(,) = New Double(NumberOfSubblocks - 1, NumberOfComponentCarriers - 1) {}
	' (%) 
	Private compositePeakEvmSlotIndex As Integer(,) = New Integer(NumberOfSubblocks - 1, NumberOfComponentCarriers - 1) {}
	Private compositePeakEvmSymbolIndex As Integer(,) = New Integer(NumberOfSubblocks - 1, NumberOfComponentCarriers - 1) {}
	Private compositePeakEvmSubcarrierIndex As Integer(,) = New Integer(NumberOfSubblocks - 1, NumberOfComponentCarriers - 1) {}

	Private pdschRmsEvmMean As Double(,) = New Double(NumberOfSubblocks - 1, NumberOfComponentCarriers - 1) {}
	' (%) 

	Private componentCarrierFrequencyErrorMean As Double(,) = New Double(NumberOfSubblocks - 1, NumberOfComponentCarriers - 1) {}
	' (Hz) 
	Private componentCarrierIQOriginOffsetMean As Double(,) = New Double(NumberOfSubblocks - 1, NumberOfComponentCarriers - 1) {}
	' (dBc) 
	Private componentCarrierIQGainImbalanceMean As Double(,) = New Double(NumberOfSubblocks - 1, NumberOfComponentCarriers - 1) {}
	' (dB) 
	Private componentCarrierQuadratureErrorMean As Double(,) = New Double(NumberOfSubblocks - 1, NumberOfComponentCarriers - 1) {}
	' (deg) 

	Private rmsEvmPerSubcarrierMean As AnalogWaveform(Of Single)(,) = New AnalogWaveform(Of Single)(NumberOfSubblocks - 1, NumberOfComponentCarriers - 1) {}
	Private rmsEvmPerSymbolMean As AnalogWaveform(Of Single)(,) = New AnalogWaveform(Of Single)(NumberOfSubblocks - 1, NumberOfComponentCarriers - 1) {}

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

		subblockFrequency(0) = 0.0
		' (Hz) 
		subblockFrequency(1) = 200000000.0
		' (Hz) 
		componentCarrierSpacingType(0) = RFmxNRMXComponentCarrierSpacingType.Nominal
		componentCarrierSpacingType(1) = RFmxNRMXComponentCarrierSpacingType.Nominal
		channelRaster(0) = 15000.0
		' (Hz) 
		channelRaster(1) = 15000.0
		' (Hz) 
		componentCarrierAtCenterFrequency(0) = -1
		componentCarrierAtCenterFrequency(1) = -1

		componentCarrierBandwidth(0, 0) = 100000000.0
		' (Hz) 
		componentCarrierBandwidth(0, 1) = 100000000.0
		' (Hz) 
		componentCarrierBandwidth(1, 0) = 100000000.0
		' (Hz) 
		componentCarrierBandwidth(1, 1) = 100000000.0
		' (Hz) 
		componentCarrierFrequency(0, 0) = -49980000.0
		' (Hz) 
		componentCarrierFrequency(0, 1) = 50010000.0
		' (Hz) 
		componentCarrierFrequency(1, 0) = -49980000.0
		' (Hz) 
		componentCarrierFrequency(1, 1) = 50010000.0
		' (Hz) 

		subcarrierSpacing = 30000.0
		' (Hz) 

		downlinkTestModel = RFmxNRMXDownlinkTestModel.TM1_1

		downlinkTestModelDuplexScheme = RFmxNRMXDownlinkTestModelDuplexScheme.Fdd

		synchronizationMode = RFmxNRMXModAccSynchronizationMode.Slot

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
		' Create a new RFmx Session 

		NR = instrSession.GetNRSignalConfiguration()
		instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
		NR.SetSelectedPorts("", selectedPorts)
		NR.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
		NR.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger)
		NR.SetLinkDirection("", RFmxNRMXLinkDirection.Downlink)
		NR.SetNumberOfSubblocks("", NumberOfSubblocks)

		For i As Integer = 0 To NumberOfSubblocks - 1
			subblockString = RFmxNRMX.BuildSubblockString("", i)
			NR.SetFrequencyRange(subblockString, frequencyRange)
			NR.SetSubblockFrequency(subblockString, subblockFrequency(i))
			NR.SetChannelRaster(subblockString, channelRaster(i))
			NR.SetComponentCarrierSpacingType(subblockString, componentCarrierSpacingType(i))
			NR.SetComponentCarrierAtCenterFrequency(subblockString, componentCarrierAtCenterFrequency(i))
			NR.ComponentCarrier.SetNumberOfComponentCarriers(subblockString, NumberOfComponentCarriers)

			For j As Integer = 0 To NumberOfComponentCarriers - 1
				carrierString = RFmxNRMX.BuildCarrierString(subblockString, j)
				NR.ComponentCarrier.SetBandwidth(carrierString, componentCarrierBandwidth(i, j))
				NR.ComponentCarrier.SetFrequency(carrierString, componentCarrierFrequency(i, j))
			Next

			carrierString = RFmxNRMX.BuildCarrierString(subblockString, -1)
			NR.ComponentCarrier.SetBandwidthPartSubcarrierSpacing(carrierString, subcarrierSpacing)
			NR.ComponentCarrier.SetDownlinkTestModelDuplexScheme(carrierString, downlinkTestModelDuplexScheme)
			NR.ComponentCarrier.SetDownlinkTestModel(carrierString, downlinkTestModel)
		Next

		NR.SelectMeasurements("", RFmxNRMXMeasurementTypes.ModAcc, True)

		NR.ModAcc.Configuration.SetSynchronizationMode("", synchronizationMode)
		NR.ModAcc.Configuration.SetMeasurementLengthUnit("", measurementLengthUnit)
		NR.ModAcc.Configuration.SetMeasurementOffset("", measurementOffset)
		NR.ModAcc.Configuration.SetMeasurementLength("", measurementLength)
		NR.Initiate("", "")
	End Sub

	Private Sub RetrieveResults()
		For i As Integer = 0 To NumberOfSubblocks - 1
			subblockString = RFmxNRMX.BuildSubblockString("", i)

			For j As Integer = 0 To NumberOfComponentCarriers - 1
				carrierString = RFmxNRMX.BuildCarrierString(subblockString, j)
				NR.ModAcc.Results.GetCompositeRmsEvmMean(carrierString, compositeRmsEvmMean(i, j))
				NR.ModAcc.Results.GetCompositePeakEvmMaximum(carrierString, compositePeakEvmMaximum(i, j))
				NR.ModAcc.Results.GetCompositePeakEvmSlotIndex(carrierString, compositePeakEvmSlotIndex(i, j))
				NR.ModAcc.Results.GetCompositePeakEvmSymbolIndex(carrierString, compositePeakEvmSymbolIndex(i, j))
				NR.ModAcc.Results.GetCompositePeakEvmSubcarrierIndex(carrierString, compositePeakEvmSubcarrierIndex(i, j))
				NR.ModAcc.Results.GetComponentCarrierFrequencyErrorMean(carrierString, componentCarrierFrequencyErrorMean(i, j))
				NR.ModAcc.Results.GetComponentCarrierIQOriginOffsetMean(carrierString, componentCarrierIQOriginOffsetMean(i, j))
				NR.ModAcc.Results.GetComponentCarrierIQGainImbalanceMean(carrierString, componentCarrierIQGainImbalanceMean(i, j))
				NR.ModAcc.Results.GetComponentCarrierQuadratureErrorMean(carrierString, componentCarrierQuadratureErrorMean(i, j))

				Select Case downlinkTestModel
					Case RFmxNRMXDownlinkTestModel.TM1_1, RFmxNRMXDownlinkTestModel.TM1_2, RFmxNRMXDownlinkTestModel.TM3_3
						NR.ModAcc.Results.GetPdschQpskRmsEvmMean(carrierString, pdschRmsEvmMean(i, j))
						Exit Select

					Case RFmxNRMXDownlinkTestModel.TM2, RFmxNRMXDownlinkTestModel.TM3_1
						NR.ModAcc.Results.GetPdsch64QamRmsEvmMean(carrierString, pdschRmsEvmMean(i, j))
						Exit Select

					Case RFmxNRMXDownlinkTestModel.TM2a, RFmxNRMXDownlinkTestModel.TM3_1a
						NR.ModAcc.Results.GetPdsch256QamRmsEvmMean(carrierString, pdschRmsEvmMean(i, j))
						Exit Select

					Case RFmxNRMXDownlinkTestModel.TM3_2
						NR.ModAcc.Results.GetPdsch16QamRmsEvmMean(carrierString, pdschRmsEvmMean(i, j))
						Exit Select
				End Select

				NR.ModAcc.Results.FetchRmsEvmPerSubcarrierMeanTrace(carrierString, timeout, rmsEvmPerSubcarrierMean(i, j))
				NR.ModAcc.Results.FetchRmsEvmPerSymbolMeanTrace(carrierString, timeout, rmsEvmPerSymbolMean(i, j))
			Next
		Next
	End Sub

	Private Sub PrintResults()
		Console.WriteLine("------------------------Measurement------------------------" & vbLf)
		For i As Integer = 0 To NumberOfSubblocks - 1
			Console.WriteLine("Subblock  : {0}" & vbLf, i)
			For j As Integer = 0 To NumberOfComponentCarriers - 1
				Console.WriteLine("Carrier  : {0}", j)
				Console.WriteLine("Composite RMS EVM Mean (%)                     : {0}", compositeRmsEvmMean(i, j))
				Console.WriteLine("Composite Peak EVM Maximum (%)                 : {0}", compositePeakEvmMaximum(i, j))
				Console.WriteLine("Composite Peak EVM Slot Index                  : {0}", compositePeakEvmSlotIndex(i, j))
				Console.WriteLine("Composite Peak EVM Symbol Index                : {0}", compositePeakEvmSymbolIndex(i, j))
				Console.WriteLine("Composite Peak EVM Subcarrier Index            : {0}", compositePeakEvmSubcarrierIndex(i, j))
				Console.WriteLine("PDSCH RMS EVM Mean (%)                         : {0}", pdschRmsEvmMean(i, j))
				Console.WriteLine("Component Carrier Frequency Error Mean (Hz)    : {0}", componentCarrierFrequencyErrorMean(i, j))
				Console.WriteLine("Component Carrier IQ Origin Offset Mean (dBc)  : {0}", componentCarrierIQOriginOffsetMean(i, j))
				Console.WriteLine("Component Carrier IQ Gain Imbalance Mean (dB)  : {0}", componentCarrierIQGainImbalanceMean(i, j))
				Console.WriteLine("Component Carrier Quadrature Error Mean (deg)  : {0}", componentCarrierQuadratureErrorMean(i, j))
				Console.WriteLine("-----------------------------------------------------------------" & vbLf)
			Next
		Next
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
