'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   ContForceBridgeSampleswCal
'
' Category:
'   AI
'
' Description:
'   This example performs Wheatstone Bridge measurements with offset nulling if
'   desired.
'
' Instructions for running:
'   1.  Enter the list of physical channels, and set the attributes
'       of the bridge configuration connected to all the channels.
'       The 'Maximum Value' and 'Minimum Value' inputs specify the
'       range, in Custom Scale units, that you expect of your
'       measurements.
'   2.  Make sure your Bridge sensor is in its relaxed state.
'   3.  You may check the 'Perform Bridge Null?' option to automatically
'       null out your offset by performing a hardware nulling
'       operation (if supported by the hardware) followed by a
'       software nulling operation. (NOTE: The software nulling
'       operation will cause a loss in dynamic range while a hardware
'       nulling operation will not cause any loss in the dynamic
'       range).
'   4.  Specify Sensor Scaling Parameters. You can choose a Linear
'       Scale or Map Ranges Scale. The channel Maximum and Minimum
'       values are specified in terms of the scaled units.
'   5.  Run the example and do not disturb your bridge sensor until
'       data starts being plotted.
'
' Steps:
'   1.  Create custom scale.
'   2.  Create a new Task. Create a AIChannel by using the
'           CreateVoltageChannelWithExcitation method.
'   3.  Set the rate for the sample clock by using the
'           Timing.ConfigureSampleClock method. Additionally, define the sample mode
'           to be continuous.
'   4.  If nulling is desired, call the DAQmx Perform Bridge Offset
'           Nulling Calibration function to perform both hardware nulling
'           (if supported) and software nulling.
'   5.  Call AnalogMultiChannelReader.BeginReadWaveform to install a callback
'       and begin the asynchronous read operation.
'   6.  Inside the callback, call AnalogMultiChannelReader.EndReadWaveform to
'       retrieve the data from the read operation.  
'   7.  Call AnalogMultiChannelReader.BeginMemoryOptimizedReadWaveform
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
'   Make sure your signal input terminals match the physical channel text box. 
'   For more information on the input and output terminals for your device, open
'   the NI-DAQmx Help, and refer to the NI-DAQmx Device Terminals and Device
'   Considerations books in the table of contents.
'
' Microsoft Windows Vista User Account Control
'   Running certain applications on Microsoft Windows Vista requires
'   administrator privileges, because the application name contains keywords
'   such as setup, update, or install. To avoid this problem, you must add an
'   additional manifest to the application that specifies the privileges
'   required to run the application. Some Measurement Studio NI-DAQmx examples
'   for Visual Studio include these keywords. Therefore, all examples for Visual
'   Studio are shipped with an additional manifest file that you must embed in
'   the example executable. The manifest file is named
'   [ExampleName].exe.manifest, where [ExampleName] is the NI-provided example
'   name. For information on how to embed the manifest file, refer to http://msdn2.microsoft.com/en-us/library/bb756929.aspx.
'   
'   Note: The manifest file is not provided with examples for Visual Studio .NET
'   2003.
'
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

Imports NationalInstruments
Imports NationalInstruments.DAQmx

Public Class MainForm
    Inherits System.Windows.Forms.Form

    Dim channelDataTable As dataTable
    Dim channelDataColumn As dataColumn()
    Dim myTask As Task
    Dim runningTask As Task
    Dim customScaleName As String = "Acq Wheatstone Bridge Samples Scale"
    Dim customScale As Scale
    Dim reader As AnalogMultiChannelReader
    Dim callback As AsyncCallback
    Private WithEvents shuntCalibrationParametersGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents shuntElementLocationComboBox As System.Windows.Forms.ComboBox
    Private WithEvents shuntElementLocationLabel As System.Windows.Forms.Label
    Private WithEvents shuntResistanceNumericUpDown As System.Windows.Forms.NumericUpDown
    Private WithEvents shuntResistanceLabel As System.Windows.Forms.Label
    Private WithEvents wheatstoneBridgeParametersGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents shuntCalibrationCheckBox As System.Windows.Forms.CheckBox
    Private WithEvents bridgeNullCheckBox As System.Windows.Forms.CheckBox
    Private WithEvents nomGageResNumericUpDown As System.Windows.Forms.NumericUpDown
    Private WithEvents excitationValueNumericUpDown As System.Windows.Forms.NumericUpDown
    Private WithEvents bridgeConfigurationComboBox As System.Windows.Forms.ComboBox
    Private WithEvents bridgeConfigurationLabel As System.Windows.Forms.Label
    Private WithEvents excitationSourceLabel As System.Windows.Forms.Label
    Private WithEvents excitationSourceComboBox As System.Windows.Forms.ComboBox
    Private WithEvents nominalGageResistanceLabel As System.Windows.Forms.Label
    Private WithEvents excitationValueLabel As System.Windows.Forms.Label
    Private WithEvents channelParametersGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents unitsComboBox As System.Windows.Forms.ComboBox
    Private WithEvents unitsLabel As System.Windows.Forms.Label
    Private WithEvents physicalChannelComboBox As System.Windows.Forms.ComboBox
    Private WithEvents minimumValueNumericUpDown As System.Windows.Forms.NumericUpDown
    Private WithEvents maximumValueNumericUpDown As System.Windows.Forms.NumericUpDown
    Private WithEvents minimumValueLabel As System.Windows.Forms.Label
    Private WithEvents maximumValueLabel As System.Windows.Forms.Label
    Private WithEvents physicalChannelLabel As System.Windows.Forms.Label
    Private WithEvents timingParametersGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents rateNumericUpDown As System.Windows.Forms.NumericUpDown
    Private WithEvents samplesPerChannelLabel As System.Windows.Forms.Label
    Private WithEvents rateLabel As System.Windows.Forms.Label
    Private WithEvents samplesPerChannelNumericUpDown As System.Windows.Forms.NumericUpDown
    Private WithEvents sensorScalingParametersGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents physicalUnitsComboBox As System.Windows.Forms.ComboBox
    Private WithEvents electricalUnitsComboBox As System.Windows.Forms.ComboBox
    Private WithEvents physicalUnitsLabel As System.Windows.Forms.Label
    Private WithEvents electricalUnitsLabel As System.Windows.Forms.Label
    Private WithEvents sensorScalingTabControl As System.Windows.Forms.TabControl
    Private WithEvents linearTabPage As System.Windows.Forms.TabPage
    Private WithEvents secondPhysicalValueNumericUpDown As System.Windows.Forms.NumericUpDown
    Private WithEvents firstPhysicalValueNumericUpDown As System.Windows.Forms.NumericUpDown
    Private WithEvents secondPhysicalValueLabel As System.Windows.Forms.Label
    Private WithEvents firstPhysicalValueLabel As System.Windows.Forms.Label
    Private WithEvents secondElectricalValueNumericUpDown As System.Windows.Forms.NumericUpDown
    Private WithEvents firstElectricalValueNumericUpDown As System.Windows.Forms.NumericUpDown
    Private WithEvents secondElectricalValueLabel As System.Windows.Forms.Label
    Private WithEvents firstElectricalValueLabel As System.Windows.Forms.Label
    Private WithEvents tableTabPage As System.Windows.Forms.TabPage
    Private WithEvents tableDataGridView As System.Windows.Forms.DataGridView
    Private WithEvents electricalValuesColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Private WithEvents physicalValuesColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Private WithEvents polynomialTabPage As System.Windows.Forms.TabPage
    Private WithEvents label1 As System.Windows.Forms.Label
    Private WithEvents polynomialDataGrid As System.Windows.Forms.DataGridView
    Private WithEvents coefficientsColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Private WithEvents polynomialRangeGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents maximumNumericUpDown As System.Windows.Forms.NumericUpDown
    Private WithEvents minimumNumericUpDown As System.Windows.Forms.NumericUpDown
    Private WithEvents maximumLabel As System.Windows.Forms.Label
    Private WithEvents minimumLabel As System.Windows.Forms.Label
    Private WithEvents coefficientsDirectionGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents physToElecRadioButton As System.Windows.Forms.RadioButton
    Private WithEvents electToPhysRadioButton As System.Windows.Forms.RadioButton
    Private WithEvents acquisitionResultGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents acquisitionDataGrid As System.Windows.Forms.DataGrid
    Dim waveform As AnalogWaveform(Of Double)()



#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()
        Application.EnableVisualStyles()
        Application.DoEvents()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
            If Not myTask Is Nothing Then
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
    Friend WithEvents stopButton As System.Windows.Forms.Button
    Friend WithEvents startButton As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.stopButton = New System.Windows.Forms.Button
        Me.startButton = New System.Windows.Forms.Button
        Me.shuntCalibrationParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.shuntElementLocationComboBox = New System.Windows.Forms.ComboBox
        Me.shuntElementLocationLabel = New System.Windows.Forms.Label
        Me.shuntResistanceNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.shuntResistanceLabel = New System.Windows.Forms.Label
        Me.wheatstoneBridgeParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.shuntCalibrationCheckBox = New System.Windows.Forms.CheckBox
        Me.bridgeNullCheckBox = New System.Windows.Forms.CheckBox
        Me.nomGageResNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.excitationValueNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.bridgeConfigurationComboBox = New System.Windows.Forms.ComboBox
        Me.bridgeConfigurationLabel = New System.Windows.Forms.Label
        Me.excitationSourceLabel = New System.Windows.Forms.Label
        Me.excitationSourceComboBox = New System.Windows.Forms.ComboBox
        Me.nominalGageResistanceLabel = New System.Windows.Forms.Label
        Me.excitationValueLabel = New System.Windows.Forms.Label
        Me.channelParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.unitsComboBox = New System.Windows.Forms.ComboBox
        Me.unitsLabel = New System.Windows.Forms.Label
        Me.physicalChannelComboBox = New System.Windows.Forms.ComboBox
        Me.minimumValueNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.maximumValueNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.minimumValueLabel = New System.Windows.Forms.Label
        Me.maximumValueLabel = New System.Windows.Forms.Label
        Me.physicalChannelLabel = New System.Windows.Forms.Label
        Me.timingParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.rateNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.samplesPerChannelLabel = New System.Windows.Forms.Label
        Me.rateLabel = New System.Windows.Forms.Label
        Me.samplesPerChannelNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.sensorScalingParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.physicalUnitsComboBox = New System.Windows.Forms.ComboBox
        Me.electricalUnitsComboBox = New System.Windows.Forms.ComboBox
        Me.physicalUnitsLabel = New System.Windows.Forms.Label
        Me.electricalUnitsLabel = New System.Windows.Forms.Label
        Me.sensorScalingTabControl = New System.Windows.Forms.TabControl
        Me.linearTabPage = New System.Windows.Forms.TabPage
        Me.secondPhysicalValueNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.firstPhysicalValueNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.secondPhysicalValueLabel = New System.Windows.Forms.Label
        Me.firstPhysicalValueLabel = New System.Windows.Forms.Label
        Me.secondElectricalValueNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.firstElectricalValueNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.secondElectricalValueLabel = New System.Windows.Forms.Label
        Me.firstElectricalValueLabel = New System.Windows.Forms.Label
        Me.tableTabPage = New System.Windows.Forms.TabPage
        Me.tableDataGridView = New System.Windows.Forms.DataGridView
        Me.electricalValuesColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.physicalValuesColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.polynomialTabPage = New System.Windows.Forms.TabPage
        Me.label1 = New System.Windows.Forms.Label
        Me.polynomialDataGrid = New System.Windows.Forms.DataGridView
        Me.coefficientsColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.polynomialRangeGroupBox = New System.Windows.Forms.GroupBox
        Me.maximumNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.minimumNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.maximumLabel = New System.Windows.Forms.Label
        Me.minimumLabel = New System.Windows.Forms.Label
        Me.coefficientsDirectionGroupBox = New System.Windows.Forms.GroupBox
        Me.physToElecRadioButton = New System.Windows.Forms.RadioButton
        Me.electToPhysRadioButton = New System.Windows.Forms.RadioButton
        Me.acquisitionResultGroupBox = New System.Windows.Forms.GroupBox
        Me.acquisitionDataGrid = New System.Windows.Forms.DataGrid
        Me.shuntCalibrationParametersGroupBox.SuspendLayout()
        CType(Me.shuntResistanceNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.wheatstoneBridgeParametersGroupBox.SuspendLayout()
        CType(Me.nomGageResNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.excitationValueNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.channelParametersGroupBox.SuspendLayout()
        CType(Me.minimumValueNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.maximumValueNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.timingParametersGroupBox.SuspendLayout()
        CType(Me.rateNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.samplesPerChannelNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.sensorScalingParametersGroupBox.SuspendLayout()
        Me.sensorScalingTabControl.SuspendLayout()
        Me.linearTabPage.SuspendLayout()
        CType(Me.secondPhysicalValueNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.firstPhysicalValueNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.secondElectricalValueNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.firstElectricalValueNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tableTabPage.SuspendLayout()
        CType(Me.tableDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.polynomialTabPage.SuspendLayout()
        CType(Me.polynomialDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.polynomialRangeGroupBox.SuspendLayout()
        CType(Me.maximumNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.minimumNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.coefficientsDirectionGroupBox.SuspendLayout()
        Me.acquisitionResultGroupBox.SuspendLayout()
        CType(Me.acquisitionDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'stopButton
        '
        Me.stopButton.Enabled = False
        Me.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.stopButton.Location = New System.Drawing.Point(108, 478)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(88, 24)
        Me.stopButton.TabIndex = 6
        Me.stopButton.Text = "Stop"
        '
        'startButton
        '
        Me.startButton.Enabled = False
        Me.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.startButton.Location = New System.Drawing.Point(12, 478)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(88, 24)
        Me.startButton.TabIndex = 5
        Me.startButton.Text = "Start"
        '
        'shuntCalibrationParametersGroupBox
        '
        Me.shuntCalibrationParametersGroupBox.Controls.Add(Me.shuntElementLocationComboBox)
        Me.shuntCalibrationParametersGroupBox.Controls.Add(Me.shuntElementLocationLabel)
        Me.shuntCalibrationParametersGroupBox.Controls.Add(Me.shuntResistanceNumericUpDown)
        Me.shuntCalibrationParametersGroupBox.Controls.Add(Me.shuntResistanceLabel)
        Me.shuntCalibrationParametersGroupBox.Location = New System.Drawing.Point(12, 392)
        Me.shuntCalibrationParametersGroupBox.Name = "shuntCalibrationParametersGroupBox"
        Me.shuntCalibrationParametersGroupBox.Size = New System.Drawing.Size(280, 78)
        Me.shuntCalibrationParametersGroupBox.TabIndex = 12
        Me.shuntCalibrationParametersGroupBox.TabStop = False
        Me.shuntCalibrationParametersGroupBox.Text = "Shunt Calibration Parameters"
        '
        'shuntElementLocationComboBox
        '
        Me.shuntElementLocationComboBox.FormattingEnabled = True
        Me.shuntElementLocationComboBox.Items.AddRange(New Object() {"None", "R1", "R2", "R3", "R4"})
        Me.shuntElementLocationComboBox.Location = New System.Drawing.Point(135, 46)
        Me.shuntElementLocationComboBox.Name = "shuntElementLocationComboBox"
        Me.shuntElementLocationComboBox.Size = New System.Drawing.Size(138, 21)
        Me.shuntElementLocationComboBox.TabIndex = 3
        '
        'shuntElementLocationLabel
        '
        Me.shuntElementLocationLabel.Location = New System.Drawing.Point(16, 43)
        Me.shuntElementLocationLabel.Name = "shuntElementLocationLabel"
        Me.shuntElementLocationLabel.Size = New System.Drawing.Size(112, 30)
        Me.shuntElementLocationLabel.TabIndex = 2
        Me.shuntElementLocationLabel.Text = "Shunt Element Location:"
        '
        'shuntResistanceNumericUpDown
        '
        Me.shuntResistanceNumericUpDown.DecimalPlaces = 2
        Me.shuntResistanceNumericUpDown.Location = New System.Drawing.Point(136, 20)
        Me.shuntResistanceNumericUpDown.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.shuntResistanceNumericUpDown.Minimum = New Decimal(New Integer() {100000, 0, 0, -2147483648})
        Me.shuntResistanceNumericUpDown.Name = "shuntResistanceNumericUpDown"
        Me.shuntResistanceNumericUpDown.Size = New System.Drawing.Size(137, 20)
        Me.shuntResistanceNumericUpDown.TabIndex = 1
        Me.shuntResistanceNumericUpDown.Value = New Decimal(New Integer() {10000000, 0, 0, 131072})
        '
        'shuntResistanceLabel
        '
        Me.shuntResistanceLabel.AutoSize = True
        Me.shuntResistanceLabel.Location = New System.Drawing.Point(16, 22)
        Me.shuntResistanceLabel.Name = "shuntResistanceLabel"
        Me.shuntResistanceLabel.Size = New System.Drawing.Size(94, 13)
        Me.shuntResistanceLabel.TabIndex = 0
        Me.shuntResistanceLabel.Text = "Shunt Resistance:"
        '
        'wheatstoneBridgeParametersGroupBox
        '
        Me.wheatstoneBridgeParametersGroupBox.Controls.Add(Me.shuntCalibrationCheckBox)
        Me.wheatstoneBridgeParametersGroupBox.Controls.Add(Me.bridgeNullCheckBox)
        Me.wheatstoneBridgeParametersGroupBox.Controls.Add(Me.nomGageResNumericUpDown)
        Me.wheatstoneBridgeParametersGroupBox.Controls.Add(Me.excitationValueNumericUpDown)
        Me.wheatstoneBridgeParametersGroupBox.Controls.Add(Me.bridgeConfigurationComboBox)
        Me.wheatstoneBridgeParametersGroupBox.Controls.Add(Me.bridgeConfigurationLabel)
        Me.wheatstoneBridgeParametersGroupBox.Controls.Add(Me.excitationSourceLabel)
        Me.wheatstoneBridgeParametersGroupBox.Controls.Add(Me.excitationSourceComboBox)
        Me.wheatstoneBridgeParametersGroupBox.Controls.Add(Me.nominalGageResistanceLabel)
        Me.wheatstoneBridgeParametersGroupBox.Controls.Add(Me.excitationValueLabel)
        Me.wheatstoneBridgeParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.wheatstoneBridgeParametersGroupBox.Location = New System.Drawing.Point(12, 230)
        Me.wheatstoneBridgeParametersGroupBox.Name = "wheatstoneBridgeParametersGroupBox"
        Me.wheatstoneBridgeParametersGroupBox.Size = New System.Drawing.Size(280, 156)
        Me.wheatstoneBridgeParametersGroupBox.TabIndex = 11
        Me.wheatstoneBridgeParametersGroupBox.TabStop = False
        Me.wheatstoneBridgeParametersGroupBox.Text = "Wheatstone Bridge Parameters"
        '
        'shuntCalibrationCheckBox
        '
        Me.shuntCalibrationCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.shuntCalibrationCheckBox.Location = New System.Drawing.Point(135, 128)
        Me.shuntCalibrationCheckBox.Name = "shuntCalibrationCheckBox"
        Me.shuntCalibrationCheckBox.Size = New System.Drawing.Size(128, 24)
        Me.shuntCalibrationCheckBox.TabIndex = 8
        Me.shuntCalibrationCheckBox.Text = "Do Shunt Calibration?"
        '
        'bridgeNullCheckBox
        '
        Me.bridgeNullCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.bridgeNullCheckBox.Location = New System.Drawing.Point(18, 128)
        Me.bridgeNullCheckBox.Name = "bridgeNullCheckBox"
        Me.bridgeNullCheckBox.Size = New System.Drawing.Size(128, 24)
        Me.bridgeNullCheckBox.TabIndex = 7
        Me.bridgeNullCheckBox.Text = "Do Bridge Null?"
        '
        'nomGageResNumericUpDown
        '
        Me.nomGageResNumericUpDown.DecimalPlaces = 2
        Me.nomGageResNumericUpDown.Location = New System.Drawing.Point(136, 99)
        Me.nomGageResNumericUpDown.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.nomGageResNumericUpDown.Minimum = New Decimal(New Integer() {100000, 0, 0, -2147483648})
        Me.nomGageResNumericUpDown.Name = "nomGageResNumericUpDown"
        Me.nomGageResNumericUpDown.Size = New System.Drawing.Size(137, 20)
        Me.nomGageResNumericUpDown.TabIndex = 6
        Me.nomGageResNumericUpDown.Value = New Decimal(New Integer() {350, 0, 0, 0})
        '
        'excitationValueNumericUpDown
        '
        Me.excitationValueNumericUpDown.DecimalPlaces = 2
        Me.excitationValueNumericUpDown.Location = New System.Drawing.Point(136, 73)
        Me.excitationValueNumericUpDown.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.excitationValueNumericUpDown.Minimum = New Decimal(New Integer() {100000, 0, 0, -2147483648})
        Me.excitationValueNumericUpDown.Name = "excitationValueNumericUpDown"
        Me.excitationValueNumericUpDown.Size = New System.Drawing.Size(137, 20)
        Me.excitationValueNumericUpDown.TabIndex = 5
        Me.excitationValueNumericUpDown.Value = New Decimal(New Integer() {25, 0, 0, 65536})
        '
        'bridgeConfigurationComboBox
        '
        Me.bridgeConfigurationComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.bridgeConfigurationComboBox.Items.AddRange(New Object() {"Full Bridge", "Half Bridge", "Quarter Bridge", "No Bridge"})
        Me.bridgeConfigurationComboBox.Location = New System.Drawing.Point(136, 19)
        Me.bridgeConfigurationComboBox.Name = "bridgeConfigurationComboBox"
        Me.bridgeConfigurationComboBox.Size = New System.Drawing.Size(137, 21)
        Me.bridgeConfigurationComboBox.TabIndex = 1
        '
        'bridgeConfigurationLabel
        '
        Me.bridgeConfigurationLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.bridgeConfigurationLabel.Location = New System.Drawing.Point(16, 24)
        Me.bridgeConfigurationLabel.Name = "bridgeConfigurationLabel"
        Me.bridgeConfigurationLabel.Size = New System.Drawing.Size(112, 16)
        Me.bridgeConfigurationLabel.TabIndex = 0
        Me.bridgeConfigurationLabel.Text = "Bridge Configuration:"
        '
        'excitationSourceLabel
        '
        Me.excitationSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.excitationSourceLabel.Location = New System.Drawing.Point(16, 49)
        Me.excitationSourceLabel.Name = "excitationSourceLabel"
        Me.excitationSourceLabel.Size = New System.Drawing.Size(112, 16)
        Me.excitationSourceLabel.TabIndex = 2
        Me.excitationSourceLabel.Text = "Excitation Source:"
        '
        'excitationSourceComboBox
        '
        Me.excitationSourceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.excitationSourceComboBox.Items.AddRange(New Object() {"Internal", "External", "None"})
        Me.excitationSourceComboBox.Location = New System.Drawing.Point(136, 46)
        Me.excitationSourceComboBox.Name = "excitationSourceComboBox"
        Me.excitationSourceComboBox.Size = New System.Drawing.Size(137, 21)
        Me.excitationSourceComboBox.TabIndex = 3
        '
        'nominalGageResistanceLabel
        '
        Me.nominalGageResistanceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.nominalGageResistanceLabel.Location = New System.Drawing.Point(16, 95)
        Me.nominalGageResistanceLabel.Name = "nominalGageResistanceLabel"
        Me.nominalGageResistanceLabel.Size = New System.Drawing.Size(112, 30)
        Me.nominalGageResistanceLabel.TabIndex = 4
        Me.nominalGageResistanceLabel.Text = "Nominal Gage Resistance:"
        '
        'excitationValueLabel
        '
        Me.excitationValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.excitationValueLabel.Location = New System.Drawing.Point(16, 76)
        Me.excitationValueLabel.Name = "excitationValueLabel"
        Me.excitationValueLabel.Size = New System.Drawing.Size(112, 16)
        Me.excitationValueLabel.TabIndex = 4
        Me.excitationValueLabel.Text = "Excitation Value (V):"
        '
        'channelParametersGroupBox
        '
        Me.channelParametersGroupBox.Controls.Add(Me.unitsComboBox)
        Me.channelParametersGroupBox.Controls.Add(Me.unitsLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.physicalChannelComboBox)
        Me.channelParametersGroupBox.Controls.Add(Me.minimumValueNumericUpDown)
        Me.channelParametersGroupBox.Controls.Add(Me.maximumValueNumericUpDown)
        Me.channelParametersGroupBox.Controls.Add(Me.minimumValueLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.maximumValueLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.physicalChannelLabel)
        Me.channelParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.channelParametersGroupBox.Location = New System.Drawing.Point(12, 12)
        Me.channelParametersGroupBox.Name = "channelParametersGroupBox"
        Me.channelParametersGroupBox.Size = New System.Drawing.Size(280, 128)
        Me.channelParametersGroupBox.TabIndex = 9
        Me.channelParametersGroupBox.TabStop = False
        Me.channelParametersGroupBox.Text = "Channel Parameters"
        '
        'unitsComboBox
        '
        Me.unitsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.unitsComboBox.FormattingEnabled = True
        Me.unitsComboBox.Items.AddRange(New Object() {"Pounds", "kgf", "Newtons"})
        Me.unitsComboBox.Location = New System.Drawing.Point(136, 98)
        Me.unitsComboBox.Name = "unitsComboBox"
        Me.unitsComboBox.Size = New System.Drawing.Size(137, 21)
        Me.unitsComboBox.TabIndex = 7
        '
        'unitsLabel
        '
        Me.unitsLabel.AutoSize = True
        Me.unitsLabel.Location = New System.Drawing.Point(16, 101)
        Me.unitsLabel.Name = "unitsLabel"
        Me.unitsLabel.Size = New System.Drawing.Size(34, 13)
        Me.unitsLabel.TabIndex = 6
        Me.unitsLabel.Text = "Units:"
        '
        'physicalChannelComboBox
        '
        Me.physicalChannelComboBox.Location = New System.Drawing.Point(136, 19)
        Me.physicalChannelComboBox.Name = "physicalChannelComboBox"
        Me.physicalChannelComboBox.Size = New System.Drawing.Size(137, 21)
        Me.physicalChannelComboBox.TabIndex = 1
        '
        'minimumValueNumericUpDown
        '
        Me.minimumValueNumericUpDown.DecimalPlaces = 3
        Me.minimumValueNumericUpDown.Location = New System.Drawing.Point(136, 72)
        Me.minimumValueNumericUpDown.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.minimumValueNumericUpDown.Minimum = New Decimal(New Integer() {10, 0, 0, -2147483648})
        Me.minimumValueNumericUpDown.Name = "minimumValueNumericUpDown"
        Me.minimumValueNumericUpDown.Size = New System.Drawing.Size(137, 20)
        Me.minimumValueNumericUpDown.TabIndex = 5
        Me.minimumValueNumericUpDown.Value = New Decimal(New Integer() {25, 0, 0, -2147287040})
        '
        'maximumValueNumericUpDown
        '
        Me.maximumValueNumericUpDown.DecimalPlaces = 3
        Me.maximumValueNumericUpDown.Location = New System.Drawing.Point(136, 46)
        Me.maximumValueNumericUpDown.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.maximumValueNumericUpDown.Minimum = New Decimal(New Integer() {10, 0, 0, -2147483648})
        Me.maximumValueNumericUpDown.Name = "maximumValueNumericUpDown"
        Me.maximumValueNumericUpDown.Size = New System.Drawing.Size(137, 20)
        Me.maximumValueNumericUpDown.TabIndex = 3
        Me.maximumValueNumericUpDown.Value = New Decimal(New Integer() {25, 0, 0, 196608})
        '
        'minimumValueLabel
        '
        Me.minimumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.minimumValueLabel.Location = New System.Drawing.Point(16, 74)
        Me.minimumValueLabel.Name = "minimumValueLabel"
        Me.minimumValueLabel.Size = New System.Drawing.Size(100, 16)
        Me.minimumValueLabel.TabIndex = 4
        Me.minimumValueLabel.Text = "Minimum Value:"
        '
        'maximumValueLabel
        '
        Me.maximumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.maximumValueLabel.Location = New System.Drawing.Point(16, 48)
        Me.maximumValueLabel.Name = "maximumValueLabel"
        Me.maximumValueLabel.Size = New System.Drawing.Size(100, 16)
        Me.maximumValueLabel.TabIndex = 2
        Me.maximumValueLabel.Text = "Maximum Value:"
        '
        'physicalChannelLabel
        '
        Me.physicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.physicalChannelLabel.Location = New System.Drawing.Point(16, 22)
        Me.physicalChannelLabel.Name = "physicalChannelLabel"
        Me.physicalChannelLabel.Size = New System.Drawing.Size(100, 16)
        Me.physicalChannelLabel.TabIndex = 0
        Me.physicalChannelLabel.Text = "Physical Channel:"
        '
        'timingParametersGroupBox
        '
        Me.timingParametersGroupBox.Controls.Add(Me.rateNumericUpDown)
        Me.timingParametersGroupBox.Controls.Add(Me.samplesPerChannelLabel)
        Me.timingParametersGroupBox.Controls.Add(Me.rateLabel)
        Me.timingParametersGroupBox.Controls.Add(Me.samplesPerChannelNumericUpDown)
        Me.timingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.timingParametersGroupBox.Location = New System.Drawing.Point(12, 148)
        Me.timingParametersGroupBox.Name = "timingParametersGroupBox"
        Me.timingParametersGroupBox.Size = New System.Drawing.Size(280, 76)
        Me.timingParametersGroupBox.TabIndex = 10
        Me.timingParametersGroupBox.TabStop = False
        Me.timingParametersGroupBox.Text = "Timing Parameters"
        '
        'rateNumericUpDown
        '
        Me.rateNumericUpDown.DecimalPlaces = 2
        Me.rateNumericUpDown.Location = New System.Drawing.Point(136, 43)
        Me.rateNumericUpDown.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.rateNumericUpDown.Name = "rateNumericUpDown"
        Me.rateNumericUpDown.Size = New System.Drawing.Size(137, 20)
        Me.rateNumericUpDown.TabIndex = 3
        Me.rateNumericUpDown.Value = New Decimal(New Integer() {5000, 0, 0, 0})
        '
        'samplesPerChannelLabel
        '
        Me.samplesPerChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.samplesPerChannelLabel.Location = New System.Drawing.Point(16, 19)
        Me.samplesPerChannelLabel.Name = "samplesPerChannelLabel"
        Me.samplesPerChannelLabel.Size = New System.Drawing.Size(120, 16)
        Me.samplesPerChannelLabel.TabIndex = 0
        Me.samplesPerChannelLabel.Text = "Samples Per Channel:"
        '
        'rateLabel
        '
        Me.rateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.rateLabel.Location = New System.Drawing.Point(16, 45)
        Me.rateLabel.Name = "rateLabel"
        Me.rateLabel.Size = New System.Drawing.Size(96, 16)
        Me.rateLabel.TabIndex = 2
        Me.rateLabel.Text = "Rate (Hz):"
        '
        'samplesPerChannelNumericUpDown
        '
        Me.samplesPerChannelNumericUpDown.Location = New System.Drawing.Point(136, 17)
        Me.samplesPerChannelNumericUpDown.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.samplesPerChannelNumericUpDown.Name = "samplesPerChannelNumericUpDown"
        Me.samplesPerChannelNumericUpDown.Size = New System.Drawing.Size(137, 20)
        Me.samplesPerChannelNumericUpDown.TabIndex = 1
        Me.samplesPerChannelNumericUpDown.Value = New Decimal(New Integer() {5000, 0, 0, 0})
        '
        'sensorScalingParametersGroupBox
        '
        Me.sensorScalingParametersGroupBox.Controls.Add(Me.physicalUnitsComboBox)
        Me.sensorScalingParametersGroupBox.Controls.Add(Me.electricalUnitsComboBox)
        Me.sensorScalingParametersGroupBox.Controls.Add(Me.physicalUnitsLabel)
        Me.sensorScalingParametersGroupBox.Controls.Add(Me.electricalUnitsLabel)
        Me.sensorScalingParametersGroupBox.Controls.Add(Me.sensorScalingTabControl)
        Me.sensorScalingParametersGroupBox.Location = New System.Drawing.Point(298, 230)
        Me.sensorScalingParametersGroupBox.Name = "sensorScalingParametersGroupBox"
        Me.sensorScalingParametersGroupBox.Size = New System.Drawing.Size(478, 276)
        Me.sensorScalingParametersGroupBox.TabIndex = 14
        Me.sensorScalingParametersGroupBox.TabStop = False
        Me.sensorScalingParametersGroupBox.Text = "Sensor Scaling Parameters"
        '
        'physicalUnitsComboBox
        '
        Me.physicalUnitsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.physicalUnitsComboBox.FormattingEnabled = True
        Me.physicalUnitsComboBox.Items.AddRange(New Object() {"Pounds", "kgf", "Newtons"})
        Me.physicalUnitsComboBox.Location = New System.Drawing.Point(338, 240)
        Me.physicalUnitsComboBox.Name = "physicalUnitsComboBox"
        Me.physicalUnitsComboBox.Size = New System.Drawing.Size(121, 21)
        Me.physicalUnitsComboBox.TabIndex = 4
        '
        'electricalUnitsComboBox
        '
        Me.electricalUnitsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.electricalUnitsComboBox.FormattingEnabled = True
        Me.electricalUnitsComboBox.Items.AddRange(New Object() {"mV/V", "V/V"})
        Me.electricalUnitsComboBox.Location = New System.Drawing.Point(106, 240)
        Me.electricalUnitsComboBox.Name = "electricalUnitsComboBox"
        Me.electricalUnitsComboBox.Size = New System.Drawing.Size(121, 21)
        Me.electricalUnitsComboBox.TabIndex = 3
        '
        'physicalUnitsLabel
        '
        Me.physicalUnitsLabel.AutoSize = True
        Me.physicalUnitsLabel.Location = New System.Drawing.Point(256, 243)
        Me.physicalUnitsLabel.Name = "physicalUnitsLabel"
        Me.physicalUnitsLabel.Size = New System.Drawing.Size(76, 13)
        Me.physicalUnitsLabel.TabIndex = 2
        Me.physicalUnitsLabel.Text = "Physical Units:"
        '
        'electricalUnitsLabel
        '
        Me.electricalUnitsLabel.AutoSize = True
        Me.electricalUnitsLabel.Location = New System.Drawing.Point(20, 243)
        Me.electricalUnitsLabel.Name = "electricalUnitsLabel"
        Me.electricalUnitsLabel.Size = New System.Drawing.Size(80, 13)
        Me.electricalUnitsLabel.TabIndex = 1
        Me.electricalUnitsLabel.Text = "Electrical Units:"
        '
        'sensorScalingTabControl
        '
        Me.sensorScalingTabControl.Controls.Add(Me.linearTabPage)
        Me.sensorScalingTabControl.Controls.Add(Me.tableTabPage)
        Me.sensorScalingTabControl.Controls.Add(Me.polynomialTabPage)
        Me.sensorScalingTabControl.Location = New System.Drawing.Point(8, 19)
        Me.sensorScalingTabControl.Name = "sensorScalingTabControl"
        Me.sensorScalingTabControl.SelectedIndex = 0
        Me.sensorScalingTabControl.Size = New System.Drawing.Size(464, 210)
        Me.sensorScalingTabControl.TabIndex = 0
        '
        'linearTabPage
        '
        Me.linearTabPage.Controls.Add(Me.secondPhysicalValueNumericUpDown)
        Me.linearTabPage.Controls.Add(Me.firstPhysicalValueNumericUpDown)
        Me.linearTabPage.Controls.Add(Me.secondPhysicalValueLabel)
        Me.linearTabPage.Controls.Add(Me.firstPhysicalValueLabel)
        Me.linearTabPage.Controls.Add(Me.secondElectricalValueNumericUpDown)
        Me.linearTabPage.Controls.Add(Me.firstElectricalValueNumericUpDown)
        Me.linearTabPage.Controls.Add(Me.secondElectricalValueLabel)
        Me.linearTabPage.Controls.Add(Me.firstElectricalValueLabel)
        Me.linearTabPage.Location = New System.Drawing.Point(4, 22)
        Me.linearTabPage.Name = "linearTabPage"
        Me.linearTabPage.Padding = New System.Windows.Forms.Padding(3)
        Me.linearTabPage.Size = New System.Drawing.Size(456, 184)
        Me.linearTabPage.TabIndex = 0
        Me.linearTabPage.Text = "Two Point Linear"
        Me.linearTabPage.UseVisualStyleBackColor = True
        '
        'secondPhysicalValueNumericUpDown
        '
        Me.secondPhysicalValueNumericUpDown.DecimalPlaces = 2
        Me.secondPhysicalValueNumericUpDown.Location = New System.Drawing.Point(249, 115)
        Me.secondPhysicalValueNumericUpDown.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.secondPhysicalValueNumericUpDown.Name = "secondPhysicalValueNumericUpDown"
        Me.secondPhysicalValueNumericUpDown.Size = New System.Drawing.Size(120, 20)
        Me.secondPhysicalValueNumericUpDown.TabIndex = 8
        Me.secondPhysicalValueNumericUpDown.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        'firstPhysicalValueNumericUpDown
        '
        Me.firstPhysicalValueNumericUpDown.DecimalPlaces = 2
        Me.firstPhysicalValueNumericUpDown.Location = New System.Drawing.Point(249, 66)
        Me.firstPhysicalValueNumericUpDown.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.firstPhysicalValueNumericUpDown.Name = "firstPhysicalValueNumericUpDown"
        Me.firstPhysicalValueNumericUpDown.Size = New System.Drawing.Size(120, 20)
        Me.firstPhysicalValueNumericUpDown.TabIndex = 7
        '
        'secondPhysicalValueLabel
        '
        Me.secondPhysicalValueLabel.AutoSize = True
        Me.secondPhysicalValueLabel.Location = New System.Drawing.Point(246, 99)
        Me.secondPhysicalValueLabel.Name = "secondPhysicalValueLabel"
        Me.secondPhysicalValueLabel.Size = New System.Drawing.Size(119, 13)
        Me.secondPhysicalValueLabel.TabIndex = 6
        Me.secondPhysicalValueLabel.Text = "Second Physical Value:"
        '
        'firstPhysicalValueLabel
        '
        Me.firstPhysicalValueLabel.AutoSize = True
        Me.firstPhysicalValueLabel.Location = New System.Drawing.Point(246, 50)
        Me.firstPhysicalValueLabel.Name = "firstPhysicalValueLabel"
        Me.firstPhysicalValueLabel.Size = New System.Drawing.Size(101, 13)
        Me.firstPhysicalValueLabel.TabIndex = 5
        Me.firstPhysicalValueLabel.Text = "First Physical Value:"
        '
        'secondElectricalValueNumericUpDown
        '
        Me.secondElectricalValueNumericUpDown.DecimalPlaces = 2
        Me.secondElectricalValueNumericUpDown.Location = New System.Drawing.Point(91, 115)
        Me.secondElectricalValueNumericUpDown.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.secondElectricalValueNumericUpDown.Name = "secondElectricalValueNumericUpDown"
        Me.secondElectricalValueNumericUpDown.Size = New System.Drawing.Size(120, 20)
        Me.secondElectricalValueNumericUpDown.TabIndex = 4
        Me.secondElectricalValueNumericUpDown.Value = New Decimal(New Integer() {2, 0, 0, 0})
        '
        'firstElectricalValueNumericUpDown
        '
        Me.firstElectricalValueNumericUpDown.DecimalPlaces = 2
        Me.firstElectricalValueNumericUpDown.Location = New System.Drawing.Point(91, 66)
        Me.firstElectricalValueNumericUpDown.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.firstElectricalValueNumericUpDown.Name = "firstElectricalValueNumericUpDown"
        Me.firstElectricalValueNumericUpDown.Size = New System.Drawing.Size(120, 20)
        Me.firstElectricalValueNumericUpDown.TabIndex = 3
        '
        'secondElectricalValueLabel
        '
        Me.secondElectricalValueLabel.AutoSize = True
        Me.secondElectricalValueLabel.Location = New System.Drawing.Point(88, 99)
        Me.secondElectricalValueLabel.Name = "secondElectricalValueLabel"
        Me.secondElectricalValueLabel.Size = New System.Drawing.Size(123, 13)
        Me.secondElectricalValueLabel.TabIndex = 1
        Me.secondElectricalValueLabel.Text = "Second Electrical Value:"
        '
        'firstElectricalValueLabel
        '
        Me.firstElectricalValueLabel.AutoSize = True
        Me.firstElectricalValueLabel.Location = New System.Drawing.Point(88, 50)
        Me.firstElectricalValueLabel.Name = "firstElectricalValueLabel"
        Me.firstElectricalValueLabel.Size = New System.Drawing.Size(105, 13)
        Me.firstElectricalValueLabel.TabIndex = 0
        Me.firstElectricalValueLabel.Text = "First Electrical Value:"
        '
        'tableTabPage
        '
        Me.tableTabPage.Controls.Add(Me.tableDataGridView)
        Me.tableTabPage.Location = New System.Drawing.Point(4, 22)
        Me.tableTabPage.Name = "tableTabPage"
        Me.tableTabPage.Padding = New System.Windows.Forms.Padding(3)
        Me.tableTabPage.Size = New System.Drawing.Size(456, 184)
        Me.tableTabPage.TabIndex = 1
        Me.tableTabPage.Text = "Table"
        Me.tableTabPage.UseVisualStyleBackColor = True
        '
        'tableDataGridView
        '
        Me.tableDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.tableDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.electricalValuesColumn, Me.physicalValuesColumn})
        Me.tableDataGridView.Location = New System.Drawing.Point(54, 8)
        Me.tableDataGridView.Name = "tableDataGridView"
        Me.tableDataGridView.Size = New System.Drawing.Size(344, 170)
        Me.tableDataGridView.TabIndex = 0
        '
        'electricalValuesColumn
        '
        Me.electricalValuesColumn.HeaderText = "Electrical Values"
        Me.electricalValuesColumn.Name = "electricalValuesColumn"
        Me.electricalValuesColumn.Width = 150
        '
        'physicalValuesColumn
        '
        Me.physicalValuesColumn.HeaderText = "Physical Values"
        Me.physicalValuesColumn.Name = "physicalValuesColumn"
        Me.physicalValuesColumn.Width = 150
        '
        'polynomialTabPage
        '
        Me.polynomialTabPage.Controls.Add(Me.label1)
        Me.polynomialTabPage.Controls.Add(Me.polynomialDataGrid)
        Me.polynomialTabPage.Controls.Add(Me.polynomialRangeGroupBox)
        Me.polynomialTabPage.Controls.Add(Me.coefficientsDirectionGroupBox)
        Me.polynomialTabPage.Location = New System.Drawing.Point(4, 22)
        Me.polynomialTabPage.Name = "polynomialTabPage"
        Me.polynomialTabPage.Padding = New System.Windows.Forms.Padding(3)
        Me.polynomialTabPage.Size = New System.Drawing.Size(456, 184)
        Me.polynomialTabPage.TabIndex = 2
        Me.polynomialTabPage.Text = "Polynomial"
        Me.polynomialTabPage.UseVisualStyleBackColor = True
        '
        'label1
        '
        Me.label1.Location = New System.Drawing.Point(321, 16)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(109, 68)
        Me.label1.TabIndex = 5
        Me.label1.Text = "Note: The top most element of the coefficients table, is the independent term."
        '
        'polynomialDataGrid
        '
        Me.polynomialDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.polynomialDataGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.coefficientsColumn})
        Me.polynomialDataGrid.Location = New System.Drawing.Point(155, 11)
        Me.polynomialDataGrid.Name = "polynomialDataGrid"
        Me.polynomialDataGrid.Size = New System.Drawing.Size(152, 167)
        Me.polynomialDataGrid.TabIndex = 4
        '
        'coefficientsColumn
        '
        Me.coefficientsColumn.HeaderText = "Coefficients"
        Me.coefficientsColumn.Name = "coefficientsColumn"
        '
        'polynomialRangeGroupBox
        '
        Me.polynomialRangeGroupBox.Controls.Add(Me.maximumNumericUpDown)
        Me.polynomialRangeGroupBox.Controls.Add(Me.minimumNumericUpDown)
        Me.polynomialRangeGroupBox.Controls.Add(Me.maximumLabel)
        Me.polynomialRangeGroupBox.Controls.Add(Me.minimumLabel)
        Me.polynomialRangeGroupBox.Location = New System.Drawing.Point(6, 84)
        Me.polynomialRangeGroupBox.Name = "polynomialRangeGroupBox"
        Me.polynomialRangeGroupBox.Size = New System.Drawing.Size(143, 94)
        Me.polynomialRangeGroupBox.TabIndex = 3
        Me.polynomialRangeGroupBox.TabStop = False
        Me.polynomialRangeGroupBox.Text = "Polynomial Range"
        '
        'maximumNumericUpDown
        '
        Me.maximumNumericUpDown.DecimalPlaces = 2
        Me.maximumNumericUpDown.Location = New System.Drawing.Point(62, 55)
        Me.maximumNumericUpDown.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.maximumNumericUpDown.Minimum = New Decimal(New Integer() {100000, 0, 0, -2147483648})
        Me.maximumNumericUpDown.Name = "maximumNumericUpDown"
        Me.maximumNumericUpDown.Size = New System.Drawing.Size(70, 20)
        Me.maximumNumericUpDown.TabIndex = 3
        Me.maximumNumericUpDown.Value = New Decimal(New Integer() {2, 0, 0, 0})
        '
        'minimumNumericUpDown
        '
        Me.minimumNumericUpDown.DecimalPlaces = 2
        Me.minimumNumericUpDown.Location = New System.Drawing.Point(62, 19)
        Me.minimumNumericUpDown.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.minimumNumericUpDown.Minimum = New Decimal(New Integer() {100000, 0, 0, -2147483648})
        Me.minimumNumericUpDown.Name = "minimumNumericUpDown"
        Me.minimumNumericUpDown.Size = New System.Drawing.Size(70, 20)
        Me.minimumNumericUpDown.TabIndex = 2
        Me.minimumNumericUpDown.Value = New Decimal(New Integer() {2, 0, 0, -2147483648})
        '
        'maximumLabel
        '
        Me.maximumLabel.AutoSize = True
        Me.maximumLabel.Location = New System.Drawing.Point(6, 57)
        Me.maximumLabel.Name = "maximumLabel"
        Me.maximumLabel.Size = New System.Drawing.Size(54, 13)
        Me.maximumLabel.TabIndex = 1
        Me.maximumLabel.Text = "Maximum:"
        '
        'minimumLabel
        '
        Me.minimumLabel.AutoSize = True
        Me.minimumLabel.Location = New System.Drawing.Point(6, 21)
        Me.minimumLabel.Name = "minimumLabel"
        Me.minimumLabel.Size = New System.Drawing.Size(51, 13)
        Me.minimumLabel.TabIndex = 0
        Me.minimumLabel.Text = "Minimum:"
        '
        'coefficientsDirectionGroupBox
        '
        Me.coefficientsDirectionGroupBox.Controls.Add(Me.physToElecRadioButton)
        Me.coefficientsDirectionGroupBox.Controls.Add(Me.electToPhysRadioButton)
        Me.coefficientsDirectionGroupBox.Location = New System.Drawing.Point(6, 11)
        Me.coefficientsDirectionGroupBox.Name = "coefficientsDirectionGroupBox"
        Me.coefficientsDirectionGroupBox.Size = New System.Drawing.Size(143, 67)
        Me.coefficientsDirectionGroupBox.TabIndex = 2
        Me.coefficientsDirectionGroupBox.TabStop = False
        Me.coefficientsDirectionGroupBox.Text = "Coefficients Direction"
        '
        'physToElecRadioButton
        '
        Me.physToElecRadioButton.AutoSize = True
        Me.physToElecRadioButton.Location = New System.Drawing.Point(6, 38)
        Me.physToElecRadioButton.Name = "physToElecRadioButton"
        Me.physToElecRadioButton.Size = New System.Drawing.Size(126, 17)
        Me.physToElecRadioButton.TabIndex = 3
        Me.physToElecRadioButton.TabStop = True
        Me.physToElecRadioButton.Text = "Physical To Electrical"
        Me.physToElecRadioButton.UseVisualStyleBackColor = True
        '
        'electToPhysRadioButton
        '
        Me.electToPhysRadioButton.AutoSize = True
        Me.electToPhysRadioButton.Location = New System.Drawing.Point(6, 19)
        Me.electToPhysRadioButton.Name = "electToPhysRadioButton"
        Me.electToPhysRadioButton.Size = New System.Drawing.Size(126, 17)
        Me.electToPhysRadioButton.TabIndex = 2
        Me.electToPhysRadioButton.TabStop = True
        Me.electToPhysRadioButton.Text = "Electrical To Physical"
        Me.electToPhysRadioButton.UseVisualStyleBackColor = True
        '
        'acquisitionResultGroupBox
        '
        Me.acquisitionResultGroupBox.Controls.Add(Me.acquisitionDataGrid)
        Me.acquisitionResultGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.acquisitionResultGroupBox.Location = New System.Drawing.Point(298, 12)
        Me.acquisitionResultGroupBox.Name = "acquisitionResultGroupBox"
        Me.acquisitionResultGroupBox.Size = New System.Drawing.Size(478, 212)
        Me.acquisitionResultGroupBox.TabIndex = 13
        Me.acquisitionResultGroupBox.TabStop = False
        Me.acquisitionResultGroupBox.Text = "Acquisition Results"
        '
        'acquisitionDataGrid
        '
        Me.acquisitionDataGrid.DataMember = ""
        Me.acquisitionDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.acquisitionDataGrid.Location = New System.Drawing.Point(8, 24)
        Me.acquisitionDataGrid.Name = "acquisitionDataGrid"
        Me.acquisitionDataGrid.ReadOnly = True
        Me.acquisitionDataGrid.Size = New System.Drawing.Size(464, 182)
        Me.acquisitionDataGrid.TabIndex = 1
        Me.acquisitionDataGrid.TabStop = False
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(784, 514)
        Me.Controls.Add(Me.sensorScalingParametersGroupBox)
        Me.Controls.Add(Me.acquisitionResultGroupBox)
        Me.Controls.Add(Me.shuntCalibrationParametersGroupBox)
        Me.Controls.Add(Me.wheatstoneBridgeParametersGroupBox)
        Me.Controls.Add(Me.channelParametersGroupBox)
        Me.Controls.Add(Me.timingParametersGroupBox)
        Me.Controls.Add(Me.stopButton)
        Me.Controls.Add(Me.startButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Wheatstone Bridge Strain Samples - 9237"
        Me.shuntCalibrationParametersGroupBox.ResumeLayout(False)
        Me.shuntCalibrationParametersGroupBox.PerformLayout()
        CType(Me.shuntResistanceNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        Me.wheatstoneBridgeParametersGroupBox.ResumeLayout(False)
        CType(Me.nomGageResNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.excitationValueNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        Me.channelParametersGroupBox.ResumeLayout(False)
        Me.channelParametersGroupBox.PerformLayout()
        CType(Me.minimumValueNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.maximumValueNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        Me.timingParametersGroupBox.ResumeLayout(False)
        CType(Me.rateNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.samplesPerChannelNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        Me.sensorScalingParametersGroupBox.ResumeLayout(False)
        Me.sensorScalingParametersGroupBox.PerformLayout()
        Me.sensorScalingTabControl.ResumeLayout(False)
        Me.linearTabPage.ResumeLayout(False)
        Me.linearTabPage.PerformLayout()
        CType(Me.secondPhysicalValueNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.firstPhysicalValueNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.secondElectricalValueNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.firstElectricalValueNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tableTabPage.ResumeLayout(False)
        CType(Me.tableDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.polynomialTabPage.ResumeLayout(False)
        CType(Me.polynomialDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.polynomialRangeGroupBox.ResumeLayout(False)
        Me.polynomialRangeGroupBox.PerformLayout()
        CType(Me.maximumNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.minimumNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        Me.coefficientsDirectionGroupBox.ResumeLayout(False)
        Me.coefficientsDirectionGroupBox.PerformLayout()
        Me.acquisitionResultGroupBox.ResumeLayout(False)
        CType(Me.acquisitionDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub MainForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        channelDataTable = New DataTable
        bridgeConfigurationComboBox.SelectedIndex = 0
        excitationSourceComboBox.SelectedIndex = 0

        physicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AI, PhysicalChannelAccess.External))

        If (physicalChannelComboBox.Items.Count > 0) Then
            physicalChannelComboBox.SelectedIndex = 0
            startButton.Enabled = True
        End If

        electToPhysRadioButton.Select()

    End Sub

    Private Sub startButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles startButton.Click
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
        startButton.Enabled = False
        stopButton.Enabled = True

        Try
            ' Get the selected AIBridgeConfiguation
            Dim bridgeConfiguration As AIBridgeConfiguration
            If bridgeConfigurationComboBox.SelectedIndex = 0 Then
                bridgeConfiguration = AIBridgeConfiguration.FullBridge
            ElseIf bridgeConfigurationComboBox.SelectedIndex = 1 Then
                bridgeConfiguration = AIBridgeConfiguration.HalfBridge
            ElseIf bridgeConfigurationComboBox.SelectedIndex = 2 Then
                bridgeConfiguration = AIBridgeConfiguration.QuarterBridge
            Else
                bridgeConfiguration = AIBridgeConfiguration.NoBridge
            End If

            ' Get the excitation source
            Dim excitationSource As AIExcitationSource
            If excitationSourceComboBox.SelectedIndex = 0 Then
                excitationSource = AIExcitationSource.Internal
            ElseIf excitationSourceComboBox.SelectedIndex = 1 Then
                excitationSource = AIExcitationSource.External
            Else
                excitationSource = AIExcitationSource.None
            End If

            Dim units As AIForceUnits
            If (unitsComboBox.SelectedItem = "kgf") Then
                units = AIForceUnits.KilogramForce
            ElseIf (unitsComboBox.SelectedItem = "Pounds") Then
                units = AIForceUnits.Pounds
            Else
                units = AIForceUnits.Newtons
            End If

            Dim electricalUnits As AIBridgeElectricalUnits
            If (electricalUnitsComboBox.SelectedItem = "mV/V") Then
                electricalUnits = AIBridgeElectricalUnits.MillivoltsPerVolt
            Else
                electricalUnits = AIBridgeElectricalUnits.VoltsPerVolt
            End If


            Dim physicalUnits As AIBridgePhysicalUnits
            If (physicalUnitsComboBox.SelectedItem = "kgf") Then
                physicalUnits = AIBridgePhysicalUnits.KilogramForce
            ElseIf (physicalUnitsComboBox.SelectedItem = "Pounds") Then
                physicalUnits = AIBridgePhysicalUnits.Pounds
            Else
                physicalUnits = AIBridgePhysicalUnits.Newtons
            End If


            ' Create and configure AI channel
            myTask = New Task
            Dim myAIChannel As AIChannel

            If (sensorScalingTabControl.SelectedIndex = 0) Then
                myAIChannel = myTask.AIChannels.CreateForceBridgeTwoPointLinearChannel( _
                    physicalChannelComboBox.Text, "", Convert.ToDouble(minimumValueNumericUpDown.Value), _
                    Convert.ToDouble(maximumValueNumericUpDown.Value), units, bridgeConfiguration, excitationSource, _
                    Convert.ToDouble(excitationValueNumericUpDown.Value), Convert.ToDouble(nomGageResNumericUpDown.Value), _
                    Convert.ToDouble(firstElectricalValueNumericUpDown.Value), _
                    Convert.ToDouble(secondElectricalValueNumericUpDown.Value), _
                    electricalUnits, _
                    Convert.ToDouble(firstPhysicalValueNumericUpDown.Value), _
                    Convert.ToDouble(secondPhysicalValueNumericUpDown.Value), _
                    physicalUnits)
            ElseIf (sensorScalingTabControl.SelectedIndex = 1) Then
                Dim electricalValues(tableDataGridView.Rows.Count - 2) As Double
                Dim physicalValues(tableDataGridView.Rows.Count - 2) As Double

                For i As Integer = 0 To electricalValues.Length - 1
                    electricalValues(i) = Convert.ToDouble(tableDataGridView.Rows(i).Cells(0).Value)
                    physicalValues(i) = Convert.ToDouble(tableDataGridView.Rows(i).Cells(1).Value)
                Next

                myAIChannel = myTask.AIChannels.CreateForceBridgeTableChannel(physicalChannelComboBox.Text, "", _
                    Convert.ToDouble(minimumValueNumericUpDown.Value), _
                    Convert.ToDouble(maximumValueNumericUpDown.Value), _
                    units, bridgeConfiguration, excitationSource, _
                    Convert.ToDouble(excitationValueNumericUpDown.Value), _
                    Convert.ToDouble(nomGageResNumericUpDown.Value), _
                    electricalValues, electricalUnits, _
                    physicalValues, physicalUnits)
            Else
                Dim coefficients(polynomialDataGrid.Rows.Count - 2) As Double
                Dim forward() As Double
                Dim reverse() As Double
                For i As Integer = 0 To coefficients.Length - 1
                    coefficients(i) = Convert.ToDouble(polynomialDataGrid.Rows(i).Cells(0).Value)
                Next

                If (electToPhysRadioButton.Checked) Then
                    forward = coefficients
                    Dim scale As PolynomialScale = New PolynomialScale("scale", _
                        PolynomialDirection.Forward, _
                        forward, Convert.ToDouble(minimumNumericUpDown.Value), _
                        Convert.ToDouble(maximumNumericUpDown.Value))
                    reverse = scale.ReverseCoefficients
                Else
                    reverse = coefficients
                    Dim scale As PolynomialScale = New PolynomialScale("scale", _
                        PolynomialDirection.Reverse, _
                        reverse, Convert.ToDouble(minimumNumericUpDown.Value), _
                        Convert.ToDouble(maximumNumericUpDown.Value))
                    forward = scale.ForwardCoefficients
                End If
                myAIChannel = myTask.AIChannels.CreateForceBridgePolynomialChannel(physicalChannelComboBox.Text, "", _
                    Convert.ToDouble(minimumValueNumericUpDown.Value), _
                    Convert.ToDouble(maximumValueNumericUpDown.Value), _
                    units, bridgeConfiguration, excitationSource, _
                    Convert.ToDouble(excitationValueNumericUpDown.Value), _
                    Convert.ToDouble(nomGageResNumericUpDown.Value), _
                    forward, reverse, electricalUnits, physicalUnits)

            End If


            ' Configure the sample clock
            myTask.Timing.ConfigureSampleClock("", Convert.ToDouble(rateNumericUpDown.Value), SampleClockActiveEdge.Rising, SampleQuantityMode.ContinuousSamples)

            ' Verify task
            myTask.Control(TaskAction.Verify)

            If bridgeNullCheckBox.Checked Then
                myAIChannel.PerformBridgeOffsetNullingCalibration()
            End If

            InitializeDataTable(channelDataTable)
            acquisitionDataGrid.DataSource = channelDataTable

            runningTask = myTask

            reader = New AnalogMultiChannelReader(myTask.Stream)

            callback = New AsyncCallback(AddressOf AnalogCallback)

            myTask.Start()

            ' Use SynchronizeCallbacks to specify that the object 
            ' marshals callbacks across threads appropriately.
            myTask.SynchronizeCallbacks = True
            reader.BeginReadWaveform(Convert.ToInt32(samplesPerChannelNumericUpDown.Value), callback, myTask)
        Catch exception As DaqException
            HandleExceptions(exception)
        End Try

        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
    End Sub

    Private Sub stopButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles stopButton.Click
        runningTask = Nothing
        myTask.Dispose()

        stopButton.Enabled = False
        startButton.Enabled = True
    End Sub

    Private Sub AnalogCallback(ByVal ar As IAsyncResult)
        Try
            If (Not (runningTask Is Nothing)) AndAlso runningTask Is ar.AsyncState Then
                ' Read the available data from the channels
                waveform = reader.EndReadWaveform(ar)

                ' Populate data table   
                dataToDataTable(waveform, channelDataTable)

                ' Set up a new callback
                reader.BeginMemoryOptimizedReadWaveform(Convert.ToInt32(samplesPerChannelNumericUpDown.Value), callback, myTask, waveform)
            End If
        Catch exception As DaqException
            HandleExceptions(exception)
        End Try
    End Sub

    Private Sub dataToDataTable(ByVal sourceArray As AnalogWaveform(Of Double)(), ByRef dataTable As DataTable)
        ' Iterate over channels
        Dim currentLineIndex As Integer = 0
        For Each waveform As AnalogWaveform(Of Double) In sourceArray
            Dim dataCount As Integer = 0
            If waveform.Samples.Count < 15 Then
                dataCount = waveform.Samples.Count
            Else
                dataCount = 15
            End If
            For sample As Integer = 0 To (dataCount - 1)
                dataTable.Rows(sample)(currentLineIndex) = waveform.Samples(sample).Value
            Next
            currentLineIndex += 1
        Next
    End Sub

    Private Sub InitializeDataTable(ByRef data As dataTable)
        Dim numOfLines As Integer = Convert.ToInt32(myTask.AIChannels.Count)
        data.Rows.Clear()
        data.Columns.Clear()
        channelDataColumn = New DataColumn(numOfLines - 1) {}
        Dim numOfRows As Integer = 15

        For currentLineIndex As Integer = 0 To numOfLines - 1
            channelDataColumn(currentLineIndex) = New DataColumn
            channelDataColumn(currentLineIndex).DataType = GetType(Double)
            channelDataColumn(currentLineIndex).ColumnName = myTask.AIChannels(currentLineIndex).PhysicalName
        Next

        data.Columns.AddRange(channelDataColumn)

        For currentDataIndex As Integer = 0 To numOfRows - 1
            Dim rowArr As Object() = New Object(numOfLines - 1) {}
            data.Rows.Add(rowArr)
        Next
    End Sub

    Private Sub HandleExceptions(ByVal exception As DaqException)
        MessageBox.Show(exception.Message)

        runningTask = Nothing
        myTask.Dispose()
        startButton.Enabled = True
        stopButton.Enabled = False
    End Sub
End Class
