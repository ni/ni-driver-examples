'=============================================================================================================
'
' Title:
'        Binary Acquisition
'
' Description:
'       The application demonstrates how to obtain data from the a NI-SCOPE device in binary format.
'       You can specify the binary data size to be 8, 16, or 32. The program then displays the
'       fetched binary data before scaling and the actual data after scaling.
'
'=================================================================================================

Imports System.Text
Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments.NIScope
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Partial Public Class MainForm
    Inherits Form
    Private scopeSession As NIScope
    Private [stop] As Boolean

    Public Sub New()
        InitializeComponent()
        ConfigureTriggerTypeComboBox()
        ConfigureBinaryDataSizeComboBox()
        LoadScopeDeviceNames()
        ChangeControlState(True)
    End Sub

#Region "Mainform Initial Configuration"
    Private Sub ConfigureTriggerTypeComboBox()
        triggerTypeComboBox.Items.Add(ScopeTriggerType.Edge)
        triggerTypeComboBox.Items.Add(ScopeTriggerType.Immediate)
        triggerTypeComboBox.SelectedIndex = 0
    End Sub

    Private Sub ConfigureBinaryDataSizeComboBox()
        binaryDataSizeComboBox.Items.Add(8)
        binaryDataSizeComboBox.Items.Add(16)
        binaryDataSizeComboBox.Items.Add(32)
        binaryDataSizeComboBox.SelectedIndex = 0
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

    Private ReadOnly Property TriggerType() As ScopeTriggerType
        Get
            Return CType(Me.triggerTypeComboBox.SelectedItem, ScopeTriggerType)
        End Get
    End Property

    Private ReadOnly Property VerticalRange() As Double
        Get
            Return Decimal.ToDouble(Me.verticalRangeNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property VerticalOffset() As Double
        Get
            Return Decimal.ToDouble(Me.verticalOffsetNumeric.Value)
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

    Private ReadOnly Property BinaryDataSize() As Integer
        Get
            Return CInt(Me.binaryDataSizeComboBox.SelectedItem)
        End Get
    End Property

    Private WriteOnly Property Offset() As String
        Set(ByVal value As String)
            Me.scaleOffsetTextBox.Text = value
        End Set
    End Property

    Private WriteOnly Property GainFactor() As String
        Set(ByVal value As String)
            Me.scaleGainFactorTextBox.Text = value
        End Set
    End Property
#End Region

    Private Sub acquireButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles acquireButton.Click
        StartAcquisition()
    End Sub

    Private Sub MainForm_Closing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles MyBase.FormClosing
        CloseSession()
    End Sub

    Private Sub stopButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles stopButton.Click
        StopAcquisition()
    End Sub

    Private Sub StartAcquisition()
        [stop] = False
        ChangeControlState(False)
        DisplayMessage("Acquisition is in progress...")

        Dim byteWaveforms As AnalogWaveformCollection(Of Byte) = Nothing
        Dim shortWaveforms As AnalogWaveformCollection(Of Short) = Nothing
        Dim intWaveforms As AnalogWaveformCollection(Of Integer) = Nothing
        Dim waveformInfo As ScopeWaveformInfo() = Nothing
        Try
            InitializeSession()

            ' Configure the vertical parameters.
            Dim coupling As ScopeVerticalCoupling = ScopeVerticalCoupling.DC
            Dim probeAttenuation As Double = 1.0
            scopeSession.Channels(ChannelName).Configure(VerticalRange, VerticalOffset, coupling, probeAttenuation, True)

            ' Configure the horizontal parameters.
            Dim referencePosition As Double = 50.0
            Dim numberOfRecords As Integer = 1
            Dim enforceRealtime As Boolean = True
            scopeSession.Timing.ConfigureTiming(SampleRateMin, RecordLengthMin, referencePosition, numberOfRecords, enforceRealtime)

            ' Configure the trigger.
            If TriggerType = ScopeTriggerType.Edge Then
                Dim source As ScopeTriggerSource = ScopeTriggerSource.Channel0
                Dim level As Double = 0.0
                Dim slope As ScopeTriggerSlope = ScopeTriggerSlope.Positive
                Dim triggerCoupling As ScopeTriggerCoupling = ScopeTriggerCoupling.DC
                Dim holdOff As PrecisionTimeSpan = PrecisionTimeSpan.Zero
                Dim delay As PrecisionTimeSpan = PrecisionTimeSpan.Zero
                scopeSession.Trigger.EdgeTrigger.Configure(source, level, slope, triggerCoupling, holdOff, delay)
            Else
                scopeSession.Trigger.ConfigureTriggerImmediate()
            End If

            Dim timeout As New PrecisionTimeSpan(5.0)
            Dim recordLength As Long = scopeSession.Acquisition.RecordLength
            While Not [stop]
                scopeSession.Measurement.Initiate()
                Select Case BinaryDataSize
                    Case 8
                        byteWaveforms = scopeSession.Channels(ChannelName).Measurement.FetchByte(timeout, recordLength, byteWaveforms, waveformInfo)
                        PlotWaveforms(byteWaveforms)
                        Exit Select
                    Case 16
                        shortWaveforms = scopeSession.Channels(ChannelName).Measurement.FetchInt16(timeout, recordLength, shortWaveforms, waveformInfo)
                        PlotWaveforms(shortWaveforms)
                        Exit Select
                    Case 32
                        intWaveforms = scopeSession.Channels(ChannelName).Measurement.FetchInt32(timeout, recordLength, intWaveforms, waveformInfo)
                        PlotWaveforms(intWaveforms)
                        Exit Select
                End Select
                UpdateResults(waveformInfo)
            End While
            DisplayMessage("Acquisition successful!!!")
        Catch ex As Exception
            ShowError(ex)
        Finally
            CloseSession()
            ChangeControlState(True)
        End Try
    End Sub

    Private Sub UpdateResults(ByVal waveformInfo As ScopeWaveformInfo())
        Dim offsetTextBuilder As New StringBuilder()
        Dim gainFactorTextBuilder As New StringBuilder()
        For ii As Integer = 0 To waveformInfo.Length - 1
            Dim prefixTextBuilder As New StringBuilder()
            If ii > 0 Then
                prefixTextBuilder.Append("; ")
            End If
            prefixTextBuilder.Append("Waveform " & ii & ": ")
            offsetTextBuilder.Append(Convert.ToString(prefixTextBuilder) & waveformInfo(ii).Offset.ToString("E"))
            gainFactorTextBuilder.Append(Convert.ToString(prefixTextBuilder) & waveformInfo(ii).Gain.ToString("E"))
        Next
        Offset = offsetTextBuilder.ToString()
        GainFactor = gainFactorTextBuilder.ToString()
    End Sub

    Private Sub InitializeSession()
        scopeSession = New NIScope(ResourceName, False, False)
        AddHandler scopeSession.DriverOperation.Warning, New EventHandler(Of ScopeWarningEventArgs)(AddressOf DriverOperation_Warning)
    End Sub

    Private Sub DriverOperation_Warning(ByVal sender As Object, ByVal e As ScopeWarningEventArgs)
        messageTextBox.Clear()
        MessageBox.Show(e.Text, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Sub

    Private Sub StopAcquisition()
        If Not [stop] Then
            DisplayMessage("Stop in progress...Fetched points are being plotted...")
            [stop] = True
        End If
    End Sub

    Private Sub ClearWaveforms()
        binaryDataGridView.Columns.Clear()
        scaledDataGridView.Columns.Clear()
    End Sub

    Private Sub PlotWaveforms(Of T)(ByVal waveforms As AnalogWaveformCollection(Of T))
        Dim rowIndex As Integer, columnIndex As Integer
        Dim lastCount As Integer = binaryDataGridView.RowCount

        SetupDataGridView(binaryDataGridView, waveforms.Count)
        SetupDataGridView(scaledDataGridView, waveforms.Count)

        Dim scaledRecords As Double()() = New Double(waveforms.Count - 1)() {}
        For i As Integer = 0 To waveforms.Count - 1
            scaledRecords(i) = waveforms(i).GetScaledData()
        Next

        For rowIndex = lastCount To lastCount + (waveforms(0).SampleCount - 1)
            columnIndex = 0
            binaryDataGridView.Rows.Add()
            scaledDataGridView.Rows.Add()

            binaryDataGridView.Rows(rowIndex).Cells(columnIndex).Value = (rowIndex + 1).ToString()
            scaledDataGridView.Rows(rowIndex).Cells(columnIndex).Value = (rowIndex + 1).ToString()
            columnIndex += 1

            For Each waveform As AnalogWaveform(Of T) In waveforms
                binaryDataGridView.Rows(rowIndex).Cells(columnIndex).Value = waveform.Samples(rowIndex - lastCount).Value.ToString()
                scaledDataGridView.Rows(rowIndex).Cells(columnIndex).Value = scaledRecords(columnIndex - 1)(rowIndex - lastCount).ToString("E")
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

    Private Sub DisplayMessage(ByVal message As String)
        messageTextBox.Text = message
        Me.Refresh()
    End Sub

    Private Sub ChangeControlState(ByVal isEnabled As Boolean)
        generalGroupBox.Enabled = isEnabled
        horizontalGroupBox.Enabled = isEnabled
        verticalGroupBox.Enabled = isEnabled
        triggerGroupBox.Enabled = isEnabled
        acquireButton.Enabled = isEnabled
        stopButton.Enabled = Not isEnabled
        If Not isEnabled Then
            ClearWaveforms()
        End If
        Me.Refresh()
    End Sub

    Private Sub CloseSession()
        Try
            If scopeSession IsNot Nothing Then
                scopeSession.Close()
                scopeSession = Nothing
            End If
        Catch ex As Exception
            ShowError(ex)
            Application.[Exit]()
        End Try
    End Sub

    Private Sub ShowError(ByVal ex As Exception)
        messageTextBox.Clear()
        MessageBox.Show(ex.Message, ex.[GetType]().ToString(), MessageBoxButtons.OK, MessageBoxIcon.[Error])
    End Sub
End Class
