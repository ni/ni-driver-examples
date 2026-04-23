'==================================================================================================
'
' Title:
'      Digital Filtering

' Description:
'      The NI-SCOPE driver includes many measurements that may be performed on the 
'      acquired waveforms from your digitizer.  This example illustrates using digital 
'      filters to eliminate noise in specific frequency ranges of your signal.
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
        ConfigureFftFunctionComboBox()
        ConfigureFilterTypeComboBox()
        ConfigureFirWindowComboBox()
        ConfigureFilterComboBox()
        LoadScopeDeviceNames()
        ChangeControlState(True)
    End Sub

#Region "Mainform initial configuration"
    Private Sub ConfigureFftFunctionComboBox()
        fftFunctionComboBox.Items.Add(ScopeArrayMeasurementType.FftAmplitudeSpectrumVoltsRms)
        fftFunctionComboBox.Items.Add(ScopeArrayMeasurementType.FftAmplitudeSpectrumDB)
        fftFunctionComboBox.SelectedIndex = 1
    End Sub

    Private Sub ConfigureFilterTypeComboBox()
        For Each enumValue As ScopeMeasurementFilterType In [Enum].GetValues(GetType(ScopeMeasurementFilterType))
            filterTypeComboBox.Items.Add(enumValue)
        Next
        filterTypeComboBox.SelectedIndex = 0
    End Sub

    Private Sub ConfigureFirWindowComboBox()
        firWindowComboBox.Items.Add(ScopeMeasurementFirFilterWindow.None)
        firWindowComboBox.Items.Add(ScopeMeasurementFirFilterWindow.HanningWindow)
        firWindowComboBox.Items.Add(ScopeMeasurementFirFilterWindow.FlatTopWindow)
        firWindowComboBox.Items.Add(ScopeMeasurementFirFilterWindow.HammingWindow)
        firWindowComboBox.Items.Add(ScopeMeasurementFirFilterWindow.TriangleWindow)
        firWindowComboBox.Items.Add(ScopeMeasurementFirFilterWindow.BlackmanWindow)
        firWindowComboBox.SelectedIndex = 1
    End Sub

    Private Sub ConfigureFilterComboBox()
        filterComboBox.Items.Add(ScopeArrayMeasurementType.NoMeasurement)
        filterComboBox.Items.Add(ScopeArrayMeasurementType.WindowedFirFilter)
        filterComboBox.Items.Add(ScopeArrayMeasurementType.BesselFilter)
        filterComboBox.Items.Add(ScopeArrayMeasurementType.ButterworthFilter)
        filterComboBox.Items.Add(ScopeArrayMeasurementType.ChebyshevFilter)
        filterComboBox.SelectedIndex = 4
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

    Private ReadOnly Property Filter() As ScopeArrayMeasurementType
        Get
            Return CType(Me.filterComboBox.SelectedItem, ScopeArrayMeasurementType)
        End Get
    End Property

    Private ReadOnly Property FftFunction() As ScopeArrayMeasurementType
        Get
            Return CType(Me.fftFunctionComboBox.SelectedItem, ScopeArrayMeasurementType)
        End Get
    End Property

    Private ReadOnly Property FilterType() As ScopeMeasurementFilterType
        Get
            Return CType(Me.filterTypeComboBox.SelectedItem, ScopeMeasurementFilterType)
        End Get
    End Property

    Private ReadOnly Property CutoffFrequency() As Double
        Get
            Return Decimal.ToDouble(Me.lowOrHighPassCutoffFrequencyNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property CenterFrequency() As Double
        Get
            Return Decimal.ToDouble(Me.bandpassOrStopCenterFrequencyNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property BandWidth() As Double
        Get
            Return Decimal.ToDouble(Me.bandpassOrBandstopWidthNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property FirTaps() As Integer
        Get
            Return Decimal.ToInt32(Me.firTapsNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property FirWindow() As ScopeMeasurementFirFilterWindow
        Get
            Return CType(Me.firWindowComboBox.SelectedItem, ScopeMeasurementFirFilterWindow)
        End Get
    End Property

    Private ReadOnly Property IirOrder() As Integer
        Get
            Return Decimal.ToInt32(Me.iirOrderNumeric.Value)
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

    Private Sub DriverOperation_Warning(sender As Object, e As ScopeWarningEventArgs)
        messageTextBox.Clear()
        MessageBox.Show(e.Text, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Sub

    Private Sub StartAcquisition()
        [stop] = False
        ChangeControlState(False)
        DisplayMessage("Acquisition is in progress...")

        Dim filteredWaveforms As AnalogWaveformCollection(Of Double) = Nothing
        Dim spectrumWaveforms As AnalogWaveformCollection(Of Double) = Nothing
        Try
            InitializeSession()
            While Not [stop]
                scopeSession.Channels(ChannelName).Enabled = True

                ' Configure the vertical parameters such as input range and coupling.
                scopeSession.Channels(ChannelName).Range = 2
                scopeSession.Channels(ChannelName).Coupling = ScopeVerticalCoupling.DC

                ' Configure the horizontal parameters such as sampling rate and number of samples to acquire.
                scopeSession.Timing.ConfigureTiming(SampleRateMin, RecordLengthMin, 50, 1, True)

                ' Configure all the attributes associated with the digital filter.
                Dim measurementFilter As ScopeChannelMeasurementFilter = scopeSession.Channels(ChannelName).Measurement.Filter
                measurementFilter.CutoffFrequency = CutoffFrequency
                measurementFilter.CenterFrequency = CenterFrequency
                measurementFilter.Taps = FirTaps
                measurementFilter.FirFilterWindow = FirWindow
                measurementFilter.Order = IirOrder
                measurementFilter.TransientPercent = 20
                measurementFilter.Ripple = 0.5
                measurementFilter.Type = FilterType
                measurementFilter.Width = BandWidth

                ' Add the user requested filter process. The measurement library will apply this process step 
                ' to the acquired data before calculating any of the array measurements.
                scopeSession.Channels(ChannelName).Measurement.AddWaveformProcessing(Filter)

                ' Initiate a new acquisition.
                scopeSession.Measurement.Initiate()

                ' Show the filtered data.
                filteredWaveforms = scopeSession.Channels(ChannelName).Measurement.FetchArrayMeasurement(New PrecisionTimeSpan(), ScopeArrayMeasurementType.NoMeasurement, filteredWaveforms)

                ' Apply an FFT to the filtered data and display the results.
                spectrumWaveforms = scopeSession.Channels(ChannelName).Measurement.FetchArrayMeasurement(New PrecisionTimeSpan(), FftFunction, spectrumWaveforms)

                ' Clear the filter process.
                scopeSession.Channels(ChannelName).Measurement.ClearWaveformProcessing()

                PlotWaveforms(filteredWaveformDataGridView, filteredWaveforms)
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
        filteredWaveformDataGridView.Columns.Clear()
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

    Private Sub StopAcquisition()
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
        generalGroupBox.Enabled = isEnabled
        timingParametersGroupBox.Enabled = isEnabled
        functionsGroupBox.Enabled = isEnabled
        filterParametersGroupBox.Enabled = isEnabled
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
