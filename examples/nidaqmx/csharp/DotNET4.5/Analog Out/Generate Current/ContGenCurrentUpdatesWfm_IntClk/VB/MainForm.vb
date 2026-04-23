'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   ContGenCurrentUpdatesWfm_IntClk
'
' Category:
'   AO
'
' Description:
'   This example demonstrates how to output a continuous number of current
'   samples to an Analog Output Channel using an internal sample clock.
'
' Instructions for running:
'   1.  Select the Physical Channel to correspond to where your signal is
'       outputon the DAQ device.
'   2.  Enter the Minimum and Maximum Current Ranges.
'   3.  Enter the desired rate for the generation. The onboard sample clock
'       willoperate at this rate.
'   4.  Select the desired waveform type.
'   5.  The rest of the parameters in the Function Generator Parameters
'       sectionwill affect the way the waveform is created, before it's sent to
'       theanalog output of the board. Select the amplitude, number of samples
'       perbuffer, the number of cycles per buffer, and offset to be used as
'       waveformdata.
'
' Steps:
'   1.  Create a new task and an Analog Output Current Channel.
'   2.  Call the ConfigureSampleClock method to define the sample rate and
'       acontinuous sample mode.
'   3.  Create a AnalogSingleChannelWriter and call the WriteWaveform methodto
'       write the waveform data.
'   4.  Call Task.Start().
'   5.  When the user presses the stop button, stop the task.
'   6.  Dispose the Task object to clean-up any resources associated with the
'       task.
'   7.  Handle any DaqExceptions, if they occur.
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

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()
        Application.EnableVisualStyles()
        Application.DoEvents()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        startButton.Enabled = False
        stopButton.Enabled = False
        FunctionGenerator.InitComboBox(signalTypeComboBox)

        physicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AO, PhysicalChannelAccess.External))
        If (physicalChannelComboBox.Items.Count > 0) Then
            physicalChannelComboBox.SelectedItem = 0
            startButton.Enabled = True
        End If

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
    Friend WithEvents stopButton As System.Windows.Forms.Button
    Friend WithEvents TimingParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents frequencyTextBox As System.Windows.Forms.TextBox
    Friend WithEvents frequencyLabel As System.Windows.Forms.Label
    Friend WithEvents startButton As System.Windows.Forms.Button
    Friend WithEvents channelParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents physicalChannelComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents maximumValueTextBox As System.Windows.Forms.TextBox
    Friend WithEvents minimumValueTextBox As System.Windows.Forms.TextBox
    Friend WithEvents maximumValueLabel As System.Windows.Forms.Label
    Friend WithEvents minimumValueLabel As System.Windows.Forms.Label
    Friend WithEvents physicalChannelLabel As System.Windows.Forms.Label
    Friend WithEvents functionGeneratorGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents offsetLabel As System.Windows.Forms.Label
    Friend WithEvents offsetNumericUpDown As System.Windows.Forms.NumericUpDown
    Friend WithEvents amplitudeLabel As System.Windows.Forms.Label
    Friend WithEvents amplitudeNumericUpDown As System.Windows.Forms.NumericUpDown
    Friend WithEvents samplesPerBufferNumericUpDown As System.Windows.Forms.NumericUpDown
    Friend WithEvents samplesPerBufferLabel As System.Windows.Forms.Label
    Friend WithEvents cyclesPerBufferLabel As System.Windows.Forms.Label
    Friend WithEvents cyclesPerBufferNumericUpDown As System.Windows.Forms.NumericUpDown
    Friend WithEvents waveformTypeLabel As System.Windows.Forms.Label
    Friend WithEvents signalTypeComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents statusCheckTimer As System.Windows.Forms.Timer
    Private myTask As Task = Nothing

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MainForm))
        Me.stopButton = New System.Windows.Forms.Button
        Me.TimingParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.frequencyTextBox = New System.Windows.Forms.TextBox
        Me.frequencyLabel = New System.Windows.Forms.Label
        Me.startButton = New System.Windows.Forms.Button
        Me.channelParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.physicalChannelComboBox = New System.Windows.Forms.ComboBox
        Me.maximumValueTextBox = New System.Windows.Forms.TextBox
        Me.minimumValueTextBox = New System.Windows.Forms.TextBox
        Me.maximumValueLabel = New System.Windows.Forms.Label
        Me.minimumValueLabel = New System.Windows.Forms.Label
        Me.physicalChannelLabel = New System.Windows.Forms.Label
        Me.functionGeneratorGroupBox = New System.Windows.Forms.GroupBox
        Me.offsetLabel = New System.Windows.Forms.Label
        Me.offsetNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.amplitudeLabel = New System.Windows.Forms.Label
        Me.amplitudeNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.samplesPerBufferNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.samplesPerBufferLabel = New System.Windows.Forms.Label
        Me.cyclesPerBufferLabel = New System.Windows.Forms.Label
        Me.cyclesPerBufferNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.waveformTypeLabel = New System.Windows.Forms.Label
        Me.signalTypeComboBox = New System.Windows.Forms.ComboBox
        Me.statusCheckTimer = New System.Windows.Forms.Timer(Me.components)
        Me.TimingParametersGroupBox.SuspendLayout()
        Me.channelParametersGroupBox.SuspendLayout()
        Me.functionGeneratorGroupBox.SuspendLayout()
        CType(Me.offsetNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.amplitudeNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.samplesPerBufferNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cyclesPerBufferNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'stopButton
        '
        Me.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.stopButton.Location = New System.Drawing.Point(448, 224)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(80, 23)
        Me.stopButton.TabIndex = 6
        Me.stopButton.Text = "Stop"
        '
        'TimingParametersGroupBox
        '
        Me.TimingParametersGroupBox.Controls.Add(Me.frequencyTextBox)
        Me.TimingParametersGroupBox.Controls.Add(Me.frequencyLabel)
        Me.TimingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.TimingParametersGroupBox.Location = New System.Drawing.Point(8, 152)
        Me.TimingParametersGroupBox.Name = "TimingParametersGroupBox"
        Me.TimingParametersGroupBox.Size = New System.Drawing.Size(272, 64)
        Me.TimingParametersGroupBox.TabIndex = 8
        Me.TimingParametersGroupBox.TabStop = False
        Me.TimingParametersGroupBox.Text = "Timing Parameters"
        '
        'frequencyTextBox
        '
        Me.frequencyTextBox.Location = New System.Drawing.Point(136, 24)
        Me.frequencyTextBox.Name = "frequencyTextBox"
        Me.frequencyTextBox.Size = New System.Drawing.Size(120, 20)
        Me.frequencyTextBox.TabIndex = 1
        Me.frequencyTextBox.Text = "1000"
        '
        'frequencyLabel
        '
        Me.frequencyLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.frequencyLabel.Location = New System.Drawing.Point(16, 24)
        Me.frequencyLabel.Name = "frequencyLabel"
        Me.frequencyLabel.Size = New System.Drawing.Size(128, 24)
        Me.frequencyLabel.TabIndex = 0
        Me.frequencyLabel.Text = "Signal Frequency (Hz):"
        '
        'startButton
        '
        Me.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.startButton.Location = New System.Drawing.Point(352, 224)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(80, 23)
        Me.startButton.TabIndex = 5
        Me.startButton.Text = "Start"
        '
        'channelParametersGroupBox
        '
        Me.channelParametersGroupBox.Controls.Add(Me.physicalChannelComboBox)
        Me.channelParametersGroupBox.Controls.Add(Me.maximumValueTextBox)
        Me.channelParametersGroupBox.Controls.Add(Me.minimumValueTextBox)
        Me.channelParametersGroupBox.Controls.Add(Me.maximumValueLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.minimumValueLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.physicalChannelLabel)
        Me.channelParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.channelParametersGroupBox.Location = New System.Drawing.Point(8, 8)
        Me.channelParametersGroupBox.Name = "channelParametersGroupBox"
        Me.channelParametersGroupBox.Size = New System.Drawing.Size(272, 136)
        Me.channelParametersGroupBox.TabIndex = 7
        Me.channelParametersGroupBox.TabStop = False
        Me.channelParametersGroupBox.Text = "Channel Parameters"
        '
        'physicalChannelComboBox
        '
        Me.physicalChannelComboBox.Location = New System.Drawing.Point(136, 24)
        Me.physicalChannelComboBox.Name = "physicalChannelComboBox"
        Me.physicalChannelComboBox.Size = New System.Drawing.Size(121, 21)
        Me.physicalChannelComboBox.TabIndex = 1
        Me.physicalChannelComboBox.Text = "Dev1/ao0"
        '
        'maximumValueTextBox
        '
        Me.maximumValueTextBox.Location = New System.Drawing.Point(136, 96)
        Me.maximumValueTextBox.Name = "maximumValueTextBox"
        Me.maximumValueTextBox.Size = New System.Drawing.Size(120, 20)
        Me.maximumValueTextBox.TabIndex = 5
        Me.maximumValueTextBox.Text = "0.02"
        '
        'minimumValueTextBox
        '
        Me.minimumValueTextBox.Location = New System.Drawing.Point(136, 60)
        Me.minimumValueTextBox.Name = "minimumValueTextBox"
        Me.minimumValueTextBox.Size = New System.Drawing.Size(120, 20)
        Me.minimumValueTextBox.TabIndex = 3
        Me.minimumValueTextBox.Text = "0"
        '
        'maximumValueLabel
        '
        Me.maximumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.maximumValueLabel.Location = New System.Drawing.Point(16, 96)
        Me.maximumValueLabel.Name = "maximumValueLabel"
        Me.maximumValueLabel.Size = New System.Drawing.Size(112, 16)
        Me.maximumValueLabel.TabIndex = 4
        Me.maximumValueLabel.Text = "Maximum Value (A):"
        '
        'minimumValueLabel
        '
        Me.minimumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.minimumValueLabel.Location = New System.Drawing.Point(16, 62)
        Me.minimumValueLabel.Name = "minimumValueLabel"
        Me.minimumValueLabel.Size = New System.Drawing.Size(104, 16)
        Me.minimumValueLabel.TabIndex = 2
        Me.minimumValueLabel.Text = "Minimum Value (A):"
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
        'functionGeneratorGroupBox
        '
        Me.functionGeneratorGroupBox.Controls.Add(Me.offsetLabel)
        Me.functionGeneratorGroupBox.Controls.Add(Me.offsetNumericUpDown)
        Me.functionGeneratorGroupBox.Controls.Add(Me.amplitudeLabel)
        Me.functionGeneratorGroupBox.Controls.Add(Me.amplitudeNumericUpDown)
        Me.functionGeneratorGroupBox.Controls.Add(Me.samplesPerBufferNumericUpDown)
        Me.functionGeneratorGroupBox.Controls.Add(Me.samplesPerBufferLabel)
        Me.functionGeneratorGroupBox.Controls.Add(Me.cyclesPerBufferLabel)
        Me.functionGeneratorGroupBox.Controls.Add(Me.cyclesPerBufferNumericUpDown)
        Me.functionGeneratorGroupBox.Controls.Add(Me.waveformTypeLabel)
        Me.functionGeneratorGroupBox.Controls.Add(Me.signalTypeComboBox)
        Me.functionGeneratorGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.functionGeneratorGroupBox.Location = New System.Drawing.Point(296, 8)
        Me.functionGeneratorGroupBox.Name = "functionGeneratorGroupBox"
        Me.functionGeneratorGroupBox.Size = New System.Drawing.Size(232, 208)
        Me.functionGeneratorGroupBox.TabIndex = 9
        Me.functionGeneratorGroupBox.TabStop = False
        Me.functionGeneratorGroupBox.Text = "Function Generator Parameters"
        '
        'offsetLabel
        '
        Me.offsetLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.offsetLabel.Location = New System.Drawing.Point(16, 168)
        Me.offsetLabel.Name = "offsetLabel"
        Me.offsetLabel.Size = New System.Drawing.Size(104, 23)
        Me.offsetLabel.TabIndex = 8
        Me.offsetLabel.Text = "Offset:"
        '
        'offsetNumericUpDown
        '
        Me.offsetNumericUpDown.DecimalPlaces = 3
        Me.offsetNumericUpDown.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
        Me.offsetNumericUpDown.Location = New System.Drawing.Point(120, 168)
        Me.offsetNumericUpDown.Name = "offsetNumericUpDown"
        Me.offsetNumericUpDown.Size = New System.Drawing.Size(104, 20)
        Me.offsetNumericUpDown.TabIndex = 9
        Me.offsetNumericUpDown.Value = New Decimal(New Integer() {1, 0, 0, 131072})
        '
        'amplitudeLabel
        '
        Me.amplitudeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.amplitudeLabel.Location = New System.Drawing.Point(16, 136)
        Me.amplitudeLabel.Name = "amplitudeLabel"
        Me.amplitudeLabel.Size = New System.Drawing.Size(104, 23)
        Me.amplitudeLabel.TabIndex = 6
        Me.amplitudeLabel.Text = "Amplitude:"
        '
        'amplitudeNumericUpDown
        '
        Me.amplitudeNumericUpDown.DecimalPlaces = 3
        Me.amplitudeNumericUpDown.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
        Me.amplitudeNumericUpDown.Location = New System.Drawing.Point(120, 136)
        Me.amplitudeNumericUpDown.Name = "amplitudeNumericUpDown"
        Me.amplitudeNumericUpDown.Size = New System.Drawing.Size(104, 20)
        Me.amplitudeNumericUpDown.TabIndex = 7
        Me.amplitudeNumericUpDown.Value = New Decimal(New Integer() {1, 0, 0, 131072})
        '
        'samplesPerBufferNumericUpDown
        '
        Me.samplesPerBufferNumericUpDown.Location = New System.Drawing.Point(120, 96)
        Me.samplesPerBufferNumericUpDown.Maximum = New Decimal(New Integer() {10000000, 0, 0, 0})
        Me.samplesPerBufferNumericUpDown.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.samplesPerBufferNumericUpDown.Name = "samplesPerBufferNumericUpDown"
        Me.samplesPerBufferNumericUpDown.Size = New System.Drawing.Size(104, 20)
        Me.samplesPerBufferNumericUpDown.TabIndex = 5
        Me.samplesPerBufferNumericUpDown.Value = New Decimal(New Integer() {250, 0, 0, 0})
        '
        'samplesPerBufferLabel
        '
        Me.samplesPerBufferLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.samplesPerBufferLabel.Location = New System.Drawing.Point(16, 96)
        Me.samplesPerBufferLabel.Name = "samplesPerBufferLabel"
        Me.samplesPerBufferLabel.Size = New System.Drawing.Size(96, 32)
        Me.samplesPerBufferLabel.TabIndex = 4
        Me.samplesPerBufferLabel.Text = "Samples Per Buffer:"
        '
        'cyclesPerBufferLabel
        '
        Me.cyclesPerBufferLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.cyclesPerBufferLabel.Location = New System.Drawing.Point(16, 62)
        Me.cyclesPerBufferLabel.Name = "cyclesPerBufferLabel"
        Me.cyclesPerBufferLabel.Size = New System.Drawing.Size(104, 23)
        Me.cyclesPerBufferLabel.TabIndex = 2
        Me.cyclesPerBufferLabel.Text = "Cycles Per Buffer:"
        '
        'cyclesPerBufferNumericUpDown
        '
        Me.cyclesPerBufferNumericUpDown.DecimalPlaces = 1
        Me.cyclesPerBufferNumericUpDown.Location = New System.Drawing.Point(120, 56)
        Me.cyclesPerBufferNumericUpDown.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.cyclesPerBufferNumericUpDown.Name = "cyclesPerBufferNumericUpDown"
        Me.cyclesPerBufferNumericUpDown.Size = New System.Drawing.Size(104, 20)
        Me.cyclesPerBufferNumericUpDown.TabIndex = 3
        Me.cyclesPerBufferNumericUpDown.Value = New Decimal(New Integer() {50, 0, 0, 65536})
        '
        'waveformTypeLabel
        '
        Me.waveformTypeLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.waveformTypeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.waveformTypeLabel.Location = New System.Drawing.Point(16, 26)
        Me.waveformTypeLabel.Name = "waveformTypeLabel"
        Me.waveformTypeLabel.Size = New System.Drawing.Size(88, 14)
        Me.waveformTypeLabel.TabIndex = 0
        Me.waveformTypeLabel.Text = "Waveform Type:"
        '
        'signalTypeComboBox
        '
        Me.signalTypeComboBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.signalTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.signalTypeComboBox.ItemHeight = 13
        Me.signalTypeComboBox.Location = New System.Drawing.Point(120, 24)
        Me.signalTypeComboBox.Name = "signalTypeComboBox"
        Me.signalTypeComboBox.Size = New System.Drawing.Size(104, 21)
        Me.signalTypeComboBox.TabIndex = 1
        '
        'statusCheckTimer
        '
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(536, 254)
        Me.Controls.Add(Me.TimingParametersGroupBox)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.channelParametersGroupBox)
        Me.Controls.Add(Me.functionGeneratorGroupBox)
        Me.Controls.Add(Me.stopButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Cont Generate Current Updates Wfm - Int Clk"
        Me.TimingParametersGroupBox.ResumeLayout(False)
        Me.channelParametersGroupBox.ResumeLayout(False)
        Me.functionGeneratorGroupBox.ResumeLayout(False)
        CType(Me.offsetNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.amplitudeNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.samplesPerBufferNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cyclesPerBufferNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub startButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles startButton.Click
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Try
            myTask = New Task
            myTask.AOChannels.CreateCurrentChannel(physicalChannelComboBox.Text, _
                    "", _
                    Convert.ToDouble(minimumValueTextBox.Text), _
                    Convert.ToDouble(maximumValueTextBox.Text), _
                    AOCurrentUnits.Amps)

            ' Verify the task before doing the waveform calculations
            myTask.Control(TaskAction.Verify)

            ' Calculate some waveform parameters and generate data
             Dim fGen As FunctionGenerator = new FunctionGenerator( _
                    myTask.Timing, _
                    frequencyTextBox.Text, _
                    samplesPerBufferNumericUpDown.Value.ToString(), _
                    cyclesPerBufferNumericUpDown.Value.ToString(), _
                    signalTypeComboBox.Text, _
                    amplitudeNumericUpDown.Value.ToString(), _
                    offsetNumericUpDown.Value.ToString())

            ' Configure the sample clock with the calculated rate
            myTask.Timing.ConfigureSampleClock("", _
                fGen.ResultingSampleClockRate, _
                SampleClockActiveEdge.Rising, _
                SampleQuantityMode.ContinuousSamples, _
                fGen.Data.Length)


            Dim writer As AnalogSingleChannelWriter = New AnalogSingleChannelWriter(myTask.Stream)

            Dim waveform As AnalogWaveform(Of Double) = AnalogWaveform(Of Double).FromArray1D(fGen.Data)

            ' Write data to buffer
            writer.WriteWaveform(False, waveform)
            ' Start writing out data
            myTask.Start()

            startButton.Enabled = False
            stopButton.Enabled = True
            statusCheckTimer.Enabled = True

        Catch ex As DaqException
            statusCheckTimer.Enabled = False
            MessageBox.Show(ex.Message)

            If Not myTask Is Nothing Then
                myTask.Dispose()
            End If

            stopButton.Enabled = False
            startButton.Enabled = True
        End Try
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
    End Sub

    Private Sub stopButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles stopButton.Click
        statusCheckTimer.Enabled = False
        If Not myTask Is Nothing Then
            Try
                myTask.Stop()
            Catch ex As DaqException
                MessageBox.Show(ex.Message)
            End Try
            myTask.Dispose()
            myTask = Nothing
            startButton.Enabled = True
            stopButton.Enabled = False
        End If
    End Sub

    Private Sub statusCheckTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles statusCheckTimer.Tick
        Try
            If myTask.IsDone Then
                statusCheckTimer.Enabled = False
                myTask.Stop()
                myTask.Dispose()
                startButton.Enabled = True
                stopButton.Enabled = False
            End If
        Catch ex As Exception
            statusCheckTimer.Enabled = False
            MessageBox.Show(ex.Message)
            myTask.Dispose()
            startButton.Enabled = True
            stopButton.Enabled = False
        End Try
    End Sub
End Class
