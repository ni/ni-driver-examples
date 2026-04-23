'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   AIAOShardTimebaseAndTrig_DSA
'
' Category:
'   Synchronization
'
' Description:
'   This example synchronizes the clocks and trigger on two Dynamic Signal
'   Acquistion (DSA) 
'   devices and performs continuous analog input and output.  NOTE: This example
'   is intended to show low 
'   level synchronization of various devices. DSA and S Series devices now support
'   including channels from 
'   multiple devices in a single task. DAQmx automatically synchronizes the
'   devices in such a task. See the 
'   DAQmx Help>>NI-DAQmx Device Considerations>>Multidevice Tasks
'   section for further details.NOTE: 
'   If you are using PXI DSA devices along with sample clock timebase
'   synchronization, the master device 
'   must reside in PXI slot 2.NOTE: This code will not run "as-is" on a
'   multifunction (MIO) DAQ device.
'
' Instructions for running:
'   1.  Select which type of Syncrhonization method you want to use.
'   2.  Select the physical channel to correspond to where your signal is input
'       and output on the DSA devices.
'   3.  Enter the minimum and maximum voltage range.Note: For better accuracy
'       try to match the input range to the expected voltage level of the
'       measured signal.
'   4.  Set the number of samples to acquire per channel.
'   5.  Set the rate of the acquisition.Note: The rate should be at least twice
'       as fast as the maximum frequency component of the signal being acquired.
'
' Steps:
'   1.  Create analog input and output voltage channels for both the Master and
'       Slave devices.
'   2.  Set timing parameters for continuous generation and acquisition.  The
'       sample rate and number of samples are set to the same values for each
'       device.
'   3.  PXI DSA devices require two timing parameters to be shared between the
'       two devices.  The first signal is the "Master Clock Timebase", shared
'       across the PXI_Star bus.  The second signal is the Sync Pulse.  This
'       signal is shared across the PXI_Trig / RTSI bus.  Use the Timing
'       subobject to configure these signals on the slave.  Prefix the signal
'       strings with the name of the Master Device.  In this example, the name
'       of the Master Device is determined from the physical channel names of
'       the Master Input task.
'   4.  Configure a digital start trigger on the slave tasks and master output
'       task to use the acquisition start signal from the master input task. 
'       Prefix the signal name with the name of the Master Device, as before. 
'       The signal is shared along the PXI_Trig / RTSI bus.
'   5.  Call Task.Start() to start the acquisition.Note: The slave tasks and
'       master output task start before the master input task because these
'       tasks are waiting on the master input task for the synchronization
'       pulse.
'   6.  Read data from the input tasks and display it.
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
'   Make sure your signal input terminals match the Physical Channel I/O
'   control.  Ensure that your PXI chassis has been properly identified in MAX.
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

Imports NationalInstruments
Imports NationalInstruments.DAQmx

Public Class MainForm
    Inherits System.Windows.Forms.Form
    Private masterIn As SyncTaskReader = Nothing
    Private masterOut As SyncTaskWriter = Nothing
    Private slaveIn As SyncTaskReader = Nothing
    Private slaveOut As SyncTaskWriter = Nothing
    Private masterDataTable As DataTable = Nothing
    Private slaveDataTable As DataTable = Nothing
    Private masterDataColumns As DataColumn() = Nothing
    Private slaveDataColumns As DataColumn() = Nothing
    Private masterCallback As AsyncCallback
    Private slaveCallback As AsyncCallback
    Private fGen As NationalInstruments.Examples.FunctionGenerator
    Private runningTask As SyncTaskReader
    Private WithEvents waveformTypeComboBox As System.Windows.Forms.ComboBox
    Private WithEvents amplitudeNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents waveformTypeLabel As System.Windows.Forms.Label
    Private WithEvents slaveMinValNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents slaveInputChanLabel As System.Windows.Forms.Label
    Private WithEvents slaveMaxValLabel As System.Windows.Forms.Label
    Private WithEvents slaveMinValLabel As System.Windows.Forms.Label
    Private WithEvents slaveMaxValNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents slaveOutputChanLabel As System.Windows.Forms.Label
    Private WithEvents slaveDataGrid As System.Windows.Forms.DataGrid
    Private WithEvents masterDataGrid As System.Windows.Forms.DataGrid
    Private WithEvents syncTypeLabel As System.Windows.Forms.Label
    Private WithEvents syncTypeComboBox As System.Windows.Forms.ComboBox
    Private WithEvents amplitudeLabel As System.Windows.Forms.Label
    Private WithEvents samplesPerBufferLabel As System.Windows.Forms.Label
    Private WithEvents samplesPerBufferNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents cyclesPerBufferNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents cyclesPerBufferLabel As System.Windows.Forms.Label
    Private WithEvents slaveInputChannelComboBox As System.Windows.Forms.ComboBox
    Private WithEvents masterMinValNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents masterInputChanLabel As System.Windows.Forms.Label
    Private WithEvents masterMaxValLabel As System.Windows.Forms.Label
    Private WithEvents masterMinValLabel As System.Windows.Forms.Label
    Private WithEvents masterMaxValNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents masterOutputChannelComboBox As System.Windows.Forms.ComboBox
    Private WithEvents syncGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents masterDataGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents masterChanGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents masterInputChannelComboBox As System.Windows.Forms.ComboBox
    Private WithEvents masterOutputChanLabel As System.Windows.Forms.Label
    Private WithEvents slaveDataGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents rateLabel As System.Windows.Forms.Label
    Private WithEvents samplesPerChannelNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents startButton As System.Windows.Forms.Button
    Private WithEvents slaveOutputChannelComboBox As System.Windows.Forms.ComboBox
    Private WithEvents slaveChanGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents samplesPerChannelLabel As System.Windows.Forms.Label
    Private WithEvents timingGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents rateNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents stopButton As System.Windows.Forms.Button
    Private WithEvents waveformGroupBox As System.Windows.Forms.GroupBox


    Private components As System.ComponentModel.Container = Nothing


    Public Sub New()
        MyBase.New()
        Application.EnableVisualStyles()

        '
        ' Required for Windows Form Designer support
        '
        InitializeComponent()

        ' Initialize UI
        masterInputChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AI, PhysicalChannelAccess.External))
        masterOutputChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AO, PhysicalChannelAccess.External))
        slaveInputChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AI, PhysicalChannelAccess.External))
        slaveOutputChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AO, PhysicalChannelAccess.External))

        ConfigNumeric(masterMaxValNumeric)
        ConfigNumeric(masterMinValNumeric)
        ConfigNumeric(slaveMaxValNumeric)
        ConfigNumeric(slaveMinValNumeric)
        ConfigNumeric(samplesPerChannelNumeric, [Decimal].Zero)
        ConfigNumeric(rateNumeric, [Decimal].Zero)

        waveformTypeComboBox.SelectedIndex = 0
        syncTypeComboBox.SelectedIndex = 0

        ' Set up the AI data tables
        masterDataTable = New DataTable
        slaveDataTable = New DataTable

        InitializeDataTables(28)
        masterDataGrid.DataSource = masterDataTable
        slaveDataGrid.DataSource = slaveDataTable
    End Sub 'New

    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
            If Not (masterIn Is Nothing) Then
                runningTask = Nothing
                masterIn.Dispose()
            End If
            If Not (slaveIn Is Nothing) Then
                slaveIn.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub 'Dispose

    Private Sub InitializeComponent()
        Me.waveformTypeComboBox = New System.Windows.Forms.ComboBox
        Me.amplitudeNumeric = New System.Windows.Forms.NumericUpDown
        Me.waveformTypeLabel = New System.Windows.Forms.Label
        Me.slaveMinValNumeric = New System.Windows.Forms.NumericUpDown
        Me.slaveInputChanLabel = New System.Windows.Forms.Label
        Me.slaveMaxValLabel = New System.Windows.Forms.Label
        Me.slaveMinValLabel = New System.Windows.Forms.Label
        Me.slaveMaxValNumeric = New System.Windows.Forms.NumericUpDown
        Me.slaveOutputChanLabel = New System.Windows.Forms.Label
        Me.slaveDataGrid = New System.Windows.Forms.DataGrid
        Me.masterDataGrid = New System.Windows.Forms.DataGrid
        Me.syncTypeLabel = New System.Windows.Forms.Label
        Me.syncTypeComboBox = New System.Windows.Forms.ComboBox
        Me.amplitudeLabel = New System.Windows.Forms.Label
        Me.samplesPerBufferLabel = New System.Windows.Forms.Label
        Me.samplesPerBufferNumeric = New System.Windows.Forms.NumericUpDown
        Me.cyclesPerBufferNumeric = New System.Windows.Forms.NumericUpDown
        Me.cyclesPerBufferLabel = New System.Windows.Forms.Label
        Me.slaveInputChannelComboBox = New System.Windows.Forms.ComboBox
        Me.masterMinValNumeric = New System.Windows.Forms.NumericUpDown
        Me.masterInputChanLabel = New System.Windows.Forms.Label
        Me.masterMaxValLabel = New System.Windows.Forms.Label
        Me.masterMinValLabel = New System.Windows.Forms.Label
        Me.masterMaxValNumeric = New System.Windows.Forms.NumericUpDown
        Me.masterOutputChannelComboBox = New System.Windows.Forms.ComboBox
        Me.syncGroupBox = New System.Windows.Forms.GroupBox
        Me.masterDataGroupBox = New System.Windows.Forms.GroupBox
        Me.masterChanGroupBox = New System.Windows.Forms.GroupBox
        Me.masterInputChannelComboBox = New System.Windows.Forms.ComboBox
        Me.masterOutputChanLabel = New System.Windows.Forms.Label
        Me.slaveDataGroupBox = New System.Windows.Forms.GroupBox
        Me.rateLabel = New System.Windows.Forms.Label
        Me.samplesPerChannelNumeric = New System.Windows.Forms.NumericUpDown
        Me.startButton = New System.Windows.Forms.Button
        Me.slaveOutputChannelComboBox = New System.Windows.Forms.ComboBox
        Me.slaveChanGroupBox = New System.Windows.Forms.GroupBox
        Me.samplesPerChannelLabel = New System.Windows.Forms.Label
        Me.timingGroupBox = New System.Windows.Forms.GroupBox
        Me.rateNumeric = New System.Windows.Forms.NumericUpDown
        Me.stopButton = New System.Windows.Forms.Button
        Me.waveformGroupBox = New System.Windows.Forms.GroupBox
        CType(Me.amplitudeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.slaveMinValNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.slaveMaxValNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.slaveDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.masterDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.samplesPerBufferNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cyclesPerBufferNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.masterMinValNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.masterMaxValNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.syncGroupBox.SuspendLayout()
        Me.masterDataGroupBox.SuspendLayout()
        Me.masterChanGroupBox.SuspendLayout()
        Me.slaveDataGroupBox.SuspendLayout()
        CType(Me.samplesPerChannelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.slaveChanGroupBox.SuspendLayout()
        Me.timingGroupBox.SuspendLayout()
        CType(Me.rateNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.waveformGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'waveformTypeComboBox
        '
        Me.waveformTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.waveformTypeComboBox.Items.AddRange(New Object() {"Sine"})
        Me.waveformTypeComboBox.Location = New System.Drawing.Point(152, 20)
        Me.waveformTypeComboBox.Name = "waveformTypeComboBox"
        Me.waveformTypeComboBox.Size = New System.Drawing.Size(168, 21)
        Me.waveformTypeComboBox.TabIndex = 1
        '
        'amplitudeNumeric
        '
        Me.amplitudeNumeric.DecimalPlaces = 3
        Me.amplitudeNumeric.Location = New System.Drawing.Point(152, 46)
        Me.amplitudeNumeric.Maximum = New Decimal(New Integer() {102400, 0, 0, 0})
        Me.amplitudeNumeric.Name = "amplitudeNumeric"
        Me.amplitudeNumeric.Size = New System.Drawing.Size(168, 20)
        Me.amplitudeNumeric.TabIndex = 3
        Me.amplitudeNumeric.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'waveformTypeLabel
        '
        Me.waveformTypeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.waveformTypeLabel.Location = New System.Drawing.Point(16, 22)
        Me.waveformTypeLabel.Name = "waveformTypeLabel"
        Me.waveformTypeLabel.Size = New System.Drawing.Size(96, 16)
        Me.waveformTypeLabel.TabIndex = 0
        Me.waveformTypeLabel.Text = "Waveform Type:"
        '
        'slaveMinValNumeric
        '
        Me.slaveMinValNumeric.DecimalPlaces = 2
        Me.slaveMinValNumeric.Location = New System.Drawing.Point(152, 93)
        Me.slaveMinValNumeric.Minimum = New Decimal(New Integer() {100, 0, 0, -2147483648})
        Me.slaveMinValNumeric.Name = "slaveMinValNumeric"
        Me.slaveMinValNumeric.Size = New System.Drawing.Size(168, 20)
        Me.slaveMinValNumeric.TabIndex = 7
        Me.slaveMinValNumeric.Value = New Decimal(New Integer() {10, 0, 0, -2147483648})
        '
        'slaveInputChanLabel
        '
        Me.slaveInputChanLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.slaveInputChanLabel.Location = New System.Drawing.Point(16, 17)
        Me.slaveInputChanLabel.Name = "slaveInputChanLabel"
        Me.slaveInputChanLabel.Size = New System.Drawing.Size(96, 16)
        Me.slaveInputChanLabel.TabIndex = 0
        Me.slaveInputChanLabel.Text = "Input Channel:"
        '
        'slaveMaxValLabel
        '
        Me.slaveMaxValLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.slaveMaxValLabel.Location = New System.Drawing.Point(16, 69)
        Me.slaveMaxValLabel.Name = "slaveMaxValLabel"
        Me.slaveMaxValLabel.Size = New System.Drawing.Size(96, 16)
        Me.slaveMaxValLabel.TabIndex = 4
        Me.slaveMaxValLabel.Text = "Maximum Value:"
        '
        'slaveMinValLabel
        '
        Me.slaveMinValLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.slaveMinValLabel.Location = New System.Drawing.Point(16, 95)
        Me.slaveMinValLabel.Name = "slaveMinValLabel"
        Me.slaveMinValLabel.Size = New System.Drawing.Size(96, 16)
        Me.slaveMinValLabel.TabIndex = 6
        Me.slaveMinValLabel.Text = "Minimum Value:"
        '
        'slaveMaxValNumeric
        '
        Me.slaveMaxValNumeric.DecimalPlaces = 2
        Me.slaveMaxValNumeric.Location = New System.Drawing.Point(152, 69)
        Me.slaveMaxValNumeric.Name = "slaveMaxValNumeric"
        Me.slaveMaxValNumeric.Size = New System.Drawing.Size(168, 20)
        Me.slaveMaxValNumeric.TabIndex = 5
        Me.slaveMaxValNumeric.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'slaveOutputChanLabel
        '
        Me.slaveOutputChanLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.slaveOutputChanLabel.Location = New System.Drawing.Point(16, 43)
        Me.slaveOutputChanLabel.Name = "slaveOutputChanLabel"
        Me.slaveOutputChanLabel.Size = New System.Drawing.Size(96, 16)
        Me.slaveOutputChanLabel.TabIndex = 2
        Me.slaveOutputChanLabel.Text = "Output Channel:"
        '
        'slaveDataGrid
        '
        Me.slaveDataGrid.AllowSorting = False
        Me.slaveDataGrid.DataMember = ""
        Me.slaveDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.slaveDataGrid.Location = New System.Drawing.Point(3, 19)
        Me.slaveDataGrid.Name = "slaveDataGrid"
        Me.slaveDataGrid.PreferredColumnWidth = 150
        Me.slaveDataGrid.ReadOnly = True
        Me.slaveDataGrid.Size = New System.Drawing.Size(205, 507)
        Me.slaveDataGrid.TabIndex = 0
        '
        'masterDataGrid
        '
        Me.masterDataGrid.AllowSorting = False
        Me.masterDataGrid.DataMember = ""
        Me.masterDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.masterDataGrid.Location = New System.Drawing.Point(3, 19)
        Me.masterDataGrid.Name = "masterDataGrid"
        Me.masterDataGrid.PreferredColumnWidth = 150
        Me.masterDataGrid.ReadOnly = True
        Me.masterDataGrid.Size = New System.Drawing.Size(205, 508)
        Me.masterDataGrid.TabIndex = 0
        '
        'syncTypeLabel
        '
        Me.syncTypeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.syncTypeLabel.Location = New System.Drawing.Point(16, 24)
        Me.syncTypeLabel.Name = "syncTypeLabel"
        Me.syncTypeLabel.Size = New System.Drawing.Size(120, 16)
        Me.syncTypeLabel.TabIndex = 0
        Me.syncTypeLabel.Text = "Synchronization Type:"
        '
        'syncTypeComboBox
        '
        Me.syncTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.syncTypeComboBox.Items.AddRange(New Object() {"DSA Sample Clock Timebase", "DSA Reference Clock"})
        Me.syncTypeComboBox.Location = New System.Drawing.Point(152, 22)
        Me.syncTypeComboBox.Name = "syncTypeComboBox"
        Me.syncTypeComboBox.Size = New System.Drawing.Size(168, 21)
        Me.syncTypeComboBox.TabIndex = 1
        '
        'amplitudeLabel
        '
        Me.amplitudeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.amplitudeLabel.Location = New System.Drawing.Point(16, 48)
        Me.amplitudeLabel.Name = "amplitudeLabel"
        Me.amplitudeLabel.Size = New System.Drawing.Size(96, 16)
        Me.amplitudeLabel.TabIndex = 2
        Me.amplitudeLabel.Text = "Amplitude:"
        '
        'samplesPerBufferLabel
        '
        Me.samplesPerBufferLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.samplesPerBufferLabel.Location = New System.Drawing.Point(16, 74)
        Me.samplesPerBufferLabel.Name = "samplesPerBufferLabel"
        Me.samplesPerBufferLabel.Size = New System.Drawing.Size(104, 16)
        Me.samplesPerBufferLabel.TabIndex = 4
        Me.samplesPerBufferLabel.Text = "Samples per Buffer:"
        '
        'samplesPerBufferNumeric
        '
        Me.samplesPerBufferNumeric.Location = New System.Drawing.Point(152, 72)
        Me.samplesPerBufferNumeric.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.samplesPerBufferNumeric.Name = "samplesPerBufferNumeric"
        Me.samplesPerBufferNumeric.Size = New System.Drawing.Size(168, 20)
        Me.samplesPerBufferNumeric.TabIndex = 5
        Me.samplesPerBufferNumeric.Value = New Decimal(New Integer() {250, 0, 0, 0})
        '
        'cyclesPerBufferNumeric
        '
        Me.cyclesPerBufferNumeric.DecimalPlaces = 2
        Me.cyclesPerBufferNumeric.Location = New System.Drawing.Point(152, 122)
        Me.cyclesPerBufferNumeric.Maximum = New Decimal(New Integer() {102400, 0, 0, 0})
        Me.cyclesPerBufferNumeric.Name = "cyclesPerBufferNumeric"
        Me.cyclesPerBufferNumeric.Size = New System.Drawing.Size(168, 20)
        Me.cyclesPerBufferNumeric.TabIndex = 7
        Me.cyclesPerBufferNumeric.Value = New Decimal(New Integer() {5, 0, 0, 0})
        '
        'cyclesPerBufferLabel
        '
        Me.cyclesPerBufferLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.cyclesPerBufferLabel.Location = New System.Drawing.Point(16, 124)
        Me.cyclesPerBufferLabel.Name = "cyclesPerBufferLabel"
        Me.cyclesPerBufferLabel.Size = New System.Drawing.Size(96, 16)
        Me.cyclesPerBufferLabel.TabIndex = 6
        Me.cyclesPerBufferLabel.Text = "Cycles per Buffer:"
        '
        'slaveInputChannelComboBox
        '
        Me.slaveInputChannelComboBox.Location = New System.Drawing.Point(152, 15)
        Me.slaveInputChannelComboBox.Name = "slaveInputChannelComboBox"
        Me.slaveInputChannelComboBox.Size = New System.Drawing.Size(168, 21)
        Me.slaveInputChannelComboBox.TabIndex = 1
        Me.slaveInputChannelComboBox.Text = "Dev3/ai0"
        '
        'masterMinValNumeric
        '
        Me.masterMinValNumeric.DecimalPlaces = 2
        Me.masterMinValNumeric.Location = New System.Drawing.Point(152, 93)
        Me.masterMinValNumeric.Minimum = New Decimal(New Integer() {100, 0, 0, -2147483648})
        Me.masterMinValNumeric.Name = "masterMinValNumeric"
        Me.masterMinValNumeric.Size = New System.Drawing.Size(168, 20)
        Me.masterMinValNumeric.TabIndex = 7
        Me.masterMinValNumeric.Value = New Decimal(New Integer() {10, 0, 0, -2147483648})
        '
        'masterInputChanLabel
        '
        Me.masterInputChanLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.masterInputChanLabel.Location = New System.Drawing.Point(16, 17)
        Me.masterInputChanLabel.Name = "masterInputChanLabel"
        Me.masterInputChanLabel.Size = New System.Drawing.Size(96, 16)
        Me.masterInputChanLabel.TabIndex = 0
        Me.masterInputChanLabel.Text = "Input Channel:"
        '
        'masterMaxValLabel
        '
        Me.masterMaxValLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.masterMaxValLabel.Location = New System.Drawing.Point(16, 69)
        Me.masterMaxValLabel.Name = "masterMaxValLabel"
        Me.masterMaxValLabel.Size = New System.Drawing.Size(96, 16)
        Me.masterMaxValLabel.TabIndex = 4
        Me.masterMaxValLabel.Text = "Maximum Value:"
        '
        'masterMinValLabel
        '
        Me.masterMinValLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.masterMinValLabel.Location = New System.Drawing.Point(16, 95)
        Me.masterMinValLabel.Name = "masterMinValLabel"
        Me.masterMinValLabel.Size = New System.Drawing.Size(96, 16)
        Me.masterMinValLabel.TabIndex = 6
        Me.masterMinValLabel.Text = "Minimum Value:"
        '
        'masterMaxValNumeric
        '
        Me.masterMaxValNumeric.DecimalPlaces = 2
        Me.masterMaxValNumeric.Location = New System.Drawing.Point(152, 67)
        Me.masterMaxValNumeric.Name = "masterMaxValNumeric"
        Me.masterMaxValNumeric.Size = New System.Drawing.Size(168, 20)
        Me.masterMaxValNumeric.TabIndex = 5
        Me.masterMaxValNumeric.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'masterOutputChannelComboBox
        '
        Me.masterOutputChannelComboBox.Location = New System.Drawing.Point(152, 39)
        Me.masterOutputChannelComboBox.Name = "masterOutputChannelComboBox"
        Me.masterOutputChannelComboBox.Size = New System.Drawing.Size(168, 21)
        Me.masterOutputChannelComboBox.TabIndex = 3
        Me.masterOutputChannelComboBox.Text = "Dev2/ao0"
        '
        'syncGroupBox
        '
        Me.syncGroupBox.Controls.Add(Me.syncTypeLabel)
        Me.syncGroupBox.Controls.Add(Me.syncTypeComboBox)
        Me.syncGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.syncGroupBox.Location = New System.Drawing.Point(6, 8)
        Me.syncGroupBox.Name = "syncGroupBox"
        Me.syncGroupBox.Size = New System.Drawing.Size(336, 56)
        Me.syncGroupBox.TabIndex = 17
        Me.syncGroupBox.TabStop = False
        Me.syncGroupBox.Text = "Synchronization Parameters"
        '
        'masterDataGroupBox
        '
        Me.masterDataGroupBox.Controls.Add(Me.masterDataGrid)
        Me.masterDataGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.masterDataGroupBox.Location = New System.Drawing.Point(342, 8)
        Me.masterDataGroupBox.Name = "masterDataGroupBox"
        Me.masterDataGroupBox.Size = New System.Drawing.Size(208, 524)
        Me.masterDataGroupBox.TabIndex = 13
        Me.masterDataGroupBox.TabStop = False
        Me.masterDataGroupBox.Text = "Master Data"
        '
        'masterChanGroupBox
        '
        Me.masterChanGroupBox.Controls.Add(Me.masterOutputChannelComboBox)
        Me.masterChanGroupBox.Controls.Add(Me.masterInputChannelComboBox)
        Me.masterChanGroupBox.Controls.Add(Me.masterMinValNumeric)
        Me.masterChanGroupBox.Controls.Add(Me.masterInputChanLabel)
        Me.masterChanGroupBox.Controls.Add(Me.masterMaxValLabel)
        Me.masterChanGroupBox.Controls.Add(Me.masterMinValLabel)
        Me.masterChanGroupBox.Controls.Add(Me.masterMaxValNumeric)
        Me.masterChanGroupBox.Controls.Add(Me.masterOutputChanLabel)
        Me.masterChanGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.masterChanGroupBox.Location = New System.Drawing.Point(6, 73)
        Me.masterChanGroupBox.Name = "masterChanGroupBox"
        Me.masterChanGroupBox.Size = New System.Drawing.Size(336, 119)
        Me.masterChanGroupBox.TabIndex = 9
        Me.masterChanGroupBox.TabStop = False
        Me.masterChanGroupBox.Text = "Channel Parameters - Master"
        '
        'masterInputChannelComboBox
        '
        Me.masterInputChannelComboBox.Location = New System.Drawing.Point(152, 15)
        Me.masterInputChannelComboBox.Name = "masterInputChannelComboBox"
        Me.masterInputChannelComboBox.Size = New System.Drawing.Size(168, 21)
        Me.masterInputChannelComboBox.TabIndex = 1
        Me.masterInputChannelComboBox.Text = "Dev2/ai0"
        '
        'masterOutputChanLabel
        '
        Me.masterOutputChanLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.masterOutputChanLabel.Location = New System.Drawing.Point(16, 43)
        Me.masterOutputChanLabel.Name = "masterOutputChanLabel"
        Me.masterOutputChanLabel.Size = New System.Drawing.Size(96, 16)
        Me.masterOutputChanLabel.TabIndex = 2
        Me.masterOutputChanLabel.Text = "Output Channel:"
        '
        'slaveDataGroupBox
        '
        Me.slaveDataGroupBox.Controls.Add(Me.slaveDataGrid)
        Me.slaveDataGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.slaveDataGroupBox.Location = New System.Drawing.Point(550, 8)
        Me.slaveDataGroupBox.Name = "slaveDataGroupBox"
        Me.slaveDataGroupBox.Size = New System.Drawing.Size(208, 524)
        Me.slaveDataGroupBox.TabIndex = 14
        Me.slaveDataGroupBox.TabStop = False
        Me.slaveDataGroupBox.Text = "Slave Data"
        '
        'rateLabel
        '
        Me.rateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.rateLabel.Location = New System.Drawing.Point(16, 45)
        Me.rateLabel.Name = "rateLabel"
        Me.rateLabel.Size = New System.Drawing.Size(96, 16)
        Me.rateLabel.TabIndex = 2
        Me.rateLabel.Text = "Rate (Hz):"
        '
        'samplesPerChannelNumeric
        '
        Me.samplesPerChannelNumeric.Location = New System.Drawing.Point(152, 17)
        Me.samplesPerChannelNumeric.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.samplesPerChannelNumeric.Name = "samplesPerChannelNumeric"
        Me.samplesPerChannelNumeric.Size = New System.Drawing.Size(168, 20)
        Me.samplesPerChannelNumeric.TabIndex = 1
        Me.samplesPerChannelNumeric.Value = New Decimal(New Integer() {1000, 0, 0, 0})
        '
        'startButton
        '
        Me.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.startButton.Location = New System.Drawing.Point(62, 509)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(75, 23)
        Me.startButton.TabIndex = 15
        Me.startButton.Text = "Start"
        '
        'slaveOutputChannelComboBox
        '
        Me.slaveOutputChannelComboBox.Location = New System.Drawing.Point(152, 39)
        Me.slaveOutputChannelComboBox.Name = "slaveOutputChannelComboBox"
        Me.slaveOutputChannelComboBox.Size = New System.Drawing.Size(168, 21)
        Me.slaveOutputChannelComboBox.TabIndex = 3
        Me.slaveOutputChannelComboBox.Text = "Dev3/ao0"
        '
        'slaveChanGroupBox
        '
        Me.slaveChanGroupBox.Controls.Add(Me.slaveOutputChannelComboBox)
        Me.slaveChanGroupBox.Controls.Add(Me.slaveInputChannelComboBox)
        Me.slaveChanGroupBox.Controls.Add(Me.slaveMinValNumeric)
        Me.slaveChanGroupBox.Controls.Add(Me.slaveInputChanLabel)
        Me.slaveChanGroupBox.Controls.Add(Me.slaveMaxValLabel)
        Me.slaveChanGroupBox.Controls.Add(Me.slaveMinValLabel)
        Me.slaveChanGroupBox.Controls.Add(Me.slaveMaxValNumeric)
        Me.slaveChanGroupBox.Controls.Add(Me.slaveOutputChanLabel)
        Me.slaveChanGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.slaveChanGroupBox.Location = New System.Drawing.Point(6, 198)
        Me.slaveChanGroupBox.Name = "slaveChanGroupBox"
        Me.slaveChanGroupBox.Size = New System.Drawing.Size(336, 120)
        Me.slaveChanGroupBox.TabIndex = 10
        Me.slaveChanGroupBox.TabStop = False
        Me.slaveChanGroupBox.Text = "Channel Parameters - Slave"
        '
        'samplesPerChannelLabel
        '
        Me.samplesPerChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.samplesPerChannelLabel.Location = New System.Drawing.Point(16, 19)
        Me.samplesPerChannelLabel.Name = "samplesPerChannelLabel"
        Me.samplesPerChannelLabel.Size = New System.Drawing.Size(120, 16)
        Me.samplesPerChannelLabel.TabIndex = 0
        Me.samplesPerChannelLabel.Text = "Samples Per Channel:"
        '
        'timingGroupBox
        '
        Me.timingGroupBox.Controls.Add(Me.rateNumeric)
        Me.timingGroupBox.Controls.Add(Me.samplesPerChannelLabel)
        Me.timingGroupBox.Controls.Add(Me.rateLabel)
        Me.timingGroupBox.Controls.Add(Me.samplesPerChannelNumeric)
        Me.timingGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.timingGroupBox.Location = New System.Drawing.Point(6, 323)
        Me.timingGroupBox.Name = "timingGroupBox"
        Me.timingGroupBox.Size = New System.Drawing.Size(336, 73)
        Me.timingGroupBox.TabIndex = 11
        Me.timingGroupBox.TabStop = False
        Me.timingGroupBox.Text = "Timing Parameters"
        '
        'rateNumeric
        '
        Me.rateNumeric.DecimalPlaces = 2
        Me.rateNumeric.Location = New System.Drawing.Point(152, 43)
        Me.rateNumeric.Maximum = New Decimal(New Integer() {102400, 0, 0, 0})
        Me.rateNumeric.Name = "rateNumeric"
        Me.rateNumeric.Size = New System.Drawing.Size(168, 20)
        Me.rateNumeric.TabIndex = 3
        Me.rateNumeric.Value = New Decimal(New Integer() {1000, 0, 0, 0})
        '
        'stopButton
        '
        Me.stopButton.Enabled = False
        Me.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.stopButton.Location = New System.Drawing.Point(182, 509)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(75, 23)
        Me.stopButton.TabIndex = 16
        Me.stopButton.Text = "Stop"
        '
        'waveformGroupBox
        '
        Me.waveformGroupBox.Controls.Add(Me.waveformTypeComboBox)
        Me.waveformGroupBox.Controls.Add(Me.amplitudeNumeric)
        Me.waveformGroupBox.Controls.Add(Me.waveformTypeLabel)
        Me.waveformGroupBox.Controls.Add(Me.amplitudeLabel)
        Me.waveformGroupBox.Controls.Add(Me.samplesPerBufferLabel)
        Me.waveformGroupBox.Controls.Add(Me.samplesPerBufferNumeric)
        Me.waveformGroupBox.Controls.Add(Me.cyclesPerBufferNumeric)
        Me.waveformGroupBox.Controls.Add(Me.cyclesPerBufferLabel)
        Me.waveformGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.waveformGroupBox.Location = New System.Drawing.Point(6, 402)
        Me.waveformGroupBox.Name = "waveformGroupBox"
        Me.waveformGroupBox.Size = New System.Drawing.Size(336, 100)
        Me.waveformGroupBox.TabIndex = 12
        Me.waveformGroupBox.TabStop = False
        Me.waveformGroupBox.Text = "Waveform Parameters"
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(760, 540)
        Me.Controls.Add(Me.syncGroupBox)
        Me.Controls.Add(Me.masterDataGroupBox)
        Me.Controls.Add(Me.masterChanGroupBox)
        Me.Controls.Add(Me.slaveDataGroupBox)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.slaveChanGroupBox)
        Me.Controls.Add(Me.timingGroupBox)
        Me.Controls.Add(Me.stopButton)
        Me.Controls.Add(Me.waveformGroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "MultiDevice Sync - AI and AO Shared Timebase And Trigger - DSA"
        CType(Me.amplitudeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.slaveMinValNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.slaveMaxValNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.slaveDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.masterDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.samplesPerBufferNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cyclesPerBufferNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.masterMinValNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.masterMaxValNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.syncGroupBox.ResumeLayout(False)
        Me.masterDataGroupBox.ResumeLayout(False)
        Me.masterChanGroupBox.ResumeLayout(False)
        Me.slaveDataGroupBox.ResumeLayout(False)
        CType(Me.samplesPerChannelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.slaveChanGroupBox.ResumeLayout(False)
        Me.timingGroupBox.ResumeLayout(False)
        CType(Me.rateNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.waveformGroupBox.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub 'InitializeComponent

    Private Overloads Sub ConfigNumeric(ByVal numeric As NumericUpDown, ByVal minVal As Decimal)
        numeric.Minimum = minVal
        numeric.Maximum = [Decimal].MaxValue
    End Sub 'ConfigNumeric


    Private Overloads Sub ConfigNumeric(ByVal numeric As NumericUpDown)
        ConfigNumeric(numeric, [Decimal].MinValue)
    End Sub 'ConfigNumeric

    Private Sub InitializeDataTables(ByVal rows As Integer)
        ' Clear out the data
        masterDataTable.Rows.Clear()
        slaveDataTable.Rows.Clear()

        masterDataTable.Columns.Clear()
        slaveDataTable.Columns.Clear()

        ' Add one column to each DataGrid of type double
        masterDataColumns = New DataColumn(1) {}
        slaveDataColumns = New DataColumn(1) {}

        masterDataColumns(0) = New DataColumn
        masterDataColumns(0).DataType = GetType(Double)
        masterDataColumns(0).ColumnName = "Master Data"

        slaveDataColumns(0) = New DataColumn
        slaveDataColumns(0).DataType = GetType(Double)
        slaveDataColumns(0).ColumnName = "Slave Data"

        masterDataTable.Columns.AddRange(masterDataColumns)
        slaveDataTable.Columns.AddRange(slaveDataColumns)

        ' Now add a certain number of rows
        Dim i As Integer
        For i = 0 To rows - 1
            Dim rowArr(0) As Object
            masterDataTable.Rows.Add(rowArr)
            slaveDataTable.Rows.Add(rowArr)
        Next i
    End Sub 'InitializeDataTables



    Private Sub startButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles startButton.Click
        ' Change the mouse to an hourglass for the duration of this function.
        System.Windows.Forms.Cursor.Current = Cursors.WaitCursor

        Try
            ' Note: This example uses the SyncTask helper class.
            ' See the SyncTask.cs file in this example for the
            ' implementation of the SyncTask class.
            ' Create the master and slave tasks
            masterIn = New SyncTaskReader("masterIn", Me, syncTypeComboBox.SelectedIndex)
            masterOut = New SyncTaskWriter("masterOut", Me, syncTypeComboBox.SelectedIndex)
            slaveIn = New SyncTaskReader("slaveIn", Me, syncTypeComboBox.SelectedIndex)
            slaveOut = New SyncTaskWriter("slaveOut", Me, syncTypeComboBox.SelectedIndex)

            ' Configure all four tasks with the values selected on the UI.
            masterIn.ConfigureDecimal(masterInputChannelComboBox.Text, masterMinValNumeric.Value, masterMaxValNumeric.Value, samplesPerChannelNumeric.Value, rateNumeric.Value)
            masterOut.ConfigureDecimal(masterOutputChannelComboBox.Text, masterMinValNumeric.Value, masterMaxValNumeric.Value, samplesPerChannelNumeric.Value, rateNumeric.Value)

            slaveIn.ConfigureDecimal(slaveInputChannelComboBox.Text, slaveMinValNumeric.Value, slaveMaxValNumeric.Value, samplesPerChannelNumeric.Value, rateNumeric.Value)
            slaveOut.ConfigureDecimal(slaveOutputChannelComboBox.Text, slaveMinValNumeric.Value, slaveMaxValNumeric.Value, samplesPerChannelNumeric.Value, rateNumeric.Value)

            ' Synchronize the slave tasks to the master tasks.
            ' (See SyncTask.cs for details.)
            slaveIn.SynchronizeSlave(masterIn)
            slaveOut.SynchronizeSlave(masterIn)
            masterOut.SynchronizeSlave(masterIn)

            ' Write data to each output channel
            fGen = New NationalInstruments.Examples.FunctionGenerator(rateNumeric.Value.ToString(), _
                samplesPerBufferNumeric.Value.ToString(), cyclesPerBufferNumeric.Value.ToString(), _
                waveformTypeComboBox.Text, amplitudeNumeric.Value.ToString())

            masterOut.BeginWrite(fGen.Data)
            slaveOut.BeginWrite(fGen.Data)

            ' Officially start the task
            StartTask()

            slaveIn.Start()
            slaveOut.Start()
            masterOut.Start()
            masterIn.Start()

            ' Start reading as well
            masterCallback = New AsyncCallback(AddressOf MasterRead)
            slaveCallback = New AsyncCallback(AddressOf SlaveRead)

            masterIn.BeginRead(masterCallback, masterIn)
            slaveIn.BeginRead(slaveCallback, masterIn)
        Catch ex As System.Exception
            StopTask()
            MessageBox.Show(ex.Message)
        End Try
    End Sub 'startButton_Click


    Private Sub SlaveRead(ByVal ar As IAsyncResult)
        Try
            If (Not (runningTask Is Nothing)) AndAlso runningTask Is ar.AsyncState Then
                ' Read the data
                Dim data As Double() = slaveIn.EndRead(ar)

                ' Display the data
                Dim i As Integer
                i = 0
                While i < slaveDataTable.Rows.Count And i < data.Length
                    slaveDataTable.Rows(i)(0) = data(i)
                    i = i + 1
                End While

                ' Set up next callback
                slaveIn.BeginRead(slaveCallback, masterIn)
            End If
        Catch ex As System.Exception
            StopTask()
            MessageBox.Show(ex.Message)
        End Try
    End Sub 'SlaveRead


    Private Sub MasterRead(ByVal ar As IAsyncResult)
        Try
            If (Not (runningTask Is Nothing)) AndAlso runningTask Is ar.AsyncState Then
                ' Read the data
                Dim data As Double() = masterIn.EndRead(ar)

                ' Display the data
                Dim i As Integer
                i = 0
                While i < masterDataTable.Rows.Count And i < data.Length
                    masterDataTable.Rows(i)(0) = data(i)
                    i = i + 1
                End While

                ' Set up next callback
                masterIn.BeginRead(masterCallback, masterIn)
            End If
        Catch ex As System.Exception
            StopTask()
            MessageBox.Show(ex.Message)
        End Try
    End Sub 'MasterRead

    Private Sub SlaveWrite(ByVal ar As IAsyncResult)
        slaveOut.BeginWrite(fGen.Data)
    End Sub 'SlaveWrite


    Private Sub MasterWrite(ByVal ar As IAsyncResult)
        masterOut.BeginWrite(fGen.Data)
    End Sub 'MasterWrite


    Private Sub stopButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles stopButton.Click
        StopTask()
    End Sub 'stopButton_Click


    Private Sub StartTask()
        If runningTask Is Nothing Then
            ' Change state
            runningTask = masterIn

            ' Fix UI
            masterInputChannelComboBox.Enabled = False
            masterOutputChannelComboBox.Enabled = False
            masterMaxValNumeric.Enabled = False
            masterMinValNumeric.Enabled = False
            masterMinValNumeric.Enabled = False

            slaveInputChannelComboBox.Enabled = False
            slaveOutputChannelComboBox.Enabled = False
            slaveMaxValNumeric.Enabled = False
            slaveMinValNumeric.Enabled = False
            slaveMinValNumeric.Enabled = False

            samplesPerChannelNumeric.Enabled = False
            rateNumeric.Enabled = False

            waveformTypeComboBox.Enabled = False
            amplitudeNumeric.Enabled = False
            samplesPerBufferNumeric.Enabled = False
            cyclesPerBufferNumeric.Enabled = False

            startButton.Enabled = False
            stopButton.Enabled = True
        End If
    End Sub 'StartTask


    Private Sub StopTask()
        ' Change state
        runningTask = Nothing

        ' Fix UI
        masterInputChannelComboBox.Enabled = True
        masterOutputChannelComboBox.Enabled = True
        masterMaxValNumeric.Enabled = True
        masterMinValNumeric.Enabled = True
        masterMinValNumeric.Enabled = True

        slaveInputChannelComboBox.Enabled = True
        slaveOutputChannelComboBox.Enabled = True
        slaveMaxValNumeric.Enabled = True
        slaveMinValNumeric.Enabled = True
        slaveMinValNumeric.Enabled = True

        samplesPerChannelNumeric.Enabled = True
        rateNumeric.Enabled = True

        waveformTypeComboBox.Enabled = True
        amplitudeNumeric.Enabled = True
        samplesPerBufferNumeric.Enabled = True
        cyclesPerBufferNumeric.Enabled = True

        startButton.Enabled = True
        stopButton.Enabled = False

        ' Stop tasks
        slaveIn.StopTask()
        slaveOut.StopTask()
        masterIn.StopTask()
        masterOut.StopTask()

        slaveIn.Dispose()
        slaveOut.Dispose()
        masterIn.Dispose()
        masterOut.Dispose()
    End Sub 'StopTask

    Public Shared Function GetDeviceName(ByVal deviceName As String) As String
        Dim device As Device = DaqSystem.Local.LoadDevice(deviceName)
        If (device.BusType <> DeviceBusType.CompactDaq) Then
            Return deviceName
        Else
            Return device.CompactDaqChassisDeviceName
        End If
    End Function 'GetDeviceName

End Class 'MainForm
