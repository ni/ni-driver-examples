'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
'4. Configure Trigger Type and Trigger Parameters.
'5. Select SlotPhase measurement and enable Traces.
'6. Configure Radio Configuration and Uplink Spreading Long code mask.
'7. Configure Synchronization Mode and Interval
'8. Initiate the Measurement.
'9. Fetch SlotPhase Measurements and Traces.
'10. Close RFmx Session.  

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.Cdma2kMX

Public Class RFmxCdma2kSlotPhase
    Private instrSession As RFmxInstrMX
    Private cdma2k As RFmxCdma2kMX

    Private resourceName As String
    Private measurement As RFmxCdma2kMXMeasurementTypes
    Private frequencyReferenceSource As String
    Private frequencyReferenceFrequency As Double
    ' Hz 
    Private centerFrequency As Double
    ' Hz 
    Private externalAttenuation As Double
    ' dB 

    Private digitalEdgeSource As String
    Private digitalEdge As RFmxCdma2kMXDigitalEdgeTriggerEdge
    Private triggerDelay As Double
    ' seconds 
    Private referenceLevel As Double
    ' dBm 

    Private radioConfiguration As RFmxCdma2kMXRadioConfiguration
    Private uplinkSpreadingLongCodeMask As Integer
    Private synchronizationMode As RFmxCdma2kMXSlotPhaseSynchronizationMode
    Private measurementOffset As Integer
    'slots
    Private measurementLength As Integer
    'slots


    Private enableAllTraces As Boolean
    Private enableTrigger As Boolean

    Private timeout As Double

    Private maximumPhaseDiscontinuity As Double
    Private slotPhaseDiscontinuity As Double() = Nothing
    Private chipPhaseError As AnalogWaveform(Of Single) = Nothing
    Private chipPhaseErrorLinearFit As AnalogWaveform(Of Single) = Nothing



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
            Console.WriteLine("Press any key to exit")
            Console.ReadKey()
        End Try
    End Sub

    Private Sub InitializeVariables()
        ' Initialize input variables 


        resourceName = "RFSA"
        measurement = RFmxCdma2kMXMeasurementTypes.SlotPhase
        frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
        frequencyReferenceFrequency = 10000000.0
        ' Hz 
        centerFrequency = 833490000.0
        ' Hz 
        externalAttenuation = 0.0
        ' dB 

        digitalEdgeSource = RFmxCdma2kMXConstants.Pfi0
        digitalEdge = RFmxCdma2kMXDigitalEdgeTriggerEdge.Rising
        triggerDelay = 0.0
        ' seconds 
        referenceLevel = 0.0
        ' dBm 

        radioConfiguration = RFmxCdma2kMXRadioConfiguration.RC3
        uplinkSpreadingLongCodeMask = 0
        synchronizationMode = RFmxCdma2kMXSlotPhaseSynchronizationMode.Slot
        measurementOffset = 0
        'slots
        measurementLength = 16
        'slots


        enableAllTraces = True
        enableTrigger = False

        timeout = 10.0

    End Sub

    Private Sub InitializeInstr()
        ' Create a new RFmx Session 

        instrSession = New RFmxInstrMX(resourceName, "")
    End Sub

    Private Sub ConfigureCdma2k()
        cdma2k = instrSession.GetCdma2kSignalConfiguration()
        instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
        cdma2k.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
        cdma2k.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger)
        cdma2k.ConfigureRadioConfiguration("", radioConfiguration)
        cdma2k.ConfigureUplinkSpreading("", uplinkSpreadingLongCodeMask)
        cdma2k.SelectMeasurements("", measurement, enableAllTraces)
        cdma2k.SlotPhase.Configuration.ConfigureSynchronizationModeAndInterval("", synchronizationMode, measurementOffset, measurementLength)
        cdma2k.Initiate("", "")
    End Sub

    Private Sub RetrieveResults()
        cdma2k.SlotPhase.Results.FetchMaximumPhaseDiscontinuity("", timeout, maximumPhaseDiscontinuity)
        cdma2k.SlotPhase.Results.FetchPhaseDiscontinuities("", timeout, slotPhaseDiscontinuity)
        cdma2k.SlotPhase.Results.FetchChipPhaseErrorTrace("", timeout, chipPhaseError)
        cdma2k.SlotPhase.Results.FetchChipPhaseErrorLinearFitTrace("", timeout, chipPhaseErrorLinearFit)
    End Sub

    Private Sub PrintResults()
        Console.WriteLine("Maximum Phase Discontinuity (deg)         : {0}" & vbLf, maximumPhaseDiscontinuity)

        For i As Integer = 0 To measurementLength - 1
            Console.WriteLine(vbLf & "Slot Number                               : {0}", i)
            Console.WriteLine("Slot Phase Discontinuity (deg)            : {0}", slotPhaseDiscontinuity(i))
        Next

    End Sub

    Private Sub CloseSession()
        If cdma2k IsNot Nothing Then
            cdma2k.Dispose()
            cdma2k = Nothing
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
