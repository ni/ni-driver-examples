'Steps:
'1. Open a new RFmx session.
'2. Configure Frequency Reference.
'3. Configure sweep settings: Frequency List, IF Bandwidth, and Power Level and Test Rx Attenuation with different port names.
'4. Configure Averaging.
'5. Select S-Parameter measurement.
'6. Configure number of S-Parameters.
'7. Configure each S-Parameter and format.
'8. Configure Magnitude Units & Phase Trace Type.
'9. Initiate the Measurement.
'10. Read Num SParams.
'11. Fetch S-Parameter X data.
'12. Fetch S-Parameter Y data for each S-Parameter.
'13. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.VnaMX

Namespace NationalInstruments.Examples.RFmxVnaSParams
    Public Class RFmxVnaSParams
        Private instrSession As RFmxInstrMX
        Private vna As RFmxVnaMX
        Private resourceName As String

        Private sweepType As RFmxVnaMXSweepType
        Private startFrequency As Double
        Private stopFrequency As Double
        Private frequencyPoints As Integer
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

        Private averagingEnabled As RFmxVnaMXAveragingEnabled
        Private averagingCount As Integer

        Private measurement As RFmxVnaMXMeasurementTypes
        Private enableAllTraces As Boolean

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

            sweepType = RFmxVnaMXSweepType.Linear
            startFrequency = 1000000000.0                                                                       '(Hz)
            stopFrequency = 10000000000.0                                                                       '(Hz)
            frequencyPoints = 10                         '(Hz)
            port1PowerLevel = -10.0                                                                             '(dBm)
            port2PowerLevel = -10.0                                                                             '(dBm)
            port1TestReceiverAttenuation = 0.0                                                                  '(dB)
            port2TestReceiverAttenuation = 0.0                                                                  '(dB)
            IFBandwidth = 100000.0                                                                                  '(Hz)

            frequencyReferenceSource = RFmxInstrMXConstants.PxiClock
            frequencyReferenceFrequency = 100000000.0                                                           '(Hz)

            numberOfSParams = 4
            sParamsSParameters = New String() {"S11", "S12", "S21", "S22"}
            sParamsFormats = New RFmxVnaMXSParamsFormat() {RFmxVnaMXSParamsFormat.Magnitude,
                RFmxVnaMXSParamsFormat.Magnitude, RFmxVnaMXSParamsFormat.Magnitude, RFmxVnaMXSParamsFormat.Magnitude}

            magnitudeUnits = RFmxVnaMXSParamsMagnitudeUnits.dB
            phaseTraceType = RFmxVnaMXSParamsPhaseTraceType.Wrapped

            averagingEnabled = RFmxVnaMXAveragingEnabled.False
            averagingCount = 10

            measurement = RFmxVnaMXMeasurementTypes.SParams
            enableAllTraces = False

            timeout = 10.0                                                                                      'seconds
        End Sub

        Private Sub InitializeInstr()
            instrSession = New RFmxInstrMX(resourceName, "")
        End Sub

        Private Sub ConfigureVna()
            vna = instrSession.GetVnaSignalConfiguration()                                                      'Create a new RFmx Session
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
            vna.SetSweepType("", sweepType)
            vna.SetStartFrequency("", startFrequency)
            vna.SetStopFrequency("", stopFrequency)
            vna.SetNumberOfPoints("", frequencyPoints)
            vna.SetIFBandwidth("", IFBandwidth)
            portSelectorString = RFmxVnaMX.BuildPortString("", "port1")
            vna.SetPowerLevel(portSelectorString, port1PowerLevel)
            vna.SetTestReceiverAttenuation(portSelectorString, port1TestReceiverAttenuation)
            portSelectorString = RFmxVnaMX.BuildPortString("", "port2")
            vna.SetPowerLevel(portSelectorString, port2PowerLevel)
            vna.SetTestReceiverAttenuation(portSelectorString, port2TestReceiverAttenuation)
            vna.SetAveragingEnabled("", averagingEnabled)
            vna.SetAveragingCount("", averagingCount)
            vna.SelectMeasurements("", measurement, enableAllTraces)
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
