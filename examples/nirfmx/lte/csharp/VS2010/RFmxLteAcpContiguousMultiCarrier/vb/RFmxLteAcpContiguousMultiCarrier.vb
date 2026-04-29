'Steps:
'1. Open a New RFmx Session.
'2. Configure Frequency Reference.
'3. Configure basic signal properties(Center Frequency, RF Attenuation And External Attenuation).
'4. Configure Trigger Type And Trigger Parameters.
'5. Configure Component Carrier Spacing.
'6. Configure Component Carriers.
'7. Configure Reference Level.
'8. Configure Duplex Mode.
'9. Configure Link Direction.
'10. Select ACP measurement And enable Traces.
'11. Configure Measurement Method.
'12. Configure Averaging Parameters for ACP measurement.
'13. Configure Sweep Time Parameters.
'14. Configure Noise Compensation Parameter.
'15. Initiate the Measurement.
'16. Fetch ACP Measurements And Traces.
'17. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.LteMX

Public Class RFmxLteAcpContiguousMultiCarrier
   Private instrSession As RFmxInstrMX
   Private lte As RFmxLteMX

   Private resourceName As String, frequencyReferenceSource As String, digitalEdgeSource As String

   Private frequencyReferenceFrequency As Double, centerFrequency As Double, referenceLevel As Double,
       autoSetReferenceLevel As Double, externalAttenuation As Double, rfAttenuation As Double, triggerDelay As Double,
       measurementInterval As Double, sweepTimeInterval As Double, timeout As Double
   Private enableTrigger As Boolean, autoLevel As Boolean

   Private averagingCount As Integer, componentCarrierAtCenterFrequency As Integer
   Private i As Integer, numberOfOffsets As Integer

   Const NumberOfComponentCarriers As Integer = 2
   Private componentCarrierFrequency As Double() = {-9900000.0, 9900000.0}
   Private componentCarrierBandwidth As Double() = {20000000.0, 20000000.0}

   Private rfAttenuationAuto As RFmxInstrMXRFAttenuationAuto
   Private averagingEnabled As RFmxLteMXAcpAveragingEnabled
   Private averagingType As RFmxLteMXAcpAveragingType
   Private digitalEdge As RFmxLteMXDigitalEdgeTriggerEdge
   Private duplexScheme As RFmxLteMXDuplexScheme
   Private componentCarrierSpacingType As RFmxLteMXComponentCarrierSpacingType
   Private measurementMethod As RFmxLteMXAcpMeasurementMethod
   Private noiseCompensationEnabled As RFmxLteMXAcpNoiseCompensationEnabled
   Private sweepTimeAuto As RFmxLteMXAcpSweepTimeAuto
   Private uplinkDownlinkConfiguration As RFmxLteMXUplinkDownlinkConfiguration
   Private linkDirection As RFmxLteMXLinkDirection

   Private totalAggregatedPower As Double
   Private lowerAbsolutePower As Double(), upperAbsolutePower As Double(), lowerRelativePower As Double(),
       upperRelativePower As Double()
   Private spectrum As Spectrum(Of Single)
   Private absolutePowersTrace As Spectrum(Of Single)
   Private relativePowersTrace As Spectrum(Of Single)

   Public Sub Run()
      Try
         InitializeVariables()
         InitializeInstr()
         ConfigureLte()
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

      centerFrequency = 1950000000.0
      ' Hz
      referenceLevel = 0.0
      ' dBm
      externalAttenuation = 0.0
      ' dB

      autoLevel = True

      rfAttenuationAuto = RFmxInstrMXRFAttenuationAuto.[True]
      rfAttenuation = 10.0
      ' dB

      frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
      frequencyReferenceFrequency = 10000000.0
      ' Hz

      enableTrigger = False
      digitalEdgeSource = RFmxLteMXConstants.Pfi0
      digitalEdge = RFmxLteMXDigitalEdgeTriggerEdge.Rising
      triggerDelay = 0.0
      ' seconds

      uplinkDownlinkConfiguration = RFmxLteMXUplinkDownlinkConfiguration.Configuration0
      duplexScheme = RFmxLteMXDuplexScheme.Fdd

      linkDirection = RFmxLteMXLinkDirection.Uplink

      componentCarrierSpacingType = RFmxLteMXComponentCarrierSpacingType.Nominal
      componentCarrierAtCenterFrequency = -1

      measurementInterval = 0.01

      measurementMethod = RFmxLteMXAcpMeasurementMethod.Normal

      noiseCompensationEnabled = RFmxLteMXAcpNoiseCompensationEnabled.[False]

      sweepTimeAuto = RFmxLteMXAcpSweepTimeAuto.[True]
      sweepTimeInterval = 0.001
      ' seconds

      averagingEnabled = RFmxLteMXAcpAveragingEnabled.[False]
      averagingCount = 10
      averagingType = RFmxLteMXAcpAveragingType.Rms

      numberOfOffsets = 3

      timeout = 10.0
      ' seconds

   End Sub

   Private Sub InitializeInstr()
      ' Create a new RFmx Session

      instrSession = New RFmxInstrMX(resourceName, "")
   End Sub

   Private Sub ConfigureLte()
      ' Get Lte signal

      lte = instrSession.GetLteSignalConfiguration()

      ' Configure measurement

      instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)

      lte.ConfigureFrequency("", centerFrequency)

      lte.ConfigureExternalAttenuation("", externalAttenuation)

      instrSession.ConfigureRFAttenuation("", rfAttenuationAuto, rfAttenuation)

      lte.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger)

      lte.ComponentCarrier.ConfigureSpacing("", componentCarrierSpacingType, componentCarrierAtCenterFrequency)

      lte.ConfigureNumberOfComponentCarriers("", NumberOfComponentCarriers)

      lte.ComponentCarrier.ConfigureArray("", componentCarrierBandwidth, componentCarrierFrequency, Nothing)

      If autoLevel Then
         lte.AutoLevel("", measurementInterval, autoSetReferenceLevel)
         Console.WriteLine("Reference level (dBm)          : {0}" & vbLf, autoSetReferenceLevel)
      Else
         lte.ConfigureReferenceLevel("", referenceLevel)
      End If

      lte.ConfigureDuplexScheme("", duplexScheme, uplinkDownlinkConfiguration)

      lte.ConfigureLinkDirection("", linkDirection)

      lte.SelectMeasurements("", RFmxLteMXMeasurementTypes.Acp, True)

      lte.Acp.Configuration.ConfigureMeasurementMethod("", measurementMethod)

      lte.Acp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)

      lte.Acp.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval)

      lte.Acp.Configuration.ConfigureNoiseCompensationEnabled("", noiseCompensationEnabled)

      lte.Initiate("", "")
   End Sub

   Private Sub RetrieveResults()
      ' Retrieve results

      lte.Acp.Results.FetchOffsetMeasurementArray("", timeout, lowerRelativePower, upperRelativePower,
                                                  lowerAbsolutePower, upperAbsolutePower)

      lte.Acp.Results.FetchTotalAggregatedPower("", timeout, totalAggregatedPower)

      For i = 0 To numberOfOffsets - 1
         lte.Acp.Results.FetchAbsolutePowersTrace("", timeout, i, absolutePowersTrace)
      Next

      For i = 0 To numberOfOffsets - 1
         lte.Acp.Results.FetchRelativePowersTrace("", timeout, i, relativePowersTrace)
      Next

      lte.Acp.Results.FetchSpectrum("", timeout, spectrum)

   End Sub

   Private Sub PrintResults()
      Console.WriteLine("Total Aggregated Power  (dBm)  : {0}", totalAggregatedPower)
      Console.WriteLine(vbLf & "----------Offset Channel Measurements----------")
      For i As Integer = 0 To lowerRelativePower.Length - 1
         Console.WriteLine(vbLf & "Offset  : {0}", i)
         Console.WriteLine("Lower Relative Power (dB)      : {0}", lowerRelativePower(i))
         Console.WriteLine("Upper Relative Power (dB)      : {0}", upperRelativePower(i))
         Console.WriteLine("Lower Absolute Power (dBm)     : {0}", lowerAbsolutePower(i))
         Console.WriteLine("Upper Absolute Power (dBm)     : {0}", upperAbsolutePower(i))
      Next
   End Sub

   Private Sub CloseSession()
      Try
         If lte IsNot Nothing Then
            lte.Dispose()
            lte = Nothing
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
