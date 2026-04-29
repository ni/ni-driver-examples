'Steps:
'1. Open a New RFmx session.
'2. Configure Frequency Reference.
'3. Configure S-parameter External Attenuation Table (External Fixture's De-embedding Table) from S2P File.  
'4. Configure sweep settings: Sweep Type, Start Frequency, Stop Frequency, Number Of Frequency Points,  If Bandwidth, And Power Level And Test Rx Attenuation With different port names.
'5. Configure Averaging.
'6. Select S-Parameter measurement.
'7. Configure number of S-Parameters.
'8. Configure each S-Parameter And format.
'9. Configure Magnitude Units & Phase Trace Type.
'10. Load Calset data from a file. 
'11. Enable Correction.
'12. Initiate the Measurement after user confirmation.
'13. Read Number of SParams.
'14. Fetch S-Parameter X data.
'15. Fetch S-Parameter Y data for each S-Parameter.
'16. Close RFmx Session.

Imports System.IO
Imports System.Reflection
Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.VnaMX

Namespace NationalInstruments.Examples.RFmxVNAExternalFixturesDeembedding
    Public Class RFmxVNAExternalFixturesDeembedding
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

        Private numberOfExternalFixtures As Integer
        Private portNames As String()
        Private s2pFilePaths As String()
        Private sParameterOrientations As RFmxInstrMXSParameterOrientation()
        Private currentDirectoryPath As String

        Private numberOfSParams As Integer
        Private sParamsSParameters As String()
        Private sParamsFormats As RFmxVnaMXSParamsFormat()

        Private magnitudeUnits As RFmxVnaMXSParamsMagnitudeUnits
        Private phaseTraceType As RFmxVnaMXSParamsPhaseTraceType

        Private averagingEnabled As RFmxVnaMXAveragingEnabled
        Private averagingCount As Integer

        Private calsetFilePath As String

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

            frequencyStart = 1000000000.0                                                                       '(Hz)
            frequencyEnd = 26000000000.0                                                                        '(Hz)
            numberOfFrequencyPoints = 251
            port1PowerLevel = -10.0                                                                             '(dBm)
            port2PowerLevel = -10.0                                                                             '(dBm)
            port1TestReceiverAttenuation = 0.0                                                                  '(dB)
            port2TestReceiverAttenuation = 0.0                                                                  '(dB)
            IFBandwidth = 100000.0                                                                              '(Hz)

            numberOfExternalFixtures = 2
            portNames = New String() {"port1", "port2"}
            s2pFilePaths = New String() {"1dB_Attenuation.s2p", "1dB_Attenuation.s2p"}
            sParameterOrientations = New RFmxInstrMXSParameterOrientation() {RFmxInstrMXSParameterOrientation.Port2TowardsDut, RFmxInstrMXSParameterOrientation.Port2TowardsDut}
            numberOfSParams = 4
            sParamsSParameters = New String() {"S11", "S12", "S21", "S22"}

            sParamsFormats = New RFmxVnaMXSParamsFormat() {RFmxVnaMXSParamsFormat.Magnitude, RFmxVnaMXSParamsFormat.Magnitude, RFmxVnaMXSParamsFormat.Magnitude, RFmxVnaMXSParamsFormat.Magnitude}

            magnitudeUnits = RFmxVnaMXSParamsMagnitudeUnits.dB
            phaseTraceType = RFmxVnaMXSParamsPhaseTraceType.Wrapped

            averagingEnabled = RFmxVnaMXAveragingEnabled.False
            averagingCount = 10

            calsetFilePath = ""

            timeout = 10.0                                                                                      'seconds
        End Sub

        Private Sub InitializeInstr()
            instrSession = New RFmxInstrMX(resourceName, "")
        End Sub

        Private Sub ConfigureVna()
            vna = instrSession.GetVnaSignalConfiguration()                                                      'Create a new RFmx Session
            instrSession.ConfigureFrequencyReference("", RFmxInstrMXConstants.PxiClock, 100000000.0)

            currentDirectoryPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\"
            For i As Integer = 0 To numberOfExternalFixtures - 1
                portSelectorString = RFmxVnaMX.BuildPortString("", portNames(i))
                instrSession.LoadSParameterExternalAttenuationTableFromS2pFile(
                    portSelectorString, "", currentDirectoryPath + s2pFilePaths(i), sParameterOrientations(i))
            Next

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

            vna.CalsetLoadFromFile("", "", calsetFilePath)

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
