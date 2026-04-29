'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
'4. Configure Trigger Type and Trigger Parameters.
'5. Configure Channel Configuration Mode.
'6. Configure Physical Layer Subtype.
'7. Configure Uplink Data Modulation Type. 
'8. Configure Uplink Spreading Parameters.
'9. Select SlotPhase measurement and enable Traces. 
'10. Configure Synchronization Mode and Interval
'11. Initiate the Measurement.
'12. Fetch SlotPhase Measurements and Traces.
'13. Close RFmx Session.  

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.EvdoMX

Public Class RFmxEvdoSlotPhase
	Private instrSession As RFmxInstrMX
	Private evdo As RFmxEvdoMX

	Private resourceName As String
	Private measurement As RFmxEvdoMXMeasurementTypes
	Private frequencyReferenceSource As String
	Private frequencyReferenceFrequency As Double
	' Hz 
	Private centerFrequency As Double
	' Hz 
	Private externalAttenuation As Double
	' dB 

	Private digitalEdgeSource As String
	Private digitalEdge As RFmxEvdoMXDigitalEdgeTriggerEdge
	Private triggerDelay As Double
	' seconds 
	Private referenceLevel As Double
	' dBm 

	Private channelConfigurationMode As RFmxEvdoMXChannelConfigurationMode
	Private synchronizationMode As RFmxEvdoMXSlotPhaseSynchronizationMode
	Private measurementOffset As Integer
	Private measurementLength As Integer
	Private physicalLayerSubtype As RFmxEvdoMXPhysicalLayerSubtype
	Private uplinkDataModulationType As RFmxEvdoMXUplinkDataModulationType
	Private uplinkSpreadingIMask As Long
	Private uplinkSpreadingQMask As Long

	Private enableAllTraces As Boolean
	Private enableTrigger As Boolean
	Private timeout As Double

	Private maximumHalfSlotPhaseDiscontinuity As Double
	Private halfSlotPhaseDiscontinuity As Double() = Nothing
	Private chipPhaseError As AnalogWaveform(Of Single) = Nothing
	Private chipPhaseErrorLinearFit As AnalogWaveform(Of Single) = Nothing

	Public Sub Run()
		Try
			InitializeVariables()
			InitializeInstr()
			ConfigureEvdo()
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
		' Initialize input variables 

		resourceName = "RFSA"
		measurement = RFmxEvdoMXMeasurementTypes.SlotPhase
		frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
		frequencyReferenceFrequency = 10000000.0
		' Hz 
		centerFrequency = 833490000.0
		' Hz 
		externalAttenuation = 0.0
		' dB 

		digitalEdgeSource = RFmxEvdoMXConstants.Pfi0
		digitalEdge = RFmxEvdoMXDigitalEdgeTriggerEdge.Rising
		triggerDelay = 0.0
		' seconds 
		referenceLevel = 0.0
		' dBm 

		channelConfigurationMode = RFmxEvdoMXChannelConfigurationMode.AutoDetect
		synchronizationMode = RFmxEvdoMXSlotPhaseSynchronizationMode.Slot
		measurementOffset = 0
		' slots 
		measurementLength = 16
		' slots 
		physicalLayerSubtype = RFmxEvdoMXPhysicalLayerSubtype.Subtype0_1
		uplinkDataModulationType = RFmxEvdoMXUplinkDataModulationType.Auto
		uplinkSpreadingIMask = 0
		uplinkSpreadingQMask = 0

		enableAllTraces = True
		enableTrigger = False
		timeout = 10.0
	End Sub

	Private Sub InitializeInstr()
		' Create a new RFmx Session 

		instrSession = New RFmxInstrMX(resourceName, "")
	End Sub

	Private Sub ConfigureEvdo()
		evdo = instrSession.GetEvdoSignalConfiguration()
		instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
		evdo.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
		evdo.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger)
		evdo.ConfigureChannelConfigurationMode("", channelConfigurationMode)
		evdo.ConfigurePhysicalLayerSubtype("", physicalLayerSubtype)
		evdo.ConfigureUplinkDataModulationType("", uplinkDataModulationType)
		evdo.SelectMeasurements("", measurement, enableAllTraces)
		evdo.ConfigureUplinkSpreading("", uplinkSpreadingIMask, uplinkSpreadingQMask)
		evdo.SlotPhase.Configuration.ConfigureSynchronizationModeAndInterval("", synchronizationMode, measurementOffset, measurementLength)
		evdo.Initiate("", "")
	End Sub

	Private Sub RetrieveResults()
		evdo.SlotPhase.Results.FetchMaximumHalfSlotPhaseDiscontinuity("", timeout, maximumHalfSlotPhaseDiscontinuity)
		evdo.SlotPhase.Results.FetchPhaseDiscontinuities("", timeout, halfSlotPhaseDiscontinuity)
		evdo.SlotPhase.Results.FetchChipPhaseErrorTrace("", timeout, chipPhaseError)
		evdo.SlotPhase.Results.FetchChipPhaseErrorLinearFitTrace("", timeout, chipPhaseErrorLinearFit)
	End Sub

	Private Sub PrintResults()
		Console.WriteLine(vbLf & "-------------- Slot Phase Results --------------" & vbLf)
		Console.WriteLine("Maximum Half Slot Phase Discontinuity (deg)         : {0}" & vbLf, maximumHalfSlotPhaseDiscontinuity)

		For i As Integer = 0 To 2 * measurementLength - 1
			Console.WriteLine(vbLf & "Slot Number {0}", i)
			Console.WriteLine("Half Slot Phase Discontinuity (deg)                 : {0}", halfSlotPhaseDiscontinuity(i))
		Next

	End Sub

	Private Sub CloseSession()
		If evdo IsNot Nothing Then
			evdo.Dispose()
			evdo = Nothing
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
