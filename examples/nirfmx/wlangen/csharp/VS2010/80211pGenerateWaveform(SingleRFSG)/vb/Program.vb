Imports NationalInstruments.RFToolkits.Wlan.Generation
Imports NationalInstruments.ModularInstruments.NIRfsg
Imports NationalInstruments.ModularInstruments.NIRfsg.Internal
Imports System.Runtime.InteropServices


Class Program
   Friend Shared Sub Main(args As String())
      Dim example As New GenerateWaveformSingleUserExample()
      example.Run()
   End Sub
End Class

Class GenerateWaveformSingleUserExample
   Private carrierFrequency As Double = 5180000000.0
   Private channelBandwidth As Double = 20000000.0
   Private ofdmDataRate As Integer = 6
   Private powerLevel As Double = -10.0
   Private externalAttenuation As Double = 0

   Private standard As Integer = niWLANGConstants.Standard80211pOfdm

   Private rfsgSession As NIRfsg
   Private externalLOSession As NIRfsg = Nothing
   Private wlanSession As niWLANG
   Private rfsgResourceName As [String] = "RIO0"
   Private rfsgClockSource As [String] = RfsgFrequencyReferenceSource.OnboardClock

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

      wlanSession.SetStandard(Nothing, standard)
      wlanSession.SetChannelBandwidth(Nothing, channelBandwidth)
      wlanSession.SetScalarAttributeI32(Nothing, niWLANGProperties.OfdmDataRate, ofdmDataRate)
      wlanSession.SetPulseShapingFilterEnabled(Nothing, niWLANGConstants.[True])
      wlanSession.SetPulseShapingFilterParameter(Nothing, 0.1)
      wlanSession.SetPulseShapingFilterType(Nothing, niWLANGConstants.FilterRaisedCosine)
      wlanSession.SetPulseShapingFilterLength(Nothing, 100)
      wlanSession.SetRFBlankingEnabled(Nothing, niWLANGConstants.[True])
   End Sub

   Private Sub CreateAndDownloadWaveform()
      Dim iqRate As Double
      Dim actualHeadRoom As Double
      Dim standard As Integer
      Dim externalLOHandle As New IntPtr()

      Dim script As [String] = "script GenerateWlan" & vbCr & vbLf & vbTab & vbTab & vbTab & vbTab & "                repeat forever" & vbCr & vbLf & vbTab & vbTab & vbTab & vbTab & vbTab & "                generate Wlan" & vbCr & vbLf & vbTab & vbTab & vbTab & vbTab & "                end repeat" & vbCr & vbLf & vbTab & vbTab & vbTab & "                end script"

      wlanSession.GetStandard(Nothing, standard)

      If externalLOSession IsNot Nothing Then
         externalLOHandle = externalLOSession.GetInstrumentHandle().DangerousGetHandle()
      End If
      wlanSession.RFSGConfigureFrequencySingleLO(New IntPtr() {rfsgSession.GetInstrumentHandle().DangerousGetHandle()}, niWLANGConstants.LOSourceOnboard, externalLOHandle, carrierFrequency, niWLANGConstants.[False], niWLANGConstants.[False])


      wlanSession.RFSGCreateAndDownloadWaveform(rfsgSession.GetInstrumentHandle().DangerousGetHandle(), Nothing, "Wlan")
      wlanSession.GetIqRate(Nothing, iqRate)
      wlanSession.GetActualHeadroom("", actualHeadRoom)

      Console.WriteLine("Standard : {0}", standard)
      Console.WriteLine("IQ Rate : {0}", iqRate)
      Console.WriteLine("Actual HeadRoom : {0}", actualHeadRoom)

      'Close WLAN session

      wlanSession.Close()

      'Configure Script

      niWLANG.WLANG_RFSGConfigureScript(rfsgSession.GetInstrumentHandle().DangerousGetHandle(), Nothing, script, powerLevel)
      rfsgSession.RF.OutputEnabled = True

      'Initiate Generation

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
      rfsgSession.Utility.Commit()
      niWLANG.WLANG_RFSGClearDatabase(rfsgSession.GetInstrumentHandle().DangerousGetHandle(), "", Nothing)
   End Sub
End Class
