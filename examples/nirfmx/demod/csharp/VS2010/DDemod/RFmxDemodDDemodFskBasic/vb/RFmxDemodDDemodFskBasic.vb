'Steps:
'1. Open a new RFmxInstrMX session and create a Demod Signal
'2. Configure the basic instrument properties (Clock Source, Clock Frequency) 
'3. Configure Selected Ports
'4. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
'5. Select DDemod Measurement
'6. Configure FSK Modulation and M
'7. Configure DDemod FSK Deviation 
'8. Configure DDemod Symbol Rate, Number of Symbols and Pulse Shaping Filter
'9. Configure DDemod Measurement Filter Type as Auto
'10. Configure DDemod Averaging
'11. Initiate Measurement
'12. Read FSK Measurement Results
'13. Dispose Demod Signal and Close the RFmxInstrMX Session

Imports NationalInstruments.RFmx.DemodMX
Imports NationalInstruments.RFmx.InstrMX

Public Class RFmxDemodDDemodFskBasic
   Private demod As RFmxDemodMX
   Private instrSession As RFmxInstrMX

   Private meanFskDeviation As Double, meanRmsFskError As Double, maxPeakFskError As Double,
       timeout As Double = 10.0



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
         'declare the variables 
         Dim frequencySource As String
         Dim selectedPorts As String
         Dim frequency As Double, centerFrequency As Double, referenceLevel As Double, externalAttenuation As Double
         Dim symbolRate As Double, fskDeviation As Double, pulseShapingFilterParameter As Double
         Dim numOfSymbols As Integer
         Dim averagingCount As Integer

         selectedPorts = ""
         centerFrequency = 1000000000.0
         referenceLevel = 0.0
         externalAttenuation = 0.0

         frequency = 10000000.0
         frequencySource = RFmxInstrMXConstants.OnboardClock

         fskDeviation = 15000.0
         symbolRate = 100000.0
         numOfSymbols = 1000

         pulseShapingFilterParameter = 0.5
         averagingCount = 10
         'initialise the demod session
         Dim resourceName As String = "RFSA"
         instrSession = New RFmxInstrMX(resourceName, "")
         demod = instrSession.GetDemodSignalConfiguration()

         'Configure the session
         instrSession.ConfigureFrequencyReference("", frequencySource, frequency)
         demod.SetSelectedPorts("", selectedPorts)
         demod.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
         demod.SelectMeasurements("", RFmxDemodMXMeasurementTypes.DDemod, True)
         demod.DDemod.Configuration.ConfigureModulationType("", RFmxDemodMXDDemodModulationType.Fsk,
             RFmxDemodMXDDemodM.M2, RFmxDemodMXDDemodDifferentialEnabled.False)
         demod.DDemod.Configuration.ConfigureFskDeviation("", fskDeviation, RFmxDemodMXDDemodFskReferenceCompensationEnabled.True)
         demod.DDemod.Configuration.ConfigureSymbolRate("", symbolRate)
         demod.DDemod.Configuration.ConfigureNumberOfSymbols("", numOfSymbols)

         demod.DDemod.Configuration.ConfigurePulseShapingFilter("", RFmxDemodMXDDemodPulseShapingFilterType.Gaussian,
                                                                pulseShapingFilterParameter, 0, 1, Nothing)

         demod.DDemod.Configuration.ConfigureMeasurementFilter("", RFmxDemodMXDDemodMeasurementFilterType.Auto,
                                                               0, 1, Nothing)
         demod.DDemod.Configuration.ConfigureAveraging("", RFmxDemodMXDDemodAveragingEnabled.False, averagingCount)
         demod.Initiate("", "")

         'retrieve the results
         demod.DDemod.Results.FetchFskResults("", timeout, meanFskDeviation, meanRmsFskError, maxPeakFskError)
         Console.WriteLine("Mean FSK Deviation(Hz)                : " & meanFskDeviation & vbLf)
         Console.WriteLine("Mean RMS FSK Error(Hz)      : " & meanRmsFskError & vbLf)
         Console.WriteLine("Maximum Peak FSK Error (%)  : " & maxPeakFskError & vbLf)

      Catch e As Exception
         Console.WriteLine(e.Message)
      Finally
         CloseSession()
         Console.WriteLine("Press any key to exit")
         Console.ReadKey()
      End Try
   End Sub
End Class
