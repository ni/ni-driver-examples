'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   CountDigEvents
'
' Category:
'   CI
'
' Description:
'   This example demonstrates how to count digital events on a Counter Input
'   Channel. The Initial Count, Count Direction, and Edge are all
'   configurable.This example shows how to count edges on the counter's default
'   source pin, but could easily be expanded to count edges on any PFI, RTSI, or
'   internal signal. Non-buffered event counting can also use a digital pause
'   trigger which could be added to this example by configuring the Trigger
'   object for the Task.
'
' Instructions for running:
'   1.  Enter the physical channel which corresponds to the counter you want to
'       use to count edges on the DAQ device.
'   2.  Enter the initial count, count direction, and measurement edge to
'       specify how you want the counter to count.Note:  Use the
'       GenDigPulseTrain_Continuous example to verify that you are counting
'       correctly on the DAQ device.
'
' Steps:
'   1.  Create the Task object. Create a CIChannel object with the correct
'       configuration to count events. The edge parameter is used to determine
'       if the counter will count on rising or falling edges of the input
'       signal.
'   2.  Call the Start() method to arm the counter and begin counting.  The
'       counter will be preloaded with the initial count.
'   3.  The counter will be continually polled until the Stop button is pressed
'       on the user interface.
'   4.  Dispose the Task object to clean-up any resources associated with the
'       task.
'   5.  Handle any DaqExceptions, if they occur.
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

Public Class mainForm
    Inherits System.Windows.Forms.Form
    Private myTask As Task
    Private edgeType As CICountEdgesActiveEdge
    Private countDirection As CICountEdgesCountDirection
    Private myCICounterReader As CounterReader
    Private myReading As UInt32

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()
        Application.EnableVisualStyles()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        edgeType = CICountEdgesActiveEdge.Rising
        countDirection = CICountEdgesCountDirection.Up

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
    Friend WithEvents stopButton As System.Windows.Forms.Button
    Friend WithEvents startButton As System.Windows.Forms.Button
    Friend WithEvents channelParameterGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents countDirectionLabel As System.Windows.Forms.Label
    Friend WithEvents initialCountTextBox As System.Windows.Forms.TextBox
    Friend WithEvents initialCountLabel As System.Windows.Forms.Label
    Friend WithEvents countDirectionComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents loopTimer As System.Windows.Forms.Timer
    Friend WithEvents counterLabel As System.Windows.Forms.Label
    Friend WithEvents countTextBox As System.Windows.Forms.TextBox
    Friend WithEvents risingRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents fallingRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents edgeGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents dataGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents countLabel As System.Windows.Forms.Label
    Friend WithEvents counterComboBox As System.Windows.Forms.ComboBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(mainForm))
        Me.stopButton = New System.Windows.Forms.Button
        Me.startButton = New System.Windows.Forms.Button
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
        Me.dataGroupBox = New System.Windows.Forms.GroupBox
        Me.countLabel = New System.Windows.Forms.Label
        Me.countTextBox = New System.Windows.Forms.TextBox
        Me.loopTimer = New System.Windows.Forms.Timer(Me.components)
        Me.channelParameterGroupBox.SuspendLayout()
        Me.edgeGroupBox.SuspendLayout()
        Me.dataGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'stopButton
        '
        Me.stopButton.Enabled = False
        Me.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.stopButton.Location = New System.Drawing.Point(264, 144)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(112, 32)
        Me.stopButton.TabIndex = 1
        Me.stopButton.Text = "Stop"
        '
        'startButton
        '
        Me.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.startButton.Location = New System.Drawing.Point(264, 112)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(112, 32)
        Me.startButton.TabIndex = 0
        Me.startButton.Text = "Start"
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
        Me.channelParameterGroupBox.Size = New System.Drawing.Size(248, 168)
        Me.channelParameterGroupBox.TabIndex = 2
        Me.channelParameterGroupBox.TabStop = False
        Me.channelParameterGroupBox.Text = "Channel Parameters"
        '
        'counterComboBox
        '
        Me.counterComboBox.Location = New System.Drawing.Point(112, 24)
        Me.counterComboBox.Name = "counterComboBox"
        Me.counterComboBox.Size = New System.Drawing.Size(128, 21)
        Me.counterComboBox.TabIndex = 1
        Me.counterComboBox.Text = "Dev1/ctr0"
        '
        'edgeGroupBox
        '
        Me.edgeGroupBox.Controls.Add(Me.fallingRadioButton)
        Me.edgeGroupBox.Controls.Add(Me.risingRadioButton)
        Me.edgeGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.edgeGroupBox.Location = New System.Drawing.Point(16, 104)
        Me.edgeGroupBox.Name = "edgeGroupBox"
        Me.edgeGroupBox.Size = New System.Drawing.Size(216, 48)
        Me.edgeGroupBox.TabIndex = 6
        Me.edgeGroupBox.TabStop = False
        Me.edgeGroupBox.Text = "Edge"
        '
        'fallingRadioButton
        '
        Me.fallingRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.fallingRadioButton.Location = New System.Drawing.Point(120, 16)
        Me.fallingRadioButton.Name = "fallingRadioButton"
        Me.fallingRadioButton.Size = New System.Drawing.Size(64, 24)
        Me.fallingRadioButton.TabIndex = 1
        Me.fallingRadioButton.Text = "Falling"
        '
        'risingRadioButton
        '
        Me.risingRadioButton.Checked = True
        Me.risingRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.risingRadioButton.Location = New System.Drawing.Point(32, 16)
        Me.risingRadioButton.Name = "risingRadioButton"
        Me.risingRadioButton.Size = New System.Drawing.Size(64, 24)
        Me.risingRadioButton.TabIndex = 0
        Me.risingRadioButton.TabStop = True
        Me.risingRadioButton.Text = "Rising"
        '
        'countDirectionLabel
        '
        Me.countDirectionLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.countDirectionLabel.Location = New System.Drawing.Point(16, 72)
        Me.countDirectionLabel.Name = "countDirectionLabel"
        Me.countDirectionLabel.Size = New System.Drawing.Size(88, 16)
        Me.countDirectionLabel.TabIndex = 4
        Me.countDirectionLabel.Text = "Count Direction:"
        '
        'initialCountTextBox
        '
        Me.initialCountTextBox.Location = New System.Drawing.Point(112, 48)
        Me.initialCountTextBox.Name = "initialCountTextBox"
        Me.initialCountTextBox.Size = New System.Drawing.Size(128, 20)
        Me.initialCountTextBox.TabIndex = 3
        Me.initialCountTextBox.Text = "0"
        '
        'initialCountLabel
        '
        Me.initialCountLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.initialCountLabel.Location = New System.Drawing.Point(16, 48)
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
        Me.counterLabel.Size = New System.Drawing.Size(64, 16)
        Me.counterLabel.TabIndex = 0
        Me.counterLabel.Text = "Counter(s):"
        '
        'countDirectionComboBox
        '
        Me.countDirectionComboBox.Items.AddRange(New Object() {"Count Up", "Count Down", "Externally Controlled"})
        Me.countDirectionComboBox.Location = New System.Drawing.Point(112, 72)
        Me.countDirectionComboBox.Name = "countDirectionComboBox"
        Me.countDirectionComboBox.Size = New System.Drawing.Size(128, 21)
        Me.countDirectionComboBox.TabIndex = 5
        Me.countDirectionComboBox.Text = "Count Up"
        '
        'dataGroupBox
        '
        Me.dataGroupBox.Controls.Add(Me.countLabel)
        Me.dataGroupBox.Controls.Add(Me.countTextBox)
        Me.dataGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.dataGroupBox.Location = New System.Drawing.Point(264, 8)
        Me.dataGroupBox.Name = "dataGroupBox"
        Me.dataGroupBox.Size = New System.Drawing.Size(112, 96)
        Me.dataGroupBox.TabIndex = 3
        Me.dataGroupBox.TabStop = False
        Me.dataGroupBox.Text = "Data"
        '
        'countLabel
        '
        Me.countLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.countLabel.Location = New System.Drawing.Point(16, 24)
        Me.countLabel.Name = "countLabel"
        Me.countLabel.Size = New System.Drawing.Size(40, 16)
        Me.countLabel.TabIndex = 0
        Me.countLabel.Text = "Count:"
        '
        'countTextBox
        '
        Me.countTextBox.Location = New System.Drawing.Point(16, 48)
        Me.countTextBox.Name = "countTextBox"
        Me.countTextBox.ReadOnly = True
        Me.countTextBox.Size = New System.Drawing.Size(80, 20)
        Me.countTextBox.TabIndex = 1
        Me.countTextBox.Text = "0"
        '
        'loopTimer
        '
        '
        'mainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(386, 184)
        Me.Controls.Add(Me.dataGroupBox)
        Me.Controls.Add(Me.stopButton)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.channelParameterGroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "mainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Count Digital Events"
        Me.channelParameterGroupBox.ResumeLayout(False)
        Me.edgeGroupBox.ResumeLayout(False)
        Me.dataGroupBox.ResumeLayout(False)
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

            Dim countDirection As String = countDirectionComboBox.SelectedItem

            Select Case (countDirection)
                Case "Count Up"
                    countDirection = CICountEdgesCountDirection.Up
                Case "Count Down"
                    countDirection = CICountEdgesCountDirection.Down
                Case "Externally Controlled"
                    countDirection = CICountEdgesCountDirection.ExternallyControlled
            End Select

            myTask.CIChannels.CreateCountEdgesChannel(counterComboBox.Text, "Count Edges", _
            edgeType, Convert.ToInt64(initialCountTextBox.Text), countDirection)

            myCICounterReader = New CounterReader(myTask.Stream)

            myTask.Start()
            loopTimer.Enabled = True

        Catch exception As DaqException
            loopTimer.Enabled = False
            MessageBox.Show(exception.Message)
            myTask.Dispose()
            Return

        End Try
        startButton.Enabled = False
        stopButton.Enabled = True

    End Sub

    Private Sub stopButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles stopButton.Click
        loopTimer.Enabled = False
        startButton.Enabled = True
        stopButton.Enabled = False
        myTask.Stop()
        myTask.Dispose()
    End Sub

    Private Sub loopTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles loopTimer.Tick

        Try
            myReading = myCICounterReader.ReadSingleSampleUInt32()
            countTextBox.Text = Convert.ToString(myReading)
        Catch exception As DaqException
            MessageBox.Show(exception.Message)
            myTask.Dispose()
            loopTimer.Enabled = False
            startButton.Enabled = True
            stopButton.Enabled = False
            Return
        End Try
    End Sub

    Private Sub risingRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles risingRadioButton.CheckedChanged
        edgeType = CICountEdgesActiveEdge.Rising
    End Sub

    Private Sub fallingRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles fallingRadioButton.CheckedChanged
        edgeType = CICountEdgesActiveEdge.Falling
    End Sub
End Class
