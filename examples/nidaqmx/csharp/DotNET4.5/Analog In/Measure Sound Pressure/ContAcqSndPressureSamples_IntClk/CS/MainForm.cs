/******************************************************************************
*
* Example program:
*   ContAcqSndPressureSamples_IntClk
*
* Category:
*   AI
*
* Description:
*   This example demonstrates how to acquire a continuous set of sound pressure
*   data using the DAQ device's internal clock.
*
* Instructions for running:
*   1.  Select the physical channel to correspond to where your signal is input
*       on the DAQ device.
*   2.  Set the maximum sound pressure in decibels.
*   3.  Select the correct input terminal configuration.
*   4.  Select the IEPE excitation source.
*   5.  Select the IEPE current, if applicable.
*   6.  Set the rate of the acquisition and number of samples.Note:  The rate
*       should be at least twice as fast as the maximum frequency component of
*       the signal being acquired.  Also, in order to avoid Error -50410 (buffer
*       overflow) it is important to make sure the rate and the number of
*       samples to read per iteration are set such that they don't fill the
*       buffer too quickly. If this error occurs try reducing the rate or
*       increasing the number of samples to read per iteration.
*   7.  Set the microphone sensitivity, in millivolts per pascal.  Note that
*       setting this value higher actually makes the results in the graph
*       smaller, and vice versa.
*
* Steps:
*   1.  Create a new analog input task.
*   2.  Create an analog input microphone channel.
*   3.  Set up the timing for the acquisition.  In this example we use the DAQ
*       device's internal clock to take a continuous number of samples.
*   4.  Create an AnalogMultiChannelReader and associate it with the task by
*       using the task's stream. Call AnalogMultiChannelReader.BeginReadWaveform
*       to install a callback and begin the asynchronous read operation.
*   5.  Inside the callback, call AnalogMultiChannelReader.EndReadWaveform to
*       retrieve the data from the read operation.  
*   6.  Call AnalogMultiChannelReader.BeginMemoryOptimizedReadWaveform
*   7.  Dispose the Task object to clean-up any resources associated with the
*       task.
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
*   Make sure your signal input terminal matches the physical channel I/O
*   control. In the default case (differential channel ai0) wire the positive
*   lead for your signal to the ACH0 pin on your DAQ device and wire the
*   negative lead for your signal to the ACH8 pin on you DAQ device.  For more
*   information on the input and output terminals for your device, open the
*   NI-DAQmx Help, and refer to the NI-DAQmx Device Terminals and Device
*   Considerations books in the table of contents.
*
* Recommended Use:
*   Create a Task object. Create the appropriate AIChannel object and configure
*   its parameters.  Configure the Timing parameters by using the Timing object.
*   Read the data by using the AnalogMultiChannelReader object. Use the
*   appropriate BeginRead method to read the data asynchronously. Use
*   Task.Dispose to stop the task and de-allocate any resources used by the
*   task.
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

namespace NationalInstruments.Examples.ContAcqSndPressureSamples_IntClk
{
    /// <summary>
    /// Summary description for MainForm.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        private NationalInstruments.DAQmx.Task myTask;
        private NationalInstruments.DAQmx.AnalogMultiChannelReader soundReader;
        private AsyncCallback soundCallback;
        private DataTable dataTable;
        private DataColumn[] dataColumn;
        private Task runningTask;

        private System.Windows.Forms.GroupBox channelParametersGroupBox;
        private System.Windows.Forms.GroupBox timingParametersGroupBox;
        private System.Windows.Forms.GroupBox microphoneParametersGroupBox;
        private System.Windows.Forms.GroupBox acquisitionResultsGroupBox;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Label physicalChannelLabel;
        private System.Windows.Forms.Label maximumPressureLabel;
        private System.Windows.Forms.ComboBox inputTerminalComboBox;
        private System.Windows.Forms.NumericUpDown timingRateNumeric;
        private System.Windows.Forms.Label timingRateLabel;
        private System.Windows.Forms.Label timingSamplesLabel;
        private System.Windows.Forms.NumericUpDown timingSamplesNumeric;
        private System.Windows.Forms.Label sensitivityLabel;
        private System.Windows.Forms.NumericUpDown sensitivityNumeric;
        private System.Windows.Forms.NumericUpDown maximumPressureNumeric;
        private System.Windows.Forms.Label excitationLabel;
        private System.Windows.Forms.ComboBox excitationComboBox;
        private System.Windows.Forms.Label currentLabel;
        private System.Windows.Forms.NumericUpDown currentNumeric;
        private System.Windows.Forms.Label inputTerminalLabel;
        private System.Windows.Forms.DataGrid resultsDataGrid;
        private System.Windows.Forms.ComboBox physicalChannelComboBox;
        private System.Windows.Forms.Label lblNote;
        
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
            physicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AI, PhysicalChannelAccess.External));
            if (physicalChannelComboBox.Items.Count > 0)
                physicalChannelComboBox.SelectedIndex = 0;
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
            this.currentNumeric = new System.Windows.Forms.NumericUpDown();
            this.inputTerminalComboBox = new System.Windows.Forms.ComboBox();
            this.physicalChannelLabel = new System.Windows.Forms.Label();
            this.inputTerminalLabel = new System.Windows.Forms.Label();
            this.excitationLabel = new System.Windows.Forms.Label();
            this.excitationComboBox = new System.Windows.Forms.ComboBox();
            this.currentLabel = new System.Windows.Forms.Label();
            this.maximumPressureNumeric = new System.Windows.Forms.NumericUpDown();
            this.maximumPressureLabel = new System.Windows.Forms.Label();
            this.timingParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.timingRateNumeric = new System.Windows.Forms.NumericUpDown();
            this.timingRateLabel = new System.Windows.Forms.Label();
            this.timingSamplesLabel = new System.Windows.Forms.Label();
            this.timingSamplesNumeric = new System.Windows.Forms.NumericUpDown();
            this.microphoneParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.sensitivityLabel = new System.Windows.Forms.Label();
            this.sensitivityNumeric = new System.Windows.Forms.NumericUpDown();
            this.acquisitionResultsGroupBox = new System.Windows.Forms.GroupBox();
            this.resultsDataGrid = new System.Windows.Forms.DataGrid();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.lblNote = new System.Windows.Forms.Label();
            this.channelParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.currentNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maximumPressureNumeric)).BeginInit();
            this.timingParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timingRateNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timingSamplesNumeric)).BeginInit();
            this.microphoneParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sensitivityNumeric)).BeginInit();
            this.acquisitionResultsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resultsDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // channelParametersGroupBox
            // 
            this.channelParametersGroupBox.Controls.Add(this.physicalChannelComboBox);
            this.channelParametersGroupBox.Controls.Add(this.currentNumeric);
            this.channelParametersGroupBox.Controls.Add(this.inputTerminalComboBox);
            this.channelParametersGroupBox.Controls.Add(this.physicalChannelLabel);
            this.channelParametersGroupBox.Controls.Add(this.inputTerminalLabel);
            this.channelParametersGroupBox.Controls.Add(this.excitationLabel);
            this.channelParametersGroupBox.Controls.Add(this.excitationComboBox);
            this.channelParametersGroupBox.Controls.Add(this.currentLabel);
            this.channelParametersGroupBox.Controls.Add(this.maximumPressureNumeric);
            this.channelParametersGroupBox.Controls.Add(this.maximumPressureLabel);
            this.channelParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.channelParametersGroupBox.Location = new System.Drawing.Point(8, 0);
            this.channelParametersGroupBox.Name = "channelParametersGroupBox";
            this.channelParametersGroupBox.Size = new System.Drawing.Size(248, 168);
            this.channelParametersGroupBox.TabIndex = 0;
            this.channelParametersGroupBox.TabStop = false;
            this.channelParametersGroupBox.Text = "Channel Parameters";
            // 
            // physicalChannelComboBox
            // 
            this.physicalChannelComboBox.Location = new System.Drawing.Point(128, 24);
            this.physicalChannelComboBox.Name = "physicalChannelComboBox";
            this.physicalChannelComboBox.Size = new System.Drawing.Size(112, 21);
            this.physicalChannelComboBox.TabIndex = 0;
            this.physicalChannelComboBox.Text = "Dev1/ai0";
            // 
            // currentNumeric
            // 
            this.currentNumeric.DecimalPlaces = 3;
            this.currentNumeric.Location = new System.Drawing.Point(128, 135);
            this.currentNumeric.Name = "currentNumeric";
            this.currentNumeric.Size = new System.Drawing.Size(112, 20);
            this.currentNumeric.TabIndex = 5;
            this.currentNumeric.Value = new System.Decimal(new int[] {
                                                                         4,
                                                                         0,
                                                                         0,
                                                                         196608});
            // 
            // inputTerminalComboBox
            // 
            this.inputTerminalComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.inputTerminalComboBox.Items.AddRange(new object[] {
                                                                       "Pseudodifferential",
                                                                       "Differential",
                                                                       "Nrse",
                                                                       "Rse"});
            this.inputTerminalComboBox.Location = new System.Drawing.Point(128, 79);
            this.inputTerminalComboBox.Name = "inputTerminalComboBox";
            this.inputTerminalComboBox.Size = new System.Drawing.Size(112, 21);
            this.inputTerminalComboBox.TabIndex = 2;
            // 
            // physicalChannelLabel
            // 
            this.physicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.physicalChannelLabel.Location = new System.Drawing.Point(8, 26);
            this.physicalChannelLabel.Name = "physicalChannelLabel";
            this.physicalChannelLabel.Size = new System.Drawing.Size(104, 16);
            this.physicalChannelLabel.TabIndex = 0;
            this.physicalChannelLabel.Text = "Physical Channel:*";
            // 
            // inputTerminalLabel
            // 
            this.inputTerminalLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.inputTerminalLabel.Location = new System.Drawing.Point(8, 81);
            this.inputTerminalLabel.Name = "inputTerminalLabel";
            this.inputTerminalLabel.Size = new System.Drawing.Size(104, 16);
            this.inputTerminalLabel.TabIndex = 0;
            this.inputTerminalLabel.Text = "Input Terminal:";
            // 
            // excitationLabel
            // 
            this.excitationLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.excitationLabel.Location = new System.Drawing.Point(8, 109);
            this.excitationLabel.Name = "excitationLabel";
            this.excitationLabel.Size = new System.Drawing.Size(104, 16);
            this.excitationLabel.TabIndex = 0;
            this.excitationLabel.Text = "IEPE Excitation:";
            // 
            // excitationComboBox
            // 
            this.excitationComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.excitationComboBox.Items.AddRange(new object[] {
                                                                    "Internal",
                                                                    "External",
                                                                    "None"});
            this.excitationComboBox.Location = new System.Drawing.Point(128, 107);
            this.excitationComboBox.Name = "excitationComboBox";
            this.excitationComboBox.Size = new System.Drawing.Size(112, 21);
            this.excitationComboBox.TabIndex = 4;
            // 
            // currentLabel
            // 
            this.currentLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.currentLabel.Location = new System.Drawing.Point(8, 137);
            this.currentLabel.Name = "currentLabel";
            this.currentLabel.Size = new System.Drawing.Size(104, 16);
            this.currentLabel.TabIndex = 0;
            this.currentLabel.Text = "IEPE Current [A]:";
            // 
            // maximumPressureNumeric
            // 
            this.maximumPressureNumeric.DecimalPlaces = 2;
            this.maximumPressureNumeric.Location = new System.Drawing.Point(128, 52);
            this.maximumPressureNumeric.Maximum = new System.Decimal(new int[] {
                                                                                   2000,
                                                                                   0,
                                                                                   0,
                                                                                   0});
            this.maximumPressureNumeric.Name = "maximumPressureNumeric";
            this.maximumPressureNumeric.Size = new System.Drawing.Size(112, 20);
            this.maximumPressureNumeric.TabIndex = 1;
            this.maximumPressureNumeric.Value = new System.Decimal(new int[] {
                                                                                 120,
                                                                                 0,
                                                                                 0,
                                                                                 0});
            // 
            // maximumPressureLabel
            // 
            this.maximumPressureLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.maximumPressureLabel.Location = new System.Drawing.Point(8, 54);
            this.maximumPressureLabel.Name = "maximumPressureLabel";
            this.maximumPressureLabel.Size = new System.Drawing.Size(128, 16);
            this.maximumPressureLabel.TabIndex = 0;
            this.maximumPressureLabel.Text = "Maximum Pressure [db]:";
            // 
            // timingParametersGroupBox
            // 
            this.timingParametersGroupBox.Controls.Add(this.timingRateNumeric);
            this.timingParametersGroupBox.Controls.Add(this.timingRateLabel);
            this.timingParametersGroupBox.Controls.Add(this.timingSamplesLabel);
            this.timingParametersGroupBox.Controls.Add(this.timingSamplesNumeric);
            this.timingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.timingParametersGroupBox.Location = new System.Drawing.Point(8, 176);
            this.timingParametersGroupBox.Name = "timingParametersGroupBox";
            this.timingParametersGroupBox.Size = new System.Drawing.Size(248, 80);
            this.timingParametersGroupBox.TabIndex = 1;
            this.timingParametersGroupBox.TabStop = false;
            this.timingParametersGroupBox.Text = "Timing Parameters";
            // 
            // timingRateNumeric
            // 
            this.timingRateNumeric.DecimalPlaces = 3;
            this.timingRateNumeric.Location = new System.Drawing.Point(128, 24);
            this.timingRateNumeric.Maximum = new System.Decimal(new int[] {
                                                                              500000,
                                                                              0,
                                                                              0,
                                                                              0});
            this.timingRateNumeric.Minimum = new System.Decimal(new int[] {
                                                                              1000,
                                                                              0,
                                                                              0,
                                                                              0});
            this.timingRateNumeric.Name = "timingRateNumeric";
            this.timingRateNumeric.Size = new System.Drawing.Size(112, 20);
            this.timingRateNumeric.TabIndex = 0;
            this.timingRateNumeric.Value = new System.Decimal(new int[] {
                                                                            25600,
                                                                            0,
                                                                            0,
                                                                            0});
            // 
            // timingRateLabel
            // 
            this.timingRateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.timingRateLabel.Location = new System.Drawing.Point(8, 24);
            this.timingRateLabel.Name = "timingRateLabel";
            this.timingRateLabel.Size = new System.Drawing.Size(104, 16);
            this.timingRateLabel.TabIndex = 0;
            this.timingRateLabel.Text = "Rate [Hz]:";
            // 
            // timingSamplesLabel
            // 
            this.timingSamplesLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.timingSamplesLabel.Location = new System.Drawing.Point(8, 48);
            this.timingSamplesLabel.Name = "timingSamplesLabel";
            this.timingSamplesLabel.Size = new System.Drawing.Size(104, 16);
            this.timingSamplesLabel.TabIndex = 0;
            this.timingSamplesLabel.Text = "Samples to Read:";
            // 
            // timingSamplesNumeric
            // 
            this.timingSamplesNumeric.Location = new System.Drawing.Point(128, 48);
            this.timingSamplesNumeric.Maximum = new System.Decimal(new int[] {
                                                                                 500000,
                                                                                 0,
                                                                                 0,
                                                                                 0});
            this.timingSamplesNumeric.Minimum = new System.Decimal(new int[] {
                                                                                 2,
                                                                                 0,
                                                                                 0,
                                                                                 0});
            this.timingSamplesNumeric.Name = "timingSamplesNumeric";
            this.timingSamplesNumeric.Size = new System.Drawing.Size(112, 20);
            this.timingSamplesNumeric.TabIndex = 1;
            this.timingSamplesNumeric.Value = new System.Decimal(new int[] {
                                                                               1024,
                                                                               0,
                                                                               0,
                                                                               0});
            // 
            // microphoneParametersGroupBox
            // 
            this.microphoneParametersGroupBox.Controls.Add(this.sensitivityLabel);
            this.microphoneParametersGroupBox.Controls.Add(this.sensitivityNumeric);
            this.microphoneParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.microphoneParametersGroupBox.Location = new System.Drawing.Point(8, 264);
            this.microphoneParametersGroupBox.Name = "microphoneParametersGroupBox";
            this.microphoneParametersGroupBox.Size = new System.Drawing.Size(248, 56);
            this.microphoneParametersGroupBox.TabIndex = 2;
            this.microphoneParametersGroupBox.TabStop = false;
            this.microphoneParametersGroupBox.Text = "Microphone Parameters";
            // 
            // sensitivityLabel
            // 
            this.sensitivityLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.sensitivityLabel.Location = new System.Drawing.Point(8, 24);
            this.sensitivityLabel.Name = "sensitivityLabel";
            this.sensitivityLabel.Size = new System.Drawing.Size(104, 16);
            this.sensitivityLabel.TabIndex = 0;
            this.sensitivityLabel.Text = "Sensitivity [mV/Pa]:";
            // 
            // sensitivityNumeric
            // 
            this.sensitivityNumeric.DecimalPlaces = 3;
            this.sensitivityNumeric.Location = new System.Drawing.Point(128, 24);
            this.sensitivityNumeric.Maximum = new System.Decimal(new int[] {
                                                                               50000,
                                                                               0,
                                                                               0,
                                                                               131072});
            this.sensitivityNumeric.Minimum = new System.Decimal(new int[] {
                                                                               1,
                                                                               0,
                                                                               0,
                                                                               262144});
            this.sensitivityNumeric.Name = "sensitivityNumeric";
            this.sensitivityNumeric.Size = new System.Drawing.Size(112, 20);
            this.sensitivityNumeric.TabIndex = 0;
            this.sensitivityNumeric.Value = new System.Decimal(new int[] {
                                                                             50,
                                                                             0,
                                                                             0,
                                                                             0});
            // 
            // acquisitionResultsGroupBox
            // 
            this.acquisitionResultsGroupBox.Controls.Add(this.resultsDataGrid);
            this.acquisitionResultsGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.acquisitionResultsGroupBox.Location = new System.Drawing.Point(264, 0);
            this.acquisitionResultsGroupBox.Name = "acquisitionResultsGroupBox";
            this.acquisitionResultsGroupBox.Size = new System.Drawing.Size(376, 448);
            this.acquisitionResultsGroupBox.TabIndex = 5;
            this.acquisitionResultsGroupBox.TabStop = false;
            this.acquisitionResultsGroupBox.Text = "Acquisition Results";
            // 
            // resultsDataGrid
            // 
            this.resultsDataGrid.AllowSorting = false;
            this.resultsDataGrid.DataMember = "";
            this.resultsDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.resultsDataGrid.Location = new System.Drawing.Point(3, 16);
            this.resultsDataGrid.Name = "resultsDataGrid";
            this.resultsDataGrid.PreferredRowHeight = 30;
            this.resultsDataGrid.ReadOnly = true;
            this.resultsDataGrid.Size = new System.Drawing.Size(365, 424);
            this.resultsDataGrid.TabIndex = 0;
            // 
            // startButton
            // 
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.startButton.Location = new System.Drawing.Point(48, 328);
            this.startButton.Name = "startButton";
            this.startButton.TabIndex = 3;
            this.startButton.Text = "Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.stopButton.Location = new System.Drawing.Point(160, 328);
            this.stopButton.Name = "stopButton";
            this.stopButton.TabIndex = 4;
            this.stopButton.Text = "Stop";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // lblNote
            // 
            this.lblNote.Location = new System.Drawing.Point(16, 368);
            this.lblNote.Name = "lblNote";
            this.lblNote.Size = new System.Drawing.Size(240, 80);
            this.lblNote.TabIndex = 6;
            this.lblNote.Text = "* Note: DSA devices now support including channels from multiple devices in a sin" +
                "gle task.  DAQmx automatically synchronizes the devices in such a task.  See the" +
                " DAQmx Help >> Device Considerations >> Multi Device Tasks section for further d" +
                "etails.";
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(650, 456);
            this.Controls.Add(this.lblNote);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.acquisitionResultsGroupBox);
            this.Controls.Add(this.microphoneParametersGroupBox);
            this.Controls.Add(this.timingParametersGroupBox);
            this.Controls.Add(this.channelParametersGroupBox);
            this.Controls.Add(this.stopButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Continuous Acquisition of Sound Pressure Samples - Internal Clock";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.channelParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.currentNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maximumPressureNumeric)).EndInit();
            this.timingParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.timingRateNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timingSamplesNumeric)).EndInit();
            this.microphoneParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sensitivityNumeric)).EndInit();
            this.acquisitionResultsGroupBox.ResumeLayout(false);
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

        private void MainForm_Load(object sender, System.EventArgs e)
        {
            inputTerminalComboBox.SelectedIndex = 0;
            excitationComboBox.SelectedIndex = 0;

            dataTable = new DataTable();
        }

        private void startButton_Click(object sender, System.EventArgs e)
        {
            if (runningTask == null)
            {
                try
                {
                    // Create a new task
                    myTask = new Task();

                    // Configure the Terminal Configuration and Excitation Source with enums
                    AITerminalConfiguration terminal = (AITerminalConfiguration)Enum.Parse(typeof(AITerminalConfiguration), inputTerminalComboBox.Text);
                    AIExcitationSource source = (AIExcitationSource)Enum.Parse(typeof(AIExcitationSource), excitationComboBox.Text);

                    // Create the channel
                    myTask.AIChannels.CreateMicrophoneChannel(physicalChannelComboBox.Text, "soundChannel",
                        Convert.ToDouble(sensitivityNumeric.Value), Convert.ToDouble(maximumPressureNumeric.Value), 
                        terminal, source, Convert.ToDouble(currentNumeric.Value), AISoundPressureUnits.Pascals);
            
                    // Configure the timing parameters
                    myTask.Timing.ConfigureSampleClock("", Convert.ToDouble(timingRateNumeric.Value),
                        SampleClockActiveEdge.Rising, SampleQuantityMode.ContinuousSamples, 1000);

                    // Verify the Task
                    myTask.Control(TaskAction.Verify);

                    // Initialize the data table
                    InitializeDataTable(myTask.AIChannels, ref dataTable); 
                    resultsDataGrid.DataSource = dataTable;   

                    // Start running the task
                    StartTask();

                    // Create the analog input sound reader
                    soundReader = new AnalogMultiChannelReader(myTask.Stream);
                    soundCallback = new AsyncCallback(SoundCallback);

                    // Use SynchronizeCallbacks to specify that the object 
                    // marshals callbacks across threads appropriately.
                    soundReader.SynchronizeCallbacks = true;
                    soundReader.BeginReadWaveform(Convert.ToInt32(timingSamplesNumeric.Value),
                        soundCallback, myTask);
                }
                catch (DaqException exception)
                {
                    // Display Errors
                    MessageBox.Show(exception.Message);
                    StopTask();
                }
            }
        }

        private void SoundCallback(IAsyncResult ar)
        {
            try
            {
                if (runningTask != null && runningTask == ar.AsyncState)
                {
                    // Read the available data from the channels
                    AnalogWaveform<double>[] data = soundReader.EndReadWaveform(ar);

                    // Plot your data here
                    dataToDataTable(data, ref dataTable);

                    // Set up a new callback
                    soundReader.BeginMemoryOptimizedReadWaveform(Convert.ToInt32(timingSamplesNumeric.Value),
                        soundCallback, myTask, data);
                }
            }
            catch (DaqException exception)
            {   
                // Display Errors
                MessageBox.Show(exception.Message);
                StopTask();
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

            for(int currentChannelIndex=0; currentChannelIndex < numOfChannels; currentChannelIndex++)
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

        private void stopButton_Click(object sender, System.EventArgs e)
        {
            if (runningTask != null)
            {
                // Dispose of the task
                runningTask = null;
                StopTask();
            }
        }

        private void StartTask()
        {
            runningTask = myTask; 
            stopButton.Enabled = true;
            startButton.Enabled = false;

            physicalChannelComboBox.Enabled = false;
            maximumPressureNumeric.Enabled = false;
            inputTerminalComboBox.Enabled = false;
            excitationComboBox.Enabled = false;
            currentNumeric.Enabled = false;
            timingRateNumeric.Enabled = false;
            timingSamplesNumeric.Enabled = false;
            sensitivityNumeric.Enabled = false;
        }

        private void StopTask()
        {
            runningTask = null;
            myTask.Dispose();
            stopButton.Enabled = false;
            startButton.Enabled = true;

            physicalChannelComboBox.Enabled = true;
            maximumPressureNumeric.Enabled = true;
            inputTerminalComboBox.Enabled = true;
            excitationComboBox.Enabled = true;
            currentNumeric.Enabled = true;
            timingRateNumeric.Enabled = true;
            timingSamplesNumeric.Enabled = true;
            sensitivityNumeric.Enabled = true;
        }
    }
}
