'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   ContAcqRTDSamples_IntClk
'
' Category:
'   AI
'
' Description:
'   This example demonstrates how to acquire temperature from an RTD using the
'   internal clock of the DAQ device.
'
' Instructions for running:
'   1.  Select the physical channel corresponding to where your signal is input
'       on the DAQ device.
'   2.  Enter the minimum and maximum temperature values.Note: For better
'       accuracy, try to match the input ranges to the expected temperature
'       level of the measured signal.
'   3.  Enter the acquisition rate.
'   4.  Enter the RTD Type and r0 (resistance at 0 degrees C).Note: If you
'       select "Custom" as your RTD type, you need to modify this example in
'       order to provide the A, B, and C coefficients of the Callendar-Van Dusen
'       equation. The coefficients are specified using the AIChannel object.
'   5.  Enter the resistance configuration, the current excitation source, and
'       the excitation value in Amps.
'
' Steps:
'   1.  Create a new Task object. Create a AIChannel object by using the
'       CreateRtdChannel method.
'   2.  Set the rate for the sample clock by using the
'       Timing.ConfigureSampleClock method. Additionally, define the sample mode
'       to be continuous.
'   3.  Call AnalogMultiChannelReader.BeginReadWaveform to install a callback
'       and begin the asynchronous read operation.
'   4.  Inside the callback, call AnalogMultiChannelReader.EndReadWaveform to
'       retrieve the data from the read operation.  
'   5.  Call AnalogMultiChannelReader.BeginMemoryOptimizedReadWaveform
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
'   Make sure your signal input terminals match the physical channel text box. 
'   For more information on the input and output terminals for your device, open
'   the NI-DAQmx Help, and refer to the NI-DAQmx Device Terminals and Device
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
    Private runningTask As Task
    Private data As AnalogWaveform(Of Double)()
    Private myReader As AnalogMultiChannelReader
    Private myAsyncCallback As AsyncCallback = New AsyncCallback(AddressOf AnalogInCallback)
    Private dataColumn() As DataColumn = Nothing
    Private dataTable As DataTable = New DataTable


#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()
        Application.EnableVisualStyles()

        'This call is required by the Windows Form Designer.
        InitializeComponent()
        RTDTypeComboBox.SelectedIndex = 2
        resistanceConfigurationComboBox.SelectedIndex = 2
        currentExcitationComboBox.SelectedIndex = 1

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
    Friend WithEvents resistanceParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents currentValueLabel As System.Windows.Forms.Label
    Friend WithEvents currentExcitationLabel As System.Windows.Forms.Label
    Friend WithEvents currentExcitationComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents stopButton As System.Windows.Forms.Button
    Friend WithEvents timingParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents rateLabel As System.Windows.Forms.Label
    Friend WithEvents startButton As System.Windows.Forms.Button
    Friend WithEvents channelParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents maximumLabel As System.Windows.Forms.Label
    Friend WithEvents minimumLabel As System.Windows.Forms.Label
    Friend WithEvents physicalChannelLabel As System.Windows.Forms.Label
    Friend WithEvents acquisitionResultsGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents resultLabel As System.Windows.Forms.Label
    Friend WithEvents acquisitionDataGrid As System.Windows.Forms.DataGrid
    Friend WithEvents resistanceConfigurationLabel As System.Windows.Forms.Label
    Friend WithEvents resistanceConfigurationComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents rtdParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents rtdTypeComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents rtdTypeLabel As System.Windows.Forms.Label
    Friend WithEvents rateNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents currentValueNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents r0Numeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents r0Label As System.Windows.Forms.Label
    Friend WithEvents minimumValueNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents maximumValueNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents physicalChannelComboBox As System.Windows.Forms.ComboBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MainForm))
        Me.rtdParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.r0Numeric = New System.Windows.Forms.NumericUpDown
        Me.r0Label = New System.Windows.Forms.Label
        Me.rtdTypeComboBox = New System.Windows.Forms.ComboBox
        Me.rtdTypeLabel = New System.Windows.Forms.Label
        Me.resistanceParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.currentValueNumeric = New System.Windows.Forms.NumericUpDown
        Me.currentValueLabel = New System.Windows.Forms.Label
        Me.currentExcitationLabel = New System.Windows.Forms.Label
        Me.resistanceConfigurationLabel = New System.Windows.Forms.Label
        Me.currentExcitationComboBox = New System.Windows.Forms.ComboBox
        Me.resistanceConfigurationComboBox = New System.Windows.Forms.ComboBox
        Me.stopButton = New System.Windows.Forms.Button
        Me.timingParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.rateNumeric = New System.Windows.Forms.NumericUpDown
        Me.rateLabel = New System.Windows.Forms.Label
        Me.startButton = New System.Windows.Forms.Button
        Me.channelParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.physicalChannelComboBox = New System.Windows.Forms.ComboBox
        Me.minimumValueNumeric = New System.Windows.Forms.NumericUpDown
        Me.maximumValueNumeric = New System.Windows.Forms.NumericUpDown
        Me.maximumLabel = New System.Windows.Forms.Label
        Me.minimumLabel = New System.Windows.Forms.Label
        Me.physicalChannelLabel = New System.Windows.Forms.Label
        Me.acquisitionResultsGroupBox = New System.Windows.Forms.GroupBox
        Me.resultLabel = New System.Windows.Forms.Label
        Me.acquisitionDataGrid = New System.Windows.Forms.DataGrid
        Me.rtdParametersGroupBox.SuspendLayout()
        CType(Me.r0Numeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.resistanceParametersGroupBox.SuspendLayout()
        CType(Me.currentValueNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.timingParametersGroupBox.SuspendLayout()
        CType(Me.rateNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.channelParametersGroupBox.SuspendLayout()
        CType(Me.minimumValueNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.maximumValueNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.acquisitionResultsGroupBox.SuspendLayout()
        CType(Me.acquisitionDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'rtdParametersGroupBox
        '
        Me.rtdParametersGroupBox.Controls.Add(Me.r0Numeric)
        Me.rtdParametersGroupBox.Controls.Add(Me.r0Label)
        Me.rtdParametersGroupBox.Controls.Add(Me.rtdTypeComboBox)
        Me.rtdParametersGroupBox.Controls.Add(Me.rtdTypeLabel)
        Me.rtdParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.rtdParametersGroupBox.Location = New System.Drawing.Point(8, 224)
        Me.rtdParametersGroupBox.Name = "rtdParametersGroupBox"
        Me.rtdParametersGroupBox.Size = New System.Drawing.Size(232, 88)
        Me.rtdParametersGroupBox.TabIndex = 4
        Me.rtdParametersGroupBox.TabStop = False
        Me.rtdParametersGroupBox.Text = "RTD Parameters"
        '
        'r0Numeric
        '
        Me.r0Numeric.DecimalPlaces = 2
        Me.r0Numeric.Location = New System.Drawing.Point(128, 56)
        Me.r0Numeric.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.r0Numeric.Name = "r0Numeric"
        Me.r0Numeric.Size = New System.Drawing.Size(96, 20)
        Me.r0Numeric.TabIndex = 3
        Me.r0Numeric.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        'r0Label
        '
        Me.r0Label.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.r0Label.Location = New System.Drawing.Point(16, 58)
        Me.r0Label.Name = "r0Label"
        Me.r0Label.Size = New System.Drawing.Size(72, 16)
        Me.r0Label.TabIndex = 2
        Me.r0Label.Text = "R0 (Ohms):"
        '
        'rtdTypeComboBox
        '
        Me.rtdTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.rtdTypeComboBox.Items.AddRange(New Object() {"Custom", "Pt3750", "Pt3851", "Pt3911", "Pt3916", "Pt3920", "Pt3928"})
        Me.rtdTypeComboBox.Location = New System.Drawing.Point(128, 24)
        Me.rtdTypeComboBox.Name = "rtdTypeComboBox"
        Me.rtdTypeComboBox.Size = New System.Drawing.Size(96, 21)
        Me.rtdTypeComboBox.TabIndex = 1
        '
        'rtdTypeLabel
        '
        Me.rtdTypeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.rtdTypeLabel.Location = New System.Drawing.Point(16, 26)
        Me.rtdTypeLabel.Name = "rtdTypeLabel"
        Me.rtdTypeLabel.Size = New System.Drawing.Size(72, 16)
        Me.rtdTypeLabel.TabIndex = 0
        Me.rtdTypeLabel.Text = "RTD Type:"
        '
        'resistanceParametersGroupBox
        '
        Me.resistanceParametersGroupBox.Controls.Add(Me.currentValueNumeric)
        Me.resistanceParametersGroupBox.Controls.Add(Me.currentValueLabel)
        Me.resistanceParametersGroupBox.Controls.Add(Me.currentExcitationLabel)
        Me.resistanceParametersGroupBox.Controls.Add(Me.resistanceConfigurationLabel)
        Me.resistanceParametersGroupBox.Controls.Add(Me.currentExcitationComboBox)
        Me.resistanceParametersGroupBox.Controls.Add(Me.resistanceConfigurationComboBox)
        Me.resistanceParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.resistanceParametersGroupBox.Location = New System.Drawing.Point(248, 8)
        Me.resistanceParametersGroupBox.Name = "resistanceParametersGroupBox"
        Me.resistanceParametersGroupBox.Size = New System.Drawing.Size(256, 120)
        Me.resistanceParametersGroupBox.TabIndex = 5
        Me.resistanceParametersGroupBox.TabStop = False
        Me.resistanceParametersGroupBox.Text = "Resistance Parameters"
        '
        'currentValueNumeric
        '
        Me.currentValueNumeric.DecimalPlaces = 5
        Me.currentValueNumeric.Location = New System.Drawing.Point(160, 88)
        Me.currentValueNumeric.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.currentValueNumeric.Name = "currentValueNumeric"
        Me.currentValueNumeric.Size = New System.Drawing.Size(88, 20)
        Me.currentValueNumeric.TabIndex = 5
        Me.currentValueNumeric.Value = New Decimal(New Integer() {15, 0, 0, 327680})
        '
        'currentValueLabel
        '
        Me.currentValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.currentValueLabel.Location = New System.Drawing.Point(16, 91)
        Me.currentValueLabel.Name = "currentValueLabel"
        Me.currentValueLabel.Size = New System.Drawing.Size(152, 14)
        Me.currentValueLabel.TabIndex = 4
        Me.currentValueLabel.Text = "Current Excitation Value (A):"
        '
        'currentExcitationLabel
        '
        Me.currentExcitationLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.currentExcitationLabel.Location = New System.Drawing.Point(16, 59)
        Me.currentExcitationLabel.Name = "currentExcitationLabel"
        Me.currentExcitationLabel.Size = New System.Drawing.Size(136, 14)
        Me.currentExcitationLabel.TabIndex = 2
        Me.currentExcitationLabel.Text = "Current Excitation Source:"
        '
        'resistanceConfigurationLabel
        '
        Me.resistanceConfigurationLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.resistanceConfigurationLabel.Location = New System.Drawing.Point(16, 27)
        Me.resistanceConfigurationLabel.Name = "resistanceConfigurationLabel"
        Me.resistanceConfigurationLabel.Size = New System.Drawing.Size(136, 14)
        Me.resistanceConfigurationLabel.TabIndex = 0
        Me.resistanceConfigurationLabel.Text = "Resistance Configuration:"
        '
        'currentExcitationComboBox
        '
        Me.currentExcitationComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.currentExcitationComboBox.Items.AddRange(New Object() {"None", "Internal", "External"})
        Me.currentExcitationComboBox.Location = New System.Drawing.Point(160, 56)
        Me.currentExcitationComboBox.Name = "currentExcitationComboBox"
        Me.currentExcitationComboBox.Size = New System.Drawing.Size(88, 21)
        Me.currentExcitationComboBox.TabIndex = 3
        '
        'resistanceConfigurationComboBox
        '
        Me.resistanceConfigurationComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.resistanceConfigurationComboBox.Items.AddRange(New Object() {"2-Wire", "3-Wire", "4-Wire"})
        Me.resistanceConfigurationComboBox.Location = New System.Drawing.Point(160, 24)
        Me.resistanceConfigurationComboBox.Name = "resistanceConfigurationComboBox"
        Me.resistanceConfigurationComboBox.Size = New System.Drawing.Size(88, 21)
        Me.resistanceConfigurationComboBox.TabIndex = 1
        '
        'stopButton
        '
        Me.stopButton.Enabled = False
        Me.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.stopButton.Location = New System.Drawing.Point(136, 344)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(80, 24)
        Me.stopButton.TabIndex = 1
        Me.stopButton.Text = "Stop"
        '
        'timingParametersGroupBox
        '
        Me.timingParametersGroupBox.Controls.Add(Me.rateNumeric)
        Me.timingParametersGroupBox.Controls.Add(Me.rateLabel)
        Me.timingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.timingParametersGroupBox.Location = New System.Drawing.Point(8, 144)
        Me.timingParametersGroupBox.Name = "timingParametersGroupBox"
        Me.timingParametersGroupBox.Size = New System.Drawing.Size(232, 64)
        Me.timingParametersGroupBox.TabIndex = 3
        Me.timingParametersGroupBox.TabStop = False
        Me.timingParametersGroupBox.Text = "Timing Parameters"
        '
        'rateNumeric
        '
        Me.rateNumeric.DecimalPlaces = 2
        Me.rateNumeric.Location = New System.Drawing.Point(128, 24)
        Me.rateNumeric.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.rateNumeric.Name = "rateNumeric"
        Me.rateNumeric.Size = New System.Drawing.Size(96, 20)
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
        'startButton
        '
        Me.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.startButton.Location = New System.Drawing.Point(24, 344)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(80, 24)
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
        Me.physicalChannelComboBox.Location = New System.Drawing.Point(128, 24)
        Me.physicalChannelComboBox.Name = "physicalChannelComboBox"
        Me.physicalChannelComboBox.Size = New System.Drawing.Size(96, 21)
        Me.physicalChannelComboBox.TabIndex = 1
        Me.physicalChannelComboBox.Text = "SC1Mod1/ai0"
        '
        'minimumValueNumeric
        '
        Me.minimumValueNumeric.DecimalPlaces = 2
        Me.minimumValueNumeric.Location = New System.Drawing.Point(128, 88)
        Me.minimumValueNumeric.Maximum = New Decimal(New Integer() {500, 0, 0, 0})
        Me.minimumValueNumeric.Minimum = New Decimal(New Integer() {500, 0, 0, -2147483648})
        Me.minimumValueNumeric.Name = "minimumValueNumeric"
        Me.minimumValueNumeric.Size = New System.Drawing.Size(96, 20)
        Me.minimumValueNumeric.TabIndex = 5
        '
        'maximumValueNumeric
        '
        Me.maximumValueNumeric.DecimalPlaces = 2
        Me.maximumValueNumeric.Location = New System.Drawing.Point(128, 56)
        Me.maximumValueNumeric.Maximum = New Decimal(New Integer() {500, 0, 0, 0})
        Me.maximumValueNumeric.Minimum = New Decimal(New Integer() {500, 0, 0, -2147483648})
        Me.maximumValueNumeric.Name = "maximumValueNumeric"
        Me.maximumValueNumeric.Size = New System.Drawing.Size(96, 20)
        Me.maximumValueNumeric.TabIndex = 3
        Me.maximumValueNumeric.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        'maximumLabel
        '
        Me.maximumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.maximumLabel.Location = New System.Drawing.Point(16, 59)
        Me.maximumLabel.Name = "maximumLabel"
        Me.maximumLabel.Size = New System.Drawing.Size(104, 14)
        Me.maximumLabel.TabIndex = 2
        Me.maximumLabel.Text = "Maximum (deg C):"
        '
        'minimumLabel
        '
        Me.minimumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.minimumLabel.Location = New System.Drawing.Point(16, 91)
        Me.minimumLabel.Name = "minimumLabel"
        Me.minimumLabel.Size = New System.Drawing.Size(96, 14)
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
        'acquisitionResultsGroupBox
        '
        Me.acquisitionResultsGroupBox.Controls.Add(Me.resultLabel)
        Me.acquisitionResultsGroupBox.Controls.Add(Me.acquisitionDataGrid)
        Me.acquisitionResultsGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.acquisitionResultsGroupBox.Location = New System.Drawing.Point(248, 144)
        Me.acquisitionResultsGroupBox.Name = "acquisitionResultsGroupBox"
        Me.acquisitionResultsGroupBox.Size = New System.Drawing.Size(256, 224)
        Me.acquisitionResultsGroupBox.TabIndex = 6
        Me.acquisitionResultsGroupBox.TabStop = False
        Me.acquisitionResultsGroupBox.Text = "Acquisition Results:"
        '
        'resultLabel
        '
        Me.resultLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.resultLabel.Location = New System.Drawing.Point(16, 16)
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
        Me.acquisitionDataGrid.Size = New System.Drawing.Size(232, 176)
        Me.acquisitionDataGrid.TabIndex = 1
        Me.acquisitionDataGrid.TabStop = False
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(514, 376)
        Me.Controls.Add(Me.rtdParametersGroupBox)
        Me.Controls.Add(Me.resistanceParametersGroupBox)
        Me.Controls.Add(Me.stopButton)
        Me.Controls.Add(Me.timingParametersGroupBox)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.channelParametersGroupBox)
        Me.Controls.Add(Me.acquisitionResultsGroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Continuous Acquire RTD Samples - Internal Clock"
        Me.rtdParametersGroupBox.ResumeLayout(False)
        CType(Me.r0Numeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.resistanceParametersGroupBox.ResumeLayout(False)
        CType(Me.currentValueNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.timingParametersGroupBox.ResumeLayout(False)
        CType(Me.rateNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.channelParametersGroupBox.ResumeLayout(False)
        CType(Me.minimumValueNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.maximumValueNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.acquisitionResultsGroupBox.ResumeLayout(False)
        CType(Me.acquisitionDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub startButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles startButton.Click

        Try
            myTask = New Task()
            Dim rtdType As AIRtdType
            Dim resistanceConfiguration As AIResistanceConfiguration
            Dim excitationSource As AIExcitationSource

            Select Case rtdTypeComboBox.SelectedIndex
                Case 0
                    rtdType = AIRtdType.Custom
                Case 1
                    rtdType = AIRtdType.Pt3750
                Case 2
                    rtdType = AIRtdType.Pt3851
                Case 3
                    rtdType = AIRtdType.Pt3911
                Case 4
                    rtdType = AIRtdType.Pt3916
                Case 5
                    rtdType = AIRtdType.Pt3920
                Case 6
                    rtdType = AIRtdType.Pt3928
            End Select

            Select Case resistanceConfigurationComboBox.SelectedIndex
                Case 0
                    resistanceConfiguration = AIResistanceConfiguration.TwoWire
                Case 1
                    resistanceConfiguration = AIResistanceConfiguration.ThreeWire
                Case 2
                    resistanceConfiguration = AIResistanceConfiguration.FourWire
            End Select

            Select Case currentExcitationComboBox.SelectedIndex
                Case 0
                    excitationSource = AIExcitationSource.None
                Case 1
                    excitationSource = AIExcitationSource.Internal
                Case 2
                    excitationSource = AIExcitationSource.External
            End Select

            myTask.AIChannels.CreateRtdChannel(physicalChannelComboBox.Text, "", _
                Convert.ToDouble(minimumValueNumeric.Value), Convert.ToDouble(maximumValueNumeric.Value), _
                AITemperatureUnits.DegreesC, rtdType, resistanceConfiguration, _
                excitationSource, Convert.ToDouble(currentValueNumeric.Value), _
                Convert.ToDouble(r0Numeric.Value))

            myTask.Timing.ConfigureSampleClock("", Convert.ToDouble(rateNumeric.Value), _
                SampleClockActiveEdge.Rising, SampleQuantityMode.ContinuousSamples, 1000)

            myTask.Control(TaskAction.Verify)

            InitializeDataTable(myTask.AIChannels, dataTable)
            acquisitionDataGrid.DataSource = dataTable

            runningTask = myTask
            myReader = New AnalogMultiChannelReader(myTask.Stream)

            ' Use SynchronizeCallbacks to specify that the object 
            ' marshals callbacks across threads appropriately.
            myReader.SynchronizeCallbacks = True
            myReader.BeginReadWaveform(10, myAsyncCallback, myTask)

            startButton.Enabled = False
            stopButton.Enabled = True
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
            If (Not (runningTask Is Nothing)) AndAlso ar.AsyncState Is runningTask Then
                data = myReader.EndReadWaveform(ar)
                dataToDataTable(data, dataTable)
                myReader.BeginMemoryOptimizedReadWaveform(10, myAsyncCallback, myTask, data)
            End If
        Catch exception As DaqException
            MessageBox.Show(exception.Message)
            myTask.Dispose()
            startButton.Enabled = True
            stopButton.Enabled = False
            runningTask = Nothing
        End Try
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
