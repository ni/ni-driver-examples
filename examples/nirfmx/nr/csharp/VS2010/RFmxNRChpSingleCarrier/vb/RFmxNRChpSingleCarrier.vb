'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure Selected Ports.
'4. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
'5. Configure Trigger Parameters for IQ Power Edge Trigger.
'6. Configure Link Direction, Frequency Range, Carrier Bandwidth and Subcarrier Spacing.
'7. Select CHP measurement and enable Traces.
'8. Configure Sweep Time Parameters.
'9. Configure Averaging Parameters for CHP measurement.
'10. Initiate the Measurement.
'11. Fetch CHP Measurements and Traces.
'12. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.NRMX

Namespace NationalInstruments.Examples.RFmxNRChpSingleCarrier
   Public Class RFmxNRChpSingleCarrier
      Private instrSession As RFmxInstrMX
      Private NR As RFmxNRMX
      Private resourceName As String

      Private selectedPorts As String
      Private centerFrequency As Double
      Private referenceLevel As Double
      Private externalAttenuation As Double

      Private frequencyReferenceSource As String
      Private frequencyReferenceFrequency As Double

      Private iqPowerEdgeEnabled As Boolean
      Private iqPowerEdgeLevel As Double
      Private triggerDelay As Double
      Private minimumQuietTimeMode As RFmxNRMXTriggerMinimumQuietTimeMode
      Private minimumQuietTime As Double

      Private linkDirection As RFmxNRMXLinkDirection
      Private frequencyRange As RFmxNRMXFrequencyRange
      Private carrierBandwidth As Double
      Private subcarrierSpacing As Double

      Private sweepTimeAuto As RFmxNRMXChpSweepTimeAuto
      Private sweepTimeInterval As Double

      Private averagingEnabled As RFmxNRMXChpAveragingEnabled
      Private averagingCount As Integer
      Private averagingType As RFmxNRMXChpAveragingType

      Private timeout As Double

      Private absolutePower As Double
      ' (dBm)
      Private relativePower As Double
      ' (dB)

      Private spectrum As Spectrum(Of Single)

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

         iqPowerEdgeEnabled = False
         iqPowerEdgeLevel = -20.0
         ' (dB or dBm)
         triggerDelay = 0.0
         ' (s)
         minimumQuietTimeMode = RFmxNRMXTriggerMinimumQuietTimeMode.Auto
         minimumQuietTime = 0.000008
         ' (s)

         linkDirection = RFmxNRMXLinkDirection.Uplink
         frequencyRange = RFmxNRMXFrequencyRange.Range1
         carrierBandwidth = 100000000.0
         ' (Hz)
         subcarrierSpacing = 30000.0
         ' (Hz)

         sweepTimeAuto = RFmxNRMXChpSweepTimeAuto.True
         sweepTimeInterval = 0.001
         ' (s)

         averagingEnabled = RFmxNRMXChpAveragingEnabled.False
         averagingCount = 10
         averagingType = RFmxNRMXChpAveragingType.Rms

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
         NR.ConfigureIQPowerEdgeTrigger("", "0", RFmxNRMXIQPowerEdgeTriggerSlope.Rising, iqPowerEdgeLevel,
            triggerDelay, minimumQuietTimeMode, minimumQuietTime, RFmxNRMXIQPowerEdgeTriggerLevelType.Relative,
            iqPowerEdgeEnabled)
         NR.SetLinkDirection("", linkDirection)
         NR.SetFrequencyRange("", frequencyRange)
         NR.ComponentCarrier.SetBandwidth("", carrierBandwidth)
         NR.ComponentCarrier.SetBandwidthPartSubcarrierSpacing("", subcarrierSpacing)
         NR.SelectMeasurements("", RFmxNRMXMeasurementTypes.Chp, True)
         NR.Chp.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval)
         NR.Chp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)
         NR.Initiate("", "")
      End Sub

      Private Sub RetrieveResults()
         NR.Chp.Results.ComponentCarrier.FetchMeasurement("", timeout, absolutePower, relativePower)
         NR.Chp.Results.FetchSpectrum("", timeout, spectrum)
      End Sub

      Private Sub PrintResults()
         Console.WriteLine("Absolute Power (dBm)     : {0}", absolutePower)
         Console.WriteLine("Relative Power (dB)      : {0}" & vbLf, relativePower)
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
