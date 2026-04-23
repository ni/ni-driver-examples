'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   ContReadDigChan_PipeSampClkwHshk
'
' Category:
'   DI
'
' Description:
'   This examples demostrates how to interface the NI 6536/7 to a synchonous
'   FIFO.
'
' Instructions for running:
'   1.  Select the Physical Channels that correspond to where your signal is
'       input on the device.
'   2.  Enter the number of Samples per Channel per Read.  This is the number of
'       samples that will be read every time the DAQmx Read function is called.
'   3.  Specify the Sample Clock Rate of the input waveform.
'   4.  Specify the Ready for Transfer Output Terminal.
'   5.  Specify whither the ready for transfer signal is active high or active
'       low.
'   6.  Specify the Ready for Transfer Deassert Condition Threshold.  This
'       specifies in samples the threshold below which the Ready for Transfer
'       Event deasserts.
'   7.  Specify the Pause Trigger Polarity.  This parameter tells this device
'       when to pause.  If the polarity is set to High, then the device will
'       pause when the corresponding PFI line is high.  Note, that the device
'       will not pause on the next sample clock edge because of pipelining.
'   8.  Specify the Pause Trigger Source Terminal.
'
' Steps:
'   1.  Create a task.
'   2.  Create one Digital Output channel for each Digital Line in the Task.
'   3.  Configure the Task to use a pipelined sampled clock.
'   4.  Configure the pause trigger.
'   5.  Configure the ready for transfer event. You need to configurethe ready
'       for transfer deassert threshold to correspond tohow many samples it
'       takes for the device connected to the NI6536/7 to pause the data
'       transfer.
'   6.  Disallow Overwrites. When overwrites are disallowed, the datatransfer
'       between the device and the DAQmx buffer will pausewhen the DAQmx buffer
'       is full. It will resume when more spaceis available in the buffer.
'   7.  Create a DigitalMultiChannelReader and associate it with the task
'       byusing the task's stream.
'   8.  Call DigitalMultiChannelReader.BeginReadWaveform to install a callback
'       and begin the asynchronous read operation.
'   9.  Inside the callback, call DigitalSingleChannelReader.EndReadWaveformto
'       retrieve the data from the read.
'   10. Display the acquired data and
'       callDigitalMultiChannelReader.BeginReadWaveform again inside the
'       callback to perform another read.
'   11. Handle any DaqExceptions, if they occur.
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
'   Connect the FIFO's Not Empty Flag to the Pause Trigger. Connectthe FIFO's
'   Read Enable signal to the Ready for transfer Event.Connect the FIFO's read
'   clock to the sample clock terminal.Connect the data lines from the NI 6536/7
'   to the data lines ofthe FIFO.
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

    Private myTask As Task
    Private runningTask As Task
    Private waveform As DigitalWaveform()
    Private asyncDigitalCallback As AsyncCallback
    Private Reader As DigitalMultiChannelReader
    Private dataColumn As dataColumn() = Nothing
    Private dataTable As DataTable

    Public Sub New()
        MyBase.New()

        Application.EnableVisualStyles()
        Application.DoEvents() '

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        startButton.Enabled = False
        dataTable = New dataTable

        physicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DOLine, PhysicalChannelAccess.External))
        If (physicalChannelComboBox.Items.Count > 0) Then
            physicalChannelComboBox.SelectedIndex = 0
            startButton.Enabled = True
        End If

        stopButton.Enabled = False

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
    Friend WithEvents resultsGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents resultsDataGrid As System.Windows.Forms.DataGrid
    Friend WithEvents startButton As System.Windows.Forms.Button
    Friend WithEvents handshakingParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents transferDeassertConditionThresholdNumericUpDown As System.Windows.Forms.NumericUpDown
    Friend WithEvents transferDeassertConditionThresholdLabel As System.Windows.Forms.Label
    Friend WithEvents pauseTriggerSourceTerminalTextBox As System.Windows.Forms.TextBox
    Friend WithEvents pauseTriggerSourceTerminalLabel As System.Windows.Forms.Label
    Friend WithEvents readyForTransferOutputTerminalTextBox As System.Windows.Forms.TextBox
    Friend WithEvents pauseTriggerPolarityComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents pauseTriggerPolarityLabel As System.Windows.Forms.Label
    Friend WithEvents readyForTransferLevelComboxBox As System.Windows.Forms.ComboBox
    Friend WithEvents readyForTransferLevelLabel As System.Windows.Forms.Label
    Friend WithEvents readyForTransferOutputTerminalLabel As System.Windows.Forms.Label
    Friend WithEvents channelParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents samplesPerBufferNumericUpDown As System.Windows.Forms.NumericUpDown
    Friend WithEvents samplesPerBufferLabel As System.Windows.Forms.Label
    Friend WithEvents physicalChannelComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents physicalChannelLabel As System.Windows.Forms.Label
    Friend WithEvents timingParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents samplesClockRateNumericUpDown As System.Windows.Forms.NumericUpDown
    Friend WithEvents sampleClockRateLabel As System.Windows.Forms.Label
    Friend WithEvents stopButton As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MainForm))
        Me.resultsGroupBox = New System.Windows.Forms.GroupBox
        Me.resultsDataGrid = New System.Windows.Forms.DataGrid
        Me.startButton = New System.Windows.Forms.Button
        Me.handshakingParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.transferDeassertConditionThresholdNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.transferDeassertConditionThresholdLabel = New System.Windows.Forms.Label
        Me.pauseTriggerSourceTerminalTextBox = New System.Windows.Forms.TextBox
        Me.pauseTriggerSourceTerminalLabel = New System.Windows.Forms.Label
        Me.readyForTransferOutputTerminalTextBox = New System.Windows.Forms.TextBox
        Me.pauseTriggerPolarityComboBox = New System.Windows.Forms.ComboBox
        Me.pauseTriggerPolarityLabel = New System.Windows.Forms.Label
        Me.readyForTransferLevelComboxBox = New System.Windows.Forms.ComboBox
        Me.readyForTransferLevelLabel = New System.Windows.Forms.Label
        Me.readyForTransferOutputTerminalLabel = New System.Windows.Forms.Label
        Me.channelParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.samplesPerBufferNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.samplesPerBufferLabel = New System.Windows.Forms.Label
        Me.physicalChannelComboBox = New System.Windows.Forms.ComboBox
        Me.physicalChannelLabel = New System.Windows.Forms.Label
        Me.timingParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.samplesClockRateNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.sampleClockRateLabel = New System.Windows.Forms.Label
        Me.stopButton = New System.Windows.Forms.Button
        Me.resultsGroupBox.SuspendLayout()
        CType(Me.resultsDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.handshakingParametersGroupBox.SuspendLayout()
        CType(Me.transferDeassertConditionThresholdNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.channelParametersGroupBox.SuspendLayout()
        CType(Me.samplesPerBufferNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.timingParametersGroupBox.SuspendLayout()
        CType(Me.samplesClockRateNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'resultsGroupBox
        '
        Me.resultsGroupBox.Controls.Add(Me.resultsDataGrid)
        Me.resultsGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.resultsGroupBox.Location = New System.Drawing.Point(384, 15)
        Me.resultsGroupBox.Name = "resultsGroupBox"
        Me.resultsGroupBox.Size = New System.Drawing.Size(336, 440)
        Me.resultsGroupBox.TabIndex = 9
        Me.resultsGroupBox.TabStop = False
        Me.resultsGroupBox.Text = "Results"
        '
        'resultsDataGrid
        '
        Me.resultsDataGrid.DataMember = ""
        Me.resultsDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.resultsDataGrid.Location = New System.Drawing.Point(8, 16)
        Me.resultsDataGrid.Name = "resultsDataGrid"
        Me.resultsDataGrid.Size = New System.Drawing.Size(320, 416)
        Me.resultsDataGrid.TabIndex = 0
        Me.resultsDataGrid.TabStop = False
        '
        'startButton
        '
        Me.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.startButton.Location = New System.Drawing.Point(86, 431)
        Me.startButton.Name = "startButton"
        Me.startButton.TabIndex = 8
        Me.startButton.Text = "Start"
        '
        'handshakingParametersGroupBox
        '
        Me.handshakingParametersGroupBox.Controls.Add(Me.transferDeassertConditionThresholdNumericUpDown)
        Me.handshakingParametersGroupBox.Controls.Add(Me.transferDeassertConditionThresholdLabel)
        Me.handshakingParametersGroupBox.Controls.Add(Me.pauseTriggerSourceTerminalTextBox)
        Me.handshakingParametersGroupBox.Controls.Add(Me.pauseTriggerSourceTerminalLabel)
        Me.handshakingParametersGroupBox.Controls.Add(Me.readyForTransferOutputTerminalTextBox)
        Me.handshakingParametersGroupBox.Controls.Add(Me.pauseTriggerPolarityComboBox)
        Me.handshakingParametersGroupBox.Controls.Add(Me.pauseTriggerPolarityLabel)
        Me.handshakingParametersGroupBox.Controls.Add(Me.readyForTransferLevelComboxBox)
        Me.handshakingParametersGroupBox.Controls.Add(Me.readyForTransferLevelLabel)
        Me.handshakingParametersGroupBox.Controls.Add(Me.readyForTransferOutputTerminalLabel)
        Me.handshakingParametersGroupBox.Location = New System.Drawing.Point(8, 199)
        Me.handshakingParametersGroupBox.Name = "handshakingParametersGroupBox"
        Me.handshakingParametersGroupBox.Size = New System.Drawing.Size(368, 224)
        Me.handshakingParametersGroupBox.TabIndex = 6
        Me.handshakingParametersGroupBox.TabStop = False
        Me.handshakingParametersGroupBox.Text = "Handshaking Parameters"
        '
        'transferDeassertConditionThresholdNumericUpDown
        '
        Me.transferDeassertConditionThresholdNumericUpDown.Location = New System.Drawing.Point(241, 104)
        Me.transferDeassertConditionThresholdNumericUpDown.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.transferDeassertConditionThresholdNumericUpDown.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.transferDeassertConditionThresholdNumericUpDown.Name = "transferDeassertConditionThresholdNumericUpDown"
        Me.transferDeassertConditionThresholdNumericUpDown.TabIndex = 5
        Me.transferDeassertConditionThresholdNumericUpDown.Value = New Decimal(New Integer() {256, 0, 0, 0})
        '
        'transferDeassertConditionThresholdLabel
        '
        Me.transferDeassertConditionThresholdLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.transferDeassertConditionThresholdLabel.Location = New System.Drawing.Point(8, 104)
        Me.transferDeassertConditionThresholdLabel.Name = "transferDeassertConditionThresholdLabel"
        Me.transferDeassertConditionThresholdLabel.Size = New System.Drawing.Size(224, 32)
        Me.transferDeassertConditionThresholdLabel.TabIndex = 4
        Me.transferDeassertConditionThresholdLabel.Text = "Ready for Transfer Deassert Condition Threshold:"
        '
        'pauseTriggerSourceTerminalTextBox
        '
        Me.pauseTriggerSourceTerminalTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pauseTriggerSourceTerminalTextBox.Location = New System.Drawing.Point(240, 192)
        Me.pauseTriggerSourceTerminalTextBox.Name = "pauseTriggerSourceTerminalTextBox"
        Me.pauseTriggerSourceTerminalTextBox.Size = New System.Drawing.Size(120, 20)
        Me.pauseTriggerSourceTerminalTextBox.TabIndex = 9
        Me.pauseTriggerSourceTerminalTextBox.Text = "/Dev1/PFI1"
        '
        'pauseTriggerSourceTerminalLabel
        '
        Me.pauseTriggerSourceTerminalLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.pauseTriggerSourceTerminalLabel.Location = New System.Drawing.Point(8, 192)
        Me.pauseTriggerSourceTerminalLabel.Name = "pauseTriggerSourceTerminalLabel"
        Me.pauseTriggerSourceTerminalLabel.Size = New System.Drawing.Size(200, 23)
        Me.pauseTriggerSourceTerminalLabel.TabIndex = 8
        Me.pauseTriggerSourceTerminalLabel.Text = "Pause Trigger Source Terminal:"
        '
        'readyForTransferOutputTerminalTextBox
        '
        Me.readyForTransferOutputTerminalTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.readyForTransferOutputTerminalTextBox.Location = New System.Drawing.Point(240, 62)
        Me.readyForTransferOutputTerminalTextBox.Name = "readyForTransferOutputTerminalTextBox"
        Me.readyForTransferOutputTerminalTextBox.Size = New System.Drawing.Size(120, 20)
        Me.readyForTransferOutputTerminalTextBox.TabIndex = 3
        Me.readyForTransferOutputTerminalTextBox.Text = "/Dev1/PFI0"
        '
        'pauseTriggerPolarityComboBox
        '
        Me.pauseTriggerPolarityComboBox.Items.AddRange(New Object() {"High", "Low"})
        Me.pauseTriggerPolarityComboBox.Location = New System.Drawing.Point(240, 152)
        Me.pauseTriggerPolarityComboBox.Name = "pauseTriggerPolarityComboBox"
        Me.pauseTriggerPolarityComboBox.Size = New System.Drawing.Size(121, 21)
        Me.pauseTriggerPolarityComboBox.TabIndex = 7
        Me.pauseTriggerPolarityComboBox.Text = "High"
        '
        'pauseTriggerPolarityLabel
        '
        Me.pauseTriggerPolarityLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.pauseTriggerPolarityLabel.Location = New System.Drawing.Point(8, 152)
        Me.pauseTriggerPolarityLabel.Name = "pauseTriggerPolarityLabel"
        Me.pauseTriggerPolarityLabel.Size = New System.Drawing.Size(200, 23)
        Me.pauseTriggerPolarityLabel.TabIndex = 6
        Me.pauseTriggerPolarityLabel.Text = "Pause Trigger Polarity (Pause When):"
        '
        'readyForTransferLevelComboxBox
        '
        Me.readyForTransferLevelComboxBox.Items.AddRange(New Object() {"Active High", "Active Low"})
        Me.readyForTransferLevelComboxBox.Location = New System.Drawing.Point(240, 19)
        Me.readyForTransferLevelComboxBox.Name = "readyForTransferLevelComboxBox"
        Me.readyForTransferLevelComboxBox.Size = New System.Drawing.Size(121, 21)
        Me.readyForTransferLevelComboxBox.TabIndex = 1
        Me.readyForTransferLevelComboxBox.Text = "Active Low"
        '
        'readyForTransferLevelLabel
        '
        Me.readyForTransferLevelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.readyForTransferLevelLabel.Location = New System.Drawing.Point(8, 24)
        Me.readyForTransferLevelLabel.Name = "readyForTransferLevelLabel"
        Me.readyForTransferLevelLabel.Size = New System.Drawing.Size(200, 23)
        Me.readyForTransferLevelLabel.TabIndex = 0
        Me.readyForTransferLevelLabel.Text = "Ready for Transfer Level:"
        '
        'readyForTransferOutputTerminalLabel
        '
        Me.readyForTransferOutputTerminalLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.readyForTransferOutputTerminalLabel.Location = New System.Drawing.Point(8, 64)
        Me.readyForTransferOutputTerminalLabel.Name = "readyForTransferOutputTerminalLabel"
        Me.readyForTransferOutputTerminalLabel.Size = New System.Drawing.Size(200, 23)
        Me.readyForTransferOutputTerminalLabel.TabIndex = 2
        Me.readyForTransferOutputTerminalLabel.Text = "Ready for Transfer Output Terminal:"
        '
        'channelParametersGroupBox
        '
        Me.channelParametersGroupBox.Controls.Add(Me.samplesPerBufferNumericUpDown)
        Me.channelParametersGroupBox.Controls.Add(Me.samplesPerBufferLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.physicalChannelComboBox)
        Me.channelParametersGroupBox.Controls.Add(Me.physicalChannelLabel)
        Me.channelParametersGroupBox.Location = New System.Drawing.Point(8, 15)
        Me.channelParametersGroupBox.Name = "channelParametersGroupBox"
        Me.channelParametersGroupBox.Size = New System.Drawing.Size(368, 96)
        Me.channelParametersGroupBox.TabIndex = 4
        Me.channelParametersGroupBox.TabStop = False
        Me.channelParametersGroupBox.Text = "Channel Parameters"
        '
        'samplesPerBufferNumericUpDown
        '
        Me.samplesPerBufferNumericUpDown.Location = New System.Drawing.Point(241, 64)
        Me.samplesPerBufferNumericUpDown.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.samplesPerBufferNumericUpDown.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.samplesPerBufferNumericUpDown.Name = "samplesPerBufferNumericUpDown"
        Me.samplesPerBufferNumericUpDown.TabIndex = 3
        Me.samplesPerBufferNumericUpDown.Value = New Decimal(New Integer() {1000, 0, 0, 0})
        '
        'samplesPerBufferLabel
        '
        Me.samplesPerBufferLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.samplesPerBufferLabel.Location = New System.Drawing.Point(8, 64)
        Me.samplesPerBufferLabel.Name = "samplesPerBufferLabel"
        Me.samplesPerBufferLabel.Size = New System.Drawing.Size(120, 23)
        Me.samplesPerBufferLabel.TabIndex = 2
        Me.samplesPerBufferLabel.Text = "Samples Per Buffer:"
        '
        'physicalChannelComboBox
        '
        Me.physicalChannelComboBox.Location = New System.Drawing.Point(240, 18)
        Me.physicalChannelComboBox.Name = "physicalChannelComboBox"
        Me.physicalChannelComboBox.Size = New System.Drawing.Size(121, 21)
        Me.physicalChannelComboBox.TabIndex = 1
        Me.physicalChannelComboBox.Text = "Dev1/port0/line0:7"
        '
        'physicalChannelLabel
        '
        Me.physicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.physicalChannelLabel.Location = New System.Drawing.Point(8, 24)
        Me.physicalChannelLabel.Name = "physicalChannelLabel"
        Me.physicalChannelLabel.TabIndex = 0
        Me.physicalChannelLabel.Text = "Physical Channel:"
        '
        'timingParametersGroupBox
        '
        Me.timingParametersGroupBox.Controls.Add(Me.samplesClockRateNumericUpDown)
        Me.timingParametersGroupBox.Controls.Add(Me.sampleClockRateLabel)
        Me.timingParametersGroupBox.Location = New System.Drawing.Point(8, 127)
        Me.timingParametersGroupBox.Name = "timingParametersGroupBox"
        Me.timingParametersGroupBox.Size = New System.Drawing.Size(368, 56)
        Me.timingParametersGroupBox.TabIndex = 5
        Me.timingParametersGroupBox.TabStop = False
        Me.timingParametersGroupBox.Text = "Timing Parameters"
        '
        'samplesClockRateNumericUpDown
        '
        Me.samplesClockRateNumericUpDown.Location = New System.Drawing.Point(241, 21)
        Me.samplesClockRateNumericUpDown.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.samplesClockRateNumericUpDown.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.samplesClockRateNumericUpDown.Name = "samplesClockRateNumericUpDown"
        Me.samplesClockRateNumericUpDown.TabIndex = 1
        Me.samplesClockRateNumericUpDown.Value = New Decimal(New Integer() {100000, 0, 0, 0})
        '
        'sampleClockRateLabel
        '
        Me.sampleClockRateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.sampleClockRateLabel.Location = New System.Drawing.Point(8, 24)
        Me.sampleClockRateLabel.Name = "sampleClockRateLabel"
        Me.sampleClockRateLabel.Size = New System.Drawing.Size(120, 23)
        Me.sampleClockRateLabel.TabIndex = 0
        Me.sampleClockRateLabel.Text = "Sample Clock Rate (Hz):"
        '
        'stopButton
        '
        Me.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.stopButton.Location = New System.Drawing.Point(214, 431)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.TabIndex = 7
        Me.stopButton.Text = "Stop"
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(728, 470)
        Me.Controls.Add(Me.resultsGroupBox)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.handshakingParametersGroupBox)
        Me.Controls.Add(Me.channelParametersGroupBox)
        Me.Controls.Add(Me.timingParametersGroupBox)
        Me.Controls.Add(Me.stopButton)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "MainForm"
        Me.Text = "Continous Read Digital Channel - Pipeline Samp Clk with Handshake"
        Me.resultsGroupBox.ResumeLayout(False)
        CType(Me.resultsDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.handshakingParametersGroupBox.ResumeLayout(False)
        CType(Me.transferDeassertConditionThresholdNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        Me.channelParametersGroupBox.ResumeLayout(False)
        CType(Me.samplesPerBufferNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        Me.timingParametersGroupBox.ResumeLayout(False)
        CType(Me.samplesClockRateNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub startButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles startButton.Click
        System.Windows.Forms.Cursor.Current = Cursors.WaitCursor
        startButton.Enabled = False
        stopButton.Enabled = True

        Try
            Dim pauseTriggerCondition As DigitalLevelPauseTriggerCondition
            Dim transferLevel As ReadyForTransferEventLevelActiveLevel

            If (pauseTriggerPolarityComboBox.SelectedIndex = 0) Then
                pauseTriggerCondition = DigitalLevelPauseTriggerCondition.High
            Else
                pauseTriggerCondition = DigitalLevelPauseTriggerCondition.Low
            End If
            If (readyForTransferLevelComboxBox.SelectedIndex = 0) Then
                transferLevel = ReadyForTransferEventLevelActiveLevel.ActiveHigh
            Else
                transferLevel = ReadyForTransferEventLevelActiveLevel.ActiveLow
            End If

            ' Create and configure DI channel
            myTask = New Task
            myTask.DIChannels.CreateChannel(physicalChannelComboBox.Text, "", ChannelLineGrouping.OneChannelForEachLine)
            myTask.Timing.ConfigurePipelinedSampleClock("", CType(samplesClockRateNumericUpDown.Value, Double), SampleClockActiveEdge.Rising, _
                                     SampleQuantityMode.ContinuousSamples, CType(samplesPerBufferNumericUpDown.Value, Integer))

            ' Configure pause trigger
            myTask.Triggers.PauseTrigger.ConfigureDigitalLevelTrigger(pauseTriggerSourceTerminalTextBox.Text, pauseTriggerCondition)

            myTask.Control(TaskAction.Verify)

            ' Set ExportSignals properties
            myTask.ExportSignals.ReadyForTransferEventOutputTerminal = readyForTransferOutputTerminalTextBox.Text
            myTask.ExportSignals.ReadyForTransferEventLevelActiveLevel = transferLevel
            myTask.ExportSignals.ReadyForTransferEventDeassertCondition = ReadyForTransferEventDeassertCondition.OnboardMemoryCustomThreshold
            myTask.ExportSignals.ReadyForTransferEventDeassertConditionCustomThreshold = CType(transferDeassertConditionThresholdNumericUpDown.Value, Long)
            myTask.Stream.ReadOverwriteMode = ReadOverwriteMode.DoNotOverwriteUnreadSamples


            runningTask = myTask

            Reader = New DigitalMultiChannelReader(myTask.Stream)

            ' Use SynchronizeCallbacks to specify that the object 
            ' marshals callbacks across threads appropriately.
            myTask.SynchronizeCallbacks = True

            asyncDigitalCallback = New AsyncCallback(AddressOf DigitalCallback)

            ' Set up callback
            Reader.BeginReadWaveform(CType(samplesPerBufferNumericUpDown.Value, Integer), asyncDigitalCallback, myTask)

        Catch exception As exception
            MessageBox.Show(exception.Message)

            runningTask = Nothing
            myTask.Dispose()
            startButton.Enabled = True
            stopButton.Enabled = False
        End Try
        System.Windows.Forms.Cursor.Current = Cursors.Default
    End Sub

    Private Sub DigitalCallback(ByVal ar As IAsyncResult)
        Try
            If (Not (runningTask Is Nothing)) AndAlso runningTask Is ar.AsyncState Then
                ' Read the available data from the channels
                waveform = Reader.EndReadWaveform(ar)

                ' Populate data table
                DataToDataTable(waveform, dataTable)

                ' Set up a new callback
                Reader.BeginReadWaveform(CType(samplesPerBufferNumericUpDown.Value, Integer), asyncDigitalCallback, myTask)
            End If
        Catch exception As DaqException
            MessageBox.Show(exception.Message)

            runningTask = Nothing
            myTask.Dispose()
            startButton.Enabled = True
            stopButton.Enabled = False
        End Try

    End Sub 'DigitalCallback

    Private Sub DataToDataTable(ByVal waveform As DigitalWaveform(), ByRef dataTable As dataTable)
        'Iterate over channels
        Dim currentLineIndex As Integer = 0
        Dim signal As DigitalWaveform

        For Each signal In waveform
            Dim sample As Integer
            For sample = 0 To (signal.Signals(0).States.Count - 1)
                If (sample = 10) Then
                    Exit For
                End If
                If (signal.Signals(0).States(sample) = DigitalState.ForceUp) Then
                    dataTable.Rows(sample)(currentLineIndex) = 1
                Else
                    dataTable.Rows(sample)(currentLineIndex) = 0
                End If
            Next
            currentLineIndex += 1
        Next
    End Sub

    Private Sub InitializeDataTable(ByRef data As dataTable)
        Dim numOfLines As Integer = Convert.ToInt32(myTask.DIChannels.Count)
        data.Rows.Clear()
        data.Columns.Clear()
        dataColumn = New DataColumn(numOfLines) {}
        Dim numOfRows As Integer = 10
        Dim currentLineIndex As Integer = 0
        Dim currentDataIndex As Integer = 0

        For currentLineIndex = 0 To (numOfLines - 1)
            dataColumn(currentLineIndex) = New DataColumn
            dataColumn(currentLineIndex).DataType = System.Type.GetType("System.Int32")
            dataColumn(currentLineIndex).ColumnName = myTask.DIChannels(currentLineIndex).PhysicalName
        Next

        data.Columns.AddRange(dataColumn)

        For currentDataIndex = 0 To (numOfRows - 1)
            Dim rowArr As Object() = New Object(numOfLines - 1) {}
            data.Rows.Add(rowArr)
        Next

    End Sub 'InitializeDataTable

    Private Sub stopButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles stopButton.Click
        runningTask = Nothing
        myTask.Dispose()

        stopButton.Enabled = False
        startButton.Enabled = True
    End Sub
End Class
