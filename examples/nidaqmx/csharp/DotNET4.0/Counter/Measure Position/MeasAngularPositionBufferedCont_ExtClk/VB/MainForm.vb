'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   MeasAngularPositionBufferedCont_ExtClk
'
' Category:
'   CI
'
' Description:
'   This example demonstrates how to measure angular position using a quadrature
'   encoder on a counter input channel.  The decoding type, pulses per
'   revolution, z-index enable, z-index phase, z-index value, and sample clock
'   source are all configurable. Position is measured on the counter's default
'   A, B, and Z input terminals (see I/O Connections Overview below for more
'   information).Note: For buffered position measurement, an external sample
'   clock is necessary to signal when a sample should be inserted into the
'   buffer.  This is set by the sample clock source.
'
' Instructions for running:
'   1.  Enter the physical channel which corresponds to the counter you want to
'       use to    measure position on the DAQ device.
'   2.  Enter the decoding type, pulses per revolution, z-index enable, z-index
'       phase, and z-index value to specify how you want the counter to measure
'       position.  Set the sample clock source.
'   3.  Set the samples to read control.  This will determine how many samples
'       are read each time.Note:  An external sample clock must be used. 
'       Counters do not have an internal sample clock available.  You can use
'       the Gen Dig Pulse Train-Continuous example to generate a pulse train on
'       another counter and connect it to the sample clock source you are using
'       n this example.
'
' Steps:
'   1.  Create a Task, then create a CIChannel using the
'       CreateAngularEncoderChannel method on CIChannels.  The decoding type,
'       pulses per revolution, z-index enable, z-index phase, and z-index value
'       parameters are used to determine how the counter should measure
'       position.
'   2.  Use the ConfigureSampleClock of Task.Timing to configure the external
'       sample clock timing parameters such as sample mode and sample clock
'       source.  The sample clock source determines when a sample will be
'       inserted into the buffer.  The 100kHz, 20MHz, and 80MHz timebases cannot
'       be used as the sample clock source. The edge parameter can be used to
'       determine when a sample is taken.
'   3.  Create a CounterReader and use BeginReadMultiSampleDouble() to initiate
'       an asynchronous read.  The counter will be preloaded with the initial
'       angle when the task is started by the initial read.
'   4.  For continuous measurements, the counter will continually read all
'       available data until the Stop button is pressed.
'   5.  Call Task.Dispose to stop the task and de-allocate the resources
'       allocated by the task.
'   6.  Handle any DaqExceptions and display any error messages.
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
    Private counterInReader As CounterReader
    Private asyncCB As AsyncCallback
    Private encoderType As CIEncoderDecodingType
    Private encoderPhase As CIEncoderZIndexPhase
    Private zIndexEnable As Boolean
    Private runningTask As Task
    Private numberOfSamples As Integer
    Private data() As Double
    Private actualNumberOfSamplesRead As Integer

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()
        Application.EnableVisualStyles()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        zIndexPhaseComboBox.SelectedIndex = 0
        decodingTypeComboBox.SelectedIndex = 0
        asyncCB = New AsyncCallback(AddressOf CounterInCallback)

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
    Friend WithEvents stopButton As System.Windows.Forms.Button
    Friend WithEvents startButton As System.Windows.Forms.Button
    Friend WithEvents dataGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents dataListBox As System.Windows.Forms.ListBox
    Friend WithEvents channelParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents zIndexPhaseLabel As System.Windows.Forms.Label
    Friend WithEvents zIndexPhaseComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents pulsesPerRevLabel As System.Windows.Forms.Label
    Friend WithEvents physicalChannelLabel As System.Windows.Forms.Label
    Friend WithEvents pulsePerRevTextBox As System.Windows.Forms.TextBox
    Friend WithEvents zIndexValueTextBox As System.Windows.Forms.TextBox
    Friend WithEvents sampleClkSourceLabel As System.Windows.Forms.Label
    Friend WithEvents sampleClkSourceTextBox As System.Windows.Forms.TextBox
    Friend WithEvents samplesToReadTextBox As System.Windows.Forms.TextBox
    Friend WithEvents samplesToReadLabel As System.Windows.Forms.Label
    Friend WithEvents rateLabel As System.Windows.Forms.Label
    Friend WithEvents rateTextBox As System.Windows.Forms.TextBox
    Friend WithEvents zIndexEnabledCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents decodingTypeLabel As System.Windows.Forms.Label
    Friend WithEvents decodingTypeComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents zIndexValueLabel As System.Windows.Forms.Label
    Friend WithEvents timingParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents counterComboBox As System.Windows.Forms.ComboBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MainForm))
        Me.stopButton = New System.Windows.Forms.Button
        Me.startButton = New System.Windows.Forms.Button
        Me.dataGroupBox = New System.Windows.Forms.GroupBox
        Me.dataListBox = New System.Windows.Forms.ListBox
        Me.channelParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.counterComboBox = New System.Windows.Forms.ComboBox
        Me.zIndexPhaseLabel = New System.Windows.Forms.Label
        Me.zIndexPhaseComboBox = New System.Windows.Forms.ComboBox
        Me.decodingTypeLabel = New System.Windows.Forms.Label
        Me.pulsesPerRevLabel = New System.Windows.Forms.Label
        Me.zIndexValueLabel = New System.Windows.Forms.Label
        Me.physicalChannelLabel = New System.Windows.Forms.Label
        Me.pulsePerRevTextBox = New System.Windows.Forms.TextBox
        Me.zIndexValueTextBox = New System.Windows.Forms.TextBox
        Me.decodingTypeComboBox = New System.Windows.Forms.ComboBox
        Me.zIndexEnabledCheckBox = New System.Windows.Forms.CheckBox
        Me.timingParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.sampleClkSourceLabel = New System.Windows.Forms.Label
        Me.sampleClkSourceTextBox = New System.Windows.Forms.TextBox
        Me.samplesToReadTextBox = New System.Windows.Forms.TextBox
        Me.samplesToReadLabel = New System.Windows.Forms.Label
        Me.rateLabel = New System.Windows.Forms.Label
        Me.rateTextBox = New System.Windows.Forms.TextBox
        Me.dataGroupBox.SuspendLayout()
        Me.channelParametersGroupBox.SuspendLayout()
        Me.timingParametersGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'stopButton
        '
        Me.stopButton.Enabled = False
        Me.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.stopButton.Location = New System.Drawing.Point(312, 296)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(100, 32)
        Me.stopButton.TabIndex = 1
        Me.stopButton.Text = "Stop"
        '
        'startButton
        '
        Me.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.startButton.Location = New System.Drawing.Point(312, 264)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(100, 32)
        Me.startButton.TabIndex = 0
        Me.startButton.Text = "Start"
        '
        'dataGroupBox
        '
        Me.dataGroupBox.Controls.Add(Me.dataListBox)
        Me.dataGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.dataGroupBox.Location = New System.Drawing.Point(288, 8)
        Me.dataGroupBox.Name = "dataGroupBox"
        Me.dataGroupBox.Size = New System.Drawing.Size(136, 224)
        Me.dataGroupBox.TabIndex = 4
        Me.dataGroupBox.TabStop = False
        Me.dataGroupBox.Text = "Data"
        '
        'dataListBox
        '
        Me.dataListBox.Location = New System.Drawing.Point(8, 16)
        Me.dataListBox.Name = "dataListBox"
        Me.dataListBox.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.dataListBox.Size = New System.Drawing.Size(120, 199)
        Me.dataListBox.TabIndex = 0
        '
        'channelParametersGroupBox
        '
        Me.channelParametersGroupBox.Controls.Add(Me.counterComboBox)
        Me.channelParametersGroupBox.Controls.Add(Me.zIndexPhaseLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.zIndexPhaseComboBox)
        Me.channelParametersGroupBox.Controls.Add(Me.decodingTypeLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.pulsesPerRevLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.zIndexValueLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.physicalChannelLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.pulsePerRevTextBox)
        Me.channelParametersGroupBox.Controls.Add(Me.zIndexValueTextBox)
        Me.channelParametersGroupBox.Controls.Add(Me.decodingTypeComboBox)
        Me.channelParametersGroupBox.Controls.Add(Me.zIndexEnabledCheckBox)
        Me.channelParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.channelParametersGroupBox.Location = New System.Drawing.Point(8, 8)
        Me.channelParametersGroupBox.Name = "channelParametersGroupBox"
        Me.channelParametersGroupBox.Size = New System.Drawing.Size(272, 224)
        Me.channelParametersGroupBox.TabIndex = 2
        Me.channelParametersGroupBox.TabStop = False
        Me.channelParametersGroupBox.Text = "Channel Parameters"
        '
        'counterComboBox
        '
        Me.counterComboBox.Location = New System.Drawing.Point(136, 24)
        Me.counterComboBox.Name = "counterComboBox"
        Me.counterComboBox.Size = New System.Drawing.Size(121, 21)
        Me.counterComboBox.TabIndex = 1
        Me.counterComboBox.Text = "Dev1/ctr0"
        '
        'zIndexPhaseLabel
        '
        Me.zIndexPhaseLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.zIndexPhaseLabel.Location = New System.Drawing.Point(12, 160)
        Me.zIndexPhaseLabel.Name = "zIndexPhaseLabel"
        Me.zIndexPhaseLabel.Size = New System.Drawing.Size(92, 16)
        Me.zIndexPhaseLabel.TabIndex = 7
        Me.zIndexPhaseLabel.Text = "Z Index Phase:"
        '
        'zIndexPhaseComboBox
        '
        Me.zIndexPhaseComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.zIndexPhaseComboBox.Items.AddRange(New Object() {"A High B High", "A High B Low", "A Low B High", "A Low B Low"})
        Me.zIndexPhaseComboBox.Location = New System.Drawing.Point(136, 160)
        Me.zIndexPhaseComboBox.Name = "zIndexPhaseComboBox"
        Me.zIndexPhaseComboBox.Size = New System.Drawing.Size(120, 21)
        Me.zIndexPhaseComboBox.TabIndex = 8
        '
        'decodingTypeLabel
        '
        Me.decodingTypeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.decodingTypeLabel.Location = New System.Drawing.Point(12, 96)
        Me.decodingTypeLabel.Name = "decodingTypeLabel"
        Me.decodingTypeLabel.Size = New System.Drawing.Size(112, 16)
        Me.decodingTypeLabel.TabIndex = 3
        Me.decodingTypeLabel.Text = "Decoding Type:"
        '
        'pulsesPerRevLabel
        '
        Me.pulsesPerRevLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.pulsesPerRevLabel.Location = New System.Drawing.Point(12, 192)
        Me.pulsesPerRevLabel.Name = "pulsesPerRevLabel"
        Me.pulsesPerRevLabel.Size = New System.Drawing.Size(120, 16)
        Me.pulsesPerRevLabel.TabIndex = 9
        Me.pulsesPerRevLabel.Text = "Pulses per Revolution:"
        '
        'zIndexValueLabel
        '
        Me.zIndexValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.zIndexValueLabel.Location = New System.Drawing.Point(12, 128)
        Me.zIndexValueLabel.Name = "zIndexValueLabel"
        Me.zIndexValueLabel.Size = New System.Drawing.Size(120, 18)
        Me.zIndexValueLabel.TabIndex = 5
        Me.zIndexValueLabel.Text = "Z Index Value:"
        '
        'physicalChannelLabel
        '
        Me.physicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.physicalChannelLabel.Location = New System.Drawing.Point(12, 26)
        Me.physicalChannelLabel.Name = "physicalChannelLabel"
        Me.physicalChannelLabel.Size = New System.Drawing.Size(96, 16)
        Me.physicalChannelLabel.TabIndex = 0
        Me.physicalChannelLabel.Text = "Counter(s):"
        '
        'pulsePerRevTextBox
        '
        Me.pulsePerRevTextBox.Location = New System.Drawing.Point(136, 192)
        Me.pulsePerRevTextBox.Name = "pulsePerRevTextBox"
        Me.pulsePerRevTextBox.Size = New System.Drawing.Size(120, 20)
        Me.pulsePerRevTextBox.TabIndex = 10
        Me.pulsePerRevTextBox.Text = "24"
        '
        'zIndexValueTextBox
        '
        Me.zIndexValueTextBox.Location = New System.Drawing.Point(136, 128)
        Me.zIndexValueTextBox.Name = "zIndexValueTextBox"
        Me.zIndexValueTextBox.Size = New System.Drawing.Size(120, 20)
        Me.zIndexValueTextBox.TabIndex = 6
        Me.zIndexValueTextBox.Text = "0"
        '
        'decodingTypeComboBox
        '
        Me.decodingTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.decodingTypeComboBox.Items.AddRange(New Object() {"X1", "X2", "X4"})
        Me.decodingTypeComboBox.Location = New System.Drawing.Point(136, 96)
        Me.decodingTypeComboBox.Name = "decodingTypeComboBox"
        Me.decodingTypeComboBox.Size = New System.Drawing.Size(120, 21)
        Me.decodingTypeComboBox.TabIndex = 4
        '
        'zIndexEnabledCheckBox
        '
        Me.zIndexEnabledCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.zIndexEnabledCheckBox.Location = New System.Drawing.Point(136, 56)
        Me.zIndexEnabledCheckBox.Name = "zIndexEnabledCheckBox"
        Me.zIndexEnabledCheckBox.Size = New System.Drawing.Size(120, 24)
        Me.zIndexEnabledCheckBox.TabIndex = 2
        Me.zIndexEnabledCheckBox.Text = "Z Index Enabled"
        '
        'timingParametersGroupBox
        '
        Me.timingParametersGroupBox.Controls.Add(Me.sampleClkSourceLabel)
        Me.timingParametersGroupBox.Controls.Add(Me.sampleClkSourceTextBox)
        Me.timingParametersGroupBox.Controls.Add(Me.samplesToReadTextBox)
        Me.timingParametersGroupBox.Controls.Add(Me.samplesToReadLabel)
        Me.timingParametersGroupBox.Controls.Add(Me.rateLabel)
        Me.timingParametersGroupBox.Controls.Add(Me.rateTextBox)
        Me.timingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.timingParametersGroupBox.Location = New System.Drawing.Point(8, 240)
        Me.timingParametersGroupBox.Name = "timingParametersGroupBox"
        Me.timingParametersGroupBox.Size = New System.Drawing.Size(272, 112)
        Me.timingParametersGroupBox.TabIndex = 3
        Me.timingParametersGroupBox.TabStop = False
        Me.timingParametersGroupBox.Text = "Timing Parameters"
        '
        'sampleClkSourceLabel
        '
        Me.sampleClkSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.sampleClkSourceLabel.Location = New System.Drawing.Point(12, 80)
        Me.sampleClkSourceLabel.Name = "sampleClkSourceLabel"
        Me.sampleClkSourceLabel.Size = New System.Drawing.Size(120, 16)
        Me.sampleClkSourceLabel.TabIndex = 4
        Me.sampleClkSourceLabel.Text = "Sample Clock Source:"
        '
        'sampleClkSourceTextBox
        '
        Me.sampleClkSourceTextBox.Location = New System.Drawing.Point(136, 80)
        Me.sampleClkSourceTextBox.Name = "sampleClkSourceTextBox"
        Me.sampleClkSourceTextBox.Size = New System.Drawing.Size(120, 20)
        Me.sampleClkSourceTextBox.TabIndex = 5
        Me.sampleClkSourceTextBox.Text = "/Dev1/PFI9"
        '
        'samplesToReadTextBox
        '
        Me.samplesToReadTextBox.Location = New System.Drawing.Point(136, 48)
        Me.samplesToReadTextBox.Name = "samplesToReadTextBox"
        Me.samplesToReadTextBox.Size = New System.Drawing.Size(120, 20)
        Me.samplesToReadTextBox.TabIndex = 3
        Me.samplesToReadTextBox.Text = "1000"
        '
        'samplesToReadLabel
        '
        Me.samplesToReadLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.samplesToReadLabel.Location = New System.Drawing.Point(12, 52)
        Me.samplesToReadLabel.Name = "samplesToReadLabel"
        Me.samplesToReadLabel.Size = New System.Drawing.Size(98, 16)
        Me.samplesToReadLabel.TabIndex = 2
        Me.samplesToReadLabel.Text = "Samples to Read:"
        '
        'rateLabel
        '
        Me.rateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.rateLabel.Location = New System.Drawing.Point(12, 24)
        Me.rateLabel.Name = "rateLabel"
        Me.rateLabel.Size = New System.Drawing.Size(56, 16)
        Me.rateLabel.TabIndex = 0
        Me.rateLabel.Text = "Rate:"
        '
        'rateTextBox
        '
        Me.rateTextBox.Location = New System.Drawing.Point(136, 16)
        Me.rateTextBox.Name = "rateTextBox"
        Me.rateTextBox.Size = New System.Drawing.Size(120, 20)
        Me.rateTextBox.TabIndex = 1
        Me.rateTextBox.Text = "1000.00"
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(434, 360)
        Me.Controls.Add(Me.timingParametersGroupBox)
        Me.Controls.Add(Me.stopButton)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.dataGroupBox)
        Me.Controls.Add(Me.channelParametersGroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Measure Angular Position  Buffered Continuous - External Clock"
        Me.dataGroupBox.ResumeLayout(False)
        Me.channelParametersGroupBox.ResumeLayout(False)
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

            Dim zIndexValue As Double = Convert.ToDouble(zIndexValueTextBox.Text)
            Dim pulsePerRev As Integer = Convert.ToInt32(pulsePerRevTextBox.Text)
            numberOfSamples = Convert.ToInt32(samplesToReadTextBox.Text)
            zIndexEnable = zIndexEnabledCheckBox.Checked

            Select Case decodingTypeComboBox.SelectedIndex

                Case 0 'X1
                    encoderType = CIEncoderDecodingType.X1
                Case 1 'X2
                    encoderType = CIEncoderDecodingType.X2
                Case 2 'X4
                    encoderType = CIEncoderDecodingType.X4
            End Select

            Select Case decodingTypeComboBox.SelectedIndex

                Case 0 'A High B High
                    encoderPhase = CIEncoderZIndexPhase.AHighBHigh
                Case 1 'A High B Low
                    encoderPhase = CIEncoderZIndexPhase.AHighBLow
                Case 2 'A Low B High
                    encoderPhase = CIEncoderZIndexPhase.ALowBHigh
                Case 3 'A Low B Low
                    encoderPhase = CIEncoderZIndexPhase.ALowBLow
            End Select

            myTask.CIChannels.CreateAngularEncoderChannel(counterComboBox.Text, _
                "", encoderType, zIndexEnable, zIndexValue, encoderPhase, pulsePerRev, _
                0.0, CIAngularEncoderUnits.Degrees)

            myTask.Timing.ConfigureSampleClock(sampleClkSourceTextBox.Text, _
                Convert.ToDouble(rateTextBox.Text), SampleClockActiveEdge.Rising, _
                SampleQuantityMode.ContinuousSamples, 1000)

            runningTask = myTask

            counterInReader = New CounterReader(myTask.Stream)

            ' Use SynchronizeCallbacks to specify that the object 
            ' marshals callbacks across threads appropriately.
            counterInReader.SynchronizeCallbacks = True
            ' Memory Optimized Read method needs an initialized array.
            data = New [Double](numberOfSamples - 1) {}
            counterInReader.BeginMemoryOptimizedReadMultiSampleDouble(numberOfSamples, asyncCB, myTask, data)

            startButton.Enabled = False
            stopButton.Enabled = True

        Catch exception As DaqException

            myTask.Dispose()
            runningTask = Nothing
            MessageBox.Show(exception.Message)

        End Try
    End Sub

    Private Sub stopButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles stopButton.Click
        myTask.Dispose()
        runningTask = Nothing
        startButton.Enabled = True
        stopButton.Enabled = False
    End Sub
    Private Sub CounterInCallback(ByVal ar As IAsyncResult)

        Try

            If (Not (runningTask Is Nothing)) AndAlso runningTask Is ar.AsyncState Then

                Dim samplesToDisplay As Integer

                data = counterInReader.EndMemoryOptimizedReadMultiSampleDouble(ar, actualNumberOfSamplesRead)

                'Display only the first 10 data points in the listbox. 
                If (data.Length < 10) Then
                    samplesToDisplay = data.Length
                Else
                    samplesToDisplay = 10
                End If

                dataListBox.BeginUpdate()
                dataListBox.Items.Clear()

                For i As Integer = 0 To samplesToDisplay
                    dataListBox.Items.Add(data(i))
                Next

                dataListBox.EndUpdate()
                counterInReader.BeginMemoryOptimizedReadMultiSampleDouble(numberOfSamples, asyncCB, myTask, data)
            End If

        Catch exception As DaqException

            MessageBox.Show(exception.Message)
            stopButton_Click(Nothing, Nothing)

        End Try

    End Sub

End Class
