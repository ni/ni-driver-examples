/******************************************************************************
*
* Example program:
*   ContAcqPowerSamples_IntClk
*
* Category:
*   Power
*
* Description:
*   This example demonstrates how to acquire a continuous amount of data using
*   the DAQ device's internal clock.
*
* Instructions for running:
*   1.  Select the physical channel(s) corresponding to your devices'
*       connections.
*   2.  Enter the voltage setpoint, current setpoint, and output enable
*       settings.
*   3.  Set the rate and Samples per Channel of the acquisition. Note: In order
*       to avoid Error -50410 (buffer overflow) it is important to make sure
*       the rate and the number of samples to read per iteration are set such
*       that they don't fill the buffer too quickly. If this error occurs, try
*       reducing the rate or increasing the number of samples to read per
*       iteration.
*   4.  Start the acquisition.
*   5.  Update the configuration while running by hitting the "Update while
*       Running" button.
*   6.  Stop the acquisition.
*
* Steps:
*   1.  Create a new task.
*   2.  Create an analog input power channel.
*   3.  Set up the timing for the acquisition. In this example, we use the DAQ
*       device's internal clock to continuously acquire samples.
*   4.  Call AnalogMultiChannelReader.BeginReadMultiSamplePower to install a
*       callback and begin the asynchronous read operation.
*   5.  Inside the callback, call EndReadMultiSamplePower to retrieve the data
*       from the read operation. Note: If the output is disabled, the data read
*       will be NaN.
*   6.  Inside the callback, query the runtime status of the task.
*   7.  Inside the callback, call BeginReadMultiSamplePower again.
*   8.  Dispose the Task object to clean-up any resources associated with the
*       task.
*   9.  Handle any DaqExceptions, if they occur.
*
*   Note: This example sets SynchronizeCallback to true. If SynchronizeCallback
*   is set to false, then you must give special consideration to safely dispose
*   the task and to update the UI from the callback. If SynchronizeCallback is
*   set to false, the callback executes on the worker thread and not on the main
*   UI thread. You can only update a UI component on the thread on which it was
*   created. Refer to the How to: Safely Dispose Task When Using Asynchronous
*   Callbacks topic in the NI-DAQmx .NET help for more information.
*
* Microsoft Windows User Account Control
*   Running certain applications on Microsoft Windows requires administrator
*   privileges, because the application name contains keywords such as setup,
*   update, or install. To avoid this problem, you must add an additional
*   manifest to the application that specifies the privileges required to run
*   the application. Some NI-DAQmx examples for Visual Studio include these
*   keywords. Therefore, all examples for Visual Studio are shipped with an
*   additional manifest file that you must embed in the example executable. The
*   manifest file is named [ExampleName].exe.manifest, where [ExampleName] is
*   the NI-provided example name. For information on how to embed the manifest
*   file, refer to http://msdn2.microsoft.com/en-us/library/bb756929.aspx.
******************************************************************************/

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using NationalInstruments.DAQmx;

namespace NationalInstruments.Examples.ContAcqPowerSamples_IntClk
{
    /// <summary>
    /// Summary description for MainForm.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.GroupBox channelParametersGroupBox;
        private System.Windows.Forms.Label currentLabel;
        private System.Windows.Forms.Label voltageLabel;
        private System.Windows.Forms.Label physicalChannelLabel;
        private System.Windows.Forms.Label rateLabel;
        private System.Windows.Forms.Label samplesLabel;
        private System.Windows.Forms.Label resultLabel;

        private AnalogMultiChannelReader analogInReader;
        private Task myTask;
        private Task runningTask;
        private AsyncCallback analogCallback;

        private AIPowerMeasurement[,] data;
        private DataColumn[] dataColumn = null;
        private DataTable dataTable = null;
        private System.Windows.Forms.GroupBox timingParametersGroupBox;
        private System.Windows.Forms.GroupBox acquisitionResultGroupBox;
        private System.Windows.Forms.DataGrid acquisitionDataGrid;
        private System.Windows.Forms.NumericUpDown rateNumeric;
        private System.Windows.Forms.NumericUpDown samplesPerChannelNumeric;
        internal System.Windows.Forms.NumericUpDown voltageSetpoint;
        internal System.Windows.Forms.NumericUpDown currentSetpoint;
        private System.Windows.Forms.ComboBox physicalChannelComboBox;
        private CheckBox enableOutput;
        private Button updateButton;
        private GroupBox statusBox;
        private Label outputStateLabel;
        private TextBox outputState;
        private CheckBox overtemperatureChannelsExistCheckbox;
        private TextBox overtemperatureChannels;
        private Label overtemperatureLabel;
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
            stopButton.Enabled = false;
            dataTable = new DataTable();

            physicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.Power, PhysicalChannelAccess.External));
            if (physicalChannelComboBox.Items.Count > 0)
                physicalChannelComboBox.SelectedIndex = 0;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
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
            base.Dispose(disposing);
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
            this.updateButton = new System.Windows.Forms.Button();
            this.enableOutput = new System.Windows.Forms.CheckBox();
            this.physicalChannelComboBox = new System.Windows.Forms.ComboBox();
            this.voltageSetpoint = new System.Windows.Forms.NumericUpDown();
            this.currentSetpoint = new System.Windows.Forms.NumericUpDown();
            this.currentLabel = new System.Windows.Forms.Label();
            this.voltageLabel = new System.Windows.Forms.Label();
            this.physicalChannelLabel = new System.Windows.Forms.Label();
            this.timingParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.rateNumeric = new System.Windows.Forms.NumericUpDown();
            this.samplesLabel = new System.Windows.Forms.Label();
            this.rateLabel = new System.Windows.Forms.Label();
            this.samplesPerChannelNumeric = new System.Windows.Forms.NumericUpDown();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.acquisitionResultGroupBox = new System.Windows.Forms.GroupBox();
            this.resultLabel = new System.Windows.Forms.Label();
            this.acquisitionDataGrid = new System.Windows.Forms.DataGrid();
            this.statusBox = new System.Windows.Forms.GroupBox();
            this.overtemperatureChannels = new System.Windows.Forms.TextBox();
            this.overtemperatureLabel = new System.Windows.Forms.Label();
            this.overtemperatureChannelsExistCheckbox = new System.Windows.Forms.CheckBox();
            this.outputStateLabel = new System.Windows.Forms.Label();
            this.outputState = new System.Windows.Forms.TextBox();
            this.channelParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.voltageSetpoint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.currentSetpoint)).BeginInit();
            this.timingParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rateNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerChannelNumeric)).BeginInit();
            this.acquisitionResultGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.acquisitionDataGrid)).BeginInit();
            this.statusBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // channelParametersGroupBox
            // 
            this.channelParametersGroupBox.Controls.Add(this.updateButton);
            this.channelParametersGroupBox.Controls.Add(this.enableOutput);
            this.channelParametersGroupBox.Controls.Add(this.physicalChannelComboBox);
            this.channelParametersGroupBox.Controls.Add(this.voltageSetpoint);
            this.channelParametersGroupBox.Controls.Add(this.currentSetpoint);
            this.channelParametersGroupBox.Controls.Add(this.currentLabel);
            this.channelParametersGroupBox.Controls.Add(this.voltageLabel);
            this.channelParametersGroupBox.Controls.Add(this.physicalChannelLabel);
            this.channelParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.channelParametersGroupBox.Location = new System.Drawing.Point(8, 8);
            this.channelParametersGroupBox.Name = "channelParametersGroupBox";
            this.channelParametersGroupBox.Size = new System.Drawing.Size(258, 186);
            this.channelParametersGroupBox.TabIndex = 2;
            this.channelParametersGroupBox.TabStop = false;
            this.channelParametersGroupBox.Text = "Channel Parameters";
            // 
            // updateButton
            // 
            this.updateButton.Enabled = false;
            this.updateButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.updateButton.Location = new System.Drawing.Point(68, 152);
            this.updateButton.Name = "updateButton";
            this.updateButton.Size = new System.Drawing.Size(125, 24);
            this.updateButton.TabIndex = 6;
            this.updateButton.Text = "Update while Running";
            this.updateButton.Click += new System.EventHandler(this.updateButton_Click);
            // 
            // enableOutput
            // 
            this.enableOutput.AutoSize = true;
            this.enableOutput.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.enableOutput.Location = new System.Drawing.Point(38, 118);
            this.enableOutput.Name = "enableOutput";
            this.enableOutput.Size = new System.Drawing.Size(94, 17);
            this.enableOutput.TabIndex = 6;
            this.enableOutput.Text = "Enable Output";
            this.enableOutput.UseVisualStyleBackColor = true;
            // 
            // physicalChannelComboBox
            // 
            this.physicalChannelComboBox.Location = new System.Drawing.Point(120, 24);
            this.physicalChannelComboBox.Name = "physicalChannelComboBox";
            this.physicalChannelComboBox.Size = new System.Drawing.Size(132, 21);
            this.physicalChannelComboBox.TabIndex = 1;
            this.physicalChannelComboBox.Text = "TS1Mod1/power";
            // 
            // voltageSetpoint
            // 
            this.voltageSetpoint.DecimalPlaces = 2;
            this.voltageSetpoint.Location = new System.Drawing.Point(120, 56);
            this.voltageSetpoint.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.voltageSetpoint.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.voltageSetpoint.Name = "voltageSetpoint";
            this.voltageSetpoint.Size = new System.Drawing.Size(96, 20);
            this.voltageSetpoint.TabIndex = 3;
            this.voltageSetpoint.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            // 
            // currentSetpoint
            // 
            this.currentSetpoint.DecimalPlaces = 2;
            this.currentSetpoint.Location = new System.Drawing.Point(120, 88);
            this.currentSetpoint.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.currentSetpoint.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.currentSetpoint.Name = "currentSetpoint";
            this.currentSetpoint.Size = new System.Drawing.Size(96, 20);
            this.currentSetpoint.TabIndex = 5;
            this.currentSetpoint.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // currentLabel
            // 
            this.currentLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.currentLabel.Location = new System.Drawing.Point(16, 88);
            this.currentLabel.Name = "currentLabel";
            this.currentLabel.Size = new System.Drawing.Size(104, 20);
            this.currentLabel.TabIndex = 4;
            this.currentLabel.Text = "Current Setpoint (A):";
            // 
            // voltageLabel
            // 
            this.voltageLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.voltageLabel.Location = new System.Drawing.Point(16, 56);
            this.voltageLabel.Name = "voltageLabel";
            this.voltageLabel.Size = new System.Drawing.Size(104, 15);
            this.voltageLabel.TabIndex = 2;
            this.voltageLabel.Text = "Voltage Setpoint (V):";
            // 
            // physicalChannelLabel
            // 
            this.physicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.physicalChannelLabel.Location = new System.Drawing.Point(16, 26);
            this.physicalChannelLabel.Name = "physicalChannelLabel";
            this.physicalChannelLabel.Size = new System.Drawing.Size(96, 16);
            this.physicalChannelLabel.TabIndex = 0;
            this.physicalChannelLabel.Text = "Physical Channel:";
            // 
            // timingParametersGroupBox
            // 
            this.timingParametersGroupBox.Controls.Add(this.rateNumeric);
            this.timingParametersGroupBox.Controls.Add(this.samplesLabel);
            this.timingParametersGroupBox.Controls.Add(this.rateLabel);
            this.timingParametersGroupBox.Controls.Add(this.samplesPerChannelNumeric);
            this.timingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.timingParametersGroupBox.Location = new System.Drawing.Point(8, 200);
            this.timingParametersGroupBox.Name = "timingParametersGroupBox";
            this.timingParametersGroupBox.Size = new System.Drawing.Size(258, 92);
            this.timingParametersGroupBox.TabIndex = 3;
            this.timingParametersGroupBox.TabStop = false;
            this.timingParametersGroupBox.Text = "Timing Parameters";
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
            10000,
            0,
            0,
            0});
            // 
            // samplesLabel
            // 
            this.samplesLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.samplesLabel.Location = new System.Drawing.Point(16, 26);
            this.samplesLabel.Name = "samplesLabel";
            this.samplesLabel.Size = new System.Drawing.Size(104, 16);
            this.samplesLabel.TabIndex = 0;
            this.samplesLabel.Text = "Samples/Channel:";
            // 
            // rateLabel
            // 
            this.rateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rateLabel.Location = new System.Drawing.Point(16, 58);
            this.rateLabel.Name = "rateLabel";
            this.rateLabel.Size = new System.Drawing.Size(56, 16);
            this.rateLabel.TabIndex = 2;
            this.rateLabel.Text = "Rate (Hz):";
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
            // startButton
            // 
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.startButton.Location = new System.Drawing.Point(40, 304);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(80, 24);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.stopButton.Location = new System.Drawing.Point(152, 304);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(80, 24);
            this.stopButton.TabIndex = 1;
            this.stopButton.Text = "Stop";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // acquisitionResultGroupBox
            // 
            this.acquisitionResultGroupBox.Controls.Add(this.resultLabel);
            this.acquisitionResultGroupBox.Controls.Add(this.acquisitionDataGrid);
            this.acquisitionResultGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.acquisitionResultGroupBox.Location = new System.Drawing.Point(272, 8);
            this.acquisitionResultGroupBox.Name = "acquisitionResultGroupBox";
            this.acquisitionResultGroupBox.Size = new System.Drawing.Size(304, 320);
            this.acquisitionResultGroupBox.TabIndex = 4;
            this.acquisitionResultGroupBox.TabStop = false;
            this.acquisitionResultGroupBox.Text = "Acquisition Results";
            // 
            // resultLabel
            // 
            this.resultLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.resultLabel.Location = new System.Drawing.Point(8, 16);
            this.resultLabel.Name = "resultLabel";
            this.resultLabel.Size = new System.Drawing.Size(112, 16);
            this.resultLabel.TabIndex = 0;
            this.resultLabel.Text = "Acquisition Data:";
            // 
            // acquisitionDataGrid
            // 
            this.acquisitionDataGrid.AllowSorting = false;
            this.acquisitionDataGrid.DataMember = "";
            this.acquisitionDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.acquisitionDataGrid.Location = new System.Drawing.Point(16, 32);
            this.acquisitionDataGrid.Name = "acquisitionDataGrid";
            this.acquisitionDataGrid.ParentRowsVisible = false;
            this.acquisitionDataGrid.PreferredColumnWidth = 120;
            this.acquisitionDataGrid.ReadOnly = true;
            this.acquisitionDataGrid.Size = new System.Drawing.Size(280, 282);
            this.acquisitionDataGrid.TabIndex = 1;
            this.acquisitionDataGrid.TabStop = false;
            // 
            // statusBox
            // 
            this.statusBox.Controls.Add(this.overtemperatureChannels);
            this.statusBox.Controls.Add(this.overtemperatureLabel);
            this.statusBox.Controls.Add(this.overtemperatureChannelsExistCheckbox);
            this.statusBox.Controls.Add(this.outputStateLabel);
            this.statusBox.Controls.Add(this.outputState);
            this.statusBox.Location = new System.Drawing.Point(582, 8);
            this.statusBox.Name = "statusBox";
            this.statusBox.Size = new System.Drawing.Size(215, 218);
            this.statusBox.TabIndex = 5;
            this.statusBox.TabStop = false;
            this.statusBox.Text = "Status";
            // 
            // overtemperatureChannels
            // 
            this.overtemperatureChannels.Location = new System.Drawing.Point(12, 113);
            this.overtemperatureChannels.Multiline = true;
            this.overtemperatureChannels.Name = "overtemperatureChannels";
            this.overtemperatureChannels.ReadOnly = true;
            this.overtemperatureChannels.Size = new System.Drawing.Size(191, 99);
            this.overtemperatureChannels.TabIndex = 3;
            // 
            // overtemperatureLabel
            // 
            this.overtemperatureLabel.AutoSize = true;
            this.overtemperatureLabel.Location = new System.Drawing.Point(9, 97);
            this.overtemperatureLabel.Name = "overtemperatureLabel";
            this.overtemperatureLabel.Size = new System.Drawing.Size(133, 13);
            this.overtemperatureLabel.TabIndex = 4;
            this.overtemperatureLabel.Text = "Overtemperature Channels";
            // 
            // overtemperatureChannelsExistCheckbox
            // 
            this.overtemperatureChannelsExistCheckbox.AutoCheck = false;
            this.overtemperatureChannelsExistCheckbox.AutoSize = true;
            this.overtemperatureChannelsExistCheckbox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.overtemperatureChannelsExistCheckbox.Location = new System.Drawing.Point(9, 64);
            this.overtemperatureChannelsExistCheckbox.Name = "overtemperatureChannelsExistCheckbox";
            this.overtemperatureChannelsExistCheckbox.Size = new System.Drawing.Size(177, 17);
            this.overtemperatureChannelsExistCheckbox.TabIndex = 2;
            this.overtemperatureChannelsExistCheckbox.Text = "Overtemperature Channels Exist";
            this.overtemperatureChannelsExistCheckbox.UseVisualStyleBackColor = true;
            // 
            // outputStateLabel
            // 
            this.outputStateLabel.AutoSize = true;
            this.outputStateLabel.Location = new System.Drawing.Point(9, 30);
            this.outputStateLabel.Name = "outputStateLabel";
            this.outputStateLabel.Size = new System.Drawing.Size(67, 13);
            this.outputStateLabel.TabIndex = 1;
            this.outputStateLabel.Text = "Output State";
            // 
            // outputState
            // 
            this.outputState.Location = new System.Drawing.Point(82, 27);
            this.outputState.Name = "outputState";
            this.outputState.ReadOnly = true;
            this.outputState.Size = new System.Drawing.Size(121, 20);
            this.outputState.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(806, 335);
            this.Controls.Add(this.statusBox);
            this.Controls.Add(this.acquisitionResultGroupBox);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.timingParametersGroupBox);
            this.Controls.Add(this.channelParametersGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Continuous Acquisition of Power Samples - Internal Clock";
            this.channelParametersGroupBox.ResumeLayout(false);
            this.channelParametersGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.voltageSetpoint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.currentSetpoint)).EndInit();
            this.timingParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rateNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerChannelNumeric)).EndInit();
            this.acquisitionResultGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.acquisitionDataGrid)).EndInit();
            this.statusBox.ResumeLayout(false);
            this.statusBox.PerformLayout();
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
                try
                {
                    stopButton.Enabled = true;
                    updateButton.Enabled = true;
                    startButton.Enabled = false;

                    // Create a new task
                    myTask = new Task();

                    // Create a virtual channel
                    myTask.AIChannels.CreatePowerChannel(physicalChannelComboBox.Text, "",
                        Convert.ToDouble(voltageSetpoint.Value), Convert.ToDouble(currentSetpoint.Value),
                        enableOutput.Checked);

                    // Configure the timing parameters
                    myTask.Timing.ConfigureSampleClock("", Convert.ToDouble(rateNumeric.Value),
                        SampleClockActiveEdge.Rising, SampleQuantityMode.ContinuousSamples, 1000);

                    // Verify the Task
                    myTask.Control(TaskAction.Verify);

                    // Prepare the table for Data
                    InitializeDataTable(myTask.AIChannels, ref dataTable);
                    acquisitionDataGrid.DataSource = dataTable;

                    runningTask = myTask;
                    analogInReader = new AnalogMultiChannelReader(myTask.Stream);
                    analogCallback = new AsyncCallback(AnalogInCallback);

                    // Use SynchronizeCallbacks to specify that the object 
                    // marshals callbacks across threads appropriately.
                    analogInReader.SynchronizeCallbacks = true;
                    analogInReader.BeginReadMultiSamplePower(Convert.ToInt32(samplesPerChannelNumeric.Value),
                        analogCallback, myTask);
                }
                catch (DaqException exception)
                {
                    // Display Errors
                    MessageBox.Show(exception.Message);
                    runningTask = null;
                    myTask.Dispose();
                    stopButton.Enabled = false;
                    updateButton.Enabled = false;
                    startButton.Enabled = true;
                }
            }
        }

        private void updateStatus()
        {
            if (runningTask != null)
            {
                switch (runningTask.AIChannels.All.PowerOutputState)
                {
                    case PowerOutputState.ConstantCurrent:
                        outputState.Text = "Constant Current";
                        break;
                    case PowerOutputState.ConstantVoltage:
                        outputState.Text = "Constant Voltage";
                        break;
                    case PowerOutputState.OutputDisabled:
                        outputState.Text = "Output Disabled";
                        break;
                    case PowerOutputState.Overvoltage:
                        outputState.Text = "Overvoltage";
                        break;
                }

                bool overtempChansExist = runningTask.Stream.ReadOvertemperatureChannelsExist;
                overtemperatureChannelsExistCheckbox.Checked = overtempChansExist;
                if (overtempChansExist)
                {
                    overtemperatureChannels.Lines = runningTask.Stream.ReadOvertemperatureChannels;
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
                    data = analogInReader.EndReadMultiSamplePower(ar);

                    // Plot your data here
                    dataToDataTable(data, ref dataTable);

                    // Read runtime status
                    updateStatus();

                    analogInReader.BeginReadMultiSamplePower(Convert.ToInt32(samplesPerChannelNumeric.Value),
                        analogCallback, myTask);
                }
            }
            catch (DaqException exception)
            {
                // Display Errors
                MessageBox.Show(exception.Message);
                runningTask = null;
                myTask.Dispose();
                stopButton.Enabled = false;
                updateButton.Enabled = false;
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
                updateButton.Enabled = false;
                startButton.Enabled = true;
            }
        }

        private void dataToDataTable(AIPowerMeasurement[,] sourceArray, ref DataTable dataTable)
        {
            // Iterate over channels
            for (int channel = 0; channel < sourceArray.GetLength(0); ++channel)
            {
                // Iterate over samples
                for (int sample = 0; sample < sourceArray.GetLength(1); ++sample)
                {
                    if (sample == 10)
                        break;

                    AIPowerMeasurement data = sourceArray[channel, sample];
                    dataTable.Rows[sample][channel * 2] = data.Voltage;
                    dataTable.Rows[sample][channel * 2 + 1] = data.Current;
                }
            }
        }

        public void InitializeDataTable(AIChannelCollection channelCollection, ref DataTable data)
        {
            int numOfChannels = channelCollection.Count;
            data.Rows.Clear();
            data.Columns.Clear();
            dataColumn = new DataColumn[numOfChannels * 2];
            int numOfRows = 10;

            for (int channel = 0; channel < numOfChannels; channel++)
            {
                int currentColumnIndex = channel * 2;
                dataColumn[currentColumnIndex] = new DataColumn();
                dataColumn[currentColumnIndex].DataType = typeof(double);
                dataColumn[currentColumnIndex].ColumnName = channelCollection[channel].PhysicalName + " (V)";

                currentColumnIndex = channel * 2 + 1;
                dataColumn[currentColumnIndex] = new DataColumn();
                dataColumn[currentColumnIndex].DataType = typeof(double);
                dataColumn[currentColumnIndex].ColumnName = channelCollection[channel].PhysicalName + " (A)";
            }

            data.Columns.AddRange(dataColumn);

            for (int currentDataIndex = 0; currentDataIndex < numOfRows; currentDataIndex++)
            {
                object[] rowArr = new object[numOfChannels];
                data.Rows.Add(rowArr);
            }
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (runningTask != null)
                {
                    runningTask.AIChannels.All.PowerVoltageSetpoint = Convert.ToDouble(voltageSetpoint.Value);
                    runningTask.AIChannels.All.PowerCurrentSetpoint = Convert.ToDouble(currentSetpoint.Value);
                    runningTask.AIChannels.All.PowerOutputEnable = enableOutput.Checked;
                }
            }
            catch (DaqException exception)
            {
                // Display Errors
                MessageBox.Show(exception.Message);
                runningTask = null;
                myTask.Dispose();
                stopButton.Enabled = false;
                updateButton.Enabled = false;
                startButton.Enabled = true;
            }
        }
    }
}
