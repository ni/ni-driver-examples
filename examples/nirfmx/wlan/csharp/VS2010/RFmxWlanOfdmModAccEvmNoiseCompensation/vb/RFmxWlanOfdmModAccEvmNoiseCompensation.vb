'Instructions:
'1. This example demonstrates the use of RFmxWLAN OFDM ModAcc measurement to compute EVM after compensating for the noise attributed to the VSA.
'2. The example uses an enum control, "Calibrate Noise Floor" with two values :
'Disabled(0) : select this to skip calibrating the VSA noise floor, and perform the OFDMModAcc measurement directly.You may want to do this when the VSA noise floor has already been calibrated or when Noise Compensation is disabled.
'Enabled(1) : select this to first calibrate the VSA noise floor, and then perform the OFDMModAcc measurement.
'
'Follow these steps to calibrate VSA noise, and then perform ModAcc measurement :
'1. Set Calibrate Noise Floor to "Enabled".
'2. Run the example.
'3. When "Turn OFF Generation" dialog box appears, ensure that signal generation is turned OFF and then click "OK". Wait for calibration to complete.
'4. When "Turn ON Generation" dialog box appears, ensure that signal generation is turned ON and then click "OK".
'
'Follow these steps to skip(re)calibrating and directly perform ModAcc measurement :
'1. Set Calibrate Noise Floor to "Disabled".
'2. Run the example

'Steps:
'1. Open a new RFmx session.
'2. Configure the frequency reference properties(Clock Source and Clock Frequency).
'3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
'4. Configure IQ Power Edge Trigger properties (Trigger Delay, IQ Power Edge Level, Minimum Quiet Time).
'5. Configure Standard and Channel Bandwidth.
'6. Select OFDMModAcc measurement and enable the traces.
'7. Configure Optimize Dynamic Range for EVM.
'8. Configure Measurement Mode as Calibrate Noise Floor.
'9. Initiate Measurement.
'10. Wait for Measurement Complete.
'11. Configure Measurement Mode as Measure.
'12. Configure Measurement Interval.
'13. Configure Averaging parameters.
'14. Configure Noise Compensation Enabled.
'15. Initiate Measurement.
'16. Fetch OFDMModAcc Measurements.
'17. Close the RFmx Session

Imports System.Windows.Forms
Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.WlanMX

Namespace NationalInstruments.Examples.RFmxWlanOfdmModAccEvmNoiseCompensation

    Public Class RFmxWlanOfdmModAccEvmNoiseCompensation
        Private instrSession As RFmxInstrMX
        Private wlan As RFmxWlanMX
        Private resourceName As String

        Private centerFrequency As Double
        Private referenceLevel As Double
        Private externalAttenuation As Double

        Private frequencyReferenceSource As String
        Private frequencyReferenceFrequency As Double

        Private iqPowerEdgeEnabled As Boolean
        Private iqPowerEdgeLevel As Double
        Private triggerDelay As Double
        Private minimumQuietTimeMode As RFmxWlanMXTriggerMinimumQuietTimeMode
        Private minimumQuietTime As Double

        Private standard As RFmxWlanMXStandard

        Private channelBandwidth As Double

        Private averagingEnabled As RFmxWlanMXOfdmModAccAveragingEnabled
        Private averagingCount As Integer

        Private noiseCompensationEnabled As RFmxWlanMXOfdmModAccNoiseCompensationEnabled
        Private optimizeDynamicRangeForEvmEnabled As RFmxWlanMXOfdmModAccOptimizeDynamicRangeForEvmEnabled
        Private optimizeDynamicRangeForEvmMargin As Double
        Private calibrateNoiseFloor As Boolean

        Private measurementOffset As Integer
        Private maximumMeasurementLength As Integer

        Private timeout As Double

        Private compositeRmsEvmMean As Double
        Private compositeDataRmsEvmMean As Double
        Private compositePilotRmsEvmMean As Double

        Private pilotConstellation As ComplexSingle()
        Private dataConstellation As ComplexSingle()
        Private chainRmsEvmPerSubcarrierMean As AnalogWaveform(Of Single)

        Public Sub Run()
            Try
                InitializeVariables()
                InitializeInstr()
                ConfigureWlan()
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
            resourceName = "RFSA"

            centerFrequency = 2412000000.0
            ' (Hz)
            referenceLevel = 0.0
            ' (dBm)
            externalAttenuation = 0.0
            ' (dB)

            frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
            frequencyReferenceFrequency = 10000000.0
            ' (Hz)

            iqPowerEdgeEnabled = True
            iqPowerEdgeLevel = -20.0
            '(dB)
            triggerDelay = 0.0
            ' (s)
            minimumQuietTimeMode = RFmxWlanMXTriggerMinimumQuietTimeMode.Auto
            minimumQuietTime = 0.000005
            ' (s)

            standard = RFmxWlanMXStandard.Standard802_11ag

            channelBandwidth = 20000000.0
            '(Hz)

            averagingEnabled = RFmxWlanMXOfdmModAccAveragingEnabled.[False]
            averagingCount = 10

            noiseCompensationEnabled = RFmxWlanMXOfdmModAccNoiseCompensationEnabled.[True]
            optimizeDynamicRangeForEvmEnabled = RFmxWlanMXOfdmModAccOptimizeDynamicRangeForEvmEnabled.[True]
            optimizeDynamicRangeForEvmMargin = 0.0
            ' (dB)
            calibrateNoiseFloor = True
            'True - Enabled, False - Disabled.
            measurementOffset = 0
            ' (symbols)
            maximumMeasurementLength = 16
            ' (symbols)

            timeout = 10.0
            ' (s)

            compositeRmsEvmMean = 0.0
            ' (dB)
            compositeDataRmsEvmMean = 0.0
            ' (dB)
            compositePilotRmsEvmMean = 0.0
            ' (dB)
        End Sub

        Private Sub InitializeInstr()
            instrSession = New RFmxInstrMX(resourceName, "")
        End Sub

        Private Sub ConfigureWlan()
            wlan = instrSession.GetWlanSignalConfiguration()
            ' Create a new RFmx Session.
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
            wlan.ConfigureFrequency("", centerFrequency)
            wlan.ConfigureReferenceLevel("", referenceLevel)
            wlan.ConfigureExternalAttenuation("", externalAttenuation)
            wlan.ConfigureIQPowerEdgeTrigger("", "0", RFmxWlanMXIQPowerEdgeTriggerSlope.Rising, iqPowerEdgeLevel, triggerDelay, minimumQuietTimeMode,
           minimumQuietTime, RFmxWlanMXIQPowerEdgeTriggerLevelType.Relative, iqPowerEdgeEnabled)
            wlan.ConfigureStandard("", standard)
            wlan.ConfigureChannelBandwidth("", channelBandwidth)
            wlan.SelectMeasurements("", RFmxWlanMXMeasurementTypes.OfdmModAcc, True)
            wlan.OfdmModAcc.Configuration.ConfigureOptimizeDynamicRangeForEvm("", optimizeDynamicRangeForEvmEnabled, optimizeDynamicRangeForEvmMargin)

            If calibrateNoiseFloor Then
                wlan.OfdmModAcc.Configuration.ConfigureMeasurementMode("", RFmxWlanMXOfdmModAccMeasurementMode.CalibrateNoiseFloor)
                Dim userInput As DialogResult = MessageBox.Show("Turn OFF Generation", "", MessageBoxButtons.OKCancel)
                If userInput = DialogResult.OK Then
                    wlan.Initiate("", "")
                    wlan.WaitForMeasurementComplete("", timeout)
                End If
                MessageBox.Show("Turn ON Generaton")
            End If

            wlan.OfdmModAcc.Configuration.ConfigureMeasurementMode("", RFmxWlanMXOfdmModAccMeasurementMode.Measure)
            wlan.OfdmModAcc.Configuration.ConfigureMeasurementLength("", measurementOffset, maximumMeasurementLength)
            wlan.OfdmModAcc.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount)
            wlan.OfdmModAcc.Configuration.ConfigureNoiseCompensationEnabled("", noiseCompensationEnabled)
            wlan.Initiate("", "")
        End Sub

        Private Sub RetrieveResults()
            wlan.OfdmModAcc.Results.FetchCompositeRmsEvm("", timeout, compositeRmsEvmMean, compositeDataRmsEvmMean, compositePilotRmsEvmMean)
            wlan.OfdmModAcc.Results.FetchPilotConstellationTrace("", timeout, pilotConstellation)
            wlan.OfdmModAcc.Results.FetchDataConstellationTrace("", timeout, dataConstellation)
            wlan.OfdmModAcc.Results.FetchChainRmsEvmPerSubcarrierMeanTrace("", timeout, chainRmsEvmPerSubcarrierMean)
        End Sub

        Private Sub PrintResults()
            Console.WriteLine("------------------Composite EVM------------------")
            Console.WriteLine("RMS EVM Mean (dB)                       :{0}", compositeRmsEvmMean)
            Console.WriteLine("Data RMS EVM Mean (dB)                  :{0}", compositeDataRmsEvmMean)
            Console.WriteLine("Pilot RMS EVM Mean (dB)                 :{0}", compositePilotRmsEvmMean)
        End Sub

        Private Sub CloseSession()
            If wlan IsNot Nothing Then
                wlan.Dispose()
                wlan = Nothing
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
