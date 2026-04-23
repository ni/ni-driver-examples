'=============================================================================================================
'
' Title:
'       NI-DCPower Source DC Voltage
'
' Description:
'      This example demonstrates how to use the DC Voltage Output Function to force an output
'      voltage. This example initializes a session; configures the Output Function,
'      Autorange, Voltage Level, and Current Limit; initiates generation; waits for a specified
'      delay; and measures the voltage and current output. This example uses Single Point
'      source mode.
'
'      Note: In this example the Output Function is set to  DC Voltage. If you change the
'      Output Function to DC Current, you must use Current Level and Voltage Limit instead
'      of Voltage Level and Current Limit.
'
'  Suggested Devices:
'      PXI-4110, PXI-4130, PXI-4132
'      PXIe-4112, PXIe-4113, PXIe-4135, PXIe-4136, PXIe-4137, PXIe-4138, PXIe-4139, PXIe-4140, PXIe-4141, PXIe-4142, PXIe-4143, PXIe-4144, PXIe-4145, PXIe-4154
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