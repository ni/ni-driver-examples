'Steps:
'1. Open a New RFmx session.
'2. Configure Frequency Reference.
'3. Configure sweep settings: Sweep Type, Start Frequency, Stop Frequency, Number Of Frequency Points, If Bandwidth, And Power Level And Test Rx Attenuation With different port names.
'4. Configure Averaging.
'5. Select S-Parameter measurement.
'6. Configure number of S-Parameters.
'7. Configure each S-Parameter And format.
'8. Configure Magnitude Units & Phase Trace Type.
'9. Configure Calibration Ports And Calibration Method
'10. Configure Connector type & vCal Resource Name for each VNA port
'11. Initiate Calibration
'12. Acquire Calibration data after user confirmation 
'13. Save Calibration data
'14. Enable Correction
'15. Initiate the Measurement after user confirmation
'16. Read Number of SParams.
'17. Fetch S-Parameter X data.
'18. Fetch S-Parameter Y data for each S-Parameter.
'19. Fetch S-Parameter Correction State.
'20. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.VnaMX

Namespace NationalInstruments.Examples.RFmxVnaSParamsCorrectedOnePort
    Public Class RFmxVnaSParamsCorrectedOnePort
        Private instrSession As RFmxInstrMX
        Private vna As RFmxVnaMX
        Private resourceName As String

        Private frequencyStart As Double
        Private frequencyEnd As Double
        Private numberOfFrequencyPoints As Integer
        Private port1PowerLevel As Double
        Private port2PowerLevel As Double
        Private port1TestReceiverAttenuation As Double
        Private port2TestReceiverAttenuation As Double
        Private IFBandwidth As Double

        Private frequencyReferenceSource As String
        Private frequencyReferenceFrequency As Double

        Private numberOfSParams As Integer
        Private sParamsSParameters As String()
        Private sParamsFormats As RFmxVnaMXSParamsFormat()

        Private calibrationPorts As String()
        Private vCalResourceName As String
        Private connectorType As String
        Private calibrationTimeout As Double
        Private allPortsSelectorString As String

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
            port1PowerLevel = -10.0                                                                             '(dBm)
            port2PowerLevel = -10.0                                                                             '(dBm)
            port1TestReceiverAttenuation = 0.0                                                                  '(dB)
            port2TestReceiverAttenuation = 0.0                                                                  '(dB)
            IFBandwidth = 100000.0                                                                              '(Hz)

            frequencyReferenceSource = RFmxInstrMXConstants.PxiClock
            frequencyReferenceFrequency = 100000000.0                                                           '(Hz)

            numberOfSParams = 2
            sParamsSParameters = New String() {"S11", "S11"}
            sParamsFormats = New RFmxVnaMXSParamsFormat() {RFmxVnaMXSParamsFormat.Magnitude,
                RFmxVnaMXSParamsFormat.Phase}

            calibrationPorts = New String() {"port1"}
            vCalResourceName = "vCal"
            connectorType = "3.5 mm female"
            calibrationTimeout = 100.0                                                                          'seconds
            allPortsSelectorString = "all"

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
            portSelectorString = RFmxVnaMX.BuildPortString("", "port1")
            vna.SetPowerLevel(portSelectorString, port1PowerLevel)
            vna.SetTestReceiverAttenuation(portSelectorString, port1TestReceiverAttenuation)
            portSelectorString = RFmxVnaMX.BuildPortString("", "port2")
            vna.SetPowerLevel(portSelectorString, port2PowerLevel)
            vna.SetTestReceiverAttenuation(portSelectorString, port2TestReceiverAttenuation)

            vna.SetAveragingEnabled("", averagingEnabled)
            vna.SetAveragingCount("", averagingCount)

            vna.SelectMeasurements("", RFmxVnaMXMeasurementTypes.SParams, False)

            vna.SParams.Configuration.SetNumberOfSParameters("", numberOfSParams)

            For i As Integer = 0 To numberOfSParams - 1
                sParamSelectorString = RFmxVnaMX.BuildSParameterString("", i)
                vna.SParams.Configuration.ConfigureSParameter(sParamSelectorString, sParamsSParameters(i))
                vna.SParams.Configuration.SetFormat(sParamSelectorString, sParamsFormats(i))
            Next

            vna.SParams.Configuration.SetMagnitudeUnits("", magnitudeUnits)
            vna.SParams.Configuration.SetPhaseTraceType("", phaseTraceType)

            vna.SetCorrectionCalibrationPorts("", calibrationPorts)
            vna.SetCorrectionCalibrationMethod("", RFmxVnaMXCorrectionCalibrationMethod.Sol)

            portSelectorString = RFmxVnaMX.BuildPortString("", allPortsSelectorString)
            vna.SetCorrectionCalibrationConnectorType(portSelectorString, connectorType)
            vna.SetCorrectionCalibrationCalkitElectronicResourceName(portSelectorString, vCalResourceName)

            vna.CalibrationInitiate("")
            Console.WriteLine("Connect Port A of NI CAL-5501 to Port 1 of NI PXIe-5633,  and Port B of NI CAL-5501 to Port 2 of NI PXIe-5633.")
            Console.WriteLine("Press any key to continue.")
            Console.ReadKey()
            vna.CalibrationAcquire("", calibrationTimeout)
            vna.CalibrationSave("", "")
            Console.WriteLine("Connect DUT to the calibrated port of NI PXIe-5633 and terminate the unused port.")
            Console.WriteLine("Press any key to continue.")
            Console.ReadKey()

            vna.SetCorrectionEnabled("", RFmxVnaMXCorrectionEnabled.True)
            vna.Initiate("", "")

        End Sub

        Private Sub RetrieveResults()
            vna.SParams.Configuration.GetNumberOfSParameters("", numberOfSParamsResult)
            vna.SParams.Results.FetchXData("", timeout, sParamsXDataResult)
            sParamsY1DataResult = New Single(numberOfSParamsResult - 1)() {}
            sParamsY2DataResult = New Single(numberOfSParamsResult - 1)() {}

            For i As Integer = 0 To numberOfSParamsResult - 1
                sParamSelectorString = RFmxVnaMX.BuildSParameterString("", i)
                vna.SParams.Results.FetchYData(sParamSelectorString, timeout, sParamsY1DataResult(i), sParamsY2DataResult(i))
            Next
            vna.SParams.Results.GetCorrectionState("", correctionStateResult)
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
