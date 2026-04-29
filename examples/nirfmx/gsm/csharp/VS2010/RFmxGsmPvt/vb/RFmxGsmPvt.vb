'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
'4. Configure operating Band.
'5. Configure Link Direction.
'6. Configure Trigger Parameters for IQ Power Edge Trigger.
'7. Configure Number of Timeslots.
'8. Configure Auto TSC Detection Enabled.
'9. Configure Signal Type.
'10. Configure TSC.
'11. Configure Power Control Level.
'12. Select PVT measurement and enable Traces.
'13. Configure RBW Filter Bandwidth (Hz) for PVT measurement.
'14. Configure Averaging Parameters for PVT measurement.
'15. Initiate the Measurement.
'16. Fetch PVT Measurements and Traces.
'17. Close RFmx Session. 

Imports NationalInstruments.RFmx.GsmMX
Imports NationalInstruments.RFmx.InstrMX

Public Class RFmxGsmPvtExample
	Private instrSession As RFmxInstrMX
	Private gsm As RFmxGsmMX
	Private resourceName As String

	Private frequencyReferenceFrequency As Double, centerFrequency As Double, referenceLevel As Double, externalAttenuation As Double, iqPowerEdgeLevel As Double, triggerDelay As Double,
		minimumQuietTime As Double, timeout As Double, rbwFilterBandwidth As Double

	Private slotAveragePower As Double(), slotBurstWidth As Double(), slotMaximumPower As Double(), slotMinimumPower As Double(), slotBurstThreshold As Double()

	Private enableTrigger As Boolean
	Private powerControlLevel As Integer, numberOfTimeslots As Integer, averagingCount As Integer
	Private frequencyReferenceSource As String
	Private band As RFmxGsmMXBand
	Private linkDirection As RFmxGsmMXLinkDirection
	Private averagingType As RFmxGsmMXPvtAveragingType
	Private burstType As RFmxGsmMXBurstType
	Private hbFilterWidth As RFmxGsmMXHBFilterWidth
	Private minimumQuietTimeMode As RFmxGsmMXTriggerMinimumQuietTimeMode
	Private autoTscDetectionEnabled As RFmxGsmMXAutoTscDetectionEnabled
	Private modulationType As RFmxGsmMXModulationType
	Private averagingEnabled As RFmxGsmMXPvtAveragingEnabled
	Private tsc As RFmxGsmMXTsc
	Private measurementStatus As RFmxGsmMXPvtMeasurementStatus
	Private slotMeasurementStatus As RFmxGsmMXPvtSlotMeasurementStatus()
	Private upperMask As AnalogWaveform(Of Single), signalPower As AnalogWaveform(Of Single), lowerMask As AnalogWaveform(Of Single)

	Public Sub Run()
		Try
			InitializeVariables()
			InitializeInstr()
			ConfigureGsm()
			RetrieveResults()
			PrintResults()
		Catch ex As Exception
			DisplayError(ex)
		Finally
			' Close session 

			CloseSession()
			Console.WriteLine("Press any key to exit.....")
			Console.ReadKey()
		End Try
	End Sub

	Private Sub InitializeVariables()
		' Initialize input variables 

		resourceName = "RFSA"
		frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
        frequencyReferenceFrequency = 10000000.0            ' Hz 
        centerFrequency = 890200000.0                       ' Hz 
        referenceLevel = 0.0                                ' dBm 
        externalAttenuation = 0.0                           ' dB 
		band = RFmxGsmMXBand.Pgsm
		linkDirection = RFmxGsmMXLinkDirection.Uplink
		enableTrigger = True
		triggerDelay = 0.0
		iqPowerEdgeLevel = -20.0
		minimumQuietTimeMode = RFmxGsmMXTriggerMinimumQuietTimeMode.Auto
		minimumQuietTime = 0.000582
		numberOfTimeslots = 1
		averagingEnabled = RFmxGsmMXPvtAveragingEnabled.[False]
		averagingCount = 10
		averagingType = RFmxGsmMXPvtAveragingType.Rms
		rbwFilterBandwidth = 500000
		autoTscDetectionEnabled = RFmxGsmMXAutoTscDetectionEnabled.[True]
		modulationType = RFmxGsmMXModulationType.ModulationType8Psk
		burstType = RFmxGsmMXBurstType.NB
		hbFilterWidth = RFmxGsmMXHBFilterWidth.Narrow
		tsc = RFmxGsmMXTsc.Tsc0
		powerControlLevel = 0
		timeout = 10.0
	End Sub

	Private Sub InitializeInstr()
		' Create a new RFmx Session 

		instrSession = New RFmxInstrMX(resourceName, "")
	End Sub

	Private Sub ConfigureGsm()
		instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)

		gsm = instrSession.GetGsmSignalConfiguration()
		gsm.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
		gsm.ConfigureBand("", band)
		gsm.ConfigureLinkDirection("", linkDirection)
		gsm.ConfigureIQPowerEdgeTrigger("", "0", RFmxGsmMXIQPowerEdgeTriggerSlope.Rising, iqPowerEdgeLevel, triggerDelay, minimumQuietTimeMode, _
			minimumQuietTime, RFmxGsmMXIQPowerEdgeTriggerLevelType.Relative, enableTrigger)
		gsm.ConfigureNumberOfTimeslots("", numberOfTimeslots)
		gsm.ConfigureAutoTscDetectionEnabled("", autoTscDetectionEnabled)
        gsm.ConfigureSignalType("slot::all", modulationType, burstType, hbFilterWidth)
        gsm.ConfigureTsc("slot::all", tsc)
        gsm.ConfigurePowerControlLevel("slot::all", powerControlLevel)
		gsm.SelectMeasurements("", RFmxGsmMXMeasurementTypes.Pvt, True)
		gsm.Pvt.Configuration.SetRbwFilterBandwidth("", rbwFilterBandwidth)
		gsm.Pvt.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)
		gsm.Initiate("", "")
	End Sub

	Private Sub RetrieveResults()
		' Retrieve results 

		gsm.Pvt.Results.FetchMeasurementStatus("", timeout, measurementStatus)
		gsm.Pvt.Results.FetchSlotMeasurementArray("", timeout, slotAveragePower, slotBurstWidth, slotMeasurementStatus, slotMaximumPower, _
			slotMinimumPower, slotBurstThreshold)
		gsm.Pvt.Results.FetchPowerTrace("", timeout, upperMask, signalPower, lowerMask)
	End Sub

	Private Sub PrintResults()
		Console.WriteLine("Measurement Status : {0}" & vbLf, measurementStatus)
		For i As Integer = 0 To slotAveragePower.Length - 1
            Console.WriteLine(vbLf & "Slot Measurement        : {0}", i)
            Console.WriteLine("Average Power (dBm)     : {0}", slotAveragePower(i))
            Console.WriteLine("Burst Width (s)         : {0}", slotBurstWidth(i))
            Console.WriteLine("Maximum Power (dBm)     : {0}", slotMaximumPower(i))
            Console.WriteLine("Minimum Power (dBm)     : {0}", slotMinimumPower(i))
            Console.WriteLine("Burst Threshold (dBm)   : {0}", slotBurstThreshold(i))
            Console.WriteLine("Measurement Status      : {0}", slotMeasurementStatus(i))
		Next
	End Sub

	Private Sub CloseSession()
		Try
			If gsm IsNot Nothing Then
				gsm.Dispose()
				gsm = Nothing
			End If

			If instrSession IsNot Nothing Then
				instrSession.Close()
				instrSession = Nothing
			End If
		Catch ex As Exception
			DisplayError(ex)
		End Try
	End Sub

	Private Shared Sub DisplayError(ex As Exception)
		Console.WriteLine("ERROR:" & vbLf & Convert.ToString(ex.[GetType]()) & ": " & ex.Message)
	End Sub
End Class
