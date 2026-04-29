'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
'4. Configure Trigger Type and Trigger Parameters.
'5. Configure Uplink Spreading Parameters.
'6. Select SlotPower measurement.
'7. Configure Synchronization Mode and Interval
'8. Initiate the Measurement.
'9. Fetch SlotPower Measurement.
'10 Close the RFmx Seesion



Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.EvdoMX

Public Class RFmxEvdoSlotPower
	Private instrSession As RFmxInstrMX
	Private evdo As RFmxEvdoMX

	Private resourceName As String
	Private measurement As RFmxEvdoMXMeasurementTypes
	Private frequencyReferenceSource As String
	Private frequencyReferenceFrequency As Double
	Private centerFrequency As Double
	Private externalAttenuation As Double

	Private digitalEdgeSource As String
	Private digitalEdge As RFmxEvdoMXDigitalEdgeTriggerEdge
	Private triggerDelay As Double
	Private referenceLevel As Double

	Private synchronizationMode As RFmxEvdoMXSlotPowerSynchronizationMode
	Private measurementOffset As Integer
	Private measurementLength As Integer
	Private uplinkSpreadingIMask As Long
	Private uplinkSpreadingQMask As Long


	Private enableAllTraces As Boolean
	Private enableTrigger As Boolean
    Private timeout As Double

    Private halfSlotPower As Double() = Nothing
	Private halfSlotPowerDelta As Double() = Nothing

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
		measurement = RFmxEvdoMXMeasurementTypes.SlotPower
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

		synchronizationMode = RFmxEvdoMXSlotPowerSynchronizationMode.Slot
		measurementOffset = 0
		'slots
		measurementLength = 16
		'slots
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
		evdo.ConfigureUplinkSpreading("", uplinkSpreadingIMask, uplinkSpreadingQMask)
		evdo.SelectMeasurements("", measurement, enableAllTraces)
		evdo.SlotPower.Configuration.ConfigureSynchronizationModeAndInterval("", synchronizationMode, measurementOffset, measurementLength)
		evdo.Initiate("", "")
	End Sub

	Private Sub RetrieveResults()
		evdo.SlotPower.Results.FetchPowers("", timeout, halfSlotPower, halfSlotPowerDelta)
	End Sub

	Private Sub PrintResults()
		Console.WriteLine("------------Half Slot Powers------------" & vbLf)
		For i As Integer = 0 To halfSlotPower.Length - 1
			Console.WriteLine(vbLf & "Half Slot Number {0}", i)
			Console.WriteLine("Half Slot Power (dBm)                    : {0}", halfSlotPower(i))
			Console.WriteLine("Half Slot Power Delta (dB)               : {0}", halfSlotPowerDelta(i))
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
