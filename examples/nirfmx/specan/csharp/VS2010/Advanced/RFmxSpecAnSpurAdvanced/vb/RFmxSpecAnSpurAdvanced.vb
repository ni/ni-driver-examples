'Steps:
'1. Open a new RFmx session
'2. Configure the basic instrument properties Clock Source, Clock Frequency
'3. Configure Selected Ports
'4. Configure the basic signal properties  (Reference Level and External Attenuation)
'5. Select Spur measurement and enable the traces
'6. Configure Spur Averaging
'7. Configure Spur FFT Window
'8. Configure Spur Trace Range Index
'9. Configure Spur Number of Ranges
'10. Configure Spur Range List properties:
'Start and Stop Frequency, Relative Attenuation, RBW Filter, Absolute Limit and Number of Spurs to Report using Selector String
'11. Initiate Measurement
'12. Fetch Range Status for all Ranges
'13. Use Number of Detected Spurs and Fetch Spur Measurement Results
'14. Fetch Spur Range Traces for all Ranges
'15. Fetch Measurement Status
'16. Close the RFmx Session

Imports NationalInstruments
Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.SpecAnMX

Class RFmxSpecAnSpurAdvanced
	Private instrSession As RFmxInstrMX
	Private specAn As RFmxSpecAnMX
	Private resourceName As [String], frequencyReferenceSource As [String]
   Private selectedPorts As String
	Private referenceLevel As Double, externalAttenuation As Double, centerFrequency As Double, frequencyReferenceFrequency As Double
	Private timeout As Double
	Private traceRangeIndex As Integer
	Private averagingCount As Integer
	Private totalSpur As Int32 = 0
	Private averagingEnabled As RFmxSpecAnMXSpurAveragingEnabled
	Private averagingType As RFmxSpecAnMXSpurAveragingType
	Private fftWindow As RFmxSpecAnMXSpurFftWindow
	Private measurementStatus As RFmxSpecAnMXSpurMeasurementStatus

	Const NumberOfRangeList As Integer = 1
	Const NumberOfSpursToReport As Integer = 10

	Private Structure RangeList

		Public enabled As RFmxSpecAnMXSpurRangeEnabled()
		Public startFrequency As Double()
		Public stopFrequency As Double()
		Public relativeAttenuation As Double()
		Public rbwFilterType As RFmxSpecAnMXSpurRbwFilterType()
		Public rbwFilterAutoBandwidth As RFmxSpecAnMXSpurRbwAutoBandwidth()
		Public vbwAuto As RFmxSpecAnMXSpurRangeVbwFilterAutoBandwidth()
		Public detectorType As RFmxSpecAnMXSpurRangeDetectorType()
		Public rbwFilterBandwidth As Double()
		Public absoluteLimitMode As RFmxSpecAnMXSpurAbsoluteLimitMode()
		Public absoluteStartLimit As Double()
		Public absoluteStopLimit As Double()
		Public peakThreshold As Double()
		Public peakExcursion As Double()
		Public numberOfSpursToReport As Integer()
		Public vbw As Double()
		Public vbwToRbwRatio As Double()
		Public detectorPoints As Integer()

		Public Sub New(numOfRangeList As Int32)
			enabled = New RFmxSpecAnMXSpurRangeEnabled(numOfRangeList - 1) {}
			startFrequency = New Double(numOfRangeList - 1) {}
			stopFrequency = New Double(numOfRangeList - 1) {}
			relativeAttenuation = New Double(numOfRangeList - 1) {}
			rbwFilterType = New RFmxSpecAnMXSpurRbwFilterType(numOfRangeList - 1) {}
			rbwFilterAutoBandwidth = New RFmxSpecAnMXSpurRbwAutoBandwidth(numOfRangeList - 1) {}
			rbwFilterBandwidth = New Double(numOfRangeList - 1) {}
			absoluteLimitMode = New RFmxSpecAnMXSpurAbsoluteLimitMode(numOfRangeList - 1) {}
			vbwAuto = New RFmxSpecAnMXSpurRangeVbwFilterAutoBandwidth(numOfRangeList - 1) {}
			detectorType = New RFmxSpecAnMXSpurRangeDetectorType(numOfRangeList - 1) {}
			absoluteStartLimit = New Double(numOfRangeList - 1) {}
			absoluteStopLimit = New Double(numOfRangeList - 1) {}
			peakThreshold = New Double(numOfRangeList - 1) {}
			peakExcursion = New Double(numOfRangeList - 1) {}
			numberOfSpursToReport = New Integer(numOfRangeList - 1) {}
			vbw = New Double(numOfRangeList - 1) {}
			vbwToRbwRatio = New Double(numOfRangeList - 1) {}
			detectorPoints = New Integer(numOfRangeList - 1) {}
		End Sub
	End Structure

	Private Structure SpurList

		Public frequency As Double()
		Public amplitude As Double()
		Public absoluteLimit As Double()
		Public margin As Double()
		Public rangeIndex As Int32()

		Public Sub New(numOfDetectedSpur As Int32)
			frequency = New Double(numOfDetectedSpur - 1) {}
			amplitude = New Double(numOfDetectedSpur - 1) {}
			absoluteLimit = New Double(numOfDetectedSpur - 1) {}
			margin = New Double(numOfDetectedSpur - 1) {}
			rangeIndex = New Int32(numOfDetectedSpur - 1) {}
		End Sub
	End Structure

	Private Structure RangeMeasurement

		Public measurementStatus As RFmxSpecAnMXSpurRangeStatus()
		Public detectedSpurs As Integer()
		Public Sub New(numOfRangeList As Int32)
			measurementStatus = New RFmxSpecAnMXSpurRangeStatus(numOfRangeList - 1) {}
			detectedSpurs = New Integer(numOfRangeList - 1) {}
		End Sub
	End Structure

	Private rangeInput As New RangeList(NumberOfRangeList)
    Private rangeMeas As New RangeMeasurement(NumberOfRangeList)
	Private spurMeasurement As SpurList


	Public Sub Run()
		Try
			InitializeVariables()
			InitializeInstr()
			ConfigureSpecAn()
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

      selectedPorts = ""
		centerFrequency = 1000000000.0
		' Hz 
		referenceLevel = 0.0
		' dBm 
		externalAttenuation = 0.0
		' dB 
		frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
		frequencyReferenceFrequency = 10000000.0
		' Hz 

		'Averaging
		averagingCount = 10
		averagingEnabled = RFmxSpecAnMXSpurAveragingEnabled.[False]
		averagingType = RFmxSpecAnMXSpurAveragingType.Rms

		'FFT window
		fftWindow = RFmxSpecAnMXSpurFftWindow.FlatTop

		timeout = 10.0
		' seconds 

		traceRangeIndex = 0

		'Range input
		For i As Integer = 0 To NumberOfRangeList - 1
			rangeInput.enabled(i) = RFmxSpecAnMXSpurRangeEnabled.[True]
			rangeInput.startFrequency(i) = 1000000000.0
			rangeInput.stopFrequency(i) = 1500000000.0
			rangeInput.relativeAttenuation(i) = 0.0
			rangeInput.rbwFilterType(i) = RFmxSpecAnMXSpurRbwFilterType.Gaussian
			rangeInput.rbwFilterAutoBandwidth(i) = RFmxSpecAnMXSpurRbwAutoBandwidth.[True]
			rangeInput.rbwFilterBandwidth(i) = 30000.0
			rangeInput.absoluteLimitMode(i) = RFmxSpecAnMXSpurAbsoluteLimitMode.Couple
			rangeInput.absoluteStartLimit(i) = -10.0
			rangeInput.absoluteStopLimit(i) = -10.0
			rangeInput.peakThreshold(i) = -200.0
			rangeInput.peakExcursion(i) = 0.0
			rangeInput.numberOfSpursToReport(i) = NumberOfSpursToReport
			rangeInput.vbwAuto(i) = RFmxSpecAnMXSpurRangeVbwFilterAutoBandwidth.[True]
			
			rangeInput.vbw(i) = 30000.0
			' Hz 
			rangeInput.vbwToRbwRatio(i) = 3
			rangeInput.detectorType(i) = RFmxSpecAnMXSpurRangeDetectorType.None
			rangeInput.detectorPoints(i) = 1001
		Next
	End Sub

	Private Sub InitializeInstr()
		' Create a new RFmx Session 

		instrSession = New RFmxInstrMX(resourceName, "")
	End Sub

	Private Sub ConfigureSpecAn()
		' Get SpecAn signal 

		specAn = instrSession.GetSpecAnSignalConfiguration()

		' Configure measurement 

		instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
      specAn.SetSelectedPorts("", selectedPorts)
		specAn.ConfigureFrequency("", centerFrequency)
		specAn.ConfigureReferenceLevel("", referenceLevel)
		specAn.ConfigureExternalAttenuation("", externalAttenuation)
		specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Spur, True)
		specAn.Spur.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)
		specAn.Spur.Configuration.ConfigureFftWindowType("", fftWindow)
		specAn.Spur.Configuration.ConfigureTraceRangeIndex("", traceRangeIndex)
		specAn.Spur.Configuration.ConfigureNumberOfRanges("", NumberOfRangeList)

      specAn.Spur.Configuration.ConfigureRangeFrequencyArray("", rangeInput.startFrequency, rangeInput.stopFrequency,
                                                             rangeInput.enabled)
		specAn.Spur.Configuration.ConfigureRangeRelativeAttenuationArray("", rangeInput.relativeAttenuation)
      specAn.Spur.Configuration.ConfigureRangeRbwArray("", rangeInput.rbwFilterAutoBandwidth, rangeInput.rbwFilterBandwidth,
                                                       rangeInput.rbwFilterType)
      specAn.Spur.Configuration.ConfigureRangeAbsoluteLimitArray("", rangeInput.absoluteLimitMode, rangeInput.absoluteStartLimit,
                                                                 rangeInput.absoluteStopLimit)
		specAn.Spur.Configuration.ConfigureRangeNumberOfSpursToReportArray("", rangeInput.numberOfSpursToReport)
		specAn.Spur.Configuration.ConfigureRangePeakCriteriaArray("", rangeInput.peakThreshold, rangeInput.peakExcursion)
		specAn.Spur.Configuration.ConfigureRangeDetectorArray("", rangeInput.detectorType, rangeInput.detectorPoints)
		specAn.Spur.Configuration.ConfigureRangeVbwFilterArray("", rangeInput.vbwAuto, rangeInput.vbw, rangeInput.vbwToRbwRatio)
		specAn.Initiate("", "")
	End Sub

	Private Sub RetrieveResults()
		' Retrieve results 

		Dim rangeString As String = ""
		Dim absoluteLimit As Spectrum(Of Single) = Nothing
		Dim spectrum As Spectrum(Of Single) = Nothing

        specAn.Spur.Results.FetchRangeStatusArray(rangeString, timeout, rangeMeas.measurementStatus, rangeMeas.detectedSpurs)
        For i As Integer = 0 To NumberOfRangeList - 1
            totalSpur += rangeMeas.detectedSpurs(i)
        Next
        spurMeasurement = New SpurList(totalSpur)
      specAn.Spur.Results.FetchAllSpurs("", timeout, spurMeasurement.frequency, spurMeasurement.amplitude, spurMeasurement.margin,
                                        spurMeasurement.absoluteLimit, spurMeasurement.rangeIndex)

        If traceRangeIndex = -1 Then
            traceRangeIndex = 0
        End If
        rangeString = RFmxSpecAnMX.BuildRangeString2("", traceRangeIndex)

        specAn.Spur.Results.FetchRangeAbsoluteLimitTrace(rangeString, timeout, absoluteLimit)

        specAn.Spur.Results.FetchRangeSpectrumTrace(rangeString, timeout, spectrum)

        specAn.Spur.Results.FetchMeasurementStatus("", timeout, measurementStatus)
    End Sub

    Private Sub PrintResults()
        Dim status As String = "Fail"

        Console.WriteLine("----------------Measurement-------------------" & vbLf)
        If measurementStatus = RFmxSpecAnMXSpurMeasurementStatus.Pass Then
            status = "Pass"
        End If
        Console.WriteLine("Measurement Status: {0}" & vbLf, status)

        Console.WriteLine("----------- Spur List-------------------" & vbLf)

        For i As Integer = 0 To totalSpur - 1
            status = "Fail"
            If rangeMeas.measurementStatus(spurMeasurement.rangeIndex(i)) = RFmxSpecAnMXSpurRangeStatus.Pass Then
                status = "Pass"
            End If
            Console.WriteLine("Spur                      {0}", i + 1)
         Console.WriteLine("Range Measurement Status  {0}", status)
            Console.WriteLine("Range Index               {0}", spurMeasurement.rangeIndex(i))
            Console.WriteLine("Frequency (Hz)            {0}", spurMeasurement.frequency(i))
            Console.WriteLine("Amplitude (dBm)           {0}", spurMeasurement.amplitude(i))
            Console.WriteLine("Absolute Limit (dBm)      {0}", spurMeasurement.absoluteLimit(i))
            Console.WriteLine("Margin (dB)               {0}", spurMeasurement.margin(i))

            Console.WriteLine("---------------------------------------")
        Next
    End Sub

	Private Sub CloseSession()
		Try
			If specAn IsNot Nothing Then
				specAn.Dispose()
				specAn = Nothing
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
