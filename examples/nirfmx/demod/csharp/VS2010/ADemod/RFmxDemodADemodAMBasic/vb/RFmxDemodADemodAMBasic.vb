'Steps:
'1. Open a new RFmxInstrMX session and create a Demod Signal
'2. Configure Selected Ports
'3. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
'4. Configure ADemod RBW, Measurement Interval, AM Carrier Suppressed and Averaging
'5. Read ADemod AM Measurement Results
'6. Dispose Demod Signal and Close the RFmxInstrMX Session

Imports NationalInstruments.RFmx.DemodMX
Imports NationalInstruments.RFmx.InstrMX

Public Class RFmxADemodAMBasic
   Private demod As RFmxDemodMX
   Private instrSession As RFmxInstrMX


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
         Dim measurementInterval As Double, rbw As Double

         selectedPorts = ""
         centerFrequency = 1000000000.0
         referenceLevel = 0.0
         externalAttenuation = 0.0
         measurementInterval = 0.01
         rbw = 100000.0
         Dim averagingCount As Integer = 10
         Dim meanModulationDepth As Double, meanCarrierPower As Double, timeout As Double = 10.0

         'initialising session
         Dim resourceName As String = "RFSA"
         instrSession = New RFmxInstrMX(resourceName, "")
         demod = instrSession.GetDemodSignalConfiguration()
         'Configuring the session
         demod.SetSelectedPorts("", selectedPorts)
         demod.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
         demod.ADemod.Configuration.ConfigureRbwFilter("", rbw, RFmxDemodMXADemodRbwFilterType.Flat, 0.1)
         demod.ADemod.Configuration.ConfigureMeasurementInterval("", measurementInterval)
         demod.ADemod.Configuration.ConfigureAMCarrierSuppressed("", RFmxDemodMXADemodAMCarrierSuppressedEnabled.False)
         demod.ADemod.Configuration.ConfigureAveraging("", RFmxDemodMXADemodAveragingEnabled.False,
                     averagingCount, RFmxDemodMXADemodAveragingType.Linear)

         'retriving results
         demod.ADemod.Results.ReadAM("", timeout, meanModulationDepth, meanCarrierPower)
         Console.WriteLine("Mean Modulation Depth (%)    :" & meanModulationDepth & vbLf)
         Console.WriteLine("Mean Carrier Power   (dBm)   :" & meanCarrierPower & vbLf)
      Catch e As Exception
         Console.WriteLine(e.Message)
      Finally
         CloseSession()
         Console.WriteLine("Press any key to exit")
         Console.ReadKey()
      End Try
   End Sub
End Class
