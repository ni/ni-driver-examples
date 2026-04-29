'Steps:
'1. Open a new RFmx session
'2. Configure the basic instrument properties (Clock Source and Clock Frequency)
'3. Configure Selected Ports
'4. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
'5. Configure IQ Power Edge Trigger properties (Trigger Delay, IQ Power Edge Level, Min Quiet Time)
'6. Configure TXP measurement and enable the traces
'7. Configure TXP Measurement Interval
'8. Configure TXP RBW Filter
'9. Configure TXP Threshold
'10. Configure TXP Averaging
'11. Initiate Measurement
'12. Fetch TXP Traces and Measurements
'13. Close the RFmx Session

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.SpecAnMX

Public Class RFmxSpecAnTxp
   Private instrSession As RFmxInstrMX
   Private specAn As RFmxSpecAnMX

   Private selectedPorts As String
   Private centerFrequency As Double, referenceLevel As Double, externalAttenuation As Double
   Private measurementInterval As Double, rbw As Double, timeout As Double, rrcAlpha As Double, frequency As Double
   Private vbw As Double, vbwToRbwRatio As Double
   Private resourceName As String
   Private averagingEnabled As RFmxSpecAnMXTxpAveragingEnabled
   Private averagingCount As Integer
   Private averagingType As RFmxSpecAnMXTxpAveragingType
   Private rbwFilterType As RFmxSpecAnMXTxpRbwFilterType
   Private thresholdEnabled As RFmxSpecAnMXTxpThresholdEnabled
   Private thresholdType As RFmxSpecAnMXTxpThresholdType
   Private vbwAuto As RFmxSpecAnMXTxpVbwFilterAutoBandwidth
   Private triggerDelay As Double, iqPowerEdgeLevel As Double, minQuietTime As Double, thresholdLevel As Double
   Private iqPowerEdgeEnabled As Boolean, enableTrigger As Boolean
   Private frequencySource As String

   Private averageMeanPower As Double, peakToAverageRatio As Double, maximumPower As Double, minimumPower As Double

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

      measurementInterval = 0.001
      ' seconds 
      rbw = 100000.0
      ' Hz 
      timeout = 10
      ' seconds 

      averagingEnabled = RFmxSpecAnMXTxpAveragingEnabled.[False]
      averagingType = RFmxSpecAnMXTxpAveragingType.Rms
      averagingCount = 10

      frequencySource = RFmxInstrMXConstants.OnboardClock
      frequency = 10000000.0
      ' Hz 

      rbwFilterType = RFmxSpecAnMXTxpRbwFilterType.Gaussian
      rrcAlpha = 0.01

      vbwAuto = RFmxSpecAnMXTxpVbwFilterAutoBandwidth.[True]
      vbw = 30000.0
      ' Hz 
      vbwToRbwRatio = 3

      thresholdEnabled = RFmxSpecAnMXTxpThresholdEnabled.[False]
      thresholdType = RFmxSpecAnMXTxpThresholdType.Relative
      thresholdLevel = -20.0
      ' (dBm or dBm / Hz) 

      iqPowerEdgeEnabled = False
      iqPowerEdgeLevel = -20.0
      ' dBm 
      triggerDelay = 0.0
      ' seconds 
      minQuietTime = 0.0
      ' seconds 
      enableTrigger = True
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
      If iqPowerEdgeEnabled Then
         specAn.ConfigureIQPowerEdgeTrigger("", "0", iqPowerEdgeLevel, RFmxSpecAnMXIQPowerEdgeTriggerSlope.Rising,
            triggerDelay, RFmxSpecAnMXTriggerMinimumQuietTimeMode.Manual, minQuietTime, enableTrigger)
      Else
         specAn.DisableTrigger("")
      End If
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

      Dim power As AnalogWaveform(Of Single) = Nothing
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
