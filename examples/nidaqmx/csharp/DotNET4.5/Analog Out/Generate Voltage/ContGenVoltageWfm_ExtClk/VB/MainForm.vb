'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   ContGenVoltageWfm_ExtClk
'
' Category:
'   AO
'
' Description:
'   This example demonstrates how to continuously output a periodic waveform
'   using an external clock.
'
' Instructions for running:
'   1.  Select the physical channel corresponding to where your signal is output
'       on the DAQ device.
'   2.  Enter the minimum and maximum voltage values.
'   3.  Specify the external sample clock source.
'   4.  Select the desired waveform type.
'   5.  The rest of the parameters in the Function Generator Parameters section
'       will affect the way the waveform is created, before it's sent to the
'       analog output of the board. Select the amplitude, number of samples per
'       buffer, and the number of cycles per buffer to be used as waveform data.
'
' Steps:
'   1.  Create a new task and an analog output voltage channel.
'   2.  Specify the external clock source, and define the sample mode for
'       continuous samples.
'   3.  Create a AnalogSingleChannelWriter and call the WriteMultiSample method
'       to write the waveform data to a buffer.
'   4.  Call Task.Start().
'   5.  When the user presses the stop button, stop the task.
'   6.  Dispose the Task object to clean-up any resources associated with the
'       task.
'   7.  Handle any DaqExceptions, if they occur.
'
' I/O Connections Overview:
'   Make sure your signal output terminal matches the text in physical channel
'   textbox. In this case the signal will output to the ao0 pin on your DAQ
'   Device. Wire the external sample clock to a PFI or RTSI pin of your choice
'   on the board. Specify the same terminal name as the argument to the
'   ConfigureSampleClock method. (PFI0 is used in this example).  For more
'   information on the input and output terminals for your device, open the
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

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()
        Application.EnableVisualStyles()

        'This call is required by the Windows Form Designer.
        InitializeComponent()
        FunctionGenerator.InitComboBox(signalTypeComboBox)

        physicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AO, PhysicalChannelAccess.External))
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
            If Not (myTask Is Nothing) Then
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
    Friend WithEvents channelParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents minimumNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents maximumValueLabel As System.Windows.Forms.Label
    Friend WithEvents minimumValueLabel As System.Windows.Forms.Label
    Friend WithEvents physicalChannelLabel As System.Windows.Forms.Label
    Friend WithEvents maximumNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents functionGeneratorGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents amplitudeLabel As System.Windows.Forms.Label
    Friend WithEvents amplitudeNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents samplesPerBufferNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents cyclesPerBufferLabel As System.Windows.Forms.Label
    Friend WithEvents cyclesPerBufferNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents signalTypeLabel As System.Windows.Forms.Label
    Friend WithEvents signalTypeComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents startButton As System.Windows.Forms.Button
    Friend WithEvents stopButton As System.Windows.Forms.Button
    Friend WithEvents timingParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents frequencyNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents frequencyLabel As System.Windows.Forms.Label
    Friend WithEvents clockSourceTextBox As System.Windows.Forms.TextBox
    Friend WithEvents clockSourceLabel As System.Windows.Forms.Label
    Friend WithEvents samplesPerBufferLabel As System.Windows.Forms.Label
    Friend WithEvents physicalChannelComboBox As System.Windows.Forms.ComboBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MainForm))
        Me.channelParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.physicalChannelComboBox = New System.Windows.Forms.ComboBox
        Me.minimumNumeric = New System.Windows.Forms.NumericUpDown
        Me.maximumValueLabel = New System.Windows.Forms.Label
        Me.minimumValueLabel = New System.Windows.Forms.Label
        Me.physicalChannelLabel = New System.Windows.Forms.Label
        Me.maximumNumeric = New System.Windows.Forms.NumericUpDown
        Me.functionGeneratorGroupBox = New System.Windows.Forms.GroupBox
        Me.amplitudeLabel = New System.Windows.Forms.Label
        Me.amplitudeNumeric = New System.Windows.Forms.NumericUpDown
        Me.samplesPerBufferNumeric = New System.Windows.Forms.NumericUpDown
        Me.cyclesPerBufferLabel = New System.Windows.Forms.Label
        Me.cyclesPerBufferNumeric = New System.Windows.Forms.NumericUpDown
        Me.signalTypeLabel = New System.Windows.Forms.Label
        Me.signalTypeComboBox = New System.Windows.Forms.ComboBox
        Me.samplesPerBufferLabel = New System.Windows.Forms.Label
        Me.startButton = New System.Windows.Forms.Button
        Me.stopButton = New System.Windows.Forms.Button
        Me.timingParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.frequencyNumeric = New System.Windows.Forms.NumericUpDown
        Me.frequencyLabel = New System.Windows.Forms.Label
        Me.clockSourceTextBox = New System.Windows.Forms.TextBox
        Me.clockSourceLabel = New System.Windows.Forms.Label
        Me.channelParametersGroupBox.SuspendLayout()
        CType(Me.minimumNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.maximumNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.functionGeneratorGroupBox.SuspendLayout()
        CType(Me.amplitudeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.samplesPerBufferNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cyclesPerBufferNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.timingParametersGroupBox.SuspendLayout()
        CType(Me.frequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'channelParametersGroupBox
        '
        Me.channelParametersGroupBox.Controls.Add(Me.physicalChannelComboBox)
        Me.channelParametersGroupBox.Controls.Add(Me.minimumNumeric)
        Me.channelParametersGroupBox.Controls.Add(Me.maximumValueLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.minimumValueLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.physicalChannelLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.maximumNumeric)
        Me.channelParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.channelParametersGroupBox.Location = New System.Drawing.Point(8, 8)
        Me.channelParametersGroupBox.Name = "channelParametersGroupBox"
        Me.channelParametersGroupBox.Size = New System.Drawing.Size(256, 136)
        Me.channelParametersGroupBox.TabIndex = 2
        Me.channelParametersGroupBox.TabStop = False
        Me.channelParametersGroupBox.Text = "Channel Parameters"
        '
        'physicalChannelComboBox
        '
        Me.physicalChannelComboBox.Location = New System.Drawing.Point(152, 24)
        Me.physicalChannelComboBox.Name = "physicalChannelComboBox"
        Me.physicalChannelComboBox.Size = New System.Drawing.Size(96, 21)
        Me.physicalChannelComboBox.TabIndex = 1
        Me.physicalChannelComboBox.Text = "Dev1/ao0"
        '
        'minimumNumeric
        '
        Me.minimumNumeric.Location = New System.Drawing.Point(152, 59)
        Me.minimumNumeric.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.minimumNumeric.Minimum = New Decimal(New Integer() {10, 0, 0, -2147483648})
        Me.minimumNumeric.Name = "minimumNumeric"
        Me.minimumNumeric.Size = New System.Drawing.Size(96, 20)
        Me.minimumNumeric.TabIndex = 3
        Me.minimumNumeric.Value = New Decimal(New Integer() {10, 0, 0, -2147483648})
        '
        'maximumValueLabel
        '
        Me.maximumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.maximumValueLabel.Location = New System.Drawing.Point(16, 99)
        Me.maximumValueLabel.Name = "maximumValueLabel"
        Me.maximumValueLabel.Size = New System.Drawing.Size(111, 14)
        Me.maximumValueLabel.TabIndex = 4
        Me.maximumValueLabel.Text = "Maximum Value (V):"
        '
        'minimumValueLabel
        '
        Me.minimumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.minimumValueLabel.Location = New System.Drawing.Point(16, 62)
        Me.minimumValueLabel.Name = "minimumValueLabel"
        Me.minimumValueLabel.Size = New System.Drawing.Size(104, 14)
        Me.minimumValueLabel.TabIndex = 2
        Me.minimumValueLabel.Text = "Minimum Value (V):"
        '
        'physicalChannelLabel
        '
        Me.physicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.physicalChannelLabel.Location = New System.Drawing.Point(16, 27)
        Me.physicalChannelLabel.Name = "physicalChannelLabel"
        Me.physicalChannelLabel.Size = New System.Drawing.Size(95, 14)
        Me.physicalChannelLabel.TabIndex = 0
        Me.physicalChannelLabel.Text = "Physical Channel:"
        '
        'maximumNumeric
        '
        Me.maximumNumeric.Location = New System.Drawing.Point(152, 96)
        Me.maximumNumeric.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.maximumNumeric.Minimum = New Decimal(New Integer() {10, 0, 0, -2147483648})
        Me.maximumNumeric.Name = "maximumNumeric"
        Me.maximumNumeric.Size = New System.Drawing.Size(96, 20)
        Me.maximumNumeric.TabIndex = 5
        Me.maximumNumeric.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'functionGeneratorGroupBox
        '
        Me.functionGeneratorGroupBox.Controls.Add(Me.amplitudeLabel)
        Me.functionGeneratorGroupBox.Controls.Add(Me.amplitudeNumeric)
        Me.functionGeneratorGroupBox.Controls.Add(Me.samplesPerBufferNumeric)
        Me.functionGeneratorGroupBox.Controls.Add(Me.cyclesPerBufferLabel)
        Me.functionGeneratorGroupBox.Controls.Add(Me.cyclesPerBufferNumeric)
        Me.functionGeneratorGroupBox.Controls.Add(Me.signalTypeLabel)
        Me.functionGeneratorGroupBox.Controls.Add(Me.signalTypeComboBox)
        Me.functionGeneratorGroupBox.Controls.Add(Me.samplesPerBufferLabel)
        Me.functionGeneratorGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.functionGeneratorGroupBox.Location = New System.Drawing.Point(272, 8)
        Me.functionGeneratorGroupBox.Name = "functionGeneratorGroupBox"
        Me.functionGeneratorGroupBox.Size = New System.Drawing.Size(224, 168)
        Me.functionGeneratorGroupBox.TabIndex = 4
        Me.functionGeneratorGroupBox.TabStop = False
        Me.functionGeneratorGroupBox.Text = "Function Generator Parameters"
        '
        'amplitudeLabel
        '
        Me.amplitudeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.amplitudeLabel.Location = New System.Drawing.Point(16, 138)
        Me.amplitudeLabel.Name = "amplitudeLabel"
        Me.amplitudeLabel.Size = New System.Drawing.Size(64, 16)
        Me.amplitudeLabel.TabIndex = 42
        Me.amplitudeLabel.Text = "Amplitude:"
        '
        'amplitudeNumeric
        '
        Me.amplitudeNumeric.DecimalPlaces = 1
        Me.amplitudeNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.amplitudeNumeric.Location = New System.Drawing.Point(120, 136)
        Me.amplitudeNumeric.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.amplitudeNumeric.Name = "amplitudeNumeric"
        Me.amplitudeNumeric.Size = New System.Drawing.Size(96, 20)
        Me.amplitudeNumeric.TabIndex = 3
        Me.amplitudeNumeric.Value = New Decimal(New Integer() {2, 0, 0, 0})
        '
        'samplesPerBufferNumeric
        '
        Me.samplesPerBufferNumeric.Location = New System.Drawing.Point(120, 96)
        Me.samplesPerBufferNumeric.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.samplesPerBufferNumeric.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.samplesPerBufferNumeric.Name = "samplesPerBufferNumeric"
        Me.samplesPerBufferNumeric.Size = New System.Drawing.Size(96, 20)
        Me.samplesPerBufferNumeric.TabIndex = 2
        Me.samplesPerBufferNumeric.Value = New Decimal(New Integer() {250, 0, 0, 0})
        '
        'cyclesPerBufferLabel
        '
        Me.cyclesPerBufferLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.cyclesPerBufferLabel.Location = New System.Drawing.Point(16, 61)
        Me.cyclesPerBufferLabel.Name = "cyclesPerBufferLabel"
        Me.cyclesPerBufferLabel.Size = New System.Drawing.Size(103, 16)
        Me.cyclesPerBufferLabel.TabIndex = 34
        Me.cyclesPerBufferLabel.Text = "Cycles Per Buffer:"
        '
        'cyclesPerBufferNumeric
        '
        Me.cyclesPerBufferNumeric.Location = New System.Drawing.Point(120, 59)
        Me.cyclesPerBufferNumeric.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.cyclesPerBufferNumeric.Name = "cyclesPerBufferNumeric"
        Me.cyclesPerBufferNumeric.Size = New System.Drawing.Size(96, 20)
        Me.cyclesPerBufferNumeric.TabIndex = 1
        Me.cyclesPerBufferNumeric.Value = New Decimal(New Integer() {5, 0, 0, 0})
        '
        'signalTypeLabel
        '
        Me.signalTypeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.signalTypeLabel.Location = New System.Drawing.Point(16, 26)
        Me.signalTypeLabel.Name = "signalTypeLabel"
        Me.signalTypeLabel.Size = New System.Drawing.Size(87, 16)
        Me.signalTypeLabel.TabIndex = 31
        Me.signalTypeLabel.Text = "Waveform Type:"
        '
        'signalTypeComboBox
        '
        Me.signalTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.signalTypeComboBox.ItemHeight = 13
        Me.signalTypeComboBox.Items.AddRange(New Object() {""})
        Me.signalTypeComboBox.Location = New System.Drawing.Point(121, 24)
        Me.signalTypeComboBox.Name = "signalTypeComboBox"
        Me.signalTypeComboBox.Size = New System.Drawing.Size(96, 21)
        Me.signalTypeComboBox.TabIndex = 0
        '
        'samplesPerBufferLabel
        '
        Me.samplesPerBufferLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.samplesPerBufferLabel.Location = New System.Drawing.Point(16, 98)
        Me.samplesPerBufferLabel.Name = "samplesPerBufferLabel"
        Me.samplesPerBufferLabel.Size = New System.Drawing.Size(112, 16)
        Me.samplesPerBufferLabel.TabIndex = 35
        Me.samplesPerBufferLabel.Text = "Samples Per Buffer:"
        '
        'startButton
        '
        Me.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.startButton.Location = New System.Drawing.Point(347, 192)
        Me.startButton.Name = "startButton"
        Me.startButton.TabIndex = 0
        Me.startButton.Text = "Start"
        '
        'stopButton
        '
        Me.stopButton.Enabled = False
        Me.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.stopButton.Location = New System.Drawing.Point(347, 225)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.TabIndex = 1
        Me.stopButton.Text = "Stop"
        '
        'timingParametersGroupBox
        '
        Me.timingParametersGroupBox.Controls.Add(Me.frequencyNumeric)
        Me.timingParametersGroupBox.Controls.Add(Me.frequencyLabel)
        Me.timingParametersGroupBox.Controls.Add(Me.clockSourceTextBox)
        Me.timingParametersGroupBox.Controls.Add(Me.clockSourceLabel)
        Me.timingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.timingParametersGroupBox.Location = New System.Drawing.Point(8, 152)
        Me.timingParametersGroupBox.Name = "timingParametersGroupBox"
        Me.timingParametersGroupBox.Size = New System.Drawing.Size(256, 96)
        Me.timingParametersGroupBox.TabIndex = 3
        Me.timingParametersGroupBox.TabStop = False
        Me.timingParametersGroupBox.Text = "Timing Parameters"
        '
        'frequencyNumeric
        '
        Me.frequencyNumeric.Location = New System.Drawing.Point(152, 56)
        Me.frequencyNumeric.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.frequencyNumeric.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.frequencyNumeric.Name = "frequencyNumeric"
        Me.frequencyNumeric.Size = New System.Drawing.Size(96, 20)
        Me.frequencyNumeric.TabIndex = 1
        Me.frequencyNumeric.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        'frequencyLabel
        '
        Me.frequencyLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.frequencyLabel.Location = New System.Drawing.Point(16, 59)
        Me.frequencyLabel.Name = "frequencyLabel"
        Me.frequencyLabel.Size = New System.Drawing.Size(128, 14)
        Me.frequencyLabel.TabIndex = 20
        Me.frequencyLabel.Text = "Desired Frequency (Hz):"
        '
        'clockSourceTextBox
        '
        Me.clockSourceTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.clockSourceTextBox.Location = New System.Drawing.Point(152, 24)
        Me.clockSourceTextBox.Name = "clockSourceTextBox"
        Me.clockSourceTextBox.Size = New System.Drawing.Size(96, 20)
        Me.clockSourceTextBox.TabIndex = 0
        Me.clockSourceTextBox.Text = "/Dev1/PFI0"
        '
        'clockSourceLabel
        '
        Me.clockSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.clockSourceLabel.Location = New System.Drawing.Point(16, 27)
        Me.clockSourceLabel.Name = "clockSourceLabel"
        Me.clockSourceLabel.Size = New System.Drawing.Size(88, 14)
        Me.clockSourceLabel.TabIndex = 16
        Me.clockSourceLabel.Text = "Clock Source:"
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(522, 257)
        Me.Controls.Add(Me.channelParametersGroupBox)
        Me.Controls.Add(Me.functionGeneratorGroupBox)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.stopButton)
        Me.Controls.Add(Me.timingParametersGroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(528, 289)
        Me.MinimumSize = New System.Drawing.Size(528, 289)
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Continuous Voltage Generation - External Clock"
        Me.channelParametersGroupBox.ResumeLayout(False)
        CType(Me.minimumNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.maximumNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.functionGeneratorGroupBox.ResumeLayout(False)
        CType(Me.amplitudeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.samplesPerBufferNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cyclesPerBufferNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.timingParametersGroupBox.ResumeLayout(False)
        CType(Me.frequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private myTask As Task

    Private Sub startButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles startButton.Click
        Try
            ' Create the task and channel
            myTask = New Task()

            myTask.AOChannels.CreateVoltageChannel(physicalChannelComboBox.Text, _
                 "", _
                 Convert.ToDouble(minimumNumeric.Value), _
                 Convert.ToDouble(maximumNumeric.Value), _
                 AOVoltageUnits.Volts)

            ' Verify the task before doing the waveform calculations
            myTask.Control(TaskAction.Verify)

            ' Calculate some waveform parameters and generate data
            Dim fGen As New FunctionGenerator(myTask.Timing, _
                frequencyNumeric.Value.ToString(), _
                samplesPerBufferNumeric.Value.ToString(), _
                cyclesPerBufferNumeric.Value.ToString(), _
                signalTypeComboBox.Text, _
                amplitudeNumeric.Value.ToString())

            ' Configure the sample clock with the calculated rate and 
            ' specified clock source
            myTask.Timing.ConfigureSampleClock(clockSourceTextBox.Text, _
                fGen.ResultingSampleClockRate, _
                SampleClockActiveEdge.Rising, _
                SampleQuantityMode.ContinuousSamples, 1000)

            ' Set up event handler
            AddHandler myTask.Done, AddressOf myTask_Done

            ' Write the data to buffer
            Dim writer As New AnalogSingleChannelWriter(myTask.Stream)
            writer.WriteMultiSample(False, fGen.Data)

            ' Start Write
            myTask.Start()

            startButton.Enabled = False
            stopButton.Enabled = True
            channelParametersGroupBox.Enabled = False
            timingParametersGroupBox.Enabled = False
            functionGeneratorGroupBox.Enabled = False
        Catch ex As DaqException
            myTask.Dispose()

            MessageBox.Show(ex.Message)

            startButton.Enabled = True
            stopButton.Enabled = False
            channelParametersGroupBox.Enabled = True
            timingParametersGroupBox.Enabled = True
            functionGeneratorGroupBox.Enabled = True
        End Try
    End Sub

    Private Sub stopButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles stopButton.Click
        If Not myTask Is Nothing Then
            myTask.Stop()
            myTask.Dispose()
        End If

        startButton.Enabled = True
        stopButton.Enabled = False
        channelParametersGroupBox.Enabled = True
        timingParametersGroupBox.Enabled = True
        functionGeneratorGroupBox.Enabled = True
    End Sub

    Private Sub myTask_Done(ByVal sender As Object, ByVal e As TaskDoneEventArgs)
        If Not e.Error Is Nothing Then
            MessageBox.Show(e.Error.Message)
        End If

        If Not myTask Is Nothing Then
            myTask.Dispose()
        End If

        startButton.Enabled = True
        stopButton.Enabled = False
        channelParametersGroupBox.Enabled = True
        timingParametersGroupBox.Enabled = True
        functionGeneratorGroupBox.Enabled = True
    End Sub
End Class
