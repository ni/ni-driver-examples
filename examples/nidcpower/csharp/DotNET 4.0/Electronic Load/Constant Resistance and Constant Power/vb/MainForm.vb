'=============================================================================================================
'
' Title:
'       NI-DCPower Constant Resistance and Constant Power
'
' Description:
'      This example demonstrates how to use the Constant Resistance Output Function to force a
'      resistance level on the electronic load and how to use the Constant Power Output Function
'      to force a power level on the electronic load.
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
        LoadDCPowerDeviceNames()
    End Sub

#Region "MainForm initial configuration"

    Private Sub LoadDCPowerDeviceNames()
        Using dcPowerDevices As New ModularInstrumentsSystem("NI-DCPower")
            For Each device As DeviceInfo In dcPowerDevices.DeviceCollection
                resourceNameComboBox.Items.Add(device.Name + "/0")
            Next
        End Using
        If resourceNameComboBox.Items.Count > 0 Then
            resourceNameComboBox.SelectedIndex = 0
        End If
    End Sub
#End Region

#Region "Mainform configuration values"
    Private ReadOnly Property FullyQualifiedChannelName() As String
        Get
            Return Me.resourceNameComboBox.Text
        End Get
    End Property

    Private ReadOnly Property ConstantResistanceLevel As Double
        Get
            Return Decimal.ToDouble(Me.ConstantResistanceLevelNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property ConstantResistanceLevelRange As Double
        Get
            Return Decimal.ToDouble(Me.ConstantResistanceLevelRangeNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property ConstantResistanceCurrentLimit As Double
        Get
            Return Decimal.ToDouble(Me.ConstantResistanceCurrentLimitNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property ConstantPowerLevel As Double
        Get
            Return Decimal.ToDouble(Me.ConstantPowerLevelNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property ConstantPowerLevelRange As Double
        Get
            Return Decimal.ToDouble(Me.ConstantPowerLevelRangeNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property ConstantPowerCurrentLimit As Double
        Get
            Return Decimal.ToDouble(Me.ConstantPowerCurrentLimitNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property SourceDelay As PrecisionTimeSpan
        Get
            Return New PrecisionTimeSpan(Decimal.ToDouble(Me.sourceDelayNumeric.Value))
        End Get
    End Property
#End Region

    Private Sub startButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles startButton.Click
        ChangeControlState(False)
        Start()
        ChangeControlState(True)
    End Sub

    Private Sub mainForm_Closing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        CloseSession()
    End Sub

    Private Sub Start()
        Try
            InitializeDCPowerSession()
            dcPowerSession.Source.Mode = DCPowerSourceMode.SinglePoint

            If Me.constantResistanceConstantPowerTabControl.SelectedTab Is constantResistanceTab Then
                dcPowerSession.Outputs(FullyQualifiedChannelName).Source.Output.[Function] = DCPowerSourceOutputFunction.ConstantResistance
                dcPowerSession.Outputs(FullyQualifiedChannelName).Source.ConstantResistance.Level = ConstantResistanceLevel
                dcPowerSession.Outputs(FullyQualifiedChannelName).Source.ConstantResistance.CurrentLimit = ConstantResistanceCurrentLimit
                dcPowerSession.Outputs(FullyQualifiedChannelName).Source.ConstantResistance.LevelRange = ConstantResistanceLevelRange
            ElseIf Me.constantResistanceConstantPowerTabControl.SelectedTab Is constantPowerTab Then
                dcPowerSession.Outputs(FullyQualifiedChannelName).Source.Output.[Function] = DCPowerSourceOutputFunction.ConstantPower
                dcPowerSession.Outputs(FullyQualifiedChannelName).Source.ConstantPower.Level = ConstantPowerLevel
                dcPowerSession.Outputs(FullyQualifiedChannelName).Source.ConstantPower.CurrentLimit = ConstantPowerCurrentLimit
                dcPowerSession.Outputs(FullyQualifiedChannelName).Source.ConstantPower.LevelRange = ConstantPowerLevelRange
            End If

            ' Configure the Source Delay to allow for sufficient startup delay for the input to reach the desired sinking level.
            ' When starting from a 0 A Or Off state, the electronic load requires additional startup delay before the input
            ' begins to sink the desired level. The default Source Delay in this example takes this startup delay into account.
            ' In cases where the electronic load is already sinking, less settling time may be needed.
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.SourceDelay = SourceDelay
            dcPowerSession.Control.Initiate()
            dcPowerSession.Events.SourceCompleteEvent.WaitForEvent(New PrecisionTimeSpan(10.0))
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

    Private Function calculateResistanceMeasurement(ByVal voltage As Double, ByVal current As Double) As Double
        Return voltage / current
    End Function

    Private Function calculatePowerMeasurement(ByVal voltage As Double, ByVal current As Double) As Double
        Return voltage * current
    End Function

    Private Sub DisplayMeasurements(ByVal voltage As Double, ByVal current As Double, ByVal inCompliance As Boolean)
        Me.currentMeasurementTextBox.Text = current.ToString("E")
        Me.voltageMeasurementTextBox.Text = voltage.ToString("E")
        Me.resistanceMeasurementTextBox.Text = calculateResistanceMeasurement(voltage, current).ToString("E")
        Me.powerMeasurementTextBox.Text = calculatePowerMeasurement(voltage, current).ToString("E")
        Me.inComplianceButtonLed.BackColor = If(inCompliance, Color.Red, SystemColors.Control)
    End Sub

    Private Shared Sub ShowError(ByVal ex As Exception)
        MessageBox.Show(ex.Message, ex.[GetType]().ToString(), MessageBoxButtons.OK, MessageBoxIcon.[Error])
    End Sub

    Private Sub ChangeControlState(isEnabled As Boolean)
        Me.resourceNameAndChannelNameGroupBox.Enabled = isEnabled
        Me.configurationGroupBox.Enabled = isEnabled
        Me.startButton.Enabled = isEnabled
        Me.resourceNameComboBox.[Select]()
        Me.Refresh()
    End Sub

End Class