'Steps:
'1. Open a new RFmx session.
'2. Configure the Frequency Reference properties (Clock Source and Clock Frequency).
'3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
'4. Configure IQ Power Edge Trigger properties (Trigger Delay, IQ Power Edge Level, Minimum Quiet Time).
'5. Configure Standard to 802.11ax and Channel Bandwidth properties.
'6. Configure MCS Index, RU Size, RU Offset, RU Type, Distribution Bandwidth, Guard Interval Type, LTF Size and PE Disambiguity.
'7. Select OFDMModAcc measurement and enable the traces.
'8. Configure Measurement Interval.
'9. Configure Unused Tone Error Mask Reference.
'10. Configure Averaging parameters.
'11. Initiate Measurement.
'12. Fetch OFDMModAcc Measurements.
'13. Close the RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.WlanMX

Namespace NationalInstruments.Examples.RFmxWlanOfdmModAccTriggerBasedPpdu

   Public Class RFmxWlanOfdmModAccTriggerBasedPpdu
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
      Private distributionBandwidth As Double

      Private mcsIndex As Integer
      Private ruSize As Integer
      Private ruOffsetMruIndex As Integer
      Private ruType As RFmxWlanMXOfdmRUType
      Private guardIntervalType As RFmxWlanMXOfdmGuardIntervalType
      Private ltfSize As RFmxWlanMXOfdmLtfSize
      Private peDisambiguity As Integer

      Private measurementOffset As Integer
      Private maximumMeasurementLength As Integer

      Private unusedToneErrorMaskReference As RFmxWlanMXOfdmModAccUnusedToneErrorMaskReference

      Private averagingEnabled As RFmxWlanMXOfdmModAccAveragingEnabled
      Private averagingCount As Integer

      Private timeout As Double

      Private compositeRmsEvmMean As Double
      Private compositeDataRmsEvmMean As Double
      Private compositePilotRmsEvmMean As Double

      Private unusedToneErrorMargin As Double
      Private unusedToneErrorMarginRUIndex As Integer

      Private unusedToneErrorMarginPerRU As Double()
      Private frequencyErrorMean As Double
      Private frequencyErrorCcdf10Percent As Double
      Private symbolClockErrorMean As Double
      Private ppduType As RFmxWlanMXOfdmPpduType

      Private pilotConstellation As ComplexSingle()
      Private dataConstellation As ComplexSingle()
      Private unusedToneError As AnalogWaveform(Of Single)
      Private unusedToneErrorMask As AnalogWaveform(Of Single)

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

         standard = RFmxWlanMXStandard.Standard802_11ax

         channelBandwidth = 20000000.0
            '(Hz)

         distributionBandwidth = 20000000.0
            '(Hz)

         mcsIndex = 0
         ruSize = 26
         ruOffsetMruIndex = 0
         ruType = RFmxWlanMXOfdmRUType.Rru
         guardIntervalType = RFmxWlanMXOfdmGuardIntervalType.OneByFour
         ltfSize = RFmxWlanMXOfdmLtfSize.LtfSize4x
         peDisambiguity = 0

         measurementOffset = 0
         ' (symbols)
         maximumMeasurementLength = 16
         ' (symbols)

         unusedToneErrorMaskReference = RFmxWlanMXOfdmModAccUnusedToneErrorMaskReference.Limit1

         averagingEnabled = RFmxWlanMXOfdmModAccAveragingEnabled.False
         averagingCount = 10

         timeout = 10.0
         ' (s)
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
         wlan.SetOfdmMcsIndex("", mcsIndex)
         wlan.SetOfdmRUSize("", ruSize)
         wlan.SetOfdmRUOffsetMruIndex("", ruOffsetMruIndex)
         wlan.SetOfdmRUType("", ruType)
         wlan.SetOfdmDistributionBandwidth("", distributionBandwidth)
         wlan.SetOfdmGuardIntervalType("", guardIntervalType)
         wlan.SetOfdmLtfSize("", ltfSize)
         wlan.SetOfdmPEDisambiguity("", peDisambiguity)
         wlan.SelectMeasurements("", RFmxWlanMXMeasurementTypes.OfdmModAcc, True)
         wlan.OfdmModAcc.Configuration.ConfigureMeasurementLength("", measurementOffset, maximumMeasurementLength)
         wlan.OfdmModAcc.Configuration.SetUnusedToneErrorMaskReference("", unusedToneErrorMaskReference)
         wlan.OfdmModAcc.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount)
         wlan.Initiate("", "")
      End Sub

      Private Sub RetrieveResults()
         wlan.OfdmModAcc.Results.FetchCompositeRmsEvm("", timeout, compositeRmsEvmMean, compositeDataRmsEvmMean, compositePilotRmsEvmMean)
         wlan.OfdmModAcc.Results.FetchUnusedToneError("", timeout, unusedToneErrorMargin, unusedToneErrorMarginRUIndex)
         wlan.OfdmModAcc.Results.FetchUnusedToneErrorMarginPerRU("", timeout, unusedToneErrorMarginPerRU)
         wlan.OfdmModAcc.Results.FetchFrequencyErrorMean("", timeout, frequencyErrorMean)
         wlan.OfdmModAcc.Results.FetchFrequencyErrorCcdf10Percent("", timeout, frequencyErrorCcdf10Percent)
         wlan.OfdmModAcc.Results.FetchSymbolClockErrorMean("", timeout, symbolClockErrorMean)
         wlan.OfdmModAcc.Results.FetchPpduType("", timeout, ppduType)
         wlan.OfdmModAcc.Results.FetchPilotConstellationTrace("", timeout, pilotConstellation)
         wlan.OfdmModAcc.Results.FetchDataConstellationTrace("", timeout, dataConstellation)
         wlan.OfdmModAcc.Results.FetchUnusedToneErrorMeanTrace("", timeout, unusedToneError, unusedToneErrorMask)
      End Sub

      Private Sub PrintResults()
         Console.WriteLine("------------------EVM & Impairments------------------" & vbLf)
         Console.WriteLine("RMS EVM Mean (dB)                       :{0}", compositeRmsEvmMean)
         Console.WriteLine("Frequency Error Mean (Hz)               :{0}", frequencyErrorMean)
         Console.WriteLine("Frequency Error CCDF 10 % (Hz)          :{0}", frequencyErrorCcdf10Percent)
         Console.WriteLine("Symbol Clock Error Mean (ppm)           :{0}", symbolClockErrorMean)
         Console.WriteLine("PPDU Type                               :{0}", ppduType)
         Console.WriteLine(vbLf & "------------------Unused Tone Error------------------" & vbLf)
         Console.WriteLine("Margin (dB)                             :{0}", unusedToneErrorMargin)
         Console.WriteLine("Margin RU Index                         :{0}", unusedToneErrorMarginRUIndex)
         For i As Integer = 0 To unusedToneErrorMarginPerRU.Length - 1
            Console.WriteLine("Unused Tone Error Margin per RU(dB)     :{0}", unusedToneErrorMarginPerRU(i))
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
