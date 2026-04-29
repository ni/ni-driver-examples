'Steps:
'1. Open a new RFmx session
'2.  Configure the basic instrument properties (Clock Source and Clock Frequency)
'3. Configure Selected Ports
'4. Configure the basic signal properties  (Reference Level and External Attenuation)
'6. Configure TXP measurement and enable the traces
'7. Configure TXP Measurement Interval
'8. Configure TXP RBW Filter
'9. Configure TXP Averaging
'10. Create a Queue for producer-consumer setup
'11. Producer loop - Configure frequency step, Initiate Measurement, enqueue result name and wait for the acquisiton to be complete
'12. Consumer loop - Dequeue result name and Fetch TXP reults
'13. Release Queue
'14. Close the RFmx Session

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.SpecAnMX
Imports System.Collections.Concurrent
Imports System.Threading

Public Class RFmxSpecAnTxpMultipleResultNames

   Const numberOfMeasurements As Integer = 2
   Const timeout As Double = 10.0
   ' seconds 
   Private Structure MeasurementResults
      Public averageMeanPower As Double(), peakToAverageRatio As Double(), maximumPower As Double(), minimumPower As Double()
      Public Sub New(numberOfMeasurements As Integer)
         averageMeanPower = New Double(numberOfMeasurements - 1) {}
         peakToAverageRatio = New Double(numberOfMeasurements - 1) {}
         maximumPower = New Double(numberOfMeasurements - 1) {}
         minimumPower = New Double(numberOfMeasurements - 1) {}
      End Sub
   End Structure

   Private frequencySource As String
   Private instrSession As RFmxInstrMX
   Private specAn As RFmxSpecAnMX
   Private referenceLevel As Double, externalAttenuation As Double
   Private measurementInterval As Double, rbw As Double, rrcAlpha As Double, frequency As Double, centerFrequency As Double
   Private resourceName As String
   Private selectedPorts As String
   Private averagingEnabled As RFmxSpecAnMXTxpAveragingEnabled
   Private averagingCount As Integer
   Private averagingType As RFmxSpecAnMXTxpAveragingType
   Private rbwFilterType As RFmxSpecAnMXTxpRbwFilterType
   Private errorFlag As Boolean

   Private TxpMeasurements As New MeasurementResults(numberOfMeasurements)

   Private taskQueue As New BlockingCollection(Of String)(New ConcurrentQueue(Of String)())


   Public Sub Run()
      Try
         InitializeVariables()
         InitializeInstr()
         ConfigureSpecAn()
         BeginProducerConsumerLoop()
         PrintResults()

         CloseSession()
      Catch ex As Exception
         DisplayError(ex.Message)
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
      referenceLevel = 0.0
      ' dBm 
      externalAttenuation = 0.0
      ' dB 
      selectedPorts = ""
      centerFrequency = 1000000000.0
      ' Hz 

      measurementInterval = 0.001
      ' seconds 
      rbw = 100000.0
      '  Hz 

      averagingEnabled = RFmxSpecAnMXTxpAveragingEnabled.[False]
      averagingType = RFmxSpecAnMXTxpAveragingType.Rms
      averagingCount = 10

      frequencySource = RFmxInstrMXConstants.OnboardClock
      frequency = 10000000.0

      rbwFilterType = RFmxSpecAnMXTxpRbwFilterType.Gaussian
      rrcAlpha = 0.01

      errorFlag = False
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
      specAn.ConfigureReferenceLevel("", referenceLevel)
      specAn.ConfigureExternalAttenuation("", externalAttenuation)

      specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Txp, True)
      specAn.Txp.Configuration.ConfigureMeasurementInterval("", measurementInterval)
      specAn.Txp.Configuration.ConfigureRbwFilter("", rbw, rbwFilterType, rrcAlpha)
      specAn.Txp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)
   End Sub

   Private Sub BeginProducerConsumerLoop()
      Dim FetchResultsThread As New Thread(New ThreadStart(AddressOf FetchResults))
      FetchResultsThread.Start() 'Start a new thread for Consumer loop
      ConfigureAndInitiate()     'Start Producer loop
      FetchResultsThread.Join()
      'Producer and Consumer have finished at this point.
   End Sub

   Public Sub ConfigureAndInitiate()
      Try
         For i As Integer = 0 To numberOfMeasurements - 1
            Dim offset As Double = i * 1000000
            specAn.ConfigureFrequency("", centerFrequency + offset)
            Dim resultString As String = RFmxSpecAnMX.BuildResultString("TXP_Result" & i)
            specAn.Initiate("", resultString)
            taskQueue.Add(resultString)
            instrSession.WaitForAcquisitionComplete(timeout)
         Next
      Catch ex As Exception
         errorFlag = True
         Console.WriteLine("ERROR in ConfigureAndInitiate thread:" & ex.Message & vbLf & vbLf)
      End Try
   End Sub

   Public Sub FetchResults()
      Try
         Dim queueTimeout As Integer = 10000 'milliseconds
         For i As Integer = 0 To numberOfMeasurements - 1
            Dim resultString As String = ""
            If taskQueue.TryTake(resultString, queueTimeout) Then
               specAn.Txp.Results.FetchMeasurement(resultString, timeout, TxpMeasurements.averageMeanPower(i),
                                                   TxpMeasurements.peakToAverageRatio(i), TxpMeasurements.maximumPower(i),
                                                   TxpMeasurements.minimumPower(i))
            Else
               Throw New Exception("Could not fetch the result string as dequeue operation timed out")
            End If
         Next
      Catch ex As Exception
         errorFlag = True
         Console.WriteLine("ERROR  in FetchResults thread:" & ex.Message & vbLf & vbLf)
      End Try
   End Sub

   Private Sub PrintResults()
      If Not errorFlag Then
         For i As Integer = 0 To numberOfMeasurements - 1
            Console.WriteLine("-------------------Measurement {0}---------------------", i + 1)
            Console.WriteLine("Average Mean Frequency (Hz)    {0}", TxpMeasurements.averageMeanPower(i))
            Console.WriteLine("Peak to Average Ratio (dB)     {0}", TxpMeasurements.peakToAverageRatio(i))
            Console.WriteLine("Maximum Power (dBm)            {0}", TxpMeasurements.maximumPower(i))
            Console.WriteLine("Minimum Power (dBm)            {0}", TxpMeasurements.minimumPower(i))
            Console.WriteLine("----------------------------------------------------")
         Next
      End If
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
         DisplayError(ex.Message)
      End Try
   End Sub

   Private Shared Sub DisplayError(message As String)
      Console.WriteLine("ERROR:" & vbLf & message)
   End Sub

End Class
