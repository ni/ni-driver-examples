'=====================================================================================================
'
' Title:
'        Flexible Resolution
'
' Description:
'      This example configures a flexible resolution acquisition with the Configure Acquisition function.
'      The vertical and horizontal parameters are configured as always.  The sampling rate used in this 
'      example will be coerced to a sampling rate supported in flexible resolution.  This example also 
'      displays the effective number of bits at each sampling rate.
'
'==================================================================================================

Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments.NIScope
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Partial Public Class MainForm
    Inherits Form
    ReadOnly ChannelName As String = "0"

    Private scopeSession As NIScope
    Private [stop] As Boolean

    Public Sub New()
        InitializeComponent()
        ConfigureTriggerTypeComboBox()
        LoadScopeDeviceNames()
        ChangeControlState(True)
    End Sub

#Region "Mainform initial configuration"
    Private Sub ConfigureTriggerTypeComboBox()
        triggerTypeComboBox.Items.Add(ScopeTriggerType.Immediate)
        triggerTypeComboBox.Items.Add(ScopeTriggerType.Edge)
        triggerTypeComboBox.SelectedIndex = 0
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

    Private ReadOnly Property SampleRateMin() As Double
        Get
            Return Decimal.ToDouble(Me.minSampleRateNumeric.Value)
        End Get
    End Property

    Private WriteOnly Property ActualSamplerate() As String
        Set(ByVal value As String)
            Me.actualSampleRateTextBox.Text = value
        End Set
    End Property

    Private WriteOnly Property Resolution() As String
        Set(ByVal value As String)
            Me.resolutionTextBox.Text = value
        End Set
    End Property

    Private ReadOnly Property RecordLengthMin() As Integer
        Get
            Return Decimal.ToInt32(Me.minRecordLengthNumeric.Value)
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
        StopAcquisition()
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

        Dim waveforms As AnalogWaveformCollection(Of Double) = Nothing
        Dim waveformInfo As ScopeWaveformInfo() = Nothing
        Dim freqwaveforms As AnalogWaveformCollection(Of Double) = Nothing
        Dim freqwaveformInfo As ScopeWaveformInfo() = Nothing

        Try
            InitializeSession()
            scopeSession.Channels(ChannelName).Measurement.AddWaveformProcessing(ScopeArrayMeasurementType.HanningWindow)

            While Not [stop]
                scopeSession.Acquisition.Type = ScopeAcquisitionType.FlexibleResolution

                ' Configure the vertical parameters.
                Dim offset As Double = 0.0
                Dim probeAttenuation As Double = 1.0
                Dim coupling As ScopeVerticalCoupling = ScopeVerticalCoupling.DC
                scopeSession.Channels(ScopeTriggerSource.Channel0).Configure(VerticalRange, offset, coupling, probeAttenuation, True)

                ' Configure the horizontal parameters.
                Dim referencePosition As Double = 50.0
                Dim numberOfRecords As Int32 = 1
                Dim enforceRealTime As [Boolean] = False
                scopeSession.Timing.ConfigureTiming(SampleRateMin, RecordLengthMin, referencePosition, numberOfRecords, enforceRealTime)

                If TriggerType = ScopeTriggerType.Edge Then
                    Dim triggerLevel As Double = 0.0
                    Dim triggerSlope As ScopeTriggerSlope = ScopeTriggerSlope.Positive
                    Dim triggerCoupling As ScopeTriggerCoupling = ScopeTriggerCoupling.DC
                    Dim triggerSource As ScopeTriggerSource = ScopeTriggerSource.Channel0
                    Dim triggerHoldoff As PrecisionTimeSpan = PrecisionTimeSpan.Zero
                    Dim triggerDelay As PrecisionTimeSpan = PrecisionTimeSpan.Zero
                    scopeSession.Trigger.EdgeTrigger.Configure(triggerSource, triggerLevel, triggerSlope, triggerCoupling, triggerHoldoff, triggerDelay)
                Else
                    scopeSession.Trigger.ConfigureTriggerImmediate()
                End If

                Dim timeout As New PrecisionTimeSpan(5.0)

                scopeSession.Measurement.Initiate()
                Dim actualRecordLength As Long = scopeSession.Acquisition.RecordLength
                waveforms = scopeSession.Channels(ScopeTriggerSource.Channel0).Measurement.FetchDouble(timeout, actualRecordLength, waveforms, waveformInfo)
                freqwaveforms = scopeSession.Channels(ScopeTriggerSource.Channel0).Measurement.FetchArrayMeasurement(timeout, ScopeArrayMeasurementType.FftAmplitudeSpectrumDB, freqwaveforms, freqwaveformInfo)

                ActualSamplerate = scopeSession.Acquisition.SampleRate.ToString()
                Resolution = scopeSession.Acquisition.Resolution.ToString()

                PlotWaveforms(sampledDataGridView, waveforms)
                PlotWaveforms(measurementDataGridView, freqwaveforms)
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
        Me.Refresh()
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

    Private Sub StopAcquisition()
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
