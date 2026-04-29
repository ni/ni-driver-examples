' Steps:
' 1.Open a new RFmx Session.
' 2.Configure Frequency Reference.
' 3.Configure basic signal properties (Reference Level and External Attenuation).
' 4.Configure Trigger Type and Trigger Parameters.
' 5. Configure Subblock Configurations.
' 5A.Configure Number of Subblocks.
' 5B.Configure Subblock Frequency.
' 5C.Configure Component Carrier Spacing.
' 5D.Configure Number of Component Carriers.
' 5E.Configure Component Carriers (Component Carrier Frequency and Component Carrier Bandwidth).
' 6.Configure Duplex Scheme.
' 7.Select SlotPhase measurement and enable Traces.
' 8.Configure Measurement Interval.
' 9.Initiate the Measurement.
' 10. Fetch SlotPhase Measurements and Traces.
' 11. Close RFmx Session. 

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.LteMX

Public Class RFmxLteULSlotPhaseNonContiguousMultiCarrier
    Private instrSession As RFmxInstrMX
    Private lte As RFmxLteMX
    Private resourceName As String
    Private frequencyReferenceSource As String
    Private frequencyReferenceFrequency As Double

    Private centerFrequency As Double
    Private referenceLevel As Double
    Private externalAttenuation As Double

    Private enableTrigger As Boolean
    Private digitalEdgeTriggerSource As String
    Private digitalEdgeTriggerEdge As RFmxLteMXDigitalEdgeTriggerEdge
    Private triggerDelay As Double

    Const numberOfSlots As Integer = 20
    Const numberOfSubblocks As Integer = 2
    Const numberOfComponentCarriers As Integer = 1

    Private duplexScheme As RFmxLteMXDuplexScheme
    Private synchronizationMode As RFmxLteMXSlotPhaseSynchronizationMode
    Private measurementOffset As Integer
    Private measurementLength As Integer

    ' Subblock inputs structure 

    Private Structure SubblockInput
        Public subblockFrequency As Double
        '(Hz)
        Public componentCarrierSpacingType As RFmxLteMXComponentCarrierSpacingType
        Public componentCarrierAtCenterFrequency As Integer
        Public componentCarrierBandwidth As Double()
        '(Hz) 
        Public componentCarrierFrequency As Double()
        '(Hz) 
        Public cellID As Integer()
    End Structure

    ' Subblock measurement outputs structure 

    Private Structure SubblockMeasurement
        Public maximumPhaseDiscontinuity As Double()
        '(deg)
        Public slotPhaseDiscontinuity As Double()
        '(dBm) 
        Public samplePhaseError As AnalogWaveform(Of Single)
        ' deg 
        Public samplePhaseErrorLinearFit As AnalogWaveform(Of Single)
        ' deg 
    End Structure

    Private subblocks As SubblockInput() = New SubblockInput(numberOfSubblocks - 1) {New SubblockInput() With {
     .subblockFrequency = 0.0,
     .componentCarrierSpacingType = RFmxLteMXComponentCarrierSpacingType.Nominal,
     .componentCarrierAtCenterFrequency = -1,
     .componentCarrierBandwidth = New Double(numberOfComponentCarriers - 1) {20000000.0},
     .componentCarrierFrequency = New Double(numberOfComponentCarriers - 1) {0.0},
     .cellID = New Integer(numberOfComponentCarriers - 1) {0}
    }, New SubblockInput() With {
     .subblockFrequency = 30000000.0,
     .componentCarrierSpacingType = RFmxLteMXComponentCarrierSpacingType.Nominal,
     .componentCarrierAtCenterFrequency = -1,
     .componentCarrierBandwidth = New Double(numberOfComponentCarriers - 1) {20000000.0},
     .componentCarrierFrequency = New Double(numberOfComponentCarriers - 1) {0.0},
     .cellID = New Integer(numberOfComponentCarriers - 1) {1}
    }}
    Private subblockMsr As SubblockMeasurement() = New SubblockMeasurement(numberOfSubblocks - 1) {}

    Private subblockString As String() = New String(numberOfSubblocks - 1) {}, subblockCarrierString As String() = New String(numberOfSubblocks - 1) {}
    Private uplinkDownlinkConfiguraiton As RFmxLteMXUplinkDownlinkConfiguration
    Private timeout As Double


    Public Sub Run()
        Try
            InitializeVariables()
            InitializeInstr()
            ConfigureLte()
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

    Private Sub InitializeInstr()
        ' Create a new RFmx Session 

        instrSession = New RFmxInstrMX(resourceName, "")
    End Sub

    Private Sub InitializeVariables()
        resourceName = "RFSA"
        frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
        frequencyReferenceFrequency = 10000000.0
        ' (Hz) 

        centerFrequency = 1950000000.0
        ' (Hz) 
        referenceLevel = 0.0
        ' (dBm) 
        externalAttenuation = 0.0
        ' (dBm) 

        enableTrigger = False
        digitalEdgeTriggerSource = RFmxLteMXConstants.Pfi0
        digitalEdgeTriggerEdge = RFmxLteMXDigitalEdgeTriggerEdge.Rising
        triggerDelay = 0.0
        ' (s) 

        duplexScheme = RFmxLteMXDuplexScheme.Fdd
        synchronizationMode = RFmxLteMXSlotPhaseSynchronizationMode.Slot
        measurementOffset = 0
        ' slots 
        measurementLength = numberOfSlots
        ' slots 

        uplinkDownlinkConfiguraiton = RFmxLteMXUplinkDownlinkConfiguration.Configuration0
        timeout = 10.0
        ' (s) 
    End Sub

    Private Sub ConfigureLte()
        ' Get Lte signal 

        lte = instrSession.GetLteSignalConfiguration()

        ' Configure measurement 

        instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
        lte.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
        lte.ConfigureDigitalEdgeTrigger("", digitalEdgeTriggerSource, digitalEdgeTriggerEdge, triggerDelay, enableTrigger)
        lte.ConfigureNumberOfSubblocks("", numberOfSubblocks)

        For i As Integer = 0 To numberOfSubblocks - 1
            subblockString(i) = RFmxLteMX.BuildSubblockString("", i)

            lte.SetSubblockFrequency(subblockString(i), subblocks(i).subblockFrequency)
            lte.ComponentCarrier.ConfigureSpacing(subblockString(i), subblocks(i).componentCarrierSpacingType,
                                                  subblocks(i).componentCarrierAtCenterFrequency)
            lte.ConfigureNumberOfComponentCarriers(subblockString(i), numberOfComponentCarriers)
            lte.ComponentCarrier.ConfigureArray(subblockString(i), subblocks(i).componentCarrierBandwidth,
                                                subblocks(i).componentCarrierFrequency, subblocks(i).cellID)
        Next

        lte.ConfigureDuplexScheme("", duplexScheme, uplinkDownlinkConfiguraiton)
        lte.SelectMeasurements("", RFmxLteMXMeasurementTypes.SlotPhase, True)
        lte.SlotPhase.Configuration.ConfigureSynchronizationModeAndInterval("", synchronizationMode, measurementOffset, measurementLength)
        lte.Initiate("", "")
    End Sub

    Private Sub RetrieveResults()
        ' Retrieve results 

        For i As Integer = 0 To numberOfSubblocks - 1
            lte.SlotPhase.Results.FetchMaximumPhaseDiscontinuityArray(subblockString(i), timeout, subblockMsr(i).maximumPhaseDiscontinuity)

            subblockCarrierString(i) = RFmxLteMX.BuildCarrierString(subblockString(i), 0)
            lte.SlotPhase.Results.FetchPhaseDiscontinuities(subblockCarrierString(i), timeout, subblockMsr(i).slotPhaseDiscontinuity)
            lte.SlotPhase.Results.FetchSamplePhaseError(subblockCarrierString(i), timeout, subblockMsr(i).samplePhaseError)
            lte.SlotPhase.Results.FetchSamplePhaseErrorLinearFitTrace(subblockCarrierString(i), timeout,
                                                                      subblockMsr(i).samplePhaseErrorLinearFit)
        Next
    End Sub

    Private Sub PrintResults()
        Console.WriteLine("********** Subblock Measurements ********** " & vbLf)
        For i As Integer = 0 To numberOfSubblocks - 1
            Console.WriteLine("Subblock                             : {0}", i)
            For j As Integer = 0 To numberOfComponentCarriers - 1
                Console.WriteLine("Carrier                              : {0}", j)
                Console.WriteLine("Maximum  Phase Discontinuity (deg)   : {0}" & vbLf, subblockMsr(i).maximumPhaseDiscontinuity(j))
            Next
        Next
    End Sub

    Private Sub CloseSession()
        Try
            If lte IsNot Nothing Then
                lte.Dispose()
                lte = Nothing
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
