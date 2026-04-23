'=============================================================================================================
'
' Title:
'      NI-DCPower Single Point Multi-Channel Synchronization
'
' Description:
'      Demonstrates how to use triggers and events to synchronize multiple channels in
'      Single Point source mode.
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

    Private Session As NIDCPower

    Public Sub New()
        InitializeComponent()
        ConfigureSlavesConfigurationDataGridView()
        ConfigureSlavesMeasurementDataGridView()
    End Sub

#Region "MainForm initial configuration"
    Private Sub ConfigureSlavesConfigurationDataGridView()
        LoadDCPowerDeviceNames()
        For i As Integer = 0 To NumberOfSlaveDevices - 1
            slavesConfigurationDataGridView.Rows.Add([String].Format("Slave {0}", i), masterConfigurationResourceNameComboBox.Text, "0", 2.ToString("E"), (0.02).ToString("E"))
        Next
        AddHandler slavesConfigurationDataGridView.CellEnter, New DataGridViewCellEventHandler(AddressOf AllowSingleClickEditInDataGridView)
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

    Private Sub ConfigureSlavesMeasurementDataGridView()
        For i As Integer = 0 To NumberOfSlaveDevices - 1
            slavesMeasurementsDataGridView.Rows.Add([String].Format("Slave {0}", i + 1), 0.ToString("E"), 0.ToString("E"))
        Next
    End Sub

    Private Sub LoadDCPowerDeviceNames()
        Using dcPowerDevices As New ModularInstrumentsSystem("NI-DCPower")
            For Each device As DeviceInfo In dcPowerDevices.DeviceCollection
                masterConfigurationResourceNameComboBox.Items.Add(device.Name)
                DirectCast(slavesConfigurationDataGridView.Columns(1), DataGridViewComboBoxColumn).Items.Add(device.Name)
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

    Private ReadOnly Property MasterVoltageLevel() As Double
        Get
            Return Decimal.ToDouble(Me.masterConfigurationVoltageLevelNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property MasterSourceDelay() As PrecisionTimeSpan
        Get
            Return New PrecisionTimeSpan(Decimal.ToDouble(Me.masterConfigurationSourceDelayNumeric.Value))
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

        Friend ReadOnly Property VoltageLevel() As Double
            Get
                Return [Double].Parse(_thisForm.slavesConfigurationDataGridView.Rows(_index).Cells(3).Value.ToString())
            End Get
        End Property

        Friend ReadOnly Property CurrentLimit() As Double
            Get
                Return [Double].Parse(_thisForm.slavesConfigurationDataGridView.Rows(_index).Cells(4).Value.ToString())
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

            ' Configure the master device.
            Session.Outputs(MasterFullyQualifiedChannelName).Source.Mode = DCPowerSourceMode.SinglePoint
            Session.Outputs(MasterFullyQualifiedChannelName).Source.Output.[Function] = DCPowerSourceOutputFunction.DCVoltage
            Session.Outputs(MasterFullyQualifiedChannelName).Source.Voltage.VoltageLevel = MasterVoltageLevel
            Session.Outputs(MasterFullyQualifiedChannelName).Source.Voltage.CurrentLimit = MasterCurrentLimit
            Session.Outputs(MasterFullyQualifiedChannelName).Source.SourceDelay = MasterSourceDelay

            ' Configure the Source trigger. On the master device, the Source trigger is
            ' disabled (None). This means that the device will source without waiting for
            ' a trigger.  When the device starts sourcing, it will export the Source trigger.
            Session.Outputs(MasterFullyQualifiedChannelName).Triggers.SourceTrigger.Disable()

            ' Configure when to take measurements.  On the master device, take a measurement
            ' as soon as the source unit completes (including any source delay).
            Session.Outputs(MasterFullyQualifiedChannelName).Measurement.MeasureWhen = DCPowerMeasurementWhen.AutomaticallyAfterSourceComplete

            Session.Outputs(MasterFullyQualifiedChannelName).Control.Commit()

            ' Configure the slave devices.
            Dim sourceTriggerInputTerminal As String = BuildFullyQualifiedTerminalName(Session, MasterResourceName, MasterChannelName, "SourceTrigger")
            Dim measureTriggerInputTerminal As String = BuildFullyQualifiedTerminalName(Session, MasterResourceName, MasterChannelName, "SourceCompleteEvent")

            Dim slaveFullyQualifiedResourceNames As String = String.Join(",", Slave.Select(Function(i As SlaveConfiguration) i.FullyQualifiedChannelName))

            Session.Outputs(slaveFullyQualifiedResourceNames).Source.Mode = DCPowerSourceMode.SinglePoint
            Session.Outputs(slaveFullyQualifiedResourceNames).Source.Output.[Function] = DCPowerSourceOutputFunction.DCVoltage
            Session.Outputs(slaveFullyQualifiedResourceNames).Source.SourceDelay = New PrecisionTimeSpan(0.00003)
            Session.Outputs(slaveFullyQualifiedResourceNames).Triggers.SourceTrigger.DigitalEdge.Configure(sourceTriggerInputTerminal, DCPowerTriggerEdge.Rising)

            For i As Integer = 0 To NumberOfSlaveDevices - 1
                Session.Outputs(Slave(i).FullyQualifiedChannelName).Source.Voltage.VoltageLevel = Slave(i).VoltageLevel
                Session.Outputs(Slave(i).FullyQualifiedChannelName).Source.Voltage.CurrentLimit = Slave(i).CurrentLimit
            Next

            ' Configure when to take measurements.  On the slave device(s), take a measurement when the
            ' source unit on the master device completes. This is accomplished by setting Measure When
            ' to On Measure Trigger and by setting the input terminal of the Measure trigger to be the
            ' master device's Source Complete event.
            Session.Outputs(slaveFullyQualifiedResourceNames).Measurement.MeasureWhen = DCPowerMeasurementWhen.AutomaticallyAfterSourceComplete
            Session.Outputs(slaveFullyQualifiedResourceNames).Triggers.MeasureTrigger.DigitalEdge.Configure(measureTriggerInputTerminal, DCPowerTriggerEdge.Rising)

            Session.Outputs(slaveFullyQualifiedResourceNames).Control.Commit()

            ' Initiate the slave device(s) and then initiate the master device to start generation
            ' and acquisition.  The order here is significant.

            ' Initiate the slave device(s).  The slave device(s) will
            ' be waiting for the Source Trigger.
            Session.Outputs(slaveFullyQualifiedResourceNames).Control.Initiate()

            ' Initiate the master device. The master device has to be initiated after the slave device(s)
            ' so that when it exports the Source trigger the slave device(s) are already waiting for
            ' the Source trigger.
            Session.Outputs(MasterFullyQualifiedChannelName).Control.Initiate()

            ' Fetch measurements from master device and update Mainform with the new values.
            Dim result As DCPowerFetchResult = Session.Measurement.Fetch(MasterFullyQualifiedChannelName, New PrecisionTimeSpan(1.0), 1)
            masterMeasurementsVoltageTextBox.Text = result.VoltageMeasurements(0).ToString("E")
            masterMeasurementsCurrentTextBox.Text = result.CurrentMeasurements(0).ToString("E")

            ' Fetch measurements from slave device(s) and update Mainform with the new values.
            For i As Integer = 0 To NumberOfSlaveDevices - 1
                result = Session.Measurement.Fetch(Slave(i).FullyQualifiedChannelName, New PrecisionTimeSpan(10.0), 1)
                slavesMeasurementsDataGridView.Rows(i).Cells(1).Value = result.VoltageMeasurements(0).ToString("E")
                slavesMeasurementsDataGridView.Rows(i).Cells(2).Value = result.CurrentMeasurements(0).ToString("E")
            Next

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

    Private Shared Sub ShowError(ByVal ex As Exception)
        MessageBox.Show(ex.Message, ex.[GetType]().ToString(), MessageBoxButtons.OK, MessageBoxIcon.[Error])
    End Sub

    Private Shared Function BuildFullyQualifiedTerminalName(ByVal session As NIDCPower, ByVal resourceName As String, ByVal channelName As String, ByVal localTerminalName As String) As String
        Dim modelName As String = session.Instruments(resourceName).Identity.InstrumentModel
        If modelName.Equals("NI PXI-4132", StringComparison.OrdinalIgnoreCase) Then
            Return [String].Format("/{0}/{1}", resourceName, localTerminalName)
        Else
            Return [String].Format("/{0}/Engine{1}/{2}", resourceName, channelName, localTerminalName)
        End If
    End Function

    Private Sub ChangeControlState(ByVal flag As Boolean)
        masterConfigurationGroupBox.Enabled = flag
        slavesConfigurationGroupBox.Enabled = flag
        startButton.Enabled = flag
        masterConfigurationResourceNameComboBox.[Select]()
        Me.Refresh()
    End Sub

End Class