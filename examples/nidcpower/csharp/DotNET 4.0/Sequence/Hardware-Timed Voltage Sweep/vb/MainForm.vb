'=============================================================================================================
'
' Title:
'      NI-DCPower Hardware-Timed Voltage Sweep
'
' Description:
'      This example demonstrates how to use triggers and events to synchronize multiple
'      channels in Sequence source mode. Use this example to sequence multiple channels
'      in lock-step.
'
'      Note: In this example the Output Function is set to DC Voltage. If you change the
'      Output Function to DC Current, you must use Current Level and Voltage Limit instead
'      of Voltage Level and Current Limit.
'
'  Suggested Devices:
'      PXI-4132
'      PXIe-4112, PXIe-4113, PXIe-4135, PXIe-4136, PXIe-4137, PXIe-4138, PXIe-4139, PXIe-4140, PXIe-4141, PXIe-4142, PXIe-4143, PXIe-4144, PXIe-4145, PXIe-4147, PXIe-4154, PXIe-4162, PXIe-4163
'
'============================================================================================================

Imports System.Windows.Forms
Imports NationalInstruments
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

    Private ReadOnly Property Timeout() As PrecisionTimeSpan
        Get
            Return New PrecisionTimeSpan(Decimal.ToDouble(Me.timeoutNumeric.Value))
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

            dcPowerSession.Source.Mode = DCPowerSourceMode.Sequence
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.Output.[Function] = DCPowerSourceOutputFunction.DCVoltage
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.Voltage.VoltageLevelAutorange = DCPowerSourceVoltageLevelAutorange.[On]
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.Voltage.CurrentLimitAutorange = DCPowerSourceCurrentLimitAutorange.[On]
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.SourceDelay = SourceDelay
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.Voltage.CurrentLimit = CurrentLimit

            Dim voltageLevelsSequence As Double() = CreateVoltageLevelsSequence()
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.SetSequence(voltageLevelsSequence)

            dcPowerSession.Control.Initiate()
            dcPowerSession.Outputs(FullyQualifiedChannelName).Events.SequenceEngineDoneEvent.WaitForEvent(Timeout)

            Dim result As DCPowerFetchResult = dcPowerSession.Measurement.Fetch(FullyQualifiedChannelName, Timeout, NumberOfPoints)
            DisplayMeasurements(result)

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

    Private Sub ClearMeasurementsDataGridView()
        measurementsDataGridView.Rows.Clear()
    End Sub

    Private Function CreateVoltageLevelsSequence() As Double()
        Dim stepSize As Double = 0.0
        If NumberOfPoints > 1 Then ' To avoid dividing by 0.
            ' Calculate step size.
            stepSize = ((VoltageLevelStop - VoltageLevelStart) / (NumberOfPoints - 1))
        End If
        Dim voltageLevelsSequence As Double() = New Double(NumberOfPoints - 1) {}
        For pointIndex As Integer = 0 To NumberOfPoints - 1
            ' Calculate the Voltage Level for this step.
            voltageLevelsSequence(pointIndex) = (stepSize * CDbl(pointIndex)) + VoltageLevelStart
        Next
        Return voltageLevelsSequence
    End Function

    Private Sub DisplayMeasurements(ByVal result As DCPowerFetchResult)
        For i As Integer = 0 To result.VoltageMeasurements.Length - 1
            measurementsDataGridView.Rows.Add((i + 1).ToString(), result.VoltageMeasurements(i).ToString("E"), result.CurrentMeasurements(i).ToString("E"))
        Next
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
        resourceNameAndChannelNameGroupBox.Enabled = isEnabled
        configurationGroupBox.Enabled = isEnabled
        startButton.Enabled = isEnabled
        resourceNameComboBox.[Select]()
        Me.Refresh()
    End Sub

End Class