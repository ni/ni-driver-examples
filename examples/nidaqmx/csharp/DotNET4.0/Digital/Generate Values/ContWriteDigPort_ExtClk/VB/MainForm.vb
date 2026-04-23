'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   ContWriteDigPort_ExtClk
'
' Category:
'   DO
'
' Description:
'   This example demonstrates how to output a continuous digital pattern using
'   an external clock.
'
' Instructions for running:
'   1.  Select the Physical Channel to correspond to where your signal is output
'       on the DAQ device.
'   2.  Select the Clock Source for the generation.
'   3.  Specify the Rate of the output digital pattern.
'   4.  Enter the digital pattern data.
'
' Steps:
'   1.  Create a new task.
'   2.  Create the digital output channel.
'   3.  Configure the task to use an external sample clock.
'   4.  Set the sample mode for continuous samples.
'   5.  Create a DigitalSingleChannelWriter and associate it with the task by
'       using the task's stream.
'   6.  Call the DigitalSingleChannelWriter.WriteMultiSample method to write the
'       digital pattern to a buffer.
'   7.  Call Task.Start().
'   8.  When the user presses the stop button, stop the task.
'   9.  Dispose the Task object to clean-up any resources associated with the
'       task.
'   10. Handle any DaqExceptions, if they occur.
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

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        physicalChannelsComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DOPort, PhysicalChannelAccess.External))
        clockSourceComboBox.Items.AddRange(DaqSystem.Local.GetTerminals(TerminalTypes.All))

        If (physicalChannelsComboBox.Items.Count > 0) Then
            clockSourceComboBox.SelectedIndex = 1
            physicalChannelsComboBox.SelectedIndex = 0
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
    Friend WithEvents dataToWriteGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents arrayData7Label As System.Windows.Forms.Label
    Friend WithEvents arrayData6Label As System.Windows.Forms.Label
    Friend WithEvents arrayData5Label As System.Windows.Forms.Label
    Friend WithEvents arrayData4Label As System.Windows.Forms.Label
    Friend WithEvents arrayData3Label As System.Windows.Forms.Label
    Friend WithEvents arrayData2Label As System.Windows.Forms.Label
    Friend WithEvents arrayData1Label As System.Windows.Forms.Label
    Friend WithEvents arrayData0Label As System.Windows.Forms.Label
    Friend WithEvents arrayData3NumericUpDown As System.Windows.Forms.NumericUpDown
    Friend WithEvents arrayData2NumericUpDown As System.Windows.Forms.NumericUpDown
    Friend WithEvents arrayData0NumericUpDown As System.Windows.Forms.NumericUpDown
    Friend WithEvents arrayData6NumericUpDown As System.Windows.Forms.NumericUpDown
    Friend WithEvents arrayData4NumericUpDown As System.Windows.Forms.NumericUpDown
    Friend WithEvents arrayData1NumericUpDown As System.Windows.Forms.NumericUpDown
    Friend WithEvents arrayData7NumericUpDown As System.Windows.Forms.NumericUpDown
    Friend WithEvents arrayData5NumericUpDown As System.Windows.Forms.NumericUpDown
    Friend WithEvents startButton As System.Windows.Forms.Button
    Friend WithEvents stopButton As System.Windows.Forms.Button
    Friend WithEvents timingParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents frequencyNumericUpDown As System.Windows.Forms.NumericUpDown
    Friend WithEvents frequencyLabel As System.Windows.Forms.Label
    Friend WithEvents clockSourceLabel As System.Windows.Forms.Label
    Friend WithEvents channelParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents physicalChannelsComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents physicalChannelsLabel As System.Windows.Forms.Label
    Friend WithEvents clockSourceComboBox As System.Windows.Forms.ComboBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MainForm))
        Me.dataToWriteGroupBox = New System.Windows.Forms.GroupBox
        Me.arrayData7Label = New System.Windows.Forms.Label
        Me.arrayData6Label = New System.Windows.Forms.Label
        Me.arrayData5Label = New System.Windows.Forms.Label
        Me.arrayData4Label = New System.Windows.Forms.Label
        Me.arrayData3Label = New System.Windows.Forms.Label
        Me.arrayData2Label = New System.Windows.Forms.Label
        Me.arrayData1Label = New System.Windows.Forms.Label
        Me.arrayData0Label = New System.Windows.Forms.Label
        Me.arrayData3NumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.arrayData2NumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.arrayData0NumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.arrayData6NumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.arrayData4NumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.arrayData1NumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.arrayData7NumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.arrayData5NumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.startButton = New System.Windows.Forms.Button
        Me.stopButton = New System.Windows.Forms.Button
        Me.timingParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.clockSourceComboBox = New System.Windows.Forms.ComboBox
        Me.frequencyNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.frequencyLabel = New System.Windows.Forms.Label
        Me.clockSourceLabel = New System.Windows.Forms.Label
        Me.channelParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.physicalChannelsComboBox = New System.Windows.Forms.ComboBox
        Me.physicalChannelsLabel = New System.Windows.Forms.Label
        Me.dataToWriteGroupBox.SuspendLayout()
        CType(Me.arrayData3NumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.arrayData2NumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.arrayData0NumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.arrayData6NumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.arrayData4NumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.arrayData1NumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.arrayData7NumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.arrayData5NumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.timingParametersGroupBox.SuspendLayout()
        CType(Me.frequencyNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.channelParametersGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'dataToWriteGroupBox
        '
        Me.dataToWriteGroupBox.Controls.Add(Me.arrayData7Label)
        Me.dataToWriteGroupBox.Controls.Add(Me.arrayData6Label)
        Me.dataToWriteGroupBox.Controls.Add(Me.arrayData5Label)
        Me.dataToWriteGroupBox.Controls.Add(Me.arrayData4Label)
        Me.dataToWriteGroupBox.Controls.Add(Me.arrayData3Label)
        Me.dataToWriteGroupBox.Controls.Add(Me.arrayData2Label)
        Me.dataToWriteGroupBox.Controls.Add(Me.arrayData1Label)
        Me.dataToWriteGroupBox.Controls.Add(Me.arrayData0Label)
        Me.dataToWriteGroupBox.Controls.Add(Me.arrayData3NumericUpDown)
        Me.dataToWriteGroupBox.Controls.Add(Me.arrayData2NumericUpDown)
        Me.dataToWriteGroupBox.Controls.Add(Me.arrayData0NumericUpDown)
        Me.dataToWriteGroupBox.Controls.Add(Me.arrayData6NumericUpDown)
        Me.dataToWriteGroupBox.Controls.Add(Me.arrayData4NumericUpDown)
        Me.dataToWriteGroupBox.Controls.Add(Me.arrayData1NumericUpDown)
        Me.dataToWriteGroupBox.Controls.Add(Me.arrayData7NumericUpDown)
        Me.dataToWriteGroupBox.Controls.Add(Me.arrayData5NumericUpDown)
        Me.dataToWriteGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.dataToWriteGroupBox.Location = New System.Drawing.Point(272, 8)
        Me.dataToWriteGroupBox.Name = "dataToWriteGroupBox"
        Me.dataToWriteGroupBox.Size = New System.Drawing.Size(200, 224)
        Me.dataToWriteGroupBox.TabIndex = 7
        Me.dataToWriteGroupBox.TabStop = False
        Me.dataToWriteGroupBox.Text = "Pattern"
        '
        'arrayData7Label
        '
        Me.arrayData7Label.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.arrayData7Label.Location = New System.Drawing.Point(24, 192)
        Me.arrayData7Label.Name = "arrayData7Label"
        Me.arrayData7Label.Size = New System.Drawing.Size(72, 24)
        Me.arrayData7Label.TabIndex = 0
        Me.arrayData7Label.Text = "Array Data 7:"
        '
        'arrayData6Label
        '
        Me.arrayData6Label.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.arrayData6Label.Location = New System.Drawing.Point(24, 168)
        Me.arrayData6Label.Name = "arrayData6Label"
        Me.arrayData6Label.Size = New System.Drawing.Size(72, 24)
        Me.arrayData6Label.TabIndex = 0
        Me.arrayData6Label.Text = "Array Data 6:"
        '
        'arrayData5Label
        '
        Me.arrayData5Label.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.arrayData5Label.Location = New System.Drawing.Point(24, 144)
        Me.arrayData5Label.Name = "arrayData5Label"
        Me.arrayData5Label.Size = New System.Drawing.Size(72, 24)
        Me.arrayData5Label.TabIndex = 0
        Me.arrayData5Label.Text = "Array Data 5:"
        '
        'arrayData4Label
        '
        Me.arrayData4Label.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.arrayData4Label.Location = New System.Drawing.Point(24, 120)
        Me.arrayData4Label.Name = "arrayData4Label"
        Me.arrayData4Label.Size = New System.Drawing.Size(72, 24)
        Me.arrayData4Label.TabIndex = 0
        Me.arrayData4Label.Text = "Array Data 4:"
        '
        'arrayData3Label
        '
        Me.arrayData3Label.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.arrayData3Label.Location = New System.Drawing.Point(24, 96)
        Me.arrayData3Label.Name = "arrayData3Label"
        Me.arrayData3Label.Size = New System.Drawing.Size(72, 24)
        Me.arrayData3Label.TabIndex = 0
        Me.arrayData3Label.Text = "Array Data 3:"
        '
        'arrayData2Label
        '
        Me.arrayData2Label.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.arrayData2Label.Location = New System.Drawing.Point(24, 72)
        Me.arrayData2Label.Name = "arrayData2Label"
        Me.arrayData2Label.Size = New System.Drawing.Size(72, 24)
        Me.arrayData2Label.TabIndex = 0
        Me.arrayData2Label.Text = "Array Data 2:"
        '
        'arrayData1Label
        '
        Me.arrayData1Label.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.arrayData1Label.Location = New System.Drawing.Point(24, 48)
        Me.arrayData1Label.Name = "arrayData1Label"
        Me.arrayData1Label.Size = New System.Drawing.Size(72, 24)
        Me.arrayData1Label.TabIndex = 0
        Me.arrayData1Label.Text = "Array Data 1:"
        '
        'arrayData0Label
        '
        Me.arrayData0Label.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.arrayData0Label.Location = New System.Drawing.Point(24, 24)
        Me.arrayData0Label.Name = "arrayData0Label"
        Me.arrayData0Label.Size = New System.Drawing.Size(72, 24)
        Me.arrayData0Label.TabIndex = 0
        Me.arrayData0Label.Text = "Array Data 0:"
        '
        'arrayData3NumericUpDown
        '
        Me.arrayData3NumericUpDown.Location = New System.Drawing.Point(120, 96)
        Me.arrayData3NumericUpDown.Maximum = New Decimal(New Integer() {255, 0, 0, 0})
        Me.arrayData3NumericUpDown.Name = "arrayData3NumericUpDown"
        Me.arrayData3NumericUpDown.Size = New System.Drawing.Size(64, 20)
        Me.arrayData3NumericUpDown.TabIndex = 7
        Me.arrayData3NumericUpDown.Value = New Decimal(New Integer() {8, 0, 0, 0})
        '
        'arrayData2NumericUpDown
        '
        Me.arrayData2NumericUpDown.Location = New System.Drawing.Point(120, 72)
        Me.arrayData2NumericUpDown.Maximum = New Decimal(New Integer() {255, 0, 0, 0})
        Me.arrayData2NumericUpDown.Name = "arrayData2NumericUpDown"
        Me.arrayData2NumericUpDown.Size = New System.Drawing.Size(64, 20)
        Me.arrayData2NumericUpDown.TabIndex = 6
        Me.arrayData2NumericUpDown.Value = New Decimal(New Integer() {4, 0, 0, 0})
        '
        'arrayData0NumericUpDown
        '
        Me.arrayData0NumericUpDown.Location = New System.Drawing.Point(120, 24)
        Me.arrayData0NumericUpDown.Maximum = New Decimal(New Integer() {255, 0, 0, 0})
        Me.arrayData0NumericUpDown.Name = "arrayData0NumericUpDown"
        Me.arrayData0NumericUpDown.Size = New System.Drawing.Size(64, 20)
        Me.arrayData0NumericUpDown.TabIndex = 4
        Me.arrayData0NumericUpDown.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'arrayData6NumericUpDown
        '
        Me.arrayData6NumericUpDown.Location = New System.Drawing.Point(120, 168)
        Me.arrayData6NumericUpDown.Maximum = New Decimal(New Integer() {255, 0, 0, 0})
        Me.arrayData6NumericUpDown.Name = "arrayData6NumericUpDown"
        Me.arrayData6NumericUpDown.Size = New System.Drawing.Size(64, 20)
        Me.arrayData6NumericUpDown.TabIndex = 10
        Me.arrayData6NumericUpDown.Value = New Decimal(New Integer() {64, 0, 0, 0})
        '
        'arrayData4NumericUpDown
        '
        Me.arrayData4NumericUpDown.Location = New System.Drawing.Point(120, 120)
        Me.arrayData4NumericUpDown.Maximum = New Decimal(New Integer() {255, 0, 0, 0})
        Me.arrayData4NumericUpDown.Name = "arrayData4NumericUpDown"
        Me.arrayData4NumericUpDown.Size = New System.Drawing.Size(64, 20)
        Me.arrayData4NumericUpDown.TabIndex = 8
        Me.arrayData4NumericUpDown.Value = New Decimal(New Integer() {16, 0, 0, 0})
        '
        'arrayData1NumericUpDown
        '
        Me.arrayData1NumericUpDown.Location = New System.Drawing.Point(120, 48)
        Me.arrayData1NumericUpDown.Maximum = New Decimal(New Integer() {255, 0, 0, 0})
        Me.arrayData1NumericUpDown.Name = "arrayData1NumericUpDown"
        Me.arrayData1NumericUpDown.Size = New System.Drawing.Size(64, 20)
        Me.arrayData1NumericUpDown.TabIndex = 5
        Me.arrayData1NumericUpDown.Value = New Decimal(New Integer() {2, 0, 0, 0})
        '
        'arrayData7NumericUpDown
        '
        Me.arrayData7NumericUpDown.Location = New System.Drawing.Point(120, 192)
        Me.arrayData7NumericUpDown.Maximum = New Decimal(New Integer() {255, 0, 0, 0})
        Me.arrayData7NumericUpDown.Name = "arrayData7NumericUpDown"
        Me.arrayData7NumericUpDown.Size = New System.Drawing.Size(64, 20)
        Me.arrayData7NumericUpDown.TabIndex = 11
        Me.arrayData7NumericUpDown.Value = New Decimal(New Integer() {128, 0, 0, 0})
        '
        'arrayData5NumericUpDown
        '
        Me.arrayData5NumericUpDown.Location = New System.Drawing.Point(120, 144)
        Me.arrayData5NumericUpDown.Maximum = New Decimal(New Integer() {255, 0, 0, 0})
        Me.arrayData5NumericUpDown.Name = "arrayData5NumericUpDown"
        Me.arrayData5NumericUpDown.Size = New System.Drawing.Size(64, 20)
        Me.arrayData5NumericUpDown.TabIndex = 9
        Me.arrayData5NumericUpDown.Value = New Decimal(New Integer() {32, 0, 0, 0})
        '
        'startButton
        '
        Me.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.startButton.Location = New System.Drawing.Point(40, 208)
        Me.startButton.Name = "startButton"
        Me.startButton.TabIndex = 12
        Me.startButton.Text = "&Start"
        '
        'stopButton
        '
        Me.stopButton.Enabled = False
        Me.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.stopButton.Location = New System.Drawing.Point(152, 208)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.TabIndex = 13
        Me.stopButton.Text = "S&top"
        '
        'timingParametersGroupBox
        '
        Me.timingParametersGroupBox.Controls.Add(Me.clockSourceComboBox)
        Me.timingParametersGroupBox.Controls.Add(Me.frequencyNumericUpDown)
        Me.timingParametersGroupBox.Controls.Add(Me.frequencyLabel)
        Me.timingParametersGroupBox.Controls.Add(Me.clockSourceLabel)
        Me.timingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.timingParametersGroupBox.Location = New System.Drawing.Point(8, 88)
        Me.timingParametersGroupBox.Name = "timingParametersGroupBox"
        Me.timingParametersGroupBox.Size = New System.Drawing.Size(256, 104)
        Me.timingParametersGroupBox.TabIndex = 6
        Me.timingParametersGroupBox.TabStop = False
        Me.timingParametersGroupBox.Text = "Timing Parameters"
        '
        'clockSourceComboBox
        '
        Me.clockSourceComboBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.clockSourceComboBox.Location = New System.Drawing.Point(152, 24)
        Me.clockSourceComboBox.Name = "clockSourceComboBox"
        Me.clockSourceComboBox.Size = New System.Drawing.Size(96, 21)
        Me.clockSourceComboBox.TabIndex = 2
        Me.clockSourceComboBox.Text = "/Dev1/PFI0"
        '
        'frequencyNumericUpDown
        '
        Me.frequencyNumericUpDown.Location = New System.Drawing.Point(152, 64)
        Me.frequencyNumericUpDown.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.frequencyNumericUpDown.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.frequencyNumericUpDown.Name = "frequencyNumericUpDown"
        Me.frequencyNumericUpDown.Size = New System.Drawing.Size(96, 20)
        Me.frequencyNumericUpDown.TabIndex = 3
        Me.frequencyNumericUpDown.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        'frequencyLabel
        '
        Me.frequencyLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.frequencyLabel.Location = New System.Drawing.Point(16, 64)
        Me.frequencyLabel.Name = "frequencyLabel"
        Me.frequencyLabel.Size = New System.Drawing.Size(128, 14)
        Me.frequencyLabel.TabIndex = 0
        Me.frequencyLabel.Text = "Sample Clock Rate:"
        '
        'clockSourceLabel
        '
        Me.clockSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.clockSourceLabel.Location = New System.Drawing.Point(16, 24)
        Me.clockSourceLabel.Name = "clockSourceLabel"
        Me.clockSourceLabel.Size = New System.Drawing.Size(88, 14)
        Me.clockSourceLabel.TabIndex = 0
        Me.clockSourceLabel.Text = "Clock Source:"
        '
        'channelParametersGroupBox
        '
        Me.channelParametersGroupBox.Controls.Add(Me.physicalChannelsComboBox)
        Me.channelParametersGroupBox.Controls.Add(Me.physicalChannelsLabel)
        Me.channelParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.channelParametersGroupBox.Location = New System.Drawing.Point(8, 8)
        Me.channelParametersGroupBox.Name = "channelParametersGroupBox"
        Me.channelParametersGroupBox.Size = New System.Drawing.Size(256, 56)
        Me.channelParametersGroupBox.TabIndex = 5
        Me.channelParametersGroupBox.TabStop = False
        Me.channelParametersGroupBox.Text = "Channel Parameters"
        '
        'physicalChannelsComboBox
        '
        Me.physicalChannelsComboBox.Location = New System.Drawing.Point(152, 24)
        Me.physicalChannelsComboBox.Name = "physicalChannelsComboBox"
        Me.physicalChannelsComboBox.Size = New System.Drawing.Size(96, 21)
        Me.physicalChannelsComboBox.TabIndex = 1
        Me.physicalChannelsComboBox.Text = "Dev1/port0"
        '
        'physicalChannelsLabel
        '
        Me.physicalChannelsLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.physicalChannelsLabel.Location = New System.Drawing.Point(16, 24)
        Me.physicalChannelsLabel.Name = "physicalChannelsLabel"
        Me.physicalChannelsLabel.Size = New System.Drawing.Size(112, 24)
        Me.physicalChannelsLabel.TabIndex = 0
        Me.physicalChannelsLabel.Text = "Physical Channels:"
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(498, 248)
        Me.Controls.Add(Me.dataToWriteGroupBox)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.stopButton)
        Me.Controls.Add(Me.timingParametersGroupBox)
        Me.Controls.Add(Me.channelParametersGroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Continuous Write Digital Port - External Clock"
        Me.dataToWriteGroupBox.ResumeLayout(False)
        CType(Me.arrayData3NumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.arrayData2NumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.arrayData0NumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.arrayData6NumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.arrayData4NumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.arrayData1NumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.arrayData7NumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.arrayData5NumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        Me.timingParametersGroupBox.ResumeLayout(False)
        CType(Me.frequencyNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        Me.channelParametersGroupBox.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private myTask As Task

    Private Sub startButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles startButton.Click
        Dim data(8) As Integer

        Try
            myTask = New Task

            myTask.DOChannels.CreateChannel(physicalChannelsComboBox.Text, "port", _
                ChannelLineGrouping.OneChannelForAllLines)


            myTask.Timing.ConfigureSampleClock(clockSourceComboBox.Text, _
                Convert.ToDouble(frequencyNumericUpDown.Value), _
                SampleClockActiveEdge.Rising, _
                SampleQuantityMode.ContinuousSamples)

            myTask.Control(TaskAction.Verify)

            AddHandler myTask.Done, AddressOf myTask_Done

            Dim writer As New DigitalSingleChannelWriter(myTask.Stream)

            data(0) = Integer.Parse(arrayData0NumericUpDown.Value)
            data(1) = Integer.Parse(arrayData1NumericUpDown.Value)
            data(2) = Integer.Parse(arrayData2NumericUpDown.Value)
            data(3) = Integer.Parse(arrayData3NumericUpDown.Value)
            data(4) = Integer.Parse(arrayData4NumericUpDown.Value)
            data(5) = Integer.Parse(arrayData5NumericUpDown.Value)
            data(6) = Integer.Parse(arrayData6NumericUpDown.Value)
            data(7) = Integer.Parse(arrayData7NumericUpDown.Value)

            writer.WriteMultiSamplePort(False, data)

            myTask.Start()

            startButton.Enabled = False
            stopButton.Enabled = True
            channelParametersGroupBox.Enabled = False
            timingParametersGroupBox.Enabled = False
            dataToWriteGroupBox.Enabled = False

        Catch ex As Exception

            myTask.Dispose()
            MessageBox.Show(ex.Message)

            ResetButtons()
        End Try
    End Sub

    Private Sub stopButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles stopButton.Click
        myTask.Dispose()
        ResetButtons()
    End Sub

    Private Sub myTask_Done(ByVal sender As Object, ByVal e As TaskDoneEventArgs)
        If Not e.Error Is Nothing Then
            MessageBox.Show(e.Error.Message)
        End If

        If Not myTask Is Nothing Then
            myTask.Dispose()
        End If
    End Sub

    Private Sub ResetButtons()
        startButton.Enabled = True
        stopButton.Enabled = False
        channelParametersGroupBox.Enabled = True
        timingParametersGroupBox.Enabled = True
        dataToWriteGroupBox.Enabled = True
    End Sub
End Class
