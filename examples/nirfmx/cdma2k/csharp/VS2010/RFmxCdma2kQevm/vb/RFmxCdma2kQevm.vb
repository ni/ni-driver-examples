'Steps:
'1. Open a new RFmx session.
'2. Configure Frequency Reference.
'3. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation).
'4. Configure Trigger Type and Trigger Parameters.
'5. Configure Radio Configuration Parameter.
'6. Configure Uplink Spreading Long Code Mask Parameter.
'7. Select QEVM measurement and enable traces.
'8. Configure Synchronization Mode.
'9. Configure  Measurement Mode & Measurement Length.
'10. Initiate the Measurement.
'11. Fetch QEVM Measurements and Traces.
'12. Close the RFmx Session.

Imports NationalInstruments.RFmx.Cdma2kMX
Imports NationalInstruments.RFmx.InstrMX

Namespace NationalInstruments.Examples.Cdma2kQevm
	Public Class RFmxCdma2kQevm
		Private instrSession As RFmxInstrMX
		Private cdma2k As RFmxCdma2kMX

		Private resourceName As String = "RFSA"
		Private frequencySource As String = RFmxInstrMXConstants.OnboardClock
		Private digitalEdgeTriggerSource As String = RFmxCdma2kMXConstants.Pfi0
		Private centerFrequency As Double = 833490000.0
		' Hz 
        Private referenceLevel As Double = 0.0          ' dBm 
		Private externalAttenuation As Double = 0.0		' dB 

        Private frequency As Double = 10000000.0        ' Hz 
        Private timeout As Double = 10                  'sec

		Private digitalEdgeTriggerEdge As RFmxCdma2kMXDigitalEdgeTriggerEdge = RFmxCdma2kMXDigitalEdgeTriggerEdge.Rising
        Private triggerDelay As Double = 0.0                       'sec
		Private enableTrigger As Boolean = False

		Private radioConfiguration As RFmxCdma2kMXRadioConfiguration = RFmxCdma2kMXRadioConfiguration.RC3
		Private uplinkSpreadingLongCodeMask As Long = 0

		Private averagingEnabled As RFmxCdma2kMXQevmAveragingEnabled = RFmxCdma2kMXQevmAveragingEnabled.[False]
		Private averagingCount As Integer = 10
		Private measurementLength As Integer = 1536		'chips

		Private meanRmsEvm As Double, maximumPeakEvm As Double, meanFrequencyError As Double, meanMagnitudeError As Double, meanPhaseError As Double, meanChipRateError As Double
		Private meanIQOriginOffset As Double, meanIQGainImbalance As Double, meanIQQuadratureError As Double, maximumIQOriginOffset As Double
		Private maximumIQGainImbalance As Double, maximumIQQuadratureError As Double

		Public Sub Run()
			Try
				InitializeInstr()
				ConfigureCdma2k()
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

        Private Sub InitializeInstr()

            ' Create a new RFmx Session 
            instrSession = New RFmxInstrMX(resourceName, "")
        End Sub

		Private Sub ConfigureCdma2k()

            ' Get CDMA2k signal 
			cdma2k = instrSession.GetCdma2kSignalConfiguration()

			' Configure measurement 
			instrSession.ConfigureFrequencyReference("", frequencySource, frequency)
			cdma2k.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
			cdma2k.ConfigureDigitalEdgeTrigger("", digitalEdgeTriggerSource, digitalEdgeTriggerEdge, triggerDelay, enableTrigger)
			cdma2k.ConfigureRadioConfiguration("", radioConfiguration)
			cdma2k.ConfigureUplinkSpreading("", uplinkSpreadingLongCodeMask)
			cdma2k.SelectMeasurements("", RFmxCdma2kMXMeasurementTypes.Qevm, True)
			cdma2k.Qevm.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount)
			cdma2k.Qevm.Configuration.ConfigureMeasurementLength("", measurementLength)
			cdma2k.Initiate("", "")
		End Sub

		Private Sub RetrieveResults()
			Dim evm As AnalogWaveform(Of Single) = Nothing
			Dim constellation As ComplexSingle() = Nothing

            ' Retrieve results 
			cdma2k.Qevm.Results.FetchEvm("", timeout, meanRmsEvm, maximumPeakEvm, meanFrequencyError, meanMagnitudeError, _
				meanPhaseError, meanChipRateError)
			cdma2k.Qevm.Results.FetchIQImpairments("", timeout, meanIQOriginOffset, meanIQGainImbalance, meanIQQuadratureError, maximumIQOriginOffset, _
				maximumIQGainImbalance, maximumIQQuadratureError)
			cdma2k.Qevm.Results.FetchEvmTrace("", timeout, evm)
			cdma2k.Qevm.Results.FetchConstellationTrace("", timeout, constellation)
		End Sub

		Private Sub PrintResults()
			Console.WriteLine("--------------------EVM Results--------------------")
			Console.WriteLine("Mean RMS EVM (%)                     : {0}", meanRmsEvm)
			Console.WriteLine("Maximum Peak EVM (%)                 : {0}", maximumPeakEvm)
			Console.WriteLine("Mean Frequency Error (Hz)            : {0}", meanFrequencyError)
			Console.WriteLine("Mean Chip Rate Error (ppm)           : {0}", meanChipRateError)
			Console.WriteLine("Mean Magnitude Error (%)             : {0}", meanMagnitudeError)
			Console.WriteLine("Mean Phase Error (deg)               : {0}", meanPhaseError)

			Console.WriteLine("---------------------I/Q Impairments------------------------")
			Console.WriteLine("Mean I/Q Origin Offset (dB)          : {0}", meanIQOriginOffset)
			Console.WriteLine("Mean I/Q Gain Imbalance (dB)         : {0}", meanIQGainImbalance)
			Console.WriteLine("Mean I/Q Quadrature Error (deg)      : {0}", meanIQQuadratureError)
			Console.WriteLine("Maximum I/Q Origin Offset (dB)       : {0}", maximumIQOriginOffset)
			Console.WriteLine("Maximum I/Q Gain Imbalance (dB)      : {0}", maximumIQGainImbalance)
			Console.WriteLine("Maximum I/Q Quadrature Error (deg)   : {0}", maximumIQQuadratureError)
		End Sub

		Private Sub CloseSession()
			Try
				If cdma2k IsNot Nothing Then
					cdma2k.Dispose()
					cdma2k = Nothing
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
End Namespace
