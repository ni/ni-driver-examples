'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   MeasGpsTimestamp_BuffFinite
'
' Category:
'   CI
'
' Description:
'   This example demonstrates how to use a finite buffereded task to measure
'   time using a GPS Timestamp Channel.  The Synchronization Method,
'   Synchronization Source, Sample Clock Source, and Samples per Channel are all
'   configurable.
'
' Instructions for running:
'   1.  Select the Physical Channel which corresponds to the GPS counter you
'       want to count edges on the DAQ device.
'   2.  Select the Synchronization Method and Synchronization Source to specify
'       which type of  GPS synchronization signal you want to use.
'   3.  Set the Sample Clock Source and Samples per Channel to configure timing
'       for the measurement.
'   4.  If applicable, set the initial time.Note:  The initial time is only
'       applicable when the synchronization method is PPS or None.
'   5.  Set the Rate of the Acquisition.
'   6.  Select the Source of the Digital Reference Trigger for the acquisition.
'
' Steps:
'   1.  Create a new counter input task.
'   2.  Create a GPS Timestamp channel on the task.
'   3.  Set the initial time if applicable.
'   4.  Create a CounterReader and associate it with the task by using the
'       task's stream.
'   5.  Configure the sample clock, set the acquisition mode to finite.
'   6.  Use the ReferenceTrigger object properties to configure a digital edge
'       trigger.
'   7.  Start the task.
'   8.  Read the count values and update the GPS time.
'   9.  Dispose the Task object to clean-up any resources associated with the
'       task.
'   10. Handle any DaqExceptions, if they occur.
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

    Private myTask As Task
    Private myCounterReader As CounterReader

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()
        Application.EnableVisualStyles()
        Application.DoEvents()
        'This call is required by the Windows Form Designer.
        InitializeComponent()

        counterComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.CI, PhysicalChannelAccess.External))
        If (counterComboBox.Items.Count > 0) Then
            counterComboBox.SelectedIndex = 0
        End If

        synchronizationMethodComboBox.SelectedIndex = 0

        synchronizationSourceComboBox.Items.AddRange(DaqSystem.Local.GetTerminals(TerminalTypes.Basic))
        If (synchronizationSourceComboBox.Items.Count > 0) Then
            synchronizationSourceComboBox.SelectedIndex = 1
        End If

        sampleClockSourceComboBox.Items.AddRange(DaqSystem.Local.GetTerminals(TerminalTypes.All))

        startingDateTimePicker.Enabled = False
        startingDateTimePicker.Value = New DateTime(DateTime.Today.Year, 1, 1)

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
    Friend WithEvents timingParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents samplesPerChannelNumericUpDown As System.Windows.Forms.NumericUpDown
    Friend WithEvents rateNumericUpDown As System.Windows.Forms.NumericUpDown
    Friend WithEvents samplesPerChannelLabel As System.Windows.Forms.Label
    Friend WithEvents rateLabel As System.Windows.Forms.Label
    Friend WithEvents sampleClockSourceComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents sampleClockSourceLabel As System.Windows.Forms.Label
    Friend WithEvents timeGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents gpsTimeListBox As System.Windows.Forms.ListBox
    Friend WithEvents startButton As System.Windows.Forms.Button
    Friend WithEvents channelParameterGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents counterComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents synchronizationMethodLabel As System.Windows.Forms.Label
    Friend WithEvents counterLabel As System.Windows.Forms.Label
    Friend WithEvents synchronizationMethodComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents synchronizationSourceComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents synchronizationSourceLabel As System.Windows.Forms.Label
    Friend WithEvents startingGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents startingDateTimePicker As System.Windows.Forms.DateTimePicker
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MainForm))
        Me.timingParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.samplesPerChannelNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.rateNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.samplesPerChannelLabel = New System.Windows.Forms.Label
        Me.rateLabel = New System.Windows.Forms.Label
        Me.sampleClockSourceComboBox = New System.Windows.Forms.ComboBox
        Me.sampleClockSourceLabel = New System.Windows.Forms.Label
        Me.timeGroupBox = New System.Windows.Forms.GroupBox
        Me.gpsTimeListBox = New System.Windows.Forms.ListBox
        Me.startButton = New System.Windows.Forms.Button
        Me.channelParameterGroupBox = New System.Windows.Forms.GroupBox
        Me.counterComboBox = New System.Windows.Forms.ComboBox
        Me.synchronizationMethodLabel = New System.Windows.Forms.Label
        Me.counterLabel = New System.Windows.Forms.Label
        Me.synchronizationMethodComboBox = New System.Windows.Forms.ComboBox
        Me.synchronizationSourceComboBox = New System.Windows.Forms.ComboBox
        Me.synchronizationSourceLabel = New System.Windows.Forms.Label
        Me.startingGroupBox = New System.Windows.Forms.GroupBox
        Me.startingDateTimePicker = New System.Windows.Forms.DateTimePicker
        Me.timingParametersGroupBox.SuspendLayout()
        CType(Me.samplesPerChannelNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rateNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.timeGroupBox.SuspendLayout()
        Me.channelParameterGroupBox.SuspendLayout()
        Me.startingGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'timingParametersGroupBox
        '
        Me.timingParametersGroupBox.Controls.Add(Me.samplesPerChannelNumericUpDown)
        Me.timingParametersGroupBox.Controls.Add(Me.rateNumericUpDown)
        Me.timingParametersGroupBox.Controls.Add(Me.samplesPerChannelLabel)
        Me.timingParametersGroupBox.Controls.Add(Me.rateLabel)
        Me.timingParametersGroupBox.Controls.Add(Me.sampleClockSourceComboBox)
        Me.timingParametersGroupBox.Controls.Add(Me.sampleClockSourceLabel)
        Me.timingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.timingParametersGroupBox.Location = New System.Drawing.Point(8, 134)
        Me.timingParametersGroupBox.Name = "timingParametersGroupBox"
        Me.timingParametersGroupBox.Size = New System.Drawing.Size(288, 119)
        Me.timingParametersGroupBox.TabIndex = 16
        Me.timingParametersGroupBox.TabStop = False
        Me.timingParametersGroupBox.Text = "Timing Parameters"
        '
        'samplesPerChannelNumericUpDown
        '
        Me.samplesPerChannelNumericUpDown.Location = New System.Drawing.Point(136, 56)
        Me.samplesPerChannelNumericUpDown.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.samplesPerChannelNumericUpDown.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.samplesPerChannelNumericUpDown.Name = "samplesPerChannelNumericUpDown"
        Me.samplesPerChannelNumericUpDown.Size = New System.Drawing.Size(144, 20)
        Me.samplesPerChannelNumericUpDown.TabIndex = 9
        Me.samplesPerChannelNumericUpDown.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        'rateNumericUpDown
        '
        Me.rateNumericUpDown.DecimalPlaces = 2
        Me.rateNumericUpDown.Location = New System.Drawing.Point(136, 24)
        Me.rateNumericUpDown.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.rateNumericUpDown.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.rateNumericUpDown.Name = "rateNumericUpDown"
        Me.rateNumericUpDown.Size = New System.Drawing.Size(144, 20)
        Me.rateNumericUpDown.TabIndex = 7
        Me.rateNumericUpDown.Value = New Decimal(New Integer() {1000, 0, 0, 0})
        '
        'samplesPerChannelLabel
        '
        Me.samplesPerChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.samplesPerChannelLabel.Location = New System.Drawing.Point(8, 58)
        Me.samplesPerChannelLabel.Name = "samplesPerChannelLabel"
        Me.samplesPerChannelLabel.Size = New System.Drawing.Size(120, 16)
        Me.samplesPerChannelLabel.TabIndex = 8
        Me.samplesPerChannelLabel.Text = "Samples per &Channel:"
        '
        'rateLabel
        '
        Me.rateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.rateLabel.Location = New System.Drawing.Point(8, 26)
        Me.rateLabel.Name = "rateLabel"
        Me.rateLabel.Size = New System.Drawing.Size(72, 16)
        Me.rateLabel.TabIndex = 6
        Me.rateLabel.Text = "&Rate:"
        '
        'sampleClockSourceComboBox
        '
        Me.sampleClockSourceComboBox.Location = New System.Drawing.Point(136, 88)
        Me.sampleClockSourceComboBox.Name = "sampleClockSourceComboBox"
        Me.sampleClockSourceComboBox.Size = New System.Drawing.Size(144, 21)
        Me.sampleClockSourceComboBox.TabIndex = 11
        Me.sampleClockSourceComboBox.Text = "/Dev1/PFI9"
        '
        'sampleClockSourceLabel
        '
        Me.sampleClockSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.sampleClockSourceLabel.Location = New System.Drawing.Point(8, 90)
        Me.sampleClockSourceLabel.Name = "sampleClockSourceLabel"
        Me.sampleClockSourceLabel.Size = New System.Drawing.Size(120, 16)
        Me.sampleClockSourceLabel.TabIndex = 10
        Me.sampleClockSourceLabel.Text = "Sample C&lock Source:"
        '
        'timeGroupBox
        '
        Me.timeGroupBox.Controls.Add(Me.gpsTimeListBox)
        Me.timeGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.timeGroupBox.Location = New System.Drawing.Point(312, 7)
        Me.timeGroupBox.Name = "timeGroupBox"
        Me.timeGroupBox.Size = New System.Drawing.Size(224, 351)
        Me.timeGroupBox.TabIndex = 14
        Me.timeGroupBox.TabStop = False
        Me.timeGroupBox.Text = "GPS Time/Date"
        '
        'gpsTimeListBox
        '
        Me.gpsTimeListBox.Location = New System.Drawing.Point(8, 16)
        Me.gpsTimeListBox.Name = "gpsTimeListBox"
        Me.gpsTimeListBox.Size = New System.Drawing.Size(208, 329)
        Me.gpsTimeListBox.TabIndex = 0
        '
        'startButton
        '
        Me.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.startButton.Location = New System.Drawing.Point(224, 334)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(72, 24)
        Me.startButton.TabIndex = 18
        Me.startButton.Text = "&Start"
        '
        'channelParameterGroupBox
        '
        Me.channelParameterGroupBox.Controls.Add(Me.counterComboBox)
        Me.channelParameterGroupBox.Controls.Add(Me.synchronizationMethodLabel)
        Me.channelParameterGroupBox.Controls.Add(Me.counterLabel)
        Me.channelParameterGroupBox.Controls.Add(Me.synchronizationMethodComboBox)
        Me.channelParameterGroupBox.Controls.Add(Me.synchronizationSourceComboBox)
        Me.channelParameterGroupBox.Controls.Add(Me.synchronizationSourceLabel)
        Me.channelParameterGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.channelParameterGroupBox.Location = New System.Drawing.Point(9, 7)
        Me.channelParameterGroupBox.Name = "channelParameterGroupBox"
        Me.channelParameterGroupBox.Size = New System.Drawing.Size(288, 119)
        Me.channelParameterGroupBox.TabIndex = 15
        Me.channelParameterGroupBox.TabStop = False
        Me.channelParameterGroupBox.Text = "Channel Parameters"
        '
        'counterComboBox
        '
        Me.counterComboBox.Location = New System.Drawing.Point(136, 24)
        Me.counterComboBox.Name = "counterComboBox"
        Me.counterComboBox.Size = New System.Drawing.Size(144, 21)
        Me.counterComboBox.TabIndex = 1
        Me.counterComboBox.Text = "Dev1/gpsTimestampCtr0"
        '
        'synchronizationMethodLabel
        '
        Me.synchronizationMethodLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.synchronizationMethodLabel.Location = New System.Drawing.Point(8, 58)
        Me.synchronizationMethodLabel.Name = "synchronizationMethodLabel"
        Me.synchronizationMethodLabel.Size = New System.Drawing.Size(120, 16)
        Me.synchronizationMethodLabel.TabIndex = 2
        Me.synchronizationMethodLabel.Text = "S&ynchronization &Method:"
        '
        'counterLabel
        '
        Me.counterLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.counterLabel.Location = New System.Drawing.Point(8, 26)
        Me.counterLabel.Name = "counterLabel"
        Me.counterLabel.Size = New System.Drawing.Size(72, 16)
        Me.counterLabel.TabIndex = 0
        Me.counterLabel.Text = "&GPS Counter:"
        '
        'synchronizationMethodComboBox
        '
        Me.synchronizationMethodComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.synchronizationMethodComboBox.Items.AddRange(New Object() {"IRIG-B", "PPS", "None"})
        Me.synchronizationMethodComboBox.Location = New System.Drawing.Point(136, 56)
        Me.synchronizationMethodComboBox.Name = "synchronizationMethodComboBox"
        Me.synchronizationMethodComboBox.Size = New System.Drawing.Size(144, 21)
        Me.synchronizationMethodComboBox.TabIndex = 3
        '
        'synchronizationSourceComboBox
        '
        Me.synchronizationSourceComboBox.Location = New System.Drawing.Point(136, 88)
        Me.synchronizationSourceComboBox.Name = "synchronizationSourceComboBox"
        Me.synchronizationSourceComboBox.Size = New System.Drawing.Size(144, 21)
        Me.synchronizationSourceComboBox.TabIndex = 5
        Me.synchronizationSourceComboBox.Text = "/Dev1/PFI7"
        '
        'synchronizationSourceLabel
        '
        Me.synchronizationSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.synchronizationSourceLabel.Location = New System.Drawing.Point(8, 90)
        Me.synchronizationSourceLabel.Name = "synchronizationSourceLabel"
        Me.synchronizationSourceLabel.Size = New System.Drawing.Size(120, 16)
        Me.synchronizationSourceLabel.TabIndex = 4
        Me.synchronizationSourceLabel.Text = "Sy&nchronization &Source:"
        '
        'startingGroupBox
        '
        Me.startingGroupBox.Controls.Add(Me.startingDateTimePicker)
        Me.startingGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.startingGroupBox.Location = New System.Drawing.Point(9, 262)
        Me.startingGroupBox.Name = "startingGroupBox"
        Me.startingGroupBox.Size = New System.Drawing.Size(288, 64)
        Me.startingGroupBox.TabIndex = 17
        Me.startingGroupBox.TabStop = False
        Me.startingGroupBox.Text = "Starting Time"
        '
        'startingDateTimePicker
        '
        Me.startingDateTimePicker.CustomFormat = "MMMM dd, yyyy hh:mm:ss tt"
        Me.startingDateTimePicker.Enabled = False
        Me.startingDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.startingDateTimePicker.Location = New System.Drawing.Point(8, 24)
        Me.startingDateTimePicker.Name = "startingDateTimePicker"
        Me.startingDateTimePicker.Size = New System.Drawing.Size(272, 20)
        Me.startingDateTimePicker.TabIndex = 12
        Me.startingDateTimePicker.Value = New Date(2005, 1, 1, 0, 0, 0, 0)
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(544, 365)
        Me.Controls.Add(Me.timingParametersGroupBox)
        Me.Controls.Add(Me.timeGroupBox)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.channelParameterGroupBox)
        Me.Controls.Add(Me.startingGroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Measure GPS Timestamp - Buffered Finite"
        Me.timingParametersGroupBox.ResumeLayout(False)
        CType(Me.samplesPerChannelNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rateNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        Me.timeGroupBox.ResumeLayout(False)
        Me.channelParameterGroupBox.ResumeLayout(False)
        Me.startingGroupBox.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub startButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles startButton.Click
        Cursor.Current = Cursors.WaitCursor
        startButton.Enabled = False
        gpsTimeListBox.Items.Clear()

        Try
            myTask = New Task

            Dim method As CIGpsSyncMethod
            Select Case synchronizationMethodComboBox.SelectedItem.ToString()
                Case "IRIG-B"
                    method = CIGpsSyncMethod.IrigB
                Case "PPS"
                    method = CIGpsSyncMethod.Pps
                Case "None"
                    method = CIGpsSyncMethod.None
            End Select

            Dim myChan As CIChannel = myTask.CIChannels.CreateGpsTimestampChannel(counterComboBox.Text, "", method, CITimestampUnits.Seconds)

            myChan.GpsSyncSource = synchronizationSourceComboBox.Text

            If synchronizationMethodComboBox.Text <> "IRIG-B" Then
                Dim startOfYear As New DateTime(startingDateTimePicker.Value.Year, 1, 1)
                Dim startTime As TimeSpan = startingDateTimePicker.Value.Subtract(startOfYear)
                myChan.TimestampInitialSeconds = CLng(startTime.TotalSeconds)
            End If

            myCounterReader = New CounterReader(myTask.Stream)

            myTask.Timing.ConfigureSampleClock(sampleClockSourceComboBox.Text, CDbl(rateNumericUpDown.Value), SampleClockActiveEdge.Rising, SampleQuantityMode.FiniteSamples, CInt(samplesPerChannelNumericUpDown.Value))

            myTask.Start()

            Dim reading() As Double = myCounterReader.ReadMultiSampleDouble(-1)
            For Each readingSample As Double In reading
                Dim today As DateTime = DateTime.Today
                Dim startOfYear As New DateTime(today.Year, 1, 1)
                Dim now As DateTime = startOfYear.Add(TimeSpan.FromSeconds(readingSample))

                gpsTimeListBox.Items.Add(now)
            Next

        Catch exception As DaqException
            MessageBox.Show(exception.Message)
            If (synchronizationMethodComboBox.Text = "IRIG-B") Then
                startingDateTimePicker.Enabled = False
            Else
                startingDateTimePicker.Enabled = True
            End If
        Finally
            myTask.Dispose()
        End Try
        Cursor.Current = Cursors.Default
        startButton.Enabled = True
    End Sub

    Private Sub synchronizationMethodComboBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles synchronizationMethodComboBox.SelectedIndexChanged
        If synchronizationMethodComboBox.Text = "IRIG-B" Then
            startingDateTimePicker.Enabled = False
        Else
            startingDateTimePicker.Enabled = True
        End If
    End Sub 'synchronizationMethodComboBox_SelectedIndexChanged
End Class
