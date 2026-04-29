'Steps:
'1. Open a new RFmx session.
'2. Configure Frequency Reference.
'3. Create a named signal instance.
'4. Configure sweep settings: Frequency List, IF Bandwidth, and Power Level and Test Rx Attenuation with different port names.
'5. Select S-Parameter measurement.
'6. Configure number of S-Parameters.
'7. Configure each S-Parameter and format.
'8. Configure Magnitude Units & Phase Trace Type.
'9. Load one or more Calset data from files(s) to create a global pool of named calsets.
'10. Select a named calset from the global pool to set as active calset for the specified signal. 
'11. Enable Correction
'12. Initiate the Measurement after user confirmation
'13. Read Number of SParams And X-Axis Values (aggregated frequency list).
'14. Fetch S-Parameter X data. 
'15. Fetch S-Parameter Y data for each S-Parameter.
'16. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.VnaMX

Namespace NationalInstruments.Examples.RFmxVnaSParamsCorrectedWithNamedCalsetLoad
    Public Class RFmxVnaSParamsCorrectedWithNamedCalsetLoad
        Private instrSession As RFmxInstrMX
        Private vna As RFmxVnaMX
        Private vnaSignal1 As RFmxVnaMX
        Private resourceName As String

        Private frequencyListSize As Integer
        Private frequencyStart As Double
        Private frequencyStop As Double
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

        Private magnitudeUnits As RFmxVnaMXSParamsMagnitudeUnits
        Private phaseTraceType As RFmxVnaMXSParamsPhaseTraceType

        Private numberOfCalsets As Integer
        Private calsetFilePath As String()
        Private calsetName As String()

        Private sParamSelectorString As String
        Private portSelectorString As String

        Private timeout As Double

        Private numberOfSParamsResult As Integer
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

            frequencyListSize = 251
            frequencyStart = 1000000000.0
            frequencyStop = 26000000000.0

            port1PowerLevel = -10.0                                                                             '(dBm)
            port2PowerLevel = -10.0                                                                             '(dBm)
            port1TestReceiverAttenuation = 0.0                                                                  '(dB)
            port2TestReceiverAttenuation = 0.0                                                                  '(dB)
            IFBandwidth = 100000.0                                                                              '(Hz)

            frequencyReferenceSource = RFmxInstrMXConstants.PxiClock
            frequencyReferenceFrequency = 100000000.0                                                           '(Hz)

            numberOfSParams = 4
            sParamsSParameters = New String() {"S11", "S12", "S21", "S22"}
            sParamsFormats = New RFmxVnaMXSParamsFormat() {RFmxVnaMXSParamsFormat.Magnitude, RFmxVnaMXSParamsFormat.Magnitude, RFmxVnaMXSParamsFormat.Magnitude, RFmxVnaMXSParamsFormat.Magnitude}

            magnitudeUnits = RFmxVnaMXSParamsMagnitudeUnits.dB
            phaseTraceType = RFmxVnaMXSParamsPhaseTraceType.Wrapped

            numberOfCalsets = 3
            calsetFilePath = New String() {"", "", ""}
            calsetName = New String() {"Calset1", "Calset2", "Calset3"}

            timeout = 10.0                                                                                      'seconds
        End Sub

        Private Sub InitializeInstr()
            instrSession = New RFmxInstrMX(resourceName, "")
        End Sub

        Private Sub ConfigureVna()
            vna = instrSession.GetVnaSignalConfiguration()                                                      'Create a new RFmx Session
            vna.CloneSignalConfiguration("Signal1", vnaSignal1)                                            'Create a named signal instance
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
            vnaSignal1.SetSweepType("", RFmxVnaMXSweepType.Linear)
            vnaSignal1.SetStartFrequency("", frequencyStart)
            vnaSignal1.SetStopFrequency("", frequencyStop)
            vnaSignal1.SetNumberOfPoints("", frequencyListSize)
            vnaSignal1.SetIFBandwidth("", IFBandwidth)
            portSelectorString = RFmxVnaMX.BuildPortString("", "port1")
            vnaSignal1.SetPowerLevel(portSelectorString, port1PowerLevel)
            vnaSignal1.SetTestReceiverAttenuation(portSelectorString, port1TestReceiverAttenuation)
            portSelectorString = RFmxVnaMX.BuildPortString("", "port2")
            vnaSignal1.SetPowerLevel(portSelectorString, port2PowerLevel)
            vnaSignal1.SetTestReceiverAttenuation(portSelectorString, port2TestReceiverAttenuation)

            vnaSignal1.SelectMeasurements("", RFmxVnaMXMeasurementTypes.SParams, False)

            vnaSignal1.SParams.Configuration.SetNumberOfSParameters("", numberOfSParams)

            For i As Integer = 0 To numberOfSParams - 1
                sParamSelectorString = RFmxVnaMX.BuildSParameterString("", i)
                vnaSignal1.SParams.Configuration.ConfigureSParameter(sParamSelectorString, sParamsSParameters(i))
                vnaSignal1.SParams.Configuration.SetFormat(sParamSelectorString, sParamsFormats(i))
            Next

            vnaSignal1.SParams.Configuration.SetMagnitudeUnits("", magnitudeUnits)
            vnaSignal1.SParams.Configuration.SetPhaseTraceType("", phaseTraceType)

            For i As Integer = 0 To numberOfCalsets - 1
                vna.CalsetLoadFromFile("", calsetName(i), calsetFilePath(i))
            Next
            vnaSignal1.SelectActiveCalset("", "Calset1", RFmxVnaMXRestoreConfiguration.None)

            vnaSignal1.SetCorrectionEnabled("", RFmxVnaMXCorrectionEnabled.True)
            vnaSignal1.Initiate("", "")

        End Sub

        Private Sub RetrieveResults()
            vnaSignal1.SParams.Configuration.GetNumberOfSParameters("", numberOfSParamsResult)
            vnaSignal1.SParams.Results.FetchXData("", timeout, sParamsXDataResult)
            sParamsY1DataResult = New Single(numberOfSParamsResult)() {}
            sParamsY2DataResult = New Single(numberOfSParamsResult)() {}

            For i As Integer = 0 To numberOfSParamsResult - 1
                sParamSelectorString = RFmxVnaMX.BuildSParameterString("", i)
                vnaSignal1.SParams.Results.FetchYData(sParamSelectorString, timeout, sParamsY1DataResult(i), sParamsY2DataResult(i))
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
