'1. Open a new RFmx session.
'2. Configure Frequency Reference.
'3. Configure sweep settings: Sweep Type, Start Frequency, Stop Frequency, Number of Frequency Points, IF Bandwidth, and Power Level and Test Rx Attenuation with different port names.
'4. Configure Averaging.
'5. Select S-Parameter measurement.
'6. Configure S-Parameter and format for selected S-Parameters.
'7. Configure Magnitude Units & Phase Trace Type.
'8. Initiate the Measurement.
'9. Copy Measurement Data to Memory and Get Memory Data.
'10. Configure Math Function.
'11. Initiate the Measurement for Applying Math Function.
'12. Read X-Axis Values (aggregated frequency list).
'13. Fetch Math Applied S-Parameter Y data.
'14. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.VnaMX

Namespace NationalInstruments.Examples.RFmxVnaSParamsTraceMath
    Public Class RFmxVnaSParamsTraceMath
        Private instrSession As RFmxInstrMX
        Private vna As RFmxVnaMX
        Private resourceName As String

        Private startFrequency As Double
        Private stopFrequency As Double
        Private numberOfPoints As Integer
        Private port1PowerLevel As Double
        Private port2PowerLevel As Double
        Private port1TestReceiverAttenuation As Double
        Private port2TestReceiverAttenuation As Double
        Private IFBandwidth As Double

        Private sParamsSParameters As String
        Private sParamsFormats As RFmxVnaMXSParamsFormat

        Private sParamsMemoryName As String
        Private sParamsMathFunction As RFmxVnaMXSParamsMathFunction

        Private averagingEnabled As RFmxVnaMXAveragingEnabled
        Private averagingCount As Integer

        Private timeout As Double

        Private portSelectorString As String
        Private sParamSelectorString As String
        Private measurementMemorySelectorString As String

        Private sParamsXDataResult As Double()
        Private sParamsY1DataResult As Single()
        Private sParamsY2DataResult As Single()
        Private sParamsMemoryXDataResult As Double()
        Private sParamsMemoryY1DataResult As Single()
        Private sParamsMemoryY2DataResult As Single()

        Public Sub Run()
            Try
                InitializeVariables()
                InitializeInstr()
                ConfigureVna()
                RetrieveResults()
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

            startFrequency = 1000000000.0
            ' (Hz) 
            stopFrequency = 26000000000.0
            ' (Hz) 
            numberOfPoints = 251
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

            sParamsSParameters = "S11"
            sParamsFormats = RFmxVnaMXSParamsFormat.Magnitude

            sParamsMemoryName = "Memory0"
            sParamsMathFunction = RFmxVnaMXSParamsMathFunction.Divide

            averagingEnabled = RFmxVnaMXAveragingEnabled.[False]
            averagingCount = 10

            timeout = 10.0
            'seconds 
        End Sub

        Private Sub InitializeInstr()
            instrSession = New RFmxInstrMX(resourceName, "")
        End Sub

        Private Sub ConfigureVna()
            vna = instrSession.GetVnaSignalConfiguration()
            ' Create a new RFmx Session 
            instrSession.ConfigureFrequencyReference("", RFmxInstrMXConstants.PxiClock, 100000000.0)
            vna.SetSweepType("", RFmxVnaMXSweepType.Linear)
            vna.SetStartFrequency("", startFrequency)
            vna.SetStopFrequency("", stopFrequency)
            vna.SetNumberOfPoints("", numberOfPoints)
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

            sParamSelectorString = RFmxVnaMX.BuildSParameterString("", 0)
            vna.SParams.Configuration.ConfigureSParameter(sParamSelectorString, sParamsSParameters)
            vna.SParams.Configuration.SetFormat(sParamSelectorString, sParamsFormats)

            vna.SParams.Configuration.SetMagnitudeUnits("", RFmxVnaMXSParamsMagnitudeUnits.dB)
            vna.SParams.Configuration.SetPhaseTraceType("", RFmxVnaMXSParamsPhaseTraceType.Wrapped)
            vna.Initiate("", "")
        End Sub

        Private Sub RetrieveResults()

            vna.CopyDataToMeasurementMemory(sParamSelectorString, sParamsMemoryName)
            measurementMemorySelectorString = RFmxVnaMX.BuildMeasurementMemoryString(sParamSelectorString, sParamsMemoryName)
            vna.GetMeasurementMemoryXData(measurementMemorySelectorString, sParamsMemoryXDataResult)
            vna.GetMeasurementMemoryYData(measurementMemorySelectorString, sParamsMemoryY1DataResult, sParamsMemoryY2DataResult)
            vna.SParams.Configuration.SetMathFunction(sParamSelectorString, sParamsMathFunction)

            vna.Initiate("", "")
            vna.SParams.Results.FetchXData("", timeout, sParamsXDataResult)
            vna.SParams.Results.FetchYData(sParamSelectorString, timeout, sParamsY1DataResult, sParamsY2DataResult)
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

