'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure Selected Ports.
'4. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
'5. Configure Trigger Parameters for Digital Edge Trigger.
'6. Configure Link Direction, Frequency Range, Channel Raster, Component Carrier Spacing and
'   Number of Component Carriers.
'7. Configure Subcarrier Spacing.
'8. Configure Component Carriers.
'9. Select OBW measurement and enable Traces.
'10. Configure Sweep Time Parameters.
'11. Configure Span Parameters for OBW measurement.
'12. Configure Averaging Parameters for OBW measurement.
'13. Initiate the Measurement.
'14. Fetch OBW Measurements and Traces.
'15. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.NRMX

Namespace NationalInstruments.Examples.RFmxNRObwContiguousMultiCarrier
   Public Class RFmxNRObwContiguousMultiCarrier
      Private instrSession As RFmxInstrMX
      Private NR As RFmxNRMX
      Private resourceName As String

      Private selectedPorts As String
      Private centerFrequency As Double
      Private referenceLevel As Double
      Private externalAttenuation As Double

      Private frequencyReferenceSource As String
      Private frequencyReferenceFrequency As Double

      Private enableTrigger As Boolean
      Private digitalEdgeSource As String
      Private digitalEdge As RFmxNRMXDigitalEdgeTriggerEdge
      Private triggerDelay As Double

      Private linkDirection As RFmxNRMXLinkDirection
      Private frequencyRange As RFmxNRMXFrequencyRange
      Private subcarrierSpacing As Double

      Private componentCarrierSpacingType As RFmxNRMXComponentCarrierSpacingType
      Private channelRaster As Double
      Private componentCarrierAtCenterFrequency As Integer

      Private Const NumberOfComponentCarriers As Integer = 2
      Private componentCarrierBandwidth As Double() = New Double(NumberOfComponentCarriers - 1) {}
      Private componentCarrierFrequency As Double() = New Double(NumberOfComponentCarriers - 1) {}

      Private sweepTimeAuto As RFmxNRMXObwSweepTimeAuto
      Private sweepTimeInterval As Double

      Private spanAuto As RFmxNRMXObwSpanAuto
      Private span As Double
	  
	  Private powerIntegrationMethod As RFmxNRMXObwPowerIntegrationMethod

      Private averagingEnabled As RFmxNRMXObwAveragingEnabled
      Private averagingCount As Integer
      Private averagingType As RFmxNRMXObwAveragingType

      Private subblockString As String
      Private carrierString As String

      Private timeout As Double

      Private spectrum As Spectrum(Of Single)

      Private occupiedBandwidth As Double
      ' (Hz)
      Private absolutePower As Double
      ' (dBm)
      Private startFrequency As Double
      ' (Hz)
      Private stopFrequency As Double
      ' (Hz)

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
            ' Close session
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

         enableTrigger = False
         digitalEdgeSource = RFmxNRMXConstants.PxiTriggerLine0
         digitalEdge = RFmxNRMXDigitalEdgeTriggerEdge.Rising
         triggerDelay = 0.0
         ' (s)

         linkDirection = RFmxNRMXLinkDirection.Uplink
         frequencyRange = RFmxNRMXFrequencyRange.Range1
         subcarrierSpacing = 30000.0
         ' (Hz)

         componentCarrierSpacingType = RFmxNRMXComponentCarrierSpacingType.Nominal
         channelRaster = 15000.0
         ' (Hz)
         componentCarrierAtCenterFrequency = -1

         componentCarrierBandwidth(0) = 100000000.0
         ' (Hz)
         componentCarrierBandwidth(1) = 100000000.0
         ' (Hz)
         componentCarrierFrequency(0) = -49980000.0
         ' (Hz)
         componentCarrierFrequency(1) = 50010000.0
         ' (Hz)

         sweepTimeAuto = RFmxNRMXObwSweepTimeAuto.True
         sweepTimeInterval = 0.001
         ' (s)

         spanAuto = RFmxNRMXObwSpanAuto.True
		 powerIntegrationMethod = RFmxNRMXObwPowerIntegrationMethod.[Normal]
         span = 399860000.0
         ' (Hz)

         averagingEnabled = RFmxNRMXObwAveragingEnabled.False
         averagingCount = 10
         averagingType = RFmxNRMXObwAveragingType.Rms

         timeout = 10.0
         ' (s)
      End Sub

      Private Sub InitializeInstr()
         instrSession = New RFmxInstrMX(resourceName, "")
      End Sub

      Private Sub ConfigureNR()
         NR = instrSession.GetNRSignalConfiguration()
         ' Create a new RFmx Session
         instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
         NR.SetSelectedPorts("", selectedPorts)
         NR.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
         NR.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger)

         NR.SetLinkDirection("", linkDirection)
         NR.SetFrequencyRange("", frequencyRange)
         NR.SetChannelRaster("", channelRaster)
         NR.SetComponentCarrierSpacingType("", componentCarrierSpacingType)
         NR.SetComponentCarrierAtCenterFrequency("", componentCarrierAtCenterFrequency)
         NR.ComponentCarrier.SetNumberOfComponentCarriers("", NumberOfComponentCarriers)

         carrierString = "carrier::all"
         NR.ComponentCarrier.SetBandwidthPartSubcarrierSpacing(carrierString, subcarrierSpacing)

         subblockString = RFmxNRMX.BuildSubblockString("", 0)
         For i As Integer = 0 To NumberOfComponentCarriers - 1
            carrierString = RFmxNRMX.BuildCarrierString(subblockString, i)
            NR.ComponentCarrier.SetBandwidth(carrierString, componentCarrierBandwidth(i))
            NR.ComponentCarrier.SetFrequency(carrierString, componentCarrierFrequency(i))
         Next

         NR.SelectMeasurements("", RFmxNRMXMeasurementTypes.Obw, True)

         NR.Obw.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval)
         NR.Obw.Configuration.SetSpanAuto("", spanAuto)
		 NR.Obw.Configuration.SetPowerIntegrationMethod("", powerIntegrationMethod)		 
         NR.Obw.Configuration.SetSpan("subblock0", span)
         NR.Obw.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)
         NR.Initiate("", "")
      End Sub

      Private Sub RetrieveResults()
         NR.Obw.Results.FetchMeasurement("", timeout, occupiedBandwidth, absolutePower,
            startFrequency, stopFrequency)
         NR.Obw.Results.FetchSpectrum("", timeout, spectrum)
      End Sub

      Private Sub PrintResults()
         Console.WriteLine("--------------- Measurement ------------------" & vbLf)
         Console.WriteLine("Occupied Bandwidth (Hz)  : {0}", occupiedBandwidth)
         Console.WriteLine("Absolute Power (dBm)     : {0}", absolutePower)
         Console.WriteLine("Start Frequency (Hz)     : {0}", startFrequency)
         Console.WriteLine("Stop Frequency (Hz)      : {0}" & vbLf, stopFrequency)
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
