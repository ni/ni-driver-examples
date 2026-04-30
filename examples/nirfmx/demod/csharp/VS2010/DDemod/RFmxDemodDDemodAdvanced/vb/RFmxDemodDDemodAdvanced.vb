'Steps:
'1. Open a new RFmxInstrMX session and create a Demod Signal
'2. Configure the basic instrument properties (Clock Source, Clock Frequency)
'3. Configure Selected Ports
'4. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
'5. Configure IQ Power Edge Trigger properties (Trigger Delay, IQ Power Edge Level, Min Quiet Time)
'6. Select DDemod Measurement and enable  the traces 
'7. Configure Modulation Type
'8. Configure DDemod Symbol Rate, Sample Per Symbol, Number of Symbols
'9. Configure DDemod PSK Format and EVM Norm Reference
'10. Configure DDemod FSK Deviation


Imports NationalInstruments
Imports NationalInstruments.RFmx.DemodMX
Imports NationalInstruments.RFmx.InstrMX

Public Class RFmxDemodDDemodAdvanced
   Private instrSession As RFmxInstrMX
   Private demod As RFmxDemodMX

   Private meanCarrierFrequencyError As Double, meanFrequencyDrift As Double, meanCarrierPhaseError As Double
   Private meanRmsEvm As Double, meanPeakEvm As Double, meanRmsOffsetEvm As Double, meanPeakOffsetEvm As Double,
       meanMer As Double, maxRmsEvm As Double, maxPeakEvm As Double, maxRmsOffsetEvm As Double, maxPeakOffsetEvm As Double
   Private meanFskDeviation As Double, meanRmsFskError As Double, maxPeakFskError As Double, timeout As Double = 10.0

   Private syncFound As Boolean
   Private meanMagnitudeError As Double, maxMagnitudeError As Double, meanPhaseError As Double,
       maxPhaseError As Double, meanIQOriginOffset As Double, meanIQGainImbalance As Double,
       meanQuadratureSkew As Double, meanRhoFactor As Double, meanAmplitudeDroop As Double


   Private Sub CreateRFmxSession()
        Dim resourceName As String = "RFSA"
        If instrSession Is Nothing Then
         instrSession = New RFmxInstrMX(resourceName, "")
         demod = instrSession.GetDemodSignalConfiguration()
      End If
   End Sub

   Private Sub ConfigureDemodSignal()
      Dim pluseShappingFilterCustomCoefficientY As Single() = Nothing
      Dim measurementFilterCustomCoefficientY As Single() = Nothing
      Dim equalizerInitialCoefficientY As ComplexSingle() = Nothing
      Dim syncBits As SByte() = Nothing

      Dim selectedPorts As String = ""
      Dim centerFrequency As Double = 1000000000.0
      Dim referenceLevel As Double = 0.0
      Dim externalAttenuation As Double = 0.0

      Dim frequency As Double = 10000000.0
      Dim frequencySource As String = RFmxInstrMXConstants.OnboardClock

      Dim iqPowerEdgeEnabled As Boolean = False
      Dim iqPowerEdgeLevel As Double = -20.0
      Dim triggerDelay As Double = 0.0
      Dim minQuietTime As Double = 0.0

      Dim samplesPerSymbol As Integer = -1
      Dim fskDeviation As Double = 15000.0
      Dim apskR2toR1Ratio As Double = 2.84
      Dim apskR3toR1Ratio As Double = 5.27
      Dim symbolRate As Double = 100000.0
      Dim numOfSymbols As Integer = 1000

      Dim measurementOffset As Integer = 0

      'Averaging 
      Dim averagingCount As Integer = 10

      ' Signal Structure

      Dim signalStructure As RFmxDemodMXDDemodSignalStructure = RFmxDemodMXDDemodSignalStructure.Continuous

      ' Burst Start Exclusion Symbols 

      Dim burstStartExclusionSymbols As Integer = 0
      ' Burst End Exclusion Symbols 

      Dim burstEndExclusionSymbols As Integer = 0

      Dim pulseShappingFilterAlphaOrBT As Double = 0.5
      Dim pluseShappingFilterCustomCoefficientX0 As Double = 0
      Dim pluseShappingFilterCustomCoefficientDx As Double = 1.0

      Dim measurementFilterCustomCoefficientX0 As Double = 0.0
      Dim measurementFilterCustomCoefficientDx As Double = 1.0

      Dim equalizerLength As Integer = 20
      Dim equalizerTrainingCount As Integer = 10
      Dim equalizerConvergenceFactor As Double = 0.01

      instrSession.ConfigureFrequencyReference("", frequencySource, frequency)
      demod.SetSelectedPorts("", selectedPorts)
      demod.ConfigureFrequency("", centerFrequency)
      demod.ConfigureReferenceLevel("", referenceLevel)
      demod.ConfigureExternalAttenuation("", externalAttenuation)

      demod.ConfigureIQPowerEdgeTrigger("", "0", iqPowerEdgeLevel, RFmxDemodMXIQPowerEdgeTriggerSlope.Rising,
                                        triggerDelay, RFmxDemodMXTriggerMinimumQuietTimeMode.Manual,
                                        minQuietTime, iqPowerEdgeEnabled)

      demod.SelectMeasurements("", RFmxDemodMXMeasurementTypes.DDemod, True)
        demod.DDemod.Configuration.ConfigureModulationType("", RFmxDemodMXDDemodModulationType.Psk,
                                      RFmxDemodMXDDemodM.M4, RFmxDemodMXDDemodDifferentialEnabled.[False])
        demod.DDemod.Configuration.ConfigureSymbolRate("", symbolRate)
      demod.DDemod.Configuration.ConfigureSamplesPerSymbol("", samplesPerSymbol)
      demod.DDemod.Configuration.ConfigureNumberOfSymbols("", numOfSymbols)

      demod.DDemod.Configuration.ConfigurePskFormat("", RFmxDemodMXDDemodPskFormat.Normal)
      demod.DDemod.Configuration.ConfigureEvmNormalizationReference("", RFmxDemodMXDDemodEvmNormalizationReference.Peak)
      demod.DDemod.Configuration.ConfigureFskDeviation("", fskDeviation, RFmxDemodMXDDemodFskReferenceCompensationEnabled.[False])
      demod.DDemod.Configuration.SetApskR2ToR1Ratio("", apskR2toR1Ratio)
      demod.DDemod.Configuration.SetApskR3ToR1Ratio("", apskR3toR1Ratio)
        demod.DDemod.Configuration.ConfigurePulseShapingFilter("", RFmxDemodMXDDemodPulseShapingFilterType.RootRaisedCosine,
              pulseShappingFilterAlphaOrBT, pluseShappingFilterCustomCoefficientX0,
              pluseShappingFilterCustomCoefficientDx, pluseShappingFilterCustomCoefficientY)
      demod.DDemod.Configuration.ConfigureMeasurementFilter("", RFmxDemodMXDDemodMeasurementFilterType.Auto,
          measurementFilterCustomCoefficientX0, measurementFilterCustomCoefficientDx, measurementFilterCustomCoefficientY)
      demod.DDemod.Configuration.ConfigureEqualizer("", RFmxDemodMXDDemodEqualizerMode.Off, equalizerLength, 0.0, 1.0, equalizerInitialCoefficientY,
       equalizerTrainingCount, equalizerConvergenceFactor)
      demod.DDemod.Configuration.ConfigureSynchronization("", RFmxDemodMXDDemodSynchronizationEnabled.[False], syncBits, measurementOffset)
      demod.DDemod.Configuration.ConfigureAveraging("", RFmxDemodMXDDemodAveragingEnabled.[False], averagingCount)
      demod.DDemod.Configuration.ConfigureSignalStructure("", signalStructure)
      demod.DDemod.Configuration.SetBurstStartExclusionSymbols("", burstStartExclusionSymbols)
      demod.DDemod.Configuration.SetBurstEndExclusionSymbols("", burstEndExclusionSymbols)
      demod.Initiate("", "")
   End Sub

   Private Sub RetrieveResults()
      Dim constellationTrace As ComplexSingle() = Nothing
      Dim evmTrace As AnalogWaveform(Of Single) = Nothing
      Dim offsetEvmTrace As AnalogWaveform(Of Single) = Nothing

      demod.DDemod.Results.FetchCarrierMeasurement("", timeout, meanCarrierFrequencyError, meanFrequencyDrift, meanCarrierPhaseError)
      demod.DDemod.Results.FetchEvm("", timeout, meanRmsEvm, maxRmsEvm, meanMer, maxPeakEvm,
       meanPeakEvm)
      demod.DDemod.Results.FetchOffsetEvm("", timeout, meanRmsOffsetEvm, maxRmsOffsetEvm, maxPeakOffsetEvm, meanPeakOffsetEvm)
      demod.DDemod.Results.FetchMagnitudeError("", timeout, meanMagnitudeError, maxMagnitudeError)
      demod.DDemod.Results.FetchPhaseError("", timeout, meanPhaseError, maxPhaseError)
      demod.DDemod.Results.FetchFskResults("", timeout, meanFskDeviation, meanRmsFskError, maxPeakFskError)
      demod.DDemod.Results.FetchIQImpairments("", timeout, meanIQGainImbalance, meanQuadratureSkew, meanIQOriginOffset)
      demod.DDemod.Results.FetchSyncFound("", timeout, syncFound)
      demod.DDemod.Results.FetchMeanRhoFactor("", timeout, meanRhoFactor)
      demod.DDemod.Results.FetchMeanAmplitudeDroop("", timeout, meanAmplitudeDroop)
      demod.DDemod.Results.FetchConstellationTrace("", timeout, constellationTrace)
      demod.DDemod.Results.FetchEvmTrace("", timeout, evmTrace)
      demod.DDemod.Results.FetchOffsetEvmTrace("", timeout, offsetEvmTrace)


      Console.WriteLine("-------------------Carrier measurements----------" & vbLf)
      Console.WriteLine("Mean Carrier Frequency Error(Hz)   " & meanCarrierFrequencyError)
      Console.WriteLine("Mean Frequency Drift (Hz)          " & meanFrequencyDrift)
      Console.WriteLine("Mean Phase Error (deg)            " & meanCarrierPhaseError)

      Console.WriteLine(vbLf & "---------------------------EVM-----------------" & vbLf)
      Console.WriteLine("Mean MER (dB)                       " & meanMer)
      Console.WriteLine("Mean RMS EVM (%)                    " & meanRmsEvm)
      Console.WriteLine("Maximum RMS EVM (%)                 " & maxRmsEvm)
      Console.WriteLine("Mean Peak EVM (%)                   " & meanPeakEvm)
      Console.WriteLine("Maximum Peak EVM (%)                " & maxPeakEvm)
      Console.WriteLine("Mean RMS Offset EVM (%)             " & meanRmsOffsetEvm)
      Console.WriteLine("Maximum RMS Offset EVM (%)          " & maxRmsOffsetEvm)
      Console.WriteLine("Mean Peak Offset EVM (%)            " & meanPeakOffsetEvm)
      Console.WriteLine("Maximum Peak Offset EVM (%)         " & maxPeakOffsetEvm)

      Console.WriteLine(vbLf & "--------------------------FSK Results--------------" & vbLf)
      Console.WriteLine("Mean Deviation (Hz)                 " & meanFskDeviation)
      Console.WriteLine("Mean RMS FSK Error (Hz)             " & meanRmsFskError)
      Console.WriteLine("Maximum Peak FSK Error (%)          " & maxPeakFskError)

      Console.WriteLine(vbLf & "--------------------------Measurements------------" & vbLf)
      If syncFound Then
         Console.WriteLine("Sync Found is True" & vbLf)
      Else
         Console.WriteLine("Sync Found is False" & vbLf)
      End If

      Console.WriteLine("Mean Magnitude Error (%)            " & meanMagnitudeError)
      Console.WriteLine("Maximum Magnitude Error (%)         " & maxMagnitudeError)
      Console.WriteLine("Mean Phase Error (deg)              " & meanPhaseError)
      Console.WriteLine("Maximum Phase Error (deg)           " & maxPhaseError)
      Console.WriteLine("Mean IQ Origin Offset (dB)          " & meanIQOriginOffset)
      Console.WriteLine("Mean IQ Gain Imbalance (dB)         " & meanIQGainImbalance)
      Console.WriteLine("Mean Quadrature Skew (deg)          " & meanQuadratureSkew)
      Console.WriteLine("Mean Rho Factor                     " & meanRhoFactor)
      Console.WriteLine("Mean Amplitude Droop (dB/Symbol)    " & meanAmplitudeDroop)

   End Sub

   Private Sub CloseSession()
      If demod IsNot Nothing Then
         demod.Dispose()
         demod = Nothing
      End If
      If instrSession IsNot Nothing Then
         instrSession.Close()
         instrSession = Nothing
      End If
   End Sub

   Public Sub Run()
      Try
         CreateRFmxSession()
         ConfigureDemodSignal()
         RetrieveResults()
      Catch e As Exception
         Console.WriteLine(e.Message)
      Finally
         CloseSession()
         Console.WriteLine("Press any key to exit")
         Console.ReadKey()
      End Try
   End Sub


End Class
