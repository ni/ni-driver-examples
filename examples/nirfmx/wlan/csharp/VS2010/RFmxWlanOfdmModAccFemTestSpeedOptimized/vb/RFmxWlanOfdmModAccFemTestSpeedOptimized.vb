'Steps:
'1. Open a new RFmx session.
'2. Configure the Frequency Reference properties (Clock Source and Clock Frequency).
'3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
'4. Configure IQ Power Edge Trigger properties (Trigger Delay, IQ Power Edge Level, Minimum Quiet Time).
'5. Configure Standard and Channel Bandwidth Properties.
'6. Disable auto PPDU type detection and header decoding. Configure PPDU Type and properties that are otherwise decoded from Header.
'7. Select OFDMModAcc measurement and disable the traces.
'8. Configure the Measurement Interval. Make sure that the input signal has number of symbols at least equal to the specified Maximum Measurement Length.
'9. Configure Frequency Error Estimation Method. Setting the Frequency Error Estimation Method to Disabled optimizes speed of the measurement
'   when there is no frequency error between transmitter and receiver.
'10. Configure Amplitude Tracking Enabled.
'11. Configure  Symbol Clock Error Correction Enabled. Setting the Symbol Clock Correction Enabled to False optimizes speed of the measurement
'    when there is no symbol clock error between transmitter and receiver.
'12. Configure Averaging parameters.
'13. Disable burst start detection and I/Q impairments estimation.
'14. Initiate Measurement.
'15. Fetch OFDMModAcc Measurements.
'16. Close the RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.WlanMX

Namespace NationalInstruments.Examples.RFmxWlanOfdmModAccFemTestSpeedOptimized
   Public Class RFmxWlanOfdmModAccFemTestSpeedOptimized
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

      Private autoPpduTypeDetectionEnabled As RFmxWlanMXOfdmAutoPpduTypeDetectionEnabled
      Private ppduType As RFmxWlanMXOfdmPpduType
      Private headerDecodingEnabled As RFmxWlanMXOfdmHeaderDecodingEnabled
      Private mcsIndex As Integer
      Private guardIntervalType As RFmxWlanMXOfdmGuardIntervalType
      Private ltfSize As RFmxWlanMXOfdmLtfSize
      Private RUSize As Integer
      Private numberOfSigSymbols As Integer

      Private burstStartDetectionEnabled As RFmxWlanMXOfdmModAccBurstStartDetectionEnabled
      Private iqImpairmentsEstimationEnabled As RFmxWlanMXOfdmModAccIQImpairmentsEstimationEnabled

      Private averagingEnabled As RFmxWlanMXOfdmModAccAveragingEnabled
      Private averagingCount As Integer

      Private measurementOffset As Integer
      Private maximumMeasurementLength As Integer
      Private frequencyErrorEstimationMethod As RFmxWlanMXOfdmModAccFrequencyErrorEstimationMethod

      Private amplitudeTrackingEnabled As RFmxWlanMXOfdmModAccAmplitudeTrackingEnabled
      Private symbolClockErrorCorrectionEnabled As RFmxWlanMXOfdmModAccSymbolClockErrorCorrectionEnabled

      Private timeout As Double

      Private compositeRmsEvmMean As Double
      Private compositeDataRmsEvmMean As Double
      Private compositePilotRmsEvmMean As Double
      Private numberOfSymbolsUsed As Integer
      Private frequencyErrorMean As Double
      Private symbolClockErrorMean As Double

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

         autoPpduTypeDetectionEnabled = RFmxWlanMXOfdmAutoPpduTypeDetectionEnabled.[False]
         ppduType = RFmxWlanMXOfdmPpduType.NonHT
         headerDecodingEnabled = RFmxWlanMXOfdmHeaderDecodingEnabled.[False]
         mcsIndex = 0
         guardIntervalType = RFmxWlanMXOfdmGuardIntervalType.OneByFour
         ltfSize = RFmxWlanMXOfdmLtfSize.LtfSize4x
         RUSize = 26
         numberOfSigSymbols = 1

         burstStartDetectionEnabled = RFmxWlanMXOfdmModAccBurstStartDetectionEnabled.[False]
         iqImpairmentsEstimationEnabled = RFmxWlanMXOfdmModAccIQImpairmentsEstimationEnabled.[False]

         amplitudeTrackingEnabled = RFmxWlanMXOfdmModAccAmplitudeTrackingEnabled.[False]
         symbolClockErrorCorrectionEnabled = RFmxWlanMXOfdmModAccSymbolClockErrorCorrectionEnabled.[False]

         averagingEnabled = RFmxWlanMXOfdmModAccAveragingEnabled.[False]
         averagingCount = 10

         measurementOffset = 0
         ' (symbols)
         maximumMeasurementLength = 16
         ' (symbols)
         frequencyErrorEstimationMethod = RFmxWlanMXOfdmModAccFrequencyErrorEstimationMethod.Disabled

         timeout = 10.0
         ' (s)

         compositeRmsEvmMean = 0.0
         '(dB)
         compositeDataRmsEvmMean = 0.0
         '(dB)
         compositePilotRmsEvmMean = 0.0
         '(dB)
         numberOfSymbolsUsed = 0

         frequencyErrorMean = 0.0
         '(Hz)
         symbolClockErrorMean = 0.0
         '(ppm)
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
         wlan.SetOfdmAutoPpduTypeDetectionEnabled("", autoPpduTypeDetectionEnabled)
         wlan.SetOfdmPpduType("", ppduType)
         wlan.SetOfdmHeaderDecodingEnabled("", headerDecodingEnabled)
         wlan.SetOfdmMcsIndex("", mcsIndex)
         wlan.SetOfdmGuardIntervalType("", guardIntervalType)
         wlan.SetOfdmLtfSize("", ltfSize)
         wlan.SetOfdmRUSize("", RUSize)
         wlan.SetOfdmNumberOfSigSymbols("", numberOfSigSymbols)
         wlan.SelectMeasurements("", RFmxWlanMXMeasurementTypes.OfdmModAcc, True)
         wlan.OfdmModAcc.Configuration.ConfigureMeasurementLength("", measurementOffset, maximumMeasurementLength)
         wlan.OfdmModAcc.Configuration.ConfigureFrequencyErrorEstimationMethod("", frequencyErrorEstimationMethod)
         wlan.OfdmModAcc.Configuration.ConfigureAmplitudeTrackingEnabled("", amplitudeTrackingEnabled)
         wlan.OfdmModAcc.Configuration.ConfigureSymbolClockErrorCorrectionEnabled("", symbolClockErrorCorrectionEnabled)
         wlan.OfdmModAcc.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount)
         wlan.OfdmModAcc.Configuration.SetBurstStartDetectionEnabled("", burstStartDetectionEnabled)
         wlan.OfdmModAcc.Configuration.SetIQImpairmentsEstimationEnabled("", iqImpairmentsEstimationEnabled)
         wlan.Initiate("", "")
      End Sub

      Private Sub RetrieveResults()
         wlan.OfdmModAcc.Results.FetchCompositeRmsEvm("", timeout, compositeRmsEvmMean, compositeDataRmsEvmMean, compositePilotRmsEvmMean)
         wlan.OfdmModAcc.Results.FetchNumberOfSymbolsUsed("", timeout, numberOfSymbolsUsed)
         wlan.OfdmModAcc.Results.FetchFrequencyErrorMean("", timeout, frequencyErrorMean)
         wlan.OfdmModAcc.Results.FetchSymbolClockErrorMean("", timeout, symbolClockErrorMean)
      End Sub

      Private Sub PrintResults()
         Console.WriteLine("------------------Composite EVM------------------")
         Console.WriteLine("RMS EVM Mean (dB)                       :{0}", compositeRmsEvmMean)
         Console.WriteLine("Data RMS EVM Mean (dB)                  :{0}", compositeDataRmsEvmMean)
         Console.WriteLine("Pilot RMS EVM Mean (dB)                 :{0}" & vbLf, compositePilotRmsEvmMean)
         Console.WriteLine("Number of Symbols Used                  :{0}", numberOfSymbolsUsed)
         Console.WriteLine("Frequency Error Mean(Hz)                :{0}", frequencyErrorMean)
         Console.WriteLine("Symbol Clock Error Mean(ppm)            :{0}", symbolClockErrorMean)
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
