'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
'4. Configure Trigger Type and Trigger Parameters.
'5. Select SlotPower measurement and enable Traces.
'6. Configure Measurement Length.
'7. Initiate the Measurement.
'8. Fetch SlotPower Measurement.
'9. Close the RFmx session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.TdscdmaMX

Public Class RFmxTdscdmaSlotPower
	Private instrSession As RFmxInstrMX
	Private tdscdma As RFmxTdscdmaMX

	Private resourceName As String
	Private measurement As RFmxTdscdmaMXMeasurementTypes
    Private frequencyReferenceSource As String, iqPowerEdgeTriggerSource As String
	Private frequencyReferenceFrequency As Double
	' Hz 
	Private centerFrequency As Double
	' Hz 
	Private externalAttenuation As Double
	' dB 
    Private triggerDelay As Double, minimumQuietTimeDuration As Double, _
     iqPowerEdgeTriggerLevel As Double

    Private referenceLevel As Double
    ' dBm 

    Private measurementLength As Integer
    'slots

    Private enableAllTraces As Boolean
    Private iqPowerEdgeTriggerSlope As RFmxTdscdmaMXIQPowerEdgeTriggerSlope
    Private minimumQuietTimeMode As RFmxTdscdmaMXTriggerMinimumQuietTimeMode
    Private enableTrigger As Boolean
    Private iqPowerEdgeTriggerLevelType As RFmxTdscdmaMXIQPowerEdgeTriggerLevelType

    Private timeout As Double

    Private slotPower As Double() = Nothing
    ' dBm
    Private slotPowerDelta As Double() = Nothing
    ' dB
    
    Public Sub Run()
        Try
            InitializeVariables()
            InitializeInstr()
            ConfigureTdscdma()
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
        measurement = RFmxTdscdmaMXMeasurementTypes.SlotPower
        frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
        frequencyReferenceFrequency = 10000000.0
        ' Hz 
        centerFrequency = 1910000000.0
        ' Hz 
        externalAttenuation = 0.0
        ' dB 

        triggerDelay = 0.0
        ' seconds 
        minimumQuietTimeDuration = 0.000016
        ' seconds 
        iqPowerEdgeTriggerLevel = -20.0
        'dB
        iqPowerEdgeTriggerSource = "0"
        iqPowerEdgeTriggerSlope = RFmxTdscdmaMXIQPowerEdgeTriggerSlope.Rising
        enableTrigger = True
        iqPowerEdgeTriggerLevelType = RFmxTdscdmaMXIQPowerEdgeTriggerLevelType.Relative
        minimumQuietTimeMode = RFmxTdscdmaMXTriggerMinimumQuietTimeMode.Auto
        
        referenceLevel = 0.0
        ' dBm 
        measurementLength = 28
        'slots
        
        enableAllTraces = True        

        timeout = 10.0

    End Sub

    Private Sub InitializeInstr()
        ' Create a new RFmx Session 

        instrSession = New RFmxInstrMX(resourceName, "")
    End Sub

    Private Sub ConfigureTdscdma()
        tdscdma = instrSession.GetTdscdmaSignalConfiguration()
        instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
        tdscdma.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)

        tdscdma.ConfigureIQPowerEdgeTrigger("", iqPowerEdgeTriggerSource, iqPowerEdgeTriggerSlope, iqPowerEdgeTriggerLevel, triggerDelay, minimumQuietTimeMode, _
         minimumQuietTimeDuration, iqPowerEdgeTriggerLevelType, enableTrigger)

        tdscdma.SelectMeasurements("", measurement, enableAllTraces)

        tdscdma.SlotPower.Configuration.ConfigureMeasurementLength("", measurementLength)
        tdscdma.Initiate("", "")
    End Sub

	Private Sub RetrieveResults()
		tdscdma.SlotPower.Results.FetchPowers("", timeout, slotPower, slotPowerDelta)
	End Sub

	Private Sub PrintResults()
        Console.WriteLine("------------Slot Powers ------------" & vbLf)
        For i As Integer = 0 To slotPower.Length - 1
            Console.WriteLine("Slot Number           : {0}", i)
            Console.WriteLine("Slot Power (dBm)      : {0}", slotPower(i))
            Console.WriteLine("Slot Power Delta (dB) : {0}", slotPowerDelta(i))
        Next
	End Sub

	Private Sub CloseSession()
		If tdscdma IsNot Nothing Then
			tdscdma.Dispose()
			tdscdma = Nothing
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
