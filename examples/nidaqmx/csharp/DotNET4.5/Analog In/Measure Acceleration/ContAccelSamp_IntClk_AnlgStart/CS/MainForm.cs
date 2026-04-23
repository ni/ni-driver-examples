/******************************************************************************
*
* Example program:
*   ContAccelSamp_IntClk_AnlgStart
*
* Category:
*   AI
*
* Description:
*   This example demonstrates how to create an analog input acceleration task
*   and perform a continuous acquisition using option IEPE excitation, analog
*   triggering, and overload detection.
*
* Instructions for running:
*   1.  Select the physical channel to correspond to where your signal is input
*       on your device.
*   2.  Enter the minimum and maximum expected acceleration values.Note:  To
*       optimize gain selection,  try to match the Input Ranges to the expected 
*       level of the measured signal.
*   3.  Program the analog input terminal configuration and IEPE excitation
*       settings for your device.
*   4.  If your device supports overload detection, check the Overload Detection
*       checkbox.  Refer to your device documentation to see if overload
*       protection is supported.
*   5.  Set the rate of the acquisition.  Also set the Samples to Read control. 
*       This will determine how many samples are acquired with each read from
*       the device.  This also determines how many points are plotted on the
*       graph each iteration.Note:  The rate should be at least twice as fast as
*       the maximum frequency component of the signal being acquired.
*   6.  Set the source of the Analog Edge Start Trigger. By default this is
*       APFI0.
*   7.  Set the slope and level of desired analog edge condition.
*   8.  Input the sensitivity and units for your accelerometer.
*
* Steps:
*   1.  Create a Task object and create an analog input acceleration channel. 
*       This step defines accelerometer sensitivity, desired range, and IEPE
*       excitation.
*   2.  Set input coupling.
*   3.  Set the sample rate and define a continuous acquisition.
*   4.  Define the trigger channel, trigger level, and rising/falling edge for
*       an analog start trigger.
*   5.  Create the AnalogMultiChannelReader object.
*   6.  Call AnalogMultiChannelReader.BeginReadWaveform to install a callback
*       and begin the asynchronous read operation.
*   7.  Inside the callback, call AnalogMultiChannelReader.EndReadWaveform to
*       retrieve the data from the read operation.  In the callback, display the
*       data and check for overloaded channels.
*   8.  Call AnalogMultiChannelReader.BeginMemoryOptimizedReadWaveform
*   9.  When the user presses the stop button, stop the task by calling
*       Task.Stop().  Dispose the Task object to clean-up any resources
*       associated with the task.
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
*   Make sure your signal input terminal matches the Physical Channel I/O
*   control.  Also, make sure your analog trigger terminal matches the Trigger
*   Source Control.  For further connection information, refer to your hardware
*   reference manual.
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


namespace NationalInstruments.Examples.ContAcqAccelSamp_IntClk_AnalogStart
{
    /// <summary>
    /// Summary description for MainForm.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.GroupBox channelParametersGroupBox;
        private System.Windows.Forms.Label maximumLabel;
        private System.Windows.Forms.Label minimumLabel;
        private System.Windows.Forms.Label physicalChannelLabel;
        private System.Windows.Forms.GroupBox timingParametersGroupBox;
        private System.Windows.Forms.Label samplesLabel;
        private System.Windows.Forms.Label rateLabel;
        private System.Windows.Forms.GroupBox acquisitionResultsGroupBox;
        private System.Windows.Forms.Label resultLabel;
        private System.Windows.Forms.DataGrid acquisitionDataGrid;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.GroupBox triggerParametersGroupBox;
        private System.Windows.Forms.TextBox triggerSourceTextBox;
        private System.Windows.Forms.Label sensitivityLabel;
        private System.Windows.Forms.Label sensitivityUnitLabel;
        private System.Windows.Forms.ComboBox sensitivityUnitComboBox;

        private AnalogMultiChannelReader analogInReader;
        private AIExcitationSource excitationSource;
        private AIAccelerometerSensitivityUnits sensitivityUnits;
        private AITerminalConfiguration terminalConfiguration;
        private AnalogEdgeStartTriggerSlope triggerSlope;
        private AICoupling inputCoupling;

        private Task myTask;
        private Task runningTask;
        private AsyncCallback analogCallback;
        private AnalogWaveform<double>[] data;
        private DataColumn[] dataColumn = null;
        private DataTable dataTable = null;
        private System.Windows.Forms.NumericUpDown rateNumeric;
        private System.Windows.Forms.NumericUpDown samplesPerChannelNumeric;
        private System.Windows.Forms.NumericUpDown triggerLevelNumeric;
        private System.Windows.Forms.NumericUpDown sensitivityNumeric;
        private System.Windows.Forms.GroupBox accelerometerParametersGroupBox;
        private System.Windows.Forms.Label triggerLevelLabel;
        private System.Windows.Forms.Label triggerSlopeLabel;
        private System.Windows.Forms.Label triggerSourceLabel;
        private System.Windows.Forms.ComboBox triggerSlopeComboBox;
        private System.Windows.Forms.NumericUpDown excitationValueNumeric;
        private System.Windows.Forms.ComboBox excitationSourceComboBox;
        private System.Windows.Forms.ComboBox terminalConfigurationComboBox;
        private System.Windows.Forms.Label excitationValueLabel;
        private System.Windows.Forms.Label excitationSourceLabel;
        private System.Windows.Forms.GroupBox deviceParametersGroupBox;
        private System.Windows.Forms.Label terminalConfigurationLabel;
        internal System.Windows.Forms.NumericUpDown minimumValueNumeric;
        internal System.Windows.Forms.NumericUpDown maximumValueNumeric;
        private System.Windows.Forms.Label triggerSourceInfo;
        private System.Windows.Forms.Label triggerSourceInfoAsterisk;
        private System.Windows.Forms.Label inputCouplingLabel;
        private System.Windows.Forms.ComboBox inputCouplingComboBox;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox overloadDetectedTextBox;
        private System.Windows.Forms.Label overloadedChannels;
        private System.Windows.Forms.ListBox overloadedChannelsListBox;
        private System.Windows.Forms.GroupBox overloadingGroupBox;
        private System.Windows.Forms.Label overloadDetectedLabel;
        private System.Windows.Forms.CheckBox overloadDetectionCheckBox;
        private System.Windows.Forms.Label overloadDetectionLabel;
        private System.Windows.Forms.ComboBox physicalChannelComboBox;
        private System.Windows.Forms.Label hysteresisLabel;
        private System.Windows.Forms.NumericUpDown hysteresisNumeric;
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
            triggerSlopeComboBox.SelectedIndex = 0;
            sensitivityUnitComboBox.SelectedIndex = 0;
            excitationSourceComboBox.SelectedIndex = 0;
            terminalConfigurationComboBox.SelectedIndex = 3;
            inputCouplingComboBox.SelectedIndex = 0;

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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainForm));
            this.triggerParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.triggerSourceInfoAsterisk = new System.Windows.Forms.Label();
            this.triggerLevelNumeric = new System.Windows.Forms.NumericUpDown();
            this.triggerLevelLabel = new System.Windows.Forms.Label();
            this.triggerSlopeLabel = new System.Windows.Forms.Label();
            this.triggerSourceLabel = new System.Windows.Forms.Label();
            this.triggerSourceTextBox = new System.Windows.Forms.TextBox();
            this.triggerSlopeComboBox = new System.Windows.Forms.ComboBox();
            this.triggerSourceInfo = new System.Windows.Forms.Label();
            this.accelerometerParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.sensitivityNumeric = new System.Windows.Forms.NumericUpDown();
            this.sensitivityUnitComboBox = new System.Windows.Forms.ComboBox();
            this.sensitivityUnitLabel = new System.Windows.Forms.Label();
            this.sensitivityLabel = new System.Windows.Forms.Label();
            this.channelParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.physicalChannelComboBox = new System.Windows.Forms.ComboBox();
            this.minimumValueNumeric = new System.Windows.Forms.NumericUpDown();
            this.maximumValueNumeric = new System.Windows.Forms.NumericUpDown();
            this.maximumLabel = new System.Windows.Forms.Label();
            this.minimumLabel = new System.Windows.Forms.Label();
            this.physicalChannelLabel = new System.Windows.Forms.Label();
            this.overloadDetectionLabel = new System.Windows.Forms.Label();
            this.overloadDetectionCheckBox = new System.Windows.Forms.CheckBox();
            this.timingParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.samplesPerChannelNumeric = new System.Windows.Forms.NumericUpDown();
            this.rateNumeric = new System.Windows.Forms.NumericUpDown();
            this.samplesLabel = new System.Windows.Forms.Label();
            this.rateLabel = new System.Windows.Forms.Label();
            this.acquisitionResultsGroupBox = new System.Windows.Forms.GroupBox();
            this.resultLabel = new System.Windows.Forms.Label();
            this.acquisitionDataGrid = new System.Windows.Forms.DataGrid();
            this.stopButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.excitationValueNumeric = new System.Windows.Forms.NumericUpDown();
            this.excitationSourceComboBox = new System.Windows.Forms.ComboBox();
            this.terminalConfigurationComboBox = new System.Windows.Forms.ComboBox();
            this.excitationValueLabel = new System.Windows.Forms.Label();
            this.excitationSourceLabel = new System.Windows.Forms.Label();
            this.terminalConfigurationLabel = new System.Windows.Forms.Label();
            this.inputCouplingLabel = new System.Windows.Forms.Label();
            this.deviceParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.inputCouplingComboBox = new System.Windows.Forms.ComboBox();
            this.overloadDetectedTextBox = new System.Windows.Forms.TextBox();
            this.overloadedChannels = new System.Windows.Forms.Label();
            this.overloadedChannelsListBox = new System.Windows.Forms.ListBox();
            this.overloadingGroupBox = new System.Windows.Forms.GroupBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.overloadDetectedLabel = new System.Windows.Forms.Label();
            this.hysteresisLabel = new System.Windows.Forms.Label();
            this.hysteresisNumeric = new System.Windows.Forms.NumericUpDown();
            this.triggerParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.triggerLevelNumeric)).BeginInit();
            this.accelerometerParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sensitivityNumeric)).BeginInit();
            this.channelParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.minimumValueNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maximumValueNumeric)).BeginInit();
            this.timingParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerChannelNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rateNumeric)).BeginInit();
            this.acquisitionResultsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.acquisitionDataGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.excitationValueNumeric)).BeginInit();
            this.deviceParametersGroupBox.SuspendLayout();
            this.overloadingGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.hysteresisNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // triggerParametersGroupBox
            // 
            this.triggerParametersGroupBox.Controls.Add(this.triggerSourceInfoAsterisk);
            this.triggerParametersGroupBox.Controls.Add(this.triggerLevelNumeric);
            this.triggerParametersGroupBox.Controls.Add(this.triggerLevelLabel);
            this.triggerParametersGroupBox.Controls.Add(this.triggerSlopeLabel);
            this.triggerParametersGroupBox.Controls.Add(this.triggerSourceLabel);
            this.triggerParametersGroupBox.Controls.Add(this.triggerSourceTextBox);
            this.triggerParametersGroupBox.Controls.Add(this.triggerSlopeComboBox);
            this.triggerParametersGroupBox.Controls.Add(this.triggerSourceInfo);
            this.triggerParametersGroupBox.Controls.Add(this.hysteresisLabel);
            this.triggerParametersGroupBox.Controls.Add(this.hysteresisNumeric);
            this.triggerParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.triggerParametersGroupBox.Location = new System.Drawing.Point(8, 304);
            this.triggerParametersGroupBox.Name = "triggerParametersGroupBox";
            this.triggerParametersGroupBox.Size = new System.Drawing.Size(232, 208);
            this.triggerParametersGroupBox.TabIndex = 4;
            this.triggerParametersGroupBox.TabStop = false;
            this.triggerParametersGroupBox.Text = "Trigger Parameters";
            // 
            // triggerSourceInfoAsterisk
            // 
            this.triggerSourceInfoAsterisk.Location = new System.Drawing.Point(8, 144);
            this.triggerSourceInfoAsterisk.Name = "triggerSourceInfoAsterisk";
            this.triggerSourceInfoAsterisk.Size = new System.Drawing.Size(8, 23);
            this.triggerSourceInfoAsterisk.TabIndex = 6;
            this.triggerSourceInfoAsterisk.Text = "*";
            // 
            // triggerLevelNumeric
            // 
            this.triggerLevelNumeric.DecimalPlaces = 2;
            this.triggerLevelNumeric.Location = new System.Drawing.Point(120, 88);
            this.triggerLevelNumeric.Maximum = new System.Decimal(new int[] {
                                                                                10,
                                                                                0,
                                                                                0,
                                                                                0});
            this.triggerLevelNumeric.Minimum = new System.Decimal(new int[] {
                                                                                10,
                                                                                0,
                                                                                0,
                                                                                -2147483648});
            this.triggerLevelNumeric.Name = "triggerLevelNumeric";
            this.triggerLevelNumeric.Size = new System.Drawing.Size(96, 20);
            this.triggerLevelNumeric.TabIndex = 5;
            // 
            // triggerLevelLabel
            // 
            this.triggerLevelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.triggerLevelLabel.Location = new System.Drawing.Point(16, 90);
            this.triggerLevelLabel.Name = "triggerLevelLabel";
            this.triggerLevelLabel.Size = new System.Drawing.Size(96, 16);
            this.triggerLevelLabel.TabIndex = 4;
            this.triggerLevelLabel.Text = "Trigger Level (g):";
            // 
            // triggerSlopeLabel
            // 
            this.triggerSlopeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.triggerSlopeLabel.Location = new System.Drawing.Point(16, 58);
            this.triggerSlopeLabel.Name = "triggerSlopeLabel";
            this.triggerSlopeLabel.Size = new System.Drawing.Size(88, 16);
            this.triggerSlopeLabel.TabIndex = 2;
            this.triggerSlopeLabel.Text = "Trigger Slope:";
            // 
            // triggerSourceLabel
            // 
            this.triggerSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.triggerSourceLabel.Location = new System.Drawing.Point(16, 26);
            this.triggerSourceLabel.Name = "triggerSourceLabel";
            this.triggerSourceLabel.Size = new System.Drawing.Size(88, 16);
            this.triggerSourceLabel.TabIndex = 0;
            this.triggerSourceLabel.Text = "Trigger Source:*";
            // 
            // triggerSourceTextBox
            // 
            this.triggerSourceTextBox.Location = new System.Drawing.Point(120, 24);
            this.triggerSourceTextBox.Name = "triggerSourceTextBox";
            this.triggerSourceTextBox.Size = new System.Drawing.Size(96, 20);
            this.triggerSourceTextBox.TabIndex = 1;
            this.triggerSourceTextBox.Text = "APFI0";
            // 
            // triggerSlopeComboBox
            // 
            this.triggerSlopeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.triggerSlopeComboBox.Items.AddRange(new object[] {
                                                                      "Rising",
                                                                      "Falling"});
            this.triggerSlopeComboBox.Location = new System.Drawing.Point(120, 56);
            this.triggerSlopeComboBox.Name = "triggerSlopeComboBox";
            this.triggerSlopeComboBox.Size = new System.Drawing.Size(96, 21);
            this.triggerSlopeComboBox.TabIndex = 3;
            // 
            // triggerSourceInfo
            // 
            this.triggerSourceInfo.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.triggerSourceInfo.Location = new System.Drawing.Point(16, 144);
            this.triggerSourceInfo.Name = "triggerSourceInfo";
            this.triggerSourceInfo.Size = new System.Drawing.Size(208, 56);
            this.triggerSourceInfo.TabIndex = 7;
            this.triggerSourceInfo.Text = "APFI0 is the default Analog Trigger pin for M Series devices.  Please refer to you" +
                "r device documentation for information regarding valid Analog Triggers for your " +
                "device.";
            // 
            // accelerometerParametersGroupBox
            // 
            this.accelerometerParametersGroupBox.Controls.Add(this.sensitivityNumeric);
            this.accelerometerParametersGroupBox.Controls.Add(this.sensitivityUnitComboBox);
            this.accelerometerParametersGroupBox.Controls.Add(this.sensitivityUnitLabel);
            this.accelerometerParametersGroupBox.Controls.Add(this.sensitivityLabel);
            this.accelerometerParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.accelerometerParametersGroupBox.Location = new System.Drawing.Point(8, 520);
            this.accelerometerParametersGroupBox.Name = "accelerometerParametersGroupBox";
            this.accelerometerParametersGroupBox.Size = new System.Drawing.Size(232, 88);
            this.accelerometerParametersGroupBox.TabIndex = 5;
            this.accelerometerParametersGroupBox.TabStop = false;
            this.accelerometerParametersGroupBox.Text = "Accelerometer Parameters";
            // 
            // sensitivityNumeric
            // 
            this.sensitivityNumeric.DecimalPlaces = 2;
            this.sensitivityNumeric.Location = new System.Drawing.Point(120, 24);
            this.sensitivityNumeric.Maximum = new System.Decimal(new int[] {
                                                                               1000,
                                                                               0,
                                                                               0,
                                                                               0});
            this.sensitivityNumeric.Name = "sensitivityNumeric";
            this.sensitivityNumeric.Size = new System.Drawing.Size(96, 20);
            this.sensitivityNumeric.TabIndex = 1;
            this.sensitivityNumeric.Value = new System.Decimal(new int[] {
                                                                             50,
                                                                             0,
                                                                             0,
                                                                             0});
            // 
            // sensitivityUnitComboBox
            // 
            this.sensitivityUnitComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sensitivityUnitComboBox.Items.AddRange(new object[] {
                                                                         "mVolts/G",
                                                                         "Volts/G"});
            this.sensitivityUnitComboBox.Location = new System.Drawing.Point(120, 56);
            this.sensitivityUnitComboBox.Name = "sensitivityUnitComboBox";
            this.sensitivityUnitComboBox.Size = new System.Drawing.Size(96, 21);
            this.sensitivityUnitComboBox.TabIndex = 3;
            // 
            // sensitivityUnitLabel
            // 
            this.sensitivityUnitLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.sensitivityUnitLabel.Location = new System.Drawing.Point(16, 58);
            this.sensitivityUnitLabel.Name = "sensitivityUnitLabel";
            this.sensitivityUnitLabel.Size = new System.Drawing.Size(88, 16);
            this.sensitivityUnitLabel.TabIndex = 2;
            this.sensitivityUnitLabel.Text = "Sensitivity Units:";
            // 
            // sensitivityLabel
            // 
            this.sensitivityLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.sensitivityLabel.Location = new System.Drawing.Point(16, 26);
            this.sensitivityLabel.Name = "sensitivityLabel";
            this.sensitivityLabel.Size = new System.Drawing.Size(88, 16);
            this.sensitivityLabel.TabIndex = 0;
            this.sensitivityLabel.Text = "Sensitivity:";
            // 
            // channelParametersGroupBox
            // 
            this.channelParametersGroupBox.Controls.Add(this.physicalChannelComboBox);
            this.channelParametersGroupBox.Controls.Add(this.minimumValueNumeric);
            this.channelParametersGroupBox.Controls.Add(this.maximumValueNumeric);
            this.channelParametersGroupBox.Controls.Add(this.maximumLabel);
            this.channelParametersGroupBox.Controls.Add(this.minimumLabel);
            this.channelParametersGroupBox.Controls.Add(this.physicalChannelLabel);
            this.channelParametersGroupBox.Controls.Add(this.overloadDetectionLabel);
            this.channelParametersGroupBox.Controls.Add(this.overloadDetectionCheckBox);
            this.channelParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.channelParametersGroupBox.Location = new System.Drawing.Point(8, 8);
            this.channelParametersGroupBox.Name = "channelParametersGroupBox";
            this.channelParametersGroupBox.Size = new System.Drawing.Size(232, 192);
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
            this.maximumLabel.Location = new System.Drawing.Point(16, 90);
            this.maximumLabel.Name = "maximumLabel";
            this.maximumLabel.Size = new System.Drawing.Size(112, 16);
            this.maximumLabel.TabIndex = 4;
            this.maximumLabel.Text = "Maximum Value (g):";
            // 
            // minimumLabel
            // 
            this.minimumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.minimumLabel.Location = new System.Drawing.Point(16, 57);
            this.minimumLabel.Name = "minimumLabel";
            this.minimumLabel.Size = new System.Drawing.Size(104, 18);
            this.minimumLabel.TabIndex = 2;
            this.minimumLabel.Text = "Minimum Value (g):";
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
            // overloadDetectionLabel
            // 
            this.overloadDetectionLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.overloadDetectionLabel.Location = new System.Drawing.Point(16, 152);
            this.overloadDetectionLabel.Name = "overloadDetectionLabel";
            this.overloadDetectionLabel.Size = new System.Drawing.Size(208, 24);
            this.overloadDetectionLabel.TabIndex = 7;
            this.overloadDetectionLabel.Text = "* Check this box if you are using a DSA device and want to check for Overloads";
            // 
            // overloadDetectionCheckBox
            // 
            this.overloadDetectionCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.overloadDetectionCheckBox.Location = new System.Drawing.Point(48, 120);
            this.overloadDetectionCheckBox.Name = "overloadDetectionCheckBox";
            this.overloadDetectionCheckBox.Size = new System.Drawing.Size(136, 24);
            this.overloadDetectionCheckBox.TabIndex = 6;
            this.overloadDetectionCheckBox.Text = "Overload Detection *";
            // 
            // timingParametersGroupBox
            // 
            this.timingParametersGroupBox.Controls.Add(this.samplesPerChannelNumeric);
            this.timingParametersGroupBox.Controls.Add(this.rateNumeric);
            this.timingParametersGroupBox.Controls.Add(this.samplesLabel);
            this.timingParametersGroupBox.Controls.Add(this.rateLabel);
            this.timingParametersGroupBox.Location = new System.Drawing.Point(8, 208);
            this.timingParametersGroupBox.Name = "timingParametersGroupBox";
            this.timingParametersGroupBox.Size = new System.Drawing.Size(232, 88);
            this.timingParametersGroupBox.TabIndex = 3;
            this.timingParametersGroupBox.TabStop = false;
            this.timingParametersGroupBox.Text = "Timing Parameters";
            // 
            // samplesPerChannelNumeric
            // 
            this.samplesPerChannelNumeric.Location = new System.Drawing.Point(120, 56);
            this.samplesPerChannelNumeric.Maximum = new System.Decimal(new int[] {
                                                                                     1000000,
                                                                                     0,
                                                                                     0,
                                                                                     0});
            this.samplesPerChannelNumeric.Name = "samplesPerChannelNumeric";
            this.samplesPerChannelNumeric.Size = new System.Drawing.Size(96, 20);
            this.samplesPerChannelNumeric.TabIndex = 3;
            this.samplesPerChannelNumeric.Value = new System.Decimal(new int[] {
                                                                                   1000,
                                                                                   0,
                                                                                   0,
                                                                                   0});
            // 
            // rateNumeric
            // 
            this.rateNumeric.DecimalPlaces = 2;
            this.rateNumeric.Location = new System.Drawing.Point(120, 24);
            this.rateNumeric.Maximum = new System.Decimal(new int[] {
                                                                        1000000,
                                                                        0,
                                                                        0,
                                                                        0});
            this.rateNumeric.Name = "rateNumeric";
            this.rateNumeric.Size = new System.Drawing.Size(96, 20);
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
            this.samplesLabel.Location = new System.Drawing.Point(16, 58);
            this.samplesLabel.Name = "samplesLabel";
            this.samplesLabel.Size = new System.Drawing.Size(104, 16);
            this.samplesLabel.TabIndex = 2;
            this.samplesLabel.Text = "Samples / Channel:";
            // 
            // rateLabel
            // 
            this.rateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rateLabel.Location = new System.Drawing.Point(16, 26);
            this.rateLabel.Name = "rateLabel";
            this.rateLabel.Size = new System.Drawing.Size(56, 16);
            this.rateLabel.TabIndex = 0;
            this.rateLabel.Text = "Rate (Hz):";
            // 
            // acquisitionResultsGroupBox
            // 
            this.acquisitionResultsGroupBox.Controls.Add(this.resultLabel);
            this.acquisitionResultsGroupBox.Controls.Add(this.acquisitionDataGrid);
            this.acquisitionResultsGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.acquisitionResultsGroupBox.Location = new System.Drawing.Point(256, 168);
            this.acquisitionResultsGroupBox.Name = "acquisitionResultsGroupBox";
            this.acquisitionResultsGroupBox.Size = new System.Drawing.Size(264, 432);
            this.acquisitionResultsGroupBox.TabIndex = 7;
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
            this.resultLabel.Text = "Acquisition Data:";
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
            this.acquisitionDataGrid.Size = new System.Drawing.Size(248, 392);
            this.acquisitionDataGrid.TabIndex = 1;
            this.acquisitionDataGrid.TabStop = false;
            // 
            // stopButton
            // 
            this.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.stopButton.Location = new System.Drawing.Point(704, 208);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(80, 24);
            this.stopButton.TabIndex = 1;
            this.stopButton.Text = "Stop";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // startButton
            // 
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.startButton.Location = new System.Drawing.Point(600, 208);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(80, 24);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // excitationValueNumeric
            // 
            this.excitationValueNumeric.DecimalPlaces = 3;
            this.excitationValueNumeric.Location = new System.Drawing.Point(136, 120);
            this.excitationValueNumeric.Name = "excitationValueNumeric";
            this.excitationValueNumeric.TabIndex = 7;
            this.excitationValueNumeric.Value = new System.Decimal(new int[] {
                                                                                 4,
                                                                                 0,
                                                                                 0,
                                                                                 196608});
            // 
            // excitationSourceComboBox
            // 
            this.excitationSourceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.excitationSourceComboBox.ItemHeight = 13;
            this.excitationSourceComboBox.Items.AddRange(new object[] {
                                                                          "Internal",
                                                                          "External",
                                                                          "None"});
            this.excitationSourceComboBox.Location = new System.Drawing.Point(136, 88);
            this.excitationSourceComboBox.Name = "excitationSourceComboBox";
            this.excitationSourceComboBox.Size = new System.Drawing.Size(120, 21);
            this.excitationSourceComboBox.TabIndex = 5;
            // 
            // terminalConfigurationComboBox
            // 
            this.terminalConfigurationComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.terminalConfigurationComboBox.ItemHeight = 13;
            this.terminalConfigurationComboBox.Items.AddRange(new object[] {
                                                                               "Rse",
                                                                               "Nrse",
                                                                               "Differential",
                                                                               "Pseudodifferential",
                                                                               "Let NI-DAQ Choose"});
            this.terminalConfigurationComboBox.Location = new System.Drawing.Point(136, 56);
            this.terminalConfigurationComboBox.Name = "terminalConfigurationComboBox";
            this.terminalConfigurationComboBox.Size = new System.Drawing.Size(120, 21);
            this.terminalConfigurationComboBox.TabIndex = 3;
            // 
            // excitationValueLabel
            // 
            this.excitationValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.excitationValueLabel.Location = new System.Drawing.Point(16, 122);
            this.excitationValueLabel.Name = "excitationValueLabel";
            this.excitationValueLabel.Size = new System.Drawing.Size(128, 16);
            this.excitationValueLabel.TabIndex = 6;
            this.excitationValueLabel.Text = "Excitation Value:";
            // 
            // excitationSourceLabel
            // 
            this.excitationSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.excitationSourceLabel.Location = new System.Drawing.Point(16, 90);
            this.excitationSourceLabel.Name = "excitationSourceLabel";
            this.excitationSourceLabel.Size = new System.Drawing.Size(128, 16);
            this.excitationSourceLabel.TabIndex = 4;
            this.excitationSourceLabel.Text = "Excitation Source:";
            // 
            // terminalConfigurationLabel
            // 
            this.terminalConfigurationLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.terminalConfigurationLabel.Location = new System.Drawing.Point(16, 59);
            this.terminalConfigurationLabel.Name = "terminalConfigurationLabel";
            this.terminalConfigurationLabel.Size = new System.Drawing.Size(128, 14);
            this.terminalConfigurationLabel.TabIndex = 2;
            this.terminalConfigurationLabel.Text = "Terminal Configuration:";
            // 
            // inputCouplingLabel
            // 
            this.inputCouplingLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.inputCouplingLabel.Location = new System.Drawing.Point(16, 26);
            this.inputCouplingLabel.Name = "inputCouplingLabel";
            this.inputCouplingLabel.Size = new System.Drawing.Size(112, 16);
            this.inputCouplingLabel.TabIndex = 0;
            this.inputCouplingLabel.Text = "Input Coupling:";
            // 
            // deviceParametersGroupBox
            // 
            this.deviceParametersGroupBox.Controls.Add(this.excitationValueNumeric);
            this.deviceParametersGroupBox.Controls.Add(this.excitationSourceComboBox);
            this.deviceParametersGroupBox.Controls.Add(this.terminalConfigurationComboBox);
            this.deviceParametersGroupBox.Controls.Add(this.excitationValueLabel);
            this.deviceParametersGroupBox.Controls.Add(this.excitationSourceLabel);
            this.deviceParametersGroupBox.Controls.Add(this.terminalConfigurationLabel);
            this.deviceParametersGroupBox.Controls.Add(this.inputCouplingLabel);
            this.deviceParametersGroupBox.Controls.Add(this.inputCouplingComboBox);
            this.deviceParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.deviceParametersGroupBox.Location = new System.Drawing.Point(256, 8);
            this.deviceParametersGroupBox.Name = "deviceParametersGroupBox";
            this.deviceParametersGroupBox.Size = new System.Drawing.Size(264, 152);
            this.deviceParametersGroupBox.TabIndex = 6;
            this.deviceParametersGroupBox.TabStop = false;
            this.deviceParametersGroupBox.Text = "Device Parameters";
            // 
            // inputCouplingComboBox
            // 
            this.inputCouplingComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.inputCouplingComboBox.ItemHeight = 13;
            this.inputCouplingComboBox.Items.AddRange(new object[] {
                                                                       "AC",
                                                                       "DC",
                                                                       "Gnd"});
            this.inputCouplingComboBox.Location = new System.Drawing.Point(136, 24);
            this.inputCouplingComboBox.Name = "inputCouplingComboBox";
            this.inputCouplingComboBox.Size = new System.Drawing.Size(120, 21);
            this.inputCouplingComboBox.TabIndex = 1;
            // 
            // overloadDetectedTextBox
            // 
            this.overloadDetectedTextBox.Enabled = false;
            this.overloadDetectedTextBox.Location = new System.Drawing.Point(176, 32);
            this.overloadDetectedTextBox.Name = "overloadDetectedTextBox";
            this.overloadDetectedTextBox.TabIndex = 1;
            this.overloadDetectedTextBox.Text = "No";
            // 
            // overloadedChannels
            // 
            this.overloadedChannels.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.overloadedChannels.Location = new System.Drawing.Point(16, 56);
            this.overloadedChannels.Name = "overloadedChannels";
            this.overloadedChannels.Size = new System.Drawing.Size(120, 16);
            this.overloadedChannels.TabIndex = 2;
            this.overloadedChannels.Text = "Overloaded Channels:";
            // 
            // overloadedChannelsListBox
            // 
            this.overloadedChannelsListBox.Enabled = false;
            this.overloadedChannelsListBox.Location = new System.Drawing.Point(16, 72);
            this.overloadedChannelsListBox.Name = "overloadedChannelsListBox";
            this.overloadedChannelsListBox.Size = new System.Drawing.Size(264, 95);
            this.overloadedChannelsListBox.TabIndex = 3;
            // 
            // overloadingGroupBox
            // 
            this.overloadingGroupBox.Controls.Add(this.overloadedChannelsListBox);
            this.overloadingGroupBox.Controls.Add(this.overloadedChannels);
            this.overloadingGroupBox.Controls.Add(this.overloadDetectedTextBox);
            this.overloadingGroupBox.Controls.Add(this.textBox2);
            this.overloadingGroupBox.Controls.Add(this.overloadDetectedLabel);
            this.overloadingGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.overloadingGroupBox.Location = new System.Drawing.Point(536, 8);
            this.overloadingGroupBox.Name = "overloadingGroupBox";
            this.overloadingGroupBox.Size = new System.Drawing.Size(296, 184);
            this.overloadingGroupBox.TabIndex = 8;
            this.overloadingGroupBox.TabStop = false;
            this.overloadingGroupBox.Text = "Overloading";
            // 
            // textBox2
            // 
            this.textBox2.Enabled = false;
            this.textBox2.Location = new System.Drawing.Point(176, 32);
            this.textBox2.Name = "textBox2";
            this.textBox2.TabIndex = 4;
            this.textBox2.Text = "No";
            // 
            // overloadDetectedLabel
            // 
            this.overloadDetectedLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.overloadDetectedLabel.Location = new System.Drawing.Point(16, 32);
            this.overloadDetectedLabel.Name = "overloadDetectedLabel";
            this.overloadDetectedLabel.Size = new System.Drawing.Size(120, 16);
            this.overloadDetectedLabel.TabIndex = 0;
            this.overloadDetectedLabel.Text = "Overloaded Detected:";
            // 
            // hysteresisLabel
            // 
            this.hysteresisLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.hysteresisLabel.Location = new System.Drawing.Point(16, 120);
            this.hysteresisLabel.Name = "hysteresisLabel";
            this.hysteresisLabel.Size = new System.Drawing.Size(96, 16);
            this.hysteresisLabel.TabIndex = 4;
            this.hysteresisLabel.Text = "Hysteresis (g):";
            // 
            // hysteresisNumeric
            // 
            this.hysteresisNumeric.DecimalPlaces = 2;
            this.hysteresisNumeric.Location = new System.Drawing.Point(120, 120);
            this.hysteresisNumeric.Maximum = new System.Decimal(new int[] {
                                                                              10,
                                                                              0,
                                                                              0,
                                                                              0});
            this.hysteresisNumeric.Minimum = new System.Decimal(new int[] {
                                                                              10,
                                                                              0,
                                                                              0,
                                                                              -2147483648});
            this.hysteresisNumeric.Name = "hysteresisNumeric";
            this.hysteresisNumeric.Size = new System.Drawing.Size(96, 20);
            this.hysteresisNumeric.TabIndex = 5;
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(842, 616);
            this.Controls.Add(this.overloadingGroupBox);
            this.Controls.Add(this.triggerParametersGroupBox);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.acquisitionResultsGroupBox);
            this.Controls.Add(this.timingParametersGroupBox);
            this.Controls.Add(this.channelParametersGroupBox);
            this.Controls.Add(this.deviceParametersGroupBox);
            this.Controls.Add(this.accelerometerParametersGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Continuous Acceleration - Internal Clock - Analog Start";
            this.triggerParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.triggerLevelNumeric)).EndInit();
            this.accelerometerParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sensitivityNumeric)).EndInit();
            this.channelParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.minimumValueNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maximumValueNumeric)).EndInit();
            this.timingParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerChannelNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rateNumeric)).EndInit();
            this.acquisitionResultsGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.acquisitionDataGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.excitationValueNumeric)).EndInit();
            this.deviceParametersGroupBox.ResumeLayout(false);
            this.overloadingGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.hysteresisNumeric)).EndInit();
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
                    startButton.Enabled = false;


                    // Get Slope
                    switch (triggerSlopeComboBox.SelectedItem.ToString())
                    {
                        case "Rising":
                            triggerSlope = AnalogEdgeStartTriggerSlope.Rising;
                            break;
                        case "Falling":
                        default:
                            triggerSlope = AnalogEdgeStartTriggerSlope.Falling;
                            break;
                    }

                    // Get Sensitivity Units
                    switch (sensitivityUnitComboBox.SelectedItem.ToString())
                    {
                        case "mVolts/G": // Units mVolts/G
                            sensitivityUnits = AIAccelerometerSensitivityUnits.MillivoltsPerG;
                            break;
                        case "Volts/G":
                        default: // Units Volts/G
                            sensitivityUnits = AIAccelerometerSensitivityUnits.VoltsPerG;
                            break;
                    }

                    // Get Terminal Configuration
                    switch (terminalConfigurationComboBox.SelectedItem.ToString())
                    {
                        case "Rse":
                            terminalConfiguration = AITerminalConfiguration.Rse;
                            break;
                        case "Nrse":
                            terminalConfiguration = AITerminalConfiguration.Nrse;
                            break;
                        case "Differential":
                        default:
                            terminalConfiguration = AITerminalConfiguration.Differential;
                            break;
                        case "Pseudodifferential":
                            terminalConfiguration = AITerminalConfiguration.Pseudodifferential;
                            break;
                        case "Let NI-DAQ Choose":
                            terminalConfiguration = (AITerminalConfiguration)(-1);
                            break;
                    }

                    // Get Ex Source
                    switch (excitationSourceComboBox.SelectedItem.ToString())
                    {
                        case "Internal":
                            excitationSource = AIExcitationSource.Internal;
                            break;
                        case "External":
                            excitationSource = AIExcitationSource.External;
                            break;
                        default:
                            excitationSource = AIExcitationSource.None;
                            break;
                    }

                    // Get Input Coupling
                    switch (inputCouplingComboBox.SelectedItem.ToString())
                    {
                        case "AC":
                            inputCoupling = AICoupling.AC;
                            break;
                        case "DC":
                            inputCoupling = AICoupling.DC;
                            break;
                        case "Gnd":
                            inputCoupling = AICoupling.Ground;
                            break;
                    }

                    // Create a new task
                    myTask = new Task();
                    AIChannel aiChannel;

                    // Create a virtual channel
                    aiChannel = myTask.AIChannels.CreateAccelerometerChannel(physicalChannelComboBox.Text, "",
                        terminalConfiguration, Convert.ToDouble(minimumValueNumeric.Value), Convert.ToDouble(maximumValueNumeric.Value),
                        Convert.ToDouble(sensitivityNumeric.Value), sensitivityUnits, excitationSource,
                        Convert.ToDouble(excitationValueNumeric.Value), AIAccelerationUnits.G);

                    // Setup the input coupling
                    aiChannel.Coupling = inputCoupling;

                    // Configure the timing parameters
                    myTask.Timing.ConfigureSampleClock("", Convert.ToDouble(rateNumeric.Value),
                        SampleClockActiveEdge.Rising, SampleQuantityMode.ContinuousSamples, 1000);

                    // Configure the Analog Trigger
                    myTask.Triggers.StartTrigger.ConfigureAnalogEdgeTrigger(triggerSourceTextBox.Text, triggerSlope,
                        Convert.ToDouble(triggerLevelNumeric.Value));

                    myTask.Triggers.StartTrigger.AnalogEdge.Hysteresis = Convert.ToDouble(hysteresisNumeric.Value);

                    // Verify the Task
                    myTask.Control(TaskAction.Verify);

                    //Prepare the table for Data
                    InitializeDataTable(myTask.AIChannels, ref dataTable);
                    acquisitionDataGrid.DataSource = dataTable;

                    runningTask = myTask;
                    analogInReader = new AnalogMultiChannelReader(myTask.Stream);
                    analogCallback = new AsyncCallback(AnalogInCallback);


                    // Use SynchronizeCallbacks to specify that the object 
                    // marshals callbacks across threads appropriately.
                    analogInReader.SynchronizeCallbacks = true;
                    analogInReader.BeginReadWaveform(Convert.ToInt32(samplesPerChannelNumeric.Value), analogCallback, myTask);
                }
                catch (DaqException exception)
                {
                    // Display Errors
                    MessageBox.Show(exception.Message);
                    runningTask = null;
                    myTask.Dispose();
                    stopButton.Enabled = false;
                    startButton.Enabled = true;
                }
            }

        }

        private void stopButton_Click(object sender, System.EventArgs e)
        {
            // Dispose of the task
            runningTask = null;
            myTask.Dispose();
            stopButton.Enabled = false;
            startButton.Enabled = true;

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

                    // Check for and report any overloaded channels
                    if (overloadDetectionCheckBox.Checked)
                        ReportOverloadedChannels();

                    analogInReader.BeginMemoryOptimizedReadWaveform(Convert.ToInt32(samplesPerChannelNumeric.Value),
                        analogCallback, myTask, data);
                }
            }
            catch (DaqException exception)
            {
                // Display Errors
                MessageBox.Show(exception.Message);
                runningTask = null;
                myTask.Dispose();
                stopButton.Enabled = false;
                startButton.Enabled = true;
            }
        }

        private void ReportOverloadedChannels()
        {
            // Check for and report any overloaded channels
            bool overloaded = myTask.Stream.OverloadedInputChannelsExist;
            overloadedChannelsListBox.Items.Clear();
            if (overloaded)
            {
                string[] chans = myTask.Stream.OverloadedInputChannels;
                overloadedChannelsListBox.Items.AddRange(chans);
            }
            overloadDetectedTextBox.Text = overloaded ? "Yes" : "No";
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


    }
}
