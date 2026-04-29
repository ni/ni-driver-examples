'Steps:
'1. Open a new RFmxInstrMX session and create a Demod Signal
'2. Configure Selected Ports
'3. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
'4. Select MSK Modulation and Differential Enabled
'5. Configure DDemod Symbol Rate, Number of Symbols and Pulse Shaping Filter
'6. Configure DDemod Measurement Filter Type as Auto
'7. Configure DDemod Averaging
'8. Read DDemod Measurement Results
'9. Dispose Demod Signal and Close the RFmxInstrMX Session

Imports NationalInstruments.RFmx.DemodMX
Imports NationalInstruments.RFmx.InstrMX

Public Class RFmxDemodDDemodMskBasic
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
         Dim resourceName As String = "RFSA", selectorString As String = ""
         Dim selectedPorts As String = ""
         Dim centerFrequency As Double = 1000000000.0
         ' Hz 
         Dim referenceLevel As Double = 0.0
         ' dBm 
         Dim externalAttenuation As Double = 0.0
         ' dB 

         Dim timeout As Double = 10.0
         ' seconds 

         Dim symbolRate As Double = 100000.0
         Dim numOfSymbols As Integer = 1000

         ' Pulse shaping filter 

         Dim pulseShapingFilterParameter As Double = 0.5

         ' Averaging 


         Dim averagingCount As Integer = 10

         ' Variables to store the results 

         Dim meanFrequencyOffset As Double = 0
         Dim meanRmsEvm As Double = 0
         Dim maxPeakEvm As Double = 0
         Dim meanModulationErrorRatio As Double = 0

         ' Create a new RFmx Session 

         instrSession = New RFmxInstrMX(resourceName, "")
         demod = instrSession.GetDemodSignalConfiguration()
         ' Configure DDemod parameters 
         demod.SetSelectedPorts("", selectedPorts)
         demod.ConfigureRF(selectorString, centerFrequency, referenceLevel, externalAttenuation)
         demod.DDemod.Configuration.ConfigureModulationType(selectorString, RFmxDemodMXDDemodModulationType.Msk,
                                                            RFmxDemodMXDDemodM.M4,
                                                            RFmxDemodMXDDemodDifferentialEnabled.[False])
         demod.DDemod.Configuration.ConfigureSymbolRate(selectorString, symbolRate)
         demod.DDemod.Configuration.ConfigureNumberOfSymbols(selectorString, numOfSymbols)
         demod.DDemod.Configuration.ConfigurePulseShapingFilter(selectorString, RFmxDemodMXDDemodPulseShapingFilterType.Gaussian,
                                                                pulseShapingFilterParameter, 0, 1, Nothing)
         demod.DDemod.Configuration.ConfigureMeasurementFilter(selectorString, RFmxDemodMXDDemodMeasurementFilterType.Auto, 0, 1, Nothing)
         demod.DDemod.Configuration.ConfigureAveraging(selectorString, RFmxDemodMXDDemodAveragingEnabled.[False], averagingCount)


         ' Retrieve results 

         demod.DDemod.Results.Read(selectorString, timeout, meanFrequencyOffset, meanRmsEvm, maxPeakEvm,
                                   meanModulationErrorRatio)

         ' Display results 

         Console.WriteLine("Mean Frequency Offset (Hz)         : " & meanFrequencyOffset)
         Console.WriteLine("Mean RMS EVM(%)                    : " & meanRmsEvm)
         Console.WriteLine("Maximum Peak EVM(%)                : " & maxPeakEvm)
         Console.WriteLine("Mean Modulation Error Ratio(dB)    : " & meanModulationErrorRatio)
      Catch e As Exception
         Console.WriteLine(e.Message)
      Finally
         CloseSession()
         Console.WriteLine("Press any key to exit")
         Console.ReadKey()
      End Try
   End Sub
End Class
