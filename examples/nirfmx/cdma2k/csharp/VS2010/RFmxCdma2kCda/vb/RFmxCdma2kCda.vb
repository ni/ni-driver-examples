'Steps:
'1. Open a new RFmx session.
'2. Configure Frequency Reference.
'3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
'4. Configure Trigger Type and Trigger Parameters.
'5. Configure Radio Configuration 
'6. Configure Uplink Scrambling.
'7. Select CDA measurement and enable traces.
'8. Configure Synchronization Mode and Interval.
'9. Configure Measurement Channel.
'10. Configure Power Unit.
'11. Initiate the Measurement.
'12. Fetch CDA Measurements and Traces.
'13. Close the RFmx session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.Cdma2kMX

Public Class RFmxCdma2kCda
	Private instrSession As RFmxInstrMX
	Private cdma2k As RFmxCdma2kMX

	Private resourceName As String
	Private measurement As RFmxCdma2kMXMeasurementTypes
	Private frequencyReferenceSource As String
	Private frequencyReferenceFrequency As Double
	' Hz 
	Private centerFrequency As Double
	' Hz 
	Private externalAttenuation As Double
	' dB 

	Private digitalEdgeTriggerSource As String
	Private digitalEdgeTriggerEdge As RFmxCdma2kMXDigitalEdgeTriggerEdge
	Private triggerDelay As Double
	' seconds 
	Private referenceLevel As Double
	' dBm 


	Private radioConfiguration As RFmxCdma2kMXRadioConfiguration
	Private synchronizationMode As RFmxCdma2kMXCdaSynchronizationMode
	Private uplinkSpreadingLongCodeMask As Integer
	Private measurementOffset As Integer
	Private measurementLength As Integer

	Private walshCodeNumber As Integer
	' s 
	Private walshCodeLength As Integer
	Private branch As RFmxCdma2kMXCdaMeasurementChannelBranch

    Private powerUnit As RFmxCdma2kMXCdaPowerUnit

	Private timeout As Double
	' seconds 


	Private enableAllTraces As Boolean
	Private enableTrigger As Boolean

	Private totalPower As Double, totalActivePower As Double, meanActivePower As Double, peakActivePower As Double, meanInactivePower As Double, peakInactivePower As Double
	Private iMeanActivePower As Double, qMeanActivePower As Double, iPeakInactivePower As Double, qPeakInactivePower As Double, iqOriginOffset As Double, iqGainImbalance As Double, _
		iqQuadratureError As Double
	Private rmsSymbolEvm As Double, peakSymbolEvm As Double, rmsSymbolMagnitudeError As Double, rmsSymbolPhaseError As Double, meanSymbolPower As Double, frequencyError As Double, _
		chipRateError As Double

	Private iCodeDomainPowers As Single()
	Private qCodeDomainPowers As Single()
	Private symbolEvm As Single()
	Private symbolConstellation As ComplexSingle()




	Public Sub Run()
		Try
			InitializeVariables()
			InitializeInstr()
			ConfigureCdma2k()
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
		centerFrequency = 833490000.0
		' Hz 
		referenceLevel = 0.0
		' dBm 
		externalAttenuation = 0.0
		' dB 

		measurement = RFmxCdma2kMXMeasurementTypes.Cda

		frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
		frequencyReferenceFrequency = 10000000.0
		' Hz 

		digitalEdgeTriggerSource = RFmxCdma2kMXConstants.Pfi0
		digitalEdgeTriggerEdge = RFmxCdma2kMXDigitalEdgeTriggerEdge.Rising
		triggerDelay = 0.0
		' seconds 
		enableTrigger = False

		radioConfiguration = RFmxCdma2kMXRadioConfiguration.RC3
		uplinkSpreadingLongCodeMask = 0
		synchronizationMode = RFmxCdma2kMXCdaSynchronizationMode.Slot
		measurementOffset = 0
		' slots 
		measurementLength = 1
		' slots 

		walshCodeNumber = 0
		walshCodeLength = 64
		branch = RFmxCdma2kMXCdaMeasurementChannelBranch.I

        powerUnit = RFmxCdma2kMXCdaPowerUnit.dB

		timeout = 10
		' seconds 

		enableAllTraces = True

	End Sub

	Private Sub ConfigureCdma2k()
		cdma2k = instrSession.GetCdma2kSignalConfiguration()
		instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
		cdma2k.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
		cdma2k.ConfigureDigitalEdgeTrigger("", digitalEdgeTriggerSource, digitalEdgeTriggerEdge, triggerDelay, enableTrigger)
		cdma2k.ConfigureRadioConfiguration("", radioConfiguration)
		cdma2k.ConfigureUplinkSpreading("", uplinkSpreadingLongCodeMask)
		cdma2k.SelectMeasurements("", measurement, enableAllTraces)
		cdma2k.Cda.Configuration.ConfigureSynchronizationModeAndInterval("", synchronizationMode, measurementOffset, measurementLength)
		cdma2k.Cda.Configuration.ConfigureMeasurementChannel("", walshCodeLength, walshCodeNumber, branch)
		cdma2k.Cda.Configuration.ConfigurePowerUnit("", powerUnit)
		cdma2k.Initiate("", "")
	End Sub

	Private Sub RetrieveResults()
		cdma2k.Cda.Results.FetchCodeDomainPower("", timeout, totalPower, totalActivePower, meanActivePower, peakActivePower, _
			meanInactivePower, peakInactivePower)
		cdma2k.Cda.Results.FetchCodeDomainIAndQPower("", timeout, iMeanActivePower, qMeanActivePower, iPeakInactivePower, qPeakInactivePower)
		cdma2k.Cda.Results.FetchSymbolEvm("", timeout, rmsSymbolEvm, peakSymbolEvm, rmsSymbolMagnitudeError, rmsSymbolPhaseError, _
			meanSymbolPower, frequencyError, chipRateError)
		cdma2k.Cda.Results.FetchIQImpairments("", timeout, iqOriginOffset, iqGainImbalance, iqQuadratureError)
		cdma2k.Cda.Results.FetchCodeDomainIAndQPowerTrace("", timeout, iCodeDomainPowers, qCodeDomainPowers)
		cdma2k.Cda.Results.FetchSymbolEvmTrace("", timeout, symbolEvm)
		cdma2k.Cda.Results.FetchSymbolConstellationTrace("", timeout, symbolConstellation)
	End Sub

	Private Sub PrintResults()
		Console.WriteLine(vbLf & "---------------------- Code Domain Power -------------------" & vbLf)
		Console.WriteLine("Total Power (dBm)                              : {0}" & vbLf, totalPower)
		Console.WriteLine("Total Active Power (dB or dBm)                 : {0}" & vbLf, totalActivePower)
		Console.WriteLine("Mean Inactive Power (dB or dBm)                : {0}" & vbLf, meanInactivePower)
		Console.WriteLine("Peak Inactive Power (dB or dBm)                : {0}" & vbLf, peakInactivePower)
		Console.WriteLine("I Peak Inactive Power (dB or dBm)              : {0}" & vbLf, iPeakInactivePower)
		Console.WriteLine("Q Peak Inactive Power (dB or dBm)              : {0}" & vbLf, qPeakInactivePower)

		Console.WriteLine(vbLf & "---------------------- IQ Impairments ----------------------" & vbLf)
		Console.WriteLine("I/Q Origin Offset (dB)                         : {0}" & vbLf, iqOriginOffset)
		Console.WriteLine("I/Q Gain Imbalance (dB)                        : {0}" & vbLf, iqGainImbalance)
		Console.WriteLine("I/Q Quadrature Error (deg)                     : {0}" & vbLf, iqQuadratureError)

		Console.WriteLine(vbLf & "------------------------- Symbol EVM -----------------------" & vbLf)
		Console.WriteLine("RMS Symbol EVM (%)                             : {0}" & vbLf, rmsSymbolEvm)
		Console.WriteLine("Peak Symbol EVM (%)                            : {0}" & vbLf, peakSymbolEvm)
		Console.WriteLine("Frequency Error (Hz)                           : {0}" & vbLf, frequencyError)
		Console.WriteLine("RMS Symbol Magnitude Error (%)                 : {0}" & vbLf, rmsSymbolMagnitudeError)
		Console.WriteLine("RMS Symbol Phase Error (deg)                   : {0}" & vbLf, rmsSymbolPhaseError)
		Console.WriteLine("Mean Symbol Power (dB or dBm)                  : {0}" & vbLf, meanSymbolPower)
		Console.WriteLine("Chip Rate Error (ppm)                          : {0}" & vbLf, chipRateError)
	End Sub

	Private Sub CloseSession()
		If cdma2k IsNot Nothing Then
			cdma2k.Dispose()
			cdma2k = Nothing
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
