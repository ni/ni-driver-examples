/******************************************************************************
*
* Example program:
*   AIAOShardTimebaseAndTrig_DSA
*
* Category:
*   Synchronization
*
* Description:
*   This example synchronizes the clocks and trigger on two Dynamic Signal
*   Acquistion (DSA) 
*   devices and performs continuous analog input and output.  NOTE: This example
*   is intended to show low 
*   level synchronization of various devices. DSA and S Series devices now support
*   including channels from 
*   multiple devices in a single task. DAQmx automatically synchronizes the
*   devices in such a task. See the 
*   DAQmx Help>>NI-DAQmx Device Considerations>>Multidevice Tasks
*   section for further details.NOTE: 
*   If you are using PXI DSA devices along with sample clock timebase
*   synchronization, the master device 
*   must reside in PXI slot 2.NOTE: This code will not run "as-is" on a
*   multifunction (MIO) DAQ device.
*
* Instructions for running:
*   1.  Select which type of Syncrhonization method you want to use.
*   2.  Select the physical channel to correspond to where your signal is input
*       and output on the DSA devices.
*   3.  Enter the minimum and maximum voltage range.Note: For better accuracy
*       try to match the input range to the expected voltage level of the
*       measured signal.
*   4.  Set the number of samples to acquire per channel.
*   5.  Set the rate of the acquisition.Note: The rate should be at least twice
*       as fast as the maximum frequency component of the signal being acquired.
*
* Steps:
*   1.  Create analog input and output voltage channels for both the Master and
*       Slave devices.
*   2.  Set timing parameters for continuous generation and acquisition.  The
*       sample rate and number of samples are set to the same values for each
*       device.
*   3.  PXI DSA devices require two timing parameters to be shared between the
*       two devices.  The first signal is the "Master Clock Timebase", shared
*       across the PXI_Star bus.  The second signal is the Sync Pulse.  This
*       signal is shared across the PXI_Trig / RTSI bus.  Use the Timing
*       subobject to configure these signals on the slave.  Prefix the signal
*       strings with the name of the Master Device.  In this example, the name
*       of the Master Device is determined from the physical channel names of
*       the Master Input task.
*   4.  Configure a digital start trigger on the slave tasks and master output
*       task to use the acquisition start signal from the master input task. 
*       Prefix the signal name with the name of the Master Device, as before. 
*       The signal is shared along the PXI_Trig / RTSI bus.
*   5.  Call Task.Start() to start the acquisition.Note: The slave tasks and
*       master output task start before the master input task because these
*       tasks are waiting on the master input task for the synchronization
*       pulse.
*   6.  Read data from the input tasks and display it.
*   7.  Dispose the Task object to clean-up any resources associated with the
*       task.
*   8.  Handle any DaqExceptions, if they occur.
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
*   Make sure your signal input terminals match the Physical Channel I/O
*   control.  Ensure that your PXI chassis has been properly identified in MAX.
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

namespace NationalInstruments.Examples.MultiDeviceSync_AIAOShardTimebaseAndTrig_DSA
{
    /// <summary>
    /// Summary description for MainForm.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        private SyncTaskReader masterIn = null;
        private SyncTaskWriter masterOut = null;
        private SyncTaskReader slaveIn = null;
        private SyncTaskWriter slaveOut = null;
        private DataTable masterDataTable = null;
        private DataTable slaveDataTable = null;
        private DataColumn[] masterDataColumns = null;
        private DataColumn[] slaveDataColumns = null;
        private AsyncCallback masterCallback;
        private AsyncCallback slaveCallback;
        private double[] output;
        private SyncTaskReader runningTask;

        private System.Windows.Forms.Label masterMaxValLabel;
        private System.Windows.Forms.Label masterMinValLabel;
        private System.Windows.Forms.NumericUpDown masterMinValNumeric;
        private System.Windows.Forms.NumericUpDown masterMaxValNumeric;
        private System.Windows.Forms.GroupBox timingGroupBox;
        private System.Windows.Forms.NumericUpDown rateNumeric;
        private System.Windows.Forms.Label samplesPerChannelLabel;
        private System.Windows.Forms.Label rateLabel;
        private System.Windows.Forms.NumericUpDown samplesPerChannelNumeric;
        private System.Windows.Forms.GroupBox masterChanGroupBox;
        private System.Windows.Forms.Label masterInputChanLabel;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Label masterOutputChanLabel;
        private System.Windows.Forms.GroupBox slaveChanGroupBox;
        private System.Windows.Forms.NumericUpDown slaveMinValNumeric;
        private System.Windows.Forms.Label slaveInputChanLabel;
        private System.Windows.Forms.Label slaveMaxValLabel;
        private System.Windows.Forms.Label slaveMinValLabel;
        private System.Windows.Forms.NumericUpDown slaveMaxValNumeric;
        private System.Windows.Forms.Label slaveOutputChanLabel;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.NumericUpDown amplitudeNumeric;
        private System.Windows.Forms.Label waveformTypeLabel;
        private System.Windows.Forms.Label amplitudeLabel;
        private System.Windows.Forms.GroupBox waveformGroupBox;
        private System.Windows.Forms.ComboBox waveformTypeComboBox;
        private System.Windows.Forms.Label samplesPerBufferLabel;
        private System.Windows.Forms.Label cyclesPerBufferLabel;
        private System.Windows.Forms.NumericUpDown samplesPerBufferNumeric;
        private System.Windows.Forms.NumericUpDown cyclesPerBufferNumeric;
        private System.Windows.Forms.GroupBox masterDataGroupBox;
        private System.Windows.Forms.GroupBox slaveDataGroupBox;
        private System.Windows.Forms.DataGrid masterDataGrid;
        private System.Windows.Forms.DataGrid slaveDataGrid;
        private System.Windows.Forms.ComboBox masterInputChannelComboBox;
        private System.Windows.Forms.ComboBox masterOutputChannelComboBox;
        private System.Windows.Forms.ComboBox slaveInputChannelComboBox;
        private System.Windows.Forms.ComboBox slaveOutputChannelComboBox;
        private ComboBox syncTypeComboBox;
        private Label syncTypeLabel;
        private GroupBox syncGroupBox;
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

            // Initialize UI
            masterInputChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AI, PhysicalChannelAccess.External));
            masterOutputChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AO, PhysicalChannelAccess.External));
            slaveInputChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AI, PhysicalChannelAccess.External));
            slaveOutputChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AO, PhysicalChannelAccess.External));

            ConfigNumeric(masterMaxValNumeric);
            ConfigNumeric(masterMinValNumeric);
            ConfigNumeric(slaveMaxValNumeric);
            ConfigNumeric(slaveMinValNumeric);
            ConfigNumeric(samplesPerChannelNumeric, Decimal.Zero);
            ConfigNumeric(rateNumeric, Decimal.Zero);

            waveformTypeComboBox.SelectedIndex = 0;
            syncTypeComboBox.SelectedIndex = 0;

            // Set up the AI data tables
            masterDataTable = new DataTable();
            slaveDataTable = new DataTable();

            InitializeDataTables(28); 
            masterDataGrid.DataSource = masterDataTable;
            slaveDataGrid.DataSource = slaveDataTable;
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
                if (masterIn != null)
                {
                    runningTask = null;
                    masterIn.Dispose();
                }
                if (slaveIn != null)
                {
                    slaveIn.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.masterChanGroupBox = new System.Windows.Forms.GroupBox();
            this.masterOutputChannelComboBox = new System.Windows.Forms.ComboBox();
            this.masterInputChannelComboBox = new System.Windows.Forms.ComboBox();
            this.masterMinValNumeric = new System.Windows.Forms.NumericUpDown();
            this.masterInputChanLabel = new System.Windows.Forms.Label();
            this.masterMaxValLabel = new System.Windows.Forms.Label();
            this.masterMinValLabel = new System.Windows.Forms.Label();
            this.masterMaxValNumeric = new System.Windows.Forms.NumericUpDown();
            this.masterOutputChanLabel = new System.Windows.Forms.Label();
            this.timingGroupBox = new System.Windows.Forms.GroupBox();
            this.rateNumeric = new System.Windows.Forms.NumericUpDown();
            this.samplesPerChannelLabel = new System.Windows.Forms.Label();
            this.rateLabel = new System.Windows.Forms.Label();
            this.samplesPerChannelNumeric = new System.Windows.Forms.NumericUpDown();
            this.startButton = new System.Windows.Forms.Button();
            this.slaveChanGroupBox = new System.Windows.Forms.GroupBox();
            this.slaveOutputChannelComboBox = new System.Windows.Forms.ComboBox();
            this.slaveInputChannelComboBox = new System.Windows.Forms.ComboBox();
            this.slaveMinValNumeric = new System.Windows.Forms.NumericUpDown();
            this.slaveInputChanLabel = new System.Windows.Forms.Label();
            this.slaveMaxValLabel = new System.Windows.Forms.Label();
            this.slaveMinValLabel = new System.Windows.Forms.Label();
            this.slaveMaxValNumeric = new System.Windows.Forms.NumericUpDown();
            this.slaveOutputChanLabel = new System.Windows.Forms.Label();
            this.stopButton = new System.Windows.Forms.Button();
            this.waveformGroupBox = new System.Windows.Forms.GroupBox();
            this.waveformTypeComboBox = new System.Windows.Forms.ComboBox();
            this.amplitudeNumeric = new System.Windows.Forms.NumericUpDown();
            this.waveformTypeLabel = new System.Windows.Forms.Label();
            this.amplitudeLabel = new System.Windows.Forms.Label();
            this.samplesPerBufferLabel = new System.Windows.Forms.Label();
            this.samplesPerBufferNumeric = new System.Windows.Forms.NumericUpDown();
            this.cyclesPerBufferNumeric = new System.Windows.Forms.NumericUpDown();
            this.cyclesPerBufferLabel = new System.Windows.Forms.Label();
            this.masterDataGroupBox = new System.Windows.Forms.GroupBox();
            this.masterDataGrid = new System.Windows.Forms.DataGrid();
            this.slaveDataGroupBox = new System.Windows.Forms.GroupBox();
            this.slaveDataGrid = new System.Windows.Forms.DataGrid();
            this.syncTypeComboBox = new System.Windows.Forms.ComboBox();
            this.syncTypeLabel = new System.Windows.Forms.Label();
            this.syncGroupBox = new System.Windows.Forms.GroupBox();
            this.masterChanGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.masterMinValNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.masterMaxValNumeric)).BeginInit();
            this.timingGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rateNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerChannelNumeric)).BeginInit();
            this.slaveChanGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.slaveMinValNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.slaveMaxValNumeric)).BeginInit();
            this.waveformGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.amplitudeNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerBufferNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cyclesPerBufferNumeric)).BeginInit();
            this.masterDataGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.masterDataGrid)).BeginInit();
            this.slaveDataGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.slaveDataGrid)).BeginInit();
            this.syncGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // masterChanGroupBox
            // 
            this.masterChanGroupBox.Controls.Add(this.masterOutputChannelComboBox);
            this.masterChanGroupBox.Controls.Add(this.masterInputChannelComboBox);
            this.masterChanGroupBox.Controls.Add(this.masterMinValNumeric);
            this.masterChanGroupBox.Controls.Add(this.masterInputChanLabel);
            this.masterChanGroupBox.Controls.Add(this.masterMaxValLabel);
            this.masterChanGroupBox.Controls.Add(this.masterMinValLabel);
            this.masterChanGroupBox.Controls.Add(this.masterMaxValNumeric);
            this.masterChanGroupBox.Controls.Add(this.masterOutputChanLabel);
            this.masterChanGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.masterChanGroupBox.Location = new System.Drawing.Point(8, 77);
            this.masterChanGroupBox.Name = "masterChanGroupBox";
            this.masterChanGroupBox.Size = new System.Drawing.Size(336, 119);
            this.masterChanGroupBox.TabIndex = 0;
            this.masterChanGroupBox.TabStop = false;
            this.masterChanGroupBox.Text = "Channel Parameters - Master";
            // 
            // masterOutputChannelComboBox
            // 
            this.masterOutputChannelComboBox.Location = new System.Drawing.Point(152, 39);
            this.masterOutputChannelComboBox.Name = "masterOutputChannelComboBox";
            this.masterOutputChannelComboBox.Size = new System.Drawing.Size(168, 21);
            this.masterOutputChannelComboBox.TabIndex = 3;
            this.masterOutputChannelComboBox.Text = "Dev2/ao0";
            // 
            // masterInputChannelComboBox
            // 
            this.masterInputChannelComboBox.Location = new System.Drawing.Point(152, 15);
            this.masterInputChannelComboBox.Name = "masterInputChannelComboBox";
            this.masterInputChannelComboBox.Size = new System.Drawing.Size(168, 21);
            this.masterInputChannelComboBox.TabIndex = 1;
            this.masterInputChannelComboBox.Text = "Dev2/ai0";
            // 
            // masterMinValNumeric
            // 
            this.masterMinValNumeric.DecimalPlaces = 2;
            this.masterMinValNumeric.Location = new System.Drawing.Point(152, 93);
            this.masterMinValNumeric.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.masterMinValNumeric.Name = "masterMinValNumeric";
            this.masterMinValNumeric.Size = new System.Drawing.Size(168, 20);
            this.masterMinValNumeric.TabIndex = 7;
            this.masterMinValNumeric.Value = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            // 
            // masterInputChanLabel
            // 
            this.masterInputChanLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.masterInputChanLabel.Location = new System.Drawing.Point(16, 17);
            this.masterInputChanLabel.Name = "masterInputChanLabel";
            this.masterInputChanLabel.Size = new System.Drawing.Size(96, 16);
            this.masterInputChanLabel.TabIndex = 0;
            this.masterInputChanLabel.Text = "Input Channel:";
            // 
            // masterMaxValLabel
            // 
            this.masterMaxValLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.masterMaxValLabel.Location = new System.Drawing.Point(16, 69);
            this.masterMaxValLabel.Name = "masterMaxValLabel";
            this.masterMaxValLabel.Size = new System.Drawing.Size(96, 16);
            this.masterMaxValLabel.TabIndex = 4;
            this.masterMaxValLabel.Text = "Maximum Value:";
            // 
            // masterMinValLabel
            // 
            this.masterMinValLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.masterMinValLabel.Location = new System.Drawing.Point(16, 95);
            this.masterMinValLabel.Name = "masterMinValLabel";
            this.masterMinValLabel.Size = new System.Drawing.Size(96, 16);
            this.masterMinValLabel.TabIndex = 6;
            this.masterMinValLabel.Text = "Minimum Value:";
            // 
            // masterMaxValNumeric
            // 
            this.masterMaxValNumeric.DecimalPlaces = 2;
            this.masterMaxValNumeric.Location = new System.Drawing.Point(152, 67);
            this.masterMaxValNumeric.Name = "masterMaxValNumeric";
            this.masterMaxValNumeric.Size = new System.Drawing.Size(168, 20);
            this.masterMaxValNumeric.TabIndex = 5;
            this.masterMaxValNumeric.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // masterOutputChanLabel
            // 
            this.masterOutputChanLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.masterOutputChanLabel.Location = new System.Drawing.Point(16, 43);
            this.masterOutputChanLabel.Name = "masterOutputChanLabel";
            this.masterOutputChanLabel.Size = new System.Drawing.Size(96, 16);
            this.masterOutputChanLabel.TabIndex = 2;
            this.masterOutputChanLabel.Text = "Output Channel:";
            // 
            // timingGroupBox
            // 
            this.timingGroupBox.Controls.Add(this.rateNumeric);
            this.timingGroupBox.Controls.Add(this.samplesPerChannelLabel);
            this.timingGroupBox.Controls.Add(this.rateLabel);
            this.timingGroupBox.Controls.Add(this.samplesPerChannelNumeric);
            this.timingGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.timingGroupBox.Location = new System.Drawing.Point(8, 327);
            this.timingGroupBox.Name = "timingGroupBox";
            this.timingGroupBox.Size = new System.Drawing.Size(336, 73);
            this.timingGroupBox.TabIndex = 2;
            this.timingGroupBox.TabStop = false;
            this.timingGroupBox.Text = "Timing Parameters";
            // 
            // rateNumeric
            // 
            this.rateNumeric.DecimalPlaces = 2;
            this.rateNumeric.Location = new System.Drawing.Point(152, 43);
            this.rateNumeric.Maximum = new decimal(new int[] {
            102400,
            0,
            0,
            0});
            this.rateNumeric.Name = "rateNumeric";
            this.rateNumeric.Size = new System.Drawing.Size(168, 20);
            this.rateNumeric.TabIndex = 3;
            this.rateNumeric.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // samplesPerChannelLabel
            // 
            this.samplesPerChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.samplesPerChannelLabel.Location = new System.Drawing.Point(16, 19);
            this.samplesPerChannelLabel.Name = "samplesPerChannelLabel";
            this.samplesPerChannelLabel.Size = new System.Drawing.Size(120, 16);
            this.samplesPerChannelLabel.TabIndex = 0;
            this.samplesPerChannelLabel.Text = "Samples Per Channel:";
            // 
            // rateLabel
            // 
            this.rateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rateLabel.Location = new System.Drawing.Point(16, 45);
            this.rateLabel.Name = "rateLabel";
            this.rateLabel.Size = new System.Drawing.Size(96, 16);
            this.rateLabel.TabIndex = 2;
            this.rateLabel.Text = "Rate (Hz):";
            // 
            // samplesPerChannelNumeric
            // 
            this.samplesPerChannelNumeric.Location = new System.Drawing.Point(152, 17);
            this.samplesPerChannelNumeric.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.samplesPerChannelNumeric.Name = "samplesPerChannelNumeric";
            this.samplesPerChannelNumeric.Size = new System.Drawing.Size(168, 20);
            this.samplesPerChannelNumeric.TabIndex = 1;
            this.samplesPerChannelNumeric.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // startButton
            // 
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.startButton.Location = new System.Drawing.Point(64, 513);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 6;
            this.startButton.Text = "Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // slaveChanGroupBox
            // 
            this.slaveChanGroupBox.Controls.Add(this.slaveOutputChannelComboBox);
            this.slaveChanGroupBox.Controls.Add(this.slaveInputChannelComboBox);
            this.slaveChanGroupBox.Controls.Add(this.slaveMinValNumeric);
            this.slaveChanGroupBox.Controls.Add(this.slaveInputChanLabel);
            this.slaveChanGroupBox.Controls.Add(this.slaveMaxValLabel);
            this.slaveChanGroupBox.Controls.Add(this.slaveMinValLabel);
            this.slaveChanGroupBox.Controls.Add(this.slaveMaxValNumeric);
            this.slaveChanGroupBox.Controls.Add(this.slaveOutputChanLabel);
            this.slaveChanGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.slaveChanGroupBox.Location = new System.Drawing.Point(8, 202);
            this.slaveChanGroupBox.Name = "slaveChanGroupBox";
            this.slaveChanGroupBox.Size = new System.Drawing.Size(336, 120);
            this.slaveChanGroupBox.TabIndex = 1;
            this.slaveChanGroupBox.TabStop = false;
            this.slaveChanGroupBox.Text = "Channel Parameters - Slave";
            // 
            // slaveOutputChannelComboBox
            // 
            this.slaveOutputChannelComboBox.Location = new System.Drawing.Point(152, 39);
            this.slaveOutputChannelComboBox.Name = "slaveOutputChannelComboBox";
            this.slaveOutputChannelComboBox.Size = new System.Drawing.Size(168, 21);
            this.slaveOutputChannelComboBox.TabIndex = 3;
            this.slaveOutputChannelComboBox.Text = "Dev3/ao0";
            // 
            // slaveInputChannelComboBox
            // 
            this.slaveInputChannelComboBox.Location = new System.Drawing.Point(152, 15);
            this.slaveInputChannelComboBox.Name = "slaveInputChannelComboBox";
            this.slaveInputChannelComboBox.Size = new System.Drawing.Size(168, 21);
            this.slaveInputChannelComboBox.TabIndex = 1;
            this.slaveInputChannelComboBox.Text = "Dev3/ai0";
            // 
            // slaveMinValNumeric
            // 
            this.slaveMinValNumeric.DecimalPlaces = 2;
            this.slaveMinValNumeric.Location = new System.Drawing.Point(152, 93);
            this.slaveMinValNumeric.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.slaveMinValNumeric.Name = "slaveMinValNumeric";
            this.slaveMinValNumeric.Size = new System.Drawing.Size(168, 20);
            this.slaveMinValNumeric.TabIndex = 7;
            this.slaveMinValNumeric.Value = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            // 
            // slaveInputChanLabel
            // 
            this.slaveInputChanLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.slaveInputChanLabel.Location = new System.Drawing.Point(16, 17);
            this.slaveInputChanLabel.Name = "slaveInputChanLabel";
            this.slaveInputChanLabel.Size = new System.Drawing.Size(96, 16);
            this.slaveInputChanLabel.TabIndex = 0;
            this.slaveInputChanLabel.Text = "Input Channel:";
            // 
            // slaveMaxValLabel
            // 
            this.slaveMaxValLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.slaveMaxValLabel.Location = new System.Drawing.Point(16, 69);
            this.slaveMaxValLabel.Name = "slaveMaxValLabel";
            this.slaveMaxValLabel.Size = new System.Drawing.Size(96, 16);
            this.slaveMaxValLabel.TabIndex = 4;
            this.slaveMaxValLabel.Text = "Maximum Value:";
            // 
            // slaveMinValLabel
            // 
            this.slaveMinValLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.slaveMinValLabel.Location = new System.Drawing.Point(16, 95);
            this.slaveMinValLabel.Name = "slaveMinValLabel";
            this.slaveMinValLabel.Size = new System.Drawing.Size(96, 16);
            this.slaveMinValLabel.TabIndex = 6;
            this.slaveMinValLabel.Text = "Minimum Value:";
            // 
            // slaveMaxValNumeric
            // 
            this.slaveMaxValNumeric.DecimalPlaces = 2;
            this.slaveMaxValNumeric.Location = new System.Drawing.Point(152, 69);
            this.slaveMaxValNumeric.Name = "slaveMaxValNumeric";
            this.slaveMaxValNumeric.Size = new System.Drawing.Size(168, 20);
            this.slaveMaxValNumeric.TabIndex = 5;
            this.slaveMaxValNumeric.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // slaveOutputChanLabel
            // 
            this.slaveOutputChanLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.slaveOutputChanLabel.Location = new System.Drawing.Point(16, 43);
            this.slaveOutputChanLabel.Name = "slaveOutputChanLabel";
            this.slaveOutputChanLabel.Size = new System.Drawing.Size(96, 16);
            this.slaveOutputChanLabel.TabIndex = 2;
            this.slaveOutputChanLabel.Text = "Output Channel:";
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.stopButton.Location = new System.Drawing.Point(184, 513);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 7;
            this.stopButton.Text = "Stop";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // waveformGroupBox
            // 
            this.waveformGroupBox.Controls.Add(this.waveformTypeComboBox);
            this.waveformGroupBox.Controls.Add(this.amplitudeNumeric);
            this.waveformGroupBox.Controls.Add(this.waveformTypeLabel);
            this.waveformGroupBox.Controls.Add(this.amplitudeLabel);
            this.waveformGroupBox.Controls.Add(this.samplesPerBufferLabel);
            this.waveformGroupBox.Controls.Add(this.samplesPerBufferNumeric);
            this.waveformGroupBox.Controls.Add(this.cyclesPerBufferNumeric);
            this.waveformGroupBox.Controls.Add(this.cyclesPerBufferLabel);
            this.waveformGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.waveformGroupBox.Location = new System.Drawing.Point(8, 406);
            this.waveformGroupBox.Name = "waveformGroupBox";
            this.waveformGroupBox.Size = new System.Drawing.Size(336, 100);
            this.waveformGroupBox.TabIndex = 3;
            this.waveformGroupBox.TabStop = false;
            this.waveformGroupBox.Text = "Waveform Parameters";
            // 
            // waveformTypeComboBox
            // 
            this.waveformTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.waveformTypeComboBox.Items.AddRange(new object[] {
            "Sine"});
            this.waveformTypeComboBox.Location = new System.Drawing.Point(152, 20);
            this.waveformTypeComboBox.Name = "waveformTypeComboBox";
            this.waveformTypeComboBox.Size = new System.Drawing.Size(168, 21);
            this.waveformTypeComboBox.TabIndex = 1;
            // 
            // amplitudeNumeric
            // 
            this.amplitudeNumeric.DecimalPlaces = 3;
            this.amplitudeNumeric.Location = new System.Drawing.Point(152, 46);
            this.amplitudeNumeric.Maximum = new decimal(new int[] {
            102400,
            0,
            0,
            0});
            this.amplitudeNumeric.Name = "amplitudeNumeric";
            this.amplitudeNumeric.Size = new System.Drawing.Size(168, 20);
            this.amplitudeNumeric.TabIndex = 3;
            this.amplitudeNumeric.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // waveformTypeLabel
            // 
            this.waveformTypeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.waveformTypeLabel.Location = new System.Drawing.Point(16, 22);
            this.waveformTypeLabel.Name = "waveformTypeLabel";
            this.waveformTypeLabel.Size = new System.Drawing.Size(96, 16);
            this.waveformTypeLabel.TabIndex = 0;
            this.waveformTypeLabel.Text = "Waveform Type:";
            // 
            // amplitudeLabel
            // 
            this.amplitudeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.amplitudeLabel.Location = new System.Drawing.Point(16, 48);
            this.amplitudeLabel.Name = "amplitudeLabel";
            this.amplitudeLabel.Size = new System.Drawing.Size(96, 16);
            this.amplitudeLabel.TabIndex = 2;
            this.amplitudeLabel.Text = "Amplitude:";
            // 
            // samplesPerBufferLabel
            // 
            this.samplesPerBufferLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.samplesPerBufferLabel.Location = new System.Drawing.Point(16, 74);
            this.samplesPerBufferLabel.Name = "samplesPerBufferLabel";
            this.samplesPerBufferLabel.Size = new System.Drawing.Size(104, 16);
            this.samplesPerBufferLabel.TabIndex = 4;
            this.samplesPerBufferLabel.Text = "Samples per Buffer:";
            // 
            // samplesPerBufferNumeric
            // 
            this.samplesPerBufferNumeric.Location = new System.Drawing.Point(152, 72);
            this.samplesPerBufferNumeric.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.samplesPerBufferNumeric.Name = "samplesPerBufferNumeric";
            this.samplesPerBufferNumeric.Size = new System.Drawing.Size(168, 20);
            this.samplesPerBufferNumeric.TabIndex = 5;
            this.samplesPerBufferNumeric.Value = new decimal(new int[] {
            250,
            0,
            0,
            0});
            // 
            // cyclesPerBufferNumeric
            // 
            this.cyclesPerBufferNumeric.DecimalPlaces = 2;
            this.cyclesPerBufferNumeric.Location = new System.Drawing.Point(152, 122);
            this.cyclesPerBufferNumeric.Maximum = new decimal(new int[] {
            102400,
            0,
            0,
            0});
            this.cyclesPerBufferNumeric.Name = "cyclesPerBufferNumeric";
            this.cyclesPerBufferNumeric.Size = new System.Drawing.Size(168, 20);
            this.cyclesPerBufferNumeric.TabIndex = 7;
            this.cyclesPerBufferNumeric.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // cyclesPerBufferLabel
            // 
            this.cyclesPerBufferLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cyclesPerBufferLabel.Location = new System.Drawing.Point(16, 124);
            this.cyclesPerBufferLabel.Name = "cyclesPerBufferLabel";
            this.cyclesPerBufferLabel.Size = new System.Drawing.Size(96, 16);
            this.cyclesPerBufferLabel.TabIndex = 6;
            this.cyclesPerBufferLabel.Text = "Cycles per Buffer:";
            // 
            // masterDataGroupBox
            // 
            this.masterDataGroupBox.Controls.Add(this.masterDataGrid);
            this.masterDataGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.masterDataGroupBox.Location = new System.Drawing.Point(344, 12);
            this.masterDataGroupBox.Name = "masterDataGroupBox";
            this.masterDataGroupBox.Size = new System.Drawing.Size(208, 524);
            this.masterDataGroupBox.TabIndex = 4;
            this.masterDataGroupBox.TabStop = false;
            this.masterDataGroupBox.Text = "Master Data";
            // 
            // masterDataGrid
            // 
            this.masterDataGrid.AllowSorting = false;
            this.masterDataGrid.DataMember = "";
            this.masterDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.masterDataGrid.Location = new System.Drawing.Point(3, 19);
            this.masterDataGrid.Name = "masterDataGrid";
            this.masterDataGrid.PreferredColumnWidth = 150;
            this.masterDataGrid.ReadOnly = true;
            this.masterDataGrid.Size = new System.Drawing.Size(205, 508);
            this.masterDataGrid.TabIndex = 0;
            // 
            // slaveDataGroupBox
            // 
            this.slaveDataGroupBox.Controls.Add(this.slaveDataGrid);
            this.slaveDataGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.slaveDataGroupBox.Location = new System.Drawing.Point(552, 12);
            this.slaveDataGroupBox.Name = "slaveDataGroupBox";
            this.slaveDataGroupBox.Size = new System.Drawing.Size(208, 524);
            this.slaveDataGroupBox.TabIndex = 5;
            this.slaveDataGroupBox.TabStop = false;
            this.slaveDataGroupBox.Text = "Slave Data";
            // 
            // slaveDataGrid
            // 
            this.slaveDataGrid.AllowSorting = false;
            this.slaveDataGrid.DataMember = "";
            this.slaveDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.slaveDataGrid.Location = new System.Drawing.Point(3, 19);
            this.slaveDataGrid.Name = "slaveDataGrid";
            this.slaveDataGrid.PreferredColumnWidth = 150;
            this.slaveDataGrid.ReadOnly = true;
            this.slaveDataGrid.Size = new System.Drawing.Size(205, 507);
            this.slaveDataGrid.TabIndex = 0;
            // 
            // syncTypeComboBox
            // 
            this.syncTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.syncTypeComboBox.Items.AddRange(new object[] {
            "DSA Sample Clock Timebase",
            "DSA Reference Clock"});
            this.syncTypeComboBox.Location = new System.Drawing.Point(152, 22);
            this.syncTypeComboBox.Name = "syncTypeComboBox";
            this.syncTypeComboBox.Size = new System.Drawing.Size(168, 21);
            this.syncTypeComboBox.TabIndex = 1;
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
            // syncGroupBox
            // 
            this.syncGroupBox.Controls.Add(this.syncTypeLabel);
            this.syncGroupBox.Controls.Add(this.syncTypeComboBox);
            this.syncGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.syncGroupBox.Location = new System.Drawing.Point(8, 12);
            this.syncGroupBox.Name = "syncGroupBox";
            this.syncGroupBox.Size = new System.Drawing.Size(336, 56);
            this.syncGroupBox.TabIndex = 8;
            this.syncGroupBox.TabStop = false;
            this.syncGroupBox.Text = "Synchronization Parameters";
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(760, 540);
            this.Controls.Add(this.syncGroupBox);
            this.Controls.Add(this.masterDataGroupBox);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.masterChanGroupBox);
            this.Controls.Add(this.timingGroupBox);
            this.Controls.Add(this.slaveChanGroupBox);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.waveformGroupBox);
            this.Controls.Add(this.slaveDataGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "MultiDevice Sync - AI and AO Shared Timebase And Trigger - DSA";
            this.masterChanGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.masterMinValNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.masterMaxValNumeric)).EndInit();
            this.timingGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rateNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerChannelNumeric)).EndInit();
            this.slaveChanGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.slaveMinValNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.slaveMaxValNumeric)).EndInit();
            this.waveformGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.amplitudeNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerBufferNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cyclesPerBufferNumeric)).EndInit();
            this.masterDataGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.masterDataGrid)).EndInit();
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
            ConfigNumeric(numeric, Decimal.MinValue);
        }

        private void InitializeDataTables(int rows)
        {
            // Clear out the data
            masterDataTable.Rows.Clear();
            slaveDataTable.Rows.Clear();

            masterDataTable.Columns.Clear();
            slaveDataTable.Columns.Clear();

            // Add one column to each DataGrid of type double
            masterDataColumns = new DataColumn[1];
            slaveDataColumns = new DataColumn[1];

            masterDataColumns[0] = new DataColumn();
            masterDataColumns[0].DataType = typeof(double);
            masterDataColumns[0].ColumnName = "Master Data";

            slaveDataColumns[0] = new DataColumn();
            slaveDataColumns[0].DataType = typeof(double);
            slaveDataColumns[0].ColumnName = "Slave Data";

            masterDataTable.Columns.AddRange(masterDataColumns);
            slaveDataTable.Columns.AddRange(slaveDataColumns);

            // Now add a certain number of rows
            for(int i = 0; i < rows; i++)             
            {
                object[] rowArr = new object[1];
                masterDataTable.Rows.Add(rowArr);
                slaveDataTable.Rows.Add(rowArr);
            }
        }


        private void startButton_Click(object sender, System.EventArgs e)
        {
            // Change the mouse to an hourglass for the duration of this function.
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                // Note: This example uses the SyncTask helper class.
                // See the SyncTask.cs file in this example for the
                // implementation of the SyncTask class.

                // Create the master and slave tasks
                masterIn = new SyncTaskReader("masterIn", this, syncTypeComboBox.SelectedIndex);
                masterOut = new SyncTaskWriter("masterOut", this, syncTypeComboBox.SelectedIndex);
                slaveIn = new SyncTaskReader("slaveIn", this, syncTypeComboBox.SelectedIndex);
                slaveOut = new SyncTaskWriter("slaveOut", this, syncTypeComboBox.SelectedIndex);

                // Configure all four tasks with the values selected on the UI.
                masterIn.ConfigureDecimal(masterInputChannelComboBox.Text, masterMinValNumeric.Value,
                    masterMaxValNumeric.Value, samplesPerChannelNumeric.Value, rateNumeric.Value);
                masterOut.ConfigureDecimal(masterOutputChannelComboBox.Text, masterMinValNumeric.Value,
                    masterMaxValNumeric.Value, samplesPerChannelNumeric.Value, rateNumeric.Value);

                slaveIn.ConfigureDecimal(slaveInputChannelComboBox.Text, slaveMinValNumeric.Value,
                    slaveMaxValNumeric.Value, samplesPerChannelNumeric.Value, rateNumeric.Value);
                slaveOut.ConfigureDecimal(slaveOutputChannelComboBox.Text, slaveMinValNumeric.Value,
                    slaveMaxValNumeric.Value, samplesPerChannelNumeric.Value, rateNumeric.Value);

                // Synchronize the slave tasks to the master tasks.
                // (See SyncTask*.cs for details.)
                slaveIn.SynchronizeSlave(masterIn);
                slaveOut.SynchronizeSlave(masterIn);
                masterOut.SynchronizeSlave(masterIn);

                // Write data to each output channel
                FunctionGenerator fGen = new FunctionGenerator(rateNumeric.Value.ToString(),
                    samplesPerBufferNumeric.Value.ToString(), cyclesPerBufferNumeric.Value.ToString(),
                    waveformTypeComboBox.Text, amplitudeNumeric.Value.ToString());

                output = fGen.Data;

                masterOut.BeginWrite(output);
                slaveOut.BeginWrite(output);

                // Officially start the task
                StartTask();

                slaveIn.Start();
                slaveOut.Start();
                masterOut.Start();
                masterIn.Start();

                // Start reading as well
                masterCallback = new AsyncCallback(MasterRead);
                slaveCallback = new AsyncCallback(SlaveRead);

                masterIn.BeginRead(masterCallback, masterIn);
                slaveIn.BeginRead(slaveCallback, masterIn);
            }
            catch (Exception ex)
            {
                StopTask();
                MessageBox.Show(ex.Message);
            }
        }

        private void SlaveRead(IAsyncResult ar)
        {
            try
            {
                if (runningTask != null && runningTask == ar.AsyncState)
                {
                    // Read the data
                    double[] data = slaveIn.EndRead(ar);

                    // Display the data
                    for (int i = 0; i < slaveDataTable.Rows.Count && i < data.Length; i++)
                    {
                        slaveDataTable.Rows[i][0] = data[i];
                    }

                    // Set up next callback
                    slaveIn.BeginRead(slaveCallback, masterIn);
                }
            }
            catch (Exception ex)
            {
                StopTask();
                MessageBox.Show(ex.Message);
            }
        }

        private void MasterRead(IAsyncResult ar)
        {
            try
            {
                if (runningTask != null && runningTask == ar.AsyncState)
                {
                    // Read the data
                    double[] data = masterIn.EndRead(ar);

                    // Display the data
                    for (int i = 0; i < masterDataTable.Rows.Count && i < data.Length; i++)
                    {
                        masterDataTable.Rows[i][0] = data[i];
                    }

                    // Set up next callback
                    masterIn.BeginRead(masterCallback, masterIn);
                }
            }
            catch (Exception ex)
            {
                StopTask();
                MessageBox.Show(ex.Message);
            }
        }

        private void SlaveWrite(IAsyncResult ar)
        {
            slaveOut.BeginWrite(output);
        }

        private void MasterWrite(IAsyncResult ar)
        {
            masterOut.BeginWrite(output);
        }

        private void stopButton_Click(object sender, System.EventArgs e)
        {
            StopTask();
        }

        private void StartTask()
        {
            if (runningTask == null)
            {
                // Change state
                runningTask = masterIn;

                // Fix UI
                masterInputChannelComboBox.Enabled = false;
                masterOutputChannelComboBox.Enabled = false;
                masterMaxValNumeric.Enabled = false;
                masterMinValNumeric.Enabled = false;
                masterMinValNumeric.Enabled = false;
            
                slaveInputChannelComboBox.Enabled = false;
                slaveOutputChannelComboBox.Enabled = false;
                slaveMaxValNumeric.Enabled = false;
                slaveMinValNumeric.Enabled = false;
                slaveMinValNumeric.Enabled = false;

                samplesPerChannelNumeric.Enabled = false;
                rateNumeric.Enabled = false;
            
                waveformTypeComboBox.Enabled = false;
                amplitudeNumeric.Enabled = false;
                samplesPerBufferNumeric.Enabled = false;
                cyclesPerBufferNumeric.Enabled = false;

                startButton.Enabled = false;
                stopButton.Enabled = true;
            }
        }

        private void StopTask()
        {
            // Change state
            runningTask = null;

            // Fix UI
            masterInputChannelComboBox.Enabled = true;
            masterOutputChannelComboBox.Enabled = true;
            masterMaxValNumeric.Enabled = true;
            masterMinValNumeric.Enabled = true;
            masterMinValNumeric.Enabled = true;
        
            slaveInputChannelComboBox.Enabled = true;
            slaveOutputChannelComboBox.Enabled = true;
            slaveMaxValNumeric.Enabled = true;
            slaveMinValNumeric.Enabled = true;
            slaveMinValNumeric.Enabled = true;

            samplesPerChannelNumeric.Enabled = true;
            rateNumeric.Enabled = true;
        
            waveformTypeComboBox.Enabled = true;
            amplitudeNumeric.Enabled = true;
            samplesPerBufferNumeric.Enabled = true;
            cyclesPerBufferNumeric.Enabled = true;

            startButton.Enabled = true;
            stopButton.Enabled = false;
        
            // Stop tasks
            slaveIn.Stop();
            slaveOut.Stop();
            masterIn.Stop();
            masterOut.Stop();

            slaveIn.Dispose();
            slaveOut.Dispose();
            masterIn.Dispose();
            masterOut.Dispose();
        }

        public static string GetDeviceName(string deviceName)
        {
            Device device = DaqSystem.Local.LoadDevice(deviceName);
            if (device.BusType != DeviceBusType.CompactDaq)
                return deviceName;
            else
                return device.CompactDaqChassisDeviceName;
        }
    }
}
