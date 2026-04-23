'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Example program:
'   AIFiniteAcquisition
'
' Category:
'   Synchronization
'
' Description:
'   This example demonstrates how to acquire a finite amount of analog input
'   data using two 
'   DAQ devices' internal clocks.  It also synchronizes these devices depending on
'   the device family (E Series, 
'   M Series, or DSA) to simultaneously acquire the data.NOTE: This example is
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
'   5.  Start the tasks.  The slave task must be started first so that it can
'       wait on the master's start trigger.
'   6.  Read all of the analog input data from both devices.
'   7.  Dispose the Task object to clean-up any resources associated with the
'       task.
'   8.  Handle any DaqExceptions, if they occur.
'
' I/O Connections Overview:
'   Make sure your signal input terminals match the Physical Channel I/O
'   Controls.  If you have a PXI chassis, ensure that it has been properly
'   identified in MAX.  If you have devices with a RTSI bus, ensure that they
'   are connected with a RTSI cable and that the RTSI cable is registered in
'   MAX.
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
        masterPhysicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AI, PhysicalChannelAccess.External))
        If masterPhysicalChannelComboBox.Items.Count > 0 Then
            masterPhysicalChannelComboBox.SelectedIndex = 0
        End If        
        
        slavePhysicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AI, PhysicalChannelAccess.External))
        If slavePhysicalChannelComboBox.Items.Count > 0 Then
            slavePhysicalChannelComboBox.SelectedIndex = 0
        End If
            
        syncTypeComboBox.SelectedIndex = 0

        ConfigNumeric(masterMaxValNumeric)
        ConfigNumeric(masterMinValNumeric)
        ConfigNumeric(slaveMaxValNumeric)
        ConfigNumeric(slaveMinValNumeric)
        ConfigNumeric(samplesPerChannelNumeric, Decimal.Zero)
        ConfigNumeric(rateNumeric, Decimal.Zero)
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
    Private masterGroupBox As System.Windows.Forms.GroupBox
    Private masterPhysChanLabel As System.Windows.Forms.Label
    Private masterMaxValLabel As System.Windows.Forms.Label
    Private masterMinValLabel As System.Windows.Forms.Label
    Private masterMinValNumeric As System.Windows.Forms.NumericUpDown
    Private masterMaxValNumeric As System.Windows.Forms.NumericUpDown
    Private slaveGroupBox As System.Windows.Forms.GroupBox
    Private slavePhysChanLabel As System.Windows.Forms.Label
    Private slaveMaxValLabel As System.Windows.Forms.Label
    Private slaveMinValLabel As System.Windows.Forms.Label
    Private slaveMinValNumeric As System.Windows.Forms.NumericUpDown
    Private slaveMaxValNumeric As System.Windows.Forms.NumericUpDown
    Private timingGroupBox As System.Windows.Forms.GroupBox
    Private rateNumeric As System.Windows.Forms.NumericUpDown
    Private samplesPerChannelLabel As System.Windows.Forms.Label
    Private rateLabel As System.Windows.Forms.Label
    Private samplesPerChannelNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents readButton As System.Windows.Forms.Button
    Private dataGridPanel As System.Windows.Forms.Panel
    Private rightDataGridPanel As System.Windows.Forms.Panel
    Private dataGridSplitter As System.Windows.Forms.Splitter
    Private masterDataGroupBox As System.Windows.Forms.GroupBox
    Private slaveDataGrid As System.Windows.Forms.DataGrid
    Private slaveDataGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents masterDataGrid As System.Windows.Forms.DataGrid
    Friend WithEvents masterPhysicalChannelComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents slavePhysicalChannelComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents syncGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents syncTypeComboBox As System.Windows.Forms.ComboBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MainForm))
        Me.masterGroupBox = New System.Windows.Forms.GroupBox
        Me.masterPhysicalChannelComboBox = New System.Windows.Forms.ComboBox
        Me.masterMinValNumeric = New System.Windows.Forms.NumericUpDown
        Me.masterPhysChanLabel = New System.Windows.Forms.Label
        Me.masterMaxValLabel = New System.Windows.Forms.Label
        Me.masterMinValLabel = New System.Windows.Forms.Label
        Me.masterMaxValNumeric = New System.Windows.Forms.NumericUpDown
        Me.slaveGroupBox = New System.Windows.Forms.GroupBox
        Me.slavePhysicalChannelComboBox = New System.Windows.Forms.ComboBox
        Me.slaveMinValNumeric = New System.Windows.Forms.NumericUpDown
        Me.slavePhysChanLabel = New System.Windows.Forms.Label
        Me.slaveMaxValLabel = New System.Windows.Forms.Label
        Me.slaveMinValLabel = New System.Windows.Forms.Label
        Me.slaveMaxValNumeric = New System.Windows.Forms.NumericUpDown
        Me.timingGroupBox = New System.Windows.Forms.GroupBox
        Me.rateNumeric = New System.Windows.Forms.NumericUpDown
        Me.samplesPerChannelLabel = New System.Windows.Forms.Label
        Me.rateLabel = New System.Windows.Forms.Label
        Me.samplesPerChannelNumeric = New System.Windows.Forms.NumericUpDown
        Me.readButton = New System.Windows.Forms.Button
        Me.dataGridPanel = New System.Windows.Forms.Panel
        Me.masterDataGroupBox = New System.Windows.Forms.GroupBox
        Me.masterDataGrid = New System.Windows.Forms.DataGrid
        Me.dataGridSplitter = New System.Windows.Forms.Splitter
        Me.rightDataGridPanel = New System.Windows.Forms.Panel
        Me.slaveDataGroupBox = New System.Windows.Forms.GroupBox
        Me.slaveDataGrid = New System.Windows.Forms.DataGrid
        Me.syncGroupBox = New System.Windows.Forms.GroupBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.syncTypeComboBox = New System.Windows.Forms.ComboBox
        Me.masterGroupBox.SuspendLayout()
        CType(Me.masterMinValNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.masterMaxValNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.slaveGroupBox.SuspendLayout()
        CType(Me.slaveMinValNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.slaveMaxValNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.timingGroupBox.SuspendLayout()
        CType(Me.rateNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.samplesPerChannelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.dataGridPanel.SuspendLayout()
        Me.masterDataGroupBox.SuspendLayout()
        CType(Me.masterDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.rightDataGridPanel.SuspendLayout()
        Me.slaveDataGroupBox.SuspendLayout()
        CType(Me.slaveDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.syncGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'masterGroupBox
        '
        Me.masterGroupBox.Controls.Add(Me.masterPhysicalChannelComboBox)
        Me.masterGroupBox.Controls.Add(Me.masterMinValNumeric)
        Me.masterGroupBox.Controls.Add(Me.masterPhysChanLabel)
        Me.masterGroupBox.Controls.Add(Me.masterMaxValLabel)
        Me.masterGroupBox.Controls.Add(Me.masterMinValLabel)
        Me.masterGroupBox.Controls.Add(Me.masterMaxValNumeric)
        Me.masterGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.masterGroupBox.Location = New System.Drawing.Point(8, 72)
        Me.masterGroupBox.Name = "masterGroupBox"
        Me.masterGroupBox.Size = New System.Drawing.Size(336, 128)
        Me.masterGroupBox.TabIndex = 1
        Me.masterGroupBox.TabStop = False
        Me.masterGroupBox.Text = "Channel Parameters - Master"
        '
        'masterPhysicalChannelComboBox
        '
        Me.masterPhysicalChannelComboBox.Location = New System.Drawing.Point(152, 24)
        Me.masterPhysicalChannelComboBox.Name = "masterPhysicalChannelComboBox"
        Me.masterPhysicalChannelComboBox.Size = New System.Drawing.Size(168, 21)
        Me.masterPhysicalChannelComboBox.TabIndex = 1
        Me.masterPhysicalChannelComboBox.Text = "Dev1/ai0"
        '
        'masterMinValNumeric
        '
        Me.masterMinValNumeric.DecimalPlaces = 2
        Me.masterMinValNumeric.Location = New System.Drawing.Point(152, 88)
        Me.masterMinValNumeric.Minimum = New Decimal(New Integer() {100, 0, 0, -2147483648})
        Me.masterMinValNumeric.Name = "masterMinValNumeric"
        Me.masterMinValNumeric.Size = New System.Drawing.Size(168, 20)
        Me.masterMinValNumeric.TabIndex = 5
        Me.masterMinValNumeric.Value = New Decimal(New Integer() {10, 0, 0, -2147483648})
        '
        'masterPhysChanLabel
        '
        Me.masterPhysChanLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.masterPhysChanLabel.Location = New System.Drawing.Point(16, 24)
        Me.masterPhysChanLabel.Name = "masterPhysChanLabel"
        Me.masterPhysChanLabel.Size = New System.Drawing.Size(96, 16)
        Me.masterPhysChanLabel.TabIndex = 0
        Me.masterPhysChanLabel.Text = "Physical Channel:"
        '
        'masterMaxValLabel
        '
        Me.masterMaxValLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.masterMaxValLabel.Location = New System.Drawing.Point(16, 56)
        Me.masterMaxValLabel.Name = "masterMaxValLabel"
        Me.masterMaxValLabel.Size = New System.Drawing.Size(96, 16)
        Me.masterMaxValLabel.TabIndex = 2
        Me.masterMaxValLabel.Text = "Maximum Value:"
        '
        'masterMinValLabel
        '
        Me.masterMinValLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.masterMinValLabel.Location = New System.Drawing.Point(16, 88)
        Me.masterMinValLabel.Name = "masterMinValLabel"
        Me.masterMinValLabel.Size = New System.Drawing.Size(96, 16)
        Me.masterMinValLabel.TabIndex = 4
        Me.masterMinValLabel.Text = "Minimum Value:"
        '
        'masterMaxValNumeric
        '
        Me.masterMaxValNumeric.DecimalPlaces = 2
        Me.masterMaxValNumeric.Location = New System.Drawing.Point(152, 56)
        Me.masterMaxValNumeric.Name = "masterMaxValNumeric"
        Me.masterMaxValNumeric.Size = New System.Drawing.Size(168, 20)
        Me.masterMaxValNumeric.TabIndex = 3
        Me.masterMaxValNumeric.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'slaveGroupBox
        '
        Me.slaveGroupBox.Controls.Add(Me.slavePhysicalChannelComboBox)
        Me.slaveGroupBox.Controls.Add(Me.slaveMinValNumeric)
        Me.slaveGroupBox.Controls.Add(Me.slavePhysChanLabel)
        Me.slaveGroupBox.Controls.Add(Me.slaveMaxValLabel)
        Me.slaveGroupBox.Controls.Add(Me.slaveMinValLabel)
        Me.slaveGroupBox.Controls.Add(Me.slaveMaxValNumeric)
        Me.slaveGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.slaveGroupBox.Location = New System.Drawing.Point(8, 208)
        Me.slaveGroupBox.Name = "slaveGroupBox"
        Me.slaveGroupBox.Size = New System.Drawing.Size(336, 128)
        Me.slaveGroupBox.TabIndex = 2
        Me.slaveGroupBox.TabStop = False
        Me.slaveGroupBox.Text = "Channel Parameters - Slave"
        '
        'slavePhysicalChannelComboBox
        '
        Me.slavePhysicalChannelComboBox.Location = New System.Drawing.Point(152, 24)
        Me.slavePhysicalChannelComboBox.Name = "slavePhysicalChannelComboBox"
        Me.slavePhysicalChannelComboBox.Size = New System.Drawing.Size(168, 21)
        Me.slavePhysicalChannelComboBox.TabIndex = 1
        Me.slavePhysicalChannelComboBox.Text = "Dev2/ai0"
        '
        'slaveMinValNumeric
        '
        Me.slaveMinValNumeric.DecimalPlaces = 2
        Me.slaveMinValNumeric.Location = New System.Drawing.Point(152, 88)
        Me.slaveMinValNumeric.Minimum = New Decimal(New Integer() {100, 0, 0, -2147483648})
        Me.slaveMinValNumeric.Name = "slaveMinValNumeric"
        Me.slaveMinValNumeric.Size = New System.Drawing.Size(168, 20)
        Me.slaveMinValNumeric.TabIndex = 5
        Me.slaveMinValNumeric.Value = New Decimal(New Integer() {10, 0, 0, -2147483648})
        '
        'slavePhysChanLabel
        '
        Me.slavePhysChanLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.slavePhysChanLabel.Location = New System.Drawing.Point(16, 24)
        Me.slavePhysChanLabel.Name = "slavePhysChanLabel"
        Me.slavePhysChanLabel.Size = New System.Drawing.Size(96, 16)
        Me.slavePhysChanLabel.TabIndex = 0
        Me.slavePhysChanLabel.Text = "Physical Channel:"
        '
        'slaveMaxValLabel
        '
        Me.slaveMaxValLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.slaveMaxValLabel.Location = New System.Drawing.Point(16, 56)
        Me.slaveMaxValLabel.Name = "slaveMaxValLabel"
        Me.slaveMaxValLabel.Size = New System.Drawing.Size(96, 16)
        Me.slaveMaxValLabel.TabIndex = 2
        Me.slaveMaxValLabel.Text = "Maximum Value:"
        '
        'slaveMinValLabel
        '
        Me.slaveMinValLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.slaveMinValLabel.Location = New System.Drawing.Point(16, 88)
        Me.slaveMinValLabel.Name = "slaveMinValLabel"
        Me.slaveMinValLabel.Size = New System.Drawing.Size(96, 16)
        Me.slaveMinValLabel.TabIndex = 4
        Me.slaveMinValLabel.Text = "Minimum Value:"
        '
        'slaveMaxValNumeric
        '
        Me.slaveMaxValNumeric.DecimalPlaces = 2
        Me.slaveMaxValNumeric.Location = New System.Drawing.Point(152, 56)
        Me.slaveMaxValNumeric.Name = "slaveMaxValNumeric"
        Me.slaveMaxValNumeric.Size = New System.Drawing.Size(168, 20)
        Me.slaveMaxValNumeric.TabIndex = 3
        Me.slaveMaxValNumeric.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'timingGroupBox
        '
        Me.timingGroupBox.Controls.Add(Me.rateNumeric)
        Me.timingGroupBox.Controls.Add(Me.samplesPerChannelLabel)
        Me.timingGroupBox.Controls.Add(Me.rateLabel)
        Me.timingGroupBox.Controls.Add(Me.samplesPerChannelNumeric)
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
        'samplesPerChannelLabel
        '
        Me.samplesPerChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.samplesPerChannelLabel.Location = New System.Drawing.Point(16, 24)
        Me.samplesPerChannelLabel.Name = "samplesPerChannelLabel"
        Me.samplesPerChannelLabel.Size = New System.Drawing.Size(120, 16)
        Me.samplesPerChannelLabel.TabIndex = 0
        Me.samplesPerChannelLabel.Text = "Samples Per Channel:"
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
        'samplesPerChannelNumeric
        '
        Me.samplesPerChannelNumeric.Location = New System.Drawing.Point(152, 24)
        Me.samplesPerChannelNumeric.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.samplesPerChannelNumeric.Name = "samplesPerChannelNumeric"
        Me.samplesPerChannelNumeric.Size = New System.Drawing.Size(168, 20)
        Me.samplesPerChannelNumeric.TabIndex = 1
        Me.samplesPerChannelNumeric.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        'readButton
        '
        Me.readButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.readButton.Location = New System.Drawing.Point(128, 456)
        Me.readButton.Name = "readButton"
        Me.readButton.TabIndex = 4
        Me.readButton.Text = "&Read"
        '
        'dataGridPanel
        '
        Me.dataGridPanel.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dataGridPanel.Controls.Add(Me.masterDataGroupBox)
        Me.dataGridPanel.Controls.Add(Me.dataGridSplitter)
        Me.dataGridPanel.Controls.Add(Me.rightDataGridPanel)
        Me.dataGridPanel.Location = New System.Drawing.Point(360, 0)
        Me.dataGridPanel.Name = "dataGridPanel"
        Me.dataGridPanel.Size = New System.Drawing.Size(480, 486)
        Me.dataGridPanel.TabIndex = 2
        '
        'masterDataGroupBox
        '
        Me.masterDataGroupBox.Controls.Add(Me.masterDataGrid)
        Me.masterDataGroupBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.masterDataGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.masterDataGroupBox.Location = New System.Drawing.Point(0, 0)
        Me.masterDataGroupBox.Name = "masterDataGroupBox"
        Me.masterDataGroupBox.Size = New System.Drawing.Size(229, 486)
        Me.masterDataGroupBox.TabIndex = 5
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
        Me.masterDataGrid.Size = New System.Drawing.Size(223, 467)
        Me.masterDataGrid.TabIndex = 0
        Me.masterDataGrid.TabStop = False
        '
        'dataGridSplitter
        '
        Me.dataGridSplitter.Dock = System.Windows.Forms.DockStyle.Right
        Me.dataGridSplitter.Location = New System.Drawing.Point(229, 0)
        Me.dataGridSplitter.Name = "dataGridSplitter"
        Me.dataGridSplitter.Size = New System.Drawing.Size(3, 486)
        Me.dataGridSplitter.TabIndex = 1
        Me.dataGridSplitter.TabStop = False
        '
        'rightDataGridPanel
        '
        Me.rightDataGridPanel.Controls.Add(Me.slaveDataGroupBox)
        Me.rightDataGridPanel.Dock = System.Windows.Forms.DockStyle.Right
        Me.rightDataGridPanel.Location = New System.Drawing.Point(232, 0)
        Me.rightDataGridPanel.Name = "rightDataGridPanel"
        Me.rightDataGridPanel.Size = New System.Drawing.Size(248, 486)
        Me.rightDataGridPanel.TabIndex = 0
        '
        'slaveDataGroupBox
        '
        Me.slaveDataGroupBox.Controls.Add(Me.slaveDataGrid)
        Me.slaveDataGroupBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.slaveDataGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.slaveDataGroupBox.Location = New System.Drawing.Point(0, 0)
        Me.slaveDataGroupBox.Name = "slaveDataGroupBox"
        Me.slaveDataGroupBox.Size = New System.Drawing.Size(248, 486)
        Me.slaveDataGroupBox.TabIndex = 6
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
        Me.slaveDataGrid.Size = New System.Drawing.Size(242, 467)
        Me.slaveDataGrid.TabIndex = 0
        Me.slaveDataGrid.TabStop = False
        '
        'syncGroupBox
        '
        Me.syncGroupBox.Controls.Add(Me.syncTypeComboBox)
        Me.syncGroupBox.Controls.Add(Me.Label1)
        Me.syncGroupBox.Location = New System.Drawing.Point(8, 8)
        Me.syncGroupBox.Name = "syncGroupBox"
        Me.syncGroupBox.Size = New System.Drawing.Size(336, 56)
        Me.syncGroupBox.TabIndex = 0
        Me.syncGroupBox.TabStop = False
        Me.syncGroupBox.Text = "Synchronization Parameters"
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(16, 24)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(120, 16)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Synchronization Type:"
        '
        'syncTypeComboBox
        '
        Me.syncTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.syncTypeComboBox.Items.AddRange(New Object() {"E-Series", "M-Series (PCI)", "M-Series (PXI)", "DSA Sample Clock Timebase", "DSA Reference Clock"})
        Me.syncTypeComboBox.Location = New System.Drawing.Point(152, 22)
        Me.syncTypeComboBox.Name = "syncTypeComboBox"
        Me.syncTypeComboBox.Size = New System.Drawing.Size(168, 21)
        Me.syncTypeComboBox.TabIndex = 1
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(840, 486)
        Me.Controls.Add(Me.syncGroupBox)
        Me.Controls.Add(Me.dataGridPanel)
        Me.Controls.Add(Me.readButton)
        Me.Controls.Add(Me.slaveGroupBox)
        Me.Controls.Add(Me.masterGroupBox)
        Me.Controls.Add(Me.timingGroupBox)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "MainForm"
        Me.Text = "Multi-Device Sync - Analog Input - Finite Acquisition"
        Me.masterGroupBox.ResumeLayout(False)
        CType(Me.masterMinValNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.masterMaxValNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.slaveGroupBox.ResumeLayout(False)
        CType(Me.slaveMinValNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.slaveMaxValNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.timingGroupBox.ResumeLayout(False)
        CType(Me.rateNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.samplesPerChannelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.dataGridPanel.ResumeLayout(False)
        Me.masterDataGroupBox.ResumeLayout(False)
        CType(Me.masterDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.rightDataGridPanel.ResumeLayout(False)
        Me.slaveDataGroupBox.ResumeLayout(False)
        CType(Me.slaveDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.syncGroupBox.ResumeLayout(False)
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

    Private Sub readButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles readButton.Click
        ' Change the mouse to an hourglass for the duration of this function.
        System.Windows.Forms.Cursor.Current = Cursors.WaitCursor

        Dim master As SyncTask = Nothing
        Dim slave As SyncTask = Nothing

        Try
            ' Note: This example uses the SyncTask helper class.
            ' See the SyncTask.cs file in this example for the
            ' implementation of the SyncTask class.
            ' Create the master and slave tasks
            master = New SyncTask("master", syncTypeComboBox.SelectedIndex)
            slave = New SyncTask("slave", syncTypeComboBox.SelectedIndex)

            ' Configure both tasks with the values selected on the UI.
            master.ConfigureDecimal(masterPhysicalChannelComboBox.Text, masterMinValNumeric.Value, masterMaxValNumeric.Value, samplesPerChannelNumeric.Value, rateNumeric.Value)
            slave.ConfigureDecimal(slavePhysicalChannelComboBox.Text, slaveMinValNumeric.Value, slaveMaxValNumeric.Value, samplesPerChannelNumeric.Value, rateNumeric.Value)

            ' Hook up the data grids to the data tables contained in
            ' the SyncTask classes.
            masterDataGrid.DataSource = master.DataTable
            slaveDataGrid.DataSource = slave.DataTable

            ' Synchronize the slave task to the master task.
            ' (See SyncTask.cs for details.)
            master.SynchronizeMaster()
            slave.SynchronizeSlave(master)

            ' Start both tasks.
            ' Note: Start the slave task first because it is waiting on
            ' the master task.
            slave.Start()
            master.Start()

            ' Read from both tasks and update the Data Grids.
            ' (The Read method updates the Data Table, which
            ' automatically causes the Data Grid to refresh).
            master.Read()
            slave.Read()
        Catch ex As System.Exception
            ' Popup a dialog if an exception is thrown.
            MessageBox.Show(ex.Message)
        Finally
            slave.Dispose()
            master.Dispose()
        End Try
    End Sub 'readButton_Click
End Class 'MainForm
