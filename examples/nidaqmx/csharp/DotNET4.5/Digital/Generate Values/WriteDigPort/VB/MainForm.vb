'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   WriteDigPort
'
' Category:
'   DO
'
' Description:
'   This example demonstrates how to write values to a digital output port.
'
' Instructions for running:
'   1.  Select the digital port on the DAQ device to be written.
'   2.  Select a value to write.
'
' Steps:
'   1.  Create a new task and a digital output channel.
'   2.  Create a DigitalSingleChannelWriter and call the WriteSingleSamplePort
'       method to write the data to the digital port.
'   3.  Dispose the Task object to clean-up any resources associated with the
'       task.
'   4.  Handle any DaqExceptions, if they occur.
'
' I/O Connections Overview:
'   Make sure your signal output terminals match the ports text box. In this
'   case wire the item to receive the signal to the digital lines on your DAQ
'   Device.  For more information on the input and output terminals for your
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

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()
        Application.EnableVisualStyles()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        physicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DOPort, PhysicalChannelAccess.External))
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
    Friend WithEvents writeButton As System.Windows.Forms.Button
    Friend WithEvents dataLabel As System.Windows.Forms.Label
    Friend WithEvents channelParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents physicalChannelLabel As System.Windows.Forms.Label
    Friend WithEvents dataToWriteNumericUpDown As System.Windows.Forms.NumericUpDown
    Friend WithEvents physicalChannelComboBox As System.Windows.Forms.ComboBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MainForm))
        Me.writeButton = New System.Windows.Forms.Button
        Me.dataLabel = New System.Windows.Forms.Label
        Me.channelParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.physicalChannelComboBox = New System.Windows.Forms.ComboBox
        Me.physicalChannelLabel = New System.Windows.Forms.Label
        Me.dataToWriteNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.channelParametersGroupBox.SuspendLayout()
        CType(Me.dataToWriteNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'writeButton
        '
        Me.writeButton.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.writeButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.writeButton.Location = New System.Drawing.Point(112, 160)
        Me.writeButton.Name = "writeButton"
        Me.writeButton.TabIndex = 0
        Me.writeButton.Text = "&Write"
        '
        'dataLabel
        '
        Me.dataLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.dataLabel.Location = New System.Drawing.Point(24, 104)
        Me.dataLabel.Name = "dataLabel"
        Me.dataLabel.Size = New System.Drawing.Size(80, 16)
        Me.dataLabel.TabIndex = 2
        Me.dataLabel.Text = "Data to Write:"
        '
        'channelParametersGroupBox
        '
        Me.channelParametersGroupBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.channelParametersGroupBox.Controls.Add(Me.physicalChannelComboBox)
        Me.channelParametersGroupBox.Controls.Add(Me.physicalChannelLabel)
        Me.channelParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.channelParametersGroupBox.Location = New System.Drawing.Point(8, 8)
        Me.channelParametersGroupBox.Name = "channelParametersGroupBox"
        Me.channelParametersGroupBox.Size = New System.Drawing.Size(288, 80)
        Me.channelParametersGroupBox.TabIndex = 1
        Me.channelParametersGroupBox.TabStop = False
        Me.channelParametersGroupBox.Text = "Channel Parameters"
        '
        'physicalChannelComboBox
        '
        Me.physicalChannelComboBox.Location = New System.Drawing.Point(16, 48)
        Me.physicalChannelComboBox.Name = "physicalChannelComboBox"
        Me.physicalChannelComboBox.Size = New System.Drawing.Size(256, 21)
        Me.physicalChannelComboBox.TabIndex = 1
        Me.physicalChannelComboBox.Text = "Dev1/port0"
        '
        'physicalChannelLabel
        '
        Me.physicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.physicalChannelLabel.Location = New System.Drawing.Point(16, 24)
        Me.physicalChannelLabel.Name = "physicalChannelLabel"
        Me.physicalChannelLabel.Size = New System.Drawing.Size(96, 14)
        Me.physicalChannelLabel.TabIndex = 0
        Me.physicalChannelLabel.Text = "Port"
        '
        'dataToWriteNumericUpDown
        '
        Me.dataToWriteNumericUpDown.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dataToWriteNumericUpDown.Location = New System.Drawing.Point(24, 120)
        Me.dataToWriteNumericUpDown.Maximum = New Decimal(New Integer() {0, 1, 0, 0})
        Me.dataToWriteNumericUpDown.Name = "dataToWriteNumericUpDown"
        Me.dataToWriteNumericUpDown.Size = New System.Drawing.Size(256, 20)
        Me.dataToWriteNumericUpDown.TabIndex = 3
        Me.dataToWriteNumericUpDown.Value = New Decimal(New Integer() {255, 0, 0, 0})
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(306, 192)
        Me.Controls.Add(Me.writeButton)
        Me.Controls.Add(Me.dataLabel)
        Me.Controls.Add(Me.channelParametersGroupBox)
        Me.Controls.Add(Me.dataToWriteNumericUpDown)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Write To Digital Port"
        Me.channelParametersGroupBox.ResumeLayout(False)
        CType(Me.dataToWriteNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub writeButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles writeButton.Click
        Dim digitalWriteTask As Task = New Task()
        Windows.Forms.Cursor.Current = Cursors.WaitCursor
        Try
            '  Create an Digital Output channel and name it.
            digitalWriteTask.DOChannels.CreateChannel( _
                physicalChannelComboBox.Text, _
                "port0", _
                ChannelLineGrouping.OneChannelForAllLines)

            '  Write digital port data. WriteDigitalSingChanSingSampPort writes a single sample
            '  of digital data on demand, so no timeout is necessary.
            Dim writer As DigitalSingleChannelWriter = New DigitalSingleChannelWriter(digitalWriteTask.Stream)
            writer.WriteSingleSamplePort(True, Decimal.ToUInt32(dataToWriteNumericUpDown.Value))
        Catch ex As System.Exception
            MessageBox.Show(ex.Message)
        Finally
            System.Windows.Forms.Cursor.Current = Cursors.Default
            digitalWriteTask.Dispose()
        End Try
    End Sub
End Class
