'=============================================================================================================
'
' Title:
'       NI-DCPower Sink DC Voltage with Output Resistance
'
' Description:
'      Demonstrates how to use the DC Voltage Output Function to force a voltage into the
'      electronic load, and how to configure the electronic load with the Output Resistance
'      and Output Shorted features.
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
                resourceNameComboBox.Items.Add(device.Name & "/0")
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

    Private ReadOnly Property VoltageLevel() As Double
        Get
            Return Decimal.ToDouble(Me.voltageLevelNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property VoltageLevelRange() As Double
        Get
            Return Decimal.ToDouble(Me.voltageLevelRangeNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property CurrentLimit() As Double
        Get
            Return Decimal.ToDouble(Me.currentLimitNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property CurrentLimitRange() As Double
        Get
            Return Decimal.ToDouble(Me.currentLimitRangeNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property OutputResistance() As Double
        Get
            Return Decimal.ToDouble(Me.outputResistanceNumeric.Value)
        End Get
    End Property

  Private ReadOnly Property SourceDelay() As PrecisionTimeSpan
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
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.Output.[Function] = DCPowerSourceOutputFunction.DCVoltage
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.Output.Resistance = OutputResistance
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.Voltage.VoltageLevel = VoltageLevel
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.Voltage.CurrentLimit = CurrentLimit
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.Voltage.VoltageLevelRange = VoltageLevelRange
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.Voltage.CurrentLimitRange = CurrentLimitRange
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.SourceDelay = SourceDelay

            dcPowerSession.Control.Initiate()

            ' Wait for output to settle.
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

    Private Sub DisplayMeasurements(voltage As Double, current As Double, inCompliance As Boolean)
        Me.currentMeasurementTextBox.Text = current.ToString("E")
        Me.voltageMeasurementTextBox.Text = voltage.ToString("E")
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