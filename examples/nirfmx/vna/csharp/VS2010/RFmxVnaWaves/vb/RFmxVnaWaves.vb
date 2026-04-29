'Steps:
'1. Open a new RFmx session.
'2. Configure Frequency Reference.
'3. Configure Sweep settings: Frequency List, IF Bandwidth, and Power Level and Test Rx Attenuation with different port names.
'4. Configure Averaging.
'5. Select Wave measurement.
'6. Configure Number of Waves.
'7. Configure Source, Reciever Ports and Format for selected Waves.
'8. Configure Magnitude Units & Phase Trace Type.
'9. Initiate the Measurement.
'10. Read the Num Waves.
'11. Fetch Waves X data.
'12. Fetch Waves Y data for each Wave.
'13. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.VnaMX

Namespace NationalInstruments.Examples.RFmxVnaWaves
    Public Class RFmxVnaWaves
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

        Private numberOfWaves As Integer
        Private waves As String()
        Private wavesFormats As RFmxVnaMXWavesFormat()

        Private magnitudeUnits As RFmxVnaMXWavesMagnitudeUnits
        Private phaseTraceType As RFmxVnaMXWavesPhaseTraceType

        Private averagingEnabled As RFmxVnaMXAveragingEnabled
        Private averagingCount As Integer

        Private measurement As RFmxVnaMXMeasurementTypes
        Private enableAllTraces As Boolean

        Private waveSelectorString As String
        Private portSelectorString As String

        Private timeout As Double

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
            frequencyPoints = 10
            port1PowerLevel = -10.0                                                                             '(dBm)
            port2PowerLevel = -10.0                                                                             '(dBm)
            port1TestReceiverAttenuation = 0.0                                                                  '(dB)
            port2TestReceiverAttenuation = 0.0                                                                  '(dB)
            IFBandwidth = 100000.0                                                                                  '(Hz)

            frequencyReferenceSource = RFmxInstrMXConstants.PxiClock
            frequencyReferenceFrequency = 100000000.0                                                           '(Hz)

            numberOfWaves = 4
            waves = New String() {"a1_1", "b1_1", "a1_2", "b1_2"}
            wavesFormats = New RFmxVnaMXWavesFormat() {RFmxVnaMXWavesFormat.Magnitude,
                RFmxVnaMXWavesFormat.Phase, RFmxVnaMXWavesFormat.Magnitude, RFmxVnaMXWavesFormat.Phase}

            magnitudeUnits = RFmxVnaMXWavesMagnitudeUnits.dBm
            phaseTraceType = RFmxVnaMXWavesPhaseTraceType.Wrapped

            averagingEnabled = RFmxVnaMXAveragingEnabled.False
            averagingCount = 10

            measurement = RFmxVnaMXMeasurementTypes.Waves
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
            vna.Waves.Configuration.SetNumberOfWaves("", numberOfWaves)
            For i As Integer = 0 To numberOfWaves - 1
                waveSelectorString = RFmxVnaMX.BuildWaveString("", i)
                vna.Waves.Configuration.ConfigureWave(waveSelectorString, waves(i))
                vna.Waves.Configuration.SetFormat(waveSelectorString, wavesFormats(i))
            Next
            vna.Waves.Configuration.SetMagnitudeUnits("", magnitudeUnits)
            vna.Waves.Configuration.SetPhaseTraceType("", phaseTraceType)
            vna.Initiate("", "")
        End Sub

        Private Sub RetrieveResults()
            vna.Waves.Configuration.GetNumberOfWaves("", numberOfWavesResult)
            vna.Waves.Results.FetchXData("", timeout, wavesXDataResult)
            wavesY1DataResult = New Single(numberOfWavesResult)() {}
            wavesY2DataResult = New Single(numberOfWavesResult)() {}

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
            Console.WriteLine("ERROR:" & vbLf & Convert.ToString(ex.[GetType]()) & ": " + ex.Message)
        End Sub
    End Class
End Namespace
