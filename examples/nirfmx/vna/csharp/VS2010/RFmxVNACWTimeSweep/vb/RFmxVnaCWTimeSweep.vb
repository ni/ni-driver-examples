'Steps:
'1.Open a new RFmx session.
'2. Configure Frequency Reference.
'3. Configure sweep settings: Sweep Type = CW Time, Number of Points, IF Bandwidth, and Power Level and Test Rx Attenuation with different port names.
'4. Select S-Parameter measurement.
'5. Configure number of S-Parameters.
'6. Configure each S-Parameter and format.
'7. Configure Magnitude Units & Phase Trace Type.
'8. Configure Calibration Ports and Calibration Method
'9. Configure Connector type & vCal Resource Name for each VNA port
'10. Initiate Calibration
'11. Acquire Calibration data after user confirmation 
'12. Save Calibration data
'13. Enable Correction
'14. Initiate the Measurement after user confirmation
'15. Read Number of SParams.
'16. Fetch S-Parameter X data.
'17. Fetch S-Parameter Y data for each S-Parameter.
'18. Fetch S-Parameter Correction State.
'19. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.VnaMX

Namespace NatioanlInstruments.Examples.RFmxVnaCWTimeSweep
    Public Class RFmxVnaCWTimeSweep
        Private instrSession As RFmxInstrMX
        Private vna As RFmxVnaMX
        Private resourceName As String

        Private cwfrequency As Double
        Private numberOfPoints As Integer
        Private IFBandwidth As Double
        Private port1PowerLevel As Double
        Private port1TestReceiverAttenuation As Double
        Private port2PowerLevel As Double
        Private port2TestReceiverAttenuation As Double


        Private frequencyReferenceSource As String
        Private frequencyReferenceFrequency As Double

        Private numberOfSParams As Integer
        Private sParamsSParameters As String()
        Private sParamsFormats As RFmxVnaMXSParamsFormat()

        Private magnitudeUnits As RFmxVnaMXSParamsMagnitudeUnits
        Private phaseTraceType As RFmxVnaMXSParamsPhaseTraceType

        Private calibrationPorts As String()
        Private vCalResourceName As String
        Private connectorType As String
        Private calibrationTimeout As Double

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
            resourceName = "VNA"

            cwfrequency = 1000000000.0
            ' (Hz) 
            numberOfPoints = 100
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

            frequencyReferenceSource = RFmxInstrMXConstants.PxiClock
            frequencyReferenceFrequency = 100000000.0
            ' (Hz) 

            numberOfSParams = 4
            sParamsSParameters = New String() {"S11", "S12", "S21", "S22"}
            sParamsFormats = New RFmxVnaMXSParamsFormat() {RFmxVnaMXSParamsFormat.Magnitude, RFmxVnaMXSParamsFormat.Magnitude, RFmxVnaMXSParamsFormat.Magnitude, RFmxVnaMXSParamsFormat.Magnitude}

            calibrationPorts = New String() {"port1", "port2"}
            vCalResourceName = "vCal"
            connectorType = "3.5 mm female"
            calibrationTimeout = 100.0
            'seconds 

            magnitudeUnits = RFmxVnaMXSParamsMagnitudeUnits.dB
            phaseTraceType = RFmxVnaMXSParamsPhaseTraceType.Wrapped

            timeout = 10.0
            'seconds 
        End Sub

        Private Sub InitializeInstr()
            instrSession = New RFmxInstrMX(resourceName, "")
        End Sub

        Private Sub ConfigureVna()
            vna = instrSession.GetVnaSignalConfiguration()
            ' Create a new RFmx Session 
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
            vna.SetSweepType("", RFmxVnaMXSweepType.CWTime)
            vna.SetCWFrequency("", cwfrequency)
            vna.SetNumberOfPoints("", numberOfPoints)
            vna.SetIFBandwidth("", IFBandwidth)
            portSelectorString = RFmxVnaMX.BuildPortString("", "port1")
            vna.SetPowerLevel(portSelectorString, port1PowerLevel)
            vna.SetTestReceiverAttenuation(portSelectorString, port1TestReceiverAttenuation)
            portSelectorString = RFmxVnaMX.BuildPortString("", "port2")
            vna.SetPowerLevel(portSelectorString, port2PowerLevel)
            vna.SetTestReceiverAttenuation(portSelectorString, port2TestReceiverAttenuation)

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

            vna.SetCorrectionEnabled("", RFmxVnaMXCorrectionEnabled.[True])
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

        Private Sub PrintResults()
            Console.WriteLine("Correction State            {0}", correctionStateResult)
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
