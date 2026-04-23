/******************************************************************************
*
* Example program:
*   ContReadDigChan_PipeSampClkwHshk
*
* Category:
*   DI
*
* Description:
*   This examples demostrates how to interface the NI 6536/7 to a synchonous
*   FIFO.
*
* Instructions for running:
*   1.  Select the Physical Channels that correspond to where your signal is
*       input on the device.
*   2.  Enter the number of Samples per Channel per Read.  This is the number of
*       samples that will be read every time the DAQmx Read function is called.
*   3.  Specify the Sample Clock Rate of the input waveform.
*   4.  Specify the Ready for Transfer Output Terminal.
*   5.  Specify whither the ready for transfer signal is active high or active
*       low.
*   6.  Specify the Ready for Transfer Deassert Condition Threshold.  This
*       specifies in samples the threshold below which the Ready for Transfer
*       Event deasserts.
*   7.  Specify the Pause Trigger Polarity.  This parameter tells this device
*       when to pause.  If the polarity is set to High, then the device will
*       pause when the corresponding PFI line is high.  Note, that the device
*       will not pause on the next sample clock edge because of pipelining.
*   8.  Specify the Pause Trigger Source Terminal.
*
* Steps:
*   1.  Create a task.
*   2.  Create one Digital Output channel for each Digital Line in the Task.
*   3.  Configure the Task to use a pipelined sampled clock.
*   4.  Configure the pause trigger.
*   5.  Configure the ready for transfer event. You need to configurethe ready
*       for transfer deassert threshold to correspond tohow many samples it
*       takes for the device connected to the NI6536/7 to pause the data
*       transfer.
*   6.  Disallow Overwrites. When overwrites are disallowed, the datatransfer
*       between the device and the DAQmx buffer will pausewhen the DAQmx buffer
*       is full. It will resume when more spaceis available in the buffer.
*   7.  Create a DigitalMultiChannelReader and associate it with the task
*       byusing the task's stream.
*   8.  Call DigitalMultiChannelReader.BeginReadWaveform to install a callback
*       and begin the asynchronous read operation.
*   9.  Inside the callback, call DigitalSingleChannelReader.EndReadWaveformto
*       retrieve the data from the read.
*   10. Display the acquired data and
*       callDigitalMultiChannelReader.BeginReadWaveform again inside the
*       callback to perform another read.
*   11. Handle any DaqExceptions, if they occur.
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
*   Connect the FIFO's Not Empty Flag to the Pause Trigger. Connectthe FIFO's
*   Read Enable signal to the Ready for transfer Event.Connect the FIFO's read
*   clock to the sample clock terminal.Connect the data lines from the NI 6536/7
*   to the data lines ofthe FIFO.
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

using NationalInstruments.DAQmx;
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace NationalInstruments.Examples.ContReadDigChan_PipeSampClkwHshk
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.GroupBox channelParametersGroupBox;
        private System.Windows.Forms.Label physicalChannelLabel;
        private System.Windows.Forms.ComboBox physicalChannelComboBox;
        private System.Windows.Forms.Label samplesPerBufferLabel;
        private System.Windows.Forms.NumericUpDown samplesPerBufferNumericUpDown;
        private System.Windows.Forms.GroupBox timingParametersGroupBox;
        private System.Windows.Forms.Label sampleClockRateLabel;
        private System.Windows.Forms.NumericUpDown samplesClockRateNumericUpDown;
        private System.Windows.Forms.Label pauseTriggerPolarityLabel;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.ComboBox pauseTriggerPolarityComboBox;
        
        private Task myTask;
        private Task runningTask;
        private DigitalWaveform[] waveform;
        private AsyncCallback digitalCallback;
        private DigitalMultiChannelReader reader;
        private DataTable dataTable;
        private DataColumn[] dataColumn = null;
        private System.Windows.Forms.GroupBox handshakingParametersGroupBox;
        private System.Windows.Forms.TextBox pauseTriggerSourceTerminalTextBox;
        private System.Windows.Forms.Label pauseTriggerSourceTerminalLabel;
        private System.Windows.Forms.GroupBox resultsGroupBox;
        private System.Windows.Forms.DataGrid resultsDataGrid;
        private System.Windows.Forms.NumericUpDown transferDeassertConditionThresholdNumericUpDown;
        private System.Windows.Forms.Label transferDeassertConditionThresholdLabel;
        private System.Windows.Forms.TextBox readyForTransferOutputTerminalTextBox;
        private System.Windows.Forms.Label readyForTransferOutputTerminalLabel;
        private System.Windows.Forms.ComboBox readyForTransferLevelComboxBox;
        private System.Windows.Forms.Label readyForTransferLevelLabel;

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

            startButton.Enabled = false;
            dataTable = new DataTable();

            physicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DOLine, PhysicalChannelAccess.External));
            if (physicalChannelComboBox.Items.Count > 0)
            {
                physicalChannelComboBox.SelectedIndex = 0;
                startButton.Enabled = true;
            }

            stopButton.Enabled = false;
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
            this.channelParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.samplesPerBufferNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.samplesPerBufferLabel = new System.Windows.Forms.Label();
            this.physicalChannelComboBox = new System.Windows.Forms.ComboBox();
            this.physicalChannelLabel = new System.Windows.Forms.Label();
            this.timingParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.samplesClockRateNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.sampleClockRateLabel = new System.Windows.Forms.Label();
            this.handshakingParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.transferDeassertConditionThresholdNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.transferDeassertConditionThresholdLabel = new System.Windows.Forms.Label();
            this.pauseTriggerSourceTerminalTextBox = new System.Windows.Forms.TextBox();
            this.pauseTriggerSourceTerminalLabel = new System.Windows.Forms.Label();
            this.readyForTransferOutputTerminalTextBox = new System.Windows.Forms.TextBox();
            this.pauseTriggerPolarityComboBox = new System.Windows.Forms.ComboBox();
            this.pauseTriggerPolarityLabel = new System.Windows.Forms.Label();
            this.readyForTransferLevelComboxBox = new System.Windows.Forms.ComboBox();
            this.readyForTransferLevelLabel = new System.Windows.Forms.Label();
            this.readyForTransferOutputTerminalLabel = new System.Windows.Forms.Label();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.resultsGroupBox = new System.Windows.Forms.GroupBox();
            this.resultsDataGrid = new System.Windows.Forms.DataGrid();
            this.channelParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerBufferNumericUpDown)).BeginInit();
            this.timingParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.samplesClockRateNumericUpDown)).BeginInit();
            this.handshakingParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.transferDeassertConditionThresholdNumericUpDown)).BeginInit();
            this.resultsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resultsDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // channelParametersGroupBox
            // 
            this.channelParametersGroupBox.Controls.Add(this.samplesPerBufferNumericUpDown);
            this.channelParametersGroupBox.Controls.Add(this.samplesPerBufferLabel);
            this.channelParametersGroupBox.Controls.Add(this.physicalChannelComboBox);
            this.channelParametersGroupBox.Controls.Add(this.physicalChannelLabel);
            this.channelParametersGroupBox.Location = new System.Drawing.Point(8, 16);
            this.channelParametersGroupBox.Name = "channelParametersGroupBox";
            this.channelParametersGroupBox.Size = new System.Drawing.Size(368, 96);
            this.channelParametersGroupBox.TabIndex = 0;
            this.channelParametersGroupBox.TabStop = false;
            this.channelParametersGroupBox.Text = "Channel Parameters";
            // 
            // samplesPerBufferNumericUpDown
            // 
            this.samplesPerBufferNumericUpDown.Location = new System.Drawing.Point(241, 64);
            this.samplesPerBufferNumericUpDown.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.samplesPerBufferNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.samplesPerBufferNumericUpDown.Name = "samplesPerBufferNumericUpDown";
            this.samplesPerBufferNumericUpDown.Size = new System.Drawing.Size(120, 20);
            this.samplesPerBufferNumericUpDown.TabIndex = 3;
            this.samplesPerBufferNumericUpDown.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // samplesPerBufferLabel
            // 
            this.samplesPerBufferLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.samplesPerBufferLabel.Location = new System.Drawing.Point(8, 64);
            this.samplesPerBufferLabel.Name = "samplesPerBufferLabel";
            this.samplesPerBufferLabel.Size = new System.Drawing.Size(120, 23);
            this.samplesPerBufferLabel.TabIndex = 2;
            this.samplesPerBufferLabel.Text = "Samples Per Buffer:";
            // 
            // physicalChannelComboBox
            // 
            this.physicalChannelComboBox.Location = new System.Drawing.Point(240, 18);
            this.physicalChannelComboBox.Name = "physicalChannelComboBox";
            this.physicalChannelComboBox.Size = new System.Drawing.Size(121, 21);
            this.physicalChannelComboBox.TabIndex = 1;
            this.physicalChannelComboBox.Text = "Dev1/port0/line0:7";
            // 
            // physicalChannelLabel
            // 
            this.physicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.physicalChannelLabel.Location = new System.Drawing.Point(8, 24);
            this.physicalChannelLabel.Name = "physicalChannelLabel";
            this.physicalChannelLabel.Size = new System.Drawing.Size(100, 23);
            this.physicalChannelLabel.TabIndex = 0;
            this.physicalChannelLabel.Text = "Physical Channel:";
            // 
            // timingParametersGroupBox
            // 
            this.timingParametersGroupBox.Controls.Add(this.samplesClockRateNumericUpDown);
            this.timingParametersGroupBox.Controls.Add(this.sampleClockRateLabel);
            this.timingParametersGroupBox.Location = new System.Drawing.Point(8, 128);
            this.timingParametersGroupBox.Name = "timingParametersGroupBox";
            this.timingParametersGroupBox.Size = new System.Drawing.Size(368, 56);
            this.timingParametersGroupBox.TabIndex = 0;
            this.timingParametersGroupBox.TabStop = false;
            this.timingParametersGroupBox.Text = "Timing Parameters";
            // 
            // samplesClockRateNumericUpDown
            // 
            this.samplesClockRateNumericUpDown.Location = new System.Drawing.Point(241, 21);
            this.samplesClockRateNumericUpDown.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.samplesClockRateNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.samplesClockRateNumericUpDown.Name = "samplesClockRateNumericUpDown";
            this.samplesClockRateNumericUpDown.Size = new System.Drawing.Size(120, 20);
            this.samplesClockRateNumericUpDown.TabIndex = 1;
            this.samplesClockRateNumericUpDown.Value = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            // 
            // sampleClockRateLabel
            // 
            this.sampleClockRateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.sampleClockRateLabel.Location = new System.Drawing.Point(8, 24);
            this.sampleClockRateLabel.Name = "sampleClockRateLabel";
            this.sampleClockRateLabel.Size = new System.Drawing.Size(120, 23);
            this.sampleClockRateLabel.TabIndex = 0;
            this.sampleClockRateLabel.Text = "Sample Clock Rate (Hz):";
            // 
            // handshakingParametersGroupBox
            // 
            this.handshakingParametersGroupBox.Controls.Add(this.transferDeassertConditionThresholdNumericUpDown);
            this.handshakingParametersGroupBox.Controls.Add(this.transferDeassertConditionThresholdLabel);
            this.handshakingParametersGroupBox.Controls.Add(this.pauseTriggerSourceTerminalTextBox);
            this.handshakingParametersGroupBox.Controls.Add(this.pauseTriggerSourceTerminalLabel);
            this.handshakingParametersGroupBox.Controls.Add(this.readyForTransferOutputTerminalTextBox);
            this.handshakingParametersGroupBox.Controls.Add(this.pauseTriggerPolarityComboBox);
            this.handshakingParametersGroupBox.Controls.Add(this.pauseTriggerPolarityLabel);
            this.handshakingParametersGroupBox.Controls.Add(this.readyForTransferLevelComboxBox);
            this.handshakingParametersGroupBox.Controls.Add(this.readyForTransferLevelLabel);
            this.handshakingParametersGroupBox.Controls.Add(this.readyForTransferOutputTerminalLabel);
            this.handshakingParametersGroupBox.Location = new System.Drawing.Point(8, 200);
            this.handshakingParametersGroupBox.Name = "handshakingParametersGroupBox";
            this.handshakingParametersGroupBox.Size = new System.Drawing.Size(368, 224);
            this.handshakingParametersGroupBox.TabIndex = 1;
            this.handshakingParametersGroupBox.TabStop = false;
            this.handshakingParametersGroupBox.Text = "Handshaking Parameters";
            // 
            // transferDeassertConditionThresholdNumericUpDown
            // 
            this.transferDeassertConditionThresholdNumericUpDown.Location = new System.Drawing.Point(241, 104);
            this.transferDeassertConditionThresholdNumericUpDown.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.transferDeassertConditionThresholdNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.transferDeassertConditionThresholdNumericUpDown.Name = "transferDeassertConditionThresholdNumericUpDown";
            this.transferDeassertConditionThresholdNumericUpDown.Size = new System.Drawing.Size(120, 20);
            this.transferDeassertConditionThresholdNumericUpDown.TabIndex = 5;
            this.transferDeassertConditionThresholdNumericUpDown.Value = new decimal(new int[] {
            256,
            0,
            0,
            0});
            // 
            // transferDeassertConditionThresholdLabel
            // 
            this.transferDeassertConditionThresholdLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.transferDeassertConditionThresholdLabel.Location = new System.Drawing.Point(8, 104);
            this.transferDeassertConditionThresholdLabel.Name = "transferDeassertConditionThresholdLabel";
            this.transferDeassertConditionThresholdLabel.Size = new System.Drawing.Size(224, 32);
            this.transferDeassertConditionThresholdLabel.TabIndex = 4;
            this.transferDeassertConditionThresholdLabel.Text = "Ready for Transfer Deassert Condition Threshold:";
            // 
            // pauseTriggerSourceTerminalTextBox
            // 
            this.pauseTriggerSourceTerminalTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pauseTriggerSourceTerminalTextBox.Location = new System.Drawing.Point(240, 192);
            this.pauseTriggerSourceTerminalTextBox.Name = "pauseTriggerSourceTerminalTextBox";
            this.pauseTriggerSourceTerminalTextBox.Size = new System.Drawing.Size(120, 20);
            this.pauseTriggerSourceTerminalTextBox.TabIndex = 9;
            this.pauseTriggerSourceTerminalTextBox.Text = "/Dev1/PFI1";
            // 
            // pauseTriggerSourceTerminalLabel
            // 
            this.pauseTriggerSourceTerminalLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.pauseTriggerSourceTerminalLabel.Location = new System.Drawing.Point(8, 192);
            this.pauseTriggerSourceTerminalLabel.Name = "pauseTriggerSourceTerminalLabel";
            this.pauseTriggerSourceTerminalLabel.Size = new System.Drawing.Size(200, 23);
            this.pauseTriggerSourceTerminalLabel.TabIndex = 8;
            this.pauseTriggerSourceTerminalLabel.Text = "Pause Trigger Source Terminal:";
            // 
            // readyForTransferOutputTerminalTextBox
            // 
            this.readyForTransferOutputTerminalTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.readyForTransferOutputTerminalTextBox.Location = new System.Drawing.Point(240, 62);
            this.readyForTransferOutputTerminalTextBox.Name = "readyForTransferOutputTerminalTextBox";
            this.readyForTransferOutputTerminalTextBox.Size = new System.Drawing.Size(120, 20);
            this.readyForTransferOutputTerminalTextBox.TabIndex = 3;
            this.readyForTransferOutputTerminalTextBox.Text = "/Dev1/PFI0";
            // 
            // pauseTriggerPolarityComboBox
            // 
            this.pauseTriggerPolarityComboBox.Items.AddRange(new object[] {
            "High",
            "Low"});
            this.pauseTriggerPolarityComboBox.Location = new System.Drawing.Point(240, 152);
            this.pauseTriggerPolarityComboBox.Name = "pauseTriggerPolarityComboBox";
            this.pauseTriggerPolarityComboBox.Size = new System.Drawing.Size(121, 21);
            this.pauseTriggerPolarityComboBox.TabIndex = 7;
            this.pauseTriggerPolarityComboBox.Text = "High";
            // 
            // pauseTriggerPolarityLabel
            // 
            this.pauseTriggerPolarityLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.pauseTriggerPolarityLabel.Location = new System.Drawing.Point(8, 152);
            this.pauseTriggerPolarityLabel.Name = "pauseTriggerPolarityLabel";
            this.pauseTriggerPolarityLabel.Size = new System.Drawing.Size(200, 23);
            this.pauseTriggerPolarityLabel.TabIndex = 6;
            this.pauseTriggerPolarityLabel.Text = "Pause Trigger Polarity (Pause When):";
            // 
            // readyForTransferLevelComboxBox
            // 
            this.readyForTransferLevelComboxBox.Items.AddRange(new object[] {
            "Active High",
            "Active Low"});
            this.readyForTransferLevelComboxBox.Location = new System.Drawing.Point(240, 19);
            this.readyForTransferLevelComboxBox.Name = "readyForTransferLevelComboxBox";
            this.readyForTransferLevelComboxBox.Size = new System.Drawing.Size(121, 21);
            this.readyForTransferLevelComboxBox.TabIndex = 1;
            this.readyForTransferLevelComboxBox.Text = "Active Low";
            // 
            // readyForTransferLevelLabel
            // 
            this.readyForTransferLevelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.readyForTransferLevelLabel.Location = new System.Drawing.Point(8, 24);
            this.readyForTransferLevelLabel.Name = "readyForTransferLevelLabel";
            this.readyForTransferLevelLabel.Size = new System.Drawing.Size(200, 23);
            this.readyForTransferLevelLabel.TabIndex = 0;
            this.readyForTransferLevelLabel.Text = "Ready for Transfer Level:";
            // 
            // readyForTransferOutputTerminalLabel
            // 
            this.readyForTransferOutputTerminalLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.readyForTransferOutputTerminalLabel.Location = new System.Drawing.Point(8, 64);
            this.readyForTransferOutputTerminalLabel.Name = "readyForTransferOutputTerminalLabel";
            this.readyForTransferOutputTerminalLabel.Size = new System.Drawing.Size(200, 23);
            this.readyForTransferOutputTerminalLabel.TabIndex = 2;
            this.readyForTransferOutputTerminalLabel.Text = "Ready for Transfer Output Terminal:";
            // 
            // startButton
            // 
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.startButton.Location = new System.Drawing.Point(86, 432);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 2;
            this.startButton.Text = "Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.stopButton.Location = new System.Drawing.Point(214, 432);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 2;
            this.stopButton.Text = "Stop";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // resultsGroupBox
            // 
            this.resultsGroupBox.Controls.Add(this.resultsDataGrid);
            this.resultsGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.resultsGroupBox.Location = new System.Drawing.Point(384, 16);
            this.resultsGroupBox.Name = "resultsGroupBox";
            this.resultsGroupBox.Size = new System.Drawing.Size(336, 439);
            this.resultsGroupBox.TabIndex = 3;
            this.resultsGroupBox.TabStop = false;
            this.resultsGroupBox.Text = "Results";
            // 
            // resultsDataGrid
            // 
            this.resultsDataGrid.DataMember = "";
            this.resultsDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.resultsDataGrid.Location = new System.Drawing.Point(8, 16);
            this.resultsDataGrid.Name = "resultsDataGrid";
            this.resultsDataGrid.Size = new System.Drawing.Size(320, 417);
            this.resultsDataGrid.TabIndex = 0;
            this.resultsDataGrid.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(728, 470);
            this.Controls.Add(this.resultsGroupBox);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.handshakingParametersGroupBox);
            this.Controls.Add(this.channelParametersGroupBox);
            this.Controls.Add(this.timingParametersGroupBox);
            this.Controls.Add(this.stopButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Continous Read Digital Channel - Pipeline Samp Clk with Handshake";
            this.channelParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerBufferNumericUpDown)).EndInit();
            this.timingParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.samplesClockRateNumericUpDown)).EndInit();
            this.handshakingParametersGroupBox.ResumeLayout(false);
            this.handshakingParametersGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.transferDeassertConditionThresholdNumericUpDown)).EndInit();
            this.resultsGroupBox.ResumeLayout(false);
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

        private void startButton_Click(object sender, System.EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            startButton.Enabled = false;
            stopButton.Enabled = true;
            try
            {
                DigitalLevelPauseTriggerCondition pauseTriggerCondition;
                ReadyForTransferEventLevelActiveLevel transferLevel;
                
                if (pauseTriggerPolarityComboBox.SelectedIndex == 0)
                    pauseTriggerCondition = DigitalLevelPauseTriggerCondition.High;
                else
                    pauseTriggerCondition = DigitalLevelPauseTriggerCondition.Low;

                if (readyForTransferLevelComboxBox.SelectedIndex == 0)
                    transferLevel = ReadyForTransferEventLevelActiveLevel.ActiveHigh;
                else
                    transferLevel = ReadyForTransferEventLevelActiveLevel.ActiveLow;
                
                // Create and configure DI channel
                myTask = new Task();
                myTask.DIChannels.CreateChannel(physicalChannelComboBox.Text, "", ChannelLineGrouping.OneChannelForEachLine);
                myTask.Timing.ConfigurePipelinedSampleClock("", (double)samplesClockRateNumericUpDown.Value, SampleClockActiveEdge.Rising, 
                                         SampleQuantityMode.ContinuousSamples, (int) samplesPerBufferNumericUpDown.Value);

                // Configure pause trigger
                myTask.Triggers.PauseTrigger.ConfigureDigitalLevelTrigger(pauseTriggerSourceTerminalTextBox.Text, pauseTriggerCondition);

                myTask.Control(TaskAction.Verify);

                // Set ExportSignals properties
                myTask.ExportSignals.ReadyForTransferEventOutputTerminal = readyForTransferOutputTerminalTextBox.Text;
                myTask.ExportSignals.ReadyForTransferEventLevelActiveLevel = transferLevel;
                myTask.ExportSignals.ReadyForTransferEventDeassertCondition = ReadyForTransferEventDeassertCondition.OnboardMemoryCustomThreshold;
                myTask.ExportSignals.ReadyForTransferEventDeassertConditionCustomThreshold = (long) transferDeassertConditionThresholdNumericUpDown.Value;
                myTask.Stream.ReadOverwriteMode = ReadOverwriteMode.DoNotOverwriteUnreadSamples;

                InitializeDataTable(ref dataTable);
                resultsDataGrid.DataSource = dataTable;

                runningTask = myTask;
                
                reader = new DigitalMultiChannelReader(myTask.Stream);

                // Use SynchronizeCallbacks to specify that the object 
                // marshals callbacks across threads appropriately.
                myTask.SynchronizeCallbacks = true;

                digitalCallback = new AsyncCallback(DigitalCallback);

                // Set up callback
                reader.BeginReadWaveform((int) samplesPerBufferNumericUpDown.Value, digitalCallback, myTask);
            }
            catch (DaqException exception)
            {
                MessageBox.Show(exception.Message);

                runningTask = null;
                myTask.Dispose();
                startButton.Enabled = true;
                stopButton.Enabled = false;
            }
            Cursor.Current = Cursors.Default;
        }

        private void DigitalCallback(IAsyncResult ar)
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
                    reader.BeginReadWaveform((int) samplesPerBufferNumericUpDown.Value, digitalCallback, myTask);
                }
            }
            catch (DaqException exception)
            {
                MessageBox.Show(exception.Message);

                runningTask = null;
                myTask.Dispose();
                startButton.Enabled = true;
                stopButton.Enabled = false;
            }
        }

        private void dataToDataTable(DigitalWaveform[] waveform, ref DataTable dataTable)
        {
            // Iterate over channels
            int currentLineIndex = 0;
            foreach (DigitalWaveform signal in waveform)
            {
                for (int sample = 0; sample < signal.Signals[0].States.Count; ++sample)
                {
                    if (sample == 10)
                    {
                        break;
                    }
                    if (signal.Signals[0].States[sample] == DigitalState.ForceUp)
                    {
                        dataTable.Rows[sample][currentLineIndex] = 1;
                    }
                    else
                    {
                        dataTable.Rows[sample][currentLineIndex] = 0;
                    }
                }
                currentLineIndex++;
            }
        }

        private void InitializeDataTable(ref DataTable data)
        {
            int numOfLines = Convert.ToInt32(myTask.DIChannels.Count);
            data.Rows.Clear();
            data.Columns.Clear();
            dataColumn = new DataColumn[numOfLines];
            int numOfRows = 10;

            for (int currentLineIndex = 0; currentLineIndex < numOfLines; currentLineIndex++)
            {
                int channelNumber = currentLineIndex + 1;
                dataColumn[currentLineIndex] = new DataColumn();
                dataColumn[currentLineIndex].DataType = typeof(int);
                dataColumn[currentLineIndex].ColumnName = myTask.DIChannels[currentLineIndex].PhysicalName;
            }
            data.Columns.AddRange(dataColumn);

            for (int currentDataIndex = 0; currentDataIndex < numOfRows; currentDataIndex++)
            {
                object[] rowArr = new object[numOfLines];
                data.Rows.Add(rowArr);
            }
        }

        private void stopButton_Click(object sender, System.EventArgs e)
        {
            runningTask = null;
            myTask.Dispose();

            stopButton.Enabled = false;
            startButton.Enabled = true;
        }
    }
}
