'=============================================================================================================
'
' Title:
'      NI-DCPower Measure Step Response
'
' Description:
'      This example demonstrates how to measure while the output is changing.
'      This example can be used to configure the transient response and observe
'      how the settings change the output.
'
'      Note: In this example the Output Function is set to DC Voltage. If you change the
'      Output Function to DC Current, you must use Current Level and Voltage Limit instead
'      of Voltage Level and Current Limit.
'
'  Suggested Devices:
'      PXIe-4135, PXIe-4136, PXIe-4137, PXIe-4138, PXIe-4139, PXIe-4140, PXIe-4141, PXIe-4142, PXIe-4143, PXIe-4144, PXIe-4145, PXIe-4147, PXIe-4162, PXIe-4163
'
'====================================================================================

Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments.NIDCPower
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Partial Public Class MainForm
    Inherits Form
    Const SequenceSize As Integer = 2

    Private dcPowerSession As NIDCPower

    Public Sub New()
        InitializeComponent()
        ConfigureTransientResponseComboBox()
        ConfigureVoltageSetpointsDataGridView()
        LoadDCPowerDeviceNames()
    End Sub

#Region "MainForm initial configuration"
    Private Sub ConfigureTransientResponseComboBox()
        transientResponseComboBox.Items.Add(DCPowerSourceTransientResponse.Normal)
        transientResponseComboBox.Items.Add(DCPowerSourceTransientResponse.Fast)
        transientResponseComboBox.Items.Add(DCPowerSourceTransientResponse.Slow)
        transientResponseComboBox.SelectedIndex = 0
    End Sub

    Private Sub ConfigureVoltageSetpointsDataGridView()
        For i As Integer = 0 To SequenceSize - 1
            voltageSetPointsDataGridView.Rows.Add((i + 1).ToString(), (i * 5.0).ToString("E"))
        Next
        AddHandler voltageSetPointsDataGridView.CellEnter, New DataGridViewCellEventHandler(AddressOf AllowSingleClickEditInDataGridView)
    End Sub

    Private Sub AllowSingleClickEditInDataGridView(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs)
        Dim target As DataGridView = TryCast(sender, DataGridView)
        If target IsNot Nothing Then
            target.BeginEdit(True)
            Dim cmb As ComboBox = TryCast(target.EditingControl, ComboBox)
            If cmb IsNot Nothing Then
                cmb.DroppedDown = True
            End If
        End If
    End Sub

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

    Private ReadOnly Property MeasureRecordLength() As Integer
        Get
            Return Decimal.ToInt32(Me.measureRecordLengthNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property TransientResponse() As DCPowerSourceTransientResponse
        Get
            Return CType(Me.transientResponseComboBox.SelectedItem, DCPowerSourceTransientResponse)
        End Get
    End Property

    Private ReadOnly Property ApertureTime() As Double
        Get
            Return Decimal.ToDouble(Me.apertureTimeNumericUpDown.Value)
        End Get
    End Property

    Private ReadOnly Property VoltageSetpoints() As Double()
        Get
            Dim sequence As Double() = New Double(Me.voltageSetPointsDataGridView.Rows.Count - 1) {}
            For i As Integer = 0 To sequence.Length - 1
                sequence(i) = [Double].Parse(Me.voltageSetPointsDataGridView.Rows(i).Cells(1).Value.ToString())
            Next
            Return sequence
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
            InitializeDCPowerSession(reset:=True)

            dcPowerSession.Source.Mode = DCPowerSourceMode.Sequence
            dcPowerSession.Outputs(FullyQualifiedChannelName).Measurement.ConfigureApertureTime(ApertureTime, DCPowerMeasureApertureTimeUnits.Seconds)

            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.Output.[Function] = DCPowerSourceOutputFunction.DCVoltage
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.Voltage.CurrentLimit = CurrentLimit
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.Voltage.VoltageLevelRange = VoltageLevelRange
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.Voltage.CurrentLimitRange = CurrentLimitRange
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.TransientResponse = TransientResponse

            ' Configure the source delay to 0 in order to view the entire step response.
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.SourceDelay = New PrecisionTimeSpan(0.0)

            dcPowerSession.Outputs(FullyQualifiedChannelName).Measurement.RecordLength = MeasureRecordLength
            dcPowerSession.Measurement.Configuration.MeasureWhen = DCPowerMeasurementWhen.AutomaticallyAfterSourceComplete

            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.SetSequence(VoltageSetpoints)
            dcPowerSession.Control.Commit()

            ' Read the measure record delta time to determine the length of time between each
            ' measurement. The length of time between each measurement is used for graphing
            ' later in the example and to compute number of measurements to fetch.
            Dim measureRecordDeltaTime As Double = dcPowerSession.Outputs(FullyQualifiedChannelName).Measurement.RecordDeltaTime

            dcPowerSession.Control.Initiate()

            ' Fetch timeout must be at least (measure record delta time measure record length number
            ' of measure records). Multiply this by 2 to ensure that the timeout is large enough.
            Dim fetchTimeout As New PrecisionTimeSpan(measureRecordDeltaTime * MeasureRecordLength * SequenceSize * 2)
            Dim result As DCPowerFetchResult = dcPowerSession.Measurement.Fetch(FullyQualifiedChannelName, fetchTimeout, MeasureRecordLength * SequenceSize)
            UpdateMeasurements(result.VoltageMeasurements, result.CurrentMeasurements)

            dcPowerSession.Utility.Reset()
        Catch ex As Exception
            ShowError(ex)
        Finally
            CloseSession()
        End Try
    End Sub

    Private Sub InitializeDCPowerSession(ByVal reset As Boolean)
        dcPowerSession = New NIDCPower(FullyQualifiedChannelName, reset, String.Empty)
        AddHandler dcPowerSession.DriverOperation.Warning, New EventHandler(Of DCPowerWarningEventArgs)(AddressOf DCPowerDriverOperationWarning)
    End Sub

    Private Sub DCPowerDriverOperationWarning(ByVal sender As Object, ByVal e As DCPowerWarningEventArgs)
        MessageBox.Show(e.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Sub

    Private Sub UpdateMeasurements(ByVal voltageMeasurements As Double(), ByVal currentMeasurements As Double())
        For i As Integer = 0 To voltageMeasurements.Length - 1
            measurementsDataGridView.Rows.Add((i + 1).ToString(), voltageMeasurements(i).ToString("E"), currentMeasurements(i).ToString("E"))
        Next
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
        resourceNameAndChannelNameGroupBox.Enabled = isEnabled
        startButton.Enabled = isEnabled
        configurationGroupBox.Enabled = isEnabled
        resourceNameComboBox.[Select]()
        Me.Refresh()
    End Sub

End Class