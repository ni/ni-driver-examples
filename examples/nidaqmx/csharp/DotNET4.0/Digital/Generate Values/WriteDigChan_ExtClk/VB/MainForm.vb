'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   WriteDigChan_ExtClk
'
' Category:
'   DO
'
' Description:
'   This example demonstrates how to write values to a digital output channel
'   using an external sample clock.
'
' Instructions for running:
'   1.  Select the physical channel on the DAQ device.
'   2.  Select the external clock source.
'   3.  Select the sample clock rate.
'   4.  Select the number of samples.
'
' Steps:
'   1.  Create a new digital output task.
'   2.  Create the digital output channel.
'   3.  Configure the task to use an external sample clock.
'   4.  Create a DigitalSingleChannelWriter and associate it with the task by
'       using the task's stream.
'   5.  Generate a waveform with random states to write to the channel.
'   6.  Call DigitalSingleChannelWriter.WriteWaveform to write the data to the
'       channel.
'   7.  Dispose the Task object to clean-up any resources associated with the
'       task.
'   8.  Handle any DaqExceptions, if they occur.
'
' I/O Connections Overview:
'   Make sure your signal output terminals match the physical channel text box. 
'   In this case wire your digital signals to the appropriate eight digital
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

Imports NationalInstruments.DAQmx

Public Class MainForm
    Inherits System.Windows.Forms.Form
    Private WithEvents physicalChannelsLabel As System.Windows.Forms.Label
    Private WithEvents clockSourceLabel As System.Windows.Forms.Label
    Private WithEvents clockSourceTextBox As System.Windows.Forms.TextBox
    Private WithEvents sampleClockRateLabel As System.Windows.Forms.Label
    Private WithEvents numberSamplesLabel As System.Windows.Forms.Label
    Private WithEvents writeButton As System.Windows.Forms.Button
    Private WithEvents sampleClockRateNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents numberSamplesNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents physicalChannelComboBox As System.Windows.Forms.ComboBox

    Private components As System.ComponentModel.Container = Nothing


    Public Sub New()
        MyBase.New()
        Application.EnableVisualStyles()
        Application.DoEvents()

        '
        ' Required for Windows Form Designer support
        '
        InitializeComponent()

        physicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DOLine Or PhysicalChannelTypes.DOPort, PhysicalChannelAccess.External))
        If physicalChannelComboBox.Items.Count > 0 Then
            physicalChannelComboBox.SelectedIndex = 0
        End If
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
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MainForm))
        Me.physicalChannelsLabel = New System.Windows.Forms.Label
        Me.clockSourceLabel = New System.Windows.Forms.Label
        Me.clockSourceTextBox = New System.Windows.Forms.TextBox
        Me.sampleClockRateLabel = New System.Windows.Forms.Label
        Me.numberSamplesLabel = New System.Windows.Forms.Label
        Me.writeButton = New System.Windows.Forms.Button
        Me.sampleClockRateNumeric = New System.Windows.Forms.NumericUpDown
        Me.numberSamplesNumeric = New System.Windows.Forms.NumericUpDown
        Me.physicalChannelComboBox = New System.Windows.Forms.ComboBox
        CType(Me.sampleClockRateNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numberSamplesNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'physicalChannelsLabel
        '
        Me.physicalChannelsLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.physicalChannelsLabel.Location = New System.Drawing.Point(8, 16)
        Me.physicalChannelsLabel.Name = "physicalChannelsLabel"
        Me.physicalChannelsLabel.Size = New System.Drawing.Size(112, 23)
        Me.physicalChannelsLabel.TabIndex = 0
        Me.physicalChannelsLabel.Text = "Physical Channels:"
        '
        'clockSourceLabel
        '
        Me.clockSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.clockSourceLabel.Location = New System.Drawing.Point(8, 53)
        Me.clockSourceLabel.Name = "clockSourceLabel"
        Me.clockSourceLabel.Size = New System.Drawing.Size(112, 23)
        Me.clockSourceLabel.TabIndex = 2
        Me.clockSourceLabel.Text = "Clock Source:"
        '
        'clockSourceTextBox
        '
        Me.clockSourceTextBox.Location = New System.Drawing.Point(144, 54)
        Me.clockSourceTextBox.Name = "clockSourceTextBox"
        Me.clockSourceTextBox.Size = New System.Drawing.Size(176, 20)
        Me.clockSourceTextBox.TabIndex = 3
        Me.clockSourceTextBox.Text = "/Dev1/PFI0"
        '
        'sampleClockRateLabel
        '
        Me.sampleClockRateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.sampleClockRateLabel.Location = New System.Drawing.Point(8, 90)
        Me.sampleClockRateLabel.Name = "sampleClockRateLabel"
        Me.sampleClockRateLabel.Size = New System.Drawing.Size(112, 23)
        Me.sampleClockRateLabel.TabIndex = 4
        Me.sampleClockRateLabel.Text = "Sample Clock Rate:"
        '
        'numberSamplesLabel
        '
        Me.numberSamplesLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.numberSamplesLabel.Location = New System.Drawing.Point(8, 127)
        Me.numberSamplesLabel.Name = "numberSamplesLabel"
        Me.numberSamplesLabel.Size = New System.Drawing.Size(112, 23)
        Me.numberSamplesLabel.TabIndex = 6
        Me.numberSamplesLabel.Text = "Number of Samples:"
        '
        'writeButton
        '
        Me.writeButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.writeButton.Location = New System.Drawing.Point(128, 160)
        Me.writeButton.Name = "writeButton"
        Me.writeButton.TabIndex = 8
        Me.writeButton.Text = "Write Data"
        '
        'sampleClockRateNumeric
        '
        Me.sampleClockRateNumeric.DecimalPlaces = 2
        Me.sampleClockRateNumeric.Location = New System.Drawing.Point(144, 91)
        Me.sampleClockRateNumeric.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.sampleClockRateNumeric.Minimum = New Decimal(New Integer() {2, 0, 0, 0})
        Me.sampleClockRateNumeric.Name = "sampleClockRateNumeric"
        Me.sampleClockRateNumeric.Size = New System.Drawing.Size(176, 20)
        Me.sampleClockRateNumeric.TabIndex = 5
        Me.sampleClockRateNumeric.Value = New Decimal(New Integer() {1000, 0, 0, 0})
        '
        'numberSamplesNumeric
        '
        Me.numberSamplesNumeric.Location = New System.Drawing.Point(144, 128)
        Me.numberSamplesNumeric.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.numberSamplesNumeric.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.numberSamplesNumeric.Name = "numberSamplesNumeric"
        Me.numberSamplesNumeric.Size = New System.Drawing.Size(176, 20)
        Me.numberSamplesNumeric.TabIndex = 7
        Me.numberSamplesNumeric.Value = New Decimal(New Integer() {1000, 0, 0, 0})
        '
        'physicalChannelComboBox
        '
        Me.physicalChannelComboBox.Location = New System.Drawing.Point(144, 17)
        Me.physicalChannelComboBox.Name = "physicalChannelComboBox"
        Me.physicalChannelComboBox.Size = New System.Drawing.Size(176, 21)
        Me.physicalChannelComboBox.TabIndex = 1
        Me.physicalChannelComboBox.Text = "Dev1/port0"
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(330, 192)
        Me.Controls.Add(Me.physicalChannelComboBox)
        Me.Controls.Add(Me.sampleClockRateNumeric)
        Me.Controls.Add(Me.writeButton)
        Me.Controls.Add(Me.physicalChannelsLabel)
        Me.Controls.Add(Me.clockSourceLabel)
        Me.Controls.Add(Me.clockSourceTextBox)
        Me.Controls.Add(Me.sampleClockRateLabel)
        Me.Controls.Add(Me.numberSamplesLabel)
        Me.Controls.Add(Me.numberSamplesNumeric)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Write Digital Channel - External Clock"
        CType(Me.sampleClockRateNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numberSamplesNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub 'InitializeComponent

    Private Sub writeButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles writeButton.Click
        System.Windows.Forms.Cursor.Current = Cursors.WaitCursor

        Try
            Dim digitalWriteTask As New Task()
            Try
                ' Create the digital output channel
                digitalWriteTask.DOChannels.CreateChannel(physicalChannelComboBox.Text, "", ChannelLineGrouping.OneChannelForAllLines)

                ' Verify the task so we can query the channel's properties
                digitalWriteTask.Control(TaskAction.Verify)

                ' Create the data to write
                Dim samples As Integer = CInt(numberSamplesNumeric.Value)
                Dim signals As Integer = CInt(digitalWriteTask.DOChannels(0).NumberOfLines)

                ' Set up the timing
                digitalWriteTask.Timing.ConfigureSampleClock(clockSourceTextBox.Text, _
                    Convert.ToDouble(sampleClockRateNumeric.Value), _
                    SampleClockActiveEdge.Rising, SampleQuantityMode.FiniteSamples, _
                    Convert.ToInt32(numberSamplesNumeric.Value))

                ' Write the data
                Dim writer As New DigitalSingleChannelWriter(digitalWriteTask.Stream)

                ' Loop through every sample
                Dim waveform As DigitalWaveform = New DigitalWaveform(Convert.ToInt32(numberSamplesNumeric.Value), _
                                                                        Convert.ToInt32(digitalWriteTask.DOChannels(0).NumberOfLines))
                Dim r As New Random
                Dim i As Integer
                For i = 0 To samples - 1
                    ' Generate a random set of boolean values
                    Dim j As Integer
                    For j = 0 To signals - 1
                        If r.Next() Mod 2 = 0 Then
                            waveform.Samples(i).States(j) = DigitalState.ForceUp
                        Else
                            waveform.Samples(i).States(j) = DigitalState.ForceDown
                        End If
                    Next j
                Next i
                ' Write those values
                writer.WriteWaveform(True, waveform)

                digitalWriteTask.WaitUntilDone()
            Finally
                digitalWriteTask.Dispose()
            End Try
        Catch ex As DaqException
            MessageBox.Show(ex.Message)
        Finally
            System.Windows.Forms.Cursor.Current = Cursors.Default
        End Try
    End Sub 'writeButton_Click
End Class 'MainForm
