/******************************************************************************
*
* Example program:
*   AIContAcquisition
*
* Category:
*   Synchronization
*
* Description:
*   This example demonstrates how to acquire a continuous amount of data using
*   the DAQ device's 
*   internal clock.  It also shows how to synchronize two devices for different
*   device families (E Series, 
*   M Series, and DSA), to simultaneously acquire the data.NOTE: This example is
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
*   5.  Start the tasks. The slave task must be started first so that it can
*       wait on the master's start trigger.
*   6.  The following three steps are done for both master and slave.
*   7.  Call AnalogMultiChannelReader.BeginReadMultiSample to install a callback
*       and begin the asynchronous read operation.
*   8.  Inside the callback, call AnalogMultiChannelReader.EndReadMultiSample to
*       retrieve the data from the read operation.  
*   9.  Call AnalogMultiChannelReader.BeginReadMultiSample again inside the
*       callback to perform another read operation.
*   10. Dispose the Task object to clean-up any resources associated with the
*       task.
*   11. Handle any DaqExceptions, if they occur.
*
*   Note: This example sets SynchronizeCallback to true. If SynchronizeCallback
*   is set to false, then you must give special consideration to safely dispose
*   the task and to update the UI from the callback. If SynchronizeCallback is
*   set to false, the callback executes on the worker thread and not on the main
*   UI thread. You can only update a UI component on the thread on which it was
*   created. Refer to the How to: Safely Dispose Task When Using Asynchronous
*   Callbacks topic in the NI-DAQmx .NET help for more information.
*
* I/O Connections Overview:
*   Make sure your signal input terminal matches the Physical Channel I/O
*   control. If you have a PXI chassis, ensure it has been properly identified
*   in MAX.  If you have devices with a RTSI bus, ensure they are connected with
*   a RTSI cable and that the RTSI cable is registered in MAX.
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

namespace NationalInstruments.Examples.AIContAcquisition
{
    /// <summary>
    /// Summary description for MainForm.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        private SyncTask runningTask;
        private SyncTask master = null;
        private SyncTask slave = null;
        private AsyncCallback masterCallback;
        private AsyncCallback slaveCallback;
        private System.Windows.Forms.GroupBox masterGroupBox;
        private System.Windows.Forms.GroupBox slaveGroupBox;
        private System.Windows.Forms.GroupBox timingGroupBox;
        private System.Windows.Forms.GroupBox masterDataGroupBox;
        private System.Windows.Forms.GroupBox slaveDataGroupBox;
        private System.Windows.Forms.DataGrid slaveDataGrid;
        private System.Windows.Forms.DataGrid masterDataGrid;
        private System.Windows.Forms.Label rateLabel;
        private System.Windows.Forms.Label samplesPerChannelLabel;
        private System.Windows.Forms.NumericUpDown samplesPerChannelNumeric;
        private System.Windows.Forms.NumericUpDown rateNumeric;
        private System.Windows.Forms.ComboBox slavePhysicalChannelComboBox;
        private System.Windows.Forms.ComboBox masterPhysicalChannelComboBox;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Label synchronizationTypeLabel;
        private System.Windows.Forms.GroupBox synchronizationGroupBox;
        private System.Windows.Forms.Label masterMinimumValueLabel;
        private System.Windows.Forms.Label masterMaximumValueLabel;
        private System.Windows.Forms.Label masterPhysicalChannelLabel;
        private System.Windows.Forms.Label slavePhysicalChannelLabel;
        private System.Windows.Forms.Label slaveMaximumValueLabel;
        private System.Windows.Forms.Label slaveMinimujValueLabel;
        private System.Windows.Forms.ComboBox synchronizationTypeComboBox;
        private System.Windows.Forms.NumericUpDown masterMaximumValueNumeric;
        private System.Windows.Forms.NumericUpDown masterMinimumValueNumeric;
        private System.Windows.Forms.NumericUpDown slaveMaximumValueNumeric;
        private System.Windows.Forms.NumericUpDown slaveMinimumValueNumeric;
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
            if(masterPhysicalChannelComboBox.Items.Count > 0)
                masterPhysicalChannelComboBox.SelectedIndex = 0;

            slavePhysicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AI, PhysicalChannelAccess.External));
            if(slavePhysicalChannelComboBox.Items.Count > 0)
                slavePhysicalChannelComboBox.SelectedIndex = 0;

            if (masterPhysicalChannelComboBox.Items.Count > 0 && slavePhysicalChannelComboBox.Items.Count > 0)
                startButton.Enabled = true;

            synchronizationTypeComboBox.SelectedIndex = 0;

            ConfigNumeric(masterMaximumValueNumeric);
            ConfigNumeric(masterMinimumValueNumeric);
            ConfigNumeric(slaveMaximumValueNumeric);
            ConfigNumeric(slaveMinimumValueNumeric);
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
                if (master != null)
                {
                    runningTask = null;
                    master.Dispose();
                }
                if (slave != null)
                {
                    slave.Dispose();
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
            this.synchronizationGroupBox = new System.Windows.Forms.GroupBox();
            this.synchronizationTypeComboBox = new System.Windows.Forms.ComboBox();
            this.synchronizationTypeLabel = new System.Windows.Forms.Label();
            this.masterGroupBox = new System.Windows.Forms.GroupBox();
            this.masterMinimumValueNumeric = new System.Windows.Forms.NumericUpDown();
            this.masterMaximumValueNumeric = new System.Windows.Forms.NumericUpDown();
            this.masterPhysicalChannelComboBox = new System.Windows.Forms.ComboBox();
            this.masterMinimumValueLabel = new System.Windows.Forms.Label();
            this.masterMaximumValueLabel = new System.Windows.Forms.Label();
            this.masterPhysicalChannelLabel = new System.Windows.Forms.Label();
            this.slaveGroupBox = new System.Windows.Forms.GroupBox();
            this.slaveMinimumValueNumeric = new System.Windows.Forms.NumericUpDown();
            this.slaveMaximumValueNumeric = new System.Windows.Forms.NumericUpDown();
            this.slavePhysicalChannelComboBox = new System.Windows.Forms.ComboBox();
            this.slavePhysicalChannelLabel = new System.Windows.Forms.Label();
            this.slaveMaximumValueLabel = new System.Windows.Forms.Label();
            this.slaveMinimujValueLabel = new System.Windows.Forms.Label();
            this.timingGroupBox = new System.Windows.Forms.GroupBox();
            this.rateNumeric = new System.Windows.Forms.NumericUpDown();
            this.samplesPerChannelNumeric = new System.Windows.Forms.NumericUpDown();
            this.samplesPerChannelLabel = new System.Windows.Forms.Label();
            this.rateLabel = new System.Windows.Forms.Label();
            this.masterDataGroupBox = new System.Windows.Forms.GroupBox();
            this.masterDataGrid = new System.Windows.Forms.DataGrid();
            this.slaveDataGroupBox = new System.Windows.Forms.GroupBox();
            this.slaveDataGrid = new System.Windows.Forms.DataGrid();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.synchronizationGroupBox.SuspendLayout();
            this.masterGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.masterMinimumValueNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.masterMaximumValueNumeric)).BeginInit();
            this.slaveGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.slaveMinimumValueNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.slaveMaximumValueNumeric)).BeginInit();
            this.timingGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rateNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerChannelNumeric)).BeginInit();
            this.masterDataGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.masterDataGrid)).BeginInit();
            this.slaveDataGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.slaveDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // synchronizationGroupBox
            // 
            this.synchronizationGroupBox.Controls.Add(this.synchronizationTypeComboBox);
            this.synchronizationGroupBox.Controls.Add(this.synchronizationTypeLabel);
            this.synchronizationGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.synchronizationGroupBox.Location = new System.Drawing.Point(8, 8);
            this.synchronizationGroupBox.Name = "synchronizationGroupBox";
            this.synchronizationGroupBox.Size = new System.Drawing.Size(336, 56);
            this.synchronizationGroupBox.TabIndex = 0;
            this.synchronizationGroupBox.TabStop = false;
            this.synchronizationGroupBox.Text = "Synchronization Parameters";
            // 
            // synchronizationTypeComboBox
            // 
            this.synchronizationTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.synchronizationTypeComboBox.Items.AddRange(new object[] {
                                                                             "E-Series",
                                                                             "M-Series (PCI)",
                                                                             "M-Series (PXI)",
                                                                             "DSA Sample Clock Timebase",
                                                                             "DSA Reference Clock"});
            this.synchronizationTypeComboBox.Location = new System.Drawing.Point(152, 22);
            this.synchronizationTypeComboBox.Name = "synchronizationTypeComboBox";
            this.synchronizationTypeComboBox.Size = new System.Drawing.Size(168, 21);
            this.synchronizationTypeComboBox.TabIndex = 1;
            // 
            // synchronizationTypeLabel
            // 
            this.synchronizationTypeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.synchronizationTypeLabel.Location = new System.Drawing.Point(16, 24);
            this.synchronizationTypeLabel.Name = "synchronizationTypeLabel";
            this.synchronizationTypeLabel.Size = new System.Drawing.Size(120, 16);
            this.synchronizationTypeLabel.TabIndex = 0;
            this.synchronizationTypeLabel.Text = "Synchronization Type:";
            // 
            // masterGroupBox
            // 
            this.masterGroupBox.Controls.Add(this.masterMinimumValueNumeric);
            this.masterGroupBox.Controls.Add(this.masterMaximumValueNumeric);
            this.masterGroupBox.Controls.Add(this.masterPhysicalChannelComboBox);
            this.masterGroupBox.Controls.Add(this.masterMinimumValueLabel);
            this.masterGroupBox.Controls.Add(this.masterMaximumValueLabel);
            this.masterGroupBox.Controls.Add(this.masterPhysicalChannelLabel);
            this.masterGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.masterGroupBox.Location = new System.Drawing.Point(8, 72);
            this.masterGroupBox.Name = "masterGroupBox";
            this.masterGroupBox.Size = new System.Drawing.Size(336, 128);
            this.masterGroupBox.TabIndex = 1;
            this.masterGroupBox.TabStop = false;
            this.masterGroupBox.Text = "Channel Parameters - Master";
            // 
            // masterMinimumValueNumeric
            // 
            this.masterMinimumValueNumeric.DecimalPlaces = 2;
            this.masterMinimumValueNumeric.Location = new System.Drawing.Point(152, 88);
            this.masterMinimumValueNumeric.Minimum = new System.Decimal(new int[] {
                                                                                      100,
                                                                                      0,
                                                                                      0,
                                                                                      -2147483648});
            this.masterMinimumValueNumeric.Name = "masterMinimumValueNumeric";
            this.masterMinimumValueNumeric.Size = new System.Drawing.Size(168, 20);
            this.masterMinimumValueNumeric.TabIndex = 5;
            this.masterMinimumValueNumeric.Value = new System.Decimal(new int[] {
                                                                                    10,
                                                                                    0,
                                                                                    0,
                                                                                    -2147483648});
            // 
            // masterMaximumValueNumeric
            // 
            this.masterMaximumValueNumeric.DecimalPlaces = 2;
            this.masterMaximumValueNumeric.Location = new System.Drawing.Point(152, 56);
            this.masterMaximumValueNumeric.Name = "masterMaximumValueNumeric";
            this.masterMaximumValueNumeric.Size = new System.Drawing.Size(168, 20);
            this.masterMaximumValueNumeric.TabIndex = 3;
            this.masterMaximumValueNumeric.Value = new System.Decimal(new int[] {
                                                                                    10,
                                                                                    0,
                                                                                    0,
                                                                                    0});
            // 
            // masterPhysicalChannelComboBox
            // 
            this.masterPhysicalChannelComboBox.Location = new System.Drawing.Point(152, 24);
            this.masterPhysicalChannelComboBox.Name = "masterPhysicalChannelComboBox";
            this.masterPhysicalChannelComboBox.Size = new System.Drawing.Size(168, 21);
            this.masterPhysicalChannelComboBox.TabIndex = 1;
            this.masterPhysicalChannelComboBox.Text = "Dev1/ai0";
            // 
            // masterMinimumValueLabel
            // 
            this.masterMinimumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.masterMinimumValueLabel.Location = new System.Drawing.Point(16, 88);
            this.masterMinimumValueLabel.Name = "masterMinimumValueLabel";
            this.masterMinimumValueLabel.Size = new System.Drawing.Size(96, 16);
            this.masterMinimumValueLabel.TabIndex = 4;
            this.masterMinimumValueLabel.Text = "Minimum Value:";
            // 
            // masterMaximumValueLabel
            // 
            this.masterMaximumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.masterMaximumValueLabel.Location = new System.Drawing.Point(16, 56);
            this.masterMaximumValueLabel.Name = "masterMaximumValueLabel";
            this.masterMaximumValueLabel.Size = new System.Drawing.Size(96, 16);
            this.masterMaximumValueLabel.TabIndex = 2;
            this.masterMaximumValueLabel.Text = "Maximum Value:";
            // 
            // masterPhysicalChannelLabel
            // 
            this.masterPhysicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.masterPhysicalChannelLabel.Location = new System.Drawing.Point(16, 24);
            this.masterPhysicalChannelLabel.Name = "masterPhysicalChannelLabel";
            this.masterPhysicalChannelLabel.Size = new System.Drawing.Size(96, 16);
            this.masterPhysicalChannelLabel.TabIndex = 0;
            this.masterPhysicalChannelLabel.Text = "Physical Channel:";
            // 
            // slaveGroupBox
            // 
            this.slaveGroupBox.Controls.Add(this.slaveMinimumValueNumeric);
            this.slaveGroupBox.Controls.Add(this.slaveMaximumValueNumeric);
            this.slaveGroupBox.Controls.Add(this.slavePhysicalChannelComboBox);
            this.slaveGroupBox.Controls.Add(this.slavePhysicalChannelLabel);
            this.slaveGroupBox.Controls.Add(this.slaveMaximumValueLabel);
            this.slaveGroupBox.Controls.Add(this.slaveMinimujValueLabel);
            this.slaveGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.slaveGroupBox.Location = new System.Drawing.Point(8, 208);
            this.slaveGroupBox.Name = "slaveGroupBox";
            this.slaveGroupBox.Size = new System.Drawing.Size(336, 128);
            this.slaveGroupBox.TabIndex = 2;
            this.slaveGroupBox.TabStop = false;
            this.slaveGroupBox.Text = "Channel Parameters - Slave";
            // 
            // slaveMinimumValueNumeric
            // 
            this.slaveMinimumValueNumeric.DecimalPlaces = 2;
            this.slaveMinimumValueNumeric.Location = new System.Drawing.Point(152, 88);
            this.slaveMinimumValueNumeric.Minimum = new System.Decimal(new int[] {
                                                                                     100,
                                                                                     0,
                                                                                     0,
                                                                                     -2147483648});
            this.slaveMinimumValueNumeric.Name = "slaveMinimumValueNumeric";
            this.slaveMinimumValueNumeric.Size = new System.Drawing.Size(168, 20);
            this.slaveMinimumValueNumeric.TabIndex = 5;
            this.slaveMinimumValueNumeric.Value = new System.Decimal(new int[] {
                                                                                   10,
                                                                                   0,
                                                                                   0,
                                                                                   -2147483648});
            // 
            // slaveMaximumValueNumeric
            // 
            this.slaveMaximumValueNumeric.DecimalPlaces = 2;
            this.slaveMaximumValueNumeric.Location = new System.Drawing.Point(152, 56);
            this.slaveMaximumValueNumeric.Name = "slaveMaximumValueNumeric";
            this.slaveMaximumValueNumeric.Size = new System.Drawing.Size(168, 20);
            this.slaveMaximumValueNumeric.TabIndex = 3;
            this.slaveMaximumValueNumeric.Value = new System.Decimal(new int[] {
                                                                                   10,
                                                                                   0,
                                                                                   0,
                                                                                   0});
            // 
            // slavePhysicalChannelComboBox
            // 
            this.slavePhysicalChannelComboBox.Location = new System.Drawing.Point(152, 24);
            this.slavePhysicalChannelComboBox.Name = "slavePhysicalChannelComboBox";
            this.slavePhysicalChannelComboBox.Size = new System.Drawing.Size(168, 21);
            this.slavePhysicalChannelComboBox.TabIndex = 1;
            this.slavePhysicalChannelComboBox.Text = "Dev2/ai0";
            // 
            // slavePhysicalChannelLabel
            // 
            this.slavePhysicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.slavePhysicalChannelLabel.Location = new System.Drawing.Point(16, 24);
            this.slavePhysicalChannelLabel.Name = "slavePhysicalChannelLabel";
            this.slavePhysicalChannelLabel.Size = new System.Drawing.Size(96, 16);
            this.slavePhysicalChannelLabel.TabIndex = 0;
            this.slavePhysicalChannelLabel.Text = "Physical Channel:";
            // 
            // slaveMaximumValueLabel
            // 
            this.slaveMaximumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.slaveMaximumValueLabel.Location = new System.Drawing.Point(16, 56);
            this.slaveMaximumValueLabel.Name = "slaveMaximumValueLabel";
            this.slaveMaximumValueLabel.Size = new System.Drawing.Size(96, 16);
            this.slaveMaximumValueLabel.TabIndex = 2;
            this.slaveMaximumValueLabel.Text = "Maximum Value:";
            // 
            // slaveMinimujValueLabel
            // 
            this.slaveMinimujValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.slaveMinimujValueLabel.Location = new System.Drawing.Point(16, 88);
            this.slaveMinimujValueLabel.Name = "slaveMinimujValueLabel";
            this.slaveMinimujValueLabel.Size = new System.Drawing.Size(96, 16);
            this.slaveMinimujValueLabel.TabIndex = 4;
            this.slaveMinimujValueLabel.Text = "Minimum Value:";
            // 
            // timingGroupBox
            // 
            this.timingGroupBox.Controls.Add(this.rateNumeric);
            this.timingGroupBox.Controls.Add(this.samplesPerChannelNumeric);
            this.timingGroupBox.Controls.Add(this.samplesPerChannelLabel);
            this.timingGroupBox.Controls.Add(this.rateLabel);
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
            // masterDataGroupBox
            // 
            this.masterDataGroupBox.Controls.Add(this.masterDataGrid);
            this.masterDataGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.masterDataGroupBox.Location = new System.Drawing.Point(360, 8);
            this.masterDataGroupBox.Name = "masterDataGroupBox";
            this.masterDataGroupBox.Size = new System.Drawing.Size(192, 486);
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
            this.masterDataGrid.Size = new System.Drawing.Size(186, 467);
            this.masterDataGrid.TabIndex = 0;
            // 
            // slaveDataGroupBox
            // 
            this.slaveDataGroupBox.Controls.Add(this.slaveDataGrid);
            this.slaveDataGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.slaveDataGroupBox.Location = new System.Drawing.Point(560, 8);
            this.slaveDataGroupBox.Name = "slaveDataGroupBox";
            this.slaveDataGroupBox.Size = new System.Drawing.Size(192, 486);
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
            this.slaveDataGrid.Size = new System.Drawing.Size(186, 467);
            this.slaveDataGrid.TabIndex = 0;
            // 
            // startButton
            // 
            this.startButton.Enabled = false;
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.startButton.Location = new System.Drawing.Point(72, 471);
            this.startButton.Name = "startButton";
            this.startButton.TabIndex = 4;
            this.startButton.Text = "Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.stopButton.Location = new System.Drawing.Point(200, 471);
            this.stopButton.Name = "stopButton";
            this.stopButton.TabIndex = 5;
            this.stopButton.Text = "Stop";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(762, 504);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.slaveDataGroupBox);
            this.Controls.Add(this.masterDataGroupBox);
            this.Controls.Add(this.timingGroupBox);
            this.Controls.Add(this.slaveGroupBox);
            this.Controls.Add(this.masterGroupBox);
            this.Controls.Add(this.synchronizationGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Multi-Device Sync - Analog Input - Continuous Acquisition";
            this.synchronizationGroupBox.ResumeLayout(false);
            this.masterGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.masterMinimumValueNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.masterMaximumValueNumeric)).EndInit();
            this.slaveGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.slaveMinimumValueNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.slaveMaximumValueNumeric)).EndInit();
            this.timingGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rateNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerChannelNumeric)).EndInit();
            this.masterDataGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.masterDataGrid)).EndInit();
            this.slaveDataGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.slaveDataGrid)).EndInit();
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
            ConfigNumeric(numeric, Decimal.MinValue);
        }

        private void startButton_Click(object sender, System.EventArgs e)
        {
            // Change the mouse to an hourglass for the duration of this function
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                // Note: This example uses the SyncTask helper class.
                // See the SyncTask.cs file in this example for the
                // implementation of the SyncTask class.

                // Create the master and slave tasks
                master = new SyncTask("master", this, synchronizationTypeComboBox.SelectedIndex);
                slave = new SyncTask("slave", this, synchronizationTypeComboBox.SelectedIndex);

                // Configure both tasks with the values selected on the UI.
                master.ConfigureDecimal(masterPhysicalChannelComboBox.Text, masterMinimumValueNumeric.Value,
                    masterMaximumValueNumeric.Value, samplesPerChannelNumeric.Value, rateNumeric.Value);
                slave.ConfigureDecimal(slavePhysicalChannelComboBox.Text, slaveMinimumValueNumeric.Value,
                    slaveMaximumValueNumeric.Value, samplesPerChannelNumeric.Value, rateNumeric.Value);

                // Hook up the data grids to the data tables contained in
                // the SyncTask classes.
                masterDataGrid.DataSource = master.DataTable;
                slaveDataGrid.DataSource = slave.DataTable;
                
                // Synchronize the slave task to the master task.
                // (See SyncTask.cs for details.)
                master.SynchronizeMaster();
                slave.SynchronizeSlave(master);

                StartTasks();

                // Start reading as well
                masterCallback = new AsyncCallback(MasterRead);
                slaveCallback = new AsyncCallback(SlaveRead);
                
                master.BeginRead(masterCallback, master);
                slave.BeginRead(slaveCallback, master);

            }
            catch (DaqException ex)
            {
                // Popup a dialog if an exception is thrown.
                MessageBox.Show(ex.Message);
                // Stop the tasks if an exception is thrown.
                StopTasks();
            }

            Cursor.Current = Cursors.Default;
        }
        private void SlaveRead(IAsyncResult ar)
        {
            try
            {
                if (runningTask != null && runningTask == ar.AsyncState)
                {
                    // Read the data
                    double[,] data = slave.EndRead(ar);

                    // Display the data
                    DisplayData(data, slave.DataTable);

                    // Set up next callback
                    slave.BeginRead(slaveCallback, master);
                }
            }
            catch (DaqException ex)
            {
                // Popup a dialog if an exception is thrown.
                MessageBox.Show(ex.Message);
                // Stop the tasks if an exception is thrown.
                StopTasks();
            }
        }

        private void MasterRead(IAsyncResult ar)
        {
            try
            {
                if (runningTask != null && runningTask == ar.AsyncState)
                {
                    // Read the data
                    double[,] data = master.EndRead(ar);

                    // Display the data
                    DisplayData(data, master.DataTable);

                    // Set up next callback
                    master.BeginRead(masterCallback, master);
                }
            }
            catch (DaqException ex)
            {
                // Popup a dialog if an exception is thrown.
                MessageBox.Show(ex.Message);
                // Stop the tasks if an exception is thrown.
                StopTasks();
            }
        }

        private void stopButton_Click(object sender, System.EventArgs e)
        {
            StopTasks();
        }

        private void DisplayData(double[,] data, DataTable dataTable)
        {
            for(int currentChannelCount = 0; currentChannelCount < dataTable.Columns.Count; currentChannelCount++)
            {
                for(int currentDataCount = 0; currentDataCount < dataTable.Rows.Count; currentDataCount++)
                {
                    dataTable.Rows[currentDataCount][currentChannelCount] = data[currentChannelCount,currentDataCount];
                }
            }
        }

        private void StartTasks()
        {
            if (runningTask == null)
            {
                // Change State
                runningTask = master;

                startButton.Enabled = false;
                stopButton.Enabled = true;
            }

            // Start both tasks.
            // Note: Start the slave task first because it is waiting on
            // the master task.
            slave.Start();
            master.Start();
        }

        private void StopTasks()
        {
            // Change state
            runningTask = null;

            startButton.Enabled = true;
            stopButton.Enabled = false;
            
            // Dispose Tasks
            slave.Dispose();
            master.Dispose();
        }
    }
}
