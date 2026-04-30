'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure basic signal properties (Reference Level and External Attenuation).
'4. Configure Trigger Type and Trigger Parameters.
'5[A-E]. Configure Subblock Configurations.
'Step 5 :
'5A. Configure Number of Subblocks.
'5B. Configure subblock Frequency.
'5C. Configure Component Carrier Spacing.
'5D. Configure Number of Component Carriers.
'5E. Configure Component Carriers (Component Carrier Frequency and Component Carrier Bandwidth).
'6. Select OBW measurement and enable Traces.
'7. Configure Sweep Time Parameters.
'8. Configure Averaging Parameters for OBW measurement.
'9. Initiate the Measurement.
'10. Fetch OBW Measurements and Traces.
'11. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.LteMX

' Input: Subblock inputs structure 

Structure SubblockInput
    Public subblockFrequency As Double
    '(Hz)
    Public componentCarrierSpacingType As RFmxLteMXComponentCarrierSpacingType
    Public componentCarrierAtCenterFrequency As Integer
    Public componentCarrierBandwidth As Double()
    '(Hz) 
    Public componentCarrierFrequency As Double()
    '(Hz) 
End Structure

' Input: Subblock measurement outputs structure 

Structure SubblockMeasurement
    Public startFrequency As Double, stopFrequency As Double, occupiedBandwidth As Double, absolutePower As Double
End Structure

Public Class RFmxLteObwNonContiguousMultiCarrier
    Private instrSession As RFmxInstrMX
    Private lte As RFmxLteMX
    Private rfsaResourceName As String

    Private Const NumberOfComponentCarriers As Integer = 1
    Private Const NumberOfSubblocks As Integer = 2

    Private frequencyReferenceSource As String
    Private frequencyReferenceFrequency As Double

    Private centerFrequency As Double
    Private referenceLevel As Double
    Private externalAttenuation As Double

    Private enableTrigger As Boolean
    Private digitalEdgeSource As String
    Private digitalEdge As RFmxLteMXDigitalEdgeTriggerEdge
    Private triggerDelay As Double

    Private linkDirection As RFmxLteMXLinkDirection
    Private sweepTimeAuto As RFmxLteMXObwSweepTimeAuto
    Private sweepTimeInterval As Double

    Private averagingEnabled As RFmxLteMXObwAveragingEnabled
    Private averagingCount As Integer

    Private averagingType As RFmxLteMXObwAveragingType
    Private timeout As Double

    Private spectrum As Spectrum(Of Single)
    Private i As Integer
    Private subblockString As String

    Private subblocks As SubblockInput()
    Private subblockMsr As SubblockMeasurement()


    Private Sub CloseSession()
        If lte IsNot Nothing Then
            lte.Dispose()
            lte = Nothing
        End If
        If instrSession IsNot Nothing Then
            instrSession.Close()
            instrSession = Nothing
        End If
    End Sub

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
        instrSession = New RFmxInstrMX(rfsaResourceName, "")
    End Sub

    Private Sub InitializeVariables()
        rfsaResourceName = "RFSA"

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
        digitalEdgeSource = RFmxLteMXConstants.Pfi0
        digitalEdge = RFmxLteMXDigitalEdgeTriggerEdge.Rising
        triggerDelay = 0.0
        ' (s) 

        linkDirection = RFmxLteMXLinkDirection.Uplink

        sweepTimeAuto = RFmxLteMXObwSweepTimeAuto.[True]
        sweepTimeInterval = 0.001
        ' (s) 

        averagingEnabled = RFmxLteMXObwAveragingEnabled.[False]
        averagingCount = 10
        averagingType = RFmxLteMXObwAveragingType.Rms

        timeout = 10.0
        ' (s) 

        subblocks = New SubblockInput() {
                                             New SubblockInput() With {
                                                                            .subblockFrequency = 0.0,
                                                                            .componentCarrierSpacingType = RFmxLteMXComponentCarrierSpacingType.Nominal,
                                                                            .componentCarrierAtCenterFrequency = -1,
                                                                            .componentCarrierBandwidth = New Double(NumberOfComponentCarriers - 1) {20000000.0},
                                                                            .componentCarrierFrequency = New Double(NumberOfComponentCarriers - 1) {0.0}},
                                             New SubblockInput() With {
                                                                            .subblockFrequency = 30000000.0,
                                                                            .componentCarrierSpacingType = RFmxLteMXComponentCarrierSpacingType.Nominal,
                                                                            .componentCarrierAtCenterFrequency = -1,
                                                                            .componentCarrierBandwidth = New Double(NumberOfComponentCarriers - 1) {20000000.0},
                                                                            .componentCarrierFrequency = New Double(NumberOfComponentCarriers - 1) {0.0}
                                                                        }
                                         }

        subblockMsr = New SubblockMeasurement(NumberOfSubblocks - 1) {}

    End Sub

    Private Sub ConfigureLte()
        lte = instrSession.GetLteSignalConfiguration()
        ' Create a new RFmx Session 
        instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
        lte.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
        lte.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger)
        lte.ConfigureNumberOfSubblocks("", numberOfSubblocks)
        For i As Integer = 0 To numberOfSubblocks - 1
            subblockString = RFmxLteMX.BuildSubblockString("", i)
            lte.SetSubblockFrequency(subblockString, subblocks(i).subblockFrequency)
            lte.ComponentCarrier.ConfigureSpacing(subblockString, subblocks(i).componentCarrierSpacingType, subblocks(i).componentCarrierAtCenterFrequency)
            lte.ConfigureNumberOfComponentCarriers(subblockString, NumberOfComponentCarriers)
			lte.ComponentCarrier.ConfigureArray(subblockString, subblocks(i).componentCarrierBandwidth, subblocks(i).componentCarrierFrequency, Nothing)
		Next

        lte.ConfigureLinkDirection("", linkDirection)

		lte.SelectMeasurements("", RFmxLteMXMeasurementTypes.Obw, True)
		lte.Obw.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval)
		lte.Obw.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)
		lte.Initiate("", "")
	End Sub

	Private Sub RetrieveResults()		
		For i As Integer = 0 To numberOfSubblocks - 1
			subblockString = RFmxLteMX.BuildSubblockString("", i)
			lte.Obw.Results.FetchMeasurement(subblockString, timeout, subblockMsr(i).occupiedBandwidth, subblockMsr(i).absolutePower, subblockMsr(i).startFrequency, subblockMsr(i).stopFrequency)
		Next
		lte.Obw.Results.FetchSpectrum("", timeout, spectrum)

	End Sub

	Private Sub PrintResults()
        Console.WriteLine("Subblock Measurements            : ")
        For i = 0 To subblockMsr.Length - 1
            Console.WriteLine(vbLf & "*****************************************")
            Console.WriteLine(vbLf & "Subblock  : {0}", i)
            Console.WriteLine("Occupied Bandwidth (Hz)          : {0}", subblockMsr(i).occupiedBandwidth)
            Console.WriteLine("Absolute Power (dBm)             : {0}", subblockMsr(i).absolutePower)
            Console.WriteLine("Start Frequency (Hz)             : {0}", subblockMsr(i).startFrequency)
            Console.WriteLine("Stop Frequency (Hz)              : {0}", subblockMsr(i).stopFrequency)
        Next
	End Sub

	Private Shared Sub DisplayError(ex As Exception)
		Console.WriteLine("ERROR:" & vbLf & Convert.ToString(ex.[GetType]()) & ": " & ex.Message)
	End Sub

End Class
