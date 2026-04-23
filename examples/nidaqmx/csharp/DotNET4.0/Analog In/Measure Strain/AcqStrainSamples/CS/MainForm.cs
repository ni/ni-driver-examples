/******************************************************************************
*
* Example program:
*   AcqStrainSamples
*
* Category:
*   AI
*
* Description:
*   This example demonstrates how to perform a strain measurement.
*
* Instructions for running:
*   1.  Enter the list of physical channels, and set the attributes of the
*       strain configuration for these channels.  The maximum and minimum value
*       inputs specify the range that you expect for your measurement.
*   2.  Select the Filter Setting to use. Default means that for a given device
*       its default filter setting and cutoff frequency will be used. Yes will
*       explicitly turn on the filter for a given device and No will explicitly
*       turn off the filter for a given device.
*   3.  Make sure all of the strain gages are in their relaxed state.
*
* Steps:
*   1.  Create a new Task object.  Create a AIChannel object by using the
*       CreateStrainGageChannel method.
*   2.  Set timing parameters by calling the Timing.ConfigureSampleClock method.
*   3.  Set filter parameters for the channel.
*   4.  Call Task.Start() to start the acquisition.
*   5.  Read the data in a loop until the user presses Stop or an error occurs.
*   6.  Dispose the Task object to clean-up any resources associated with the
*       task.
*   7.  Handle any DaqExceptions, if they occur.
*
* I/O Connections Overview:
*   Make sure your signal input terminal matches the physical channel control.
*   In the default case (differential channel ai0) wire the positive lead for
*   your signal to the ACH0 pin on your DAQ device and wire the negative lead
*   for your signal to the ACH8 pin on you DAQ device.  For more information on
*   the input and output terminals for your device, open the NI-DAQmx Help, and
*   refer to the NI-DAQmx Device Terminals and Device Considerations books in
*   the table of contents.
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

namespace NationalInstruments.Examples.AcqStrainSamples
{
    /// <summary>
    /// Summary description for MainForm.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Label physicalChannelLabel;
        private System.Windows.Forms.Label lowPassCutOffLabel;
        private System.Windows.Forms.Label rateLabel;
        private System.Windows.Forms.Label gageFactorLabel;
        private System.Windows.Forms.Label poissonRatioLabel;
        private System.Windows.Forms.Label excitationValueLabel;
        private System.Windows.Forms.Label excitationSourceLabel;
        private System.Windows.Forms.ComboBox strainConfigurationComboBox;
        private System.Windows.Forms.ComboBox excitationSourceComboBox;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.Timer readTimer;
        private System.Windows.Forms.ComboBox filterEnabledComboBox;
        private System.Windows.Forms.Label filterEnabledLabel;
        private Task myTask;
        private DataTable dataTable = null;
        private AIStrainGageConfiguration strainGageConfiguration;
        private AIExcitationSource excitationSource;
        private System.Windows.Forms.GroupBox channelParametersGroupBox;
        private System.Windows.Forms.Label minimumValueLabel;
        private System.Windows.Forms.Label maximumValueLabel;
        private System.Windows.Forms.GroupBox timingParametersGroupBox;
        private System.Windows.Forms.GroupBox strainGageParametersGroupBox;
        private System.Windows.Forms.Label nominalGageResistanceLabel;
        private System.Windows.Forms.GroupBox bridgeParametersGroupBox;
        private System.Windows.Forms.Label leadWireResistanceLabel;
        private System.Windows.Forms.Label initialBridgeVoltageLabel;
        private System.Windows.Forms.Label strainConfigurationLabel;
        private System.Windows.Forms.NumericUpDown cutoffFrequencyNumeric;
        private System.Windows.Forms.NumericUpDown rateNumeric;
        private System.Windows.Forms.NumericUpDown excitationValueNumeric;
        private System.Windows.Forms.NumericUpDown leadWireResistanceNumeric;
        private System.Windows.Forms.NumericUpDown poissonRatioNumeric;
        private System.Windows.Forms.NumericUpDown gageFactorNumeric;
        private System.Windows.Forms.NumericUpDown initialBridgeVoltageNumeric;
        private System.Windows.Forms.NumericUpDown gageResistanceNumeric;
        private System.Windows.Forms.NumericUpDown maximumValueNumeric;
        private System.Windows.Forms.NumericUpDown minimumValueNumeric;
        private System.Windows.Forms.CheckBox bridgeNull;
        private System.Windows.Forms.ComboBox physicalChannelComboBox;
        private GroupBox acquisitionResultGroupBox;
        private DataGrid acquisitionDataGrid;
        private AnalogSingleChannelReader myAIChannelReader;

        public MainForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            dataTable = new DataTable();
            filterEnabledComboBox.SelectedIndex=1;
            excitationSourceComboBox.SelectedIndex=0;
            strainConfigurationComboBox.SelectedIndex=0;
            filterEnabledComboBox.SelectedIndex=1;

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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.channelParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.physicalChannelComboBox = new System.Windows.Forms.ComboBox();
            this.minimumValueNumeric = new System.Windows.Forms.NumericUpDown();
            this.maximumValueNumeric = new System.Windows.Forms.NumericUpDown();
            this.cutoffFrequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.filterEnabledLabel = new System.Windows.Forms.Label();
            this.filterEnabledComboBox = new System.Windows.Forms.ComboBox();
            this.lowPassCutOffLabel = new System.Windows.Forms.Label();
            this.minimumValueLabel = new System.Windows.Forms.Label();
            this.maximumValueLabel = new System.Windows.Forms.Label();
            this.physicalChannelLabel = new System.Windows.Forms.Label();
            this.timingParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.rateNumeric = new System.Windows.Forms.NumericUpDown();
            this.rateLabel = new System.Windows.Forms.Label();
            this.strainGageParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.gageFactorNumeric = new System.Windows.Forms.NumericUpDown();
            this.poissonRatioNumeric = new System.Windows.Forms.NumericUpDown();
            this.nominalGageResistanceLabel = new System.Windows.Forms.Label();
            this.poissonRatioLabel = new System.Windows.Forms.Label();
            this.gageFactorLabel = new System.Windows.Forms.Label();
            this.gageResistanceNumeric = new System.Windows.Forms.NumericUpDown();
            this.bridgeParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.leadWireResistanceNumeric = new System.Windows.Forms.NumericUpDown();
            this.initialBridgeVoltageNumeric = new System.Windows.Forms.NumericUpDown();
            this.excitationValueNumeric = new System.Windows.Forms.NumericUpDown();
            this.excitationSourceComboBox = new System.Windows.Forms.ComboBox();
            this.strainConfigurationComboBox = new System.Windows.Forms.ComboBox();
            this.excitationSourceLabel = new System.Windows.Forms.Label();
            this.leadWireResistanceLabel = new System.Windows.Forms.Label();
            this.excitationValueLabel = new System.Windows.Forms.Label();
            this.initialBridgeVoltageLabel = new System.Windows.Forms.Label();
            this.strainConfigurationLabel = new System.Windows.Forms.Label();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.readTimer = new System.Windows.Forms.Timer(this.components);
            this.bridgeNull = new System.Windows.Forms.CheckBox();
            this.acquisitionResultGroupBox = new System.Windows.Forms.GroupBox();
            this.acquisitionDataGrid = new System.Windows.Forms.DataGrid();
            this.channelParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.minimumValueNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maximumValueNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cutoffFrequencyNumeric)).BeginInit();
            this.timingParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rateNumeric)).BeginInit();
            this.strainGageParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gageFactorNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.poissonRatioNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gageResistanceNumeric)).BeginInit();
            this.bridgeParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.leadWireResistanceNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.initialBridgeVoltageNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.excitationValueNumeric)).BeginInit();
            this.acquisitionResultGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.acquisitionDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // channelParametersGroupBox
            // 
            this.channelParametersGroupBox.Controls.Add(this.physicalChannelComboBox);
            this.channelParametersGroupBox.Controls.Add(this.minimumValueNumeric);
            this.channelParametersGroupBox.Controls.Add(this.maximumValueNumeric);
            this.channelParametersGroupBox.Controls.Add(this.cutoffFrequencyNumeric);
            this.channelParametersGroupBox.Controls.Add(this.filterEnabledLabel);
            this.channelParametersGroupBox.Controls.Add(this.filterEnabledComboBox);
            this.channelParametersGroupBox.Controls.Add(this.lowPassCutOffLabel);
            this.channelParametersGroupBox.Controls.Add(this.minimumValueLabel);
            this.channelParametersGroupBox.Controls.Add(this.maximumValueLabel);
            this.channelParametersGroupBox.Controls.Add(this.physicalChannelLabel);
            this.channelParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.channelParametersGroupBox.Location = new System.Drawing.Point(8, 8);
            this.channelParametersGroupBox.Name = "channelParametersGroupBox";
            this.channelParametersGroupBox.Size = new System.Drawing.Size(168, 272);
            this.channelParametersGroupBox.TabIndex = 2;
            this.channelParametersGroupBox.TabStop = false;
            this.channelParametersGroupBox.Text = "Channel Parameters";
            // 
            // physicalChannelComboBox
            // 
            this.physicalChannelComboBox.Location = new System.Drawing.Point(16, 40);
            this.physicalChannelComboBox.Name = "physicalChannelComboBox";
            this.physicalChannelComboBox.Size = new System.Drawing.Size(112, 21);
            this.physicalChannelComboBox.TabIndex = 1;
            // 
            // minimumValueNumeric
            // 
            this.minimumValueNumeric.DecimalPlaces = 3;
            this.minimumValueNumeric.Location = new System.Drawing.Point(16, 136);
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
            this.minimumValueNumeric.Size = new System.Drawing.Size(112, 20);
            this.minimumValueNumeric.TabIndex = 5;
            this.minimumValueNumeric.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147287040});
            // 
            // maximumValueNumeric
            // 
            this.maximumValueNumeric.DecimalPlaces = 3;
            this.maximumValueNumeric.Location = new System.Drawing.Point(16, 88);
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
            this.maximumValueNumeric.Size = new System.Drawing.Size(112, 20);
            this.maximumValueNumeric.TabIndex = 3;
            this.maximumValueNumeric.Value = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            // 
            // cutoffFrequencyNumeric
            // 
            this.cutoffFrequencyNumeric.DecimalPlaces = 2;
            this.cutoffFrequencyNumeric.Location = new System.Drawing.Point(16, 232);
            this.cutoffFrequencyNumeric.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.cutoffFrequencyNumeric.Name = "cutoffFrequencyNumeric";
            this.cutoffFrequencyNumeric.Size = new System.Drawing.Size(112, 20);
            this.cutoffFrequencyNumeric.TabIndex = 9;
            this.cutoffFrequencyNumeric.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // filterEnabledLabel
            // 
            this.filterEnabledLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.filterEnabledLabel.Location = new System.Drawing.Point(16, 168);
            this.filterEnabledLabel.Name = "filterEnabledLabel";
            this.filterEnabledLabel.Size = new System.Drawing.Size(88, 16);
            this.filterEnabledLabel.TabIndex = 6;
            this.filterEnabledLabel.Text = "Filter Enabled?:";
            // 
            // filterEnabledComboBox
            // 
            this.filterEnabledComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.filterEnabledComboBox.Items.AddRange(new object[] {
            "Default",
            "Yes",
            "No"});
            this.filterEnabledComboBox.Location = new System.Drawing.Point(16, 184);
            this.filterEnabledComboBox.Name = "filterEnabledComboBox";
            this.filterEnabledComboBox.Size = new System.Drawing.Size(112, 21);
            this.filterEnabledComboBox.TabIndex = 7;
            this.filterEnabledComboBox.SelectedIndexChanged += new System.EventHandler(this.filterEnabledComboBox_SelectedIndexChanged);
            // 
            // lowPassCutOffLabel
            // 
            this.lowPassCutOffLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lowPassCutOffLabel.Location = new System.Drawing.Point(16, 216);
            this.lowPassCutOffLabel.Name = "lowPassCutOffLabel";
            this.lowPassCutOffLabel.Size = new System.Drawing.Size(144, 16);
            this.lowPassCutOffLabel.TabIndex = 8;
            this.lowPassCutOffLabel.Text = "Lowpass Cutoff Frequency:";
            // 
            // minimumValueLabel
            // 
            this.minimumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.minimumValueLabel.Location = new System.Drawing.Point(16, 120);
            this.minimumValueLabel.Name = "minimumValueLabel";
            this.minimumValueLabel.Size = new System.Drawing.Size(100, 16);
            this.minimumValueLabel.TabIndex = 4;
            this.minimumValueLabel.Text = "Minimum Value:";
            // 
            // maximumValueLabel
            // 
            this.maximumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.maximumValueLabel.Location = new System.Drawing.Point(16, 72);
            this.maximumValueLabel.Name = "maximumValueLabel";
            this.maximumValueLabel.Size = new System.Drawing.Size(100, 16);
            this.maximumValueLabel.TabIndex = 2;
            this.maximumValueLabel.Text = "Maximum Value:";
            // 
            // physicalChannelLabel
            // 
            this.physicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.physicalChannelLabel.Location = new System.Drawing.Point(16, 24);
            this.physicalChannelLabel.Name = "physicalChannelLabel";
            this.physicalChannelLabel.Size = new System.Drawing.Size(100, 16);
            this.physicalChannelLabel.TabIndex = 0;
            this.physicalChannelLabel.Text = "Physical Channel:";
            // 
            // timingParametersGroupBox
            // 
            this.timingParametersGroupBox.Controls.Add(this.rateNumeric);
            this.timingParametersGroupBox.Controls.Add(this.rateLabel);
            this.timingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.timingParametersGroupBox.Location = new System.Drawing.Point(8, 288);
            this.timingParametersGroupBox.Name = "timingParametersGroupBox";
            this.timingParametersGroupBox.Size = new System.Drawing.Size(168, 72);
            this.timingParametersGroupBox.TabIndex = 3;
            this.timingParametersGroupBox.TabStop = false;
            this.timingParametersGroupBox.Text = "Timing Parameters";
            // 
            // rateNumeric
            // 
            this.rateNumeric.DecimalPlaces = 2;
            this.rateNumeric.Location = new System.Drawing.Point(16, 40);
            this.rateNumeric.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.rateNumeric.Name = "rateNumeric";
            this.rateNumeric.Size = new System.Drawing.Size(112, 20);
            this.rateNumeric.TabIndex = 1;
            this.rateNumeric.Value = new decimal(new int[] {
            100,
            0,
            0,
            65536});
            // 
            // rateLabel
            // 
            this.rateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rateLabel.Location = new System.Drawing.Point(16, 24);
            this.rateLabel.Name = "rateLabel";
            this.rateLabel.Size = new System.Drawing.Size(40, 16);
            this.rateLabel.TabIndex = 0;
            this.rateLabel.Text = "Rate:";
            // 
            // strainGageParametersGroupBox
            // 
            this.strainGageParametersGroupBox.Controls.Add(this.gageFactorNumeric);
            this.strainGageParametersGroupBox.Controls.Add(this.poissonRatioNumeric);
            this.strainGageParametersGroupBox.Controls.Add(this.nominalGageResistanceLabel);
            this.strainGageParametersGroupBox.Controls.Add(this.poissonRatioLabel);
            this.strainGageParametersGroupBox.Controls.Add(this.gageFactorLabel);
            this.strainGageParametersGroupBox.Controls.Add(this.gageResistanceNumeric);
            this.strainGageParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.strainGageParametersGroupBox.Location = new System.Drawing.Point(184, 8);
            this.strainGageParametersGroupBox.Name = "strainGageParametersGroupBox";
            this.strainGageParametersGroupBox.Size = new System.Drawing.Size(168, 168);
            this.strainGageParametersGroupBox.TabIndex = 4;
            this.strainGageParametersGroupBox.TabStop = false;
            this.strainGageParametersGroupBox.Text = "Strain Gauge Parameters";
            // 
            // gageFactorNumeric
            // 
            this.gageFactorNumeric.DecimalPlaces = 2;
            this.gageFactorNumeric.Location = new System.Drawing.Point(16, 40);
            this.gageFactorNumeric.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.gageFactorNumeric.Name = "gageFactorNumeric";
            this.gageFactorNumeric.Size = new System.Drawing.Size(112, 20);
            this.gageFactorNumeric.TabIndex = 1;
            this.gageFactorNumeric.Value = new decimal(new int[] {
            20,
            0,
            0,
            65536});
            // 
            // poissonRatioNumeric
            // 
            this.poissonRatioNumeric.DecimalPlaces = 2;
            this.poissonRatioNumeric.Location = new System.Drawing.Point(16, 88);
            this.poissonRatioNumeric.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.poissonRatioNumeric.Name = "poissonRatioNumeric";
            this.poissonRatioNumeric.Size = new System.Drawing.Size(112, 20);
            this.poissonRatioNumeric.TabIndex = 3;
            this.poissonRatioNumeric.Value = new decimal(new int[] {
            30,
            0,
            0,
            131072});
            // 
            // nominalGageResistanceLabel
            // 
            this.nominalGageResistanceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.nominalGageResistanceLabel.Location = new System.Drawing.Point(16, 120);
            this.nominalGageResistanceLabel.Name = "nominalGageResistanceLabel";
            this.nominalGageResistanceLabel.Size = new System.Drawing.Size(144, 16);
            this.nominalGageResistanceLabel.TabIndex = 4;
            this.nominalGageResistanceLabel.Text = "Nominal Gage Resistance:";
            // 
            // poissonRatioLabel
            // 
            this.poissonRatioLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.poissonRatioLabel.Location = new System.Drawing.Point(16, 72);
            this.poissonRatioLabel.Name = "poissonRatioLabel";
            this.poissonRatioLabel.Size = new System.Drawing.Size(100, 16);
            this.poissonRatioLabel.TabIndex = 2;
            this.poissonRatioLabel.Text = "Poisson Ratio:";
            // 
            // gageFactorLabel
            // 
            this.gageFactorLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.gageFactorLabel.Location = new System.Drawing.Point(16, 24);
            this.gageFactorLabel.Name = "gageFactorLabel";
            this.gageFactorLabel.Size = new System.Drawing.Size(100, 16);
            this.gageFactorLabel.TabIndex = 0;
            this.gageFactorLabel.Text = "Gage Factor:";
            // 
            // gageResistanceNumeric
            // 
            this.gageResistanceNumeric.DecimalPlaces = 2;
            this.gageResistanceNumeric.Location = new System.Drawing.Point(16, 136);
            this.gageResistanceNumeric.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.gageResistanceNumeric.Name = "gageResistanceNumeric";
            this.gageResistanceNumeric.Size = new System.Drawing.Size(112, 20);
            this.gageResistanceNumeric.TabIndex = 5;
            this.gageResistanceNumeric.Value = new decimal(new int[] {
            350,
            0,
            0,
            0});
            // 
            // bridgeParametersGroupBox
            // 
            this.bridgeParametersGroupBox.Controls.Add(this.leadWireResistanceNumeric);
            this.bridgeParametersGroupBox.Controls.Add(this.initialBridgeVoltageNumeric);
            this.bridgeParametersGroupBox.Controls.Add(this.excitationValueNumeric);
            this.bridgeParametersGroupBox.Controls.Add(this.excitationSourceComboBox);
            this.bridgeParametersGroupBox.Controls.Add(this.strainConfigurationComboBox);
            this.bridgeParametersGroupBox.Controls.Add(this.excitationSourceLabel);
            this.bridgeParametersGroupBox.Controls.Add(this.leadWireResistanceLabel);
            this.bridgeParametersGroupBox.Controls.Add(this.excitationValueLabel);
            this.bridgeParametersGroupBox.Controls.Add(this.initialBridgeVoltageLabel);
            this.bridgeParametersGroupBox.Controls.Add(this.strainConfigurationLabel);
            this.bridgeParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bridgeParametersGroupBox.Location = new System.Drawing.Point(184, 192);
            this.bridgeParametersGroupBox.Name = "bridgeParametersGroupBox";
            this.bridgeParametersGroupBox.Size = new System.Drawing.Size(272, 168);
            this.bridgeParametersGroupBox.TabIndex = 5;
            this.bridgeParametersGroupBox.TabStop = false;
            this.bridgeParametersGroupBox.Text = "Bridge Parameters";
            // 
            // leadWireResistanceNumeric
            // 
            this.leadWireResistanceNumeric.DecimalPlaces = 2;
            this.leadWireResistanceNumeric.Location = new System.Drawing.Point(144, 40);
            this.leadWireResistanceNumeric.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.leadWireResistanceNumeric.Name = "leadWireResistanceNumeric";
            this.leadWireResistanceNumeric.Size = new System.Drawing.Size(112, 20);
            this.leadWireResistanceNumeric.TabIndex = 7;
            // 
            // initialBridgeVoltageNumeric
            // 
            this.initialBridgeVoltageNumeric.DecimalPlaces = 2;
            this.initialBridgeVoltageNumeric.Location = new System.Drawing.Point(16, 88);
            this.initialBridgeVoltageNumeric.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.initialBridgeVoltageNumeric.Name = "initialBridgeVoltageNumeric";
            this.initialBridgeVoltageNumeric.Size = new System.Drawing.Size(112, 20);
            this.initialBridgeVoltageNumeric.TabIndex = 3;
            // 
            // excitationValueNumeric
            // 
            this.excitationValueNumeric.DecimalPlaces = 2;
            this.excitationValueNumeric.Location = new System.Drawing.Point(16, 136);
            this.excitationValueNumeric.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.excitationValueNumeric.Name = "excitationValueNumeric";
            this.excitationValueNumeric.Size = new System.Drawing.Size(112, 20);
            this.excitationValueNumeric.TabIndex = 5;
            this.excitationValueNumeric.Value = new decimal(new int[] {
            25,
            0,
            0,
            65536});
            // 
            // excitationSourceComboBox
            // 
            this.excitationSourceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.excitationSourceComboBox.Items.AddRange(new object[] {
            "Internal",
            "External",
            "None"});
            this.excitationSourceComboBox.Location = new System.Drawing.Point(144, 88);
            this.excitationSourceComboBox.Name = "excitationSourceComboBox";
            this.excitationSourceComboBox.Size = new System.Drawing.Size(112, 21);
            this.excitationSourceComboBox.TabIndex = 9;
            // 
            // strainConfigurationComboBox
            // 
            this.strainConfigurationComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.strainConfigurationComboBox.Items.AddRange(new object[] {
            "Full Bridge I",
            "Full Bridge II",
            "Full Bridge III",
            "Half Bridge I",
            "Half Bridge II",
            "Quarter Bridge I",
            "Quarter Bridge II"});
            this.strainConfigurationComboBox.Location = new System.Drawing.Point(16, 40);
            this.strainConfigurationComboBox.Name = "strainConfigurationComboBox";
            this.strainConfigurationComboBox.Size = new System.Drawing.Size(112, 21);
            this.strainConfigurationComboBox.TabIndex = 1;
            // 
            // excitationSourceLabel
            // 
            this.excitationSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.excitationSourceLabel.Location = new System.Drawing.Point(144, 72);
            this.excitationSourceLabel.Name = "excitationSourceLabel";
            this.excitationSourceLabel.Size = new System.Drawing.Size(100, 16);
            this.excitationSourceLabel.TabIndex = 8;
            this.excitationSourceLabel.Text = "Excitation Source:";
            // 
            // leadWireResistanceLabel
            // 
            this.leadWireResistanceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.leadWireResistanceLabel.Location = new System.Drawing.Point(144, 24);
            this.leadWireResistanceLabel.Name = "leadWireResistanceLabel";
            this.leadWireResistanceLabel.Size = new System.Drawing.Size(120, 16);
            this.leadWireResistanceLabel.TabIndex = 6;
            this.leadWireResistanceLabel.Text = "Lead Wire Resistance:";
            // 
            // excitationValueLabel
            // 
            this.excitationValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.excitationValueLabel.Location = new System.Drawing.Point(16, 120);
            this.excitationValueLabel.Name = "excitationValueLabel";
            this.excitationValueLabel.Size = new System.Drawing.Size(104, 16);
            this.excitationValueLabel.TabIndex = 4;
            this.excitationValueLabel.Text = "Excitation Value:";
            // 
            // initialBridgeVoltageLabel
            // 
            this.initialBridgeVoltageLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.initialBridgeVoltageLabel.Location = new System.Drawing.Point(16, 72);
            this.initialBridgeVoltageLabel.Name = "initialBridgeVoltageLabel";
            this.initialBridgeVoltageLabel.Size = new System.Drawing.Size(120, 16);
            this.initialBridgeVoltageLabel.TabIndex = 2;
            this.initialBridgeVoltageLabel.Text = "Initial Bridge Voltage:";
            // 
            // strainConfigurationLabel
            // 
            this.strainConfigurationLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.strainConfigurationLabel.Location = new System.Drawing.Point(16, 24);
            this.strainConfigurationLabel.Name = "strainConfigurationLabel";
            this.strainConfigurationLabel.Size = new System.Drawing.Size(112, 16);
            this.strainConfigurationLabel.TabIndex = 0;
            this.strainConfigurationLabel.Text = "Strain Configuration:";
            // 
            // startButton
            // 
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.startButton.Location = new System.Drawing.Point(372, 120);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(64, 24);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.stopButton.Location = new System.Drawing.Point(372, 152);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(64, 24);
            this.stopButton.TabIndex = 1;
            this.stopButton.Text = "Stop";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // readTimer
            // 
            this.readTimer.Tick += new System.EventHandler(this.readTimer_Tick);
            // 
            // bridgeNull
            // 
            this.bridgeNull.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bridgeNull.Location = new System.Drawing.Point(360, 24);
            this.bridgeNull.Name = "bridgeNull";
            this.bridgeNull.Size = new System.Drawing.Size(104, 40);
            this.bridgeNull.TabIndex = 6;
            this.bridgeNull.Text = "Perform Bridge Null?";
            // 
            // acquisitionResultGroupBox
            // 
            this.acquisitionResultGroupBox.Controls.Add(this.acquisitionDataGrid);
            this.acquisitionResultGroupBox.Location = new System.Drawing.Point(466, 8);
            this.acquisitionResultGroupBox.Name = "acquisitionResultGroupBox";
            this.acquisitionResultGroupBox.Size = new System.Drawing.Size(166, 352);
            this.acquisitionResultGroupBox.TabIndex = 9;
            this.acquisitionResultGroupBox.TabStop = false;
            this.acquisitionResultGroupBox.Text = "Acquisition Results";
            // 
            // acquisitionDataGrid
            // 
            this.acquisitionDataGrid.DataMember = "";
            this.acquisitionDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.acquisitionDataGrid.Location = new System.Drawing.Point(6, 24);
            this.acquisitionDataGrid.Name = "acquisitionDataGrid";
            this.acquisitionDataGrid.ReadOnly = true;
            this.acquisitionDataGrid.Size = new System.Drawing.Size(154, 322);
            this.acquisitionDataGrid.TabIndex = 1;
            this.acquisitionDataGrid.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(636, 400);
            this.Controls.Add(this.acquisitionResultGroupBox);
            this.Controls.Add(this.bridgeNull);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.bridgeParametersGroupBox);
            this.Controls.Add(this.strainGageParametersGroupBox);
            this.Controls.Add(this.timingParametersGroupBox);
            this.Controls.Add(this.channelParametersGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Acquire Strain Samples";
            this.channelParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.minimumValueNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maximumValueNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cutoffFrequencyNumeric)).EndInit();
            this.timingParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rateNumeric)).EndInit();
            this.strainGageParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gageFactorNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.poissonRatioNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gageResistanceNumeric)).EndInit();
            this.bridgeParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.leadWireResistanceNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.initialBridgeVoltageNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.excitationValueNumeric)).EndInit();
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
                
            try
            {

                switch (strainConfigurationComboBox.SelectedItem.ToString())
                {
                    case "Full Bridge I":
                        strainGageConfiguration=AIStrainGageConfiguration.FullBridgeI;
                        break;
                    case "Full Bridge II":
                        strainGageConfiguration=AIStrainGageConfiguration.FullBridgeII;
                        break;
                    case "Full Bridge III":
                        strainGageConfiguration=AIStrainGageConfiguration.FullBridgeIII;
                        break;
                    case "Half Bridge I":
                        strainGageConfiguration=AIStrainGageConfiguration.HalfBridgeI;
                        break;
                    case "Half Bridge II":
                        strainGageConfiguration=AIStrainGageConfiguration.HalfBridgeII;
                        break;
                    case "Quarter Bridge I":
                        strainGageConfiguration=AIStrainGageConfiguration.QuarterBridgeI;
                        break;
                    case "Quarter Bridge II":
                        strainGageConfiguration=AIStrainGageConfiguration.QuarterBridgeII;
                        break;
                }

                switch (excitationSourceComboBox.SelectedItem.ToString())
                {
                    case "Internal":
                        excitationSource=AIExcitationSource.Internal;
                        break;
                    case "External":
                        excitationSource=AIExcitationSource.External;
                        break;
                    case "None":
                        excitationSource=AIExcitationSource.None;
                        break;
                }

                myTask= new Task();

                AIChannel myAIChannel = myTask.AIChannels.CreateStrainGageChannel(physicalChannelComboBox.Text,
                    "",Convert.ToDouble(minimumValueNumeric.Value), Convert.ToDouble(maximumValueNumeric.Value),
                    strainGageConfiguration ,excitationSource,Convert.ToDouble(excitationValueNumeric.Value),
                    Convert.ToDouble(gageFactorNumeric.Value), Convert.ToDouble(initialBridgeVoltageNumeric.Value),
                    Convert.ToDouble(gageResistanceNumeric.Value),Convert.ToDouble(poissonRatioNumeric.Value),
                    Convert.ToDouble(leadWireResistanceNumeric.Value),AIStrainUnits.Strain);

                myTask.Timing.ConfigureSampleClock("", Convert.ToDouble(rateNumeric.Value),
                    SampleClockActiveEdge.Rising, SampleQuantityMode.ContinuousSamples, 1000);

                myTask.Control(TaskAction.Verify);

                switch(filterEnabledComboBox.SelectedItem.ToString())
                {
                    case "Yes":
                        myAIChannel.LowpassEnable = true;
                        myAIChannel.LowpassCutoffFrequency = Convert.ToDouble(cutoffFrequencyNumeric.Value);
                        break;
                    case "No":
                        myAIChannel.LowpassEnable = false;
                        break;
                    case "Default":
                        break;
                }

                if (bridgeNull.Checked)
                {
                    myAIChannel.PerformBridgeOffsetNullingCalibration();
                }

                myAIChannelReader = new AnalogSingleChannelReader(myTask.Stream);

                myTask.Start();
                startButton.Enabled = false;
                stopButton.Enabled = true;
                readTimer.Enabled = true;

            }
            catch (DaqException exception)
            {
                readTimer.Enabled = false;
                MessageBox.Show(exception.Message);
                myTask.Dispose();
                startButton.Enabled=true;
                stopButton.Enabled=false;
            }


        }

        private void stopButton_Click(object sender, System.EventArgs e)
        {
            readTimer.Enabled = false;
            while (readTimer.Enabled == true)
                ;
            myTask.Stop();
            myTask.Dispose();
            startButton.Enabled = true;
            stopButton.Enabled = false;

        }

        private void readTimer_Tick(object sender, System.EventArgs e)
        {
            try
            {
                InitializeDataTable(ref dataTable);
                acquisitionDataGrid.DataSource = dataTable;

                AnalogWaveform<double> data = myAIChannelReader.ReadWaveform(-1);

                dataToDataTable(data, ref dataTable);
            }
            catch (DaqException exception)
            {
                readTimer.Enabled = false;
                MessageBox.Show(exception.Message);
                myTask.Dispose();
                startButton.Enabled = true;
                stopButton.Enabled = false;
            }
        }

        private void dataToDataTable(AnalogWaveform<double> waveform, ref DataTable dataTable)
        {
            for (int sample = 0; sample < waveform.Samples.Count; ++sample)
            {
                if (sample == 10)
                    break;

                dataTable.Rows[sample][0] = waveform.Samples[sample].Value;
            }
        }

        private void InitializeDataTable(ref DataTable data)
        {
            data.Rows.Clear();
            data.Columns.Clear();
            DataColumn dataColumn = new DataColumn(myTask.AIChannels[0].PhysicalName, typeof(double));
            int numOfRows = 10;

            data.Columns.Add(dataColumn);

            for (int currentIndex = 0; currentIndex < numOfRows; ++currentIndex)
            {
                object[] row = new object[1];
                data.Rows.Add(row);
            }
        }

        private void filterEnabledComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            switch (filterEnabledComboBox.SelectedIndex)
            {
                case 1: // Filter enabled 
                    cutoffFrequencyNumeric.Enabled = true;
                    break;
                default: // Filter not enabled
                    cutoffFrequencyNumeric.Enabled = false;
                    break;
            }
        }
    }
}
