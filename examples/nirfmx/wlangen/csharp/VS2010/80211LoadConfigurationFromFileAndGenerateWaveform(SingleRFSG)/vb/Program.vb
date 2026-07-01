Imports NationalInstruments.RFToolkits.Wlan.Generation
Imports NationalInstruments.ModularInstruments.NIRfsg
Imports NationalInstruments.ModularInstruments.NIRfsg.Internal
Imports System.Runtime.InteropServices


Class Program
   Friend Shared Sub Main(args As String())
      Dim example As New LoadConfigurationFromFileAndGenerateWaveform()
      example.Run()
   End Sub
End Class

Class LoadConfigurationFromFileAndGenerateWaveform
   Private carrierFrequency As Double = 5180000000.0
   Private powerLevel As Double = -10.0
   Private externalAttenuation As Double = 0
   Private filePath As [String] = ""
   Private rfsgSession As NIRfsg
   Private wlanSession As niWLANG
   Private rfsgResourceName As [String] = "RIO0"
   Private rfsgClockSource As [String] = RfsgFrequencyReferenceSource.OnboardClock
   Private externalLOSession As NIRfsg = Nothing

   Private waveformName As [String] = "Wlan"

   Public Sub Run()
      Try
         ConfigureRfsgSession()
         ConfigureWlanGenerationSession()
         CreateAndDownloadWaveform()
         StopGeneration()
      Catch e As Exception
         Console.WriteLine("Error : " & e.ToString())
         Console.ReadKey()
      Finally
         If rfsgSession IsNot Nothing Then
            rfsgSession.Close()
         End If

         If wlanSession IsNot Nothing Then
            wlanSession.Close()
         End If
      End Try
   End Sub

   Private Sub ConfigureWlanGenerationSession()
      Dim isNewSession As Integer

      If wlanSession Is Nothing Then
         wlanSession = New niWLANG("WLANG", niWLANGConstants.CompatibilityVersion060000, isNewSession)
      End If

      wlanSession.LoadConfigurationFromFile(filePath, niWLANGConstants.[True])
   End Sub

   Private Sub CreateAndDownloadWaveform()
      Dim iqRate As Double
      Dim actualHeadRoom As Double
      Dim script As [String] = "script GenerateWlan" & vbCr & vbLf & vbTab & vbTab & vbTab & vbTab & "                repeat forever" & vbCr & vbLf & vbTab & vbTab & vbTab & vbTab & vbTab & "                generate Wlan" & vbCr & vbLf & vbTab & vbTab & vbTab & vbTab & "                end repeat" & vbCr & vbLf & vbTab & vbTab & vbTab & "                end script"
      Dim standard As Integer

      wlanSession.GetStandard(Nothing, standard)
      Dim ChannelString As [String]
      Dim externalLOHandle As New IntPtr()

      If externalLOSession IsNot Nothing Then
         externalLOHandle = externalLOSession.GetInstrumentHandle().DangerousGetHandle()
      End If
      wlanSession.RFSGConfigureFrequencySingleLO(New IntPtr() {rfsgSession.GetInstrumentHandle().DangerousGetHandle()}, niWLANGConstants.LOSourceOnboard, externalLOHandle, carrierFrequency, niWLANGConstants.[False], niWLANGConstants.[False])

      If standard = niWLANGConstants.Standard80211AcMimoOfdm OrElse standard = niWLANGConstants.Standard80211AfMimoOfdm OrElse standard = niWLANGConstants.Standard80211nMimoOfdm OrElse standard = niWLANGConstants.Standard80211AhMimoOfdm OrElse standard = niWLANGConstants.Standard80211AxMimoOfdm OrElse standard = niWLANGConstants.Standard80211beMimoOfdm OrElse standard = niWLANGConstants.Standard80211bnMimoOfdm Then
         wlanSession.RFSGCreateAndDownloadMIMOWaveforms(New IntPtr() {rfsgSession.GetInstrumentHandle().DangerousGetHandle()}, Nothing, 1, waveformName)
         ChannelString = "channel0"
      Else
         wlanSession.RFSGCreateAndDownloadWaveform(rfsgSession.GetInstrumentHandle().DangerousGetHandle(), Nothing, waveformName)
         ChannelString = ""
      End If

      wlanSession.GetIqRate(Nothing, iqRate)
      wlanSession.GetActualHeadroom(ChannelString, actualHeadRoom)

      Console.WriteLine("Standard : {0}", standard)
      Console.WriteLine("IQ Rate : {0}", iqRate)
      Console.WriteLine("Actual HeadRoom : {0}", actualHeadRoom)

      wlanSession.Close()

      niWLANG.WLANG_RFSGConfigureScript(rfsgSession.GetInstrumentHandle().DangerousGetHandle(), Nothing, script, powerLevel)
      rfsgSession.RF.OutputEnabled = True

      rfsgSession.Initiate()
      'Check for successful generation
      CheckGeneration()
      Console.WriteLine("Press any key to exit")
      Console.ReadKey()
   End Sub

   Private Sub ConfigureRfsgSession()

      If rfsgSession Is Nothing Then
         rfsgSession = New NIRfsg(rfsgResourceName, True, False)
      End If
      rfsgSession.FrequencyReference.Configure(rfsgClockSource, 10000000.0)
      rfsgSession.RF.PowerLevelType = RfsgRFPowerLevelType.PeakPower
      rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.Script
      rfsgSession.RF.ExternalGain = -(externalAttenuation)
   End Sub

   Private Function CheckGeneration() As Boolean
      Dim status As RfsgGenerationStatus = RfsgGenerationStatus.InProgress
      status = rfsgSession.CheckGenerationStatus()
      Return Convert.ToBoolean(status)
   End Function

   Private Sub StopGeneration()
      rfsgSession.Abort()
      rfsgSession.RF.OutputEnabled = False
      niWLANG.WLANG_RFSGClearDatabase(rfsgSession.GetInstrumentHandle().DangerousGetHandle(), "", Nothing)
   End Sub
End Class
