'==================================================================================================
' Title        : Simple Script
' Description  : This example demonstrates how to use scripts to dictate the behavior of the 
'			     waveform generation. 
'
'			     Note: In order to run this example, the upconverter must be configured with 
'			     an Arbitrary Waveform Generator. 
'			     To do this, open Measurement & Automation Explorer, select the upconverter 
'			     and click on properties.
'==================================================================================================

Imports System
Imports System.Windows.Forms
Imports System.Collections
Imports System.Collections.Generic
Imports NationalInstruments.ModularInstruments.NIRfsg
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Partial Public Class MainForm
    Inherits Form
    Private _rfsgSession As NIRfsg
    Const ArbPreFilterGain As Double = -2

    Public Sub New()
        InitializeComponent()

        LoadRfsgDeviceNames()

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

#End Region

#Region "Program Functions"

    Private Sub StartGeneration()
        Dim kNumberOfSamples As Integer = 1000000
        Dim kSamplesPerCycle As Double = 10000.0
        Dim resourceName As String
        Dim frequency As Double
        Dim frequencyOffset As Double
        Dim power As Double
        Dim iqRate As Double
        Dim powerLevelType As RfsgRFPowerLevelType
        Dim script As String
        Dim actualIQRate As Double
        Dim iData As Double()
        Dim qData As Double()
        Try
            ' Read in all of the control values 
            resourceName = resourceNameComboBox.Text
            frequency = CDbl(frequencyNumeric.Value)
            power = CDbl(powerLevelNumeric.Value)
            iqRate = CDbl(iqRateNumeric.Value)
            script = scriptTextBox.Text
            powerLevelType = RfsgRFPowerLevelType.PeakPower

            iData = SinePattern(kNumberOfSamples, 1.0, 0.0, kSamplesPerCycle)
            qData = SinePattern(kNumberOfSamples, 1.0, 90.0, kSamplesPerCycle)

            errorTextBox.Text = "No error."
            Application.DoEvents()

            ' Initialize the NIRfsg session
            _rfsgSession = New NIRfsg(resourceName, True, False)

            ' Subscribe to Rfsg warnings
            AddHandler _rfsgSession.DriverOperation.Warning, AddressOf Me.DriverOperation_Warning

            ' Configure the power level type 
            _rfsgSession.RF.PowerLevelType = powerLevelType

            ' Configure the instrument 
            _rfsgSession.RF.Configure(frequency, power)

            ' Configure the generation mode to Script 
            _rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.Script

            ' Configure the IQ rate of the waveforms 
            _rfsgSession.Arb.IQRate = iqRate

            ' Configure Pre-filter Gain to avoid overflow due to phase-
            '  discontinuous signals 
            _rfsgSession.Arb.PreFilterGain = ArbPreFilterGain


            ' Get the actual IQ rate, and populate the GUI output 
            actualIQRate = _rfsgSession.Arb.IQRate

            frequencyOffset = actualIQRate / (kNumberOfSamples / kSamplesPerCycle)

            actualIQRateTextBox.Text = actualIQRate.ToString()
            actualFrequencyOffsetTextBox.Text = frequencyOffset.ToString()

            ' Configure the signal bandwidth to twice the maximum frequency
            '  deviation of all the waveforms in the script. 
            _rfsgSession.Arb.SignalBandwidth = 2 * frequencyOffset

            ' Write the two waveforms 
            _rfsgSession.Arb.WriteWaveform("negativeOffset", iData, qData)
            _rfsgSession.Arb.WriteWaveform("positiveOffset", qData, iData)

            ' Write the script 
            _rfsgSession.Arb.Scripting.WriteScript(script)

            ' Initiate Generation 
            _rfsgSession.Initiate()

            ' Start the status checking timer 
            EnableControls(False)
            stopButton.Focus()
        Catch ex As Exception
            ShowError("StartGeneration()", ex)
        End Try
    End Sub

    Private Sub DriverOperation_Warning(ByVal sender As Object, ByVal e As RfsgWarningEventArgs)
        ' Display the rfsg warning
        errorTextBox.Text = e.Message
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
            _rfsgSession.CheckGenerationStatus()
        Catch ex As Exception
            ShowError("CheckGeneration()", ex)
        End Try
    End Sub

    Private Sub StopGeneration()
        ' Stop the status checking timer 
        EnableControls(True)
        Try
            ' Disable the output.  This sets the noise floor as low as possible.
            If _rfsgSession IsNot Nothing Then
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
        resourceNameComboBox.Enabled = enabled
        scriptTextBox.Enabled = enabled
        frequencyNumeric.Enabled = enabled
        powerLevelNumeric.Enabled = enabled
        iqRateNumeric.Enabled = enabled
        rfsgStatusTimer.Enabled = Not enabled

        Application.DoEvents()
    End Sub
End Class
