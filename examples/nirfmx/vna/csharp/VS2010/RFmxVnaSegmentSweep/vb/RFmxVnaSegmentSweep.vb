'Steps:
'1.Open a New RFmx session.
'2. Configure Frequency Reference.
'3. Configure sweep settings: If Bandwidth, Power Level And Test Rx Attenuation With different port names.
'4. Configure Sweep Type as Segment.Configure Number of Segments And Independent Settings enabled per segment.
'5. Configure per segment settings Like Segment Enabled, Start And Stop Frequencies, Number of Frequency points, Segment IF Bandwidth, Segment Dwell Time, Segment Power Level And Segment Test Receiver Attenuation.
'   For the properties where the Segment <property> Enabled was set to True in Step 4, values configured in Step 5 are used. If they were set to False, values configured in Step 3 are used.
'6. Configure Trigger settings.
'7. Select S-Parameter measurement.
'8. Configure number of S-Parameters.
'9. Configure each S-Parameter and format.
'10. Configure Magnitude Units & Phase Trace Type.
'11. Configure Calibration Ports And Calibration Method
'12. Configure Connector type & vCal Resource Name for each VNA port
'13. Initiate Calibration
'14. Acquire Calibration data after user confirmation
'15. Save Calibration data
'16. Enable Correction
'17. Initiate the Measurement after user confirmation
'18. Read Number of SParams.
'19. Fetch S-Parameter Correction State.
'20. Fetch S-Parameter X data.
'21. Fetch S-Parameter Y data for each S-Parameter.
'22. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.VnaMX

Namespace NationalInstruments.Examples.RFmxVnaSegmentSweep
    Public Class RFmxVnaSegmentSweep
        Private instrSession As RFmxInstrMX
        Private vna As RFmxVnaMX
        Private resourceName As String

        Private port1PowerLevel As Double
        Private port2PowerLevel As Double
        Private port1TestReceiverAttenuation As Double
        Private port2TestReceiverAttenuation As Double

        Private IFBandwidth As Double
        Private segmentPowerLevelEnabled As RFmxVnaMXSegmentPowerLevelEnabled
        Private segmentIFBandwidthEnabled As RFmxVnaMXSegmentIFBandwidthEnabled
        Private segmentTestReceiverAttenuationEnabled As RFmxVnaMXSegmentTestReceiverAttenuationEnabled
        Private segmentDwellTimeEnabled As RFmxVnaMXSegmentDwellTimeEnabled

        Private segmentEnabled As RFmxVnaMXSegmentEnabled()
        Private numberOfSegments As Integer
        Private segmentStartFrequency As Double()
        Private segmentStopFrequency As Double()
        Private segmentNumberOfFrequencyPoints As Integer()
        Private segmentIFBandwidth As Double()
        Private segmentDwellTime As Double()
        Private port1SegmentPowerLevel As Double()
        Private port1SegmentTestReceiverAttenuation As Double()
        Private port2SegmentPowerLevel As Double()
        Private port2SegmentTestReceiverAttenuation As Double()

        Private triggerType As RFmxVnaMXTriggerType
        Private triggerMode As RFmxVnaMXTriggerMode
        Private triggerDelay As Double

        Private numberOfSParams As Integer
        Private sParamsSParameters As String()
        Private sParamsFormats As RFmxVnaMXSParamsFormat()
        Private magnitudeUnits As RFmxVnaMXSParamsMagnitudeUnits
        Private phaseTraceType As RFmxVnaMXSParamsPhaseTraceType

        Private vCalResourceName As String
        Private connectorType As String
        Private calibrationTimeout As Double

        Private sParamSelectorString As String
        Private portSelectorString As String
        Private segmentSelectorString As String
        Private timeout As Double

        Private numberOfSParamsResult As Integer
        Private xAxisValues As Double()
        Private correctionStateResult As RFmxVnaMXSParamsCorrectionState
        Private sParamsXDataResult As Double()
        Private sParamsY1DataResult As Single()()
        Private sParamsY2DataResult As Single()()

        Public Sub Run()
            Try
                InitializeVariables()
                InitializeInstr()
                ConfigureVna()
                RetrieveResults()
            Catch ex As Exception
                DisplayError(ex)
            Finally
                'Close session
                CloseSession()
                Console.WriteLine("Press any key to exit")
                Console.ReadKey()
            End Try
        End Sub

        Private Sub InitializeVariables()
            resourceName = "VNA"
            port1PowerLevel = -10.0                                                                                          '(dBm)
            port2PowerLevel = -10.0                                                                                          '(dBm)
            port1TestReceiverAttenuation = 0.0                                                                               '(dB)
            port2TestReceiverAttenuation = 0.0                                                                               '(dB)
            IFBandwidth = 100000.0                                                                                           '(Hz)

            segmentPowerLevelEnabled = RFmxVnaMXSegmentPowerLevelEnabled.True
            segmentIFBandwidthEnabled = RFmxVnaMXSegmentIFBandwidthEnabled.True
            segmentTestReceiverAttenuationEnabled = RFmxVnaMXSegmentTestReceiverAttenuationEnabled.True
            segmentDwellTimeEnabled = RFmxVnaMXSegmentDwellTimeEnabled.True

            numberOfSegments = 5
            segmentEnabled = New RFmxVnaMXSegmentEnabled() {RFmxVnaMXSegmentEnabled.True, RFmxVnaMXSegmentEnabled.True,
                RFmxVnaMXSegmentEnabled.True, RFmxVnaMXSegmentEnabled.True, RFmxVnaMXSegmentEnabled.True}
            segmentStartFrequency = New Double() {1000000000.0, 5100000000.0, 10100000000.0, 15100000000.0, 20100000000.0}   '(Hz)
            segmentStopFrequency = New Double() {5000000000.0, 10000000000.0, 15000000000.0, 20000000000.0, 26500000000.0}   '(Hz)
            segmentNumberOfFrequencyPoints = New Integer() {41, 50, 50, 50, 65}
            segmentIFBandwidth = New Double() {100000.0, 1000000.0, 100000.0, 10000.0, 100000.0}                             '(Hz)
            segmentDwellTime = New Double() {0.0, 0.0, 0.0, 0.0, 0.0}                                                        '(s)
            port1SegmentPowerLevel = New Double() {-10.0, -10.0, -10.0, -10.0, -10.0}                                        '(dBm)
            port1SegmentTestReceiverAttenuation = New Double() {0.0, 0.0, 0.0, 0.0, 0.0}                                     '(dB)
            port2SegmentPowerLevel = New Double() {-10.0, -10.0, -10.0, -10.0, -10.0}                                        '(dBm)
            port2SegmentTestReceiverAttenuation = New Double() {0.0, 0.0, 0.0, 0.0, 0.0}                                     '(dB)

            triggerType = RFmxVnaMXTriggerType.None
            triggerMode = RFmxVnaMXTriggerMode.Segment
            triggerDelay = 0.0                                                                                               '(s)

            numberOfSParams = 4
            sParamsSParameters = New String() {"S11", "S12", "S21", "S22"}
            sParamsFormats = New RFmxVnaMXSParamsFormat() {RFmxVnaMXSParamsFormat.Magnitude,
                RFmxVnaMXSParamsFormat.Magnitude, RFmxVnaMXSParamsFormat.Magnitude, RFmxVnaMXSParamsFormat.Magnitude}
            magnitudeUnits = RFmxVnaMXSParamsMagnitudeUnits.dB
            phaseTraceType = RFmxVnaMXSParamsPhaseTraceType.Wrapped

            vCalResourceName = "vCal"
            connectorType = "3.5 mm female"
            calibrationTimeout = 100.0                                                                                       'seconds

            timeout = 10.0                                                                                                   'seconds

            xAxisValues = New Double(numberOfSegments) {}
        End Sub

        Private Sub InitializeInstr()
            instrSession = New RFmxInstrMX(resourceName, "")
        End Sub

        Private Sub ConfigureVna()
            vna = instrSession.GetVnaSignalConfiguration()                                                                   'Create a new RFmx Session
            instrSession.ConfigureFrequencyReference("", RFmxInstrMXConstants.PxiClock, 100000000.0)
            vna.SetIFBandwidth("", IFBandwidth)
            portSelectorString = RFmxVnaMX.BuildPortString("", "port1")
            vna.SetPowerLevel(portSelectorString, port1PowerLevel)
            vna.SetTestReceiverAttenuation(portSelectorString, port1TestReceiverAttenuation)
            portSelectorString = RFmxVnaMX.BuildPortString("", "port2")
            vna.SetPowerLevel(portSelectorString, port2PowerLevel)
            vna.SetTestReceiverAttenuation(portSelectorString, port2TestReceiverAttenuation)

            vna.SetSweepType("", RFmxVnaMXSweepType.Segment)
            vna.SetNumberOfSegments("", numberOfSegments)
            vna.SetSegmentPowerLevelEnabled("", segmentPowerLevelEnabled)
            vna.SetSegmentIFBandwidthEnabled("", segmentIFBandwidthEnabled)
            vna.SetSegmentTestReceiverAttenuationEnabled("", segmentTestReceiverAttenuationEnabled)
            vna.SetSegmentDwellTimeEnabled("", segmentDwellTimeEnabled)
            For i As Integer = 0 To numberOfSegments - 1
                segmentSelectorString = RFmxVnaMX.BuildSegmentString("", i)
                vna.SetSegmentEnabled(segmentSelectorString, segmentEnabled(i))
                vna.SetSegmentStartFrequency(segmentSelectorString, segmentStartFrequency(i))
                vna.SetSegmentStopFrequency(segmentSelectorString, segmentStopFrequency(i))
                vna.SetSegmentNumberOfFrequencyPoints(segmentSelectorString, segmentNumberOfFrequencyPoints(i))
                vna.SetSegmentIFBandwidth(segmentSelectorString, segmentIFBandwidth(i))
                vna.SetSegmentDwellTime(segmentSelectorString, segmentDwellTime(i))
                portSelectorString = RFmxVnaMX.BuildPortString(segmentSelectorString, "port1")
                vna.SetSegmentPowerLevel(portSelectorString, port1SegmentPowerLevel(i))
                vna.SetSegmentTestReceiverAttenuation(portSelectorString, port1SegmentTestReceiverAttenuation(i))
                portSelectorString = RFmxVnaMX.BuildPortString(segmentSelectorString, "port2")
                vna.SetSegmentPowerLevel(portSelectorString, port2SegmentPowerLevel(i))
                vna.SetSegmentTestReceiverAttenuation(portSelectorString, port2SegmentTestReceiverAttenuation(i))
            Next
            vna.SetTriggerType("", triggerType)
            vna.SetTriggerMode("", triggerMode)
            vna.SetTriggerDelay("", triggerDelay)
            vna.SelectMeasurements("", RFmxVnaMXMeasurementTypes.SParams, False)
            vna.SParams.Configuration.SetNumberOfSParameters("", numberOfSParams)
            For i As Integer = 0 To numberOfSParams - 1
                sParamSelectorString = RFmxVnaMX.BuildSParameterString("", i)
                vna.SParams.Configuration.ConfigureSParameter(sParamSelectorString, sParamsSParameters(i))
                vna.SParams.Configuration.SetFormat(sParamSelectorString, sParamsFormats(i))
            Next
            vna.SParams.Configuration.SetMagnitudeUnits("", magnitudeUnits)
            vna.SParams.Configuration.SetPhaseTraceType("", phaseTraceType)

            vna.SetCorrectionCalibrationPorts("", New String() {"port1", "port2"})
            vna.SetCorrectionCalibrationMethod("", RFmxVnaMXCorrectionCalibrationMethod.Solt)

            vna.SetCorrectionCalibrationConnectorType("port::all", connectorType)
            vna.SetCorrectionCalibrationCalkitElectronicResourceName("port::all", vCalResourceName)

            vna.CalibrationInitiate("")
            Console.WriteLine("Connect Port A of NI CAL-5501 to Port 1 of NI PXIe-5633,  and Port B of NI CAL-5501 to Port 2 of NI PXIe-5633.")
            Console.WriteLine("Press any key to continue.")
            Console.ReadKey()
            vna.CalibrationAcquire("", calibrationTimeout)
            vna.CalibrationSave("", "")
            Console.WriteLine("Connect DUT across port1 and port2 of NI PXIe-5633.")
            Console.WriteLine("Press any key to continue.")
            Console.ReadKey()

            vna.SetCorrectionEnabled("", RFmxVnaMXCorrectionEnabled.True)
            vna.Initiate("", "")
        End Sub

        Private Sub RetrieveResults()
            vna.SParams.Configuration.GetNumberOfSParameters("", numberOfSParamsResult)
            vna.SParams.Results.GetCorrectionState("", correctionStateResult)
            vna.SParams.Results.FetchXData("", timeout, sParamsXDataResult)
            sParamsY1DataResult = New Single(numberOfSParamsResult)() {}
            sParamsY2DataResult = New Single(numberOfSParamsResult)() {}

            For i As Integer = 0 To numberOfSParamsResult - 1
               sParamSelectorString = RFmxVnaMX.BuildSParameterString("", i)
               vna.SParams.Results.FetchYData(sParamSelectorString, timeout, sParamsY1DataResult(i), sParamsY2DataResult(i))
            Next
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
            Console.WriteLine("ERROR:" & vbLf & Convert.ToString(ex.[GetType]()) & ": " + ex.Message)
        End Sub
    End Class
End Namespace
