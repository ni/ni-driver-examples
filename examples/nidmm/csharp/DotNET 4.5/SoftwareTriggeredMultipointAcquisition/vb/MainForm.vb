
'==================================================================================================
' Title        : Software Triggered Multipoint Acquisition
' Copyright    : National Instruments 2011. All Rights Reserved.
' Description  : This application demonstrates how to configure a DMM for a triggered
'                multipoint acquisition, send a software trigger and fetch the acquired
'		          samples using the .NET class library.
'===================================================================================================

Imports System
Imports NationalInstruments.ModularInstruments.NIDmm
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Partial Public Class MainForm
    Inherits Form

    Private sampleDmmSession As NIDmm
    Private triggerSource As DmmTriggerSource
    Private sampleCount As Integer
    Private softwareTriggerButtonClicked As Boolean = False
    Private mainFormClosed As Boolean = False

    '/ <summary>
    '/ The main entry point for the application.
    '/ </summary>
    <STAThread()> _
    Shared Sub Main()
        Application.EnableVisualStyles()
        Application.Run(New MainForm)
    End Sub 'Main

    Public Sub New()
        InitializeComponent()
        LoadDmmDeviceNames()
        LoadPowerlineFrequencyValues()
        LoadMeasurementModes()
        LoadResolutionValues()
        LoadTriggerSourceOptions()
    End Sub

    Private Sub LoadDmmDeviceNames()
        Dim modularInstrumentsSystem As New ModularInstrumentsSystem("NI-DMM")
        For Each device As DeviceInfo In modularInstrumentsSystem.DeviceCollection
            resourceNameComboBox.Items.Add(device.Name)
        Next
        If modularInstrumentsSystem.DeviceCollection.Count > 0 Then
            resourceNameComboBox.SelectedIndex = 0
        End If
    End Sub

    Private Sub LoadPowerlineFrequencyValues()
        Dim powerlineFrequencyValues As Double() = {50, 60}
        Dim i As Integer = 0
        While i < powerlineFrequencyValues.Length
            powerlineFrequencyValueComboBox.Items.Add(powerlineFrequencyValues(i))
            i += 1
        End While
        powerlineFrequencyValueComboBox.SelectedIndex = 1
    End Sub

    Private Sub LoadMeasurementModes()
        measurementModeComboBox.Items.AddRange([Enum].GetNames(GetType(DmmMeasurementFunction)))
        measurementModeComboBox.Items.Remove(DmmMeasurementFunction.WaveformCurrent.ToString())
        measurementModeComboBox.Items.Remove(DmmMeasurementFunction.WaveformVoltage.ToString())
        measurementModeComboBox.SelectedIndex = 0
    End Sub

    Private Sub LoadResolutionValues()
        Dim resolutionValues As Double() = {3.5, 4.5, 5.5, 6.5, 7.5}
        Dim i As Integer = 0
        While i < resolutionValues.Length
            resolutionValueComboBox.Items.Add(resolutionValues(i))
            i += 1
        End While
        resolutionValueComboBox.SelectedIndex = 3
    End Sub

    Private Sub LoadTriggerSourceOptions()
        Dim triggerSourceValues As String() = {"Immediate", "External", "Software Trigger", "Ttl0", "Ttl1", _
"Ttl2", "Ttl3", "Ttl4", "Ttl5", "Ttl6", "Ttl7", "PxiStar", "LbrTrig1", "AuxTrig1"}
        triggerSourceComboBox.Items.AddRange(triggerSourceValues)
        triggerSourceComboBox.SelectedIndex = 2
    End Sub

    Private Sub UpdateActualRange(ByVal sampleDmmSession As NIDmm)
        Dim actualRange As Double = sampleDmmSession.Range
        actualRangeTextBox.Text = [String].Format("{0:0.000}", actualRange)
    End Sub

    Private Sub EnableControls(ByVal enabled As Boolean)
        resolutionValueComboBox.Enabled = enabled
        measurementModeComboBox.Enabled = enabled
        resourceNameComboBox.Enabled = enabled
        rangeNumericUpDown.Enabled = enabled
        powerlineFrequencyValueComboBox.Enabled = enabled
        clearButton.Enabled = enabled
        triggerDelayNumericUpDown.Enabled = enabled
        triggerSourceComboBox.Enabled = enabled
        numberOfMeasurementsNumericUpDown.Enabled = enabled
        readButton.Enabled = enabled
    End Sub

    Private Sub UpdateMeasurementDisplay(ByVal ParamArray readingArray As Double())
        Dim point As Integer = readingDataGridView.Rows.Count
        For Each reading As Double In readingArray
            point += 1
            readingDataGridView.Rows.Add(point, reading)
        Next
    End Sub

    Private Sub Configure()
        ' Get the Measurement Mode from the UI
        Dim measurementMode As DmmMeasurementFunction = DirectCast([Enum].Parse(GetType(DmmMeasurementFunction), measurementModeComboBox.Text), DmmMeasurementFunction)
        ' Get the Trigger Source from the UI
        ' Using implicit converter of DmmTriggerSource to assign string to DmmTriggerSource
        triggerSource = triggerSourceComboBox.Text
        Dim range As Double = rangeNumericUpDown.Value
        Dim resolution As Double = Double.Parse(resolutionValueComboBox.Text)
        Dim powerlineFrequency As Double = Double.Parse(powerlineFrequencyValueComboBox.Text)
        Dim triggerDelay As Double = triggerDelayNumericUpDown.Value
        sampleCount = numberOfMeasurementsNumericUpDown.Value
        ' Configure Dmm session Measurement parameters
        sampleDmmSession.ConfigureMeasurementDigits(measurementMode, range, resolution)
        sampleDmmSession.Advanced.PowerlineFrequency = powerlineFrequency
        ' Configure Trigger
        sampleDmmSession.Trigger.Configure(triggerSource, PrecisionTimeSpan.FromSeconds(triggerDelay))
        sampleDmmSession.Trigger.MultiPoint.SampleCount = sampleCount
    End Sub

    Private Sub readButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles readButton.Click
        EnableControls(False)
        messageTextBox.Clear()
        Application.DoEvents()
        Dim readingsArray As Double()
        Try
            ' Create a Dmm Session
            sampleDmmSession = New NIDmm(resourceNameComboBox.Text, True, True)
            Configure()
            ' Initiate Aquisition
            sampleDmmSession.Measurement.Initiate()
            UpdateActualRange(sampleDmmSession)
            If sampleDmmSession.Trigger.Source.Equals(DmmTriggerSource.SoftwareTrigger) Then
                softwareTriggerButton.Enabled = True
                ' Poll
                While (Not softwareTriggerButtonClicked And Not mainFormClosed)
                    Application.DoEvents()
                End While
                softwareTriggerButtonClicked = False
                softwareTriggerButton.Enabled = False
                ' Send software trigger
                sampleDmmSession.Measurement.SendSoftwareTrigger()
            End If
            messageTextBox.Text = "Acquisition in progress..."
            Application.DoEvents()
            readingsArray = sampleDmmSession.Measurement.FetchMultiPoint(NationalInstruments.PrecisionTimeSpan.FromSeconds(2 * (sampleCount)), sampleCount)
            UpdateMeasurementDisplay(readingsArray)
            messageTextBox.Text = "Operation completed successfully."

        Catch exception As Exception
            messageTextBox.Text = exception.Message
        Finally
            If sampleDmmSession IsNot Nothing Then
                sampleDmmSession.Close()
            End If
            Application.DoEvents()
            EnableControls(True)
        End Try
    End Sub

    Private Sub softwareTriggerButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles softwareTriggerButton.Click
        softwareTriggerButtonClicked = True
    End Sub

    Private Sub clearButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clearButton.Click
        readingDataGridView.Rows.Clear()
    End Sub

    Private Sub mainFormForm_Closing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        mainFormClosed = True
    End Sub
End Class
