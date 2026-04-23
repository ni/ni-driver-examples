'=============================================================================================================
'
' Title:
'       NI-DCPower Export/Import Configuration File
'
' Description:
'      This example demonstrates how to use the Export and Import Attribute Configuration File APIs.
'      Clicking Export will prompt for a file to save the attribute configuration into; initialize a session;
'      configure the Output Function, Voltage Level, and Current Limit; initiate generation; wait for a
'      specified delay; measure the voltage and current output; and export those attributes to file.
'      Clicking Import will prompt for a file to load the attribute configuration from; initialize a session;
'      import the attribute configuration into the session; initiate generation; wait for a specified delay;
'      measure the voltage and current output.
'
'      This example uses Single Point source mode.
'
'      Note: In this example the Output Function is set to  DC Voltage. If you change the
'      Output Function to DC Current, you must use Current Level and Voltage Limit instead
'      of Voltage Level and Current Limit.
'
'  Suggested Devices:
'      PXI-4132
'      PXIe-4112, PXIe-4113, PXIe-4135, PXIe-4136, PXIe-4137, PXIe-4138, PXIe-4139, PXIe-4140, PXIe-4141, PXIe-4142, PXIe-4143, PXIe-4144, PXIe-4145, PXIe-4154
'
'=============================================================================================================

Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments.NIDCPower
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Partial Public Class MainForm
    Inherits Form

    Public Sub New()
        InitializeComponent()
        LoadDCPowerDeviceNames()
    End Sub

#Region "MainForm initial configuration"

    Private Sub LoadDCPowerDeviceNames()
        Using dcPowerDevices As New ModularInstrumentsSystem("NI-DCPower")
            For Each device As DeviceInfo In dcPowerDevices.DeviceCollection
                resourceNameComboBox.Items.Add(device.Name)
            Next
        End Using
        If resourceNameComboBox.Items.Count > 0 Then
            resourceNameComboBox.SelectedIndex = 0
        End If
    End Sub
#End Region

#Region "Mainform configuration values"
    Private ReadOnly Property ResourceName() As String
        Get
            Return Me.resourceNameComboBox.Text
        End Get
    End Property

    Private ReadOnly Property ChannelName() As String
        Get
            Return Me.channelNameTextBox.Text
        End Get
    End Property

    Private ReadOnly Property FullyQualifiedChannelName() As String
        Get
            Return $"{ResourceName}/{ChannelName}"
        End Get
    End Property

    Private Property VoltageLevel() As Double
        Get
            Return Decimal.ToDouble(Me.voltageLevelNumeric.Value)
        End Get
        Set(value As Double)
            Me.voltageLevelNumeric.Value = Convert.ToDecimal(value)
        End Set
    End Property

    Private Property VoltageLevelRange() As Double
        Get
            Return Decimal.ToDouble(Me.voltageLevelRangeNumeric.Value)
        End Get
        Set(value As Double)
            Me.voltageLevelRangeNumeric.Value = Convert.ToDecimal(value)
        End Set
    End Property

    Private Property CurrentLimit() As Double
        Get
            Return Decimal.ToDouble(Me.currentLimitNumeric.Value)
        End Get
        Set(value As Double)
            Me.currentLimitNumeric.Value = Convert.ToDecimal(value)
        End Set
    End Property

    Private Property CurrentLimitRange() As Double
        Get
            Return Decimal.ToDouble(Me.currentLimitRangeNumeric.Value)
        End Get
        Set(value As Double)
            Me.currentLimitRangeNumeric.Value = Convert.ToDecimal(value)
        End Set
    End Property

    Private Property SourceDelay() As PrecisionTimeSpan
        Get
            Return New PrecisionTimeSpan(Decimal.ToDouble(Me.sourceDelayNumeric.Value))
        End Get
        Set(value As PrecisionTimeSpan)
            Me.sourceDelayNumeric.Value = Convert.ToDecimal(value.FractionalSeconds)
        End Set
    End Property
#End Region

    Private Sub startButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles startButton.Click
        ChangeControlState(False)
        Start()
        ChangeControlState(True)
    End Sub

    Private Sub Import_Click(sender As Object, e As EventArgs) Handles importButton.Click
        ChangeControlState(False)

        Dim fileDialog As OpenFileDialog = GetConfigurationFileDialog()
        fileDialog.Title = "Select filename to import configuration from..."

        If fileDialog.ShowDialog() = DialogResult.OK Then
            ImportConfiguration(fileDialog.FileName)
        End If

        ChangeControlState(True)
    End Sub

    Private Sub Export_Click(sender As Object, e As EventArgs) Handles exportButton.Click
        ChangeControlState(False)

        Dim fileDialog As OpenFileDialog = GetConfigurationFileDialog()
        fileDialog.Title = "Select filename to export configuration to..."

        If fileDialog.ShowDialog() = DialogResult.OK Then
            ExportConfiguration(fileDialog.FileName)
        End If

        ChangeControlState(True)
    End Sub

    Private Sub Start()
        ' 1.  Initialize a session to the device.
        ' 2.  Configure session based on UI controls.
        ' 3.  Initiate sourcing and measure.
        ' 4.  Reset to disable the output.
        ' Cleanup of session is handled by "Using".

        Try
            Using dcPowerSession As New NIDCPower(FullyQualifiedChannelName, False, String.Empty)
                AddHandler dcPowerSession.DriverOperation.Warning, AddressOf DCPowerDriverOperationWarning

                Configure(dcPowerSession)
                Measure(dcPowerSession)

                dcPowerSession.Utility.Reset()
            End Using
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub DCPowerDriverOperationWarning(ByVal sender As Object, ByVal e As DCPowerWarningEventArgs)
        MessageBox.Show(e.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Sub

    Private Sub Configure(dcPowerSession As NIDCPower)
        ' 1.  Configure the Source mode to Single Point.
        ' 2.  Set the Output Function to DC Voltage.
        '     If you change the Output Function to DC Current, you must use
        '     Current Level and Voltage Limit instead of Voltage Level and Current Limit.
        ' 3.  Configure the Voltage Level.
        '     This property must be used instead of Voltage Limit because the
        '     Output Function is DC Voltage.
        ' 4.  Configure the Current Limit.
        '     This property must be used instead of Current Level because
        '     the Output Function is DC Voltage.
        ' 5.  Configure the Voltage Level Range.
        ' 6.  Configure the Current Limit Range.
        ' 7.  Configure the Source Delay.

        dcPowerSession.Source.Mode = DCPowerSourceMode.SinglePoint
        dcPowerSession.Outputs(FullyQualifiedChannelName).Source.Output.Function = DCPowerSourceOutputFunction.DCVoltage
        dcPowerSession.Outputs(FullyQualifiedChannelName).Source.Voltage.VoltageLevel = VoltageLevel
        dcPowerSession.Outputs(FullyQualifiedChannelName).Source.Voltage.CurrentLimit = CurrentLimit
        dcPowerSession.Outputs(FullyQualifiedChannelName).Source.Voltage.VoltageLevelRange = VoltageLevelRange
        dcPowerSession.Outputs(FullyQualifiedChannelName).Source.Voltage.CurrentLimitRange = CurrentLimitRange
        dcPowerSession.Outputs(FullyQualifiedChannelName).Source.SourceDelay = SourceDelay
    End Sub

    Private Sub Measure(dcPowerSession As NIDCPower)
        ' 1.  Initiate the device to start generation and acquisition.
        ' 2.  Wait for output to settle.
        ' 3.  Measure the voltage and current.
        ' 4.  Determine if the output is in compliance and update the indicator.

        dcPowerSession.Control.Initiate()
        dcPowerSession.Events.SourceCompleteEvent.WaitForEvent(New PrecisionTimeSpan(5.0))

        Dim voltageMeasurement As Double
        Dim currentMeasurement As Double
        Dim inCompliance As Boolean

        If dcPowerSession.Measurement.Configuration.MeasureWhen = DCPowerMeasurementWhen.OnDemand Then
            Dim result As DCPowerMeasureResult = dcPowerSession.Measurement.Measure(FullyQualifiedChannelName)
            voltageMeasurement = result.VoltageMeasurements(0)
            currentMeasurement = result.CurrentMeasurements(0)
            inCompliance = dcPowerSession.Measurement.QueryInCompliance(FullyQualifiedChannelName)
        Else
            If dcPowerSession.Measurement.Configuration.MeasureWhen = DCPowerMeasurementWhen.OnMeasureTrigger Then
                dcPowerSession.Triggers.MeasureTrigger.SendSoftwareEdgeTrigger()
            End If

            Dim result As DCPowerFetchResult = dcPowerSession.Measurement.Fetch(FullyQualifiedChannelName, PrecisionTimeSpan.FromSeconds(10), 1)
            voltageMeasurement = result.VoltageMeasurements(0)
            currentMeasurement = result.CurrentMeasurements(0)
            inCompliance = result.InCompliance(0)
        End If

        DisplayMeasurements(voltageMeasurement, currentMeasurement, inCompliance)
    End Sub

    Private Sub ExportConfiguration(path As String)
        ' 1.  Initialize a session to the device.
        ' 2.  Configure session based on UI controls.
        ' 3.  Initiate sourcing and measure.
        ' 4.  Export configuration to file.
        ' 5.  Reset to disable the output.
        ' Cleanup of session is handled by "using".

        Try
            Using dcPowerSession As New NIDCPower(FullyQualifiedChannelName, False, String.Empty)
                AddHandler dcPowerSession.DriverOperation.Warning, AddressOf DCPowerDriverOperationWarning

                Configure(dcPowerSession)
                Measure(dcPowerSession)

                dcPowerSession.Utility.ExportAttributeConfigurationFile(path)
                dcPowerSession.Utility.Reset()
            End Using
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub ImportConfiguration(path As String)
        ' 1.  Initialize a session to the device.
        ' 2.  Import configuration from file and write session properties to the UI.
        ' 3.  Initiate sourcing and measure.
        ' 4.  Reset to disable the output.
        ' Cleanup of session is handled by "using".

        Try
            Using dcPowerSession As New NIDCPower(FullyQualifiedChannelName, False, String.Empty)
                AddHandler dcPowerSession.DriverOperation.Warning, AddressOf DCPowerDriverOperationWarning

                dcPowerSession.Utility.ImportAttributeConfigurationFile(path)
                VoltageLevel = dcPowerSession.Outputs(FullyQualifiedChannelName).Source.Voltage.VoltageLevel
                CurrentLimit = dcPowerSession.Outputs(FullyQualifiedChannelName).Source.Voltage.CurrentLimit
                VoltageLevelRange = dcPowerSession.Outputs(FullyQualifiedChannelName).Source.Voltage.VoltageLevelRange
                CurrentLimitRange = dcPowerSession.Outputs(FullyQualifiedChannelName).Source.Voltage.CurrentLimitRange
                SourceDelay = dcPowerSession.Outputs(FullyQualifiedChannelName).Source.SourceDelay

                Measure(dcPowerSession)

                dcPowerSession.Utility.Reset()
            End Using
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub DisplayMeasurements(voltage As Double, current As Double, inCompliance As Boolean)
        Me.currentMeasurementTextBox.Text = current.ToString("E")
        Me.voltageMeasurementTextBox.Text = voltage.ToString("E")
        Me.inComplianceButtonLed.BackColor = If(inCompliance, Color.Red, SystemColors.Control)
    End Sub

    Private Shared Sub ShowError(ByVal ex As Exception)
        MessageBox.Show(ex.Message, ex.GetType().ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

    Private Sub ChangeControlState(isEnabled As Boolean)
        Me.resourceNameAndChannelNameGroupBox.Enabled = isEnabled
        Me.configurationGroupBox.Enabled = isEnabled
        Me.startButton.Enabled = isEnabled
        Me.importButton.Enabled = isEnabled
        Me.exportButton.Enabled = isEnabled
        Me.resourceNameComboBox.Select()
        Me.Refresh()
    End Sub

    Private Function GetConfigurationFileDialog() As OpenFileDialog
        Dim fileDialog As New OpenFileDialog()
        Dim userDir As String = Environment.GetEnvironmentVariable("userprofile")

        fileDialog.InitialDirectory = Path.Combine(userDir, "Desktop")
        fileDialog.Filter = "NI-DCPower configuration files (*.nidcpowerconfig)|*.nidcpowerconfig|All files (*.*)|*.*"
        fileDialog.FilterIndex = 1
        fileDialog.RestoreDirectory = True
        fileDialog.CheckFileExists = False

        Return fileDialog
    End Function

End Class