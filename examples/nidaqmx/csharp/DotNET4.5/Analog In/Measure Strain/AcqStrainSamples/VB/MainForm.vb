'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   AcqStrainSamples
'
' Category:
'   AI
'
' Description:
'   This example demonstrates how to perform a strain measurement.
'
' Instructions for running:
'   1.  Enter the list of physical channels, and set the attributes of the
'       strain configuration for these channels.  The maximum and minimum value
'       inputs specify the range that you expect for your measurement.
'   2.  Select the Filter Setting to use. Default means that for a given device
'       its default filter setting and cutoff frequency will be used. Yes will
'       explicitly turn on the filter for a given device and No will explicitly
'       turn off the filter for a given device.
'   3.  Make sure all of the strain gages are in their relaxed state.
'
' Steps:
'   1.  Create a new Task object.  Create a AIChannel object by using the
'       CreateStrainGageChannel method.
'   2.  Set timing parameters by calling the Timing.ConfigureSampleClock method.
'   3.  Set filter parameters for the channel.
'   4.  Call Task.Start() to start the acquisition.
'   5.  Read the data in a loop until the user presses Stop or an error occurs.
'   6.  Dispose the Task object to clean-up any resources associated with the
'       task.
'   7.  Handle any DaqExceptions, if they occur.
'
' I/O Connections Overview:
'   Make sure your signal input terminal matches the physical channel control.
'   In the default case (differential channel ai0) wire the positive lead for
'   your signal to the ACH0 pin on your DAQ device and wire the negative lead
'   for your signal to the ACH8 pin on you DAQ device.  For more information on
'   the input and output terminals for your device, open the NI-DAQmx Help, and
'   refer to the NI-DAQmx Device Terminals and Device Considerations books in
'   the table of contents.
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
    Private strainGageConfiguration As AIStrainGageConfiguration
    Private excitationSource As AIExcitationSource
    Private myAIChannelReader As AnalogSingleChannelReader
    Friend WithEvents acquisitionResultGroupBox As System.Windows.Forms.GroupBox
    Private myAIChannel As AIChannel
    Friend WithEvents acquisitionDataGrid As System.Windows.Forms.DataGrid
    Private dataTable As DataTable


#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()
        Application.EnableVisualStyles()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        dataTable = New DataTable()
        filterEnabledComboBox.SelectedIndex = 1
        excitationSourceComboBox.SelectedIndex = 0
        strainConfigurationComboBox.SelectedIndex = 0
        filterEnabledComboBox.SelectedIndex = 1

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
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents readTimer As System.Windows.Forms.Timer
    Friend WithEvents stopButton As System.Windows.Forms.Button
    Friend WithEvents startButton As System.Windows.Forms.Button
    Friend WithEvents bridgeParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents leadWireResistanceNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents initialBridgeVoltageNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents excitationValueNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents excitationSourceComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents strainConfigurationComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents excitationSourceLabel As System.Windows.Forms.Label
    Friend WithEvents leadWireResistanceLabel As System.Windows.Forms.Label
    Friend WithEvents excitationValueLabel As System.Windows.Forms.Label
    Friend WithEvents initialBridgeVoltageLabel As System.Windows.Forms.Label
    Friend WithEvents strainConfigurationLabel As System.Windows.Forms.Label
    Friend WithEvents strainGageParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents gageFactorNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents poissonRatioNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents nominalGageResistanceLabel As System.Windows.Forms.Label
    Friend WithEvents poissonRatioLabel As System.Windows.Forms.Label
    Friend WithEvents gageFactorLabel As System.Windows.Forms.Label
    Friend WithEvents gageResistanceNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents timingParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents rateNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents rateLabel As System.Windows.Forms.Label
    Friend WithEvents channelParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents cutoffFrequencyNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents filterEnabledLabel As System.Windows.Forms.Label
    Friend WithEvents filterEnabledComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents lowPassCutOffLabel As System.Windows.Forms.Label
    Friend WithEvents minimumValueLabel As System.Windows.Forms.Label
    Friend WithEvents maximumValueLabel As System.Windows.Forms.Label
    Friend WithEvents physicalChannelLabel As System.Windows.Forms.Label
    Friend WithEvents minimumValueNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents maximumValueNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents bridgeNull As System.Windows.Forms.CheckBox
    Friend WithEvents physicalChannelComboBox As System.Windows.Forms.ComboBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.readTimer = New System.Windows.Forms.Timer(Me.components)
        Me.stopButton = New System.Windows.Forms.Button
        Me.startButton = New System.Windows.Forms.Button
        Me.bridgeParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.leadWireResistanceNumeric = New System.Windows.Forms.NumericUpDown
        Me.initialBridgeVoltageNumeric = New System.Windows.Forms.NumericUpDown
        Me.excitationValueNumeric = New System.Windows.Forms.NumericUpDown
        Me.excitationSourceComboBox = New System.Windows.Forms.ComboBox
        Me.strainConfigurationComboBox = New System.Windows.Forms.ComboBox
        Me.excitationSourceLabel = New System.Windows.Forms.Label
        Me.leadWireResistanceLabel = New System.Windows.Forms.Label
        Me.excitationValueLabel = New System.Windows.Forms.Label
        Me.initialBridgeVoltageLabel = New System.Windows.Forms.Label
        Me.strainConfigurationLabel = New System.Windows.Forms.Label
        Me.strainGageParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.gageFactorNumeric = New System.Windows.Forms.NumericUpDown
        Me.poissonRatioNumeric = New System.Windows.Forms.NumericUpDown
        Me.nominalGageResistanceLabel = New System.Windows.Forms.Label
        Me.poissonRatioLabel = New System.Windows.Forms.Label
        Me.gageFactorLabel = New System.Windows.Forms.Label
        Me.gageResistanceNumeric = New System.Windows.Forms.NumericUpDown
        Me.timingParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.rateNumeric = New System.Windows.Forms.NumericUpDown
        Me.rateLabel = New System.Windows.Forms.Label
        Me.channelParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.physicalChannelComboBox = New System.Windows.Forms.ComboBox
        Me.minimumValueNumeric = New System.Windows.Forms.NumericUpDown
        Me.maximumValueNumeric = New System.Windows.Forms.NumericUpDown
        Me.cutoffFrequencyNumeric = New System.Windows.Forms.NumericUpDown
        Me.filterEnabledLabel = New System.Windows.Forms.Label
        Me.filterEnabledComboBox = New System.Windows.Forms.ComboBox
        Me.lowPassCutOffLabel = New System.Windows.Forms.Label
        Me.minimumValueLabel = New System.Windows.Forms.Label
        Me.maximumValueLabel = New System.Windows.Forms.Label
        Me.physicalChannelLabel = New System.Windows.Forms.Label
        Me.bridgeNull = New System.Windows.Forms.CheckBox
        Me.acquisitionResultGroupBox = New System.Windows.Forms.GroupBox
        Me.acquisitionDataGrid = New System.Windows.Forms.DataGrid
        Me.bridgeParametersGroupBox.SuspendLayout()
        CType(Me.leadWireResistanceNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.initialBridgeVoltageNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.excitationValueNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.strainGageParametersGroupBox.SuspendLayout()
        CType(Me.gageFactorNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.poissonRatioNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gageResistanceNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.timingParametersGroupBox.SuspendLayout()
        CType(Me.rateNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.channelParametersGroupBox.SuspendLayout()
        CType(Me.minimumValueNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.maximumValueNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cutoffFrequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.acquisitionResultGroupBox.SuspendLayout()
        CType(Me.acquisitionDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'readTimer
        '
        '
        'stopButton
        '
        Me.stopButton.Enabled = False
        Me.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.stopButton.Location = New System.Drawing.Point(372, 152)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(64, 24)
        Me.stopButton.TabIndex = 1
        Me.stopButton.Text = "Stop"
        '
        'startButton
        '
        Me.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.startButton.Location = New System.Drawing.Point(372, 120)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(64, 24)
        Me.startButton.TabIndex = 0
        Me.startButton.Text = "Start"
        '
        'bridgeParametersGroupBox
        '
        Me.bridgeParametersGroupBox.Controls.Add(Me.leadWireResistanceNumeric)
        Me.bridgeParametersGroupBox.Controls.Add(Me.initialBridgeVoltageNumeric)
        Me.bridgeParametersGroupBox.Controls.Add(Me.excitationValueNumeric)
        Me.bridgeParametersGroupBox.Controls.Add(Me.excitationSourceComboBox)
        Me.bridgeParametersGroupBox.Controls.Add(Me.strainConfigurationComboBox)
        Me.bridgeParametersGroupBox.Controls.Add(Me.excitationSourceLabel)
        Me.bridgeParametersGroupBox.Controls.Add(Me.leadWireResistanceLabel)
        Me.bridgeParametersGroupBox.Controls.Add(Me.excitationValueLabel)
        Me.bridgeParametersGroupBox.Controls.Add(Me.initialBridgeVoltageLabel)
        Me.bridgeParametersGroupBox.Controls.Add(Me.strainConfigurationLabel)
        Me.bridgeParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.bridgeParametersGroupBox.Location = New System.Drawing.Point(184, 192)
        Me.bridgeParametersGroupBox.Name = "bridgeParametersGroupBox"
        Me.bridgeParametersGroupBox.Size = New System.Drawing.Size(272, 168)
        Me.bridgeParametersGroupBox.TabIndex = 5
        Me.bridgeParametersGroupBox.TabStop = False
        Me.bridgeParametersGroupBox.Text = "Bridge Parameters"
        '
        'leadWireResistanceNumeric
        '
        Me.leadWireResistanceNumeric.DecimalPlaces = 2
        Me.leadWireResistanceNumeric.Location = New System.Drawing.Point(144, 40)
        Me.leadWireResistanceNumeric.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.leadWireResistanceNumeric.Name = "leadWireResistanceNumeric"
        Me.leadWireResistanceNumeric.Size = New System.Drawing.Size(112, 20)
        Me.leadWireResistanceNumeric.TabIndex = 7
        '
        'initialBridgeVoltageNumeric
        '
        Me.initialBridgeVoltageNumeric.DecimalPlaces = 2
        Me.initialBridgeVoltageNumeric.Location = New System.Drawing.Point(16, 88)
        Me.initialBridgeVoltageNumeric.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.initialBridgeVoltageNumeric.Name = "initialBridgeVoltageNumeric"
        Me.initialBridgeVoltageNumeric.Size = New System.Drawing.Size(112, 20)
        Me.initialBridgeVoltageNumeric.TabIndex = 3
        '
        'excitationValueNumeric
        '
        Me.excitationValueNumeric.DecimalPlaces = 2
        Me.excitationValueNumeric.Location = New System.Drawing.Point(16, 136)
        Me.excitationValueNumeric.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.excitationValueNumeric.Name = "excitationValueNumeric"
        Me.excitationValueNumeric.Size = New System.Drawing.Size(112, 20)
        Me.excitationValueNumeric.TabIndex = 5
        Me.excitationValueNumeric.Value = New Decimal(New Integer() {25, 0, 0, 65536})
        '
        'excitationSourceComboBox
        '
        Me.excitationSourceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.excitationSourceComboBox.Items.AddRange(New Object() {"Internal", "External", "None"})
        Me.excitationSourceComboBox.Location = New System.Drawing.Point(144, 88)
        Me.excitationSourceComboBox.Name = "excitationSourceComboBox"
        Me.excitationSourceComboBox.Size = New System.Drawing.Size(112, 21)
        Me.excitationSourceComboBox.TabIndex = 9
        '
        'strainConfigurationComboBox
        '
        Me.strainConfigurationComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.strainConfigurationComboBox.Items.AddRange(New Object() {"Full Bridge I", "Full Bridge II", "Full Bridge III", "Half Bridge I", "Half Bridge II", "Quarter Bridge I", "Quarter Bridge II"})
        Me.strainConfigurationComboBox.Location = New System.Drawing.Point(16, 40)
        Me.strainConfigurationComboBox.Name = "strainConfigurationComboBox"
        Me.strainConfigurationComboBox.Size = New System.Drawing.Size(112, 21)
        Me.strainConfigurationComboBox.TabIndex = 1
        '
        'excitationSourceLabel
        '
        Me.excitationSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.excitationSourceLabel.Location = New System.Drawing.Point(144, 72)
        Me.excitationSourceLabel.Name = "excitationSourceLabel"
        Me.excitationSourceLabel.Size = New System.Drawing.Size(100, 16)
        Me.excitationSourceLabel.TabIndex = 8
        Me.excitationSourceLabel.Text = "Excitation Source:"
        '
        'leadWireResistanceLabel
        '
        Me.leadWireResistanceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.leadWireResistanceLabel.Location = New System.Drawing.Point(144, 24)
        Me.leadWireResistanceLabel.Name = "leadWireResistanceLabel"
        Me.leadWireResistanceLabel.Size = New System.Drawing.Size(120, 16)
        Me.leadWireResistanceLabel.TabIndex = 6
        Me.leadWireResistanceLabel.Text = "Lead Wire Resistance:"
        '
        'excitationValueLabel
        '
        Me.excitationValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.excitationValueLabel.Location = New System.Drawing.Point(16, 120)
        Me.excitationValueLabel.Name = "excitationValueLabel"
        Me.excitationValueLabel.Size = New System.Drawing.Size(104, 16)
        Me.excitationValueLabel.TabIndex = 4
        Me.excitationValueLabel.Text = "Excitation Value:"
        '
        'initialBridgeVoltageLabel
        '
        Me.initialBridgeVoltageLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.initialBridgeVoltageLabel.Location = New System.Drawing.Point(16, 72)
        Me.initialBridgeVoltageLabel.Name = "initialBridgeVoltageLabel"
        Me.initialBridgeVoltageLabel.Size = New System.Drawing.Size(120, 16)
        Me.initialBridgeVoltageLabel.TabIndex = 2
        Me.initialBridgeVoltageLabel.Text = "Initial Bridge Voltage:"
        '
        'strainConfigurationLabel
        '
        Me.strainConfigurationLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.strainConfigurationLabel.Location = New System.Drawing.Point(16, 24)
        Me.strainConfigurationLabel.Name = "strainConfigurationLabel"
        Me.strainConfigurationLabel.Size = New System.Drawing.Size(112, 16)
        Me.strainConfigurationLabel.TabIndex = 0
        Me.strainConfigurationLabel.Text = "Strain Configuration:"
        '
        'strainGageParametersGroupBox
        '
        Me.strainGageParametersGroupBox.Controls.Add(Me.gageFactorNumeric)
        Me.strainGageParametersGroupBox.Controls.Add(Me.poissonRatioNumeric)
        Me.strainGageParametersGroupBox.Controls.Add(Me.nominalGageResistanceLabel)
        Me.strainGageParametersGroupBox.Controls.Add(Me.poissonRatioLabel)
        Me.strainGageParametersGroupBox.Controls.Add(Me.gageFactorLabel)
        Me.strainGageParametersGroupBox.Controls.Add(Me.gageResistanceNumeric)
        Me.strainGageParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.strainGageParametersGroupBox.Location = New System.Drawing.Point(184, 8)
        Me.strainGageParametersGroupBox.Name = "strainGageParametersGroupBox"
        Me.strainGageParametersGroupBox.Size = New System.Drawing.Size(168, 168)
        Me.strainGageParametersGroupBox.TabIndex = 4
        Me.strainGageParametersGroupBox.TabStop = False
        Me.strainGageParametersGroupBox.Text = "Strain Gauge Parameters"
        '
        'gageFactorNumeric
        '
        Me.gageFactorNumeric.DecimalPlaces = 2
        Me.gageFactorNumeric.Location = New System.Drawing.Point(16, 40)
        Me.gageFactorNumeric.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.gageFactorNumeric.Name = "gageFactorNumeric"
        Me.gageFactorNumeric.Size = New System.Drawing.Size(112, 20)
        Me.gageFactorNumeric.TabIndex = 1
        Me.gageFactorNumeric.Value = New Decimal(New Integer() {20, 0, 0, 65536})
        '
        'poissonRatioNumeric
        '
        Me.poissonRatioNumeric.DecimalPlaces = 2
        Me.poissonRatioNumeric.Location = New System.Drawing.Point(16, 88)
        Me.poissonRatioNumeric.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.poissonRatioNumeric.Name = "poissonRatioNumeric"
        Me.poissonRatioNumeric.Size = New System.Drawing.Size(112, 20)
        Me.poissonRatioNumeric.TabIndex = 3
        Me.poissonRatioNumeric.Value = New Decimal(New Integer() {30, 0, 0, 131072})
        '
        'nominalGageResistanceLabel
        '
        Me.nominalGageResistanceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.nominalGageResistanceLabel.Location = New System.Drawing.Point(16, 120)
        Me.nominalGageResistanceLabel.Name = "nominalGageResistanceLabel"
        Me.nominalGageResistanceLabel.Size = New System.Drawing.Size(144, 16)
        Me.nominalGageResistanceLabel.TabIndex = 4
        Me.nominalGageResistanceLabel.Text = "Nominal Gage Resistance:"
        '
        'poissonRatioLabel
        '
        Me.poissonRatioLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.poissonRatioLabel.Location = New System.Drawing.Point(16, 72)
        Me.poissonRatioLabel.Name = "poissonRatioLabel"
        Me.poissonRatioLabel.Size = New System.Drawing.Size(100, 16)
        Me.poissonRatioLabel.TabIndex = 2
        Me.poissonRatioLabel.Text = "Poisson Ratio:"
        '
        'gageFactorLabel
        '
        Me.gageFactorLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.gageFactorLabel.Location = New System.Drawing.Point(16, 24)
        Me.gageFactorLabel.Name = "gageFactorLabel"
        Me.gageFactorLabel.Size = New System.Drawing.Size(100, 16)
        Me.gageFactorLabel.TabIndex = 0
        Me.gageFactorLabel.Text = "Gage Factor:"
        '
        'gageResistanceNumeric
        '
        Me.gageResistanceNumeric.DecimalPlaces = 2
        Me.gageResistanceNumeric.Location = New System.Drawing.Point(16, 136)
        Me.gageResistanceNumeric.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.gageResistanceNumeric.Name = "gageResistanceNumeric"
        Me.gageResistanceNumeric.Size = New System.Drawing.Size(112, 20)
        Me.gageResistanceNumeric.TabIndex = 5
        Me.gageResistanceNumeric.Value = New Decimal(New Integer() {350, 0, 0, 0})
        '
        'timingParametersGroupBox
        '
        Me.timingParametersGroupBox.Controls.Add(Me.rateNumeric)
        Me.timingParametersGroupBox.Controls.Add(Me.rateLabel)
        Me.timingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.timingParametersGroupBox.Location = New System.Drawing.Point(8, 288)
        Me.timingParametersGroupBox.Name = "timingParametersGroupBox"
        Me.timingParametersGroupBox.Size = New System.Drawing.Size(168, 72)
        Me.timingParametersGroupBox.TabIndex = 3
        Me.timingParametersGroupBox.TabStop = False
        Me.timingParametersGroupBox.Text = "Timing Parameters"
        '
        'rateNumeric
        '
        Me.rateNumeric.DecimalPlaces = 2
        Me.rateNumeric.Location = New System.Drawing.Point(16, 40)
        Me.rateNumeric.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.rateNumeric.Name = "rateNumeric"
        Me.rateNumeric.Size = New System.Drawing.Size(112, 20)
        Me.rateNumeric.TabIndex = 1
        Me.rateNumeric.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'rateLabel
        '
        Me.rateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.rateLabel.Location = New System.Drawing.Point(16, 24)
        Me.rateLabel.Name = "rateLabel"
        Me.rateLabel.Size = New System.Drawing.Size(40, 16)
        Me.rateLabel.TabIndex = 0
        Me.rateLabel.Text = "Rate:"
        '
        'channelParametersGroupBox
        '
        Me.channelParametersGroupBox.Controls.Add(Me.physicalChannelComboBox)
        Me.channelParametersGroupBox.Controls.Add(Me.minimumValueNumeric)
        Me.channelParametersGroupBox.Controls.Add(Me.maximumValueNumeric)
        Me.channelParametersGroupBox.Controls.Add(Me.cutoffFrequencyNumeric)
        Me.channelParametersGroupBox.Controls.Add(Me.filterEnabledLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.filterEnabledComboBox)
        Me.channelParametersGroupBox.Controls.Add(Me.lowPassCutOffLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.minimumValueLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.maximumValueLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.physicalChannelLabel)
        Me.channelParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.channelParametersGroupBox.Location = New System.Drawing.Point(8, 8)
        Me.channelParametersGroupBox.Name = "channelParametersGroupBox"
        Me.channelParametersGroupBox.Size = New System.Drawing.Size(168, 272)
        Me.channelParametersGroupBox.TabIndex = 2
        Me.channelParametersGroupBox.TabStop = False
        Me.channelParametersGroupBox.Text = "Channel Parameters"
        '
        'physicalChannelComboBox
        '
        Me.physicalChannelComboBox.Location = New System.Drawing.Point(16, 40)
        Me.physicalChannelComboBox.Name = "physicalChannelComboBox"
        Me.physicalChannelComboBox.Size = New System.Drawing.Size(112, 21)
        Me.physicalChannelComboBox.TabIndex = 1
        '
        'minimumValueNumeric
        '
        Me.minimumValueNumeric.DecimalPlaces = 3
        Me.minimumValueNumeric.Location = New System.Drawing.Point(16, 136)
        Me.minimumValueNumeric.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.minimumValueNumeric.Minimum = New Decimal(New Integer() {10, 0, 0, -2147483648})
        Me.minimumValueNumeric.Name = "minimumValueNumeric"
        Me.minimumValueNumeric.Size = New System.Drawing.Size(112, 20)
        Me.minimumValueNumeric.TabIndex = 5
        Me.minimumValueNumeric.Value = New Decimal(New Integer() {1, 0, 0, -2147287040})
        '
        'maximumValueNumeric
        '
        Me.maximumValueNumeric.DecimalPlaces = 3
        Me.maximumValueNumeric.Location = New System.Drawing.Point(16, 88)
        Me.maximumValueNumeric.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.maximumValueNumeric.Minimum = New Decimal(New Integer() {10, 0, 0, -2147483648})
        Me.maximumValueNumeric.Name = "maximumValueNumeric"
        Me.maximumValueNumeric.Size = New System.Drawing.Size(112, 20)
        Me.maximumValueNumeric.TabIndex = 3
        Me.maximumValueNumeric.Value = New Decimal(New Integer() {1, 0, 0, 196608})
        '
        'cutoffFrequencyNumeric
        '
        Me.cutoffFrequencyNumeric.DecimalPlaces = 2
        Me.cutoffFrequencyNumeric.Location = New System.Drawing.Point(16, 240)
        Me.cutoffFrequencyNumeric.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.cutoffFrequencyNumeric.Name = "cutoffFrequencyNumeric"
        Me.cutoffFrequencyNumeric.Size = New System.Drawing.Size(112, 20)
        Me.cutoffFrequencyNumeric.TabIndex = 9
        Me.cutoffFrequencyNumeric.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        'filterEnabledLabel
        '
        Me.filterEnabledLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.filterEnabledLabel.Location = New System.Drawing.Point(16, 168)
        Me.filterEnabledLabel.Name = "filterEnabledLabel"
        Me.filterEnabledLabel.Size = New System.Drawing.Size(88, 16)
        Me.filterEnabledLabel.TabIndex = 6
        Me.filterEnabledLabel.Text = "Filter Enabled?:"
        '
        'filterEnabledComboBox
        '
        Me.filterEnabledComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.filterEnabledComboBox.Items.AddRange(New Object() {"Default", "Yes", "No"})
        Me.filterEnabledComboBox.Location = New System.Drawing.Point(16, 184)
        Me.filterEnabledComboBox.Name = "filterEnabledComboBox"
        Me.filterEnabledComboBox.Size = New System.Drawing.Size(112, 21)
        Me.filterEnabledComboBox.TabIndex = 7
        '
        'lowPassCutOffLabel
        '
        Me.lowPassCutOffLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.lowPassCutOffLabel.Location = New System.Drawing.Point(16, 216)
        Me.lowPassCutOffLabel.Name = "lowPassCutOffLabel"
        Me.lowPassCutOffLabel.Size = New System.Drawing.Size(144, 16)
        Me.lowPassCutOffLabel.TabIndex = 8
        Me.lowPassCutOffLabel.Text = "Lowpass Cutoff Frequency:"
        '
        'minimumValueLabel
        '
        Me.minimumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.minimumValueLabel.Location = New System.Drawing.Point(16, 120)
        Me.minimumValueLabel.Name = "minimumValueLabel"
        Me.minimumValueLabel.Size = New System.Drawing.Size(100, 16)
        Me.minimumValueLabel.TabIndex = 4
        Me.minimumValueLabel.Text = "Minimum Value:"
        '
        'maximumValueLabel
        '
        Me.maximumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.maximumValueLabel.Location = New System.Drawing.Point(16, 72)
        Me.maximumValueLabel.Name = "maximumValueLabel"
        Me.maximumValueLabel.Size = New System.Drawing.Size(100, 16)
        Me.maximumValueLabel.TabIndex = 2
        Me.maximumValueLabel.Text = "Maximum Value:"
        '
        'physicalChannelLabel
        '
        Me.physicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.physicalChannelLabel.Location = New System.Drawing.Point(16, 24)
        Me.physicalChannelLabel.Name = "physicalChannelLabel"
        Me.physicalChannelLabel.Size = New System.Drawing.Size(100, 16)
        Me.physicalChannelLabel.TabIndex = 0
        Me.physicalChannelLabel.Text = "Physical Channel:"
        '
        'bridgeNull
        '
        Me.bridgeNull.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.bridgeNull.Location = New System.Drawing.Point(360, 16)
        Me.bridgeNull.Name = "bridgeNull"
        Me.bridgeNull.Size = New System.Drawing.Size(104, 32)
        Me.bridgeNull.TabIndex = 6
        Me.bridgeNull.Text = "Perform Bridge Null?"
        '
        'acquisitionResultGroupBox
        '
        Me.acquisitionResultGroupBox.Controls.Add(Me.acquisitionDataGrid)
        Me.acquisitionResultGroupBox.Location = New System.Drawing.Point(466, 8)
        Me.acquisitionResultGroupBox.Name = "acquisitionResultGroupBox"
        Me.acquisitionResultGroupBox.Size = New System.Drawing.Size(166, 352)
        Me.acquisitionResultGroupBox.TabIndex = 9
        Me.acquisitionResultGroupBox.TabStop = False
        Me.acquisitionResultGroupBox.Text = "Acquisition Results"
        '
        'acquisitionDataGrid
        '
        Me.acquisitionDataGrid.DataMember = ""
        Me.acquisitionDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.acquisitionDataGrid.Location = New System.Drawing.Point(6, 24)
        Me.acquisitionDataGrid.Name = "acquisitionDataGrid"
        Me.acquisitionDataGrid.Size = New System.Drawing.Size(154, 322)
        Me.acquisitionDataGrid.TabIndex = 1
        Me.acquisitionDataGrid.TabStop = False
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(636, 400)
        Me.Controls.Add(Me.acquisitionResultGroupBox)
        Me.Controls.Add(Me.bridgeNull)
        Me.Controls.Add(Me.stopButton)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.bridgeParametersGroupBox)
        Me.Controls.Add(Me.strainGageParametersGroupBox)
        Me.Controls.Add(Me.timingParametersGroupBox)
        Me.Controls.Add(Me.channelParametersGroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Acquire Strain Samples"
        Me.bridgeParametersGroupBox.ResumeLayout(False)
        CType(Me.leadWireResistanceNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.initialBridgeVoltageNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.excitationValueNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.strainGageParametersGroupBox.ResumeLayout(False)
        CType(Me.gageFactorNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.poissonRatioNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gageResistanceNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.timingParametersGroupBox.ResumeLayout(False)
        CType(Me.rateNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.channelParametersGroupBox.ResumeLayout(False)
        CType(Me.minimumValueNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.maximumValueNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cutoffFrequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.acquisitionResultGroupBox.ResumeLayout(False)
        CType(Me.acquisitionDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub startButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles startButton.Click

        Try

            Select Case (strainConfigurationComboBox.SelectedItem.ToString())

                Case "Full Bridge I"
                    strainGageConfiguration = AIStrainGageConfiguration.FullBridgeI

                Case "Full Bridge II"
                    strainGageConfiguration = AIStrainGageConfiguration.FullBridgeII

                Case "Full Bridge III"
                    strainGageConfiguration = AIStrainGageConfiguration.FullBridgeIII

                Case "Half Bridge I"
                    strainGageConfiguration = AIStrainGageConfiguration.HalfBridgeI

                Case "Half Bridge II"
                    strainGageConfiguration = AIStrainGageConfiguration.HalfBridgeII

                Case "Quarter Bridge I"
                    strainGageConfiguration = AIStrainGageConfiguration.QuarterBridgeI

                Case "Quarter Bridge II"
                    strainGageConfiguration = AIStrainGageConfiguration.QuarterBridgeII

            End Select

            Select Case (excitationSourceComboBox.SelectedItem.ToString())

                Case "Internal"
                    excitationSource = AIExcitationSource.Internal

                Case "External"
                    excitationSource = AIExcitationSource.External

                Case "None"
                    excitationSource = AIExcitationSource.None

            End Select

            myTask = New Task()

            myAIChannel = myTask.AIChannels.CreateStrainGageChannel(physicalChannelComboBox.Text, _
                "", Convert.ToDouble(minimumValueNumeric.Value), Convert.ToDouble(maximumValueNumeric.Value), _
                strainGageConfiguration, excitationSource, Convert.ToDouble(excitationValueNumeric.Value), _
                Convert.ToDouble(gageFactorNumeric.Value), Convert.ToDouble(initialBridgeVoltageNumeric.Value), _
                Convert.ToDouble(gageResistanceNumeric.Value), Convert.ToDouble(poissonRatioNumeric.Value), _
                Convert.ToDouble(leadWireResistanceNumeric.Value), AIStrainUnits.Strain)

            myTask.Timing.ConfigureSampleClock("", Convert.ToDouble(rateNumeric.Value), _
                SampleClockActiveEdge.Rising, SampleQuantityMode.ContinuousSamples, 1000)

            myTask.Control(TaskAction.Verify)

            Select Case (filterEnabledComboBox.SelectedItem.ToString())
                Case "Yes"
                    myAIChannel.LowpassEnable = True
                    myAIChannel.LowpassCutoffFrequency = Convert.ToDouble(cutoffFrequencyNumeric.Value)

                Case "No"
                    myAIChannel.LowpassEnable = False

                Case "Default"

            End Select

            If bridgeNull.Checked = True Then
                myAIChannel.PerformBridgeOffsetNullingCalibration()
            End If

            myAIChannelReader = New AnalogSingleChannelReader(myTask.Stream)

            myTask.Start()
            startButton.Enabled = False
            stopButton.Enabled = True
            readTimer.Enabled = True

        Catch exception As DaqException
            readTimer.Enabled = False
            MessageBox.Show(exception.Message)
            myTask.Dispose()
            startButton.Enabled = True
            stopButton.Enabled = False
        End Try

    End Sub

    Private Sub stopButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles stopButton.Click
        readTimer.Enabled = False

        While readTimer.Enabled = True
        End While

        myTask.Stop()
        myTask.Dispose()
        startButton.Enabled = True
        stopButton.Enabled = False
    End Sub

    Private Sub readTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles readTimer.Tick
        Try
            InitializeDataTable(dataTable)
            acquisitionDataGrid.DataSource = dataTable

            Dim data As AnalogWaveform(Of Double) = myAIChannelReader.ReadWaveform(-1)

            dataToDataTable(data, dataTable)
        Catch exception As DaqException
            readTimer.Enabled = False
            MessageBox.Show(exception.Message)
            myTask.Dispose()
            startButton.Enabled = True
            stopButton.Enabled = False
        End Try
    End Sub

    Private Sub dataToDataTable(ByVal waveform As AnalogWaveform(Of Double), ByRef dataTable As DataTable)
        Dim dataCount As Integer = 0
        If waveform.Samples.Count < 10 Then
            dataCount = waveform.Samples.Count
        Else
            dataCount = 10
        End If
        For sample As Integer = 0 To (dataCount - 1)
            dataTable.Rows(sample)(0) = waveform.Samples(sample).Value
        Next
    End Sub

    Private Sub InitializeDataTable(ByRef data As DataTable)
        data.Rows.Clear()
        data.Columns.Clear()
        Dim dataColumn As DataColumn = New DataColumn(myTask.AIChannels(0).PhysicalName, System.Type.GetType("System.Double"))
        Dim numOfRows As Integer = 10
        Dim currentIndex As Integer

        data.Columns.Add(dataColumn)

        For currentIndex = 0 To (numOfRows - 1)
            Dim row As Object() = New Object(0) {}
            data.Rows.Add(row)
        Next
    End Sub

    Private Sub filterEnabledComboBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles filterEnabledComboBox.SelectedIndexChanged
        Select Case filterEnabledComboBox.SelectedIndex
            Case 1
                cutoffFrequencyNumeric.Enabled = True
            Case Else
                cutoffFrequencyNumeric.Enabled = False
        End Select
    End Sub
End Class
