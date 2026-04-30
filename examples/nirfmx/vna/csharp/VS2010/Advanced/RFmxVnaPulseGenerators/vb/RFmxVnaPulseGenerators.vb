'Steps:
'1. Open a New RFmx session.
'2. Configure Frequency Reference.
'3. Configure sweep settings: Sweep Type, Start Frequency, Stop Frequency, Number Of Frequency Points, If Bandwidth, And Power Level With different port names.
'4. Configure Pulse Settings.
'5. Configure Pulse Generator Settings.
'6. Configure Averaging,
'7. Select S-Parameter measurement.
'8. Configure number of S-Parameters.
'9. Configure each S-Parameter And format.
'10. Initiate the Measurement.
''11. Read Number of SParams.
'12. Fetch S-Parameter X data.
'13. Fetch S-Parameter Y data for each S-Parameter.
'14. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.VnaMX

Namespace NationalInstruments.Examples.RFmxVnaPulseGenerators
    Public Class RFmxVnaPulseGenerators
        Private instrSession As RFmxInstrMX
        Private vna As RFmxVnaMX
        Private resourceName As String
        Private frequencyStart As Double
        Private frequencyStop As Double
        Private numberOfFrequencyPoints As Integer
        Private port1PowerLevel As Double
        Private port2PowerLevel As Double
        Private frequencyReferenceSource As String
        Private frequencyReferenceFrequency As Double
        Private numberOfSParams As Integer
        Private sParamsSParameters As String()
        Private sParamsFormats As RFmxVnaMXSParamsFormat()
        Private pulseModeEnabled As RFmxVnaMXPulseModeEnabled
        Private pulsePeriod As Double
        Private pulseModulatorDelay As Double
        Private pulseModulatorWidth As Double
        Private pulseAcquisitionAuto As RFmxVnaMXPulseAcquisitionAuto
        Private pulseAcquisitionDelay As Double
        Private pulseAcquisitionWidth As Double
        Private numberOfPulseGenerators As Integer
        Private pulseGeneratorEnabled As RFmxVnaMXPulseGeneratorEnabled()
        Private pulseGeneratorExportOutputTerminal As String()
        Private pulseGeneratorDelay As Double()
        Private pulseGeneratorWidth As Double()
        Private averagingEnabled As RFmxVnaMXAveragingEnabled
        Private averagingCount As Integer
        Private sParamSelectorString As String
        Private portSelectorString As String
        Private pulseGeneratorSelectorString As String
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
            frequencyStop = 1000000000.0                                                                        'Hz
            numberOfFrequencyPoints = 1
            port1PowerLevel = -10.0                                                                             '(dBm)
            port2PowerLevel = -10.0                                                                             '(dBm)
            frequencyReferenceSource = RFmxInstrMXConstants.PxiClock
            frequencyReferenceFrequency = 100000000.0                                                           '(Hz)
            numberOfSParams = 1
            numberOfPulseGenerators = 4
            sParamsSParameters = New String() {"S11"}
            sParamsFormats = New RFmxVnaMXSParamsFormat() {RFmxVnaMXSParamsFormat.Magnitude}
            pulseModeEnabled = RFmxVnaMXPulseModeEnabled.True
            pulsePeriod = 0.001                                                                                 'seconds
            pulseModulatorDelay = 0.0                                                                           'seconds
            pulseModulatorWidth = 0.0001                                                                        'seconds
            pulseAcquisitionAuto = RFmxVnaMXPulseAcquisitionAuto.True
            pulseAcquisitionDelay = 0.00004665                                                                  'seconds
            pulseAcquisitionWidth = 0.00002                                                                     'seconds
            pulseGeneratorEnabled = New RFmxVnaMXPulseGeneratorEnabled() {
                RFmxVnaMXPulseGeneratorEnabled.True,
                RFmxVnaMXPulseGeneratorEnabled.False,
                RFmxVnaMXPulseGeneratorEnabled.False,
                RFmxVnaMXPulseGeneratorEnabled.False}
            pulseGeneratorExportOutputTerminal = New String() {
                RFmxInstrMXConstants.Pfi0,
                RFmxInstrMXConstants.DoNotExportSignal,
                RFmxInstrMXConstants.DoNotExportSignal,
                RFmxInstrMXConstants.DoNotExportSignal}
            pulseGeneratorDelay = {0, 0, 0, 0}                                                                  'seconds
            pulseGeneratorWidth = {0.0001, 0.0001, 0.0001, 0.0001}                                              'seconds
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
            vna.SetStartFrequency("", frequencyStart)
            vna.SetStopFrequency("", frequencyStop)
            vna.SetNumberOfPoints("", numberOfFrequencyPoints)
            vna.SetSweepType("", RFmxVnaMXSweepType.Linear)
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
            For i As Integer = 0 To numberOfPulseGenerators - 1
                pulseGeneratorSelectorString = RFmxVnaMX.BuildPulseGeneratorString("", i)
                vna.SetPulseGeneratorEnabled(pulseGeneratorSelectorString, pulseGeneratorEnabled(i))
                vna.SetPulseGeneratorExportOutputTerminal(pulseGeneratorSelectorString, pulseGeneratorExportOutputTerminal(i))
                vna.SetPulseGeneratorDelay(pulseGeneratorSelectorString, pulseGeneratorDelay(i))
                vna.SetPulseGeneratorWidth(pulseGeneratorSelectorString, pulseGeneratorWidth(i))
            Next
            vna.SetAveragingEnabled("", averagingEnabled)
            vna.SetAveragingCount("", averagingCount)
            vna.SelectMeasurements("", RFmxVnaMXMeasurementTypes.SParams, False)
            vna.SParams.Configuration.SetNumberOfSParameters("", numberOfSParams)
            For i As Integer = 0 To numberOfSParams - 1
                sParamSelectorString = RFmxVnaMX.BuildSParameterString("", i)
                vna.SParams.Configuration.ConfigureSParameter(sParamSelectorString, sParamsSParameters(i))
                vna.SParams.Configuration.SetFormat(sParamSelectorString, sParamsFormats(i))
            Next
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
