'=============================================================================================================
'
' Title:
'      NI-DCPower Pulse Voltage Sequence
'
' Description:
'      Demonstrates how to use the DCPower Pulse API to generate a Voltage Pulse
'      Sequence. This example initializes a session, configures the Source Mode
'      and the Output Function, configures the pulse parameters, initiates the
'      pulse output sequence, and takes measurements.
'
' Suggested Devices
'      NI PXIe-4135, NI PXIe-4136, NI PXIe-4137, NI PXIe-4138, NI PXIe-4139
'
'=============================================================================================================

Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments.NIDCPower
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Public Partial Class MainForm
    Inherits Form
    Private dcPowerSession As NIDCPower
    Private Const NumberOfStepsInVoltageSequence As Integer = 3
    Private Const NumberOfStepsInSourceDelay As Integer = 3
    Private Const NumberOfStepsInVoltageMeasurements As Integer = 3
    Private Const NumberOfStepsInCurrentMeasurements As Integer = 3
    Public Sub New()
        InitializeComponent()
        LoadDCPowerDeviceNames()
        ConfigurePulseVoltageSeqDataGridView()
        ConfigureSourceDelaysDataGridView()
    End Sub

#Region "UI Initial Value Config Section"
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

    Private Sub ConfigurePulseVoltageSeqDataGridView()
        For i As Integer = 0 To NumberOfStepsInVoltageSequence - 1
            pulseVoltageSequenceDataGridView.Rows.Add((i + 1).ToString(), i.ToString("E"), i.ToString("E"))
        Next
        AddHandler pulseVoltageSequenceDataGridView.CellEnter, New DataGridViewCellEventHandler(AddressOf AllowSingleClickEditInDataGridView)
    End Sub

    Private Sub ConfigureSourceDelaysDataGridView()
        For i As Integer = 0 To NumberOfStepsInSourceDelay - 1
            sourceDelaysDataGridView.Rows.Add(((i + 5) / 100000.0).ToString(), i.ToString("E"), i.ToString("E"))
        Next
        AddHandler sourceDelaysDataGridView.CellEnter, New DataGridViewCellEventHandler(AddressOf AllowSingleClickEditInDataGridView)
    End Sub

    Private Sub ConfigureVoltageMeasurmentsDataGridView()
        For i As Integer = 0 To NumberOfStepsInVoltageMeasurements - 1
            voltageMeasurmentsDataGridView.Rows.Add((i + 1).ToString(), i.ToString("E"), i.ToString("E"))
        Next
        AddHandler voltageMeasurmentsDataGridView.CellEnter, New DataGridViewCellEventHandler(AddressOf AllowSingleClickEditInDataGridView)
    End Sub

    Private Sub ConfigureCurrentMeasurmentsDataGridView()
        For i As Integer = 0 To NumberOfStepsInCurrentMeasurements - 1
            currentMeasurementsDataGridView.Rows.Add((i + 1).ToString(), i.ToString("E"), i.ToString("E"))
        Next
        AddHandler currentMeasurementsDataGridView.CellEnter, New DataGridViewCellEventHandler(AddressOf AllowSingleClickEditInDataGridView)
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

    Private ReadOnly Property PulseVoltageLevelRange() As Double
        Get
            Return Decimal.ToDouble(Me.pulseVoltLevelRangeNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property BiasVoltageLevel() As Double
        Get
            Return Decimal.ToDouble(Me.biasVoltageLevelNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property ApertureTime() As Double
        Get
            Return Decimal.ToDouble(Me.apertureTimeNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property PulseCurrentLimitRange() As Double
        Get
            Return Decimal.ToDouble(Me.pulseCurLimitRangeNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property PulseCurrentLimit() As Double
        Get
            Return Decimal.ToDouble(Me.pulseCurrentLimitNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property PulseBiasCurrentLimit() As Double
        Get
            Return Decimal.ToDouble(Me.biasCurrentLimitNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property PulseOnTime() As Double
        Get
            Return Decimal.ToDouble(Me.pulseOnTimeNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property PulseOffTime() As Double
        Get
            Return Decimal.ToDouble(Me.pulseOffTimeNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property PulseBiasDelay() As PrecisionTimeSpan
        Get
            Return New PrecisionTimeSpan(Decimal.ToDouble(Me.pulseBiasDelayNumeric.Value))
        End Get
    End Property

    Private ReadOnly Property PulseVoltageSequence() As Double()
        Get
            Dim sequence As Double() = New Double(NumberOfStepsInVoltageSequence - 1) {}
            For i As Integer = 0 To NumberOfStepsInVoltageSequence - 1
                sequence(i) = [Double].Parse(pulseVoltageSequenceDataGridView.Rows(i).Cells(0).Value.ToString())
            Next
            Return sequence
        End Get
    End Property

    Private ReadOnly Property SourceDelay() As PrecisionTimeSpan()
        Get
            Dim sequence As PrecisionTimeSpan() = New PrecisionTimeSpan(NumberOfStepsInSourceDelay - 1) {}
            For i As Integer = 0 To NumberOfStepsInSourceDelay - 1
                sequence(i) = New PrecisionTimeSpan(Double.Parse(sourceDelaysDataGridView.Rows(i).Cells(0).Value.ToString()))
            Next
            Return sequence
        End Get
    End Property

#End Region

    Private Sub startButton_Click(sender As Object, e As System.EventArgs) Handles startButton.Click
        ChangeControlState(False)
        Start()
        ChangeControlState(True)
    End Sub

    Private Sub mainForm_Closing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        CloseSession()
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

    Private Sub Start()
        Try
            InitializeDCPowerSession(False)

            dcPowerSession.Source.Mode = DCPowerSourceMode.Sequence
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.Output.[Function] = DCPowerSourceOutputFunction.PulseVoltage
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.SetSequence(PulseVoltageSequence, SourceDelay)
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.PulseVoltage.VoltageLevelRange = PulseVoltageLevelRange
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.PulseVoltage.BiasVoltageLevel = BiasVoltageLevel
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.PulseVoltage.CurrentLimit = PulseCurrentLimit
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.PulseVoltage.CurrentLimitRange = PulseCurrentLimitRange
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.PulseVoltage.BiasCurrentLimit = PulseBiasCurrentLimit
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.PulseOnTime = PulseOnTime
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.PulseOffTime = PulseOffTime
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.PulseBiasDelay = PulseBiasDelay
            dcPowerSession.Outputs(FullyQualifiedChannelName).Measurement.ConfigureApertureTime(ApertureTime, DCPowerMeasureApertureTimeUnits.Seconds)

            dcPowerSession.Control.Initiate()
            Dim result As DCPowerFetchResult = dcPowerSession.Measurement.Fetch(FullyQualifiedChannelName, New PrecisionTimeSpan(10), NumberOfStepsInVoltageSequence)
            UpdateDataGridView(Me.voltageMeasurmentsDataGridView, Me.currentMeasurementsDataGridView, result)
            dcPowerSession.Utility.Reset()
        Catch ex As Exception
            ShowError(ex)
        Finally
            CloseSession()
        End Try
    End Sub

    Private Shared Sub ShowError(ex As Exception)
        MessageBox.Show(ex.Message, ex.[GetType]().ToString(), MessageBoxButtons.OK, MessageBoxIcon.[Error])
    End Sub

    Private Sub InitializeDCPowerSession(reset As Boolean)
        dcPowerSession = New NIDCPower(FullyQualifiedChannelName, reset, String.Empty)
        AddHandler dcPowerSession.DriverOperation.Warning, New EventHandler(Of DCPowerWarningEventArgs)(AddressOf DCPowerDriverOperationWarning)
    End Sub

    Private Sub DCPowerDriverOperationWarning(sender As Object, e As DCPowerWarningEventArgs)
        MessageBox.Show(e.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Sub

    Private Sub ChangeControlState(isEnabled As Boolean)
        resourceAndChannelNameGroupBox.Enabled = isEnabled
        configurationGroupBox.Enabled = isEnabled
        inputSequenceGroupBox.Enabled = isEnabled
        startButton.Enabled = isEnabled
        resourceNameComboBox.[Select]()
        Me.Refresh()
    End Sub

    Private Sub AllowSingleClickEditInDataGridView(sender As Object, e As DataGridViewCellEventArgs)
        Dim target As DataGridView = TryCast(sender, DataGridView)
        If target IsNot Nothing Then
            target.BeginEdit(True)
            Dim cmb As ComboBox = TryCast(target.EditingControl, ComboBox)
            If cmb IsNot Nothing Then
                cmb.DroppedDown = True
            End If
        End If
    End Sub

    Private Shared Sub UpdateDataGridView(voltageTable As DataGridView, currentTable As DataGridView, newResult As DCPowerFetchResult)
        voltageTable.Rows.Add(newResult.VoltageMeasurements.Length)
        currentTable.Rows.Add(newResult.VoltageMeasurements.Length)
        For i As Integer = 0 To newResult.VoltageMeasurements.Length - 1
            voltageTable.Rows(i).Cells(0).Value = newResult.VoltageMeasurements(i).ToString("E")
            currentTable.Rows(i).Cells(0).Value = newResult.CurrentMeasurements(i).ToString("E")
        Next
    End Sub

End Class