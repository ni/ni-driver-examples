'Steps:
'1. Open a New RFmx Session.
'2. Configure Frequency Reference.
'3. Configure basic signal properties(Center Frequency, RF Attenuation And External Attenuation).
'4. Configure Trigger Type And Trigger Parameters.
'5. Configure Carrier Bandwidth.
'6. Configure Reference Level.
'7. Configure Duplex Mode.
'8. Configure Link Direction.
'9. Select ACP measurement And enable Traces.
'10. Configure Measurement Method.
'11. Configure Averaging Parameters for ACP measurement.
'12. Configure Sweep Time Parameters.
'13. Configure Noise Compensation Parameter.
'14. Initiate the Measurement.
'15. Fetch ACP Measurements And Traces.
'16. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.LteMX

Public Class RFmxLteAcpSingleCarrier
   Private instrSession As RFmxInstrMX
   Private lte As RFmxLteMX

   Private resourceName As String, frequencyReferenceSource As String, digitalEdgeSource As String

   Private frequencyReferenceFrequency As Double, centerFrequency As Double, referenceLevel As Double, autoSetReferenceLevel As Double,
            externalAttenuation As Double, rfAttenuation As Double, triggerDelay As Double, componentCarrierFrequency As Double,
            componentCarrierBandwidth As Double, measurementInterval As Double, sweepTimeInterval As Double, timeout As Double

   Private enableTrigger As Boolean, autoLevel As Boolean

   Private cellID As Integer, averagingCount As Integer
   Private numberOfOffsets As Integer, i As Integer

   Private rfAttenuationAuto As RFmxInstrMXRFAttenuationAuto
   Private averagingEnabled As RFmxLteMXAcpAveragingEnabled
   Private averagingType As RFmxLteMXAcpAveragingType
   Private digitalEdge As RFmxLteMXDigitalEdgeTriggerEdge
   Private duplexScheme As RFmxLteMXDuplexScheme
   Private measurementMethod As RFmxLteMXAcpMeasurementMethod
   Private noiseCompensationEnabled As RFmxLteMXAcpNoiseCompensationEnabled
   Private sweepTimeAuto As RFmxLteMXAcpSweepTimeAuto
   Private uplinkDownlinkConfiguration As RFmxLteMXUplinkDownlinkConfiguration
   Private linkDirection As RFmxLteMXLinkDirection

   Private absolutePower As Double, relativePower As Double
   Private lowerAbsolutePower As Double(), upperAbsolutePower As Double(), lowerRelativePower As Double(), upperRelativePower As Double()
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
      componentCarrierBandwidth = 10000000.0
      ' Hz
      componentCarrierFrequency = 0.0
      ' Hz
      cellID = 0

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

      lte.ComponentCarrier.Configure("", componentCarrierBandwidth, componentCarrierFrequency, cellID)

      If autoLevel Then
         lte.AutoLevel("", measurementInterval, autoSetReferenceLevel)
         Console.WriteLine("Reference level (dBm)  : {0}" & vbLf, autoSetReferenceLevel)
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

      lte.Acp.Results.ComponentCarrier.FetchMeasurement("", timeout, absolutePower, relativePower)

      For i = 0 To numberOfOffsets - 1
         lte.Acp.Results.FetchAbsolutePowersTrace("", timeout, i, absolutePowersTrace)
      Next

      For i = 0 To numberOfOffsets - 1
         lte.Acp.Results.FetchRelativePowersTrace("", timeout, i, relativePowersTrace)
      Next

      lte.Acp.Results.FetchSpectrum("", timeout, spectrum)
   End Sub

   Private Sub PrintResults()
      Console.WriteLine("Carrier Absolute Power  (dBm)   : {0}", absolutePower)
      Console.WriteLine(vbLf & "-----------Offset Channel Measurements-----------")
      For i As Integer = 0 To lowerRelativePower.Length - 1
         Console.WriteLine(vbLf & "Offset  {0}", i)
         Console.WriteLine("Lower Relative Power (dB)  : {0}", lowerRelativePower(i))
         Console.WriteLine("Upper Relative Power (dB)  : {0}", upperRelativePower(i))
         Console.WriteLine("Lower Absolute Power (dBm) : {0}", lowerAbsolutePower(i))
         Console.WriteLine("Upper Absolute Power (dBm) : {0}", upperAbsolutePower(i))
         Console.WriteLine("------------------------------------------")
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
