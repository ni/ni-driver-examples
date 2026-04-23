'==================================================================================================
'
' Title:
'        Configured Acquisition
'
' Description:
'      This application demonstrates acquiring data from multiple channels of a high-speed 
'      digitizer device. The application allows you to configure various Horizontal and Vertical
'      parameters along with the Trigger parameters. The fetched data is plotted onto a datagrid. 
'
'=================================================================================================

Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments.NIScope
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Partial Public Class MainForm
    Inherits Form
    Private scopeSession As NIScope
    Private [stop] As Boolean

    Public Sub New()
        InitializeComponent()
        ConfigureAcquisitionTypeComboBox()
        ConfigureVerticalCouplingComboBox()
        ConfigureInputImpedanceComboBox()
        ConfigureTriggerTypeComboBox()
        ConfigureTriggerSourceComboBox()
        ConfigureTriggerCouplingComboBox()
        ConfigureTriggerSlopeComboBox()
        ConfigureWindowModeComboBox()
        LoadScopeDeviceNames()
        ChangeControlState(True)
    End Sub

#Region "Mainform initial configuration"
    Private Sub ConfigureAcquisitionTypeComboBox()
        acquisitionTypeComboBox.Items.Add(ScopeAcquisitionType.Normal)
        acquisitionTypeComboBox.Items.Add(ScopeAcquisitionType.FlexibleResolution)
        acquisitionTypeComboBox.SelectedIndex = 0
    End Sub

    Private Sub ConfigureVerticalCouplingComboBox()
        For Each value As ScopeVerticalCoupling In [Enum].GetValues(GetType(ScopeVerticalCoupling))
            verticalCouplingComboBox.Items.Add(value)
        Next
        verticalCouplingComboBox.SelectedIndex = 1
    End Sub

    Private Sub ConfigureInputImpedanceComboBox()
        inputImpedanceComboBox.Items.Add(50)
        inputImpedanceComboBox.Items.Add(1000000)
        inputImpedanceComboBox.SelectedIndex = 1
    End Sub

    Private Sub ConfigureTriggerTypeComboBox()
        triggerTypeComboBox.Items.Add(ScopeTriggerType.DigitalEdge)
        triggerTypeComboBox.Items.Add(ScopeTriggerType.Edge)
        triggerTypeComboBox.Items.Add(ScopeTriggerType.Hysteresis)
        triggerTypeComboBox.Items.Add(ScopeTriggerType.Immediate)
        triggerTypeComboBox.Items.Add(ScopeTriggerType.Window)
        triggerTypeComboBox.SelectedIndex = 3
    End Sub

    Private Sub ConfigureTriggerSourceComboBox()
        triggerSourceComboBox.Items.Add(ScopeTriggerSource.Channel0)
        triggerSourceComboBox.Items.Add(ScopeTriggerSource.Channel1)
        triggerSourceComboBox.Items.Add(ScopeTriggerSource.Channel2)
        triggerSourceComboBox.Items.Add(ScopeTriggerSource.Channel3)
        triggerSourceComboBox.Items.Add(ScopeTriggerSource.Channel4)
        triggerSourceComboBox.Items.Add(ScopeTriggerSource.Channel5)
        triggerSourceComboBox.Items.Add(ScopeTriggerSource.Channel6)
        triggerSourceComboBox.Items.Add(ScopeTriggerSource.Channel7)
        triggerSourceComboBox.Items.Add(ScopeTriggerSource.External)
        triggerSourceComboBox.Items.Add(ScopeTriggerSource.Rtsi0)
        triggerSourceComboBox.Items.Add(ScopeTriggerSource.Rtsi1)
        triggerSourceComboBox.Items.Add(ScopeTriggerSource.Rtsi2)
        triggerSourceComboBox.Items.Add(ScopeTriggerSource.Rtsi3)
        triggerSourceComboBox.Items.Add(ScopeTriggerSource.Rtsi4)
        triggerSourceComboBox.Items.Add(ScopeTriggerSource.Rtsi5)
        triggerSourceComboBox.Items.Add(ScopeTriggerSource.Rtsi6)
        triggerSourceComboBox.Items.Add(ScopeTriggerSource.Pfi0)
        triggerSourceComboBox.Items.Add(ScopeTriggerSource.Pfi1)
        triggerSourceComboBox.Items.Add(ScopeTriggerSource.Pfi2)
        triggerSourceComboBox.Items.Add(ScopeTriggerSource.PxiStar)
        triggerSourceComboBox.SelectedIndex = 0
    End Sub

    Private Sub ConfigureTriggerCouplingComboBox()
        For Each value As ScopeTriggerCoupling In [Enum].GetValues(GetType(ScopeTriggerCoupling))
            triggerCouplingComboBox.Items.Add(value)
        Next
        triggerCouplingComboBox.SelectedIndex = 1
    End Sub

    Private Sub ConfigureTriggerSlopeComboBox()
        For Each value As ScopeTriggerSlope In [Enum].GetValues(GetType(ScopeTriggerSlope))
            triggerSlopeComboBox.Items.Add(value)
        Next
        triggerSlopeComboBox.SelectedIndex = 1
    End Sub

    Private Sub ConfigureWindowModeComboBox()
        For Each value As ScopeWindowTriggerMode In [Enum].GetValues(GetType(ScopeWindowTriggerMode))
            windowModeComboBox.Items.Add(value)
        Next
        windowModeComboBox.SelectedIndex = 0
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

    Private ReadOnly Property Timeout() As PrecisionTimeSpan
        Get
            Return PrecisionTimeSpan.FromSeconds(Decimal.ToDouble(Me.timeoutNumeric.Value))
        End Get
    End Property

    Private ReadOnly Property AcquisitionType() As ScopeAcquisitionType
        Get
            Return CType(Me.acquisitionTypeComboBox.SelectedItem, ScopeAcquisitionType)
        End Get
    End Property

    Private ReadOnly Property Range() As Double
        Get
            Return Decimal.ToDouble(Me.verticalRangeNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property Offset() As Double
        Get
            Return Decimal.ToDouble(Me.verticalOffsetNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property ProbeAttenuation() As Double
        Get
            Return Decimal.ToDouble(Me.probeAttenuationNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property Coupling() As ScopeVerticalCoupling
        Get
            Return CType(Me.verticalCouplingComboBox.SelectedItem, ScopeVerticalCoupling)
        End Get
    End Property

    Private ReadOnly Property SampleRateMin() As Double
        Get
            Return Decimal.ToDouble(Me.minSampleRateNumeric.Value)
        End Get
    End Property

    Private WriteOnly Property SampleRate() As String
        Set(ByVal value As String)
            Me.actualSampleRateTextBox.Text = value
        End Set
    End Property

    Private ReadOnly Property RecordLengthMin() As Integer
        Get
            Return Decimal.ToInt32(Me.minRecordLengthNumeric.Value)
        End Get
    End Property

    Private WriteOnly Property RecordLength() As String
        Set(ByVal value As String)
            Me.actualRecordLengthTextBox.Text = value
        End Set
    End Property

    Private ReadOnly Property NumberOfRecords() As Integer
        Get
            Return Decimal.ToInt32(Me.numRecordsNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property InputFrequencyMax() As Double
        Get
            Return Decimal.ToDouble(Me.maxInputFrequencyNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property InputImpedance() As Integer
        Get
            Return CInt(Me.inputImpedanceComboBox.SelectedItem)
        End Get
    End Property

    Private ReadOnly Property TriggerType() As ScopeTriggerType
        Get
            Return CType(Me.triggerTypeComboBox.SelectedItem, ScopeTriggerType)
        End Get
    End Property

    Private ReadOnly Property TriggerSource() As ScopeTriggerSource
        Get
            Return DirectCast(Me.triggerSourceComboBox.SelectedItem, ScopeTriggerSource)
        End Get
    End Property

    Private ReadOnly Property ReferencePosition() As Double
        Get
            Return Decimal.ToDouble(Me.referencePositionNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property HoldOff() As PrecisionTimeSpan
        Get
            Return PrecisionTimeSpan.FromSeconds(Decimal.ToDouble(Me.triggerHoldoffNumeric.Value))
        End Get
    End Property

    Private ReadOnly Property Delay() As PrecisionTimeSpan
        Get
            Return PrecisionTimeSpan.FromSeconds(Decimal.ToDouble(Me.triggerDelayNumeric.Value))
        End Get
    End Property

    Private ReadOnly Property Level() As Double
        Get
            Return Decimal.ToDouble(Me.triggerLevelNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property TriggerCoupling() As ScopeTriggerCoupling
        Get
            Return CType(Me.triggerCouplingComboBox.SelectedItem, ScopeTriggerCoupling)
        End Get
    End Property

    Private ReadOnly Property Slope() As ScopeTriggerSlope
        Get
            Return CType(Me.triggerSlopeComboBox.SelectedItem, ScopeTriggerSlope)
        End Get
    End Property

    Private ReadOnly Property Hysteresis() As Double
        Get
            Return Decimal.ToDouble(Me.hysteresisNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property Mode() As ScopeWindowTriggerMode
        Get
            Return CType(Me.windowModeComboBox.SelectedItem, ScopeWindowTriggerMode)
        End Get
    End Property

    Private ReadOnly Property Low() As Double
        Get
            Return Decimal.ToDouble(Me.lowLevelWindowNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property High() As Double
        Get
            Return Decimal.ToDouble(Me.highLevelWindowNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property EnforceRealTime() As Boolean
        Get
            Return enforceRealtimecheckBox.Checked
        End Get
    End Property

    Private ReadOnly Property EnableTimeInterleavedSampling() As Boolean
        Get
            Return enableTimeInterleavedSamplingCheckBox.Checked
        End Get
    End Property
#End Region

    Private Sub acquireButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles acquireButton.Click
        ConfigureAndStartAcquisition()
    End Sub

    Private Sub mainForm_closing(ByVal obj As Object, ByVal e As FormClosingEventArgs) Handles MyBase.FormClosing
        CloseSession()
    End Sub

    Private Sub stopButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles stopButton.Click
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

    Private Sub ConfigureAndStartAcquisition()
        [stop] = False
        ChangeControlState(False)
        DisplayMessage("Acquisition is in progress...")

        Dim waveforms As AnalogWaveformCollection(Of Double) = Nothing
        Try
            InitializeSession()

            While Not [stop]
                scopeSession.Acquisition.Type = AcquisitionType
                scopeSession.Channels(ChannelName).Configure(Range, Offset, Coupling, ProbeAttenuation, True)
                scopeSession.Channels(ChannelName).ConfigureCharacteristics(InputImpedance, InputFrequencyMax)
                scopeSession.Timing.ConfigureTiming(SampleRateMin, RecordLengthMin, ReferencePosition, NumberOfRecords, EnforceRealTime)
                scopeSession.Channels(ChannelName).EnableTimeInterleavedSampling = EnableTimeInterleavedSampling

                Select Case TriggerType
                    Case ScopeTriggerType.Edge
                        scopeSession.Trigger.EdgeTrigger.Configure(TriggerSource, Level, Slope, TriggerCoupling, HoldOff, Delay)
                        Exit Select
                    Case ScopeTriggerType.Hysteresis
                        scopeSession.Trigger.ConfigureTriggerHysteresis(TriggerSource, Level, Hysteresis, Slope, TriggerCoupling, HoldOff, _
                         Delay)
                        Exit Select
                    Case ScopeTriggerType.Immediate
                        scopeSession.Trigger.ConfigureTriggerImmediate()
                        Exit Select
                    Case ScopeTriggerType.DigitalEdge
                        scopeSession.Trigger.ConfigureTriggerDigital(TriggerSource, Slope, HoldOff, Delay)
                        Exit Select
                    Case ScopeTriggerType.Window
                        scopeSession.Trigger.ConfigureTriggerWindow(TriggerSource, Low, High, Mode, TriggerCoupling, HoldOff, _
                         Delay)
                        Exit Select
                End Select

                scopeSession.Measurement.Initiate()
                waveforms = scopeSession.Channels(ChannelName).Measurement.FetchDouble(Timeout, -1, waveforms)

                PlotWaveforms(sampledDataGridView, waveforms)
                SampleRate = scopeSession.Acquisition.SampleRate.ToString("E")
                RecordLength = scopeSession.Acquisition.RecordLength.ToString()
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
