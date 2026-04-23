/******************************************************************************
*
* Example program:
*   AIFiniteAcquisition
*
* Category:
*   Synchronization
*
* Description:
*   This example demonstrates how to acquire a finite amount of analog input
*   data using two 
*   DAQ devices' internal clocks.  It also synchronizes these devices depending on
*   the device family (E Series, 
*   M Series, or DSA) to simultaneously acquire the data.NOTE: This example is
*   intended to show low level 
*   synchronization of various devices. DSA and S Series devices now support
*   including channels from multiple 
*   devices in a single task. DAQmx automatically synchronizes the devices in such
*   a task. See the DAQmx 
*   Help>>NI-DAQmx Device Considerations>>Multidevice Tasks section
*   for further details.NOTE: 
*   PXI 6115 and 6120 (S Series) devices don't require sharing of master timebase,
*   because they auto-lock 
*   to Clock 10.  For those devices sharing a start trigger is adequate.NOTE: For
*   the PCI-6154 S Series device 
*   use the M Series (PCI) synchronization type to synchronize using the reference
*   clock.
*
* Instructions for running:
*   1.  Select the synchronization type according to the devices you are using
*       for acquisition.
*   2.  Select the physical channels which correspond to where your signals are
*       input on the DAQ device.
*   3.  Enter the minimum and maximum voltage ranges for the physical channels.
*   4.  Set the number of samples to acquire per channel.
*   5.  Set the rate of the acquisition, in Hertz.
*
* Steps:
*   1.  Create an analog input voltage channel for both the master and slave
*       devices.
*   2.  Set timing parameters for the acquisition.
*   3.  Call master.SynchronizeMaster() to configure the master device for
*       synchronization, depending on the synchronization type.
*   4.  Call slave.SynchronizeSlave(master) to configure the slave device for
*       synchronization, depending on the synchronization type.
*   5.  Start the tasks.  The slave task must be started first so that it can
*       wait on the master's start trigger.
*   6.  Read all of the analog input data from both devices.
*   7.  Dispose the Task object to clean-up any resources associated with the
*       task.
*   8.  Handle any DaqExceptions, if they occur.
*
* I/O Connections Overview:
*   Make sure your signal input terminals match the Physical Channel I/O
*   Controls.  If you have a PXI chassis, ensure that it has been properly
*   identified in MAX.  If you have devices with a RTSI bus, ensure that they
*   are connected with a RTSI cable and that the RTSI cable is registered in
*   MAX.
*
* Microsoft Windows Vista User Account Control
*   Running certain applications on Microsoft Windows Vista requires
*   administrator privileges, 
*   because the application name contains keywords such as setup, update, or
*   install. To avoid this problem, 
*   you must add an additional manifest to the application that specifies the
*   privileges required to run 
*   the application. Some Measurement Studio NI-DAQmx examples for Visual Studio
*   include these keywords. 
*   Therefore, all examples for Visual Studio are shipped with an additional
*   manifest file that you must 
*   embed in the example executable. The manifest file is named
*   [ExampleName].exe.manifest, where [ExampleName] 
*   is the NI-provided example name. For information on how to embed the manifest
*   file, refer to http://msdn2.microsoft.com/en-us/library/bb756929.aspx.Note: 
*   The manifest file is not provided with examples for Visual Studio .NET 2003.
*
******************************************************************************/

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using NationalInstruments;
using NationalInstruments.DAQmx;

namespace NationalInstruments.Examples.AIFiniteAcquisition
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.GroupBox masterGroupBox;
        private System.Windows.Forms.Label masterPhysChanLabel;
        private System.Windows.Forms.Label masterMaxValLabel;
        private System.Windows.Forms.Label masterMinValLabel;
        private System.Windows.Forms.NumericUpDown masterMinValNumeric;
        private System.Windows.Forms.NumericUpDown masterMaxValNumeric;
        private System.Windows.Forms.GroupBox slaveGroupBox;
        private System.Windows.Forms.Label slavePhysChanLabel;
        private System.Windows.Forms.Label slaveMaxValLabel;
        private System.Windows.Forms.Label slaveMinValLabel;
        private System.Windows.Forms.NumericUpDown slaveMinValNumeric;
        private System.Windows.Forms.NumericUpDown slaveMaxValNumeric;
        private System.Windows.Forms.GroupBox timingGroupBox;
        private System.Windows.Forms.NumericUpDown rateNumeric;
        private System.Windows.Forms.Label samplesPerChannelLabel;
        private System.Windows.Forms.Label rateLabel;
        private System.Windows.Forms.NumericUpDown samplesPerChannelNumeric;
        private System.Windows.Forms.Button readButton;
        private System.Windows.Forms.Panel dataGridPanel;
        private System.Windows.Forms.Panel rightDataGridPanel;
        private System.Windows.Forms.Splitter dataGridSplitter;
        private System.Windows.Forms.GroupBox masterDataGroupBox;
        private System.Windows.Forms.DataGrid masterDataGrid;
        private System.Windows.Forms.DataGrid slaveDataGrid;
        private System.Windows.Forms.GroupBox slaveDataGroupBox;
        private System.Windows.Forms.ComboBox masterPhysicalChannelComboBox;
        private System.Windows.Forms.ComboBox slavePhysicalChannelComboBox;
        private System.Windows.Forms.GroupBox syncGroupBox;
        private System.Windows.Forms.Label syncTypeLabel;
        private System.Windows.Forms.ComboBox syncTypeComboBox;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public MainForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            masterPhysicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AI, PhysicalChannelAccess.External));
            if (masterPhysicalChannelComboBox.Items.Count > 0)
                masterPhysicalChannelComboBox.SelectedIndex = 0;

            slavePhysicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AI, PhysicalChannelAccess.External));
            if (slavePhysicalChannelComboBox.Items.Count > 0)
                slavePhysicalChannelComboBox.SelectedIndex = 0;

            syncTypeComboBox.SelectedIndex = 0;

            ConfigNumeric(masterMaxValNumeric);
            ConfigNumeric(masterMinValNumeric);
            ConfigNumeric(slaveMaxValNumeric);
            ConfigNumeric(slaveMinValNumeric);
            ConfigNumeric(samplesPerChannelNumeric,Decimal.Zero);
            ConfigNumeric(rateNumeric, Decimal.Zero);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                if (components != null) 
                {
                    components.Dispose();
                }
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainForm));
            this.masterGroupBox = new System.Windows.Forms.GroupBox();
            this.masterPhysicalChannelComboBox = new System.Windows.Forms.ComboBox();
            this.masterMinValNumeric = new System.Windows.Forms.NumericUpDown();
            this.masterPhysChanLabel = new System.Windows.Forms.Label();
            this.masterMaxValLabel = new System.Windows.Forms.Label();
            this.masterMinValLabel = new System.Windows.Forms.Label();
            this.masterMaxValNumeric = new System.Windows.Forms.NumericUpDown();
            this.slaveGroupBox = new System.Windows.Forms.GroupBox();
            this.slavePhysicalChannelComboBox = new System.Windows.Forms.ComboBox();
            this.slaveMinValNumeric = new System.Windows.Forms.NumericUpDown();
            this.slavePhysChanLabel = new System.Windows.Forms.Label();
            this.slaveMaxValLabel = new System.Windows.Forms.Label();
            this.slaveMinValLabel = new System.Windows.Forms.Label();
            this.slaveMaxValNumeric = new System.Windows.Forms.NumericUpDown();
            this.timingGroupBox = new System.Windows.Forms.GroupBox();
            this.rateNumeric = new System.Windows.Forms.NumericUpDown();
            this.samplesPerChannelLabel = new System.Windows.Forms.Label();
            this.rateLabel = new System.Windows.Forms.Label();
            this.samplesPerChannelNumeric = new System.Windows.Forms.NumericUpDown();
            this.readButton = new System.Windows.Forms.Button();
            this.dataGridPanel = new System.Windows.Forms.Panel();
            this.masterDataGroupBox = new System.Windows.Forms.GroupBox();
            this.masterDataGrid = new System.Windows.Forms.DataGrid();
            this.dataGridSplitter = new System.Windows.Forms.Splitter();
            this.rightDataGridPanel = new System.Windows.Forms.Panel();
            this.slaveDataGroupBox = new System.Windows.Forms.GroupBox();
            this.slaveDataGrid = new System.Windows.Forms.DataGrid();
            this.syncGroupBox = new System.Windows.Forms.GroupBox();
            this.syncTypeLabel = new System.Windows.Forms.Label();
            this.syncTypeComboBox = new System.Windows.Forms.ComboBox();
            this.masterGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.masterMinValNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.masterMaxValNumeric)).BeginInit();
            this.slaveGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.slaveMinValNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.slaveMaxValNumeric)).BeginInit();
            this.timingGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rateNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerChannelNumeric)).BeginInit();
            this.dataGridPanel.SuspendLayout();
            this.masterDataGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.masterDataGrid)).BeginInit();
            this.rightDataGridPanel.SuspendLayout();
            this.slaveDataGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.slaveDataGrid)).BeginInit();
            this.syncGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // masterGroupBox
            // 
            this.masterGroupBox.Controls.Add(this.masterPhysicalChannelComboBox);
            this.masterGroupBox.Controls.Add(this.masterMinValNumeric);
            this.masterGroupBox.Controls.Add(this.masterPhysChanLabel);
            this.masterGroupBox.Controls.Add(this.masterMaxValLabel);
            this.masterGroupBox.Controls.Add(this.masterMinValLabel);
            this.masterGroupBox.Controls.Add(this.masterMaxValNumeric);
            this.masterGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.masterGroupBox.Location = new System.Drawing.Point(8, 72);
            this.masterGroupBox.Name = "masterGroupBox";
            this.masterGroupBox.Size = new System.Drawing.Size(336, 128);
            this.masterGroupBox.TabIndex = 1;
            this.masterGroupBox.TabStop = false;
            this.masterGroupBox.Text = "Channel Parameters - Master";
            // 
            // masterPhysicalChannelComboBox
            // 
            this.masterPhysicalChannelComboBox.Location = new System.Drawing.Point(152, 24);
            this.masterPhysicalChannelComboBox.Name = "masterPhysicalChannelComboBox";
            this.masterPhysicalChannelComboBox.Size = new System.Drawing.Size(168, 21);
            this.masterPhysicalChannelComboBox.TabIndex = 1;
            this.masterPhysicalChannelComboBox.Text = "Dev1/ai0";
            // 
            // masterMinValNumeric
            // 
            this.masterMinValNumeric.DecimalPlaces = 2;
            this.masterMinValNumeric.Location = new System.Drawing.Point(152, 88);
            this.masterMinValNumeric.Minimum = new System.Decimal(new int[] {
                                                                                100,
                                                                                0,
                                                                                0,
                                                                                -2147483648});
            this.masterMinValNumeric.Name = "masterMinValNumeric";
            this.masterMinValNumeric.Size = new System.Drawing.Size(168, 20);
            this.masterMinValNumeric.TabIndex = 5;
            this.masterMinValNumeric.Value = new System.Decimal(new int[] {
                                                                              10,
                                                                              0,
                                                                              0,
                                                                              -2147483648});
            // 
            // masterPhysChanLabel
            // 
            this.masterPhysChanLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.masterPhysChanLabel.Location = new System.Drawing.Point(16, 24);
            this.masterPhysChanLabel.Name = "masterPhysChanLabel";
            this.masterPhysChanLabel.Size = new System.Drawing.Size(96, 16);
            this.masterPhysChanLabel.TabIndex = 0;
            this.masterPhysChanLabel.Text = "Physical Channel:";
            // 
            // masterMaxValLabel
            // 
            this.masterMaxValLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.masterMaxValLabel.Location = new System.Drawing.Point(16, 56);
            this.masterMaxValLabel.Name = "masterMaxValLabel";
            this.masterMaxValLabel.Size = new System.Drawing.Size(96, 16);
            this.masterMaxValLabel.TabIndex = 2;
            this.masterMaxValLabel.Text = "Maximum Value:";
            // 
            // masterMinValLabel
            // 
            this.masterMinValLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.masterMinValLabel.Location = new System.Drawing.Point(16, 88);
            this.masterMinValLabel.Name = "masterMinValLabel";
            this.masterMinValLabel.Size = new System.Drawing.Size(96, 16);
            this.masterMinValLabel.TabIndex = 4;
            this.masterMinValLabel.Text = "Minimum Value:";
            // 
            // masterMaxValNumeric
            // 
            this.masterMaxValNumeric.DecimalPlaces = 2;
            this.masterMaxValNumeric.Location = new System.Drawing.Point(152, 56);
            this.masterMaxValNumeric.Name = "masterMaxValNumeric";
            this.masterMaxValNumeric.Size = new System.Drawing.Size(168, 20);
            this.masterMaxValNumeric.TabIndex = 3;
            this.masterMaxValNumeric.Value = new System.Decimal(new int[] {
                                                                              10,
                                                                              0,
                                                                              0,
                                                                              0});
            // 
            // slaveGroupBox
            // 
            this.slaveGroupBox.Controls.Add(this.slavePhysicalChannelComboBox);
            this.slaveGroupBox.Controls.Add(this.slaveMinValNumeric);
            this.slaveGroupBox.Controls.Add(this.slavePhysChanLabel);
            this.slaveGroupBox.Controls.Add(this.slaveMaxValLabel);
            this.slaveGroupBox.Controls.Add(this.slaveMinValLabel);
            this.slaveGroupBox.Controls.Add(this.slaveMaxValNumeric);
            this.slaveGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.slaveGroupBox.Location = new System.Drawing.Point(8, 208);
            this.slaveGroupBox.Name = "slaveGroupBox";
            this.slaveGroupBox.Size = new System.Drawing.Size(336, 128);
            this.slaveGroupBox.TabIndex = 2;
            this.slaveGroupBox.TabStop = false;
            this.slaveGroupBox.Text = "Channel Parameters - Slave";
            // 
            // slavePhysicalChannelComboBox
            // 
            this.slavePhysicalChannelComboBox.Location = new System.Drawing.Point(152, 24);
            this.slavePhysicalChannelComboBox.Name = "slavePhysicalChannelComboBox";
            this.slavePhysicalChannelComboBox.Size = new System.Drawing.Size(168, 21);
            this.slavePhysicalChannelComboBox.TabIndex = 1;
            this.slavePhysicalChannelComboBox.Text = "Dev2/ai0";
            // 
            // slaveMinValNumeric
            // 
            this.slaveMinValNumeric.DecimalPlaces = 2;
            this.slaveMinValNumeric.Location = new System.Drawing.Point(152, 88);
            this.slaveMinValNumeric.Minimum = new System.Decimal(new int[] {
                                                                               100,
                                                                               0,
                                                                               0,
                                                                               -2147483648});
            this.slaveMinValNumeric.Name = "slaveMinValNumeric";
            this.slaveMinValNumeric.Size = new System.Drawing.Size(168, 20);
            this.slaveMinValNumeric.TabIndex = 5;
            this.slaveMinValNumeric.Value = new System.Decimal(new int[] {
                                                                             10,
                                                                             0,
                                                                             0,
                                                                             -2147483648});
            // 
            // slavePhysChanLabel
            // 
            this.slavePhysChanLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.slavePhysChanLabel.Location = new System.Drawing.Point(16, 24);
            this.slavePhysChanLabel.Name = "slavePhysChanLabel";
            this.slavePhysChanLabel.Size = new System.Drawing.Size(96, 16);
            this.slavePhysChanLabel.TabIndex = 0;
            this.slavePhysChanLabel.Text = "Physical Channel:";
            // 
            // slaveMaxValLabel
            // 
            this.slaveMaxValLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.slaveMaxValLabel.Location = new System.Drawing.Point(16, 56);
            this.slaveMaxValLabel.Name = "slaveMaxValLabel";
            this.slaveMaxValLabel.Size = new System.Drawing.Size(96, 16);
            this.slaveMaxValLabel.TabIndex = 2;
            this.slaveMaxValLabel.Text = "Maximum Value:";
            // 
            // slaveMinValLabel
            // 
            this.slaveMinValLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.slaveMinValLabel.Location = new System.Drawing.Point(16, 88);
            this.slaveMinValLabel.Name = "slaveMinValLabel";
            this.slaveMinValLabel.Size = new System.Drawing.Size(96, 16);
            this.slaveMinValLabel.TabIndex = 4;
            this.slaveMinValLabel.Text = "Minimum Value:";
            // 
            // slaveMaxValNumeric
            // 
            this.slaveMaxValNumeric.DecimalPlaces = 2;
            this.slaveMaxValNumeric.Location = new System.Drawing.Point(152, 56);
            this.slaveMaxValNumeric.Name = "slaveMaxValNumeric";
            this.slaveMaxValNumeric.Size = new System.Drawing.Size(168, 20);
            this.slaveMaxValNumeric.TabIndex = 3;
            this.slaveMaxValNumeric.Value = new System.Decimal(new int[] {
                                                                             10,
                                                                             0,
                                                                             0,
                                                                             0});
            // 
            // timingGroupBox
            // 
            this.timingGroupBox.Controls.Add(this.rateNumeric);
            this.timingGroupBox.Controls.Add(this.samplesPerChannelLabel);
            this.timingGroupBox.Controls.Add(this.rateLabel);
            this.timingGroupBox.Controls.Add(this.samplesPerChannelNumeric);
            this.timingGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.timingGroupBox.Location = new System.Drawing.Point(8, 344);
            this.timingGroupBox.Name = "timingGroupBox";
            this.timingGroupBox.Size = new System.Drawing.Size(336, 96);
            this.timingGroupBox.TabIndex = 3;
            this.timingGroupBox.TabStop = false;
            this.timingGroupBox.Text = "Timing Parameters";
            // 
            // rateNumeric
            // 
            this.rateNumeric.DecimalPlaces = 2;
            this.rateNumeric.Location = new System.Drawing.Point(152, 56);
            this.rateNumeric.Maximum = new System.Decimal(new int[] {
                                                                        102400,
                                                                        0,
                                                                        0,
                                                                        0});
            this.rateNumeric.Name = "rateNumeric";
            this.rateNumeric.Size = new System.Drawing.Size(168, 20);
            this.rateNumeric.TabIndex = 3;
            this.rateNumeric.Value = new System.Decimal(new int[] {
                                                                      1000,
                                                                      0,
                                                                      0,
                                                                      0});
            // 
            // samplesPerChannelLabel
            // 
            this.samplesPerChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.samplesPerChannelLabel.Location = new System.Drawing.Point(16, 24);
            this.samplesPerChannelLabel.Name = "samplesPerChannelLabel";
            this.samplesPerChannelLabel.Size = new System.Drawing.Size(120, 16);
            this.samplesPerChannelLabel.TabIndex = 0;
            this.samplesPerChannelLabel.Text = "Samples Per Channel:";
            // 
            // rateLabel
            // 
            this.rateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rateLabel.Location = new System.Drawing.Point(16, 56);
            this.rateLabel.Name = "rateLabel";
            this.rateLabel.Size = new System.Drawing.Size(96, 16);
            this.rateLabel.TabIndex = 2;
            this.rateLabel.Text = "Rate (Hz):";
            // 
            // samplesPerChannelNumeric
            // 
            this.samplesPerChannelNumeric.Location = new System.Drawing.Point(152, 24);
            this.samplesPerChannelNumeric.Maximum = new System.Decimal(new int[] {
                                                                                     1000,
                                                                                     0,
                                                                                     0,
                                                                                     0});
            this.samplesPerChannelNumeric.Name = "samplesPerChannelNumeric";
            this.samplesPerChannelNumeric.Size = new System.Drawing.Size(168, 20);
            this.samplesPerChannelNumeric.TabIndex = 1;
            this.samplesPerChannelNumeric.Value = new System.Decimal(new int[] {
                                                                                   100,
                                                                                   0,
                                                                                   0,
                                                                                   0});
            // 
            // readButton
            // 
            this.readButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.readButton.Location = new System.Drawing.Point(128, 456);
            this.readButton.Name = "readButton";
            this.readButton.TabIndex = 4;
            this.readButton.Text = "&Read";
            this.readButton.Click += new System.EventHandler(this.readButton_Click);
            // 
            // dataGridPanel
            // 
            this.dataGridPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
                | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridPanel.Controls.Add(this.masterDataGroupBox);
            this.dataGridPanel.Controls.Add(this.dataGridSplitter);
            this.dataGridPanel.Controls.Add(this.rightDataGridPanel);
            this.dataGridPanel.Location = new System.Drawing.Point(360, 0);
            this.dataGridPanel.Name = "dataGridPanel";
            this.dataGridPanel.Size = new System.Drawing.Size(480, 486);
            this.dataGridPanel.TabIndex = 2;
            // 
            // masterDataGroupBox
            // 
            this.masterDataGroupBox.Controls.Add(this.masterDataGrid);
            this.masterDataGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.masterDataGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.masterDataGroupBox.Location = new System.Drawing.Point(0, 0);
            this.masterDataGroupBox.Name = "masterDataGroupBox";
            this.masterDataGroupBox.Size = new System.Drawing.Size(229, 486);
            this.masterDataGroupBox.TabIndex = 4;
            this.masterDataGroupBox.TabStop = false;
            this.masterDataGroupBox.Text = "Master Data";
            // 
            // masterDataGrid
            // 
            this.masterDataGrid.DataMember = "";
            this.masterDataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.masterDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.masterDataGrid.Location = new System.Drawing.Point(3, 16);
            this.masterDataGrid.Name = "masterDataGrid";
            this.masterDataGrid.PreferredColumnWidth = 100;
            this.masterDataGrid.Size = new System.Drawing.Size(223, 467);
            this.masterDataGrid.TabIndex = 0;
            this.masterDataGrid.TabStop = false;
            // 
            // dataGridSplitter
            // 
            this.dataGridSplitter.Dock = System.Windows.Forms.DockStyle.Right;
            this.dataGridSplitter.Location = new System.Drawing.Point(229, 0);
            this.dataGridSplitter.Name = "dataGridSplitter";
            this.dataGridSplitter.Size = new System.Drawing.Size(3, 486);
            this.dataGridSplitter.TabIndex = 1;
            this.dataGridSplitter.TabStop = false;
            // 
            // rightDataGridPanel
            // 
            this.rightDataGridPanel.Controls.Add(this.slaveDataGroupBox);
            this.rightDataGridPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.rightDataGridPanel.Location = new System.Drawing.Point(232, 0);
            this.rightDataGridPanel.Name = "rightDataGridPanel";
            this.rightDataGridPanel.Size = new System.Drawing.Size(248, 486);
            this.rightDataGridPanel.TabIndex = 0;
            // 
            // slaveDataGroupBox
            // 
            this.slaveDataGroupBox.Controls.Add(this.slaveDataGrid);
            this.slaveDataGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.slaveDataGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.slaveDataGroupBox.Location = new System.Drawing.Point(0, 0);
            this.slaveDataGroupBox.Name = "slaveDataGroupBox";
            this.slaveDataGroupBox.Size = new System.Drawing.Size(248, 486);
            this.slaveDataGroupBox.TabIndex = 5;
            this.slaveDataGroupBox.TabStop = false;
            this.slaveDataGroupBox.Text = "Slave Data";
            // 
            // slaveDataGrid
            // 
            this.slaveDataGrid.DataMember = "";
            this.slaveDataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.slaveDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.slaveDataGrid.Location = new System.Drawing.Point(3, 16);
            this.slaveDataGrid.Name = "slaveDataGrid";
            this.slaveDataGrid.PreferredColumnWidth = 100;
            this.slaveDataGrid.Size = new System.Drawing.Size(242, 467);
            this.slaveDataGrid.TabIndex = 0;
            this.slaveDataGrid.TabStop = false;
            // 
            // syncGroupBox
            // 
            this.syncGroupBox.Controls.Add(this.syncTypeLabel);
            this.syncGroupBox.Controls.Add(this.syncTypeComboBox);
            this.syncGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.syncGroupBox.Location = new System.Drawing.Point(8, 8);
            this.syncGroupBox.Name = "syncGroupBox";
            this.syncGroupBox.Size = new System.Drawing.Size(336, 56);
            this.syncGroupBox.TabIndex = 0;
            this.syncGroupBox.TabStop = false;
            this.syncGroupBox.Text = "Synchronization Parameters";
            // 
            // syncTypeLabel
            // 
            this.syncTypeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.syncTypeLabel.Location = new System.Drawing.Point(16, 24);
            this.syncTypeLabel.Name = "syncTypeLabel";
            this.syncTypeLabel.Size = new System.Drawing.Size(120, 16);
            this.syncTypeLabel.TabIndex = 0;
            this.syncTypeLabel.Text = "Synchronization Type:";
            // 
            // syncTypeComboBox
            // 
            this.syncTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.syncTypeComboBox.Items.AddRange(new object[] {
                                                                  "E-Series",
                                                                  "M-Series (PCI)",
                                                                  "M-Series (PXI)",
                                                                  "DSA Sample Clock Timebase",
                                                                  "DSA Reference Clock"});
            this.syncTypeComboBox.Location = new System.Drawing.Point(152, 22);
            this.syncTypeComboBox.Name = "syncTypeComboBox";
            this.syncTypeComboBox.Size = new System.Drawing.Size(168, 21);
            this.syncTypeComboBox.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(840, 486);
            this.Controls.Add(this.syncGroupBox);
            this.Controls.Add(this.dataGridPanel);
            this.Controls.Add(this.readButton);
            this.Controls.Add(this.slaveGroupBox);
            this.Controls.Add(this.masterGroupBox);
            this.Controls.Add(this.timingGroupBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Multi-Device Sync - Analog Input - Finite Acquisition";
            this.masterGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.masterMinValNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.masterMaxValNumeric)).EndInit();
            this.slaveGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.slaveMinValNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.slaveMaxValNumeric)).EndInit();
            this.timingGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rateNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerChannelNumeric)).EndInit();
            this.dataGridPanel.ResumeLayout(false);
            this.masterDataGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.masterDataGrid)).EndInit();
            this.rightDataGridPanel.ResumeLayout(false);
            this.slaveDataGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.slaveDataGrid)).EndInit();
            this.syncGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() 
        {
            Application.EnableVisualStyles();
            Application.DoEvents();
            Application.Run(new MainForm());
        }

        private void ConfigNumeric(NumericUpDown numeric, decimal minVal)
        {
            numeric.Minimum = minVal;
            numeric.Maximum = Decimal.MaxValue;
        }

        private void ConfigNumeric(NumericUpDown numeric)
        {
            ConfigNumeric(numeric,Decimal.MinValue);
        }

        private void readButton_Click(object sender, System.EventArgs e)
        {
            // Change the mouse to an hourglass for the duration of this function.
            Cursor.Current = Cursors.WaitCursor;

            SyncTask master = null;
            SyncTask slave = null;

            try
            {
                // Note: This example uses the SyncTask helper class.
                // See the SyncTask.cs file in this example for the
                // implementation of the SyncTask class.

                // Create the master and slave tasks
                master = new SyncTask("master", syncTypeComboBox.SelectedIndex);
                slave = new SyncTask("slave", syncTypeComboBox.SelectedIndex);

                // Configure both tasks with the values selected on the UI.
                master.ConfigureDecimal(masterPhysicalChannelComboBox.Text,masterMinValNumeric.Value,
                    masterMaxValNumeric.Value,samplesPerChannelNumeric.Value,rateNumeric.Value);
                slave.ConfigureDecimal(slavePhysicalChannelComboBox.Text,slaveMinValNumeric.Value,
                    slaveMaxValNumeric.Value,samplesPerChannelNumeric.Value,rateNumeric.Value);

                // Hook up the data grids to the data tables contained in
                // the SyncTask classes.
                masterDataGrid.DataSource = master.DataTable;
                slaveDataGrid.DataSource = slave.DataTable;

                // Synchronize the slave task to the master task.
                // (See SyncTask.cs for details.)
                master.SynchronizeMaster();
                slave.SynchronizeSlave(master);

                // Start both tasks.
                // Note: Start the slave task first because it is waiting on
                // the master task.
                slave.Start();
                master.Start();

                // Read from both tasks and update the Data Grids.
                // (The Read method updates the Data Table, which
                // automatically causes the Data Grid to refresh).
                master.Read();
                slave.Read();
            }
            catch(Exception ex)
            {
                // Popup a dialog if an exception is thrown.
                MessageBox.Show(ex.Message);
            }
            finally
            {
                slave.Dispose();
                master.Dispose();
            }
        }
    }
}
