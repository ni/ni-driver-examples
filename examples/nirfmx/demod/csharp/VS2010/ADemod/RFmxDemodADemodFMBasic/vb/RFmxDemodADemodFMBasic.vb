'Steps:
'1. Open a new RFmxInstrMX session and create a Demod Signal
'2. Configure Selected Ports
'3. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
'4. Configure ADemod RBW, Measurement Interval, FM DeEmphasis and Averaging
'5. Read ADemod FM Measurement Results
'6. Dispose Demod Signal and Close the RFmxInstrMX Session

Imports NationalInstruments.RFmx.DemodMX
Imports NationalInstruments.RFmx.InstrMX

Public Class RFmxDemodADemodFMBasic
   Private instrSession As RFmxInstrMX
   Private demod As RFmxDemodMX

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
         'declaring the variables
         Dim selectedPorts As String
         Dim centerFrequency As Double, referenceLevel As Double, externalAttenuation As Double
         Dim deEmphasis As Double, rbw As Double
         Dim averagingCount As Integer

         selectedPorts = ""
         centerFrequency = 1000000000.0
         referenceLevel = 0.0
         externalAttenuation = 0.0
         deEmphasis = 0.0
         rbw = 100000.0
         averagingCount = 10
         Dim meanDeviation As Double, meanCarrierFrequencyError As Double, timeout As Double = 10.0, measurementInterval As Double = 0.01

         'initialising a new session
         Dim resourceName As String = "RFSA"
         instrSession = New RFmxInstrMX(resourceName, "")
         demod = instrSession.GetDemodSignalConfiguration()
         'Configuring the session
         demod.SetSelectedPorts("", selectedPorts)
         demod.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
         demod.ADemod.Configuration.ConfigureRbwFilter("", rbw, RFmxDemodMXADemodRbwFilterType.Flat, 0.1)
         demod.ADemod.Configuration.ConfigureMeasurementInterval("", measurementInterval)
         demod.ADemod.Configuration.ConfigureFMDeEmphasis("", deEmphasis)
         demod.ADemod.Configuration.ConfigureAveraging("", RFmxDemodMXADemodAveragingEnabled.[False], averagingCount, RFmxDemodMXADemodAveragingType.Linear)

         'retriving the results
         demod.ADemod.Results.ReadFM("", timeout, meanDeviation, meanCarrierFrequencyError)
         Console.WriteLine("Mean Deviation(deg)               : " & meanDeviation)
         Console.WriteLine("Mean Carrier Frequency Error(Hz)  : " & meanCarrierFrequencyError)

      Catch e As Exception
         Console.WriteLine(e.Message)
      Finally
         CloseSession()
         Console.WriteLine("Press any key to exit")
         Console.ReadKey()
      End Try
   End Sub
End Class
