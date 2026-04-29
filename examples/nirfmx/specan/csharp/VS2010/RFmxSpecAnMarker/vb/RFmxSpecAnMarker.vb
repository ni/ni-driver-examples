'Steps:
'1. Open a new RFmx session
'2. Configure the instrument properties Clock Source and Clock Frequency
'3. Configure Selected Ports
'4. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
'5. Select Spectrum measurement
'6. Configure Spectrum RBW
'7. Configure Spectrum Span
'8. Configure Spectrum Averaging
'9.  Initiate Measurment
'10. Fetch Spectrum Trace
'11. Configure Marker Peak Threshold
'12. Configure Marker Type as Normal
'13. Configure "Spectrum" as the trace to be used by the Marker
'14. Use Marker Peak Search to detect the Number of Peaks in the Spectrum
'15. Fetch XY Location of the Marker
'16. Based on the user selection move the Marker to Next Highest, Next Left and Next Right position
'and Fetch XY Location of the Marker after moving the Marker to new position
'Stop the Loop on Error or if the user has pressed the Stop button
'17. Close the RFmx Session

Imports NationalInstruments
Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.SpecAnMX

Class RFmxSpecAnMarker
   Private instrSession As RFmxInstrMX
   Private specAn As RFmxSpecAnMX
   Private resourceName As String, frequencySource As String
   Private selectedPorts As String
   Private centerFrequency As Double, referenceLevel As Double, externalAttenuation As Double,
           frequency As Double, span As Double, rbw As Double, timeout As Double
   Private rbwFilterType As RFmxSpecAnMXSpectrumRbwFilterType
   Private rbwAutoBandwidth As RFmxSpecAnMXSpectrumRbwAutoBandwidth
   Private averagingEnabled As RFmxSpecAnMXSpectrumAveragingEnabled
   Private averagingType As RFmxSpecAnMXSpectrumAveragingType
   Private averagingCount As Integer, threshold As Integer, numberOfPeaks As Integer
   Private thresholdEnabled As RFmxSpecAnMXMarkerThresholdEnabled
   Private excursionEnabled As RFmxSpecAnMXMarkerPeakExcursionEnabled
   Private excursion As Double
   Private markerType As RFmxSpecAnMXMarkerType
   Private markerTrace As RFmxSpecAnMXMarkerTrace
   Const NumberOfMarkers As Integer = 1

   Private markerXLocation As Double, markerYLocation As Double
   Private nextPeakFound As Boolean
   Private spectrum As Spectrum(Of Single)

   Public Sub Run()
      Try
         InitializeVariables()
         InitializeInstr()
         ConfigureSpecAn()
         RetrieveResults()
         DisplayResults()
      Catch ex As Exception
         DisplayError(ex.Message)
      Finally
         ' Close session 

         CloseSession()
         Console.WriteLine("Press any key to exit.....")
         Console.ReadKey()
      End Try
   End Sub

   Private Sub InitializeVariables()
      ' Intialize input variables 

      resourceName = "RFSA"
      selectedPorts = ""
      centerFrequency = 1000000000.0 ' Hz 
      referenceLevel = 0.0 ' dBm 
      externalAttenuation = 0.0 ' dB 

      frequencySource = RFmxInstrMXConstants.OnboardClock
      frequency = 10000000.0 ' Hz 
      span = 1000000.0 ' Hz 

      rbwFilterType = RFmxSpecAnMXSpectrumRbwFilterType.Gaussian
      rbwAutoBandwidth = RFmxSpecAnMXSpectrumRbwAutoBandwidth.[False]
      rbw = 10000.0 ' Hz 

      'Averaging 
      averagingEnabled = RFmxSpecAnMXSpectrumAveragingEnabled.[False]
      averagingCount = 10
      averagingType = RFmxSpecAnMXSpectrumAveragingType.Rms

      markerType = RFmxSpecAnMXMarkerType.Normal
      markerTrace = RFmxSpecAnMXMarkerTrace.Spectrum

      'Peak threshold
      thresholdEnabled = RFmxSpecAnMXMarkerThresholdEnabled.[False]
      threshold = -90

      'Peak Excursion
      excursionEnabled = RFmxSpecAnMXMarkerPeakExcursionEnabled.[False]
      excursion = 6
      ' Rel Units 

      timeout = 10
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
      specAn.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)

      specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Spectrum, True)
      specAn.Spectrum.Configuration.ConfigureRbwFilter("", rbwAutoBandwidth, rbw, rbwFilterType)
      specAn.Spectrum.Configuration.ConfigureSpan("", span)
      specAn.Spectrum.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount,
                                                       averagingType)
      specAn.Initiate("", "")
   End Sub

   Private Sub RetrieveResults()
      ' Retrieve Results 

      'Fetch Spectrum data
      specAn.Spectrum.Results.FetchSpectrum("", timeout, spectrum)

      'Configure Marker settings 
      specAn.Marker.Configuration.ConfigureThreshold("", thresholdEnabled, threshold)
      specAn.Marker.Configuration.ConfigurePeakExcursion("", excursionEnabled, excursion)
      specAn.Marker.Configuration.ConfigureType("marker0", markerType)
      specAn.Marker.Configuration.ConfigureTrace("marker0", markerTrace)

      specAn.Marker.Results.PeakSearch("marker0", numberOfPeaks)
      specAn.Marker.Results.FetchXY("marker0", markerXLocation, markerYLocation)
   End Sub

   Private Sub DisplayResults()
      Dim ch As ConsoleKeyInfo
      Dim exitMenu As Boolean = False
      Dim donotFetch As Boolean
      Dim peakToFetch As RFmxSpecAnMXMarkerNextPeak = RFmxSpecAnMXMarkerNextPeak.NextHighest

      DisplayMarkerData(numberOfPeaks, nextPeakFound, markerXLocation, markerYLocation)
      DisplayMarkerMenu()

      While True
         ch = Console.ReadKey(True)
         donotFetch = False
         Select Case ch.KeyChar
            Case "l"c, "L"c
               peakToFetch = RFmxSpecAnMXMarkerNextPeak.NextLeft
               Exit Select

            Case "r"c, "R"c
               peakToFetch = RFmxSpecAnMXMarkerNextPeak.NextRight
               Exit Select

            Case "h"c, "H"c
               peakToFetch = RFmxSpecAnMXMarkerNextPeak.NextHighest
               Exit Select

            Case "s"c, "S"c
               exitMenu = True
               donotFetch = True
               Exit Select
            Case Else
               donotFetch = True
               DisplayMarkerMenu()
               Exit Select
         End Select

         If Not donotFetch Then
            'Fetch marker data
            specAn.Marker.Results.NextPeak("marker0", peakToFetch, nextPeakFound)
            specAn.Marker.Results.FetchXY("marker0", markerXLocation, markerYLocation)
            DisplayMarkerData(numberOfPeaks, nextPeakFound, markerXLocation, markerYLocation)
         End If

         If exitMenu Then
            Console.WriteLine("Exiting menu.. ")
            Exit While
         End If
      End While
   End Sub

   Private Sub DisplayMarkerMenu()
      Console.WriteLine("To read different peaks, use the following keys" & vbLf)
      Console.WriteLine("l/L - Next Left" & vbLf)
      Console.WriteLine("r/R - Next Right" & vbLf)
      Console.WriteLine("h/H - Next Highest" & vbLf)
      Console.WriteLine("s/S - Stop and Exit menu" & vbLf)
   End Sub

   Private Sub DisplayMarkerData(ByVal numberOfPeaks As Integer, ByVal peakFound As Boolean,
                                 ByVal markerXLocation As Double, ByVal markerYLocation As Double)
      Console.WriteLine("---------------------------------------------------" & vbLf)
      Console.WriteLine("Number of peaks          {0}", numberOfPeaks)
      Console.WriteLine("Next Peak found?         {0}", peakFound)
      Console.WriteLine("Marker X Location (Hz)   {0}", markerXLocation)
      Console.WriteLine("Marker Y Location (dBm)  {0}", markerYLocation)
      Console.WriteLine("----------------------------------------------------" & vbLf)
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
         DisplayError(ex.Message)
      End Try
   End Sub

   Private Sub DisplayError(message As String)
      Console.WriteLine("ERROR:" & vbLf & message)
   End Sub
End Class
