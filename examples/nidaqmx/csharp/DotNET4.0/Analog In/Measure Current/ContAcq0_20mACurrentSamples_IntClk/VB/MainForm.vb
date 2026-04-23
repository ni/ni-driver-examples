'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   ContAcq0_20mACurrentSamples_IntClk
'
' Category:
'   AI
'
' Description:
'   This example demonstrates how to continuously measure current using an
'   internal hardware clock for timing.
'
' Instructions for running:
'   1.  Select the physical channel to correspond to where your signal is input
'       on the DAQ device.
'   2.  Enter the minimum and maximum current ranges, in Amps.Note:  For better
'       accuracy try to match the input ranges to the expected current level of
'       the measured signal.
'   3.  Set the rate of the acquisition.  Higher values will result in faster
'       updates, approximately corresponding to samples per second.  Also, set
'       the number of samples to read at a time.
'   4.  Enter in the parameters of your current shunt resistor.  The shunt
'       resistor location will usually be "External" unless you are using an
'       SCXI current input terminal block or SCC current input module.  The
'       shunt resistor value should correspond to the shunt resistor that you
'       are using, and is specified in ohms.  If you are using an SCXI current
'       input terminal block or SCC current input module, you must select
'       "Internal" for the shunt resistor location.
'
' Steps:
'   1.  Create a Task object. Create an AI channel object by using the
'       CreateCurrentChannel method.
'   2.  Configure the clock by using the Timing.ConfigureSampleClock method.
'   3.  Create the AnalogMultiChannelReader object.
'   4.  Call AnalogMultiChannelReader.BeginReadWaveform to install a callback
'       and begin the asynchronous read operation.Inside the callback, call
'       AnalogMultiChannelReader.ReadWaveform to retrieve the data from the read
'       operation.  
'   5.  The timeout is set to 10 seconds by default.
'   6.  Call AnalogMultiChannelReader.BeginMemoryOptimizedReadWaveform
'   7.  Call Task.Stop() to stop the task.  Dispose the Task object to clean-up
'       any resources associated with the task.
'   8.  Handle any DaqExceptions, if they occur.
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
'   Make sure your signal input terminal matches the physical channel control. 
'   If you are using an external shunt resistor, make sure to hook it up in
'   parallel with the current signal you are trying to measure.  For more
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

    'Global Variables
    Private runningTask As Task
    Private data As AnalogWaveform(Of Double)()
    Private myTask As Task
    Private myAnalogReader As AnalogMultiChannelReader
    Private analogCallback As AsyncCallback

    Private dataColumn As DataColumn() = Nothing
    Private dataTable As DataTable = New DataTable 

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()
        Application.EnableVisualStyles()

        'This call is required by the Windows Form Designer.
        InitializeComponent()
        shuntResistorLocationComboxBox.SelectedIndex = 1

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
    Friend WithEvents sampleRateLabel As System.Windows.Forms.Label
    Friend WithEvents stopButton As System.Windows.Forms.Button
    Friend WithEvents startButton As System.Windows.Forms.Button
    Friend WithEvents resultLabel As System.Windows.Forms.Label
    Friend WithEvents rateNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents samplesToReadLabel As System.Windows.Forms.Label
    Friend WithEvents currentParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents shuntResistorLocationLabel As System.Windows.Forms.Label
    Friend WithEvents shuntResistorLocationComboxBox As System.Windows.Forms.ComboBox
    Friend WithEvents shuntResistorLabel As System.Windows.Forms.Label
    Friend WithEvents channelParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents physicalChannelLabel As System.Windows.Forms.Label
    Friend WithEvents maximumValueLabel As System.Windows.Forms.Label
    Friend WithEvents minimumValueLabel As System.Windows.Forms.Label
    Friend WithEvents acquisitionResultsGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents acquisitionDataGrid As System.Windows.Forms.DataGrid
    Friend WithEvents shuntResistorNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents samplesToReadNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents timingParametersGroupbox As System.Windows.Forms.GroupBox
    Friend WithEvents maximumValueNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents minimumValueNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents physicalChannelComboBox As System.Windows.Forms.ComboBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MainForm))
        Me.timingParametersGroupbox = New System.Windows.Forms.GroupBox
        Me.rateNumeric = New System.Windows.Forms.NumericUpDown
        Me.sampleRateLabel = New System.Windows.Forms.Label
        Me.samplesToReadLabel = New System.Windows.Forms.Label
        Me.samplesToReadNumeric = New System.Windows.Forms.NumericUpDown
        Me.currentParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.shuntResistorNumeric = New System.Windows.Forms.NumericUpDown
        Me.shuntResistorLocationComboxBox = New System.Windows.Forms.ComboBox
        Me.shuntResistorLocationLabel = New System.Windows.Forms.Label
        Me.shuntResistorLabel = New System.Windows.Forms.Label
        Me.channelParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.physicalChannelComboBox = New System.Windows.Forms.ComboBox
        Me.minimumValueNumeric = New System.Windows.Forms.NumericUpDown
        Me.maximumValueNumeric = New System.Windows.Forms.NumericUpDown
        Me.physicalChannelLabel = New System.Windows.Forms.Label
        Me.maximumValueLabel = New System.Windows.Forms.Label
        Me.minimumValueLabel = New System.Windows.Forms.Label
        Me.stopButton = New System.Windows.Forms.Button
        Me.startButton = New System.Windows.Forms.Button
        Me.acquisitionResultsGroupBox = New System.Windows.Forms.GroupBox
        Me.acquisitionDataGrid = New System.Windows.Forms.DataGrid
        Me.resultLabel = New System.Windows.Forms.Label
        Me.timingParametersGroupbox.SuspendLayout()
        CType(Me.rateNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.samplesToReadNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.currentParametersGroupBox.SuspendLayout()
        CType(Me.shuntResistorNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.channelParametersGroupBox.SuspendLayout()
        CType(Me.minimumValueNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.maximumValueNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.acquisitionResultsGroupBox.SuspendLayout()
        CType(Me.acquisitionDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'timingParametersGroupbox
        '
        Me.timingParametersGroupbox.Controls.Add(Me.rateNumeric)
        Me.timingParametersGroupbox.Controls.Add(Me.sampleRateLabel)
        Me.timingParametersGroupbox.Controls.Add(Me.samplesToReadLabel)
        Me.timingParametersGroupbox.Controls.Add(Me.samplesToReadNumeric)
        Me.timingParametersGroupbox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.timingParametersGroupbox.Location = New System.Drawing.Point(8, 136)
        Me.timingParametersGroupbox.Name = "timingParametersGroupbox"
        Me.timingParametersGroupbox.Size = New System.Drawing.Size(232, 88)
        Me.timingParametersGroupbox.TabIndex = 3
        Me.timingParametersGroupbox.TabStop = False
        Me.timingParametersGroupbox.Text = "Timing Parameters"
        '
        'rateNumeric
        '
        Me.rateNumeric.DecimalPlaces = 2
        Me.rateNumeric.Location = New System.Drawing.Point(144, 24)
        Me.rateNumeric.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.rateNumeric.Name = "rateNumeric"
        Me.rateNumeric.Size = New System.Drawing.Size(80, 20)
        Me.rateNumeric.TabIndex = 1
        Me.rateNumeric.Value = New Decimal(New Integer() {1000, 0, 0, 0})
        '
        'sampleRateLabel
        '
        Me.sampleRateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.sampleRateLabel.Location = New System.Drawing.Point(16, 26)
        Me.sampleRateLabel.Name = "sampleRateLabel"
        Me.sampleRateLabel.Size = New System.Drawing.Size(100, 16)
        Me.sampleRateLabel.TabIndex = 0
        Me.sampleRateLabel.Text = "Sample Rate (Hz):"
        '
        'samplesToReadLabel
        '
        Me.samplesToReadLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.samplesToReadLabel.Location = New System.Drawing.Point(16, 58)
        Me.samplesToReadLabel.Name = "samplesToReadLabel"
        Me.samplesToReadLabel.Size = New System.Drawing.Size(112, 16)
        Me.samplesToReadLabel.TabIndex = 2
        Me.samplesToReadLabel.Text = "Samples to Read:"
        '
        'samplesToReadNumeric
        '
        Me.samplesToReadNumeric.Location = New System.Drawing.Point(144, 56)
        Me.samplesToReadNumeric.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.samplesToReadNumeric.Name = "samplesToReadNumeric"
        Me.samplesToReadNumeric.Size = New System.Drawing.Size(80, 20)
        Me.samplesToReadNumeric.TabIndex = 3
        Me.samplesToReadNumeric.Value = New Decimal(New Integer() {1000, 0, 0, 0})
        '
        'currentParametersGroupBox
        '
        Me.currentParametersGroupBox.Controls.Add(Me.shuntResistorNumeric)
        Me.currentParametersGroupBox.Controls.Add(Me.shuntResistorLocationComboxBox)
        Me.currentParametersGroupBox.Controls.Add(Me.shuntResistorLocationLabel)
        Me.currentParametersGroupBox.Controls.Add(Me.shuntResistorLabel)
        Me.currentParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.currentParametersGroupBox.Location = New System.Drawing.Point(8, 240)
        Me.currentParametersGroupBox.Name = "currentParametersGroupBox"
        Me.currentParametersGroupBox.Size = New System.Drawing.Size(232, 88)
        Me.currentParametersGroupBox.TabIndex = 4
        Me.currentParametersGroupBox.TabStop = False
        Me.currentParametersGroupBox.Text = "Current Parameters"
        '
        'shuntResistorNumeric
        '
        Me.shuntResistorNumeric.DecimalPlaces = 2
        Me.shuntResistorNumeric.Location = New System.Drawing.Point(144, 56)
        Me.shuntResistorNumeric.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.shuntResistorNumeric.Name = "shuntResistorNumeric"
        Me.shuntResistorNumeric.Size = New System.Drawing.Size(80, 20)
        Me.shuntResistorNumeric.TabIndex = 3
        Me.shuntResistorNumeric.Value = New Decimal(New Integer() {249, 0, 0, 0})
        '
        'shuntResistorLocationComboxBox
        '
        Me.shuntResistorLocationComboxBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.shuntResistorLocationComboxBox.Items.AddRange(New Object() {"Internal", "External"})
        Me.shuntResistorLocationComboxBox.Location = New System.Drawing.Point(144, 24)
        Me.shuntResistorLocationComboxBox.Name = "shuntResistorLocationComboxBox"
        Me.shuntResistorLocationComboxBox.Size = New System.Drawing.Size(80, 21)
        Me.shuntResistorLocationComboxBox.TabIndex = 1
        '
        'shuntResistorLocationLabel
        '
        Me.shuntResistorLocationLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.shuntResistorLocationLabel.Location = New System.Drawing.Point(16, 24)
        Me.shuntResistorLocationLabel.Name = "shuntResistorLocationLabel"
        Me.shuntResistorLocationLabel.Size = New System.Drawing.Size(128, 16)
        Me.shuntResistorLocationLabel.TabIndex = 0
        Me.shuntResistorLocationLabel.Text = "Shunt Resistor Location:"
        '
        'shuntResistorLabel
        '
        Me.shuntResistorLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.shuntResistorLabel.Location = New System.Drawing.Point(16, 56)
        Me.shuntResistorLabel.Name = "shuntResistorLabel"
        Me.shuntResistorLabel.Size = New System.Drawing.Size(144, 16)
        Me.shuntResistorLabel.TabIndex = 2
        Me.shuntResistorLabel.Text = "Shunt Resistor (Ohms):"
        '
        'channelParametersGroupBox
        '
        Me.channelParametersGroupBox.Controls.Add(Me.physicalChannelComboBox)
        Me.channelParametersGroupBox.Controls.Add(Me.minimumValueNumeric)
        Me.channelParametersGroupBox.Controls.Add(Me.maximumValueNumeric)
        Me.channelParametersGroupBox.Controls.Add(Me.physicalChannelLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.maximumValueLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.minimumValueLabel)
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
        Me.physicalChannelComboBox.Location = New System.Drawing.Point(144, 24)
        Me.physicalChannelComboBox.Name = "physicalChannelComboBox"
        Me.physicalChannelComboBox.Size = New System.Drawing.Size(80, 21)
        Me.physicalChannelComboBox.TabIndex = 1
        Me.physicalChannelComboBox.Text = "Dev1/ai0"
        '
        'minimumValueNumeric
        '
        Me.minimumValueNumeric.DecimalPlaces = 3
        Me.minimumValueNumeric.Location = New System.Drawing.Point(144, 88)
        Me.minimumValueNumeric.Name = "minimumValueNumeric"
        Me.minimumValueNumeric.Size = New System.Drawing.Size(80, 20)
        Me.minimumValueNumeric.TabIndex = 5
        '
        'maximumValueNumeric
        '
        Me.maximumValueNumeric.DecimalPlaces = 3
        Me.maximumValueNumeric.Location = New System.Drawing.Point(144, 56)
        Me.maximumValueNumeric.Name = "maximumValueNumeric"
        Me.maximumValueNumeric.Size = New System.Drawing.Size(80, 20)
        Me.maximumValueNumeric.TabIndex = 3
        Me.maximumValueNumeric.Value = New Decimal(New Integer() {20, 0, 0, 196608})
        '
        'physicalChannelLabel
        '
        Me.physicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.physicalChannelLabel.Location = New System.Drawing.Point(16, 26)
        Me.physicalChannelLabel.Name = "physicalChannelLabel"
        Me.physicalChannelLabel.Size = New System.Drawing.Size(100, 16)
        Me.physicalChannelLabel.TabIndex = 0
        Me.physicalChannelLabel.Text = "Physical Channel:"
        '
        'maximumValueLabel
        '
        Me.maximumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.maximumValueLabel.Location = New System.Drawing.Point(16, 58)
        Me.maximumValueLabel.Name = "maximumValueLabel"
        Me.maximumValueLabel.Size = New System.Drawing.Size(112, 16)
        Me.maximumValueLabel.TabIndex = 2
        Me.maximumValueLabel.Text = "Maximum Value (A):"
        '
        'minimumValueLabel
        '
        Me.minimumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.minimumValueLabel.Location = New System.Drawing.Point(16, 90)
        Me.minimumValueLabel.Name = "minimumValueLabel"
        Me.minimumValueLabel.Size = New System.Drawing.Size(104, 16)
        Me.minimumValueLabel.TabIndex = 4
        Me.minimumValueLabel.Text = "Minimum Value (A):"
        '
        'stopButton
        '
        Me.stopButton.Enabled = False
        Me.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.stopButton.Location = New System.Drawing.Point(376, 264)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(80, 24)
        Me.stopButton.TabIndex = 1
        Me.stopButton.Text = "Stop"
        '
        'startButton
        '
        Me.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.startButton.Location = New System.Drawing.Point(280, 264)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(80, 24)
        Me.startButton.TabIndex = 0
        Me.startButton.Text = "Start"
        '
        'acquisitionResultsGroupBox
        '
        Me.acquisitionResultsGroupBox.Controls.Add(Me.acquisitionDataGrid)
        Me.acquisitionResultsGroupBox.Controls.Add(Me.resultLabel)
        Me.acquisitionResultsGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.acquisitionResultsGroupBox.Location = New System.Drawing.Point(248, 8)
        Me.acquisitionResultsGroupBox.Name = "acquisitionResultsGroupBox"
        Me.acquisitionResultsGroupBox.Size = New System.Drawing.Size(240, 240)
        Me.acquisitionResultsGroupBox.TabIndex = 5
        Me.acquisitionResultsGroupBox.TabStop = False
        Me.acquisitionResultsGroupBox.Text = "Acquisition Results"
        '
        'acquisitionDataGrid
        '
        Me.acquisitionDataGrid.AllowSorting = False
        Me.acquisitionDataGrid.DataMember = ""
        Me.acquisitionDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.acquisitionDataGrid.Location = New System.Drawing.Point(8, 32)
        Me.acquisitionDataGrid.Name = "acquisitionDataGrid"
        Me.acquisitionDataGrid.ParentRowsVisible = False
        Me.acquisitionDataGrid.ReadOnly = True
        Me.acquisitionDataGrid.Size = New System.Drawing.Size(224, 200)
        Me.acquisitionDataGrid.TabIndex = 1
        Me.acquisitionDataGrid.TabStop = False
        '
        'resultLabel
        '
        Me.resultLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.resultLabel.Location = New System.Drawing.Point(8, 16)
        Me.resultLabel.Name = "resultLabel"
        Me.resultLabel.Size = New System.Drawing.Size(112, 16)
        Me.resultLabel.TabIndex = 0
        Me.resultLabel.Text = "Acquisition Data (V):"
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(498, 344)
        Me.Controls.Add(Me.timingParametersGroupbox)
        Me.Controls.Add(Me.currentParametersGroupBox)
        Me.Controls.Add(Me.channelParametersGroupBox)
        Me.Controls.Add(Me.stopButton)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.acquisitionResultsGroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Continuous Acquire 0 to 20 mA Current Samples - Internal Clock"
        Me.timingParametersGroupbox.ResumeLayout(False)
        CType(Me.rateNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.samplesToReadNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.currentParametersGroupBox.ResumeLayout(False)
        CType(Me.shuntResistorNumeric, System.ComponentModel.ISupportInitialize).EndInit()
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
            ' Create a new task
            myTask = New Task()

            ' Create a virtual channel
            Select Case shuntResistorLocationComboxBox.SelectedItem.ToString()
                Case "Internal"
                    myTask.AIChannels.CreateCurrentChannel(physicalChannelComboBox.Text, _
                    "", CType(-1, AITerminalConfiguration), Convert.ToDouble(minimumValueNumeric.Value), _
                    Convert.ToDouble(maximumValueNumeric.Value), AICurrentUnits.Amps)

                Case "External"
                    myTask.AIChannels.CreateCurrentChannel(physicalChannelComboBox.Text, _
                    "", CType(-1, AITerminalConfiguration), Convert.ToDouble(minimumValueNumeric.Value), _
                    Convert.ToDouble(maximumValueNumeric.Value), Convert.ToDouble(shuntResistorNumeric.Value), _
                    AICurrentUnits.Amps)
            End Select

            myTask.Timing.ConfigureSampleClock("", Convert.ToDouble(rateNumeric.Value), _
                    SampleClockActiveEdge.Rising, SampleQuantityMode.ContinuousSamples, 1000)

            ' Verify the Task
            myTask.Control(TaskAction.Verify)

            ' Prepare the table for Data
            InitializeDataTable(myTask.AIChannels, dataTable)
            acquisitionDataGrid.DataSource = dataTable

            ' Prepare for read
            myAnalogReader = New AnalogMultiChannelReader(myTask.Stream)
            runningTask = myTask
            analogCallback = New AsyncCallback(AddressOf AnalogInCallback)


            ' Use SynchronizeCallbacks to specify that the object 
            ' marshals callbacks across threads appropriately.
            myAnalogReader.SynchronizeCallbacks = True
            myAnalogReader.BeginReadWaveform(Convert.ToInt32(samplesToReadNumeric.Value), analogCallback, _
                    myTask)

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
        myTask.Dispose()
        startButton.Enabled = True
        stopButton.Enabled = False
        runningTask = Nothing
    End Sub

    Private Sub AnalogInCallback(ByVal ar As IAsyncResult)
        Try
            If (Not (runningTask Is Nothing)) AndAlso runningTask Is ar.AsyncState Then
                ' Retrieve data
                data = myAnalogReader.EndReadWaveform(ar)

                ' Plot your data here
                dataToDataTable(data, dataTable)

                myAnalogReader.BeginMemoryOptimizedReadWaveform(Convert.ToInt32(samplesToReadNumeric.Value), analogCallback, myTask, data)
            End If
        Catch ex As DaqException
            MessageBox.Show(ex.Message)
            runningTask = Nothing
            myTask.Dispose()
            stopButton.Enabled = False
            startButton.Enabled = True
        End Try

    End Sub

    Private Sub shuntResistComboxBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles shuntResistorLocationComboxBox.SelectedIndexChanged
        Select Case shuntResistorLocationComboxBox.SelectedItem.ToString()
            Case "Internal"
                shuntResistorNumeric.Enabled = False
            Case "External"
                shuntResistorNumeric.Enabled = True
        End Select
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

    Public Sub InitializeDataTable(ByVal channelCollection As AIChannelCollection, ByRef data As DataTable)
        Dim numOfChannels As Int16 = channelCollection.Count
        data.Rows.Clear()
        data.Columns.Clear()
        dataColumn = New DataColumn(numOfChannels) {}
        Dim numOfRows As Int16 = 10
        Dim currentChannelIndex As Int16 = 0
        Dim currentDataIndex As Int16 = 0

        For currentChannelIndex = 0 To (numOfChannels - 1)
            dataColumn(currentChannelIndex) = New DataColumn
            dataColumn(currentChannelIndex).DataType = System.Type.GetType("System.Double")
            dataColumn(currentChannelIndex).ColumnName = channelCollection(currentChannelIndex).PhysicalName
        Next

        data.Columns.AddRange(dataColumn)

        For currentDataIndex = 0 To (numOfRows - 1)
            Dim rowArr As Object() = New Object(numOfChannels - 1) {}
            data.Rows.Add(rowArr)
        Next
    End Sub
End Class
