'==================================================================================================
'
' Title:
'      Windowing
'
' Description:
'      This example illustrates using some of the advanced measurement library functions 
'      do a windowed, FFT measurement. Windowing is a technique to reduce the 
'      spectral leakage in FFT measurements.
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
    Private [stop] As Boolean

    Public Sub New()
        InitializeComponent()
        ConfigureWindowComboBox()
        ConfigureFftFunctionComboBox()
        LoadScopeDeviceNames()
        ChangeControlState(True)
    End Sub

#Region "MainForm initial configuration"
    Private Sub ConfigureWindowComboBox()
        windowComboBox.Items.Add(ScopeArrayMeasurementType.NoMeasurement)
        windowComboBox.Items.Add(ScopeArrayMeasurementType.HanningWindow)
        windowComboBox.Items.Add(ScopeArrayMeasurementType.FlatTopWindow)
        windowComboBox.Items.Add(ScopeArrayMeasurementType.HammingWindow)
        windowComboBox.Items.Add(ScopeArrayMeasurementType.TriangleWindow)
        windowComboBox.Items.Add(ScopeArrayMeasurementType.BlackmanWindow)
        windowComboBox.SelectedIndex = 1
    End Sub

    Private Sub ConfigureFftFunctionComboBox()
        fftFunctionComboBox.Items.Add(ScopeArrayMeasurementType.FftAmplitudeSpectrumVoltsRms)
        fftFunctionComboBox.Items.Add(ScopeArrayMeasurementType.FftAmplitudeSpectrumDB)
        fftFunctionComboBox.SelectedIndex = 1
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

#Region "Information configuration values"
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

    Private ReadOnly Property FftFunction() As ScopeArrayMeasurementType
        Get
            Return CType(Me.fftFunctionComboBox.SelectedItem, ScopeArrayMeasurementType)
        End Get
    End Property

    Private ReadOnly Property Window() As ScopeArrayMeasurementType
        Get
            Return CType(Me.windowComboBox.SelectedItem, ScopeArrayMeasurementType)
        End Get
    End Property

    Private ReadOnly Property AverageSpectrum() As Boolean
        Get
            Return Me.averageSpectrumCheckBox.Checked
        End Get
    End Property

    Private ReadOnly Property ClearAveraging() As Boolean
        Get
            Return Me.clearAveragingCheckBox.Checked
        End Get
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
        ' Open a session to the digitizer.
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
        Dim spectrumWaveforms As AnalogWaveformCollection(Of Double) = Nothing
        Try
            InitializeSession()
            While Not [stop]
                ' Configure the vertical parameters such as input range, offset, and coupling.
                scopeSession.Channels(ChannelName).Range = 5

                ' Configure the horizontal parameters such as sampling rate and number of samples to acquire.
                Dim referencePosition As Double = 50.0
                Dim numberOfRecords As Integer = 1
                Dim enforceRealtime As Boolean = True
                scopeSession.Timing.ConfigureTiming(SampleRateMin, RecordLengthMin, referencePosition, numberOfRecords, enforceRealtime)

                ' Initiate a new acquisition with the configured settings, wait for 
                ' this acquisition to complete, and fetch the data. Plot the data.
                waveformFromScope = scopeSession.Channels(ChannelName).Measurement.Read(PrecisionTimeSpan.MaxValue, -1, waveformFromScope)

                ' Add the user-requested Window processing step.
                scopeSession.Channels(ChannelName).Measurement.AddWaveformProcessing(Window)

                ' Add the user-requested FFT function processing step. This processing step will be applied
                ' after the Window processing step has been applied to the acquired data.
                scopeSession.Channels(ChannelName).Measurement.AddWaveformProcessing(FftFunction)

                ' Calculate and display the running average of the waveform. The driver keeps a cached running
                ' average of the waveform. Every time this function is called the driver adds the current 
                ' processed data to the running average. Clear stats clears the cached data.
                Dim arrayMeasurementType As ScopeArrayMeasurementType = If(AverageSpectrum, ScopeArrayMeasurementType.MultipleAcquisitionAverage, ScopeArrayMeasurementType.NoMeasurement)
                spectrumWaveforms = scopeSession.Channels(ChannelName).Measurement.FetchArrayMeasurement(New PrecisionTimeSpan(), arrayMeasurementType, spectrumWaveforms)

                ' If the user requests, clear any cached statistics.
                If ClearAveraging OrElse Not AverageSpectrum Then
                    scopeSession.Channels(ChannelName).Measurement.ClearWaveformMeasurements()
                End If

                ' Clear all processes.
                scopeSession.Channels(ChannelName).Measurement.ClearWaveformProcessing()

                PlotWaveforms(waveformFromScopeDataGridView, waveformFromScope)
                PlotWaveforms(spectrumDataGridView, spectrumWaveforms)
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
        spectrumDataGridView.Columns.Clear()
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
        channelTextBox.Enabled = isEnabled
        scalarMeasurementsGroupBox.Enabled = isEnabled
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
