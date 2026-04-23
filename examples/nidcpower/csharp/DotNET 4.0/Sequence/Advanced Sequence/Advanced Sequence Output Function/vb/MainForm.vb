'=============================================================================================================
'
' Title:
'       NI-DCPower Advanced Sequence Output Function
'
' Description:
'       This example demonstrates how to use the Advanced Sequence functions to
' change the output function in a sequence.  This example initializes a
' session, configures the Source Delay, creates an Advanced Sequence, creates
' a step to set the configured Voltage Level in DC Voltage mode, creates a step
' to set the configured Current Level in DC Current mode, initiates generation,
' waits for a specified delay, and then measures the voltage and current output.
' This example uses Advanced Sequence source mode.
'
'  Suggested Devices:
'      PXIe-4135, PXIe-4136, PXIe-4137, PXIe-4138, PXIe-4139, PXIe-4140, PXIe-4141, PXIe-4142, PXIe-4143, PXIe-4144, PXIe-4145, PXIe-4147, PXIe-4162, PXIe-4163
'
'============================================================================================================

Imports System.Windows.Forms
Imports NationalInstruments
Imports NationalInstruments.ModularInstruments.NIDCPower
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Public Partial Class MainForm
    Inherits Form
    Dim dcPowerSession As NIDCPower

    Public Sub New()
        InitializeComponent()
        LoadDCPowerDeviceNames()
    End Sub

#Region "MainForm initial configuration"

    Sub LoadDCPowerDeviceNames()
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

    ReadOnly Property ResourceName() As String
        Get
            Return Me.resourceNameComboBox.Text
        End Get
    End Property

    ReadOnly Property ChannelName() As String
        Get
            Return Me.channelNameTextBox.Text
        End Get
    End Property

    ReadOnly Property FullyQualifiedChannelName() As String
        Get
            Return $"{ResourceName}/{ChannelName}"
        End Get
    End Property

    ReadOnly Property NumberOfPoints() As Integer
        Get
            Return Convert.ToInt32(Me.stepsNumeric.Value)
        End Get
    End Property

    ReadOnly Property CurrentLevelStart() As Double
        Get
            Return Convert.ToDouble(Me.currentLevelStartNumeric.Value)
        End Get
    End Property

    ReadOnly Property VoltageLevelStart() As Double
        Get
            Return Convert.ToDouble(Me.voltageLevelStartNumeric.Value)
        End Get
    End Property

    ReadOnly Property CurrentLevelStop() As Double
        Get
            Return Convert.ToDouble(Me.currentLevelStopNumeric.Value)
        End Get
    End Property

    ReadOnly Property VoltageLevelStop() As Double
        Get
            Return Convert.ToDouble(Me.voltageLevelStopNumeric.Value)
        End Get
    End Property

    ReadOnly Property SourceDelay() As PrecisionTimeSpan
        Get
            Return New PrecisionTimeSpan(Convert.ToDouble(Me.delayNumeric.Value))
        End Get
    End Property

#End Region

    Sub startButton_Click(sender As Object, e As EventArgs)
        ChangeControlState(False)
        ClearMeasurementsDataGridView()
        Start()
        ChangeControlState(True)
    End Sub

    Sub Start()

        ' Specify the Advanced Sequence Attributes which can change per step.
        Dim advancedSequenceProperties As DCPowerAdvancedSequenceProperty() = {DCPowerAdvancedSequenceProperty.VoltageLevel, DCPowerAdvancedSequenceProperty.CurrentLevel, DCPowerAdvancedSequenceProperty.OutputFunction}

        Try
            InitializeDCPowerSession()

            ' Allocate enough memory for the sequence steps.
            Dim voltageLevelsSequence As Double() = New Double(NumberOfPoints - 1) {}
            Dim currentLevelsSequence As Double() = New Double(NumberOfPoints - 1) {}

            ' Configure the Source mode to Sequence.
            dcPowerSession.Source.Mode = DCPowerSourceMode.Sequence

            ' Set the Source Delay.  This is the amount of time the device should
            ' wait after each sourcing step in the sequence. The device
            ' automatically takes a measurement after this delay.
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.SourceDelay = SourceDelay

            ' Set the Voltage Level Autorange and Current Level Autorange to On to
            ' configure the Voltage Level and Current Level without having to
            ' explicitly set the range.
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.Voltage.VoltageLevelAutorange = DCPowerSourceVoltageLevelAutorange.[On]
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.Current.CurrentLevelAutorange = DCPowerSourceCurrentLevelAutorange.[On]

            ' Create the Advanced Sequence.
            Dim advancedSequenceName As String = "MySequence"
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.AdvancedSequencing.CreateAdvancedSequence(advancedSequenceName, advancedSequenceProperties, True)

            ' Create Voltage Setpoints.
            ' Calculate the step size.
            Dim stepSize As Double
            If NumberOfPoints = 1 Then
                ' To avoid dividing by 0.
                stepSize = 0
            Else
                stepSize = (VoltageLevelStop - VoltageLevelStart) / ((NumberOfPoints) - 1)
            End If

            ' Create the sequence.
            For pointIndex As Integer = 0 To NumberOfPoints - 1
                ' Calculate the Voltage Level for this step.
                voltageLevelsSequence(pointIndex) = (stepSize * CDbl(pointIndex)) + VoltageLevelStart
            Next

            ' Create Current Setpoints.
            ' Calculate the step size.
            If NumberOfPoints = 1 Then
                ' To avoid dividing by 0.
                stepSize = 0
            Else
                stepSize = (CurrentLevelStop - CurrentLevelStart) / ((NumberOfPoints) - 1)
            End If

            ' Create the sequence.
            For pointIndex As Integer = 0 To NumberOfPoints - 1
                ' Calculate the Current Level for this step.
                currentLevelsSequence(pointIndex) = (stepSize * CDbl(pointIndex)) + CurrentLevelStart
            Next

            ' Create new Advanced Sequence Step for every Current Level.
            For i As Integer = 0 To NumberOfPoints - 1
                dcPowerSession.Outputs(FullyQualifiedChannelName).Source.AdvancedSequencing.CreateAdvancedSequenceStep(True)

                ' Set the Output Function to DC Voltage.
                dcPowerSession.Outputs(FullyQualifiedChannelName).Source.Output.[Function] = DCPowerSourceOutputFunction.DCVoltage

                ' Configure the Voltage Level.
                dcPowerSession.Outputs(FullyQualifiedChannelName).Source.Voltage.VoltageLevel = voltageLevelsSequence(i)
            Next

            ' Create new Advanced Sequence Step for every Voltage Level.
            For i As Integer = 0 To NumberOfPoints - 1
                dcPowerSession.Outputs(FullyQualifiedChannelName).Source.AdvancedSequencing.CreateAdvancedSequenceStep(True)

                ' Set the Output Function to DC Current.
                dcPowerSession.Outputs(FullyQualifiedChannelName).Source.Output.[Function] = DCPowerSourceOutputFunction.DCCurrent

                ' Configure the Current Level.
                dcPowerSession.Outputs(FullyQualifiedChannelName).Source.Current.CurrentLevel = currentLevelsSequence(i)
            Next

            ' Initiate the device.
            dcPowerSession.Control.Initiate()

            ' Wait for the sequence engine to be done.
            Dim timeout As New PrecisionTimeSpan(10.0)
            dcPowerSession.Outputs(FullyQualifiedChannelName).Events.SequenceEngineDoneEvent.WaitForEvent(timeout)

            ' Fetch the arrays of measured voltages and currents from the device.
            Dim result As DCPowerFetchResult = dcPowerSession.Measurement.Fetch(FullyQualifiedChannelName, timeout, NumberOfPoints)
            DisplayMeasurements(result)

        Catch ex As Exception
            ShowError(ex)
        Finally
            CloseSession()
        End Try
    End Sub

    Sub DisplayMeasurements(result As DCPowerFetchResult)
        For i As Integer = 0 To result.VoltageMeasurements.Length - 1
            measurementsDataGridView.Rows.Add((i + 1).ToString(), result.VoltageMeasurements(i).ToString("E"), result.CurrentMeasurements(i).ToString("E"))
        Next
    End Sub

    Sub InitializeDCPowerSession()
        dcPowerSession = New NIDCPower(FullyQualifiedChannelName, False, String.Empty)
        AddHandler dcPowerSession.DriverOperation.Warning, New EventHandler(Of DCPowerWarningEventArgs)(AddressOf DCPowerDriverOperationWarning)
    End Sub

    Sub DCPowerDriverOperationWarning(sender As Object, e As DCPowerWarningEventArgs)
        MessageBox.Show(e.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Sub

    Sub mainForm_Closing(sender As Object, e As FormClosingEventArgs)
        CloseSession()
    End Sub

    Sub ClearMeasurementsDataGridView()
        measurementsDataGridView.Rows.Clear()
    End Sub

    Sub CloseSession()
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

    Shared Sub ShowError(ex As Exception)
        MessageBox.Show(ex.Message, ex.[GetType]().ToString(), MessageBoxButtons.OK, MessageBoxIcon.[Error])
    End Sub

    Sub ChangeControlState(isEnabled As Boolean)
        resourceNameAndChannelNameGroupBox.Enabled = isEnabled
        configurationGroupBox.Enabled = isEnabled
        startButton.Enabled = isEnabled
        resourceNameComboBox.[Select]()
        Me.Refresh()
    End Sub

End Class