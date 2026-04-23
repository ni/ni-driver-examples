'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   WriteDigChan
'
' Category:
'   DO
'
' Description:
'   This example demonstrates how to write values to a digital output channel.
'
' Instructions for running:
'   1.  Select the channel parameters on the DAQ device to be written.Note: You
'       must specify exactly 8 lines in the channel string box.
'   2.  Use the checkboxes to select a value to write.
'
' Steps:
'   1.  Create a new task and a digital output channel.
'   2.  Create a DigitalSingleChannelWriter and call the
'       WriteSingleSampleMultiLine method to write the data to the channel.
'   3.  Dispose the Task object to clean-up any resources associated with the
'       task.
'   4.  Handle any DaqExceptions, if they occur.
'
' I/O Connections Overview:
'   Make sure your signal output terminals match the Lines text box. In this
'   case wire the item to receive the signal to the specified eight digital
'   lines on your DAQ Device.  For more information on the input and output
'   terminals for your device, open the NI-DAQmx Help, and refer to the NI-DAQmx
'   Device Terminals and Device Considerations books in the table of contents.
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

Public Class MainForm
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()
        Application.EnableVisualStyles()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        physicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DOLine, PhysicalChannelAccess.External))

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
    Friend WithEvents bit0CheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents bit1CheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents bit3CheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents bit4CheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents bit5CheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents bit6CheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents bit7CheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents dataToWriteLabel As System.Windows.Forms.Label
    Friend WithEvents bit2CheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents bit7Label As System.Windows.Forms.Label
    Friend WithEvents bit6Label As System.Windows.Forms.Label
    Friend WithEvents bit5Label As System.Windows.Forms.Label
    Friend WithEvents bit4Label As System.Windows.Forms.Label
    Friend WithEvents bit3Label As System.Windows.Forms.Label
    Friend WithEvents bit2Label As System.Windows.Forms.Label
    Friend WithEvents bit1Label As System.Windows.Forms.Label
    Friend WithEvents bit0Label As System.Windows.Forms.Label
    Friend WithEvents channelParamsLabel As System.Windows.Forms.Label
    Friend WithEvents writeButton As System.Windows.Forms.Button
    Friend WithEvents warningLabel As System.Windows.Forms.Label
    Friend WithEvents physicalChannelComboBox As System.Windows.Forms.ComboBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MainForm))
        Me.bit7Label = New System.Windows.Forms.Label
        Me.bit6Label = New System.Windows.Forms.Label
        Me.bit5Label = New System.Windows.Forms.Label
        Me.bit4Label = New System.Windows.Forms.Label
        Me.bit3Label = New System.Windows.Forms.Label
        Me.bit2Label = New System.Windows.Forms.Label
        Me.bit1Label = New System.Windows.Forms.Label
        Me.bit0Label = New System.Windows.Forms.Label
        Me.bit0CheckBox = New System.Windows.Forms.CheckBox
        Me.bit1CheckBox = New System.Windows.Forms.CheckBox
        Me.bit3CheckBox = New System.Windows.Forms.CheckBox
        Me.bit4CheckBox = New System.Windows.Forms.CheckBox
        Me.bit5CheckBox = New System.Windows.Forms.CheckBox
        Me.bit6CheckBox = New System.Windows.Forms.CheckBox
        Me.bit7CheckBox = New System.Windows.Forms.CheckBox
        Me.channelParamsLabel = New System.Windows.Forms.Label
        Me.writeButton = New System.Windows.Forms.Button
        Me.dataToWriteLabel = New System.Windows.Forms.Label
        Me.bit2CheckBox = New System.Windows.Forms.CheckBox
        Me.warningLabel = New System.Windows.Forms.Label
        Me.physicalChannelComboBox = New System.Windows.Forms.ComboBox
        Me.SuspendLayout()
        '
        'bit7Label
        '
        Me.bit7Label.Location = New System.Drawing.Point(171, 160)
        Me.bit7Label.Name = "bit7Label"
        Me.bit7Label.Size = New System.Drawing.Size(16, 16)
        Me.bit7Label.TabIndex = 19
        Me.bit7Label.Text = "7"
        '
        'bit6Label
        '
        Me.bit6Label.Location = New System.Drawing.Point(150, 160)
        Me.bit6Label.Name = "bit6Label"
        Me.bit6Label.Size = New System.Drawing.Size(16, 16)
        Me.bit6Label.TabIndex = 17
        Me.bit6Label.Text = "6"
        '
        'bit5Label
        '
        Me.bit5Label.Location = New System.Drawing.Point(129, 160)
        Me.bit5Label.Name = "bit5Label"
        Me.bit5Label.Size = New System.Drawing.Size(16, 16)
        Me.bit5Label.TabIndex = 15
        Me.bit5Label.Text = "5"
        '
        'bit4Label
        '
        Me.bit4Label.Location = New System.Drawing.Point(108, 160)
        Me.bit4Label.Name = "bit4Label"
        Me.bit4Label.Size = New System.Drawing.Size(16, 16)
        Me.bit4Label.TabIndex = 13
        Me.bit4Label.Text = "4"
        '
        'bit3Label
        '
        Me.bit3Label.Location = New System.Drawing.Point(87, 160)
        Me.bit3Label.Name = "bit3Label"
        Me.bit3Label.Size = New System.Drawing.Size(16, 16)
        Me.bit3Label.TabIndex = 11
        Me.bit3Label.Text = "3"
        '
        'bit2Label
        '
        Me.bit2Label.Location = New System.Drawing.Point(66, 160)
        Me.bit2Label.Name = "bit2Label"
        Me.bit2Label.Size = New System.Drawing.Size(16, 16)
        Me.bit2Label.TabIndex = 9
        Me.bit2Label.Text = "2"
        '
        'bit1Label
        '
        Me.bit1Label.Location = New System.Drawing.Point(45, 160)
        Me.bit1Label.Name = "bit1Label"
        Me.bit1Label.Size = New System.Drawing.Size(16, 16)
        Me.bit1Label.TabIndex = 7
        Me.bit1Label.Text = "1"
        '
        'bit0Label
        '
        Me.bit0Label.Location = New System.Drawing.Point(24, 160)
        Me.bit0Label.Name = "bit0Label"
        Me.bit0Label.Size = New System.Drawing.Size(16, 16)
        Me.bit0Label.TabIndex = 5
        Me.bit0Label.Text = "0"
        '
        'bit0CheckBox
        '
        Me.bit0CheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.bit0CheckBox.Location = New System.Drawing.Point(24, 136)
        Me.bit0CheckBox.Name = "bit0CheckBox"
        Me.bit0CheckBox.Size = New System.Drawing.Size(16, 16)
        Me.bit0CheckBox.TabIndex = 6
        Me.bit0CheckBox.Text = "Line0"
        '
        'bit1CheckBox
        '
        Me.bit1CheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.bit1CheckBox.Location = New System.Drawing.Point(45, 136)
        Me.bit1CheckBox.Name = "bit1CheckBox"
        Me.bit1CheckBox.Size = New System.Drawing.Size(16, 16)
        Me.bit1CheckBox.TabIndex = 8
        Me.bit1CheckBox.Text = "Line1"
        '
        'bit3CheckBox
        '
        Me.bit3CheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.bit3CheckBox.Location = New System.Drawing.Point(87, 136)
        Me.bit3CheckBox.Name = "bit3CheckBox"
        Me.bit3CheckBox.Size = New System.Drawing.Size(16, 16)
        Me.bit3CheckBox.TabIndex = 12
        Me.bit3CheckBox.Text = "Line3"
        '
        'bit4CheckBox
        '
        Me.bit4CheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.bit4CheckBox.Location = New System.Drawing.Point(108, 136)
        Me.bit4CheckBox.Name = "bit4CheckBox"
        Me.bit4CheckBox.Size = New System.Drawing.Size(16, 16)
        Me.bit4CheckBox.TabIndex = 14
        Me.bit4CheckBox.Text = "Line4"
        '
        'bit5CheckBox
        '
        Me.bit5CheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.bit5CheckBox.Location = New System.Drawing.Point(129, 136)
        Me.bit5CheckBox.Name = "bit5CheckBox"
        Me.bit5CheckBox.Size = New System.Drawing.Size(16, 16)
        Me.bit5CheckBox.TabIndex = 16
        Me.bit5CheckBox.Text = "Line5"
        '
        'bit6CheckBox
        '
        Me.bit6CheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.bit6CheckBox.Location = New System.Drawing.Point(150, 136)
        Me.bit6CheckBox.Name = "bit6CheckBox"
        Me.bit6CheckBox.Size = New System.Drawing.Size(16, 16)
        Me.bit6CheckBox.TabIndex = 18
        Me.bit6CheckBox.Text = "Line6"
        '
        'bit7CheckBox
        '
        Me.bit7CheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.bit7CheckBox.Location = New System.Drawing.Point(171, 136)
        Me.bit7CheckBox.Name = "bit7CheckBox"
        Me.bit7CheckBox.Size = New System.Drawing.Size(16, 16)
        Me.bit7CheckBox.TabIndex = 20
        Me.bit7CheckBox.Text = "Line7"
        '
        'channelParamsLabel
        '
        Me.channelParamsLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.channelParamsLabel.Location = New System.Drawing.Point(16, 8)
        Me.channelParamsLabel.Name = "channelParamsLabel"
        Me.channelParamsLabel.Size = New System.Drawing.Size(112, 16)
        Me.channelParamsLabel.TabIndex = 1
        Me.channelParamsLabel.Text = "Lines:"
        '
        'writeButton
        '
        Me.writeButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.writeButton.Location = New System.Drawing.Point(64, 192)
        Me.writeButton.Name = "writeButton"
        Me.writeButton.Size = New System.Drawing.Size(80, 24)
        Me.writeButton.TabIndex = 0
        Me.writeButton.Text = "&Write"
        '
        'dataToWriteLabel
        '
        Me.dataToWriteLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.dataToWriteLabel.Location = New System.Drawing.Point(16, 112)
        Me.dataToWriteLabel.Name = "dataToWriteLabel"
        Me.dataToWriteLabel.Size = New System.Drawing.Size(128, 16)
        Me.dataToWriteLabel.TabIndex = 4
        Me.dataToWriteLabel.Text = "Data to Write:"
        '
        'bit2CheckBox
        '
        Me.bit2CheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.bit2CheckBox.Location = New System.Drawing.Point(66, 136)
        Me.bit2CheckBox.Name = "bit2CheckBox"
        Me.bit2CheckBox.Size = New System.Drawing.Size(16, 16)
        Me.bit2CheckBox.TabIndex = 10
        Me.bit2CheckBox.Text = "Line3"
        '
        'warningLabel
        '
        Me.warningLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.warningLabel.Location = New System.Drawing.Point(16, 56)
        Me.warningLabel.Name = "warningLabel"
        Me.warningLabel.Size = New System.Drawing.Size(184, 48)
        Me.warningLabel.TabIndex = 3
        Me.warningLabel.Text = "You must specify eight lines in the channel string"
        '
        'physicalChannelComboBox
        '
        Me.physicalChannelComboBox.Location = New System.Drawing.Point(16, 24)
        Me.physicalChannelComboBox.Name = "physicalChannelComboBox"
        Me.physicalChannelComboBox.Size = New System.Drawing.Size(176, 21)
        Me.physicalChannelComboBox.TabIndex = 2
        Me.physicalChannelComboBox.Text = "Dev1/Port0/line0:7"
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(210, 232)
        Me.Controls.Add(Me.physicalChannelComboBox)
        Me.Controls.Add(Me.warningLabel)
        Me.Controls.Add(Me.bit7Label)
        Me.Controls.Add(Me.bit6Label)
        Me.Controls.Add(Me.bit5Label)
        Me.Controls.Add(Me.bit4Label)
        Me.Controls.Add(Me.bit3Label)
        Me.Controls.Add(Me.bit2Label)
        Me.Controls.Add(Me.bit1Label)
        Me.Controls.Add(Me.bit0Label)
        Me.Controls.Add(Me.bit0CheckBox)
        Me.Controls.Add(Me.bit1CheckBox)
        Me.Controls.Add(Me.bit3CheckBox)
        Me.Controls.Add(Me.bit4CheckBox)
        Me.Controls.Add(Me.bit5CheckBox)
        Me.Controls.Add(Me.bit6CheckBox)
        Me.Controls.Add(Me.bit7CheckBox)
        Me.Controls.Add(Me.channelParamsLabel)
        Me.Controls.Add(Me.writeButton)
        Me.Controls.Add(Me.dataToWriteLabel)
        Me.Controls.Add(Me.bit2CheckBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Write Dig Channel"
        Me.ResumeLayout(False)

    End Sub

#End Region


    Private Sub CheckLines(ByRef dataArray() As Boolean)
        dataArray(0) = bit0CheckBox.Checked
        dataArray(1) = bit1CheckBox.Checked
        dataArray(2) = bit2CheckBox.Checked
        dataArray(3) = bit3CheckBox.Checked
        dataArray(4) = bit4CheckBox.Checked
        dataArray(5) = bit5CheckBox.Checked
        dataArray(6) = bit6CheckBox.Checked
        dataArray(7) = bit7CheckBox.Checked
    End Sub


    Private Sub writeButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles writeButton.Click
        System.Windows.Forms.Cursor.Current = Cursors.WaitCursor
        Dim digitalWriteTask As Task = Nothing

        Try
            digitalWriteTask = New Task()
            digitalWriteTask.DOChannels.CreateChannel(physicalChannelComboBox.Text, "port0", _
                   ChannelLineGrouping.OneChannelForAllLines)

            Dim dataArray(7) As Boolean
            CheckLines(dataArray)

            Dim writer As New DigitalSingleChannelWriter(digitalWriteTask.Stream)
            writer.WriteSingleSampleMultiLine(True, dataArray)

        Catch exception As DaqException
            MessageBox.Show(exception.Message)

        Finally
            digitalWriteTask.Dispose()
            System.Windows.Forms.Cursor.Current = Cursors.Default

        End Try
    End Sub

End Class
