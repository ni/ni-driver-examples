'Steps:
'1.  Open NI - RFSG session.
'2. Configure RFSG Selected Ports.
'3. Configure RFSG generation mode to Script. This allows for writing a script that determines the behavior of the generation.
'4.  Configure RFSG frequency reference.
'5.  Configure RFSG configuration settled event to the device scriptTrigger0, to make sure generation starts only after
'    RFSG configuration settled.
'6.  Configure RFSG to advance upon receipt of RFSA Ready for Advance event through PXI trigger line
'7.  Configure frequency and external gain of RF output signal.
'8.  Get terminal name for marker0and assign to the RFSA Reference Trigger Digital Edge source
'9.  Create a Configuration List. Pass Power Level in the Configuration List Properties parameter to be able to configure
'    Power Level in each step that we create. The Set As Active List parameter in this VI defaults to true, this will set the
'    Active Configuration List property to the name of the created configuration list.Once the Active Configuration List
'    is set, using a property node to access Power Level will modify the property for this configuration list.
'10.  Create a Configuration List Step.The Set As Active Step parameter in this VI defaults to true, this will set the Active
'    Configuration List Step property to the created configuration list step index.Once the Active Configuration List
'    Step is set, using a property node to access Power Level will modify the property for this configuration list step in
'    the configuration list indicated by the Active Configuration List property.
'11. Configure the Power Level for the Active Configuration List Step in the Active Configuration List.
'12. Read waveform from file and download Waveform from file to RFSG.
'13. Write script to generate the waveform specified in the script.This script is programmed to generate waveform
'    continuously and generate a marker at the start of the waveform(sample 0).
'14. Open a new RFmx Session.
'15. Configure the Frequency Reference properties(Clock Source and Clock Frequency).
'16. Export RFSA Ready for Advance event to PXI trigger line
'17. Create RFmx SpecAn List.
'18. Create RFmx SpecAn List Step.
'19. Configure the Reference Level for the Specified List Step in the SpecAn List.
'20. Configure Center Frequency, Selected Ports, External Attenuation, Trigger Type and Trigger Parameters for all Configuration List Step.
'21. Configure Sweep Time, RBW Filter, FFT parameters for all List Steps.
'22. Configure Integration BW of the Carrier channel, Number of Offset Channelsand Channel Spacing for all List Steps.
'23. Configure Carrier and Offset RRC Filter for all List Steps.
'24. Select ACP measurement and disable traces for all List Steps.
'25. Initiate ACP measurement for List.
'26. Initiate signal generation.
'27. Wait for Acquisition to complete
'28. Stop signal generation.
'29. Delete the Configuration List to avoid committing the Configuration List to the hardware with subsequent
'    calls to Commit. Deleting the list will reset the Active Configuration List.
'30. Fetch ACP measurement Results for all Configuration List Steps one by one.
'31. Delete RFmx SpecAn List.
'32. Close the RFmx Session.
'33. Close the RFSG session. 
'    It is recommended to clear the waveform before closing RFSG session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.SpecAnMX
Imports NationalInstruments.ModularInstruments.NIRfsg
Imports NationalInstruments.ModularInstruments.NIRfsgPlayback

Namespace NationalInstruments.Examples.RFmxSpecAnListModeWithSGSAHandshaking
	Public Class RFmxSpecAnListModeWithSGSAHandshaking
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

		Private digitalEdge As RFmxSpecAnMXDigitalEdgeTriggerEdge
		Private triggerDelay As Double

		Private numberOfSteps As Integer

		Private startReferenceLevel As Double
		Private stopReferenceLevel As Double

		Private rampPattern As Double()
		Private timeout As Double
		Private script As String
		Private [step] As RFmxSpecAnMX()
		Private absolutePower As Double()

		Private configurationSettledEvenTerminalName As String
		Private markerEventTerminalName As String

		Private sweepTimeAuto As RFmxSpecAnMXAcpSweepTimeAuto
		Private sweepTimeInterval As Double
		Private rbwAutoBandwidth As RFmxSpecAnMXAcpRbwAutoBandwidth
		Private rbwFilterType As RFmxSpecAnMXAcpRbwFilterType
		Private rbwBandwidth As Double
		Private fftPading As Double
		Private fftWindow As RFmxSpecAnMXAcpFftWindow
		Private integrationBandwidth As Double

		Private offsetString As String
		Private carrierString As String

		Private numberOfOffsetChannels As Integer
		Private channelSpacing As Double
		' Hz 

		Private offsetRRCEnabled As RFmxSpecAnMXAcpOffsetRrcFilterEnabled
		Private carrierRRCEnabled As RFmxSpecAnMXAcpCarrierRrcFilterEnabled
		Private offsetRRCAlpha As Double
		Private carrierRRCAlpha As Double

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

			digitalEdge = RFmxSpecAnMXDigitalEdgeTriggerEdge.Rising
			triggerDelay = 0.0
			' (s) 

			numberOfSteps = 10

			startReferenceLevel = -20.0
			' (dBm) 
			stopReferenceLevel = 0.0
			' (dBm) 

			sweepTimeAuto = RFmxSpecAnMXAcpSweepTimeAuto.[False]
			sweepTimeInterval = 0.00066667
			' (us) 
			rbwAutoBandwidth = RFmxSpecAnMXAcpRbwAutoBandwidth.[True]
			rbwFilterType = RFmxSpecAnMXAcpRbwFilterType.FftBased
			rbwBandwidth = 38400.0
			fftPading = 1.0
			fftWindow = RFmxSpecAnMXAcpFftWindow.FlatTop
			integrationBandwidth = 3840000.0

			numberOfOffsetChannels = 2

			channelSpacing = 5000000.0
			offsetRRCEnabled = RFmxSpecAnMXAcpOffsetRrcFilterEnabled.[True]
			carrierRRCEnabled = RFmxSpecAnMXAcpCarrierRrcFilterEnabled.[True]
			offsetRRCAlpha = 0.22
			carrierRRCAlpha = 0.22

			rampPattern = New Double(numberOfSteps - 1) {}
			LinearRampPattern(startReferenceLevel, stopReferenceLevel, numberOfSteps, False, rampPattern)

			timeout = 10.0
			' (s) 

			script = String.Format("script GenerateWaveform" & vbLf & "  repeat forever" & vbLf & "    generate {0} marker0(0)" & vbLf & "  wait until scripttrigger0" & vbLf & "   end repeat" & vbLf & "  end script", waveformName)

			[step] = New RFmxSpecAnMX(numberOfSteps - 1) {}
			absolutePower = New Double(numberOfSteps - 1) {}
			' (dBm or dBm/Hz) 

		End Sub

		Private Sub ConfigureRfsg()
			rfsgSession = New NIRfsg(rfsgResourceName, True, False)
			rfsgSession.SignalPath.SelectedPorts = rfsgSelectedPorts
			rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.Script
			rfsgSession.FrequencyReference.Configure(rfsgFrequencyReferenceSource, rfsgFrequency)
			configurationSettledEvenTerminalName = rfsgSession.DeviceEvents.ConfigurationSettledEvent.TerminalName
			rfsgSession.Triggers.ScriptTriggers(0).DigitalEdge.Configure(configurationSettledEvenTerminalName, RfsgTriggerEdge.RisingEdge)
			rfsgSession.Triggers.ConfigurationListStepTrigger.DigitalEdge.Configure(RfsgDigitalEdgeConfigurationListStepTriggerSource.PxiTriggerLine0, RfsgTriggerEdge.RisingEdge)
			rfsgSession.RF.ExternalGain = -1 * rfsgExternalAttenuation
			rfsgSession.RF.Frequency = centerFrequency
			markerEventTerminalName = rfsgSession.DeviceEvents.MarkerEvents(0).TerminalName
			Dim properties As RfsgConfigurationListProperties() = New RfsgConfigurationListProperties(0) {RfsgConfigurationListProperties.PowerLevel}
			rfsgSession.BasicConfigurationList.CreateConfigurationList("PowerLevelList", properties, True)
			For i As Integer = 0 To numberOfSteps - 1
				rfsgSession.BasicConfigurationList.CreateStep(True)
				rfsgSession.RF.PowerLevel = rampPattern(i)
			Next
			instrumentHandle = rfsgSession.GetInstrumentHandle().DangerousGetHandle()
			rfsgSession.Arb.ReadAndDownloadWaveformFromFileTdms(waveformName, waveformFilePath, 0)
			rfsgSession.Arb.Scripting.WriteScript(script)
		End Sub

		Private Sub ConfigureRFmx()
			instrSession = New RFmxInstrMX(rfsaResourceName, "")
			instrSession.ConfigureFrequencyReference("", rfsaFrequencyReferenceSource, rfsaFrequency)
			instrSession.ExportSignal(RFmxInstrMXExportSignalSource.ReadyForAdvanceEvent, RFmxInstrMXConstants.PxiTriggerLine0)
			SpecAnList = instrSession.GetSpecAnList("ACP_List")
			For i As Integer = 0 To numberOfSteps - 1
				[step](i) = SpecAnList.CreateListStep()
				[step](i).SetReferenceLevel("", rampPattern(i))
			Next
			Dim stepAll As RFmxSpecAnMX = SpecAnList.GetListStepAll()
			stepAll.ConfigureFrequency("", centerFrequency)
			stepAll.SetSelectedPorts("", rfsaSelectedPorts)
			stepAll.ConfigureDigitalEdgeTrigger("", markerEventTerminalName, digitalEdge, triggerDelay, True)
			stepAll.ConfigureExternalAttenuation("", rfsaExternalAttenuation)
			stepAll.Acp.Configuration.SetSweepTimeAuto("", sweepTimeAuto)
			stepAll.Acp.Configuration.SetSweepTimeInterval("", sweepTimeInterval)
			stepAll.Acp.Configuration.SetRbwFilterAutoBandwidth("", rbwAutoBandwidth)
			stepAll.Acp.Configuration.SetRbwFilterType("", rbwFilterType)
			stepAll.Acp.Configuration.SetRbwFilterBandwidth("", rbwBandwidth)
			stepAll.Acp.Configuration.SetFftPadding("", fftPading)
			stepAll.Acp.Configuration.SetFftWindow("", fftWindow)
			stepAll.Acp.Configuration.ConfigureCarrierAndOffsets("", integrationBandwidth, numberOfOffsetChannels, channelSpacing)
			offsetString = RFmxSpecAnMX.BuildOffsetString2("", -1)
			stepAll.Acp.Configuration.ConfigureOffsetRrcFilter(offsetString, offsetRRCEnabled, offsetRRCAlpha)
			carrierString = RFmxSpecAnMX.BuildCarrierString2("", -1)
			stepAll.Acp.Configuration.ConfigureCarrierRrcFilter(carrierString, carrierRRCEnabled, carrierRRCAlpha)
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
