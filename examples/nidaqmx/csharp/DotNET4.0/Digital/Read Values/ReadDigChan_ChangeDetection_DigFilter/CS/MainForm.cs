/******************************************************************************
*
* Example program:
*   ReadDigChan_ChangeDetection_DigFilter
*
* Category:
*   DIO
*
* Description:
*   This example demonstrates how to acquire filtered digital input via change
*   detection and digital filtering.
*
* Instructions for running:
*   1.  Select the physical lines for unfiltered digital input.
*   2.  Select the physical lines for filtered digital input.Note: The
*       unfiltered and filtered lines should not be the same.
*   3.  Select the physical lines for rising-edge change detection.
*   4.  Select the physical lines for falling-edge change detection on.Note: You
*       can enable one, both, or neither kinds of detection on each line.
*   5.  Set the minimum pulse width for the filter.
*   6.  Set the number of samples to acquire per channel per read.
*   7.  Set the read timeout, in seconds.
*
* Steps:
*   1.  Create a new digital input task.
*   2.  Create two digital input channels, one for filtered digital input and
*       one for unfiltered.
*   3.  Configure the channels for digital filtering enabled or disabled.
*   4.  Set up change detection on specific lines for digital input.
*   5.  Create a DigitalMultiChannelReader and associate it with the task by
*       using the task's stream. Call
*       DigitalMultiChannelReader.BeginReadSingleSampleMultiLine to install a
*       callback and begin the asynchronous read operation.
*   6.  Inside the callback, call
*       DigitalMultiChannelReader.EndReadSingleSampleMultiLine to retrieve the
*       data from the read.
*   7.  Call DigitalMultiChannelReader.BeginReadSingleSampleMultiLine again
*       inside the callback to perform another read operation.
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
*   Make sure your signal input terminals match the filtered and unfiltered
*   lines.  In this case wire your digital signals to the first eight digital
*   lines on your DAQ Device.  For more information on the input and output
*   terminals for your device, open the NI-DAQmx Help, and refer to the NI-DAQmx
*   Device Terminals and Device Considerations books in the table of
*   contents.NOTE: For NI-6534 devices, either 32 bytes of data needs to be
*   transferred first for the DMA transfer to take place, or interrupts must be
*   used instead of DMA.
*
* Recommended Use:
*   Create a Task object. Create the appropriate DIChannel object and configure
*   its parameters.  Configure the Timing parameters by using the Timing object.
*   Read the data by using the AnalogMultiChannelReader object. Use the
*   appropriate BeginRead method to read the data asynchronously. Dispose the
*   Task object to clean-up any resources associated with the task.
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
using System.Runtime.InteropServices;

namespace NationalInstruments.Examples.ReadDigChan_ChangeDetection_DigFilter
{
    /// <summary>
    /// Summary description for MainForm.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        private Task myTask;
        private DigitalMultiChannelReader digitalReader;
        private AsyncCallback digitalCallback;
        private DataTable dataTable;
        private Task runningTask;

        private System.Windows.Forms.GroupBox resultsGroup;
        private System.Windows.Forms.Label pulseWidthLabel;
        private System.Windows.Forms.NumericUpDown pulseWidthNumeric;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Label risingEdgeLabel;
        private System.Windows.Forms.Label fallingEdgeLabel;
        private System.Windows.Forms.GroupBox lineGroup;
        private System.Windows.Forms.NumericUpDown timeoutNumeric;
        private System.Windows.Forms.Label timeoutLabel;
        private System.Windows.Forms.GroupBox timingGroup;
        private System.Windows.Forms.Label unfilteredLabel;
        private System.Windows.Forms.Label filteredLabel;
        private System.Windows.Forms.DataGrid resultsDataGrid;
        private System.Windows.Forms.ComboBox unfilteredLinesComboBox;
        private System.Windows.Forms.ComboBox filteredLinesComboBox;
        private System.Windows.Forms.ComboBox risingEdgeComboBox;
        private System.Windows.Forms.ComboBox fallingEdgeComboBox;
        private System.Windows.Forms.NumericUpDown samplesNumeric;
        private System.Windows.Forms.Label samplesLabel;
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
            dataTable = new DataTable();

            unfilteredLinesComboBox.Items.Add("None");
            filteredLinesComboBox.Items.Add("None");
            risingEdgeComboBox.Items.Add("None");
            fallingEdgeComboBox.Items.Add("None");

            unfilteredLinesComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DILine | PhysicalChannelTypes.DIPort, PhysicalChannelAccess.External));
            filteredLinesComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DILine | PhysicalChannelTypes.DIPort, PhysicalChannelAccess.External));
            risingEdgeComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DILine | PhysicalChannelTypes.DIPort, PhysicalChannelAccess.External));
            fallingEdgeComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DILine | PhysicalChannelTypes.DIPort, PhysicalChannelAccess.External));
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
            this.timingGroup = new System.Windows.Forms.GroupBox();
            this.samplesNumeric = new System.Windows.Forms.NumericUpDown();
            this.samplesLabel = new System.Windows.Forms.Label();
            this.timeoutNumeric = new System.Windows.Forms.NumericUpDown();
            this.timeoutLabel = new System.Windows.Forms.Label();
            this.pulseWidthNumeric = new System.Windows.Forms.NumericUpDown();
            this.pulseWidthLabel = new System.Windows.Forms.Label();
            this.lineGroup = new System.Windows.Forms.GroupBox();
            this.fallingEdgeComboBox = new System.Windows.Forms.ComboBox();
            this.risingEdgeComboBox = new System.Windows.Forms.ComboBox();
            this.filteredLinesComboBox = new System.Windows.Forms.ComboBox();
            this.unfilteredLinesComboBox = new System.Windows.Forms.ComboBox();
            this.filteredLabel = new System.Windows.Forms.Label();
            this.risingEdgeLabel = new System.Windows.Forms.Label();
            this.fallingEdgeLabel = new System.Windows.Forms.Label();
            this.unfilteredLabel = new System.Windows.Forms.Label();
            this.resultsGroup = new System.Windows.Forms.GroupBox();
            this.resultsDataGrid = new System.Windows.Forms.DataGrid();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.timingGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.samplesNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeoutNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pulseWidthNumeric)).BeginInit();
            this.lineGroup.SuspendLayout();
            this.resultsGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resultsDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // timingGroup
            // 
            this.timingGroup.Controls.Add(this.samplesNumeric);
            this.timingGroup.Controls.Add(this.samplesLabel);
            this.timingGroup.Controls.Add(this.timeoutNumeric);
            this.timingGroup.Controls.Add(this.timeoutLabel);
            this.timingGroup.Controls.Add(this.pulseWidthNumeric);
            this.timingGroup.Controls.Add(this.pulseWidthLabel);
            this.timingGroup.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.timingGroup.Location = new System.Drawing.Point(8, 168);
            this.timingGroup.Name = "timingGroup";
            this.timingGroup.Size = new System.Drawing.Size(240, 120);
            this.timingGroup.TabIndex = 3;
            this.timingGroup.TabStop = false;
            this.timingGroup.Text = "Timing Parameters";
            // 
            // samplesNumeric
            // 
            this.samplesNumeric.Location = new System.Drawing.Point(112, 56);
            this.samplesNumeric.Maximum = new System.Decimal(new int[] {
                                                                           1000,
                                                                           0,
                                                                           0,
                                                                           0});
            this.samplesNumeric.Name = "samplesNumeric";
            this.samplesNumeric.TabIndex = 3;
            this.samplesNumeric.Value = new System.Decimal(new int[] {
                                                                         4,
                                                                         0,
                                                                         0,
                                                                         0});
            // 
            // samplesLabel
            // 
            this.samplesLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.samplesLabel.Location = new System.Drawing.Point(8, 56);
            this.samplesLabel.Name = "samplesLabel";
            this.samplesLabel.Size = new System.Drawing.Size(112, 23);
            this.samplesLabel.TabIndex = 2;
            this.samplesLabel.Text = "Number of Samples:";
            // 
            // timeoutNumeric
            // 
            this.timeoutNumeric.Location = new System.Drawing.Point(112, 88);
            this.timeoutNumeric.Name = "timeoutNumeric";
            this.timeoutNumeric.TabIndex = 5;
            this.timeoutNumeric.Value = new System.Decimal(new int[] {
                                                                         10,
                                                                         0,
                                                                         0,
                                                                         0});
            // 
            // timeoutLabel
            // 
            this.timeoutLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.timeoutLabel.Location = new System.Drawing.Point(8, 88);
            this.timeoutLabel.Name = "timeoutLabel";
            this.timeoutLabel.Size = new System.Drawing.Size(112, 23);
            this.timeoutLabel.TabIndex = 4;
            this.timeoutLabel.Text = "Timeout (s):";
            // 
            // pulseWidthNumeric
            // 
            this.pulseWidthNumeric.DecimalPlaces = 7;
            this.pulseWidthNumeric.Increment = new System.Decimal(new int[] {
                                                                                1,
                                                                                0,
                                                                                0,
                                                                                262144});
            this.pulseWidthNumeric.Location = new System.Drawing.Point(112, 24);
            this.pulseWidthNumeric.Name = "pulseWidthNumeric";
            this.pulseWidthNumeric.TabIndex = 1;
            this.pulseWidthNumeric.Value = new System.Decimal(new int[] {
                                                                            1,
                                                                            0,
                                                                            0,
                                                                            262144});
            // 
            // pulseWidthLabel
            // 
            this.pulseWidthLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.pulseWidthLabel.Location = new System.Drawing.Point(8, 24);
            this.pulseWidthLabel.Name = "pulseWidthLabel";
            this.pulseWidthLabel.Size = new System.Drawing.Size(112, 23);
            this.pulseWidthLabel.TabIndex = 0;
            this.pulseWidthLabel.Text = "Min. Pulse Width (s):";
            // 
            // lineGroup
            // 
            this.lineGroup.Controls.Add(this.fallingEdgeComboBox);
            this.lineGroup.Controls.Add(this.risingEdgeComboBox);
            this.lineGroup.Controls.Add(this.filteredLinesComboBox);
            this.lineGroup.Controls.Add(this.unfilteredLinesComboBox);
            this.lineGroup.Controls.Add(this.filteredLabel);
            this.lineGroup.Controls.Add(this.risingEdgeLabel);
            this.lineGroup.Controls.Add(this.fallingEdgeLabel);
            this.lineGroup.Controls.Add(this.unfilteredLabel);
            this.lineGroup.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lineGroup.Location = new System.Drawing.Point(8, 8);
            this.lineGroup.Name = "lineGroup";
            this.lineGroup.Size = new System.Drawing.Size(240, 152);
            this.lineGroup.TabIndex = 2;
            this.lineGroup.TabStop = false;
            this.lineGroup.Text = "Line Parameters";
            // 
            // fallingEdgeComboBox
            // 
            this.fallingEdgeComboBox.Location = new System.Drawing.Point(112, 120);
            this.fallingEdgeComboBox.Name = "fallingEdgeComboBox";
            this.fallingEdgeComboBox.Size = new System.Drawing.Size(121, 21);
            this.fallingEdgeComboBox.TabIndex = 7;
            this.fallingEdgeComboBox.Text = "Dev1/port0/line4:7";
            // 
            // risingEdgeComboBox
            // 
            this.risingEdgeComboBox.Location = new System.Drawing.Point(112, 88);
            this.risingEdgeComboBox.Name = "risingEdgeComboBox";
            this.risingEdgeComboBox.Size = new System.Drawing.Size(121, 21);
            this.risingEdgeComboBox.TabIndex = 5;
            this.risingEdgeComboBox.Text = "Dev1/port0/line0:4";
            // 
            // filteredLinesComboBox
            // 
            this.filteredLinesComboBox.Location = new System.Drawing.Point(112, 56);
            this.filteredLinesComboBox.Name = "filteredLinesComboBox";
            this.filteredLinesComboBox.Size = new System.Drawing.Size(121, 21);
            this.filteredLinesComboBox.TabIndex = 3;
            this.filteredLinesComboBox.Text = "Dev1/port0/line4:7";
            // 
            // unfilteredLinesComboBox
            // 
            this.unfilteredLinesComboBox.Location = new System.Drawing.Point(112, 24);
            this.unfilteredLinesComboBox.Name = "unfilteredLinesComboBox";
            this.unfilteredLinesComboBox.Size = new System.Drawing.Size(121, 21);
            this.unfilteredLinesComboBox.TabIndex = 1;
            this.unfilteredLinesComboBox.Text = "Dev1/port0/line0:3";
            // 
            // filteredLabel
            // 
            this.filteredLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.filteredLabel.Location = new System.Drawing.Point(8, 55);
            this.filteredLabel.Name = "filteredLabel";
            this.filteredLabel.Size = new System.Drawing.Size(88, 23);
            this.filteredLabel.TabIndex = 2;
            this.filteredLabel.Text = "Filtered Lines:";
            // 
            // risingEdgeLabel
            // 
            this.risingEdgeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.risingEdgeLabel.Location = new System.Drawing.Point(8, 87);
            this.risingEdgeLabel.Name = "risingEdgeLabel";
            this.risingEdgeLabel.Size = new System.Drawing.Size(112, 23);
            this.risingEdgeLabel.TabIndex = 4;
            this.risingEdgeLabel.Text = "Detect Rising Edges:";
            // 
            // fallingEdgeLabel
            // 
            this.fallingEdgeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.fallingEdgeLabel.Location = new System.Drawing.Point(8, 119);
            this.fallingEdgeLabel.Name = "fallingEdgeLabel";
            this.fallingEdgeLabel.Size = new System.Drawing.Size(112, 23);
            this.fallingEdgeLabel.TabIndex = 6;
            this.fallingEdgeLabel.Text = "Detect Falling Edges:";
            // 
            // unfilteredLabel
            // 
            this.unfilteredLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.unfilteredLabel.Location = new System.Drawing.Point(8, 23);
            this.unfilteredLabel.Name = "unfilteredLabel";
            this.unfilteredLabel.Size = new System.Drawing.Size(104, 23);
            this.unfilteredLabel.TabIndex = 0;
            this.unfilteredLabel.Text = "Unfiltered Lines:";
            // 
            // resultsGroup
            // 
            this.resultsGroup.Controls.Add(this.resultsDataGrid);
            this.resultsGroup.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.resultsGroup.Location = new System.Drawing.Point(256, 8);
            this.resultsGroup.Name = "resultsGroup";
            this.resultsGroup.Size = new System.Drawing.Size(304, 320);
            this.resultsGroup.TabIndex = 4;
            this.resultsGroup.TabStop = false;
            this.resultsGroup.Text = "Results";
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
            this.resultsDataGrid.Size = new System.Drawing.Size(293, 296);
            this.resultsDataGrid.TabIndex = 0;
            // 
            // startButton
            // 
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.startButton.Location = new System.Drawing.Point(40, 304);
            this.startButton.Name = "startButton";
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.stopButton.Location = new System.Drawing.Point(144, 304);
            this.stopButton.Name = "stopButton";
            this.stopButton.TabIndex = 1;
            this.stopButton.Text = "Stop";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(568, 334);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.resultsGroup);
            this.Controls.Add(this.lineGroup);
            this.Controls.Add(this.timingGroup);
            this.Controls.Add(this.stopButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Digital Channel Read - Change Detection and Digital Filter";
            this.timingGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.samplesNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeoutNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pulseWidthNumeric)).EndInit();
            this.lineGroup.ResumeLayout(false);
            this.resultsGroup.ResumeLayout(false);
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

                    // Create the channels
                    DIChannel unfilteredChannel;
                    DIChannel filteredChannel;

                    // Disable filtering for these lines, if any
                    if (unfilteredLinesComboBox.Text != "None")
                    {
                        unfilteredChannel = myTask.DIChannels.CreateChannel(unfilteredLinesComboBox.Text, "", ChannelLineGrouping.OneChannelForAllLines);
                        unfilteredChannel.DigitalFilterEnable = false;
                    }
                    
                    // Enable filtering for these lines, if any
                    if (filteredLinesComboBox.Text != "None")
                    {
                        filteredChannel = myTask.DIChannels.CreateChannel(filteredLinesComboBox.Text, "", ChannelLineGrouping.OneChannelForAllLines);
                        filteredChannel.DigitalFilterEnable = true;
                        filteredChannel.DigitalFilterMinimumPulseWidth = (double)pulseWidthNumeric.Value;
                    }

                    // Change "None" to "" from combo box rising/falling edges selections
                    String rising, falling;

                    if (risingEdgeComboBox.Text == "None")
                        rising = "";
                    else
                        rising = risingEdgeComboBox.Text;

                    if (fallingEdgeComboBox.Text == "None")
                        falling = "";
                    else
                        falling = fallingEdgeComboBox.Text;

                    // Configure the timing parameters for change detection
                    myTask.Timing.ConfigureChangeDetection(rising, falling, SampleQuantityMode.ContinuousSamples, (int)samplesNumeric.Value);

                    // Set timeout
                    myTask.Stream.Timeout = (int)timeoutNumeric.Value * 1000;

                    // Verify the Task
                    myTask.Control(TaskAction.Verify);

                    // Set up the data table
                    InitializeDataTable();
                    resultsDataGrid.DataSource = dataTable;   

                    // Start running the task
                    StartTask();

                    // Create the analog input sound reader
                    digitalReader = new DigitalMultiChannelReader(myTask.Stream);

                    // Use SynchronizeCallbacks to specify that the object 
                    // marshals callbacks across threads appropriately.
                    digitalReader.SynchronizeCallbacks = true;

                    // Set up our first callback
                    digitalCallback = new AsyncCallback(DigitalCallback);
                    digitalReader.BeginReadSingleSampleMultiLine(digitalCallback, myTask);
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
                    bool[,] data = digitalReader.EndReadSingleSampleMultiLine(ar);

                    // Clear data table
                    dataTable.Rows.Clear();

                    // Iterate over channels and lines
                    int i = 0;
                    for (int chan = 0; chan < myTask.DIChannels.Count; ++chan)
                    {
                        for (int line = 0; line < myTask.DIChannels[chan].NumberOfLines; ++line)
                        {
                            bool b = data[chan, line];
                            DataRow myRow = dataTable.NewRow();
                            myRow["Line"] = "Line " + i.ToString();
                            myRow["Value"] = b.ToString();
                            dataTable.Rows.Add(myRow);
                            i++;
                        }
                    }

                    // Set up a new callback
                    digitalReader.BeginReadSingleSampleMultiLine(digitalCallback, myTask);
                }
            }
            catch (DaqException exception)
            {   
                // Display Errors
                MessageBox.Show(exception.Message);
                StopTask();
            }
        }

        public void InitializeDataTable()
        {
            dataTable.Rows.Clear();
            dataTable.Columns.Clear();

            // Create new DataColumn, set DataType, ColumnName and add to DataTable.    
            DataColumn myColumn = new DataColumn();
            myColumn.DataType = System.Type.GetType("System.String");
            myColumn.ColumnName = "Line";
            dataTable.Columns.Add(myColumn);

            // Create second column.
            myColumn = new DataColumn();
            myColumn.DataType = Type.GetType("System.String");
            myColumn.ColumnName = "Value";
            dataTable.Columns.Add(myColumn);
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

            filteredLinesComboBox.Enabled = false;
            unfilteredLinesComboBox.Enabled = false;
            risingEdgeComboBox.Enabled = false;
            fallingEdgeComboBox.Enabled = false;
            pulseWidthNumeric.Enabled = false;
            samplesNumeric.Enabled = false;
            timeoutNumeric.Enabled = false;
            startButton.Enabled = false;
            stopButton.Enabled = true;

            this.Refresh();
        }

        private void StopTask()
        {
            runningTask = null;
            myTask.Dispose();

            filteredLinesComboBox.Enabled = true;
            unfilteredLinesComboBox.Enabled = true;
            risingEdgeComboBox.Enabled = true;
            fallingEdgeComboBox.Enabled = true;
            pulseWidthNumeric.Enabled = true;
            samplesNumeric.Enabled = true;
            timeoutNumeric.Enabled = true;
            startButton.Enabled = true;
            stopButton.Enabled = false;
        }
    }
}
