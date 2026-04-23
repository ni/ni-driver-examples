'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   ContAcqVoltageSamples_IntClk_SWTrigger
'
' Category:
'   AI
'
' Description:
'   This example demonstrates how to perform an analog software triggered
'   acquisition.  The example allows the user to specify the triggering
'   condition and the number of pre-trigger samples to acquire.
'
' Instructions for running:
'   1.  Select the physical channels corresponding to where your signals are
'       input on the DAQ device.
'   2.  Enter the minimum and maximum voltage ranges.NOTE:  For better accuracy,
'       try to match the input ranges to the expected voltage levels of the
'       measured signals.
'   3.  Specify the rate of the sample clock in Hz.
'   4.  Specify the number of samples to read per iteration of the continuous
'       input operation.  These samples will then be monitored for the trigger
'       condition.
'   5.  Specify the index of the channel that will be monitored for the
'       requested condition.  The first channel in the scan list will have an
'       index equal to zero.
'   6.  Set the triggering condition to be applied to the channel specified by
'       the channel index.  
'       The trigger conditions for this example are:   - Above Level:  The trigger
'       condition is met as soon as 
'       the monitored signal is above the specified level.    - Below Level: The
'       trigger condition is met as 
'       soon as the monitored signal is below the specified level.   - Rising
'       Edge: The trigger condition is 
'       met as soon as the monitored signal starts below the hysteresis window and
'       then rises past the specified 
'       level.   - Falling Edge: The trigger condition is met as soon as the
'       monitored signal starts above the 
'       hysteresis window and then falls below the specified level.   - Inside
'       Window: The trigger condition 
'       is met as soon as the monitored signal is within the specified window.   -
'       Outside Window: The trigger 
'       condition is met as soon as the monitored signal is outside of the
'       specified window.
'   7.  Set the level value to be used by the selected condition.
'   8.  Set the window amplitude / hysteresis to be used by the trigger
'       conditions:   - The upper limit of the conditional window is calculated
'       by adding the window amplitude to the condition level.   - The lower
'       limit of the conditional window is calculated by subtracting the window
'       amplitude from the condition level.   - The hysteresis window for the
'       Rising Edge condition is defined as the level minus the hysteresis
'       value.   - The hysteresis window for the Falling Edge condition is
'       defined as the level plus the hysteresis value.
'   9.  Set the number of pre-trigger samples.  The first voltage samples
'       displayed will be for these pre-trigger values, or zeroes if not enough
'       samples are read before the trigger fires to fill up the pre-trigger
'       samples buffer.
'
' Steps:
'   1.  Create a new analog input task.
'   2.  Create the analog input voltage channels.
'   3.  Configure the timing for the acquisition.  In this example we use the
'       DAQ device's internal clock to take a continuous number of samples.
'   4.  Verify the task.
'   5.  Prepare the number of pretrigger samples to read.
'   6.  Create a AnalogMultiChannelReader and associate it with the task by
'       using the task's stream. Call
'       AnalogMultiChannelReader.BeginBeginReadMultiSample to install a callback
'       and begin the asynchronous read operation.
'   7.  Inside the callback, analyze the data for the trigger condition.  If one
'       exists, display the pretrigger samples and the post-trigger samples. 
'       Otherwise, call AnalogMultiChannelReader.BeginReadMultiSample to perform
'       another read.
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
'   Make sure your signal input terminals match the physical I/O control.  In
'   the default case (differential channel ai0), wire the positive lead for your
'   signal to the ACH0 pin on your DAQ device and wire the negative lead for
'   your signal to the ACH8 pin.  For more information on the input and output
'   terminals for your device, open the NI-DAQmx Help and refer to the NI-DAQmx
'   Device Terminals and Device Considerations books in the table of contents.
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

Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Data
Imports NationalInstruments
Imports NationalInstruments.DAQmx

Public Class MainForm
    Inherits System.Windows.Forms.Form
    Private myTask As Task
    Private reader As AnalogMultiChannelReader
    Private dataColumn As dataColumn() = Nothing
    Private dataTable As dataTable = Nothing
    Private pretrigger(,) As Double
    Private ptSize As Integer
    Private samples As Integer
    Private pretriggerSamples As Integer
    Private ptSaved As Integer

    Private runningTask As Task

    Friend WithEvents channelGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents timingGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents triggeringGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents physicalChannelLabel As System.Windows.Forms.Label
    Friend WithEvents physicalChannelComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents maximumLabel As System.Windows.Forms.Label
    Friend WithEvents maximumNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents minumumLabel As System.Windows.Forms.Label
    Friend WithEvents minimumNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents samplesLabel As System.Windows.Forms.Label
    Friend WithEvents rateLabel As System.Windows.Forms.Label
    Friend WithEvents samplesNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents rateNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents indexNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents indexLabel As System.Windows.Forms.Label
    Friend WithEvents conditionLabel As System.Windows.Forms.Label
    Friend WithEvents conditionComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents windowLabel As System.Windows.Forms.Label
    Friend WithEvents levelLabel As System.Windows.Forms.Label
    Friend WithEvents windowNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents levelNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents pretriggerLabel As System.Windows.Forms.Label
    Friend WithEvents pretriggerNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents startButton As System.Windows.Forms.Button
    Friend WithEvents stopButton As System.Windows.Forms.Button
    Friend WithEvents acquiredGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents resultsDataGrid As System.Windows.Forms.DataGrid

    Private components As System.ComponentModel.Container = Nothing

    Public Sub New()
        Application.EnableVisualStyles()
        Application.DoEvents()
        InitializeComponent()

        ' Initialize UI
        physicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AI, PhysicalChannelAccess.External))
        If physicalChannelComboBox.Items.Count > 0 Then
            physicalChannelComboBox.SelectedIndex = 0
        End If
        conditionComboBox.SelectedIndex = 0

        dataTable = New DataTable
    End Sub 'New

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
    End Sub 'Dispose
    Friend WithEvents pretriggerAcquiredLabel As System.Windows.Forms.Label
    Friend WithEvents pretriggerAcquiredNumeric As System.Windows.Forms.NumericUpDown

    Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MainForm))
        Me.channelGroupBox = New System.Windows.Forms.GroupBox
        Me.maximumNumeric = New System.Windows.Forms.NumericUpDown
        Me.physicalChannelLabel = New System.Windows.Forms.Label
        Me.physicalChannelComboBox = New System.Windows.Forms.ComboBox
        Me.maximumLabel = New System.Windows.Forms.Label
        Me.minumumLabel = New System.Windows.Forms.Label
        Me.minimumNumeric = New System.Windows.Forms.NumericUpDown
        Me.timingGroupBox = New System.Windows.Forms.GroupBox
        Me.samplesLabel = New System.Windows.Forms.Label
        Me.rateLabel = New System.Windows.Forms.Label
        Me.samplesNumeric = New System.Windows.Forms.NumericUpDown
        Me.rateNumeric = New System.Windows.Forms.NumericUpDown
        Me.triggeringGroupBox = New System.Windows.Forms.GroupBox
        Me.indexNumeric = New System.Windows.Forms.NumericUpDown
        Me.indexLabel = New System.Windows.Forms.Label
        Me.conditionLabel = New System.Windows.Forms.Label
        Me.conditionComboBox = New System.Windows.Forms.ComboBox
        Me.windowLabel = New System.Windows.Forms.Label
        Me.levelLabel = New System.Windows.Forms.Label
        Me.windowNumeric = New System.Windows.Forms.NumericUpDown
        Me.levelNumeric = New System.Windows.Forms.NumericUpDown
        Me.pretriggerLabel = New System.Windows.Forms.Label
        Me.pretriggerNumeric = New System.Windows.Forms.NumericUpDown
        Me.startButton = New System.Windows.Forms.Button
        Me.stopButton = New System.Windows.Forms.Button
        Me.acquiredGroupBox = New System.Windows.Forms.GroupBox
        Me.resultsDataGrid = New System.Windows.Forms.DataGrid
        Me.pretriggerAcquiredLabel = New System.Windows.Forms.Label
        Me.pretriggerAcquiredNumeric = New System.Windows.Forms.NumericUpDown
        Me.channelGroupBox.SuspendLayout()
        CType(Me.maximumNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.minimumNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.timingGroupBox.SuspendLayout()
        CType(Me.samplesNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rateNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.triggeringGroupBox.SuspendLayout()
        CType(Me.indexNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.windowNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.levelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pretriggerNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.acquiredGroupBox.SuspendLayout()
        CType(Me.resultsDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pretriggerAcquiredNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'channelGroupBox
        '
        Me.channelGroupBox.Controls.Add(Me.maximumNumeric)
        Me.channelGroupBox.Controls.Add(Me.physicalChannelLabel)
        Me.channelGroupBox.Controls.Add(Me.physicalChannelComboBox)
        Me.channelGroupBox.Controls.Add(Me.maximumLabel)
        Me.channelGroupBox.Controls.Add(Me.minumumLabel)
        Me.channelGroupBox.Controls.Add(Me.minimumNumeric)
        Me.channelGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.channelGroupBox.Location = New System.Drawing.Point(8, 8)
        Me.channelGroupBox.Name = "channelGroupBox"
        Me.channelGroupBox.Size = New System.Drawing.Size(272, 120)
        Me.channelGroupBox.TabIndex = 0
        Me.channelGroupBox.TabStop = False
        Me.channelGroupBox.Text = "Channel Parameters"
        '
        'maximumNumeric
        '
        Me.maximumNumeric.DecimalPlaces = 2
        Me.maximumNumeric.Location = New System.Drawing.Point(128, 57)
        Me.maximumNumeric.Minimum = New Decimal(New Integer() {100, 0, 0, -2147483648})
        Me.maximumNumeric.Name = "maximumNumeric"
        Me.maximumNumeric.TabIndex = 3
        Me.maximumNumeric.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'physicalChannelLabel
        '
        Me.physicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.physicalChannelLabel.Location = New System.Drawing.Point(8, 27)
        Me.physicalChannelLabel.Name = "physicalChannelLabel"
        Me.physicalChannelLabel.Size = New System.Drawing.Size(104, 16)
        Me.physicalChannelLabel.TabIndex = 0
        Me.physicalChannelLabel.Text = "Physical Channels:"
        '
        'physicalChannelComboBox
        '
        Me.physicalChannelComboBox.Location = New System.Drawing.Point(128, 25)
        Me.physicalChannelComboBox.Name = "physicalChannelComboBox"
        Me.physicalChannelComboBox.Size = New System.Drawing.Size(120, 21)
        Me.physicalChannelComboBox.TabIndex = 1
        Me.physicalChannelComboBox.Text = "Dev1/ai0"
        '
        'maximumLabel
        '
        Me.maximumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.maximumLabel.Location = New System.Drawing.Point(8, 59)
        Me.maximumLabel.Name = "maximumLabel"
        Me.maximumLabel.Size = New System.Drawing.Size(112, 16)
        Me.maximumLabel.TabIndex = 2
        Me.maximumLabel.Text = "Maximum Value (V):"
        '
        'minumumLabel
        '
        Me.minumumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.minumumLabel.Location = New System.Drawing.Point(8, 90)
        Me.minumumLabel.Name = "minumumLabel"
        Me.minumumLabel.Size = New System.Drawing.Size(112, 16)
        Me.minumumLabel.TabIndex = 4
        Me.minumumLabel.Text = "Minimum Value (V):"
        '
        'minimumNumeric
        '
        Me.minimumNumeric.DecimalPlaces = 2
        Me.minimumNumeric.Location = New System.Drawing.Point(128, 88)
        Me.minimumNumeric.Minimum = New Decimal(New Integer() {100, 0, 0, -2147483648})
        Me.minimumNumeric.Name = "minimumNumeric"
        Me.minimumNumeric.TabIndex = 5
        Me.minimumNumeric.Value = New Decimal(New Integer() {10, 0, 0, -2147483648})
        '
        'timingGroupBox
        '
        Me.timingGroupBox.Controls.Add(Me.samplesLabel)
        Me.timingGroupBox.Controls.Add(Me.rateLabel)
        Me.timingGroupBox.Controls.Add(Me.samplesNumeric)
        Me.timingGroupBox.Controls.Add(Me.rateNumeric)
        Me.timingGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.timingGroupBox.Location = New System.Drawing.Point(8, 136)
        Me.timingGroupBox.Name = "timingGroupBox"
        Me.timingGroupBox.Size = New System.Drawing.Size(272, 88)
        Me.timingGroupBox.TabIndex = 1
        Me.timingGroupBox.TabStop = False
        Me.timingGroupBox.Text = "Timing Parameters"
        '
        'samplesLabel
        '
        Me.samplesLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.samplesLabel.Location = New System.Drawing.Point(8, 59)
        Me.samplesLabel.Name = "samplesLabel"
        Me.samplesLabel.Size = New System.Drawing.Size(120, 16)
        Me.samplesLabel.TabIndex = 2
        Me.samplesLabel.Text = "Samples per Channel:"
        '
        'rateLabel
        '
        Me.rateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.rateLabel.Location = New System.Drawing.Point(8, 27)
        Me.rateLabel.Name = "rateLabel"
        Me.rateLabel.Size = New System.Drawing.Size(112, 16)
        Me.rateLabel.TabIndex = 0
        Me.rateLabel.Text = "Sample Rate (Hz):"
        '
        'samplesNumeric
        '
        Me.samplesNumeric.Location = New System.Drawing.Point(128, 57)
        Me.samplesNumeric.Maximum = New Decimal(New Integer() {1410065408, 2, 0, 0})
        Me.samplesNumeric.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.samplesNumeric.Name = "samplesNumeric"
        Me.samplesNumeric.TabIndex = 3
        Me.samplesNumeric.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        'rateNumeric
        '
        Me.rateNumeric.DecimalPlaces = 2
        Me.rateNumeric.Location = New System.Drawing.Point(128, 25)
        Me.rateNumeric.Maximum = New Decimal(New Integer() {1410065408, 2, 0, 0})
        Me.rateNumeric.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.rateNumeric.Name = "rateNumeric"
        Me.rateNumeric.TabIndex = 1
        Me.rateNumeric.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        'triggeringGroupBox
        '
        Me.triggeringGroupBox.Controls.Add(Me.indexNumeric)
        Me.triggeringGroupBox.Controls.Add(Me.indexLabel)
        Me.triggeringGroupBox.Controls.Add(Me.conditionLabel)
        Me.triggeringGroupBox.Controls.Add(Me.conditionComboBox)
        Me.triggeringGroupBox.Controls.Add(Me.windowLabel)
        Me.triggeringGroupBox.Controls.Add(Me.levelLabel)
        Me.triggeringGroupBox.Controls.Add(Me.windowNumeric)
        Me.triggeringGroupBox.Controls.Add(Me.levelNumeric)
        Me.triggeringGroupBox.Controls.Add(Me.pretriggerLabel)
        Me.triggeringGroupBox.Controls.Add(Me.pretriggerNumeric)
        Me.triggeringGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.triggeringGroupBox.Location = New System.Drawing.Point(8, 232)
        Me.triggeringGroupBox.Name = "triggeringGroupBox"
        Me.triggeringGroupBox.Size = New System.Drawing.Size(272, 184)
        Me.triggeringGroupBox.TabIndex = 2
        Me.triggeringGroupBox.TabStop = False
        Me.triggeringGroupBox.Text = "Triggering Parameters"
        '
        'indexNumeric
        '
        Me.indexNumeric.Location = New System.Drawing.Point(128, 25)
        Me.indexNumeric.Maximum = New Decimal(New Integer() {511, 0, 0, 0})
        Me.indexNumeric.Name = "indexNumeric"
        Me.indexNumeric.TabIndex = 1
        '
        'indexLabel
        '
        Me.indexLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.indexLabel.Location = New System.Drawing.Point(8, 27)
        Me.indexLabel.Name = "indexLabel"
        Me.indexLabel.Size = New System.Drawing.Size(120, 16)
        Me.indexLabel.TabIndex = 0
        Me.indexLabel.Text = "Trigger Channel Index:"
        '
        'conditionLabel
        '
        Me.conditionLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.conditionLabel.Location = New System.Drawing.Point(8, 59)
        Me.conditionLabel.Name = "conditionLabel"
        Me.conditionLabel.Size = New System.Drawing.Size(104, 16)
        Me.conditionLabel.TabIndex = 2
        Me.conditionLabel.Text = "Trigger Condition:"
        '
        'conditionComboBox
        '
        Me.conditionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.conditionComboBox.Items.AddRange(New Object() {"Above Level", "Below Level", "Rising Edge", "Falling Edge", "Inside Window", "Outside Window"})
        Me.conditionComboBox.Location = New System.Drawing.Point(128, 57)
        Me.conditionComboBox.Name = "conditionComboBox"
        Me.conditionComboBox.Size = New System.Drawing.Size(120, 21)
        Me.conditionComboBox.TabIndex = 3
        '
        'windowLabel
        '
        Me.windowLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.windowLabel.Location = New System.Drawing.Point(8, 123)
        Me.windowLabel.Name = "windowLabel"
        Me.windowLabel.Size = New System.Drawing.Size(112, 16)
        Me.windowLabel.TabIndex = 6
        Me.windowLabel.Text = "Window/Hysteresis (V):"
        '
        'levelLabel
        '
        Me.levelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.levelLabel.Location = New System.Drawing.Point(8, 91)
        Me.levelLabel.Name = "levelLabel"
        Me.levelLabel.Size = New System.Drawing.Size(112, 16)
        Me.levelLabel.TabIndex = 4
        Me.levelLabel.Text = "Level (V):"
        '
        'windowNumeric
        '
        Me.windowNumeric.DecimalPlaces = 2
        Me.windowNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.windowNumeric.Location = New System.Drawing.Point(128, 121)
        Me.windowNumeric.Name = "windowNumeric"
        Me.windowNumeric.TabIndex = 7
        Me.windowNumeric.Value = New Decimal(New Integer() {1, 0, 0, 65536})
        '
        'levelNumeric
        '
        Me.levelNumeric.DecimalPlaces = 2
        Me.levelNumeric.Location = New System.Drawing.Point(128, 89)
        Me.levelNumeric.Minimum = New Decimal(New Integer() {100, 0, 0, -2147483648})
        Me.levelNumeric.Name = "levelNumeric"
        Me.levelNumeric.TabIndex = 5
        Me.levelNumeric.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'pretriggerLabel
        '
        Me.pretriggerLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.pretriggerLabel.Location = New System.Drawing.Point(8, 155)
        Me.pretriggerLabel.Name = "pretriggerLabel"
        Me.pretriggerLabel.Size = New System.Drawing.Size(112, 16)
        Me.pretriggerLabel.TabIndex = 8
        Me.pretriggerLabel.Text = "Pretrigger Samples:"
        '
        'pretriggerNumeric
        '
        Me.pretriggerNumeric.Location = New System.Drawing.Point(128, 153)
        Me.pretriggerNumeric.Maximum = New Decimal(New Integer() {1410065408, 2, 0, 0})
        Me.pretriggerNumeric.Name = "pretriggerNumeric"
        Me.pretriggerNumeric.TabIndex = 9
        Me.pretriggerNumeric.Value = New Decimal(New Integer() {5, 0, 0, 0})
        '
        'startButton
        '
        Me.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.startButton.Location = New System.Drawing.Point(48, 424)
        Me.startButton.Name = "startButton"
        Me.startButton.TabIndex = 3
        Me.startButton.Text = "Start"
        '
        'stopButton
        '
        Me.stopButton.Enabled = False
        Me.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.stopButton.Location = New System.Drawing.Point(152, 424)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.TabIndex = 4
        Me.stopButton.Text = "Stop"
        '
        'acquiredGroupBox
        '
        Me.acquiredGroupBox.Controls.Add(Me.resultsDataGrid)
        Me.acquiredGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.acquiredGroupBox.Location = New System.Drawing.Point(288, 8)
        Me.acquiredGroupBox.Name = "acquiredGroupBox"
        Me.acquiredGroupBox.Size = New System.Drawing.Size(320, 408)
        Me.acquiredGroupBox.TabIndex = 5
        Me.acquiredGroupBox.TabStop = False
        Me.acquiredGroupBox.Text = "Acquired Data"
        '
        'resultsDataGrid
        '
        Me.resultsDataGrid.DataMember = ""
        Me.resultsDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.resultsDataGrid.Location = New System.Drawing.Point(3, 16)
        Me.resultsDataGrid.Name = "resultsDataGrid"
        Me.resultsDataGrid.Size = New System.Drawing.Size(309, 384)
        Me.resultsDataGrid.TabIndex = 0
        '
        'pretriggerAcquiredLabel
        '
        Me.pretriggerAcquiredLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.pretriggerAcquiredLabel.Location = New System.Drawing.Point(296, 424)
        Me.pretriggerAcquiredLabel.Name = "pretriggerAcquiredLabel"
        Me.pretriggerAcquiredLabel.Size = New System.Drawing.Size(136, 16)
        Me.pretriggerAcquiredLabel.TabIndex = 8
        Me.pretriggerAcquiredLabel.Text = "Pretrigger Samples Acquired:"
        '
        'pretriggerAcquiredNumeric
        '
        Me.pretriggerAcquiredNumeric.Enabled = False
        Me.pretriggerAcquiredNumeric.Location = New System.Drawing.Point(488, 422)
        Me.pretriggerAcquiredNumeric.Maximum = New Decimal(New Integer() {1410065408, 2, 0, 0})
        Me.pretriggerAcquiredNumeric.Name = "pretriggerAcquiredNumeric"
        Me.pretriggerAcquiredNumeric.TabIndex = 9
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(618, 456)
        Me.Controls.Add(Me.acquiredGroupBox)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.channelGroupBox)
        Me.Controls.Add(Me.timingGroupBox)
        Me.Controls.Add(Me.triggeringGroupBox)
        Me.Controls.Add(Me.stopButton)
        Me.Controls.Add(Me.pretriggerAcquiredLabel)
        Me.Controls.Add(Me.pretriggerAcquiredNumeric)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Continuous Acquisition of Voltage Samples - Internal Clock - SW Trigger"
        Me.channelGroupBox.ResumeLayout(False)
        CType(Me.maximumNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.minimumNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.timingGroupBox.ResumeLayout(False)
        CType(Me.samplesNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rateNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.triggeringGroupBox.ResumeLayout(False)
        CType(Me.indexNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.windowNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.levelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pretriggerNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.acquiredGroupBox.ResumeLayout(False)
        CType(Me.resultsDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pretriggerAcquiredNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub 'InitializeComponent

    Private Sub startButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles startButton.Click
        Try
            ' Create the task
            myTask = New Task()

            ' Create channels
            myTask.AIChannels.CreateVoltageChannel(physicalChannelComboBox.Text, "", CType(-1, AITerminalConfiguration), Convert.ToDouble(minimumNumeric.Value), Convert.ToDouble(maximumNumeric.Value), AIVoltageUnits.Volts)

            ' Set up timing
            myTask.Timing.ConfigureSampleClock("", Convert.ToDouble(rateNumeric.Value), SampleClockActiveEdge.Rising, SampleQuantityMode.ContinuousSamples, 1000)

            ' Verify the task
            myTask.Control(TaskAction.Verify)

            ' Prepare pretrigger samples
            samples = Convert.ToInt32(samplesNumeric.Value)
            pretriggerSamples = Convert.ToInt32(pretriggerNumeric.Value)
            ptSaved = 0
            pretriggerAcquiredNumeric.Value = 0

            If pretriggerSamples <= samples Then
                pretrigger = New Double(myTask.AIChannels.Count - 1, samples - 1) {}
                ptSize = samples
            Else
                ' Make the size of the pretrigger buffer the smallest multiple of samples that is greater
                ' than the requested pretrigger samples
                ptSize = (CInt(pretriggerSamples / samples) + 1) * samples
                pretrigger = New Double(myTask.AIChannels.Count - 1, ptSize - 1) {}
            End If

            ' Prepare the table for data
            InitializeDataTable()
            resultsDataGrid.DataSource = dataTable

            ' Read the data
            runningTask = myTask
            startButton.Enabled = False
            stopButton.Enabled = True
            EnableControls(False)

            reader = New AnalogMultiChannelReader(myTask.Stream)

            ' Use SynchronizeCallbacks to specify that the object 
            ' marshals callbacks across threads appropriately.
            reader.SynchronizeCallbacks = True

            reader.BeginReadMultiSample(Convert.ToInt32(samplesNumeric.Value), New AsyncCallback(AddressOf ReadData), myTask)
        Catch ex As DaqException
            MessageBox.Show(ex.Message)
            myTask.Dispose()
            startButton.Enabled = True
            stopButton.Enabled = False
            EnableControls(True)
            runningTask = Nothing
        End Try
    End Sub 'startButton_Click

    Private Sub ReadData(ByVal ar As IAsyncResult)
        Try
            If (Not (runningTask Is Nothing)) AndAlso runningTask Is ar.AsyncState Then
                ' Read the data
                Dim data As Double(,) = reader.EndReadMultiSample(ar)

                ' Get the channel index
                Dim index As Integer = Convert.ToInt32(indexNumeric.Value)

                If index < 0 Or index >= myTask.AIChannels.Count Then
                    MessageBox.Show("Invalid channel index.")
                    myTask.Dispose()
                    startButton.Enabled = True
                    stopButton.Enabled = False
                    EnableControls(True)
                    runningTask = Nothing
                    Return
                End If

                ' Analyze the data for a start trigger
                Dim level As Double = Convert.ToDouble(levelNumeric.Value)
                Dim window As Double = Convert.ToDouble(windowNumeric.Value)
                Dim triggerLocation As Integer = FindTrigger(data, index, level, window)
                Dim iData As Integer
                Dim iChan As Integer

                ' Read the next set of data
                If triggerLocation <> -1 Then
                    ' Found a trigger
                    Dim iDisplay As Integer = 0

                    ' Display pretrigger samples
                    If pretriggerSamples > triggerLocation Then
                        ' Figure out how many samples we need from the pretrigger buffer
                        Dim deficit As Integer = pretriggerSamples - triggerLocation

                        ' Display samples from pretrigger buffer
                        For iData = 0 To deficit - 1
                            For iChan = 0 To myTask.AIChannels.Count - 1
                                dataTable.Rows(iDisplay)(iChan) = pretrigger(iChan, iData + ptSize - deficit)
                            Next iChan

                            iDisplay += 1
                        Next iData

                        ' Now include all samples up to the trigger location in data
                        For iData = 0 To triggerLocation - 1
                            For iChan = 0 To myTask.AIChannels.Count - 1
                                dataTable.Rows(iDisplay)(iChan) = data(iChan, iData)
                            Next iChan

                            iDisplay += 1
                        Next iData

                        If ptSaved + triggerLocation > pretriggerSamples Then
                            pretriggerAcquiredNumeric.Value = pretriggerSamples
                        Else
                            pretriggerAcquiredNumeric.Value = ptSaved + triggerLocation
                        End If

                    Else ' pretriggerSamples <= triggerLocation
                        ' We have enough pretrigger samples in the current data array
                        For iData = 0 To pretriggerSamples - 1
                            For iChan = 0 To myTask.AIChannels.Count - 1
                                dataTable.Rows(iDisplay)(iChan) = data(iChan, iData + triggerLocation - pretriggerSamples)
                            Next iChan

                            iDisplay += 1
                        Next iData

                        pretriggerAcquiredNumeric.Value = pretriggerSamples
                    End If

                    ' Display data after the trigger
                    For iData = triggerLocation To samples - 1
                        For iChan = 0 To myTask.AIChannels.Count - 1
                            dataTable.Rows(iDisplay)(iChan) = data(iChan, iData)
                        Next iChan

                        iDisplay += 1
                    Next iData

                    ' Read more data
                    data = reader.ReadMultiSample(triggerLocation)

                    ' Display additional data after trigger
                    For iData = 0 To triggerLocation - 1
                        For iChan = 0 To myTask.AIChannels.Count - 1
                            dataTable.Rows(iDisplay)(iChan) = data(iChan, iData)
                        Next iChan

                        iDisplay += 1
                    Next iData

                    ' Stop the task
                    myTask.Dispose()
                    startButton.Enabled = True
                    stopButton.Enabled = False
                    EnableControls(True)
                    runningTask = Nothing

                Else ' triggerLocation == -1
                    ' Trigger not found; save pretrigger samples
                    If pretriggerSamples <= samples Then
                        ' Save only one iteration (over all channels)
                        For iChan = 0 To myTask.AIChannels.Count - 1
                            For iData = 0 To samples - 1
                                pretrigger(iChan, iData) = data(iChan, iData)
                            Next iData
                        Next iChan

                    Else ' pretriggerSamples > samples
                        ' Save over multiple iterations
                        Dim offset As Integer = ptSize - samples

                        ' Shift elements in the array (discarding the first samples of data)
                        For iChan = 0 To myTask.AIChannels.Count - 1
                            For iData = 0 To offset - 1
                                pretrigger(iChan, iData) = pretrigger(iChan, iData + samples)
                            Next iData
                        Next iChan

                        ' Copy the new data into the array
                        For iChan = 0 To myTask.AIChannels.Count - 1
                            For iData = 0 To samples - 1
                                pretrigger(iChan, iData + offset) = data(iChan, iData)
                            Next iData
                        Next iChan
                    End If

                    ptSaved += samples

                    ' Read the next set of samples
                    reader.BeginReadMultiSample(Convert.ToInt32(samplesNumeric.Value), New AsyncCallback(AddressOf ReadData), myTask)
                End If
            End If
        Catch ex As DaqException
            MessageBox.Show(ex.Message)
            myTask.Dispose()
            startButton.Enabled = True
            stopButton.Enabled = False
            EnableControls(True)
            runningTask = Nothing
        End Try
    End Sub 'ReadData

    Private Function FindTrigger(ByVal data(,) As Double, ByVal index As Integer, ByVal level As Double, ByVal window As Double) As Integer
        Dim triggerLocation As Integer = -1
        Dim i As Integer

        If conditionComboBox.Text = "Rising Edge" Then
            ' Find a value less than (level - window)
            For i = 0 To (data.GetLength(1)) - 1
                If data(index, i) < level - window Then
                    Exit For
                End If
            Next i

            ' Then do an "above level" search

            While i < data.GetLength(1)
                If data(index, i) > level Then
                    triggerLocation = i
                    Exit While
                End If
                i += 1
            End While
        ElseIf conditionComboBox.Text = "Falling Edge" Then
            ' Find a value greater than (level + window)
            For i = 0 To (data.GetLength(1)) - 1
                If data(index, i) > level + window Then
                    Exit For
                End If
            Next i

            ' Then do a "below level" search

            While i < data.GetLength(1)
                If data(index, i) < level Then
                    triggerLocation = i
                    Exit While
                End If
                i += 1
            End While
        ElseIf conditionComboBox.Text = "Above Level" Then
            ' Trigger if we find a voltage above the level
            For i = 0 To (data.GetLength(1)) - 1
                If data(index, i) > level Then
                    triggerLocation = i
                    Exit For
                End If
            Next i
        ElseIf conditionComboBox.Text = "Below Level" Then
            ' Trigger if we find a voltage below the level
            For i = 0 To (data.GetLength(1)) - 1
                If data(index, i) < level Then
                    triggerLocation = i
                    Exit For
                End If
            Next i
        ElseIf conditionComboBox.Text = "Inside Window" Then
            ' Trigger if we find a voltage inside the window surrounding the level
            For i = 0 To (data.GetLength(1)) - 1
                If Math.Abs((data(index, i) - level)) < window Then
                    triggerLocation = i
                    Exit For
                End If
            Next i
        ElseIf conditionComboBox.Text = "Outside Window" Then
            ' Trigger if we find a voltage outside the window surrounding the level
            For i = 0 To (data.GetLength(1)) - 1
                If Math.Abs((data(index, i) - level)) > window Then
                    triggerLocation = i
                    Exit For
                End If
            Next i
        End If

        Return triggerLocation
    End Function 'FindTrigger

    Private Sub InitializeDataTable()
        dataTable.Rows.Clear()
        dataTable.Columns.Clear()
        dataColumn = New DataColumn(myTask.AIChannels.Count - 1) {}

        Dim iChan As Integer
        For iChan = 0 To myTask.AIChannels.Count - 1
            dataColumn(iChan) = New DataColumn
            dataColumn(iChan).DataType = GetType(Double)
            dataColumn(iChan).ColumnName = myTask.AIChannels(iChan).PhysicalName
        Next iChan

        dataTable.Columns.AddRange(dataColumn)

        Dim iData As Integer
        For iData = 0 To (samples + pretriggerSamples) - 1
            Dim rowArr(myTask.AIChannels.Count - 1) As Object
            dataTable.Rows.Add(rowArr)
        Next iData

        resultsDataGrid.Refresh()
    End Sub 'InitializeDataTable

    Private Sub EnableControls(ByVal enable As Boolean)
        physicalChannelComboBox.Enabled = enable
        maximumNumeric.Enabled = enable
        minimumNumeric.Enabled = enable
        rateNumeric.Enabled = enable
        samplesNumeric.Enabled = enable
        conditionComboBox.Enabled = enable
        levelNumeric.Enabled = enable
        windowNumeric.Enabled = enable
        indexNumeric.Enabled = enable
        pretriggerNumeric.Enabled = enable
    End Sub 'EnableControls

    Private Sub stopButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles stopButton.Click
        myTask.Dispose()
        runningTask = Nothing
        startButton.Enabled = True
        stopButton.Enabled = False
        EnableControls(True)
    End Sub 'stopButton_Click
End Class 'MainForm
