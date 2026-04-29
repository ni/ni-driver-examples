'Steps:
'1.Open a new RFmx session.
'2. Configure Frequency Reference.
'3. Configure sweep settings: Sweep Type, Start Frequency, Stop Frequency, Number of Frequency Points, IF Bandwidth, and Power Level and Test Rx Attenuation with different port names.
'4. Configure Averaging.
'5. Select S-Parameter and Waves measurements.
'6. Configure number of S-Parameters.
'7. Configure each S-Parameter and Format.
'8. Configure Magnitude Units & Phase Trace Type.
'9. Configure Number of Waves.
'10. Configure each Wave and Format
'11. Configure Magnitude Units & Phase Trace Type
'12. Initiate the Measurement.
'13. Read Number of SParams and X-Axis Values (aggregated frequency list).
'14.Fetch S - Parameter X data.
'15.Fetch S - Parameter Y data for each S-Parameter.
'16. Read the Num Waves.
'17. Fetch Waves X data.
'18. Fetch Waves Y Data for each Wave.
'9. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.VnaMX

Namespace NationalInstruments.Examples.RFmxVnaSparamsWavesComposite
    Public Class RFmxVnaSparamsWavesComposite
        Private instrSession As RFmxInstrMX
        Private vna As RFmxVnaMX
        Private resourceName As String

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

        Private averagingEnabled As RFmxVnaMXAveragingEnabled
        Private averagingCount As Integer

        Private numberOfSParams As Integer
        Private sParamsSParameters As String()
        Private sParamsFormats As RFmxVnaMXSParamsFormat()

        Private sParamsMagnitudeUnits As RFmxVnaMXSParamsMagnitudeUnits
        Private sParamsPhaseTraceType As RFmxVnaMXSParamsPhaseTraceType

        Private numberOfWaves As Integer
        Private waves As String()
        Private wavesFormats As RFmxVnaMXWavesFormat()

        Private wavesMagnitudeUnits As RFmxVnaMXWavesMagnitudeUnits
        Private wavesPhaseTraceType As RFmxVnaMXWavesPhaseTraceType

        Private sParamSelectorString As String
        Private waveSelectorString As String
        Private portSelectorString As String

        Private timeout As Double

        Private numberOfSParamsResult As Integer
        Private sParamsXDataResult As Double()
        Private sParamsY1DataResult As Single()()
        Private sParamsY2DataResult As Single()()

        Private numberOfWavesResult As Integer
        Private wavesXDataResult As Double()
        Private wavesY1DataResult As Single()()
        Private wavesY2DataResult As Single()()

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
            frequencyPoints = 251
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

            averagingEnabled = RFmxVnaMXAveragingEnabled.[False]
            averagingCount = 10

            numberOfSParams = 4
            sParamsSParameters = New String() {"S11", "S12", "S21", "S22"}
            sParamsFormats = New RFmxVnaMXSParamsFormat() {RFmxVnaMXSParamsFormat.Magnitude, RFmxVnaMXSParamsFormat.Magnitude, RFmxVnaMXSParamsFormat.Magnitude, RFmxVnaMXSParamsFormat.Magnitude}

            sParamsMagnitudeUnits = RFmxVnaMXSParamsMagnitudeUnits.dB
            sParamsPhaseTraceType = RFmxVnaMXSParamsPhaseTraceType.Wrapped

            numberOfWaves = 4
            waves = New String() {"a1_1", "b1_1", "a1_2", "b1_2"}
            wavesFormats = New RFmxVnaMXWavesFormat() {RFmxVnaMXWavesFormat.Magnitude, RFmxVnaMXWavesFormat.Phase, RFmxVnaMXWavesFormat.Magnitude, RFmxVnaMXWavesFormat.Phase}

            wavesMagnitudeUnits = RFmxVnaMXWavesMagnitudeUnits.dBm
            wavesPhaseTraceType = RFmxVnaMXWavesPhaseTraceType.Wrapped

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
            vna.SetSweepType("", RFmxVnaMXSweepType.Linear)
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
            vna.SelectMeasurements("", RFmxVnaMXMeasurementTypes.SParams Or RFmxVnaMXMeasurementTypes.Waves, False)
            vna.SParams.Configuration.SetNumberOfSParameters("", numberOfSParams)
            For i As Integer = 0 To numberOfSParams - 1
                sParamSelectorString = RFmxVnaMX.BuildSParameterString("", i)
                vna.SParams.Configuration.ConfigureSParameter(sParamSelectorString, sParamsSParameters(i))
                vna.SParams.Configuration.SetFormat(sParamSelectorString, sParamsFormats(i))
            Next
            vna.SParams.Configuration.SetMagnitudeUnits("", sParamsMagnitudeUnits)
            vna.SParams.Configuration.SetPhaseTraceType("", sParamsPhaseTraceType)

            vna.Waves.Configuration.SetNumberOfWaves("", numberOfWaves)
            For i As Integer = 0 To numberOfWaves - 1
                waveSelectorString = RFmxVnaMX.BuildWaveString("", i)
                vna.Waves.Configuration.ConfigureWave(waveSelectorString, waves(i))
                vna.Waves.Configuration.SetFormat(waveSelectorString, wavesFormats(i))
            Next
            vna.Waves.Configuration.SetMagnitudeUnits("", wavesMagnitudeUnits)
            vna.Waves.Configuration.SetPhaseTraceType("", wavesPhaseTraceType)
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

            vna.Waves.Configuration.GetNumberOfWaves("", numberOfWavesResult)
            vna.Waves.Results.FetchXData("", timeout, wavesXDataResult)
            wavesY1DataResult = New Single(numberOfWavesResult - 1)() {}
            wavesY2DataResult = New Single(numberOfWavesResult - 1)() {}
            For i As Integer = 0 To numberOfWavesResult - 1
                waveSelectorString = RFmxVnaMX.BuildWaveString("", i)
                vna.Waves.Results.FetchYData(waveSelectorString, timeout, wavesY1DataResult(i), wavesY2DataResult(i))
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
            Console.WriteLine("ERROR:" & vbLf & Convert.ToString(ex.[GetType]()) & ": " & ex.Message)
        End Sub
    End Class
End Namespace

