'Steps:
'1.  Open NI - RFSG session. 
'2.  Configure RFSG generation mode to Script. This allows for writing a script that determines the behavior of the generation.
'3.  Configure RFSG Frequency Reference (Clock Source and Clock Frequency).
'4.  Configure RFSG Selected Ports, External Gain and Frequency of RF output signal.
'5.  Get the terminal name for marker0. Use this as the source to configure RFSG List to advance upon receipt of a 
'    marker event.
'6.  Read waveform from file and download Waveform from file to RFSG. 
'7.  Retrieve waveform sample rate from the waveform file.
'8.  Retrieve the value of PAPR from the waveform file.
'9.  Write script to generate a waveform. This script is programmed to continuously generate a waveform of length
'    equal to the RFmx list step duration and generate marker0 at the end of list step acquisition. 
'10.  Create a Configuration List. Pass Power Level in the Configuration List Properties parameter to be able to configure
'    Power Level in each step that we create.The Set As Active List parameter in this method defaults to true, this will set the
'    Active Configuration List property to the name of the created configuration list.Once the Active Configuration List
'    is set, using a property node to access Power Level will modify the property for this configuration list.
'11. Create a Configuration List Step.The Set As Active Step parameter in this method defaults to true, this will set the Active
'    Configuration List Step property to the created configuration list step index. Once the Active Configuration List
'    Step is set, using a property node to access Power Level will modify the property for this configuration list step in
'    the configuration list indicated by the Active Configuration List property.
'12. Configure the Power Level for the Active Configuration List Step in the Active Configuration List.
'13. Open a new RFmx Session.
'14. Configure the Frequency Reference properties(Clock Source and Clock Frequency). 
'15. Create RFmx SpecAn List.
'16. Create RFmx SpecAn List Step.
'17. Configure the Reference Level for the Specified List Step in the SpecAn List.
'18. Configure Trigger Parameters for IQ Power Edge Trigger on List Step0.
'19. Configure Trigger Parameters for Digital Edge Time Trigger on List Step1 to N - 1.
'20. Configure List Step Timer Offset for List Step1 to N - 1
'21. Configure List Step Timer Duration for all List steps.
'22. Configure Center Frequency, Selected Ports, External Attenuation for all Configuration List Step.
'23. Configure Sweep Time, RBW Filter, FFT parameters for all List Steps.
'24. Configure Integration BW of the Carrier channel, Number of Offset Channelsand Channel Spacing for all List Steps.
'25. Configure Carrier and Offset RRC Filter for all List Steps.
'26. Select ACP measurement and disable traces for all List Steps.
'27. Initiate ACP measurement for List.
'28. Initiate signal generation.
'29. Wait for Acquisition to complete.
'30. Fetch ACP measurement Results for all List steps one by one.
'31. Stop signal generation.
'32. Delete the Configuration List to avoid committing the Configuration List to the hardware with subsequent
'    calls to Commit. Deleting the list will reset the Active Configuration List. 
'33. Delete RFmx SpecAn List
'34. Close the RFmx Session.
'35. Close the RFSG session.
'    It is recommended to clear the waveform before closing RFSG session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.SpecAnMX
Imports NationalInstruments.ModularInstruments.NIRfsg
Imports NationalInstruments.ModularInstruments.NIRfsgPlayback

Namespace NationalInstruments.Examples.RFmxSpecAnListModeWithTimeBasedSynchronization
	Public Class RFmxSpecAnListModeWithTimeBasedSynchronization
		Private instrSession As RFmxInstrMX
		Private SpecAnList As RFmxSpecAnMXList
		Private rfsgSession As NIRfsg
		Private instrumentHandle As IntPtr

		Private centerFrequency As Double

		Private rfsgResourceName As String
		Private rfsgSelectedPorts As String
		Private waveformFilePath As String
		Private waveformName As String

		Private rfsgExternalAttenuation As Double

		Private rfsgFrequencyReferenceSource As RfsgFrequencyReferenceSource
		Private rfsgFrequency As Double

		Private rfsaResourceName As String
		Private rfsaSelectedPorts As String
		Private rfsaExternalAttenuation As Double

		Private rfsaFrequencyReferenceSource As String
		Private rfsaFrequency As Double

		Private step0IQPowerEdgeLevel As Double
		Private step0MinimumQuietTime As Double
		Private triggerDelay As Double

		Private numberOfSteps As Integer

		Private listStepTimerDuration As Double
		Private listStepTimerOffset As Double

		Private startReferenceLevel As Double
		Private stopReferenceLevel As Double

		Private integrationBandwidth As Double
		Private channelSpacing As Double
		Private sweepTimeInterval As Double

		Private rampPattern As Double()
		Private timeout As Double
		Private script As String
		Private markerEventTerminalName As String
		Private sampleRate As Double
		Private numberOfSamples As Integer
		Private markerLocation As Integer
		Private papr As Double
		Private [step] As RFmxSpecAnMX()
		Private absolutePower As Double()

		Private offsetString As String
		Private carrierString As String

		Private numberOfOffsetChannels As Integer

		Private Structure OffsetMeasurement
			Public lowerRelativePower As Double()
			Public upperRelativePower As Double()
			Public lowerAbsolutePower As Double()
			Public upperAbsolutePower As Double()
		End Structure
		Private offsetMeasurementObject As OffsetMeasurement()


		Public Sub Run()
			Try
				InitializeVariables()
				ConfigureRfsg()
				ConfigureRFmx()
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
			centerFrequency = 1950000000.0
			' (Hz) 

			rfsgResourceName = "RFSG"
			rfsgSelectedPorts = ""
			waveformFilePath = "WCDMA_Uplink_DPCH_Waveform.tdms"
			waveformName = "Wfm"

			rfsgExternalAttenuation = 0.0
			' (dB) 

			rfsgFrequencyReferenceSource = RfsgFrequencyReferenceSource.OnboardClock
			rfsgFrequency = 10000000.0
			' (Hz) 

			rfsaResourceName = "RFSA"
			rfsaSelectedPorts = ""
			rfsaExternalAttenuation = 0.0
			' (dB) 

			rfsaFrequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
			rfsaFrequency = 10000000.0
			' (Hz) 

			step0IQPowerEdgeLevel = -10
			' (dB) 
			step0MinimumQuietTime = 0.0
			' (s) 
			triggerDelay = 0.0
			' (s) 

			numberOfSteps = 10

			listStepTimerDuration = 0.001
			' (s) 
			listStepTimerOffset = 0.0
			' (s) 

			startReferenceLevel = -20.0
			' (dBm) 
			stopReferenceLevel = 0.0
			' (dBm) 

			integrationBandwidth = 3840000.0
			' (Hz) 
			channelSpacing = 5000000.0
			' (Hz) 
			sweepTimeInterval = 0.00066667
			' (s) 

			numberOfOffsetChannels = 2

			rampPattern = New Double(numberOfSteps - 1) {}
			LinearRampPattern(startReferenceLevel, stopReferenceLevel, numberOfSteps, False, rampPattern)

			timeout = 10.0
			' (s) 

			[step] = New RFmxSpecAnMX(numberOfSteps - 1) {}
			absolutePower = New Double(numberOfSteps - 1) {}
			' (dBm or dBm/Hz) 

		End Sub

		Private Sub ConfigureRfsg()
			rfsgSession = New NIRfsg(rfsgResourceName, True, False)
			rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.Script
			rfsgSession.FrequencyReference.Configure(rfsgFrequencyReferenceSource, rfsgFrequency)

			rfsgSession.RF.ExternalGain = -1 * rfsgExternalAttenuation
			rfsgSession.SignalPath.SelectedPorts = rfsgSelectedPorts
			rfsgSession.RF.Frequency = centerFrequency

			markerEventTerminalName = rfsgSession.DeviceEvents.MarkerEvents(0).TerminalName
			rfsgSession.Triggers.ConfigurationListStepTrigger.DigitalEdge.Configure(markerEventTerminalName, RfsgTriggerEdge.RisingEdge)

			instrumentHandle = rfsgSession.GetInstrumentHandle().DangerousGetHandle()
			rfsgSession.Arb.ReadAndDownloadWaveformFromFileTdms(waveformName, waveformFilePath, 0)
			sampleRate = rfsgSession.Arb.Waveforms(waveformName).IQRate
			papr = rfsgSession.Arb.Waveforms(waveformName).Papr
			numberOfSamples = CInt(Math.Truncate(sampleRate * listStepTimerDuration))
			markerLocation = CInt(Math.Truncate(sampleRate * sweepTimeInterval))
			script = String.Format("script GenerateWaveform" & vbLf & "  repeat forever" & vbLf & "    generate {0} subset(0, {1}) marker0({2})" & vbLf & "  end repeat" & vbLf & "end script", waveformName, numberOfSamples, markerLocation)
			rfsgSession.Arb.Scripting.WriteScript(script)

			Dim properties As RfsgConfigurationListProperties() = New RfsgConfigurationListProperties(0) {RfsgConfigurationListProperties.PowerLevel}
			rfsgSession.BasicConfigurationList.CreateConfigurationList("PowerLevelList", properties, True)
			For i As Integer = 0 To numberOfSteps - 1
				rfsgSession.BasicConfigurationList.CreateStep(True)
				rfsgSession.RF.PowerLevel = rampPattern(i)
			Next
		End Sub

		Private Sub ConfigureRFmx()
			instrSession = New RFmxInstrMX(rfsaResourceName, "")
			instrSession.ConfigureFrequencyReference("", rfsaFrequencyReferenceSource, rfsaFrequency)
			SpecAnList = instrSession.GetSpecAnList("ACP_List")
			For i As Integer = 0 To numberOfSteps - 1
				[step](i) = SpecAnList.CreateListStep()
				[step](i).SetReferenceLevel("", papr + rampPattern(i))

				If i = 0 Then
					[step](i).ConfigureIQPowerEdgeTrigger("", "0", step0IQPowerEdgeLevel, RFmxSpecAnMXIQPowerEdgeTriggerSlope.Rising, triggerDelay, RFmxSpecAnMXTriggerMinimumQuietTimeMode.Manual,
					step0MinimumQuietTime, True)
					[step](i).SetIQPowerEdgeTriggerLevelType("", RFmxSpecAnMXIQPowerEdgeTriggerLevelType.Relative)
				Else
					[step](i).ConfigureDigitalEdgeTrigger("", "TimerEvent", RFmxSpecAnMXDigitalEdgeTriggerEdge.Rising, triggerDelay, True)
					[step](i).SetListStepTimerOffset("", listStepTimerOffset)
				End If
				[step](i).SetListStepTimerDuration("", listStepTimerDuration)
			Next
			Dim stepAll As RFmxSpecAnMX = SpecAnList.GetListStepAll()
			stepAll.ConfigureFrequency("", centerFrequency)
			stepAll.SetSelectedPorts("", rfsaSelectedPorts)
			stepAll.ConfigureExternalAttenuation("", rfsaExternalAttenuation)
			stepAll.Acp.Configuration.SetSweepTimeAuto("", RFmxSpecAnMXAcpSweepTimeAuto.[False])
			stepAll.Acp.Configuration.SetSweepTimeInterval("", sweepTimeInterval)
			stepAll.Acp.Configuration.SetRbwFilterAutoBandwidth("", RFmxSpecAnMXAcpRbwAutoBandwidth.[True])
			stepAll.Acp.Configuration.SetRbwFilterBandwidth("", 38400.0)
			stepAll.Acp.Configuration.SetRbwFilterType("", RFmxSpecAnMXAcpRbwFilterType.FftBased)
			stepAll.Acp.Configuration.SetFftPadding("", 1.0)
			stepAll.Acp.Configuration.SetFftWindow("", RFmxSpecAnMXAcpFftWindow.FlatTop)
			stepAll.Acp.Configuration.ConfigureCarrierAndOffsets("", integrationBandwidth, numberOfOffsetChannels, channelSpacing)
			offsetString = RFmxSpecAnMX.BuildOffsetString2("", -1)
			stepAll.Acp.Configuration.ConfigureOffsetRrcFilter(offsetString, RFmxSpecAnMXAcpOffsetRrcFilterEnabled.[True], 0.22)
			carrierString = RFmxSpecAnMX.BuildCarrierString2("", -1)
			stepAll.Acp.Configuration.ConfigureCarrierRrcFilter(carrierString, RFmxSpecAnMXAcpCarrierRrcFilterEnabled.[True], 0.22)
			stepAll.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Acp, False)
			SpecAnList.Initiate("", "")
			rfsgSession.Initiate()
			instrSession.WaitForAcquisitionComplete(timeout)
		End Sub

		Private Sub RetrieveResults()
			Dim totalRelativePower As Double() = New Double(numberOfSteps - 1) {}
			Dim carrierFrequency As Double() = New Double(numberOfSteps - 1) {}
			Dim integrationBandwidth As Double() = New Double(numberOfSteps - 1) {}

			offsetMeasurementObject = New OffsetMeasurement(numberOfSteps - 1) {}
			For i As Integer = 0 To numberOfSteps - 1
				[step](i).Acp.Results.FetchOffsetMeasurementArray("", timeout, offsetMeasurementObject(i).lowerRelativePower, offsetMeasurementObject(i).upperRelativePower, offsetMeasurementObject(i).lowerAbsolutePower, offsetMeasurementObject(i).upperAbsolutePower)

				[step](i).Acp.Results.FetchCarrierMeasurement("", timeout, absolutePower(i), totalRelativePower(i), carrierFrequency(i), integrationBandwidth(i))
			Next
		End Sub


		Private Sub PrintResults()
			Console.WriteLine(vbLf & "-----------Measurements----------- " & vbLf)
			For i As Integer = 0 To numberOfSteps - 1
				Console.WriteLine("Step {0}: ", i)
				Console.WriteLine(vbLf & "-----------Carrier Measurements----------- " & vbLf)
				Console.WriteLine(vbLf & "Absolute Power (dBm or dBm/Hz) : {0}", absolutePower(i))
				Console.WriteLine(vbLf & "-----------Offset Channel Measurements----------- " & vbLf)
				For j As Integer = 0 To offsetMeasurementObject(i).lowerRelativePower.Length - 1
					Console.WriteLine("Offset {0}: ", j)
					Console.WriteLine("Lower Relative Power (dB)              : {0}", offsetMeasurementObject(i).lowerRelativePower(j))
					Console.WriteLine("Upper Relative Power (dB)              : {0}", offsetMeasurementObject(i).upperRelativePower(j))
					Console.WriteLine("Lower Absolute Power (dBm or dBm/Hz)   : {0}", offsetMeasurementObject(i).lowerAbsolutePower(j))
					Console.WriteLine("Upper Absolute Power (dBm or dBm/Hz)   : {0}", offsetMeasurementObject(i).upperAbsolutePower(j))
					Console.WriteLine("-------------------------------------------------" & vbLf)
				Next
				Console.WriteLine("-------------------------------------------------" & vbLf)
			Next
		End Sub

		Private Sub CloseSession()
			If SpecAnList IsNot Nothing Then
				SpecAnList.Dispose()
				SpecAnList = Nothing
			End If
			If instrSession IsNot Nothing Then
				instrSession.Close()
				instrSession = Nothing
			End If
			If rfsgSession IsNot Nothing Then
				rfsgSession.Abort()
				rfsgSession.BasicConfigurationList.DeleteConfigurationList("PowerLevelList")
				rfsgSession.Arb.ClearWaveform(waveformName)
				rfsgSession.Close()
				rfsgSession = Nothing
			End If
		End Sub

		Private Shared Sub DisplayError(ex As Exception)
			Console.WriteLine("ERROR:" & vbLf & Convert.ToString(ex.[GetType]()) & ": " & ex.Message)
		End Sub

		Private Sub LinearRampPattern(start As Double, [end] As Double, samples As Integer, includeEnd As Boolean, ByRef rampPattern As Double())
			Dim m As Integer = If(includeEnd, samples, (samples - 1))
			Dim delta As Double = ([end] - start) / m
			For i As Integer = 0 To samples - 1
				rampPattern(i) = start + (i * delta)
			Next
		End Sub
	End Class
End Namespace