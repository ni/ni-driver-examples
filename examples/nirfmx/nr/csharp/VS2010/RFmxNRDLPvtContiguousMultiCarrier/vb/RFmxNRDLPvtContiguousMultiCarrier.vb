'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure Selected Ports.
'4. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
'5. Configure Trigger Type and Trigger Parameters.
'6. Configure Link Direction as Downlink, Frequency Range, Channel Raster, Component Carrier Spacing and gNodeB Type.
'7. Configure Carrier.
'8. Configure BWP Subcarrier Spacing, DL Test Model Duplex Scheme and DL Test Model for all carriers
'9. Select PVT measurement and enable Traces.
'10. Configure Measurement Methods.
'11. Configure OFF Power Exclusion Periods.
'12. Configure Averaging Parameters for PVT measurement.
'13. Configure Measurement Interval.
'14. Initiate the Measurement.
'15. Fetch PVT Measurements and Traces.
'16. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.NRMX


Namespace NationalInstruments.Examples.RFmxNRDLPvtContiguousMultiCarrier
	Public Class RFmxNRDLPvtContiguousMultiCarrier
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
		Private componentCarrierSpacingType As RFmxNRMXComponentCarrierSpacingType
		Private measurementMethod As RFmxNRMXPvtMeasurementMethod
		Private offPowerExclusionBefore As Double
		Private offPowerExclusionAfter As Double
		Private gNodeBType As RFmxNRMXgNodeBType

		Private channelRaster As Double
		Private componentCarrierAtCenterFrequency As Integer
		Private subcarrierSpacing As Double

		Const NumberOfComponentCarriers As Integer = 2
		Private componentCarrierBandwidth As Double() = New Double(NumberOfComponentCarriers - 1) {}
		Private componentCarrierFrequency As Double() = New Double(NumberOfComponentCarriers - 1) {}
		Private ratedTRP As Double() = New Double(NumberOfComponentCarriers - 1) {}
		Private ratedEIRP As Double() = New Double(NumberOfComponentCarriers - 1) {}

		Private downlinkTestModel As RFmxNRMXDownlinkTestModel
		Private downlinkTestModelDuplexScheme As RFmxNRMXDownlinkTestModelDuplexScheme


		Private averagingEnabled As RFmxNRMXPvtAveragingEnabled
		Private averagingCount As Integer
		Private averagingType As RFmxNRMXPvtAveragingType

		Private measurementIntervalAuto As RFmxNRMXPvtMeasurementIntervalAuto
		Private measurementInterval As Double

		Private subblockString As String
		Private carrierString As String

		Private timeout As Double

		Private measurementStatus As RFmxNRMXPvtMeasurementStatus() = New RFmxNRMXPvtMeasurementStatus(NumberOfComponentCarriers - 1) {}
		Private pvtResultsPkWindowedOffPwr As Double() = New Double(NumberOfComponentCarriers - 1) {}
		' (dBm/MHz) 
		Private pvtResultsPkWindowedOffPwrMargin As Double() = New Double(NumberOfComponentCarriers - 1) {}
		' (dB) 
		Private pvtResultsPkWindowedOffPwrTime As Double() = New Double(NumberOfComponentCarriers - 1) {}
		' (s) 
		Private absoluteONPower As Double() = New Double(NumberOfComponentCarriers - 1) {}
		' (dBm) 

		Private signalPower As AnalogWaveform(Of Single)() = New AnalogWaveform(Of Single)(NumberOfComponentCarriers - 1) {}
		' (dBm/MHz) 
		Private absoluteLimit As AnalogWaveform(Of Single)() = New AnalogWaveform(Of Single)(NumberOfComponentCarriers - 1) {}
		' (dBm/MHz) 
		Private windowedSignalPower As AnalogWaveform(Of Single)() = New AnalogWaveform(Of Single)(NumberOfComponentCarriers - 1) {}
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
			subcarrierSpacing = 30000.0
			' (Hz) 
			measurementMethod = RFmxNRMXPvtMeasurementMethod.Normal
			offPowerExclusionBefore = 0.0
			' (s) 
			offPowerExclusionAfter = 0.0
			' (s) 
			gNodeBType = RFmxNRMXgNodeBType.Type1C

			componentCarrierSpacingType = RFmxNRMXComponentCarrierSpacingType.Nominal
			channelRaster = 15000.0
			' (Hz) 
			componentCarrierAtCenterFrequency = -1

			componentCarrierBandwidth(0) = 100000000.0
			' (Hz) 
			componentCarrierBandwidth(1) = 100000000.0
			' (Hz) 
			componentCarrierFrequency(0) = -49980000.0
			' (Hz) 
			componentCarrierFrequency(1) = 50010000.0
			' (Hz) 
			ratedTRP(0) = 0.0
			' (dBm) 
			ratedTRP(1) = 0.0
			' (dBm) 
			ratedEIRP(0) = 0.0
			' (dBm) 
			ratedEIRP(1) = 0.0
			' (dBm) 

			downlinkTestModel = RFmxNRMXDownlinkTestModel.TM1_1
			downlinkTestModelDuplexScheme = RFmxNRMXDownlinkTestModelDuplexScheme.Tdd

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
			NR.SetChannelRaster("", channelRaster)
			NR.SetComponentCarrierSpacingType("", componentCarrierSpacingType)
			NR.SetComponentCarrierAtCenterFrequency("", componentCarrierAtCenterFrequency)
			NR.SetgNodeBType("", gNodeBType)

			NR.ComponentCarrier.SetNumberOfComponentCarriers("", NumberOfComponentCarriers)

			subblockString = RFmxNRMX.BuildSubblockString("", 0)
			For i As Integer = 0 To NumberOfComponentCarriers - 1
				carrierString = RFmxNRMX.BuildCarrierString(subblockString, i)
				NR.ComponentCarrier.SetBandwidth(carrierString, componentCarrierBandwidth(i))
				NR.ComponentCarrier.SetFrequency(carrierString, componentCarrierFrequency(i))
				NR.ComponentCarrier.SetRatedTrp(carrierString, ratedTRP(i))
				NR.ComponentCarrier.SetRatedEirp(carrierString, ratedEIRP(i))
			Next

			carrierString = "carrier::all"

			NR.ComponentCarrier.SetBandwidthPartSubcarrierSpacing(carrierString, subcarrierSpacing)
			NR.ComponentCarrier.SetDownlinkTestModelDuplexScheme(carrierString, downlinkTestModelDuplexScheme)
			NR.ComponentCarrier.SetDownlinkTestModel(carrierString, downlinkTestModel)

			NR.SelectMeasurements("", RFmxNRMXMeasurementTypes.Pvt, True)

			NR.Pvt.Configuration.ConfigureMeasurementMethod("", measurementMethod)
			NR.Pvt.Configuration.ConfigureOffPowerExclusionPeriods("", offPowerExclusionBefore, offPowerExclusionAfter)
			NR.Pvt.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)

			NR.Pvt.Configuration.SetMeasurementIntervalAuto("", measurementIntervalAuto)
			NR.Pvt.Configuration.SetMeasurementInterval("", measurementInterval)
			NR.Initiate("", "")
		End Sub

		Private Sub RetrieveResults()
			subblockString = RFmxNRMX.BuildSubblockString("", 0)
			For i As Integer = 0 To NumberOfComponentCarriers - 1
				carrierString = RFmxNRMX.BuildCarrierString(subblockString, i)
				NR.Pvt.Results.GetMeasurementStatus(carrierString, measurementStatus(i))
				NR.Pvt.Results.GetPeakWindowedOffPower(carrierString, pvtResultsPkWindowedOffPwr(i))
				NR.Pvt.Results.GetPeakWindowedOffPowerMargin(carrierString, pvtResultsPkWindowedOffPwrMargin(i))
				NR.Pvt.Results.GetPeakWindowedOffPowerTime(carrierString, pvtResultsPkWindowedOffPwrTime(i))
				NR.Pvt.Results.GetAbsoluteONPower(carrierString, absoluteONPower(i))
			Next

			For i As Integer = 0 To NumberOfComponentCarriers - 1
				carrierString = RFmxNRMX.BuildCarrierString("", i)
				NR.Pvt.Results.FetchSignalPowerTrace(carrierString, timeout, signalPower(i), absoluteLimit(i))

				NR.Pvt.Results.FetchWindowedSignalPowerTrace(carrierString, timeout, windowedSignalPower(i))
			Next
		End Sub

		Private Sub PrintResults()
			Console.WriteLine("------------------------Measurements------------------------" & vbLf)
			For i As Integer = 0 To NumberOfComponentCarriers - 1
				Console.WriteLine("Carrier  : {0}", i)
				Console.WriteLine("Measurement Status                                   : {0}", measurementStatus(i))
				Console.WriteLine("PVT Results Pk Windowed OFF Pwr (dBm/MHz)            : {0}", pvtResultsPkWindowedOffPwr(i))
				Console.WriteLine("PVT Results Pk Windowed OFF Pwr Margin (dB)          : {0}", pvtResultsPkWindowedOffPwrMargin(i))
				Console.WriteLine("PVT Results Pk Windowed OFF Pwr Time (s)             : {0}", pvtResultsPkWindowedOffPwrTime(i))
				Console.WriteLine("Absolute ON Power (dBm)                              : {0}", absoluteONPower(i))
				Console.WriteLine("-----------------------------------------------------------------" & vbLf)
			Next
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

