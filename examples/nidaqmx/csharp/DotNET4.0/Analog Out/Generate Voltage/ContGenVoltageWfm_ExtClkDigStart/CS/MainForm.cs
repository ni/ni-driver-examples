/******************************************************************************
*
* Example program:
*   ContGenVoltageWfm_ExtClkDigStart
*
* Category:
*   AO
*
* Description:
*   This example demonstrates how to continuously output a waveform using an
*   external sample clock and a digital start trigger.
*
* Instructions for running:
*   1.  Select the physical channel corresponding to where your signal is output
*       on the DAQ device.
*   2.  Enter the minimum and maximum voltage values.
*   3.  Select the sample clock source.
*   4.  Set the signal frequency of the generation.
*   5.  Select the digital trigger source.
*   6.  Specify the desired trigger edge.
*   7.  Select the desired waveform type.
*   8.  The rest of the parameters in the Function Generator Parameters section
*       will affect the way the waveform is created, before it's sent to the
*       analog output of the board. Select the amplitude, number of samples per
*       buffer, and the number of cycles per buffer to be used as waveform data.
*
* Steps:
*   1.  Create a new task and an analog output voltage channel.
*   2.  Specify the external sample clock source. In this example, the external
*       sample clock source defaults to the signal on PFI7. Additionally, define
*       the sample mode to be continuous.
*   3.  Specify the source and edge triggering parameters. In this example,
*       source defaults to PFI0.
*   4.  Create a AnalogSingleChannelWriter and call the WriteMultiSample method
*       to write the waveform to a buffer.
*   5.  Call Task.Start().
*   6.  When the user presses the stop button, stop the task.
*   7.  Dispose the Task object to clean-up any resources associated with the
*       task.
*   8.  Handle any DaqExceptions, if they occur.
*
* I/O Connections Overview:
*   Make sure your signal output terminal matches the text in the physical
*   channel text box. 
*   In this case the signal will output to the ao0 pin on your DAQ Device. Wire in
*   the external sample clock 
*   to a PFI or RTSI pin of your choice on the board. Specify the same terminal
*   name as an argument to the 
*   ConfigureSampleClock method (PFI7 is used as an example). Also, make sure your
*   digital trigger terminal 
*   matches the text in the digital trigger text box (PFI0 is used as an example).
*   For more information on 
*   the input and output terminals for your device, open the NI-DAQmx Help, and
*   refer to the NI-DAQmx Device 
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

namespace NationalInstruments.Examples.ContGenVoltageWfm_ExtClkDigStart
{
    /// <summary>
    /// Summary description for MainForm.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
		private Task myTask;

        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.TextBox clockSourceTextBox;
        private System.Windows.Forms.Label clockSourceLabel;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.GroupBox channelParametersGroupBox;
        private System.Windows.Forms.Label maximumValueLabel;
        private System.Windows.Forms.Label minimumValueLabel;
        private System.Windows.Forms.Label physicalChannelLabel;
        private System.Windows.Forms.Label digitalTriggerLabel;
        private System.Windows.Forms.Label digitalTriggerEdgeLabel;
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.RadioButton risingRadioButton;
        private System.Windows.Forms.RadioButton fallingRadioButton;
        private System.Windows.Forms.Label frequencyLabel;
        private System.Windows.Forms.GroupBox functionGeneratorGroupBox;
        private System.Windows.Forms.Label waveformTypeLabel;
        private System.Windows.Forms.ComboBox signalTypeComboBox;
        internal System.Windows.Forms.Label amplitudeLabel;
        private System.Windows.Forms.NumericUpDown maximumValueNumeric;
        private System.Windows.Forms.NumericUpDown minimumValueNumeric;
        private System.Windows.Forms.GroupBox timingParametersGroupBox;
        private System.Windows.Forms.GroupBox triggeringGroupBox;
        private System.Windows.Forms.TextBox digitalTriggerSourceTextBox;
        internal System.Windows.Forms.NumericUpDown amplitudeNumeric;
        private System.Windows.Forms.NumericUpDown samplesPerBufferNumeric;
        private System.Windows.Forms.Label samplesPerBufferLabel;
        private System.Windows.Forms.Label cyclesPerBufferLabel;
        private System.Windows.Forms.NumericUpDown cyclesPerBufferNumeric;
        private System.Windows.Forms.NumericUpDown desiredFrequencyNumeric;
        private System.Windows.Forms.ComboBox physicalChannelComboBox;
        private DigitalEdgeStartTriggerEdge edge = DigitalEdgeStartTriggerEdge.Rising;

        public MainForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            FunctionGenerator.InitComboBox(signalTypeComboBox);

            physicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AO, PhysicalChannelAccess.External));
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
            this.startButton = new System.Windows.Forms.Button();
            this.timingParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.desiredFrequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.frequencyLabel = new System.Windows.Forms.Label();
            this.clockSourceTextBox = new System.Windows.Forms.TextBox();
            this.clockSourceLabel = new System.Windows.Forms.Label();
            this.stopButton = new System.Windows.Forms.Button();
            this.channelParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.physicalChannelComboBox = new System.Windows.Forms.ComboBox();
            this.maximumValueLabel = new System.Windows.Forms.Label();
            this.minimumValueLabel = new System.Windows.Forms.Label();
            this.physicalChannelLabel = new System.Windows.Forms.Label();
            this.maximumValueNumeric = new System.Windows.Forms.NumericUpDown();
            this.minimumValueNumeric = new System.Windows.Forms.NumericUpDown();
            this.triggeringGroupBox = new System.Windows.Forms.GroupBox();
            this.fallingRadioButton = new System.Windows.Forms.RadioButton();
            this.risingRadioButton = new System.Windows.Forms.RadioButton();
            this.digitalTriggerSourceTextBox = new System.Windows.Forms.TextBox();
            this.digitalTriggerEdgeLabel = new System.Windows.Forms.Label();
            this.digitalTriggerLabel = new System.Windows.Forms.Label();
            this.functionGeneratorGroupBox = new System.Windows.Forms.GroupBox();
            this.amplitudeLabel = new System.Windows.Forms.Label();
            this.amplitudeNumeric = new System.Windows.Forms.NumericUpDown();
            this.samplesPerBufferNumeric = new System.Windows.Forms.NumericUpDown();
            this.samplesPerBufferLabel = new System.Windows.Forms.Label();
            this.cyclesPerBufferLabel = new System.Windows.Forms.Label();
            this.cyclesPerBufferNumeric = new System.Windows.Forms.NumericUpDown();
            this.waveformTypeLabel = new System.Windows.Forms.Label();
            this.signalTypeComboBox = new System.Windows.Forms.ComboBox();
            this.timingParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.desiredFrequencyNumeric)).BeginInit();
            this.channelParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maximumValueNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minimumValueNumeric)).BeginInit();
            this.triggeringGroupBox.SuspendLayout();
            this.functionGeneratorGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.amplitudeNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerBufferNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cyclesPerBufferNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // startButton
            // 
            this.startButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.startButton.Location = new System.Drawing.Point(359, 208);
            this.startButton.Name = "startButton";
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // timingParametersGroupBox
            // 
            this.timingParametersGroupBox.Controls.Add(this.desiredFrequencyNumeric);
            this.timingParametersGroupBox.Controls.Add(this.frequencyLabel);
            this.timingParametersGroupBox.Controls.Add(this.clockSourceTextBox);
            this.timingParametersGroupBox.Controls.Add(this.clockSourceLabel);
            this.timingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.timingParametersGroupBox.Location = new System.Drawing.Point(8, 152);
            this.timingParametersGroupBox.Name = "timingParametersGroupBox";
            this.timingParametersGroupBox.Size = new System.Drawing.Size(256, 96);
            this.timingParametersGroupBox.TabIndex = 3;
            this.timingParametersGroupBox.TabStop = false;
            this.timingParametersGroupBox.Text = "Timing Parameters";
            // 
            // desiredFrequencyNumeric
            // 
            this.desiredFrequencyNumeric.Location = new System.Drawing.Point(144, 56);
            this.desiredFrequencyNumeric.Maximum = new System.Decimal(new int[] {
                                                                                    1000000,
                                                                                    0,
                                                                                    0,
                                                                                    0});
            this.desiredFrequencyNumeric.Minimum = new System.Decimal(new int[] {
                                                                                    1,
                                                                                    0,
                                                                                    0,
                                                                                    0});
            this.desiredFrequencyNumeric.Name = "desiredFrequencyNumeric";
            this.desiredFrequencyNumeric.Size = new System.Drawing.Size(96, 20);
            this.desiredFrequencyNumeric.TabIndex = 1;
            this.desiredFrequencyNumeric.Value = new System.Decimal(new int[] {
                                                                                  1000,
                                                                                  0,
                                                                                  0,
                                                                                  0});
            // 
            // frequencyLabel
            // 
            this.frequencyLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.frequencyLabel.Location = new System.Drawing.Point(16, 58);
            this.frequencyLabel.Name = "frequencyLabel";
            this.frequencyLabel.Size = new System.Drawing.Size(128, 16);
            this.frequencyLabel.TabIndex = 20;
            this.frequencyLabel.Text = "Desired Frequency (Hz):";
            // 
            // clockSourceTextBox
            // 
            this.clockSourceTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.clockSourceTextBox.Location = new System.Drawing.Point(144, 24);
            this.clockSourceTextBox.Name = "clockSourceTextBox";
            this.clockSourceTextBox.Size = new System.Drawing.Size(96, 20);
            this.clockSourceTextBox.TabIndex = 0;
            this.clockSourceTextBox.Text = "/Dev1/PFI7";
            // 
            // clockSourceLabel
            // 
            this.clockSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.clockSourceLabel.Location = new System.Drawing.Point(16, 26);
            this.clockSourceLabel.Name = "clockSourceLabel";
            this.clockSourceLabel.Size = new System.Drawing.Size(96, 16);
            this.clockSourceLabel.TabIndex = 16;
            this.clockSourceLabel.Text = "Clock Source:";
            // 
            // stopButton
            // 
            this.stopButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.stopButton.Enabled = false;
            this.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.stopButton.Location = new System.Drawing.Point(359, 240);
            this.stopButton.Name = "stopButton";
            this.stopButton.TabIndex = 1;
            this.stopButton.Text = "Stop";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // channelParametersGroupBox
            // 
            this.channelParametersGroupBox.Controls.Add(this.physicalChannelComboBox);
            this.channelParametersGroupBox.Controls.Add(this.maximumValueLabel);
            this.channelParametersGroupBox.Controls.Add(this.minimumValueLabel);
            this.channelParametersGroupBox.Controls.Add(this.physicalChannelLabel);
            this.channelParametersGroupBox.Controls.Add(this.maximumValueNumeric);
            this.channelParametersGroupBox.Controls.Add(this.minimumValueNumeric);
            this.channelParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.channelParametersGroupBox.Location = new System.Drawing.Point(8, 8);
            this.channelParametersGroupBox.Name = "channelParametersGroupBox";
            this.channelParametersGroupBox.Size = new System.Drawing.Size(256, 136);
            this.channelParametersGroupBox.TabIndex = 2;
            this.channelParametersGroupBox.TabStop = false;
            this.channelParametersGroupBox.Text = "Channel Parameters";
            // 
            // physicalChannelComboBox
            // 
            this.physicalChannelComboBox.Location = new System.Drawing.Point(144, 24);
            this.physicalChannelComboBox.Name = "physicalChannelComboBox";
            this.physicalChannelComboBox.Size = new System.Drawing.Size(96, 21);
            this.physicalChannelComboBox.TabIndex = 1;
            this.physicalChannelComboBox.Text = "Dev1/ao0";
            // 
            // maximumValueLabel
            // 
            this.maximumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.maximumValueLabel.Location = new System.Drawing.Point(16, 98);
            this.maximumValueLabel.Name = "maximumValueLabel";
            this.maximumValueLabel.Size = new System.Drawing.Size(112, 16);
            this.maximumValueLabel.TabIndex = 4;
            this.maximumValueLabel.Text = "Maximum Value (V):";
            // 
            // minimumValueLabel
            // 
            this.minimumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.minimumValueLabel.Location = new System.Drawing.Point(16, 62);
            this.minimumValueLabel.Name = "minimumValueLabel";
            this.minimumValueLabel.Size = new System.Drawing.Size(104, 16);
            this.minimumValueLabel.TabIndex = 2;
            this.minimumValueLabel.Text = "Minimum Value (V):";
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
            // maximumValueNumeric
            // 
            this.maximumValueNumeric.Location = new System.Drawing.Point(144, 96);
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
                                                                              10,
                                                                              0,
                                                                              0,
                                                                              0});
            // 
            // minimumValueNumeric
            // 
            this.minimumValueNumeric.Location = new System.Drawing.Point(144, 60);
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
                                                                              10,
                                                                              0,
                                                                              0,
                                                                              -2147483648});
            // 
            // triggeringGroupBox
            // 
            this.triggeringGroupBox.Controls.Add(this.fallingRadioButton);
            this.triggeringGroupBox.Controls.Add(this.risingRadioButton);
            this.triggeringGroupBox.Controls.Add(this.digitalTriggerSourceTextBox);
            this.triggeringGroupBox.Controls.Add(this.digitalTriggerEdgeLabel);
            this.triggeringGroupBox.Controls.Add(this.digitalTriggerLabel);
            this.triggeringGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.triggeringGroupBox.Location = new System.Drawing.Point(8, 256);
            this.triggeringGroupBox.Name = "triggeringGroupBox";
            this.triggeringGroupBox.Size = new System.Drawing.Size(256, 104);
            this.triggeringGroupBox.TabIndex = 4;
            this.triggeringGroupBox.TabStop = false;
            this.triggeringGroupBox.Text = "Triggering Parameters";
            // 
            // fallingRadioButton
            // 
            this.fallingRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.fallingRadioButton.Location = new System.Drawing.Point(136, 80);
            this.fallingRadioButton.Name = "fallingRadioButton";
            this.fallingRadioButton.Size = new System.Drawing.Size(56, 16);
            this.fallingRadioButton.TabIndex = 2;
            this.fallingRadioButton.Text = "Falling";
            // 
            // risingRadioButton
            // 
            this.risingRadioButton.Checked = true;
            this.risingRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.risingRadioButton.Location = new System.Drawing.Point(136, 56);
            this.risingRadioButton.Name = "risingRadioButton";
            this.risingRadioButton.Size = new System.Drawing.Size(56, 24);
            this.risingRadioButton.TabIndex = 1;
            this.risingRadioButton.TabStop = true;
            this.risingRadioButton.Text = "Rising";
            this.risingRadioButton.CheckedChanged += new System.EventHandler(this.risingRadioButton_CheckedChanged);
            // 
            // digitalTriggerSourceTextBox
            // 
            this.digitalTriggerSourceTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.digitalTriggerSourceTextBox.Location = new System.Drawing.Point(144, 24);
            this.digitalTriggerSourceTextBox.Name = "digitalTriggerSourceTextBox";
            this.digitalTriggerSourceTextBox.Size = new System.Drawing.Size(96, 20);
            this.digitalTriggerSourceTextBox.TabIndex = 0;
            this.digitalTriggerSourceTextBox.Text = "/Dev1/PFI0";
            // 
            // digitalTriggerEdgeLabel
            // 
            this.digitalTriggerEdgeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.digitalTriggerEdgeLabel.Location = new System.Drawing.Point(16, 56);
            this.digitalTriggerEdgeLabel.Name = "digitalTriggerEdgeLabel";
            this.digitalTriggerEdgeLabel.Size = new System.Drawing.Size(112, 16);
            this.digitalTriggerEdgeLabel.TabIndex = 17;
            this.digitalTriggerEdgeLabel.Text = "Digital Trigger Edge:";
            // 
            // digitalTriggerLabel
            // 
            this.digitalTriggerLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.digitalTriggerLabel.Location = new System.Drawing.Point(16, 26);
            this.digitalTriggerLabel.Name = "digitalTriggerLabel";
            this.digitalTriggerLabel.Size = new System.Drawing.Size(120, 16);
            this.digitalTriggerLabel.TabIndex = 16;
            this.digitalTriggerLabel.Text = "Digital Trigger Source:";
            // 
            // functionGeneratorGroupBox
            // 
            this.functionGeneratorGroupBox.Controls.Add(this.amplitudeLabel);
            this.functionGeneratorGroupBox.Controls.Add(this.amplitudeNumeric);
            this.functionGeneratorGroupBox.Controls.Add(this.samplesPerBufferNumeric);
            this.functionGeneratorGroupBox.Controls.Add(this.samplesPerBufferLabel);
            this.functionGeneratorGroupBox.Controls.Add(this.cyclesPerBufferLabel);
            this.functionGeneratorGroupBox.Controls.Add(this.cyclesPerBufferNumeric);
            this.functionGeneratorGroupBox.Controls.Add(this.waveformTypeLabel);
            this.functionGeneratorGroupBox.Controls.Add(this.signalTypeComboBox);
            this.functionGeneratorGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.functionGeneratorGroupBox.Location = new System.Drawing.Point(280, 8);
            this.functionGeneratorGroupBox.Name = "functionGeneratorGroupBox";
            this.functionGeneratorGroupBox.Size = new System.Drawing.Size(232, 176);
            this.functionGeneratorGroupBox.TabIndex = 5;
            this.functionGeneratorGroupBox.TabStop = false;
            this.functionGeneratorGroupBox.Text = "Function Generator Parameters";
            // 
            // amplitudeLabel
            // 
            this.amplitudeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.amplitudeLabel.Location = new System.Drawing.Point(16, 138);
            this.amplitudeLabel.Name = "amplitudeLabel";
            this.amplitudeLabel.Size = new System.Drawing.Size(72, 16);
            this.amplitudeLabel.TabIndex = 44;
            this.amplitudeLabel.Text = "Amplitude:";
            // 
            // amplitudeNumeric
            // 
            this.amplitudeNumeric.DecimalPlaces = 1;
            this.amplitudeNumeric.Increment = new System.Decimal(new int[] {
                                                                               1,
                                                                               0,
                                                                               0,
                                                                               65536});
            this.amplitudeNumeric.Location = new System.Drawing.Point(120, 136);
            this.amplitudeNumeric.Minimum = new System.Decimal(new int[] {
                                                                             1,
                                                                             0,
                                                                             0,
                                                                             0});
            this.amplitudeNumeric.Name = "amplitudeNumeric";
            this.amplitudeNumeric.Size = new System.Drawing.Size(96, 20);
            this.amplitudeNumeric.TabIndex = 3;
            this.amplitudeNumeric.Value = new System.Decimal(new int[] {
                                                                           2,
                                                                           0,
                                                                           0,
                                                                           0});
            // 
            // samplesPerBufferNumeric
            // 
            this.samplesPerBufferNumeric.Location = new System.Drawing.Point(120, 96);
            this.samplesPerBufferNumeric.Maximum = new System.Decimal(new int[] {
                                                                                    10000000,
                                                                                    0,
                                                                                    0,
                                                                                    0});
            this.samplesPerBufferNumeric.Minimum = new System.Decimal(new int[] {
                                                                                    1,
                                                                                    0,
                                                                                    0,
                                                                                    0});
            this.samplesPerBufferNumeric.Name = "samplesPerBufferNumeric";
            this.samplesPerBufferNumeric.Size = new System.Drawing.Size(96, 20);
            this.samplesPerBufferNumeric.TabIndex = 2;
            this.samplesPerBufferNumeric.Value = new System.Decimal(new int[] {
                                                                                  250,
                                                                                  0,
                                                                                  0,
                                                                                  0});
            // 
            // samplesPerBufferLabel
            // 
            this.samplesPerBufferLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.samplesPerBufferLabel.Location = new System.Drawing.Point(16, 98);
            this.samplesPerBufferLabel.Name = "samplesPerBufferLabel";
            this.samplesPerBufferLabel.Size = new System.Drawing.Size(120, 16);
            this.samplesPerBufferLabel.TabIndex = 35;
            this.samplesPerBufferLabel.Text = "Samples Per Buffer:";
            // 
            // cyclesPerBufferLabel
            // 
            this.cyclesPerBufferLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cyclesPerBufferLabel.Location = new System.Drawing.Point(16, 62);
            this.cyclesPerBufferLabel.Name = "cyclesPerBufferLabel";
            this.cyclesPerBufferLabel.Size = new System.Drawing.Size(104, 16);
            this.cyclesPerBufferLabel.TabIndex = 34;
            this.cyclesPerBufferLabel.Text = "Cycles Per Buffer:";
            // 
            // cyclesPerBufferNumeric
            // 
            this.cyclesPerBufferNumeric.Location = new System.Drawing.Point(120, 60);
            this.cyclesPerBufferNumeric.Minimum = new System.Decimal(new int[] {
                                                                                   1,
                                                                                   0,
                                                                                   0,
                                                                                   0});
            this.cyclesPerBufferNumeric.Name = "cyclesPerBufferNumeric";
            this.cyclesPerBufferNumeric.Size = new System.Drawing.Size(96, 20);
            this.cyclesPerBufferNumeric.TabIndex = 1;
            this.cyclesPerBufferNumeric.Value = new System.Decimal(new int[] {
                                                                                 5,
                                                                                 0,
                                                                                 0,
                                                                                 0});
            // 
            // waveformTypeLabel
            // 
            this.waveformTypeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.waveformTypeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.waveformTypeLabel.Location = new System.Drawing.Point(16, 26);
            this.waveformTypeLabel.Name = "waveformTypeLabel";
            this.waveformTypeLabel.Size = new System.Drawing.Size(88, 16);
            this.waveformTypeLabel.TabIndex = 31;
            this.waveformTypeLabel.Text = "Waveform Type:";
            // 
            // signalTypeComboBox
            // 
            this.signalTypeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.signalTypeComboBox.ItemHeight = 13;
            this.signalTypeComboBox.Location = new System.Drawing.Point(120, 24);
            this.signalTypeComboBox.Name = "signalTypeComboBox";
            this.signalTypeComboBox.Size = new System.Drawing.Size(96, 21);
            this.signalTypeComboBox.TabIndex = 0;
            this.signalTypeComboBox.Text = "Sine Wave";
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(522, 376);
            this.Controls.Add(this.functionGeneratorGroupBox);
            this.Controls.Add(this.triggeringGroupBox);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.timingParametersGroupBox);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.channelParametersGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(528, 408);
            this.MinimumSize = new System.Drawing.Size(528, 408);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Continuous Voltage Generation - External Clock - Digital Start";
            this.timingParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.desiredFrequencyNumeric)).EndInit();
            this.channelParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.maximumValueNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minimumValueNumeric)).EndInit();
            this.triggeringGroupBox.ResumeLayout(false);
            this.functionGeneratorGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.amplitudeNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerBufferNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cyclesPerBufferNumeric)).EndInit();
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
            try
            {
                // Create the task and channel
                myTask = new Task();
                myTask.AOChannels.CreateVoltageChannel(physicalChannelComboBox.Text,
                    "",
                    Convert.ToDouble(minimumValueNumeric.Value), 
                    Convert.ToDouble(maximumValueNumeric.Value),
                    AOVoltageUnits.Volts);

                // Verify the task before doing the waveform calculations
                  myTask.Control(TaskAction.Verify);

                // Calculate some waveform parameters and generate data
                FunctionGenerator fGen = new FunctionGenerator(
                    myTask.Timing,
                    desiredFrequencyNumeric.Value.ToString(),
                    samplesPerBufferNumeric.Value.ToString(),
                    cyclesPerBufferNumeric.Value.ToString(),
                    signalTypeComboBox.Text,
                    amplitudeNumeric.Value.ToString());

                // Configure the sample clock with the calculated rate
                myTask.Timing.ConfigureSampleClock(
                    clockSourceTextBox.Text, // specified external clock
                    fGen.ResultingSampleClockRate,
                    SampleClockActiveEdge.Rising,
                    SampleQuantityMode.ContinuousSamples, 1000);

                // Setup trigger
                myTask.Triggers.StartTrigger.ConfigureDigitalEdgeTrigger(
                    digitalTriggerSourceTextBox.Text, //trigger source
                    edge);
                
                // Set up the Task Done event handler        
                myTask.Done += new TaskDoneEventHandler(myTask_Done);

                // Write the data
                AnalogSingleChannelWriter writer = new AnalogSingleChannelWriter(myTask.Stream);
                writer.WriteMultiSample(false, fGen.Data);
                myTask.Start();

                startButton.Enabled = false;
                stopButton.Enabled = true;
                channelParametersGroupBox.Enabled = false;
                timingParametersGroupBox.Enabled = false;
                triggeringGroupBox.Enabled = false;
                functionGeneratorGroupBox.Enabled = false;
            }
            catch (Exception exception)
            {
                myTask.Dispose();
                MessageBox.Show(exception.Message);

                startButton.Enabled = true;
                stopButton.Enabled = false;
                channelParametersGroupBox.Enabled = true;
                timingParametersGroupBox.Enabled = true;
                triggeringGroupBox.Enabled = true;
                functionGeneratorGroupBox.Enabled = true;
            }
            Cursor.Current = Cursors.Default;
        }

        private void stopButton_Click(object sender, System.EventArgs e)
        {
            if (myTask != null)
            {
				myTask.Stop();
                myTask.Dispose();
            }

            startButton.Enabled = true;
            stopButton.Enabled = false;
            channelParametersGroupBox.Enabled = true;
            timingParametersGroupBox.Enabled = true;
            triggeringGroupBox.Enabled = true;
            functionGeneratorGroupBox.Enabled = true;
        }

        private void myTask_Done(object sender, TaskDoneEventArgs e)
        {
			if (e.Error != null)
			{
				MessageBox.Show(e.Error.Message);
			}

			if (myTask != null)
			{
				myTask.Dispose();
			}

			startButton.Enabled = true;
			stopButton.Enabled = false;
			channelParametersGroupBox.Enabled = true;
			timingParametersGroupBox.Enabled = true;
			this.triggeringGroupBox.Enabled = true;
			functionGeneratorGroupBox.Enabled = true;
        }

        private void risingRadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            if (risingRadioButton.Checked)
            {
                edge = DigitalEdgeStartTriggerEdge.Rising;
            }
            else
            {
                edge = DigitalEdgeStartTriggerEdge.Falling;
            }
        }
    }
}
