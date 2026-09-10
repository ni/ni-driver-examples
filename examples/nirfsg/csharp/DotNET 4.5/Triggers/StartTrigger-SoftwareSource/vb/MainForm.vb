'==================================================================================================
' Title        : Start Trigger-Software Source
' Description  : This example demonstrates how to do a software triggered generation. The signal
'			     generated is a sine wave at a specified frequency and power level. 
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

    Public Sub New()
        InitializeComponent()

        ' Enable controls on startup
        EnableControls(True)

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

#Region "Program Functions"

    Private Sub StartGeneration()
        Dim resourceName As String
        Dim frequency As Double, power As Double
        Try
            ' Read in all of the control values 
            resourceName = resourceNameComboBox.Text
            frequency = CDbl(frequencyNumeric.Value)
            power = CDbl(powerLevelNumeric.Value)

            errorTextBox.Text = "No error."
            Application.DoEvents()

            ' Initialize the NIRfsg session
            _rfsgSession = New NIRfsg(resourceName, True, False)

            ' Subscribe to Rfsg warnings
            AddHandler _rfsgSession.DriverOperation.Warning, AddressOf Me.DriverOperation_Warning

            ' Configure the instrument 
            _rfsgSession.RF.Configure(frequency, power)
            _rfsgSession.Triggers.StartTrigger.ConfigureSoftwareTrigger()

            ' Initiate Generation 
            _rfsgSession.Initiate()

            ' Start the status checking timer 
            EnableControls(False)

            ' Activate stop and trigger buttons 
            stopButton.Focus()
        Catch ex As Exception
            ShowError("StartGeneration()", ex)
        End Try
    End Sub

    Private Sub DriverOperation_Warning(ByVal sender As Object, ByVal e As RfsgWarningEventArgs)
        ' Display the rfsg warning
        errorTextBox.Text = e.Message
    End Sub

    Private Sub CheckGeneration()
        Try
            ' Check the status of the RFSG 
            _rfsgSession.CheckGenerationStatus()
        Catch ex As Exception
            ShowError("CheckGeneration()", ex)
        End Try
    End Sub

    Private Sub SendSoftwareTrigger()
        Try
            _rfsgSession.Triggers.StartTrigger.SendSoftwareEdgeTrigger()
        Catch ex As Exception
            ShowError("SendSoftwareTrigger()", ex)
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

    Private Sub sendSoftwareTriggerButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles sendSoftwareTriggerButton.Click
        SendSoftwareTrigger()
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
        sendSoftwareTriggerButton.Enabled = Not enabled
        resourceNameComboBox.Enabled = enabled
        frequencyNumeric.Enabled = enabled
        powerLevelNumeric.Enabled = enabled
        rfsgStatusTimer.Enabled = Not enabled

        Application.DoEvents()
    End Sub
End Class
