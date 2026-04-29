'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure Selected Ports.
'4. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
'5. Configure Trigger Type and Trigger Parameters.
'6. Configure Auto Increment Cell ID Enabled, Auto RB Detection Enabled and Number of Subblocks.
'7. Configure Subblocks and Carriers.
'8. Configure PUSCH for all Carriers in each Subblock.
'9. Configure PUSCH DMRS for all Carriers in each Subblock.
'10. Select ModAcc measurement and enable Traces.
'11. Configure Synchronization Mode for ModAcc measurement.
'12. Configure Measurement Interval.
'13. Initiate the Measurement.
'14. Fetch ModAcc Measurements and Traces.
'15. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.NRMX

Namespace NationalInstruments.Examples.RFmxNRULModAccNonContiguousMultiCarrier
Public Class RFmxNRULModAccNonContiguousMultiCarrier
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

	Private band As Integer() = New Integer(NumberOfSubblocks - 1) {}
	Private subblockFrequency As Double() = New Double(NumberOfSubblocks - 1) {}
	Private componentCarrierSpacingType As RFmxNRMXComponentCarrierSpacingType() = New RFmxNRMXComponentCarrierSpacingType(NumberOfSubblocks - 1) {}
	Private channelRaster As Double() = New Double(NumberOfSubblocks - 1) {}
	Private componentCarrierAtCenterFrequency As Integer() = New Integer(NumberOfSubblocks - 1) {}

	Private componentCarrierBandwidth As Double(,) = New Double(NumberOfSubblocks - 1, NumberOfComponentCarriers - 1) {}
	Private componentCarrierFrequency As Double(,) = New Double(NumberOfSubblocks - 1, NumberOfComponentCarriers - 1) {}
	Private cellID As Integer(,) = New Integer(NumberOfSubblocks - 1, NumberOfComponentCarriers - 1) {}

	Private frequencyRange As RFmxNRMXFrequencyRange

	Private subcarrierSpacing As Double

	Private autoIncrementCellIDEnabled As RFmxNRMXAutoIncrementCellIDEnabled

	Private puschTransformPrecodingEnabled As RFmxNRMXPuschTransformPrecodingEnabled
	Private puschSlotAllocation As String
	Private puschSymbolAllocation As String

	Private puschDmrsPowerMode As RFmxNRMXPuschDmrsPowerMode
	Private puschDmrsPower As Double
	Private puschDmrsConfigurationType As RFmxNRMXPuschDmrsConfigurationType
	Private puschMappingType As RFmxNRMXPuschMappingType
	Private puschDmrsTypeAPosition As Integer
	Private puschDmrsDuration As RFmxNRMXPuschDmrsDuration
	Private puschDmrsAdditionalPositions As Integer

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
	Private componentCarrierFrequencyErrorMean As Double(,) = New Double(NumberOfSubblocks - 1, NumberOfComponentCarriers - 1) {}
	' (Hz) 
	Private componentCarrierIQOriginOffsetMean As Double(,) = New Double(NumberOfSubblocks - 1, NumberOfComponentCarriers - 1) {}
	' (dBc) 
	Private componentCarrierIQGainImbalanceMean As Double(,) = New Double(NumberOfSubblocks - 1, NumberOfComponentCarriers - 1) {}
	' (dB) 
	Private componentCarrierQuadratureErrorMean As Double(,) = New Double(NumberOfSubblocks - 1, NumberOfComponentCarriers - 1) {}
	' (deg) 
	Private inBandEmissionMargin As Double(,) = New Double(NumberOfSubblocks - 1, NumberOfComponentCarriers - 1) {}
	' (dB) 

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

		band(0) = 78
		band(1) = 78
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
		cellID(0, 0) = 0
		cellID(0, 1) = 1
		cellID(1, 0) = 0
		cellID(1, 1) = 1

		frequencyRange = RFmxNRMXFrequencyRange.Range1

		subcarrierSpacing = 30000.0
		' (Hz) 

		autoIncrementCellIDEnabled = RFmxNRMXAutoIncrementCellIDEnabled.[True]

		puschTransformPrecodingEnabled = RFmxNRMXPuschTransformPrecodingEnabled.[False]
		puschSlotAllocation = "0-Last"
		puschSymbolAllocation = "0-Last"

		puschDmrsPowerMode = RFmxNRMXPuschDmrsPowerMode.CdmGroups
		puschDmrsPower = 0.0
		' (dB) 
		puschDmrsConfigurationType = RFmxNRMXPuschDmrsConfigurationType.Type1
		puschMappingType = RFmxNRMXPuschMappingType.TypeA
		puschDmrsTypeAPosition = 2
		puschDmrsDuration = RFmxNRMXPuschDmrsDuration.SingleSymbol
		puschDmrsAdditionalPositions = 0

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
		NR.SetLinkDirection("", RFmxNRMXLinkDirection.Uplink)
		NR.SetAutoResourceBlockDetectionEnabled("", RFmxNRMXAutoResourceBlockDetectionEnabled.[True])
		NR.SetAutoIncrementCellIDEnabled("", autoIncrementCellIDEnabled)
		NR.SetNumberOfSubblocks("", NumberOfSubblocks)

		For i As Integer = 0 To NumberOfSubblocks - 1
			subblockString = RFmxNRMX.BuildSubblockString("", i)
			NR.SetFrequencyRange(subblockString, frequencyRange)
			NR.SetSubblockFrequency(subblockString, subblockFrequency(i))
			NR.SetBand(subblockString, band(i))
			NR.SetComponentCarrierSpacingType(subblockString, componentCarrierSpacingType(i))
			NR.SetComponentCarrierAtCenterFrequency(subblockString, componentCarrierAtCenterFrequency(i))
			NR.SetChannelRaster(subblockString, channelRaster(i))
			NR.ComponentCarrier.SetNumberOfComponentCarriers(subblockString, NumberOfComponentCarriers)

			For j As Integer = 0 To NumberOfComponentCarriers - 1
				carrierString = RFmxNRMX.BuildCarrierString(subblockString, j)
				NR.ComponentCarrier.SetBandwidth(carrierString, componentCarrierBandwidth(i, j))
				NR.ComponentCarrier.SetFrequency(carrierString, componentCarrierFrequency(i, j))
				NR.ComponentCarrier.SetCellID(carrierString, cellID(i, j))
			Next

			carrierString = RFmxNRMX.BuildCarrierString(subblockString, -1)
			NR.ComponentCarrier.SetPuschTransformPrecodingEnabled(carrierString, puschTransformPrecodingEnabled)
			NR.ComponentCarrier.SetPuschSlotAllocation(carrierString, puschSlotAllocation)
			NR.ComponentCarrier.SetPuschSymbolAllocation(carrierString, puschSymbolAllocation)

			NR.ComponentCarrier.SetBandwidthPartSubcarrierSpacing(carrierString, subcarrierSpacing)

			NR.ComponentCarrier.SetPuschDmrsPowerMode(carrierString, puschDmrsPowerMode)
			NR.ComponentCarrier.SetPuschDmrsPower(carrierString, puschDmrsPower)
			NR.ComponentCarrier.SetPuschDmrsConfigurationType(carrierString, puschDmrsConfigurationType)
			NR.ComponentCarrier.SetPuschMappingType(carrierString, puschMappingType)
			NR.ComponentCarrier.SetPuschDmrsTypeAPosition(carrierString, puschDmrsTypeAPosition)
			NR.ComponentCarrier.SetPuschDmrsDuration(carrierString, puschDmrsDuration)
			NR.ComponentCarrier.SetPuschDmrsAdditionalPositions(carrierString, puschDmrsAdditionalPositions)
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
				NR.ModAcc.Results.GetInBandEmissionMargin(carrierString, inBandEmissionMargin(i, j))

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
				Console.WriteLine("Component Carrier Frequency Error Mean (Hz)    : {0}", componentCarrierFrequencyErrorMean(i, j))
				Console.WriteLine("Component Carrier IQ Origin Offset Mean (dBc)  : {0}", componentCarrierIQOriginOffsetMean(i, j))
				Console.WriteLine("Component Carrier IQ Gain Imbalance Mean (dB)  : {0}", componentCarrierIQGainImbalanceMean(i, j))
				Console.WriteLine("Component Carrier Quadrature Error Mean (deg)  : {0}", componentCarrierQuadratureErrorMean(i, j))
				Console.WriteLine("In-Band Emission Margin (dB)                   : {0}", inBandEmissionMargin(i, j))
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
