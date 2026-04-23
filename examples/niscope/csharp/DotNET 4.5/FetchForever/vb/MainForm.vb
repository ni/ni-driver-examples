'=====================================================================================================
'
' Title:
'        Fetch Forever
'
' Description:
'      The application demonstrates how to fetch data records continuously from a NI-SCOPE device.
'      It uses a Memory Optimized asynchronous version of Fetch. The program continues fetching 
'      until the stop button is pressed or an exception is thrown.
'
'==================================================================================================

Imports System.Windows.Forms
Imports System.Data
Imports NationalInstruments.ModularInstruments.NIScope
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices
Imports System.Text.RegularExpressions

Partial Public Class MainForm
    Inherits Form
    Private scopeSession As NIScope
    Private [stop] As Boolean

    Public Sub New()
        InitializeComponent()
        LoadScopeDeviceNames()
        ChangeControlState(True)
    End Sub

#Region "Mainform initial configuration"
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
            Dim val As Integer
            Dim unqualifiedChannelName As String
            'Strip off the device name from the fully qualified channel name (i.e. convert Dev1/0 to 0)
            unqualifiedChannelName = Regex.Replace(Me.channelNameTextBox.Text, "[^,]+/", "")
            'Check the unqualified channel name to verify it is a single channel with only digits
            If Not Int32.TryParse(unqualifiedChannelName, val) Then
                Throw New ArgumentException("The channel name specified is either invalid or multiple channels are specified." & vbLf & vbCr & vbLf & vbCr & "This example supports only 1 channel.")
            End If
            Return Me.channelNameTextBox.Text
        End Get
    End Property

    Private ReadOnly Property SampleRateMin() As Double
        Get
            Return Decimal.ToDouble(Me.minSampleRateNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property Range() As Double
        Get
            Return Decimal.ToDouble(Me.verticalRangeNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property PointsToFetchMax() As Long
        Get
            Return Decimal.ToInt64(Me.maxPointsFetchedNumeric.Value)
        End Get
    End Property

    Private WriteOnly Property TotalPointsFetched() As String
        Set(value As String)
            Me.totalPointsFetchedTextBox.Text = value
        End Set
    End Property

    Private WriteOnly Property LastFetchPoints() As String
        Set(value As String)
            Me.lastFetchedPointsTextBox.Text = value
        End Set
    End Property
#End Region

    Private Sub acquireButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles acquireButton.Click
        FetchForever()
    End Sub

    Private Sub FetchForever()
        [stop] = False
        ChangeControlState(False)
        DisplayMessage("Acquisition is in progress...")

        Try
            InitializeSession()

            ' Configure the vertical parameters.
            Dim offset As Double = 0.0
            Dim coupling As ScopeVerticalCoupling = ScopeVerticalCoupling.DC
            Dim probeAttenuation As Double = 1.0
            scopeSession.Channels(ChannelName).Configure(Range, offset, coupling, probeAttenuation, True)

            ' Configure the horizontal parameters.
            Dim recordLengthMin As Integer = 1
            Dim referencePosition As Double = 0.0
            Dim numberOfRecords As Integer = 1
            Dim enforceRealtime As Boolean = True
            scopeSession.Timing.ConfigureTiming(SampleRateMin, recordLengthMin, referencePosition, numberOfRecords, enforceRealtime)

            ' Configure software trigger, but never send the trigger.
            ' This starts an infinite acquisition, until you call niScope_Abort or niScope_close.
            scopeSession.Trigger.ConfigureTriggerSoftware(PrecisionTimeSpan.Zero, PrecisionTimeSpan.Zero)
            scopeSession.Measurement.Initiate()

            Dim totalPointsFetched As Long = 0
            scopeSession.Measurement.FetchRelativeTo = ScopeFetchRelativeTo.ReadPointer
            While Not [stop]
                Dim waveforms As AnalogWaveformCollection(Of Double) = Nothing
                Dim waveformInfo As ScopeWaveformInfo() = Nothing
                waveforms = scopeSession.Channels(ChannelName).Measurement.FetchDouble(PrecisionTimeSpan.Zero, PointsToFetchMax, waveforms, waveformInfo)
                totalPointsFetched += waveformInfo(0).ActualNumberOfSamples
                UpdateOutputResults(totalPointsFetched, waveformInfo(0).ActualNumberOfSamples)
                PlotWaveforms(waveformDataGridView, waveforms(0), waveformInfo(0).ActualNumberOfSamples)
                Application.DoEvents()
            End While
            DisplayMessage("Acquisition successful!!!")
        Catch ex As Exception
            ShowError(ex)
        Finally
            ChangeControlState(True)
            CloseSession()
        End Try
    End Sub

    Private Sub ClearWaveforms()
        waveformDataGridView.Columns.Clear()
    End Sub

    Private Shared Sub PlotWaveforms(dgv As DataGridView, waveform As AnalogWaveform(Of Double), actualNumberOfSamples As Long)

        Dim table As DataTable = New DataTable()
        table.Columns.Add("Index", GetType(Integer))
        table.Columns.Add("Waveforms", GetType(Double))

        For idx As Long = 0 To actualNumberOfSamples - 1
            table.Rows.Add(idx + 1, waveform.Samples(idx).Value)
        Next
        dgv.DataSource = table
    End Sub

    Private Sub UpdateOutputResults(totalPointsFetched__1 As Long, actualNumberOfSamples As Long)
        TotalPointsFetched = totalPointsFetched__1.ToString()
        LastFetchPoints = actualNumberOfSamples.ToString()
    End Sub

    Private Sub mainForm_Closing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles MyBase.FormClosing
        CloseSession()
    End Sub

    Private Sub stopButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles stopButton.Click
        If Not [stop] Then
            [stop] = True
            DisplayMessage("Stop in progress...Fetched points are being plotted...")
        End If
    End Sub

    Private Sub DisplayMessage(message As String)
        messageTextBox.Text = message
        Me.Refresh()
    End Sub

    Private Sub InitializeSession()
        scopeSession = New NIScope(ResourceName, False, False)
        AddHandler scopeSession.DriverOperation.Warning, New EventHandler(Of ScopeWarningEventArgs)(AddressOf DriverOperation_Warning)
    End Sub

    Private Sub DriverOperation_Warning(sender As Object, e As ScopeWarningEventArgs)
        messageTextBox.Clear()
        MessageBox.Show(e.Text, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Sub

    Private Sub ChangeControlState(isEnabled As Boolean)
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

    Private Sub ShowError(ex As Exception)
        messageTextBox.Clear()
        MessageBox.Show(ex.Message, ex.[GetType]().ToString(), MessageBoxButtons.OK, MessageBoxIcon.[Error])
    End Sub
End Class
