'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation)
'4. Configure Trigger Type and Trigger Parameters.
'5. Configure UARFCN Band.
'6. Configure Contiguous Carriers.
'7. Select SEM measurement and enable Traces.
'8. Configure Sweep Time Parameters.
'9. Configure Averaging Parameters for SEM measurement.
'10. Initiate the Measurement.
'11. Fetch SEM Measurements and Traces.
'12. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.WcdmaMX

Public Class RFmxWcdmaSemMultiCarrier
   Private instrSession As RFmxInstrMX
   Private wcdma As RFmxWcdmaMX

   Private resourceName As String = "RFSA"
   Private measurement As RFmxWcdmaMXMeasurementTypes = RFmxWcdmaMXMeasurementTypes.Sem
   Private frequencyReferenceSource As String = RFmxInstrMXConstants.OnboardClock
   Private frequencyReferenceFrequency As Double = 10000000.0
   ' Hz
   Private centerFrequency As Double = 1950000000.0
   ' Hz
   Private externalAttenuation As Double = 0.0
   ' dB

   Private digitalEdgeSource As String = RFmxWcdmaMXConstants.Pfi0
   Private digitalEdge As RFmxWcdmaMXDigitalEdgeTriggerEdge = RFmxWcdmaMXDigitalEdgeTriggerEdge.Rising
   Private triggerDelay As Double = 0.0
   ' seconds
   Private referenceLevel As Double = 0.0
   ' dBm
   Private averagingEnabled As RFmxWcdmaMXSemAveragingEnabled = RFmxWcdmaMXSemAveragingEnabled.[False]
   Private averagingCount As Integer = 10
   Private averagingType As RFmxWcdmaMXSemAveragingType = RFmxWcdmaMXSemAveragingType.Rms
   Private sweepTimeAuto As RFmxWcdmaMXSemSweepTimeAuto = RFmxWcdmaMXSemSweepTimeAuto.[True]
   Private sweepTimeInterval As Double = 0.000000066667
   ' seconds
   Private timeout As Double = 10
   ' seconds
   Private totalCarrierPower As Double = 0.0
   ' dBm
   Private spectrum As Spectrum(Of Single)

   Private enableAllTraces As Boolean = True
   Private enableTrigger As Boolean = False

   Private numberOfCarriers As Integer = 2
   Private carrierAtCenterFrequency As Integer = -1
   Private absoluteMask As Spectrum(Of Single)
   Private relativeMask As Spectrum(Of Single)
   Private band As Integer = 1

   Private lowerOffsetMeasurementStatus As RFmxWcdmaMXSemLowerOffsetMeasurementStatus()
   Private lowerOffsetMargin As Double()
   Private lowerOffsetMarginFrequency As Double()
   Private lowerOffsetMarginAbsolutePower As Double()
   Private lowerOffsetMarginRelativePower As Double()

   Private upperOffsetMeasurementStatus As RFmxWcdmaMXSemUpperOffsetMeasurementStatus()
   Private upperOffsetMargin As Double()
   Private upperOffsetMarginFrequency As Double()
   Private upperOffsetMarginAbsolutePower As Double()
   Private upperOffsetMarginRelativePower As Double()
   Private measurementStatus As RFmxWcdmaMXSemMeasurementStatus
   Private absoluteIntegratedPower As Double()
   Private relativeIntegratedPower As Double()

   Public Sub Run()
      Try
         InitializeInstr()
         ConfigureWcdma()
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

   Private Sub InitializeInstr()
      ' Create a new RFmx Session

      instrSession = New RFmxInstrMX(resourceName, "")
   End Sub

   Private Sub ConfigureWcdma()
      wcdma = instrSession.GetWcdmaSignalConfiguration()
      instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
      wcdma.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)

      wcdma.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger)
      wcdma.ConfigureBand("", band)
      wcdma.ConfigureContiguousCarriers("", numberOfCarriers, carrierAtCenterFrequency)
      wcdma.SelectMeasurements("", measurement, enableAllTraces)
      wcdma.Sem.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)
      wcdma.Sem.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval)
   End Sub

   Private Sub RetrieveResults()
      wcdma.Initiate("", "")
      wcdma.Sem.Results.FetchLowerOffsetMarginArray("", timeout, lowerOffsetMeasurementStatus, lowerOffsetMargin, lowerOffsetMarginFrequency, lowerOffsetMarginAbsolutePower,
         lowerOffsetMarginRelativePower)
      wcdma.Sem.Results.FetchUpperOffsetMarginArray("", timeout, upperOffsetMeasurementStatus, upperOffsetMargin, upperOffsetMarginFrequency, upperOffsetMarginAbsolutePower,
         upperOffsetMarginRelativePower)
      wcdma.Sem.Results.FetchMeasurementStatus("", timeout, measurementStatus)
      wcdma.Sem.Results.FetchCarrierMeasurementArray("", timeout, absoluteIntegratedPower, relativeIntegratedPower)
      wcdma.Sem.Results.FetchSpectrum("", timeout, spectrum, relativeMask, absoluteMask)
      wcdma.Sem.Results.FetchTotalCarrierPower("", timeout, totalCarrierPower)
   End Sub

   Private Sub PrintResults()
      Console.WriteLine("Measurement Status                  : {0}", measurementStatus)
      Console.WriteLine("Total Carrier Power                 : {0}", totalCarrierPower)
      Console.WriteLine(vbLf & "Carrier Measurements                :" & vbLf)
      For i As Integer = 0 To absoluteIntegratedPower.Length - 1
         Console.WriteLine(vbLf & "Carrier                             : {0}", i)
         Console.WriteLine("Absolute Integrated Power  (dBm)    : {0}", absoluteIntegratedPower(i))
         Console.WriteLine("Relative Integrated Power (dB)     : {0}", relativeIntegratedPower(i))
      Next
      Console.WriteLine(vbLf & "Lower Offset Segment Measurements   :" & vbLf)
      For i As Integer = 0 To lowerOffsetMargin.Length - 1
         Console.WriteLine(vbLf & "Measurement                         : {0}", i)
         Console.WriteLine("Margin (dB)                         : {0}", lowerOffsetMargin(i))
         Console.WriteLine("Margin Absolute Power (dBm)         : {0}", lowerOffsetMarginAbsolutePower(i))
         Console.WriteLine("Margin Relative Power (dB)         : {0}", lowerOffsetMarginRelativePower(i))
         Console.WriteLine("Margin Frequency (Hz)               : {0}", lowerOffsetMarginFrequency(i))
         Console.WriteLine("Measurement Status                  : {0}", lowerOffsetMeasurementStatus(i))
      Next

      Console.WriteLine(vbLf & "Upper Offset Segment Measurements   :" & vbLf)
      For i As Integer = 0 To upperOffsetMargin.Length - 1
         Console.WriteLine(vbLf & "Measurement                         : {0}", i)
         Console.WriteLine("Margin (dB)                         : {0}", upperOffsetMargin(i))
         Console.WriteLine("Margin Absolute Power (dBm)         : {0}", upperOffsetMarginAbsolutePower(i))
         Console.WriteLine("Margin Relative Power (dB)         : {0}", upperOffsetMarginRelativePower(i))
         Console.WriteLine("Margin Frequency (Hz)               : {0}", upperOffsetMarginFrequency(i))
         Console.WriteLine("Measurement Status                  : {0}", upperOffsetMeasurementStatus(i))
      Next
   End Sub

   Private Sub CloseSession()
      If wcdma IsNot Nothing Then
         wcdma.Dispose()
         wcdma = Nothing
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
