'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure Selected Ports.
'4. Configure basic signal properties(Center Frequency, RF Attenuation and External Attenuation).
'5. Configure Trigger Parameters for Digital Edge Trigger.
'6. Configure Link Direction, Frequency Range, Channel Raster, Component Carrier Spacing and
'   Number of Component Carriers.
'7. Configure Subcarrier Spacing.
'8. Configure Component Carriers.
'9. Configure Reference Level.
'10. Select ACP measurement and enable Traces.
'11. Configure Measurement Method.
'12. Configure Noise Compensation Parameter.
'13. Configure Sweep Time Parameters.
'14. Configure Averaging Parameters for ACP measurement.
'15. Initiate the Measurement.
'16. Fetch ACP Measurements and Traces.
'17. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.NRMX

Namespace NationalInstruments.Examples.RFmxNRAcpContiguousMultiCarrier
   Public Class RFmxNRAcpContiguousMultiCarrier
      Private instrSession As RFmxInstrMX
      Private NR As RFmxNRMX
      Private resourceName As String

      Private selectedPorts As String
      Private centerFrequency As Double
      Private externalAttenuation As Double

      Private autoLevel As Boolean
      Private referenceLevel As Double
      Private measurementInterval As Double

      Private rfAttenuationAuto As RFmxInstrMXRFAttenuationAuto
      Private rfAttenuation As Double

      Private frequencyReferenceSource As String
      Private frequencyReferenceFrequency As Double

      Private enableTrigger As Boolean
      Private digitalEdgeSource As String
      Private digitalEdge As RFmxNRMXDigitalEdgeTriggerEdge
      Private triggerDelay As Double

      Private linkDirection As RFmxNRMXLinkDirection
      Private frequencyRange As RFmxNRMXFrequencyRange

      Private componentCarrierSpacingType As RFmxNRMXComponentCarrierSpacingType
      Private channelRaster As Double
      Private componentCarrierAtCenterFrequency As Integer
      Private subcarrierSpacing As Double

      Const NumberOfComponentCarriers As Integer = 2
      Private componentCarrierBandwidth As Double() = New Double(NumberOfComponentCarriers - 1) {}
      Private componentCarrierFrequency As Double() = New Double(NumberOfComponentCarriers - 1) {}

      Private measurementMethod As RFmxNRMXAcpMeasurementMethod
      Private noiseCompensationEnabled As RFmxNRMXAcpNoiseCompensationEnabled

      Private sweepTimeAuto As RFmxNRMXAcpSweepTimeAuto
      Private sweepTimeInterval As Double

      Private averagingEnabled As RFmxNRMXAcpAveragingEnabled
      Private averagingCount As Integer
      Private averagingType As RFmxNRMXAcpAveragingType

      Private subblockString As String
      Private carrierString As String

      Private timeout As Double

      Private totalAggregatedPower As Double
      ' (dBm)

      Private lowerRelativePower As Double()
      ' (dB)
      Private upperRelativePower As Double()
      ' (dB)
      Private lowerAbsolutePower As Double()
      ' (dBm)
      Private upperAbsolutePower As Double()
      ' (dBm)

      Private spectrum As Spectrum(Of Single)
      Private relativePowersTrace As Spectrum(Of Single)

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
         externalAttenuation = 0.0
         ' (dB)

         autoLevel = True
         referenceLevel = 0.0
         ' (dBm)
         measurementInterval = 0.01
         ' (s)

         rfAttenuationAuto = RFmxInstrMXRFAttenuationAuto.True
         rfAttenuation = 10.0
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

         measurementMethod = RFmxNRMXAcpMeasurementMethod.Normal
         noiseCompensationEnabled = RFmxNRMXAcpNoiseCompensationEnabled.False

         sweepTimeAuto = RFmxNRMXAcpSweepTimeAuto.True
         sweepTimeInterval = 0.001
         ' (s)

         averagingEnabled = RFmxNRMXAcpAveragingEnabled.False
         averagingCount = 10
         averagingType = RFmxNRMXAcpAveragingType.Rms

         timeout = 10.0
         ' (s)
      End Sub

      Private Sub InitializeInstr()
         ' Create a new RFmx Session
         instrSession = New RFmxInstrMX(resourceName, "")
      End Sub

      Private Sub ConfigureNR()
         NR = instrSession.GetNRSignalConfiguration()
         instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
         NR.SetSelectedPorts("", selectedPorts)
         NR.ConfigureFrequency("", centerFrequency)
         NR.ConfigureExternalAttenuation("", externalAttenuation)
         instrSession.ConfigureRFAttenuation("", rfAttenuationAuto, rfAttenuation)
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

         If autoLevel Then
            NR.AutoLevel("", measurementInterval, referenceLevel)
            Console.WriteLine("Reference level (dBm)           : {0}" & vbLf, referenceLevel)
         Else
            NR.ConfigureReferenceLevel("", referenceLevel)
         End If

         NR.SelectMeasurements("", RFmxNRMXMeasurementTypes.Acp, True)
         NR.Acp.Configuration.ConfigureMeasurementMethod("", measurementMethod)
         NR.Acp.Configuration.ConfigureNoiseCompensationEnabled("", noiseCompensationEnabled)
         NR.Acp.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval)
         NR.Acp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)
         NR.Initiate("", "")
      End Sub

      Private Sub RetrieveResults()
         NR.Acp.Results.FetchOffsetMeasurementArray("", timeout, lowerRelativePower, upperRelativePower,
            lowerAbsolutePower, upperAbsolutePower)

         NR.Acp.Results.FetchTotalAggregatedPower("", timeout, totalAggregatedPower)

         For i As Integer = 0 To lowerRelativePower.Length - 1
            NR.Acp.Results.FetchRelativePowersTrace("", timeout, i, relativePowersTrace)
         Next

         NR.Acp.Results.FetchSpectrum("", timeout, spectrum)
      End Sub

      Private Sub PrintResults()
            Console.WriteLine("Total Aggregated Power (dBm or dBm/Hz)    : {0}", totalAggregatedPower)
            Console.WriteLine(vbLf & "-----------Offset Channel Measurements------------" & vbLf)
         For i As Integer = 0 To lowerRelativePower.Length - 1
            Console.WriteLine("Offset  : {0}", i)
            Console.WriteLine("Lower Relative Power (dB)                 : {0}", lowerRelativePower(i))
            Console.WriteLine("Upper Relative Power (dB)                 : {0}", upperRelativePower(i))
            Console.WriteLine("Lower Absolute Power (dBm or dBm/Hz)      : {0}", lowerAbsolutePower(i))
            Console.WriteLine("Upper Absolute Power (dBm or dBm/Hz)      : {0}", upperAbsolutePower(i))
            Console.WriteLine("---------------------------------------------------" & vbLf)
         Next
      End Sub

      Private Sub CloseSession()
         Try
            If NR IsNot Nothing Then
               NR.Dispose()
               NR = Nothing
            End If

            If instrSession IsNot Nothing Then
               instrSession.Close()
               instrSession = Nothing
            End If
         Catch ex As Exception
            DisplayError(ex)
         End Try
      End Sub

      Private Shared Sub DisplayError(ex As Exception)
         Console.WriteLine("ERROR:" & vbLf & Convert.ToString(ex.[GetType]()) & ": " & ex.Message)
      End Sub

   End Class
End Namespace
