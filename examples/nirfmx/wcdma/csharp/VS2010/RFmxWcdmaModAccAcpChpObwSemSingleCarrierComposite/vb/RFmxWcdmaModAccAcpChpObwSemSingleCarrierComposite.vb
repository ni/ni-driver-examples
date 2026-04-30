'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
'4. Configure Trigger Type and Trigger Parameters.
'5. Configure UARFCN Band.
'6. Configure Uplink Scrambling.
'7. Select ACP,CHP,ModAcc,OBW and SEM measurements and enable Traces.
'8. Configure Synchronization Mode and Measurement Interval.
'9. Configure Sweep Time Parameters for ACP. 
'10. Configure Averaging Parameters for ACP.
'11. Configure Sweep Time Parameters for CHP. 
'12. Configure Averaging Parameters for CHP.
'13. Configure Sweep Time Parameters for OBW. 
'14. Configure Averaging Parameters for OBW.
'15. Configure Sweep Time Parameters for SEM. 
'16. Configure Averaging Parameters for SEM.
'17. Initiate the Measurement.
'18. Fetch ACP,SEM,ModAcc,CHP & OBW Measurements.
'19. Close RFmx Session. 
Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.WcdmaMX

Public Class RFmxWcdmaModAccAcpChpObwSemSingleCarrierComposite
	Private instrSession As RFmxInstrMX
	Private wcdma As RFmxWcdmaMX

	Private resourceName As String = "RFSA"
	Private i As Integer

	Private frequencyReferenceSource As String = RFmxInstrMXConstants.OnboardClock
	Private frequencyReferenceFrequency As Double = 10000000.0
	' Hz 
	Private centerFrequency As Double = 1950000000.0
	' Hz 
	Private referenceLevel As Double = 0.0
	' dBm 
	Private externalAttenuation As Double = 0.0
	' dB 

	Private enableTrigger As Boolean = False
	Private digitalEdgeSource As String = RFmxWcdmaMXConstants.Pfi0
	Private digitalEdge As RFmxWcdmaMXDigitalEdgeTriggerEdge = RFmxWcdmaMXDigitalEdgeTriggerEdge.Rising
	Private triggerDelay As Double = 0.0

	Private numberOfCarriers As Integer = 2
	Private band As Integer = 1
	Private carrierAtCenterFrequency As Integer = -1

	Private uplinkScramblingType As RFmxWcdmaMXUplinkScramblingType = RFmxWcdmaMXUplinkScramblingType.[Long]
	Private uplinkScramblingCode As Integer = &H0

	Private synchronizationMode As RFmxWcdmaMXModAccSynchronizationMode = RFmxWcdmaMXModAccSynchronizationMode.Slot
	Private measurementOffset As Integer = 0
	' slots 
	Private measurementLength As Integer = 1
	' slots 

	Private acpSweepTimeAuto As RFmxWcdmaMXAcpSweepTimeAuto = RFmxWcdmaMXAcpSweepTimeAuto.[True]
	Private chpSweepTimeAuto As RFmxWcdmaMXChpSweepTimeAuto = RFmxWcdmaMXChpSweepTimeAuto.[True]
	Private obwSweepTimeAuto As RFmxWcdmaMXObwSweepTimeAuto = RFmxWcdmaMXObwSweepTimeAuto.[True]
	Private semSweepTimeAuto As RFmxWcdmaMXSemSweepTimeAuto = RFmxWcdmaMXSemSweepTimeAuto.[True]
	Private sweepTimeInterval As Double = 0.000667
	' seconds 

	Private acpAveragingEnabled As RFmxWcdmaMXAcpAveragingEnabled = RFmxWcdmaMXAcpAveragingEnabled.[False]
	Private chpAveragingEnabled As RFmxWcdmaMXChpAveragingEnabled = RFmxWcdmaMXChpAveragingEnabled.[False]
	Private obwAveragingEnabled As RFmxWcdmaMXObwAveragingEnabled = RFmxWcdmaMXObwAveragingEnabled.[False]
	Private semAveragingEnabled As RFmxWcdmaMXSemAveragingEnabled = RFmxWcdmaMXSemAveragingEnabled.[False]
	Private averagingCount As Integer = 10
	Private acpAveragingType As RFmxWcdmaMXAcpAveragingType = RFmxWcdmaMXAcpAveragingType.Rms
	Private chpAveragingType As RFmxWcdmaMXChpAveragingType = RFmxWcdmaMXChpAveragingType.Rms
	Private obwAveragingType As RFmxWcdmaMXObwAveragingType = RFmxWcdmaMXObwAveragingType.Rms
	Private semAveragingType As RFmxWcdmaMXSemAveragingType = RFmxWcdmaMXSemAveragingType.Rms

	Private acpAbsolutePower As Double
	Private acpRelativePower As Double
	Private timeout As Double = 10.0
	' seconds 
	Private chipRateError As Double
	Private frequencyError As Double
	Private rmsEvm As Double
	Private peakEvm As Double
	Private rho As Double
	Private rmsPhaseError As Double
	Private rmsMagnitudeError As Double
	Private semMeasurementStatus As RFmxWcdmaMXSemMeasurementStatus
	Private chpAbsolutePower As Double
	Private chpRelativePower As Double
	Private obwAbsolutePower As Double
	Private obwStopFrequency As Double
	Private obwStartFrequency As Double
	Private obwOccupiedBandwidth As Double
	Private semAbsoluteIntegratedPower As Double
	Private semRelativeIntegratedPower As Double


	Private acpLowerAbsolutePower As Double()
	'(dBm) 

	Private acpUpperAbsolutePower As Double()
	'(dBm) 

	Private acpLowerRelativePower As Double()
	'(dB) 

	Private acpUpperRelativePower As Double()

	Private semLowerOffsetMarginRelativePower As Double()
	'(dB) 

	Private semLowerOffsetMarginAbsolutePower As Double()
	'(dBm) 

	Private semLowerOffsetMeasurementStatus As RFmxWcdmaMXSemLowerOffsetMeasurementStatus()

	Private semLowerOffsetMargin As Double()
	'(dB) 

	Private semLowerOffsetMarginFrequency As Double()
	'(Hz) 

	Private semUpperOffsetMarginRelativePower As Double()
	'(dB) 

	Private semUpperOffsetMarginAbsolutePower As Double()
	'(dBm) 

	Private semUpperOffsetMeasurementStatus As RFmxWcdmaMXSemUpperOffsetMeasurementStatus()

	Private semUpperOffsetMargin As Double()
	'(dB) 

	Private semUpperOffsetMarginFrequency As Double()
	'(Hz) 


	Public Sub Run()
		Try
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

	Private Sub ConfigureWcdma()
		wcdma = instrSession.GetWcdmaSignalConfiguration()
		instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
		wcdma.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
		wcdma.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger)
		wcdma.ConfigureBand("", band)
		wcdma.ConfigureContiguousCarriers("", numberOfCarriers, carrierAtCenterFrequency)
		wcdma.ConfigureUplinkScrambling("", uplinkScramblingCode, uplinkScramblingType)
		wcdma.SelectMeasurements("", RFmxWcdmaMXMeasurementTypes.Acp Or RFmxWcdmaMXMeasurementTypes.Chp Or RFmxWcdmaMXMeasurementTypes.Obw Or RFmxWcdmaMXMeasurementTypes.Sem Or RFmxWcdmaMXMeasurementTypes.ModAcc, True)
		wcdma.ModAcc.Configuration.ConfigureSynchronizationModeAndInterval("", synchronizationMode, measurementOffset, measurementLength)
		wcdma.Acp.Configuration.ConfigureSweepTime("", acpSweepTimeAuto, sweepTimeInterval)
		wcdma.Acp.Configuration.ConfigureAveraging("", acpAveragingEnabled, averagingCount, acpAveragingType)
		wcdma.Chp.Configuration.ConfigureSweepTime("", chpSweepTimeAuto, sweepTimeInterval)
		wcdma.Chp.Configuration.ConfigureAveraging("", chpAveragingEnabled, averagingCount, chpAveragingType)
		wcdma.Obw.Configuration.ConfigureSweepTime("", obwSweepTimeAuto, sweepTimeInterval)
		wcdma.Obw.Configuration.ConfigureAveraging("", obwAveragingEnabled, averagingCount, obwAveragingType)
		wcdma.Sem.Configuration.ConfigureSweepTime("", semSweepTimeAuto, sweepTimeInterval)
		wcdma.Sem.Configuration.ConfigureAveraging("", semAveragingEnabled, averagingCount, semAveragingType)
		wcdma.Initiate("", "")
	End Sub

	Private Sub RetrieveResults()
		wcdma.Acp.Results.FetchOffsetMeasurementArray("", timeout, acpLowerRelativePower, acpUpperRelativePower, acpLowerAbsolutePower, acpUpperAbsolutePower)


		wcdma.Sem.Results.FetchLowerOffsetMarginArray("", timeout, semLowerOffsetMeasurementStatus, semLowerOffsetMargin, semLowerOffsetMarginFrequency, semLowerOffsetMarginAbsolutePower, _
			semLowerOffsetMarginRelativePower)

		wcdma.Sem.Results.FetchUpperOffsetMarginArray("", timeout, semUpperOffsetMeasurementStatus, semUpperOffsetMargin, semUpperOffsetMarginFrequency, semUpperOffsetMarginAbsolutePower, _
			semUpperOffsetMarginRelativePower)


		wcdma.ModAcc.Results.FetchEvm("", timeout, rmsEvm, peakEvm, rho, frequencyError, _
			chipRateError, rmsMagnitudeError, rmsPhaseError)
		wcdma.Sem.Results.FetchMeasurementStatus("", timeout, semMeasurementStatus)
		wcdma.Acp.Results.FetchCarrierMeasurement("", timeout, acpAbsolutePower, acpRelativePower)
		wcdma.Chp.Results.FetchCarrierMeasurement("", timeout, chpAbsolutePower, chpRelativePower)
		wcdma.Obw.Results.FetchMeasurement("", timeout, obwOccupiedBandwidth, obwAbsolutePower, obwStartFrequency, obwStopFrequency)
		wcdma.Sem.Results.FetchCarrierMeasurement("", timeout, semAbsoluteIntegratedPower, semRelativeIntegratedPower)

	End Sub

	Private Sub PrintResults()
		Console.WriteLine("************************* ModAcc *************************" & vbLf)
		Console.WriteLine("RMS EVM (%)                    : {0}", rmsEvm)
		Console.WriteLine("Peak EVM (%)                   : {0}", peakEvm)
		Console.WriteLine("Rho                            : {0}", rho)
		Console.WriteLine("Frequency Error (Hz)           : {0}", frequencyError)
		Console.WriteLine("Chip Rate Error (ppm)          : {0}", chipRateError)
		Console.WriteLine("RMS Magnitude Error (%)        : {0}", rmsMagnitudeError)
		Console.WriteLine("RMS Phase Error (deg)          : {0}", rmsPhaseError)

		Console.WriteLine(vbLf & "************************* ACP *************************" & vbLf)
		Console.WriteLine("Carrier Absolute Power (dBm)    : {0}", acpAbsolutePower)
		For i = 0 To acpLowerRelativePower.Length - 1
			Console.WriteLine(vbLf & "Offset Channel Measurements" & vbTab & ":  {0}", i)
			Console.WriteLine("Lower Relative Power (dB)" & vbTab & ":  {0}", acpLowerRelativePower(i))
			Console.WriteLine("Upper Relative Power (dB)" & vbTab & ":  {0}", acpUpperRelativePower(i))
			Console.WriteLine("Lower Absolute Power (dBm)" & vbTab & ":  {0}", acpLowerAbsolutePower(i))
			Console.WriteLine("Upper Absolute Power (dBm)" & vbTab & ":  {0}", acpUpperAbsolutePower(i))
		Next

		Console.WriteLine(vbLf & "************************* CHP *************************" & vbLf)
		Console.WriteLine("Carrier Absolute Power (dBm)    :  {0}", chpAbsolutePower)

		Console.WriteLine(vbLf & "************************* OBW *************************" & vbLf)
		Console.WriteLine("Occupied Bandwidth (Hz)" & vbTab & vbTab & ":  {0}", obwOccupiedBandwidth)
		Console.WriteLine("Absoulte Power (dBm)" & vbTab & vbTab & ":  {0}", obwAbsolutePower)
		Console.WriteLine("Start Frequency (Hz)" & vbTab & vbTab & ":  {0}", obwStartFrequency)
		Console.WriteLine("Stop Frequency (Hz)" & vbTab & vbTab & ":  {0}", obwStopFrequency)

		Console.WriteLine(vbLf & "************************* SEM *************************" & vbLf)
		Console.WriteLine("Measurement Status" & vbTab & vbTab & ":  {0}", semMeasurementStatus)
		Console.WriteLine("Carrier Absolute Integrated Power (dBm)" & vbTab & ":  {0}", semAbsoluteIntegratedPower)

		Console.WriteLine(vbLf & "---------------Lower Offset---------------" & vbLf)
		For i = 0 To semLowerOffsetMargin.Length - 1
			Console.WriteLine(vbLf & "Offset Channel Measurements" & vbTab & ":  {0}", i)
			Console.WriteLine("Margin (dB)" & vbTab & vbTab & vbTab & ":  {0}", semLowerOffsetMargin(i))
			Console.WriteLine("Margin Absolute Power (dBm)" & vbTab & ":  {0}", semLowerOffsetMarginAbsolutePower(i))
			Console.WriteLine("Margin Relative Power (dB)" & vbTab & ":  {0}", semLowerOffsetMarginRelativePower(i))
			Console.WriteLine("Margin Frequency (Hz)" & vbTab & vbTab & ":  {0}", semLowerOffsetMarginFrequency(i))
			Console.WriteLine("Measurement Status" & vbTab & vbTab & ":  {0}", semLowerOffsetMeasurementStatus(i))
		Next

		Console.WriteLine(vbLf & "---------------Upper Offset---------------" & vbLf)
		For i = 0 To semUpperOffsetMargin.Length - 1
			Console.WriteLine(vbLf & "Offset Channel Measurements" & vbTab & ":  {0}", i)
			Console.WriteLine("Margin (dB)" & vbTab & vbTab & vbTab & ":  {0}", semUpperOffsetMargin(i))
			Console.WriteLine("Margin Absolute Power (dBm)" & vbTab & ":  {0}", semUpperOffsetMarginAbsolutePower(i))
			Console.WriteLine("Margin Relative Power (dB)" & vbTab & ":  {0}", semUpperOffsetMarginRelativePower(i))
			Console.WriteLine("Margin Frequency (Hz)" & vbTab & vbTab & ":  {0}", semUpperOffsetMarginFrequency(i))
			Console.WriteLine("Measurement Status" & vbTab & vbTab & ":  {0}", semUpperOffsetMeasurementStatus(i))
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
