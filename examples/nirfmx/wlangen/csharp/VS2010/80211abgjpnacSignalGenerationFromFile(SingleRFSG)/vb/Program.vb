
Imports NationalInstruments.RFToolkits.Wlan.Generation
Imports NationalInstruments.ModularInstruments.NIRfsg
Imports NationalInstruments.ModularInstruments.NIRfsg.Internal
Imports System.Runtime.InteropServices

Class Program
   Friend Shared Sub Main(args As String())
      Dim example As New SignalGenerationFromFileExample()
      example.Run()
   End Sub
End Class

Class SignalGenerationFromFileExample
   Private carrierFrequency As Double = 5180000000.0
   Private powerLevel As Double = -10.0
   Private externalAttenuation As Double = 0
   Private rfsgSession As NIRfsg
   Private rfsgResourceName As [String] = "RFSA"
   Private rfsgClockSource As [String] = RfsgFrequencyReferenceSource.OnboardClock
   Private script As [String] = "script GenerateWlan" & vbCr & vbLf & "                            repeat forever" & vbCr & vbLf & vbTab & vbTab & "        " & vbTab & vbTab & vbTab & "    generate Wlan" & vbCr & vbLf & vbTab & vbTab & vbTab & vbTab & "            end repeat" & vbCr & vbLf & vbTab & vbTab & vbTab & "             end script"
   Private filePath As [String] = ""
   Private waveformName As [String] = "Wlan"
   Private handle As IntPtr = IntPtr.Zero

   Public Sub Run()
      Try
         ConfigureRfsgSession()
         ReadAndDownloadWaveform()
         StopGeneration()
      Catch e As Exception
         Console.WriteLine("Error : " & e.ToString())
         Console.ReadKey()
      Finally
         If rfsgSession IsNot Nothing Then
            rfsgSession.Close()
         End If
      End Try
   End Sub

   Private Sub ReadAndDownloadWaveform()
      handle = rfsgSession.GetInstrumentHandle().DangerousGetHandle()
      niWLANG.WLANG_RFSGReadAndDownloadWaveformsFromFile(New IntPtr() {handle}, 1, waveformName, filePath)
      niWLANG.WLANG_RFSGConfigureScript(handle, "", script, powerLevel)
      rfsgSession.RF.OutputEnabled = True
      rfsgSession.Initiate()
      CheckGeneration()
      Console.WriteLine("Press any key to exit")
      Console.ReadKey()
   End Sub

   Private Sub ConfigureRfsgSession()
      If rfsgSession Is Nothing Then
         rfsgSession = New NIRfsg(rfsgResourceName, True, False)
      End If

      rfsgSession.RF.Frequency = carrierFrequency
      rfsgSession.RF.PowerLevelType = RfsgRFPowerLevelType.PeakPower
      rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.Script
      rfsgSession.FrequencyReference.Configure(rfsgClockSource, 10000000.0)
      rfsgSession.RF.ExternalGain = -(externalAttenuation)
   End Sub

   Private Function CheckGeneration() As Boolean
      Dim status As RfsgGenerationStatus = rfsgSession.CheckGenerationStatus()
      Return Convert.ToBoolean(status)
   End Function

   Private Sub StopGeneration()
      rfsgSession.Abort()
      rfsgSession.RF.OutputEnabled = False
      rfsgSession.Utility.Commit()
      niWLANG.WLANG_RFSGClearDatabase(handle, "", waveformName)
   End Sub
End Class
