/******************************************************************************
*
* Example program:
*   ContAcqVoltageSamples_IntClk_SWTrigger
*
* Category:
*   AI
*
* Description:
*   This example demonstrates how to perform an analog software triggered
*   acquisition.  The example allows the user to specify the triggering
*   condition and the number of pre-trigger samples to acquire.
*
* Instructions for running:
*   1.  Select the physical channels corresponding to where your signals are
*       input on the DAQ device.
*   2.  Enter the minimum and maximum voltage ranges.NOTE:  For better accuracy,
*       try to match the input ranges to the expected voltage levels of the
*       measured signals.
*   3.  Specify the rate of the sample clock in Hz.
*   4.  Specify the number of samples to read per iteration of the continuous
*       input operation.  These samples will then be monitored for the trigger
*       condition.
*   5.  Specify the index of the channel that will be monitored for the
*       requested condition.  The first channel in the scan list will have an
*       index equal to zero.
*   6.  Set the triggering condition to be applied to the channel specified by
*       the channel index.  
*       The trigger conditions for this example are:   - Above Level:  The trigger
*       condition is met as soon as 
*       the monitored signal is above the specified level.    - Below Level: The
*       trigger condition is met as 
*       soon as the monitored signal is below the specified level.   - Rising
*       Edge: The trigger condition is 
*       met as soon as the monitored signal starts below the hysteresis window and
*       then rises past the specified 
*       level.   - Falling Edge: The trigger condition is met as soon as the
*       monitored signal starts above the 
*       hysteresis window and then falls below the specified level.   - Inside
*       Window: The trigger condition 
*       is met as soon as the monitored signal is within the specified window.   -
*       Outside Window: The trigger 
*       condition is met as soon as the monitored signal is outside of the
*       specified window.
*   7.  Set the level value to be used by the selected condition.
*   8.  Set the window amplitude / hysteresis to be used by the trigger
*       conditions:   - The upper limit of the conditional window is calculated
*       by adding the window amplitude to the condition level.   - The lower
*       limit of the conditional window is calculated by subtracting the window
*       amplitude from the condition level.   - The hysteresis window for the
*       Rising Edge condition is defined as the level minus the hysteresis
*       value.   - The hysteresis window for the Falling Edge condition is
*       defined as the level plus the hysteresis value.
*   9.  Set the number of pre-trigger samples.  The first voltage samples
*       displayed will be for these pre-trigger values, or zeroes if not enough
*       samples are read before the trigger fires to fill up the pre-trigger
*       samples buffer.
*
* Steps:
*   1.  Create a new analog input task.
*   2.  Create the analog input voltage channels.
*   3.  Configure the timing for the acquisition.  In this example we use the
*       DAQ device's internal clock to take a continuous number of samples.
*   4.  Verify the task.
*   5.  Prepare the number of pretrigger samples to read.
*   6.  Create a AnalogMultiChannelReader and associate it with the task by
*       using the task's stream. Call
*       AnalogMultiChannelReader.BeginBeginReadMultiSample to install a callback
*       and begin the asynchronous read operation.
*   7.  Inside the callback, analyze the data for the trigger condition.  If one
*       exists, display the pretrigger samples and the post-trigger samples. 
*       Otherwise, call AnalogMultiChannelReader.BeginReadMultiSample to perform
*       another read.
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
*   Make sure your signal input terminals match the physical I/O control.  In
*   the default case (differential channel ai0), wire the positive lead for your
*   signal to the ACH0 pin on your DAQ device and wire the negative lead for
*   your signal to the ACH8 pin.  For more information on the input and output
*   terminals for your device, open the NI-DAQmx Help and refer to the NI-DAQmx
*   Device Terminals and Device Considerations books in the table of contents.
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

namespace NationalInstruments.Examples.ContAcqVoltageSamples_IntClk_SWTrigger
{
    /// <summary>
    /// Summary description for MainForm.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        private Task myTask;
        private AnalogMultiChannelReader reader;
        private DataColumn[] dataColumn = null;
        private DataTable dataTable = null;
        private double[,] pretrigger;
        private int ptSize;
        private int samples;
        private int pretriggerSamples;
        private int ptSaved;

        private Task runningTask;

        private System.Windows.Forms.GroupBox channelGroupBox;
        private System.Windows.Forms.GroupBox timingGroupBox;
        private System.Windows.Forms.Label physicalChannelLabel;
        private System.Windows.Forms.ComboBox physicalChannelComboBox;
        private System.Windows.Forms.Label maximumLabel;
        private System.Windows.Forms.NumericUpDown maximumNumeric;
        private System.Windows.Forms.Label minumumLabel;
        private System.Windows.Forms.NumericUpDown minimumNumeric;
        private System.Windows.Forms.Label samplesLabel;
        private System.Windows.Forms.Label rateLabel;
        private System.Windows.Forms.NumericUpDown samplesNumeric;
        private System.Windows.Forms.NumericUpDown rateNumeric;
        private System.Windows.Forms.NumericUpDown indexNumeric;
        private System.Windows.Forms.Label indexLabel;
        private System.Windows.Forms.Label conditionLabel;
        private System.Windows.Forms.ComboBox conditionComboBox;
        private System.Windows.Forms.Label windowLabel;
        private System.Windows.Forms.Label levelLabel;
        private System.Windows.Forms.NumericUpDown windowNumeric;
        private System.Windows.Forms.NumericUpDown levelNumeric;
        private System.Windows.Forms.Label pretriggerLabel;
        private System.Windows.Forms.NumericUpDown pretriggerNumeric;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.GroupBox acquiredGroupBox;
        private System.Windows.Forms.DataGrid resultsDataGrid;
        private System.Windows.Forms.GroupBox triggeringGroupBox;
        private System.Windows.Forms.NumericUpDown pretriggerAcquiredNumeric;
        private System.Windows.Forms.Label pretriggerAcquiredLabel;
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
            physicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AI, PhysicalChannelAccess.External));
            if (physicalChannelComboBox.Items.Count > 0)
                physicalChannelComboBox.SelectedIndex = 0;

            conditionComboBox.SelectedIndex = 0;

            dataTable = new DataTable();
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
            this.channelGroupBox = new System.Windows.Forms.GroupBox();
            this.maximumNumeric = new System.Windows.Forms.NumericUpDown();
            this.physicalChannelLabel = new System.Windows.Forms.Label();
            this.physicalChannelComboBox = new System.Windows.Forms.ComboBox();
            this.maximumLabel = new System.Windows.Forms.Label();
            this.minumumLabel = new System.Windows.Forms.Label();
            this.minimumNumeric = new System.Windows.Forms.NumericUpDown();
            this.timingGroupBox = new System.Windows.Forms.GroupBox();
            this.samplesLabel = new System.Windows.Forms.Label();
            this.rateLabel = new System.Windows.Forms.Label();
            this.samplesNumeric = new System.Windows.Forms.NumericUpDown();
            this.rateNumeric = new System.Windows.Forms.NumericUpDown();
            this.triggeringGroupBox = new System.Windows.Forms.GroupBox();
            this.indexNumeric = new System.Windows.Forms.NumericUpDown();
            this.indexLabel = new System.Windows.Forms.Label();
            this.conditionLabel = new System.Windows.Forms.Label();
            this.conditionComboBox = new System.Windows.Forms.ComboBox();
            this.windowLabel = new System.Windows.Forms.Label();
            this.levelLabel = new System.Windows.Forms.Label();
            this.windowNumeric = new System.Windows.Forms.NumericUpDown();
            this.levelNumeric = new System.Windows.Forms.NumericUpDown();
            this.pretriggerLabel = new System.Windows.Forms.Label();
            this.pretriggerNumeric = new System.Windows.Forms.NumericUpDown();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.acquiredGroupBox = new System.Windows.Forms.GroupBox();
            this.resultsDataGrid = new System.Windows.Forms.DataGrid();
            this.pretriggerAcquiredNumeric = new System.Windows.Forms.NumericUpDown();
            this.pretriggerAcquiredLabel = new System.Windows.Forms.Label();
            this.channelGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maximumNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minimumNumeric)).BeginInit();
            this.timingGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.samplesNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rateNumeric)).BeginInit();
            this.triggeringGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.indexNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.windowNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.levelNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pretriggerNumeric)).BeginInit();
            this.acquiredGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resultsDataGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pretriggerAcquiredNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // channelGroupBox
            // 
            this.channelGroupBox.Controls.Add(this.maximumNumeric);
            this.channelGroupBox.Controls.Add(this.physicalChannelLabel);
            this.channelGroupBox.Controls.Add(this.physicalChannelComboBox);
            this.channelGroupBox.Controls.Add(this.maximumLabel);
            this.channelGroupBox.Controls.Add(this.minumumLabel);
            this.channelGroupBox.Controls.Add(this.minimumNumeric);
            this.channelGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.channelGroupBox.Location = new System.Drawing.Point(8, 8);
            this.channelGroupBox.Name = "channelGroupBox";
            this.channelGroupBox.Size = new System.Drawing.Size(272, 120);
            this.channelGroupBox.TabIndex = 0;
            this.channelGroupBox.TabStop = false;
            this.channelGroupBox.Text = "Channel Parameters";
            // 
            // maximumNumeric
            // 
            this.maximumNumeric.DecimalPlaces = 2;
            this.maximumNumeric.Location = new System.Drawing.Point(128, 57);
            this.maximumNumeric.Minimum = new System.Decimal(new int[] {
                                                                           100,
                                                                           0,
                                                                           0,
                                                                           -2147483648});
            this.maximumNumeric.Name = "maximumNumeric";
            this.maximumNumeric.TabIndex = 3;
            this.maximumNumeric.Value = new System.Decimal(new int[] {
                                                                         10,
                                                                         0,
                                                                         0,
                                                                         0});
            // 
            // physicalChannelLabel
            // 
            this.physicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.physicalChannelLabel.Location = new System.Drawing.Point(8, 27);
            this.physicalChannelLabel.Name = "physicalChannelLabel";
            this.physicalChannelLabel.Size = new System.Drawing.Size(104, 16);
            this.physicalChannelLabel.TabIndex = 0;
            this.physicalChannelLabel.Text = "Physical Channels:";
            // 
            // physicalChannelComboBox
            // 
            this.physicalChannelComboBox.Location = new System.Drawing.Point(128, 25);
            this.physicalChannelComboBox.Name = "physicalChannelComboBox";
            this.physicalChannelComboBox.Size = new System.Drawing.Size(120, 21);
            this.physicalChannelComboBox.TabIndex = 1;
            this.physicalChannelComboBox.Text = "Dev1/ai0";
            // 
            // maximumLabel
            // 
            this.maximumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.maximumLabel.Location = new System.Drawing.Point(8, 59);
            this.maximumLabel.Name = "maximumLabel";
            this.maximumLabel.Size = new System.Drawing.Size(112, 16);
            this.maximumLabel.TabIndex = 2;
            this.maximumLabel.Text = "Maximum Value (V):";
            // 
            // minumumLabel
            // 
            this.minumumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.minumumLabel.Location = new System.Drawing.Point(8, 90);
            this.minumumLabel.Name = "minumumLabel";
            this.minumumLabel.Size = new System.Drawing.Size(112, 16);
            this.minumumLabel.TabIndex = 4;
            this.minumumLabel.Text = "Minimum Value (V):";
            // 
            // minimumNumeric
            // 
            this.minimumNumeric.DecimalPlaces = 2;
            this.minimumNumeric.Location = new System.Drawing.Point(128, 88);
            this.minimumNumeric.Minimum = new System.Decimal(new int[] {
                                                                           100,
                                                                           0,
                                                                           0,
                                                                           -2147483648});
            this.minimumNumeric.Name = "minimumNumeric";
            this.minimumNumeric.TabIndex = 5;
            this.minimumNumeric.Value = new System.Decimal(new int[] {
                                                                         10,
                                                                         0,
                                                                         0,
                                                                         -2147483648});
            // 
            // timingGroupBox
            // 
            this.timingGroupBox.Controls.Add(this.samplesLabel);
            this.timingGroupBox.Controls.Add(this.rateLabel);
            this.timingGroupBox.Controls.Add(this.samplesNumeric);
            this.timingGroupBox.Controls.Add(this.rateNumeric);
            this.timingGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.timingGroupBox.Location = new System.Drawing.Point(8, 136);
            this.timingGroupBox.Name = "timingGroupBox";
            this.timingGroupBox.Size = new System.Drawing.Size(272, 88);
            this.timingGroupBox.TabIndex = 1;
            this.timingGroupBox.TabStop = false;
            this.timingGroupBox.Text = "Timing Parameters";
            // 
            // samplesLabel
            // 
            this.samplesLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.samplesLabel.Location = new System.Drawing.Point(8, 59);
            this.samplesLabel.Name = "samplesLabel";
            this.samplesLabel.Size = new System.Drawing.Size(120, 16);
            this.samplesLabel.TabIndex = 2;
            this.samplesLabel.Text = "Samples per Channel:";
            // 
            // rateLabel
            // 
            this.rateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rateLabel.Location = new System.Drawing.Point(8, 27);
            this.rateLabel.Name = "rateLabel";
            this.rateLabel.Size = new System.Drawing.Size(112, 16);
            this.rateLabel.TabIndex = 0;
            this.rateLabel.Text = "Sample Rate (Hz):";
            // 
            // samplesNumeric
            // 
            this.samplesNumeric.Location = new System.Drawing.Point(128, 57);
            this.samplesNumeric.Maximum = new System.Decimal(new int[] {
                                                                           1410065408,
                                                                           2,
                                                                           0,
                                                                           0});
            this.samplesNumeric.Minimum = new System.Decimal(new int[] {
                                                                           1,
                                                                           0,
                                                                           0,
                                                                           0});
            this.samplesNumeric.Name = "samplesNumeric";
            this.samplesNumeric.TabIndex = 3;
            this.samplesNumeric.Value = new System.Decimal(new int[] {
                                                                         100,
                                                                         0,
                                                                         0,
                                                                         0});
            // 
            // rateNumeric
            // 
            this.rateNumeric.DecimalPlaces = 2;
            this.rateNumeric.Location = new System.Drawing.Point(128, 25);
            this.rateNumeric.Maximum = new System.Decimal(new int[] {
                                                                        1410065408,
                                                                        2,
                                                                        0,
                                                                        0});
            this.rateNumeric.Minimum = new System.Decimal(new int[] {
                                                                        1,
                                                                        0,
                                                                        0,
                                                                        0});
            this.rateNumeric.Name = "rateNumeric";
            this.rateNumeric.TabIndex = 1;
            this.rateNumeric.Value = new System.Decimal(new int[] {
                                                                      100,
                                                                      0,
                                                                      0,
                                                                      0});
            // 
            // triggeringGroupBox
            // 
            this.triggeringGroupBox.Controls.Add(this.indexNumeric);
            this.triggeringGroupBox.Controls.Add(this.indexLabel);
            this.triggeringGroupBox.Controls.Add(this.conditionLabel);
            this.triggeringGroupBox.Controls.Add(this.conditionComboBox);
            this.triggeringGroupBox.Controls.Add(this.windowLabel);
            this.triggeringGroupBox.Controls.Add(this.levelLabel);
            this.triggeringGroupBox.Controls.Add(this.windowNumeric);
            this.triggeringGroupBox.Controls.Add(this.levelNumeric);
            this.triggeringGroupBox.Controls.Add(this.pretriggerLabel);
            this.triggeringGroupBox.Controls.Add(this.pretriggerNumeric);
            this.triggeringGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.triggeringGroupBox.Location = new System.Drawing.Point(8, 232);
            this.triggeringGroupBox.Name = "triggeringGroupBox";
            this.triggeringGroupBox.Size = new System.Drawing.Size(272, 184);
            this.triggeringGroupBox.TabIndex = 2;
            this.triggeringGroupBox.TabStop = false;
            this.triggeringGroupBox.Text = "Triggering Parameters";
            // 
            // indexNumeric
            // 
            this.indexNumeric.Location = new System.Drawing.Point(128, 25);
            this.indexNumeric.Maximum = new System.Decimal(new int[] {
                                                                         511,
                                                                         0,
                                                                         0,
                                                                         0});
            this.indexNumeric.Name = "indexNumeric";
            this.indexNumeric.TabIndex = 1;
            // 
            // indexLabel
            // 
            this.indexLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.indexLabel.Location = new System.Drawing.Point(8, 27);
            this.indexLabel.Name = "indexLabel";
            this.indexLabel.Size = new System.Drawing.Size(120, 16);
            this.indexLabel.TabIndex = 0;
            this.indexLabel.Text = "Trigger Channel Index:";
            // 
            // conditionLabel
            // 
            this.conditionLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.conditionLabel.Location = new System.Drawing.Point(8, 59);
            this.conditionLabel.Name = "conditionLabel";
            this.conditionLabel.Size = new System.Drawing.Size(104, 16);
            this.conditionLabel.TabIndex = 2;
            this.conditionLabel.Text = "Trigger Condition:";
            // 
            // conditionComboBox
            // 
            this.conditionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.conditionComboBox.Items.AddRange(new object[] {
                                                                   "Above Level",
                                                                   "Below Level",
                                                                   "Rising Edge",
                                                                   "Falling Edge",
                                                                   "Inside Window",
                                                                   "Outside Window"});
            this.conditionComboBox.Location = new System.Drawing.Point(128, 57);
            this.conditionComboBox.Name = "conditionComboBox";
            this.conditionComboBox.Size = new System.Drawing.Size(120, 21);
            this.conditionComboBox.TabIndex = 3;
            // 
            // windowLabel
            // 
            this.windowLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.windowLabel.Location = new System.Drawing.Point(8, 123);
            this.windowLabel.Name = "windowLabel";
            this.windowLabel.Size = new System.Drawing.Size(112, 16);
            this.windowLabel.TabIndex = 6;
            this.windowLabel.Text = "Window/Hysteresis (V):";
            // 
            // levelLabel
            // 
            this.levelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.levelLabel.Location = new System.Drawing.Point(8, 91);
            this.levelLabel.Name = "levelLabel";
            this.levelLabel.Size = new System.Drawing.Size(112, 16);
            this.levelLabel.TabIndex = 4;
            this.levelLabel.Text = "Level (V):";
            // 
            // windowNumeric
            // 
            this.windowNumeric.DecimalPlaces = 2;
            this.windowNumeric.Increment = new System.Decimal(new int[] {
                                                                            1,
                                                                            0,
                                                                            0,
                                                                            65536});
            this.windowNumeric.Location = new System.Drawing.Point(128, 121);
            this.windowNumeric.Name = "windowNumeric";
            this.windowNumeric.TabIndex = 7;
            this.windowNumeric.Value = new System.Decimal(new int[] {
                                                                        1,
                                                                        0,
                                                                        0,
                                                                        65536});
            // 
            // levelNumeric
            // 
            this.levelNumeric.DecimalPlaces = 2;
            this.levelNumeric.Location = new System.Drawing.Point(128, 89);
            this.levelNumeric.Minimum = new System.Decimal(new int[] {
                                                                         100,
                                                                         0,
                                                                         0,
                                                                         -2147483648});
            this.levelNumeric.Name = "levelNumeric";
            this.levelNumeric.TabIndex = 5;
            this.levelNumeric.Value = new System.Decimal(new int[] {
                                                                       1,
                                                                       0,
                                                                       0,
                                                                       0});
            // 
            // pretriggerLabel
            // 
            this.pretriggerLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.pretriggerLabel.Location = new System.Drawing.Point(8, 155);
            this.pretriggerLabel.Name = "pretriggerLabel";
            this.pretriggerLabel.Size = new System.Drawing.Size(112, 16);
            this.pretriggerLabel.TabIndex = 8;
            this.pretriggerLabel.Text = "Pretrigger Samples:";
            // 
            // pretriggerNumeric
            // 
            this.pretriggerNumeric.Location = new System.Drawing.Point(128, 153);
            this.pretriggerNumeric.Maximum = new System.Decimal(new int[] {
                                                                              1410065408,
                                                                              2,
                                                                              0,
                                                                              0});
            this.pretriggerNumeric.Name = "pretriggerNumeric";
            this.pretriggerNumeric.TabIndex = 9;
            this.pretriggerNumeric.Value = new System.Decimal(new int[] {
                                                                            5,
                                                                            0,
                                                                            0,
                                                                            0});
            // 
            // startButton
            // 
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.startButton.Location = new System.Drawing.Point(48, 424);
            this.startButton.Name = "startButton";
            this.startButton.TabIndex = 3;
            this.startButton.Text = "Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.stopButton.Location = new System.Drawing.Point(152, 424);
            this.stopButton.Name = "stopButton";
            this.stopButton.TabIndex = 4;
            this.stopButton.Text = "Stop";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // acquiredGroupBox
            // 
            this.acquiredGroupBox.Controls.Add(this.resultsDataGrid);
            this.acquiredGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.acquiredGroupBox.Location = new System.Drawing.Point(288, 8);
            this.acquiredGroupBox.Name = "acquiredGroupBox";
            this.acquiredGroupBox.Size = new System.Drawing.Size(320, 408);
            this.acquiredGroupBox.TabIndex = 5;
            this.acquiredGroupBox.TabStop = false;
            this.acquiredGroupBox.Text = "Acquired Data";
            // 
            // resultsDataGrid
            // 
            this.resultsDataGrid.DataMember = "";
            this.resultsDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.resultsDataGrid.Location = new System.Drawing.Point(3, 16);
            this.resultsDataGrid.Name = "resultsDataGrid";
            this.resultsDataGrid.Size = new System.Drawing.Size(309, 384);
            this.resultsDataGrid.TabIndex = 0;
            // 
            // pretriggerAcquiredNumeric
            // 
            this.pretriggerAcquiredNumeric.Enabled = false;
            this.pretriggerAcquiredNumeric.Location = new System.Drawing.Point(488, 422);
            this.pretriggerAcquiredNumeric.Maximum = new System.Decimal(new int[] {
                                                                                      1410065408,
                                                                                      2,
                                                                                      0,
                                                                                      0});
            this.pretriggerAcquiredNumeric.Name = "pretriggerAcquiredNumeric";
            this.pretriggerAcquiredNumeric.TabIndex = 9;
            // 
            // pretriggerAcquiredLabel
            // 
            this.pretriggerAcquiredLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.pretriggerAcquiredLabel.Location = new System.Drawing.Point(296, 424);
            this.pretriggerAcquiredLabel.Name = "pretriggerAcquiredLabel";
            this.pretriggerAcquiredLabel.Size = new System.Drawing.Size(136, 16);
            this.pretriggerAcquiredLabel.TabIndex = 8;
            this.pretriggerAcquiredLabel.Text = "Pretrigger Samples Acquired:";
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(618, 456);
            this.Controls.Add(this.acquiredGroupBox);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.channelGroupBox);
            this.Controls.Add(this.timingGroupBox);
            this.Controls.Add(this.triggeringGroupBox);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.pretriggerAcquiredNumeric);
            this.Controls.Add(this.pretriggerAcquiredLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Continuous Acquisition of Voltage Samples - Internal Clock - SW Trigger";
            this.channelGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.maximumNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minimumNumeric)).EndInit();
            this.timingGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.samplesNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rateNumeric)).EndInit();
            this.triggeringGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.indexNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.windowNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.levelNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pretriggerNumeric)).EndInit();
            this.acquiredGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.resultsDataGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pretriggerAcquiredNumeric)).EndInit();
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
                // Create the task
                myTask = new Task();

                // Create channels
                myTask.AIChannels.CreateVoltageChannel(physicalChannelComboBox.Text,
                    "",
                    (AITerminalConfiguration)(-1),
                    Convert.ToDouble(minimumNumeric.Value),
                    Convert.ToDouble(maximumNumeric.Value),
                    AIVoltageUnits.Volts);

                // Set up timing
                myTask.Timing.ConfigureSampleClock("",
                    Convert.ToDouble(rateNumeric.Value),
                    SampleClockActiveEdge.Rising,
                    SampleQuantityMode.ContinuousSamples, 1000);

                // Verify the task
                myTask.Control(TaskAction.Verify);

                // Prepare pretrigger samples
                samples = Convert.ToInt32(samplesNumeric.Value);
                pretriggerSamples = Convert.ToInt32(pretriggerNumeric.Value);
                ptSaved = 0;
                pretriggerAcquiredNumeric.Value = 0;

                if (pretriggerSamples <= samples)
                {
                    pretrigger = new double[myTask.AIChannels.Count, samples];
                    ptSize = samples;
                }
                else
                {
                    // Make the size of the pretrigger buffer the smallest multiple of samples that is greater
                    // than the requested pretrigger samples
                    ptSize = ((int)(pretriggerSamples / samples) + 1) * samples;
                    pretrigger = new double[myTask.AIChannels.Count, ptSize];
                }

                // Prepare the table for data
                InitializeDataTable();
                resultsDataGrid.DataSource = dataTable;

                // Read the data
                runningTask = myTask;
                startButton.Enabled = false;
                stopButton.Enabled = true;
                EnableControls(false);

                reader = new AnalogMultiChannelReader(myTask.Stream);

                // Use SynchronizeCallbacks to specify that the object 
                // marshals callbacks across threads appropriately.
                reader.SynchronizeCallbacks = true;
                
                reader.BeginReadMultiSample(Convert.ToInt32(samplesNumeric.Value), new AsyncCallback(ReadData), myTask);
            }
            catch (DaqException ex)
            {
                MessageBox.Show(ex.Message);
                myTask.Dispose();
                startButton.Enabled = true;
                stopButton.Enabled = false;
                EnableControls(true);
                runningTask = null;
            }
        }

        private void ReadData(IAsyncResult ar)
        {
            try
            {
                if (runningTask != null && runningTask == ar.AsyncState)
                {
                    // Read the data
                    double[,] data = reader.EndReadMultiSample(ar);

                    // Get the channel index
                    int index = Convert.ToInt32(indexNumeric.Value);

                    if (index < 0 || index >= myTask.AIChannels.Count)
                    {
                        MessageBox.Show("Invalid channel index.");
                        myTask.Dispose();
                        startButton.Enabled = true;
                        stopButton.Enabled = false;
                        EnableControls(true);
                        runningTask = null;
                        return;
                    }

                    // Analyze the data for a start trigger
                    double level = Convert.ToDouble(levelNumeric.Value);
                    double window = Convert.ToDouble(windowNumeric.Value);
                    int triggerLocation = FindTrigger(data, index, level, window);
                    
                    // Read the next set of data
                    if (triggerLocation != -1)
                    {
                        // Found a trigger
                        int iDisplay = 0;

                        // Display pretrigger samples
                        if (pretriggerSamples > triggerLocation)
                        {
                            // Figure out how many samples we need from the pretrigger buffer
                            int deficit = pretriggerSamples - triggerLocation;

                            // Display samples from pretrigger buffer
                            for (int iData = 0; iData < deficit; iData++)
                            {
                                for (int iChan = 0; iChan < myTask.AIChannels.Count; iChan++)
                                {
                                    dataTable.Rows[iDisplay][iChan] = pretrigger[iChan, iData + ptSize - deficit];
                                }

                                iDisplay++;
                            }

                            // Now include all samples up to the trigger location in data
                            for (int iData = 0; iData < triggerLocation; iData++)
                            {
                                for (int iChan = 0; iChan < myTask.AIChannels.Count; iChan++)
                                {
                                    dataTable.Rows[iDisplay][iChan] = data[iChan, iData];
                                }

                                iDisplay++;
                            }

                            if (ptSaved + triggerLocation > pretriggerSamples)
                                pretriggerAcquiredNumeric.Value = pretriggerSamples;
                            else
                                pretriggerAcquiredNumeric.Value = ptSaved + triggerLocation;
                        }
                        else // pretriggerSamples <= triggerLocation
                        {
                            // We have enough pretrigger samples in the current data array
                            for (int iData = 0; iData < pretriggerSamples; iData++)
                            {
                                for (int iChan = 0; iChan < myTask.AIChannels.Count; iChan++)
                                {
                                    dataTable.Rows[iDisplay][iChan] = data[iChan, iData + triggerLocation - pretriggerSamples];
                                }

                                iDisplay++;
                            }

                            pretriggerAcquiredNumeric.Value = pretriggerSamples;
                        }

                        // Display data after the trigger
                        for (int iData = triggerLocation; iData < samples; iData++)
                        {
                            for (int iChan = 0; iChan < myTask.AIChannels.Count; iChan++)
                            {
                                dataTable.Rows[iDisplay][iChan] = data[iChan, iData];
                            }

                            iDisplay++;
                        }

                        // Read more data
                        data = reader.ReadMultiSample(triggerLocation);

                        // Display additional data after trigger
                        for (int iData = 0; iData < triggerLocation; iData++)
                        {
                            for (int iChan = 0; iChan < myTask.AIChannels.Count; iChan++)
                            {
                                dataTable.Rows[iDisplay][iChan] = data[iChan, iData];
                            }

                            iDisplay++;
                        }

                        // Stop the task
                        myTask.Dispose();
                        startButton.Enabled = true;
                        stopButton.Enabled = false;
                        EnableControls(true);
                        runningTask = null;
                    }
                    else // triggerLocation == -1
                    {
                        // Trigger not found; save pretrigger samples
                        if (pretriggerSamples <= samples)
                        {
                            // Save only one iteration (over all channels)
                            for (int iChan = 0; iChan < myTask.AIChannels.Count; iChan++)
                            {
                                for (int iData = 0; iData < samples; iData++)
                                {
                                    pretrigger[iChan, iData] = data[iChan, iData];
                                }
                            }
                        }
                        else // pretriggerSamples > samples
                        {
                            // Save over multiple iterations
                            int offset = ptSize - samples;

                            // Shift elements in the array (discarding the first samples of data)
                            for (int iChan = 0; iChan < myTask.AIChannels.Count; iChan++)
                            {
                                for (int iData = 0; iData < offset; iData++)
                                {
                                    pretrigger[iChan, iData] = pretrigger[iChan, iData + samples];
                                }
                            }

                            // Copy the new data into the array
                            for (int iChan = 0; iChan < myTask.AIChannels.Count; iChan++)
                            {
                                for (int iData = 0; iData < samples; iData++)
                                {
                                    pretrigger[iChan, iData + offset] = data[iChan, iData];
                                }
                            }
                        }

                        ptSaved += samples;

                        // Read the next set of samples
                        reader.BeginReadMultiSample(Convert.ToInt32(samplesNumeric.Value), new AsyncCallback(ReadData), myTask);
                    }
                }
            }
            catch (DaqException ex)
            {
                MessageBox.Show(ex.Message);
                myTask.Dispose();
                startButton.Enabled = true;
                stopButton.Enabled = false;
                EnableControls(true);
                runningTask = null;
            }
        }

        private int FindTrigger(double[,] data, int index, double level, double window)
        {
            int triggerLocation = -1;

            if (conditionComboBox.Text == "Rising Edge")
            {
                // Find a value less than (level - window)
                int i;
                for (i = 0; i < data.GetLength(1); i++)
                {
                    if (data[index, i] < level - window)
                    {
                        break;
                    }
                }

                // Then do an "above level" search
                for (; i < data.GetLength(1); i++)
                {
                    if (data[index, i] > level)
                    {
                        triggerLocation = i;
                        break;
                    }
                }
            }
            else if (conditionComboBox.Text == "Falling Edge")
            {
                // Find a value greater than (level + window)
                int i;
                for (i = 0; i < data.GetLength(1); i++)
                {
                    if (data[index, i] > level + window)
                    {
                        break;
                    }
                }

                // Then do a "below level" search
                for (; i < data.GetLength(1); i++)
                {
                    if (data[index, i] < level)
                    {
                        triggerLocation = i;
                        break;
                    }
                }
            }
            else if (conditionComboBox.Text == "Above Level")
            {
                // Trigger if we find a voltage above the level
                for (int i = 0; i < data.GetLength(1); i++)
                {
                    if (data[index, i] > level)
                    {
                        triggerLocation = i;
                        break;
                    }
                }                
            }
            else if (conditionComboBox.Text == "Below Level")
            {
                // Trigger if we find a voltage below the level
                for (int i = 0; i < data.GetLength(1); i++)
                {
                    if (data[index, i] < level)
                    {
                        triggerLocation = i;
                        break;
                    }
                }
            }
            else if (conditionComboBox.Text == "Inside Window")
            {
                // Trigger if we find a voltage inside the window surrounding the level
                for (int i = 0; i < data.GetLength(1); i++)
                {
                    if (Math.Abs(data[index, i] - level) < window)
                    {
                        triggerLocation = i;
                        break;
                    }
                }
            }
            else if (conditionComboBox.Text == "Outside Window")
            {
                // Trigger if we find a voltage outside the window surrounding the level
                for (int i = 0; i < data.GetLength(1); i++)
                {
                    if (Math.Abs(data[index, i] - level) > window)
                    {
                        triggerLocation = i;
                        break;
                    }
                }                
            }

            return triggerLocation;
        }

        private void InitializeDataTable()
        {
            dataTable.Rows.Clear();
            dataTable.Columns.Clear();
            dataColumn = new DataColumn[myTask.AIChannels.Count];

            for (int iChan = 0; iChan < myTask.AIChannels.Count; iChan++)
            {   
                dataColumn[iChan] = new DataColumn();
                dataColumn[iChan].DataType = typeof(double);
                dataColumn[iChan].ColumnName = myTask.AIChannels[iChan].PhysicalName;
            }

            dataTable.Columns.AddRange(dataColumn);

            for (int iData = 0; iData < samples + pretriggerSamples; iData++)
            {
                object[] rowArr = new object[myTask.AIChannels.Count];
                dataTable.Rows.Add(rowArr); 
            }

            resultsDataGrid.Refresh();
        }

        private void EnableControls(bool enable)
        {
            physicalChannelComboBox.Enabled = enable;
            maximumNumeric.Enabled = enable;
            minimumNumeric.Enabled = enable;
            rateNumeric.Enabled = enable;
            samplesNumeric.Enabled = enable;
            conditionComboBox.Enabled = enable;
            levelNumeric.Enabled = enable;
            windowNumeric.Enabled = enable;
            indexNumeric.Enabled = enable;
            pretriggerNumeric.Enabled = enable;
        }

        private void stopButton_Click(object sender, System.EventArgs e)
        {
            myTask.Dispose();
            runningTask = null;
            startButton.Enabled = true;
            stopButton.Enabled = false;
            EnableControls(true);
        }
    }
}
