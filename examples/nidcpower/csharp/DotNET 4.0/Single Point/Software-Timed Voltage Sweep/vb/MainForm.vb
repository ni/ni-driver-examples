'=============================================================================================================
'
' Title:
'      NI-DCPower Software-Timed Voltage Sweep
'
' Description:
'      This example demonstrates how to sweep the voltage on a single channel.
'      This example performs a software-timed sweep using Single Point source mode.
'      Do not use this example if your device supports Sequence source mode;
'      use the hardware-timed example instead.
'
'      Note: In this example the Output Function is set to DC Voltage. If you change the
'      Output Function to DC Current, you must use Current Level and Voltage Limit instead
'      of Voltage Level and Current Limit.
'
'  Suggested Devices:
'      PXI-4110, PXI-4130, PXI-4132
'      PXIe-4112, PXIe-4113, PXIe-4135, PXIe-4136, PXIe-4137, PXIe-4138, PXIe-4139, PXIe-4140, PXIe-4141, PXIe-4142, PXIe-4143, PXIe-4144, PXIe-4145, PXIe-4147, PXIe-4154, PXIe-4162, PXIe-4163
'
'=============================================================================================================
Imports System.Windows.Forms
Imports System.Threading
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

#Region "MainForm configuration values"
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

    Private ReadOnly Property NumberOfPoints() As Integer
        Get
            Return Decimal.ToInt32(Me.numberOfPointsNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property VoltageLevelStart() As Double
        Get
            Return Decimal.ToDouble(Me.voltageLevelStartNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property VoltageLevelStop() As Double
        Get
            Return Decimal.ToDouble(Me.voltageLevelStopNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property CurrentLimit() As Double
        Get
            Return Decimal.ToDouble(Me.currentLimitNumeric.Value)
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
        ClearMeasurementsDataGridView()
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
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.Voltage.VoltageLevelAutorange = DCPowerSourceVoltageLevelAutorange.[On]
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.Voltage.CurrentLimitAutorange = DCPowerSourceCurrentLimitAutorange.[On]
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.Voltage.CurrentLimit = CurrentLimit

            dcPowerSession.Control.Initiate()

            ' Calculate the step size.
            Dim stepSize As Double = 0
            If NumberOfPoints > 1 Then ' Avoid dividing by 0.
                stepSize = ((VoltageLevelStop - VoltageLevelStart) / (NumberOfPoints - 1))
            End If

            ' For each step..
            For pointIndex As Integer = 0 To NumberOfPoints - 1
                ' Calculate the Voltage Level for this step.
                Dim voltageLevel As Double = (stepSize * CDbl(pointIndex)) + VoltageLevelStart

                dcPowerSession.Outputs(FullyQualifiedChannelName).Source.Voltage.VoltageLevel = voltageLevel

                ' Delay after sourcing so that the output can settle.
                Thread.Sleep(CInt(SourceDelay.TotalMilliseconds))

                Dim result As DCPowerMeasureResult = dcPowerSession.Measurement.Measure(FullyQualifiedChannelName)
                UpdateMeasurements(result)
            Next

            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.Output.Enabled = False
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

    Private Sub UpdateMeasurements(ByVal result As DCPowerMeasureResult)
        measurementsDataGridView.Rows.Add((measurementsDataGridView.Rows.Count + 1).ToString(), result.VoltageMeasurements(0).ToString("E"), result.CurrentMeasurements(0).ToString("E"))
    End Sub

    Private Sub ClearMeasurementsDataGridView()
        measurementsDataGridView.Rows.Clear()
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