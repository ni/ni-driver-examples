'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure External Attenuation.
'4. Configure Center Frequency. 
'5. Configure Link Direction.
'6. Configure Trigger Parameters for IQ Power Edge Trigger.
'7. Configure Auto Level.
'8. Configure Auto TSC Detection Enabled.
'9. Configure Number of Timeslots.
'10. Configure Signal Type.
'11. Configure TSC.
'12. Select  ORFS  measurement and enable Traces.
'13. Configure Noise Compensation Enabled.
'14. Configure Measurement Type. 
'15. Configure Offset Frequency Mode.
'16. Configure Evaluation Symbols.
'17. Configure Averaging Parameters.
'18. Initiate the Measurement.
'19  Fetch ORFS Measurements and Traces.
'20. Close RFmx Session. 

Imports NationalInstruments.RFmx.GsmMX
Imports NationalInstruments.RFmx.InstrMX

Namespace NationalInstruments.Examples.RFmxGsmMultiSlotOrfs
    Public Class RFmxGsmMultiSlotOrfsExample
        Private instrSession As RFmxInstrMX
        Private gsm As RFmxGsmMX
        Private resourceName As String

        Private frequencyReferenceFrequency As Double, centerFrequency As Double, referenceLevel As Double, externalAttenuation As Double, iqPowerEdgeLevel As Double, triggerDelay As Double,
        minimumQuietTime As Double, autoReferenceLevel As Double, timeout As Double, modulationCarrierPower As Double, evaluationSymbolsStart As Double, evaluationSymbolsStop As Double,
        switchingCarrierPower As Double

        Private modulationResultsLowerRelativePower As Double(), modulationResultsUpperRelativePower As Double(), modulationResultsLowerAbsolutePower As Double(), modulationResultsUpperAbsolutePower As Double(), switchingResultsLowerRelativePower As Double(), switchingResultsUpperRelativePower As Double(),
        switchingResultsLowerAbsolutePower As Double(), switchingResultsUpperAbsolutePower As Double()

        Private modulationPowerTraceOffsetFrequency As Single(), modulationPowerTraceAbsolutePower As Single(), modulationPowerTraceRelativePower As Single(), switchingPowerTraceOffsetFrequency As Single(), switchingPowerTraceAbsolutePower As Single(), switchingPowerTraceRelativePower As Single()

        Private enableTrigger As Boolean
        Private numberOfTimeslots As Integer, averagingCount As Integer, measurementOffset As Integer
        Private frequencyReferenceSource As String, slotString As String
        Private linkDirection As RFmxGsmMXLinkDirection
        Private averagingType As RFmxGsmMXOrfsAveragingType
        Private measurementType As RFmxGsmMXOrfsMeasurementType
        Private offsetFrequencyMode As RFmxGsmMXOrfsOffsetFrequencyMode
        Private burstType As RFmxGsmMXBurstType
        Private hbFilterWidth As RFmxGsmMXHBFilterWidth
        Private minimumQuietTimeMode As RFmxGsmMXTriggerMinimumQuietTimeMode
        Private autoTscDetectionEnabled As RFmxGsmMXAutoTscDetectionEnabled
        Private modulationType As RFmxGsmMXModulationType
        Private averagingEnabled As RFmxGsmMXOrfsAveragingEnabled
        Private tsc As RFmxGsmMXTsc
        Private noiseCompensationEnabled As RFmxGsmMXOrfsNoiseCompensationEnabled
        Private evaluationSymbolsIncludeTsc As RFmxGsmMXOrfsEvaluationSymbolsIncludeTsc
        Private measurementInterval As RFmxGsmMXOrfsMeasurementInterval

        Private autoLevel As Boolean = True
        Private Structure SlotConfiguration
            Public modulationType As RFmxGsmMXModulationType
            Public burstType As RFmxGsmMXBurstType
            Public hbFilterWidth As RFmxGsmMXHBFilterWidth
            Public tsc As RFmxGsmMXTsc
        End Structure
        Private slotConfigurationInput As SlotConfiguration()
        Public Sub Run()
            Try
                InitializeVariables()
                InitializeInstr()
                ConfigureGsm()
                RetrieveResults()
                PrintResults()
            Catch ex As Exception
                DisplayError(ex)
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

            frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
            frequencyReferenceFrequency = 10000000.0
            ' Hz 

            centerFrequency = 890200000.0
            ' Hz 
            referenceLevel = 0.0
            ' dBm 
            externalAttenuation = 0.0
            ' dB 


            enableTrigger = True
            iqPowerEdgeLevel = -20.0
            ' dBm 
            triggerDelay = 0.0
            ' second 
            minimumQuietTimeMode = RFmxGsmMXTriggerMinimumQuietTimeMode.Auto
            minimumQuietTime = 0.000582
            ' second 

            linkDirection = RFmxGsmMXLinkDirection.Uplink
            noiseCompensationEnabled = RFmxGsmMXOrfsNoiseCompensationEnabled.[False]

            averagingEnabled = RFmxGsmMXOrfsAveragingEnabled.[False]
            averagingCount = 10
            averagingType = RFmxGsmMXOrfsAveragingType.Log

            evaluationSymbolsStart = 50.0
            evaluationSymbolsStop = 90.0
            evaluationSymbolsIncludeTsc = RFmxGsmMXOrfsEvaluationSymbolsIncludeTsc.[False]

            offsetFrequencyMode = RFmxGsmMXOrfsOffsetFrequencyMode.Standard
            measurementType = RFmxGsmMXOrfsMeasurementType.ModulationAndSwitching

            autoTscDetectionEnabled = RFmxGsmMXAutoTscDetectionEnabled.[True]

            numberOfTimeslots = 1
            measurementInterval = RFmxGsmMXOrfsMeasurementInterval.NumberOfTimeslots
            measurementOffset = 0

            timeout = 10.0
            ' second 

            slotConfigurationInput = New SlotConfiguration(numberOfTimeslots - 1) {}

            For i As Integer = 0 To numberOfTimeslots - 1
                slotConfigurationInput(i).modulationType = RFmxGsmMXModulationType.ModulationTypeGmsk
                slotConfigurationInput(i).burstType = RFmxGsmMXBurstType.NB
                slotConfigurationInput(i).hbFilterWidth = RFmxGsmMXHBFilterWidth.Narrow
                ' Hz 
                slotConfigurationInput(i).tsc = RFmxGsmMXTsc.Tsc0
            Next

        End Sub

        Private Sub InitializeInstr()
            ' Create a new RFmx Session 

            instrSession = New RFmxInstrMX(resourceName, "")
        End Sub

        Private Sub ConfigureGsm()
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)

            gsm = instrSession.GetGsmSignalConfiguration()
            gsm.ConfigureExternalAttenuation("", externalAttenuation)
            gsm.ConfigureFrequency("", centerFrequency)
            gsm.ConfigureLinkDirection("", linkDirection)
            gsm.ConfigureIQPowerEdgeTrigger("", "0", RFmxGsmMXIQPowerEdgeTriggerSlope.Rising, iqPowerEdgeLevel, triggerDelay, minimumQuietTimeMode,
            minimumQuietTime, RFmxGsmMXIQPowerEdgeTriggerLevelType.Relative, enableTrigger)
            If autoLevel Then
                gsm.AutoLevel("", 0.0046, autoReferenceLevel)
                Console.WriteLine("Reference Level : {0}", autoReferenceLevel)
            Else
                gsm.ConfigureReferenceLevel("", referenceLevel)
            End If

            gsm.ConfigureAutoTscDetectionEnabled("", autoTscDetectionEnabled)
            gsm.ConfigureNumberOfTimeslots("", numberOfTimeslots)
            For i As Integer = 0 To numberOfTimeslots - 1
                slotString = RFmxGsmMX.BuildSlotString("", i)
                gsm.ConfigureSignalType(slotString, slotConfigurationInput(i).modulationType, slotConfigurationInput(i).burstType, slotConfigurationInput(i).hbFilterWidth)
                gsm.ConfigureTsc(slotString, slotConfigurationInput(i).tsc)
            Next

            gsm.SelectMeasurements("", RFmxGsmMXMeasurementTypes.Orfs, True)
            gsm.Orfs.Configuration.ConfigureNoiseCompensationEnabled("", noiseCompensationEnabled)
            gsm.Orfs.Configuration.ConfigureMeasurementType("", measurementType)
            gsm.Orfs.Configuration.ConfigureOffsetFrequencyMode("", offsetFrequencyMode)
            gsm.Orfs.Configuration.ConfigureEvaluationSymbols("", evaluationSymbolsStart, evaluationSymbolsIncludeTsc, evaluationSymbolsStop)
            gsm.Orfs.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)
            gsm.Orfs.Configuration.SetMeasurementInterval("", measurementInterval)
            gsm.Orfs.Configuration.SetMeasurementOffset("", measurementOffset)
            gsm.Initiate("", "")
        End Sub

        Private Sub RetrieveResults()
            gsm.Orfs.Results.FetchModulationResultsArray("", timeout, modulationCarrierPower, modulationResultsLowerRelativePower, modulationResultsUpperRelativePower, modulationResultsLowerAbsolutePower,
            modulationResultsUpperAbsolutePower)
            gsm.Orfs.Results.FetchSwitchingResultsArray("", timeout, switchingCarrierPower, switchingResultsLowerRelativePower, switchingResultsUpperRelativePower, switchingResultsLowerAbsolutePower,
            switchingResultsUpperAbsolutePower)

            gsm.Orfs.Results.FetchModulationPowerTrace("", timeout, modulationPowerTraceOffsetFrequency, modulationPowerTraceAbsolutePower, modulationPowerTraceRelativePower)
            gsm.Orfs.Results.FetchSwitchingPowerTrace("", timeout, switchingPowerTraceOffsetFrequency, switchingPowerTraceAbsolutePower, switchingPowerTraceRelativePower)
        End Sub

        Private Sub PrintResults()
            Console.WriteLine("---------Modulation Results---------")
            Console.WriteLine("Modulation Carrier Power (dBm) {0}" & vbLf, modulationCarrierPower)
            For i As Integer = 0 To modulationResultsLowerAbsolutePower.Length - 1
                Console.WriteLine("Offset : {0}", i)
                Console.WriteLine("Lower Absolute Power (dBm)   {0}", modulationResultsLowerAbsolutePower(i))
                Console.WriteLine("Lower Relative Power (dB)    {0}", modulationResultsLowerRelativePower(i))
                Console.WriteLine("Upper Absolute Power (dBm)   {0}", modulationResultsUpperAbsolutePower(i))
                Console.WriteLine("Upper Relative Power (dB)    {0}" & vbLf, modulationResultsUpperRelativePower(i))
            Next
            Console.WriteLine("---------Switching Results---------")
            Console.WriteLine("Switching Carrier Power (dBm)  {0}" & vbLf, switchingCarrierPower)

            For i As Integer = 0 To switchingResultsLowerAbsolutePower.Length - 1
                Console.WriteLine("Offset : {0}", i)
                Console.WriteLine("Lower Absolute Power (dBm)   {0}", switchingResultsLowerAbsolutePower(i))
                Console.WriteLine("Lower Relative Power (dB)    {0}", switchingResultsLowerRelativePower(i))
                Console.WriteLine("Upper Absolute Power (dBm)   {0}", switchingResultsUpperAbsolutePower(i))
                Console.WriteLine("Upper Relative Power (dB)    {0}" & vbLf, switchingResultsUpperRelativePower(i))
            Next
        End Sub

        Private Sub CloseSession()
            Try
                If gsm IsNot Nothing Then
                    gsm.Dispose()
                    gsm = Nothing
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