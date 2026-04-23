'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   ContGenVoltageWfmIntClk_AnalogStart
'
' Category:
'   AO
'
' Description:
'   This example demonstrates how to continuously output a periodic waveform
'   using an internal clock and an analog trigger signal.
'
' Instructions for running:
'   1.  Select the physical channel corresponding to where your signal is output
'       on the DAQ device.
'   2.  Enter the minimum and maximum voltage values.
'   3.  Enter the desired frequency for the generation. The onboard sample clock
'       will operate at this rate.
'   4.  Specify the analog trigger source, slope, and level.
'   5.  Select the desired waveform type.
'   6.  The rest of the parameters in the Function Generator Parameters section
'       will affect the way the waveform is created, before it's sent to the
'       analog output of the board. Select the amplitude, number of samples per
'       buffer, and the number of cycles per buffer to be used as waveform data.
'
' Steps:
'   1.  Create a new task and an analog output voltage channel.
'   2.  Define the parameters for the analog trigger source. Additionally,
'       define the sample mode to be continuous.
'   3.  Create a AnalogSingleChannelWriter and call the WriteMultiSample method
'       to write the waveform to a buffer.
'   4.  When the user presses the stop button, stop the task.
'   5.  Dispose the Task object to clean-up any resources associated with the
'       task.
'   6.  Handle any DaqExceptions, if they occur.
'
' I/O Connections Overview:
'   Make sure your signal output terminal matches the physical channel text box.
'   In this 
'   case the signal will output to the ao0 pin on your DAQ device. Wire in the
'   analog trigger signal to APFI0 
'   or the pin of your choice.  Make sure to pass in the same terminal name to the
'   parameter of the ConfigureSampleClock 
'   method.  APFI0 is the default Analog Trigger pin for M Series devices.  For
'   more information on the input 
'   and output terminals for your device, open the NI-DAQmx Help, and refer to the
'   NI-DAQmx Device Terminals 
'   and Device Considerations books in the table of contents.
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

    Private triggerSlope As AnalogEdgeStartTriggerSlope
    Private myTask As Task

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()
        Application.EnableVisualStyles()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        FunctionGenerator.InitComboBox(signalTypeComboBox)
        triggerSlope = AnalogEdgeStartTriggerSlope.Rising
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
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents triggerParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents triggerLevelNumericUpDown As System.Windows.Forms.NumericUpDown
    Friend WithEvents triggerLevelLabel As System.Windows.Forms.Label
    Friend WithEvents fallingRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents risingRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents edgeLabel As System.Windows.Forms.Label
    Friend WithEvents triggerSourceTextBox As System.Windows.Forms.TextBox
    Friend WithEvents triggerSourceLabel As System.Windows.Forms.Label
    Friend WithEvents functionGeneratorGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents waveformTypeLabel As System.Windows.Forms.Label
    Friend WithEvents signalTypeComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents startButton As System.Windows.Forms.Button
    Friend WithEvents TimingParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents frequencyTextBox As System.Windows.Forms.TextBox
    Friend WithEvents frequencyLabel As System.Windows.Forms.Label
    Friend WithEvents channelParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents maximumValueTextBox As System.Windows.Forms.TextBox
    Friend WithEvents minimumValueTextBox As System.Windows.Forms.TextBox
    Friend WithEvents maximumValueLabel As System.Windows.Forms.Label
    Friend WithEvents minimumValueLabel As System.Windows.Forms.Label
    Friend WithEvents physicalChannelLabel As System.Windows.Forms.Label
    Friend WithEvents stopButton As System.Windows.Forms.Button
    Friend WithEvents samplesPerBufferLabel As System.Windows.Forms.Label
    Friend WithEvents cyclesPerBufferLabel As System.Windows.Forms.Label
    Friend WithEvents cyclesPerBuffer As System.Windows.Forms.NumericUpDown
    Friend WithEvents amplitude As System.Windows.Forms.NumericUpDown
    Friend WithEvents amplitudeLabel As System.Windows.Forms.Label
    Friend WithEvents samplesPerBuffer As System.Windows.Forms.NumericUpDown
    Friend WithEvents triggerSourceInfoAsterisk As System.Windows.Forms.Label
    Friend WithEvents triggerSourceInfo As System.Windows.Forms.Label
    Friend WithEvents physicalChannelComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents hysteresisLabel As System.Windows.Forms.Label
    Friend WithEvents hysteresisNumeric As System.Windows.Forms.NumericUpDown
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MainForm))
        Me.triggerParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.triggerSourceInfoAsterisk = New System.Windows.Forms.Label
        Me.triggerSourceInfo = New System.Windows.Forms.Label
        Me.triggerLevelNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.triggerLevelLabel = New System.Windows.Forms.Label
        Me.fallingRadioButton = New System.Windows.Forms.RadioButton
        Me.risingRadioButton = New System.Windows.Forms.RadioButton
        Me.edgeLabel = New System.Windows.Forms.Label
        Me.triggerSourceTextBox = New System.Windows.Forms.TextBox
        Me.triggerSourceLabel = New System.Windows.Forms.Label
        Me.hysteresisLabel = New System.Windows.Forms.Label
        Me.hysteresisNumeric = New System.Windows.Forms.NumericUpDown
        Me.functionGeneratorGroupBox = New System.Windows.Forms.GroupBox
        Me.samplesPerBuffer = New System.Windows.Forms.NumericUpDown
        Me.samplesPerBufferLabel = New System.Windows.Forms.Label
        Me.cyclesPerBufferLabel = New System.Windows.Forms.Label
        Me.cyclesPerBuffer = New System.Windows.Forms.NumericUpDown
        Me.waveformTypeLabel = New System.Windows.Forms.Label
        Me.signalTypeComboBox = New System.Windows.Forms.ComboBox
        Me.amplitude = New System.Windows.Forms.NumericUpDown
        Me.amplitudeLabel = New System.Windows.Forms.Label
        Me.startButton = New System.Windows.Forms.Button
        Me.TimingParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.frequencyTextBox = New System.Windows.Forms.TextBox
        Me.frequencyLabel = New System.Windows.Forms.Label
        Me.channelParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.physicalChannelComboBox = New System.Windows.Forms.ComboBox
        Me.maximumValueTextBox = New System.Windows.Forms.TextBox
        Me.minimumValueTextBox = New System.Windows.Forms.TextBox
        Me.maximumValueLabel = New System.Windows.Forms.Label
        Me.minimumValueLabel = New System.Windows.Forms.Label
        Me.physicalChannelLabel = New System.Windows.Forms.Label
        Me.stopButton = New System.Windows.Forms.Button
        Me.triggerParametersGroupBox.SuspendLayout()
        CType(Me.triggerLevelNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.hysteresisNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.functionGeneratorGroupBox.SuspendLayout()
        CType(Me.samplesPerBuffer, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cyclesPerBuffer, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.amplitude, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TimingParametersGroupBox.SuspendLayout()
        Me.channelParametersGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'triggerParametersGroupBox
        '
        Me.triggerParametersGroupBox.Controls.Add(Me.triggerSourceInfoAsterisk)
        Me.triggerParametersGroupBox.Controls.Add(Me.triggerSourceInfo)
        Me.triggerParametersGroupBox.Controls.Add(Me.triggerLevelNumericUpDown)
        Me.triggerParametersGroupBox.Controls.Add(Me.triggerLevelLabel)
        Me.triggerParametersGroupBox.Controls.Add(Me.fallingRadioButton)
        Me.triggerParametersGroupBox.Controls.Add(Me.risingRadioButton)
        Me.triggerParametersGroupBox.Controls.Add(Me.edgeLabel)
        Me.triggerParametersGroupBox.Controls.Add(Me.triggerSourceTextBox)
        Me.triggerParametersGroupBox.Controls.Add(Me.triggerSourceLabel)
        Me.triggerParametersGroupBox.Controls.Add(Me.hysteresisLabel)
        Me.triggerParametersGroupBox.Controls.Add(Me.hysteresisNumeric)
        Me.triggerParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.triggerParametersGroupBox.Location = New System.Drawing.Point(8, 224)
        Me.triggerParametersGroupBox.Name = "triggerParametersGroupBox"
        Me.triggerParametersGroupBox.Size = New System.Drawing.Size(272, 216)
        Me.triggerParametersGroupBox.TabIndex = 4
        Me.triggerParametersGroupBox.TabStop = False
        Me.triggerParametersGroupBox.Text = "Trigger Parameters"
        '
        'triggerSourceInfoAsterisk
        '
        Me.triggerSourceInfoAsterisk.Location = New System.Drawing.Point(16, 152)
        Me.triggerSourceInfoAsterisk.Name = "triggerSourceInfoAsterisk"
        Me.triggerSourceInfoAsterisk.Size = New System.Drawing.Size(8, 8)
        Me.triggerSourceInfoAsterisk.TabIndex = 7
        Me.triggerSourceInfoAsterisk.Text = "*"
        '
        'triggerSourceInfo
        '
        Me.triggerSourceInfo.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.triggerSourceInfo.Location = New System.Drawing.Point(24, 152)
        Me.triggerSourceInfo.Name = "triggerSourceInfo"
        Me.triggerSourceInfo.Size = New System.Drawing.Size(240, 56)
        Me.triggerSourceInfo.TabIndex = 8
        Me.triggerSourceInfo.Text = "APFI0 is the default Analog Trigger pin for M Series devices.  Please refer to you" & _
        "r device documentation for information regarding valid Analog Triggers for your " & _
        "device."
        '
        'triggerLevelNumericUpDown
        '
        Me.triggerLevelNumericUpDown.DecimalPlaces = 2
        Me.triggerLevelNumericUpDown.Location = New System.Drawing.Point(136, 88)
        Me.triggerLevelNumericUpDown.Maximum = New Decimal(New Integer() {20, 0, 0, 0})
        Me.triggerLevelNumericUpDown.Minimum = New Decimal(New Integer() {20, 0, 0, -2147483648})
        Me.triggerLevelNumericUpDown.Name = "triggerLevelNumericUpDown"
        Me.triggerLevelNumericUpDown.TabIndex = 6
        Me.triggerLevelNumericUpDown.Value = New Decimal(New Integer() {5, 0, 0, 65536})
        '
        'triggerLevelLabel
        '
        Me.triggerLevelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.triggerLevelLabel.Location = New System.Drawing.Point(16, 88)
        Me.triggerLevelLabel.Name = "triggerLevelLabel"
        Me.triggerLevelLabel.Size = New System.Drawing.Size(100, 16)
        Me.triggerLevelLabel.TabIndex = 5
        Me.triggerLevelLabel.Text = "Trigger Level:"
        '
        'fallingRadioButton
        '
        Me.fallingRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.fallingRadioButton.Location = New System.Drawing.Point(200, 56)
        Me.fallingRadioButton.Name = "fallingRadioButton"
        Me.fallingRadioButton.Size = New System.Drawing.Size(64, 24)
        Me.fallingRadioButton.TabIndex = 4
        Me.fallingRadioButton.Text = "Falling"
        '
        'risingRadioButton
        '
        Me.risingRadioButton.Checked = True
        Me.risingRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.risingRadioButton.Location = New System.Drawing.Point(136, 56)
        Me.risingRadioButton.Name = "risingRadioButton"
        Me.risingRadioButton.Size = New System.Drawing.Size(64, 24)
        Me.risingRadioButton.TabIndex = 3
        Me.risingRadioButton.TabStop = True
        Me.risingRadioButton.Text = "Rising"
        '
        'edgeLabel
        '
        Me.edgeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.edgeLabel.Location = New System.Drawing.Point(16, 56)
        Me.edgeLabel.Name = "edgeLabel"
        Me.edgeLabel.Size = New System.Drawing.Size(100, 16)
        Me.edgeLabel.TabIndex = 2
        Me.edgeLabel.Text = "Trigger Slope:"
        '
        'triggerSourceTextBox
        '
        Me.triggerSourceTextBox.Location = New System.Drawing.Point(136, 24)
        Me.triggerSourceTextBox.Name = "triggerSourceTextBox"
        Me.triggerSourceTextBox.Size = New System.Drawing.Size(120, 20)
        Me.triggerSourceTextBox.TabIndex = 1
        Me.triggerSourceTextBox.Text = "/Dev1/APFI0"
        '
        'triggerSourceLabel
        '
        Me.triggerSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.triggerSourceLabel.Location = New System.Drawing.Point(16, 24)
        Me.triggerSourceLabel.Name = "triggerSourceLabel"
        Me.triggerSourceLabel.Size = New System.Drawing.Size(128, 24)
        Me.triggerSourceLabel.TabIndex = 0
        Me.triggerSourceLabel.Text = "Trigger Source:"
        '
        'hysteresisLabel
        '
        Me.hysteresisLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.hysteresisLabel.Location = New System.Drawing.Point(16, 120)
        Me.hysteresisLabel.Name = "hysteresisLabel"
        Me.hysteresisLabel.Size = New System.Drawing.Size(100, 16)
        Me.hysteresisLabel.TabIndex = 5
        Me.hysteresisLabel.Text = "Hysteresis (V):"
        '
        'hysteresisNumeric
        '
        Me.hysteresisNumeric.DecimalPlaces = 2
        Me.hysteresisNumeric.Location = New System.Drawing.Point(136, 120)
        Me.hysteresisNumeric.Maximum = New Decimal(New Integer() {20, 0, 0, 0})
        Me.hysteresisNumeric.Minimum = New Decimal(New Integer() {20, 0, 0, -2147483648})
        Me.hysteresisNumeric.Name = "hysteresisNumeric"
        Me.hysteresisNumeric.TabIndex = 6
        '
        'functionGeneratorGroupBox
        '
        Me.functionGeneratorGroupBox.Controls.Add(Me.samplesPerBuffer)
        Me.functionGeneratorGroupBox.Controls.Add(Me.samplesPerBufferLabel)
        Me.functionGeneratorGroupBox.Controls.Add(Me.cyclesPerBufferLabel)
        Me.functionGeneratorGroupBox.Controls.Add(Me.cyclesPerBuffer)
        Me.functionGeneratorGroupBox.Controls.Add(Me.waveformTypeLabel)
        Me.functionGeneratorGroupBox.Controls.Add(Me.signalTypeComboBox)
        Me.functionGeneratorGroupBox.Controls.Add(Me.amplitude)
        Me.functionGeneratorGroupBox.Controls.Add(Me.amplitudeLabel)
        Me.functionGeneratorGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.functionGeneratorGroupBox.Location = New System.Drawing.Point(296, 8)
        Me.functionGeneratorGroupBox.Name = "functionGeneratorGroupBox"
        Me.functionGeneratorGroupBox.Size = New System.Drawing.Size(232, 176)
        Me.functionGeneratorGroupBox.TabIndex = 5
        Me.functionGeneratorGroupBox.TabStop = False
        Me.functionGeneratorGroupBox.Text = "Function Generator Parameters"
        '
        'samplesPerBuffer
        '
        Me.samplesPerBuffer.DecimalPlaces = 1
        Me.samplesPerBuffer.Location = New System.Drawing.Point(120, 96)
        Me.samplesPerBuffer.Maximum = New Decimal(New Integer() {10000000, 0, 0, 0})
        Me.samplesPerBuffer.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.samplesPerBuffer.Name = "samplesPerBuffer"
        Me.samplesPerBuffer.Size = New System.Drawing.Size(104, 20)
        Me.samplesPerBuffer.TabIndex = 5
        Me.samplesPerBuffer.Value = New Decimal(New Integer() {250, 0, 0, 0})
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
        'cyclesPerBuffer
        '
        Me.cyclesPerBuffer.DecimalPlaces = 1
        Me.cyclesPerBuffer.Location = New System.Drawing.Point(120, 56)
        Me.cyclesPerBuffer.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.cyclesPerBuffer.Name = "cyclesPerBuffer"
        Me.cyclesPerBuffer.Size = New System.Drawing.Size(104, 20)
        Me.cyclesPerBuffer.TabIndex = 3
        Me.cyclesPerBuffer.Value = New Decimal(New Integer() {50, 0, 0, 65536})
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
        Me.signalTypeComboBox.ItemHeight = 13
        Me.signalTypeComboBox.Location = New System.Drawing.Point(120, 24)
        Me.signalTypeComboBox.Name = "signalTypeComboBox"
        Me.signalTypeComboBox.Size = New System.Drawing.Size(104, 21)
        Me.signalTypeComboBox.TabIndex = 1
        '
        'amplitude
        '
        Me.amplitude.DecimalPlaces = 1
        Me.amplitude.Location = New System.Drawing.Point(120, 136)
        Me.amplitude.Maximum = New Decimal(New Integer() {10000000, 0, 0, 0})
        Me.amplitude.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.amplitude.Name = "amplitude"
        Me.amplitude.Size = New System.Drawing.Size(104, 20)
        Me.amplitude.TabIndex = 7
        Me.amplitude.Value = New Decimal(New Integer() {2, 0, 0, 0})
        '
        'amplitudeLabel
        '
        Me.amplitudeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.amplitudeLabel.Location = New System.Drawing.Point(16, 136)
        Me.amplitudeLabel.Name = "amplitudeLabel"
        Me.amplitudeLabel.Size = New System.Drawing.Size(96, 32)
        Me.amplitudeLabel.TabIndex = 6
        Me.amplitudeLabel.Text = "Amplitude:"
        '
        'startButton
        '
        Me.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.startButton.Location = New System.Drawing.Point(360, 296)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(120, 23)
        Me.startButton.TabIndex = 0
        Me.startButton.Text = "Start"
        '
        'TimingParametersGroupBox
        '
        Me.TimingParametersGroupBox.Controls.Add(Me.frequencyTextBox)
        Me.TimingParametersGroupBox.Controls.Add(Me.frequencyLabel)
        Me.TimingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.TimingParametersGroupBox.Location = New System.Drawing.Point(8, 152)
        Me.TimingParametersGroupBox.Name = "TimingParametersGroupBox"
        Me.TimingParametersGroupBox.Size = New System.Drawing.Size(272, 64)
        Me.TimingParametersGroupBox.TabIndex = 3
        Me.TimingParametersGroupBox.TabStop = False
        Me.TimingParametersGroupBox.Text = "Timing Parameters"
        '
        'frequencyTextBox
        '
        Me.frequencyTextBox.Location = New System.Drawing.Point(136, 24)
        Me.frequencyTextBox.Name = "frequencyTextBox"
        Me.frequencyTextBox.Size = New System.Drawing.Size(120, 20)
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
        Me.channelParametersGroupBox.Size = New System.Drawing.Size(272, 136)
        Me.channelParametersGroupBox.TabIndex = 2
        Me.channelParametersGroupBox.TabStop = False
        Me.channelParametersGroupBox.Text = "Channel Parameters"
        '
        'physicalChannelComboBox
        '
        Me.physicalChannelComboBox.Location = New System.Drawing.Point(136, 24)
        Me.physicalChannelComboBox.Name = "physicalChannelComboBox"
        Me.physicalChannelComboBox.Size = New System.Drawing.Size(121, 21)
        Me.physicalChannelComboBox.TabIndex = 1
        Me.physicalChannelComboBox.Text = "Dev1/ao0"
        '
        'maximumValueTextBox
        '
        Me.maximumValueTextBox.Location = New System.Drawing.Point(136, 96)
        Me.maximumValueTextBox.Name = "maximumValueTextBox"
        Me.maximumValueTextBox.Size = New System.Drawing.Size(120, 20)
        Me.maximumValueTextBox.TabIndex = 5
        Me.maximumValueTextBox.Text = "10"
        '
        'minimumValueTextBox
        '
        Me.minimumValueTextBox.Location = New System.Drawing.Point(136, 60)
        Me.minimumValueTextBox.Name = "minimumValueTextBox"
        Me.minimumValueTextBox.Size = New System.Drawing.Size(120, 20)
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
        'stopButton
        '
        Me.stopButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.stopButton.Location = New System.Drawing.Point(360, 320)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(120, 23)
        Me.stopButton.TabIndex = 1
        Me.stopButton.Text = "Stop"
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(546, 448)
        Me.Controls.Add(Me.stopButton)
        Me.Controls.Add(Me.triggerParametersGroupBox)
        Me.Controls.Add(Me.functionGeneratorGroupBox)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.TimingParametersGroupBox)
        Me.Controls.Add(Me.channelParametersGroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Continuous Generate Voltage Internal Clock - Analog Start"
        Me.triggerParametersGroupBox.ResumeLayout(False)
        CType(Me.triggerLevelNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.hysteresisNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.functionGeneratorGroupBox.ResumeLayout(False)
        CType(Me.samplesPerBuffer, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cyclesPerBuffer, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.amplitude, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TimingParametersGroupBox.ResumeLayout(False)
        Me.channelParametersGroupBox.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub startButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles startButton.Click
        System.Windows.Forms.Cursor.Current = Cursors.WaitCursor
        Try
            Dim minV, maxV, triggerLevel As Double
            minV = Double.Parse(minimumValueTextBox.Text)
            maxV = Double.Parse(maximumValueTextBox.Text)
            triggerLevel = CDbl(triggerLevelNumericUpDown.Value)

            ' Create the task and channel
            myTask = New Task()
            myTask.AOChannels.CreateVoltageChannel( _
                physicalChannelComboBox.Text, _
                "myChan", _
                minV, _
                maxV, _
                AOVoltageUnits.Volts)

            ' Verify the task before doing the waveform calculations
            myTask.Control(TaskAction.Verify)

            ' Calculate some waveform parameters and generate data
            Dim fGen as New FunctionGenerator( _
                myTask.Timing, _
                frequencyTextBox.Text, _
                samplesPerBuffer.Text, _
                cyclesPerBuffer.Text, _
                signalTypeComboBox.Text, _
                amplitude.Text)

            ' Configure the sample clock with the calculated rate
            myTask.Timing.ConfigureSampleClock( _
                "", _
                fGen.ResultingSampleClockRate, _
                SampleClockActiveEdge.Rising, _
                SampleQuantityMode.ContinuousSamples, 1000)

            ' Setup the triggering
            myTask.Triggers.StartTrigger.ConfigureAnalogEdgeTrigger(triggerSourceTextBox.Text, _
                triggerSlope, triggerLevel)

            myTask.Triggers.StartTrigger.AnalogEdge.Hysteresis = Convert.ToDouble(hysteresisNumeric.Value)

            ' Set up the Task Done event handler
            AddHandler myTask.Done, AddressOf myTask_Done

            ' Write the data
            Dim writer As New AnalogSingleChannelWriter(myTask.Stream)
            writer.WriteMultiSample(False, fGen.Data)
            myTask.Start()

            ' Set up UI
            startButton.Enabled = False
            stopButton.Enabled = True
            channelParametersGroupBox.Enabled = False
            TimingParametersGroupBox.Enabled = False
            triggerParametersGroupBox.Enabled = False
            functionGeneratorGroupBox.Enabled = False
        Catch x As System.Exception
            myTask.Dispose()

            MessageBox.Show(x.Message)

            startButton.Enabled = True
            stopButton.Enabled = False
            channelParametersGroupBox.Enabled = True
            TimingParametersGroupBox.Enabled = True
            triggerParametersGroupBox.Enabled = True
            functionGeneratorGroupBox.Enabled = True
        End Try
        System.Windows.Forms.Cursor.Current = Cursors.Default
    End Sub

    Private Sub risingRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles risingRadioButton.CheckedChanged
        triggerSlope = AnalogEdgeStartTriggerSlope.Rising
    End Sub

    Private Sub fallingRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles fallingRadioButton.CheckedChanged
        triggerSlope = AnalogEdgeStartTriggerSlope.Falling
    End Sub

    Private Sub stopButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles stopButton.Click
        If Not myTask Is Nothing Then
            myTask.Stop()
            myTask.Dispose()
        End If

        startButton.Enabled = True
        stopButton.Enabled = False
        channelParametersGroupBox.Enabled = True
        TimingParametersGroupBox.Enabled = True
        triggerParametersGroupBox.Enabled = True
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
        TimingParametersGroupBox.Enabled = True
        triggerParametersGroupBox.Enabled = True
        functionGeneratorGroupBox.Enabled = True
    End Sub
End Class
