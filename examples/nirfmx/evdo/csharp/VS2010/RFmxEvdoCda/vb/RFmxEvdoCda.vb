'Steps:
'1. Open a new RFmx session.
'2. Configure Frequency Reference.
'3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
'4. Configure Trigger Type and Trigger Parameters.
'5. Configure Channel Configuration Mode.
'6. Configure Physical Layer Subtype.
'7. Configure Uplink Data Modulation Type.
'8. Configure Uplink Spreading Parameters. 
'9. Select CDA measurement and enable traces.
'10. Configure Synchronization Mode and Interval.
'11. Configure Measurement Channel.
'12. Configure Power Unit.
'13. Initiate the Measurement.
'14. Fetch CDA Measurements and Traces.
'15. Close the RFmx session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.EvdoMX

Public Class RFmxEvdoCda
	Private instrSession As RFmxInstrMX
	Private evdo As RFmxEvdoMX

	Private resourceName As String
	Private measurement As RFmxEvdoMXMeasurementTypes
	Private frequencyReferenceSource As String
	Private frequencyReferenceFrequency As Double
	Private centerFrequency As Double
	Private externalAttenuation As Double

	Private digitalEdgeTriggerSource As String
	Private digitalEdgeTriggerEdge As RFmxEvdoMXDigitalEdgeTriggerEdge
	Private triggerDelay As Double
	Private referenceLevel As Double

	Private uplinkSpreadingIMask As Long
	Private uplinkSpreadingQMask As Long
	Private synchronizationMode As RFmxEvdoMXCdaSynchronizationMode
	Private channelConfigurationMode As RFmxEvdoMXChannelConfigurationMode
	Private measurementOffset As Integer
	Private measurementLength As Integer

	Private walshCodeNumber As Integer
	Private walshCodeLength As Integer
	Private branch As RFmxEvdoMXCdaUplinkBranch

	Private powerUnit As RFmxEvdoMXCdaPowerUnit
	Private physicalLayerSubtype As RFmxEvdoMXPhysicalLayerSubtype
	Private uplinkDataModulationType As RFmxEvdoMXUplinkDataModulationType

	Private timeout As Double
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
			ConfigureEvdo()
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

		measurement = RFmxEvdoMXMeasurementTypes.Cda

		frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
		frequencyReferenceFrequency = 10000000.0
		' Hz 

		digitalEdgeTriggerSource = RFmxEvdoMXConstants.Pfi0
		digitalEdgeTriggerEdge = RFmxEvdoMXDigitalEdgeTriggerEdge.Rising
		triggerDelay = 0.0
		' seconds 
		enableTrigger = False

		uplinkSpreadingIMask = 0
		uplinkSpreadingQMask = 0
		synchronizationMode = RFmxEvdoMXCdaSynchronizationMode.Slot
		channelConfigurationMode = RFmxEvdoMXChannelConfigurationMode.AutoDetect
		measurementOffset = 0
		' slots 
		measurementLength = 1
		' slots 

		walshCodeNumber = 0
		walshCodeLength = 16
		branch = RFmxEvdoMXCdaUplinkBranch.I

		powerUnit = RFmxEvdoMXCdaPowerUnit.dB
		physicalLayerSubtype = RFmxEvdoMXPhysicalLayerSubtype.Subtype0_1
		uplinkDataModulationType = RFmxEvdoMXUplinkDataModulationType.Auto

		timeout = 10
		' seconds 

		enableAllTraces = True
	End Sub

	Private Sub ConfigureEvdo()
		evdo = instrSession.GetEvdoSignalConfiguration()
		instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
		evdo.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
		evdo.ConfigureDigitalEdgeTrigger("", digitalEdgeTriggerSource, digitalEdgeTriggerEdge, triggerDelay, enableTrigger)
		evdo.ConfigureChannelConfigurationMode("", channelConfigurationMode)
		evdo.ConfigurePhysicalLayerSubtype("", physicalLayerSubtype)
		evdo.ConfigureUplinkDataModulationType("", uplinkDataModulationType)
		evdo.ConfigureUplinkSpreading("", uplinkSpreadingIMask, uplinkSpreadingQMask)
		evdo.SelectMeasurements("", measurement, enableAllTraces)
		evdo.Cda.Configuration.ConfigureSynchronizationModeAndInterval("", synchronizationMode, measurementOffset, measurementLength)
		evdo.Cda.Configuration.ConfigureUplinkMeasurementChannel("", walshCodeLength, walshCodeNumber, branch)
		evdo.Cda.Configuration.ConfigurePowerUnit("", powerUnit)
		evdo.Initiate("", "")
	End Sub

	Private Sub RetrieveResults()
		evdo.Cda.Results.FetchUplinkCodeDomainPower("", timeout, totalPower, totalActivePower, meanActivePower, peakActivePower, _
			meanInactivePower, peakInactivePower)
		evdo.Cda.Results.FetchUplinkCodeDomainIAndQPower("", timeout, iMeanActivePower, qMeanActivePower, iPeakInactivePower, qPeakInactivePower)
		evdo.Cda.Results.FetchUplinkSymbolEvm("", timeout, rmsSymbolEvm, peakSymbolEvm, rmsSymbolMagnitudeError, rmsSymbolPhaseError, _
			meanSymbolPower, frequencyError, chipRateError)
		evdo.Cda.Results.FetchIQImpairments("", timeout, iqOriginOffset, iqGainImbalance, iqQuadratureError)
		evdo.Cda.Results.FetchUplinkCodeDomainIandQPowerTrace("", timeout, iCodeDomainPowers, qCodeDomainPowers)
		evdo.Cda.Results.FetchUplinkSymbolEvmTrace("", timeout, symbolEvm)
		evdo.Cda.Results.FetchUplinkSymbolConstellationTrace("", timeout, symbolConstellation)
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
		If evdo IsNot Nothing Then
			evdo.Dispose()
			evdo = Nothing
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
