'==================================================================================================
'
' Title:
'      Basic Measurement Library
'
' Description:
'      This example illustrates fetching scalar measurements from NI-SCOPE, 
'      such as period and rise time calculations. Some common horizontal and vertical 
'      parameters are configured for the digitizer. 
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
        ConfigureTriggerTypeComboBox()
        ConfigureTriggerSlopeComboBox()
        ConfigureTriggerSourceComboBox()
        ConfigureTriggerCouplingComboBox()
        ConfigureScalarMeasurementComboBox()
        LoadScopeDeviceNames()
        ChangeControlState(True)
    End Sub

#Region "Mainform Initial Configuration"
    Private Sub ConfigureTriggerTypeComboBox()
        triggerTypeComboBox.Items.Add(ScopeTriggerType.Edge)
        triggerTypeComboBox.Items.Add(ScopeTriggerType.Immediate)
        triggerTypeComboBox.SelectedIndex = 1
    End Sub

    Private Sub ConfigureTriggerSlopeComboBox()
        For Each value As ScopeTriggerSlope In [Enum].GetValues(GetType(ScopeTriggerSlope))
            triggerSlopeComboBox.Items.Add(value)
        Next
        triggerSlopeComboBox.SelectedIndex = 1
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
        triggerSourceComboBox.SelectedIndex = 0
    End Sub

    Private Sub ConfigureTriggerCouplingComboBox()
        For Each value As ScopeTriggerCoupling In [Enum].GetValues(GetType(ScopeTriggerCoupling))
            triggerCouplingComboBox.Items.Add(value)
        Next
        triggerCouplingComboBox.SelectedIndex = 1
    End Sub

    Private Sub ConfigureScalarMeasurementComboBox()
        For Each value As ScopeScalarMeasurementType In [Enum].GetValues(GetType(ScopeScalarMeasurementType))
            scalarMeasurementComboBox.Items.Add(value)
        Next
        scalarMeasurementComboBox.SelectedIndex = 4
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

    Private ReadOnly Property SampleRateMin() As Double
        Get
            Return Decimal.ToDouble(Me.sampleRateMinNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property RecordLengthMin() As Integer
        Get
            Return Decimal.ToInt32(Me.minRecordLengthNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property TriggerType() As ScopeTriggerType
        Get
            Return CType(Me.triggerTypeComboBox.SelectedItem, ScopeTriggerType)
        End Get
    End Property

    Private ReadOnly Property Timeout() As PrecisionTimeSpan
        Get
            Return PrecisionTimeSpan.FromSeconds(Decimal.ToDouble(Me.maxTimeNumeric.Value))
        End Get
    End Property

    Private ReadOnly Property TriggerSource() As ScopeTriggerSource
        Get
            Return CType(Me.triggerSourceComboBox.SelectedIndex, ScopeTriggerSource)
        End Get
    End Property

    Private ReadOnly Property TriggerLevel() As Double
        Get
            Return Decimal.ToDouble(Me.triggerLevelNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property TriggerSlope() As ScopeTriggerSlope
        Get
            Return CType(Me.triggerSlopeComboBox.SelectedItem, ScopeTriggerSlope)
        End Get
    End Property

    Private ReadOnly Property TriggerCoupling() As ScopeTriggerCoupling
        Get
            Return CType(Me.triggerCouplingComboBox.SelectedItem, ScopeTriggerCoupling)
        End Get
    End Property

    Private ReadOnly Property ScalarMeasurement() As ScopeScalarMeasurementType
        Get
            Return CType(Me.scalarMeasurementComboBox.SelectedItem, ScopeScalarMeasurementType)
        End Get
    End Property

    Private ReadOnly Property LowReference() As Double
        Get
            Return Convert.ToDouble(Me.lowReferenceNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property MiddleReference() As Double
        Get
            Return Convert.ToDouble(Me.middleReferenceNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property HighReference() As Double
        Get
            Return Convert.ToDouble(Me.highReferenceNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property EnforceRealTime() As Boolean
        Get
            Return enforceRealTimeCheckBox.Checked
        End Get
    End Property

    Private WriteOnly Property ScalarResult() As String
        Set(ByVal value As String)
            Me.scalarResultTextBox.Text = value
        End Set
    End Property

    Private WriteOnly Property Mean() As String
        Set(ByVal value As String)
            Me.meanTextBox.Text = value
        End Set
    End Property

    Private WriteOnly Property StandardDeviation() As String
        Set(ByVal value As String)
            Me.standardDeviationTextBox.Text = value
        End Set
    End Property

    Private WriteOnly Property Minimum() As String
        Set(ByVal value As String)
            Me.minimumTextBox.Text = value
        End Set
    End Property

    Private WriteOnly Property Maximum() As String
        Set(ByVal value As String)
            Me.maximumTextBox.Text = value
        End Set
    End Property

    Private WriteOnly Property NumberInStatistics() As String
        Set(ByVal value As String)
            Me.numberInStatsTextBox.Text = value
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

    Private Sub clearStatisticsButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles clearStatisticsButton.Click
        ClearStatistics()
    End Sub

    Private Sub ClearStatistics()
        scopeSession.Channels(ChannelName).Measurement.ClearWaveformMeasurements(ScalarMeasurement)
    End Sub

    Private Sub StartAcquisition()
        [stop] = False
        ChangeControlState(False)

        Dim waveform As AnalogWaveformCollection(Of Double) = Nothing
        Dim measurementStatistics As ScopeScalarMeasurementStatistics() = Nothing
        Dim data As Double() = Nothing
        Try
            InitializeSession()
            scopeSession.Acquisition.Type = ScopeAcquisitionType.Normal

            ' Configure the vertical parameters.
            Dim verticalCoupling As ScopeVerticalCoupling = ScopeVerticalCoupling.DC
            Dim verticalOffset As Double = 0.0
            Dim probeAttenuation As Double = 1.0
            Dim channelEnabled As Boolean = True
            scopeSession.Channels(ChannelName).Configure(VerticalRange, verticalOffset, verticalCoupling, probeAttenuation, channelEnabled)

            ' Configure the horizontal parameters.
            Dim referencePosition As Double = 50.0
            Dim numberOfRecords As Integer = 1
            scopeSession.Timing.ConfigureTiming(SampleRateMin, RecordLengthMin, referencePosition, numberOfRecords, EnforceRealTime)

            ' Configure the trigger.
            If TriggerType = ScopeTriggerType.Edge Then
                Dim holdOff As PrecisionTimeSpan = PrecisionTimeSpan.Zero
                Dim delay As PrecisionTimeSpan = PrecisionTimeSpan.Zero
                scopeSession.Trigger.EdgeTrigger.Configure(TriggerSource, TriggerLevel, TriggerSlope, TriggerCoupling, holdOff, delay)
            End If

            While Not [stop]
                scopeSession.Measurement.Initiate()
                scopeSession.Channels(ChannelName).Measurement.ReferenceLevel.Low = LowReference
                scopeSession.Channels(ChannelName).Measurement.ReferenceLevel.Mid = MiddleReference
                scopeSession.Channels(ChannelName).Measurement.ReferenceLevel.High = HighReference

                ' Acquire data from the NI-Scope device
                ' Get measurement data and statistics.
                waveform = scopeSession.Channels(ChannelName).Measurement.FetchDouble(Timeout, RecordLengthMin, waveform)
                data = scopeSession.Channels(ChannelName).Measurement.FetchScalarMeasurement(Timeout, ScalarMeasurement)
                measurementStatistics = scopeSession.Channels(ChannelName).Measurement.FetchScalarMeasurementStatistics(Timeout, ScalarMeasurement)

                DisplayResults(data, measurementStatistics)
            End While
        Catch ex As Exception
            ShowError(ex)
        Finally
            CloseSession()
            ChangeControlState(True)
        End Try
    End Sub

    Private Sub DisplayResults(ByVal data As Double(), ByVal measurementStatistics As ScopeScalarMeasurementStatistics())
        Dim currentChannelIndex As Integer = 0
        ScalarResult = data(currentChannelIndex).ToString("E")
        Mean = measurementStatistics(currentChannelIndex).Mean.ToString("E")
        StandardDeviation = measurementStatistics(currentChannelIndex).StandardDeviation.ToString("E")
        Maximum = measurementStatistics(currentChannelIndex).Max.ToString("E")
        Minimum = measurementStatistics(currentChannelIndex).Min.ToString("E")
        NumberInStatistics = measurementStatistics(currentChannelIndex).StatisticsCount.ToString()
        Application.DoEvents()
    End Sub

    Private Sub InitializeSession()
        scopeSession = New NIScope(ResourceName, False, False)
        AddHandler scopeSession.DriverOperation.Warning, New EventHandler(Of ScopeWarningEventArgs)(AddressOf DriverOperation_Warning)
    End Sub

    Private Sub DriverOperation_Warning(ByVal sender As Object, ByVal e As ScopeWarningEventArgs)
        MessageBox.Show(e.Text, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Sub

    Private Sub StopAcquisition()
        If Not [stop] Then
            [stop] = True
        End If
    End Sub

    Private Sub ChangeControlState(ByVal isEnabled As Boolean)
        generalGroupBox.Enabled = isEnabled
        timingGroupBox.Enabled = isEnabled
        triggeringGroupBox.Enabled = isEnabled
        edgeTriggerGroupBox.Enabled = isEnabled
        acquireButton.Enabled = isEnabled
        clearStatisticsButton.Enabled = Not isEnabled
        stopButton.Enabled = Not isEnabled
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

    Private Shared Sub ShowError(ByVal ex As Exception)
        MessageBox.Show(ex.Message, ex.[GetType]().ToString(), MessageBoxButtons.OK, MessageBoxIcon.[Error])
    End Sub
End Class
