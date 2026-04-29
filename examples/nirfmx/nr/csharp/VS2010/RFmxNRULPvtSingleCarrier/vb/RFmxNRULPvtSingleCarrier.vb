'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure Selected Ports.
'4. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
'5. Configure Trigger Parameters for IQ Power Edge Trigger.
'6. Configure Frequency Range, Carrier Bandwidth, Cell ID and Subcarrier Spacing.
'7. Configure PUSCH and PUSCH RB Allocation.
'8. Configure PUSCH DMRS.
'9. Select PVT measurement and enable Traces.
'10. Configure Measurement Methods.
'11. Configure OFF Power Exclusion Periods.
'12. Configure Averaging Parameters for PVT measurement.
'13. Initiate the Measurement.
'14. Fetch PVT Traces and Measurements.
'15. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.NRMX


Namespace NationalInstruments.Examples.RFmxNRULPvtSingleCarrier
	Public Class RFmxNRULPvtSingleCarrier
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
		Private cellID As Integer
		Private carrierBandwidth As Double
		Private subcarrierSpacing As Double

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
		Private bandwidthPartString As String
		Private userString As String
		Private puschString As String
		Private puschClusterString As String

		Private timeout As Double

		Private measurementStatus As RFmxNRMXPvtMeasurementStatus
		Private absoluteOffPowerBefore As Double
		' (dBm) 
		Private absoluteOffPowerAfter As Double
		' (dBm) 
		Private absoluteONPower As Double
		' (dBm) 
		Private burstWidth As Double
		' (s) 

		Private signalPower As AnalogWaveform(Of Single)
		' (dBm) 
		Private absoluteLimit As AnalogWaveform(Of Single)
		' (dBm) 

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
			cellID = 0
			carrierBandwidth = 100000000.0
			' (Hz) 
			subcarrierSpacing = 30000.0
			' (Hz) 

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
			NR.ComponentCarrier.SetBandwidth("", carrierBandwidth)
			NR.ComponentCarrier.SetCellID("", cellID)
			NR.ComponentCarrier.SetBandwidthPartSubcarrierSpacing("", subcarrierSpacing)

			NR.ComponentCarrier.SetPuschTransformPrecodingEnabled("", puschTransformPrecodingEnabled)
			NR.ComponentCarrier.SetPuschSlotAllocation("", puschSlotAllocation)
			NR.ComponentCarrier.SetPuschSymbolAllocation("", puschSymbolAllocation)
			NR.ComponentCarrier.SetPuschModulationType("", puschModulationType)

			NR.ComponentCarrier.SetPuschNumberOfResourceBlockClusters("", NumberOfResourceBlockClusters)

			subblockString = RFmxNRMX.BuildSubblockString("", 0)
			carrierString = RFmxNRMX.BuildCarrierString(subblockString, 0)
			bandwidthPartString = RFmxNRMX.BuildBandwidthPartString(carrierString, 0)
			userString = RFmxNRMX.BuildUserString(bandwidthPartString, 0)
			puschString = RFmxNRMX.BuildPuschString(userString, 0)
			For i As Integer = 0 To NumberOfResourceBlockClusters - 1
				puschClusterString = RFmxNRMX.BuildPuschClusterString(puschString, i)
				NR.ComponentCarrier.SetPuschResourceBlockOffset(puschClusterString, puschResourceBlockOffset(i))
				NR.ComponentCarrier.SetPuschNumberOfResourceBlocks(puschClusterString, puschNumberOfResourceBlocks(i))
			Next

			NR.ComponentCarrier.SetPuschDmrsPowerMode("", puschDmrsPowerMode)
			NR.ComponentCarrier.SetPuschDmrsPower("", puschDmrsPower)
			NR.ComponentCarrier.SetPuschDmrsConfigurationType("", puschDmrsConfigurationType)
			NR.ComponentCarrier.SetPuschMappingType("", puschMappingType)
			NR.ComponentCarrier.SetPuschDmrsTypeAPosition("", puschDmrsTypeAPosition)
			NR.ComponentCarrier.SetPuschDmrsDuration("", puschDmrsDuration)
			NR.ComponentCarrier.SetPuschDmrsAdditionalPositions("", puschDmrsAdditionalPositions)

			NR.SelectMeasurements("", RFmxNRMXMeasurementTypes.Pvt, True)

			NR.Pvt.Configuration.ConfigureMeasurementMethod("", measurementMethod)
			NR.Pvt.Configuration.ConfigureOffPowerExclusionPeriods("", offPowerExclusionBefore, offPowerExclusionAfter)
			NR.Pvt.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)

			NR.Initiate("", "")
		End Sub

		Private Sub RetrieveResults()
			NR.Pvt.Results.FetchMeasurement("", timeout, measurementStatus, absoluteOffPowerBefore, absoluteOffPowerAfter, absoluteONPower,
				burstWidth)

			NR.Pvt.Results.FetchSignalPowerTrace("", timeout, signalPower, absoluteLimit)
		End Sub

		Private Sub PrintResults()
			Console.WriteLine("------------------Measurement------------------" & vbLf)
			Console.WriteLine("Measurement Status                       : {0}", measurementStatus)
			Console.WriteLine("Mean Absolute OFF power Before (dBm)     : {0}", absoluteOffPowerBefore)
			Console.WriteLine("Mean Absolute OFF power After (dBm)      : {0}", absoluteOffPowerAfter)
			Console.WriteLine("Mean Absolute ON power (dBm)             : {0}", absoluteONPower)
			Console.WriteLine("Burst Width (s)                          : {0}", burstWidth)
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

