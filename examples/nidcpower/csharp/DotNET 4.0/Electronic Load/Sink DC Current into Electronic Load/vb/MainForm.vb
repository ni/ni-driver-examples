'=============================================================================================================
'
' Title:
'      NI-DCPower Sink DC Current into Electronic Load
'
' Description:
'      Demonstrates how to use the DC Current Output Function to force a current into the electronic load,
'      and how to configure the electronic load with the Output Shorted, Conduction Voltage and Current Level
'      Slew Rate features.
'
' Suggested Device:
'      PXIe-4051
'
'=============================================================================================================

Imports System.Drawing
Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments.NIDCPower
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Partial Public Class MainForm
    Inherits Form

    Private dcPowerSession As NIDCPower

    Public Sub New()
        InitializeComponent()
        ConfigureConductionVoltageModeComboBox()
        LoadDCPowerDeviceNames()
    End Sub

#Region "MainForm initial configuration"

    Private Sub LoadDCPowerDeviceNames()
        Using dcPowerDevices As ModularInstrumentsSystem = New ModularInstrumentsSystem("NI-DCPower")

            For Each device As DeviceInfo In dcPowerDevices.DeviceCollection
                resourceNameComboBox.Items.Add(device.Name & "/0")
            Next
        End Using

        If resourceNameComboBox.Items.Count > 0 Then
            resourceNameComboBox.SelectedIndex = 0
        End If
    End Sub
#End Region

#Region "Mainform configuration values"
    Private Sub ConfigureConductionVoltageModeComboBox()
        For Each item As DCPowerConductionVoltageMode In [Enum].GetValues(GetType(DCPowerConductionVoltageMode))
            conductionVoltageModeComboBox.Items.Add(item)
        Next

        conductionVoltageModeComboBox.SelectedIndex = 0
    End Sub

    Private ReadOnly Property FullyQualifiedChannelName As String
        Get
            Return Me.resourceNameComboBox.Text
        End Get
    End Property

    Private ReadOnly Property ConductionVoltageMode As DCPowerConductionVoltageMode
        Get
            Return CType(Me.conductionVoltageModeComboBox.SelectedItem, DCPowerConductionVoltageMode)
        End Get
    End Property

    Private ReadOnly Property CurrentLevel As Double
        Get
            Return Decimal.ToDouble(Me.currentLevelNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property CurrentLevelRange As Double
        Get
            Return Decimal.ToDouble(Me.currentLevelRangeNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property VoltageLimitRange As Double
        Get
            Return Decimal.ToDouble(Me.voltageLimitRangeNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property ConductionVoltageOnThreshold As Double
        Get
            Return Decimal.ToDouble(Me.conductionVoltageOnThresholdNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property ConductionVoltageOffThreshold As Double
        Get
            Return Decimal.ToDouble(Me.conductionVoltageOffThresholdNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property CurrentLevelRisingSlewRate As Double
        Get
            Return Decimal.ToDouble(Me.currentLevelRisingSlewRateNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property CurrentLevelFallingSlewRate As Double
        Get
            Return Decimal.ToDouble(Me.currentLevelFallingSlewRateNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property OutputShorted As Boolean
        Get
            Return Me.outputShortedCheckBox.Checked
        End Get
    End Property

    Private ReadOnly Property SourceDelay As PrecisionTimeSpan
        Get
            Return New PrecisionTimeSpan(Decimal.ToDouble(Me.sourceDelayNumeric.Value))
        End Get
    End Property

    Private Sub startButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles startButton.Click
        ChangeControlState(False)
        Start()
        ChangeControlState(True)
    End Sub

    Private Sub mainForm_Closing(ByVal sender As Object, ByVal e As FormClosingEventArgs)
        CloseSession()
    End Sub
#End Region

    Private Sub Start()
        Try
            InitializeDCPowerSession()
            dcPowerSession.Source.Mode = DCPowerSourceMode.SinglePoint
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.Output.[Function] = DCPowerSourceOutputFunction.DCCurrent
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.Current.CurrentLevel = CurrentLevel
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.Current.CurrentLevelRange = CurrentLevelRange

            ' Note that the Voltage Limit property is not applicable for electronic loads and is not configured in this example.
            ' If you change the Output Function, configure the appropriate level, limit and range properties corresponding to your
            ' selected Output Function.
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.Current.VoltageLimitRange = VoltageLimitRange
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.SourceDelay = SourceDelay

            ' Configure the Output Shorted property to specify whether to simulate a short circuit in the electronic load.
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.OutputShorted = OutputShorted

            ' If you are using the DC Current or Constant Power Output Functions, set the Conduction Voltage Mode to "Automatic" or
            ' "Enabled" to enable Conduction Voltage. If you are using the DC Voltage or Constant Resistance Output Functions, set the
            ' Conduction Voltage Mode to "Automatic" or "Disabled" to disable Conduction Voltage.
            ' If Conduction Voltage is enabled, set the Conduction Voltage On Threshold to configure the electronic load to start
            ' sinking current when the input voltage exceeds the configured threshold, and set the Conduction Voltage Off Threshold to
            ' configure the electronic load to stop sinking current when the input voltage falls below the threshold.
            ' If Conduction Voltage is disabled, the electronic load attempts to sink the desired level regardless of the input voltage.
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.ConductionVoltageMode = ConductionVoltageMode
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.ConductionVoltageOnThreshold = ConductionVoltageOnThreshold
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.ConductionVoltageOffThreshold = ConductionVoltageOffThreshold

            ' If you are using the DC Current Output Function, configure the Current Level Rising Slew Rate and Current Level Falling Slew
            ' Rate, in amps per microsecond, to control the rising and falling current slew rates of the electronic load while sinking current.
            ' When using Output Functions besides DC Current, these properties have no effect.
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.Current.CurrentLevelRisingSlewRate = CurrentLevelRisingSlewRate
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.Current.CurrentLevelFallingSlewRate = CurrentLevelFallingSlewRate
            dcPowerSession.Control.Initiate()
            dcPowerSession.Events.SourceCompleteEvent.WaitForEvent(New PrecisionTimeSpan(5.0))
            Dim result As DCPowerMeasureResult = dcPowerSession.Measurement.Measure(FullyQualifiedChannelName)
            Dim inCompliance As Boolean = dcPowerSession.Measurement.QueryInCompliance(FullyQualifiedChannelName)
            DisplayMeasurements(result.VoltageMeasurements(0), result.CurrentMeasurements(0), inCompliance)
            dcPowerSession.Utility.Reset()
        Catch ex As Exception
            ShowError(ex)
        Finally
            CloseSession()
        End Try
    End Sub

    Private Sub InitializeDCPowerSession()
        dcPowerSession = New NIDCPower(FullyQualifiedChannelName, False, String.Empty)
        AddHandler dcPowerSession.DriverOperation.Warning, New EventHandler(Of DCPowerWarningEventArgs)(AddressOf DCPowerDriverOperationWarning)
    End Sub

    Private Sub DCPowerDriverOperationWarning(ByVal sender As Object, ByVal e As DCPowerWarningEventArgs)
        MessageBox.Show(e.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Sub

    Private Sub CloseSession()
        If dcPowerSession IsNot Nothing Then

            Try
                dcPowerSession.Close()
                dcPowerSession = Nothing
            Catch ex As Exception
                ShowError(ex)
                Application.[Exit]()
            End Try
        End If
    End Sub

    Private Sub DisplayMeasurements(ByVal voltage As Double, ByVal current As Double, ByVal inCompliance As Boolean)
        Me.currentMeasurementTextBox.Text = current.ToString("E")
        Me.voltageMeasurementTextBox.Text = voltage.ToString("E")
        Me.inComplianceButtonLed.BackColor = If(inCompliance, Color.Red, SystemColors.Control)
    End Sub

    Private Shared Sub ShowError(ByVal ex As Exception)
        MessageBox.Show(ex.Message, ex.[GetType]().ToString(), MessageBoxButtons.OK, MessageBoxIcon.[Error])
    End Sub

    Private Sub ChangeControlState(ByVal isEnabled As Boolean)
        Me.resourceNameAndChannelNameGroupBox.Enabled = isEnabled
        Me.configurationGroupBox.Enabled = isEnabled
        Me.startButton.Enabled = isEnabled
        Me.resourceNameComboBox.[Select]()
        Me.Refresh()
    End Sub

End Class