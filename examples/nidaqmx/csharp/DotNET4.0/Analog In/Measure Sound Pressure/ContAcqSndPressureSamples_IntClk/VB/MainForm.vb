'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   ContAcqSndPressureSamples_IntClk
'
' Category:
'   AI
'
' Description:
'   This example demonstrates how to acquire a continuous set of sound pressure
'   data using the DAQ device's internal clock.
'
' Instructions for running:
'   1.  Select the physical channel to correspond to where your signal is input
'       on the DAQ device.
'   2.  Set the maximum sound pressure in decibels.
'   3.  Select the correct input terminal configuration.
'   4.  Select the IEPE excitation source.
'   5.  Select the IEPE current, if applicable.
'   6.  Set the rate of the acquisition and number of samples.Note:  The rate
'       should be at least twice as fast as the maximum frequency component of
'       the signal being acquired.  Also, in order to avoid Error -50410 (buffer
'       overflow) it is important to make sure the rate and the number of
'       samples to read per iteration are set such that they don't fill the
'       buffer too quickly. If this error occurs try reducing the rate or
'       increasing the number of samples to read per iteration.
'   7.  Set the microphone sensitivity, in millivolts per pascal.  Note that
'       setting this value higher actually makes the results in the graph
'       smaller, and vice versa.
'
' Steps:
'   1.  Create a new analog input task.
'   2.  Create an analog input microphone channel.
'   3.  Set up the timing for the acquisition.  In this example we use the DAQ
'       device's internal clock to take a continuous number of samples.
'   4.  Create an AnalogMultiChannelReader and associate it with the task by
'       using the task's stream. Call AnalogMultiChannelReader.BeginReadWaveform
'       to install a callback and begin the asynchronous read operation.
'   5.  Inside the callback, call AnalogMultiChannelReader.EndReadWaveform to
'       retrieve the data from the read operation.  
'   6.  Call AnalogMultiChannelReader.BeginMemoryOptimizedReadWaveform
'   7.  Dispose the Task object to clean-up any resources associated with the
'       task.
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
'   Make sure your signal input terminal matches the physical channel I/O
'   control. In the default case (differential channel ai0) wire the positive
'   lead for your signal to the ACH0 pin on your DAQ device and wire the
'   negative lead for your signal to the ACH8 pin on you DAQ device.  For more
'   information on the input and output terminals for your device, open the
'   NI-DAQmx Help, and refer to the NI-DAQmx Device Terminals and Device
'   Considerations books in the table of contents.
'
' Recommended Use:
'   Create a Task object. Create the appropriate AIChannel object and configure
'   its parameters.  Configure the Timing parameters by using the Timing object.
'   Read the data by using the AnalogMultiChannelReader object. Use the
'   appropriate BeginRead method to read the data asynchronously. Use
'   Task.Dispose to stop the task and de-allocate any resources used by the
'   task.
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
    Private soundReader As AnalogMultiChannelReader
    Private sCallback As AsyncCallback
    Private dTable As DataTable
    Private dColumn As DataColumn()
    Private runningTask As Task


#Region " Windows Form Designer generated code "

    Friend WithEvents channelParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents timingParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents microphoneParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents acquisitionResultsGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents startButton As System.Windows.Forms.Button
    Friend WithEvents stopButton As System.Windows.Forms.Button
    Friend WithEvents physicalChannelLabel As System.Windows.Forms.Label
    Friend WithEvents maximumPressureLabel As System.Windows.Forms.Label
    Friend WithEvents inputTerminalComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents timingRateNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents timingRateLabel As System.Windows.Forms.Label
    Friend WithEvents timingSamplesLabel As System.Windows.Forms.Label
    Friend WithEvents timingSamplesNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents sensitivityLabel As System.Windows.Forms.Label
    Friend WithEvents sensitivityNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents maximumPressureNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents excitationLabel As System.Windows.Forms.Label
    Friend WithEvents excitationComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents currentLabel As System.Windows.Forms.Label
    Friend WithEvents currentNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents inputTerminalLabel As System.Windows.Forms.Label

    Private components As System.ComponentModel.Container = Nothing
    Friend WithEvents resultsDataGrid As System.Windows.Forms.DataGrid
    Friend WithEvents physicalChannelComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents lblNote As System.Windows.Forms.Label

    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MainForm))
        Me.channelParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.physicalChannelComboBox = New System.Windows.Forms.ComboBox
        Me.currentNumeric = New System.Windows.Forms.NumericUpDown
        Me.inputTerminalComboBox = New System.Windows.Forms.ComboBox
        Me.physicalChannelLabel = New System.Windows.Forms.Label
        Me.inputTerminalLabel = New System.Windows.Forms.Label
        Me.excitationLabel = New System.Windows.Forms.Label
        Me.excitationComboBox = New System.Windows.Forms.ComboBox
        Me.currentLabel = New System.Windows.Forms.Label
        Me.maximumPressureNumeric = New System.Windows.Forms.NumericUpDown
        Me.maximumPressureLabel = New System.Windows.Forms.Label
        Me.timingParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.timingRateNumeric = New System.Windows.Forms.NumericUpDown
        Me.timingRateLabel = New System.Windows.Forms.Label
        Me.timingSamplesLabel = New System.Windows.Forms.Label
        Me.timingSamplesNumeric = New System.Windows.Forms.NumericUpDown
        Me.microphoneParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.sensitivityLabel = New System.Windows.Forms.Label
        Me.sensitivityNumeric = New System.Windows.Forms.NumericUpDown
        Me.acquisitionResultsGroupBox = New System.Windows.Forms.GroupBox
        Me.resultsDataGrid = New System.Windows.Forms.DataGrid
        Me.startButton = New System.Windows.Forms.Button
        Me.stopButton = New System.Windows.Forms.Button
        Me.lblNote = New System.Windows.Forms.Label
        Me.channelParametersGroupBox.SuspendLayout()
        CType(Me.currentNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.maximumPressureNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.timingParametersGroupBox.SuspendLayout()
        CType(Me.timingRateNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.timingSamplesNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.microphoneParametersGroupBox.SuspendLayout()
        CType(Me.sensitivityNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.acquisitionResultsGroupBox.SuspendLayout()
        CType(Me.resultsDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'channelParametersGroupBox
        '
        Me.channelParametersGroupBox.Controls.Add(Me.physicalChannelComboBox)
        Me.channelParametersGroupBox.Controls.Add(Me.currentNumeric)
        Me.channelParametersGroupBox.Controls.Add(Me.inputTerminalComboBox)
        Me.channelParametersGroupBox.Controls.Add(Me.physicalChannelLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.inputTerminalLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.excitationLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.excitationComboBox)
        Me.channelParametersGroupBox.Controls.Add(Me.currentLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.maximumPressureNumeric)
        Me.channelParametersGroupBox.Controls.Add(Me.maximumPressureLabel)
        Me.channelParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.channelParametersGroupBox.Location = New System.Drawing.Point(8, 0)
        Me.channelParametersGroupBox.Name = "channelParametersGroupBox"
        Me.channelParametersGroupBox.Size = New System.Drawing.Size(248, 168)
        Me.channelParametersGroupBox.TabIndex = 0
        Me.channelParametersGroupBox.TabStop = False
        Me.channelParametersGroupBox.Text = "Channel Parameters"
        '
        'physicalChannelComboBox
        '
        Me.physicalChannelComboBox.Location = New System.Drawing.Point(128, 24)
        Me.physicalChannelComboBox.Name = "physicalChannelComboBox"
        Me.physicalChannelComboBox.Size = New System.Drawing.Size(112, 21)
        Me.physicalChannelComboBox.TabIndex = 0
        Me.physicalChannelComboBox.Text = "Dev1/ai0"
        '
        'currentNumeric
        '
        Me.currentNumeric.DecimalPlaces = 3
        Me.currentNumeric.Location = New System.Drawing.Point(128, 135)
        Me.currentNumeric.Name = "currentNumeric"
        Me.currentNumeric.Size = New System.Drawing.Size(112, 20)
        Me.currentNumeric.TabIndex = 5
        Me.currentNumeric.Value = New Decimal(New Integer() {4, 0, 0, 196608})
        '
        'inputTerminalComboBox
        '
        Me.inputTerminalComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.inputTerminalComboBox.Items.AddRange(New Object() {"Pseudodifferential", "Differential", "Nrse", "Rse"})
        Me.inputTerminalComboBox.Location = New System.Drawing.Point(128, 74)
        Me.inputTerminalComboBox.Name = "inputTerminalComboBox"
        Me.inputTerminalComboBox.Size = New System.Drawing.Size(112, 21)
        Me.inputTerminalComboBox.TabIndex = 2
        '
        'physicalChannelLabel
        '
        Me.physicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.physicalChannelLabel.Location = New System.Drawing.Point(8, 26)
        Me.physicalChannelLabel.Name = "physicalChannelLabel"
        Me.physicalChannelLabel.Size = New System.Drawing.Size(104, 16)
        Me.physicalChannelLabel.TabIndex = 0
        Me.physicalChannelLabel.Text = "Physical Channel:*"
        '
        'inputTerminalLabel
        '
        Me.inputTerminalLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.inputTerminalLabel.Location = New System.Drawing.Point(8, 76)
        Me.inputTerminalLabel.Name = "inputTerminalLabel"
        Me.inputTerminalLabel.Size = New System.Drawing.Size(104, 16)
        Me.inputTerminalLabel.TabIndex = 0
        Me.inputTerminalLabel.Text = "Input Terminal:"
        '
        'excitationLabel
        '
        Me.excitationLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.excitationLabel.Location = New System.Drawing.Point(8, 109)
        Me.excitationLabel.Name = "excitationLabel"
        Me.excitationLabel.Size = New System.Drawing.Size(104, 16)
        Me.excitationLabel.TabIndex = 0
        Me.excitationLabel.Text = "IEPE Excitation:"
        '
        'excitationComboBox
        '
        Me.excitationComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.excitationComboBox.Items.AddRange(New Object() {"Internal", "External", "None"})
        Me.excitationComboBox.Location = New System.Drawing.Point(128, 107)
        Me.excitationComboBox.Name = "excitationComboBox"
        Me.excitationComboBox.Size = New System.Drawing.Size(112, 21)
        Me.excitationComboBox.TabIndex = 4
        '
        'currentLabel
        '
        Me.currentLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.currentLabel.Location = New System.Drawing.Point(8, 137)
        Me.currentLabel.Name = "currentLabel"
        Me.currentLabel.Size = New System.Drawing.Size(104, 16)
        Me.currentLabel.TabIndex = 0
        Me.currentLabel.Text = "IEPE Current [A]:"
        '
        'maximumPressureNumeric
        '
        Me.maximumPressureNumeric.DecimalPlaces = 2
        Me.maximumPressureNumeric.Location = New System.Drawing.Point(128, 48)
        Me.maximumPressureNumeric.Maximum = New Decimal(New Integer() {2000, 0, 0, 0})
        Me.maximumPressureNumeric.Name = "maximumPressureNumeric"
        Me.maximumPressureNumeric.Size = New System.Drawing.Size(112, 20)
        Me.maximumPressureNumeric.TabIndex = 1
        Me.maximumPressureNumeric.Value = New Decimal(New Integer() {120, 0, 0, 0})
        '
        'maximumPressureLabel
        '
        Me.maximumPressureLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.maximumPressureLabel.Location = New System.Drawing.Point(8, 51)
        Me.maximumPressureLabel.Name = "maximumPressureLabel"
        Me.maximumPressureLabel.Size = New System.Drawing.Size(128, 16)
        Me.maximumPressureLabel.TabIndex = 0
        Me.maximumPressureLabel.Text = "Maximum Pressure [db]:"
        '
        'timingParametersGroupBox
        '
        Me.timingParametersGroupBox.Controls.Add(Me.timingRateNumeric)
        Me.timingParametersGroupBox.Controls.Add(Me.timingRateLabel)
        Me.timingParametersGroupBox.Controls.Add(Me.timingSamplesLabel)
        Me.timingParametersGroupBox.Controls.Add(Me.timingSamplesNumeric)
        Me.timingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.timingParametersGroupBox.Location = New System.Drawing.Point(8, 176)
        Me.timingParametersGroupBox.Name = "timingParametersGroupBox"
        Me.timingParametersGroupBox.Size = New System.Drawing.Size(248, 80)
        Me.timingParametersGroupBox.TabIndex = 1
        Me.timingParametersGroupBox.TabStop = False
        Me.timingParametersGroupBox.Text = "Timing Parameters"
        '
        'timingRateNumeric
        '
        Me.timingRateNumeric.DecimalPlaces = 3
        Me.timingRateNumeric.Location = New System.Drawing.Point(128, 24)
        Me.timingRateNumeric.Maximum = New Decimal(New Integer() {500000, 0, 0, 0})
        Me.timingRateNumeric.Minimum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.timingRateNumeric.Name = "timingRateNumeric"
        Me.timingRateNumeric.Size = New System.Drawing.Size(112, 20)
        Me.timingRateNumeric.TabIndex = 0
        Me.timingRateNumeric.Value = New Decimal(New Integer() {25600, 0, 0, 0})
        '
        'timingRateLabel
        '
        Me.timingRateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.timingRateLabel.Location = New System.Drawing.Point(8, 24)
        Me.timingRateLabel.Name = "timingRateLabel"
        Me.timingRateLabel.Size = New System.Drawing.Size(104, 16)
        Me.timingRateLabel.TabIndex = 0
        Me.timingRateLabel.Text = "Rate [Hz]:"
        '
        'timingSamplesLabel
        '
        Me.timingSamplesLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.timingSamplesLabel.Location = New System.Drawing.Point(8, 48)
        Me.timingSamplesLabel.Name = "timingSamplesLabel"
        Me.timingSamplesLabel.Size = New System.Drawing.Size(104, 16)
        Me.timingSamplesLabel.TabIndex = 0
        Me.timingSamplesLabel.Text = "Samples to Read:"
        '
        'timingSamplesNumeric
        '
        Me.timingSamplesNumeric.Location = New System.Drawing.Point(128, 48)
        Me.timingSamplesNumeric.Maximum = New Decimal(New Integer() {500000, 0, 0, 0})
        Me.timingSamplesNumeric.Minimum = New Decimal(New Integer() {2, 0, 0, 0})
        Me.timingSamplesNumeric.Name = "timingSamplesNumeric"
        Me.timingSamplesNumeric.Size = New System.Drawing.Size(112, 20)
        Me.timingSamplesNumeric.TabIndex = 1
        Me.timingSamplesNumeric.Value = New Decimal(New Integer() {1024, 0, 0, 0})
        '
        'microphoneParametersGroupBox
        '
        Me.microphoneParametersGroupBox.Controls.Add(Me.sensitivityLabel)
        Me.microphoneParametersGroupBox.Controls.Add(Me.sensitivityNumeric)
        Me.microphoneParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.microphoneParametersGroupBox.Location = New System.Drawing.Point(8, 264)
        Me.microphoneParametersGroupBox.Name = "microphoneParametersGroupBox"
        Me.microphoneParametersGroupBox.Size = New System.Drawing.Size(248, 56)
        Me.microphoneParametersGroupBox.TabIndex = 2
        Me.microphoneParametersGroupBox.TabStop = False
        Me.microphoneParametersGroupBox.Text = "Microphone Parameters"
        '
        'sensitivityLabel
        '
        Me.sensitivityLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.sensitivityLabel.Location = New System.Drawing.Point(8, 24)
        Me.sensitivityLabel.Name = "sensitivityLabel"
        Me.sensitivityLabel.Size = New System.Drawing.Size(104, 16)
        Me.sensitivityLabel.TabIndex = 0
        Me.sensitivityLabel.Text = "Sensitivity [mV/Pa]:"
        '
        'sensitivityNumeric
        '
        Me.sensitivityNumeric.DecimalPlaces = 3
        Me.sensitivityNumeric.Location = New System.Drawing.Point(128, 24)
        Me.sensitivityNumeric.Maximum = New Decimal(New Integer() {50000, 0, 0, 131072})
        Me.sensitivityNumeric.Minimum = New Decimal(New Integer() {1, 0, 0, 262144})
        Me.sensitivityNumeric.Name = "sensitivityNumeric"
        Me.sensitivityNumeric.Size = New System.Drawing.Size(112, 20)
        Me.sensitivityNumeric.TabIndex = 0
        Me.sensitivityNumeric.Value = New Decimal(New Integer() {50, 0, 0, 0})
        '
        'acquisitionResultsGroupBox
        '
        Me.acquisitionResultsGroupBox.Controls.Add(Me.resultsDataGrid)
        Me.acquisitionResultsGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.acquisitionResultsGroupBox.Location = New System.Drawing.Point(264, 0)
        Me.acquisitionResultsGroupBox.Name = "acquisitionResultsGroupBox"
        Me.acquisitionResultsGroupBox.Size = New System.Drawing.Size(376, 448)
        Me.acquisitionResultsGroupBox.TabIndex = 5
        Me.acquisitionResultsGroupBox.TabStop = False
        Me.acquisitionResultsGroupBox.Text = "Acquisition Results"
        '
        'resultsDataGrid
        '
        Me.resultsDataGrid.AllowSorting = False
        Me.resultsDataGrid.DataMember = ""
        Me.resultsDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.resultsDataGrid.Location = New System.Drawing.Point(3, 16)
        Me.resultsDataGrid.Name = "resultsDataGrid"
        Me.resultsDataGrid.PreferredRowHeight = 30
        Me.resultsDataGrid.ReadOnly = True
        Me.resultsDataGrid.Size = New System.Drawing.Size(365, 424)
        Me.resultsDataGrid.TabIndex = 0
        '
        'startButton
        '
        Me.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.startButton.Location = New System.Drawing.Point(48, 328)
        Me.startButton.Name = "startButton"
        Me.startButton.TabIndex = 3
        Me.startButton.Text = "Start"
        '
        'stopButton
        '
        Me.stopButton.Enabled = False
        Me.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.stopButton.Location = New System.Drawing.Point(160, 328)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.TabIndex = 4
        Me.stopButton.Text = "Stop"
        '
        'lblNote
        '
        Me.lblNote.Location = New System.Drawing.Point(16, 368)
        Me.lblNote.Name = "lblNote"
        Me.lblNote.Size = New System.Drawing.Size(240, 80)
        Me.lblNote.TabIndex = 6
        Me.lblNote.Text = "* Note: DSA devices now support including channels from multiple devices in a sin" & _
        "gle task.  DAQmx automatically synchronizes the devices in such a task.  See the" & _
        " DAQmx Help >> Device Considerations >> Multi Device Tasks section for further d" & _
        "etails."
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(650, 456)
        Me.Controls.Add(Me.lblNote)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.acquisitionResultsGroupBox)
        Me.Controls.Add(Me.microphoneParametersGroupBox)
        Me.Controls.Add(Me.timingParametersGroupBox)
        Me.Controls.Add(Me.channelParametersGroupBox)
        Me.Controls.Add(Me.stopButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Continuous Acquisition of Sound Pressure Samples - Internal Clock"
        Me.channelParametersGroupBox.ResumeLayout(False)
        CType(Me.currentNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.maximumPressureNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.timingParametersGroupBox.ResumeLayout(False)
        CType(Me.timingRateNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.timingSamplesNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.microphoneParametersGroupBox.ResumeLayout(False)
        CType(Me.sensitivityNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.acquisitionResultsGroupBox.ResumeLayout(False)
        CType(Me.resultsDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub 'InitializeComponent
#End Region

    Public Sub New()
        MyBase.New()
        Application.EnableVisualStyles()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        stopButton.Enabled = False
        sCallback = New AsyncCallback(AddressOf SoundCallback)
        dTable = New DataTable

        physicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AI, PhysicalChannelAccess.External))
        If (physicalChannelComboBox.Items.Count > 0) Then
            physicalChannelComboBox.SelectedIndex = 0
        End If

    End Sub

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

    Private Sub MainForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        inputTerminalComboBox.SelectedIndex = 0
        excitationComboBox.SelectedIndex = 0
    End Sub ' MainForm_Load


    Private Sub startButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles startButton.Click
        If runningTask Is Nothing Then
            Try
                myTask = New Task()

                ' Configure the Terminal Configuration and Excitation Source with enums
                Dim terminal As AITerminalConfiguration = CType([Enum].Parse(GetType(AITerminalConfiguration), inputTerminalComboBox.Text), AITerminalConfiguration)
                Dim [source] As AIExcitationSource = CType([Enum].Parse(GetType(AIExcitationSource), excitationComboBox.Text), AIExcitationSource)

                ' Create the channel
                myTask.AIChannels.CreateMicrophoneChannel(physicalChannelComboBox.Text, "soundChannel", Convert.ToDouble(sensitivityNumeric.Value), Convert.ToDouble(maximumPressureNumeric.Value), terminal, [source], Convert.ToDouble(currentNumeric.Value), AISoundPressureUnits.Pascals)

                ' Configure the timing parameters
                myTask.Timing.ConfigureSampleClock("", Convert.ToDouble(timingRateNumeric.Value), SampleClockActiveEdge.Rising, SampleQuantityMode.ContinuousSamples, 1000)

                ' Verify the Task
                myTask.Control(TaskAction.Verify)

                InitializeDataTable(myTask.AIChannels, dTable)
                resultsDataGrid.DataSource = dTable

                ' Start running the task
                StartTask()

                ' Create the analog input sound reader
                soundReader = New AnalogMultiChannelReader(myTask.Stream)

                ' Use SynchronizeCallbacks to specify that the object 
                ' marshals callbacks across threads appropriately.
                soundReader.SynchronizeCallbacks = True
                soundReader.BeginReadWaveform(Convert.ToInt32(timingSamplesNumeric.Value), sCallback, myTask)
            Catch exception As DaqException
                ' Display Errors
                MessageBox.Show(exception.Message)
                StopTask()
            End Try
        End If
    End Sub ' startButton_Click

    Private Sub SoundCallback(ByVal ar As IAsyncResult)
        Try
            If (Not (runningTask Is Nothing)) AndAlso runningTask Is ar.AsyncState Then
                ' Read the available data from the channels
                Dim data As AnalogWaveform(Of Double)() = soundReader.EndReadWaveform(ar)

                ' Plot your data here
                dataToDataTable(data, dTable)

                ' Set up a new callback
                soundReader.BeginMemoryOptimizedReadWaveform(Convert.ToInt32(timingSamplesNumeric.Value), sCallback, myTask, data)
            End If
        Catch exception As DaqException
            ' Display Errors
            MessageBox.Show(exception.Message)
            StopTask()
        End Try
    End Sub ' SoundCallback

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

    Public Sub InitializeDataTable(ByVal channelCollection As AIChannelCollection, ByRef data As DataTable)
        Dim numOfChannels As Int16 = channelCollection.Count
        data.Rows.Clear()
        data.Columns.Clear()
        dColumn = New DataColumn(numOfChannels) {}
        Dim numOfRows As Int16 = 10
        Dim currentChannelIndex As Int16 = 0
        Dim currentDataIndex As Int16 = 0

        For currentChannelIndex = 0 To (numOfChannels - 1)
            dColumn(currentChannelIndex) = New DataColumn
            dColumn(currentChannelIndex).DataType = System.Type.GetType("System.Double")
            dColumn(currentChannelIndex).ColumnName = channelCollection(currentChannelIndex).PhysicalName
        Next

        data.Columns.AddRange(dColumn)

        For currentDataIndex = 0 To (numOfRows - 1)
            Dim rowArr As Object() = New Object(numOfChannels - 1) {}
            data.Rows.Add(rowArr)
        Next
    End Sub

    Private Sub stopButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles stopButton.Click
        If Not (runningTask Is Nothing) Then
            ' Dispose of the task
            runningTask = Nothing
            StopTask()
        End If
    End Sub ' stopButton_Click

    Private Sub StartTask()
        runningTask = myTask
        stopButton.Enabled = True
        startButton.Enabled = False

        physicalChannelComboBox.Enabled = False
        maximumPressureNumeric.Enabled = False
        inputTerminalComboBox.Enabled = False
        excitationComboBox.Enabled = False
        currentNumeric.Enabled = False
        timingRateNumeric.Enabled = False
        timingSamplesNumeric.Enabled = False
        sensitivityNumeric.Enabled = False
    End Sub ' StartTask

    Private Sub StopTask()
        runningTask = Nothing
        myTask.Dispose()
        stopButton.Enabled = False
        startButton.Enabled = True

        physicalChannelComboBox.Enabled = True
        maximumPressureNumeric.Enabled = True
        inputTerminalComboBox.Enabled = True
        excitationComboBox.Enabled = True
        currentNumeric.Enabled = True
        timingRateNumeric.Enabled = True
        timingSamplesNumeric.Enabled = True
        sensitivityNumeric.Enabled = True
    End Sub ' StopTask
End Class ' MainForm
