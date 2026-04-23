/******************************************************************************
*
* Example program:
*   AcqVoltageSamples_IntClkDigRef
*
* Category:
*   AI
*
* Description:
*   This example demonstrates how to acquire a finite amount of data using an
*   internal clock and a digital reference trigger.
*
* Instructions for running:
*   1.  Select the physical channel corresponding to where your signal is input
*       on the DAQ device.
*   2.  Enter the minimum and maximum voltage values.Note: For better accuracy,
*       try to match the input range to the expected voltage level of the
*       measured signal.
*   3.  Select the number of samples per channel to acquire.
*   4.  Set the rate in Hz for the internal clock.Note: The rate should be at
*       least twice as fast as the maximum frequency component of the signal
*       being acquired.
*   5.  Set the source of the reference trigger as well as the polarity of the
*       reference edges. Additionally, set the number of pre-trigger samples to
*       be acquired.
*
* Steps:
*   1.  Create a new task and an analog input voltage channel.
*   2.  Configure timing specifications.
*   3.  Configure the reference trigger.
*   4.  Call AnalogMultiChannelReader.BeginReadWaveform to install a callback
*       and begin the asynchronous read operation.
*   5.  Inside the callback, call AnalogMultiChannelReader.EndReadWaveform to
*       retrieve the data from the read operation.  
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
*   Make sure your signal input terminal matches the physical channel I/O
*   control. In the default case (differential channel ai0) wire the positive
*   lead for your signal to the ACH0 pin on your DAQ device and wire the
*   negative lead for your signal to the ACH8 pin on you DAQ device. Also, make
*   sure your digital trigger terminal matches the trigger source control.  For
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

namespace NationalInstruments.Examples.AcqVoltageSamples_IntClkDigRef
{
    /// <summary>
    /// Summary description for MainForm.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {        
        private Task myTask; 
        private AnalogMultiChannelReader reader; 
        private DigitalEdgeReferenceTriggerEdge referenceEdge = DigitalEdgeReferenceTriggerEdge.Rising;     
        private AnalogWaveform<double>[] data;

        private DataColumn[] dataColumn = null;
        private DataTable dataTable = null;

        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.GroupBox channelParametersGroupBox;
        private System.Windows.Forms.Label maximumLabel;
        private System.Windows.Forms.Label minimumLabel;
        private System.Windows.Forms.Label physicalChannelLabel;
        private System.Windows.Forms.Label rateLabel;
        private System.Windows.Forms.Label samplesLabel;
        private System.Windows.Forms.Label resultLabel;
        private System.Windows.Forms.GroupBox timingParametersGroupBox;
        private System.Windows.Forms.NumericUpDown preTriggerNumeric;
        private System.Windows.Forms.Label preTriggerLabel;
        private System.Windows.Forms.GroupBox acquisitionResultGroupBox;
        private System.Windows.Forms.NumericUpDown samplesPerChannelNumeric;
        private System.Windows.Forms.TextBox referenceTriggerSourceTextBox;
        private System.Windows.Forms.Label referenceTriggerSourceLabel;
        private System.Windows.Forms.GroupBox referenceEdgeGroupBox;
        private System.Windows.Forms.RadioButton referenceEdgeRisingButton;
        private System.Windows.Forms.RadioButton referenceEdgeFallingButton;
        private System.Windows.Forms.GroupBox triggerParametersGroupBox;
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

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            dataTable = new DataTable();

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
            this.minimumValueNumeric = new System.Windows.Forms.NumericUpDown();
            this.maximumValueNumeric = new System.Windows.Forms.NumericUpDown();
            this.maximumLabel = new System.Windows.Forms.Label();
            this.minimumLabel = new System.Windows.Forms.Label();
            this.physicalChannelLabel = new System.Windows.Forms.Label();
            this.timingParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.rateLabel = new System.Windows.Forms.Label();
            this.samplesLabel = new System.Windows.Forms.Label();
            this.samplesPerChannelNumeric = new System.Windows.Forms.NumericUpDown();
            this.rateNumeric = new System.Windows.Forms.NumericUpDown();
            this.startButton = new System.Windows.Forms.Button();
            this.triggerParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.preTriggerNumeric = new System.Windows.Forms.NumericUpDown();
            this.referenceTriggerSourceTextBox = new System.Windows.Forms.TextBox();
            this.referenceTriggerSourceLabel = new System.Windows.Forms.Label();
            this.preTriggerLabel = new System.Windows.Forms.Label();
            this.referenceEdgeGroupBox = new System.Windows.Forms.GroupBox();
            this.referenceEdgeRisingButton = new System.Windows.Forms.RadioButton();
            this.referenceEdgeFallingButton = new System.Windows.Forms.RadioButton();
            this.acquisitionResultGroupBox = new System.Windows.Forms.GroupBox();
            this.resultLabel = new System.Windows.Forms.Label();
            this.acquisitionDataGrid = new System.Windows.Forms.DataGrid();
            this.channelParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.minimumValueNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maximumValueNumeric)).BeginInit();
            this.timingParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerChannelNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rateNumeric)).BeginInit();
            this.triggerParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.preTriggerNumeric)).BeginInit();
            this.referenceEdgeGroupBox.SuspendLayout();
            this.acquisitionResultGroupBox.SuspendLayout();
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
            this.channelParametersGroupBox.Location = new System.Drawing.Point(8, 12);
            this.channelParametersGroupBox.Name = "channelParametersGroupBox";
            this.channelParametersGroupBox.Size = new System.Drawing.Size(232, 124);
            this.channelParametersGroupBox.TabIndex = 1;
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
            this.maximumLabel.Location = new System.Drawing.Point(16, 88);
            this.maximumLabel.Name = "maximumLabel";
            this.maximumLabel.Size = new System.Drawing.Size(96, 16);
            this.maximumLabel.TabIndex = 4;
            this.maximumLabel.Text = "Maximum (Volts):";
            // 
            // minimumLabel
            // 
            this.minimumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.minimumLabel.Location = new System.Drawing.Point(16, 56);
            this.minimumLabel.Name = "minimumLabel";
            this.minimumLabel.Size = new System.Drawing.Size(96, 16);
            this.minimumLabel.TabIndex = 2;
            this.minimumLabel.Text = "Minimum (Volts):";
            // 
            // physicalChannelLabel
            // 
            this.physicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.physicalChannelLabel.Location = new System.Drawing.Point(16, 24);
            this.physicalChannelLabel.Name = "physicalChannelLabel";
            this.physicalChannelLabel.Size = new System.Drawing.Size(96, 16);
            this.physicalChannelLabel.TabIndex = 0;
            this.physicalChannelLabel.Text = "Physical Channel:";
            // 
            // timingParametersGroupBox
            // 
            this.timingParametersGroupBox.Controls.Add(this.rateLabel);
            this.timingParametersGroupBox.Controls.Add(this.samplesLabel);
            this.timingParametersGroupBox.Controls.Add(this.samplesPerChannelNumeric);
            this.timingParametersGroupBox.Controls.Add(this.rateNumeric);
            this.timingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.timingParametersGroupBox.Location = new System.Drawing.Point(8, 144);
            this.timingParametersGroupBox.Name = "timingParametersGroupBox";
            this.timingParametersGroupBox.Size = new System.Drawing.Size(232, 88);
            this.timingParametersGroupBox.TabIndex = 2;
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
            // rateNumeric
            // 
            this.rateNumeric.DecimalPlaces = 2;
            this.rateNumeric.Location = new System.Drawing.Point(120, 56);
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
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(84, 256);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(80, 24);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // triggerParametersGroupBox
            // 
            this.triggerParametersGroupBox.Controls.Add(this.preTriggerNumeric);
            this.triggerParametersGroupBox.Controls.Add(this.referenceTriggerSourceTextBox);
            this.triggerParametersGroupBox.Controls.Add(this.referenceTriggerSourceLabel);
            this.triggerParametersGroupBox.Controls.Add(this.preTriggerLabel);
            this.triggerParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.triggerParametersGroupBox.Location = new System.Drawing.Point(264, 12);
            this.triggerParametersGroupBox.Name = "triggerParametersGroupBox";
            this.triggerParametersGroupBox.Size = new System.Drawing.Size(272, 84);
            this.triggerParametersGroupBox.TabIndex = 3;
            this.triggerParametersGroupBox.TabStop = false;
            this.triggerParametersGroupBox.Text = "Trigger Parameters";
            // 
            // preTriggerNumeric
            // 
            this.preTriggerNumeric.Location = new System.Drawing.Point(160, 48);
            this.preTriggerNumeric.Maximum = new System.Decimal(new int[] {
                                                                              32768,
                                                                              0,
                                                                              0,
                                                                              0});
            this.preTriggerNumeric.Name = "preTriggerNumeric";
            this.preTriggerNumeric.Size = new System.Drawing.Size(96, 20);
            this.preTriggerNumeric.TabIndex = 3;
            this.preTriggerNumeric.Value = new System.Decimal(new int[] {
                                                                            100,
                                                                            0,
                                                                            0,
                                                                            0});
            // 
            // referenceTriggerSourceTextBox
            // 
            this.referenceTriggerSourceTextBox.Location = new System.Drawing.Point(160, 16);
            this.referenceTriggerSourceTextBox.Name = "referenceTriggerSourceTextBox";
            this.referenceTriggerSourceTextBox.Size = new System.Drawing.Size(96, 20);
            this.referenceTriggerSourceTextBox.TabIndex = 1;
            this.referenceTriggerSourceTextBox.Text = "/Dev1/PFI0";
            // 
            // referenceTriggerSourceLabel
            // 
            this.referenceTriggerSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.referenceTriggerSourceLabel.Location = new System.Drawing.Point(16, 24);
            this.referenceTriggerSourceLabel.Name = "referenceTriggerSourceLabel";
            this.referenceTriggerSourceLabel.Size = new System.Drawing.Size(144, 16);
            this.referenceTriggerSourceLabel.TabIndex = 0;
            this.referenceTriggerSourceLabel.Text = "Reference Trigger Source:";
            // 
            // preTriggerLabel
            // 
            this.preTriggerLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.preTriggerLabel.Location = new System.Drawing.Point(16, 56);
            this.preTriggerLabel.Name = "preTriggerLabel";
            this.preTriggerLabel.Size = new System.Drawing.Size(112, 16);
            this.preTriggerLabel.TabIndex = 2;
            this.preTriggerLabel.Text = "Pre-Trigger Samples:";
            // 
            // referenceEdgeGroupBox
            // 
            this.referenceEdgeGroupBox.Controls.Add(this.referenceEdgeRisingButton);
            this.referenceEdgeGroupBox.Controls.Add(this.referenceEdgeFallingButton);
            this.referenceEdgeGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.referenceEdgeGroupBox.Location = new System.Drawing.Point(264, 104);
            this.referenceEdgeGroupBox.Name = "referenceEdgeGroupBox";
            this.referenceEdgeGroupBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.referenceEdgeGroupBox.Size = new System.Drawing.Size(272, 48);
            this.referenceEdgeGroupBox.TabIndex = 4;
            this.referenceEdgeGroupBox.TabStop = false;
            this.referenceEdgeGroupBox.Text = "Reference Edge";
            // 
            // referenceEdgeRisingButton
            // 
            this.referenceEdgeRisingButton.Checked = true;
            this.referenceEdgeRisingButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.referenceEdgeRisingButton.Location = new System.Drawing.Point(8, 16);
            this.referenceEdgeRisingButton.Name = "referenceEdgeRisingButton";
            this.referenceEdgeRisingButton.Size = new System.Drawing.Size(56, 24);
            this.referenceEdgeRisingButton.TabIndex = 0;
            this.referenceEdgeRisingButton.TabStop = true;
            this.referenceEdgeRisingButton.Text = "Rising";
            this.referenceEdgeRisingButton.CheckedChanged += new System.EventHandler(this.referenceEdgeRisingButton_CheckedChanged);
            // 
            // referenceEdgeFallingButton
            // 
            this.referenceEdgeFallingButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.referenceEdgeFallingButton.Location = new System.Drawing.Point(72, 16);
            this.referenceEdgeFallingButton.Name = "referenceEdgeFallingButton";
            this.referenceEdgeFallingButton.Size = new System.Drawing.Size(56, 24);
            this.referenceEdgeFallingButton.TabIndex = 1;
            this.referenceEdgeFallingButton.Text = "Falling";
            this.referenceEdgeFallingButton.CheckedChanged += new System.EventHandler(this.referenceEdgeFallingButton_CheckedChanged);
            // 
            // acquisitionResultGroupBox
            // 
            this.acquisitionResultGroupBox.Controls.Add(this.resultLabel);
            this.acquisitionResultGroupBox.Controls.Add(this.acquisitionDataGrid);
            this.acquisitionResultGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.acquisitionResultGroupBox.Location = new System.Drawing.Point(264, 160);
            this.acquisitionResultGroupBox.Name = "acquisitionResultGroupBox";
            this.acquisitionResultGroupBox.Size = new System.Drawing.Size(272, 120);
            this.acquisitionResultGroupBox.TabIndex = 4;
            this.acquisitionResultGroupBox.TabStop = false;
            this.acquisitionResultGroupBox.Text = "Acquisition Results";
            // 
            // resultLabel
            // 
            this.resultLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.resultLabel.Location = new System.Drawing.Point(16, 16);
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
            this.acquisitionDataGrid.Location = new System.Drawing.Point(16, 32);
            this.acquisitionDataGrid.Name = "acquisitionDataGrid";
            this.acquisitionDataGrid.ParentRowsVisible = false;
            this.acquisitionDataGrid.ReadOnly = true;
            this.acquisitionDataGrid.Size = new System.Drawing.Size(248, 80);
            this.acquisitionDataGrid.TabIndex = 1;
            this.acquisitionDataGrid.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(546, 296);
            this.Controls.Add(this.acquisitionResultGroupBox);
            this.Controls.Add(this.triggerParametersGroupBox);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.timingParametersGroupBox);
            this.Controls.Add(this.channelParametersGroupBox);
            this.Controls.Add(this.referenceEdgeGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(584, 336);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Acquire Voltage Samples using a Digital Trigger and the Internal Clock";
            this.channelParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.minimumValueNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maximumValueNumeric)).EndInit();
            this.timingParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerChannelNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rateNumeric)).EndInit();
            this.triggerParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.preTriggerNumeric)).EndInit();
            this.referenceEdgeGroupBox.ResumeLayout(false);
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
            startButton.Enabled = false;
           
            try
            {
                // Create a new task
                myTask = new Task();
                
                // Initialize Local Variables
                double sampleRate = Convert.ToDouble(rateNumeric.Value);
                double rangeMinimum = Convert.ToDouble(minimumValueNumeric.Value);
                double rangeMaximum = Convert.ToDouble(maximumValueNumeric.Value);
                int samplesPerChannel = Convert.ToInt32(samplesPerChannelNumeric.Value);

                // Create a channel
                myTask.AIChannels.CreateVoltageChannel(physicalChannelComboBox.Text,"",
                    (AITerminalConfiguration)(-1),rangeMinimum,rangeMaximum,AIVoltageUnits.Volts);
                    
                // Configure Timing Specs    
                myTask.Timing.ConfigureSampleClock("", sampleRate, SampleClockActiveEdge.Rising,
                    SampleQuantityMode.FiniteSamples, samplesPerChannel);
                    
                // Configure Reference Trigger
                myTask.Triggers.ReferenceTrigger.ConfigureDigitalEdgeTrigger(
                    referenceTriggerSourceTextBox.Text, referenceEdge, Convert.ToInt32(preTriggerNumeric.Value));
                
                // Verify the Task
                myTask.Control(TaskAction.Verify);

                // Prepare the table for Data
                InitializeDataTable(myTask.AIChannels, ref dataTable);   
                acquisitionDataGrid.DataSource = dataTable;

                reader = new AnalogMultiChannelReader(myTask.Stream); 

                // Use SynchronizeCallbacks to specify that the object 
                // marshals callbacks across threads appropriately.
                reader.SynchronizeCallbacks = true;
                reader.BeginReadWaveform(samplesPerChannel, new AsyncCallback(myCallback), null);
            }
            catch(DaqException exception)
            {
                MessageBox.Show(exception.Message);    
                startButton.Enabled = true;
                myTask.Dispose();           
            }
        }

        private void myCallback(IAsyncResult ar)
        {
            try
            {
                // Read the available data from the channels
                data = reader.EndReadWaveform(ar);

                // Plot your data here
                dataToDataTable(data, ref dataTable);
               
            }
            catch (DaqException exception)
            {
                MessageBox.Show(exception.Message);                                
            }
            finally 
            {               
                myTask.Dispose();
                startButton.Enabled = true;
            }
        }
        
        private void referenceEdgeRisingButton_CheckedChanged(object sender, System.EventArgs e)
        {
            // Change the reference edge to rising
            if (referenceEdgeRisingButton.Checked)
            {
                referenceEdge = DigitalEdgeReferenceTriggerEdge.Rising;
            }
        }

        private void referenceEdgeFallingButton_CheckedChanged(object sender, System.EventArgs e)
        {
            // Change the reference edge to falling
            if (referenceEdgeFallingButton.Checked)
            {
                referenceEdge = DigitalEdgeReferenceTriggerEdge.Falling;
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

            for (int currentChannelIndex=0;currentChannelIndex<numOfChannels;currentChannelIndex++)
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
