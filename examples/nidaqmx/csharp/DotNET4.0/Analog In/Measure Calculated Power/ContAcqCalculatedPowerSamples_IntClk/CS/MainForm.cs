/******************************************************************************
*
* Example program:
*   ContAcqCalculatedPowerSamples_IntClk
*
* Category:
*   AI
*
* Description:
*   This example demonstrates how to acquire a continuous amount of calculated power data using an
*   internal clock.
*
* Instructions for running:
*   1.  Select the physical channels corresponding to the input signal on the DAQ
*       device.
*   2.  Enter the minimum and maximum voltage and current ranges.Note:  For better accuracy,
*       try to match the input ranges to the expected voltage and current levels of the
*       measured signal.
*   3.  Select the number of samples per channel to acquire.
*   4.  Set the rate in Hz for the internal clock.Note:  The rate should be at
*       least twice as fast as the maximum frequency component of the signal
*       being acquired. Also, in order to avoid Error -50410 (buffer overflow) it is important
*       to make sure the rate and the number of samples to read per iteration
*       are set such that they don't fill the buffer too quickly. If this error
*       occurs, try reducing the rate or increasing the number of samples to
*       read per iteration.
*
* Steps:
*   1.  Create a new task with analog input voltage and current channel.
*   2.  Configure the task to use the internal clock. In this example, we use the DAQ
*       device's internal clock to continuously acquire samples.
*   3.  Call AnalogMultiChannelReader.BeginReadWaveform to install a callback
*       and begin the asynchronous read operation.
*   4.  Inside the callback, call AnalogMultiChannelReader.EndReadWaveform to
*       retrieve the data from the read operation.
*   5.  Call AnalogMultiChannelReader.BeginMemoryOptimizedReadWaveform
*   6.  Dispose the Task object to clean-up any resources associated with the
*       task.
*   7.  Handle any DaqExceptions, if they occur.
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
*   Make sure your signal input terminals match the physical channel text box.
*   For more information on the input and output terminals for your device, open
*   the NI-DAQmx Help, and refer to the NI-DAQmx Device Terminals and Device
*   Considerations books in the table of contents.
*
*
******************************************************************************/

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using NationalInstruments.DAQmx;
using NationalInstruments.Restricted;

namespace NationalInstruments.Examples.AcqCalculatedPowerSamples_IntClk
{
    /// <summary>
    /// Summary description for MainForm.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        private Task myTask;
        private Task runningTask;
        private AnalogMultiChannelReader reader;
        private AsyncCallback analogCallback;
        private AnalogWaveform<double>[] data;
        private DataColumn[] dataColumn = null;
        private DataTable dataTable = null;

        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.GroupBox channelParametersGroupBox;
        private System.Windows.Forms.Label voltagePhysicalChannelLabel;
        private System.Windows.Forms.Label currentPhysicalChannelLabel;
        private System.Windows.Forms.Label voltageMinimumLabel;
        private System.Windows.Forms.Label voltageMaximumLabel;
        private System.Windows.Forms.Label currentMinimumLabel;
        private System.Windows.Forms.Label currentMaximumLabel;
        private System.Windows.Forms.GroupBox currentParametersGroupBox;
        private System.Windows.Forms.ComboBox shuntResistorLocationComboxBox;
        private System.Windows.Forms.NumericUpDown shuntResistorNumeric;
        private System.Windows.Forms.Label shuntResistorLabel;
        private System.Windows.Forms.Label shuntResistorLocationLabel;
        private System.Windows.Forms.GroupBox timingParametersGroupBox;
        private System.Windows.Forms.Label rateLabel;
        private System.Windows.Forms.Label samplesLabel;
        private System.Windows.Forms.GroupBox acquisitionResultGroupBox;
        private System.Windows.Forms.NumericUpDown samplesPerChannelNumeric;
        private System.Windows.Forms.DataGrid acquisitionDataGrid;
        private System.Windows.Forms.NumericUpDown rateNumeric;
        private System.Windows.Forms.NumericUpDown voltageMinimumValueNumeric;
        private System.Windows.Forms.NumericUpDown voltageMaximumValueNumeric;
        private System.Windows.Forms.NumericUpDown currentMinimumValueNumeric;
        private System.Windows.Forms.NumericUpDown currentMaximumValueNumeric;
        private System.Windows.Forms.ComboBox voltagePhysicalChannelComboBox;
        private System.Windows.Forms.ComboBox currentPhysicalChannelComboBox;
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
            shuntResistorLocationComboxBox.SelectedIndex = 1;

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            dataTable = new DataTable();

            voltagePhysicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AI, PhysicalChannelAccess.External));
            if (voltagePhysicalChannelComboBox.Items.Count > 0)
                voltagePhysicalChannelComboBox.SelectedIndex = 0;
            currentPhysicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AI, PhysicalChannelAccess.External));
            if (currentPhysicalChannelComboBox.Items.Count > 0)
                currentPhysicalChannelComboBox.SelectedIndex = 0;
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
                if (myTask != null)
                {
                    runningTask = null;
                    myTask.Dispose();
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
            this.channelParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.voltagePhysicalChannelComboBox = new System.Windows.Forms.ComboBox();
            this.currentPhysicalChannelComboBox = new System.Windows.Forms.ComboBox();
            this.voltageMinimumValueNumeric = new System.Windows.Forms.NumericUpDown();
            this.voltageMaximumValueNumeric = new System.Windows.Forms.NumericUpDown();
            this.currentMinimumValueNumeric = new System.Windows.Forms.NumericUpDown();
            this.currentMaximumValueNumeric = new System.Windows.Forms.NumericUpDown();
            this.voltagePhysicalChannelLabel = new System.Windows.Forms.Label();
            this.currentPhysicalChannelLabel = new System.Windows.Forms.Label();
            this.voltageMinimumLabel = new System.Windows.Forms.Label();
            this.voltageMaximumLabel = new System.Windows.Forms.Label();
            this.currentMinimumLabel = new System.Windows.Forms.Label();
            this.currentMaximumLabel = new System.Windows.Forms.Label();
            this.currentParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.shuntResistorNumeric = new System.Windows.Forms.NumericUpDown();
            this.shuntResistorLocationComboxBox = new System.Windows.Forms.ComboBox();
            this.shuntResistorLocationLabel = new System.Windows.Forms.Label();
            this.shuntResistorLabel = new System.Windows.Forms.Label();
            this.timingParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.rateLabel = new System.Windows.Forms.Label();
            this.samplesLabel = new System.Windows.Forms.Label();
            this.samplesPerChannelNumeric = new System.Windows.Forms.NumericUpDown();
            this.rateNumeric = new System.Windows.Forms.NumericUpDown();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.acquisitionResultGroupBox = new System.Windows.Forms.GroupBox();
            this.acquisitionDataGrid = new System.Windows.Forms.DataGrid();
            this.channelParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.voltageMinimumValueNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.voltageMaximumValueNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.currentMinimumValueNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.currentMaximumValueNumeric)).BeginInit();
            this.currentParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shuntResistorNumeric)).BeginInit();
            this.timingParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerChannelNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rateNumeric)).BeginInit();
            this.acquisitionResultGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.acquisitionDataGrid)).BeginInit();
            this.SuspendLayout();
            //
            // channelParametersGroupBox
            //
            this.channelParametersGroupBox.Controls.Add(this.voltagePhysicalChannelComboBox);
            this.channelParametersGroupBox.Controls.Add(this.currentPhysicalChannelComboBox);
            this.channelParametersGroupBox.Controls.Add(this.voltageMinimumValueNumeric);
            this.channelParametersGroupBox.Controls.Add(this.voltageMaximumValueNumeric);
            this.channelParametersGroupBox.Controls.Add(this.currentMinimumValueNumeric);
            this.channelParametersGroupBox.Controls.Add(this.currentMaximumValueNumeric);
            this.channelParametersGroupBox.Controls.Add(this.voltagePhysicalChannelLabel);
            this.channelParametersGroupBox.Controls.Add(this.currentPhysicalChannelLabel);
            this.channelParametersGroupBox.Controls.Add(this.voltageMinimumLabel);
            this.channelParametersGroupBox.Controls.Add(this.voltageMaximumLabel);
            this.channelParametersGroupBox.Controls.Add(this.currentMinimumLabel);
            this.channelParametersGroupBox.Controls.Add(this.currentMaximumLabel);
            this.channelParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.channelParametersGroupBox.Location = new System.Drawing.Point(8, 12);
            this.channelParametersGroupBox.Name = "channelParametersGroupBox";
            this.channelParametersGroupBox.Size = new System.Drawing.Size(250, 200);
            this.channelParametersGroupBox.TabIndex = 0;
            this.channelParametersGroupBox.TabStop = false;
            this.channelParametersGroupBox.Text = "Channel Parameters";
            //
            // voltagePhysicalChannelComboBox
            //
            this.voltagePhysicalChannelComboBox.Location = new System.Drawing.Point(145, 25);
            this.voltagePhysicalChannelComboBox.Name = "voltagePhysicalChannelComboBox";
            this.voltagePhysicalChannelComboBox.Size = new System.Drawing.Size(96, 21);
            this.voltagePhysicalChannelComboBox.TabIndex = 1;
            this.voltagePhysicalChannelComboBox.Text = "Dev1/ai0";
            //
            // currentPhysicalChannelComboBox
            //
            this.currentPhysicalChannelComboBox.Location = new System.Drawing.Point(145, 115);
            this.currentPhysicalChannelComboBox.Name = "currentPhysicalChannelComboBox";
            this.currentPhysicalChannelComboBox.Size = new System.Drawing.Size(96, 21);
            this.currentPhysicalChannelComboBox.TabIndex = 1;
            this.currentPhysicalChannelComboBox.Text = "Dev1/ai1";
            //
            // voltageMinimumValueNumeric
            //
            this.voltageMinimumValueNumeric.DecimalPlaces = 2;
            this.voltageMinimumValueNumeric.Location = new System.Drawing.Point(145, 55);
            this.voltageMinimumValueNumeric.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.voltageMinimumValueNumeric.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.voltageMinimumValueNumeric.Name = "voltageMinimumValueNumeric";
            this.voltageMinimumValueNumeric.Size = new System.Drawing.Size(96, 20);
            this.voltageMinimumValueNumeric.TabIndex = 3;
            //
            // voltageMaximumValueNumeric
            //
            this.voltageMaximumValueNumeric.DecimalPlaces = 2;
            this.voltageMaximumValueNumeric.Location = new System.Drawing.Point(145, 85);
            this.voltageMaximumValueNumeric.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.voltageMaximumValueNumeric.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.voltageMaximumValueNumeric.Name = "voltageMaximumValueNumeric";
            this.voltageMaximumValueNumeric.Size = new System.Drawing.Size(96, 20);
            this.voltageMaximumValueNumeric.TabIndex = 5;
            this.voltageMaximumValueNumeric.Value = new decimal(new int[] {
            100,
            0,
            0,
            65536});
            //
            // currentMinimumValueNumeric
            //
            this.currentMinimumValueNumeric.DecimalPlaces = 3;
            this.currentMinimumValueNumeric.Location = new System.Drawing.Point(145, 145);
            this.currentMinimumValueNumeric.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.currentMinimumValueNumeric.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.currentMinimumValueNumeric.Name = "currentMinimumValueNumeric";
            this.currentMinimumValueNumeric.Size = new System.Drawing.Size(96, 20);
            this.currentMinimumValueNumeric.TabIndex = 7;
            //
            // currentMaximumValueNumeric
            //
            this.currentMaximumValueNumeric.DecimalPlaces = 3;
            this.currentMaximumValueNumeric.Location = new System.Drawing.Point(145, 175);
            this.currentMaximumValueNumeric.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.currentMaximumValueNumeric.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.currentMaximumValueNumeric.Name = "currentMaximumValueNumeric";
            this.currentMaximumValueNumeric.Size = new System.Drawing.Size(96, 20);
            this.currentMaximumValueNumeric.TabIndex = 7;
            this.currentMaximumValueNumeric.Value = new decimal(new int[] {
            20,
            0,
            0,
            196608});
            //
            // voltagePhysicalChannelLabel
            //
            this.voltagePhysicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.voltagePhysicalChannelLabel.Location = new System.Drawing.Point(16, 25);
            this.voltagePhysicalChannelLabel.Name = "voltagePhysicalChannelLabel";
            this.voltagePhysicalChannelLabel.Size = new System.Drawing.Size(123, 16);
            this.voltagePhysicalChannelLabel.TabIndex = 0;
            this.voltagePhysicalChannelLabel.Text = "Voltage Physical Channel:";
            //
            // currentPhysicalChannelLabel
            //
            this.currentPhysicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.currentPhysicalChannelLabel.Location = new System.Drawing.Point(16, 115);
            this.currentPhysicalChannelLabel.Name = "currentPhysicalChannelLabel";
            this.currentPhysicalChannelLabel.Size = new System.Drawing.Size(123, 16);
            this.currentPhysicalChannelLabel.TabIndex = 0;
            this.currentPhysicalChannelLabel.Text = "Current Physical Channel:";
            //
            // voltageMinimumLabel
            //
            this.voltageMinimumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.voltageMinimumLabel.Location = new System.Drawing.Point(16, 55);
            this.voltageMinimumLabel.Name = "voltageMinimumLabel";
            this.voltageMinimumLabel.Size = new System.Drawing.Size(96, 16);
            this.voltageMinimumLabel.TabIndex = 2;
            this.voltageMinimumLabel.Text = "Minimum (V):";
            //
            // voltageMaximumLabel
            //
            this.voltageMaximumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.voltageMaximumLabel.Location = new System.Drawing.Point(16, 85);
            this.voltageMaximumLabel.Name = "voltageMaximumLabel";
            this.voltageMaximumLabel.Size = new System.Drawing.Size(96, 16);
            this.voltageMaximumLabel.TabIndex = 4;
            this.voltageMaximumLabel.Text = "Maximum (V):";
            //
            // currentMinimumLabel
            //
            this.currentMinimumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.currentMinimumLabel.Location = new System.Drawing.Point(16, 145);
            this.currentMinimumLabel.Name = "currentMinimumLabel";
            this.currentMinimumLabel.Size = new System.Drawing.Size(123, 16);
            this.currentMinimumLabel.TabIndex = 2;
            this.currentMinimumLabel.Text = "Minimum (A):";
            //
            // currentMaximumLabel
            //
            this.currentMaximumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.currentMaximumLabel.Location = new System.Drawing.Point(16, 175);
            this.currentMaximumLabel.Name = "currentMaximumLabel";
            this.currentMaximumLabel.Size = new System.Drawing.Size(100, 23);
            this.currentMaximumLabel.TabIndex = 9;
            this.currentMaximumLabel.Text = "Maximum (A):";
            //
            // currentParametersGroupBox
            //
            this.currentParametersGroupBox.Controls.Add(this.shuntResistorNumeric);
            this.currentParametersGroupBox.Controls.Add(this.shuntResistorLocationComboxBox);
            this.currentParametersGroupBox.Controls.Add(this.shuntResistorLocationLabel);
            this.currentParametersGroupBox.Controls.Add(this.shuntResistorLabel);
            this.currentParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.currentParametersGroupBox.Location = new System.Drawing.Point(8, 220);
            this.currentParametersGroupBox.Name = "currentParametersGroupBox";
            this.currentParametersGroupBox.Size = new System.Drawing.Size(250, 88);
            this.currentParametersGroupBox.TabIndex = 0;
            this.currentParametersGroupBox.TabStop = false;
            this.currentParametersGroupBox.Text = "Current Parameters";
            //
            // shuntResistorNumeric
            //
            this.shuntResistorNumeric.DecimalPlaces = 2;
            this.shuntResistorNumeric.Location = new System.Drawing.Point(144, 56);
            this.shuntResistorNumeric.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.shuntResistorNumeric.Name = "shuntResistorNumeric";
            this.shuntResistorNumeric.Size = new System.Drawing.Size(80, 20);
            this.shuntResistorNumeric.TabIndex = 3;
            this.shuntResistorNumeric.Value = new decimal(new int[] {
            249,
            0,
            0,
            0});
            //
            // shuntResistorLocationComboxBox
            //
            this.shuntResistorLocationComboxBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.shuntResistorLocationComboxBox.Items.AddRange(new object[] {
            "Internal",
            "External"});
            this.shuntResistorLocationComboxBox.Location = new System.Drawing.Point(144, 24);
            this.shuntResistorLocationComboxBox.Name = "shuntResistorLocationComboxBox";
            this.shuntResistorLocationComboxBox.Size = new System.Drawing.Size(80, 21);
            this.shuntResistorLocationComboxBox.TabIndex = 1;
            this.shuntResistorLocationComboxBox.SelectedIndexChanged += new System.EventHandler(this.shuntResistorLocationComboBox_SelectedIndexChanged);
            //
            // shuntResistorLocationLabel
            //
            this.shuntResistorLocationLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.shuntResistorLocationLabel.Location = new System.Drawing.Point(16, 24);
            this.shuntResistorLocationLabel.Name = "shuntResistorLocationLabel";
            this.shuntResistorLocationLabel.Size = new System.Drawing.Size(128, 16);
            this.shuntResistorLocationLabel.TabIndex = 0;
            this.shuntResistorLocationLabel.Text = "Shunt Resistor Location:";
            //
            // shuntResistorLabel
            //
            this.shuntResistorLabel.Location = new System.Drawing.Point(13, 56);
            this.shuntResistorLabel.Name = "shuntResistorLabel";
            this.shuntResistorLabel.Size = new System.Drawing.Size(150, 16);
            this.shuntResistorLabel.TabIndex = 0;
            this.shuntResistorLabel.Text = "Shunt Resistor (Ohms):";
            //
            // timingParametersGroupBox
            //
            this.timingParametersGroupBox.Controls.Add(this.rateLabel);
            this.timingParametersGroupBox.Controls.Add(this.samplesLabel);
            this.timingParametersGroupBox.Controls.Add(this.samplesPerChannelNumeric);
            this.timingParametersGroupBox.Controls.Add(this.rateNumeric);
            this.timingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.timingParametersGroupBox.Location = new System.Drawing.Point(8, 320);
            this.timingParametersGroupBox.Name = "timingParametersGroupBox";
            this.timingParametersGroupBox.Size = new System.Drawing.Size(250, 88);
            this.timingParametersGroupBox.TabIndex = 1;
            this.timingParametersGroupBox.TabStop = false;
            this.timingParametersGroupBox.Text = "Timing Parameters";
            //
            // rateLabel
            //
            this.rateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rateLabel.Location = new System.Drawing.Point(16, 56);
            this.rateLabel.Name = "rateLabel";
            this.rateLabel.Size = new System.Drawing.Size(64, 16);
            this.rateLabel.TabIndex = 2;
            this.rateLabel.Text = "Rate (Hz):";
            //
            // samplesLabel
            //
            this.samplesLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.samplesLabel.Location = new System.Drawing.Point(16, 24);
            this.samplesLabel.Name = "samplesLabel";
            this.samplesLabel.Size = new System.Drawing.Size(104, 16);
            this.samplesLabel.TabIndex = 0;
            this.samplesLabel.Text = "Samples / Channel:";
            //
            // samplesPerChannelNumeric
            //
            this.samplesPerChannelNumeric.Location = new System.Drawing.Point(120, 24);
            this.samplesPerChannelNumeric.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.samplesPerChannelNumeric.Name = "samplesPerChannelNumeric";
            this.samplesPerChannelNumeric.Size = new System.Drawing.Size(96, 20);
            this.samplesPerChannelNumeric.TabIndex = 1;
            this.samplesPerChannelNumeric.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            //
            // rateNumeric
            //
            this.rateNumeric.DecimalPlaces = 2;
            this.rateNumeric.Location = new System.Drawing.Point(120, 56);
            this.rateNumeric.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.rateNumeric.Name = "rateNumeric";
            this.rateNumeric.Size = new System.Drawing.Size(96, 20);
            this.rateNumeric.TabIndex = 3;
            this.rateNumeric.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            //
            // startButton
            //
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.startButton.Location = new System.Drawing.Point(290, 340);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(80, 24);
            this.startButton.TabIndex = 2;
            this.startButton.Text = "Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            //
            // stopButton
            //
            this.stopButton.Enabled = false;
            this.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.stopButton.Location = new System.Drawing.Point(430, 340);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(80, 24);
            this.stopButton.TabIndex = 1;
            this.stopButton.Text = "Stop";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            //
            // acquisitionResultGroupBox
            //
            this.acquisitionResultGroupBox.Controls.Add(this.acquisitionDataGrid);
            this.acquisitionResultGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.acquisitionResultGroupBox.Location = new System.Drawing.Point(268, 12);
            this.acquisitionResultGroupBox.Name = "acquisitionResultGroupBox";
            this.acquisitionResultGroupBox.Size = new System.Drawing.Size(272, 276);
            this.acquisitionResultGroupBox.TabIndex = 3;
            this.acquisitionResultGroupBox.TabStop = false;
            this.acquisitionResultGroupBox.Text = "Acquisition Results";
            //
            // acquisitionDataGrid
            //
            this.acquisitionDataGrid.AllowSorting = false;
            this.acquisitionDataGrid.DataMember = "";
            this.acquisitionDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.acquisitionDataGrid.Location = new System.Drawing.Point(8, 24);
            this.acquisitionDataGrid.Name = "acquisitionDataGrid";
            this.acquisitionDataGrid.ParentRowsVisible = false;
            this.acquisitionDataGrid.ReadOnly = true;
            this.acquisitionDataGrid.Size = new System.Drawing.Size(256, 248);
            this.acquisitionDataGrid.TabIndex = 1;
            this.acquisitionDataGrid.TabStop = false;
            //
            // MainForm
            //
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(568, 433);
            this.Controls.Add(this.acquisitionResultGroupBox);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.timingParametersGroupBox);
            this.Controls.Add(this.channelParametersGroupBox);
            this.Controls.Add(this.currentParametersGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(590, 500);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Continuous Acquisition of Calculated Power Samples - Internal Clock";
            this.channelParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.voltageMinimumValueNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.voltageMaximumValueNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.currentMinimumValueNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.currentMaximumValueNumeric)).EndInit();
            this.currentParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.shuntResistorNumeric)).EndInit();
            this.timingParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerChannelNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rateNumeric)).EndInit();
            this.acquisitionResultGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.acquisitionDataGrid)).EndInit();
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

        private void startButton_Click(object sender, System.EventArgs e)
        {
            if (runningTask == null)
            {
                startButton.Enabled = false;
                stopButton.Enabled = true;

                try
                {
                    // Create a new task
                    myTask = new Task();

                    // Initialize local variables
                    double sampleRate = Convert.ToDouble(rateNumeric.Value);
                    double voltageRangeMinimum = Convert.ToDouble(voltageMinimumValueNumeric.Value);
                    double voltageRangeMaximum = Convert.ToDouble(voltageMaximumValueNumeric.Value);
                    double currentRangeMinimum = Convert.ToDouble(currentMinimumValueNumeric.Value);
                    double currentRangeMaximum = Convert.ToDouble(currentMaximumValueNumeric.Value);
                    double externalShuntResistor = Convert.ToDouble(shuntResistorNumeric.Value);
                    int samplesPerChannel = Convert.ToInt32(samplesPerChannelNumeric.Value);
                    AICurrentShuntLocation shuntLocation = AICurrentShuntLocation.External;

                    if (shuntResistorLocationComboxBox.SelectedItem.ToString() == "Internal")
                    {
                        shuntLocation = AICurrentShuntLocation.Internal;
                    }


                    // Create calculated power channel
                    myTask.AIChannels.CreateCalculatedPowerChannel(
                        voltagePhysicalChannelComboBox.Text,
                        currentPhysicalChannelComboBox.Text,
                        "", // nameToAssignChannel,
                        (AITerminalConfiguration)(-1),
                        voltageRangeMinimum,
                        voltageRangeMaximum,
                        currentRangeMinimum,
                        currentRangeMaximum,
                        AIPowerUnits.Watts,
                        shuntLocation,
                        externalShuntResistor,
                        "");

                    // Configure timing specs
                    myTask.Timing.ConfigureSampleClock("", sampleRate, SampleClockActiveEdge.Rising,
                        SampleQuantityMode.ContinuousSamples, samplesPerChannel);

                    // Verify the task
                    myTask.Control(TaskAction.Verify);

                    // Prepare the table for data
                    InitializeDataTable(myTask.AIChannels, ref dataTable);
                    acquisitionDataGrid.DataSource = dataTable;

                    // Read the data
                    runningTask = myTask;
                    reader = new AnalogMultiChannelReader(myTask.Stream);
                    analogCallback = new AsyncCallback(AnalogInCallback);

                    // Use SynchronizeCallbacks to specify that the object
                    // marshals callbacks across threads appropriately.
                    reader.SynchronizeCallbacks = true;
                    reader.BeginReadWaveform(Convert.ToInt32(samplesPerChannel),
                        analogCallback, myTask);
                }
                catch (DaqException exception)
                {
                    MessageBox.Show(exception.Message);
                    runningTask = null;
                    myTask.Dispose();
                    startButton.Enabled = true;
                    stopButton.Enabled = false;
                    runningTask = null;
                }
            }
        }

        private void AnalogInCallback(IAsyncResult ar)
        {
            try
            {
                if (runningTask != null && runningTask == ar.AsyncState)
                {
                    // Read the available data from the channels
                    data = reader.EndReadWaveform(ar);

                    // Plot your data here
                    dataToDataTable(data, ref dataTable);

                    reader.BeginMemoryOptimizedReadWaveform(Convert.ToInt32(samplesPerChannelNumeric.Value),
                        analogCallback, myTask, data);
                }
            }
            catch (DaqException exception)
            {
                // Display Errors
                MessageBox.Show(exception.Message);
                runningTask = null;
                myTask.Dispose();
                stopButton.Enabled = false;
                startButton.Enabled = true;
            }
        }

        private void stopButton_Click(object sender, System.EventArgs e)
        {
            if (runningTask != null)
            {
                // Dispose of the task
                runningTask = null;
                myTask.Dispose();
                stopButton.Enabled = false;
                startButton.Enabled = true;
            }
        }

        private void shuntResistorLocationComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            switch (shuntResistorLocationComboxBox.SelectedItem.ToString())
            {
                case "Internal":
                    shuntResistorNumeric.Enabled = false;
                    break;
                case "External":
                default:
                    shuntResistorNumeric.Enabled = true;
                    break;
            }
        }

        private void dataToDataTable(AnalogWaveform<double>[] sourceArray, ref DataTable dataTable)
        {
            // Iterate over channels
            int currentLineIndex = 0;
            foreach (AnalogWaveform<double> waveform in sourceArray)
            {
                for (int sample = 0; sample < waveform.Samples.Count; ++sample)
                {
                    if (sample == 10)
                        break;

                    dataTable.Rows[sample][currentLineIndex] = waveform.Samples[sample].Value;
                }
                currentLineIndex++;
            }
        }

        public void InitializeDataTable(AIChannelCollection channelCollection, ref DataTable data)
        {
            int numOfChannels = channelCollection.Count;
            data.Rows.Clear();
            data.Columns.Clear();
            dataColumn = new DataColumn[numOfChannels];
            int numOfRows = 10;

            for (int currentChannelIndex = 0; currentChannelIndex < numOfChannels; currentChannelIndex++)
            {
                dataColumn[currentChannelIndex] = new DataColumn();
                dataColumn[currentChannelIndex].DataType = typeof(double);
                dataColumn[currentChannelIndex].ColumnName = channelCollection[currentChannelIndex].PhysicalName;
            }

            data.Columns.AddRange(dataColumn);

            for (int currentDataIndex = 0; currentDataIndex < numOfRows; currentDataIndex++)
            {
                object[] rowArr = new object[numOfChannels];
                data.Rows.Add(rowArr);
            }
        }
    }
}
