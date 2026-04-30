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
Imports NationalInstruments.RFmx.TdscdmaMX

Public Class RFmxTdscdmaSem
	Private instrSession As RFmxInstrMX
	Private tdscdma As RFmxTdscdmaMX
	Private resourceName As [String], frequencySource As [String], iqPowerEdgeTriggerSource As [String]
    Private centerFrequency As Double, referenceLevel As Double, externalAttenuation As Double, frequencyReferenceFrequency As Double, _
        sweepTimeInterval As Double, triggerDelay As Double, minimumQuietTimeDuration As Double, iqPowerEdgeTriggerLevel As Double

	Private averagingCount As Integer
	Private averagingEnabled As RFmxTdscdmaMXSemAveragingEnabled
	Private averagingType As RFmxTdscdmaMXSemAveragingType
	Private sweepTimeAuto As RFmxTdscdmaMXSemSweepTimeAuto

	Private timeout As Double = 10.0

    Private carrierAbsoluteIntegratedPower As Double
	Private lowerOffsetMargin As Double()
	Private lowerOffsetMarginAbsolutePower As Double()
	Private lowerOffsetMarginRelativePower As Double()
	Private lowerOffsetMarginFrequency As Double()
	Private lowerOffsetMeasurementStatus As RFmxTdscdmaMXSemLowerOffsetMeasurementStatus()

	Private measurementStatus As RFmxTdscdmaMXSemMeasurementStatus

	Private upperOffsetMargin As Double()
	Private upperOffsetMarginAbsolutePower As Double()
	Private upperOffsetMarginRelativePower As Double()
	Private upperOffsetMarginFrequency As Double()
	Private spectrum As Spectrum(Of Single), absoluteMask As Spectrum(Of Single), relativeMask As Spectrum(Of Single)

	Private upperOffsetMeasurementStatus As RFmxTdscdmaMXSemUpperOffsetMeasurementStatus()

	Private iqPowerEdgeTriggerSlope As RFmxTdscdmaMXIQPowerEdgeTriggerSlope
	Private minimumQuietTimeMode As RFmxTdscdmaMXTriggerMinimumQuietTimeMode
	Private enableTrigger As Boolean
	Private iqPowerEdgeTriggerLevelType As RFmxTdscdmaMXIQPowerEdgeTriggerLevelType

	Public Sub Run()
		Try
			InitializeVariables()
			InitializeInstr()
			ConfigureTdscdma()
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

		centerFrequency = 1910000000.0
		' Hz 
		referenceLevel = 0.0
		' dBm 
		externalAttenuation = 0.0
		' dB 

		frequencySource = RFmxInstrMXConstants.OnboardClock
        frequencyReferenceFrequency = 10000000.0
        ' Hz 

		triggerDelay = 0.0
		' seconds 
		minimumQuietTimeDuration = 1.6E-05
		' seconds 
		iqPowerEdgeTriggerLevel = -20.0
		'dB
		iqPowerEdgeTriggerSource = "0"
		iqPowerEdgeTriggerSlope = RFmxTdscdmaMXIQPowerEdgeTriggerSlope.Rising
		enableTrigger = True
		iqPowerEdgeTriggerLevelType = RFmxTdscdmaMXIQPowerEdgeTriggerLevelType.Relative
		minimumQuietTimeMode = RFmxTdscdmaMXTriggerMinimumQuietTimeMode.Auto

		' Sweep Time
		sweepTimeAuto = RFmxTdscdmaMXSemSweepTimeAuto.[True]
		sweepTimeInterval = 0.00066
		' seconds 

		averagingEnabled = RFmxTdscdmaMXSemAveragingEnabled.[False]
		averagingCount = 10
        averagingType = RFmxTdscdmaMXSemAveragingType.Rms
	End Sub


	Private Sub InitializeInstr()
		' Create a new RFmx Session 

		instrSession = New RFmxInstrMX(resourceName, "")
	End Sub

	Private Sub ConfigureTdscdma()
		' Get SpecAn signal 

		tdscdma = instrSession.GetTdscdmaSignalConfiguration()

		' Configure measurement 

        instrSession.ConfigureFrequencyReference("", frequencySource, frequencyReferenceFrequency)
		tdscdma.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
        tdscdma.ConfigureIQPowerEdgeTrigger("", iqPowerEdgeTriggerSource, iqPowerEdgeTriggerSlope, iqPowerEdgeTriggerLevel, _
            triggerDelay, minimumQuietTimeMode, minimumQuietTimeDuration, iqPowerEdgeTriggerLevelType, enableTrigger)

		tdscdma.SelectMeasurements("", RFmxTdscdmaMXMeasurementTypes.Sem, True)

		tdscdma.Sem.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)
		tdscdma.Sem.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval)


		tdscdma.Initiate("", "")
	End Sub

	Private Sub RetrieveResults()
        ' Retrieve results 

        tdscdma.Sem.Results.FetchLowerOffsetMarginArray("", timeout, lowerOffsetMeasurementStatus, lowerOffsetMargin, _
            lowerOffsetMarginFrequency, lowerOffsetMarginAbsolutePower, lowerOffsetMarginRelativePower)

        tdscdma.Sem.Results.FetchUpperOffsetMarginArray("", timeout, upperOffsetMeasurementStatus, upperOffsetMargin, _
            upperOffsetMarginFrequency, upperOffsetMarginAbsolutePower, upperOffsetMarginRelativePower)

		tdscdma.Sem.Results.FetchMeasurementStatus("", timeout, measurementStatus)
        tdscdma.Sem.Results.FetchCarrierAbsoluteIntegratedPower("", timeout, carrierAbsoluteIntegratedPower)
		tdscdma.Sem.Results.FetchSpectrum("", timeout, spectrum, absoluteMask, relativeMask)

	End Sub

	Private Sub PrintResults()
        Console.WriteLine("Measurement Status  :                                 {0}" & vbLf, measurementStatus)

		Console.WriteLine(vbLf & "--------------------------Carrier Measurements----------------------------" & vbLf)
        Console.WriteLine("Carrier Absolute Integrated Power (dBm):              {0}", carrierAbsoluteIntegratedPower)

        Console.WriteLine(vbLf & "--------------Offset Segment Measurements ---------------------------" & vbLf)
        For i As Integer = 0 To lowerOffsetMargin.Length - 1
            Console.WriteLine("Offset {0}" & vbLf, i)

            Console.WriteLine("Lower Offset : Margin (dB):                           {0}", lowerOffsetMargin(i))
            Console.WriteLine("Lower Offset : Margin Absolute Power (dBm):           {0}", lowerOffsetMarginAbsolutePower(i))
            Console.WriteLine("Lower Offset : Margin Relative Power (dB):            {0}", lowerOffsetMarginRelativePower(i))
            Console.WriteLine("Lower Offset : Margin Frequency (Hz):                 {0}", lowerOffsetMarginFrequency(i))

            Console.WriteLine("Lower Offset : Measurement Status :                   {0}" & vbLf, lowerOffsetMeasurementStatus(i))


            Console.WriteLine("Upper Offset : Margin (dB):                            {0}", upperOffsetMargin(i))
            Console.WriteLine("Upper Offset : Margin Absolute Power (dBm):            {0}", upperOffsetMarginAbsolutePower(i))
            Console.WriteLine("Upper Offset : Margin Relative Power (dB):             {0}", upperOffsetMarginRelativePower(i))
            Console.WriteLine("Upper Offset : Margin Frequency (Hz):                  {0}", upperOffsetMarginFrequency(i))

            Console.WriteLine("Upper Offset : Measurement Status :                    {0}" & vbLf, upperOffsetMeasurementStatus(i))
            Console.WriteLine("-----------------------------------------------------------------------" & vbLf)
        Next
	End Sub

	Private Sub CloseSession()
		Try
			If tdscdma IsNot Nothing Then
				tdscdma.Dispose()
				tdscdma = Nothing
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
