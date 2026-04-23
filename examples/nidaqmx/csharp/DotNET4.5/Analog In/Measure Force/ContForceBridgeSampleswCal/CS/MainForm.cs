/******************************************************************************
*
* Example program:
*   ContForceBridgeSampleswCal
*
* Category:
*   AI
*
* Description:
*   This example performs Wheatstone Bridge measurements with offset nulling if
*   desired.
*
* Instructions for running:
*   1.  Enter the list of physical channels, and set the attributes
*       of the bridge configuration connected to all the channels.
*       The 'Maximum Value' and 'Minimum Value' inputs specify the
*       range, in Custom Scale units, that you expect of your
*       measurements.
*   2.  Make sure your Bridge sensor is in its relaxed state.
*   3.  You may check the 'Perform Bridge Null?' option to automatically
*       null out your offset by performing a hardware nulling
*       operation (if supported by the hardware) followed by a
*       software nulling operation. (NOTE: The software nulling
*       operation will cause a loss in dynamic range while a hardware
*       nulling operation will not cause any loss in the dynamic
*       range).
*   4.  Specify Sensor Scaling Parameters. You can choose a Linear
*       Scale or Map Ranges Scale. The channel Maximum and Minimum
*       values are specified in terms of the scaled units.
*   5.  Run the example and do not disturb your bridge sensor until
*       data starts being plotted.
*
* Steps:
*   1.  Create custom scale.
*   2.  Create a new Task. Create a AIChannel by using the
*           CreateVoltageChannelWithExcitation method.
*   3.  Set the rate for the sample clock by using the
*           Timing.ConfigureSampleClock method. Additionally, define the sample mode
*           to be continuous.
*   4.  If nulling is desired, call the DAQmx Perform Bridge Offset
*           Nulling Calibration function to perform both hardware nulling
*           (if supported) and software nulling.
*   5.  Call AnalogMultiChannelReader.BeginReadWaveform to install a callback
*       and begin the asynchronous read operation.
*   6.  Inside the callback, call AnalogMultiChannelReader.EndReadWaveform to
*       retrieve the data from the read operation.  
*   7.  Call AnalogMultiChannelReader.BeginMemoryOptimizedReadWaveform
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
*   Make sure your signal input terminals match the physical channel text box. 
*   For more information on the input and output terminals for your device, open
*   the NI-DAQmx Help, and refer to the NI-DAQmx Device Terminals and Device
*   Considerations books in the table of contents.
*
* Microsoft Windows Vista User Account Control
*   Running certain applications on Microsoft Windows Vista requires
*   administrator privileges, because the application name contains keywords
*   such as setup, update, or install. To avoid this problem, you must add an
*   additional manifest to the application that specifies the privileges
*   required to run the application. Some Measurement Studio NI-DAQmx examples
*   for Visual Studio include these keywords. Therefore, all examples for Visual
*   Studio are shipped with an additional manifest file that you must embed in
*   the example executable. The manifest file is named
*   [ExampleName].exe.manifest, where [ExampleName] is the NI-provided example
*   name. For information on how to embed the manifest file, refer to http://msdn2.microsoft.com/en-us/library/bb756929.aspx.
*   
*   Note: The manifest file is not provided with examples for Visual Studio .NET
*   2003.
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

namespace NationalInstruments.Examples.ContForceBridgeSampleswCal
{
    /// <summary>
    /// Summary description for MainForm.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.GroupBox wheatstoneBridgeParametersGroupBox;
        private System.Windows.Forms.ComboBox bridgeConfigurationComboBox;
        private System.Windows.Forms.Label bridgeConfigurationLabel;
        private System.Windows.Forms.Label excitationSourceLabel;
        private System.Windows.Forms.ComboBox excitationSourceComboBox;
        private System.Windows.Forms.Label excitationValueLabel;
        private System.Windows.Forms.GroupBox acquisitionResultGroupBox;
        private System.Windows.Forms.DataGrid acquisitionDataGrid;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.GroupBox channelParametersGroupBox;
        private System.Windows.Forms.ComboBox physicalChannelComboBox;
        private System.Windows.Forms.NumericUpDown minimumValueNumericUpDown;
        private System.Windows.Forms.NumericUpDown maximumValueNumericUpDown;
        private System.Windows.Forms.Label minimumValueLabel;
        private System.Windows.Forms.Label maximumValueLabel;
        private System.Windows.Forms.Label physicalChannelLabel;
        private System.Windows.Forms.GroupBox timingParametersGroupBox;
        private System.Windows.Forms.NumericUpDown rateNumericUpDown;
        private System.Windows.Forms.Label samplesPerChannelLabel;
        private System.Windows.Forms.Label rateLabel;
        private System.Windows.Forms.NumericUpDown samplesPerChannelNumericUpDown;
        private DataTable dataTable;
        private DataColumn[] dataColumn;
        private Task myTask;
        private Task runningTask;
        private string customScaleName = "Acq Wheatstone Bridge Samples Scale";
        private Scale customScale;
        private AnalogMultiChannelReader reader;
        private AsyncCallback callback;
        private AnalogWaveform<double>[] waveform;

        private ComboBox unitsComboBox;
        private Label unitsLabel;
        private Label nominalGageResistanceLabel;
        private NumericUpDown excitationValueNumericUpDown;
        private NumericUpDown nomGageResNumericUpDown;
        private CheckBox shuntCalibrationCheckBox;
        private CheckBox bridgeNullCheckBox;
        private GroupBox shuntCalibrationParametersGroupBox;
        private Label shuntResistanceLabel;
        private NumericUpDown shuntResistanceNumericUpDown;
        private ComboBox shuntElementLocationComboBox;
        private Label shuntElementLocationLabel;
        private GroupBox sensorScalingParametersGroupBox;
        private TabControl sensorScalingTabControl;
        private TabPage linearTabPage;
        private TabPage tableTabPage;
        private TabPage polynomialTabPage;
        private Label secondElectricalValueLabel;
        private Label firstElectricalValueLabel;
        private NumericUpDown secondElectricalValueNumericUpDown;
        private NumericUpDown firstElectricalValueNumericUpDown;
        private Label physicalUnitsLabel;
        private Label electricalUnitsLabel;
        private ComboBox electricalUnitsComboBox;
        private ComboBox physicalUnitsComboBox;
        private NumericUpDown secondPhysicalValueNumericUpDown;
        private NumericUpDown firstPhysicalValueNumericUpDown;
        private Label secondPhysicalValueLabel;
        private Label firstPhysicalValueLabel;
        private DataGridView tableDataGridView;
        private DataGridViewTextBoxColumn electricalValuesColumn;
        private DataGridViewTextBoxColumn physicalValuesColumn;
        private GroupBox coefficientsDirectionGroupBox;
        private RadioButton physToElecRadioButton;
        private RadioButton electToPhysRadioButton;
        private GroupBox polynomialRangeGroupBox;
        private Label minimumLabel;
        private NumericUpDown maximumNumericUpDown;
        private NumericUpDown minimumNumericUpDown;
        private Label maximumLabel;
        private Label label1;
        private DataGridView polynomialDataGrid;
        private DataGridViewTextBoxColumn coefficientsColumn;
        private IContainer components;

        public MainForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            dataTable = new DataTable();
            bridgeConfigurationComboBox.SelectedIndex = 0;
            excitationSourceComboBox.SelectedIndex = 0;
            shuntElementLocationComboBox.SelectedIndex = 0;
            electricalUnitsComboBox.SelectedIndex = 0;
            physicalUnitsComboBox.SelectedIndex = 0;

            physicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AI, PhysicalChannelAccess.External));

            if (physicalChannelComboBox.Items.Count > 0)
            {
                physicalChannelComboBox.SelectedIndex = 0;
                startButton.Enabled = true;
            }

            electToPhysRadioButton.Select();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.wheatstoneBridgeParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.shuntCalibrationCheckBox = new System.Windows.Forms.CheckBox();
            this.bridgeNullCheckBox = new System.Windows.Forms.CheckBox();
            this.nomGageResNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.excitationValueNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.bridgeConfigurationComboBox = new System.Windows.Forms.ComboBox();
            this.bridgeConfigurationLabel = new System.Windows.Forms.Label();
            this.excitationSourceLabel = new System.Windows.Forms.Label();
            this.excitationSourceComboBox = new System.Windows.Forms.ComboBox();
            this.nominalGageResistanceLabel = new System.Windows.Forms.Label();
            this.excitationValueLabel = new System.Windows.Forms.Label();
            this.acquisitionResultGroupBox = new System.Windows.Forms.GroupBox();
            this.acquisitionDataGrid = new System.Windows.Forms.DataGrid();
            this.stopButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.channelParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.unitsComboBox = new System.Windows.Forms.ComboBox();
            this.unitsLabel = new System.Windows.Forms.Label();
            this.physicalChannelComboBox = new System.Windows.Forms.ComboBox();
            this.minimumValueNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.maximumValueNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.minimumValueLabel = new System.Windows.Forms.Label();
            this.maximumValueLabel = new System.Windows.Forms.Label();
            this.physicalChannelLabel = new System.Windows.Forms.Label();
            this.timingParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.rateNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.samplesPerChannelLabel = new System.Windows.Forms.Label();
            this.rateLabel = new System.Windows.Forms.Label();
            this.samplesPerChannelNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.shuntCalibrationParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.shuntElementLocationComboBox = new System.Windows.Forms.ComboBox();
            this.shuntElementLocationLabel = new System.Windows.Forms.Label();
            this.shuntResistanceNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.shuntResistanceLabel = new System.Windows.Forms.Label();
            this.sensorScalingParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.physicalUnitsComboBox = new System.Windows.Forms.ComboBox();
            this.electricalUnitsComboBox = new System.Windows.Forms.ComboBox();
            this.physicalUnitsLabel = new System.Windows.Forms.Label();
            this.electricalUnitsLabel = new System.Windows.Forms.Label();
            this.sensorScalingTabControl = new System.Windows.Forms.TabControl();
            this.linearTabPage = new System.Windows.Forms.TabPage();
            this.secondPhysicalValueNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.firstPhysicalValueNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.secondPhysicalValueLabel = new System.Windows.Forms.Label();
            this.firstPhysicalValueLabel = new System.Windows.Forms.Label();
            this.secondElectricalValueNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.firstElectricalValueNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.secondElectricalValueLabel = new System.Windows.Forms.Label();
            this.firstElectricalValueLabel = new System.Windows.Forms.Label();
            this.tableTabPage = new System.Windows.Forms.TabPage();
            this.tableDataGridView = new System.Windows.Forms.DataGridView();
            this.electricalValuesColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.physicalValuesColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.polynomialTabPage = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.polynomialDataGrid = new System.Windows.Forms.DataGridView();
            this.coefficientsColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.polynomialRangeGroupBox = new System.Windows.Forms.GroupBox();
            this.maximumNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.minimumNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.maximumLabel = new System.Windows.Forms.Label();
            this.minimumLabel = new System.Windows.Forms.Label();
            this.coefficientsDirectionGroupBox = new System.Windows.Forms.GroupBox();
            this.physToElecRadioButton = new System.Windows.Forms.RadioButton();
            this.electToPhysRadioButton = new System.Windows.Forms.RadioButton();
            this.wheatstoneBridgeParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nomGageResNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.excitationValueNumericUpDown)).BeginInit();
            this.acquisitionResultGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.acquisitionDataGrid)).BeginInit();
            this.channelParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.minimumValueNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maximumValueNumericUpDown)).BeginInit();
            this.timingParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rateNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerChannelNumericUpDown)).BeginInit();
            this.shuntCalibrationParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shuntResistanceNumericUpDown)).BeginInit();
            this.sensorScalingParametersGroupBox.SuspendLayout();
            this.sensorScalingTabControl.SuspendLayout();
            this.linearTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.secondPhysicalValueNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.firstPhysicalValueNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.secondElectricalValueNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.firstElectricalValueNumericUpDown)).BeginInit();
            this.tableTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tableDataGridView)).BeginInit();
            this.polynomialTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.polynomialDataGrid)).BeginInit();
            this.polynomialRangeGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maximumNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minimumNumericUpDown)).BeginInit();
            this.coefficientsDirectionGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // wheatstoneBridgeParametersGroupBox
            // 
            this.wheatstoneBridgeParametersGroupBox.Controls.Add(this.shuntCalibrationCheckBox);
            this.wheatstoneBridgeParametersGroupBox.Controls.Add(this.bridgeNullCheckBox);
            this.wheatstoneBridgeParametersGroupBox.Controls.Add(this.nomGageResNumericUpDown);
            this.wheatstoneBridgeParametersGroupBox.Controls.Add(this.excitationValueNumericUpDown);
            this.wheatstoneBridgeParametersGroupBox.Controls.Add(this.bridgeConfigurationComboBox);
            this.wheatstoneBridgeParametersGroupBox.Controls.Add(this.bridgeConfigurationLabel);
            this.wheatstoneBridgeParametersGroupBox.Controls.Add(this.excitationSourceLabel);
            this.wheatstoneBridgeParametersGroupBox.Controls.Add(this.excitationSourceComboBox);
            this.wheatstoneBridgeParametersGroupBox.Controls.Add(this.nominalGageResistanceLabel);
            this.wheatstoneBridgeParametersGroupBox.Controls.Add(this.excitationValueLabel);
            this.wheatstoneBridgeParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.wheatstoneBridgeParametersGroupBox.Location = new System.Drawing.Point(12, 226);
            this.wheatstoneBridgeParametersGroupBox.Name = "wheatstoneBridgeParametersGroupBox";
            this.wheatstoneBridgeParametersGroupBox.Size = new System.Drawing.Size(280, 156);
            this.wheatstoneBridgeParametersGroupBox.TabIndex = 2;
            this.wheatstoneBridgeParametersGroupBox.TabStop = false;
            this.wheatstoneBridgeParametersGroupBox.Text = "Wheatstone Bridge Parameters";
            // 
            // shuntCalibrationCheckBox
            // 
            this.shuntCalibrationCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.shuntCalibrationCheckBox.Location = new System.Drawing.Point(135, 128);
            this.shuntCalibrationCheckBox.Name = "shuntCalibrationCheckBox";
            this.shuntCalibrationCheckBox.Size = new System.Drawing.Size(128, 24);
            this.shuntCalibrationCheckBox.TabIndex = 8;
            this.shuntCalibrationCheckBox.Text = "Do Shunt Calibration?";
            // 
            // bridgeNullCheckBox
            // 
            this.bridgeNullCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bridgeNullCheckBox.Location = new System.Drawing.Point(18, 128);
            this.bridgeNullCheckBox.Name = "bridgeNullCheckBox";
            this.bridgeNullCheckBox.Size = new System.Drawing.Size(128, 24);
            this.bridgeNullCheckBox.TabIndex = 7;
            this.bridgeNullCheckBox.Text = "Do Bridge Null?";
            // 
            // nomGageResNumericUpDown
            // 
            this.nomGageResNumericUpDown.DecimalPlaces = 2;
            this.nomGageResNumericUpDown.Location = new System.Drawing.Point(136, 99);
            this.nomGageResNumericUpDown.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nomGageResNumericUpDown.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.nomGageResNumericUpDown.Name = "nomGageResNumericUpDown";
            this.nomGageResNumericUpDown.Size = new System.Drawing.Size(137, 20);
            this.nomGageResNumericUpDown.TabIndex = 6;
            this.nomGageResNumericUpDown.Value = new decimal(new int[] {
            350,
            0,
            0,
            0});
            // 
            // excitationValueNumericUpDown
            // 
            this.excitationValueNumericUpDown.DecimalPlaces = 2;
            this.excitationValueNumericUpDown.Location = new System.Drawing.Point(136, 73);
            this.excitationValueNumericUpDown.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.excitationValueNumericUpDown.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.excitationValueNumericUpDown.Name = "excitationValueNumericUpDown";
            this.excitationValueNumericUpDown.Size = new System.Drawing.Size(137, 20);
            this.excitationValueNumericUpDown.TabIndex = 5;
            this.excitationValueNumericUpDown.Value = new decimal(new int[] {
            25,
            0,
            0,
            65536});
            // 
            // bridgeConfigurationComboBox
            // 
            this.bridgeConfigurationComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.bridgeConfigurationComboBox.Items.AddRange(new object[] {
            "Full Bridge",
            "Half Bridge",
            "Quarter Bridge",
            "No Bridge"});
            this.bridgeConfigurationComboBox.Location = new System.Drawing.Point(136, 19);
            this.bridgeConfigurationComboBox.Name = "bridgeConfigurationComboBox";
            this.bridgeConfigurationComboBox.Size = new System.Drawing.Size(137, 21);
            this.bridgeConfigurationComboBox.TabIndex = 1;
            // 
            // bridgeConfigurationLabel
            // 
            this.bridgeConfigurationLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bridgeConfigurationLabel.Location = new System.Drawing.Point(16, 24);
            this.bridgeConfigurationLabel.Name = "bridgeConfigurationLabel";
            this.bridgeConfigurationLabel.Size = new System.Drawing.Size(112, 16);
            this.bridgeConfigurationLabel.TabIndex = 0;
            this.bridgeConfigurationLabel.Text = "Bridge Configuration:";
            // 
            // excitationSourceLabel
            // 
            this.excitationSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.excitationSourceLabel.Location = new System.Drawing.Point(16, 49);
            this.excitationSourceLabel.Name = "excitationSourceLabel";
            this.excitationSourceLabel.Size = new System.Drawing.Size(112, 16);
            this.excitationSourceLabel.TabIndex = 2;
            this.excitationSourceLabel.Text = "Excitation Source:";
            // 
            // excitationSourceComboBox
            // 
            this.excitationSourceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.excitationSourceComboBox.Items.AddRange(new object[] {
            "Internal",
            "External",
            "None"});
            this.excitationSourceComboBox.Location = new System.Drawing.Point(136, 46);
            this.excitationSourceComboBox.Name = "excitationSourceComboBox";
            this.excitationSourceComboBox.Size = new System.Drawing.Size(137, 21);
            this.excitationSourceComboBox.TabIndex = 3;
            // 
            // nominalGageResistanceLabel
            // 
            this.nominalGageResistanceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.nominalGageResistanceLabel.Location = new System.Drawing.Point(16, 95);
            this.nominalGageResistanceLabel.Name = "nominalGageResistanceLabel";
            this.nominalGageResistanceLabel.Size = new System.Drawing.Size(112, 30);
            this.nominalGageResistanceLabel.TabIndex = 4;
            this.nominalGageResistanceLabel.Text = "Nominal Gage Resistance:";
            // 
            // excitationValueLabel
            // 
            this.excitationValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.excitationValueLabel.Location = new System.Drawing.Point(16, 76);
            this.excitationValueLabel.Name = "excitationValueLabel";
            this.excitationValueLabel.Size = new System.Drawing.Size(112, 16);
            this.excitationValueLabel.TabIndex = 4;
            this.excitationValueLabel.Text = "Excitation Value (V):";
            // 
            // acquisitionResultGroupBox
            // 
            this.acquisitionResultGroupBox.Controls.Add(this.acquisitionDataGrid);
            this.acquisitionResultGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.acquisitionResultGroupBox.Location = new System.Drawing.Point(298, 8);
            this.acquisitionResultGroupBox.Name = "acquisitionResultGroupBox";
            this.acquisitionResultGroupBox.Size = new System.Drawing.Size(478, 212);
            this.acquisitionResultGroupBox.TabIndex = 7;
            this.acquisitionResultGroupBox.TabStop = false;
            this.acquisitionResultGroupBox.Text = "Acquisition Results";
            // 
            // acquisitionDataGrid
            // 
            this.acquisitionDataGrid.DataMember = "";
            this.acquisitionDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.acquisitionDataGrid.Location = new System.Drawing.Point(8, 24);
            this.acquisitionDataGrid.Name = "acquisitionDataGrid";
            this.acquisitionDataGrid.ReadOnly = true;
            this.acquisitionDataGrid.Size = new System.Drawing.Size(464, 182);
            this.acquisitionDataGrid.TabIndex = 1;
            this.acquisitionDataGrid.TabStop = false;
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.stopButton.Location = new System.Drawing.Point(156, 478);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(88, 24);
            this.stopButton.TabIndex = 6;
            this.stopButton.Text = "Stop";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // startButton
            // 
            this.startButton.Enabled = false;
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.startButton.Location = new System.Drawing.Point(60, 478);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(88, 24);
            this.startButton.TabIndex = 5;
            this.startButton.Text = "Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // channelParametersGroupBox
            // 
            this.channelParametersGroupBox.Controls.Add(this.unitsComboBox);
            this.channelParametersGroupBox.Controls.Add(this.unitsLabel);
            this.channelParametersGroupBox.Controls.Add(this.physicalChannelComboBox);
            this.channelParametersGroupBox.Controls.Add(this.minimumValueNumericUpDown);
            this.channelParametersGroupBox.Controls.Add(this.maximumValueNumericUpDown);
            this.channelParametersGroupBox.Controls.Add(this.minimumValueLabel);
            this.channelParametersGroupBox.Controls.Add(this.maximumValueLabel);
            this.channelParametersGroupBox.Controls.Add(this.physicalChannelLabel);
            this.channelParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.channelParametersGroupBox.Location = new System.Drawing.Point(12, 8);
            this.channelParametersGroupBox.Name = "channelParametersGroupBox";
            this.channelParametersGroupBox.Size = new System.Drawing.Size(280, 128);
            this.channelParametersGroupBox.TabIndex = 0;
            this.channelParametersGroupBox.TabStop = false;
            this.channelParametersGroupBox.Text = "Channel Parameters";
            // 
            // unitsComboBox
            // 
            this.unitsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.unitsComboBox.FormattingEnabled = true;
            this.unitsComboBox.Items.AddRange(new object[] {
            "Pounds",
            "kgf",
            "Newtons"});
            this.unitsComboBox.Location = new System.Drawing.Point(136, 98);
            this.unitsComboBox.Name = "unitsComboBox";
            this.unitsComboBox.Size = new System.Drawing.Size(137, 21);
            this.unitsComboBox.TabIndex = 7;
            // 
            // unitsLabel
            // 
            this.unitsLabel.AutoSize = true;
            this.unitsLabel.Location = new System.Drawing.Point(16, 101);
            this.unitsLabel.Name = "unitsLabel";
            this.unitsLabel.Size = new System.Drawing.Size(34, 13);
            this.unitsLabel.TabIndex = 6;
            this.unitsLabel.Text = "Units:";
            // 
            // physicalChannelComboBox
            // 
            this.physicalChannelComboBox.Location = new System.Drawing.Point(136, 19);
            this.physicalChannelComboBox.Name = "physicalChannelComboBox";
            this.physicalChannelComboBox.Size = new System.Drawing.Size(137, 21);
            this.physicalChannelComboBox.TabIndex = 1;
            // 
            // minimumValueNumericUpDown
            // 
            this.minimumValueNumericUpDown.DecimalPlaces = 3;
            this.minimumValueNumericUpDown.Location = new System.Drawing.Point(136, 72);
            this.minimumValueNumericUpDown.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.minimumValueNumericUpDown.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.minimumValueNumericUpDown.Name = "minimumValueNumericUpDown";
            this.minimumValueNumericUpDown.Size = new System.Drawing.Size(137, 20);
            this.minimumValueNumericUpDown.TabIndex = 5;
            this.minimumValueNumericUpDown.Value = new decimal(new int[] {
            25,
            0,
            0,
            -2147287040});
            // 
            // maximumValueNumericUpDown
            // 
            this.maximumValueNumericUpDown.DecimalPlaces = 3;
            this.maximumValueNumericUpDown.Location = new System.Drawing.Point(136, 46);
            this.maximumValueNumericUpDown.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.maximumValueNumericUpDown.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.maximumValueNumericUpDown.Name = "maximumValueNumericUpDown";
            this.maximumValueNumericUpDown.Size = new System.Drawing.Size(137, 20);
            this.maximumValueNumericUpDown.TabIndex = 3;
            this.maximumValueNumericUpDown.Value = new decimal(new int[] {
            25,
            0,
            0,
            196608});
            // 
            // minimumValueLabel
            // 
            this.minimumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.minimumValueLabel.Location = new System.Drawing.Point(16, 74);
            this.minimumValueLabel.Name = "minimumValueLabel";
            this.minimumValueLabel.Size = new System.Drawing.Size(100, 16);
            this.minimumValueLabel.TabIndex = 4;
            this.minimumValueLabel.Text = "Minimum Value:";
            // 
            // maximumValueLabel
            // 
            this.maximumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.maximumValueLabel.Location = new System.Drawing.Point(16, 48);
            this.maximumValueLabel.Name = "maximumValueLabel";
            this.maximumValueLabel.Size = new System.Drawing.Size(100, 16);
            this.maximumValueLabel.TabIndex = 2;
            this.maximumValueLabel.Text = "Maximum Value:";
            // 
            // physicalChannelLabel
            // 
            this.physicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.physicalChannelLabel.Location = new System.Drawing.Point(16, 22);
            this.physicalChannelLabel.Name = "physicalChannelLabel";
            this.physicalChannelLabel.Size = new System.Drawing.Size(100, 16);
            this.physicalChannelLabel.TabIndex = 0;
            this.physicalChannelLabel.Text = "Physical Channel:";
            // 
            // timingParametersGroupBox
            // 
            this.timingParametersGroupBox.Controls.Add(this.rateNumericUpDown);
            this.timingParametersGroupBox.Controls.Add(this.samplesPerChannelLabel);
            this.timingParametersGroupBox.Controls.Add(this.rateLabel);
            this.timingParametersGroupBox.Controls.Add(this.samplesPerChannelNumericUpDown);
            this.timingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.timingParametersGroupBox.Location = new System.Drawing.Point(12, 144);
            this.timingParametersGroupBox.Name = "timingParametersGroupBox";
            this.timingParametersGroupBox.Size = new System.Drawing.Size(280, 76);
            this.timingParametersGroupBox.TabIndex = 1;
            this.timingParametersGroupBox.TabStop = false;
            this.timingParametersGroupBox.Text = "Timing Parameters";
            // 
            // rateNumericUpDown
            // 
            this.rateNumericUpDown.DecimalPlaces = 2;
            this.rateNumericUpDown.Location = new System.Drawing.Point(136, 43);
            this.rateNumericUpDown.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.rateNumericUpDown.Name = "rateNumericUpDown";
            this.rateNumericUpDown.Size = new System.Drawing.Size(137, 20);
            this.rateNumericUpDown.TabIndex = 3;
            this.rateNumericUpDown.Value = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            // 
            // samplesPerChannelLabel
            // 
            this.samplesPerChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.samplesPerChannelLabel.Location = new System.Drawing.Point(16, 19);
            this.samplesPerChannelLabel.Name = "samplesPerChannelLabel";
            this.samplesPerChannelLabel.Size = new System.Drawing.Size(120, 16);
            this.samplesPerChannelLabel.TabIndex = 0;
            this.samplesPerChannelLabel.Text = "Samples Per Channel:";
            // 
            // rateLabel
            // 
            this.rateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rateLabel.Location = new System.Drawing.Point(16, 45);
            this.rateLabel.Name = "rateLabel";
            this.rateLabel.Size = new System.Drawing.Size(96, 16);
            this.rateLabel.TabIndex = 2;
            this.rateLabel.Text = "Rate (Hz):";
            // 
            // samplesPerChannelNumericUpDown
            // 
            this.samplesPerChannelNumericUpDown.Location = new System.Drawing.Point(136, 17);
            this.samplesPerChannelNumericUpDown.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.samplesPerChannelNumericUpDown.Name = "samplesPerChannelNumericUpDown";
            this.samplesPerChannelNumericUpDown.Size = new System.Drawing.Size(137, 20);
            this.samplesPerChannelNumericUpDown.TabIndex = 1;
            this.samplesPerChannelNumericUpDown.Value = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            // 
            // shuntCalibrationParametersGroupBox
            // 
            this.shuntCalibrationParametersGroupBox.Controls.Add(this.shuntElementLocationComboBox);
            this.shuntCalibrationParametersGroupBox.Controls.Add(this.shuntElementLocationLabel);
            this.shuntCalibrationParametersGroupBox.Controls.Add(this.shuntResistanceNumericUpDown);
            this.shuntCalibrationParametersGroupBox.Controls.Add(this.shuntResistanceLabel);
            this.shuntCalibrationParametersGroupBox.Location = new System.Drawing.Point(12, 388);
            this.shuntCalibrationParametersGroupBox.Name = "shuntCalibrationParametersGroupBox";
            this.shuntCalibrationParametersGroupBox.Size = new System.Drawing.Size(280, 78);
            this.shuntCalibrationParametersGroupBox.TabIndex = 8;
            this.shuntCalibrationParametersGroupBox.TabStop = false;
            this.shuntCalibrationParametersGroupBox.Text = "Shunt Calibration Parameters";
            // 
            // shuntElementLocationComboBox
            // 
            this.shuntElementLocationComboBox.FormattingEnabled = true;
            this.shuntElementLocationComboBox.Items.AddRange(new object[] {
            "None",
            "R1",
            "R2",
            "R3",
            "R4"});
            this.shuntElementLocationComboBox.Location = new System.Drawing.Point(135, 46);
            this.shuntElementLocationComboBox.Name = "shuntElementLocationComboBox";
            this.shuntElementLocationComboBox.Size = new System.Drawing.Size(138, 21);
            this.shuntElementLocationComboBox.TabIndex = 3;
            // 
            // shuntElementLocationLabel
            // 
            this.shuntElementLocationLabel.Location = new System.Drawing.Point(16, 43);
            this.shuntElementLocationLabel.Name = "shuntElementLocationLabel";
            this.shuntElementLocationLabel.Size = new System.Drawing.Size(112, 30);
            this.shuntElementLocationLabel.TabIndex = 2;
            this.shuntElementLocationLabel.Text = "Shunt Element Location:";
            // 
            // shuntResistanceNumericUpDown
            // 
            this.shuntResistanceNumericUpDown.DecimalPlaces = 2;
            this.shuntResistanceNumericUpDown.Location = new System.Drawing.Point(136, 20);
            this.shuntResistanceNumericUpDown.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.shuntResistanceNumericUpDown.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.shuntResistanceNumericUpDown.Name = "shuntResistanceNumericUpDown";
            this.shuntResistanceNumericUpDown.Size = new System.Drawing.Size(137, 20);
            this.shuntResistanceNumericUpDown.TabIndex = 1;
            this.shuntResistanceNumericUpDown.Value = new decimal(new int[] {
            10000000,
            0,
            0,
            131072});
            // 
            // shuntResistanceLabel
            // 
            this.shuntResistanceLabel.AutoSize = true;
            this.shuntResistanceLabel.Location = new System.Drawing.Point(16, 22);
            this.shuntResistanceLabel.Name = "shuntResistanceLabel";
            this.shuntResistanceLabel.Size = new System.Drawing.Size(94, 13);
            this.shuntResistanceLabel.TabIndex = 0;
            this.shuntResistanceLabel.Text = "Shunt Resistance:";
            // 
            // sensorScalingParametersGroupBox
            // 
            this.sensorScalingParametersGroupBox.Controls.Add(this.physicalUnitsComboBox);
            this.sensorScalingParametersGroupBox.Controls.Add(this.electricalUnitsComboBox);
            this.sensorScalingParametersGroupBox.Controls.Add(this.physicalUnitsLabel);
            this.sensorScalingParametersGroupBox.Controls.Add(this.electricalUnitsLabel);
            this.sensorScalingParametersGroupBox.Controls.Add(this.sensorScalingTabControl);
            this.sensorScalingParametersGroupBox.Location = new System.Drawing.Point(298, 226);
            this.sensorScalingParametersGroupBox.Name = "sensorScalingParametersGroupBox";
            this.sensorScalingParametersGroupBox.Size = new System.Drawing.Size(478, 276);
            this.sensorScalingParametersGroupBox.TabIndex = 9;
            this.sensorScalingParametersGroupBox.TabStop = false;
            this.sensorScalingParametersGroupBox.Text = "Sensor Scaling Parameters";
            // 
            // physicalUnitsComboBox
            // 
            this.physicalUnitsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.physicalUnitsComboBox.FormattingEnabled = true;
            this.physicalUnitsComboBox.Items.AddRange(new object[] {
            "Pounds",
            "kgf",
            "Newtons"});
            this.physicalUnitsComboBox.Location = new System.Drawing.Point(338, 240);
            this.physicalUnitsComboBox.Name = "physicalUnitsComboBox";
            this.physicalUnitsComboBox.Size = new System.Drawing.Size(121, 21);
            this.physicalUnitsComboBox.TabIndex = 4;
            // 
            // electricalUnitsComboBox
            // 
            this.electricalUnitsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.electricalUnitsComboBox.FormattingEnabled = true;
            this.electricalUnitsComboBox.Items.AddRange(new object[] {
            "mV/V",
            "V/V"});
            this.electricalUnitsComboBox.Location = new System.Drawing.Point(106, 240);
            this.electricalUnitsComboBox.Name = "electricalUnitsComboBox";
            this.electricalUnitsComboBox.Size = new System.Drawing.Size(121, 21);
            this.electricalUnitsComboBox.TabIndex = 3;
            // 
            // physicalUnitsLabel
            // 
            this.physicalUnitsLabel.AutoSize = true;
            this.physicalUnitsLabel.Location = new System.Drawing.Point(256, 243);
            this.physicalUnitsLabel.Name = "physicalUnitsLabel";
            this.physicalUnitsLabel.Size = new System.Drawing.Size(76, 13);
            this.physicalUnitsLabel.TabIndex = 2;
            this.physicalUnitsLabel.Text = "Physical Units:";
            // 
            // electricalUnitsLabel
            // 
            this.electricalUnitsLabel.AutoSize = true;
            this.electricalUnitsLabel.Location = new System.Drawing.Point(20, 243);
            this.electricalUnitsLabel.Name = "electricalUnitsLabel";
            this.electricalUnitsLabel.Size = new System.Drawing.Size(80, 13);
            this.electricalUnitsLabel.TabIndex = 1;
            this.electricalUnitsLabel.Text = "Electrical Units:";
            // 
            // sensorScalingTabControl
            // 
            this.sensorScalingTabControl.Controls.Add(this.linearTabPage);
            this.sensorScalingTabControl.Controls.Add(this.tableTabPage);
            this.sensorScalingTabControl.Controls.Add(this.polynomialTabPage);
            this.sensorScalingTabControl.Location = new System.Drawing.Point(8, 19);
            this.sensorScalingTabControl.Name = "sensorScalingTabControl";
            this.sensorScalingTabControl.SelectedIndex = 0;
            this.sensorScalingTabControl.Size = new System.Drawing.Size(464, 210);
            this.sensorScalingTabControl.TabIndex = 0;
            // 
            // linearTabPage
            // 
            this.linearTabPage.Controls.Add(this.secondPhysicalValueNumericUpDown);
            this.linearTabPage.Controls.Add(this.firstPhysicalValueNumericUpDown);
            this.linearTabPage.Controls.Add(this.secondPhysicalValueLabel);
            this.linearTabPage.Controls.Add(this.firstPhysicalValueLabel);
            this.linearTabPage.Controls.Add(this.secondElectricalValueNumericUpDown);
            this.linearTabPage.Controls.Add(this.firstElectricalValueNumericUpDown);
            this.linearTabPage.Controls.Add(this.secondElectricalValueLabel);
            this.linearTabPage.Controls.Add(this.firstElectricalValueLabel);
            this.linearTabPage.Location = new System.Drawing.Point(4, 22);
            this.linearTabPage.Name = "linearTabPage";
            this.linearTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.linearTabPage.Size = new System.Drawing.Size(456, 184);
            this.linearTabPage.TabIndex = 0;
            this.linearTabPage.Text = "Two Point Linear";
            this.linearTabPage.UseVisualStyleBackColor = true;
            // 
            // secondPhysicalValueNumericUpDown
            // 
            this.secondPhysicalValueNumericUpDown.DecimalPlaces = 2;
            this.secondPhysicalValueNumericUpDown.Location = new System.Drawing.Point(249, 115);
            this.secondPhysicalValueNumericUpDown.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.secondPhysicalValueNumericUpDown.Name = "secondPhysicalValueNumericUpDown";
            this.secondPhysicalValueNumericUpDown.Size = new System.Drawing.Size(120, 20);
            this.secondPhysicalValueNumericUpDown.TabIndex = 8;
            this.secondPhysicalValueNumericUpDown.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // firstPhysicalValueNumericUpDown
            // 
            this.firstPhysicalValueNumericUpDown.DecimalPlaces = 2;
            this.firstPhysicalValueNumericUpDown.Location = new System.Drawing.Point(249, 66);
            this.firstPhysicalValueNumericUpDown.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.firstPhysicalValueNumericUpDown.Name = "firstPhysicalValueNumericUpDown";
            this.firstPhysicalValueNumericUpDown.Size = new System.Drawing.Size(120, 20);
            this.firstPhysicalValueNumericUpDown.TabIndex = 7;
            // 
            // secondPhysicalValueLabel
            // 
            this.secondPhysicalValueLabel.AutoSize = true;
            this.secondPhysicalValueLabel.Location = new System.Drawing.Point(246, 99);
            this.secondPhysicalValueLabel.Name = "secondPhysicalValueLabel";
            this.secondPhysicalValueLabel.Size = new System.Drawing.Size(119, 13);
            this.secondPhysicalValueLabel.TabIndex = 6;
            this.secondPhysicalValueLabel.Text = "Second Physical Value:";
            // 
            // firstPhysicalValueLabel
            // 
            this.firstPhysicalValueLabel.AutoSize = true;
            this.firstPhysicalValueLabel.Location = new System.Drawing.Point(246, 50);
            this.firstPhysicalValueLabel.Name = "firstPhysicalValueLabel";
            this.firstPhysicalValueLabel.Size = new System.Drawing.Size(101, 13);
            this.firstPhysicalValueLabel.TabIndex = 5;
            this.firstPhysicalValueLabel.Text = "First Physical Value:";
            // 
            // secondElectricalValueNumericUpDown
            // 
            this.secondElectricalValueNumericUpDown.DecimalPlaces = 2;
            this.secondElectricalValueNumericUpDown.Location = new System.Drawing.Point(91, 115);
            this.secondElectricalValueNumericUpDown.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.secondElectricalValueNumericUpDown.Name = "secondElectricalValueNumericUpDown";
            this.secondElectricalValueNumericUpDown.Size = new System.Drawing.Size(120, 20);
            this.secondElectricalValueNumericUpDown.TabIndex = 4;
            this.secondElectricalValueNumericUpDown.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // firstElectricalValueNumericUpDown
            // 
            this.firstElectricalValueNumericUpDown.DecimalPlaces = 2;
            this.firstElectricalValueNumericUpDown.Location = new System.Drawing.Point(91, 66);
            this.firstElectricalValueNumericUpDown.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.firstElectricalValueNumericUpDown.Name = "firstElectricalValueNumericUpDown";
            this.firstElectricalValueNumericUpDown.Size = new System.Drawing.Size(120, 20);
            this.firstElectricalValueNumericUpDown.TabIndex = 3;
            // 
            // secondElectricalValueLabel
            // 
            this.secondElectricalValueLabel.AutoSize = true;
            this.secondElectricalValueLabel.Location = new System.Drawing.Point(88, 99);
            this.secondElectricalValueLabel.Name = "secondElectricalValueLabel";
            this.secondElectricalValueLabel.Size = new System.Drawing.Size(123, 13);
            this.secondElectricalValueLabel.TabIndex = 1;
            this.secondElectricalValueLabel.Text = "Second Electrical Value:";
            // 
            // firstElectricalValueLabel
            // 
            this.firstElectricalValueLabel.AutoSize = true;
            this.firstElectricalValueLabel.Location = new System.Drawing.Point(88, 50);
            this.firstElectricalValueLabel.Name = "firstElectricalValueLabel";
            this.firstElectricalValueLabel.Size = new System.Drawing.Size(105, 13);
            this.firstElectricalValueLabel.TabIndex = 0;
            this.firstElectricalValueLabel.Text = "First Electrical Value:";
            // 
            // tableTabPage
            // 
            this.tableTabPage.Controls.Add(this.tableDataGridView);
            this.tableTabPage.Location = new System.Drawing.Point(4, 22);
            this.tableTabPage.Name = "tableTabPage";
            this.tableTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.tableTabPage.Size = new System.Drawing.Size(456, 184);
            this.tableTabPage.TabIndex = 1;
            this.tableTabPage.Text = "Table";
            this.tableTabPage.UseVisualStyleBackColor = true;
            // 
            // tableDataGridView
            // 
            this.tableDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tableDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.electricalValuesColumn,
            this.physicalValuesColumn});
            this.tableDataGridView.Location = new System.Drawing.Point(54, 8);
            this.tableDataGridView.Name = "tableDataGridView";
            this.tableDataGridView.Size = new System.Drawing.Size(344, 170);
            this.tableDataGridView.TabIndex = 0;
            // 
            // electricalValuesColumn
            // 
            this.electricalValuesColumn.HeaderText = "Electrical Values";
            this.electricalValuesColumn.Name = "electricalValuesColumn";
            this.electricalValuesColumn.Width = 150;
            // 
            // physicalValuesColumn
            // 
            this.physicalValuesColumn.HeaderText = "Physical Values";
            this.physicalValuesColumn.Name = "physicalValuesColumn";
            this.physicalValuesColumn.Width = 150;
            // 
            // polynomialTabPage
            // 
            this.polynomialTabPage.Controls.Add(this.label1);
            this.polynomialTabPage.Controls.Add(this.polynomialDataGrid);
            this.polynomialTabPage.Controls.Add(this.polynomialRangeGroupBox);
            this.polynomialTabPage.Controls.Add(this.coefficientsDirectionGroupBox);
            this.polynomialTabPage.Location = new System.Drawing.Point(4, 22);
            this.polynomialTabPage.Name = "polynomialTabPage";
            this.polynomialTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.polynomialTabPage.Size = new System.Drawing.Size(456, 184);
            this.polynomialTabPage.TabIndex = 2;
            this.polynomialTabPage.Text = "Polynomial";
            this.polynomialTabPage.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(321, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 68);
            this.label1.TabIndex = 5;
            this.label1.Text = "Note: The top most element of the coefficients table, is the independent term.";
            // 
            // polynomialDataGrid
            // 
            this.polynomialDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.polynomialDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.coefficientsColumn});
            this.polynomialDataGrid.Location = new System.Drawing.Point(155, 11);
            this.polynomialDataGrid.Name = "polynomialDataGrid";
            this.polynomialDataGrid.Size = new System.Drawing.Size(152, 167);
            this.polynomialDataGrid.TabIndex = 4;
            // 
            // coefficientsColumn
            // 
            this.coefficientsColumn.HeaderText = "Coefficients";
            this.coefficientsColumn.Name = "coefficientsColumn";
            // 
            // polynomialRangeGroupBox
            // 
            this.polynomialRangeGroupBox.Controls.Add(this.maximumNumericUpDown);
            this.polynomialRangeGroupBox.Controls.Add(this.minimumNumericUpDown);
            this.polynomialRangeGroupBox.Controls.Add(this.maximumLabel);
            this.polynomialRangeGroupBox.Controls.Add(this.minimumLabel);
            this.polynomialRangeGroupBox.Location = new System.Drawing.Point(6, 84);
            this.polynomialRangeGroupBox.Name = "polynomialRangeGroupBox";
            this.polynomialRangeGroupBox.Size = new System.Drawing.Size(143, 94);
            this.polynomialRangeGroupBox.TabIndex = 3;
            this.polynomialRangeGroupBox.TabStop = false;
            this.polynomialRangeGroupBox.Text = "Polynomial Range";
            // 
            // maximumNumericUpDown
            // 
            this.maximumNumericUpDown.DecimalPlaces = 2;
            this.maximumNumericUpDown.Location = new System.Drawing.Point(62, 55);
            this.maximumNumericUpDown.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.maximumNumericUpDown.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.maximumNumericUpDown.Name = "maximumNumericUpDown";
            this.maximumNumericUpDown.Size = new System.Drawing.Size(70, 20);
            this.maximumNumericUpDown.TabIndex = 3;
            this.maximumNumericUpDown.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // minimumNumericUpDown
            // 
            this.minimumNumericUpDown.DecimalPlaces = 2;
            this.minimumNumericUpDown.Location = new System.Drawing.Point(62, 19);
            this.minimumNumericUpDown.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.minimumNumericUpDown.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.minimumNumericUpDown.Name = "minimumNumericUpDown";
            this.minimumNumericUpDown.Size = new System.Drawing.Size(70, 20);
            this.minimumNumericUpDown.TabIndex = 2;
            this.minimumNumericUpDown.Value = new decimal(new int[] {
            2,
            0,
            0,
            -2147483648});
            // 
            // maximumLabel
            // 
            this.maximumLabel.AutoSize = true;
            this.maximumLabel.Location = new System.Drawing.Point(6, 57);
            this.maximumLabel.Name = "maximumLabel";
            this.maximumLabel.Size = new System.Drawing.Size(54, 13);
            this.maximumLabel.TabIndex = 1;
            this.maximumLabel.Text = "Maximum:";
            // 
            // minimumLabel
            // 
            this.minimumLabel.AutoSize = true;
            this.minimumLabel.Location = new System.Drawing.Point(6, 21);
            this.minimumLabel.Name = "minimumLabel";
            this.minimumLabel.Size = new System.Drawing.Size(51, 13);
            this.minimumLabel.TabIndex = 0;
            this.minimumLabel.Text = "Minimum:";
            // 
            // coefficientsDirectionGroupBox
            // 
            this.coefficientsDirectionGroupBox.Controls.Add(this.physToElecRadioButton);
            this.coefficientsDirectionGroupBox.Controls.Add(this.electToPhysRadioButton);
            this.coefficientsDirectionGroupBox.Location = new System.Drawing.Point(6, 11);
            this.coefficientsDirectionGroupBox.Name = "coefficientsDirectionGroupBox";
            this.coefficientsDirectionGroupBox.Size = new System.Drawing.Size(143, 67);
            this.coefficientsDirectionGroupBox.TabIndex = 2;
            this.coefficientsDirectionGroupBox.TabStop = false;
            this.coefficientsDirectionGroupBox.Text = "Coefficients Direction";
            // 
            // physToElecRadioButton
            // 
            this.physToElecRadioButton.AutoSize = true;
            this.physToElecRadioButton.Location = new System.Drawing.Point(6, 38);
            this.physToElecRadioButton.Name = "physToElecRadioButton";
            this.physToElecRadioButton.Size = new System.Drawing.Size(126, 17);
            this.physToElecRadioButton.TabIndex = 3;
            this.physToElecRadioButton.TabStop = true;
            this.physToElecRadioButton.Text = "Physical To Electrical";
            this.physToElecRadioButton.UseVisualStyleBackColor = true;
            // 
            // electToPhysRadioButton
            // 
            this.electToPhysRadioButton.AutoSize = true;
            this.electToPhysRadioButton.Location = new System.Drawing.Point(6, 19);
            this.electToPhysRadioButton.Name = "electToPhysRadioButton";
            this.electToPhysRadioButton.Size = new System.Drawing.Size(126, 17);
            this.electToPhysRadioButton.TabIndex = 2;
            this.electToPhysRadioButton.TabStop = true;
            this.electToPhysRadioButton.Text = "Electrical To Physical";
            this.electToPhysRadioButton.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(784, 514);
            this.Controls.Add(this.sensorScalingParametersGroupBox);
            this.Controls.Add(this.shuntCalibrationParametersGroupBox);
            this.Controls.Add(this.wheatstoneBridgeParametersGroupBox);
            this.Controls.Add(this.acquisitionResultGroupBox);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.channelParametersGroupBox);
            this.Controls.Add(this.timingParametersGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Continuously Acquire Force Bridge Samples";
            this.wheatstoneBridgeParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nomGageResNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.excitationValueNumericUpDown)).EndInit();
            this.acquisitionResultGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.acquisitionDataGrid)).EndInit();
            this.channelParametersGroupBox.ResumeLayout(false);
            this.channelParametersGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.minimumValueNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maximumValueNumericUpDown)).EndInit();
            this.timingParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rateNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerChannelNumericUpDown)).EndInit();
            this.shuntCalibrationParametersGroupBox.ResumeLayout(false);
            this.shuntCalibrationParametersGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shuntResistanceNumericUpDown)).EndInit();
            this.sensorScalingParametersGroupBox.ResumeLayout(false);
            this.sensorScalingParametersGroupBox.PerformLayout();
            this.sensorScalingTabControl.ResumeLayout(false);
            this.linearTabPage.ResumeLayout(false);
            this.linearTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.secondPhysicalValueNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.firstPhysicalValueNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.secondElectricalValueNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.firstElectricalValueNumericUpDown)).EndInit();
            this.tableTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tableDataGridView)).EndInit();
            this.polynomialTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.polynomialDataGrid)).EndInit();
            this.polynomialRangeGroupBox.ResumeLayout(false);
            this.polynomialRangeGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maximumNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minimumNumericUpDown)).EndInit();
            this.coefficientsDirectionGroupBox.ResumeLayout(false);
            this.coefficientsDirectionGroupBox.PerformLayout();
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
            Cursor.Current = Cursors.WaitCursor;
            startButton.Enabled = false;
            stopButton.Enabled = true;
            try
            {
                // Get the selected AIBridgeConfiguation
                AIBridgeConfiguration bridgeConfiguration;
                if (bridgeConfigurationComboBox.SelectedIndex == 0)
                    bridgeConfiguration = AIBridgeConfiguration.FullBridge;
                else if (bridgeConfigurationComboBox.SelectedIndex == 1)
                    bridgeConfiguration = AIBridgeConfiguration.HalfBridge;
                else if (bridgeConfigurationComboBox.SelectedIndex == 2)
                    bridgeConfiguration = AIBridgeConfiguration.QuarterBridge;
                else
                    bridgeConfiguration = AIBridgeConfiguration.NoBridge;

                // Get the excitation source
                AIExcitationSource excitationSource;
                if (excitationSourceComboBox.SelectedIndex == 0)
                    excitationSource = AIExcitationSource.Internal;
                else if (excitationSourceComboBox.SelectedIndex == 1)
                    excitationSource = AIExcitationSource.External;
                else
                    excitationSource = AIExcitationSource.None;

                AIForceUnits units;
                if (unitsComboBox.SelectedItem == "kgf")
                    units = AIForceUnits.KilogramForce;
                else if (unitsComboBox.SelectedItem == "Pounds")
                    units = AIForceUnits.Pounds;
                else
                    units = AIForceUnits.Newtons;

                AIBridgeElectricalUnits electricalUnits;
                if (electricalUnitsComboBox.SelectedItem == "mV/V")
                    electricalUnits = AIBridgeElectricalUnits.MillivoltsPerVolt;
                else
                    electricalUnits = AIBridgeElectricalUnits.VoltsPerVolt;

                AIBridgePhysicalUnits physicalUnits;
                if (physicalUnitsComboBox.SelectedItem == "kgf")
                    physicalUnits = AIBridgePhysicalUnits.KilogramForce;
                else if (physicalUnitsComboBox.SelectedItem == "Pounds")
                    physicalUnits = AIBridgePhysicalUnits.Pounds;
                else
                    physicalUnits = AIBridgePhysicalUnits.Newtons;

                // Create and configure AI channel
                myTask = new Task();
                AIChannel myAIChannel;

                if (sensorScalingTabControl.SelectedIndex == 0)
                {
                    myAIChannel = myTask.AIChannels.CreateForceBridgeTwoPointLinearChannel(
                        physicalChannelComboBox.Text, "", Convert.ToDouble(minimumValueNumericUpDown.Value),
                        Convert.ToDouble(maximumValueNumericUpDown.Value), units, bridgeConfiguration, excitationSource,
                        Convert.ToDouble(excitationValueNumericUpDown.Value), Convert.ToDouble(nomGageResNumericUpDown.Value),
                        Convert.ToDouble(firstElectricalValueNumericUpDown.Value),
                        Convert.ToDouble(secondElectricalValueNumericUpDown.Value),
                        electricalUnits,
                        Convert.ToDouble(firstPhysicalValueNumericUpDown.Value),
                        Convert.ToDouble(secondPhysicalValueNumericUpDown.Value),
                        physicalUnits);
                }
                else if (sensorScalingTabControl.SelectedIndex == 1)
                {
                    double[] electricalValues = new double[tableDataGridView.Rows.Count - 1];
                    double[] physicalValues = new double[tableDataGridView.Rows.Count - 1];
                    for (int i = 0; i < electricalValues.Length; i++)
                    {
                        electricalValues[i] = Convert.ToDouble(tableDataGridView.Rows[i].Cells[0].Value);
                        physicalValues[i] = Convert.ToDouble(tableDataGridView.Rows[i].Cells[1].Value);
                    }
                    myAIChannel = myTask.AIChannels.CreateForceBridgeTableChannel(physicalChannelComboBox.Text, "",
                        Convert.ToDouble(minimumValueNumericUpDown.Value),
                        Convert.ToDouble(maximumValueNumericUpDown.Value),
                        units, bridgeConfiguration, excitationSource,
                        Convert.ToDouble(excitationValueNumericUpDown.Value),
                        Convert.ToDouble(nomGageResNumericUpDown.Value),
                        electricalValues, electricalUnits,
                        physicalValues, physicalUnits);
                }
                else
                {
                    double[] coefficients = new double[polynomialDataGrid.Rows.Count - 1];
                    double[] forward;
                    double[] reverse;
                    for (int i = 0; i < coefficients.Length; i++)
                    {
                        coefficients[i] = Convert.ToDouble(polynomialDataGrid.Rows[i].Cells[0].Value);
                    }
                    if (electToPhysRadioButton.Checked)
                    {
                        forward = coefficients;
                        PolynomialScale scale = new PolynomialScale("scale",
                            PolynomialDirection.Forward,
                            forward, Convert.ToDouble(minimumNumericUpDown.Value),
                            Convert.ToDouble(maximumNumericUpDown.Value));
                        reverse = scale.ReverseCoefficients;
                    }
                    else
                    {
                        reverse = coefficients;
                        PolynomialScale scale = new PolynomialScale("scale",
                            PolynomialDirection.Reverse,
                            reverse, Convert.ToDouble(minimumNumericUpDown.Value),
                            Convert.ToDouble(maximumNumericUpDown.Value));
                        forward = scale.ForwardCoefficients;
                    }

                    myAIChannel = myTask.AIChannels.CreateForceBridgePolynomialChannel(physicalChannelComboBox.Text, "",
                        Convert.ToDouble(minimumValueNumericUpDown.Value),
                        Convert.ToDouble(maximumValueNumericUpDown.Value),
                        units, bridgeConfiguration, excitationSource,
                        Convert.ToDouble(excitationValueNumericUpDown.Value),
                        Convert.ToDouble(nomGageResNumericUpDown.Value),
                        forward, reverse, electricalUnits, physicalUnits);
                }

                // Configure the sample clock
                myTask.Timing.ConfigureSampleClock("", Convert.ToDouble(rateNumericUpDown.Value), SampleClockActiveEdge.Rising, SampleQuantityMode.ContinuousSamples);

                // Verify task
                myTask.Control(TaskAction.Verify);

                if (bridgeNullCheckBox.Checked)
                    myAIChannel.PerformBridgeOffsetNullingCalibration();
                if (shuntCalibrationCheckBox.Checked)
                {

                }

                InitializeDataTable(ref dataTable);
                acquisitionDataGrid.DataSource = dataTable;

                runningTask = myTask;

                reader = new AnalogMultiChannelReader(myTask.Stream);

                callback = new AsyncCallback(AnalogCallback);

                myTask.Start();

                myTask.SynchronizeCallbacks = true;
                reader.BeginReadWaveform(Convert.ToInt32(samplesPerChannelNumericUpDown.Value), callback, myTask);
            }
            catch (DaqException exception)
            {
                HandleExceptions(exception);
            }

            Cursor.Current = Cursors.Default;
        }

        private void stopButton_Click(object sender, System.EventArgs e)
        {
            runningTask = null;
            myTask.Dispose();

            stopButton.Enabled = false;
            startButton.Enabled = true;
        }

        private void AnalogCallback(IAsyncResult ar)
        {
            try
            {
                if (runningTask != null && runningTask == ar.AsyncState)
                {
                    // Read the available data from the channels
                    waveform = reader.EndReadWaveform(ar);

                    // Populate data table
                    dataToDataTable(waveform, ref dataTable);

                    // Set up a new callback
                    reader.BeginMemoryOptimizedReadWaveform(Convert.ToInt32(samplesPerChannelNumericUpDown.Value), callback, myTask, waveform);                 
                }
            }
            catch (DaqException exception)
            {
                HandleExceptions(exception);
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
                    if (sample == 15)
                        break;

                    dataTable.Rows[sample][currentLineIndex] = waveform.Samples[sample].Value;
                }
                currentLineIndex++;
            }
        }

        private void InitializeDataTable(ref DataTable data)
        {
            int numOfLines = Convert.ToInt32(myTask.AIChannels.Count);
            data.Rows.Clear();
            data.Columns.Clear();
            dataColumn = new DataColumn[numOfLines];
            int numOfRows = 15;

            for (int currentLineIndex = 0; currentLineIndex < numOfLines; currentLineIndex++)
            {
                dataColumn[currentLineIndex] = new DataColumn();
                dataColumn[currentLineIndex].DataType = typeof(double);
                dataColumn[currentLineIndex].ColumnName = myTask.AIChannels[currentLineIndex].PhysicalName;
            }
            data.Columns.AddRange(dataColumn);

            for (int currentDataIndex = 0; currentDataIndex < numOfRows; currentDataIndex++)
            {
                object[] rowArr = new object[numOfLines];
                data.Rows.Add(rowArr);

            }
        }

        private void HandleExceptions(DaqException exception)
        {
            MessageBox.Show(exception.Message);

            runningTask = null;
            myTask.Dispose();
            startButton.Enabled = true;
            stopButton.Enabled = false;
        }
    }
}
