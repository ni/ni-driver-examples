'=============================================================================================================
'
' Title:
'     NI-DCPower Hardware-Timed Two-Channel Sweep
'
' Description:
'      This example demonstrates how to set up a hardware-timed two-channel nested
'      voltage sweep. Use this example to produce the characteristic curves of a FET transistor.
'      It can be easily adapted to test a BJT by performing a current sweep instead of
'      a voltage sweep. This example performs a hardware-timed sweep (with triggers
'      and events) using Sequence source mode.
'
'      Note: In this example the Output Function is set to DC Voltage. If you change the
'      Output Function to DC Current, you must use Current Level and Voltage Limit instead
'      of Voltage Level and Current Limit.
'
' Suggested Devices:
'      PXI-4132
'      PXIe-4112, PXIe-4113, PXIe-4135, PXIe-4136, PXIe-4137, PXIe-4138, PXIe-4139, PXIe-4140, PXIe-4141, PXIe-4142, PXIe-4143, PXIe-4144, PXIe-4145, PXIe-4147, PXIe-4154, PXIe-4162, PXIe-4163
'
'============================================================================================================

Imports System.Collections.Generic
Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments.NIDCPower
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Partial Public Class MainForm
    Inherits Form
    Private session As NIDCPower
    Private measurements As New Dictionary(Of String, DCPowerFetchResult)

    Public Sub New()
        InitializeComponent()
        ConfigureSenseComboBox()
        LoadDCPowerDeviceNames()
    End Sub

#Region "MainForm initial configuration"
    Private Sub ConfigureSenseComboBox()
        For Each item As DCPowerMeasurementSense In [Enum].GetValues(GetType(DCPowerMeasurementSense))
            device0SenseComboBox.Items.Add(item)
            device1SenseComboBox.Items.Add(item)
        Next
        device0SenseComboBox.SelectedIndex = 0
        device1SenseComboBox.SelectedIndex = 0
    End Sub

    Private Sub LoadDCPowerDeviceNames()
        Using dcPowerDevices As New ModularInstrumentsSystem("NI-DCPower")
            For Each device As DeviceInfo In dcPowerDevices.DeviceCollection
                device0ResourceNameComboBox.Items.Add(device.Name)
                device1ResourceNameComboBox.Items.Add(device.Name)
            Next
        End Using
        If device0ResourceNameComboBox.Items.Count > 0 Then
            device0ResourceNameComboBox.SelectedIndex = 0
        End If
        If device1ResourceNameComboBox.Items.Count > 1 Then
            device1ResourceNameComboBox.SelectedIndex = 1
        End If
    End Sub
#End Region

#Region "MainForm configuration values"
    Friend ReadOnly Property ResourceName0() As String
        Get
            Return Me.device0ResourceNameComboBox.Text
        End Get
    End Property

    Friend ReadOnly Property ChannelName0() As String
        Get
            Return Me.device0ChannelNameTextBox.Text
        End Get
    End Property

    Friend ReadOnly Property FullyQualifiedChannelName0() As String
        Get
            Return $"{ResourceName0}/{ChannelName0}"
        End Get
    End Property

    Friend ReadOnly Property NumberOfPlots0() As Integer
        Get
            Return Decimal.ToInt32(Me.device0NumberOfPlotsNumeric.Value)
        End Get
    End Property

    Friend ReadOnly Property CurrentLimit0() As Double
        Get
            Return Decimal.ToDouble(Me.device0CurrentLimitNumeric.Value)
        End Get
    End Property

    Friend ReadOnly Property VoltageLevelStart0() As Double
        Get
            Return Decimal.ToDouble(Me.device0VoltageLevelStartNumeric.Value)
        End Get
    End Property

    Friend ReadOnly Property VoltageLevelStop0() As Double
        Get
            Return Decimal.ToDouble(Me.device0VoltageLevelStopNumeric.Value)
        End Get
    End Property

    Friend ReadOnly Property SourceDelay0() As PrecisionTimeSpan
        Get
            Return New PrecisionTimeSpan(Decimal.ToDouble(Me.device0SourceDelayNumeric.Value))
        End Get
    End Property

    Friend ReadOnly Property Sense0() As DCPowerMeasurementSense
        Get
            Return CType(Me.device0SenseComboBox.SelectedItem, DCPowerMeasurementSense)
        End Get
    End Property

    Friend ReadOnly Property ResourceName1() As String
        Get
            Return Me.device1ResourceNameComboBox.Text
        End Get
    End Property

    Friend ReadOnly Property ChannelName1() As String
        Get
            Return Me.device1ChannelNameTextBox.Text
        End Get
    End Property

    Friend ReadOnly Property FullyQualifiedChannelName1() As String
        Get
            Return $"{ResourceName1}/{ChannelName1}"
        End Get
    End Property

    Friend ReadOnly Property NumberOfPoints1() As Integer
        Get
            Return Decimal.ToInt32(Me.device1NumberOfPointsNumeric.Value)
        End Get
    End Property

    Friend ReadOnly Property CurrentLimit1() As Double
        Get
            Return Decimal.ToDouble(Me.device1CurrentLimitNumeric.Value)
        End Get
    End Property

    Friend ReadOnly Property VoltageLevelStart1() As Double
        Get
            Return Decimal.ToDouble(Me.device1VoltageLevelStartNumeric.Value)
        End Get
    End Property

    Friend ReadOnly Property VoltageLevelStop1() As Double
        Get
            Return Decimal.ToDouble(Me.device1VoltageLevelStopNumeric.Value)
        End Get
    End Property

    Friend ReadOnly Property SourceDelay1() As PrecisionTimeSpan
        Get
            Return New PrecisionTimeSpan(Decimal.ToDouble(Me.device1SourceDelayNumeric.Value))
        End Get
    End Property

    Friend ReadOnly Property Sense1() As DCPowerMeasurementSense
        Get
            Return CType(Me.device1SenseComboBox.SelectedItem, DCPowerMeasurementSense)
        End Get
    End Property

    Private ReadOnly Property Timeout() As PrecisionTimeSpan
        Get
            Return New PrecisionTimeSpan(Decimal.ToDouble(Me.timeoutNumeric.Value))
        End Get
    End Property

    Private ReadOnly Property SelectedPlotName() As String
        Get
            Return Me.plotNameComboBox.Text
        End Get
    End Property
#End Region

    Private Sub startButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles startButton.Click
        ChangeControlState(False)
        measurements.Clear()
        measurementsDataGridView.Rows.Clear()
        Start()
        ChangeControlState(True)
    End Sub

    Private Sub mainForm_Closing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles MyBase.FormClosing
        CloseSession()
    End Sub

    Private Sub plotNameComboBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles plotNameComboBox.SelectedIndexChanged
        DisplayPlot(SelectedPlotName)
    End Sub

    Private Sub Start()
        Try
            InitializeDCPowerSession()

            'FullyQualifiedChannelName0 controls the gate voltage
            'FullyQualifiedChannelName1 controls the drain voltage
            session.Source.Mode = DCPowerSourceMode.Sequence

            ' "" means all channels in the session
            session.Outputs("").Source.Output.[Function] = DCPowerSourceOutputFunction.DCVoltage

            session.Outputs(FullyQualifiedChannelName0).Measurement.Sense = Sense0
            session.Outputs(FullyQualifiedChannelName1).Measurement.Sense = Sense1

            session.Outputs("").Source.Voltage.VoltageLevelAutorange = DCPowerSourceVoltageLevelAutorange.[On]
            session.Outputs("").Source.Voltage.CurrentLimitAutorange = DCPowerSourceCurrentLimitAutorange.[On]

            session.Outputs(FullyQualifiedChannelName0).Source.SourceDelay = SourceDelay0
            session.Outputs(FullyQualifiedChannelName1).Source.SourceDelay = SourceDelay1

            session.Outputs(FullyQualifiedChannelName0).Source.Voltage.CurrentLimit = CurrentLimit0
            session.Outputs(FullyQualifiedChannelName1).Source.Voltage.CurrentLimit = CurrentLimit1

            Dim voltageLevelsSequence0 As Double() = CreateVoltageLevelsSequence(VoltageLevelStop0, VoltageLevelStart0, NumberOfPlots0)
            Dim voltageLevelsSequence1 As Double() = CreateVoltageLevelsSequence(VoltageLevelStop1, VoltageLevelStart1, NumberOfPoints1)

            session.Outputs(FullyQualifiedChannelName0).Source.SetSequence(voltageLevelsSequence0)
            session.Outputs(FullyQualifiedChannelName1).Source.SetSequence(voltageLevelsSequence1)

            ' The Source trigger for the gate device is the Sequence Iteration Complete event
            ' of the drain device. This way the gate device will start sourcing the next gate
            ' voltage when the drain completes an iteration of its sequence.
            Dim inputTerminal As String = BuildFullyQualifiedTerminalName(session, ResourceName1, ChannelName1, "SequenceIterationCompleteEvent")
            session.Outputs(FullyQualifiedChannelName0).Triggers.SourceTrigger.DigitalEdge.Configure(inputTerminal, DCPowerTriggerEdge.Rising)

            ' The Start trigger and the Sequence Advance trigger for the drain device come from the
            ' Measure Complete event of the gate device. This way the drain device starts each
            ' iteration of the sequence when the gate device finishes sourcing and measuring
            ' the gate voltage.
            Dim device1inputTerminal As String = BuildFullyQualifiedTerminalName(session, ResourceName0, ChannelName0, "MeasureCompleteEvent")
            session.Outputs(FullyQualifiedChannelName1).Triggers.StartTrigger.DigitalEdge.Configure(device1inputTerminal, DCPowerTriggerEdge.Rising)
            session.Outputs(FullyQualifiedChannelName1).Triggers.SequenceAdvanceTrigger.DigitalEdge.Configure(device1inputTerminal, DCPowerTriggerEdge.Rising)

            ' Configure the second device to loop over the same sequence as many times as there
            ' are gate voltages.
            session.Outputs(FullyQualifiedChannelName1).Source.SequenceLoopCount = NumberOfPlots0

            ' Commit the settings to the devices so that the driver establishes all the routing
            ' required for the configured triggers and events.
            session.Outputs(FullyQualifiedChannelName0).Control.Commit()
            session.Outputs(FullyQualifiedChannelName1).Control.Commit()

            ' Initiate both devices. Initiate the drain device first so that it waits for the
            ' Start trigger before the gate device sources its gate voltage.
            session.Outputs(FullyQualifiedChannelName1).Control.Initiate()
            session.Outputs(FullyQualifiedChannelName0).Control.Initiate()

            ' Wait for the sequence engine to be done on the drain device. When the drain device's
            ' sequence engine is done, the gate device engine must also be done due to the trigger
            ' setup (the gate device completes the last step in its sequence just before the
            ' drain device starts the last iteration of its sequence).
            session.Outputs(FullyQualifiedChannelName1).Events.SequenceEngineDoneEvent.WaitForEvent(Timeout)

            ' Fetch the array of measured gate voltages from the gate device.
            Dim fetchTimeout As New PrecisionTimeSpan(1.0)
            Dim device0Measurements As DCPowerFetchResult, device1Measurements As DCPowerFetchResult
            device0Measurements = session.Measurement.Fetch(FullyQualifiedChannelName0, fetchTimeout, NumberOfPlots0)

            ' For each gate voltage.
            For plotIndex As Integer = 0 To NumberOfPlots0 - 1
                ' Fetch the array of measured drain voltages and currents from the drain device for the iteration.
                device1Measurements = session.Measurement.Fetch(FullyQualifiedChannelName1, fetchTimeout, NumberOfPoints1)

                ' Store the results in dictionary.
                Dim plotName As String = [String].Format("Plot {0}: ch{1} v={2:0.00}", plotIndex, FullyQualifiedChannelName0, device0Measurements.VoltageMeasurements(plotIndex))
                measurements.Add(plotName, device1Measurements)
            Next

            UpdateMeasurements()

            session.Outputs("").Source.Output.Enabled = False
        Catch ex As Exception
            ShowError(ex)
        Finally
            CloseSession()
        End Try
    End Sub

    Private Shared Function CreateVoltageLevelsSequence(ByVal voltageLevelStop As Double, ByVal voltageLevelStart As Double, ByVal numberOfPoints As Integer) As Double()
        Dim stepSize As Double = 0.0
        If numberOfPoints > 1 Then
            ' To avoid dividing by 0.
            ' Calculate step size.
            stepSize = ((voltageLevelStop - voltageLevelStart) / (numberOfPoints - 1))
        End If
        Dim voltageLevelsSequence As Double() = New Double(numberOfPoints - 1) {}
        For pointIndex As Integer = 0 To numberOfPoints - 1
            ' Calculate the Voltage Level for this step.
            voltageLevelsSequence(pointIndex) = (stepSize * CDbl(pointIndex)) + voltageLevelStart
        Next
        Return voltageLevelsSequence
    End Function

    Private Sub InitializeDCPowerSession()

        Dim fullyQualifiedResourceNames As String = [String].Join(",", FullyQualifiedChannelName0, FullyQualifiedChannelName1)
        session = New NIDCPower(fullyQualifiedResourceNames, False, String.Empty)
        AddHandler session.DriverOperation.Warning, New EventHandler(Of DCPowerWarningEventArgs)(AddressOf DCPowerDriverOperationWarning)

    End Sub

    Private Sub DCPowerDriverOperationWarning(ByVal sender As Object, ByVal e As DCPowerWarningEventArgs)
        MessageBox.Show(e.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Sub

    Private Sub CloseSession()
        Try
            If session IsNot Nothing Then
                session.Close()
                session = Nothing
            End If

        Catch ex As Exception
            ShowError(ex)
            Application.[Exit]()
        End Try
    End Sub

    Private Sub UpdateMeasurements()
        plotNameComboBox.Items.Clear()
        For Each measurement As KeyValuePair(Of String, DCPowerFetchResult) In measurements
            plotNameComboBox.Items.Add(measurement.Key)
        Next
        If plotNameComboBox.Items.Count > 0 Then
            plotNameComboBox.SelectedIndex = 0
        End If
        DisplayPlot(SelectedPlotName)
    End Sub

    Private Sub DisplayPlot(plotName As String)
        measurementsDataGridView.Rows.Clear()

        Dim plot As DCPowerFetchResult
        If measurements.TryGetValue(plotName, plot) Then
            For i As Integer = 0 To plot.VoltageMeasurements.Length - 1
                measurementsDataGridView.Rows.Add((i + 1).ToString(), plot.VoltageMeasurements(i).ToString("E"), plot.CurrentMeasurements(i).ToString("E"))
            Next
        End If
    End Sub

    Private Shared Sub ShowError(ByVal ex As Exception)
        MessageBox.Show(ex.Message, ex.[GetType]().ToString(), MessageBoxButtons.OK, MessageBoxIcon.[Error])
    End Sub

    Private Shared Function BuildFullyQualifiedTerminalName(dcPowerSession As NIDCPower, resourceName As String, channelName As String, localTerminalName As String) As String
        Dim modelName As String = dcPowerSession.Instruments(resourceName).Identity.InstrumentModel
        If modelName.Equals("NI PXI-4132", StringComparison.OrdinalIgnoreCase) Then
            Return [String].Format("/{0}/{1}", resourceName, localTerminalName)
        Else
            Return [String].Format("/{0}/Engine{1}/{2}", resourceName, channelName, localTerminalName)
        End If
    End Function

    Private Sub ChangeControlState(isEnabled As Boolean)
        Me.device0GroupBox.Enabled = isEnabled
        Me.device1GroupBox.Enabled = isEnabled
        Me.timeoutGroupBox.Enabled = isEnabled
        Me.startButton.Enabled = isEnabled
        Me.device0ResourceNameComboBox.[Select]()
        Me.Refresh()
    End Sub

End Class