'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   ContWriteDigChan_Burst
'
' Category:
'   DO
'
' Description:
'   This example demonstrates how to output a continuous digital waveform using
'   burst handshaking mode.Note: This example program exports the sample clock
'   from the device. To import the sample clock, call
'   theConfigureHandshakingBurstExportClock method instead.
'
' Instructions for running:
'   1.  Select the Physical Channels that correspond to where yoursignal is
'       output on the device.
'   2.  Enter the number of Samples per Buffer. This is the number ofsamples
'       that will be downloaded to the device every time theDAQmx Write function
'       is called.
'   3.  Specify the Sample Clock Rate of the output Waveform.
'   4.  Specify the Output Terminal for the Exported Sample Clock.
'   5.  Specify the Sample Clock Pulse Polarity. When set to ActiveHigh, the
'       data lines will toggle on the rising edge of thesample clock.
'   6.  Specify the handshaking parameters. The Ready for TransferEvent will be
'       asserted any time this device is ready totransfer data. The Pause
'       Trigger Polarity tells this devicewhen to pause. If the polarity is set
'       to High, then thedevice will pause when the corresponding PFI line is
'       high.
'
' Steps:
'   1.  Create a task.
'   2.  Create one Digital Output channel for each Digital Line in the Task.
'   3.  Configure the Task to use a burst export clock, which configures the
'       device for Burst Mode Handshaking, setsthe sample clock rate, exports
'       the clock on the specified PFILine, and sets the sample mode to
'       Continuous.
'   4.  Generate a random DigitalWaveform for every digital channel in the Task.
'   5.  Create a DigitalMultiChannelWriter and associate it with the task
'       byusing the task's stream.
'   6.  Call DigitalMultiChannelWriter.BeginWriteWaveform to install a callback
'       and begin the asynchronous write operation.
'   7.  Inside the callback, call DigitalMultiChannelWriter.EndWrite to handle
'       the end of the asynchronous write operation.  
'   8.  Call DigitalMultiChannelWriter.BeginWriteWaveform again inside the
'       callback to perform another write operation.
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
'   Connect the Pause Trigger and Ready For Transfer event to thedefault PFI
'   terminals for the device. The sample clock will beexported to the specified
'   PFI terminal. Make sure your waveformoutput terminals match the Physical
'   Channel I/O Control.
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
    Private waveform() As DigitalWaveform
    Private asyncDigitalCallback As AsyncCallback
    Private writer As DigitalMultiChannelWriter

    Public Sub New()
        MyBase.New()

        Application.EnableVisualStyles()
        Application.DoEvents() '

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        startButton.Enabled = False

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
    Friend WithEvents startButton As System.Windows.Forms.Button
    Friend WithEvents pauseTriggerPolarityComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents pauseTriggerPolarityLabel As System.Windows.Forms.Label
    Friend WithEvents transferEventActiveLevelComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents label2 As System.Windows.Forms.Label
    Friend WithEvents channelParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents samplesPerBufferNumericUpDown As System.Windows.Forms.NumericUpDown
    Friend WithEvents samplesPerBufferLabel As System.Windows.Forms.Label
    Friend WithEvents physicalChannelComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents physicalChannelLabel As System.Windows.Forms.Label
    Friend WithEvents timingParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents sampleClockPulsePolarityComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents sampleCloclPulsePolarityLabel As System.Windows.Forms.Label
    Friend WithEvents clockSourceTextBox As System.Windows.Forms.TextBox
    Friend WithEvents samplesClockRateNumericUpDown As System.Windows.Forms.NumericUpDown
    Friend WithEvents sampleClockOutputTerminalLabel As System.Windows.Forms.Label
    Friend WithEvents sampleClockRateLabel As System.Windows.Forms.Label
    Friend WithEvents stopButton As System.Windows.Forms.Button
    Friend WithEvents handshakingParametersGroupBox As System.Windows.Forms.GroupBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MainForm))
        Me.startButton = New System.Windows.Forms.Button
        Me.handshakingParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.pauseTriggerPolarityComboBox = New System.Windows.Forms.ComboBox
        Me.pauseTriggerPolarityLabel = New System.Windows.Forms.Label
        Me.transferEventActiveLevelComboBox = New System.Windows.Forms.ComboBox
        Me.label2 = New System.Windows.Forms.Label
        Me.channelParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.samplesPerBufferNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.samplesPerBufferLabel = New System.Windows.Forms.Label
        Me.physicalChannelComboBox = New System.Windows.Forms.ComboBox
        Me.physicalChannelLabel = New System.Windows.Forms.Label
        Me.timingParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.sampleClockPulsePolarityComboBox = New System.Windows.Forms.ComboBox
        Me.sampleCloclPulsePolarityLabel = New System.Windows.Forms.Label
        Me.clockSourceTextBox = New System.Windows.Forms.TextBox
        Me.samplesClockRateNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.sampleClockOutputTerminalLabel = New System.Windows.Forms.Label
        Me.sampleClockRateLabel = New System.Windows.Forms.Label
        Me.stopButton = New System.Windows.Forms.Button
        Me.handshakingParametersGroupBox.SuspendLayout()
        Me.channelParametersGroupBox.SuspendLayout()
        CType(Me.samplesPerBufferNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.timingParametersGroupBox.SuspendLayout()
        CType(Me.samplesClockRateNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'startButton
        '
        Me.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.startButton.Location = New System.Drawing.Point(80, 392)
        Me.startButton.Name = "startButton"
        Me.startButton.TabIndex = 7
        Me.startButton.Text = "Start"
        '
        'handshakingParametersGroupBox
        '
        Me.handshakingParametersGroupBox.Controls.Add(Me.pauseTriggerPolarityComboBox)
        Me.handshakingParametersGroupBox.Controls.Add(Me.pauseTriggerPolarityLabel)
        Me.handshakingParametersGroupBox.Controls.Add(Me.transferEventActiveLevelComboBox)
        Me.handshakingParametersGroupBox.Controls.Add(Me.label2)
        Me.handshakingParametersGroupBox.Location = New System.Drawing.Point(8, 280)
        Me.handshakingParametersGroupBox.Name = "handshakingParametersGroupBox"
        Me.handshakingParametersGroupBox.Size = New System.Drawing.Size(368, 96)
        Me.handshakingParametersGroupBox.TabIndex = 5
        Me.handshakingParametersGroupBox.TabStop = False
        Me.handshakingParametersGroupBox.Text = "Handshaking Parameters"
        '
        'pauseTriggerPolarityComboBox
        '
        Me.pauseTriggerPolarityComboBox.Items.AddRange(New Object() {"High", "Low"})
        Me.pauseTriggerPolarityComboBox.Location = New System.Drawing.Point(240, 61)
        Me.pauseTriggerPolarityComboBox.Name = "pauseTriggerPolarityComboBox"
        Me.pauseTriggerPolarityComboBox.Size = New System.Drawing.Size(121, 21)
        Me.pauseTriggerPolarityComboBox.TabIndex = 3
        Me.pauseTriggerPolarityComboBox.Text = "Low"
        '
        'pauseTriggerPolarityLabel
        '
        Me.pauseTriggerPolarityLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.pauseTriggerPolarityLabel.Location = New System.Drawing.Point(8, 64)
        Me.pauseTriggerPolarityLabel.Name = "pauseTriggerPolarityLabel"
        Me.pauseTriggerPolarityLabel.Size = New System.Drawing.Size(200, 23)
        Me.pauseTriggerPolarityLabel.TabIndex = 2
        Me.pauseTriggerPolarityLabel.Text = "Pause Trigger Polarity (Pause When):"
        '
        'transferEventActiveLevelComboBox
        '
        Me.transferEventActiveLevelComboBox.Items.AddRange(New Object() {"Active High", "Active Low"})
        Me.transferEventActiveLevelComboBox.Location = New System.Drawing.Point(240, 19)
        Me.transferEventActiveLevelComboBox.Name = "transferEventActiveLevelComboBox"
        Me.transferEventActiveLevelComboBox.Size = New System.Drawing.Size(121, 21)
        Me.transferEventActiveLevelComboBox.TabIndex = 1
        Me.transferEventActiveLevelComboBox.Text = "Active High"
        '
        'label2
        '
        Me.label2.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.label2.Location = New System.Drawing.Point(8, 24)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(200, 23)
        Me.label2.TabIndex = 0
        Me.label2.Text = "Ready For Transfer Event Active Level:"
        '
        'channelParametersGroupBox
        '
        Me.channelParametersGroupBox.Controls.Add(Me.samplesPerBufferNumericUpDown)
        Me.channelParametersGroupBox.Controls.Add(Me.samplesPerBufferLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.physicalChannelComboBox)
        Me.channelParametersGroupBox.Controls.Add(Me.physicalChannelLabel)
        Me.channelParametersGroupBox.Location = New System.Drawing.Point(8, 16)
        Me.channelParametersGroupBox.Name = "channelParametersGroupBox"
        Me.channelParametersGroupBox.Size = New System.Drawing.Size(368, 96)
        Me.channelParametersGroupBox.TabIndex = 3
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
        Me.timingParametersGroupBox.Controls.Add(Me.sampleClockPulsePolarityComboBox)
        Me.timingParametersGroupBox.Controls.Add(Me.sampleCloclPulsePolarityLabel)
        Me.timingParametersGroupBox.Controls.Add(Me.clockSourceTextBox)
        Me.timingParametersGroupBox.Controls.Add(Me.samplesClockRateNumericUpDown)
        Me.timingParametersGroupBox.Controls.Add(Me.sampleClockOutputTerminalLabel)
        Me.timingParametersGroupBox.Controls.Add(Me.sampleClockRateLabel)
        Me.timingParametersGroupBox.Location = New System.Drawing.Point(8, 128)
        Me.timingParametersGroupBox.Name = "timingParametersGroupBox"
        Me.timingParametersGroupBox.Size = New System.Drawing.Size(368, 136)
        Me.timingParametersGroupBox.TabIndex = 4
        Me.timingParametersGroupBox.TabStop = False
        Me.timingParametersGroupBox.Text = "Timing Parameters"
        '
        'sampleClockPulsePolarityComboBox
        '
        Me.sampleClockPulsePolarityComboBox.Items.AddRange(New Object() {"Active High", "Active Low"})
        Me.sampleClockPulsePolarityComboBox.Location = New System.Drawing.Point(240, 96)
        Me.sampleClockPulsePolarityComboBox.Name = "sampleClockPulsePolarityComboBox"
        Me.sampleClockPulsePolarityComboBox.Size = New System.Drawing.Size(121, 21)
        Me.sampleClockPulsePolarityComboBox.TabIndex = 7
        Me.sampleClockPulsePolarityComboBox.Text = "Active High"
        '
        'sampleCloclPulsePolarityLabel
        '
        Me.sampleCloclPulsePolarityLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.sampleCloclPulsePolarityLabel.Location = New System.Drawing.Point(8, 104)
        Me.sampleCloclPulsePolarityLabel.Name = "sampleCloclPulsePolarityLabel"
        Me.sampleCloclPulsePolarityLabel.Size = New System.Drawing.Size(148, 23)
        Me.sampleCloclPulsePolarityLabel.TabIndex = 6
        Me.sampleCloclPulsePolarityLabel.Text = "Sample Clock Pulse Polarity:"
        '
        'clockSourceTextBox
        '
        Me.clockSourceTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.clockSourceTextBox.Location = New System.Drawing.Point(241, 59)
        Me.clockSourceTextBox.Name = "clockSourceTextBox"
        Me.clockSourceTextBox.Size = New System.Drawing.Size(120, 20)
        Me.clockSourceTextBox.TabIndex = 5
        Me.clockSourceTextBox.Text = "/Dev1/PFI4"
        '
        'samplesClockRateNumericUpDown
        '
        Me.samplesClockRateNumericUpDown.Location = New System.Drawing.Point(241, 21)
        Me.samplesClockRateNumericUpDown.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.samplesClockRateNumericUpDown.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.samplesClockRateNumericUpDown.Name = "samplesClockRateNumericUpDown"
        Me.samplesClockRateNumericUpDown.TabIndex = 4
        Me.samplesClockRateNumericUpDown.Value = New Decimal(New Integer() {1000, 0, 0, 0})
        '
        'sampleClockOutputTerminalLabel
        '
        Me.sampleClockOutputTerminalLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.sampleClockOutputTerminalLabel.Location = New System.Drawing.Point(8, 64)
        Me.sampleClockOutputTerminalLabel.Name = "sampleClockOutputTerminalLabel"
        Me.sampleClockOutputTerminalLabel.Size = New System.Drawing.Size(160, 23)
        Me.sampleClockOutputTerminalLabel.TabIndex = 2
        Me.sampleClockOutputTerminalLabel.Text = "Sample Clock Output Terminal"
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
        Me.stopButton.Location = New System.Drawing.Point(208, 392)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.TabIndex = 6
        Me.stopButton.Text = "Stop"
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(384, 422)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.handshakingParametersGroupBox)
        Me.Controls.Add(Me.channelParametersGroupBox)
        Me.Controls.Add(Me.timingParametersGroupBox)
        Me.Controls.Add(Me.stopButton)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "MainForm"
        Me.Text = "Continuous Write Digital Channel - Burst"
        Me.handshakingParametersGroupBox.ResumeLayout(False)
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
            Dim activeLevel As ReadyForTransferEventLevelActiveLevel
            Dim pulsePolarity As SampleClockPulsePolarity


            If (pauseTriggerPolarityComboBox.SelectedIndex = 0) Then
                pauseTriggerCondition = DigitalLevelPauseTriggerCondition.High
            Else
                pauseTriggerCondition = DigitalLevelPauseTriggerCondition.Low
            End If
            If (transferEventActiveLevelComboBox.SelectedIndex = 0) Then
                activeLevel = ReadyForTransferEventLevelActiveLevel.ActiveHigh
            Else
                activeLevel = ReadyForTransferEventLevelActiveLevel.ActiveLow
            End If
            If (sampleClockPulsePolarityComboBox.SelectedIndex = 0) Then
                pulsePolarity = SampleClockPulsePolarity.ActiveHigh
            Else
                pulsePolarity = SampleClockPulsePolarity.ActiveLow
            End If

            ' Create and configure DO channel
            myTask = New Task
            myTask.DOChannels.CreateChannel(physicalChannelComboBox.Text, "", ChannelLineGrouping.OneChannelForEachLine)
            myTask.Timing.ConfigureHandshakingBurstExportClock(clockSourceTextBox.Text, CType(samplesClockRateNumericUpDown.Value, Double), _
                                pauseTriggerCondition, activeLevel, pulsePolarity, _
                                SampleQuantityMode.ContinuousSamples, CType(samplesPerBufferNumericUpDown.Value, Integer))

            myTask.Control(TaskAction.Verify)

            Dim states As Integer = CType(samplesPerBufferNumericUpDown.Value, Integer)
            Dim signals As Integer = myTask.DOChannels.Count

            ' Loop through every sample
            waveform = New DigitalWaveform(signals - 1) {}

            Dim r As Random = New Random
            Dim i As Integer
            For i = 0 To signals - 1
                waveform(i) = New DigitalWaveform(Convert.ToInt32(samplesPerBufferNumericUpDown.Value), 1)
                ' Generate a random set of boolean values
                Dim j As Integer
                For j = 0 To states - 1
                    If r.Next() Mod 2 = 0 Then
                        waveform(i).Signals(0).States(j) = DigitalState.ForceUp
                    Else
                        waveform(i).Signals(0).States(j) = DigitalState.ForceDown
                    End If
                Next j
            Next i

            runningTask = myTask

            writer = New DigitalMultiChannelWriter(myTask.Stream)

            ' Use SynchronizeCallbacks to specify that the object 
            ' marshals callbacks across threads appropriately.
            myTask.SynchronizeCallbacks = True

            asyncDigitalCallback = New AsyncCallback(AddressOf DigitalCallback)

            ' Set up callback
            writer.BeginWriteWaveform(True, waveform, asyncDigitalCallback, myTask)

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
                ' End async write operation
                writer.EndWrite(ar)

                ' Set up new callback
                writer.BeginWriteWaveform(False, waveform, asyncDigitalCallback, myTask)
            End If
        Catch exception As DaqException
            MessageBox.Show(exception.Message)

            runningTask = Nothing
            myTask.Dispose()
            startButton.Enabled = True
            stopButton.Enabled = False
        End Try

    End Sub 'DigitalCallback

    Private Sub stopButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles stopButton.Click
        runningTask = Nothing
        myTask.Dispose()

        stopButton.Enabled = False
        startButton.Enabled = True
    End Sub
End Class
