'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
'4. Configure Trigger Type and Trigger Parameters.
'5. Configure Downlink Scrambling.
'6. Select ModAcc measurement and enable Traces.
'7. Configure Synchronization Mode and Measurement Interval.
'8. Initiate the Measurement.
'9. Fetch ModAcc Measurements and Traces.
'10. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.WcdmaMX

Namespace NationalInstruments.Examples.RFmxWcdmaDLModAccSingleCarrier

    Public Class RFmxWcdmaDLModAccSingleCarrier
        Private instrSession As RFmxInstrMX
        Private wcdma As RFmxWcdmaMX

        Private resourceName As String
        Private centerFrequency As Double
        Private referenceLevel As Double
        Private externalAttenuation As Double

        Private frequencyReferenceSource As String
        Private frequencyReferenceFrequency As Double

        Private enableTrigger As Boolean
        Private digitalEdgeTriggerSource As String
        Private digitalEdgeTriggerEdge As RFmxWcdmaMXDigitalEdgeTriggerEdge
        Private triggerDelay As Double

        Private synchronizationMode As RFmxWcdmaMXModAccSynchronizationMode
        Private measurementOffset As Integer
        Private measurementLength As Integer

        Private downlinkScramblingType As RFmxWcdmaMXDownlinkScramblingType
        Private downlinkScramblingPrimaryCode As Integer
        Private downlinkScramblingSecondaryCode As Integer

        Private measurement As RFmxWcdmaMXMeasurementTypes
        Private enableAllTraces As Boolean
        Private timeout As Double

        Private rmsEvm As Double, peakEvm As Double, rho As Double, frequencyError As Double
        Private chipRateError As Double, rmsMagnitudeError As Double, rmsPhaseError As Double
        Private iqOriginOffset As Double, iqGainImbalance As Double, iqQuadratureError As Double
        Private peakCde As Double, peakActiveCde As Double, peakRcde As Double
        Private peakCdeCode As Integer, peakActiveCdeSpreadingFactor As Integer
        Private peakActiveCdeCode As Integer, peakRcdeSpreadingFactor As Integer, peakRcdeCode As Integer
        Private peakCdeBranch As RFmxWcdmaMXModAccPeakCdeBranch
        Private peakActiveCdeBranch As RFmxWcdmaMXModAccPeakActiveCdeBranch
        Private peakRcdeBranch As RFmxWcdmaMXModAccPeakRcdeBranch
        Private evm As AnalogWaveform(Of Single)
        Private constellation As ComplexSingle()
        Private detectedSpreadingFactor As Integer(), detectedSpreadingCode As Integer()
        Private detectedModulationType As RFmxWcdmaMXModAccDetectedModulationType()
        Private detectedBranch As RFmxWcdmaMXModAccDetectedBranch()

        Public Sub Run()
            Try
                InitializeVariable()
                InitializeInstr()
                ConfigureWcdma()
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

        Private Sub InitializeVariable()
            ' Initialize input variables

            resourceName = "RFSA"
            centerFrequency = 1950000000.0
            ' Hz
            referenceLevel = 0.0
            ' dBm
            externalAttenuation = 0.0
            ' dB

            frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
            frequencyReferenceFrequency = 10000000.0
            ' Hz

            enableTrigger = False
            digitalEdgeTriggerSource = RFmxWcdmaMXConstants.Pfi0
            digitalEdgeTriggerEdge = RFmxWcdmaMXDigitalEdgeTriggerEdge.Rising
            triggerDelay = 0.0
            ' seconds

            synchronizationMode = RFmxWcdmaMXModAccSynchronizationMode.Slot
            measurementOffset = 0
            'slots
            measurementLength = 1
            'slots

            downlinkScramblingType = RFmxWcdmaMXDownlinkScramblingType.Standard
            downlinkScramblingPrimaryCode = 0
            downlinkScramblingSecondaryCode = 0

            measurement = RFmxWcdmaMXMeasurementTypes.ModAcc
            enableAllTraces = True

            timeout = 10.0
            ' seconds
        End Sub

        Private Sub InitializeInstr()
            ' Create a new RFmx Session
            instrSession = New RFmxInstrMX(resourceName, "")
        End Sub

        Private Sub ConfigureWcdma()
            wcdma = instrSession.GetWcdmaSignalConfiguration()
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
            wcdma.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
            wcdma.ConfigureDigitalEdgeTrigger("", digitalEdgeTriggerSource, digitalEdgeTriggerEdge,
                triggerDelay, enableTrigger)
            wcdma.SetLinkDirection("", RFmxWcdmaMXLinkDirection.Downlink)
            wcdma.SetDownlinkScramblingType("", downlinkScramblingType)
            wcdma.SetDownlinkScramblingPrimaryCode("", downlinkScramblingPrimaryCode)
            wcdma.SetDownlinkScramblingSecondaryCode("", downlinkScramblingSecondaryCode)
            wcdma.SelectMeasurements("", measurement, enableAllTraces)
            wcdma.ModAcc.Configuration.ConfigureSynchronizationModeAndInterval("", synchronizationMode,
                measurementOffset, measurementLength)
            wcdma.Initiate("", "")
        End Sub

        Private Sub RetrieveResults()
            wcdma.ModAcc.Results.FetchEvm("", timeout, rmsEvm, peakEvm, rho, frequencyError,
                chipRateError, rmsMagnitudeError, rmsPhaseError)
            wcdma.ModAcc.Results.FetchIQImpairments("", timeout, iqOriginOffset, iqGainImbalance, iqQuadratureError)
            wcdma.ModAcc.Results.FetchPeakCde("", timeout, peakCde, peakCdeCode, peakCdeBranch)
            wcdma.ModAcc.Results.FetchPeakActiveCde("", timeout, peakActiveCde, peakActiveCdeSpreadingFactor,
                peakActiveCdeCode, peakActiveCdeBranch)
            wcdma.ModAcc.Results.FetchRcde("", timeout, peakRcde, peakRcdeSpreadingFactor, peakRcdeCode, peakRcdeBranch)
            wcdma.ModAcc.Results.FetchEvmTrace("", timeout, evm)
            wcdma.ModAcc.Results.FetchConstellationTrace("", timeout, constellation)
            wcdma.ModAcc.Results.FetchDetectedChannelArray("", timeout, detectedSpreadingFactor,
                detectedSpreadingCode, detectedModulationType, detectedBranch)
        End Sub

        Private Sub PrintResults()
            Console.WriteLine("----------------------------EVM---------------------------")
            Console.WriteLine("RMS EVM (%)                              : {0}", rmsEvm)
            Console.WriteLine("Peak EVM (%)                             : {0}", peakEvm)
            Console.WriteLine("Rho                                      : {0}", rho)
            Console.WriteLine("Frequency Error (Hz)                     : {0}", frequencyError)
            Console.WriteLine("Chip Rate Error (ppm)                    : {0}", chipRateError)
            Console.WriteLine("RMS Magnitude Error (%)                  : {0}", rmsMagnitudeError)
            Console.WriteLine("RMS Phase Error (deg)                    : {0}", rmsPhaseError)

            Console.WriteLine(vbLf & "----------------------IQ Impairments----------------------")
            Console.WriteLine("I/Q Origin Offset (dB)                   : {0}", iqOriginOffset)
            Console.WriteLine("I/Q Gain Imbalance (dB)                  : {0}", iqGainImbalance)
            Console.WriteLine("I/Q Quadrature Error (deg)               : {0}", iqQuadratureError)

            Console.WriteLine(vbLf & "---------------------Code Domain Error--------------------")
            Console.WriteLine("Peak CDE (dB)                            : {0}", peakCde)
            Console.WriteLine("Peak CDE Code                            : {0}", peakCdeCode)
            Console.WriteLine("Peak CDE Branch                          : {0}", peakCdeBranch)
            Console.WriteLine("Peak Active CDE (dB)                     : {0}", peakActiveCde)
            Console.WriteLine("Peak Active CDE Code                     : {0}", peakActiveCdeCode)
            Console.WriteLine("Peak Active CDE Spreading Factor         : {0}", peakActiveCdeSpreadingFactor)
            Console.WriteLine("Peak Active CDE Branch                   : {0}", peakActiveCdeBranch)
            Console.WriteLine("Peak RCDE (dB)                           : {0}", peakRcde)
            Console.WriteLine("Peak RCDE Code                           : {0}", peakRcdeCode)
            Console.WriteLine("Peak RCDE Spreading Factor               : {0}", peakRcdeSpreadingFactor)
            Console.WriteLine("Peak RCDE Branch                         : {0}", peakRcdeBranch)

            Console.WriteLine(vbLf & "---------------------Detected Channels--------------------")
            For i As Integer = 0 To detectedSpreadingFactor.Length - 1
                Console.WriteLine("Idx                                      : {0}", i)
                Console.WriteLine("SF                                       : {0}", detectedSpreadingFactor(i))
                Console.WriteLine("Code                                     : {0}", detectedSpreadingCode(i))
                Console.WriteLine("Modulation                               : {0}", detectedModulationType(i))
                Console.WriteLine("Branch                                   : {0}" & vbLf, detectedBranch(i))
            Next
        End Sub

        Private Sub CloseSession()
            If wcdma IsNot Nothing Then
                wcdma.Dispose()
                wcdma = Nothing
            End If
            If instrSession IsNot Nothing Then
                instrSession.Close()
                instrSession = Nothing
            End If
        End Sub

        Private Shared Sub DisplayError(ex As Exception)
            Console.WriteLine("ERROR:" & vbLf & Convert.ToString(ex.[GetType]()) & ": " & ex.Message)
        End Sub

    End Class
End Namespace
