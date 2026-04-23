/******************************************************************************
*
* Example program:
*   ContAcqCustomVoltageSamples_9237
*
* Category:
*   AI
*
* Description:
*   This example performs Wheatstone Bridge measurements with offset nulling if
*   desired.
*
* Instructions for running:
*   1.  Enter the list of physical channels, and set the attributesof the bridge
*       configuration connected to all the channels.The 'Maximum Value' and
*       'Minimum Value' inputs specify therange, in Custom Scale units, that you
*       expect of yourmeasurements.
*   2.  Make sure your Bridge sensor is in its relaxed state.
*   3.  You may check the 'Perform Bridge Null?' option to automaticallynull out
*       your offset by performing a hardware nullingoperation (if supported by
*       the hardware) followed by asoftware nulling operation. (NOTE: The
*       software nullingoperation will cause a loss in dynamic range while a
*       hardwarenulling operation will not cause any loss in the dynamicrange).
*   4.  Specify Sensor Scaling Parameters. You can choose a LinearScale or Map
*       Ranges Scale. The channel Maximum and Minimumvalues are specified in
*       terms of the scaled units.
*   5.  Run the example and do not disturb your bridge sensor untildata starts
*       being plotted.
*
* Steps:
*   1.  Create custom scale.
*   2.  Create a new Task. Create a AIChannel by using the
*       CreateVoltageChannelWithExcitation method.
*   3.  Set the rate for the sample clock by using the
*       Timing.ConfigureSampleClock method. Additionally, define the sample mode
*       to be continuous.
*   4.  If nulling is desired, call the DAQmx Perform Bridge OffsetNulling
*       Calibration function to perform both hardware nulling(if supported) and
*       software nulling.
*   5.  Call AnalogMultiChannelReader.BeginReadWaveform to install a callback
*       and begin the asynchronous read operation.
*   6.  Inside the callback, call AnalogMultiChannelReader.EndReadWaveform to
*       retrieve the data from the read operation.  
*   7.  Call AnalogMultiChannelReader.BeginMemoryOptimizedReadWaveform
*   8.  When the user presses the stop button, stop the task.
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
*   Make sure your signal input terminal matches the Physical Channel I/O
*   control.  For more detailed connection information and bridge calibration
*   procedures refer to your NI 9237 module's hardware reference manual.
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

namespace NationalInstruments.Examples.ContAcqCustomVoltageSamples_9237
{
    /// <summary>
    /// Summary description for MainForm.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.GroupBox sensorScalingParametersGroupBox;
        private System.Windows.Forms.TabPage linearTabPage;
        private System.Windows.Forms.TextBox linearScaledUnitsTextBox;
        private System.Windows.Forms.Label mLabel;
        private System.Windows.Forms.NumericUpDown mNumericUpDown;
        private System.Windows.Forms.Label equationLabel;
        private System.Windows.Forms.Label bLabel;
        private System.Windows.Forms.NumericUpDown bNumericUpDown;
        private System.Windows.Forms.Label linearScaledUnitsLabel;
        private System.Windows.Forms.TabPage mapRangesTabPage;
        private System.Windows.Forms.TextBox mapRangesScaledUnitsTextBox;
        private System.Windows.Forms.Label minLabel;
        private System.Windows.Forms.NumericUpDown minNumericUpDown;
        private System.Windows.Forms.Label maxLabel;
        private System.Windows.Forms.NumericUpDown maxNumericUpDown;
        private System.Windows.Forms.Label mapRangesScaledUnitsLabel;
        private System.Windows.Forms.NumericUpDown minScaledNumericUpDown;
        private System.Windows.Forms.Label maxScaledLabel;
        private System.Windows.Forms.NumericUpDown maxScaledNumericUpDown;
        private System.Windows.Forms.Label minScaledLabel;
        private System.Windows.Forms.GroupBox wheatstoneBridgeParametersGroupBox;
        private System.Windows.Forms.ComboBox bridgeConfigurationComboBox;
        private System.Windows.Forms.Label bridgeConfigurationLabel;
        private System.Windows.Forms.Label excitationSourceLabel;
        private System.Windows.Forms.ComboBox excitationSourceComboBox;
        private System.Windows.Forms.ComboBox excitationValueComboBox;
        private System.Windows.Forms.Label excitationValueLabel;
        private System.Windows.Forms.GroupBox acquisitionResultGroupBox;
        private System.Windows.Forms.DataGrid acquisitionDataGrid;
        private System.Windows.Forms.CheckBox bridgeNullCheckBox;
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

        private System.Windows.Forms.TabControl scaleTabControl;
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

            dataTable = new DataTable();
            bridgeConfigurationComboBox.SelectedIndex = 0;
            excitationSourceComboBox.SelectedIndex = 0;
            excitationValueComboBox.SelectedIndex = 0;

            physicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AI, PhysicalChannelAccess.External));

            if (physicalChannelComboBox.Items.Count > 0)
            {
                physicalChannelComboBox.SelectedIndex = 0;
                startButton.Enabled = true;
            }
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
            this.sensorScalingParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.scaleTabControl = new System.Windows.Forms.TabControl();
            this.linearTabPage = new System.Windows.Forms.TabPage();
            this.linearScaledUnitsTextBox = new System.Windows.Forms.TextBox();
            this.mLabel = new System.Windows.Forms.Label();
            this.mNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.equationLabel = new System.Windows.Forms.Label();
            this.bLabel = new System.Windows.Forms.Label();
            this.bNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.linearScaledUnitsLabel = new System.Windows.Forms.Label();
            this.mapRangesTabPage = new System.Windows.Forms.TabPage();
            this.mapRangesScaledUnitsTextBox = new System.Windows.Forms.TextBox();
            this.minLabel = new System.Windows.Forms.Label();
            this.minNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.maxLabel = new System.Windows.Forms.Label();
            this.maxNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.mapRangesScaledUnitsLabel = new System.Windows.Forms.Label();
            this.minScaledNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.maxScaledLabel = new System.Windows.Forms.Label();
            this.maxScaledNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.minScaledLabel = new System.Windows.Forms.Label();
            this.wheatstoneBridgeParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.bridgeConfigurationComboBox = new System.Windows.Forms.ComboBox();
            this.bridgeConfigurationLabel = new System.Windows.Forms.Label();
            this.excitationSourceLabel = new System.Windows.Forms.Label();
            this.excitationSourceComboBox = new System.Windows.Forms.ComboBox();
            this.excitationValueComboBox = new System.Windows.Forms.ComboBox();
            this.excitationValueLabel = new System.Windows.Forms.Label();
            this.acquisitionResultGroupBox = new System.Windows.Forms.GroupBox();
            this.acquisitionDataGrid = new System.Windows.Forms.DataGrid();
            this.bridgeNullCheckBox = new System.Windows.Forms.CheckBox();
            this.stopButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.channelParametersGroupBox = new System.Windows.Forms.GroupBox();
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
            this.sensorScalingParametersGroupBox.SuspendLayout();
            this.scaleTabControl.SuspendLayout();
            this.linearTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bNumericUpDown)).BeginInit();
            this.mapRangesTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.minNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minScaledNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxScaledNumericUpDown)).BeginInit();
            this.wheatstoneBridgeParametersGroupBox.SuspendLayout();
            this.acquisitionResultGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.acquisitionDataGrid)).BeginInit();
            this.channelParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.minimumValueNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maximumValueNumericUpDown)).BeginInit();
            this.timingParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rateNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerChannelNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // sensorScalingParametersGroupBox
            // 
            this.sensorScalingParametersGroupBox.Controls.Add(this.scaleTabControl);
            this.sensorScalingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.sensorScalingParametersGroupBox.Location = new System.Drawing.Point(332, 8);
            this.sensorScalingParametersGroupBox.Name = "sensorScalingParametersGroupBox";
            this.sensorScalingParametersGroupBox.Size = new System.Drawing.Size(240, 224);
            this.sensorScalingParametersGroupBox.TabIndex = 3;
            this.sensorScalingParametersGroupBox.TabStop = false;
            this.sensorScalingParametersGroupBox.Text = "Sensor Scaling Parameters";
            // 
            // scaleTabControl
            // 
            this.scaleTabControl.Controls.Add(this.linearTabPage);
            this.scaleTabControl.Controls.Add(this.mapRangesTabPage);
            this.scaleTabControl.Location = new System.Drawing.Point(8, 24);
            this.scaleTabControl.Name = "scaleTabControl";
            this.scaleTabControl.SelectedIndex = 0;
            this.scaleTabControl.Size = new System.Drawing.Size(224, 192);
            this.scaleTabControl.TabIndex = 0;
            // 
            // linearTabPage
            // 
            this.linearTabPage.Controls.Add(this.linearScaledUnitsTextBox);
            this.linearTabPage.Controls.Add(this.mLabel);
            this.linearTabPage.Controls.Add(this.mNumericUpDown);
            this.linearTabPage.Controls.Add(this.equationLabel);
            this.linearTabPage.Controls.Add(this.bLabel);
            this.linearTabPage.Controls.Add(this.bNumericUpDown);
            this.linearTabPage.Controls.Add(this.linearScaledUnitsLabel);
            this.linearTabPage.Location = new System.Drawing.Point(4, 22);
            this.linearTabPage.Name = "linearTabPage";
            this.linearTabPage.Size = new System.Drawing.Size(216, 166);
            this.linearTabPage.TabIndex = 0;
            this.linearTabPage.Text = "Linear";
            // 
            // linearScaledUnitsTextBox
            // 
            this.linearScaledUnitsTextBox.Location = new System.Drawing.Point(96, 132);
            this.linearScaledUnitsTextBox.Name = "linearScaledUnitsTextBox";
            this.linearScaledUnitsTextBox.Size = new System.Drawing.Size(112, 20);
            this.linearScaledUnitsTextBox.TabIndex = 6;
            this.linearScaledUnitsTextBox.Text = "psi";
            // 
            // mLabel
            // 
            this.mLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.mLabel.Location = new System.Drawing.Point(16, 40);
            this.mLabel.Name = "mLabel";
            this.mLabel.Size = new System.Drawing.Size(56, 16);
            this.mLabel.TabIndex = 1;
            this.mLabel.Text = "M:";
            // 
            // mNumericUpDown
            // 
            this.mNumericUpDown.DecimalPlaces = 3;
            this.mNumericUpDown.Location = new System.Drawing.Point(96, 36);
            this.mNumericUpDown.Maximum = new System.Decimal(new int[] {
                                                                           100000,
                                                                           0,
                                                                           0,
                                                                           0});
            this.mNumericUpDown.Minimum = new System.Decimal(new int[] {
                                                                           100000,
                                                                           0,
                                                                           0,
                                                                           -2147483648});
            this.mNumericUpDown.Name = "mNumericUpDown";
            this.mNumericUpDown.Size = new System.Drawing.Size(112, 20);
            this.mNumericUpDown.TabIndex = 2;
            this.mNumericUpDown.Value = new System.Decimal(new int[] {
                                                                         1000,
                                                                         0,
                                                                         0,
                                                                         0});
            // 
            // equationLabel
            // 
            this.equationLabel.Location = new System.Drawing.Point(72, 8);
            this.equationLabel.Name = "equationLabel";
            this.equationLabel.Size = new System.Drawing.Size(64, 16);
            this.equationLabel.TabIndex = 0;
            this.equationLabel.Text = "y = Mx + B";
            // 
            // bLabel
            // 
            this.bLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bLabel.Location = new System.Drawing.Point(16, 88);
            this.bLabel.Name = "bLabel";
            this.bLabel.Size = new System.Drawing.Size(56, 16);
            this.bLabel.TabIndex = 3;
            this.bLabel.Text = "B:";
            // 
            // bNumericUpDown
            // 
            this.bNumericUpDown.DecimalPlaces = 3;
            this.bNumericUpDown.Location = new System.Drawing.Point(96, 84);
            this.bNumericUpDown.Maximum = new System.Decimal(new int[] {
                                                                           1000,
                                                                           0,
                                                                           0,
                                                                           0});
            this.bNumericUpDown.Minimum = new System.Decimal(new int[] {
                                                                           1000,
                                                                           0,
                                                                           0,
                                                                           -2147483648});
            this.bNumericUpDown.Name = "bNumericUpDown";
            this.bNumericUpDown.Size = new System.Drawing.Size(112, 20);
            this.bNumericUpDown.TabIndex = 4;
            // 
            // linearScaledUnitsLabel
            // 
            this.linearScaledUnitsLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.linearScaledUnitsLabel.Location = new System.Drawing.Point(16, 136);
            this.linearScaledUnitsLabel.Name = "linearScaledUnitsLabel";
            this.linearScaledUnitsLabel.Size = new System.Drawing.Size(72, 16);
            this.linearScaledUnitsLabel.TabIndex = 5;
            this.linearScaledUnitsLabel.Text = "Scaled Units:";
            // 
            // mapRangesTabPage
            // 
            this.mapRangesTabPage.Controls.Add(this.mapRangesScaledUnitsTextBox);
            this.mapRangesTabPage.Controls.Add(this.minLabel);
            this.mapRangesTabPage.Controls.Add(this.minNumericUpDown);
            this.mapRangesTabPage.Controls.Add(this.maxLabel);
            this.mapRangesTabPage.Controls.Add(this.maxNumericUpDown);
            this.mapRangesTabPage.Controls.Add(this.mapRangesScaledUnitsLabel);
            this.mapRangesTabPage.Controls.Add(this.minScaledNumericUpDown);
            this.mapRangesTabPage.Controls.Add(this.maxScaledLabel);
            this.mapRangesTabPage.Controls.Add(this.maxScaledNumericUpDown);
            this.mapRangesTabPage.Controls.Add(this.minScaledLabel);
            this.mapRangesTabPage.Location = new System.Drawing.Point(4, 22);
            this.mapRangesTabPage.Name = "mapRangesTabPage";
            this.mapRangesTabPage.Size = new System.Drawing.Size(216, 166);
            this.mapRangesTabPage.TabIndex = 1;
            this.mapRangesTabPage.Text = "Map Ranges";
            this.mapRangesTabPage.Visible = false;
            // 
            // mapRangesScaledUnitsTextBox
            // 
            this.mapRangesScaledUnitsTextBox.Location = new System.Drawing.Point(92, 140);
            this.mapRangesScaledUnitsTextBox.Name = "mapRangesScaledUnitsTextBox";
            this.mapRangesScaledUnitsTextBox.Size = new System.Drawing.Size(112, 20);
            this.mapRangesScaledUnitsTextBox.TabIndex = 9;
            this.mapRangesScaledUnitsTextBox.Text = "lbs";
            // 
            // minLabel
            // 
            this.minLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.minLabel.Location = new System.Drawing.Point(12, 24);
            this.minLabel.Name = "minLabel";
            this.minLabel.Size = new System.Drawing.Size(56, 16);
            this.minLabel.TabIndex = 0;
            this.minLabel.Text = "Min (V):";
            // 
            // minNumericUpDown
            // 
            this.minNumericUpDown.DecimalPlaces = 2;
            this.minNumericUpDown.Location = new System.Drawing.Point(92, 16);
            this.minNumericUpDown.Maximum = new System.Decimal(new int[] {
                                                                             1000,
                                                                             0,
                                                                             0,
                                                                             0});
            this.minNumericUpDown.Minimum = new System.Decimal(new int[] {
                                                                             1000,
                                                                             0,
                                                                             0,
                                                                             -2147483648});
            this.minNumericUpDown.Name = "minNumericUpDown";
            this.minNumericUpDown.Size = new System.Drawing.Size(112, 20);
            this.minNumericUpDown.TabIndex = 1;
            this.minNumericUpDown.Value = new System.Decimal(new int[] {
                                                                           20,
                                                                           0,
                                                                           0,
                                                                           -2147483648});
            // 
            // maxLabel
            // 
            this.maxLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.maxLabel.Location = new System.Drawing.Point(12, 56);
            this.maxLabel.Name = "maxLabel";
            this.maxLabel.Size = new System.Drawing.Size(56, 16);
            this.maxLabel.TabIndex = 2;
            this.maxLabel.Text = "Max (V):";
            // 
            // maxNumericUpDown
            // 
            this.maxNumericUpDown.DecimalPlaces = 2;
            this.maxNumericUpDown.Location = new System.Drawing.Point(92, 48);
            this.maxNumericUpDown.Maximum = new System.Decimal(new int[] {
                                                                             1000,
                                                                             0,
                                                                             0,
                                                                             0});
            this.maxNumericUpDown.Minimum = new System.Decimal(new int[] {
                                                                             1000,
                                                                             0,
                                                                             0,
                                                                             -2147483648});
            this.maxNumericUpDown.Name = "maxNumericUpDown";
            this.maxNumericUpDown.Size = new System.Drawing.Size(112, 20);
            this.maxNumericUpDown.TabIndex = 3;
            this.maxNumericUpDown.Value = new System.Decimal(new int[] {
                                                                           20,
                                                                           0,
                                                                           0,
                                                                           0});
            // 
            // mapRangesScaledUnitsLabel
            // 
            this.mapRangesScaledUnitsLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.mapRangesScaledUnitsLabel.Location = new System.Drawing.Point(12, 144);
            this.mapRangesScaledUnitsLabel.Name = "mapRangesScaledUnitsLabel";
            this.mapRangesScaledUnitsLabel.Size = new System.Drawing.Size(72, 16);
            this.mapRangesScaledUnitsLabel.TabIndex = 8;
            this.mapRangesScaledUnitsLabel.Text = "Scaled Units:";
            // 
            // minScaledNumericUpDown
            // 
            this.minScaledNumericUpDown.DecimalPlaces = 2;
            this.minScaledNumericUpDown.Location = new System.Drawing.Point(92, 80);
            this.minScaledNumericUpDown.Maximum = new System.Decimal(new int[] {
                                                                                   1000,
                                                                                   0,
                                                                                   0,
                                                                                   0});
            this.minScaledNumericUpDown.Minimum = new System.Decimal(new int[] {
                                                                                   1000,
                                                                                   0,
                                                                                   0,
                                                                                   -2147483648});
            this.minScaledNumericUpDown.Name = "minScaledNumericUpDown";
            this.minScaledNumericUpDown.Size = new System.Drawing.Size(112, 20);
            this.minScaledNumericUpDown.TabIndex = 5;
            this.minScaledNumericUpDown.Value = new System.Decimal(new int[] {
                                                                                 50,
                                                                                 0,
                                                                                 0,
                                                                                 -2147483648});
            // 
            // maxScaledLabel
            // 
            this.maxScaledLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.maxScaledLabel.Location = new System.Drawing.Point(12, 120);
            this.maxScaledLabel.Name = "maxScaledLabel";
            this.maxScaledLabel.Size = new System.Drawing.Size(68, 16);
            this.maxScaledLabel.TabIndex = 6;
            this.maxScaledLabel.Text = "Max (scaled):";
            // 
            // maxScaledNumericUpDown
            // 
            this.maxScaledNumericUpDown.DecimalPlaces = 2;
            this.maxScaledNumericUpDown.Location = new System.Drawing.Point(92, 112);
            this.maxScaledNumericUpDown.Maximum = new System.Decimal(new int[] {
                                                                                   1000,
                                                                                   0,
                                                                                   0,
                                                                                   0});
            this.maxScaledNumericUpDown.Minimum = new System.Decimal(new int[] {
                                                                                   1000,
                                                                                   0,
                                                                                   0,
                                                                                   -2147483648});
            this.maxScaledNumericUpDown.Name = "maxScaledNumericUpDown";
            this.maxScaledNumericUpDown.Size = new System.Drawing.Size(112, 20);
            this.maxScaledNumericUpDown.TabIndex = 7;
            this.maxScaledNumericUpDown.Value = new System.Decimal(new int[] {
                                                                                 50,
                                                                                 0,
                                                                                 0,
                                                                                 0});
            // 
            // minScaledLabel
            // 
            this.minScaledLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.minScaledLabel.Location = new System.Drawing.Point(12, 88);
            this.minScaledLabel.Name = "minScaledLabel";
            this.minScaledLabel.Size = new System.Drawing.Size(68, 16);
            this.minScaledLabel.TabIndex = 4;
            this.minScaledLabel.Text = "Min (scaled):";
            // 
            // wheatstoneBridgeParametersGroupBox
            // 
            this.wheatstoneBridgeParametersGroupBox.Controls.Add(this.bridgeConfigurationComboBox);
            this.wheatstoneBridgeParametersGroupBox.Controls.Add(this.bridgeConfigurationLabel);
            this.wheatstoneBridgeParametersGroupBox.Controls.Add(this.excitationSourceLabel);
            this.wheatstoneBridgeParametersGroupBox.Controls.Add(this.excitationSourceComboBox);
            this.wheatstoneBridgeParametersGroupBox.Controls.Add(this.excitationValueComboBox);
            this.wheatstoneBridgeParametersGroupBox.Controls.Add(this.excitationValueLabel);
            this.wheatstoneBridgeParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.wheatstoneBridgeParametersGroupBox.Location = new System.Drawing.Point(12, 240);
            this.wheatstoneBridgeParametersGroupBox.Name = "wheatstoneBridgeParametersGroupBox";
            this.wheatstoneBridgeParametersGroupBox.Size = new System.Drawing.Size(320, 112);
            this.wheatstoneBridgeParametersGroupBox.TabIndex = 2;
            this.wheatstoneBridgeParametersGroupBox.TabStop = false;
            this.wheatstoneBridgeParametersGroupBox.Text = "Wheatstone Bridge Parameters";
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
            this.bridgeConfigurationComboBox.Size = new System.Drawing.Size(176, 21);
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
            this.excitationSourceLabel.Location = new System.Drawing.Point(16, 56);
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
            this.excitationSourceComboBox.Location = new System.Drawing.Point(136, 51);
            this.excitationSourceComboBox.Name = "excitationSourceComboBox";
            this.excitationSourceComboBox.Size = new System.Drawing.Size(176, 21);
            this.excitationSourceComboBox.TabIndex = 3;
            // 
            // excitationValueComboBox
            // 
            this.excitationValueComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.excitationValueComboBox.Items.AddRange(new object[] {
                                                                         "2.5",
                                                                         "3.3",
                                                                         "5",
                                                                         "10"});
            this.excitationValueComboBox.Location = new System.Drawing.Point(136, 83);
            this.excitationValueComboBox.Name = "excitationValueComboBox";
            this.excitationValueComboBox.Size = new System.Drawing.Size(176, 21);
            this.excitationValueComboBox.TabIndex = 5;
            // 
            // excitationValueLabel
            // 
            this.excitationValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.excitationValueLabel.Location = new System.Drawing.Point(16, 88);
            this.excitationValueLabel.Name = "excitationValueLabel";
            this.excitationValueLabel.Size = new System.Drawing.Size(112, 16);
            this.excitationValueLabel.TabIndex = 4;
            this.excitationValueLabel.Text = "Excitation Value (V):";
            // 
            // acquisitionResultGroupBox
            // 
            this.acquisitionResultGroupBox.Controls.Add(this.acquisitionDataGrid);
            this.acquisitionResultGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.acquisitionResultGroupBox.Location = new System.Drawing.Point(576, 8);
            this.acquisitionResultGroupBox.Name = "acquisitionResultGroupBox";
            this.acquisitionResultGroupBox.Size = new System.Drawing.Size(200, 344);
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
            this.acquisitionDataGrid.Size = new System.Drawing.Size(184, 312);
            this.acquisitionDataGrid.TabIndex = 1;
            this.acquisitionDataGrid.TabStop = false;
            // 
            // bridgeNullCheckBox
            // 
            this.bridgeNullCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bridgeNullCheckBox.Location = new System.Drawing.Point(400, 264);
            this.bridgeNullCheckBox.Name = "bridgeNullCheckBox";
            this.bridgeNullCheckBox.Size = new System.Drawing.Size(128, 40);
            this.bridgeNullCheckBox.TabIndex = 4;
            this.bridgeNullCheckBox.Text = "Perform Bridge Null?";
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.stopButton.Location = new System.Drawing.Point(464, 328);
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
            this.startButton.Location = new System.Drawing.Point(368, 328);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(88, 24);
            this.startButton.TabIndex = 5;
            this.startButton.Text = "Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // channelParametersGroupBox
            // 
            this.channelParametersGroupBox.Controls.Add(this.physicalChannelComboBox);
            this.channelParametersGroupBox.Controls.Add(this.minimumValueNumericUpDown);
            this.channelParametersGroupBox.Controls.Add(this.maximumValueNumericUpDown);
            this.channelParametersGroupBox.Controls.Add(this.minimumValueLabel);
            this.channelParametersGroupBox.Controls.Add(this.maximumValueLabel);
            this.channelParametersGroupBox.Controls.Add(this.physicalChannelLabel);
            this.channelParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.channelParametersGroupBox.Location = new System.Drawing.Point(12, 8);
            this.channelParametersGroupBox.Name = "channelParametersGroupBox";
            this.channelParametersGroupBox.Size = new System.Drawing.Size(320, 128);
            this.channelParametersGroupBox.TabIndex = 0;
            this.channelParametersGroupBox.TabStop = false;
            this.channelParametersGroupBox.Text = "Channel Parameters";
            // 
            // physicalChannelComboBox
            // 
            this.physicalChannelComboBox.Location = new System.Drawing.Point(136, 19);
            this.physicalChannelComboBox.Name = "physicalChannelComboBox";
            this.physicalChannelComboBox.Size = new System.Drawing.Size(176, 21);
            this.physicalChannelComboBox.TabIndex = 1;
            // 
            // minimumValueNumericUpDown
            // 
            this.minimumValueNumericUpDown.DecimalPlaces = 3;
            this.minimumValueNumericUpDown.Location = new System.Drawing.Point(136, 96);
            this.minimumValueNumericUpDown.Maximum = new System.Decimal(new int[] {
                                                                                      10,
                                                                                      0,
                                                                                      0,
                                                                                      0});
            this.minimumValueNumericUpDown.Minimum = new System.Decimal(new int[] {
                                                                                      10,
                                                                                      0,
                                                                                      0,
                                                                                      -2147483648});
            this.minimumValueNumericUpDown.Name = "minimumValueNumericUpDown";
            this.minimumValueNumericUpDown.Size = new System.Drawing.Size(176, 20);
            this.minimumValueNumericUpDown.TabIndex = 5;
            this.minimumValueNumericUpDown.Value = new System.Decimal(new int[] {
                                                                                    25,
                                                                                    0,
                                                                                    0,
                                                                                    -2147287040});
            // 
            // maximumValueNumericUpDown
            // 
            this.maximumValueNumericUpDown.DecimalPlaces = 3;
            this.maximumValueNumericUpDown.Location = new System.Drawing.Point(136, 56);
            this.maximumValueNumericUpDown.Maximum = new System.Decimal(new int[] {
                                                                                      10,
                                                                                      0,
                                                                                      0,
                                                                                      0});
            this.maximumValueNumericUpDown.Minimum = new System.Decimal(new int[] {
                                                                                      10,
                                                                                      0,
                                                                                      0,
                                                                                      -2147483648});
            this.maximumValueNumericUpDown.Name = "maximumValueNumericUpDown";
            this.maximumValueNumericUpDown.Size = new System.Drawing.Size(176, 20);
            this.maximumValueNumericUpDown.TabIndex = 3;
            this.maximumValueNumericUpDown.Value = new System.Decimal(new int[] {
                                                                                    25,
                                                                                    0,
                                                                                    0,
                                                                                    196608});
            // 
            // minimumValueLabel
            // 
            this.minimumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.minimumValueLabel.Location = new System.Drawing.Point(16, 104);
            this.minimumValueLabel.Name = "minimumValueLabel";
            this.minimumValueLabel.Size = new System.Drawing.Size(100, 16);
            this.minimumValueLabel.TabIndex = 4;
            this.minimumValueLabel.Text = "Minimum Value:";
            // 
            // maximumValueLabel
            // 
            this.maximumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.maximumValueLabel.Location = new System.Drawing.Point(16, 64);
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
            this.timingParametersGroupBox.Controls.Add(this.rateNumericUpDown);
            this.timingParametersGroupBox.Controls.Add(this.samplesPerChannelLabel);
            this.timingParametersGroupBox.Controls.Add(this.rateLabel);
            this.timingParametersGroupBox.Controls.Add(this.samplesPerChannelNumericUpDown);
            this.timingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.timingParametersGroupBox.Location = new System.Drawing.Point(12, 144);
            this.timingParametersGroupBox.Name = "timingParametersGroupBox";
            this.timingParametersGroupBox.Size = new System.Drawing.Size(320, 88);
            this.timingParametersGroupBox.TabIndex = 1;
            this.timingParametersGroupBox.TabStop = false;
            this.timingParametersGroupBox.Text = "Timing Parameters";
            // 
            // rateNumericUpDown
            // 
            this.rateNumericUpDown.DecimalPlaces = 2;
            this.rateNumericUpDown.Location = new System.Drawing.Point(136, 56);
            this.rateNumericUpDown.Maximum = new System.Decimal(new int[] {
                                                                              100000,
                                                                              0,
                                                                              0,
                                                                              0});
            this.rateNumericUpDown.Name = "rateNumericUpDown";
            this.rateNumericUpDown.Size = new System.Drawing.Size(176, 20);
            this.rateNumericUpDown.TabIndex = 3;
            this.rateNumericUpDown.Value = new System.Decimal(new int[] {
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
            this.rateLabel.Location = new System.Drawing.Point(16, 56);
            this.rateLabel.Name = "rateLabel";
            this.rateLabel.Size = new System.Drawing.Size(96, 16);
            this.rateLabel.TabIndex = 2;
            this.rateLabel.Text = "Rate (Hz):";
            // 
            // samplesPerChannelNumericUpDown
            // 
            this.samplesPerChannelNumericUpDown.Location = new System.Drawing.Point(136, 17);
            this.samplesPerChannelNumericUpDown.Maximum = new System.Decimal(new int[] {
                                                                                           100000,
                                                                                           0,
                                                                                           0,
                                                                                           0});
            this.samplesPerChannelNumericUpDown.Name = "samplesPerChannelNumericUpDown";
            this.samplesPerChannelNumericUpDown.Size = new System.Drawing.Size(176, 20);
            this.samplesPerChannelNumericUpDown.TabIndex = 1;
            this.samplesPerChannelNumericUpDown.Value = new System.Decimal(new int[] {
                                                                                         5000,
                                                                                         0,
                                                                                         0,
                                                                                         0});
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(784, 366);
            this.Controls.Add(this.sensorScalingParametersGroupBox);
            this.Controls.Add(this.wheatstoneBridgeParametersGroupBox);
            this.Controls.Add(this.acquisitionResultGroupBox);
            this.Controls.Add(this.bridgeNullCheckBox);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.channelParametersGroupBox);
            this.Controls.Add(this.timingParametersGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Acquire Wheatstone Bridge Strain Samples - 9237";
            this.sensorScalingParametersGroupBox.ResumeLayout(false);
            this.scaleTabControl.ResumeLayout(false);
            this.linearTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bNumericUpDown)).EndInit();
            this.mapRangesTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.minNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minScaledNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxScaledNumericUpDown)).EndInit();
            this.wheatstoneBridgeParametersGroupBox.ResumeLayout(false);
            this.acquisitionResultGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.acquisitionDataGrid)).EndInit();
            this.channelParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.minimumValueNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maximumValueNumericUpDown)).EndInit();
            this.timingParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rateNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerChannelNumericUpDown)).EndInit();
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
                // Create the custom scale based on the user selection, either LinearScale or RangeMapScale.
                if (scaleTabControl.SelectedIndex == 0)
                {
                    customScale = new LinearScale(customScaleName, Convert.ToDouble(mNumericUpDown.Value), Convert.ToDouble(bNumericUpDown.Value));
                }
                else
                {
                    customScale = new RangeMapScale(customScaleName, Convert.ToDouble(minNumericUpDown.Value), Convert.ToDouble(maxNumericUpDown.Value),
                                                                  Convert.ToDouble(minScaledNumericUpDown.Value), Convert.ToDouble(maxScaledNumericUpDown.Value));
                }

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

                // Create and configure AI channel
                myTask = new Task();
                AIChannel myAIChannel = myTask.AIChannels.CreateVoltageChannelWithExcitation(physicalChannelComboBox.Text, "", AITerminalConfiguration.Differential, Convert.ToDouble(minimumValueNumericUpDown.Value),
                                        Convert.ToDouble(maximumValueNumericUpDown.Value), bridgeConfiguration, excitationSource, Convert.ToDouble(excitationValueComboBox.Text), true, customScale.Name);

                // Configure the sample clock
                myTask.Timing.ConfigureSampleClock("", Convert.ToDouble(rateNumericUpDown.Value), SampleClockActiveEdge.Rising, SampleQuantityMode.ContinuousSamples);

                // Verify task
                myTask.Control(TaskAction.Verify);

                if (bridgeNullCheckBox.Checked)
                    myAIChannel.PerformBridgeOffsetNullingCalibration();

                InitializeDataTable(ref dataTable);
                acquisitionDataGrid.DataSource = dataTable;

                runningTask = myTask;

                reader = new AnalogMultiChannelReader(myTask.Stream);

                callback = new AsyncCallback(AnalogCallback);

                myTask.Start();

                // Use SynchronizeCallbacks to specify that the object 
                // marshals callbacks across threads appropriately.
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
