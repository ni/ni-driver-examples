'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   MeasDigFrequency_LowFreq1Ctr
'
' Category:
'   CI
'
' Description:
'   This example demonstrates how to measure a frequency using one counter on a
'   counter input channel.  The starting edge, minimum value and maximum value
'   are all configurable. This example shows how to measure frequency on the
'   counter's default input terminal (see I/O Connections Overview below for
'   more information), but could easily be expanded to measure frequency on any
'   PFI, RTSI, or internal signal.  Additionally, this example could be extended
'   to measure frequency with two counters for different frequency and
'   quantization error requirements.
'
' Instructions for running:
'   1.  Enter the physical channel which corresponds to the counter you want to
'       use to measure the frequency on the DAQ device.
'   2.  Enter the measurement edge to specify the edge on which you want the
'       counter to start measuring.  Enter the maximum and minimum value to
'       specify the expected range of your unknown frequency. Note:  It is
'       important to set the maximum and minimum values of your unknown
'       frequency as accurately as possible so the best internal timebase can be
'       chosen to minimize measurement error.  The default values specify a
'       range that can be measured by the counter using the 20MHzTimebase. Use
'       the GenDigPulseTrain_Continuous example to verify that you are measuring
'       correctly on the DAQ device.
'
' Steps:
'   1.  Create a Task.
'   2.  Create a CIChannel by using the CreateFrequencyChannel method to measure
'       frequency.  The edge parameter is used to determine if the counter  will
'       begin measuring on a rising or falling edge.  It is important to set the
'       maximum and minimum values of your unknown signal as accurately as
'       possible so the best internal timebase can be chosen to minimize
'       measurement error.  The default values specify the range that can be
'       measured by the counter using the 20MHzTimebase.
'   3.  Create a CounterReader object and use the ReadSingleSampleDouble method
'       to read the data. The timeout is set by default to 10 seconds.
'   4.  Handle any DaqExceptions and display any error messages in a Message
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
    Private edges As CIFrequencyStartingEdge = CIFrequencyStartingEdge.Rising
    Private myTask As Task
    Private measureFrequency As Double
    Private myCounterReader As CounterReader
    Private myCallBack As AsyncCallback

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()
        Application.EnableVisualStyles()

        'This call is required by the Windows Form Designer.
        InitializeComponent()
        'Add any initialization after the InitializeComponent() call
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
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents measureFrequencyButton As System.Windows.Forms.Button
    Friend WithEvents measuredFrequencyTextBox As System.Windows.Forms.TextBox
    Friend WithEvents measuredFrequencyLabel As System.Windows.Forms.Label
    Friend WithEvents channelParamGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents physicalChannelLabel As System.Windows.Forms.Label
    Friend WithEvents minValueTextBox As System.Windows.Forms.TextBox
    Friend WithEvents minimumValueLabel As System.Windows.Forms.Label
    Friend WithEvents maximumValueLabel As System.Windows.Forms.Label
    Friend WithEvents maxValueTextBox As System.Windows.Forms.TextBox
    Friend WithEvents startEdgeGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents fallingRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents risingRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents counterComboBox As System.Windows.Forms.ComboBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MainForm))
        Me.measureFrequencyButton = New System.Windows.Forms.Button
        Me.measuredFrequencyTextBox = New System.Windows.Forms.TextBox
        Me.measuredFrequencyLabel = New System.Windows.Forms.Label
        Me.channelParamGroupBox = New System.Windows.Forms.GroupBox
        Me.counterComboBox = New System.Windows.Forms.ComboBox
        Me.startEdgeGroupBox = New System.Windows.Forms.GroupBox
        Me.fallingRadioButton = New System.Windows.Forms.RadioButton
        Me.risingRadioButton = New System.Windows.Forms.RadioButton
        Me.physicalChannelLabel = New System.Windows.Forms.Label
        Me.minValueTextBox = New System.Windows.Forms.TextBox
        Me.minimumValueLabel = New System.Windows.Forms.Label
        Me.maximumValueLabel = New System.Windows.Forms.Label
        Me.maxValueTextBox = New System.Windows.Forms.TextBox
        Me.channelParamGroupBox.SuspendLayout()
        Me.startEdgeGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'measureFrequencyButton
        '
        Me.measureFrequencyButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.measureFrequencyButton.Location = New System.Drawing.Point(176, 216)
        Me.measureFrequencyButton.Name = "measureFrequencyButton"
        Me.measureFrequencyButton.Size = New System.Drawing.Size(120, 40)
        Me.measureFrequencyButton.TabIndex = 0
        Me.measureFrequencyButton.Text = "Measure Frequency"
        '
        'measuredFrequencyTextBox
        '
        Me.measuredFrequencyTextBox.Location = New System.Drawing.Point(162, 42)
        Me.measuredFrequencyTextBox.Name = "measuredFrequencyTextBox"
        Me.measuredFrequencyTextBox.ReadOnly = True
        Me.measuredFrequencyTextBox.Size = New System.Drawing.Size(142, 20)
        Me.measuredFrequencyTextBox.TabIndex = 3
        Me.measuredFrequencyTextBox.Text = "0.0"
        '
        'measuredFrequencyLabel
        '
        Me.measuredFrequencyLabel.BackColor = System.Drawing.SystemColors.Control
        Me.measuredFrequencyLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.measuredFrequencyLabel.Location = New System.Drawing.Point(160, 24)
        Me.measuredFrequencyLabel.Name = "measuredFrequencyLabel"
        Me.measuredFrequencyLabel.Size = New System.Drawing.Size(144, 16)
        Me.measuredFrequencyLabel.TabIndex = 2
        Me.measuredFrequencyLabel.Text = "Measured Frequency (Hz):"
        '
        'channelParamGroupBox
        '
        Me.channelParamGroupBox.Controls.Add(Me.counterComboBox)
        Me.channelParamGroupBox.Controls.Add(Me.startEdgeGroupBox)
        Me.channelParamGroupBox.Controls.Add(Me.physicalChannelLabel)
        Me.channelParamGroupBox.Controls.Add(Me.minValueTextBox)
        Me.channelParamGroupBox.Controls.Add(Me.minimumValueLabel)
        Me.channelParamGroupBox.Controls.Add(Me.maximumValueLabel)
        Me.channelParamGroupBox.Controls.Add(Me.maxValueTextBox)
        Me.channelParamGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.channelParamGroupBox.Location = New System.Drawing.Point(8, 8)
        Me.channelParamGroupBox.Name = "channelParamGroupBox"
        Me.channelParamGroupBox.Size = New System.Drawing.Size(136, 266)
        Me.channelParamGroupBox.TabIndex = 1
        Me.channelParamGroupBox.TabStop = False
        Me.channelParamGroupBox.Text = "Channel Parameters"
        '
        'counterComboBox
        '
        Me.counterComboBox.Location = New System.Drawing.Point(16, 40)
        Me.counterComboBox.Name = "counterComboBox"
        Me.counterComboBox.Size = New System.Drawing.Size(104, 21)
        Me.counterComboBox.TabIndex = 1
        Me.counterComboBox.Text = "Dev1/ctr0"
        '
        'startEdgeGroupBox
        '
        Me.startEdgeGroupBox.Controls.Add(Me.fallingRadioButton)
        Me.startEdgeGroupBox.Controls.Add(Me.risingRadioButton)
        Me.startEdgeGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.startEdgeGroupBox.Location = New System.Drawing.Point(16, 176)
        Me.startEdgeGroupBox.Name = "startEdgeGroupBox"
        Me.startEdgeGroupBox.Size = New System.Drawing.Size(104, 72)
        Me.startEdgeGroupBox.TabIndex = 6
        Me.startEdgeGroupBox.TabStop = False
        Me.startEdgeGroupBox.Text = "Starting Edge"
        '
        'fallingRadioButton
        '
        Me.fallingRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.fallingRadioButton.Location = New System.Drawing.Point(17, 38)
        Me.fallingRadioButton.Name = "fallingRadioButton"
        Me.fallingRadioButton.Size = New System.Drawing.Size(77, 24)
        Me.fallingRadioButton.TabIndex = 1
        Me.fallingRadioButton.Text = "Falling"
        '
        'risingRadioButton
        '
        Me.risingRadioButton.Checked = True
        Me.risingRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.risingRadioButton.Location = New System.Drawing.Point(17, 19)
        Me.risingRadioButton.Name = "risingRadioButton"
        Me.risingRadioButton.Size = New System.Drawing.Size(77, 24)
        Me.risingRadioButton.TabIndex = 0
        Me.risingRadioButton.TabStop = True
        Me.risingRadioButton.Text = "Rising"
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
        Me.minValueTextBox.Location = New System.Drawing.Point(16, 88)
        Me.minValueTextBox.Name = "minValueTextBox"
        Me.minValueTextBox.Size = New System.Drawing.Size(104, 20)
        Me.minValueTextBox.TabIndex = 3
        Me.minValueTextBox.Text = "1.192093"
        '
        'minimumValueLabel
        '
        Me.minimumValueLabel.BackColor = System.Drawing.SystemColors.Control
        Me.minimumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.minimumValueLabel.Location = New System.Drawing.Point(16, 72)
        Me.minimumValueLabel.Name = "minimumValueLabel"
        Me.minimumValueLabel.Size = New System.Drawing.Size(120, 16)
        Me.minimumValueLabel.TabIndex = 2
        Me.minimumValueLabel.Text = "Minimum value (Hz):"
        '
        'maximumValueLabel
        '
        Me.maximumValueLabel.BackColor = System.Drawing.SystemColors.Control
        Me.maximumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.maximumValueLabel.Location = New System.Drawing.Point(16, 120)
        Me.maximumValueLabel.Name = "maximumValueLabel"
        Me.maximumValueLabel.Size = New System.Drawing.Size(120, 16)
        Me.maximumValueLabel.TabIndex = 4
        Me.maximumValueLabel.Text = "Maximum Value (Hz):"
        '
        'maxValueTextBox
        '
        Me.maxValueTextBox.Location = New System.Drawing.Point(16, 136)
        Me.maxValueTextBox.Name = "maxValueTextBox"
        Me.maxValueTextBox.Size = New System.Drawing.Size(104, 20)
        Me.maxValueTextBox.TabIndex = 5
        Me.maxValueTextBox.Text = "10000000.0"
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(322, 280)
        Me.Controls.Add(Me.measureFrequencyButton)
        Me.Controls.Add(Me.measuredFrequencyTextBox)
        Me.Controls.Add(Me.measuredFrequencyLabel)
        Me.Controls.Add(Me.channelParamGroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimumSize = New System.Drawing.Size(328, 312)
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Meas Dig Frequency-Low Freq 1 Ctr"
        Me.channelParamGroupBox.ResumeLayout(False)
        Me.startEdgeGroupBox.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub measureFrequencyButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles measureFrequencyButton.Click

        ' This example uses the default source (or gate) terminal for 
        ' the counter of your device.  To determine what the default 
        ' counter pins for your device are or to set a different source 
        ' (or gate) pin, refer to the Connecting Counter Signals topic
        ' in the NI-DAQmx Help (search for "Connecting Counter Signals").

        measureFrequencyButton.Enabled = False

        Try
            myTask = New Task()

            myTask.CIChannels.CreateFrequencyChannel(counterComboBox.Text, "", _
                Convert.ToDouble(minValueTextBox.Text), _
                Convert.ToDouble(maxValueTextBox.Text), edges, _
                CIFrequencyMeasurementMethod.LowFrequencyOneCounter, 0.001, 4, _
                CIFrequencyUnits.Hertz)

            myCounterReader = New CounterReader(myTask.Stream)

            ' Use SynchronizeCallbacks to specify that the object 
            ' marshals callbacks across threads appropriately.
            myCounterReader.SynchronizeCallbacks = True

            myCallBack = New AsyncCallback(AddressOf CounterInCallback)
            myCounterReader.BeginReadSingleSampleDouble(myCallBack, Nothing)

        Catch exception As System.Exception

            MessageBox.Show(exception.Message)
            myTask.Dispose()
            measureFrequencyButton.Enabled = True

        End Try

    End Sub

    Private Sub risingRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles risingRadioButton.CheckedChanged
        edges = CIFrequencyStartingEdge.Rising
    End Sub

    Private Sub fallingRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles fallingRadioButton.CheckedChanged
        edges = CIFrequencyStartingEdge.Falling
    End Sub

    Private Sub CounterInCallback(ByVal ar As IAsyncResult)

        'Read the measured value
        Try

            measureFrequency = myCounterReader.EndReadSingleSampleDouble(ar)
            measuredFrequencyTextBox.Text = measureFrequency.ToString()

        Catch exception As DaqException

            MessageBox.Show(exception.Message)

        Finally

            myTask.Dispose()
            measureFrequencyButton.Enabled = True

        End Try

    End Sub

End Class
