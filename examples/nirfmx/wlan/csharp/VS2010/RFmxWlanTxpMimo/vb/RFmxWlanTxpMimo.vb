'Steps:
'1. Open a new RFmx session.
'2. Configure the frequency reference properties (Clock Source and Clock Frequency).
'3. Configure Number of Frequency Segment and Receive Chain.
'4. Configure the Center Frequency for each Segment.
'5. Configure Selected Port.
'6. Configure Standard and Channel Bandwidth Properties.
'7. Configure Reference Level.
'8. Configure the External Attenuation.
'9. Configure IQ Power Edge Trigger properties (Trigger Delay, IQ Power Edge Level, Minimum Quiet Time).
'10. Select TXP measurement and enable the traces.
'11. Configure the Measurement Interval.
'12. Configure Averaging parameters.
'13. Initiate Measurement.
'14. Fetch TXP Traces and Measurements.
'15. Close the RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.WlanMX

Public Class RFmxWlanTxpMimo
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

	Private portString As String()

	Private selectedPortsString As String()

	Private standard As RFmxWlanMXStandard

	Private channelBandwidth As Double

	Private autoLevel As Boolean
	Private measurementInterval As Double
	Private referenceLevel As Double()
	Private externalAttenuation As Double()

	Private iqPowerEdgeEnabled As Boolean
	Private iqPowerEdgeLevel As Double
	Private triggerDelay As Double
	Private minimumQuietTimeMode As RFmxWlanMXTriggerMinimumQuietTimeMode
	Private minimumQuietTime As Double

	Private averagingEnabled As RFmxWlanMXTxpAveragingEnabled
	Private averagingCount As Integer

	Private timeout As Double

	Private maximumMeasurementInterval As Double

	Private power As AnalogWaveform(Of Single)(,)

	Private averagePowerMean As Double(,), peakPowerMaximum As Double(,)

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

		portString = New String(numberOfDevices - 1) {}

		selectedPortsString = New String(numberOfDevices - 1) {}

		standard = RFmxWlanMXStandard.Standard802_11n

		channelBandwidth = 20000000.0
		' (Hz) 

		autoLevel = True
		measurementInterval = 0.01
		' (s) 
		referenceLevel = New Double() {0.0, 0.0}
		' (dBm) 
		externalAttenuation = New Double() {0.0, 0.0}
		' (dB) 

		iqPowerEdgeEnabled = True
		iqPowerEdgeLevel = -20.0
		' (dB) 
		triggerDelay = 0.0
		' (s) 
		minimumQuietTimeMode = RFmxWlanMXTriggerMinimumQuietTimeMode.Auto
		minimumQuietTime = 5E-06
		' (s) 

		averagingEnabled = RFmxWlanMXTxpAveragingEnabled.[False]
		averagingCount = 10

		maximumMeasurementInterval = 0.001
		' (s) 

		timeout = 10.0
		' (s) 

		power = New AnalogWaveform(Of Single)(numberOfFrequencySegments - 1, numberOfReceiveChains - 1) {}
		averagePowerMean = New Double(numberOfFrequencySegments - 1, numberOfReceiveChains - 1) {}
		' (dBm) 
		peakPowerMaximum = New Double(numberOfFrequencySegments - 1, numberOfReceiveChains - 1) {}
		' (dBm) 
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
		Next
		wlan.ConfigureSelectedPortsMultiple("", selectedPortsString)
		wlan.ConfigureStandard("", standard)
		wlan.ConfigureChannelBandwidth("", channelBandwidth)
		If autoLevel Then
			wlan.AutoLevel("", measurementInterval)
		Else
			For i As Integer = 0 To numberOfDevices - 1
				wlan.ConfigureReferenceLevel(portString(i), referenceLevel(i))
			Next
		End If
		For i As Integer = 0 To numberOfDevices - 1
			wlan.ConfigureExternalAttenuation(portString(i), externalAttenuation(i))
		Next
		wlan.ConfigureIQPowerEdgeTrigger("", "0", RFmxWlanMXIQPowerEdgeTriggerSlope.Rising, iqPowerEdgeLevel, triggerDelay, minimumQuietTimeMode, _
			minimumQuietTime, RFmxWlanMXIQPowerEdgeTriggerLevelType.Relative, iqPowerEdgeEnabled)
		wlan.SelectMeasurements("", RFmxWlanMXMeasurementTypes.Txp, True)
		wlan.Txp.Configuration.ConfigureMaximumMeasurementInterval("", maximumMeasurementInterval)
		wlan.Txp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount)
		wlan.Initiate("", "")
	End Sub

	Private Sub RetrieveResults()
		For i As Integer = 0 To numberOfFrequencySegments - 1
			segmentString = RFmxWlanMX.BuildSegmentString("", i)
			For j As Integer = 0 To numberOfReceiveChains - 1
				chainString = RFmxWlanMX.BuildChainString(segmentString, j)
				wlan.Txp.Results.FetchMeasurement(chainString, timeout, averagePowerMean(i, j), peakPowerMaximum(i, j))
				wlan.Txp.Results.FetchPowerTrace(chainString, timeout, power(i, j))
			Next
		Next
	End Sub

	Private Sub PrintResults()
		For i As Integer = 0 To numberOfFrequencySegments - 1
			segmentString = RFmxWlanMX.BuildSegmentString("", i)
			For j As Integer = 0 To numberOfReceiveChains - 1
				chainString = RFmxWlanMX.BuildChainString(segmentString, j)
				Console.WriteLine(vbLf & "----------Measurement for {0}----------" & vbLf, chainString)
				Console.WriteLine("Average Power Mean (dBm)         :{0}", averagePowerMean(i, j))
				Console.WriteLine("Peak Power Maximum (dBm)         :{0}", peakPowerMaximum(i, j))
				Console.WriteLine()
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
