
'==================================================================================================
' Title        : Voltage Measurement
' Copyright    : National Instruments 2011. All Rights Reserved.
' Description  : The application demonstrates how to make measurements using the .NET class 
'                library. The application configures the DMM for DC Voltage measurement, 
'                acquires a reading and displays the aquired reading to the user.
'===================================================================================================

Imports System
Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments.NIDmm
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Partial Public Class MainForm
    Inherits Form
    Private sampleDmmSession As NIDmm

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

    Private Sub EnableControls(ByVal enabled As Boolean)
        resolutionValueComboBox.Enabled = enabled
        measurementModeComboBox.Enabled = enabled
        readButton.Enabled = enabled
        resourceNameComboBox.Enabled = enabled
        rangeNumericUpDown.Enabled = enabled
        powerlineFrequencyValueComboBox.Enabled = enabled
    End Sub

    Private Sub Configure()
        Dim range As Double = rangeNumericUpDown.Value
        Dim resolution As Double = Double.Parse(resolutionValueComboBox.Text)
        Dim powerlineFrequency As Double = Double.Parse(powerlineFrequencyValueComboBox.Text)
        'Get the Measurement Mode from the UI
        Dim measurementMode As DmmMeasurementFunction = DmmMeasurementFunction.ACVolts
        measurementMode = DirectCast([Enum].Parse(GetType(DmmMeasurementFunction), measurementModeComboBox.Text), DmmMeasurementFunction)
        'Configure Dmm session Measurement parameters
        sampleDmmSession.ConfigureMeasurementDigits(measurementMode, range, resolution)
        sampleDmmSession.Advanced.PowerlineFrequency = powerlineFrequency
    End Sub

    Private Sub UpdateActualRange(ByVal sampleDmmSession As NIDmm)
        Dim actualRange As Double = sampleDmmSession.Range
        actualRangTextBox.Text = actualRange.ToString()
    End Sub

    Private Sub readButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles readButton.Click
        EnableControls(False)
        messageTextBox.Clear()
        Application.DoEvents()
        Try
            Dim reading As Double
            ' Create a Dmm Session
            sampleDmmSession = New NIDmm(resourceNameComboBox.Text, True, True)
            Configure()
            ' Obtain the reading
            reading = sampleDmmSession.Measurement.Read()
            ' Update the actual range
            UpdateActualRange(sampleDmmSession)
            ' Display the reading
            measurementTextBox.Text = reading.ToString()
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
End Class
