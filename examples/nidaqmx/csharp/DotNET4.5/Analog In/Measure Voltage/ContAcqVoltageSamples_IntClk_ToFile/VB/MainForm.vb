'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   ContAcqVoltageSamples_IntClk_ToFile
'
' Category:
'   AI
'
' Description:
'   This example demonstrates how to acquire, write to file, and load from disk
'   a continuous amount of analog input data using the DAQ device's internal
'   clock.
'
' Instructions for running:
'   1.  Select the physical channels corresponding to where your signals are
'       input on the DAQ device.
'   2.  Enter the minimum and maximum voltage range.Note: For better accuracy,
'       try to match the input range to the expected voltage levels of the
'       measured signals.
'   3.  Set the rate of the acquisition and number of samples.
'   4.  Choose an output file format, either text or binary.
'   5.  Select the output filename.
'   6.  Start the acquisition.
'   7.  Select the file format of the file you want to load data from, either
'       text or binary.
'   8.  Select the input filename.
'   9.  Click the Read button to read the data from disk and display it.
'
' Steps:
'   1.  Create a new analog input task.
'   2.  Create the analog input voltage channels.
'   3.  Configure the timing for the acquisition.  In this example we use the
'       DAQ device's internal clock to take a continuous number of samples.
'   4.  Open the output file for writing.
'   5.  Create a AnalogMultiChannelReader and associate it with the task by
'       using the task's stream. Call
'       AnalogMultiChannelReader.BeginBeginReadMultiSample to install a callback
'       and begin the asynchronous read operation.
'   6.  Inside the callback, call AnalogMultiChannelReader.EndReadMultiSample to
'       retrieve the data from the read operation.  
'   7.  Call AnalogMultiChannelReader.BeginBeginReadMultiSample again inside the
'       callback to perform another read operation.
'   8.  Dispose the Task object to clean-up any resources associated with the
'       task.
'   9.  Close the output file.
'   10. Open the input file for reading.
'   11. Read and display the data.
'   12. Handle any DaqExceptions, if they occur.
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
'   Make sure your signal input terminals match the physical I/O control.  In
'   the default case (differential channel ai0), wire the positive lead for your
'   signal to the ACH0 pin on your DAQ device and wire the negative lead for
'   your signal to the ACH8 pin.  For more information on the input and output
'   terminals for your device, open the NI-DAQmx Help and refer to the NI-DAQmx
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
Imports System.IO
Imports NationalInstruments
Imports NationalInstruments.DAQmx

Public Class MainForm
    Inherits System.Windows.Forms.Form

    Private myTask As Task
    Private analogInReader As AnalogMultiChannelReader
    Private runningTask As Task
    Private analogCallback As AsyncCallback
    Private data(,) As Double
    Private dataColumn As DataColumn() = Nothing
    Private dataTable As dataTable = Nothing
    Private savedData As ArrayList
    Private fileStreamWriter As StreamWriter
    Private fileBinaryWriter As BinaryWriter
    Private fileStreamReader As StreamReader
    Private fileBinaryReader As BinaryReader
    Private fileNameWrite As String
    Private fileNameRead As String
    Private useTextFileWrite As Boolean
    Private useTextFileRead As Boolean

    Friend WithEvents channelParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents maximumLabel As System.Windows.Forms.Label
    Friend WithEvents minimumLabel As System.Windows.Forms.Label
    Friend WithEvents physicalChannelLabel As System.Windows.Forms.Label
    Friend WithEvents rateLabel As System.Windows.Forms.Label
    Friend WithEvents samplesLabel As System.Windows.Forms.Label
    Friend WithEvents resultLabel As System.Windows.Forms.Label

    Friend WithEvents timingParametersGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents acquisitionResultGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents acquisitionDataGrid As System.Windows.Forms.DataGrid
    Friend WithEvents rateNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents samplesPerChannelNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents minimumValueNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents maximumValueNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents physicalChannelComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents writeToFileSaveFileDialog As System.Windows.Forms.SaveFileDialog
    Friend WithEvents readFromFileOpenFileDialog As System.Windows.Forms.OpenFileDialog
    Friend WithEvents writeToFileGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents fileToolTip As System.Windows.Forms.ToolTip
    Friend WithEvents filePathWriteTextBox As System.Windows.Forms.TextBox
    Friend WithEvents browseWriteButton As System.Windows.Forms.Button
    Friend WithEvents filePathWriteLabel As System.Windows.Forms.Label
    Friend WithEvents binaryFileWriteRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents textFileWriteRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents fileTypeWriteLabel As System.Windows.Forms.Label
    Friend WithEvents readFromFileGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents browseReadButton As System.Windows.Forms.Button
    Friend WithEvents filePathReadLabel As System.Windows.Forms.Label
    Friend WithEvents binaryFileReadRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents textFileReadRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents filePathReadTextBox As System.Windows.Forms.TextBox
    Friend WithEvents fileTypeReadLabel As System.Windows.Forms.Label
    Friend WithEvents stopButton As System.Windows.Forms.Button
    Friend WithEvents startButton As System.Windows.Forms.Button
    Friend WithEvents readButton As System.Windows.Forms.Button
    Private components As System.ComponentModel.IContainer

    Public Sub New()
        MyBase.New()
        Application.EnableVisualStyles()
        Application.DoEvents()

        '
        ' Required for Windows Form Designer support
        '
        InitializeComponent()

        '
        ' TODO: Add any constructor code after InitializeComponent call
        '
        stopButton.Enabled = False
        dataTable = New DataTable

        physicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AI, PhysicalChannelAccess.External))
        If physicalChannelComboBox.Items.Count > 0 Then
            physicalChannelComboBox.SelectedIndex = 0
        End If
    End Sub 'New

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
    End Sub 'Dispose

    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.channelParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.physicalChannelComboBox = New System.Windows.Forms.ComboBox
        Me.minimumValueNumeric = New System.Windows.Forms.NumericUpDown
        Me.maximumValueNumeric = New System.Windows.Forms.NumericUpDown
        Me.maximumLabel = New System.Windows.Forms.Label
        Me.minimumLabel = New System.Windows.Forms.Label
        Me.physicalChannelLabel = New System.Windows.Forms.Label
        Me.timingParametersGroupBox = New System.Windows.Forms.GroupBox
        Me.rateNumeric = New System.Windows.Forms.NumericUpDown
        Me.samplesLabel = New System.Windows.Forms.Label
        Me.rateLabel = New System.Windows.Forms.Label
        Me.samplesPerChannelNumeric = New System.Windows.Forms.NumericUpDown
        Me.acquisitionResultGroupBox = New System.Windows.Forms.GroupBox
        Me.resultLabel = New System.Windows.Forms.Label
        Me.acquisitionDataGrid = New System.Windows.Forms.DataGrid
        Me.filePathWriteTextBox = New System.Windows.Forms.TextBox
        Me.writeToFileSaveFileDialog = New System.Windows.Forms.SaveFileDialog
        Me.readFromFileOpenFileDialog = New System.Windows.Forms.OpenFileDialog
        Me.fileToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.writeToFileGroupBox = New System.Windows.Forms.GroupBox
        Me.stopButton = New System.Windows.Forms.Button
        Me.startButton = New System.Windows.Forms.Button
        Me.browseWriteButton = New System.Windows.Forms.Button
        Me.filePathWriteLabel = New System.Windows.Forms.Label
        Me.binaryFileWriteRadioButton = New System.Windows.Forms.RadioButton
        Me.textFileWriteRadioButton = New System.Windows.Forms.RadioButton
        Me.fileTypeWriteLabel = New System.Windows.Forms.Label
        Me.readFromFileGroupBox = New System.Windows.Forms.GroupBox
        Me.browseReadButton = New System.Windows.Forms.Button
        Me.filePathReadLabel = New System.Windows.Forms.Label
        Me.binaryFileReadRadioButton = New System.Windows.Forms.RadioButton
        Me.textFileReadRadioButton = New System.Windows.Forms.RadioButton
        Me.filePathReadTextBox = New System.Windows.Forms.TextBox
        Me.fileTypeReadLabel = New System.Windows.Forms.Label
        Me.readButton = New System.Windows.Forms.Button
        Me.channelParametersGroupBox.SuspendLayout()
        CType(Me.minimumValueNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.maximumValueNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.timingParametersGroupBox.SuspendLayout()
        CType(Me.rateNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.samplesPerChannelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.acquisitionResultGroupBox.SuspendLayout()
        CType(Me.acquisitionDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.writeToFileGroupBox.SuspendLayout()
        Me.readFromFileGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'channelParametersGroupBox
        '
        Me.channelParametersGroupBox.Controls.Add(Me.physicalChannelComboBox)
        Me.channelParametersGroupBox.Controls.Add(Me.minimumValueNumeric)
        Me.channelParametersGroupBox.Controls.Add(Me.maximumValueNumeric)
        Me.channelParametersGroupBox.Controls.Add(Me.maximumLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.minimumLabel)
        Me.channelParametersGroupBox.Controls.Add(Me.physicalChannelLabel)
        Me.channelParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.channelParametersGroupBox.Location = New System.Drawing.Point(8, 8)
        Me.channelParametersGroupBox.Name = "channelParametersGroupBox"
        Me.channelParametersGroupBox.Size = New System.Drawing.Size(224, 120)
        Me.channelParametersGroupBox.TabIndex = 0
        Me.channelParametersGroupBox.TabStop = False
        Me.channelParametersGroupBox.Text = "Channel Parameters"
        '
        'physicalChannelComboBox
        '
        Me.physicalChannelComboBox.Location = New System.Drawing.Point(120, 24)
        Me.physicalChannelComboBox.Name = "physicalChannelComboBox"
        Me.physicalChannelComboBox.Size = New System.Drawing.Size(96, 21)
        Me.physicalChannelComboBox.TabIndex = 1
        Me.physicalChannelComboBox.Text = "Dev1/ai0"
        '
        'minimumValueNumeric
        '
        Me.minimumValueNumeric.DecimalPlaces = 2
        Me.minimumValueNumeric.Location = New System.Drawing.Point(120, 56)
        Me.minimumValueNumeric.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.minimumValueNumeric.Minimum = New Decimal(New Integer() {100, 0, 0, -2147418112})
        Me.minimumValueNumeric.Name = "minimumValueNumeric"
        Me.minimumValueNumeric.Size = New System.Drawing.Size(96, 20)
        Me.minimumValueNumeric.TabIndex = 3
        Me.minimumValueNumeric.Value = New Decimal(New Integer() {100, 0, 0, -2147418112})
        '
        'maximumValueNumeric
        '
        Me.maximumValueNumeric.DecimalPlaces = 2
        Me.maximumValueNumeric.Location = New System.Drawing.Point(120, 88)
        Me.maximumValueNumeric.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.maximumValueNumeric.Minimum = New Decimal(New Integer() {10, 0, 0, -2147418112})
        Me.maximumValueNumeric.Name = "maximumValueNumeric"
        Me.maximumValueNumeric.Size = New System.Drawing.Size(96, 20)
        Me.maximumValueNumeric.TabIndex = 5
        Me.maximumValueNumeric.Value = New Decimal(New Integer() {100, 0, 0, 65536})
        '
        'maximumLabel
        '
        Me.maximumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.maximumLabel.Location = New System.Drawing.Point(16, 88)
        Me.maximumLabel.Name = "maximumLabel"
        Me.maximumLabel.Size = New System.Drawing.Size(112, 16)
        Me.maximumLabel.TabIndex = 4
        Me.maximumLabel.Text = "Maximum Value (V):"
        '
        'minimumLabel
        '
        Me.minimumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.minimumLabel.Location = New System.Drawing.Point(16, 56)
        Me.minimumLabel.Name = "minimumLabel"
        Me.minimumLabel.Size = New System.Drawing.Size(104, 15)
        Me.minimumLabel.TabIndex = 2
        Me.minimumLabel.Text = "Minimum Value (V):"
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
        'timingParametersGroupBox
        '
        Me.timingParametersGroupBox.Controls.Add(Me.rateNumeric)
        Me.timingParametersGroupBox.Controls.Add(Me.samplesLabel)
        Me.timingParametersGroupBox.Controls.Add(Me.rateLabel)
        Me.timingParametersGroupBox.Controls.Add(Me.samplesPerChannelNumeric)
        Me.timingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.timingParametersGroupBox.Location = New System.Drawing.Point(8, 140)
        Me.timingParametersGroupBox.Name = "timingParametersGroupBox"
        Me.timingParametersGroupBox.Size = New System.Drawing.Size(224, 92)
        Me.timingParametersGroupBox.TabIndex = 1
        Me.timingParametersGroupBox.TabStop = False
        Me.timingParametersGroupBox.Text = "Timing Parameters"
        '
        'rateNumeric
        '
        Me.rateNumeric.DecimalPlaces = 2
        Me.rateNumeric.Location = New System.Drawing.Point(120, 56)
        Me.rateNumeric.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.rateNumeric.Name = "rateNumeric"
        Me.rateNumeric.Size = New System.Drawing.Size(96, 20)
        Me.rateNumeric.TabIndex = 3
        Me.rateNumeric.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        'samplesLabel
        '
        Me.samplesLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.samplesLabel.Location = New System.Drawing.Point(16, 26)
        Me.samplesLabel.Name = "samplesLabel"
        Me.samplesLabel.Size = New System.Drawing.Size(104, 16)
        Me.samplesLabel.TabIndex = 0
        Me.samplesLabel.Text = "Samples/Channel:"
        '
        'rateLabel
        '
        Me.rateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.rateLabel.Location = New System.Drawing.Point(16, 58)
        Me.rateLabel.Name = "rateLabel"
        Me.rateLabel.Size = New System.Drawing.Size(56, 16)
        Me.rateLabel.TabIndex = 2
        Me.rateLabel.Text = "Rate (Hz):"
        '
        'samplesPerChannelNumeric
        '
        Me.samplesPerChannelNumeric.Location = New System.Drawing.Point(120, 24)
        Me.samplesPerChannelNumeric.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.samplesPerChannelNumeric.Name = "samplesPerChannelNumeric"
        Me.samplesPerChannelNumeric.Size = New System.Drawing.Size(96, 20)
        Me.samplesPerChannelNumeric.TabIndex = 1
        Me.samplesPerChannelNumeric.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        'acquisitionResultGroupBox
        '
        Me.acquisitionResultGroupBox.Controls.Add(Me.resultLabel)
        Me.acquisitionResultGroupBox.Controls.Add(Me.acquisitionDataGrid)
        Me.acquisitionResultGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.acquisitionResultGroupBox.Location = New System.Drawing.Point(240, 8)
        Me.acquisitionResultGroupBox.Name = "acquisitionResultGroupBox"
        Me.acquisitionResultGroupBox.Size = New System.Drawing.Size(304, 224)
        Me.acquisitionResultGroupBox.TabIndex = 3
        Me.acquisitionResultGroupBox.TabStop = False
        Me.acquisitionResultGroupBox.Text = "Acquisition Results"
        '
        'resultLabel
        '
        Me.resultLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.resultLabel.Location = New System.Drawing.Point(8, 16)
        Me.resultLabel.Name = "resultLabel"
        Me.resultLabel.Size = New System.Drawing.Size(112, 16)
        Me.resultLabel.TabIndex = 0
        Me.resultLabel.Text = "Acquisition Data (V):"
        '
        'acquisitionDataGrid
        '
        Me.acquisitionDataGrid.AllowSorting = False
        Me.acquisitionDataGrid.DataMember = ""
        Me.acquisitionDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.acquisitionDataGrid.Location = New System.Drawing.Point(16, 32)
        Me.acquisitionDataGrid.Name = "acquisitionDataGrid"
        Me.acquisitionDataGrid.ParentRowsVisible = False
        Me.acquisitionDataGrid.ReadOnly = True
        Me.acquisitionDataGrid.Size = New System.Drawing.Size(280, 184)
        Me.acquisitionDataGrid.TabIndex = 1
        Me.acquisitionDataGrid.TabStop = False
        '
        'filePathWriteTextBox
        '
        Me.filePathWriteTextBox.Location = New System.Drawing.Point(120, 57)
        Me.filePathWriteTextBox.Name = "filePathWriteTextBox"
        Me.filePathWriteTextBox.ReadOnly = True
        Me.filePathWriteTextBox.Size = New System.Drawing.Size(384, 20)
        Me.filePathWriteTextBox.TabIndex = 4
        Me.filePathWriteTextBox.Text = "Choose file location"
        '
        'writeToFileSaveFileDialog
        '
        Me.writeToFileSaveFileDialog.CreatePrompt = True
        Me.writeToFileSaveFileDialog.DefaultExt = "txt"
        Me.writeToFileSaveFileDialog.FileName = "acquisitionData.txt"
        Me.writeToFileSaveFileDialog.Filter = "Text Files|*.txt| All Files|*.*"
        Me.writeToFileSaveFileDialog.Title = "Save Acquisition Data To File"
        '
        'readFromFileOpenFileDialog
        '
        Me.readFromFileOpenFileDialog.DefaultExt = "txt"
        Me.readFromFileOpenFileDialog.FileName = "acquisitionData.txt"
        Me.readFromFileOpenFileDialog.Filter = "Text Files|*.txt| All Files|*.*"
        Me.readFromFileOpenFileDialog.Title = "Open Acquisition Data"
        '
        'writeToFileGroupBox
        '
        Me.writeToFileGroupBox.AccessibleRole = System.Windows.Forms.AccessibleRole.None
        Me.writeToFileGroupBox.Controls.Add(Me.stopButton)
        Me.writeToFileGroupBox.Controls.Add(Me.startButton)
        Me.writeToFileGroupBox.Controls.Add(Me.browseWriteButton)
        Me.writeToFileGroupBox.Controls.Add(Me.filePathWriteLabel)
        Me.writeToFileGroupBox.Controls.Add(Me.binaryFileWriteRadioButton)
        Me.writeToFileGroupBox.Controls.Add(Me.textFileWriteRadioButton)
        Me.writeToFileGroupBox.Controls.Add(Me.filePathWriteTextBox)
        Me.writeToFileGroupBox.Controls.Add(Me.fileTypeWriteLabel)
        Me.writeToFileGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.writeToFileGroupBox.Location = New System.Drawing.Point(8, 240)
        Me.writeToFileGroupBox.Name = "writeToFileGroupBox"
        Me.writeToFileGroupBox.Size = New System.Drawing.Size(536, 120)
        Me.writeToFileGroupBox.TabIndex = 2
        Me.writeToFileGroupBox.TabStop = False
        Me.writeToFileGroupBox.Text = "Write To File"
        '
        'stopButton
        '
        Me.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.stopButton.Location = New System.Drawing.Point(216, 88)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(80, 24)
        Me.stopButton.TabIndex = 7
        Me.stopButton.Text = "Stop"
        '
        'startButton
        '
        Me.startButton.Enabled = False
        Me.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.startButton.Location = New System.Drawing.Point(120, 88)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(80, 24)
        Me.startButton.TabIndex = 6
        Me.startButton.Text = "Start"
        '
        'browseWriteButton
        '
        Me.browseWriteButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.browseWriteButton.Location = New System.Drawing.Point(504, 56)
        Me.browseWriteButton.Name = "browseWriteButton"
        Me.browseWriteButton.Size = New System.Drawing.Size(24, 23)
        Me.browseWriteButton.TabIndex = 5
        Me.browseWriteButton.Text = "..."
        '
        'filePathWriteLabel
        '
        Me.filePathWriteLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.filePathWriteLabel.Location = New System.Drawing.Point(16, 59)
        Me.filePathWriteLabel.Name = "filePathWriteLabel"
        Me.filePathWriteLabel.Size = New System.Drawing.Size(72, 16)
        Me.filePathWriteLabel.TabIndex = 3
        Me.filePathWriteLabel.Text = "File Path:"
        '
        'binaryFileWriteRadioButton
        '
        Me.binaryFileWriteRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.binaryFileWriteRadioButton.Location = New System.Drawing.Point(192, 24)
        Me.binaryFileWriteRadioButton.Name = "binaryFileWriteRadioButton"
        Me.binaryFileWriteRadioButton.Size = New System.Drawing.Size(72, 16)
        Me.binaryFileWriteRadioButton.TabIndex = 2
        Me.binaryFileWriteRadioButton.Text = "Binary File"
        '
        'textFileWriteRadioButton
        '
        Me.textFileWriteRadioButton.Checked = True
        Me.textFileWriteRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.textFileWriteRadioButton.Location = New System.Drawing.Point(120, 24)
        Me.textFileWriteRadioButton.Name = "textFileWriteRadioButton"
        Me.textFileWriteRadioButton.Size = New System.Drawing.Size(72, 16)
        Me.textFileWriteRadioButton.TabIndex = 1
        Me.textFileWriteRadioButton.TabStop = True
        Me.textFileWriteRadioButton.Text = "Text File"
        '
        'fileTypeWriteLabel
        '
        Me.fileTypeWriteLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.fileTypeWriteLabel.Location = New System.Drawing.Point(16, 24)
        Me.fileTypeWriteLabel.Name = "fileTypeWriteLabel"
        Me.fileTypeWriteLabel.Size = New System.Drawing.Size(72, 16)
        Me.fileTypeWriteLabel.TabIndex = 0
        Me.fileTypeWriteLabel.Text = "File Type:"
        '
        'readFromFileGroupBox
        '
        Me.readFromFileGroupBox.AccessibleRole = System.Windows.Forms.AccessibleRole.None
        Me.readFromFileGroupBox.Controls.Add(Me.browseReadButton)
        Me.readFromFileGroupBox.Controls.Add(Me.filePathReadLabel)
        Me.readFromFileGroupBox.Controls.Add(Me.binaryFileReadRadioButton)
        Me.readFromFileGroupBox.Controls.Add(Me.textFileReadRadioButton)
        Me.readFromFileGroupBox.Controls.Add(Me.filePathReadTextBox)
        Me.readFromFileGroupBox.Controls.Add(Me.fileTypeReadLabel)
        Me.readFromFileGroupBox.Controls.Add(Me.readButton)
        Me.readFromFileGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.readFromFileGroupBox.Location = New System.Drawing.Point(9, 360)
        Me.readFromFileGroupBox.Name = "readFromFileGroupBox"
        Me.readFromFileGroupBox.Size = New System.Drawing.Size(536, 120)
        Me.readFromFileGroupBox.TabIndex = 6
        Me.readFromFileGroupBox.TabStop = False
        Me.readFromFileGroupBox.Text = "Read From File"
        '
        'browseReadButton
        '
        Me.browseReadButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.browseReadButton.Location = New System.Drawing.Point(504, 56)
        Me.browseReadButton.Name = "browseReadButton"
        Me.browseReadButton.Size = New System.Drawing.Size(24, 23)
        Me.browseReadButton.TabIndex = 5
        Me.browseReadButton.Text = "..."
        '
        'filePathReadLabel
        '
        Me.filePathReadLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.filePathReadLabel.Location = New System.Drawing.Point(16, 56)
        Me.filePathReadLabel.Name = "filePathReadLabel"
        Me.filePathReadLabel.Size = New System.Drawing.Size(72, 16)
        Me.filePathReadLabel.TabIndex = 3
        Me.filePathReadLabel.Text = "File Path:"
        '
        'binaryFileReadRadioButton
        '
        Me.binaryFileReadRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.binaryFileReadRadioButton.Location = New System.Drawing.Point(192, 24)
        Me.binaryFileReadRadioButton.Name = "binaryFileReadRadioButton"
        Me.binaryFileReadRadioButton.Size = New System.Drawing.Size(72, 16)
        Me.binaryFileReadRadioButton.TabIndex = 2
        Me.binaryFileReadRadioButton.Text = "Binary File"
        '
        'textFileReadRadioButton
        '
        Me.textFileReadRadioButton.Checked = True
        Me.textFileReadRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.textFileReadRadioButton.Location = New System.Drawing.Point(120, 24)
        Me.textFileReadRadioButton.Name = "textFileReadRadioButton"
        Me.textFileReadRadioButton.Size = New System.Drawing.Size(72, 16)
        Me.textFileReadRadioButton.TabIndex = 1
        Me.textFileReadRadioButton.TabStop = True
        Me.textFileReadRadioButton.Text = "Text File"
        '
        'filePathReadTextBox
        '
        Me.filePathReadTextBox.Location = New System.Drawing.Point(120, 56)
        Me.filePathReadTextBox.Name = "filePathReadTextBox"
        Me.filePathReadTextBox.ReadOnly = True
        Me.filePathReadTextBox.Size = New System.Drawing.Size(384, 20)
        Me.filePathReadTextBox.TabIndex = 4
        Me.filePathReadTextBox.Text = "Choose file location"
        '
        'fileTypeReadLabel
        '
        Me.fileTypeReadLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.fileTypeReadLabel.Location = New System.Drawing.Point(16, 24)
        Me.fileTypeReadLabel.Name = "fileTypeReadLabel"
        Me.fileTypeReadLabel.Size = New System.Drawing.Size(72, 16)
        Me.fileTypeReadLabel.TabIndex = 0
        Me.fileTypeReadLabel.Text = "File Type:"
        '
        'readButton
        '
        Me.readButton.Enabled = False
        Me.readButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.readButton.Location = New System.Drawing.Point(120, 88)
        Me.readButton.Name = "readButton"
        Me.readButton.Size = New System.Drawing.Size(80, 24)
        Me.readButton.TabIndex = 6
        Me.readButton.Text = "Read"
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(554, 504)
        Me.Controls.Add(Me.readFromFileGroupBox)
        Me.Controls.Add(Me.writeToFileGroupBox)
        Me.Controls.Add(Me.acquisitionResultGroupBox)
        Me.Controls.Add(Me.timingParametersGroupBox)
        Me.Controls.Add(Me.channelParametersGroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Continuous Acquisition of Voltage Samples - Int Clk - Write to File"
        Me.channelParametersGroupBox.ResumeLayout(False)
        CType(Me.minimumValueNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.maximumValueNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.timingParametersGroupBox.ResumeLayout(False)
        CType(Me.rateNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.samplesPerChannelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.acquisitionResultGroupBox.ResumeLayout(False)
        CType(Me.acquisitionDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.writeToFileGroupBox.ResumeLayout(False)
        Me.writeToFileGroupBox.PerformLayout()
        Me.readFromFileGroupBox.ResumeLayout(False)
        Me.readFromFileGroupBox.PerformLayout()
        Me.ResumeLayout(False)

    End Sub 'InitializeComponent

    Private Sub browseWriteButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles browseWriteButton.Click
        If textFileWriteRadioButton.Checked Then
            useTextFileWrite = True
            writeToFileSaveFileDialog.DefaultExt = "*.txt"
            writeToFileSaveFileDialog.FileName = "acquisitionData.txt"
            writeToFileSaveFileDialog.Filter = "Text Files|*.txt|All Files|*.*"
        Else
            useTextFileWrite = False
            writeToFileSaveFileDialog.DefaultExt = "*.bin"
            writeToFileSaveFileDialog.FileName = "acquisitionData.bin"
            writeToFileSaveFileDialog.Filter = "Binary Files|*.bin|All Files|*.*"
        End If

        ' Display Save File Dialog (Windows forms control)
        Dim result As DialogResult = writeToFileSaveFileDialog.ShowDialog()

        If result = System.Windows.Forms.DialogResult.OK Then
            fileNameWrite = writeToFileSaveFileDialog.FileName
            filePathWriteTextBox.Text = fileNameWrite
            fileToolTip.SetToolTip(filePathWriteTextBox, fileNameWrite)
            startButton.Enabled = True
        End If
    End Sub 'browseWriteButton_Click

    Private Sub browseReadButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles browseReadButton.Click
        If textFileReadRadioButton.Checked Then
            useTextFileRead = True
            readFromFileOpenFileDialog.DefaultExt = "*.txt"
            readFromFileOpenFileDialog.FileName = "acquisitionData.txt"
            readFromFileOpenFileDialog.Filter = "Text Files|*.txt|All Files|*.*"
        Else
            useTextFileRead = False
            readFromFileOpenFileDialog.DefaultExt = "*.bin"
            readFromFileOpenFileDialog.FileName = "acquisitionData.bin"
            readFromFileOpenFileDialog.Filter = "Binary Files|*.bin|All Files|*.*"
        End If

        ' Display Open File Dialog (Windows forms control)
        Dim result As DialogResult = readFromFileOpenFileDialog.ShowDialog()

        If result = System.Windows.Forms.DialogResult.OK Then
            fileNameRead = readFromFileOpenFileDialog.FileName
            filePathReadTextBox.Text = fileNameRead
            fileToolTip.SetToolTip(filePathReadTextBox, fileNameRead)
            readButton.Enabled = True
        End If
    End Sub 'browseReadButton_Click

    Private Sub startButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles startButton.Click
        If runningTask Is Nothing Then
            Try
                ' Create a new file for data
                Dim opened As Boolean = CreateDataFile()
                If Not opened Then
                    Return
                End If

                ' Modify the UI
                stopButton.Enabled = True
                startButton.Enabled = False
                'Create a new task
                myTask = New Task()

                'Create a virtual channel
                myTask.AIChannels.CreateVoltageChannel(physicalChannelComboBox.Text, "", CType(-1, AITerminalConfiguration), Convert.ToDouble(minimumValueNumeric.Value), Convert.ToDouble(maximumValueNumeric.Value), AIVoltageUnits.Volts)

                'Configure the timing parameters
                myTask.Timing.ConfigureSampleClock("", Convert.ToDouble(rateNumeric.Value), SampleClockActiveEdge.Rising, SampleQuantityMode.ContinuousSamples, 1000)

                'Verify the Task
                myTask.Control(TaskAction.Verify)

                'Prepare the table and file for Data
                Dim channelNames(myTask.AIChannels.Count - 1) As String
                Dim i As Integer = 0
                Dim a As AIChannel
                For Each a In myTask.AIChannels
                    channelNames(i) = a.PhysicalName
                    i = i + 1
                Next a

                InitializeDataTable(channelNames, dataTable)
                acquisitionDataGrid.DataSource = dataTable

                ' Add the channel names (and any other information) to the file
                Dim samples As Integer = Convert.ToInt32(samplesPerChannelNumeric.Value)
                PrepareFileForData()
                savedData = New ArrayList
                For i = 0 To myTask.AIChannels.Count - 1
                    savedData.Add(New ArrayList)
                Next

                runningTask = myTask
                analogInReader = New AnalogMultiChannelReader(myTask.Stream)

                ' Use SynchronizeCallbacks to specify that the object 
                ' marshals callbacks across threads appropriately.
                analogInReader.SynchronizeCallbacks = True

                analogCallback = New AsyncCallback(AddressOf AnalogInCallback)

                analogInReader.BeginReadMultiSample(samples, analogCallback, myTask)

            Catch exception As DaqException
                'Display Errors
                MessageBox.Show(exception.Message)
                runningTask = Nothing
                myTask.Dispose()
                stopButton.Enabled = False
                startButton.Enabled = True
                writeToFileGroupBox.Enabled = True
            End Try
        End If
    End Sub 'startButton_Click

    Private Sub AnalogInCallback(ByVal ar As IAsyncResult)
        Try
            If (Not (runningTask Is Nothing)) AndAlso runningTask Is ar.AsyncState Then
                'Read the available data from the channels
                data = analogInReader.EndReadMultiSample(ar)

                'Plot your data here
                'Displays data in grid and writes to file
                DisplayData(data, dataTable)

                LogData(data)

                analogInReader.BeginReadMultiSample(Convert.ToInt32(samplesPerChannelNumeric.Value), analogCallback, myTask)
            End If
        Catch exception As DaqException
            'Display Errors
            MessageBox.Show(exception.Message)
            runningTask = Nothing
            myTask.Dispose()
            stopButton.Enabled = False
            startButton.Enabled = True
            writeToFileGroupBox.Enabled = True
        End Try
    End Sub 'AnalogInCallback

    Private Sub stopButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles stopButton.Click
        If Not (runningTask Is Nothing) Then
            'Dispose of the task
            CloseFile()

            runningTask = Nothing
            myTask.Dispose()
            stopButton.Enabled = False
            startButton.Enabled = True
            writeToFileGroupBox.Enabled = True
        End If
    End Sub 'stopButton_Click

    Private Sub readButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles readButton.Click
        ' Modify UI
        readButton.Enabled = False
        Me.Cursor = Cursors.WaitCursor

        ' Open file
        Dim opened As Boolean = OpenDataFile()

        If opened Then
            ' Load data
            If useTextFileRead Then
                ReadTextData()
                fileStreamReader.Close()
            Else
                ReadBinaryData()
                fileBinaryReader.Close()
            End If
        End If

        Me.Cursor = Cursors.Default
        readButton.Enabled = True
    End Sub 'readButton_Click

    Private Sub DisplayData(ByVal sourceArray(,) As Double, ByRef dataTable As dataTable)
        Try
            Dim channelCount As Integer = sourceArray.GetLength(0)
            Dim dataCount As Integer

            ' Display the first 10 points of the Read/Write in the Datagrid
            If sourceArray.GetLength(1) < 10 Then
                dataCount = sourceArray.GetLength(1)
            Else
                dataCount = 10
            End If
            ' Write to Data Table
            Dim i As Integer
            For i = 0 To dataCount - 1
                Dim j As Integer
                For j = 0 To channelCount - 1
                    ' Writes data to data table
                    dataTable.Rows(i)(j) = sourceArray.GetValue(j, i)
                Next j
            Next i
        Catch e As Exception
            MessageBox.Show(e.ToString())
            runningTask = Nothing
            myTask.Dispose()
            stopButton.Enabled = False
            startButton.Enabled = True
            writeToFileGroupBox.Enabled = True
        End Try
    End Sub 'DisplayData

    Private Sub LogData(ByVal data(,) As Double)
        Dim channelCount As Integer = data.GetLength(0)
        Dim dataCount As Integer = data.GetLength(1)

        Dim i As Integer
        Dim j As Integer
        For i = 0 To channelCount - 1
            Dim l As ArrayList
            l = savedData(i)

            For j = 0 To dataCount - 1
                l.Add(data(i, j))
            Next
        Next
    End Sub

    Private Sub CloseFile()
        Dim channelCount As Integer = savedData.Count
        Dim l As ArrayList = savedData(0)
        Dim dataCount As Integer = l.Count

        Try
            If useTextFileWrite Then
                fileStreamWriter.WriteLine(dataCount.ToString())

                Dim i As Integer
                For i = 0 To dataCount - 1
                    Dim j As Integer
                    For j = 0 To channelCount - 1
                        ' Writes data to file
                        l = savedData(j)
                        Dim dataValue As Double = l(i)
                        fileStreamWriter.Write(dataValue.ToString("e6"))
                        fileStreamWriter.Write(ControlChars.Tab) 'seperate the data for each channel
                    Next j
                    fileStreamWriter.WriteLine() 'new line of data (start next scan)
                Next i

                fileStreamWriter.Close()
            Else
                fileBinaryWriter.Write(dataCount.ToString())

                Dim i As Integer
                For i = 0 To dataCount - 1
                    Dim j As Integer
                    For j = 0 To channelCount - 1
                        ' Writes data to file
                        l = savedData(j)
                        Dim dataValue As Double = l(i)
                        fileBinaryWriter.Write(dataValue)
                    Next j
                Next i

                fileBinaryWriter.Close()
            End If
        Catch e As Exception
            MessageBox.Show(e.TargetSite.ToString())
            runningTask = Nothing
            myTask.Dispose()
            stopButton.Enabled = False
            startButton.Enabled = True
            writeToFileGroupBox.Enabled = True
        End Try
    End Sub 'LogData

    Private Sub ReadTextData()
        Try
            Dim tabchar(0) As Char
            tabchar(0) = ControlChars.Tab
            Dim split() As String = fileStreamReader.ReadLine().Replace(ControlChars.Lf, "").Split(tabchar)
            Dim channels(split.GetLength(0) - 2) As String
            System.Array.Copy(split, 0, channels, 0, split.GetLength(0) - 1)
            Dim samples As Integer = Int32.Parse(fileStreamReader.ReadLine().Replace(ControlChars.Lf, ""))
            Dim channelCount As Integer = channels.GetLength(0)

            Dim array(channelCount - 1, samples - 1) As Double

            Dim line As String
            Dim iSample As Integer
            For iSample = 0 To samples - 1
                line = fileStreamReader.ReadLine()
                Dim values() As String = line.Split(tabchar)

                Dim iChan As Integer
                For iChan = 0 To channelCount - 1
                    array(iChan, iSample) = Convert.ToDouble(values(iChan))
                Next iChan
            Next iSample

            InitializeDataTable(channels, dataTable)
            acquisitionDataGrid.DataSource = dataTable
            DisplayData(array, dataTable)
        Catch e As Exception
            MessageBox.Show(e.ToString())
            runningTask = Nothing
            readButton.Enabled = True
            readFromFileGroupBox.Enabled = True
        End Try
    End Sub 'ReadTextData

    Private Sub ReadBinaryData()
        Try
            Dim s As String
            Dim arrayList As New ArrayList
            s = fileBinaryReader.ReadString()
            While s <> ControlChars.Cr + ControlChars.Lf
                arrayList.Add(s)
                s = fileBinaryReader.ReadString()
            End While

            Dim channels() As String = arrayList.ToArray(GetType(String))
            Dim samples As Integer = Int32.Parse(fileBinaryReader.ReadString())
            Dim channelCount As Integer = channels.GetLength(0)

            Dim array(channelCount - 1, samples - 1) As Double

            Dim iSample As Integer
            For iSample = 0 To samples - 1
                Dim iChan As Integer
                For iChan = 0 To channelCount - 1
                    array(iChan, iSample) = fileBinaryReader.ReadDouble()
                Next iChan
            Next iSample

            InitializeDataTable(channels, dataTable)
            acquisitionDataGrid.DataSource = dataTable
            DisplayData(array, dataTable)
        Catch e As Exception
            MessageBox.Show(e.ToString())
            runningTask = Nothing
            readButton.Enabled = True
            readFromFileGroupBox.Enabled = True
        End Try
    End Sub 'ReadBinaryData

    Public Sub InitializeDataTable(ByVal channelNames() As String, ByRef data As dataTable)
        Dim numChannels As Integer = channelNames.GetLength(0)
        data.Rows.Clear()
        data.Columns.Clear()
        dataColumn = New DataColumn(numChannels - 1) {}
        Dim numRows As Integer = 10

        Dim i As Integer
        For i = 0 To numChannels - 1
            dataColumn(i) = New DataColumn
            dataColumn(i).DataType = GetType(Double)
            dataColumn(i).ColumnName = channelNames(i)
        Next i

        data.Columns.AddRange(dataColumn)

        For i = 0 To numRows - 1
            Dim rowArr(numChannels - 1) As Object
            data.Rows.Add(rowArr)
        Next i
    End Sub 'InitializeDataTable

    'Creates a text/binary stream based on the user selections
    Private Function CreateDataFile() As Boolean
        Try

            Dim fs As New FileStream(fileNameWrite, FileMode.Create)
            If useTextFileWrite Then
                fileStreamWriter = New StreamWriter(fs)
            Else
                fileBinaryWriter = New BinaryWriter(fs)
            End If
        Catch ex As System.IO.IOException
            MessageBox.Show(ex.Message)
            Return False
        End Try

        Return True
    End Function 'CreateDataFile

    ' Opens a text/binary stream based on the user selections
    Private Function OpenDataFile() As Boolean
        Try
            Dim fs As New FileStream(fileNameRead, FileMode.Open)
            If useTextFileRead Then
                fileStreamReader = New StreamReader(fs)
            Else
                fileBinaryReader = New BinaryReader(fs)
            End If
        Catch ex As System.IO.IOException
            MessageBox.Show(ex.Message)
            Return False
        End Try

        Return True
    End Function 'OpenDataFile

    Private Sub PrepareFileForData()
        'Prepare file for data (Write out the channel names)
        Dim numChannels As Integer = myTask.AIChannels.Count

        If useTextFileWrite Then
            Dim i As Integer
            For i = 0 To numChannels - 1
                fileStreamWriter.Write(myTask.AIChannels(i).PhysicalName)
                fileStreamWriter.Write(ControlChars.Tab)
            Next i
            fileStreamWriter.WriteLine()
        Else
            Dim i As Integer
            For i = 0 To numChannels - 1
                fileBinaryWriter.Write(myTask.AIChannels(i).PhysicalName)
            Next i
            fileBinaryWriter.Write(ControlChars.Cr + ControlChars.Lf)
        End If
    End Sub 'PrepareFileForData

    Private Sub textFileWriteRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles textFileWriteRadioButton.CheckedChanged
        If textFileWriteRadioButton.Checked Then
            useTextFileWrite = True
        End If

        startButton.Enabled = False
    End Sub 'textFileWriteRadioButton_CheckedChanged


    Private Sub binaryFileWriteRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles binaryFileWriteRadioButton.CheckedChanged
        If binaryFileWriteRadioButton.Checked Then
            useTextFileWrite = False
        End If

        startButton.Enabled = False
    End Sub 'binaryFileWriteRadioButton_CheckedChanged

    Private Sub textFileReadRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles textFileReadRadioButton.CheckedChanged
        If textFileReadRadioButton.Checked Then
            useTextFileRead = True
        End If

        readButton.Enabled = False
    End Sub 'textFileReadRadioButton_CheckedChanged


    Private Sub binaryFileReadRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles binaryFileReadRadioButton.CheckedChanged
        If binaryFileReadRadioButton.Checked Then
            useTextFileRead = True
        End If

        readButton.Enabled = False
    End Sub 'binaryFileReadRadioButton_CheckedChanged
End Class 'MainForm
