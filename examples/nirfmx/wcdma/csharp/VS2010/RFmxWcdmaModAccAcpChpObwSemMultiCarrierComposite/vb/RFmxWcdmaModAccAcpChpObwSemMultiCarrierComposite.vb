'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
'4. Configure Trigger Type and Trigger Parameters.
'5. Configure UARFCN Band.
'6. Configure Contiguous Carriers.
'7. Configure Uplink Scrambling (Array).
'8. Select ModAcc,ACP,CHP,OBW and SEM measurements and enable Traces.
'9. Configure Synchronization Mode and Interval for ModAcc
'10. Configure Sweep Time Parameters for ACP.
'11. Configure Averaging Parameters for ACP.
'12. Configure Sweep Time Parameters for CHP. 
'13. Configure Averaging Parameters for CHP.
'14. Configure Sweep Time Parameters for OBW. 
'15. Configure Averaging Parameters for OBW.
'16. Configure Sweep Time Parameters for SEM. 
'17. Configure Averaging Parameters for SEM.
'18. Initiate the Measurement.
'19. Fetch ModAcc, ACP, CHP, SEM & OBW Measurements.
'20. Close RFmx Session.  

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.WcdmaMX

Public Class RFmxWcdmaModAccAcpChpObwSemMultiCarrierComposite
	Private instrSession As RFmxInstrMX
	Private wcdma As RFmxWcdmaMX

	Private resourceName As String
	Private selectorString As String
	Private i As Integer

	Private frequencyReferenceSource As String
	Private frequencyReferenceFrequency As Double
	Private centerFrequency As Double
	Private referenceLevel As Double
	Private externalAttenuation As Double
	Private enableTrigger As Boolean = False
	Private digitalEdgeSource As String
	Private digitalEdge As RFmxWcdmaMXDigitalEdgeTriggerEdge
	Private triggerDelay As Double
	Private band As Integer
	Private timeout As Double

	Private carrierAtCenterFrequency As Integer
	Const numberOfCarriers As Integer = 2

	Private synchronizationMode As RFmxWcdmaMXModAccSynchronizationMode
	Private measurementOffset As Integer
	Private measurementLength As Integer
	Private uplinkScramblingType As RFmxWcdmaMXUplinkScramblingType() = New RFmxWcdmaMXUplinkScramblingType(numberOfCarriers - 1) {}
	Private uplinkScramblingCode As Integer() = New Integer(numberOfCarriers - 1) {}

	Private acpSweepTimeAuto As RFmxWcdmaMXAcpSweepTimeAuto
	Private chpSweepTimeAuto As RFmxWcdmaMXChpSweepTimeAuto
	Private obwSweepTimeAuto As RFmxWcdmaMXObwSweepTimeAuto
	Private semSweepTimeAuto As RFmxWcdmaMXSemSweepTimeAuto
	Private sweepTimeInterval As Double
	Private acpAveragingEnabled As RFmxWcdmaMXAcpAveragingEnabled
	Private chpAveragingEnabled As RFmxWcdmaMXChpAveragingEnabled
	Private obwAveragingEnabled As RFmxWcdmaMXObwAveragingEnabled
	Private semAveragingEnabled As RFmxWcdmaMXSemAveragingEnabled
	Private averagingCount As Integer
	Private acpAveragingType As RFmxWcdmaMXAcpAveragingType
	Private chpAveragingType As RFmxWcdmaMXChpAveragingType
	Private obwAveragingType As RFmxWcdmaMXObwAveragingType
	Private semAveragingType As RFmxWcdmaMXSemAveragingType


	Private chpTotalCarrierPower As Double
	Private acpTotalCarrierPower As Double
	Private obwOccupiedBandwidth As Double
	Private obwAbsolutePower As Double
	Private obwStopFrequency As Double
	Private obwStartFrequency As Double
	Private semMeasurementStatus As RFmxWcdmaMXSemMeasurementStatus
	Private semTotalCarrierPower As Double

	Private chipRateError As Double()
	Private frequencyError As Double()
	Private rmsEvm As Double()
	Private peakEvm As Double()
	Private rho As Double()
	Private rmsPhaseError As Double()
	Private rmsMagnitudeError As Double()

	Private chpAbsolutePower As Double()
	'(dBm) 
	Private chpRelativePower As Double()
	'(dB) 

	Private semAbsoluteIntegratedPower As Double()
	Private semRelativeIntegratedPower As Double()

	Private acpAbsolutePower As Double()
	Private acpRelativePower As Double()

	Private acpLowerAbsolutePower As Double()
	'(dBm) 
	Private acpUpperAbsolutePower As Double()
	'(dBm) 
	Private acpLowerRelativePower As Double()
	'(dB) 
	Private acpUpperRelativePower As Double()
	'(dB) 

	Private semLowerOffsetMeasurementStatus As RFmxWcdmaMXSemLowerOffsetMeasurementStatus()
	Private semLowerOffsetMargin As Double()
	'(dB) 
	Private semLowerOffsetMarginFrequency As Double()
	'(Hz) 
	Private semLowerOffsetMarginAbsolutePower As Double()
	'(dBm) 
	Private semLowerOffsetMarginRelativePower As Double()
	'(dB) 

	Private semUpperOffsetMeasurementStatus As RFmxWcdmaMXSemUpperOffsetMeasurementStatus()
	Private semUpperOffsetMargin As Double()
	'(dB) 
	Private semUpperOffsetMarginFrequency As Double()
	'(Hz) 
	Private semUpperOffsetMarginAbsolutePower As Double()
	'(dBm) 
	Private semUpperOffsetMarginRelativePower As Double()
	'(dB) 

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
			CloseSession()
			Console.WriteLine("Press any key to exit")
			Console.ReadKey()
		End Try
	End Sub

	Private Sub InitializeInstr()
		' Create a new RFmx Session 

		instrSession = New RFmxInstrMX(resourceName, "")
	End Sub

	Private Sub InitializeVariables()
		resourceName = "RFSA"
		selectorString = ""
		i = 0

		frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
		frequencyReferenceFrequency = 10000000.0
		' Hz 
		centerFrequency = 1950000000.0
		' Hz 
		referenceLevel = 0.0
		' dBm 
		externalAttenuation = 0.0
		' dB 
		enableTrigger = False
		digitalEdgeSource = RFmxWcdmaMXConstants.Pfi0
		digitalEdge = RFmxWcdmaMXDigitalEdgeTriggerEdge.Rising
		triggerDelay = 0.0
		' seconds 
		band = 1
		timeout = 10.0
		' seconds 

		carrierAtCenterFrequency = -1

		synchronizationMode = RFmxWcdmaMXModAccSynchronizationMode.Slot
		measurementOffset = 0
		' slots 
		measurementLength = 1
		' slots 

		For i = 0 To numberOfCarriers - 1

			uplinkScramblingType(i) = RFmxWcdmaMXUplinkScramblingType.[Long]
			uplinkScramblingCode(i) = 0
		Next

		acpSweepTimeAuto = RFmxWcdmaMXAcpSweepTimeAuto.[True]
		chpSweepTimeAuto = RFmxWcdmaMXChpSweepTimeAuto.[True]
		obwSweepTimeAuto = RFmxWcdmaMXObwSweepTimeAuto.[True]
		semSweepTimeAuto = RFmxWcdmaMXSemSweepTimeAuto.[True]
		sweepTimeInterval = 0.000667
		acpAveragingEnabled = RFmxWcdmaMXAcpAveragingEnabled.[False]
		chpAveragingEnabled = RFmxWcdmaMXChpAveragingEnabled.[False]
		obwAveragingEnabled = RFmxWcdmaMXObwAveragingEnabled.[False]
		semAveragingEnabled = RFmxWcdmaMXSemAveragingEnabled.[False]
		averagingCount = 10
		acpAveragingType = RFmxWcdmaMXAcpAveragingType.Rms
		chpAveragingType = RFmxWcdmaMXChpAveragingType.Rms
		obwAveragingType = RFmxWcdmaMXObwAveragingType.Rms
		semAveragingType = RFmxWcdmaMXSemAveragingType.Rms
	End Sub

	Private Sub ConfigureWcdma()
		wcdma = instrSession.GetWcdmaSignalConfiguration()

		instrSession.ConfigureFrequencyReference(selectorString, frequencyReferenceSource, frequencyReferenceFrequency)

		wcdma.ConfigureRF(selectorString, centerFrequency, referenceLevel, externalAttenuation)
		wcdma.ConfigureDigitalEdgeTrigger(selectorString, digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger)
		wcdma.ConfigureBand(selectorString, band)
		wcdma.ConfigureContiguousCarriers(selectorString, numberOfCarriers, carrierAtCenterFrequency)
		wcdma.ConfigureUplinkScramblingArray("", uplinkScramblingType, uplinkScramblingCode)
		wcdma.SelectMeasurements(selectorString, RFmxWcdmaMXMeasurementTypes.ModAcc Or RFmxWcdmaMXMeasurementTypes.Acp Or RFmxWcdmaMXMeasurementTypes.Chp Or RFmxWcdmaMXMeasurementTypes.Obw Or RFmxWcdmaMXMeasurementTypes.Sem, True)
		wcdma.ModAcc.Configuration.ConfigureSynchronizationModeAndInterval("", synchronizationMode, measurementOffset, measurementLength)
		wcdma.Acp.Configuration.ConfigureSweepTime(selectorString, acpSweepTimeAuto, sweepTimeInterval)
		wcdma.Acp.Configuration.ConfigureAveraging(selectorString, acpAveragingEnabled, averagingCount, acpAveragingType)
		wcdma.Chp.Configuration.ConfigureSweepTime(selectorString, chpSweepTimeAuto, sweepTimeInterval)
		wcdma.Chp.Configuration.ConfigureAveraging(selectorString, chpAveragingEnabled, averagingCount, chpAveragingType)
		wcdma.Obw.Configuration.ConfigureSweepTime(selectorString, obwSweepTimeAuto, sweepTimeInterval)
		wcdma.Obw.Configuration.ConfigureAveraging(selectorString, obwAveragingEnabled, averagingCount, obwAveragingType)
		wcdma.Sem.Configuration.ConfigureSweepTime(selectorString, semSweepTimeAuto, sweepTimeInterval)
		wcdma.Sem.Configuration.ConfigureAveraging(selectorString, semAveragingEnabled, averagingCount, semAveragingType)
		wcdma.Initiate(selectorString, "")
	End Sub

	Private Sub RetrieveResults()
		'ModAcc
		wcdma.ModAcc.Results.FetchEvmArray("", timeout, rmsEvm, peakEvm, rho, frequencyError, _
			chipRateError, rmsMagnitudeError, rmsPhaseError)

		'ACP
		wcdma.Acp.Results.FetchOffsetMeasurementArray(selectorString, timeout, acpLowerRelativePower, acpUpperRelativePower, acpLowerAbsolutePower, acpUpperAbsolutePower)
		wcdma.Acp.Results.FetchCarrierMeasurementArray(selectorString, timeout, acpAbsolutePower, acpRelativePower)
		wcdma.Acp.Results.FetchTotalCarrierPower(selectorString, timeout, acpTotalCarrierPower)

		'CHP
		wcdma.Chp.Results.FetchCarrierMeasurementArray(selectorString, timeout, chpAbsolutePower, chpRelativePower)
		wcdma.Chp.Results.FetchTotalCarrierPower("", timeout, chpTotalCarrierPower)

		'SEM
		wcdma.Sem.Results.FetchLowerOffsetMarginArray(selectorString, timeout, semLowerOffsetMeasurementStatus, semLowerOffsetMargin, semLowerOffsetMarginFrequency, semLowerOffsetMarginAbsolutePower, _
			semLowerOffsetMarginRelativePower)
		wcdma.Sem.Results.FetchUpperOffsetMarginArray(selectorString, timeout, semUpperOffsetMeasurementStatus, semUpperOffsetMargin, semUpperOffsetMarginFrequency, semUpperOffsetMarginAbsolutePower, _
			semUpperOffsetMarginRelativePower)

		wcdma.Sem.Results.FetchCarrierMeasurementArray(selectorString, timeout, semAbsoluteIntegratedPower, semRelativeIntegratedPower)

		wcdma.Sem.Results.FetchMeasurementStatus("", timeout, semMeasurementStatus)
		wcdma.Sem.Results.FetchTotalCarrierPower("", timeout, semTotalCarrierPower)

		'OBW
		wcdma.Obw.Results.FetchMeasurement(selectorString, timeout, obwOccupiedBandwidth, obwAbsolutePower, obwStartFrequency, obwStopFrequency)

	End Sub

	Private Sub PrintResults()

		Console.WriteLine("************************* ModAcc *************************" & vbLf)

		For i = 0 To rmsEvm.Length - 1
			Console.WriteLine(vbLf & "Measurement {0}", i)
			Console.WriteLine("RMS EVM (%)                      : {0}", rmsEvm(i))
			Console.WriteLine("Peak EVM (%)                     : {0}", peakEvm(i))
			Console.WriteLine("Rho                              : {0}", rho(i))
			Console.WriteLine("Frequency Error (Hz)             : {0}", frequencyError(i))
			Console.WriteLine("Chip Rate Error (ppm)            : {0}", chipRateError(i))
			Console.WriteLine("RMS Magnitude Error (%)          : {0}", rmsMagnitudeError(i))
			Console.WriteLine("RMS Phase Error (deg)            : {0}", rmsPhaseError(i))
		Next

		Console.WriteLine(vbLf & "************************* ACP *************************" & vbLf & vbLf)
		Console.WriteLine("Total Carrier Power  (dBm)       : {0}", acpTotalCarrierPower)
		Console.WriteLine("Carrier Measurements" & vbTab & "         : " & vbLf)
		For i = 0 To acpAbsolutePower.Length - 1
			Console.WriteLine("Carrier {0}", i)
			Console.WriteLine("Absolute Power  (dBm)            : {0}", acpAbsolutePower(i))
			Console.WriteLine("Relative Power  (dB)             : {0}", acpRelativePower(i))
		Next
		Console.WriteLine(vbLf & "Offset Channel Measurements      : " & vbLf)
		For i = 0 To acpLowerRelativePower.Length - 1
			Console.WriteLine(vbLf & "Offset {0}", i)
			Console.WriteLine("Lower Relative Power (dB)        : {0}", acpLowerRelativePower(i))
			Console.WriteLine("Upper Relative Power (dB)        : {0}", acpUpperRelativePower(i))
			Console.WriteLine("Lower Absolute Power (dBm)       : {0}", acpLowerAbsolutePower(i))
			Console.WriteLine("Upper Absolute Power (dBm)       : {0}", acpUpperAbsolutePower(i))
		Next

		Console.WriteLine(vbLf & "************************* CHP *************************" & vbLf & vbLf)
		Console.WriteLine("Total Carrier Power  (dBm)       : {0}", chpTotalCarrierPower)
		Console.WriteLine("Carrier Measurements             : " & vbLf)
		For i = 0 To chpAbsolutePower.Length - 1
			Console.WriteLine("Carrier {0}", i)
			Console.WriteLine("Absolute Power  (dBm)            : {0}", chpAbsolutePower(i))
			Console.WriteLine("Relative Power  (dB)             : {0}", chpRelativePower(i))
		Next

		Console.WriteLine(vbLf & "************************* OBW *************************" & vbLf & vbLf)
		Console.WriteLine("Occupied Bandwidth  (Hz)         : {0}", obwOccupiedBandwidth)
		Console.WriteLine("Absolute Power  (dBm)            : {0}", obwAbsolutePower)
		Console.WriteLine("Start Frequency  (Hz)            : {0}", obwStartFrequency)
		Console.WriteLine("Stop Frequency  (Hz)             : {0}", obwStopFrequency)

		Console.WriteLine(vbLf & "************************* SEM *************************" & vbLf & vbLf)
		Console.WriteLine("Measurement Status               : {0}", semMeasurementStatus)
		Console.WriteLine("Total Carrier Power  (dBm)       : {0}", semTotalCarrierPower)
		Console.WriteLine(vbLf & "Carrier Measurements" & vbTab & "         : " & vbLf)
		For i = 0 To semAbsoluteIntegratedPower.Length - 1
			Console.WriteLine(vbLf & "Carrier {0}", i)
			Console.WriteLine("Absolute Integrated Power  (dBm) : {0}", semAbsoluteIntegratedPower(i))
			Console.WriteLine("Relative Integrated Power  (dB)  : {0}", semRelativeIntegratedPower(i))
		Next
		Console.WriteLine(vbLf & "Lower Offset Segment Measurements: " & vbLf)

		For i = 0 To semLowerOffsetMargin.Length - 1
			Console.WriteLine(vbLf & "Offset {0}", i)
			Console.WriteLine("Margin  (dB)                     : {0}", semLowerOffsetMargin(i))
			Console.WriteLine("Margin Absolute Power  (dBm)     : {0}", semLowerOffsetMarginAbsolutePower(i))
			Console.WriteLine("Margin Frequency  (Hz)           : {0}", semLowerOffsetMarginFrequency(i))
			Console.WriteLine("Measurement Status               : {0}", semLowerOffsetMeasurementStatus(i))
		Next
		Console.WriteLine(vbLf & "Upper Offset Segment Measurements: " & vbLf)

		For i = 0 To semUpperOffsetMeasurementStatus.Length - 1
			Console.WriteLine(vbLf & "Offset {0}", i)
			Console.WriteLine("Margin  (dB)                     : {0}", semUpperOffsetMargin(i))
			Console.WriteLine("Margin Absolute Power  (dBm)     : {0}", semUpperOffsetMarginAbsolutePower(i))
			Console.WriteLine("Margin Frequency  (Hz)           : {0}", semUpperOffsetMarginFrequency(i))
			Console.WriteLine("Measurement Status               : {0}", semUpperOffsetMeasurementStatus(i))
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
