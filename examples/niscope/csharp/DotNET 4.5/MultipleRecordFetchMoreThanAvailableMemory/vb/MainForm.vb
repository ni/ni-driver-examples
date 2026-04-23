'==================================================================================================
'
' Title:
'      Multiple Record Fetch More Than Available Memory
'
' Description:
'      This example demonstrates the multi-record and continuous acquisition capabilities of National 
'      Instruments digitizers.
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
        LoadScopeDeviceNames()
        ChangeControlState(True)
    End Sub

#Region "Mainform initial configuration"
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

    Private ReadOnly Property VerticalRange() As Double
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

    Private ReadOnly Property AllowMoreRecordsThanAvailableMem() As Boolean
        Get
            Return Me.allowMoreRecsThanAvaiMemCheckBox.Checked
        End Get
    End Property

    Private ReadOnly Property NumberOfRecords() As Integer
        Get
            Return Decimal.ToInt32(Me.numOfRecordNumeric.Value)
        End Get
    End Property

    Private WriteOnly Property NumberOfRecordsFetched() As String
        Set(ByVal value As String)
            Me.numRecordsFetchedTextBox.Text = value
        End Set
    End Property

    Private WriteOnly Property NumberOfRecordsAcquired() As String
        Set(ByVal value As String)
            Me.numRecordsAcquiredTextBox.Text = value
        End Set
    End Property
#End Region

    Private Sub acquireButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles acquireButton.Click
        MultiRecordFetch()
    End Sub

    Private Sub stopButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles stopButton.Click
        StopFetch()
    End Sub
    Private Sub mainForm_Closing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles MyBase.FormClosing
        CloseSession()
    End Sub

    Private Sub InitializeSession()
        scopeSession = New NIScope(ResourceName, False, False)
        AddHandler scopeSession.DriverOperation.Warning, New EventHandler(Of ScopeWarningEventArgs)(AddressOf DriverOperation_Warning)
    End Sub

    Private Sub DriverOperation_Warning(ByVal sender As Object, ByVal e As ScopeWarningEventArgs)
        messageTextBox.Clear()
        MessageBox.Show(e.Text, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Sub

    Private Sub MultiRecordFetch()
        [stop] = False
        ChangeControlState(False)
        DisplayMessage("Acquisition is in progress...")

        Dim waveforms As AnalogWaveformCollection(Of Double) = Nothing
        Try
            InitializeSession()
            scopeSession.Timing.MoreRecordsThanMemoryAllowed = AllowMoreRecordsThanAvailableMem

            Dim offset As Double = 0.0
            Dim probeAttenuation As Double = 1.0
            scopeSession.Channels(ChannelName).Configure(VerticalRange, offset, ScopeVerticalCoupling.DC, probeAttenuation, True)

            Dim referencePosition As Double = 50.0
            Dim enforceRealtime As Boolean = True
            scopeSession.Timing.ConfigureTiming(SampleRateMin, RecordLengthMin, referencePosition, NumberOfRecords, enforceRealtime)

            Dim triggerLevel As Double = 0.0
            Dim triggerSlope As ScopeTriggerSlope = ScopeTriggerSlope.Positive
            Dim triggerCoupling As ScopeTriggerCoupling = ScopeTriggerCoupling.DC
            Dim triggerSource As ScopeTriggerSource = ScopeTriggerSource.Channel0
            Dim triggerHoldoff As PrecisionTimeSpan = PrecisionTimeSpan.Zero
            Dim triggerDelay As PrecisionTimeSpan = PrecisionTimeSpan.Zero
            scopeSession.Trigger.EdgeTrigger.Configure(triggerSource, triggerLevel, triggerSlope, triggerCoupling, triggerHoldoff, triggerDelay)

            scopeSession.Measurement.Initiate()

            Dim actualRecordLength As Long = scopeSession.Acquisition.RecordLength
            Dim timeout As New PrecisionTimeSpan(5.0)
            Dim record As Integer = 0
            While record < NumberOfRecords AndAlso Not [stop]
                NumberOfRecordsAcquired = scopeSession.Measurement.RecordsDone.ToString()
                NumberOfRecordsFetched = (record + 1).ToString()
                scopeSession.Acquisition.NumberOfRecordsToFetch = 1
                scopeSession.Acquisition.RecordNumberToFetch = record
                waveforms = scopeSession.Channels(ChannelName).Measurement.FetchDouble(timeout, actualRecordLength, waveforms)
                PlotWaveforms(sampledDataGridView, waveforms)
                record += 1
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
            If rowIndex Mod 1000 = 0 Then
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

    Private Sub StopFetch()
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
