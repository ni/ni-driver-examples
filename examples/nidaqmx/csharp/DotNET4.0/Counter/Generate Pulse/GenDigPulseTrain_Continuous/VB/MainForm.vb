'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   GenDigPulseTrain_Continuous
'
' Category:
'   CO
'
' Description:
'   This example demonstrates how to generate a continuous digital pulse train
'   from a counter output channel.  The frequency, duty cycle, and idle state
'   are all configurable.This example shows how to configure the pulse in terms
'   of frequency and duty cycle, but it can easily be modified to generate a
'   pulse in terms of time or ticks.
'
' Instructions for running:
'   1.  Enter the physical channel of the counter you want to output your signal
'       to on the DAQ device.
'   2.  Enter the frequency and duty cycle to define the pulse parameters.  You
'       can also change the idle state to set the state the line will remain in
'       after the generation is stopped.Note:  Use the CountDigEvents example to
'       verify you are outputting the pulse train on the DAQ device.
'
' Steps:
'   1.  Create a counter output channel to produce a pulse in terms of
'       frequency.  If the idle state of the pulse is set to low, the first
'       transition of the generated signal is from low to high.
'   2.  Use the Task.Timing.ConfigureImplicit method to configure the duration
'       of the pulse generation.
'   3.  Call the Task.Start method to arm the counter and begin the pulse train
'       generation.
'   4.  This example will continue to generate the pulse train until the Stop
'       button is pressed.
'   5.  Call the Task.Stop method to stop the function
'   6.  Dispose the Task object to clean-up any resources associated with the
'       task.
'   7.  Handle any DaqExceptions, if they occur.
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

    Private idleState As COPulseIdleState
    Private myTask As Task

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()
        Application.EnableVisualStyles()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
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
    Friend WithEvents startButton As System.Windows.Forms.Button
    Friend WithEvents channelParameterGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents dutyCycleTextBox As System.Windows.Forms.TextBox
    Friend WithEvents dutyCycleLabel As System.Windows.Forms.Label
    Friend WithEvents frequencyTextBox As System.Windows.Forms.TextBox
    Friend WithEvents frequencyLabel As System.Windows.Forms.Label
    Friend WithEvents physicalChannelLabel As System.Windows.Forms.Label
    Friend WithEvents idleStateGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents lowRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents highRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents statusCheckTimer As System.Windows.Forms.Timer
    Friend WithEvents counterComboBox As System.Windows.Forms.ComboBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MainForm))
        Me.stopButton = New System.Windows.Forms.Button
        Me.startButton = New System.Windows.Forms.Button
        Me.channelParameterGroupBox = New System.Windows.Forms.GroupBox
        Me.counterComboBox = New System.Windows.Forms.ComboBox
        Me.idleStateGroupBox = New System.Windows.Forms.GroupBox
        Me.highRadioButton = New System.Windows.Forms.RadioButton
        Me.lowRadioButton = New System.Windows.Forms.RadioButton
        Me.dutyCycleTextBox = New System.Windows.Forms.TextBox
        Me.dutyCycleLabel = New System.Windows.Forms.Label
        Me.frequencyTextBox = New System.Windows.Forms.TextBox
        Me.frequencyLabel = New System.Windows.Forms.Label
        Me.physicalChannelLabel = New System.Windows.Forms.Label
        Me.statusCheckTimer = New System.Windows.Forms.Timer(Me.components)
        Me.channelParameterGroupBox.SuspendLayout()
        Me.idleStateGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'stopButton
        '
        Me.stopButton.Enabled = False
        Me.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.stopButton.Location = New System.Drawing.Point(248, 56)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(88, 32)
        Me.stopButton.TabIndex = 1
        Me.stopButton.Text = "Stop"
        '
        'startButton
        '
        Me.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.startButton.Location = New System.Drawing.Point(248, 16)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(88, 32)
        Me.startButton.TabIndex = 0
        Me.startButton.Text = "Start"
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
        Me.channelParameterGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.channelParameterGroupBox.Location = New System.Drawing.Point(8, 8)
        Me.channelParameterGroupBox.Name = "channelParameterGroupBox"
        Me.channelParameterGroupBox.Size = New System.Drawing.Size(232, 192)
        Me.channelParameterGroupBox.TabIndex = 2
        Me.channelParameterGroupBox.TabStop = False
        Me.channelParameterGroupBox.Text = "Channel Parameters:"
        '
        'counterComboBox
        '
        Me.counterComboBox.Location = New System.Drawing.Point(120, 24)
        Me.counterComboBox.Name = "counterComboBox"
        Me.counterComboBox.Size = New System.Drawing.Size(100, 21)
        Me.counterComboBox.TabIndex = 1
        Me.counterComboBox.Text = "Dev1/ctr0"
        '
        'idleStateGroupBox
        '
        Me.idleStateGroupBox.Controls.Add(Me.highRadioButton)
        Me.idleStateGroupBox.Controls.Add(Me.lowRadioButton)
        Me.idleStateGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.idleStateGroupBox.Location = New System.Drawing.Point(8, 120)
        Me.idleStateGroupBox.Name = "idleStateGroupBox"
        Me.idleStateGroupBox.Size = New System.Drawing.Size(216, 64)
        Me.idleStateGroupBox.TabIndex = 6
        Me.idleStateGroupBox.TabStop = False
        Me.idleStateGroupBox.Text = "Idle State:"
        '
        'highRadioButton
        '
        Me.highRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.highRadioButton.Location = New System.Drawing.Point(112, 24)
        Me.highRadioButton.Name = "highRadioButton"
        Me.highRadioButton.Size = New System.Drawing.Size(64, 24)
        Me.highRadioButton.TabIndex = 1
        Me.highRadioButton.Text = "High"
        '
        'lowRadioButton
        '
        Me.lowRadioButton.Checked = True
        Me.lowRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.lowRadioButton.Location = New System.Drawing.Point(48, 24)
        Me.lowRadioButton.Name = "lowRadioButton"
        Me.lowRadioButton.Size = New System.Drawing.Size(64, 24)
        Me.lowRadioButton.TabIndex = 0
        Me.lowRadioButton.TabStop = True
        Me.lowRadioButton.Text = "Low"
        '
        'dutyCycleTextBox
        '
        Me.dutyCycleTextBox.Location = New System.Drawing.Point(120, 88)
        Me.dutyCycleTextBox.Name = "dutyCycleTextBox"
        Me.dutyCycleTextBox.TabIndex = 5
        Me.dutyCycleTextBox.Text = "0.5"
        '
        'dutyCycleLabel
        '
        Me.dutyCycleLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.dutyCycleLabel.Location = New System.Drawing.Point(8, 91)
        Me.dutyCycleLabel.Name = "dutyCycleLabel"
        Me.dutyCycleLabel.Size = New System.Drawing.Size(100, 16)
        Me.dutyCycleLabel.TabIndex = 4
        Me.dutyCycleLabel.Text = "Duty Cycle:"
        '
        'frequencyTextBox
        '
        Me.frequencyTextBox.Location = New System.Drawing.Point(120, 56)
        Me.frequencyTextBox.Name = "frequencyTextBox"
        Me.frequencyTextBox.TabIndex = 3
        Me.frequencyTextBox.Text = "1.0"
        '
        'frequencyLabel
        '
        Me.frequencyLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.frequencyLabel.Location = New System.Drawing.Point(8, 59)
        Me.frequencyLabel.Name = "frequencyLabel"
        Me.frequencyLabel.Size = New System.Drawing.Size(100, 16)
        Me.frequencyLabel.TabIndex = 2
        Me.frequencyLabel.Text = "Frequency (Hz):"
        '
        'physicalChannelLabel
        '
        Me.physicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.physicalChannelLabel.Location = New System.Drawing.Point(8, 28)
        Me.physicalChannelLabel.Name = "physicalChannelLabel"
        Me.physicalChannelLabel.Size = New System.Drawing.Size(100, 16)
        Me.physicalChannelLabel.TabIndex = 0
        Me.physicalChannelLabel.Text = "Counter(s):"
        '
        'statusCheckTimer
        '
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(346, 208)
        Me.Controls.Add(Me.stopButton)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.channelParameterGroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Generate Continuous Digital Pulse Train"
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
                "ContinuousPulseTrain", COPulseFrequencyUnits.Hertz, idleState, 0.0, _
                Convert.ToDouble(frequencyTextBox.Text), _
                Convert.ToDouble(dutyCycleTextBox.Text))

            myTask.Timing.ConfigureImplicit(SampleQuantityMode.ContinuousSamples, 1000)

            myTask.Start()

            startButton.Enabled = False
            stopButton.Enabled = True

            StatusCheckTimer.Enabled = True

        Catch ex As System.Exception

            MessageBox.Show(ex.Message)
            myTask.Dispose()
            startButton.Enabled = True
            stopButton.Enabled = False
            statusCheckTimer.Enabled = False

        End Try

    End Sub

    Private Sub stopButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles stopButton.Click

        statusCheckTimer.Enabled = False
        myTask.Stop()
        myTask.Dispose()
        startButton.Enabled = True
        stopButton.Enabled = False

    End Sub

    Private Sub lowRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lowRadioButton.CheckedChanged
        idleState = COPulseIdleState.Low
    End Sub

    Private Sub highRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles highRadioButton.CheckedChanged
        idleState = COPulseIdleState.High
    End Sub

    Private Sub statusCheckTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles statusCheckTimer.Tick
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
            System.Windows.Forms.MessageBox.Show(ex.Message)
            statusCheckTimer.Enabled = False
            myTask.Stop()
            myTask.Dispose()
            startButton.Enabled = True
            stopButton.Enabled = False
        End Try
    End Sub
End Class
