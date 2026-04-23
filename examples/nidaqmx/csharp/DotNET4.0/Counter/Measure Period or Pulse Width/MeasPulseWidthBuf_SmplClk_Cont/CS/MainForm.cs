/******************************************************************************
*
* Example program:
*   MeasPulseWidthBuf_SmplClk_Cont
*
* Category:
*   CI
*
* Description:
*   This example demonstrates how to continually measure pulsewidths on a
*   Counter Input Channel 
*   using an external sampleclock. The Maximum and Minimum Values, Sample Clock
*   Source, andSamples per Channel 
*   are all configurable.This example shows how to measure pulse width on the
*   counter'sdefault input terminal 
*   (refer to section IV, I/O ConnectionsOverview, below for more information),
*   but could easily beexpanded 
*   to measure pulse width on any PFI, RTSI, or internalsignal.Note: For sample
*   clock measurements, an external 
*   sample clock isnecessary to signal when the counter should measure asample.
*   This is set by the Sample 
*   Clock Source control.
*
* Instructions for running:
*   1.  Select the Physical Channel which corresponds to the counter you want to
*       measure pulse width on the DAQ device.
*   2.  Enter the Maximum and Minimum Value to specify the range or your unknown
*       pulse width. Note:  It is important to set the Maximum and Minimum
*       Values of your unknown pulse width as accurately as possible so the best
*       internal timebase can be chosen to minimize measurement error.  The
*       default values specify a range that can be measured by the counter using
*       the 20MhzTimebase.  Use the Gen Dig Pulse Train-Continuous example to
*       verify that you are measuring correctly on the DAQ device.
*   3.  Set the Sample Clock Source and Samples per Channel to configure timing
*       for the measurement. Note:  An external sample clock must be used. 
*       Counters do not have an internal sample clock available.  You can use
*       the Gen Dig Pulse Train-Continuous example to generate a pulse train on
*       another counter and connect it to the Sample Clock Source you are using
*       in this example.
*
* Steps:
*   1.  Create a Task.
*   2.  Create a CIChannel object using CreatePulseWidthChannel.  The
*       edgeparameter is used to determine if the counter will measure high or
*       lowpulses.
*   3.  Call the Task.Timing.ConfigureSampleClock method to configurethe
*       external sample clock timing parameters such as SampleMode and Sample
*       Clock Source. The Sample Clock Sourcedetermines when a sample will be
*       inserted into the buffer.The Edge parameter can be used to determine
*       when a sample istaken.
*   4.  Create a CounterReader object and use
*       theCounterReader.BeginReadMultiSampleDouble method to read the data
*       andregister an asynchronous callback to be called when the requested
*       datais available.
*   5.  Inside the callback, call the CounterReader.EndReadMultiSampleDouble to
*       retrieve the data from the read.
*   6.  Call BeginReadMultiSampleDouble again insidethe callback to perform
*       another read.
*   7.  When the user presses the stop button, stop the task.
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
*   The counter will measure pulses on the input terminal of thecounter
*   specified in the Physical Channel I/O control.In this example the pulse
*   width will be measured on the defaultinput terminal on ctr0. The counter
*   will take measurements onvalid edges of the external Sample Clock Source.For
*   more information on the default counter input and outputterminals for your
*   device, open the NI-DAQmx Help, and refer toCounter Signal Connections found
*   under the Device Considerationsbook in the table of contents.
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

namespace NationalInstruments.Examples.MeasPulseWidthBuf_SmplClk_Cont
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.GroupBox acquisitionResultGroupBox;
        private System.Windows.Forms.Label resultLabel;
        private System.Windows.Forms.DataGrid acquisitionDataGrid;
        private System.Windows.Forms.GroupBox timingParametersGroupBox;
        private System.Windows.Forms.NumericUpDown rateNumeric;
        private System.Windows.Forms.Label samplesLabel;
        private System.Windows.Forms.Label rateLabel;
        private System.Windows.Forms.NumericUpDown samplesPerChannelNumeric;
        private System.Windows.Forms.GroupBox channelParametersGroupBox;
        internal System.Windows.Forms.NumericUpDown minimumValueNumeric;
        internal System.Windows.Forms.NumericUpDown maximumValueNumeric;
        private System.Windows.Forms.Label maximumLabel;
        private System.Windows.Forms.Label minimumLabel;
        private System.Windows.Forms.Label sampleClockSourceLabel;
        private System.Windows.Forms.TextBox sampleClockSourceTextBox;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button startButton;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private Task myTask, runningTask;
        private CounterReader myCounterReader;
        private AsyncCallback myCallBack;
        private double[] data;
        private int actualNumberOfSamplesRead;
        private DataColumn[] dataColumn = null;
        private System.Windows.Forms.ComboBox counterComboBox;
        private System.Windows.Forms.Label counterLabel;
        private DataTable dataTable=null;

        public MainForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            startButton.Enabled = false;
            stopButton.Enabled = false;
            dataTable = new DataTable();

            counterComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.CI, PhysicalChannelAccess.External));
            if (counterComboBox.Items.Count > 0)
            {
                counterComboBox.SelectedIndex = 0;
                startButton.Enabled = true;
            }
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
            this.acquisitionResultGroupBox = new System.Windows.Forms.GroupBox();
            this.resultLabel = new System.Windows.Forms.Label();
            this.acquisitionDataGrid = new System.Windows.Forms.DataGrid();
            this.timingParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.sampleClockSourceTextBox = new System.Windows.Forms.TextBox();
            this.sampleClockSourceLabel = new System.Windows.Forms.Label();
            this.rateNumeric = new System.Windows.Forms.NumericUpDown();
            this.samplesLabel = new System.Windows.Forms.Label();
            this.rateLabel = new System.Windows.Forms.Label();
            this.samplesPerChannelNumeric = new System.Windows.Forms.NumericUpDown();
            this.channelParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.counterComboBox = new System.Windows.Forms.ComboBox();
            this.minimumValueNumeric = new System.Windows.Forms.NumericUpDown();
            this.maximumValueNumeric = new System.Windows.Forms.NumericUpDown();
            this.maximumLabel = new System.Windows.Forms.Label();
            this.minimumLabel = new System.Windows.Forms.Label();
            this.counterLabel = new System.Windows.Forms.Label();
            this.stopButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.acquisitionResultGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.acquisitionDataGrid)).BeginInit();
            this.timingParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rateNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerChannelNumeric)).BeginInit();
            this.channelParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.minimumValueNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maximumValueNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // acquisitionResultGroupBox
            // 
            this.acquisitionResultGroupBox.Controls.Add(this.resultLabel);
            this.acquisitionResultGroupBox.Controls.Add(this.acquisitionDataGrid);
            this.acquisitionResultGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.acquisitionResultGroupBox.Location = new System.Drawing.Point(272, 8);
            this.acquisitionResultGroupBox.Name = "acquisitionResultGroupBox";
            this.acquisitionResultGroupBox.Size = new System.Drawing.Size(304, 288);
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
            this.resultLabel.Text = "Acquisition Data (sec):";
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
            this.acquisitionDataGrid.Size = new System.Drawing.Size(288, 248);
            this.acquisitionDataGrid.TabIndex = 1;
            this.acquisitionDataGrid.TabStop = false;
            // 
            // timingParametersGroupBox
            // 
            this.timingParametersGroupBox.Controls.Add(this.sampleClockSourceTextBox);
            this.timingParametersGroupBox.Controls.Add(this.sampleClockSourceLabel);
            this.timingParametersGroupBox.Controls.Add(this.rateNumeric);
            this.timingParametersGroupBox.Controls.Add(this.samplesLabel);
            this.timingParametersGroupBox.Controls.Add(this.rateLabel);
            this.timingParametersGroupBox.Controls.Add(this.samplesPerChannelNumeric);
            this.timingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.timingParametersGroupBox.Location = new System.Drawing.Point(8, 136);
            this.timingParametersGroupBox.Name = "timingParametersGroupBox";
            this.timingParametersGroupBox.Size = new System.Drawing.Size(256, 120);
            this.timingParametersGroupBox.TabIndex = 3;
            this.timingParametersGroupBox.TabStop = false;
            this.timingParametersGroupBox.Text = "Timing Parameters";
            // 
            // sampleClockSourceTextBox
            // 
            this.sampleClockSourceTextBox.Location = new System.Drawing.Point(152, 92);
            this.sampleClockSourceTextBox.Name = "sampleClockSourceTextBox";
            this.sampleClockSourceTextBox.Size = new System.Drawing.Size(96, 20);
            this.sampleClockSourceTextBox.TabIndex = 5;
            this.sampleClockSourceTextBox.Text = "/Dev1/PFI0";
            // 
            // sampleClockSourceLabel
            // 
            this.sampleClockSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.sampleClockSourceLabel.Location = new System.Drawing.Point(8, 96);
            this.sampleClockSourceLabel.Name = "sampleClockSourceLabel";
            this.sampleClockSourceLabel.Size = new System.Drawing.Size(112, 16);
            this.sampleClockSourceLabel.TabIndex = 4;
            this.sampleClockSourceLabel.Text = "Sample Clock Source:";
            // 
            // rateNumeric
            // 
            this.rateNumeric.DecimalPlaces = 2;
            this.rateNumeric.Location = new System.Drawing.Point(152, 56);
            this.rateNumeric.Maximum = new System.Decimal(new int[] {
                                                                        100000,
                                                                        0,
                                                                        0,
                                                                        0});
            this.rateNumeric.Name = "rateNumeric";
            this.rateNumeric.Size = new System.Drawing.Size(96, 20);
            this.rateNumeric.TabIndex = 3;
            this.rateNumeric.Value = new System.Decimal(new int[] {
                                                                      1000,
                                                                      0,
                                                                      0,
                                                                      0});
            // 
            // samplesLabel
            // 
            this.samplesLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.samplesLabel.Location = new System.Drawing.Point(8, 26);
            this.samplesLabel.Name = "samplesLabel";
            this.samplesLabel.Size = new System.Drawing.Size(104, 16);
            this.samplesLabel.TabIndex = 0;
            this.samplesLabel.Text = "Samples/Channel:";
            // 
            // rateLabel
            // 
            this.rateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rateLabel.Location = new System.Drawing.Point(8, 58);
            this.rateLabel.Name = "rateLabel";
            this.rateLabel.Size = new System.Drawing.Size(56, 16);
            this.rateLabel.TabIndex = 2;
            this.rateLabel.Text = "Rate (Hz):";
            // 
            // samplesPerChannelNumeric
            // 
            this.samplesPerChannelNumeric.Location = new System.Drawing.Point(152, 24);
            this.samplesPerChannelNumeric.Maximum = new System.Decimal(new int[] {
                                                                                     100000,
                                                                                     0,
                                                                                     0,
                                                                                     0});
            this.samplesPerChannelNumeric.Name = "samplesPerChannelNumeric";
            this.samplesPerChannelNumeric.Size = new System.Drawing.Size(96, 20);
            this.samplesPerChannelNumeric.TabIndex = 1;
            this.samplesPerChannelNumeric.Value = new System.Decimal(new int[] {
                                                                                   1000,
                                                                                   0,
                                                                                   0,
                                                                                   0});
            // 
            // channelParametersGroupBox
            // 
            this.channelParametersGroupBox.Controls.Add(this.counterComboBox);
            this.channelParametersGroupBox.Controls.Add(this.minimumValueNumeric);
            this.channelParametersGroupBox.Controls.Add(this.maximumValueNumeric);
            this.channelParametersGroupBox.Controls.Add(this.maximumLabel);
            this.channelParametersGroupBox.Controls.Add(this.minimumLabel);
            this.channelParametersGroupBox.Controls.Add(this.counterLabel);
            this.channelParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.channelParametersGroupBox.Location = new System.Drawing.Point(8, 8);
            this.channelParametersGroupBox.Name = "channelParametersGroupBox";
            this.channelParametersGroupBox.Size = new System.Drawing.Size(256, 120);
            this.channelParametersGroupBox.TabIndex = 2;
            this.channelParametersGroupBox.TabStop = false;
            this.channelParametersGroupBox.Text = "Channel Parameters";
            // 
            // counterComboBox
            // 
            this.counterComboBox.Location = new System.Drawing.Point(152, 24);
            this.counterComboBox.Name = "counterComboBox";
            this.counterComboBox.Size = new System.Drawing.Size(96, 21);
            this.counterComboBox.TabIndex = 1;
            this.counterComboBox.Text = "Dev1/ai0";
            // 
            // minimumValueNumeric
            // 
            this.minimumValueNumeric.DecimalPlaces = 9;
            this.minimumValueNumeric.Location = new System.Drawing.Point(152, 56);
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
                                                                              589824});
            // 
            // maximumValueNumeric
            // 
            this.maximumValueNumeric.DecimalPlaces = 9;
            this.maximumValueNumeric.Location = new System.Drawing.Point(152, 88);
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
                                                                              838860750,
                                                                              0,
                                                                              0,
                                                                              589824});
            // 
            // maximumLabel
            // 
            this.maximumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.maximumLabel.Location = new System.Drawing.Point(8, 88);
            this.maximumLabel.Name = "maximumLabel";
            this.maximumLabel.Size = new System.Drawing.Size(112, 16);
            this.maximumLabel.TabIndex = 4;
            this.maximumLabel.Text = "Maximum Value (sec):";
            // 
            // minimumLabel
            // 
            this.minimumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.minimumLabel.Location = new System.Drawing.Point(8, 56);
            this.minimumLabel.Name = "minimumLabel";
            this.minimumLabel.Size = new System.Drawing.Size(104, 15);
            this.minimumLabel.TabIndex = 2;
            this.minimumLabel.Text = "Minimum Value (sec):";
            // 
            // counterLabel
            // 
            this.counterLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.counterLabel.Location = new System.Drawing.Point(8, 26);
            this.counterLabel.Name = "counterLabel";
            this.counterLabel.Size = new System.Drawing.Size(96, 16);
            this.counterLabel.TabIndex = 0;
            this.counterLabel.Text = "Physical Channel:";
            // 
            // stopButton
            // 
            this.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.stopButton.Location = new System.Drawing.Point(152, 272);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(80, 24);
            this.stopButton.TabIndex = 1;
            this.stopButton.Text = "Stop";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // startButton
            // 
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.startButton.Location = new System.Drawing.Point(40, 272);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(80, 24);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(584, 302);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.acquisitionResultGroupBox);
            this.Controls.Add(this.timingParametersGroupBox);
            this.Controls.Add(this.channelParametersGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Measure Pulse Width Buffered - Sample Clock - Continuous";
            this.acquisitionResultGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.acquisitionDataGrid)).EndInit();
            this.timingParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rateNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerChannelNumeric)).EndInit();
            this.channelParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.minimumValueNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maximumValueNumeric)).EndInit();
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
            try
            {
                // Create channel and task
                myTask = new Task();

                myTask.CIChannels.CreatePulseWidthChannel(
                    counterComboBox.Text, 
                    "",
                    Convert.ToDouble(minimumValueNumeric.Value),
                    Convert.ToDouble(maximumValueNumeric.Value),
                    CIPulseWidthStartingEdge.Rising,
                    CIPulseWidthUnits.Seconds);

                // Configure the timing parameters
                myTask.Timing.ConfigureSampleClock(
                    sampleClockSourceTextBox.Text, 
                    Convert.ToDouble(rateNumeric.Value),
                    SampleClockActiveEdge.Rising,
                    SampleQuantityMode.ContinuousSamples);

                // Verify the task
                myTask.Control(TaskAction.Verify);

                // Initialize the data table
                InitializeDataTable(myTask.CIChannels, ref dataTable);
                acquisitionDataGrid.DataSource = dataTable;

                runningTask = myTask;
                myCounterReader = new CounterReader(myTask.Stream);

                // Use SynchronizeCallbacks to specify that the object 
                // marshals callbacks across threads appropriately.
                myCounterReader.SynchronizeCallbacks = true;

                myCallBack = new AsyncCallback(CounterInCallback);

                // Memory Optimized Read method needs an initialized array.
                data = new Double[Convert.ToInt32(samplesPerChannelNumeric.Value)];
                // Start the read async callback
                myCounterReader.BeginMemoryOptimizedReadMultiSampleDouble(
                    Convert.ToInt32(samplesPerChannelNumeric.Value),
                    myCallBack,
                    myTask,
                    data);

                startButton.Enabled = false;
                stopButton.Enabled = true;
            }
            catch (DaqException ex)
            {
                MessageBox.Show(ex.Message);
                myTask.Dispose();
                runningTask = null;
                stopButton.Enabled = false;
                startButton.Enabled = true;
            }
        }

        private void CounterInCallback(IAsyncResult ar)
        {
            try
            {
                if (runningTask != null && runningTask == ar.AsyncState)
                {
                    // Read the available data from the channel
                    data = myCounterReader.EndMemoryOptimizedReadMultiSampleDouble(ar, out actualNumberOfSamplesRead);

                    // Plot the data
                    dataToDataTable(data, ref dataTable);

                    // Start the read async callback
                    myCounterReader.BeginMemoryOptimizedReadMultiSampleDouble(
                        Convert.ToInt32(samplesPerChannelNumeric.Value),
                        myCallBack,
                        myTask,
                        data);
                }
            }
            catch (DaqException ex)
            {
                //Display Errors
                MessageBox.Show(ex.Message);
                myTask.Dispose();
                runningTask = null;
                stopButton.Enabled = false;
                startButton.Enabled = true;
            }
        }

        private void dataToDataTable(double[] sourceArray,ref DataTable dataTable)
        {   
            try
            {
                int dataCount = (sourceArray.Length<10)?sourceArray.Length:10;
                            
                for(int currentDataIndex = 0;currentDataIndex<dataCount;currentDataIndex++)             
                {
                    dataTable.Rows[currentDataIndex][0] = sourceArray[currentDataIndex];     
                }

            }
            catch(Exception e)
            {
                MessageBox.Show(e.TargetSite.ToString());
                runningTask = null;
                myTask.Dispose();
                stopButton.Enabled = false;
                startButton.Enabled = true;
            }
        }

        public void InitializeDataTable(CIChannelCollection channelCollection,ref DataTable data)
        {
            int numOfChannels= channelCollection.Count;
            data.Rows.Clear();
            data.Columns.Clear();
            dataColumn = new DataColumn[numOfChannels];
            int numOfRows= 10;

            for(int currentChannelIndex=0;currentChannelIndex<numOfChannels;currentChannelIndex++)
            {   
                dataColumn[currentChannelIndex] = new DataColumn();
                dataColumn[currentChannelIndex].DataType = typeof(double);
                dataColumn[currentChannelIndex].ColumnName=channelCollection[currentChannelIndex].PhysicalName;
            }

            data.Columns.AddRange(dataColumn); 

            for(int currentDataIndex = 0;currentDataIndex<numOfRows;currentDataIndex++)             
            {
                object[] rowArr = new object[numOfChannels];
                data.Rows.Add(rowArr);              
            }
        }

        private void stopButton_Click(object sender, System.EventArgs e)
        {
            runningTask = null;
            myTask.Dispose();
            stopButton.Enabled = false;
            startButton.Enabled = true;
        }
    }
}
