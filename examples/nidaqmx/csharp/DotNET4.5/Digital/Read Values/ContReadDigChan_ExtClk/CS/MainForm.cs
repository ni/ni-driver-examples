/******************************************************************************
*
* Example program:
*   ContReadDigChan_ExtClk
*
* Category:
*   DI
*
* Description:
*   This example demonstrates how to continuously read values from a digital
*   input channel using an external sample clock.
*
* Instructions for running:
*   1.  Select the physical channel on the DAQ device.
*   2.  Select the external clock source.
*   3.  Select the number of samples per channel.
*   4.  Select the sample clock rate.
*
* Steps:
*   1.  Create a new digital input task.
*   2.  Create the digital input channel.
*   3.  Configure the task to use an external sample clock.
*   4.  Set the stream's timeout to ten seconds.
*   5.  Create a DigitalSingleChannelReader and associate it with the task by
*       using the task's stream.
*   6.  Call DigitalSingleChannelReader.BeginReadWaveform to install a callback
*       and begin the asynchronous read operation.
*   7.  Inside the callback, call DigitalSingleChannelReader.EndReadWaveform to
*       retrieve the data from the read.
*   8.  Call DigitalSingleChannelReader.BeginReadWaveform again inside the
*       callback to perform another read operation.
*   9.  Dispose the Task object to clean-up any resources associated with the
*       task.
*   10. Handle any DaqExceptions, if they occur.
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
*   In this case wire your digital signals to the appropriate eight digital
*   lines on your DAQ Device.  For more information on the input and output
*   terminals for your device, open the NI-DAQmx Help, and refer to the NI-DAQmx
*   Device Terminals and Device Considerations books in the table of contents.
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

namespace NationalInstruments.Examples.ContReadDigChan_ExtClk
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        private NationalInstruments.DAQmx.Task myTask;
        private NationalInstruments.DAQmx.DigitalSingleChannelReader digitalReader;
        private AsyncCallback digitalCallback;
        private DataTable dataTable;
        private DataColumn[] dataColumn = null;
        private Task runningTask;

        private System.Windows.Forms.GroupBox channelParametersGroupBox;
        private System.Windows.Forms.NumericUpDown samplesPerChannelNumeric;
        private System.Windows.Forms.TextBox clockSourceTextBox;
        private System.Windows.Forms.Label clockSourceLabel;
        private System.Windows.Forms.Label physicalChannelLabel;
        private System.Windows.Forms.Label samplesPerChannelLabel;
        private System.Windows.Forms.NumericUpDown sampleRateNumeric;
        private System.Windows.Forms.Label sampleRateLabel;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.GroupBox resultsGroupBox;
        private System.Windows.Forms.DataGrid resultsDataGrid;
        private System.Windows.Forms.Button stopButton;
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

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            physicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DILine | PhysicalChannelTypes.DIPort, PhysicalChannelAccess.External));
            if (physicalChannelComboBox.Items.Count > 0)
                physicalChannelComboBox.SelectedIndex = 0;

            dataTable = new DataTable();
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
            this.channelParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.physicalChannelComboBox = new System.Windows.Forms.ComboBox();
            this.samplesPerChannelNumeric = new System.Windows.Forms.NumericUpDown();
            this.clockSourceTextBox = new System.Windows.Forms.TextBox();
            this.clockSourceLabel = new System.Windows.Forms.Label();
            this.physicalChannelLabel = new System.Windows.Forms.Label();
            this.samplesPerChannelLabel = new System.Windows.Forms.Label();
            this.sampleRateNumeric = new System.Windows.Forms.NumericUpDown();
            this.sampleRateLabel = new System.Windows.Forms.Label();
            this.startButton = new System.Windows.Forms.Button();
            this.resultsGroupBox = new System.Windows.Forms.GroupBox();
            this.resultsDataGrid = new System.Windows.Forms.DataGrid();
            this.stopButton = new System.Windows.Forms.Button();
            this.channelParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerChannelNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sampleRateNumeric)).BeginInit();
            this.resultsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resultsDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // channelParametersGroupBox
            // 
            this.channelParametersGroupBox.Controls.Add(this.physicalChannelComboBox);
            this.channelParametersGroupBox.Controls.Add(this.samplesPerChannelNumeric);
            this.channelParametersGroupBox.Controls.Add(this.clockSourceTextBox);
            this.channelParametersGroupBox.Controls.Add(this.clockSourceLabel);
            this.channelParametersGroupBox.Controls.Add(this.physicalChannelLabel);
            this.channelParametersGroupBox.Controls.Add(this.samplesPerChannelLabel);
            this.channelParametersGroupBox.Controls.Add(this.sampleRateNumeric);
            this.channelParametersGroupBox.Controls.Add(this.sampleRateLabel);
            this.channelParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.channelParametersGroupBox.Location = new System.Drawing.Point(8, 8);
            this.channelParametersGroupBox.Name = "channelParametersGroupBox";
            this.channelParametersGroupBox.Size = new System.Drawing.Size(312, 176);
            this.channelParametersGroupBox.TabIndex = 0;
            this.channelParametersGroupBox.TabStop = false;
            this.channelParametersGroupBox.Text = "Channel Parameters";
            // 
            // physicalChannelComboBox
            // 
            this.physicalChannelComboBox.Location = new System.Drawing.Point(144, 33);
            this.physicalChannelComboBox.Name = "physicalChannelComboBox";
            this.physicalChannelComboBox.Size = new System.Drawing.Size(152, 21);
            this.physicalChannelComboBox.TabIndex = 1;
            this.physicalChannelComboBox.Text = "Dev1/port0";
            // 
            // samplesPerChannelNumeric
            // 
            this.samplesPerChannelNumeric.Location = new System.Drawing.Point(144, 105);
            this.samplesPerChannelNumeric.Maximum = new System.Decimal(new int[] {
                                                                                     100000,
                                                                                     0,
                                                                                     0,
                                                                                     0});
            this.samplesPerChannelNumeric.Minimum = new System.Decimal(new int[] {
                                                                                     1,
                                                                                     0,
                                                                                     0,
                                                                                     0});
            this.samplesPerChannelNumeric.Name = "samplesPerChannelNumeric";
            this.samplesPerChannelNumeric.Size = new System.Drawing.Size(152, 20);
            this.samplesPerChannelNumeric.TabIndex = 5;
            this.samplesPerChannelNumeric.Value = new System.Decimal(new int[] {
                                                                                   1000,
                                                                                   0,
                                                                                   0,
                                                                                   0});
            // 
            // clockSourceTextBox
            // 
            this.clockSourceTextBox.Location = new System.Drawing.Point(144, 69);
            this.clockSourceTextBox.Name = "clockSourceTextBox";
            this.clockSourceTextBox.Size = new System.Drawing.Size(152, 20);
            this.clockSourceTextBox.TabIndex = 3;
            this.clockSourceTextBox.Text = "/Dev1/PFI0";
            // 
            // clockSourceLabel
            // 
            this.clockSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.clockSourceLabel.Location = new System.Drawing.Point(16, 68);
            this.clockSourceLabel.Name = "clockSourceLabel";
            this.clockSourceLabel.TabIndex = 2;
            this.clockSourceLabel.Text = "Clock Source:";
            // 
            // physicalChannelLabel
            // 
            this.physicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.physicalChannelLabel.Location = new System.Drawing.Point(16, 32);
            this.physicalChannelLabel.Name = "physicalChannelLabel";
            this.physicalChannelLabel.TabIndex = 0;
            this.physicalChannelLabel.Text = "Physical Channel:";
            // 
            // samplesPerChannelLabel
            // 
            this.samplesPerChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.samplesPerChannelLabel.Location = new System.Drawing.Point(16, 104);
            this.samplesPerChannelLabel.Name = "samplesPerChannelLabel";
            this.samplesPerChannelLabel.Size = new System.Drawing.Size(120, 23);
            this.samplesPerChannelLabel.TabIndex = 4;
            this.samplesPerChannelLabel.Text = "Samples per Channel:";
            // 
            // sampleRateNumeric
            // 
            this.sampleRateNumeric.DecimalPlaces = 2;
            this.sampleRateNumeric.Location = new System.Drawing.Point(144, 141);
            this.sampleRateNumeric.Maximum = new System.Decimal(new int[] {
                                                                              100000,
                                                                              0,
                                                                              0,
                                                                              0});
            this.sampleRateNumeric.Minimum = new System.Decimal(new int[] {
                                                                              1,
                                                                              0,
                                                                              0,
                                                                              0});
            this.sampleRateNumeric.Name = "sampleRateNumeric";
            this.sampleRateNumeric.Size = new System.Drawing.Size(152, 20);
            this.sampleRateNumeric.TabIndex = 7;
            this.sampleRateNumeric.Value = new System.Decimal(new int[] {
                                                                            1000,
                                                                            0,
                                                                            0,
                                                                            0});
            // 
            // sampleRateLabel
            // 
            this.sampleRateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.sampleRateLabel.Location = new System.Drawing.Point(16, 140);
            this.sampleRateLabel.Name = "sampleRateLabel";
            this.sampleRateLabel.Size = new System.Drawing.Size(120, 23);
            this.sampleRateLabel.TabIndex = 6;
            this.sampleRateLabel.Text = "Sample Rate (Hz):";
            // 
            // startButton
            // 
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.startButton.Location = new System.Drawing.Point(64, 264);
            this.startButton.Name = "startButton";
            this.startButton.TabIndex = 1;
            this.startButton.Text = "Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // resultsGroupBox
            // 
            this.resultsGroupBox.Controls.Add(this.resultsDataGrid);
            this.resultsGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.resultsGroupBox.Location = new System.Drawing.Point(328, 8);
            this.resultsGroupBox.Name = "resultsGroupBox";
            this.resultsGroupBox.Size = new System.Drawing.Size(304, 288);
            this.resultsGroupBox.TabIndex = 3;
            this.resultsGroupBox.TabStop = false;
            this.resultsGroupBox.Text = "Results";
            // 
            // resultsDataGrid
            // 
            this.resultsDataGrid.AllowSorting = false;
            this.resultsDataGrid.DataMember = "";
            this.resultsDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.resultsDataGrid.Location = new System.Drawing.Point(3, 16);
            this.resultsDataGrid.Name = "resultsDataGrid";
            this.resultsDataGrid.PreferredColumnWidth = 125;
            this.resultsDataGrid.ReadOnly = true;
            this.resultsDataGrid.Size = new System.Drawing.Size(293, 264);
            this.resultsDataGrid.TabIndex = 0;
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.stopButton.Location = new System.Drawing.Point(168, 264);
            this.stopButton.Name = "stopButton";
            this.stopButton.TabIndex = 2;
            this.stopButton.Text = "Stop";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(632, 302);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.resultsGroupBox);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.channelParametersGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Continuous Read Digital Channel - External Clock";
            this.channelParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerChannelNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sampleRateNumeric)).EndInit();
            this.resultsGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.resultsDataGrid)).EndInit();
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
                    // Create the task
                    myTask = new Task();

                    // Create the channel
                    myTask.DIChannels.CreateChannel(physicalChannelComboBox.Text, "", ChannelLineGrouping.OneChannelForAllLines);

                    // Configure the external clock
                    myTask.Timing.ConfigureSampleClock(clockSourceTextBox.Text,
                        Convert.ToDouble(sampleRateNumeric.Value),
                        SampleClockActiveEdge.Rising,
                        SampleQuantityMode.ContinuousSamples, Convert.ToInt32(samplesPerChannelNumeric.Value));

                    // Set timeout to 10 s
                    myTask.Stream.Timeout = 10000;

                    // Verify the Task
                    myTask.Control(TaskAction.Verify);

                    // Set up the data table
                    InitializeDataTable(ref dataTable);
                    resultsDataGrid.DataSource = dataTable;

                    // Start running the task
                    StartTask();

                    // Create the analog input sound reader
                    digitalReader = new DigitalSingleChannelReader(myTask.Stream);

                    // Use SynchronizeCallbacks to specify that the object 
                    // marshals callbacks across threads appropriately.
                    digitalReader.SynchronizeCallbacks = true;

                    // Set up our first callback
                    digitalCallback = new AsyncCallback(DigitalCallback);

                    digitalReader.BeginReadWaveform(Convert.ToInt32(samplesPerChannelNumeric.Value), digitalCallback, myTask);
                }
                catch (DaqException exception)
                {
                    // Display Errors
                    MessageBox.Show(exception.Message);
                    StopTask();
                }
            }
        }

        private void DigitalCallback(IAsyncResult ar)
        {
            try
            {
                if (runningTask != null && runningTask == ar.AsyncState)
                {
                    // Read the available data from the channels
                    DigitalWaveform waveform = digitalReader.EndReadWaveform(ar);

                    //populate data table
                    dataToDataTable(waveform, ref dataTable);

                    // Set up a new callback
                    digitalReader.BeginReadWaveform(Convert.ToInt32(samplesPerChannelNumeric.Value), digitalCallback, myTask);
                }
            }
            catch (DaqException exception)
            {
                // Display Errors
                MessageBox.Show(exception.Message);
                StopTask();
            }
        }

        private void dataToDataTable(DigitalWaveform waveform, ref DataTable dataTable)
        {
            // Iterate over channels
            int currentLineIndex = 0;
            foreach (DigitalWaveformSignal signal in waveform.Signals)
            {
                int currentSampleIndex = 0;
                foreach (DigitalState sample in signal.States)
                {
                    if (currentSampleIndex == 10)
                    {
                        break;
                    }
                    if (sample == DigitalState.ForceUp)
                    {
                        dataTable.Rows[currentSampleIndex][currentLineIndex] = 1;
                    }
                    else
                    {
                        dataTable.Rows[currentSampleIndex][currentLineIndex] = 0;
                    }
                    currentSampleIndex++;
                }
                currentLineIndex++;
            }
        }

        private void InitializeDataTable(ref DataTable data)
        {
            int numOfLines = Convert.ToInt32(myTask.DIChannels[0].NumberOfLines);
            data.Rows.Clear();
            data.Columns.Clear();
            dataColumn = new DataColumn[numOfLines];
            int numOfRows = 10;

            for (int currentLineIndex = 0; currentLineIndex < numOfLines; currentLineIndex++)
            {
                int channelNumber = currentLineIndex + 1;
                dataColumn[currentLineIndex] = new DataColumn();
                dataColumn[currentLineIndex].DataType = typeof(int);
                dataColumn[currentLineIndex].ColumnName = "Channel " + channelNumber.ToString();
            }
            data.Columns.AddRange(dataColumn);

            for (int currentDataIndex = 0; currentDataIndex < numOfRows; currentDataIndex++)
            {
                object[] rowArr = new object[numOfLines];
                data.Rows.Add(rowArr);
            }
        }

        private void stopButton_Click(object sender, System.EventArgs e)
        {
            if (runningTask != null)
            {
                StopTask();
            }
        }

        private void StartTask()
        {
            runningTask = myTask;

            physicalChannelComboBox.Enabled = false;
            clockSourceTextBox.Enabled = false;
            samplesPerChannelNumeric.Enabled = false;
            sampleRateNumeric.Enabled = false;

            startButton.Enabled = false;
            stopButton.Enabled = true;

            this.Refresh();
        }

        private void StopTask()
        {
            runningTask = null;
            myTask.Dispose();

            physicalChannelComboBox.Enabled = true;
            clockSourceTextBox.Enabled = true;
            samplesPerChannelNumeric.Enabled = true;
            sampleRateNumeric.Enabled = true;

            startButton.Enabled = true;
            stopButton.Enabled = false;
        }
    }
}
