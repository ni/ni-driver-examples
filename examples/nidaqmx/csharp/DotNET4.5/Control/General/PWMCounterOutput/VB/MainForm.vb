'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   PWMCounterOutput
'
' Category:
'   Control
'
' Description:
'   This example demonstrates how to do Pulse Width Modulation using Analog
'   Input and Counter Output.
'
' Instructions for running:
'   1.  Select the physical channel to correspond to where your signal is input
'       on the DAQ device. Also, select the corresponding channel for where your
'       signal is being generated.
'   2.  Enter the minimum and maximum voltage ranges.Note:  For better accuracy
'       try to match the input range to the expected voltage level of the
'       measured signal.
'   3.  Set the sample rate of the acquisition.Note:  The rate should be at
'       least twice as fast as the maximum frequency component of the signal
'       being acquired.
'   4.  Set the initial frequency and duty cycle of the output pulse.
'
' Steps:
'   1.  Create an analog input voltage channel. Also, create acounter output
'       pulse channel.
'   2.  Set the rate for the sample clock of the analog input and thecounter
'       output. Additionally, define the sample mode to behardware timed single
'       point for both tasks.
'   3.  Call the GetDeviceName function. This willtake a task and a terminal and
'       create a properly formatteddevice + terminal name to use as the source
'       of the sampleclock for the CO task. By sharing the sample clocks the
'       taskswill be synchronized.
'   4.  Call the Start function to arm the two functions. Make surethe counter
'       output is armed before the analog input. Thiswill ensure both will start
'       at the same time.
'   5.  Call AnalogSingleChannelReader.BeginReadSingleSample to install a
'       callback and begin the asynchronous read operation.
'   6.  Inside the callback, call AnalogSingleChannelReader.EndReadSingleSample
'       to retrieve the data from the read operation.  Pass this data to
'       DutyCycleCalculation method to determine if the duty cycle has changed.
'       If the duty cycle has changed, then we need to write this new value by
'       calling CounterSingleChannelReader.WriteSingleSample.
'   7.  Call AnalogSingleChannelReader.BeginReadSingleSample again inside the
'       callback to perform another read operation.
'   8.  Dispose the Task object to clean-up any resources associated with the
'       task.
'   9.  Handle any DaqExceptions, if they occur.
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
'   Make sure your signal input/output terminals match the Physical Channel I/O
'   controls.Note: For this example to work you must ensure you have identified
'   your PXI chassis in MAX.
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

Public Class MainForm
    Inherits System.Windows.Forms.Form

    Private writer As CounterSingleChannelWriter
    Private reader As AnalogSingleChannelReader
    Private inputCallback As AsyncCallback
    Private counterTask As Task
    Private analogInputTask As Task
    Private runningAnalogTask As Task
    Private runningCounterTask As Task
    Private idleState As COPulseIdleState
    Private COData As CODataFrequency
    Private dutyCycle As Double
    Private lastPoint As Double
    Private point As Double
    Private tempDutyCycle As Double
    Private minimumInputValue As Double = -10
    Private maximumInputValue As Double = 10
    Friend WithEvents counterOutputPhysicalChannelComboBox As System.Windows.Forms.ComboBox
    Private dutyCycleChanged As Boolean = False


#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()
        Application.EnableVisualStyles()
        Application.DoEvents()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        idleState = COPulseIdleState.Low

        inputPhysicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AI, PhysicalChannelAccess.External))
        counterOutputPhysicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.CO, PhysicalChannelAccess.External))

        If (inputPhysicalChannelComboBox.Items.Count > 0) Then
            inputPhysicalChannelComboBox.SelectedIndex = 0
        End If

        If (counterOutputPhysicalChannelComboBox.Items.Count > 0) Then
            counterOutputPhysicalChannelComboBox.SelectedIndex = 0
        End If

        If (inputPhysicalChannelComboBox.Items.Count > 0 And counterOutputPhysicalChannelComboBox.Items.Count > 0) Then
            startButton.Enabled = True
        End If

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then

            If Not (components Is Nothing) Then
                components.Dispose()
            End If

            If Not (analogInputTask Is Nothing) Then
                runningAnalogTask = Nothing
                analogInputTask.Dispose()
            End If

            If Not (counterTask Is Nothing) Then
                runningCounterTask = Nothing
                counterTask.Dispose()
            End If

        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents outputChannelParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents dutyCycleNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents dutyCycleLabel As System.Windows.Forms.Label
    Friend WithEvents counterOutputPhysicalChannelLabel As System.Windows.Forms.Label
    Friend WithEvents frequencyLabel As System.Windows.Forms.Label
    Friend WithEvents frequencyNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents inputChannelParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents inputMinimumValueLabel As System.Windows.Forms.Label
    Friend WithEvents inputPhysicalChannelLabel As System.Windows.Forms.Label
    Friend WithEvents inputMaximumValueLabel As System.Windows.Forms.Label
    Friend WithEvents inputMinimumValueNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents inputMaximumValueNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents startButton As System.Windows.Forms.Button
    Friend WithEvents stopButton As System.Windows.Forms.Button
    Friend WithEvents controlParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents setPointLabel As System.Windows.Forms.Label
    Friend WithEvents proportionalGainNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents setPointNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents proportionalGainLabel As System.Windows.Forms.Label
    Friend WithEvents timingParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents inputRateLabel As System.Windows.Forms.Label
    Friend WithEvents inputRateNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents inputPhysicalChannelComboBox As System.Windows.Forms.ComboBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MainForm))
        Me.outputChannelParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.counterOutputPhysicalChannelComboBox = New System.Windows.Forms.ComboBox
        Me.dutyCycleNumeric = New System.Windows.Forms.NumericUpDown
        Me.dutyCycleLabel = New System.Windows.Forms.Label
        Me.counterOutputPhysicalChannelLabel = New System.Windows.Forms.Label
        Me.frequencyLabel = New System.Windows.Forms.Label
        Me.frequencyNumeric = New System.Windows.Forms.NumericUpDown
        Me.inputChannelParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.inputPhysicalChannelComboBox = New System.Windows.Forms.ComboBox
        Me.inputMinimumValueLabel = New System.Windows.Forms.Label
        Me.inputPhysicalChannelLabel = New System.Windows.Forms.Label
        Me.inputMaximumValueLabel = New System.Windows.Forms.Label
        Me.inputMinimumValueNumeric = New System.Windows.Forms.NumericUpDown
        Me.inputMaximumValueNumeric = New System.Windows.Forms.NumericUpDown
        Me.startButton = New System.Windows.Forms.Button
        Me.stopButton = New System.Windows.Forms.Button
        Me.controlParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.setPointLabel = New System.Windows.Forms.Label
        Me.proportionalGainNumeric = New System.Windows.Forms.NumericUpDown
        Me.setPointNumeric = New System.Windows.Forms.NumericUpDown
        Me.proportionalGainLabel = New System.Windows.Forms.Label
        Me.timingParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.inputRateLabel = New System.Windows.Forms.Label
        Me.inputRateNumeric = New System.Windows.Forms.NumericUpDown
        Me.outputChannelParametersGroupBox.SuspendLayout()
        CType(Me.dutyCycleNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.frequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.inputChannelParametersGroupBox.SuspendLayout()
        CType(Me.inputMinimumValueNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.inputMaximumValueNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.controlParametersGroupBox.SuspendLayout()
        CType(Me.proportionalGainNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.setPointNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.timingParametersGroupBox.SuspendLayout()
        CType(Me.inputRateNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'outputChannelParametersGroupBox
        '
        Me.outputChannelParametersGroupBox.Controls.Add(Me.counterOutputPhysicalChannelComboBox)
        Me.outputChannelParametersGroupBox.Controls.Add(Me.dutyCycleNumeric)
        Me.outputChannelParametersGroupBox.Controls.Add(Me.dutyCycleLabel)
        Me.outputChannelParametersGroupBox.Controls.Add(Me.counterOutputPhysicalChannelLabel)
        Me.outputChannelParametersGroupBox.Controls.Add(Me.frequencyLabel)
        Me.outputChannelParametersGroupBox.Controls.Add(Me.frequencyNumeric)
        Me.outputChannelParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.outputChannelParametersGroupBox.Location = New System.Drawing.Point(16, 168)
        Me.outputChannelParametersGroupBox.Name = "outputChannelParametersGroupBox"
        Me.outputChannelParametersGroupBox.Size = New System.Drawing.Size(304, 136)
        Me.outputChannelParametersGroupBox.TabIndex = 1
        Me.outputChannelParametersGroupBox.TabStop = False
        Me.outputChannelParametersGroupBox.Text = "Output Channel Parameters"
        '
        'counterOutputPhysicalChannelComboBox
        '
        Me.counterOutputPhysicalChannelComboBox.Location = New System.Drawing.Point(216, 19)
        Me.counterOutputPhysicalChannelComboBox.Name = "counterOutputPhysicalChannelComboBox"
        Me.counterOutputPhysicalChannelComboBox.Size = New System.Drawing.Size(80, 21)
        Me.counterOutputPhysicalChannelComboBox.TabIndex = 1
        Me.counterOutputPhysicalChannelComboBox.Text = "Dev1/ctr0"
        '
        'dutyCycleNumeric
        '
        Me.dutyCycleNumeric.DecimalPlaces = 2
        Me.dutyCycleNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.dutyCycleNumeric.Location = New System.Drawing.Point(216, 104)
        Me.dutyCycleNumeric.Maximum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.dutyCycleNumeric.Name = "dutyCycleNumeric"
        Me.dutyCycleNumeric.Size = New System.Drawing.Size(80, 20)
        Me.dutyCycleNumeric.TabIndex = 5
        Me.dutyCycleNumeric.Value = New Decimal(New Integer() {5, 0, 0, 65536})
        '
        'dutyCycleLabel
        '
        Me.dutyCycleLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.dutyCycleLabel.Location = New System.Drawing.Point(16, 106)
        Me.dutyCycleLabel.Name = "dutyCycleLabel"
        Me.dutyCycleLabel.Size = New System.Drawing.Size(104, 16)
        Me.dutyCycleLabel.TabIndex = 4
        Me.dutyCycleLabel.Text = "Duty Cycle"
        '
        'counterOutputPhysicalChannelLabel
        '
        Me.counterOutputPhysicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.counterOutputPhysicalChannelLabel.Location = New System.Drawing.Point(16, 21)
        Me.counterOutputPhysicalChannelLabel.Name = "counterOutputPhysicalChannelLabel"
        Me.counterOutputPhysicalChannelLabel.Size = New System.Drawing.Size(160, 16)
        Me.counterOutputPhysicalChannelLabel.TabIndex = 0
        Me.counterOutputPhysicalChannelLabel.Text = "Counter Output Physical Channel"
        '
        'frequencyLabel
        '
        Me.frequencyLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.frequencyLabel.Location = New System.Drawing.Point(16, 66)
        Me.frequencyLabel.Name = "frequencyLabel"
        Me.frequencyLabel.Size = New System.Drawing.Size(112, 16)
        Me.frequencyLabel.TabIndex = 2
        Me.frequencyLabel.Text = "Frequency"
        '
        'frequencyNumeric
        '
        Me.frequencyNumeric.Location = New System.Drawing.Point(216, 64)
        Me.frequencyNumeric.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.frequencyNumeric.Name = "frequencyNumeric"
        Me.frequencyNumeric.Size = New System.Drawing.Size(80, 20)
        Me.frequencyNumeric.TabIndex = 3
        Me.frequencyNumeric.Value = New Decimal(New Integer() {100000, 0, 0, 0})
        '
        'inputChannelParametersGroupBox
        '
        Me.inputChannelParametersGroupBox.Controls.Add(Me.inputPhysicalChannelComboBox)
        Me.inputChannelParametersGroupBox.Controls.Add(Me.inputMinimumValueLabel)
        Me.inputChannelParametersGroupBox.Controls.Add(Me.inputPhysicalChannelLabel)
        Me.inputChannelParametersGroupBox.Controls.Add(Me.inputMaximumValueLabel)
        Me.inputChannelParametersGroupBox.Controls.Add(Me.inputMinimumValueNumeric)
        Me.inputChannelParametersGroupBox.Controls.Add(Me.inputMaximumValueNumeric)
        Me.inputChannelParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.inputChannelParametersGroupBox.Location = New System.Drawing.Point(16, 16)
        Me.inputChannelParametersGroupBox.Name = "inputChannelParametersGroupBox"
        Me.inputChannelParametersGroupBox.Size = New System.Drawing.Size(304, 136)
        Me.inputChannelParametersGroupBox.TabIndex = 0
        Me.inputChannelParametersGroupBox.TabStop = False
        Me.inputChannelParametersGroupBox.Text = "Input Channel Parameters"
        '
        'inputPhysicalChannelComboBox
        '
        Me.inputPhysicalChannelComboBox.Location = New System.Drawing.Point(216, 24)
        Me.inputPhysicalChannelComboBox.Name = "inputPhysicalChannelComboBox"
        Me.inputPhysicalChannelComboBox.Size = New System.Drawing.Size(80, 21)
        Me.inputPhysicalChannelComboBox.TabIndex = 1
        Me.inputPhysicalChannelComboBox.Text = "Dev1/ai0"
        '
        'inputMinimumValueLabel
        '
        Me.inputMinimumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.inputMinimumValueLabel.Location = New System.Drawing.Point(16, 66)
        Me.inputMinimumValueLabel.Name = "inputMinimumValueLabel"
        Me.inputMinimumValueLabel.Size = New System.Drawing.Size(112, 16)
        Me.inputMinimumValueLabel.TabIndex = 2
        Me.inputMinimumValueLabel.Text = "Input Minimum Value"
        '
        'inputPhysicalChannelLabel
        '
        Me.inputPhysicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.inputPhysicalChannelLabel.Location = New System.Drawing.Point(16, 26)
        Me.inputPhysicalChannelLabel.Name = "inputPhysicalChannelLabel"
        Me.inputPhysicalChannelLabel.Size = New System.Drawing.Size(128, 16)
        Me.inputPhysicalChannelLabel.TabIndex = 0
        Me.inputPhysicalChannelLabel.Text = "Input Physical Channel"
        '
        'inputMaximumValueLabel
        '
        Me.inputMaximumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.inputMaximumValueLabel.Location = New System.Drawing.Point(16, 106)
        Me.inputMaximumValueLabel.Name = "inputMaximumValueLabel"
        Me.inputMaximumValueLabel.Size = New System.Drawing.Size(120, 16)
        Me.inputMaximumValueLabel.TabIndex = 4
        Me.inputMaximumValueLabel.Text = "Input Maximum Value"
        '
        'inputMinimumValueNumeric
        '
        Me.inputMinimumValueNumeric.DecimalPlaces = 1
        Me.inputMinimumValueNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.inputMinimumValueNumeric.Location = New System.Drawing.Point(216, 64)
        Me.inputMinimumValueNumeric.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.inputMinimumValueNumeric.Minimum = New Decimal(New Integer() {10, 0, 0, -2147483648})
        Me.inputMinimumValueNumeric.Name = "inputMinimumValueNumeric"
        Me.inputMinimumValueNumeric.Size = New System.Drawing.Size(80, 20)
        Me.inputMinimumValueNumeric.TabIndex = 3
        Me.inputMinimumValueNumeric.Value = New Decimal(New Integer() {100, 0, 0, -2147418112})
        '
        'inputMaximumValueNumeric
        '
        Me.inputMaximumValueNumeric.DecimalPlaces = 1
        Me.inputMaximumValueNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.inputMaximumValueNumeric.Location = New System.Drawing.Point(216, 104)
        Me.inputMaximumValueNumeric.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.inputMaximumValueNumeric.Minimum = New Decimal(New Integer() {10, 0, 0, -2147483648})
        Me.inputMaximumValueNumeric.Name = "inputMaximumValueNumeric"
        Me.inputMaximumValueNumeric.Size = New System.Drawing.Size(80, 20)
        Me.inputMaximumValueNumeric.TabIndex = 5
        Me.inputMaximumValueNumeric.Value = New Decimal(New Integer() {100, 0, 0, 65536})
        '
        'startButton
        '
        Me.startButton.Enabled = False
        Me.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.startButton.Location = New System.Drawing.Point(64, 512)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(80, 24)
        Me.startButton.TabIndex = 4
        Me.startButton.Text = "Start"
        '
        'stopButton
        '
        Me.stopButton.Enabled = False
        Me.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.stopButton.Location = New System.Drawing.Point(192, 512)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(80, 24)
        Me.stopButton.TabIndex = 5
        Me.stopButton.Text = "Stop"
        '
        'controlParametersGroupBox
        '
        Me.controlParametersGroupBox.Controls.Add(Me.setPointLabel)
        Me.controlParametersGroupBox.Controls.Add(Me.proportionalGainNumeric)
        Me.controlParametersGroupBox.Controls.Add(Me.setPointNumeric)
        Me.controlParametersGroupBox.Controls.Add(Me.proportionalGainLabel)
        Me.controlParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.controlParametersGroupBox.Location = New System.Drawing.Point(16, 392)
        Me.controlParametersGroupBox.Name = "controlParametersGroupBox"
        Me.controlParametersGroupBox.Size = New System.Drawing.Size(304, 104)
        Me.controlParametersGroupBox.TabIndex = 3
        Me.controlParametersGroupBox.TabStop = False
        Me.controlParametersGroupBox.Text = "Control Parameters"
        '
        'setPointLabel
        '
        Me.setPointLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.setPointLabel.Location = New System.Drawing.Point(16, 26)
        Me.setPointLabel.Name = "setPointLabel"
        Me.setPointLabel.Size = New System.Drawing.Size(72, 16)
        Me.setPointLabel.TabIndex = 0
        Me.setPointLabel.Text = "Set Point"
        '
        'proportionalGainNumeric
        '
        Me.proportionalGainNumeric.Location = New System.Drawing.Point(216, 64)
        Me.proportionalGainNumeric.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.proportionalGainNumeric.Minimum = New Decimal(New Integer() {1000, 0, 0, -2147483648})
        Me.proportionalGainNumeric.Name = "proportionalGainNumeric"
        Me.proportionalGainNumeric.Size = New System.Drawing.Size(80, 20)
        Me.proportionalGainNumeric.TabIndex = 3
        Me.proportionalGainNumeric.Value = New Decimal(New Integer() {5, 0, 0, 0})
        '
        'setPointNumeric
        '
        Me.setPointNumeric.Location = New System.Drawing.Point(216, 24)
        Me.setPointNumeric.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.setPointNumeric.Minimum = New Decimal(New Integer() {1000, 0, 0, -2147483648})
        Me.setPointNumeric.Name = "setPointNumeric"
        Me.setPointNumeric.Size = New System.Drawing.Size(80, 20)
        Me.setPointNumeric.TabIndex = 1
        Me.setPointNumeric.Value = New Decimal(New Integer() {8, 0, 0, 0})
        '
        'proportionalGainLabel
        '
        Me.proportionalGainLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.proportionalGainLabel.Location = New System.Drawing.Point(16, 66)
        Me.proportionalGainLabel.Name = "proportionalGainLabel"
        Me.proportionalGainLabel.Size = New System.Drawing.Size(96, 16)
        Me.proportionalGainLabel.TabIndex = 2
        Me.proportionalGainLabel.Text = "Proportional Gain"
        '
        'timingParametersGroupBox
        '
        Me.timingParametersGroupBox.Controls.Add(Me.inputRateLabel)
        Me.timingParametersGroupBox.Controls.Add(Me.inputRateNumeric)
        Me.timingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.timingParametersGroupBox.Location = New System.Drawing.Point(16, 320)
        Me.timingParametersGroupBox.Name = "timingParametersGroupBox"
        Me.timingParametersGroupBox.Size = New System.Drawing.Size(304, 56)
        Me.timingParametersGroupBox.TabIndex = 2
        Me.timingParametersGroupBox.TabStop = False
        Me.timingParametersGroupBox.Text = "Timing Parameters"
        '
        'inputRateLabel
        '
        Me.inputRateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.inputRateLabel.Location = New System.Drawing.Point(16, 26)
        Me.inputRateLabel.Name = "inputRateLabel"
        Me.inputRateLabel.Size = New System.Drawing.Size(96, 16)
        Me.inputRateLabel.TabIndex = 0
        Me.inputRateLabel.Text = "Input Rate"
        '
        'inputRateNumeric
        '
        Me.inputRateNumeric.Location = New System.Drawing.Point(216, 24)
        Me.inputRateNumeric.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.inputRateNumeric.Minimum = New Decimal(New Integer() {100000, 0, 0, -2147483648})
        Me.inputRateNumeric.Name = "inputRateNumeric"
        Me.inputRateNumeric.Size = New System.Drawing.Size(80, 20)
        Me.inputRateNumeric.TabIndex = 1
        Me.inputRateNumeric.Value = New Decimal(New Integer() {1000, 0, 0, 0})
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(336, 550)
        Me.Controls.Add(Me.outputChannelParametersGroupBox)
        Me.Controls.Add(Me.inputChannelParametersGroupBox)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.stopButton)
        Me.Controls.Add(Me.controlParametersGroupBox)
        Me.Controls.Add(Me.timingParametersGroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Pulse Width Modulation-Counter Output"
        Me.outputChannelParametersGroupBox.ResumeLayout(False)
        CType(Me.dutyCycleNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.frequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.inputChannelParametersGroupBox.ResumeLayout(False)
        CType(Me.inputMinimumValueNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.inputMaximumValueNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.controlParametersGroupBox.ResumeLayout(False)
        CType(Me.proportionalGainNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.setPointNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.timingParametersGroupBox.ResumeLayout(False)
        CType(Me.inputRateNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region


    Private Sub startButton_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles startButton.Click
        If (runningAnalogTask Is Nothing) Then

            Try

                ' Initialize dutyCycle
                dutyCycle = Convert.ToDouble(dutyCycleNumeric.Value)

                ' Initialize minimumInputValue and maximumInputValue
                minimumInputValue = inputMinimumValueNumeric.Value
                maximumInputValue = inputMaximumValueNumeric.Value

                ' Create tasks
                counterTask = New Task("Counter Output")
                analogInputTask = New Task("Analog Input")

                ' Configure channels
                analogInputTask.AIChannels.CreateVoltageChannel(inputPhysicalChannelComboBox.Text, "Analog Input", NationalInstruments.DAQmx.AITerminalConfiguration.Differential, Convert.ToDouble(inputMinimumValueNumeric.Value), Convert.ToDouble(inputMaximumValueNumeric.Value), AIVoltageUnits.Volts)
                counterTask.COChannels.CreatePulseChannelFrequency(counterOutputPhysicalChannelComboBox.Text, "PWM Channel", COPulseFrequencyUnits.Hertz, idleState, 0.0, Convert.ToDouble(frequencyNumeric.Value), Convert.ToDouble(dutyCycleNumeric.Value))

                ' Set up the timing for both tasks
                analogInputTask.Timing.ConfigureSampleClock("", Convert.ToDouble(inputRateNumeric.Value), NationalInstruments.DAQmx.SampleClockActiveEdge.Rising, SampleQuantityMode.HardwareTimedSinglePoint)

                ' Use the same timing source for the PWM Output
                Dim deviceName As String = inputPhysicalChannelComboBox.Text.Split("/")(0)
                Dim terminalNameBase As String = "/" + GetDeviceName(deviceName) + "/"

                counterTask.Timing.ConfigureSampleClock(terminalNameBase + "ai/SampleClock", Convert.ToDouble(Convert.ToDouble(inputRateNumeric.Value)), SampleClockActiveEdge.Rising, SampleQuantityMode.HardwareTimedSinglePoint)

                ' Start tasks
                StartTask()

                ' Start PWM as well
                inputCallback = New AsyncCallback(AddressOf InputRead)
                reader = New AnalogSingleChannelReader(analogInputTask.Stream)
                writer = New CounterSingleChannelWriter(counterTask.Stream)
                COData = New CODataFrequency(Convert.ToDouble(frequencyNumeric.Value), dutyCycle)

                ' Use SynchronizeCallbacks to specify that the object 
                ' marshals callbacks across threads appropriately.
                reader.SynchronizeCallbacks = True
                reader.BeginReadSingleSample(inputCallback, analogInputTask)


            Catch ex As System.Exception
                StopTask()
                MessageBox.Show(ex.Message)

            End Try
        End If
    End Sub

    Private Sub stopButton_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles stopButton.Click
        If Not (runningCounterTask Is Nothing) Then
            StopTask()
        End If
    End Sub

    Private Sub InputRead(ByVal ar As IAsyncResult)
        Try
            If (Not (runningAnalogTask Is Nothing)) AndAlso (runningAnalogTask Is ar.AsyncState) Then

                ' Calculate PWM duty cycle based on the set point, gain, and input
                dutyCycleChanged = DutyCycleCalculation(reader.EndReadSingleSample(ar), Convert.ToDouble(setPointNumeric.Value), Convert.ToDouble(proportionalGainNumeric.Value))

                If (dutyCycleChanged) Then
                    ' Change duty cycle
                    COData.DutyCycle = dutyCycle
                    writer.WriteSingleSample(True, COData)
                End If

                ' Wait for the next sample clock
                analogInputTask.Timing.SinglePoint.WaitForNextSampleClock()

                ' Set up next callback
                reader.BeginReadSingleSample(inputCallback, analogInputTask)

            End If

        Catch ex As Exception
            StopTask()
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    ' Returns true if duty cycle needs to be changed and edits duty cycle, otherwise, returns true and leaves dutyCycle unchanged
    Private Function DutyCycleCalculation(ByVal input As Double, ByVal setPoint As Double, ByVal proportionalGain As Double) As Boolean

        point = (input - setPoint) * proportionalGain

        ' If new point is not within .5 of old point, update the output
        If (point <= lastPoint - 0.5 Or point >= lastPoint + 0.5) Then
            lastPoint = point
            tempDutyCycle = (input - minimumInputValue) / (maximumInputValue - minimumInputValue)

            If (tempDutyCycle > 0.999) Then
                dutyCycle = 0.999
            ElseIf (tempDutyCycle < 0.001) Then
                dutyCycle = 0.001
            Else
                dutyCycle = tempDutyCycle
            End If

            Return True
        Else
            Return False
        End If
    End Function


    Public Function GetDeviceName(ByVal deviceName As String) As String
        Dim device As Device = DaqSystem.Local.LoadDevice(deviceName)

        If (device.BusType <> DeviceBusType.CompactDaq) Then
            Return deviceName
        Else
            Return device.CompactDaqChassisDeviceName()
        End If
    End Function

    Private Sub StartTask()
        If (runningAnalogTask Is Nothing) Then

            ' Change state
            runningAnalogTask = analogInputTask
            runningCounterTask = counterTask

            startButton.Enabled = False
            stopButton.Enabled = True

            counterTask.Start()
            analogInputTask.Start()
        End If
    End Sub

    Private Sub StopTask()

        ' Change State
        runningAnalogTask = Nothing
        runningCounterTask = Nothing

        stopButton.Enabled = False
        startButton.Enabled = True
        counterTask.Stop()

        analogInputTask.Dispose()
        counterTask.Dispose()
    End Sub
End Class
