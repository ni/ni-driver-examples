'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   ReadDigPort
'
' Category:
'   DI
'
' Description:
'   This example demonstrates how to read a single value from a digital port.
'
' Instructions for running:
'   1.  Select the digital port on the DAQ device to be read.Note: The data read
'       indicator displays data in hexadecimal format.
'
' Steps:
'   1.  Create a new task and a DIChannel object by calling the CreateChannel
'       method.
'   2.  Create a DigitalSingleChannelReader object to read the data.
'   3.  Read a single sample of digital data from the port as an unsigned
'       integer.
'   4.  Dispose the Task object to clean-up any resources associated with the
'       task.
'   5.  Handle any DaqExceptions, if they occur.
'
' I/O Connections Overview:
'   Make sure your signal input terminals match the port text box. For more
'   information on the input and output terminals for your device, open the
'   NI-DAQmx Help, and refer to the NI-DAQmx Device Terminals and Device
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

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()
        Application.EnableVisualStyles()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        physicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DIPort, PhysicalChannelAccess.External))
        If (physicalChannelComboBox.Items.Count > 0) Then
            physicalChannelComboBox.SelectedIndex = 0
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
    Friend WithEvents channelParametersLabel As System.Windows.Forms.Label
    Friend WithEvents dataReadLabel As System.Windows.Forms.Label
    Friend WithEvents readButton As System.Windows.Forms.Button
    Friend WithEvents physicalChannelGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents dataGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents hexData As System.Windows.Forms.TextBox
    Friend WithEvents physicalChannelComboBox As System.Windows.Forms.ComboBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MainForm))
        Me.channelParametersLabel = New System.Windows.Forms.Label
        Me.readButton = New System.Windows.Forms.Button
        Me.dataReadLabel = New System.Windows.Forms.Label
        Me.hexData = New System.Windows.Forms.TextBox
        Me.dataGroupBox = New System.Windows.Forms.GroupBox
        Me.physicalChannelGroupBox = New System.Windows.Forms.GroupBox
        Me.physicalChannelComboBox = New System.Windows.Forms.ComboBox
        Me.dataGroupBox.SuspendLayout()
        Me.physicalChannelGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'channelParametersLabel
        '
        Me.channelParametersLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.channelParametersLabel.Location = New System.Drawing.Point(16, 24)
        Me.channelParametersLabel.Name = "channelParametersLabel"
        Me.channelParametersLabel.Size = New System.Drawing.Size(32, 16)
        Me.channelParametersLabel.TabIndex = 0
        Me.channelParametersLabel.Text = "Port:"
        '
        'readButton
        '
        Me.readButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.readButton.Location = New System.Drawing.Point(45, 167)
        Me.readButton.Name = "readButton"
        Me.readButton.Size = New System.Drawing.Size(112, 24)
        Me.readButton.TabIndex = 0
        Me.readButton.Text = "&Read"
        '
        'dataReadLabel
        '
        Me.dataReadLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.dataReadLabel.Location = New System.Drawing.Point(16, 24)
        Me.dataReadLabel.Name = "dataReadLabel"
        Me.dataReadLabel.Size = New System.Drawing.Size(64, 16)
        Me.dataReadLabel.TabIndex = 0
        Me.dataReadLabel.Text = "Data Read:"
        '
        'hexData
        '
        Me.hexData.Location = New System.Drawing.Point(80, 24)
        Me.hexData.Name = "hexData"
        Me.hexData.ReadOnly = True
        Me.hexData.Size = New System.Drawing.Size(72, 20)
        Me.hexData.TabIndex = 1
        Me.hexData.Text = "0x0"
        '
        'dataGroupBox
        '
        Me.dataGroupBox.Controls.Add(Me.hexData)
        Me.dataGroupBox.Controls.Add(Me.dataReadLabel)
        Me.dataGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.dataGroupBox.Location = New System.Drawing.Point(17, 95)
        Me.dataGroupBox.Name = "dataGroupBox"
        Me.dataGroupBox.Size = New System.Drawing.Size(168, 56)
        Me.dataGroupBox.TabIndex = 2
        Me.dataGroupBox.TabStop = False
        Me.dataGroupBox.Text = "Data"
        '
        'physicalChannelGroupBox
        '
        Me.physicalChannelGroupBox.Controls.Add(Me.physicalChannelComboBox)
        Me.physicalChannelGroupBox.Controls.Add(Me.channelParametersLabel)
        Me.physicalChannelGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.physicalChannelGroupBox.Location = New System.Drawing.Point(17, 15)
        Me.physicalChannelGroupBox.Name = "physicalChannelGroupBox"
        Me.physicalChannelGroupBox.Size = New System.Drawing.Size(168, 72)
        Me.physicalChannelGroupBox.TabIndex = 1
        Me.physicalChannelGroupBox.TabStop = False
        Me.physicalChannelGroupBox.Text = "Channel Parameters"
        '
        'physicalChannelComboBox
        '
        Me.physicalChannelComboBox.Location = New System.Drawing.Point(16, 40)
        Me.physicalChannelComboBox.Name = "physicalChannelComboBox"
        Me.physicalChannelComboBox.Size = New System.Drawing.Size(136, 21)
        Me.physicalChannelComboBox.TabIndex = 1
        Me.physicalChannelComboBox.Text = "Dev1/port0"
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(210, 207)
        Me.Controls.Add(Me.physicalChannelGroupBox)
        Me.Controls.Add(Me.dataGroupBox)
        Me.Controls.Add(Me.readButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimumSize = New System.Drawing.Size(216, 208)
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Read Digital Port"
        Me.dataGroupBox.ResumeLayout(False)
        Me.physicalChannelGroupBox.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub ReadButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles readButton.Click
        System.Windows.Forms.Cursor.Current = Cursors.WaitCursor

        Dim digitalReadTask As New Task()
        Try
            digitalReadTask.DIChannels.CreateChannel( _
                physicalChannelComboBox.Text, _
                "port0", _
                ChannelLineGrouping.OneChannelForAllLines)

            Dim data As UInt32
            Dim reader As DigitalSingleChannelReader = New DigitalSingleChannelReader(digitalReadTask.Stream)

            data = reader.ReadSingleSamplePortUInt32()

            hexData.Text = String.Format("0x{0:X}", data)

        Catch ex As DaqException
            MessageBox.Show(ex.Message)
        Finally
            digitalReadTask.Dispose()

            System.Windows.Forms.Cursor.Current = Cursors.Default
        End Try
    End Sub

End Class
