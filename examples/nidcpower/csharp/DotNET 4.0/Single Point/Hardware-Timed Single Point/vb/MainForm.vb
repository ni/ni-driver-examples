'=============================================================================================================
'
' Title:
'      NI-DCPower Hardware-Timed Single Point
'
' Description:
'      This example demonstrates how to set up a hardware-timed Single Point operation.
'      The hardware is configured to source a voltage, wait for a specified delay, and
'      then take a measurement.  This example uses Single Point source mode.
'
'      Note: In this example the Output Function is set to DC Voltage. If you change the
'      Output Function to DC Current, you must use Current Level and Voltage Limit instead
'      of Voltage Level and Current Limit.
'
'  Suggested Devices:
'      PXI-4110, PXI-4130
'      PXIe-4112, PXIe-4113, PXIe-4140, PXIe-4141, PXIe-4142, PXIe-4143, PXIe-4144, PXIe-4145, PXIe-4147, PXIe-4154, PXIe-4162,, PXIe-4163
'
'============================================================================================================

Imports System.Drawing
Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments.NIDCPower
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Partial Public Class MainForm
    Inherits Form
    Private dcPowerSession As NIDCPower

    Public Sub New()
        InitializeComponent()
        LoadNIDCPowerDeviceNames()
    End Sub

#Region "MainForm initial configuration"

    Private Sub LoadNIDCPowerDeviceNames()
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

    Private ReadOnly Property VoltageLevelRange() As Double
        Get
            Return Decimal.ToDouble(Me.voltageLevelRangeNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property VoltageLevel1() As Double
        Get
            Return Decimal.ToDouble(Me.voltageLevel1Numeric.Value)
        End Get
    End Property

    Private ReadOnly Property VoltageLevel2() As Double
        Get
            Return Decimal.ToDouble(Me.voltageLevel2Numeric.Value)
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

    Private ReadOnly Property FetchTimeout() As PrecisionTimeSpan
        Get
            Return New PrecisionTimeSpan(Decimal.ToDouble(Me.fetchTimeoutNumeric.Value))
        End Get
    End Property
#End Region

    Private Sub startButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles startButton.Click
        ChangeControlState(False)
        Start()
        ChangeControlState(True)
    End Sub

    Private Sub mainForm_Closing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles MyBase.FormClosing
        CloseSession()
    End Sub

    Private Sub Start()
        Try
            InitializeDCPowerSession()

            dcPowerSession.Source.Mode = DCPowerSourceMode.SinglePoint
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.Output.[Function] = DCPowerSourceOutputFunction.DCVoltage
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.Voltage.VoltageLevel = VoltageLevel1
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.Voltage.CurrentLimit = CurrentLimit
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.Voltage.VoltageLevelRange = VoltageLevelRange
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.Voltage.CurrentLimitRange = CurrentLimitRange
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.SourceDelay = SourceDelay
            dcPowerSession.Measurement.Configuration.MeasureWhen = DCPowerMeasurementWhen.AutomaticallyAfterSourceComplete

            dcPowerSession.Control.Initiate()

            ' Fetch the first set of measurements.
            Dim result As DCPowerFetchResult = dcPowerSession.Measurement.Fetch(FullyQualifiedChannelName, FetchTimeout, 1)
            UpdateMeasurement(voltage1MeasurementsTextBox, current1MeasurementsTextBox, inCompliance1ButtonLed, result)

            ' Dynamically reconfigure the voltage level. This is another source operation.
            ' The device waits for the source delay after reprogramming the output and then
            ' takes a measurement.
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.Voltage.VoltageLevel = VoltageLevel2

            ' Fetch the new set of measurements.
            result = dcPowerSession.Measurement.Fetch(FullyQualifiedChannelName, FetchTimeout, 1)
            UpdateMeasurement(voltage2MeasurementsTextBox, current2MeasurementsTextBox, inCompliance2ButtonLed, result)

            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.Output.Enabled = False
        Catch ex As Exception
            ShowError(ex)
        Finally
            CloseSession()
        End Try
    End Sub

    Private Shared Sub UpdateMeasurement(ByVal voltageTextBox As TextBox, ByVal currentTextBox As TextBox, ByVal inComplianceButtonLed As Button, ByVal newResult As DCPowerFetchResult)
        voltageTextBox.Text = newResult.VoltageMeasurements(0).ToString("E")
        currentTextBox.Text = newResult.CurrentMeasurements(0).ToString("E")
        inComplianceButtonLed.BackColor = If(newResult.InCompliance(0), Color.Red, SystemColors.Control)
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

    Private Sub InitializeDCPowerSession()
        dcPowerSession = New NIDCPower(FullyQualifiedChannelName, False, String.Empty)
        AddHandler dcPowerSession.DriverOperation.Warning, New EventHandler(Of DCPowerWarningEventArgs)(AddressOf DCPowerDriverOperationWarning)
    End Sub

    Private Sub DCPowerDriverOperationWarning(ByVal sender As Object, ByVal e As DCPowerWarningEventArgs)
        MessageBox.Show(e.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Sub

    Private Shared Sub ShowError(ByVal ex As Exception)
        MessageBox.Show(ex.Message, ex.[GetType]().ToString(), MessageBoxButtons.OK, MessageBoxIcon.[Error])
    End Sub

    Private Sub ChangeControlState(flag As Boolean)
        resourceNameAndChannelNameGroupBox.Enabled = flag
        configurationGroupBox.Enabled = flag
        voltageLevel1GroupBox.Enabled = flag
        voltageLevel2GroupBox.Enabled = flag
        startButton.Enabled = flag
        resourceNameComboBox.[Select]()
        Me.Refresh()
    End Sub

End Class