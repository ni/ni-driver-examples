'=======================================================================================================
'
' Title:
'      External Clocking
'
' Description:
'      This example demonstrates the external clock feature of the digitizer. In this mode,
'      an external signal is used to run the instrument. This external signal must have
'      frequency of at least 30Mhz and at most 105Mhz.       
'
'======================================================================================================

Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments.NIScope
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Partial Public Class MainForm
    Inherits Form
    Private scopeSession As NIScope
    Private [stop] As Boolean

    Public Sub New()
        InitializeComponent()
        ConfigureTimebaseSourceComboBox()
        LoadScopeDeviceNames()
        ChangeControlState(True)
    End Sub

#Region "Mainform initial configuration"
    Private Sub ConfigureTimebaseSourceComboBox()
        timebaseSourceComboBox.Items.Add(ScopeSampleClockTimebaseSource.ClockIn)
        timebaseSourceComboBox.Items.Add(ScopeSampleClockTimebaseSource.PxiStar)
        timebaseSourceComboBox.Items.Add(ScopeSampleClockTimebaseSource.Pfi0)
        timebaseSourceComboBox.Items.Add(ScopeSampleClockTimebaseSource.Pfi1)
        timebaseSourceComboBox.SelectedIndex = 0
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

    Private ReadOnly Property RecordLengthMin() As Integer
        Get
            Return Decimal.ToInt32(Me.recordLengthMinNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property TimebaseSource() As ScopeSampleClockTimebaseSource
        Get
            Return DirectCast(Me.timebaseSourceComboBox.SelectedItem, ScopeSampleClockTimebaseSource)
        End Get
    End Property

    Private ReadOnly Property TimebaseRate() As Double
        Get
            Return Decimal.ToDouble(Me.timebaseRateNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property TimebaseDivisor() As Integer
        Get
            Return Decimal.ToInt32(Me.timebaseDivisorNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property TimebaseMultiplier() As Integer
        Get
            Return Decimal.ToInt32(Me.timebaseMultiplierNumeric.Value)
        End Get
    End Property

    Private WriteOnly Property ActualSampleRate() As Double
        Set(ByVal value As Double)
            Me.actualSampleRateTextBox.Text = value.ToString("E")
        End Set
    End Property
#End Region

    Private Sub acquireButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        StartAcquisition()
    End Sub

    Private Sub MainForm_Closing(ByVal sender As Object, ByVal e As FormClosingEventArgs)
        CloseSession()
    End Sub

    Private Sub stopButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        StopAcquistion()
    End Sub

    Private Sub StartAcquisition()
        [stop] = False
        ChangeControlState(False)
        DisplayMessage("Acquisition is in progress...")

        Dim timeout As New PrecisionTimeSpan(5.0)
        Dim holdOff As PrecisionTimeSpan = PrecisionTimeSpan.Zero
        Dim scaledWaveForms As AnalogWaveformCollection(Of Double) = Nothing
        Try
            InitializeSession()

            ' Configure the vertical parameters.
            Dim verticalCoupling As ScopeVerticalCoupling = ScopeVerticalCoupling.DC
            Dim probeAttenuation As Double = 1.0
            scopeSession.Channels(ChannelName).Configure(VerticalRange, VerticalOffset, verticalCoupling, probeAttenuation, True)

            ' Configure the record length and reference position.
            Dim referencePosition As Double = 50.0
            scopeSession.Acquisition.NumberOfPointsMin = RecordLengthMin
            scopeSession.Trigger.ReferenceTrigger.ReferencePosition = referencePosition

            ' Configure immediate triggering.
            scopeSession.Trigger.ConfigureTriggerImmediate()

            ' Configure the external clocking attributes.
            scopeSession.Timing.SampleClockTimebaseSource = TimebaseSource
            scopeSession.Timing.SampleClockTimebaseDivisor = TimebaseDivisor
            scopeSession.Timing.SampleClockTimebaseRate = TimebaseRate
            scopeSession.Timing.SampleClockTimebaseMultiplier = TimebaseMultiplier

            ' Query the coerced record length.
            Dim actualRecordLength As Long = scopeSession.Acquisition.RecordLength

            ' // Query the actual sample rate.
            Dim actualSampRate As Double = scopeSession.Acquisition.SampleRate
            ActualSampleRate = actualSampRate

            ' Loop until the stop flag is set.  This example loops around initiate acquisition
            ' and fetch, using the same configuration for every acquisition.  This is the 
            ' most efficient method for acquiring multiple waveforms with the same configuration
            ' parameters. (Although typically, you would scale the data at a later time.)
            While Not [stop]
                scaledWaveForms = scopeSession.Channels(ChannelName).Measurement.Read(timeout, actualRecordLength, scaledWaveForms)
                PlotWaveforms(scaledDataGridView, scaledWaveForms)
            End While
            DisplayMessage("Acquisition successful!!!")
        Catch ex As Exception
            ShowError(ex)
        Finally
            CloseSession()
            ChangeControlState(True)
        End Try
    End Sub

    Private Sub InitializeSession()
        scopeSession = New NIScope(ResourceName, False, False)
        AddHandler scopeSession.DriverOperation.Warning, New EventHandler(Of ScopeWarningEventArgs)(AddressOf DriverOperation_Warning)
    End Sub

    Private Sub DriverOperation_Warning(ByVal sender As Object, ByVal e As ScopeWarningEventArgs)
        messageTextBox.Clear()
        MessageBox.Show(e.Text, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Sub

    Private Sub StopAcquistion()
        If Not [stop] Then
            DisplayMessage("Stop in progress...Fetched points are being plotted...")
            [stop] = True
        End If
    End Sub

    Private Sub ClearWaveforms()
        scaledDataGridView.Columns.Clear()
    End Sub

    Private Shared Sub PlotWaveforms(ByVal dgv As DataGridView, ByVal waveforms As AnalogWaveformCollection(Of Double))
        Dim rowIndex As Integer, columnIndex As Integer
        Dim lastCount As Integer = dgv.RowCount

        SetupDataGridView(dgv, waveforms.Count)
        For rowIndex = lastCount To lastCount + (waveforms(0).SampleCount - 1)
            columnIndex = 0
            dgv.Rows.Add()
            dgv.Rows(rowIndex).Cells(columnIndex).Value = (rowIndex + 1).ToString()
            columnIndex = columnIndex + 1
            For Each waveform As AnalogWaveform(Of Double) In waveforms
                dgv.Rows(rowIndex).Cells(columnIndex).Value = waveform.Samples(rowIndex - lastCount).Value.ToString("E")
                columnIndex = columnIndex + 1
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
        verticalAndHorizontalGroupBox.Enabled = isEnabled
        externalClockGroupBox.Enabled = isEnabled
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
