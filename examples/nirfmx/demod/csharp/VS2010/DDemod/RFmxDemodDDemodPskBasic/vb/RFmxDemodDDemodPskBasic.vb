'Steps:
'1. Open a new RFmxInstrMX session and create a Demod Signal
'2. Configure Selected Ports
'3. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
'4. Select PSK Modulation and M
'5. Configure DDemod PSK Format, Symbol Rate, Number of Symbols and Pulse Shaping Filter
'6. Configure DDemod Measurement Filter Type as Auto
'7. Configure DDemod Averaging
'8. Read DDemod Measurement Results
'9. Dispose Demod Signal and Close the RFmxInstrMX Session

Imports NationalInstruments.RFmx.DemodMX
Imports NationalInstruments.RFmx.InstrMX

Public Class RFmxDemodDDemodPskBasic
   Private demod As RFmxDemodMX
   Private instrSession As RFmxInstrMX

   Public Sub CloseSession()
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
         'declaring the variables
         Dim symbolRate As Double, pulseShapingFilterParameter As Double
         Dim numOfSymbols As Integer
         Dim averagingCount As Integer

         Dim selectedPorts As String = ""
         Dim centerFrequency As Double = 1000000000.0
         Dim referenceLevel As Double = 0.0
         Dim externalAttenuation As Double = 0.0
         Dim m As RFmxDemodMXDDemodM
         Dim meanFrequencyOffset As Double, meanRmsEvm As Double, maxPeakEvm As Double, meanModulationErrorRatio As Double,
             timeout As Double = 10.0
         symbolRate = 100000.0
         numOfSymbols = 1000
         m = RFmxDemodMXDDemodM.M4

         pulseShapingFilterParameter = 0.5
         averagingCount = 10
         Dim resourceName As String = "RFSA"

         'initialise the session
         instrSession = New RFmxInstrMX(resourceName, "")
         demod = instrSession.GetDemodSignalConfiguration()
         'configuring the session
         demod.SetSelectedPorts("", selectedPorts)
         demod.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
         demod.DDemod.Configuration.ConfigureModulationType("", RFmxDemodMXDDemodModulationType.Psk,
             m, RFmxDemodMXDDemodDifferentialEnabled.False)
         demod.DDemod.Configuration.ConfigurePskFormat("", RFmxDemodMXDDemodPskFormat.Normal)
         demod.DDemod.Configuration.ConfigureSymbolRate("", symbolRate)
         demod.DDemod.Configuration.ConfigureNumberOfSymbols("", numOfSymbols)

         demod.DDemod.Configuration.ConfigurePulseShapingFilter("", RFmxDemodMXDDemodPulseShapingFilterType.RootRaisedCosine,
                                                                pulseShapingFilterParameter, 0, 1, Nothing)
         demod.DDemod.Configuration.ConfigureMeasurementFilter("", RFmxDemodMXDDemodMeasurementFilterType.Auto, 0, 1, Nothing)
         demod.DDemod.Configuration.ConfigureAveraging("", RFmxDemodMXDDemodAveragingEnabled.False, averagingCount)

         'retriving the results
         demod.DDemod.Results.Read("", timeout, meanFrequencyOffset, meanRmsEvm, maxPeakEvm, meanModulationErrorRatio)
         Console.WriteLine("Mean Frequency Offset (Hz)            : " & meanFrequencyOffset)
         Console.WriteLine("Mean RMS EVM (%)                      : " & meanRmsEvm)
         Console.WriteLine("Maximum Peak EVM (%)                  : " & maxPeakEvm)
         Console.WriteLine("Mean Modulation Error Ratio(dB)       : " & meanModulationErrorRatio)

      Catch e As Exception
         Console.WriteLine(e.Message)
      Finally
         CloseSession()
         Console.WriteLine("Press any key to exit")
         Console.ReadKey()
      End Try
   End Sub
End Class
