'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   MeasBuffered_SemiPeriodFinite
'
' Category:
'   CI
'
' Description:
'   This example demonstrates how to measure semi-periods on a counter input
'   channel. The 
'   minimum value, maximum value, sample mode, and samples per channel are all
'   configurable.This example 
'   shows how to measure semi-period on the counter's default input terminal (see
'   I/O Conections Overview 
'   below for more information), but can easily be expanded to measure semi-period
'   on any PFI, RTSI, or internal 
'   signal by setting the properties on the CIChannel object.Semi-period
'   measurement differs from pulse width 
'   measurement in that it measures both the high and the low pulses of a given
'   signal.  So for every period, 
'   two data points will be returned.
'
' Instructions for running:
'   1.  Enter the physical channel which corresponds to the counter you want to
'       use to measure semi-periods on the DAQ device.
'   2.  Enter the maximum and minimum value to specify a range for your unknown
'       semi-periods.Note:  It is important to set the maximum and minimum
'       values of your unknown semi-period as accurately as possible so the best
'       internal timebase can be chosen to minimize measurement error.  The
'       default values specify the range that can be measured by the counter
'       using the 20MHzTimebase.  Use the GenDigPulseTrain_Continuous example to
'       verify that you are measuring correctly on the DAQ device.
'
' Steps:
'   1.  Create a counter input channel using
'       Task.CIChannel.CreateCISemiPeriodChannel. It is important to set the
'       maximum and minimum values of your unknown period as accurately as
'       possible so the best internal timebase can be chosen to minimize
'       measurement error.  The default values specify a range that can be
'       measured by the counter using the 20MHzTimebase.
'   2.  Call the ConfigureImplicit method to configure the sample mode and
'       samples per channel.  Note: For time measurements with counters, the
'       ConfigureImplicit method is used because the signal being measured
'       itself determines the sample rate.  This is unlike buffered event
'       counting, where an external sample clock must be used.
'   3.  Call the CounterReader.BeginReadMultiSampleDouble method to arm the
'       counter and begin measuring.
'   4.  Call the Task.Dispose method to stop the task and de-allocate any
'       resources used by the Task.
'   5.  Handle any DaqExceptions and display a message box if there are errors.
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
'   measurement being taken.  In this example the two edge separation will be
'   measured on the default input terminals on ctr0. For more information on the
'   default counter input and output terminals for your device, open the
'   NI-DAQmx Help, and refer to Counter Signal Connections found under the
'   Device Considerations book in the table of contents.
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
    Private loopFlag As Boolean
    Private measureValue As Double()
    Private myCounterRead As CounterReader
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
    Friend WithEvents startButton As System.Windows.Forms.Button
    Friend WithEvents measuredPeriodLabel As System.Windows.Forms.Label
    Friend WithEvents channelParamGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents physicalChannelLabel As System.Windows.Forms.Label
    Friend WithEvents minValueTextBox As System.Windows.Forms.TextBox
    Friend WithEvents minimumValueLabel As System.Windows.Forms.Label
    Friend WithEvents maximumValueLabel As System.Windows.Forms.Label
    Friend WithEvents maxValueTextBox As System.Windows.Forms.TextBox
    Friend WithEvents periodListBox As System.Windows.Forms.ListBox
    Friend WithEvents timingParamGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents samplesPerChannelTextBox As System.Windows.Forms.TextBox
    Friend WithEvents samplesPerChannelLabel As System.Windows.Forms.Label
    Friend WithEvents dataGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents counterComboBox As System.Windows.Forms.ComboBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MainForm))
        Me.startButton = New System.Windows.Forms.Button
        Me.measuredPeriodLabel = New System.Windows.Forms.Label
        Me.channelParamGroupBox = New System.Windows.Forms.GroupBox
        Me.counterComboBox = New System.Windows.Forms.ComboBox
        Me.physicalChannelLabel = New System.Windows.Forms.Label
        Me.minValueTextBox = New System.Windows.Forms.TextBox
        Me.minimumValueLabel = New System.Windows.Forms.Label
        Me.maximumValueLabel = New System.Windows.Forms.Label
        Me.maxValueTextBox = New System.Windows.Forms.TextBox
        Me.periodListBox = New System.Windows.Forms.ListBox
        Me.timingParamGroupBox = New System.Windows.Forms.GroupBox
        Me.samplesPerChannelLabel = New System.Windows.Forms.Label
        Me.samplesPerChannelTextBox = New System.Windows.Forms.TextBox
        Me.dataGroupBox = New System.Windows.Forms.GroupBox
        Me.channelParamGroupBox.SuspendLayout()
        Me.timingParamGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'startButton
        '
        Me.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.startButton.Location = New System.Drawing.Point(192, 224)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(112, 40)
        Me.startButton.TabIndex = 0
        Me.startButton.Text = "Measure Periods"
        '
        'measuredPeriodLabel
        '
        Me.measuredPeriodLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.measuredPeriodLabel.Location = New System.Drawing.Point(184, 32)
        Me.measuredPeriodLabel.Name = "measuredPeriodLabel"
        Me.measuredPeriodLabel.Size = New System.Drawing.Size(128, 16)
        Me.measuredPeriodLabel.TabIndex = 4
        Me.measuredPeriodLabel.Text = "Measured Period (sec):"
        '
        'channelParamGroupBox
        '
        Me.channelParamGroupBox.Controls.Add(Me.counterComboBox)
        Me.channelParamGroupBox.Controls.Add(Me.physicalChannelLabel)
        Me.channelParamGroupBox.Controls.Add(Me.minValueTextBox)
        Me.channelParamGroupBox.Controls.Add(Me.minimumValueLabel)
        Me.channelParamGroupBox.Controls.Add(Me.maximumValueLabel)
        Me.channelParamGroupBox.Controls.Add(Me.maxValueTextBox)
        Me.channelParamGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.channelParamGroupBox.Location = New System.Drawing.Point(8, 8)
        Me.channelParamGroupBox.Name = "channelParamGroupBox"
        Me.channelParamGroupBox.Size = New System.Drawing.Size(152, 189)
        Me.channelParamGroupBox.TabIndex = 1
        Me.channelParamGroupBox.TabStop = False
        Me.channelParamGroupBox.Text = "Channel Parameters:"
        '
        'counterComboBox
        '
        Me.counterComboBox.Location = New System.Drawing.Point(16, 40)
        Me.counterComboBox.Name = "counterComboBox"
        Me.counterComboBox.Size = New System.Drawing.Size(121, 21)
        Me.counterComboBox.TabIndex = 1
        Me.counterComboBox.Text = "Dev1/ctr0"
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
        Me.minValueTextBox.Location = New System.Drawing.Point(16, 96)
        Me.minValueTextBox.Name = "minValueTextBox"
        Me.minValueTextBox.Size = New System.Drawing.Size(120, 20)
        Me.minValueTextBox.TabIndex = 3
        Me.minValueTextBox.Text = "0.00000010"
        '
        'minimumValueLabel
        '
        Me.minimumValueLabel.BackColor = System.Drawing.SystemColors.Control
        Me.minimumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.minimumValueLabel.Location = New System.Drawing.Point(16, 80)
        Me.minimumValueLabel.Name = "minimumValueLabel"
        Me.minimumValueLabel.Size = New System.Drawing.Size(112, 16)
        Me.minimumValueLabel.TabIndex = 2
        Me.minimumValueLabel.Text = "Minimum value (sec):"
        '
        'maximumValueLabel
        '
        Me.maximumValueLabel.BackColor = System.Drawing.SystemColors.Control
        Me.maximumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.maximumValueLabel.Location = New System.Drawing.Point(16, 136)
        Me.maximumValueLabel.Name = "maximumValueLabel"
        Me.maximumValueLabel.Size = New System.Drawing.Size(120, 16)
        Me.maximumValueLabel.TabIndex = 4
        Me.maximumValueLabel.Text = "Maximum Value (sec):"
        '
        'maxValueTextBox
        '
        Me.maxValueTextBox.Location = New System.Drawing.Point(16, 152)
        Me.maxValueTextBox.Name = "maxValueTextBox"
        Me.maxValueTextBox.Size = New System.Drawing.Size(120, 20)
        Me.maxValueTextBox.TabIndex = 5
        Me.maxValueTextBox.Text = "0.8388"
        '
        'periodListBox
        '
        Me.periodListBox.Location = New System.Drawing.Point(184, 48)
        Me.periodListBox.Name = "periodListBox"
        Me.periodListBox.Size = New System.Drawing.Size(120, 134)
        Me.periodListBox.TabIndex = 5
        '
        'timingParamGroupBox
        '
        Me.timingParamGroupBox.Controls.Add(Me.samplesPerChannelLabel)
        Me.timingParamGroupBox.Controls.Add(Me.samplesPerChannelTextBox)
        Me.timingParamGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.timingParamGroupBox.Location = New System.Drawing.Point(8, 200)
        Me.timingParamGroupBox.Name = "timingParamGroupBox"
        Me.timingParamGroupBox.Size = New System.Drawing.Size(152, 80)
        Me.timingParamGroupBox.TabIndex = 2
        Me.timingParamGroupBox.TabStop = False
        Me.timingParamGroupBox.Text = "Timing Parameters:"
        '
        'samplesPerChannelLabel
        '
        Me.samplesPerChannelLabel.BackColor = System.Drawing.SystemColors.Control
        Me.samplesPerChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.samplesPerChannelLabel.Location = New System.Drawing.Point(16, 24)
        Me.samplesPerChannelLabel.Name = "samplesPerChannelLabel"
        Me.samplesPerChannelLabel.Size = New System.Drawing.Size(120, 16)
        Me.samplesPerChannelLabel.TabIndex = 0
        Me.samplesPerChannelLabel.Text = "Samples Per Channel:"
        '
        'samplesPerChannelTextBox
        '
        Me.samplesPerChannelTextBox.Location = New System.Drawing.Point(16, 40)
        Me.samplesPerChannelTextBox.Name = "samplesPerChannelTextBox"
        Me.samplesPerChannelTextBox.Size = New System.Drawing.Size(120, 20)
        Me.samplesPerChannelTextBox.TabIndex = 1
        Me.samplesPerChannelTextBox.Text = "1000"
        '
        'dataGroupBox
        '
        Me.dataGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.dataGroupBox.Location = New System.Drawing.Point(168, 8)
        Me.dataGroupBox.Name = "dataGroupBox"
        Me.dataGroupBox.Size = New System.Drawing.Size(152, 189)
        Me.dataGroupBox.TabIndex = 3
        Me.dataGroupBox.TabStop = False
        Me.dataGroupBox.Text = "Data:"
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(330, 288)
        Me.Controls.Add(Me.timingParamGroupBox)
        Me.Controls.Add(Me.periodListBox)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.measuredPeriodLabel)
        Me.Controls.Add(Me.channelParamGroupBox)
        Me.Controls.Add(Me.dataGroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Measure Buffered Semi-Period-Finite"
        Me.channelParamGroupBox.ResumeLayout(False)
        Me.timingParamGroupBox.ResumeLayout(False)
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

            myTask.CIChannels.CreateSemiPeriodChannel(counterComboBox.Text, _
                "SemiPeriodMeasurement", Convert.ToDouble(minValueTextBox.Text), _
                Convert.ToDouble(maxValueTextBox.Text), CISemiPeriodUnits.Seconds)

            myTask.Timing.ConfigureImplicit(SampleQuantityMode.FiniteSamples, _
                Convert.ToInt32(samplesPerChannelTextBox.Text))

            myCounterRead = New CounterReader(myTask.Stream)

            ' Use SynchronizeCallbacks to specify that the object 
            ' marshals callbacks across threads appropriately.
            myCounterRead.SynchronizeCallbacks = True

            myCallBack = New AsyncCallback(AddressOf CounterInCallback)
            myCounterRead.BeginReadMultiSampleDouble(-1, myCallBack, Nothing)

            startButton.Enabled = False

        Catch exception As DaqException

            myTask.Dispose()
            MessageBox.Show(exception.Message)
            startButton.Enabled = True

        End Try

    End Sub

    Private Sub CounterInCallback(ByVal ar As IAsyncResult)

        'Reads the returned data and updates the form control
        Try

            measureValue = myCounterRead.EndReadMultiSampleDouble(ar)

            'Display only the first 10 points of data acquired
            Dim samplesToDisplay As Int32 = 10
            If measureValue.Length < 10 Then
                samplesToDisplay = measureValue.Length
            End If

            periodListBox.BeginUpdate()
            periodListBox.Items.Clear()

            For i As Int32 = 0 To samplesToDisplay - 1
                periodListBox.Items.Add(measureValue(i))
            Next

            periodListBox.EndUpdate()

        Catch exception As DaqException

            MessageBox.Show(exception.Message)

        Finally

            myTask.Dispose()
            startButton.Enabled = True

        End Try

    End Sub

End Class
