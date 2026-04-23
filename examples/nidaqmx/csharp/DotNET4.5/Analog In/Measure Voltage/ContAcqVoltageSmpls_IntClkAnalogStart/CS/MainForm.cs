/******************************************************************************
*
* Example program:
*   ContAcqVoltageSmpls_IntClkAnalogStart
*
* Category:
*   AI
*
* Description:
*   This example demonstrates how to continuously acquire data using the DAQ
*   device's internal clock and an analog slope start trigger.
*
* Instructions for running:
*   1.  Select the physical channel corresponding to where your signal is input
*       on the DAQ device.
*   2.  Enter the minimum and maximum voltage values.Note: For better accuracy,
*       try to match the input range to the expected voltage level of the
*       measured signal.
*   3.  Set the rate of the acquisition, and the number of samples to read.Note:
*       The rate should be at least twice as fast as the maximum frequency
*       component of the signal being acquired.
*   4.  Set the source of the analog slope start trigger. The default analog
*       trigger pin for M Series devices is APFI0. Please refer to your device
*       documentation for information regarding valid analog triggers for your
*       device.
*   5.  Set the slope and level for the desired analog edge condition.
*
* Steps:
*   1.  Create a new task and an analog input voltage channel using the
*       CreateVoltageChannel method.
*   2.  Configure timing specifications for the task using
*       Timing.ConfigureSampleClock method.
*   3.  Configure the analog trigger specifications using the
*       Triggers.StartTrigger.ConfigureAnalogEdgeTrigger method.
*   4.  Call AnalogMultiChannelReader.BeginReadWaveform to install a callback
*       and begin the asynchronous read operation.
*   5.  Inside the callback, call AnalogMultiChannelReader.EndReadWaveform to
*       retrieve the data from the read operation.  
*   6.  Call AnalogMultiChannelReader.BeginMemoryOptimizedReadWaveform
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
*   Make sure your signal input terminal matches the physical channel I/O
*   control. In the default case (differential channel ai0) wire the positive
*   lead for your signal to the ACH0 pin on your DAQ device and wire the
*   negative lead for your signal to the ACH8 pin on you DAQ device. Also, make
*   sure your analog trigger terminal matches the trigger source control. For
*   more information on the input and output terminals for your device, open the
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

namespace NationalInstruments.Examples.ContAcqVoltageSmpls_IntClkAnalogStart
{
    /// <summary>
    /// Summary description for MainForm.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {        
        private Task myTask;            // A new Task is created when the start button is pressed
        private Task runningTask;       // Flags whether or not an acquisition is in progress
        private int numberOfSamples = 0;
        private AnalogEdgeStartTriggerSlope analogSlope = AnalogEdgeStartTriggerSlope.Rising; //The start trigger value changes when the Radio button is selected
        private AnalogWaveform<double>[] data;

        private DataColumn[] dataColumn = null;
        private DataTable dataTable = null;

        private System.Windows.Forms.Label rateLabel;
        private System.Windows.Forms.Label samplesLabel;
        private System.Windows.Forms.NumericUpDown samplesPerChannelNumeric;
        private System.Windows.Forms.GroupBox channelParametersGroupBox;
        private System.Windows.Forms.Label maximumLabel;
        private System.Windows.Forms.Label minimumLabel;
        private System.Windows.Forms.Label physicalChannelLabel;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.GroupBox triggerParametersGroupBox;
        private System.Windows.Forms.GroupBox slopeGroupBox;
        private System.Windows.Forms.TextBox triggerSourceTextBox;
        private System.Windows.Forms.Label triggerSourceLabel;
        private System.Windows.Forms.GroupBox timingParametersGroupBox;
        private AnalogMultiChannelReader analogInReader;
        private AsyncCallback myAsyncCallback;
        private System.Windows.Forms.GroupBox acquisitionResultGroupBox;
        private System.Windows.Forms.Label resultLabel;
        private System.Windows.Forms.DataGrid acquisitionDataGrid;
        private System.Windows.Forms.NumericUpDown rateNumeric;
        private System.Windows.Forms.NumericUpDown triggerLevelNumeric;
        private System.Windows.Forms.RadioButton slopeRisingButton;
        private System.Windows.Forms.RadioButton slopeFallingButton;
        private System.Windows.Forms.Label triggerLevelLabel;
        internal System.Windows.Forms.NumericUpDown minimumValueNumeric;
        internal System.Windows.Forms.NumericUpDown maximumValueNumeric;
        private System.Windows.Forms.Label triggerSourceInfoAsterisk;
        private System.Windows.Forms.Label triggerSourceInfo;
        private System.Windows.Forms.ComboBox physicalChannelComboBox;
        private System.Windows.Forms.NumericUpDown hysteresisNumeric;
        private System.Windows.Forms.Label hysteresisLabel;
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
            
            dataTable= new DataTable();
            stopButton.Enabled = false;
            myAsyncCallback = new AsyncCallback(AnalogInCallback);

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.timingParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.rateLabel = new System.Windows.Forms.Label();
            this.samplesLabel = new System.Windows.Forms.Label();
            this.samplesPerChannelNumeric = new System.Windows.Forms.NumericUpDown();
            this.rateNumeric = new System.Windows.Forms.NumericUpDown();
            this.channelParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.physicalChannelComboBox = new System.Windows.Forms.ComboBox();
            this.minimumValueNumeric = new System.Windows.Forms.NumericUpDown();
            this.maximumValueNumeric = new System.Windows.Forms.NumericUpDown();
            this.maximumLabel = new System.Windows.Forms.Label();
            this.minimumLabel = new System.Windows.Forms.Label();
            this.physicalChannelLabel = new System.Windows.Forms.Label();
            this.stopButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.triggerParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.triggerSourceInfo = new System.Windows.Forms.Label();
            this.triggerSourceInfoAsterisk = new System.Windows.Forms.Label();
            this.triggerLevelNumeric = new System.Windows.Forms.NumericUpDown();
            this.triggerSourceTextBox = new System.Windows.Forms.TextBox();
            this.slopeGroupBox = new System.Windows.Forms.GroupBox();
            this.slopeRisingButton = new System.Windows.Forms.RadioButton();
            this.slopeFallingButton = new System.Windows.Forms.RadioButton();
            this.triggerSourceLabel = new System.Windows.Forms.Label();
            this.triggerLevelLabel = new System.Windows.Forms.Label();
            this.hysteresisNumeric = new System.Windows.Forms.NumericUpDown();
            this.hysteresisLabel = new System.Windows.Forms.Label();
            this.acquisitionResultGroupBox = new System.Windows.Forms.GroupBox();
            this.resultLabel = new System.Windows.Forms.Label();
            this.acquisitionDataGrid = new System.Windows.Forms.DataGrid();
            this.timingParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerChannelNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rateNumeric)).BeginInit();
            this.channelParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.minimumValueNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maximumValueNumeric)).BeginInit();
            this.triggerParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.triggerLevelNumeric)).BeginInit();
            this.slopeGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.hysteresisNumeric)).BeginInit();
            this.acquisitionResultGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.acquisitionDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // timingParametersGroupBox
            // 
            this.timingParametersGroupBox.Controls.Add(this.rateLabel);
            this.timingParametersGroupBox.Controls.Add(this.samplesLabel);
            this.timingParametersGroupBox.Controls.Add(this.samplesPerChannelNumeric);
            this.timingParametersGroupBox.Controls.Add(this.rateNumeric);
            this.timingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.timingParametersGroupBox.Location = new System.Drawing.Point(8, 136);
            this.timingParametersGroupBox.Name = "timingParametersGroupBox";
            this.timingParametersGroupBox.Size = new System.Drawing.Size(232, 88);
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
            32768,
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
            this.rateNumeric.Location = new System.Drawing.Point(120, 56);
            this.rateNumeric.Maximum = new decimal(new int[] {
            1000000,
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
            this.channelParametersGroupBox.Size = new System.Drawing.Size(232, 120);
            this.channelParametersGroupBox.TabIndex = 0;
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
            this.minimumValueNumeric.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.minimumValueNumeric.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.minimumValueNumeric.Name = "minimumValueNumeric";
            this.minimumValueNumeric.Size = new System.Drawing.Size(96, 20);
            this.minimumValueNumeric.TabIndex = 3;
            this.minimumValueNumeric.Value = new decimal(new int[] {
            100,
            0,
            0,
            -2147418112});
            // 
            // maximumValueNumeric
            // 
            this.maximumValueNumeric.DecimalPlaces = 2;
            this.maximumValueNumeric.Location = new System.Drawing.Point(120, 88);
            this.maximumValueNumeric.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.maximumValueNumeric.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.maximumValueNumeric.Name = "maximumValueNumeric";
            this.maximumValueNumeric.Size = new System.Drawing.Size(96, 20);
            this.maximumValueNumeric.TabIndex = 5;
            this.maximumValueNumeric.Value = new decimal(new int[] {
            100,
            0,
            0,
            65536});
            // 
            // maximumLabel
            // 
            this.maximumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.maximumLabel.Location = new System.Drawing.Point(16, 90);
            this.maximumLabel.Name = "maximumLabel";
            this.maximumLabel.Size = new System.Drawing.Size(96, 16);
            this.maximumLabel.TabIndex = 4;
            this.maximumLabel.Text = "Maximum (Volts):";
            // 
            // minimumLabel
            // 
            this.minimumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.minimumLabel.Location = new System.Drawing.Point(16, 58);
            this.minimumLabel.Name = "minimumLabel";
            this.minimumLabel.Size = new System.Drawing.Size(96, 16);
            this.minimumLabel.TabIndex = 2;
            this.minimumLabel.Text = "Minimum (Volts):";
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
            // stopButton
            // 
            this.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.stopButton.Location = new System.Drawing.Point(312, 392);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(112, 24);
            this.stopButton.TabIndex = 5;
            this.stopButton.Text = "Stop";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // startButton
            // 
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.startButton.Location = new System.Drawing.Point(312, 368);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(112, 24);
            this.startButton.TabIndex = 4;
            this.startButton.Text = "Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // triggerParametersGroupBox
            // 
            this.triggerParametersGroupBox.Controls.Add(this.triggerSourceInfo);
            this.triggerParametersGroupBox.Controls.Add(this.triggerSourceInfoAsterisk);
            this.triggerParametersGroupBox.Controls.Add(this.triggerLevelNumeric);
            this.triggerParametersGroupBox.Controls.Add(this.triggerSourceTextBox);
            this.triggerParametersGroupBox.Controls.Add(this.slopeGroupBox);
            this.triggerParametersGroupBox.Controls.Add(this.triggerSourceLabel);
            this.triggerParametersGroupBox.Controls.Add(this.triggerLevelLabel);
            this.triggerParametersGroupBox.Controls.Add(this.hysteresisNumeric);
            this.triggerParametersGroupBox.Controls.Add(this.hysteresisLabel);
            this.triggerParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.triggerParametersGroupBox.Location = new System.Drawing.Point(8, 232);
            this.triggerParametersGroupBox.Name = "triggerParametersGroupBox";
            this.triggerParametersGroupBox.Size = new System.Drawing.Size(232, 248);
            this.triggerParametersGroupBox.TabIndex = 2;
            this.triggerParametersGroupBox.TabStop = false;
            this.triggerParametersGroupBox.Text = "Trigger Parameters";
            // 
            // triggerSourceInfo
            // 
            this.triggerSourceInfo.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.triggerSourceInfo.Location = new System.Drawing.Point(24, 168);
            this.triggerSourceInfo.Name = "triggerSourceInfo";
            this.triggerSourceInfo.Size = new System.Drawing.Size(192, 64);
            this.triggerSourceInfo.TabIndex = 8;
            this.triggerSourceInfo.Text = "APFI0 is the default Analog Trigger pin for M Series devices.  Please refer to yo" +
                "ur device documentation for information regarding valid Analog Triggers for your" +
                " device.";
            // 
            // triggerSourceInfoAsterisk
            // 
            this.triggerSourceInfoAsterisk.Location = new System.Drawing.Point(16, 168);
            this.triggerSourceInfoAsterisk.Name = "triggerSourceInfoAsterisk";
            this.triggerSourceInfoAsterisk.Size = new System.Drawing.Size(16, 23);
            this.triggerSourceInfoAsterisk.TabIndex = 7;
            this.triggerSourceInfoAsterisk.Text = "*";
            // 
            // triggerLevelNumeric
            // 
            this.triggerLevelNumeric.DecimalPlaces = 2;
            this.triggerLevelNumeric.Location = new System.Drawing.Point(120, 56);
            this.triggerLevelNumeric.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.triggerLevelNumeric.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.triggerLevelNumeric.Name = "triggerLevelNumeric";
            this.triggerLevelNumeric.Size = new System.Drawing.Size(96, 20);
            this.triggerLevelNumeric.TabIndex = 3;
            // 
            // triggerSourceTextBox
            // 
            this.triggerSourceTextBox.Location = new System.Drawing.Point(120, 24);
            this.triggerSourceTextBox.Name = "triggerSourceTextBox";
            this.triggerSourceTextBox.Size = new System.Drawing.Size(96, 20);
            this.triggerSourceTextBox.TabIndex = 1;
            this.triggerSourceTextBox.Text = "APFI0";
            // 
            // slopeGroupBox
            // 
            this.slopeGroupBox.Controls.Add(this.slopeRisingButton);
            this.slopeGroupBox.Controls.Add(this.slopeFallingButton);
            this.slopeGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.slopeGroupBox.Location = new System.Drawing.Point(16, 112);
            this.slopeGroupBox.Name = "slopeGroupBox";
            this.slopeGroupBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.slopeGroupBox.Size = new System.Drawing.Size(200, 48);
            this.slopeGroupBox.TabIndex = 6;
            this.slopeGroupBox.TabStop = false;
            this.slopeGroupBox.Text = "Slope";
            // 
            // slopeRisingButton
            // 
            this.slopeRisingButton.Checked = true;
            this.slopeRisingButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.slopeRisingButton.Location = new System.Drawing.Point(8, 16);
            this.slopeRisingButton.Name = "slopeRisingButton";
            this.slopeRisingButton.Size = new System.Drawing.Size(56, 24);
            this.slopeRisingButton.TabIndex = 0;
            this.slopeRisingButton.TabStop = true;
            this.slopeRisingButton.Text = "Rising";
            this.slopeRisingButton.CheckedChanged += new System.EventHandler(this.slopeRisingButton_CheckedChanged);
            // 
            // slopeFallingButton
            // 
            this.slopeFallingButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.slopeFallingButton.Location = new System.Drawing.Point(72, 16);
            this.slopeFallingButton.Name = "slopeFallingButton";
            this.slopeFallingButton.Size = new System.Drawing.Size(56, 24);
            this.slopeFallingButton.TabIndex = 1;
            this.slopeFallingButton.Text = "Falling";
            this.slopeFallingButton.CheckedChanged += new System.EventHandler(this.slopeFallingButton_CheckedChanged);
            // 
            // triggerSourceLabel
            // 
            this.triggerSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.triggerSourceLabel.Location = new System.Drawing.Point(16, 24);
            this.triggerSourceLabel.Name = "triggerSourceLabel";
            this.triggerSourceLabel.Size = new System.Drawing.Size(88, 16);
            this.triggerSourceLabel.TabIndex = 0;
            this.triggerSourceLabel.Text = "Trigger Source:*";
            // 
            // triggerLevelLabel
            // 
            this.triggerLevelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.triggerLevelLabel.Location = new System.Drawing.Point(16, 56);
            this.triggerLevelLabel.Name = "triggerLevelLabel";
            this.triggerLevelLabel.Size = new System.Drawing.Size(112, 16);
            this.triggerLevelLabel.TabIndex = 2;
            this.triggerLevelLabel.Text = "Trigger Level (V):";
            // 
            // hysteresisNumeric
            // 
            this.hysteresisNumeric.DecimalPlaces = 2;
            this.hysteresisNumeric.Location = new System.Drawing.Point(120, 88);
            this.hysteresisNumeric.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.hysteresisNumeric.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.hysteresisNumeric.Name = "hysteresisNumeric";
            this.hysteresisNumeric.Size = new System.Drawing.Size(96, 20);
            this.hysteresisNumeric.TabIndex = 5;
            // 
            // hysteresisLabel
            // 
            this.hysteresisLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.hysteresisLabel.Location = new System.Drawing.Point(16, 88);
            this.hysteresisLabel.Name = "hysteresisLabel";
            this.hysteresisLabel.Size = new System.Drawing.Size(112, 16);
            this.hysteresisLabel.TabIndex = 4;
            this.hysteresisLabel.Text = "Hysteresis (V):";
            // 
            // acquisitionResultGroupBox
            // 
            this.acquisitionResultGroupBox.Controls.Add(this.resultLabel);
            this.acquisitionResultGroupBox.Controls.Add(this.acquisitionDataGrid);
            this.acquisitionResultGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.acquisitionResultGroupBox.Location = new System.Drawing.Point(248, 8);
            this.acquisitionResultGroupBox.Name = "acquisitionResultGroupBox";
            this.acquisitionResultGroupBox.Size = new System.Drawing.Size(224, 320);
            this.acquisitionResultGroupBox.TabIndex = 3;
            this.acquisitionResultGroupBox.TabStop = false;
            this.acquisitionResultGroupBox.Text = "Acquisition Results";
            // 
            // resultLabel
            // 
            this.resultLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.resultLabel.Location = new System.Drawing.Point(8, 16);
            this.resultLabel.Name = "resultLabel";
            this.resultLabel.Size = new System.Drawing.Size(128, 16);
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
            this.acquisitionDataGrid.Size = new System.Drawing.Size(208, 280);
            this.acquisitionDataGrid.TabIndex = 1;
            this.acquisitionDataGrid.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(482, 488);
            this.Controls.Add(this.triggerParametersGroupBox);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.timingParametersGroupBox);
            this.Controls.Add(this.channelParametersGroupBox);
            this.Controls.Add(this.acquisitionResultGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Continuous Voltage Acquisition - Internal Clock & Analog Start";
            this.timingParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerChannelNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rateNumeric)).EndInit();
            this.channelParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.minimumValueNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maximumValueNumeric)).EndInit();
            this.triggerParametersGroupBox.ResumeLayout(false);
            this.triggerParametersGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.triggerLevelNumeric)).EndInit();
            this.slopeGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.hysteresisNumeric)).EndInit();
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
            numberOfSamples = Convert.ToInt32(samplesPerChannelNumeric.Value);           

            if(runningTask == null)
            {           
                try
                {
                    // Create new Task
                    myTask = new Task();
            
                    // Create virtual channel
                    myTask.AIChannels.CreateVoltageChannel(physicalChannelComboBox.Text, "",
                        (AITerminalConfiguration)(-1), Convert.ToDouble(minimumValueNumeric.Value), 
                        Convert.ToDouble(maximumValueNumeric.Value), AIVoltageUnits.Volts);
            
                    // Configure Timing Specs    
                    myTask.Timing.ConfigureSampleClock("", Convert.ToDouble(rateNumeric.Value), 
                        SampleClockActiveEdge.Rising, SampleQuantityMode.ContinuousSamples, 1000);

                    // Configure the Analog Trigger
                    myTask.Triggers.StartTrigger.ConfigureAnalogEdgeTrigger(triggerSourceTextBox.Text,
                        analogSlope, Convert.ToDouble(triggerLevelNumeric.Value));
                    
                    myTask.Triggers.StartTrigger.AnalogEdge.Hysteresis = Convert.ToDouble(hysteresisNumeric.Value);

                    // Verify the Task
                    myTask.Control(TaskAction.Verify);

                    // Prepare the table for Data
                    InitializeDataTable(myTask.AIChannels,ref dataTable); 
                    acquisitionDataGrid.DataSource=dataTable;   
                   
                    runningTask = myTask;
                    analogInReader = new AnalogMultiChannelReader(myTask.Stream);

                    // Use SynchronizeCallbacks to specify that the object 
                    // marshals callbacks across threads appropriately.
                    analogInReader.SynchronizeCallbacks = true;
                    analogInReader.BeginReadWaveform(numberOfSamples, myAsyncCallback, myTask); 
                    
                   
                    startButton.Enabled = false;
                    stopButton.Enabled = true;          
                }
                catch(DaqException exception)
                {                    
                    MessageBox.Show(exception.Message);              
                    myTask.Dispose();
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

                    analogInReader.BeginMemoryOptimizedReadWaveform(numberOfSamples, myAsyncCallback, myTask, data); 

                }
            }
            catch(DaqException exception)
            {
                MessageBox.Show(exception.Message);
                stopButton_Click(null, null);
            }
        }
        
        private void slopeRisingButton_CheckedChanged(object sender, System.EventArgs e)
        {
            // Configure the Analog Start Trigger Edge as Rising
            if (slopeRisingButton.Checked)
            {
                analogSlope = AnalogEdgeStartTriggerSlope.Rising;
            }
        }

        private void slopeFallingButton_CheckedChanged(object sender, System.EventArgs e)
        {
            // Configure the Analog Start Trigger Edge as Falling
            if (slopeFallingButton.Checked)
            {
                analogSlope = AnalogEdgeStartTriggerSlope.Falling;
            }
        }

        private void stopButton_Click(object sender, System.EventArgs e)
        {
            stopButton.Enabled = false;
            startButton.Enabled = true;
            runningTask = null;
            myTask.Dispose();
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
