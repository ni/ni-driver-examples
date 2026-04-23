'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   ReadDigChan
'
' Category:
'   DI
'
' Description:
'   This example demonstrates how to read values from one or more digital input
'   channels.
'
' Instructions for running:
'   1.  Select the digital lines on the DAQ device to be read.
'   2.  Click the Start button to start reading the digital channels.
'
' Steps:
'   1.  Create a Task object. Create a DIChannel object. Use one channel for all
'       lines.
'   2.  Install a timer to expire every 500 milliseconds. In the timer callback,
'       read the digital data. Continue until the user hits the stop button or
'       an error occurs. Use the DigitalSingleChannelReader object to read from
'       the channel.
'   3.  Call Task.Stop() to stop the task.
'   4.  Dispose the Task object to clean-up any resources associated with the
'       task.
'   5.  Handle any DaqExceptions, if they occur.
'
' I/O Connections Overview:
'   Make sure your signal input terminals match the Lines textbox. In this case
'   connect your digital signals to the first eight digital lines on your DAQ
'   Device. For more information on the input and output terminals for your
'   device, open the NI-DAQmx Help, and refer to the NI-DAQmx Device Terminals
'   and Device Considerations books in the table of contents.
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
    Private myDigitalReader As DigitalSingleChannelReader

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()
        Application.EnableVisualStyles()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        physicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DILine, PhysicalChannelAccess.External))

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
    Friend WithEvents dataReadGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents hexLabel As System.Windows.Forms.Label
    Friend WithEvents hexTextBox As System.Windows.Forms.TextBox
    Friend WithEvents startButton As System.Windows.Forms.Button
    Friend WithEvents channelParamtersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents loopTimer As System.Windows.Forms.Timer
    Friend WithEvents stopButton As System.Windows.Forms.Button
    Friend WithEvents bit0Label As System.Windows.Forms.Label
    Friend WithEvents bit1Label As System.Windows.Forms.Label
    Friend WithEvents bit2Label As System.Windows.Forms.Label
    Friend WithEvents bit3Label As System.Windows.Forms.Label
    Friend WithEvents bit4Label As System.Windows.Forms.Label
    Friend WithEvents bit5Label As System.Windows.Forms.Label
    Friend WithEvents bit6Label As System.Windows.Forms.Label
    Friend WithEvents bit7Label As System.Windows.Forms.Label
    Friend WithEvents linesLabel As System.Windows.Forms.Label
    Friend WithEvents line7CheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents line6CheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents line5CheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents line4CheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents line3CheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents line2CheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents line1CheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents line0CheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents warningLabel As System.Windows.Forms.Label
    Friend WithEvents physicalChannelComboBox As System.Windows.Forms.ComboBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MainForm))
        Me.dataReadGroupBox = New System.Windows.Forms.GroupBox
        Me.warningLabel = New System.Windows.Forms.Label
        Me.bit0Label = New System.Windows.Forms.Label
        Me.bit1Label = New System.Windows.Forms.Label
        Me.bit2Label = New System.Windows.Forms.Label
        Me.bit3Label = New System.Windows.Forms.Label
        Me.bit4Label = New System.Windows.Forms.Label
        Me.bit5Label = New System.Windows.Forms.Label
        Me.bit6Label = New System.Windows.Forms.Label
        Me.bit7Label = New System.Windows.Forms.Label
        Me.line7CheckBox = New System.Windows.Forms.CheckBox
        Me.line6CheckBox = New System.Windows.Forms.CheckBox
        Me.line5CheckBox = New System.Windows.Forms.CheckBox
        Me.line4CheckBox = New System.Windows.Forms.CheckBox
        Me.line3CheckBox = New System.Windows.Forms.CheckBox
        Me.line2CheckBox = New System.Windows.Forms.CheckBox
        Me.line1CheckBox = New System.Windows.Forms.CheckBox
        Me.line0CheckBox = New System.Windows.Forms.CheckBox
        Me.hexLabel = New System.Windows.Forms.Label
        Me.hexTextBox = New System.Windows.Forms.TextBox
        Me.startButton = New System.Windows.Forms.Button
        Me.channelParamtersGroupBox = New System.Windows.Forms.GroupBox
        Me.physicalChannelComboBox = New System.Windows.Forms.ComboBox
        Me.linesLabel = New System.Windows.Forms.Label
        Me.loopTimer = New System.Windows.Forms.Timer(Me.components)
        Me.stopButton = New System.Windows.Forms.Button
        Me.dataReadGroupBox.SuspendLayout()
        Me.channelParamtersGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'dataReadGroupBox
        '
        Me.dataReadGroupBox.Controls.Add(Me.warningLabel)
        Me.dataReadGroupBox.Controls.Add(Me.bit0Label)
        Me.dataReadGroupBox.Controls.Add(Me.bit1Label)
        Me.dataReadGroupBox.Controls.Add(Me.bit2Label)
        Me.dataReadGroupBox.Controls.Add(Me.bit3Label)
        Me.dataReadGroupBox.Controls.Add(Me.bit4Label)
        Me.dataReadGroupBox.Controls.Add(Me.bit5Label)
        Me.dataReadGroupBox.Controls.Add(Me.bit6Label)
        Me.dataReadGroupBox.Controls.Add(Me.bit7Label)
        Me.dataReadGroupBox.Controls.Add(Me.line7CheckBox)
        Me.dataReadGroupBox.Controls.Add(Me.line6CheckBox)
        Me.dataReadGroupBox.Controls.Add(Me.line5CheckBox)
        Me.dataReadGroupBox.Controls.Add(Me.line4CheckBox)
        Me.dataReadGroupBox.Controls.Add(Me.line3CheckBox)
        Me.dataReadGroupBox.Controls.Add(Me.line2CheckBox)
        Me.dataReadGroupBox.Controls.Add(Me.line1CheckBox)
        Me.dataReadGroupBox.Controls.Add(Me.line0CheckBox)
        Me.dataReadGroupBox.Controls.Add(Me.hexLabel)
        Me.dataReadGroupBox.Controls.Add(Me.hexTextBox)
        Me.dataReadGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.dataReadGroupBox.Location = New System.Drawing.Point(8, 88)
        Me.dataReadGroupBox.Name = "dataReadGroupBox"
        Me.dataReadGroupBox.Size = New System.Drawing.Size(216, 136)
        Me.dataReadGroupBox.TabIndex = 3
        Me.dataReadGroupBox.TabStop = False
        Me.dataReadGroupBox.Text = "Data Read"
        '
        'warningLabel
        '
        Me.warningLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.warningLabel.Location = New System.Drawing.Point(28, 104)
        Me.warningLabel.Name = "warningLabel"
        Me.warningLabel.Size = New System.Drawing.Size(160, 16)
        Me.warningLabel.TabIndex = 18
        Me.warningLabel.Text = "Lowermost 7 bits are displayed"
        '
        'bit0Label
        '
        Me.bit0Label.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.bit0Label.Location = New System.Drawing.Point(184, 80)
        Me.bit0Label.Name = "bit0Label"
        Me.bit0Label.Size = New System.Drawing.Size(16, 16)
        Me.bit0Label.TabIndex = 16
        Me.bit0Label.Text = "7"
        '
        'bit1Label
        '
        Me.bit1Label.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.bit1Label.Location = New System.Drawing.Point(160, 80)
        Me.bit1Label.Name = "bit1Label"
        Me.bit1Label.Size = New System.Drawing.Size(16, 16)
        Me.bit1Label.TabIndex = 14
        Me.bit1Label.Text = "6"
        '
        'bit2Label
        '
        Me.bit2Label.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.bit2Label.Location = New System.Drawing.Point(136, 80)
        Me.bit2Label.Name = "bit2Label"
        Me.bit2Label.Size = New System.Drawing.Size(16, 16)
        Me.bit2Label.TabIndex = 12
        Me.bit2Label.Text = "5"
        '
        'bit3Label
        '
        Me.bit3Label.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.bit3Label.Location = New System.Drawing.Point(112, 80)
        Me.bit3Label.Name = "bit3Label"
        Me.bit3Label.Size = New System.Drawing.Size(16, 16)
        Me.bit3Label.TabIndex = 10
        Me.bit3Label.Text = "4"
        '
        'bit4Label
        '
        Me.bit4Label.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.bit4Label.Location = New System.Drawing.Point(88, 80)
        Me.bit4Label.Name = "bit4Label"
        Me.bit4Label.Size = New System.Drawing.Size(16, 16)
        Me.bit4Label.TabIndex = 8
        Me.bit4Label.Text = "3"
        '
        'bit5Label
        '
        Me.bit5Label.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.bit5Label.Location = New System.Drawing.Point(64, 80)
        Me.bit5Label.Name = "bit5Label"
        Me.bit5Label.Size = New System.Drawing.Size(16, 16)
        Me.bit5Label.TabIndex = 6
        Me.bit5Label.Text = "2"
        '
        'bit6Label
        '
        Me.bit6Label.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.bit6Label.Location = New System.Drawing.Point(40, 80)
        Me.bit6Label.Name = "bit6Label"
        Me.bit6Label.Size = New System.Drawing.Size(16, 16)
        Me.bit6Label.TabIndex = 4
        Me.bit6Label.Text = "1"
        '
        'bit7Label
        '
        Me.bit7Label.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.bit7Label.Location = New System.Drawing.Point(16, 80)
        Me.bit7Label.Name = "bit7Label"
        Me.bit7Label.Size = New System.Drawing.Size(8, 16)
        Me.bit7Label.TabIndex = 2
        Me.bit7Label.Text = "0"
        '
        'line7CheckBox
        '
        Me.line7CheckBox.AutoCheck = False
        Me.line7CheckBox.Enabled = False
        Me.line7CheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.line7CheckBox.Location = New System.Drawing.Point(184, 56)
        Me.line7CheckBox.Name = "line7CheckBox"
        Me.line7CheckBox.Size = New System.Drawing.Size(16, 24)
        Me.line7CheckBox.TabIndex = 17
        Me.line7CheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'line6CheckBox
        '
        Me.line6CheckBox.AutoCheck = False
        Me.line6CheckBox.Enabled = False
        Me.line6CheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.line6CheckBox.Location = New System.Drawing.Point(160, 56)
        Me.line6CheckBox.Name = "line6CheckBox"
        Me.line6CheckBox.Size = New System.Drawing.Size(16, 24)
        Me.line6CheckBox.TabIndex = 15
        Me.line6CheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'line5CheckBox
        '
        Me.line5CheckBox.AutoCheck = False
        Me.line5CheckBox.Enabled = False
        Me.line5CheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.line5CheckBox.Location = New System.Drawing.Point(136, 56)
        Me.line5CheckBox.Name = "line5CheckBox"
        Me.line5CheckBox.Size = New System.Drawing.Size(16, 24)
        Me.line5CheckBox.TabIndex = 13
        Me.line5CheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'line4CheckBox
        '
        Me.line4CheckBox.AutoCheck = False
        Me.line4CheckBox.Enabled = False
        Me.line4CheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.line4CheckBox.Location = New System.Drawing.Point(112, 56)
        Me.line4CheckBox.Name = "line4CheckBox"
        Me.line4CheckBox.Size = New System.Drawing.Size(16, 24)
        Me.line4CheckBox.TabIndex = 11
        Me.line4CheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'line3CheckBox
        '
        Me.line3CheckBox.AutoCheck = False
        Me.line3CheckBox.Enabled = False
        Me.line3CheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.line3CheckBox.Location = New System.Drawing.Point(88, 56)
        Me.line3CheckBox.Name = "line3CheckBox"
        Me.line3CheckBox.Size = New System.Drawing.Size(16, 24)
        Me.line3CheckBox.TabIndex = 9
        Me.line3CheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'line2CheckBox
        '
        Me.line2CheckBox.AutoCheck = False
        Me.line2CheckBox.Enabled = False
        Me.line2CheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.line2CheckBox.Location = New System.Drawing.Point(64, 56)
        Me.line2CheckBox.Name = "line2CheckBox"
        Me.line2CheckBox.Size = New System.Drawing.Size(16, 24)
        Me.line2CheckBox.TabIndex = 7
        Me.line2CheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'line1CheckBox
        '
        Me.line1CheckBox.AutoCheck = False
        Me.line1CheckBox.Enabled = False
        Me.line1CheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.line1CheckBox.Location = New System.Drawing.Point(40, 56)
        Me.line1CheckBox.Name = "line1CheckBox"
        Me.line1CheckBox.Size = New System.Drawing.Size(16, 24)
        Me.line1CheckBox.TabIndex = 5
        Me.line1CheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'line0CheckBox
        '
        Me.line0CheckBox.AutoCheck = False
        Me.line0CheckBox.Enabled = False
        Me.line0CheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.line0CheckBox.Location = New System.Drawing.Point(16, 56)
        Me.line0CheckBox.Name = "line0CheckBox"
        Me.line0CheckBox.Size = New System.Drawing.Size(16, 24)
        Me.line0CheckBox.TabIndex = 3
        Me.line0CheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'hexLabel
        '
        Me.hexLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.hexLabel.Location = New System.Drawing.Point(8, 24)
        Me.hexLabel.Name = "hexLabel"
        Me.hexLabel.Size = New System.Drawing.Size(32, 16)
        Me.hexLabel.TabIndex = 0
        Me.hexLabel.Text = "Hex:"
        '
        'hexTextBox
        '
        Me.hexTextBox.BackColor = System.Drawing.SystemColors.InactiveBorder
        Me.hexTextBox.Location = New System.Drawing.Point(40, 24)
        Me.hexTextBox.Name = "hexTextBox"
        Me.hexTextBox.ReadOnly = True
        Me.hexTextBox.Size = New System.Drawing.Size(112, 20)
        Me.hexTextBox.TabIndex = 1
        Me.hexTextBox.Text = "0x0"
        '
        'startButton
        '
        Me.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.startButton.Location = New System.Drawing.Point(45, 240)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(144, 23)
        Me.startButton.TabIndex = 0
        Me.startButton.Text = "Start"
        '
        'channelParamtersGroupBox
        '
        Me.channelParamtersGroupBox.Controls.Add(Me.physicalChannelComboBox)
        Me.channelParamtersGroupBox.Controls.Add(Me.linesLabel)
        Me.channelParamtersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.channelParamtersGroupBox.Location = New System.Drawing.Point(8, 8)
        Me.channelParamtersGroupBox.Name = "channelParamtersGroupBox"
        Me.channelParamtersGroupBox.Size = New System.Drawing.Size(216, 72)
        Me.channelParamtersGroupBox.TabIndex = 2
        Me.channelParamtersGroupBox.TabStop = False
        Me.channelParamtersGroupBox.Text = "Channel Parameters"
        '
        'physicalChannelComboBox
        '
        Me.physicalChannelComboBox.Location = New System.Drawing.Point(16, 40)
        Me.physicalChannelComboBox.Name = "physicalChannelComboBox"
        Me.physicalChannelComboBox.Size = New System.Drawing.Size(184, 21)
        Me.physicalChannelComboBox.TabIndex = 1
        Me.physicalChannelComboBox.Text = "Dev1/port0/line0:7"
        '
        'linesLabel
        '
        Me.linesLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.linesLabel.Location = New System.Drawing.Point(8, 16)
        Me.linesLabel.Name = "linesLabel"
        Me.linesLabel.Size = New System.Drawing.Size(40, 16)
        Me.linesLabel.TabIndex = 0
        Me.linesLabel.Text = "Lines:"
        '
        'loopTimer
        '
        '
        'stopButton
        '
        Me.stopButton.Enabled = False
        Me.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.stopButton.Location = New System.Drawing.Point(45, 272)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(144, 23)
        Me.stopButton.TabIndex = 1
        Me.stopButton.Text = "Stop"
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(234, 312)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.channelParamtersGroupBox)
        Me.Controls.Add(Me.stopButton)
        Me.Controls.Add(Me.dataReadGroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Read Digital Channel"
        Me.dataReadGroupBox.ResumeLayout(False)
        Me.channelParamtersGroupBox.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub startButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles startButton.Click

        System.Windows.Forms.Cursor.Current = Cursors.WaitCursor

        Try

            'Create a task such that it will be disposed after
            'we are done using it.
            myTask = New Task()

            'Create channel
            myTask.DIChannels.CreateChannel(physicalChannelComboBox.Text, "DigRead", _
                ChannelLineGrouping.OneChannelForAllLines)

            myDigitalReader = New DigitalSingleChannelReader(myTask.Stream)

            'enable the timer
            loopTimer.Enabled = True

            startButton.Enabled = False
            stopButton.Enabled = True

        Catch exception As DaqException

            loopTimer.Enabled = False
            MessageBox.Show(exception.Message)

            'dispose task
            myTask.Dispose()

            startButton.Enabled = True
            stopButton.Enabled = False

        Finally
            System.Windows.Forms.Cursor.Current = Cursors.Default

        End Try

    End Sub

    Private Sub stopButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles stopButton.Click

        'disable the timer and dispose of the task
        loopTimer.Enabled = False

        myTask.Dispose()

        startButton.Enabled = True
        stopButton.Enabled = False

    End Sub

    Private Sub loopTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles loopTimer.Tick

        Try

            Dim readData() As Boolean

            'Read the digital channel
            readData = myDigitalReader.ReadSingleSampleMultiLine()

            line0CheckBox.Checked = readData(0)
            line1CheckBox.Checked = readData(1)
            line2CheckBox.Checked = readData(2)
            line3CheckBox.Checked = readData(3)
            line4CheckBox.Checked = readData(4)
            line5CheckBox.Checked = readData(5)
            line6CheckBox.Checked = readData(6)
            line7CheckBox.Checked = readData(7)

            Dim val As Integer
            Dim index As Integer
            For index = 0 To readData.Length - 1

                If readData(index) = True Then
                    'if bit is true
                    'add decimal value of bit
                    val += 1L << index
                End If

            Next

            'display read value in hex
            hexTextBox.Text = String.Format("0x{0:X}", val)


        Catch exception As DaqException

            loopTimer.Enabled = False

            'dispose task
            myTask.Dispose()

            MessageBox.Show(exception.Message)
            startButton.Enabled = True
            stopButton.Enabled = False

        Catch exception As IndexOutOfRangeException

            loopTimer.Enabled = False

            'dispose task
            myTask.Dispose()

            MessageBox.Show("Error: You must specify eight lines in the channel string (i.e., 0:7).")
            startButton.Enabled = True
            stopButton.Enabled = False

        End Try

    End Sub
End Class
