'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   MeasPulseWidthBuf_SmplClk_Cont
'
' Category:
'   CI
'
' Description:
'   This example demonstrates how to continually measure pulsewidths on a
'   Counter Input Channel 
'   using an external sampleclock. The Maximum and Minimum Values, Sample Clock
'   Source, andSamples per Channel 
'   are all configurable.This example shows how to measure pulse width on the
'   counter'sdefault input terminal 
'   (refer to section IV, I/O ConnectionsOverview, below for more information),
'   but could easily beexpanded 
'   to measure pulse width on any PFI, RTSI, or internalsignal.Note: For sample
'   clock measurements, an external 
'   sample clock isnecessary to signal when the counter should measure asample.
'   This is set by the Sample 
'   Clock Source control.
'
' Instructions for running:
'   1.  Select the Physical Channel which corresponds to the counter you want to
'       measure pulse width on the DAQ device.
'   2.  Enter the Maximum and Minimum Value to specify the range or your unknown
'       pulse width. Note:  It is important to set the Maximum and Minimum
'       Values of your unknown pulse width as accurately as possible so the best
'       internal timebase can be chosen to minimize measurement error.  The
'       default values specify a range that can be measured by the counter using
'       the 20MhzTimebase.  Use the Gen Dig Pulse Train-Continuous example to
'       verify that you are measuring correctly on the DAQ device.
'   3.  Set the Sample Clock Source and Samples per Channel to configure timing
'       for the measurement. Note:  An external sample clock must be used. 
'       Counters do not have an internal sample clock available.  You can use
'       the Gen Dig Pulse Train-Continuous example to generate a pulse train on
'       another counter and connect it to the Sample Clock Source you are using
'       in this example.
'
' Steps:
'   1.  Create a Task.
'   2.  Create a CIChannel object using CreatePulseWidthChannel.  The
'       edgeparameter is used to determine if the counter will measure high or
'       lowpulses.
'   3.  Call the Task.Timing.ConfigureSampleClock method to configurethe
'       external sample clock timing parameters such as SampleMode and Sample
'       Clock Source. The Sample Clock Sourcedetermines when a sample will be
'       inserted into the buffer.The Edge parameter can be used to determine
'       when a sample istaken.
'   4.  Create a CounterReader object and use
'       theCounterReader.BeginReadMultiSampleDouble method to read the data
'       andregister an asynchronous callback to be called when the requested
'       datais available.
'   5.  Inside the callback, call the CounterReader.EndReadMultiSampleDouble to
'       retrieve the data from the read.
'   6.  Call BeginReadMultiSampleDouble again insidethe callback to perform
'       another read.
'   7.  When the user presses the stop button, stop the task.
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
'   The counter will measure pulses on the input terminal of thecounter
'   specified in the Physical Channel I/O control.In this example the pulse
'   width will be measured on the defaultinput terminal on ctr0. The counter
'   will take measurements onvalid edges of the external Sample Clock Source.For
'   more information on the default counter input and outputterminals for your
'   device, open the NI-DAQmx Help, and refer toCounter Signal Connections found
'   under the Device Considerationsbook in the table of contents.
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
        dataTable = New DataTable

        counterComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.CI, PhysicalChannelAccess.External))
        If counterComboBox.Items.Count > 0 Then
            counterComboBox.SelectedIndex = 0
            startButton.Enabled = True
        End If

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
    Friend WithEvents stopButton As System.Windows.Forms.Button
    Friend WithEvents startButton As System.Windows.Forms.Button
    Friend WithEvents acquisitionResultGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents resultLabel As System.Windows.Forms.Label
    Friend WithEvents acquisitionDataGrid As System.Windows.Forms.DataGrid
    Friend WithEvents timingParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents sampleClockSourceTextBox As System.Windows.Forms.TextBox
    Friend WithEvents sampleClockSourceLabel As System.Windows.Forms.Label
    Friend WithEvents rateNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents samplesLabel As System.Windows.Forms.Label
    Friend WithEvents rateLabel As System.Windows.Forms.Label
    Friend WithEvents samplesPerChannelNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents channelParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents counterComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents minimumValueNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents maximumValueNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents maximumLabel As System.Windows.Forms.Label
    Friend WithEvents minimumLabel As System.Windows.Forms.Label
    Friend WithEvents counterLabel As System.Windows.Forms.Label
    Private myTask As Task
    Private runningTask As Task
    Private myCounterReader As CounterReader
    Private myCallBack As AsyncCallback
    Private data As Double()
    Private dataColumn As DataColumn() = Nothing
    Private dataTable As DataTable = New DataTable
    Private actualNumberOfSamplesRead As Integer


    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.stopButton = New System.Windows.Forms.Button
        Me.startButton = New System.Windows.Forms.Button
        Me.acquisitionResultGroupBox = New System.Windows.Forms.GroupBox
        Me.resultLabel = New System.Windows.Forms.Label
        Me.acquisitionDataGrid = New System.Windows.Forms.DataGrid
        Me.timingParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.sampleClockSourceTextBox = New System.Windows.Forms.TextBox
        Me.sampleClockSourceLabel = New System.Windows.Forms.Label
        Me.rateNumeric = New System.Windows.Forms.NumericUpDown
        Me.samplesLabel = New System.Windows.Forms.Label
        Me.rateLabel = New System.Windows.Forms.Label
        Me.samplesPerChannelNumeric = New System.Windows.Forms.NumericUpDown
        Me.channelParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.counterComboBox = New System.Windows.Forms.ComboBox
        Me.minimumValueNumeric = New System.Windows.Forms.NumericUpDown
        Me.maximumValueNumeric = New System.Windows.Forms.NumericUpDown
        Me.maximumLabel = New System.Windows.Forms.Label
        Me.minimumLabel = New System.Windows.Forms.Label
        Me.counterLabel = New System.Windows.Forms.Label
        Me.acquisitionResultGroupBox.SuspendLayout()
        CType(Me.acquisitionDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.timingParametersGroupBox.SuspendLayout()
        CType(Me.rateNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.samplesPerChannelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.channelParametersGroupBox.SuspendLayout()
        CType(Me.minimumValueNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.maximumValueNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'stopButton
        '
        Me.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.stopButton.Location = New System.Drawing.Point(152, 271)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(80, 24)
        Me.stopButton.TabIndex = 1
        Me.stopButton.Text = "Stop"
        '
        'startButton
        '
        Me.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.startButton.Location = New System.Drawing.Point(40, 271)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(80, 24)
        Me.startButton.TabIndex = 0
        Me.startButton.Text = "Start"
        '
        'acquisitionResultGroupBox
        '
        Me.acquisitionResultGroupBox.Controls.Add(Me.resultLabel)
        Me.acquisitionResultGroupBox.Controls.Add(Me.acquisitionDataGrid)
        Me.acquisitionResultGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.acquisitionResultGroupBox.Location = New System.Drawing.Point(272, 7)
        Me.acquisitionResultGroupBox.Name = "acquisitionResultGroupBox"
        Me.acquisitionResultGroupBox.Size = New System.Drawing.Size(304, 288)
        Me.acquisitionResultGroupBox.TabIndex = 4
        Me.acquisitionResultGroupBox.TabStop = False
        Me.acquisitionResultGroupBox.Text = "Acquisition Results"
        '
        'resultLabel
        '
        Me.resultLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.resultLabel.Location = New System.Drawing.Point(8, 16)
        Me.resultLabel.Name = "resultLabel"
        Me.resultLabel.Size = New System.Drawing.Size(112, 16)
        Me.resultLabel.TabIndex = 0
        Me.resultLabel.Text = "Acquisition Data (sec):"
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
        Me.acquisitionDataGrid.Size = New System.Drawing.Size(288, 248)
        Me.acquisitionDataGrid.TabIndex = 1
        Me.acquisitionDataGrid.TabStop = False
        '
        'timingParametersGroupBox
        '
        Me.timingParametersGroupBox.Controls.Add(Me.sampleClockSourceTextBox)
        Me.timingParametersGroupBox.Controls.Add(Me.sampleClockSourceLabel)
        Me.timingParametersGroupBox.Controls.Add(Me.rateNumeric)
        Me.timingParametersGroupBox.Controls.Add(Me.samplesLabel)
        Me.timingParametersGroupBox.Controls.Add(Me.rateLabel)
        Me.timingParametersGroupBox.Controls.Add(Me.samplesPerChannelNumeric)
        Me.timingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.timingParametersGroupBox.Location = New System.Drawing.Point(8, 135)
        Me.timingParametersGroupBox.Name = "timingParametersGroupBox"
        Me.timingParametersGroupBox.Size = New System.Drawing.Size(256, 120)
        Me.timingParametersGroupBox.TabIndex = 3
        Me.timingParametersGroupBox.TabStop = False
        Me.timingParametersGroupBox.Text = "Timing Parameters"
        '
        'sampleClockSourceTextBox
        '
        Me.sampleClockSourceTextBox.Location = New System.Drawing.Point(152, 92)
        Me.sampleClockSourceTextBox.Name = "sampleClockSourceTextBox"
        Me.sampleClockSourceTextBox.Size = New System.Drawing.Size(96, 20)
        Me.sampleClockSourceTextBox.TabIndex = 5
        Me.sampleClockSourceTextBox.Text = "/Dev1/PFI0"
        '
        'sampleClockSourceLabel
        '
        Me.sampleClockSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.sampleClockSourceLabel.Location = New System.Drawing.Point(8, 96)
        Me.sampleClockSourceLabel.Name = "sampleClockSourceLabel"
        Me.sampleClockSourceLabel.Size = New System.Drawing.Size(112, 16)
        Me.sampleClockSourceLabel.TabIndex = 4
        Me.sampleClockSourceLabel.Text = "Sample Clock Source:"
        '
        'rateNumeric
        '
        Me.rateNumeric.DecimalPlaces = 2
        Me.rateNumeric.Location = New System.Drawing.Point(152, 56)
        Me.rateNumeric.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.rateNumeric.Name = "rateNumeric"
        Me.rateNumeric.Size = New System.Drawing.Size(96, 20)
        Me.rateNumeric.TabIndex = 3
        Me.rateNumeric.Value = New Decimal(New Integer() {1000, 0, 0, 0})
        '
        'samplesLabel
        '
        Me.samplesLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.samplesLabel.Location = New System.Drawing.Point(8, 26)
        Me.samplesLabel.Name = "samplesLabel"
        Me.samplesLabel.Size = New System.Drawing.Size(104, 16)
        Me.samplesLabel.TabIndex = 0
        Me.samplesLabel.Text = "Samples/Channel:"
        '
        'rateLabel
        '
        Me.rateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.rateLabel.Location = New System.Drawing.Point(8, 58)
        Me.rateLabel.Name = "rateLabel"
        Me.rateLabel.Size = New System.Drawing.Size(56, 16)
        Me.rateLabel.TabIndex = 2
        Me.rateLabel.Text = "Rate (Hz):"
        '
        'samplesPerChannelNumeric
        '
        Me.samplesPerChannelNumeric.Location = New System.Drawing.Point(152, 24)
        Me.samplesPerChannelNumeric.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.samplesPerChannelNumeric.Name = "samplesPerChannelNumeric"
        Me.samplesPerChannelNumeric.Size = New System.Drawing.Size(96, 20)
        Me.samplesPerChannelNumeric.TabIndex = 1
        Me.samplesPerChannelNumeric.Value = New Decimal(New Integer() {1000, 0, 0, 0})
        '
        'channelParametersGroupBox
        '
        Me.channelParametersGroupBox.Controls.Add(Me.counterComboBox)
        Me.channelParametersGroupBox.Controls.Add(Me.minimumValueNumeric)
        Me.channelParametersGroupBox.Controls.Add(Me.maximumValueNumeric)
        Me.channelParametersGroupBox.Controls.Add(Me.maximumLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.minimumLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.counterLabel)
        Me.channelParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.channelParametersGroupBox.Location = New System.Drawing.Point(8, 7)
        Me.channelParametersGroupBox.Name = "channelParametersGroupBox"
        Me.channelParametersGroupBox.Size = New System.Drawing.Size(256, 120)
        Me.channelParametersGroupBox.TabIndex = 2
        Me.channelParametersGroupBox.TabStop = False
        Me.channelParametersGroupBox.Text = "Channel Parameters"
        '
        'counterComboBox
        '
        Me.counterComboBox.Location = New System.Drawing.Point(152, 24)
        Me.counterComboBox.Name = "counterComboBox"
        Me.counterComboBox.Size = New System.Drawing.Size(96, 21)
        Me.counterComboBox.TabIndex = 1
        Me.counterComboBox.Text = "Dev1/ai0"
        '
        'minimumValueNumeric
        '
        Me.minimumValueNumeric.DecimalPlaces = 9
        Me.minimumValueNumeric.Location = New System.Drawing.Point(152, 56)
        Me.minimumValueNumeric.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.minimumValueNumeric.Minimum = New Decimal(New Integer() {10, 0, 0, -2147483648})
        Me.minimumValueNumeric.Name = "minimumValueNumeric"
        Me.minimumValueNumeric.Size = New System.Drawing.Size(96, 20)
        Me.minimumValueNumeric.TabIndex = 3
        Me.minimumValueNumeric.Value = New Decimal(New Integer() {100, 0, 0, 589824})
        '
        'maximumValueNumeric
        '
        Me.maximumValueNumeric.DecimalPlaces = 9
        Me.maximumValueNumeric.Location = New System.Drawing.Point(152, 88)
        Me.maximumValueNumeric.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.maximumValueNumeric.Minimum = New Decimal(New Integer() {10, 0, 0, -2147483648})
        Me.maximumValueNumeric.Name = "maximumValueNumeric"
        Me.maximumValueNumeric.Size = New System.Drawing.Size(96, 20)
        Me.maximumValueNumeric.TabIndex = 5
        Me.maximumValueNumeric.Value = New Decimal(New Integer() {838860750, 0, 0, 589824})
        '
        'maximumLabel
        '
        Me.maximumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.maximumLabel.Location = New System.Drawing.Point(8, 88)
        Me.maximumLabel.Name = "maximumLabel"
        Me.maximumLabel.Size = New System.Drawing.Size(112, 16)
        Me.maximumLabel.TabIndex = 4
        Me.maximumLabel.Text = "Maximum Value (sec):"
        '
        'minimumLabel
        '
        Me.minimumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.minimumLabel.Location = New System.Drawing.Point(8, 56)
        Me.minimumLabel.Name = "minimumLabel"
        Me.minimumLabel.Size = New System.Drawing.Size(104, 15)
        Me.minimumLabel.TabIndex = 2
        Me.minimumLabel.Text = "Minimum Value (sec):"
        '
        'counterLabel
        '
        Me.counterLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.counterLabel.Location = New System.Drawing.Point(8, 26)
        Me.counterLabel.Name = "counterLabel"
        Me.counterLabel.Size = New System.Drawing.Size(96, 16)
        Me.counterLabel.TabIndex = 0
        Me.counterLabel.Text = "Physical Channel:"
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(584, 302)
        Me.Controls.Add(Me.stopButton)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.acquisitionResultGroupBox)
        Me.Controls.Add(Me.timingParametersGroupBox)
        Me.Controls.Add(Me.channelParametersGroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Measure Pulse Width Buffered - Sample Clock - Continuous"
        Me.acquisitionResultGroupBox.ResumeLayout(False)
        CType(Me.acquisitionDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.timingParametersGroupBox.ResumeLayout(False)
        CType(Me.rateNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.samplesPerChannelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.channelParametersGroupBox.ResumeLayout(False)
        CType(Me.minimumValueNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.maximumValueNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub startButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles startButton.Click
        Try
            ' Create channel and task
            myTask = New Task

            myTask.CIChannels.CreatePulseWidthChannel( _
                    counterComboBox.Text, _
                    "", _
                    Convert.ToDouble(minimumValueNumeric.Value), _
                    Convert.ToDouble(maximumValueNumeric.Value), _
                    CIPulseWidthStartingEdge.Rising, _
                    CIPulseWidthUnits.Seconds)

            ' Configure the timing parameters
            myTask.Timing.ConfigureSampleClock( _
                    sampleClockSourceTextBox.Text, _
                    Convert.ToDouble(rateNumeric.Value), _
                    SampleClockActiveEdge.Rising, _
                    SampleQuantityMode.ContinuousSamples)

            ' Verify the task
            myTask.Control(TaskAction.Verify)

            ' Initialize the data table
            InitializeDataTable(myTask.CIChannels, dataTable)
            acquisitionDataGrid.DataSource = dataTable

            runningTask = myTask
            myCounterReader = New CounterReader(myTask.Stream)

            ' Use SynchronizeCallbacks to specify that the object 
                ' marshals callbacks across threads appropriately.
                myCounterReader.SynchronizeCallbacks = True

            myCallBack = New AsyncCallback(AddressOf CounterInCallback)
            ' Memory Optimized Read method needs an initialized array.
            data = New [Double](Convert.ToInt32(samplesPerChannelNumeric.Value) - 1) {}
            ' Start the read async callback
            myCounterReader.BeginReadMultiSampleDouble(Convert.ToInt32(samplesPerChannelNumeric.Value), myCallBack, myTask)

            startButton.Enabled = False
            stopButton.Enabled = True
        Catch ex As DaqException
            MessageBox.Show(ex.Message)
            myTask.Dispose()
            runningTask = Nothing
            stopButton.Enabled = False
            startButton.Enabled = True
        End Try
    End Sub

    Private Sub CounterInCallback(ByVal ar As IAsyncResult)
        Try
            If (Not (runningTask Is Nothing)) AndAlso runningTask Is ar.AsyncState Then
                ' Read the available data from the channel
                data = myCounterReader.EndMemoryOptimizedReadMultiSampleDouble(ar, actualNumberOfSamplesRead)

                ' Plot the data
                dataToDataTable(data, dataTable)

                ' Start the read async callback
                myCounterReader.BeginMemoryOptimizedReadMultiSampleDouble(Convert.ToInt32(samplesPerChannelNumeric.Value), myCallBack, myTask, data)
            End If
        Catch ex As DaqException
            ' Display Errors
            MessageBox.Show(ex.Message)
            myTask.Dispose()
            runningTask = Nothing
            stopButton.Enabled = False
            startButton.Enabled = True
        End Try
    End Sub

    Private Sub dataToDataTable(ByVal sourceArray As Double(), ByRef dataTable As dataTable)
        Try
            Dim dataCount As Integer

            If sourceArray.Length < 10 Then
                dataCount = sourceArray.Length
            Else
                dataCount = 10
            End If

            Dim currentDataIndex As Int32

            For currentDataIndex = 0 To (dataCount - 1)
                dataTable.Rows(currentDataIndex)(0) = sourceArray(currentDataIndex)
            Next
        Catch e As System.Exception
            MessageBox.Show(e.TargetSite.ToString())
            runningTask = Nothing
            myTask.Dispose()
            stopButton.Enabled = False
            startButton.Enabled = True
        End Try

    End Sub

    Public Sub InitializeDataTable(ByVal channelCollection As CIChannelCollection, ByRef data As dataTable)

        Dim numOfChannels As Int16 = channelCollection.Count
        data.Rows.Clear()
        data.Columns.Clear()
        dataColumn = New DataColumn(numOfChannels) {}
        Dim numOfRows As Int32 = 10
        Dim currentChannelIndex As Int32 = 0
        Dim currentDataIndex As Int32 = 0

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

    Private Sub stopButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles stopButton.Click
        runningTask = Nothing
        myTask.Dispose()
        stopButton.Enabled = False
        startButton.Enabled = True
    End Sub
End Class
