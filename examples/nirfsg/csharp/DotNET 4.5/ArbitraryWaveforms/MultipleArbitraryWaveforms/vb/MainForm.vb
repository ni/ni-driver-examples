'==================================================================================================
' Title        : Multiple Arbitrary Waveforms
' Description  : This example demonstrates how to create and write multiple 
'			     arbitrary waveforms in Arb Waveform mode.  This technique 
'			     allows you to switch between waveforms without incurring the 
'			     overhead of writing them.  The example creates and writes 
'			     three waveforms and lets you select a waveform for generation. 
'			     The three waveforms are: 
'
'			        + Double Side Band - two tones around the center frequency 
'			        + Lower Side Band  - one tone left of the center frequency 
'			        + Upper Side Band  - one tone right of the center frequency 
'
'			     Each waveform is 100 samples long.  Since the sample clock 
'			     rate is 100 MS/s, the frequency of the waveforms is 1 MHz; if 
'			     viewed on a spectrum analyzer, the waveform peak(s) will be 
'			     1 MHz away from the specified center frequency. 
'
'			     Note: In order to run this example, the upconverter must be configured with 
'			     an Arbitrary Waveform Generator. 
'			     To do this, open Measurement & Automation Explorer, select the upconverter 
'			     and click on properties.
'==================================================================================================

Imports System
Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments.NIRfsg
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Partial Public Class MainForm
    Inherits Form
    Private _rfsgSession As NIRfsg
    Private Const DoubleSideband As String = "DoubleSideBand"
    Private Const LowerSideband As String = "LowerSideBand"
    Private Const UpperSideband As String = "UpperSideBand"

    Public Sub New()
        InitializeComponent()

        ' Enable controls on startup
        EnableControls(True)

        LoadRfsgDeviceNames()

        ConfigureWaveformComboBox()
    End Sub

    Private Sub LoadRfsgDeviceNames()
        Dim modularInstrumentsSystem As New ModularInstrumentsSystem("NI-Rfsg")
        For Each device As DeviceInfo In modularInstrumentsSystem.DeviceCollection
            resourceNameComboBox.Items.Add(device.Name)
        Next
        If modularInstrumentsSystem.DeviceCollection.Count > 0 Then
            resourceNameComboBox.SelectedIndex = 0
        End If
    End Sub

#Region "UI Initial Value Config Section"

    Private Sub ConfigureWaveformComboBox()
        waveformComboBox.Items.AddRange(New String() {DoubleSideband, LowerSideband, UpperSideband})
        waveformComboBox.SelectedIndex = 0
    End Sub

#End Region

#Region "Program Functions"

    Private Sub StartGeneration()
        Dim resourceName As String
        Dim numberOfSamples As Integer = 100
        Dim frequency As Double
        Dim frequencyOffset As Double
        Dim power As Double
        Dim iqRate As Double
        Dim actualIQRate As Double
        Dim waveform As Integer
        Dim data As Double(), data90 As Double()
        Try
            ' Read in all of the control values 
            resourceName = resourceNameComboBox.Text
            frequency = CDbl(frequencyNumeric.Value)
            power = CDbl(powerLevelNumeric.Value)
            waveform = waveformComboBox.SelectedIndex
            iqRate = CDbl(iqRateNumeric.Value)

            data = SinePattern(numberOfSamples, 1.0, 0.0, 1.0)
            data90 = SinePattern(numberOfSamples, 1.0, 90.0, 1.0)

            errorTextBox.Text = "No error."
            Application.DoEvents()

            ' Initialize the NIRfsg session
            _rfsgSession = New NIRfsg(resourceName, True, False)

            ' Subscribe to Rfsg warnings
            AddHandler _rfsgSession.DriverOperation.Warning, AddressOf Me.DriverOperation_Warning

            ' Configure the instrument 
            _rfsgSession.RF.Configure(frequency, power)
            _rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.ArbitraryWaveform
            _rfsgSession.Arb.IQRate = iqRate

            ' Get the actual IQ rate, and populate the GUI output 
            actualIQRate = _rfsgSession.Arb.IQRate

            frequencyOffset = actualIQRate / numberOfSamples
            actualIQRateTextBox.Text = actualIQRate.ToString()
            actualFrequencyOffsetTextBox.Text = frequencyOffset.ToString()

            ' Configure the signal bandwidth to twice the baseband signal's
            '  maximum frequency deviation from 0 Hz. 
            _rfsgSession.Arb.SignalBandwidth = 2 * frequencyOffset

            ' Select the requested waveform by string name 
            Select Case waveform
                Case 0
                    ' DoubleSideBand 
                    _rfsgSession.Arb.SelectedWaveform = DoubleSideband
                    Exit Select

                Case 1
                    ' LowerSideBand 
                    _rfsgSession.Arb.SelectedWaveform = LowerSideband
                    Exit Select

                Case 2
                    ' UpperSideBand 
                    _rfsgSession.Arb.SelectedWaveform = UpperSideband
                    Exit Select
            End Select

            ' Configure three waveforms 
            _rfsgSession.Arb.WriteWaveform(DoubleSideband, data, data)
            _rfsgSession.Arb.WriteWaveform(LowerSideband, data, data90)
            _rfsgSession.Arb.WriteWaveform(UpperSideband, data90, data)

            ' Initiate Generation 
            _rfsgSession.Initiate()

            ' Start the status checking timer 
            EnableControls(False)

            ' Activate stop and update buttons 
            stopButton.Focus()
        Catch ex As Exception
            ShowError("StartGeneration()", ex)
        End Try
    End Sub

    Private Sub DriverOperation_Warning(ByVal sender As Object, ByVal e As RfsgWarningEventArgs)
        ' Display the rfsg warning
        errorTextBox.Text = e.Message
    End Sub

    Private Shared Function SinePattern(ByVal numberOfSamples As Integer, ByVal amplitude As Double, ByVal phaseDegrees As Double, ByVal numberOfCycles As Double) As Double()
        Dim sineArray As Double() = New Double(numberOfSamples - 1) {}
        For i As Integer = 0 To numberOfSamples - 1
            sineArray(i) = amplitude * Math.Sin(2 * Math.PI * i * numberOfCycles / numberOfSamples + Math.PI * phaseDegrees / 180)
        Next
        Return sineArray
    End Function

    Private Sub CheckGeneration()
        Try
            ' Check the status of the RFSG 
            _rfsgSession.CheckGenerationStatus()
        Catch ex As Exception
            ShowError("CheckGeneration()", ex)
        End Try
    End Sub

    Private Sub UpdateGeneration()
        Dim waveform As Integer
        Try
            ' Stop the status checking timer 
            EnableControls(True)

            ' Read in the waveform selection 
            waveform = waveformComboBox.SelectedIndex

            ' Abort generation 
            _rfsgSession.Abort()

            ' Select the requested waveform by string name 
            Select Case waveform
                Case 0
                    ' DoubleSideBand 
                    _rfsgSession.Arb.SelectedWaveform = doubleSideband
                    Exit Select

                Case 1
                    ' LowerSideBand 
                    _rfsgSession.Arb.SelectedWaveform = lowerSideband
                    Exit Select

                Case 2
                    ' UpperSideBand 
                    _rfsgSession.Arb.SelectedWaveform = upperSideband
                    Exit Select
            End Select

            ' Initiate Generation 
            _rfsgSession.Initiate()

            ' Start the status checking timer 
            EnableControls(False)
        Catch ex As Exception
            ShowError("UpdateGeneration()", ex)
        End Try
    End Sub

    Private Sub StopGeneration()
        ' Activate all stopped controls 
        EnableControls(True)

        Try
            If _rfsgSession IsNot Nothing Then
                ' Disable the output.  This sets the noise floor as low as possible.
                _rfsgSession.RF.OutputEnabled = False

                ' Unsubscribe from warning events
                RemoveHandler _rfsgSession.DriverOperation.Warning, AddressOf Me.DriverOperation_Warning

                ' Close the RFSG NIRfsg session
                _rfsgSession.Close()
            End If
            _rfsgSession = Nothing
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

    Private Sub updateButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles updateButton.Click
        UpdateGeneration()
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
        updateButton.Enabled = Not enabled
        resourceNameComboBox.Enabled = enabled
        frequencyNumeric.Enabled = enabled
        powerLevelNumeric.Enabled = enabled
        iqRateNumeric.Enabled = enabled
        rfsgStatusTimer.Enabled = Not enabled

        Application.DoEvents()
    End Sub
End Class
