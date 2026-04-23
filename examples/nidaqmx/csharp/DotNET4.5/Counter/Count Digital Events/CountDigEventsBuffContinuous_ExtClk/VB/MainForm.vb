'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   CountDigEventsBuffContinuous_ExtClk
'
' Category:
'   CI
'
' Description:
'   This example demonstrates how to count buffered digital events on a Counter
'   Input channel.  
'   The initial count, count direction, edge, and sample clock source are all
'   configurable.  Edges are counted 
'   on the counter's default input terminal (see I/O Connections Overview below
'   for more information), but 
'   could easily be modified to count edges on a PFI or RTSI line.Note: For
'   buffered event counting, an external 
'   sample clock is necessary to signal when a sample should be inserted into the
'   buffer.  Specify the source 
'   terminal of the external clock in the clock source text box when you run the
'   example.
'
' Instructions for running:
'   1.  Enter the physical channel which corresponds to the counter you want to
'       use to count edges on the DAQ device.
'   2.  Enter the initial count, count direction, and measurement edge to
'       specify how you want the counter to count.
'   3.  Set the sample clock source. Note:  An external sample clock must be
'       used.  Counters do not have an internal sample clock available.  You can
'       use the GenDigPulseTrain_Continuous example to generate a pulse train on
'       another counter and connect it to the sample clock source you are using 
'       in this example.
'
' Steps:
'   1.  Create a counter input channel to count events.  The edge parameter is
'       used to determine if the counter will count rising or falling edges.
'   2.  Call ConfigureSampleClock to configure the external sample clock timing
'       parameters such as sample mode and sample clock source.  The sample
'       clock source determines when a sample will be inserted into the buffer. 
'       The edge parameter can be used to determine when a sample is taken.
'   3.  Call the CounterReader.BeginReadMultiSampleDouble method to arm the
'       counter and begin counting.  The counter will be preloaded with the
'       initial count.
'   4.  For continuous measurements, the counter will continually read new data
'       every time the set number of samples becomes available in the buffer.
'   5.  Call Task.Dispose to stop the task and de-allocate any resources used by
'       the task
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
'   This example will perform a measurement on the default terminal(s) of the
'   counter specified. In this example the two edge separation will be measured
'   on the default input terminals on ctr0. For more information on the default
'   counter input and output terminals for your device, open the NI-DAQmx Help,
'   and refer to Counter Signal Connections found under the Device
'   Considerations book in the table of contents.
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

        countDirectionComboBox.SelectedIndex = 0

        counterComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.CI, PhysicalChannelAccess.External))
        If (counterComboBox.Items.Count > 0) Then
            counterComboBox.SelectedIndex = 0
        End If

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
            If Not (counterReadTask Is Nothing) Then
                runningTask = Nothing
                counterReadTask.Dispose()
            End If
            If Not (runningTask Is Nothing) Then
                runningTask.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.

    Friend WithEvents startButton As System.Windows.Forms.Button
    Friend WithEvents stopButton As System.Windows.Forms.Button
    Friend WithEvents channelParameterGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents edgeGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents fallingRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents risingRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents countDirectionLabel As System.Windows.Forms.Label
    Friend WithEvents initialCountTextBox As System.Windows.Forms.TextBox
    Friend WithEvents initialCountLabel As System.Windows.Forms.Label
    Friend WithEvents counterLabel As System.Windows.Forms.Label
    Friend WithEvents countDirectionComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents sampleClockTextBox As System.Windows.Forms.TextBox
    Friend WithEvents clockSourceLabel As System.Windows.Forms.Label
    Friend WithEvents dataGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents countLabel As System.Windows.Forms.Label
    Friend WithEvents sampleToReadTextBox As System.Windows.Forms.TextBox
    Friend WithEvents sampleReadLabel As System.Windows.Forms.Label
    Friend WithEvents rateTextBox As System.Windows.Forms.TextBox
    Friend WithEvents rateLabel As System.Windows.Forms.Label
    Friend WithEvents dataListBox As System.Windows.Forms.ListBox
    Friend WithEvents timingParameterGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents counterComboBox As System.Windows.Forms.ComboBox

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MainForm))
        Me.startButton = New System.Windows.Forms.Button
        Me.stopButton = New System.Windows.Forms.Button
        Me.channelParameterGroupBox = New System.Windows.Forms.GroupBox
        Me.counterComboBox = New System.Windows.Forms.ComboBox
        Me.edgeGroupBox = New System.Windows.Forms.GroupBox
        Me.fallingRadioButton = New System.Windows.Forms.RadioButton
        Me.risingRadioButton = New System.Windows.Forms.RadioButton
        Me.countDirectionLabel = New System.Windows.Forms.Label
        Me.initialCountTextBox = New System.Windows.Forms.TextBox
        Me.initialCountLabel = New System.Windows.Forms.Label
        Me.counterLabel = New System.Windows.Forms.Label
        Me.countDirectionComboBox = New System.Windows.Forms.ComboBox
        Me.timingParameterGroupBox = New System.Windows.Forms.GroupBox
        Me.sampleToReadTextBox = New System.Windows.Forms.TextBox
        Me.sampleReadLabel = New System.Windows.Forms.Label
        Me.rateTextBox = New System.Windows.Forms.TextBox
        Me.rateLabel = New System.Windows.Forms.Label
        Me.sampleClockTextBox = New System.Windows.Forms.TextBox
        Me.clockSourceLabel = New System.Windows.Forms.Label
        Me.dataGroupBox = New System.Windows.Forms.GroupBox
        Me.dataListBox = New System.Windows.Forms.ListBox
        Me.countLabel = New System.Windows.Forms.Label
        Me.channelParameterGroupBox.SuspendLayout()
        Me.edgeGroupBox.SuspendLayout()
        Me.timingParameterGroupBox.SuspendLayout()
        Me.dataGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'startButton
        '
        Me.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.startButton.Location = New System.Drawing.Point(192, 224)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(112, 32)
        Me.startButton.TabIndex = 0
        Me.startButton.Text = "Start"
        '
        'stopButton
        '
        Me.stopButton.Enabled = False
        Me.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.stopButton.Location = New System.Drawing.Point(328, 224)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(112, 32)
        Me.stopButton.TabIndex = 1
        Me.stopButton.Text = "Stop"
        '
        'channelParameterGroupBox
        '
        Me.channelParameterGroupBox.Controls.Add(Me.counterComboBox)
        Me.channelParameterGroupBox.Controls.Add(Me.edgeGroupBox)
        Me.channelParameterGroupBox.Controls.Add(Me.countDirectionLabel)
        Me.channelParameterGroupBox.Controls.Add(Me.initialCountTextBox)
        Me.channelParameterGroupBox.Controls.Add(Me.initialCountLabel)
        Me.channelParameterGroupBox.Controls.Add(Me.counterLabel)
        Me.channelParameterGroupBox.Controls.Add(Me.countDirectionComboBox)
        Me.channelParameterGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.channelParameterGroupBox.Location = New System.Drawing.Point(8, 8)
        Me.channelParameterGroupBox.Name = "channelParameterGroupBox"
        Me.channelParameterGroupBox.Size = New System.Drawing.Size(152, 272)
        Me.channelParameterGroupBox.TabIndex = 2
        Me.channelParameterGroupBox.TabStop = False
        Me.channelParameterGroupBox.Text = "Channel Parameters"
        '
        'counterComboBox
        '
        Me.counterComboBox.Location = New System.Drawing.Point(16, 40)
        Me.counterComboBox.Name = "counterComboBox"
        Me.counterComboBox.Size = New System.Drawing.Size(121, 21)
        Me.counterComboBox.TabIndex = 1
        Me.counterComboBox.Text = "Dev1/ctr0"
        '
        'edgeGroupBox
        '
        Me.edgeGroupBox.Controls.Add(Me.fallingRadioButton)
        Me.edgeGroupBox.Controls.Add(Me.risingRadioButton)
        Me.edgeGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.edgeGroupBox.Location = New System.Drawing.Point(16, 176)
        Me.edgeGroupBox.Name = "edgeGroupBox"
        Me.edgeGroupBox.Size = New System.Drawing.Size(120, 80)
        Me.edgeGroupBox.TabIndex = 6
        Me.edgeGroupBox.TabStop = False
        Me.edgeGroupBox.Text = "Edge"
        '
        'fallingRadioButton
        '
        Me.fallingRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.fallingRadioButton.Location = New System.Drawing.Point(16, 48)
        Me.fallingRadioButton.Name = "fallingRadioButton"
        Me.fallingRadioButton.Size = New System.Drawing.Size(80, 24)
        Me.fallingRadioButton.TabIndex = 1
        Me.fallingRadioButton.Text = "Falling"
        '
        'risingRadioButton
        '
        Me.risingRadioButton.Checked = True
        Me.risingRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.risingRadioButton.Location = New System.Drawing.Point(16, 24)
        Me.risingRadioButton.Name = "risingRadioButton"
        Me.risingRadioButton.Size = New System.Drawing.Size(80, 24)
        Me.risingRadioButton.TabIndex = 0
        Me.risingRadioButton.TabStop = True
        Me.risingRadioButton.Text = "Rising"
        '
        'countDirectionLabel
        '
        Me.countDirectionLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.countDirectionLabel.Location = New System.Drawing.Point(16, 120)
        Me.countDirectionLabel.Name = "countDirectionLabel"
        Me.countDirectionLabel.Size = New System.Drawing.Size(88, 16)
        Me.countDirectionLabel.TabIndex = 4
        Me.countDirectionLabel.Text = "Count Direction:"
        '
        'initialCountTextBox
        '
        Me.initialCountTextBox.Location = New System.Drawing.Point(16, 88)
        Me.initialCountTextBox.Name = "initialCountTextBox"
        Me.initialCountTextBox.Size = New System.Drawing.Size(120, 20)
        Me.initialCountTextBox.TabIndex = 3
        Me.initialCountTextBox.Text = "0"
        '
        'initialCountLabel
        '
        Me.initialCountLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.initialCountLabel.Location = New System.Drawing.Point(16, 72)
        Me.initialCountLabel.Name = "initialCountLabel"
        Me.initialCountLabel.Size = New System.Drawing.Size(72, 16)
        Me.initialCountLabel.TabIndex = 2
        Me.initialCountLabel.Text = "Initial Count:"
        '
        'counterLabel
        '
        Me.counterLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.counterLabel.Location = New System.Drawing.Point(16, 24)
        Me.counterLabel.Name = "counterLabel"
        Me.counterLabel.Size = New System.Drawing.Size(100, 16)
        Me.counterLabel.TabIndex = 0
        Me.counterLabel.Text = "Counter(s):"
        '
        'countDirectionComboBox
        '
        Me.countDirectionComboBox.Items.AddRange(New Object() {"Count Up", "Count Down", "Externally Controlled"})
        Me.countDirectionComboBox.Location = New System.Drawing.Point(16, 136)
        Me.countDirectionComboBox.Name = "countDirectionComboBox"
        Me.countDirectionComboBox.Size = New System.Drawing.Size(120, 21)
        Me.countDirectionComboBox.TabIndex = 5
        Me.countDirectionComboBox.Text = "Count Up"
        '
        'timingParameterGroupBox
        '
        Me.timingParameterGroupBox.Controls.Add(Me.sampleToReadTextBox)
        Me.timingParameterGroupBox.Controls.Add(Me.sampleReadLabel)
        Me.timingParameterGroupBox.Controls.Add(Me.rateTextBox)
        Me.timingParameterGroupBox.Controls.Add(Me.rateLabel)
        Me.timingParameterGroupBox.Controls.Add(Me.sampleClockTextBox)
        Me.timingParameterGroupBox.Controls.Add(Me.clockSourceLabel)
        Me.timingParameterGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.timingParameterGroupBox.Location = New System.Drawing.Point(168, 8)
        Me.timingParameterGroupBox.Name = "timingParameterGroupBox"
        Me.timingParameterGroupBox.Size = New System.Drawing.Size(144, 184)
        Me.timingParameterGroupBox.TabIndex = 3
        Me.timingParameterGroupBox.TabStop = False
        Me.timingParameterGroupBox.Text = "Timing Parameters"
        '
        'sampleToReadTextBox
        '
        Me.sampleToReadTextBox.Location = New System.Drawing.Point(16, 136)
        Me.sampleToReadTextBox.Name = "sampleToReadTextBox"
        Me.sampleToReadTextBox.Size = New System.Drawing.Size(112, 20)
        Me.sampleToReadTextBox.TabIndex = 5
        Me.sampleToReadTextBox.Text = "1000"
        '
        'sampleReadLabel
        '
        Me.sampleReadLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.sampleReadLabel.Location = New System.Drawing.Point(16, 120)
        Me.sampleReadLabel.Name = "sampleReadLabel"
        Me.sampleReadLabel.Size = New System.Drawing.Size(96, 16)
        Me.sampleReadLabel.TabIndex = 4
        Me.sampleReadLabel.Text = "Samples to Read:"
        '
        'rateTextBox
        '
        Me.rateTextBox.Location = New System.Drawing.Point(16, 88)
        Me.rateTextBox.Name = "rateTextBox"
        Me.rateTextBox.Size = New System.Drawing.Size(112, 20)
        Me.rateTextBox.TabIndex = 3
        Me.rateTextBox.Text = "1000.00"
        '
        'rateLabel
        '
        Me.rateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.rateLabel.Location = New System.Drawing.Point(16, 72)
        Me.rateLabel.Name = "rateLabel"
        Me.rateLabel.Size = New System.Drawing.Size(88, 16)
        Me.rateLabel.TabIndex = 2
        Me.rateLabel.Text = "Rate:"
        '
        'sampleClockTextBox
        '
        Me.sampleClockTextBox.Location = New System.Drawing.Point(16, 40)
        Me.sampleClockTextBox.Name = "sampleClockTextBox"
        Me.sampleClockTextBox.Size = New System.Drawing.Size(112, 20)
        Me.sampleClockTextBox.TabIndex = 1
        Me.sampleClockTextBox.Text = "/Dev1/PFI9"
        '
        'clockSourceLabel
        '
        Me.clockSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.clockSourceLabel.Location = New System.Drawing.Point(16, 24)
        Me.clockSourceLabel.Name = "clockSourceLabel"
        Me.clockSourceLabel.Size = New System.Drawing.Size(120, 16)
        Me.clockSourceLabel.TabIndex = 0
        Me.clockSourceLabel.Text = "Sample Clock Source:"
        '
        'dataGroupBox
        '
        Me.dataGroupBox.Controls.Add(Me.dataListBox)
        Me.dataGroupBox.Controls.Add(Me.countLabel)
        Me.dataGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.dataGroupBox.Location = New System.Drawing.Point(320, 8)
        Me.dataGroupBox.Name = "dataGroupBox"
        Me.dataGroupBox.Size = New System.Drawing.Size(128, 184)
        Me.dataGroupBox.TabIndex = 4
        Me.dataGroupBox.TabStop = False
        Me.dataGroupBox.Text = "Data"
        '
        'dataListBox
        '
        Me.dataListBox.Location = New System.Drawing.Point(16, 32)
        Me.dataListBox.Name = "dataListBox"
        Me.dataListBox.Size = New System.Drawing.Size(96, 134)
        Me.dataListBox.TabIndex = 1
        '
        'countLabel
        '
        Me.countLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.countLabel.Location = New System.Drawing.Point(16, 16)
        Me.countLabel.Name = "countLabel"
        Me.countLabel.Size = New System.Drawing.Size(48, 16)
        Me.countLabel.TabIndex = 0
        Me.countLabel.Text = "Counts:"
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(458, 288)
        Me.Controls.Add(Me.dataGroupBox)
        Me.Controls.Add(Me.channelParameterGroupBox)
        Me.Controls.Add(Me.timingParameterGroupBox)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.stopButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Count Digital Events Buffered Continuous - External Clock"
        Me.channelParameterGroupBox.ResumeLayout(False)
        Me.edgeGroupBox.ResumeLayout(False)
        Me.timingParameterGroupBox.ResumeLayout(False)
        Me.dataGroupBox.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    ' Global variables
    Private edgeType As CICountEdgesActiveEdge
    Private myCounterReader As CounterReader
    Private counterReadTask As Task
    Private runningTask As Task
    Private countDirection As CICountEdgesCountDirection = CICountEdgesCountDirection.Up
    Private countEdges As CICountEdgesActiveEdge = CICountEdgesActiveEdge.Rising
    Private myCallBack As AsyncCallback
    Private data As Double()
    Private actualNumberOfSamplesRead As Integer = 0



    Private Sub CounterReadCallback(ByVal ar As IAsyncResult)

        Try

            If (Not (runningTask Is Nothing)) AndAlso runningTask Is ar.AsyncState Then

                data = myCounterReader.EndMemoryOptimizedReadMultiSampleDouble(ar, actualNumberOfSamplesRead)

                'Display only the first 10 points acquired in the list box
                Dim samplesToShow As Int32 = 0
                If data.Length < 10 Then
                    samplesToShow = data.Length
                Else
                    samplesToShow = 9
                End If

                dataListBox.BeginUpdate()
                dataListBox.Items.Clear()

                For i As Int32 = 0 To samplesToShow
                    dataListBox.Items.Add(data(i))
                Next

                dataListBox.EndUpdate()

                myCounterReader.BeginMemoryOptimizedReadMultiSampleDouble( _
                    Convert.ToInt32(sampleToReadTextBox.Text), myCallBack, counterReadTask, data)

            End If

        Catch exception As DaqException

            counterReadTask.Dispose()
            MessageBox.Show(exception.Message)
            runningTask = Nothing
            startButton.Enabled = True
            stopButton.Enabled = False

        End Try
    End Sub

    Private Sub startButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles startButton.Click

        ' This example uses the default source (or gate) terminal for 
        ' the counter of your device.  To determine what the default 
        ' counter pins for your device are or to set a different source 
        ' (or gate) pin, refer to the Connecting Counter Signals topic
        ' in the NI-DAQmx Help (search for "Connecting Counter Signals").

        Try

            counterReadTask = New Task()

            counterReadTask.CIChannels.CreateCountEdgesChannel(counterComboBox.Text, "", _
                    edgeType, Convert.ToInt64(initialCountTextBox.Text), countDirection)

            counterReadTask.Timing.ConfigureSampleClock(sampleClockTextBox.Text, _
                    Convert.ToDouble(rateTextBox.Text), SampleClockActiveEdge.Rising, _
                    SampleQuantityMode.ContinuousSamples, 1000)

            runningTask = counterReadTask
            myCounterReader = New CounterReader(counterReadTask.Stream)

            myCallBack = New AsyncCallback(AddressOf CounterReadCallback)

            ' Use SynchronizeCallbacks to specify that the object 
            ' marshals callbacks across threads appropriately.
            myCounterReader.SynchronizeCallbacks = True

            ' Memory Optimized Read method needs an initialized array.
            data = New [Double](Convert.ToInt32(sampleToReadTextBox.Text) - 1) {}

            myCounterReader.BeginMemoryOptimizedReadMultiSampleDouble( _
                Convert.ToInt32(sampleToReadTextBox.Text), myCallBack, counterReadTask, data)

            startButton.Enabled = False
            stopButton.Enabled = True

        Catch exception As DaqException

            MessageBox.Show(exception.Message)
            counterReadTask.Dispose()
            startButton.Enabled = True
            stopButton.Enabled = False
            runningTask = Nothing

        End Try

    End Sub

    Private Sub stopButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles stopButton.Click

        runningTask = Nothing
        counterReadTask.Dispose()
        startButton.Enabled = True
        stopButton.Enabled = False

    End Sub

    Private Sub fallingRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles fallingRadioButton.CheckedChanged
        edgeType = CICountEdgesActiveEdge.Falling
    End Sub

    Private Sub risingRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles risingRadioButton.CheckedChanged
        edgeType = CICountEdgesActiveEdge.Rising
    End Sub

    Private Sub countDirectionComboBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles countDirectionComboBox.SelectedIndexChanged

        Select Case countDirectionComboBox.SelectedIndex
            Case 0 : countDirection = CICountEdgesCountDirection.Up
            Case 1 : countDirection = CICountEdgesCountDirection.Down
            Case 2 : countDirection = CICountEdgesCountDirection.ExternallyControlled
        End Select

    End Sub

End Class
