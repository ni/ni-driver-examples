'Steps:
'1. Open a New RFmx session.
'2. Configure the Frequency Reference properties (Clock Source And Clock Frequency).
'3. Configure Number of Frequency Segment And Receive Chain.
'4. Configure Center Frequency for each Segment.
'5.Configure Selected Port.
'6. Configure the basic signal port specific properties ( Reference Level And External Attenuation).
'7. Configure IQ Power Edge Trigger properties (Trigger Delay, IQ Power Edge Level, Min Quiet Time).
'8. Configure Standard And Channel Bandwidth Properties.
'9. Select OFDMModAcc measurement And enable the traces. 
'10. Configure the Measurement Interval.
'11. Configure Frequency Error Estimation Method.
'12. Configure Amplitude Tracking Enabled.
'13. Configure Phase Tracking Enabled.
'14. Configure Symbol Clock Error Correction Enabled.
'15. Configure Channel Estimation Type.
'16. Configure Averaging parameters.
'17. Configure Channel Matrix Power Enabled.
'18. Initiate Measurement.
'19. Fetch OFDMModAcc Measurements.
'19. Fetch User Specific Results based on the PPDU Type. 
'20.Close the RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.WlanMX

Namespace NationalInstruments.Examples.RFmxWlanOfdmModAccMimo
    Public Class RFmxWlanOfdmModAccMimo
        Private instrSession As RFmxInstrMX
        Private wlan As RFmxWlanMX
        Private resourceName As String()
        Private numberOfDevices As Integer

        Private selectedPorts As String()

        Private frequencyReferenceSource As String
        Private frequencyReferenceFrequency As Double

        Private numberOfFrequencySegments As Integer
        Private numberOfReceiveChains As Integer

        Private segmentString As String
        Private chainString As String

        Private centerFrequency As Double()
        Private referenceLevel As Double()
        Private externalAttenuation As Double()

        Private portString As String()

        Private selectedPortsString As String()

        Private iqPowerEdgeEnabled As Boolean
        Private iqPowerEdgeLevel As Double
        Private triggerDelay As Double
        Private minimumQuietTimeMode As RFmxWlanMXTriggerMinimumQuietTimeMode
        Private minimumQuietTime As Double

        Private standard As RFmxWlanMXStandard

        Private channelBandwidth As Double

        Private measurementOffset As Integer
        Private maximumMeasurementLength As Integer

        Private frequencyErrorEstimationMethod As RFmxWlanMXOfdmModAccFrequencyErrorEstimationMethod
        Private amplitudeTrackingEnabled As RFmxWlanMXOfdmModAccAmplitudeTrackingEnabled
        Private phaseTrackingEnabled As RFmxWlanMXOfdmModAccPhaseTrackingEnabled
        Private symbolClockErrorCorrectionEnabled As RFmxWlanMXOfdmModAccSymbolClockErrorCorrectionEnabled
        Private channelEstimationType As RFmxWlanMXOfdmModAccChannelEstimationType
        Private channelMatrixPowerEnabled As RFmxWlanMXOfdmModAccChannelMatrixPowerEnabled

        Private averagingEnabled As RFmxWlanMXOfdmModAccAveragingEnabled
        Private averagingCount As Integer

        Private timeout As Double

        Private compositeRmsEvmMean As Double
        Private compositeDataRmsEvmMean As Double
        Private compositePilotRmsEvmMean As Double
        Private numberOfSymbolsUsed As Integer

        Private ppduType As RFmxWlanMXOfdmPpduType
        Private guardIntervalType As RFmxWlanMXOfdmGuardIntervalType
        Private lSigParityCheckStatus As RFmxWlanMXOfdmModAccLSigParityCheckStatus
        Private sigCrcStatus As RFmxWlanMXOfdmModAccSigCrcStatus
        Private sigBCrcStatus As RFmxWlanMXOfdmModAccSigBCrcStatus

        Private mcsIndex As Integer()
        Private numberOfSpaceTimeStreams As Integer()

        Private numberOfUsers As Integer
        Private userString As String
        Private numberOfStreamResults As Integer

        Private frequencyErrorMean As Double()
        Private symbolClockErrorMean As Double()

        Private streamString As String

        Private streamRmsEvmMean As Double(,)
        Private streamDataRmsEvmMean As Double(,)
        Private streamPilotRmsEvmMean As Double(,)

        Private streamRmsEvmPerSubcarrierMean As AnalogWaveform(Of Single)(,)
        Private pilotConstellation As ComplexSingle(,)()
        Private dataConstellation As ComplexSingle(,)()

        Private crossPowerMean As Double(,)

        Private relativeIQOriginOffsetMean As Double(,)
        Private iqGainImbalanceMean As Double(,)
        Private iqQuadratureErrorMean As Double(,)
        Private absoluteIQOriginOffsetMean As Double(,)
        Private iqTimingSkewMean As Double(,)

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
            resourceName = New String() {"RFSA1", "RFSA2"}
            numberOfDevices = resourceName.GetLength(0)

            selectedPorts = New String() {"", ""}

            frequencyReferenceSource = RFmxInstrMXConstants.PxiClock
            frequencyReferenceFrequency = 10000000.0
            ' (Hz) 

            numberOfFrequencySegments = 1
            numberOfReceiveChains = 2

            centerFrequency = New Double() {5180000000.0, 5260000000.0}
            ' (Hz) 
            referenceLevel = New Double() {0.0, 0.0}
            ' (dBm) 
            externalAttenuation = New Double() {0.0, 0.0}
            ' (dB) 

            portString = New String(numberOfDevices - 1) {}

            selectedPortsString = New String(numberOfDevices - 1) {}

            iqPowerEdgeEnabled = True
            iqPowerEdgeLevel = -20.0
            '(dB) 
            triggerDelay = 0.0
            ' (s) 
            minimumQuietTimeMode = RFmxWlanMXTriggerMinimumQuietTimeMode.Auto
            minimumQuietTime = 0.000005
            ' (s) 

            standard = RFmxWlanMXStandard.Standard802_11n

            channelBandwidth = 20000000.0
            '(Hz) 

            measurementOffset = 0
            ' (symbols) 
            maximumMeasurementLength = 16
            ' (symbols) 

            frequencyErrorEstimationMethod = RFmxWlanMXOfdmModAccFrequencyErrorEstimationMethod.PreambleAndPilots
            amplitudeTrackingEnabled = RFmxWlanMXOfdmModAccAmplitudeTrackingEnabled.[False]
            phaseTrackingEnabled = RFmxWlanMXOfdmModAccPhaseTrackingEnabled.[True]
            symbolClockErrorCorrectionEnabled = RFmxWlanMXOfdmModAccSymbolClockErrorCorrectionEnabled.[True]
            channelEstimationType = RFmxWlanMXOfdmModAccChannelEstimationType.Reference
            channelMatrixPowerEnabled = RFmxWlanMXOfdmModAccChannelMatrixPowerEnabled.True

            averagingEnabled = RFmxWlanMXOfdmModAccAveragingEnabled.[False]
            averagingCount = 10

            timeout = 10.0
            ' (s) 

            ppduType = RFmxWlanMXOfdmPpduType.NonHT
            guardIntervalType = RFmxWlanMXOfdmGuardIntervalType.OneByFour
            lSigParityCheckStatus = RFmxWlanMXOfdmModAccLSigParityCheckStatus.NotApplicable
            sigCrcStatus = RFmxWlanMXOfdmModAccSigCrcStatus.NotApplicable
            sigBCrcStatus = RFmxWlanMXOfdmModAccSigBCrcStatus.NotApplicable

            numberOfStreamResults = Int32.MinValue

            frequencyErrorMean = New Double(numberOfFrequencySegments - 1) {}
            symbolClockErrorMean = New Double(numberOfFrequencySegments - 1) {}

            crossPowerMean = New Double(numberOfFrequencySegments - 1, numberOfReceiveChains - 1) {}

            relativeIQOriginOffsetMean = New Double(numberOfFrequencySegments - 1, numberOfReceiveChains - 1) {}
            iqGainImbalanceMean = New Double(numberOfFrequencySegments - 1, numberOfReceiveChains - 1) {}
            iqQuadratureErrorMean = New Double(numberOfFrequencySegments - 1, numberOfReceiveChains - 1) {}
            absoluteIQOriginOffsetMean = New Double(numberOfFrequencySegments - 1, numberOfReceiveChains - 1) {}
            iqTimingSkewMean = New Double(numberOfFrequencySegments - 1, numberOfReceiveChains - 1) {}
        End Sub

        Private Sub InitializeInstr()
            instrSession = New RFmxInstrMX(resourceName, "")
        End Sub

        Private Sub ConfigureWlan()
            wlan = instrSession.GetWlanSignalConfiguration()
            ' Create a new RFmx Session 
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
            wlan.ConfigureNumberOfFrequencySegmentsAndReceiveChains("", numberOfFrequencySegments, numberOfReceiveChains)
            For i As Integer = 0 To numberOfFrequencySegments - 1
                segmentString = RFmxWlanMX.BuildSegmentString("", i)
                wlan.ConfigureFrequency(segmentString, centerFrequency(i))
            Next
            For i As Integer = 0 To numberOfDevices - 1
                selectedPortsString(i) = RFmxInstrMX.BuildPortString2("", selectedPorts(i), resourceName(i), 0)
                portString(i) = RFmxInstrMX.BuildPortString2("", "", resourceName(i), 0)
                wlan.ConfigureReferenceLevel(portString(i), referenceLevel(i))
                wlan.ConfigureExternalAttenuation(portString(i), externalAttenuation(i))
            Next
            wlan.ConfigureSelectedPortsMultiple("", selectedPortsString)
            wlan.ConfigureIQPowerEdgeTrigger("", "0", RFmxWlanMXIQPowerEdgeTriggerSlope.Rising, iqPowerEdgeLevel, triggerDelay, minimumQuietTimeMode,
                minimumQuietTime, RFmxWlanMXIQPowerEdgeTriggerLevelType.Relative, iqPowerEdgeEnabled)
            wlan.ConfigureStandard("", standard)
            wlan.ConfigureChannelBandwidth("", channelBandwidth)
            wlan.SelectMeasurements("", RFmxWlanMXMeasurementTypes.OfdmModAcc, True)
            wlan.OfdmModAcc.Configuration.ConfigureMeasurementLength("", measurementOffset, maximumMeasurementLength)
            wlan.OfdmModAcc.Configuration.ConfigureFrequencyErrorEstimationMethod("", frequencyErrorEstimationMethod)
            wlan.OfdmModAcc.Configuration.ConfigureAmplitudeTrackingEnabled("", amplitudeTrackingEnabled)
            wlan.OfdmModAcc.Configuration.ConfigurePhaseTrackingEnabled("", phaseTrackingEnabled)
            wlan.OfdmModAcc.Configuration.ConfigureSymbolClockErrorCorrectionEnabled("", symbolClockErrorCorrectionEnabled)
            wlan.OfdmModAcc.Configuration.ConfigureChannelEstimationType("", channelEstimationType)
            wlan.OfdmModAcc.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount)
            wlan.OfdmModAcc.Configuration.SetChannelMatrixPowerEnabled("", channelMatrixPowerEnabled)
            wlan.Initiate("", "")
        End Sub

        Private Sub RetrieveResults()
            wlan.OfdmModAcc.Results.FetchCompositeRmsEvm("", timeout, compositeRmsEvmMean, compositeDataRmsEvmMean, compositePilotRmsEvmMean)
            wlan.OfdmModAcc.Results.FetchNumberOfSymbolsUsed("", timeout, numberOfSymbolsUsed)
            wlan.OfdmModAcc.Results.FetchPpduType("", timeout, ppduType)
            wlan.OfdmModAcc.Results.FetchGuardIntervalType("", timeout, guardIntervalType)
            wlan.OfdmModAcc.Results.FetchLSigParityCheckStatus("", timeout, lSigParityCheckStatus)
            wlan.OfdmModAcc.Results.FetchSigCrcStatus("", timeout, sigCrcStatus)
            wlan.OfdmModAcc.Results.FetchSigBCrcStatus("", timeout, sigBCrcStatus)
            If ppduType = RFmxWlanMXOfdmPpduType.MU Then
                wlan.OfdmModAcc.Results.FetchNumberOfUsers("", timeout, numberOfUsers)
                mcsIndex = New Integer(numberOfUsers - 1) {}
                numberOfSpaceTimeStreams = New Integer(numberOfUsers - 1) {}
                For i As Integer = 0 To numberOfUsers - 1
                    userString = RFmxWlanMX.BuildUserString("", i)
                    wlan.OfdmModAcc.Results.FetchMcsIndex(userString, timeout, mcsIndex(i))
                    wlan.OfdmModAcc.Results.FetchNumberOfSpaceTimeStreams(userString, timeout, numberOfSpaceTimeStreams(i))
                    Dim tempOffset As Integer
                    wlan.OfdmModAcc.Results.GetSpaceTimeStreamOffset(userString, tempOffset)
                    tempOffset = tempOffset + numberOfSpaceTimeStreams(i)
                    If tempOffset > numberOfStreamResults Then
                        numberOfStreamResults = tempOffset
                    End If
                Next
            Else
                mcsIndex = New Integer(0) {}
                numberOfSpaceTimeStreams = New Integer(0) {}
                wlan.OfdmModAcc.Results.FetchMcsIndex("", timeout, mcsIndex(0))
                wlan.OfdmModAcc.Results.FetchNumberOfSpaceTimeStreams("", timeout, numberOfSpaceTimeStreams(0))
                numberOfStreamResults = numberOfSpaceTimeStreams(0)
            End If
            streamRmsEvmMean = New Double(numberOfFrequencySegments - 1, numberOfStreamResults - 1) {}
            streamDataRmsEvmMean = New Double(numberOfFrequencySegments - 1, numberOfStreamResults - 1) {}
            streamPilotRmsEvmMean = New Double(numberOfFrequencySegments - 1, numberOfStreamResults - 1) {}
            streamRmsEvmPerSubcarrierMean = New AnalogWaveform(Of Single)(numberOfFrequencySegments - 1, numberOfStreamResults - 1) {}
            pilotConstellation = New ComplexSingle(numberOfFrequencySegments - 1, numberOfStreamResults - 1)() {}
            dataConstellation = New ComplexSingle(numberOfFrequencySegments - 1, numberOfStreamResults - 1)() {}
            For i As Integer = 0 To numberOfFrequencySegments - 1
                segmentString = RFmxWlanMX.BuildSegmentString("", i)
                wlan.OfdmModAcc.Results.FetchFrequencyErrorMean("", timeout, frequencyErrorMean(i))
                wlan.OfdmModAcc.Results.FetchSymbolClockErrorMean("", timeout, symbolClockErrorMean(i))
                For j As Integer = 0 To numberOfStreamResults - 1
                    streamString = RFmxWlanMX.BuildStreamString(segmentString, j)
                    wlan.OfdmModAcc.Results.FetchStreamRmsEvm(streamString, timeout, streamDataRmsEvmMean(i, j), streamDataRmsEvmMean(i, j), streamPilotRmsEvmMean(i, j))
                    wlan.OfdmModAcc.Results.FetchStreamRmsEvmPerSubcarrierMeanTrace(streamString, timeout, streamRmsEvmPerSubcarrierMean(i, j))
                    wlan.OfdmModAcc.Results.FetchPilotConstellationTrace(streamString, timeout, pilotConstellation(i, j))
                    wlan.OfdmModAcc.Results.FetchDataConstellationTrace(streamString, timeout, dataConstellation(i, j))
                Next
                For j As Integer = 0 To numberOfReceiveChains - 1
                    chainString = RFmxWlanMX.BuildChainString(segmentString, j)
                    wlan.OfdmModAcc.Results.FetchCrossPower(segmentString, timeout, crossPowerMean(i, j))
                    wlan.OfdmModAcc.Results.FetchIQImpairments(segmentString, timeout, relativeIQOriginOffsetMean(i, j), iqGainImbalanceMean(i, j), iqQuadratureErrorMean(i, j), absoluteIQOriginOffsetMean(i, j),
                        iqTimingSkewMean(i, j))
                Next
            Next
        End Sub

        Private Sub PrintResults()
            Console.WriteLine("-----------------------EVM-----------------------" & vbLf)
            Console.WriteLine("------------------Composite EVM------------------")
            Console.WriteLine("RMS EVM Mean (dB)                       : {0}", compositeRmsEvmMean)
            Console.WriteLine("Data RMS EVM Mean (dB)                  : {0}", compositeDataRmsEvmMean)
            Console.WriteLine("Pilot RMS EVM Mean (dB)                 : {0}" & vbLf, compositePilotRmsEvmMean)
            Console.WriteLine("Number of Symbols Used                  : {0}" & vbLf, numberOfSymbolsUsed)
            Console.WriteLine(vbLf & "--------------------------------------------------" & vbLf & vbLf)

            Console.WriteLine("---------------------PPDU Info--------------------")
            Console.WriteLine("PPDU Type                               : {0}", ppduType)
            If ppduType = RFmxWlanMXOfdmPpduType.MU Then
                For i As Integer = 0 To numberOfUsers - 1
                    Console.WriteLine(vbLf & "NSTS {0}                                  : {0}", i, numberOfSpaceTimeStreams(i))
                    Console.WriteLine("MCS Index {0}                             : {0}" & vbLf, i, mcsIndex(i))
                Next
            Else
                Console.WriteLine("NSTS                                    : {0}", numberOfSpaceTimeStreams(0))
                Console.WriteLine("MCS Index                               : {0}", mcsIndex(0))
            End If
            Console.WriteLine("Guard Interval Type                     : {0}", guardIntervalType)
            Console.WriteLine("L-SIG Parity Check Status               : {0}", lSigParityCheckStatus)
            Console.WriteLine("SIG CRC Status                          : {0}", sigCrcStatus)
            Console.WriteLine("SIG-B CRC Status                        : {0}", sigBCrcStatus)

            Console.WriteLine(vbLf & "--------------------------------------------------" & vbLf & vbLf)
            For i As Integer = 0 To numberOfFrequencySegments - 1
                segmentString = RFmxWlanMX.BuildSegmentString("", i)
                Console.WriteLine("------------Measurements for {0}-------------" & vbLf, segmentString)
                Console.WriteLine("Frequency Error Mean (Hz)               : {0}", frequencyErrorMean(i))
                Console.WriteLine("Symbol Clock Error Mean (ppm)           : {0}" & vbLf, symbolClockErrorMean(i))
                For j As Integer = 0 To numberOfStreamResults - 1
                    streamString = RFmxWlanMX.BuildStreamString(segmentString, j)
                    Console.WriteLine(vbLf & "---------Measurements for {0}--------", streamString)
                    Console.WriteLine("Stream RMS EVM Mean (dB)                 : {0}", streamDataRmsEvmMean(i, j))
                    Console.WriteLine("Stream Pilot RMS EVM Mean (dB)           : {0}", streamPilotRmsEvmMean(i, j))
                    Console.WriteLine("Stream Data RMS EVM Mean (dB)            : {0}" & vbLf, streamDataRmsEvmMean(i, j))
                Next
                For j As Integer = 0 To numberOfReceiveChains - 1
                    chainString = RFmxWlanMX.BuildChainString(segmentString, j)
                    Console.WriteLine(vbLf & "---------Measurements for {0}---------", chainString)
                    Console.WriteLine("Cross Power Mean (dB)                   : {0}", crossPowerMean(i, j))
                    Console.WriteLine(vbLf & "------------------IQ Impairments------------------")
                    Console.WriteLine("Relative I/Q Origin Offset Mean (dB)    : {0}", relativeIQOriginOffsetMean(i, j))
                    Console.WriteLine("Absolute I/Q Origin Offset Mean (dBm)   : {0}", absoluteIQOriginOffsetMean(i, j))
                    Console.WriteLine("I/Q Gain Imbalance Mean (dB)            : {0}", iqGainImbalanceMean(i, j))
                    Console.WriteLine("I/Q Quadrature Error Mean (deg)         : {0}", iqQuadratureErrorMean(i, j))
                    Console.WriteLine("I/Q Timing Skew Mean (s)                : {0}" & vbLf, iqTimingSkewMean(i, j))
                Next
                Console.WriteLine(vbLf & "--------------------------------------------------" & vbLf & vbLf)
            Next
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