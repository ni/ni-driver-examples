'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure Selected Ports.
'4. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
'5. Configure Trigger Parameters for IQ Power Edge Trigger.
'6. Configure Frequency Range, Channel Raster, Component Carrier Spacing and Number of Component Carriers.
'7. Configure Component Carriers.
'8. Configure PUSCH and PUSCH RB Allocation.
'9. Configure PUSCH DMRS.
'10. Select PVT measurement and enable Traces.
'11. Configure Measurement Methods.
'12. Configure OFF Power Exclusion Periods.
'13. Configure Averaging Parameters for PVT measurement.
'14. Initiate the Measurement.
'15. Fetch PVT Traces and Measurements.
'16. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.NRMX


Namespace NationalInstruments.Examples.RFmxNRULPvtContiguousMultiCarrier
	Public Class RFmxNRULPvtContiguousMultiCarrier
		Private instrSession As RFmxInstrMX
		Private NR As RFmxNRMX
		Private resourceName As String

		Private selectedPorts As String
		Private centerFrequency As Double
		Private referenceLevel As Double
		Private externalAttenuation As Double

		Private frequencyReferenceSource As String
		Private frequencyReferenceFrequency As Double

		Private iqPowerEdgeLevel As Double
		Private triggerDelay As Double
		Private minimumQuietTimeMode As RFmxNRMXTriggerMinimumQuietTimeMode
		Private minimumQuietTime As Double

		Private frequencyRange As RFmxNRMXFrequencyRange

		Private componentCarrierSpacingType As RFmxNRMXComponentCarrierSpacingType
		Private channelRaster As Double
		Private componentCarrierAtCenterFrequency As Integer
		Private subcarrierSpacing As Double

		Const NumberOfComponentCarriers As Integer = 2
		Private componentCarrierBandwidth As Double() = New Double(NumberOfComponentCarriers - 1) {}
		Private componentCarrierFrequency As Double() = New Double(NumberOfComponentCarriers - 1) {}
		Private cellID As Integer() = New Integer(NumberOfComponentCarriers - 1) {}

		Private puschTransformPrecodingEnabled As RFmxNRMXPuschTransformPrecodingEnabled
		Private puschModulationType As RFmxNRMXPuschModulationType
		Const NumberOfResourceBlockClusters As Integer = 1
		Private puschResourceBlockOffset As Integer() = New Integer(NumberOfResourceBlockClusters - 1) {}
		Private puschNumberOfResourceBlocks As Integer() = New Integer(NumberOfResourceBlockClusters - 1) {}
		Private puschSlotAllocation As String
		Private puschSymbolAllocation As String

		Private puschDmrsPowerMode As RFmxNRMXPuschDmrsPowerMode
		Private puschDmrsPower As Double
		Private puschDmrsConfigurationType As RFmxNRMXPuschDmrsConfigurationType
		Private puschMappingType As RFmxNRMXPuschMappingType
		Private puschDmrsTypeAPosition As Integer
		Private puschDmrsDuration As RFmxNRMXPuschDmrsDuration
		Private puschDmrsAdditionalPositions As Integer

		Private measurementMethod As RFmxNRMXPvtMeasurementMethod
		Private offPowerExclusionBefore As Double
		Private offPowerExclusionAfter As Double

		Private averagingEnabled As RFmxNRMXPvtAveragingEnabled
		Private averagingCount As Integer
		Private averagingType As RFmxNRMXPvtAveragingType

		Private subblockString As String
		Private carrierString As String
		Private puschClusterString As String

		Private timeout As Double

		Private measurementStatus As RFmxNRMXPvtMeasurementStatus() = New RFmxNRMXPvtMeasurementStatus(NumberOfComponentCarriers - 1) {}
		Private absoluteOffPowerBefore As Double() = New Double(NumberOfComponentCarriers - 1) {}
		' (dBm) 
		Private absoluteOffPowerAfter As Double() = New Double(NumberOfComponentCarriers - 1) {}
		' (dBm) 
		Private absoluteONPower As Double() = New Double(NumberOfComponentCarriers - 1) {}
		' (dBm) 
		Private burstWidth As Double() = New Double(NumberOfComponentCarriers - 1) {}
		' (s) 

		Private signalPower As AnalogWaveform(Of Single)() = New AnalogWaveform(Of Single)(NumberOfComponentCarriers - 1) {}
		Private absoluteLimit As AnalogWaveform(Of Single)() = New AnalogWaveform(Of Single)(NumberOfComponentCarriers - 1) {}

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

			iqPowerEdgeLevel = -20.0
			' (dB) 
			triggerDelay = 0.0
			' (s) 
			minimumQuietTimeMode = RFmxNRMXTriggerMinimumQuietTimeMode.Auto
			minimumQuietTime = 0.000008
			' (s) 

			frequencyRange = RFmxNRMXFrequencyRange.Range1

			componentCarrierSpacingType = RFmxNRMXComponentCarrierSpacingType.Nominal
			channelRaster = 15000.0
			' (Hz) 
			componentCarrierAtCenterFrequency = -1
			subcarrierSpacing = 30000.0
			' (Hz) 

			componentCarrierBandwidth(0) = 100000000.0
			' (Hz) 
			componentCarrierBandwidth(1) = 100000000.0
			' (Hz) 
			componentCarrierFrequency(0) = -49980000.0
			' (Hz) 
			componentCarrierFrequency(1) = 50010000.0
			' (Hz) 
			cellID(0) = 0
			cellID(1) = 1

			puschTransformPrecodingEnabled = RFmxNRMXPuschTransformPrecodingEnabled.[False]
			puschModulationType = RFmxNRMXPuschModulationType.Qpsk
			puschResourceBlockOffset(0) = 0
			puschNumberOfResourceBlocks(0) = -1
			puschSlotAllocation = "1"
			puschSymbolAllocation = "0-Last"

			puschDmrsPowerMode = RFmxNRMXPuschDmrsPowerMode.CdmGroups
			puschDmrsPower = 0.0
			' (dB) 
			puschDmrsConfigurationType = RFmxNRMXPuschDmrsConfigurationType.Type1
			puschMappingType = RFmxNRMXPuschMappingType.TypeA
			puschDmrsTypeAPosition = 2
			puschDmrsDuration = RFmxNRMXPuschDmrsDuration.SingleSymbol
			puschDmrsAdditionalPositions = 0

			measurementMethod = RFmxNRMXPvtMeasurementMethod.Normal
			offPowerExclusionBefore = 0.0
			' (s) 
			offPowerExclusionAfter = 0.0
			' (s) 

			averagingEnabled = RFmxNRMXPvtAveragingEnabled.[False]
			averagingCount = 10
			averagingType = RFmxNRMXPvtAveragingType.Rms

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
			NR.ConfigureIQPowerEdgeTrigger("", "0", RFmxNRMXIQPowerEdgeTriggerSlope.Rising, iqPowerEdgeLevel, triggerDelay, minimumQuietTimeMode,
				minimumQuietTime, RFmxNRMXIQPowerEdgeTriggerLevelType.Relative, True)

			NR.SetFrequencyRange("", frequencyRange)
			NR.SetChannelRaster("", channelRaster)
			NR.SetComponentCarrierSpacingType("", componentCarrierSpacingType)
			NR.SetComponentCarrierAtCenterFrequency("", componentCarrierAtCenterFrequency)
			NR.ComponentCarrier.SetNumberOfComponentCarriers("", NumberOfComponentCarriers)

			subblockString = RFmxNRMX.BuildSubblockString("", 0)
			For i As Integer = 0 To NumberOfComponentCarriers - 1
				carrierString = RFmxNRMX.BuildCarrierString(subblockString, i)
				NR.ComponentCarrier.SetBandwidth(carrierString, componentCarrierBandwidth(i))
				NR.ComponentCarrier.SetFrequency(carrierString, componentCarrierFrequency(i))
				NR.ComponentCarrier.SetCellID(carrierString, cellID(i))
			Next

			carrierString = "carrier::all"
			NR.ComponentCarrier.SetPuschTransformPrecodingEnabled(carrierString, puschTransformPrecodingEnabled)
			NR.ComponentCarrier.SetPuschModulationType(carrierString, puschModulationType)
			NR.ComponentCarrier.SetPuschSlotAllocation(carrierString, puschSlotAllocation)
			NR.ComponentCarrier.SetPuschSymbolAllocation(carrierString, puschSymbolAllocation)

			NR.ComponentCarrier.SetBandwidthPartSubcarrierSpacing(carrierString, subcarrierSpacing)
			NR.ComponentCarrier.SetPuschNumberOfResourceBlockClusters(carrierString, NumberOfResourceBlockClusters)

			For i As Integer = 0 To NumberOfResourceBlockClusters - 1
				puschClusterString = RFmxNRMX.BuildPuschClusterString(carrierString, i)
				NR.ComponentCarrier.SetPuschResourceBlockOffset(puschClusterString, puschResourceBlockOffset(i))
				NR.ComponentCarrier.SetPuschNumberOfResourceBlocks(puschClusterString, puschNumberOfResourceBlocks(i))
			Next

			NR.ComponentCarrier.SetPuschDmrsPowerMode(carrierString, puschDmrsPowerMode)
			NR.ComponentCarrier.SetPuschDmrsPower(carrierString, puschDmrsPower)
			NR.ComponentCarrier.SetPuschDmrsConfigurationType(carrierString, puschDmrsConfigurationType)
			NR.ComponentCarrier.SetPuschMappingType(carrierString, puschMappingType)
			NR.ComponentCarrier.SetPuschDmrsTypeAPosition(carrierString, puschDmrsTypeAPosition)
			NR.ComponentCarrier.SetPuschDmrsDuration(carrierString, puschDmrsDuration)
			NR.ComponentCarrier.SetPuschDmrsAdditionalPositions(carrierString, puschDmrsAdditionalPositions)

			NR.SelectMeasurements("", RFmxNRMXMeasurementTypes.Pvt, True)

			NR.Pvt.Configuration.ConfigureMeasurementMethod("", measurementMethod)
			NR.Pvt.Configuration.ConfigureOffPowerExclusionPeriods("", offPowerExclusionBefore, offPowerExclusionAfter)
			NR.Pvt.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)

			NR.Initiate("", "")
		End Sub

		Private Sub RetrieveResults()
			NR.Pvt.Results.FetchMeasurementArray("", timeout, measurementStatus, absoluteOffPowerBefore, absoluteOffPowerAfter, absoluteONPower,
				burstWidth)

			For i As Integer = 0 To NumberOfComponentCarriers - 1
				carrierString = RFmxNRMX.BuildCarrierString(subblockString, i)
				NR.Pvt.Results.FetchSignalPowerTrace(carrierString, timeout, signalPower(i), absoluteLimit(i))
			Next
		End Sub

		Private Sub PrintResults()
			Console.WriteLine("------------------------Measurements------------------------" & vbLf)
			For i As Integer = 0 To NumberOfComponentCarriers - 1
				Console.WriteLine("Carrier  : {0}", i)
				Console.WriteLine("Measurement Status                       : {0}", measurementStatus(i))
				Console.WriteLine("Mean Absolute OFF power Before (dBm)     : {0}", absoluteOffPowerBefore(i))
				Console.WriteLine("Mean Absolute OFF power After (dBm)      : {0}", absoluteOffPowerAfter(i))
				Console.WriteLine("Mean Absolute ON power (dBm)             : {0}", absoluteONPower(i))
				Console.WriteLine("Burst Width (s)                          : {0}", burstWidth(i))
				Console.WriteLine("-----------------------------------------------------------------" & vbLf)
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

