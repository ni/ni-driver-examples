'==================================================================================================
'
' Title:
'      Voltage Histogram
'
' Description:
'      This example illustrates using some of the advanced measurement library functions
'      to create a voltage histogram. Voltage histograms eliminate the voltage information from 
'      waveforms by sorting all points into bins based on their time from the trigger. 
'      The result is a histogram of counts versus voltage value that can be used to see 
'      statistical information about the amplitude of a signal. Consult the NI High-Speed
'      Digitizers Help for more information about voltage histograms.
'
'==================================================================================================

Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments.NIScope
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Partial Public Class MainForm
    Inherits Form
    Private scopeSession As NIScope
    Private [stop] As Boolean

    Public Sub New()
        InitializeComponent()
        ConfigureMeasurement2ComboBox()
        ConfigureMeasurement1ComboBox()
        LoadScopeDeviceNames()
        ChangeControlState(True)
    End Sub

#Region "MainForm initial configuration"
    Private Sub ConfigureMeasurement2ComboBox()
        measurement2ComboBox.Items.AddRange([Enum].GetNames(GetType(ScopeScalarMeasurementType)))
        measurement2ComboBox.SelectedIndex = 39
    End Sub

    Private Sub ConfigureMeasurement1ComboBox()
        measurement1ComboBox.Items.AddRange([Enum].GetNames(GetType(ScopeScalarMeasurementType)))
        measurement1ComboBox.SelectedIndex = 8
    End Sub

    Private Sub LoadScopeDeviceNames()
        Using scopeDevices As New ModularInstrumentsSystem("NI-Scope")
            For Each device As DeviceInfo In scopeDevices.DeviceCollection
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
            Return Me.channelTextBox.Text
        End Get
    End Property

    Private ReadOnly Property RecordLengthMin() As Integer
        Get
            Return Decimal.ToInt32(Me.recordLengthMinNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property SampleRateMin() As Double
        Get
            Return Decimal.ToDouble(Me.sampleRateMinNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property HighVoltageLimit() As Double
        Get
            Return Decimal.ToDouble(Me.highVoltageLimitNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property LowVoltageLimit() As Double
        Get
            Return Decimal.ToDouble(Me.lowVoltageLimitNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property HistogramSize() As Integer
        Get
            Return Decimal.ToInt32(Me.histogramSizeNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property Measurement1() As ScopeScalarMeasurementType
        Get
            Return DirectCast([Enum].Parse(GetType(ScopeScalarMeasurementType), measurement1ComboBox.Text), ScopeScalarMeasurementType)
        End Get
    End Property

    Private ReadOnly Property Measurement2() As ScopeScalarMeasurementType
        Get
            Return DirectCast([Enum].Parse(GetType(ScopeScalarMeasurementType), measurement2ComboBox.Text), ScopeScalarMeasurementType)
        End Get
    End Property

    Private ReadOnly Property ClearStats() As Boolean
        Get
            Return Me.clearStatsCheckBox.Checked
        End Get
    End Property

    Private WriteOnly Property ScalarResult2() As Double
        Set(value As Double)
            Me.scalarResult2TextBox.Text = value.ToString()
        End Set
    End Property

    Private WriteOnly Property ScalarResult1() As Double
        Set(value As Double)
            Me.scalarResult1TextBox.Text = value.ToString()
        End Set
    End Property

    Private WriteOnly Property Mean() As Double
        Set(value As Double)
            Me.meanTextBox.Text = value.ToString()
        End Set
    End Property
#End Region

    Private Sub acquireButton_Click(sender As Object, e As EventArgs) Handles acquireButton.Click
        StartAcquisition()
    End Sub

    Private Sub mainForm_Closing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        CloseSession()
    End Sub

    Private Sub stopButton_Click(sender As Object, e As EventArgs) Handles stopButton.Click
        StopAcquistion()
    End Sub

    Private Sub InitializeSession()
        scopeSession = New NIScope(ResourceName, False, False)
        AddHandler scopeSession.DriverOperation.Warning, New EventHandler(Of ScopeWarningEventArgs)(AddressOf DriverOperation_Warning)
    End Sub

    Private Sub DriverOperation_Warning(sender As Object, e As ScopeWarningEventArgs)
        messageTextBox.Clear()
        MessageBox.Show(e.Text, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Sub

    Private Sub StartAcquisition()
        [stop] = False
        ChangeControlState(False)
        DisplayMessage("Acquisition is in progress...")

        Dim waveformFromScope As AnalogWaveformCollection(Of Double) = Nothing
        Dim voltageHistogram As AnalogWaveformCollection(Of Double) = Nothing
        Dim ZeroTimeout As New PrecisionTimeSpan(0)
        Dim Timeout As New PrecisionTimeSpan(5)
        Try
            InitializeSession()
            While Not [stop]
                ' Configure the vertical parameters such as input range, offset, and coupling.
                scopeSession.Channels(ChannelName).Range = 5

                ' Configure the horizontal parameters such as sampling rate and number of samples to acquire.
                scopeSession.Timing.ConfigureTiming(SampleRateMin, RecordLengthMin, 50, 1, True)

                ' Configure an analog edge trigger.
                scopeSession.Trigger.EdgeTrigger.Configure(ScopeTriggerSource.Channel0, 0, ScopeTriggerSlope.Positive,
                                                           ScopeTriggerCoupling.DC, ZeroTimeout, ZeroTimeout)

                ' Configure the attributes used in the voltage histogram calculation.
                scopeSession.Channels(ChannelName).Measurement.VoltageHistogram.HighVolts = HighVoltageLimit
                scopeSession.Channels(ChannelName).Measurement.VoltageHistogram.LowVolts = LowVoltageLimit
                scopeSession.Channels(ChannelName).Measurement.VoltageHistogram.Size = HistogramSize

                ' Initiate a new acquisition with the configured settings, wait for this acquisition to complete,
                ' and fetch the data. Plot the data.
                waveformFromScope = scopeSession.Channels(ChannelName).Measurement.Read(Timeout, -1, waveformFromScope)
                PlotWaveforms(waveformFromScopeDataGridView, waveformFromScope)

                ' Calculate the array measurement, multi-acquisition voltage histogram, and plot the result.
                voltageHistogram = scopeSession.Channels(ChannelName).Measurement.FetchArrayMeasurement(
                    Timeout, ScopeArrayMeasurementType.MultipleAcquisitionVoltageHistogram, voltageHistogram)
                PlotWaveforms(voltageHistogramDataGridView, voltageHistogram)

                ' Perform the requested scalar measurement.
                ScalarResult2 = scopeSession.Channels(ChannelName).Measurement.FetchScalarMeasurement(Timeout, Measurement2)(0)
                Dim statistics As ScopeScalarMeasurementStatistics = scopeSession.Channels(ChannelName).Measurement.
                    FetchScalarMeasurementStatistics(Timeout, Measurement1)(0)
                ScalarResult1 = statistics.MeasurementResult
                Mean = statistics.Mean

                ' If the user requests, clear any cached statistics.
                If ClearStats Then
                    scopeSession.Channels(ChannelName).Measurement.ClearWaveformMeasurements()
                End If
                Application.DoEvents()
            End While
            DisplayMessage("Acquisition successful!!!")
        Catch ex As Exception
            ShowError(ex)
        Finally
            CloseSession()
            ChangeControlState(True)
        End Try
    End Sub

    Private Sub ClearWaveforms()
        waveformFromScopeDataGridView.Columns.Clear()
        voltageHistogramDataGridView.Columns.Clear()
    End Sub

    Private Shared Sub PlotWaveforms(dgv As DataGridView, waveforms As AnalogWaveformCollection(Of Double))
        Dim rowIndex As Integer, columnIndex As Integer
        Dim lastCount As Integer = dgv.RowCount

        SetupDataGridView(dgv, waveforms.Count)
        For rowIndex = lastCount To lastCount + (waveforms(0).SampleCount - 1)
            columnIndex = 0
            dgv.Rows.Add()
            dgv.Rows(rowIndex).Cells(columnIndex).Value = (rowIndex + 1).ToString()
            columnIndex += 1
            For Each waveform As AnalogWaveform(Of Double) In waveforms
                dgv.Rows(rowIndex).Cells(columnIndex).Value = waveform.Samples(rowIndex - lastCount).Value.ToString("E")
                columnIndex += 1
            Next
            If rowIndex Mod 100 = 0 Then
                Application.DoEvents()
            End If
        Next
    End Sub

    Private Shared Sub SetupDataGridView(dgv As DataGridView, numberOfWaveforms As Integer)
        If dgv.ColumnCount > 0 Then
            Return
        End If

        Dim indexColumn As New DataGridViewTextBoxColumn()
        indexColumn.Width = 45
        indexColumn.HeaderText = "Index"
        dgv.Columns.Add(indexColumn)

        For waveformIndex As Integer = 0 To numberOfWaveforms - 1
            Dim waveformColumn As New DataGridViewTextBoxColumn()
            waveformColumn.Width = 125
            waveformColumn.HeaderText = "Waveform " & waveformIndex
            dgv.Columns.Add(waveformColumn)
        Next
    End Sub

    Private Sub StopAcquistion()
        If Not [stop] Then
            DisplayMessage("Stop in progress...Fetched points are being plotted...")
            [stop] = True
        End If
    End Sub

    Private Sub DisplayMessage(message As String)
        messageTextBox.Text = message
        Me.Refresh()
    End Sub

    Private Sub ChangeControlState(isEnabled As Boolean)
        resourceNameComboBox.Enabled = isEnabled
        measurement1ComboBox.Enabled = isEnabled
        measurement2ComboBox.Enabled = isEnabled
        clearStatsCheckBox.Enabled = isEnabled
        histogramParametersGroupBox.Enabled = isEnabled
        timingParametersGroupBox.Enabled = isEnabled
        acquireButton.Enabled = isEnabled
        stopButton.Enabled = Not isEnabled
        If Not isEnabled Then
            ClearWaveforms()
        End If
        Me.Refresh()
    End Sub

    Private Sub CloseSession()
        If scopeSession IsNot Nothing Then
            Try
                scopeSession.Close()
                scopeSession = Nothing
            Catch ex As Exception
                ShowError(ex)
                Application.[Exit]()
            End Try
        End If
    End Sub

    Private Sub ShowError(ex As Exception)
        messageTextBox.Clear()
        MessageBox.Show(ex.Message, ex.[GetType]().ToString(), MessageBoxButtons.OK, MessageBoxIcon.[Error])
    End Sub
End Class
