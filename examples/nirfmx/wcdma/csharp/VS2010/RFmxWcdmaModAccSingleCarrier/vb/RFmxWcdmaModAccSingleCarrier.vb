'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
'4. Configure Trigger Type and Trigger Parameters.
'5. Configure Uplink Scrambling.
'6. Select ModAcc measurement and enable Traces.
'7. Configure Synchronization Mode and Measurement Interval.
'8. Initiate the Measurement.
'9. Fetch ModAcc Measurements and Traces.
'10. Close RFmx Session. 
Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.WcdmaMX

Public Class RFmxWcdmaModAccSingleCarrier
	Private instrSession As RFmxInstrMX
	Private wcdma As RFmxWcdmaMX

	Private resourceName As String
	Private measurement As RFmxWcdmaMXMeasurementTypes
	Private frequencyReferenceSource As String
	Private frequencyReferenceFrequency As Double
	Private centerFrequency As Double
	Private externalAttenuation As Double

	Private digitalEdgeTriggerSource As String
	Private digitalEdgeTriggerEdge As RFmxWcdmaMXDigitalEdgeTriggerEdge
	Private triggerDelay As Double
	Private referenceLevel As Double
	Private timeout As Double

	Private enableAllTraces As Boolean
	Private enableTrigger As Boolean
	Private uplinkScramblingCode As Integer
	Private uplinkScramblingType As RFmxWcdmaMXUplinkScramblingType
	Private synchronizationMode As RFmxWcdmaMXModAccSynchronizationMode

	Private measurementOffset As Integer
	Private measurementLength As Integer

	Private rmsEvm As Double, peakEvm As Double, rho As Double, frequencyError As Double, chipRateError As Double, rmsMagnitudeError As Double, _
		rmsPhaseError As Double
	Private iqOriginOffset As Double, iqGainImbalance As Double, iqQuadratureError As Double

	Private peakCde As Double, peakActiveCde As Double, peakRcde As Double
	Private peakCdeCode As Integer, peakActiveCdeSpreadingFactor As Integer, peakActiveCdeCode As Integer, peakRcdeSpreadingFactor As Integer, peakRcdeCode As Integer
	Private peakCdeBranch As RFmxWcdmaMXModAccPeakCdeBranch
	Private peakActiveCdeBranch As RFmxWcdmaMXModAccPeakActiveCdeBranch
	Private peakRcdeBranch As RFmxWcdmaMXModAccPeakRcdeBranch
	Private evm As AnalogWaveform(Of Single)
	Private constellation As ComplexSingle()

	Public Sub Run()
		Try
			InitializeVariable()
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

	Private Sub InitializeVariable()
		' Initialize input variables 


		resourceName = "RFSA"
		measurement = RFmxWcdmaMXMeasurementTypes.ModAcc
		frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
		frequencyReferenceFrequency = 10000000.0
		' Hz 
		centerFrequency = 1950000000.0
		' Hz 
		externalAttenuation = 0.0
		' dB 

		digitalEdgeTriggerSource = RFmxWcdmaMXConstants.Pfi0
		digitalEdgeTriggerEdge = RFmxWcdmaMXDigitalEdgeTriggerEdge.Rising
		triggerDelay = 0.0
		' seconds 
		referenceLevel = 0.0
		' dBm 

		synchronizationMode = RFmxWcdmaMXModAccSynchronizationMode.Slot
		measurementOffset = 0
		'slots
		measurementLength = 1
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

		wcdma.ConfigureDigitalEdgeTrigger("", digitalEdgeTriggerSource, digitalEdgeTriggerEdge, triggerDelay, enableTrigger)
		wcdma.ConfigureUplinkScrambling("", uplinkScramblingCode, uplinkScramblingType)

		wcdma.SelectMeasurements("", measurement, enableAllTraces)

		wcdma.ModAcc.Configuration.ConfigureSynchronizationModeAndInterval("", synchronizationMode, measurementOffset, measurementLength)

		wcdma.Initiate("", "")


	End Sub

	Private Sub RetrieveResults()

		wcdma.ModAcc.Results.FetchEvm("", timeout, rmsEvm, peakEvm, rho, frequencyError, _
			chipRateError, rmsMagnitudeError, rmsPhaseError)
		wcdma.ModAcc.Results.FetchIQImpairments("", timeout, iqOriginOffset, iqGainImbalance, iqQuadratureError)
		wcdma.ModAcc.Results.FetchPeakCde("", timeout, peakCde, peakCdeCode, peakCdeBranch)
		wcdma.ModAcc.Results.FetchPeakActiveCde("", timeout, peakActiveCde, peakActiveCdeSpreadingFactor, peakActiveCdeCode, peakActiveCdeBranch)
		wcdma.ModAcc.Results.FetchRcde("", timeout, peakRcde, peakRcdeSpreadingFactor, peakRcdeCode, peakRcdeBranch)
		wcdma.ModAcc.Results.FetchEvmTrace("", timeout, evm)
		wcdma.ModAcc.Results.FetchConstellationTrace("", timeout, constellation)
	End Sub

	Private Sub PrintResults()

		Console.WriteLine("----------------------------EVM---------------------------")
		Console.WriteLine("RMS EVM (%)                                      : {0}", rmsEvm)
		Console.WriteLine("Peak EVM (%)                                     : {0}", peakEvm)
		Console.WriteLine("Rho                                              : {0}", rho)
		Console.WriteLine("Frequency Error (Hz)                             : {0}", frequencyError)
		Console.WriteLine("Chip Rate Error (ppm)                            : {0}", chipRateError)
		Console.WriteLine("RMS Magnitude Error (%)                          : {0}", rmsMagnitudeError)
		Console.WriteLine("RMS Phase Error (deg)                            : {0}", rmsPhaseError)

		Console.WriteLine(vbLf & "----------------------IQ Impairments----------------------")
		Console.WriteLine(vbLf & "I/Q Origin Offset (dB)                           : {0}", iqOriginOffset)
		Console.WriteLine("I/Q Gain Imbalance (dB)                          : {0}", iqGainImbalance)
		Console.WriteLine("I/Q Quadrature Error (deg)                       : {0}", iqQuadratureError)
		Console.WriteLine(vbLf & "---------------------Code Domain Error--------------------")
		Console.WriteLine("Peak CDE (dB)                                    : {0}", peakCde)
		Console.WriteLine("Peak CDE Code                                    : {0}", peakCdeCode)
		Console.WriteLine("Peak CDE Branch                                  : {0}", peakCdeBranch)
		Console.WriteLine("Peak Active CDE (dB)                             : {0}", peakActiveCde)
		Console.WriteLine("Peak Active CDE Code                             : {0}", peakActiveCdeCode)
		Console.WriteLine("Peak Active CDE Spreading Factor                 : {0}", peakActiveCdeSpreadingFactor)
		Console.WriteLine("Peak Active CDE Branch                           : {0}", peakActiveCdeBranch)
		Console.WriteLine("Peak RCDE (dB)                                   : {0}", peakRcde)
		Console.WriteLine("Peak RCDE Code                                   : {0}", peakRcdeCode)
		Console.WriteLine("Peak RCDE Spreading Factor                       : {0}", peakRcdeSpreadingFactor)
		Console.WriteLine("Peak RCDE Branch                                 : {0}", peakRcdeBranch)

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
