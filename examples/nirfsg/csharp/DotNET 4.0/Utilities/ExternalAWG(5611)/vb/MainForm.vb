'==================================================================================================
' Title        : External AWG (5611)
' Description  : This example demonstrates how to use the 5611 with             
'			     an external AWG.
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
    Const FrequencyReferenceRate As Double = 10000000.0

    Public Sub New()
        InitializeComponent()

        LoadRfsgDeviceNames()

        ConfigureLOSwitchComboBox()
        ConfigureFrequencyReferenceSourceComboBox()
        ConfigureOutputTerminalComboBox()
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

    Private Sub ConfigureLOSwitchComboBox()
        RemoveHandler loSwitchComboBox.SelectedIndexChanged, AddressOf Me.loSwitchComboBox_SelectedIndexChanged
        loSwitchComboBox.Items.AddRange(New String() {"Associated LO", "External LO"})
        loSwitchComboBox.SelectedIndex = 0
        AddHandler loSwitchComboBox.SelectedIndexChanged, AddressOf Me.loSwitchComboBox_SelectedIndexChanged
    End Sub

    Private Sub ConfigureFrequencyReferenceSourceComboBox()
        Dim refSourceValueList = New List(Of DictionaryEntry)()
        refSourceValueList.Add(New DictionaryEntry("ClkIn", RfsgFrequencyReferenceSource.ClockIn))
        refSourceValueList.Add(New DictionaryEntry("OnboardClock", RfsgFrequencyReferenceSource.OnboardClock))
        refSourceValueList.Add(New DictionaryEntry("PXI_CLK", RfsgFrequencyReferenceSource.PxiClock))
        refSourceValueList.Add(New DictionaryEntry("RefIn", RfsgFrequencyReferenceSource.ReferenceIn))

        frequencyReferenceSourceComboBox.DataSource = refSourceValueList
        frequencyReferenceSourceComboBox.DisplayMember = "Key"
        frequencyReferenceSourceComboBox.ValueMember = "Value"
        frequencyReferenceSourceComboBox.SelectedValue = RfsgFrequencyReferenceSource.OnboardClock
    End Sub

    Private Sub ConfigureOutputTerminalComboBox()
        Dim outputTerminalValueList = New List(Of DictionaryEntry)()
        outputTerminalValueList.Add(New DictionaryEntry("ClkOut", RfsgFrequencyReferenceExportedOutputTerminal.ClockOut))
        outputTerminalValueList.Add(New DictionaryEntry("Do Not Export", RfsgFrequencyReferenceExportedOutputTerminal.DoNotExport))
        outputTerminalValueList.Add(New DictionaryEntry("RefOut", RfsgFrequencyReferenceExportedOutputTerminal.ReferenceOut))
        outputTerminalValueList.Add(New DictionaryEntry("RefOut2", RfsgFrequencyReferenceExportedOutputTerminal.ReferenceOut2))

        outputTerminalComboBox.DataSource = outputTerminalValueList
        outputTerminalComboBox.DisplayMember = "Key"
        outputTerminalComboBox.ValueMember = "Value"
        outputTerminalComboBox.SelectedValue = RfsgFrequencyReferenceExportedOutputTerminal.DoNotExport
    End Sub

#End Region

#Region "Program Functions"

    Private Sub StartGeneration()
        Dim optionsString As String
        Dim resourceName As String
        Dim centerFrequency As Double
        Dim gain As Double
        Dim refClockSource As RfsgFrequencyReferenceSource
        Dim outputTerminal As RfsgFrequencyReferenceExportedOutputTerminal
        Dim externalLO As Integer

        Try
            ' Read in all the control values 
            resourceName = resourceNameComboBox.Text
            centerFrequency = CDbl(centerFrequencyNumeric.Value)
            gain = CDbl(powerLevelNumeric.Value)
            externalLO = loSwitchComboBox.SelectedIndex

            errorTextBox.Text = "No error."
            Application.DoEvents()

            If externalLO = 0 Then
                refClockSource = If(TryCast(frequencyReferenceSourceComboBox.SelectedValue, RfsgFrequencyReferenceSource), RfsgFrequencyReferenceSource.FromString(frequencyReferenceSourceComboBox.Text))
                outputTerminal = If(TryCast(outputTerminalComboBox.SelectedValue, RfsgFrequencyReferenceExportedOutputTerminal), RfsgFrequencyReferenceExportedOutputTerminal.FromString(outputTerminalComboBox.Text))

                optionsString = "DriverSetup=awg:<External>"

                ' Initialize the NIRfsg session
                _rfsgSession = New NIRfsg(resourceName, True, False, optionsString)

                ' Subscribe to Rfsg warnings
                AddHandler _rfsgSession.DriverOperation.Warning, AddressOf Me.DriverOperation_Warning

                _rfsgSession.FrequencyReference.Configure(refClockSource, FrequencyReferenceRate)

                _rfsgSession.FrequencyReference.ExportedOutputTerminal = outputTerminal
            Else
                ' Using external LO 
                optionsString = "DriverSetup=awg:<External>;lo:<External>"

                ' Initialize the NIRfsg session
                _rfsgSession = New NIRfsg(resourceName, True, False, optionsString)

                ' Subscribe to Rfsg warnings
                AddHandler _rfsgSession.DriverOperation.Warning, AddressOf Me.DriverOperation_Warning
            End If

            ' Initiate Generation 
            _rfsgSession.Initiate()

            ' Basic configuration 
            _rfsgSession.RF.Upconverter.CenterFrequency = centerFrequency
            _rfsgSession.RF.Upconverter.Gain = gain

            UpdateActualIQImpairments()

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

    Private Sub StopGeneration()
        ' Activate all stopped controls 
        EnableControls(True)

        Try
            If _rfsgSession IsNot Nothing Then
                ' Unsubscribe from warning events
                RemoveHandler _rfsgSession.DriverOperation.Warning, AddressOf Me.DriverOperation_Warning
                ' Close NIRfsg session
                _rfsgSession.Close()
                _rfsgSession = Nothing
            End If
        Catch ex As Exception
            errorTextBox.Text = "Error in StopGeneration(): " + ex.Message
        End Try
    End Sub

    Private Sub CheckGeneration()
        Try
            ' Check the status of the RFSG 
            _rfsgSession.CheckGenerationStatus()
        Catch ex As Exception
            ShowError("CheckGeneration()", ex)
        End Try
    End Sub

    Private Sub UpdateActualIQImpairments()
        Dim iOffset As Double
        Dim qOffset As Double
        Dim gainImbalance As Double
        Dim skew As Double
        Dim actualGain As Double

        Try
            _rfsgSession.Utility.WaitUntilSettled(10000)

            ' Retrieve IQ impairment information 

            iOffset = _rfsgSession.IQImpairments.IOffset

            qOffset = _rfsgSession.IQImpairments.QOffset

            gainImbalance = _rfsgSession.IQImpairments.GainImbalance

            skew = _rfsgSession.IQImpairments.Skew

            actualGain = _rfsgSession.RF.Upconverter.Gain

            ' Output Impairments 
            actualIOffsetTextBox.Text = iOffset.ToString()
            actualQOffsetTextBox.Text = qOffset.ToString()
            actualGainImbalanceTextBox.Text = gainImbalance.ToString()
            actualSkewTextBox.Text = skew.ToString()
            actualGainTextBox.Text = actualGain.ToString()
        Catch ex As Exception
            ShowError("UpdateActualIQImpairments()", ex)
        End Try
    End Sub

    Private Sub ShowError(ByVal functionName As String, ByVal exception As Exception)
        StopGeneration()
        errorTextBox.Text = "Error in " + functionName + ": " + exception.Message
    End Sub

#End Region

#Region "Form Events"

    Private Sub MainForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        StopGeneration()
    End Sub

    Private Sub stopButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles stopButton.Click
        StopGeneration()
    End Sub

    Private Sub startButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles startButton.Click
        StartGeneration()
    End Sub

    Private Sub rfsgStatusTimer_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles rfsgStatusTimer.Tick
        CheckGeneration()
    End Sub

    Private Sub loSwitchComboBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles loSwitchComboBox.SelectedIndexChanged
        Dim externalLO As Integer
        externalLO = loSwitchComboBox.SelectedIndex
        frequencyReferenceSourceLabel.Visible = (externalLO = 0)
        frequencyReferenceSourceComboBox.Visible = (externalLO = 0)
        outputTerminalLabel.Visible = (externalLO = 0)
        outputTerminalComboBox.Visible = (externalLO = 0)
    End Sub

#End Region

    Private Sub EnableControls(ByVal enabled As Boolean)
        startButton.Enabled = enabled
        stopButton.Enabled = Not enabled
        resourceNameComboBox.Enabled = enabled
        centerFrequencyNumeric.Enabled = enabled
        powerLevelNumeric.Enabled = enabled
        frequencyReferenceSourceComboBox.Enabled = enabled
        outputTerminalComboBox.Enabled = enabled
        loSwitchComboBox.Enabled = enabled

        ' Start the status checking timer
        rfsgStatusTimer.Enabled = Not enabled

        Application.DoEvents()
    End Sub
End Class
