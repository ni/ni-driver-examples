
'==================================================================================================
' Title        : Waveform Acquisition
' Copyright    : National Instruments 2011. All Rights Reserved.
' Description  : The application demonstrates how to configure a DMM for waveform acquisition, 
'                and Read waveform data using the .NET class library.
'===================================================================================================

Imports System
Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments.NIDmm
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Partial Public Class MainForm
    Inherits Form

    Private sampleDmmSession As NIDmm
    Private sampleCount As Integer

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
        LoadMeasurementModes()
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

    Private Sub LoadMeasurementModes()
        acquisitionModeComboBox.Items.Add(DmmMeasurementFunction.WaveformVoltage)
        acquisitionModeComboBox.Items.Add(DmmMeasurementFunction.WaveformCurrent)
        acquisitionModeComboBox.SelectedIndex = 0
    End Sub

    Private Sub UpdateActualRange(ByVal sampleDmmSession As NIDmm)
        Dim actualRange As Double = sampleDmmSession.Range
        actualRangeTextBox.Text = [String].Format("{0:0.000}", actualRange)
    End Sub

    Private Sub EnableControls(ByVal enabled As Boolean)
        rateValueTextBox.Enabled = enabled
        acquisitionModeComboBox.Enabled = enabled
        resourceNameComboBox.Enabled = enabled
        rangeNumericUpDown.Enabled = enabled
        clearButton.Enabled = enabled
        acquireButton.Enabled = enabled
        numberOfSamplesNumericUpDown.Enabled = enabled
    End Sub

    Private Sub UpdateMeasurementDisplay(ByVal analogWaveform As AnalogWaveform(Of Double))
        Dim point As Integer = readingDataGridView.Rows.Count
        Dim readingsBuffer As Double() = analogWaveform.GetRawData()
        Dim i As Integer = 0
        While i < analogWaveform.SampleCount
            point += 1
            readingDataGridView.Rows.Add(point, readingsBuffer(i))
            i += 1
        End While
    End Sub

    Private Sub Configure()
        'Get the Measurement Mode from the UI
        Dim measurementMode As DmmMeasurementFunction = DirectCast([Enum].Parse(GetType(DmmMeasurementFunction), acquisitionModeComboBox.Text), DmmMeasurementFunction)
        Dim range As Double = rangeNumericUpDown.Value
        Dim rate As Double = Double.Parse(rateValueTextBox.Text)
        sampleCount = numberOfSamplesNumericUpDown.Value
        'Configure Dmm session waveform acquisition parameters
        sampleDmmSession.ConfigureWaveformAcquisition(measurementMode, range, rate, sampleCount)
        Dim actualRange As Double = sampleDmmSession.Range
    End Sub

    Private Sub acquireButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles acquireButton.Click
        EnableControls(False)
        messageTextBox.Text = "Acquiring data..."
        Application.DoEvents()
        Dim failedException As Exception = Nothing
        Dim analogWaveform As AnalogWaveform(Of Double)
        Try
            sampleDmmSession = New NIDmm(resourceNameComboBox.Text, True, True)
            Configure()
            analogWaveform = sampleDmmSession.WaveformAcquisition.ReadWaveform(sampleCount, PrecisionTimeSpan.MaxValue)
            UpdateMeasurementDisplay(analogWaveform)
            messageTextBox.Text = "Operation completed successfully."
            UpdateActualRange(sampleDmmSession)
        Catch exception As Exception
            messageTextBox.Text = exception.Message
        Finally
            If (sampleDmmSession IsNot Nothing) Then
                sampleDmmSession.Close()
            End If
            Application.DoEvents()
            EnableControls(True)
        End Try
    End Sub

    Private Sub clearButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clearButton.Click
        readingDataGridView.Rows.Clear()
    End Sub
End Class
