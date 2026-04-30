'Steps:
'1. Open a new RFmx session.
'2. Configure the basic instrument properties (Clock Source and Clock Frequency).
'3. Configure Selected Ports.
'4. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
'5. Configure Trigger Type and Trigger Parameters.
'6. Configure PAVT measurement and enable the traces.
'7. Configure Measurement Location Type.

'8. Follow these steps depending upon Measurement Location Type :
' When Measurement Location Type is Time, configure
'8.1. Segment Start Time by :
'8.1.1. Configuring Number of Segments, Segment0 Start Time(s) and Segment Interval(s).
'8.2. Segment Start Time by :
'8.2.1. Configuring Segment Start Time(s).
'8.2.2. Configuring Number of Segments.
' When Measurement Location Type is Trigger, configure
'8.3. Number of Segments.

'9. Configure Measurement Bandwidth.
'10. Configure Measurement Interval.
'11. Initiate Measurement.
'12. Fetch PAVT Traces and Measurements.
'13. Close the RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.SpecAnMX

Namespace NationalInstruments.Examples.RFmxSpecAnPavt
   Public Enum MeasurementStartTimeType
      [Step] = 0
      List = 1
   End Enum

   Public Class RFmxSpecAnPavt
      Private instrSession As RFmxInstrMX
      Private specAn As RFmxSpecAnMX

      Private resourceName As String
      Private selectedPorts As String
      Private centerFrequency As Double
      Private referenceLevel As Double
      Private externalAttenuation As Double

      Private frequencyReferenceSource As String
      Private frequencyReference As Double

      Private enableTrigger As Boolean
      Private digitalEdgeSource As String
      Private digitalEdge As RFmxSpecAnMXDigitalEdgeTriggerEdge
      Private triggerDelay As Double

      Private measurementLocationType As RFmxSpecAnMXPavtMeasurementLocationType
      Const NumberOfSegments As Integer = 1

      Private segment0StartTime As Double
      Private segmentInterval As Double

      Const segmentStartTimeArraySize As Integer = 1
      Private segmentStartTime As Double() = New Double(segmentStartTimeArraySize - 1) {}

      Private measurementStartTimeType As MeasurementStartTimeType

      Private measurementBandwidth As Double

      Private measurementOffset As Double
      Private measurementLength As Double

      Private timeout As Double

      Private meanRelativePhase As Double() = New Double(NumberOfSegments - 1) {}
      ' (deg)
      Private meanRelativeAmplitude As Double() = New Double(NumberOfSegments - 1) {}
      ' (dB)
      Private meanAbsolutePhase As Double() = New Double(NumberOfSegments - 1) {}
      ' (deg)
      Private meanAbsoluteAmplitude As Double() = New Double(NumberOfSegments - 1) {}
      ' (dBm)

      Private amplitude As AnalogWaveform(Of Single)() = New AnalogWaveform(Of Single)(NumberOfSegments - 1) {}
      ' (dBm)
      Private phase As AnalogWaveform(Of Single)() = New AnalogWaveform(Of Single)(NumberOfSegments - 1) {}
      ' (deg)

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
            Console.WriteLine("Press any key to exit")
            Console.ReadKey()
         End Try
      End Sub

      Private Sub InitializeVariables()
         resourceName = "RFSA"

         selectedPorts = ""
         centerFrequency = 1000000000.0
         ' (Hz)
         referenceLevel = 0.0
         ' (dBm)
         externalAttenuation = 0.0
         ' (dB)

         frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
         frequencyReference = 10000000.0
         ' (Hz)

         enableTrigger = True
         digitalEdgeSource = RFmxSpecAnMXConstants.PxiTriggerLine0
         digitalEdge = RFmxSpecAnMXDigitalEdgeTriggerEdge.Rising
         triggerDelay = 0.0
         ' (s)

         measurementLocationType = RFmxSpecAnMXPavtMeasurementLocationType.Time

         ' Segment Step
         segment0StartTime = 0.0
         ' (s)
         segmentInterval = 0.001
         ' (s)

         ' Segment List
         segmentStartTime(0) = 0.0
         ' (s)

         measurementStartTimeType = MeasurementStartTimeType.Step

         measurementBandwidth = 10000000.0
         ' (Hz)

         measurementOffset = 0.0
         ' (s)
         measurementLength = 0.001
         ' (s)

         timeout = 10.0
         ' (s)
      End Sub

      Private Sub InitializeInstr()
         ' Create a new RFmx Session
         instrSession = New RFmxInstrMX(resourceName, "")
      End Sub

      Private Sub ConfigureSpecAn()
         ' Get SpecAn signal
         specAn = instrSession.GetSpecAnSignalConfiguration()

         ' Configure measurement
         instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReference)
         specAn.SetSelectedPorts("", selectedPorts)
         specAn.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
         specAn.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger)
         specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Pavt, True)
         specAn.Pavt.Configuration.ConfigureMeasurementLocationType("", measurementLocationType)
         If measurementLocationType = RFmxSpecAnMXPavtMeasurementLocationType.Time Then
            If measurementStartTimeType = MeasurementStartTimeType.Step Then
               specAn.Pavt.Configuration.ConfigureSegmentStartTimeStep("", NumberOfSegments,
                  segment0StartTime, segmentInterval)
            Else
               specAn.Pavt.Configuration.ConfigureNumberOfSegments("", segmentStartTimeArraySize)
               specAn.Pavt.Configuration.ConfigureSegmentStartTimeList("", segmentStartTime)
            End If
         Else
            specAn.Pavt.Configuration.ConfigureNumberOfSegments("", NumberOfSegments)
         End If
         specAn.Pavt.Configuration.ConfigureMeasurementBandwidth("", measurementBandwidth)
         specAn.Pavt.Configuration.ConfigureMeasurementInterval("", measurementOffset, measurementLength)
         specAn.Initiate("", "")
      End Sub

      Private Sub RetrieveResults()
         specAn.Pavt.Results.FetchPhaseAndAmplitudeArray("", timeout, meanRelativePhase, meanRelativeAmplitude,
            meanAbsolutePhase, meanAbsoluteAmplitude)

         For i As Integer = 0 To NumberOfSegments - 1
            specAn.Pavt.Results.FetchPhaseTrace("", timeout, i, phase(i))
            specAn.Pavt.Results.FetchAmplitudeTrace("", timeout, i, amplitude(i))
         Next
      End Sub

      Private Sub PrintResults()
         Console.WriteLine("Segment0 Mean Absolute Phase (deg)       : {0}", meanAbsolutePhase(0))
         Console.WriteLine("Segment0 Mean Absolute Amplitude (dBm)   : {0}" & vbLf, meanAbsoluteAmplitude(0))
         Console.WriteLine("Segment Measurements")
         For i As Integer = 0 To NumberOfSegments - 1
            Console.WriteLine("Segment  :  {0}", i)
            Console.WriteLine("Mean Relative Phase (deg)                : {0}", meanRelativePhase(i))
            Console.WriteLine("Mean Relative Amplitude (dB)             : {0}", meanRelativeAmplitude(i))
            Console.WriteLine("-------------------------------------------------" & vbLf)
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
End Namespace