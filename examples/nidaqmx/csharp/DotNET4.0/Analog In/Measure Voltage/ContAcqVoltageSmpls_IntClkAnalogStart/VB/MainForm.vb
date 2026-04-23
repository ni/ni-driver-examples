'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   ContAcqVoltageSmpls_IntClkAnalogStart
'
' Category:
'   AI
'
' Description:
'   This example demonstrates how to continuously acquire data using the DAQ
'   device's internal clock and an analog slope start trigger.
'
' Instructions for running:
'   1.  Select the physical channel corresponding to where your signal is input
'       on the DAQ device.
'   2.  Enter the minimum and maximum voltage values.Note: For better accuracy,
'       try to match the input range to the expected voltage level of the
'       measured signal.
'   3.  Set the rate of the acquisition, and the number of samples to read.Note:
'       The rate should be at least twice as fast as the maximum frequency
'       component of the signal being acquired.
'   4.  Set the source of the analog slope start trigger. The default analog
'       trigger pin for M Series devices is APFI0. Please refer to your device
'       documentation for information regarding valid analog triggers for your
'       device.
'   5.  Set the slope and level for the desired analog edge condition.
'
' Steps:
'   1.  Create a new task and an analog input voltage channel using the
'       CreateVoltageChannel method.
'   2.  Configure timing specifications for the task using
'       Timing.ConfigureSampleClock method.
'   3.  Configure the analog trigger specifications using the
'       Triggers.StartTrigger.ConfigureAnalogEdgeTrigger method.
'   4.  Call AnalogMultiChannelReader.BeginReadWaveform to install a callback
'       and begin the asynchronous read operation.
'   5.  Inside the callback, call AnalogMultiChannelReader.EndReadWaveform to
'       retrieve the data from the read operation.  
'   6.  Call AnalogMultiChannelReader.BeginMemoryOptimizedReadWaveform
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
'   Make sure your signal input terminal matches the physical channel I/O
'   control. In the default case (differential channel ai0) wire the positive
'   lead for your signal to the ACH0 pin on your DAQ device and wire the
'   negative lead for your signal to the ACH8 pin on you DAQ device. Also, make
'   sure your analog trigger terminal matches the trigger source control. For
'   more information on the input and output terminals for your device, open the
'   NI-DAQmx Help, and refer to the NI-DAQmx Device Terminals and Device
'   Considerations books in the table of contents.
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
        stopButton.Enabled = False
        myAsyncCallback = New AsyncCallback(AddressOf AnalogInCallback)

        physicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AI, PhysicalChannelAccess.External))
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
                runningTask = Nothing
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
    Friend WithEvents triggerParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents triggerSourceTextBox As System.Windows.Forms.TextBox
    Friend WithEvents slopeGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents triggerSourceLabel As System.Windows.Forms.Label
    Friend WithEvents stopButton As System.Windows.Forms.Button
    Friend WithEvents startButton As System.Windows.Forms.Button
    Friend WithEvents timingParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents rateLabel As System.Windows.Forms.Label
    Friend WithEvents samplesLabel As System.Windows.Forms.Label
    Friend WithEvents samplesPerChannelNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents channelParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents maximumLabel As System.Windows.Forms.Label
    Friend WithEvents minimumLabel As System.Windows.Forms.Label
    Friend WithEvents physicalChannelLabel As System.Windows.Forms.Label
    Friend WithEvents acquisitionResultGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents resultLabel As System.Windows.Forms.Label
    Friend WithEvents acquisitionDataGrid As System.Windows.Forms.DataGrid
    Friend WithEvents rateNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents slopeRisingButton As System.Windows.Forms.RadioButton
    Friend WithEvents slopeFallingButton As System.Windows.Forms.RadioButton
    Friend WithEvents triggerLevelLabel As System.Windows.Forms.Label
    Friend WithEvents triggerLevelNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents minimumValueNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents maximumValueNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents triggerSourceInfoAsterisk As System.Windows.Forms.Label
    Friend WithEvents triggerSourceInfo As System.Windows.Forms.Label
    Friend WithEvents physicalChannelComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents hysteresisLabel As System.Windows.Forms.Label
    Friend WithEvents hysteresisNumeric As System.Windows.Forms.NumericUpDown
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.triggerParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.triggerSourceInfoAsterisk = New System.Windows.Forms.Label
        Me.triggerSourceInfo = New System.Windows.Forms.Label
        Me.triggerLevelNumeric = New System.Windows.Forms.NumericUpDown
        Me.triggerSourceTextBox = New System.Windows.Forms.TextBox
        Me.slopeGroupBox = New System.Windows.Forms.GroupBox
        Me.slopeRisingButton = New System.Windows.Forms.RadioButton
        Me.slopeFallingButton = New System.Windows.Forms.RadioButton
        Me.triggerSourceLabel = New System.Windows.Forms.Label
        Me.triggerLevelLabel = New System.Windows.Forms.Label
        Me.hysteresisNumeric = New System.Windows.Forms.NumericUpDown
        Me.hysteresisLabel = New System.Windows.Forms.Label
        Me.stopButton = New System.Windows.Forms.Button
        Me.startButton = New System.Windows.Forms.Button
        Me.timingParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.rateLabel = New System.Windows.Forms.Label
        Me.samplesLabel = New System.Windows.Forms.Label
        Me.samplesPerChannelNumeric = New System.Windows.Forms.NumericUpDown
        Me.rateNumeric = New System.Windows.Forms.NumericUpDown
        Me.channelParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.physicalChannelComboBox = New System.Windows.Forms.ComboBox
        Me.minimumValueNumeric = New System.Windows.Forms.NumericUpDown
        Me.maximumValueNumeric = New System.Windows.Forms.NumericUpDown
        Me.maximumLabel = New System.Windows.Forms.Label
        Me.minimumLabel = New System.Windows.Forms.Label
        Me.physicalChannelLabel = New System.Windows.Forms.Label
        Me.acquisitionResultGroupBox = New System.Windows.Forms.GroupBox
        Me.resultLabel = New System.Windows.Forms.Label
        Me.acquisitionDataGrid = New System.Windows.Forms.DataGrid
        Me.triggerParametersGroupBox.SuspendLayout()
        CType(Me.triggerLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.slopeGroupBox.SuspendLayout()
        CType(Me.hysteresisNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.timingParametersGroupBox.SuspendLayout()
        CType(Me.samplesPerChannelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rateNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.channelParametersGroupBox.SuspendLayout()
        CType(Me.minimumValueNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.maximumValueNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.acquisitionResultGroupBox.SuspendLayout()
        CType(Me.acquisitionDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'triggerParametersGroupBox
        '
        Me.triggerParametersGroupBox.Controls.Add(Me.triggerSourceInfoAsterisk)
        Me.triggerParametersGroupBox.Controls.Add(Me.triggerSourceInfo)
        Me.triggerParametersGroupBox.Controls.Add(Me.triggerLevelNumeric)
        Me.triggerParametersGroupBox.Controls.Add(Me.triggerSourceTextBox)
        Me.triggerParametersGroupBox.Controls.Add(Me.slopeGroupBox)
        Me.triggerParametersGroupBox.Controls.Add(Me.triggerSourceLabel)
        Me.triggerParametersGroupBox.Controls.Add(Me.triggerLevelLabel)
        Me.triggerParametersGroupBox.Controls.Add(Me.hysteresisNumeric)
        Me.triggerParametersGroupBox.Controls.Add(Me.hysteresisLabel)
        Me.triggerParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.triggerParametersGroupBox.Location = New System.Drawing.Point(8, 232)
        Me.triggerParametersGroupBox.Name = "triggerParametersGroupBox"
        Me.triggerParametersGroupBox.Size = New System.Drawing.Size(232, 248)
        Me.triggerParametersGroupBox.TabIndex = 4
        Me.triggerParametersGroupBox.TabStop = False
        Me.triggerParametersGroupBox.Text = "Trigger Parameters"
        '
        'triggerSourceInfoAsterisk
        '
        Me.triggerSourceInfoAsterisk.Location = New System.Drawing.Point(16, 168)
        Me.triggerSourceInfoAsterisk.Name = "triggerSourceInfoAsterisk"
        Me.triggerSourceInfoAsterisk.Size = New System.Drawing.Size(8, 23)
        Me.triggerSourceInfoAsterisk.TabIndex = 5
        Me.triggerSourceInfoAsterisk.Text = "*"
        '
        'triggerSourceInfo
        '
        Me.triggerSourceInfo.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.triggerSourceInfo.Location = New System.Drawing.Point(24, 168)
        Me.triggerSourceInfo.Name = "triggerSourceInfo"
        Me.triggerSourceInfo.Size = New System.Drawing.Size(192, 64)
        Me.triggerSourceInfo.TabIndex = 6
        Me.triggerSourceInfo.Text = "APFI0 is the default Analog Trigger pin for M Series devices.  Please refer to yo" & _
            "ur device documentation for information regarding valid Analog Triggers for your" & _
            " device."
        '
        'triggerLevelNumeric
        '
        Me.triggerLevelNumeric.DecimalPlaces = 2
        Me.triggerLevelNumeric.Location = New System.Drawing.Point(120, 56)
        Me.triggerLevelNumeric.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.triggerLevelNumeric.Minimum = New Decimal(New Integer() {10, 0, 0, -2147483648})
        Me.triggerLevelNumeric.Name = "triggerLevelNumeric"
        Me.triggerLevelNumeric.Size = New System.Drawing.Size(96, 20)
        Me.triggerLevelNumeric.TabIndex = 3
        '
        'triggerSourceTextBox
        '
        Me.triggerSourceTextBox.Location = New System.Drawing.Point(120, 24)
        Me.triggerSourceTextBox.Name = "triggerSourceTextBox"
        Me.triggerSourceTextBox.Size = New System.Drawing.Size(96, 20)
        Me.triggerSourceTextBox.TabIndex = 1
        Me.triggerSourceTextBox.Text = "APFI0"
        '
        'slopeGroupBox
        '
        Me.slopeGroupBox.Controls.Add(Me.slopeRisingButton)
        Me.slopeGroupBox.Controls.Add(Me.slopeFallingButton)
        Me.slopeGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.slopeGroupBox.Location = New System.Drawing.Point(16, 112)
        Me.slopeGroupBox.Name = "slopeGroupBox"
        Me.slopeGroupBox.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.slopeGroupBox.Size = New System.Drawing.Size(200, 48)
        Me.slopeGroupBox.TabIndex = 4
        Me.slopeGroupBox.TabStop = False
        Me.slopeGroupBox.Text = "Slope:"
        '
        'slopeRisingButton
        '
        Me.slopeRisingButton.Checked = True
        Me.slopeRisingButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.slopeRisingButton.Location = New System.Drawing.Point(8, 16)
        Me.slopeRisingButton.Name = "slopeRisingButton"
        Me.slopeRisingButton.Size = New System.Drawing.Size(56, 24)
        Me.slopeRisingButton.TabIndex = 0
        Me.slopeRisingButton.TabStop = True
        Me.slopeRisingButton.Text = "Rising"
        '
        'slopeFallingButton
        '
        Me.slopeFallingButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.slopeFallingButton.Location = New System.Drawing.Point(72, 16)
        Me.slopeFallingButton.Name = "slopeFallingButton"
        Me.slopeFallingButton.Size = New System.Drawing.Size(56, 24)
        Me.slopeFallingButton.TabIndex = 1
        Me.slopeFallingButton.Text = "Falling"
        '
        'triggerSourceLabel
        '
        Me.triggerSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.triggerSourceLabel.Location = New System.Drawing.Point(16, 24)
        Me.triggerSourceLabel.Name = "triggerSourceLabel"
        Me.triggerSourceLabel.Size = New System.Drawing.Size(88, 16)
        Me.triggerSourceLabel.TabIndex = 0
        Me.triggerSourceLabel.Text = "Trigger Source:*"
        '
        'triggerLevelLabel
        '
        Me.triggerLevelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.triggerLevelLabel.Location = New System.Drawing.Point(16, 56)
        Me.triggerLevelLabel.Name = "triggerLevelLabel"
        Me.triggerLevelLabel.Size = New System.Drawing.Size(88, 16)
        Me.triggerLevelLabel.TabIndex = 2
        Me.triggerLevelLabel.Text = "Trigger Level (V):"
        '
        'hysteresisNumeric
        '
        Me.hysteresisNumeric.DecimalPlaces = 2
        Me.hysteresisNumeric.Location = New System.Drawing.Point(120, 88)
        Me.hysteresisNumeric.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.hysteresisNumeric.Minimum = New Decimal(New Integer() {10, 0, 0, -2147483648})
        Me.hysteresisNumeric.Name = "hysteresisNumeric"
        Me.hysteresisNumeric.Size = New System.Drawing.Size(96, 20)
        Me.hysteresisNumeric.TabIndex = 3
        '
        'hysteresisLabel
        '
        Me.hysteresisLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.hysteresisLabel.Location = New System.Drawing.Point(16, 88)
        Me.hysteresisLabel.Name = "hysteresisLabel"
        Me.hysteresisLabel.Size = New System.Drawing.Size(88, 16)
        Me.hysteresisLabel.TabIndex = 2
        Me.hysteresisLabel.Text = "Hysteresis (V):"
        '
        'stopButton
        '
        Me.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.stopButton.Location = New System.Drawing.Point(304, 400)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(112, 24)
        Me.stopButton.TabIndex = 1
        Me.stopButton.Text = "Stop"
        '
        'startButton
        '
        Me.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.startButton.Location = New System.Drawing.Point(304, 376)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(112, 24)
        Me.startButton.TabIndex = 0
        Me.startButton.Text = "Start"
        '
        'timingParametersGroupBox
        '
        Me.timingParametersGroupBox.Controls.Add(Me.rateLabel)
        Me.timingParametersGroupBox.Controls.Add(Me.samplesLabel)
        Me.timingParametersGroupBox.Controls.Add(Me.samplesPerChannelNumeric)
        Me.timingParametersGroupBox.Controls.Add(Me.rateNumeric)
        Me.timingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.timingParametersGroupBox.Location = New System.Drawing.Point(8, 136)
        Me.timingParametersGroupBox.Name = "timingParametersGroupBox"
        Me.timingParametersGroupBox.Size = New System.Drawing.Size(232, 88)
        Me.timingParametersGroupBox.TabIndex = 3
        Me.timingParametersGroupBox.TabStop = False
        Me.timingParametersGroupBox.Text = "Timing Parameters"
        '
        'rateLabel
        '
        Me.rateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.rateLabel.Location = New System.Drawing.Point(16, 56)
        Me.rateLabel.Name = "rateLabel"
        Me.rateLabel.Size = New System.Drawing.Size(64, 16)
        Me.rateLabel.TabIndex = 2
        Me.rateLabel.Text = "Rate (Hz):"
        '
        'samplesLabel
        '
        Me.samplesLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.samplesLabel.Location = New System.Drawing.Point(16, 24)
        Me.samplesLabel.Name = "samplesLabel"
        Me.samplesLabel.Size = New System.Drawing.Size(104, 16)
        Me.samplesLabel.TabIndex = 0
        Me.samplesLabel.Text = "Samples / Channel:"
        '
        'samplesPerChannelNumeric
        '
        Me.samplesPerChannelNumeric.Location = New System.Drawing.Point(120, 24)
        Me.samplesPerChannelNumeric.Maximum = New Decimal(New Integer() {32768, 0, 0, 0})
        Me.samplesPerChannelNumeric.Name = "samplesPerChannelNumeric"
        Me.samplesPerChannelNumeric.Size = New System.Drawing.Size(96, 20)
        Me.samplesPerChannelNumeric.TabIndex = 1
        Me.samplesPerChannelNumeric.Value = New Decimal(New Integer() {1000, 0, 0, 0})
        '
        'rateNumeric
        '
        Me.rateNumeric.Location = New System.Drawing.Point(120, 56)
        Me.rateNumeric.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.rateNumeric.Name = "rateNumeric"
        Me.rateNumeric.Size = New System.Drawing.Size(96, 20)
        Me.rateNumeric.TabIndex = 3
        Me.rateNumeric.Value = New Decimal(New Integer() {1000, 0, 0, 0})
        '
        'channelParametersGroupBox
        '
        Me.channelParametersGroupBox.Controls.Add(Me.physicalChannelComboBox)
        Me.channelParametersGroupBox.Controls.Add(Me.minimumValueNumeric)
        Me.channelParametersGroupBox.Controls.Add(Me.maximumValueNumeric)
        Me.channelParametersGroupBox.Controls.Add(Me.maximumLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.minimumLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.physicalChannelLabel)
        Me.channelParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.channelParametersGroupBox.Location = New System.Drawing.Point(8, 8)
        Me.channelParametersGroupBox.Name = "channelParametersGroupBox"
        Me.channelParametersGroupBox.Size = New System.Drawing.Size(232, 120)
        Me.channelParametersGroupBox.TabIndex = 2
        Me.channelParametersGroupBox.TabStop = False
        Me.channelParametersGroupBox.Text = "Channel Parameters"
        '
        'physicalChannelComboBox
        '
        Me.physicalChannelComboBox.Location = New System.Drawing.Point(120, 24)
        Me.physicalChannelComboBox.Name = "physicalChannelComboBox"
        Me.physicalChannelComboBox.Size = New System.Drawing.Size(96, 21)
        Me.physicalChannelComboBox.TabIndex = 1
        Me.physicalChannelComboBox.Text = "Dev1/ai0"
        '
        'minimumValueNumeric
        '
        Me.minimumValueNumeric.DecimalPlaces = 2
        Me.minimumValueNumeric.Location = New System.Drawing.Point(120, 56)
        Me.minimumValueNumeric.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.minimumValueNumeric.Minimum = New Decimal(New Integer() {10, 0, 0, -2147483648})
        Me.minimumValueNumeric.Name = "minimumValueNumeric"
        Me.minimumValueNumeric.Size = New System.Drawing.Size(96, 20)
        Me.minimumValueNumeric.TabIndex = 3
        Me.minimumValueNumeric.Value = New Decimal(New Integer() {100, 0, 0, -2147418112})
        '
        'maximumValueNumeric
        '
        Me.maximumValueNumeric.DecimalPlaces = 2
        Me.maximumValueNumeric.Location = New System.Drawing.Point(120, 88)
        Me.maximumValueNumeric.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.maximumValueNumeric.Minimum = New Decimal(New Integer() {10, 0, 0, -2147483648})
        Me.maximumValueNumeric.Name = "maximumValueNumeric"
        Me.maximumValueNumeric.Size = New System.Drawing.Size(96, 20)
        Me.maximumValueNumeric.TabIndex = 5
        Me.maximumValueNumeric.Value = New Decimal(New Integer() {100, 0, 0, 65536})
        '
        'maximumLabel
        '
        Me.maximumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.maximumLabel.Location = New System.Drawing.Point(16, 90)
        Me.maximumLabel.Name = "maximumLabel"
        Me.maximumLabel.Size = New System.Drawing.Size(96, 16)
        Me.maximumLabel.TabIndex = 4
        Me.maximumLabel.Text = "Maximum (Volts):"
        '
        'minimumLabel
        '
        Me.minimumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.minimumLabel.Location = New System.Drawing.Point(16, 58)
        Me.minimumLabel.Name = "minimumLabel"
        Me.minimumLabel.Size = New System.Drawing.Size(96, 16)
        Me.minimumLabel.TabIndex = 2
        Me.minimumLabel.Text = "Minimum (Volts):"
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
        'acquisitionResultGroupBox
        '
        Me.acquisitionResultGroupBox.Controls.Add(Me.resultLabel)
        Me.acquisitionResultGroupBox.Controls.Add(Me.acquisitionDataGrid)
        Me.acquisitionResultGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.acquisitionResultGroupBox.Location = New System.Drawing.Point(248, 8)
        Me.acquisitionResultGroupBox.Name = "acquisitionResultGroupBox"
        Me.acquisitionResultGroupBox.Size = New System.Drawing.Size(224, 328)
        Me.acquisitionResultGroupBox.TabIndex = 5
        Me.acquisitionResultGroupBox.TabStop = False
        Me.acquisitionResultGroupBox.Text = "Acquisition Results"
        '
        'resultLabel
        '
        Me.resultLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.resultLabel.Location = New System.Drawing.Point(8, 16)
        Me.resultLabel.Name = "resultLabel"
        Me.resultLabel.Size = New System.Drawing.Size(128, 16)
        Me.resultLabel.TabIndex = 0
        Me.resultLabel.Text = "Acquisition Data (V):"
        '
        'acquisitionDataGrid
        '
        Me.acquisitionDataGrid.AllowSorting = False
        Me.acquisitionDataGrid.DataMember = ""
        Me.acquisitionDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.acquisitionDataGrid.Location = New System.Drawing.Point(8, 32)
        Me.acquisitionDataGrid.Name = "acquisitionDataGrid"
        Me.acquisitionDataGrid.ParentRowsVisible = False
        Me.acquisitionDataGrid.ReadOnly = True
        Me.acquisitionDataGrid.Size = New System.Drawing.Size(208, 288)
        Me.acquisitionDataGrid.TabIndex = 1
        Me.acquisitionDataGrid.TabStop = False
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(482, 488)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.acquisitionResultGroupBox)
        Me.Controls.Add(Me.triggerParametersGroupBox)
        Me.Controls.Add(Me.stopButton)
        Me.Controls.Add(Me.timingParametersGroupBox)
        Me.Controls.Add(Me.channelParametersGroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Continuous Voltage Acquisition - Internal Clock & Analog Start"
        Me.triggerParametersGroupBox.ResumeLayout(False)
        Me.triggerParametersGroupBox.PerformLayout()
        CType(Me.triggerLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.slopeGroupBox.ResumeLayout(False)
        CType(Me.hysteresisNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.timingParametersGroupBox.ResumeLayout(False)
        CType(Me.samplesPerChannelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rateNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.channelParametersGroupBox.ResumeLayout(False)
        CType(Me.minimumValueNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.maximumValueNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.acquisitionResultGroupBox.ResumeLayout(False)
        CType(Me.acquisitionDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private myTask As Task              ' A new Task is created when the start button is pressed
    Private runningTask As Task  ' Flags whether or not an acquisition is in progress
    Private data As AnalogWaveform(Of Double)() ' The variable where the Data for an acquisition is written

    Private analogSlope As AnalogEdgeStartTriggerSlope ' The start trigger value changes when the Radio button is selected
    Private analogInReader As AnalogMultiChannelReader
    Private numberOfSamples As Int32
    Private myAsyncCallback As AsyncCallback

    Private dataColumn As DataColumn()             ' Channels of Data
    Private dataTable As DataTable = New DataTable ' Table to Display Data

    Private Sub startButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles startButton.Click
        numberOfSamples = Convert.ToInt32(samplesPerChannelNumeric.Value)
        If runningTask Is Nothing Then
            Try
                ' Create a new task
                myTask = New Task()

                ' Create a new virtual channel
                myTask.AIChannels.CreateVoltageChannel(physicalChannelComboBox.Text, "", _
                CType(-1, AITerminalConfiguration), Convert.ToDouble(minimumValueNumeric.Value), _
                Convert.ToDouble(maximumValueNumeric.Value), AIVoltageUnits.Volts)

                ' Configure Timing Specs 
                myTask.Timing.ConfigureSampleClock("", Convert.ToDouble(rateNumeric.Value), _
                SampleClockActiveEdge.Rising, SampleQuantityMode.ContinuousSamples, 1000)

                ' Configure the Analog Trigger
                myTask.Triggers.StartTrigger.ConfigureAnalogEdgeTrigger(triggerSourceTextBox.Text, _
                analogSlope, Convert.ToDouble(triggerLevelNumeric.Value))

                myTask.Triggers.StartTrigger.AnalogEdge.Hysteresis = Convert.ToDouble(hysteresisNumeric.Value)

                ' Verify the Task
                myTask.Control(TaskAction.Verify)

                ' Prepare the table for Data
                InitializeDataTable(myTask.AIChannels, dataTable)
                acquisitionDataGrid.DataSource = dataTable

                runningTask = myTask
                analogInReader = New AnalogMultiChannelReader(myTask.Stream)

                ' Use SynchronizeCallbacks to specify that the object 
                ' marshals callbacks across threads appropriately.
                analogInReader.SynchronizeCallbacks = True
                analogInReader.BeginReadWaveform(numberOfSamples, myAsyncCallback, myTask)

                startButton.Enabled = False
                stopButton.Enabled = True

            Catch exception As DaqException
                MessageBox.Show(exception.Message)
                myTask.Dispose()
            End Try
        End If

    End Sub

    Private Sub AnalogInCallback(ByVal ar As IAsyncResult)
        Try
            If (Not (runningTask Is Nothing)) AndAlso runningTask Is ar.AsyncState Then
                data = analogInReader.EndReadWaveform(ar)

                dataToDataTable(data, dataTable)

                analogInReader.BeginMemoryOptimizedReadWaveform(numberOfSamples, myAsyncCallback, myTask, data)
            End If
        Catch exception As DaqException
            MessageBox.Show(exception.Message)
            stopButton_Click(Nothing, Nothing)
        End Try
    End Sub


    Private Sub stopButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles stopButton.Click
        startButton.Enabled = True
        stopButton.Enabled = False
        runningTask = Nothing
        myTask.Dispose()
    End Sub

    Private Sub slopeRisingButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles slopeRisingButton.CheckedChanged
        If slopeRisingButton.Checked Then analogSlope = AnalogEdgeStartTriggerSlope.Rising
    End Sub

    Private Sub slopeFallingButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles slopeFallingButton.CheckedChanged
        If slopeFallingButton.Checked Then analogSlope = AnalogEdgeStartTriggerSlope.Falling
    End Sub

    Private Sub dataToDataTable(ByVal sourceArray As AnalogWaveform(Of Double)(), ByRef dataTable As DataTable)
        ' Iterate over channels
        Dim currentLineIndex As Integer = 0
        For Each waveform As AnalogWaveform(Of Double) In sourceArray
            Dim dataCount As Integer = 0
            If waveform.Samples.Count < 10 Then
                dataCount = waveform.Samples.Count
            Else
                dataCount = 10
            End If
            For sample As Integer = 0 To (dataCount - 1)
                dataTable.Rows(sample)(currentLineIndex) = waveform.Samples(sample).Value
            Next
            currentLineIndex += 1
        Next
    End Sub

    Private Sub InitializeDataTable(ByVal channelCollection As AIChannelCollection, ByRef data As DataTable)
        Dim numOfChannels As Int16 = channelCollection.Count
        data.Rows.Clear()
        data.Columns.Clear()
        dataColumn = New DataColumn(numOfChannels) {}
        Dim numOfRows As Int16 = 10
        Dim currentChannelIndex As Int16 = 0
        Dim currentDataIndex As Int16 = 0

        For currentChannelIndex = 0 To (numOfChannels - 1)
            dataColumn(currentChannelIndex) = New DataColumn
            dataColumn(currentChannelIndex).DataType = System.Type.GetType("System.Double")
            dataColumn(currentChannelIndex).ColumnName = channelCollection(currentChannelIndex).PhysicalName
        Next

        data.Columns.AddRange(dataColumn)

        For currentDataIndex = 0 To (numOfRows - 1)
            Dim rowArr As Object() = New Object(numOfChannels - 1) {}
            data.Rows.Add(rowArr)
        Next
    End Sub
End Class
