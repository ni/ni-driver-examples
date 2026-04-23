'=============================================================================================================
'
' Title:
'      NI-DCPower Simultaneous Operation
'
' Description:
'      This example demonstrates how to simultaneously change the configuration on multiple
'      channels.  This example initializes a session, configures the Voltage Levels and
'      Current Limits on two channels, and updates the outputs of the device simultaneously
'      and then takes measurements on both channels.
'
'      Note: In this example the Output Function is set to DC Voltage. If you change the
'      Output Function to DC Current, you must use Current Level and Voltage Limit instead
'      of Voltage Level and Current Limit.
'
' Suggested Devices
'      PXI 4110, PXI 4130
'      PXIe-4112, PXIe-4113, PXIe-4140, PXIe-4141, PXIe-4142, PXIe-4143, PXIe-4144, PXIe-4145, PXIe-4147, PXIe-4154, PXIe-4162, PXIe-4163
'
'=============================================================================================================

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

#Region "MainForm initial values"
    Private ReadOnly Property ResourceName() As String
        Get
            Return Me.resourceNameComboBox.Text
        End Get
    End Property

    Private ReadOnly Property FirstChannelName() As String
        Get
            Return Me.firstChannelNameTextBox.Text
        End Get
    End Property

    Private ReadOnly Property FirstFullyQualifiedChannelName() As String
        Get
            Return $"{ResourceName}/{FirstChannelName}"
        End Get
    End Property

    Private ReadOnly Property FirstChannelVoltageLevel() As Double
        Get
            Return Decimal.ToDouble(Me.firstChannelVoltagLevelNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property FirstChannelCurrentLimit() As Double
        Get
            Return Decimal.ToDouble(Me.firstChannelCurrentLimitNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property SecondChannelName() As String
        Get
            Return Me.secondChannelNameTextBox.Text
        End Get
    End Property

    Private ReadOnly Property SecondFullyQualifiedChannelName() As String
        Get
            Return $"{ResourceName}/{SecondChannelName}"
        End Get
    End Property

    Private ReadOnly Property SecondChannelVoltageLevel() As Double
        Get
            Return Decimal.ToDouble(Me.secondChannelVoltageLevelNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property SecondChannelCurrentLimit() As Double
        Get
            Return Decimal.ToDouble(Me.secondChannelCurrentLimitNumeric.Value)
        End Get
    End Property

#End Region

    Private Sub startButton_Click(sender As Object, e As EventArgs) Handles startButton.Click
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

            ' "" means all channels in the session
            dcPowerSession.Outputs("").Source.Output.[Function] = DCPowerSourceOutputFunction.DCVoltage

            dcPowerSession.Outputs(FirstFullyQualifiedChannelName).Source.Voltage.VoltageLevel = FirstChannelVoltageLevel
            dcPowerSession.Outputs(FirstFullyQualifiedChannelName).Source.Voltage.CurrentLimit = FirstChannelCurrentLimit

            dcPowerSession.Outputs(SecondFullyQualifiedChannelName).Source.Voltage.VoltageLevel = SecondChannelVoltageLevel
            dcPowerSession.Outputs(SecondFullyQualifiedChannelName).Source.Voltage.CurrentLimit = SecondChannelCurrentLimit

            dcPowerSession.Control.Initiate()

            Dim fullyQualifiedChannelNames As String = [String].Join(",", FirstFullyQualifiedChannelName, SecondFullyQualifiedChannelName)
            dcPowerSession.Events.SourceCompleteEvent.WaitForEvent(New PrecisionTimeSpan(5.0))
            Dim result As DCPowerMeasureResult = dcPowerSession.Measurement.Measure(fullyQualifiedChannelNames)

            UpdateMeasurements(result)
        Catch ex As Exception
            ShowError(ex)
        Finally
            CloseSession()
        End Try
    End Sub

    Private Sub UpdateMeasurements(newResult As DCPowerMeasureResult)
        firstChaannelVoltageMeasurementTextBox.Text = newResult.VoltageMeasurements(0).ToString("E")
        firstChannelCurrentMeasurementTextBox.Text = newResult.CurrentMeasurements(0).ToString("E")

        secondChaannelVoltageMeasurementTextBox.Text = newResult.VoltageMeasurements(1).ToString("E")
        secondChannelCurrentMeasurementTextBox.Text = newResult.CurrentMeasurements(1).ToString("E")
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
        Dim fullyQualifiedResourceNames As String = [String].Join(",", FirstFullyQualifiedChannelName, SecondFullyQualifiedChannelName)
        dcPowerSession = New NIDCPower(fullyQualifiedResourceNames, False, String.Empty)
        AddHandler dcPowerSession.DriverOperation.Warning, New EventHandler(Of DCPowerWarningEventArgs)(AddressOf DCPowerDriverOperationWarning)
    End Sub

    Private Sub DCPowerDriverOperationWarning(ByVal sender As Object, ByVal e As DCPowerWarningEventArgs)
        MessageBox.Show(e.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Sub

    Private Shared Sub ShowError(ByVal ex As Exception)
        MessageBox.Show(ex.Message, ex.[GetType]().ToString(), MessageBoxButtons.OK, MessageBoxIcon.[Error])
    End Sub

    Private Sub ChangeControlState(isEnabled As Boolean)
        resourceNameGroupBox.Enabled = isEnabled
        firstChannelGroupBox.Enabled = isEnabled
        secondChannelGroupBox.Enabled = isEnabled
        startButton.Enabled = isEnabled
        resourceNameComboBox.[Select]()
        Me.Refresh()
    End Sub

End Class