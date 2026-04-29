'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
'4. Configure Trigger Parameters for IQ Power Edge Trigger.
'5. Configure Auto TSC Detection Enabled.
'6. Configure Number of Timeslots.
'7. Configure Signal Type.
'8. Configure TSC.
'9. Select ModAcc measurement and enable Traces.
'10. Configure Averaging Parameters for ModAcc measurement.
'11. Initiate the Measurement.
'12  Fetch ModAcc Measurements and Traces.
'13. Close RFmx Session. 

Imports NationalInstruments.RFmx.GsmMX
Imports NationalInstruments.RFmx.InstrMX

Namespace NationalInstruments.Examples.RFmxGsmMultiSlotPferEvm
    Public Class RFmxGsmMultiSlotPferEvmExample
        Private instrSession As RFmxInstrMX
        Private gsm As RFmxGsmMX
        Private resourceName As String

        Private frequencyReferenceFrequency As Double, centerFrequency As Double, referenceLevel As Double, externalAttenuation As Double
        Private iqPowerEdgeLevel As Double, triggerDelay As Double, minimumQuietTime As Double, timeout As Double, meanRmsPhaseError As Double
        Private meanFrequencyError As Double, meanIQGainImbalance As Double, maximumIQGainImbalance As Double, meanIQOriginOffset As Double
        Private maximumIQOriginOffset As Double, maximumRmsPhaseError As Double, meanPeakPhaseError As Double, maximumPeakPhaseError As Double
        Private meanRmsEvm As Double, maximumRmsEvm As Double, meanPeakEvm As Double, maximumPeakEvm As Double, ninetyFifthPercentileEvm As Double

        Private enableTrigger As Boolean
        Private numberOfTimeslots As Integer, averagingCount As Integer, peakSymbol As Integer, peakEvmSymbol As Integer, measurementOffset As Integer
        Private frequencyReferenceSource As String, slotString As String
        Private detectedTsc As RFmxGsmMXModAccDetectedTsc()
        Private minimumQuietTimeMode As RFmxGsmMXTriggerMinimumQuietTimeMode
        Private autoTscDetectionEnabled As RFmxGsmMXAutoTscDetectionEnabled
        Private averagingEnabled As RFmxGsmMXModAccAveragingEnabled
        Private measurementInterval As RFmxGsmMXModAccMeasurementInterval
        Private meanTraceError As AnalogWaveform(Of Single)
        Private evm As AnalogWaveform(Of Single)
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
            triggerDelay = 0.0
            ' second 
            iqPowerEdgeLevel = -20.0
            ' dBm 
            minimumQuietTimeMode = RFmxGsmMXTriggerMinimumQuietTimeMode.Auto
            minimumQuietTime = 0.000582
            ' second 

            averagingEnabled = RFmxGsmMXModAccAveragingEnabled.[False]
            averagingCount = 10

            autoTscDetectionEnabled = RFmxGsmMXAutoTscDetectionEnabled.[True]

            numberOfTimeslots = 1
            measurementInterval = RFmxGsmMXModAccMeasurementInterval.NumberOfTimeslots
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
            gsm.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
            gsm.ConfigureIQPowerEdgeTrigger("", "0", RFmxGsmMXIQPowerEdgeTriggerSlope.Rising, iqPowerEdgeLevel, triggerDelay, minimumQuietTimeMode,
            minimumQuietTime, RFmxGsmMXIQPowerEdgeTriggerLevelType.Relative, enableTrigger)
            gsm.ConfigureAutoTscDetectionEnabled("", autoTscDetectionEnabled)
            gsm.ConfigureNumberOfTimeslots("", numberOfTimeslots)

            For i As Integer = 0 To numberOfTimeslots - 1
                slotString = RFmxGsmMX.BuildSlotString("", i)
                gsm.ConfigureSignalType(slotString, slotConfigurationInput(i).modulationType, slotConfigurationInput(i).burstType, slotConfigurationInput(i).hbFilterWidth)
                gsm.ConfigureTsc(slotString, slotConfigurationInput(i).tsc)
            Next

            gsm.SelectMeasurements("", RFmxGsmMXMeasurementTypes.ModAcc, True)
            gsm.ModAcc.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount)
            gsm.ModAcc.Configuration.SetMeasurementInterval("", measurementInterval)
            gsm.ModAcc.Configuration.SetMeasurementOffset("", measurementOffset)
            gsm.Initiate("", "")
        End Sub

        Private Sub RetrieveResults()
            If slotConfigurationInput(measurementOffset).modulationType <> RFmxGsmMXModulationType.ModulationTypeGmsk Then
                gsm.ModAcc.Results.FetchEvm("", timeout, meanRmsEvm, maximumRmsEvm, meanPeakEvm, maximumPeakEvm,
                ninetyFifthPercentileEvm, meanFrequencyError, peakEvmSymbol)
                gsm.ModAcc.Results.FetchEvmTrace("", timeout, evm)
            Else
                gsm.ModAcc.Results.FetchPfer("", timeout, meanRmsPhaseError, maximumRmsPhaseError, meanPeakPhaseError, maximumPeakPhaseError,
                meanFrequencyError, peakSymbol)
                gsm.ModAcc.Results.FetchPhaseErrorTrace("", timeout, meanTraceError)
            End If

            gsm.ModAcc.Results.FetchIQImpairments("", timeout, meanIQGainImbalance, maximumIQGainImbalance, meanIQOriginOffset, maximumIQOriginOffset)
            gsm.ModAcc.Results.FetchDetectedTscArray("", timeout, detectedTsc)

        End Sub

        Private Sub PrintResults()
            If slotConfigurationInput(measurementOffset).modulationType = RFmxGsmMXModulationType.ModulationTypeGmsk Then
                Console.WriteLine("---------PFER Measurement---------")
                Console.WriteLine("Mean RMS Phase Error (deg)     {0}", meanRmsPhaseError)
                Console.WriteLine("Maximum RMS Phase Error (deg)  {0}", maximumRmsPhaseError)
                Console.WriteLine("Mean Peak Phase Error (deg)    {0}", meanPeakPhaseError)
                Console.WriteLine("Maximum Peak Phase Error (deg) {0}", maximumPeakPhaseError)
                Console.WriteLine("Mean Frequency Error (Hz)      {0}", meanFrequencyError)
                Console.WriteLine("Peak Symbol                    {0}" & vbLf, peakSymbol)
            Else
                Console.WriteLine("---------------EVM Measurement---------------")
                Console.WriteLine("Mean RMS EVM (%)              {0}", meanRmsEvm)
                Console.WriteLine("Maximum RMS EVM (%)           {0}", maximumRmsEvm)
                Console.WriteLine("Mean Peak EVM (%)             {0}", meanPeakEvm)
                Console.WriteLine("Maximum Peak EVM (%)          {0}", maximumPeakEvm)
                Console.WriteLine("95th Percentile EVM (%)       {0}", ninetyFifthPercentileEvm)
                Console.WriteLine("Mean Frequency Error (Hz)     {0}", meanFrequencyError)
                Console.WriteLine("Peak EVM Symbol               {0}" & vbLf, peakEvmSymbol)
            End If

            Console.WriteLine("-----------------IQ Impairments----------------")
            Console.WriteLine("Maximum IQ Gain Imbalance (dB) {0}", maximumIQGainImbalance)
            Console.WriteLine("Maximum IQ Origin Offset (dB)  {0}", maximumIQOriginOffset)
            Console.WriteLine("Mean IQ Gain Imbalance (dB)    {0}", meanIQGainImbalance)
            Console.WriteLine("Mean IQ Origin Offset (dB)     {0}" & vbLf, meanIQOriginOffset)

            Console.WriteLine("------------Detected TSC-----------" & vbLf)
            For i As Integer = 0 To detectedTsc.Length - 1
                If measurementInterval = RFmxGsmMXModAccMeasurementInterval.NumberOfTimeslots Then
                    Console.WriteLine("Slot {0}                         {1}" & vbLf, i, detectedTsc(i))
                Else
                    Console.WriteLine("Slot {0}                         {1}" & vbLf, measurementOffset, detectedTsc(i))
                End If
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