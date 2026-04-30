'Steps:
'1. Open a new RFmx session
'2. Configure the basic instrument properties: Clock Source and Clock Frequency
'3. Configure the basic signal properties: Center Frequency, Reference Level and External Attenuation
'4. Configure the trigger properties
'5. Select all measurements (ModAcc, ACP, CHP, OBW, SEM)  and enable the traces
'6. Configure Measurement Offset/ Length, Uplink Scrambling Code and Midamble Code/ Shift for the ModAcc measurement
'7. Configure Sweep Time and Averaging Parameters for the ACP measurement
'8. Configure Sweep Time and Averaging Parameters for the CHP measurement
'9. Configure Sweep Time and Averaging Parameters for the OBW measurement
'10. Configure Sweep Time and Averaging Parameters for the SEM measurement
'11. Initiate Measurement
'12. Fetch SEM Measurements and Traces
'13. Fetch OBW Measurements and Traces
'14. Fetch CHP Measurements and Traces
'15. Fetch ACP Measurements and Traces
'16. Fetch ModAcc Measurements and Traces
'17. Close the RFmx session

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.TdscdmaMX

Public Class RFmxTdscdmaModAccAcpChpObwSemComposite
	Private instrSession As RFmxInstrMX
	Private tdscdma As RFmxTdscdmaMX
	Private resourceName As String, frequencyReferenceSource As String, iqPowerEdgeTriggerSource As String
	Private centerFrequency As Double, referenceLevel As Double, externalAttenuation As Double, frequencyReferenceFrequency As Double, sweepTimeInterval As Double, triggerDelay As Double, _
		minimumQuietTimeDuration As Double, iqPowerEdgeTriggerLevel As Double, timeout As Double

	Private averagingCount As Integer, measurementOffset As Integer, measurementLength As Integer, maximumNumberOfUsers As Integer, midambleShift As Integer, uplinkScramblingCode As Integer

	Private iqPowerEdgeTriggerSlope As RFmxTdscdmaMXIQPowerEdgeTriggerSlope
	Private minimumQuietTimeMode As RFmxTdscdmaMXTriggerMinimumQuietTimeMode
	Private enableTrigger As Boolean
	Private iqPowerEdgeTriggerLevelType As RFmxTdscdmaMXIQPowerEdgeTriggerLevelType

	Private synchronizationMode As RFmxTdscdmaMXModAccSynchronizationMode

	Private midambleAutoDetectionMode As RFmxTdscdmaMXMidambleAutoDetectionMode


	Private rmsCompositeEvm As Double, peakCompositeEvm As Double, compositeRho As Double, frequencyError As Double, chipRateError As Double, rmsCompositeMagnitudeError As Double, _
		rmsCompositePhaseError As Double

	Private carrierAbsolutePowerAcp As Double
	Private lowerRelativePower As Double()
	Private upperRelativePower As Double()
	Private lowerAbsolutePower As Double()
	Private upperAbsolutePower As Double()

	Private carrierAbsolutePowerChp As Double

	Private stopFrequency As Double, startFrequency As Double, occupiedBandwidth As Double, absolutePower As Double

	Private carrierAbsoluteIntegratedPower As Double
	Private lowerOffsetMargin As Double()
	Private lowerOffsetMarginAbsolutePower As Double()
	Private lowerOffsetMarginRelativePower As Double()
	Private lowerOffsetMarginFrequency As Double()
	Private lowerOffsetMeasurementStatus As RFmxTdscdmaMXSemLowerOffsetMeasurementStatus()

	Private measurementStatus As RFmxTdscdmaMXSemMeasurementStatus

	Private upperOffsetMargin As Double()
	Private upperOffsetMarginAbsolutePower As Double()
	Private upperOffsetMarginRelativePower As Double()
	Private upperOffsetMarginFrequency As Double()
	Private upperOffsetMeasurementStatus As RFmxTdscdmaMXSemUpperOffsetMeasurementStatus()

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
		uplinkScramblingCode = 0
		frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
		frequencyReferenceFrequency = 10000000.0
		' Hz 

		triggerDelay = 0.0
		' seconds 
        minimumQuietTimeDuration = 0.00008
		' seconds 
		iqPowerEdgeTriggerLevel = -20.0
		'dB
		iqPowerEdgeTriggerSource = "0"
		iqPowerEdgeTriggerSlope = RFmxTdscdmaMXIQPowerEdgeTriggerSlope.Rising
		enableTrigger = True
		iqPowerEdgeTriggerLevelType = RFmxTdscdmaMXIQPowerEdgeTriggerLevelType.Relative
		minimumQuietTimeMode = RFmxTdscdmaMXTriggerMinimumQuietTimeMode.Auto

		'Sweep Time
		sweepTimeInterval = 0.00066
		' seconds 

		'Averaging
		averagingCount = 10

		synchronizationMode = RFmxTdscdmaMXModAccSynchronizationMode.Slot

		measurementOffset = 0
		measurementLength = 1
		maximumNumberOfUsers = 16
		midambleShift = 8
		midambleAutoDetectionMode = RFmxTdscdmaMXMidambleAutoDetectionMode.MidambleShift
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
			minimumQuietTimeDuration, iqPowerEdgeTriggerLevelType, enableTrigger)

		tdscdma.SelectMeasurements("", RFmxTdscdmaMXMeasurementTypes.ModAcc Or RFmxTdscdmaMXMeasurementTypes.Acp Or RFmxTdscdmaMXMeasurementTypes.Chp Or RFmxTdscdmaMXMeasurementTypes.Obw Or RFmxTdscdmaMXMeasurementTypes.Sem, True)


		tdscdma.ConfigureMidambleShift("", midambleAutoDetectionMode, maximumNumberOfUsers, midambleShift)
		tdscdma.ModAcc.Configuration.ConfigureSynchronizationModeAndInterval("", synchronizationMode, measurementOffset, measurementLength)
		tdscdma.ConfigureUplinkScramblingCode("", uplinkScramblingCode)

		tdscdma.Acp.Configuration.ConfigureSweepTime("", RFmxTdscdmaMXAcpSweepTimeAuto.[False], sweepTimeInterval)
		tdscdma.Acp.Configuration.ConfigureAveraging("", RFmxTdscdmaMXAcpAveragingEnabled.[False], averagingCount, RFmxTdscdmaMXAcpAveragingType.Rms)

		tdscdma.Chp.Configuration.ConfigureSweepTime("", RFmxTdscdmaMXChpSweepTimeAuto.[False], sweepTimeInterval)
		tdscdma.Chp.Configuration.ConfigureAveraging("", RFmxTdscdmaMXChpAveragingEnabled.[False], averagingCount, RFmxTdscdmaMXChpAveragingType.Rms)

		tdscdma.Obw.Configuration.ConfigureSweepTime("", RFmxTdscdmaMXObwSweepTimeAuto.[False], sweepTimeInterval)
		tdscdma.Obw.Configuration.ConfigureAveraging("", RFmxTdscdmaMXObwAveragingEnabled.[False], averagingCount, RFmxTdscdmaMXObwAveragingType.Rms)

		tdscdma.Sem.Configuration.ConfigureSweepTime("", RFmxTdscdmaMXSemSweepTimeAuto.[False], sweepTimeInterval)
		tdscdma.Sem.Configuration.ConfigureAveraging("", RFmxTdscdmaMXSemAveragingEnabled.[False], averagingCount, RFmxTdscdmaMXSemAveragingType.Rms)

		tdscdma.Initiate("", "")
	End Sub

	Private Sub RetrieveResults()
		' Retrieve results 

		tdscdma.Sem.Results.FetchLowerOffsetMarginArray("", timeout, lowerOffsetMeasurementStatus, lowerOffsetMargin, lowerOffsetMarginFrequency, lowerOffsetMarginAbsolutePower, _
			lowerOffsetMarginRelativePower)

		tdscdma.Sem.Results.FetchUpperOffsetMarginArray("", timeout, upperOffsetMeasurementStatus, upperOffsetMargin, upperOffsetMarginFrequency, upperOffsetMarginAbsolutePower, _
			upperOffsetMarginRelativePower)
		tdscdma.Sem.Results.FetchCarrierAbsoluteIntegratedPower("", timeout, carrierAbsoluteIntegratedPower)
		tdscdma.Sem.Results.FetchMeasurementStatus("", timeout, measurementStatus)

		tdscdma.Obw.Results.FetchMeasurement("", timeout, occupiedBandwidth, absolutePower, startFrequency, stopFrequency)


		tdscdma.Chp.Results.FetchCarrierAbsolutePower("", timeout, carrierAbsolutePowerChp)

		tdscdma.Acp.Results.FetchOffsetMeasurementArray("", timeout, lowerRelativePower, upperRelativePower, lowerAbsolutePower, upperAbsolutePower)

		tdscdma.Acp.Results.FetchCarrierAbsolutePower("", timeout, carrierAbsolutePowerAcp)


		tdscdma.ModAcc.Results.FetchCompositeEvm("", timeout, rmsCompositeEvm, peakCompositeEvm, compositeRho, frequencyError, _
			chipRateError, rmsCompositeMagnitudeError, rmsCompositePhaseError)

	End Sub

	Private Sub PrintResults()
		Console.WriteLine("-----------------------------ModAcc Results----------------------------")
		Console.WriteLine("-------------------Composite EVM Results--------------------")
        Console.WriteLine("RMS Composite EVM (%)                 {0}", rmsCompositeEvm)
        Console.WriteLine("Peak Composite EVM (%)                {0}", peakCompositeEvm)
        Console.WriteLine("Composite Rho                         {0}", compositeRho)
        Console.WriteLine("Frequency Error (Hz)                  {0}", frequencyError)
        Console.WriteLine("Chip Rate Error (ppm)                 {0}", chipRateError)
        Console.WriteLine("RMS Composite Phase Error (deg)       {0}", rmsCompositePhaseError)
        Console.WriteLine("RMS Composite Magnitude Error (%)     {0}", rmsCompositeMagnitudeError)



		Console.WriteLine("----------------------------------ACP Results----------------------------")

		Console.WriteLine("---------------------Carrier Measurements-----------------------" & vbLf)
        Console.WriteLine("Carrier Absolute Power (dBm)          {0}", carrierAbsolutePowerAcp)

		Console.WriteLine(vbLf & "-----------------Offset Channel Measurements------------------" & vbLf)
		For i As Integer = 0 To lowerRelativePower.Length - 1
			Console.WriteLine("----Offset {0}" & vbLf, i)
			Console.WriteLine("Lower Relative Power (dB)            {0}", lowerRelativePower(i))
			Console.WriteLine("Upper Relative Power (dB)            {0}", upperRelativePower(i))
			Console.WriteLine("Lower Absolute Power (dBm)           {0}", lowerAbsolutePower(i))
			Console.WriteLine("Upper Absolute Power (dBm)           {0}", upperAbsolutePower(i))
		Next
        Console.WriteLine("-----------------------------------CHP Results-------------------------------")
        Console.WriteLine("Carrier Absolute Power (dBm)           {0}", carrierAbsolutePowerChp)
		Console.WriteLine("------------------------------------OBW Results------------------------------")
        Console.WriteLine("Occupied Bandwidth (Hz)                {0}", occupiedBandwidth)
        Console.WriteLine("Absolute Power (dBm)                   {0}", absolutePower)
        Console.WriteLine("Start Frequency (Hz)                   {0}", startFrequency)
        Console.WriteLine("Stop Frequency (Hz)                    {0}", stopFrequency)

		Console.WriteLine("--------------------------------------SEM Results----------------------------")


        Console.WriteLine("Measurement Status  :                   {0}" & vbLf, measurementStatus)
		Console.WriteLine(vbLf & "---------------------------Carrier Measurements-----------------------" & vbLf)
        Console.WriteLine("Carrier Absolute Integrated Power(dBm): {0}", carrierAbsoluteIntegratedPower)

		Console.WriteLine(vbLf & "--------------Offset segment measurements ---------------------------" & vbLf)
		For i As Integer = 0 To lowerOffsetMargin.Length - 1
			Console.WriteLine("Offset {0}" & vbLf, i)

			Console.WriteLine("Lower Offset : Margin (dB):                           {0}", lowerOffsetMargin(i))
			Console.WriteLine("Lower Offset : Margin Absolute Power (dBm):           {0}", lowerOffsetMarginAbsolutePower(i))
			Console.WriteLine("Lower Offset : Margin Relative Power (dB):            {0}", lowerOffsetMarginRelativePower(i))
			Console.WriteLine("Lower Offset : Margin Frequency (Hz):                 {0}", lowerOffsetMarginFrequency(i))


			Console.WriteLine("Lower Offset : Measurement Status :                   {0}" & vbLf, lowerOffsetMeasurementStatus(i))


			Console.WriteLine("Upper Offset : Margin (dB):                            {0}", upperOffsetMargin(i))
			Console.WriteLine("Upper Offset : Margin Absolute Power (dBm):            {0}", upperOffsetMarginAbsolutePower(i))
			Console.WriteLine("Upper Offset : Margin Relative Power (dB):             {0}", upperOffsetMarginRelativePower(i))
			Console.WriteLine("Upper Offset : Margin Frequency (Hz):                  {0}", upperOffsetMarginFrequency(i))


			Console.WriteLine("Upper Offset : Measurement Status :                    {0}" & vbLf, upperOffsetMeasurementStatus(i))
		Next
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
