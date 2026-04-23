/******************************************************************************
*
* Example program:
*   ContAcqRTDSamples_IntClk
*
* Category:
*   AI
*
* Description:
*   This example demonstrates how to acquire temperature from an RTD using the
*   internal clock of the DAQ device.
*
* Instructions for running:
*   1.  Select the physical channel corresponding to where your signal is input
*       on the DAQ device.
*   2.  Enter the minimum and maximum temperature values.Note: For better
*       accuracy, try to match the input ranges to the expected temperature
*       level of the measured signal.
*   3.  Enter the acquisition rate.
*   4.  Enter the RTD Type and r0 (resistance at 0 degrees C).Note: If you
*       select "Custom" as your RTD type, you need to modify this example in
*       order to provide the A, B, and C coefficients of the Callendar-Van Dusen
*       equation. The coefficients are specified using the AIChannel object.
*   5.  Enter the resistance configuration, the current excitation source, and
*       the excitation value in Amps.
*
* Steps:
*   1.  Create a new Task object. Create a AIChannel object by using the
*       CreateRtdChannel method.
*   2.  Set the rate for the sample clock by using the
*       Timing.ConfigureSampleClock method. Additionally, define the sample mode
*       to be continuous.
*   3.  Call AnalogMultiChannelReader.BeginReadWaveform to install a callback
*       and begin the asynchronous read operation.
*   4.  Inside the callback, call AnalogMultiChannelReader.EndReadWaveform to
*       retrieve the data from the read operation.  
*   5.  Call AnalogMultiChannelReader.BeginMemoryOptimizedReadWaveform
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
*   Make sure your signal input terminals match the physical channel text box. 
*   For more information on the input and output terminals for your device, open
*   the NI-DAQmx Help, and refer to the NI-DAQmx Device Terminals and Device
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

namespace NationalInstruments.Examples.ContAcqRTDSamples_IntClk
{
    /// <summary>
    /// Summary description for MainForm.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        private AnalogMultiChannelReader analogInReader;
        private AsyncCallback myAsyncCallback;
        private AnalogWaveform<double>[] data;
        private DataColumn[] dataColumn = null;
        private DataTable dataTable = null;
        private Task myTask;
        private Task runningTask;
        internal System.Windows.Forms.GroupBox resistanceParametersGroupBox;
        internal System.Windows.Forms.Label currentValueLabel;
        internal System.Windows.Forms.Label currentExcitationLabel;
        internal System.Windows.Forms.Label resistanceConfigurationLabel;
        internal System.Windows.Forms.ComboBox currentExcitationComboBox;
        internal System.Windows.Forms.ComboBox resistanceConfigurationComboBox;
        internal System.Windows.Forms.Button stopButton;
        internal System.Windows.Forms.GroupBox timingParametersGroupBox;
        internal System.Windows.Forms.Label rateLabel;
        internal System.Windows.Forms.Button startButton;
        internal System.Windows.Forms.GroupBox channelParametersGroupBox;
        internal System.Windows.Forms.Label maximumLabel;
        internal System.Windows.Forms.Label minimumLabel;
        internal System.Windows.Forms.Label physicalChannelLabel;
        internal System.Windows.Forms.GroupBox acquisitionResultsGroupBox;
        internal System.Windows.Forms.Label resultLabel;
        internal System.Windows.Forms.DataGrid acquisitionDataGrid;
        internal System.Windows.Forms.GroupBox rtdParametersGroupBox;
        internal System.Windows.Forms.Label r0Label;
        internal System.Windows.Forms.ComboBox rtdTypeComboBox;
        internal System.Windows.Forms.Label rtdTypeLabel;
        private System.Windows.Forms.NumericUpDown rateNumeric;
        private System.Windows.Forms.NumericUpDown r0Numeric;
        private System.Windows.Forms.NumericUpDown currentExcitationNumeric;
        private System.Windows.Forms.NumericUpDown maximumValueNumeric;
        private System.Windows.Forms.NumericUpDown minimumValueNumeric;
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
            rtdTypeComboBox.SelectedIndex = 2;
            resistanceConfigurationComboBox.SelectedIndex = 2;
            currentExcitationComboBox.SelectedIndex = 1;
            myAsyncCallback = new AsyncCallback(AnalogInCallback);
            dataTable = new DataTable();

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
            this.rtdParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.r0Numeric = new System.Windows.Forms.NumericUpDown();
            this.r0Label = new System.Windows.Forms.Label();
            this.rtdTypeComboBox = new System.Windows.Forms.ComboBox();
            this.rtdTypeLabel = new System.Windows.Forms.Label();
            this.resistanceParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.currentExcitationNumeric = new System.Windows.Forms.NumericUpDown();
            this.currentValueLabel = new System.Windows.Forms.Label();
            this.currentExcitationLabel = new System.Windows.Forms.Label();
            this.resistanceConfigurationLabel = new System.Windows.Forms.Label();
            this.currentExcitationComboBox = new System.Windows.Forms.ComboBox();
            this.resistanceConfigurationComboBox = new System.Windows.Forms.ComboBox();
            this.stopButton = new System.Windows.Forms.Button();
            this.timingParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.rateNumeric = new System.Windows.Forms.NumericUpDown();
            this.rateLabel = new System.Windows.Forms.Label();
            this.startButton = new System.Windows.Forms.Button();
            this.channelParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.physicalChannelComboBox = new System.Windows.Forms.ComboBox();
            this.minimumValueNumeric = new System.Windows.Forms.NumericUpDown();
            this.maximumValueNumeric = new System.Windows.Forms.NumericUpDown();
            this.maximumLabel = new System.Windows.Forms.Label();
            this.minimumLabel = new System.Windows.Forms.Label();
            this.physicalChannelLabel = new System.Windows.Forms.Label();
            this.acquisitionResultsGroupBox = new System.Windows.Forms.GroupBox();
            this.resultLabel = new System.Windows.Forms.Label();
            this.acquisitionDataGrid = new System.Windows.Forms.DataGrid();
            this.rtdParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.r0Numeric)).BeginInit();
            this.resistanceParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.currentExcitationNumeric)).BeginInit();
            this.timingParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rateNumeric)).BeginInit();
            this.channelParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.minimumValueNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maximumValueNumeric)).BeginInit();
            this.acquisitionResultsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.acquisitionDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // rtdParametersGroupBox
            // 
            this.rtdParametersGroupBox.Controls.Add(this.r0Numeric);
            this.rtdParametersGroupBox.Controls.Add(this.r0Label);
            this.rtdParametersGroupBox.Controls.Add(this.rtdTypeComboBox);
            this.rtdParametersGroupBox.Controls.Add(this.rtdTypeLabel);
            this.rtdParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rtdParametersGroupBox.Location = new System.Drawing.Point(8, 224);
            this.rtdParametersGroupBox.Name = "rtdParametersGroupBox";
            this.rtdParametersGroupBox.Size = new System.Drawing.Size(232, 88);
            this.rtdParametersGroupBox.TabIndex = 4;
            this.rtdParametersGroupBox.TabStop = false;
            this.rtdParametersGroupBox.Text = "RTD Parameters";
            // 
            // r0Numeric
            // 
            this.r0Numeric.DecimalPlaces = 2;
            this.r0Numeric.Location = new System.Drawing.Point(128, 56);
            this.r0Numeric.Maximum = new System.Decimal(new int[] {
                                                                      1000000,
                                                                      0,
                                                                      0,
                                                                      0});
            this.r0Numeric.Name = "r0Numeric";
            this.r0Numeric.Size = new System.Drawing.Size(96, 20);
            this.r0Numeric.TabIndex = 3;
            this.r0Numeric.Value = new System.Decimal(new int[] {
                                                                    100,
                                                                    0,
                                                                    0,
                                                                    0});
            // 
            // r0Label
            // 
            this.r0Label.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.r0Label.Location = new System.Drawing.Point(16, 58);
            this.r0Label.Name = "r0Label";
            this.r0Label.Size = new System.Drawing.Size(72, 16);
            this.r0Label.TabIndex = 2;
            this.r0Label.Text = "R0 (Ohms):";
            // 
            // rtdTypeComboBox
            // 
            this.rtdTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.rtdTypeComboBox.Items.AddRange(new object[] {
                                                                 "Custom",
                                                                 "Pt3750",
                                                                 "Pt3851",
                                                                 "Pt3911",
                                                                 "Pt3916",
                                                                 "Pt3920",
                                                                 "Pt3928"});
            this.rtdTypeComboBox.Location = new System.Drawing.Point(128, 24);
            this.rtdTypeComboBox.Name = "rtdTypeComboBox";
            this.rtdTypeComboBox.Size = new System.Drawing.Size(96, 21);
            this.rtdTypeComboBox.TabIndex = 1;
            // 
            // rtdTypeLabel
            // 
            this.rtdTypeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rtdTypeLabel.Location = new System.Drawing.Point(16, 26);
            this.rtdTypeLabel.Name = "rtdTypeLabel";
            this.rtdTypeLabel.Size = new System.Drawing.Size(72, 16);
            this.rtdTypeLabel.TabIndex = 0;
            this.rtdTypeLabel.Text = "RTD Type:";
            // 
            // resistanceParametersGroupBox
            // 
            this.resistanceParametersGroupBox.Controls.Add(this.currentExcitationNumeric);
            this.resistanceParametersGroupBox.Controls.Add(this.currentValueLabel);
            this.resistanceParametersGroupBox.Controls.Add(this.currentExcitationLabel);
            this.resistanceParametersGroupBox.Controls.Add(this.resistanceConfigurationLabel);
            this.resistanceParametersGroupBox.Controls.Add(this.currentExcitationComboBox);
            this.resistanceParametersGroupBox.Controls.Add(this.resistanceConfigurationComboBox);
            this.resistanceParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.resistanceParametersGroupBox.Location = new System.Drawing.Point(248, 8);
            this.resistanceParametersGroupBox.Name = "resistanceParametersGroupBox";
            this.resistanceParametersGroupBox.Size = new System.Drawing.Size(256, 120);
            this.resistanceParametersGroupBox.TabIndex = 5;
            this.resistanceParametersGroupBox.TabStop = false;
            this.resistanceParametersGroupBox.Text = "Resistance Parameters";
            // 
            // currentExcitationNumeric
            // 
            this.currentExcitationNumeric.DecimalPlaces = 5;
            this.currentExcitationNumeric.Location = new System.Drawing.Point(160, 88);
            this.currentExcitationNumeric.Maximum = new System.Decimal(new int[] {
                                                                                     1000000,
                                                                                     0,
                                                                                     0,
                                                                                     0});
            this.currentExcitationNumeric.Name = "currentExcitationNumeric";
            this.currentExcitationNumeric.Size = new System.Drawing.Size(88, 20);
            this.currentExcitationNumeric.TabIndex = 5;
            this.currentExcitationNumeric.Value = new System.Decimal(new int[] {
                                                                                   15,
                                                                                   0,
                                                                                   0,
                                                                                   327680});
            // 
            // currentValueLabel
            // 
            this.currentValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.currentValueLabel.Location = new System.Drawing.Point(16, 91);
            this.currentValueLabel.Name = "currentValueLabel";
            this.currentValueLabel.Size = new System.Drawing.Size(152, 14);
            this.currentValueLabel.TabIndex = 4;
            this.currentValueLabel.Text = "Current Excitation Value (A):";
            // 
            // currentExcitationLabel
            // 
            this.currentExcitationLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.currentExcitationLabel.Location = new System.Drawing.Point(16, 59);
            this.currentExcitationLabel.Name = "currentExcitationLabel";
            this.currentExcitationLabel.Size = new System.Drawing.Size(136, 14);
            this.currentExcitationLabel.TabIndex = 2;
            this.currentExcitationLabel.Text = "Current Excitation Source:";
            // 
            // resistanceConfigurationLabel
            // 
            this.resistanceConfigurationLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.resistanceConfigurationLabel.Location = new System.Drawing.Point(16, 27);
            this.resistanceConfigurationLabel.Name = "resistanceConfigurationLabel";
            this.resistanceConfigurationLabel.Size = new System.Drawing.Size(136, 14);
            this.resistanceConfigurationLabel.TabIndex = 0;
            this.resistanceConfigurationLabel.Text = "Resistance Configuration:";
            // 
            // currentExcitationComboBox
            // 
            this.currentExcitationComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.currentExcitationComboBox.Items.AddRange(new object[] {
                                                                           "None",
                                                                           "Internal",
                                                                           "External"});
            this.currentExcitationComboBox.Location = new System.Drawing.Point(160, 56);
            this.currentExcitationComboBox.Name = "currentExcitationComboBox";
            this.currentExcitationComboBox.Size = new System.Drawing.Size(88, 21);
            this.currentExcitationComboBox.TabIndex = 3;
            // 
            // resistanceConfigurationComboBox
            // 
            this.resistanceConfigurationComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.resistanceConfigurationComboBox.Items.AddRange(new object[] {
                                                                                 "2-Wire",
                                                                                 "3-Wire",
                                                                                 "4-Wire"});
            this.resistanceConfigurationComboBox.Location = new System.Drawing.Point(160, 24);
            this.resistanceConfigurationComboBox.Name = "resistanceConfigurationComboBox";
            this.resistanceConfigurationComboBox.Size = new System.Drawing.Size(88, 21);
            this.resistanceConfigurationComboBox.TabIndex = 1;
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.stopButton.Location = new System.Drawing.Point(136, 344);
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
            this.timingParametersGroupBox.Size = new System.Drawing.Size(232, 64);
            this.timingParametersGroupBox.TabIndex = 3;
            this.timingParametersGroupBox.TabStop = false;
            this.timingParametersGroupBox.Text = "Timing Parameters";
            // 
            // rateNumeric
            // 
            this.rateNumeric.DecimalPlaces = 2;
            this.rateNumeric.Location = new System.Drawing.Point(128, 24);
            this.rateNumeric.Maximum = new System.Decimal(new int[] {
                                                                        1000000,
                                                                        0,
                                                                        0,
                                                                        0});
            this.rateNumeric.Name = "rateNumeric";
            this.rateNumeric.Size = new System.Drawing.Size(96, 20);
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
            // startButton
            // 
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.startButton.Location = new System.Drawing.Point(24, 344);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(80, 24);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
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
            this.channelParametersGroupBox.TabIndex = 2;
            this.channelParametersGroupBox.TabStop = false;
            this.channelParametersGroupBox.Text = "Channel Parameters";
            // 
            // physicalChannelComboBox
            // 
            this.physicalChannelComboBox.Location = new System.Drawing.Point(128, 24);
            this.physicalChannelComboBox.Name = "physicalChannelComboBox";
            this.physicalChannelComboBox.Size = new System.Drawing.Size(96, 21);
            this.physicalChannelComboBox.TabIndex = 1;
            this.physicalChannelComboBox.Text = "SC1Mod1/ai0";
            // 
            // minimumValueNumeric
            // 
            this.minimumValueNumeric.DecimalPlaces = 2;
            this.minimumValueNumeric.Location = new System.Drawing.Point(128, 88);
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
            this.minimumValueNumeric.Size = new System.Drawing.Size(96, 20);
            this.minimumValueNumeric.TabIndex = 5;
            // 
            // maximumValueNumeric
            // 
            this.maximumValueNumeric.DecimalPlaces = 2;
            this.maximumValueNumeric.Location = new System.Drawing.Point(128, 56);
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
            this.maximumValueNumeric.Size = new System.Drawing.Size(96, 20);
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
            this.maximumLabel.Location = new System.Drawing.Point(16, 59);
            this.maximumLabel.Name = "maximumLabel";
            this.maximumLabel.Size = new System.Drawing.Size(104, 14);
            this.maximumLabel.TabIndex = 2;
            this.maximumLabel.Text = "Maximum (deg C):";
            // 
            // minimumLabel
            // 
            this.minimumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.minimumLabel.Location = new System.Drawing.Point(16, 91);
            this.minimumLabel.Name = "minimumLabel";
            this.minimumLabel.Size = new System.Drawing.Size(96, 14);
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
            // acquisitionResultsGroupBox
            // 
            this.acquisitionResultsGroupBox.Controls.Add(this.resultLabel);
            this.acquisitionResultsGroupBox.Controls.Add(this.acquisitionDataGrid);
            this.acquisitionResultsGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.acquisitionResultsGroupBox.Location = new System.Drawing.Point(248, 144);
            this.acquisitionResultsGroupBox.Name = "acquisitionResultsGroupBox";
            this.acquisitionResultsGroupBox.Size = new System.Drawing.Size(256, 224);
            this.acquisitionResultsGroupBox.TabIndex = 6;
            this.acquisitionResultsGroupBox.TabStop = false;
            this.acquisitionResultsGroupBox.Text = "Acquisition Results:";
            // 
            // resultLabel
            // 
            this.resultLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.resultLabel.Location = new System.Drawing.Point(16, 16);
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
            this.acquisitionDataGrid.Size = new System.Drawing.Size(232, 176);
            this.acquisitionDataGrid.TabIndex = 1;
            this.acquisitionDataGrid.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(514, 376);
            this.Controls.Add(this.rtdParametersGroupBox);
            this.Controls.Add(this.resistanceParametersGroupBox);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.timingParametersGroupBox);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.channelParametersGroupBox);
            this.Controls.Add(this.acquisitionResultsGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Continuous Acquire RTD Samples - Internal Clock";
            this.rtdParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.r0Numeric)).EndInit();
            this.resistanceParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.currentExcitationNumeric)).EndInit();
            this.timingParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rateNumeric)).EndInit();
            this.channelParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.minimumValueNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maximumValueNumeric)).EndInit();
            this.acquisitionResultsGroupBox.ResumeLayout(false);
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
            stopButton.Enabled = true;

            try
            {
                myTask = new Task();
                AIRtdType rtdType;
                AIResistanceConfiguration resistanceConfiguration;
                AIExcitationSource excitationSource;

                switch (rtdTypeComboBox.SelectedIndex)
                {
                    case 0:
                        rtdType = AIRtdType.Custom;
                        break;
                    case 1:
                        rtdType = AIRtdType.Pt3750;
                        break;
                    case 2:
                        rtdType = AIRtdType.Pt3851;
                        break;
                    case 3:
                        rtdType = AIRtdType.Pt3911;
                        break;
                    case 4:
                        rtdType = AIRtdType.Pt3916;
                        break;
                    case 5:
                        rtdType = AIRtdType.Pt3920;
                        break;
                    case 6:
                    default:
                        rtdType = AIRtdType.Pt3928;
                        break;
                }

                switch (resistanceConfigurationComboBox.SelectedIndex)
                {
                    case 0:
                        resistanceConfiguration = AIResistanceConfiguration.TwoWire;
                        break;
                    case 1:
                        resistanceConfiguration = AIResistanceConfiguration.ThreeWire;
                        break;
                    case 2:
                    default:
                        resistanceConfiguration = AIResistanceConfiguration.FourWire;
                        break;
                }

                switch (currentExcitationComboBox.SelectedIndex)
                {
                    case 0:
                        excitationSource = AIExcitationSource.None;
                        break;
                    case 1:
                        excitationSource = AIExcitationSource.Internal;
                        break;
                    case 2:
                    default:
                        excitationSource = AIExcitationSource.External;
                        break;
                }

                myTask.AIChannels.CreateRtdChannel(physicalChannelComboBox.Text, "",
                    Convert.ToDouble(minimumValueNumeric.Value), Convert.ToDouble(maximumValueNumeric.Value),
                    AITemperatureUnits.DegreesC, rtdType, resistanceConfiguration, excitationSource,
                    Convert.ToDouble(currentExcitationNumeric.Value), Convert.ToDouble(r0Numeric.Value));

                myTask.Timing.ConfigureSampleClock("", Convert.ToDouble(rateNumeric.Value),
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
            catch (DaqException ex)
            {
                MessageBox.Show(ex.Message);
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

        private void InitializeDataTable(AIChannelCollection channelCollection, ref DataTable data)
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
            for (int currentDataIndex = 0; currentDataIndex < numOfRows; currentDataIndex++)
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
