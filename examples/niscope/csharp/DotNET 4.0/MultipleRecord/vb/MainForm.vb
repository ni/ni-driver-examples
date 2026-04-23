'==================================================================================================
'
' Title:
'      Multi Record
'
' Description  : This example demonstrates the multi-record capabilities of National Instruments 
'                high-speed digitizers.  In a multi-record acquisition, each record is one 
'                waveform with at least "min record length" points as specified with the 
'                Configure Horizontal Timing function. 
'
'==================================================================================================

Imports System.Collections
Imports System.Collections.Generic
Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments.NIScope
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Partial Public Class MainForm
    Inherits Form
    Private scopeSession As NIScope

    Public Sub New()
        InitializeComponent()
        ConfigurePlotRelativeToComboBox()
        LoadScopeDeviceNames()
        ChangeControlState(True)
    End Sub

#Region "Mainform initial configuration"
    Private Sub ConfigurePlotRelativeToComboBox()
        Dim plotRelativeToList As New List(Of DictionaryEntry)()
        plotRelativeToList.Add(New DictionaryEntry("Trigger", "0"))
        plotRelativeToList.Add(New DictionaryEntry("Absolute", "1"))
        plotRelativeToComboBox.DataSource = plotRelativeToList
        plotRelativeToComboBox.ValueMember = "Value"
        plotRelativeToComboBox.DisplayMember = "Key"
        plotRelativeToComboBox.SelectedIndex = 0
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
            Return Me.channelNameTextBox.Text
        End Get
    End Property

    Private ReadOnly Property NumberOfRecords() As Integer
        Get
            Return Decimal.ToInt32(Me.numOfRecordsNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property Range() As Double
        Get
            Return Decimal.ToDouble(Me.verticalRangeNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property SampleRateMin() As Double
        Get
            Return Decimal.ToDouble(Me.minSampleRateNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property RecordLengthMin() As Integer
        Get
            Return Decimal.ToInt32(Me.minRecordLengthNumeric.Value)
        End Get
    End Property
#End Region

    Private Sub acquireButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles acquireButton.Click, MyBase.Click
        StartMultiRecordAcquisition()
    End Sub

    Private Sub mainForm_Closing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles MyBase.FormClosing
        CloseSession()
    End Sub

    Private Sub DriverOperation_Warning(ByVal sender As Object, ByVal e As ScopeWarningEventArgs)
        MessageBox.Show(e.Text, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Sub

    Private Sub InitializeSession()
        scopeSession = New NIScope(ResourceName, False, False)
        AddHandler scopeSession.DriverOperation.Warning, New EventHandler(Of ScopeWarningEventArgs)(AddressOf DriverOperation_Warning)
    End Sub

    Private Sub StartMultiRecordAcquisition()
        ChangeControlState(False)

        Dim waveforms As AnalogWaveformCollection(Of Double) = Nothing
        Dim waveformInfo As ScopeWaveformInfo() = Nothing
        Try
            InitializeSession()

            Dim offset As Double = 0.0
            Dim coupling As ScopeVerticalCoupling = ScopeVerticalCoupling.DC
            Dim probeAttenuation As Double = 1.0
            scopeSession.Channels(ChannelName).Configure(Range, offset, coupling, probeAttenuation, True)

            Dim referencePosition As Double = 50.0
            Dim enforceRealtime As Boolean = True
            scopeSession.Timing.ConfigureTiming(SampleRateMin, RecordLengthMin, referencePosition, NumberOfRecords, enforceRealtime)

            Dim triggerLevel As Double = 0.0
            Dim triggerSlope As ScopeTriggerSlope = ScopeTriggerSlope.Positive
            Dim triggerCoupling As ScopeTriggerCoupling = ScopeTriggerCoupling.DC
            Dim triggerHoldoff As PrecisionTimeSpan = PrecisionTimeSpan.Zero
            Dim triggerDelay As PrecisionTimeSpan = PrecisionTimeSpan.Zero
            Dim triggerSource As ScopeTriggerSource = ScopeTriggerSource.Channel0

            scopeSession.Trigger.EdgeTrigger.Configure(triggerSource, triggerLevel, triggerSlope, triggerCoupling, triggerHoldoff, triggerDelay)

            scopeSession.Measurement.Initiate()

            Dim recordLength As Long = scopeSession.Acquisition.RecordLength

            Dim timeout As New PrecisionTimeSpan(5.0)
            waveforms = scopeSession.Channels(ChannelName).Measurement.FetchDouble(timeout, recordLength, waveforms, waveformInfo)
            PlotWaveforms(sampledDataGridView, waveforms)
        Catch ex As System.Exception
            ShowError(ex)
        Finally
            CloseSession()
            ChangeControlState(True)
        End Try
    End Sub

    Private Sub ClearWaveforms()
        sampledDataGridView.Columns.Clear()
    End Sub

    Private Shared Sub PlotWaveforms(ByVal dgv As DataGridView, ByVal waveforms As AnalogWaveformCollection(Of Double))
        Dim rowIndex As Integer, columnIndex As Integer

        SetupDataGridView(dgv, waveforms.Count)
        For rowIndex = 0 To waveforms(0).SampleCount - 1
            columnIndex = 0
            dgv.Rows.Add()
            dgv.Rows(rowIndex).Cells(columnIndex).Value = (rowIndex + 1).ToString()
            columnIndex += 1
            For Each waveform As AnalogWaveform(Of Double) In waveforms
                dgv.Rows(rowIndex).Cells(columnIndex).Value = waveform.Samples(rowIndex).Value.ToString("E")
                columnIndex += 1
            Next
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

    Private Sub ChangeControlState(ByVal isEnabled As Boolean)
        generalGroupBox.Enabled = isEnabled
        verticalAndHorizontalGroupBox.Enabled = isEnabled
        acquireButton.Enabled = isEnabled
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

    Private Shared Sub ShowError(ByVal ex As Exception)
        MessageBox.Show(ex.Message, ex.[GetType]().ToString(), MessageBoxButtons.OK, MessageBoxIcon.[Error])
    End Sub
End Class

