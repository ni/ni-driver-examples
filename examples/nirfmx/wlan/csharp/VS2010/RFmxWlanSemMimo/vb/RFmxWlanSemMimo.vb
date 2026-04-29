'Steps:
'1. Open a new RFmx session.
'2. Configure the Frequency Reference properties(Clock Source and Clock Frequency).
'3. Configure Number of Frequency Segment and Receive Chain.
'4. Configure Center Frequency for each Segment.
'5. Configure the basic signal port specific properties(Reference Level and External Attenuation).
'6. Configure Selected Port.
'7. Configure IQ Power Edge Trigger properties(Trigger Delay, IQ Power Edge Level, Min Quiet Time).
'8. Configure Standard and Channel Bandwidth Properties.
'9. Select SEM measurement and enable the traces.
'10. Configure SEM Mask Type.
'11. Configure Averaging parameters.
'12. Configure Sweep Time and Span parameters.
'13. Initiate Measurement.
'14. Fetch SEM Traces and Measurements.
'15. Close the RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.WlanMX

Public Class RFmxWlanSemMimo
	Private instrSession As RFmxInstrMX
	Private wlan As RFmxWlanMX
	Private resourceName As String()
	Private numberOfDevices As Integer

	Private selectedPorts As String()

	Private frequencyReferenceSource As String
	Private frequencyReferenceFrequency As Double

	Private numberOfFrequencySegments As Integer
	Private numberOfReceiveChains As Integer

	Private segmentString As String
	Private chainString As String

	Private centerFrequency As Double()
	Private referenceLevel As Double()
	Private externalAttenuation As Double()

	Private portString As String()

	Private selectedPortsString As String()

	Private iqPowerEdgeEnabled As Boolean
	Private iqPowerEdgeLevel As Double
	Private triggerDelay As Double
	Private minimumQuietTimeMode As RFmxWlanMXTriggerMinimumQuietTimeMode
	Private minimumQuietTime As Double

	Private standard As RFmxWlanMXStandard

	Private channelBandwidth As Double

	Private averagingEnabled As RFmxWlanMXSemAveragingEnabled
	Private averagingCount As Integer
	Private averagingType As RFmxWlanMXSemAveragingType

	Private sweepTimeAuto As RFmxWlanMXSemSweepTimeAuto
	Private sweepTime As Double

	Private spanAuto As RFmxWlanMXSemSpanAuto
	Private span As Double

	Private timeout As Double

	Private measurementStatus As RFmxWlanMXSemMeasurementStatus

	Private absolutePower As Double(,)
	'(dBm) 
	Private relativePower As Double(,)
	'(dBm) 

	Private upperOffsetMeasurementStatus As RFmxWlanMXSemUpperOffsetMeasurementStatus(,)()
	Private upperOffsetMargin As Double(,)()
	'(dB) 
	Private upperOffsetMarginFrequency As Double(,)()
	'(Hz) 
	Private upperOffsetMarginAbsolutePower As Double(,)()
	'(dBm) 
	Private upperOffsetMarginRelativePower As Double(,)()
	'(dBm) 

	Private lowerOffsetMeasurementStatus As RFmxWlanMXSemLowerOffsetMeasurementStatus(,)()
	Private lowerOffsetMargin As Double(,)()
	'(dB) 
	Private lowerOffsetMarginFrequency As Double(,)()
	'(Hz) 
	Private lowerOffsetMarginAbsolutePower As Double(,)()
	'(dBm) 
	Private lowerOffsetMarginRelativePower As Double(,)()
	'(dBm) 

	Private spectrum As Spectrum(Of Single)(,)
	Private compositeMask As Spectrum(Of Single)(,)

	Public Sub Run()
		Try
			InitializeVariables()
			InitializeInstr()
			ConfigureWlan()
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
		resourceName = New String() {"RFSA1", "RFSA2"}
		numberOfDevices = resourceName.GetLength(0)

        selectedPorts = New String() {"", ""}

		frequencyReferenceSource = RFmxInstrMXConstants.PxiClock
		frequencyReferenceFrequency = 10000000.0
		' (Hz) 

		numberOfFrequencySegments = 1
		numberOfReceiveChains = 2

		centerFrequency = New Double() {5180000000.0, 5260000000.0}
		' (Hz) 
		referenceLevel = New Double() {0.0, 0.0}
		' (dBm) 
		externalAttenuation = New Double() {0.0, 0.0}
		' (dB) 

		portString = New String(numberOfDevices - 1) {}

		selectedPortsString = New String(numberOfDevices - 1) {}

		iqPowerEdgeEnabled = True
		iqPowerEdgeLevel = -20.0
		'(dB) 
		triggerDelay = 0.0
		' (s) 
		minimumQuietTimeMode = RFmxWlanMXTriggerMinimumQuietTimeMode.Auto
		minimumQuietTime = 5E-06
		' (s) 

		standard = RFmxWlanMXStandard.Standard802_11n

		channelBandwidth = 20000000.0
		'(Hz) 

		averagingEnabled = RFmxWlanMXSemAveragingEnabled.[False]
		averagingCount = 10
		averagingType = RFmxWlanMXSemAveragingType.Rms

		sweepTimeAuto = RFmxWlanMXSemSweepTimeAuto.[True]
		sweepTime = 0.001
		' (s) 

		spanAuto = RFmxWlanMXSemSpanAuto.[True]
		span = 66000000.0
		'(Hz) 

		timeout = 10.0
		' (s) 

		measurementStatus = New RFmxWlanMXSemMeasurementStatus()

		absolutePower = New Double(numberOfFrequencySegments - 1, numberOfReceiveChains - 1) {}
		relativePower = New Double(numberOfFrequencySegments - 1, numberOfReceiveChains - 1) {}

		upperOffsetMeasurementStatus = New RFmxWlanMXSemUpperOffsetMeasurementStatus(numberOfFrequencySegments - 1, numberOfReceiveChains - 1)() {}
		upperOffsetMargin = New Double(numberOfFrequencySegments - 1, numberOfReceiveChains - 1)() {}
		upperOffsetMarginFrequency = New Double(numberOfFrequencySegments - 1, numberOfReceiveChains - 1)() {}
		upperOffsetMarginAbsolutePower = New Double(numberOfFrequencySegments - 1, numberOfReceiveChains - 1)() {}
		upperOffsetMarginRelativePower = New Double(numberOfFrequencySegments - 1, numberOfReceiveChains - 1)() {}

		lowerOffsetMeasurementStatus = New RFmxWlanMXSemLowerOffsetMeasurementStatus(numberOfFrequencySegments - 1, numberOfReceiveChains - 1)() {}
		lowerOffsetMargin = New Double(numberOfFrequencySegments - 1, numberOfReceiveChains - 1)() {}
		lowerOffsetMarginFrequency = New Double(numberOfFrequencySegments - 1, numberOfReceiveChains - 1)() {}
		lowerOffsetMarginAbsolutePower = New Double(numberOfFrequencySegments - 1, numberOfReceiveChains - 1)() {}
		lowerOffsetMarginRelativePower = New Double(numberOfFrequencySegments - 1, numberOfReceiveChains - 1)() {}

		spectrum = New Spectrum(Of Single)(numberOfFrequencySegments - 1, numberOfReceiveChains - 1) {}
		compositeMask = New Spectrum(Of Single)(numberOfFrequencySegments - 1, numberOfReceiveChains - 1) {}
	End Sub

	Private Sub InitializeInstr()
        instrSession = New RFmxInstrMX(resourceName, "")
	End Sub

	Private Sub ConfigureWlan()
		wlan = instrSession.GetWlanSignalConfiguration()
		' Create a new RFmx Session 
		instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
		wlan.ConfigureNumberOfFrequencySegmentsAndReceiveChains("", numberOfFrequencySegments, numberOfReceiveChains)
		For i As Integer = 0 To numberOfFrequencySegments - 1
			segmentString = RFmxWlanMX.BuildSegmentString("", i)
			wlan.ConfigureFrequency(segmentString, centerFrequency(i))
		Next
		For i As Integer = 0 To numberOfDevices - 1
			selectedPortsString(i) = RFmxInstrMX.BuildPortString2("", selectedPorts(i), resourceName(i), 0)
			portString(i) = RFmxInstrMX.BuildPortString2("", "", resourceName(i), 0)
			wlan.ConfigureReferenceLevel(portString(i), referenceLevel(i))
			wlan.ConfigureExternalAttenuation(portString(i), externalAttenuation(i))
		Next
		wlan.ConfigureSelectedPortsMultiple("", selectedPortsString)
		wlan.ConfigureIQPowerEdgeTrigger("", "0", RFmxWlanMXIQPowerEdgeTriggerSlope.Rising, iqPowerEdgeLevel, triggerDelay, minimumQuietTimeMode, _
			minimumQuietTime, RFmxWlanMXIQPowerEdgeTriggerLevelType.Relative, iqPowerEdgeEnabled)
		wlan.ConfigureStandard("", standard)
		wlan.ConfigureChannelBandwidth("", channelBandwidth)
		wlan.SelectMeasurements("", RFmxWlanMXMeasurementTypes.Sem, True)
		wlan.Sem.Configuration.ConfigureMaskType("", RFmxWlanMXSemMaskType.Standard)
		wlan.Sem.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)
		wlan.Sem.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTime)
		wlan.Sem.Configuration.ConfigureSpan("", spanAuto, span)
		wlan.Initiate("", "")
	End Sub

	Private Sub RetrieveResults()
		For i As Integer = 0 To numberOfFrequencySegments - 1
			segmentString = RFmxWlanMX.BuildSegmentString("", i)
			For j As Integer = 0 To numberOfReceiveChains - 1
				chainString = RFmxWlanMX.BuildChainString(segmentString, j)
				wlan.Sem.Results.FetchCarrierMeasurement(chainString, timeout, absolutePower(i, j), relativePower(i, j))
				wlan.Sem.Results.FetchLowerOffsetMarginArray(chainString, timeout, lowerOffsetMeasurementStatus(i, j), lowerOffsetMargin(i, j), lowerOffsetMarginFrequency(i, j), lowerOffsetMarginAbsolutePower(i, j), _
					lowerOffsetMarginRelativePower(i, j))
				wlan.Sem.Results.FetchUpperOffsetMarginArray(chainString, timeout, upperOffsetMeasurementStatus(i, j), upperOffsetMargin(i, j), upperOffsetMarginFrequency(i, j), upperOffsetMarginAbsolutePower(i, j), _
					upperOffsetMarginRelativePower(i, j))
				wlan.Sem.Results.FetchSpectrum(chainString, timeout, spectrum(i, j), compositeMask(i, j))
			Next
		Next
		wlan.Sem.Results.FetchMeasurementStatus("", timeout, measurementStatus)

	End Sub

	Private Sub PrintResults()
		Console.WriteLine("Measurement Status                          :{0}", measurementStatus)
		For i As Integer = 0 To numberOfFrequencySegments - 1
			segmentString = RFmxWlanMX.BuildSegmentString("", i)
			For j As Integer = 0 To numberOfReceiveChains - 1
				chainString = RFmxWlanMX.BuildChainString(segmentString, j)

				Console.WriteLine(vbLf & "-------Measurement for {0}-------" & vbLf & vbLf, chainString)
				Console.WriteLine("Carrier Absolute Power (dBm)                :{0}", absolutePower(i, j))

				Console.WriteLine(vbLf & "----------Lower Offset Measurements----------" & vbLf)
				For k As Integer = 0 To lowerOffsetMargin(i, j).Length - 1
					Console.WriteLine("Offset {0}", k)
					Console.WriteLine("Measurement Status              :{0}", lowerOffsetMeasurementStatus(i, j)(k))
					Console.WriteLine("Margin (dB)                     :{0}", lowerOffsetMargin(i, j)(k))
					Console.WriteLine("Margin Frequency (Hz)           :{0}", lowerOffsetMarginFrequency(i, j)(k))
					Console.WriteLine("Margin Absolute Power (dBm)     :{0}" & vbLf, lowerOffsetMarginAbsolutePower(i, j)(k))
				Next

				Console.WriteLine(vbLf & "----------Upper Offset Measurements----------" & vbLf)
				For k As Integer = 0 To upperOffsetMargin(i, j).Length - 1
					Console.WriteLine("Offset {0}", k)
					Console.WriteLine("Measurement Status              :{0}", upperOffsetMeasurementStatus(i, j)(k))
					Console.WriteLine("Margin (dB)                     :{0}", upperOffsetMargin(i, j)(k))
					Console.WriteLine("Margin Frequency (Hz)           :{0}", upperOffsetMarginFrequency(i, j)(k))
					Console.WriteLine("Margin Absolute Power (dBm)     :{0}" & vbLf, upperOffsetMarginAbsolutePower(i, j)(k))
				Next
			Next
		Next
	End Sub

	Private Sub CloseSession()
		If wlan IsNot Nothing Then
			wlan.Dispose()
			wlan = Nothing
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
