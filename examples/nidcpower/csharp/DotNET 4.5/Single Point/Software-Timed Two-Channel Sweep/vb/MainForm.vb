'=============================================================================================================
'
' Title:
'      NI-DCPower Software-Timed Two-Channel Voltage Sweep
'
' Description:
'      This example demonstrates how to set up a software-timed two-channel nested voltage
'      sweep and display the results in a graph (IV Curve).  Use this example to produce
'      the characteristic curves of a FET transistor.  It can be easily adapted to test
'      a BJT by performing a current sweep instead of a voltage sweep. This example
'      performs a software-timed sweep using Single Point source mode. Do not use this
'      example if your device supports Sequence source mode; use the hardware-timed
'      example instead.
'
'      Note: In this example the Output Function is set to DC Voltage. If you change the
'      Output Function to DC Current, you must use Current Level and Voltage Limit instead
'      of Voltage Level and Current Limit.
'
'  Suggested Devices:
'      PXI-4110, PXI-4130
'      PXIe-4112, PXIe-4113, PXIe-4140, PXIe-4141, PXIe-4142, PXIe-4143, PXIe-4144, PXIe-4145, PXIe-4147, PXIe-4154, PXIe-4162, PXIe-4163
'
'=============================================================================================================

Imports System.Collections.Generic
Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments.NIDCPower
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Partial Public Class MainForm
    Inherits Form
    Private session As NIDCPower
    Private measurements As New Dictionary(Of String, Double(,))()

    Public Sub New()
        InitializeComponent()
        ConfigureSenseComboBox()
        LoadDCPowerDeviceNames()
    End Sub

#Region "MainForm initial configuration"
    Private Sub ConfigureSenseComboBox()
        For Each item As DCPowerMeasurementSense In [Enum].GetValues(GetType(DCPowerMeasurementSense))
            channel1SenseComboBox.Items.Add(item)
            channel2SenseComboBox.Items.Add(item)
        Next
        channel1SenseComboBox.SelectedIndex = 0
        channel2SenseComboBox.SelectedIndex = 0
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

    Private ReadOnly Property Channel1Name() As String
        Get
            Return Me.channel1NameTextBox.Text
        End Get
    End Property

    Private ReadOnly Property FullyQualifiedChannel1Name() As String
        Get
            Return $"{ResourceName}/{Channel1Name}"
        End Get
    End Property

    Private ReadOnly Property Channel1NumberOfPlots() As Integer
        Get
            Return Decimal.ToInt32(Me.channel1NumberOfPlotsNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property Channel1VoltageLevelStart() As Double
        Get
            Return Decimal.ToDouble(Me.channel1VoltageLevelStartNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property Channel1VoltageLevelStop() As Double
        Get
            Return Decimal.ToDouble(Me.channel1VoltageLevelStopNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property Channel1CurrentLimit() As Double
        Get
            Return Decimal.ToDouble(Me.channel1CurrentLimitNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property Channel1Sense() As DCPowerMeasurementSense
        Get
            Return CType(Me.channel1SenseComboBox.SelectedItem, DCPowerMeasurementSense)
        End Get
    End Property

    Private ReadOnly Property Channel1SourceDelay() As PrecisionTimeSpan
        Get
            Return New PrecisionTimeSpan(Decimal.ToDouble(Me.channel1SourceDelayNumeric.Value))
        End Get
    End Property

    Private ReadOnly Property Channel2Name() As String
        Get
            Return Me.channel2NameTextBox.Text
        End Get
    End Property

    Private ReadOnly Property FullyQualifiedChannel2Name() As String
        Get
            Return $"{ResourceName}/{Channel2Name}"
        End Get
    End Property

    Private ReadOnly Property Channel2NumberOfPoints() As Integer
        Get
            Return Decimal.ToInt32(Me.channel2NumberOfPointsNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property Channel2VoltageLevelStart() As Double
        Get
            Return Decimal.ToDouble(Me.channel2VoltageLevelStartNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property Channel2VoltageLevelStop() As Double
        Get
            Return Decimal.ToDouble(Me.channel2VoltageLevelStopNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property Channel2CurrentLimit() As Double
        Get
            Return Decimal.ToDouble(Me.channel2CurrentLimitNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property Channel2Sense() As DCPowerMeasurementSense
        Get
            Return CType(Me.channel2SenseComboBox.SelectedItem, DCPowerMeasurementSense)
        End Get
    End Property

    Private ReadOnly Property Channel2SourceDelay() As PrecisionTimeSpan
        Get
            Return New PrecisionTimeSpan(Decimal.ToDouble(Me.channel2SourceDelayNumeric.Value))
        End Get
    End Property

    Private ReadOnly Property SelectedPlotName() As String
        Get
            Return Me.selectedPlotNameComboBox.Text
        End Get
    End Property
#End Region

    Private Sub Start()
        Try
            InitializeDCPowerSession()

            session.Source.Mode = DCPowerSourceMode.SinglePoint

            ' "" means all channels in the session
            session.Outputs("").Source.Output.[Function] = DCPowerSourceOutputFunction.DCVoltage

            session.Outputs(FullyQualifiedChannel1Name).Measurement.Sense = Channel1Sense
            session.Outputs(FullyQualifiedChannel2Name).Measurement.Sense = Channel2Sense

            session.Outputs("").Source.Voltage.VoltageLevelAutorange = DCPowerSourceVoltageLevelAutorange.[On]
            session.Outputs("").Source.Voltage.CurrentLimitAutorange = DCPowerSourceCurrentLimitAutorange.[On]

            session.Outputs(FullyQualifiedChannel1Name).Source.Voltage.CurrentLimit = Channel1CurrentLimit
            session.Outputs(FullyQualifiedChannel2Name).Source.Voltage.CurrentLimit = Channel2CurrentLimit

            session.Control.Initiate()

            Dim channel1StepSize As Double = CalculateStepSize(Channel1VoltageLevelStart, Channel1VoltageLevelStop, Channel1NumberOfPlots)
            Dim channel2StepSize As Double = CalculateStepSize(Channel2VoltageLevelStart, Channel2VoltageLevelStop, Channel2NumberOfPoints)

            ' Nested Sweep with 2 channels.
            For plotIndex As Integer = 0 To Channel1NumberOfPlots - 1
                ' Calculate the Voltage Level for this step.
                Dim channel1VoltageLevel As Double = (channel1StepSize * plotIndex) + Channel1VoltageLevelStart
                session.Outputs(FullyQualifiedChannel1Name).Source.Voltage.VoltageLevel = channel1VoltageLevel

                session.Outputs(FullyQualifiedChannel1Name).Events.SourceCompleteEvent.WaitForEvent(New PrecisionTimeSpan(5.0))

                ' Construct the plot name.
                Dim measureResult As DCPowerMeasureResult = session.Measurement.Measure(FullyQualifiedChannel1Name)

                Dim plotName As String = [String].Format("Plot {0}: ch{1} v={2:0.00}", plotIndex, FullyQualifiedChannel1Name, measureResult.VoltageMeasurements(0))

                Dim plot As Double(,) = New Double(Channel2NumberOfPoints - 1, 1) {}
                For pointIndex As Integer = 0 To Channel2NumberOfPoints - 1
                    ' Calculate the Voltage Level for this step.
                    Dim channel2VoltageLevel As Double = (channel2StepSize * pointIndex) + Channel2VoltageLevelStart
                    session.Outputs(FullyQualifiedChannel2Name).Source.Voltage.VoltageLevel = channel2VoltageLevel

                    session.Outputs(FullyQualifiedChannel2Name).Events.SourceCompleteEvent.WaitForEvent(New PrecisionTimeSpan(5.0))

                    ' Measure the Current and Voltage.
                    Dim result As DCPowerMeasureResult = session.Measurement.Measure(FullyQualifiedChannel2Name)
                    plot(pointIndex, 0) = result.VoltageMeasurements(0)
                    plot(pointIndex, 1) = result.CurrentMeasurements(0)
                Next

                ' Store the plotName and corresponding measurements in dictionary.
                measurements.Add(plotName, plot)
            Next

            UpdateMeasurements()

            session.Utility.Reset()

        Catch ex As Exception
            ShowError(ex)
        Finally
            CloseSession()
        End Try
    End Sub

    Private Sub startButton_Click(sender As Object, e As EventArgs) Handles startButton.Click
        ChangeControlState(False)
        measurements.Clear()
        selectedPlotNameComboBox.Items.Clear()
        Start()
        ChangeControlState(True)
    End Sub

    Private Sub mainForm_Closing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        CloseSession()
    End Sub

    Private Sub plotNameComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles selectedPlotNameComboBox.SelectedIndexChanged
        DisplayPlot(SelectedPlotName)
    End Sub

    Private Sub InitializeDCPowerSession()
        Dim fullyQualifiedResourceNames As String = [String].Join(",", FullyQualifiedChannel1Name, FullyQualifiedChannel2Name)
        session = New NIDCPower(fullyQualifiedResourceNames, False, String.Empty)
        AddHandler session.DriverOperation.Warning, New EventHandler(Of DCPowerWarningEventArgs)(AddressOf DCPowerDriverOperationWarning)
    End Sub

    Private Sub DCPowerDriverOperationWarning(ByVal sender As Object, ByVal e As DCPowerWarningEventArgs)
        MessageBox.Show(e.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Sub

    Private Shared Function CalculateStepSize(ByVal voltageLevelStart As Double, ByVal voltageLevelStop As Double, ByVal numberOfPoints As Integer) As Double
        Dim stepSize As Double = 0.0
        If numberOfPoints > 1 Then ' To avoid dividing by 0.
            stepSize = ((voltageLevelStop - voltageLevelStart) / (numberOfPoints - 1))
        End If
        Return stepSize
    End Function

    Private Sub UpdateMeasurements()
        For Each m As KeyValuePair(Of String, Double(,)) In measurements
            selectedPlotNameComboBox.Items.Add(m.Key)
        Next
        If selectedPlotNameComboBox.Items.Count > 0 Then
            selectedPlotNameComboBox.SelectedIndex = 0
        End If
        DisplayPlot(SelectedPlotName)
    End Sub

    Private Sub DisplayPlot(plotName As String)
        measurementsDataGridView.Rows.Clear()

        Dim plot As Double(,) = New Double(Channel2NumberOfPoints - 1, 1) {}
        If measurements.TryGetValue(plotName, plot) Then
            For i As Integer = 0 To plot.GetLength(0) - 1
                measurementsDataGridView.Rows.Add((i + 1).ToString(), plot(i, 0).ToString("E"), plot(i, 1).ToString("E"))
            Next
        End If
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

    Private Shared Sub ShowError(ByVal ex As Exception)
        MessageBox.Show(ex.Message, ex.[GetType]().ToString(), MessageBoxButtons.OK, MessageBoxIcon.[Error])
    End Sub

    Private Sub ChangeControlState(flag As Boolean)
        channel1GroupBox.Enabled = flag
        channel2GroupBox.Enabled = flag
        resourceNameGroupBox.Enabled = flag
        startButton.Enabled = flag
        resourceNameComboBox.[Select]()
        Me.Refresh()
    End Sub

End Class