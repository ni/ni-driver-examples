'Steps:
'1. Open a new RFmx session
'2. Configure the basic instrument properties: Clock Source and Clock Frequency
'3. Configure the basic signal properties: Center Frequency, Reference Level and External Attenuation
'4. Configure the trigger properties
'5. Select SEM measurement and enable the traces
'6. Configure Sweep Time for the SEM measurement
'7. Configure Averaging Parameters for the SEM measurement
'8. Initiate Measurement
'9. Fetch SEM Measurements and Traces
'10. Close the RFmx session

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.Cdma2kMX

Public Class RFmxCdma2kSem
	Private instrSession As RFmxInstrMX
	Private cdma2k As RFmxCdma2kMX
	Private resourceName As [String], frequencySource As [String]
	Private centerFrequency As Double, referenceLevel As Double, externalAttenuation As Double, frequency As Double, sweepTimeInterval As Double

	Private averagingCount As Integer
	Private averagingEnabled As RFmxCdma2kMXSemAveragingEnabled
	Private averagingType As RFmxCdma2kMXSemAveragingType
	Private sweepTimeAuto As RFmxCdma2kMXSemSweepTimeAuto

	Private bandclass As Integer

	Private timeout As Double = 10.0

	Const NumberOfOffsets As Integer = 2

    'Output values
	Private carrierAbsoluteIntegratedPower As Double
	Private lowerOffsetMargin As Double()
	Private lowerOffsetMarginAbsolutePower As Double()
	Private lowerOffsetMarginRelativePower As Double()
	Private lowerOffsetMarginFrequency As Double()
	Private lowerOffsetMeasurementStatus As RFmxCdma2kMXSemLowerOffsetMeasurementStatus()

	Private measurementStatus As RFmxCdma2kMXSemMeasurementStatus

	Private upperOffsetMargin As Double()
	Private upperOffsetMarginAbsolutePower As Double()
	Private upperOffsetMarginRelativePower As Double()
	Private upperOffsetMarginFrequency As Double()

	Private upperOffsetMeasurementStatus As RFmxCdma2kMXSemUpperOffsetMeasurementStatus()

	Private digitalEdgeTriggerSource As String
	Private digitalEdgeTriggerEdge As RFmxCdma2kMXDigitalEdgeTriggerEdge
	Private triggerDelay As Double
	Private enableTrigger As Boolean

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
			Console.WriteLine("Press any key to exit.....")
			Console.ReadKey()
		End Try
	End Sub

	Private Sub InitializeVariables()
		resourceName = "RFSA"
		centerFrequency = 833490000.0
		' Hz 
		referenceLevel = 0.0
		' dBm 
		externalAttenuation = 0.0
		' dB 

		frequencySource = RFmxInstrMXConstants.OnboardClock
		frequency = 10000000.0
		' Hz 

		triggerDelay = 0.0
		' seconds 

		digitalEdgeTriggerSource = RFmxCdma2kMXConstants.Pfi0
		digitalEdgeTriggerEdge = RFmxCdma2kMXDigitalEdgeTriggerEdge.Rising
		enableTrigger = False

		bandclass = 0

		' Sweep Time
		sweepTimeAuto = RFmxCdma2kMXSemSweepTimeAuto.[True]
		sweepTimeInterval = 0.00167
		' seconds 

		averagingEnabled = RFmxCdma2kMXSemAveragingEnabled.[False]
		averagingCount = 10
		averagingType = RFmxCdma2kMXSemAveragingType.Rms
	End Sub


	Private Sub InitializeInstr()
		' Create a new RFmx Session 

		instrSession = New RFmxInstrMX(resourceName, "")
	End Sub

	Private Sub ConfigureCdma2k()
		' Get SpecAn signal 

		cdma2k = instrSession.GetCdma2kSignalConfiguration()

		' Configure measurement 

		instrSession.ConfigureFrequencyReference("", frequencySource, frequency)
		cdma2k.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
		cdma2k.ConfigureDigitalEdgeTrigger("", digitalEdgeTriggerSource, digitalEdgeTriggerEdge, triggerDelay, enableTrigger)

		cdma2k.ConfigureBandClass("", bandclass)

		cdma2k.SelectMeasurements("", RFmxCdma2kMXMeasurementTypes.Sem, True)

		cdma2k.Sem.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval)

		cdma2k.Sem.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)

		cdma2k.Initiate("", "")
	End Sub

	Private Sub RetrieveResults()
		' Retrieve results 


		Dim spectrum As Spectrum(Of Single) = Nothing, absoluteMask As Spectrum(Of Single) = Nothing, relativeMask As Spectrum(Of Single) = Nothing

		cdma2k.Sem.Results.FetchLowerOffsetMarginArray("", timeout, lowerOffsetMeasurementStatus, lowerOffsetMargin, lowerOffsetMarginFrequency, lowerOffsetMarginAbsolutePower, _
			lowerOffsetMarginRelativePower)

		cdma2k.Sem.Results.FetchUpperOffsetMarginArray("", timeout, upperOffsetMeasurementStatus, upperOffsetMargin, upperOffsetMarginFrequency, upperOffsetMarginAbsolutePower, _
			upperOffsetMarginRelativePower)

		cdma2k.Sem.Results.FetchMeasurementStatus("", timeout, measurementStatus)
		cdma2k.Sem.Results.FetchCarrierAbsoluteIntegratedPower("", timeout, carrierAbsoluteIntegratedPower)
		cdma2k.Sem.Results.FetchSpectrum("", timeout, spectrum, absoluteMask, relativeMask)

	End Sub

	Private Sub PrintResults()
		Console.WriteLine("Measurement Status                         : {0}" & vbLf, measurementStatus)

        Console.WriteLine("--------------------------Carrier Measurement-----------------------------" & vbLf)
        Console.WriteLine("Carrier Absolute Integrated Power (dBm)    : {0}", carrierAbsoluteIntegratedPower)

        Console.WriteLine(vbLf & "--------------Offset segment measurements ---------------------------" & vbLf)
        Console.WriteLine(vbLf & "Lower Offset segment measurements")
		For i As Integer = 0 To NumberOfOffsets - 1
			Console.WriteLine("Offset                                     : {0}" & vbLf, i)

			Console.WriteLine("Lower Offset : Margin (dB)                 : {0}", lowerOffsetMargin(i))
			Console.WriteLine("Lower offset : Margin Absolute Power (dBm) : {0}", lowerOffsetMarginAbsolutePower(i))
			Console.WriteLine("Lower offset : Margin Relative Power (dB)  : {0}", lowerOffsetMarginRelativePower(i))
			Console.WriteLine("Lower offset : Margin Frequency (Hz)       : {0}", lowerOffsetMarginFrequency(i))
			Console.WriteLine("Lower offset : Measurement Status          : {0}" & vbLf, lowerOffsetMeasurementStatus(i))
        Next

        Console.WriteLine(vbLf)
        Console.WriteLine(vbLf & "Upper Offset segment measurements")
        For i As Integer = 0 To NumberOfOffsets - 1
            Console.WriteLine("Offset                                     : {0}" & vbLf, i)
            Console.WriteLine("Upper Offset : Margin (dB)                 : {0}", upperOffsetMargin(i))
            Console.WriteLine("Upper offset : Margin Absolute Power (dBm) : {0}", upperOffsetMarginAbsolutePower(i))
            Console.WriteLine("Upper offset : Margin Relative Power (dB)  : {0}", upperOffsetMarginRelativePower(i))
            Console.WriteLine("Upper offset : Margin Frequency (Hz)       : {0}", upperOffsetMarginFrequency(i))
            Console.WriteLine("Upper offset : Measurement Status          : {0}" & vbLf, upperOffsetMeasurementStatus(i))
            Console.WriteLine("-----------------------------------------------------------------------" & vbLf)
        Next
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

	Private Sub DisplayError(ex As Exception)
		Console.WriteLine("ERROR:" & vbLf & Convert.ToString(ex.[GetType]()) & ": " & ex.Message)
	End Sub
End Class
