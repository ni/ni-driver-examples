'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   AcqVoltageSamples_IntClkDigStartAndRef
'
' Category:
'   AI
'
' Description:
'   This example demonstrates how to acquire a finite amount of data using an
'   internal clock and a digital start and reference trigger.
'
' Instructions for running:
'   1.  Select the physical channel corresponding to where your signal is input
'       on the DAQ device.
'   2.  Enter the minimum and maximum voltage values.Note: For better accuracy,
'       try to match the input range to the expected voltage level of the
'       measured signal.
'   3.  Select the number of samples per channel to acquire.
'   4.  Set the rate in Hz for the acquisition.Note: The rate should be at least
'       twice as fast as the maximum frequency component of the signal being
'       acquired.
'   5.  Set the source of the start and reference trigger as well as the
'       polarity of the start and reference edges.
'   6.  Set the number of pre-trigger samples to be acquired.
'
' Steps:
'   1.  Create a new task and an analog input voltage channel.
'   2.  Configure timing specifications.
'   3.  Configure the start and reference triggers.
'   4.  Start the task and tell the board to start acquiring data.
'   5.  Read all of the voltage samples and stop the task.
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
'   Make sure your signal input terminal matches the physical channel I/O
'   control. In the default case (differential channel ai0) wire the positive
'   lead for your signal to the ACH0 pin on your DAQ device and wire the
'   negative lead for your signal to the ACH8 pin on you DAQ device. Also, make
'   sure your digital trigger terminals match the trigger source controls.  For
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

    'Global Variables
    Private myTask As Task  'A new Task is created when the Start Button is clicked
    Private reader As AnalogMultiChannelReader

    Private dataColumn As DataColumn()             'Channels of Data
    Private dataTable As DataTable = New DataTable 'Table to Display Data

    'The Start and Reference Trigger values are changed every time the corresponding radio buttons are selected
    Private startEdge As DigitalEdgeStartTriggerEdge = DigitalEdgeStartTriggerEdge.Rising
    Private referenceEdge As DigitalEdgeReferenceTriggerEdge = DigitalEdgeReferenceTriggerEdge.Rising

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
    Friend WithEvents rateLabel As System.Windows.Forms.Label
    Friend WithEvents samplesLabel As System.Windows.Forms.Label
    Friend WithEvents channelParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents maximumLabel As System.Windows.Forms.Label
    Friend WithEvents minimumLabel As System.Windows.Forms.Label
    Friend WithEvents physicalChannelLabel As System.Windows.Forms.Label
    Friend WithEvents startEdgeGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents resultLabel As System.Windows.Forms.Label
    Friend WithEvents rateNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents triggerParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents preTriggerNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents startTriggerSourceLabel As System.Windows.Forms.Label
    Friend WithEvents startTriggerSourceTextBox As System.Windows.Forms.TextBox
    Friend WithEvents referenceTriggerSourceTextBox As System.Windows.Forms.TextBox
    Friend WithEvents referenceTriggerSourceLabel As System.Windows.Forms.Label
    Friend WithEvents preTriggerSampleLabel As System.Windows.Forms.Label
    Friend WithEvents startEdgeRisingButton As System.Windows.Forms.RadioButton
    Friend WithEvents startEdgeFallingButton As System.Windows.Forms.RadioButton
    Friend WithEvents referenceEdgeGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents referenceEdgeRisingButton As System.Windows.Forms.RadioButton
    Friend WithEvents referenceEdgeFallingButton As System.Windows.Forms.RadioButton
    Friend WithEvents timingParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents samplesPerChannelNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents acquisitionResultGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents acquisitionDataGrid As System.Windows.Forms.DataGrid
    Friend WithEvents minimumValueNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents maximumValueNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents physicalChannelComboBox As System.Windows.Forms.ComboBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MainForm))
        Me.triggerParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.preTriggerNumeric = New System.Windows.Forms.NumericUpDown
        Me.startTriggerSourceLabel = New System.Windows.Forms.Label
        Me.startTriggerSourceTextBox = New System.Windows.Forms.TextBox
        Me.referenceTriggerSourceTextBox = New System.Windows.Forms.TextBox
        Me.referenceTriggerSourceLabel = New System.Windows.Forms.Label
        Me.preTriggerSampleLabel = New System.Windows.Forms.Label
        Me.startEdgeGroupBox = New System.Windows.Forms.GroupBox
        Me.startEdgeRisingButton = New System.Windows.Forms.RadioButton
        Me.startEdgeFallingButton = New System.Windows.Forms.RadioButton
        Me.referenceEdgeGroupBox = New System.Windows.Forms.GroupBox
        Me.referenceEdgeRisingButton = New System.Windows.Forms.RadioButton
        Me.referenceEdgeFallingButton = New System.Windows.Forms.RadioButton
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
        CType(Me.preTriggerNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.startEdgeGroupBox.SuspendLayout()
        Me.referenceEdgeGroupBox.SuspendLayout()
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
        Me.triggerParametersGroupBox.Controls.Add(Me.preTriggerNumeric)
        Me.triggerParametersGroupBox.Controls.Add(Me.startTriggerSourceLabel)
        Me.triggerParametersGroupBox.Controls.Add(Me.startTriggerSourceTextBox)
        Me.triggerParametersGroupBox.Controls.Add(Me.referenceTriggerSourceTextBox)
        Me.triggerParametersGroupBox.Controls.Add(Me.referenceTriggerSourceLabel)
        Me.triggerParametersGroupBox.Controls.Add(Me.preTriggerSampleLabel)
        Me.triggerParametersGroupBox.Controls.Add(Me.startEdgeGroupBox)
        Me.triggerParametersGroupBox.Controls.Add(Me.referenceEdgeGroupBox)
        Me.triggerParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.triggerParametersGroupBox.Location = New System.Drawing.Point(244, 8)
        Me.triggerParametersGroupBox.Name = "triggerParametersGroupBox"
        Me.triggerParametersGroupBox.Size = New System.Drawing.Size(304, 192)
        Me.triggerParametersGroupBox.TabIndex = 3
        Me.triggerParametersGroupBox.TabStop = False
        Me.triggerParametersGroupBox.Text = "Trigger Parameters"
        '
        'preTriggerNumeric
        '
        Me.preTriggerNumeric.Location = New System.Drawing.Point(168, 98)
        Me.preTriggerNumeric.Maximum = New Decimal(New Integer() {32768, 0, 0, 0})
        Me.preTriggerNumeric.Name = "preTriggerNumeric"
        Me.preTriggerNumeric.Size = New System.Drawing.Size(128, 20)
        Me.preTriggerNumeric.TabIndex = 5
        Me.preTriggerNumeric.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        'startTriggerSourceLabel
        '
        Me.startTriggerSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.startTriggerSourceLabel.Location = New System.Drawing.Point(16, 28)
        Me.startTriggerSourceLabel.Name = "startTriggerSourceLabel"
        Me.startTriggerSourceLabel.Size = New System.Drawing.Size(112, 16)
        Me.startTriggerSourceLabel.TabIndex = 0
        Me.startTriggerSourceLabel.Text = "Start Trigger Source:"
        '
        'startTriggerSourceTextBox
        '
        Me.startTriggerSourceTextBox.Location = New System.Drawing.Point(168, 26)
        Me.startTriggerSourceTextBox.Name = "startTriggerSourceTextBox"
        Me.startTriggerSourceTextBox.Size = New System.Drawing.Size(128, 20)
        Me.startTriggerSourceTextBox.TabIndex = 1
        Me.startTriggerSourceTextBox.Text = "/Dev1/PFI0"
        '
        'referenceTriggerSourceTextBox
        '
        Me.referenceTriggerSourceTextBox.Location = New System.Drawing.Point(168, 62)
        Me.referenceTriggerSourceTextBox.Name = "referenceTriggerSourceTextBox"
        Me.referenceTriggerSourceTextBox.Size = New System.Drawing.Size(128, 20)
        Me.referenceTriggerSourceTextBox.TabIndex = 3
        Me.referenceTriggerSourceTextBox.Text = "/Dev1/PFI1"
        '
        'referenceTriggerSourceLabel
        '
        Me.referenceTriggerSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.referenceTriggerSourceLabel.Location = New System.Drawing.Point(16, 64)
        Me.referenceTriggerSourceLabel.Name = "referenceTriggerSourceLabel"
        Me.referenceTriggerSourceLabel.Size = New System.Drawing.Size(144, 16)
        Me.referenceTriggerSourceLabel.TabIndex = 2
        Me.referenceTriggerSourceLabel.Text = "Reference Trigger Source:"
        '
        'preTriggerSampleLabel
        '
        Me.preTriggerSampleLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.preTriggerSampleLabel.Location = New System.Drawing.Point(16, 100)
        Me.preTriggerSampleLabel.Name = "preTriggerSampleLabel"
        Me.preTriggerSampleLabel.Size = New System.Drawing.Size(112, 16)
        Me.preTriggerSampleLabel.TabIndex = 4
        Me.preTriggerSampleLabel.Text = "Pre-Trigger Samples:"
        '
        'startEdgeGroupBox
        '
        Me.startEdgeGroupBox.Controls.Add(Me.startEdgeRisingButton)
        Me.startEdgeGroupBox.Controls.Add(Me.startEdgeFallingButton)
        Me.startEdgeGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.startEdgeGroupBox.Location = New System.Drawing.Point(16, 128)
        Me.startEdgeGroupBox.Name = "startEdgeGroupBox"
        Me.startEdgeGroupBox.Size = New System.Drawing.Size(136, 48)
        Me.startEdgeGroupBox.TabIndex = 6
        Me.startEdgeGroupBox.TabStop = False
        Me.startEdgeGroupBox.Text = "Start Edge"
        '
        'startEdgeRisingButton
        '
        Me.startEdgeRisingButton.Checked = True
        Me.startEdgeRisingButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.startEdgeRisingButton.Location = New System.Drawing.Point(8, 16)
        Me.startEdgeRisingButton.Name = "startEdgeRisingButton"
        Me.startEdgeRisingButton.Size = New System.Drawing.Size(56, 24)
        Me.startEdgeRisingButton.TabIndex = 0
        Me.startEdgeRisingButton.TabStop = True
        Me.startEdgeRisingButton.Text = "Rising"
        '
        'startEdgeFallingButton
        '
        Me.startEdgeFallingButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.startEdgeFallingButton.Location = New System.Drawing.Point(72, 16)
        Me.startEdgeFallingButton.Name = "startEdgeFallingButton"
        Me.startEdgeFallingButton.Size = New System.Drawing.Size(56, 24)
        Me.startEdgeFallingButton.TabIndex = 1
        Me.startEdgeFallingButton.Text = "Falling"
        '
        'referenceEdgeGroupBox
        '
        Me.referenceEdgeGroupBox.Controls.Add(Me.referenceEdgeRisingButton)
        Me.referenceEdgeGroupBox.Controls.Add(Me.referenceEdgeFallingButton)
        Me.referenceEdgeGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.referenceEdgeGroupBox.Location = New System.Drawing.Point(168, 128)
        Me.referenceEdgeGroupBox.Name = "referenceEdgeGroupBox"
        Me.referenceEdgeGroupBox.Size = New System.Drawing.Size(128, 48)
        Me.referenceEdgeGroupBox.TabIndex = 7
        Me.referenceEdgeGroupBox.TabStop = False
        Me.referenceEdgeGroupBox.Text = "Reference Edge"
        '
        'referenceEdgeRisingButton
        '
        Me.referenceEdgeRisingButton.Checked = True
        Me.referenceEdgeRisingButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.referenceEdgeRisingButton.Location = New System.Drawing.Point(8, 16)
        Me.referenceEdgeRisingButton.Name = "referenceEdgeRisingButton"
        Me.referenceEdgeRisingButton.Size = New System.Drawing.Size(56, 24)
        Me.referenceEdgeRisingButton.TabIndex = 0
        Me.referenceEdgeRisingButton.TabStop = True
        Me.referenceEdgeRisingButton.Text = "Rising"
        '
        'referenceEdgeFallingButton
        '
        Me.referenceEdgeFallingButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.referenceEdgeFallingButton.Location = New System.Drawing.Point(64, 16)
        Me.referenceEdgeFallingButton.Name = "referenceEdgeFallingButton"
        Me.referenceEdgeFallingButton.Size = New System.Drawing.Size(56, 24)
        Me.referenceEdgeFallingButton.TabIndex = 1
        Me.referenceEdgeFallingButton.Text = "Falling"
        '
        'startButton
        '
        Me.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.startButton.Location = New System.Drawing.Point(484, 296)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(64, 24)
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
        Me.timingParametersGroupBox.Location = New System.Drawing.Point(244, 208)
        Me.timingParametersGroupBox.Name = "timingParametersGroupBox"
        Me.timingParametersGroupBox.Size = New System.Drawing.Size(232, 112)
        Me.timingParametersGroupBox.TabIndex = 4
        Me.timingParametersGroupBox.TabStop = False
        Me.timingParametersGroupBox.Text = "Timing Parameters"
        '
        'rateLabel
        '
        Me.rateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.rateLabel.Location = New System.Drawing.Point(16, 74)
        Me.rateLabel.Name = "rateLabel"
        Me.rateLabel.Size = New System.Drawing.Size(64, 16)
        Me.rateLabel.TabIndex = 2
        Me.rateLabel.Text = "Rate (Hz):"
        '
        'samplesLabel
        '
        Me.samplesLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.samplesLabel.Location = New System.Drawing.Point(16, 34)
        Me.samplesLabel.Name = "samplesLabel"
        Me.samplesLabel.Size = New System.Drawing.Size(104, 16)
        Me.samplesLabel.TabIndex = 0
        Me.samplesLabel.Text = "Samples / Channel:"
        '
        'samplesPerChannelNumeric
        '
        Me.samplesPerChannelNumeric.Location = New System.Drawing.Point(120, 32)
        Me.samplesPerChannelNumeric.Maximum = New Decimal(New Integer() {32768, 0, 0, 0})
        Me.samplesPerChannelNumeric.Name = "samplesPerChannelNumeric"
        Me.samplesPerChannelNumeric.Size = New System.Drawing.Size(96, 20)
        Me.samplesPerChannelNumeric.TabIndex = 1
        Me.samplesPerChannelNumeric.Value = New Decimal(New Integer() {1000, 0, 0, 0})
        '
        'rateNumeric
        '
        Me.rateNumeric.DecimalPlaces = 2
        Me.rateNumeric.Location = New System.Drawing.Point(120, 72)
        Me.rateNumeric.Maximum = New Decimal(New Integer() {32768, 0, 0, 0})
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
        Me.channelParametersGroupBox.Location = New System.Drawing.Point(5, 8)
        Me.channelParametersGroupBox.Name = "channelParametersGroupBox"
        Me.channelParametersGroupBox.Size = New System.Drawing.Size(232, 128)
        Me.channelParametersGroupBox.TabIndex = 1
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
        Me.maximumLabel.Location = New System.Drawing.Point(24, 90)
        Me.maximumLabel.Name = "maximumLabel"
        Me.maximumLabel.Size = New System.Drawing.Size(96, 16)
        Me.maximumLabel.TabIndex = 4
        Me.maximumLabel.Text = "Maximum (Volts):"
        '
        'minimumLabel
        '
        Me.minimumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.minimumLabel.Location = New System.Drawing.Point(24, 58)
        Me.minimumLabel.Name = "minimumLabel"
        Me.minimumLabel.Size = New System.Drawing.Size(96, 16)
        Me.minimumLabel.TabIndex = 2
        Me.minimumLabel.Text = "Minimum (Volts):"
        '
        'physicalChannelLabel
        '
        Me.physicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.physicalChannelLabel.Location = New System.Drawing.Point(24, 28)
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
        Me.acquisitionResultGroupBox.Location = New System.Drawing.Point(8, 144)
        Me.acquisitionResultGroupBox.Name = "acquisitionResultGroupBox"
        Me.acquisitionResultGroupBox.Size = New System.Drawing.Size(232, 176)
        Me.acquisitionResultGroupBox.TabIndex = 2
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
        Me.resultLabel.Text = "Acquisition Data (Volts):"
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
        Me.acquisitionDataGrid.Size = New System.Drawing.Size(216, 136)
        Me.acquisitionDataGrid.TabIndex = 1
        Me.acquisitionDataGrid.TabStop = False
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(554, 328)
        Me.Controls.Add(Me.acquisitionResultGroupBox)
        Me.Controls.Add(Me.triggerParametersGroupBox)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.timingParametersGroupBox)
        Me.Controls.Add(Me.channelParametersGroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Acquire Voltage Samples with  Digital Start and Reference Triggers"
        Me.triggerParametersGroupBox.ResumeLayout(False)
        CType(Me.preTriggerNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.startEdgeGroupBox.ResumeLayout(False)
        Me.referenceEdgeGroupBox.ResumeLayout(False)
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


    Private Sub startButton_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles startButton.Click
        startButton.Enabled = False
        Try
            ' Create a new Task
            myTask = New Task()

            ' Initialize Local Variables
            Dim sampleRate As Double = Convert.ToDouble(rateNumeric.Value)
            Dim rangeMin As Double = Convert.ToDouble(minimumValueNumeric.Value)
            Dim rangeMax As Double = Convert.ToDouble(maximumValueNumeric.Value)

            ' Create a virtual channel
            myTask.AIChannels.CreateVoltageChannel(physicalChannelComboBox.Text, "", CType(-1, AITerminalConfiguration), rangeMin, rangeMax, AIVoltageUnits.Volts)

            ' Configure Timing Specs 
            myTask.Timing.ConfigureSampleClock("", sampleRate, SampleClockActiveEdge.Rising, SampleQuantityMode.FiniteSamples, Convert.ToInt32(samplesPerChannelNumeric.Value))

            ' Configure Start and Reference Triggers
            myTask.Triggers.StartTrigger.ConfigureDigitalEdgeTrigger(startTriggerSourceTextBox.Text, startEdge)
            myTask.Triggers.ReferenceTrigger.ConfigureDigitalEdgeTrigger(referenceTriggerSourceTextBox.Text, referenceEdge, preTriggerNumeric.Value)

            ' Verify the Task
            myTask.Control(TaskAction.Verify)

            reader = New AnalogMultiChannelReader(myTask.Stream)

            ' Prepare the table for Data
            InitializeDataTable(myTask.AIChannels, dataTable)
            acquisitionDataGrid.DataSource = dataTable

            ' Use SynchronizeCallbacks to specify that the object 
            ' marshals callbacks across threads appropriately.
            reader.SynchronizeCallbacks = True
            reader.BeginReadWaveform(Convert.ToInt32(samplesPerChannelNumeric.Value), AddressOf myCallback, Nothing)

        Catch exception As DaqException
            startButton.Enabled = True
            MessageBox.Show(exception.Message)
            myTask.Dispose()
        End Try
    End Sub

    Private Sub myCallback(ByVal ar As IAsyncResult)

        Try
            ' Read one point to data per channel

            Dim data() As AnalogWaveform(Of Double) = reader.EndReadWaveform(ar)
            ' Plot your data here
            dataToDataTable(data, dataTable)

        Catch exception As DaqException
            MessageBox.Show(exception.Message)
        Finally
            myTask.Dispose()
            startButton.Enabled = True
        End Try

    End Sub

    Private Sub startEdgeRisingButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles startEdgeRisingButton.CheckedChanged
        'Configure the Start Trigger edge as Rising
        If startEdgeRisingButton.Checked Then startEdge = DigitalEdgeStartTriggerEdge.Rising
    End Sub

    Private Sub startEdgeFallingButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles startEdgeFallingButton.CheckedChanged
        'Configure the Start Trigger edge as Falling
        If startEdgeFallingButton.Checked Then startEdge = DigitalEdgeStartTriggerEdge.Falling
    End Sub

    Private Sub referenceEdgeRisingButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles referenceEdgeRisingButton.CheckedChanged
        'Configure the Reference Trigger edge as Rising
        If referenceEdgeRisingButton.Checked Then referenceEdge = DigitalEdgeReferenceTriggerEdge.Rising
    End Sub

    Private Sub referenceEdgeFallingButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles referenceEdgeFallingButton.CheckedChanged
        'Configure the Reference Trigger edge as Falling
        If referenceEdgeFallingButton.Checked Then referenceEdge = DigitalEdgeReferenceTriggerEdge.Falling
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
