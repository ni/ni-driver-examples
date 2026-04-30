'Steps:
'1. Open a new RFmxInstrMX session and create a Demod Signal
'2. Configure the basic instrument properties (Clock Source, Clock Frequency)
'3. Configure Selected Ports
'4. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
'5. Select ADemod Measurement and enable  the traces 
'6. Configure FM Modulation
'7. Configure ADemod RBW Filter, Measurement Interval, and Carrier Correction
'8. Configure ADemod FM DeEmphasis, Audio Filter and Averaging
'9. Initiate Measurement

Imports NationalInstruments
Imports NationalInstruments.RFmx.DemodMX
Imports NationalInstruments.RFmx.InstrMX

Public Class RFmxDemodADemodFMAdvanced
   Private instrSession As RFmxInstrMX
   Private demod As RFmxDemodMX

   Private Sub CreateRFmxSession()
      Dim resourceName As String = "RFSA"
      If instrSession Is Nothing Then
         instrSession = New RFmxInstrMX(resourceName, "")
         demod = instrSession.GetDemodSignalConfiguration()
      End If
   End Sub

   Private Sub ConfigureDemodSignal()
      Dim selectedPorts As String
      Dim frequencySource As String
      Dim centerFrequency As Double, referenceLevel As Double, externalAttenuation As Double, frequency As Double
      Dim measurementInterval As Double, deemphasis As Double, rbw As Double, rbwRrcAlpha As Double
      Dim audioFilterLowerCutoff As Double, audioFilterUpperCutoff As Double
      Dim averagingCount As Integer

      selectedPorts = ""
      centerFrequency = 1000000000.0
      referenceLevel = 0.0
      externalAttenuation = 0.0

      frequencySource = RFmxInstrMXConstants.OnboardClock
      frequency = 10000000.0

      measurementInterval = 0.01
      deemphasis = 0.0

      rbw = 100000.0
      rbwRrcAlpha = 0.1

      audioFilterLowerCutoff = 100.0
      audioFilterUpperCutoff = 10000.0

      'Averaging 
      averagingCount = 10

      instrSession.ConfigureFrequencyReference("", frequencySource, frequency)
      demod.SetSelectedPorts("", selectedPorts)
      demod.ConfigureFrequency("", centerFrequency)
      demod.ConfigureReferenceLevel("", referenceLevel)
      demod.ConfigureExternalAttenuation("", externalAttenuation)
      demod.SelectMeasurements("", RFmxDemodMXMeasurementTypes.ADemod, True)
      demod.ADemod.Configuration.SetAudioMeasurementEnabled("", RFmxDemodMXADemodAudioMeasurementEnabled.True)
      demod.ADemod.Configuration.ConfigureModulationType("", RFmxDemodMXADemodModulationType.FM)
      demod.ADemod.Configuration.ConfigureRbwFilter("", rbw, RFmxDemodMXADemodRbwFilterType.Flat, rbwRrcAlpha)
      demod.ADemod.Configuration.ConfigureMeasurementInterval("", measurementInterval)
      demod.ADemod.Configuration.ConfigureCarrierCorrection("", RFmxDemodMXADemodCarrierFrequencyCorrectionEnabled.True,
          RFmxDemodMXADemodCarrierPhaseCorrectionEnabled.True)
      demod.ADemod.Configuration.ConfigureFMDeEmphasis("", deemphasis)
      demod.ADemod.Configuration.ConfigureAudioFilter("", RFmxDemodMXADemodAudioFilterType.None, audioFilterLowerCutoff,
          audioFilterUpperCutoff)
      demod.ADemod.Configuration.ConfigureAveraging("", RFmxDemodMXADemodAveragingEnabled.False, averagingCount,
          RFmxDemodMXADemodAveragingType.Linear)

      demod.Initiate("", "")
   End Sub

   Private Sub RetrieveResults()
      Dim meanCarrierPower As Double, averageSinad As Double, averageThdWithNoise As Double, meanCarrierFrequencyError As Double,
          averageSnr As Double, averageThd As Double, meanModulationFrequency As Double
      Dim meanDeviation As Double, meanHalfPeaktoPeak As Double, meanPositivePeak As Double, meanNegativePeak As Double,
          meanRms As Double
      Dim maxDeviation As Double, maxPeaktoPeak As Double, maxRms As Double, maxPositivePeak As Double,
          maxNegativePeak As Double
      Dim timeout As Double = 10.0
      Dim demodSpectrumTrace As Spectrum(Of Single) = Nothing
      Dim demodSignalTrace As AnalogWaveform(Of Single) = Nothing
      demod.ADemod.Results.FetchDistortions("", timeout, averageSinad, averageSnr, averageThd, averageThdWithNoise)
      demod.ADemod.Results.FetchMeanModulationFrequency("", timeout, meanModulationFrequency)
      demod.ADemod.Results.FetchCarrierMeasurement("", timeout, meanCarrierFrequencyError, meanCarrierPower)
      demod.ADemod.Results.FetchFMMeanDeviation("", timeout, meanDeviation, meanHalfPeaktoPeak, meanRms, meanPositivePeak,
       meanNegativePeak)
      demod.ADemod.Results.FetchFMMaximumDeviation("", timeout, maxDeviation, maxPeaktoPeak, maxRms, maxPositivePeak,
       maxNegativePeak)
      demod.ADemod.Results.FetchDemodSpectrumTrace("", timeout, demodSpectrumTrace)
      demod.ADemod.Results.FetchDemodSignalTrace("", timeout, demodSignalTrace)


      Console.WriteLine("------------------------------------------------------" & vbLf)
      Console.WriteLine("Mean Carrier Power (dBm)          " & meanCarrierPower)
      Console.WriteLine("Average SINAD (dB)                " & averageSinad)
      Console.WriteLine("Average THD with Noise (%)       " & averageThdWithNoise)
      Console.WriteLine("Mean Carrier Frequency Error(Hz)  " & meanCarrierFrequencyError)
      Console.WriteLine("Average SNR (dB)                  " & averageSnr)
      Console.WriteLine("Average THD (%)                  " & averageThd)
      Console.WriteLine("Mean Modulation Frequency (Hz)    " & meanModulationFrequency)


      Console.WriteLine(vbLf & "-------------------FM Deviations---------------------" & vbLf)
      Console.WriteLine("Mean Deviation (Hz)            " & meanDeviation)
      Console.WriteLine("Maximum Deviation (Hz)         " & maxDeviation)
      Console.WriteLine("Mean Peak to Peak/2 (Hz)       " & meanHalfPeaktoPeak)
      Console.WriteLine("Maximum Peak to Peak/2 (Hz)    " & maxPeaktoPeak)
      Console.WriteLine("Mean Positive Peak (Hz)        " & meanPositivePeak)
      Console.WriteLine("Maximum Positive Peak (Hz)     " & maxPositivePeak)
      Console.WriteLine("Mean Negative peak (Hz)        " & meanNegativePeak)
      Console.WriteLine("Maximum Negative peak (Hz)     " & maxNegativePeak)
      Console.WriteLine("Mean RMS (Hz)                  " & meanRms)
      Console.WriteLine("Maximum RMS (Hz)               " & maxRms)


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
