'Steps:
'1. Open a New RFmx session.
'2. Configure Frequency Reference.
'3. Configure sweep settings: Sweep Type, Start Frequency, Stop Frequency, Number Of Frequency Points,
'   If Bandwidth, And Power Level And Test Rx Attenuation With different port names.
'4. Configure Averaging.
'5. Select S-Parameter measurement.
'6. Configure number of S-Parameters.
'7. Configure each S-Parameter And format.
'8. Configure Magnitude Units & Phase Trace Type.
'9. Configure Calibration Ports And Calibration Method
'10. Configure Connector type & vCal Resource Name for each VNA port
'11. Initiate Calibration
'12. Read the Calstep Description for connection information.
'13. Acquire Calibration data after user confirmation 
'14. Save Calibration data
'15. Enable Correction
'16. Initiate the Measurement after user confirmation
'17. Read Number of SParams.
'18. Fetch S-Parameter X data.
'19. Fetch S-Parameter Y data for each S-Parameter.
'20. Fetch S-Parameter Correction State.
'21. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.VnaMX

Namespace NationalInstruments.Examples.RFmxVnaSParamsCorrectedSwitchModule
    Public Class RFmxVnaSParamsCorrectedSwitchModule
        Private instrSession As RFmxInstrMX
        Private vna As RFmxVnaMX
        Private resourceName As String
        Private frequencyStart As Double
        Private frequencyEnd As Double
        Private numberOfFrequencyPoints As Integer
        Private portNames As String()
        Private powerLevel As Double()
        Private testReceiverAttenuation As Double()
        Private IFBandwidth As Double
        Private frequencyReferenceSource As String
        Private frequencyReferenceFrequency As Double
        Private numberOfSParams As Integer
        Private sParamsReceiverPorts As String()
        Private sParamsSourcePorts As String()
        Private sParamsFormats As RFmxVnaMXSParamsFormat()
        Private calibrationPorts As String()
        Private vCalOrientation As String
        Private vCalResourceName As String
        Private connectorType As String
        Private calibrationTimeout As Double
        Private connectionInstruction As String
        Private magnitudeUnits As RFmxVnaMXSParamsMagnitudeUnits
        Private phaseTraceType As RFmxVnaMXSParamsPhaseTraceType
        Private averagingEnabled As RFmxVnaMXAveragingEnabled
        Private averagingCount As Integer
        Private sParamSelectorString As String
        Private portSelectorString As String
        Private timeout As Double
        Private numberOfSParamsResult As Integer
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

            frequencyStart = 1000000000.0                                                                       '(Hz)
            frequencyEnd = 26000000000.0                                                                        '(Hz)
            numberOfFrequencyPoints = 251

            portNames = New String() {"rmm0/port0", "rmm0/port1"}
            powerLevel = New Double() {-10.0, -10.0}                                                            '(dBm)
            testReceiverAttenuation = New Double() {0.0, 0.0}                                                   '(dB)

            IFBandwidth = 100000.0                                                                              '(Hz)

            frequencyReferenceSource = RFmxInstrMXConstants.PxiClock
            frequencyReferenceFrequency = 100000000.0                                                           '(Hz)

            numberOfSParams = 4
            sParamsReceiverPorts = New String() {"rmm0/port0", "rmm0/port0", "rmm0/port1", "rmm0/port1"}
            sParamsSourcePorts = New String() {"rmm0/port0", "rmm0/port1", "rmm0/port0", "rmm0/port1"}

            sParamsFormats = New RFmxVnaMXSParamsFormat() {RFmxVnaMXSParamsFormat.Magnitude, RFmxVnaMXSParamsFormat.Magnitude, RFmxVnaMXSParamsFormat.Magnitude, RFmxVnaMXSParamsFormat.Magnitude}

            calibrationPorts = New String() {"rmm0/port0", "rmm0/port1"}
            vCalOrientation = "portA:rmm0/port0,portB:rmm0/port1"
            vCalResourceName = "vCal"
            connectorType = "3.5 mm female"
            calibrationTimeout = 100.0                                                                          'seconds

            magnitudeUnits = RFmxVnaMXSParamsMagnitudeUnits.dB
            phaseTraceType = RFmxVnaMXSParamsPhaseTraceType.Wrapped

            averagingEnabled = RFmxVnaMXAveragingEnabled.False
            averagingCount = 10

            timeout = 10.0                                                                                      'seconds
        End Sub

        Private Sub InitializeInstr()
            instrSession = New RFmxInstrMX(resourceName, "")
        End Sub

        Private Sub ConfigureVna()
            vna = instrSession.GetVnaSignalConfiguration()                                                      'Create a new RFmx Session
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
            vna.SetSweepType("", RFmxVnaMXSweepType.Linear)
            vna.SetStartFrequency("", frequencyStart)
            vna.SetStopFrequency("", frequencyEnd)
            vna.SetNumberOfPoints("", numberOfFrequencyPoints)
            vna.SetIFBandwidth("", IFBandwidth)
            For i As Integer = 0 To portNames.Length - 1
                portSelectorString = RFmxVnaMX.BuildPortString("", portNames(i))
                vna.SetPowerLevel(portSelectorString, powerLevel(i))
                vna.SetTestReceiverAttenuation(portSelectorString, testReceiverAttenuation(i))
            Next

            vna.SetAveragingEnabled("", averagingEnabled)
            vna.SetAveragingCount("", averagingCount)

            vna.SelectMeasurements("", RFmxVnaMXMeasurementTypes.SParams, False)

            vna.SParams.Configuration.SetNumberOfSParameters("", numberOfSParams)

            For i As Integer = 0 To numberOfSParams - 1
                sParamSelectorString = RFmxVnaMX.BuildSParameterString("", i)
                vna.SParams.Configuration.SetReceiverPort(sParamSelectorString, sParamsReceiverPorts(i))
                vna.SParams.Configuration.SetSourcePort(sParamSelectorString, sParamsSourcePorts(i))
                vna.SParams.Configuration.SetFormat(sParamSelectorString, sParamsFormats(i))
            Next

            vna.SParams.Configuration.SetMagnitudeUnits("", magnitudeUnits)
            vna.SParams.Configuration.SetPhaseTraceType("", phaseTraceType)

            vna.SetCorrectionCalibrationPorts("", calibrationPorts)
            vna.SetCorrectionCalibrationMethod("", RFmxVnaMXCorrectionCalibrationMethod.Solt)
            vna.SetCorrectionCalibrationCalkitElectronicOrientation("", vCalOrientation)

            vna.SetCorrectionCalibrationConnectorType("port::all", connectorType)
            vna.SetCorrectionCalibrationCalkitElectronicResourceName("port::all", vCalResourceName)

            vna.CalibrationInitiate("")
            vna.GetCorrectionCalibrationStepDescription("", connectionInstruction)
            Console.WriteLine(connectionInstruction)
            Console.WriteLine("Press any key to continue.")
            Console.ReadKey()
            vna.CalibrationAcquire("", calibrationTimeout)
            vna.CalibrationSave("", "")
            Console.WriteLine("Connect DUT across specified measurement ports.")
            Console.WriteLine("Press any key to continue.")
            Console.ReadKey()

            vna.SetCorrectionEnabled("", RFmxVnaMXCorrectionEnabled.True)
            vna.Initiate("", "")

        End Sub

        Private Sub RetrieveResults()
            vna.SParams.Configuration.GetNumberOfSParameters("", numberOfSParamsResult)
            vna.SParams.Results.FetchXData("", timeout, sParamsXDataResult)
            sParamsY1DataResult = New Single(numberOfSParamsResult)() {}
            sParamsY2DataResult = New Single(numberOfSParamsResult)() {}

            For i As Integer = 0 To numberOfSParamsResult - 1
                sParamSelectorString = RFmxVnaMX.BuildSParameterString("", i)
                vna.SParams.Results.FetchYData(sParamSelectorString, timeout, sParamsY1DataResult(i), sParamsY2DataResult(i))
            Next
            vna.SParams.Results.GetCorrectionState("", correctionStateResult)
            Console.WriteLine("Correction State: " & correctionStateResult)
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
