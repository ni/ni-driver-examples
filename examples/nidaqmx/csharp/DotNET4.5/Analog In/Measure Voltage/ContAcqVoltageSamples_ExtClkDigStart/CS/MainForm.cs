/******************************************************************************
*
* Example program:
*   ContAcqVoltageSamples_ExtClkDigStart
*
* Category:
*   AI
*
* Description:
*   This example demonstrates how to continuously acquire analog voltage data
*   using an external clock, started by a digital trigger.
*
* Instructions for running:
*   1.  Select the physical channel corresponding to where your signal is input
*       on the DAQ device.
*   2.  Enter the minimum and maximum voltage values.Note: For better accuracy,
*       try to match the input range to the expected voltage level of the
*       measured signal.
*   3.  Select the source for the external sample clock.
*   4.  Set the approximate rate of the sample clock. This allows the internal
*       characteristics of the acquisition to be as efficient as possible.
*   5.  Select the source for the digital edge start trigger.
*   6.  Select the edge, rising or falling, on which to trigger.
*
* Steps:
*   1.  Create a new analog input task.
*   2.  Create an analog input voltage channel and define the mode to be
*       continuous.
*   3.  Define the parameters for the external clock source.
*   4.  Define the parameters for the digital start trigger.
*   5.  Call AnalogMultiChannelReader.BeginReadWaveform to install a callback
*       and begin the asynchronous read operation.
*   6.  Inside the callback, call AnalogMultiChannelReader.EndReadWaveform to
*       retrieve the data from the read operation.  
*   7.  Call AnalogMultiChannelReader.BeginMemoryOptimizedReadWaveform
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
* I/O Connections Overview:
*   Make sure your signal input terminals match the physical channel text box. 
*   For more information on the input and output terminals for your device, open
*   the NI-DAQmx Help, and refer to the NI-DAQmx Device Terminals and Device
*   Considerations books in the table of contents.
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

using NationalInstruments.DAQmx;

namespace NationalInstruments.Examples.ContAcqVoltageSamples_ExtClkDigStart
{
    /// <summary>
    /// Summary description for MainForm.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.GroupBox channelParametersGroupBox;
        private System.Windows.Forms.Label maximumLabel;
        private System.Windows.Forms.Label minimumLabel;
        private System.Windows.Forms.Label physicalChannelLabel;
        private System.Windows.Forms.GroupBox timingParametersGroupBox;
        private System.Windows.Forms.Label rateLabel;
        private System.Windows.Forms.Label clockSourceLabel;
        private System.Windows.Forms.TextBox clockSourceTextBox;
        private System.Windows.Forms.GroupBox triggerParametersGroupBox;
        private System.Windows.Forms.ComboBox triggerEdgeComboBox;
        private System.Windows.Forms.Label triggerEdgeLabel;
        private System.Windows.Forms.Label triggerSourceLabel;
        private System.Windows.Forms.TextBox triggerSourceTextBox;

        private DigitalEdgeStartTriggerEdge triggerEdge = DigitalEdgeStartTriggerEdge.Rising;
        private AnalogMultiChannelReader analogInReader;
        private AnalogWaveform<double>[] data;


        private DataColumn[] dataColumn = null;
        private DataTable dataTable = null;
        private Task analogInTask;
        private Task runningTask;
        private AsyncCallback analogCallback;

        private System.Windows.Forms.GroupBox acquisitionResultsGroupBox;
        private System.Windows.Forms.Label resultLabel;
        private System.Windows.Forms.DataGrid acquisitionDataGrid;
        private System.Windows.Forms.NumericUpDown rateNumeric;
        internal System.Windows.Forms.NumericUpDown minimumValueNumeric;
        internal System.Windows.Forms.NumericUpDown maximumValueNumeric;
        private System.Windows.Forms.ComboBox physicalChannelComboBox;


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
            dataTable = new DataTable();

            //
            // TODO: Add any constructor code after InitializeComponent call
            stopButton.Enabled = false;
            triggerEdgeComboBox.SelectedIndex = 0;

            physicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AI, PhysicalChannelAccess.External));
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
                if (analogInTask != null)
                {
                    runningTask = null;
                    analogInTask.Dispose();
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainForm));
            this.channelParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.physicalChannelComboBox = new System.Windows.Forms.ComboBox();
            this.minimumValueNumeric = new System.Windows.Forms.NumericUpDown();
            this.maximumValueNumeric = new System.Windows.Forms.NumericUpDown();
            this.maximumLabel = new System.Windows.Forms.Label();
            this.minimumLabel = new System.Windows.Forms.Label();
            this.physicalChannelLabel = new System.Windows.Forms.Label();
            this.timingParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.rateNumeric = new System.Windows.Forms.NumericUpDown();
            this.rateLabel = new System.Windows.Forms.Label();
            this.clockSourceLabel = new System.Windows.Forms.Label();
            this.clockSourceTextBox = new System.Windows.Forms.TextBox();
            this.triggerParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.triggerEdgeComboBox = new System.Windows.Forms.ComboBox();
            this.triggerEdgeLabel = new System.Windows.Forms.Label();
            this.triggerSourceLabel = new System.Windows.Forms.Label();
            this.triggerSourceTextBox = new System.Windows.Forms.TextBox();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.acquisitionResultsGroupBox = new System.Windows.Forms.GroupBox();
            this.resultLabel = new System.Windows.Forms.Label();
            this.acquisitionDataGrid = new System.Windows.Forms.DataGrid();
            this.channelParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.minimumValueNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maximumValueNumeric)).BeginInit();
            this.timingParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rateNumeric)).BeginInit();
            this.triggerParametersGroupBox.SuspendLayout();
            this.acquisitionResultsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.acquisitionDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // channelParametersGroupBox
            // 
            this.channelParametersGroupBox.Controls.Add(this.physicalChannelComboBox);
            this.channelParametersGroupBox.Controls.Add(this.minimumValueNumeric);
            this.channelParametersGroupBox.Controls.Add(this.maximumValueNumeric);
            this.channelParametersGroupBox.Controls.Add(this.maximumLabel);
            this.channelParametersGroupBox.Controls.Add(this.minimumLabel);
            this.channelParametersGroupBox.Controls.Add(this.physicalChannelLabel);
            this.channelParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.channelParametersGroupBox.Location = new System.Drawing.Point(8, 8);
            this.channelParametersGroupBox.Name = "channelParametersGroupBox";
            this.channelParametersGroupBox.Size = new System.Drawing.Size(224, 120);
            this.channelParametersGroupBox.TabIndex = 2;
            this.channelParametersGroupBox.TabStop = false;
            this.channelParametersGroupBox.Text = "Channel Parameters";
            // 
            // physicalChannelComboBox
            // 
            this.physicalChannelComboBox.Location = new System.Drawing.Point(120, 24);
            this.physicalChannelComboBox.Name = "physicalChannelComboBox";
            this.physicalChannelComboBox.Size = new System.Drawing.Size(96, 21);
            this.physicalChannelComboBox.TabIndex = 1;
            this.physicalChannelComboBox.Text = "Dev1/ai0";
            // 
            // minimumValueNumeric
            // 
            this.minimumValueNumeric.DecimalPlaces = 2;
            this.minimumValueNumeric.Location = new System.Drawing.Point(120, 56);
            this.minimumValueNumeric.Maximum = new System.Decimal(new int[] {
                                                                                10,
                                                                                0,
                                                                                0,
                                                                                0});
            this.minimumValueNumeric.Minimum = new System.Decimal(new int[] {
                                                                                10,
                                                                                0,
                                                                                0,
                                                                                -2147483648});
            this.minimumValueNumeric.Name = "minimumValueNumeric";
            this.minimumValueNumeric.Size = new System.Drawing.Size(96, 20);
            this.minimumValueNumeric.TabIndex = 3;
            this.minimumValueNumeric.Value = new System.Decimal(new int[] {
                                                                              100,
                                                                              0,
                                                                              0,
                                                                              -2147418112});
            // 
            // maximumValueNumeric
            // 
            this.maximumValueNumeric.DecimalPlaces = 2;
            this.maximumValueNumeric.Location = new System.Drawing.Point(120, 88);
            this.maximumValueNumeric.Maximum = new System.Decimal(new int[] {
                                                                                10,
                                                                                0,
                                                                                0,
                                                                                0});
            this.maximumValueNumeric.Minimum = new System.Decimal(new int[] {
                                                                                10,
                                                                                0,
                                                                                0,
                                                                                -2147483648});
            this.maximumValueNumeric.Name = "maximumValueNumeric";
            this.maximumValueNumeric.Size = new System.Drawing.Size(96, 20);
            this.maximumValueNumeric.TabIndex = 5;
            this.maximumValueNumeric.Value = new System.Decimal(new int[] {
                                                                              100,
                                                                              0,
                                                                              0,
                                                                              65536});
            // 
            // maximumLabel
            // 
            this.maximumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.maximumLabel.Location = new System.Drawing.Point(8, 88);
            this.maximumLabel.Name = "maximumLabel";
            this.maximumLabel.Size = new System.Drawing.Size(112, 20);
            this.maximumLabel.TabIndex = 4;
            this.maximumLabel.Text = "Maximum Value (V):";
            // 
            // minimumLabel
            // 
            this.minimumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.minimumLabel.Location = new System.Drawing.Point(8, 56);
            this.minimumLabel.Name = "minimumLabel";
            this.minimumLabel.Size = new System.Drawing.Size(104, 20);
            this.minimumLabel.TabIndex = 2;
            this.minimumLabel.Text = "Minimum Value (V):";
            // 
            // physicalChannelLabel
            // 
            this.physicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.physicalChannelLabel.Location = new System.Drawing.Point(8, 26);
            this.physicalChannelLabel.Name = "physicalChannelLabel";
            this.physicalChannelLabel.Size = new System.Drawing.Size(96, 20);
            this.physicalChannelLabel.TabIndex = 0;
            this.physicalChannelLabel.Text = "Physical Channel:";
            // 
            // timingParametersGroupBox
            // 
            this.timingParametersGroupBox.Controls.Add(this.rateNumeric);
            this.timingParametersGroupBox.Controls.Add(this.rateLabel);
            this.timingParametersGroupBox.Controls.Add(this.clockSourceLabel);
            this.timingParametersGroupBox.Controls.Add(this.clockSourceTextBox);
            this.timingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.timingParametersGroupBox.Location = new System.Drawing.Point(8, 136);
            this.timingParametersGroupBox.Name = "timingParametersGroupBox";
            this.timingParametersGroupBox.Size = new System.Drawing.Size(224, 96);
            this.timingParametersGroupBox.TabIndex = 3;
            this.timingParametersGroupBox.TabStop = false;
            this.timingParametersGroupBox.Text = "Timing Parameters";
            // 
            // rateNumeric
            // 
            this.rateNumeric.DecimalPlaces = 2;
            this.rateNumeric.Location = new System.Drawing.Point(120, 64);
            this.rateNumeric.Maximum = new System.Decimal(new int[] {
                                                                        1000000,
                                                                        0,
                                                                        0,
                                                                        0});
            this.rateNumeric.Name = "rateNumeric";
            this.rateNumeric.Size = new System.Drawing.Size(96, 20);
            this.rateNumeric.TabIndex = 3;
            this.rateNumeric.Value = new System.Decimal(new int[] {
                                                                      10000,
                                                                      0,
                                                                      0,
                                                                      0});
            // 
            // rateLabel
            // 
            this.rateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rateLabel.Location = new System.Drawing.Point(8, 64);
            this.rateLabel.Name = "rateLabel";
            this.rateLabel.Size = new System.Drawing.Size(56, 16);
            this.rateLabel.TabIndex = 2;
            this.rateLabel.Text = "Rate (Hz):";
            // 
            // clockSourceLabel
            // 
            this.clockSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.clockSourceLabel.Location = new System.Drawing.Point(8, 26);
            this.clockSourceLabel.Name = "clockSourceLabel";
            this.clockSourceLabel.Size = new System.Drawing.Size(80, 16);
            this.clockSourceLabel.TabIndex = 0;
            this.clockSourceLabel.Text = "Clock Source:";
            // 
            // clockSourceTextBox
            // 
            this.clockSourceTextBox.Location = new System.Drawing.Point(120, 24);
            this.clockSourceTextBox.Name = "clockSourceTextBox";
            this.clockSourceTextBox.Size = new System.Drawing.Size(96, 20);
            this.clockSourceTextBox.TabIndex = 1;
            this.clockSourceTextBox.Text = "/Dev1/PFI7";
            // 
            // triggerParametersGroupBox
            // 
            this.triggerParametersGroupBox.Controls.Add(this.triggerEdgeComboBox);
            this.triggerParametersGroupBox.Controls.Add(this.triggerEdgeLabel);
            this.triggerParametersGroupBox.Controls.Add(this.triggerSourceLabel);
            this.triggerParametersGroupBox.Controls.Add(this.triggerSourceTextBox);
            this.triggerParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.triggerParametersGroupBox.Location = new System.Drawing.Point(8, 240);
            this.triggerParametersGroupBox.Name = "triggerParametersGroupBox";
            this.triggerParametersGroupBox.Size = new System.Drawing.Size(224, 88);
            this.triggerParametersGroupBox.TabIndex = 4;
            this.triggerParametersGroupBox.TabStop = false;
            this.triggerParametersGroupBox.Text = "Trigger Parameters";
            // 
            // triggerEdgeComboBox
            // 
            this.triggerEdgeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.triggerEdgeComboBox.Items.AddRange(new object[] {
                                                                     "Rising Edge",
                                                                     "Falling Edge"});
            this.triggerEdgeComboBox.Location = new System.Drawing.Point(120, 56);
            this.triggerEdgeComboBox.Name = "triggerEdgeComboBox";
            this.triggerEdgeComboBox.Size = new System.Drawing.Size(96, 21);
            this.triggerEdgeComboBox.TabIndex = 3;
            this.triggerEdgeComboBox.SelectedIndexChanged += new System.EventHandler(this.triggerEdgeComboBox_SelectedIndexChanged);
            // 
            // triggerEdgeLabel
            // 
            this.triggerEdgeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.triggerEdgeLabel.Location = new System.Drawing.Point(8, 58);
            this.triggerEdgeLabel.Name = "triggerEdgeLabel";
            this.triggerEdgeLabel.Size = new System.Drawing.Size(80, 16);
            this.triggerEdgeLabel.TabIndex = 2;
            this.triggerEdgeLabel.Text = "Trigger Edge:";
            // 
            // triggerSourceLabel
            // 
            this.triggerSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.triggerSourceLabel.Location = new System.Drawing.Point(8, 26);
            this.triggerSourceLabel.Name = "triggerSourceLabel";
            this.triggerSourceLabel.Size = new System.Drawing.Size(88, 16);
            this.triggerSourceLabel.TabIndex = 0;
            this.triggerSourceLabel.Text = "Trigger Source:";
            // 
            // triggerSourceTextBox
            // 
            this.triggerSourceTextBox.Location = new System.Drawing.Point(120, 24);
            this.triggerSourceTextBox.Name = "triggerSourceTextBox";
            this.triggerSourceTextBox.Size = new System.Drawing.Size(96, 20);
            this.triggerSourceTextBox.TabIndex = 1;
            this.triggerSourceTextBox.Text = "/Dev1/PFI0";
            // 
            // startButton
            // 
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.startButton.Location = new System.Drawing.Point(24, 496);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(80, 24);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.stopButton.Location = new System.Drawing.Point(136, 496);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(80, 24);
            this.stopButton.TabIndex = 1;
            this.stopButton.Text = "Stop";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // acquisitionResultsGroupBox
            // 
            this.acquisitionResultsGroupBox.Controls.Add(this.resultLabel);
            this.acquisitionResultsGroupBox.Controls.Add(this.acquisitionDataGrid);
            this.acquisitionResultsGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.acquisitionResultsGroupBox.Location = new System.Drawing.Point(8, 336);
            this.acquisitionResultsGroupBox.Name = "acquisitionResultsGroupBox";
            this.acquisitionResultsGroupBox.Size = new System.Drawing.Size(224, 152);
            this.acquisitionResultsGroupBox.TabIndex = 5;
            this.acquisitionResultsGroupBox.TabStop = false;
            this.acquisitionResultsGroupBox.Text = "Acquisition Results";
            // 
            // resultLabel
            // 
            this.resultLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.resultLabel.Location = new System.Drawing.Point(8, 16);
            this.resultLabel.Name = "resultLabel";
            this.resultLabel.Size = new System.Drawing.Size(112, 16);
            this.resultLabel.TabIndex = 0;
            this.resultLabel.Text = "Acquisition Data (V):";
            // 
            // acquisitionDataGrid
            // 
            this.acquisitionDataGrid.AllowSorting = false;
            this.acquisitionDataGrid.DataMember = "";
            this.acquisitionDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.acquisitionDataGrid.Location = new System.Drawing.Point(8, 32);
            this.acquisitionDataGrid.Name = "acquisitionDataGrid";
            this.acquisitionDataGrid.ParentRowsVisible = false;
            this.acquisitionDataGrid.ReadOnly = true;
            this.acquisitionDataGrid.Size = new System.Drawing.Size(208, 112);
            this.acquisitionDataGrid.TabIndex = 1;
            this.acquisitionDataGrid.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(242, 528);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.triggerParametersGroupBox);
            this.Controls.Add(this.timingParametersGroupBox);
            this.Controls.Add(this.channelParametersGroupBox);
            this.Controls.Add(this.acquisitionResultsGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Continuous External Clock Acquisition - DigStart";
            this.channelParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.minimumValueNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maximumValueNumeric)).EndInit();
            this.timingParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rateNumeric)).EndInit();
            this.triggerParametersGroupBox.ResumeLayout(false);
            this.acquisitionResultsGroupBox.ResumeLayout(false);
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
                try
                {
                    // Don't allow the user to press Start before stopping acquisition
                    stopButton.Enabled = true;
                    startButton.Enabled = false;

                    // Create a new task
                    analogInTask = new Task();

                    // Create a virtual channel
                    analogInTask.AIChannels.CreateVoltageChannel(physicalChannelComboBox.Text, "",
                        (AITerminalConfiguration)(-1), Convert.ToDouble(minimumValueNumeric.Value),
                        Convert.ToDouble(maximumValueNumeric.Value), AIVoltageUnits.Volts);

                    // Configure the timing parameters
                    analogInTask.Timing.ConfigureSampleClock(clockSourceTextBox.Text, Convert.ToDouble(rateNumeric.Value),
                         SampleClockActiveEdge.Rising, SampleQuantityMode.ContinuousSamples, 1000);

                    analogInTask.Triggers.StartTrigger.ConfigureDigitalEdgeTrigger(triggerSourceTextBox.Text,
                         triggerEdge);

                    // Verify the Task
                    analogInTask.Control(TaskAction.Verify);

                    // Prepare the table for Data
                    InitializeDataTable(analogInTask.AIChannels, ref dataTable);
                    acquisitionDataGrid.DataSource = dataTable;

                    analogInReader = new AnalogMultiChannelReader(analogInTask.Stream);
                    runningTask = analogInTask;
                    analogCallback = new AsyncCallback(AnalogInCallback);

                    // Use SynchronizeCallbacks to specify that the object 
                    // marshals callbacks across threads appropriately.
                    analogInReader.SynchronizeCallbacks = true;
                    analogInReader.BeginReadWaveform(Convert.ToInt32(rateNumeric.Value), analogCallback, analogInTask);
                }
                catch (DaqException ex)
                {
                    MessageBox.Show(ex.Message);
                    runningTask = null;
                    analogInTask.Dispose();
                    stopButton.Enabled = false;
                    startButton.Enabled = true;
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
                    data = analogInReader.EndReadWaveform(ar);

                    // Plot your data here
                    dataToDataTable(data, ref dataTable);

                    analogInReader.BeginMemoryOptimizedReadWaveform(Convert.ToInt32(rateNumeric.Value), analogCallback, analogInTask, data);
                }
            }
            catch (DaqException ex)
            {
                MessageBox.Show(ex.Message);
                runningTask = null;
                analogInTask.Dispose();
                stopButton.Enabled = false;
                startButton.Enabled = true;
            }
        }

        private void stopButton_Click(object sender, System.EventArgs e)
        {
            runningTask = null;
            analogInTask.Stop();
            analogInTask.Dispose();
            stopButton.Enabled = false;
            startButton.Enabled = true;
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

        private void triggerEdgeComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (triggerEdgeComboBox.SelectedIndex == 0)
            {
                triggerEdge = DigitalEdgeStartTriggerEdge.Rising;
            }
            else
            {
                triggerEdge = DigitalEdgeStartTriggerEdge.Falling;
            }
        }
    }
}
