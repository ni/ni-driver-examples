
'==================================================================================================
' Title        : NI-DMM Export/Import Configuration File
' Description  : This example demonstrates how to use the Export and Import Attribute Configuration File APIs.
'                Clicking Export will prompt for a file to save the attribute configuration into; initialize a session;
'                configure the measurement function, range, digits of resolution and power line frequency; and export 
'                those attributes to file.
'
'                Clicking Import will prompt for a file to load the attribute configuration from; initialize a session;
'                import the attribute configuration into the session; and display those attributes on the window.
'
'                Clicking Read will take a single measurement with the configuration displayed on the window.
'
'===================================================================================================

Imports System
Imports System.IO
Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments.NIDmm
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Partial Public Class MainForm
    Inherits Form

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
        For Each powerLineFrequencyValue As Double In powerlineFrequencyValues
            powerlineFrequencyValueComboBox.Items.Add(powerLineFrequencyValue)
        Next
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
        For Each resolutionValue As Double In resolutionValues
            resolutionValueComboBox.Items.Add(resolutionValue)
        Next
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

    Private Sub Configure(ByVal dmmSession As NIDmm)
        Dim range As Double = rangeNumericUpDown.Value
        Dim resolution As Double = Double.Parse(resolutionValueComboBox.Text)
        Dim powerlineFrequency As Double = Double.Parse(powerlineFrequencyValueComboBox.Text)
        ' Get the Measurement Mode from the UI
        Dim measurementMode As DmmMeasurementFunction
        If Not DmmMeasurementFunction.TryParse(measurementModeComboBox.Text, measurementMode) Then
            measurementMode = DmmMeasurementFunction.ACVolts
        End If
        ' Configure Dmm session Measurement parameters
        dmmSession.ConfigureMeasurementDigits(measurementMode, range, resolution)
        dmmSession.Advanced.PowerlineFrequency = powerlineFrequency
    End Sub

    Private Sub ExportConfiguration(ByVal path As String)
        Try
            Using dmmSession As New NIDmm(resourceNameComboBox.Text, True, True)
                ' Configure the session
                Configure(dmmSession)
                ' Export the configuration to the specified file
                dmmSession.DriverUtility.ExportAttributeConfigurationFile(path)

                messageTextBox.Text = "Export operation completed successfully."
            End Using
            ' Handle driver-specific exceptions before the general exception handler that follows.
            ' An example of a driver-specific exception is Ivi.Driver.IviCDriverException.
        Catch exception As Exception
            messageTextBox.Text = exception.Message
        End Try
    End Sub

    Private Sub ImportConfiguration(ByVal path As String)
        Try
            Using dmmSession As New NIDmm(resourceNameComboBox.Text, True, True)
                ' Export the configuration to the specified file
                dmmSession.DriverUtility.ImportAttributeConfigurationFile(path)
                ' Display the imported configuration on the UI
                measurementModeComboBox.Text = dmmSession.MeasurementFunction.ToString()
                rangeNumericUpDown.Value = dmmSession.Range
                resolutionValueComboBox.Text = dmmSession.DigitsResolution.ToString()
                powerlineFrequencyValueComboBox.Text = dmmSession.Advanced.PowerlineFrequency.ToString()

                messageTextBox.Text = "Import operation completed successfully."
            End Using
            ' Handle driver-specific exceptions before the general exception handler that follows.
            ' An example of a driver-specific exception is Ivi.Driver.IviCDriverException.
        Catch exception As Exception
            messageTextBox.Text = exception.Message
        End Try
    End Sub

    Private Sub UpdateActualRange(ByVal dmmSession As NIDmm)
        Dim actualRange As Double = dmmSession.Range
        actualRangTextBox.Text = actualRange.ToString()
    End Sub

    Private Sub readButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles readButton.Click
        EnableControls(False)
        messageTextBox.Clear()
        Application.DoEvents()

        Try
            Using dmmSession As New NIDmm(resourceNameComboBox.Text, True, True)
                ' Configure the session
                Configure(dmmSession)
                ' Obtain the reading
                Dim reading As Double = dmmSession.Measurement.Read()
                ' Update the actual range
                UpdateActualRange(dmmSession)
                ' Display the reading
                measurementTextBox.Text = reading.ToString()
                messageTextBox.Text = "Operation completed successfully."
            End Using
        Catch exception As Exception
            messageTextBox.Text = exception.Message
        End Try

        Application.DoEvents()
        EnableControls(True)
    End Sub

    Private Sub Export_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles exportButton.Click
        EnableControls(False)
        messageTextBox.Clear()
        Application.DoEvents()

        Dim fileDialog As OpenFileDialog = GetConfigurationFileDialog()
        fileDialog.Title = "Select filename to export configuration to..."

        If fileDialog.ShowDialog() = DialogResult.OK Then
            ExportConfiguration(fileDialog.FileName)
        End If

        Application.DoEvents()
        EnableControls(True)
    End Sub

    Private Sub Import_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles importButton.Click
        EnableControls(False)
        messageTextBox.Clear()
        Application.DoEvents()

        Dim fileDialog As OpenFileDialog = GetConfigurationFileDialog()
        fileDialog.Title = "Select filename to import configuration from..."

        If fileDialog.ShowDialog() = DialogResult.OK Then
            ImportConfiguration(fileDialog.FileName)
        End If

        Application.DoEvents()
        EnableControls(True)
    End Sub

    Private Function GetConfigurationFileDialog() As OpenFileDialog
        Dim fileDialog As New OpenFileDialog()
        Dim userDir As String = Environment.GetEnvironmentVariable("userprofile")

        fileDialog.InitialDirectory = Path.Combine(userDir, "Desktop")
        fileDialog.Filter = "NI-DMM configuration files (*.nidmmconfig)|*.nidmmconfig|All files (*.*)|*.*"
        fileDialog.FilterIndex = 1
        fileDialog.RestoreDirectory = True
        fileDialog.CheckFileExists = False

        Return fileDialog
    End Function
End Class
