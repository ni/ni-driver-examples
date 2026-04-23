'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   MultiFunctionSyncAI_ReadDigChan
'
' Category:
'   Synchronization
'
' Description:
'   This example demonstrates how to continuously acquire analog and digital
'   data at the same time, synchronized with one another on the same device.
'
' Instructions for running:
'   1.  Select the physical channel to correspond to where your analog signal is
'       input on the DAQ device.
'   2.  Select the channel to correspond to where your digital signal is input
'       on the DAQ device.
'   3.  Enter the minimum and maximum voltage ranges.Note:  For better accuracy
'       try to match the input range to the expected voltage level of the
'       measured signal.
'   4.  Set the sample rate of the acquisition.Note:  The rate should be at
'       least twice as fast as the maximum frequency component of the signal
'       being acquired.  Note:  This example requires two DMA channels to run. 
'       If your hardware does not support two DMA channels, you need to set the
'       DataTransferMechanism property for the digital input task to use
'       interrupts.  The DataTransferMechanism property is accessible via the
'       DIChannel class. Refer to your device documentation to determine how
'       many DMA channels are supported for your hardware.
'
' Steps:
'   1.  Create an analog input voltage channel and a digital input channel.
'   2.  Set the rate for the sample clocks. Additionally, define the sample
'       modes to be continuous.
'   3.  Set the source of the digital task's sample clock to the sample clock of
'       the analog task.
'   4.  Call Task.Start() on each task to start the acquisition and
'       generation.Note: The digital input task must start before the analog
'       input task to ensure that both tasks start at the same time.
'   5.  Create an AnalogMultiChannelReader and associate it with the analog
'       input task by using the task's stream. Call
'       AnalogMultiChannelReader.BeginReadWaveform to install a callback and
'       begin the asynchronous read operation.
'   6.  Create an DigitalMultiChannelReader and associate it with the digital
'       input task by using the task's stream. Call
'       DigitalMultiChannelReader.BeginReadWaveform to install a callback and
'       begin the asynchronous read operation.
'   7.  Inside the callbacks, read the data and display it.
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
'   Make sure your signal input terminals match the Physical Channel I/O
'   controls.
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

    Private inputDataTable As DataTable = Nothing
    Private inputDataColumns As DataColumn() = Nothing
    Private analogTask As Task
    Private digitalTask As Task
    Private analogReader As AnalogMultiChannelReader
    Private digitalReader As DigitalMultiChannelReader
    Private analogCallback As AsyncCallback
    Private digitalCallback As AsyncCallback
    Private runningAnalogTask As Task
    Private runningDigitalTask As Task
    Private samples As Integer
    Private aCount As Integer
    Private dCount As Integer

    Private timingGroupBox As System.Windows.Forms.GroupBox
    Private rateNumeric As System.Windows.Forms.NumericUpDown
    Private rateLabel As System.Windows.Forms.Label
    Private WithEvents startButton As System.Windows.Forms.Button
    Private WithEvents stopButton As System.Windows.Forms.Button
    Private analogInputComboBox As System.Windows.Forms.ComboBox
    Private inputMinValNumeric As System.Windows.Forms.NumericUpDown
    Private analogChannelLabel As System.Windows.Forms.Label
    Private inputMaxValLabel As System.Windows.Forms.Label
    Private inputMinValLabel As System.Windows.Forms.Label
    Private inputMaxValNumeric As System.Windows.Forms.NumericUpDown
    Private samplesLabel As System.Windows.Forms.Label
    Private samplesNumeric As System.Windows.Forms.NumericUpDown
    Private digitalInputComboBox As System.Windows.Forms.ComboBox
    Private digitalChannelLabel As System.Windows.Forms.Label
    Private inputDataGroupBox As System.Windows.Forms.GroupBox
    Private inputDataGrid As System.Windows.Forms.DataGrid
    Private analogInputGroupBox As System.Windows.Forms.GroupBox
    Private digitalInputGroupBox As System.Windows.Forms.GroupBox
    Private components As System.ComponentModel.Container = Nothing


    Public Sub New()
        '
        ' Required for Windows Form Designer support
        '
        InitializeComponent()

        ' Initialize UI
        analogInputComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AI, PhysicalChannelAccess.External))
        digitalInputComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DOLine, PhysicalChannelAccess.External))

        If analogInputComboBox.Items.Count > 0 Then
            analogInputComboBox.SelectedIndex = 0
        End If

        If digitalInputComboBox.Items.Count > 0 Then
            digitalInputComboBox.SelectedIndex = 0
        End If

        If analogInputComboBox.Items.Count > 0 And digitalInputComboBox.Items.Count > 0 Then
            startButton.Enabled = True
        End If
        
        ' Set up the data table
        inputDataTable = New DataTable()

        inputDataGrid.DataSource = inputDataTable
    End Sub 'New


    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
            If Not (analogTask Is Nothing) Then
                runningAnalogTask = Nothing
                analogTask.Dispose()
            End If
            If Not (digitalTask Is Nothing) Then
                runningDigitalTask = Nothing
                digitalTask.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub 'Dispose


    Private Sub InitializeComponent()
        Dim resources As New System.Resources.ResourceManager(GetType(MainForm))
        Me.analogInputGroupBox = New System.Windows.Forms.GroupBox
        Me.analogInputComboBox = New System.Windows.Forms.ComboBox
        Me.inputMinValNumeric = New System.Windows.Forms.NumericUpDown
        Me.analogChannelLabel = New System.Windows.Forms.Label
        Me.inputMaxValLabel = New System.Windows.Forms.Label
        Me.inputMinValLabel = New System.Windows.Forms.Label
        Me.inputMaxValNumeric = New System.Windows.Forms.NumericUpDown
        Me.timingGroupBox = New System.Windows.Forms.GroupBox
        Me.rateNumeric = New System.Windows.Forms.NumericUpDown
        Me.samplesLabel = New System.Windows.Forms.Label
        Me.rateLabel = New System.Windows.Forms.Label
        Me.samplesNumeric = New System.Windows.Forms.NumericUpDown
        Me.startButton = New System.Windows.Forms.Button
        Me.digitalInputGroupBox = New System.Windows.Forms.GroupBox
        Me.digitalInputComboBox = New System.Windows.Forms.ComboBox
        Me.digitalChannelLabel = New System.Windows.Forms.Label
        Me.stopButton = New System.Windows.Forms.Button
        Me.inputDataGroupBox = New System.Windows.Forms.GroupBox
        Me.inputDataGrid = New System.Windows.Forms.DataGrid
        Me.analogInputGroupBox.SuspendLayout()
        CType(Me.inputMinValNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.inputMaxValNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.timingGroupBox.SuspendLayout()
        CType(Me.rateNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.samplesNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.digitalInputGroupBox.SuspendLayout()
        Me.inputDataGroupBox.SuspendLayout()
        CType(Me.inputDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        ' 
        ' analogInputGroupBox
        ' 
        Me.analogInputGroupBox.Controls.Add(Me.analogInputComboBox)
        Me.analogInputGroupBox.Controls.Add(Me.inputMinValNumeric)
        Me.analogInputGroupBox.Controls.Add(Me.analogChannelLabel)
        Me.analogInputGroupBox.Controls.Add(Me.inputMaxValLabel)
        Me.analogInputGroupBox.Controls.Add(Me.inputMinValLabel)
        Me.analogInputGroupBox.Controls.Add(Me.inputMaxValNumeric)
        Me.analogInputGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.analogInputGroupBox.Location = New System.Drawing.Point(8, 8)
        Me.analogInputGroupBox.Name = "analogInputGroupBox"
        Me.analogInputGroupBox.Size = New System.Drawing.Size(328, 112)
        Me.analogInputGroupBox.TabIndex = 0
        Me.analogInputGroupBox.TabStop = False
        Me.analogInputGroupBox.Text = "Channel Parameters - Analog Input"
        ' 
        ' analogInputComboBox
        ' 
        Me.analogInputComboBox.Location = New System.Drawing.Point(152, 24)
        Me.analogInputComboBox.Name = "analogInputComboBox"
        Me.analogInputComboBox.Size = New System.Drawing.Size(168, 21)
        Me.analogInputComboBox.TabIndex = 1
        Me.analogInputComboBox.Text = "Dev1/ai0"
        ' 
        ' inputMinValNumeric
        ' 
        Me.inputMinValNumeric.DecimalPlaces = 2
        Me.inputMinValNumeric.Location = New System.Drawing.Point(152, 80)
        Me.inputMinValNumeric.Minimum = New System.Decimal(New Integer() {100, 0, 0, -2147483648})
        Me.inputMinValNumeric.Name = "inputMinValNumeric"
        Me.inputMinValNumeric.Size = New System.Drawing.Size(168, 20)
        Me.inputMinValNumeric.TabIndex = 5
        Me.inputMinValNumeric.Value = New System.Decimal(New Integer() {10, 0, 0, -2147483648})
        ' 
        ' analogChannelLabel
        ' 
        Me.analogChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.analogChannelLabel.Location = New System.Drawing.Point(16, 26)
        Me.analogChannelLabel.Name = "analogChannelLabel"
        Me.analogChannelLabel.Size = New System.Drawing.Size(120, 16)
        Me.analogChannelLabel.TabIndex = 0
        Me.analogChannelLabel.Text = "Analog Input Channels:"
        ' 
        ' inputMaxValLabel
        ' 
        Me.inputMaxValLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.inputMaxValLabel.Location = New System.Drawing.Point(16, 54)
        Me.inputMaxValLabel.Name = "inputMaxValLabel"
        Me.inputMaxValLabel.Size = New System.Drawing.Size(96, 16)
        Me.inputMaxValLabel.TabIndex = 2
        Me.inputMaxValLabel.Text = "Maximum Value:"
        ' 
        ' inputMinValLabel
        ' 
        Me.inputMinValLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.inputMinValLabel.Location = New System.Drawing.Point(16, 82)
        Me.inputMinValLabel.Name = "inputMinValLabel"
        Me.inputMinValLabel.Size = New System.Drawing.Size(96, 16)
        Me.inputMinValLabel.TabIndex = 4
        Me.inputMinValLabel.Text = "Minimum Value:"
        ' 
        ' inputMaxValNumeric
        ' 
        Me.inputMaxValNumeric.DecimalPlaces = 2
        Me.inputMaxValNumeric.Location = New System.Drawing.Point(152, 52)
        Me.inputMaxValNumeric.Name = "inputMaxValNumeric"
        Me.inputMaxValNumeric.Size = New System.Drawing.Size(168, 20)
        Me.inputMaxValNumeric.TabIndex = 3
        Me.inputMaxValNumeric.Value = New System.Decimal(New Integer() {10, 0, 0, 0})
        ' 
        ' timingGroupBox
        ' 
        Me.timingGroupBox.Controls.Add(Me.rateNumeric)
        Me.timingGroupBox.Controls.Add(Me.samplesLabel)
        Me.timingGroupBox.Controls.Add(Me.rateLabel)
        Me.timingGroupBox.Controls.Add(Me.samplesNumeric)
        Me.timingGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.timingGroupBox.Location = New System.Drawing.Point(8, 194)
        Me.timingGroupBox.Name = "timingGroupBox"
        Me.timingGroupBox.Size = New System.Drawing.Size(328, 88)
        Me.timingGroupBox.TabIndex = 2
        Me.timingGroupBox.TabStop = False
        Me.timingGroupBox.Text = "Timing Parameters"
        ' 
        ' rateNumeric
        ' 
        Me.rateNumeric.DecimalPlaces = 2
        Me.rateNumeric.Location = New System.Drawing.Point(152, 24)
        Me.rateNumeric.Maximum = New System.Decimal(New Integer() {102400, 0, 0, 0})
        Me.rateNumeric.Name = "rateNumeric"
        Me.rateNumeric.Size = New System.Drawing.Size(168, 20)
        Me.rateNumeric.TabIndex = 1
        Me.rateNumeric.Value = New System.Decimal(New Integer() {10000, 0, 0, 0})
        ' 
        ' samplesLabel
        ' 
        Me.samplesLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.samplesLabel.Location = New System.Drawing.Point(16, 54)
        Me.samplesLabel.Name = "samplesLabel"
        Me.samplesLabel.Size = New System.Drawing.Size(120, 16)
        Me.samplesLabel.TabIndex = 2
        Me.samplesLabel.Text = "Samples to Read:"
        ' 
        ' rateLabel
        ' 
        Me.rateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.rateLabel.Location = New System.Drawing.Point(16, 26)
        Me.rateLabel.Name = "rateLabel"
        Me.rateLabel.Size = New System.Drawing.Size(96, 16)
        Me.rateLabel.TabIndex = 0
        Me.rateLabel.Text = "Sample Rate (Hz):"
        ' 
        ' samplesNumeric
        ' 
        Me.samplesNumeric.Location = New System.Drawing.Point(152, 52)
        Me.samplesNumeric.Maximum = New System.Decimal(New Integer() {1000, 0, 0, 0})
        Me.samplesNumeric.Name = "samplesNumeric"
        Me.samplesNumeric.Size = New System.Drawing.Size(168, 20)
        Me.samplesNumeric.TabIndex = 3
        Me.samplesNumeric.Value = New System.Decimal(New Integer() {1000, 0, 0, 0})
        ' 
        ' startButton
        ' 
        Me.startButton.Anchor = CType(System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left, System.Windows.Forms.AnchorStyles)
        Me.startButton.Enabled = False
        Me.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.startButton.Location = New System.Drawing.Point(347, 288)
        Me.startButton.Name = "startButton"
        Me.startButton.TabIndex = 4
        Me.startButton.Text = "Start"
        ' 
        ' digitalInputGroupBox
        ' 
        Me.digitalInputGroupBox.Controls.Add(Me.digitalInputComboBox)
        Me.digitalInputGroupBox.Controls.Add(Me.digitalChannelLabel)
        Me.digitalInputGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.digitalInputGroupBox.Location = New System.Drawing.Point(8, 128)
        Me.digitalInputGroupBox.Name = "digitalInputGroupBox"
        Me.digitalInputGroupBox.Size = New System.Drawing.Size(328, 60)
        Me.digitalInputGroupBox.TabIndex = 1
        Me.digitalInputGroupBox.TabStop = False
        Me.digitalInputGroupBox.Text = "Channel Parameters - Digital Input"
        ' 
        ' digitalInputComboBox
        ' 
        Me.digitalInputComboBox.Location = New System.Drawing.Point(152, 24)
        Me.digitalInputComboBox.Name = "digitalInputComboBox"
        Me.digitalInputComboBox.Size = New System.Drawing.Size(168, 21)
        Me.digitalInputComboBox.TabIndex = 1
        Me.digitalInputComboBox.Text = "Dev1/port0"
        ' 
        ' digitalChannelLabel
        ' 
        Me.digitalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.digitalChannelLabel.Location = New System.Drawing.Point(16, 26)
        Me.digitalChannelLabel.Name = "digitalChannelLabel"
        Me.digitalChannelLabel.Size = New System.Drawing.Size(120, 16)
        Me.digitalChannelLabel.TabIndex = 0
        Me.digitalChannelLabel.Text = "Digital Input Channels:"
        ' 
        ' stopButton
        ' 
        Me.stopButton.Anchor = CType(System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left, System.Windows.Forms.AnchorStyles)
        Me.stopButton.Enabled = False
        Me.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.stopButton.Location = New System.Drawing.Point(428, 288)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.TabIndex = 5
        Me.stopButton.Text = "Stop"
        ' 
        ' inputDataGroupBox
        ' 
        Me.inputDataGroupBox.Controls.Add(Me.inputDataGrid)
        Me.inputDataGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.inputDataGroupBox.Location = New System.Drawing.Point(344, 8)
        Me.inputDataGroupBox.Name = "inputDataGroupBox"
        Me.inputDataGroupBox.Size = New System.Drawing.Size(390, 274)
        Me.inputDataGroupBox.TabIndex = 3
        Me.inputDataGroupBox.TabStop = False
        Me.inputDataGroupBox.Text = "Input Data"
        ' 
        ' inputDataGrid
        ' 
        Me.inputDataGrid.AllowSorting = False
        Me.inputDataGrid.DataMember = ""
        Me.inputDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.inputDataGrid.Location = New System.Drawing.Point(3, 16)
        Me.inputDataGrid.Name = "inputDataGrid"
        Me.inputDataGrid.PreferredColumnWidth = 100
        Me.inputDataGrid.ReadOnly = True
        Me.inputDataGrid.Size = New System.Drawing.Size(381, 242)
        Me.inputDataGrid.TabIndex = 0
        Me.inputDataGrid.TabStop = False
        ' 
        ' MainForm
        ' 
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(746, 319)
        Me.Controls.Add(inputDataGroupBox)
        Me.Controls.Add(startButton)
        Me.Controls.Add(analogInputGroupBox)
        Me.Controls.Add(timingGroupBox)
        Me.Controls.Add(digitalInputGroupBox)
        Me.Controls.Add(stopButton)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimumSize = New System.Drawing.Size(754, 353)
        Me.Name = "MainForm"
        Me.Text = "Multi-Function Synchronization - Analog and Digital Input"
        Me.analogInputGroupBox.ResumeLayout(False)
        CType(Me.inputMinValNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.inputMaxValNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.timingGroupBox.ResumeLayout(False)
        CType(Me.rateNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.samplesNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.digitalInputGroupBox.ResumeLayout(False)
        Me.inputDataGroupBox.ResumeLayout(False)
        CType(Me.inputDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
    End Sub 'InitializeComponent

    <STAThread()> _
    Public Shared Sub Main()
        Application.EnableVisualStyles()
        Application.DoEvents()
        Application.Run(New MainForm())
    End Sub 'Main


    Private Sub MainForm_Resize(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Resize
        inputDataGroupBox.Width = Me.Width - 362
        inputDataGroupBox.Height = Me.Height - 79
        inputDataGrid.Width = Me.Width - 373
        inputDataGrid.Height = Me.Height - 111
    End Sub 'MainForm_Resize


    Private Overloads Sub ConfigNumeric(ByVal numeric As NumericUpDown, ByVal minVal As Decimal)
        numeric.Minimum = minVal
        numeric.Maximum = [Decimal].MaxValue
    End Sub 'ConfigNumeric


    Private Overloads Sub ConfigNumeric(ByVal numeric As NumericUpDown)
        ConfigNumeric(numeric, [Decimal].MinValue)
    End Sub 'ConfigNumeric


    Private Sub InitializeDataTables(ByVal rows As Integer)
        ' Clear out the data
        inputDataTable.Rows.Clear()
        inputDataTable.Columns.Clear()

        ' Get the number of columns
        aCount = analogTask.AIChannels.Count
        dCount = digitalTask.DIChannels.Count

        ' Add one column of type double
        inputDataColumns = New DataColumn(aCount + dCount - 1) {}

        ' Add analog columns
        Dim i As Integer = 0

        While i < aCount
            inputDataColumns(i) = New DataColumn()
            inputDataColumns(i).DataType = GetType(Double)
            inputDataColumns(i).ColumnName = analogTask.AIChannels(i).PhysicalName
            i += 1
        End While


        While i < aCount + dCount
            inputDataColumns(i) = New DataColumn()
            inputDataColumns(i).DataType = GetType(Boolean)
            inputDataColumns(i).ColumnName = digitalTask.DIChannels((i - aCount)).PhysicalName
            i += 1
        End While

        inputDataTable.Columns.AddRange(inputDataColumns)

        ' Now add a certain number of rows
        For i = 0 To rows - 1
            Dim rowArr(aCount + dCount - 1) As Object
            inputDataTable.Rows.Add(rowArr)
        Next i
    End Sub 'InitializeDataTables


    Private Sub startButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles startButton.Click
        ' Change the mouse to an hourglass for the duration of this function.
        Windows.Forms.Cursor.Current = Cursors.WaitCursor

        ' Read UI selections
        samples = Convert.ToInt32(samplesNumeric.Value)

        Try
            ' Create the master and slave tasks
            analogTask = New Task("analogTask")
            digitalTask = New Task("digitalTask")

            ' Configure both tasks with the values selected on the UI.
            analogTask.AIChannels.CreateVoltageChannel(analogInputComboBox.Text, "", AITerminalConfiguration.Differential, Convert.ToDouble(inputMinValNumeric.Value), Convert.ToDouble(inputMaxValNumeric.Value), AIVoltageUnits.Volts)

            digitalTask.DIChannels.CreateChannel(digitalInputComboBox.Text, "", ChannelLineGrouping.OneChannelForEachLine)

            ' Set up the timing for the first task
            analogTask.Timing.ConfigureSampleClock("", Convert.ToDouble(rateNumeric.Value), SampleClockActiveEdge.Rising, SampleQuantityMode.ContinuousSamples, samples)

            ' Use the same timebase for the second task
            Dim deviceName As String = analogInputComboBox.Text.Split("/"c)(0)
            Dim terminalNameBase As String = "/" + GetDeviceName(deviceName) + "/"

            digitalTask.Timing.ConfigureSampleClock(terminalNameBase + "ai/SampleClock", Convert.ToDouble(rateNumeric.Value), SampleClockActiveEdge.Rising, SampleQuantityMode.ContinuousSamples, Convert.ToInt32(samplesNumeric.Value))

            ' Verify the tasks
            analogTask.Control(TaskAction.Verify)
            digitalTask.Control(TaskAction.Verify)

            ' Set up the data table
            InitializeDataTables(Math.Min((inputDataGrid.Height - 50) / 17, samples))

            ' Officially start the task
            StartTask()

            digitalTask.Start()
            analogTask.Start()

            ' Start reading as well
            analogCallback = New AsyncCallback(AddressOf AnalogRead)
            analogReader = New AnalogMultiChannelReader(analogTask.Stream)

            digitalCallback = New AsyncCallback(AddressOf DigitalRead)
            digitalReader = New DigitalMultiChannelReader(digitalTask.Stream)

            analogReader.SynchronizeCallbacks = True
            digitalReader.SynchronizeCallbacks = True

            analogReader.BeginReadMultiSample(samples, analogCallback, analogTask) '
            digitalReader.BeginReadWaveform(samples, digitalCallback, digitalTask)
        Catch ex As Exception
            StopTask()
            MessageBox.Show(ex.Message)
        End Try
    End Sub 'startButton_Click


    Private Sub AnalogRead(ByVal ar As IAsyncResult)
        Try
            If (Not (runningAnalogTask Is Nothing)) AndAlso ar.AsyncState.Equals(runningAnalogTask) Then
                ' Read the data
                Dim data As Double(,) = analogReader.EndReadMultiSample(ar)

                ' Display the data
                Dim i As Integer
                For i = 0 To Math.Min(inputDataTable.Rows.Count, data.GetLength(1)) - 1
                    Dim j As Integer
                    For j = 0 To (data.GetLength(0)) - 1
                        inputDataTable.Rows(i)(j) = data(j, i)
                    Next j
                Next i

                ' Set up next callback
                analogReader.BeginReadMultiSample(samples, analogCallback, analogTask)
            End If
        Catch ex As Exception
            StopTask()
            MessageBox.Show(ex.Message)
        End Try
    End Sub 'AnalogRead


    Private Sub DigitalRead(ByVal ar As IAsyncResult)
        Try
            If (Not (runningDigitalTask Is Nothing)) AndAlso ar.AsyncState.Equals(runningDigitalTask) Then
                ' Read the data
                Dim data As DigitalWaveform() = digitalReader.EndReadWaveform(ar)

                ' Display the data
                Dim i As Integer
                For i = 0 To Math.Min(inputDataTable.Rows.Count, data(0).Samples.Count) - 1
                    Dim j As Integer
                    For j = 0 To (data.GetLength(0)) - 1
                        inputDataTable.Rows(i)((aCount + j)) = data(j).Samples(i).States(0)
                    Next j
                Next i

                ' Set up next callback
                digitalReader.BeginReadWaveform(samples, digitalCallback, digitalTask)
            End If
        Catch ex As Exception
            StopTask()
            MessageBox.Show(ex.Message)
        End Try
    End Sub 'DigitalRead


    Private Sub stopButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles stopButton.Click
        StopTask()
    End Sub 'stopButton_Click


    Private Sub StartTask()
        If runningAnalogTask Is Nothing Then
            ' Change state
            runningAnalogTask = analogTask
            runningDigitalTask = digitalTask

            ' Fix UI
            analogInputComboBox.Enabled = False
            inputMinValNumeric.Enabled = False
            inputMaxValNumeric.Enabled = False

            digitalInputComboBox.Enabled = False
            digitalInputComboBox.Enabled = False
            digitalInputComboBox.Enabled = False

            rateNumeric.Enabled = False
            samplesNumeric.Enabled = False

            startButton.Enabled = False
            stopButton.Enabled = True
        End If
    End Sub 'StartTask


    Private Sub StopTask()
        ' Change state
        runningAnalogTask = Nothing
        runningDigitalTask = Nothing

        ' Fix UI
        analogInputComboBox.Enabled = True
        inputMinValNumeric.Enabled = True
        inputMaxValNumeric.Enabled = True

        digitalInputComboBox.Enabled = True
        digitalInputComboBox.Enabled = True
        digitalInputComboBox.Enabled = True

        rateNumeric.Enabled = True
        samplesNumeric.Enabled = True

        startButton.Enabled = True
        stopButton.Enabled = False

        ' Stop tasks
        analogTask.Stop()
        digitalTask.Stop()

        analogTask.Dispose()
        digitalTask.Dispose()
    End Sub 'StopTask


    Public Shared Function GetDeviceName(ByVal deviceName As String) As String
        Dim d As Device = DaqSystem.Local.LoadDevice(deviceName)
        If d.BusType <> DeviceBusType.CompactDaq Then
            Return deviceName
        Else
            Return d.CompactDaqChassisDeviceName
        End If
    End Function 'GetDeviceName
End Class 'MainForm

