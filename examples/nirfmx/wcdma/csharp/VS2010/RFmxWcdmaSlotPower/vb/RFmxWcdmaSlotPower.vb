'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
'4. Configure Trigger Type and Trigger Parameters.
'5. Configure Uplink Scrambling.
'6. Select SlotPower measurement and enable Traces.
'7. Configure Synchronization Mode and Interval
'8. Initiate the Measurement.
'9. Fetch SlotPower Measurement.
'10. Close the RFmx session. 

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.WcdmaMX

Public Class RFmxWcdmaSlotPower
	Private instrSession As RFmxInstrMX
	Private wcdma As RFmxWcdmaMX

	Private resourceName As String
	Private measurement As RFmxWcdmaMXMeasurementTypes
	Private frequencyReferenceSource As String
	Private frequencyReferenceFrequency As Double
	' Hz 
	Private centerFrequency As Double
	' Hz 
	Private externalAttenuation As Double
	' dB 

	Private digitalEdgeSource As String
	Private digitalEdge As RFmxWcdmaMXDigitalEdgeTriggerEdge
	Private triggerDelay As Double
	' seconds 
	Private referenceLevel As Double
	' dBm 
	Private synchronizationMode As RFmxWcdmaMXSlotPowerSynchronizationMode
	Private measurementOffset As Integer
	'slots
	Private measurementLength As Integer
	'slots
	Private uplinkScramblingType As RFmxWcdmaMXUplinkScramblingType
	Private uplinkScramblingCode As Integer

	Private enableAllTraces As Boolean
	Private enableTrigger As Boolean

	Private timeout As Double

	Private slotPower As Double() = Nothing
	Private slotPowerDelta As Double() = Nothing

	Public Sub Run()
		Try
			InitializeVariables()
			InitializeInstr()
			ConfigureWcdma()
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
		measurement = RFmxWcdmaMXMeasurementTypes.SlotPower
		frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
		frequencyReferenceFrequency = 10000000.0
		' Hz 
		centerFrequency = 1950000000.0
		' Hz 
		externalAttenuation = 0.0
		' dB 
		digitalEdgeSource = RFmxWcdmaMXConstants.Pfi0
		digitalEdge = RFmxWcdmaMXDigitalEdgeTriggerEdge.Rising
		triggerDelay = 0.0
		' seconds 
		referenceLevel = 0.0
		' dBm 

		synchronizationMode = RFmxWcdmaMXSlotPowerSynchronizationMode.Slot
		measurementOffset = 0
		'slots
		measurementLength = 15
		'slots

		uplinkScramblingType = RFmxWcdmaMXUplinkScramblingType.[Long]
		uplinkScramblingCode = 0

		enableAllTraces = True
		enableTrigger = False
		timeout = 10.0
	End Sub

	Private Sub InitializeInstr()
		' Create a new RFmx Session 
		instrSession = New RFmxInstrMX(resourceName, "")
	End Sub

	Private Sub ConfigureWcdma()
		wcdma = instrSession.GetWcdmaSignalConfiguration()
		instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
		wcdma.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
		wcdma.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger)
		wcdma.ConfigureUplinkScrambling("", uplinkScramblingCode, uplinkScramblingType)
		wcdma.SelectMeasurements("", measurement, enableAllTraces)
		wcdma.SlotPower.Configuration.ConfigureSynchronizationModeAndInterval("", synchronizationMode, measurementOffset, measurementLength)
		wcdma.Initiate("", "")
	End Sub

	Private Sub RetrieveResults()
		wcdma.SlotPower.Results.FetchPowers("", timeout, slotPower, slotPowerDelta)
	End Sub

	Private Sub PrintResults()
		Console.WriteLine("------------Slot Powers------------" & vbLf)
		For i As Integer = 0 To measurementLength - 1
			Console.WriteLine(vbLf & "Slot Number {0}", i)
			Console.WriteLine("Slot Power (dBm)                  : {0}", slotPower(i))
			Console.WriteLine("Slot Power Delta (dB)             : {0}", slotPowerDelta(i))
		Next

	End Sub

	Private Sub CloseSession()
		If wcdma IsNot Nothing Then
			wcdma.Dispose()
			wcdma = Nothing
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
