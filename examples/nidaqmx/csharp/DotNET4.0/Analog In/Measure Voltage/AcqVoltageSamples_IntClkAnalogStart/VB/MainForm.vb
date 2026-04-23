'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   AcqVoltageSamples_IntClkAnalogStart
'
' Category:
'   AI
'
' Description:
'   This example demonstrates how to acquire a finite amount of data using the
'   DAQ device's internal clock, started by an analog edge condition.
'
' Instructions for running:
'   1.  Select the physical channel corresponding to where your signal is input
'       on the DAQ device.
'   2.  Enter the minimum and maximum voltage values.Note: For better accuracy,
'       try to match the input range to the expected voltage level of the
'       measured signal.
'   3.  Set the number of samples to acquire per channel.
'   4.  Set the rate of the acquisition.Note: The rate should be at least twice
'       as fast as the maximum frequency component of the signal being acquired.
'   5.  Set the source of the start trigger. By default this is analog input
'       channel 0.
'   6.  Set the slope and level of the desired analog edge condition.
'
' Steps:
'   1.  Create a new analog input task.
'   2.  Create an analog input voltage channel and define the mode to be finite.
'   3.  Define the parameters for the analog start trigger.
'   4.  Call AnalogMultiChannelReader.BeginReadWaveform to install a callback
'       and begin the asynchronous read operation.
'   5.  Inside the callback, call AnalogMultiChannelReader.EndReadWaveform to
'       retrieve the data from the read operation.  
'   6.  Dispose the Task object to clean-up any resources associated with the
'       task.
'   7.  Handle any DaqExceptions, if they occur.
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
'   Make sure your signal input terminal matches the Physical Channel I/O
'   Control.  In this case wire your signal to the ai0 pin on your DAQ Device. 
'   By default, the signal routed to APFI0 will be used as the analog start
'   trigger.  APFI0 is the default Analog Trigger pin for M Series devices.  For
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


    Private myTask As Task
    Private reader As AnalogMultiChannelReader
    Private referenceEdge As AnalogEdgeStartTriggerSlope = AnalogEdgeStartTriggerSlope.Rising

    Private dataColumn As DataColumn()
    Private dataTable As dataTable = New dataTable

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()
        Application.EnableVisualStyles()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
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
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents startButton As System.Windows.Forms.Button
    Friend WithEvents triggerParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents triggerLevelLabel As System.Windows.Forms.Label
    Friend WithEvents rateLabel As System.Windows.Forms.Label
    Friend WithEvents channelSamplesLabel As System.Windows.Forms.Label
    Friend WithEvents channelParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents maximumLabel As System.Windows.Forms.Label
    Friend WithEvents minimumLabel As System.Windows.Forms.Label
    Friend WithEvents physicalChannelLabel As System.Windows.Forms.Label
    Friend WithEvents resultLabel As System.Windows.Forms.Label
    Friend WithEvents samplesPerChannelNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents rateNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents referenceTriggerSourceTextBox As System.Windows.Forms.TextBox
    Friend WithEvents referenceTriggerSourceLabel As System.Windows.Forms.Label
    Friend WithEvents referenceEdgeGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents referenceEdgeRisingButton As System.Windows.Forms.RadioButton
    Friend WithEvents referenceEdgeFallingButton As System.Windows.Forms.RadioButton
    Friend WithEvents timingParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents acquisitionResultGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents acquisitionDataGrid As System.Windows.Forms.DataGrid
    Friend WithEvents minimumValueNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents maximumValueNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents triggerSourceInfoAsterisk As System.Windows.Forms.Label
    Friend WithEvents triggerSourceInfo As System.Windows.Forms.Label
    Friend WithEvents physicalChannelComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents hysteresisLabel As System.Windows.Forms.Label
    Friend WithEvents hysteresisNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents triggerLevelNumeric As System.Windows.Forms.NumericUpDown
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MainForm))
        Me.startButton = New System.Windows.Forms.Button
        Me.triggerParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.triggerSourceInfoAsterisk = New System.Windows.Forms.Label
        Me.triggerSourceInfo = New System.Windows.Forms.Label
        Me.referenceTriggerSourceTextBox = New System.Windows.Forms.TextBox
        Me.referenceTriggerSourceLabel = New System.Windows.Forms.Label
        Me.triggerLevelLabel = New System.Windows.Forms.Label
        Me.referenceEdgeGroupBox = New System.Windows.Forms.GroupBox
        Me.referenceEdgeRisingButton = New System.Windows.Forms.RadioButton
        Me.referenceEdgeFallingButton = New System.Windows.Forms.RadioButton
        Me.timingParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.samplesPerChannelNumeric = New System.Windows.Forms.NumericUpDown
        Me.rateLabel = New System.Windows.Forms.Label
        Me.channelSamplesLabel = New System.Windows.Forms.Label
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
        Me.triggerLevelNumeric = New System.Windows.Forms.NumericUpDown
        Me.hysteresisLabel = New System.Windows.Forms.Label
        Me.hysteresisNumeric = New System.Windows.Forms.NumericUpDown
        Me.triggerParametersGroupBox.SuspendLayout()
        Me.referenceEdgeGroupBox.SuspendLayout()
        Me.timingParametersGroupBox.SuspendLayout()
        CType(Me.samplesPerChannelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rateNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.channelParametersGroupBox.SuspendLayout()
        CType(Me.minimumValueNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.maximumValueNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.acquisitionResultGroupBox.SuspendLayout()
        CType(Me.acquisitionDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.triggerLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.hysteresisNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'startButton
        '
        Me.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.startButton.Location = New System.Drawing.Point(344, 392)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(80, 32)
        Me.startButton.TabIndex = 0
        Me.startButton.Text = "Start"
        '
        'triggerParametersGroupBox
        '
        Me.triggerParametersGroupBox.Controls.Add(Me.triggerSourceInfoAsterisk)
        Me.triggerParametersGroupBox.Controls.Add(Me.triggerSourceInfo)
        Me.triggerParametersGroupBox.Controls.Add(Me.referenceTriggerSourceTextBox)
        Me.triggerParametersGroupBox.Controls.Add(Me.referenceTriggerSourceLabel)
        Me.triggerParametersGroupBox.Controls.Add(Me.triggerLevelLabel)
        Me.triggerParametersGroupBox.Controls.Add(Me.referenceEdgeGroupBox)
        Me.triggerParametersGroupBox.Controls.Add(Me.triggerLevelNumeric)
        Me.triggerParametersGroupBox.Controls.Add(Me.hysteresisLabel)
        Me.triggerParametersGroupBox.Controls.Add(Me.hysteresisNumeric)
        Me.triggerParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.triggerParametersGroupBox.Location = New System.Drawing.Point(264, 8)
        Me.triggerParametersGroupBox.Name = "triggerParametersGroupBox"
        Me.triggerParametersGroupBox.Size = New System.Drawing.Size(232, 264)
        Me.triggerParametersGroupBox.TabIndex = 3
        Me.triggerParametersGroupBox.TabStop = False
        Me.triggerParametersGroupBox.Text = "Trigger Parameters"
        '
        'triggerSourceInfoAsterisk
        '
        Me.triggerSourceInfoAsterisk.Location = New System.Drawing.Point(16, 184)
        Me.triggerSourceInfoAsterisk.Name = "triggerSourceInfoAsterisk"
        Me.triggerSourceInfoAsterisk.Size = New System.Drawing.Size(8, 23)
        Me.triggerSourceInfoAsterisk.TabIndex = 5
        Me.triggerSourceInfoAsterisk.Text = "*"
        '
        'triggerSourceInfo
        '
        Me.triggerSourceInfo.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.triggerSourceInfo.Location = New System.Drawing.Point(24, 184)
        Me.triggerSourceInfo.Name = "triggerSourceInfo"
        Me.triggerSourceInfo.Size = New System.Drawing.Size(192, 64)
        Me.triggerSourceInfo.TabIndex = 6
        Me.triggerSourceInfo.Text = "APFI0 is the default Analog Trigger pin for M Series devices.  Please refer to you" & _
        "r device documentation for information regarding valid Analog Triggers for your " & _
        "device."
        '
        'referenceTriggerSourceTextBox
        '
        Me.referenceTriggerSourceTextBox.Location = New System.Drawing.Point(128, 24)
        Me.referenceTriggerSourceTextBox.Name = "referenceTriggerSourceTextBox"
        Me.referenceTriggerSourceTextBox.Size = New System.Drawing.Size(96, 20)
        Me.referenceTriggerSourceTextBox.TabIndex = 1
        Me.referenceTriggerSourceTextBox.Text = "APFI0"
        '
        'referenceTriggerSourceLabel
        '
        Me.referenceTriggerSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.referenceTriggerSourceLabel.Location = New System.Drawing.Point(16, 26)
        Me.referenceTriggerSourceLabel.Name = "referenceTriggerSourceLabel"
        Me.referenceTriggerSourceLabel.Size = New System.Drawing.Size(88, 16)
        Me.referenceTriggerSourceLabel.TabIndex = 0
        Me.referenceTriggerSourceLabel.Text = "Trigger Source:*"
        '
        'triggerLevelLabel
        '
        Me.triggerLevelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.triggerLevelLabel.Location = New System.Drawing.Point(16, 58)
        Me.triggerLevelLabel.Name = "triggerLevelLabel"
        Me.triggerLevelLabel.Size = New System.Drawing.Size(96, 16)
        Me.triggerLevelLabel.TabIndex = 2
        Me.triggerLevelLabel.Text = "Trigger Level (V):"
        '
        'referenceEdgeGroupBox
        '
        Me.referenceEdgeGroupBox.Controls.Add(Me.referenceEdgeRisingButton)
        Me.referenceEdgeGroupBox.Controls.Add(Me.referenceEdgeFallingButton)
        Me.referenceEdgeGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.referenceEdgeGroupBox.Location = New System.Drawing.Point(16, 112)
        Me.referenceEdgeGroupBox.Name = "referenceEdgeGroupBox"
        Me.referenceEdgeGroupBox.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.referenceEdgeGroupBox.Size = New System.Drawing.Size(208, 56)
        Me.referenceEdgeGroupBox.TabIndex = 4
        Me.referenceEdgeGroupBox.TabStop = False
        Me.referenceEdgeGroupBox.Text = "Reference Edge:"
        '
        'referenceEdgeRisingButton
        '
        Me.referenceEdgeRisingButton.Checked = True
        Me.referenceEdgeRisingButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.referenceEdgeRisingButton.Location = New System.Drawing.Point(24, 24)
        Me.referenceEdgeRisingButton.Name = "referenceEdgeRisingButton"
        Me.referenceEdgeRisingButton.Size = New System.Drawing.Size(56, 24)
        Me.referenceEdgeRisingButton.TabIndex = 0
        Me.referenceEdgeRisingButton.TabStop = True
        Me.referenceEdgeRisingButton.Text = "Rising"
        '
        'referenceEdgeFallingButton
        '
        Me.referenceEdgeFallingButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.referenceEdgeFallingButton.Location = New System.Drawing.Point(120, 24)
        Me.referenceEdgeFallingButton.Name = "referenceEdgeFallingButton"
        Me.referenceEdgeFallingButton.Size = New System.Drawing.Size(56, 24)
        Me.referenceEdgeFallingButton.TabIndex = 1
        Me.referenceEdgeFallingButton.Text = "Falling"
        '
        'timingParametersGroupBox
        '
        Me.timingParametersGroupBox.Controls.Add(Me.samplesPerChannelNumeric)
        Me.timingParametersGroupBox.Controls.Add(Me.rateLabel)
        Me.timingParametersGroupBox.Controls.Add(Me.channelSamplesLabel)
        Me.timingParametersGroupBox.Controls.Add(Me.rateNumeric)
        Me.timingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.timingParametersGroupBox.Location = New System.Drawing.Point(264, 280)
        Me.timingParametersGroupBox.Name = "timingParametersGroupBox"
        Me.timingParametersGroupBox.Size = New System.Drawing.Size(232, 96)
        Me.timingParametersGroupBox.TabIndex = 4
        Me.timingParametersGroupBox.TabStop = False
        Me.timingParametersGroupBox.Text = "Timing Parameters"
        '
        'samplesPerChannelNumeric
        '
        Me.samplesPerChannelNumeric.Location = New System.Drawing.Point(128, 24)
        Me.samplesPerChannelNumeric.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.samplesPerChannelNumeric.Name = "samplesPerChannelNumeric"
        Me.samplesPerChannelNumeric.Size = New System.Drawing.Size(96, 20)
        Me.samplesPerChannelNumeric.TabIndex = 1
        Me.samplesPerChannelNumeric.Value = New Decimal(New Integer() {1000, 0, 0, 0})
        '
        'rateLabel
        '
        Me.rateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.rateLabel.Location = New System.Drawing.Point(16, 58)
        Me.rateLabel.Name = "rateLabel"
        Me.rateLabel.Size = New System.Drawing.Size(64, 16)
        Me.rateLabel.TabIndex = 2
        Me.rateLabel.Text = "Rate (Hz):"
        '
        'channelSamplesLabel
        '
        Me.channelSamplesLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.channelSamplesLabel.Location = New System.Drawing.Point(16, 26)
        Me.channelSamplesLabel.Name = "channelSamplesLabel"
        Me.channelSamplesLabel.Size = New System.Drawing.Size(120, 16)
        Me.channelSamplesLabel.TabIndex = 0
        Me.channelSamplesLabel.Text = "Samples per Channel:"
        '
        'rateNumeric
        '
        Me.rateNumeric.DecimalPlaces = 2
        Me.rateNumeric.Location = New System.Drawing.Point(128, 56)
        Me.rateNumeric.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
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
        Me.channelParametersGroupBox.Size = New System.Drawing.Size(248, 120)
        Me.channelParametersGroupBox.TabIndex = 1
        Me.channelParametersGroupBox.TabStop = False
        Me.channelParametersGroupBox.Text = "Channel Parameters"
        '
        'physicalChannelComboBox
        '
        Me.physicalChannelComboBox.Location = New System.Drawing.Point(144, 24)
        Me.physicalChannelComboBox.Name = "physicalChannelComboBox"
        Me.physicalChannelComboBox.Size = New System.Drawing.Size(96, 21)
        Me.physicalChannelComboBox.TabIndex = 1
        Me.physicalChannelComboBox.Text = "Dev1/ai0"
        '
        'minimumValueNumeric
        '
        Me.minimumValueNumeric.DecimalPlaces = 2
        Me.minimumValueNumeric.Location = New System.Drawing.Point(144, 56)
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
        Me.maximumValueNumeric.Location = New System.Drawing.Point(144, 88)
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
        Me.maximumLabel.Size = New System.Drawing.Size(112, 16)
        Me.maximumLabel.TabIndex = 4
        Me.maximumLabel.Text = "Maximum Value (V):"
        '
        'minimumLabel
        '
        Me.minimumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.minimumLabel.Location = New System.Drawing.Point(16, 58)
        Me.minimumLabel.Name = "minimumLabel"
        Me.minimumLabel.Size = New System.Drawing.Size(104, 16)
        Me.minimumLabel.TabIndex = 2
        Me.minimumLabel.Text = "Minimum Value (V):"
        '
        'physicalChannelLabel
        '
        Me.physicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.physicalChannelLabel.Location = New System.Drawing.Point(16, 26)
        Me.physicalChannelLabel.Name = "physicalChannelLabel"
        Me.physicalChannelLabel.Size = New System.Drawing.Size(104, 16)
        Me.physicalChannelLabel.TabIndex = 0
        Me.physicalChannelLabel.Text = "Physical Channel:"
        '
        'acquisitionResultGroupBox
        '
        Me.acquisitionResultGroupBox.Controls.Add(Me.resultLabel)
        Me.acquisitionResultGroupBox.Controls.Add(Me.acquisitionDataGrid)
        Me.acquisitionResultGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.acquisitionResultGroupBox.Location = New System.Drawing.Point(8, 136)
        Me.acquisitionResultGroupBox.Name = "acquisitionResultGroupBox"
        Me.acquisitionResultGroupBox.Size = New System.Drawing.Size(248, 272)
        Me.acquisitionResultGroupBox.TabIndex = 2
        Me.acquisitionResultGroupBox.TabStop = False
        Me.acquisitionResultGroupBox.Text = "Acquisition Results"
        '
        'resultLabel
        '
        Me.resultLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.resultLabel.Location = New System.Drawing.Point(16, 16)
        Me.resultLabel.Name = "resultLabel"
        Me.resultLabel.Size = New System.Drawing.Size(112, 16)
        Me.resultLabel.TabIndex = 0
        Me.resultLabel.Text = "Acquisition Data (V):"
        '
        'acquisitionDataGrid
        '
        Me.acquisitionDataGrid.AllowSorting = False
        Me.acquisitionDataGrid.DataMember = ""
        Me.acquisitionDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.acquisitionDataGrid.Location = New System.Drawing.Point(16, 32)
        Me.acquisitionDataGrid.Name = "acquisitionDataGrid"
        Me.acquisitionDataGrid.ParentRowsVisible = False
        Me.acquisitionDataGrid.ReadOnly = True
        Me.acquisitionDataGrid.Size = New System.Drawing.Size(224, 232)
        Me.acquisitionDataGrid.TabIndex = 1
        Me.acquisitionDataGrid.TabStop = False
        '
        'triggerLevelNumeric
        '
        Me.triggerLevelNumeric.DecimalPlaces = 2
        Me.triggerLevelNumeric.Location = New System.Drawing.Point(128, 56)
        Me.triggerLevelNumeric.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.triggerLevelNumeric.Minimum = New Decimal(New Integer() {10, 0, 0, -2147483648})
        Me.triggerLevelNumeric.Name = "triggerLevelNumeric"
        Me.triggerLevelNumeric.Size = New System.Drawing.Size(96, 20)
        Me.triggerLevelNumeric.TabIndex = 3
        '
        'hysteresisLabel
        '
        Me.hysteresisLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.hysteresisLabel.Location = New System.Drawing.Point(16, 88)
        Me.hysteresisLabel.Name = "hysteresisLabel"
        Me.hysteresisLabel.Size = New System.Drawing.Size(96, 16)
        Me.hysteresisLabel.TabIndex = 2
        Me.hysteresisLabel.Text = "Hysteresis (V):"
        '
        'hysteresisNumeric
        '
        Me.hysteresisNumeric.DecimalPlaces = 2
        Me.hysteresisNumeric.Location = New System.Drawing.Point(128, 88)
        Me.hysteresisNumeric.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.hysteresisNumeric.Minimum = New Decimal(New Integer() {10, 0, 0, -2147483648})
        Me.hysteresisNumeric.Name = "hysteresisNumeric"
        Me.hysteresisNumeric.Size = New System.Drawing.Size(96, 20)
        Me.hysteresisNumeric.TabIndex = 3
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(506, 440)
        Me.Controls.Add(Me.acquisitionResultGroupBox)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.triggerParametersGroupBox)
        Me.Controls.Add(Me.timingParametersGroupBox)
        Me.Controls.Add(Me.channelParametersGroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Acquire Voltage Samples - Internal Clock & Analog Start"
        Me.triggerParametersGroupBox.ResumeLayout(False)
        Me.referenceEdgeGroupBox.ResumeLayout(False)
        Me.timingParametersGroupBox.ResumeLayout(False)
        CType(Me.samplesPerChannelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rateNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.channelParametersGroupBox.ResumeLayout(False)
        CType(Me.minimumValueNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.maximumValueNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.acquisitionResultGroupBox.ResumeLayout(False)
        CType(Me.acquisitionDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.triggerLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.hysteresisNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub startButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles startButton.Click
        startButton.Enabled = False
        Try
            ' Create a new task
            myTask = New Task()

            ' Create a virtual channel
            myTask.AIChannels.CreateVoltageChannel(physicalChannelComboBox.Text, "", _
                CType(-1, AITerminalConfiguration), Convert.ToDouble(minimumValueNumeric.Value), _
                Convert.ToDouble(maximumValueNumeric.Value), AIVoltageUnits.Volts)

            myTask.Timing.ConfigureSampleClock("", Convert.ToDouble(rateNumeric.Value), SampleClockActiveEdge.Rising, _
                SampleQuantityMode.FiniteSamples, Convert.ToInt16(samplesPerChannelNumeric.Value))

            myTask.Triggers.StartTrigger.ConfigureAnalogEdgeTrigger(referenceTriggerSourceTextBox.Text, _
                referenceEdge, Convert.ToDouble(triggerLevelNumeric.Value))

            myTask.Triggers.StartTrigger.AnalogEdge.Hysteresis = Convert.ToDouble(hysteresisNumeric.Value)

            ' Verify the Task
            myTask.Control(TaskAction.Verify)

            reader = New AnalogMultiChannelReader(myTask.Stream)

            ' Use SynchronizeCallbacks to specify that the object 
            ' marshals callbacks across threads appropriately.
            reader.SynchronizeCallbacks = True
            reader.BeginReadWaveform(Convert.ToInt32(samplesPerChannelNumeric.Value), AddressOf myCallback, Nothing)

            ' Prepare the table for Data
            InitializeDataTable(myTask.AIChannels, dataTable)
            acquisitionDataGrid.DataSource = dataTable
        Catch ex As DaqException
            MessageBox.Show(ex.Message)
            myTask.Dispose()
            startButton.Enabled = True
        End Try
    End Sub

    Private Sub myCallback(ByVal ar As IAsyncResult)
        Try
            ' Read one point of data per channel
            Dim data() As AnalogWaveform(Of Double) = reader.EndReadWaveform(ar)

            ' Plot your data here
            dataToDataTable(data, dataTable)

        Catch ex As DaqException
            MessageBox.Show(ex.Message)
        Finally
            myTask.Dispose()
            startButton.Enabled = True
        End Try
    End Sub

    Private Sub referenceEdgeRisingButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles referenceEdgeRisingButton.CheckedChanged
        If referenceEdgeRisingButton.Checked Then referenceEdge = AnalogEdgeStartTriggerSlope.Rising
    End Sub

    Private Sub referenceEdgeFallingButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles referenceEdgeFallingButton.CheckedChanged
        If referenceEdgeFallingButton.Checked Then referenceEdge = AnalogEdgeStartTriggerSlope.Falling
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

    Public Sub InitializeDataTable(ByVal channelCollection As AIChannelCollection, ByRef data As dataTable)
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
