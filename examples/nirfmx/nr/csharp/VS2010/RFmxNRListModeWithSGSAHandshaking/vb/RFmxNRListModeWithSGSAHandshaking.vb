'Steps:
'1. Open NI-RFSG session.
'2. Configure RFSG Selected Ports and GenerationMode to Script.
'3. Configure RFSG frequency reference.
'4. Configure RFSG configuration settled event to the device scriptTrigger0, to make sure generation starts only after
'   RFSG configuration settled.
'5. Configure RFSG to advance upon receipt of RFSA Ready for Advance event through PXI trigger line.
'6. Configure frequency, external gain, Power Level Type and Pre-filter Gain of RF output signal.
'7. Get terminal name for marker0 and assign to the RFSA Reference Trigger Digital Edge source.
'8. Create a Configuration List. Pass Power Level in the Configuration List Properties parameter to be able to configure
'   Power Level in each step that we create. The Set As Active List parameter in this VI defaults to true, this will set the
'   Active Configuration List property to the name of the created configuration list.Once the Active Configuration List
'   is set, using a property node to access Power Level will modify the property for this configuration list.
'9. Create a Configuration List Step.The Set As Active Step parameter in this VI defaults to true, this will set the Active
'   Configuration List Step property to the created configuration list step index.Once the Active Configuration List
'   Step is set, using a property node to access Power Level will modify the property for this configuration list step in
'   the configuration list indicated by the Active Configuration List property.
'10. Configure the Power Level for the Active Configuration List Step in the Active Configuration List.
'11. Configure RFSG LO Source to Automatic SG SA Shared.
'12. Read waveform from file and download Waveform from file to RFSG.
'13. Retrieve the waveform PAPR, Signal Bandwidth and IQ rate. Add the waveform PAPR to the RFSA reference level while configuring to
'    every list step.
'14. Configure RFSG Signal Bandwidth, IQ rate, and PAPR. With the signal bandwidth configured and the Upconverter Frequency Offset
'       Mode set to Automatic by default, the RFSG LO is placed outside the signal if the signal bandwidth is less than half of the device
'       instantaneous bandwidth; otherwise, the LO is placed at the center of the signal.
'15. Write script to generate the waveform specified in the script. This script is programmed to generate waveform
'    continuously and generate a marker at the start of the waveform (sample 0).
'16. Open a new RFmx Session.
'17. Configure the Frequency Reference properties (Clock Source and Clock Frequency).
'18. Export RFSA Ready for Advance event to PXI trigger line.
'19. Create RFmx NR List.
'20. Create RFmx NR List Step.
'21. Configure the Reference Level for the Specified List Step in the NR List.
'22. Configure personality and measurement parameters for a List Step.
'23. Configure Center Frequency, Selected Ports, External Attenuation, Trigger Type and Trigger Parameters for all List Steps.
'24. Configure Link Direction, Frequency Range, CC bandwidth, Cell ID, Band, and BWP Subcarrier Spacing for all List Steps.
'25. Set LO Leakage Avoidance Enabled to True and Automatic SG SA Shared LO to Enabled. Enabling LO Leakage Avoidance causes RFmx
'    to place the SA LO outside the measurement bandwidth, if the measurement bandwidth is less than half of the device instantaneous
'    bandwidth; otherwise, the LO is placed at the center of the signal.
'26. Select ModAcc measurement and disable traces for all List Steps.
'27. Initiate ModAcc measurement for List.
'28. Initiate signal generation.
'29. Wait for Acquisition to complete.
'30. Stop signal generation.
'31. Delete the Configuration List to avoid committing the Configuration List to the hardware with subsequent
'    calls to Commit. Deleting the list will reset the Active Configuration List.
'32. Fetch ModAcc measurement Results for all Configuration List Steps one by one.
'33. Delete RFmx NR List.
'34. Close the RFmx Session.
'35. Close the RFSG session. 
'    It is recommended to clear the waveform before closing RFSG session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.NRMX
Imports NationalInstruments.ModularInstruments.NIRfsg

Namespace NationalInstruments.Examples.RFmxNRListModeWithSGSAHandshaking
	Public Class RFmxNRListModeWithSGSAHandshaking
		Private instrSession As RFmxInstrMX
		Private NRList As RFmxNRMXList
		Private rfsgSession As NIRfsg

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

		Private digitalEdge As RFmxNRMXDigitalEdgeTriggerEdge
		Private triggerDelay As Double

		Private numberOfSteps As Integer

		Private startReferenceLevel As Double
		Private stopReferenceLevel As Double

		Private frequencyRange As RFmxNRMXFrequencyRange
		Private carrierBandwidth As Double
		Private subcarrierSpacing As Double
		Private band As Integer
		Private cellID As Integer

		Private rampPattern As Double()
		Private timeout As Double
		Private script As String
		Private papr As Double
		Private [step] As RFmxNRMX()
		Private configurationSettledEvenTerminalName As String
		Private markerEventTerminalName As String

		Private compositeRmsEvmMean As Double()
		Private compositePeakEvmMaximum As Double()


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
			centerFrequency = 3500000000.0
			' (Hz) 

			rfsgResourceName = "RFSA"
			rfsgSelectedPorts = ""
			waveformFilePath = "NR_FR2_UL_SISO_CC-1_BW-50MHz_SCS-120kHz.tdms"
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

			digitalEdge = RFmxNRMXDigitalEdgeTriggerEdge.Rising
			triggerDelay = 0.0
			' (s) 

			numberOfSteps = 10

			startReferenceLevel = -20.0
			' (dBm) 
			stopReferenceLevel = 0.0
			' (dBm) 

			frequencyRange = RFmxNRMXFrequencyRange.Range2
			carrierBandwidth = 50000000.0
			' (Hz) 
			subcarrierSpacing = 120000.0
			' (Hz) 
			band = 257
			cellID = 0

			rampPattern = New Double(numberOfSteps - 1) {}
			LinearRampPattern(startReferenceLevel, stopReferenceLevel, numberOfSteps, False, rampPattern)

			timeout = 10.0
			' (s) 

			script = String.Format("script GenerateWaveform" & vbLf & "  repeat forever" & vbLf & "    generate {0} marker0(0)" & vbLf & "  wait until scripttrigger0" & vbLf & "   end repeat" & vbLf & "  end script", waveformName)

			[step] = New RFmxNRMX(numberOfSteps - 1) {}
			compositeRmsEvmMean = New Double(numberOfSteps - 1) {}
			compositePeakEvmMaximum = New Double(numberOfSteps - 1) {}
		End Sub

		Private Sub ConfigureRfsg()
			rfsgSession = New NIRfsg(rfsgResourceName, True, False)
			rfsgSession.SignalPath.SelectedPorts = rfsgSelectedPorts
			rfsgSession.FrequencyReference.Configure(rfsgFrequencyReferenceSource, rfsgFrequency)
			configurationSettledEvenTerminalName = rfsgSession.DeviceEvents.ConfigurationSettledEvent.TerminalName
			rfsgSession.Triggers.ScriptTriggers(0).DigitalEdge.Configure(configurationSettledEvenTerminalName, RfsgTriggerEdge.RisingEdge)
			rfsgSession.Triggers.ConfigurationListStepTrigger.DigitalEdge.Configure(RfsgDigitalEdgeConfigurationListStepTriggerSource.PxiTriggerLine0, RfsgTriggerEdge.RisingEdge)
			rfsgSession.RF.ExternalGain = -1 * rfsgExternalAttenuation
			rfsgSession.RF.PowerLevelType = RfsgRFPowerLevelType.PeakPower
			rfsgSession.Arb.PreFilterGain = -1.5
			rfsgSession.RF.Frequency = centerFrequency
			markerEventTerminalName = rfsgSession.DeviceEvents.MarkerEvents(0).TerminalName
			Dim properties As RfsgConfigurationListProperties() = New RfsgConfigurationListProperties(0) {RfsgConfigurationListProperties.PowerLevel}
			rfsgSession.BasicConfigurationList.CreateConfigurationList("PowerLevelList", properties, True)
			For i As Integer = 0 To numberOfSteps - 1
				rfsgSession.BasicConfigurationList.CreateStep(True)
				rfsgSession.RF.PowerLevel = rampPattern(i)
			Next
			rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.Script
			rfsgSession.Arb.ReadAndDownloadWaveformFromFileTdms(waveformName, waveformFilePath, 0)
			papr = rfsgSession.Arb.Waveforms(waveformName).Papr
			Dim waveformIqRate As Double = rfsgSession.Arb.Waveforms(waveformName).IQRate
			Dim waveformSignalBandwidth As Double = rfsgSession.Arb.Waveforms(waveformName).SignalBandwidth
			rfsgSession.Arb.IQRate = waveformIqRate
			rfsgSession.Arb.SignalBandwidth = waveformSignalBandwidth
			rfsgSession.RF.PeakPowerAdjustment = papr
			rfsgSession.RF.LocalOscillator.Source = RfsgLocalOscillatorSource.AutomaticSGSAShared
			rfsgSession.Arb.Scripting.WriteScript(script)
		End Sub

		Private Sub ConfigureRFmx()
			instrSession = New RFmxInstrMX(rfsaResourceName, "")
			instrSession.ConfigureFrequencyReference("", rfsaFrequencyReferenceSource, rfsaFrequency)
			instrSession.ExportSignal(RFmxInstrMXExportSignalSource.ReadyForAdvanceEvent, RFmxInstrMXConstants.PxiTriggerLine0)
			NRList = instrSession.GetNRList("ModAcc_List")
			For i As Integer = 0 To numberOfSteps - 1
				[step](i) = NRList.CreateListStep()
				[step](i).SetReferenceLevel("", papr + rampPattern(i))
			Next
			Dim stepAll As RFmxNRMX = NRList.GetListStepAll()
			stepAll.ConfigureFrequency("", centerFrequency)
			stepAll.SetSelectedPorts("", rfsaSelectedPorts)
			stepAll.ConfigureDigitalEdgeTrigger("", markerEventTerminalName, digitalEdge, triggerDelay, True)
			stepAll.ConfigureExternalAttenuation("", rfsaExternalAttenuation)
			stepAll.SetLinkDirection("", RFmxNRMXLinkDirection.Uplink)
			stepAll.SetFrequencyRange("", frequencyRange)
			stepAll.ComponentCarrier.SetBandwidth("", carrierBandwidth)
			stepAll.ComponentCarrier.SetCellID("", cellID)
			stepAll.SetBand("", band)
			stepAll.ComponentCarrier.SetBandwidthPartSubcarrierSpacing("", subcarrierSpacing)

			instrSession.SetLOSource("", RFmxInstrMXConstants.LOSourceAutomaticSGSAShared)
			instrSession.SetLOLeakageAvoidanceEnabled("", RFmxInstrMXLOLeakageAvoidanceEnabled.[True])

			stepAll.SelectMeasurements("", RFmxNRMXMeasurementTypes.ModAcc, False)
			NRList.Initiate("", "")
			rfsgSession.Initiate()
			instrSession.WaitForAcquisitionComplete(timeout)
		End Sub

		Private Sub RetrieveResults()
			For i As Integer = 0 To numberOfSteps - 1
				[step](i).ModAcc.Results.GetCompositeRmsEvmMean("", compositeRmsEvmMean(i))
				[step](i).ModAcc.Results.GetCompositePeakEvmMaximum("", compositePeakEvmMaximum(i))
			Next
		End Sub


		Private Sub PrintResults()
			Console.WriteLine("------------------Measurements------------------" & vbLf)
			Console.WriteLine("Composite RMS EVM Mean (%)")
			For i As Integer = 0 To numberOfSteps - 1
				Console.WriteLine("Step{0}    : {1}", i, compositeRmsEvmMean(i))
			Next
			Console.WriteLine(vbLf & "Composite Peak EVM Maximum (%)")
			For i As Integer = 0 To numberOfSteps - 1
				Console.WriteLine("Step{0}    : {1}", i, compositePeakEvmMaximum(i))
			Next
		End Sub

		Private Sub CloseSession()
			If NRList IsNot Nothing Then
				NRList.Dispose()
				NRList = Nothing
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
