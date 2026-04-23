'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   MeasureGpsTimestamp
'
' Category:
'   CI
'
' Description:
'   This example demonstrates how to use a GPS counter to update the current
'   time.
'
' Instructions for running:
'   1.  Select the GPS counter.
'   2.  Select the GPS synchronization method.
'   3.  Select the GPS synchronization source.
'   4.  If applicable, set the initial time.Note:  The initial time is only
'       applicable when the synchronization method is PPS or None.
'
' Steps:
'   1.  Create a new counter input task.
'   2.  Create a GPS Timestamp channel on the task.
'   3.  Set the initial time if applicable.
'   4.  Create a CounterReader and associate it with the task by using the
'       task's stream.
'   5.  Start the task and the timer.
'   6.  Inside the timer callback, read the count value and update the GPS time.
'   7.  Dispose the Task object to clean-up any resources associated with the
'       task.
'   8.  Handle any DaqExceptions, if they occur.
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
    Private myCounterReader As CounterReader
    Private reading As System.Double

    Private components As System.ComponentModel.IContainer
    Friend WithEvents stopButton As System.Windows.Forms.Button
    Friend WithEvents startButton As System.Windows.Forms.Button
    Friend WithEvents channelParameterGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents counterLabel As System.Windows.Forms.Label
    Friend WithEvents loopTimer As System.Windows.Forms.Timer
    Friend WithEvents counterComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents timeGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents gpsDateTimePicker As System.Windows.Forms.DateTimePicker
    Friend WithEvents syncMethodLabel As System.Windows.Forms.Label
    Friend WithEvents syncMethodComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents syncSourceComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents syncSourceLabel As System.Windows.Forms.Label
    Friend WithEvents startingGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents startingDateTimePicker As System.Windows.Forms.DateTimePicker

    Public Sub New()
        MyBase.New()
        Application.EnableVisualStyles()
        Application.DoEvents()
        '
        ' Required for Windows Form Designer support
        '
        InitializeComponent()

        ' Initialize UI
        counterComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.CI, PhysicalChannelAccess.External))
        If counterComboBox.Items.Count > 0 Then
            counterComboBox.SelectedIndex = 0
        End If

        syncSourceComboBox.Items.AddRange(DaqSystem.Local.GetTerminals(TerminalTypes.All))
        If syncSourceComboBox.Items.Count > 0 Then
            syncSourceComboBox.SelectedIndex = 0
        End If

        syncMethodComboBox.SelectedIndex = 0

        startingDateTimePicker.Enabled = False
        startingDateTimePicker.Value = New DateTime(DateTime.Today.Year, 1, 1)

        gpsDateTimePicker.Value = New DateTime(DateTime.Today.Year, 1, 1)
        gpsDateTimePicker.MinDate = gpsDateTimePicker.Value
        gpsDateTimePicker.MaxDate = gpsDateTimePicker.Value.Add(TimeSpan.FromMilliseconds(1))
    End Sub 'New

    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub 'Dispose

    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MainForm))
        Me.timeGroupBox = New System.Windows.Forms.GroupBox
        Me.gpsDateTimePicker = New System.Windows.Forms.DateTimePicker
        Me.stopButton = New System.Windows.Forms.Button
        Me.startButton = New System.Windows.Forms.Button
        Me.channelParameterGroupBox = New System.Windows.Forms.GroupBox
        Me.counterComboBox = New System.Windows.Forms.ComboBox
        Me.syncMethodLabel = New System.Windows.Forms.Label
        Me.counterLabel = New System.Windows.Forms.Label
        Me.syncMethodComboBox = New System.Windows.Forms.ComboBox
        Me.syncSourceComboBox = New System.Windows.Forms.ComboBox
        Me.syncSourceLabel = New System.Windows.Forms.Label
        Me.loopTimer = New System.Windows.Forms.Timer(Me.components)
        Me.startingGroupBox = New System.Windows.Forms.GroupBox
        Me.startingDateTimePicker = New System.Windows.Forms.DateTimePicker
        Me.timeGroupBox.SuspendLayout()
        Me.channelParameterGroupBox.SuspendLayout()
        Me.startingGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'timeGroupBox
        '
        Me.timeGroupBox.Controls.Add(Me.gpsDateTimePicker)
        Me.timeGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.timeGroupBox.Location = New System.Drawing.Point(8, 208)
        Me.timeGroupBox.Name = "timeGroupBox"
        Me.timeGroupBox.Size = New System.Drawing.Size(288, 64)
        Me.timeGroupBox.TabIndex = 2
        Me.timeGroupBox.TabStop = False
        Me.timeGroupBox.Text = "GPS Time"
        '
        'gpsDateTimePicker
        '
        Me.gpsDateTimePicker.CustomFormat = "MMMM dd, yyyy hh:mm:ss tt"
        Me.gpsDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.gpsDateTimePicker.Location = New System.Drawing.Point(8, 24)
        Me.gpsDateTimePicker.Name = "gpsDateTimePicker"
        Me.gpsDateTimePicker.ShowUpDown = True
        Me.gpsDateTimePicker.Size = New System.Drawing.Size(272, 20)
        Me.gpsDateTimePicker.TabIndex = 0
        Me.gpsDateTimePicker.Value = New Date(2005, 1, 1, 0, 0, 0, 0)
        '
        'stopButton
        '
        Me.stopButton.Enabled = False
        Me.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.stopButton.Location = New System.Drawing.Point(165, 280)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(72, 24)
        Me.stopButton.TabIndex = 4
        Me.stopButton.Text = "Stop"
        '
        'startButton
        '
        Me.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.startButton.Location = New System.Drawing.Point(69, 280)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(72, 24)
        Me.startButton.TabIndex = 3
        Me.startButton.Text = "Start"
        '
        'channelParameterGroupBox
        '
        Me.channelParameterGroupBox.Controls.Add(Me.counterComboBox)
        Me.channelParameterGroupBox.Controls.Add(Me.syncMethodLabel)
        Me.channelParameterGroupBox.Controls.Add(Me.counterLabel)
        Me.channelParameterGroupBox.Controls.Add(Me.syncMethodComboBox)
        Me.channelParameterGroupBox.Controls.Add(Me.syncSourceComboBox)
        Me.channelParameterGroupBox.Controls.Add(Me.syncSourceLabel)
        Me.channelParameterGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.channelParameterGroupBox.Location = New System.Drawing.Point(8, 9)
        Me.channelParameterGroupBox.Name = "channelParameterGroupBox"
        Me.channelParameterGroupBox.Size = New System.Drawing.Size(288, 119)
        Me.channelParameterGroupBox.TabIndex = 0
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
        'syncMethodLabel
        '
        Me.syncMethodLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.syncMethodLabel.Location = New System.Drawing.Point(8, 58)
        Me.syncMethodLabel.Name = "syncMethodLabel"
        Me.syncMethodLabel.Size = New System.Drawing.Size(120, 16)
        Me.syncMethodLabel.TabIndex = 2
        Me.syncMethodLabel.Text = "Synchronization Method:"
        '
        'counterLabel
        '
        Me.counterLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.counterLabel.Location = New System.Drawing.Point(8, 26)
        Me.counterLabel.Name = "counterLabel"
        Me.counterLabel.Size = New System.Drawing.Size(72, 16)
        Me.counterLabel.TabIndex = 0
        Me.counterLabel.Text = "GPS Counter:"
        '
        'syncMethodComboBox
        '
        Me.syncMethodComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.syncMethodComboBox.Items.AddRange(New Object() {"IRIG-B", "PPS", "None"})
        Me.syncMethodComboBox.Location = New System.Drawing.Point(136, 56)
        Me.syncMethodComboBox.Name = "syncMethodComboBox"
        Me.syncMethodComboBox.Size = New System.Drawing.Size(144, 21)
        Me.syncMethodComboBox.TabIndex = 3
        '
        'syncSourceComboBox
        '
        Me.syncSourceComboBox.Location = New System.Drawing.Point(136, 88)
        Me.syncSourceComboBox.Name = "syncSourceComboBox"
        Me.syncSourceComboBox.Size = New System.Drawing.Size(144, 21)
        Me.syncSourceComboBox.TabIndex = 5
        Me.syncSourceComboBox.Text = "/Dev1/PFI7"
        '
        'syncSourceLabel
        '
        Me.syncSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.syncSourceLabel.Location = New System.Drawing.Point(8, 90)
        Me.syncSourceLabel.Name = "syncSourceLabel"
        Me.syncSourceLabel.Size = New System.Drawing.Size(120, 16)
        Me.syncSourceLabel.TabIndex = 4
        Me.syncSourceLabel.Text = "Synchronization Source:"
        '
        'loopTimer
        '
        '
        'startingGroupBox
        '
        Me.startingGroupBox.Controls.Add(Me.startingDateTimePicker)
        Me.startingGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.startingGroupBox.Location = New System.Drawing.Point(8, 136)
        Me.startingGroupBox.Name = "startingGroupBox"
        Me.startingGroupBox.Size = New System.Drawing.Size(288, 64)
        Me.startingGroupBox.TabIndex = 1
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
        Me.startingDateTimePicker.TabIndex = 0
        Me.startingDateTimePicker.Value = New Date(2005, 1, 1, 0, 0, 0, 0)
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(306, 312)
        Me.Controls.Add(Me.timeGroupBox)
        Me.Controls.Add(Me.stopButton)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.channelParameterGroupBox)
        Me.Controls.Add(Me.startingGroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Measure GPS Timestamp"
        Me.timeGroupBox.ResumeLayout(False)
        Me.channelParameterGroupBox.ResumeLayout(False)
        Me.startingGroupBox.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub 'InitializeComponent

    Private Sub startButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles startButton.Click

        ' This example uses the default source (or gate) terminal for 
        ' the counter of your device.  To determine what the default 
        ' counter pins for your device are or to set a different source 
        ' (or gate) pin, refer to the Connecting Counter Signals topic
        ' in the NI-DAQmx Help (search for "Connecting Counter Signals").

        Try
            ' Create the task
            myTask = New Task()

            ' Determine the method
            Dim method As CIGpsSyncMethod
            Select Case syncMethodComboBox.SelectedItem.ToString()
                Case "IRIG-B"
                    method = CIGpsSyncMethod.IrigB
                Case "PPS"
                    method = CIGpsSyncMethod.Pps
                Case "None"
                    method = CIGpsSyncMethod.None
            End Select

            ' Create the channel
            Dim myChan As CIChannel = myTask.CIChannels.CreateGpsTimestampChannel(counterComboBox.Text, "", method, CITimestampUnits.Seconds)

            ' Set the synchronization source
            myChan.GpsSyncSource = syncSourceComboBox.Text

            ' Set start time
            If syncMethodComboBox.Text <> "IRIG-B" Then
                Dim startOfYear As New DateTime(startingDateTimePicker.Value.Year, 1, 1)
                Dim startTime As TimeSpan = startingDateTimePicker.Value.Subtract(startOfYear)
                myChan.TimestampInitialSeconds = CLng(startTime.TotalSeconds)
            End If

            ' Create the reader
            myCounterReader = New CounterReader(myTask.Stream)

            ' Setup UI
            startButton.Enabled = False
            stopButton.Enabled = True
            counterComboBox.Enabled = False
            syncMethodComboBox.Enabled = False
            syncSourceComboBox.Enabled = False
            startingDateTimePicker.Enabled = False
            
            ' Begin loop
            loopTimer.Enabled = True
        Catch exception As DaqException
            loopTimer.Enabled = False
            myTask.Dispose()
            MessageBox.Show(exception.Message)

            startButton.Enabled = True
            stopButton.Enabled = False
            counterComboBox.Enabled = True
            syncMethodComboBox.Enabled = True
            syncSourceComboBox.Enabled = True

            If syncMethodComboBox.Text = "IRIG-B" Then
                startingDateTimePicker.Enabled = False
            Else
                startingDateTimePicker.Enabled = True
            End If            
        End Try
    End Sub 'startButton_Click

    Private Sub loopTimer_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles loopTimer.Tick
        Try
            ' Get the number of seconds since midnight on January 1st
            reading = myCounterReader.ReadSingleSampleDouble()

            ' Update time display
            Dim today As DateTime = DateTime.Today
            Dim startOfYear As New DateTime(today.Year, 1, 1)
            Dim now As DateTime = startOfYear.Add(TimeSpan.FromSeconds(reading))

            ' In order to prevent an exception from setting the DateTimePicker's
            ' MinDate to be later than the MaxDate, first set the MinDate to
            ' the earliest possible DateTime value, and the MaxDate to the latest
            ' possible value.
            gpsDateTimePicker.MinDate = DateTimePicker.MinDateTime
            gpsDateTimePicker.MaxDate = DateTimePicker.MaxDateTime
            gpsDateTimePicker.Value = now
            gpsDateTimePicker.MinDate = now
            gpsDateTimePicker.MaxDate = now.Add(TimeSpan.FromMilliseconds(1))
        Catch exception As DaqException
            loopTimer.Enabled = False
            myTask.Dispose()
            MessageBox.Show(exception.Message)
            
            startButton.Enabled = True
            stopButton.Enabled = False
            counterComboBox.Enabled = True
            syncMethodComboBox.Enabled = True
            syncSourceComboBox.Enabled = True

            If syncMethodComboBox.Text = "IRIG-B" Then
                startingDateTimePicker.Enabled = False
            Else
                startingDateTimePicker.Enabled = True
            End If
        End Try
    End Sub 'loopTimer_Tick

    Private Sub stopButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles stopButton.Click
        loopTimer.Enabled = False
        myTask.Stop()
        myTask.Dispose()

        startButton.Enabled = True
        stopButton.Enabled = False
        counterComboBox.Enabled = True
        syncMethodComboBox.Enabled = True
        syncSourceComboBox.Enabled = True

        If syncMethodComboBox.Text = "IRIG-B" Then
            startingDateTimePicker.Enabled = False
        Else
            startingDateTimePicker.Enabled = True
        End If
    End Sub 'stopButton_Click


    Private Sub syncMethodComboBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles syncMethodComboBox.SelectedIndexChanged
        If syncMethodComboBox.Text = "IRIG-B" Then
            startingDateTimePicker.Enabled = False
        Else
            startingDateTimePicker.Enabled = True
        End If
    End Sub 'syncMethodComboBox_SelectedIndexChanged
End Class 'MainForm
