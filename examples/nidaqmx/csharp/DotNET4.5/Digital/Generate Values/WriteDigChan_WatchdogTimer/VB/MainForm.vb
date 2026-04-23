'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   WriteDigChan_WatchdogTimer
'
' Category:
'   DO
'
' Description:
'   This example demonstrates how to write values to a digital output channel,
'   using a watchdog timer.
'
' Instructions for running:
'   1.  Select the digital lines on the DAQ device to be written to.
'   2.  Select a value to write.
'
' Steps:
'   1.  Create an output task and a watchdog timer task.
'   2.  Create a digital output channel. Use one channel for all lines.
'   3.  Call Task.Start() on both tasks.
'   4.  Write the digital boolean array data. This write method writes a single
'       sample of digital data on demand, so no timeout is necessary. This
'       example uses an asynchronous write method and installs a callback.
'   5.  Inside the callback, reset the watchdog timer and perform another write.
'   6.  When the user presses the stop button, stop the tasks.
'   7.  Dispose the Task object to clean-up any resources associated with the
'       task.
'   8.  Handle any DaqExceptions, if they occur.
'
'   Note: This example sets SynchronizeCallback to true. If SynchronizeCallback
'   is set to false, then you must give special consideration to safely dispose
'   the task and to update the UI from the callback. If SynchronizeCallback is
'   set to false, the callback executes on the worker thread and not on the main
'   UI thread. You can only update a UI component on the thread on which it was
'   created. Refer to the How to: Safely Dispose Task When Using Asynchronous
'   Callbacks topic in the NI-DAQmx .NET help for more information.
'
' I/O Connections Overview:
'   Make sure your signal output terminals match the Lines I/O Control. In this
'   case wire the item to receive the signal to the first eight digital lines on
'   your DAQ Device.  For more information on the input and output terminals for
'   your device, open the NI-DAQmx Help, and refer to the NI-DAQmx Device
'   Terminals and Device Considerations books in the table of contents.
'
' Microsoft Windows Vista User Account Control
'   Running certain applications on Microsoft Windows Vista requires
'   administrator privileges, 
'   because the application name contains keywords such as setup, update, or
'   install. To avoid this problem, 
'   you must add an additional manifest to the application that specifies the
'   privileges required to run 
'   the application. Some Measurement Studio NI-DAQmx examples for Visual Studio
'   include these keywords. 
'   Therefore, all examples for Visual Studio are shipped with an additional
'   manifest file that you must 
'   embed in the example executable. The manifest file is named
'   [ExampleName].exe.manifest, where [ExampleName] 
'   is the NI-provided example name. For information on how to embed the manifest
'   file, refer to http://msdn2.microsoft.com/en-us/library/bb756929.aspx.Note: 
'   The manifest file is not provided with examples for Visual Studio .NET 2003.
'
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

Imports NationalInstruments
Imports NationalInstruments.DAQmx

Public Class MainForm
    Inherits System.Windows.Forms.Form

    Private runningTask As Task
    Private outputTask As Task = Nothing
    Private watchdogTask As Task = Nothing
    Private writer As DigitalSingleChannelWriter = Nothing

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()
        Application.EnableVisualStyles()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        physicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DOLine, PhysicalChannelAccess.External))
        deviceComboBox.Items.AddRange(DaqSystem.Local.Devices)
        watchdogPhysicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DOLine Or PhysicalChannelTypes.DOPort, PhysicalChannelAccess.External))
    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
            If Not (outputTask Is Nothing) Then
                runningTask = Nothing
                outputTask.Dispose()
            End If
            If Not (watchdogTask Is Nothing) Then
                watchdogTask.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Private WithEvents startButton As System.Windows.Forms.Button
    Private WithEvents watchdogSettingsGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents watchdogExpirationState As System.Windows.Forms.ComboBox
    Private WithEvents expirationStateLabel As System.Windows.Forms.Label
    Private WithEvents physicalChannelLabel As System.Windows.Forms.Label
    Private WithEvents warningText As System.Windows.Forms.Label
    Private WithEvents dataToWriteGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents line5Label As System.Windows.Forms.Label
    Private WithEvents line2Label As System.Windows.Forms.Label
    Private WithEvents line6 As System.Windows.Forms.CheckBox
    Private WithEvents line1Label As System.Windows.Forms.Label
    Private WithEvents line3 As System.Windows.Forms.CheckBox
    Private WithEvents line6Label As System.Windows.Forms.Label
    Private WithEvents line1 As System.Windows.Forms.CheckBox
    Private WithEvents line0Label As System.Windows.Forms.Label
    Private WithEvents line4 As System.Windows.Forms.CheckBox
    Private WithEvents line7Label As System.Windows.Forms.Label
    Private WithEvents line3Label As System.Windows.Forms.Label
    Private WithEvents line0 As System.Windows.Forms.CheckBox
    Private WithEvents line4Label As System.Windows.Forms.Label
    Private WithEvents line7 As System.Windows.Forms.CheckBox
    Private WithEvents line5 As System.Windows.Forms.CheckBox
    Private WithEvents line2 As System.Windows.Forms.CheckBox
    Private WithEvents physicalChannelToWriteLabel As System.Windows.Forms.Label
    Private WithEvents timeout As System.Windows.Forms.TextBox
    Private WithEvents timeoutLabel As System.Windows.Forms.Label
    Private WithEvents loopTimeLabel As System.Windows.Forms.Label
    Private WithEvents stopButton As System.Windows.Forms.Button
    Private WithEvents deviceNameLabel As System.Windows.Forms.Label
    Friend WithEvents writeInterval As System.Windows.Forms.NumericUpDown
    Friend WithEvents physicalChannelComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents deviceComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents watchdogPhysicalChannelComboBox As System.Windows.Forms.ComboBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MainForm))
        Me.startButton = New System.Windows.Forms.Button
        Me.watchdogSettingsGroupBox = New System.Windows.Forms.GroupBox
        Me.watchdogPhysicalChannelComboBox = New System.Windows.Forms.ComboBox
        Me.watchdogExpirationState = New System.Windows.Forms.ComboBox
        Me.expirationStateLabel = New System.Windows.Forms.Label
        Me.physicalChannelLabel = New System.Windows.Forms.Label
        Me.warningText = New System.Windows.Forms.Label
        Me.dataToWriteGroupBox = New System.Windows.Forms.GroupBox
        Me.line5Label = New System.Windows.Forms.Label
        Me.line2Label = New System.Windows.Forms.Label
        Me.line6 = New System.Windows.Forms.CheckBox
        Me.line1Label = New System.Windows.Forms.Label
        Me.line3 = New System.Windows.Forms.CheckBox
        Me.line6Label = New System.Windows.Forms.Label
        Me.line1 = New System.Windows.Forms.CheckBox
        Me.line0Label = New System.Windows.Forms.Label
        Me.line4 = New System.Windows.Forms.CheckBox
        Me.line7Label = New System.Windows.Forms.Label
        Me.line3Label = New System.Windows.Forms.Label
        Me.line0 = New System.Windows.Forms.CheckBox
        Me.line4Label = New System.Windows.Forms.Label
        Me.line7 = New System.Windows.Forms.CheckBox
        Me.line5 = New System.Windows.Forms.CheckBox
        Me.line2 = New System.Windows.Forms.CheckBox
        Me.physicalChannelToWriteLabel = New System.Windows.Forms.Label
        Me.timeout = New System.Windows.Forms.TextBox
        Me.timeoutLabel = New System.Windows.Forms.Label
        Me.loopTimeLabel = New System.Windows.Forms.Label
        Me.stopButton = New System.Windows.Forms.Button
        Me.deviceNameLabel = New System.Windows.Forms.Label
        Me.writeInterval = New System.Windows.Forms.NumericUpDown
        Me.physicalChannelComboBox = New System.Windows.Forms.ComboBox
        Me.deviceComboBox = New System.Windows.Forms.ComboBox
        Me.watchdogSettingsGroupBox.SuspendLayout()
        Me.dataToWriteGroupBox.SuspendLayout()
        CType(Me.writeInterval, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'startButton
        '
        Me.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.startButton.Location = New System.Drawing.Point(380, 107)
        Me.startButton.Name = "startButton"
        Me.startButton.TabIndex = 0
        Me.startButton.Text = "&Start"
        '
        'watchdogSettingsGroupBox
        '
        Me.watchdogSettingsGroupBox.Controls.Add(Me.watchdogPhysicalChannelComboBox)
        Me.watchdogSettingsGroupBox.Controls.Add(Me.watchdogExpirationState)
        Me.watchdogSettingsGroupBox.Controls.Add(Me.expirationStateLabel)
        Me.watchdogSettingsGroupBox.Controls.Add(Me.physicalChannelLabel)
        Me.watchdogSettingsGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.watchdogSettingsGroupBox.Location = New System.Drawing.Point(188, 99)
        Me.watchdogSettingsGroupBox.Name = "watchdogSettingsGroupBox"
        Me.watchdogSettingsGroupBox.Size = New System.Drawing.Size(184, 160)
        Me.watchdogSettingsGroupBox.TabIndex = 12
        Me.watchdogSettingsGroupBox.TabStop = False
        Me.watchdogSettingsGroupBox.Text = "Expiration State (Watchdog)"
        '
        'watchdogPhysicalChannelComboBox
        '
        Me.watchdogPhysicalChannelComboBox.Location = New System.Drawing.Point(16, 68)
        Me.watchdogPhysicalChannelComboBox.Name = "watchdogPhysicalChannelComboBox"
        Me.watchdogPhysicalChannelComboBox.Size = New System.Drawing.Size(152, 21)
        Me.watchdogPhysicalChannelComboBox.TabIndex = 1
        Me.watchdogPhysicalChannelComboBox.Text = "Dev1/port0/line0:7"
        '
        'watchdogExpirationState
        '
        Me.watchdogExpirationState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.watchdogExpirationState.Items.AddRange(New Object() {"High", "Low", "Tristate", "No Change"})
        Me.watchdogExpirationState.Location = New System.Drawing.Point(16, 112)
        Me.watchdogExpirationState.Name = "watchdogExpirationState"
        Me.watchdogExpirationState.Size = New System.Drawing.Size(152, 21)
        Me.watchdogExpirationState.TabIndex = 3
        '
        'expirationStateLabel
        '
        Me.expirationStateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.expirationStateLabel.Location = New System.Drawing.Point(16, 96)
        Me.expirationStateLabel.Name = "expirationStateLabel"
        Me.expirationStateLabel.Size = New System.Drawing.Size(88, 16)
        Me.expirationStateLabel.TabIndex = 2
        Me.expirationStateLabel.Text = "Expiration State"
        '
        'physicalChannelLabel
        '
        Me.physicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.physicalChannelLabel.Location = New System.Drawing.Point(16, 48)
        Me.physicalChannelLabel.Name = "physicalChannelLabel"
        Me.physicalChannelLabel.Size = New System.Drawing.Size(140, 16)
        Me.physicalChannelLabel.TabIndex = 0
        Me.physicalChannelLabel.Text = "Physical Channel Name"
        '
        'warningText
        '
        Me.warningText.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.warningText.Location = New System.Drawing.Point(12, 59)
        Me.warningText.Name = "warningText"
        Me.warningText.Size = New System.Drawing.Size(160, 32)
        Me.warningText.TabIndex = 4
        Me.warningText.Text = "You must specify exactly eight lines in the channel string"
        '
        'dataToWriteGroupBox
        '
        Me.dataToWriteGroupBox.Controls.Add(Me.line5Label)
        Me.dataToWriteGroupBox.Controls.Add(Me.line2Label)
        Me.dataToWriteGroupBox.Controls.Add(Me.line6)
        Me.dataToWriteGroupBox.Controls.Add(Me.line1Label)
        Me.dataToWriteGroupBox.Controls.Add(Me.line3)
        Me.dataToWriteGroupBox.Controls.Add(Me.line6Label)
        Me.dataToWriteGroupBox.Controls.Add(Me.line1)
        Me.dataToWriteGroupBox.Controls.Add(Me.line0Label)
        Me.dataToWriteGroupBox.Controls.Add(Me.line4)
        Me.dataToWriteGroupBox.Controls.Add(Me.line7Label)
        Me.dataToWriteGroupBox.Controls.Add(Me.line3Label)
        Me.dataToWriteGroupBox.Controls.Add(Me.line0)
        Me.dataToWriteGroupBox.Controls.Add(Me.line4Label)
        Me.dataToWriteGroupBox.Controls.Add(Me.line7)
        Me.dataToWriteGroupBox.Controls.Add(Me.line5)
        Me.dataToWriteGroupBox.Controls.Add(Me.line2)
        Me.dataToWriteGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.dataToWriteGroupBox.Location = New System.Drawing.Point(188, 11)
        Me.dataToWriteGroupBox.Name = "dataToWriteGroupBox"
        Me.dataToWriteGroupBox.Size = New System.Drawing.Size(272, 80)
        Me.dataToWriteGroupBox.TabIndex = 11
        Me.dataToWriteGroupBox.TabStop = False
        Me.dataToWriteGroupBox.Text = "Data to Write"
        '
        'line5Label
        '
        Me.line5Label.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.line5Label.Location = New System.Drawing.Point(176, 48)
        Me.line5Label.Name = "line5Label"
        Me.line5Label.Size = New System.Drawing.Size(16, 16)
        Me.line5Label.TabIndex = 10
        Me.line5Label.Text = "5"
        '
        'line2Label
        '
        Me.line2Label.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.line2Label.Location = New System.Drawing.Point(80, 48)
        Me.line2Label.Name = "line2Label"
        Me.line2Label.Size = New System.Drawing.Size(16, 16)
        Me.line2Label.TabIndex = 4
        Me.line2Label.Text = "2"
        '
        'line6
        '
        Me.line6.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.line6.Location = New System.Drawing.Point(208, 24)
        Me.line6.Name = "line6"
        Me.line6.Size = New System.Drawing.Size(16, 24)
        Me.line6.TabIndex = 13
        '
        'line1Label
        '
        Me.line1Label.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.line1Label.Location = New System.Drawing.Point(48, 48)
        Me.line1Label.Name = "line1Label"
        Me.line1Label.Size = New System.Drawing.Size(16, 16)
        Me.line1Label.TabIndex = 2
        Me.line1Label.Text = "1"
        '
        'line3
        '
        Me.line3.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.line3.Location = New System.Drawing.Point(112, 24)
        Me.line3.Name = "line3"
        Me.line3.Size = New System.Drawing.Size(16, 24)
        Me.line3.TabIndex = 7
        '
        'line6Label
        '
        Me.line6Label.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.line6Label.Location = New System.Drawing.Point(208, 48)
        Me.line6Label.Name = "line6Label"
        Me.line6Label.Size = New System.Drawing.Size(16, 16)
        Me.line6Label.TabIndex = 12
        Me.line6Label.Text = "6"
        '
        'line1
        '
        Me.line1.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.line1.Location = New System.Drawing.Point(48, 24)
        Me.line1.Name = "line1"
        Me.line1.Size = New System.Drawing.Size(16, 24)
        Me.line1.TabIndex = 3
        '
        'line0Label
        '
        Me.line0Label.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.line0Label.Location = New System.Drawing.Point(16, 48)
        Me.line0Label.Name = "line0Label"
        Me.line0Label.Size = New System.Drawing.Size(16, 16)
        Me.line0Label.TabIndex = 0
        Me.line0Label.Text = "0"
        '
        'line4
        '
        Me.line4.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.line4.Location = New System.Drawing.Point(144, 24)
        Me.line4.Name = "line4"
        Me.line4.Size = New System.Drawing.Size(16, 24)
        Me.line4.TabIndex = 9
        '
        'line7Label
        '
        Me.line7Label.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.line7Label.Location = New System.Drawing.Point(240, 48)
        Me.line7Label.Name = "line7Label"
        Me.line7Label.Size = New System.Drawing.Size(16, 16)
        Me.line7Label.TabIndex = 14
        Me.line7Label.Text = "7"
        '
        'line3Label
        '
        Me.line3Label.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.line3Label.Location = New System.Drawing.Point(112, 48)
        Me.line3Label.Name = "line3Label"
        Me.line3Label.Size = New System.Drawing.Size(16, 16)
        Me.line3Label.TabIndex = 6
        Me.line3Label.Text = "3"
        '
        'line0
        '
        Me.line0.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.line0.Location = New System.Drawing.Point(16, 24)
        Me.line0.Name = "line0"
        Me.line0.Size = New System.Drawing.Size(16, 24)
        Me.line0.TabIndex = 1
        '
        'line4Label
        '
        Me.line4Label.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.line4Label.Location = New System.Drawing.Point(144, 48)
        Me.line4Label.Name = "line4Label"
        Me.line4Label.Size = New System.Drawing.Size(16, 16)
        Me.line4Label.TabIndex = 8
        Me.line4Label.Text = "4"
        '
        'line7
        '
        Me.line7.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.line7.Location = New System.Drawing.Point(240, 24)
        Me.line7.Name = "line7"
        Me.line7.Size = New System.Drawing.Size(16, 24)
        Me.line7.TabIndex = 15
        '
        'line5
        '
        Me.line5.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.line5.Location = New System.Drawing.Point(176, 24)
        Me.line5.Name = "line5"
        Me.line5.Size = New System.Drawing.Size(16, 24)
        Me.line5.TabIndex = 11
        '
        'line2
        '
        Me.line2.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.line2.Location = New System.Drawing.Point(80, 24)
        Me.line2.Name = "line2"
        Me.line2.Size = New System.Drawing.Size(16, 24)
        Me.line2.TabIndex = 5
        '
        'physicalChannelToWriteLabel
        '
        Me.physicalChannelToWriteLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.physicalChannelToWriteLabel.Location = New System.Drawing.Point(12, 11)
        Me.physicalChannelToWriteLabel.Name = "physicalChannelToWriteLabel"
        Me.physicalChannelToWriteLabel.Size = New System.Drawing.Size(160, 16)
        Me.physicalChannelToWriteLabel.TabIndex = 2
        Me.physicalChannelToWriteLabel.Text = "Physical Channel to Write"
        '
        'timeout
        '
        Me.timeout.Location = New System.Drawing.Point(12, 163)
        Me.timeout.Name = "timeout"
        Me.timeout.Size = New System.Drawing.Size(152, 20)
        Me.timeout.TabIndex = 8
        Me.timeout.Text = "0.8"
        '
        'timeoutLabel
        '
        Me.timeoutLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.timeoutLabel.Location = New System.Drawing.Point(12, 147)
        Me.timeoutLabel.Name = "timeoutLabel"
        Me.timeoutLabel.Size = New System.Drawing.Size(112, 16)
        Me.timeoutLabel.TabIndex = 7
        Me.timeoutLabel.Text = "Timeout (seconds)"
        '
        'loopTimeLabel
        '
        Me.loopTimeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.loopTimeLabel.Location = New System.Drawing.Point(12, 195)
        Me.loopTimeLabel.Name = "loopTimeLabel"
        Me.loopTimeLabel.Size = New System.Drawing.Size(144, 16)
        Me.loopTimeLabel.TabIndex = 9
        Me.loopTimeLabel.Text = "Write Interval (seconds)"
        '
        'stopButton
        '
        Me.stopButton.Enabled = False
        Me.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.stopButton.Location = New System.Drawing.Point(380, 139)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.TabIndex = 1
        Me.stopButton.Text = "S&top"
        '
        'deviceNameLabel
        '
        Me.deviceNameLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.deviceNameLabel.Location = New System.Drawing.Point(12, 99)
        Me.deviceNameLabel.Name = "deviceNameLabel"
        Me.deviceNameLabel.Size = New System.Drawing.Size(112, 16)
        Me.deviceNameLabel.TabIndex = 5
        Me.deviceNameLabel.Text = "Device Name"
        '
        'writeInterval
        '
        Me.writeInterval.DecimalPlaces = 2
        Me.writeInterval.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.writeInterval.Location = New System.Drawing.Point(12, 212)
        Me.writeInterval.Name = "writeInterval"
        Me.writeInterval.Size = New System.Drawing.Size(152, 20)
        Me.writeInterval.TabIndex = 10
        Me.writeInterval.Value = New Decimal(New Integer() {5, 0, 0, 131072})
        '
        'physicalChannelComboBox
        '
        Me.physicalChannelComboBox.Location = New System.Drawing.Point(12, 28)
        Me.physicalChannelComboBox.Name = "physicalChannelComboBox"
        Me.physicalChannelComboBox.Size = New System.Drawing.Size(152, 21)
        Me.physicalChannelComboBox.TabIndex = 3
        Me.physicalChannelComboBox.Text = "Dev1/port0/line0:7"
        '
        'deviceComboBox
        '
        Me.deviceComboBox.Location = New System.Drawing.Point(12, 120)
        Me.deviceComboBox.Name = "deviceComboBox"
        Me.deviceComboBox.Size = New System.Drawing.Size(152, 21)
        Me.deviceComboBox.TabIndex = 6
        Me.deviceComboBox.Text = "Dev1"
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(472, 270)
        Me.Controls.Add(Me.deviceComboBox)
        Me.Controls.Add(Me.physicalChannelComboBox)
        Me.Controls.Add(Me.writeInterval)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.watchdogSettingsGroupBox)
        Me.Controls.Add(Me.warningText)
        Me.Controls.Add(Me.dataToWriteGroupBox)
        Me.Controls.Add(Me.physicalChannelToWriteLabel)
        Me.Controls.Add(Me.timeout)
        Me.Controls.Add(Me.timeoutLabel)
        Me.Controls.Add(Me.loopTimeLabel)
        Me.Controls.Add(Me.stopButton)
        Me.Controls.Add(Me.deviceNameLabel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Write Digital Channel - Watchdog Timer"
        Me.watchdogSettingsGroupBox.ResumeLayout(False)
        Me.dataToWriteGroupBox.ResumeLayout(False)
        CType(Me.writeInterval, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public Shared Sub Main()
        Application.Run(New MainForm)
    End Sub

    Private Sub DimControls(ByVal startOrStop As Boolean)
        physicalChannelComboBox.Enabled = Not startOrStop
        deviceComboBox.Enabled = Not startOrStop
        timeout.Enabled = Not startOrStop
        watchdogPhysicalChannelComboBox.Enabled = Not startOrStop
        watchdogExpirationState.Enabled = Not startOrStop
        startButton.Enabled = Not startOrStop
        stopButton.Enabled = startOrStop
    End Sub

    Private Function GetDataAsBoolArray() As Boolean()
        Return New Boolean() { _
            line0.Checked, _
            line1.Checked, _
            line2.Checked, _
            line3.Checked, _
            line4.Checked, _
            line5.Checked, _
            line6.Checked, _
            line7.Checked _
        }
    End Function

    Private Function GetWatchdogExpirationState() As WatchdogDOExpirationState
        Select Case watchdogExpirationState.Text
            Case "High"
                Return WatchdogDOExpirationState.High
            Case "Low"
                Return WatchdogDOExpirationState.Low
            Case "Tristate"
                Return WatchdogDOExpirationState.Tristate
            Case "No Change"
                Return WatchdogDOExpirationState.NoChange
            Case Else
                Throw New System.Exception("Unexpected value of watchdogExpiration field")
        End Select
    End Function

    Private Sub startButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles startButton.Click
        System.Windows.Forms.Cursor.Current = Cursors.WaitCursor
        Try
            ' Configure and start the output task
            outputTask = New Task()
            outputTask.DOChannels.CreateChannel( _
                physicalChannelComboBox.Text, _
                "myChannel", _
                ChannelLineGrouping.OneChannelForAllLines _
            )
            outputTask.Start()

            ' Configure and start the watchdog task
            watchdogTask = DaqSystem.Local.CreateWatchdogTimerTask( _
                "watchdogTask", _
                deviceComboBox.Text, _
                Double.Parse(timeout.Text), _
                New String() {watchdogPhysicalChannelComboBox.Text}, _
                New WatchdogDOExpirationState() {GetWatchdogExpirationState()} _
            )
            watchdogTask.Start()

            writer = New DigitalSingleChannelWriter(outputTask.Stream)
            runningTask = outputTask
            writer.BeginWriteSingleSampleMultiLine( _
                True, _
                GetDataAsBoolArray(), _
                New AsyncCallback(AddressOf OnDataWritten), _
                outputTask _
            )

            DimControls(True)
        Catch ex As System.Exception
            MessageBox.Show(ex.Message)

            If Not watchdogTask Is Nothing Then
                watchdogTask.Dispose()
                watchdogTask = Nothing
            End If

            If Not outputTask Is Nothing Then
                outputTask.Dispose()
                outputTask = Nothing
            End If

            System.Windows.Forms.Cursor.Current = Cursors.Default
        End Try
    End Sub

    Private Sub stopButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles stopButton.Click
        System.Windows.Forms.Cursor.Current = Cursors.WaitCursor
        Try
            runningTask = Nothing
            outputTask.Stop()
            outputTask.Dispose()
            outputTask = Nothing
            watchdogTask.Stop()
            watchdogTask.Dispose()
            watchdogTask = Nothing
        Catch ex As System.Exception
            MessageBox.Show(ex.Message)
        End Try

        System.Windows.Forms.Cursor.Current = Cursors.Default
        DimControls(False)
    End Sub

    Private Sub OnDataWritten(ByVal result As IAsyncResult)
        Try
            If (Not (runningTask Is Nothing)) AndAlso runningTask Is result.AsyncState Then
                writer.EndWrite(result)
                watchdogTask.Watchdog.ResetTimer()
                System.Threading.Thread.Sleep(CType(writeInterval.Value * 1000, Long))

                writer.BeginWriteSingleSampleMultiLine( _
                    True, _
                    GetDataAsBoolArray(), _
                    New AsyncCallback(AddressOf OnDataWritten), _
                    outputTask _
                )
            End If
        Catch ex As System.Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub MainForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        watchdogExpirationState.Text = "High"
    End Sub
End Class
