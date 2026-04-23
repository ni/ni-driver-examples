'==================================================================================================
' Title        : Interleaved Scanning PXI 2584
' Description  : Use this example to use NI-Switch and Ni-Dmm together. The example,first gets a 
'interleaved scan list and using this scan list configuration is done for the scanning operation for
' switch, then measurement configurations for Dmm is done, Finally, the Dmm device takes the reading 
'after the switch scan is performed.            
'==================================================================================================
Imports System.Collections
Imports System.Collections.Generic
Imports System.Threading
Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments.NIDmm
Imports NationalInstruments.ModularInstruments.NISwitch
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices
Imports System.Reflection


Partial Public Class MainForm
    Inherits Form
    Private generateMultiDeviceInterleavedScanList As GenerateMultiDeviceInterleavedScanList
    Private Const SwitchTopology As String = "2584/Independent"
    Private interleavedScanList As String
    Private switchSession As NISwitch
    Private dmmSession As NIDmm
    Private dmmInstrumentModel As String
    Private workerThread As Thread
    Private threadStop As Boolean
    Private closeRequested As Boolean
    Private lockobj As New Object()
    Private StartEndRelationError As String, ValidChannelValuesError As String, InputFieldBlank As String

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
    Private acquisitionBacklog As Integer
    Private measurement As Double()

    Public Sub New()
        InitializeComponent()
        InitializeLookupTable()
        LoadSwitchDeviceNames()
        LoadSwitchTriggerInput()
        LoadSwitchTriggerOutput()
        LoadDmmDeviceNames()
        LoadDmmMeasurementModes()
        LoadDmmMeasurementCompleteDestination()
        LoadDmmTriggerSource()
    End Sub
#Region "UI Initial Value Config Section"
    Private Sub LoadSwitchDeviceNames()
        Dim modularInstrumentsSystem As New ModularInstrumentsSystem("NI-SWITCH")
        For Each device As DeviceInfo In modularInstrumentsSystem.DeviceCollection
            switchResourceNameComboBox.Items.Add(device.Name)
        Next
        If modularInstrumentsSystem.DeviceCollection.Count > 0 Then
            switchResourceNameComboBox.SelectedIndex = 0
        End If
    End Sub
    Private Sub LoadSwitchTriggerInput()
        Dim myType As Type = GetType(SwitchScanTriggerInput)
        Dim properties As PropertyInfo() = myType.GetProperties()

        For Each prop As PropertyInfo In properties
            switchTriggerInputComboBox.Items.Add(prop.GetValue(myType, Nothing).ToString())
        Next
        switchTriggerInputComboBox.SelectedIndex = 3
    End Sub
    Private Sub LoadSwitchTriggerOutput()
        Dim myType As Type = GetType(SwitchScanAdvancedOutput)
        Dim properties As PropertyInfo() = myType.GetProperties()

        For Each prop As PropertyInfo In properties
            switchScanAdvancedOutputComboBox.Items.Add(prop.GetValue(myType, Nothing).ToString())
        Next
        switchScanAdvancedOutputComboBox.SelectedIndex = 3

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
    Private Sub LoadDmmMeasurementModes()
        dmmMeasurementTypeComboBox.Items.AddRange([Enum].GetNames(GetType(DmmMeasurementFunction)))
        dmmMeasurementTypeComboBox.Items.Remove(DmmMeasurementFunction.WaveformCurrent.ToString())
        dmmMeasurementTypeComboBox.Items.Remove(DmmMeasurementFunction.WaveformVoltage.ToString())
        dmmMeasurementTypeComboBox.SelectedIndex = 0
    End Sub
    Private Sub LoadDmmMeasurementCompleteDestination()
        Dim myType As Type = GetType(DmmMeasurementCompleteDestination)
        Dim properties As PropertyInfo() = myType.GetProperties()

        For Each prop As PropertyInfo In properties
            dmmMeasurementCompleteDestinationComboBox.Items.Add(prop.GetValue(myType, Nothing).ToString())
        Next
        dmmMeasurementCompleteDestinationComboBox.SelectedIndex = 2
    End Sub
    Private Sub LoadDmmTriggerSource()
        Dim myType As Type = GetType(DmmTriggerSource)
        Dim properties As PropertyInfo() = myType.GetProperties()

        For Each prop As PropertyInfo In properties
            dmmTriggerSourceComboBox.Items.Add(prop.GetValue(myType, Nothing).ToString())
        Next
        dmmTriggerSourceComboBox.SelectedIndex = 4

    End Sub
#End Region

#Region "FormEvents"
    Private Sub startButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles startButton.Click
        ChangeControlState(False)
        Try
            ThreadStopMarker = False

            CloseSwitchSession()
            If InputNotBlank() Then

                interleavedScanList = generateMultiDeviceInterleavedScanList.Generate(InterleavedStartChannel, InterleavedEndChannel)
                If interleavedScanList.Equals(StartEndRelationError) OrElse interleavedScanList.Equals(ValidChannelValuesError) Then
                    Throw New System.ArgumentException(interleavedScanList)
                End If
                'programming switch
                InitializeSwitchSession()
                ConfigureSwitchScan()
                CommitSwitch()



                'programming DMM
                CloseDmmSession()
                InitializeDmmSession()
                dmmInstrumentModel = GetDmmInstrumentModel()
                ConfigureDmmMeasurement()
                'If the DMM model is an NI 4060, add a delay.
                If dmmInstrumentModel.Equals("PXI-4060") OrElse dmmInstrumentModel.Equals("PCI-4060") Then
                    Thread.Sleep(1000)
                End If
                InitiateDmmMeasurement()
                CreateDataGridColumns()
                InitiateSwitchScan()
                'start measuring
                StartMeasurement()
                ToggleButtonState(False)
            Else
                Throw New System.ArgumentException(InputFieldBlank)
            End If
        Catch ex As Exception
            ShowError(ex.Message)
            ChangeControlState(True)
            ToggleButtonState(True)
            CloseDmmSession()
            CloseSwitchSession()

        End Try

    End Sub
    Private Sub CreateDataGridColumns()
        dataGridViewResults.Rows.Clear()
        dataGridViewResults.ColumnCount = 0
        dataGridViewResults.Columns.Add("", "Sample Number")
        dataGridViewResults.Columns.Add("", "Measurement")
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
    Private Sub MainForm_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles MyBase.FormClosing
        closeRequested = True
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
#End Region

#Region "ProgramFunctions"
    Private Function InputNotBlank() As Boolean

        If ((String.IsNullOrEmpty((Me.switchInterleavedEndChannelNumericUpDown).Text)) Or (String.IsNullOrEmpty((Me.switchInterleavedStartChannelNumericUpDown).Text))) Then

            Return False

        Else
            Return True
        End If

    End Function

    Private Sub ToggleButtonState(ByVal isEnabled As Boolean)
        Me.startButton.Enabled = isEnabled
        Me.stopButton.Enabled = Not isEnabled
    End Sub

    Private Sub ChangeControlState(ByVal isEnabled As Boolean)

        Me.startButton.Enabled = isEnabled
        Me.stopButton.Enabled = isEnabled
        Me.dmmResourceNameComboBox.Enabled = isEnabled
        Me.switchResourceNameComboBox.Enabled = isEnabled
        Me.startButton.Enabled = isEnabled
        Me.dmmResourceNameComboBox.Enabled = isEnabled
        Me.switchResourceNameComboBox.Enabled = isEnabled
        Me.switchInterleavedEndChannelNumericUpDown.Enabled = isEnabled
        Me.switchInterleavedStartChannelNumericUpDown.Enabled = isEnabled
        Me.switchTriggerInputComboBox.Enabled = isEnabled
        Me.switchScanAdvancedOutputComboBox.Enabled = isEnabled
        Me.dmmMeasurementTypeComboBox.Enabled = isEnabled
        Me.dmmRangeNumericUpDown.Enabled = isEnabled
        Me.dmmResolutionNumericUpDown.Enabled = isEnabled
        Me.dmmSamplesNumericUpDown.Enabled = isEnabled
        Me.dmmMeasurementCompleteDestinationComboBox.Enabled = isEnabled
        Me.dmmTriggerSourceComboBox.Enabled = isEnabled

    End Sub

    Private Sub InitializeLookupTable()
        generateMultiDeviceInterleavedScanList = New GenerateMultiDeviceInterleavedScanList()
        generateMultiDeviceInterleavedScanList.InitializeLookupTable()
        StartEndRelationError = "Interleaved start channel must be smaller or equal to interleaved end channel on all devices."
        ValidChannelValuesError = "Valid channel numbers for the PXI-2584 interleaved scanning are between 0 and 10."
        InputFieldBlank = "The input fields for Interleaved Start or End Channel should not be blank"
    End Sub

    Private Shared Sub ShowError(ByVal message As String)
        If String.IsNullOrEmpty(message) Then
            message = "Unexpected Error"
        End If
        MessageBox.Show(message, "Error")


    End Sub

    Private Sub DriverOperationWarning(ByVal sender As Object, ByVal e As SwitchWarningEventArgs)
        MessageBox.Show(e.ToString(), "Warning")
    End Sub

    Private Sub InitializeSwitchSession()
        'Open a session to the switch module and set the topology.
        switchSession = New NISwitch(SwitchResourceName, SwitchTopology, False, True)
        AddHandler switchSession.DriverOperation.Warning, New System.EventHandler(Of SwitchWarningEventArgs)(AddressOf DriverOperationWarning)
    End Sub

    Private Sub ConfigureSwitchScan()
        'Configures the input trigger of the switch module. This should match the
        'output trigger of the DMM.
        switchSession.Scan.ConfigureTrigger(New PrecisionTimeSpan(0.0), TriggerInput, ScanAdvancedOutput)

        'Configures the switch to loop continuously through the scan list until
        'niSwitch_Abort is called.
        switchSession.Scan.Continuous = True

        'Configures the switch module for scanning. 
        switchSession.Scan.ConfigureList(interleavedScanList, SwitchScanMode.None)
    End Sub

    Private Sub CommitSwitch()
        switchSession.Scan.Commit()
    End Sub

    Private Sub InitiateSwitchScan()
        switchSession.Scan.Initiate()
    End Sub

    Private Sub AbortSwitchScan()
        If switchSession IsNot Nothing Then
            switchSession.Scan.Abort()
        End If
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

    Private Function GetDmmInstrumentModel() As String
        'Queries an attribute value to determine the instrument module of the DMM.
        Return (dmmSession.DriverIdentity.InstrumentModel)
    End Function

    Private Sub ConfigureDmmMeasurement()
        ConfigureMeasurement()
        ConfigureDmmTrigger()
        ConfigureDmmMultiPoint()
        ConfigureSampleTriggerSlope()
        ConfigureDmmMeasurementComplete()
    End Sub

    Private Sub ConfigureMeasurement()

        'Configure the function, range, and resolution of the measurement.
        dmmSession.Configure(DirectCast([Enum].Parse(GetType(DmmMeasurementFunction), MeasurementMode), DmmMeasurementFunction), DmmRange, DmmResolution)

    End Sub

    Private Sub ConfigureDmmTrigger()
        'Configure the input trigger for the DMM. 
        'This should match the output trigger (Scan Advanced Output) of the switch.

        dmmSession.Trigger.Configure(DmmTriggerSource, True)
        dmmSession.Trigger.Slope = DmmSlope.Negative
    End Sub

    Private Sub ConfigureDmmMultiPoint()
        'Configure a multipoint acquisition.
        dmmSession.Trigger.MultiPoint.Configure(1, 1, DmmSampleTrigger.External, PrecisionTimeSpan.MaxValue)
        dmmSession.Trigger.MultiPoint.SampleCount = 0

    End Sub

    Private Sub ConfigureSampleTriggerSlope()
        'Configures the slope of the secondary DMM input trigger (Sample Trigger).
        dmmSession.Trigger.MultiPoint.SampleTriggerSlope = DmmSlope.Negative
    End Sub

    Private Sub ConfigureDmmMeasurementComplete()
        'Configures the destination of the DMM output trigger (Measurement Complete). 
        'This should match the switch module input trigger.
        dmmSession.Trigger.MeasurementCompleteDestination = DmmMeasurementCompleteDestination
        'Configures the slope of the DMM output trigger.
        dmmSession.Trigger.MeasurementCompleteDestinationSlope = DmmSlope.Negative
    End Sub

    Private Sub InitiateDmmMeasurement()
        dmmSession.Measurement.Initiate()
    End Sub

    Private Sub CloseDmm()
        If dmmSession IsNot Nothing Then
            Try
                dmmSession.Measurement.Abort()
                dmmSession.Close()
                dmmSession = Nothing
            Catch ex As System.Exception
                ShowError("Unable to Close Session, Reset the device." & vbLf & "Error : " & ex.Message)
                Application.[Exit]()
            End Try
        End If
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
                measurement = dmmSession.Measurement.FetchMultiPoint(New PrecisionTimeSpan(5), Math.Max(acquisitionBacklog, SamplesToFetch))
                Dim methodCall As Action = Sub() UpdateData(measurement)
                dataGridViewResults.Invoke(methodCall)
            End While
        Catch ex As Exception

            ShowError(ex.Message)
        Finally
            Me.BeginInvoke(New Action(AddressOf ThreadTerminate))
        End Try
    End Sub

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
#End Region

#Region "ProgramProperties"
    Private ReadOnly Property SwitchResourceName() As String
        Get
            Return Me.switchResourceNameComboBox.Text
        End Get
    End Property
    Private ReadOnly Property InterleavedStartChannel() As Integer
        Get
            Return CInt(Me.switchInterleavedStartChannelNumericUpDown.Value)
        End Get
    End Property

    Private ReadOnly Property InterleavedEndChannel() As Integer
        Get
            Return CInt(Me.switchInterleavedEndChannelNumericUpDown.Value)
        End Get
    End Property
    Private ReadOnly Property TriggerInput() As String
        Get
            Return Me.switchTriggerInputComboBox.Text
        End Get
    End Property
    Private ReadOnly Property SamplesToFetch() As Integer
        Get
            Return CInt(Me.dmmSamplesNumericUpDown.Value)
        End Get
    End Property
    Private ReadOnly Property ScanAdvancedOutput() As String
        Get
            Return Me.switchScanAdvancedOutputComboBox.Text
        End Get
    End Property


    Private ReadOnly Property DmmResourceName() As String
        Get
            Return Me.dmmResourceNameComboBox.Text
        End Get
    End Property
    Private ReadOnly Property MeasurementMode() As String
        Get
            Return Me.dmmMeasurementTypeComboBox.Text
        End Get
    End Property
   
   
    Private ReadOnly Property DmmResolution() As Double
        Get
            Return CDbl(Me.dmmResolutionNumericUpDown.Value)
        End Get
    End Property
    Private ReadOnly Property DmmMeasurementCompleteDestination() As String
        Get
            Return Me.dmmMeasurementCompleteDestinationComboBox.Text
        End Get
    End Property
    Private ReadOnly Property DmmRange() As Double
        Get
            Return CDbl(Me.dmmRangeNumericUpDown.Value)
        End Get
    End Property
    
    Private ReadOnly Property DmmTriggerSource() As String
        Get
            Return Me.dmmTriggerSourceComboBox.Text
        End Get
    End Property

#End Region




End Class

