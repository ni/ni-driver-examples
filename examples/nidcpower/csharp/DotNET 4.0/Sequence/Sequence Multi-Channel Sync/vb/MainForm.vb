'=============================================================================================================
'
' Title:
'      NI-DCPower Sequence Multi-Channel Synchronization
'
' Description:
'      This example demonstrates how to use triggers and events to synchronize multiple
'      channels in Sequence source mode.  Use this example to sequence  multiple channels
'      in lock-step.
'
'      Note: In this example the Output Function is set to DC Voltage. If you change the
'      Output Function to DC Current, you must use Current Level and Voltage Limit instead
'      of Voltage Level and Current Limit.
'
' Suggested Devices
'      PXI 4132
'      PXIe-4112, PXIe-4113, PXIe-4135, PXIe-4136, PXIe-4137, PXIe-4138, PXIe-4139, PXIe-4140, PXIe-4141, PXIe-4142, PXIe-4143, PXIe-4144, PXIe-4145, PXIe-4147, PXIe-4154, PXIe-4162, PXIe-4163
'
'=============================================================================================================

Imports System.Linq
Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments.NIDCPower
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Partial Public Class MainForm
    Inherits Form
    Const NumberOfSlaveDevices As Integer = 2
    Const NumberOfStepsInMasterSequence As Integer = 3
    Const NumberOfStepsInSlaveSequence As Integer = 3

    Private Session As NIDCPower

    Public Sub New()
        InitializeComponent()
        ConfigureMasterSequenceDataGridView()
        ConfigureMasterMeasurementDataGridView()
        ConfigureSlavesConfigurationDataGridView()
        ConfigureSlavesSequenceDataGridView()
        ConfigureSlave0MeasurementsDataGridView()
        ConfigureSlave1MeasurementsDataGridView()
    End Sub

#Region "MainForm initial configuration"
    Private Sub ConfigureMasterSequenceDataGridView()
        For i As Integer = 0 To NumberOfStepsInMasterSequence - 1
            masterSequenceDataGridView.Rows.Add((i + 1).ToString(), i.ToString("E"))
        Next
        AddHandler masterSequenceDataGridView.CellEnter, New DataGridViewCellEventHandler(AddressOf AllowSingleClickEditInDataGridView)
    End Sub

    Private Sub ConfigureMasterMeasurementDataGridView()
        For i As Integer = 0 To NumberOfStepsInMasterSequence - 1
            masterMeasurementDataGridView.Rows.Add([String].Format("{0}", i + 1), 0.ToString("E"), 0.ToString("E"))
        Next
    End Sub

    Private Sub ConfigureSlavesSequenceDataGridView()
        For i As Integer = 0 To NumberOfStepsInSlaveSequence - 1
            slavesSequenceDataGridView.Rows.Add([String].Format("{0}", i + 1), i.ToString("E"), i.ToString("E"))
        Next
        AddHandler slavesSequenceDataGridView.CellEnter, New DataGridViewCellEventHandler(AddressOf AllowSingleClickEditInDataGridView)
    End Sub

    Private Sub ConfigureSlavesConfigurationDataGridView()
        LoadDCPowerDeviceNames()
        For i As Integer = 0 To NumberOfSlaveDevices - 1
            slavesConfigurationDataGridView.Rows.Add([String].Format("Slave {0}", i), masterConfigurationResourceNameComboBox.Text, "0", (0.02).ToString("E"))
        Next
        AddHandler slavesConfigurationDataGridView.CellEnter, New DataGridViewCellEventHandler(AddressOf AllowSingleClickEditInDataGridView)
    End Sub

    Private Sub ConfigureSlave0MeasurementsDataGridView()
        For i As Integer = 0 To NumberOfStepsInSlaveSequence - 1
            slave0MeasurementsDataGridView.Rows.Add([String].Format("{0}", i + 1), 0.ToString("E"), 0.ToString("E"))
        Next
    End Sub

    Private Sub ConfigureSlave1MeasurementsDataGridView()
        For i As Integer = 0 To NumberOfStepsInSlaveSequence - 1
            slave1MeasurementsDataGridView.Rows.Add([String].Format("{0}", i + 1), 0.ToString("E"), 0.ToString("E"))
        Next
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
                masterConfigurationResourceNameComboBox.Items.Add(device.Name)
                DirectCast(slavesConfigurationDataGridView.Columns("resourceNameColumn"), DataGridViewComboBoxColumn).Items.Add(device.Name)
                DirectCast(slavesConfigurationDataGridView.Columns("resourceNameColumn"), DataGridViewComboBoxColumn).DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton
            Next
        End Using
        If masterConfigurationResourceNameComboBox.Items.Count > 0 Then
            masterConfigurationResourceNameComboBox.SelectedIndex = 0
        End If
    End Sub
#End Region

#Region "MainForm configuration values"
    Private ReadOnly Property MasterResourceName() As String
        Get
            Return Me.masterConfigurationResourceNameComboBox.Text
        End Get
    End Property

    Private ReadOnly Property MasterChannelName() As String
        Get
            Return Me.masterConfigurationChannelNameTextBox.Text
        End Get
    End Property

    Private ReadOnly Property MasterFullyQualifiedChannelName() As String
        Get
            Return $"{MasterResourceName}/{MasterChannelName}"
        End Get
    End Property

    Private ReadOnly Property MasterCurrentLimit() As Double
        Get
            Return Decimal.ToDouble(Me.masterConfigurationCurrentLimitNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property MasterSourceDelay() As PrecisionTimeSpan
        Get
            Return New PrecisionTimeSpan(Decimal.ToDouble(Me.masterSourceDelayNumeric.Value))
        End Get
    End Property

    Private ReadOnly Property MasterMeasureCompleteEventDelay() As PrecisionTimeSpan
        Get
            Return New PrecisionTimeSpan(Decimal.ToDouble(Me.masterMeasureCompleteEventDelayNumeric.Value))
        End Get
    End Property

    Private ReadOnly Property MasterSequence() As Double()
        Get
            Dim sequence As Double() = New Double(NumberOfStepsInMasterSequence - 1) {}
            For i As Integer = 0 To NumberOfStepsInMasterSequence - 1
                sequence(i) = [Double].Parse(masterSequenceDataGridView.Rows(i).Cells(1).Value.ToString())
            Next
            Return sequence
        End Get
    End Property

    Private Class SlaveConfiguration
        Private _index As Integer
        Private _thisForm As MainForm

        Friend Sub New(ByVal form As MainForm, ByVal index As Integer)
            _index = index
            _thisForm = form
        End Sub

        Friend ReadOnly Property ResourceName() As String
            Get
                Return _thisForm.slavesConfigurationDataGridView.Rows(_index).Cells(1).Value.ToString()
            End Get
        End Property

        Friend ReadOnly Property ChannelName() As String
            Get
                Return _thisForm.slavesConfigurationDataGridView.Rows(_index).Cells(2).Value.ToString()
            End Get
        End Property

        Friend ReadOnly Property FullyQualifiedChannelName() As String
            Get
                Return $"{ResourceName}/{ChannelName}"
            End Get
        End Property

        Friend ReadOnly Property CurrentLimit() As Double
            Get
                Return [Double].Parse(_thisForm.slavesConfigurationDataGridView.Rows(_index).Cells(3).Value.ToString())
            End Get
        End Property

        Friend ReadOnly Property Sequence() As Double()
            Get
                Dim sequence__1 As Double() = New Double(NumberOfStepsInSlaveSequence - 1) {}
                For i As Integer = 0 To NumberOfStepsInSlaveSequence - 1
                    sequence__1(i) = [Double].Parse(_thisForm.slavesSequenceDataGridView.Rows(i).Cells(_index + 1).Value.ToString())
                Next
                Return sequence__1
            End Get
        End Property
    End Class

    Private _slave As SlaveConfiguration()
    Private ReadOnly Property Slave() As SlaveConfiguration()
        Get
            If _slave Is Nothing Then
                _slave = New SlaveConfiguration(NumberOfSlaveDevices - 1) {}
                For i As Integer = 0 To NumberOfSlaveDevices - 1
                    _slave(i) = New SlaveConfiguration(Me, i)
                Next
            End If
            Return _slave
        End Get
    End Property
#End Region

    Private Sub startButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles startButton.Click
        ChangeControlState(False)
        Start()
        ChangeControlState(True)
    End Sub

    Private Sub mainForm_Closing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles MyBase.FormClosing
        CloseSession()
    End Sub

    Private Sub Start()
        Try
            InitializeDCPowerSessions()

            ' Configure master device.
            Session.Outputs(MasterFullyQualifiedChannelName).Source.Mode = DCPowerSourceMode.Sequence
            Session.Outputs(MasterFullyQualifiedChannelName).Source.Output.[Function] = DCPowerSourceOutputFunction.DCVoltage
            Session.Outputs(MasterFullyQualifiedChannelName).Source.SetSequence(MasterSequence)
            Session.Outputs(MasterFullyQualifiedChannelName).Source.Voltage.CurrentLimit = MasterCurrentLimit

            ' Configure the Source Delay on the master device. This delay has to be long enough for
            ' all the devices to finish programming the output and to settle.
            Session.Outputs(MasterFullyQualifiedChannelName).Source.SourceDelay = MasterSourceDelay

            ' Configure the Source trigger. On the master device, the Source trigger is
            ' disabled (None). This means that the device will source without waiting for
            ' a trigger.  When the device starts sourcing, it will export the Source trigger.
            Session.Outputs(MasterFullyQualifiedChannelName).Triggers.SourceTrigger.Disable()

            ' Configure when to take measurements.  On the master device, take a measurement
            ' as soon as the source unit completes (including any source delay).

            Session.Outputs(MasterFullyQualifiedChannelName).Measurement.MeasureWhen = DCPowerMeasurementWhen.AutomaticallyAfterSourceComplete

            ' Configure the Measure Complete Event Delay on the master device. The master device has
            ' to delay generating the Measure Complete event (which triggers the next step of the master's
            ' sequence) until all slave device(s) have completed taking measurements. The amount of
            ' time a slave takes to measure can vary based on its configuration and model.
            Session.Outputs(MasterFullyQualifiedChannelName).Events.MeasureCompleteEvent.Delay = MasterMeasureCompleteEventDelay

            Session.Outputs(MasterFullyQualifiedChannelName).Control.Commit()

            ' Configure slave devices.
            Dim sourceTriggerInputTerminal As String = BuildFullyQualifiedTerminalName(Session, MasterResourceName, MasterChannelName, "SourceTrigger")
            Dim measureTriggerInputTerminal As String = BuildFullyQualifiedTerminalName(Session, MasterResourceName, MasterChannelName, "SourceCompleteEvent")

            Dim slaveFullyQualifiedResourceNames As String = String.Join(",", Slave.Select(Function(i As SlaveConfiguration) i.FullyQualifiedChannelName))

            Session.Outputs(slaveFullyQualifiedResourceNames).Source.Mode = DCPowerSourceMode.Sequence
            Session.Outputs(slaveFullyQualifiedResourceNames).Source.Output.[Function] = DCPowerSourceOutputFunction.DCVoltage

            ' Configure the Source Delay.  On the slave device(s), set the delay to 0, so that the slave(s)
            ' are ready to receive the next trigger from the master as quickly as possible.
            Session.Outputs(slaveFullyQualifiedResourceNames).Source.SourceDelay = New PrecisionTimeSpan(0.00003)

            For i As Integer = 0 To NumberOfSlaveDevices - 1
                Session.Outputs(Slave(i).FullyQualifiedChannelName).Source.SetSequence(Slave(i).Sequence)
                Session.Outputs(Slave(i).FullyQualifiedChannelName).Source.Voltage.CurrentLimit = Slave(i).CurrentLimit
            Next

            ' Configure the Source trigger.  On the slave device(s), the source trigger is the exported
            ' Source trigger from the master device.
            Session.Outputs(slaveFullyQualifiedResourceNames).Triggers.SourceTrigger.DigitalEdge.Configure(sourceTriggerInputTerminal, DCPowerTriggerEdge.Rising)

            ' On the slave device(s), take a measurement when the source unit on the master device completes.
            ' This is accomplished by setting Measure When to On Measure Trigger and by setting the input
            ' terminal of the Measure Trigger to be the master device's Source Complete event.
            Session.Outputs(slaveFullyQualifiedResourceNames).Measurement.MeasureWhen = DCPowerMeasurementWhen.OnMeasureTrigger
            Session.Outputs(slaveFullyQualifiedResourceNames).Triggers.MeasureTrigger.DigitalEdge.Configure(measureTriggerInputTerminal, DCPowerTriggerEdge.Rising)

            Session.Outputs(slaveFullyQualifiedResourceNames).Control.Commit()

            ' Initiate the slave device(s) and then initiate the master device to start generation
            ' and acquisition. The order here is significant.

            ' Initiate the slave device(s). The slave device(s) will
            ' be waiting for the Source Trigger.
            Session.Outputs(slaveFullyQualifiedResourceNames).Control.Initiate()

            ' Initiate the master device. The master device has to be initiated after the slave device(s)
            ' so that when it exports the Source trigger the slave device(s) are already waiting for
            ' the Source trigger.
            Session.Outputs(MasterFullyQualifiedChannelName).Control.Initiate()

            ' Fetch measurements from master device and update Mainform with the new values.
            Dim result As DCPowerFetchResult = Session.Measurement.Fetch(MasterFullyQualifiedChannelName, New PrecisionTimeSpan(1), NumberOfStepsInMasterSequence)
            UpdateDataGridView(Me.masterMeasurementDataGridView, result)

            ' Fetch measurements from slave device(s) and update MainForm with the new values.
            result = Session.Measurement.Fetch(Slave(0).FullyQualifiedChannelName, New PrecisionTimeSpan(10), NumberOfStepsInSlaveSequence)
            UpdateDataGridView(Me.slave0MeasurementsDataGridView, result)

            result = Session.Measurement.Fetch(Slave(1).FullyQualifiedChannelName, New PrecisionTimeSpan(10), NumberOfStepsInSlaveSequence)
            UpdateDataGridView(Me.slave1MeasurementsDataGridView, result)

            ' Reset to disable the output.
            Session.Utility.Reset()

        Catch ex As Exception
            ShowError(ex)
        Finally
            CloseSession()
        End Try
    End Sub

    Private Sub InitializeDCPowerSessions()

        Dim slaveFullyQualifiedResourceNames As String = String.Join(",", Slave.Select(Function(i As SlaveConfiguration) i.FullyQualifiedChannelName))
        Dim fullyQualifiedResourceNames As String = String.Join(",", MasterFullyQualifiedChannelName, slaveFullyQualifiedResourceNames)

        Session = New NIDCPower(fullyQualifiedResourceNames, False, String.Empty)
        AddHandler Session.DriverOperation.Warning, New EventHandler(Of DCPowerWarningEventArgs)(AddressOf DCPowerDriverOperationWarning)
    End Sub

    Private Sub DCPowerDriverOperationWarning(ByVal sender As Object, ByVal e As DCPowerWarningEventArgs)
        MessageBox.Show(e.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Sub

    Private Sub CloseSession()
        Try
            If Session IsNot Nothing Then
                Session.Close()
                Session = Nothing
            End If

        Catch ex As Exception
            ShowError(ex)
            Application.[Exit]()
        End Try
    End Sub

    Private Shared Sub UpdateDataGridView(ByVal table As DataGridView, ByVal newResult As DCPowerFetchResult)
        For i As Integer = 0 To newResult.VoltageMeasurements.Length - 1
            table.Rows(i).Cells(1).Value = [String].Format("{0}", newResult.VoltageMeasurements(i).ToString("E"))
            table.Rows(i).Cells(2).Value = [String].Format("{0}", newResult.CurrentMeasurements(i).ToString("E"))
        Next
    End Sub

    Private Shared Sub ShowError(ByVal ex As Exception)
        MessageBox.Show(ex.Message, ex.[GetType]().ToString(), MessageBoxButtons.OK, MessageBoxIcon.[Error])
    End Sub

    Private Sub ChangeControlState(ByVal isEnabled As Boolean)
        Me.masterConfigurationGroupBox.Enabled = isEnabled
        Me.slavesConfigurationGroupBox.Enabled = isEnabled
        Me.slavesSequenceDataGridView.Enabled = isEnabled
        Me.startButton.Enabled = isEnabled
        Me.masterConfigurationResourceNameComboBox.[Select]()
        Me.Refresh()
    End Sub

    Private Shared Function BuildFullyQualifiedTerminalName(session As NIDCPower, resourceName As String, channelName As String, localTerminalName As String) As String
        Dim modelName As String = session.Instruments(resourceName).Identity.InstrumentModel

        If modelName.Equals("NI PXI-4132", StringComparison.OrdinalIgnoreCase) Then
            Return [String].Format("/{0}/{1}", resourceName, localTerminalName)
        Else
            Return [String].Format("/{0}/Engine{1}/{2}", resourceName, channelName, localTerminalName)
        End If
    End Function

End Class