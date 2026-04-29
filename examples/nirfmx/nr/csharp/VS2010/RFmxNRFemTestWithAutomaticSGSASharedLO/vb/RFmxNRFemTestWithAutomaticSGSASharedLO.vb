'[1] Steps
'1. Open an NI - RFSG session.
'2. Configure RFSG Selected Ports.
'3. Configure RFSG frequency reference.
'4. Configure frequency and power level of RF output signal.
'5. Set RFSG External Gain. #4 and #5 ensure that the average power of the signal at the input of the DUT
'   matches the user configured DUT Average Input Power.
'6. Read waveform from file and download Waveform from file to RFSG.
'7. Set Automatic SG SA Shared LO to Enabled.
'
'[2] Steps to perform ModAcc measurement
'8. Set LO Offset Mode to Auto while performing an in - band ModAcc measurement.
'   This causes the RFSG LO to be placed outside the signal, if signal bandwidth is less than
'   half of the device instantaneous bandwidth; otherwise, the LO is placed at the center of the signal.
'9. Write script to generate the waveform specified in the script. This script is programmed to generate waveform continuously.
'10. Initiate signal generation.
'-------------------------------------------------------------------------------------------------------------------------------
'11. Open a new RFmx Session.
'12. Configure the Frequency Reference properties (Clock Source and Clock Frequency).
'13. Configure Selected Ports.
'14. Configure Automatic SG SA Shared LO to Enabled.
'15. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
'16. Configure Trigger Type and Trigger Parameters.
'17. Configure Link Direction, Frequency Range, CC bandwidth, Cell ID, Band and BWP Subcarrier Spacing.
'18. Set LO Leakage Avoidance Enabled to True. This causes RFmx to place the SA LO outside the measurement bandwidth,
'    if the measurement bandwidth is less than half of the device instantaneous bandwidth;
'    otherwise, the LO is placed at the center of the signal.
'19. Select ModAcc measurement and disable Traces.
'20. Initiate ModAcc measurement.
'21. Fetch ModAcc measurements.
'
'[3] Steps to perform SEM measurement
'22. Stop signal generation.
'23. Configure LO Offset Mode for SEM measurement.
'    Set LO Offset Mode to Auto. This causes the RFSG LO to be placed outside the signal, if signal bandwidth is less than
'    half of the device instantaneous bandwidth; otherwise, the LO is placed at the center of the signal.
'    Set LO Offset Mode to No Offset when you see significant difference in the SEM upper and lower offset margin results.
'    This causes the RFSG LO to be placed at the center of the signal and avoids RFSG LO impacting the SEM offset results.
'24. Write script to generate the waveform specified in the script. This script is programmed to generate waveform continuously.
'25. Initiate signal generation.
'-------------------------------------------------------------------------------------------------------------------------------
'26. Select SEM measurement and disable the traces.
'27. Initiate SEM measurement.
'28. Fetch SEM measurements.
'
'[4] Steps
'29. Close the RFmx Session.
'30. Close the RFSG session.
'It is recommended to clear the waveform before closing RFSG session.


Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.NRMX
Imports NationalInstruments.ModularInstruments.NIRfsg
Imports NationalInstruments.ModularInstruments.NIRfsgPlayback

Namespace NationalInstruments.Examples.RFmxNRFemTestWithAutomaticSGSASharedLO

   Public Class RFmxNRFemTestWithAutomaticSGSASharedLO
      Private instrSession As RFmxInstrMX
      Private NR As RFmxNRMX
      Private rfsgSession As NIRfsg
      Private instrumentHandle As IntPtr

      Private centerFrequency As Double

      Private rfsgResourceName As String
      Private rfsgSelectedPorts As String
      Private waveformFilePath As String
      Private waveformName As String

      Private powerLevel As Double
      Private rfsgExternalAttenuation As Double

      Private rfsgFrequencyReferenceSource As RfsgFrequencyReferenceSource
      Private rfsgFrequency As Double

      Private rfsaResourceName As String
      Private rfsaSelectedPorts As String
      Private referenceLevel As Double
      Private rfsaExternalAttenuation As Double

      Private rfsaFrequencyReferenceSource As String
      Private rfsaFrequency As Double

      Private enableTrigger As Boolean
      Private digitalEdgeSource As String
      Private digitalEdge As RFmxNRMXDigitalEdgeTriggerEdge
      Private triggerDelay As Double

      Private frequencyRange As RFmxNRMXFrequencyRange
      Private carrierBandwidth As Double
      Private subcarrierSpacing As Double
      Private band As Integer
      Private cellID As Integer

      Private rfsgLOOffsetMode As NIRfsgPlaybackLOOffsetMode

      Private timeout As Double
      Private script As String

      Private compositeRmsEvmMean As Double
      ' (%)
      Private compositePeakEvmMaximum As Double
      ' (%)

      Private measurementStatus As RFmxNRMXSemMeasurementStatus

      Private absolutePower As Double
      ' (dBm)
      Private relativePower As Double
      Private peakFrequency As Double
      Private peakAbsolutePower As Double

      Private lowerOffsetMeasurementStatus As RFmxNRMXSemLowerOffsetMeasurementStatus()
      Private lowerOffsetMargin As Double()
      ' (dB)
      Private lowerOffsetMarginFrequency As Double()
      ' (Hz)
      Private lowerOffsetMarginAbsolutePower As Double()
      ' (dBm)
      Private lowerOffsetMarginRelativePower As Double()

      Private upperOffsetMeasurementStatus As RFmxNRMXSemUpperOffsetMeasurementStatus()
      Private upperOffsetMargin As Double()
      ' (dB)
      Private upperOffsetMarginFrequency As Double()
      ' (Hz)
      Private upperOffsetMarginAbsolutePower As Double()
      ' (dBm)
      Private upperOffsetMarginRelativePower As Double()

      Public Sub Run()
         Try
            InitializeVariables()
            ConfigureRfsg()
            ConfigureRFmxAndRetrieveResults()
            PrintResults()
         Catch ex As Exception
            DisplayError(ex)
         Finally
            CloseSession()
            Console.WriteLine("Press any key to exit")
            Console.ReadKey()
         End Try
      End Sub

      Private Sub InitializeVariables()
         centerFrequency = 3500000000.0
         ' (Hz)

         rfsgResourceName = "RFSG"
         rfsgSelectedPorts = ""
         waveformFilePath = "NR_FR2_UL_SISO_CC-1_BW-50MHz_SCS-120kHz.tdms"
         waveformName = "Wfm"

         powerLevel = -10.0
         ' (dBm)
         rfsgExternalAttenuation = 0.0
         ' (dB)

         rfsgFrequencyReferenceSource = RfsgFrequencyReferenceSource.OnboardClock
         rfsgFrequency = 10000000.0
         ' (Hz)

         rfsaResourceName = "RFSA"
         rfsaSelectedPorts = ""
         referenceLevel = 0.0
         ' (dBm)
         rfsaExternalAttenuation = 0.0
         ' (dB)

         rfsaFrequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
         rfsaFrequency = 10000000.0
         ' (Hz)

         enableTrigger = False
         digitalEdgeSource = RFmxNRMXConstants.PxiTriggerLine0
         digitalEdge = RFmxNRMXDigitalEdgeTriggerEdge.Rising
         triggerDelay = 0.0
         ' (s)

         frequencyRange = RFmxNRMXFrequencyRange.Range2
         carrierBandwidth = 50000000.0
         ' (Hz)
         subcarrierSpacing = 120000.0
         ' (Hz)
         band = 257
         cellID = 0

         rfsgLOOffsetMode = NIRfsgPlaybackLOOffsetMode.Auto

         timeout = 10.0
         ' (s)

         script = "script GenerateWaveform" & vbLf & "  repeat forever" & vbLf & "    generate Wfm" & vbLf & "   end repeat" & vbLf & "  end script"
      End Sub

      Private Sub ConfigureRfsg()
         rfsgSession = New NIRfsg(rfsgResourceName, True, False)
         rfsgSession.SignalPath.SelectedPorts = rfsgSelectedPorts
         rfsgSession.FrequencyReference.Configure(rfsgFrequencyReferenceSource, rfsgFrequency)
         rfsgSession.RF.Configure(centerFrequency, powerLevel)
         rfsgSession.RF.ExternalGain = -1 * rfsgExternalAttenuation
         instrumentHandle = rfsgSession.GetInstrumentHandle().DangerousGetHandle()
         NIRfsgPlayback.ReadAndDownloadWaveformFromFile(instrumentHandle, waveformFilePath, waveformName)
         NIRfsgPlayback.StoreAutomaticSGSASharedLO(instrumentHandle, "", RfsgPlaybackAutomaticSGSASharedLO.Enabled)
         NIRfsgPlayback.StoreWaveformLOOffsetMode(instrumentHandle, waveformName, NIRfsgPlaybackLOOffsetMode.Auto)
         NIRfsgPlayback.SetScriptToGenerateSingleRfsg(instrumentHandle, script)
         rfsgSession.Initiate()
      End Sub

      Private Sub ConfigureRFmxAndRetrieveResults()
         instrSession = New RFmxInstrMX(rfsaResourceName, "")
         NR = instrSession.GetNRSignalConfiguration()
         instrSession.ConfigureFrequencyReference("", rfsaFrequencyReferenceSource, rfsaFrequency)
         NR.SetSelectedPorts("", rfsaSelectedPorts)
         instrSession.SetLOSource("", RFmxInstrMXConstants.LOSourceAutomaticSGSAShared)
         NR.ConfigureRF("", centerFrequency, referenceLevel, rfsaExternalAttenuation)
         NR.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger)
         NR.SetLinkDirection("", RFmxNRMXLinkDirection.Uplink)
         NR.SetFrequencyRange("", frequencyRange)
         NR.ComponentCarrier.SetBandwidth("", carrierBandwidth)
         NR.ComponentCarrier.SetCellID("", cellID)
         NR.SetBand("", band)
         NR.ComponentCarrier.SetBandwidthPartSubcarrierSpacing("", subcarrierSpacing)
         instrSession.SetLOLeakageAvoidanceEnabled("", RFmxInstrMXLOLeakageAvoidanceEnabled.True)
         NR.SelectMeasurements("", RFmxNRMXMeasurementTypes.ModAcc, False)
         NR.Initiate("", "")

         RetrieveModAccResults()

         rfsgSession.Abort()
         NIRfsgPlayback.StoreWaveformLOOffsetMode(instrumentHandle, waveformName, rfsgLOOffsetMode)
         NIRfsgPlayback.SetScriptToGenerateSingleRfsg(instrumentHandle, script)
         rfsgSession.Initiate()

         NR.SelectMeasurements("", RFmxNRMXMeasurementTypes.Sem, False)
         NR.Initiate("", "")

         RetrieveSemResults()
      End Sub

      Private Sub RetrieveModAccResults()
         NR.ModAcc.Results.GetCompositeRmsEvmMean("", compositeRmsEvmMean)
         NR.ModAcc.Results.GetCompositePeakEvmMaximum("", compositePeakEvmMaximum)
      End Sub

      Private Sub RetrieveSemResults()
         NR.Sem.Results.FetchMeasurementStatus("", timeout, measurementStatus)
         NR.Sem.Results.ComponentCarrier.FetchMeasurement("", timeout, absolutePower, peakAbsolutePower,
            peakFrequency, relativePower)
         NR.Sem.Results.FetchLowerOffsetMarginArray("", timeout, lowerOffsetMeasurementStatus, lowerOffsetMargin,
            lowerOffsetMarginFrequency, lowerOffsetMarginAbsolutePower, lowerOffsetMarginRelativePower)
         NR.Sem.Results.FetchUpperOffsetMarginArray("", timeout, upperOffsetMeasurementStatus, upperOffsetMargin,
            upperOffsetMarginFrequency, upperOffsetMarginAbsolutePower, upperOffsetMarginRelativePower)
      End Sub

      Private Sub PrintResults()
         Console.WriteLine("------------------ModAcc------------------" & vbLf)
         Console.WriteLine("------------------Measurement------------------" & vbLf)
         Console.WriteLine("Composite RMS EVM Mean (%)                     : {0}", compositeRmsEvmMean)
         Console.WriteLine("Composite Peak EVM Maximum (%)                 : {0}", compositePeakEvmMaximum)

         Console.WriteLine(vbLf & "------------------SEM------------------" & vbLf)
         Console.WriteLine("Measurement Status                       : {0}", measurementStatus)
         Console.WriteLine("Carrier Absolute Integrated Power (dBm)  : {0}", absolutePower)
         Console.WriteLine(vbLf & "----------Lower Offset Segment Measurements----------" & vbLf)
         For i As Integer = 0 To lowerOffsetMargin.Length - 1
            Console.WriteLine("Offset {0}", i)
            Console.WriteLine("Measurement Status                 : {0}", lowerOffsetMeasurementStatus(i))
            Console.WriteLine("Margin (dB)                        : {0}", lowerOffsetMargin(i))
            Console.WriteLine("Margin Frequency (Hz)              : {0}", lowerOffsetMarginFrequency(i))
            Console.WriteLine("Margin Absolute Power (dBm)        : {0}" & vbLf, lowerOffsetMarginAbsolutePower(i))
         Next
         Console.WriteLine(vbLf & "----------Upper Offset Segment Measurements----------" & vbLf)
         For i As Integer = 0 To upperOffsetMargin.Length - 1
            Console.WriteLine("Offset {0}", i)
            Console.WriteLine("Measurement Status                 : {0}", upperOffsetMeasurementStatus(i))
            Console.WriteLine("Margin (dB)                        : {0}", upperOffsetMargin(i))
            Console.WriteLine("Margin Frequency (Hz)              : {0}", upperOffsetMarginFrequency(i))
            Console.WriteLine("Margin Absolute Power (dBm)        : {0}" & vbLf, upperOffsetMarginAbsolutePower(i))
         Next
      End Sub

      Private Sub CloseSession()
         If NR IsNot Nothing Then
            NR.Dispose()
            NR = Nothing
         End If
         If instrSession IsNot Nothing Then
            instrSession.Close()
            instrSession = Nothing
         End If
         If rfsgSession IsNot Nothing Then
            rfsgSession.Abort()
            NIRfsgPlayback.ClearWaveform(instrumentHandle, waveformName)
            rfsgSession.Close()
            rfsgSession = Nothing
         End If
      End Sub

      Private Shared Sub DisplayError(ex As Exception)
         Console.WriteLine("ERROR:" & vbLf & Convert.ToString(ex.[GetType]()) & ": " & ex.Message)
      End Sub

   End Class
End Namespace
