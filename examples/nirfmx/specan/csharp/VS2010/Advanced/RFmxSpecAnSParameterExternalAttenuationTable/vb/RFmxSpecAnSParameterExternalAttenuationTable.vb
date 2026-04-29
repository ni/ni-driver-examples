'Steps:
'1. Open a new RFmx session.
'2. Configure the basic instrument properties (Clock Source and Clock Frequency).
'3. Configure S - parameter External Attenuation Table.
'4. Configure External Attenuation Interpolation.
'5. Configure S - parameter External Attenuation Type.
'6. Configure Selected Ports.
'7. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
'8. Configure IQ Power Edge Trigger properties (Trigger Delay, IQ Power Edge Level, Min Quiet Time).
'9. Configure TXP measurement and enable the traces.
'10. Configure the Measurement Interval.
'11. Configure RBW filter parameters.
'12. Configure Thresholding.
'13. Configure Averaging parameters.
'14. Configure VBW filter parameters.
'15. Initiate Measurement.
'16. Fetch TXP Traces and Measurements.
'17. Close the RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.SpecAnMX

Namespace NationalInstruments.Examples.RFmxSpecAnSParameterExternalAttenuationTable
   Public Class RFmxSpecAnSParameterExternalAttenuationTable
      Private instrSession As RFmxInstrMX
      Private specAn As RFmxSpecAnMX

      Private resourceName As String
      Private centerFrequency As Double
      Private referenceLevel As Double
      Private externalAttenuation As Double

      Private selectedPorts As String
      Private portString As String

      Private frequencySource As String
      Private frequency As Double

      Private triggerDelay As Double
      Private iqPowerEdgeLevel As Double
      Private minimumQuietTime As Double
      Private enableTrigger As Boolean

      Const frequencyArraySize As Integer = 3
      Private tableName As String
      Private frequencyArray As Double() = New Double(frequencyArraySize - 1) {997000000.0, 1000000000.0, 1003000000.0}
      ' Hz
      Private sParameters As ComplexDouble(,,) = New ComplexDouble(2, 1, 1) {{{New ComplexDouble(1.0, 0.0), New ComplexDouble(1.0, 0.1)}, {New ComplexDouble(1.0, 0.1), New ComplexDouble(1.0, 0.0)}}, {{New ComplexDouble(0.8, 0.1), New ComplexDouble(0.8, 0.25)}, {New ComplexDouble(0.8, 0.25), New ComplexDouble(0.8, 0.1)}}, {{New ComplexDouble(1.0, 0.25), New ComplexDouble(1.0, 0.5)}, {New ComplexDouble(1.0, 0.5), New ComplexDouble(1.0, 0.25)}}}
      Private sParameterOrientation As RFmxInstrMXSParameterOrientation
      Private format As RFmxInstrMXLinearInterpolationFormat
      Private sParameterType As RFmxInstrMXSParameterType

      Private measurementInterval As Double

      Private rbwFilterType As RFmxSpecAnMXTxpRbwFilterType
      Private rbw As Double
      Private rrcAlpha As Double

      Private vbwAuto As RFmxSpecAnMXTxpVbwFilterAutoBandwidth
      Private vbw As Double
      Private vbwToRbwRatio As Double
      Private averagingEnabled As RFmxSpecAnMXTxpAveragingEnabled
      Private averagingCount As Integer
      Private averagingType As RFmxSpecAnMXTxpAveragingType

      Private thresholdEnabled As RFmxSpecAnMXTxpThresholdEnabled
      Private thresholdType As RFmxSpecAnMXTxpThresholdType
      Private thresholdLevel As Double

      Private timeout As Double
      Private averageMeanPower As Double
      Private peakToAverageRatio As Double
      Private maximumPower As Double
      Private minimumPower As Double

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

         iqPowerEdgeLevel = -20.0
         ' dBm
         triggerDelay = 0.0
         ' seconds
         minimumQuietTime = 0.0
         ' seconds
         enableTrigger = False

         tableName = ""
         format = RFmxInstrMXLinearInterpolationFormat.RealAndImaginary
         sParameterOrientation = RFmxInstrMXSParameterOrientation.Port1TowardsDut
         sParameterType = RFmxInstrMXSParameterType.Scalar

         measurementInterval = 0.001
         ' seconds

         rbwFilterType = RFmxSpecAnMXTxpRbwFilterType.Gaussian
         rbw = 100000.0
         ' Hz
         rrcAlpha = 0.01

         vbwAuto = RFmxSpecAnMXTxpVbwFilterAutoBandwidth.[True]
         vbw = 30000.0
         ' Hz
         vbwToRbwRatio = 3

         averagingEnabled = RFmxSpecAnMXTxpAveragingEnabled.[False]
         averagingCount = 10
         averagingType = RFmxSpecAnMXTxpAveragingType.Rms

         thresholdEnabled = RFmxSpecAnMXTxpThresholdEnabled.[False]
         thresholdType = RFmxSpecAnMXTxpThresholdType.Relative
         thresholdLevel = -20.0
         ' (dB or dBm)

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
         portString = RFmxInstrMX.BuildPortString2("", selectedPorts, "", 0)
         instrSession.ConfigureSParameterExternalAttenuationTable(portString, tableName, frequencyArray, sParameters, sParameterOrientation)
         instrSession.ConfigureExternalAttenuationInterpolationLinear(portString, tableName, format)
         instrSession.ConfigureSParameterExternalAttenuationType(portString, sParameterType)
         specAn.SetSelectedPorts("", selectedPorts)
         specAn.SetSelectedPorts("", selectedPorts)
         specAn.ConfigureFrequency("", centerFrequency)
         specAn.ConfigureReferenceLevel("", referenceLevel)
         specAn.ConfigureExternalAttenuation("", externalAttenuation)
         specAn.ConfigureIQPowerEdgeTrigger("", "0", iqPowerEdgeLevel, RFmxSpecAnMXIQPowerEdgeTriggerSlope.Rising, triggerDelay, RFmxSpecAnMXTriggerMinimumQuietTimeMode.Manual,
             minimumQuietTime, enableTrigger)
         specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Txp, True)
         specAn.Txp.Configuration.ConfigureMeasurementInterval("", measurementInterval)
         specAn.Txp.Configuration.ConfigureRbwFilter("", rbw, rbwFilterType, rrcAlpha)
         specAn.Txp.Configuration.ConfigureThreshold("", thresholdEnabled, thresholdLevel, thresholdType)
         specAn.Txp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)
         specAn.Txp.Configuration.ConfigureVbwFilter("", vbwAuto, vbw, vbwToRbwRatio)
         specAn.Initiate("", "")
      End Sub

      Private Sub RetrieveResults()
         ' Retrieve results


         Dim power As AnalogWaveform(Of Single) = Nothing
         specAn.Txp.Results.FetchPowerTrace("", timeout, power)
         specAn.Txp.Results.FetchMeasurement("", timeout, averageMeanPower, peakToAverageRatio, maximumPower, minimumPower)
      End Sub

      Private Sub PrintResults()
         Console.WriteLine("---------------Measurement---------------")
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
End Namespace
