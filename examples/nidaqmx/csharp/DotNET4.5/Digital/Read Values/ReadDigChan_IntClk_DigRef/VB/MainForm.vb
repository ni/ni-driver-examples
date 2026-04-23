'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   ReadDigChan_IntClk_DigRef
'
' Category:
'   DI
'
' Description:
'   This example demonstrates how to acquire a finite amount of data (Waveform)
'   using a digital reference trigger.
'
' Instructions for running:
'   1.  Select the Physical Channel to correspond to where your signal is input
'       on the DAQ device.
'   2.  Select how many Samples to Acquire on Each Channel.
'   3.  Set the Rate of the Acquisition.
'   4.  Select the Source and Edge of the Digital Reference Trigger for the
'       acquisition.
'   5.  Set the number of Pre-Trigger samples.
'
' Steps:
'   1.  Create a new digital input task.
'   2.  Create a digital input channel. Use one channel of each line.
'   3.  Configure the sample clock, set the acquisition mode to finite.
'   4.  Use the ReferenceTrigger object properties to configure a digital edge
'       trigger.
'   5.  Start the task to begin the acquisition.
'   6.  Call the DigitalMultiChannelReader.ReadWaveform method to read the data
'       and then display the acquired data.
'   7.  Dispose the Task object to clean-up any resources associated with the
'       task.
'   8.  Handle any DaqExceptions, if they occur.
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

        dataTable = New DataTable

        physicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DILine, PhysicalChannelAccess.External))
        triggerSourceComboBox.Items.AddRange(DaqSystem.Local.GetTerminals(TerminalTypes.All))

        If (physicalChannelComboBox.Items.Count > 0) Then
            triggerSourceComboBox.SelectedIndex = 0
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
    Friend WithEvents channelParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents physicalChannelComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents physicalChannelLabel As System.Windows.Forms.Label
    Friend WithEvents readButton As System.Windows.Forms.Button
    Friend WithEvents resultsGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents resultsDataGrid As System.Windows.Forms.DataGrid
    Friend WithEvents triggerParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents edgeComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents preTriggerSamplesNumericUpDown As System.Windows.Forms.NumericUpDown
    Friend WithEvents edgeLabel As System.Windows.Forms.Label
    Friend WithEvents preTriggerSamplesLabel As System.Windows.Forms.Label
    Friend WithEvents triggerSourceComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents triggerSourceLabel As System.Windows.Forms.Label
    Friend WithEvents timingParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents samplesPerChannelNumericUpDown As System.Windows.Forms.NumericUpDown
    Friend WithEvents samplesPerChannelLabel As System.Windows.Forms.Label
    Friend WithEvents samplesClockRateNumericUpDown As System.Windows.Forms.NumericUpDown
    Friend WithEvents sampleClockRateLabel As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MainForm))
        Me.channelParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.physicalChannelComboBox = New System.Windows.Forms.ComboBox
        Me.physicalChannelLabel = New System.Windows.Forms.Label
        Me.readButton = New System.Windows.Forms.Button
        Me.resultsGroupBox = New System.Windows.Forms.GroupBox
        Me.resultsDataGrid = New System.Windows.Forms.DataGrid
        Me.triggerParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.edgeComboBox = New System.Windows.Forms.ComboBox
        Me.preTriggerSamplesNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.edgeLabel = New System.Windows.Forms.Label
        Me.preTriggerSamplesLabel = New System.Windows.Forms.Label
        Me.triggerSourceComboBox = New System.Windows.Forms.ComboBox
        Me.triggerSourceLabel = New System.Windows.Forms.Label
        Me.timingParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.samplesPerChannelNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.samplesPerChannelLabel = New System.Windows.Forms.Label
        Me.samplesClockRateNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.sampleClockRateLabel = New System.Windows.Forms.Label
        Me.channelParametersGroupBox.SuspendLayout()
        Me.resultsGroupBox.SuspendLayout()
        CType(Me.resultsDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.triggerParametersGroupBox.SuspendLayout()
        CType(Me.preTriggerSamplesNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.timingParametersGroupBox.SuspendLayout()
        CType(Me.samplesPerChannelNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.samplesClockRateNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'channelParametersGroupBox
        '
        Me.channelParametersGroupBox.Controls.Add(Me.physicalChannelComboBox)
        Me.channelParametersGroupBox.Controls.Add(Me.physicalChannelLabel)
        Me.channelParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.channelParametersGroupBox.Location = New System.Drawing.Point(8, 7)
        Me.channelParametersGroupBox.Name = "channelParametersGroupBox"
        Me.channelParametersGroupBox.Size = New System.Drawing.Size(288, 56)
        Me.channelParametersGroupBox.TabIndex = 10
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
        'readButton
        '
        Me.readButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.readButton.Location = New System.Drawing.Point(120, 336)
        Me.readButton.Name = "readButton"
        Me.readButton.TabIndex = 14
        Me.readButton.Text = "&Read"
        '
        'resultsGroupBox
        '
        Me.resultsGroupBox.Controls.Add(Me.resultsDataGrid)
        Me.resultsGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.resultsGroupBox.Location = New System.Drawing.Point(312, 7)
        Me.resultsGroupBox.Name = "resultsGroupBox"
        Me.resultsGroupBox.Size = New System.Drawing.Size(272, 352)
        Me.resultsGroupBox.TabIndex = 13
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
        'triggerParametersGroupBox
        '
        Me.triggerParametersGroupBox.Controls.Add(Me.edgeComboBox)
        Me.triggerParametersGroupBox.Controls.Add(Me.preTriggerSamplesNumericUpDown)
        Me.triggerParametersGroupBox.Controls.Add(Me.edgeLabel)
        Me.triggerParametersGroupBox.Controls.Add(Me.preTriggerSamplesLabel)
        Me.triggerParametersGroupBox.Controls.Add(Me.triggerSourceComboBox)
        Me.triggerParametersGroupBox.Controls.Add(Me.triggerSourceLabel)
        Me.triggerParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.triggerParametersGroupBox.Location = New System.Drawing.Point(8, 191)
        Me.triggerParametersGroupBox.Name = "triggerParametersGroupBox"
        Me.triggerParametersGroupBox.Size = New System.Drawing.Size(288, 136)
        Me.triggerParametersGroupBox.TabIndex = 12
        Me.triggerParametersGroupBox.TabStop = False
        Me.triggerParametersGroupBox.Text = "Trigger Parameters"
        '
        'edgeComboBox
        '
        Me.edgeComboBox.Items.AddRange(New Object() {"Rising", "Falling"})
        Me.edgeComboBox.Location = New System.Drawing.Point(160, 56)
        Me.edgeComboBox.Name = "edgeComboBox"
        Me.edgeComboBox.Size = New System.Drawing.Size(121, 21)
        Me.edgeComboBox.TabIndex = 5
        Me.edgeComboBox.Text = "Rising"
        '
        'preTriggerSamplesNumericUpDown
        '
        Me.preTriggerSamplesNumericUpDown.Location = New System.Drawing.Point(160, 96)
        Me.preTriggerSamplesNumericUpDown.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.preTriggerSamplesNumericUpDown.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.preTriggerSamplesNumericUpDown.Name = "preTriggerSamplesNumericUpDown"
        Me.preTriggerSamplesNumericUpDown.TabIndex = 6
        Me.preTriggerSamplesNumericUpDown.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        'edgeLabel
        '
        Me.edgeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.edgeLabel.Location = New System.Drawing.Point(8, 56)
        Me.edgeLabel.Name = "edgeLabel"
        Me.edgeLabel.TabIndex = 2
        Me.edgeLabel.Text = "Edge:"
        '
        'preTriggerSamplesLabel
        '
        Me.preTriggerSamplesLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.preTriggerSamplesLabel.Location = New System.Drawing.Point(8, 96)
        Me.preTriggerSamplesLabel.Name = "preTriggerSamplesLabel"
        Me.preTriggerSamplesLabel.TabIndex = 4
        Me.preTriggerSamplesLabel.Text = "Pre-Trigger Samples:"
        '
        'triggerSourceComboBox
        '
        Me.triggerSourceComboBox.Location = New System.Drawing.Point(160, 18)
        Me.triggerSourceComboBox.Name = "triggerSourceComboBox"
        Me.triggerSourceComboBox.Size = New System.Drawing.Size(121, 21)
        Me.triggerSourceComboBox.TabIndex = 4
        Me.triggerSourceComboBox.Text = "/Dev1/PFI0"
        '
        'triggerSourceLabel
        '
        Me.triggerSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.triggerSourceLabel.Location = New System.Drawing.Point(8, 24)
        Me.triggerSourceLabel.Name = "triggerSourceLabel"
        Me.triggerSourceLabel.Size = New System.Drawing.Size(128, 23)
        Me.triggerSourceLabel.TabIndex = 0
        Me.triggerSourceLabel.Text = "Trigger Source:"
        '
        'timingParametersGroupBox
        '
        Me.timingParametersGroupBox.Controls.Add(Me.samplesPerChannelNumericUpDown)
        Me.timingParametersGroupBox.Controls.Add(Me.samplesPerChannelLabel)
        Me.timingParametersGroupBox.Controls.Add(Me.samplesClockRateNumericUpDown)
        Me.timingParametersGroupBox.Controls.Add(Me.sampleClockRateLabel)
        Me.timingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.timingParametersGroupBox.Location = New System.Drawing.Point(8, 79)
        Me.timingParametersGroupBox.Name = "timingParametersGroupBox"
        Me.timingParametersGroupBox.Size = New System.Drawing.Size(288, 96)
        Me.timingParametersGroupBox.TabIndex = 11
        Me.timingParametersGroupBox.TabStop = False
        Me.timingParametersGroupBox.Text = "Timing Parameters"
        '
        'samplesPerChannelNumericUpDown
        '
        Me.samplesPerChannelNumericUpDown.Location = New System.Drawing.Point(160, 24)
        Me.samplesPerChannelNumericUpDown.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.samplesPerChannelNumericUpDown.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.samplesPerChannelNumericUpDown.Name = "samplesPerChannelNumericUpDown"
        Me.samplesPerChannelNumericUpDown.TabIndex = 2
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
        Me.Controls.Add(Me.channelParametersGroupBox)
        Me.Controls.Add(Me.readButton)
        Me.Controls.Add(Me.resultsGroupBox)
        Me.Controls.Add(Me.triggerParametersGroupBox)
        Me.Controls.Add(Me.timingParametersGroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Read Digital Chan - Internal Clock - Digital Reference"
        Me.channelParametersGroupBox.ResumeLayout(False)
        Me.resultsGroupBox.ResumeLayout(False)
        CType(Me.resultsDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.triggerParametersGroupBox.ResumeLayout(False)
        CType(Me.preTriggerSamplesNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
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
            myTask = New Task
            myTask.DIChannels.CreateChannel(physicalChannelComboBox.Text, "", ChannelLineGrouping.OneChannelForEachLine)

            myTask.Timing.ConfigureSampleClock("", CType(samplesClockRateNumericUpDown.Value, Double), SampleClockActiveEdge.Rising, SampleQuantityMode.FiniteSamples, CType(samplesPerChannelNumericUpDown.Value, Integer))

            Dim Edge As DigitalEdgeReferenceTriggerEdge

            If (edgeComboBox.Text = "Rising") Then
                Edge = DigitalEdgeReferenceTriggerEdge.Rising
            Else
                Edge = DigitalEdgeReferenceTriggerEdge.Falling
            End If

            myTask.Control(TaskAction.Verify)

            myTask.Triggers.ReferenceTrigger.ConfigureDigitalEdgeTrigger(triggerSourceComboBox.Text, Edge, CType(preTriggerSamplesNumericUpDown.Value, Long))

            myTask.Start()

            InitializeDataTable(dataTable)
            resultsDataGrid.DataSource = dataTable

            reader = New DigitalMultiChannelReader(myTask.Stream)

            waveform = reader.ReadWaveform(CType(samplesPerChannelNumericUpDown.Value, Integer))

            DataToDataTable(waveform, dataTable)

        Catch exception As DaqException
            MessageBox.Show(exception.Message)
        Finally
            myTask.Dispose()
            readButton.Enabled = True
        End Try
    End Sub

    Private Sub DataToDataTable(ByVal waveform As DigitalWaveform(), ByRef dataTable As dataTable)
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
