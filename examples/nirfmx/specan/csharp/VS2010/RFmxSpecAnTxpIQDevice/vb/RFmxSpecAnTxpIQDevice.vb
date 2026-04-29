'Steps:
'1. Open a new RFmx session.
'2. Configure the basic instrument properties (Clock Source and Clock Frequency).
'3. Configure Selected Ports.
'4. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
'5. Configure IQ Power Edge Trigger properties (Trigger Delay, IQ Power Edge Level, Minimum Quiet Time).
'6. Configure Txp measurement and enable the traces.
'7. Configure the Measurement Interval.
'8. Configure Rbw filter parameters.
'9. Configure Thresholding.
'10. Configure Averaging parameters.
'11. Configure Vbw filter parameters.
'12. Initiate Measurement.
'13. Fetch Txp Traces and Measurements.
'14. Close the RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.SpecAnMX

Public Class RFmxSpecAnTxpIQDevice
   Private instrSession As RFmxInstrMX
   Private specAn As RFmxSpecAnMX

   Private resourceName As String
   Private selectedPorts As String
   Private centerFrequency As Double
   Private referenceLevel As Double
   Private externalAttenuation As Double
   Private frequencySource As String
   Private frequency As Double

   Private triggerDelay As Double
   Private iqPowerEdgeLevel As Double
   Private minimumQuietTime As Double
   Private iqPowerEdgeEnabled As Boolean

   Private measurementInterval As Double

   Private rbwFilterType As RFmxSpecAnMXTxpRbwFilterType
   Private rbw As Double
   Private rrcAlpha As Double

   Private vbwAuto As RFmxSpecAnMXTxpVbwFilterAutoBandwidth
   Private vbw As Double
   Private vbwToRbwRatio As Double

   Private thresholdEnabled As RFmxSpecAnMXTxpThresholdEnabled
   Private thresholdType As RFmxSpecAnMXTxpThresholdType
   Private thresholdLevel As Double

   Private averagingEnabled As RFmxSpecAnMXTxpAveragingEnabled
   Private averagingCount As Integer
   Private averagingType As RFmxSpecAnMXTxpAveragingType

   Private timeout As Double
   Private averageMeanPower As Double
   Private peakToAverageRatio As Double
   Private maximumPower As Double
   Private minimumPower As Double
   Private power As AnalogWaveform(Of Single) = Nothing

   Public Sub Run()
      Try
         InitializeVariables()
         InitializeInstr()
         ConfigureSpecAn()
         RetrieveResults()

         PrintResults()
      Catch ex As Exception
         DisplayError(ex)
      Finally
         ' Close session

         CloseSession()
         Console.WriteLine("Press any key to exit.....")
         Console.ReadKey()
      End Try
   End Sub

   Private Sub InitializeVariables()
      ' Initialize input variables

      resourceName = "RFSA"
      selectedPorts = ""
      centerFrequency = 1000000000.0
      ' Hz
      referenceLevel = 0.0
      ' dBm
      externalAttenuation = 0.0
      ' dB

      frequencySource = RFmxInstrMXConstants.OnboardClock
      frequency = 10000000.0
      ' Hz

      iqPowerEdgeEnabled = False
      iqPowerEdgeLevel = -20.0
      ' dBm
      triggerDelay = 0.0
      ' seconds
      minimumQuietTime = 0.0
      ' seconds

      measurementInterval = 0.001
      ' seconds

      rbwFilterType = RFmxSpecAnMXTxpRbwFilterType.Gaussian
      rrcAlpha = 0.01
      rbw = 100000.0
      ' Hz

      vbwAuto = RFmxSpecAnMXTxpVbwFilterAutoBandwidth.[True]
      vbw = 30000.0
      ' Hz
      vbwToRbwRatio = 3

      thresholdEnabled = RFmxSpecAnMXTxpThresholdEnabled.[False]
      thresholdType = RFmxSpecAnMXTxpThresholdType.Relative
      thresholdLevel = -20.0
      ' (dB or dBm)

      averagingEnabled = RFmxSpecAnMXTxpAveragingEnabled.[False]
      averagingType = RFmxSpecAnMXTxpAveragingType.Rms
      averagingCount = 10

      timeout = 10
      ' seconds
   End Sub

   Private Sub InitializeInstr()
      ' Create a new RFmx Session

      instrSession = New RFmxInstrMX(resourceName, "")
   End Sub

   Private Sub ConfigureSpecAn()
      ' Get SpecAn signal

      specAn = instrSession.GetSpecAnSignalConfiguration()

      ' Configure measurement
      instrSession.ConfigureFrequencyReference("", frequencySource, frequency)
      specAn.SetSelectedPorts("", selectedPorts)
      specAn.ConfigureFrequency("", centerFrequency)
      specAn.ConfigureReferenceLevel("", referenceLevel)
      specAn.ConfigureExternalAttenuation("", externalAttenuation)
      specAn.ConfigureIQPowerEdgeTrigger("", "0", iqPowerEdgeLevel, RFmxSpecAnMXIQPowerEdgeTriggerSlope.Rising,
         triggerDelay, RFmxSpecAnMXTriggerMinimumQuietTimeMode.Manual, minimumQuietTime, iqPowerEdgeEnabled)
      specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Txp, True)
      specAn.Txp.Configuration.ConfigureMeasurementInterval("", measurementInterval)
      specAn.Txp.Configuration.ConfigureRbwFilter("", rbw, rbwFilterType, rrcAlpha)
      specAn.Txp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)
      specAn.Txp.Configuration.ConfigureVbwFilter("", vbwAuto, vbw, vbwToRbwRatio)
      specAn.Txp.Configuration.ConfigureThreshold("", thresholdEnabled, thresholdLevel, thresholdType)
      specAn.Initiate("", "")
   End Sub

   Private Sub RetrieveResults()
      ' Retrieve results

      specAn.Txp.Results.FetchPowerTrace("", timeout, power)
      specAn.Txp.Results.FetchMeasurement("", timeout, averageMeanPower, peakToAverageRatio, maximumPower, minimumPower)
   End Sub

   Private Sub PrintResults()
      Console.WriteLine("Average Mean Power  (dBm)      : {0}", averageMeanPower)
      Console.WriteLine("Peak to Average Ratio(dB)      : {0}", peakToAverageRatio)
      Console.WriteLine("Maximum Power (dBm)            : {0}", maximumPower)
      Console.WriteLine("Minimum Power (dBm)            : {0}", minimumPower)
   End Sub

   Private Sub CloseSession()
      Try
         If specAn IsNot Nothing Then
            specAn.Dispose()
            specAn = Nothing
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
