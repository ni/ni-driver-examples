'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   MeasDigFreqBuffCont_LargeRange2Ctr
'
' Category:
'   CI
'
' Description:
'   This example demonstrates how to measure buffered frequency using two
'   counters on a counter 
'   input channel. The divisor, maximum and minimum frequency values, and the edge
'   parameter are configurable.This 
'   example shows how to measure frequency on the counter's default input terminal
'   (see I/O Connections Overview 
'   below for more information), but could easily be expanded to measure frequency
'   on any PFI, RTSI, or internal 
'   signal. Additionally, this example could be extended to measure frequency with
'   other measurement methods 
'   for different frequency and quantization error requirements.
'
' Instructions for running:
'   1.  Enter the physical channel which corresponds to the counter you want to
'       use to measure frequency on the DAQ device.
'   2.  Enter the divisor which specifies how many periods of the unkown signal
'       are used to calculate the frequency. Also, enter in the maximum and
'       minimum expected frequency values. This will determine what internal
'       timebase is used.
'   3.  Set the edge parameter which determines if the measurement begins on a
'       rising or falling edge or you signal.Note: It is important to set the
'       maximum and minimum values of your unknown frequency as accurately as
'       possible so the best internal timebase can be chosen to minimize
'       measurement error.  The default values specify a range that can be
'       measured by the counter using the 20MHzTimebase.
'
' Steps:
'   1.  Create a Task.
'   2.  Create a counter input channel using the
'       Task.CIChannel.CreateCIFreqChannel method. The edge 
'       parameter is used to determine if the counter will begin measuring on a
'       rising or falling edge. The divisor 
'       specifies how many periods of the unknow signal are used to calculate the
'       frequency. The higher this 
'       is, the more accurate your measurement will be, it will also take longer
'       to perform each measurement.  
'       It is important to set the maximum and minimum values of your unknown
'       frequency as accurately as possible 
'       so the best internal timebase can be chosen to minimize measurement error.
'       The default values specify 
'       a range that can be measured by the counter using the 20MHzTimebase.
'   3.  Call the Task.Timing.ConfigureImplicit method to configure the sample
'       mode and samples per channel.  For time measurements with counters, the
'       ConfigureImplicit method is used because the signal being measured
'       itself determines the sample rate.
'   4.  Create a CounterReader object, and set its SynchronizingObject property
'       to  the current instance of the form class.  Register an asynchronous
'       callback for the counter operation using
'       CounterReader.BeginReadMultiSampleDouble.
'   5.  The async callback is called everytime the required number of samples
'       has been acquired. Calling CounterReader.EndReadMultiSampleDouble
'       returns the acquired data. Calling the BeginReadMultiSampleDouble method
'       again makes this operation continuously return data.
'   6.  Call the Task.Dispose method to stop the task and de-allocate any
'       resources acquired by the task
'   7.  Handle any DaqExceptions and display any error messages in a Message
'       box.
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
    Private edge As CIFrequencyStartingEdge
    Private runningTask As Task
    Private measureData As Double()
    Private myReader As CounterReader
    Private myCallBack As AsyncCallback
    Private samplesPerRead As Int32
    Private actualNumberOfSamplesRead As Integer

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()
        Application.EnableVisualStyles()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        edge = CIFrequencyStartingEdge.Rising

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
    Friend WithEvents measureFrequencyLabel As System.Windows.Forms.Label
    Friend WithEvents channelParamGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents divisorLabel As System.Windows.Forms.Label
    Friend WithEvents physicalChannelLabel As System.Windows.Forms.Label
    Friend WithEvents minValueTextBox As System.Windows.Forms.TextBox
    Friend WithEvents minimumValueLabel As System.Windows.Forms.Label
    Friend WithEvents maximumValueLabel As System.Windows.Forms.Label
    Friend WithEvents maxValueTextBox As System.Windows.Forms.TextBox
    Friend WithEvents startButton As System.Windows.Forms.Button
    Friend WithEvents edgeGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents fallingRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents risingRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents frequencyListBox As System.Windows.Forms.ListBox
    Friend WithEvents stopButton As System.Windows.Forms.Button
    Friend WithEvents divisorNumericUpDown As System.Windows.Forms.NumericUpDown
    Friend WithEvents dataGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents timingParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents samplesPerReadLabel As System.Windows.Forms.Label
    Friend WithEvents samplesPerReadTextBox As System.Windows.Forms.TextBox
    Friend WithEvents counterComboBox As System.Windows.Forms.ComboBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MainForm))
        Me.startButton = New System.Windows.Forms.Button
        Me.measureFrequencyLabel = New System.Windows.Forms.Label
        Me.channelParamGroupBox = New System.Windows.Forms.GroupBox
        Me.counterComboBox = New System.Windows.Forms.ComboBox
        Me.divisorLabel = New System.Windows.Forms.Label
        Me.divisorNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.physicalChannelLabel = New System.Windows.Forms.Label
        Me.minValueTextBox = New System.Windows.Forms.TextBox
        Me.minimumValueLabel = New System.Windows.Forms.Label
        Me.maximumValueLabel = New System.Windows.Forms.Label
        Me.maxValueTextBox = New System.Windows.Forms.TextBox
        Me.edgeGroupBox = New System.Windows.Forms.GroupBox
        Me.fallingRadioButton = New System.Windows.Forms.RadioButton
        Me.risingRadioButton = New System.Windows.Forms.RadioButton
        Me.frequencyListBox = New System.Windows.Forms.ListBox
        Me.stopButton = New System.Windows.Forms.Button
        Me.dataGroupBox = New System.Windows.Forms.GroupBox
        Me.timingParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.samplesPerReadTextBox = New System.Windows.Forms.TextBox
        Me.samplesPerReadLabel = New System.Windows.Forms.Label
        Me.channelParamGroupBox.SuspendLayout()
        CType(Me.divisorNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.edgeGroupBox.SuspendLayout()
        Me.dataGroupBox.SuspendLayout()
        Me.timingParametersGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'startButton
        '
        Me.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.startButton.Location = New System.Drawing.Point(232, 320)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(128, 32)
        Me.startButton.TabIndex = 0
        Me.startButton.Text = "Start"
        '
        'measureFrequencyLabel
        '
        Me.measureFrequencyLabel.BackColor = System.Drawing.SystemColors.Control
        Me.measureFrequencyLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.measureFrequencyLabel.Location = New System.Drawing.Point(16, 24)
        Me.measureFrequencyLabel.Name = "measureFrequencyLabel"
        Me.measureFrequencyLabel.Size = New System.Drawing.Size(144, 16)
        Me.measureFrequencyLabel.TabIndex = 0
        Me.measureFrequencyLabel.Text = "Measured Frequency (Hz):"
        '
        'channelParamGroupBox
        '
        Me.channelParamGroupBox.Controls.Add(Me.counterComboBox)
        Me.channelParamGroupBox.Controls.Add(Me.divisorLabel)
        Me.channelParamGroupBox.Controls.Add(Me.divisorNumericUpDown)
        Me.channelParamGroupBox.Controls.Add(Me.physicalChannelLabel)
        Me.channelParamGroupBox.Controls.Add(Me.minValueTextBox)
        Me.channelParamGroupBox.Controls.Add(Me.minimumValueLabel)
        Me.channelParamGroupBox.Controls.Add(Me.maximumValueLabel)
        Me.channelParamGroupBox.Controls.Add(Me.maxValueTextBox)
        Me.channelParamGroupBox.Controls.Add(Me.edgeGroupBox)
        Me.channelParamGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.channelParamGroupBox.Location = New System.Drawing.Point(8, 8)
        Me.channelParamGroupBox.Name = "channelParamGroupBox"
        Me.channelParamGroupBox.Size = New System.Drawing.Size(184, 296)
        Me.channelParamGroupBox.TabIndex = 2
        Me.channelParamGroupBox.TabStop = False
        Me.channelParamGroupBox.Text = "Channel Parameters:"
        '
        'counterComboBox
        '
        Me.counterComboBox.Location = New System.Drawing.Point(16, 40)
        Me.counterComboBox.Name = "counterComboBox"
        Me.counterComboBox.Size = New System.Drawing.Size(152, 21)
        Me.counterComboBox.TabIndex = 1
        Me.counterComboBox.Text = "Dev1/ctr0"
        '
        'divisorLabel
        '
        Me.divisorLabel.BackColor = System.Drawing.SystemColors.Control
        Me.divisorLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.divisorLabel.Location = New System.Drawing.Point(16, 240)
        Me.divisorLabel.Name = "divisorLabel"
        Me.divisorLabel.Size = New System.Drawing.Size(100, 16)
        Me.divisorLabel.TabIndex = 7
        Me.divisorLabel.Text = "Divisor:"
        '
        'divisorNumericUpDown
        '
        Me.divisorNumericUpDown.Location = New System.Drawing.Point(16, 256)
        Me.divisorNumericUpDown.Maximum = New Decimal(New Integer() {-1, 0, 0, 0})
        Me.divisorNumericUpDown.Minimum = New Decimal(New Integer() {4, 0, 0, 0})
        Me.divisorNumericUpDown.Name = "divisorNumericUpDown"
        Me.divisorNumericUpDown.Size = New System.Drawing.Size(152, 20)
        Me.divisorNumericUpDown.TabIndex = 8
        Me.divisorNumericUpDown.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'physicalChannelLabel
        '
        Me.physicalChannelLabel.BackColor = System.Drawing.SystemColors.Control
        Me.physicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.physicalChannelLabel.Location = New System.Drawing.Point(16, 24)
        Me.physicalChannelLabel.Name = "physicalChannelLabel"
        Me.physicalChannelLabel.Size = New System.Drawing.Size(100, 16)
        Me.physicalChannelLabel.TabIndex = 0
        Me.physicalChannelLabel.Text = "Counter(s):"
        '
        'minValueTextBox
        '
        Me.minValueTextBox.Location = New System.Drawing.Point(16, 160)
        Me.minValueTextBox.Name = "minValueTextBox"
        Me.minValueTextBox.Size = New System.Drawing.Size(152, 20)
        Me.minValueTextBox.TabIndex = 4
        Me.minValueTextBox.Text = "100000.000000"
        '
        'minimumValueLabel
        '
        Me.minimumValueLabel.BackColor = System.Drawing.SystemColors.Control
        Me.minimumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.minimumValueLabel.Location = New System.Drawing.Point(16, 144)
        Me.minimumValueLabel.Name = "minimumValueLabel"
        Me.minimumValueLabel.Size = New System.Drawing.Size(112, 16)
        Me.minimumValueLabel.TabIndex = 3
        Me.minimumValueLabel.Text = "Minimum value (Hz):"
        '
        'maximumValueLabel
        '
        Me.maximumValueLabel.BackColor = System.Drawing.SystemColors.Control
        Me.maximumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.maximumValueLabel.Location = New System.Drawing.Point(16, 192)
        Me.maximumValueLabel.Name = "maximumValueLabel"
        Me.maximumValueLabel.Size = New System.Drawing.Size(120, 16)
        Me.maximumValueLabel.TabIndex = 5
        Me.maximumValueLabel.Text = "Maximum Value (Hz):"
        '
        'maxValueTextBox
        '
        Me.maxValueTextBox.Location = New System.Drawing.Point(16, 208)
        Me.maxValueTextBox.Name = "maxValueTextBox"
        Me.maxValueTextBox.Size = New System.Drawing.Size(152, 20)
        Me.maxValueTextBox.TabIndex = 6
        Me.maxValueTextBox.Text = "1000000.000000"
        '
        'edgeGroupBox
        '
        Me.edgeGroupBox.Controls.Add(Me.fallingRadioButton)
        Me.edgeGroupBox.Controls.Add(Me.risingRadioButton)
        Me.edgeGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.edgeGroupBox.Location = New System.Drawing.Point(16, 72)
        Me.edgeGroupBox.Name = "edgeGroupBox"
        Me.edgeGroupBox.Size = New System.Drawing.Size(152, 64)
        Me.edgeGroupBox.TabIndex = 2
        Me.edgeGroupBox.TabStop = False
        Me.edgeGroupBox.Text = "Edge:"
        '
        'fallingRadioButton
        '
        Me.fallingRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.fallingRadioButton.Location = New System.Drawing.Point(24, 37)
        Me.fallingRadioButton.Name = "fallingRadioButton"
        Me.fallingRadioButton.Size = New System.Drawing.Size(64, 24)
        Me.fallingRadioButton.TabIndex = 1
        Me.fallingRadioButton.Text = "Falling"
        '
        'risingRadioButton
        '
        Me.risingRadioButton.Checked = True
        Me.risingRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.risingRadioButton.Location = New System.Drawing.Point(24, 16)
        Me.risingRadioButton.Name = "risingRadioButton"
        Me.risingRadioButton.Size = New System.Drawing.Size(64, 24)
        Me.risingRadioButton.TabIndex = 0
        Me.risingRadioButton.TabStop = True
        Me.risingRadioButton.Text = "Rising"
        '
        'frequencyListBox
        '
        Me.frequencyListBox.Location = New System.Drawing.Point(216, 48)
        Me.frequencyListBox.Name = "frequencyListBox"
        Me.frequencyListBox.Size = New System.Drawing.Size(152, 238)
        Me.frequencyListBox.TabIndex = 5
        '
        'stopButton
        '
        Me.stopButton.Enabled = False
        Me.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.stopButton.Location = New System.Drawing.Point(232, 352)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(128, 32)
        Me.stopButton.TabIndex = 1
        Me.stopButton.Text = "Stop"
        '
        'dataGroupBox
        '
        Me.dataGroupBox.Controls.Add(Me.measureFrequencyLabel)
        Me.dataGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.dataGroupBox.Location = New System.Drawing.Point(200, 8)
        Me.dataGroupBox.Name = "dataGroupBox"
        Me.dataGroupBox.Size = New System.Drawing.Size(184, 296)
        Me.dataGroupBox.TabIndex = 4
        Me.dataGroupBox.TabStop = False
        Me.dataGroupBox.Text = "Data:"
        '
        'timingParametersGroupBox
        '
        Me.timingParametersGroupBox.Controls.Add(Me.samplesPerReadTextBox)
        Me.timingParametersGroupBox.Controls.Add(Me.samplesPerReadLabel)
        Me.timingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.timingParametersGroupBox.Location = New System.Drawing.Point(8, 312)
        Me.timingParametersGroupBox.Name = "timingParametersGroupBox"
        Me.timingParametersGroupBox.Size = New System.Drawing.Size(184, 80)
        Me.timingParametersGroupBox.TabIndex = 3
        Me.timingParametersGroupBox.TabStop = False
        Me.timingParametersGroupBox.Text = "Timing Parameters:"
        '
        'samplesPerReadTextBox
        '
        Me.samplesPerReadTextBox.Location = New System.Drawing.Point(16, 40)
        Me.samplesPerReadTextBox.Name = "samplesPerReadTextBox"
        Me.samplesPerReadTextBox.Size = New System.Drawing.Size(152, 20)
        Me.samplesPerReadTextBox.TabIndex = 1
        Me.samplesPerReadTextBox.Text = "1000"
        '
        'samplesPerReadLabel
        '
        Me.samplesPerReadLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.samplesPerReadLabel.Location = New System.Drawing.Point(16, 24)
        Me.samplesPerReadLabel.Name = "samplesPerReadLabel"
        Me.samplesPerReadLabel.Size = New System.Drawing.Size(152, 16)
        Me.samplesPerReadLabel.TabIndex = 0
        Me.samplesPerReadLabel.Text = "Samples Per Read:"
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(394, 400)
        Me.Controls.Add(Me.timingParametersGroupBox)
        Me.Controls.Add(Me.stopButton)
        Me.Controls.Add(Me.frequencyListBox)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.channelParamGroupBox)
        Me.Controls.Add(Me.dataGroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Meas Dig Freq-Buffered-Cont-Large Range 2 Ctr"
        Me.channelParamGroupBox.ResumeLayout(False)
        CType(Me.divisorNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        Me.edgeGroupBox.ResumeLayout(False)
        Me.dataGroupBox.ResumeLayout(False)
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

            myTask.CIChannels.CreateFrequencyChannel(counterComboBox.Text, "", _
                Convert.ToDouble(minValueTextBox.Text), _
                Convert.ToDouble(maxValueTextBox.Text), edge, _
                CIFrequencyMeasurementMethod.LargeRangeTwoCounter, 0.001, _
                CType(divisorNumericUpDown.Value, Long), CIFrequencyUnits.Hertz)

            myTask.Timing.ConfigureImplicit(SampleQuantityMode.ContinuousSamples, 1000)


            runningTask = myTask
            myReader = New CounterReader(myTask.Stream)

            ' Use SynchronizeCallbacks to specify that the object 
            ' marshals callbacks across threads appropriately.
            myReader.SynchronizeCallbacks = True

            myCallBack = New AsyncCallback(AddressOf CounterInCallback)
            samplesPerRead = Convert.ToInt32(samplesPerReadTextBox.Text)

            ' Memory Optimized Read method needs an initialized array.

            measureData = New [Double](samplesPerRead - 1) {}
            myReader.BeginMemoryOptimizedReadMultiSampleDouble(samplesPerRead, myCallBack, myTask, measureData)

            startButton.Enabled = False
            stopButton.Enabled = True

        Catch exception As System.Exception

            myTask.Dispose()
            MessageBox.Show(exception.Message)
            runningTask = Nothing
            startButton.Enabled = True
            stopButton.Enabled = False

        End Try

    End Sub

    Private Sub risingRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles risingRadioButton.CheckedChanged

        edge = CIFrequencyStartingEdge.Rising

    End Sub

    Private Sub fallingRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles fallingRadioButton.CheckedChanged

        edge = CIFrequencyStartingEdge.Falling

    End Sub

    Private Sub stopButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles stopButton.Click

        runningTask = Nothing
        stopButton.Enabled = False
        myTask.Dispose()
        startButton.Enabled = True

    End Sub

    Private Sub CounterInCallback(ByVal ar As IAsyncResult)
        'Reads the returned data and updates the form control
        Try
            If (Not (runningTask Is Nothing)) AndAlso runningTask Is ar.AsyncState Then

                measureData = myReader.EndMemoryOptimizedReadMultiSampleDouble(ar, actualNumberOfSamplesRead)

                'Display only the first 10 points of the data acquired
                Dim samplesToDisplay As Int32 = 10
                If measureData.Length < 10 Then
                    samplesToDisplay = measureData.Length
                End If

                frequencyListBox.BeginUpdate()
                frequencyListBox.Items.Clear()

                For i As Int32 = 0 To samplesToDisplay - 1
                    frequencyListBox.Items.Add(measureData(i))
                Next

                frequencyListBox.EndUpdate()

                myReader.BeginMemoryOptimizedReadMultiSampleDouble(samplesPerRead, myCallBack, myTask, measureData)

            End If

        Catch exception As DaqException

            runningTask = Nothing
            myTask.Dispose()
            stopButton.Enabled = False
            MessageBox.Show(exception.Message)
            startButton.Enabled = True

        End Try

    End Sub

End Class
