'==================================================================================================
' Title        : DMM Switch Synchronous Scanning
' Description  : Use this example to learn how to work with NI-Switch and NI-DMM together, The example,
'first configures scanning operation for switch, then measurement configurations for Dmm is done, 
'Finally, the Dmm device does a fetch operation after the switch scan is performed.           
'==================================================================================================
Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Reflection
Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments.NIDmm
Imports NationalInstruments.ModularInstruments.NISwitch
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices


Partial Public Class MainForm
    Inherits Form
    Private dmmSession As NIDmm
    Private switchSession As NISwitch
    Private dmmSamplesArray As Double()
    Const MillisecondsToWait As Long = 5000

    Public Sub New()
        InitializeComponent()
        LoadSwitchDeviceNames()
        LoadTopology()
        LoadDmmDeviceNames()
        LoadMeasurementModes()
        LoadSwitchTriggerInput()
        LoadDmmMeasCompleteDest()
    End Sub
#Region "UI Initial Value Config Section"
    Private Sub LoadTopology()
        Dim myType As Type = GetType(SwitchDeviceTopology)
        Dim properties As PropertyInfo() = myType.GetProperties()

        For Each prop As PropertyInfo In properties
            topologyNameComboBox.Items.Add(prop.GetValue(myType, Nothing).ToString())
        Next
        topologyNameComboBox.SelectedIndex = 0
    End Sub
    Private Sub LoadSwitchDeviceNames()
        Dim modularInstrumentsSystem As New ModularInstrumentsSystem("NI-SWITCH")
        For Each device As DeviceInfo In modularInstrumentsSystem.DeviceCollection
            switchResourceNameComboBox.Items.Add(device.Name)
        Next
        If modularInstrumentsSystem.DeviceCollection.Count > 0 Then
            switchResourceNameComboBox.SelectedIndex = 0
        End If
    End Sub
    Private Sub LoadDmmDeviceNames()
        Dim modularInstrumentsSystem As New ModularInstrumentsSystem("NI-DMM")
        For Each device As DeviceInfo In modularInstrumentsSystem.DeviceCollection
            dmmResourceNameComboBox.Items.Add(device.Name)
        Next
        If modularInstrumentsSystem.DeviceCollection.Count > 0 Then
            dmmResourceNameComboBox.SelectedIndex = 0
        End If
    End Sub
    Private Sub LoadMeasurementModes()
        measurementModeComboBox.Items.AddRange([Enum].GetNames(GetType(DmmMeasurementFunction)))
        measurementModeComboBox.Items.Remove(DmmMeasurementFunction.WaveformCurrent.ToString())
        measurementModeComboBox.Items.Remove(DmmMeasurementFunction.WaveformVoltage.ToString())
        measurementModeComboBox.SelectedIndex = 0
    End Sub
    Private Sub LoadSwitchTriggerInput()
        Dim myType As Type = GetType(SwitchScanTriggerInput)
        Dim properties As PropertyInfo() = myType.GetProperties()

        For Each prop As PropertyInfo In properties
            switchTriggerInputComboBox.Items.Add(prop.GetValue(myType, Nothing).ToString())
        Next
        switchTriggerInputComboBox.SelectedIndex = 12
    End Sub

    Private Sub LoadDmmMeasCompleteDest()
        Dim dmmMeasCompleteDestList As List(Of KeyValuePair(Of String, DmmMeasurementCompleteDestination)) = New List(Of KeyValuePair(Of String, DmmMeasurementCompleteDestination))()
        dmmMeasCompleteDestList.Add(New KeyValuePair(Of String, DmmMeasurementCompleteDestination)("None", DmmMeasurementCompleteDestination.None))
        dmmMeasCompleteDestList.Add(New KeyValuePair(Of String, DmmMeasurementCompleteDestination)("External", DmmMeasurementCompleteDestination.External))
        dmmMeasCompleteDestList.Add(New KeyValuePair(Of String, DmmMeasurementCompleteDestination)("TTL 0", DmmMeasurementCompleteDestination.Ttl0))
        dmmMeasCompleteDestList.Add(New KeyValuePair(Of String, DmmMeasurementCompleteDestination)("TTL 1", DmmMeasurementCompleteDestination.Ttl1))
        dmmMeasCompleteDestList.Add(New KeyValuePair(Of String, DmmMeasurementCompleteDestination)("TTL 2", DmmMeasurementCompleteDestination.Ttl2))
        dmmMeasCompleteDestList.Add(New KeyValuePair(Of String, DmmMeasurementCompleteDestination)("TTL 3", DmmMeasurementCompleteDestination.Ttl3))
        dmmMeasCompleteDestList.Add(New KeyValuePair(Of String, DmmMeasurementCompleteDestination)("TTL 4", DmmMeasurementCompleteDestination.Ttl4))
        dmmMeasCompleteDestList.Add(New KeyValuePair(Of String, DmmMeasurementCompleteDestination)("TTL 5", DmmMeasurementCompleteDestination.Ttl5))
        dmmMeasCompleteDestList.Add(New KeyValuePair(Of String, DmmMeasurementCompleteDestination)("TTL 6", DmmMeasurementCompleteDestination.Ttl6))
        dmmMeasCompleteDestList.Add(New KeyValuePair(Of String, DmmMeasurementCompleteDestination)("TTL 7", DmmMeasurementCompleteDestination.Ttl7))
        dmmMeasCompleteDestList.Add(New KeyValuePair(Of String, DmmMeasurementCompleteDestination)("LBR Trig 0 7", DmmMeasurementCompleteDestination.LbrTrig0))

        measCompleteDestComboBox.DisplayMember = "Key"
        measCompleteDestComboBox.ValueMember = "Value"
        measCompleteDestComboBox.DataSource = dmmMeasCompleteDestList
        measCompleteDestComboBox.SelectedIndex = 1
    End Sub
#End Region

#Region "Program Properties"
    Private ReadOnly Property SwitchResourceName() As String
        Get
            Return Me.switchResourceNameComboBox.Text
        End Get
    End Property
    Private ReadOnly Property DmmResourceName() As String
        Get
            Return Me.dmmResourceNameComboBox.Text
        End Get
    End Property
    Private ReadOnly Property TopologyName() As String
        Get
            Return Me.topologyNameComboBox.SelectedItem.ToString()
        End Get
    End Property
    Private ReadOnly Property MeasurementMode() As String
        Get
            Return Me.measurementModeComboBox.Text
        End Get
    End Property
    Private ReadOnly Property ScanList() As String
        Get
            Return Me.scanListTextBox.Text
        End Get
    End Property
    Private ReadOnly Property MeasCompleteDestination() As DmmMeasurementCompleteDestination
        Get
            Return DirectCast(Me.measCompleteDestComboBox.SelectedValue, DmmMeasurementCompleteDestination)
        End Get
    End Property
    Private ReadOnly Property MeasurementRange() As Double
        Get
            Return CDbl(Me.rangeNumericUpDown.Value)
        End Get
    End Property
    Private ReadOnly Property MeasurementResolution() As Double
        Get
            Return CDbl(Me.resolutionNumericUpDown.Value)
        End Get
    End Property
    Private ReadOnly Property SamplesToFetch() As Integer
        Get
            Return CInt(Me.samplesToFetchNumericUpDown.Value)
        End Get
    End Property
    Private ReadOnly Property SwitchTriggerInput() As String
        Get
            Return Me.switchTriggerInputComboBox.Text
        End Get
    End Property
    Private ReadOnly Property SampleInterval() As Double
        Get
            Return CDbl(Me.sampleIntervalNumericUpDown.Value)
        End Get
    End Property
#End Region
#Region "Program Functions"
    Private Sub InitializeDmmSession()
        'Open a session to the DMM.
        dmmSession = New NIDmm(DmmResourceName, True, True)
    End Sub
    Private Sub InitializeSwitchSession()
        'Open a session to the switch module and set the topology.
        switchSession = New NISwitch(SwitchResourceName, TopologyName, False, True)
        AddHandler switchSession.DriverOperation.Warning, New System.EventHandler(Of SwitchWarningEventArgs)(AddressOf DriverOperationWarning)
    End Sub
    Private Sub DriverOperationWarning(ByVal sender As Object, ByVal e As SwitchWarningEventArgs)
        MessageBox.Show(e.ToString(), "Warning")
    End Sub
    Private Sub CloseSwitchSession()
        If switchSession IsNot Nothing Then
            Try
                switchSession.Close()
                switchSession = Nothing
            Catch ex As System.Exception
                ShowError("Unable to Close Session, Reset the device." & vbLf + "Error : " + ex.Message)
                Application.[Exit]()
            End Try
        End If
    End Sub
    Private Sub CloseDmmSession()
        If dmmSession IsNot Nothing Then
            Try
                dmmSession.Close()
                dmmSession = Nothing
            Catch ex As System.Exception
                ShowError("Unable to Close Session, Reset the device." & vbLf + "Error : " + ex.Message)
                Application.[Exit]()
            End Try
        End If


    End Sub
    Private Shared Sub ShowError(ByVal message As String)
        If String.IsNullOrEmpty(message) Then
            message = "Unexpected Error"
        End If
        MessageBox.Show(message, "Error")


    End Sub
    Private Sub ChangeControlState(ByVal isEnabled As Boolean)
        Me.startButton.Enabled = isEnabled
        Me.dmmResourceNameComboBox.Enabled = isEnabled
        Me.switchResourceNameComboBox.Enabled = isEnabled
        Me.topologyNameComboBox.Enabled = isEnabled
        Me.measurementModeComboBox.Enabled = isEnabled
        Me.scanListTextBox.Enabled = isEnabled
        Me.switchTriggerInputComboBox.Enabled = isEnabled
        Me.rangeNumericUpDown.Enabled = isEnabled
        Me.resolutionNumericUpDown.Enabled = isEnabled
        Me.samplesToFetchNumericUpDown.Enabled = isEnabled
        Me.measCompleteDestComboBox.Enabled = isEnabled
    End Sub
    Private Sub ConfigureDmmMeasurement()
        ConfigureMeasurement()
        ConfigureDmmMultiPoint()
        ConfigureDmmMeasurementComplete()
    End Sub
    Private Sub ConfigureDmmMeasurementComplete()
        'Configures the destination of the DMM output trigger (Measurement Complete).
        'This should match the switch module input trigger.
        dmmSession.Trigger.MeasurementCompleteDestination = MeasCompleteDestination

        'Configures the slope of the DMM output trigger.
        dmmSession.Trigger.MeasurementCompleteDestinationSlope = DmmSlope.Negative
    End Sub
    Private Sub ConfigureDmmMultiPoint()
        'Configure a multipoint acquisition.
        dmmSession.Trigger.MultiPoint.Configure(1, SamplesToFetch, DmmSampleTrigger.Interval, New PrecisionTimeSpan(SampleInterval))
    End Sub
    Private Sub ConfigureMeasurement()
        'Configure the function, range, and resolution of the measurement.
        dmmSession.Configure(DirectCast([Enum].Parse(GetType(DmmMeasurementFunction), MeasurementMode), DmmMeasurementFunction), MeasurementRange, MeasurementResolution)

    End Sub
    Private Sub AbortSwitchScan()
        '''/Halts the scan.
        switchSession.Scan.Abort()

    End Sub
    Private Sub InitiateDmmMeasurement()
        'Initiate the DMM measurement.
        dmmSession.Measurement.Initiate()
    End Sub
    Private Sub InitiateSwitchScan()
        'Initiates the scan usings the configured scan list and triggers.
        switchSession.Scan.Initiate()
    End Sub

    Private Sub ConfigureSwitchScan()


        'Configures the input trigger of the switch module. This should match the
        'output trigger of the DMM.
        switchSession.Scan.ConfigureTrigger(New PrecisionTimeSpan(0.0), SwitchTriggerInput, SwitchScanAdvancedOutput.None)

        'Configures the switch to loop continuously through the scan list until
        'niSwitch_Abort is called.
        switchSession.Scan.Continuous = True

        'Configures the switch module for scanning. 
        switchSession.Scan.ConfigureList(ScanList, SwitchScanMode.BreakBeforeMake)


    End Sub


#End Region
#Region "FormEvents"
    Private Sub MainForm_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles MyBase.FormClosing
        CloseDmmSession()
        CloseSwitchSession()
    End Sub


    Private Sub startButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles startButton.Click
        ChangeControlState(False)
        Try
            'Programming the Switch
            CloseSwitchSession()
            InitializeSwitchSession()
            ConfigureSwitchScan()


            'Programming the DMM
            CloseDmmSession()

            InitializeDmmSession()
            ConfigureDmmMeasurement()

            InitiateSwitchScan()

            Dim isWaitingForTrigger As Boolean = switchSession.Scan.IsWaitingForTrigger

            Dim sw As New Stopwatch()
            sw.Start()

            While isWaitingForTrigger
                If sw.ElapsedMilliseconds > MillisecondsToWait Then
                    Exit While
                End If
                isWaitingForTrigger = switchSession.Scan.IsWaitingForTrigger
            End While

            InitiateDmmMeasurement()

            'Download data from the DMM.
            dmmSamplesArray = dmmSession.Measurement.FetchMultiPoint(SamplesToFetch)

            AbortSwitchScan()
            Me.dataGridViewResults.Rows.Clear()
            Me.dataGridViewResults.Rows.Add(dmmSamplesArray.Length - 1)
            For i As Integer = 0 To dmmSamplesArray.Length - 1
                Me.dataGridViewResults.Rows(i).Cells(0).Value = i + 1
                Me.dataGridViewResults.Rows(i).Cells(1).Value = dmmSamplesArray(i)
            Next
        Catch ex As Exception
            ShowError(ex.Message)
        Finally
            ChangeControlState(True)
            CloseDmmSession()
            CloseSwitchSession()
        End Try

    End Sub
#End Region

End Class


