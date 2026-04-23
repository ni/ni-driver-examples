'==================================================================================================
' Title        : Software Scanning
' Description  : Use this example to learn how to perform scanning operation with a software trigger.The 
' scanning operation is set to a continous mode.Once the start button is clicked the  configuration required 
' for scan operation is done. When the next button is clicked a software trigger is sent, the next scan 
'operation in the list is performed.Click the stop button to abort the scan and to close the switch 
'session.              
'==================================================================================================
Imports System.Drawing
Imports System.Reflection
Imports System.Threading
Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments.NISwitch
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices


Partial Public Class MainForm
    Inherits Form

    Private switchSession As NISwitch
    Private scanDelay As New PrecisionTimeSpan(0)
    Private isNextConnection As Boolean
    Private workerThread As Thread
    Private threadStop As Boolean
    Private closeRequested As Boolean
    Private lockobj As New Object()

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

    Public Sub New()
        InitializeComponent()
        LoadSwitchDeviceNames()
        LoadTopology()
        ToggleButtonState(True)
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
            resourceNameComboBox.Items.Add(device.Name)
        Next
        If modularInstrumentsSystem.DeviceCollection.Count > 0 Then
            resourceNameComboBox.SelectedIndex = 0
        End If
    End Sub
#End Region
#Region "Program Properties"
    Private ReadOnly Property ResourceName() As String
        Get
            Return Me.resourceNameComboBox.Text
        End Get
    End Property

    Private ReadOnly Property TopologyName() As String
        Get
            Return Me.topologyNameComboBox.SelectedItem.ToString()
        End Get
    End Property
    Private ReadOnly Property ScanList() As String
        Get
            Return Me.scanListTextBox.Text
        End Get
    End Property

#End Region
#Region "Form Events"
    Private Sub startScanningButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles startScanningButton.Click
        ChangeControlState(False)
        Try
            CloseSession()

            InitializeSwitchSession()
            ConfigureScanList()
            InitiateScan()
            ToggleButtonState(False)
            StartScan()
        Catch ex As System.Exception
            ShowError(ex.Message)
            CloseSession()
            ChangeControlState(True)
            ToggleButtonState(True)
        End Try
    End Sub

    Private Sub StartScan()
        ' Starts a Thread for Performing Scan, when NextButton is clicked, so that Main UI thread doesn't hangs.
        workerThread = New Thread(AddressOf SendSoftwareTrigger)
        ThreadStopMarker = False
        workerThread.Start()
    End Sub

    Private Sub InitiateScan()
        'Initiate scanning.
        switchSession.Scan.Initiate()
    End Sub

    Private Sub ConfigureScanList()
        'Configures the switch module for scanning. 
        switchSession.Scan.ConfigureList(ScanList, SwitchScanMode.BreakBeforeMake)
        'Configures the trigger to be software trigger.
        switchSession.Scan.ConfigureTrigger(scanDelay, SwitchScanTriggerInput.SoftwareTrigger, SwitchScanAdvancedOutput.None)

        'Loop through scan list continuously.
        switchSession.Scan.Continuous = True
    End Sub
    Private Sub ThreadTerminate()

        'Waits for the thread To Terminate.
        workerThread.Join()
        workerThread = Nothing
        If closeRequested = True Then
            Me.Close()
        End If
    End Sub

    Private Sub SendSoftwareTrigger()
        'Send software trigger to switch module when Next Connection button pressed on front panel.
        While ThreadStopMarker = False

            If isNextConnection Then
                Try
                    switchSession.Scan.SendSoftwareTrigger()
                Catch ex As Exception
                    ShowError(ex.Message)
                    Me.BeginInvoke(New Action(AddressOf StopScanning))
                    Exit Try
                End Try
                isNextConnection = False
            End If
        End While
        Me.BeginInvoke(New Action(AddressOf ThreadTerminate))
    End Sub

    Private Sub nextConnectionButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles nextConnectionButton.Click
        isNextConnection = True
    End Sub

    Private Sub stopScanningButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles stopScanningButton.Click

        ' Stops the Acquisition.
        If workerThread IsNot Nothing Then
            ThreadStopMarker = True
        End If
        'Abort scanning.
        switchSession.Scan.Abort()
        ChangeControlState(True)
        CloseSession()
        ToggleButtonState(True)
    End Sub
    Private Sub StopScanning()
        ' Stops the Acquisition.
        If workerThread IsNot Nothing Then
            ThreadStopMarker = True
        End If
        'Abort scanning.
        If switchSession.Scan.IsScanning Then
            switchSession.Scan.Abort()
        End If
        ChangeControlState(True)
        CloseSession()
        ToggleButtonState(True)
    End Sub

    Private Sub MainForm_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles MyBase.FormClosing
        closeRequested = True
        ' Stops the Acquisition.
        If workerThread IsNot Nothing Then
            ThreadStopMarker = True
        End If
        'Abort scanning.
        If switchSession IsNot Nothing Then
            If switchSession.Scan.IsScanning Then
                switchSession.Scan.Abort()
            End If
            CloseSession()
        End If

        If workerThread IsNot Nothing Then
            e.Cancel = True
        End If
    End Sub
#End Region
#Region "Program Functions"
    Private Sub ChangeControlState(ByVal isEnabled As Boolean)
        Me.startScanningButton.Enabled = isEnabled
        Me.nextConnectionButton.Enabled = isEnabled
        Me.stopScanningButton.Enabled = isEnabled
        Me.resourceNameComboBox.Enabled = isEnabled
        Me.topologyNameComboBox.Enabled = isEnabled
        Me.scanListTextBox.Enabled = isEnabled
    End Sub

    Private Sub ToggleButtonState(ByVal isEnabled As Boolean)
        Me.startScanningButton.Enabled = isEnabled
        Me.stopScanningButton.Enabled = Not isEnabled
        Me.nextConnectionButton.Enabled = Not isEnabled

    End Sub
    Private Shared Sub ShowError(ByVal message As String)
        If String.IsNullOrEmpty(message) Then
            message = "Unexpected Error"
        End If
        MessageBox.Show(message, "Error")


    End Sub
    Private Sub InitializeSwitchSession()
        switchSession = New NISwitch(ResourceName, TopologyName, False, True)
        AddHandler switchSession.DriverOperation.Warning, New System.EventHandler(Of SwitchWarningEventArgs)(AddressOf DriverOperationWarning)
    End Sub
    Private Sub DriverOperationWarning(ByVal sender As Object, ByVal e As SwitchWarningEventArgs)
        MessageBox.Show(e.ToString(), "Warning")
    End Sub
    Private Sub CloseSession()
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
#End Region
End Class

