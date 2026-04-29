'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure basic signal properties(Center Frequency and External Attenuation).
'4. Configure Trigger Type and Trigger Parameters.
'5. Configure Packet Type.
'6. Configure Data Rate.
'7. Configure Payload Length.
'8. Configure Direction Finding.
'9. Configure Reference Level.
'10. Select Txp measurement and enable Traces.
'11. Configure Txp Burst Synchronization Type.
'12. Configure Averaging Parameters for Txp measurement.
'13. Initiate the Measurement.
'14. Fetch Txp Measurements and Trace.
'15. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.BTMX

Namespace NationalInstruments.Examples.RFmxBTTxp

    Public Class RFmxBTTxp
        Private instrSession As RFmxInstrMX
        Private BT As RFmxBTMX
        Private resourceName As String

        Private frequencyReferenceSource As String
        Private frequencyReferenceFrequency As Double

        Private centerFrequency As Double
        Private referenceLevel As Double
        Private externalAttenuation As Double
        Private autoLevel As Boolean
        Private measurementInterval As Double

        Private enableTrigger As Boolean
        Private iqPowerEdgeTriggerSlope As RFmxBTMXIQPowerEdgeTriggerSlope
        Private iqPowerEdgeLevel As Double
        Private minimumQuiteTimeMode As RFmxBTMXTriggerMinimumQuietTimeMode
        Private minimumQuietTime As Double
        Private iqPowerEdgeTriggerLevelType As RFmxBTMXIQPowerEdgeTriggerLevelType
        Private triggerDelay As Double

        Private packetType As RFmxBTMXPacketType
        Private dataRate As Integer

        Private payloadLengthMode As RFmxBTMXPayloadLengthMode
        Private payloadLength As Integer

        Private directionFindingMode As RFmxBTMXDirectionFindingMode
        Private cteLength As Double
	     Private cteSlotDuration As Double

        Private packetFormat As RFmxBTMXChannelSoundingPacketFormat
        Private syncSequence As RFmxBTMXChannelSoundingSyncSequence
        Private phaseMeasurementPeriod As Double
        Private toneExtensionSlot As RFmxBTMXChannelSoundingToneExtensionSlot

        Private zadoffChuIndex As Integer
        Private highDataThroughputPacketFormat As RFmxBTMXHighDataThroughputPacketFormat

        Private burstSynchronizationType As RFmxBTMXTxpBurstSynchronizationType

        Private measurement As RFmxBTMXMeasurementTypes
        Private enableAllTraces As Boolean

        Private averagingEnabled As RFmxBTMXTxpAveragingEnabled
        Private averagingCount As Integer

        Private timeout As Double
        Private averagePowerMean As Double
        Private averagePowerMaximum As Double
        Private averagePowerMinimum As Double
        Private peakToAveragePowerRatioMaximum As Double
        Private edrGfskAveragePowerMean As Double
        Private edrDpskAveragePowerMean As Double
        Private edrDpskGfskAveragePowerRatioMean As Double
        Private referencePeriodAveragePowerMean As Double
        Private referencePeriodPeakAbsolutePowerDeviationMaximum As Double
     	  Private transmitSlotAveragePowerMean As Double()
     	  Private transmitSlotPeakAbsolutePowerDeviationMaximum As Double()

     	  Private power As AnalogWaveform(Of Single)

        Public Sub Run()
            Try
                InitializeVariables()
                InitializeInstr()
                ConfigureBT()
                RetrieveResults()
                PrintResults()
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
            resourceName = "RFSA"

            frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
            frequencyReferenceFrequency = 10000000.0                                     ' (Hz)

            centerFrequency = 2402000000.0                                               ' (Hz)
            referenceLevel = 0.0                                                         ' (dBm)
            externalAttenuation = 0.0                                                    ' (dB)
            autoLevel = True
            measurementInterval = 0.01                                                   ' (s)

            enableTrigger = True
            iqPowerEdgeTriggerSlope = RFmxBTMXIQPowerEdgeTriggerSlope.Rising
            iqPowerEdgeLevel = -20.0                                                     'dB
            minimumQuiteTimeMode = RFmxBTMXTriggerMinimumQuietTimeMode.Auto
            minimumQuietTime = 0.0001                                                    'seconds
            iqPowerEdgeTriggerLevelType = RFmxBTMXIQPowerEdgeTriggerLevelType.Relative
            triggerDelay = 0.0                                                           'seconds

            packetType = RFmxBTMXPacketType.PacketTypeDH1
            dataRate = 1000000                                                           'bps

            payloadLengthMode = RFmxBTMXPayloadLengthMode.Auto
            payloadLength = 10                                                           'bytes

            directionFindingMode = RFmxBTMXDirectionFindingMode.Disabled
            cteLength = 0.00016                                                          'seconds
	    	   cteSlotDuration = 0.000001                                             		  'seconds

            packetFormat = RFmxBTMXChannelSoundingPacketFormat.Sync
            syncSequence = RFmxBTMXChannelSoundingSyncSequence.None
            phaseMeasurementPeriod = 10e-6                                               'seconds
            toneExtensionSlot = RFmxBTMXChannelSoundingToneExtensionSlot.Disabled

            zadoffChuIndex = 7
            highDataThroughputPacketFormat = RFmxBTMXHighDataThroughputPacketFormat.Format0

            burstSynchronizationType = RFmxBTMXTxpBurstSynchronizationType.Preamble

            measurement = RFmxBTMXMeasurementTypes.Txp
            enableAllTraces = True

            averagingEnabled = RFmxBTMXTxpAveragingEnabled.False
            averagingCount = 10

            timeout = 10.0                                                               'seconds
            averagePowerMean = 0                                                         '(dBm)
            averagePowerMaximum = 0                                                      '(dBm)
            averagePowerMinimum = 0.0                                                    '(dBm)
            peakToAveragePowerRatioMaximum = 0.0                                         '(dB)
            edrGfskAveragePowerMean = 0.0                                                '(dB)
            edrDpskAveragePowerMean = 0.0                                                '(dB)
         	edrDpskGfskAveragePowerRatioMean = 0.0                                       '(dB)
         	referencePeriodAveragePowerMean = 0.0                                        '(dBm)
         	referencePeriodPeakAbsolutePowerDeviationMaximum = 0.0                       '(%)
      	End Sub

        Private Sub InitializeInstr()
            instrSession = New RFmxInstrMX(resourceName, "")
        End Sub

        Private Sub ConfigureBT()
            BT = instrSession.GetBTSignalConfiguration()       ' Create a new RFmx Session 
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
            BT.ConfigureFrequency("", centerFrequency)
            BT.ConfigureExternalAttenuation("", externalAttenuation)
            BT.ConfigureIQPowerEdgeTrigger("", "0", iqPowerEdgeTriggerSlope, iqPowerEdgeLevel,
            triggerDelay, minimumQuiteTimeMode, minimumQuietTime, iqPowerEdgeTriggerLevelType,
            enableTrigger)
            BT.ConfigurePacketType("", packetType)
            BT.ConfigureDataRate("", dataRate)
            BT.ConfigurePayloadLength("", payloadLengthMode, payloadLength)
            BT.ConfigureLEDirectionFinding("", directionFindingMode, cteLength, cteSlotDuration)
            BT.SetChannelSoundingPacketFormat("",packetFormat)
            BT.SetChannelSoundingSyncSequence("",syncSequence)
            BT.SetChannelSoundingPhaseMeasurementPeriod("",phaseMeasurementPeriod)
            BT.SetChannelSoundingToneExtensionSlot("",toneExtensionSlot)
            BT.SetZadoffChuIndex("",zadoffChuIndex)
            BT.SetHighDataThroughputPacketFormat("",highDataThroughputPacketFormat)

            If autoLevel Then
                BT.AutoLevel("", measurementInterval, referenceLevel)
            Else
                BT.ConfigureReferenceLevel("", referenceLevel)
            End If
            BT.SelectMeasurements("", measurement, enableAllTraces)
            BT.Txp.Configuration.ConfigureBurstSynchronizationType("", burstSynchronizationType)
            BT.Txp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount)
            BT.Initiate("", "")
        End Sub

        Private Sub RetrieveResults()
            BT.Txp.Results.FetchPowers("", timeout, averagePowerMean, averagePowerMaximum, averagePowerMinimum, peakToAveragePowerRatioMaximum)
         	BT.Txp.Results.FetchEdrPowers("", timeout, edrGfskAveragePowerMean, edrDpskAveragePowerMean, edrDpskGfskAveragePowerRatioMean)
         	BT.Txp.Results.FetchLECteReferencePeriodPowers("", timeout, referencePeriodAveragePowerMean, referencePeriodPeakAbsolutePowerDeviationMaximum)
         	BT.Txp.Results.FetchLECteTransmitSlotPowersArray("", timeout, transmitSlotAveragePowerMean, transmitSlotPeakAbsolutePowerDeviationMaximum)	
         	BT.Txp.Results.FetchPowerTrace("", timeout, power)
        End Sub

        Private Sub PrintResults()
            Console.WriteLine("------------------Measurement------------------")
            Console.WriteLine("Average Power Mean (dBm)                        : {0}", averagePowerMean)
            Console.WriteLine("Average Power Maximum (dBm)                     : {0}", averagePowerMaximum)
            Console.WriteLine("Average Power Minimum (dBm)                     : {0}", averagePowerMinimum)
            Console.WriteLine("EDR GFSK Average Power Mean (dBm)               : {0}", edrGfskAveragePowerMean)
            Console.WriteLine("EDR DPSK Average Power Mean (dBm)               : {0}", edrDpskAveragePowerMean)
         	Console.WriteLine("EDR DPSK GFSK Average Power Ratio Mean (dB)     : {0}", edrDpskGfskAveragePowerRatioMean)

         	Console.WriteLine("------------------LE CTE Reference Period Measurement------------------")
         	Console.WriteLine("Average Power Mean (dBm)                                         : {0}", referencePeriodAveragePowerMean)
         	Console.WriteLine("Peak Absolute Power Deviation Maximum (%)                         : {0}", referencePeriodPeakAbsolutePowerDeviationMaximum)

         	Console.WriteLine("------------------LE CTE Transmit Slot Power Measurement------------------")
         	For i As Integer = 0 To transmitSlotAveragePowerMean.Length - 1
            	Console.WriteLine("Average Power Mean (dBm)[{0}]                     : {1}", i, transmitSlotAveragePowerMean(i))
            	Console.WriteLine("Peak Absolute Power Deviation Maximum (%)[{0}]     : {1}", i, transmitSlotPeakAbsolutePowerDeviationMaximum(i))
        	Next

      	End Sub

        Private Sub CloseSession()
            If BT IsNot Nothing Then
                BT.Dispose()
                BT = Nothing
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
