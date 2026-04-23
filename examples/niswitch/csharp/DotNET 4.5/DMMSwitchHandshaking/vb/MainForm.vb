'==================================================================================================
' Title        : 	 DMM Switch Handshaking
' Description  : Use this example to use NI-Switch and Ni-Dmm together. The example, first configures scanning operation for switch, 
'then measurement configurations for Dmm is done, Finally, the Dmm device takes the reading after the switch scan is performed.
'==================================================================================================
Imports System.Reflection
Imports System.Threading
Imports System.Windows.Forms
Imports System.Collections.Generic
Imports NationalInstruments.ModularInstruments.NIDmm
Imports NationalInstruments.ModularInstruments.NISwitch
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices



Partial Public Class MainForm
    Inherits Form
    Private dmmSession As NIDmm
    Private switchSession As NISwitch

    Private dmmInstrumentModel As String
    Private workerThread As Thread
    Private threadStop As Boolean
    Private closeRequested As Boolean
    Private lockobj As New Object()
    Private acquisitionBacklog As Integer
    Private measurement As Double()
    Public Sub New()
        InitializeComponent()
        LoadSwitchDeviceNames()
        LoadTopology()
        LoadDmmDeviceNames()
        LoadMeasurementModes()
        LoadSwitchTriggerInput()
        LoadScanAdvancedOutput()
        LoadDmmTriggerSource()
        LoadDmmMeasCompleteDest()
    End Sub
#Region "UI Initial Value Config Section"
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
    Private Sub LoadDmmTriggerSource()
        Dim triggerSourceValueList As List(Of KeyValuePair(Of String, DmmTriggerSource)) = New List(Of KeyValuePair(Of String, DmmTriggerSource))()
        triggerSourceValueList.Add(New KeyValuePair(Of String, DmmTriggerSource)("Immediate", DmmTriggerSource.Immediate))
        triggerSourceValueList.Add(New KeyValuePair(Of String, DmmTriggerSource)("External", DmmTriggerSource.External))
        triggerSourceValueList.Add(New KeyValuePair(Of String, DmmTriggerSource)("TTL 0", DmmTriggerSource.Ttl0))
        triggerSourceValueList.Add(New KeyValuePair(Of String, DmmTriggerSource)("TTL 1", DmmTriggerSource.Ttl1))
        triggerSourceValueList.Add(New KeyValuePair(Of String, DmmTriggerSource)("TTL 2", DmmTriggerSource.Ttl2))
        triggerSourceValueList.Add(New KeyValuePair(Of String, DmmTriggerSource)("TTL 3", DmmTriggerSource.Ttl3))
        triggerSourceValueList.Add(New KeyValuePair(Of String, DmmTriggerSource)("TTL 4", DmmTriggerSource.Ttl4))
        triggerSourceValueList.Add(New KeyValuePair(Of String, DmmTriggerSource)("TTL 5", DmmTriggerSource.Ttl5))
        triggerSourceValueList.Add(New KeyValuePair(Of String, DmmTriggerSource)("TTL 6", DmmTriggerSource.Ttl6))
        triggerSourceValueList.Add(New KeyValuePair(Of String, DmmTriggerSource)("Software", DmmTriggerSource.SoftwareTrigger))
        triggerSourceValueList.Add(New KeyValuePair(Of String, DmmTriggerSource)("TTL 7", DmmTriggerSource.Ttl7))
        triggerSourceValueList.Add(New KeyValuePair(Of String, DmmTriggerSource)("PXI Star", DmmTriggerSource.PxiStar))
        triggerSourceValueList.Add(New KeyValuePair(Of String, DmmTriggerSource)("LBR Trig 1", DmmTriggerSource.LbrTrig1))
        triggerSourceValueList.Add(New KeyValuePair(Of String, DmmTriggerSource)("Aux Trig 1", DmmTriggerSource.LbrTrig1))

        triggerSourceComboBox.DisplayMember = "Key"
        triggerSourceComboBox.ValueMember = "Value"
        triggerSourceComboBox.DataSource = triggerSourceValueList
        triggerSourceComboBox.SelectedIndex = 1
    End Sub
    Private Sub LoadSwitchTriggerInput()
        Dim myType As Type = GetType(SwitchScanTriggerInput)
        Dim properties As PropertyInfo() = myType.GetProperties()

        For Each prop As PropertyInfo In properties
            switchTriggerInputComboBox.Items.Add(prop.GetValue(myType, Nothing).ToString())
        Next
        switchTriggerInputComboBox.SelectedIndex = 12
    End Sub
    Private Sub LoadScanAdvancedOutput()

        Dim myType As Type = GetType(SwitchScanAdvancedOutput)
        Dim properties As PropertyInfo() = myType.GetProperties()

        For Each prop As PropertyInfo In properties
            scanAdvancedOutputComboBox.Items.Add(prop.GetValue(myType, Nothing).ToString())
        Next
        scanAdvancedOutputComboBox.SelectedIndex = 1
    End Sub
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
    Private ReadOnly Property SwitchTriggerInput() As String
        Get
            Return Me.switchTriggerInputComboBox.Text
        End Get
    End Property
    Private ReadOnly Property ScanAdvancedOutput() As String
        Get
            Return Me.scanAdvancedOutputComboBox.Text
        End Get
    End Property
    Private ReadOnly Property TriggerSource() As DmmTriggerSource
        Get
            Return DirectCast(Me.triggerSourceComboBox.SelectedValue, DmmTriggerSource)
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
#End Region
#Region "Program Functions"
    Private Sub ThreadTerminate()

        'Waits for the thread To Terminate.
        workerThread.Join()
        workerThread = Nothing
        If closeRequested = True Then
            Me.Close()
        End If
    End Sub

    Private Sub UpdateData(ByVal temp As Double())
        Dim rowsAlreadyAdded As Integer = dataGridViewResults.Rows.Count
        Dim totalRowsAfterEntry As Integer = temp.Length + rowsAlreadyAdded - 1
        Me.dataGridViewResults.Rows.Add(totalRowsAfterEntry - rowsAlreadyAdded)
        For i As Integer = rowsAlreadyAdded - 1 To totalRowsAfterEntry - 1
            Me.dataGridViewResults.Rows(i).Cells(0).Value = i + 1
            Me.dataGridViewResults.Rows(i).Cells(1).Value = temp(i - rowsAlreadyAdded + 1)
        Next

    End Sub
    Private Property ThreadStopMarker() As Boolean
        Get
            SyncLock lockobj
                Return threadStop
            End SyncLock
        End Get
        Set(ByVal value As Boolean)
            SyncLock lockobj
                threadStop = value
            End SyncLock
        End Set
    End Property
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
        Me.triggerSourceComboBox.Enabled = isEnabled
        Me.scanAdvancedOutputComboBox.Enabled = isEnabled
        Me.switchTriggerInputComboBox.Enabled = isEnabled
        Me.rangeNumericUpDown.Enabled = isEnabled
        Me.resolutionNumericUpDown.Enabled = isEnabled
        Me.samplesToFetchNumericUpDown.Enabled = isEnabled
        Me.measCompleteDestComboBox.Enabled = isEnabled

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
                ShowError("Unable to Close Session, Reset the device." & vbLf & "Error : " & ex.Message)
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
                ShowError("Unable to Close Session, Reset the device." & vbLf & "Error : " & ex.Message)
                Application.[Exit]()
            End Try
        End If


    End Sub

    Private Sub InitializeDmmSession()

        ' Create a Dmm Session
        dmmSession = New NIDmm(DmmResourceName, True, True)

    End Sub
    Private Sub ConfigureDmmMeasurement()
        ConfigureMeasurement()
        ConfigureDmmTrigger()
        ConfigureDmmMultiPoint()
        ConfigureSampleTriggerSlope()
        ConfigureDmmMeasurementComplete()
    End Sub
    Private Sub InitiateDmmMeasurement()
        'Initiate the DMM measurement.
        dmmSession.Measurement.Initiate()
    End Sub

    Private Function GetDmmInstrumentModel() As String
        'Queries an attribute value to determine the instrument module of the DMM.
        Return (dmmSession.DriverIdentity.InstrumentModel)
    End Function

    Private Sub ConfigureDmmMeasurementComplete()
        'Configures the destination of the DMM output trigger (Measurement Complete). 
        'This should match the switch module input trigger.
        dmmSession.Trigger.MeasurementCompleteDestination = MeasCompleteDestination

        'Configures the slope of the DMM output trigger.
        dmmSession.Trigger.MeasurementCompleteDestinationSlope = DmmSlope.Negative
    End Sub

    Private Sub ConfigureSampleTriggerSlope()
        'Configures the slope of the secondary DMM input trigger (Sample Trigger).
        dmmSession.Trigger.MultiPoint.SampleTriggerSlope = DmmSlope.Negative
    End Sub

    Private Sub ConfigureDmmMultiPoint()
        'Configure a multipoint acquisition.
        Dim SampleTrigger As DmmSampleTrigger = GetSampleTrigger()
        dmmSession.Trigger.MultiPoint.Configure(1, 1, SampleTrigger, PrecisionTimeSpan.MaxValue)
        dmmSession.Trigger.MultiPoint.SampleCount = 0
    End Sub

    Private Function GetSampleTrigger() As DmmSampleTrigger
        If TriggerSource Is DmmTriggerSource.SoftwareTrigger Then
            Return DmmSampleTrigger.SoftwareTrigger
        ElseIf TriggerSource Is DmmTriggerSource.Ttl7 Then
            Return DmmSampleTrigger.Ttl7
        ElseIf TriggerSource Is DmmTriggerSource.LbrTrig1 Then
            Return DmmSampleTrigger.LbrTrig1
        ElseIf TriggerSource Is DmmTriggerSource.PxiStar Then
            Return DmmSampleTrigger.PxiStar
        ElseIf TriggerSource Is DmmTriggerSource.AuxTrig1 Then
            Return DmmSampleTrigger.AuxTrig1
        ElseIf TriggerSource Is DmmTriggerSource.Ttl0 Then
            Return DmmSampleTrigger.Ttl0
        ElseIf TriggerSource Is DmmTriggerSource.Ttl1 Then
            Return DmmSampleTrigger.Ttl1
        ElseIf TriggerSource Is DmmTriggerSource.Ttl2 Then
            Return DmmSampleTrigger.Ttl2
        ElseIf TriggerSource Is DmmTriggerSource.Ttl3 Then
            Return DmmSampleTrigger.Ttl3
        ElseIf TriggerSource Is DmmTriggerSource.Ttl4 Then
            Return DmmSampleTrigger.Ttl4
        ElseIf TriggerSource Is DmmTriggerSource.Ttl5 Then
            Return DmmSampleTrigger.Ttl5
        ElseIf TriggerSource Is DmmTriggerSource.Ttl6 Then
            Return DmmSampleTrigger.Ttl6
        ElseIf TriggerSource Is DmmTriggerSource.External Then
            Return DmmSampleTrigger.External
        Else
            Return DmmSampleTrigger.Immediate
        End If
    End Function

    Private Sub ConfigureDmmTrigger()
        'Configure the input trigger for the DMM. 
        'This should match the output trigger (Scan Advanced Output) of the switch.
        dmmSession.Trigger.Configure(TriggerSource, True)
        dmmSession.Trigger.Slope = DmmSlope.Negative
    End Sub

    Private Sub ConfigureMeasurement()
        'Configure the function, range, and resolution of the measurement.
        dmmSession.Configure(DirectCast([Enum].Parse(GetType(DmmMeasurementFunction), MeasurementMode), DmmMeasurementFunction), MeasurementRange, MeasurementResolution)

    End Sub
    Private Sub AbortSwitchScan()
        '''/Halts the scan.
        switchSession.Scan.Abort()

    End Sub

    Private Sub InitiateSwitchScan()
        'Initiates the scan usings the configured scan list and triggers.
        switchSession.Scan.Initiate()
    End Sub

    Private Sub ConfigureSwitchScan()
        'Configures the switch module for scanning. 
        switchSession.Scan.ConfigureList(ScanList, SwitchScanMode.BreakBeforeMake)

        'Configures the input trigger of the switch module. This should match the
        'output trigger of the DMM.
        switchSession.Scan.ConfigureTrigger(New PrecisionTimeSpan(0.0), SwitchTriggerInput, ScanAdvancedOutput)

        'Configures the switch to loop continuously through the scan list until
        'niSwitch_Abort is called.
        switchSession.Scan.Continuous = True

        'Sets the polarity and edge of the switch input trigger.
        switchSession.Scan.TriggerInputPolarity = SwitchTriggerInputPolarity.FallingEdge

        'Sets the polarity and edge of the switch output trigger.
        switchSession.Scan.AdvancedPolarity = SwitchAdvancedPolarity.FallingEdge
    End Sub
    Private Sub StartMeasurement()
        ' Starts a Thread for Performing Scan, when NextButton is clicked, so that Main UI thread doesn't hangs.
        workerThread = New Thread(AddressOf StartDmmMeasurement)
        workerThread.Start()

    End Sub
    Private Sub StartDmmMeasurement()
        Try

            While ThreadStopMarker = False
                dmmSession.Measurement.ReadStatus(acquisitionBacklog)
                measurement = dmmSession.Measurement.FetchMultiPoint(PrecisionTimeSpan.MaxValue, Math.Max(acquisitionBacklog, SamplesToFetch))
                Dim methodCall As Action = Sub() UpdateData(measurement)
                dataGridViewResults.Invoke(methodCall)
            End While
        Catch ex As Exception

            ShowError(ex.Message)
        Finally
            Me.BeginInvoke(New Action(AddressOf ThreadTerminate))
        End Try
    End Sub

    Private Sub CreateDataGridColumns()
        dataGridViewResults.Rows.Clear()
        dataGridViewResults.ColumnCount = 0
        dataGridViewResults.Columns.Add("", "Sample Number")
        dataGridViewResults.Columns.Add("", "Measurement")
    End Sub
    Private Sub ToggleButtonState(ByVal isEnabled As Boolean)
        Me.startButton.Enabled = isEnabled
        Me.stopButton.Enabled = Not isEnabled
    End Sub
#End Region
#Region "FormEvents"
    Private Sub MainForm_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles MyBase.FormClosing
        closeRequested = True
        ' Stops the Acquisition.
        If workerThread IsNot Nothing Then
            ThreadStopMarker = True
        End If
        If dmmSession IsNot Nothing Then
            dmmSession.Measurement.Abort()
            CloseDmmSession()
        End If
        'Abort scanning.
        If switchSession IsNot Nothing Then
            switchSession.Scan.Abort()
            CloseSwitchSession()
        End If

        If workerThread IsNot Nothing Then
            e.Cancel = True
        End If
    End Sub

    Private Sub startButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles startButton.Click
        ChangeControlState(False)
        Try
            ThreadStopMarker = False
            ToggleButtonState(False)

            'Programming the Switch       
            CloseSwitchSession()
            InitializeSwitchSession()
            ConfigureSwitchScan()

            'Programming the DMM         
            CloseDmmSession()

            InitializeDmmSession()
            ConfigureDmmMeasurement()
            dmmInstrumentModel = GetDmmInstrumentModel()
            InitiateDmmMeasurement()

            'If the DMM model is an NI 4060, add a delay.
            If dmmInstrumentModel.Equals("PXI-4060") OrElse dmmInstrumentModel.Equals("PCI-4060") Then
                Thread.Sleep(1000)
            End If
            CreateDataGridColumns()
            InitiateSwitchScan()

            'start measuring


            StartMeasurement()
        Catch ex As Exception
            ShowError(ex.Message)
            ChangeControlState(True)
            ToggleButtonState(True)
            CloseDmmSession()
            CloseSwitchSession()
        End Try

    End Sub

    Private Sub stopButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles stopButton.Click

        ' Stops the Acquisition.
        If workerThread IsNot Nothing Then
            ThreadStopMarker = True
        End If
        'Abort scanning.
        If dmmSession IsNot Nothing Then
            dmmSession.Measurement.Abort()
            CloseDmmSession()
        End If
        If switchSession IsNot Nothing Then
            switchSession.Scan.Abort()
            CloseSwitchSession()
        End If
        ChangeControlState(True)
        ToggleButtonState(True)
    End Sub

#End Region



End Class

