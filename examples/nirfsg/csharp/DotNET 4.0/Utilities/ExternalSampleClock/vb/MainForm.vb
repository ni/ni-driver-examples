'==================================================================================================
' Title        : External Sample Clock
' Description  : This example demonstrates how to use an external sample clock
'			     to generate an arbitrary waveform. The example lets you choose
'			     one of three waveforms to create and write:
'			       + Double Side Band --> two tones around the center frequency.
'			       + Lower Side Band  --> one tone to the left of the center frequency.
'			       + Upper Side Band  --> one tone to the right of the center frequency.
'			     Each waveform is 100 samples long. The waveforms are generated at a
'			     frequency of IQ Rate / 100 samples. The user sets the IQ rate on the
'			     RF Signal Generator and reads out the Arb Sample Clock Rate, which
'			     is the rate at which the external clock is generated. The use of
'			     the external clock gives the user the flexibility of not having to resample
'			     his data as the IQ rate is not coerced.
'==================================================================================================

Imports System
Imports System.Windows.Forms
Imports System.Collections
Imports System.Collections.Generic
Imports NationalInstruments.ModularInstruments.NIRfsg
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Partial Public Class MainForm
    Inherits Form
    Private _rfsgSignalGenerator As NIRfsg, _rfsgExternalClock As NIRfsg
    Const PowerLevel As Double = 0.0

    Public Sub New()
        InitializeComponent()

        LoadRfsgVectorSignalGeneratorDeviceNames()
        LoadRfsgExternalClockSourceDeviceNames()

        ConfigureWaveformComboBox()
        ConfigureArbSampleClkSrcComboBox()
    End Sub

    Private Sub LoadRfsgVectorSignalGeneratorDeviceNames()
        Dim modularInstrumentsSystem As New ModularInstrumentsSystem("NI-Rfsg")
        For Each device As DeviceInfo In modularInstrumentsSystem.DeviceCollection
            vectorSignalGeneratorComboBox.Items.Add(device.Name)
        Next
        If modularInstrumentsSystem.DeviceCollection.Count > 0 Then
            vectorSignalGeneratorComboBox.SelectedIndex = 0
        End If
    End Sub

    Private Sub LoadRfsgExternalClockSourceDeviceNames()
        Dim modularInstrumentsSystem As New ModularInstrumentsSystem("NI-Rfsg")
        For Each device As DeviceInfo In modularInstrumentsSystem.DeviceCollection
            externalClockSourceComboBox.Items.Add(device.Name)
        Next
        If modularInstrumentsSystem.DeviceCollection.Count > 0 Then
            externalClockSourceComboBox.SelectedIndex = 0
        End If
    End Sub

#Region "UI Initial Value Config Section"

    Private Sub ConfigureWaveformComboBox()
        waveformComboBox.Items.AddRange(New String() {"DoubleSideBand", "LowerSideBand", "UpperSideBand"})
        waveformComboBox.SelectedIndex = 0
    End Sub

    Private Sub ConfigureArbSampleClkSrcComboBox()
        Dim arbSampleClockSourceValueList = New List(Of DictionaryEntry)()
        arbSampleClockSourceValueList.Add(New DictionaryEntry("ClkIn", RfsgArbSampleClockSource.ClockIn))
        arbSampleClockSourceValueList.Add(New DictionaryEntry("OnboardClock", RfsgArbSampleClockSource.OnboardClock))

        arbSampleClockSourceComboBox.DataSource = arbSampleClockSourceValueList
        arbSampleClockSourceComboBox.DisplayMember = "Key"
        arbSampleClockSourceComboBox.ValueMember = "Value"
        arbSampleClockSourceComboBox.SelectedValue = RfsgArbSampleClockSource.ClockIn
    End Sub
#End Region

#Region "Program Functions"

    Private Sub StartGeneration()
        Dim vectorSigGenName As String
        Dim externalClockName As String
        Dim kNumberOfSamples As Integer = 100
        Dim frequency As Double, power As Double, iqRate As Double, arbSampleClockRate As Double
        Dim waveform As Integer
        Dim arbSampleClockSource As RfsgArbSampleClockSource
        Dim sameDeviceError As String = "The signal generator and external sample clock cannot be the same device."
        Try
            Dim iData As Double() = Nothing
            Dim qData As Double() = Nothing

            ' Read in all the control values
            vectorSigGenName = vectorSignalGeneratorComboBox.Text
            frequency = CDbl(frequencyNumeric.Value)
            power = CDbl(powerLevelNumeric.Value)
            iqRate = CDbl(iqRateNumeric.Value)
            arbSampleClockSource = If(TryCast(arbSampleClockSourceComboBox.SelectedValue, RfsgArbSampleClockSource), RfsgArbSampleClockSource.FromString(arbSampleClockSourceComboBox.Text))
            waveform = waveformComboBox.SelectedIndex
            externalClockName = externalClockSourceComboBox.Text

            If vectorSigGenName.Equals(externalClockName) Then
                errorTextBox.Text = "Error: " + sameDeviceError
                Return
            End If

            errorTextBox.Text = "No error."
            Application.DoEvents()

            Select Case waveform
                Case 0
                    ' Double Side Band
                    iData = SinePattern(kNumberOfSamples, 1.0, 0.0, 1.0)
                    qData = SinePattern(kNumberOfSamples, 1.0, 0.0, 1.0)
                    Exit Select

                Case 1
                    ' Lower Side Band
                    iData = SinePattern(kNumberOfSamples, 1.0, 0.0, 1.0)
                    qData = SinePattern(kNumberOfSamples, 1.0, 90.0, 1.0)
                    Exit Select

                Case 2
                    ' Upper Side Band
                    iData = SinePattern(kNumberOfSamples, 1.0, 90.0, 1.0)
                    qData = SinePattern(kNumberOfSamples, 1.0, 0.0, 1.0)
                    Exit Select
            End Select

            ' Open a session to NI-RFSG.
            _rfsgSignalGenerator = New NIRfsg(vectorSigGenName, True, False)

            ' Subscribe to Rfsg warnings
            AddHandler _rfsgSignalGenerator.DriverOperation.Warning, AddressOf Me.RfsgSigGen_DriverOperation_Warning

            ' Configure the center frequency and output power level.
            _rfsgSignalGenerator.RF.Configure(frequency, power)

            ' Configure the generation mode to Arb Waveform.
            _rfsgSignalGenerator.Arb.GenerationMode = RfsgWaveformGenerationMode.ArbitraryWaveform

            ' Configure the desired IQ Rate. Set the Arb Sample Clock Source to the desired source.
            _rfsgSignalGenerator.Arb.IQRate = iqRate
            _rfsgSignalGenerator.ArbSampleClock.Source = arbSampleClockSource

            ' Get the Arb Sample Clock Rate to configure the external clock source. Get the actual IQ Rate.
            arbSampleClockRate = _rfsgSignalGenerator.ArbSampleClock.Rate
            iqRate = _rfsgSignalGenerator.Arb.IQRate

            actualArbSampleRateTextBox.Text = arbSampleClockRate.ToString()
            actualIQRateTextBox.Text = iqRate.ToString()
            actualFrequencyOffsetTextBox.Text = (iqRate / kNumberOfSamples).ToString()

            ' Open a session to the external sample clock source.
            _rfsgExternalClock = New NIRfsg(externalClockName, True, False)

            ' Subscribe to Rfsg warnings
            AddHandler _rfsgExternalClock.DriverOperation.Warning, AddressOf Me.RfsgExtClk_DriverOperation_Warning

            ' Configure the power level and the center frequency with the arb sample clock rate acquired earlier.
            _rfsgExternalClock.RF.Configure(arbSampleClockRate, PowerLevel)

            ' Initiate generation on the external sample clock source according to programmed settings.
            _rfsgExternalClock.Initiate()

            ' Set the signal bandwidth.  Signal bandwidth is 2 * baseband signal's
            '   maximum frequency deviation from 0 Hz.
            _rfsgSignalGenerator.Arb.SignalBandwidth = (iqRate / kNumberOfSamples) * 2

            ' Write the arbitrary waveform.
            _rfsgSignalGenerator.Arb.WriteWaveform(String.Empty, iData, qData)

            ' Initiate Generation
            _rfsgSignalGenerator.Initiate()

            ' Start the status checking timer
            EnableControls(False)

            ' Activate stop button
            stopButton.Focus()
        Catch ex As Exception
            ShowError("StartGeneration()", ex)
        End Try
    End Sub

    Private Sub RfsgSigGen_DriverOperation_Warning(ByVal sender As Object, ByVal e As RfsgWarningEventArgs)
        ' Display the rfsg warning
        errorTextBox.Text = "Vector Signal Generator Warning: " + e.Message
    End Sub

    Private Sub RfsgExtClk_DriverOperation_Warning(ByVal sender As Object, ByVal e As RfsgWarningEventArgs)
        ' Display the rfsg warning
        errorTextBox.Text = "Extrenal Clock Source Warning: " + e.Message
    End Sub

    Private Shared Function SinePattern(ByVal kNumberOfSamples As Integer, ByVal amplitude As Double, ByVal phaseDegrees As Double, ByVal numberOfCycles As Double) As Double()
        Dim sineArray As Double() = New Double(kNumberOfSamples - 1) {}
        For i As Integer = 0 To kNumberOfSamples - 1
            sineArray(i) = amplitude * Math.Sin(2 * Math.PI * i * numberOfCycles / kNumberOfSamples + Math.PI * phaseDegrees / 180)
        Next
        Return sineArray
    End Function

    Private Sub CheckGeneration()
        Try
            ' Check the status of the RFSG
            _rfsgSignalGenerator.CheckGenerationStatus()
            _rfsgExternalClock.CheckGenerationStatus()
        Catch ex As Exception
            ShowError("CheckGeneration()", ex)
        End Try
    End Sub

    Private Sub StopGeneration()
        EnableControls(True)
        Try
            If _rfsgSignalGenerator IsNot Nothing Then
                _rfsgSignalGenerator.RF.OutputEnabled = False

                ' Unsubscribe from rfsg warnings
                RemoveHandler _rfsgSignalGenerator.DriverOperation.Warning, AddressOf Me.RfsgSigGen_DriverOperation_Warning

                _rfsgSignalGenerator.Close()
            End If
            _rfsgSignalGenerator = Nothing

            If _rfsgExternalClock IsNot Nothing Then
                _rfsgExternalClock.RF.OutputEnabled = False

                ' Unsubscribe from rfsg warnings
                RemoveHandler _rfsgExternalClock.DriverOperation.Warning, AddressOf Me.RfsgExtClk_DriverOperation_Warning

                _rfsgExternalClock.Close()
            End If
            _rfsgExternalClock = Nothing
        Catch ex As Exception
            errorTextBox.Text = "Error in StopGeneration(): " + ex.Message
        End Try

    End Sub

    Private Sub ShowError(ByVal functionName As String, ByVal exception As Exception)
        StopGeneration()
        errorTextBox.Text = "Error in " + functionName + ": " + exception.Message
    End Sub

#End Region

#Region "Form Events"
    Private Sub startButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles startButton.Click
        StartGeneration()
    End Sub

    Private Sub stopButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles stopButton.Click
        StopGeneration()
    End Sub

    Private Sub MainForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        StopGeneration()
    End Sub

    Private Sub rfsgStatusTimer_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles rfsgStatusTimer.Tick
        CheckGeneration()
    End Sub
#End Region

    Private Sub EnableControls(ByVal enabled As Boolean)
        startButton.Enabled = enabled
        stopButton.Enabled = Not enabled
        vectorSignalGeneratorComboBox.Enabled = enabled
        frequencyNumeric.Enabled = enabled
        powerLevelNumeric.Enabled = enabled
        iqRateNumeric.Enabled = enabled
        arbSampleClockSourceComboBox.Enabled = enabled
        externalClockSourceComboBox.Enabled = enabled
        waveformComboBox.Enabled = enabled
        rfsgStatusTimer.Enabled = Not enabled

        Application.DoEvents()
    End Sub
End Class
