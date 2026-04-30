'Steps:
'1.Open a session to the NI-RFSG.
'2. Configure RFSG generation mode to Script. This allows for writing a script that determines the behavior of the generation.
'3. Configure the Reference Clock Source and Frequency.
'4. Configure Frequency Setting Units and Frequency Settings.
'5. Configure following properties:
'  - External Gain
'  - Selected Ports
'6. Get the terminal name for marker0. Use this as the source to configure RFSG List to advance upon receipt of a marker event
'7. Read waveform from file and download waveform from file to RFSG.
'8. Retrieve the waveform sample rate and waveform PAPR. Add the retrieved PAPR to the RFSA reference level while configuring to every list step.
'9. Write script to generate a waveform. This script is programmed to continuously generate a waveform of length equal to the RFmx list step duration 
'   and generate marker0 at the end of list step acquisition.
'10. Create a Configuration List. Pass Frequency and Power Level in the Configuration List Properties parameter to be able to configure Frequency and 
'    Power Level in each step that is created. The Set As Active List parameter in this VI defaults to true, this will set the Active Configuration List   
'    property to the name of the created configuration list. Once the Active Configuration List is set, use a property node to access Power Level will    
'    modify the property for this configuration list.
'11. Create a Configuration List Step. The Set As Active Step parameter in this VI defaults to true, this will set the Active Configuration List Step property    
'    to the created configuration list step index. Once the Active Configuration List Step is set, using a property node to access Frequency or Power 
'    Level will modify the property for this configuration list step, in the configuration list indicated by the Active Configuration List property.
'12. Configure the Frequency and Power Level for the Active Configuration List Step in the Active Configuration List.
'13. Open a new RFmx Session.
'14. Configure the Frequency Reference properties (Clock Source and Clock Frequency).
'15 - 17. Configure Selected Ports, External Attenuation and Trigger Parameters
'18. Configure Segment Parameters like Number of Segments, Frequency, Reference Level, Measurement Length, Trigger Type, Segment Length and RBW Filter.
'19. Select PowerList measurement.
'20. Initiate PowerList measurement.
'21. Initiate signal generation.
'22. Wait for Acquisition to complete.
'23. Stop signal generation.
'24. Delete the Configuration List to avoid committing the Configuration List to the hardware with subsequent calls to Commit. 
'    Deleting the list will reset the Active Configuration List.
'25. Fetch PowerList measurement results for all segments.
'26. Close the RFmx Session.
'27. Close the RFSG session. 
'    It is recommended to clear the waveform before closing RFSG session.
'32. Delete the Configuration List to avoid committing the Configuration List to the hardware with subsequent calls to Commit.
'    Deleting the list will reset the Active Configuration List. 
'33. Delete RFmx SpecAn List
'34. Close the RFmx Session.
'35. Close the RFSG session.
'    It is recommended to clear the waveform before closing RFSG session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.SpecAnMX
Imports NationalInstruments.ModularInstruments.NIRfsg
Imports NationalInstruments.ModularInstruments.NIRfsgPlayback
Imports System.Threading

Namespace NationalInstruments.Examples.RFmxSpecAnPowerListWithTimeBasedSynchronization
    Public Class RFmxSpecAnPowerListWithTimeBasedSynchronization
        Private instrSession As RFmxInstrMX
        Private rfsgSession As NIRfsg
        Private instrumentHandle As IntPtr

        Private rfsgResourceName As String
        Private rfsgSelectedPorts As String
        Private waveformFilePath As String
        Private waveformName As String

        Private rfsgExternalAttenuation As Double

        Private rfsgFrequencyReferenceSource As RfsgFrequencyReferenceSource
        Private rfsgFrequency As Double

        Private rfsaResourceName As String
        Private rfsaSelectedPorts As String
        Private rfsaExternalAttenuation As Double

        Private rfsaFrequencyReferenceSource As String
        Private rfsaFrequency As Double

        Private segment0IQPowerEdgeLevel As Double
        Private segment0MinimumQuietTime As Double
        Private triggerDelay As Double

        Private numberOfSegments As Integer

        Private StartCenterFrequency As Double
        Private StopCenterFrequency As Double

        Private startReferenceLevel As Double
        Private stopReferenceLevel As Double

        Private measurementLengthArray As Double()
        Private referenceLevelArray As Double()
        Private segmentTriggerArray As Integer()
        Private segmentLengthArray As Double()
        Private rbwArray As Double()
        Private rbwFilterTypeArray As Integer()
        Private rbwRrcAlphaArray As Double()



        Private measurementLength As Double
        Private segmentLength As Double
        Private rbw As Double, rrcAlpha As Double
        Private rbwFilterType As RFmxSpecAnMXTxpRbwFilterType
        Private specAn As RFmxSpecAnMX
        Private referenceLevelRampPattern As Double()
        Private centerFrequencyRampPattern As Double()
        Private timeout As Double
        Private script As String
        Private markerEventTerminalName As String
        Private sampleRate As Double
        Private numberOfSamples As Integer
        Private markerLocation As Integer
        Private papr As Double
        Enum segmentTriggerType
            None
            DigitalEdge
            IQPowerEdge
        End Enum


        Private meanAbsolutePower As Double()
        Private maximumPower As Double()
        Private minimumPower As Double()


        Public Sub Run()
            Try
                InitializeVariables()
                ConfigureRfsg()
                ConfigureRFmx()
                RetrieveResults()
                PrintResults()
            Catch ex As Exception
                DisplayError(ex)
            Finally
                CloseSession()
                Console.WriteLine(vbLf & "Press any key to exit")
                Console.ReadKey()
            End Try
        End Sub

        Private Sub InitializeVariables()
            rfsgResourceName = "RFSG"
            rfsgSelectedPorts = ""
            waveformFilePath = "WCDMA_Uplink_DPCH_Waveform.tdms"
            waveformName = "Wfm"

            rfsgExternalAttenuation = 0.0
            ' (dB) 

            rfsgFrequencyReferenceSource = RfsgFrequencyReferenceSource.OnboardClock
            rfsgFrequency = 10000000.0
            ' (Hz) 

            rfsaResourceName = "RFSA"
            rfsaSelectedPorts = ""
            rfsaExternalAttenuation = 0.0
            ' (dB) 

            rfsaFrequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
            rfsaFrequency = 10000000.0
            ' (Hz) 

            segment0IQPowerEdgeLevel = -10
            ' (dB) 
            segment0MinimumQuietTime = 0.0
            ' (s) 
            triggerDelay = 0.0
            ' (s) 

            numberOfSegments = 10

            StartCenterFrequency = 1000000000.0
            ' (s) 
            StopCenterFrequency = 2000000000.0
            ' (s) 

            startReferenceLevel = -20.0
            ' (dBm) 
            stopReferenceLevel = 0.0
            ' (dBm) 

            measurementLength = 0.001
            ' (Hz) 
            segmentLength = 0.0015
            ' (Hz) 
            rbw = 5000000.0
            ' (Hz) 
            rbwFilterType = RFmxSpecAnMXTxpRbwFilterType.Flat
            rrcAlpha = 0.01
            measurementLengthArray = New Double(numberOfSegments - 1) {}
            referenceLevelArray = New Double(numberOfSegments - 1) {}
            segmentTriggerArray = New Integer(numberOfSegments - 1) {}
            segmentLengthArray = New Double(numberOfSegments - 1) {}
            rbwArray = New Double(numberOfSegments - 1) {}
            rbwFilterTypeArray = New Integer(numberOfSegments - 1) {}
            rbwRrcAlphaArray = New Double(numberOfSegments - 1) {}

            centerFrequencyRampPattern = New Double(numberOfSegments - 1) {}
            referenceLevelRampPattern = New Double(numberOfSegments - 1) {}
            LinearRampPatternReverse(StartCenterFrequency, StopCenterFrequency, numberOfSegments, False, centerFrequencyRampPattern)
            LinearRampPattern(startReferenceLevel, stopReferenceLevel, numberOfSegments, False, referenceLevelRampPattern)

            timeout = 10.0
            ' (s) 
        End Sub

        Private Sub ConfigureRfsg()
            rfsgSession = New NIRfsg(rfsgResourceName, True, False)
            rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.Script
            rfsgSession.FrequencyReference.Configure(rfsgFrequencyReferenceSource, rfsgFrequency)
            rfsgSession.RF.Advanced.FrequencySettlingUnits = RfsgRFFrequencySettlingUnits.TimeAfterIO
            rfsgSession.RF.Advanced.FrequencySettlingTime = 0.001
            rfsgSession.RF.ExternalGain = -1 * rfsgExternalAttenuation
            rfsgSession.SignalPath.SelectedPorts = rfsgSelectedPorts

            markerEventTerminalName = rfsgSession.DeviceEvents.MarkerEvents(0).TerminalName
            rfsgSession.Triggers.ConfigurationListStepTrigger.DigitalEdge.Configure(markerEventTerminalName, RfsgTriggerEdge.RisingEdge)

            instrumentHandle = rfsgSession.GetInstrumentHandle().DangerousGetHandle()
            rfsgSession.Arb.ReadAndDownloadWaveformFromFileTdms(waveformName, waveformFilePath, 0)
            sampleRate = rfsgSession.Arb.Waveforms(waveformName).IQRate
            papr = rfsgSession.Arb.Waveforms(waveformName).Papr
            numberOfSamples = CInt(Math.Truncate(sampleRate * segmentLength))
            markerLocation = CInt(Math.Truncate(sampleRate * measurementLength))
            script = String.Format("script GenerateWaveform" & vbLf & "  repeat forever" & vbLf & "    generate {0} subset(0, {1}) marker0({2})" & vbLf & "  end repeat" & vbLf & "end script", waveformName, numberOfSamples, markerLocation)
            rfsgSession.Arb.Scripting.WriteScript(script)

            Dim properties As RfsgConfigurationListProperties() = New RfsgConfigurationListProperties(1) {RfsgConfigurationListProperties.Frequency, RfsgConfigurationListProperties.PowerLevel}
            rfsgSession.BasicConfigurationList.CreateConfigurationList("PowerList", properties, True)
            For i As Integer = 0 To numberOfSegments - 1
                rfsgSession.BasicConfigurationList.CreateStep(True)
                rfsgSession.RF.Frequency = centerFrequencyRampPattern(i)
                rfsgSession.RF.PowerLevel = referenceLevelRampPattern(i)
            Next
        End Sub

        Private Sub ConfigureRFmx()
            instrSession = New RFmxInstrMX(rfsaResourceName, "")
            instrSession.ConfigureFrequencyReference("", rfsaFrequencyReferenceSource, rfsaFrequency)
            specAn = instrSession.GetSpecAnSignalConfiguration()
            specAn.SetSelectedPorts("", rfsaSelectedPorts)
            specAn.ConfigureExternalAttenuation("", rfsaExternalAttenuation)
            specAn.SetDigitalEdgeTriggerSource("", RFmxSpecAnMXConstants.TimerEvent)
            specAn.SetIQPowerEdgeTriggerSource("", "0")
            specAn.SetIQPowerEdgeTriggerLevelType("", RFmxSpecAnMXIQPowerEdgeTriggerLevelType.Relative)
            specAn.SetIQPowerEdgeTriggerLevel("", segment0IQPowerEdgeLevel)
            specAn.SetTriggerMinimumQuietTimeDuration("", segment0MinimumQuietTime)
            specAn.SetTriggerDelay("", triggerDelay)


            specAn.PowerList.Configuration.SetNumberOfSegments("", numberOfSegments)
            specAn.PowerList.Configuration.SetSegmentFrequency("", centerFrequencyRampPattern)

            For i As Integer = 0 To numberOfSegments - 1
                referenceLevelArray(i) = referenceLevelRampPattern(i) + papr
            Next
            specAn.PowerList.Configuration.SetSegmentReferenceLevel("", referenceLevelArray)
            For i As Integer = 0 To numberOfSegments - 1
                measurementLengthArray(i) = measurementLength
            Next
            specAn.PowerList.Configuration.SetSegmentMeasurementLength("", measurementLengthArray)
            segmentTriggerArray(0) = CInt(segmentTriggerType.IQPowerEdge)
            For i As Integer = 1 To numberOfSegments - 1
                segmentTriggerArray(i) = CInt(segmentTriggerType.DigitalEdge)
            Next
            specAn.PowerList.Configuration.SetSegmentTriggerType("", segmentTriggerArray)
            For i As Integer = 0 To numberOfSegments - 1
                segmentLengthArray(i) = segmentLength
            Next
            specAn.PowerList.Configuration.SetSegmentLength("", segmentLengthArray)
            For i As Integer = 0 To numberOfSegments - 1
                rbwArray(i) = rbw
            Next
            specAn.PowerList.Configuration.SetSegmentRbwFilterBandwidth("", rbwArray)
            For i As Integer = 0 To numberOfSegments - 1
                rbwFilterTypeArray(i) = CInt(rbwFilterType)
            Next
            specAn.PowerList.Configuration.SetSegmentRbwFilterType("", rbwFilterTypeArray)
            For i As Integer = 0 To numberOfSegments - 1
                rbwRrcAlphaArray(i) = rrcAlpha
            Next
            specAn.PowerList.Configuration.SetSegmentRbwFilterAlpha("", rbwRrcAlphaArray)

            specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.PowerList, False)

            specAn.Initiate("", "")
            rfsgSession.Initiate()
            instrSession.WaitForAcquisitionComplete(timeout)
        End Sub

        Private Sub RetrieveResults()
            specAn.PowerList.Results.FetchMinimumPowerArray("", timeout, minimumPower)
            specAn.PowerList.Results.FetchMaximumPowerArray("", timeout, maximumPower)
            specAn.PowerList.Results.FetchMeanAbsolutePowerArray("", timeout, meanAbsolutePower)
        End Sub


        Private Sub PrintResults()
            Console.WriteLine(vbLf & "-----------Measurements----------- " & vbLf)
            For i As Integer = 0 To numberOfSegments - 1
                Console.WriteLine("Offset {0}: ", i)
                Console.WriteLine("Mean Absolute Power (dBm)     :  {0}", meanAbsolutePower(i))
                Console.WriteLine("Maximum Power (dBm)           :  {0}", maximumPower(i))
                Console.WriteLine("Minimum Power (dBm)           :  {0}", minimumPower(i))
                Console.WriteLine("-------------------------------------------------" & vbLf)
            Next
        End Sub

        Private Sub CloseSession()
            If specAn IsNot Nothing Then
                specAn.Dispose()
                specAn = Nothing
            End If
            If instrSession IsNot Nothing Then
                instrSession.Close()
                instrSession = Nothing
            End If
            If rfsgSession IsNot Nothing Then
                rfsgSession.Abort()
                rfsgSession.BasicConfigurationList.DeleteConfigurationList("PowerList")
                rfsgSession.Arb.ClearWaveform(waveformName)
                rfsgSession.Close()
                rfsgSession = Nothing
            End If
        End Sub

        Private Shared Sub DisplayError(ex As Exception)
            Console.WriteLine("ERROR:" & vbLf & Convert.ToString(ex.[GetType]()) & ": " & ex.Message)
        End Sub

        Private Sub LinearRampPattern(start As Double, [end] As Double, samples As Integer, includeEnd As Boolean, ByRef rampPattern As Double())
            Dim m As Integer = If(includeEnd, samples, (samples - 1))
            Dim delta As Double = ([end] - start) / m
            For i As Integer = 0 To samples - 1
                rampPattern(i) = start + (i * delta)
            Next
        End Sub
        Private Sub LinearRampPatternReverse(start As Double, [end] As Double, samples As Integer, includeEnd As Boolean, ByRef rampPattern As Double())
            Dim m As Integer = If(includeEnd, samples - 1, (samples - 1))
            Dim delta As Double = (start - [end]) / m
            For i As Integer = 0 To samples - 1
                rampPattern(i) = [end] + (i * delta)
            Next
        End Sub
    End Class
End Namespace