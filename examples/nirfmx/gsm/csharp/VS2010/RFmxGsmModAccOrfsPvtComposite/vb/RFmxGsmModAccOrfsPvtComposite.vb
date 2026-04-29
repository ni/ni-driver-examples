' Steps:
' 1. Open a new RFmx Session.
' 2. Configure Frequency Reference.
' 3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
' 4. Configure operating Band.
' 5. Configure Link Direction.
' 6. Configure Trigger Parameters for IQ Power Edge Trigger.
' 7. Select  measurements and enable Traces.
' 8. Configure Number of Timeslots.
' 9. Configure Averaging Parameters for ModAcc measurement.
' 10. Configure Averaging Parameters for ORFS measurement.
' 11. Configure Averaging Parameters for PVT measurement.
' 12. Configure ORFS Measurement Type.
' 13. Configure Offset Frequency Mode for ORFS measurement.
' 14. Configure Auto TSC Detection Enabled.
' 15. Configure Signal Type.
' 16. Configure TSC.
' 17. Configure Power Control Level.
' 18. Initiate the Measurement.
' 19  Fetch ModAcc/ORFS/PVT Measurements and Traces.
' 20. Close RFmx Session. 


Imports NationalInstruments.RFmx.GsmMX
Imports NationalInstruments.RFmx.InstrMX

Public Class RFmxGsmModAccOrfsPvtCompositeExample
	Private instrSession As RFmxInstrMX
	Private gsm As RFmxGsmMX
	Private resourceName As String

	Private frequencyReferenceFrequency As Double, centerFrequency As Double, referenceLevel As Double, externalAttenuation As Double, iqPowerEdgeLevel As Double, triggerDelay As Double, _
		minimumQuietTime As Double, timeout As Double, meanRmsEvm As Double, maximumRmsEvm As Double, meanPeakEvm As Double, meanRmsPhaseError As Double, _
		maximumPeakEvm As Double, ninetyFifthPercentileEvm As Double, meanFrequencyErrorEvm As Double, meanFrequencyErrorPfer As Double, meanIQGainImbalance As Double, maximumIQGainImbalance As Double, _
		meanIQOriginOffset As Double, maximumIQOriginOffset As Double, maximumRmsPhaseError As Double, modulationCarrierPower As Double, meanPeakPhaseError As Double, maximumPeakPhaseError As Double, _
		switchingCarrierPower As Double

	Private modulationResultsLowerRelativePower As Double(), modulationResultsUpperRelativePower As Double(), modulationResultsLowerAbsolutePower As Double(), modulationResultsUpperAbsolutePower As Double(), switchingResultsLowerRelativePower As Double(), switchingResultsUpperRelativePower As Double(), _
		switchingResultsLowerAbsolutePower As Double(), switchingResultsUpperAbsolutePower As Double(), slotAveragePower As Double(), slotBurstWidth As Double(), slotMaximumPower As Double(), slotMinimumPower As Double(), _
		slotBurstThreshold As Double()

	Private modulationPowerTraceOffsetFrequency As Single(), modulationPowerTraceAbsolutePower As Single(), modulationPowerTraceRelativePower As Single(), switchingPowerTraceOffsetFrequency As Single(), switchingPowerTraceAbsolutePower As Single(), switchingPowerTraceRelativePower As Single()


	Private enableTrigger As Boolean
	Private powerControlLevel As Integer, numberOfTimeslots As Integer, averagingCount As Integer, peakEvmSymbol As Integer, peakSymbol As Integer
	Private frequencyReferenceSource As String
	Private detectedTsc As RFmxGsmMXModAccDetectedTsc()
	Private band As RFmxGsmMXBand
	Private linkDirection As RFmxGsmMXLinkDirection
	Private orfsAveragingType As RFmxGsmMXOrfsAveragingType
	Private pvtAveragingType As RFmxGsmMXPvtAveragingType
	Private measurementType As RFmxGsmMXOrfsMeasurementType
	Private burstType As RFmxGsmMXBurstType
	Private hbFilterWidth As RFmxGsmMXHBFilterWidth
	Private minimumQuietTimeMode As RFmxGsmMXTriggerMinimumQuietTimeMode
	Private autoTscDetectionEnabled As RFmxGsmMXAutoTscDetectionEnabled
	Private modulationType As RFmxGsmMXModulationType
	Private modAccaveragingEnabled As RFmxGsmMXModAccAveragingEnabled
	Private orfsAveragingEnabled As RFmxGsmMXOrfsAveragingEnabled
	Private pvtAveragingEnabled As RFmxGsmMXPvtAveragingEnabled
	Private tsc As RFmxGsmMXTsc
	Private measurementStatus As RFmxGsmMXPvtMeasurementStatus
	Private slotMeasurementStatus As RFmxGsmMXPvtSlotMeasurementStatus()
	Private evm As AnalogWaveform(Of Single), meanTraceError As AnalogWaveform(Of Single), upperMask As AnalogWaveform(Of Single), signalPower As AnalogWaveform(Of Single), lowerMask As AnalogWaveform(Of Single)

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
        frequencyReferenceFrequency = 10000000.0             ' Hz 
        centerFrequency = 890200000.0                        ' Hz 
        referenceLevel = 0.0                                 ' dBm 
        externalAttenuation = 0.0                            ' dB 
		band = RFmxGsmMXBand.Pgsm
		linkDirection = RFmxGsmMXLinkDirection.Uplink
		enableTrigger = True
		triggerDelay = 0.0
		iqPowerEdgeLevel = -20.0
		minimumQuietTimeMode = RFmxGsmMXTriggerMinimumQuietTimeMode.Auto
		minimumQuietTime = 0.000582
		numberOfTimeslots = 1
		modAccaveragingEnabled = RFmxGsmMXModAccAveragingEnabled.[False]
		orfsAveragingEnabled = RFmxGsmMXOrfsAveragingEnabled.[False]
		pvtAveragingEnabled = RFmxGsmMXPvtAveragingEnabled.[False]
		averagingCount = 10
		pvtAveragingType = RFmxGsmMXPvtAveragingType.Rms
		orfsAveragingType = RFmxGsmMXOrfsAveragingType.Rms
		measurementType = RFmxGsmMXOrfsMeasurementType.ModulationAndSwitching
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
		gsm.SelectMeasurements("", RFmxGsmMXMeasurementTypes.ModAcc Or RFmxGsmMXMeasurementTypes.Orfs Or RFmxGsmMXMeasurementTypes.Pvt, True)
		gsm.ConfigureNumberOfTimeslots("", numberOfTimeslots)
		gsm.ModAcc.Configuration.ConfigureAveraging("", modAccaveragingEnabled, averagingCount)
		gsm.Orfs.Configuration.ConfigureAveraging("", orfsAveragingEnabled, averagingCount, orfsAveragingType)
		gsm.Pvt.Configuration.ConfigureAveraging("", pvtAveragingEnabled, averagingCount, pvtAveragingType)
		gsm.Orfs.Configuration.ConfigureMeasurementType("", measurementType)
		gsm.Orfs.Configuration.ConfigureOffsetFrequencyMode("", RFmxGsmMXOrfsOffsetFrequencyMode.Standard)
		gsm.ConfigureAutoTscDetectionEnabled("", autoTscDetectionEnabled)
        gsm.ConfigureSignalType("slot::all", modulationType, burstType, hbFilterWidth)
        gsm.ConfigureTsc("slot::all", tsc)
        gsm.ConfigurePowerControlLevel("slot::all", powerControlLevel)
		gsm.Initiate("", "")
	End Sub

	Private Sub RetrieveResults()
		' Fetch Results 

		gsm.ModAcc.Results.FetchEvm("", timeout, meanRmsEvm, maximumRmsEvm, meanPeakEvm, maximumPeakEvm, _
			ninetyFifthPercentileEvm, meanFrequencyErrorEvm, peakEvmSymbol)
		gsm.ModAcc.Results.FetchIQImpairments("", timeout, meanIQGainImbalance, maximumIQGainImbalance, meanIQOriginOffset, maximumIQOriginOffset)
		gsm.ModAcc.Results.FetchEvmTrace("", timeout, evm)
		gsm.ModAcc.Results.FetchDetectedTscArray("", timeout, detectedTsc)

		gsm.ModAcc.Results.FetchPfer("", timeout, meanRmsPhaseError, maximumRmsPhaseError, meanFrequencyErrorPfer, meanPeakPhaseError, _
			maximumPeakPhaseError, peakSymbol)
		gsm.ModAcc.Results.FetchPhaseErrorTrace("", timeout, meanTraceError)

		gsm.Orfs.Results.FetchModulationResultsArray("", timeout, modulationCarrierPower, modulationResultsLowerRelativePower, modulationResultsUpperRelativePower, modulationResultsLowerAbsolutePower, _
			modulationResultsUpperAbsolutePower)
		gsm.Orfs.Results.FetchSwitchingResultsArray("", timeout, switchingCarrierPower, switchingResultsLowerRelativePower, switchingResultsUpperRelativePower, switchingResultsLowerAbsolutePower, _
			switchingResultsUpperAbsolutePower)

		gsm.Orfs.Results.FetchModulationPowerTrace("", timeout, modulationPowerTraceOffsetFrequency, modulationPowerTraceAbsolutePower, modulationPowerTraceRelativePower)
		gsm.Orfs.Results.FetchSwitchingPowerTrace("", timeout, switchingPowerTraceOffsetFrequency, switchingPowerTraceAbsolutePower, switchingPowerTraceRelativePower)

		gsm.Pvt.Results.FetchSlotMeasurementArray("", timeout, slotAveragePower, slotBurstWidth, slotMeasurementStatus, slotMaximumPower, _
			slotMinimumPower, slotBurstThreshold)
		gsm.Pvt.Results.FetchMeasurementStatus("", timeout, measurementStatus)

		gsm.Pvt.Results.FetchPowerTrace("", timeout, upperMask, signalPower, lowerMask)
	End Sub

	Private Sub PrintResults()
        Console.WriteLine("Measurement Status              {0}", measurementStatus)

		Console.WriteLine("-----------------ModAcc Measurements---------------" & vbLf & vbLf)
		Console.WriteLine("-----------------EVM Measurement-----------------" & vbLf)
		Console.WriteLine("Mean RMS EVM (%)                {0}", meanRmsEvm)
		Console.WriteLine("Maximum RMS EVM (%)             {0}", maximumRmsEvm)
		Console.WriteLine("Mean Peak EVM (%)               {0}", meanPeakEvm)
		Console.WriteLine("Maximum Peak EVM (%)            {0}", maximumPeakEvm)
		Console.WriteLine("95th Percentile EVM (%)         {0}", ninetyFifthPercentileEvm)
		Console.WriteLine("Mean Frequency Error (Hz)       {0}", meanFrequencyErrorEvm)
		Console.WriteLine("Peak EVM Symbol                 {0}" & vbLf, peakEvmSymbol)

		Console.WriteLine("-----------------PFER Measurement-----------------" & vbLf)
		Console.WriteLine("Mean RMS Phase Error (deg)      {0}", meanRmsPhaseError)
		Console.WriteLine("Maximum RMS Phase Error (deg)   {0}", maximumRmsPhaseError)
		Console.WriteLine("Mean Peak Phase Error  (deg)    {0}", meanPeakPhaseError)
		Console.WriteLine("Maximum Peak Phase Error (deg)  {0}", maximumPeakPhaseError)
		Console.WriteLine("Mean Frequency Error (Hz)       {0}", meanFrequencyErrorPfer)
		Console.WriteLine("Peak Symbol                     {0}" & vbLf, peakSymbol)

		Console.WriteLine("----------------IQ Impairments-----------------" & vbLf)
		Console.WriteLine("Mean IQ Gain Imbalance (dB)      {0}", meanIQGainImbalance)
		Console.WriteLine("Maximum IQ Gain Imbalance (dB)   {0}", maximumIQGainImbalance)
		Console.WriteLine("Maximum IQ Origin Offset (dB)    {0}", maximumIQOriginOffset)
		Console.WriteLine("Mean IQ Origin Offset (dB)       {0}" & vbLf, meanIQOriginOffset)

		Console.WriteLine("----------------Detected TSC------------------")
		For i As Integer = 0 To detectedTsc.Length - 1
			Console.WriteLine("Slot {0}                : {1}", i, detectedTsc(i))
		Next
		
		Console.WriteLine("-----------------ORFS Measurements-------------" & vbLf)
		Console.WriteLine("----------------Modulation Results--------------" & vbLf)
		Console.WriteLine("Modulation Carrier Power (dBm)   {0}", modulationCarrierPower)
		For i As Integer = 0 To modulationResultsLowerAbsolutePower.Length - 1
			Console.WriteLine("Offset : {0}", i)
            Console.WriteLine("Lower Absolute Power (dBm)      {0}", modulationResultsLowerAbsolutePower(i))
            Console.WriteLine("Lower Relative Power (dB)       {0}", modulationResultsLowerRelativePower(i))
            Console.WriteLine("Upper Absolute Power (dBm)      {0}", modulationResultsUpperAbsolutePower(i))
            Console.WriteLine("Upper Relative Power (dB)       {0}" & vbLf, modulationResultsUpperRelativePower(i))
		Next
		Console.WriteLine("----------------Switching Results--------------" & vbLf)
        Console.WriteLine("Switching Carrier Power (dBm)    {0}", switchingCarrierPower)
		For i As Integer = 0 To switchingResultsLowerAbsolutePower.Length - 1
			Console.WriteLine("Offset : {0}", i)
            Console.WriteLine("Lower Absolute Power (dBm)      {0}", switchingResultsLowerAbsolutePower(i))
            Console.WriteLine("Lower Relative Power (dB)       {0}", switchingResultsLowerRelativePower(i))
            Console.WriteLine("Upper Absolute Power (dBm)      {0}", switchingResultsUpperAbsolutePower(i))
            Console.WriteLine("Upper Relative Power (dB)       {0}" & vbLf, switchingResultsUpperRelativePower(i))
		Next

		Console.WriteLine("-----------------PVT Measurements--------------" & vbLf)
		Console.WriteLine("Slot Measurement Status : {0}", measurementStatus)
		For i As Integer = 0 To numberOfTimeslots - 1
            Console.WriteLine(vbLf & "Slot Measurement :      {0}" & vbLf, i)
            Console.WriteLine("Average Power (dBm)            {0}", slotAveragePower(i))
            Console.WriteLine("Burst Width (s)                {0}", slotBurstWidth(i))
            Console.WriteLine("Maximum Power (dBm)            {0}", slotMaximumPower(i))
            Console.WriteLine("Minimum Power (dBm)            {0}", slotMinimumPower(i))
            Console.WriteLine("Burst Threshold (dBm)          {0}", slotBurstThreshold(i))
            Console.WriteLine("Measurement Status             {0}", slotMeasurementStatus(i))
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
