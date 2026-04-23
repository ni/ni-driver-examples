'=============================================================================================================
'
' Title:
'        OSP Quadrature Downconversion
'
' Description:
'      This example shows how to program a quadrature downconversion acquisition. Digitizers support only a 
'      certain number of defined sample rates with DDC processing enabled, so if the value chosen is not a
'      valid rate, it is rounded to the next higher rate, called the actual sample rate.
'         
'=============================================================================================================

Imports System
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
        ConfigureInputImpedanceComboBox()
        LoadScopeDeviceNames()
        ChangeControlState(True)
    End Sub

#Region "Mainform initial configuration"
    Private Sub ConfigureTriggerTypeComboBox()
        triggerTypeComboBox.Items.Add(ScopeTriggerType.Edge)
        triggerTypeComboBox.Items.Add(ScopeTriggerType.Immediate)
        triggerTypeComboBox.SelectedIndex = 1
    End Sub

    Private Sub ConfigureInputImpedanceComboBox()
        inputImpedanceComboBox.Items.Add(50)
        inputImpedanceComboBox.Items.Add(1000000)
        inputImpedanceComboBox.SelectedIndex = 0
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

    Private ReadOnly Property InputImpedance() As Integer
        Get
            Return CInt(Me.inputImpedanceComboBox.SelectedItem)
        End Get
    End Property

    Private ReadOnly Property VerticalRange() As Double
        Get
            Return Decimal.ToDouble(Me.verticalRangeNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property DigitalGain() As Double
        Get
            Return Decimal.ToDouble(Me.digitalGainNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property CenterFrequency() As Double
        Get
            Return Decimal.ToDouble(Me.centerFrequencyNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property PhaseI() As Double
        Get
            Return Decimal.ToDouble(Me.phaseINumeric.Value)
        End Get
    End Property

    Private ReadOnly Property PhaseQ() As Double
        Get
            Return Decimal.ToDouble(Me.phaseQNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property SampleRateMin() As Double
        Get
            Return Decimal.ToDouble(Me.sampleRateMinNumeric.Value)
        End Get
    End Property

    Private WriteOnly Property ActualSamplerate() As String
        Set(ByVal value As String)
            Me.actualSampleRateTextBox.Text = value
        End Set
    End Property

    Private ReadOnly Property RecordLengthMin() As Integer
        Get
            Return Decimal.ToInt32(Me.recordLengthMinNumeric.Value)
        End Get
    End Property

    Private WriteOnly Property ActualRecordLength() As String
        Set(ByVal value As String)
            Me.actualRecordLengthTextBox.Text = value
        End Set
    End Property

    Private ReadOnly Property FractionalResampleEnabled() As Boolean
        Get
            Return Me.fracResampleEnabledCheckBox.Checked
        End Get
    End Property

    Private ReadOnly Property TriggerType() As ScopeTriggerType
        Get
            Return CType(Me.triggerTypeComboBox.SelectedItem, ScopeTriggerType)
        End Get
    End Property

    Private ReadOnly Property TriggerLevel() As Double
        Get
            Return Decimal.ToDouble(Me.triggerLevelNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property TriggerMinQuietTime() As Double
        Get
            Return Decimal.ToDouble(Me.triggerMinQuietTimeNumeric.Value)
        End Get
    End Property
#End Region

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

    Private Sub DriverOperation_Warning(ByVal sender As Object, ByVal e As ScopeWarningEventArgs)
        messageTextBox.Clear()
        MessageBox.Show(e.Text, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Sub

    Private Sub stopButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles stopButton.Click
        StopAcquisition()
    End Sub

    Private Sub StartAcquisition()
        [stop] = False
        ChangeControlState(False)
        DisplayMessage("Acquisition is in progress...")

        Dim waveforms As AnalogWaveformCollection(Of Double) = Nothing
        Try
            InitializeSession()
            While Not [stop]
                scopeSession.Channels(ChannelName).Range = VerticalRange
                scopeSession.Channels(ChannelName).Enabled = True
                scopeSession.Timing.FractionalResample.Enabled = FractionalResampleEnabled

                Dim inputFrequencyMaximum As Double = -1.0
                scopeSession.Channels(ChannelName).ConfigureCharacteristics(InputImpedance, inputFrequencyMaximum)

                Dim referencePosition As Double = 50.0
                Dim numberOfRecords As Integer = 1
                Dim enforceRealtime As Boolean = True
                scopeSession.Timing.ConfigureTiming(SampleRateMin, RecordLengthMin, referencePosition, numberOfRecords, enforceRealtime)

                scopeSession.Channels(ChannelName).OnboardSignalProcessing.Ddc.Enabled = True
                scopeSession.Channels(ChannelName).OnboardSignalProcessing.Ddc.FrequencyTranslationEnabled = True
                scopeSession.Channels(ChannelName).OnboardSignalProcessing.Ddc.CenterFrequency = CenterFrequency
                scopeSession.Channels(ChannelName).OnboardSignalProcessing.Ddc.FrequencyTranslationPhaseI = PhaseI
                scopeSession.Channels(ChannelName).OnboardSignalProcessing.Ddc.FrequencyTranslationPhaseQ = PhaseQ

                scopeSession.Acquisition.DdcDataProcessingMode = ScopeDdcDataProcessingMode.Complex
                scopeSession.Acquisition.OverflowErrorReportingMode = ScopeOverflowErrorReportingMode.Warning
                scopeSession.Trigger.ReferenceTrigger.DetectorLocation = ScopeReferenceTriggerDetectorLocation.DdcOutput
                scopeSession.Trigger.ReferenceTrigger.QuietTimeMin = PrecisionTimeSpan.FromSeconds(TriggerMinQuietTime)

                If TriggerType = ScopeTriggerType.Immediate Then
                    scopeSession.Trigger.ConfigureTriggerImmediate()
                ElseIf TriggerType = ScopeTriggerType.Edge Then
                    Dim triggerSource As ScopeTriggerSource = ScopeTriggerSource.Channel0
                    Dim triggerHoldoff As PrecisionTimeSpan = PrecisionTimeSpan.Zero
                    Dim triggerDelay As PrecisionTimeSpan = PrecisionTimeSpan.Zero
                    Dim triggerSlope As ScopeTriggerSlope = ScopeTriggerSlope.Positive
                    Dim triggerCoupling As ScopeTriggerCoupling = ScopeTriggerCoupling.DC
                    scopeSession.Trigger.EdgeTrigger.Configure(triggerSource, TriggerLevel, triggerSlope, triggerCoupling, triggerHoldoff, triggerDelay)
                End If

                scopeSession.Measurement.FetchInterleavedIQData = False
                scopeSession.Measurement.Initiate()

                Dim actualRecLength As Long = scopeSession.Acquisition.RecordLength
                waveforms = scopeSession.Channels(ChannelName).Measurement.FetchDouble(Timeout, actualRecLength, waveforms)

                PlotWaveforms(acquisitionDataGridView, waveforms)
                ActualRecordLength = scopeSession.Acquisition.RecordLength.ToString()
                ActualSamplerate = scopeSession.Acquisition.SampleRate.ToString("E")
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
        acquisitionDataGridView.Columns.Clear()
    End Sub

    Private Sub StopAcquisition()
        If Not [stop] Then
            DisplayMessage("Stop in progress...Fetched points are being plotted...")
            [stop] = True
        End If
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
