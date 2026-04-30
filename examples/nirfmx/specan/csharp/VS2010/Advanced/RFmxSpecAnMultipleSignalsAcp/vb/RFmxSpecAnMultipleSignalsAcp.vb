'Steps:
'1. Open a new RFmx session
'2. Create WCDMA Signal
'3. Configure Selected Ports for WCDMA Signal
'4. Configure the basic signal properties for WCDMA Signal (Center Frequency, Reference Level and External Attenuation)
'5. Configure ACP Averaging for WCDMA Signal
'6. Configure ACP Integration BW, Number of Offset Channels, Channel Spacing for WCDMA Signal
'7. Create LTE Signal
'8. Configure Selected Ports for LTE Signal
'9. Configure the basic signal properties for LTE Signal (Center Frequency, Reference Level and External Attenuation)
'10. Configure ACP Averaging for LTE Signal
'11. Configure ACP Number of Carriers = "1" for LTE Signal
'12. Configure ACP Carrier Integration BW for LTE Signal
'13. Configure ACP Number of Offsets for LTE Signal
'14. Configure ACP Offset Integration BW, Offset Frequency, Sidebands and RRC Filter for LTE Signal
'15. Read ACP Measurements for WCDMA Signal
'16. Read ACP Measurements for LTE Signal
'17. Delete WCDMA Signal
'18. Delete LTE Signal
'19. Close the RFmx session

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.SpecAnMX

Public Class RFmxSpecAnMultipleSignalsAcp
   Private instrSession As RFmxInstrMX
   Private wcdmaSignal As RFmxSpecAnMX, lteSignal As RFmxSpecAnMX
   Private resourceName As String
   Private selectedPorts As String
   Private referenceLevel As Double, externalAttenuation As Double, timeout As Double,
           wcdmaCenterFrequency As Double, wcdmaIntegrationBandwidth As Double,
           wcdmaChannelSpacing As Double, lteCenterFrequency As Double,
           lteIntegrationBandwidth As Double
   Private wcdmaNumberOfOffsetChannels As Integer, wcdmaAveragingCount As Integer,
           lteAveragingCount As Integer
   Private wcdmaAveragingEnabled As RFmxSpecAnMXAcpAveragingEnabled,
           lteAveragingEnabled As RFmxSpecAnMXAcpAveragingEnabled
   Private wcdmaAveragingType As RFmxSpecAnMXAcpAveragingType,
           lteAveragingType As RFmxSpecAnMXAcpAveragingType

   Const NumberOfOffsetChannels As Integer = 2
   Const NumberOfCarriers As Integer = 1

   Private offsetChannelEnabled As RFmxSpecAnMXAcpOffsetEnabled() = New RFmxSpecAnMXAcpOffsetEnabled(NumberOfOffsetChannels - 1) {}
   Private offsetChannelOffset As Double() = New Double(NumberOfOffsetChannels - 1) {}
   Private offsetChannelSideband As RFmxSpecAnMXAcpOffsetSideband() = New RFmxSpecAnMXAcpOffsetSideband(NumberOfOffsetChannels - 1) {}
   Private offsetChannelIntegrationBandwidth As Double() = New Double(NumberOfOffsetChannels - 1) {}
   Private offsetChannelRrcFilterEnabled As RFmxSpecAnMXAcpOffsetRrcFilterEnabled() = New RFmxSpecAnMXAcpOffsetRrcFilterEnabled(NumberOfOffsetChannels - 1) {}
   Private offsetChannelRrcFilterAlpha As Double() = New Double(NumberOfOffsetChannels - 1) {}


   Private wcdmaCarrierAbsolutePower As Double, wcdmaOffCh0LowerRelativePower As Double,
           wcdmaOffCh0UpperRelativePower As Double, wcdmaOffCh1LowerRelativePower As Double,
           wcdmaOffCh1UpperRelativePower As Double
   Private lteCarrierAbsolutePower As Double, lteOffCh0LowerRelativePower As Double,
           lteOffCh0UpperRelativePower As Double, lteOffCh1LowerRelativePower As Double,
           lteOffCh1UpperRelativePower As Double

   Public Sub Run()
      Try
         InitializeVariables()
         InitializeInstr()
         ConfigureSpecAn()
         RetrieveResults()

         PrintResults()
      Catch ex As Exception
         DisplayError(ex.Message)
      Finally
         ' Close session 

         CloseSession()
         Console.WriteLine("Press any key to exit.....")
         Console.ReadKey()
      End Try
   End Sub

   Private Sub InitializeVariables()
      ' Initialize input variables 

      resourceName = "RFSA"
      selectedPorts = ""
      referenceLevel = 0.0 ' dBm 
      externalAttenuation = 0.0 ' dB 
      timeout = 10.0 ' seconds 

      ' WCDMA signal settings 

      wcdmaCenterFrequency = 468000000.0 ' Hz 
      wcdmaIntegrationBandwidth = 3840000.0 ' Hz 
      wcdmaNumberOfOffsetChannels = NumberOfOffsetChannels
      wcdmaChannelSpacing = 5000000.0
      ' Hz 
      wcdmaAveragingEnabled = RFmxSpecAnMXAcpAveragingEnabled.[False]
      wcdmaAveragingCount = 10
      wcdmaAveragingType = RFmxSpecAnMXAcpAveragingType.Rms

      ' LTE signal settings 

      lteCenterFrequency = 2100000000.0 ' Hz 
      lteIntegrationBandwidth = 9000000.0 ' Hz 
      lteAveragingEnabled = RFmxSpecAnMXAcpAveragingEnabled.[False]
      lteAveragingCount = 10
      lteAveragingType = RFmxSpecAnMXAcpAveragingType.Rms

      For i As Integer = 0 To NumberOfOffsetChannels - 1
         offsetChannelEnabled(i) = RFmxSpecAnMXAcpOffsetEnabled.[True]
         offsetChannelSideband(i) = RFmxSpecAnMXAcpOffsetSideband.Both
         offsetChannelRrcFilterAlpha(i) = 0.22

         If i = 0 Then
            offsetChannelOffset(i) = 10000000.0
            offsetChannelIntegrationBandwidth(i) = 9000000.0
            offsetChannelRrcFilterEnabled(i) = RFmxSpecAnMXAcpOffsetRrcFilterEnabled.[False]
         Else
            offsetChannelOffset(i) = 7500000.0
            offsetChannelIntegrationBandwidth(i) = 3840000.0
            offsetChannelRrcFilterEnabled(i) = RFmxSpecAnMXAcpOffsetRrcFilterEnabled.[True]
         End If
      Next
   End Sub

   Private Sub InitializeInstr()
      ' Create a new RFmx Session 

      instrSession = New RFmxInstrMX(resourceName, "")
   End Sub

   Private Sub ConfigureSpecAn()

      ' Get a SpecAn signal for WCDMA analysis 
      wcdmaSignal = instrSession.GetSpecAnSignalConfiguration("WCDMA")

      ' Configure measurement for WCDMA
      wcdmaSignal.SetSelectedPorts("", selectedPorts)
      wcdmaSignal.ConfigureRF("", wcdmaCenterFrequency, referenceLevel, externalAttenuation)
      wcdmaSignal.Acp.Configuration.ConfigureAveraging("", wcdmaAveragingEnabled,
                                                       wcdmaAveragingCount, wcdmaAveragingType)
      wcdmaSignal.Acp.Configuration.ConfigureCarrierAndOffsets("", wcdmaIntegrationBandwidth,
                                                               wcdmaNumberOfOffsetChannels,
                                                               wcdmaChannelSpacing)

      ' Get a SpecAn signal for LTE analysis
      lteSignal = instrSession.GetSpecAnSignalConfiguration("LTE")

      ' Configure measurement for LTE
      lteSignal.SetSelectedPorts("", selectedPorts)
      lteSignal.ConfigureRF("", lteCenterFrequency, referenceLevel, externalAttenuation)
      lteSignal.Acp.Configuration.ConfigureAveraging("", lteAveragingEnabled, lteAveragingCount,
                                                     lteAveragingType)
      lteSignal.Acp.Configuration.ConfigureNumberOfCarriers("", NumberOfCarriers)
      lteSignal.Acp.Configuration.ConfigureCarrierIntegrationBandwidth("",
                                                                       lteIntegrationBandwidth)
      lteSignal.Acp.Configuration.ConfigureNumberOfOffsets("", NumberOfOffsetChannels)

      lteSignal.Acp.Configuration.ConfigureOffsetIntegrationBandwidthArray("", offsetChannelIntegrationBandwidth)
      lteSignal.Acp.Configuration.ConfigureOffsetArray("", offsetChannelOffset, offsetChannelSideband, offsetChannelEnabled)
      lteSignal.Acp.Configuration.ConfigureOffsetRrcFilterArray("", offsetChannelRrcFilterEnabled, offsetChannelRrcFilterAlpha)
   End Sub

   Private Sub RetrieveResults()
      ' Retrieve results 

      wcdmaSignal.Acp.Results.Read("", timeout, wcdmaCarrierAbsolutePower,
                                   wcdmaOffCh0LowerRelativePower,
                                   wcdmaOffCh0UpperRelativePower,
                                   wcdmaOffCh1LowerRelativePower,
                                   wcdmaOffCh1UpperRelativePower)

      lteSignal.Acp.Results.Read("", timeout, lteCarrierAbsolutePower,
                                 lteOffCh0LowerRelativePower,
                                 lteOffCh0UpperRelativePower,
                                 lteOffCh1LowerRelativePower,
                                 lteOffCh1UpperRelativePower)
   End Sub

   Private Sub PrintResults()
      Console.WriteLine("---------------WCDMA Result--------------------" & vbLf)
      Console.WriteLine("Carrier Abs Power (dBm or dBm/Hz)    : {0}", wcdmaCarrierAbsolutePower)
      Console.WriteLine("Off ch0 Lower Relative Power (dB)    : {0}", wcdmaOffCh0LowerRelativePower)
      Console.WriteLine("Off ch0 Upper Relative Power(dB)     : {0}", wcdmaOffCh0UpperRelativePower)
      Console.WriteLine("Off ch1 Lower Relative Power(dB)     : {0}", wcdmaOffCh1LowerRelativePower)
      Console.WriteLine("Off ch1 Upper Relative Power(dB)     : {0}", wcdmaOffCh1UpperRelativePower)

      Console.WriteLine(vbLf & "---------------LTE Result----------------------" & vbLf)
      Console.WriteLine("Carrier Abs Power (dBm or dBm/Hz)    : {0}", lteCarrierAbsolutePower)
      Console.WriteLine("Off ch0 Lower Relative Power (dB)    : {0}", lteOffCh0LowerRelativePower)
      Console.WriteLine("Off ch0 Upper Relative Power(dB)     : {0}", lteOffCh0UpperRelativePower)
      Console.WriteLine("Off ch1 Lower Relative Power(dB)     : {0}", lteOffCh1LowerRelativePower)
      Console.WriteLine("Off ch1 Upper Relative Power(dB)     : {0}", lteOffCh1UpperRelativePower)
   End Sub

   Private Sub CloseSession()
      Try
         If wcdmaSignal IsNot Nothing Then
            wcdmaSignal.Dispose()
            wcdmaSignal = Nothing
         End If
         If lteSignal IsNot Nothing Then
            lteSignal.Dispose()
            lteSignal = Nothing
         End If
         If instrSession IsNot Nothing Then
            instrSession.Close()
            instrSession = Nothing
         End If
      Catch ex As Exception
         DisplayError(ex.Message)
      End Try
   End Sub

   Private Shared Sub DisplayError(message As String)
      Console.WriteLine("ERROR:" & vbLf & message)
   End Sub
End Class
