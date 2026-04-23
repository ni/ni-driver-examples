'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   ContGenVoltageWfm_IntClk
'
' Category:
'   AO
'
' Description:
'   This example demonstrates how to continuously output a periodic waveform
'   using an internal sample clock.
'
' Instructions for running:
'   1.  Select the physical channel corresponding to where your signal is output
'       on the DAQ device.
'   2.  Enter the minimum and maximum voltage values.
'   3.  Enter the desired rate for the generation. The onboard sample clock will
'       operate at this rate.
'   4.  Select the desired waveform type.
'   5.  The rest of the parameters in the Function Generator Parameters section
'       will affect the way the waveform is created, before it's sent to the
'       analog output of the board. Select the amplitude, number of samples per
'       buffer, and the number of cycles per buffer to be used as waveform data.
'
' Steps:
'   1.  Create a new task and an analog output voltage channel.
'   2.  Call the ConfigureSampleClock method to define the sample rate and a
'       continuous sample mode.
'   3.  Create a AnalogSingleChannelWriter and call the WriteMultiSample method
'       to write the waveform data to a buffer.
'   4.  Call Task.Start().
'   5.  When the user presses the stop button, stop the task.
'   6.  Dispose the Task object to clean-up any resources associated with the
'       task.
'   7.  Handle any DaqExceptions, if they occur.
'
' I/O Connections Overview:
'   Make sure your signal output terminal matches the physical channel control.
'   In this example, the signal will output to the ao0 pin on your DAQ device.
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

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()
        Application.EnableVisualStyles()

        'This call is required by the Windows Form Designer.
        InitializeComponent()
        FunctionGenerator.InitComboBox (signalTypeComboBox)
        startButton.Enabled = True
        stopButton.Enabled = False

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
                signalTypeComboBox.SelectedIndex = 0
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents functionGeneratorGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents amplitudeLabel As System.Windows.Forms.Label
    Friend WithEvents amplitudeNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents samplesPerBufferNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents cyclesPerBufferLabel As System.Windows.Forms.Label
    Friend WithEvents cyclesPerBufferNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents signalTypeLabel As System.Windows.Forms.Label
    Friend WithEvents signalTypeComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents samplesperBufferLabel As System.Windows.Forms.Label
    Friend WithEvents stopButton As System.Windows.Forms.Button
    Friend WithEvents timingParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents frequencyNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents frequencyLabel As System.Windows.Forms.Label
    Friend WithEvents startButton As System.Windows.Forms.Button
    Friend WithEvents channelParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents physicalChannelLabel As System.Windows.Forms.Label
    Friend WithEvents maximumTextBox As System.Windows.Forms.TextBox
    Friend WithEvents minimumTextBox As System.Windows.Forms.TextBox
    Friend WithEvents maximumLabel As System.Windows.Forms.Label
    Friend WithEvents minimumLabel As System.Windows.Forms.Label
    Friend WithEvents statusCheckTimer As System.Windows.Forms.Timer
    Friend WithEvents physicalChannelComboBox As System.Windows.Forms.ComboBox


    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MainForm))
        Me.functionGeneratorGroupBox = New System.Windows.Forms.GroupBox
        Me.amplitudeLabel = New System.Windows.Forms.Label
        Me.amplitudeNumeric = New System.Windows.Forms.NumericUpDown
        Me.samplesPerBufferNumeric = New System.Windows.Forms.NumericUpDown
        Me.cyclesPerBufferLabel = New System.Windows.Forms.Label
        Me.cyclesPerBufferNumeric = New System.Windows.Forms.NumericUpDown
        Me.signalTypeLabel = New System.Windows.Forms.Label
        Me.signalTypeComboBox = New System.Windows.Forms.ComboBox
        Me.samplesperBufferLabel = New System.Windows.Forms.Label
        Me.stopButton = New System.Windows.Forms.Button
        Me.timingParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.frequencyNumeric = New System.Windows.Forms.NumericUpDown
        Me.frequencyLabel = New System.Windows.Forms.Label
        Me.startButton = New System.Windows.Forms.Button
        Me.channelParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.physicalChannelComboBox = New System.Windows.Forms.ComboBox
        Me.physicalChannelLabel = New System.Windows.Forms.Label
        Me.maximumTextBox = New System.Windows.Forms.TextBox
        Me.minimumTextBox = New System.Windows.Forms.TextBox
        Me.maximumLabel = New System.Windows.Forms.Label
        Me.minimumLabel = New System.Windows.Forms.Label
        Me.statusCheckTimer = New System.Windows.Forms.Timer(Me.components)
        Me.functionGeneratorGroupBox.SuspendLayout()
        CType(Me.amplitudeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.samplesPerBufferNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cyclesPerBufferNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.timingParametersGroupBox.SuspendLayout()
        CType(Me.frequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.channelParametersGroupBox.SuspendLayout()
        Me.SuspendLayout()
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
        Me.functionGeneratorGroupBox.Controls.Add(Me.samplesperBufferLabel)
        Me.functionGeneratorGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.functionGeneratorGroupBox.Location = New System.Drawing.Point(45, 222)
        Me.functionGeneratorGroupBox.Name = "functionGeneratorGroupBox"
        Me.functionGeneratorGroupBox.Size = New System.Drawing.Size(248, 176)
        Me.functionGeneratorGroupBox.TabIndex = 4
        Me.functionGeneratorGroupBox.TabStop = False
        Me.functionGeneratorGroupBox.Text = "Function Generator Parameters"
        '
        'amplitudeLabel
        '
        Me.amplitudeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.amplitudeLabel.Location = New System.Drawing.Point(16, 138)
        Me.amplitudeLabel.Name = "amplitudeLabel"
        Me.amplitudeLabel.Size = New System.Drawing.Size(56, 16)
        Me.amplitudeLabel.TabIndex = 6
        Me.amplitudeLabel.Text = "Amplitude:"
        '
        'amplitudeNumeric
        '
        Me.amplitudeNumeric.DecimalPlaces = 1
        Me.amplitudeNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.amplitudeNumeric.Location = New System.Drawing.Point(120, 136)
        Me.amplitudeNumeric.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.amplitudeNumeric.Name = "amplitudeNumeric"
        Me.amplitudeNumeric.Size = New System.Drawing.Size(112, 20)
        Me.amplitudeNumeric.TabIndex = 7
        Me.amplitudeNumeric.Value = New Decimal(New Integer() {2, 0, 0, 0})
        '
        'samplesPerBufferNumeric
        '
        Me.samplesPerBufferNumeric.Location = New System.Drawing.Point(120, 96)
        Me.samplesPerBufferNumeric.Maximum = New Decimal(New Integer() {10000000, 0, 0, 0})
        Me.samplesPerBufferNumeric.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.samplesPerBufferNumeric.Name = "samplesPerBufferNumeric"
        Me.samplesPerBufferNumeric.Size = New System.Drawing.Size(112, 20)
        Me.samplesPerBufferNumeric.TabIndex = 5
        Me.samplesPerBufferNumeric.Value = New Decimal(New Integer() {250, 0, 0, 0})
        '
        'cyclesPerBufferLabel
        '
        Me.cyclesPerBufferLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.cyclesPerBufferLabel.Location = New System.Drawing.Point(16, 61)
        Me.cyclesPerBufferLabel.Name = "cyclesPerBufferLabel"
        Me.cyclesPerBufferLabel.Size = New System.Drawing.Size(103, 16)
        Me.cyclesPerBufferLabel.TabIndex = 2
        Me.cyclesPerBufferLabel.Text = "Cycles Per Buffer:"
        '
        'cyclesPerBufferNumeric
        '
        Me.cyclesPerBufferNumeric.Location = New System.Drawing.Point(120, 59)
        Me.cyclesPerBufferNumeric.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.cyclesPerBufferNumeric.Name = "cyclesPerBufferNumeric"
        Me.cyclesPerBufferNumeric.Size = New System.Drawing.Size(112, 20)
        Me.cyclesPerBufferNumeric.TabIndex = 3
        Me.cyclesPerBufferNumeric.Value = New Decimal(New Integer() {5, 0, 0, 0})
        '
        'signalTypeLabel
        '
        Me.signalTypeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.signalTypeLabel.Location = New System.Drawing.Point(16, 26)
        Me.signalTypeLabel.Name = "signalTypeLabel"
        Me.signalTypeLabel.Size = New System.Drawing.Size(87, 16)
        Me.signalTypeLabel.TabIndex = 0
        Me.signalTypeLabel.Text = "Waveform Type:"
        '
        'signalTypeComboBox
        '
        Me.signalTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.signalTypeComboBox.ItemHeight = 13
        Me.signalTypeComboBox.Items.AddRange(New Object() {""})
        Me.signalTypeComboBox.Location = New System.Drawing.Point(121, 24)
        Me.signalTypeComboBox.Name = "signalTypeComboBox"
        Me.signalTypeComboBox.Size = New System.Drawing.Size(112, 21)
        Me.signalTypeComboBox.TabIndex = 1
        '
        'samplesperBufferLabel
        '
        Me.samplesperBufferLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.samplesperBufferLabel.Location = New System.Drawing.Point(16, 98)
        Me.samplesperBufferLabel.Name = "samplesperBufferLabel"
        Me.samplesperBufferLabel.Size = New System.Drawing.Size(112, 16)
        Me.samplesperBufferLabel.TabIndex = 4
        Me.samplesperBufferLabel.Text = "Samples Per Buffer:"
        '
        'stopButton
        '
        Me.stopButton.Enabled = False
        Me.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.stopButton.Location = New System.Drawing.Point(184, 414)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(80, 24)
        Me.stopButton.TabIndex = 1
        Me.stopButton.Text = "Stop"
        '
        'timingParametersGroupBox
        '
        Me.timingParametersGroupBox.Controls.Add(Me.frequencyNumeric)
        Me.timingParametersGroupBox.Controls.Add(Me.frequencyLabel)
        Me.timingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.timingParametersGroupBox.Location = New System.Drawing.Point(46, 150)
        Me.timingParametersGroupBox.Name = "timingParametersGroupBox"
        Me.timingParametersGroupBox.Size = New System.Drawing.Size(247, 64)
        Me.timingParametersGroupBox.TabIndex = 3
        Me.timingParametersGroupBox.TabStop = False
        Me.timingParametersGroupBox.Text = "Timing Parameters"
        '
        'frequencyNumeric
        '
        Me.frequencyNumeric.Location = New System.Drawing.Point(120, 24)
        Me.frequencyNumeric.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.frequencyNumeric.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.frequencyNumeric.Name = "frequencyNumeric"
        Me.frequencyNumeric.Size = New System.Drawing.Size(112, 20)
        Me.frequencyNumeric.TabIndex = 1
        Me.frequencyNumeric.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        'frequencyLabel
        '
        Me.frequencyLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.frequencyLabel.Location = New System.Drawing.Point(16, 26)
        Me.frequencyLabel.Name = "frequencyLabel"
        Me.frequencyLabel.Size = New System.Drawing.Size(88, 16)
        Me.frequencyLabel.TabIndex = 0
        Me.frequencyLabel.Text = "Frequency (Hz):"
        '
        'startButton
        '
        Me.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.startButton.Location = New System.Drawing.Point(70, 414)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(80, 24)
        Me.startButton.TabIndex = 0
        Me.startButton.Text = "Start"
        '
        'channelParametersGroupBox
        '
        Me.channelParametersGroupBox.Controls.Add(Me.physicalChannelComboBox)
        Me.channelParametersGroupBox.Controls.Add(Me.physicalChannelLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.maximumTextBox)
        Me.channelParametersGroupBox.Controls.Add(Me.minimumTextBox)
        Me.channelParametersGroupBox.Controls.Add(Me.maximumLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.minimumLabel)
        Me.channelParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.channelParametersGroupBox.Location = New System.Drawing.Point(46, 14)
        Me.channelParametersGroupBox.Name = "channelParametersGroupBox"
        Me.channelParametersGroupBox.Size = New System.Drawing.Size(247, 128)
        Me.channelParametersGroupBox.TabIndex = 2
        Me.channelParametersGroupBox.TabStop = False
        Me.channelParametersGroupBox.Text = "Channel Parameters"
        '
        'physicalChannelComboBox
        '
        Me.physicalChannelComboBox.Location = New System.Drawing.Point(120, 24)
        Me.physicalChannelComboBox.Name = "physicalChannelComboBox"
        Me.physicalChannelComboBox.Size = New System.Drawing.Size(112, 21)
        Me.physicalChannelComboBox.TabIndex = 1
        Me.physicalChannelComboBox.Text = "Dev1/ao0"
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
        'maximumTextBox
        '
        Me.maximumTextBox.Location = New System.Drawing.Point(120, 96)
        Me.maximumTextBox.Name = "maximumTextBox"
        Me.maximumTextBox.Size = New System.Drawing.Size(112, 20)
        Me.maximumTextBox.TabIndex = 5
        Me.maximumTextBox.Text = "10"
        '
        'minimumTextBox
        '
        Me.minimumTextBox.Location = New System.Drawing.Point(120, 60)
        Me.minimumTextBox.Name = "minimumTextBox"
        Me.minimumTextBox.Size = New System.Drawing.Size(112, 20)
        Me.minimumTextBox.TabIndex = 3
        Me.minimumTextBox.Text = "-10"
        '
        'maximumLabel
        '
        Me.maximumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.maximumLabel.Location = New System.Drawing.Point(16, 98)
        Me.maximumLabel.Name = "maximumLabel"
        Me.maximumLabel.Size = New System.Drawing.Size(112, 16)
        Me.maximumLabel.TabIndex = 4
        Me.maximumLabel.Text = "Maximum Value (V):"
        '
        'minimumLabel
        '
        Me.minimumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.minimumLabel.Location = New System.Drawing.Point(16, 62)
        Me.minimumLabel.Name = "minimumLabel"
        Me.minimumLabel.Size = New System.Drawing.Size(104, 16)
        Me.minimumLabel.TabIndex = 2
        Me.minimumLabel.Text = "Minimum Value (V):"
        '
        'statusCheckTimer
        '
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(338, 453)
        Me.Controls.Add(Me.functionGeneratorGroupBox)
        Me.Controls.Add(Me.stopButton)
        Me.Controls.Add(Me.timingParametersGroupBox)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.channelParametersGroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Continuous Voltage Generation - Int Clk"
        Me.functionGeneratorGroupBox.ResumeLayout(False)
        CType(Me.amplitudeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.samplesPerBufferNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cyclesPerBufferNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.timingParametersGroupBox.ResumeLayout(False)
        CType(Me.frequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.channelParametersGroupBox.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region
    'global variables
    Private myTask As Task


    Private Sub startButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) handles startButton.Click
        System.Windows.Forms.Cursor.Current = Cursors.WaitCursor
        Try
            ' Create the task and channel
            myTask = New Task()
            myTask.AOChannels.CreateVoltageChannel(physicalChannelComboBox.Text, _
                "", _
                Convert.ToDouble(minimumTextBox.Text), _
                Convert.ToDouble(maximumTextBox.Text), _
                AOVoltageUnits.Volts)

            ' Verify the task before doing the waveform calculations
            myTask.Control(TaskAction.Verify)

            ' Calculate some waveform parameters and generate data
            Dim fGen as New FunctionGenerator( _
                myTask.Timing, _
                frequencyNumeric.Value.ToString(), _
                samplesPerBufferNumeric.Value.ToString(), _
                cyclesPerBufferNumeric.Value.ToString(), _
                signalTypeComboBox.Text, _
                amplitudeNumeric.Value.ToString())

            ' Configure the sample clock with the calculated rate
            myTask.Timing.ConfigureSampleClock( _
                "", _
                fGen.ResultingSampleClockRate, _
                SampleClockActiveEdge.Rising, _
                SampleQuantityMode.ContinuousSamples, 1000)

            ' Write the data to the buffer
            Dim writer As New AnalogSingleChannelWriter(myTask.Stream)
            writer.WriteMultiSample(False, fGen.Data)

            'Start writing out data
            myTask.Start()
            startButton.Enabled = False
            stopButton.Enabled = True

            statusCheckTimer.Enabled = True
        Catch err As DaqException
            statusCheckTimer.Enabled = False
            MessageBox.Show(err.Message)
            myTask.Dispose()
        End Try
        System.Windows.Forms.Cursor.Current = Cursors.Default
    End Sub
    Private Sub stopButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) handles stopButton.Click
        statusCheckTimer.Enabled = False
        If Not myTask Is Nothing Then
            Try
                myTask.Stop()
            Catch x As System.Exception
                MessageBox.Show(x.Message)
            End Try
            myTask.Dispose()
            myTask = Nothing
            startButton.Enabled = True
            stopButton.Enabled = False
        End If
    End Sub

    Private Sub statusCheckTimer_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles statusCheckTimer.Tick
        Try
            'Getting myTask.IsDone also checks for errors that would prematurely
            'halt the continuous generation.
            If (myTask.IsDone) Then
                statusCheckTimer.Enabled = False
                myTask.Stop()
                myTask.Dispose()
                startButton.Enabled = True
                stopButton.Enabled = False
            End If
        Catch ex As DaqException
            statusCheckTimer.Enabled = False
            System.Windows.Forms.MessageBox.Show(ex.Message)
            myTask.Dispose()
            startButton.Enabled = True
            stopButton.Enabled = False
        End Try
    End Sub
End Class
