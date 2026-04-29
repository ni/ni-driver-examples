'Steps:
'1. Open a new RFmx session
'2. Configure the basic instrument properties Clock Source, Clock Frequency
'3. Configure Selected Ports
'4. Configure the basic signal properties  (Reference Level and External Attenuation)
'5. Select Spur measurement and enable the traces
'6. Configure Spur Averaging
'7. Configure Spur Number of Ranges
'8. Configure Spur Range Start and Stop frequency
'9. Configure Spur Range RBW filter
'10. Configure Spur Range Limit Mode, Absolute Start and Stop Limit
'11. Configure Spur Range Number of Spurs to Report
'12. Configure Spur Trace Range Index
'13. Initiate Measurement
'14. Fetch Spur Measurements, Traces and Status
'15. Close the RFmx Session

Imports NationalInstruments
Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.SpecAnMX

Class RFmxSpecAnSpur
   Private instrSession As RFmxInstrMX
   Private specAn As RFmxSpecAnMX
   Private resourceName As [String], frequencySource As [String]
   Private selectedPorts As String
   Private referenceLevel As Double, externalAttenuation As Double, frequency As Double
   Private timeout As Double
   Private averagingCount As Integer
   Private averagingEnabled As RFmxSpecAnMXSpurAveragingEnabled
   Private averagingType As RFmxSpecAnMXSpurAveragingType
   Private measurementStatus As RFmxSpecAnMXSpurMeasurementStatus
   Private traceRangeIndex As Integer

   Const NumberOfRanges As Integer = 1
   Const NumberOfSpursToReport As Integer = 10

   'Input array values.
   Private rangeEnabled As RFmxSpecAnMXSpurRangeEnabled() = New RFmxSpecAnMXSpurRangeEnabled(NumberOfRanges - 1) {}
   Private startFrequency As Double() = New Double(NumberOfRanges - 1) {}
   Private stopFrequency As Double() = New Double(NumberOfRanges - 1) {}
   Private rbwFilterType As RFmxSpecAnMXSpurRbwFilterType() = New RFmxSpecAnMXSpurRbwFilterType(NumberOfRanges - 1) {}
   Private rbwFilterAutoBandwidth As RFmxSpecAnMXSpurRbwAutoBandwidth() = New RFmxSpecAnMXSpurRbwAutoBandwidth(NumberOfRanges - 1) {}
   Private rbwFilterBandwidth As Double() = New Double(NumberOfRanges - 1) {}
   Private absoluteLimitMode As RFmxSpecAnMXSpurAbsoluteLimitMode() = New RFmxSpecAnMXSpurAbsoluteLimitMode(NumberOfRanges - 1) {}
   Private vbwAuto As RFmxSpecAnMXSpurRangeVbwFilterAutoBandwidth() = New RFmxSpecAnMXSpurRangeVbwFilterAutoBandwidth(NumberOfRanges - 1) {}
   Private detectorType As RFmxSpecAnMXSpurRangeDetectorType() = New RFmxSpecAnMXSpurRangeDetectorType(NumberOfRanges - 1) {}
   Private absoluteLimitStart As Double() = New Double(NumberOfRanges - 1) {}
   Private absoluteLimitStop As Double() = New Double(NumberOfRanges - 1) {}
   Private peakThreshold As Double() = New Double(NumberOfRanges - 1) {}
   Private peakExcursion As Double() = New Double(NumberOfRanges - 1) {}
   Private numOfSpursToReport As Integer() = New Integer(NumberOfRanges - 1) {}
   Private vbw As Double() = New Double(NumberOfRanges - 1) {}
   Private vbwToRbwRatio As Double() = New Double(NumberOfRanges - 1) {}
   Private detectorPoints As Integer() = New Integer(NumberOfRanges - 1) {}

   'Output values
   Private numberOfDetectedSpurs As Integer()
   Private totalSpur As Integer = 0
   Private rangeStatus As RFmxSpecAnMXSpurRangeStatus()

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
      referenceLevel = 0.0
      ' dBm 
      externalAttenuation = 0.0
      ' dB 
      frequencySource = RFmxInstrMXConstants.OnboardClock
      frequency = 10000000.0
      ' Hz 

      averagingCount = 10
      averagingEnabled = RFmxSpecAnMXSpurAveragingEnabled.[False]
      averagingType = RFmxSpecAnMXSpurAveragingType.Rms

      ' Rangelist       
      For i As Integer = 0 To NumberOfRanges - 1
         rangeEnabled(i) = RFmxSpecAnMXSpurRangeEnabled.[True]
         startFrequency(i) = 1000000000.0
         ' Hz 
         stopFrequency(i) = 1500000000.0
         ' Hz 

         rbwFilterType(i) = RFmxSpecAnMXSpurRbwFilterType.Gaussian
         rbwFilterAutoBandwidth(i) = RFmxSpecAnMXSpurRbwAutoBandwidth.[True]
         rbwFilterBandwidth(i) = 30000.0
         ' Hz 

         vbwAuto(i) = RFmxSpecAnMXSpurRangeVbwFilterAutoBandwidth.[True]
         vbw(i) = 30000.0
         ' Hz 
         vbwToRbwRatio(i) = 3

         detectorType(i) = RFmxSpecAnMXSpurRangeDetectorType.None
         detectorPoints(i) = 1001

         absoluteLimitMode(i) = RFmxSpecAnMXSpurAbsoluteLimitMode.Couple
         absoluteLimitStart(i) = -10.0
         absoluteLimitStop(i) = -10.0

         peakThreshold(i) = -200
         peakExcursion(i) = 0.0

         numOfSpursToReport(i) = NumberOfSpursToReport
      Next
      traceRangeIndex = 0

      timeout = 10.0
      ' seconds 
   End Sub

   Private Sub InitializeInstr()
      ' Create a new RFmx Session 

      instrSession = New RFmxInstrMX(resourceName, "")
   End Sub

   Private Sub ConfigureSpecAn()
      ' Get SpecAn signal 

      specAn = instrSession.GetSpecAnSignalConfiguration()

      ' Configure measurement 

      instrSession.ConfigureFrequencyReference("", frequencySource, frequency)
      specAn.SetSelectedPorts("", selectedPorts)
      specAn.ConfigureReferenceLevel("", referenceLevel)
      specAn.ConfigureExternalAttenuation("", externalAttenuation)

      specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Spur, True)
      specAn.Spur.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)
      specAn.Spur.Configuration.ConfigureNumberOfRanges("", NumberOfRanges)
      specAn.Spur.Configuration.ConfigureRangeFrequencyArray("", startFrequency, stopFrequency, rangeEnabled)
      specAn.Spur.Configuration.ConfigureRangeRbwArray("", rbwFilterAutoBandwidth, rbwFilterBandwidth, rbwFilterType)
      specAn.Spur.Configuration.ConfigureRangeAbsoluteLimitArray("", absoluteLimitMode, absoluteLimitStart, absoluteLimitStop)
      specAn.Spur.Configuration.ConfigureRangeNumberOfSpursToReportArray("", numOfSpursToReport)
      specAn.Spur.Configuration.ConfigureRangePeakCriteriaArray("", peakThreshold, peakExcursion)
      specAn.Spur.Configuration.ConfigureRangeDetectorArray("", detectorType, detectorPoints)
      specAn.Spur.Configuration.ConfigureRangeVbwFilterArray("", vbwAuto, vbw, vbwToRbwRatio)
      specAn.Spur.Configuration.ConfigureTraceRangeIndex("", traceRangeIndex)

      specAn.Initiate("", "")
   End Sub

   Private Sub RetrieveResults()
      ' Retrieve results 

      Dim absoluteLimitTrace As Spectrum(Of Single) = Nothing
      Dim spectrumTrace As Spectrum(Of Single) = Nothing
      Dim rangeString As String = ""

      specAn.Spur.Results.FetchRangeStatusArray("", timeout, rangeStatus, numberOfDetectedSpurs)

      For i As Integer = 0 To NumberOfRanges - 1
         totalSpur += numberOfDetectedSpurs(i)
      Next
      spurMeasurement = New SpurList(totalSpur)
      specAn.Spur.Results.FetchAllSpurs("", timeout, spurMeasurement.frequency, spurMeasurement.amplitude, spurMeasurement.margin, spurMeasurement.absoluteLimit,
         spurMeasurement.rangeIndex)

      Dim rangeNumber As Integer = If((traceRangeIndex = -1), 0, traceRangeIndex)
      rangeString = RFmxSpecAnMX.BuildRangeString2("", rangeNumber)

      specAn.Spur.Results.FetchRangeAbsoluteLimitTrace(rangeString, timeout, absoluteLimitTrace)

      specAn.Spur.Results.FetchRangeSpectrumTrace(rangeString, timeout, spectrumTrace)

      specAn.Spur.Results.FetchMeasurementStatus("", timeout, measurementStatus)
   End Sub

   Private Sub PrintResults()
      Dim status As String = "Fail"

      Console.WriteLine("------------------Measurement------------------" & vbLf)
      If measurementStatus = RFmxSpecAnMXSpurMeasurementStatus.Pass Then
         status = "Pass"
      End If
      Console.WriteLine("Measurement Status   : {0}" & vbLf, status)

      Console.WriteLine(vbLf & "Spur List:" & vbLf)
      For i As Integer = 0 To NumberOfSpursToReport - 1
         Console.WriteLine("Spur {0}", i)
         Console.WriteLine("Range Index          : {0}", spurMeasurement.rangeIndex(i))
         Console.WriteLine("Frequency (Hz)       : {0}", spurMeasurement.frequency(i))
         Console.WriteLine("Amplitude (dBm)      : {0}", spurMeasurement.amplitude(i))
         Console.WriteLine("Abosulte Limit (dBm) : {0}", spurMeasurement.absoluteLimit(i))
         Console.WriteLine("Margin (dB)          : {0}", spurMeasurement.margin(i))
         Console.WriteLine("--------------------------------------------------------------" & vbLf)
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

   Private Sub DisplayError(ex As Exception)
      Console.WriteLine("ERROR:" & vbLf & Convert.ToString(ex.[GetType]()) & ": " & ex.Message)
   End Sub

End Class
