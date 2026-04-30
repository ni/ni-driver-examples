'Steps:
'1. Open a New RFmx session.
'2. Configure Frequency Reference.
'3. Configure sweep settings: Sweep Type, Start Frequency, Stop Frequency, Number Of Frequency Points, If Bandwidth, And Power Level With different port names.
'4. Configure Pulse Settings.
'5. Configure Averaging,
'6. Select S-Parameter measurement.
'7. Configure number of S-Parameters.
'8. Configure each S-Parameter And format.
'9. Initiate the Measurement.
'10. Read Number of SParams.
'11. Fetch S-Parameter X data.
'12. Fetch S-Parameter Y data for each S-Parameter.
'13. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.VnaMX

Namespace NationalInstruments.Examples.RFmxVnaPulsedSParams
    Public Class RFmxVnaPulsedSParams
        Private instrSession As RFmxInstrMX
        Private vna As RFmxVnaMX
        Private resourceName As String
        Private frequencyStart As Double
        Private frequencyEnd As Double
        Private numberOfFrequencyPoints As Integer
        Private port1PowerLevel As Double
        Private port2PowerLevel As Double
        Private frequencyReferenceSource As String
        Private frequencyReferenceFrequency As Double
        Private numberOfSParams As Integer
        Private sParamsSParameters As String()
        Private sParamsFormats As RFmxVnaMXSParamsFormat()
        Private magnitudeUnits As RFmxVnaMXSParamsMagnitudeUnits
        Private phaseTraceType As RFmxVnaMXSParamsPhaseTraceType
        Private pulseModeEnabled As RFmxVnaMXPulseModeEnabled
        Private pulsePeriod As Double
        Private pulseModulatorDelay As Double
        Private pulseModulatorWidth As Double
        Private pulseAcquisitionAuto As RFmxVnaMXPulseAcquisitionAuto
        Private pulseAcquisitionDelay As Double
        Private pulseAcquisitionWidth As Double
        Private averagingEnabled As RFmxVnaMXAveragingEnabled
        Private averagingCount As Integer
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
            frequencyStart = 1000000000.0                                                                       'Hz
            frequencyEnd = 26000000000.0                                                                        'Hz
            numberOfFrequencyPoints = 251
            port1PowerLevel = -10.0                                                                             '(dBm)
            port2PowerLevel = -10.0                                                                             '(dBm)
            frequencyReferenceSource = RFmxInstrMXConstants.PxiClock
            frequencyReferenceFrequency = 100000000.0                                                           '(Hz)
            numberOfSParams = 4
            sParamsSParameters = New String() {"S11", "S12", "S21", "S22"}
            sParamsFormats = New RFmxVnaMXSParamsFormat() {RFmxVnaMXSParamsFormat.Magnitude,
                RFmxVnaMXSParamsFormat.Magnitude, RFmxVnaMXSParamsFormat.Magnitude, RFmxVnaMXSParamsFormat.Magnitude}
            magnitudeUnits = RFmxVnaMXSParamsMagnitudeUnits.dB
            phaseTraceType = RFmxVnaMXSParamsPhaseTraceType.Wrapped
            pulseModeEnabled = RFmxVnaMXPulseModeEnabled.True
            pulsePeriod = 1.0E-3                                                                                ' seconds
            pulseModulatorDelay = 0.0                                                                           ' seconds
            pulseModulatorWidth = 0.0001                                                                        ' seconds
            pulseAcquisitionAuto = RFmxVnaMXPulseAcquisitionAuto.True
            pulseAcquisitionDelay = 0.00002                                                                     ' seconds
            pulseAcquisitionWidth = 0.00004665                                                                  ' seconds
            averagingEnabled = RFmxVnaMXAveragingEnabled.False
            averagingCount = 10
            timeout = 10.0                                                                                      'seconds
        End Sub

        Private Sub InitializeInstr()
            instrSession = New RFmxInstrMX(resourceName, "")
        End Sub

        Private Sub ConfigureVna()
            vna = instrSession.GetVnaSignalConfiguration()                                                       'Create a new RFmx Session
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)

            vna.SetSweepType("", RFmxVnaMXSweepType.Linear)
            vna.SetStartFrequency("", frequencyStart)
            vna.SetStopFrequency("", frequencyEnd)
            vna.SetNumberOfPoints("", numberOfFrequencyPoints)
            portSelectorString = RFmxVnaMX.BuildPortString("", "port1")
            vna.SetPowerLevel(portSelectorString, port1PowerLevel)
            portSelectorString = RFmxVnaMX.BuildPortString("", "port2")
            vna.SetPowerLevel(portSelectorString, port2PowerLevel)
            vna.SetPulseModeEnabled("", pulseModeEnabled)
            vna.SetPulsePeriod("", pulsePeriod)
            vna.SetPulseModulatorDelay("", pulseModulatorDelay)
            vna.SetPulseModulatorWidth("", pulseModulatorWidth)
            vna.SetPulseAcquisitionAuto("", pulseAcquisitionAuto)
            vna.SetPulseAcquisitionDelay("", pulseAcquisitionDelay)
            vna.SetPulseAcquisitionWidth("", pulseAcquisitionWidth)
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
