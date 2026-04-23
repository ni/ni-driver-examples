/******************************************************************************
*
* Example program:
*   ContAcqThermocoupleSamples_IntClk
*
* Category:
*   AI
*
* Description:
*   This example demonstrates how to continuously acquire temperature readings
*   from one or more thermocouples.
*
* Instructions for running:
*   1.  Specify the physical channel where you have connected the thermocouple.
*   2.  Enter the minimum and maximum temperature values in degrees C that you
*       expect to measure. A smaller range will allow a more accurate
*       measurement.
*   3.  Enter the scan rate at which you want to run the acquisition.
*   4.  Specify the type of thermocouple you are using.
*   5.  Thermocouple measurements require cold-junction compensation (CJC) to
*       correctly scale them. Specify the source of your cold-junction
*       compensation.
*   6.  If your CJC source is "Internal", skip the rest of the steps.
*   7.  If your CJC source is "Constant Value", specify the value (usually room
*       temperature) in degrees C.
*   8.  If your CJC source is "Channel", specify the CJC Channel name.
*   9.  Specify the appropriate Auto Zero Mode. See your SCXI device's hardware
*       manual to find out if your device supports this attribute. E-Series
*       devices do not support this attribute.
*
* Steps:
*   1.  Create a new Task.  Create a AIChannel object by using the
*       CreateThermocoupleChannel method.
*   2.  Set the AutoZero mode.  This attribute is set to compensate for input
*       offset errors and may not be supported by all devices.
*   3.  Configure the timing parameters by using the Timing.ConfigureSampleClock
*       method.  Use the device's internal clock, continuous mode acquisition,
*       and the sample rate specified by the user.
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
*   Connect your thermocouple to the terminals corresponding to the physical
*   channel value. For more information on the input and output terminals for
*   your device, open the NI-DAQmx Help, and refer to the NI-DAQmx Device
*   Terminals and Device Considerations books in the table of contents.
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

namespace NationalInstruments.Examples.ContAcqThermocoupleSamples_IntClk
{
    /// <summary>
    /// Summary description for MainForm.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {

        private AnalogMultiChannelReader analogInReader;
        private AsyncCallback myAsyncCallback;
        private Task myTask;
        private Task runningTask;
        private AnalogWaveform<double>[] data;
        private DataColumn[] dataColumn = null;
        private DataTable dataTable = null;
        internal System.Windows.Forms.GroupBox highAccuracySettingsGroupBox;
        internal System.Windows.Forms.ComboBox autoZeroModeComboBox;
        internal System.Windows.Forms.Label autoZeroModeLabel;
        internal System.Windows.Forms.GroupBox acquisitionResultsGroupBox;
        internal System.Windows.Forms.Label resultLabel;
        internal System.Windows.Forms.DataGrid acquisitionDataGrid;
        internal System.Windows.Forms.GroupBox channelParametersGroupBox;
        internal System.Windows.Forms.Label maximumLabel;
        internal System.Windows.Forms.Label minimumLabel;
        internal System.Windows.Forms.Label physicalChannelLabel;
        internal System.Windows.Forms.Button startButton;
        internal System.Windows.Forms.Button stopButton;
        internal System.Windows.Forms.GroupBox timingParametersGroupBox;
        internal System.Windows.Forms.NumericUpDown rateNumeric;
        internal System.Windows.Forms.Label rateLabel;
        internal System.Windows.Forms.GroupBox thermocoupleParametersGroupBox;
        internal System.Windows.Forms.ComboBox thermocoupleTypeComboBox;
        internal System.Windows.Forms.Label thermocoupleTypeLabel;
        internal System.Windows.Forms.GroupBox coldJunctionParametersGroupBox;
        internal System.Windows.Forms.NumericUpDown cjcValueNumeric;
        internal System.Windows.Forms.Label cjcValueLabel;
        internal System.Windows.Forms.Label cjcChannelLabel;
        internal System.Windows.Forms.Label cjcSourceLabel;
        internal System.Windows.Forms.ComboBox cjcSourceComboBox;
        internal System.Windows.Forms.TextBox cjcChannelTextBox;
        internal System.Windows.Forms.Label scxiModuleLabel;
        internal System.Windows.Forms.CheckBox scxiModuleCheckBox;
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
            cjcSourceComboBox.SelectedIndex = 1;
            autoZeroModeComboBox.SelectedIndex = 0;
            thermocoupleTypeComboBox.SelectedIndex = 2;
            myAsyncCallback = new AsyncCallback(AnalogInCallback);
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
            this.highAccuracySettingsGroupBox = new System.Windows.Forms.GroupBox();
            this.autoZeroModeComboBox = new System.Windows.Forms.ComboBox();
            this.scxiModuleLabel = new System.Windows.Forms.Label();
            this.scxiModuleCheckBox = new System.Windows.Forms.CheckBox();
            this.autoZeroModeLabel = new System.Windows.Forms.Label();
            this.acquisitionResultsGroupBox = new System.Windows.Forms.GroupBox();
            this.resultLabel = new System.Windows.Forms.Label();
            this.acquisitionDataGrid = new System.Windows.Forms.DataGrid();
            this.channelParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.physicalChannelComboBox = new System.Windows.Forms.ComboBox();
            this.minimumValueNumeric = new System.Windows.Forms.NumericUpDown();
            this.maximumValueNumeric = new System.Windows.Forms.NumericUpDown();
            this.maximumLabel = new System.Windows.Forms.Label();
            this.minimumLabel = new System.Windows.Forms.Label();
            this.physicalChannelLabel = new System.Windows.Forms.Label();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.timingParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.rateNumeric = new System.Windows.Forms.NumericUpDown();
            this.rateLabel = new System.Windows.Forms.Label();
            this.thermocoupleParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.thermocoupleTypeComboBox = new System.Windows.Forms.ComboBox();
            this.thermocoupleTypeLabel = new System.Windows.Forms.Label();
            this.coldJunctionParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.cjcValueNumeric = new System.Windows.Forms.NumericUpDown();
            this.cjcValueLabel = new System.Windows.Forms.Label();
            this.cjcChannelLabel = new System.Windows.Forms.Label();
            this.cjcSourceLabel = new System.Windows.Forms.Label();
            this.cjcSourceComboBox = new System.Windows.Forms.ComboBox();
            this.cjcChannelTextBox = new System.Windows.Forms.TextBox();
            this.highAccuracySettingsGroupBox.SuspendLayout();
            this.acquisitionResultsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.acquisitionDataGrid)).BeginInit();
            this.channelParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.minimumValueNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maximumValueNumeric)).BeginInit();
            this.timingParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rateNumeric)).BeginInit();
            this.thermocoupleParametersGroupBox.SuspendLayout();
            this.coldJunctionParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cjcValueNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // highAccuracySettingsGroupBox
            // 
            this.highAccuracySettingsGroupBox.Controls.Add(this.autoZeroModeComboBox);
            this.highAccuracySettingsGroupBox.Controls.Add(this.scxiModuleLabel);
            this.highAccuracySettingsGroupBox.Controls.Add(this.scxiModuleCheckBox);
            this.highAccuracySettingsGroupBox.Controls.Add(this.autoZeroModeLabel);
            this.highAccuracySettingsGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.highAccuracySettingsGroupBox.Location = new System.Drawing.Point(256, 8);
            this.highAccuracySettingsGroupBox.Name = "highAccuracySettingsGroupBox";
            this.highAccuracySettingsGroupBox.Size = new System.Drawing.Size(232, 98);
            this.highAccuracySettingsGroupBox.TabIndex = 6;
            this.highAccuracySettingsGroupBox.TabStop = false;
            this.highAccuracySettingsGroupBox.Text = "High Accuracy Settings";
            // 
            // autoZeroModeComboBox
            // 
            this.autoZeroModeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.autoZeroModeComboBox.Items.AddRange(new object[] {
                                                                      "None",
                                                                      "Once"});
            this.autoZeroModeComboBox.Location = new System.Drawing.Point(120, 56);
            this.autoZeroModeComboBox.Name = "autoZeroModeComboBox";
            this.autoZeroModeComboBox.Size = new System.Drawing.Size(104, 21);
            this.autoZeroModeComboBox.TabIndex = 3;
            // 
            // scxiModuleLabel
            // 
            this.scxiModuleLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.scxiModuleLabel.Location = new System.Drawing.Point(16, 28);
            this.scxiModuleLabel.Name = "scxiModuleLabel";
            this.scxiModuleLabel.Size = new System.Drawing.Size(100, 16);
            this.scxiModuleLabel.TabIndex = 0;
            this.scxiModuleLabel.Text = "SCXI Module?:";
            // 
            // scxiModuleCheckBox
            // 
            this.scxiModuleCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.scxiModuleCheckBox.Location = new System.Drawing.Point(120, 24);
            this.scxiModuleCheckBox.Name = "scxiModuleCheckBox";
            this.scxiModuleCheckBox.Size = new System.Drawing.Size(16, 24);
            this.scxiModuleCheckBox.TabIndex = 1;
            this.scxiModuleCheckBox.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // autoZeroModeLabel
            // 
            this.autoZeroModeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.autoZeroModeLabel.Location = new System.Drawing.Point(16, 58);
            this.autoZeroModeLabel.Name = "autoZeroModeLabel";
            this.autoZeroModeLabel.Size = new System.Drawing.Size(88, 16);
            this.autoZeroModeLabel.TabIndex = 2;
            this.autoZeroModeLabel.Text = "Auto Zero Mode:";
            // 
            // acquisitionResultsGroupBox
            // 
            this.acquisitionResultsGroupBox.Controls.Add(this.resultLabel);
            this.acquisitionResultsGroupBox.Controls.Add(this.acquisitionDataGrid);
            this.acquisitionResultsGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.acquisitionResultsGroupBox.Location = new System.Drawing.Point(256, 120);
            this.acquisitionResultsGroupBox.Name = "acquisitionResultsGroupBox";
            this.acquisitionResultsGroupBox.Size = new System.Drawing.Size(280, 248);
            this.acquisitionResultsGroupBox.TabIndex = 7;
            this.acquisitionResultsGroupBox.TabStop = false;
            this.acquisitionResultsGroupBox.Text = "Acquisition Results";
            // 
            // resultLabel
            // 
            this.resultLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.resultLabel.Location = new System.Drawing.Point(16, 24);
            this.resultLabel.Name = "resultLabel";
            this.resultLabel.Size = new System.Drawing.Size(96, 16);
            this.resultLabel.TabIndex = 0;
            this.resultLabel.Text = "Acquisition Data:";
            // 
            // acquisitionDataGrid
            // 
            this.acquisitionDataGrid.AllowSorting = false;
            this.acquisitionDataGrid.DataMember = "";
            this.acquisitionDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.acquisitionDataGrid.Location = new System.Drawing.Point(16, 40);
            this.acquisitionDataGrid.Name = "acquisitionDataGrid";
            this.acquisitionDataGrid.ParentRowsVisible = false;
            this.acquisitionDataGrid.ReadOnly = true;
            this.acquisitionDataGrid.Size = new System.Drawing.Size(256, 200);
            this.acquisitionDataGrid.TabIndex = 1;
            this.acquisitionDataGrid.TabStop = false;
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
            this.channelParametersGroupBox.Size = new System.Drawing.Size(232, 122);
            this.channelParametersGroupBox.TabIndex = 2;
            this.channelParametersGroupBox.TabStop = false;
            this.channelParametersGroupBox.Text = "Channel Parameters";
            // 
            // physicalChannelComboBox
            // 
            this.physicalChannelComboBox.Location = new System.Drawing.Point(120, 24);
            this.physicalChannelComboBox.Name = "physicalChannelComboBox";
            this.physicalChannelComboBox.Size = new System.Drawing.Size(104, 21);
            this.physicalChannelComboBox.TabIndex = 1;
            this.physicalChannelComboBox.Text = "SC1Mod1/ai0";
            // 
            // minimumValueNumeric
            // 
            this.minimumValueNumeric.DecimalPlaces = 2;
            this.minimumValueNumeric.Location = new System.Drawing.Point(120, 88);
            this.minimumValueNumeric.Maximum = new System.Decimal(new int[] {
                                                                                500,
                                                                                0,
                                                                                0,
                                                                                0});
            this.minimumValueNumeric.Minimum = new System.Decimal(new int[] {
                                                                                500,
                                                                                0,
                                                                                0,
                                                                                -2147483648});
            this.minimumValueNumeric.Name = "minimumValueNumeric";
            this.minimumValueNumeric.Size = new System.Drawing.Size(104, 20);
            this.minimumValueNumeric.TabIndex = 5;
            // 
            // maximumValueNumeric
            // 
            this.maximumValueNumeric.DecimalPlaces = 2;
            this.maximumValueNumeric.Location = new System.Drawing.Point(120, 56);
            this.maximumValueNumeric.Maximum = new System.Decimal(new int[] {
                                                                                500,
                                                                                0,
                                                                                0,
                                                                                0});
            this.maximumValueNumeric.Minimum = new System.Decimal(new int[] {
                                                                                500,
                                                                                0,
                                                                                0,
                                                                                -2147483648});
            this.maximumValueNumeric.Name = "maximumValueNumeric";
            this.maximumValueNumeric.Size = new System.Drawing.Size(104, 20);
            this.maximumValueNumeric.TabIndex = 3;
            this.maximumValueNumeric.Value = new System.Decimal(new int[] {
                                                                              100,
                                                                              0,
                                                                              0,
                                                                              0});
            // 
            // maximumLabel
            // 
            this.maximumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.maximumLabel.Location = new System.Drawing.Point(16, 58);
            this.maximumLabel.Name = "maximumLabel";
            this.maximumLabel.Size = new System.Drawing.Size(104, 16);
            this.maximumLabel.TabIndex = 2;
            this.maximumLabel.Text = "Maximum (deg C):";
            // 
            // minimumLabel
            // 
            this.minimumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.minimumLabel.Location = new System.Drawing.Point(16, 90);
            this.minimumLabel.Name = "minimumLabel";
            this.minimumLabel.Size = new System.Drawing.Size(96, 16);
            this.minimumLabel.TabIndex = 4;
            this.minimumLabel.Text = "Minimum (deg C):";
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
            // startButton
            // 
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.startButton.Location = new System.Drawing.Point(288, 384);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(80, 24);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.stopButton.Location = new System.Drawing.Point(424, 384);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(80, 24);
            this.stopButton.TabIndex = 1;
            this.stopButton.Text = "Stop";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // timingParametersGroupBox
            // 
            this.timingParametersGroupBox.Controls.Add(this.rateNumeric);
            this.timingParametersGroupBox.Controls.Add(this.rateLabel);
            this.timingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.timingParametersGroupBox.Location = new System.Drawing.Point(8, 144);
            this.timingParametersGroupBox.Name = "timingParametersGroupBox";
            this.timingParametersGroupBox.Size = new System.Drawing.Size(232, 56);
            this.timingParametersGroupBox.TabIndex = 3;
            this.timingParametersGroupBox.TabStop = false;
            this.timingParametersGroupBox.Text = "Timing Parameters";
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
            this.rateNumeric.Size = new System.Drawing.Size(104, 20);
            this.rateNumeric.TabIndex = 1;
            this.rateNumeric.Value = new System.Decimal(new int[] {
                                                                      10,
                                                                      0,
                                                                      0,
                                                                      0});
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
            // thermocoupleParametersGroupBox
            // 
            this.thermocoupleParametersGroupBox.Controls.Add(this.thermocoupleTypeComboBox);
            this.thermocoupleParametersGroupBox.Controls.Add(this.thermocoupleTypeLabel);
            this.thermocoupleParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.thermocoupleParametersGroupBox.Location = new System.Drawing.Point(8, 216);
            this.thermocoupleParametersGroupBox.Name = "thermocoupleParametersGroupBox";
            this.thermocoupleParametersGroupBox.Size = new System.Drawing.Size(232, 56);
            this.thermocoupleParametersGroupBox.TabIndex = 4;
            this.thermocoupleParametersGroupBox.TabStop = false;
            this.thermocoupleParametersGroupBox.Text = "Thermocouple Parameters";
            // 
            // thermocoupleTypeComboBox
            // 
            this.thermocoupleTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.thermocoupleTypeComboBox.Items.AddRange(new object[] {
                                                                          "B",
                                                                          "E",
                                                                          "J",
                                                                          "K",
                                                                          "N",
                                                                          "R",
                                                                          "S",
                                                                          "T"});
            this.thermocoupleTypeComboBox.Location = new System.Drawing.Point(120, 24);
            this.thermocoupleTypeComboBox.Name = "thermocoupleTypeComboBox";
            this.thermocoupleTypeComboBox.Size = new System.Drawing.Size(104, 21);
            this.thermocoupleTypeComboBox.TabIndex = 1;
            // 
            // thermocoupleTypeLabel
            // 
            this.thermocoupleTypeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.thermocoupleTypeLabel.Location = new System.Drawing.Point(16, 26);
            this.thermocoupleTypeLabel.Name = "thermocoupleTypeLabel";
            this.thermocoupleTypeLabel.Size = new System.Drawing.Size(112, 16);
            this.thermocoupleTypeLabel.TabIndex = 0;
            this.thermocoupleTypeLabel.Text = "Thermocouple Type:";
            // 
            // coldJunctionParametersGroupBox
            // 
            this.coldJunctionParametersGroupBox.Controls.Add(this.cjcValueNumeric);
            this.coldJunctionParametersGroupBox.Controls.Add(this.cjcValueLabel);
            this.coldJunctionParametersGroupBox.Controls.Add(this.cjcChannelLabel);
            this.coldJunctionParametersGroupBox.Controls.Add(this.cjcSourceLabel);
            this.coldJunctionParametersGroupBox.Controls.Add(this.cjcSourceComboBox);
            this.coldJunctionParametersGroupBox.Controls.Add(this.cjcChannelTextBox);
            this.coldJunctionParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.coldJunctionParametersGroupBox.Location = new System.Drawing.Point(8, 288);
            this.coldJunctionParametersGroupBox.Name = "coldJunctionParametersGroupBox";
            this.coldJunctionParametersGroupBox.Size = new System.Drawing.Size(232, 120);
            this.coldJunctionParametersGroupBox.TabIndex = 5;
            this.coldJunctionParametersGroupBox.TabStop = false;
            this.coldJunctionParametersGroupBox.Text = "Cold Junction Parameters";
            // 
            // cjcValueNumeric
            // 
            this.cjcValueNumeric.DecimalPlaces = 2;
            this.cjcValueNumeric.Location = new System.Drawing.Point(120, 88);
            this.cjcValueNumeric.Name = "cjcValueNumeric";
            this.cjcValueNumeric.Size = new System.Drawing.Size(104, 20);
            this.cjcValueNumeric.TabIndex = 5;
            this.cjcValueNumeric.Value = new System.Decimal(new int[] {
                                                                          25,
                                                                          0,
                                                                          0,
                                                                          0});
            // 
            // cjcValueLabel
            // 
            this.cjcValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cjcValueLabel.Location = new System.Drawing.Point(16, 90);
            this.cjcValueLabel.Name = "cjcValueLabel";
            this.cjcValueLabel.Size = new System.Drawing.Size(104, 16);
            this.cjcValueLabel.TabIndex = 4;
            this.cjcValueLabel.Text = "CJC Value (deg C):";
            // 
            // cjcChannelLabel
            // 
            this.cjcChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cjcChannelLabel.Location = new System.Drawing.Point(16, 58);
            this.cjcChannelLabel.Name = "cjcChannelLabel";
            this.cjcChannelLabel.Size = new System.Drawing.Size(80, 16);
            this.cjcChannelLabel.TabIndex = 2;
            this.cjcChannelLabel.Text = "CJC Channel:";
            // 
            // cjcSourceLabel
            // 
            this.cjcSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cjcSourceLabel.Location = new System.Drawing.Point(16, 26);
            this.cjcSourceLabel.Name = "cjcSourceLabel";
            this.cjcSourceLabel.Size = new System.Drawing.Size(88, 16);
            this.cjcSourceLabel.TabIndex = 0;
            this.cjcSourceLabel.Text = "CJC Source:";
            // 
            // cjcSourceComboBox
            // 
            this.cjcSourceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cjcSourceComboBox.Items.AddRange(new object[] {
                                                                   "Channel",
                                                                   "Constant Value",
                                                                   "Internal"});
            this.cjcSourceComboBox.Location = new System.Drawing.Point(120, 24);
            this.cjcSourceComboBox.Name = "cjcSourceComboBox";
            this.cjcSourceComboBox.Size = new System.Drawing.Size(104, 21);
            this.cjcSourceComboBox.TabIndex = 1;
            this.cjcSourceComboBox.SelectedIndexChanged += new System.EventHandler(this.cjcSourceComboBox_SelectedIndexChanged);
            // 
            // cjcChannelTextBox
            // 
            this.cjcChannelTextBox.Location = new System.Drawing.Point(120, 56);
            this.cjcChannelTextBox.Name = "cjcChannelTextBox";
            this.cjcChannelTextBox.Size = new System.Drawing.Size(104, 20);
            this.cjcChannelTextBox.TabIndex = 3;
            this.cjcChannelTextBox.Text = "";
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(546, 416);
            this.Controls.Add(this.highAccuracySettingsGroupBox);
            this.Controls.Add(this.acquisitionResultsGroupBox);
            this.Controls.Add(this.channelParametersGroupBox);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.timingParametersGroupBox);
            this.Controls.Add(this.thermocoupleParametersGroupBox);
            this.Controls.Add(this.coldJunctionParametersGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Continuous Acquisition of Thermocouple Samples - Internal Clock";
            this.highAccuracySettingsGroupBox.ResumeLayout(false);
            this.acquisitionResultsGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.acquisitionDataGrid)).EndInit();
            this.channelParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.minimumValueNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maximumValueNumeric)).EndInit();
            this.timingParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rateNumeric)).EndInit();
            this.thermocoupleParametersGroupBox.ResumeLayout(false);
            this.coldJunctionParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cjcValueNumeric)).EndInit();
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
            stopButton.Enabled = true;

            try 
            {
                myTask = new Task();
                AIChannel myChannel;
                AIThermocoupleType thermocoupleType;
                AIAutoZeroMode autoZeroMode;

                switch (thermocoupleTypeComboBox.SelectedIndex)
                {
                    case 0:
                        thermocoupleType = AIThermocoupleType.B;
                        break;
                    case 1:
                        thermocoupleType = AIThermocoupleType.E;
                        break;
                    case 2:
                        thermocoupleType = AIThermocoupleType.J;
                        break;
                    case 3:
                        thermocoupleType = AIThermocoupleType.K;
                        break;
                    case 4:
                        thermocoupleType = AIThermocoupleType.N;
                        break;
                    case 5:
                        thermocoupleType = AIThermocoupleType.R;
                        break;
                    case 6:
                        thermocoupleType = AIThermocoupleType.S;
                        break;
                    case 7:
                    default: 
                        thermocoupleType = AIThermocoupleType.T;
                        break;
                }

                switch (cjcSourceComboBox.SelectedIndex)
                {
                    case 0: // Channel
                        myChannel = myTask.AIChannels.CreateThermocoupleChannel(physicalChannelComboBox.Text,
                            "",Convert.ToDouble(minimumValueNumeric.Value),Convert.ToDouble(maximumValueNumeric.Value),
                            thermocoupleType,AITemperatureUnits.DegreesC,cjcChannelTextBox.Text);
                        break;
                    case 1: // Constant
                        myChannel = myTask.AIChannels.CreateThermocoupleChannel(physicalChannelComboBox.Text,
                            "",Convert.ToDouble(minimumValueNumeric.Value),Convert.ToDouble(maximumValueNumeric.Value),
                            thermocoupleType,AITemperatureUnits.DegreesC,Convert.ToDouble(cjcValueNumeric.Value));
                        break;
                    case 2: // Internal
                    default: 
                        myChannel = myTask.AIChannels.CreateThermocoupleChannel(physicalChannelComboBox.Text,
                            "",Convert.ToDouble(minimumValueNumeric.Value),Convert.ToDouble(maximumValueNumeric.Value),
                            thermocoupleType,AITemperatureUnits.DegreesC);
                        break;
                }                

                if (scxiModuleCheckBox.Checked)
                {
                    switch (autoZeroModeComboBox.SelectedIndex) 
                    {
                        case 0:
                            autoZeroMode = AIAutoZeroMode.None;
                            break;
                        case 1:
                        default: 
                            autoZeroMode = AIAutoZeroMode.Once;
                            break;
                    }
                    myChannel.AutoZeroMode = autoZeroMode;
                }

                myTask.Timing.ConfigureSampleClock("",Convert.ToDouble(rateNumeric.Value),
                    SampleClockActiveEdge.Rising, SampleQuantityMode.ContinuousSamples, 1000);

                myTask.Control(TaskAction.Verify);

                analogInReader = new AnalogMultiChannelReader(myTask.Stream);
                
                runningTask = myTask;
                InitializeDataTable(myTask.AIChannels, ref dataTable);
                acquisitionDataGrid.DataSource = dataTable;

                // Use SynchronizeCallbacks to specify that the object 
                // marshals callbacks across threads appropriately.
                analogInReader.SynchronizeCallbacks = true;
                analogInReader.BeginReadWaveform(10, myAsyncCallback, myTask);
            }
            catch (DaqException exception)
            {
                MessageBox.Show(exception.Message);
                myTask.Dispose();
                startButton.Enabled = true;
                stopButton.Enabled = false;
                runningTask = null;
            }
        }

        private void stopButton_Click(object sender, System.EventArgs e)
        {
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
                    data = analogInReader.EndReadWaveform(ar);
                  
                    dataToDataTable(data, ref dataTable);

                    analogInReader.BeginMemoryOptimizedReadWaveform(10, myAsyncCallback, myTask, data);
                }
            }
            catch (DaqException exception)
            {
                MessageBox.Show(exception.Message);
                myTask.Dispose();
                startButton.Enabled = true;
                stopButton.Enabled = false;
                runningTask = null;
            }
            
        }

        private void cjcSourceComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            switch (cjcSourceComboBox.SelectedIndex)
            {
                case 0: // Channel
                    cjcChannelTextBox.Enabled = true;
                    cjcValueNumeric.Enabled = false;
                    break;
                case 1: // Constant 
                    cjcChannelTextBox.Enabled = false;
                    cjcValueNumeric.Enabled = true;
                    break;
                case 2: // Internal
                    cjcChannelTextBox.Enabled = false;
                    cjcValueNumeric.Enabled = false;
                    break;
            }
        }

        private void InitializeDataTable(AIChannelCollection channelCollection,ref DataTable data)
        {
            if (channelCollection == null)
                return;

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

            for(int currentDataIndex = 0; currentDataIndex < numOfRows; currentDataIndex++)             
            {
                object[] rowArr = new object[numOfChannels];
                data.Rows.Add(rowArr);              
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


    }
}
