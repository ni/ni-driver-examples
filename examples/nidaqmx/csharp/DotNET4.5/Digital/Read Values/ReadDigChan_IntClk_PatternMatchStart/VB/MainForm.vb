'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   ReadDigChan_IntClk_PatternMatchStart
'
' Category:
'   DI
'
' Description:
'   This example demonstrates how to acquire a finite amount of digital data
'   (Waveform) using a pattern match start trigger (i.e. the acquisition begins
'   when a specified pattern has been matched).
'
' Instructions for running:
'   1.  Select the Physical Channel to correspond to where your signal is input
'       on the DAQ device.
'   2.  Select how many Samples to Acquire on Each Channel.
'   3.  Set the Rate of the Acquisition.
'   4.  Select the Pattern Match Channels, Pattern, and Trigger When parameters
'       of the Pattern Match Start Trigger for the acquisition.
'
' Steps:
'   1.  Create a new digital input task.
'   2.  Create a digital input channel. Use one channel of each line.
'   3.  Configure the sample clock, set the acquisition mode to finite.
'   4.  Use the StartTrigger object properties to configure a pattern trigger.
'   5.  Start the task to begin the acquisition.
'   6.  Call the DigitalMultiChannelReader.ReadWaveform method to read the data
'       and then display the acquired data.
'   7.  Dispose the Task object to clean-up any resources associated with the
'       task.
'   8.  Handle any DaqExceptions, if they occur.
'
' I/O Connections Overview:
'   Make sure your signal input terminal matches the Physical Channel I/O
'   Control and the Pattern Match Channels control.  For further connection
'   information, refer to your hardware reference manual.
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

    Private myTask As Task
    Private waveform As DigitalWaveform()
    Private reader As DigitalMultiChannelReader
    Private dataTable As DataTable
    Private dataColumn As dataColumn() = Nothing

    Public Sub New()
        MyBase.New()

        Application.EnableVisualStyles()
        Application.DoEvents()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        readButton.Enabled = False
        dataTable = New dataTable

        physicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DILine, PhysicalChannelAccess.External))
        patternMatchChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DILine, PhysicalChannelAccess.External))
        If (physicalChannelComboBox.Items.Count > 0) Then
            physicalChannelComboBox.SelectedIndex = 0
            patternMatchChannelComboBox.SelectedIndex = 0
            readButton.Enabled = True
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
    Friend WithEvents triggerParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents patternTextBox As System.Windows.Forms.TextBox
    Friend WithEvents patternLabel As System.Windows.Forms.Label
    Friend WithEvents triggerWhenComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents triggerWhenLabel As System.Windows.Forms.Label
    Friend WithEvents patternMatchChannelComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents patternMatchChannelLabel As System.Windows.Forms.Label
    Friend WithEvents readButton As System.Windows.Forms.Button
    Friend WithEvents resultsGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents resultsDataGrid As System.Windows.Forms.DataGrid
    Friend WithEvents channelParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents physicalChannelComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents physicalChannelLabel As System.Windows.Forms.Label
    Friend WithEvents timingParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents samplesPerChannelNumericUpDown As System.Windows.Forms.NumericUpDown
    Friend WithEvents samplesPerChannelLabel As System.Windows.Forms.Label
    Friend WithEvents samplesClockRateNumericUpDown As System.Windows.Forms.NumericUpDown
    Friend WithEvents sampleClockRateLabel As System.Windows.Forms.Label

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MainForm))
        Me.triggerParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.patternTextBox = New System.Windows.Forms.TextBox
        Me.patternLabel = New System.Windows.Forms.Label
        Me.triggerWhenComboBox = New System.Windows.Forms.ComboBox
        Me.triggerWhenLabel = New System.Windows.Forms.Label
        Me.patternMatchChannelComboBox = New System.Windows.Forms.ComboBox
        Me.patternMatchChannelLabel = New System.Windows.Forms.Label
        Me.readButton = New System.Windows.Forms.Button
        Me.resultsGroupBox = New System.Windows.Forms.GroupBox
        Me.resultsDataGrid = New System.Windows.Forms.DataGrid
        Me.channelParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.physicalChannelComboBox = New System.Windows.Forms.ComboBox
        Me.physicalChannelLabel = New System.Windows.Forms.Label
        Me.timingParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.samplesPerChannelNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.samplesPerChannelLabel = New System.Windows.Forms.Label
        Me.samplesClockRateNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.sampleClockRateLabel = New System.Windows.Forms.Label
        Me.triggerParametersGroupBox.SuspendLayout()
        Me.resultsGroupBox.SuspendLayout()
        CType(Me.resultsDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.channelParametersGroupBox.SuspendLayout()
        Me.timingParametersGroupBox.SuspendLayout()
        CType(Me.samplesPerChannelNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.samplesClockRateNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'triggerParametersGroupBox
        '
        Me.triggerParametersGroupBox.Controls.Add(Me.patternTextBox)
        Me.triggerParametersGroupBox.Controls.Add(Me.patternLabel)
        Me.triggerParametersGroupBox.Controls.Add(Me.triggerWhenComboBox)
        Me.triggerParametersGroupBox.Controls.Add(Me.triggerWhenLabel)
        Me.triggerParametersGroupBox.Controls.Add(Me.patternMatchChannelComboBox)
        Me.triggerParametersGroupBox.Controls.Add(Me.patternMatchChannelLabel)
        Me.triggerParametersGroupBox.Location = New System.Drawing.Point(8, 191)
        Me.triggerParametersGroupBox.Name = "triggerParametersGroupBox"
        Me.triggerParametersGroupBox.Size = New System.Drawing.Size(288, 136)
        Me.triggerParametersGroupBox.TabIndex = 2
        Me.triggerParametersGroupBox.TabStop = False
        Me.triggerParametersGroupBox.Text = "Trigger Parameters"
        '
        'patternTextBox
        '
        Me.patternTextBox.Location = New System.Drawing.Point(160, 64)
        Me.patternTextBox.Name = "patternTextBox"
        Me.patternTextBox.Size = New System.Drawing.Size(120, 20)
        Me.patternTextBox.TabIndex = 3
        Me.patternTextBox.Text = "00XX 11XX"
        '
        'patternLabel
        '
        Me.patternLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.patternLabel.Location = New System.Drawing.Point(8, 64)
        Me.patternLabel.Name = "patternLabel"
        Me.patternLabel.TabIndex = 2
        Me.patternLabel.Text = "Pattern:"
        '
        'triggerWhenComboBox
        '
        Me.triggerWhenComboBox.Items.AddRange(New Object() {"Pattern Matches", "Pattern Does Not Match"})
        Me.triggerWhenComboBox.Location = New System.Drawing.Point(160, 104)
        Me.triggerWhenComboBox.Name = "triggerWhenComboBox"
        Me.triggerWhenComboBox.Size = New System.Drawing.Size(121, 21)
        Me.triggerWhenComboBox.TabIndex = 5
        Me.triggerWhenComboBox.Text = "Pattern Matches"
        '
        'triggerWhenLabel
        '
        Me.triggerWhenLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.triggerWhenLabel.Location = New System.Drawing.Point(8, 104)
        Me.triggerWhenLabel.Name = "triggerWhenLabel"
        Me.triggerWhenLabel.TabIndex = 4
        Me.triggerWhenLabel.Text = "Trigger When:"
        '
        'patternMatchChannelComboBox
        '
        Me.patternMatchChannelComboBox.Location = New System.Drawing.Point(160, 18)
        Me.patternMatchChannelComboBox.Name = "patternMatchChannelComboBox"
        Me.patternMatchChannelComboBox.Size = New System.Drawing.Size(121, 21)
        Me.patternMatchChannelComboBox.TabIndex = 1
        Me.patternMatchChannelComboBox.Text = "Dev1/port0/line0:7"
        '
        'patternMatchChannelLabel
        '
        Me.patternMatchChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.patternMatchChannelLabel.Location = New System.Drawing.Point(8, 24)
        Me.patternMatchChannelLabel.Name = "patternMatchChannelLabel"
        Me.patternMatchChannelLabel.Size = New System.Drawing.Size(128, 23)
        Me.patternMatchChannelLabel.TabIndex = 0
        Me.patternMatchChannelLabel.Text = "Pattern Match Channel:"
        '
        'readButton
        '
        Me.readButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.readButton.Location = New System.Drawing.Point(104, 335)
        Me.readButton.Name = "readButton"
        Me.readButton.TabIndex = 3
        Me.readButton.Text = "Read"
        '
        'resultsGroupBox
        '
        Me.resultsGroupBox.Controls.Add(Me.resultsDataGrid)
        Me.resultsGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.resultsGroupBox.Location = New System.Drawing.Point(312, 7)
        Me.resultsGroupBox.Name = "resultsGroupBox"
        Me.resultsGroupBox.Size = New System.Drawing.Size(272, 352)
        Me.resultsGroupBox.TabIndex = 4
        Me.resultsGroupBox.TabStop = False
        Me.resultsGroupBox.Text = "Results"
        '
        'resultsDataGrid
        '
        Me.resultsDataGrid.DataMember = ""
        Me.resultsDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.resultsDataGrid.Location = New System.Drawing.Point(8, 16)
        Me.resultsDataGrid.Name = "resultsDataGrid"
        Me.resultsDataGrid.Size = New System.Drawing.Size(256, 328)
        Me.resultsDataGrid.TabIndex = 0
        Me.resultsDataGrid.TabStop = False
        '
        'channelParametersGroupBox
        '
        Me.channelParametersGroupBox.Controls.Add(Me.physicalChannelComboBox)
        Me.channelParametersGroupBox.Controls.Add(Me.physicalChannelLabel)
        Me.channelParametersGroupBox.Location = New System.Drawing.Point(8, 7)
        Me.channelParametersGroupBox.Name = "channelParametersGroupBox"
        Me.channelParametersGroupBox.Size = New System.Drawing.Size(288, 56)
        Me.channelParametersGroupBox.TabIndex = 0
        Me.channelParametersGroupBox.TabStop = False
        Me.channelParametersGroupBox.Text = "Channel Parameters"
        '
        'physicalChannelComboBox
        '
        Me.physicalChannelComboBox.Location = New System.Drawing.Point(160, 18)
        Me.physicalChannelComboBox.Name = "physicalChannelComboBox"
        Me.physicalChannelComboBox.Size = New System.Drawing.Size(121, 21)
        Me.physicalChannelComboBox.TabIndex = 1
        Me.physicalChannelComboBox.Text = "Dev1/port0/line0:7"
        '
        'physicalChannelLabel
        '
        Me.physicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.physicalChannelLabel.Location = New System.Drawing.Point(8, 24)
        Me.physicalChannelLabel.Name = "physicalChannelLabel"
        Me.physicalChannelLabel.TabIndex = 0
        Me.physicalChannelLabel.Text = "Physical Channel:"
        '
        'timingParametersGroupBox
        '
        Me.timingParametersGroupBox.Controls.Add(Me.samplesPerChannelNumericUpDown)
        Me.timingParametersGroupBox.Controls.Add(Me.samplesPerChannelLabel)
        Me.timingParametersGroupBox.Controls.Add(Me.samplesClockRateNumericUpDown)
        Me.timingParametersGroupBox.Controls.Add(Me.sampleClockRateLabel)
        Me.timingParametersGroupBox.Location = New System.Drawing.Point(8, 79)
        Me.timingParametersGroupBox.Name = "timingParametersGroupBox"
        Me.timingParametersGroupBox.Size = New System.Drawing.Size(288, 96)
        Me.timingParametersGroupBox.TabIndex = 1
        Me.timingParametersGroupBox.TabStop = False
        Me.timingParametersGroupBox.Text = "Timing Parameters"
        '
        'samplesPerChannelNumericUpDown
        '
        Me.samplesPerChannelNumericUpDown.Location = New System.Drawing.Point(160, 24)
        Me.samplesPerChannelNumericUpDown.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.samplesPerChannelNumericUpDown.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.samplesPerChannelNumericUpDown.Name = "samplesPerChannelNumericUpDown"
        Me.samplesPerChannelNumericUpDown.TabIndex = 1
        Me.samplesPerChannelNumericUpDown.Value = New Decimal(New Integer() {1000, 0, 0, 0})
        '
        'samplesPerChannelLabel
        '
        Me.samplesPerChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.samplesPerChannelLabel.Location = New System.Drawing.Point(8, 24)
        Me.samplesPerChannelLabel.Name = "samplesPerChannelLabel"
        Me.samplesPerChannelLabel.Size = New System.Drawing.Size(120, 23)
        Me.samplesPerChannelLabel.TabIndex = 0
        Me.samplesPerChannelLabel.Text = "Samples per Channel:"
        '
        'samplesClockRateNumericUpDown
        '
        Me.samplesClockRateNumericUpDown.Location = New System.Drawing.Point(160, 64)
        Me.samplesClockRateNumericUpDown.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.samplesClockRateNumericUpDown.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.samplesClockRateNumericUpDown.Name = "samplesClockRateNumericUpDown"
        Me.samplesClockRateNumericUpDown.TabIndex = 3
        Me.samplesClockRateNumericUpDown.Value = New Decimal(New Integer() {100000, 0, 0, 0})
        '
        'sampleClockRateLabel
        '
        Me.sampleClockRateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.sampleClockRateLabel.Location = New System.Drawing.Point(8, 64)
        Me.sampleClockRateLabel.Name = "sampleClockRateLabel"
        Me.sampleClockRateLabel.Size = New System.Drawing.Size(120, 23)
        Me.sampleClockRateLabel.TabIndex = 2
        Me.sampleClockRateLabel.Text = "Sample Clock Rate (Hz):"
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(592, 366)
        Me.Controls.Add(Me.triggerParametersGroupBox)
        Me.Controls.Add(Me.readButton)
        Me.Controls.Add(Me.resultsGroupBox)
        Me.Controls.Add(Me.channelParametersGroupBox)
        Me.Controls.Add(Me.timingParametersGroupBox)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "MainForm"
        Me.Text = "Read Dig Chan - Internal Clock - Pattern Match Start"
        Me.triggerParametersGroupBox.ResumeLayout(False)
        Me.resultsGroupBox.ResumeLayout(False)
        CType(Me.resultsDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.channelParametersGroupBox.ResumeLayout(False)
        Me.timingParametersGroupBox.ResumeLayout(False)
        CType(Me.samplesPerChannelNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.samplesClockRateNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub readButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles readButton.Click
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
        readButton.Enabled = True
        Try
            Dim condition As DigitalPatternStartTriggerCondition
            If (triggerWhenComboBox.SelectedIndex = 0) Then
                condition = DigitalPatternStartTriggerCondition.PatternMatches
            Else
                condition = DigitalPatternStartTriggerCondition.PatternDoesNotMatch
            End If

            ' Create and configure the Task
            myTask = New Task
            myTask.DIChannels.CreateChannel(physicalChannelComboBox.Text, "", ChannelLineGrouping.OneChannelForEachLine)
            myTask.Timing.ConfigureSampleClock("", CType(samplesClockRateNumericUpDown.Value, Double), _
                                SampleClockActiveEdge.Rising, SampleQuantityMode.FiniteSamples, CType(samplesPerChannelNumericUpDown.Value, Integer))
            myTask.Triggers.StartTrigger.ConfigureDigitalPatternTrigger(patternMatchChannelComboBox.Text, patternTextBox.Text, condition)

            myTask.Start()

            InitializeDataTable(dataTable)
            resultsDataGrid.DataSource = dataTable

            reader = New DigitalMultiChannelReader(myTask.Stream)

            waveform = reader.ReadWaveform(CType(samplesPerChannelNumericUpDown.Value, Integer))

            DataToDataTable(waveform, dataTable)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            myTask.Dispose()
            readButton.Enabled = True
        End Try
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
    End Sub

    Private Sub DataToDataTable(ByVal waveform As DigitalWaveform(), ByRef dataTable As dataTable)
        'Iterate over channels
        Dim currentLineIndex As Integer = 0
        Dim signal As DigitalWaveform

        For Each signal In waveform
            Dim sample As Integer
            For sample = 0 To (signal.Signals(0).States.Count - 1)
                If (sample = 10) Then
                    Exit For
                End If
                If (signal.Signals(0).States(sample) = DigitalState.ForceUp) Then
                    dataTable.Rows(sample)(currentLineIndex) = 1
                Else
                    dataTable.Rows(sample)(currentLineIndex) = 0
                End If
            Next
            currentLineIndex += 1
        Next
    End Sub

    Private Sub InitializeDataTable(ByRef data As dataTable)
        Dim numOfLines As Integer = Convert.ToInt32(myTask.DIChannels.Count)
        data.Rows.Clear()
        data.Columns.Clear()
        dataColumn = New DataColumn(numOfLines) {}
        Dim numOfRows As Integer = 10
        Dim currentLineIndex As Integer = 0
        Dim currentDataIndex As Integer = 0

        For currentLineIndex = 0 To (numOfLines - 1)
            dataColumn(currentLineIndex) = New DataColumn
            dataColumn(currentLineIndex).DataType = System.Type.GetType("System.Int32")
            dataColumn(currentLineIndex).ColumnName = myTask.DIChannels(currentLineIndex).PhysicalName
        Next

        data.Columns.AddRange(dataColumn)

        For currentDataIndex = 0 To (numOfRows - 1)
            Dim rowArr As Object() = New Object(numOfLines - 1) {}
            data.Rows.Add(rowArr)
        Next

    End Sub 'InitializeDataTable
End Class
