'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   GenMultVoltUpdatesIntClk_DigStart
'
' Category:
'   AO
'
' Description:
'   This example demonstrates how to output multiple voltage updates (samples)
'   to an analog output channel.  The generation starts when a digital trigger
'   is received.
'
' Instructions for running:
'   1.  Select the physical channel corresponding to where your signal is output
'       on the DAQ device.
'   2.  Enter the minimum and maximum voltage values.
'   3.  Define the digital start trigger source and ege. In this example, the
'       source defaults to /Dev1/PFI0.
'   4.  Set the signal frequency of the waveform to output.
'   5.  Set the function generator parameters, including the waveform type,
'       cycles per buffer, samples per bufrer, and amplitude.
'
' Steps:
'   1.  Create a new task and an analog output voltage channel.
'   2.  Set up the digital start trigger for the measurement using the
'       Task.Triggers property.
'   3.  Use the FunctionGenerator class to generate waveform data and calculate
'       the sample clock rate.
'   4.  Set up the timing for the measurement. In this example we use the
'       internal DAQ Device clock to take a finite number of samples.
'   5.  Create a AnalogSingleChannelWriter, add the Task Done event handler, and
'       call the WriteMultiSample method to write multiple samples to a single
'       channel on your DAQ device. The autoStart parameter is set to false, so
'       Task.Start() must be explicitly called to begin the voltage generation.
'   6.  Call Task.Start().
'   7.  Dispose the Task object to clean-up any resources associated with the
'       task.
'   8.  Handle any DaqExceptions, if they occur.
'   9.  In the Task Done event check for any errors and Dispose the Task object
'       to clean-up any resources associated with the task.
'
' I/O Connections Overview:
'   Make sure your signal output terminal matches the text in the physical
'   channel text box and your digital trigger signal is connected to the
'   terminal specified in the trigger source text box.  The default settings
'   will start output to the ao0 pin on your DAQ Device when a digital rising
'   edge occurs on Dev1/PFI0. For more information on the input and output
'   terminals for your device, open the NI-DAQmx Help, and refer to the NI-DAQmx
'   Device Terminals and Device Considerations books in the table of contents.
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

Public Class MainForm
    Inherits System.Windows.Forms.Form

    Private edge As DigitalEdgeStartTriggerEdge = DigitalEdgeStartTriggerEdge.Rising
    Private taskRunning As Boolean = False
    Dim myTask As Task

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()
        Application.EnableVisualStyles()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        FunctionGenerator.InitComboBox(signalTypeComboBox)

        physicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AO, PhysicalChannelAccess.External))
        If (physicalChannelComboBox.Items.Count > 0) Then
            physicalChannelComboBox.SelectedIndex = 0
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
    Friend WithEvents TimingParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents frequencyTextBox As System.Windows.Forms.TextBox
    Friend WithEvents frequencyLabel As System.Windows.Forms.Label
    Friend WithEvents functionGeneratorGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents amplitudeLabel As System.Windows.Forms.Label
    Friend WithEvents amplitudeNumericUpDown As System.Windows.Forms.NumericUpDown
    Friend WithEvents waveformTypeLabel As System.Windows.Forms.Label
    Friend WithEvents signalTypeComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents startButton As System.Windows.Forms.Button
    Friend WithEvents triggerParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents fallingRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents risingRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents edgeLabel As System.Windows.Forms.Label
    Friend WithEvents digitalTriggerTextBox As System.Windows.Forms.TextBox
    Friend WithEvents triggerSourceLabel As System.Windows.Forms.Label
    Friend WithEvents channelParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents maximumValueTextBox As System.Windows.Forms.TextBox
    Friend WithEvents minimumValueTextBox As System.Windows.Forms.TextBox
    Friend WithEvents maximumValueLabel As System.Windows.Forms.Label
    Friend WithEvents minimumValueLabel As System.Windows.Forms.Label
    Friend WithEvents physicalChannelLabel As System.Windows.Forms.Label
    Friend WithEvents samplesPerBufferNumericUpDown As System.Windows.Forms.NumericUpDown
    Friend WithEvents samplesPerBufferLabel As System.Windows.Forms.Label
    Friend WithEvents cyclesPerBufferLabel As System.Windows.Forms.Label
    Friend WithEvents cyclesPerBufferNumericUpDown As System.Windows.Forms.NumericUpDown
    Friend WithEvents physicalChannelComboBox As System.Windows.Forms.ComboBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MainForm))
        Me.TimingParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.frequencyTextBox = New System.Windows.Forms.TextBox
        Me.frequencyLabel = New System.Windows.Forms.Label
        Me.functionGeneratorGroupBox = New System.Windows.Forms.GroupBox
        Me.amplitudeLabel = New System.Windows.Forms.Label
        Me.amplitudeNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.samplesPerBufferNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.samplesPerBufferLabel = New System.Windows.Forms.Label
        Me.cyclesPerBufferLabel = New System.Windows.Forms.Label
        Me.cyclesPerBufferNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.waveformTypeLabel = New System.Windows.Forms.Label
        Me.signalTypeComboBox = New System.Windows.Forms.ComboBox
        Me.startButton = New System.Windows.Forms.Button
        Me.triggerParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.fallingRadioButton = New System.Windows.Forms.RadioButton
        Me.risingRadioButton = New System.Windows.Forms.RadioButton
        Me.edgeLabel = New System.Windows.Forms.Label
        Me.digitalTriggerTextBox = New System.Windows.Forms.TextBox
        Me.triggerSourceLabel = New System.Windows.Forms.Label
        Me.channelParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.physicalChannelComboBox = New System.Windows.Forms.ComboBox
        Me.maximumValueTextBox = New System.Windows.Forms.TextBox
        Me.minimumValueTextBox = New System.Windows.Forms.TextBox
        Me.maximumValueLabel = New System.Windows.Forms.Label
        Me.minimumValueLabel = New System.Windows.Forms.Label
        Me.physicalChannelLabel = New System.Windows.Forms.Label
        Me.TimingParametersGroupBox.SuspendLayout()
        Me.functionGeneratorGroupBox.SuspendLayout()
        CType(Me.amplitudeNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.samplesPerBufferNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cyclesPerBufferNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.triggerParametersGroupBox.SuspendLayout()
        Me.channelParametersGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'TimingParametersGroupBox
        '
        Me.TimingParametersGroupBox.Controls.Add(Me.frequencyTextBox)
        Me.TimingParametersGroupBox.Controls.Add(Me.frequencyLabel)
        Me.TimingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.TimingParametersGroupBox.Location = New System.Drawing.Point(8, 152)
        Me.TimingParametersGroupBox.Name = "TimingParametersGroupBox"
        Me.TimingParametersGroupBox.Size = New System.Drawing.Size(296, 64)
        Me.TimingParametersGroupBox.TabIndex = 2
        Me.TimingParametersGroupBox.TabStop = False
        Me.TimingParametersGroupBox.Text = "Timing Parameters"
        '
        'frequencyTextBox
        '
        Me.frequencyTextBox.Location = New System.Drawing.Point(136, 24)
        Me.frequencyTextBox.Name = "frequencyTextBox"
        Me.frequencyTextBox.Size = New System.Drawing.Size(144, 20)
        Me.frequencyTextBox.TabIndex = 1
        Me.frequencyTextBox.Text = "1000"
        '
        'frequencyLabel
        '
        Me.frequencyLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.frequencyLabel.Location = New System.Drawing.Point(16, 24)
        Me.frequencyLabel.Name = "frequencyLabel"
        Me.frequencyLabel.Size = New System.Drawing.Size(128, 24)
        Me.frequencyLabel.TabIndex = 0
        Me.frequencyLabel.Text = "Signal Frequency (Hz):"
        '
        'functionGeneratorGroupBox
        '
        Me.functionGeneratorGroupBox.Controls.Add(Me.amplitudeLabel)
        Me.functionGeneratorGroupBox.Controls.Add(Me.amplitudeNumericUpDown)
        Me.functionGeneratorGroupBox.Controls.Add(Me.samplesPerBufferNumericUpDown)
        Me.functionGeneratorGroupBox.Controls.Add(Me.samplesPerBufferLabel)
        Me.functionGeneratorGroupBox.Controls.Add(Me.cyclesPerBufferLabel)
        Me.functionGeneratorGroupBox.Controls.Add(Me.cyclesPerBufferNumericUpDown)
        Me.functionGeneratorGroupBox.Controls.Add(Me.waveformTypeLabel)
        Me.functionGeneratorGroupBox.Controls.Add(Me.signalTypeComboBox)
        Me.functionGeneratorGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.functionGeneratorGroupBox.Location = New System.Drawing.Point(320, 8)
        Me.functionGeneratorGroupBox.Name = "functionGeneratorGroupBox"
        Me.functionGeneratorGroupBox.Size = New System.Drawing.Size(256, 168)
        Me.functionGeneratorGroupBox.TabIndex = 4
        Me.functionGeneratorGroupBox.TabStop = False
        Me.functionGeneratorGroupBox.Text = "Function Generator Parameters"
        '
        'amplitudeLabel
        '
        Me.amplitudeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.amplitudeLabel.Location = New System.Drawing.Point(16, 136)
        Me.amplitudeLabel.Name = "amplitudeLabel"
        Me.amplitudeLabel.Size = New System.Drawing.Size(104, 23)
        Me.amplitudeLabel.TabIndex = 6
        Me.amplitudeLabel.Text = "Amplitude:"
        '
        'amplitudeNumericUpDown
        '
        Me.amplitudeNumericUpDown.DecimalPlaces = 1
        Me.amplitudeNumericUpDown.Location = New System.Drawing.Point(120, 136)
        Me.amplitudeNumericUpDown.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.amplitudeNumericUpDown.Name = "amplitudeNumericUpDown"
        Me.amplitudeNumericUpDown.TabIndex = 7
        Me.amplitudeNumericUpDown.Value = New Decimal(New Integer() {2, 0, 0, 0})
        '
        'samplesPerBufferNumericUpDown
        '
        Me.samplesPerBufferNumericUpDown.Location = New System.Drawing.Point(120, 96)
        Me.samplesPerBufferNumericUpDown.Maximum = New Decimal(New Integer() {10000000, 0, 0, 0})
        Me.samplesPerBufferNumericUpDown.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.samplesPerBufferNumericUpDown.Name = "samplesPerBufferNumericUpDown"
        Me.samplesPerBufferNumericUpDown.TabIndex = 5
        Me.samplesPerBufferNumericUpDown.Value = New Decimal(New Integer() {250, 0, 0, 0})
        '
        'samplesPerBufferLabel
        '
        Me.samplesPerBufferLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.samplesPerBufferLabel.Location = New System.Drawing.Point(16, 96)
        Me.samplesPerBufferLabel.Name = "samplesPerBufferLabel"
        Me.samplesPerBufferLabel.Size = New System.Drawing.Size(96, 32)
        Me.samplesPerBufferLabel.TabIndex = 4
        Me.samplesPerBufferLabel.Text = "Samples Per Buffer:"
        '
        'cyclesPerBufferLabel
        '
        Me.cyclesPerBufferLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.cyclesPerBufferLabel.Location = New System.Drawing.Point(16, 62)
        Me.cyclesPerBufferLabel.Name = "cyclesPerBufferLabel"
        Me.cyclesPerBufferLabel.Size = New System.Drawing.Size(104, 23)
        Me.cyclesPerBufferLabel.TabIndex = 2
        Me.cyclesPerBufferLabel.Text = "Cycles Per Buffer:"
        '
        'cyclesPerBufferNumericUpDown
        '
        Me.cyclesPerBufferNumericUpDown.DecimalPlaces = 1
        Me.cyclesPerBufferNumericUpDown.Location = New System.Drawing.Point(120, 56)
        Me.cyclesPerBufferNumericUpDown.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.cyclesPerBufferNumericUpDown.Name = "cyclesPerBufferNumericUpDown"
        Me.cyclesPerBufferNumericUpDown.TabIndex = 3
        Me.cyclesPerBufferNumericUpDown.Value = New Decimal(New Integer() {50, 0, 0, 65536})
        '
        'waveformTypeLabel
        '
        Me.waveformTypeLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.waveformTypeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.waveformTypeLabel.Location = New System.Drawing.Point(16, 26)
        Me.waveformTypeLabel.Name = "waveformTypeLabel"
        Me.waveformTypeLabel.Size = New System.Drawing.Size(88, 14)
        Me.waveformTypeLabel.TabIndex = 0
        Me.waveformTypeLabel.Text = "Waveform Type:"
        '
        'signalTypeComboBox
        '
        Me.signalTypeComboBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.signalTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.signalTypeComboBox.ItemHeight = 13
        Me.signalTypeComboBox.Location = New System.Drawing.Point(120, 24)
        Me.signalTypeComboBox.Name = "signalTypeComboBox"
        Me.signalTypeComboBox.Size = New System.Drawing.Size(120, 21)
        Me.signalTypeComboBox.TabIndex = 1
        '
        'startButton
        '
        Me.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.startButton.Location = New System.Drawing.Point(416, 312)
        Me.startButton.Name = "startButton"
        Me.startButton.TabIndex = 0
        Me.startButton.Text = "&Start"
        '
        'triggerParametersGroupBox
        '
        Me.triggerParametersGroupBox.Controls.Add(Me.fallingRadioButton)
        Me.triggerParametersGroupBox.Controls.Add(Me.risingRadioButton)
        Me.triggerParametersGroupBox.Controls.Add(Me.edgeLabel)
        Me.triggerParametersGroupBox.Controls.Add(Me.digitalTriggerTextBox)
        Me.triggerParametersGroupBox.Controls.Add(Me.triggerSourceLabel)
        Me.triggerParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.triggerParametersGroupBox.Location = New System.Drawing.Point(8, 224)
        Me.triggerParametersGroupBox.Name = "triggerParametersGroupBox"
        Me.triggerParametersGroupBox.Size = New System.Drawing.Size(296, 112)
        Me.triggerParametersGroupBox.TabIndex = 3
        Me.triggerParametersGroupBox.TabStop = False
        Me.triggerParametersGroupBox.Text = "Trigger Parameters"
        '
        'fallingRadioButton
        '
        Me.fallingRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.fallingRadioButton.Location = New System.Drawing.Point(136, 80)
        Me.fallingRadioButton.Name = "fallingRadioButton"
        Me.fallingRadioButton.Size = New System.Drawing.Size(144, 24)
        Me.fallingRadioButton.TabIndex = 4
        Me.fallingRadioButton.Text = "Falling"
        '
        'risingRadioButton
        '
        Me.risingRadioButton.Checked = True
        Me.risingRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.risingRadioButton.Location = New System.Drawing.Point(136, 64)
        Me.risingRadioButton.Name = "risingRadioButton"
        Me.risingRadioButton.Size = New System.Drawing.Size(144, 16)
        Me.risingRadioButton.TabIndex = 3
        Me.risingRadioButton.TabStop = True
        Me.risingRadioButton.Text = "Rising"
        '
        'edgeLabel
        '
        Me.edgeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.edgeLabel.Location = New System.Drawing.Point(16, 72)
        Me.edgeLabel.Name = "edgeLabel"
        Me.edgeLabel.Size = New System.Drawing.Size(100, 16)
        Me.edgeLabel.TabIndex = 2
        Me.edgeLabel.Text = "Edge:"
        '
        'digitalTriggerTextBox
        '
        Me.digitalTriggerTextBox.Location = New System.Drawing.Point(136, 32)
        Me.digitalTriggerTextBox.Name = "digitalTriggerTextBox"
        Me.digitalTriggerTextBox.Size = New System.Drawing.Size(144, 20)
        Me.digitalTriggerTextBox.TabIndex = 1
        Me.digitalTriggerTextBox.Text = "/Dev1/PFI0"
        '
        'triggerSourceLabel
        '
        Me.triggerSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.triggerSourceLabel.Location = New System.Drawing.Point(16, 32)
        Me.triggerSourceLabel.Name = "triggerSourceLabel"
        Me.triggerSourceLabel.Size = New System.Drawing.Size(128, 24)
        Me.triggerSourceLabel.TabIndex = 0
        Me.triggerSourceLabel.Text = "Trigger Source:"
        '
        'channelParametersGroupBox
        '
        Me.channelParametersGroupBox.Controls.Add(Me.physicalChannelComboBox)
        Me.channelParametersGroupBox.Controls.Add(Me.maximumValueTextBox)
        Me.channelParametersGroupBox.Controls.Add(Me.minimumValueTextBox)
        Me.channelParametersGroupBox.Controls.Add(Me.maximumValueLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.minimumValueLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.physicalChannelLabel)
        Me.channelParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.channelParametersGroupBox.Location = New System.Drawing.Point(8, 8)
        Me.channelParametersGroupBox.Name = "channelParametersGroupBox"
        Me.channelParametersGroupBox.Size = New System.Drawing.Size(296, 136)
        Me.channelParametersGroupBox.TabIndex = 1
        Me.channelParametersGroupBox.TabStop = False
        Me.channelParametersGroupBox.Text = "Channel Parameters"
        '
        'physicalChannelComboBox
        '
        Me.physicalChannelComboBox.Location = New System.Drawing.Point(136, 24)
        Me.physicalChannelComboBox.Name = "physicalChannelComboBox"
        Me.physicalChannelComboBox.Size = New System.Drawing.Size(144, 21)
        Me.physicalChannelComboBox.TabIndex = 1
        Me.physicalChannelComboBox.Text = "Dev1/ao0"
        '
        'maximumValueTextBox
        '
        Me.maximumValueTextBox.Location = New System.Drawing.Point(136, 96)
        Me.maximumValueTextBox.Name = "maximumValueTextBox"
        Me.maximumValueTextBox.Size = New System.Drawing.Size(144, 20)
        Me.maximumValueTextBox.TabIndex = 5
        Me.maximumValueTextBox.Text = "10"
        '
        'minimumValueTextBox
        '
        Me.minimumValueTextBox.Location = New System.Drawing.Point(136, 60)
        Me.minimumValueTextBox.Name = "minimumValueTextBox"
        Me.minimumValueTextBox.Size = New System.Drawing.Size(144, 20)
        Me.minimumValueTextBox.TabIndex = 3
        Me.minimumValueTextBox.Text = "-10"
        '
        'maximumValueLabel
        '
        Me.maximumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.maximumValueLabel.Location = New System.Drawing.Point(16, 96)
        Me.maximumValueLabel.Name = "maximumValueLabel"
        Me.maximumValueLabel.Size = New System.Drawing.Size(112, 16)
        Me.maximumValueLabel.TabIndex = 4
        Me.maximumValueLabel.Text = "Maximum Value (V):"
        '
        'minimumValueLabel
        '
        Me.minimumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.minimumValueLabel.Location = New System.Drawing.Point(16, 64)
        Me.minimumValueLabel.Name = "minimumValueLabel"
        Me.minimumValueLabel.Size = New System.Drawing.Size(104, 16)
        Me.minimumValueLabel.TabIndex = 2
        Me.minimumValueLabel.Text = "Minimum Value (V):"
        '
        'physicalChannelLabel
        '
        Me.physicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.physicalChannelLabel.Location = New System.Drawing.Point(16, 24)
        Me.physicalChannelLabel.Name = "physicalChannelLabel"
        Me.physicalChannelLabel.Size = New System.Drawing.Size(96, 16)
        Me.physicalChannelLabel.TabIndex = 0
        Me.physicalChannelLabel.Text = "Physical Channel:"
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(586, 344)
        Me.Controls.Add(Me.TimingParametersGroupBox)
        Me.Controls.Add(Me.functionGeneratorGroupBox)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.triggerParametersGroupBox)
        Me.Controls.Add(Me.channelParametersGroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Generate Multiple Volt Updates IntClk-Digital Start"
        Me.TimingParametersGroupBox.ResumeLayout(False)
        Me.functionGeneratorGroupBox.ResumeLayout(False)
        CType(Me.amplitudeNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.samplesPerBufferNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cyclesPerBufferNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        Me.triggerParametersGroupBox.ResumeLayout(False)
        Me.channelParametersGroupBox.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Delegate Sub EnableStartButtonDelegate()

    Private Sub EnableStartButton()
        startButton.Enabled = True
    End Sub

    Private Sub startButton_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles startButton.Click
        startButton.Enabled = False
        taskRunning = True

        Try
            ' Create the task and channel
            myTask = New Task
            myTask.AOChannels.CreateVoltageChannel( _
                physicalChannelComboBox.Text, _
                "aoChannel", _
                Double.Parse(minimumValueTextBox.Text), _
                Double.Parse(maximumValueTextBox.Text), _
                AOVoltageUnits.Volts)

            ' Verify the task before doing the waveform calculations
            myTask.Control(TaskAction.Verify)

            ' Calculate some waveform parameters and generate data
            Dim fGen As New FunctionGenerator( _
                myTask.Timing, _
                Double.Parse(frequencyTextBox.Text), _
                CDbl(samplesPerBufferNumericUpDown.Value), _
                CDbl(cyclesPerBufferNumericUpDown.Value), _
                CType(signalTypeComboBox.SelectedIndex, WaveformType), _
                CDbl(amplitudeNumericUpDown.Value))

            ' Configure the sample clock with the calculated rate
            myTask.Timing.ConfigureSampleClock( _
                "", _
                fGen.ResultingSampleClockRate, _
                SampleClockActiveEdge.Rising, _
                SampleQuantityMode.FiniteSamples, _
                fGen.Data.Length)

            ' Setup the triggering
            myTask.Triggers.StartTrigger.ConfigureDigitalEdgeTrigger(digitalTriggerTextBox.Text, edge)

            Dim writer As New AnalogSingleChannelWriter(myTask.Stream)

            ' Setup the Task Done event
            AddHandler myTask.Done, AddressOf myTask_Done

            writer.WriteMultiSample(False, fGen.Data)
            myTask.Start()
        Catch x As System.Exception
            MessageBox.Show(x.Message)

            If Not (myTask Is Nothing) Then
                myTask.Dispose()
            End If

            taskRunning = False
            startButton.Enabled = True
        End Try
    End Sub

    Private Sub myTask_Done(ByVal sender As Object, ByVal e As TaskDoneEventArgs)
        If Not (e.Error Is Nothing) Then
            MessageBox.Show(e.Error.Message)
        End If

        If Not (myTask Is Nothing) Then
            myTask.Dispose()
        End If

        taskRunning = False
        startButton.Enabled = True
    End Sub

    Private Sub risingRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles risingRadioButton.CheckedChanged
        If risingRadioButton.Checked Then
            edge = DigitalEdgeStartTriggerEdge.Rising
        Else
            edge = DigitalEdgeStartTriggerEdge.Falling
        End If
    End Sub

    Protected Overrides Sub OnClosing(ByVal e As System.ComponentModel.CancelEventArgs)
        e.Cancel = taskRunning
    End Sub
End Class
