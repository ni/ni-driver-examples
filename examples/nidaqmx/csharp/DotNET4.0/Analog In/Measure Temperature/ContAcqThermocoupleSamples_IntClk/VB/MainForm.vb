'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   ContAcqThermocoupleSamples_IntClk
'
' Category:
'   AI
'
' Description:
'   This example demonstrates how to continuously acquire temperature readings
'   from one or more thermocouples.
'
' Instructions for running:
'   1.  Specify the physical channel where you have connected the thermocouple.
'   2.  Enter the minimum and maximum temperature values in degrees C that you
'       expect to measure. A smaller range will allow a more accurate
'       measurement.
'   3.  Enter the scan rate at which you want to run the acquisition.
'   4.  Specify the type of thermocouple you are using.
'   5.  Thermocouple measurements require cold-junction compensation (CJC) to
'       correctly scale them. Specify the source of your cold-junction
'       compensation.
'   6.  If your CJC source is "Internal", skip the rest of the steps.
'   7.  If your CJC source is "Constant Value", specify the value (usually room
'       temperature) in degrees C.
'   8.  If your CJC source is "Channel", specify the CJC Channel name.
'   9.  Specify the appropriate Auto Zero Mode. See your SCXI device's hardware
'       manual to find out if your device supports this attribute. E-Series
'       devices do not support this attribute.
'
' Steps:
'   1.  Create a new Task.  Create a AIChannel object by using the
'       CreateThermocoupleChannel method.
'   2.  Set the AutoZero mode.  This attribute is set to compensate for input
'       offset errors and may not be supported by all devices.
'   3.  Configure the timing parameters by using the Timing.ConfigureSampleClock
'       method.  Use the device's internal clock, continuous mode acquisition,
'       and the sample rate specified by the user.
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
'   Connect your thermocouple to the terminals corresponding to the physical
'   channel value. For more information on the input and output terminals for
'   your device, open the NI-DAQmx Help, and refer to the NI-DAQmx Device
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
    Private myTask As Task
    Private runningTask As Task
    Private data As AnalogWaveform(Of Double)()
    Private analogInReader As AnalogMultiChannelReader
    Private myAsyncCallback As AsyncCallback = New AsyncCallback(AddressOf AnalogInCallback)
    Private dataColumn() As DataColumn = Nothing
    Private dataTable As DataTable = New DataTable

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()
        Application.EnableVisualStyles()

        'This call is required by the Windows Form Designer.
        InitializeComponent()
        cjcSourceComboBox.SelectedIndex = 1
        autoZeroModeComboBox.SelectedIndex = 0
        thermocoupleTypeComboBox.SelectedIndex = 2

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
    Friend WithEvents acquisitionResultsGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents resultLabel As System.Windows.Forms.Label
    Friend WithEvents highAccuracySettingsGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents autoZeroModeLabel As System.Windows.Forms.Label
    Friend WithEvents coldJunctionParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents cjcValueLabel As System.Windows.Forms.Label
    Friend WithEvents cjcChannelLabel As System.Windows.Forms.Label
    Friend WithEvents cjcSourceLabel As System.Windows.Forms.Label
    Friend WithEvents cjcSourceComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents cjcChannelTextBox As System.Windows.Forms.TextBox
    Friend WithEvents thermocoupleParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents thermocoupleTypeComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents thermocoupleTypeLabel As System.Windows.Forms.Label
    Friend WithEvents stopButton As System.Windows.Forms.Button
    Friend WithEvents startButton As System.Windows.Forms.Button
    Friend WithEvents timingParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents rateLabel As System.Windows.Forms.Label
    Friend WithEvents channelParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents maximumLabel As System.Windows.Forms.Label
    Friend WithEvents minimumLabel As System.Windows.Forms.Label
    Friend WithEvents physicalChannelLabel As System.Windows.Forms.Label
    Friend WithEvents autoZeroModeComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents acquisitionDataGrid As System.Windows.Forms.DataGrid
    Friend WithEvents rateNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents cjcValueNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents scxiModuleLabel As System.Windows.Forms.Label
    Friend WithEvents scxiModuleCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents minimumValueNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents maximumValueNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents physicalChannelComboBox As System.Windows.Forms.ComboBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MainForm))
        Me.acquisitionResultsGroupBox = New System.Windows.Forms.GroupBox
        Me.resultLabel = New System.Windows.Forms.Label
        Me.acquisitionDataGrid = New System.Windows.Forms.DataGrid
        Me.highAccuracySettingsGroupBox = New System.Windows.Forms.GroupBox
        Me.autoZeroModeComboBox = New System.Windows.Forms.ComboBox
        Me.scxiModuleLabel = New System.Windows.Forms.Label
        Me.scxiModuleCheckBox = New System.Windows.Forms.CheckBox
        Me.autoZeroModeLabel = New System.Windows.Forms.Label
        Me.coldJunctionParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.cjcValueNumeric = New System.Windows.Forms.NumericUpDown
        Me.cjcValueLabel = New System.Windows.Forms.Label
        Me.cjcChannelLabel = New System.Windows.Forms.Label
        Me.cjcSourceLabel = New System.Windows.Forms.Label
        Me.cjcSourceComboBox = New System.Windows.Forms.ComboBox
        Me.cjcChannelTextBox = New System.Windows.Forms.TextBox
        Me.thermocoupleParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.thermocoupleTypeComboBox = New System.Windows.Forms.ComboBox
        Me.thermocoupleTypeLabel = New System.Windows.Forms.Label
        Me.stopButton = New System.Windows.Forms.Button
        Me.startButton = New System.Windows.Forms.Button
        Me.timingParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.rateNumeric = New System.Windows.Forms.NumericUpDown
        Me.rateLabel = New System.Windows.Forms.Label
        Me.channelParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.physicalChannelComboBox = New System.Windows.Forms.ComboBox
        Me.minimumValueNumeric = New System.Windows.Forms.NumericUpDown
        Me.maximumValueNumeric = New System.Windows.Forms.NumericUpDown
        Me.maximumLabel = New System.Windows.Forms.Label
        Me.minimumLabel = New System.Windows.Forms.Label
        Me.physicalChannelLabel = New System.Windows.Forms.Label
        Me.acquisitionResultsGroupBox.SuspendLayout()
        CType(Me.acquisitionDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.highAccuracySettingsGroupBox.SuspendLayout()
        Me.coldJunctionParametersGroupBox.SuspendLayout()
        CType(Me.cjcValueNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.thermocoupleParametersGroupBox.SuspendLayout()
        Me.timingParametersGroupBox.SuspendLayout()
        CType(Me.rateNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.channelParametersGroupBox.SuspendLayout()
        CType(Me.minimumValueNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.maximumValueNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'acquisitionResultsGroupBox
        '
        Me.acquisitionResultsGroupBox.Controls.Add(Me.resultLabel)
        Me.acquisitionResultsGroupBox.Controls.Add(Me.acquisitionDataGrid)
        Me.acquisitionResultsGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.acquisitionResultsGroupBox.Location = New System.Drawing.Point(256, 112)
        Me.acquisitionResultsGroupBox.Name = "acquisitionResultsGroupBox"
        Me.acquisitionResultsGroupBox.Size = New System.Drawing.Size(280, 248)
        Me.acquisitionResultsGroupBox.TabIndex = 7
        Me.acquisitionResultsGroupBox.TabStop = False
        Me.acquisitionResultsGroupBox.Text = "Acquisition Results"
        '
        'resultLabel
        '
        Me.resultLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.resultLabel.Location = New System.Drawing.Point(16, 24)
        Me.resultLabel.Name = "resultLabel"
        Me.resultLabel.Size = New System.Drawing.Size(96, 16)
        Me.resultLabel.TabIndex = 0
        Me.resultLabel.Text = "Acquisition Data:"
        '
        'acquisitionDataGrid
        '
        Me.acquisitionDataGrid.AllowSorting = False
        Me.acquisitionDataGrid.DataMember = ""
        Me.acquisitionDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.acquisitionDataGrid.Location = New System.Drawing.Point(16, 40)
        Me.acquisitionDataGrid.Name = "acquisitionDataGrid"
        Me.acquisitionDataGrid.ParentRowsVisible = False
        Me.acquisitionDataGrid.ReadOnly = True
        Me.acquisitionDataGrid.Size = New System.Drawing.Size(256, 200)
        Me.acquisitionDataGrid.TabIndex = 1
        Me.acquisitionDataGrid.TabStop = False
        '
        'highAccuracySettingsGroupBox
        '
        Me.highAccuracySettingsGroupBox.Controls.Add(Me.autoZeroModeComboBox)
        Me.highAccuracySettingsGroupBox.Controls.Add(Me.scxiModuleLabel)
        Me.highAccuracySettingsGroupBox.Controls.Add(Me.scxiModuleCheckBox)
        Me.highAccuracySettingsGroupBox.Controls.Add(Me.autoZeroModeLabel)
        Me.highAccuracySettingsGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.highAccuracySettingsGroupBox.Location = New System.Drawing.Point(256, 8)
        Me.highAccuracySettingsGroupBox.Name = "highAccuracySettingsGroupBox"
        Me.highAccuracySettingsGroupBox.Size = New System.Drawing.Size(232, 98)
        Me.highAccuracySettingsGroupBox.TabIndex = 6
        Me.highAccuracySettingsGroupBox.TabStop = False
        Me.highAccuracySettingsGroupBox.Text = "High Accuracy Settings"
        '
        'autoZeroModeComboBox
        '
        Me.autoZeroModeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.autoZeroModeComboBox.Items.AddRange(New Object() {"None", "Once"})
        Me.autoZeroModeComboBox.Location = New System.Drawing.Point(120, 56)
        Me.autoZeroModeComboBox.Name = "autoZeroModeComboBox"
        Me.autoZeroModeComboBox.Size = New System.Drawing.Size(104, 21)
        Me.autoZeroModeComboBox.TabIndex = 3
        '
        'scxiModuleLabel
        '
        Me.scxiModuleLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.scxiModuleLabel.Location = New System.Drawing.Point(16, 28)
        Me.scxiModuleLabel.Name = "scxiModuleLabel"
        Me.scxiModuleLabel.Size = New System.Drawing.Size(100, 16)
        Me.scxiModuleLabel.TabIndex = 0
        Me.scxiModuleLabel.Text = "SCXI Module?:"
        '
        'scxiModuleCheckBox
        '
        Me.scxiModuleCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.scxiModuleCheckBox.Location = New System.Drawing.Point(120, 24)
        Me.scxiModuleCheckBox.Name = "scxiModuleCheckBox"
        Me.scxiModuleCheckBox.Size = New System.Drawing.Size(16, 24)
        Me.scxiModuleCheckBox.TabIndex = 1
        Me.scxiModuleCheckBox.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'autoZeroModeLabel
        '
        Me.autoZeroModeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.autoZeroModeLabel.Location = New System.Drawing.Point(16, 58)
        Me.autoZeroModeLabel.Name = "autoZeroModeLabel"
        Me.autoZeroModeLabel.Size = New System.Drawing.Size(88, 16)
        Me.autoZeroModeLabel.TabIndex = 2
        Me.autoZeroModeLabel.Text = "Auto Zero Mode:"
        '
        'coldJunctionParametersGroupBox
        '
        Me.coldJunctionParametersGroupBox.Controls.Add(Me.cjcValueNumeric)
        Me.coldJunctionParametersGroupBox.Controls.Add(Me.cjcValueLabel)
        Me.coldJunctionParametersGroupBox.Controls.Add(Me.cjcChannelLabel)
        Me.coldJunctionParametersGroupBox.Controls.Add(Me.cjcSourceLabel)
        Me.coldJunctionParametersGroupBox.Controls.Add(Me.cjcSourceComboBox)
        Me.coldJunctionParametersGroupBox.Controls.Add(Me.cjcChannelTextBox)
        Me.coldJunctionParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.coldJunctionParametersGroupBox.Location = New System.Drawing.Point(8, 280)
        Me.coldJunctionParametersGroupBox.Name = "coldJunctionParametersGroupBox"
        Me.coldJunctionParametersGroupBox.Size = New System.Drawing.Size(232, 120)
        Me.coldJunctionParametersGroupBox.TabIndex = 5
        Me.coldJunctionParametersGroupBox.TabStop = False
        Me.coldJunctionParametersGroupBox.Text = "Cold Junction Parameters"
        '
        'cjcValueNumeric
        '
        Me.cjcValueNumeric.DecimalPlaces = 2
        Me.cjcValueNumeric.Location = New System.Drawing.Point(120, 88)
        Me.cjcValueNumeric.Name = "cjcValueNumeric"
        Me.cjcValueNumeric.Size = New System.Drawing.Size(104, 20)
        Me.cjcValueNumeric.TabIndex = 5
        Me.cjcValueNumeric.Value = New Decimal(New Integer() {25, 0, 0, 0})
        '
        'cjcValueLabel
        '
        Me.cjcValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.cjcValueLabel.Location = New System.Drawing.Point(16, 90)
        Me.cjcValueLabel.Name = "cjcValueLabel"
        Me.cjcValueLabel.Size = New System.Drawing.Size(104, 16)
        Me.cjcValueLabel.TabIndex = 4
        Me.cjcValueLabel.Text = "CJC Value (deg C):"
        '
        'cjcChannelLabel
        '
        Me.cjcChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.cjcChannelLabel.Location = New System.Drawing.Point(16, 58)
        Me.cjcChannelLabel.Name = "cjcChannelLabel"
        Me.cjcChannelLabel.Size = New System.Drawing.Size(80, 16)
        Me.cjcChannelLabel.TabIndex = 2
        Me.cjcChannelLabel.Text = "CJC Channel:"
        '
        'cjcSourceLabel
        '
        Me.cjcSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.cjcSourceLabel.Location = New System.Drawing.Point(16, 26)
        Me.cjcSourceLabel.Name = "cjcSourceLabel"
        Me.cjcSourceLabel.Size = New System.Drawing.Size(88, 16)
        Me.cjcSourceLabel.TabIndex = 0
        Me.cjcSourceLabel.Text = "CJC Source:"
        '
        'cjcSourceComboBox
        '
        Me.cjcSourceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cjcSourceComboBox.Items.AddRange(New Object() {"Channel", "Constant Value", "Internal"})
        Me.cjcSourceComboBox.Location = New System.Drawing.Point(120, 24)
        Me.cjcSourceComboBox.Name = "cjcSourceComboBox"
        Me.cjcSourceComboBox.Size = New System.Drawing.Size(104, 21)
        Me.cjcSourceComboBox.TabIndex = 1
        '
        'cjcChannelTextBox
        '
        Me.cjcChannelTextBox.Location = New System.Drawing.Point(120, 56)
        Me.cjcChannelTextBox.Name = "cjcChannelTextBox"
        Me.cjcChannelTextBox.Size = New System.Drawing.Size(104, 20)
        Me.cjcChannelTextBox.TabIndex = 3
        Me.cjcChannelTextBox.Text = ""
        '
        'thermocoupleParametersGroupBox
        '
        Me.thermocoupleParametersGroupBox.Controls.Add(Me.thermocoupleTypeComboBox)
        Me.thermocoupleParametersGroupBox.Controls.Add(Me.thermocoupleTypeLabel)
        Me.thermocoupleParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.thermocoupleParametersGroupBox.Location = New System.Drawing.Point(8, 208)
        Me.thermocoupleParametersGroupBox.Name = "thermocoupleParametersGroupBox"
        Me.thermocoupleParametersGroupBox.Size = New System.Drawing.Size(232, 56)
        Me.thermocoupleParametersGroupBox.TabIndex = 4
        Me.thermocoupleParametersGroupBox.TabStop = False
        Me.thermocoupleParametersGroupBox.Text = "Thermocouple Parameters"
        '
        'thermocoupleTypeComboBox
        '
        Me.thermocoupleTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.thermocoupleTypeComboBox.Items.AddRange(New Object() {"B", "E", "J", "K", "N", "R", "S", "T"})
        Me.thermocoupleTypeComboBox.Location = New System.Drawing.Point(120, 24)
        Me.thermocoupleTypeComboBox.Name = "thermocoupleTypeComboBox"
        Me.thermocoupleTypeComboBox.Size = New System.Drawing.Size(104, 21)
        Me.thermocoupleTypeComboBox.TabIndex = 1
        '
        'thermocoupleTypeLabel
        '
        Me.thermocoupleTypeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.thermocoupleTypeLabel.Location = New System.Drawing.Point(16, 26)
        Me.thermocoupleTypeLabel.Name = "thermocoupleTypeLabel"
        Me.thermocoupleTypeLabel.Size = New System.Drawing.Size(112, 16)
        Me.thermocoupleTypeLabel.TabIndex = 0
        Me.thermocoupleTypeLabel.Text = "Thermocouple Type:"
        '
        'stopButton
        '
        Me.stopButton.Enabled = False
        Me.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.stopButton.Location = New System.Drawing.Point(424, 376)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(80, 24)
        Me.stopButton.TabIndex = 1
        Me.stopButton.Text = "Stop"
        '
        'startButton
        '
        Me.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.startButton.Location = New System.Drawing.Point(288, 376)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(80, 24)
        Me.startButton.TabIndex = 0
        Me.startButton.Text = "Start"
        '
        'timingParametersGroupBox
        '
        Me.timingParametersGroupBox.Controls.Add(Me.rateNumeric)
        Me.timingParametersGroupBox.Controls.Add(Me.rateLabel)
        Me.timingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.timingParametersGroupBox.Location = New System.Drawing.Point(8, 136)
        Me.timingParametersGroupBox.Name = "timingParametersGroupBox"
        Me.timingParametersGroupBox.Size = New System.Drawing.Size(232, 56)
        Me.timingParametersGroupBox.TabIndex = 3
        Me.timingParametersGroupBox.TabStop = False
        Me.timingParametersGroupBox.Text = "Timing Parameters"
        '
        'rateNumeric
        '
        Me.rateNumeric.DecimalPlaces = 2
        Me.rateNumeric.Location = New System.Drawing.Point(120, 24)
        Me.rateNumeric.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.rateNumeric.Name = "rateNumeric"
        Me.rateNumeric.Size = New System.Drawing.Size(104, 20)
        Me.rateNumeric.TabIndex = 1
        Me.rateNumeric.Value = New Decimal(New Integer() {10, 0, 0, 0})
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
        Me.channelParametersGroupBox.Size = New System.Drawing.Size(232, 122)
        Me.channelParametersGroupBox.TabIndex = 2
        Me.channelParametersGroupBox.TabStop = False
        Me.channelParametersGroupBox.Text = "Channel Parameters"
        '
        'physicalChannelComboBox
        '
        Me.physicalChannelComboBox.Location = New System.Drawing.Point(120, 24)
        Me.physicalChannelComboBox.Name = "physicalChannelComboBox"
        Me.physicalChannelComboBox.Size = New System.Drawing.Size(104, 21)
        Me.physicalChannelComboBox.TabIndex = 1
        Me.physicalChannelComboBox.Text = "SC1Mod1/ai0"
        '
        'minimumValueNumeric
        '
        Me.minimumValueNumeric.DecimalPlaces = 2
        Me.minimumValueNumeric.Location = New System.Drawing.Point(120, 88)
        Me.minimumValueNumeric.Maximum = New Decimal(New Integer() {500, 0, 0, 0})
        Me.minimumValueNumeric.Minimum = New Decimal(New Integer() {500, 0, 0, -2147483648})
        Me.minimumValueNumeric.Name = "minimumValueNumeric"
        Me.minimumValueNumeric.Size = New System.Drawing.Size(104, 20)
        Me.minimumValueNumeric.TabIndex = 5
        '
        'maximumValueNumeric
        '
        Me.maximumValueNumeric.DecimalPlaces = 2
        Me.maximumValueNumeric.Location = New System.Drawing.Point(120, 56)
        Me.maximumValueNumeric.Maximum = New Decimal(New Integer() {500, 0, 0, 0})
        Me.maximumValueNumeric.Minimum = New Decimal(New Integer() {500, 0, 0, -2147483648})
        Me.maximumValueNumeric.Name = "maximumValueNumeric"
        Me.maximumValueNumeric.Size = New System.Drawing.Size(104, 20)
        Me.maximumValueNumeric.TabIndex = 3
        Me.maximumValueNumeric.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        'maximumLabel
        '
        Me.maximumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.maximumLabel.Location = New System.Drawing.Point(16, 58)
        Me.maximumLabel.Name = "maximumLabel"
        Me.maximumLabel.Size = New System.Drawing.Size(104, 16)
        Me.maximumLabel.TabIndex = 2
        Me.maximumLabel.Text = "Maximum (deg C):"
        '
        'minimumLabel
        '
        Me.minimumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.minimumLabel.Location = New System.Drawing.Point(16, 90)
        Me.minimumLabel.Name = "minimumLabel"
        Me.minimumLabel.Size = New System.Drawing.Size(96, 16)
        Me.minimumLabel.TabIndex = 4
        Me.minimumLabel.Text = "Minimum (deg C):"
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
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(546, 408)
        Me.Controls.Add(Me.acquisitionResultsGroupBox)
        Me.Controls.Add(Me.highAccuracySettingsGroupBox)
        Me.Controls.Add(Me.coldJunctionParametersGroupBox)
        Me.Controls.Add(Me.thermocoupleParametersGroupBox)
        Me.Controls.Add(Me.stopButton)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.timingParametersGroupBox)
        Me.Controls.Add(Me.channelParametersGroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Continuous Acquisition of Thermocouple Samples - Internal Clock"
        Me.acquisitionResultsGroupBox.ResumeLayout(False)
        CType(Me.acquisitionDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.highAccuracySettingsGroupBox.ResumeLayout(False)
        Me.coldJunctionParametersGroupBox.ResumeLayout(False)
        CType(Me.cjcValueNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.thermocoupleParametersGroupBox.ResumeLayout(False)
        Me.timingParametersGroupBox.ResumeLayout(False)
        CType(Me.rateNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.channelParametersGroupBox.ResumeLayout(False)
        CType(Me.minimumValueNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.maximumValueNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub startButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles startButton.Click

        startButton.Enabled = False
        stopButton.Enabled = True

        Try
            ' Create a new task
            myTask = New Task()
            Dim myChannel As AIChannel = Nothing
            Dim thermocoupleType As AIThermocoupleType
            Dim autoZeroMode As AIAutoZeroMode

            Select Case thermocoupleTypeComboBox.SelectedIndex
                Case 0
                    thermocoupleType = AIThermocoupleType.B
                Case 1
                    thermocoupleType = AIThermocoupleType.E
                Case 2
                    thermocoupleType = AIThermocoupleType.J
                Case 3
                    thermocoupleType = AIThermocoupleType.K
                Case 4
                    thermocoupleType = AIThermocoupleType.N
                Case 5
                    thermocoupleType = AIThermocoupleType.R
                Case 6
                    thermocoupleType = AIThermocoupleType.S
                Case 7
                    thermocoupleType = AIThermocoupleType.T
            End Select

            Select Case cjcSourceComboBox.SelectedIndex
                Case 0 ' channel
                    myChannel = myTask.AIChannels.CreateThermocoupleChannel(physicalChannelComboBox.Text, "", _
                        Convert.ToDouble(minimumValueNumeric.Value), Convert.ToDouble(maximumValueNumeric.Value), _
                        thermocoupleType, AITemperatureUnits.DegreesC, cjcChannelTextBox.Text)

                Case 1 ' constant value
                    myChannel = myTask.AIChannels.CreateThermocoupleChannel(physicalChannelComboBox.Text, "", _
                        Convert.ToDouble(minimumValueNumeric.Value), Convert.ToDouble(maximumValueNumeric.Value), _
                        thermocoupleType, AITemperatureUnits.DegreesC, Convert.ToDouble(cjcValueNumeric.Value))

                Case 2 ' internal
                    myChannel = myTask.AIChannels.CreateThermocoupleChannel(physicalChannelComboBox.Text, "", _
                        Convert.ToDouble(minimumValueNumeric.Value), Convert.ToDouble(maximumValueNumeric.Value), _
                        thermocoupleType, AITemperatureUnits.DegreesC)
            End Select

            If (scxiModuleCheckBox.Checked = True) Then
                Select Case autoZeroModeComboBox.SelectedIndex
                    Case 0
                        autoZeroMode = AIAutoZeroMode.None
                    Case 1
                        autoZeroMode = AIAutoZeroMode.Once
                End Select
                myChannel.AutoZeroMode = autoZeroMode
            End If

            myTask.Timing.ConfigureSampleClock("", Convert.ToDouble(rateNumeric.Value), _
               SampleClockActiveEdge.Rising, SampleQuantityMode.ContinuousSamples, 1000)

            myTask.Control(TaskAction.Verify)
            analogInReader = New AnalogMultiChannelReader(myTask.Stream)
            runningTask = myTask

            InitializeDataTable(myTask.AIChannels, dataTable)
            acquisitionDataGrid.DataSource = dataTable

            ' Use SynchronizeCallbacks to specify that the object 
            ' marshals callbacks across threads appropriately.
            analogInReader.SynchronizeCallbacks = True
            analogInReader.BeginReadWaveform(10, myAsyncCallback, myTask)
        Catch exception As DaqException
            MessageBox.Show(exception.Message)
            myTask.Dispose()
            startButton.Enabled = True
            stopButton.Enabled = False
            runningTask = Nothing
        End Try
    End Sub

    Private Sub stopButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles stopButton.Click
        runningTask = Nothing
        myTask.Dispose()
        stopButton.Enabled = False
        startButton.Enabled = True
    End Sub

    Private Sub AnalogInCallback(ByVal ar As IAsyncResult)
        Try
            If (Not (runningTask Is Nothing)) AndAlso runningTask Is ar.AsyncState Then
                data = analogInReader.EndReadWaveform(ar)
                dataToDataTable(data, dataTable)
                analogInReader.BeginMemoryOptimizedReadWaveform(10, myAsyncCallback, myTask, data)
            End If
        Catch exception As DaqException
            MessageBox.Show(exception.Message)
            myTask.Dispose()
            startButton.Enabled = True
            stopButton.Enabled = False
            runningTask = Nothing
        End Try
    End Sub

    Private Sub cjcSourceComboBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cjcSourceComboBox.SelectedIndexChanged
        Select Case cjcSourceComboBox.SelectedIndex
            Case 0 ' channel
                cjcChannelTextBox.Enabled = True
                cjcValueNumeric.Enabled = False
            Case 1 ' constant
                cjcChannelTextBox.Enabled = False
                cjcValueNumeric.Enabled = True
            Case 2 ' internal
                cjcChannelTextBox.Enabled = False
                cjcValueNumeric.Enabled = False
        End Select
    End Sub

    Private Sub InitializeDataTable(ByVal channelCollection As AIChannelCollection, ByRef data As DataTable)
        If (channelCollection Is Nothing) Then
            End
        End If

        Dim numOfChannels As Int16 = channelCollection.Count
        data.Rows.Clear()
        data.Columns.Clear()
        dataColumn = New DataColumn(numOfChannels) {}
        Dim numOfRows As Integer = 10

        For currentChannelIndex As Integer = 0 To (numOfChannels - 1)

            dataColumn(currentChannelIndex) = New DataColumn
            dataColumn(currentChannelIndex).DataType = Type.GetType("System.Double")
            dataColumn(currentChannelIndex).ColumnName = channelCollection(currentChannelIndex).PhysicalName
        Next

        data.Columns.AddRange(dataColumn)

        For currentDataIndex As Integer = 0 To (numOfRows - 1)
            Dim rowArr() As Object = New Object(numOfChannels - 1) {}
            data.Rows.Add(rowArr)
        Next
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

End Class
