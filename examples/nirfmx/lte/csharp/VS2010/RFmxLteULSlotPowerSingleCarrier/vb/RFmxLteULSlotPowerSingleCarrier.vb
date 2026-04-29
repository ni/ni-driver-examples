'Steps:
'1.Open a new RFmx Session.
'2.Configure Frequency Reference.
'3.Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
'4.Configure Trigger Type and Trigger Parameters.
'5.Configure Carrier Bandwidth.
'6.Configure Duplex Scheme.
'7.Select SlotPhase measurement and enable Traces.
'8.Configure Synchronization Mode and Interval
'9. Initiate the Measurement.
'10. Fetch SlotPhase  Traces and Measurements.
'11. Close RFmx Session.  

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.LteMX

Public Class RFmxLteULSlotPowerSingleCarrier
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

    Private subblockString As String, subblockCarrierString As String
    Private duplexScheme As RFmxLteMXDuplexScheme
    Private uplinkDownlinkConfiguration As RFmxLteMXUplinkDownlinkConfiguration
    Private componentCarrierBandwidth As Double
    Private componentCarrierFrequency As Double
    Private cellID As Integer, i As Integer
    Const numberOfSlots As Integer = 20

    Private measurementOffset As Integer
    Private measurementLength As Integer

    Private timeout As Double

    Private subFramePower As Double(), subFramePowerDelta As Double()


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
        ' (dB) 

        enableTrigger = False
        digitalEdgeTriggerSource = RFmxLteMXConstants.Pfi0
        digitalEdgeTriggerEdge = RFmxLteMXDigitalEdgeTriggerEdge.Rising
        triggerDelay = 0.0
        ' (s) 

        duplexScheme = RFmxLteMXDuplexScheme.Fdd
        uplinkDownlinkConfiguration = RFmxLteMXUplinkDownlinkConfiguration.Configuration0
        componentCarrierBandwidth = 10000000.0
        ' (Hz) 
        componentCarrierFrequency = 0.0
        ' (Hz) 
        cellID = 0

        timeout = 10.0
        ' (s) 
        measurementOffset = 0
        ' subframes 
        measurementLength = numberOfSlots
        ' subframes 

    End Sub

    Private Sub ConfigureLte()
        ' Get Lte signal 

        lte = instrSession.GetLteSignalConfiguration()

        ' Configure measurement 

        instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
        lte.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
        lte.ConfigureDigitalEdgeTrigger("", digitalEdgeTriggerSource, digitalEdgeTriggerEdge, triggerDelay, enableTrigger)
        subblockString = RFmxLteMX.BuildSubblockString("", 0)
        subblockCarrierString = RFmxLteMX.BuildCarrierString(subblockString, 0)

        lte.ComponentCarrier.Configure(subblockCarrierString, componentCarrierBandwidth, componentCarrierFrequency, cellID)
        lte.ConfigureDuplexScheme("", duplexScheme, uplinkDownlinkConfiguration)
        lte.SelectMeasurements("", RFmxLteMXMeasurementTypes.SlotPower, True)
        lte.SlotPower.Configuration.ConfigureMeasurementInterval("", measurementOffset, measurementLength)
        lte.Initiate("", "")
    End Sub

    Private Sub RetrieveResults()
        ' Retrieve results 

        lte.SlotPower.Results.FetchPowers("", timeout, subFramePower, subFramePowerDelta)
    End Sub

    Private Sub PrintResults()
        ' Retrieve results 

        Console.WriteLine("Subframe Power(dBm): " & vbLf)

        For i = 0 To subFramePower.Length - 1
            If i = subFramePower.Length - 1 Then
                Console.Write(subFramePower(i))
            Else
                Console.Write("{0}, ", subFramePower(i))
            End If
        Next

        Console.WriteLine(vbLf & vbLf & "Subframe Power Delta(dB): " & vbLf)

        For i = 0 To subFramePowerDelta.Length - 1
            If i = subFramePowerDelta.Length - 1 Then
                Console.Write(subFramePowerDelta(i))
            Else
                Console.Write("{0}, ", subFramePowerDelta(i))
            End If
        Next
        Console.WriteLine(vbLf)
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
