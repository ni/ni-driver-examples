'==================================================================================================
' Title        : Script Trigger-Software Source
' Description  : This example demonstrates how to a generate signals based on a software script 
'			     trigger. The behavior of the signals is dictated by a script. This example has 
'			     two scripts.  The first alternates between two signals when the specified software 
'			     script trigger is received. The second script shows nested triggering.  It waits 
'			     for a trigger for it to start generating a waveform.  A separate trigger will 
'			     generate a second waveform. 
'
'			     Note: In order to run this example, the upconverter must be configured with 
'			     an Arbitrary Waveform Generator. 
'			     To do this, open Measurement & Automation Explorer, select the upconverter 
'			     and click on properties.
'==================================================================================================

Imports System
Imports System.Linq
Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments.NIRfsg
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Partial Public Class MainForm
    Inherits Form
    Private _rfsgSession As NIRfsg
    Const ArbPreFilterGain As Double = -2
    Private _scripts As String() = {
                                    "script triggersToggleWaveforms" + Environment.NewLine +
                                    "   repeat forever" + Environment.NewLine +
                                    "      repeat until scriptTrigger0" + Environment.NewLine +
                                    "         generate negativeOffset" + Environment.NewLine +
                                    "      end repeat" + Environment.NewLine +
                                    "      repeat until scriptTrigger0" + Environment.NewLine +
                                    "         generate positiveOffset" + Environment.NewLine +
                                    "      end repeat " + Environment.NewLine +
                                    "   end repeat " + Environment.NewLine +
                                    "end script",
                                   "script myScript" + Environment.NewLine +
                                   "  Repeat forever" + Environment.NewLine +
                                   "     Generate allZeros" + Environment.NewLine +
                                   "     Clear scriptTrigger0" + Environment.NewLine +
                                   "     Wait until scriptTrigger0" + Environment.NewLine +
                                   "     Clear scriptTrigger1 " + Environment.NewLine +
                                   "    Repeat until scriptTrigger1" + Environment.NewLine +
                                   "        Generate positiveOffset" + Environment.NewLine +
                                   "     end repeat" + Environment.NewLine +
                                   "     Clear scriptTrigger1" + Environment.NewLine +
                                   "     Repeat until scriptTrigger1" + Environment.NewLine +
                                   "        Generate negativeOffset" + Environment.NewLine +
                                   "     end repeat" + Environment.NewLine +
                                   "  end repeat" + Environment.NewLine +
                                   "end script"}

    Public Sub New()
        InitializeComponent()

        LoadRfsgDeviceNames()

        ' force set the script
        scriptIndexNumeric.Value = 1
        scriptIndexNumeric.Value = 0

        EnableControls(True)
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

#Region "Program Functions"

    Private Sub StartGeneration()
        Dim resourceName As String
        Dim numberOfSamples As Integer = 100
        Dim numberOfBlankSamples As Integer = 500
        Dim scriptNumber As Integer
        Dim frequency As Double
        Dim power As Double
        Dim iqRate As Double
        Dim actualIQRate As Double
        Dim frequencyOffset As Double
        Dim iData As Double()
        Dim qData As Double()
        Dim blankData As Double()
        Try
            ' Read in all of the control values 
            resourceName = resourceNameComboBox.Text
            frequency = CDbl(frequencyNumeric.Value)
            power = CDbl(powerLevelNumeric.Value)
            iqRate = CDbl(iqRateNumeric.Value)

            iData = SinePattern(numberOfSamples, 1.0, 0.0, 1.0)
            qData = SinePattern(numberOfSamples, 1.0, 90.0, 1.0)
            blankData = Enumerable.Repeat(0.0, numberOfBlankSamples).ToArray()

            errorTextBox.Text = "No error."
            Application.DoEvents()

            ' Initialize the NIRfsg session
            _rfsgSession = New NIRfsg(resourceName, True, False)

            ' Subscribe to Rfsg warnings
            AddHandler _rfsgSession.DriverOperation.Warning, AddressOf Me.DriverOperation_Warning

            ' Configure the instrument 
            _rfsgSession.RF.Configure(frequency, power)

            ' Configure the generation mode to Script 
            _rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.Script

            ' Configure scriptTrigger0 and scriptTrigger1 
            _rfsgSession.Triggers.ScriptTriggers(0).ConfigureSoftwareTrigger()

            _rfsgSession.Triggers.ScriptTriggers(1).ConfigureSoftwareTrigger()

            ' Configure the IQ rate of the waveforms 
            _rfsgSession.Arb.IQRate = iqRate

            ' Configure Pre-filter Gain to avoid overflow due to phase-
            '  discontinuous signals 
            _rfsgSession.Arb.PreFilterGain = ArbPreFilterGain


            ' Configure the power level type 
            _rfsgSession.RF.PowerLevelType = RfsgRFPowerLevelType.PeakPower

            ' Get the actual IQ rate, and populate the GUI output 
            actualIQRate = _rfsgSession.Arb.IQRate
            actualIQRateTextBox.Text = actualIQRate.ToString()
            frequencyOffset = actualIQRate / numberOfSamples
            actualFrequencyOffsetTextBox.Text = frequencyOffset.ToString()

            ' Configure the signal bandwidth 
            _rfsgSession.Arb.SignalBandwidth = frequencyOffset * 2

            ' Write the arb waveforms 
            _rfsgSession.Arb.WriteWaveform("negativeOffset", iData, qData)
            _rfsgSession.Arb.WriteWaveform("positiveOffset", qData, iData)

            _rfsgSession.Arb.WriteWaveform("allZeros", blankData, blankData)

            ' Write the script 
            scriptNumber = CInt(scriptIndexNumeric.Value)
            _rfsgSession.Arb.Scripting.WriteScript(_scripts(scriptNumber))

            ' Initiate Generation 
            _rfsgSession.Initiate()

            stopButton.Focus()

            ' Start the status checking timer 
            EnableControls(False)
        Catch ex As Exception
            ShowError("StartGeneration()", ex)
        End Try
    End Sub

    Private Shared Function SinePattern(ByVal numberOfSamples As Integer, ByVal amplitude As Double, ByVal phaseDegrees As Double, ByVal numberOfCycles As Double) As Double()
        Dim sineArray As Double() = New Double(numberOfSamples - 1) {}
        For i As Integer = 0 To numberOfSamples - 1
            sineArray(i) = amplitude * Math.Sin(2 * Math.PI * i * numberOfCycles / numberOfSamples + Math.PI * phaseDegrees / 180)
        Next
        Return sineArray
    End Function

    Private Sub DriverOperation_Warning(ByVal sender As Object, ByVal e As RfsgWarningEventArgs)
        ' Display the rfsg warning
        errorTextBox.Text = e.Message
    End Sub

    Private Sub StopGeneration()
        ' Stop the status checking timer, and turn off the LED 
        EnableControls(True)
        readyLed.BackColor = SystemColors.Control
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

    Private Sub CheckGeneration()
        Try
            readyLed.BackColor = Color.Lime

            ' Check the status of the RFSG 
            If _rfsgSession.CheckGenerationStatus() = RfsgGenerationStatus.Complete Then
                StopGeneration()
            End If
        Catch ex As Exception
            ShowError("CheckGeneration()", ex)
        End Try
    End Sub

    Private Sub SendSoftwareScriptTrigger0()
        Try
            ' Send the software trigger 
            _rfsgSession.Triggers.ScriptTriggers(0).SendSoftwareEdgeTrigger()
        Catch ex As Exception
            ShowError("SendSoftwareScriptTrigger0()", ex)
        End Try
    End Sub

    Private Sub SendSoftwareScriptTrigger1()
        Try
            ' Send the software trigger 
            _rfsgSession.Triggers.ScriptTriggers(1).SendSoftwareEdgeTrigger()
        Catch ex As Exception
            ShowError("SendSoftwareScriptTrigger1()", ex)
        End Try
    End Sub

    Private Sub ShowError(ByVal functionName As String, ByVal exception As Exception)
        StopGeneration()
        errorTextBox.Text = "Error in " + functionName + ": " + exception.Message
    End Sub

#End Region

#Region "Form Events"
    Private Sub scriptIndexNumeric_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles scriptIndexNumeric.ValueChanged
        Dim scriptNum As Integer
        scriptNum = CInt(scriptIndexNumeric.Value)
        If scriptNum > 1 Then
            scriptNum = 1
        End If
        scriptTextBox.Text = _scripts(scriptNum)
    End Sub

    Private Sub startButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles startButton.Click
        StartGeneration()
    End Sub

    Private Sub stopButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles stopButton.Click
        StopGeneration()
    End Sub

    Private Sub trigger2Button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles trigger2Button.Click
        SendSoftwareScriptTrigger1()
    End Sub

    Private Sub triggerButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles triggerButton.Click
        SendSoftwareScriptTrigger0()
    End Sub

    Private Sub MainForm_FormClosing(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.FormClosing
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
        frequencyNumeric.Enabled = enabled
        powerLevelNumeric.Enabled = enabled
        iqRateNumeric.Enabled = enabled
        scriptIndexNumeric.Enabled = enabled
        triggerButton.Enabled = Not enabled
        trigger2Button.Enabled = Not enabled And (CInt(scriptIndexNumeric.Value) = 1)

        ' Start the status checking timer
        rfsgStatusTimer.Enabled = Not enabled

        Application.DoEvents()
    End Sub
End Class
