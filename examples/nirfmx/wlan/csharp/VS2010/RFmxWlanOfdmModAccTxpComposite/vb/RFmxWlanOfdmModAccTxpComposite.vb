'Steps:
'1. Open a new RFmx session.
'2. Configure the Frequency Reference properties (Clock Source and Clock Frequency).
'3. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation).
'4. Configure IQ Power Edge Trigger properties (Trigger Delay, IQ Power Edge Level, Minimum Quiet Time).
'5. Configure Standard and Channel Bandwidth Properties.
'6. Select OFDMModAcc and TXP measurements.
'7. Configure the Measurement Interval.
'8. Configure Averaging parameters for OFDMModAcc.
'9. Configure Averaging parameters for TXP.
'10. Configure the Maximum Measurement Interval.
'11. Initiate Measurement.
'12. Fetch OFDMModAcc Measurement.
'13. Fetch TXP Measurement.
'14. Close the RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.WlanMX

Namespace NationalInstruments.Examples.RFmxWlanOfdmModAccTxpComposite

    Public Class RFmxWlanOfdmModAccTxpComposite
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

        Private measurementOffset As Integer
        Private maximumMeasurementLength As Integer

        Private ofdmModAccAveragingEnabled As RFmxWlanMXOfdmModAccAveragingEnabled
        Private ofdmModAccAveragingCount As Integer

        Private txpAveragingEnabled As RFmxWlanMXTxpAveragingEnabled
        Private txpAveragingCount As Integer

        Private timeout As Double

        Private maximumMeasurementInterval As Double

        Private compositeRmsEvmMean As Double
        Private compositeDataRmsEvmMean As Double
        Private compositePilotRmsEvmMean As Double

        Private averagePowerMean As Double, peakPowerMaximum As Double

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

            measurementOffset = 0
            ' (symbols)
            maximumMeasurementLength = 16
            ' (symbols)

            ofdmModAccAveragingEnabled = RFmxWlanMXOfdmModAccAveragingEnabled.[False]
            ofdmModAccAveragingCount = 10

            txpAveragingEnabled = RFmxWlanMXTxpAveragingEnabled.[False]
            txpAveragingCount = 10

            maximumMeasurementInterval = 0.001
            ' (s)

            timeout = 10.0
            ' (s)

            compositeRmsEvmMean = 0.0
            ' (dB)
            compositeDataRmsEvmMean = 0.0
            ' (dB)
            compositePilotRmsEvmMean = 0.0
            ' (dB)

            averagePowerMean = 0
            ' (dBm)
            peakPowerMaximum = 0
            ' (dBm)
        End Sub

        Private Sub InitializeInstr()
            instrSession = New RFmxInstrMX(resourceName, "")
        End Sub

        Private Sub ConfigureWlan()
            wlan = instrSession.GetWlanSignalConfiguration()
            ' Create a new RFmx Session
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
            wlan.ConfigureFrequency("", centerFrequency)
            wlan.ConfigureReferenceLevel("", referenceLevel)
            wlan.ConfigureExternalAttenuation("", externalAttenuation)
            wlan.ConfigureIQPowerEdgeTrigger("", "0", RFmxWlanMXIQPowerEdgeTriggerSlope.Rising, iqPowerEdgeLevel, triggerDelay, minimumQuietTimeMode,
         minimumQuietTime, RFmxWlanMXIQPowerEdgeTriggerLevelType.Relative, iqPowerEdgeEnabled)
            wlan.ConfigureStandard("", standard)
            wlan.ConfigureChannelBandwidth("", channelBandwidth)
            wlan.SelectMeasurements("", RFmxWlanMXMeasurementTypes.Txp Or RFmxWlanMXMeasurementTypes.OfdmModAcc, True)
            wlan.OfdmModAcc.Configuration.ConfigureMeasurementLength("", measurementOffset, maximumMeasurementLength)
            wlan.OfdmModAcc.Configuration.ConfigureAveraging("", ofdmModAccAveragingEnabled, ofdmModAccAveragingCount)
            wlan.Txp.Configuration.ConfigureAveraging("", txpAveragingEnabled, txpAveragingCount)
            wlan.Txp.Configuration.ConfigureMaximumMeasurementInterval("", maximumMeasurementInterval)
            wlan.Initiate("", "")
        End Sub

        Private Sub RetrieveResults()
            wlan.OfdmModAcc.Results.FetchCompositeRmsEvm("", timeout, compositeRmsEvmMean, compositeDataRmsEvmMean, compositePilotRmsEvmMean)
            wlan.Txp.Results.FetchMeasurement("", timeout, averagePowerMean, peakPowerMaximum)
        End Sub

        Private Sub PrintResults()
            Console.WriteLine("------------------OfdmModAcc Measurement------------------" & vbLf)
            Console.WriteLine("Rms Evm Mean (dB)                       :{0}", compositeRmsEvmMean)
            Console.WriteLine("Data Rms Evm Mean (dB)                  :{0}", compositeDataRmsEvmMean)
            Console.WriteLine("Pilot Rms Evm Mean (dB)                 :{0}", compositePilotRmsEvmMean)
            Console.WriteLine(vbLf & "----------Txp Measurement----------" & vbLf)
            Console.WriteLine("Average Power Mean (dBm)                :{0}", averagePowerMean)
            Console.WriteLine("Peak Power Maximum (dBm)                :{0}", peakPowerMaximum)
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
