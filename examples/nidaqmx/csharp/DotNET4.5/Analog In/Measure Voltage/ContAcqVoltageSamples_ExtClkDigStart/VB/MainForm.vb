'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   ContAcqVoltageSamples_ExtClkDigStart
'
' Category:
'   AI
'
' Description:
'   This example demonstrates how to continuously acquire analog voltage data
'   using an external clock, started by a digital trigger.
'
' Instructions for running:
'   1.  Select the physical channel corresponding to where your signal is input
'       on the DAQ device.
'   2.  Enter the minimum and maximum voltage values.Note: For better accuracy,
'       try to match the input range to the expected voltage level of the
'       measured signal.
'   3.  Select the source for the external sample clock.
'   4.  Set the approximate rate of the sample clock. This allows the internal
'       characteristics of the acquisition to be as efficient as possible.
'   5.  Select the source for the digital edge start trigger.
'   6.  Select the edge, rising or falling, on which to trigger.
'
' Steps:
'   1.  Create a new analog input task.
'   2.  Create an analog input voltage channel and define the mode to be
'       continuous.
'   3.  Define the parameters for the external clock source.
'   4.  Define the parameters for the digital start trigger.
'   5.  Call AnalogMultiChannelReader.BeginReadWaveform to install a callback
'       and begin the asynchronous read operation.
'   6.  Inside the callback, call AnalogMultiChannelReader.EndReadWaveform to
'       retrieve the data from the read operation.  
'   7.  Call AnalogMultiChannelReader.BeginMemoryOptimizedReadWaveform
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
        triggerEdgeComboBox.SelectedIndex = 0
        triggerEdge = DigitalEdgeStartTriggerEdge.Rising
        stopButton.Enabled = False
        myAsyncCallback = New AsyncCallback(AddressOf AnalogInCallback)

        physicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AI, PhysicalChannelAccess.External))
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
            If Not (analogInTask Is Nothing) Then
                runningTask = Nothing
                analogInTask.Dispose()
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
    Friend WithEvents triggerParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents triggerEdgeComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents triggerEdgeLabel As System.Windows.Forms.Label
    Friend WithEvents triggerSourceLabel As System.Windows.Forms.Label
    Friend WithEvents triggerSourceTextBox As System.Windows.Forms.TextBox
    Friend WithEvents rateLabel As System.Windows.Forms.Label
    Friend WithEvents clockSourceLabel As System.Windows.Forms.Label
    Friend WithEvents clockSourceTextBox As System.Windows.Forms.TextBox
    Friend WithEvents channelParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents maximumLabel As System.Windows.Forms.Label
    Friend WithEvents minimumLabel As System.Windows.Forms.Label
    Friend WithEvents physicalChannelLabel As System.Windows.Forms.Label
    Friend WithEvents timingParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents acquisitionResultGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents resultLabel As System.Windows.Forms.Label
    Friend WithEvents acquisitionDataGrid As System.Windows.Forms.DataGrid
    Friend WithEvents rateNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents minimumValueNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents maximumValueNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents physicalChannelComboBox As System.Windows.Forms.ComboBox

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MainForm))
        Me.stopButton = New System.Windows.Forms.Button
        Me.startButton = New System.Windows.Forms.Button
        Me.triggerParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.triggerEdgeComboBox = New System.Windows.Forms.ComboBox
        Me.triggerEdgeLabel = New System.Windows.Forms.Label
        Me.triggerSourceLabel = New System.Windows.Forms.Label
        Me.triggerSourceTextBox = New System.Windows.Forms.TextBox
        Me.timingParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.rateNumeric = New System.Windows.Forms.NumericUpDown
        Me.rateLabel = New System.Windows.Forms.Label
        Me.clockSourceLabel = New System.Windows.Forms.Label
        Me.clockSourceTextBox = New System.Windows.Forms.TextBox
        Me.channelParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.physicalChannelComboBox = New System.Windows.Forms.ComboBox
        Me.minimumValueNumeric = New System.Windows.Forms.NumericUpDown
        Me.maximumValueNumeric = New System.Windows.Forms.NumericUpDown
        Me.maximumLabel = New System.Windows.Forms.Label
        Me.physicalChannelLabel = New System.Windows.Forms.Label
        Me.minimumLabel = New System.Windows.Forms.Label
        Me.acquisitionResultGroupBox = New System.Windows.Forms.GroupBox
        Me.resultLabel = New System.Windows.Forms.Label
        Me.acquisitionDataGrid = New System.Windows.Forms.DataGrid
        Me.triggerParametersGroupBox.SuspendLayout()
        Me.timingParametersGroupBox.SuspendLayout()
        CType(Me.rateNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.channelParametersGroupBox.SuspendLayout()
        CType(Me.minimumValueNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.maximumValueNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.acquisitionResultGroupBox.SuspendLayout()
        CType(Me.acquisitionDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'stopButton
        '
        Me.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.stopButton.Location = New System.Drawing.Point(136, 504)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(80, 24)
        Me.stopButton.TabIndex = 1
        Me.stopButton.Text = "Stop"
        '
        'startButton
        '
        Me.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.startButton.Location = New System.Drawing.Point(24, 504)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(80, 24)
        Me.startButton.TabIndex = 0
        Me.startButton.Text = "Start"
        '
        'triggerParametersGroupBox
        '
        Me.triggerParametersGroupBox.Controls.Add(Me.triggerEdgeComboBox)
        Me.triggerParametersGroupBox.Controls.Add(Me.triggerEdgeLabel)
        Me.triggerParametersGroupBox.Controls.Add(Me.triggerSourceLabel)
        Me.triggerParametersGroupBox.Controls.Add(Me.triggerSourceTextBox)
        Me.triggerParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.triggerParametersGroupBox.Location = New System.Drawing.Point(8, 248)
        Me.triggerParametersGroupBox.Name = "triggerParametersGroupBox"
        Me.triggerParametersGroupBox.Size = New System.Drawing.Size(224, 88)
        Me.triggerParametersGroupBox.TabIndex = 4
        Me.triggerParametersGroupBox.TabStop = False
        Me.triggerParametersGroupBox.Text = "Trigger Parameters"
        '
        'triggerEdgeComboBox
        '
        Me.triggerEdgeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.triggerEdgeComboBox.Items.AddRange(New Object() {"Rising Edge", "Falling Edge"})
        Me.triggerEdgeComboBox.Location = New System.Drawing.Point(120, 56)
        Me.triggerEdgeComboBox.Name = "triggerEdgeComboBox"
        Me.triggerEdgeComboBox.Size = New System.Drawing.Size(96, 21)
        Me.triggerEdgeComboBox.TabIndex = 3
        '
        'triggerEdgeLabel
        '
        Me.triggerEdgeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.triggerEdgeLabel.Location = New System.Drawing.Point(16, 58)
        Me.triggerEdgeLabel.Name = "triggerEdgeLabel"
        Me.triggerEdgeLabel.Size = New System.Drawing.Size(80, 16)
        Me.triggerEdgeLabel.TabIndex = 2
        Me.triggerEdgeLabel.Text = "Trigger Edge:"
        '
        'triggerSourceLabel
        '
        Me.triggerSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.triggerSourceLabel.Location = New System.Drawing.Point(16, 26)
        Me.triggerSourceLabel.Name = "triggerSourceLabel"
        Me.triggerSourceLabel.Size = New System.Drawing.Size(88, 16)
        Me.triggerSourceLabel.TabIndex = 0
        Me.triggerSourceLabel.Text = "Trigger Source:"
        '
        'triggerSourceTextBox
        '
        Me.triggerSourceTextBox.Location = New System.Drawing.Point(120, 24)
        Me.triggerSourceTextBox.Name = "triggerSourceTextBox"
        Me.triggerSourceTextBox.Size = New System.Drawing.Size(96, 20)
        Me.triggerSourceTextBox.TabIndex = 1
        Me.triggerSourceTextBox.Text = "/Dev1/PFI0"
        '
        'timingParametersGroupBox
        '
        Me.timingParametersGroupBox.Controls.Add(Me.rateNumeric)
        Me.timingParametersGroupBox.Controls.Add(Me.rateLabel)
        Me.timingParametersGroupBox.Controls.Add(Me.clockSourceLabel)
        Me.timingParametersGroupBox.Controls.Add(Me.clockSourceTextBox)
        Me.timingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.timingParametersGroupBox.Location = New System.Drawing.Point(8, 144)
        Me.timingParametersGroupBox.Name = "timingParametersGroupBox"
        Me.timingParametersGroupBox.Size = New System.Drawing.Size(224, 96)
        Me.timingParametersGroupBox.TabIndex = 3
        Me.timingParametersGroupBox.TabStop = False
        Me.timingParametersGroupBox.Text = "Timing Parameters"
        '
        'rateNumeric
        '
        Me.rateNumeric.DecimalPlaces = 2
        Me.rateNumeric.Location = New System.Drawing.Point(120, 64)
        Me.rateNumeric.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.rateNumeric.Name = "rateNumeric"
        Me.rateNumeric.Size = New System.Drawing.Size(96, 20)
        Me.rateNumeric.TabIndex = 3
        Me.rateNumeric.Value = New Decimal(New Integer() {10000, 0, 0, 0})
        '
        'rateLabel
        '
        Me.rateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.rateLabel.Location = New System.Drawing.Point(16, 66)
        Me.rateLabel.Name = "rateLabel"
        Me.rateLabel.Size = New System.Drawing.Size(40, 16)
        Me.rateLabel.TabIndex = 2
        Me.rateLabel.Text = "Rate:"
        '
        'clockSourceLabel
        '
        Me.clockSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.clockSourceLabel.Location = New System.Drawing.Point(16, 26)
        Me.clockSourceLabel.Name = "clockSourceLabel"
        Me.clockSourceLabel.Size = New System.Drawing.Size(80, 16)
        Me.clockSourceLabel.TabIndex = 0
        Me.clockSourceLabel.Text = "Clock Source:"
        '
        'clockSourceTextBox
        '
        Me.clockSourceTextBox.Location = New System.Drawing.Point(120, 24)
        Me.clockSourceTextBox.Name = "clockSourceTextBox"
        Me.clockSourceTextBox.Size = New System.Drawing.Size(96, 20)
        Me.clockSourceTextBox.TabIndex = 1
        Me.clockSourceTextBox.Text = "/Dev1/PFI7"
        '
        'channelParametersGroupBox
        '
        Me.channelParametersGroupBox.Controls.Add(Me.physicalChannelComboBox)
        Me.channelParametersGroupBox.Controls.Add(Me.minimumValueNumeric)
        Me.channelParametersGroupBox.Controls.Add(Me.maximumValueNumeric)
        Me.channelParametersGroupBox.Controls.Add(Me.maximumLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.physicalChannelLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.minimumLabel)
        Me.channelParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.channelParametersGroupBox.Location = New System.Drawing.Point(8, 8)
        Me.channelParametersGroupBox.Name = "channelParametersGroupBox"
        Me.channelParametersGroupBox.Size = New System.Drawing.Size(224, 128)
        Me.channelParametersGroupBox.TabIndex = 2
        Me.channelParametersGroupBox.TabStop = False
        Me.channelParametersGroupBox.Text = "Channel Parameters"
        '
        'physicalChannelComboBox
        '
        Me.physicalChannelComboBox.Location = New System.Drawing.Point(120, 24)
        Me.physicalChannelComboBox.Name = "physicalChannelComboBox"
        Me.physicalChannelComboBox.Size = New System.Drawing.Size(96, 21)
        Me.physicalChannelComboBox.TabIndex = 1
        Me.physicalChannelComboBox.Text = "Dev1/ai0"
        '
        'minimumValueNumeric
        '
        Me.minimumValueNumeric.DecimalPlaces = 2
        Me.minimumValueNumeric.Location = New System.Drawing.Point(120, 56)
        Me.minimumValueNumeric.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.minimumValueNumeric.Minimum = New Decimal(New Integer() {10, 0, 0, -2147483648})
        Me.minimumValueNumeric.Name = "minimumValueNumeric"
        Me.minimumValueNumeric.Size = New System.Drawing.Size(96, 20)
        Me.minimumValueNumeric.TabIndex = 3
        Me.minimumValueNumeric.Value = New Decimal(New Integer() {100, 0, 0, -2147418112})
        '
        'maximumValueNumeric
        '
        Me.maximumValueNumeric.DecimalPlaces = 2
        Me.maximumValueNumeric.Location = New System.Drawing.Point(120, 88)
        Me.maximumValueNumeric.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.maximumValueNumeric.Minimum = New Decimal(New Integer() {10, 0, 0, -2147483648})
        Me.maximumValueNumeric.Name = "maximumValueNumeric"
        Me.maximumValueNumeric.Size = New System.Drawing.Size(96, 20)
        Me.maximumValueNumeric.TabIndex = 5
        Me.maximumValueNumeric.Value = New Decimal(New Integer() {100, 0, 0, 65536})
        '
        'maximumLabel
        '
        Me.maximumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.maximumLabel.Location = New System.Drawing.Point(16, 90)
        Me.maximumLabel.Name = "maximumLabel"
        Me.maximumLabel.Size = New System.Drawing.Size(112, 16)
        Me.maximumLabel.TabIndex = 4
        Me.maximumLabel.Text = "Maximum Value (V):"
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
        'minimumLabel
        '
        Me.minimumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.minimumLabel.Location = New System.Drawing.Point(16, 58)
        Me.minimumLabel.Name = "minimumLabel"
        Me.minimumLabel.Size = New System.Drawing.Size(104, 16)
        Me.minimumLabel.TabIndex = 2
        Me.minimumLabel.Text = "Minimum Value (V):"
        '
        'acquisitionResultGroupBox
        '
        Me.acquisitionResultGroupBox.Controls.Add(Me.resultLabel)
        Me.acquisitionResultGroupBox.Controls.Add(Me.acquisitionDataGrid)
        Me.acquisitionResultGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.acquisitionResultGroupBox.Location = New System.Drawing.Point(9, 344)
        Me.acquisitionResultGroupBox.Name = "acquisitionResultGroupBox"
        Me.acquisitionResultGroupBox.Size = New System.Drawing.Size(224, 152)
        Me.acquisitionResultGroupBox.TabIndex = 5
        Me.acquisitionResultGroupBox.TabStop = False
        Me.acquisitionResultGroupBox.Text = "Acquisition Results"
        '
        'resultLabel
        '
        Me.resultLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.resultLabel.Location = New System.Drawing.Point(16, 16)
        Me.resultLabel.Name = "resultLabel"
        Me.resultLabel.Size = New System.Drawing.Size(112, 16)
        Me.resultLabel.TabIndex = 0
        Me.resultLabel.Text = "Acquisition Data (V):"
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
        Me.acquisitionDataGrid.Size = New System.Drawing.Size(208, 112)
        Me.acquisitionDataGrid.TabIndex = 1
        Me.acquisitionDataGrid.TabStop = False
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(242, 543)
        Me.Controls.Add(Me.acquisitionResultGroupBox)
        Me.Controls.Add(Me.stopButton)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.triggerParametersGroupBox)
        Me.Controls.Add(Me.timingParametersGroupBox)
        Me.Controls.Add(Me.channelParametersGroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Continuous External Clock Acquisition - DigStart"
        Me.triggerParametersGroupBox.ResumeLayout(False)
        Me.timingParametersGroupBox.ResumeLayout(False)
        CType(Me.rateNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.channelParametersGroupBox.ResumeLayout(False)
        CType(Me.minimumValueNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.maximumValueNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.acquisitionResultGroupBox.ResumeLayout(False)
        CType(Me.acquisitionDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private triggerEdge As DigitalEdgeStartTriggerEdge
    Private analogInReader As AnalogMultiChannelReader
    Private analogInTask As Task
    Private runningTask As Task
    Private myAsyncCallback As AsyncCallback
    Private data As AnalogWaveform(Of Double)()
    Private dataColumn As dataColumn()             'Channels of Data
    Private dataTable As dataTable = New dataTable 'Table to Display Data

    Private Sub startButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles startButton.Click
        Try
            If runningTask Is Nothing Then
                stopButton.Enabled = True
                startButton.Enabled = False

                ' Create a new task
                analogInTask = New Task()

                ' Create a virtual channel
                analogInTask.AIChannels.CreateVoltageChannel(physicalChannelComboBox.Text, "", _
                    CType(-1, AITerminalConfiguration), Convert.ToDouble(minimumValueNumeric.Value), _
                    Convert.ToDouble(maximumValueNumeric.Value), AIVoltageUnits.Volts)

                analogInTask.Timing.ConfigureSampleClock(clockSourceTextBox.Text, Convert.ToDouble(rateNumeric.Value), _
                    SampleClockActiveEdge.Rising, SampleQuantityMode.ContinuousSamples, 1000)

                analogInTask.Triggers.StartTrigger.ConfigureDigitalEdgeTrigger(triggerSourceTextBox.Text, triggerEdge)

                ' Verify the Task
                analogInTask.Control(TaskAction.Verify)

                analogInReader = New AnalogMultiChannelReader(analogInTask.Stream)

                ' Prepare the table for Data
                InitializeDataTable(analogInTask.AIChannels, dataTable)
                acquisitionDataGrid.DataSource = dataTable
                runningTask = analogInTask

                ' Use SynchronizeCallbacks to specify that the object 
                ' marshals callbacks across threads appropriately.
                analogInReader.SynchronizeCallbacks = True
                analogInReader.BeginReadWaveform(Convert.ToInt32(rateNumeric.Value), myAsyncCallback, analogInTask)

            End If
        Catch ex As DaqException
            MessageBox.Show(ex.Message)
            runningTask = Nothing
            analogInTask.Dispose()
            stopButton.Enabled = False
            startButton.Enabled = True
        End Try
    End Sub

    Private Sub AnalogInCallback(ByVal ar As IAsyncResult)
        Try
            If (Not (runningTask Is Nothing)) AndAlso runningTask Is ar.AsyncState Then
                ' Retrieve data
                data = analogInReader.EndReadWaveform(ar)

                'Plot your data here 
                dataToDataTable(data, dataTable)

                analogInReader.BeginMemoryOptimizedReadWaveform(Convert.ToInt32(Convert.ToDouble(rateNumeric.Value)), myAsyncCallback, analogInTask, data)

            End If
        Catch ex As DaqException
            MessageBox.Show(ex.Message)
            runningTask = Nothing
            analogInTask.Dispose()
            stopButton.Enabled = False
            startButton.Enabled = True
        End Try
    End Sub

    Private Sub stopButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles stopButton.Click
        runningTask = Nothing
        analogInTask.Stop()
        analogInTask.Dispose()
        stopButton.Enabled = False
        startButton.Enabled = True
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

    Public Sub InitializeDataTable(ByVal channelCollection As AIChannelCollection, ByRef data As dataTable)
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

    Private Sub triggerEdgeComboBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles triggerEdgeComboBox.SelectedIndexChanged
        If triggerEdgeComboBox.SelectedIndex = 0 Then
            triggerEdge = DigitalEdgeStartTriggerEdge.Rising
        Else
            triggerEdge = DigitalEdgeStartTriggerEdge.Falling
        End If
    End Sub
End Class
