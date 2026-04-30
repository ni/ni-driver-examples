'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure Selected Ports.
'4. Configure basic signal properties (Center Frequency, RF Attenuation and External Attenuation).
'5. Configure Trigger Parameters for IQ Power Edge Trigger.
'6. Configure Link Direction, Frequency Range and Carrier Bandwidth and Subcarrier Spacing.
'7. Configure Reference Level.
'8. Select ACP measurement and enable Traces.
'9. Configure Measurement Method.
'10. Configure Noise Compensation Parameter.
'11. Configure Sweep Time Parameters.
'12. Configure Averaging Parameters for ACP measurement.
'13. Initiate the Measurement.
'14. Fetch ACP Measurements and Traces.
'15. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.NRMX

Namespace NationalInstruments.Examples.RFmxNRAcpSingleCarrier
   Public Class RFmxNRAcpSingleCarrier
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

      Private iqPowerEdgeEnabled As Boolean
      Private iqPowerEdgeLevel As Double
      Private triggerDelay As Double
      Private minimumQuietTimeMode As RFmxNRMXTriggerMinimumQuietTimeMode
      Private minimumQuietTime As Double

      Private linkDirection As RFmxNRMXLinkDirection
      Private frequencyRange As RFmxNRMXFrequencyRange
      Private carrierBandwidth As Double
      Private subcarrierSpacing As Double
      Private measurementMethod As RFmxNRMXAcpMeasurementMethod
      Private noiseCompensationEnabled As RFmxNRMXAcpNoiseCompensationEnabled

      Private sweepTimeAuto As RFmxNRMXAcpSweepTimeAuto
      Private sweepTimeInterval As Double

      Private averagingEnabled As RFmxNRMXAcpAveragingEnabled
      Private averagingCount As Integer
      Private averagingType As RFmxNRMXAcpAveragingType

      Private timeout As Double

      Private absolutePower As Double
      '(dBm)
      Private relativePower As Double
      '(dBm)

      Private lowerRelativePower As Double()
      '(dB)
      Private upperRelativePower As Double()
      '(dB)
      Private lowerAbsolutePower As Double()
      '(dBm)
      Private upperAbsolutePower As Double()
      '(dBm)

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
            'Close session
            CloseSession()
            Console.WriteLine("Press any key to exit")
            Console.ReadKey()
         End Try
      End Sub

      Private Sub InitializeVariables()
         resourceName = "RFSA"

         selectedPorts = ""
         centerFrequency = 3500000000.0
         '(Hz)
         externalAttenuation = 0.0
         '(dB)

         autoLevel = True
         referenceLevel = 0.0
         '(dBm)
         measurementInterval = 0.01
         '(s)

         rfAttenuationAuto = RFmxInstrMXRFAttenuationAuto.True
         rfAttenuation = 10.0
         '(dB)

         frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
         frequencyReferenceFrequency = 10000000.0
         '(Hz)

         iqPowerEdgeEnabled = False
         iqPowerEdgeLevel = -20.0
         '(dB or dBm)
         triggerDelay = 0.0
         '(s)
         minimumQuietTimeMode = RFmxNRMXTriggerMinimumQuietTimeMode.Auto
         minimumQuietTime = 0.000008
         '(s)

         linkDirection = RFmxNRMXLinkDirection.Uplink
         frequencyRange = RFmxNRMXFrequencyRange.Range1
         carrierBandwidth = 100000000.0
         '(Hz)
         subcarrierSpacing = 30000.0
         '(Hz)
         measurementMethod = RFmxNRMXAcpMeasurementMethod.Normal
         noiseCompensationEnabled = RFmxNRMXAcpNoiseCompensationEnabled.False

         sweepTimeAuto = RFmxNRMXAcpSweepTimeAuto.True
         sweepTimeInterval = 0.001
         '(s)

         averagingEnabled = RFmxNRMXAcpAveragingEnabled.False
         averagingCount = 10
         averagingType = RFmxNRMXAcpAveragingType.Rms

         timeout = 10.0
         '(s)
      End Sub

      Private Sub InitializeInstr()
         instrSession = New RFmxInstrMX(resourceName, "")
      End Sub

      Private Sub ConfigureNR()
         NR = instrSession.GetNRSignalConfiguration()
         'Create a new RFmx Session
         instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
         NR.SetSelectedPorts("", selectedPorts)
         NR.ConfigureFrequency("", centerFrequency)
         NR.ConfigureExternalAttenuation("", externalAttenuation)
         instrSession.ConfigureRFAttenuation("", rfAttenuationAuto, rfAttenuation)
         NR.ConfigureIQPowerEdgeTrigger("", "0", RFmxNRMXIQPowerEdgeTriggerSlope.Rising, iqPowerEdgeLevel, triggerDelay,
         minimumQuietTimeMode, minimumQuietTime, RFmxNRMXIQPowerEdgeTriggerLevelType.Relative, iqPowerEdgeEnabled)
         NR.SetLinkDirection("", linkDirection)
         NR.SetFrequencyRange("", frequencyRange)
         NR.ComponentCarrier.SetBandwidth("", carrierBandwidth)
         NR.ComponentCarrier.SetBandwidthPartSubcarrierSpacing("", subcarrierSpacing)
         If autoLevel Then
            NR.AutoLevel("", measurementInterval, referenceLevel)
            Console.WriteLine("Reference level  (dBm)       : {0}", referenceLevel)
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

         NR.Acp.Results.ComponentCarrier.FetchMeasurement("", timeout, absolutePower, relativePower)

         For i As Integer = 0 To lowerRelativePower.Length - 1
            NR.Acp.Results.FetchRelativePowersTrace("", timeout, i, relativePowersTrace)
         Next

         NR.Acp.Results.FetchSpectrum("", timeout, spectrum)
      End Sub

      Private Sub PrintResults()
            Console.WriteLine(vbLf & "Carrier Absolute Power (dBm or dBm/Hz) : {0}", absolutePower)

            Console.WriteLine(vbLf & "-----------Offset Channel Measurements----------- " & vbLf)
         For i As Integer = 0 To lowerRelativePower.Length - 1
            Console.WriteLine("Offset  {0}", i)
            Console.WriteLine("Lower Relative Power (dB)              : {0}", lowerRelativePower(i))
            Console.WriteLine("Upper Relative Power (dB)              : {0}", upperRelativePower(i))
            Console.WriteLine("Lower Absolute Power (dBm or dBm/Hz)   : {0}", lowerAbsolutePower(i))
            Console.WriteLine("Upper Absolute Power (dBm or dBm/Hz)   : {0}", upperAbsolutePower(i))
            Console.WriteLine("-------------------------------------------------" & vbLf)
         Next
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
