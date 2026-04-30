'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure WCDMA centre frequency.
'4. Configure Trigger Type and Trigger Parameters.
'5. Select SlotPhase measurement and enable Traces.
'6. Configure Uplink Scrambling.
'7. Configure Synchronisation Mode and Interval
'8. Initiate the Measurement.
'9. Fetch SlotPhase Measurements and Traces.
'10. Close RFmx Session. 

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.WcdmaMX

Public Class RFmxWcdmaSlotPhase
	Private instrSession As RFmxInstrMX
	Private wcdma As RFmxWcdmaMX

    Private resourceName As String
    Private measurement As RFmxWcdmaMXMeasurementTypes
    Private frequencyReferenceSource As String
    Private frequencyReferenceFrequency As Double
    ' Hz 
    Private centerFrequency As Double
    ' Hz 
    Private externalAttenuation As Double
    ' dB 

    Private digitalEdgeSource As String
    Private digitalEdge As RFmxWcdmaMXDigitalEdgeTriggerEdge
    Private triggerDelay As Double
    ' seconds 
    Private referenceLevel As Double
    ' dBm 

    Private synchronisationMode As RFmxWcdmaMXSlotPhaseSynchronizationMode
    Private measurementOffset As Integer
    'slots
    Private measurementLength As Integer
    'slots

    Private uplinkScramblingType As RFmxWcdmaMXUplinkScramblingType
    Private uplinkScramblingCode As Integer

    Private enableAllTraces As Boolean
    Private enableTrigger As Boolean

    Private timeout As Double

    Private discontinuityMinimumDistance As Integer
    'slots
    Private discontinuityCountGreaterThanlimit1 As Integer
    Private discontinuityCountGreaterThanlimit2 As Integer
    Private maximumPhaseDiscontinuity As Double
    'deg

    Private slotPhaseDiscontinuity As Double() = Nothing
    Private chipPhaseError As AnalogWaveform(Of Single) = Nothing
    Private chipPhaseErrorLinearFit As AnalogWaveform(Of Single) = Nothing



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
        ' Initialize input variables 


        resourceName = "RFSA"
        measurement = RFmxWcdmaMXMeasurementTypes.SlotPhase
        frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
        frequencyReferenceFrequency = 10000000.0
        ' Hz 
        centerFrequency = 1950000000.0
        ' Hz 
        externalAttenuation = 0.0
        ' dB 

        digitalEdgeSource = RFmxWcdmaMXConstants.Pfi0
        digitalEdge = RFmxWcdmaMXDigitalEdgeTriggerEdge.Rising
        triggerDelay = 0.0
        ' seconds 
        referenceLevel = 0.0
        ' dBm 

        synchronisationMode = RFmxWcdmaMXSlotPhaseSynchronizationMode.Slot
        measurementOffset = 0
        'slots
        measurementLength = 15
        'slots

        uplinkScramblingType = RFmxWcdmaMXUplinkScramblingType.[Long]
        uplinkScramblingCode = 0

        enableAllTraces = True
        enableTrigger = False

        timeout = 10.0

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

        wcdma.SelectMeasurements("", measurement, enableAllTraces)

        wcdma.ConfigureUplinkScrambling("", uplinkScramblingCode, uplinkScramblingType)
        wcdma.SlotPhase.Configuration.ConfigureSynchronizationModeAndInterval("", synchronisationMode, measurementOffset, measurementLength)
        wcdma.Initiate("", "")
    End Sub

    Private Sub RetrieveResults()
        wcdma.SlotPhase.Results.FetchMeasurement("", timeout, maximumPhaseDiscontinuity, discontinuityCountGreaterThanlimit1, discontinuityCountGreaterThanlimit2, discontinuityMinimumDistance)
        wcdma.SlotPhase.Results.FetchPhaseDiscontinuities("", timeout, slotPhaseDiscontinuity)
        wcdma.SlotPhase.Results.FetchChipPhaseErrorTrace("", timeout, chipPhaseError)
        wcdma.SlotPhase.Results.FetchChipPhaseErrorLinearFitTrace("", timeout, chipPhaseErrorLinearFit)
    End Sub

    Private Sub PrintResults()
        Console.WriteLine("------------Measurement------------" & vbLf)
        Console.WriteLine("Maximum Phase Discontinuity (deg)        : {0}" & vbLf, maximumPhaseDiscontinuity)
        Console.WriteLine("Discontinuity Count > Limit1             : {0}" & vbLf, discontinuityCountGreaterThanlimit1)
        Console.WriteLine("Discontinuity Count > Limit2             : {0}" & vbLf, discontinuityCountGreaterThanlimit2)
        Console.WriteLine("Discontinuity Minimum Distance (slots)   : {0}" & vbLf, discontinuityMinimumDistance)

        Console.WriteLine("Slot Phase Discontinuity (deg)           :")
        For i As Integer = 0 To measurementLength - 1
            Console.WriteLine("{0} : {1}", i, slotPhaseDiscontinuity(i))
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
