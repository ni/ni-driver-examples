'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
'4. Configure Trigger Type and Trigger Parameters.
'5. Configure Component Carrier Spacing.
'6. Configure Component Carriers.
'7. Configure Duplex Scheme.
'8. Select SlotPower measurement and enable Traces.
'9. Configure Measurement Method.
'10. Initiate the Measurement.
'11. Fetch SlotPower Measurements and Traces.
'12. Close RFmx Session. 

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.LteMX

Public Class RFmxLteULSlotPowerContiguousMultiCarrier
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

    Const numberOfComponentCarriers As Integer = 2

    Private duplexScheme As RFmxLteMXDuplexScheme
    Private measurementOffset As Integer
    Private measurementLength As Integer

    Private componentCarrierSpacingType As RFmxLteMXComponentCarrierSpacingType
    Private componentCarrierAtCenterFrequency As Integer

    Private componentCarrierBandwidth As Double() = {20000000.0, 20000000.0}
    '(Hz) 
    Private componentCarrierFrequency As Double() = {-9900000.0, 9900000.0}
    '(Hz) 
    Private cellId As Integer() = {0, 1}

    Private uplinkDownlinkConfiguraiton As RFmxLteMXUplinkDownlinkConfiguration
    Private carrierString As String
    Private timeout As Double

    Private subframePower As Double()() = New Double(numberOfComponentCarriers - 1)() {}
    Private subframePowerDelta As Double()() = New Double(numberOfComponentCarriers - 1)() {}

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
        ' (dB) 

        enableTrigger = False
        digitalEdgeTriggerSource = RFmxLteMXConstants.Pfi0
        digitalEdgeTriggerEdge = RFmxLteMXDigitalEdgeTriggerEdge.Rising
        triggerDelay = 0.0
        ' (s) 

        duplexScheme = RFmxLteMXDuplexScheme.Fdd

        measurementOffset = 0
        ' subframes 
        measurementLength = 10
        ' subframes 

        componentCarrierSpacingType = RFmxLteMXComponentCarrierSpacingType.Nominal
        componentCarrierAtCenterFrequency = -1
        uplinkDownlinkConfiguraiton = RFmxLteMXUplinkDownlinkConfiguration.Configuration0
        timeout = 10.0
        ' (s) 
    End Sub

    Private Sub InitializeInstr()
        ' Create a new RFmx Session 

        instrSession = New RFmxInstrMX(resourceName, "")
    End Sub

    Private Sub ConfigureLte()

        ' Get Lte signal 

        lte = instrSession.GetLteSignalConfiguration()

        ' Configure measurement 

        instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
        lte.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
        lte.ConfigureDigitalEdgeTrigger("", digitalEdgeTriggerSource, digitalEdgeTriggerEdge, triggerDelay, enableTrigger)
        lte.ComponentCarrier.ConfigureSpacing("", componentCarrierSpacingType, componentCarrierAtCenterFrequency)
        lte.ConfigureNumberOfComponentCarriers("", numberOfComponentCarriers)
        lte.ComponentCarrier.ConfigureArray("", componentCarrierBandwidth, componentCarrierFrequency, cellId)
        lte.ConfigureDuplexScheme("", duplexScheme, uplinkDownlinkConfiguraiton)
        lte.SelectMeasurements("", RFmxLteMXMeasurementTypes.SlotPower, True)
        lte.SlotPower.Configuration.ConfigureMeasurementInterval("", measurementOffset, measurementLength)
        lte.Initiate("", "")
    End Sub

    Private Sub RetrieveResults()
        ' Retrieve results 


        For i As Integer = 0 To numberOfComponentCarriers - 1
            carrierString = RFmxLteMX.BuildCarrierString("", i)
            lte.SlotPower.Results.FetchPowers(carrierString, timeout, subframePower(i), subframePowerDelta(i))
        Next
    End Sub

    Private Sub PrintResults()
        ' Retrieve results 

        For i As Integer = 0 To numberOfComponentCarriers - 1
            Console.WriteLine(vbLf & vbLf & "CC {0} Trace", i)
            Console.WriteLine("Subframe Power(dBm)")
            PrintArray(subframePower(i))
            Console.WriteLine()
            Console.WriteLine(vbLf & "Subframe Power Delta(dB)")
            PrintArray(subframePowerDelta(i))
        Next
    End Sub

    Private Sub PrintArray(arrayToPrint As Double())
        Dim i As Integer = 0, length As Integer = arrayToPrint.Length
        For i = 0 To length - 1
            If i = (length - 1) Then
                Console.Write(arrayToPrint(i))
            Else
                Console.Write(arrayToPrint(i) & ",")
            End If
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
