'Steps:
' 1. Open NI-RFSG sessions. 
' 2. Configure Reference Clock Source, Frequency, Power Level, Power Level Type and External Gain. 
' 3.  Export marker0 event to the specified output terminal  
' 4.  Read the waveforms from the tdms file and write it to RFSG memory.
' 5. Set waveform generation mode to Script.
' 6. Set the Script to be used for generation. 
' 7. Retrieve the session reference. This is used by the NI-TClk VIs.
' 8. Configure the devices for homogeneous triggers
' 9. Synchronize the generators
' 10. Initiate generation
' 11. Open a new RFmx Session.
' 12. Configure Frequency Reference.
' 13. Configure Number of Receive Chains and Center Frequency.
' 14. Configure Selected Ports and Signal Analyser properties ( Reference Level and External Attenuation).
' 15. Configure Trigger Type and Trigger Parameters.
' 16. Configure Frequency Range, CC bandwidth, Cell ID, Band, BWP Subcarrier Spacing and Auto RB Detection Enabled
' 17. Configure PUSCH and PUSCH RB Allocation.
' 18. Configure PUSCH DMRS.
' 19. Select ModAcc measurement and enable Traces.
' 20. Configure the interval used for Pre-FFT error estimation, settings for the post-FFT tracking, Synchronization Mode and Averaging Parameters for the ModAcc measurement.
' 21. Configure Measurement Interval.
' 22. Initiate the Measurement.
' 23. Fetch ModAcc Measurements and Traces.
' 24. Close RFmx Session. 
' 25. Abort signal generation.
' 26. Disable the output. This sets the noise floor as low as possible.
' 27. Call NI-RFSG Commit.
' 28. Clear the waveforms and waveform properties from the device memory. 
' 29. Close the NI-RFSG sessions.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.NRMX
Imports NationalInstruments.ModularInstruments.NIRfsg
Imports NationalInstruments.ModularInstruments.SystemServices.TimingServices
Imports NationalInstruments.ModularInstruments

Namespace NationalInstruments.Examples.RFmxNRULModAccSingleCarrier2x2MIMO
    Public Class RFmxNRULModAccSingleCarrier2x2MIMO
        Private instrSession As RFmxInstrMX
        Private NR As RFmxNRMX
        Private rfsgSessions As NIRfsg()
        Private tClockSession As TClock
        Private rfsgSynchronizableDevices As ITClockSynchronizableDevice()

        Private rfsgResourceNames As String()
        Private rfsaResourceNames As String()
        Private numberOfDevices As Integer

        Private selectedPorts As String()
        Const numberOfReceiveChains As Integer = 2

        Private layerString As String
        Private chainString As String

        Private centerFrequency As Double
        Private referenceLevel As Double
        Private rfsaExternalAttenuation As Double
        Private rfsgExternalAttenuation As Double
        Private powerLevel As Double
        Private markerNumber As Integer

        Private portString As String()

        Private selectedPortsString As String()

        Private frequencyReferenceSource As String
        Private rfsgReferenceClockSource As String
        Private frequencyReferenceFrequency As Double
        Private PxiTriggerLines As String()

        Private waveformName As String
        Private waveformFile As String
        Private scriptName As String
        Private waveformScript As String
        Private waveformIndex As UInteger

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
        Private puschDmrsNumberOfCdmGroups As Integer
        Private puschDmrsAntennaPorts As String

        Private synchronizationMode As RFmxNRMXModAccSynchronizationMode

        Private measurementLengthUnit As RFmxNRMXModAccMeasurementLengthUnit
        Private measurementOffset As Double
        Private measurementLength As Double

        Private averagingEnabled As RFmxNRMXModAccAveragingEnabled
        Private averagingCount As Integer
        Private subblockString As String
        Private carrierString As String
        Private bandwidthPartString As String
        Private userString As String
        Private puschString As String
        Private puschClusterString As String

        Private timeout As Double

        Private compositeRmsEvmMean As Double() = New Double(numberOfReceiveChains - 1) {}
        ' (%) 
        Private compositePeakEvmMaximum As Double() = New Double(numberOfReceiveChains - 1) {}
        ' (%) 
        Private compositePeakEvmSlotIndex As Integer() = New Integer(numberOfReceiveChains - 1) {}
        Private compositePeakEvmSymbolIndex As Integer() = New Integer(numberOfReceiveChains - 1) {}
        Private compositePeakEvmSubcarrierIndex As Integer() = New Integer(numberOfReceiveChains - 1) {}

        Private componentCarrierFrequencyErrorMean As Double() = New Double(numberOfReceiveChains - 1) {}
        ' (Hz) 
        Private componentCarrierIQOriginOffsetMean As Double() = New Double(numberOfReceiveChains - 1) {}
        ' (dBc) 
        Private componentCarrierTimingOffsetMean As Double() = New Double(numberOfReceiveChains - 1) {}
        ' (dB) 
        Private componentCarrierSymbolClockErrorMean As Double() = New Double(numberOfReceiveChains - 1) {}
        ' (deg) 
        Private inBandEmissionMargin As Double() = New Double(numberOfReceiveChains - 1) {}
        ' (dB) 

        Private puschDataConstellation As ComplexSingle()() = New ComplexSingle(numberOfReceiveChains - 1)() {}
        Private puschDmrsConstellation As ComplexSingle()() = New ComplexSingle(numberOfReceiveChains - 1)() {}

        Private rmsEvmPerSubcarrierMean As AnalogWaveform(Of Single)() = New AnalogWaveform(Of Single)(numberOfReceiveChains - 1) {}
        Private rmsEvmPerSymbolMean As AnalogWaveform(Of Single)() = New AnalogWaveform(Of Single)(numberOfReceiveChains - 1) {}

        Private spectralFlatness As Spectrum(Of Single)() = New Spectrum(Of Single)(numberOfReceiveChains - 1) {}
        Private spectralFlatnessLowerMask As Spectrum(Of Single)() = New Spectrum(Of Single)(numberOfReceiveChains - 1) {}
        Private spectralFlatnessUpperMask As Spectrum(Of Single)() = New Spectrum(Of Single)(numberOfReceiveChains - 1) {}

        Public Sub Run()
            Try
                InitializeVariables()
                InitializeInstr()
                ConfigureRfsg()
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
            rfsgResourceNames = New String() {"RFSG1", "RFSG2"}
            rfsaResourceNames = New String() {"RFSA1", "RFSA2"}
            numberOfDevices = rfsaResourceNames.GetLength(0)
            rfsgSynchronizableDevices = New ITClockSynchronizableDevice(numberOfDevices - 1) {}

            selectedPorts = New String() {"", ""}

            centerFrequency = 3500000000.0
            ' (Hz) 
            referenceLevel = 0.0
            ' (dBm) 
            rfsaExternalAttenuation = 0.0
            ' (dB) 
            rfsgExternalAttenuation = 0.0
            ' (dB) 
            powerLevel = -10.0
            ' (dBm) 
            markerNumber = 0

            portString = New String(numberOfDevices - 1) {}

            selectedPortsString = New String(numberOfDevices - 1) {}

            frequencyReferenceSource = RFmxInstrMXConstants.PxiClock
            rfsgReferenceClockSource = RfsgFrequencyReferenceSource.PxiClock
            frequencyReferenceFrequency = 10000000.0
            ' (Hz) 
            PxiTriggerLines = New String() {RfsgMarkerEventExportedOutputTerminal.PxiTriggerLine0, RfsgMarkerEventExportedOutputTerminal.PxiTriggerLine1}

            scriptName = "GenerateWfm"
            waveformName = "Wfm"
            waveformFile = "NR_FR1_UL_MIMO_BW-100MHz_SCS-30kHz_Ports-01_SF-1ms.tdms"

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
            autoResourceBlockDetectionEnabled = RFmxNRMXAutoResourceBlockDetectionEnabled.[True]

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
            puschDmrsNumberOfCdmGroups = 1
            puschDmrsAntennaPorts = "0,1"

            synchronizationMode = RFmxNRMXModAccSynchronizationMode.Slot

            measurementLengthUnit = RFmxNRMXModAccMeasurementLengthUnit.Slot
            measurementOffset = 0.0
            measurementLength = 1

            averagingEnabled = RFmxNRMXModAccAveragingEnabled.[False]
            averagingCount = 10

            timeout = 10.0
            ' (s) 
        End Sub

        Private Sub InitializeInstr()
            instrSession = New RFmxInstrMX(rfsaResourceNames, "")
        End Sub
        Private Sub ConfigureRfsg()
            rfsgSessions = New NIRfsg(numberOfDevices - 1) {}
            For i As Integer = 0 To numberOfDevices - 1
                rfsgSessions(i) = New NIRfsg(rfsgResourceNames(i), False, True)
                rfsgSynchronizableDevices(i) = DirectCast(rfsgSessions(i), ITClockSynchronizableDevice)
                rfsgSessions(i).FrequencyReference.Configure(rfsgReferenceClockSource, frequencyReferenceFrequency)
                rfsgSessions(i).RF.Configure(centerFrequency, powerLevel)
                rfsgSessions(i).RF.PowerLevelType = RfsgRFPowerLevelType.PeakPower
                rfsgSessions(i).RF.ExternalGain = -rfsgExternalAttenuation
                'instrumentHandles[i] = rfsgSessions[i].GetInstrumentHandle().DangerousGetHandle();
                rfsgSessions(i).DeviceEvents.MarkerEvents(markerNumber).ExportedOutputTerminal = PxiTriggerLines(i)
                waveformIndex = CUInt(i)
                rfsgSessions(i).Arb.ReadAndDownloadWaveformFromFileTdms(waveformName, waveformFile, waveformIndex)
                waveformScript = [String].Format("script {0}{1}repeat forever{1}generate {2} marker{3}(0){1}end repeat{1}end script", scriptName, Environment.NewLine, waveformName, markerNumber)
                rfsgSessions(i).RF.ExternalGain = -rfsgExternalAttenuation
                rfsgSessions(i).Arb.GenerationMode = RfsgWaveformGenerationMode.Script
                rfsgSessions(i).Arb.Scripting.WriteScript(waveformScript)
            Next
            tClockSession = New TClock(rfsgSynchronizableDevices)
            tClockSession.ConfigureForHomogeneousTriggers()
            tClockSession.Synchronize()
            tClockSession.Initiate()
        End Sub
        Private Sub ConfigureNR()
            NR = instrSession.GetNRSignalConfiguration()
            ' Create a new RFmx Session 
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
            NR.SetNumberOfReceiveChains("", numberOfReceiveChains)
            NR.SetCenterFrequency("", centerFrequency)
            For i As Integer = 0 To numberOfReceiveChains - 1
                selectedPortsString(i) = RFmxInstrMX.BuildPortString2("", selectedPorts(i), rfsaResourceNames(i), 0)
                portString(i) = RFmxInstrMX.BuildPortString2("", "", rfsaResourceNames(i), 0)
                NR.ConfigureReferenceLevel(portString(i), referenceLevel)
                NR.ConfigureExternalAttenuation(portString(i), rfsaExternalAttenuation)
            Next
            NR.ConfigureSelectedPortsMultiple("", selectedPortsString)
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
            NR.ComponentCarrier.SetPuschDmrsAntennaPorts("", puschDmrsAntennaPorts)
            NR.ComponentCarrier.SetPuschDmrsNumberOfCdmGroups("", puschDmrsNumberOfCdmGroups)

            NR.SelectMeasurements("", RFmxNRMXMeasurementTypes.ModAcc, True)

            NR.ModAcc.Configuration.SetPreFftErrorEstimationInterval("", RFmxNRMXModAccPreFftErrorEstimationInterval.Slot)
            NR.ModAcc.Configuration.SetPhaseTrackingMode("", RFmxNRMXModAccPhaseTrackingMode.Disabled)
            NR.ModAcc.Configuration.SetTimingTrackingMode("", RFmxNRMXModAccTimingTrackingMode.Disabled)
            NR.ModAcc.Configuration.SetSynchronizationMode("", synchronizationMode)
            NR.ModAcc.Configuration.SetAveragingEnabled("", averagingEnabled)
            NR.ModAcc.Configuration.SetAveragingCount("", averagingCount)

            NR.ModAcc.Configuration.SetMeasurementLengthUnit("", measurementLengthUnit)
            NR.ModAcc.Configuration.SetMeasurementOffset("", measurementOffset)
            NR.ModAcc.Configuration.SetMeasurementLength("", measurementLength)

            NR.Initiate("", "")
        End Sub

        Private Sub RetrieveResults()
            For i As Integer = 0 To numberOfReceiveChains - 1
                layerString = RFmxNRMX.BuildLayerString("subblock0/carrier0", i)
                chainString = RFmxNRMX.BuildChainString("subblock0/carrier0", i)

                NR.ModAcc.Results.GetCompositeRmsEvmMean(layerString, compositeRmsEvmMean(i))
                NR.ModAcc.Results.GetCompositePeakEvmMaximum(layerString, compositePeakEvmMaximum(i))
                NR.ModAcc.Results.GetCompositePeakEvmSlotIndex(layerString, compositePeakEvmSlotIndex(i))
                NR.ModAcc.Results.GetCompositePeakEvmSymbolIndex(layerString, compositePeakEvmSymbolIndex(i))
                NR.ModAcc.Results.GetCompositePeakEvmSubcarrierIndex(layerString, compositePeakEvmSubcarrierIndex(i))
                NR.ModAcc.Results.GetComponentCarrierFrequencyErrorMean(layerString, componentCarrierFrequencyErrorMean(i))
                NR.ModAcc.Results.GetComponentCarrierIQOriginOffsetMean(layerString, componentCarrierIQOriginOffsetMean(i))
                NR.ModAcc.Results.GetComponentCarrierTimeOffsetMean(chainString, componentCarrierTimingOffsetMean(i))
                NR.ModAcc.Results.GetComponentCarrierSymbolClockErrorMean(chainString, componentCarrierSymbolClockErrorMean(i))
                NR.ModAcc.Results.GetInBandEmissionMargin(chainString, inBandEmissionMargin(i))
                NR.ModAcc.Results.FetchPuschDataConstellationTrace(layerString, timeout, puschDataConstellation(i))
                NR.ModAcc.Results.FetchPuschDmrsConstellationTrace(layerString, timeout, puschDmrsConstellation(i))
                NR.ModAcc.Results.FetchRmsEvmPerSubcarrierMeanTrace(layerString, timeout, rmsEvmPerSubcarrierMean(i))
                NR.ModAcc.Results.FetchRmsEvmPerSymbolMeanTrace(layerString, timeout, rmsEvmPerSymbolMean(i))
                NR.ModAcc.Results.FetchSpectralFlatnessTrace(layerString, timeout, spectralFlatness(i), spectralFlatnessLowerMask(i), spectralFlatnessUpperMask(i))
            Next

        End Sub

        Private Sub PrintResults()
            Console.WriteLine("------------------Measurement------------------" & vbLf)
            For i As Integer = 0 To numberOfReceiveChains - 1
                Console.WriteLine("Layer  : {0}", i)
                Console.WriteLine("Composite RMS EVM Mean (%)                     : {0}", compositeRmsEvmMean(i))
                Console.WriteLine("Composite Peak EVM Maximum (%)                 : {0}", compositePeakEvmMaximum(i))
                Console.WriteLine("Composite Peak EVM Slot Index                  : {0}", compositePeakEvmSlotIndex(i))
                Console.WriteLine("Composite Peak EVM Symbol Index                : {0}", compositePeakEvmSymbolIndex(i))
                Console.WriteLine("Composite Peak EVM Subcarrier Index            : {0}", compositePeakEvmSubcarrierIndex(i))
                Console.WriteLine("Component Carrier Frequency Error Mean (Hz)    : {0}", componentCarrierFrequencyErrorMean(i))
                Console.WriteLine("Component Carrier IQ Origin Offset Mean (dBc)  : {0}", componentCarrierIQOriginOffsetMean(i))
                Console.WriteLine("Chain  : {0}", i)
                Console.WriteLine("Component Carrier Time Offset Mean (s)         : {0}", componentCarrierTimingOffsetMean(i))
                Console.WriteLine("component Carrier Symbol Clock Error Mean (ppm): {0}", componentCarrierSymbolClockErrorMean(i))
                Console.WriteLine("In-Band Emission Margin (dB)                   : {0}", inBandEmissionMargin(i))
                Console.WriteLine("-----------------------------------------------------------------" & vbLf)
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
            If rfsgSessions IsNot Nothing Then
                For i As Integer = 0 To numberOfDevices - 1
                    rfsgSessions(i).Abort()
                    rfsgSessions(i).RF.OutputEnabled = False
                    rfsgSessions(i).Utility.Commit()
                    rfsgSessions(i).Arb.ClearWaveform(waveformName)
                    rfsgSessions(i).Close()
                    rfsgSessions(i) = Nothing
                Next
            End If
            tClockSession = Nothing
        End Sub

        Private Shared Sub DisplayError(ex As Exception)
            Console.WriteLine("ERROR:" & vbLf & Convert.ToString(ex.[GetType]()) & ": " & ex.Message)
        End Sub

    End Class
End Namespace
