'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure Selected Ports.
'4. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
'5. Configure Trigger Parameters for Digital Edge Trigger.
'6. Configure Link Direction, Frequency Range, Channel Raster, Component Carrier Spacing and Number of Component Carriers.
'7. Configure Component Carriers.
'8. Configure Subcarrier Spacing for all Component Carrier.
'9. Select TXP measurement and enable Traces. 
'10. Configure Measurement Offset & Measurement Length Parameters and Averaging Parameters for TXP measurement.
'11. Initiate the Measurement.
'12. Fetch TXP Traces and Measurements.
'13. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.NRMX

Namespace NationalInstruments.Examples.RFmxNRTxpContiguousMultiCarrier
   Public Class RFmxNRTxpContiguousMultiCarrier
      Private instrSession As RFmxInstrMX
      Private NR As RFmxNRMX
      Private resourceName As String

      Private selectedPorts As String
      Private centerFrequency As Double
      Private referenceLevel As Double
      Private externalAttenuation As Double

      Private frequencyReferenceSource As String
      Private frequencyReferenceFrequency As Double

      Private digitalEdgeEnabled As Boolean
      Private digitalEdgeSource As String
      Private triggerDelay As Double
      Private digitalEdge As RFmxNRMXDigitalEdgeTriggerEdge

      Private linkDirection As RFmxNRMXLinkDirection
      Private frequencyRange As RFmxNRMXFrequencyRange

      Private componentCarrierSpacingType As RFmxNRMXComponentCarrierSpacingType
      Private channelRaster As Double
      Private componentCarrierAtCenterFrequency As Integer
      Private subcarrierSpacing As Double

      Const NumberOfComponentCarriers As Integer = 2
      Private componentCarrierBandwidth As Double() = New Double(NumberOfComponentCarriers - 1) {}
      Private componentCarrierFrequency As Double() = New Double(NumberOfComponentCarriers - 1) {}

      Private measurementLength As Double
      Private measurementOffset As Double

      Private averagingEnabled As RFmxNRMXTxpAveragingEnabled
      Private averagingCount As Integer

      Private timeout As Double

      Private subblockString As String
      Private carrierString As String

      Private averagePowerMean As Double
      ' (dBm)
      Private peakPowerMaximum As Double
      ' (dB)

      Private power As AnalogWaveform(Of Single)

      Public Sub Run()
         Try
            InitializeVariables()
            InitializeInstr()
            ConfigureNR()
            RetrieveResults()
            PrintResults()
         Catch ex As Exception
            DisplayError(ex)
         Finally
            CloseSession()
            Console.WriteLine("Press any key to exit")
            Console.ReadKey()
         End Try
      End Sub

      Private Sub InitializeVariables()
         resourceName = "RFSA"

         selectedPorts = ""
         centerFrequency = 3500000000.0
         ' (Hz)
         referenceLevel = 0.0
         ' (dBm)
         externalAttenuation = 0.0
         ' (dB)

         frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
         frequencyReferenceFrequency = 10000000.0
         ' (Hz)

         digitalEdgeEnabled = False
         digitalEdgeSource = RFmxNRMXConstants.PxiTriggerLine0
         triggerDelay = 0.0
         ' (s)
         digitalEdge = RFmxNRMXDigitalEdgeTriggerEdge.Rising

         linkDirection = RFmxNRMXLinkDirection.Uplink
         frequencyRange = RFmxNRMXFrequencyRange.Range1

         componentCarrierSpacingType = RFmxNRMXComponentCarrierSpacingType.Nominal
         channelRaster = 15000.0
         ' (Hz)
         componentCarrierAtCenterFrequency = -1
         subcarrierSpacing = 30000.0
         ' (Hz)

         componentCarrierBandwidth(0) = 100000000.0
         ' (Hz)
         componentCarrierBandwidth(1) = 100000000.0
         ' (Hz)
         componentCarrierFrequency(0) = -49980000.0
         ' (Hz)
         componentCarrierFrequency(1) = 50010000.0
         ' (Hz)

         measurementOffset = 0.0
         ' (s)
         measurementLength = 0.001
         ' (s)

         averagingEnabled = RFmxNRMXTxpAveragingEnabled.False
         averagingCount = 10

         timeout = 10.0
         ' (s)
      End Sub

      Private Sub InitializeInstr()
         instrSession = New RFmxInstrMX(resourceName, "")
      End Sub

      Private Sub ConfigureNR()
         NR = instrSession.GetNRSignalConfiguration()
         instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
         NR.SetSelectedPorts("", selectedPorts)
         NR.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
         NR.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, digitalEdgeEnabled)
         NR.SetLinkDirection("", linkDirection)
         NR.SetFrequencyRange("", frequencyRange)
         NR.SetChannelRaster("", channelRaster)
         NR.SetComponentCarrierSpacingType("", componentCarrierSpacingType)
         NR.SetComponentCarrierAtCenterFrequency("", componentCarrierAtCenterFrequency)

         NR.ComponentCarrier.SetNumberOfComponentCarriers("", NumberOfComponentCarriers)

         subblockString = RFmxNRMX.BuildSubblockString("", 0)
         For i As Integer = 0 To NumberOfComponentCarriers - 1
            carrierString = RFmxNRMX.BuildCarrierString(subblockString, i)
            NR.ComponentCarrier.SetBandwidth(carrierString, componentCarrierBandwidth(i))
            NR.ComponentCarrier.SetFrequency(carrierString, componentCarrierFrequency(i))
         Next

         carrierString = "carrier::all"
         NR.ComponentCarrier.SetBandwidthPartSubcarrierSpacing(carrierString, subcarrierSpacing)

         NR.SelectMeasurements("", RFmxNRMXMeasurementTypes.Txp, True)

         NR.Txp.Configuration.SetMeasurementInterval("", measurementLength)
         NR.Txp.Configuration.SetMeasurementOffset("", measurementOffset)
         NR.Txp.Configuration.SetAveragingEnabled("", averagingEnabled)
         NR.Txp.Configuration.SetAveragingCount("", averagingCount)

         NR.Initiate("", "")
      End Sub

      Private Sub RetrieveResults()
         NR.Txp.Results.FetchMeasurement("", timeout, averagePowerMean, peakPowerMaximum)
         NR.Txp.Results.FetchPowerTrace("", timeout, power)
      End Sub

      Private Sub PrintResults()
         Console.WriteLine(vbLf & "-------------Measurement------------" & vbLf)
         Console.WriteLine("Average Power Mean (dBm)      : {0}", averagePowerMean)
         Console.WriteLine("Peak Power Maximum (dBm)      : {0}" & vbLf, peakPowerMaximum)
      End Sub

      Private Sub CloseSession()
         If NR IsNot Nothing Then
            NR.Dispose()
            NR = Nothing
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
