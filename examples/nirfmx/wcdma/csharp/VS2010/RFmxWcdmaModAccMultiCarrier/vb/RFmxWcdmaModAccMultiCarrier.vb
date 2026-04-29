'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
'4. Configure Trigger Type and Trigger Parameters.
'5. Configure Contiguous Carriers.
'6. Configure Uplink Scramble (Array).
'7. Select ModAcc measurement and enable Traces.
'8. Configure Syncronization Mode and Interval.
'9. Initiate the Measurement.
'10. Fetch ModAcc Measurements and Traces.
'11.Close RFmx Session. 

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.WcdmaMX

Structure CarrierMeasurement
    Public evm As AnalogWaveform(Of Single)
    Public constellation As ComplexSingle()
End Structure

Public Class RFmxWcdmaModAccMultiCarrier
    Private instrSession As RFmxInstrMX
    Private wcdma As RFmxWcdmaMX

    Private resourceName As String
    Private measurement As RFmxWcdmaMXMeasurementTypes
    Private frequencyReferenceSource As String
    Private frequencyReferenceFrequency As Double
    Private centerFrequency As Double
    Private externalAttenuation As Double

    Private digitalEdgeTriggerSource As String
    Private digitalEdgeTriggerEdge As RFmxWcdmaMXDigitalEdgeTriggerEdge
    Private triggerDelay As Double
    Private referenceLevel As Double
    Private timeout As Double

    Const numberOfCarriers As Integer = 2
    Private enableAllTraces As Boolean
    Private enableTrigger As Boolean
    Private uplinkScramblingCode As Integer() = {0, 0}
    Private uplinkScramblingType As RFmxWcdmaMXUplinkScramblingType() = {RFmxWcdmaMXUplinkScramblingType.[Long], RFmxWcdmaMXUplinkScramblingType.[Long]}
    Private synchronizationMode As RFmxWcdmaMXModAccSynchronizationMode

    Private measurementOffset As Integer
    Private measurementLength As Integer
    Private carrierAtCenterFrequency As Integer

    Private rmsEvm As Double(), peakEvm As Double(), rho As Double(), frequencyError As Double(), chipRateError As Double(), rmsMagnitudeError As Double(), _
     rmsPhaseError As Double()
    Private iqOriginOffset As Double(), iqGainImbalance As Double(), iqQuadratureError As Double()

    Private peakCde As Double(), peakActiveCde As Double(), peakRcde As Double()
    Private peakCdeCode As Integer(), peakActiveCdeSpreadingFactor As Integer(), peakActiveCdeCode As Integer(), peakRcdeSpreadingFactor As Integer(), peakRcdeCode As Integer()
    Private peakCdeBranch As RFmxWcdmaMXModAccPeakCdeBranch()
    Private peakActiveCdeBranch As RFmxWcdmaMXModAccPeakActiveCdeBranch()
    Private peakRcdeBranch As RFmxWcdmaMXModAccPeakRcdeBranch()
    Private carrierChannelOutput As CarrierMeasurement() = New CarrierMeasurement(numberOfCarriers - 1) {}


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
        measurement = RFmxWcdmaMXMeasurementTypes.ModAcc
        frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
        frequencyReferenceFrequency = 10000000.0
        ' Hz 
        centerFrequency = 1950000000.0
        ' Hz 
        externalAttenuation = 0.0
        ' dB 

        digitalEdgeTriggerSource = RFmxWcdmaMXConstants.Pfi0
        digitalEdgeTriggerEdge = RFmxWcdmaMXDigitalEdgeTriggerEdge.Rising
        triggerDelay = 0.0
        ' seconds 
        referenceLevel = 0.0
        ' dBm 

        synchronizationMode = RFmxWcdmaMXModAccSynchronizationMode.Slot
        measurementOffset = 0
        'slots
        measurementLength = 1
        'slots
        carrierAtCenterFrequency = -1

        enableAllTraces = True
        enableTrigger = False

        timeout = 10.0
    End Sub

    Private Sub InitializeInstr()
        ' Create a new RFmx Session 

        instrSession = New RFmxInstrMX(resourceName, "")
    End Sub

    Private Sub ConfigureWcdma()
        ' Get Wcdma signal 

        wcdma = instrSession.GetWcdmaSignalConfiguration()

        ' Configure measurement 

        instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
        wcdma.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
        wcdma.ConfigureDigitalEdgeTrigger("", digitalEdgeTriggerSource, digitalEdgeTriggerEdge, triggerDelay, enableTrigger)
        wcdma.ConfigureContiguousCarriers("", numberOfCarriers, carrierAtCenterFrequency)
        wcdma.ConfigureUplinkScramblingArray("", uplinkScramblingType, uplinkScramblingCode)

        wcdma.SelectMeasurements("", measurement, enableAllTraces)

        wcdma.ModAcc.Configuration.ConfigureSynchronizationModeAndInterval("", synchronizationMode, measurementOffset, measurementLength)
        wcdma.Initiate("", "")
    End Sub

    Private Sub RetrieveResults()
        Dim carrierString As String

        wcdma.ModAcc.Results.FetchEvmArray("", timeout, rmsEvm, peakEvm, rho, frequencyError, _
         chipRateError, rmsMagnitudeError, rmsPhaseError)
        wcdma.ModAcc.Results.FetchIQImpairmentsArray("", timeout, iqOriginOffset, iqGainImbalance, iqQuadratureError)
        wcdma.ModAcc.Results.FetchPeakCdeArray("", timeout, peakCde, peakCdeCode, peakCdeBranch)
        wcdma.ModAcc.Results.FetchPeakActiveCdeArray("", timeout, peakActiveCde, peakActiveCdeSpreadingFactor, peakActiveCdeCode, peakActiveCdeBranch)
        wcdma.ModAcc.Results.FetchRcdeArray("", timeout, peakRcde, peakRcdeSpreadingFactor, peakRcdeCode, peakRcdeBranch)
        For i As Integer = 0 To numberOfCarriers - 1
            carrierString = RFmxWcdmaMX.BuildCarrierString("", i)
            wcdma.ModAcc.Results.FetchEvmTrace(carrierString, timeout, carrierChannelOutput(i).evm)
            wcdma.ModAcc.Results.FetchConstellationTrace("", timeout, carrierChannelOutput(i).constellation)
        Next
    End Sub

    Private Sub PrintResults()

        Console.WriteLine("-----------------------------EVM------------------------" & vbLf)
        For i As Integer = 0 To numberOfCarriers - 1
            Console.WriteLine(vbLf & "Carrier {0}", i)
            Console.WriteLine("RMS EVM (%)                                      : {0}", rmsEvm(i))
            Console.WriteLine("Peak EVM (%)                                     : {0}", peakEvm(i))
            Console.WriteLine("Rho                                              : {0}", rho(i))
            Console.WriteLine("Frequency Error (Hz)                             : {0}", frequencyError(i))
            Console.WriteLine("Chip Rate Error (ppm)                            : {0}", chipRateError(i))
            Console.WriteLine("RMS Magnitude Error (%)                          : {0}", rmsMagnitudeError(i))
            Console.WriteLine("RMS Phase Error (deg)                            : {0}", rmsPhaseError(i))
        Next

        Console.WriteLine(vbLf & "---------------------IQ Impairments------------------" & vbLf)
        For i As Integer = 0 To numberOfCarriers - 1
            Console.WriteLine(vbLf & "Carrier {0}", i)
            Console.WriteLine(vbLf & "I/Q Origin Offset (dB)                           : {0}", iqOriginOffset(i))
            Console.WriteLine("I/Q Gain Imbalance (dB)                          : {0}", iqGainImbalance(i))
            Console.WriteLine("I/Q Quadrature Error (deg)                       : {0}", iqQuadratureError(i))
        Next

        Console.WriteLine(vbLf & "---------------------Peak CDE----------------------------")
        For i As Integer = 0 To numberOfCarriers - 1
            Console.WriteLine(vbLf & "Carrier {0}", i)
            Console.WriteLine("Peak CDE (dB)                                    : {0}", peakCde(i))
            Console.WriteLine("Peak CDE Code                                    : {0}", peakCdeCode(i))
            Console.WriteLine("Peak CDE Branch                                  : {0}", peakCdeBranch(i))
        Next

        Console.WriteLine(vbLf & "----------------------Peak Active CDE----------------" & vbLf)
        For i As Integer = 0 To numberOfCarriers - 1
            Console.WriteLine(vbLf & "Carrier {0}", i)
            Console.WriteLine("Peak Active CDE (dB)                             : {0}", peakActiveCde(i))
            Console.WriteLine("Peak Active CDE Spreading Factor                 : {0}", peakActiveCdeSpreadingFactor(i))
            Console.WriteLine("Peak Active CDE Code                             : {0}", peakActiveCdeCode(i))
            Console.WriteLine("Peak Active CDE Branch                           : {0}", peakActiveCdeBranch(i))
        Next

        Console.WriteLine(vbLf & "------------Peak RCDE------------" & vbLf)
        For i As Integer = 0 To numberOfCarriers - 1
            Console.WriteLine(vbLf & "Carrier {0}", i)
            Console.WriteLine("Peak RCDE (dB)                                   : {0}", peakRcde(i))
            Console.WriteLine("Peak RCDE Spreading Factor                       : {0}", peakRcdeSpreadingFactor(i))
            Console.WriteLine("Peak RCDE Code                                   : {0}", peakRcdeCode(i))
            Console.WriteLine("Peak RCDE Branch                                 : {0}", peakRcdeBranch(i))
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
