'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure Selected Ports.
'4. Configure basic signal properties(Center Frequency, Reference Level and External Attenuation).
'5. Configure Trigger Parameters for Digitial Edge Trigger.
'6. Configure Number of Subblocks and Link Direction.
'7. Configure Frequency Range, Component Carrier Spacing, Channel Raster,
'   Component Carrier Center Frequency, Subblock Frequency and Number of Component Carriers.
'8. Configure Component Carriers.
'9. Configure Bandwidth Part Subcarrier Spacing.
'10. Select SEM measurement and enable Traces.
'11. Configure Uplink Mask Type, or Downlink Mask, Band, gNodeB Category and Delta F_Max(Hz) based on Link Direction.
'12. Configure Component Carrier Rated Output Power based on Link Direction.
'13. Configure Offsets.
'14. Configure Sweep Time Parameters.
'15. Configure Averaging Parameters for SEM measurement.
'16. Initiate the Measurement.
'17. Fetch SEM Measurements and Traces.
'18. Close the RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.NRMX

Namespace NationalInstruments.Examples.RFmxNRSemNonContiguousMultiCarrier
Public Class RFmxNRSemNonContiguousMultiCarrier
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

	Private linkDirection As RFmxNRMXLinkDirection
	Private frequencyRange As RFmxNRMXFrequencyRange

	Private uplinkMaskType As RFmxNRMXSemUplinkMaskType
	Private gNodeBCategory As RFmxNRMXgNodeBCategory
	Private downlinkMaskType As RFmxNRMXSemDownlinkMaskType
	Private deltaFMaximum As Double
	Private band As Integer

	Private subcarrierSpacing As Double

	Private sweepTimeAuto As RFmxNRMXSemSweepTimeAuto
	Private sweepTimeInterval As Double

	Private averagingEnabled As RFmxNRMXSemAveragingEnabled
	Private averagingCount As Integer
	Private averagingType As RFmxNRMXSemAveragingType

	Private timeout As Double

	Private subblockString As String
	Private carrierString As String

	Const NumberOfSubblocks As Integer = 2
	Const NumberOfComponentCarriers As Integer = 2
	Const NumberOfOffsets As Integer = 4

	' Subblock inputs structure 

	Private Structure SubblockMeasurementInput
		Public subblockFrequency As Double
		' (Hz) 
		Public componentCarrierSpacingType As RFmxNRMXComponentCarrierSpacingType
		Public channelRaster As Double
		' (Hz) 
		Public componentCarrierAtCenterFrequency As Integer

		Public componentCarrierBandwidth As Double()
		' (Hz) 
		Public componentCarrierFrequency As Double()
		' (Hz) 
		Public componentCarrierRatedOutputPower As Double()
		' (dBm) 

		Public offsetStartFrequency As Double()
		' (Hz) 
		Public offsetStopFrequency As Double()
		' (Hz) 
		Public offsetSideband As RFmxNRMXSemOffsetSideband()
		Public offsetRbw As Double()
		' (Hz) 
		Public offsetRbwFilterType As RFmxNRMXSemOffsetRbwFilterType()
		Public bandwidthIntegral As Integer()
		Public offsetLimitFailMask As RFmxNRMXSemOffsetLimitFailMask()
		Public absoluteLimitStart As Double()
		' (dBm) 
		Public absoluteLimitStop As Double()
		' (dBm) 
		Public relativeLimitStart As Double()
		' (dB) 
		Public relativeLimitStop As Double()
		' (dB) 
	End Structure

	' Subblock measurement outputs structure 

	Private Structure SubblockMeasurementOutput
		Public subblockPower As Double
		Public integrationBandwidth As Double
		Public frequency As Double

		Public lowerOffsetMarginRelativePower As Double()
		' (dB) 
		Public lowerOffsetMarginAbsolutePower As Double()
		' (dBm) 
		Public lowerOffsetMargin As Double()
		Public lowerOffsetMarginFrequency As Double()
		' (Hz) 
		Public lowerOffsetMeasurementStatus As RFmxNRMXSemLowerOffsetMeasurementStatus()

		Public upperOffsetMarginRelativePower As Double()
		' (dB) 
		Public upperOffsetMarginAbsolutePower As Double()
		' (dBm) 
		Public upperOffsetMargin As Double()
		Public upperOffsetMarginFrequency As Double()
		' (Hz) 
		Public upperOffsetMeasurementStatus As RFmxNRMXSemUpperOffsetMeasurementStatus()
	End Structure

	Private subblockInput As SubblockMeasurementInput()
	Private subblockOutput As SubblockMeasurementOutput()

	Private spectrum As Spectrum(Of Single)
	Private absoluteMask As Spectrum(Of Single)
	Private totalAggregatedPower As Double
	' (dBm) 
	Private measurementStatus As RFmxNRMXSemMeasurementStatus


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
			CloseSession()
			Console.WriteLine(vbLf & "Press any key to exit")
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

		linkDirection = RFmxNRMXLinkDirection.Uplink
		frequencyRange = RFmxNRMXFrequencyRange.Range1

		uplinkMaskType = RFmxNRMXSemUplinkMaskType.General

		gNodeBCategory = RFmxNRMXgNodeBCategory.WideAreaBaseStationCategoryA
		downlinkMaskType = RFmxNRMXSemDownlinkMaskType.Standard
		deltaFMaximum = 15000000.0
		' (Hz) 
		band = 78

		subcarrierSpacing = 30000.0
		' (Hz) 

		sweepTimeAuto = RFmxNRMXSemSweepTimeAuto.[True]
		sweepTimeInterval = 0.001
		' (s) 

		averagingEnabled = RFmxNRMXSemAveragingEnabled.[False]
		averagingCount = 10
		averagingType = RFmxNRMXSemAveragingType.Rms

		timeout = 10.0
		' (s) 

		subblockOutput = New SubblockMeasurementOutput(NumberOfSubblocks - 1) {}





			subblockInput = New SubblockMeasurementInput(NumberOfSubblocks - 1) {New SubblockMeasurementInput() With {
			.subblockFrequency = 0.0,
			.componentCarrierSpacingType = RFmxNRMXComponentCarrierSpacingType.Nominal,
			.channelRaster = 15000.0,
			.componentCarrierAtCenterFrequency = -1,
			.componentCarrierBandwidth = New Double(NumberOfComponentCarriers - 1) {100000000.0, 100000000.0},
			.componentCarrierFrequency = New Double(NumberOfComponentCarriers - 1) {-49980000.0, 50010000.0},
			.componentCarrierRatedOutputPower = New Double(NumberOfComponentCarriers - 1) {0.0, 0.0},
			.offsetStartFrequency = New Double(NumberOfOffsets - 1) {15000.0, 1500000.0, 5500000.0, 20500000.0},
			.offsetStopFrequency = New Double(NumberOfOffsets - 1) {985000.0, 4500000.0, 19500000.0, 24500000.0},
			.offsetSideband = New RFmxNRMXSemOffsetSideband(NumberOfOffsets - 1) {RFmxNRMXSemOffsetSideband.Both, RFmxNRMXSemOffsetSideband.Both, RFmxNRMXSemOffsetSideband.Both, RFmxNRMXSemOffsetSideband.Both},
			.offsetRbw = New Double(NumberOfOffsets - 1) {10000.0, 250000.0, 250000.0, 250000.0},
			.offsetRbwFilterType = New RFmxNRMXSemOffsetRbwFilterType(NumberOfOffsets - 1) {RFmxNRMXSemOffsetRbwFilterType.Gaussian, RFmxNRMXSemOffsetRbwFilterType.Gaussian, RFmxNRMXSemOffsetRbwFilterType.Gaussian, RFmxNRMXSemOffsetRbwFilterType.Gaussian},
			.bandwidthIntegral = New Integer(NumberOfOffsets - 1) {3, 4, 4, 4},
			.offsetLimitFailMask = New RFmxNRMXSemOffsetLimitFailMask(NumberOfOffsets - 1) {RFmxNRMXSemOffsetLimitFailMask.Absolute, RFmxNRMXSemOffsetLimitFailMask.Absolute, RFmxNRMXSemOffsetLimitFailMask.Absolute, RFmxNRMXSemOffsetLimitFailMask.Absolute},
			.absoluteLimitStart = New Double(NumberOfOffsets - 1) {-22.5, -8.5, -11.5, -23.5},
			.absoluteLimitStop = New Double(NumberOfOffsets - 1) {-22.5, -8.5, -11.5, -23.5},
			.relativeLimitStart = New Double(NumberOfOffsets - 1) {-53.0, -53.0, -53.0, -53.0},
			.relativeLimitStop = New Double(NumberOfOffsets - 1) {-60.0, -60.0, -60.0, -60.0}
		}, New SubblockMeasurementInput() With {
			.subblockFrequency = 200000000.0,
			.componentCarrierSpacingType = RFmxNRMXComponentCarrierSpacingType.Nominal,
			.channelRaster = 15000.0,
			.componentCarrierAtCenterFrequency = -1,
			.componentCarrierBandwidth = New Double(NumberOfComponentCarriers - 1) {100000000.0, 100000000.0},
			.componentCarrierFrequency = New Double(NumberOfComponentCarriers - 1) {-49980000.0, 50010000.0},
			.componentCarrierRatedOutputPower = New Double(NumberOfComponentCarriers - 1) {0.0, 0.0},
			.offsetStartFrequency = New Double(NumberOfOffsets - 1) {15000.0, 1500000.0, 5500000.0, 20500000.0},
			.offsetStopFrequency = New Double(NumberOfOffsets - 1) {985000.0, 4500000.0, 19500000.0, 24500000.0},
			.offsetSideband = New RFmxNRMXSemOffsetSideband(NumberOfOffsets - 1) {RFmxNRMXSemOffsetSideband.Both, RFmxNRMXSemOffsetSideband.Both, RFmxNRMXSemOffsetSideband.Both, RFmxNRMXSemOffsetSideband.Both},
			.offsetRbw = New Double(NumberOfOffsets - 1) {10000.0, 250000.0, 250000.0, 250000.0},
			.offsetRbwFilterType = New RFmxNRMXSemOffsetRbwFilterType(NumberOfOffsets - 1) {RFmxNRMXSemOffsetRbwFilterType.Gaussian, RFmxNRMXSemOffsetRbwFilterType.Gaussian, RFmxNRMXSemOffsetRbwFilterType.Gaussian, RFmxNRMXSemOffsetRbwFilterType.Gaussian},
			.bandwidthIntegral = New Integer(NumberOfOffsets - 1) {3, 4, 4, 4},
			.offsetLimitFailMask = New RFmxNRMXSemOffsetLimitFailMask(NumberOfOffsets - 1) {RFmxNRMXSemOffsetLimitFailMask.Absolute, RFmxNRMXSemOffsetLimitFailMask.Absolute, RFmxNRMXSemOffsetLimitFailMask.Absolute, RFmxNRMXSemOffsetLimitFailMask.Absolute},
			.absoluteLimitStart = New Double(NumberOfOffsets - 1) {-22.5, -8.5, -11.5, -23.5},
			.absoluteLimitStop = New Double(NumberOfOffsets - 1) {-22.5, -8.5, -11.5, -23.5},
			.relativeLimitStart = New Double(NumberOfOffsets - 1) {-53.0, -53.0, -53.0, -51.5},
			.relativeLimitStop = New Double(NumberOfOffsets - 1) {-60.0, -60.0, -60.0, -58.5}
		}}
		End Sub

	Private Sub InitializeInstr()
		instrSession = New RFmxInstrMX(resourceName, "")
	End Sub

	Private Sub ConfigureNR()
		NR = instrSession.GetNRSignalConfiguration()
		instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
		NR.SetSelectedPorts("", selectedPorts)
		NR.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
		NR.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger)

		NR.SetNumberOfSubblocks("", NumberOfSubblocks)
		NR.SetLinkDirection("", linkDirection)

		For i As Integer = 0 To NumberOfSubblocks - 1
			subblockString = RFmxNRMX.BuildSubblockString("", i)
			NR.SetFrequencyRange(subblockString, frequencyRange)
			NR.SetComponentCarrierSpacingType(subblockString, subblockInput(i).componentCarrierSpacingType)
			NR.SetChannelRaster(subblockString, subblockInput(i).channelRaster)
			NR.SetComponentCarrierAtCenterFrequency(subblockString, subblockInput(i).componentCarrierAtCenterFrequency)
			NR.SetSubblockFrequency(subblockString, subblockInput(i).subblockFrequency)
			NR.ComponentCarrier.SetNumberOfComponentCarriers(subblockString, NumberOfComponentCarriers)

			For j As Integer = 0 To NumberOfComponentCarriers - 1
				carrierString = RFmxNRMX.BuildCarrierString(subblockString, j)
				NR.ComponentCarrier.SetBandwidth(carrierString, subblockInput(i).componentCarrierBandwidth(j))
				NR.ComponentCarrier.SetFrequency(carrierString, subblockInput(i).componentCarrierFrequency(j))
			Next

			carrierString = RFmxNRMX.BuildCarrierString(subblockString, -1)
			NR.ComponentCarrier.SetBandwidthPartSubcarrierSpacing(carrierString, subcarrierSpacing)
		Next

		NR.SelectMeasurements("", RFmxNRMXMeasurementTypes.Sem, True)

		If linkDirection = RFmxNRMXLinkDirection.Uplink Then
			NR.Sem.Configuration.ConfigureUplinkMaskType("", uplinkMaskType)
		Else
			NR.ConfiguregNodeBCategory("", gNodeBCategory)
			NR.Sem.Configuration.SetDownlinkMaskType("", downlinkMaskType)
			NR.Sem.Configuration.SetDeltaFMaximum("", deltaFMaximum)
			subblockString = RFmxNRMX.BuildSubblockString("", -1)
			NR.SetBand(subblockString, band)
		End If

		For i As Integer = 0 To NumberOfSubblocks - 1
			subblockString = RFmxNRMX.BuildSubblockString("", i)
			If linkDirection = RFmxNRMXLinkDirection.Downlink Then
				NR.Sem.Configuration.ComponentCarrier.ConfigureRatedOutputPowerArray(subblockString, subblockInput(i).componentCarrierRatedOutputPower)
			End If
			NR.Sem.Configuration.ConfigureNumberOfOffsets(subblockString, NumberOfOffsets)
			NR.Sem.Configuration.ConfigureOffsetFrequencyArray(subblockString, subblockInput(i).offsetStartFrequency, subblockInput(i).offsetStopFrequency, subblockInput(i).offsetSideband)
			NR.Sem.Configuration.ConfigureOffsetRbwFilterArray(subblockString, subblockInput(i).offsetRbw, subblockInput(i).offsetRbwFilterType)
			NR.Sem.Configuration.ConfigureOffsetBandwidthIntegralArray(subblockString, subblockInput(i).bandwidthIntegral)
			NR.Sem.Configuration.ConfigureOffsetLimitFailMaskArray(subblockString, subblockInput(i).offsetLimitFailMask)
			NR.Sem.Configuration.ConfigureOffsetAbsoluteLimitArray(subblockString, subblockInput(i).absoluteLimitStart, subblockInput(i).absoluteLimitStop)
			NR.Sem.Configuration.ConfigureOffsetRelativeLimitArray(subblockString, subblockInput(i).relativeLimitStart, subblockInput(i).relativeLimitStop)
		Next

		NR.Sem.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval)
		NR.Sem.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)
		NR.Initiate("", "")
	End Sub

	Private Sub RetrieveResults()
		For i As Integer = 0 To NumberOfSubblocks - 1
			subblockString = RFmxNRMX.BuildSubblockString("", i)

			NR.Sem.Results.FetchUpperOffsetMarginArray(subblockString, timeout, subblockOutput(i).upperOffsetMeasurementStatus, subblockOutput(i).upperOffsetMargin, subblockOutput(i).upperOffsetMarginFrequency, subblockOutput(i).upperOffsetMarginAbsolutePower, _
				subblockOutput(i).upperOffsetMarginRelativePower)

			NR.Sem.Results.FetchLowerOffsetMarginArray(subblockString, timeout, subblockOutput(i).lowerOffsetMeasurementStatus, subblockOutput(i).lowerOffsetMargin, subblockOutput(i).lowerOffsetMarginFrequency, subblockOutput(i).lowerOffsetMarginAbsolutePower, _
				subblockOutput(i).lowerOffsetMarginRelativePower)

			NR.Sem.Results.FetchSubblockMeasurement(subblockString, timeout, subblockOutput(i).subblockPower, subblockOutput(i).integrationBandwidth, subblockOutput(i).frequency)
		Next

		NR.Sem.Results.FetchTotalAggregatedPower("", timeout, totalAggregatedPower)
		NR.Sem.Results.FetchMeasurementStatus("", timeout, measurementStatus)
		NR.Sem.Results.FetchSpectrum("", timeout, spectrum, absoluteMask)
	End Sub

	Private Sub PrintResults()
		Console.WriteLine("Total Aggregated Power (dBm)    :{0}", totalAggregatedPower)
		Console.WriteLine("Measurement Status              :{0}", measurementStatus)
		Console.WriteLine(vbLf & "--------------------Subblock Measurements--------------------")
			For i As Integer = 0 To NumberOfSubblocks - 1
				Console.WriteLine(vbLf & "Subblock {0}" & vbLf, i)

				Console.WriteLine("Subblock Power (dBm)            :{0}", subblockOutput(i).subblockPower)
				Console.WriteLine("Integration Bandwidth (Hz)      :{0}", subblockOutput(i).integrationBandwidth)
				Console.WriteLine("Frequency (Hz)                  :{0}", subblockOutput(i).frequency)

				Console.WriteLine(vbLf & "Offset Segment Measurements")
				For j As Integer = 0 To subblockOutput(i).lowerOffsetMargin.Length - 1
					Console.WriteLine(vbLf & "Lower Offset Segement Measurement {0}", j)

					Console.WriteLine("Measurement Status              :{0}", subblockOutput(i).lowerOffsetMeasurementStatus(j))
					Console.WriteLine("Margin (dB)                     :{0}", subblockOutput(i).lowerOffsetMargin(j))
					Console.WriteLine("Margin Frequency (Hz)           :{0}", subblockOutput(i).lowerOffsetMarginFrequency(j))
					Console.WriteLine("Margin Absolute Power (dBm)     :{0}", subblockOutput(i).lowerOffsetMarginAbsolutePower(j))

					Console.WriteLine(vbLf & "Upper Offset Segement Measurement {0}", j)

					Console.WriteLine("Measurement Status              :{0}", subblockOutput(i).upperOffsetMeasurementStatus(j))
					Console.WriteLine("Margin (dB)                     :{0}", subblockOutput(i).upperOffsetMargin(j))
					Console.WriteLine("Margin Frequency (Hz)           :{0}", subblockOutput(i).upperOffsetMarginFrequency(j))
					Console.WriteLine("Margin Absolute Power (dBm)     :{0}", subblockOutput(i).upperOffsetMarginAbsolutePower(j))
				Next
				Console.WriteLine(vbLf & "-----------------------------------------------------")
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
