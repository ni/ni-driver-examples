'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   ReadDigPort_ExtClk
'
' Category:
'   DI
'
' Description:
'   This example demonstrates how to read values from a digital port using an
'   external sample clock.
'
' Instructions for running:
'   1.  Select the physical channel on the DAQ device.Note: You must specify
'       exactly 8 lines in the physical channel text box.
'   2.  Select the external clock source.
'   3.  Select the number of samples per channel.
'   4.  Select the sample clock rate.
'
' Steps:
'   1.  Create a new digital input task.
'   2.  Create the digital input channel.
'   3.  Configure the task to use an external sample clock.
'   4.  Create a DigitalSingleChannelReader and associate it with the task by
'       using the task's stream.
'   5.  Call DigitalSingleChannelReader.ReadSingleSamplePortByte to read the
'       data from the channel.
'   6.  Dispose the Task object to clean-up any resources associated with the
'       task.
'   7.  Handle any DaqExceptions, if they occur.
'
' I/O Connections Overview:
'   Make sure your signal input terminals match the physical channel text box. 
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
    Private reader As DigitalSingleChannelReader

    Private channelParametersGroupBox As System.Windows.Forms.GroupBox
    Private samplesPerChannelNumeric As System.Windows.Forms.NumericUpDown
    Private clockSourceTextBox As System.Windows.Forms.TextBox
    Private clockSourceLabel As System.Windows.Forms.Label
    Private physicalChannelLabel As System.Windows.Forms.Label
    Private samplesPerChannelLabel As System.Windows.Forms.Label
    Private sampleRateNumeric As System.Windows.Forms.NumericUpDown
    Private sampleRateLabel As System.Windows.Forms.Label
    Private digitalValuesGroupBox As System.Windows.Forms.GroupBox
    Private valuesListBox As System.Windows.Forms.ListBox
    Private WithEvents readButton As System.Windows.Forms.Button
    Friend WithEvents physicalChannelComboBox As System.Windows.Forms.ComboBox

    Private components As System.ComponentModel.Container = Nothing


    Public Sub New()
        MyBase.New()
        Application.EnableVisualStyles()

        '
        ' Required for Windows Form Designer support
        '
        InitializeComponent()

        physicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DIPort, PhysicalChannelAccess.External))
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
        Me.channelParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.physicalChannelComboBox = New System.Windows.Forms.ComboBox
        Me.samplesPerChannelNumeric = New System.Windows.Forms.NumericUpDown
        Me.clockSourceTextBox = New System.Windows.Forms.TextBox
        Me.clockSourceLabel = New System.Windows.Forms.Label
        Me.physicalChannelLabel = New System.Windows.Forms.Label
        Me.samplesPerChannelLabel = New System.Windows.Forms.Label
        Me.sampleRateNumeric = New System.Windows.Forms.NumericUpDown
        Me.sampleRateLabel = New System.Windows.Forms.Label
        Me.digitalValuesGroupBox = New System.Windows.Forms.GroupBox
        Me.valuesListBox = New System.Windows.Forms.ListBox
        Me.readButton = New System.Windows.Forms.Button
        Me.channelParametersGroupBox.SuspendLayout()
        CType(Me.samplesPerChannelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.sampleRateNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.digitalValuesGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'channelParametersGroupBox
        '
        Me.channelParametersGroupBox.Controls.Add(Me.physicalChannelComboBox)
        Me.channelParametersGroupBox.Controls.Add(Me.samplesPerChannelNumeric)
        Me.channelParametersGroupBox.Controls.Add(Me.clockSourceTextBox)
        Me.channelParametersGroupBox.Controls.Add(Me.clockSourceLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.physicalChannelLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.samplesPerChannelLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.sampleRateNumeric)
        Me.channelParametersGroupBox.Controls.Add(Me.sampleRateLabel)
        Me.channelParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.channelParametersGroupBox.Location = New System.Drawing.Point(8, 8)
        Me.channelParametersGroupBox.Name = "channelParametersGroupBox"
        Me.channelParametersGroupBox.Size = New System.Drawing.Size(312, 176)
        Me.channelParametersGroupBox.TabIndex = 0
        Me.channelParametersGroupBox.TabStop = False
        Me.channelParametersGroupBox.Text = "Channel Parameters"
        '
        'physicalChannelComboBox
        '
        Me.physicalChannelComboBox.Location = New System.Drawing.Point(144, 33)
        Me.physicalChannelComboBox.Name = "physicalChannelComboBox"
        Me.physicalChannelComboBox.Size = New System.Drawing.Size(152, 21)
        Me.physicalChannelComboBox.TabIndex = 1
        Me.physicalChannelComboBox.Text = "Dev1/port0"
        '
        'samplesPerChannelNumeric
        '
        Me.samplesPerChannelNumeric.Location = New System.Drawing.Point(144, 105)
        Me.samplesPerChannelNumeric.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.samplesPerChannelNumeric.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.samplesPerChannelNumeric.Name = "samplesPerChannelNumeric"
        Me.samplesPerChannelNumeric.Size = New System.Drawing.Size(152, 20)
        Me.samplesPerChannelNumeric.TabIndex = 5
        Me.samplesPerChannelNumeric.Value = New Decimal(New Integer() {1000, 0, 0, 0})
        '
        'clockSourceTextBox
        '
        Me.clockSourceTextBox.Location = New System.Drawing.Point(144, 69)
        Me.clockSourceTextBox.Name = "clockSourceTextBox"
        Me.clockSourceTextBox.Size = New System.Drawing.Size(152, 20)
        Me.clockSourceTextBox.TabIndex = 3
        Me.clockSourceTextBox.Text = "/Dev1/PFI0"
        '
        'clockSourceLabel
        '
        Me.clockSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.clockSourceLabel.Location = New System.Drawing.Point(16, 68)
        Me.clockSourceLabel.Name = "clockSourceLabel"
        Me.clockSourceLabel.TabIndex = 2
        Me.clockSourceLabel.Text = "Clock Source:"
        '
        'physicalChannelLabel
        '
        Me.physicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.physicalChannelLabel.Location = New System.Drawing.Point(16, 32)
        Me.physicalChannelLabel.Name = "physicalChannelLabel"
        Me.physicalChannelLabel.TabIndex = 0
        Me.physicalChannelLabel.Text = "Physical Channel:"
        '
        'samplesPerChannelLabel
        '
        Me.samplesPerChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.samplesPerChannelLabel.Location = New System.Drawing.Point(16, 104)
        Me.samplesPerChannelLabel.Name = "samplesPerChannelLabel"
        Me.samplesPerChannelLabel.Size = New System.Drawing.Size(120, 23)
        Me.samplesPerChannelLabel.TabIndex = 4
        Me.samplesPerChannelLabel.Text = "Samples per Channel:"
        '
        'sampleRateNumeric
        '
        Me.sampleRateNumeric.DecimalPlaces = 2
        Me.sampleRateNumeric.Location = New System.Drawing.Point(144, 141)
        Me.sampleRateNumeric.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.sampleRateNumeric.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.sampleRateNumeric.Name = "sampleRateNumeric"
        Me.sampleRateNumeric.Size = New System.Drawing.Size(152, 20)
        Me.sampleRateNumeric.TabIndex = 7
        Me.sampleRateNumeric.Value = New Decimal(New Integer() {1000, 0, 0, 0})
        '
        'sampleRateLabel
        '
        Me.sampleRateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.sampleRateLabel.Location = New System.Drawing.Point(16, 140)
        Me.sampleRateLabel.Name = "sampleRateLabel"
        Me.sampleRateLabel.Size = New System.Drawing.Size(120, 23)
        Me.sampleRateLabel.TabIndex = 6
        Me.sampleRateLabel.Text = "Sample Rate (Hz):"
        '
        'digitalValuesGroupBox
        '
        Me.digitalValuesGroupBox.Controls.Add(Me.valuesListBox)
        Me.digitalValuesGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.digitalValuesGroupBox.Location = New System.Drawing.Point(8, 192)
        Me.digitalValuesGroupBox.Name = "digitalValuesGroupBox"
        Me.digitalValuesGroupBox.Size = New System.Drawing.Size(312, 184)
        Me.digitalValuesGroupBox.TabIndex = 1
        Me.digitalValuesGroupBox.TabStop = False
        Me.digitalValuesGroupBox.Text = "Digital Values"
        '
        'valuesListBox
        '
        Me.valuesListBox.Location = New System.Drawing.Point(8, 16)
        Me.valuesListBox.Name = "valuesListBox"
        Me.valuesListBox.Size = New System.Drawing.Size(296, 160)
        Me.valuesListBox.TabIndex = 0
        '
        'readButton
        '
        Me.readButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.readButton.Location = New System.Drawing.Point(128, 384)
        Me.readButton.Name = "readButton"
        Me.readButton.TabIndex = 2
        Me.readButton.Text = "Read"
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(328, 414)
        Me.Controls.Add(Me.readButton)
        Me.Controls.Add(Me.digitalValuesGroupBox)
        Me.Controls.Add(Me.channelParametersGroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "MainForm"
        Me.Text = "Read Digital Port - External Clock"
        Me.channelParametersGroupBox.ResumeLayout(False)
        CType(Me.samplesPerChannelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.sampleRateNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.digitalValuesGroupBox.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub 'InitializeComponent

    Private Sub readButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles readButton.Click
        System.Windows.Forms.Cursor.Current = Cursors.WaitCursor

        Try
            ' Create the task
            Dim myTask As Task = New Task()
            ' Create the digital input channel
            myTask.DIChannels.CreateChannel(physicalChannelComboBox.Text, "", ChannelLineGrouping.OneChannelForAllLines)

            ' Configure the external clock
            myTask.Timing.ConfigureSampleClock(clockSourceTextBox.Text, Convert.ToDouble(sampleRateNumeric.Value), SampleClockActiveEdge.Rising, SampleQuantityMode.FiniteSamples, Convert.ToInt32(samplesPerChannelNumeric.Value))

            ' Create a task reader
            reader = New DigitalSingleChannelReader(myTask.Stream)

            ' Read the data
            Dim data As Byte() = reader.ReadMultiSamplePortByte(Convert.ToInt32(samplesPerChannelNumeric.Value))

            ' Update the UI
            valuesListBox.Items.Clear()

            Dim i As Integer
            For i = 0 To data.Length - 1
                valuesListBox.Items.Add(data(i))
            Next i

            myTask.Dispose()
        Catch ex As DaqException
            MessageBox.Show(ex.Message)
        Finally
            System.Windows.Forms.Cursor.Current = Cursors.Default
        End Try
    End Sub 'readButton_Click
End Class 'MainForm
