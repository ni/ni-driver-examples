'Steps:
'1. Open a new RFmx session.
'2. Configure Frequency Reference.
'3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
'4. Configure Trigger Type and Trigger Parameters..
'5. Select CDA measurement and enable traces.
'6. Configure Synchronization Mode and Offset.
'7. Configure Measurement Channel.
'8. Configure Power Unit.
'9. Initiate the Measurement.
'10. Fetch CDA Measurements and Traces.
'11. Close the RFmx session.


Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.TdscdmaMX

Namespace NationalInstruments.Examples.RFmxTdscdmaCda
    Public Class RFmxTdscdmaCda
        Private instrSession As RFmxInstrMX
        Private tdscdma As RFmxTdscdmaMX

        Private resourceName As String, iqPowerEdgeTriggerSource As String, frequencyReferenceSource As String
        Private centerFrequency As Double, referenceLevel As Double, externalAttenuation As Double, minimumQuietTimeDuration As Double, frequencyReferenceFrequency As Double, triggerDelay As Double, _
         iqPowerEdgeTriggerLevel As Double

        Private minimumQuietTimeMode As RFmxTdscdmaMXTriggerMinimumQuietTimeMode
        Private enableTrigger As Boolean
        Private iqPowerEdgeTriggerLevelType As RFmxTdscdmaMXIQPowerEdgeTriggerLevelType
        Private iqPowerEdgeTriggerSlope As RFmxTdscdmaMXIQPowerEdgeTriggerSlope


        Private synchronizationMode As RFmxTdscdmaMXCdaSynchronizationMode
        Private powerUnit As RFmxTdscdmaMXCdaPowerUnit
        Private midambleMode As RFmxTdscdmaMXMidambleAutoDetectionMode
        Private averagingEnabled As RFmxTdscdmaMXCdaAveragingEnabled
        Private measurementOffset As Integer, SpreadingFactor As Integer, midambleShift As Integer, channelizationCode As Integer, uplinkScramblingCode As Integer
        Private averagingCount As Integer, maximumNumberOfUsers As Integer

        Private timeout As Double
        Private meanRmsSymbolMagnitudeError As Double, maximumPeakSymbolEvm As Double, frequencyError As Double, chipRateError As Double
        Private meanRmsSymbolEvm As Double, meanRmsSymbolPhaseError As Double
        Private meanSymbolPower As Double, meanTotalPower As Double, meanActivePower As Double, maximumPeakActivePower As Double, meanInactivePower As Double, maximumPeakInactivePower As Double
        Private meanTotalActivePower As Double, iqOriginOffset As Double, iqGainImbalance As Double, iqQuadratureError As Double

        Private meanCodeDomainPowers As Single(), meanSymbolEvm As Single()
        Private symbolConstellation As ComplexSingle()

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
            resourceName = "RFSA"
            frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
            frequencyReferenceFrequency = 10000000.0
            ' Hz 
            centerFrequency = 1910000000.0
            ' Hz 
            referenceLevel = 0.0
            ' dBm 
            externalAttenuation = 0.0
            ' dB 
            enableTrigger = True
            SpreadingFactor = 16
            iqPowerEdgeTriggerSource = "0"
            iqPowerEdgeTriggerSlope = RFmxTdscdmaMXIQPowerEdgeTriggerSlope.Rising
            iqPowerEdgeTriggerLevel = -20.0
            'dB
            iqPowerEdgeTriggerLevelType = RFmxTdscdmaMXIQPowerEdgeTriggerLevelType.Relative
            triggerDelay = 0.0
            ' seconds 
            midambleMode = RFmxTdscdmaMXMidambleAutoDetectionMode.MidambleShift
            maximumNumberOfUsers = 16
            midambleShift = 8
            channelizationCode = 1
            averagingEnabled = RFmxTdscdmaMXCdaAveragingEnabled.[False]
            averagingCount = 10
            uplinkScramblingCode = 0
            powerUnit = RFmxTdscdmaMXCdaPowerUnit.dB
            minimumQuietTimeMode = RFmxTdscdmaMXTriggerMinimumQuietTimeMode.Auto
            minimumQuietTimeDuration = 0.000016
            ' seconds 
            synchronizationMode = RFmxTdscdmaMXCdaSynchronizationMode.Slot
            measurementOffset = 0
            timeout = 10.0
            ' seconds 
        End Sub

        Private Sub InitializeInstr()
            ' Create a new RFmx Session 

            instrSession = New RFmxInstrMX(resourceName, "")
        End Sub

        Private Sub ConfigureTdscdma()
            ' Get Tdscdma signal 

            tdscdma = instrSession.GetTdscdmaSignalConfiguration()

            ' Configure measurement 

            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
            tdscdma.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
            tdscdma.ConfigureIQPowerEdgeTrigger("", iqPowerEdgeTriggerSource, iqPowerEdgeTriggerSlope, iqPowerEdgeTriggerLevel,
                                                triggerDelay, minimumQuietTimeMode, minimumQuietTimeDuration, iqPowerEdgeTriggerLevelType, enableTrigger)
            tdscdma.SelectMeasurements("", RFmxTdscdmaMXMeasurementTypes.Cda, True)
            tdscdma.Cda.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount)
            tdscdma.Cda.Configuration.ConfigureSynchronizationModeAndOffset("", synchronizationMode, measurementOffset)
            tdscdma.Cda.Configuration.ConfigureMeasurementChannel("", SpreadingFactor, channelizationCode)
            tdscdma.Cda.Configuration.ConfigurePowerUnit("", powerUnit)
            tdscdma.ConfigureMidambleShift("", midambleMode, maximumNumberOfUsers, midambleShift)
            tdscdma.ConfigureUplinkScramblingCode("", uplinkScramblingCode)
            tdscdma.Initiate("", "")
        End Sub

        Private Sub RetrieveResults()
            tdscdma.Cda.Results.FetchIQImpairments("", timeout, iqOriginOffset, iqGainImbalance, iqQuadratureError)
            tdscdma.Cda.Results.FetchCodeDomainPower("", timeout, meanTotalPower, meanTotalActivePower, meanActivePower, maximumPeakActivePower, _
             meanInactivePower, maximumPeakInactivePower)
            tdscdma.Cda.Results.FetchSymbolEvm("", timeout, meanRmsSymbolEvm, maximumPeakSymbolEvm, frequencyError, chipRateError, _
             meanRmsSymbolMagnitudeError, meanRmsSymbolPhaseError, meanSymbolPower)
            tdscdma.Cda.Results.FetchMeanSymbolEvmTrace("", timeout, meanSymbolEvm)
            tdscdma.Cda.Results.FetchSymbolConstellationTrace("", timeout, symbolConstellation)
            tdscdma.Cda.Results.FetchMeanCodeDomainPowerTrace("", timeout, meanCodeDomainPowers)
        End Sub

        Private Sub PrintResults()
            Console.WriteLine("--------------------Code Domain Power--------------------")

            Console.WriteLine("Mean Total Power (dBm)                    {0}", meanTotalPower)

            Console.WriteLine("Mean Total Active Power (dB or dBm)       {0}", meanTotalActivePower)

            Console.WriteLine("Mean Active Power (dB or dBm)             {0}", meanActivePower)

            Console.WriteLine("Maximum Peak Active Power (dB or dBm)     {0}", maximumPeakActivePower)

            Console.WriteLine("Mean Inactive Power (dB or dBm)           {0}", meanInactivePower)

            Console.WriteLine("Maximum Peak Inactive Power (dB or dBm)   {0}", maximumPeakInactivePower)

            Console.WriteLine(vbLf & "--------------------Symbol EVM--------------------")

            Console.WriteLine("Mean RMS Symbol EVM (%)                   {0}", meanRmsSymbolEvm)

            Console.WriteLine("Maximum Peak Symbol EVM (%)               {0}", maximumPeakSymbolEvm)

            Console.WriteLine("Frequency  error (Hz)                     {0}", frequencyError)

            Console.WriteLine("Chip Rate Error (ppm)                     {0}", chipRateError)

            Console.WriteLine("Mean RMS Symbol Magnitude Error (%)       {0}", meanRmsSymbolMagnitudeError)

            Console.WriteLine("Mean RMS Symbol Phase error (deg)         {0}", meanRmsSymbolPhaseError)

            Console.WriteLine("Mean Symbol Power (dB or dBm)             {0}", meanSymbolPower)

            Console.WriteLine(vbLf & "--------------------IQ Impairments--------------------")

            Console.WriteLine("IQ Origin Offset (dB)                     {0}", iqOriginOffset)

            Console.WriteLine("IQ Gain Imbalance (dB)                    {0}", iqGainImbalance)

            Console.WriteLine("IQ Quadrature Error (deg)                 {0}", iqQuadratureError)

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
End Namespace

