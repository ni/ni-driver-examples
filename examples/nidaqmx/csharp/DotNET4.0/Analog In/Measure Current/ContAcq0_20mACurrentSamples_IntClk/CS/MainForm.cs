/******************************************************************************
*
* Example program:
*   ContAcq0_20mACurrentSamples_IntClk
*
* Category:
*   AI
*
* Description:
*   This example demonstrates how to continuously measure current using an
*   internal hardware clock for timing.
*
* Instructions for running:
*   1.  Select the physical channel to correspond to where your signal is input
*       on the DAQ device.
*   2.  Enter the minimum and maximum current ranges, in Amps.Note:  For better
*       accuracy try to match the input ranges to the expected current level of
*       the measured signal.
*   3.  Set the rate of the acquisition.  Higher values will result in faster
*       updates, approximately corresponding to samples per second.  Also, set
*       the number of samples to read at a time.
*   4.  Enter in the parameters of your current shunt resistor.  The shunt
*       resistor location will usually be "External" unless you are using an
*       SCXI current input terminal block or SCC current input module.  The
*       shunt resistor value should correspond to the shunt resistor that you
*       are using, and is specified in ohms.  If you are using an SCXI current
*       input terminal block or SCC current input module, you must select
*       "Internal" for the shunt resistor location.
*
* Steps:
*   1.  Create a Task object. Create an AI channel object by using the
*       CreateCurrentChannel method.
*   2.  Configure the clock by using the Timing.ConfigureSampleClock method.
*   3.  Create the AnalogMultiChannelReader object.
*   4.  Call AnalogMultiChannelReader.BeginReadWaveform to install a callback
*       and begin the asynchronous read operation.Inside the callback, call
*       AnalogMultiChannelReader.ReadWaveform to retrieve the data from the read
*       operation.  
*   5.  The timeout is set to 10 seconds by default.
*   6.  Call AnalogMultiChannelReader.BeginMemoryOptimizedReadWaveform
*   7.  Call Task.Stop() to stop the task.  Dispose the Task object to clean-up
*       any resources associated with the task.
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
*   Make sure your signal input terminal matches the physical channel control. 
*   If you are using an external shunt resistor, make sure to hook it up in
*   parallel with the current signal you are trying to measure.  For more
*   information on the input and output terminals for your device, open the
*   NI-DAQmx Help, and refer to the NI-DAQmx Device Terminals and Device
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

namespace NationalInstruments.Examples.ContAcq0_20mACurrentSamples_IntClk
{
    /// <summary>
    /// Summary description for MainForm.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {

        private Task myTask;
        private Task runningTask;
        private AnalogMultiChannelReader myAnalogReader;
        private AnalogWaveform<double>[] data;
        private AsyncCallback myAsyncCallback;
        private DataColumn[] dataColumn = null;
        private DataTable dataTable = null;

        internal System.Windows.Forms.GroupBox timingParametersGroupbox;
        internal System.Windows.Forms.NumericUpDown rateNumeric;
        internal System.Windows.Forms.Label sampleRateLabel;
        internal System.Windows.Forms.Label samplesToReadLabel;
        internal System.Windows.Forms.NumericUpDown samplesToReadNumeric;
        internal System.Windows.Forms.Button stopButton;
        internal System.Windows.Forms.GroupBox currentParametersGroupBox;
        internal System.Windows.Forms.NumericUpDown shuntResistorNumeric;
        internal System.Windows.Forms.ComboBox shuntResistorLocationComboxBox;
        internal System.Windows.Forms.Label shuntResistorLocationLabel;
        internal System.Windows.Forms.Label shuntResistorLabel;
        internal System.Windows.Forms.GroupBox channelParametersGroupBox;
        internal System.Windows.Forms.Label physicalChannelLabel;
        internal System.Windows.Forms.Label maximumValueLabel;
        internal System.Windows.Forms.Label minimumValueLabel;
        internal System.Windows.Forms.Button startButton;
        internal System.Windows.Forms.GroupBox acquisitionResultsGroupBox;
        internal System.Windows.Forms.DataGrid acquisitionDataGrid;
        internal System.Windows.Forms.Label resultLabel;
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
            shuntResistorLocationComboxBox.SelectedIndex=1;
            dataTable= new DataTable();

            physicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AI, PhysicalChannelAccess.External));
            if (physicalChannelComboBox.Items.Count > 0)
                physicalChannelComboBox.SelectedIndex = 0;
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainForm));
            this.timingParametersGroupbox = new System.Windows.Forms.GroupBox();
            this.rateNumeric = new System.Windows.Forms.NumericUpDown();
            this.sampleRateLabel = new System.Windows.Forms.Label();
            this.samplesToReadLabel = new System.Windows.Forms.Label();
            this.samplesToReadNumeric = new System.Windows.Forms.NumericUpDown();
            this.stopButton = new System.Windows.Forms.Button();
            this.currentParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.shuntResistorNumeric = new System.Windows.Forms.NumericUpDown();
            this.shuntResistorLocationComboxBox = new System.Windows.Forms.ComboBox();
            this.shuntResistorLocationLabel = new System.Windows.Forms.Label();
            this.shuntResistorLabel = new System.Windows.Forms.Label();
            this.channelParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.physicalChannelComboBox = new System.Windows.Forms.ComboBox();
            this.minimumValueNumeric = new System.Windows.Forms.NumericUpDown();
            this.maximumValueNumeric = new System.Windows.Forms.NumericUpDown();
            this.physicalChannelLabel = new System.Windows.Forms.Label();
            this.maximumValueLabel = new System.Windows.Forms.Label();
            this.minimumValueLabel = new System.Windows.Forms.Label();
            this.startButton = new System.Windows.Forms.Button();
            this.acquisitionResultsGroupBox = new System.Windows.Forms.GroupBox();
            this.acquisitionDataGrid = new System.Windows.Forms.DataGrid();
            this.resultLabel = new System.Windows.Forms.Label();
            this.timingParametersGroupbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rateNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesToReadNumeric)).BeginInit();
            this.currentParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shuntResistorNumeric)).BeginInit();
            this.channelParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.minimumValueNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maximumValueNumeric)).BeginInit();
            this.acquisitionResultsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.acquisitionDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // timingParametersGroupbox
            // 
            this.timingParametersGroupbox.Controls.Add(this.rateNumeric);
            this.timingParametersGroupbox.Controls.Add(this.sampleRateLabel);
            this.timingParametersGroupbox.Controls.Add(this.samplesToReadLabel);
            this.timingParametersGroupbox.Controls.Add(this.samplesToReadNumeric);
            this.timingParametersGroupbox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.timingParametersGroupbox.Location = new System.Drawing.Point(9, 136);
            this.timingParametersGroupbox.Name = "timingParametersGroupbox";
            this.timingParametersGroupbox.Size = new System.Drawing.Size(232, 88);
            this.timingParametersGroupbox.TabIndex = 3;
            this.timingParametersGroupbox.TabStop = false;
            this.timingParametersGroupbox.Text = "Timing Parameters";
            // 
            // rateNumeric
            // 
            this.rateNumeric.DecimalPlaces = 2;
            this.rateNumeric.Location = new System.Drawing.Point(144, 24);
            this.rateNumeric.Maximum = new System.Decimal(new int[] {
                                                                        100000,
                                                                        0,
                                                                        0,
                                                                        0});
            this.rateNumeric.Name = "rateNumeric";
            this.rateNumeric.Size = new System.Drawing.Size(80, 20);
            this.rateNumeric.TabIndex = 1;
            this.rateNumeric.Value = new System.Decimal(new int[] {
                                                                      1000,
                                                                      0,
                                                                      0,
                                                                      0});
            // 
            // sampleRateLabel
            // 
            this.sampleRateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.sampleRateLabel.Location = new System.Drawing.Point(16, 26);
            this.sampleRateLabel.Name = "sampleRateLabel";
            this.sampleRateLabel.Size = new System.Drawing.Size(100, 16);
            this.sampleRateLabel.TabIndex = 0;
            this.sampleRateLabel.Text = "Sample Rate (Hz):";
            // 
            // samplesToReadLabel
            // 
            this.samplesToReadLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.samplesToReadLabel.Location = new System.Drawing.Point(16, 58);
            this.samplesToReadLabel.Name = "samplesToReadLabel";
            this.samplesToReadLabel.Size = new System.Drawing.Size(112, 16);
            this.samplesToReadLabel.TabIndex = 2;
            this.samplesToReadLabel.Text = "Samples to Read:";
            // 
            // samplesToReadNumeric
            // 
            this.samplesToReadNumeric.Location = new System.Drawing.Point(144, 56);
            this.samplesToReadNumeric.Maximum = new System.Decimal(new int[] {
                                                                                 100000,
                                                                                 0,
                                                                                 0,
                                                                                 0});
            this.samplesToReadNumeric.Name = "samplesToReadNumeric";
            this.samplesToReadNumeric.Size = new System.Drawing.Size(80, 20);
            this.samplesToReadNumeric.TabIndex = 3;
            this.samplesToReadNumeric.Value = new System.Decimal(new int[] {
                                                                               1000,
                                                                               0,
                                                                               0,
                                                                               0});
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.stopButton.Location = new System.Drawing.Point(377, 264);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(80, 24);
            this.stopButton.TabIndex = 1;
            this.stopButton.Text = "Stop";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // currentParametersGroupBox
            // 
            this.currentParametersGroupBox.Controls.Add(this.shuntResistorNumeric);
            this.currentParametersGroupBox.Controls.Add(this.shuntResistorLocationComboxBox);
            this.currentParametersGroupBox.Controls.Add(this.shuntResistorLocationLabel);
            this.currentParametersGroupBox.Controls.Add(this.shuntResistorLabel);
            this.currentParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.currentParametersGroupBox.Location = new System.Drawing.Point(9, 240);
            this.currentParametersGroupBox.Name = "currentParametersGroupBox";
            this.currentParametersGroupBox.Size = new System.Drawing.Size(232, 88);
            this.currentParametersGroupBox.TabIndex = 4;
            this.currentParametersGroupBox.TabStop = false;
            this.currentParametersGroupBox.Text = "Current Parameters";
            // 
            // shuntResistorNumeric
            // 
            this.shuntResistorNumeric.DecimalPlaces = 2;
            this.shuntResistorNumeric.Location = new System.Drawing.Point(144, 56);
            this.shuntResistorNumeric.Maximum = new System.Decimal(new int[] {
                                                                                 100000,
                                                                                 0,
                                                                                 0,
                                                                                 0});
            this.shuntResistorNumeric.Name = "shuntResistorNumeric";
            this.shuntResistorNumeric.Size = new System.Drawing.Size(80, 20);
            this.shuntResistorNumeric.TabIndex = 3;
            this.shuntResistorNumeric.Value = new System.Decimal(new int[] {
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
            this.shuntResistorLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.shuntResistorLabel.Location = new System.Drawing.Point(16, 56);
            this.shuntResistorLabel.Name = "shuntResistorLabel";
            this.shuntResistorLabel.Size = new System.Drawing.Size(144, 16);
            this.shuntResistorLabel.TabIndex = 2;
            this.shuntResistorLabel.Text = "Shunt Resistor (Ohms):";
            // 
            // channelParametersGroupBox
            // 
            this.channelParametersGroupBox.Controls.Add(this.physicalChannelComboBox);
            this.channelParametersGroupBox.Controls.Add(this.minimumValueNumeric);
            this.channelParametersGroupBox.Controls.Add(this.maximumValueNumeric);
            this.channelParametersGroupBox.Controls.Add(this.physicalChannelLabel);
            this.channelParametersGroupBox.Controls.Add(this.maximumValueLabel);
            this.channelParametersGroupBox.Controls.Add(this.minimumValueLabel);
            this.channelParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.channelParametersGroupBox.Location = new System.Drawing.Point(9, 8);
            this.channelParametersGroupBox.Name = "channelParametersGroupBox";
            this.channelParametersGroupBox.Size = new System.Drawing.Size(232, 120);
            this.channelParametersGroupBox.TabIndex = 2;
            this.channelParametersGroupBox.TabStop = false;
            this.channelParametersGroupBox.Text = "Channel Parameters";
            // 
            // physicalChannelComboBox
            // 
            this.physicalChannelComboBox.Location = new System.Drawing.Point(144, 24);
            this.physicalChannelComboBox.Name = "physicalChannelComboBox";
            this.physicalChannelComboBox.Size = new System.Drawing.Size(80, 21);
            this.physicalChannelComboBox.TabIndex = 1;
            this.physicalChannelComboBox.Text = "Dev1/ai0";
            // 
            // minimumValueNumeric
            // 
            this.minimumValueNumeric.DecimalPlaces = 3;
            this.minimumValueNumeric.Location = new System.Drawing.Point(144, 88);
            this.minimumValueNumeric.Name = "minimumValueNumeric";
            this.minimumValueNumeric.Size = new System.Drawing.Size(80, 20);
            this.minimumValueNumeric.TabIndex = 5;
            // 
            // maximumValueNumeric
            // 
            this.maximumValueNumeric.DecimalPlaces = 3;
            this.maximumValueNumeric.Location = new System.Drawing.Point(144, 56);
            this.maximumValueNumeric.Name = "maximumValueNumeric";
            this.maximumValueNumeric.Size = new System.Drawing.Size(80, 20);
            this.maximumValueNumeric.TabIndex = 3;
            this.maximumValueNumeric.Value = new System.Decimal(new int[] {
                                                                              20,
                                                                              0,
                                                                              0,
                                                                              196608});
            // 
            // physicalChannelLabel
            // 
            this.physicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.physicalChannelLabel.Location = new System.Drawing.Point(16, 26);
            this.physicalChannelLabel.Name = "physicalChannelLabel";
            this.physicalChannelLabel.Size = new System.Drawing.Size(100, 16);
            this.physicalChannelLabel.TabIndex = 0;
            this.physicalChannelLabel.Text = "Physical Channel:";
            // 
            // maximumValueLabel
            // 
            this.maximumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.maximumValueLabel.Location = new System.Drawing.Point(16, 58);
            this.maximumValueLabel.Name = "maximumValueLabel";
            this.maximumValueLabel.Size = new System.Drawing.Size(112, 16);
            this.maximumValueLabel.TabIndex = 2;
            this.maximumValueLabel.Text = "Maximum Value (A):";
            // 
            // minimumValueLabel
            // 
            this.minimumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.minimumValueLabel.Location = new System.Drawing.Point(16, 90);
            this.minimumValueLabel.Name = "minimumValueLabel";
            this.minimumValueLabel.Size = new System.Drawing.Size(104, 16);
            this.minimumValueLabel.TabIndex = 4;
            this.minimumValueLabel.Text = "Minimum Value (A):";
            // 
            // startButton
            // 
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.startButton.Location = new System.Drawing.Point(281, 264);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(80, 24);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // acquisitionResultsGroupBox
            // 
            this.acquisitionResultsGroupBox.Controls.Add(this.acquisitionDataGrid);
            this.acquisitionResultsGroupBox.Controls.Add(this.resultLabel);
            this.acquisitionResultsGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.acquisitionResultsGroupBox.Location = new System.Drawing.Point(249, 8);
            this.acquisitionResultsGroupBox.Name = "acquisitionResultsGroupBox";
            this.acquisitionResultsGroupBox.Size = new System.Drawing.Size(240, 240);
            this.acquisitionResultsGroupBox.TabIndex = 5;
            this.acquisitionResultsGroupBox.TabStop = false;
            this.acquisitionResultsGroupBox.Text = "Acquisition Results";
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
            this.acquisitionDataGrid.Size = new System.Drawing.Size(224, 200);
            this.acquisitionDataGrid.TabIndex = 1;
            this.acquisitionDataGrid.TabStop = false;
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
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(498, 336);
            this.Controls.Add(this.timingParametersGroupbox);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.currentParametersGroupBox);
            this.Controls.Add(this.channelParametersGroupBox);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.acquisitionResultsGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Continuous Acquire 0 to 20 mA Current Samples - Internal Clock";
            this.timingParametersGroupbox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rateNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesToReadNumeric)).EndInit();
            this.currentParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.shuntResistorNumeric)).EndInit();
            this.channelParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.minimumValueNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maximumValueNumeric)).EndInit();
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

        private void stopButton_Click(object sender, System.EventArgs e)
        {
            runningTask = null;
            myTask.Dispose();
            startButton.Enabled=true;
            stopButton.Enabled=false;
        }

        private void startButton_Click(object sender, System.EventArgs e)
        {
            try
            {
                // Create a new task
                myTask= new Task();
            
                switch(shuntResistorLocationComboxBox.SelectedItem.ToString())
                {
                    case "Internal":
                        // Create a virtual channel
                        myTask.AIChannels.CreateCurrentChannel(physicalChannelComboBox.Text,"",
                            (AITerminalConfiguration)(-1), Convert.ToDouble(minimumValueNumeric.Value),
                            Convert.ToDouble(maximumValueNumeric.Value), AICurrentUnits.Amps);
                        break;
                    case "External":
                        // Create a virtual channel
                        myTask.AIChannels.CreateCurrentChannel(physicalChannelComboBox.Text,"", 
                            (AITerminalConfiguration)(-1), Convert.ToDouble(minimumValueNumeric.Value),
                            Convert.ToDouble(maximumValueNumeric.Value), Convert.ToDouble(shuntResistorNumeric.Value),
                            AICurrentUnits.Amps);
                        break;
                }
            
                myTask.Timing.ConfigureSampleClock("",Convert.ToDouble(samplesToReadNumeric.Value),
                    SampleClockActiveEdge.Rising,SampleQuantityMode.ContinuousSamples, 1000);
                
                // Verify the Task
                myTask.Control(TaskAction.Verify);
                
                // Prepare the table for Data
                InitializeDataTable(myTask.AIChannels,ref dataTable);  
                acquisitionDataGrid.DataSource=dataTable;

                runningTask = myTask;
                myAnalogReader = new AnalogMultiChannelReader(myTask.Stream);
                myAsyncCallback = new AsyncCallback(AnalogInCallback);

                // Use SynchronizeCallbacks to specify that the object 
                // marshals callbacks across threads appropriately.
                myAnalogReader.SynchronizeCallbacks = true;
                myAnalogReader.BeginReadWaveform(Convert.ToInt32(samplesToReadNumeric.Value), myAsyncCallback,
                    myTask);

                startButton.Enabled = false;
                stopButton.Enabled = true;               
            
            }
            catch(DaqException exception)
            {
                // Display Errors
                MessageBox.Show(exception.Message);
                myTask.Dispose();
                startButton.Enabled=true;
                stopButton.Enabled=false;
                runningTask = null;
            }
       
        }

        private void AnalogInCallback(IAsyncResult ar)
        {
            try
            {
                if (runningTask != null && runningTask == ar.AsyncState)
                {
                    // Read the available data from the channels
                    data = myAnalogReader.EndReadWaveform(ar);

                    // Plot your data here
                    dataToDataTable(data, ref dataTable);

                    myAnalogReader.BeginMemoryOptimizedReadWaveform(Convert.ToInt32(samplesToReadNumeric.Value), myAsyncCallback, myTask, data);
                }
            }
            catch(DaqException exception)
            {
                // Display Errors
                MessageBox.Show(exception.Message);
                myTask.Dispose();
                runningTask = null;
                startButton.Enabled=true;
                stopButton.Enabled=false;
            }

        }

        private void shuntResistorLocationComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            switch(shuntResistorLocationComboxBox.SelectedItem.ToString())
            {
                case "Internal": 
                    shuntResistorNumeric.Enabled=false;
                    break;
                case "External":
                default: 
                    shuntResistorNumeric.Enabled=true;
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

        public void InitializeDataTable(AIChannelCollection channelCollection,ref DataTable data)
        {
            int numOfChannels = channelCollection.Count;
            data.Rows.Clear();
            data.Columns.Clear();
            dataColumn = new DataColumn[numOfChannels];
            int numOfRows = 10;

            for(int currentChannelIndex=0;currentChannelIndex<numOfChannels;currentChannelIndex++)
            {   
                dataColumn[currentChannelIndex] = new DataColumn();
                dataColumn[currentChannelIndex].DataType = typeof(double);
                dataColumn[currentChannelIndex].ColumnName=channelCollection[currentChannelIndex].PhysicalName;
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
