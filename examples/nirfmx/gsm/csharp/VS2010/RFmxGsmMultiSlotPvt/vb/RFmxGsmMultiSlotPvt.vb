'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
'4. Configure operating Band.
'5. Configure Link Direction.
'6. Configure Trigger Parameters for IQ Power Edge Trigger.
'7. Configure Auto TSC Detection Enabled.
'8. Configure Number of Timeslots.
'9. Configure Signal Type.
'10. Configure TSC.
'11. Configure Power Control Level.
'12. Select PVT measurement and enable Traces.
'13. Configure RBW Filter Bandwidth (Hz) for PVT measurement.
'14. Configure Averaging Parameters for PVT measurement.
'15. Initiate the Measurement.
'16  Fetch PVT Measurements and Traces.
'17. Close RFmx Session. 

Imports NationalInstruments.RFmx.GsmMX
Imports NationalInstruments.RFmx.InstrMX

Public Class RFmxGsmMultiSlotPvtExample
	Private instrSession As RFmxInstrMX
	Private gsm As RFmxGsmMX
	Private resourceName As String

	Private frequencyReferenceFrequency As Double, centerFrequency As Double, referenceLevel As Double, externalAttenuation As Double, iqPowerEdgeLevel As Double, triggerDelay As Double,
		minimumQuietTime As Double, timeout As Double, rbwFilterBanwidth As Double

	Private slotAveragePower As Double(), slotBurstWidth As Double(), slotMaximumPower As Double(), slotMinimumPower As Double(), slotBurstThreshold As Double()

	Private enableTrigger As Boolean
	Private numberOfSlots As Integer, numberOfTimeslots As Integer, averagingCount As Integer
	Private frequencyReferenceSource As String, slotString As String
	Private band As RFmxGsmMXBand
	Private linkDirection As RFmxGsmMXLinkDirection
	Private averagingType As RFmxGsmMXPvtAveragingType
	Private minimumQuietTimeMode As RFmxGsmMXTriggerMinimumQuietTimeMode
	Private autoTscDetectionEnabled As RFmxGsmMXAutoTscDetectionEnabled
	Private averagingEnabled As RFmxGsmMXPvtAveragingEnabled
	Private measurementStatus As RFmxGsmMXPvtMeasurementStatus
	Private slotMeasurementStatus As RFmxGsmMXPvtSlotMeasurementStatus()
	Private upperMask As AnalogWaveform(Of Single), signalPower As AnalogWaveform(Of Single), lowerMask As AnalogWaveform(Of Single)

	Private Structure SlotConfiguration
		Public modulationType As RFmxGsmMXModulationType
		Public burstType As RFmxGsmMXBurstType
		Public hbFilterWidth As RFmxGsmMXHBFilterWidth
		Public tsc As RFmxGsmMXTsc
		Public powerControlLevel As Integer
	End Structure
	Private slotConfigurationInput As SlotConfiguration()

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
        frequencyReferenceFrequency = 10000000.0           ' Hz 
        centerFrequency = 890200000.0                      ' Hz 
        referenceLevel = 0.0                               ' dBm 
        externalAttenuation = 0.0                          ' dB 
		numberOfSlots = 1
		slotConfigurationInput = New SlotConfiguration(numberOfSlots - 1) {}
		band = RFmxGsmMXBand.Pgsm
		linkDirection = RFmxGsmMXLinkDirection.Uplink
		triggerDelay = 0.0
		enableTrigger = True
		iqPowerEdgeLevel = -20.0
		minimumQuietTimeMode = RFmxGsmMXTriggerMinimumQuietTimeMode.Auto
		minimumQuietTime = 0.000582
		numberOfTimeslots = 1
		averagingEnabled = RFmxGsmMXPvtAveragingEnabled.[False]
		averagingCount = 10
		rbwFilterBanwidth = 500000
		averagingType = RFmxGsmMXPvtAveragingType.Rms

		autoTscDetectionEnabled = RFmxGsmMXAutoTscDetectionEnabled.[True]
		timeout = 10.0

		For i As Integer = 0 To numberOfSlots - 1
			slotConfigurationInput(i).modulationType = RFmxGsmMXModulationType.ModulationType8Psk
			slotConfigurationInput(i).burstType = RFmxGsmMXBurstType.NB
            slotConfigurationInput(i).hbFilterWidth = RFmxGsmMXHBFilterWidth.Narrow          ' Hz 
			slotConfigurationInput(i).tsc = RFmxGsmMXTsc.Tsc0
			slotConfigurationInput(i).powerControlLevel = 0
		Next
	End Sub

	Private Sub InitializeInstr()
		' Create a new RFmx Session 

		instrSession = New RFmxInstrMX(resourceName, "")
	End Sub

	Private Sub ConfigureGsm()
		' Configure PVT measurements 

		instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)

		gsm = instrSession.GetGsmSignalConfiguration()
		gsm.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
		gsm.ConfigureBand("", band)
		gsm.ConfigureLinkDirection("", linkDirection)
		gsm.ConfigureIQPowerEdgeTrigger("", "0", RFmxGsmMXIQPowerEdgeTriggerSlope.Rising, iqPowerEdgeLevel, triggerDelay, minimumQuietTimeMode, _
			minimumQuietTime, RFmxGsmMXIQPowerEdgeTriggerLevelType.Relative, enableTrigger)

		gsm.ConfigureNumberOfTimeslots("", numberOfTimeslots)
		gsm.ConfigureAutoTscDetectionEnabled("", autoTscDetectionEnabled)
		For i As Integer = 0 To numberOfSlots - 1
			slotString = RFmxGsmMX.BuildSlotString("", i)
			gsm.ConfigureSignalType(slotString, slotConfigurationInput(i).modulationType, slotConfigurationInput(i).burstType, slotConfigurationInput(i).hbFilterWidth)
			gsm.ConfigureTsc(slotString, slotConfigurationInput(i).tsc)
			gsm.ConfigurePowerControlLevel(slotString, slotConfigurationInput(i).powerControlLevel)
		Next

		gsm.SelectMeasurements("", RFmxGsmMXMeasurementTypes.Pvt, True)
		gsm.Pvt.Configuration.SetRbwFilterBandwidth("", rbwFilterBanwidth)
		gsm.Pvt.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)
		gsm.Initiate("", "")
	End Sub

	Private Sub RetrieveResults()
		gsm.Pvt.Results.FetchMeasurementStatus("", timeout, measurementStatus)
		gsm.Pvt.Results.FetchSlotMeasurementArray("", timeout, slotAveragePower, slotBurstWidth, slotMeasurementStatus, slotMaximumPower, _
			slotMinimumPower, slotBurstThreshold)
		gsm.Pvt.Results.FetchPowerTrace("", timeout, upperMask, signalPower, lowerMask)
	End Sub

	Private Sub PrintResults()
        Console.WriteLine("Measurement Status   : {0}", measurementStatus)
		Console.WriteLine(vbLf & "--------------Slot Measurement--------------" & vbLf)
		For i As Integer = 0 To slotAveragePower.Length - 1
            Console.WriteLine("Slot Measurement : {0}", i)
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
