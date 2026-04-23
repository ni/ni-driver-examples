'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   GenVoltageUpdate
'
' Category:
'   AO
'
' Description:
'   This example demonstrates how to output a single voltage update (sample) to
'   an analog output channel.
'
' Instructions for running:
'   1.  Select the physical channel corresponding to where your signal is output
'       on the DAQ device.
'   2.  Enter the minimum and maximum voltage values.
'
' Steps:
'   1.  Create a new task and an analog output voltage channel.
'   2.  Create a AnalogSingleChannelWriter and call the WriteSingleSample method
'       to output a single sample to your DAQ device.
'   3.  Dispose the Task object to clean-up any resources associated with the
'       task.
'   4.  Handle any DaqExceptions, if they occur.
'
' I/O Connections Overview:
'   Make sure your signal output terminal matches the text in the physical
'   channel text box. In this case the signal will output to the ao0 pin on your
'   DAQ Device.  For more information on the input and output terminals for your
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

        physicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AO, PhysicalChannelAccess.External))
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
    Friend WithEvents startButton As System.Windows.Forms.Button
    Friend WithEvents voltageOutputLabel As System.Windows.Forms.Label
    Friend WithEvents channelParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents maximumValueLabel As System.Windows.Forms.Label
    Friend WithEvents minimumValueLabel As System.Windows.Forms.Label
    Friend WithEvents physicalChannelLabel As System.Windows.Forms.Label
    Friend WithEvents voltageOutput As System.Windows.Forms.TextBox
    Friend WithEvents maximumValue As System.Windows.Forms.TextBox
    Friend WithEvents minimumValue As System.Windows.Forms.TextBox
    Friend WithEvents physicalChannelComboBox As System.Windows.Forms.ComboBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MainForm))
        Me.voltageOutput = New System.Windows.Forms.TextBox
        Me.startButton = New System.Windows.Forms.Button
        Me.voltageOutputLabel = New System.Windows.Forms.Label
        Me.channelParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.physicalChannelComboBox = New System.Windows.Forms.ComboBox
        Me.maximumValue = New System.Windows.Forms.TextBox
        Me.minimumValue = New System.Windows.Forms.TextBox
        Me.maximumValueLabel = New System.Windows.Forms.Label
        Me.minimumValueLabel = New System.Windows.Forms.Label
        Me.physicalChannelLabel = New System.Windows.Forms.Label
        Me.channelParametersGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'voltageOutput
        '
        Me.voltageOutput.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.voltageOutput.Location = New System.Drawing.Point(128, 152)
        Me.voltageOutput.Name = "voltageOutput"
        Me.voltageOutput.Size = New System.Drawing.Size(168, 20)
        Me.voltageOutput.TabIndex = 3
        Me.voltageOutput.Text = "4.73"
        '
        'startButton
        '
        Me.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.startButton.Location = New System.Drawing.Point(123, 184)
        Me.startButton.Name = "startButton"
        Me.startButton.TabIndex = 0
        Me.startButton.Text = "&Start"
        '
        'voltageOutputLabel
        '
        Me.voltageOutputLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.voltageOutputLabel.Location = New System.Drawing.Point(24, 152)
        Me.voltageOutputLabel.Name = "voltageOutputLabel"
        Me.voltageOutputLabel.Size = New System.Drawing.Size(104, 16)
        Me.voltageOutputLabel.TabIndex = 2
        Me.voltageOutputLabel.Text = "Voltage Output (V):"
        '
        'channelParametersGroupBox
        '
        Me.channelParametersGroupBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.channelParametersGroupBox.Controls.Add(Me.physicalChannelComboBox)
        Me.channelParametersGroupBox.Controls.Add(Me.maximumValue)
        Me.channelParametersGroupBox.Controls.Add(Me.minimumValue)
        Me.channelParametersGroupBox.Controls.Add(Me.maximumValueLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.minimumValueLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.physicalChannelLabel)
        Me.channelParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.channelParametersGroupBox.Location = New System.Drawing.Point(8, 8)
        Me.channelParametersGroupBox.Name = "channelParametersGroupBox"
        Me.channelParametersGroupBox.Size = New System.Drawing.Size(304, 128)
        Me.channelParametersGroupBox.TabIndex = 1
        Me.channelParametersGroupBox.TabStop = False
        Me.channelParametersGroupBox.Text = "Channel Parameters"
        '
        'physicalChannelComboBox
        '
        Me.physicalChannelComboBox.Location = New System.Drawing.Point(120, 24)
        Me.physicalChannelComboBox.Name = "physicalChannelComboBox"
        Me.physicalChannelComboBox.Size = New System.Drawing.Size(168, 21)
        Me.physicalChannelComboBox.TabIndex = 1
        Me.physicalChannelComboBox.Text = "Dev1/ao0"
        '
        'maximumValue
        '
        Me.maximumValue.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.maximumValue.Location = New System.Drawing.Point(120, 96)
        Me.maximumValue.Name = "maximumValue"
        Me.maximumValue.Size = New System.Drawing.Size(168, 20)
        Me.maximumValue.TabIndex = 5
        Me.maximumValue.Text = "10"
        '
        'minimumValue
        '
        Me.minimumValue.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.minimumValue.Location = New System.Drawing.Point(120, 60)
        Me.minimumValue.Name = "minimumValue"
        Me.minimumValue.Size = New System.Drawing.Size(168, 20)
        Me.minimumValue.TabIndex = 3
        Me.minimumValue.Text = "-10"
        '
        'maximumValueLabel
        '
        Me.maximumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.maximumValueLabel.Location = New System.Drawing.Point(16, 96)
        Me.maximumValueLabel.Name = "maximumValueLabel"
        Me.maximumValueLabel.Size = New System.Drawing.Size(112, 16)
        Me.maximumValueLabel.TabIndex = 4
        Me.maximumValueLabel.Text = "Maximum Value (V):"
        '
        'minimumValueLabel
        '
        Me.minimumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.minimumValueLabel.Location = New System.Drawing.Point(16, 62)
        Me.minimumValueLabel.Name = "minimumValueLabel"
        Me.minimumValueLabel.Size = New System.Drawing.Size(104, 16)
        Me.minimumValueLabel.TabIndex = 2
        Me.minimumValueLabel.Text = "Minimum Value (V):"
        '
        'physicalChannelLabel
        '
        Me.physicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.physicalChannelLabel.Location = New System.Drawing.Point(16, 26)
        Me.physicalChannelLabel.Name = "physicalChannelLabel"
        Me.physicalChannelLabel.Size = New System.Drawing.Size(96, 16)
        Me.physicalChannelLabel.TabIndex = 0
        Me.physicalChannelLabel.Text = "Physical Channel:"
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(320, 222)
        Me.Controls.Add(Me.voltageOutput)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.voltageOutputLabel)
        Me.Controls.Add(Me.channelParametersGroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(600, 256)
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Generate Voltage Update"
        Me.channelParametersGroupBox.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub startButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles startButton.Click

        System.Windows.Forms.Cursor.Current = Cursors.WaitCursor
        Dim myTask As Task = Nothing

        Try
            myTask = New Task()
            myTask.AOChannels.CreateVoltageChannel( _
                physicalChannelComboBox.Text, _
                "aoChannel", _
                Convert.ToDouble(minimumValue.Text), _
                Convert.ToDouble(maximumValue.Text), _
                AOVoltageUnits.Volts)

            Dim writer As AnalogSingleChannelWriter = New AnalogSingleChannelWriter(myTask.Stream)
            writer.WriteSingleSample(True, Convert.ToDouble(voltageOutput.Text))
        Catch ex As DaqException
            MessageBox.Show(ex.Message)
        Finally
            myTask.Dispose()
        End Try
        System.Windows.Forms.Cursor.Current = Cursors.Default
    End Sub
End Class
