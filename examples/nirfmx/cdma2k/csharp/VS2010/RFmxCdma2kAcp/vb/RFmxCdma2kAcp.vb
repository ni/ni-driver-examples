'Steps:
'1. Open a new RFmx session
'2. Configure Reference Clock
'3. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
'4. Configure RF Attenuation
'5. Configure Trigger
'6. Configure Band Class
'7. Set Measurement to ACP
'8. Configure Number of Offsets
'9. Set Noise Compensation
'10. Set Dynamic Range Mode
'11. Configure Sweep Time
'12. Configure Averaging Parameters
'13. Commit Settings and Initiate Measurement
'14. Fetch diverse ACP Measurement Results
'15. Close the RFmx Session


Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.Cdma2kMX

Public Class RFmxCdma2kAcp
	Private instrSession As RFmxInstrMX
	Private cdma2k As RFmxCdma2kMX
	Private i As Integer = 0
	Private centerFrequency As Double
	Private externalAttenuation As Double
	Private autoLevel As Boolean
	Private referenceLevel As Double
	Private frequency As Double
	Private triggerDelay As Double
	Private numberOfOffsets As Integer
	Private sweepTimeInterval As Double
	Private averagingCount As Integer
	Private timeout As Double
	Private attenuationAuto As RFmxInstrMXRFAttenuationAuto
	Private attenuationValue As Double
	Private digitalEdgeTriggerSource As String
	Private digitalEdgeTriggerEdge As RFmxCdma2kMXDigitalEdgeTriggerEdge
	Private enableTrigger As Boolean
	Private bandclass As Integer
	Private noiseCompensationEnabled As RFmxCdma2kMXAcpNoiseCompensationEnabled
	Private measurementMethod As RFmxCdma2kMXAcpMeasurementMethod
	Private sweepTimeAuto As RFmxCdma2kMXAcpSweepTimeAuto
	Private averagingEnabled As RFmxCdma2kMXAcpAveragingEnabled
	Private averagingType As RFmxCdma2kMXAcpAveragingType

	Private carrierAbsolutePower As Double

	Private lowerRelativePower As Double()
	Private upperRelativePower As Double()
	Private lowerAbsolutePower As Double()
	Private upperAbsolutePower As Double()

	Private spectrum As Spectrum(Of Single)
	Private measurementInterval As Double
    Private resourceName As String


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

	Private Sub InitializeVariables()
		' Initialize input variables 

		centerFrequency = 833490000.0
		'Hz
		externalAttenuation = 0.0
		'dB
		attenuationAuto = RFmxInstrMXRFAttenuationAuto.[True]
		attenuationValue = 10.0
		'dB
		digitalEdgeTriggerSource = RFmxInstrMXConstants.Pfi0
		digitalEdgeTriggerEdge = RFmxCdma2kMXDigitalEdgeTriggerEdge.Rising
		enableTrigger = False
		bandclass = 0
		noiseCompensationEnabled = RFmxCdma2kMXAcpNoiseCompensationEnabled.[False]
		measurementMethod = RFmxCdma2kMXAcpMeasurementMethod.Normal
		sweepTimeAuto = RFmxCdma2kMXAcpSweepTimeAuto.[True]
		averagingEnabled = RFmxCdma2kMXAcpAveragingEnabled.[False]
		averagingType = RFmxCdma2kMXAcpAveragingType.Rms
		autoLevel = True
		referenceLevel = 0.0
		'dBm
		frequency = 10000000.0
		'Hz
		triggerDelay = 0.0
		'seconds
		numberOfOffsets = 2
		sweepTimeInterval = 0.00167
		'seconds
		averagingCount = 10
		timeout = 10.0
		'seconds
		carrierAbsolutePower = 0.0
		'dBm
		lowerRelativePower = Nothing
		upperRelativePower = Nothing
		lowerAbsolutePower = Nothing
		upperAbsolutePower = Nothing

		spectrum = Nothing
		measurementInterval = 0.02
        'seconds
        resourceName = "RFSA"
	End Sub

	Private Sub InitializeInstr()
		' Create a new RFmx Session 

		instrSession = New RFmxInstrMX(resourceName, "")
	End Sub

	Private Sub ConfigureCdma2k()
		' Get cdma2k signal 

		cdma2k = instrSession.GetCdma2kSignalConfiguration()
		' Configure CDMA2k ACP measurement parameters 

		instrSession.ConfigureFrequencyReference("", RFmxInstrMXConstants.OnboardClock, frequency)
		cdma2k.ConfigureExternalAttenuation("", externalAttenuation)
		cdma2k.ConfigureFrequency("", centerFrequency)
		instrSession.ConfigureRFAttenuation("", attenuationAuto, attenuationValue)

        cdma2k.ConfigureDigitalEdgeTrigger("", digitalEdgeTriggerSource, digitalEdgeTriggerEdge, triggerDelay,
                                           enableTrigger)

		If autoLevel Then
			cdma2k.AutoLevel("", measurementInterval, referenceLevel)

            Console.WriteLine("Reference level  (dBm)      : {0}" & vbLf, referenceLevel)
		Else
			cdma2k.ConfigureReferenceLevel("", referenceLevel)
		End If
		cdma2k.ConfigureBandClass("", bandclass)
		cdma2k.SelectMeasurements("", RFmxCdma2kMXMeasurementTypes.Acp, True)
		cdma2k.Acp.Configuration.ConfigureNumberOfOffsets("", numberOfOffsets)

		cdma2k.Acp.Configuration.ConfigureNoiseCompensationEnabled("", noiseCompensationEnabled)
		cdma2k.Acp.Configuration.ConfigureMeasurementMethod("", measurementMethod)
		cdma2k.Acp.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval)
		cdma2k.Acp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)
		cdma2k.Initiate("", "")
	End Sub

	Private Sub RetrieveResults()
		' Fetch ACP Measurement Results 

		cdma2k.Acp.Results.FetchOffsetMeasurementArray("", timeout, lowerRelativePower, upperRelativePower, lowerAbsolutePower, upperAbsolutePower)
		cdma2k.Acp.Results.FetchCarrierAbsolutePower("", timeout, carrierAbsolutePower)
		cdma2k.Acp.Results.FetchSpectrum("", timeout, spectrum)
	End Sub

	Private Sub PrintResults()
		' Display ACP Measurement Results 

		Console.WriteLine("Carrier Absolute Power (dBm):  {0}" & vbLf, carrierAbsolutePower)
		Console.WriteLine("-------------- Offset Channel Measurements --------------")
		For i = 0 To lowerRelativePower.Length - 1
            Console.WriteLine(vbLf & "OFFSET                      : {0}", i)
			Console.WriteLine("Lower Relative Power (dB)   :  {0}", lowerRelativePower(i))
			Console.WriteLine("Upper Relative Power (dB)   :  {0}", upperRelativePower(i))
			Console.WriteLine("Lower Absolute Power (dBm)  :  {0}", lowerAbsolutePower(i))
			Console.WriteLine("Upper Absolute Power (dBm)  :  {0}", upperAbsolutePower(i))
		Next
	End Sub

	Public Sub Run()
		Try
			InitializeVariables()
			InitializeInstr()
			ConfigureCdma2k()
			RetrieveResults()
			PrintResults()
		Catch e As Exception
			Console.WriteLine(e.Message)
		Finally
			CloseSession()
			Console.WriteLine("Press any key to exit")
			Console.ReadKey()
		End Try
	End Sub


End Class
