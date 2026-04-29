'Steps:
'1. Open a new RFmx session
'2. Configure the basic instrument properties: Clock Source and Clock Frequency
'3. Configure the basic signal properties: Center Frequency, Reference Level and External Attenuation
'4. Configure the trigger properties
'5. Select ModAcc measurement and enable the traces
'6. Configure Uplink Scrambling Code
'7. Configure the basic Measurement Settings
'8. Configure the Midamble Settings 
'9. Initiate Measurement
'10. Fetch ModAcc Measurements and Traces
'11. Close the RFmx session

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.TdscdmaMX

Public Class RFmxTdscdmaModAcc
    Private instrSession As RFmxInstrMX
    Private tdscdma As RFmxTdscdmaMX
    Private resourceName As String, frequencySource As String, iqPowerEdgeTriggerSource As String
    Private centerFrequency As Double, referenceLevel As Double, externalAttenuation As Double, frequencyReferenceFrequency As Double, triggerDelay As Double, minimumQuietTimeDuration As Double, _
     iqPowerEdgeTriggerLevel As Double

    Private iqPowerEdgeTriggerSlope As RFmxTdscdmaMXIQPowerEdgeTriggerSlope
    Private minimumQuietTimeMode As RFmxTdscdmaMXTriggerMinimumQuietTimeMode
    Private enableTrigger As Boolean
    Private iqPowerEdgeTriggerLevelType As RFmxTdscdmaMXIQPowerEdgeTriggerLevelType


    Private synchronizationMode As RFmxTdscdmaMXModAccSynchronizationMode
    Private measurementOffset As Integer, measurementLength As Integer, uplinkScramblingCode As Integer

    Private midambleAutoDetectionMode As RFmxTdscdmaMXMidambleAutoDetectionMode
    Private maximumNumberOfUsers As Integer, midambleShift As Integer

    Private timeout As Double
    Private rmsCompositeEvm As Double, peakCompositeEvm As Double, compositeRho As Double, frequencyError As Double, chipRateErrorPpm As Double, rmsCompositeMagnitudeError As Double, _
     rmsCompositePhaseError As Double
    Private iqOriginOffset As Double, iqGainImbalance As Double, iqQuadratureError As Double
    Private rmsMidambleEvm As Double, peakMidambleEvm As Double, midambleRho As Double, rmsMidambleMagnitudeError As Double, rmsMidamblePhaseError As Double
    Private midamblePower As Double, dataField1Power As Double, dataField2Power As Double
    Private rmsDataEvm As Double, peakDataEvm As Double, dataRho As Double, rmsDataMagnitudeError As Double, rmsDataPhaseError As Double
    Public Sub Run()
        Try
            InitializeVariables()
            InitializeInstr()
            ConfigureTdscdma()
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

        centerFrequency = 1910000000.0
        ' Hz 
        referenceLevel = 0.0
        ' dBm 
        externalAttenuation = 0.0
        ' dB 

        frequencySource = RFmxInstrMXConstants.OnboardClock
        frequencyReferenceFrequency = 10000000.0
        ' Hz 
        uplinkScramblingCode = 0

        triggerDelay = 0.0
        ' seconds 
        minimumQuietTimeDuration = 0.00008
        ' seconds 
        iqPowerEdgeTriggerLevel = -20.0
        'dB
        iqPowerEdgeTriggerSource = "0"
        iqPowerEdgeTriggerSlope = RFmxTdscdmaMXIQPowerEdgeTriggerSlope.Rising
        enableTrigger = True
        iqPowerEdgeTriggerLevelType = RFmxTdscdmaMXIQPowerEdgeTriggerLevelType.Relative
        minimumQuietTimeMode = RFmxTdscdmaMXTriggerMinimumQuietTimeMode.Auto

        synchronizationMode = RFmxTdscdmaMXModAccSynchronizationMode.Slot

        measurementOffset = 0
        measurementLength = 1
        maximumNumberOfUsers = 16
        midambleShift = 8
        midambleAutoDetectionMode = RFmxTdscdmaMXMidambleAutoDetectionMode.MidambleShift
        timeout = 10
        ' seconds 

    End Sub

    Private Sub InitializeInstr()
        ' Create a new RFmx Session 

        instrSession = New RFmxInstrMX(resourceName, "")
    End Sub

    Private Sub ConfigureTdscdma()
        ' Get SpecAn signal 

        tdscdma = instrSession.GetTdscdmaSignalConfiguration()

        ' Configure measurement 

        instrSession.ConfigureFrequencyReference("", frequencySource, frequencyReferenceFrequency)
        tdscdma.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
        tdscdma.ConfigureIQPowerEdgeTrigger("", iqPowerEdgeTriggerSource, iqPowerEdgeTriggerSlope, iqPowerEdgeTriggerLevel, triggerDelay, minimumQuietTimeMode, _
         minimumQuietTimeDuration, iqPowerEdgeTriggerLevelType, enableTrigger)

        tdscdma.SelectMeasurements("", RFmxTdscdmaMXMeasurementTypes.ModAcc, True)

        tdscdma.ConfigureUplinkScramblingCode("", uplinkScramblingCode)

        tdscdma.ModAcc.Configuration.ConfigureSynchronizationModeAndInterval("", synchronizationMode, measurementOffset, measurementLength)
        tdscdma.ConfigureMidambleShift("", midambleAutoDetectionMode, maximumNumberOfUsers, midambleShift)

        tdscdma.Initiate("", "")
    End Sub

    Private Sub RetrieveResults()
        Dim constellation As ComplexSingle() = Nothing
        Dim evm As AnalogWaveform(Of Single) = Nothing
        ' Retrieve results 

        tdscdma.ModAcc.Results.FetchCompositeEvm("", timeout, rmsCompositeEvm, peakCompositeEvm, compositeRho, frequencyError, _
         chipRateErrorPpm, rmsCompositeMagnitudeError, rmsCompositePhaseError)
        tdscdma.ModAcc.Results.FetchConstellationTrace("", timeout, constellation)
        tdscdma.ModAcc.Results.FetchDataEvm("", timeout, rmsDataEvm, peakDataEvm, dataRho, rmsDataMagnitudeError, _
         rmsDataPhaseError)
        tdscdma.ModAcc.Results.FetchMidambleEvm("", timeout, rmsMidambleEvm, peakMidambleEvm, midambleRho, rmsMidambleMagnitudeError, _
         rmsMidamblePhaseError)
        tdscdma.ModAcc.Results.FetchIQImpairments("", timeout, iqOriginOffset, iqGainImbalance, iqQuadratureError)
        tdscdma.ModAcc.Results.FetchMidambleAndDataPower("", timeout, midamblePower, dataField1Power, dataField2Power)
        tdscdma.ModAcc.Results.FetchEvmTrace("", timeout, evm)
    End Sub

    Private Sub PrintResults()
        Console.WriteLine("--------------------Composite EVM Results--------------------")
        Console.WriteLine("RMS Composite EVM (%)             {0}", rmsCompositeEvm)
        Console.WriteLine("Peak Composite EVM (%)            {0}", peakCompositeEvm)
        Console.WriteLine("Composite Rho                     {0}", compositeRho)
        Console.WriteLine("Frequency Error (Hz)              {0}", frequencyError)
        Console.WriteLine("Chip Rate Error (ppm)             {0}", chipRateErrorPpm)
        Console.WriteLine("RMS Composite Phase Error (deg)   {0}", rmsCompositePhaseError)
        Console.WriteLine("RMS Composite Magnitude Error (%) {0}", rmsCompositeMagnitudeError)

        Console.WriteLine("---------------------Data EVM Results------------------------")
        Console.WriteLine("RMS Data EVM (%)                  {0}", rmsDataEvm)
        Console.WriteLine("Peak Data EVM (%)                 {0}", peakDataEvm)
        Console.WriteLine("Data Rho                          {0}", dataRho)
        Console.WriteLine("RMS Data Phase Error (deg)        {0}", rmsDataPhaseError)
        Console.WriteLine("RMS Data Magnitude Error (%)      {0}", rmsDataMagnitudeError)
        Console.WriteLine("Data Field 1 Power (dBm)          {0}", dataField1Power)
        Console.WriteLine("Data Field 2 Power (dBm)          {0}", dataField2Power)


        Console.WriteLine("---------------------IQ Impairments------------------------")
        Console.WriteLine("I/Q Origin Offset (dB)            {0}", iqOriginOffset)
        Console.WriteLine("I/Q Gain Imbalance (dB)           {0}", iqGainImbalance)
        Console.WriteLine("I/Q Quadrature Error (deg)        {0}", iqQuadratureError)

        Console.WriteLine("-------------------Midable EVM Results------------------------")
        Console.WriteLine("RMS Midamble EVM (%)               {0}", rmsMidambleEvm)
        Console.WriteLine("Peak Midamble EVM (%)              {0}", peakMidambleEvm)
        Console.WriteLine("Midamble Rho                       {0}", midambleRho)
        Console.WriteLine("RMS Midamble Phase Error (deg)     {0}", rmsMidamblePhaseError)
        Console.WriteLine("RMS Midamble Magnitude Error (%)   {0}", rmsMidambleMagnitudeError)
        Console.WriteLine("Midamble Power (dBm)               {0}", midamblePower)


    End Sub

    Private Sub CloseSession()
        Try
            If tdscdma IsNot Nothing Then
                tdscdma.Dispose()
                tdscdma = Nothing
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
