
'==================================================================================================
' Title        : Continuous Acquisition
' Copyright    : National Instruments 2011. All Rights Reserved.
' Description  : The application demonstrates how to configure the DMM for multipoint acquisition
'                and take multipoint readings using the .NET class library.
'===================================================================================================

Imports System
Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments.NIDmm
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices


Partial Public Class MainForm
    Inherits Form
    Private sampleDmmSession As NIDmm
    Private modeACEnabled As [Boolean] = False
    Private Const MaxSamplesPerReading As Integer = 100000
    Private samplesPerReading As Integer
    Private totalNumberOfSamples As Integer = 0
    Private averageReading As Double, minReading As Double = [Double].PositiveInfinity, maxReading As Double = [Double].NegativeInfinity
    Private stopped As Boolean
    Private reading As Double() = New Double(MaxSamplesPerReading) {}

    '/ <summary>
    '/ The main entry point for the application.
    '/ </summary>
    <STAThread()> _
    Shared Sub Main()
        Application.EnableVisualStyles()
        Application.Run(New MainForm)
    End Sub 'Main

    Public Sub New()
        stopped = False
        InitializeComponent()
        LoadDmmDeviceNames()
        LoadPowerlineFrequencyValues()
        LoadMeasurementModes()
        LoadResolutionValues()
    End Sub

    Private Sub ResetValues()
        minReading = [Double].PositiveInfinity
        maxReading = [Double].NegativeInfinity
        averageReading = 0
        totalNumberOfSamples = 0
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
        resourceNameComboBox.Enabled = enabled
        rangeTextBox.Enabled = enabled
        samplesPerReadingNumericUpDown.Enabled = enabled
        powerlineFrequencyValueComboBox.Enabled = enabled
        acquireButton.Enabled = enabled
        stopButton.Enabled = Not enabled
        If modeACEnabled = True Then
            acConfigurationGroupBox.Enabled = enabled
        End If
    End Sub

    Private Sub UpdateActualRange()
        Dim actualRange As Double = sampleDmmSession.Range
        actualRangeTextBox.Text = [String].Format("{0:G5}", actualRange)
    End Sub

    Private Sub UpdateMeasurementDisplay(ByVal reading As Double(), ByVal numberOfMeasurements As Integer)
        Dim sum As Double = 0
        Dim i As Integer = 0
        While i < numberOfMeasurements
            minReading = If((reading(i) < minReading), reading(i), minReading)
            maxReading = If((reading(i) > maxReading), reading(i), maxReading)
            sum += reading(i)
            i += 1
        End While
        averageReading = (sum + averageReading) / (numberOfMeasurements + 1)
        averageReadingTextBox.Text = [String].Format("{0:G8}", averageReading)
        minReadingTextBox.Text = [String].Format("{0:G8}", minReading)
        maxReadingTextBox.Text = [String].Format("{0:G8}", maxReading)
        numberOfSamplesTextBox.Text = totalNumberOfSamples.ToString()
        UpdateActualRange()
    End Sub

    Private Sub ClearMeasurementDisplay()
        minReadingTextBox.Clear()
        maxReadingTextBox.Clear()
        averageReadingTextBox.Clear()
        numberOfSamplesTextBox.Clear()
        actualRangeTextBox.Clear()
    End Sub

    Private Sub TakeMeasurement()
        sampleDmmSession.Trigger.MultiPoint.SampleCount = samplesPerReading
        sampleDmmSession.Measurement.MemoryOptimizedReadMultiPointAsync(samplesPerReading, reading, Nothing)
    End Sub

    Private Sub Configure()
        Dim measurementMode As DmmMeasurementFunction = DirectCast([Enum].Parse(GetType(DmmMeasurementFunction), measurementModeComboBox.Text), DmmMeasurementFunction)
        Dim range As Double = Double.Parse(rangeTextBox.Text)
        Dim resolution As Double = Double.Parse(resolutionValueComboBox.Text)
        samplesPerReading = Integer.Parse(samplesPerReadingNumericUpDown.Text)
        Dim powerlineFrequency As Double = Double.Parse(powerlineFrequencyValueComboBox.Text)
        'Configure Dmm session Measurement parameters
        sampleDmmSession.ConfigureMeasurementDigits(measurementMode, range, resolution)
        'Configure Powerline Frequency
        sampleDmmSession.Advanced.PowerlineFrequency = powerlineFrequency
        'Configure minimum and maximum AC frequency
        If modeACEnabled = True Then
            sampleDmmSession.AC.FrequencyMin = Double.Parse(minACFrequencyTextBox.Text)
            sampleDmmSession.AC.FrequencyMax = Double.Parse(maxACFrequencyTextBox.Text)
        End If
    End Sub

    Private Sub acquireButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles acquireButton.Click
        stopped = False
        EnableControls(False)
        ResetValues()
        messageTextBox.Clear()
        ClearMeasurementDisplay()
        Application.DoEvents()
        Try
            sampleDmmSession = New NIDmm(resourceNameComboBox.Text, True, True)
            AddHandler sampleDmmSession.Measurement.ReadMultiPointCompleted, AddressOf Measurement_ReadMultipointCompleted
            Configure()
            messageTextBox.Text = "Acquisition in progress..."
            TakeMeasurement()
        Catch exception As Exception
            DisplayMessageAndCloseSession(exception.Message)
        End Try
    End Sub

    Sub Measurement_ReadMultipointCompleted(ByVal sender As Object, ByVal e As DmmMeasurementEventArgs(Of Double()))
        If e.[Error] IsNot Nothing Then
            DisplayMessageAndCloseSession(e.[Error].Message)
            Return
        End If
        totalNumberOfSamples += e.ActualNumberOfPoints
        UpdateMeasurementDisplay(e.Reading, e.ActualNumberOfPoints)

        If Not stopped Then
            ' Continue Acquisition
            TakeMeasurement()
        Else
            DisplayMessageAndCloseSession("Finished Acquisition.")
        End If
    End Sub

    Private Sub DisplayMessageAndCloseSession(ByVal messageText As String)
        If (sampleDmmSession IsNot Nothing AndAlso Not sampleDmmSession.IsDisposed) Then
            RemoveHandler sampleDmmSession.Measurement.ReadMultiPointCompleted, AddressOf Measurement_ReadMultipointCompleted
            sampleDmmSession.Close()
        End If
        messageTextBox.Text = messageText
        EnableControls(True)
    End Sub

    Private Sub stopButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles stopButton.Click
        stopped = True
        messageTextBox.Text = "Waiting for completion of acquisition..."
        EnableControls(False)
        stopButton.Enabled = False
        Application.DoEvents()
    End Sub

    Private Sub measurementModeComboBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles measurementModeComboBox.SelectedIndexChanged
        If measurementModeComboBox.Text.Contains("AC") Then
            acConfigurationGroupBox.Enabled = True
            modeACEnabled = True
        Else
            acConfigurationGroupBox.Enabled = False
            modeACEnabled = False
        End If
    End Sub

End Class
