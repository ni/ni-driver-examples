'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
'4. Configure Trigger Type and Trigger Parameters.
'5. Configure Uplink Scrambling.
'6. Configure Channel Configuration Mode.
'7[Auto Detect]. {No VIs to Configure for Channels}
'7[User Defined]. Configure User Defined Channels. 
'7[Test Model]Configure Test Model.
'8. Select ModAcc measurement and enable Traces.
'9. Configure Synchronization Mode and Measurement Interval.
'10[No Reference Waveform] Initiate Measurement & Fetch Reference Waveform
'10[Reference Waveform] {No VIs} 
'11. Configure Reference Waveform.
'12. Initiate the Measurement. 
'13. Fetch ModAcc Measurements and Traces.
'14. Close RFmx Session. 

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.WcdmaMX

Public Class RFmxWcdmaMXModAccMarkerMode

    Private instrSession As RFmxInstrMX
    Private wcdma As RFmxWcdmaMX

    Private resourceName As String = "RFSA"
    Private measurement As RFmxWcdmaMXMeasurementTypes = RFmxWcdmaMXMeasurementTypes.ModAcc
    Private frequencyReferenceSource As String = RFmxInstrMXConstants.OnboardClock
    Private frequencyReferenceFrequency As Double = 10000000.0                        ' Hz 
    Private centerFrequency As Double = 1950000000.0                ' Hz 
    Private externalAttenuation As Double = 0.0                     ' dB 

    Private digitalEdgeSource As String = RFmxWcdmaMXConstants.Pfi0
    Private digitalEdge As RFmxWcdmaMXDigitalEdgeTriggerEdge = RFmxWcdmaMXDigitalEdgeTriggerEdge.Rising
    Private triggerDelay As Double = 0.0                            ' seconds 
    Private referenceLevel As Double = 0.0                          ' dBm 

    Private timeout As Double = 10.0                                ' seconds 

    Private enableAllTraces As Boolean = True
    Private enableTrigger As Boolean = True
    Private uplinkScramblingCode As Integer = &H0
    Private uplinkScramblingType As RFmxWcdmaMXUplinkScramblingType = RFmxWcdmaMXUplinkScramblingType.Long
    Private measurementOffset As Integer = 0
    Private measurementLength As Integer = 1

    Private numberOfUserDefinedChannels As Integer
    Private userDefinedChannelsSpreadingFactor As Integer()
    Private userDefinedChannelsSpreadingCode As Integer()
    Private userDefinedChannelsModulationType As RFmxWcdmaMXModulationType()
    Private userDefinedChannelsBranch As RFmxWcdmaMXBranch()

    Private rmsEvm As Double
    Private peakEvm As Double
    Private rho As Double
    Private frequencyError As Double
    Private chipRateError As Double
    Private rmsMagnitudeError As Double
    Private rmsPhaseError As Double
    Private iqOriginOffset As Double
    Private iqGainImbalance As Double
    Private iqQuadratureError As Double
    Private peakCde As Double
    Private peakCdeCode As Integer
    Private peakCdeBranch As RFmxWcdmaMXModAccPeakCdeBranch
    Private peakActiveCde As Double
    Private peakActiveCdeSpreadingFactor As Integer
    Private peakActiveCdeCode As Integer
    Private peakActiveCdeBranch As RFmxWcdmaMXModAccPeakActiveCdeBranch
    Private peakRcde As Double
    Private peakRcdeSpreadingFactor As Integer
    Private peakRcdeCode As Integer
    Private peakRcdeBranch As RFmxWcdmaMXModAccPeakRcdeBranch
    Private evm As AnalogWaveform(Of Single)
    Private constellation As ComplexSingle()
    Private channelConfigurationMode As RFmxWcdmaMXChannelConfigurationMode = RFmxWcdmaMXChannelConfigurationMode.TestModel
    Private uplinkTestModel As RFmxWcdmaMXUplinkTestModel = RFmxWcdmaMXUplinkTestModel.R6C_2_1
    Private referenceWaveform As ComplexWaveform(Of ComplexSingle)

    Public Sub Run()
        Try
            InitializeVariables()
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

    Private Sub InitializeVariables()
        numberOfUserDefinedChannels = 0
        userDefinedChannelsBranch = New RFmxWcdmaMXBranch() {RFmxWcdmaMXBranch.Q, RFmxWcdmaMXBranch.I}
        userDefinedChannelsSpreadingCode = New Integer() {0, 16}
        userDefinedChannelsModulationType = New RFmxWcdmaMXModulationType() {RFmxWcdmaMXModulationType.ModulationTypeBpskQpsk, RFmxWcdmaMXModulationType.ModulationTypeBpskQpsk}
        userDefinedChannelsSpreadingFactor = New Integer() {256, 64}

        referenceWaveform = New ComplexWaveform(Of ComplexSingle)(0)
    End Sub

    Private Sub InitializeInstr()
        ' Create a new RFmx Session 

        instrSession = New RFmxInstrMX(resourceName, "")
    End Sub

    Private Sub ConfigureWcdma()
        wcdma = instrSession.GetWcdmaSignalConfiguration()
        instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
        wcdma.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)

        wcdma.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger)
        wcdma.ConfigureUplinkScrambling("", uplinkScramblingCode, uplinkScramblingType)
        wcdma.ConfigureChannelConfigurationMode("", channelConfigurationMode)

        Select Case channelConfigurationMode
            Case RFmxWcdmaMXChannelConfigurationMode.UserDefined
                wcdma.ConfigureNumberOfChannels("", numberOfUserDefinedChannels)
                wcdma.ConfigureUserDefinedChannelArray("", userDefinedChannelsSpreadingFactor, userDefinedChannelsSpreadingCode, userDefinedChannelsModulationType, userDefinedChannelsBranch)
                Exit Select
            Case RFmxWcdmaMXChannelConfigurationMode.TestModel
                wcdma.ConfigureUplinkTestModel("", uplinkTestModel)
                Exit Select
            Case Else
                Exit Select
        End Select

        wcdma.SelectMeasurements("", measurement, enableAllTraces)

        wcdma.ModAcc.Configuration.ConfigureSynchronizationModeAndInterval("", RFmxWcdmaMXModAccSynchronizationMode.Marker, measurementOffset, measurementLength)
        If referenceWaveform.Capacity = 0 Then
            wcdma.Initiate("", "")
            wcdma.ModAcc.Results.FetchReferenceWaveform("", timeout, referenceWaveform)
        End If

        wcdma.ModAcc.Configuration.ConfigureReferenceWaveform("", referenceWaveform)

    End Sub

    Private Sub RetrieveResults()
        wcdma.Initiate("", "")

        wcdma.ModAcc.Results.FetchEvm("", timeout, rmsEvm, peakEvm, rho, frequencyError, _
         chipRateError, rmsMagnitudeError, rmsPhaseError)
        wcdma.ModAcc.Results.FetchIQImpairments("", timeout, iqOriginOffset, iqGainImbalance, iqQuadratureError)
        wcdma.ModAcc.Results.FetchPeakCde("", timeout, peakCde, peakCdeCode, peakCdeBranch)
        wcdma.ModAcc.Results.FetchPeakActiveCde("", timeout, peakActiveCde, peakActiveCdeSpreadingFactor, peakActiveCdeCode, peakActiveCdeBranch)
        wcdma.ModAcc.Results.FetchRcde("", timeout, peakRcde, peakRcdeSpreadingFactor, peakRcdeCode, peakRcdeBranch)
        wcdma.ModAcc.Results.FetchEvmTrace("", timeout, evm)
        wcdma.ModAcc.Results.FetchConstellationTrace("", timeout, constellation)
    End Sub

    Private Sub PrintResults()

        Console.WriteLine("EVM:")
        Console.WriteLine("RMS EVM (%)                                      : {0}", rmsEvm)
        Console.WriteLine("Peak EVM (%)                                     : {0}", peakEvm)
        Console.WriteLine("Rho                                              : {0}", rho)
        Console.WriteLine("Frequency Error (Hz)                             : {0}", frequencyError)
        Console.WriteLine("Chip Rate Error (ppm)                            : {0}", chipRateError)
        Console.WriteLine("RMS Magnitude Error (%)                          : {0}", rmsMagnitudeError)
        Console.WriteLine("RMS Phase Error (deg)                            : {0}", rmsPhaseError)
        Console.WriteLine(vbLf & "---------------------------------------------------" & vbLf)
        Console.WriteLine("IQ Impairments :")
        Console.WriteLine("I/Q Origin Offset (dB)                           : {0}", iqOriginOffset)
        Console.WriteLine("I/Q Gain Imbalance (dB)                          : {0}", iqGainImbalance)
        Console.WriteLine("I/Q Quadrature Error (deg)                       : {0}", iqQuadratureError)
        Console.WriteLine(vbLf & "---------------------------------------------------")
        Console.WriteLine("Code Domain Error:")
        Console.WriteLine("Peak CDE (dB)                                    : {0}", peakCde)
        Console.WriteLine("Peak CDE Code                                    : {0}", peakCdeCode)
        Console.WriteLine("Peak CDE Branch                                  : {0}", peakCdeBranch)
        Console.WriteLine("Peak Active CDE (dB)                             : {0}", peakActiveCde)
        Console.WriteLine("Peak Active CDE Code                             : {0}", peakActiveCdeCode)
        Console.WriteLine("Peak Active CDE Spreading Factor                 : {0}", peakActiveCdeSpreadingFactor)
        Console.WriteLine("Peak Active CDE Branch                           : {0}", peakActiveCdeBranch)
        Console.WriteLine("Peak RCDE (dB)                                   : {0}", peakRcde)
        Console.WriteLine("Peak RCDE Code                                   : {0}", peakRcdeCode)
        Console.WriteLine("Peak RCDE Spreading Factor                       : {0}", peakRcdeSpreadingFactor)
        Console.WriteLine("Peak RCDE Branch                                 : {0}", peakRcdeBranch)
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
