'=============================================================================================================
'
' Title:
'       Scope Synchronization Using TClock
'
' Description:
'       This application demonstrates how to synchronize two NI-SCOPE devices. The first scope device is 
'       triggered by an analog edge or is triggered immediately. The second scope device is synchronized 
'       with the first device. Data is fetched from both the devices and displayed on datagrids.
'
'=============================================================================================================

Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments
Imports NationalInstruments.ModularInstruments.NIScope
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices
Imports NationalInstruments.ModularInstruments.SystemServices.TimingServices

Partial Public Class MainForm
    Inherits Form
    Private scopeSession1 As NIScope
    Private scopeSession2 As NIScope
    Private tClockSession As TClock

    Public Sub New()
        InitializeComponent()
        ConfigureTriggerTypeComboBox()
        ConfigureTriggerSourceComboBox()
        LoadScopeDeviceNames()
        ChangeControlState(True)
    End Sub

#Region "Mainform initial configuration"
    Private Sub ConfigureTriggerTypeComboBox()
        triggerTypeComboBox.Items.Add(ScopeTriggerType.Edge)
        triggerTypeComboBox.Items.Add(ScopeTriggerType.Immediate)
        triggerTypeComboBox.SelectedIndex = 0
    End Sub

    Private Sub ConfigureTriggerSourceComboBox()
        triggerSourceComboBox.Items.Add(ScopeTriggerSource.Channel0)
        triggerSourceComboBox.Items.Add(ScopeTriggerSource.Channel1)
        triggerSourceComboBox.Items.Add(ScopeTriggerSource.FromString("TRIG"))
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

    Private Sub LoadScopeDeviceNames()
        Using scopeDevices As New ModularInstrumentsSystem("NI-Scope")
            For Each device As DeviceInfo In scopeDevices.DeviceCollection
                resourceNameDevice1ComboBox.Items.Add(device.Name)
                resourceNameDevice2ComboBox.Items.Add(device.Name)
            Next
        End Using
        If resourceNameDevice1ComboBox.Items.Count > 0 Then
            resourceNameDevice1ComboBox.SelectedIndex = 0
            resourceNameDevice2ComboBox.SelectedIndex = If(resourceNameDevice1ComboBox.Items.Count > 1, 1, 0)
        End If
    End Sub
#End Region

#Region "Mainform configuration values"
    Private ReadOnly Property ResourceName1() As String
        Get
            Return Me.resourceNameDevice1ComboBox.Text
        End Get
    End Property

    Private ReadOnly Property ChannelName1() As String
        Get
            Return Me.channelNameDevice1TextBox.Text
        End Get
    End Property

    Private ReadOnly Property ResourceName2() As String
        Get
            Return Me.resourceNameDevice2ComboBox.Text
        End Get
    End Property

    Private ReadOnly Property ChannelName2() As String
        Get
            Return Me.channelNameDevice2TextBox.Text
        End Get
    End Property

    Private ReadOnly Property VerticalRange() As Double
        Get
            Return Decimal.ToDouble(Me.verticalRangeNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property SampleRateMin() As Double
        Get
            Return Decimal.ToDouble(Me.sampleRateMinNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property RecordLengthMin() As Integer
        Get
            Return Decimal.ToInt32(Me.recordLengthMinNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property InputFrequencyMax() As Double
        Get
            Return Decimal.ToDouble(Me.maximumInputFrequencyNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property TriggerType() As ScopeTriggerType
        Get
            Return CType(Me.triggerTypeComboBox.SelectedItem, ScopeTriggerType)
        End Get
    End Property

    Private ReadOnly Property TriggerSource() As ScopeTriggerSource
        Get
            Return CType(Me.triggerSourceComboBox.SelectedItem, ScopeTriggerSource)
        End Get
    End Property

    Private ReadOnly Property TriggerLevel() As Double
        Get
            Return Decimal.ToDouble(Me.triggerLevelNumeric.Value)
        End Get
    End Property
#End Region

    Private Sub acquireButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles acquireButton.Click
        StartAcquisition()
    End Sub

    Private Sub mainForm_Closing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles MyBase.FormClosing
        CloseSession()
    End Sub

    Private Sub IntializeSession()
        scopeSession1 = New NIScope(ResourceName1, False, False)
        AddHandler scopeSession1.DriverOperation.Warning, New EventHandler(Of ScopeWarningEventArgs)(AddressOf DriverOperation_Warning)

        scopeSession2 = New NIScope(ResourceName2, False, False)
        AddHandler scopeSession2.DriverOperation.Warning, New EventHandler(Of ScopeWarningEventArgs)(AddressOf DriverOperation_Warning)

        Dim scopeSynchronizableDevices As ITClockSynchronizableDevice() = New ITClockSynchronizableDevice(1) {scopeSession1, scopeSession2}
        tClockSession = New TClock(scopeSynchronizableDevices)
    End Sub

    Private Sub DriverOperation_Warning(ByVal sender As Object, ByVal e As ScopeWarningEventArgs)
        MessageBox.Show(e.Text, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Sub

    Private Sub StartAcquisition()
        ChangeControlState(False)

        Dim scope1Waveform As AnalogWaveformCollection(Of Double) = Nothing
        Dim scope2Waveform As AnalogWaveformCollection(Of Double) = Nothing
        Try
            IntializeSession()

            ' Configure Vertical and Horizontal parameters on both the devices.
            scopeSession1.Channels(ChannelName1).Enabled = True
            scopeSession2.Channels(ChannelName2).Enabled = True

            scopeSession1.Channels(ChannelName1).Range = VerticalRange
            scopeSession2.Channels(ChannelName2).Range = VerticalRange

            scopeSession1.Acquisition.SampleRateMin = SampleRateMin
            scopeSession2.Acquisition.SampleRateMin = SampleRateMin

            scopeSession1.Acquisition.NumberOfPointsMin = RecordLengthMin
            scopeSession2.Acquisition.NumberOfPointsMin = RecordLengthMin

            scopeSession1.Channels(ChannelName1).InputFrequencyMax = InputFrequencyMax
            scopeSession2.Channels(ChannelName2).InputFrequencyMax = InputFrequencyMax

            ' NI-TClock does not support Random Interleaved Sampling ( RIS ).
            scopeSession1.Timing.EnforceRealtime = True
            scopeSession2.Timing.EnforceRealtime = True

            ' Configure Triggering on the first NI-SCOPE device.
            scopeSession1.Trigger.Type = TriggerType
            If scopeSession1.Trigger.Type = ScopeTriggerType.Edge Then
                scopeSession1.Trigger.Level = TriggerLevel
                scopeSession1.Trigger.Source = TriggerSource
            End If

            tClockSession.ConfigureForHomogeneousTriggers()
            tClockSession.Synchronize()
            tClockSession.Initiate()

            Dim recordLength1 As Long = scopeSession1.Acquisition.RecordLength
            Dim recordLength2 As Long = scopeSession2.Acquisition.RecordLength

            Dim timeout As New PrecisionTimeSpan(5.0)
            scope1Waveform = scopeSession1.Channels(ChannelName1).Measurement.FetchDouble(timeout, recordLength1, scope1Waveform)
            scope2Waveform = scopeSession2.Channels(ChannelName2).Measurement.FetchDouble(timeout, recordLength2, scope2Waveform)

            PlotWaveforms(scope1DataGridView, scope1Waveform)
            PlotWaveforms(scope2DataGridView, scope2Waveform)
        Catch ex As Exception
            ShowError(ex)
        Finally
            CloseSession()
            ChangeControlState(True)
        End Try
    End Sub

    Private Sub ClearWaveforms()
        scope1DataGridView.Columns.Clear()
        scope2DataGridView.Columns.Clear()
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

    Private Sub ChangeControlState(ByVal isEnabled As Boolean)
        scopeDevice1GroupBox.Enabled = isEnabled
        scopeDevice2GroupBox.Enabled = isEnabled
        commonConfigurationGroupBox.Enabled = isEnabled
        triggeringGroupBox.Enabled = isEnabled
        acquireButton.Enabled = isEnabled
        If Not isEnabled Then
            ClearWaveforms()
        End If
        Me.Refresh()
    End Sub

    Private Sub CloseSession()
        Try
            If scopeSession1 IsNot Nothing Then
                scopeSession1.Close()
                scopeSession1 = Nothing
            End If
            If scopeSession2 IsNot Nothing Then
                scopeSession2.Close()
                scopeSession2 = Nothing
            End If
        Catch ex As Exception
            ShowError(ex)
            Application.[Exit]()
        End Try
        tClockSession = Nothing
    End Sub

    Private Shared Sub ShowError(ByVal ex As Exception)
        MessageBox.Show(ex.Message, ex.[GetType]().ToString(), MessageBoxButtons.OK, MessageBoxIcon.[Error])
    End Sub
End Class
