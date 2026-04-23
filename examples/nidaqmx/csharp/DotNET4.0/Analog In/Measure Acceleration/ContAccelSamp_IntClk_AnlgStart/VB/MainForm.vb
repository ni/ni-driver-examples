'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   ContAccelSamp_IntClk_AnlgStart
'
' Category:
'   AI
'
' Description:
'   This example demonstrates how to create an analog input acceleration task
'   and perform a continuous acquisition using option IEPE excitation, analog
'   triggering, and overload detection.
'
' Instructions for running:
'   1.  Select the physical channel to correspond to where your signal is input
'       on your device.
'   2.  Enter the minimum and maximum expected acceleration values.Note:  To
'       optimize gain selection,  try to match the Input Ranges to the expected 
'       level of the measured signal.
'   3.  Program the analog input terminal configuration and IEPE excitation
'       settings for your device.
'   4.  If your device supports overload detection, check the Overload Detection
'       checkbox.  Refer to your device documentation to see if overload
'       protection is supported.
'   5.  Set the rate of the acquisition.  Also set the Samples to Read control. 
'       This will determine how many samples are acquired with each read from
'       the device.  This also determines how many points are plotted on the
'       graph each iteration.Note:  The rate should be at least twice as fast as
'       the maximum frequency component of the signal being acquired.
'   6.  Set the source of the Analog Edge Start Trigger. By default this is
'       APFI0.
'   7.  Set the slope and level of desired analog edge condition.
'   8.  Input the sensitivity and units for your accelerometer.
'
' Steps:
'   1.  Create a Task object and create an analog input acceleration channel. 
'       This step defines accelerometer sensitivity, desired range, and IEPE
'       excitation.
'   2.  Set input coupling.
'   3.  Set the sample rate and define a continuous acquisition.
'   4.  Define the trigger channel, trigger level, and rising/falling edge for
'       an analog start trigger.
'   5.  Create the AnalogMultiChannelReader object.
'   6.  Call AnalogMultiChannelReader.BeginReadWaveform to install a callback
'       and begin the asynchronous read operation.
'   7.  Inside the callback, call AnalogMultiChannelReader.EndReadWaveform to
'       retrieve the data from the read operation.  In the callback, display the
'       data and check for overloaded channels.
'   8.  Call AnalogMultiChannelReader.BeginMemoryOptimizedReadWaveform
'   9.  When the user presses the stop button, stop the task by calling
'       Task.Stop().  Dispose the Task object to clean-up any resources
'       associated with the task.
'   10. Handle any DaqExceptions, if they occur.
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
'   control.  Also, make sure your analog trigger terminal matches the Trigger
'   Source Control.  For further connection information, refer to your hardware
'   reference manual.
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

Public Class MainFrom
    Inherits System.Windows.Forms.Form

    Private inputCoupling As AICoupling
    Private analogInReader As AnalogMultiChannelReader
    Private excitationSource As AIExcitationSource
    Private sensitivityUnits As AIAccelerometerSensitivityUnits
    Private terminalConfiguration As AITerminalConfiguration
    Private triggerSlope As AnalogEdgeStartTriggerSlope
    Private myTask As Task
    Private runningTask As Task
    Private analogCallback As AsyncCallback

    Private data As AnalogWaveform(Of Double)()
    Private dataColumn As DataColumn()
    Private dataTable As DataTable

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()
        Application.EnableVisualStyles()

        'This call is required by the Windows Form Designer.
        InitializeComponent()
        triggerSlopeComboBox.SelectedIndex = 0
        sensitivityUnitComboBox.SelectedIndex = 0
        excitationSourceComboBox.SelectedIndex = 0
        terminalConfigurationComboBox.SelectedIndex = 3
        inputCouplingComboBox.SelectedIndex = 0
        dataTable = New DataTable
        stopButton.Enabled = False

        physicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AI, PhysicalChannelAccess.External))
        If (physicalChannelComboBox.Items.Count > 0) Then
            physicalChannelComboBox.SelectedIndex = 0
        End If

        'Add any initialization after the InitializeComponent() call

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
    Friend WithEvents timingParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents samplesPerChannelNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents rateNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents samplesLabel As System.Windows.Forms.Label
    Friend WithEvents rateLabel As System.Windows.Forms.Label
    Friend WithEvents deviceParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents excitationValueNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents excitationSourceComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents terminalConfigurationComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents excitationValueLabel As System.Windows.Forms.Label
    Friend WithEvents excitationSourceLabel As System.Windows.Forms.Label
    Friend WithEvents stopButton As System.Windows.Forms.Button
    Friend WithEvents acquisitionResultsGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents resultLabel As System.Windows.Forms.Label
    Friend WithEvents acquisitionDataGrid As System.Windows.Forms.DataGrid
    Friend WithEvents accelerometerParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents sensitivityNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents sensitivityUnitComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents sensitivityUnitLabel As System.Windows.Forms.Label
    Friend WithEvents sensitivityLabel As System.Windows.Forms.Label
    Friend WithEvents triggerParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents triggerLevelNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents triggerLevelLabel As System.Windows.Forms.Label
    Friend WithEvents triggerSlopeLabel As System.Windows.Forms.Label
    Friend WithEvents triggerSourceLabel As System.Windows.Forms.Label
    Friend WithEvents triggerSourceTextBox As System.Windows.Forms.TextBox
    Friend WithEvents triggerSlopeComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents startButton As System.Windows.Forms.Button
    Friend WithEvents channelParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents maximumLabel As System.Windows.Forms.Label
    Friend WithEvents minimumLabel As System.Windows.Forms.Label
    Friend WithEvents physicalChannelLabel As System.Windows.Forms.Label
    Friend WithEvents terminalConfigurationLabel As System.Windows.Forms.Label
    Friend WithEvents minimumValueNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents maximumValueNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents triggerSourceInfoAsterisk As System.Windows.Forms.Label
    Friend WithEvents triggerSourceInfo As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents inputCouplingLabel As System.Windows.Forms.Label
    Friend WithEvents inputCouplingComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents overloadingGoupBox As System.Windows.Forms.GroupBox
    Friend WithEvents overloadDetectedTextBox As System.Windows.Forms.TextBox
    Friend WithEvents overloadedChannelsLabel As System.Windows.Forms.Label
    Friend WithEvents overloadedChannelsListBox As System.Windows.Forms.ListBox
    Friend WithEvents overloadDetectionLabel As System.Windows.Forms.Label
    Friend WithEvents overloadDetectionCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents physicalChannelComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents hysteresisNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents hysteresisLabel As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MainFrom))
        Me.timingParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.samplesPerChannelNumeric = New System.Windows.Forms.NumericUpDown
        Me.rateNumeric = New System.Windows.Forms.NumericUpDown
        Me.samplesLabel = New System.Windows.Forms.Label
        Me.rateLabel = New System.Windows.Forms.Label
        Me.deviceParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.excitationValueNumeric = New System.Windows.Forms.NumericUpDown
        Me.excitationSourceComboBox = New System.Windows.Forms.ComboBox
        Me.terminalConfigurationComboBox = New System.Windows.Forms.ComboBox
        Me.excitationValueLabel = New System.Windows.Forms.Label
        Me.excitationSourceLabel = New System.Windows.Forms.Label
        Me.terminalConfigurationLabel = New System.Windows.Forms.Label
        Me.inputCouplingLabel = New System.Windows.Forms.Label
        Me.inputCouplingComboBox = New System.Windows.Forms.ComboBox
        Me.stopButton = New System.Windows.Forms.Button
        Me.acquisitionResultsGroupBox = New System.Windows.Forms.GroupBox
        Me.resultLabel = New System.Windows.Forms.Label
        Me.acquisitionDataGrid = New System.Windows.Forms.DataGrid
        Me.accelerometerParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.sensitivityNumeric = New System.Windows.Forms.NumericUpDown
        Me.sensitivityUnitComboBox = New System.Windows.Forms.ComboBox
        Me.sensitivityUnitLabel = New System.Windows.Forms.Label
        Me.sensitivityLabel = New System.Windows.Forms.Label
        Me.triggerParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.triggerSourceInfoAsterisk = New System.Windows.Forms.Label
        Me.triggerSourceInfo = New System.Windows.Forms.Label
        Me.triggerLevelNumeric = New System.Windows.Forms.NumericUpDown
        Me.triggerLevelLabel = New System.Windows.Forms.Label
        Me.triggerSlopeLabel = New System.Windows.Forms.Label
        Me.triggerSourceLabel = New System.Windows.Forms.Label
        Me.triggerSourceTextBox = New System.Windows.Forms.TextBox
        Me.triggerSlopeComboBox = New System.Windows.Forms.ComboBox
        Me.startButton = New System.Windows.Forms.Button
        Me.channelParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.physicalChannelComboBox = New System.Windows.Forms.ComboBox
        Me.minimumValueNumeric = New System.Windows.Forms.NumericUpDown
        Me.maximumValueNumeric = New System.Windows.Forms.NumericUpDown
        Me.maximumLabel = New System.Windows.Forms.Label
        Me.minimumLabel = New System.Windows.Forms.Label
        Me.physicalChannelLabel = New System.Windows.Forms.Label
        Me.overloadDetectionLabel = New System.Windows.Forms.Label
        Me.overloadDetectionCheckBox = New System.Windows.Forms.CheckBox
        Me.overloadingGoupBox = New System.Windows.Forms.GroupBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.overloadDetectedTextBox = New System.Windows.Forms.TextBox
        Me.overloadedChannelsLabel = New System.Windows.Forms.Label
        Me.overloadedChannelsListBox = New System.Windows.Forms.ListBox
        Me.hysteresisNumeric = New System.Windows.Forms.NumericUpDown
        Me.hysteresisLabel = New System.Windows.Forms.Label
        Me.timingParametersGroupBox.SuspendLayout()
        CType(Me.samplesPerChannelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rateNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.deviceParametersGroupBox.SuspendLayout()
        CType(Me.excitationValueNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.acquisitionResultsGroupBox.SuspendLayout()
        CType(Me.acquisitionDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.accelerometerParametersGroupBox.SuspendLayout()
        CType(Me.sensitivityNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.triggerParametersGroupBox.SuspendLayout()
        CType(Me.triggerLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.channelParametersGroupBox.SuspendLayout()
        CType(Me.minimumValueNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.maximumValueNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.overloadingGoupBox.SuspendLayout()
        CType(Me.hysteresisNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'timingParametersGroupBox
        '
        Me.timingParametersGroupBox.Controls.Add(Me.samplesPerChannelNumeric)
        Me.timingParametersGroupBox.Controls.Add(Me.rateNumeric)
        Me.timingParametersGroupBox.Controls.Add(Me.samplesLabel)
        Me.timingParametersGroupBox.Controls.Add(Me.rateLabel)
        Me.timingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.timingParametersGroupBox.Location = New System.Drawing.Point(8, 208)
        Me.timingParametersGroupBox.Name = "timingParametersGroupBox"
        Me.timingParametersGroupBox.Size = New System.Drawing.Size(232, 88)
        Me.timingParametersGroupBox.TabIndex = 3
        Me.timingParametersGroupBox.TabStop = False
        Me.timingParametersGroupBox.Text = "Timing Parameters"
        '
        'samplesPerChannelNumeric
        '
        Me.samplesPerChannelNumeric.Location = New System.Drawing.Point(120, 56)
        Me.samplesPerChannelNumeric.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.samplesPerChannelNumeric.Name = "samplesPerChannelNumeric"
        Me.samplesPerChannelNumeric.Size = New System.Drawing.Size(96, 20)
        Me.samplesPerChannelNumeric.TabIndex = 3
        Me.samplesPerChannelNumeric.Value = New Decimal(New Integer() {1000, 0, 0, 0})
        '
        'rateNumeric
        '
        Me.rateNumeric.DecimalPlaces = 2
        Me.rateNumeric.Location = New System.Drawing.Point(120, 24)
        Me.rateNumeric.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.rateNumeric.Name = "rateNumeric"
        Me.rateNumeric.Size = New System.Drawing.Size(96, 20)
        Me.rateNumeric.TabIndex = 1
        Me.rateNumeric.Value = New Decimal(New Integer() {10000, 0, 0, 0})
        '
        'samplesLabel
        '
        Me.samplesLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.samplesLabel.Location = New System.Drawing.Point(16, 58)
        Me.samplesLabel.Name = "samplesLabel"
        Me.samplesLabel.Size = New System.Drawing.Size(104, 16)
        Me.samplesLabel.TabIndex = 2
        Me.samplesLabel.Text = "Samples / Channel:"
        '
        'rateLabel
        '
        Me.rateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.rateLabel.Location = New System.Drawing.Point(16, 26)
        Me.rateLabel.Name = "rateLabel"
        Me.rateLabel.Size = New System.Drawing.Size(56, 16)
        Me.rateLabel.TabIndex = 0
        Me.rateLabel.Text = "Rate (Hz):"
        '
        'deviceParametersGroupBox
        '
        Me.deviceParametersGroupBox.Controls.Add(Me.excitationValueNumeric)
        Me.deviceParametersGroupBox.Controls.Add(Me.excitationSourceComboBox)
        Me.deviceParametersGroupBox.Controls.Add(Me.terminalConfigurationComboBox)
        Me.deviceParametersGroupBox.Controls.Add(Me.excitationValueLabel)
        Me.deviceParametersGroupBox.Controls.Add(Me.excitationSourceLabel)
        Me.deviceParametersGroupBox.Controls.Add(Me.terminalConfigurationLabel)
        Me.deviceParametersGroupBox.Controls.Add(Me.inputCouplingLabel)
        Me.deviceParametersGroupBox.Controls.Add(Me.inputCouplingComboBox)
        Me.deviceParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.deviceParametersGroupBox.Location = New System.Drawing.Point(256, 8)
        Me.deviceParametersGroupBox.Name = "deviceParametersGroupBox"
        Me.deviceParametersGroupBox.Size = New System.Drawing.Size(264, 152)
        Me.deviceParametersGroupBox.TabIndex = 6
        Me.deviceParametersGroupBox.TabStop = False
        Me.deviceParametersGroupBox.Text = "Device Parameters"
        '
        'excitationValueNumeric
        '
        Me.excitationValueNumeric.DecimalPlaces = 3
        Me.excitationValueNumeric.Location = New System.Drawing.Point(136, 120)
        Me.excitationValueNumeric.Name = "excitationValueNumeric"
        Me.excitationValueNumeric.TabIndex = 7
        Me.excitationValueNumeric.Value = New Decimal(New Integer() {4, 0, 0, 196608})
        '
        'excitationSourceComboBox
        '
        Me.excitationSourceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.excitationSourceComboBox.ItemHeight = 13
        Me.excitationSourceComboBox.Items.AddRange(New Object() {"Internal", "External", "None"})
        Me.excitationSourceComboBox.Location = New System.Drawing.Point(136, 88)
        Me.excitationSourceComboBox.Name = "excitationSourceComboBox"
        Me.excitationSourceComboBox.Size = New System.Drawing.Size(120, 21)
        Me.excitationSourceComboBox.TabIndex = 5
        '
        'terminalConfigurationComboBox
        '
        Me.terminalConfigurationComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.terminalConfigurationComboBox.ItemHeight = 13
        Me.terminalConfigurationComboBox.Items.AddRange(New Object() {"Rse", "Nrse", "Differential", "Pseudodifferential", "Let NI-DAQ Choose"})
        Me.terminalConfigurationComboBox.Location = New System.Drawing.Point(136, 56)
        Me.terminalConfigurationComboBox.Name = "terminalConfigurationComboBox"
        Me.terminalConfigurationComboBox.Size = New System.Drawing.Size(120, 21)
        Me.terminalConfigurationComboBox.TabIndex = 3
        '
        'excitationValueLabel
        '
        Me.excitationValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.excitationValueLabel.Location = New System.Drawing.Point(16, 122)
        Me.excitationValueLabel.Name = "excitationValueLabel"
        Me.excitationValueLabel.Size = New System.Drawing.Size(128, 16)
        Me.excitationValueLabel.TabIndex = 6
        Me.excitationValueLabel.Text = "Excitation Value:"
        '
        'excitationSourceLabel
        '
        Me.excitationSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.excitationSourceLabel.Location = New System.Drawing.Point(16, 90)
        Me.excitationSourceLabel.Name = "excitationSourceLabel"
        Me.excitationSourceLabel.Size = New System.Drawing.Size(128, 16)
        Me.excitationSourceLabel.TabIndex = 4
        Me.excitationSourceLabel.Text = "Excitation Source:"
        '
        'terminalConfigurationLabel
        '
        Me.terminalConfigurationLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.terminalConfigurationLabel.Location = New System.Drawing.Point(16, 59)
        Me.terminalConfigurationLabel.Name = "terminalConfigurationLabel"
        Me.terminalConfigurationLabel.Size = New System.Drawing.Size(128, 14)
        Me.terminalConfigurationLabel.TabIndex = 2
        Me.terminalConfigurationLabel.Text = "Terminal Configuration:"
        '
        'inputCouplingLabel
        '
        Me.inputCouplingLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.inputCouplingLabel.Location = New System.Drawing.Point(16, 26)
        Me.inputCouplingLabel.Name = "inputCouplingLabel"
        Me.inputCouplingLabel.Size = New System.Drawing.Size(112, 16)
        Me.inputCouplingLabel.TabIndex = 0
        Me.inputCouplingLabel.Text = "Input Coupling:"
        '
        'inputCouplingComboBox
        '
        Me.inputCouplingComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.inputCouplingComboBox.ItemHeight = 13
        Me.inputCouplingComboBox.Items.AddRange(New Object() {"AC", "DC", "Gnd"})
        Me.inputCouplingComboBox.Location = New System.Drawing.Point(136, 24)
        Me.inputCouplingComboBox.Name = "inputCouplingComboBox"
        Me.inputCouplingComboBox.Size = New System.Drawing.Size(120, 21)
        Me.inputCouplingComboBox.TabIndex = 1
        '
        'stopButton
        '
        Me.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.stopButton.Location = New System.Drawing.Point(696, 216)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(80, 25)
        Me.stopButton.TabIndex = 1
        Me.stopButton.Text = "Stop"
        '
        'acquisitionResultsGroupBox
        '
        Me.acquisitionResultsGroupBox.Controls.Add(Me.resultLabel)
        Me.acquisitionResultsGroupBox.Controls.Add(Me.acquisitionDataGrid)
        Me.acquisitionResultsGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.acquisitionResultsGroupBox.Location = New System.Drawing.Point(256, 168)
        Me.acquisitionResultsGroupBox.Name = "acquisitionResultsGroupBox"
        Me.acquisitionResultsGroupBox.Size = New System.Drawing.Size(264, 432)
        Me.acquisitionResultsGroupBox.TabIndex = 7
        Me.acquisitionResultsGroupBox.TabStop = False
        Me.acquisitionResultsGroupBox.Text = "Acquisition Results"
        '
        'resultLabel
        '
        Me.resultLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.resultLabel.Location = New System.Drawing.Point(8, 16)
        Me.resultLabel.Name = "resultLabel"
        Me.resultLabel.Size = New System.Drawing.Size(112, 16)
        Me.resultLabel.TabIndex = 0
        Me.resultLabel.Text = "Acquisition Data:"
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
        Me.acquisitionDataGrid.Size = New System.Drawing.Size(248, 392)
        Me.acquisitionDataGrid.TabIndex = 1
        Me.acquisitionDataGrid.TabStop = False
        '
        'accelerometerParametersGroupBox
        '
        Me.accelerometerParametersGroupBox.Controls.Add(Me.sensitivityNumeric)
        Me.accelerometerParametersGroupBox.Controls.Add(Me.sensitivityUnitComboBox)
        Me.accelerometerParametersGroupBox.Controls.Add(Me.sensitivityUnitLabel)
        Me.accelerometerParametersGroupBox.Controls.Add(Me.sensitivityLabel)
        Me.accelerometerParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.accelerometerParametersGroupBox.Location = New System.Drawing.Point(8, 520)
        Me.accelerometerParametersGroupBox.Name = "accelerometerParametersGroupBox"
        Me.accelerometerParametersGroupBox.Size = New System.Drawing.Size(232, 88)
        Me.accelerometerParametersGroupBox.TabIndex = 5
        Me.accelerometerParametersGroupBox.TabStop = False
        Me.accelerometerParametersGroupBox.Text = "Accelerometer Parameters"
        '
        'sensitivityNumeric
        '
        Me.sensitivityNumeric.DecimalPlaces = 2
        Me.sensitivityNumeric.Location = New System.Drawing.Point(120, 24)
        Me.sensitivityNumeric.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.sensitivityNumeric.Name = "sensitivityNumeric"
        Me.sensitivityNumeric.Size = New System.Drawing.Size(96, 20)
        Me.sensitivityNumeric.TabIndex = 1
        Me.sensitivityNumeric.Value = New Decimal(New Integer() {50, 0, 0, 0})
        '
        'sensitivityUnitComboBox
        '
        Me.sensitivityUnitComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.sensitivityUnitComboBox.Items.AddRange(New Object() {"mVolts/G", "Volts/G"})
        Me.sensitivityUnitComboBox.Location = New System.Drawing.Point(120, 56)
        Me.sensitivityUnitComboBox.Name = "sensitivityUnitComboBox"
        Me.sensitivityUnitComboBox.Size = New System.Drawing.Size(96, 21)
        Me.sensitivityUnitComboBox.TabIndex = 3
        '
        'sensitivityUnitLabel
        '
        Me.sensitivityUnitLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.sensitivityUnitLabel.Location = New System.Drawing.Point(16, 58)
        Me.sensitivityUnitLabel.Name = "sensitivityUnitLabel"
        Me.sensitivityUnitLabel.Size = New System.Drawing.Size(88, 16)
        Me.sensitivityUnitLabel.TabIndex = 2
        Me.sensitivityUnitLabel.Text = "Sensitivity Units:"
        '
        'sensitivityLabel
        '
        Me.sensitivityLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.sensitivityLabel.Location = New System.Drawing.Point(16, 26)
        Me.sensitivityLabel.Name = "sensitivityLabel"
        Me.sensitivityLabel.Size = New System.Drawing.Size(88, 16)
        Me.sensitivityLabel.TabIndex = 0
        Me.sensitivityLabel.Text = "Sensitivity:"
        '
        'triggerParametersGroupBox
        '
        Me.triggerParametersGroupBox.Controls.Add(Me.triggerSourceInfoAsterisk)
        Me.triggerParametersGroupBox.Controls.Add(Me.triggerSourceInfo)
        Me.triggerParametersGroupBox.Controls.Add(Me.triggerLevelNumeric)
        Me.triggerParametersGroupBox.Controls.Add(Me.triggerLevelLabel)
        Me.triggerParametersGroupBox.Controls.Add(Me.triggerSlopeLabel)
        Me.triggerParametersGroupBox.Controls.Add(Me.triggerSourceLabel)
        Me.triggerParametersGroupBox.Controls.Add(Me.triggerSourceTextBox)
        Me.triggerParametersGroupBox.Controls.Add(Me.triggerSlopeComboBox)
        Me.triggerParametersGroupBox.Controls.Add(Me.hysteresisNumeric)
        Me.triggerParametersGroupBox.Controls.Add(Me.hysteresisLabel)
        Me.triggerParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.triggerParametersGroupBox.Location = New System.Drawing.Point(8, 304)
        Me.triggerParametersGroupBox.Name = "triggerParametersGroupBox"
        Me.triggerParametersGroupBox.Size = New System.Drawing.Size(232, 208)
        Me.triggerParametersGroupBox.TabIndex = 4
        Me.triggerParametersGroupBox.TabStop = False
        Me.triggerParametersGroupBox.Text = "Trigger Parameters"
        '
        'triggerSourceInfoAsterisk
        '
        Me.triggerSourceInfoAsterisk.Location = New System.Drawing.Point(8, 144)
        Me.triggerSourceInfoAsterisk.Name = "triggerSourceInfoAsterisk"
        Me.triggerSourceInfoAsterisk.Size = New System.Drawing.Size(8, 23)
        Me.triggerSourceInfoAsterisk.TabIndex = 6
        Me.triggerSourceInfoAsterisk.Text = "*"
        '
        'triggerSourceInfo
        '
        Me.triggerSourceInfo.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.triggerSourceInfo.Location = New System.Drawing.Point(16, 144)
        Me.triggerSourceInfo.Name = "triggerSourceInfo"
        Me.triggerSourceInfo.Size = New System.Drawing.Size(208, 56)
        Me.triggerSourceInfo.TabIndex = 7
        Me.triggerSourceInfo.Text = "APFI0 is the default Analog Trigger pin for M Series devices.  Please refer to you" & _
        "r device documentation for information regarding valid Analog Triggers for your " & _
        "device."
        '
        'triggerLevelNumeric
        '
        Me.triggerLevelNumeric.DecimalPlaces = 2
        Me.triggerLevelNumeric.Location = New System.Drawing.Point(120, 88)
        Me.triggerLevelNumeric.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.triggerLevelNumeric.Minimum = New Decimal(New Integer() {10, 0, 0, -2147483648})
        Me.triggerLevelNumeric.Name = "triggerLevelNumeric"
        Me.triggerLevelNumeric.Size = New System.Drawing.Size(96, 20)
        Me.triggerLevelNumeric.TabIndex = 5
        '
        'triggerLevelLabel
        '
        Me.triggerLevelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.triggerLevelLabel.Location = New System.Drawing.Point(16, 90)
        Me.triggerLevelLabel.Name = "triggerLevelLabel"
        Me.triggerLevelLabel.Size = New System.Drawing.Size(96, 16)
        Me.triggerLevelLabel.TabIndex = 4
        Me.triggerLevelLabel.Text = "Trigger Level (g):"
        '
        'triggerSlopeLabel
        '
        Me.triggerSlopeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.triggerSlopeLabel.Location = New System.Drawing.Point(16, 58)
        Me.triggerSlopeLabel.Name = "triggerSlopeLabel"
        Me.triggerSlopeLabel.Size = New System.Drawing.Size(88, 16)
        Me.triggerSlopeLabel.TabIndex = 2
        Me.triggerSlopeLabel.Text = "Trigger Slope:"
        '
        'triggerSourceLabel
        '
        Me.triggerSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.triggerSourceLabel.Location = New System.Drawing.Point(16, 26)
        Me.triggerSourceLabel.Name = "triggerSourceLabel"
        Me.triggerSourceLabel.Size = New System.Drawing.Size(88, 16)
        Me.triggerSourceLabel.TabIndex = 0
        Me.triggerSourceLabel.Text = "Trigger Source:*"
        '
        'triggerSourceTextBox
        '
        Me.triggerSourceTextBox.Location = New System.Drawing.Point(120, 24)
        Me.triggerSourceTextBox.Name = "triggerSourceTextBox"
        Me.triggerSourceTextBox.Size = New System.Drawing.Size(96, 20)
        Me.triggerSourceTextBox.TabIndex = 1
        Me.triggerSourceTextBox.Text = "APFI0"
        '
        'triggerSlopeComboBox
        '
        Me.triggerSlopeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.triggerSlopeComboBox.Items.AddRange(New Object() {"Rising", "Falling"})
        Me.triggerSlopeComboBox.Location = New System.Drawing.Point(120, 56)
        Me.triggerSlopeComboBox.Name = "triggerSlopeComboBox"
        Me.triggerSlopeComboBox.Size = New System.Drawing.Size(96, 21)
        Me.triggerSlopeComboBox.TabIndex = 3
        '
        'startButton
        '
        Me.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.startButton.Location = New System.Drawing.Point(592, 216)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(80, 25)
        Me.startButton.TabIndex = 0
        Me.startButton.Text = "Start"
        '
        'channelParametersGroupBox
        '
        Me.channelParametersGroupBox.Controls.Add(Me.physicalChannelComboBox)
        Me.channelParametersGroupBox.Controls.Add(Me.minimumValueNumeric)
        Me.channelParametersGroupBox.Controls.Add(Me.maximumValueNumeric)
        Me.channelParametersGroupBox.Controls.Add(Me.maximumLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.minimumLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.physicalChannelLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.overloadDetectionLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.overloadDetectionCheckBox)
        Me.channelParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.channelParametersGroupBox.Location = New System.Drawing.Point(8, 8)
        Me.channelParametersGroupBox.Name = "channelParametersGroupBox"
        Me.channelParametersGroupBox.Size = New System.Drawing.Size(232, 192)
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
        Me.maximumLabel.Size = New System.Drawing.Size(112, 16)
        Me.maximumLabel.TabIndex = 4
        Me.maximumLabel.Text = "Maximum Value (g):"
        '
        'minimumLabel
        '
        Me.minimumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.minimumLabel.Location = New System.Drawing.Point(16, 57)
        Me.minimumLabel.Name = "minimumLabel"
        Me.minimumLabel.Size = New System.Drawing.Size(104, 18)
        Me.minimumLabel.TabIndex = 2
        Me.minimumLabel.Text = "Minimum Value (g):"
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
        'overloadDetectionLabel
        '
        Me.overloadDetectionLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.overloadDetectionLabel.Location = New System.Drawing.Point(16, 152)
        Me.overloadDetectionLabel.Name = "overloadDetectionLabel"
        Me.overloadDetectionLabel.Size = New System.Drawing.Size(208, 32)
        Me.overloadDetectionLabel.TabIndex = 7
        Me.overloadDetectionLabel.Text = "* Check this box if you are using a DSA device and want to check for Overloads."
        '
        'overloadDetectionCheckBox
        '
        Me.overloadDetectionCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.overloadDetectionCheckBox.Location = New System.Drawing.Point(48, 120)
        Me.overloadDetectionCheckBox.Name = "overloadDetectionCheckBox"
        Me.overloadDetectionCheckBox.Size = New System.Drawing.Size(128, 24)
        Me.overloadDetectionCheckBox.TabIndex = 6
        Me.overloadDetectionCheckBox.Text = "Overload Detection *"
        '
        'overloadingGoupBox
        '
        Me.overloadingGoupBox.Controls.Add(Me.Label1)
        Me.overloadingGoupBox.Controls.Add(Me.overloadDetectedTextBox)
        Me.overloadingGoupBox.Controls.Add(Me.overloadedChannelsLabel)
        Me.overloadingGoupBox.Controls.Add(Me.overloadedChannelsListBox)
        Me.overloadingGoupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.overloadingGoupBox.Location = New System.Drawing.Point(536, 8)
        Me.overloadingGoupBox.Name = "overloadingGoupBox"
        Me.overloadingGoupBox.Size = New System.Drawing.Size(296, 192)
        Me.overloadingGoupBox.TabIndex = 8
        Me.overloadingGoupBox.TabStop = False
        Me.overloadingGoupBox.Text = "Overloading"
        '
        'Label1
        '
        Me.Label1.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label1.Location = New System.Drawing.Point(16, 26)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(128, 16)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Overload Detected:"
        '
        'overloadDetectedTextBox
        '
        Me.overloadDetectedTextBox.Enabled = False
        Me.overloadDetectedTextBox.Location = New System.Drawing.Point(176, 24)
        Me.overloadDetectedTextBox.Name = "overloadDetectedTextBox"
        Me.overloadDetectedTextBox.TabIndex = 1
        Me.overloadDetectedTextBox.Text = "TextBox1"
        '
        'overloadedChannelsLabel
        '
        Me.overloadedChannelsLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.overloadedChannelsLabel.Location = New System.Drawing.Point(16, 59)
        Me.overloadedChannelsLabel.Name = "overloadedChannelsLabel"
        Me.overloadedChannelsLabel.Size = New System.Drawing.Size(128, 16)
        Me.overloadedChannelsLabel.TabIndex = 2
        Me.overloadedChannelsLabel.Text = "Overloaded Channels:"
        '
        'overloadedChannelsListBox
        '
        Me.overloadedChannelsListBox.Enabled = False
        Me.overloadedChannelsListBox.Location = New System.Drawing.Point(16, 80)
        Me.overloadedChannelsListBox.Name = "overloadedChannelsListBox"
        Me.overloadedChannelsListBox.Size = New System.Drawing.Size(264, 95)
        Me.overloadedChannelsListBox.TabIndex = 3
        '
        'hysteresisNumeric
        '
        Me.hysteresisNumeric.DecimalPlaces = 2
        Me.hysteresisNumeric.Location = New System.Drawing.Point(120, 120)
        Me.hysteresisNumeric.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.hysteresisNumeric.Minimum = New Decimal(New Integer() {10, 0, 0, -2147483648})
        Me.hysteresisNumeric.Name = "hysteresisNumeric"
        Me.hysteresisNumeric.Size = New System.Drawing.Size(96, 20)
        Me.hysteresisNumeric.TabIndex = 5
        '
        'hysteresisLabel
        '
        Me.hysteresisLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.hysteresisLabel.Location = New System.Drawing.Point(16, 120)
        Me.hysteresisLabel.Name = "hysteresisLabel"
        Me.hysteresisLabel.Size = New System.Drawing.Size(96, 16)
        Me.hysteresisLabel.TabIndex = 4
        Me.hysteresisLabel.Text = "Hysteresis (g):"
        '
        'MainFrom
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(842, 616)
        Me.Controls.Add(Me.overloadingGoupBox)
        Me.Controls.Add(Me.timingParametersGroupBox)
        Me.Controls.Add(Me.deviceParametersGroupBox)
        Me.Controls.Add(Me.stopButton)
        Me.Controls.Add(Me.acquisitionResultsGroupBox)
        Me.Controls.Add(Me.accelerometerParametersGroupBox)
        Me.Controls.Add(Me.triggerParametersGroupBox)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.channelParametersGroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainFrom"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Continuous Acceleration - Internal Clock - Analog Start"
        Me.timingParametersGroupBox.ResumeLayout(False)
        CType(Me.samplesPerChannelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rateNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.deviceParametersGroupBox.ResumeLayout(False)
        CType(Me.excitationValueNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.acquisitionResultsGroupBox.ResumeLayout(False)
        CType(Me.acquisitionDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.accelerometerParametersGroupBox.ResumeLayout(False)
        CType(Me.sensitivityNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.triggerParametersGroupBox.ResumeLayout(False)
        CType(Me.triggerLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.channelParametersGroupBox.ResumeLayout(False)
        CType(Me.minimumValueNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.maximumValueNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.overloadingGoupBox.ResumeLayout(False)
        CType(Me.hysteresisNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub startButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles startButton.Click
        If runningTask Is Nothing Then

            Try
                stopButton.Enabled = True
                startButton.Enabled = False

                ' Get Slope
                Select Case (triggerSlopeComboBox.SelectedItem.ToString())
                    Case "Rising"
                        triggerSlope = AnalogEdgeStartTriggerSlope.Rising
                    Case Else
                        triggerSlope = AnalogEdgeStartTriggerSlope.Falling
                End Select

                ' Get Sensitivity Units
                Select Case (sensitivityUnitComboBox.SelectedItem.ToString())
                    Case "mVolts/G" ' Units mVolts/G
                        sensitivityUnits = AIAccelerometerSensitivityUnits.MillivoltsPerG
                    Case Else ' Units Volts/G
                        sensitivityUnits = AIAccelerometerSensitivityUnits.VoltsPerG
                End Select

                ' Get Terminal Configuration
                Select Case terminalConfigurationComboBox.SelectedItem.ToString()
                    Case "Rse"
                        terminalConfiguration = AITerminalConfiguration.Rse
                    Case "Nrse"
                        terminalConfiguration = AITerminalConfiguration.Nrse
                    Case "Pseudodifferential"
                        terminalConfiguration = AITerminalConfiguration.Pseudodifferential
                    Case "Let NI-DAQ Choose"
                        terminalConfiguration = CType(-1, AITerminalConfiguration)
                    Case Else
                        terminalConfiguration = AITerminalConfiguration.Differential
                End Select

                ' Get Ex Source
                Select Case excitationSourceComboBox.SelectedItem.ToString()
                    Case "Internal"
                        excitationSource = AIExcitationSource.Internal
                    Case "External"
                        excitationSource = AIExcitationSource.External
                    Case Else
                        excitationSource = AIExcitationSource.None
                End Select

                ' Get Input Coupling
                Select Case inputCouplingComboBox.SelectedItem.ToString()
                    Case "DC"
                        inputCoupling = AICoupling.DC
                    Case "Gnd"
                        inputCoupling = AICoupling.Ground
                    Case Else
                        inputCoupling = AICoupling.AC
                End Select

                ' Create a new task
                myTask = New Task()
                Dim accelerometerChannel As AIChannel

                ' Create a virtual channel
                accelerometerChannel = myTask.AIChannels.CreateAccelerometerChannel(physicalChannelComboBox.Text, "", terminalConfiguration, _
                    Convert.ToDouble(minimumValueNumeric.Value), Convert.ToDouble(maximumValueNumeric.Value), _
                    Convert.ToDouble(sensitivityNumeric.Value), sensitivityUnits, excitationSource, _
                    Convert.ToDouble(excitationValueNumeric.Value), AIAccelerationUnits.G)

                ' Configure input coupling
                accelerometerChannel.Coupling = inputCoupling

                ' Configure the Analog Trigger
                myTask.Triggers.StartTrigger.ConfigureAnalogEdgeTrigger(triggerSourceTextBox.Text, triggerSlope, _
                    Convert.ToDouble(triggerLevelNumeric.Value))

                myTask.Triggers.StartTrigger.AnalogEdge.Hysteresis = Convert.ToDouble(hysteresisNumeric.Value)

                ' Configure the timing parameters
                myTask.Timing.ConfigureSampleClock("", Convert.ToDouble(rateNumeric.Value), _
                    SampleClockActiveEdge.Rising, SampleQuantityMode.ContinuousSamples, 1000)

                ' Verify the Task
                myTask.Control(TaskAction.Verify)

                ' Prepare the table for Data
                InitializeDataTable(myTask.AIChannels, dataTable)
                acquisitionDataGrid.DataSource = dataTable

                runningTask = myTask
                analogInReader = New AnalogMultiChannelReader(myTask.Stream)
                analogCallback = New AsyncCallback(AddressOf AnalogInCallback)

                ' Use SynchronizeCallbacks to specify that the object 
                ' marshals callbacks across threads appropriately.
                analogInReader.SynchronizeCallbacks = True
                analogInReader.BeginReadWaveform(Convert.ToInt32(samplesPerChannelNumeric.Value), analogCallback, myTask)

            Catch exception As DaqException
                ' Display Errors
                MessageBox.Show(exception.Message)
                runningTask = Nothing
                myTask.Dispose()
                stopButton.Enabled = False
                startButton.Enabled = True
            End Try
        End If
    End Sub

    Private Sub stopButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles stopButton.Click
        ' Dispose of the task
        runningTask = Nothing
        myTask.Dispose()
        stopButton.Enabled = False
        startButton.Enabled = True
    End Sub
    Private Sub AnalogInCallback(ByVal ar As IAsyncResult)
        Try
            If (Not (runningTask Is Nothing)) AndAlso runningTask Is ar.AsyncState Then

                data = analogInReader.EndReadWaveform(ar)

                'Plot your data here
                dataToDataTable(data, dataTable)

                ' Check for and report any overloaded channels
                If overloadDetectionCheckBox.Checked Then
                    ReportOverloadedChannels()
                End If

                analogInReader.BeginMemoryOptimizedReadWaveform(Convert.ToInt32(samplesPerChannelNumeric.Value), analogCallback, myTask, data)

            End If
        Catch ex As DaqException
            MessageBox.Show(ex.Message)
            runningTask = Nothing
            myTask.Dispose()
            stopButton.Enabled = False
            startButton.Enabled = True
        End Try

    End Sub

    Private Sub ReportOverloadedChannels()
        ' Check for and report any overloaded channels
        Dim overloaded As Boolean = myTask.Stream.OverloadedInputChannelsExist
        overloadedChannelsListBox.Items.Clear()
        If overloaded Then
            Dim chans As String() = myTask.Stream.OverloadedInputChannels
            overloadedChannelsListBox.Items.AddRange(chans)
        End If
        If overloaded Then
            overloadDetectedTextBox.Text = "Yes"
        Else
            overloadDetectedTextBox.Text = "No"
        End If
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

    Public Sub InitializeDataTable(ByVal channelCollection As AIChannelCollection, ByRef data As DataTable)
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
