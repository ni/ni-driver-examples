'Steps:
'1. Open a new RFmx session.
'2.  Configure the frequency reference properties (Clock Source and Clock Frequency).
'3. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation).
'4. Configure IQ Power Edge Trigger properties (Trigger Delay, IQ Power Edge Level, Minimum Quiet Time).
'5. Configure Standard to 802.11b.
'6. Select DSSSModAcc measurement and enable the traces.
'7. Configure Measurement Length.
'8. Configure Pulse Shaping Filter Type and Parameter.
'9. Configure EVM unit.
'10. Configure Averaging parameters.
'11. Initiate Measurement.
'12. Fetch DSSSModAcc Traces and Measurements.
'13. Close the RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.WlanMX

Namespace NationalInstruments.Examples.RFmxWlanDsssModAcc

   Public Class RFmxWlanDsssModAcc
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

      Private measurementOffset As Integer
      Private maximumMeasurementLength As Integer

      Private pulseShapingFilterType As RFmxWlanMXDsssModAccPulseShapingFilterType
      Private pulseShapingFilterParameter As Double

      Private evmUnit As RFmxWlanMXDsssModAccEvmUnit

      Private averagingEnabled As RFmxWlanMXDsssModAccAveragingEnabled
      Private averagingCount As Integer

      Private timeout As Double

      Private rmsEvmMean As Double
      Private peakEvm80211_2016Maximum As Double
      Private peakEvm80211_2007Maximum As Double
      Private peakEvm80211_1999Maximum As Double
      Private frequencyErrorMean As Double
      Private chipClockErrorMean As Double
      Private numberOfChipsUsed As Integer

      Private dataModulationFormat As RFmxWlanMXDsssModAccDataModulationFormat
      Private payloadLength As Integer
      Private preambleType As RFmxWlanMXDsssModAccPreambleType
      Private lockedClocksBit As Integer
      Private headerCrcStatus As RFmxWlanMXDsssModAccPayloadHeaderCrcStatus
      Private psduCrcStatus As RFmxWlanMXDsssModAccPsduCrcStatus

      Private iqOriginOffsetMean As Double
      Private iqGainImbalanceMean As Double
      Private iqQuadratureErrorMean As Double

      Private evmPerChipMean As AnalogWaveform(Of Single)
      Private constellation As ComplexSingle()

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

         standard = RFmxWlanMXStandard.Standard802_11b

         measurementOffset = 0
         '(chips)
         maximumMeasurementLength = 1000
         '(chips)

         pulseShapingFilterType = RFmxWlanMXDsssModAccPulseShapingFilterType.Rectangular
         pulseShapingFilterParameter = 0.5

         evmUnit = RFmxWlanMXDsssModAccEvmUnit.Percentage

         averagingEnabled = RFmxWlanMXDsssModAccAveragingEnabled.False
         averagingCount = 10

         timeout = 10.0
         ' (s)

         rmsEvmMean = 0.0
         '(% or dB)
         peakEvm80211_2016Maximum = 0.0
         '(% or dB)
         peakEvm80211_2007Maximum = 0.0
         '(% or dB)
         peakEvm80211_1999Maximum = 0.0
         '(% or dB)
         frequencyErrorMean = 0.0
         '(Hz)
         chipClockErrorMean = 0.0
         '(ppm)
         numberOfChipsUsed = 0

         dataModulationFormat = RFmxWlanMXDsssModAccDataModulationFormat.Dsss1Mbps
         payloadLength = 0
         '(byte)
         preambleType = RFmxWlanMXDsssModAccPreambleType.Long
         lockedClocksBit = 0
         headerCrcStatus = RFmxWlanMXDsssModAccPayloadHeaderCrcStatus.Fail
         psduCrcStatus = RFmxWlanMXDsssModAccPsduCrcStatus.Fail

         iqOriginOffsetMean = 0.0
         '(dB)
         iqGainImbalanceMean = 0.0
         '(dB)
         iqQuadratureErrorMean = 0.0
         '(deg)
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
         wlan.SelectMeasurements("", RFmxWlanMXMeasurementTypes.DsssModAcc, True)
         wlan.DsssModAcc.Configuration.ConfigureMeasurementLength("", measurementOffset, maximumMeasurementLength)
         wlan.DsssModAcc.Configuration.SetPulseShapingFilterType("", pulseShapingFilterType)
         wlan.DsssModAcc.Configuration.SetPulseShapingFilterParameter("", pulseShapingFilterParameter)
         wlan.DsssModAcc.Configuration.ConfigureEvmUnit("", evmUnit)
         wlan.DsssModAcc.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount)
         wlan.Initiate("", "")
      End Sub

      Private Sub RetrieveResults()
         wlan.DsssModAcc.Results.FetchEvm("", timeout, rmsEvmMean, peakEvm80211_2016Maximum, peakEvm80211_2007Maximum, peakEvm80211_1999Maximum,
            frequencyErrorMean, chipClockErrorMean, numberOfChipsUsed)
         wlan.DsssModAcc.Results.FetchPpduInformation("", timeout, dataModulationFormat, payloadLength, preambleType, lockedClocksBit,
            headerCrcStatus, psduCrcStatus)
         wlan.DsssModAcc.Results.FetchIQImpairments("", timeout, iqOriginOffsetMean, iqGainImbalanceMean, iqQuadratureErrorMean)
         wlan.DsssModAcc.Results.FetchEvmPerChipMeanTrace("", timeout, evmPerChipMean)
         wlan.DsssModAcc.Results.FetchConstellationTrace("", timeout, constellation)
      End Sub

      Private Sub PrintResults()
         Console.WriteLine(vbLf & "---------------EVM---------------" & vbLf)
         Console.WriteLine("RMS EVM Mean (% or dB)                     :{0}", rmsEvmMean)
         Console.WriteLine("Peak EVM (802.11-2016) Maximum (% or dB)   :{0}", peakEvm80211_2016Maximum)
         Console.WriteLine("Peak EVM (802.11-2007) Maximum (% or dB)   :{0}", peakEvm80211_2007Maximum)
         Console.WriteLine("Peak EVM (802.11-1999) Maximum (% or dB)   :{0}", peakEvm80211_1999Maximum)
         Console.WriteLine("Number of Chips Used                       :{0}", numberOfChipsUsed)
         Console.WriteLine(vbLf & "---------------Impairments & PPDU Info---------------" & vbLf)
         Console.WriteLine("Frequency Error Mean (Hz)                  :{0}", frequencyErrorMean)
         Console.WriteLine("Chip Clock Error Mean (ppm)                :{0}", chipClockErrorMean)

         Console.WriteLine(vbLf & "---------------IQ Impairments---------------" & vbLf)
         Console.WriteLine("I/Q Origin Offset Mean (dB)                :{0}", iqOriginOffsetMean)
         Console.WriteLine("I/Q Gain Imbalance Mean (dB)               :{0}", iqGainImbalanceMean)
         Console.WriteLine("I/Q Quadrature Error Mean (deg)            :{0}", iqQuadratureErrorMean)

         Console.WriteLine(vbLf & "---------------PPDU Information---------------" & vbLf)
         Console.WriteLine("Data Modulation Format                     :{0}", dataModulationFormat)
         Console.WriteLine("Payload Length (bytes)                     :{0}", payloadLength)
         Console.WriteLine("Preamble Type                              :{0}", preambleType)
         Console.WriteLine("Locked Clock Bit                           :{0}", lockedClocksBit)
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
