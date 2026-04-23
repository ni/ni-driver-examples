'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   MeasDigPeriodsBufCon_HighFrq2Ctr
'
' Category:
'   CI
'
' Description:
'   This example demonstrates how to measure periods using two counters on a
'   counter input channel.  The measurement time, sample mode, and samples per
'   read are configurable.This example shows how to measure period on the
'   counters default input terminal, (see I/O Connections Overview below for
'   more information), , but could easily be expanded to measure periods on any
'   PFI, RTSI, or internal signal.  Additionally, this example could be extended
'   to measure period with other measurement methods for different frequency and
'   quantization error requirements.
'
' Instructions for running:
'   1.  Enter the physical channel which corresponds to the counter you want to
'       use to measure period on the DAQ device.
'   2.  Enter the measurement time to specify how often a period is calculated
'       by counting the number of edges that have passed in the elapsed time. 
'       Additionally, you can change the input terminal where the period is
'       measured using properties on the CIChannel object.Note: Use the
'       GenDigPulseTrain_Continuous example to verify that you are measuring
'       correctly on the DAQ device.
'
' Steps:
'   1.  Create a Task object. Create a CIChannel object for period measurement
'       by using the CreateCIPeriodChannel method. The edge parameter is used to
'       determine if the counter will begin measuring on a rising or falling
'       edge.  The measurement time specifies how often a period is calculated
'       by counting the number of edges that have passed in the elapsed time.
'       Note: The maximum and minimum values are not used when measuring period
'       using the High Frequency 2 Ctr method.
'   2.  Call the ConfigureImplicit method to configure the sample mode.Note: For
'       time measurements with counters, ConfigureImplicit is used because the
'       signal being measured itself determines the sample rate.  This is unlike
'       buffered event counting, where an external sample clock must be used.
'   3.  Create a CounterReader object and use the
'       CounterReader.BeginReadMultiSampleDouble method to read the data and
'       register an asynchronous callback to be called when the requested data
'       is available.
'   4.  Call the CounterReader.EndReadMultiSampleDouble method to get the data
'       in the asynchronous callback. Call BeginReadMultiSampleDouble again in
'       the callback to continue retrieving the data being acquired.
'   5.  Call the Task.Dispose method to stop the acquisition and free resources
'       used by the task.
'   6.  Handle any DaqExceptions and display a message for errors.
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
'   counter specified. The default counter terminal(s) depend on the type of
'   measurement being taken. For more information on the default counter input
'   and output terminals for your device, open the NI-DAQmx Help, and refer to
'   Counter Signal Connections found under the Device Considerations book in the
'   table of contents.
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

    Private myTask As Task
    Private maxValue As Double
    Private minValue As Double
    Private divisor As Long
    Private samplesPerRead As Int32
    Private runningTask As Task
    Private samples As Double()
    Private myCounterReader As CounterReader
    Private edge As CIPeriodStartingEdge
    Private myCallBack As AsyncCallback
    Private actualNumberOfSamplesRead As Integer

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()
        Application.EnableVisualStyles()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        maxValue = 0.838861
        minValue = 0
        divisor = 4
        runningTask = Nothing

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
    Friend WithEvents dataGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents stopButton As System.Windows.Forms.Button
    Friend WithEvents startButton As System.Windows.Forms.Button
    Friend WithEvents channelParamGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents counterLabel As System.Windows.Forms.Label
    Friend WithEvents measurementTimeLabel As System.Windows.Forms.Label
    Friend WithEvents measurementTimeTextBox As System.Windows.Forms.TextBox
    Friend WithEvents periodListBox As System.Windows.Forms.ListBox
    Friend WithEvents edgeGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents fallingRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents risingRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents periodsLabel As System.Windows.Forms.Label
    Friend WithEvents timingParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents samplesPerReadLabel As System.Windows.Forms.Label
    Friend WithEvents samplesPerReadTextBox As System.Windows.Forms.TextBox
    Friend WithEvents counterComboBox As System.Windows.Forms.ComboBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MainForm))
        Me.dataGroupBox = New System.Windows.Forms.GroupBox
        Me.periodListBox = New System.Windows.Forms.ListBox
        Me.periodsLabel = New System.Windows.Forms.Label
        Me.stopButton = New System.Windows.Forms.Button
        Me.startButton = New System.Windows.Forms.Button
        Me.channelParamGroupBox = New System.Windows.Forms.GroupBox
        Me.counterComboBox = New System.Windows.Forms.ComboBox
        Me.edgeGroupBox = New System.Windows.Forms.GroupBox
        Me.fallingRadioButton = New System.Windows.Forms.RadioButton
        Me.risingRadioButton = New System.Windows.Forms.RadioButton
        Me.counterLabel = New System.Windows.Forms.Label
        Me.measurementTimeLabel = New System.Windows.Forms.Label
        Me.measurementTimeTextBox = New System.Windows.Forms.TextBox
        Me.timingParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.samplesPerReadTextBox = New System.Windows.Forms.TextBox
        Me.samplesPerReadLabel = New System.Windows.Forms.Label
        Me.dataGroupBox.SuspendLayout()
        Me.channelParamGroupBox.SuspendLayout()
        Me.edgeGroupBox.SuspendLayout()
        Me.timingParametersGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'dataGroupBox
        '
        Me.dataGroupBox.Controls.Add(Me.periodListBox)
        Me.dataGroupBox.Controls.Add(Me.periodsLabel)
        Me.dataGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.dataGroupBox.Location = New System.Drawing.Point(224, 8)
        Me.dataGroupBox.Name = "dataGroupBox"
        Me.dataGroupBox.Size = New System.Drawing.Size(200, 216)
        Me.dataGroupBox.TabIndex = 4
        Me.dataGroupBox.TabStop = False
        Me.dataGroupBox.Text = "Data:"
        '
        'periodListBox
        '
        Me.periodListBox.Location = New System.Drawing.Point(8, 32)
        Me.periodListBox.Name = "periodListBox"
        Me.periodListBox.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.periodListBox.Size = New System.Drawing.Size(184, 173)
        Me.periodListBox.TabIndex = 1
        '
        'periodsLabel
        '
        Me.periodsLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.periodsLabel.Location = New System.Drawing.Point(8, 16)
        Me.periodsLabel.Name = "periodsLabel"
        Me.periodsLabel.Size = New System.Drawing.Size(88, 16)
        Me.periodsLabel.TabIndex = 0
        Me.periodsLabel.Text = "Periods (sec):"
        '
        'stopButton
        '
        Me.stopButton.Enabled = False
        Me.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.stopButton.Location = New System.Drawing.Point(264, 272)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(128, 32)
        Me.stopButton.TabIndex = 1
        Me.stopButton.Text = "Stop"
        '
        'startButton
        '
        Me.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.startButton.Location = New System.Drawing.Point(264, 240)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(128, 32)
        Me.startButton.TabIndex = 0
        Me.startButton.Text = "Start"
        '
        'channelParamGroupBox
        '
        Me.channelParamGroupBox.Controls.Add(Me.counterComboBox)
        Me.channelParamGroupBox.Controls.Add(Me.edgeGroupBox)
        Me.channelParamGroupBox.Controls.Add(Me.counterLabel)
        Me.channelParamGroupBox.Controls.Add(Me.measurementTimeLabel)
        Me.channelParamGroupBox.Controls.Add(Me.measurementTimeTextBox)
        Me.channelParamGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.channelParamGroupBox.Location = New System.Drawing.Point(8, 8)
        Me.channelParamGroupBox.Name = "channelParamGroupBox"
        Me.channelParamGroupBox.Size = New System.Drawing.Size(208, 216)
        Me.channelParamGroupBox.TabIndex = 2
        Me.channelParamGroupBox.TabStop = False
        Me.channelParamGroupBox.Text = "Channel Parameters:"
        '
        'counterComboBox
        '
        Me.counterComboBox.Location = New System.Drawing.Point(16, 40)
        Me.counterComboBox.Name = "counterComboBox"
        Me.counterComboBox.Size = New System.Drawing.Size(176, 21)
        Me.counterComboBox.TabIndex = 1
        Me.counterComboBox.Text = "Dev1/ctr0"
        '
        'edgeGroupBox
        '
        Me.edgeGroupBox.Controls.Add(Me.fallingRadioButton)
        Me.edgeGroupBox.Controls.Add(Me.risingRadioButton)
        Me.edgeGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.edgeGroupBox.Location = New System.Drawing.Point(16, 72)
        Me.edgeGroupBox.Name = "edgeGroupBox"
        Me.edgeGroupBox.Size = New System.Drawing.Size(176, 72)
        Me.edgeGroupBox.TabIndex = 2
        Me.edgeGroupBox.TabStop = False
        Me.edgeGroupBox.Text = "Edge:"
        '
        'fallingRadioButton
        '
        Me.fallingRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.fallingRadioButton.Location = New System.Drawing.Point(24, 40)
        Me.fallingRadioButton.Name = "fallingRadioButton"
        Me.fallingRadioButton.Size = New System.Drawing.Size(72, 24)
        Me.fallingRadioButton.TabIndex = 1
        Me.fallingRadioButton.Text = "Falling"
        '
        'risingRadioButton
        '
        Me.risingRadioButton.Checked = True
        Me.risingRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.risingRadioButton.Location = New System.Drawing.Point(24, 16)
        Me.risingRadioButton.Name = "risingRadioButton"
        Me.risingRadioButton.Size = New System.Drawing.Size(72, 24)
        Me.risingRadioButton.TabIndex = 0
        Me.risingRadioButton.TabStop = True
        Me.risingRadioButton.Text = "Rising"
        '
        'counterLabel
        '
        Me.counterLabel.BackColor = System.Drawing.SystemColors.Control
        Me.counterLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.counterLabel.Location = New System.Drawing.Point(16, 24)
        Me.counterLabel.Name = "counterLabel"
        Me.counterLabel.Size = New System.Drawing.Size(100, 16)
        Me.counterLabel.TabIndex = 0
        Me.counterLabel.Text = "Counter(s):"
        '
        'measurementTimeLabel
        '
        Me.measurementTimeLabel.BackColor = System.Drawing.SystemColors.Control
        Me.measurementTimeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.measurementTimeLabel.Location = New System.Drawing.Point(16, 160)
        Me.measurementTimeLabel.Name = "measurementTimeLabel"
        Me.measurementTimeLabel.Size = New System.Drawing.Size(136, 16)
        Me.measurementTimeLabel.TabIndex = 3
        Me.measurementTimeLabel.Text = "Measurement Time (sec):"
        '
        'measurementTimeTextBox
        '
        Me.measurementTimeTextBox.Location = New System.Drawing.Point(16, 176)
        Me.measurementTimeTextBox.Name = "measurementTimeTextBox"
        Me.measurementTimeTextBox.Size = New System.Drawing.Size(176, 20)
        Me.measurementTimeTextBox.TabIndex = 4
        Me.measurementTimeTextBox.Text = "0.000100"
        '
        'timingParametersGroupBox
        '
        Me.timingParametersGroupBox.Controls.Add(Me.samplesPerReadTextBox)
        Me.timingParametersGroupBox.Controls.Add(Me.samplesPerReadLabel)
        Me.timingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.timingParametersGroupBox.Location = New System.Drawing.Point(8, 232)
        Me.timingParametersGroupBox.Name = "timingParametersGroupBox"
        Me.timingParametersGroupBox.Size = New System.Drawing.Size(208, 80)
        Me.timingParametersGroupBox.TabIndex = 3
        Me.timingParametersGroupBox.TabStop = False
        Me.timingParametersGroupBox.Text = "Timing Parameters:"
        '
        'samplesPerReadTextBox
        '
        Me.samplesPerReadTextBox.Location = New System.Drawing.Point(16, 40)
        Me.samplesPerReadTextBox.Name = "samplesPerReadTextBox"
        Me.samplesPerReadTextBox.Size = New System.Drawing.Size(176, 20)
        Me.samplesPerReadTextBox.TabIndex = 1
        Me.samplesPerReadTextBox.Text = "1000"
        '
        'samplesPerReadLabel
        '
        Me.samplesPerReadLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.samplesPerReadLabel.Location = New System.Drawing.Point(16, 24)
        Me.samplesPerReadLabel.Name = "samplesPerReadLabel"
        Me.samplesPerReadLabel.Size = New System.Drawing.Size(112, 16)
        Me.samplesPerReadLabel.TabIndex = 0
        Me.samplesPerReadLabel.Text = "Samples Per Read:"
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(434, 320)
        Me.Controls.Add(Me.timingParametersGroupBox)
        Me.Controls.Add(Me.dataGroupBox)
        Me.Controls.Add(Me.stopButton)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.channelParamGroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Meas Dig Periods Buffered Continuous - High Freq 2 Ctr"
        Me.dataGroupBox.ResumeLayout(False)
        Me.channelParamGroupBox.ResumeLayout(False)
        Me.edgeGroupBox.ResumeLayout(False)
        Me.timingParametersGroupBox.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub startButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles startButton.Click

        ' This example uses the default source (or gate) terminal for 
        ' the counter of your device.  To determine what the default 
        ' counter pins for your device are or to set a different source 
        ' (or gate) pin, refer to the Connecting Counter Signals topic
        ' in the NI-DAQmx Help (search for "Connecting Counter Signals").

        Try

            myTask = New Task()

            myTask.CIChannels.CreatePeriodChannel(counterComboBox.Text, "", minValue, _
                maxValue, edge, CIPeriodMeasurementMethod.HighFrequencyTwoCounter, _
                Convert.ToDouble(measurementTimeTextBox.Text), divisor, _
                CIPeriodUnits.Seconds)

            myTask.Timing.ConfigureImplicit(SampleQuantityMode.ContinuousSamples, 1000)

            runningTask = myTask
            myCounterReader = New CounterReader(myTask.Stream)

            ' Use SynchronizeCallbacks to specify that the object 
            ' marshals callbacks across threads appropriately.
            myCounterReader.SynchronizeCallbacks = True

            samplesPerRead = Convert.ToInt32(samplesPerReadTextBox.Text)

            myCallBack = New AsyncCallback(AddressOf CounterInCallback)
            ' Memory Optimized Read method needs an initialized array.
            samples = New [Double](samplesPerRead - 1) {}
            myCounterReader.BeginMemoryOptimizedReadMultiSampleDouble(samplesPerRead, myCallBack, myTask, samples)

            startButton.Enabled = False
            stopButton.Enabled = True

        Catch exception As System.Exception

            myTask.Dispose()
            stopButton.Enabled = False
            runningTask = Nothing
            MessageBox.Show(exception.Message)
            startButton.Enabled = True

        End Try

    End Sub

    Private Sub stopButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles stopButton.Click
        runningTask = Nothing
        stopButton.Enabled = False
        myTask.Dispose()
        startButton.Enabled = True
    End Sub

    Private Sub CounterInCallback(ByVal ar As IAsyncResult)

        Try

            If (Not (runningTask Is Nothing)) AndAlso runningTask Is ar.AsyncState Then

                samples = myCounterReader.EndMemoryOptimizedReadMultiSampleDouble(ar, actualNumberOfSamplesRead)

                periodListBox.BeginUpdate()
                periodListBox.Items.Clear()

                'Display the first 10 data points acquired
                Dim samplesToDisplay As Int32 = 10
                If (samples.Length < 10) Then
                    samplesToDisplay = samples.Length
                End If

                For i As Int32 = 0 To samplesToDisplay - 1
                    periodListBox.Items.Add(samples(i))
                Next

                periodListBox.EndUpdate()

                myCounterReader.BeginMemoryOptimizedReadMultiSampleDouble(samplesPerRead, myCallBack, myTask, samples)

            End If

        Catch exception As DaqException

            myTask.Dispose()
            stopButton.Enabled = False
            runningTask = Nothing
            MessageBox.Show(exception.Message)
            startButton.Enabled = True

        End Try

    End Sub

    Private Sub fallingRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles fallingRadioButton.CheckedChanged

        edge = CIPeriodStartingEdge.Falling

    End Sub

    Private Sub risingRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles risingRadioButton.CheckedChanged

        edge = CIPeriodStartingEdge.Rising

    End Sub

End Class
