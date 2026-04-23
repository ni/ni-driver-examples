'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   Gen0_20mACurrent
'
' Category:
'   AO
'
' Description:
'   This example demonstrates how to generate a single current value on a single
'   current output channel of a SCXI-1124 module and NI-6238/6239 M-Series
'   devices.
'
' Instructions for running:
'   1.  Select the physical channel corresponding to where your signal is to be
'       generated on the SCXI-1124 or the NI-6238/6239 M-Series devices.
'   2.  Enter the minimum and maximum current values, in amps.
'   3.  Enter a current value to generate, in amps.
'
' Steps:
'   1.  Create a new Task object.  Use the CreateCurrentChannel method to create
'       an AO channel for current output.
'   2.  Create a AnalogSingleChannelWriter object and use the WriteSingleSample
'       method to generate the current value at the output channel. The
'       autoStart parameter of the WriteSingleSample method is set to true, so
'       that the task is automatically started when the method is called.
'   3.  Dispose the Task object to clean-up any resources associated with the
'       task.
'   4.  Handle any DaqExceptions, if they occur.
'
' I/O Connections Overview:
'   The SCXI-1124 can operate on either an external or internal current source.
'   The only 
'   difference is in the signal connections. When using the internal current
'   source, connect a load between 
'   the SUPPLY and ISINK terminals. When using an external current source, connect
'   the source and load to 
'   the ISINK and GND terminals. In either case, be sure that the channel numbers
'   of the terminals used match 
'   the channel numbers specified in the Physical Channel text box.  For more
'   information on the input and 
'   output terminals for your device, open the NI-DAQmx Help, and refer to the
'   NI-DAQmx Device Terminals 
'   and Device Considerations books in the table of contents. Note: When using an
'   external current source, 
'   be careful to avoid creating an uncontrolled current loop. See the device
'   User's Manual for more information.  
'   The output current can be measured by connecting an ammeter in series with the
'   current loop. Alternatively, 
'   the current can be measured by replacing the load with a resistor of known
'   value. By measuring the voltage 
'   across the resistor and dividing by the resistance, the current through the
'   resistor can be calculated 
'   (Ohm's law).
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
    Friend WithEvents channelParaGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents physicalChanLabel As System.Windows.Forms.Label
    Friend WithEvents maxValueLabel As System.Windows.Forms.Label
    Friend WithEvents minValueLabel As System.Windows.Forms.Label
    Friend WithEvents currentValueLabel As System.Windows.Forms.Label
    Friend WithEvents generateButton As System.Windows.Forms.Button
    Friend WithEvents minValue As System.Windows.Forms.TextBox
    Friend WithEvents maxValue As System.Windows.Forms.TextBox
    Friend WithEvents currentValue As System.Windows.Forms.TextBox
    Friend WithEvents physicalChannelComboBox As System.Windows.Forms.ComboBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.channelParaGroupBox = New System.Windows.Forms.GroupBox
        Me.physicalChannelComboBox = New System.Windows.Forms.ComboBox
        Me.physicalChanLabel = New System.Windows.Forms.Label
        Me.maxValueLabel = New System.Windows.Forms.Label
        Me.minValueLabel = New System.Windows.Forms.Label
        Me.minValue = New System.Windows.Forms.TextBox
        Me.maxValue = New System.Windows.Forms.TextBox
        Me.currentValueLabel = New System.Windows.Forms.Label
        Me.currentValue = New System.Windows.Forms.TextBox
        Me.generateButton = New System.Windows.Forms.Button
        Me.channelParaGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'channelParaGroupBox
        '
        Me.channelParaGroupBox.Controls.Add(Me.physicalChannelComboBox)
        Me.channelParaGroupBox.Controls.Add(Me.physicalChanLabel)
        Me.channelParaGroupBox.Controls.Add(Me.maxValueLabel)
        Me.channelParaGroupBox.Controls.Add(Me.minValueLabel)
        Me.channelParaGroupBox.Controls.Add(Me.minValue)
        Me.channelParaGroupBox.Controls.Add(Me.maxValue)
        Me.channelParaGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.channelParaGroupBox.Location = New System.Drawing.Point(8, 8)
        Me.channelParaGroupBox.Name = "channelParaGroupBox"
        Me.channelParaGroupBox.Size = New System.Drawing.Size(200, 176)
        Me.channelParaGroupBox.TabIndex = 1
        Me.channelParaGroupBox.TabStop = False
        Me.channelParaGroupBox.Text = "Channel Parameters"
        '
        'physicalChannelComboBox
        '
        Me.physicalChannelComboBox.Location = New System.Drawing.Point(16, 40)
        Me.physicalChannelComboBox.Name = "physicalChannelComboBox"
        Me.physicalChannelComboBox.Size = New System.Drawing.Size(168, 21)
        Me.physicalChannelComboBox.TabIndex = 1
        Me.physicalChannelComboBox.Text = "SC1Mod1/ao0"
        '
        'physicalChanLabel
        '
        Me.physicalChanLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.physicalChanLabel.Location = New System.Drawing.Point(16, 24)
        Me.physicalChanLabel.Name = "physicalChanLabel"
        Me.physicalChanLabel.Size = New System.Drawing.Size(100, 16)
        Me.physicalChanLabel.TabIndex = 0
        Me.physicalChanLabel.Text = "Physical Channel:"
        '
        'maxValueLabel
        '
        Me.maxValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.maxValueLabel.Location = New System.Drawing.Point(16, 72)
        Me.maxValueLabel.Name = "maxValueLabel"
        Me.maxValueLabel.Size = New System.Drawing.Size(112, 16)
        Me.maxValueLabel.TabIndex = 2
        Me.maxValueLabel.Text = "Maximum Value (A):"
        '
        'minValueLabel
        '
        Me.minValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.minValueLabel.Location = New System.Drawing.Point(16, 128)
        Me.minValueLabel.Name = "minValueLabel"
        Me.minValueLabel.Size = New System.Drawing.Size(112, 16)
        Me.minValueLabel.TabIndex = 4
        Me.minValueLabel.Text = "Minimum Value (A):"
        '
        'minValue
        '
        Me.minValue.Location = New System.Drawing.Point(16, 144)
        Me.minValue.Name = "minValue"
        Me.minValue.Size = New System.Drawing.Size(168, 20)
        Me.minValue.TabIndex = 5
        Me.minValue.Text = "0.00"
        '
        'maxValue
        '
        Me.maxValue.Location = New System.Drawing.Point(16, 96)
        Me.maxValue.Name = "maxValue"
        Me.maxValue.Size = New System.Drawing.Size(168, 20)
        Me.maxValue.TabIndex = 3
        Me.maxValue.Text = "0.020"
        '
        'currentValueLabel
        '
        Me.currentValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.currentValueLabel.Location = New System.Drawing.Point(224, 32)
        Me.currentValueLabel.Name = "currentValueLabel"
        Me.currentValueLabel.Size = New System.Drawing.Size(100, 16)
        Me.currentValueLabel.TabIndex = 2
        Me.currentValueLabel.Text = "Current Value (A):"
        '
        'currentValue
        '
        Me.currentValue.Location = New System.Drawing.Point(224, 48)
        Me.currentValue.Name = "currentValue"
        Me.currentValue.Size = New System.Drawing.Size(168, 20)
        Me.currentValue.TabIndex = 3
        Me.currentValue.Text = "0.020"
        '
        'generateButton
        '
        Me.generateButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.generateButton.Location = New System.Drawing.Point(248, 152)
        Me.generateButton.Name = "generateButton"
        Me.generateButton.Size = New System.Drawing.Size(125, 24)
        Me.generateButton.TabIndex = 0
        Me.generateButton.Text = "&Generate Current"
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(402, 191)
        Me.Controls.Add(Me.channelParaGroupBox)
        Me.Controls.Add(Me.currentValueLabel)
        Me.Controls.Add(Me.currentValue)
        Me.Controls.Add(Me.generateButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Generate Current"
        Me.channelParaGroupBox.ResumeLayout(False)
        Me.channelParaGroupBox.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private Sub generateButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles generateButton.Click

        System.Windows.Forms.Cursor.Current = Cursors.WaitCursor

        Dim myTask As Task = Nothing

        Try
            myTask = New Task()

            myTask.AOChannels.CreateCurrentChannel( _
                physicalChannelComboBox.Text, _
                "GenCurrent", _
                Convert.ToDouble(minValue.Text), _
                Convert.ToDouble(maxValue.Text), _
                AOCurrentUnits.Amps)

            Dim myChannelWriter As AnalogSingleChannelWriter = _
                New AnalogSingleChannelWriter(myTask.Stream)

            myChannelWriter.WriteSingleSample(True, Convert.ToDouble(currentValue.Text))

        Catch exception As DaqException
            MessageBox.Show(exception.Message)

        Finally
            myTask.Dispose()

            System.Windows.Forms.Cursor.Current = Cursors.Default

        End Try

    End Sub

End Class
