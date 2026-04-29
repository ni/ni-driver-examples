'Steps:
'1. Open a new RFmx session and create a Demod Signal
'2. Configure Selected Ports
'3. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
'4. Configure ADemod RBW, RBW Filter Type, Measurement Interval 
'5. Configure ADemod Carrier Frequency and Carrier Phase Correction as True
'6. Configure ADemod Averaging 
'7. Read ADemod PM Measurement Results
'8. Dispose Demod Signal and Close the RFmx Session

Imports NationalInstruments.RFmx.DemodMX
Imports NationalInstruments.RFmx.InstrMX

Public Class RFmxDemodADemodPMBasic
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
         'declaring all variables
         Dim selectedPorts As String
         Dim centerFrequency As Double, referenceLevel As Double, externalAttenuation As Double
         Dim measurementInterval As Double, rbw As Double
         Dim averagingCount As Integer

         selectedPorts = ""
         centerFrequency = 1000000000.0
         referenceLevel = 0.0
         externalAttenuation = 0.0
         measurementInterval = 0.01
         rbw = 100000.0
         averagingCount = 10
         Dim meanDeviation As Double, meanCarrierFrequencyError As Double, timeout As Double = 10.0
         'initialising demod session
         Dim resourceName As String = "RFSA"
         instrSession = New RFmxInstrMX(resourceName, "")
         demod = instrSession.GetDemodSignalConfiguration()
         'configuring the session
         demod.SetSelectedPorts("", selectedPorts)
         demod.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
         demod.ADemod.Configuration.ConfigureRbwFilter("", rbw, RFmxDemodMXADemodRbwFilterType.Flat, 0.1)
         demod.ADemod.Configuration.ConfigureMeasurementInterval("", measurementInterval)
         demod.ADemod.Configuration.ConfigureCarrierCorrection("", RFmxDemodMXADemodCarrierFrequencyCorrectionEnabled.True,
                                 RFmxDemodMXADemodCarrierPhaseCorrectionEnabled.True)
         demod.ADemod.Configuration.ConfigureAveraging("", RFmxDemodMXADemodAveragingEnabled.False,
             averagingCount, RFmxDemodMXADemodAveragingType.Linear)
         'retriving the results
         demod.ADemod.Results.ReadPM("", timeout, meanDeviation, meanCarrierFrequencyError)
         Console.WriteLine("Mean Deviation(deg)              : " & meanDeviation & vbLf)
         Console.WriteLine("Mean Carrier Frequency Error(Hz) : " & meanCarrierFrequencyError & vbLf)

      Catch e As Exception
         Console.WriteLine(e.Message)
      Finally
         CloseSession()
         Console.WriteLine("Press any key to exit")
         Console.ReadKey()
      End Try
   End Sub
End Class
