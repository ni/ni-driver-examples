'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure Selected Ports.
'4. Configure basic signal properties(Center Frequency, RF Attenuation and External Attenuation).
'5. Configure Trigger Parameters for Digital Edge Trigger.
'6. Configure Link Direction and Number of Subblocks.
'7. Configure Frequency Range, Center Frequency, Subblock Frequency Definition, Component Carrier Spacing Type,
'   Channel Raster, Component Carrier Center Frequency and Number of Component Carriers.
'8. Configure Subcarrier Spacing.
'9. Configure Component Carriers.
'10. Configure Reference Level.
'11. Select ACP measurement and enable Traces.
'12. Configure Measurement Method.
'13. Configure Noise Compensation Parameter.
'14. Configure Sweep Time Parameters.
'15. Configure Averaging Parameters for ACP measurement.
'16. Initiate the Measurement.
'17. Fetch ACP Measurements and Traces.
'18. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.NRMX

Namespace NationalInstruments.Examples.RFmxNRAcpNonContiguousMultiCarrier
Public Class RFmxNRAcpNonContiguousMultiCarrier
	Private instrSession As RFmxInstrMX
	Private NR As RFmxNRMX

	Private resourceName As String
	Private selectedPorts As String
	Private centerFrequency As Double
	Private externalAttenuation As Double

	Private autoLevel As Boolean
	Private referenceLevel As Double
	Private measurementInterval As Double

	Private rfAttenuationAuto As RFmxInstrMXRFAttenuationAuto
	Private rfAttenuation As Double

	Private frequencyReferenceSource As String
	Private frequencyReferenceFrequency As Double

	Private enableTrigger As Boolean
	Private digitalEdgeSource As String
	Private digitalEdge As RFmxNRMXDigitalEdgeTriggerEdge
	Private triggerDelay As Double

	Private linkDirection As RFmxNRMXLinkDirection
	Private frequencyRange As RFmxNRMXFrequencyRange

	Const NumberOfSubblocks As Integer = 2
	Const NumberOfComponentCarriers As Integer = 2

	Private subblockFrequency As Double() = New Double(NumberOfSubblocks - 1) {}
	Private componentCarrierSpacingType As RFmxNRMXComponentCarrierSpacingType() = New RFmxNRMXComponentCarrierSpacingType(NumberOfSubblocks - 1) {}
	Private channelRaster As Double() = New Double(NumberOfSubblocks - 1) {}
	Private componentCarrierAtCenterFrequency As Integer() = New Integer(NumberOfSubblocks - 1) {}

	Private componentCarrierBandwidth As Double(,) = New Double(NumberOfSubblocks - 1, NumberOfComponentCarriers - 1) {}
	Private componentCarrierFrequency As Double(,) = New Double(NumberOfSubblocks - 1, NumberOfComponentCarriers - 1) {}

	Private subcarrierSpacing As Double
	Private measurementMethod As RFmxNRMXAcpMeasurementMethod
	Private noiseCompensationEnabled As RFmxNRMXAcpNoiseCompensationEnabled

	Private sweepTimeAuto As RFmxNRMXAcpSweepTimeAuto
	Private sweepTimeInterval As Double

	Private averagingEnabled As RFmxNRMXAcpAveragingEnabled
	Private averagingCount As Integer
	Private averagingType As RFmxNRMXAcpAveragingType

	Private subblockString As String
	Private carrierString As String

	Private timeout As Double

	Private totalAggregatedPower As Double
	' (dBm or dBm/Hz) 

	' Subblock measurement outputs structure 

	Private Structure SubblockMeasurementOutput
		Public subblockPower As Double
		' (dBm or dBm/Hz) 
		Public integrationBandwidth As Double
		' (Hz) 
		Public frequency As Double
		' (Hz) 
		Public lowerRelativePower As Double()
		' (dB) 
		Public upperRelativePower As Double()
		' (dB) 
		Public lowerAbsolutePower As Double()
		' (dBm) 
		Public upperAbsolutePower As Double()
		' (dBm) 
	End Structure

	Private subblockOutput As SubblockMeasurementOutput() = New SubblockMeasurementOutput(NumberOfSubblocks - 1) {}

	Private spectrum As Spectrum(Of Single)
	Private relativePowersTrace As Spectrum(Of Single)() = New Spectrum(Of Single)(NumberOfSubblocks - 1) {}

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
			Console.WriteLine("Press any key to exit")
			Console.ReadKey()
		End Try
	End Sub

	Private Sub InitializeVariables()
		resourceName = "RFSA"

		selectedPorts = ""
		centerFrequency = 3500000000.0
		' (Hz) 
		externalAttenuation = 0.0
		' (dB) 

		autoLevel = True
		referenceLevel = 0.0
		' (dBm) 
		measurementInterval = 0.01
		' (s) 

		rfAttenuationAuto = RFmxInstrMXRFAttenuationAuto.[True]
		rfAttenuation = 10.0
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
		measurementMethod = RFmxNRMXAcpMeasurementMethod.Normal
		noiseCompensationEnabled = RFmxNRMXAcpNoiseCompensationEnabled.[False]

		sweepTimeAuto = RFmxNRMXAcpSweepTimeAuto.[True]
		sweepTimeInterval = 0.001
		' (s) 

		averagingEnabled = RFmxNRMXAcpAveragingEnabled.[False]
		averagingCount = 10
		averagingType = RFmxNRMXAcpAveragingType.Rms

		timeout = 10.0
		' (s) 
	End Sub

	Private Sub InitializeInstr()
		' Create a new RFmx Session 

		instrSession = New RFmxInstrMX(resourceName, "")
	End Sub

	Private Sub ConfigureNR()
		NR = instrSession.GetNRSignalConfiguration()
		instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
		NR.SetSelectedPorts("", selectedPorts)
		NR.ConfigureFrequency("", centerFrequency)
		NR.ConfigureExternalAttenuation("", externalAttenuation)
		instrSession.ConfigureRFAttenuation("", rfAttenuationAuto, rfAttenuation)
		NR.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger)
		NR.SetLinkDirection("", linkDirection)
		NR.SetNumberOfSubblocks("", NumberOfSubblocks)

		For i As Integer = 0 To NumberOfSubblocks - 1
			subblockString = RFmxNRMX.BuildSubblockString("", i)
			NR.SetFrequencyRange(subblockString, frequencyRange)
			NR.SetSubblockFrequency(subblockString, subblockFrequency(i))
			NR.SetChannelRaster(subblockString, channelRaster(i))
			NR.SetComponentCarrierSpacingType(subblockString, componentCarrierSpacingType(i))
			NR.SetComponentCarrierAtCenterFrequency(subblockString, componentCarrierAtCenterFrequency(i))
			NR.ComponentCarrier.SetNumberOfComponentCarriers(subblockString, NumberOfComponentCarriers)

			carrierString = RFmxNRMX.BuildCarrierString(subblockString, -1)
			NR.ComponentCarrier.SetBandwidthPartSubcarrierSpacing(carrierString, subcarrierSpacing)

			For j As Integer = 0 To NumberOfComponentCarriers - 1
				carrierString = RFmxNRMX.BuildCarrierString(subblockString, j)
				NR.ComponentCarrier.SetBandwidth(carrierString, componentCarrierBandwidth(i, j))
				NR.ComponentCarrier.SetFrequency(carrierString, componentCarrierFrequency(i, j))
			Next
		Next

		If autoLevel Then
			NR.AutoLevel("", measurementInterval, referenceLevel)
			Console.WriteLine("Reference level (dBm)           : {0}" & vbLf, referenceLevel)
		Else
			NR.ConfigureReferenceLevel("", referenceLevel)
		End If

		NR.SelectMeasurements("", RFmxNRMXMeasurementTypes.Acp, True)
		NR.Acp.Configuration.ConfigureMeasurementMethod("", measurementMethod)
		NR.Acp.Configuration.ConfigureNoiseCompensationEnabled("", noiseCompensationEnabled)
		NR.Acp.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval)
		NR.Acp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)
		NR.Initiate("", "")
	End Sub

	Private Sub RetrieveResults()
		For i As Integer = 0 To NumberOfSubblocks - 1
			subblockString = RFmxNRMX.BuildSubblockString("", i)

			NR.Acp.Results.FetchSubblockMeasurement(subblockString, timeout, subblockOutput(i).subblockPower, subblockOutput(i).integrationBandwidth, subblockOutput(i).frequency)

			NR.Acp.Results.FetchOffsetMeasurementArray(subblockString, timeout, subblockOutput(i).lowerRelativePower, subblockOutput(i).upperRelativePower, subblockOutput(i).lowerAbsolutePower, subblockOutput(i).upperAbsolutePower)
		Next

		NR.Acp.Results.FetchTotalAggregatedPower("", timeout, totalAggregatedPower)

		For i As Integer = 0 To NumberOfSubblocks - 1
			NR.Acp.Results.FetchRelativePowersTrace("", timeout, i, relativePowersTrace(i))
		Next

		NR.Acp.Results.FetchSpectrum("", timeout, spectrum)

	End Sub

	Private Sub PrintResults()
		Console.WriteLine("Total Aggregated Power (dBm or dBm/Hz)    : {0}", totalAggregatedPower)
		Console.WriteLine(vbLf & "-----------Subblock Measurements------------" & vbLf)
		For i As Integer = 0 To NumberOfSubblocks - 1
			Console.WriteLine("Subblock  : {0}", i)
			Console.WriteLine("Subblock Power (dBm or dBm/Hz)     : {0}", subblockOutput(i).subblockPower)
			Console.WriteLine("Integration Bandwidth (Hz)         : {0}", subblockOutput(i).integrationBandwidth)
			Console.WriteLine("Frequency (Hz)                     : {0}", subblockOutput(i).frequency)
			Console.WriteLine(vbLf & "-----------Offset Channel Measurements------------" & vbLf)
			For j As Integer = 0 To subblockOutput(i).lowerRelativePower.Length - 1
				Console.WriteLine("Offset  : {0}", j)
				Console.WriteLine("Lower Relative Power (dB)       : {0}", subblockOutput(i).lowerRelativePower(j))
				Console.WriteLine("Upper Relative Power (dB)       : {0}", subblockOutput(i).upperRelativePower(j))
				Console.WriteLine("Lower Absolute Power (dBm)      : {0}", subblockOutput(i).lowerAbsolutePower(j))
				Console.WriteLine("Upper Absolute Power (dBm)      : {0}", subblockOutput(i).upperAbsolutePower(j))
				Console.WriteLine("---------------------------------------------------" & vbLf)
			Next
		Next
	End Sub

	Private Sub CloseSession()
		Try
			If NR IsNot Nothing Then
				NR.Dispose()
				NR = Nothing
			End If
			If instrSession IsNot Nothing Then
				instrSession.Close()
				instrSession = Nothing
			End If
		Catch ex As Exception
			DisplayError(ex)
		End Try
	End Sub

	Private Shared Sub DisplayError(ex As Exception)
		Console.WriteLine("ERROR:" & vbLf & Convert.ToString(ex.[GetType]()) & ": " & ex.Message)
	End Sub

End Class
End Namespace
