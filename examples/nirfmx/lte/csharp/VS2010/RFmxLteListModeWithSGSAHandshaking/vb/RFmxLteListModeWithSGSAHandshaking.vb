' Steps:
' 1. Open NI - RFSG session.
' 2. Configure RFSG Selected Ports.
' 3. Configure RFSG frequency reference.
' 4. Configure RFSG configuration settled event to the device scriptTrigger0, to make sure generation starts only after
'    RFSG configuration settled.
' 5. Configure RFSG to advance upon receipt of RFSA Ready for Advance event through PXI trigger line
' 6. Configure frequency and external gain of RF output signal. 
' 7. Get terminal name for marker0 and assign to the RFSA Reference Trigger Digital Edge source 
' 8. Create a Configuration List. Pass Power Level in the Configuration List Properties parameter to be able to configure 
'    Power Level in each step that we create. The Set As Active List parameter in this VI defaults to true, this will set the    
'    Active Configuration List property to the name of the created configuration list. Once the Active Configuration List 
'    is set, using a property node to access Power Level will modify the property for this configuration list.
' 9. Create a Configuration List Step. The Set As Active Step parameter in this VI defaults to true, this will set the Active 
'    Configuration List Step property to the created configuration list step index. Once the Active Configuration List 
'    Step is set, using a property node to access Power Level will modify the property for this configuration list step in 
'    the configuration list indicated by the Active Configuration List property.
' 10. Configure the Power Level for the Active Configuration List Step in the Active Configuration List.
' 11. Read waveform from file and download Waveform from file to RFSG.
' 12. Retrieve waveform PAPR and add the same to the RFSA reference level while configuring to every list step
' 13. Set Automatic SG SA Shared LO to Enabled.
' 14. Set LO Offset Mode to Auto while performing an in-band ModAcc measurement. This causes the RFSG LO to be 
'     placed outside the signal, if signal bandwidth is less than half of the device instantaneous bandwidth; otherwise,
'     the LO is placed at the center of the signal.
' 15. Write script to generate the waveform specified in the script. This script is programmed to generate waveform 
'     continuously and generate a marker at the start of the waveform (sample 0).
' 16. Open a new RFmx Session.
' 17. Configure the Frequency Reference properties (Clock Source and Clock Frequency).
' 18.Export RFSA Ready for Advance event to PXI trigger line. 
' 19. Create RFmx LTE List.
' 20. Create RFmx LTE List Step.
' 21. Configure the Reference Level for the Specified List Step in the LTE List.
' 22. Configure personality and measurement parameters for a List Step.
' 23  Configure Center Frequency, Selected Ports, External Attenuation, Trigger Type and Trigger Parameters for all List Steps.
' 24. Configure Link Direction, CC Spacing Type, CC bandwidth, Band, Duplex Scheme, In-Band Emission Mask Type, EVM Unit
'     and Auto DMRS Detection Enabled for all List Steps.
' 25. Set LO Leakage Avoidance Enabled to True and Automatic SG SA Shared LO to Enabled. Enabling LO Leakage Avoidance causes RFmx 
'      to place the SA LO outside the measurement bandwidth, if the measurement bandwidth is less than half of the device instantaneous 
'      bandwidth; otherwise, the LO is placed at the center of the signal.
' 26.  Select ModAcc measurement and disable traces for all List Steps.
' 27.  Initiate ModAcc measurement for RFmx LTE List.
' 28.  Initiate signal generation.
' 29.  Wait for Acquisition to complete.
' 30. Stop signal generation.
' 31. Delete the Configuration List to avoid committing the Configuration List to the hardware with subsequent 
'     calls to Commit. Deleting the list will reset the Active Configuration List.
' 32. Fetch ModAcc measurement Results for all List Steps one by one.
' 33. Delete RFmx LTE List
' 34. Close the RFmx Session.
' 35. Close the RFSG session. 
'     It is recommended to clear the waveform before closing RFSG session.
Imports System
Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.LteMX
Imports NationalInstruments.ModularInstruments.NIRfsg
Imports NationalInstruments.ModularInstruments.NIRfsgPlayback

Namespace NationalInstruments.Examples.RFmxLteListModeWithSGSAHandshaking
 Public Class RFmxLteListModeWithSGSAHandshaking
  Private instrSession As RFmxInstrMX
  Private LteList As RFmxLteMXList
  Private rfsgSession As NIRfsg
  Private instrumentHandle As IntPtr

  Private centerFrequency As Double                    

  Private rfsgResourceName As String
  Private rfsgSelectedPorts As String
  Private waveformFilePath As String
  Private waveformName As String

  Private rfsgExternalAttenuation As Double            

  Private rfsgFrequencyReferenceSource As RfsgFrequencyReferenceSource
  Private rfsgFrequency As Double                      

  Private rfsaResourceName As String
  Private rfsaSelectedPorts As String
  Private rfsaExternalAttenuation As Double            

  Private rfsaFrequencyReferenceSource As String
  Private rfsaFrequency As Double                      

  Private digitalEdge As RFmxLteMXDigitalEdgeTriggerEdge
  Private triggerDelay As Double                       

  Private numberOfSteps As Integer

  Private startReferenceLevel As Double                
  Private stopReferenceLevel As Double                 

  Private carrierBandwidth As Double                   
  Private band As Integer
  Private duplexScheme As RFmxLteMXDuplexScheme
  Private inBandEmissionMaskType As RFmxLteMXModAccInBandEmissionMaskType
  Private evmUnit As RFmxLteMXModAccEvmUnit

  Private rampPattern As Double()
  Private timeout As Double                            
  Private script As String
  Private papr As Double
  Private [step] As RFmxLteMX()
  Private configurationSettledEvenTerminalName As String
  Private markerEventTerminalName As String

  Private meanRmsCompositeEvm As Double()                                ' (% or dB)
  Private maximumPeakCompositeEvm As Double()                            ' (% or dB)

  Public Sub Run()
   Try
    InitializeVariables()
    ConfigureRfsg()
    ConfigureRFmx()
    RetrieveResults()
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
   centerFrequency = 1950000000.0                                        ' (Hz) 

   rfsgResourceName = "RFSG"
   rfsgSelectedPorts = ""
   waveformFilePath = "LTE_UL_FDD_CC-1_BW-10MHz_PUSCH-QPSK.tdms"
   waveformName = "Wfm"

   rfsgExternalAttenuation = 0.0                                         ' (dB) 

   rfsgFrequencyReferenceSource = RfsgFrequencyReferenceSource.OnboardClock
   rfsgFrequency = 10000000.0                                            ' (Hz) 

   rfsaResourceName = "RFSA"
   rfsaSelectedPorts = ""
   rfsaExternalAttenuation = 0.0                                         ' (dB) 

   rfsaFrequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
   rfsaFrequency = 10000000.0                                            ' (Hz) 

   digitalEdge = RFmxLteMXDigitalEdgeTriggerEdge.Rising
   triggerDelay = 0.0                                                    ' (s) 

   numberOfSteps = 10

   startReferenceLevel = -20.0                                           ' (dBm) 
   stopReferenceLevel = 0.0                                              ' (dBm) 

   carrierBandwidth = 10000000.0                                         ' (Hz) 

   band = 1
   duplexScheme = RFmxLteMXDuplexScheme.Fdd
   evmUnit = RFmxLteMXModAccEvmUnit.Percentage
   rampPattern = New Double(numberOfSteps - 1) {}
   LinearRampPattern(startReferenceLevel, stopReferenceLevel, numberOfSteps, False, rampPattern)

   timeout = 10.0                                                        ' (s) 

   script = String.Format("script GenerateWaveform" & vbLf & "  repeat forever" & vbLf & " generate
      {0} marker0(0)" & vbLf & "  wait until scripttrigger0" & vbLf & "   end repeat" & vbLf & "  end script", waveformName)

   [step] = New RFmxLteMX(numberOfSteps - 1) {}
   meanRmsCompositeEvm = New Double(numberOfSteps - 1) {}
   maximumPeakCompositeEvm = New Double(numberOfSteps - 1) {}
  End Sub

  Private Sub ConfigureRfsg()
   rfsgSession = New NIRfsg(rfsgResourceName, True, False)
   rfsgSession.SignalPath.SelectedPorts = rfsgSelectedPorts
   rfsgSession.FrequencyReference.Configure(rfsgFrequencyReferenceSource, rfsgFrequency)
   configurationSettledEvenTerminalName = rfsgSession.DeviceEvents.ConfigurationSettledEvent.TerminalName
   rfsgSession.Triggers.ScriptTriggers(0).DigitalEdge.Configure(configurationSettledEvenTerminalName, RfsgTriggerEdge.RisingEdge)
   rfsgSession.Triggers.ConfigurationListStepTrigger.DigitalEdge.Configure(
    RfsgDigitalEdgeConfigurationListStepTriggerSource.PxiTriggerLine0, RfsgTriggerEdge.RisingEdge)
   rfsgSession.RF.ExternalGain = -1 * rfsgExternalAttenuation
   rfsgSession.RF.Frequency = centerFrequency
   markerEventTerminalName = rfsgSession.DeviceEvents.MarkerEvents(0).TerminalName
   Dim properties As RfsgConfigurationListProperties() = New RfsgConfigurationListProperties(0) {RfsgConfigurationListProperties.PowerLevel}
   rfsgSession.BasicConfigurationList.CreateConfigurationList("PowerLevelList", properties, True)
   For i As Integer = 0 To numberOfSteps - 1
    rfsgSession.BasicConfigurationList.CreateStep(True)
    rfsgSession.RF.PowerLevel = rampPattern(i)
   Next
   instrumentHandle = rfsgSession.GetInstrumentHandle().DangerousGetHandle()
   NIRfsgPlayback.ReadAndDownloadWaveformFromFile(instrumentHandle, waveformFilePath, waveformName)
   NIRfsgPlayback.RetrieveWaveformPapr(instrumentHandle, waveformName, papr)
   NIRfsgPlayback.StoreAutomaticSGSASharedLO(instrumentHandle, "", RfsgPlaybackAutomaticSGSASharedLO.Enabled)
   NIRfsgPlayback.StoreWaveformLOOffsetMode(instrumentHandle, waveformName, NIRfsgPlaybackLOOffsetMode.Auto)
   NIRfsgPlayback.SetScriptToGenerateSingleRfsg(instrumentHandle, script)
  End Sub

  Private Sub ConfigureRFmx()
   instrSession = New RFmxInstrMX(rfsaResourceName, "")
   instrSession.ConfigureFrequencyReference("", rfsaFrequencyReferenceSource, rfsaFrequency)
   instrSession.ExportSignal(RFmxInstrMXExportSignalSource.ReadyForAdvanceEvent,
                             RFmxInstrMXConstants.PxiTriggerLine0)
   LteList = instrSession.GetLteList("ModAcc_List")
   For i As Integer = 0 To numberOfSteps - 1
    [step](i) = LteList.CreateListStep()
    [step](i).SetReferenceLevel("", papr + rampPattern(i))
   Next
   Dim stepAll As RFmxLteMX = LteList.GetListStepAll()
   stepAll.ConfigureFrequency("", centerFrequency)
   stepAll.SetSelectedPorts("", rfsaSelectedPorts)
   stepAll.ConfigureDigitalEdgeTrigger("", markerEventTerminalName, digitalEdge, triggerDelay, True)
   stepAll.ConfigureExternalAttenuation("", rfsaExternalAttenuation)

   stepAll.SetLinkDirection("", RFmxLteMXLinkDirection.Uplink)
   stepAll.ComponentCarrier.ConfigureSpacing("", RFmxLteMXComponentCarrierSpacingType.Nominal, 0)
   stepAll.ComponentCarrier.SetBandwidth("", carrierBandwidth)
   stepAll.SetBand("", band)
   stepAll.SetDuplexScheme("", duplexScheme)
   stepAll.ModAcc.Configuration.ConfigureInBandEmissionMaskType("", inBandEmissionMaskType)
   stepAll.ModAcc.Configuration.ConfigureEvmUnit("", evmUnit)
   stepAll.ConfigureAutoDmrsDetectionEnabled("", RFmxLteMXAutoDmrsDetectionEnabled.True)

   instrSession.SetLOSource("", RFmxInstrMXConstants.LOSourceAutomaticSGSAShared)
   instrSession.SetLOLeakageAvoidanceEnabled("", RFmxInstrMXLOLeakageAvoidanceEnabled.True)

   stepAll.SelectMeasurements("", RFmxLteMXMeasurementTypes.ModAcc, False)
   LteList.Initiate("", "")
   rfsgSession.Initiate()
   instrSession.WaitForAcquisitionComplete(timeout)
  End Sub

  Private Sub RetrieveResults()
   For i As Integer = 0 To numberOfSteps - 1
    [step](i).ModAcc.Results.GetMeanRmsCompositeEvm("", meanRmsCompositeEvm(i))
    [step](i).ModAcc.Results.GetMaximumPeakCompositeEvm("", maximumPeakCompositeEvm(i))
   Next
  End Sub

  Private Sub PrintResults()
   Console.WriteLine("------------------Measurements------------------" & vbLf)
   Console.WriteLine("Mean RMS Composite EVM (% or dB)")
   For i As Integer = 0 To numberOfSteps - 1
    Console.WriteLine("Step{0}    : {1}", i, meanRmsCompositeEvm(i))
   Next
   Console.WriteLine(vbLf & "Maximum Peak Composite EVM (% or dB)")
   For i As Integer = 0 To numberOfSteps - 1
    Console.WriteLine("Step{0}    : {1}", i, maximumPeakCompositeEvm(i))
   Next
  End Sub

  Private Sub CloseSession()
   If LteList IsNot Nothing Then
    LteList.Dispose()
    LteList = Nothing
   End If
   If instrSession IsNot Nothing Then
    instrSession.Close()
    instrSession = Nothing
   End If
   If rfsgSession IsNot Nothing Then
    rfsgSession.Abort()
    rfsgSession.BasicConfigurationList.DeleteConfigurationList("PowerLevelList")
    NIRfsgPlayback.ClearWaveform(instrumentHandle, waveformName)
    rfsgSession.Close()
    rfsgSession = Nothing
   End If
  End Sub

  Private Shared Sub DisplayError(ex As Exception)
   Console.WriteLine("ERROR:" & vbLf & Convert.ToString(ex.[GetType]()) & ": " & ex.Message)
  End Sub

  Private Sub LinearRampPattern(start As Double, [end] As Double, samples As Integer, includeEnd As Boolean, ByRef rampPattern As Double())
   Dim m As Integer = If(includeEnd, samples, (samples - 1))
   Dim delta As Double = ([end] - start) / m
   For i As Integer = 0 To samples - 1
    rampPattern(i) = start + (i * delta)
   Next
  End Sub

 End Class
End Namespace
