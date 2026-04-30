'Steps:
'1. Open a new RFmx session.
'2. Configure Frequency Reference.
'3. Configure the basic signal properties: Center Frequency, Reference Level and External Attenuation.
'4. Configure the Trigger Type and Trigger Parameters.
'5. Configure Radio Configuration Parameter.
'6. Configure Downlink PN Offset.
'7. Select ModAcc measurement and enable the traces.
'8. Configure Synchronization Mode and Measurement Interval. 
'9. Configure Multi Carrier filter.
'10. Initiate Measurement.
'11. Fetch ModAcc Measurements and Traces.
'12. Close the RFmx session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.Cdma2kMX

Namespace NationalInstruments.Examples.RFmxCdma2kDLModAcc
    Public Class RFmxCdma2kDLModAcc
        Private instrSession As RFmxInstrMX
        Private cdma2k As RFmxCdma2kMX
        Private resourceName As String, frequencySource As String, digitalEdgeTriggerSource As String
        Private centerFrequency As Double, referenceLevel As Double, externalAttenuation As Double, frequency As Double

        Private triggerDelay As Double
        Private enableTrigger As Boolean
        Private digitalEdgeTriggerEdge As RFmxCdma2kMXDigitalEdgeTriggerEdge

        Private radioConfiguration As RFmxCdma2kMXRadioConfiguration

        Private DLSpreadingPNOffset As Integer

        Private multiCarrierFilterEnabled As RFmxCdma2kMXModAccMultiCarrierFilterEnabled

        Private peakActiveCdeBranch As RFmxCdma2kMXModAccPeakActiveCdeBranch
        Private synchronizationMode As RFmxCdma2kMXModAccSynchronizationMode
        Private measurementOffset As Integer, measurementLength As Integer

        Private peakActiveCdeWalshCodeLength As Integer, peakActiveCdeWalshCodeNumber As Integer

        Private timeout As Double
        Private RMSEVM As Double, peakEvm As Double, rho As Double, frequencyError As Double, chipRateError As Double, RMSMagnitudeError As Double, _
         RMSPhaseError As Double
        Private peakCde As Double, peakActiveCde As Double
        Private iqOriginOffset As Double, iqGainImbalence As Double, iqQuadratureError As Double
        Private peakCdeWalshCodeNumber As Integer
        Private peakCdeBranch As RFmxCdma2kMXModAccPeakCdeBranch
        Private detectedWalshCodeLength As Integer(), detectedWalshCodeNumber As Integer()
        Private detectedBranch As RFmxCdma2kMXModAccDetectedBranch()

        Public Sub Run()
            Try
                InitializeVariables()
                InitializeInstr()
                ConfigureCdma2k()
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

            centerFrequency = 833490000.0
            ' Hz 
            referenceLevel = 0.0
            ' dBm 
            externalAttenuation = 0.0
            ' dB 

            frequencySource = RFmxInstrMXConstants.OnboardClock
            frequency = 10000000.0
            ' Hz 

            triggerDelay = 0.0
            ' seconds 
            enableTrigger = False
            digitalEdgeTriggerEdge = RFmxCdma2kMXDigitalEdgeTriggerEdge.Rising
            digitalEdgeTriggerSource = RFmxCdma2kMXConstants.Pfi0

            radioConfiguration = RFmxCdma2kMXRadioConfiguration.RC3
            DLSpreadingPNOffset = 0
            ' 64 chips
            multiCarrierFilterEnabled = RFmxCdma2kMXModAccMultiCarrierFilterEnabled.False

            synchronizationMode = RFmxCdma2kMXModAccSynchronizationMode.Slot

            measurementOffset = 0
            ' slots
            measurementLength = 1
            ' slots

            timeout = 10
            ' seconds 
        End Sub

        Private Sub InitializeInstr()
            ' Create a new RFmx Session 

            instrSession = New RFmxInstrMX(resourceName, "")
        End Sub

        Private Sub ConfigureCdma2k()
            ' Get SpecAn signal 

            cdma2k = instrSession.GetCdma2kSignalConfiguration()

            ' Configure measurement 

            instrSession.ConfigureFrequencyReference("", frequencySource, frequency)
            cdma2k.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
            cdma2k.ConfigureDigitalEdgeTrigger("", digitalEdgeTriggerSource, digitalEdgeTriggerEdge, triggerDelay, enableTrigger)
            cdma2k.ConfigureRadioConfiguration("", radioConfiguration)
            cdma2k.SetLinkDirection("", RFmxCdma2kMXLinkDirection.Downlink)
            cdma2k.SetDownlinkSpreadingPNOffset("", DLSpreadingPNOffset)
            cdma2k.SelectMeasurements("", RFmxCdma2kMXMeasurementTypes.ModAcc, True)
            cdma2k.ModAcc.Configuration.SetMultiCarrierFilterEnabled("", multiCarrierFilterEnabled)
            cdma2k.ModAcc.Configuration.ConfigureSynchronizationModeAndInterval("", synchronizationMode, measurementOffset,
                                                                                measurementLength)
            cdma2k.Initiate("", "")
        End Sub

        Private Sub RetrieveResults()
            Dim constellation As ComplexSingle() = Nothing
            Dim evm As AnalogWaveform(Of Single) = Nothing
            ' Retrieve results 

            cdma2k.ModAcc.Results.FetchEvm("", timeout, RMSEVM, peakEvm, rho, frequencyError, _
             chipRateError, RMSMagnitudeError, RMSPhaseError)
            cdma2k.ModAcc.Results.FetchIQImpairments("", timeout, iqOriginOffset, iqGainImbalence, iqQuadratureError)
            cdma2k.ModAcc.Results.FetchPeakCde("", timeout, peakCde, peakCdeWalshCodeNumber, peakCdeBranch)
            cdma2k.ModAcc.Results.FetchPeakActiveCde("", timeout, peakActiveCde, peakActiveCdeWalshCodeLength,
                                                     peakActiveCdeWalshCodeNumber, peakActiveCdeBranch)
            cdma2k.ModAcc.Results.FetchConstellationTrace("", timeout, constellation)
            cdma2k.ModAcc.Results.FetchEvmTrace("", timeout, evm)
            cdma2k.ModAcc.Results.FetchDetectedChannelArray("", timeout, detectedWalshCodeLength,
               detectedWalshCodeNumber, detectedBranch)
        End Sub

        Private Sub PrintResults()
            Console.WriteLine("--------------------EVM Results--------------------")
            Console.WriteLine("RMS EVM (%)                       : {0}", RMSEVM)
            Console.WriteLine("Peak EVM (%)                      : {0}", peakEvm)
            Console.WriteLine("Rho                               : {0}", rho)
            Console.WriteLine("Frequency Error (Hz)              : {0}", frequencyError)
            Console.WriteLine("Chip Rate Error (ppm)             : {0}", chipRateError)
            Console.WriteLine("RMS Magnitude Error (%)           : {0}", RMSMagnitudeError)
            Console.WriteLine("RMS Phase Error (deg)             : {0}", RMSPhaseError)


            Console.WriteLine(vbLf & "---------------------I/Q Impairments------------------------")
            Console.WriteLine("I/Q Origin Offset (dB)            : {0}", iqOriginOffset)
            Console.WriteLine("I/Q Gain Imbalance (dB)           : {0}", iqGainImbalence)
            Console.WriteLine("I/Q Quadrature Error (deg)        : {0}", iqQuadratureError)


            Console.WriteLine(vbLf & "-------------------Code Domain Error------------------------")
            Console.WriteLine("Peak CDE (dB)                     : {0}", peakCde)
            Console.WriteLine("Peak CDE Walsh Code Number        : {0}", peakCdeWalshCodeNumber)
            Console.WriteLine("Peak CDE Branch                   : {0}", peakCdeBranch)
            Console.WriteLine("Peak Active CDE (dB)              : {0}", peakActiveCde)
            Console.WriteLine("Peak Active CDE Walsh Code Number : {0}", peakActiveCdeWalshCodeNumber)
            Console.WriteLine("Peak Active CDE Walsh Code Length : {0}", peakActiveCdeWalshCodeLength)
            Console.WriteLine("Peak Active CDE Branch            : {0}", peakActiveCdeBranch)


            Console.WriteLine(vbLf & "---------------------Detected Channels--------------------")
            For i As Integer = 0 To detectedWalshCodeLength.Length - 1
                Console.WriteLine("Idx                               : {0}", i)
                Console.WriteLine("Length                            : {0}", detectedWalshCodeLength(i))
                Console.WriteLine("Number                            : {0}", detectedWalshCodeNumber(i))
                Console.WriteLine("Branch                            : {0}" & vbLf, detectedBranch(i))
            Next
        End Sub

        Private Sub CloseSession()
            Try
                If cdma2k IsNot Nothing Then
                    cdma2k.Dispose()
                    cdma2k = Nothing
                End If

                If instrSession IsNot Nothing Then
                    instrSession.Close()
                    instrSession = Nothing
                End If
            Catch ex As Exception
                DisplayError(ex)
            End Try
        End Sub

        Private Shared Sub DisplayError(ByVal ex As Exception)
            Console.WriteLine("ERROR:" & vbLf & Convert.ToString(ex.[GetType]()) & ": " & ex.Message)
        End Sub
    End Class
End Namespace
