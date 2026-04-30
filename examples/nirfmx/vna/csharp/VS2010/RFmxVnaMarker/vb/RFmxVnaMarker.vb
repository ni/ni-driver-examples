'1. Open a new RFmx session.
'2. Configure Frequency Reference.
'3. Configure sweep settings: Frequency List, IF Bandwidth, and Power Level and Test Rx Attenuation with different port names.
'4. Configure Averaging.
'5. Select S-Parameter measurement.
'6. Configure S-Parameter and Format.
'7. Configure Magnitude Units & Phase Trace Type.
'8. Initiate the Measurement.
'9. Read X-Axis Values (aggregated frequency list).
'10. Fetch S-Parameter Y data.
'11. Configure Marker Type as Normal.
'12. Configure Marker Data Source as 'sparam0' to be used by the Marker.
'13. Configure Marker Peak Threshold.
'14. Configure Marker Peak Excursion. 
'15. Perform Peak Search on the configured data source.
'16. Fetch X value of the Marker.
'17. Fetch Y value of the Marker.
'18. Based on the user selection move the Marker to Next Peak, Next Left Peak and Next Right Peak  position and Fetch X and Y value of the Marker after moving the Marker to new position. Stop the Loop on Error or if the user has pressed the Stop button.
'19. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.VnaMX

Namespace NationalInstruments.Examples.RFmxVnaMarker
    Public Class RFmxVnaMarker
        Private instrSession As RFmxInstrMX
        Private vna As RFmxVnaMX
        Private resourceName As String

        Private startFrequency As Double
        Private stopFrequency As Double
        Private numberOfPoints As Integer
        Private port1PowerLevel As Double
        Private port2PowerLevel As Double
        Private port1TestReceiverAttenuation As Double
        Private port2TestReceiverAttenuation As Double
        Private IFBandwidth As Double

        Private sParamsSParameters As String
        Private sParamsFormats As RFmxVnaMXSParamsFormat

        Private averagingEnabled As RFmxVnaMXAveragingEnabled
        Private averagingCount As Integer

        Private portSelectorString As String

        Private timeout As Double

        Private frequencyListResult As Double()
        Private sParamsY1DataResult As Single()
        Private sParamsY2DataResult As Single()

        Private thresholdEnabled As RFmxVnaMXMarkerPeakSearchThresholdEnabled
        Private excursionEnabled As RFmxVnaMXMarkerPeakSearchExcursionEnabled
        Private threshold As Double, excursion As Double
        Private markerX As Double, markerY1 As Double, markerY2 As Double

        Public Sub Run()
            Try
                InitializeVariables()
                InitializeInstr()
                ConfigureVna()
                RetrieveResults()
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
            resourceName = "VNA"

            startFrequency = 1000000000.0
            ' (Hz) 
            stopFrequency = 26000000000.0
            ' (Hz) 
            numberOfPoints = 251
            port1PowerLevel = -10.0
            ' (dBm) 
            port2PowerLevel = -10.0
            ' (dBm) 
            port1TestReceiverAttenuation = 0.0
            ' (dB) 
            port2TestReceiverAttenuation = 0.0
            ' (dB) 
            IFBandwidth = 100000.0
            ' (Hz) 

            sParamsSParameters = "S11"
            sParamsFormats = RFmxVnaMXSParamsFormat.Magnitude

            averagingEnabled = RFmxVnaMXAveragingEnabled.[False]
            averagingCount = 10

            'Peak threshold
            thresholdEnabled = RFmxVnaMXMarkerPeakSearchThresholdEnabled.[False]
            threshold = -100

            'Peak Excursion
            excursionEnabled = RFmxVnaMXMarkerPeakSearchExcursionEnabled.[False]
            excursion = 3

            timeout = 10.0
            'seconds 
        End Sub

        Private Sub InitializeInstr()
            instrSession = New RFmxInstrMX(resourceName, "")
        End Sub

        Private Sub ConfigureVna()
            vna = instrSession.GetVnaSignalConfiguration()
            ' Create a new RFmx Session 
            instrSession.ConfigureFrequencyReference("", RFmxInstrMXConstants.PxiClock, 100000000.0)
            vna.SetSweepType("", RFmxVnaMXSweepType.Linear)
            vna.SetStartFrequency("", startFrequency)
            vna.SetStopFrequency("", stopFrequency)
            vna.SetNumberOfPoints("", numberOfPoints)
            vna.SetIFBandwidth("", IFBandwidth)
            portSelectorString = RFmxVnaMX.BuildPortString("", "port1")
            vna.SetPowerLevel(portSelectorString, port1PowerLevel)
            vna.SetTestReceiverAttenuation(portSelectorString, port1TestReceiverAttenuation)
            portSelectorString = RFmxVnaMX.BuildPortString("", "port2")
            vna.SetPowerLevel(portSelectorString, port2PowerLevel)
            vna.SetTestReceiverAttenuation(portSelectorString, port2TestReceiverAttenuation)
            vna.SetAveragingEnabled("", averagingEnabled)
            vna.SetAveragingCount("", averagingCount)
            vna.SelectMeasurements("", RFmxVnaMXMeasurementTypes.SParams, False)

            vna.SParams.Configuration.ConfigureSParameter("", sParamsSParameters)
            vna.SParams.Configuration.SetFormat("", sParamsFormats)

            vna.SParams.Configuration.SetMagnitudeUnits("", RFmxVnaMXSParamsMagnitudeUnits.dB)
            vna.SParams.Configuration.SetPhaseTraceType("", RFmxVnaMXSParamsPhaseTraceType.Wrapped)
            vna.Initiate("", "")
        End Sub

        Private Sub RetrieveResults()

            vna.SParams.Results.FetchXData("", timeout, frequencyListResult)
            vna.SParams.Results.FetchYData("", timeout, sParamsY1DataResult, sParamsY2DataResult)

            'Configure Marker settings    
            vna.Marker.Configuration.ConfigureType("", RFmxVnaMXMarkerType.Normal)
            vna.Marker.Configuration.ConfigureDataSource("", "sparam0")
            vna.Marker.Configuration.ConfigurePeakSearchThreshold("", thresholdEnabled, threshold)
            vna.Marker.Configuration.ConfigurePeakSearchExcursion("", excursionEnabled, excursion)

            vna.Marker.Results.MarkerSearch("", RFmxVnaMXMarkerSearchMode.Peak)

            'Fetch Marker Results
            vna.Marker.Results.FetchX("", markerX)
            vna.Marker.Results.FetchY("", markerY1, markerY2)
        End Sub

        Private Sub DisplayResults()
            Dim ch As ConsoleKeyInfo
            Dim exitMenu As Boolean = False
            Dim donotFetch As Boolean
            Dim peakToFetch As RFmxVnaMXMarkerSearchMode = RFmxVnaMXMarkerSearchMode.NextPeak

            DisplayMarkerData(markerX, markerY1, markerY2)
            DisplayMarkerMenu()

            While True
                ch = Console.ReadKey(True)
                donotFetch = False
                Select Case ch.KeyChar
                    Case "l"c, "L"c
                        peakToFetch = RFmxVnaMXMarkerSearchMode.NextLeftPeak
                        Exit Select

                    Case "r"c, "R"c
                        peakToFetch = RFmxVnaMXMarkerSearchMode.NextRightPeak
                        Exit Select

                    Case "h"c, "H"c
                        peakToFetch = RFmxVnaMXMarkerSearchMode.NextPeak
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
                    vna.Marker.Results.MarkerSearch("", peakToFetch)
                    vna.Marker.Results.FetchX("", markerX)
                    vna.Marker.Results.FetchY("", markerY1, markerY2)
                    DisplayMarkerData(markerX, markerY1, markerY2)
                End If

                If exitMenu Then
                    Console.WriteLine("Exiting menu.. ")
                    Exit While
                End If
            End While
        End Sub
        Private Sub DisplayMarkerMenu()
            Console.WriteLine("To read different peaks, use the following keys" & vbLf)
            Console.WriteLine("l/L - Next Left Peak" & vbLf)
            Console.WriteLine("r/R - Next Right Peak" & vbLf)
            Console.WriteLine("h/H - Next Peak" & vbLf)
            Console.WriteLine("s/S - Stop and Exit menu" & vbLf)
        End Sub


        Private Sub DisplayMarkerData(markerX As Double, markerY1 As Double, markerY2 As Double)
            Console.WriteLine("---------------------------------------------------" & vbLf)
            Console.WriteLine("Marker X", markerX)
            Console.WriteLine("Marker Y1", markerY1)
            Console.WriteLine("Marker Y2", markerY2)
            Console.WriteLine("----------------------------------------------------" & vbLf)
        End Sub

        Private Sub CloseSession()
            If vna IsNot Nothing Then
                vna.Dispose()
                vna = Nothing
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
