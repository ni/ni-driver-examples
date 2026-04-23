'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   ReadDigChan_ChangeDetection_DigFilter
'
' Category:
'   DIO
'
' Description:
'   This example demonstrates how to acquire filtered digital input via change
'   detection and digital filtering.
'
' Instructions for running:
'   1.  Select the physical lines for unfiltered digital input.
'   2.  Select the physical lines for filtered digital input.Note: The
'       unfiltered and filtered lines should not be the same.
'   3.  Select the physical lines for rising-edge change detection.
'   4.  Select the physical lines for falling-edge change detection on.Note: You
'       can enable one, both, or neither kinds of detection on each line.
'   5.  Set the minimum pulse width for the filter.
'   6.  Set the number of samples to acquire per channel per read.
'   7.  Set the read timeout, in seconds.
'
' Steps:
'   1.  Create a new digital input task.
'   2.  Create two digital input channels, one for filtered digital input and
'       one for unfiltered.
'   3.  Configure the channels for digital filtering enabled or disabled.
'   4.  Set up change detection on specific lines for digital input.
'   5.  Create a DigitalMultiChannelReader and associate it with the task by
'       using the task's stream. Call
'       DigitalMultiChannelReader.BeginReadSingleSampleMultiLine to install a
'       callback and begin the asynchronous read operation.
'   6.  Inside the callback, call
'       DigitalMultiChannelReader.EndReadSingleSampleMultiLine to retrieve the
'       data from the read.
'   7.  Call DigitalMultiChannelReader.BeginReadSingleSampleMultiLine again
'       inside the callback to perform another read operation.
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
'   Make sure your signal input terminals match the filtered and unfiltered
'   lines.  In this case wire your digital signals to the first eight digital
'   lines on your DAQ Device.  For more information on the input and output
'   terminals for your device, open the NI-DAQmx Help, and refer to the NI-DAQmx
'   Device Terminals and Device Considerations books in the table of
'   contents.NOTE: For NI-6534 devices, either 32 bytes of data needs to be
'   transferred first for the DMA transfer to take place, or interrupts must be
'   used instead of DMA.
'
' Recommended Use:
'   Create a Task object. Create the appropriate DIChannel object and configure
'   its parameters.  Configure the Timing parameters by using the Timing object.
'   Read the data by using the AnalogMultiChannelReader object. Use the
'   appropriate BeginRead method to read the data asynchronously. Dispose the
'   Task object to clean-up any resources associated with the task.
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
    Private digitalReader As DigitalMultiChannelReader
    Private dCallback As AsyncCallback
    Private dTable As DataTable
    Private runningTask As Task
    Friend WithEvents resultsDataGrid As System.Windows.Forms.DataGrid

    Friend WithEvents resultsGroup As System.Windows.Forms.GroupBox
    Friend WithEvents pulseWidthLabel As System.Windows.Forms.Label
    Friend WithEvents pulseWidthNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents startButton As System.Windows.Forms.Button
    Friend WithEvents stopButton As System.Windows.Forms.Button
    Friend WithEvents risingEdgeLabel As System.Windows.Forms.Label
    Friend WithEvents fallingEdgeLabel As System.Windows.Forms.Label
    Friend WithEvents lineGroup As System.Windows.Forms.GroupBox
    Friend WithEvents timeoutNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents timeoutLabel As System.Windows.Forms.Label
    Friend WithEvents timingGroup As System.Windows.Forms.GroupBox
    Friend WithEvents unfilteredLabel As System.Windows.Forms.Label
    Friend WithEvents filteredLabel As System.Windows.Forms.Label
    Friend WithEvents unfilteredLinesComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents filteredLinesComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents risingEdgeComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents fallingEdgeComboBox As System.Windows.Forms.ComboBox

    Private components As System.ComponentModel.Container = Nothing


    Public Sub New()
        MyBase.New()
        Application.EnableVisualStyles()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Initialize UI
        stopButton.Enabled = False
        dCallback = New AsyncCallback(AddressOf DigitalCallback)
        dTable = New DataTable

        unfilteredLinesComboBox.Items.Add("None")
        filteredLinesComboBox.Items.Add("None")
        risingEdgeComboBox.Items.Add("None")
        fallingEdgeComboBox.Items.Add("None")

        unfilteredLinesComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DILine Or PhysicalChannelTypes.DIPort, PhysicalChannelAccess.External))
        filteredLinesComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DILine Or PhysicalChannelTypes.DIPort, PhysicalChannelAccess.External))
        risingEdgeComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DILine Or PhysicalChannelTypes.DIPort, PhysicalChannelAccess.External))
        fallingEdgeComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DILine Or PhysicalChannelTypes.DIPort, PhysicalChannelAccess.External))

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
    Friend WithEvents samplesNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents samplesLabel As System.Windows.Forms.Label

    Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MainForm))
        Me.timingGroup = New System.Windows.Forms.GroupBox
        Me.samplesNumeric = New System.Windows.Forms.NumericUpDown
        Me.samplesLabel = New System.Windows.Forms.Label
        Me.timeoutNumeric = New System.Windows.Forms.NumericUpDown
        Me.timeoutLabel = New System.Windows.Forms.Label
        Me.pulseWidthNumeric = New System.Windows.Forms.NumericUpDown
        Me.pulseWidthLabel = New System.Windows.Forms.Label
        Me.lineGroup = New System.Windows.Forms.GroupBox
        Me.fallingEdgeComboBox = New System.Windows.Forms.ComboBox
        Me.risingEdgeComboBox = New System.Windows.Forms.ComboBox
        Me.filteredLinesComboBox = New System.Windows.Forms.ComboBox
        Me.unfilteredLinesComboBox = New System.Windows.Forms.ComboBox
        Me.filteredLabel = New System.Windows.Forms.Label
        Me.risingEdgeLabel = New System.Windows.Forms.Label
        Me.fallingEdgeLabel = New System.Windows.Forms.Label
        Me.unfilteredLabel = New System.Windows.Forms.Label
        Me.resultsGroup = New System.Windows.Forms.GroupBox
        Me.resultsDataGrid = New System.Windows.Forms.DataGrid
        Me.startButton = New System.Windows.Forms.Button
        Me.stopButton = New System.Windows.Forms.Button
        Me.timingGroup.SuspendLayout()
        CType(Me.samplesNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.timeoutNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pulseWidthNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.lineGroup.SuspendLayout()
        Me.resultsGroup.SuspendLayout()
        CType(Me.resultsDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'timingGroup
        '
        Me.timingGroup.Controls.Add(Me.samplesNumeric)
        Me.timingGroup.Controls.Add(Me.samplesLabel)
        Me.timingGroup.Controls.Add(Me.timeoutNumeric)
        Me.timingGroup.Controls.Add(Me.timeoutLabel)
        Me.timingGroup.Controls.Add(Me.pulseWidthNumeric)
        Me.timingGroup.Controls.Add(Me.pulseWidthLabel)
        Me.timingGroup.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.timingGroup.Location = New System.Drawing.Point(8, 168)
        Me.timingGroup.Name = "timingGroup"
        Me.timingGroup.Size = New System.Drawing.Size(240, 120)
        Me.timingGroup.TabIndex = 3
        Me.timingGroup.TabStop = False
        Me.timingGroup.Text = "Timing Parameters"
        '
        'samplesNumeric
        '
        Me.samplesNumeric.Location = New System.Drawing.Point(112, 56)
        Me.samplesNumeric.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.samplesNumeric.Name = "samplesNumeric"
        Me.samplesNumeric.TabIndex = 3
        Me.samplesNumeric.Value = New Decimal(New Integer() {4, 0, 0, 0})
        '
        'samplesLabel
        '
        Me.samplesLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.samplesLabel.Location = New System.Drawing.Point(8, 56)
        Me.samplesLabel.Name = "samplesLabel"
        Me.samplesLabel.Size = New System.Drawing.Size(112, 23)
        Me.samplesLabel.TabIndex = 2
        Me.samplesLabel.Text = "Number of Samples:"
        '
        'timeoutNumeric
        '
        Me.timeoutNumeric.Location = New System.Drawing.Point(112, 88)
        Me.timeoutNumeric.Name = "timeoutNumeric"
        Me.timeoutNumeric.TabIndex = 5
        Me.timeoutNumeric.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'timeoutLabel
        '
        Me.timeoutLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.timeoutLabel.Location = New System.Drawing.Point(8, 88)
        Me.timeoutLabel.Name = "timeoutLabel"
        Me.timeoutLabel.Size = New System.Drawing.Size(112, 23)
        Me.timeoutLabel.TabIndex = 4
        Me.timeoutLabel.Text = "Timeout (s):"
        '
        'pulseWidthNumeric
        '
        Me.pulseWidthNumeric.DecimalPlaces = 7
        Me.pulseWidthNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 262144})
        Me.pulseWidthNumeric.Location = New System.Drawing.Point(112, 24)
        Me.pulseWidthNumeric.Name = "pulseWidthNumeric"
        Me.pulseWidthNumeric.TabIndex = 1
        Me.pulseWidthNumeric.Value = New Decimal(New Integer() {1, 0, 0, 262144})
        '
        'pulseWidthLabel
        '
        Me.pulseWidthLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.pulseWidthLabel.Location = New System.Drawing.Point(8, 24)
        Me.pulseWidthLabel.Name = "pulseWidthLabel"
        Me.pulseWidthLabel.Size = New System.Drawing.Size(112, 23)
        Me.pulseWidthLabel.TabIndex = 0
        Me.pulseWidthLabel.Text = "Min. Pulse Width (s):"
        '
        'lineGroup
        '
        Me.lineGroup.Controls.Add(Me.fallingEdgeComboBox)
        Me.lineGroup.Controls.Add(Me.risingEdgeComboBox)
        Me.lineGroup.Controls.Add(Me.filteredLinesComboBox)
        Me.lineGroup.Controls.Add(Me.unfilteredLinesComboBox)
        Me.lineGroup.Controls.Add(Me.filteredLabel)
        Me.lineGroup.Controls.Add(Me.risingEdgeLabel)
        Me.lineGroup.Controls.Add(Me.fallingEdgeLabel)
        Me.lineGroup.Controls.Add(Me.unfilteredLabel)
        Me.lineGroup.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.lineGroup.Location = New System.Drawing.Point(8, 8)
        Me.lineGroup.Name = "lineGroup"
        Me.lineGroup.Size = New System.Drawing.Size(240, 152)
        Me.lineGroup.TabIndex = 2
        Me.lineGroup.TabStop = False
        Me.lineGroup.Text = "Line Parameters"
        '
        'fallingEdgeComboBox
        '
        Me.fallingEdgeComboBox.Location = New System.Drawing.Point(112, 120)
        Me.fallingEdgeComboBox.Name = "fallingEdgeComboBox"
        Me.fallingEdgeComboBox.Size = New System.Drawing.Size(121, 21)
        Me.fallingEdgeComboBox.TabIndex = 7
        Me.fallingEdgeComboBox.Text = "Dev1/port0/line4:7"
        '
        'risingEdgeComboBox
        '
        Me.risingEdgeComboBox.Location = New System.Drawing.Point(112, 88)
        Me.risingEdgeComboBox.Name = "risingEdgeComboBox"
        Me.risingEdgeComboBox.Size = New System.Drawing.Size(121, 21)
        Me.risingEdgeComboBox.TabIndex = 5
        Me.risingEdgeComboBox.Text = "Dev1/port0/line0:4"
        '
        'filteredLinesComboBox
        '
        Me.filteredLinesComboBox.Location = New System.Drawing.Point(112, 56)
        Me.filteredLinesComboBox.Name = "filteredLinesComboBox"
        Me.filteredLinesComboBox.Size = New System.Drawing.Size(121, 21)
        Me.filteredLinesComboBox.TabIndex = 3
        Me.filteredLinesComboBox.Text = "Dev1/port0/line4:7"
        '
        'unfilteredLinesComboBox
        '
        Me.unfilteredLinesComboBox.Location = New System.Drawing.Point(112, 24)
        Me.unfilteredLinesComboBox.Name = "unfilteredLinesComboBox"
        Me.unfilteredLinesComboBox.Size = New System.Drawing.Size(121, 21)
        Me.unfilteredLinesComboBox.TabIndex = 1
        Me.unfilteredLinesComboBox.Text = "Dev1/port0/line0:3"
        '
        'filteredLabel
        '
        Me.filteredLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.filteredLabel.Location = New System.Drawing.Point(8, 55)
        Me.filteredLabel.Name = "filteredLabel"
        Me.filteredLabel.Size = New System.Drawing.Size(88, 23)
        Me.filteredLabel.TabIndex = 2
        Me.filteredLabel.Text = "Filtered Lines:"
        '
        'risingEdgeLabel
        '
        Me.risingEdgeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.risingEdgeLabel.Location = New System.Drawing.Point(8, 87)
        Me.risingEdgeLabel.Name = "risingEdgeLabel"
        Me.risingEdgeLabel.Size = New System.Drawing.Size(112, 23)
        Me.risingEdgeLabel.TabIndex = 4
        Me.risingEdgeLabel.Text = "Detect Rising Edges:"
        '
        'fallingEdgeLabel
        '
        Me.fallingEdgeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.fallingEdgeLabel.Location = New System.Drawing.Point(8, 119)
        Me.fallingEdgeLabel.Name = "fallingEdgeLabel"
        Me.fallingEdgeLabel.Size = New System.Drawing.Size(112, 23)
        Me.fallingEdgeLabel.TabIndex = 6
        Me.fallingEdgeLabel.Text = "Detect Falling Edges:"
        '
        'unfilteredLabel
        '
        Me.unfilteredLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.unfilteredLabel.Location = New System.Drawing.Point(8, 23)
        Me.unfilteredLabel.Name = "unfilteredLabel"
        Me.unfilteredLabel.Size = New System.Drawing.Size(104, 23)
        Me.unfilteredLabel.TabIndex = 0
        Me.unfilteredLabel.Text = "Unfiltered Lines:"
        '
        'resultsGroup
        '
        Me.resultsGroup.Controls.Add(Me.resultsDataGrid)
        Me.resultsGroup.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.resultsGroup.Location = New System.Drawing.Point(256, 8)
        Me.resultsGroup.Name = "resultsGroup"
        Me.resultsGroup.Size = New System.Drawing.Size(304, 320)
        Me.resultsGroup.TabIndex = 4
        Me.resultsGroup.TabStop = False
        Me.resultsGroup.Text = "Results"
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
        Me.resultsDataGrid.Size = New System.Drawing.Size(293, 296)
        Me.resultsDataGrid.TabIndex = 0
        '
        'startButton
        '
        Me.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.startButton.Location = New System.Drawing.Point(40, 304)
        Me.startButton.Name = "startButton"
        Me.startButton.TabIndex = 0
        Me.startButton.Text = "Start"
        '
        'stopButton
        '
        Me.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.stopButton.Location = New System.Drawing.Point(144, 304)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.TabIndex = 1
        Me.stopButton.Text = "Stop"
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(568, 334)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.resultsGroup)
        Me.Controls.Add(Me.lineGroup)
        Me.Controls.Add(Me.timingGroup)
        Me.Controls.Add(Me.stopButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Digital Channel Read - Change Detection and Digital Filter"
        Me.timingGroup.ResumeLayout(False)
        CType(Me.samplesNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.timeoutNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pulseWidthNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.lineGroup.ResumeLayout(False)
        Me.resultsGroup.ResumeLayout(False)
        CType(Me.resultsDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub 'InitializeComponent

    Private Sub startButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles startButton.Click
        If runningTask Is Nothing Then
            Try
                ' Create the task
                myTask = New Task()

                ' Create the channels
                Dim unfilteredChannel As DIChannel
                Dim filteredChannel As DIChannel

                ' Disable filtering for these lines, if any
                If unfilteredLinesComboBox.Text <> "None" Then
                    unfilteredChannel = myTask.DIChannels.CreateChannel(unfilteredLinesComboBox.Text, "", ChannelLineGrouping.OneChannelForAllLines)
                    unfilteredChannel.DigitalFilterEnable = False
                End If

                ' Enable filtering for these lines, if any
                If filteredLinesComboBox.Text <> "None" Then
                    filteredChannel = myTask.DIChannels.CreateChannel(filteredLinesComboBox.Text, "", ChannelLineGrouping.OneChannelForAllLines)
                    filteredChannel.DigitalFilterEnable = True
                    filteredChannel.DigitalFilterMinimumPulseWidth = CDbl(pulseWidthNumeric.Value)
                End If

                ' Change "None" to "" from combo box rising/falling edges selections
                Dim rising As String
                Dim falling As String

                If risingEdgeComboBox.Text = "None" Then
                    rising = ""
                Else
                    rising = risingEdgeComboBox.Text
                End If

                If fallingEdgeComboBox.Text = "None" Then
                    falling = ""
                Else
                    falling = fallingEdgeComboBox.Text
                End If

                ' Configure the timing parameters for change detection
                myTask.Timing.ConfigureChangeDetection(rising, falling, SampleQuantityMode.ContinuousSamples, CInt(samplesNumeric.Value))

                ' Set timeout
                myTask.Stream.Timeout = CInt(timeoutNumeric.Value) * 1000

                ' Verify the Task
                myTask.Control(TaskAction.Verify)

                ' Set up the data table
                InitializeDataTable()
                resultsDataGrid.DataSource = dTable

                ' Start running the task
                StartTask()

                ' Create the analog input sound reader
                digitalReader = New DigitalMultiChannelReader(myTask.Stream)

                ' Use SynchronizeCallbacks to specify that the object 
                ' marshals callbacks across threads appropriately.
                digitalReader.SynchronizeCallbacks = True

                ' Set up our first callback
                digitalReader.BeginReadSingleSampleMultiLine(dCallback, myTask)
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
                Dim data As Boolean(,) = digitalReader.EndReadSingleSampleMultiLine(ar)

                ' Clear data table
                dTable.Rows.Clear()

                Dim i As Integer = 0
                Dim b As Boolean
                For chan As Integer = 0 To myTask.DIChannels.Count - 1
                    For line As Integer = 0 To myTask.DIChannels(chan).NumberOfLines - 1
                        b = data(chan, line)
                        Dim myRow As DataRow = dTable.NewRow()
                        myRow.Item("Line") = "Line " + i.ToString()
                        myRow.Item("Value") = b.ToString()
                        dTable.Rows.Add(myRow)
                        i += 1
                    Next
                Next

                ' Set up a new callback
                digitalReader.BeginReadSingleSampleMultiLine(dCallback, myTask)
            End If
        Catch exception As DaqException
            ' Display Errors
            MessageBox.Show(exception.Message)
            StopTask()
        End Try
    End Sub 'DigitalCallback

    Public Sub InitializeDataTable()
        ' Clear data
        dTable.Rows.Clear()
        dTable.Columns.Clear()

        ' Create new DataColumn, set DataType, ColumnName and add to DataTable.    
        Dim myColumn As DataColumn = New DataColumn
        myColumn.DataType = System.Type.GetType("System.String")
        myColumn.ColumnName = "Line"
        dTable.Columns.Add(myColumn)

        ' Create second column.
        myColumn = New DataColumn
        myColumn.DataType = Type.GetType("System.String")
        myColumn.ColumnName = "Value"
        dTable.Columns.Add(myColumn)
    End Sub

    Private Sub stopButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles stopButton.Click
        If Not (runningTask Is Nothing) Then
            StopTask()
        End If
    End Sub 'stopButton_Click


    Private Sub StartTask()
        runningTask = myTask

        filteredLinesComboBox.Enabled = False
        unfilteredLinesComboBox.Enabled = False
        risingEdgeComboBox.Enabled = False
        fallingEdgeComboBox.Enabled = False
        pulseWidthNumeric.Enabled = False
        samplesNumeric.Enabled = False
        timeoutNumeric.Enabled = False
        startButton.Enabled = False
        stopButton.Enabled = True

        Me.Refresh()
    End Sub 'StartTask


    Private Sub StopTask()
        runningTask = Nothing
        myTask.Dispose()

        filteredLinesComboBox.Enabled = True
        unfilteredLinesComboBox.Enabled = True
        risingEdgeComboBox.Enabled = True
        fallingEdgeComboBox.Enabled = True
        pulseWidthNumeric.Enabled = True
        samplesNumeric.Enabled = True
        timeoutNumeric.Enabled = True
        startButton.Enabled = True
        stopButton.Enabled = False
    End Sub 'StopTask
End Class 'MainForm
