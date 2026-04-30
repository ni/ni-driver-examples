'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure Selected Ports.
'4. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
'5. Configure Trigger Parameters for IQ Power Edge Trigger.
'6. Configure Link Direction, Frequency Range, Carrier Bandwidth and Subcarrier Spacing.
'7. Select OBW measurement and enable Traces.
'8. Configure Sweep Time Parameters.
'9. Configure Span Parameters for OBW measurement.
'10. Configure Averaging Parameters for OBW measurement.
'11. Initiate the Measurement.
'12. Fetch OBW Measurements and Traces.
'13. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.NRMX

Namespace NationalInstruments.Examples.RFmxNRObwSingleCarrier
   Public Class RFmxNRObwSingleCarrier
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

      Private sweepTimeAuto As RFmxNRMXObwSweepTimeAuto
      Private sweepTimeInterval As Double

      Private averagingEnabled As RFmxNRMXObwAveragingEnabled
      Private averagingCount As Integer
      Private averagingType As RFmxNRMXObwAveragingType

      Private spanAuto As RFmxNRMXObwSpanAuto
      Private span As Double

	  Private powerIntegrationMethod As RFmxNRMXObwPowerIntegrationMethod
	  
      Private timeout As Double

      Private occupiedBandwidth As Double
      ' (Hz)
      Private absolutePower As Double
      ' (dBm)
      Private startFrequency As Double
      ' (Hz)
      Private stopFrequency As Double
      ' (Hz)

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

         sweepTimeAuto = RFmxNRMXObwSweepTimeAuto.True
         sweepTimeInterval = 0.001
         ' (s)

         averagingEnabled = RFmxNRMXObwAveragingEnabled.False
         averagingCount = 10
         averagingType = RFmxNRMXObwAveragingType.Rms

         spanAuto = RFmxNRMXObwSpanAuto.True
		 powerIntegrationMethod = RFmxNRMXObwPowerIntegrationMethod.[Normal]
         span = 200000000.0
         ' (Hz)

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

         NR.SelectMeasurements("", RFmxNRMXMeasurementTypes.Obw, True)

         NR.Obw.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval)
         NR.Obw.Configuration.SetSpanAuto("", spanAuto)
		 NR.Obw.Configuration.SetPowerIntegrationMethod("", powerIntegrationMethod)
         NR.Obw.Configuration.SetSpan("subblock0", span)
         NR.Obw.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)
         NR.Initiate("", "")
      End Sub

      Private Sub RetrieveResults()
         NR.Obw.Results.FetchSpectrum("", timeout, spectrum)

         NR.Obw.Results.FetchMeasurement("", timeout, occupiedBandwidth, absolutePower,
            startFrequency, stopFrequency)
      End Sub

      Private Sub PrintResults()
         Console.WriteLine("----------------- Measurement -----------------" & vbLf)
         Console.WriteLine("Occupied Bandwidth (Hz)   : {0}", occupiedBandwidth)
         Console.WriteLine("Absolute Power (dBm)      : {0}", absolutePower)
         Console.WriteLine("Start Frequency (Hz)      : {0}", startFrequency)
         Console.WriteLine("Stop Frequency (Hz)       : {0}" & vbLf, stopFrequency)
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
