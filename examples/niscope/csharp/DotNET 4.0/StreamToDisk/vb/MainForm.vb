'==================================================================================================
'
' Title:
'      Stream To Disk
'
' Description:
'      This application demonstrates the programmatic usage of .NET API for  NI-SCOPE. 
'      This application acquires data from a channel and saves it to a file on the local 
'      disk. The waveform stored in the file can be loaded onto a datagrid.
'
'==================================================================================================

Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments.NIScope
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Partial Public Class MainForm
    Inherits Form
    Private scopeSession As NIScope

    Public Sub New()
        InitializeComponent()
        LoadScopeDeviceNames()
        ChangeControlState(True)
    End Sub

#Region "Mainform Initial Configuration"
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
            Return Decimal.ToInt32(Me.recordLengthMinNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property Acquire() As Boolean
        Get
            Return Me.acquireRadioButton.Checked
        End Get
    End Property

    Private ReadOnly Property FilePath() As String
        Get
            Return Me.filePathTextBox.Text
        End Get
    End Property
#End Region

    Private Sub startButton_Click(sender As Object, e As System.EventArgs)
        BinaryAcquisition()
    End Sub

    Private Sub InitializeSession()
        scopeSession = New NIScope(ResourceName, False, False)
        AddHandler scopeSession.DriverOperation.Warning, New EventHandler(Of ScopeWarningEventArgs)(AddressOf DriverOperation_Warning)
    End Sub

    Private Sub DriverOperation_Warning(sender As Object, e As ScopeWarningEventArgs)
        MessageBox.Show(e.Text, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Sub

    Private Sub BinaryAcquisition()
        ChangeControlState(False)

        Dim sampledwaveforms As AnalogWaveformCollection(Of Double) = Nothing
        Try
            If Acquire Then
                InitializeSession()

                ' Configure the vertical parameters.
                Dim verticalOffset As Double = 0.0
                Dim verticalCoupling As ScopeVerticalCoupling = ScopeVerticalCoupling.DC
                Dim probeAttenuation As Double = 1.0
                scopeSession.Channels(ChannelName).Configure(VerticalRange, verticalOffset, verticalCoupling, probeAttenuation, True)

                ' Configure the horizontal parameters.
                Dim referencePosition As Double = 50.0
                Dim numberOfRecords As Integer = 1
                Dim enforceRealTime As Boolean = True
                scopeSession.Timing.ConfigureTiming(SampleRateMin, RecordLengthMin, referencePosition, numberOfRecords, enforceRealTime)

                scopeSession.Measurement.Initiate()
                Dim recordLength As Long = scopeSession.Acquisition.RecordLength
                Dim timeout As New PrecisionTimeSpan(5.0)
                sampledwaveforms = scopeSession.Channels(ChannelName).Measurement.FetchDouble(timeout, recordLength, sampledwaveforms)
                PlotWaveforms(Of Double)(acquiredDataGridView, sampledwaveforms)

                ' Configure the folder location.
                Dim directoryPath As String = "C:\waveform"
                If Not Directory.Exists(directoryPath) Then
                    Directory.CreateDirectory(directoryPath)
                End If

                Dim filePath As New FileInfo(Path.Combine(directoryPath, "waveform.txt"))
                If Not filePath.Exists Then
                    filePath.Create().Close()
                Else
                    File.WriteAllText(filePath.ToString(), [String].Empty)
                End If

                Using fileStream As New FileStream(filePath.ToString(), FileMode.Create, FileAccess.ReadWrite)
                    Dim binaryFormatter As New BinaryFormatter()
                    binaryFormatter.Serialize(fileStream, sampledwaveforms)
                End Using
            Else
                Using fileStream As New FileStream(FilePath, FileMode.Open)
                    Dim binaryFormatter As New BinaryFormatter()
                    Dim records As AnalogWaveformCollection(Of Double) = TryCast(binaryFormatter.Deserialize(fileStream), AnalogWaveformCollection(Of Double))
                    PlotWaveforms(Of Double)(recordDataGridView, records)
                End Using
            End If
        Catch ex As Exception
            ShowError(ex)
        Finally
            CloseSession()
            ChangeControlState(True)
        End Try
    End Sub

    Private Sub ClearWaveforms()
        acquiredDataGridView.Columns.Clear()
        recordDataGridView.Columns.Clear()
    End Sub

    Private Shared Sub PlotWaveforms(Of T)(dgv As DataGridView, waveforms As AnalogWaveformCollection(Of T))
        Dim rowindex As Integer, columnIndex As Integer
        Dim lastCount As Integer = dgv.RowCount

        SetupDataGridView(dgv, waveforms.Count)
        For rowindex = lastCount To lastCount + (waveforms(0).SampleCount - 1)
            columnIndex = 0
            dgv.Rows.Add()
            dgv.Rows(rowindex).Cells(columnIndex).Value = (rowindex + 1).ToString()
            columnIndex += 1
            For Each waveform As AnalogWaveform(Of T) In waveforms
                dgv.Rows(rowindex).Cells(columnIndex).Value = waveform.Samples(rowindex - lastCount).Value.ToString()
                columnIndex += 1
            Next
        Next
    End Sub

    Private Shared Sub SetupDataGridView(dgv As DataGridView, numberOfWaveforms As Integer)
        If dgv.Columns.Count > 0 Then
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

    Private Sub browseButton_Click(sender As Object, e As EventArgs)
        Dim op As New OpenFileDialog()
        If op.ShowDialog() = DialogResult.OK Then
            filePathTextBox.Text = op.FileName
        End If
    End Sub

    Private Sub radioButton_CheckedChanged(sender As Object, e As EventArgs)
        ChangeControlState(True)
    End Sub

    Private Sub ChangeControlState(isEnabled As Boolean)
        If isEnabled Then
            configurationGroupBox.Enabled = Acquire
            filePathGroupBox.Enabled = Not Acquire
        Else
            ClearWaveforms()
            configurationGroupBox.Enabled = False
            filePathGroupBox.Enabled = False
        End If
        startButton.Enabled = isEnabled
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

    Private Shared Sub ShowError(ex As Exception)
        MessageBox.Show(ex.Message, ex.[GetType]().ToString(), MessageBoxButtons.OK, MessageBoxIcon.[Error])
    End Sub
End Class
