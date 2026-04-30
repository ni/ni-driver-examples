'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure Selected Ports.
'4. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
'5. Configure Trigger Type and Trigger Parameters.
'6. Configure Link Direction as Uplink, Frequency Range, Band, Component Carrier and Subcarrier Spacing.
'7. Select ModAcc, ACP, CHP, OBW, SEM and TXP measurements and enable Traces.
'8. Configure ACP Sweep Time.
'9. Configure CHP Sweep Time.
'10. Configure OBW Sweep Time.
'11. Configure SEM Sweep Time.
'12. Configure Averaging Parameters for ModAcc.
'13. Configure Averaging Parameters for ACP.
'14. Configure Averaging Parameters for CHP.
'15. Configure Averaging Parameters for OBW.
'16. Configure Averaging Parameters for SEM.
'17. Configure Averaging Parameters for TXP.
'18. Configure Measurement Interval for ModAcc.
'19. Configure Measurement Interval for TXP.
'20. Configure SEM Uplink Mask Type, or Downlink Mask, gNodeB Category, Delta F_Max (Hz) and Component Carrier Rated Output Power depending on Link Direction.
'21. Initiate the Measurement.
'22. Fetch ModAcc Measurements.
'23. Fetch ACP Measurements.
'24. Fetch CHP Measurements.
'25. Fetch OBW Measurements.
'26. Fetch SEM Measurements.
'27. Fetch TXP Measurements
'28. Close RFmx Session. 

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.NRMX

Namespace NationalInstruments.Examples.RFmxNRModAccAcpChpObwSemTxpCompositeSingleCarrier
   Public Class RFmxNRModAccAcpChpObwSemTxpCompositeSingleCarrier
      Private instrSession As RFmxInstrMX
      Private NR As RFmxNRMX
      Private resourceName As String

      Private selectedPorts As String
      Private centerFrequency As Double
      Private referenceLevel As Double
      Private externalAttenuation As Double

      Private frequencyReferenceSource As String
      Private frequencyReferenceFrequency As Double

      Private enableTrigger As Boolean
      Private digitalEdgeSource As String
      Private digitalEdge As RFmxNRMXDigitalEdgeTriggerEdge
      Private triggerDelay As Double

      Private linkDirection As RFmxNRMXLinkDirection
      Private frequencyRange As RFmxNRMXFrequencyRange
      Private carrierBandwidth As Double
      Private subcarrierSpacing As Double
      Private modaccBand As Integer

      Private measurementLengthUnit As RFmxNRMXModAccMeasurementLengthUnit
      Private measurementOffset As Double
      Private measurementLength As Double

      Private txpMeasurementOffset As Double
	  ' (s)
      Private txpMeasurementLength As Double
      ' (s)
      Private uplinkMaskType As RFmxNRMXSemUplinkMaskType

      Private gNodeBCategory As RFmxNRMXgNodeBCategory
      Private downlinkMaskType As RFmxNRMXSemDownlinkMaskType
      Private deltaFMaximum As Double
      Private componentCarrierRatedOutputPower As Double

      Private sweepTimeInterval As Double
      Private averagingCount As Integer

      Private timeout As Double

      Private compositeRmsEvmMean As Double
      ' (%)
      Private compositePeakEvmMaximum As Double
      ' (%)
      Private componentCarrierFrequencyErrorMean As Double
      ' (Hz)
      Private componentCarrierIQOriginOffsetMean As Double
      ' (dBc)

      Private chpAbsolutePower As Double
      ' (dBm)
      Private chpRelativePower As Double
      ' (dB)

      Private acpAbsolutePower As Double
      ' (dBm)
      Private acpRelativePower As Double
      ' (dB)
      Private acpLowerRelativePower As Double()
      ' (dB)
      Private acpUpperRelativePower As Double()
      ' (dB)
      Private acpLowerAbsolutePower As Double()
      ' (dBm)
      Private acpUpperAbsolutePower As Double()
      ' (dBm)

      Private obwOccupiedBandwidth As Double
      ' (Hz)
      Private obwAbsolutePower As Double
      ' (dBm)
      Private obwStartFrequency As Double
      ' (Hz)
      Private obwStopFrequency As Double
      ' (Hz)

      Private semMeasurementStatus As RFmxNRMXSemMeasurementStatus
      Private semAbsoluteIntegratedPower As Double
      ' (dBm)
      Private semRelativeIntegratedPower As Double
      ' (dB)
      Private semPeakAbsoluteIntegratedPower As Double
      Private semPeakFrequency As Double
      Private semLowerOffsetMeasurementStatus As RFmxNRMXSemLowerOffsetMeasurementStatus()
      Private semLowerOffsetMargin As Double()
      ' (dB)
      Private semLowerOffsetMarginFrequency As Double()
      ' (Hz)
      Private semLowerOffsetMarginAbsolutePower As Double()
      ' (dBm)
      Private semLowerOffsetMarginRelativePower As Double()
      ' (dB)
      Private semUpperOffsetMeasurementStatus As RFmxNRMXSemUpperOffsetMeasurementStatus()
      Private semUpperOffsetMargin As Double()
      ' (dB)
      Private semUpperOffsetMarginFrequency As Double()
      ' (Hz)
      Private semUpperOffsetMarginAbsolutePower As Double()
      ' (dBm)
      Private semUpperOffsetMarginRelativePower As Double()
      ' (dB)

      Private averagePowerMean As Double
      ' (dBm)
      Private peakPowerMaximum As Double
      ' (dBm)

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
            Console.WriteLine(vbLf & "Press any key to exit")
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

         enableTrigger = False
         digitalEdgeSource = RFmxNRMXConstants.PxiTriggerLine0
         digitalEdge = RFmxNRMXDigitalEdgeTriggerEdge.Rising
         triggerDelay = 0.0
         ' (s)

         linkDirection = RFmxNRMXLinkDirection.Uplink
         frequencyRange = RFmxNRMXFrequencyRange.Range1
         carrierBandwidth = 100000000.0
         ' (Hz)
         subcarrierSpacing = 30000.0
         ' (Hz)
         modaccBand = 78

         measurementLengthUnit = RFmxNRMXModAccMeasurementLengthUnit.Slot
         measurementOffset = 0.0
         measurementLength = 1

         txpMeasurementLength = 0.001
		 ' (s)
         txpMeasurementOffset = 0.0
         ' (s)

         uplinkMaskType = RFmxNRMXSemUplinkMaskType.General

         gNodeBCategory = RFmxNRMXgNodeBCategory.WideAreaBaseStationCategoryA
         downlinkMaskType = RFmxNRMXSemDownlinkMaskType.Standard
         deltaFMaximum = 15000000.0
         ' (Hz)
         componentCarrierRatedOutputPower = 0.0
         ' (dBm)

         sweepTimeInterval = 0.001
         ' (s)

         averagingCount = 10

         timeout = 10.0
         ' (s)
      End Sub

      Private Sub InitializeInstr()
         ' Create a new RFmx Session
         instrSession = New RFmxInstrMX(resourceName, "")
      End Sub

      Private Sub ConfigureNR()
         NR = instrSession.GetNRSignalConfiguration()

         instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)

         NR.SetSelectedPorts("", selectedPorts)

         NR.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)

         NR.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger)

         NR.SetLinkDirection("", linkDirection)
         NR.SetFrequencyRange("", frequencyRange)
         NR.SetBand("", modaccBand)
         NR.ComponentCarrier.SetBandwidth("", carrierBandwidth)
         NR.ComponentCarrier.SetBandwidthPartSubcarrierSpacing("", subcarrierSpacing)

         NR.SelectMeasurements("", RFmxNRMXMeasurementTypes.ModAcc Or RFmxNRMXMeasurementTypes.Acp Or
         RFmxNRMXMeasurementTypes.Chp Or RFmxNRMXMeasurementTypes.Obw Or RFmxNRMXMeasurementTypes.Sem Or RFmxNRMXMeasurementTypes.Txp, True)

         NR.Acp.Configuration.ConfigureSweepTime("", RFmxNRMXAcpSweepTimeAuto.True, sweepTimeInterval)

         NR.Chp.Configuration.ConfigureSweepTime("", RFmxNRMXChpSweepTimeAuto.True, sweepTimeInterval)

         NR.Obw.Configuration.ConfigureSweepTime("", RFmxNRMXObwSweepTimeAuto.True, sweepTimeInterval)

         NR.Sem.Configuration.ConfigureSweepTime("", RFmxNRMXSemSweepTimeAuto.True, sweepTimeInterval)

         NR.ModAcc.Configuration.SetAveragingEnabled("", RFmxNRMXModAccAveragingEnabled.False)
         NR.ModAcc.Configuration.SetAveragingCount("", averagingCount)

         NR.Acp.Configuration.ConfigureAveraging("", RFmxNRMXAcpAveragingEnabled.False, averagingCount,
            RFmxNRMXAcpAveragingType.Rms)

         NR.Chp.Configuration.ConfigureAveraging("", RFmxNRMXChpAveragingEnabled.False, averagingCount,
            RFmxNRMXChpAveragingType.Rms)

         NR.Obw.Configuration.ConfigureAveraging("", RFmxNRMXObwAveragingEnabled.False, averagingCount,
            RFmxNRMXObwAveragingType.Rms)

            NR.Sem.Configuration.ConfigureAveraging("", RFmxNRMXSemAveragingEnabled.False, averagingCount,
            RFmxNRMXSemAveragingType.Rms)

            NR.Txp.Configuration.SetAveragingEnabled("", RFmxNRMXTxpAveragingEnabled.False)
            NR.Txp.Configuration.SetAveragingCount("", averagingCount)

            NR.ModAcc.Configuration.SetMeasurementLengthUnit("", measurementLengthUnit)
         NR.ModAcc.Configuration.SetMeasurementOffset("", measurementOffset)
         NR.ModAcc.Configuration.SetMeasurementLength("", measurementLength)

            NR.Txp.Configuration.SetMeasurementOffset("", txpMeasurementOffset)
            NR.Txp.Configuration.SetMeasurementInterval("", txpMeasurementLength)

            If linkDirection = RFmxNRMXLinkDirection.Uplink Then
            NR.Sem.Configuration.ConfigureUplinkMaskType("", uplinkMaskType)
         Else
            NR.ConfiguregNodeBCategory("", gNodeBCategory)
            NR.Sem.Configuration.SetDownlinkMaskType("", downlinkMaskType)
            NR.Sem.Configuration.SetDeltaFMaximum("", deltaFMaximum)
            NR.Sem.Configuration.ComponentCarrier.ConfigureRatedOutputPower("", componentCarrierRatedOutputPower)
         End If

         NR.Initiate("", "")
      End Sub

      Private Sub RetrieveResults()
         NR.ModAcc.Results.GetCompositeRmsEvmMean("", compositeRmsEvmMean)
         NR.ModAcc.Results.GetCompositePeakEvmMaximum("", compositePeakEvmMaximum)
         NR.ModAcc.Results.GetComponentCarrierFrequencyErrorMean("", componentCarrierFrequencyErrorMean)
         NR.ModAcc.Results.GetComponentCarrierIQOriginOffsetMean("", componentCarrierIQOriginOffsetMean)

         NR.Acp.Results.FetchOffsetMeasurementArray("", timeout, acpLowerRelativePower, acpUpperRelativePower,
            acpLowerAbsolutePower, acpUpperAbsolutePower)

         NR.Acp.Results.ComponentCarrier.FetchMeasurement("", timeout, acpAbsolutePower, acpRelativePower)

         NR.Chp.Results.ComponentCarrier.FetchMeasurement("", timeout, chpAbsolutePower, chpRelativePower)

         NR.Obw.Results.FetchMeasurement("", timeout, obwOccupiedBandwidth, obwAbsolutePower, obwStartFrequency,
            obwStopFrequency)

         NR.Sem.Results.FetchLowerOffsetMarginArray("", timeout, semLowerOffsetMeasurementStatus, semLowerOffsetMargin,
            semLowerOffsetMarginFrequency, semLowerOffsetMarginAbsolutePower, semLowerOffsetMarginRelativePower)

         NR.Sem.Results.FetchUpperOffsetMarginArray("", timeout, semUpperOffsetMeasurementStatus, semUpperOffsetMargin,
            semUpperOffsetMarginFrequency, semUpperOffsetMarginAbsolutePower, semUpperOffsetMarginRelativePower)

         NR.Sem.Results.ComponentCarrier.FetchMeasurement("", timeout, semAbsoluteIntegratedPower,
            semPeakAbsoluteIntegratedPower, semPeakFrequency, semRelativeIntegratedPower)

         NR.Sem.Results.FetchMeasurementStatus("", timeout, semMeasurementStatus)

         NR.Txp.Results.FetchMeasurement("", timeout, averagePowerMean, peakPowerMaximum)
        End Sub

      Private Sub PrintResults()
         Console.WriteLine("************************* ModAcc *************************" & vbLf)
         Console.WriteLine("Composite RMS EVM Mean (%)                     : {0}", compositeRmsEvmMean)
         Console.WriteLine("Composite Peak EVM Maximum (% )                : {0}", compositePeakEvmMaximum)
         Console.WriteLine("Component Carrier Frequency Error Mean (Hz)    : {0}", componentCarrierFrequencyErrorMean)
         Console.WriteLine("Component Carrier IQ Origin Offset Mean (dBc)  : {0}" & vbLf, componentCarrierIQOriginOffsetMean)

         Console.WriteLine(vbLf & vbLf & "************************* CHP *************************" & vbLf)
         Console.WriteLine("Carrier Absolute Power (dBm)                   : {0}" & vbLf, chpAbsolutePower)

         Console.WriteLine(vbLf & vbLf & "************************* ACP *************************" & vbLf)
         Console.WriteLine("Carrier Absolute Power (dBm)                   : {0}", acpAbsolutePower)
         Console.WriteLine(vbLf & "------- Offset Channel Measurements -------")
         For i As Integer = 0 To acpLowerRelativePower.Length - 1
            Console.WriteLine(vbLf & "Offset  {0}", i)
            Console.WriteLine("Lower Relative Power (dB)                      : {0}", acpLowerRelativePower(i))
            Console.WriteLine("Upper Relative Power (dB)                      : {0}", acpUpperRelativePower(i))
            Console.WriteLine("Lower Absolute Power (dBm)                     : {0}", acpLowerAbsolutePower(i))
            Console.WriteLine("Upper Absolute Power (dBm)                     : {0}", acpUpperAbsolutePower(i))
         Next

         Console.WriteLine(vbLf & vbLf & vbLf & "************************* OBW *************************" & vbLf)
         Console.WriteLine("Occupied Bandwidth (Hz)                        : {0}", obwOccupiedBandwidth)
         Console.WriteLine("Absolute Power (dBm)                           : {0}", obwAbsolutePower)
         Console.WriteLine("Start Frequency (Hz)                           : {0}", obwStartFrequency)
         Console.WriteLine("Stop Frequency (Hz)                            : {0}" & vbLf, obwStopFrequency)

         Console.WriteLine(vbLf & vbLf & "************************* SEM *************************" & vbLf)
         Console.WriteLine("Measurement Status                             : {0}", semMeasurementStatus)
         Console.WriteLine("Carrier Absolute Integrated Power (dBm)        : {0}", semAbsoluteIntegratedPower)
         Console.WriteLine(vbLf & "----- Lower Offset Segment Measurements -----")
         For i As Integer = 0 To semLowerOffsetMargin.Length - 1
            Console.WriteLine(vbLf & "Offset  {0}", i)
            Console.WriteLine("Measurement Status                             : {0}", semLowerOffsetMeasurementStatus(i))
            Console.WriteLine("Margin (dB)                                    : {0}", semLowerOffsetMargin(i))
            Console.WriteLine("Margin Frequency (Hz)                          : {0}", semLowerOffsetMarginFrequency(i))

            Console.WriteLine("Margin Absolute Power (dBm)                    : {0}", semLowerOffsetMarginAbsolutePower(i))
         Next
         Console.WriteLine(vbLf & "----- Upper Offset Segment Measurements -----")
            For i As Integer = 0 To semUpperOffsetMargin.Length - 1
                Console.WriteLine(vbLf & "Offset  {0}", i)
                Console.WriteLine("Measurement Status                             : {0}", semUpperOffsetMeasurementStatus(i))
                Console.WriteLine("Margin (dB)                                    : {0}", semUpperOffsetMargin(i))
                Console.WriteLine("Margin Frequency (Hz)                          : {0}", semUpperOffsetMarginFrequency(i))
                Console.WriteLine("Margin Absolute Power (dBm)                    : {0}", semUpperOffsetMarginAbsolutePower(i))
            Next

         Console.WriteLine(vbLf & vbLf & "************************* TXP *************************" & vbLf)
         Console.WriteLine("Average Power Mean (dBm)                           : {0}" & vbLf, averagePowerMean)
         Console.WriteLine("Peak Power Maximum (dBm)                           : {0}" & vbLf, peakPowerMaximum)


        End Sub

      Private Sub CloseSession()
         Try
            If NR IsNot Nothing Then
               NR.Dispose()
               NR = Nothing
            End If
            If instrSession IsNot Nothing Then
               instrSession.Close()
               instrSession = Nothing
            End If
         Catch ex As Exception
            DisplayError(ex)
         End Try
      End Sub

      Private Shared Sub DisplayError(ex As Exception)
         Console.WriteLine("ERROR:" & vbLf & Convert.ToString(ex.[GetType]()) & ": " & ex.Message)
      End Sub

   End Class
End Namespace
