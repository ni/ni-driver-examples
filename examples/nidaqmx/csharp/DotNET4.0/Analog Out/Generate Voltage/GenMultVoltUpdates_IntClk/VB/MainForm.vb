'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   GenMultVoltUpdates_IntClk
'
' Category:
'   AO
'
' Description:
'   This example demonstrates how to output multiple voltage updates (samples)
'   to an analog output channel.
'
' Instructions for running:
'   1.  Select the physical channel corresponding to where your signal is output
'       on the DAQ device.
'   2.  Enter the minimum and maximum voltage values.
'   3.  Set the signal frequency of the waveform to write.
'   4.  Set the function generator parameters, including the waveform type,
'       cycles per buffer, and samples per buffer.
'
' Steps:
'   1.  Create a new task and an analog output voltage channel.
'   2.  Use the FunctionGenerator class to generate an array of data points
'       representing a waveform.
'   3.  Set up the timing for the generation. This example uses the onboard
'       clock to write a finite number of samples.
'   4.  Create a AnalogSingleChannelWriter, add the Task Done event handler, and
'       call the WriteMultiSample method to write multiple samples to the DAQ
'       device. The autoStart parameter is set to false, so you must explicitly
'       call Task.Start() to begin the output.
'   5.  Call Task.Start().
'   6.  Handle any DaqExceptions, if they occur.
'   7.  In the Task Done event check for any errors and Dispose the Task object
'       to clean-up any resources associated with the task.
'
' I/O Connections Overview:
'   Make sure your signal output terminal matches the text in physical channel
'   text box. In this case the signal will output to the ao0 pin on your DAQ
'   Device.  For more information on the input and output terminals for your
'   device, open the NI-DAQmx Help, and refer to the NI-DAQmx Device Terminals
'   and Device Considerations books in the table of contents.
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

Imports System.Threading
Imports NationalInstruments.DAQmx

Public Class MainForm
    Inherits System.Windows.Forms.Form

    Dim taskRunning As Boolean
    Dim myTask As Task

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()
        Application.EnableVisualStyles()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
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
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents functionGeneratorGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents waveformTypeLabel As System.Windows.Forms.Label
    Friend WithEvents signalTypeComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents writeButton As System.Windows.Forms.Button
    Friend WithEvents channelParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents maximumValueTextBox As System.Windows.Forms.TextBox
    Friend WithEvents minimumValueTextBox As System.Windows.Forms.TextBox
    Friend WithEvents maximumValueLabel As System.Windows.Forms.Label
    Friend WithEvents minimumValueLabel As System.Windows.Forms.Label
    Friend WithEvents physicalChannelLabel As System.Windows.Forms.Label
    Friend WithEvents TimingParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents frequencyTextBox As System.Windows.Forms.TextBox
    Friend WithEvents frequencyLabel As System.Windows.Forms.Label
    Friend WithEvents amplitudeLabel As System.Windows.Forms.Label
    Friend WithEvents amplitudeNumericUpDown As System.Windows.Forms.NumericUpDown
    Friend WithEvents samplesPerBufferNumericUpDown As System.Windows.Forms.NumericUpDown
    Friend WithEvents samplesPerBufferLabel As System.Windows.Forms.Label
    Friend WithEvents cyclesPerBufferLabel As System.Windows.Forms.Label
    Friend WithEvents cyclesPerBufferNumericUpDown As System.Windows.Forms.NumericUpDown
    Friend WithEvents physicalChannelComboBox As System.Windows.Forms.ComboBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.functionGeneratorGroupBox = New System.Windows.Forms.GroupBox
        Me.amplitudeLabel = New System.Windows.Forms.Label
        Me.amplitudeNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.samplesPerBufferNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.samplesPerBufferLabel = New System.Windows.Forms.Label
        Me.cyclesPerBufferLabel = New System.Windows.Forms.Label
        Me.cyclesPerBufferNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.waveformTypeLabel = New System.Windows.Forms.Label
        Me.signalTypeComboBox = New System.Windows.Forms.ComboBox
        Me.writeButton = New System.Windows.Forms.Button
        Me.channelParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.physicalChannelComboBox = New System.Windows.Forms.ComboBox
        Me.maximumValueTextBox = New System.Windows.Forms.TextBox
        Me.minimumValueTextBox = New System.Windows.Forms.TextBox
        Me.maximumValueLabel = New System.Windows.Forms.Label
        Me.minimumValueLabel = New System.Windows.Forms.Label
        Me.physicalChannelLabel = New System.Windows.Forms.Label
        Me.TimingParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.frequencyTextBox = New System.Windows.Forms.TextBox
        Me.frequencyLabel = New System.Windows.Forms.Label
        Me.functionGeneratorGroupBox.SuspendLayout()
        CType(Me.amplitudeNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.samplesPerBufferNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cyclesPerBufferNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.channelParametersGroupBox.SuspendLayout()
        Me.TimingParametersGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'functionGeneratorGroupBox
        '
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
        Me.functionGeneratorGroupBox.Size = New System.Drawing.Size(232, 168)
        Me.functionGeneratorGroupBox.TabIndex = 3
        Me.functionGeneratorGroupBox.TabStop = False
        Me.functionGeneratorGroupBox.Text = "Function Generator Parameters"
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
        Me.amplitudeNumericUpDown.DecimalPlaces = 1
        Me.amplitudeNumericUpDown.Location = New System.Drawing.Point(120, 136)
        Me.amplitudeNumericUpDown.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.amplitudeNumericUpDown.Name = "amplitudeNumericUpDown"
        Me.amplitudeNumericUpDown.Size = New System.Drawing.Size(104, 20)
        Me.amplitudeNumericUpDown.TabIndex = 7
        Me.amplitudeNumericUpDown.Value = New Decimal(New Integer() {2, 0, 0, 0})
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
        Me.cyclesPerBufferNumericUpDown.Value = New Decimal(New Integer() {5, 0, 0, 0})
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
        'writeButton
        '
        Me.writeButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.writeButton.Location = New System.Drawing.Point(368, 192)
        Me.writeButton.Name = "writeButton"
        Me.writeButton.Size = New System.Drawing.Size(75, 23)
        Me.writeButton.TabIndex = 0
        Me.writeButton.Text = "&Write"
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
        Me.channelParametersGroupBox.TabIndex = 1
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
        Me.maximumValueTextBox.Text = "10"
        '
        'minimumValueTextBox
        '
        Me.minimumValueTextBox.Location = New System.Drawing.Point(136, 60)
        Me.minimumValueTextBox.Name = "minimumValueTextBox"
        Me.minimumValueTextBox.Size = New System.Drawing.Size(120, 20)
        Me.minimumValueTextBox.TabIndex = 3
        Me.minimumValueTextBox.Text = "-10"
        '
        'maximumValueLabel
        '
        Me.maximumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.maximumValueLabel.Location = New System.Drawing.Point(16, 96)
        Me.maximumValueLabel.Name = "maximumValueLabel"
        Me.maximumValueLabel.Size = New System.Drawing.Size(112, 16)
        Me.maximumValueLabel.TabIndex = 4
        Me.maximumValueLabel.Text = "Maximum Value (V):"
        '
        'minimumValueLabel
        '
        Me.minimumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.minimumValueLabel.Location = New System.Drawing.Point(16, 62)
        Me.minimumValueLabel.Name = "minimumValueLabel"
        Me.minimumValueLabel.Size = New System.Drawing.Size(104, 16)
        Me.minimumValueLabel.TabIndex = 2
        Me.minimumValueLabel.Text = "Minimum Value (V):"
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
        'TimingParametersGroupBox
        '
        Me.TimingParametersGroupBox.Controls.Add(Me.frequencyTextBox)
        Me.TimingParametersGroupBox.Controls.Add(Me.frequencyLabel)
        Me.TimingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.TimingParametersGroupBox.Location = New System.Drawing.Point(8, 152)
        Me.TimingParametersGroupBox.Name = "TimingParametersGroupBox"
        Me.TimingParametersGroupBox.Size = New System.Drawing.Size(272, 64)
        Me.TimingParametersGroupBox.TabIndex = 2
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
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(538, 224)
        Me.Controls.Add(Me.functionGeneratorGroupBox)
        Me.Controls.Add(Me.writeButton)
        Me.Controls.Add(Me.channelParametersGroupBox)
        Me.Controls.Add(Me.TimingParametersGroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Generate Multiple Voltage Updates - Int Clk"
        Me.functionGeneratorGroupBox.ResumeLayout(False)
        CType(Me.amplitudeNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.samplesPerBufferNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cyclesPerBufferNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        Me.channelParametersGroupBox.ResumeLayout(False)
        Me.channelParametersGroupBox.PerformLayout()
        Me.TimingParametersGroupBox.ResumeLayout(False)
        Me.TimingParametersGroupBox.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub writeButton_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles writeButton.Click
        writeButton.Enabled = False
        taskRunning = True

        Try
            ' Create the task and channel
            myTask = New Task()
            myTask.AOChannels.CreateVoltageChannel( _
                physicalChannelComboBox.Text, _
                "aoChannel", _
                Double.Parse(minimumValueTextBox.Text), _
                Double.Parse(maximumValueTextBox.Text), _
                AOVoltageUnits.Volts)

            ' Verify the task before doing the waveform calculations
            myTask.Control(TaskAction.Verify)

            ' Calculate some waveform parameters and generate data
            Dim fGen As New FunctionGenerator( _
                myTask.Timing, _
                Double.Parse(frequencyTextBox.Text), _
                CDbl(samplesPerBufferNumericUpDown.Value), _
                CDbl(cyclesPerBufferNumericUpDown.Value), _
                CType(signalTypeComboBox.SelectedIndex, WaveformType), _
                CDbl(amplitudeNumericUpDown.Value))

            ' Configure the sample clock with the calculated rate
            myTask.Timing.ConfigureSampleClock( _
                "", _
                fGen.ResultingSampleClockRate, _
                SampleClockActiveEdge.Rising, _
                SampleQuantityMode.FiniteSamples, _
                fGen.Data.Length)

            ' Setup the Task Done event
            AddHandler myTask.Done, AddressOf myTask_Done

            ' Write the data
            Dim writer As New AnalogSingleChannelWriter(myTask.Stream)
            writer.WriteMultiSample(False, fGen.Data)
            myTask.Start()
        Catch x As System.Exception
            MessageBox.Show(x.Message)

            If Not (myTask Is Nothing) Then
                myTask.Dispose()
            End If

            taskRunning = False
            writeButton.Enabled = True
        End Try

    End Sub

    Private Sub myTask_Done(ByVal sender As Object, ByVal e As TaskDoneEventArgs)
        If Not (e.Error Is Nothing) Then
            MessageBox.Show(e.Error.Message)
        End If

        If Not (myTask Is Nothing) Then
            myTask.Dispose()
        End If

        taskRunning = False
        writeButton.Enabled = True

    End Sub

    Protected Overrides Sub OnClosing(ByVal e As System.ComponentModel.CancelEventArgs)
        e.Cancel = taskRunning
    End Sub
End Class
