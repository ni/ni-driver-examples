'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   ContAcqCustomVoltageSamples_9237
'
' Category:
'   AI
'
' Description:
'   This example performs Wheatstone Bridge measurements with offset nulling if
'   desired.
'
' Instructions for running:
'   1.  Enter the list of physical channels, and set the attributesof the bridge
'       configuration connected to all the channels.The 'Maximum Value' and
'       'Minimum Value' inputs specify therange, in Custom Scale units, that you
'       expect of yourmeasurements.
'   2.  Make sure your Bridge sensor is in its relaxed state.
'   3.  You may check the 'Perform Bridge Null?' option to automaticallynull out
'       your offset by performing a hardware nullingoperation (if supported by
'       the hardware) followed by asoftware nulling operation. (NOTE: The
'       software nullingoperation will cause a loss in dynamic range while a
'       hardwarenulling operation will not cause any loss in the dynamicrange).
'   4.  Specify Sensor Scaling Parameters. You can choose a LinearScale or Map
'       Ranges Scale. The channel Maximum and Minimumvalues are specified in
'       terms of the scaled units.
'   5.  Run the example and do not disturb your bridge sensor untildata starts
'       being plotted.
'
' Steps:
'   1.  Create custom scale.
'   2.  Create a new Task. Create a AIChannel by using the
'       CreateVoltageChannelWithExcitation method.
'   3.  Set the rate for the sample clock by using the
'       Timing.ConfigureSampleClock method. Additionally, define the sample mode
'       to be continuous.
'   4.  If nulling is desired, call the DAQmx Perform Bridge OffsetNulling
'       Calibration function to perform both hardware nulling(if supported) and
'       software nulling.
'   5.  Call AnalogMultiChannelReader.BeginReadWaveform to install a callback
'       and begin the asynchronous read operation.
'   6.  Inside the callback, call AnalogMultiChannelReader.EndReadWaveform to
'       retrieve the data from the read operation.  
'   7.  Call AnalogMultiChannelReader.BeginMemoryOptimizedReadWaveform
'   8.  When the user presses the stop button, stop the task.
'   9.  Dispose the Task object to clean-up any resources associated with the
'       task.
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
'   control.  For more detailed connection information and bridge calibration
'   procedures refer to your NI 9237 module's hardware reference manual.
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
    Friend WithEvents sensorScalingParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents scaleTabControl As System.Windows.Forms.TabControl
    Friend WithEvents linearTabPage As System.Windows.Forms.TabPage
    Friend WithEvents linearScaledUnitsTextBox As System.Windows.Forms.TextBox
    Friend WithEvents mLabel As System.Windows.Forms.Label
    Friend WithEvents mNumericUpDown As System.Windows.Forms.NumericUpDown
    Friend WithEvents equationLabel As System.Windows.Forms.Label
    Friend WithEvents bLabel As System.Windows.Forms.Label
    Friend WithEvents bNumericUpDown As System.Windows.Forms.NumericUpDown
    Friend WithEvents linearScaledUnitsLabel As System.Windows.Forms.Label
    Friend WithEvents mapRangesTabPage As System.Windows.Forms.TabPage
    Friend WithEvents mapRangesScaledUnitsTextBox As System.Windows.Forms.TextBox
    Friend WithEvents minLabel As System.Windows.Forms.Label
    Friend WithEvents minNumericUpDown As System.Windows.Forms.NumericUpDown
    Friend WithEvents maxLabel As System.Windows.Forms.Label
    Friend WithEvents maxNumericUpDown As System.Windows.Forms.NumericUpDown
    Friend WithEvents mapRangesScaledUnitsLabel As System.Windows.Forms.Label
    Friend WithEvents minScaledNumericUpDown As System.Windows.Forms.NumericUpDown
    Friend WithEvents maxScaledLabel As System.Windows.Forms.Label
    Friend WithEvents maxScaledNumericUpDown As System.Windows.Forms.NumericUpDown
    Friend WithEvents minScaledLabel As System.Windows.Forms.Label
    Friend WithEvents timingParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents rateNumericUpDown As System.Windows.Forms.NumericUpDown
    Friend WithEvents samplesPerChannelLabel As System.Windows.Forms.Label
    Friend WithEvents rateLabel As System.Windows.Forms.Label
    Friend WithEvents samplesPerChannelNumericUpDown As System.Windows.Forms.NumericUpDown
    Friend WithEvents wheatstoneBridgeParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents bridgeConfigurationComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents bridgeConfigurationLabel As System.Windows.Forms.Label
    Friend WithEvents excitationSourceLabel As System.Windows.Forms.Label
    Friend WithEvents excitationSourceComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents excitationValueComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents excitationValueLabel As System.Windows.Forms.Label
    Friend WithEvents acquisitionResultGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents acquisitionDataGrid As System.Windows.Forms.DataGrid
    Friend WithEvents bridgeNullCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents stopButton As System.Windows.Forms.Button
    Friend WithEvents startButton As System.Windows.Forms.Button
    Friend WithEvents channelParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents physicalChannelComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents minimumValueNumericUpDown As System.Windows.Forms.NumericUpDown
    Friend WithEvents maximumValueNumericUpDown As System.Windows.Forms.NumericUpDown
    Friend WithEvents minimumValueLabel As System.Windows.Forms.Label
    Friend WithEvents maximumValueLabel As System.Windows.Forms.Label
    Friend WithEvents physicalChannelLabel As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.sensorScalingParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.scaleTabControl = New System.Windows.Forms.TabControl
        Me.linearTabPage = New System.Windows.Forms.TabPage
        Me.linearScaledUnitsTextBox = New System.Windows.Forms.TextBox
        Me.mLabel = New System.Windows.Forms.Label
        Me.mNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.equationLabel = New System.Windows.Forms.Label
        Me.bLabel = New System.Windows.Forms.Label
        Me.bNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.linearScaledUnitsLabel = New System.Windows.Forms.Label
        Me.mapRangesTabPage = New System.Windows.Forms.TabPage
        Me.mapRangesScaledUnitsTextBox = New System.Windows.Forms.TextBox
        Me.minLabel = New System.Windows.Forms.Label
        Me.minNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.maxLabel = New System.Windows.Forms.Label
        Me.maxNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.mapRangesScaledUnitsLabel = New System.Windows.Forms.Label
        Me.minScaledNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.maxScaledLabel = New System.Windows.Forms.Label
        Me.maxScaledNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.minScaledLabel = New System.Windows.Forms.Label
        Me.timingParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.rateNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.samplesPerChannelLabel = New System.Windows.Forms.Label
        Me.rateLabel = New System.Windows.Forms.Label
        Me.samplesPerChannelNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.wheatstoneBridgeParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.bridgeConfigurationComboBox = New System.Windows.Forms.ComboBox
        Me.bridgeConfigurationLabel = New System.Windows.Forms.Label
        Me.excitationSourceLabel = New System.Windows.Forms.Label
        Me.excitationSourceComboBox = New System.Windows.Forms.ComboBox
        Me.excitationValueComboBox = New System.Windows.Forms.ComboBox
        Me.excitationValueLabel = New System.Windows.Forms.Label
        Me.acquisitionResultGroupBox = New System.Windows.Forms.GroupBox
        Me.acquisitionDataGrid = New System.Windows.Forms.DataGrid
        Me.bridgeNullCheckBox = New System.Windows.Forms.CheckBox
        Me.stopButton = New System.Windows.Forms.Button
        Me.startButton = New System.Windows.Forms.Button
        Me.channelParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.physicalChannelComboBox = New System.Windows.Forms.ComboBox
        Me.minimumValueNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.maximumValueNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.minimumValueLabel = New System.Windows.Forms.Label
        Me.maximumValueLabel = New System.Windows.Forms.Label
        Me.physicalChannelLabel = New System.Windows.Forms.Label
        Me.sensorScalingParametersGroupBox.SuspendLayout()
        Me.scaleTabControl.SuspendLayout()
        Me.linearTabPage.SuspendLayout()
        CType(Me.mNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.mapRangesTabPage.SuspendLayout()
        CType(Me.minNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.maxNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.minScaledNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.maxScaledNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.timingParametersGroupBox.SuspendLayout()
        CType(Me.rateNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.samplesPerChannelNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.wheatstoneBridgeParametersGroupBox.SuspendLayout()
        Me.acquisitionResultGroupBox.SuspendLayout()
        CType(Me.acquisitionDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.channelParametersGroupBox.SuspendLayout()
        CType(Me.minimumValueNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.maximumValueNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'sensorScalingParametersGroupBox
        '
        Me.sensorScalingParametersGroupBox.Controls.Add(Me.scaleTabControl)
        Me.sensorScalingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.sensorScalingParametersGroupBox.Location = New System.Drawing.Point(329, 10)
        Me.sensorScalingParametersGroupBox.Name = "sensorScalingParametersGroupBox"
        Me.sensorScalingParametersGroupBox.Size = New System.Drawing.Size(240, 224)
        Me.sensorScalingParametersGroupBox.TabIndex = 3
        Me.sensorScalingParametersGroupBox.TabStop = False
        Me.sensorScalingParametersGroupBox.Text = "Sensor Scaling Parameters"
        '
        'scaleTabControl
        '
        Me.scaleTabControl.Controls.Add(Me.linearTabPage)
        Me.scaleTabControl.Controls.Add(Me.mapRangesTabPage)
        Me.scaleTabControl.Location = New System.Drawing.Point(8, 24)
        Me.scaleTabControl.Name = "scaleTabControl"
        Me.scaleTabControl.SelectedIndex = 0
        Me.scaleTabControl.Size = New System.Drawing.Size(224, 192)
        Me.scaleTabControl.TabIndex = 0
        '
        'linearTabPage
        '
        Me.linearTabPage.Controls.Add(Me.linearScaledUnitsTextBox)
        Me.linearTabPage.Controls.Add(Me.mLabel)
        Me.linearTabPage.Controls.Add(Me.mNumericUpDown)
        Me.linearTabPage.Controls.Add(Me.equationLabel)
        Me.linearTabPage.Controls.Add(Me.bLabel)
        Me.linearTabPage.Controls.Add(Me.bNumericUpDown)
        Me.linearTabPage.Controls.Add(Me.linearScaledUnitsLabel)
        Me.linearTabPage.Location = New System.Drawing.Point(4, 22)
        Me.linearTabPage.Name = "linearTabPage"
        Me.linearTabPage.Size = New System.Drawing.Size(216, 166)
        Me.linearTabPage.TabIndex = 0
        Me.linearTabPage.Text = "Linear"
        '
        'linearScaledUnitsTextBox
        '
        Me.linearScaledUnitsTextBox.Location = New System.Drawing.Point(96, 132)
        Me.linearScaledUnitsTextBox.Name = "linearScaledUnitsTextBox"
        Me.linearScaledUnitsTextBox.Size = New System.Drawing.Size(112, 20)
        Me.linearScaledUnitsTextBox.TabIndex = 6
        Me.linearScaledUnitsTextBox.Text = "psi"
        '
        'mLabel
        '
        Me.mLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.mLabel.Location = New System.Drawing.Point(16, 40)
        Me.mLabel.Name = "mLabel"
        Me.mLabel.Size = New System.Drawing.Size(56, 16)
        Me.mLabel.TabIndex = 1
        Me.mLabel.Text = "M:"
        '
        'mNumericUpDown
        '
        Me.mNumericUpDown.DecimalPlaces = 3
        Me.mNumericUpDown.Location = New System.Drawing.Point(96, 36)
        Me.mNumericUpDown.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.mNumericUpDown.Minimum = New Decimal(New Integer() {100000, 0, 0, -2147483648})
        Me.mNumericUpDown.Name = "mNumericUpDown"
        Me.mNumericUpDown.Size = New System.Drawing.Size(112, 20)
        Me.mNumericUpDown.TabIndex = 2
        Me.mNumericUpDown.Value = New Decimal(New Integer() {1000, 0, 0, 0})
        '
        'equationLabel
        '
        Me.equationLabel.Location = New System.Drawing.Point(72, 8)
        Me.equationLabel.Name = "equationLabel"
        Me.equationLabel.Size = New System.Drawing.Size(64, 16)
        Me.equationLabel.TabIndex = 0
        Me.equationLabel.Text = "y = Mx + B"
        '
        'bLabel
        '
        Me.bLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.bLabel.Location = New System.Drawing.Point(16, 88)
        Me.bLabel.Name = "bLabel"
        Me.bLabel.Size = New System.Drawing.Size(56, 16)
        Me.bLabel.TabIndex = 3
        Me.bLabel.Text = "B:"
        '
        'bNumericUpDown
        '
        Me.bNumericUpDown.DecimalPlaces = 3
        Me.bNumericUpDown.Location = New System.Drawing.Point(96, 84)
        Me.bNumericUpDown.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.bNumericUpDown.Minimum = New Decimal(New Integer() {1000, 0, 0, -2147483648})
        Me.bNumericUpDown.Name = "bNumericUpDown"
        Me.bNumericUpDown.Size = New System.Drawing.Size(112, 20)
        Me.bNumericUpDown.TabIndex = 4
        '
        'linearScaledUnitsLabel
        '
        Me.linearScaledUnitsLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.linearScaledUnitsLabel.Location = New System.Drawing.Point(16, 136)
        Me.linearScaledUnitsLabel.Name = "linearScaledUnitsLabel"
        Me.linearScaledUnitsLabel.Size = New System.Drawing.Size(72, 16)
        Me.linearScaledUnitsLabel.TabIndex = 5
        Me.linearScaledUnitsLabel.Text = "Scaled Units:"
        '
        'mapRangesTabPage
        '
        Me.mapRangesTabPage.Controls.Add(Me.mapRangesScaledUnitsTextBox)
        Me.mapRangesTabPage.Controls.Add(Me.minLabel)
        Me.mapRangesTabPage.Controls.Add(Me.minNumericUpDown)
        Me.mapRangesTabPage.Controls.Add(Me.maxLabel)
        Me.mapRangesTabPage.Controls.Add(Me.maxNumericUpDown)
        Me.mapRangesTabPage.Controls.Add(Me.mapRangesScaledUnitsLabel)
        Me.mapRangesTabPage.Controls.Add(Me.minScaledNumericUpDown)
        Me.mapRangesTabPage.Controls.Add(Me.maxScaledLabel)
        Me.mapRangesTabPage.Controls.Add(Me.maxScaledNumericUpDown)
        Me.mapRangesTabPage.Controls.Add(Me.minScaledLabel)
        Me.mapRangesTabPage.Location = New System.Drawing.Point(4, 22)
        Me.mapRangesTabPage.Name = "mapRangesTabPage"
        Me.mapRangesTabPage.Size = New System.Drawing.Size(216, 166)
        Me.mapRangesTabPage.TabIndex = 1
        Me.mapRangesTabPage.Text = "Map Ranges"
        Me.mapRangesTabPage.Visible = False
        '
        'mapRangesScaledUnitsTextBox
        '
        Me.mapRangesScaledUnitsTextBox.Location = New System.Drawing.Point(92, 140)
        Me.mapRangesScaledUnitsTextBox.Name = "mapRangesScaledUnitsTextBox"
        Me.mapRangesScaledUnitsTextBox.Size = New System.Drawing.Size(112, 20)
        Me.mapRangesScaledUnitsTextBox.TabIndex = 9
        Me.mapRangesScaledUnitsTextBox.Text = "lbs"
        '
        'minLabel
        '
        Me.minLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.minLabel.Location = New System.Drawing.Point(12, 24)
        Me.minLabel.Name = "minLabel"
        Me.minLabel.Size = New System.Drawing.Size(56, 16)
        Me.minLabel.TabIndex = 0
        Me.minLabel.Text = "Min (V):"
        '
        'minNumericUpDown
        '
        Me.minNumericUpDown.DecimalPlaces = 2
        Me.minNumericUpDown.Location = New System.Drawing.Point(92, 16)
        Me.minNumericUpDown.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.minNumericUpDown.Minimum = New Decimal(New Integer() {1000, 0, 0, -2147483648})
        Me.minNumericUpDown.Name = "minNumericUpDown"
        Me.minNumericUpDown.Size = New System.Drawing.Size(112, 20)
        Me.minNumericUpDown.TabIndex = 1
        Me.minNumericUpDown.Value = New Decimal(New Integer() {20, 0, 0, -2147483648})
        '
        'maxLabel
        '
        Me.maxLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.maxLabel.Location = New System.Drawing.Point(12, 56)
        Me.maxLabel.Name = "maxLabel"
        Me.maxLabel.Size = New System.Drawing.Size(56, 16)
        Me.maxLabel.TabIndex = 2
        Me.maxLabel.Text = "Max (V):"
        '
        'maxNumericUpDown
        '
        Me.maxNumericUpDown.DecimalPlaces = 2
        Me.maxNumericUpDown.Location = New System.Drawing.Point(92, 48)
        Me.maxNumericUpDown.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.maxNumericUpDown.Minimum = New Decimal(New Integer() {1000, 0, 0, -2147483648})
        Me.maxNumericUpDown.Name = "maxNumericUpDown"
        Me.maxNumericUpDown.Size = New System.Drawing.Size(112, 20)
        Me.maxNumericUpDown.TabIndex = 3
        Me.maxNumericUpDown.Value = New Decimal(New Integer() {20, 0, 0, 0})
        '
        'mapRangesScaledUnitsLabel
        '
        Me.mapRangesScaledUnitsLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.mapRangesScaledUnitsLabel.Location = New System.Drawing.Point(12, 144)
        Me.mapRangesScaledUnitsLabel.Name = "mapRangesScaledUnitsLabel"
        Me.mapRangesScaledUnitsLabel.Size = New System.Drawing.Size(72, 16)
        Me.mapRangesScaledUnitsLabel.TabIndex = 8
        Me.mapRangesScaledUnitsLabel.Text = "Scaled Units:"
        '
        'minScaledNumericUpDown
        '
        Me.minScaledNumericUpDown.DecimalPlaces = 2
        Me.minScaledNumericUpDown.Location = New System.Drawing.Point(92, 80)
        Me.minScaledNumericUpDown.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.minScaledNumericUpDown.Minimum = New Decimal(New Integer() {1000, 0, 0, -2147483648})
        Me.minScaledNumericUpDown.Name = "minScaledNumericUpDown"
        Me.minScaledNumericUpDown.Size = New System.Drawing.Size(112, 20)
        Me.minScaledNumericUpDown.TabIndex = 5
        Me.minScaledNumericUpDown.Value = New Decimal(New Integer() {50, 0, 0, -2147483648})
        '
        'maxScaledLabel
        '
        Me.maxScaledLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.maxScaledLabel.Location = New System.Drawing.Point(12, 120)
        Me.maxScaledLabel.Name = "maxScaledLabel"
        Me.maxScaledLabel.Size = New System.Drawing.Size(68, 16)
        Me.maxScaledLabel.TabIndex = 6
        Me.maxScaledLabel.Text = "Max (scaled):"
        '
        'maxScaledNumericUpDown
        '
        Me.maxScaledNumericUpDown.DecimalPlaces = 2
        Me.maxScaledNumericUpDown.Location = New System.Drawing.Point(92, 112)
        Me.maxScaledNumericUpDown.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.maxScaledNumericUpDown.Minimum = New Decimal(New Integer() {1000, 0, 0, -2147483648})
        Me.maxScaledNumericUpDown.Name = "maxScaledNumericUpDown"
        Me.maxScaledNumericUpDown.Size = New System.Drawing.Size(112, 20)
        Me.maxScaledNumericUpDown.TabIndex = 7
        Me.maxScaledNumericUpDown.Value = New Decimal(New Integer() {50, 0, 0, 0})
        '
        'minScaledLabel
        '
        Me.minScaledLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.minScaledLabel.Location = New System.Drawing.Point(12, 88)
        Me.minScaledLabel.Name = "minScaledLabel"
        Me.minScaledLabel.Size = New System.Drawing.Size(68, 16)
        Me.minScaledLabel.TabIndex = 4
        Me.minScaledLabel.Text = "Min (scaled):"
        '
        'timingParametersGroupBox
        '
        Me.timingParametersGroupBox.Controls.Add(Me.rateNumericUpDown)
        Me.timingParametersGroupBox.Controls.Add(Me.samplesPerChannelLabel)
        Me.timingParametersGroupBox.Controls.Add(Me.rateLabel)
        Me.timingParametersGroupBox.Controls.Add(Me.samplesPerChannelNumericUpDown)
        Me.timingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.timingParametersGroupBox.Location = New System.Drawing.Point(9, 146)
        Me.timingParametersGroupBox.Name = "timingParametersGroupBox"
        Me.timingParametersGroupBox.Size = New System.Drawing.Size(320, 88)
        Me.timingParametersGroupBox.TabIndex = 1
        Me.timingParametersGroupBox.TabStop = False
        Me.timingParametersGroupBox.Text = "Timing Parameters"
        '
        'rateNumericUpDown
        '
        Me.rateNumericUpDown.DecimalPlaces = 2
        Me.rateNumericUpDown.Location = New System.Drawing.Point(136, 56)
        Me.rateNumericUpDown.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.rateNumericUpDown.Name = "rateNumericUpDown"
        Me.rateNumericUpDown.Size = New System.Drawing.Size(176, 20)
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
        Me.rateLabel.Location = New System.Drawing.Point(16, 56)
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
        Me.samplesPerChannelNumericUpDown.Size = New System.Drawing.Size(176, 20)
        Me.samplesPerChannelNumericUpDown.TabIndex = 1
        Me.samplesPerChannelNumericUpDown.Value = New Decimal(New Integer() {5000, 0, 0, 0})
        '
        'wheatstoneBridgeParametersGroupBox
        '
        Me.wheatstoneBridgeParametersGroupBox.Controls.Add(Me.bridgeConfigurationComboBox)
        Me.wheatstoneBridgeParametersGroupBox.Controls.Add(Me.bridgeConfigurationLabel)
        Me.wheatstoneBridgeParametersGroupBox.Controls.Add(Me.excitationSourceLabel)
        Me.wheatstoneBridgeParametersGroupBox.Controls.Add(Me.excitationSourceComboBox)
        Me.wheatstoneBridgeParametersGroupBox.Controls.Add(Me.excitationValueComboBox)
        Me.wheatstoneBridgeParametersGroupBox.Controls.Add(Me.excitationValueLabel)
        Me.wheatstoneBridgeParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.wheatstoneBridgeParametersGroupBox.Location = New System.Drawing.Point(9, 242)
        Me.wheatstoneBridgeParametersGroupBox.Name = "wheatstoneBridgeParametersGroupBox"
        Me.wheatstoneBridgeParametersGroupBox.Size = New System.Drawing.Size(320, 112)
        Me.wheatstoneBridgeParametersGroupBox.TabIndex = 2
        Me.wheatstoneBridgeParametersGroupBox.TabStop = False
        Me.wheatstoneBridgeParametersGroupBox.Text = "Wheatstone Bridge Parameters"
        '
        'bridgeConfigurationComboBox
        '
        Me.bridgeConfigurationComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.bridgeConfigurationComboBox.Items.AddRange(New Object() {"Full Bridge", "Half Bridge", "Quarter Bridge", "No Bridge"})
        Me.bridgeConfigurationComboBox.Location = New System.Drawing.Point(136, 19)
        Me.bridgeConfigurationComboBox.Name = "bridgeConfigurationComboBox"
        Me.bridgeConfigurationComboBox.Size = New System.Drawing.Size(176, 21)
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
        Me.excitationSourceLabel.Location = New System.Drawing.Point(16, 56)
        Me.excitationSourceLabel.Name = "excitationSourceLabel"
        Me.excitationSourceLabel.Size = New System.Drawing.Size(112, 16)
        Me.excitationSourceLabel.TabIndex = 2
        Me.excitationSourceLabel.Text = "Excitation Source:"
        '
        'excitationSourceComboBox
        '
        Me.excitationSourceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.excitationSourceComboBox.Items.AddRange(New Object() {"Internal", "External", "None"})
        Me.excitationSourceComboBox.Location = New System.Drawing.Point(136, 51)
        Me.excitationSourceComboBox.Name = "excitationSourceComboBox"
        Me.excitationSourceComboBox.Size = New System.Drawing.Size(176, 21)
        Me.excitationSourceComboBox.TabIndex = 3
        '
        'excitationValueComboBox
        '
        Me.excitationValueComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.excitationValueComboBox.Items.AddRange(New Object() {"2.5", "3.3", "5", "10"})
        Me.excitationValueComboBox.Location = New System.Drawing.Point(136, 83)
        Me.excitationValueComboBox.Name = "excitationValueComboBox"
        Me.excitationValueComboBox.Size = New System.Drawing.Size(176, 21)
        Me.excitationValueComboBox.TabIndex = 5
        '
        'excitationValueLabel
        '
        Me.excitationValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.excitationValueLabel.Location = New System.Drawing.Point(16, 88)
        Me.excitationValueLabel.Name = "excitationValueLabel"
        Me.excitationValueLabel.Size = New System.Drawing.Size(112, 16)
        Me.excitationValueLabel.TabIndex = 4
        Me.excitationValueLabel.Text = "Excitation Value (V):"
        '
        'acquisitionResultGroupBox
        '
        Me.acquisitionResultGroupBox.Controls.Add(Me.acquisitionDataGrid)
        Me.acquisitionResultGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.acquisitionResultGroupBox.Location = New System.Drawing.Point(573, 10)
        Me.acquisitionResultGroupBox.Name = "acquisitionResultGroupBox"
        Me.acquisitionResultGroupBox.Size = New System.Drawing.Size(200, 344)
        Me.acquisitionResultGroupBox.TabIndex = 7
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
        Me.acquisitionDataGrid.Size = New System.Drawing.Size(184, 312)
        Me.acquisitionDataGrid.TabIndex = 1
        Me.acquisitionDataGrid.TabStop = False
        '
        'bridgeNullCheckBox
        '
        Me.bridgeNullCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.bridgeNullCheckBox.Location = New System.Drawing.Point(397, 266)
        Me.bridgeNullCheckBox.Name = "bridgeNullCheckBox"
        Me.bridgeNullCheckBox.Size = New System.Drawing.Size(128, 40)
        Me.bridgeNullCheckBox.TabIndex = 4
        Me.bridgeNullCheckBox.Text = "Perform Bridge Null?"
        '
        'stopButton
        '
        Me.stopButton.Enabled = False
        Me.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.stopButton.Location = New System.Drawing.Point(461, 330)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(88, 24)
        Me.stopButton.TabIndex = 6
        Me.stopButton.Text = "Stop"
        '
        'startButton
        '
        Me.startButton.Enabled = False
        Me.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.startButton.Location = New System.Drawing.Point(365, 330)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(88, 24)
        Me.startButton.TabIndex = 5
        Me.startButton.Text = "Start"
        '
        'channelParametersGroupBox
        '
        Me.channelParametersGroupBox.Controls.Add(Me.physicalChannelComboBox)
        Me.channelParametersGroupBox.Controls.Add(Me.minimumValueNumericUpDown)
        Me.channelParametersGroupBox.Controls.Add(Me.maximumValueNumericUpDown)
        Me.channelParametersGroupBox.Controls.Add(Me.minimumValueLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.maximumValueLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.physicalChannelLabel)
        Me.channelParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.channelParametersGroupBox.Location = New System.Drawing.Point(9, 10)
        Me.channelParametersGroupBox.Name = "channelParametersGroupBox"
        Me.channelParametersGroupBox.Size = New System.Drawing.Size(320, 128)
        Me.channelParametersGroupBox.TabIndex = 0
        Me.channelParametersGroupBox.TabStop = False
        Me.channelParametersGroupBox.Text = "Channel Parameters"
        '
        'physicalChannelComboBox
        '
        Me.physicalChannelComboBox.Location = New System.Drawing.Point(136, 19)
        Me.physicalChannelComboBox.Name = "physicalChannelComboBox"
        Me.physicalChannelComboBox.Size = New System.Drawing.Size(176, 21)
        Me.physicalChannelComboBox.TabIndex = 1
        '
        'minimumValueNumericUpDown
        '
        Me.minimumValueNumericUpDown.DecimalPlaces = 3
        Me.minimumValueNumericUpDown.Location = New System.Drawing.Point(136, 96)
        Me.minimumValueNumericUpDown.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.minimumValueNumericUpDown.Minimum = New Decimal(New Integer() {10, 0, 0, -2147483648})
        Me.minimumValueNumericUpDown.Name = "minimumValueNumericUpDown"
        Me.minimumValueNumericUpDown.Size = New System.Drawing.Size(176, 20)
        Me.minimumValueNumericUpDown.TabIndex = 5
        Me.minimumValueNumericUpDown.Value = New Decimal(New Integer() {25, 0, 0, -2147287040})
        '
        'maximumValueNumericUpDown
        '
        Me.maximumValueNumericUpDown.DecimalPlaces = 3
        Me.maximumValueNumericUpDown.Location = New System.Drawing.Point(136, 56)
        Me.maximumValueNumericUpDown.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.maximumValueNumericUpDown.Minimum = New Decimal(New Integer() {10, 0, 0, -2147483648})
        Me.maximumValueNumericUpDown.Name = "maximumValueNumericUpDown"
        Me.maximumValueNumericUpDown.Size = New System.Drawing.Size(176, 20)
        Me.maximumValueNumericUpDown.TabIndex = 3
        Me.maximumValueNumericUpDown.Value = New Decimal(New Integer() {25, 0, 0, 196608})
        '
        'minimumValueLabel
        '
        Me.minimumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.minimumValueLabel.Location = New System.Drawing.Point(16, 104)
        Me.minimumValueLabel.Name = "minimumValueLabel"
        Me.minimumValueLabel.Size = New System.Drawing.Size(100, 16)
        Me.minimumValueLabel.TabIndex = 4
        Me.minimumValueLabel.Text = "Minimum Value:"
        '
        'maximumValueLabel
        '
        Me.maximumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.maximumValueLabel.Location = New System.Drawing.Point(16, 64)
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
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(782, 364)
        Me.Controls.Add(Me.sensorScalingParametersGroupBox)
        Me.Controls.Add(Me.timingParametersGroupBox)
        Me.Controls.Add(Me.wheatstoneBridgeParametersGroupBox)
        Me.Controls.Add(Me.acquisitionResultGroupBox)
        Me.Controls.Add(Me.bridgeNullCheckBox)
        Me.Controls.Add(Me.stopButton)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.channelParametersGroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Wheatstone Bridge Strain Samples - 9237"
        Me.sensorScalingParametersGroupBox.ResumeLayout(False)
        Me.scaleTabControl.ResumeLayout(False)
        Me.linearTabPage.ResumeLayout(False)
        CType(Me.mNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        Me.mapRangesTabPage.ResumeLayout(False)
        CType(Me.minNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.maxNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.minScaledNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.maxScaledNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        Me.timingParametersGroupBox.ResumeLayout(False)
        CType(Me.rateNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.samplesPerChannelNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        Me.wheatstoneBridgeParametersGroupBox.ResumeLayout(False)
        Me.acquisitionResultGroupBox.ResumeLayout(False)
        CType(Me.acquisitionDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.channelParametersGroupBox.ResumeLayout(False)
        CType(Me.minimumValueNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.maximumValueNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub MainForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        channelDataTable = New DataTable
        bridgeConfigurationComboBox.SelectedIndex = 0
        excitationSourceComboBox.SelectedIndex = 0
        excitationValueComboBox.SelectedIndex = 0

        physicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AI, PhysicalChannelAccess.External))

        If (physicalChannelComboBox.Items.Count > 0) Then
            physicalChannelComboBox.SelectedIndex = 0
            startButton.Enabled = True
        End If

    End Sub

    Private Sub startButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles startButton.Click
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
        startButton.Enabled = False
        stopButton.Enabled = True

        Try
            ' Create the custom scale based on the user selection, either LinearScale or RangeMapScale.
            If scaleTabControl.SelectedIndex = 0 Then
                customScale = New LinearScale(customScaleName, Convert.ToDouble(mNumericUpDown.Value), Convert.ToDouble(bNumericUpDown.Value))
            Else
                customScale = New RangeMapScale(customScaleName, Convert.ToDouble(minNumericUpDown.Value), Convert.ToDouble(maxNumericUpDown.Value), _
                                                                 Convert.ToDouble(minScaledNumericUpDown.Value), Convert.ToDouble(maxScaledNumericUpDown.Value))
            End If

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

            ' Create and configure AI channel
            myTask = New Task
            Dim myAIChannel As AIChannel = myTask.AIChannels.CreateVoltageChannelWithExcitation(physicalChannelComboBox.Text, "", AITerminalConfiguration.Differential, Convert.ToDouble(minimumValueNumericUpDown.Value), _
                                    Convert.ToDouble(maximumValueNumericUpDown.Value), bridgeConfiguration, excitationSource, Convert.ToDouble(excitationValueComboBox.Text), True, customScale.Name)

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
