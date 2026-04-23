'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   ReadDigChan_ChangeDetection
'
' Category:
'   DI
'
' Description:
'   This example demonstrates how to read values from one or more digital input
'   channels, using change detection timing.
'
' Instructions for running:
'   1.  Select the digital lines on the DAQ device to be read.
'   2.  Select which digital lines should trigger the completion of a read for
'       rising and falling edges.
'
' Steps:
'   1.  Create a new task and a digital input channel. Use one channel for all
'       lines.
'   2.  Call the Timing.ConfigureChangeDetection method to set up change
'       detection timing for the task.
'   3.  Call Task.Start() to start the task.
'   4.  Call DigitalSingleChannelReader.BeginReadSingleSampleMultiLine to
'       install a callback and begin the asynchronous read operation.
'   5.  Inside the callback, call
'       DigitalSingleChannelReader.EndReadSingleSampleMultiLine to retrieve the
'       data from the read operation.  
'   6.  Call DigitalSingleChannelReader.BeginReadSingleSampleMultiLine again
'       inside the callback to perform another read operation.
'   7.  Dispose the Task object to clean-up any resources associated with the
'       task.
'   8.  Handle any DaqExceptions, if they occur.
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
'   Make sure your signal input terminals match the Lines I/O Control. In this
'   case wire your digital signals to the first eight digital lines on your DAQ
'   Device.  For more information on the input and output terminals for your
'   device, open the NI-DAQmx Help, and refer to the NI-DAQmx Device Terminals
'   and Device Considerations books in the table of contents.NOTE: For NI-6534
'   devices, either 32 bytes of data needs to be transferred first for the DMA
'   transfer to take place, or interrupts must be used instead of DMA.
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

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()
        Application.EnableVisualStyles()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        physicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DILine, PhysicalChannelAccess.External))
        risingEdgeLinesComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DILine Or PhysicalChannelTypes.DIPort, PhysicalChannelAccess.External))
        fallingEdgeLinesComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DILine Or PhysicalChannelTypes.DIPort, PhysicalChannelAccess.External))

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
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents channelParamtersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents linesLabel As System.Windows.Forms.Label
    Friend WithEvents dataReadGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents hexLabel As System.Windows.Forms.Label
    Friend WithEvents hexTextBox As System.Windows.Forms.TextBox
    Friend WithEvents bit7Label As System.Windows.Forms.Label
    Friend WithEvents bit6Label As System.Windows.Forms.Label
    Friend WithEvents bit5Label As System.Windows.Forms.Label
    Friend WithEvents bit4Label As System.Windows.Forms.Label
    Friend WithEvents startButton As System.Windows.Forms.Button
    Friend WithEvents stopButton As System.Windows.Forms.Button
    Friend WithEvents bit3Label As System.Windows.Forms.Label
    Friend WithEvents bit2Label As System.Windows.Forms.Label
    Friend WithEvents bit1Label As System.Windows.Forms.Label
    Friend WithEvents bit0Label As System.Windows.Forms.Label
    Friend WithEvents warningLabel As System.Windows.Forms.Label
    Friend WithEvents line7CheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents line6CheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents line5CheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents line4CheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents line3CheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents line2CheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents line1CheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents line0CheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents physicalChannelComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents risingEdgeLinesComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents fallingEdgeLinesComboBox As System.Windows.Forms.ComboBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MainForm))
        Me.channelParamtersGroupBox = New System.Windows.Forms.GroupBox
        Me.fallingEdgeLinesComboBox = New System.Windows.Forms.ComboBox
        Me.risingEdgeLinesComboBox = New System.Windows.Forms.ComboBox
        Me.physicalChannelComboBox = New System.Windows.Forms.ComboBox
        Me.warningLabel = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.linesLabel = New System.Windows.Forms.Label
        Me.dataReadGroupBox = New System.Windows.Forms.GroupBox
        Me.bit0Label = New System.Windows.Forms.Label
        Me.bit1Label = New System.Windows.Forms.Label
        Me.bit2Label = New System.Windows.Forms.Label
        Me.bit3Label = New System.Windows.Forms.Label
        Me.bit4Label = New System.Windows.Forms.Label
        Me.bit5Label = New System.Windows.Forms.Label
        Me.bit6Label = New System.Windows.Forms.Label
        Me.line7CheckBox = New System.Windows.Forms.CheckBox
        Me.line6CheckBox = New System.Windows.Forms.CheckBox
        Me.line5CheckBox = New System.Windows.Forms.CheckBox
        Me.line4CheckBox = New System.Windows.Forms.CheckBox
        Me.line3CheckBox = New System.Windows.Forms.CheckBox
        Me.line2CheckBox = New System.Windows.Forms.CheckBox
        Me.line1CheckBox = New System.Windows.Forms.CheckBox
        Me.line0CheckBox = New System.Windows.Forms.CheckBox
        Me.hexTextBox = New System.Windows.Forms.TextBox
        Me.hexLabel = New System.Windows.Forms.Label
        Me.bit7Label = New System.Windows.Forms.Label
        Me.startButton = New System.Windows.Forms.Button
        Me.stopButton = New System.Windows.Forms.Button
        Me.channelParamtersGroupBox.SuspendLayout()
        Me.dataReadGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'channelParamtersGroupBox
        '
        Me.channelParamtersGroupBox.Controls.Add(Me.fallingEdgeLinesComboBox)
        Me.channelParamtersGroupBox.Controls.Add(Me.risingEdgeLinesComboBox)
        Me.channelParamtersGroupBox.Controls.Add(Me.physicalChannelComboBox)
        Me.channelParamtersGroupBox.Controls.Add(Me.warningLabel)
        Me.channelParamtersGroupBox.Controls.Add(Me.Label3)
        Me.channelParamtersGroupBox.Controls.Add(Me.Label2)
        Me.channelParamtersGroupBox.Controls.Add(Me.linesLabel)
        Me.channelParamtersGroupBox.Location = New System.Drawing.Point(8, 8)
        Me.channelParamtersGroupBox.Name = "channelParamtersGroupBox"
        Me.channelParamtersGroupBox.Size = New System.Drawing.Size(216, 200)
        Me.channelParamtersGroupBox.TabIndex = 2
        Me.channelParamtersGroupBox.TabStop = False
        Me.channelParamtersGroupBox.Text = "Channel Parameters"
        '
        'fallingEdgeLinesComboBox
        '
        Me.fallingEdgeLinesComboBox.Location = New System.Drawing.Point(8, 168)
        Me.fallingEdgeLinesComboBox.Name = "fallingEdgeLinesComboBox"
        Me.fallingEdgeLinesComboBox.Size = New System.Drawing.Size(200, 21)
        Me.fallingEdgeLinesComboBox.TabIndex = 6
        Me.fallingEdgeLinesComboBox.Text = "Dev1/port0/line0:7"
        '
        'risingEdgeLinesComboBox
        '
        Me.risingEdgeLinesComboBox.Location = New System.Drawing.Point(8, 120)
        Me.risingEdgeLinesComboBox.Name = "risingEdgeLinesComboBox"
        Me.risingEdgeLinesComboBox.Size = New System.Drawing.Size(200, 21)
        Me.risingEdgeLinesComboBox.TabIndex = 4
        Me.risingEdgeLinesComboBox.Text = "Dev1/port0/line0:7"
        '
        'physicalChannelComboBox
        '
        Me.physicalChannelComboBox.Location = New System.Drawing.Point(8, 32)
        Me.physicalChannelComboBox.Name = "physicalChannelComboBox"
        Me.physicalChannelComboBox.Size = New System.Drawing.Size(200, 21)
        Me.physicalChannelComboBox.TabIndex = 1
        Me.physicalChannelComboBox.Text = "Dev1/port0/line0:7"
        '
        'warningLabel
        '
        Me.warningLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.warningLabel.Location = New System.Drawing.Point(8, 64)
        Me.warningLabel.Name = "warningLabel"
        Me.warningLabel.Size = New System.Drawing.Size(200, 32)
        Me.warningLabel.TabIndex = 2
        Me.warningLabel.Text = "You must specify eight lines in the channel string"
        '
        'Label3
        '
        Me.Label3.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label3.Location = New System.Drawing.Point(8, 152)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(112, 16)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Falling Edge Lines:"
        '
        'Label2
        '
        Me.Label2.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label2.Location = New System.Drawing.Point(8, 104)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(100, 16)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Rising Edge Lines:"
        '
        'linesLabel
        '
        Me.linesLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.linesLabel.Location = New System.Drawing.Point(8, 16)
        Me.linesLabel.Name = "linesLabel"
        Me.linesLabel.Size = New System.Drawing.Size(100, 16)
        Me.linesLabel.TabIndex = 0
        Me.linesLabel.Text = "Lines:"
        '
        'dataReadGroupBox
        '
        Me.dataReadGroupBox.Controls.Add(Me.bit0Label)
        Me.dataReadGroupBox.Controls.Add(Me.bit1Label)
        Me.dataReadGroupBox.Controls.Add(Me.bit2Label)
        Me.dataReadGroupBox.Controls.Add(Me.bit3Label)
        Me.dataReadGroupBox.Controls.Add(Me.bit4Label)
        Me.dataReadGroupBox.Controls.Add(Me.bit5Label)
        Me.dataReadGroupBox.Controls.Add(Me.bit6Label)
        Me.dataReadGroupBox.Controls.Add(Me.line7CheckBox)
        Me.dataReadGroupBox.Controls.Add(Me.line6CheckBox)
        Me.dataReadGroupBox.Controls.Add(Me.line5CheckBox)
        Me.dataReadGroupBox.Controls.Add(Me.line4CheckBox)
        Me.dataReadGroupBox.Controls.Add(Me.line3CheckBox)
        Me.dataReadGroupBox.Controls.Add(Me.line2CheckBox)
        Me.dataReadGroupBox.Controls.Add(Me.line1CheckBox)
        Me.dataReadGroupBox.Controls.Add(Me.line0CheckBox)
        Me.dataReadGroupBox.Controls.Add(Me.hexTextBox)
        Me.dataReadGroupBox.Controls.Add(Me.hexLabel)
        Me.dataReadGroupBox.Controls.Add(Me.bit7Label)
        Me.dataReadGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.dataReadGroupBox.Location = New System.Drawing.Point(8, 216)
        Me.dataReadGroupBox.Name = "dataReadGroupBox"
        Me.dataReadGroupBox.Size = New System.Drawing.Size(216, 112)
        Me.dataReadGroupBox.TabIndex = 3
        Me.dataReadGroupBox.TabStop = False
        Me.dataReadGroupBox.Text = "Data Read"
        '
        'bit0Label
        '
        Me.bit0Label.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.bit0Label.Location = New System.Drawing.Point(184, 80)
        Me.bit0Label.Name = "bit0Label"
        Me.bit0Label.Size = New System.Drawing.Size(8, 16)
        Me.bit0Label.TabIndex = 16
        Me.bit0Label.Text = "7"
        '
        'bit1Label
        '
        Me.bit1Label.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.bit1Label.Location = New System.Drawing.Point(160, 80)
        Me.bit1Label.Name = "bit1Label"
        Me.bit1Label.Size = New System.Drawing.Size(8, 16)
        Me.bit1Label.TabIndex = 14
        Me.bit1Label.Text = "6"
        '
        'bit2Label
        '
        Me.bit2Label.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.bit2Label.Location = New System.Drawing.Point(136, 80)
        Me.bit2Label.Name = "bit2Label"
        Me.bit2Label.Size = New System.Drawing.Size(8, 16)
        Me.bit2Label.TabIndex = 12
        Me.bit2Label.Text = "5"
        '
        'bit3Label
        '
        Me.bit3Label.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.bit3Label.Location = New System.Drawing.Point(112, 80)
        Me.bit3Label.Name = "bit3Label"
        Me.bit3Label.Size = New System.Drawing.Size(8, 16)
        Me.bit3Label.TabIndex = 10
        Me.bit3Label.Text = "4"
        '
        'bit4Label
        '
        Me.bit4Label.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.bit4Label.Location = New System.Drawing.Point(88, 80)
        Me.bit4Label.Name = "bit4Label"
        Me.bit4Label.Size = New System.Drawing.Size(8, 16)
        Me.bit4Label.TabIndex = 8
        Me.bit4Label.Text = "3"
        '
        'bit5Label
        '
        Me.bit5Label.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.bit5Label.Location = New System.Drawing.Point(64, 80)
        Me.bit5Label.Name = "bit5Label"
        Me.bit5Label.Size = New System.Drawing.Size(8, 16)
        Me.bit5Label.TabIndex = 6
        Me.bit5Label.Text = "2"
        '
        'bit6Label
        '
        Me.bit6Label.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.bit6Label.Location = New System.Drawing.Point(40, 80)
        Me.bit6Label.Name = "bit6Label"
        Me.bit6Label.Size = New System.Drawing.Size(8, 16)
        Me.bit6Label.TabIndex = 4
        Me.bit6Label.Text = "1"
        '
        'line7CheckBox
        '
        Me.line7CheckBox.Enabled = False
        Me.line7CheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.line7CheckBox.Location = New System.Drawing.Point(184, 56)
        Me.line7CheckBox.Name = "line7CheckBox"
        Me.line7CheckBox.Size = New System.Drawing.Size(16, 24)
        Me.line7CheckBox.TabIndex = 17
        '
        'line6CheckBox
        '
        Me.line6CheckBox.Enabled = False
        Me.line6CheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.line6CheckBox.Location = New System.Drawing.Point(160, 56)
        Me.line6CheckBox.Name = "line6CheckBox"
        Me.line6CheckBox.Size = New System.Drawing.Size(16, 24)
        Me.line6CheckBox.TabIndex = 15
        '
        'line5CheckBox
        '
        Me.line5CheckBox.Enabled = False
        Me.line5CheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.line5CheckBox.Location = New System.Drawing.Point(136, 56)
        Me.line5CheckBox.Name = "line5CheckBox"
        Me.line5CheckBox.Size = New System.Drawing.Size(16, 24)
        Me.line5CheckBox.TabIndex = 13
        '
        'line4CheckBox
        '
        Me.line4CheckBox.Enabled = False
        Me.line4CheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.line4CheckBox.Location = New System.Drawing.Point(112, 56)
        Me.line4CheckBox.Name = "line4CheckBox"
        Me.line4CheckBox.Size = New System.Drawing.Size(16, 24)
        Me.line4CheckBox.TabIndex = 11
        '
        'line3CheckBox
        '
        Me.line3CheckBox.Enabled = False
        Me.line3CheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.line3CheckBox.Location = New System.Drawing.Point(88, 56)
        Me.line3CheckBox.Name = "line3CheckBox"
        Me.line3CheckBox.Size = New System.Drawing.Size(16, 24)
        Me.line3CheckBox.TabIndex = 9
        '
        'line2CheckBox
        '
        Me.line2CheckBox.Enabled = False
        Me.line2CheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.line2CheckBox.Location = New System.Drawing.Point(64, 56)
        Me.line2CheckBox.Name = "line2CheckBox"
        Me.line2CheckBox.Size = New System.Drawing.Size(16, 24)
        Me.line2CheckBox.TabIndex = 7
        '
        'line1CheckBox
        '
        Me.line1CheckBox.Enabled = False
        Me.line1CheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.line1CheckBox.Location = New System.Drawing.Point(40, 56)
        Me.line1CheckBox.Name = "line1CheckBox"
        Me.line1CheckBox.Size = New System.Drawing.Size(16, 24)
        Me.line1CheckBox.TabIndex = 5
        '
        'line0CheckBox
        '
        Me.line0CheckBox.Enabled = False
        Me.line0CheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.line0CheckBox.Location = New System.Drawing.Point(16, 56)
        Me.line0CheckBox.Name = "line0CheckBox"
        Me.line0CheckBox.Size = New System.Drawing.Size(16, 24)
        Me.line0CheckBox.TabIndex = 3
        '
        'hexTextBox
        '
        Me.hexTextBox.BackColor = System.Drawing.SystemColors.InactiveBorder
        Me.hexTextBox.Enabled = False
        Me.hexTextBox.Location = New System.Drawing.Point(40, 24)
        Me.hexTextBox.Name = "hexTextBox"
        Me.hexTextBox.ReadOnly = True
        Me.hexTextBox.TabIndex = 1
        Me.hexTextBox.Text = "0x0"
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
        'bit7Label
        '
        Me.bit7Label.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.bit7Label.Location = New System.Drawing.Point(16, 80)
        Me.bit7Label.Name = "bit7Label"
        Me.bit7Label.Size = New System.Drawing.Size(8, 16)
        Me.bit7Label.TabIndex = 2
        Me.bit7Label.Text = "0"
        '
        'startButton
        '
        Me.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.startButton.Location = New System.Drawing.Point(32, 344)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(152, 24)
        Me.startButton.TabIndex = 0
        Me.startButton.Text = "Start"
        '
        'stopButton
        '
        Me.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.stopButton.Location = New System.Drawing.Point(32, 376)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(152, 24)
        Me.stopButton.TabIndex = 1
        Me.stopButton.Text = "Stop"
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(232, 422)
        Me.Controls.Add(Me.stopButton)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.channelParamtersGroupBox)
        Me.Controls.Add(Me.dataReadGroupBox)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "ReadDigChan-ChangeDetect"
        Me.channelParamtersGroupBox.ResumeLayout(False)
        Me.dataReadGroupBox.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region


    Private runningTask As Task
    Private myTask As Task
    Private myDigitalReader As DigitalSingleChannelReader

    Private Sub startButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles startButton.Click
        System.Windows.Forms.Cursor.Current = Cursors.WaitCursor

        Try
            'Create a task such that it will be disposed after
            'we are done using it.
            myTask = New Task()

            'Create channel
            myTask.DIChannels.CreateChannel(physicalChannelComboBox.Text, "myChannel", ChannelLineGrouping.OneChannelForAllLines)

            myTask.Timing.ConfigureChangeDetection(risingEdgeLinesComboBox.Text, _
            fallingEdgeLinesComboBox.Text, SampleQuantityMode.ContinuousSamples, 1000)

            myDigitalReader = New DigitalSingleChannelReader(myTask.Stream)

            ' Use SynchronizeCallbacks to specify that the object 
            ' marshals callbacks across threads appropriately.
            myDigitalReader.SynchronizeCallbacks = True

            runningTask = myTask
            myDigitalReader.BeginReadSingleSampleMultiLine(New AsyncCallback(AddressOf OnDataReady), myTask)

            startButton.Enabled = False
            stopButton.Enabled = True
            physicalChannelComboBox.Enabled = False
            risingEdgeLinesComboBox.Enabled = False
            fallingEdgeLinesComboBox.Enabled = False

        Catch exception As DaqException
            MessageBox.Show(exception.Message)

            'dispose task
            If Not (myTask Is Nothing) Then
                myTask.Dispose()
            End If

        End try
        System.Windows.Forms.Cursor.Current = Cursors.Default
    End Sub


    Private Sub DisplayData(ByVal readData() As Boolean)
        Dim val As Long = 0
        Dim i As Integer

        Try
            line0CheckBox.Checked = readData(0)
            line1CheckBox.Checked = readData(1)
            line2CheckBox.Checked = readData(2)
            line3CheckBox.Checked = readData(3)
            line4CheckBox.Checked = readData(4)
            line5CheckBox.Checked = readData(5)
            line6CheckBox.Checked = readData(6)
            line7CheckBox.Checked = readData(7)

            For i = 0 To readData.Length - 1
                If readData(i) Then
                    'if bit is true
                    'add decimal value of bit
                    val += 1L << i
                End If
            Next i

            'display read value in hex
            hexTextBox.Text = String.Format("0x{0:X}", val)
        Catch ex As IndexOutOfRangeException

            'dispose task
            myTask.Dispose()

            MessageBox.Show("Error: You must specify eight lines in the channel string (i.e., 0:7).")
            startButton.Enabled = True
            stopButton.Enabled = False
        End Try
    End Sub

    Private Sub OnDataReady(ByVal result As IAsyncResult)
        Try
            If (Not (runningTask Is Nothing)) AndAlso runningTask Is result.AsyncState Then
                Dim data As Boolean() = myDigitalReader.EndReadSingleSampleMultiLine(result)
                DisplayData(data)

                myDigitalReader.BeginReadSingleSampleMultiLine(New AsyncCallback(AddressOf OnDataReady), myTask)
            End If
        Catch exception As DaqException
            runningTask = Nothing
            MessageBox.Show(exception.Message)
            myDigitalReader = Nothing
            myTask.Dispose()
            startButton.Enabled = True
            stopButton.Enabled = False
            physicalChannelComboBox.Enabled = True
            risingEdgeLinesComboBox.Enabled = True
            fallingEdgeLinesComboBox.Enabled = True
        End Try
    End Sub

    Private Sub stopButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles stopButton.Click
        runningTask = Nothing
        myTask.Dispose()
        startButton.Enabled = True
        stopButton.Enabled = False
        physicalChannelComboBox.Enabled = True
        risingEdgeLinesComboBox.Enabled = True
        fallingEdgeLinesComboBox.Enabled = True
    End Sub
End Class
