'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   GenDigPulseTrainContinuous_DigStart
'
' Category:
'   CO
'
' Description:
'   This example demonstrates how to generate a continuous digital pulse train
'   from a counter output channel using a digital start trigger. The frequency,
'   duty cycle, and idle state are all configurable.This example shows how to
'   configure the pulse in terms of frequency and duty cycle, but it can easily
'   be modified to generate a pulse in terms of time or ticks.
'
' Instructions for running:
'   1.  Enter the physical channel which corresponds to the counter you want to
'       output your signal to on the DAQ device.
'   2.  Enter the frequency and duty cycle to define the pulse parameters. You
'       can also change the idle state to set the state the line will remain in
'       after the generation is stopped.
'   3.  Setup the trigger parameters.  The default in this example is a rising
'       edge digital trigger on PFI9 (the default gate of ctr0).
'
' Steps:
'   1.  Create a Task object. Create a COChannel object for pulse generation in
'       terms of frequency. If the idle state of the pulse is set to low the
'       first transition of the generated signal is from low to high.
'   2.  Configure the Triggers object to have pulse generation start on a
'       digital edge.
'   3.  Use the ConfigureImplicit() method to configure the duration of the
'       pulse generation.
'   4.  Call the Start() method to arm the counter and begin waiting for the
'       trigger to start the pulse train generation.
'   5.  Call the Stop() method to stop the task and Dispose() method to
'       de-allocate any resources used by the Task.
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
    Private triggerEdge As DigitalEdgeStartTriggerEdge

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()
        Application.EnableVisualStyles()
        Application.DoEvents()

        'This call is required by the Windows Form Designer.
        InitializeComponent()
        'Add any initialization after the InitializeComponent() call
        triggerEdge = DigitalEdgeStartTriggerEdge.Rising
        idleState = COPulseIdleState.Low

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
    Friend WithEvents idleStateLabel As System.Windows.Forms.Label
    Friend WithEvents dutyCycleTextBox As System.Windows.Forms.TextBox
    Friend WithEvents dutyCycleLabel As System.Windows.Forms.Label
    Friend WithEvents frequencyTextBox As System.Windows.Forms.TextBox
    Friend WithEvents frequencyLabel As System.Windows.Forms.Label
    Friend WithEvents startButton As System.Windows.Forms.Button
    Friend WithEvents edgeLabel As System.Windows.Forms.Label
    Friend WithEvents triggerSourceLabel As System.Windows.Forms.Label
    Friend WithEvents counterLabel As System.Windows.Forms.Label
    Friend WithEvents triggerSourceTextBox As System.Windows.Forms.TextBox
    Friend WithEvents lowRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents highRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents fallingRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents risingRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents statusCheckTimer As System.Windows.Forms.Timer
    Friend WithEvents counterComboBox As System.Windows.Forms.ComboBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MainForm))
        Me.stopButton = New System.Windows.Forms.Button
        Me.triggerParameterGroupBox = New System.Windows.Forms.GroupBox
        Me.fallingRadioButton = New System.Windows.Forms.RadioButton
        Me.risingRadioButton = New System.Windows.Forms.RadioButton
        Me.triggerSourceLabel = New System.Windows.Forms.Label
        Me.edgeLabel = New System.Windows.Forms.Label
        Me.triggerSourceTextBox = New System.Windows.Forms.TextBox
        Me.channelParameterGroupBox = New System.Windows.Forms.GroupBox
        Me.counterComboBox = New System.Windows.Forms.ComboBox
        Me.lowRadioButton = New System.Windows.Forms.RadioButton
        Me.highRadioButton = New System.Windows.Forms.RadioButton
        Me.idleStateLabel = New System.Windows.Forms.Label
        Me.dutyCycleTextBox = New System.Windows.Forms.TextBox
        Me.dutyCycleLabel = New System.Windows.Forms.Label
        Me.frequencyTextBox = New System.Windows.Forms.TextBox
        Me.frequencyLabel = New System.Windows.Forms.Label
        Me.counterLabel = New System.Windows.Forms.Label
        Me.startButton = New System.Windows.Forms.Button
        Me.statusCheckTimer = New System.Windows.Forms.Timer(Me.components)
        Me.triggerParameterGroupBox.SuspendLayout()
        Me.channelParameterGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'stopButton
        '
        Me.stopButton.Enabled = False
        Me.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.stopButton.Location = New System.Drawing.Point(208, 208)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(112, 32)
        Me.stopButton.TabIndex = 1
        Me.stopButton.Text = "Stop"
        '
        'triggerParameterGroupBox
        '
        Me.triggerParameterGroupBox.Controls.Add(Me.fallingRadioButton)
        Me.triggerParameterGroupBox.Controls.Add(Me.risingRadioButton)
        Me.triggerParameterGroupBox.Controls.Add(Me.triggerSourceLabel)
        Me.triggerParameterGroupBox.Controls.Add(Me.edgeLabel)
        Me.triggerParameterGroupBox.Controls.Add(Me.triggerSourceTextBox)
        Me.triggerParameterGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.triggerParameterGroupBox.Location = New System.Drawing.Point(184, 8)
        Me.triggerParameterGroupBox.Name = "triggerParameterGroupBox"
        Me.triggerParameterGroupBox.Size = New System.Drawing.Size(160, 152)
        Me.triggerParameterGroupBox.TabIndex = 3
        Me.triggerParameterGroupBox.TabStop = False
        Me.triggerParameterGroupBox.Text = "Trigger Parameters"
        '
        'fallingRadioButton
        '
        Me.fallingRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.fallingRadioButton.Location = New System.Drawing.Point(40, 112)
        Me.fallingRadioButton.Name = "fallingRadioButton"
        Me.fallingRadioButton.Size = New System.Drawing.Size(88, 24)
        Me.fallingRadioButton.TabIndex = 4
        Me.fallingRadioButton.Text = "Falling"
        '
        'risingRadioButton
        '
        Me.risingRadioButton.Checked = True
        Me.risingRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.risingRadioButton.Location = New System.Drawing.Point(40, 88)
        Me.risingRadioButton.Name = "risingRadioButton"
        Me.risingRadioButton.Size = New System.Drawing.Size(88, 24)
        Me.risingRadioButton.TabIndex = 3
        Me.risingRadioButton.TabStop = True
        Me.risingRadioButton.Text = "Rising"
        '
        'triggerSourceLabel
        '
        Me.triggerSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.triggerSourceLabel.Location = New System.Drawing.Point(16, 24)
        Me.triggerSourceLabel.Name = "triggerSourceLabel"
        Me.triggerSourceLabel.Size = New System.Drawing.Size(96, 16)
        Me.triggerSourceLabel.TabIndex = 0
        Me.triggerSourceLabel.Text = "Trigger Source:"
        '
        'edgeLabel
        '
        Me.edgeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.edgeLabel.Location = New System.Drawing.Point(16, 71)
        Me.edgeLabel.Name = "edgeLabel"
        Me.edgeLabel.Size = New System.Drawing.Size(100, 16)
        Me.edgeLabel.TabIndex = 2
        Me.edgeLabel.Text = "Edge:"
        '
        'triggerSourceTextBox
        '
        Me.triggerSourceTextBox.Location = New System.Drawing.Point(16, 40)
        Me.triggerSourceTextBox.Name = "triggerSourceTextBox"
        Me.triggerSourceTextBox.Size = New System.Drawing.Size(128, 20)
        Me.triggerSourceTextBox.TabIndex = 1
        Me.triggerSourceTextBox.Text = "/Dev1/PFI9"
        '
        'channelParameterGroupBox
        '
        Me.channelParameterGroupBox.Controls.Add(Me.counterComboBox)
        Me.channelParameterGroupBox.Controls.Add(Me.lowRadioButton)
        Me.channelParameterGroupBox.Controls.Add(Me.highRadioButton)
        Me.channelParameterGroupBox.Controls.Add(Me.idleStateLabel)
        Me.channelParameterGroupBox.Controls.Add(Me.dutyCycleTextBox)
        Me.channelParameterGroupBox.Controls.Add(Me.dutyCycleLabel)
        Me.channelParameterGroupBox.Controls.Add(Me.frequencyTextBox)
        Me.channelParameterGroupBox.Controls.Add(Me.frequencyLabel)
        Me.channelParameterGroupBox.Controls.Add(Me.counterLabel)
        Me.channelParameterGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.channelParameterGroupBox.Location = New System.Drawing.Point(8, 8)
        Me.channelParameterGroupBox.Name = "channelParameterGroupBox"
        Me.channelParameterGroupBox.Size = New System.Drawing.Size(160, 248)
        Me.channelParameterGroupBox.TabIndex = 2
        Me.channelParameterGroupBox.TabStop = False
        Me.channelParameterGroupBox.Text = "Channel Parameters"
        '
        'counterComboBox
        '
        Me.counterComboBox.Location = New System.Drawing.Point(16, 40)
        Me.counterComboBox.Name = "counterComboBox"
        Me.counterComboBox.Size = New System.Drawing.Size(128, 21)
        Me.counterComboBox.TabIndex = 1
        Me.counterComboBox.Text = "Dev1/ctr0"
        '
        'lowRadioButton
        '
        Me.lowRadioButton.Checked = True
        Me.lowRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.lowRadioButton.Location = New System.Drawing.Point(40, 184)
        Me.lowRadioButton.Name = "lowRadioButton"
        Me.lowRadioButton.Size = New System.Drawing.Size(64, 24)
        Me.lowRadioButton.TabIndex = 7
        Me.lowRadioButton.TabStop = True
        Me.lowRadioButton.Text = "Low"
        '
        'highRadioButton
        '
        Me.highRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.highRadioButton.Location = New System.Drawing.Point(40, 208)
        Me.highRadioButton.Name = "highRadioButton"
        Me.highRadioButton.Size = New System.Drawing.Size(64, 24)
        Me.highRadioButton.TabIndex = 8
        Me.highRadioButton.Text = "High"
        '
        'idleStateLabel
        '
        Me.idleStateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.idleStateLabel.Location = New System.Drawing.Point(16, 165)
        Me.idleStateLabel.Name = "idleStateLabel"
        Me.idleStateLabel.Size = New System.Drawing.Size(100, 16)
        Me.idleStateLabel.TabIndex = 6
        Me.idleStateLabel.Text = "Idle State:"
        '
        'dutyCycleTextBox
        '
        Me.dutyCycleTextBox.Location = New System.Drawing.Point(16, 132)
        Me.dutyCycleTextBox.Name = "dutyCycleTextBox"
        Me.dutyCycleTextBox.Size = New System.Drawing.Size(128, 20)
        Me.dutyCycleTextBox.TabIndex = 5
        Me.dutyCycleTextBox.Text = "0.5"
        '
        'dutyCycleLabel
        '
        Me.dutyCycleLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.dutyCycleLabel.Location = New System.Drawing.Point(16, 116)
        Me.dutyCycleLabel.Name = "dutyCycleLabel"
        Me.dutyCycleLabel.Size = New System.Drawing.Size(100, 16)
        Me.dutyCycleLabel.TabIndex = 4
        Me.dutyCycleLabel.Text = "Duty Cycle:"
        '
        'frequencyTextBox
        '
        Me.frequencyTextBox.Location = New System.Drawing.Point(16, 86)
        Me.frequencyTextBox.Name = "frequencyTextBox"
        Me.frequencyTextBox.Size = New System.Drawing.Size(128, 20)
        Me.frequencyTextBox.TabIndex = 3
        Me.frequencyTextBox.Text = "1.0"
        '
        'frequencyLabel
        '
        Me.frequencyLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.frequencyLabel.Location = New System.Drawing.Point(16, 70)
        Me.frequencyLabel.Name = "frequencyLabel"
        Me.frequencyLabel.Size = New System.Drawing.Size(100, 16)
        Me.frequencyLabel.TabIndex = 2
        Me.frequencyLabel.Text = "Frequency (Hz):"
        '
        'counterLabel
        '
        Me.counterLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.counterLabel.Location = New System.Drawing.Point(16, 23)
        Me.counterLabel.Name = "counterLabel"
        Me.counterLabel.Size = New System.Drawing.Size(100, 16)
        Me.counterLabel.TabIndex = 0
        Me.counterLabel.Text = "Counter(s):"
        '
        'startButton
        '
        Me.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.startButton.Location = New System.Drawing.Point(208, 176)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(112, 32)
        Me.startButton.TabIndex = 0
        Me.startButton.Text = "Start"
        '
        'statusCheckTimer
        '
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(354, 263)
        Me.Controls.Add(Me.stopButton)
        Me.Controls.Add(Me.triggerParameterGroupBox)
        Me.Controls.Add(Me.channelParameterGroupBox)
        Me.Controls.Add(Me.startButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Gen Dig Pulse Train-Continuous-Dig Start"
        Me.triggerParameterGroupBox.ResumeLayout(False)
        Me.channelParameterGroupBox.ResumeLayout(False)
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
                "PulseTrain", COPulseFrequencyUnits.Hertz, idleState, 0.0, _
                Convert.ToDouble(frequencyTextBox.Text), _
                Convert.ToDouble(dutyCycleTextBox.Text))

            myTask.Triggers.StartTrigger.Type = StartTriggerType.DigitalEdge
            myTask.Triggers.StartTrigger.DigitalEdge.Edge = triggerEdge
            myTask.Triggers.StartTrigger.DigitalEdge.Source = triggerSourceTextBox.Text

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

    Private Sub lowRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lowRadioButton.CheckedChanged
        idleState = COPulseIdleState.Low
    End Sub

    Private Sub highRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles highRadioButton.CheckedChanged
        idleState = COPulseIdleState.High
    End Sub

    Private Sub risingRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles risingRadioButton.CheckedChanged
        triggerEdge = DigitalEdgeStartTriggerEdge.Rising
    End Sub

    Private Sub fallingRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles fallingRadioButton.CheckedChanged
        triggerEdge = DigitalEdgeStartTriggerEdge.Falling
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
