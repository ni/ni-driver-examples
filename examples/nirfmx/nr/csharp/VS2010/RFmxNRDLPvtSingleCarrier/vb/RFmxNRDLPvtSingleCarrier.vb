'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure Selected Ports.
'4. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
'5. Configure Trigger Type and Trigger Parameters.
'6. Configure Link Direction as Downlink, Frequency Range, Carrier Bandwidth and BWP Subcarrier Spacing.
'7. Configure DL Test Model and DL Test Model Duplex Scheme.
'8. Configure gNodeB Type.
'9. Configure Rated TRP and Rated EIRP.
'10. Select PVT measurement and enable Traces.
'11. Configure Measurement Methods.
'12. Configure OFF Power Exclusion Periods.
'13. Configure Averaging Parameters for PVT measurement.
'14. Configure Measurement Interval.
'15. Initiate the Measurement.
'16. Fetch PVT Measurements and Traces.
'17. Close RFmx Session. 

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.NRMX


Namespace NationalInstruments.Examples.RFmxNRDLPvtSingleCarrier
	Public Class RFmxNRDLPvtSingleCarrier
		Private instrSession As RFmxInstrMX
		Private NR As RFmxNRMX
		Private resourceName As String

		Private selectedPorts As String
		Private centerFrequency As Double
		Private referenceLevel As Double
		Private externalAttenuation As Double

		Private frequencyReferenceSource As String
		Private frequencyReferenceFrequency As Double

		Private iqPowerEdgeLevel As Double
		Private triggerDelay As Double
		Private minimumQuietTimeMode As RFmxNRMXTriggerMinimumQuietTimeMode
		Private minimumQuietTime As Double

		Private frequencyRange As RFmxNRMXFrequencyRange
		Private carrierBandwidth As Double
		Private subcarrierSpacing As Double
		Private downlinkTestModel As RFmxNRMXDownlinkTestModel
		Private downlinkTestModelDuplexScheme As RFmxNRMXDownlinkTestModelDuplexScheme

		Private gNodeBType As RFmxNRMXgNodeBType
		Private ratedTRP As Double
		Private ratedEIRP As Double
		Private measurementMethod As RFmxNRMXPvtMeasurementMethod
		Private offPowerExclusionBefore As Double
		Private offPowerExclusionAfter As Double

		Private averagingEnabled As RFmxNRMXPvtAveragingEnabled
		Private averagingCount As Integer
		Private averagingType As RFmxNRMXPvtAveragingType

		Private measurementIntervalAuto As RFmxNRMXPvtMeasurementIntervalAuto
		Private measurementInterval As Double

		Private timeout As Double

		Private measurementStatus As RFmxNRMXPvtMeasurementStatus
		Private pvtResultsPkWindowedOffPwr As Double
		' (dBm/MHz) 
		Private pvtResultsPkWindowedOffPwrMargin As Double
		' (dB) 
		Private pvtResultsPkWindowedOffPwrTime As Double
		' (s) 
		Private absoluteONPower As Double
		' (dBm) 

		Private signalPower As AnalogWaveform(Of Single)
		' (dBm/MHz) 
		Private absoluteLimit As AnalogWaveform(Of Single)
		' (dBm/MHz) 
		Private windowedSignalPower As AnalogWaveform(Of Single)
		' (dBm/MHz) 

		Public Sub Run()
			Try
				InitializeVariables()
				InitializeInstr()
				ConfigureNR()
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

		Private Sub InitializeVariables()
			resourceName = "RFSA"

			selectedPorts = ""
			centerFrequency = 3500000000.0
			' (Hz) 
			referenceLevel = 0.0
			' (dBm) 
			externalAttenuation = 0.0
			' (dB) 

			frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
			frequencyReferenceFrequency = 10000000.0
			' (Hz) 

			iqPowerEdgeLevel = -20.0
			' (dB) 
			triggerDelay = 0.0
			' (s) 
			minimumQuietTimeMode = RFmxNRMXTriggerMinimumQuietTimeMode.Auto
			minimumQuietTime = 0.000008
			' (s) 

			frequencyRange = RFmxNRMXFrequencyRange.Range1
			carrierBandwidth = 100000000.0
			' (Hz) 
			subcarrierSpacing = 30000.0
			' (Hz) 
			downlinkTestModelDuplexScheme = RFmxNRMXDownlinkTestModelDuplexScheme.Tdd
			downlinkTestModel = RFmxNRMXDownlinkTestModel.TM1_1

			gNodeBType = RFmxNRMXgNodeBType.Type1C
			ratedTRP = 0.0
			' (dBm) 
			ratedEIRP = 0.0
			' (dBm) 
			measurementMethod = RFmxNRMXPvtMeasurementMethod.Normal
			offPowerExclusionBefore = 0.0
			' (s) 
			offPowerExclusionAfter = 0.0
			' (s) 

			averagingEnabled = RFmxNRMXPvtAveragingEnabled.[False]
			averagingCount = 10
			averagingType = RFmxNRMXPvtAveragingType.Rms

			measurementIntervalAuto = RFmxNRMXPvtMeasurementIntervalAuto.[True]
			measurementInterval = 0.01
			' (s) 

			timeout = 10.0
			' (s) 
		End Sub

		Private Sub InitializeInstr()
			instrSession = New RFmxInstrMX(resourceName, "")
		End Sub

		Private Sub ConfigureNR()
			NR = instrSession.GetNRSignalConfiguration()
			' Create a new RFmx Session 
			instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
			NR.SetSelectedPorts("", selectedPorts)
			NR.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
			NR.ConfigureIQPowerEdgeTrigger("", "0", RFmxNRMXIQPowerEdgeTriggerSlope.Rising, iqPowerEdgeLevel, triggerDelay, minimumQuietTimeMode,
				minimumQuietTime, RFmxNRMXIQPowerEdgeTriggerLevelType.Relative, True)

			NR.SetLinkDirection("", RFmxNRMXLinkDirection.Downlink)
			NR.SetFrequencyRange("", frequencyRange)
			NR.ComponentCarrier.SetBandwidth("", carrierBandwidth)
			NR.ComponentCarrier.SetBandwidthPartSubcarrierSpacing("", subcarrierSpacing)

			NR.ComponentCarrier.SetDownlinkTestModel("", downlinkTestModel)
			NR.ComponentCarrier.SetDownlinkTestModelDuplexScheme("", downlinkTestModelDuplexScheme)

			NR.SetgNodeBType("", gNodeBType)

			NR.ComponentCarrier.SetRatedTrp("", ratedTRP)
			NR.ComponentCarrier.SetRatedEirp("", ratedEIRP)

			NR.SelectMeasurements("", RFmxNRMXMeasurementTypes.Pvt, True)

			NR.Pvt.Configuration.ConfigureMeasurementMethod("", measurementMethod)
			NR.Pvt.Configuration.ConfigureOffPowerExclusionPeriods("", offPowerExclusionBefore, offPowerExclusionAfter)
			NR.Pvt.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)
			NR.Pvt.Configuration.SetMeasurementIntervalAuto("", measurementIntervalAuto)
			NR.Pvt.Configuration.SetMeasurementInterval("", measurementInterval)

			NR.Initiate("", "")
		End Sub

		Private Sub RetrieveResults()
			NR.Pvt.Results.GetMeasurementStatus("", measurementStatus)
			NR.Pvt.Results.GetPeakWindowedOffPower("", pvtResultsPkWindowedOffPwr)
			NR.Pvt.Results.GetPeakWindowedOffPowerMargin("", pvtResultsPkWindowedOffPwrMargin)
			NR.Pvt.Results.GetPeakWindowedOffPowerTime("", pvtResultsPkWindowedOffPwrTime)
			NR.Pvt.Results.GetAbsoluteONPower("", absoluteONPower)

			NR.Pvt.Results.FetchSignalPowerTrace("", timeout, signalPower, absoluteLimit)

			NR.Pvt.Results.FetchWindowedSignalPowerTrace("", timeout, windowedSignalPower)
		End Sub

		Private Sub PrintResults()
			Console.WriteLine("------------------Measurement------------------" & vbLf)
			Console.WriteLine("Measurement Status                             : {0}", measurementStatus)
			Console.WriteLine("PVT Results Pk Windowed OFF Pwr (dBm/MHz)      : {0}", pvtResultsPkWindowedOffPwr)
			Console.WriteLine("PVT Results Pk Windowed OFF Pwr Margin (dB)    : {0}", pvtResultsPkWindowedOffPwrMargin)
			Console.WriteLine("PVT Results Pk Windowed OFF Pwr Time (s)       : {0}", pvtResultsPkWindowedOffPwrTime)
			Console.WriteLine("Absolute ON Power (dBm)                        : {0}", absoluteONPower)
		End Sub

		Private Sub CloseSession()
			If NR IsNot Nothing Then
				NR.Dispose()
				NR = Nothing
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
End Namespace

