'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   GenDigPulse
'
' Category:
'   CO
'
' Description:
'   This example demonstrates how to generate a single digital pulse from a
'   counter output channel.  The initial delay, high time, low time, and idle
'   state are all software configurable. This example shows how to configure the
'   pulse in terms of time, but can easily be modified to generate a pulse in
'   terms of frequency and duty cycle or ticks.
'
' Instructions for running:
'   1.  Enter the physical channel which corresponds to the counter you want to
'       use to output your signal to on the DAQ device.
'   2.  Enter the low time and high time (in seconds) to define the pulse
'       parameters. Additionally, you can set the initial delay (in seconds)
'       which will delay the beginning of the pulse from the start call. Also,
'       you can change the idle state to generate a high or low pulse.Note: Use
'       the MeasPulseWidth example to verify you are outputting a pulse on the
'       DAQ device.
'
' Steps:
'   1.  Create a counter output channel using
'       Task.COChannels.CreatePulseChannelTime to produce a pulse in terms of
'       time. If the idle state of the pulse is set to low the first transition
'       of the generated signal is from low to high.
'   2.  Call Task.Start to arm the counter and begin the pulse generation.  The
'       pulse will not begin until after the initial delay (in seconds) has
'       expired.
'   3.  Use Task.WaitUntilDone to ensure the entire pulse is generated before
'       ending the task.
'   4.  Dispose the Task object to clean-up any resources associated with the
'       task.
'   5.  Handle any DaqExceptions, if they occur.
'
' I/O Connections Overview:
'   This example will cause the counter to output the pulse on the output
'   terminal of the counter specified. The default counter terminal(s) depend on
'   the type of measurement being taken. For more information on the default
'   counter input and output terminals for your device, open the NI-DAQmx Help,
'   and refer to Counter Signal Connections found under the Device
'   Considerations book in the table of contents.
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

Imports System.Threading
Imports NationalInstruments.DAQmx

Public Delegate Sub FormDelegate()

Public Class MainForm
    Inherits System.Windows.Forms.Form
    Private myTask As Task
    Private idleStates As COPulseIdleState

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()
        Application.EnableVisualStyles()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        idleStates = COPulseIdleState.Low

        counterComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.CO, PhysicalChannelAccess.External))
        If (counterComboBox.Items.Count > 0) Then
            counterComboBox.SelectedIndex = 0
        End If

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents startButton As System.Windows.Forms.Button
    Friend WithEvents channelParamGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents idleStateGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents highRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents lowRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents iniDelayLabel As System.Windows.Forms.Label
    Friend WithEvents iniDelayTextBox As System.Windows.Forms.TextBox
    Friend WithEvents highTimeLabel As System.Windows.Forms.Label
    Friend WithEvents highTimeTextBox As System.Windows.Forms.TextBox
    Friend WithEvents lowTimeLabel As System.Windows.Forms.Label
    Friend WithEvents lowTimeTextBox As System.Windows.Forms.TextBox
    Friend WithEvents physicalChannelLabel As System.Windows.Forms.Label
    Friend WithEvents counterComboBox As System.Windows.Forms.ComboBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MainForm))
        Me.startButton = New System.Windows.Forms.Button
        Me.channelParamGroupBox = New System.Windows.Forms.GroupBox
        Me.counterComboBox = New System.Windows.Forms.ComboBox
        Me.idleStateGroupBox = New System.Windows.Forms.GroupBox
        Me.highRadioButton = New System.Windows.Forms.RadioButton
        Me.lowRadioButton = New System.Windows.Forms.RadioButton
        Me.iniDelayLabel = New System.Windows.Forms.Label
        Me.iniDelayTextBox = New System.Windows.Forms.TextBox
        Me.highTimeLabel = New System.Windows.Forms.Label
        Me.highTimeTextBox = New System.Windows.Forms.TextBox
        Me.lowTimeLabel = New System.Windows.Forms.Label
        Me.lowTimeTextBox = New System.Windows.Forms.TextBox
        Me.physicalChannelLabel = New System.Windows.Forms.Label
        Me.channelParamGroupBox.SuspendLayout()
        Me.idleStateGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'startButton
        '
        Me.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.startButton.Location = New System.Drawing.Point(152, 16)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(104, 40)
        Me.startButton.TabIndex = 0
        Me.startButton.Text = "Generate Pulse"
        '
        'channelParamGroupBox
        '
        Me.channelParamGroupBox.Controls.Add(Me.counterComboBox)
        Me.channelParamGroupBox.Controls.Add(Me.idleStateGroupBox)
        Me.channelParamGroupBox.Controls.Add(Me.iniDelayLabel)
        Me.channelParamGroupBox.Controls.Add(Me.iniDelayTextBox)
        Me.channelParamGroupBox.Controls.Add(Me.highTimeLabel)
        Me.channelParamGroupBox.Controls.Add(Me.highTimeTextBox)
        Me.channelParamGroupBox.Controls.Add(Me.lowTimeLabel)
        Me.channelParamGroupBox.Controls.Add(Me.lowTimeTextBox)
        Me.channelParamGroupBox.Controls.Add(Me.physicalChannelLabel)
        Me.channelParamGroupBox.Location = New System.Drawing.Point(8, 8)
        Me.channelParamGroupBox.Name = "channelParamGroupBox"
        Me.channelParamGroupBox.Size = New System.Drawing.Size(136, 304)
        Me.channelParamGroupBox.TabIndex = 1
        Me.channelParamGroupBox.TabStop = False
        Me.channelParamGroupBox.Text = "Channel Parameters"
        '
        'counterComboBox
        '
        Me.counterComboBox.Location = New System.Drawing.Point(16, 40)
        Me.counterComboBox.Name = "counterComboBox"
        Me.counterComboBox.Size = New System.Drawing.Size(104, 21)
        Me.counterComboBox.TabIndex = 1
        Me.counterComboBox.Text = "Dev1/ctr0"
        '
        'idleStateGroupBox
        '
        Me.idleStateGroupBox.Controls.Add(Me.highRadioButton)
        Me.idleStateGroupBox.Controls.Add(Me.lowRadioButton)
        Me.idleStateGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.idleStateGroupBox.Location = New System.Drawing.Point(16, 216)
        Me.idleStateGroupBox.Name = "idleStateGroupBox"
        Me.idleStateGroupBox.Size = New System.Drawing.Size(104, 72)
        Me.idleStateGroupBox.TabIndex = 8
        Me.idleStateGroupBox.TabStop = False
        Me.idleStateGroupBox.Text = "Idle State:"
        '
        'highRadioButton
        '
        Me.highRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.highRadioButton.Location = New System.Drawing.Point(16, 40)
        Me.highRadioButton.Name = "highRadioButton"
        Me.highRadioButton.Size = New System.Drawing.Size(64, 24)
        Me.highRadioButton.TabIndex = 1
        Me.highRadioButton.Text = "High"
        '
        'lowRadioButton
        '
        Me.lowRadioButton.Checked = True
        Me.lowRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.lowRadioButton.Location = New System.Drawing.Point(16, 16)
        Me.lowRadioButton.Name = "lowRadioButton"
        Me.lowRadioButton.Size = New System.Drawing.Size(64, 24)
        Me.lowRadioButton.TabIndex = 0
        Me.lowRadioButton.TabStop = True
        Me.lowRadioButton.Text = "Low"
        '
        'iniDelayLabel
        '
        Me.iniDelayLabel.BackColor = System.Drawing.SystemColors.Control
        Me.iniDelayLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.iniDelayLabel.Location = New System.Drawing.Point(16, 168)
        Me.iniDelayLabel.Name = "iniDelayLabel"
        Me.iniDelayLabel.Size = New System.Drawing.Size(100, 16)
        Me.iniDelayLabel.TabIndex = 6
        Me.iniDelayLabel.Text = "Initial Delay (sec):"
        '
        'iniDelayTextBox
        '
        Me.iniDelayTextBox.Location = New System.Drawing.Point(16, 184)
        Me.iniDelayTextBox.Name = "iniDelayTextBox"
        Me.iniDelayTextBox.Size = New System.Drawing.Size(104, 20)
        Me.iniDelayTextBox.TabIndex = 7
        Me.iniDelayTextBox.Text = "1.00"
        '
        'highTimeLabel
        '
        Me.highTimeLabel.BackColor = System.Drawing.SystemColors.Control
        Me.highTimeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.highTimeLabel.Location = New System.Drawing.Point(16, 120)
        Me.highTimeLabel.Name = "highTimeLabel"
        Me.highTimeLabel.Size = New System.Drawing.Size(100, 16)
        Me.highTimeLabel.TabIndex = 4
        Me.highTimeLabel.Text = "High Time (sec):"
        '
        'highTimeTextBox
        '
        Me.highTimeTextBox.Location = New System.Drawing.Point(16, 136)
        Me.highTimeTextBox.Name = "highTimeTextBox"
        Me.highTimeTextBox.Size = New System.Drawing.Size(104, 20)
        Me.highTimeTextBox.TabIndex = 5
        Me.highTimeTextBox.Text = "1.00"
        '
        'lowTimeLabel
        '
        Me.lowTimeLabel.BackColor = System.Drawing.SystemColors.Control
        Me.lowTimeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.lowTimeLabel.Location = New System.Drawing.Point(16, 72)
        Me.lowTimeLabel.Name = "lowTimeLabel"
        Me.lowTimeLabel.Size = New System.Drawing.Size(100, 16)
        Me.lowTimeLabel.TabIndex = 2
        Me.lowTimeLabel.Text = "Low Time (sec):"
        '
        'lowTimeTextBox
        '
        Me.lowTimeTextBox.Location = New System.Drawing.Point(16, 88)
        Me.lowTimeTextBox.Name = "lowTimeTextBox"
        Me.lowTimeTextBox.Size = New System.Drawing.Size(104, 20)
        Me.lowTimeTextBox.TabIndex = 3
        Me.lowTimeTextBox.Text = "0.50"
        '
        'physicalChannelLabel
        '
        Me.physicalChannelLabel.BackColor = System.Drawing.SystemColors.Control
        Me.physicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.physicalChannelLabel.Location = New System.Drawing.Point(16, 24)
        Me.physicalChannelLabel.Name = "physicalChannelLabel"
        Me.physicalChannelLabel.Size = New System.Drawing.Size(100, 16)
        Me.physicalChannelLabel.TabIndex = 0
        Me.physicalChannelLabel.Text = "Physical Channel:"
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(266, 320)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.channelParamGroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Generate Digital Pulse"
        Me.channelParamGroupBox.ResumeLayout(False)
        Me.idleStateGroupBox.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub startButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles startButton.Click
        ' This example uses the default source (or gate) terminal for 
        ' the counter of your device.  To determine what the default 
        ' counter pins for your device are or to set a different source 
        ' (or gate) pin, refer to the Connecting Counter Signals topic
        ' in the NI-DAQmx Help (search for "Connecting Counter Signals").

        Try
            myTask = New Task()

            myTask.COChannels.CreatePulseChannelTime(counterComboBox.Text, _
                "GenerateDigitalPulse", COPulseTimeUnits.Seconds, idleStates, _
                Convert.ToDouble(iniDelayTextBox.Text), _
                Convert.ToDouble(lowTimeTextBox.Text), _
                Convert.ToDouble(highTimeTextBox.Text))

            myTask.Start()

            startButton.Enabled = False

            'Wait for the task to complete in another thread so the UI does not
            'freeze
            ThreadPool.QueueUserWorkItem(New WaitCallback(AddressOf WaitMethod))

        Catch exception As DaqException
            MessageBox.Show(exception.Message)
            myTask.Dispose()
            startButton.Enabled = True
        End Try
    End Sub

    Private Sub highRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles highRadioButton.CheckedChanged
        idleStates = COPulseIdleState.High
    End Sub

    Private Sub lowRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lowRadioButton.CheckedChanged
        idleStates = COPulseIdleState.Low
    End Sub

    Private Sub WaitMethod(ByVal obj As Object)
        myTask.WaitUntilDone(-1)
        myTask.Stop()
        myTask.Dispose()

        Dim d As FormDelegate = New FormDelegate(AddressOf EnableStart)
        Invoke(d)
    End Sub

    Private Sub EnableStart()
        startButton.Enabled = True
    End Sub
End Class
