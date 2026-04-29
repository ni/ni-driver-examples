'Steps:
'1. Open a new RFmx session
'2. Configure the basic instrument properties: Clock Source and Clock Frequency
'3. Configure the basic signal properties: Center Frequency, Reference Level and External Attenuation
'4. Configure the trigger properties
'5. Select ModAcc measurement and enable the traces
'6. Configure Uplink Scrambling Code
'7. Configure the basic Measurement Settings
'8. Configure the Midamble Settings 
'9. Initiate Measurement
'10. Fetch ModAcc Measurements and Traces
'11. Close the RFmx session

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.TdscdmaMX

Public Class RFmxTdscdmaModAccPilot
	Private instrSession As RFmxInstrMX
	Private tdscdma As RFmxTdscdmaMX
	Private resourceName As String, frequencyReferenceSource As String, iqPowerEdgeTriggerSource As String
	Private centerFrequency As Double, referenceLevel As Double, externalAttenuation As Double, frequencyReferenceFrequency As Double, triggerDelay As Double, minimumQuietTime As Double, _
		iqPowerEdgeTriggerLevel As Double

	Private iqPowerEdgeTriggerSlope As RFmxTdscdmaMXIQPowerEdgeTriggerSlope
	Private minimumQuietTimeMode As RFmxTdscdmaMXTriggerMinimumQuietTimeMode
	Private enableTrigger As Boolean
	Private iqPowerEdgeTriggerLevelType As RFmxTdscdmaMXIQPowerEdgeTriggerLevelType

	Private pilotCode As Integer
	Private slotType As RFmxTdscdmaMXModAccSlotType
	Private averagingEnabled As RFmxTdscdmaMXModAccAveragingEnabled
	Private averagingCount As Integer

	Private timeout As Double
	Private rmsPilotEvm As Double, peakPilotEvm As Double, pilotRho As Double, frequencyError As Double, rmsPilotMagnitudeError As Double, rmsPilotPhaseError As Double
	Private rmsCompositeEvm As Double, peakCompositeEvm As Double, compositeRho As Double, chipRateError As Double, rmsCompositeMagnitudeError As Double, rmsCompositePhaseError As Double
	Private iqOriginOffset As Double, iqGainImbalance As Double, iqQuadratureError As Double
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
			Console.WriteLine("Press any key to exit.....")
			Console.ReadKey()
		End Try
	End Sub

	Private Sub InitializeVariables()
		' Initialize input variables 


		resourceName = "RFSA"

		centerFrequency = 1910000000.0
		' Hz 
		referenceLevel = 0.0
		' dBm 
		externalAttenuation = 0.0
		' dB 

		frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
		frequencyReferenceFrequency = 10000000.0
		' Hz 

		triggerDelay = 0.0
		' seconds 
        minimumQuietTime = 0.00005
		' seconds 
		iqPowerEdgeTriggerLevel = -20.0
		'dB
		iqPowerEdgeTriggerSource = "0"
		iqPowerEdgeTriggerSlope = RFmxTdscdmaMXIQPowerEdgeTriggerSlope.Rising
		enableTrigger = True
		iqPowerEdgeTriggerLevelType = RFmxTdscdmaMXIQPowerEdgeTriggerLevelType.Relative
		minimumQuietTimeMode = RFmxTdscdmaMXTriggerMinimumQuietTimeMode.Auto

		pilotCode = 0
		slotType = RFmxTdscdmaMXModAccSlotType.Pilot
		averagingEnabled = RFmxTdscdmaMXModAccAveragingEnabled.[False]
		averagingCount = 10

		timeout = 10
		' seconds 
	End Sub

	Private Sub InitializeInstr()
		' Create a new RFmx Session 

		instrSession = New RFmxInstrMX(resourceName, "")
	End Sub

	Private Sub ConfigureTdscdma()
		' Get SpecAn signal 

		tdscdma = instrSession.GetTdscdmaSignalConfiguration()

		' Configure measurement 

		instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
		tdscdma.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
		tdscdma.ConfigureIQPowerEdgeTrigger("", iqPowerEdgeTriggerSource, iqPowerEdgeTriggerSlope, iqPowerEdgeTriggerLevel, triggerDelay, minimumQuietTimeMode, _
			minimumQuietTime, iqPowerEdgeTriggerLevelType, enableTrigger)
		tdscdma.ConfigurePilot("", pilotCode)
		tdscdma.SelectMeasurements("", RFmxTdscdmaMXMeasurementTypes.ModAcc, True)
		tdscdma.ModAcc.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount)
		tdscdma.ModAcc.Configuration.ConfigureSlotType("", slotType)
		tdscdma.Initiate("", "")
	End Sub

	Private Sub RetrieveResults()
		Dim constellation As ComplexSingle() = Nothing
		Dim evm As AnalogWaveform(Of Single) = Nothing
		' Retrieve results 

		tdscdma.ModAcc.Results.FetchPilotEvm("", timeout, rmsPilotEvm, peakPilotEvm, pilotRho, rmsPilotMagnitudeError, _
			rmsPilotPhaseError)
		tdscdma.ModAcc.Results.FetchCompositeEvm("", timeout, rmsCompositeEvm, peakCompositeEvm, compositeRho, frequencyError, _
			chipRateError, rmsCompositeMagnitudeError, rmsCompositePhaseError)
		tdscdma.ModAcc.Results.FetchConstellationTrace("", timeout, constellation)
		tdscdma.ModAcc.Results.FetchIQImpairments("", timeout, iqOriginOffset, iqGainImbalance, iqQuadratureError)
		tdscdma.ModAcc.Results.FetchEvmTrace("", timeout, evm)
	End Sub

    Private Sub PrintResults()
        Console.WriteLine("--------------------Pilot EVM Results--------------------")
        Console.WriteLine("RMS Pilot EVM (%)             {0}", rmsPilotEvm)
        Console.WriteLine("Peak Pilot EVM (%)            {0}", peakPilotEvm)
        Console.WriteLine("Pilot Rho                     {0}", pilotRho)
        Console.WriteLine("RMS Pilot Magnitude Error (%) {0}", rmsPilotMagnitudeError)
        Console.WriteLine("RMS Pilot Phase Error (deg)   {0}", rmsPilotPhaseError)
        Console.WriteLine("Frequency Error (Hz)          {0}", frequencyError)

        Console.WriteLine(vbLf & "---------------------IQ Impairments------------------------")
        Console.WriteLine("I/Q Origin Offset (dB)         {0}", iqOriginOffset)
        Console.WriteLine("I/Q Gain Imbalance (dB)        {0}", iqGainImbalance)
        Console.WriteLine("I/Q Quadrature Error (deg)     {0}", iqQuadratureError)
    End Sub

	Private Sub CloseSession()
		Try
			If tdscdma IsNot Nothing Then
				tdscdma.Dispose()
				tdscdma = Nothing
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
