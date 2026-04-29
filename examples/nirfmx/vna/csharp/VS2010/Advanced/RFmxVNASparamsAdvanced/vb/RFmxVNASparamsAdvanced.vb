'Steps:
'1. Open a New RFmx session.
'2. Configure Frequency Reference.
'3. Configure S-parameter External Attenuation Table (External Fixture's De-embedding Table) from S2P File.
'4. Configure Port Extension.  
'5 & 6. Configure sweep settings: Start Frequency, Stop Frequency, Number Of Frequency Points, If Bandwidth, And Power Level And Test Rx Attenuation With different port names.
'7. Configure Averaging.
'8. Configure Trigger.
'9. Select S-Parameter measurement.
'10. Configure number of S-Parameters.
'11. Configure S-Parameter And format.
'12. Configure Magnitude Units, Phase Trace Type & Group Delay Aperture Settings.
'13. Load Calset data from a file. 
'14. Enable Correction, Configure Interpolation Enabled and Configure correction port subset settings.
'15. Initiate the Measurement after user confirmation.
'16. Read Number of SParams.
'17. Fetch S-Parameter X data.
'18. Fetch S-Parameter Y data for each S-Parameter.
'19. Fetch S-Parameter Correction Level.
'20. Fetch S-Parameter Correction State.
'21. Set SnP Export attributes (can be accessed And written before Or after measurement initiate) And save S-Parameter data to file.
'22. Close RFmx Session.

Imports System.IO
Imports System.Reflection
Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.VnaMX

Namespace NationalInstruments.Examples.RFmxVNASparamsAdvanced
    Public Class RFmxVnaSParamsAdvanced
        Private instrSession As RFmxInstrMX
        Private vna As RFmxVnaMX
        Private resourceName As String

        Private frequencyList As Double()
        Private frequencyListSize As Integer
        Private frequencyStep As Double
        Private frequencyStart As Double
        Private frequencyStop As Double
        Private numberOfFrequencyPoints As Integer
        Private sweepType As RFmxVnaMXSweepType
        Private port1PowerLevel As Double
        Private port2PowerLevel As Double
        Private port1TestReceiverAttenuation As Double
        Private port2TestReceiverAttenuation As Double
        Private IFBandwidth As Double

        Private frequencyReferenceSource As String
        Private frequencyReferenceFrequency As Double

        Private portNames As String()
        Private portExtensionEnabled As RFmxVnaMXCorrectionPortExtensionEnabled
        Private portExtensionDelayDomain As RFmxVnaMXCorrectionPortExtensionDelayDomain
        Private portExtensionDelay As Double
        Private portExtensionDistance As Double
        Private portExtensionDistanceUnit As RFmxVnaMXCorrectionPortExtensionDistanceUnit
        Private portExtensionVelocityFactor As Double
        Private PortExtensionDCLossEnabled As RFmxVnaMXCorrectionPortExtensionDCLossEnabled
        Private portExtensionDCLoss As Double
        Private numberOfPortExtension As Integer
        Private portExtensionLoss1Enabled As RFmxVnaMXCorrectionPortExtensionLoss1Enabled
        Private portExtensionLoss2Enabled As RFmxVnaMXCorrectionPortExtensionLoss2Enabled
        Private portExtensionLoss1Frequency As Double
        Private portExtensionLoss2Frequency As Double
        Private portExtensionLoss1 As Double
        Private portExtensionLoss2 As Double

        Private numberOfExternalFixtures As Integer
        Private s2pFilePaths As String()
        Private sParameterOrientations As RFmxInstrMXSParameterOrientation()
        Private currentDirectoryPath As String

        Private numberOfSParams As Integer
        Private sParamsSParameters As String()
        Private sParamsFormats As RFmxVnaMXSParamsFormat()

        Private magnitudeUnits As RFmxVnaMXSParamsMagnitudeUnits
        Private phaseTraceType As RFmxVnaMXSParamsPhaseTraceType
        Private groupDelayApertureMode As RFmxVnaMXSParamsGroupDelayApertureMode
        Private groupDelayAperturePoints As Double
        Private groupDelayAperturePercentage As Double
        Private groupDelayApertureFrequencySpan As Double

        Private snPFilePath As String

        Private calsetFilePath As String
        Private interpolationEnabled As RFmxVnaMXCorrectionInterpolationEnabled
        Private portSubsetEnabled As RFmxVnaMXCorrectionPortSubsetEnabled

        Private averagingEnabled As RFmxVnaMXAveragingEnabled
        Private averagingCount As Integer

        Private triggerType As RFmxVnaMXTriggerType
        Private triggerMode As RFmxVnaMXTriggerMode
        Private triggerDelay As Double

        Private measurement As RFmxVnaMXMeasurementTypes
        Private enableAllTraces As Boolean

        Private sParamSelectorString As String
        Private portSelectorString As String
        Private correctionLevelResult As String


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

            frequencyList = New Double() {}
            frequencyListSize = 251
            frequencyStep = 100000000.0                                                                         '(Hz)
            port1PowerLevel = -10.0                                                                             '(dBm)
            port2PowerLevel = -10.0                                                                             '(dBm)
            port1TestReceiverAttenuation = 0.0                                                                  '(dB)
            port2TestReceiverAttenuation = 0.0                                                                  '(dB)
            IFBandwidth = 100000.0                                                                              '(Hz)

            frequencyReferenceSource = RFmxInstrMXConstants.PxiClock
            frequencyReferenceFrequency = 100000000.0                                                           '(Hz)
            frequencyStart = 1000000000.0                                                                       '(Hz)
            frequencyStop = 26000000000.0                                                                       '(Hz)
            numberOfFrequencyPoints = 251
            sweepType = RFmxVnaMXSweepType.Linear
            portNames = New String() {"port1", "port2"}
            portExtensionEnabled = RFmxVnaMXCorrectionPortExtensionEnabled.False
            portExtensionDelayDomain = RFmxVnaMXCorrectionPortExtensionDelayDomain.Delay
            numberOfPortExtension = 2
            portExtensionDelay = 0.0000000001                                                                   '(s)
            portExtensionDistance = 0.0299792
            portExtensionDistanceUnit = RFmxVnaMXCorrectionPortExtensionDistanceUnit.Meters
            portExtensionVelocityFactor = 1.0
            PortExtensionDCLossEnabled = RFmxVnaMXCorrectionPortExtensionDCLossEnabled.False
            portExtensionDCLoss = 0.0
            portExtensionLoss1Enabled = RFmxVnaMXCorrectionPortExtensionLoss1Enabled.False
            portExtensionLoss2Enabled = RFmxVnaMXCorrectionPortExtensionLoss2Enabled.False
            portExtensionLoss1Frequency = 0.0                                                                   '(Hz)
            portExtensionLoss2Frequency = 0.0                                                                   '(Hz)
            portExtensionLoss1 = 0.0                                                                            '(dB)
            portExtensionLoss2 = 0.0                                                                            '(dB)

            numberOfExternalFixtures = 2

            s2pFilePaths = New String() {"1dB_Attenuation.s2p", "1dB_Attenuation.s2p"}
            sParameterOrientations = New RFmxInstrMXSParameterOrientation() {RFmxInstrMXSParameterOrientation.Port2TowardsDut, RFmxInstrMXSParameterOrientation.Port2TowardsDut}

            numberOfSParams = 4
            sParamsSParameters = New String() {"S11", "S12", "S21", "S22"}
            sParamsFormats = New RFmxVnaMXSParamsFormat() {RFmxVnaMXSParamsFormat.Magnitude,
                RFmxVnaMXSParamsFormat.Magnitude, RFmxVnaMXSParamsFormat.Magnitude, RFmxVnaMXSParamsFormat.Magnitude}

            magnitudeUnits = RFmxVnaMXSParamsMagnitudeUnits.dB
            phaseTraceType = RFmxVnaMXSParamsPhaseTraceType.Wrapped
            groupDelayApertureMode = RFmxVnaMXSParamsGroupDelayApertureMode.Points
            groupDelayAperturePoints = 11.0
            groupDelayAperturePercentage = 4.0                                                                    '(%)
            groupDelayApertureFrequencySpan = 1000000000.0                                                        '(Hz)

            snPFilePath = ""

            calsetFilePath = ""
            interpolationEnabled = RFmxVnaMXCorrectionInterpolationEnabled.True
            portSubsetEnabled = RFmxVnaMXCorrectionPortSubsetEnabled.False


            averagingEnabled = RFmxVnaMXAveragingEnabled.False
            averagingCount = 10

            triggerType = RFmxVnaMXTriggerType.None
            triggerMode = RFmxVnaMXTriggerMode.Signal
            triggerDelay = 0.0                                                                                  '(s)

            measurement = RFmxVnaMXMeasurementTypes.SParams
            enableAllTraces = False

            timeout = 10.0                                                                                      '(s)
        End Sub

        Private Sub InitializeInstr()
            instrSession = New RFmxInstrMX(resourceName, "")
        End Sub

        Private Sub ConfigureVna()
            vna = instrSession.GetVnaSignalConfiguration() ' Create a new RFmx Session
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)

            For i As Integer = 0 To numberOfExternalFixtures - 1
                currentDirectoryPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\"
                portSelectorString = RFmxVnaMX.BuildPortString("", portNames(i))
                instrSession.LoadSParameterExternalAttenuationTableFromS2pFile(portSelectorString, "", currentDirectoryPath + s2pFilePaths(i), sParameterOrientations(i))
            Next
            For i As Integer = 0 To numberOfPortExtension - 1
                portSelectorString = RFmxVnaMX.BuildPortString("", portNames(i))
                vna.SetCorrectionPortExtensionEnabled(portSelectorString, portExtensionEnabled)
                vna.SetCorrectionPortExtensionDelayDomain(portSelectorString, portExtensionDelayDomain)
                vna.SetCorrectionPortExtensionDelay(portSelectorString, portExtensionDelay)
                vna.SetCorrectionPortExtensionDistance(portSelectorString, portExtensionDistance)
                vna.SetCorrectionPortExtensionDistanceUnit(portSelectorString, portExtensionDistanceUnit)
                vna.SetCorrectionPortExtensionVelocityFactor(portSelectorString, portExtensionVelocityFactor)
                vna.SetCorrectionPortExtensionDCLossEnabled(portSelectorString, PortExtensionDCLossEnabled)
                vna.SetCorrectionPortExtensionLossDCLoss(portSelectorString, portExtensionDCLoss)
                vna.SetCorrectionPortExtensionLoss1Enabled(portSelectorString, portExtensionLoss1Enabled)
                vna.SetCorrectionPortExtensionLoss2Enabled(portSelectorString, portExtensionLoss2Enabled)
                vna.SetCorrectionPortExtensionLoss1Frequency(portSelectorString, portExtensionLoss1Frequency)
                vna.SetCorrectionPortExtensionLoss2Frequency(portSelectorString, portExtensionLoss2Frequency)
                vna.SetCorrectionPortExtensionLoss1(portSelectorString, portExtensionLoss1)
                vna.SetCorrectionPortExtensionLoss2(portSelectorString, portExtensionLoss2)
            Next
            Select Case sweepType
                Case RFmxVnaMXSweepType.Linear
                    vna.SetSweepType("", sweepType)
                    vna.SetStartFrequency("", frequencyStart)
                    vna.SetStopFrequency("", frequencyStop)
                    vna.SetNumberOfPoints("", numberOfFrequencyPoints)
                Case RFmxVnaMXSweepType.List
                    vna.SetSweepType("", sweepType)
                    For i As Integer = 0 To frequencyListSize
                        frequencyList(i) = (frequencyStart + i * frequencyStep)
                    Next
                    vna.SetFrequencyList("", frequencyList)
                Case Else
            End Select

            vna.SetIFBandwidth("", IFBandwidth)
            portSelectorString = RFmxVnaMX.BuildPortString("", "port1")
            vna.SetPowerLevel(portSelectorString, port1PowerLevel)
            vna.SetTestReceiverAttenuation(portSelectorString, port1TestReceiverAttenuation)
            portSelectorString = RFmxVnaMX.BuildPortString("", "port2")
            vna.SetPowerLevel(portSelectorString, port2PowerLevel)
            vna.SetTestReceiverAttenuation(portSelectorString, port2TestReceiverAttenuation)
            vna.SetAveragingEnabled("", averagingEnabled)
            vna.SetAveragingCount("", averagingCount)
            vna.SetTriggerType("", triggerType)
            vna.SetTriggerMode("", triggerMode)
            vna.SetTriggerDelay("", triggerDelay)
            vna.SelectMeasurements("", measurement, enableAllTraces)
            vna.SParams.Configuration.SetNumberOfSParameters("", numberOfSParams)
            For i As Integer = 0 To numberOfSParams - 1
                sParamSelectorString = RFmxVnaMX.BuildSParameterString("", i)
                vna.SParams.Configuration.ConfigureSParameter(sParamSelectorString, sParamsSParameters(i))
                vna.SParams.Configuration.SetFormat(sParamSelectorString, sParamsFormats(i))
            Next
            vna.SParams.Configuration.SetMagnitudeUnits("", magnitudeUnits)
            vna.SParams.Configuration.SetPhaseTraceType("", phaseTraceType)
            vna.SParams.Configuration.SetGroupDelayApertureMode("", groupDelayApertureMode)
            vna.SParams.Configuration.SetGroupDelayAperturePoints("", groupDelayAperturePoints)
            vna.SParams.Configuration.SetGroupDelayAperturePercentage("", groupDelayAperturePercentage)
            vna.SParams.Configuration.SetGroupDelayApertureFrequencySpan("", groupDelayApertureFrequencySpan)
            vna.CalsetLoadFromFile("", "", calsetFilePath)
            vna.SetCorrectionEnabled("", RFmxVnaMXCorrectionEnabled.True)
            vna.SetCorrectionInterpolationEnabled("", interpolationEnabled)
            vna.SetCorrectionPortSubsetEnabled("", portSubsetEnabled)
            vna.SetCorrectionPortSubsetFullPorts("", "port1,port2")
            vna.SetCorrectionPortSubsetResponsePorts("", "")
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
                vna.SParams.Results.GetCorrectionLevel(sParamSelectorString, correctionLevelResult)
            Next
            vna.SParams.Results.GetCorrectionState("", correctionStateResult)
            vna.SParams.Configuration.SetSnPDataFormat("", RFmxVnaMXSParamsSnPDataFormat.Auto)
            vna.SParams.Configuration.SetSnPPorts("", "port1,port2")
            vna.SParams.Configuration.ExportToSnPFile("", snPFilePath)
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
