'==================================================================================================
' Title        : External AWG (5610)
' Description  : This example demonstrates how to use NI-RFSG in Upconverter
'			     Only Mode. In this mode, the driver operates the NI PXI-5610
'			     using NI-RFSG without having an AWG.
'==================================================================================================

Imports System
Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments.NIRfsg
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Partial Public Class MainForm
    Inherits Form
    Private _rfsgSession As NIRfsg
    Const RfsgMaxSettlingTime As Integer = 10000

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

#Region "Program Functions"

    Private Sub StartGeneration()
        Dim optionsString As String = "DriverSetup = UpconverterOnly:1"
        Dim resourceName As String
        Dim frequency As Double
        Dim gain As Double
        Dim bandwidth As Double
        Dim arbCarrierFrequency As Double
        Dim actualFrequency As Double
        Dim actualGain As Double
        Try
            ' Read in all the control values 
            resourceName = resourceNameComboBox.Text
            frequency = CDbl(frequencyNumeric.Value)
            gain = CDbl(gainNumeric.Value)
            bandwidth = CDbl(bandwidthNumeric.Value)
            arbCarrierFrequency = CDbl(arbCarrierNumeric.Value)

            errorTextBox.Text = "No error."

            ' Initialize the NIRfsg session
            _rfsgSession = New NIRfsg(resourceName, True, False, optionsString)

            ' Subscribe to Rfsg warnings
            AddHandler _rfsgSession.DriverOperation.Warning, AddressOf Me.DriverOperation_Warning

            ' Basic configuration 
            _rfsgSession.RF.Upconverter.CenterFrequency = frequency
            _rfsgSession.RF.Upconverter.Gain = gain
            _rfsgSession.Arb.SignalBandwidth = bandwidth
            _rfsgSession.Arb.CarrierFrequency = arbCarrierFrequency

            ' Commit settings 
            _rfsgSession.Utility.Commit()
            _rfsgSession.Utility.WaitUntilSettled(RfsgMaxSettlingTime)

            ' Retrieve some information 
            actualFrequency = _rfsgSession.RF.Upconverter.CenterFrequency
            actualGain = _rfsgSession.RF.Upconverter.Gain

            actualFrequencyTextBox.Text = actualFrequency.ToString()
            actualGainTextBox.Text = actualGain.ToString()

            ' Unsubscribe from warning events
            RemoveHandler _rfsgSession.DriverOperation.Warning, AddressOf Me.DriverOperation_Warning

            ' Close NIRfsg session
            _rfsgSession.Close()
        Catch ex As Exception
            ShowError("StartGeneration()", ex)
        End Try
    End Sub

    Private Sub StopGeneration()
        Try
            ' Close NIRfsg session
            If _rfsgSession IsNot Nothing Then
                _rfsgSession.Close()
            End If
            _rfsgSession = Nothing
        Catch ex As Exception
            ShowError("StopGeneration", ex)
        End Try
    End Sub

    Private Sub DriverOperation_Warning(ByVal sender As Object, ByVal e As RfsgWarningEventArgs)
        ' Display the rfsg warning
        errorTextBox.Text = e.Message
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

    Private Sub MainForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        StopGeneration()
    End Sub

#End Region

End Class
