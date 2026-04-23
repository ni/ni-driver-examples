'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   ContReadDigChan_ExtClk
'
' Category:
'   DI
'
' Description:
'   This example demonstrates how to continuously read values from a digital
'   input channel using an external sample clock.
'
' Instructions for running:
'   1.  Select the physical channel on the DAQ device.
'   2.  Select the external clock source.
'   3.  Select the number of samples per channel.
'   4.  Select the sample clock rate.
'
' Steps:
'   1.  Create a new digital input task.
'   2.  Create the digital input channel.
'   3.  Configure the task to use an external sample clock.
'   4.  Set the stream's timeout to ten seconds.
'   5.  Create a DigitalSingleChannelReader and associate it with the task by
'       using the task's stream.
'   6.  Call DigitalSingleChannelReader.BeginReadWaveform to install a callback
'       and begin the asynchronous read operation.
'   7.  Inside the callback, call
'       DigitalSingleChannelReader.EndReadSingleSampleMultiLine to retrieve the
'       data from the read.
'   8.  Call DigitalSingleChannelReader.BeginReadWaveform again inside the
'       callback to perform another read operation.
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
'   Make sure your signal input terminals match the physical channel text box. 
'   In this case wire your digital signals to the appropriate eight digital
'   lines on your DAQ Device.  For more information on the input and output
'   terminals for your device, open the NI-DAQmx Help, and refer to the NI-DAQmx
'   Device Terminals and Device Considerations books in the table of contents.
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
    Private myTask As NationalInstruments.DAQmx.Task
    Private digitalReader As NationalInstruments.DAQmx.DigitalSingleChannelReader
    Private dCallback As AsyncCallback
    Private dataTable As DataTable
    Private dataColumn As DataColumn() = Nothing
    Private runningTask As Task

    Friend WithEvents channelParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents samplesPerChannelNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents clockSourceTextBox As System.Windows.Forms.TextBox
    Friend WithEvents clockSourceLabel As System.Windows.Forms.Label
    Friend WithEvents physicalChannelLabel As System.Windows.Forms.Label
    Friend WithEvents samplesPerChannelLabel As System.Windows.Forms.Label
    Friend WithEvents sampleRateNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents sampleRateLabel As System.Windows.Forms.Label
    Friend WithEvents startButton As System.Windows.Forms.Button
    Friend WithEvents resultsGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents resultsDataGrid As System.Windows.Forms.DataGrid
    Friend WithEvents stopButton As System.Windows.Forms.Button
    Friend WithEvents physicalChannelComboBox As System.Windows.Forms.ComboBox

    Private components As System.ComponentModel.Container = Nothing


    Public Sub New()
        MyBase.New()
        Application.EnableVisualStyles()
        Application.DoEvents()

        '
        ' Required for Windows Form Designer support
        '
        InitializeComponent()

        '
        ' TODO: Add any constructor code after InitializeComponent call
        '
        physicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DILine Or PhysicalChannelTypes.DIPort, PhysicalChannelAccess.External))
        If physicalChannelComboBox.Items.Count > 0 Then
            physicalChannelComboBox.SelectedIndex = 0
        End If

        dataTable = New DataTable
    End Sub 'New

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
    End Sub 'Dispose

    Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MainForm))
        Me.channelParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.physicalChannelComboBox = New System.Windows.Forms.ComboBox
        Me.samplesPerChannelNumeric = New System.Windows.Forms.NumericUpDown
        Me.clockSourceTextBox = New System.Windows.Forms.TextBox
        Me.clockSourceLabel = New System.Windows.Forms.Label
        Me.physicalChannelLabel = New System.Windows.Forms.Label
        Me.samplesPerChannelLabel = New System.Windows.Forms.Label
        Me.sampleRateNumeric = New System.Windows.Forms.NumericUpDown
        Me.sampleRateLabel = New System.Windows.Forms.Label
        Me.startButton = New System.Windows.Forms.Button
        Me.resultsGroupBox = New System.Windows.Forms.GroupBox
        Me.resultsDataGrid = New System.Windows.Forms.DataGrid
        Me.stopButton = New System.Windows.Forms.Button
        Me.channelParametersGroupBox.SuspendLayout()
        CType(Me.samplesPerChannelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.sampleRateNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.resultsGroupBox.SuspendLayout()
        CType(Me.resultsDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'channelParametersGroupBox
        '
        Me.channelParametersGroupBox.Controls.Add(Me.physicalChannelComboBox)
        Me.channelParametersGroupBox.Controls.Add(Me.samplesPerChannelNumeric)
        Me.channelParametersGroupBox.Controls.Add(Me.clockSourceTextBox)
        Me.channelParametersGroupBox.Controls.Add(Me.clockSourceLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.physicalChannelLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.samplesPerChannelLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.sampleRateNumeric)
        Me.channelParametersGroupBox.Controls.Add(Me.sampleRateLabel)
        Me.channelParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.channelParametersGroupBox.Location = New System.Drawing.Point(8, 8)
        Me.channelParametersGroupBox.Name = "channelParametersGroupBox"
        Me.channelParametersGroupBox.Size = New System.Drawing.Size(312, 176)
        Me.channelParametersGroupBox.TabIndex = 0
        Me.channelParametersGroupBox.TabStop = False
        Me.channelParametersGroupBox.Text = "Channel Parameters"
        '
        'physicalChannelComboBox
        '
        Me.physicalChannelComboBox.Location = New System.Drawing.Point(144, 33)
        Me.physicalChannelComboBox.Name = "physicalChannelComboBox"
        Me.physicalChannelComboBox.Size = New System.Drawing.Size(152, 21)
        Me.physicalChannelComboBox.TabIndex = 1
        Me.physicalChannelComboBox.Text = "Dev1/port0"
        '
        'samplesPerChannelNumeric
        '
        Me.samplesPerChannelNumeric.Location = New System.Drawing.Point(144, 105)
        Me.samplesPerChannelNumeric.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.samplesPerChannelNumeric.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.samplesPerChannelNumeric.Name = "samplesPerChannelNumeric"
        Me.samplesPerChannelNumeric.Size = New System.Drawing.Size(152, 20)
        Me.samplesPerChannelNumeric.TabIndex = 5
        Me.samplesPerChannelNumeric.Value = New Decimal(New Integer() {1000, 0, 0, 0})
        '
        'clockSourceTextBox
        '
        Me.clockSourceTextBox.Location = New System.Drawing.Point(144, 69)
        Me.clockSourceTextBox.Name = "clockSourceTextBox"
        Me.clockSourceTextBox.Size = New System.Drawing.Size(152, 20)
        Me.clockSourceTextBox.TabIndex = 3
        Me.clockSourceTextBox.Text = "/Dev1/PFI0"
        '
        'clockSourceLabel
        '
        Me.clockSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.clockSourceLabel.Location = New System.Drawing.Point(16, 68)
        Me.clockSourceLabel.Name = "clockSourceLabel"
        Me.clockSourceLabel.TabIndex = 2
        Me.clockSourceLabel.Text = "Clock Source:"
        '
        'physicalChannelLabel
        '
        Me.physicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.physicalChannelLabel.Location = New System.Drawing.Point(16, 32)
        Me.physicalChannelLabel.Name = "physicalChannelLabel"
        Me.physicalChannelLabel.TabIndex = 0
        Me.physicalChannelLabel.Text = "Physical Channel:"
        '
        'samplesPerChannelLabel
        '
        Me.samplesPerChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.samplesPerChannelLabel.Location = New System.Drawing.Point(16, 104)
        Me.samplesPerChannelLabel.Name = "samplesPerChannelLabel"
        Me.samplesPerChannelLabel.Size = New System.Drawing.Size(120, 23)
        Me.samplesPerChannelLabel.TabIndex = 4
        Me.samplesPerChannelLabel.Text = "Samples per Channel:"
        '
        'sampleRateNumeric
        '
        Me.sampleRateNumeric.DecimalPlaces = 2
        Me.sampleRateNumeric.Location = New System.Drawing.Point(144, 141)
        Me.sampleRateNumeric.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.sampleRateNumeric.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.sampleRateNumeric.Name = "sampleRateNumeric"
        Me.sampleRateNumeric.Size = New System.Drawing.Size(152, 20)
        Me.sampleRateNumeric.TabIndex = 7
        Me.sampleRateNumeric.Value = New Decimal(New Integer() {1000, 0, 0, 0})
        '
        'sampleRateLabel
        '
        Me.sampleRateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.sampleRateLabel.Location = New System.Drawing.Point(16, 140)
        Me.sampleRateLabel.Name = "sampleRateLabel"
        Me.sampleRateLabel.Size = New System.Drawing.Size(120, 23)
        Me.sampleRateLabel.TabIndex = 6
        Me.sampleRateLabel.Text = "Sample Rate (Hz):"
        '
        'startButton
        '
        Me.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.startButton.Location = New System.Drawing.Point(64, 264)
        Me.startButton.Name = "startButton"
        Me.startButton.TabIndex = 1
        Me.startButton.Text = "Start"
        '
        'resultsGroupBox
        '
        Me.resultsGroupBox.Controls.Add(Me.resultsDataGrid)
        Me.resultsGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.resultsGroupBox.Location = New System.Drawing.Point(328, 8)
        Me.resultsGroupBox.Name = "resultsGroupBox"
        Me.resultsGroupBox.Size = New System.Drawing.Size(304, 288)
        Me.resultsGroupBox.TabIndex = 3
        Me.resultsGroupBox.TabStop = False
        Me.resultsGroupBox.Text = "Results"
        '
        'resultsDataGrid
        '
        Me.resultsDataGrid.AllowSorting = False
        Me.resultsDataGrid.DataMember = ""
        Me.resultsDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.resultsDataGrid.Location = New System.Drawing.Point(3, 16)
        Me.resultsDataGrid.Name = "resultsDataGrid"
        Me.resultsDataGrid.PreferredColumnWidth = 125
        Me.resultsDataGrid.ReadOnly = True
        Me.resultsDataGrid.Size = New System.Drawing.Size(293, 264)
        Me.resultsDataGrid.TabIndex = 0
        '
        'stopButton
        '
        Me.stopButton.Enabled = False
        Me.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.stopButton.Location = New System.Drawing.Point(168, 264)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.TabIndex = 2
        Me.stopButton.Text = "Stop"
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(632, 302)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.resultsGroupBox)
        Me.Controls.Add(Me.stopButton)
        Me.Controls.Add(Me.channelParametersGroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Continuous Read Digital Channel - External Clock"
        Me.channelParametersGroupBox.ResumeLayout(False)
        CType(Me.samplesPerChannelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.sampleRateNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.resultsGroupBox.ResumeLayout(False)
        CType(Me.resultsDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub 'InitializeComponent

    Private Sub startButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles startButton.Click
        If runningTask Is Nothing Then
            Try
                ' Create the task
                myTask = New Task()

                ' Create the channel
                myTask.DIChannels.CreateChannel(physicalChannelComboBox.Text, "", ChannelLineGrouping.OneChannelForAllLines)

                ' Configure the external clock
                myTask.Timing.ConfigureSampleClock(clockSourceTextBox.Text, _
                Convert.ToDouble(sampleRateNumeric.Value), SampleClockActiveEdge.Rising, _
                SampleQuantityMode.ContinuousSamples, Convert.ToInt32(samplesPerChannelNumeric.Value))

                ' Set timeout to 10 s
                myTask.Stream.Timeout = 10000

                ' Verify the Task
                myTask.Control(TaskAction.Verify)

                ' Set up the data table
                InitializeDataTable(dataTable)
                resultsDataGrid.DataSource = dataTable

                ' Start running the task
                StartTask()

                ' Create the analog input sound reader
                digitalReader = New DigitalSingleChannelReader(myTask.Stream)

                ' Use SynchronizeCallbacks to specify that the object 
                ' marshals callbacks across threads appropriately.
                digitalReader.SynchronizeCallbacks = True

                ' Set up our first callback
                dCallback = New AsyncCallback(AddressOf DigitalCallback)
                digitalReader.BeginReadWaveform(Convert.ToInt32(samplesPerChannelNumeric.Value), dCallback, myTask)
            Catch exception As DaqException
                ' Display Errors
                MessageBox.Show(exception.Message)
                StopTask()
            End Try
        End If
    End Sub 'startButton_Click


    Private Sub DigitalCallback(ByVal ar As IAsyncResult)
        Try
            If (Not (runningTask Is Nothing)) AndAlso runningTask Is ar.AsyncState Then
                ' Read the available data from the channels
                Dim waveform As DigitalWaveform = digitalReader.EndReadWaveform(ar)

                ' Populate data table
                dataToDataTable(waveform, dataTable)

                ' Set up a new callback
                digitalReader.BeginReadWaveform(Convert.ToInt32(samplesPerChannelNumeric.Value), dCallback, myTask)
            End If
        Catch exception As DaqException
            ' Display Errors
            MessageBox.Show(exception.Message)
            StopTask()
        End Try
    End Sub 'DigitalCallback

    Private Sub dataToDataTable(ByVal waveform As DigitalWaveform, ByRef dataTable As DataTable)
        'Iterate over channels
        Dim currentLineIndex As Integer = 0
        Dim signal As DigitalWaveformSignal

        For Each signal In waveform.Signals
            Dim currentSampleIndex As Integer = 0
            Dim sample As DigitalState
            For Each sample In signal.States
                If (currentSampleIndex = 10) Then
                    Exit For
                End If
                If (sample = DigitalState.ForceUp) Then
                    dataTable.Rows(currentSampleIndex)(currentLineIndex) = 1
                Else
                    dataTable.Rows(currentSampleIndex)(currentLineIndex) = 0
                End If
                currentSampleIndex += 1
            Next
            currentLineIndex += 1
        Next
    End Sub

    Private Sub InitializeDataTable(ByRef data As DataTable)
        Dim numOfLines As Integer = Convert.ToInt32(myTask.DIChannels(0).NumberOfLines)
        Data.Rows.Clear()
        Data.Columns.Clear()
        dataColumn = New DataColumn(numOfLines) {}
        Dim numOfRows As Integer = 10
        Dim currentLineIndex As Integer = 0
        Dim currentDataIndex As Integer = 0

        For currentLineIndex = 0 To (numOfLines - 1)
            Dim channelNumber As Integer = currentLineIndex + 1
            dataColumn(currentLineIndex) = New DataColumn
            dataColumn(currentLineIndex).DataType = System.Type.GetType("System.Int32")
            dataColumn(currentLineIndex).ColumnName = "Channel " + channelNumber.ToString()
        Next

        Data.Columns.AddRange(dataColumn)

        For currentDataIndex = 0 To (numOfRows - 1)
            Dim rowArr As Object() = New Object(numOfLines - 1) {}
            Data.Rows.Add(rowArr)
        Next

    End Sub 'InitializeDataTable


    Private Sub stopButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles stopButton.Click
        If Not (runningTask Is Nothing) Then
            StopTask()
        End If
    End Sub 'stopButton_Click


    Private Sub StartTask()
        runningTask = myTask

        physicalChannelComboBox.Enabled = False
        clockSourceTextBox.Enabled = False
        samplesPerChannelNumeric.Enabled = False
        sampleRateNumeric.Enabled = False

        startButton.Enabled = False
        stopButton.Enabled = True

        Me.Refresh()
    End Sub 'StartTask


    Private Sub StopTask()
        runningTask = Nothing
        myTask.Dispose()

        physicalChannelComboBox.Enabled = True
        clockSourceTextBox.Enabled = True
        samplesPerChannelNumeric.Enabled = True
        sampleRateNumeric.Enabled = True

        startButton.Enabled = True
        stopButton.Enabled = False
    End Sub 'StopTask
End Class 'MainForm
