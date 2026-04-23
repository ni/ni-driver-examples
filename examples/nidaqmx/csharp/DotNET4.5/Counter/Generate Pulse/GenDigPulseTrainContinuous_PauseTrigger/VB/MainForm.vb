'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   GenDigPulseTrainContinuous_PauseTrigger
'
' Category:
'   CO
'
' Description:
'   This example demonstrates how to generate a continuous digital pulse train
'   from a counter output channel and controlled by an external digital pause
'   trigger.  The frequency, duty cycle, and idle state are all
'   configurable.This example shows how to configure the pulse in terms of
'   frequency and duty cycle, but can easily be modified to generate a pulse in
'   terms of time or ticks.
'
' Instructions for running:
'   1.  Enter the physical channel which corresponds to the counter you want to
'       output your signal to on the DAQ device.
'   2.  Enter the frequency and duty cycle to define the pulse parameters. You
'       can also change the idle state to set the state the line will remain in
'       after the generation is stopped.
'   3.  Setup the pause trigger parameters.
'
' Steps:
'   1.  Create a counter output channel to produce a pulse in terms of
'       frequency.  If the idle state of the pulse is set to low the first
'       transition of the generated signal is from low to high.
'   2.  Use the PauseTrigger object properties to configure a pause trigger.
'   3.  Use the ConfigureImplicit() method to configure the duration of the
'       pulse generation.
'   4.  Call the Start() method to arm the counter and begin the pulse train
'       generation.
'   5.  This example will continue to generate the pulse train until the Stop
'       button is  pressed on the user interface.
'   6.  Call Stop() to stop the task and Dispose() to to clear any resources
'       allocated by the task.
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

Imports NationalInstruments.DAQmx

Public Class MainForm
    Inherits System.Windows.Forms.Form
    Private myTask As Task
    Private idleState As COPulseIdleState
    Private pauseCondition As DigitalLevelPauseTriggerCondition

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()
        Application.EnableVisualStyles()

        'This call is required by the Windows Form Designer.
        InitializeComponent()
        'Add any initialization after the InitializeComponent() call
        idleState = COPulseIdleState.Low
        pauseCondition = DigitalLevelPauseTriggerCondition.Low

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
    Friend WithEvents stopButton As System.Windows.Forms.Button
    Friend WithEvents triggerParameterGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents channelParameterGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents dutyCycleTextBox As System.Windows.Forms.TextBox
    Friend WithEvents dutyCycleLabel As System.Windows.Forms.Label
    Friend WithEvents frequencyTextBox As System.Windows.Forms.TextBox
    Friend WithEvents frequencyLabel As System.Windows.Forms.Label
    Friend WithEvents physicalChannelLabel As System.Windows.Forms.Label
    Friend WithEvents startButton As System.Windows.Forms.Button
    Friend WithEvents idleStateGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents triggerSourceLabel As System.Windows.Forms.Label
    Friend WithEvents pauseTrigSourceTextBox As System.Windows.Forms.TextBox
    Friend WithEvents idleStateHighRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents idleStateLowRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents pauseConditionGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents pauseWhenHighRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents pauseWhenLowRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents statusCheckTimer As System.Windows.Forms.Timer
    Friend WithEvents counterComboBox As System.Windows.Forms.ComboBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MainForm))
        Me.stopButton = New System.Windows.Forms.Button
        Me.triggerParameterGroupBox = New System.Windows.Forms.GroupBox
        Me.pauseConditionGroupBox = New System.Windows.Forms.GroupBox
        Me.pauseWhenHighRadioButton = New System.Windows.Forms.RadioButton
        Me.pauseWhenLowRadioButton = New System.Windows.Forms.RadioButton
        Me.triggerSourceLabel = New System.Windows.Forms.Label
        Me.pauseTrigSourceTextBox = New System.Windows.Forms.TextBox
        Me.channelParameterGroupBox = New System.Windows.Forms.GroupBox
        Me.counterComboBox = New System.Windows.Forms.ComboBox
        Me.idleStateGroupBox = New System.Windows.Forms.GroupBox
        Me.idleStateHighRadioButton = New System.Windows.Forms.RadioButton
        Me.idleStateLowRadioButton = New System.Windows.Forms.RadioButton
        Me.dutyCycleTextBox = New System.Windows.Forms.TextBox
        Me.dutyCycleLabel = New System.Windows.Forms.Label
        Me.frequencyTextBox = New System.Windows.Forms.TextBox
        Me.frequencyLabel = New System.Windows.Forms.Label
        Me.physicalChannelLabel = New System.Windows.Forms.Label
        Me.startButton = New System.Windows.Forms.Button
        Me.statusCheckTimer = New System.Windows.Forms.Timer(Me.components)
        Me.triggerParameterGroupBox.SuspendLayout()
        Me.pauseConditionGroupBox.SuspendLayout()
        Me.channelParameterGroupBox.SuspendLayout()
        Me.idleStateGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'stopButton
        '
        Me.stopButton.Enabled = False
        Me.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.stopButton.Location = New System.Drawing.Point(240, 224)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(96, 32)
        Me.stopButton.TabIndex = 1
        Me.stopButton.Text = "Stop"
        '
        'triggerParameterGroupBox
        '
        Me.triggerParameterGroupBox.Controls.Add(Me.pauseConditionGroupBox)
        Me.triggerParameterGroupBox.Controls.Add(Me.triggerSourceLabel)
        Me.triggerParameterGroupBox.Controls.Add(Me.pauseTrigSourceTextBox)
        Me.triggerParameterGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.triggerParameterGroupBox.Location = New System.Drawing.Point(200, 8)
        Me.triggerParameterGroupBox.Name = "triggerParameterGroupBox"
        Me.triggerParameterGroupBox.Size = New System.Drawing.Size(176, 160)
        Me.triggerParameterGroupBox.TabIndex = 3
        Me.triggerParameterGroupBox.TabStop = False
        Me.triggerParameterGroupBox.Text = "Trigger Parameters:"
        '
        'pauseConditionGroupBox
        '
        Me.pauseConditionGroupBox.Controls.Add(Me.pauseWhenHighRadioButton)
        Me.pauseConditionGroupBox.Controls.Add(Me.pauseWhenLowRadioButton)
        Me.pauseConditionGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.pauseConditionGroupBox.Location = New System.Drawing.Point(16, 80)
        Me.pauseConditionGroupBox.Name = "pauseConditionGroupBox"
        Me.pauseConditionGroupBox.Size = New System.Drawing.Size(144, 64)
        Me.pauseConditionGroupBox.TabIndex = 2
        Me.pauseConditionGroupBox.TabStop = False
        Me.pauseConditionGroupBox.Text = "Pause When:"
        '
        'pauseWhenHighRadioButton
        '
        Me.pauseWhenHighRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.pauseWhenHighRadioButton.Location = New System.Drawing.Point(19, 38)
        Me.pauseWhenHighRadioButton.Name = "pauseWhenHighRadioButton"
        Me.pauseWhenHighRadioButton.Size = New System.Drawing.Size(56, 16)
        Me.pauseWhenHighRadioButton.TabIndex = 1
        Me.pauseWhenHighRadioButton.Text = "High"
        '
        'pauseWhenLowRadioButton
        '
        Me.pauseWhenLowRadioButton.Checked = True
        Me.pauseWhenLowRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.pauseWhenLowRadioButton.Location = New System.Drawing.Point(19, 22)
        Me.pauseWhenLowRadioButton.Name = "pauseWhenLowRadioButton"
        Me.pauseWhenLowRadioButton.Size = New System.Drawing.Size(56, 16)
        Me.pauseWhenLowRadioButton.TabIndex = 0
        Me.pauseWhenLowRadioButton.TabStop = True
        Me.pauseWhenLowRadioButton.Text = "Low"
        '
        'triggerSourceLabel
        '
        Me.triggerSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.triggerSourceLabel.Location = New System.Drawing.Point(16, 28)
        Me.triggerSourceLabel.Name = "triggerSourceLabel"
        Me.triggerSourceLabel.Size = New System.Drawing.Size(128, 16)
        Me.triggerSourceLabel.TabIndex = 0
        Me.triggerSourceLabel.Text = "Pause Trigger Source:"
        '
        'pauseTrigSourceTextBox
        '
        Me.pauseTrigSourceTextBox.Location = New System.Drawing.Point(16, 48)
        Me.pauseTrigSourceTextBox.Name = "pauseTrigSourceTextBox"
        Me.pauseTrigSourceTextBox.Size = New System.Drawing.Size(144, 20)
        Me.pauseTrigSourceTextBox.TabIndex = 1
        Me.pauseTrigSourceTextBox.Text = "/Dev1/PFI0"
        '
        'channelParameterGroupBox
        '
        Me.channelParameterGroupBox.Controls.Add(Me.counterComboBox)
        Me.channelParameterGroupBox.Controls.Add(Me.idleStateGroupBox)
        Me.channelParameterGroupBox.Controls.Add(Me.dutyCycleTextBox)
        Me.channelParameterGroupBox.Controls.Add(Me.dutyCycleLabel)
        Me.channelParameterGroupBox.Controls.Add(Me.frequencyTextBox)
        Me.channelParameterGroupBox.Controls.Add(Me.frequencyLabel)
        Me.channelParameterGroupBox.Controls.Add(Me.physicalChannelLabel)
        Me.channelParameterGroupBox.Location = New System.Drawing.Point(8, 8)
        Me.channelParameterGroupBox.Name = "channelParameterGroupBox"
        Me.channelParameterGroupBox.Size = New System.Drawing.Size(184, 272)
        Me.channelParameterGroupBox.TabIndex = 2
        Me.channelParameterGroupBox.TabStop = False
        Me.channelParameterGroupBox.Text = "Channel Parameters:"
        '
        'counterComboBox
        '
        Me.counterComboBox.Location = New System.Drawing.Point(16, 48)
        Me.counterComboBox.Name = "counterComboBox"
        Me.counterComboBox.Size = New System.Drawing.Size(152, 21)
        Me.counterComboBox.TabIndex = 1
        Me.counterComboBox.Text = "Dev1/ctr0"
        '
        'idleStateGroupBox
        '
        Me.idleStateGroupBox.Controls.Add(Me.idleStateHighRadioButton)
        Me.idleStateGroupBox.Controls.Add(Me.idleStateLowRadioButton)
        Me.idleStateGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.idleStateGroupBox.Location = New System.Drawing.Point(16, 192)
        Me.idleStateGroupBox.Name = "idleStateGroupBox"
        Me.idleStateGroupBox.Size = New System.Drawing.Size(152, 64)
        Me.idleStateGroupBox.TabIndex = 6
        Me.idleStateGroupBox.TabStop = False
        Me.idleStateGroupBox.Text = "Idle State:"
        '
        'idleStateHighRadioButton
        '
        Me.idleStateHighRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.idleStateHighRadioButton.Location = New System.Drawing.Point(19, 39)
        Me.idleStateHighRadioButton.Name = "idleStateHighRadioButton"
        Me.idleStateHighRadioButton.Size = New System.Drawing.Size(56, 16)
        Me.idleStateHighRadioButton.TabIndex = 1
        Me.idleStateHighRadioButton.Text = "High"
        '
        'idleStateLowRadioButton
        '
        Me.idleStateLowRadioButton.Checked = True
        Me.idleStateLowRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.idleStateLowRadioButton.Location = New System.Drawing.Point(19, 22)
        Me.idleStateLowRadioButton.Name = "idleStateLowRadioButton"
        Me.idleStateLowRadioButton.Size = New System.Drawing.Size(56, 16)
        Me.idleStateLowRadioButton.TabIndex = 0
        Me.idleStateLowRadioButton.TabStop = True
        Me.idleStateLowRadioButton.Text = "Low"
        '
        'dutyCycleTextBox
        '
        Me.dutyCycleTextBox.Location = New System.Drawing.Point(16, 159)
        Me.dutyCycleTextBox.Name = "dutyCycleTextBox"
        Me.dutyCycleTextBox.Size = New System.Drawing.Size(152, 20)
        Me.dutyCycleTextBox.TabIndex = 5
        Me.dutyCycleTextBox.Text = "0.5"
        '
        'dutyCycleLabel
        '
        Me.dutyCycleLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.dutyCycleLabel.Location = New System.Drawing.Point(16, 139)
        Me.dutyCycleLabel.Name = "dutyCycleLabel"
        Me.dutyCycleLabel.Size = New System.Drawing.Size(104, 16)
        Me.dutyCycleLabel.TabIndex = 4
        Me.dutyCycleLabel.Text = "Duty Cycle:"
        '
        'frequencyTextBox
        '
        Me.frequencyTextBox.Location = New System.Drawing.Point(16, 103)
        Me.frequencyTextBox.Name = "frequencyTextBox"
        Me.frequencyTextBox.Size = New System.Drawing.Size(152, 20)
        Me.frequencyTextBox.TabIndex = 3
        Me.frequencyTextBox.Text = "1.0"
        '
        'frequencyLabel
        '
        Me.frequencyLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.frequencyLabel.Location = New System.Drawing.Point(16, 83)
        Me.frequencyLabel.Name = "frequencyLabel"
        Me.frequencyLabel.Size = New System.Drawing.Size(104, 16)
        Me.frequencyLabel.TabIndex = 2
        Me.frequencyLabel.Text = "Frequency (Hz):"
        '
        'physicalChannelLabel
        '
        Me.physicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.physicalChannelLabel.Location = New System.Drawing.Point(16, 28)
        Me.physicalChannelLabel.Name = "physicalChannelLabel"
        Me.physicalChannelLabel.Size = New System.Drawing.Size(104, 16)
        Me.physicalChannelLabel.TabIndex = 0
        Me.physicalChannelLabel.Text = "Counter(s):"
        '
        'startButton
        '
        Me.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.startButton.Location = New System.Drawing.Point(240, 192)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(96, 32)
        Me.startButton.TabIndex = 0
        Me.startButton.Text = "Start"
        '
        'statusCheckTimer
        '
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(386, 288)
        Me.Controls.Add(Me.stopButton)
        Me.Controls.Add(Me.triggerParameterGroupBox)
        Me.Controls.Add(Me.channelParameterGroupBox)
        Me.Controls.Add(Me.startButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Gen Dig Pulse Train-Continuous-Pause Trigger"
        Me.triggerParameterGroupBox.ResumeLayout(False)
        Me.pauseConditionGroupBox.ResumeLayout(False)
        Me.channelParameterGroupBox.ResumeLayout(False)
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

            myTask.COChannels.CreatePulseChannelFrequency(counterComboBox.Text, _
                "PulseGenPauseTrigger", COPulseFrequencyUnits.Hertz, idleState, 0.0, _
                Convert.ToDouble(frequencyTextBox.Text), _
                Convert.ToDouble(dutyCycleTextBox.Text))

            myTask.Triggers.PauseTrigger.ConfigureDigitalLevelTrigger( _
                pauseTrigSourceTextBox.Text, pauseCondition)

            myTask.Timing.ConfigureImplicit(SampleQuantityMode.ContinuousSamples, 1000)

            myTask.Start()

            startButton.Enabled = False
            stopButton.Enabled = True

            statusCheckTimer.Enabled = True

        Catch exception As System.Exception

            statusCheckTimer.Enabled = False
            MessageBox.Show(exception.Message)
            myTask.Dispose()
            startButton.Enabled = True
            stopButton.Enabled = False

        End Try

        
    End Sub

    Private Sub stopButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles stopButton.Click

        statusCheckTimer.Enabled = False
        myTask.Stop()
        myTask.Dispose()
        startButton.Enabled = True
        stopButton.Enabled = False

    End Sub

    Private Sub lowRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles idleStateLowRadioButton.CheckedChanged
        idleState = COPulseIdleState.Low
    End Sub

    Private Sub highRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles idleStateHighRadioButton.CheckedChanged
        idleState = COPulseIdleState.High
    End Sub

    Private Sub pauseWhenLowRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pauseWhenLowRadioButton.CheckedChanged
        pauseCondition = DigitalLevelPauseTriggerCondition.Low
    End Sub

    Private Sub pauseWhenHighRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pauseWhenHighRadioButton.CheckedChanged
        pauseCondition = DigitalLevelPauseTriggerCondition.High
    End Sub

    Private Sub statusCheckTimer_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles statusCheckTimer.Tick
        Try
            'Getting myTask.IsDone also checks for errors that would prematurely
            'halt the continuous generation.
            If (myTask.IsDone) Then
                statusCheckTimer.Enabled = False
                myTask.Stop()
                myTask.Dispose()
                startButton.Enabled = True
                stopButton.Enabled = False
            End If
        Catch ex As DaqException
            statusCheckTimer.Enabled = False
            System.Windows.Forms.MessageBox.Show(ex.Message)
            myTask.Stop()
            myTask.Dispose()
            startButton.Enabled = True
            stopButton.Enabled = False
        End Try
    End Sub
End Class
