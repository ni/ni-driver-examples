'Steps:
'1.  Open a new RFmx Session.
'2.  Configure Frequency Reference.
'3.  Configure LO Source to Automatic SG SA Shared.
'4.  Configure Selected Ports.
'5.  Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
'6.  Configure Trigger Type and Trigger Parameters.
'7.  Configure Frequency Range, CC bandwidth, Cell ID, Band, BWP Subcarrier Spacing and Auto RB Detection Enabled.
'    Setting Auto RB Detection Enabled to False reduces the measurement time.
'8.  Configure PUSCH and PUSCH RB Allocation.
'9.  Configure PUSCH DMRS.
'10. Select ModAcc measurement and disable Traces.
'11. Configure Synchronization Mode and Averaging Parameters for ModAcc measurement.
'12. Configure Measurement Interval.
'13. Set ModAcc Magnitude and Phase Error Enabled and IQ Mismatch Estimation Enabled to False. This disables computation
'    of the corresponding results. Configure ModAcc Frequency Error Estimation, Symbol Clock Error Estimation Enabled,
'    Phase Tracking Mode, Timing Tracking Mode, and IQ Origin Offset Estimation Enabled. Set these attributes to
'    False/Disabled to reduce the measurement time. This disables estimation and, in turn, correction of the corresponding impairments.
'    You may disable estimation of an impairment only if it is not present in the signal to be measured.
'14. Configure EVM Reference Data Sympbol Mode.
'15. Configure Reference Waveform if EVM Reference Data Symbol Mode is set as Reference Waveform.
'16. Initiate the Measurement.
'17. Fetch ModAcc Measurements.
'18. Close RFmx Session.

Imports NationalInstruments
Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.NRMX
Imports NationalInstruments.ModularInstruments.NIRfsgPlayback

Namespace NationalInstruments.Examples.RFmxNRULModAccSpeedOptimized
  Public Class RFmxNRULModAccSpeedOptimized
    Private instrSession As RFmxInstrMX
    Private NR As RFmxNRMX
    Private resourceName As String

    Private frequencyReferenceSource As String
    Private frequencyReferenceFrequency As Double

    Private selectedPorts As String
    Private centerFrequency As Double
    Private referenceLevel As Double
    Private externalAttenuation As Double

    Private enableTrigger As Boolean
    Private digitalEdgeSource As String
    Private digitalEdge As RFmxNRMXDigitalEdgeTriggerEdge
    Private triggerDelay As Double

    Private frequencyRange As RFmxNRMXFrequencyRange
    Private band As Integer
    Private cellID As Integer
    Private carrierBandwidth As Double
    Private subcarrierSpacing As Double
    Private autoResourceBlockDetectionEnabled As RFmxNRMXAutoResourceBlockDetectionEnabled

    Private puschTransformPrecodingEnabled As RFmxNRMXPuschTransformPrecodingEnabled
    Private puschModulationType As RFmxNRMXPuschModulationType
    Const NumberOfResourceBlockClusters As Integer = 1
    Private puschResourceBlockOffset As Integer() = New Integer(NumberOfResourceBlockClusters - 1) {}
    Private puschNumberOfResourceBlocks As Integer() = New Integer(NumberOfResourceBlockClusters - 1) {}
    Private puschSlotAllocation As String
    Private puschSymbolAllocation As String

    Private puschDmrsPowerMode As RFmxNRMXPuschDmrsPowerMode
    Private puschDmrsPower As Double
    Private puschDmrsConfigurationType As RFmxNRMXPuschDmrsConfigurationType
    Private puschMappingType As RFmxNRMXPuschMappingType
    Private puschDmrsTypeAPosition As Integer
    Private puschDmrsDuration As RFmxNRMXPuschDmrsDuration
    Private puschDmrsAdditionalPositions As Integer

    Private synchronizationMode As RFmxNRMXModAccSynchronizationMode

    Private measurementLengthUnit As RFmxNRMXModAccMeasurementLengthUnit
    Private measurementOffset As Double
    Private measurementLength As Double

    Private averagingEnabled As RFmxNRMXModAccAveragingEnabled
    Private averagingCount As Integer

    Private frequencyErrorEstimation As RFmxNRMXModAccFrequencyErrorEstimation
    Private symbolClockErrorEstimationEnabled As RFmxNRMXModAccSymbolClockErrorEstimationEnabled
    Private phaseTrackingMode As RFmxNRMXModAccPhaseTrackingMode
    Private timingTrackingMode As RFmxNRMXModAccTimingTrackingMode
    Private IQOriginOffsetEstimationEnabled As RFmxNRMXModAccIQOriginOffsetEstimationEnabled

    Private evmReferenceDataSymbolsMode As RFmxNRMXModAccEvmReferenceDataSymbolsMode
    Private waveformFileName As String

    Private subblockString As String
    Private carrierString As String
    Private bandwidthPartString As String
    Private userString As String
    Private puschString As String
    Private puschClusterString As String
    Private referenceWaveformSingle As ComplexWaveform(Of ComplexSingle)

    Private compositeRmsEvmMean As Double
    ' (%)
    Private inBandEmissionMargin As Double
    ' (dB)

    Public Sub Run()
      Try
        InitializeVariables()
        InitializeInstr()
        ConfigureNR()
        RetrieveResults()
        PrintResults()
      Catch ex As Exception
        DisplayError(ex)
      Finally
        ' Close session

        CloseSession()
        Console.WriteLine("Press any key to exit")
        Console.ReadKey()
      End Try
    End Sub

    Private Sub InitializeVariables()
      resourceName = "RFSA"

      frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
      frequencyReferenceFrequency = 10000000.0
      ' (Hz)

      selectedPorts = ""
      centerFrequency = 3500000000.0
      ' (Hz)
      referenceLevel = 0.0
      ' (dBm)
      externalAttenuation = 0.0
      ' (dB)

      enableTrigger = True
      digitalEdgeSource = RFmxNRMXConstants.PxiTriggerLine0
      digitalEdge = RFmxNRMXDigitalEdgeTriggerEdge.Rising
      triggerDelay = 0.0
      ' (s)

      frequencyRange = RFmxNRMXFrequencyRange.Range1
      band = 78
      cellID = 0
      carrierBandwidth = 100000000.0
      ' (Hz)
      subcarrierSpacing = 30000.0
      ' (Hz)
      autoResourceBlockDetectionEnabled = RFmxNRMXAutoResourceBlockDetectionEnabled.[False]

      puschTransformPrecodingEnabled = RFmxNRMXPuschTransformPrecodingEnabled.[False]
      puschModulationType = RFmxNRMXPuschModulationType.Qpsk
      puschResourceBlockOffset(0) = 0
      puschNumberOfResourceBlocks(0) = -1
      puschSlotAllocation = "0-Last"
      puschSymbolAllocation = "0-Last"

      puschDmrsPowerMode = RFmxNRMXPuschDmrsPowerMode.CdmGroups
      puschDmrsPower = 0.0
      ' (dB)
      puschDmrsConfigurationType = RFmxNRMXPuschDmrsConfigurationType.Type1
      puschMappingType = RFmxNRMXPuschMappingType.TypeA
      puschDmrsTypeAPosition = 2
      puschDmrsDuration = RFmxNRMXPuschDmrsDuration.SingleSymbol
      puschDmrsAdditionalPositions = 0

      synchronizationMode = RFmxNRMXModAccSynchronizationMode.Frame

      measurementLengthUnit = RFmxNRMXModAccMeasurementLengthUnit.Slot
      measurementOffset = 0.0
      measurementLength = 1

      averagingEnabled = RFmxNRMXModAccAveragingEnabled.[False]
      averagingCount = 10

      frequencyErrorEstimation = RFmxNRMXModAccFrequencyErrorEstimation.Disabled
      symbolClockErrorEstimationEnabled = RFmxNRMXModAccSymbolClockErrorEstimationEnabled.[False]
      phaseTrackingMode = RFmxNRMXModAccPhaseTrackingMode.Disabled
      timingTrackingMode = RFmxNRMXModAccTimingTrackingMode.Disabled
      IQOriginOffsetEstimationEnabled = RFmxNRMXModAccIQOriginOffsetEstimationEnabled.[False]
      evmReferenceDataSymbolsMode = RFmxNRMXModAccEvmReferenceDataSymbolsMode.AcquiredWaveform
      waveformFileName = ""
    End Sub

    Private Sub InitializeInstr()
      instrSession = New RFmxInstrMX(resourceName, "")
    End Sub

    Private Sub ConfigureNR()
      NR = instrSession.GetNRSignalConfiguration()
      ' Create a new RFmx Session
      instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
      instrSession.SetLOSource("", RFmxInstrMXConstants.LOSourceAutomaticSGSAShared)
      NR.SetSelectedPorts("", selectedPorts)
      NR.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
      NR.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger)

      NR.SetFrequencyRange("", frequencyRange)
      NR.ComponentCarrier.SetBandwidth("", carrierBandwidth)
      NR.ComponentCarrier.SetCellID("", cellID)
      NR.SetBand("", band)
      NR.ComponentCarrier.SetBandwidthPartSubcarrierSpacing("", subcarrierSpacing)
      NR.SetAutoResourceBlockDetectionEnabled("", autoResourceBlockDetectionEnabled)

      NR.ComponentCarrier.SetPuschTransformPrecodingEnabled("", puschTransformPrecodingEnabled)
      NR.ComponentCarrier.SetPuschSlotAllocation("", puschSlotAllocation)
      NR.ComponentCarrier.SetPuschSymbolAllocation("", puschSymbolAllocation)
      NR.ComponentCarrier.SetPuschModulationType("", puschModulationType)

      NR.ComponentCarrier.SetPuschNumberOfResourceBlockClusters("", NumberOfResourceBlockClusters)

      subblockString = RFmxNRMX.BuildSubblockString("", 0)
      carrierString = RFmxNRMX.BuildCarrierString(subblockString, 0)
      bandwidthPartString = RFmxNRMX.BuildBandwidthPartString(carrierString, 0)
      userString = RFmxNRMX.BuildUserString(bandwidthPartString, 0)
      puschString = RFmxNRMX.BuildPuschString(userString, 0)
      For i As Integer = 0 To NumberOfResourceBlockClusters - 1
        puschClusterString = RFmxNRMX.BuildPuschClusterString(puschString, i)
        NR.ComponentCarrier.SetPuschResourceBlockOffset(puschClusterString, puschResourceBlockOffset(i))
        NR.ComponentCarrier.SetPuschNumberOfResourceBlocks(puschClusterString, puschNumberOfResourceBlocks(i))
      Next

      NR.ComponentCarrier.SetPuschDmrsPowerMode("", puschDmrsPowerMode)
      NR.ComponentCarrier.SetPuschDmrsPower("", puschDmrsPower)
      NR.ComponentCarrier.SetPuschDmrsConfigurationType("", puschDmrsConfigurationType)
      NR.ComponentCarrier.SetPuschMappingType("", puschMappingType)
      NR.ComponentCarrier.SetPuschDmrsTypeAPosition("", puschDmrsTypeAPosition)
      NR.ComponentCarrier.SetPuschDmrsDuration("", puschDmrsDuration)
      NR.ComponentCarrier.SetPuschDmrsAdditionalPositions("", puschDmrsAdditionalPositions)

      NR.SelectMeasurements("", RFmxNRMXMeasurementTypes.ModAcc, False)

      NR.ModAcc.Configuration.SetSynchronizationMode("", synchronizationMode)
      NR.ModAcc.Configuration.SetAveragingEnabled("", averagingEnabled)
      NR.ModAcc.Configuration.SetAveragingCount("", averagingCount)

      NR.ModAcc.Configuration.SetMeasurementLengthUnit("", measurementLengthUnit)
      NR.ModAcc.Configuration.SetMeasurementOffset("", measurementOffset)
      NR.ModAcc.Configuration.SetMeasurementLength("", measurementLength)

      NR.ModAcc.Configuration.SetMagnitudeAndPhaseErrorEnabled("", RFmxNRMXModAccMagnitudeAndPhaseErrorEnabled.[False])
      NR.ModAcc.Configuration.SetIQMismatchEstimationEnabled("", RFmxNRMXModAccIQMismatchEstimationEnabled.[False])
      NR.ModAcc.Configuration.SetFrequencyErrorEstimation("", frequencyErrorEstimation)
      NR.ModAcc.Configuration.SetSymbolClockErrorEstimationEnabled("", symbolClockErrorEstimationEnabled)
      NR.ModAcc.Configuration.SetPhaseTrackingMode("", phaseTrackingMode)
      NR.ModAcc.Configuration.SetTimingTrackingMode("", timingTrackingMode)
      NR.ModAcc.Configuration.SetIQOriginOffsetEstimationEnabled("", IQOriginOffsetEstimationEnabled)

      NR.ModAcc.Configuration.SetEvmReferenceDataSymbolsMode("", evmReferenceDataSymbolsMode)
      If evmReferenceDataSymbolsMode = RFmxNRMXModAccEvmReferenceDataSymbolsMode.ReferenceWaveform Then
        NIRfsgPlayback.ReadWaveformFromFileByIndexComplex(waveformFileName, 0, referenceWaveformSingle)
        NR.ModAcc.Configuration.ConfigureReferenceWaveform("", referenceWaveformSingle)
      End If

      NR.Initiate("", "")
    End Sub

    Private Sub RetrieveResults()
      NR.ModAcc.Results.GetCompositeRmsEvmMean("", compositeRmsEvmMean)
      NR.ModAcc.Results.GetInBandEmissionMargin("", inBandEmissionMargin)
    End Sub

    Private Sub PrintResults()
      Console.WriteLine("------------------Measurement------------------" & vbLf)
      Console.WriteLine("Composite RMS EVM Mean (%)                     : {0}", compositeRmsEvmMean)
      Console.WriteLine("In-Band Emission Margin (dB)                   : {0}" & vbLf, inBandEmissionMargin)
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
    End Sub

    Private Shared Sub DisplayError(ByVal ex As Exception)
      Console.WriteLine("ERROR:" & vbLf & Convert.ToString(ex.[GetType]()) & ": " & ex.Message)
    End Sub
  End Class
End Namespace

