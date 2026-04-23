'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   GenDigPulseTrain_ContBuff_ExtClk
'
' Category:
'   CO
'
' Description:
'   This example demonstrates how to generate a continuous buffered sample
'   clocked digital pulse train from a Counter Output Channel. The Frequency,
'   Duty Cycle, and Idle State are all configurable. The default data generated
'   is a pulse train with a fixed frequency but a duty cycle that varies based
'   on the Duty Cycle Max/Min and the signal type. The duty cycle will update
'   with each sample clock edge.
'
' Instructions for running:
'   1.  Enter the physical channel of the counter you want to output your signal
'       to on the DAQ device.
'   2.  Enter the Sample Clock Rate, samples per channel and clock source to
'       configure timing. Note: The sample clock rate should be less than half
'       the output PWM Frequency to avoid an over run error.
'   3.  Enter the frequency and a minimum duty cycle to define the initial pulse
'       parameters.
'   4.  Enter the signal type, minimum and maximum duty cycle to define the
'       pulse train generated.
'
' Steps:
'   1.  Create a counter output channel to produce a pulse in terms of
'       frequency.  If the idle state of the pulse is set to low, the first
'       transition of the generated signal is from low to high.
'   2.  Use the Task.Timing.ConfigureImplicit method to configure the duration
'       of the pulse generation.
'   3.  Write the array of frequency and duty cycle specifications to the output
'       buffer.
'   4.  Start the task to arm the counter and begin the pulse train generation.
'   5.  For continuous generation, the counter will continuously generate the
'       pulse train until the Stop button is pressed.
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
    Private WithEvents dutyCycleMaxTextBox As System.Windows.Forms.TextBox
    Private WithEvents channelParameterGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents counterComboBox As System.Windows.Forms.ComboBox
    Private WithEvents idleStateGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents highRadioButton As System.Windows.Forms.RadioButton
    Private WithEvents lowRadioButton As System.Windows.Forms.RadioButton
    Private WithEvents physicalChannelLabel As System.Windows.Forms.Label
    Friend WithEvents clockSourceLabel As System.Windows.Forms.Label
    Private WithEvents dutyCycleMaxLabel As System.Windows.Forms.Label
    Private WithEvents dutyCycleMinTextBox As System.Windows.Forms.TextBox
    Private WithEvents frequencyTextBox As System.Windows.Forms.TextBox
    Private WithEvents frequencyLabel As System.Windows.Forms.Label
    Private WithEvents pwmParametersGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents dutyCycleMinLabel As System.Windows.Forms.Label
    Private WithEvents timingParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents clockSourceTextBox As System.Windows.Forms.TextBox
    Friend WithEvents rateLabel As System.Windows.Forms.Label
    Friend WithEvents samplesLabel As System.Windows.Forms.Label
    Friend WithEvents samplesPerChannelNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents rateNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents stopButton As System.Windows.Forms.Button
    Private WithEvents startButton As System.Windows.Forms.Button
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
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.dutyCycleMaxTextBox = New System.Windows.Forms.TextBox
        Me.channelParameterGroupBox = New System.Windows.Forms.GroupBox
        Me.counterComboBox = New System.Windows.Forms.ComboBox
        Me.idleStateGroupBox = New System.Windows.Forms.GroupBox
        Me.highRadioButton = New System.Windows.Forms.RadioButton
        Me.lowRadioButton = New System.Windows.Forms.RadioButton
        Me.physicalChannelLabel = New System.Windows.Forms.Label
        Me.clockSourceLabel = New System.Windows.Forms.Label
        Me.dutyCycleMaxLabel = New System.Windows.Forms.Label
        Me.dutyCycleMinTextBox = New System.Windows.Forms.TextBox
        Me.frequencyTextBox = New System.Windows.Forms.TextBox
        Me.frequencyLabel = New System.Windows.Forms.Label
        Me.pwmParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.dutyCycleMinLabel = New System.Windows.Forms.Label
        Me.timingParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.clockSourceTextBox = New System.Windows.Forms.TextBox
        Me.rateLabel = New System.Windows.Forms.Label
        Me.samplesLabel = New System.Windows.Forms.Label
        Me.samplesPerChannelNumeric = New System.Windows.Forms.NumericUpDown
        Me.rateNumeric = New System.Windows.Forms.NumericUpDown
        Me.stopButton = New System.Windows.Forms.Button
        Me.startButton = New System.Windows.Forms.Button
        Me.channelParameterGroupBox.SuspendLayout()
        Me.idleStateGroupBox.SuspendLayout()
        Me.pwmParametersGroupBox.SuspendLayout()
        Me.timingParametersGroupBox.SuspendLayout()
        CType(Me.samplesPerChannelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rateNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dutyCycleMaxTextBox
        '
        Me.dutyCycleMaxTextBox.Location = New System.Drawing.Point(112, 82)
        Me.dutyCycleMaxTextBox.Name = "dutyCycleMaxTextBox"
        Me.dutyCycleMaxTextBox.Size = New System.Drawing.Size(132, 20)
        Me.dutyCycleMaxTextBox.TabIndex = 11
        Me.dutyCycleMaxTextBox.Text = "0.8"
        '
        'channelParameterGroupBox
        '
        Me.channelParameterGroupBox.Controls.Add(Me.counterComboBox)
        Me.channelParameterGroupBox.Controls.Add(Me.idleStateGroupBox)
        Me.channelParameterGroupBox.Controls.Add(Me.physicalChannelLabel)
        Me.channelParameterGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.channelParameterGroupBox.Location = New System.Drawing.Point(10, 9)
        Me.channelParameterGroupBox.Name = "channelParameterGroupBox"
        Me.channelParameterGroupBox.Size = New System.Drawing.Size(250, 117)
        Me.channelParameterGroupBox.TabIndex = 7
        Me.channelParameterGroupBox.TabStop = False
        Me.channelParameterGroupBox.Text = "Channel Parameters:"
        '
        'counterComboBox
        '
        Me.counterComboBox.Location = New System.Drawing.Point(112, 16)
        Me.counterComboBox.Name = "counterComboBox"
        Me.counterComboBox.Size = New System.Drawing.Size(132, 21)
        Me.counterComboBox.TabIndex = 1
        Me.counterComboBox.Text = "Dev1/ctr0"
        '
        'idleStateGroupBox
        '
        Me.idleStateGroupBox.Controls.Add(Me.highRadioButton)
        Me.idleStateGroupBox.Controls.Add(Me.lowRadioButton)
        Me.idleStateGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.idleStateGroupBox.Location = New System.Drawing.Point(6, 43)
        Me.idleStateGroupBox.Name = "idleStateGroupBox"
        Me.idleStateGroupBox.Size = New System.Drawing.Size(238, 64)
        Me.idleStateGroupBox.TabIndex = 6
        Me.idleStateGroupBox.TabStop = False
        Me.idleStateGroupBox.Text = "Idle State:"
        '
        'highRadioButton
        '
        Me.highRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.highRadioButton.Location = New System.Drawing.Point(123, 24)
        Me.highRadioButton.Name = "highRadioButton"
        Me.highRadioButton.Size = New System.Drawing.Size(64, 24)
        Me.highRadioButton.TabIndex = 1
        Me.highRadioButton.Text = "High"
        '
        'lowRadioButton
        '
        Me.lowRadioButton.Checked = True
        Me.lowRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.lowRadioButton.Location = New System.Drawing.Point(51, 24)
        Me.lowRadioButton.Name = "lowRadioButton"
        Me.lowRadioButton.Size = New System.Drawing.Size(64, 24)
        Me.lowRadioButton.TabIndex = 0
        Me.lowRadioButton.TabStop = True
        Me.lowRadioButton.Text = "Low"
        '
        'physicalChannelLabel
        '
        Me.physicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.physicalChannelLabel.Location = New System.Drawing.Point(8, 23)
        Me.physicalChannelLabel.Name = "physicalChannelLabel"
        Me.physicalChannelLabel.Size = New System.Drawing.Size(72, 16)
        Me.physicalChannelLabel.TabIndex = 0
        Me.physicalChannelLabel.Text = "Counter(s):"
        '
        'clockSourceLabel
        '
        Me.clockSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.clockSourceLabel.Location = New System.Drawing.Point(8, 21)
        Me.clockSourceLabel.Name = "clockSourceLabel"
        Me.clockSourceLabel.Size = New System.Drawing.Size(80, 16)
        Me.clockSourceLabel.TabIndex = 6
        Me.clockSourceLabel.Text = "Clock Source:"
        '
        'dutyCycleMaxLabel
        '
        Me.dutyCycleMaxLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.dutyCycleMaxLabel.Location = New System.Drawing.Point(6, 86)
        Me.dutyCycleMaxLabel.Name = "dutyCycleMaxLabel"
        Me.dutyCycleMaxLabel.Size = New System.Drawing.Size(104, 16)
        Me.dutyCycleMaxLabel.TabIndex = 10
        Me.dutyCycleMaxLabel.Text = "Duty Cycle Max:"
        '
        'dutyCycleMinTextBox
        '
        Me.dutyCycleMinTextBox.Location = New System.Drawing.Point(112, 51)
        Me.dutyCycleMinTextBox.Name = "dutyCycleMinTextBox"
        Me.dutyCycleMinTextBox.Size = New System.Drawing.Size(132, 20)
        Me.dutyCycleMinTextBox.TabIndex = 9
        Me.dutyCycleMinTextBox.Text = "0.5"
        '
        'frequencyTextBox
        '
        Me.frequencyTextBox.Location = New System.Drawing.Point(112, 19)
        Me.frequencyTextBox.Name = "frequencyTextBox"
        Me.frequencyTextBox.Size = New System.Drawing.Size(132, 20)
        Me.frequencyTextBox.TabIndex = 7
        Me.frequencyTextBox.Text = "1000.0"
        '
        'frequencyLabel
        '
        Me.frequencyLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.frequencyLabel.Location = New System.Drawing.Point(8, 24)
        Me.frequencyLabel.Name = "frequencyLabel"
        Me.frequencyLabel.Size = New System.Drawing.Size(120, 16)
        Me.frequencyLabel.TabIndex = 6
        Me.frequencyLabel.Text = "Frequency (Hz):"
        '
        'pwmParametersGroupBox
        '
        Me.pwmParametersGroupBox.Controls.Add(Me.dutyCycleMaxTextBox)
        Me.pwmParametersGroupBox.Controls.Add(Me.dutyCycleMaxLabel)
        Me.pwmParametersGroupBox.Controls.Add(Me.dutyCycleMinTextBox)
        Me.pwmParametersGroupBox.Controls.Add(Me.dutyCycleMinLabel)
        Me.pwmParametersGroupBox.Controls.Add(Me.frequencyTextBox)
        Me.pwmParametersGroupBox.Controls.Add(Me.frequencyLabel)
        Me.pwmParametersGroupBox.Location = New System.Drawing.Point(10, 252)
        Me.pwmParametersGroupBox.Name = "pwmParametersGroupBox"
        Me.pwmParametersGroupBox.Size = New System.Drawing.Size(250, 110)
        Me.pwmParametersGroupBox.TabIndex = 9
        Me.pwmParametersGroupBox.TabStop = False
        Me.pwmParametersGroupBox.Text = "Pulse-width Modulation Parameters:"
        '
        'dutyCycleMinLabel
        '
        Me.dutyCycleMinLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.dutyCycleMinLabel.Location = New System.Drawing.Point(8, 55)
        Me.dutyCycleMinLabel.Name = "dutyCycleMinLabel"
        Me.dutyCycleMinLabel.Size = New System.Drawing.Size(104, 16)
        Me.dutyCycleMinLabel.TabIndex = 8
        Me.dutyCycleMinLabel.Text = "Duty Cycle Min:"
        '
        'timingParametersGroupBox
        '
        Me.timingParametersGroupBox.Controls.Add(Me.clockSourceLabel)
        Me.timingParametersGroupBox.Controls.Add(Me.clockSourceTextBox)
        Me.timingParametersGroupBox.Controls.Add(Me.rateLabel)
        Me.timingParametersGroupBox.Controls.Add(Me.samplesLabel)
        Me.timingParametersGroupBox.Controls.Add(Me.samplesPerChannelNumeric)
        Me.timingParametersGroupBox.Controls.Add(Me.rateNumeric)
        Me.timingParametersGroupBox.Location = New System.Drawing.Point(10, 132)
        Me.timingParametersGroupBox.Name = "timingParametersGroupBox"
        Me.timingParametersGroupBox.Size = New System.Drawing.Size(250, 114)
        Me.timingParametersGroupBox.TabIndex = 8
        Me.timingParametersGroupBox.TabStop = False
        Me.timingParametersGroupBox.Text = "Timing Parameters:"
        '
        'clockSourceTextBox
        '
        Me.clockSourceTextBox.Location = New System.Drawing.Point(112, 19)
        Me.clockSourceTextBox.Name = "clockSourceTextBox"
        Me.clockSourceTextBox.Size = New System.Drawing.Size(132, 20)
        Me.clockSourceTextBox.TabIndex = 7
        Me.clockSourceTextBox.Text = "/Dev1/PFI7"
        '
        'rateLabel
        '
        Me.rateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.rateLabel.Location = New System.Drawing.Point(8, 85)
        Me.rateLabel.Name = "rateLabel"
        Me.rateLabel.Size = New System.Drawing.Size(64, 16)
        Me.rateLabel.TabIndex = 10
        Me.rateLabel.Text = "Rate (Hz):"
        '
        'samplesLabel
        '
        Me.samplesLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.samplesLabel.Location = New System.Drawing.Point(8, 53)
        Me.samplesLabel.Name = "samplesLabel"
        Me.samplesLabel.Size = New System.Drawing.Size(104, 16)
        Me.samplesLabel.TabIndex = 8
        Me.samplesLabel.Text = "Samples / Channel:"
        '
        'samplesPerChannelNumeric
        '
        Me.samplesPerChannelNumeric.Location = New System.Drawing.Point(112, 51)
        Me.samplesPerChannelNumeric.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.samplesPerChannelNumeric.Name = "samplesPerChannelNumeric"
        Me.samplesPerChannelNumeric.Size = New System.Drawing.Size(132, 20)
        Me.samplesPerChannelNumeric.TabIndex = 9
        Me.samplesPerChannelNumeric.Value = New Decimal(New Integer() {1000, 0, 0, 0})
        '
        'rateNumeric
        '
        Me.rateNumeric.DecimalPlaces = 2
        Me.rateNumeric.Location = New System.Drawing.Point(112, 83)
        Me.rateNumeric.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.rateNumeric.Name = "rateNumeric"
        Me.rateNumeric.Size = New System.Drawing.Size(132, 20)
        Me.rateNumeric.TabIndex = 11
        Me.rateNumeric.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        'stopButton
        '
        Me.stopButton.Enabled = False
        Me.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.stopButton.Location = New System.Drawing.Point(266, 56)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(96, 32)
        Me.stopButton.TabIndex = 6
        Me.stopButton.Text = "Stop"
        '
        'startButton
        '
        Me.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.startButton.Location = New System.Drawing.Point(266, 18)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(96, 32)
        Me.startButton.TabIndex = 5
        Me.startButton.Text = "Start"
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(372, 370)
        Me.Controls.Add(Me.channelParameterGroupBox)
        Me.Controls.Add(Me.pwmParametersGroupBox)
        Me.Controls.Add(Me.timingParametersGroupBox)
        Me.Controls.Add(Me.stopButton)
        Me.Controls.Add(Me.startButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Digital Pulse Train - Continuous - Buffered"
        Me.channelParameterGroupBox.ResumeLayout(False)
        Me.idleStateGroupBox.ResumeLayout(False)
        Me.pwmParametersGroupBox.ResumeLayout(False)
        Me.pwmParametersGroupBox.PerformLayout()
        Me.timingParametersGroupBox.ResumeLayout(False)
        Me.timingParametersGroupBox.PerformLayout()
        CType(Me.samplesPerChannelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rateNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region



    Private Sub startButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles startButton.Click

        ' This example uses the default source (or gate) terminal for 
        ' the counter of your device.  To determine what the default 
        ' counter pins for your device are or to set a different source 
        ' (or gate) pin, refer to the Connecting Counter Signals topic
        ' in the NI-DAQmx Help (search for "Connecting Counter Signals").

        Dim samplesPerChannel As Integer = Convert.ToInt32(samplesPerChannelNumeric.Value)
        Dim rate As Double = Convert.ToDouble(rateNumeric.Value)
        Dim frequency As Double = Convert.ToDouble(frequencyTextBox.Text)
        Dim dutyCycleMin As Double = Convert.ToDouble(dutyCycleMinTextBox.Text)
        Dim dutyCycleMax As Double = Convert.ToDouble(dutyCycleMaxTextBox.Text)
        Dim dutyStep As Double = (dutyCycleMax - dutyCycleMin) / samplesPerChannel

        Try
            Dim data(samplesPerChannel - 1) As CODataFrequency
            For I As Integer = 0 To data.Length - 1
                data(I) = New CODataFrequency(frequency, dutyCycleMin + dutyStep * I)
            Next

            myTask = New Task()

            myTask.COChannels.CreatePulseChannelFrequency(counterComboBox.Text, _
                "ContinuousPulseTrain", COPulseFrequencyUnits.Hertz, idleState, 0.0, _
                frequency, _
                dutyCycleMin)

            myTask.Timing.ConfigureSampleClock(clockSourceTextBox.Text, rate, _
                SampleClockActiveEdge.Rising, SampleQuantityMode.ContinuousSamples)

            Dim writer As CounterSingleChannelWriter = New CounterSingleChannelWriter(myTask.Stream)
            writer.WriteMultiSample(False, data)

            AddHandler myTask.Done, AddressOf OnTaskDone
            myTask.Start()

            startButton.Enabled = False
            stopButton.Enabled = True
        Catch ex As DaqException
            MessageBox.Show(ex.Message)
            myTask.Dispose()
            startButton.Enabled = True
            stopButton.Enabled = False
        End Try

    End Sub

    Private Sub stopButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles stopButton.Click
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

    Private Sub OnTaskDone(ByVal sender As Object, ByVal e As TaskDoneEventArgs)
        Try
            stopButton.Enabled = False
            startButton.Enabled = True
            e.CheckForException()
            myTask.Stop()
            myTask.Dispose()
            myTask = Nothing
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            myTask.Stop()
            myTask.Dispose()
            myTask = Nothing
        End Try
    End Sub
End Class
