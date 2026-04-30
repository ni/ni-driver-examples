'Steps:
'1. Open a new RFmx session.
'2. Configure Frequency Reference.
'3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
'4. Configure Trigger Type and Trigger Parameters.
'5. Configure Uplink Scrambling.
'6. Select CDA measurement and enable traces.
'7. Configure Synchronization Mode and Interval.
'8. Configure Measurement Channel.
'9. Configure Power Unit.
'10. Initiate the Measurement.
'11. Fetch CDA Measurements and Traces.
'12. Close the RFmx session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.WcdmaMX

Public Class RFmxWcdmaCda
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

	Private digitalEdgeTriggerSource As String
	Private digitalEdgeTriggerEdge As RFmxWcdmaMXDigitalEdgeTriggerEdge
	Private triggerDelay As Double
	' seconds 
	Private referenceLevel As Double
	' dBm 

	Private synchronizationMode As RFmxWcdmaMXCdaSynchronizationMode
	Private measurementOffset As Integer
	Private measurementLength As Integer

	Private spreadingFactor As Integer
	Private spreadingCode As Integer
	' s 
	Private modulationType As RFmxWcdmaMXCdaMeasurementChannelModulationType
	Private branch As RFmxWcdmaMXCdaMeasurementChannelBranch

	Private powerUnit As RFmxWcdmaMXCdaPowerUnit

	Private timeout As Double
	' seconds 

	Private uplinkScramblingType As RFmxWcdmaMXUplinkScramblingType
	Private uplinkScramblingCode As Integer

	Private enableAllTraces As Boolean
	Private enableTrigger As Boolean

	Private totalPower As Double, totalActivePower As Double, meanActivePower As Double, peakActivePower As Double, meanInactivePower As Double, peakInactivePower As Double
	Private iMeanActivePower As Double, qMeanActivePower As Double, iPeakInactivePower As Double, qPeakInactivePower As Double
	Private rmsSymbolEVM As Double, peakSymbolEvm As Double, rmsSymbolMagnitudeError As Double, rmsSymbolPhaseError As Double, meanSymbolPower As Double, chipRateError As Double

	Private iCodeDomainPowers As Single() = Nothing
	Private qCodeDomainPowers As Single() = Nothing
	Private symbolEVM As Single() = Nothing




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

	Private Sub InitializeInstr()
		' Create a new RFmx Session 

		instrSession = New RFmxInstrMX(resourceName, "")
	End Sub

	Private Sub InitializeVariables()
		' Initialize input variables 


		resourceName = "RFSA"
		centerFrequency = 1950000000.0
		' Hz 
		referenceLevel = 0.0
		' dBm 
		externalAttenuation = 0.0
		' dB 

		measurement = RFmxWcdmaMXMeasurementTypes.Cda

		frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
		frequencyReferenceFrequency = 10000000.0
		' Hz 
		digitalEdgeTriggerSource = RFmxWcdmaMXConstants.Pfi0
		digitalEdgeTriggerEdge = RFmxWcdmaMXDigitalEdgeTriggerEdge.Rising
		triggerDelay = 0.0
		' s 
		enableTrigger = False

		uplinkScramblingType = RFmxWcdmaMXUplinkScramblingType.[Long]
		uplinkScramblingCode = 0

		synchronizationMode = RFmxWcdmaMXCdaSynchronizationMode.Slot
		measurementOffset = 0
		' slots 
		measurementLength = 1
		' slots 

		spreadingFactor = 256
		spreadingCode = 0
		modulationType = RFmxWcdmaMXCdaMeasurementChannelModulationType.ModulationTypeBpskQpsk
		branch = RFmxWcdmaMXCdaMeasurementChannelBranch.Q

		powerUnit = RFmxWcdmaMXCdaPowerUnit.dB

		timeout = 10

		enableAllTraces = True

	End Sub

	Private Sub ConfigureWcdma()
		wcdma = instrSession.GetWcdmaSignalConfiguration()
		instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
		wcdma.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
		wcdma.ConfigureDigitalEdgeTrigger("", digitalEdgeTriggerSource, digitalEdgeTriggerEdge, triggerDelay, enableTrigger)
		wcdma.ConfigureUplinkScrambling("", uplinkScramblingCode, uplinkScramblingType)
		wcdma.SelectMeasurements("", measurement, enableAllTraces)
		wcdma.Cda.Configuration.ConfigureSynchronizationModeAndInterval("", synchronizationMode, measurementOffset, measurementLength)
		wcdma.Cda.Configuration.ConfigureMeasurementChannel("", spreadingFactor, spreadingCode, modulationType, branch)
		wcdma.Cda.Configuration.ConfigurePowerUnit("", powerUnit)
		wcdma.Initiate("", "")
	End Sub

	Private Sub RetrieveResults()
		wcdma.Cda.Results.FetchSymbolEvm("", timeout, rmsSymbolEVM, peakSymbolEvm, rmsSymbolMagnitudeError, rmsSymbolPhaseError, _
			meanSymbolPower, chipRateError)
		wcdma.Cda.Results.FetchCodeDomainPower("", timeout, totalPower, totalActivePower, meanActivePower, peakActivePower, _
			meanInactivePower, peakInactivePower)
		wcdma.Cda.Results.FetchCodeDomainIAndQPower("", timeout, iMeanActivePower, qMeanActivePower, iPeakInactivePower, qPeakInactivePower)
        wcdma.Cda.Results.FetchCodeDomainIAndQPowerTrace("", timeout, iCodeDomainPowers, qCodeDomainPowers)
		wcdma.Cda.Results.FetchSymbolEvmTrace("", timeout, symbolEVM)
	End Sub

	Private Sub PrintResults()
		Console.WriteLine(vbLf & "---------------------- Code Domain Power -------------------" & vbLf)
        Console.WriteLine("Total Power (dBm)                              : {0}", totalPower)
        Console.WriteLine("Total Active Power (dB or dBm)                 : {0}", totalActivePower)
        Console.WriteLine("Mean Inactive Power (dB or dBm)                : {0}", meanInactivePower)
        Console.WriteLine("Peak Inactive Power (dB or dBm)                : {0}", peakInactivePower)
        Console.WriteLine("I Peak Inactive Power (dB or dBm)              : {0}", iPeakInactivePower)
        Console.WriteLine("Q Peak Inactive Power (dB or dBm)              : {0}", qPeakInactivePower)

		Console.WriteLine(vbLf & "------------------------- Symbol EVM -----------------------" & vbLf)
        Console.WriteLine("RMS Symbol EVM (%)                             : {0}", rmsSymbolEVM)
        Console.WriteLine("Peak Symbol EVM (%)                            : {0}", peakSymbolEvm)
        Console.WriteLine("RMS Symbol Magnitude Error (%)                 : {0}", rmsSymbolMagnitudeError)
        Console.WriteLine("RMS Symbol Phase Error (deg)                   : {0}", rmsSymbolPhaseError)
        Console.WriteLine("Mean Symbol Power (dB or dBm)                  : {0}", meanSymbolPower)
        Console.WriteLine("Chip Rate Error (ppm)                          : {0}", chipRateError)
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
