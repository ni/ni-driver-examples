'==================================================================================================
'
' Title:
'      Advanced Measurement Library

' Description:
'      This example illustrates using some of the advanced measurement library functions 
'      such as waveform processing.  The example calls Auto Setup once to configure the 
'      vertical and horizontal subsystems of the digitizer based on the input to all 
'      the channels.
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
        ConfigureArrayMeasurementComboBox()
        ConfigureProcessingStepComboBox()
        ConfigureFilterComboBox()
        LoadScopeDeviceNames()
        ChangeControlState(True)
    End Sub

#Region "Mainform initial configuration"
    Private Sub ConfigureArrayMeasurementComboBox()
        For Each enumValue As ScopeArrayMeasurementType In [Enum].GetValues(GetType(ScopeArrayMeasurementType))
            arrayMeasurementComboBox.Items.Add(enumValue)
        Next
        arrayMeasurementComboBox.SelectedItem = arrayMeasurementComboBox.Items(19)
    End Sub

    Private Sub ConfigureProcessingStepComboBox()
        For Each enumValue As ScopeArrayMeasurementType In [Enum].GetValues(GetType(ScopeArrayMeasurementType))
            processingStepComboBox.Items.Add(enumValue)
        Next
        processingStepComboBox.SelectedItem = processingStepComboBox.Items(18)
    End Sub

    Private Sub ConfigureFilterComboBox()
        For Each enumValue As ScopeMeasurementFilterType In [Enum].GetValues(GetType(ScopeMeasurementFilterType))
            filterComboBox.Items.Add(enumValue)
        Next
        filterComboBox.SelectedItem = filterComboBox.Items(0)
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

    Private ReadOnly Property ProcessStepMeasurement() As ScopeArrayMeasurementType
        Get
            Return CType(Me.processingStepComboBox.SelectedItem, ScopeArrayMeasurementType)
        End Get
    End Property

    Private ReadOnly Property ArrayMeasurement() As ScopeArrayMeasurementType
        Get
            Return CType(Me.arrayMeasurementComboBox.SelectedItem, ScopeArrayMeasurementType)
        End Get
    End Property

    Private ReadOnly Property FilterType() As ScopeMeasurementFilterType
        Get
            Return CType(Me.filterComboBox.SelectedItem, ScopeMeasurementFilterType)
        End Get
    End Property

    Private ReadOnly Property CutoffFrequency() As Double
        Get
            Return Decimal.ToDouble(Me.cutoffFrequencyNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property CenterFrequency() As Double
        Get
            Return Decimal.ToDouble(Me.centerFrequencyNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property BandPassWidth() As Double
        Get
            Return Decimal.ToDouble(Me.bandpassWidthNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property Timeout() As PrecisionTimeSpan
        Get
            Return PrecisionTimeSpan.FromSeconds(Decimal.ToDouble(Me.timeoutNumeric.Value))
        End Get
    End Property
#End Region

    Private Sub acquireButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles acquireButton.Click
        StartAcquisition()
    End Sub

    Private Sub mainForm_Closing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles MyBase.FormClosing
        CloseSession()
    End Sub

    Private Sub stopButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles stopButton.Click
        StopAcquistion()
    End Sub

    Private Sub InitializeSession()
        scopeSession = New NIScope(ResourceName, False, False)
        AddHandler scopeSession.DriverOperation.Warning, New EventHandler(Of ScopeWarningEventArgs)(AddressOf DriverOperation_Warning)
    End Sub

    Private Sub DriverOperation_Warning(ByVal sender As Object, ByVal e As ScopeWarningEventArgs)
        messageTextBox.Clear()
        MessageBox.Show(e.Text, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Sub

    Private Sub StartAcquisition()
        [stop] = False
        ChangeControlState(False)
        DisplayMessage("Acquisition is in progress...")

        Dim sampledwaveforms As AnalogWaveformCollection(Of Double) = Nothing
        Dim measurementWaveforms As AnalogWaveformCollection(Of Double) = Nothing
        Try
            InitializeSession()
            scopeSession.Measurement.AutoSetup()
            While Not [stop]
                scopeSession.Channels(ChannelName).Measurement.Filter.Type = FilterType
                scopeSession.Channels(ChannelName).Measurement.Filter.CutoffFrequency = CutoffFrequency
                scopeSession.Channels(ChannelName).Measurement.Filter.CenterFrequency = CenterFrequency
                scopeSession.Channels(ChannelName).Measurement.Filter.Width = BandPassWidth

                scopeSession.Channels(ChannelName).Measurement.AddWaveformProcessing(ProcessStepMeasurement)
                Dim recordLength As Long = scopeSession.Acquisition.RecordLength
                sampledwaveforms = scopeSession.Channels(ChannelName).Measurement.Read(Timeout, recordLength, sampledwaveforms)
                measurementWaveforms = scopeSession.Channels(ChannelName).Measurement.FetchArrayMeasurement(Timeout, ArrayMeasurement, measurementWaveforms)
                scopeSession.Channels(ChannelName).Measurement.ClearWaveformProcessing()

                PlotWaveforms(sampledDataGridView, sampledwaveforms)
                PlotWaveforms(measurementDataGridView, measurementWaveforms)
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
        sampledDataGridView.Columns.Clear()
        measurementDataGridView.Columns.Clear()
    End Sub

    Private Shared Sub PlotWaveforms(ByVal dgv As DataGridView, ByVal waveforms As AnalogWaveformCollection(Of Double))
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

    Private Shared Sub SetupDataGridView(ByVal dgv As DataGridView, ByVal numberOfWaveforms As Integer)
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

    Private Sub DisplayMessage(ByVal message As String)
        messageTextBox.Text = message
        Me.Refresh()
    End Sub

    Private Sub ChangeControlState(ByVal isEnabled As Boolean)
        generalGroupBox.Enabled = isEnabled
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

    Private Sub ShowError(ByVal ex As Exception)
        messageTextBox.Clear()
        MessageBox.Show(ex.Message, ex.[GetType]().ToString(), MessageBoxButtons.OK, MessageBoxIcon.[Error])
    End Sub
End Class
