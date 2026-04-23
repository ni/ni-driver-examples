'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   Meas2EdgeSeparation_BufCont
'
' Category:
'   CI
'
' Description:
'   This example demonstrates how to perform a continuous number of two edge
'   separation measurements 
'   on a counter input channel.  The first edge, second edge, minimum value,
'   maximum value, and samples to 
'   read are all configurable. This example shows how to perform a two edge
'   separation measurement on the 
'   counter's default input terminals (refer to the I/O Connections Overview below
'   for more information), 
'   but could easily be expanded to measure two edge separation on any PFI, RTSI,
'   or internal signal.Refer 
'   to your device documentation to see if your device supports two edge
'   separation measurements.
'
' Instructions for running:
'   1.  Select the physical channel which corresponds to the counter on the DAQ
'       device you want to perform a two edge separation measurement on.
'   2.  Enter the first edge and second edge corresponding to the two edges you
'       want the counter to measure.  Enter the maximum and minimum value to
'       specify the range of your unknown two edge separation.  Additionally,
'       you can choose the first and second edge input terminals.Note:  It is
'       important to set the maximum and minimum values of your unknown two edge
'       separation as accurately as possible so the best internal timebase can
'       be chosen to minimize measurement error.  The default values specify a
'       range that can be measured by the counter using the 20MHzTimebase.
'   3.  Set the samples to read.  This will determine how many samples are read
'       during each asynchronous cycle.
'
' Steps:
'   1.  Create a Task.
'   2.  Create a CIChannel object by using the CreateTwoEdgeSeparationChannel
'       method.  The first and second edge parameters are used to specify the
'       rising or falling edge of one digital signal and the rising or falling
'       edge of another digital signal.  It is important to set the maximum and
'       minimum values of your unknown two edge separation as accurately as
'       possible so the best internal timebase can be chosen to minimize
'       measurement error.  The default values specify a range that can be
'       measured by the counter using the 20MHzTimebase.
'   3.  Call the ConfigureImplicit method to configure the sample mode to be
'       continuous.  Note: For time measurements with counters, the implicit
'       timing method is used because the signals being measured determine the
'       sample rate.
'   4.  Create a CounterReader object and use the BeginReadMultiSampleDouble
'       method to initiate the measurement and return the data.
'   5.  For continuous measurements, the counter will continually read all
'       available data until the Stop button is pressed.
'   6.  Dispose the Task object to clean-up any resources associated with the
'       task.
'   7.  Handle any DaqExceptions, if they occur.
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
'   The counter will perform a two edge separation measurement on the first and
'   second edge input terminals of the counter specified by the physical
'   channel.In this example the two edge separation will be measured on the
'   default input terminals on ctr0. For more information on the default counter
'   input and output terminals for your device, open the NI-DAQmx Help, and
'   refer to Counter Signal Connections found under the Device Considerations
'   book in the table of contents.
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
Imports NationalInstruments.DAQmx


Public Class MainForm
    Inherits System.Windows.Forms.Form

    Private myTask As Task
    Private runningTask As Task
    Private counterInReader As CounterReader
    Private asyncCB As AsyncCallback
    Private data() As Double
    Private actualNumberOfSamplesRead As Integer = 0
    Private numberOfSamples As Integer
    Private firstEdge As CITwoEdgeSeparationFirstEdge
    Private secondEdge As CITwoEdgeSeparationSecondEdge

    Private channelParametersGroupBox As System.Windows.Forms.GroupBox
    Private maximumLabel As System.Windows.Forms.Label
    Private minimumLabel As System.Windows.Forms.Label
    Private physicalChannelLabel As System.Windows.Forms.Label
    Private maximumTextBox As System.Windows.Forms.TextBox
    Private minimumTextBox As System.Windows.Forms.TextBox
    Private acqResultGroupBox As System.Windows.Forms.GroupBox
    Private resultLabel As System.Windows.Forms.Label
    Private firstEdgeComboBox As System.Windows.Forms.ComboBox
    Private firstEdgeLabel As System.Windows.Forms.Label
    Private secondEdgeLabel As System.Windows.Forms.Label
    Private secondEdgeComboBox As System.Windows.Forms.ComboBox
    Private acquisitionDataTextBox As System.Windows.Forms.TextBox
    Private counterComboBox As System.Windows.Forms.ComboBox
    Private timingGroupBox As GroupBox
    Private samplesLabel As Label
    Private samplesTextBox As TextBox
    Private WithEvents startButton As System.Windows.Forms.Button
    Private WithEvents stopButton As Button
    Private components As System.ComponentModel.Container = Nothing


    Public Sub New()
        '
        ' Required for Windows Form Designer support
        '
        InitializeComponent()

        firstEdgeComboBox.SelectedIndex = 0
        secondEdgeComboBox.SelectedIndex = 1

        counterComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.CI, PhysicalChannelAccess.External))
        If counterComboBox.Items.Count > 0 Then
            counterComboBox.SelectedIndex = 0
            startButton.Enabled = True
        End If
    End Sub 'New

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
    End Sub 'Dispose


    Private Sub InitializeComponent()
        Dim resources As New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.channelParametersGroupBox = New System.Windows.Forms.GroupBox()
        Me.counterComboBox = New System.Windows.Forms.ComboBox()
        Me.secondEdgeLabel = New System.Windows.Forms.Label()
        Me.secondEdgeComboBox = New System.Windows.Forms.ComboBox()
        Me.firstEdgeLabel = New System.Windows.Forms.Label()
        Me.maximumLabel = New System.Windows.Forms.Label()
        Me.minimumLabel = New System.Windows.Forms.Label()
        Me.physicalChannelLabel = New System.Windows.Forms.Label()
        Me.maximumTextBox = New System.Windows.Forms.TextBox()
        Me.minimumTextBox = New System.Windows.Forms.TextBox()
        Me.firstEdgeComboBox = New System.Windows.Forms.ComboBox()
        Me.acqResultGroupBox = New System.Windows.Forms.GroupBox()
        Me.acquisitionDataTextBox = New System.Windows.Forms.TextBox()
        Me.resultLabel = New System.Windows.Forms.Label()
        Me.startButton = New System.Windows.Forms.Button()
        Me.timingGroupBox = New System.Windows.Forms.GroupBox()
        Me.samplesLabel = New System.Windows.Forms.Label()
        Me.samplesTextBox = New System.Windows.Forms.TextBox()
        Me.stopButton = New System.Windows.Forms.Button()
        Me.channelParametersGroupBox.SuspendLayout()
        Me.acqResultGroupBox.SuspendLayout()
        Me.timingGroupBox.SuspendLayout()
        Me.SuspendLayout()
        ' 
        ' channelParametersGroupBox
        ' 
        Me.channelParametersGroupBox.Controls.Add(Me.counterComboBox)
        Me.channelParametersGroupBox.Controls.Add(Me.secondEdgeLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.secondEdgeComboBox)
        Me.channelParametersGroupBox.Controls.Add(Me.firstEdgeLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.maximumLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.minimumLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.physicalChannelLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.maximumTextBox)
        Me.channelParametersGroupBox.Controls.Add(Me.minimumTextBox)
        Me.channelParametersGroupBox.Controls.Add(Me.firstEdgeComboBox)
        Me.channelParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.channelParametersGroupBox.Location = New System.Drawing.Point(8, 8)
        Me.channelParametersGroupBox.Name = "channelParametersGroupBox"
        Me.channelParametersGroupBox.Size = New System.Drawing.Size(248, 212)
        Me.channelParametersGroupBox.TabIndex = 0
        Me.channelParametersGroupBox.TabStop = False
        Me.channelParametersGroupBox.Text = "Channel Parameters:"
        ' 
        ' counterComboBox
        ' 
        Me.counterComboBox.Location = New System.Drawing.Point(136, 24)
        Me.counterComboBox.Name = "counterComboBox"
        Me.counterComboBox.Size = New System.Drawing.Size(96, 21)
        Me.counterComboBox.TabIndex = 1
        Me.counterComboBox.Text = "Dev1/ctr0"
        ' 
        ' secondEdgeLabel
        ' 
        Me.secondEdgeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.secondEdgeLabel.Location = New System.Drawing.Point(12, 184)
        Me.secondEdgeLabel.Name = "secondEdgeLabel"
        Me.secondEdgeLabel.Size = New System.Drawing.Size(112, 16)
        Me.secondEdgeLabel.TabIndex = 8
        Me.secondEdgeLabel.Text = "Second Edge:"
        ' 
        ' secondEdgeComboBox
        ' 
        Me.secondEdgeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.secondEdgeComboBox.Items.AddRange(New Object() {"Rising", "Falling"})
        Me.secondEdgeComboBox.Location = New System.Drawing.Point(136, 182)
        Me.secondEdgeComboBox.Name = "secondEdgeComboBox"
        Me.secondEdgeComboBox.Size = New System.Drawing.Size(96, 21)
        Me.secondEdgeComboBox.TabIndex = 9
        ' 
        ' firstEdgeLabel
        ' 
        Me.firstEdgeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.firstEdgeLabel.Location = New System.Drawing.Point(12, 144)
        Me.firstEdgeLabel.Name = "firstEdgeLabel"
        Me.firstEdgeLabel.Size = New System.Drawing.Size(112, 16)
        Me.firstEdgeLabel.TabIndex = 6
        Me.firstEdgeLabel.Text = "First Edge:"
        ' 
        ' maximumLabel
        ' 
        Me.maximumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.maximumLabel.Location = New System.Drawing.Point(12, 105)
        Me.maximumLabel.Name = "maximumLabel"
        Me.maximumLabel.Size = New System.Drawing.Size(120, 16)
        Me.maximumLabel.TabIndex = 4
        Me.maximumLabel.Text = "Maximum Value (sec):"
        ' 
        ' minimumLabel
        ' 
        Me.minimumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.minimumLabel.Location = New System.Drawing.Point(12, 65)
        Me.minimumLabel.Name = "minimumLabel"
        Me.minimumLabel.Size = New System.Drawing.Size(120, 18)
        Me.minimumLabel.TabIndex = 2
        Me.minimumLabel.Text = "Minimum Value (sec):"
        ' 
        ' physicalChannelLabel
        ' 
        Me.physicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.physicalChannelLabel.Location = New System.Drawing.Point(12, 26)
        Me.physicalChannelLabel.Name = "physicalChannelLabel"
        Me.physicalChannelLabel.Size = New System.Drawing.Size(96, 16)
        Me.physicalChannelLabel.TabIndex = 0
        Me.physicalChannelLabel.Text = "Counter:"
        ' 
        ' maximumTextBox
        ' 
        Me.maximumTextBox.Location = New System.Drawing.Point(136, 103)
        Me.maximumTextBox.Name = "maximumTextBox"
        Me.maximumTextBox.Size = New System.Drawing.Size(96, 20)
        Me.maximumTextBox.TabIndex = 5
        Me.maximumTextBox.Text = "0.838860750"
        ' 
        ' minimumTextBox
        ' 
        Me.minimumTextBox.Location = New System.Drawing.Point(136, 64)
        Me.minimumTextBox.Name = "minimumTextBox"
        Me.minimumTextBox.Size = New System.Drawing.Size(96, 20)
        Me.minimumTextBox.TabIndex = 3
        Me.minimumTextBox.Text = "0.000000100"
        ' 
        ' firstEdgeComboBox
        ' 
        Me.firstEdgeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.firstEdgeComboBox.Items.AddRange(New Object() {"Rising", "Falling"})
        Me.firstEdgeComboBox.Location = New System.Drawing.Point(136, 142)
        Me.firstEdgeComboBox.Name = "firstEdgeComboBox"
        Me.firstEdgeComboBox.Size = New System.Drawing.Size(96, 21)
        Me.firstEdgeComboBox.TabIndex = 7
        ' 
        ' acqResultGroupBox
        ' 
        Me.acqResultGroupBox.Controls.Add(Me.acquisitionDataTextBox)
        Me.acqResultGroupBox.Controls.Add(Me.resultLabel)
        Me.acqResultGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.acqResultGroupBox.Location = New System.Drawing.Point(264, 8)
        Me.acqResultGroupBox.Name = "acqResultGroupBox"
        Me.acqResultGroupBox.Size = New System.Drawing.Size(152, 96)
        Me.acqResultGroupBox.TabIndex = 2
        Me.acqResultGroupBox.TabStop = False
        Me.acqResultGroupBox.Text = "Acquisition Results:"
        ' 
        ' acquisitionDataTextBox
        ' 
        Me.acquisitionDataTextBox.Location = New System.Drawing.Point(16, 56)
        Me.acquisitionDataTextBox.Name = "acquisitionDataTextBox"
        Me.acquisitionDataTextBox.ReadOnly = True
        Me.acquisitionDataTextBox.Size = New System.Drawing.Size(120, 20)
        Me.acquisitionDataTextBox.TabIndex = 1
        Me.acquisitionDataTextBox.Text = "0.0"
        ' 
        ' resultLabel
        ' 
        Me.resultLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.resultLabel.Location = New System.Drawing.Point(16, 24)
        Me.resultLabel.Name = "resultLabel"
        Me.resultLabel.Size = New System.Drawing.Size(112, 32)
        Me.resultLabel.TabIndex = 0
        Me.resultLabel.Text = "Measured Two Edge Separation (sec):"
        '
        ' startButton
        '
        Me.startButton.Enabled = False
        Me.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.startButton.Location = New System.Drawing.Point(272, 231)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(128, 32)
        Me.startButton.TabIndex = 3
        Me.startButton.Text = "Start"
        ' 
        ' timingGroupBox
        ' 
        Me.timingGroupBox.Controls.Add(Me.samplesLabel)
        Me.timingGroupBox.Controls.Add(Me.samplesTextBox)
        Me.timingGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.timingGroupBox.Location = New System.Drawing.Point(8, 231)
        Me.timingGroupBox.Name = "timingGroupBox"
        Me.timingGroupBox.Size = New System.Drawing.Size(248, 63)
        Me.timingGroupBox.TabIndex = 1
        Me.timingGroupBox.TabStop = False
        Me.timingGroupBox.Text = "Timing Parameters:"
        ' 
        ' samplesLabel
        ' 
        Me.samplesLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.samplesLabel.Location = New System.Drawing.Point(12, 25)
        Me.samplesLabel.Name = "samplesLabel"
        Me.samplesLabel.Size = New System.Drawing.Size(120, 18)
        Me.samplesLabel.TabIndex = 0
        Me.samplesLabel.Text = "Samples to Read:"
        '
        ' samplesTextBox
        '
        Me.samplesTextBox.Location = New System.Drawing.Point(136, 24)
        Me.samplesTextBox.Name = "samplesTextBox"
        Me.samplesTextBox.Size = New System.Drawing.Size(96, 20)
        Me.samplesTextBox.TabIndex = 1
        Me.samplesTextBox.Text = "1000"
        ' 
        ' stopButton
        ' 
        Me.stopButton.Enabled = False
        Me.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.stopButton.Location = New System.Drawing.Point(272, 269)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(128, 32)
        Me.stopButton.TabIndex = 4
        Me.stopButton.Text = "Stop"
        ' 
        ' MainForm
        ' 
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(426, 310)
        Me.Controls.Add(timingGroupBox)
        Me.Controls.Add(stopButton)
        Me.Controls.Add(startButton)
        Me.Controls.Add(acqResultGroupBox)
        Me.Controls.Add(channelParametersGroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Measure Two Edge Separation - Buffered Continuous"
        Me.channelParametersGroupBox.ResumeLayout(False)
        Me.channelParametersGroupBox.PerformLayout()
        Me.acqResultGroupBox.ResumeLayout(False)
        Me.acqResultGroupBox.PerformLayout()
        Me.timingGroupBox.ResumeLayout(False)
        Me.timingGroupBox.PerformLayout()
        Me.ResumeLayout(False)
    End Sub 'InitializeComponent


    <STAThread()> _
    Public Shared Sub Main()
        Application.EnableVisualStyles()
        Application.DoEvents()
        Application.Run(New MainForm())
    End Sub 'Main


    Private Sub startButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles startButton.Click
        ' This example uses the default source (or gate) terminal for 
        ' the counter of your device.  To determine what the default 
        ' counter pins for your device are or to set a different source 
        ' (or gate) pin, refer to the Connecting Counter Signals topic
        ' in the NI-DAQmx Help (search for "Connecting Counter Signals").
        Try
            ' Determine rising or falling edges
            Select Case firstEdgeComboBox.SelectedItem.ToString()
                Case "Rising"
                    firstEdge = CITwoEdgeSeparationFirstEdge.Rising
                Case "Falling"
                    firstEdge = CITwoEdgeSeparationFirstEdge.Falling
            End Select

            Select Case secondEdgeComboBox.SelectedItem.ToString()
                Case "Rising"
                    secondEdge = CITwoEdgeSeparationSecondEdge.Rising
                Case "Falling"
                    secondEdge = CITwoEdgeSeparationSecondEdge.Falling
            End Select

            ' Create the task
            myTask = New Task()

            ' Create the two edge separation counter channel
            myTask.CIChannels.CreateTwoEdgeSeparationChannel(counterComboBox.Text, "", _
                Convert.ToDouble(minimumTextBox.Text), Convert.ToDouble(maximumTextBox.Text), firstEdge, secondEdge, CITwoEdgeSeparationUnits.Seconds)

            ' Determine the number of samples per read
            numberOfSamples = Convert.ToInt32(samplesTextBox.Text)

            ' Configure the task for continuous implicit sampling
            myTask.Timing.ConfigureImplicit(SampleQuantityMode.ContinuousSamples, numberOfSamples)

            runningTask = myTask

            ' Create the reader
            counterInReader = New CounterReader(myTask.Stream)

            counterInReader.SynchronizeCallbacks = True

            ' Memory Optimized Read method needs an initialized array.
            data = New [Double](numberOfSamples - 1) {}

            ' Create the callback and start the first read
            asyncCB = New AsyncCallback(AddressOf CounterInCallback)
            counterInReader.BeginMemoryOptimizedReadMultiSampleDouble(numberOfSamples, asyncCB, myTask, data)

            ' Disable UI
            startButton.Enabled = False
            stopButton.Enabled = True
            channelParametersGroupBox.Enabled = False
            timingGroupBox.Enabled = False
        Catch exception As DaqException
            MessageBox.Show(exception.Message)
            stopButton_Click(Nothing, Nothing)
        End Try
    End Sub 'startButton_Click


    Private Sub stopButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles stopButton.Click
        ' Dispose task and enable UI
        runningTask = Nothing
        myTask.Dispose()
        startButton.Enabled = True
        stopButton.Enabled = False
        channelParametersGroupBox.Enabled = True
        timingGroupBox.Enabled = True
    End Sub 'stopButton_Click


    Private Sub CounterInCallback(ByVal ar As IAsyncResult)
        Try
            ' Prevent stale events
            If (Not (runningTask Is Nothing)) AndAlso ar.AsyncState.Equals(runningTask) Then
                ' Retrieve the data
                data = counterInReader.EndMemoryOptimizedReadMultiSampleDouble(ar, actualNumberOfSamplesRead)

                ' Display the data
                acquisitionDataTextBox.Text = data(0).ToString()

                ' Start the next read
                counterInReader.BeginMemoryOptimizedReadMultiSampleDouble(numberOfSamples, asyncCB, myTask, data)
            End If
        Catch exception As DaqException
            MessageBox.Show(exception.Message)
            stopButton_Click(Nothing, Nothing)
        End Try
    End Sub 'CounterInCallback
End Class 'MainForm

