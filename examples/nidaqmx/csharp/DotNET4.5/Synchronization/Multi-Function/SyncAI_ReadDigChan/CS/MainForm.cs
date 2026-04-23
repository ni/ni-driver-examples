/******************************************************************************
*
* Example program:
*   MultiFunctionSyncAI_ReadDigChan
*
* Category:
*   Synchronization
*
* Description:
*   This example demonstrates how to continuously acquire analog and digital
*   data at the same time, synchronized with one another on the same device.
*
* Instructions for running:
*   1.  Select the physical channel to correspond to where your analog signal is
*       input on the DAQ device.
*   2.  Select the channel to correspond to where your digital signal is input
*       on the DAQ device.
*   3.  Enter the minimum and maximum voltage ranges.Note:  For better accuracy
*       try to match the input range to the expected voltage level of the
*       measured signal.
*   4.  Set the sample rate of the acquisition.Note:  The rate should be at
*       least twice as fast as the maximum frequency component of the signal
*       being acquired.  Note:  This example requires two DMA channels to run. 
*       If your hardware does not support two DMA channels, you need to set the
*       DataTransferMechanism property for the digital input task to use
*       interrupts.  The DataTransferMechanism property is accessible via the
*       DIChannel class. Refer to your device documentation to determine how
*       many DMA channels are supported for your hardware.
*
* Steps:
*   1.  Create an analog input voltage channel and a digital input channel.
*   2.  Set the rate for the sample clocks. Additionally, define the sample
*       modes to be continuous.
*   3.  Set the source of the digital task's sample clock to the sample clock of
*       the analog task.
*   4.  Call Task.Start() on each task to start the acquisition and
*       generation.Note: The digital input task must start before the analog
*       input task to ensure that both tasks start at the same time.
*   5.  Create an AnalogMultiChannelReader and associate it with the analog
*       input task by using the task's stream. Call
*       AnalogMultiChannelReader.BeginReadWaveform to install a callback and
*       begin the asynchronous read operation.
*   6.  Create an DigitalMultiChannelReader and associate it with the digital
*       input task by using the task's stream. Call
*       DigitalMultiChannelReader.BeginReadWaveform to install a callback and
*       begin the asynchronous read operation.
*   7.  Inside the callbacks, read the data and display it.
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
*   Make sure your signal input terminals match the Physical Channel I/O
*   controls.
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

namespace NationalInstruments.Examples.SyncAI_ReadDigChan
{
    /// <summary>
    /// Summary description for MainForm.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        private DataTable inputDataTable = null;
        private DataColumn[] inputDataColumns = null;
        private Task analogTask;
        private Task digitalTask;
        private AnalogMultiChannelReader analogReader;
        private DigitalMultiChannelReader digitalReader;
        private AsyncCallback analogCallback;
        private AsyncCallback digitalCallback;
        private Task runningAnalogTask;
        private Task runningDigitalTask;
        private int samples;
        private int aCount;
        private int dCount;

        private System.Windows.Forms.GroupBox timingGroupBox;
        private System.Windows.Forms.NumericUpDown rateNumeric;
        private System.Windows.Forms.Label rateLabel;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.ComboBox analogInputComboBox;
        private System.Windows.Forms.NumericUpDown inputMinValNumeric;
        private System.Windows.Forms.Label analogChannelLabel;
        private System.Windows.Forms.Label inputMaxValLabel;
        private System.Windows.Forms.Label inputMinValLabel;
        private System.Windows.Forms.NumericUpDown inputMaxValNumeric;
        private System.Windows.Forms.Label samplesLabel;
        private System.Windows.Forms.NumericUpDown samplesNumeric;
        private System.Windows.Forms.ComboBox digitalInputComboBox;
        private System.Windows.Forms.Label digitalChannelLabel;
        private System.Windows.Forms.GroupBox inputDataGroupBox;
        private System.Windows.Forms.DataGrid inputDataGrid;
        private System.Windows.Forms.GroupBox analogInputGroupBox;
        private System.Windows.Forms.GroupBox digitalInputGroupBox;
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
            analogInputComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AI, PhysicalChannelAccess.External));
            digitalInputComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DOLine, PhysicalChannelAccess.External));

            if (analogInputComboBox.Items.Count > 0)
                analogInputComboBox.SelectedIndex = 0;
            if (digitalInputComboBox.Items.Count > 0)
                digitalInputComboBox.SelectedIndex = 0;

            if (analogInputComboBox.Items.Count > 0 && digitalInputComboBox.Items.Count > 0)
                startButton.Enabled = true;
            
            // Set up the data table
            inputDataTable = new DataTable();

            inputDataGrid.DataSource = inputDataTable;
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
                if (analogTask != null)
                {
                    runningAnalogTask = null;
                    analogTask.Dispose();
                }
                if (digitalTask != null)
                {
                    runningDigitalTask = null;
                    digitalTask.Dispose();
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
            this.analogInputGroupBox = new System.Windows.Forms.GroupBox();
            this.analogInputComboBox = new System.Windows.Forms.ComboBox();
            this.inputMinValNumeric = new System.Windows.Forms.NumericUpDown();
            this.analogChannelLabel = new System.Windows.Forms.Label();
            this.inputMaxValLabel = new System.Windows.Forms.Label();
            this.inputMinValLabel = new System.Windows.Forms.Label();
            this.inputMaxValNumeric = new System.Windows.Forms.NumericUpDown();
            this.timingGroupBox = new System.Windows.Forms.GroupBox();
            this.rateNumeric = new System.Windows.Forms.NumericUpDown();
            this.samplesLabel = new System.Windows.Forms.Label();
            this.rateLabel = new System.Windows.Forms.Label();
            this.samplesNumeric = new System.Windows.Forms.NumericUpDown();
            this.startButton = new System.Windows.Forms.Button();
            this.digitalInputGroupBox = new System.Windows.Forms.GroupBox();
            this.digitalInputComboBox = new System.Windows.Forms.ComboBox();
            this.digitalChannelLabel = new System.Windows.Forms.Label();
            this.stopButton = new System.Windows.Forms.Button();
            this.inputDataGroupBox = new System.Windows.Forms.GroupBox();
            this.inputDataGrid = new System.Windows.Forms.DataGrid();
            this.analogInputGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inputMinValNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inputMaxValNumeric)).BeginInit();
            this.timingGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rateNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesNumeric)).BeginInit();
            this.digitalInputGroupBox.SuspendLayout();
            this.inputDataGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inputDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // analogInputGroupBox
            // 
            this.analogInputGroupBox.Controls.Add(this.analogInputComboBox);
            this.analogInputGroupBox.Controls.Add(this.inputMinValNumeric);
            this.analogInputGroupBox.Controls.Add(this.analogChannelLabel);
            this.analogInputGroupBox.Controls.Add(this.inputMaxValLabel);
            this.analogInputGroupBox.Controls.Add(this.inputMinValLabel);
            this.analogInputGroupBox.Controls.Add(this.inputMaxValNumeric);
            this.analogInputGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.analogInputGroupBox.Location = new System.Drawing.Point(8, 8);
            this.analogInputGroupBox.Name = "analogInputGroupBox";
            this.analogInputGroupBox.Size = new System.Drawing.Size(328, 112);
            this.analogInputGroupBox.TabIndex = 0;
            this.analogInputGroupBox.TabStop = false;
            this.analogInputGroupBox.Text = "Channel Parameters - Analog Input";
            // 
            // analogInputComboBox
            // 
            this.analogInputComboBox.Location = new System.Drawing.Point(152, 24);
            this.analogInputComboBox.Name = "analogInputComboBox";
            this.analogInputComboBox.Size = new System.Drawing.Size(168, 21);
            this.analogInputComboBox.TabIndex = 1;
            this.analogInputComboBox.Text = "Dev1/ai0";
            // 
            // inputMinValNumeric
            // 
            this.inputMinValNumeric.DecimalPlaces = 2;
            this.inputMinValNumeric.Location = new System.Drawing.Point(152, 80);
            this.inputMinValNumeric.Minimum = new System.Decimal(new int[] {
                                                                               100,
                                                                               0,
                                                                               0,
                                                                               -2147483648});
            this.inputMinValNumeric.Name = "inputMinValNumeric";
            this.inputMinValNumeric.Size = new System.Drawing.Size(168, 20);
            this.inputMinValNumeric.TabIndex = 5;
            this.inputMinValNumeric.Value = new System.Decimal(new int[] {
                                                                             10,
                                                                             0,
                                                                             0,
                                                                             -2147483648});
            // 
            // analogChannelLabel
            // 
            this.analogChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.analogChannelLabel.Location = new System.Drawing.Point(16, 26);
            this.analogChannelLabel.Name = "analogChannelLabel";
            this.analogChannelLabel.Size = new System.Drawing.Size(120, 16);
            this.analogChannelLabel.TabIndex = 0;
            this.analogChannelLabel.Text = "Analog Input Channels:";
            // 
            // inputMaxValLabel
            // 
            this.inputMaxValLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.inputMaxValLabel.Location = new System.Drawing.Point(16, 54);
            this.inputMaxValLabel.Name = "inputMaxValLabel";
            this.inputMaxValLabel.Size = new System.Drawing.Size(96, 16);
            this.inputMaxValLabel.TabIndex = 2;
            this.inputMaxValLabel.Text = "Maximum Value:";
            // 
            // inputMinValLabel
            // 
            this.inputMinValLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.inputMinValLabel.Location = new System.Drawing.Point(16, 82);
            this.inputMinValLabel.Name = "inputMinValLabel";
            this.inputMinValLabel.Size = new System.Drawing.Size(96, 16);
            this.inputMinValLabel.TabIndex = 4;
            this.inputMinValLabel.Text = "Minimum Value:";
            // 
            // inputMaxValNumeric
            // 
            this.inputMaxValNumeric.DecimalPlaces = 2;
            this.inputMaxValNumeric.Location = new System.Drawing.Point(152, 52);
            this.inputMaxValNumeric.Name = "inputMaxValNumeric";
            this.inputMaxValNumeric.Size = new System.Drawing.Size(168, 20);
            this.inputMaxValNumeric.TabIndex = 3;
            this.inputMaxValNumeric.Value = new System.Decimal(new int[] {
                                                                             10,
                                                                             0,
                                                                             0,
                                                                             0});
            // 
            // timingGroupBox
            // 
            this.timingGroupBox.Controls.Add(this.rateNumeric);
            this.timingGroupBox.Controls.Add(this.samplesLabel);
            this.timingGroupBox.Controls.Add(this.rateLabel);
            this.timingGroupBox.Controls.Add(this.samplesNumeric);
            this.timingGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.timingGroupBox.Location = new System.Drawing.Point(8, 194);
            this.timingGroupBox.Name = "timingGroupBox";
            this.timingGroupBox.Size = new System.Drawing.Size(328, 88);
            this.timingGroupBox.TabIndex = 2;
            this.timingGroupBox.TabStop = false;
            this.timingGroupBox.Text = "Timing Parameters";
            // 
            // rateNumeric
            // 
            this.rateNumeric.DecimalPlaces = 2;
            this.rateNumeric.Location = new System.Drawing.Point(152, 24);
            this.rateNumeric.Maximum = new System.Decimal(new int[] {
                                                                        102400,
                                                                        0,
                                                                        0,
                                                                        0});
            this.rateNumeric.Name = "rateNumeric";
            this.rateNumeric.Size = new System.Drawing.Size(168, 20);
            this.rateNumeric.TabIndex = 1;
            this.rateNumeric.Value = new System.Decimal(new int[] {
                                                                      10000,
                                                                      0,
                                                                      0,
                                                                      0});
            // 
            // samplesLabel
            // 
            this.samplesLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.samplesLabel.Location = new System.Drawing.Point(16, 54);
            this.samplesLabel.Name = "samplesLabel";
            this.samplesLabel.Size = new System.Drawing.Size(120, 16);
            this.samplesLabel.TabIndex = 2;
            this.samplesLabel.Text = "Samples to Read:";
            // 
            // rateLabel
            // 
            this.rateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rateLabel.Location = new System.Drawing.Point(16, 26);
            this.rateLabel.Name = "rateLabel";
            this.rateLabel.Size = new System.Drawing.Size(96, 16);
            this.rateLabel.TabIndex = 0;
            this.rateLabel.Text = "Sample Rate (Hz):";
            // 
            // samplesNumeric
            // 
            this.samplesNumeric.Location = new System.Drawing.Point(152, 52);
            this.samplesNumeric.Maximum = new System.Decimal(new int[] {
                                                                           1000,
                                                                           0,
                                                                           0,
                                                                           0});
            this.samplesNumeric.Name = "samplesNumeric";
            this.samplesNumeric.Size = new System.Drawing.Size(168, 20);
            this.samplesNumeric.TabIndex = 3;
            this.samplesNumeric.Value = new System.Decimal(new int[] {
                                                                         1000,
                                                                         0,
                                                                         0,
                                                                         0});
            // 
            // startButton
            // 
            this.startButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.startButton.Enabled = false;
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.startButton.Location = new System.Drawing.Point(347, 288);
            this.startButton.Name = "startButton";
            this.startButton.TabIndex = 4;
            this.startButton.Text = "Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // digitalInputGroupBox
            // 
            this.digitalInputGroupBox.Controls.Add(this.digitalInputComboBox);
            this.digitalInputGroupBox.Controls.Add(this.digitalChannelLabel);
            this.digitalInputGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.digitalInputGroupBox.Location = new System.Drawing.Point(8, 128);
            this.digitalInputGroupBox.Name = "digitalInputGroupBox";
            this.digitalInputGroupBox.Size = new System.Drawing.Size(328, 60);
            this.digitalInputGroupBox.TabIndex = 1;
            this.digitalInputGroupBox.TabStop = false;
            this.digitalInputGroupBox.Text = "Channel Parameters - Digital Input";
            // 
            // digitalInputComboBox
            // 
            this.digitalInputComboBox.Location = new System.Drawing.Point(152, 24);
            this.digitalInputComboBox.Name = "digitalInputComboBox";
            this.digitalInputComboBox.Size = new System.Drawing.Size(168, 21);
            this.digitalInputComboBox.TabIndex = 1;
            this.digitalInputComboBox.Text = "Dev1/port0";
            // 
            // digitalChannelLabel
            // 
            this.digitalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.digitalChannelLabel.Location = new System.Drawing.Point(16, 26);
            this.digitalChannelLabel.Name = "digitalChannelLabel";
            this.digitalChannelLabel.Size = new System.Drawing.Size(120, 16);
            this.digitalChannelLabel.TabIndex = 0;
            this.digitalChannelLabel.Text = "Digital Input Channels:";
            // 
            // stopButton
            // 
            this.stopButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.stopButton.Enabled = false;
            this.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.stopButton.Location = new System.Drawing.Point(428, 288);
            this.stopButton.Name = "stopButton";
            this.stopButton.TabIndex = 5;
            this.stopButton.Text = "Stop";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // inputDataGroupBox
            // 
            this.inputDataGroupBox.Controls.Add(this.inputDataGrid);
            this.inputDataGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.inputDataGroupBox.Location = new System.Drawing.Point(344, 8);
            this.inputDataGroupBox.Name = "inputDataGroupBox";
            this.inputDataGroupBox.Size = new System.Drawing.Size(390, 274);
            this.inputDataGroupBox.TabIndex = 3;
            this.inputDataGroupBox.TabStop = false;
            this.inputDataGroupBox.Text = "Input Data";
            // 
            // inputDataGrid
            // 
            this.inputDataGrid.AllowSorting = false;
            this.inputDataGrid.DataMember = "";
            this.inputDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.inputDataGrid.Location = new System.Drawing.Point(3, 16);
            this.inputDataGrid.Name = "inputDataGrid";
            this.inputDataGrid.PreferredColumnWidth = 100;
            this.inputDataGrid.ReadOnly = true;
            this.inputDataGrid.Size = new System.Drawing.Size(381, 242);
            this.inputDataGrid.TabIndex = 0;
            this.inputDataGrid.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(746, 319);
            this.Controls.Add(this.inputDataGroupBox);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.analogInputGroupBox);
            this.Controls.Add(this.timingGroupBox);
            this.Controls.Add(this.digitalInputGroupBox);
            this.Controls.Add(this.stopButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(754, 353);
            this.Name = "MainForm";
            this.Text = "Multi-Function Synchronization - Analog and Digital Input";
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.analogInputGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.inputMinValNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inputMaxValNumeric)).EndInit();
            this.timingGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rateNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesNumeric)).EndInit();
            this.digitalInputGroupBox.ResumeLayout(false);
            this.inputDataGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.inputDataGrid)).EndInit();
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

        private void MainForm_Resize(object sender, EventArgs e)
        {
            inputDataGroupBox.Width = this.Width - 362;
            inputDataGroupBox.Height = this.Height - 79;
            inputDataGrid.Width = this.Width - 373;
            inputDataGrid.Height = this.Height - 111;
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
            inputDataTable.Rows.Clear();
            inputDataTable.Columns.Clear();

            // Get the number of columns
            aCount = analogTask.AIChannels.Count;
            dCount = digitalTask.DIChannels.Count;

            // Add one column of type double
            inputDataColumns = new DataColumn[aCount + dCount];

            // Add analog columns
            int i = 0;
            for (; i < aCount; i++)
            {
                inputDataColumns[i] = new DataColumn();
                inputDataColumns[i].DataType = typeof(double);
                inputDataColumns[i].ColumnName = analogTask.AIChannels[i].PhysicalName;
            }

            for (; i < aCount + dCount; i++)
            {
                inputDataColumns[i] = new DataColumn();
                inputDataColumns[i].DataType = typeof(bool);
                inputDataColumns[i].ColumnName = digitalTask.DIChannels[i - aCount].PhysicalName;
            }

            inputDataTable.Columns.AddRange(inputDataColumns);

            // Now add a certain number of rows
            for(i = 0; i < rows; i++)             
            {
                object[] rowArr = new object[aCount + dCount];
                inputDataTable.Rows.Add(rowArr);
            }
        }

        private void startButton_Click(object sender, System.EventArgs e)
        {
            // Change the mouse to an hourglass for the duration of this function.
            Cursor.Current = Cursors.WaitCursor;

            // Read UI selections
            samples = Convert.ToInt32(samplesNumeric.Value);

            try
            {
                // Create the master and slave tasks
                analogTask = new Task("analogTask");
                digitalTask = new Task("digitalTask");

                // Configure both tasks with the values selected on the UI.
                analogTask.AIChannels.CreateVoltageChannel(analogInputComboBox.Text,
                    "",
                    AITerminalConfiguration.Differential,
                    Convert.ToDouble(inputMinValNumeric.Value),
                    Convert.ToDouble(inputMaxValNumeric.Value),
                    AIVoltageUnits.Volts);

                digitalTask.DIChannels.CreateChannel(digitalInputComboBox.Text,
                    "",
                    ChannelLineGrouping.OneChannelForEachLine);

                // Set up the timing for the first task
                analogTask.Timing.ConfigureSampleClock("",
                    Convert.ToDouble(rateNumeric.Value),
                    SampleClockActiveEdge.Rising,
                    SampleQuantityMode.ContinuousSamples,
                    samples);

                // Use the same timebase for the second task
                string deviceName = analogInputComboBox.Text.Split('/')[0];
                string terminalNameBase = "/" + GetDeviceName(deviceName) + "/";

                digitalTask.Timing.ConfigureSampleClock(terminalNameBase + "ai/SampleClock",
                    Convert.ToDouble(rateNumeric.Value),
                    SampleClockActiveEdge.Rising,
                    SampleQuantityMode.ContinuousSamples,
                    Convert.ToInt32(samplesNumeric.Value));

                // Verify the tasks
                analogTask.Control(TaskAction.Verify);
                digitalTask.Control(TaskAction.Verify);

                // Set up the data table
                InitializeDataTables(Math.Min((inputDataGrid.Height - 50) / 17, samples)); 

                // Officially start the task
                StartTask();

                digitalTask.Start();
                analogTask.Start();

                // Start reading as well
                analogCallback = new AsyncCallback(AnalogRead);
                analogReader = new AnalogMultiChannelReader(analogTask.Stream);

                digitalCallback = new AsyncCallback(DigitalRead);
                digitalReader = new DigitalMultiChannelReader(digitalTask.Stream);

                // Use SynchronizeCallbacks to specify that the object 
                // marshals callbacks across threads appropriately.
                analogReader.SynchronizeCallbacks = true;
                digitalReader.SynchronizeCallbacks = true;
                
                analogReader.BeginReadMultiSample(samples, analogCallback, analogTask);
                digitalReader.BeginReadWaveform(samples, digitalCallback, digitalTask);
            }
            catch (Exception ex)
            {
                StopTask();
                MessageBox.Show(ex.Message);
            }
        }

        private void AnalogRead(IAsyncResult ar)
        {
            try
            {
                if (runningAnalogTask != null && runningAnalogTask == ar.AsyncState)
                {
                    // Read the data
                    double[,] data = analogReader.EndReadMultiSample(ar);

                    // Display the data
                    for (int i = 0; i < inputDataTable.Rows.Count && i < data.GetLength(1); i++)
                    {
                        for (int j = 0; j < data.GetLength(0); j++)
                        {
                            inputDataTable.Rows[i][j] = data[j,i];
                        }
                    }

                    // Set up next callback
                    analogReader.BeginReadMultiSample(samples, analogCallback, analogTask);
                }
            }
            catch (Exception ex)
            {
                StopTask();
                MessageBox.Show(ex.Message);
            }
        }

        private void DigitalRead(IAsyncResult ar)
        {
            try
            {
                if (runningDigitalTask != null && runningDigitalTask == ar.AsyncState)
                {
                    // Read the data
                    DigitalWaveform[] data = digitalReader.EndReadWaveform(ar);

                    // Display the data
                    for (int i = 0; i < inputDataTable.Rows.Count && i < data[0].Samples.Count; i++)
                    {
                        for (int j = 0; j < data.GetLength(0); j++)
                        {
                            inputDataTable.Rows[i][aCount + j] = data[j].Samples[i].States[0];
                        }
                    }

                    // Set up next callback
                    digitalReader.BeginReadWaveform(samples, digitalCallback, digitalTask);
                }
            }
            catch (Exception ex)
            {
                StopTask();
                MessageBox.Show(ex.Message);
            }
        }

        private void stopButton_Click(object sender, System.EventArgs e)
        {
            StopTask();
        }

        private void StartTask()
        {
            if (runningAnalogTask == null)
            {
                // Change state
                runningAnalogTask = analogTask;
                runningDigitalTask = digitalTask;

                // Fix UI
                analogInputComboBox.Enabled = false;
                inputMinValNumeric.Enabled = false;
                inputMaxValNumeric.Enabled = false;
            
                digitalInputComboBox.Enabled = false;
                digitalInputComboBox.Enabled = false;
                digitalInputComboBox.Enabled = false;

                rateNumeric.Enabled = false;
                samplesNumeric.Enabled = false;
            
                startButton.Enabled = false;
                stopButton.Enabled = true;
            }
        }

        private void StopTask()
        {
            // Change state
            runningAnalogTask = null;
            runningDigitalTask = null;

            // Fix UI
            analogInputComboBox.Enabled = true;
            inputMinValNumeric.Enabled = true;
            inputMaxValNumeric.Enabled = true;
            
            digitalInputComboBox.Enabled = true;
            digitalInputComboBox.Enabled = true;
            digitalInputComboBox.Enabled = true;

            rateNumeric.Enabled = true;
            samplesNumeric.Enabled = true;
            
            startButton.Enabled = true;
            stopButton.Enabled = false;
        
            // Stop tasks
            analogTask.Stop();
            digitalTask.Stop();

            analogTask.Dispose();
            digitalTask.Dispose();
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
