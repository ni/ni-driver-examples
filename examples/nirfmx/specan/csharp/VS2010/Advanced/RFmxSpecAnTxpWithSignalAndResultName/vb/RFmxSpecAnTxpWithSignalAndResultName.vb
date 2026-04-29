'Steps:
'1. Open a new RFmx session
'2. Create a Named signal
'3. Configure the basic instrument properties (Clock Source and Clock Frequency)
'4. Configure Selected Ports
'5. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation)
'6. Configure TXP measurement and enable the traces
'7. Configure the Measurement Interval
'8. Configure RBW filter parameters
'9. Configure Averaging parameters
'10. Initiate Measurement
'11. Fetch TXP Traces and Measurements
'12. Close the RFmx Session


Imports NationalInstruments
Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.SpecAnMX

Namespace NationalInstruments.Examples.RFmxSpecAnTxpWithSignalAndResultName
   Public Class RFmxSpecAnTxpWithSignalAndResultName
      Private instrSession As RFmxInstrMX
      Private specAn As RFmxSpecAnMX
      Private resourceName As String

      Const NumberOfOffsets As Integer = 2

      Private selectedPorts As String
      Private centerFrequency As Double, referenceLevel As Double, externalAttenuation As Double, rbw As Double, frequency As Double, measurementInterval As Double,
         timeout As Double, rrcAlpha As Double
      Private frequencySource As String

      Private averagingCount As Integer
      Private averagingEnabled As RFmxSpecAnMXTxpAveragingEnabled
      Private averagingType As RFmxSpecAnMXTxpAveragingType

      Private RBWFilterType As RFmxSpecAnMXTxpRbwFilterType

      'Output values
      Private averageMeanPower As Double
      Private peakToAverageRatio As Double
      Private maxPower As Double
      Private minPower As Double
      Private resultName As String

      Friend Sub Run()
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

         measurementInterval = 0.001
         ' seconds 

         ' RBW Filter
         RBWFilterType = RFmxSpecAnMXTxpRbwFilterType.Gaussian
         rbw = 100000.0
         ' Hz 
         rrcAlpha = 0.01

         'Averaging 
         averagingEnabled = RFmxSpecAnMXTxpAveragingEnabled.[False]
         averagingCount = 10
         averagingType = RFmxSpecAnMXTxpAveragingType.Rms

         timeout = 10.0
         ' seconds 
      End Sub

      Private Sub InitializeInstr()
         ' Create a new RFmx Session 

         instrSession = New RFmxInstrMX(resourceName, "")
      End Sub

      Private Sub ConfigureSpecAn()
         ' Get SpecAn signal 

         specAn = instrSession.GetSpecAnSignalConfiguration("TxP_Signal")

         ' Configure measurement 

         instrSession.ConfigureFrequencyReference("", frequencySource, frequency)
         specAn.SetSelectedPorts("", selectedPorts)
         specAn.ConfigureExternalAttenuation("", externalAttenuation)
         specAn.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
         specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Txp, True)

         specAn.Txp.Configuration.ConfigureMeasurementInterval("", measurementInterval)
         specAn.Txp.Configuration.ConfigureRbwFilter("", rbw, RBWFilterType, rrcAlpha)
         specAn.Txp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)
         resultName = RFmxSpecAnMX.BuildResultString("TxP_Result")
         specAn.Initiate("", resultName)
      End Sub

      Private Sub RetrieveResults()
         ' Retrieve results 

         Dim power As AnalogWaveform(Of Single) = Nothing
         specAn.Txp.Results.FetchPowerTrace(resultName, timeout, power)
         specAn.Txp.Results.FetchMeasurement(resultName, timeout, averageMeanPower, peakToAverageRatio, maxPower, minPower)
      End Sub

      Private Sub PrintResults()
         Console.WriteLine("Average Mean Frequency (Hz)    {0}", averageMeanPower)
         Console.WriteLine("Mean Phase (deg)               {0}", peakToAverageRatio)
         Console.WriteLine("Maximum Power (dBm)            {0}", maxPower)
         Console.WriteLine("Minimum Power (dBm)            {0}", minPower)
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
