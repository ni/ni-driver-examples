'[1] Steps:
'1. Open an NI-RFSG session.
'2. Configure RFSG frequency reference.
'3. Configure frequency And power level of RF output signal. 
'4. Set RFSG External Gain.
'5. Export the Marker Event marker0 to the to the terminal specified by the user, which Is also used as the source for the digital edge trigger on RFSA. 
'6. Read waveform from file And download it to RFSG.
'7. Set Automatic SG SA Shared LO to Enabled.

'[2] Steps to perform ModAcc measurement
'8.  Set LO Offset Mode to Auto while performing an in-band ModAcc measurement. This causes the RFSG LO to be placed outside the signal, if signal bandwidth Is less than half of the device instantaneous bandwidth; otherwise, the LO Is placed at the center of the signal.
'9.  Write script to generate the waveform specified in the script. This script Is programmed to generate waveform continuously. The marker0 Is configured at sample0.
'10. Initiate signal generation.
'---------------------------------------------------------------------------------------------------------------------------------------------------
'11. Open a New RFmx session.
'12. Configure the Frequency Reference properties (Clock Source And Clock Frequency).
'13. Configure the basic signal properties  (Center Frequency, Reference Level And External Attenuation).
'14. Configure Digital Edge Trigger properties (Digital Edge Source, Digital Edge, Trigger Delay).
'15. Configure Standard And Channel Bandwidth properties.
'16. Set LO Source to Automatic_SG_SA_Shared.
'17. Set LO Leakage Avoidance Enabled to True. This causes RFmx to place the SA LO outside the measurement bandwidth, if the measurement bandwidth Is less than half of the device instantaneous bandwidth; otherwise, the LO Is placed at the center of the signal.
'18. Select OFDMModAcc measurement And disable traces.
'19. Configure OFDMModAcc Averaging properties (Averaging Enabled, Averaging Count, Averaging Type, Vector Averaging Time Alignment Enabled, Vector Averaging Phase Alignment Enabled).
'20. Initiate OFDMModAcc measurement.
'21. Fetch OFDMModAcc measurements.

'[3] Steps to perform SEM measurement
'22. Stop signal generation.
'23. Configure LO Offset Mode for SEM measurement.
'    Set LO Offset Mode to Auto if the SEM measurement span does Not include the frequency of the RFSG LO.This causes the RFSG LO to be placed outside the signal, if signal bandwidth Is less than half of the device instantaneous bandwidth; otherwise, the LO Is placed at the center of the signal.
'    Set LO Offset Mode to No Offset if the SEM measurement span includes the frequency of the RFSG LO. This causes the RFSG LO to be placed at the center of the signal And avoids RFSG LO leakage impacting the SEM offset results.
'24. Initiate signal generation.
'---------------------------------------------------------------------------------------------------------------------------------------------------
'25. Select SEM measurement And disable traces. 
'26. Configure SEM Averaging properties (Averaging Enabled, Averaging Count, Averaging Type)
'27. Initiate SEM measurement.
'28. Fetch SEM measurements.

'[4] Steps
'29. Close the RFmx Session.
'30. Close the RFSG session. 
'It Is recommended to clear the waveform before closing RFSG session.

Imports NationalInstruments.ModularInstruments.NIRfsg
Imports NationalInstruments.ModularInstruments.NIRfsgPlayback
Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.WlanMX

Namespace NationalInstruments.Examples.RFmxWlanFemTestWithAutomaticSGSASharedLO

	Public Class RFmxWlanFemTestWithAutomaticSGSASharedLO
		Private instrSession As RFmxInstrMX
		Private wlan As RFmxWlanMX
		Private rfsgSession As NIRfsg
		Private instrumentHandle As IntPtr

		Private centerFrequency As Double

		Private rfsgResourceName As String
		Private waveformFilePath As String
		Private waveformName As String

		Private powerLevel As Double
		Private rfsgExternalAttenuation As Double

		Private rfsgFrequencyReferenceSource As RfsgFrequencyReferenceSource
		Private rfsgFrequency As Double

		Private rfsaResourceName As String
		Private referenceLevel As Double
		Private rfsaExternalAttenuation As Double

		Private rfsaFrequencyReferenceSource As String
		Private rfsaFrequency As Double

		Private digitalTriggerEnabled As Boolean
		Private digitalEdgeTriggerEdge As RFmxWlanMXDigitalEdgeTriggerEdge
		Private digitalEdgeSource As [String]
		Private triggerDelay As Double

		Private standard As RFmxWlanMXStandard
		Private channelBandwidth As Double

		Private ofdmModAccAveragingEnabled As RFmxWlanMXOfdmModAccAveragingEnabled
		Private ofdmModAccAveragingCount As Int32
		Private ofdmModAccAveragingType As RFmxWlanMXOfdmModAccAveragingType
		Private vectorAveragingTimeAlignmentEnabled As RFmxWlanMXOfdmModAccVectorAveragingTimeAlignmentEnabled
		Private vectorAveragingPhaseAlignmentEnabled As RFmxWlanMXOfdmModAccVectorAveragingPhaseAlignmentEnabled

		Private rfsgLOOffsetMode As NIRfsgPlaybackLOOffsetMode
		Private semAveragingEnabled As RFmxWlanMXSemAveragingEnabled
		Private semAveragingCount As Int32
		Private semAveragingType As RFmxWlanMXSemAveragingType

		Private timeout As Double

		Private script As String
		Private markerNumber As Integer

		Private compositeRmsEvmMean As Double
		' (dB) 
		Private compositeDataRmsEvmMean As Double
		' (dB) 
		Private compositePilotRmsEvmMean As Double
		' (dB) 

		Private measurementStatus As RFmxWlanMXSemMeasurementStatus

		Private absolutePower As Double
		' (dBm) 
		Private relativePower As Double

		Private lowerOffsetMeasurementStatus As RFmxWlanMXSemLowerOffsetMeasurementStatus()
		Private lowerOffsetMargin As Double()
		' (dB) 
		Private lowerOffsetMarginFrequency As Double()
		' (Hz) 
		Private lowerOffsetMarginAbsolutePower As Double()
		' (dBm) 
		Private lowerOffsetMarginRelativePower As Double()

		Private upperOffsetMeasurementStatus As RFmxWlanMXSemUpperOffsetMeasurementStatus()
		Private upperOffsetMargin As Double()
		' (dB) 
		Private upperOffsetMarginFrequency As Double()
		' (Hz) 
		Private upperOffsetMarginAbsolutePower As Double()
		' (dBm) 
		Private upperOffsetMarginRelativePower As Double()

		Public Sub Run()
			Try
				InitializeVariables()
				ConfigureRfsg()
				ConfigureRFmxAndRetrieveResults()
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
			centerFrequency = 2412000000.0
			' (Hz) 

			rfsgResourceName = "RFSG"
			waveformFilePath = "WLAN_80211ac_BW-80MHz_SISO.tdms"
			waveformName = "Wfm"

			powerLevel = -10.0
			' (dBm) 
			rfsgExternalAttenuation = 0.0
			' (dB) 

			rfsgFrequencyReferenceSource = RfsgFrequencyReferenceSource.OnboardClock
			rfsgFrequency = 10000000.0
			' (Hz) 

			rfsaResourceName = "RFSA"
			referenceLevel = 0.0
			' (dBm) 
			rfsaExternalAttenuation = 0.0
			' (dB) 

			rfsaFrequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
			rfsaFrequency = 10000000.0
			' (Hz) 

			digitalTriggerEnabled = True
			triggerDelay = 0.0
			' (s) 
			digitalEdgeTriggerEdge = RFmxWlanMXDigitalEdgeTriggerEdge.Rising
			digitalEdgeSource = "PFI0"

			standard = RFmxWlanMXStandard.Standard802_11ac
			channelBandwidth = 80000000.0
			' (Hz) 

			ofdmModAccAveragingEnabled = RFmxWlanMXOfdmModAccAveragingEnabled.[False]
			ofdmModAccAveragingCount = 10
			ofdmModAccAveragingType = RFmxWlanMXOfdmModAccAveragingType.Rms
			vectorAveragingTimeAlignmentEnabled = RFmxWlanMXOfdmModAccVectorAveragingTimeAlignmentEnabled.[True]
			vectorAveragingPhaseAlignmentEnabled = RFmxWlanMXOfdmModAccVectorAveragingPhaseAlignmentEnabled.[True]

			rfsgLOOffsetMode = NIRfsgPlaybackLOOffsetMode.Auto
			semAveragingEnabled = RFmxWlanMXSemAveragingEnabled.[False]
			semAveragingCount = 10
			semAveragingType = RFmxWlanMXSemAveragingType.Rms

			timeout = 10.0
			' (s) 

			script = [String].Format("script GenerateWaveform" & vbLf & "  repeat forever" & vbLf & "    generate {0} marker0(0)" & vbLf & "   end repeat" & vbLf & "  end script", waveformName)
			markerNumber = 0

		End Sub

		Private Sub ConfigureRfsg()
			rfsgSession = New NIRfsg(rfsgResourceName, True, False)
			rfsgSession.FrequencyReference.Configure(rfsgFrequencyReferenceSource, rfsgFrequency)
			rfsgSession.RF.Configure(centerFrequency, powerLevel)
			rfsgSession.DeviceEvents.MarkerEvents(markerNumber).ExportedOutputTerminal = RfsgMarkerEventExportedOutputTerminal.Pfi0
			rfsgSession.RF.ExternalGain = -1 * rfsgExternalAttenuation
			instrumentHandle = rfsgSession.GetInstrumentHandle().DangerousGetHandle()
			NIRfsgPlayback.ReadAndDownloadWaveformFromFile(instrumentHandle, waveformFilePath, waveformName)
			NIRfsgPlayback.StoreAutomaticSGSASharedLO(instrumentHandle, "", RfsgPlaybackAutomaticSGSASharedLO.Enabled)
			NIRfsgPlayback.StoreWaveformLOOffsetMode(instrumentHandle, waveformName, rfsgLOOffsetMode)
			NIRfsgPlayback.SetScriptToGenerateSingleRfsg(instrumentHandle, script)
			rfsgSession.Initiate()
		End Sub

		Private Sub ConfigureRFmxAndRetrieveResults()
			instrSession = New RFmxInstrMX(rfsaResourceName, "")
			wlan = instrSession.GetWlanSignalConfiguration()
			instrSession.ConfigureFrequencyReference("", rfsaFrequencyReferenceSource, rfsaFrequency)
			wlan.ConfigureFrequency("", centerFrequency)
			wlan.ConfigureReferenceLevel("", referenceLevel)
			wlan.ConfigureExternalAttenuation("", rfsaExternalAttenuation)
			wlan.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdgeTriggerEdge, triggerDelay, digitalTriggerEnabled)
			wlan.ConfigureStandard("", standard)
			wlan.ConfigureChannelBandwidth("", channelBandwidth)
			instrSession.SetLOSource("", RFmxInstrMXConstants.LOSourceAutomaticSGSAShared)
			instrSession.SetLOLeakageAvoidanceEnabled("", RFmxInstrMXLOLeakageAvoidanceEnabled.[True])
			wlan.SelectMeasurements("", RFmxWlanMXMeasurementTypes.OfdmModAcc, False)
			wlan.OfdmModAcc.Configuration.ConfigureAveraging("", ofdmModAccAveragingEnabled, ofdmModAccAveragingCount)
			wlan.OfdmModAcc.Configuration.SetAveragingType("", ofdmModAccAveragingType)
			wlan.OfdmModAcc.Configuration.SetVectorAveragingTimeAlignmentEnabled("", vectorAveragingTimeAlignmentEnabled)
			wlan.OfdmModAcc.Configuration.SetVectorAveragingPhaseAlignmentEnabled("", vectorAveragingPhaseAlignmentEnabled)
			wlan.Initiate("", "")

			RetrieveOfdmModAccResults()

			rfsgSession.Abort()
			NIRfsgPlayback.StoreWaveformLOOffsetMode(instrumentHandle, waveformName, rfsgLOOffsetMode)
			rfsgSession.Initiate()

			wlan.SelectMeasurements("", RFmxWlanMXMeasurementTypes.Sem, False)
			wlan.Sem.Configuration.ConfigureAveraging("", semAveragingEnabled, semAveragingCount, semAveragingType)
			wlan.Initiate("", "")

			RetrieveSemResults()
		End Sub

		Private Sub RetrieveOfdmModAccResults()
			wlan.OfdmModAcc.Results.FetchCompositeRmsEvm("", timeout, compositeRmsEvmMean, compositeDataRmsEvmMean, compositePilotRmsEvmMean)
		End Sub

		Private Sub RetrieveSemResults()
			wlan.Sem.Results.FetchMeasurementStatus("", timeout, measurementStatus)
			wlan.Sem.Results.FetchCarrierMeasurement("", timeout, absolutePower, relativePower)
			wlan.Sem.Results.FetchLowerOffsetMarginArray("", timeout, lowerOffsetMeasurementStatus, lowerOffsetMargin, lowerOffsetMarginFrequency, lowerOffsetMarginAbsolutePower,
				lowerOffsetMarginRelativePower)
			wlan.Sem.Results.FetchUpperOffsetMarginArray("", timeout, upperOffsetMeasurementStatus, upperOffsetMargin, upperOffsetMarginFrequency, upperOffsetMarginAbsolutePower,
				upperOffsetMarginRelativePower)
		End Sub

		Private Sub PrintResults()
			Console.WriteLine("------------------OFDMModAcc------------------" & vbLf)
			Console.WriteLine("------------------Composite EVM------------------")
			Console.WriteLine("RMS EVM Mean (dB)                  : {0}", compositeRmsEvmMean)
			Console.WriteLine("Data RMS EVM Mean (dB)             : {0}", compositeDataRmsEvmMean)
			Console.WriteLine("Pilot RMS EVM Mean (dB)            : {0}" & vbLf, compositePilotRmsEvmMean)

			Console.WriteLine(vbLf & "------------------SEM------------------" & vbLf)
			Console.WriteLine("Measurement Status                 :{0}", measurementStatus)
			Console.WriteLine("Carrier Absolute Power (dBm)       :{0}", absolutePower)
			Console.WriteLine(vbLf & "----------Lower Offset Measurements----------" & vbLf)
			For i As Integer = 0 To lowerOffsetMargin.Length - 1
				Console.WriteLine("Offset {0}", i)
				Console.WriteLine("Measurement Status                 :{0}", lowerOffsetMeasurementStatus(i))
				Console.WriteLine("Margin (dB)                        :{0}", lowerOffsetMargin(i))
				Console.WriteLine("Margin Frequency (Hz)              :{0}", lowerOffsetMarginFrequency(i))
				Console.WriteLine("Margin Absolute Power (dBm)        :{0}" & vbLf, lowerOffsetMarginAbsolutePower(i))
			Next
			Console.WriteLine(vbLf & "----------Upper Offset Measurements----------" & vbLf)
			For i As Integer = 0 To upperOffsetMargin.Length - 1
				Console.WriteLine("Offset {0}", i)
				Console.WriteLine("Measurement Status                 :{0}", upperOffsetMeasurementStatus(i))
				Console.WriteLine("Margin (dB)                        :{0}", upperOffsetMargin(i))
				Console.WriteLine("Margin Frequency (Hz)              :{0}", upperOffsetMarginFrequency(i))
				Console.WriteLine("Margin Absolute Power (dBm)        :{0}" & vbLf, upperOffsetMarginAbsolutePower(i))
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
			If rfsgSession IsNot Nothing Then
				rfsgSession.Abort()
				NIRfsgPlayback.ClearWaveform(instrumentHandle, waveformName)
				rfsgSession.Close()
				rfsgSession = Nothing
			End If
		End Sub

		Private Shared Sub DisplayError(ex As Exception)
			Console.WriteLine("ERROR:" & vbLf & Convert.ToString(ex.[GetType]()) & ": " & ex.Message)
		End Sub

	End Class
End Namespace