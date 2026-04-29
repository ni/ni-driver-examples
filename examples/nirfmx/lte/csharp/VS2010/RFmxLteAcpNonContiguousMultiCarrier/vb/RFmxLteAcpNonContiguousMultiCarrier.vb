'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure basic signal properties (Center Frequency, Reference Level, External Attenuation and RF Attenuation).
'4. Configure Trigger Type and Trigger Parameters.
'5. Configure Duplex Mode.
'6[A-E]. Configure Subblock Parameters.
'Step 6 :
'6A. Configure Number of Subblocks.
'6B. Configure subblock Frequency.
'6C. Configure Component Carrier Spacing.
'6D. Configure Number of Component Carriers.
'6E. Configure Component Carriers (Component Carrier Frequency and Component Carrier Bandwidth).
'7. Select ACP measurement and enable Traces.
'8. Configure Measurement Method.
'9. Configure Averaging Parameters for ACP measurement.
'10. Configure Sweep Time Parameters.
'11. Configure Noise Compensation Parameter.
'12. Initiate the Measurement.
'13. Fetch ACP Measurements and Traces.
'14. Close RFmx Session. 

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.LteMX

Public Class RFmxLteAcpNonContiguousMultiCarrier
	Private instrSession As RFmxInstrMX
	Private lte As RFmxLteMX

	Private resourceName As String, frequencyReferenceSource As String, digitalEdgeSource As String, subblockString As String

    Private frequencyReferenceFrequency As Double, centerFrequency As Double, referenceLevel As Double, externalAttenuation As Double, rfAttenuation As Double, triggerDelay As Double, sweepTimeInterval As Double,
  timeout As Double
    Private enableTrigger As Boolean

	Private averagingCount As Integer

    Const NumberOfComponentCarriers As Integer = 1
    Const NumberOfSubblocks As Integer = 2

	Private rfAttenuationAuto As RFmxInstrMXRFAttenuationAuto
	Private averagingEnabled As RFmxLteMXAcpAveragingEnabled
	Private averagingType As RFmxLteMXAcpAveragingType
	Private digitalEdge As RFmxLteMXDigitalEdgeTriggerEdge
	Private duplexScheme As RFmxLteMXDuplexScheme
	Private measurementMethod As RFmxLteMXAcpMeasurementMethod
	Private noiseCompensationEnabled As RFmxLteMXAcpNoiseCompensationEnabled
	Private sweepTimeAuto As RFmxLteMXAcpSweepTimeAuto
    Private uplinkDownlinkConfiguration As RFmxLteMXUplinkDownlinkConfiguration
    Private linkDirection As RFmxLteMXLinkDirection

    ' Subblock inputs structure 

    Private Structure SubblockInput
		Public subblockFrequency As Double
        Public componentCarrierSpacingType As RFmxLteMXComponentCarrierSpacingType
        Public componentCarrierAtCenterFrequency As Integer
        Public componentCarrierBandwidth As Double()
        Public componentCarrierFrequency As Double()
        Public cellID As Integer()
    End Structure

    ' Subblock measurement outputs structure 

    Private Structure SubblockMeasurement
        Public subblockPower As Double
        Public integrationBandwidth As Double
        Public frequency As Double
        Public lowerAbsolutePower As Double()
        Public upperAbsolutePower As Double()
        Public lowerRelativePower As Double()
        Public upperRelativePower As Double()
    End Structure
    Private inputSubblock As SubblockInput()
    Private subblockMsr As SubblockMeasurement()

    Private totalAggregatedPower As Double
    Private spectrum As Spectrum(Of Single)

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

        centerFrequency = 1950000000.0
        ' Hz
        referenceLevel = 0.0
        ' dBm 
        externalAttenuation = 0.0
        ' dB 

        rfAttenuationAuto = RFmxInstrMXRFAttenuationAuto.[True]
        rfAttenuation = 10.0
        ' dB 

        frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
        frequencyReferenceFrequency = 10000000.0
        ' Hz 

        enableTrigger = False
        digitalEdgeSource = RFmxLteMXConstants.Pfi0
        digitalEdge = RFmxLteMXDigitalEdgeTriggerEdge.Rising
        triggerDelay = 0.0
        ' seconds 

        uplinkDownlinkConfiguration = RFmxLteMXUplinkDownlinkConfiguration.Configuration0
        duplexScheme = RFmxLteMXDuplexScheme.Fdd

        linkDirection = RFmxLteMXLinkDirection.Uplink

        measurementMethod = RFmxLteMXAcpMeasurementMethod.Normal

        noiseCompensationEnabled = RFmxLteMXAcpNoiseCompensationEnabled.[False]

        sweepTimeAuto = RFmxLteMXAcpSweepTimeAuto.[True]
        sweepTimeInterval = 0.001
        ' seconds 

        averagingEnabled = RFmxLteMXAcpAveragingEnabled.[False]
        averagingCount = 10
        averagingType = RFmxLteMXAcpAveragingType.Rms

        timeout = 10.0
        ' seconds 


        subblockMsr = New SubblockMeasurement(NumberOfSubblocks - 1) {}

        inputSubblock = New SubblockInput() {
                                                New SubblockInput() With {
                                                                            .subblockFrequency = 0.0,
                                                                            .componentCarrierSpacingType = RFmxLteMXComponentCarrierSpacingType.Nominal,
                                                                            .componentCarrierAtCenterFrequency = -1,
                                                                            .componentCarrierBandwidth = New Double(NumberOfComponentCarriers - 1) {20000000.0},
                                                                            .componentCarrierFrequency = New Double(NumberOfComponentCarriers - 1) {0.0},
                                                                            .cellID = Nothing
                                                                         },
                                                New SubblockInput() With {
                                                                            .subblockFrequency = 30000000.0,
                                                                            .componentCarrierSpacingType = RFmxLteMXComponentCarrierSpacingType.Nominal,
                                                                            .componentCarrierAtCenterFrequency = -1,
                                                                            .componentCarrierBandwidth = New Double(NumberOfComponentCarriers - 1) {20000000.0},
                                                                            .componentCarrierFrequency = New Double(NumberOfComponentCarriers - 1) {0.0},
                                                                            .cellID = Nothing
                                                                        }
                                            }

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

        lte.ConfigureFrequency("", centerFrequency)

        lte.ConfigureReferenceLevel("", referenceLevel)

        lte.ConfigureExternalAttenuation("", externalAttenuation)

        instrSession.ConfigureRFAttenuation("", rfAttenuationAuto, rfAttenuation)

        lte.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger)

        lte.ConfigureDuplexScheme("", duplexScheme, uplinkDownlinkConfiguration)

        lte.ConfigureNumberOfSubblocks("", NumberOfSubblocks)

        For i As Integer = 0 To NumberOfSubblocks - 1
            subblockString = RFmxLteMX.BuildSubblockString("", i)
            lte.SetSubblockFrequency(subblockString, inputSubblock(i).subblockFrequency)

            lte.ComponentCarrier.ConfigureSpacing(subblockString, inputSubblock(i).componentCarrierSpacingType,
                                                  inputSubblock(i).componentCarrierAtCenterFrequency)

            lte.ConfigureNumberOfComponentCarriers(subblockString, NumberOfComponentCarriers)


            lte.ComponentCarrier.ConfigureArray(subblockString, inputSubblock(i).componentCarrierBandwidth,
                                                inputSubblock(i).componentCarrierFrequency, inputSubblock(i).cellID)
        Next

        lte.ConfigureLinkDirection("", linkDirection)

        lte.SelectMeasurements("", RFmxLteMXMeasurementTypes.Acp, True)

        lte.Acp.Configuration.ConfigureMeasurementMethod("", measurementMethod)

        lte.Acp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)

        lte.Acp.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval)

        lte.Acp.Configuration.ConfigureNoiseCompensationEnabled("", noiseCompensationEnabled)

        lte.Initiate("", "")

    End Sub

	Private Sub RetrieveResults()
		' Retrieve results 


		For i As Integer = 0 To NumberOfSubblocks - 1
			subblockString = RFmxLteMX.BuildSubblockString("", i)

            lte.Acp.Results.FetchSubblockMeasurement(subblockString, timeout, subblockMsr(i).subblockPower,
                                                     subblockMsr(i).integrationBandwidth, subblockMsr(i).frequency)

            lte.Acp.Results.FetchOffsetMeasurementArray(subblockString, timeout, subblockMsr(i).lowerRelativePower,
                                                        subblockMsr(i).upperRelativePower, subblockMsr(i).lowerAbsolutePower, subblockMsr(i).upperAbsolutePower)
		Next

		lte.Acp.Results.FetchTotalAggregatedPower("", timeout, totalAggregatedPower)

		lte.Acp.Results.FetchSpectrum("", timeout, spectrum)
	End Sub

	Private Sub PrintResults()

        Console.WriteLine(vbLf & "Total Aggregated Power (dBm): {0}", totalAggregatedPower)
        Console.WriteLine(vbLf & "****************Subblock Measurements****************")

		For i As Integer = 0 To NumberOfSubblocks - 1
			Console.WriteLine(vbLf & "******************************************************")
			Console.WriteLine(vbLf & "Subblock  :  {0}" & vbLf, i)
            Console.WriteLine("Subblock Power (dBm)       : {0}", subblockMsr(i).subblockPower)
            Console.WriteLine("Integration Bandwidth (Hz) : {0}", subblockMsr(i).integrationBandwidth)
            Console.WriteLine("Frequency (Hz)             : {0}", subblockMsr(i).frequency)

            Console.WriteLine(vbLf & "Offset Channel Measurements:")
            For j As Integer = 0 To subblockMsr(i).lowerRelativePower.Length - 1
                Console.WriteLine(vbLf & "Offset  :  {0}", j)
                Console.WriteLine("Lower Relative Power (dB)  : {0}", subblockMsr(i).lowerRelativePower(j))
                Console.WriteLine("Upper Relative Power (dB)  : {0}", subblockMsr(i).upperRelativePower(j))
                Console.WriteLine("Lower Absolute Power (dBm) : {0}", subblockMsr(i).lowerAbsolutePower(j))
                Console.WriteLine("Upper Absolute Power (dBm) : {0}", subblockMsr(i).upperAbsolutePower(j))
            Next
            Console.WriteLine("------------------------------------------" & vbLf)
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
