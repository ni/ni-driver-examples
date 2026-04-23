'==================================================================================================
'
' Title:
'      GettingStarted
'
' Description:
'      This example opens a session to the NI-SCOPE driver by passing in the device's 
'      resource name to the Init function.  The resource name is a unique identifier 
'      for your National Instruments hardware product.  It can be found (and changed) 
'      by running Measurement & Automation Explorer (MAX). 
'
'==================================================================================================

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

#Region "Mainform Configuration values"
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

    Private WriteOnly Property RecordLength() As String
        Set(value As String)
            Me.actualRecordLengthTextBox.Text = value
        End Set
    End Property

    Private WriteOnly Property SampleRate() As String
        Set(value As String)
            Me.actualSampleRateTextBox.Text = value
        End Set
    End Property
#End Region

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

    Private Sub acquireButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles acquireButton.Click
        StartAcquisition()
    End Sub

    Private Sub mainForm_Closing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles MyBase.FormClosing
        CloseSession()
    End Sub

    Private Sub InitializeSession()
        scopeSession = New NIScope(ResourceName, False, False)
        AddHandler scopeSession.DriverOperation.Warning, New EventHandler(Of ScopeWarningEventArgs)(AddressOf DriverOperation_Warning)
    End Sub

    Private Sub DriverOperation_Warning(sender As Object, e As ScopeWarningEventArgs)
        MessageBox.Show(e.Text, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Sub

    Private Sub StartAcquisition()
        ChangeControlState(False)

        Dim timeout As New PrecisionTimeSpan(5.0)
        Dim waveforms As AnalogWaveformCollection(Of Double) = Nothing
        Try
            InitializeSession()
            scopeSession.Measurement.AutoSetup()

            Dim recordLength As Long = scopeSession.Acquisition.RecordLength
            Dim sampleRate As Double = scopeSession.Acquisition.SampleRate

            waveforms = scopeSession.Channels(ChannelName).Measurement.Read(timeout, recordLength, waveforms)

            DisplayResults(recordLength, sampleRate)
            PlotWaveforms(sampledDataGridView, waveforms)
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

    Private Shared Sub PlotWaveforms(dgv As DataGridView, waveforms As AnalogWaveformCollection(Of Double))
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

    Private Sub DisplayResults(length As Long, rate As Double)
        RecordLength = length.ToString()
        SampleRate = rate.ToString("E")
    End Sub

    Private Shared Sub SetupDataGridView(dgv As DataGridView, numberOfWaveforms As Integer)
        If dgv.ColumnCount > 0 Then
            Return
        End If

        Dim indexColumn As New DataGridViewTextBoxColumn()
        indexColumn.Width = 60
        indexColumn.HeaderText = "Index"
        dgv.Columns.Add(indexColumn)

        For waveformIndex As Integer = 0 To numberOfWaveforms - 1
            Dim waveformColumn As New DataGridViewTextBoxColumn()
            waveformColumn.Width = 125
            waveformColumn.HeaderText = "Waveform " & waveformIndex
            dgv.Columns.Add(waveformColumn)
        Next
    End Sub

    Private Sub ChangeControlState(isEnabled As Boolean)
        acquireButton.Enabled = isEnabled
        resourceNameComboBox.Enabled = isEnabled
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

    Private Shared Sub ShowError(ex As Exception)
        MessageBox.Show(ex.Message, ex.[GetType]().ToString(), MessageBoxButtons.OK, MessageBoxIcon.[Error])
    End Sub
End Class
