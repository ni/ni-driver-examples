'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   ContGenVoltageWfm_ExtClkDigStart
'
' Category:
'   AO
'
' Description:
'   This example demonstrates how to continuously output a waveform using an
'   external sample clock and a digital start trigger.
'
' Instructions for running:
'   1.  Select the physical channel corresponding to where your signal is output
'       on the DAQ device.
'   2.  Enter the minimum and maximum voltage values.
'   3.  Select the sample clock source.
'   4.  Set the signal frequency of the generation.
'   5.  Select the digital trigger source.
'   6.  Specify the desired trigger edge.
'   7.  Select the desired waveform type.
'   8.  The rest of the parameters in the Function Generator Parameters section
'       will affect the way the waveform is created, before it's sent to the
'       analog output of the board. Select the amplitude, number of samples per
'       buffer, and the number of cycles per buffer to be used as waveform data.
'
' Steps:
'   1.  Create a new task and an analog output voltage channel.
'   2.  Specify the external sample clock source. In this example, the external
'       sample clock source defaults to the signal on PFI7. Additionally, define
'       the sample mode to be continuous.
'   3.  Specify the source and edge triggering parameters. In this example,
'       source defaults to PFI0.
'   4.  Create a AnalogSingleChannelWriter and call the WriteMultiSample method
'       to write the waveform to a buffer.
'   5.  Call Task.Start().
'   6.  When the user presses the stop button, stop the task.
'   7.  Dispose the Task object to clean-up any resources associated with the
'       task.
'   8.  Handle any DaqExceptions, if they occur.
'
' I/O Connections Overview:
'   Make sure your signal output terminal matches the text in the physical
'   channel text box. 
'   In this case the signal will output to the ao0 pin on your DAQ Device. Wire in
'   the external sample clock 
'   to a PFI or RTSI pin of your choice on the board. Specify the same terminal
'   name as an argument to the 
'   ConfigureSampleClock method (PFI7 is used as an example). Also, make sure your
'   digital trigger terminal 
'   matches the text in the digital trigger text box (PFI0 is used as an example).
'   For more information on 
'   the input and output terminals for your device, open the NI-DAQmx Help, and
'   refer to the NI-DAQmx Device 
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

Imports NationalInstruments.DAQmx

Public Class MainForm
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()
        Application.EnableVisualStyles()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        FunctionGenerator.InitComboBox(signalTypeComboBox)
        edge = DigitalEdgeStartTriggerEdge.Rising
        startButton.Enabled = True
        stopButton.Enabled = False

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
            If Not (myTask Is Nothing) Then
                myTask.Dispose()
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
    Friend WithEvents triggeringGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents fallingRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents risingRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents digitalTriggerSourceTextBox As System.Windows.Forms.TextBox
    Friend WithEvents digitalTriggerEdgeLabel As System.Windows.Forms.Label
    Friend WithEvents digitalTriggerLabel As System.Windows.Forms.Label
    Friend WithEvents timingParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents desiredFrequencyNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents frequencyLabel As System.Windows.Forms.Label
    Friend WithEvents clockSourceTextBox As System.Windows.Forms.TextBox
    Friend WithEvents clockSourceLabel As System.Windows.Forms.Label
    Friend WithEvents functionGeneratorGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents amplitudeLabel As System.Windows.Forms.Label
    Friend WithEvents amplitudeNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents samplesPerBufferNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents samplesPerBufferLabel As System.Windows.Forms.Label
    Friend WithEvents cyclesPerBufferLabel As System.Windows.Forms.Label
    Friend WithEvents cyclesPerBufferNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents waveformTypeLabel As System.Windows.Forms.Label
    Friend WithEvents signalTypeComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents channelParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents maximumValueLabel As System.Windows.Forms.Label
    Friend WithEvents minimumValueLabel As System.Windows.Forms.Label
    Friend WithEvents physicalChannelLabel As System.Windows.Forms.Label
    Friend WithEvents maximumValueNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents minimumValueNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents startButton As System.Windows.Forms.Button
    Friend WithEvents physicalChannelComboBox As System.Windows.Forms.ComboBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MainForm))
        Me.stopButton = New System.Windows.Forms.Button
        Me.triggeringGroupBox = New System.Windows.Forms.GroupBox
        Me.fallingRadioButton = New System.Windows.Forms.RadioButton
        Me.risingRadioButton = New System.Windows.Forms.RadioButton
        Me.digitalTriggerSourceTextBox = New System.Windows.Forms.TextBox
        Me.digitalTriggerEdgeLabel = New System.Windows.Forms.Label
        Me.digitalTriggerLabel = New System.Windows.Forms.Label
        Me.timingParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.desiredFrequencyNumeric = New System.Windows.Forms.NumericUpDown
        Me.frequencyLabel = New System.Windows.Forms.Label
        Me.clockSourceTextBox = New System.Windows.Forms.TextBox
        Me.clockSourceLabel = New System.Windows.Forms.Label
        Me.functionGeneratorGroupBox = New System.Windows.Forms.GroupBox
        Me.amplitudeLabel = New System.Windows.Forms.Label
        Me.amplitudeNumeric = New System.Windows.Forms.NumericUpDown
        Me.samplesPerBufferNumeric = New System.Windows.Forms.NumericUpDown
        Me.samplesPerBufferLabel = New System.Windows.Forms.Label
        Me.cyclesPerBufferLabel = New System.Windows.Forms.Label
        Me.cyclesPerBufferNumeric = New System.Windows.Forms.NumericUpDown
        Me.waveformTypeLabel = New System.Windows.Forms.Label
        Me.signalTypeComboBox = New System.Windows.Forms.ComboBox
        Me.channelParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.physicalChannelComboBox = New System.Windows.Forms.ComboBox
        Me.maximumValueLabel = New System.Windows.Forms.Label
        Me.minimumValueLabel = New System.Windows.Forms.Label
        Me.physicalChannelLabel = New System.Windows.Forms.Label
        Me.maximumValueNumeric = New System.Windows.Forms.NumericUpDown
        Me.minimumValueNumeric = New System.Windows.Forms.NumericUpDown
        Me.startButton = New System.Windows.Forms.Button
        Me.triggeringGroupBox.SuspendLayout()
        Me.timingParametersGroupBox.SuspendLayout()
        CType(Me.desiredFrequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.functionGeneratorGroupBox.SuspendLayout()
        CType(Me.amplitudeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.samplesPerBufferNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cyclesPerBufferNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.channelParametersGroupBox.SuspendLayout()
        CType(Me.maximumValueNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.minimumValueNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'stopButton
        '
        Me.stopButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.stopButton.Enabled = False
        Me.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.stopButton.Location = New System.Drawing.Point(360, 240)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.TabIndex = 1
        Me.stopButton.Text = "Stop"
        '
        'triggeringGroupBox
        '
        Me.triggeringGroupBox.Controls.Add(Me.fallingRadioButton)
        Me.triggeringGroupBox.Controls.Add(Me.risingRadioButton)
        Me.triggeringGroupBox.Controls.Add(Me.digitalTriggerSourceTextBox)
        Me.triggeringGroupBox.Controls.Add(Me.digitalTriggerEdgeLabel)
        Me.triggeringGroupBox.Controls.Add(Me.digitalTriggerLabel)
        Me.triggeringGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.triggeringGroupBox.Location = New System.Drawing.Point(9, 256)
        Me.triggeringGroupBox.Name = "triggeringGroupBox"
        Me.triggeringGroupBox.Size = New System.Drawing.Size(256, 104)
        Me.triggeringGroupBox.TabIndex = 4
        Me.triggeringGroupBox.TabStop = False
        Me.triggeringGroupBox.Text = "Triggering Parameters"
        '
        'fallingRadioButton
        '
        Me.fallingRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.fallingRadioButton.Location = New System.Drawing.Point(136, 80)
        Me.fallingRadioButton.Name = "fallingRadioButton"
        Me.fallingRadioButton.Size = New System.Drawing.Size(56, 16)
        Me.fallingRadioButton.TabIndex = 4
        Me.fallingRadioButton.Text = "Falling"
        '
        'risingRadioButton
        '
        Me.risingRadioButton.Checked = True
        Me.risingRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.risingRadioButton.Location = New System.Drawing.Point(136, 56)
        Me.risingRadioButton.Name = "risingRadioButton"
        Me.risingRadioButton.Size = New System.Drawing.Size(56, 24)
        Me.risingRadioButton.TabIndex = 3
        Me.risingRadioButton.TabStop = True
        Me.risingRadioButton.Text = "Rising"
        '
        'digitalTriggerSourceTextBox
        '
        Me.digitalTriggerSourceTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.digitalTriggerSourceTextBox.Location = New System.Drawing.Point(144, 24)
        Me.digitalTriggerSourceTextBox.Name = "digitalTriggerSourceTextBox"
        Me.digitalTriggerSourceTextBox.Size = New System.Drawing.Size(96, 20)
        Me.digitalTriggerSourceTextBox.TabIndex = 1
        Me.digitalTriggerSourceTextBox.Text = "/Dev1/PFI0"
        '
        'digitalTriggerEdgeLabel
        '
        Me.digitalTriggerEdgeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.digitalTriggerEdgeLabel.Location = New System.Drawing.Point(16, 56)
        Me.digitalTriggerEdgeLabel.Name = "digitalTriggerEdgeLabel"
        Me.digitalTriggerEdgeLabel.Size = New System.Drawing.Size(112, 16)
        Me.digitalTriggerEdgeLabel.TabIndex = 2
        Me.digitalTriggerEdgeLabel.Text = "Digital Trigger Edge:"
        '
        'digitalTriggerLabel
        '
        Me.digitalTriggerLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.digitalTriggerLabel.Location = New System.Drawing.Point(16, 26)
        Me.digitalTriggerLabel.Name = "digitalTriggerLabel"
        Me.digitalTriggerLabel.Size = New System.Drawing.Size(120, 16)
        Me.digitalTriggerLabel.TabIndex = 0
        Me.digitalTriggerLabel.Text = "Digital Trigger Source:"
        '
        'timingParametersGroupBox
        '
        Me.timingParametersGroupBox.Controls.Add(Me.desiredFrequencyNumeric)
        Me.timingParametersGroupBox.Controls.Add(Me.frequencyLabel)
        Me.timingParametersGroupBox.Controls.Add(Me.clockSourceTextBox)
        Me.timingParametersGroupBox.Controls.Add(Me.clockSourceLabel)
        Me.timingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.timingParametersGroupBox.Location = New System.Drawing.Point(9, 152)
        Me.timingParametersGroupBox.Name = "timingParametersGroupBox"
        Me.timingParametersGroupBox.Size = New System.Drawing.Size(256, 96)
        Me.timingParametersGroupBox.TabIndex = 3
        Me.timingParametersGroupBox.TabStop = False
        Me.timingParametersGroupBox.Text = "Timing Parameters"
        '
        'desiredFrequencyNumeric
        '
        Me.desiredFrequencyNumeric.Location = New System.Drawing.Point(144, 56)
        Me.desiredFrequencyNumeric.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.desiredFrequencyNumeric.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.desiredFrequencyNumeric.Name = "desiredFrequencyNumeric"
        Me.desiredFrequencyNumeric.Size = New System.Drawing.Size(96, 20)
        Me.desiredFrequencyNumeric.TabIndex = 3
        Me.desiredFrequencyNumeric.Value = New Decimal(New Integer() {1000, 0, 0, 0})
        '
        'frequencyLabel
        '
        Me.frequencyLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.frequencyLabel.Location = New System.Drawing.Point(16, 58)
        Me.frequencyLabel.Name = "frequencyLabel"
        Me.frequencyLabel.Size = New System.Drawing.Size(128, 16)
        Me.frequencyLabel.TabIndex = 2
        Me.frequencyLabel.Text = "Desired Frequency (Hz):"
        '
        'clockSourceTextBox
        '
        Me.clockSourceTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.clockSourceTextBox.Location = New System.Drawing.Point(144, 24)
        Me.clockSourceTextBox.Name = "clockSourceTextBox"
        Me.clockSourceTextBox.Size = New System.Drawing.Size(96, 20)
        Me.clockSourceTextBox.TabIndex = 1
        Me.clockSourceTextBox.Text = "/Dev1/PFI7"
        '
        'clockSourceLabel
        '
        Me.clockSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.clockSourceLabel.Location = New System.Drawing.Point(16, 26)
        Me.clockSourceLabel.Name = "clockSourceLabel"
        Me.clockSourceLabel.Size = New System.Drawing.Size(96, 16)
        Me.clockSourceLabel.TabIndex = 0
        Me.clockSourceLabel.Text = "Clock Source:"
        '
        'functionGeneratorGroupBox
        '
        Me.functionGeneratorGroupBox.Controls.Add(Me.amplitudeLabel)
        Me.functionGeneratorGroupBox.Controls.Add(Me.amplitudeNumeric)
        Me.functionGeneratorGroupBox.Controls.Add(Me.samplesPerBufferNumeric)
        Me.functionGeneratorGroupBox.Controls.Add(Me.samplesPerBufferLabel)
        Me.functionGeneratorGroupBox.Controls.Add(Me.cyclesPerBufferLabel)
        Me.functionGeneratorGroupBox.Controls.Add(Me.cyclesPerBufferNumeric)
        Me.functionGeneratorGroupBox.Controls.Add(Me.waveformTypeLabel)
        Me.functionGeneratorGroupBox.Controls.Add(Me.signalTypeComboBox)
        Me.functionGeneratorGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.functionGeneratorGroupBox.Location = New System.Drawing.Point(281, 8)
        Me.functionGeneratorGroupBox.Name = "functionGeneratorGroupBox"
        Me.functionGeneratorGroupBox.Size = New System.Drawing.Size(232, 176)
        Me.functionGeneratorGroupBox.TabIndex = 5
        Me.functionGeneratorGroupBox.TabStop = False
        Me.functionGeneratorGroupBox.Text = "Function Generator Parameters"
        '
        'amplitudeLabel
        '
        Me.amplitudeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.amplitudeLabel.Location = New System.Drawing.Point(16, 138)
        Me.amplitudeLabel.Name = "amplitudeLabel"
        Me.amplitudeLabel.Size = New System.Drawing.Size(72, 16)
        Me.amplitudeLabel.TabIndex = 6
        Me.amplitudeLabel.Text = "Amplitude:"
        '
        'amplitudeNumeric
        '
        Me.amplitudeNumeric.DecimalPlaces = 1
        Me.amplitudeNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.amplitudeNumeric.Location = New System.Drawing.Point(120, 136)
        Me.amplitudeNumeric.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.amplitudeNumeric.Name = "amplitudeNumeric"
        Me.amplitudeNumeric.Size = New System.Drawing.Size(96, 20)
        Me.amplitudeNumeric.TabIndex = 7
        Me.amplitudeNumeric.Value = New Decimal(New Integer() {2, 0, 0, 0})
        '
        'samplesPerBufferNumeric
        '
        Me.samplesPerBufferNumeric.Location = New System.Drawing.Point(120, 96)
        Me.samplesPerBufferNumeric.Maximum = New Decimal(New Integer() {10000000, 0, 0, 0})
        Me.samplesPerBufferNumeric.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.samplesPerBufferNumeric.Name = "samplesPerBufferNumeric"
        Me.samplesPerBufferNumeric.Size = New System.Drawing.Size(96, 20)
        Me.samplesPerBufferNumeric.TabIndex = 5
        Me.samplesPerBufferNumeric.Value = New Decimal(New Integer() {250, 0, 0, 0})
        '
        'samplesPerBufferLabel
        '
        Me.samplesPerBufferLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.samplesPerBufferLabel.Location = New System.Drawing.Point(16, 98)
        Me.samplesPerBufferLabel.Name = "samplesPerBufferLabel"
        Me.samplesPerBufferLabel.Size = New System.Drawing.Size(120, 16)
        Me.samplesPerBufferLabel.TabIndex = 4
        Me.samplesPerBufferLabel.Text = "Samples Per Buffer:"
        '
        'cyclesPerBufferLabel
        '
        Me.cyclesPerBufferLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.cyclesPerBufferLabel.Location = New System.Drawing.Point(16, 62)
        Me.cyclesPerBufferLabel.Name = "cyclesPerBufferLabel"
        Me.cyclesPerBufferLabel.Size = New System.Drawing.Size(104, 16)
        Me.cyclesPerBufferLabel.TabIndex = 2
        Me.cyclesPerBufferLabel.Text = "Cycles Per Buffer:"
        '
        'cyclesPerBufferNumeric
        '
        Me.cyclesPerBufferNumeric.Location = New System.Drawing.Point(120, 60)
        Me.cyclesPerBufferNumeric.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.cyclesPerBufferNumeric.Name = "cyclesPerBufferNumeric"
        Me.cyclesPerBufferNumeric.Size = New System.Drawing.Size(96, 20)
        Me.cyclesPerBufferNumeric.TabIndex = 3
        Me.cyclesPerBufferNumeric.Value = New Decimal(New Integer() {5, 0, 0, 0})
        '
        'waveformTypeLabel
        '
        Me.waveformTypeLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.waveformTypeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.waveformTypeLabel.Location = New System.Drawing.Point(16, 26)
        Me.waveformTypeLabel.Name = "waveformTypeLabel"
        Me.waveformTypeLabel.Size = New System.Drawing.Size(88, 16)
        Me.waveformTypeLabel.TabIndex = 0
        Me.waveformTypeLabel.Text = "Waveform Type:"
        '
        'signalTypeComboBox
        '
        Me.signalTypeComboBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.signalTypeComboBox.ItemHeight = 13
        Me.signalTypeComboBox.Location = New System.Drawing.Point(120, 24)
        Me.signalTypeComboBox.Name = "signalTypeComboBox"
        Me.signalTypeComboBox.Size = New System.Drawing.Size(96, 21)
        Me.signalTypeComboBox.TabIndex = 1
        Me.signalTypeComboBox.Text = "Sine Wave"
        '
        'channelParametersGroupBox
        '
        Me.channelParametersGroupBox.Controls.Add(Me.physicalChannelComboBox)
        Me.channelParametersGroupBox.Controls.Add(Me.maximumValueLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.minimumValueLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.physicalChannelLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.maximumValueNumeric)
        Me.channelParametersGroupBox.Controls.Add(Me.minimumValueNumeric)
        Me.channelParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.channelParametersGroupBox.Location = New System.Drawing.Point(9, 8)
        Me.channelParametersGroupBox.Name = "channelParametersGroupBox"
        Me.channelParametersGroupBox.Size = New System.Drawing.Size(256, 136)
        Me.channelParametersGroupBox.TabIndex = 2
        Me.channelParametersGroupBox.TabStop = False
        Me.channelParametersGroupBox.Text = "Channel Parameters"
        '
        'physicalChannelComboBox
        '
        Me.physicalChannelComboBox.Location = New System.Drawing.Point(144, 24)
        Me.physicalChannelComboBox.Name = "physicalChannelComboBox"
        Me.physicalChannelComboBox.Size = New System.Drawing.Size(96, 21)
        Me.physicalChannelComboBox.TabIndex = 1
        Me.physicalChannelComboBox.Text = "Dev1/ao0"
        '
        'maximumValueLabel
        '
        Me.maximumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.maximumValueLabel.Location = New System.Drawing.Point(16, 98)
        Me.maximumValueLabel.Name = "maximumValueLabel"
        Me.maximumValueLabel.Size = New System.Drawing.Size(112, 16)
        Me.maximumValueLabel.TabIndex = 4
        Me.maximumValueLabel.Text = "Maximum Value (V):"
        '
        'minimumValueLabel
        '
        Me.minimumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.minimumValueLabel.Location = New System.Drawing.Point(16, 62)
        Me.minimumValueLabel.Name = "minimumValueLabel"
        Me.minimumValueLabel.Size = New System.Drawing.Size(104, 16)
        Me.minimumValueLabel.TabIndex = 2
        Me.minimumValueLabel.Text = "Minimum Value (V):"
        '
        'physicalChannelLabel
        '
        Me.physicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.physicalChannelLabel.Location = New System.Drawing.Point(16, 26)
        Me.physicalChannelLabel.Name = "physicalChannelLabel"
        Me.physicalChannelLabel.Size = New System.Drawing.Size(96, 16)
        Me.physicalChannelLabel.TabIndex = 0
        Me.physicalChannelLabel.Text = "Physical Channel:"
        '
        'maximumValueNumeric
        '
        Me.maximumValueNumeric.Location = New System.Drawing.Point(144, 96)
        Me.maximumValueNumeric.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.maximumValueNumeric.Minimum = New Decimal(New Integer() {10, 0, 0, -2147483648})
        Me.maximumValueNumeric.Name = "maximumValueNumeric"
        Me.maximumValueNumeric.Size = New System.Drawing.Size(96, 20)
        Me.maximumValueNumeric.TabIndex = 5
        Me.maximumValueNumeric.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'minimumValueNumeric
        '
        Me.minimumValueNumeric.Location = New System.Drawing.Point(144, 60)
        Me.minimumValueNumeric.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.minimumValueNumeric.Minimum = New Decimal(New Integer() {10, 0, 0, -2147483648})
        Me.minimumValueNumeric.Name = "minimumValueNumeric"
        Me.minimumValueNumeric.Size = New System.Drawing.Size(96, 20)
        Me.minimumValueNumeric.TabIndex = 3
        Me.minimumValueNumeric.Value = New Decimal(New Integer() {10, 0, 0, -2147483648})
        '
        'startButton
        '
        Me.startButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.startButton.Location = New System.Drawing.Point(360, 208)
        Me.startButton.Name = "startButton"
        Me.startButton.TabIndex = 0
        Me.startButton.Text = "Start"
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(522, 376)
        Me.Controls.Add(Me.stopButton)
        Me.Controls.Add(Me.triggeringGroupBox)
        Me.Controls.Add(Me.timingParametersGroupBox)
        Me.Controls.Add(Me.functionGeneratorGroupBox)
        Me.Controls.Add(Me.channelParametersGroupBox)
        Me.Controls.Add(Me.startButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(528, 408)
        Me.MinimumSize = New System.Drawing.Size(528, 408)
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Continuous Voltage Generation - External Clock - Digital Start"
        Me.triggeringGroupBox.ResumeLayout(False)
        Me.timingParametersGroupBox.ResumeLayout(False)
        CType(Me.desiredFrequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.functionGeneratorGroupBox.ResumeLayout(False)
        CType(Me.amplitudeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.samplesPerBufferNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cyclesPerBufferNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.channelParametersGroupBox.ResumeLayout(False)
        CType(Me.maximumValueNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.minimumValueNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private myTask As Task
    Private edge As DigitalEdgeStartTriggerEdge

    Private Sub startButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles startButton.Click
        System.Windows.Forms.Cursor.Current = Cursors.WaitCursor
        Try
            ' Create task and channel
            myTask = New Task()
            myTask.AOChannels.CreateVoltageChannel(physicalChannelComboBox.Text, _
                "", _
                Convert.ToDouble(minimumValueNumeric.Value), _
                Convert.ToDouble(maximumValueNumeric.Value), _
                AOVoltageUnits.Volts)

            ' Verify the task before doing the waveform calculations
            myTask.Control(TaskAction.Verify)

            ' Calculate some waveform parameters and generate data
            Dim fGen as New FunctionGenerator( _
                myTask.Timing, _
                desiredFrequencyNumeric.Value.ToString(), _
                samplesPerBufferNumeric.Value.ToString(), _
                cyclesPerBufferNumeric.Value.ToString(), _
                signalTypeComboBox.Text, _
                amplitudeNumeric.Value.ToString())

            ' Configure the sample clock with the calculated rate
            ' and external clock source
            myTask.Timing.ConfigureSampleClock( _
                clockSourceTextBox.Text, _
                fGen.ResultingSampleClockRate, _
                SampleClockActiveEdge.Rising, _
                SampleQuantityMode.ContinuousSamples, 1000)

            ' Setup digital trigger
            myTask.Triggers.StartTrigger.ConfigureDigitalEdgeTrigger( _ 
            digitalTriggerSourceTextBox.Text, _ 
            edge)

            ' Set up Task Done event handler
            AddHandler myTask.Done, AddressOf myTask_Done

            ' Write the data
            Dim writer As New AnalogSingleChannelWriter(myTask.Stream)
            writer.WriteMultiSample(False, fGen.Data)
            myTask.Start()

            startButton.Enabled = False
            stopButton.Enabled = True
            channelParametersGroupBox.Enabled = False
            timingParametersGroupBox.Enabled = False
            triggeringGroupBox.Enabled = False
            functionGeneratorGroupBox.Enabled = False
        Catch x As System.Exception
            myTask.Dispose()

            MessageBox.Show(x.Message)

            startButton.Enabled = True
            stopButton.Enabled = False
            channelParametersGroupBox.Enabled = True
            timingParametersGroupBox.Enabled = True
            triggeringGroupBox.Enabled = True
            functionGeneratorGroupBox.Enabled = True
        End Try
        System.Windows.Forms.Cursor.Current = Cursors.Default

    End Sub

    Private Sub stopButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles stopButton.Click
        If Not myTask Is Nothing Then
            myTask.Stop()
            myTask.Dispose()
        End If

        startButton.Enabled = True
        stopButton.Enabled = False
        channelParametersGroupBox.Enabled = True
        timingParametersGroupBox.Enabled = True
        triggeringGroupBox.Enabled = True
        functionGeneratorGroupBox.Enabled = True
    End Sub

    Private Sub myTask_Done(ByVal sender As Object, ByVal e As TaskDoneEventArgs)
        If Not e.Error Is Nothing Then
            MessageBox.Show(e.Error.Message)
        End If

        If Not myTask Is Nothing Then
            myTask.Dispose()
        End If

        startButton.Enabled = True
        stopButton.Enabled = False
        channelParametersGroupBox.Enabled = True
        timingParametersGroupBox.Enabled = True
        triggeringGroupBox.Enabled = True
        functionGeneratorGroupBox.Enabled = True
    End Sub

    Private Sub risingRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles risingRadioButton.CheckedChanged
        If risingRadioButton.Checked Then
            edge = DigitalEdgeStartTriggerEdge.Rising
        Else
            edge = DigitalEdgeStartTriggerEdge.Falling
        End If
    End Sub
End Class
