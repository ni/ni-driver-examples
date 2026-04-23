'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   AIContAcquisition
'
' Category:
'   Synchronization
'
' Description:
'   This example demonstrates how to acquire a continuous amount of data using
'   the DAQ device's 
'   internal clock.  It also shows how to synchronize two devices for different
'   device families (E Series, 
'   M Series, and DSA), to simultaneously acquire the data.NOTE: This example is
'   intended to show low level 
'   synchronization of various devices. DSA and S Series devices now support
'   including channels from multiple 
'   devices in a single task. DAQmx automatically synchronizes the devices in such
'   a task. See the DAQmx 
'   Help>>NI-DAQmx Device Considerations>>Multidevice Tasks section
'   for further details.NOTE: 
'   PXI 6115 and 6120 (S Series) devices don't require sharing of master timebase,
'   because they auto-lock 
'   to Clock 10.  For those devices sharing a start trigger is adequate.NOTE: For
'   the PCI-6154 S Series device 
'   use the M Series (PCI) synchronization type to synchronize using the reference
'   clock.
'
' Instructions for running:
'   1.  Select the synchronization type according to the devices you are using
'       for acquisition.
'   2.  Select the physical channels which correspond to where your signals are
'       input on the DAQ device.
'   3.  Enter the minimum and maximum voltage ranges for the physical channels.
'   4.  Set the number of samples to acquire per channel.
'   5.  Set the rate of the acquisition, in Hertz.
'
' Steps:
'   1.  Create an analog input voltage channel for both the master and slave
'       devices.
'   2.  Set timing parameters for the acquisition.
'   3.  Call master.SynchronizeMaster() to configure the master device for
'       synchronization, depending on the synchronization type.
'   4.  Call slave.SynchronizeSlave(master) to configure the slave device for
'       synchronization, depending on the synchronization type.
'   5.  Start the tasks. The slave task must be started first so that it can
'       wait on the master's start trigger.
'   6.  The following three steps are done for both master and slave.
'   7.  Call AnalogMultiChannelReader.BeginReadMultiSample to install a callback
'       and begin the asynchronous read operation.
'   8.  Inside the callback, call AnalogMultiChannelReader.EndReadMultiSample to
'       retrieve the data from the read operation.  
'   9.  Call AnalogMultiChannelReader.BeginReadMultiSample again inside the
'       callback to perform another read operation.
'   10. Dispose the Task object to clean-up any resources associated with the
'       task.
'   11. Handle any DaqExceptions, if they occur.
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
'   Make sure your signal input terminal matches the Physical Channel I/O
'   control. If you have a PXI chassis, ensure it has been properly identified
'   in MAX.  If you have devices with a RTSI bus, ensure they are connected with
'   a RTSI cable and that the RTSI cable is registered in MAX.
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
        Application.DoEvents()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        masterPhysicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AI, PhysicalChannelAccess.External))
        If masterPhysicalChannelComboBox.Items.Count > 0 Then
            masterPhysicalChannelComboBox.SelectedIndex = 0
        End If

        slavePhysicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AI, PhysicalChannelAccess.External))
        If slavePhysicalChannelComboBox.Items.Count > 0 Then
            slavePhysicalChannelComboBox.SelectedIndex = 0
        End If

        If masterPhysicalChannelComboBox.Items.Count > 0 And slavePhysicalChannelComboBox.Items.Count > 0 Then
            startButton.Enabled = True
        End If

        synchronizationTypeComboBox.SelectedIndex = 0

        ConfigNumeric(masterMaximumValueNumeric)
        ConfigNumeric(masterMinimumValueNumeric)
        ConfigNumeric(slaveMaximumValueNumeric)
        ConfigNumeric(slaveMinimumValueNumeric)
        ConfigNumeric(samplesPerChannelNumeric, Decimal.Zero)
        ConfigNumeric(rateNumeric, Decimal.Zero)

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
            If Not (master Is Nothing) Then
                runningTask = Nothing
                master.Dispose()
            End If
            If Not (slave Is Nothing) Then
                slave.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer
    Private runningTask As SyncTask
    Private master As SyncTask = Nothing
    Private slave As SyncTask = Nothing
    Private masterCallback As AsyncCallback
    Private slaveCallback As AsyncCallback

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents syncGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents masterGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents masterDataGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents slaveGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents timingGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents slaveDataGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents masterPhysicalChannelComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents slavePhysicalChannelComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents samplesPerChannelLabel As System.Windows.Forms.Label
    Friend WithEvents rateLabel As System.Windows.Forms.Label
    Friend WithEvents samplesPerChannelNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents rateNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents masterDataGrid As System.Windows.Forms.DataGrid
    Friend WithEvents slaveDataGrid As System.Windows.Forms.DataGrid
    Friend WithEvents startButton As System.Windows.Forms.Button
    Friend WithEvents stopButton As System.Windows.Forms.Button
    Friend WithEvents synchronizationTypeComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents synchronizationTypeLabel As System.Windows.Forms.Label
    Friend WithEvents masterMinimumValueLabel As System.Windows.Forms.Label
    Friend WithEvents masterMaximumValueLabel As System.Windows.Forms.Label
    Friend WithEvents masterPhysicalChannelLabel As System.Windows.Forms.Label
    Friend WithEvents slavePhysicalChannelLabel As System.Windows.Forms.Label
    Friend WithEvents slaveMaximumValueLabel As System.Windows.Forms.Label
    Friend WithEvents slaveMinimujValueLabel As System.Windows.Forms.Label
    Friend WithEvents masterMaximumValueNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents masterMinimumValueNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents slaveMaximumValueNumeric As System.Windows.Forms.NumericUpDown
    Friend WithEvents slaveMinimumValueNumeric As System.Windows.Forms.NumericUpDown
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MainForm))
        Me.syncGroupBox = New System.Windows.Forms.GroupBox
        Me.synchronizationTypeComboBox = New System.Windows.Forms.ComboBox
        Me.synchronizationTypeLabel = New System.Windows.Forms.Label
        Me.masterGroupBox = New System.Windows.Forms.GroupBox
        Me.masterMinimumValueNumeric = New System.Windows.Forms.NumericUpDown
        Me.masterMaximumValueNumeric = New System.Windows.Forms.NumericUpDown
        Me.masterPhysicalChannelComboBox = New System.Windows.Forms.ComboBox
        Me.masterMinimumValueLabel = New System.Windows.Forms.Label
        Me.masterMaximumValueLabel = New System.Windows.Forms.Label
        Me.masterPhysicalChannelLabel = New System.Windows.Forms.Label
        Me.masterDataGroupBox = New System.Windows.Forms.GroupBox
        Me.masterDataGrid = New System.Windows.Forms.DataGrid
        Me.slaveDataGroupBox = New System.Windows.Forms.GroupBox
        Me.slaveDataGrid = New System.Windows.Forms.DataGrid
        Me.slaveGroupBox = New System.Windows.Forms.GroupBox
        Me.slaveMinimumValueNumeric = New System.Windows.Forms.NumericUpDown
        Me.slaveMaximumValueNumeric = New System.Windows.Forms.NumericUpDown
        Me.slavePhysicalChannelComboBox = New System.Windows.Forms.ComboBox
        Me.slaveMinimujValueLabel = New System.Windows.Forms.Label
        Me.slaveMaximumValueLabel = New System.Windows.Forms.Label
        Me.slavePhysicalChannelLabel = New System.Windows.Forms.Label
        Me.timingGroupBox = New System.Windows.Forms.GroupBox
        Me.rateNumeric = New System.Windows.Forms.NumericUpDown
        Me.samplesPerChannelNumeric = New System.Windows.Forms.NumericUpDown
        Me.rateLabel = New System.Windows.Forms.Label
        Me.samplesPerChannelLabel = New System.Windows.Forms.Label
        Me.startButton = New System.Windows.Forms.Button
        Me.stopButton = New System.Windows.Forms.Button
        Me.syncGroupBox.SuspendLayout()
        Me.masterGroupBox.SuspendLayout()
        CType(Me.masterMinimumValueNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.masterMaximumValueNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.masterDataGroupBox.SuspendLayout()
        CType(Me.masterDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.slaveDataGroupBox.SuspendLayout()
        CType(Me.slaveDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.slaveGroupBox.SuspendLayout()
        CType(Me.slaveMinimumValueNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.slaveMaximumValueNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.timingGroupBox.SuspendLayout()
        CType(Me.rateNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.samplesPerChannelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'syncGroupBox
        '
        Me.syncGroupBox.Controls.Add(Me.synchronizationTypeComboBox)
        Me.syncGroupBox.Controls.Add(Me.synchronizationTypeLabel)
        Me.syncGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.syncGroupBox.Location = New System.Drawing.Point(8, 8)
        Me.syncGroupBox.Name = "syncGroupBox"
        Me.syncGroupBox.Size = New System.Drawing.Size(336, 56)
        Me.syncGroupBox.TabIndex = 0
        Me.syncGroupBox.TabStop = False
        Me.syncGroupBox.Text = "Synchronization Parameters"
        '
        'synchronizationTypeComboBox
        '
        Me.synchronizationTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.synchronizationTypeComboBox.Items.AddRange(New Object() {"E-Series", "M-Series (PCI)", "M-Series (PXI)", "DSA Sample Clock Timebase", "DSA Reference Clock"})
        Me.synchronizationTypeComboBox.Location = New System.Drawing.Point(152, 22)
        Me.synchronizationTypeComboBox.Name = "synchronizationTypeComboBox"
        Me.synchronizationTypeComboBox.Size = New System.Drawing.Size(168, 21)
        Me.synchronizationTypeComboBox.TabIndex = 1
        '
        'synchronizationTypeLabel
        '
        Me.synchronizationTypeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.synchronizationTypeLabel.Location = New System.Drawing.Point(16, 24)
        Me.synchronizationTypeLabel.Name = "synchronizationTypeLabel"
        Me.synchronizationTypeLabel.Size = New System.Drawing.Size(120, 16)
        Me.synchronizationTypeLabel.TabIndex = 0
        Me.synchronizationTypeLabel.Text = "Synchronization Type:"
        '
        'masterGroupBox
        '
        Me.masterGroupBox.Controls.Add(Me.masterMinimumValueNumeric)
        Me.masterGroupBox.Controls.Add(Me.masterMaximumValueNumeric)
        Me.masterGroupBox.Controls.Add(Me.masterPhysicalChannelComboBox)
        Me.masterGroupBox.Controls.Add(Me.masterMinimumValueLabel)
        Me.masterGroupBox.Controls.Add(Me.masterMaximumValueLabel)
        Me.masterGroupBox.Controls.Add(Me.masterPhysicalChannelLabel)
        Me.masterGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.masterGroupBox.Location = New System.Drawing.Point(8, 72)
        Me.masterGroupBox.Name = "masterGroupBox"
        Me.masterGroupBox.Size = New System.Drawing.Size(336, 128)
        Me.masterGroupBox.TabIndex = 1
        Me.masterGroupBox.TabStop = False
        Me.masterGroupBox.Text = "Channel Parameters - Master"
        '
        'masterMinimumValueNumeric
        '
        Me.masterMinimumValueNumeric.DecimalPlaces = 2
        Me.masterMinimumValueNumeric.Location = New System.Drawing.Point(152, 88)
        Me.masterMinimumValueNumeric.Minimum = New Decimal(New Integer() {100, 0, 0, -2147483648})
        Me.masterMinimumValueNumeric.Name = "masterMinimumValueNumeric"
        Me.masterMinimumValueNumeric.Size = New System.Drawing.Size(168, 20)
        Me.masterMinimumValueNumeric.TabIndex = 5
        Me.masterMinimumValueNumeric.Value = New Decimal(New Integer() {10, 0, 0, -2147483648})
        '
        'masterMaximumValueNumeric
        '
        Me.masterMaximumValueNumeric.DecimalPlaces = 2
        Me.masterMaximumValueNumeric.Location = New System.Drawing.Point(152, 56)
        Me.masterMaximumValueNumeric.Name = "masterMaximumValueNumeric"
        Me.masterMaximumValueNumeric.Size = New System.Drawing.Size(168, 20)
        Me.masterMaximumValueNumeric.TabIndex = 3
        Me.masterMaximumValueNumeric.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'masterPhysicalChannelComboBox
        '
        Me.masterPhysicalChannelComboBox.Location = New System.Drawing.Point(152, 24)
        Me.masterPhysicalChannelComboBox.Name = "masterPhysicalChannelComboBox"
        Me.masterPhysicalChannelComboBox.Size = New System.Drawing.Size(168, 21)
        Me.masterPhysicalChannelComboBox.TabIndex = 1
        Me.masterPhysicalChannelComboBox.Text = "Dev1/ai0"
        '
        'masterMinimumValueLabel
        '
        Me.masterMinimumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.masterMinimumValueLabel.Location = New System.Drawing.Point(16, 88)
        Me.masterMinimumValueLabel.Name = "masterMinimumValueLabel"
        Me.masterMinimumValueLabel.Size = New System.Drawing.Size(96, 16)
        Me.masterMinimumValueLabel.TabIndex = 4
        Me.masterMinimumValueLabel.Text = "Minimum Value:"
        '
        'masterMaximumValueLabel
        '
        Me.masterMaximumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.masterMaximumValueLabel.Location = New System.Drawing.Point(16, 56)
        Me.masterMaximumValueLabel.Name = "masterMaximumValueLabel"
        Me.masterMaximumValueLabel.Size = New System.Drawing.Size(96, 16)
        Me.masterMaximumValueLabel.TabIndex = 2
        Me.masterMaximumValueLabel.Text = "Maximum Value:"
        '
        'masterPhysicalChannelLabel
        '
        Me.masterPhysicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.masterPhysicalChannelLabel.Location = New System.Drawing.Point(16, 24)
        Me.masterPhysicalChannelLabel.Name = "masterPhysicalChannelLabel"
        Me.masterPhysicalChannelLabel.Size = New System.Drawing.Size(96, 16)
        Me.masterPhysicalChannelLabel.TabIndex = 0
        Me.masterPhysicalChannelLabel.Text = "Physical Channel:"
        '
        'masterDataGroupBox
        '
        Me.masterDataGroupBox.Controls.Add(Me.masterDataGrid)
        Me.masterDataGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.masterDataGroupBox.Location = New System.Drawing.Point(360, 8)
        Me.masterDataGroupBox.Name = "masterDataGroupBox"
        Me.masterDataGroupBox.Size = New System.Drawing.Size(192, 486)
        Me.masterDataGroupBox.TabIndex = 6
        Me.masterDataGroupBox.TabStop = False
        Me.masterDataGroupBox.Text = "Master Data"
        '
        'masterDataGrid
        '
        Me.masterDataGrid.DataMember = ""
        Me.masterDataGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.masterDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.masterDataGrid.Location = New System.Drawing.Point(3, 16)
        Me.masterDataGrid.Name = "masterDataGrid"
        Me.masterDataGrid.PreferredColumnWidth = 100
        Me.masterDataGrid.Size = New System.Drawing.Size(186, 467)
        Me.masterDataGrid.TabIndex = 0
        Me.masterDataGrid.TabStop = False
        '
        'slaveDataGroupBox
        '
        Me.slaveDataGroupBox.Controls.Add(Me.slaveDataGrid)
        Me.slaveDataGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.slaveDataGroupBox.Location = New System.Drawing.Point(560, 8)
        Me.slaveDataGroupBox.Name = "slaveDataGroupBox"
        Me.slaveDataGroupBox.Size = New System.Drawing.Size(192, 486)
        Me.slaveDataGroupBox.TabIndex = 7
        Me.slaveDataGroupBox.TabStop = False
        Me.slaveDataGroupBox.Text = "Slave Data"
        '
        'slaveDataGrid
        '
        Me.slaveDataGrid.DataMember = ""
        Me.slaveDataGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.slaveDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.slaveDataGrid.Location = New System.Drawing.Point(3, 16)
        Me.slaveDataGrid.Name = "slaveDataGrid"
        Me.slaveDataGrid.PreferredColumnWidth = 100
        Me.slaveDataGrid.Size = New System.Drawing.Size(186, 467)
        Me.slaveDataGrid.TabIndex = 0
        Me.slaveDataGrid.TabStop = False
        '
        'slaveGroupBox
        '
        Me.slaveGroupBox.Controls.Add(Me.slaveMinimumValueNumeric)
        Me.slaveGroupBox.Controls.Add(Me.slaveMaximumValueNumeric)
        Me.slaveGroupBox.Controls.Add(Me.slavePhysicalChannelComboBox)
        Me.slaveGroupBox.Controls.Add(Me.slaveMinimujValueLabel)
        Me.slaveGroupBox.Controls.Add(Me.slaveMaximumValueLabel)
        Me.slaveGroupBox.Controls.Add(Me.slavePhysicalChannelLabel)
        Me.slaveGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.slaveGroupBox.Location = New System.Drawing.Point(8, 208)
        Me.slaveGroupBox.Name = "slaveGroupBox"
        Me.slaveGroupBox.Size = New System.Drawing.Size(336, 128)
        Me.slaveGroupBox.TabIndex = 2
        Me.slaveGroupBox.TabStop = False
        Me.slaveGroupBox.Text = "Channel Parameters - Slave"
        '
        'slaveMinimumValueNumeric
        '
        Me.slaveMinimumValueNumeric.DecimalPlaces = 2
        Me.slaveMinimumValueNumeric.Location = New System.Drawing.Point(152, 88)
        Me.slaveMinimumValueNumeric.Minimum = New Decimal(New Integer() {100, 0, 0, -2147483648})
        Me.slaveMinimumValueNumeric.Name = "slaveMinimumValueNumeric"
        Me.slaveMinimumValueNumeric.Size = New System.Drawing.Size(168, 20)
        Me.slaveMinimumValueNumeric.TabIndex = 5
        Me.slaveMinimumValueNumeric.Value = New Decimal(New Integer() {10, 0, 0, -2147483648})
        '
        'slaveMaximumValueNumeric
        '
        Me.slaveMaximumValueNumeric.DecimalPlaces = 2
        Me.slaveMaximumValueNumeric.Location = New System.Drawing.Point(152, 56)
        Me.slaveMaximumValueNumeric.Name = "slaveMaximumValueNumeric"
        Me.slaveMaximumValueNumeric.Size = New System.Drawing.Size(168, 20)
        Me.slaveMaximumValueNumeric.TabIndex = 3
        Me.slaveMaximumValueNumeric.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'slavePhysicalChannelComboBox
        '
        Me.slavePhysicalChannelComboBox.Location = New System.Drawing.Point(152, 24)
        Me.slavePhysicalChannelComboBox.Name = "slavePhysicalChannelComboBox"
        Me.slavePhysicalChannelComboBox.Size = New System.Drawing.Size(168, 21)
        Me.slavePhysicalChannelComboBox.TabIndex = 1
        Me.slavePhysicalChannelComboBox.Text = "Dev2/ai0"
        '
        'slaveMinimujValueLabel
        '
        Me.slaveMinimujValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.slaveMinimujValueLabel.Location = New System.Drawing.Point(16, 88)
        Me.slaveMinimujValueLabel.Name = "slaveMinimujValueLabel"
        Me.slaveMinimujValueLabel.TabIndex = 4
        Me.slaveMinimujValueLabel.Text = "Minimum Value:"
        '
        'slaveMaximumValueLabel
        '
        Me.slaveMaximumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.slaveMaximumValueLabel.Location = New System.Drawing.Point(16, 56)
        Me.slaveMaximumValueLabel.Name = "slaveMaximumValueLabel"
        Me.slaveMaximumValueLabel.Size = New System.Drawing.Size(96, 16)
        Me.slaveMaximumValueLabel.TabIndex = 2
        Me.slaveMaximumValueLabel.Text = "Maximum Value:"
        '
        'slavePhysicalChannelLabel
        '
        Me.slavePhysicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.slavePhysicalChannelLabel.Location = New System.Drawing.Point(16, 24)
        Me.slavePhysicalChannelLabel.Name = "slavePhysicalChannelLabel"
        Me.slavePhysicalChannelLabel.Size = New System.Drawing.Size(96, 16)
        Me.slavePhysicalChannelLabel.TabIndex = 0
        Me.slavePhysicalChannelLabel.Text = "Physical Channel:"
        '
        'timingGroupBox
        '
        Me.timingGroupBox.Controls.Add(Me.rateNumeric)
        Me.timingGroupBox.Controls.Add(Me.samplesPerChannelNumeric)
        Me.timingGroupBox.Controls.Add(Me.rateLabel)
        Me.timingGroupBox.Controls.Add(Me.samplesPerChannelLabel)
        Me.timingGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.timingGroupBox.Location = New System.Drawing.Point(8, 344)
        Me.timingGroupBox.Name = "timingGroupBox"
        Me.timingGroupBox.Size = New System.Drawing.Size(336, 96)
        Me.timingGroupBox.TabIndex = 3
        Me.timingGroupBox.TabStop = False
        Me.timingGroupBox.Text = "Timing Parameters"
        '
        'rateNumeric
        '
        Me.rateNumeric.DecimalPlaces = 2
        Me.rateNumeric.Location = New System.Drawing.Point(152, 56)
        Me.rateNumeric.Maximum = New Decimal(New Integer() {102400, 0, 0, 0})
        Me.rateNumeric.Name = "rateNumeric"
        Me.rateNumeric.Size = New System.Drawing.Size(168, 20)
        Me.rateNumeric.TabIndex = 3
        Me.rateNumeric.Value = New Decimal(New Integer() {1000, 0, 0, 0})
        '
        'samplesPerChannelNumeric
        '
        Me.samplesPerChannelNumeric.Location = New System.Drawing.Point(152, 24)
        Me.samplesPerChannelNumeric.Name = "samplesPerChannelNumeric"
        Me.samplesPerChannelNumeric.Size = New System.Drawing.Size(168, 20)
        Me.samplesPerChannelNumeric.TabIndex = 1
        Me.samplesPerChannelNumeric.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        'rateLabel
        '
        Me.rateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.rateLabel.Location = New System.Drawing.Point(16, 56)
        Me.rateLabel.Name = "rateLabel"
        Me.rateLabel.Size = New System.Drawing.Size(96, 16)
        Me.rateLabel.TabIndex = 2
        Me.rateLabel.Text = "Rate (Hz):"
        '
        'samplesPerChannelLabel
        '
        Me.samplesPerChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.samplesPerChannelLabel.Location = New System.Drawing.Point(16, 24)
        Me.samplesPerChannelLabel.Name = "samplesPerChannelLabel"
        Me.samplesPerChannelLabel.Size = New System.Drawing.Size(120, 16)
        Me.samplesPerChannelLabel.TabIndex = 0
        Me.samplesPerChannelLabel.Text = "Samples Per Channel:"
        '
        'startButton
        '
        Me.startButton.Enabled = False
        Me.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.startButton.Location = New System.Drawing.Point(72, 471)
        Me.startButton.Name = "startButton"
        Me.startButton.TabIndex = 4
        Me.startButton.Text = "Start"
        '
        'stopButton
        '
        Me.stopButton.Enabled = False
        Me.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.stopButton.Location = New System.Drawing.Point(200, 471)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.TabIndex = 5
        Me.stopButton.Text = "Stop"
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(762, 504)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.timingGroupBox)
        Me.Controls.Add(Me.slaveGroupBox)
        Me.Controls.Add(Me.slaveDataGroupBox)
        Me.Controls.Add(Me.masterDataGroupBox)
        Me.Controls.Add(Me.masterGroupBox)
        Me.Controls.Add(Me.syncGroupBox)
        Me.Controls.Add(Me.stopButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Multi-Device Sync - Analog Input - Continuous Acquisition"
        Me.syncGroupBox.ResumeLayout(False)
        Me.masterGroupBox.ResumeLayout(False)
        CType(Me.masterMinimumValueNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.masterMaximumValueNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.masterDataGroupBox.ResumeLayout(False)
        CType(Me.masterDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.slaveDataGroupBox.ResumeLayout(False)
        CType(Me.slaveDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.slaveGroupBox.ResumeLayout(False)
        CType(Me.slaveMinimumValueNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.slaveMaximumValueNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.timingGroupBox.ResumeLayout(False)
        CType(Me.rateNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.samplesPerChannelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub ConfigNumeric(ByRef numeric As NumericUpDown, ByVal minVal As Decimal)
        numeric.Minimum = minVal
        numeric.Maximum = Decimal.MaxValue
    End Sub

    Private Sub ConfigNumeric(ByRef numeric As NumericUpDown)
        ConfigNumeric(numeric, Decimal.MinValue)
    End Sub


    Private Sub startButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles startButton.Click
        ' Change the mouse to an hourglass for the duration of this function
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Try
            ' Note: This example uses the SyncTask helper class.
            ' See the SyncTask.cs file in this example for the
            ' implementation of the SyncTask class.

            ' Create the master and slave tasks
            master = New SyncTask("master", Me, synchronizationTypeComboBox.SelectedIndex)
            slave = New SyncTask("slave", Me, synchronizationTypeComboBox.SelectedIndex)

            ' Configure both tasks with the values selected on the UI.
            master.ConfigureDecimal(masterPhysicalChannelComboBox.Text, masterMinimumValueNumeric.Value, masterMaximumValueNumeric.Value, samplesPerChannelNumeric.Value, rateNumeric.Value)
            slave.ConfigureDecimal(slavePhysicalChannelComboBox.Text, slaveMinimumValueNumeric.Value, slaveMaximumValueNumeric.Value, samplesPerChannelNumeric.Value, rateNumeric.Value)

            ' Hook up the data grids to the data tables contained in
            ' the SyncTask classes.
            masterDataGrid.DataSource = master.DataTable
            slaveDataGrid.DataSource = slave.DataTable

            ' Synchronize the slave task to the master task.
            ' (See SyncTask.cs for details.)
            master.SynchronizeMaster()
            slave.SynchronizeSlave(master)

            StartTasks()

            ' Start Reading as well
            masterCallback = New AsyncCallback(AddressOf MasterRead)
            slaveCallback = New AsyncCallback(AddressOf SlaveRead)

            master.BeginRead(masterCallback, master)
            slave.BeginRead(slaveCallback, master)
        Catch ex As DaqException
            ' Popup a dialog if an exception is thrown.
            MessageBox.Show(ex.Message)

            ' Stop the tasks if an exception is thrown.
            StopTasks()
        End Try

        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
    End Sub 'readButton_Click

    Private Sub SlaveRead(ByVal ar As IAsyncResult)
        Try
            If (Not (runningTask Is Nothing)) AndAlso runningTask Is ar.AsyncState Then
                ' Read the data
                Dim data As Double(,) = slave.EndRead(ar)

                ' Display the data
                DisplayData(data, slave.DataTable)

                ' Set up next callback
                slave.BeginRead(slaveCallback, master)
            End If
        Catch ex As DaqException
            ' Popup a dialog if an exception is thrown.
            MessageBox.Show(ex.Message)
            ' Stop the tasks if an exception is thrown.
            StopTasks()
        End Try
    End Sub 'SlaveRead

    Private Sub MasterRead(ByVal ar As IAsyncResult)
        Try
            If (Not (runningTask Is Nothing)) AndAlso runningTask Is ar.AsyncState Then
                ' Read the data
                Dim data As Double(,) = master.EndRead(ar)

                ' Display the data 
                DisplayData(data, master.DataTable)

                ' Set up the next callback
                master.BeginRead(masterCallback, master)
            End If
        Catch ex As DaqException
            ' Popup a dialog if an exception is thrown.
            MessageBox.Show(ex.Message)
            ' Stop the tasks if an exception is thrown.
            StopTasks()
        End Try
    End Sub 'MasterRead

    Private Sub stopButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles stopButton.Click
        StopTasks()
    End Sub 'stopButton_Click

    Private Sub DisplayData(ByVal data As Double(,), ByVal dataTable As DataTable)
        Dim currentChannelCount As Integer
        Dim currentDataCount As Integer

        For currentChannelCount = 0 To dataTable.Columns.Count - 1
            For currentDataCount = 0 To dataTable.Rows.Count - 1
                dataTable.Rows(currentDataCount)(currentChannelCount) = data(currentChannelCount, currentDataCount)
            Next currentDataCount
        Next currentChannelCount
    End Sub 'DisplayData

    Private Sub StartTasks()

        If runningTask Is Nothing Then
            ' Change the State
            runningTask = master

            startButton.Enabled = False
            stopButton.Enabled = True
        End If

        ' Start both tasks.
        ' Note: Start the slave task first because it is waiting on
        ' the master task.
        slave.Start()
        master.Start()
    End Sub 'StartTask

    Private Sub StopTasks()
        ' Change the State
        runningTask = Nothing

        startButton.Enabled = True
        stopButton.Enabled = False

        ' Dispose Tasks
        slave.Dispose()
        master.Dispose()
    End Sub 'StopTasks
End Class 'MainForm
