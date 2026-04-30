'Steps:
'1. Open a New RFmx Session.
'2. Configure Frequency Reference.
'3. Configure basic signal properties (Center Frequency And External Attenuation).
'4. Configure Trigger Type And Trigger Parameters.
'5. Configure Carrier Bandwidth.
'6. Configure Uplink Subcarrier Spacing.
'7. Select ACP measurement And enable Traces.
'8. Configure Averaging Parameters for ACP measurement.
'9. Configure Sweep Time Parameters.
'10. Initiate the Measurement.
'11. Fetch ACP Measurements And Traces.
'12. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.LteMX

Public Class RFmxLteNBIoTAcp
   Private instrSession As RFmxInstrMX
   Private lte As RFmxLteMX

   Private resourceName As String, frequencyReferenceSource As String, iqPowerEdgeTriggerSource As String

   Private frequencyReferenceFrequency As Double, centerFrequency As Double, referenceLevel As Double,
      externalAttenuation As Double, triggerDelay As Double, componentCarrierFrequency As Double,
      componentCarrierBandwidth As Double, sweepTimeInterval As Double, timeout As Double

   Private enableTrigger As Boolean
   Private cellID As Integer, averagingCount As Integer, numberOfOffsets As Integer
   Private i As Integer
   Private iqPowerEdgeTriggerLevel As Double, minimumQuietTimeDuration As Double
   Private minimumQuietTimeMode As RFmxLteMXTriggerMinimumQuietTimeMode
   Private averagingEnabled As RFmxLteMXAcpAveragingEnabled
   Private iqPowerEdgeTriggerLevelType As RFmxLteMXIQPowerEdgeTriggerLevelType
   Private iqPowerEdgeTriggerSlope As RFmxLteMXIQPowerEdgeTriggerSlope
   Private averagingType As RFmxLteMXAcpAveragingType
   Private sweepTimeAuto As RFmxLteMXAcpSweepTimeAuto
   Private uplinkSubcarrierSpacing As RFmxLteMXNBIoTUplinkSubcarrierSpacing
   Private linkDirection As RFmxLteMXLinkDirection

   Private absolutePower As Double, relativePower As Double
   Private lowerAbsolutePower As Double(), upperAbsolutePower As Double(), lowerRelativePower As Double(),
      upperRelativePower As Double()
   Private spectrum As Spectrum(Of Single)
   Private absolutePowerTrace As Spectrum(Of Single)
   Private relativePowerTrace As Spectrum(Of Single)


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

      enableTrigger = True
      iqPowerEdgeTriggerSource = "0"
      iqPowerEdgeTriggerSlope = RFmxLteMXIQPowerEdgeTriggerSlope.Rising
      iqPowerEdgeTriggerLevel = -20.0
      ' dB
      iqPowerEdgeTriggerLevelType = RFmxLteMXIQPowerEdgeTriggerLevelType.Relative
      minimumQuietTimeDuration = 0.0001
      ' seconds
      minimumQuietTimeMode = RFmxLteMXTriggerMinimumQuietTimeMode.Auto
      triggerDelay = 0.0
      ' seconds

      frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
      frequencyReferenceFrequency = 10000000.0
      ' Hz

      uplinkSubcarrierSpacing = RFmxLteMXNBIoTUplinkSubcarrierSpacing.SubcarrierSpacing15kHz
	  
	  linkDirection = RFmxLteMXLinkDirection.Uplink

      sweepTimeAuto = RFmxLteMXAcpSweepTimeAuto.True
      sweepTimeInterval = 0.001
      ' seconds

      averagingEnabled = RFmxLteMXAcpAveragingEnabled.False
      averagingCount = 10
      averagingType = RFmxLteMXAcpAveragingType.Rms

      componentCarrierBandwidth = 200000.0
      ' Hz
      componentCarrierFrequency = 0.0
      ' Hz
      cellID = 0

      numberOfOffsets = 2

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

      lte.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)

      lte.ConfigureIQPowerEdgeTrigger("", iqPowerEdgeTriggerSource, iqPowerEdgeTriggerSlope, iqPowerEdgeTriggerLevel,
         triggerDelay, minimumQuietTimeMode, minimumQuietTimeDuration, iqPowerEdgeTriggerLevelType, enableTrigger)

      lte.ComponentCarrier.Configure("", componentCarrierBandwidth, componentCarrierFrequency, cellID)

      lte.ComponentCarrier.ConfigureNBIoTComponentCarrier("", cellID, uplinkSubcarrierSpacing)

      lte.ConfigureLinkDirection("", linkDirection)

      lte.SelectMeasurements("", RFmxLteMXMeasurementTypes.Acp, True)

      lte.Acp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)

      lte.Acp.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval)

      lte.Initiate("", "")
   End Sub

   Private Sub RetrieveResults()
      ' Retrieve results

      lte.Acp.Results.FetchOffsetMeasurementArray("", timeout, lowerRelativePower, upperRelativePower,
         lowerAbsolutePower, upperAbsolutePower)

      lte.Acp.Results.ComponentCarrier.FetchMeasurement("", timeout, absolutePower, relativePower)

      For i = 0 To numberOfOffsets - 1
         lte.Acp.Results.FetchAbsolutePowersTrace("", timeout, i, absolutePowerTrace)
      Next

      For i = 0 To numberOfOffsets - 1
         lte.Acp.Results.FetchRelativePowersTrace("", timeout, i, relativePowerTrace)
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
