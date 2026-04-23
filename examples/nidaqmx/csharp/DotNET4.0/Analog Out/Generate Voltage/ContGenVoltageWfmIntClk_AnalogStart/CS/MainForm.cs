/******************************************************************************
*
* Example program:
*   ContGenVoltageWfmIntClk_AnalogStart
*
* Category:
*   AO
*
* Description:
*   This example demonstrates how to continuously output a periodic waveform
*   using an internal clock and an analog trigger signal.
*
* Instructions for running:
*   1.  Select the physical channel corresponding to where your signal is output
*       on the DAQ device.
*   2.  Enter the minimum and maximum voltage values.
*   3.  Enter the desired frequency for the generation. The onboard sample clock
*       will operate at this rate.
*   4.  Specify the analog trigger source, slope, and level.
*   5.  Select the desired waveform type.
*   6.  The rest of the parameters in the Function Generator Parameters section
*       will affect the way the waveform is created, before it's sent to the
*       analog output of the board. Select the amplitude, number of samples per
*       buffer, and the number of cycles per buffer to be used as waveform data.
*
* Steps:
*   1.  Create a new task and an analog output voltage channel.
*   2.  Define the parameters for the analog trigger source. Additionally,
*       define the sample mode to be continuous.
*   3.  Create a AnalogSingleChannelWriter and call the WriteMultiSample method
*       to write the waveform to a buffer.
*   4.  When the user presses the stop button, stop the task.
*   5.  Dispose the Task object to clean-up any resources associated with the
*       task.
*   6.  Handle any DaqExceptions, if they occur.
*
* I/O Connections Overview:
*   Make sure your signal output terminal matches the physical channel text box.
*   In this 
*   case the signal will output to the ao0 pin on your DAQ device. Wire in the
*   analog trigger signal to APFI0 
*   or the pin of your choice.  Make sure to pass in the same terminal name to the
*   parameter of the ConfigureSampleClock 
*   method.  APFI0 is the default Analog Trigger pin for M Series devices.  For
*   more information on the input 
*   and output terminals for your device, open the NI-DAQmx Help, and refer to the
*   NI-DAQmx Device Terminals 
*   and Device Considerations books in the table of contents.
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
using NationalInstruments.Examples;

namespace NationalInstruments.Examples.ContGenVoltageWfmIntClk_AnalogStart
{
    /// <summary>
    /// Summary description for MainForm.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
		private Task myTask;
		private AnalogEdgeStartTriggerSlope triggerSlope;

        private System.Windows.Forms.GroupBox functionGeneratorGroupBox;
        private System.Windows.Forms.Label cyclesLabel;
        private System.Windows.Forms.Label waveformTypeLabel;
        private System.Windows.Forms.ComboBox signalTypeComboBox;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.GroupBox TimingParametersGroupBox;
        private System.Windows.Forms.TextBox frequencyTextBox;
        private System.Windows.Forms.Label frequencyLabel;
        private System.Windows.Forms.GroupBox channelParametersGroupBox;
        private System.Windows.Forms.TextBox maximumValueTextBox;
        private System.Windows.Forms.TextBox minimumValueTextBox;
        private System.Windows.Forms.Label maximumValueLabel;
        private System.Windows.Forms.Label minimumValueLabel;
        private System.Windows.Forms.Label physicalChannelLabel;
        private System.Windows.Forms.GroupBox triggerParametersGroupBox;
        internal System.Windows.Forms.RadioButton fallingRadioButton;
        internal System.Windows.Forms.RadioButton risingRadioButton;
        internal System.Windows.Forms.Label edgeLabel;
        internal System.Windows.Forms.TextBox triggerSourceTextBox;
        internal System.Windows.Forms.Label triggerSourceLabel;
        private System.Windows.Forms.Label triggerLevelLabel;
        private System.Windows.Forms.NumericUpDown triggerLevelNumericUpDown;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Label samplesPerBufferLabel;
        private System.Windows.Forms.Label amplitudeLabel;
        private System.Windows.Forms.Label triggerSourceInfoAsterisk;
        private System.Windows.Forms.Label triggerSourceInfo;
        private System.Windows.Forms.ComboBox physicalChannelComboBox;
        private System.Windows.Forms.Label hysteresisLabel;
        private System.Windows.Forms.NumericUpDown hysteresisNumeric;
        private System.Windows.Forms.NumericUpDown cyclesPerBuffer;
        private System.Windows.Forms.NumericUpDown samplesPerBuffer;
        private System.Windows.Forms.NumericUpDown amplitude;
        private System.ComponentModel.IContainer components = null;

        public MainForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            FunctionGenerator.InitComboBox(signalTypeComboBox);
            triggerSlope = AnalogEdgeStartTriggerSlope.Rising;
            startButton.Enabled = true;
            stopButton.Enabled = false;

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
            this.functionGeneratorGroupBox = new System.Windows.Forms.GroupBox();
            this.samplesPerBufferLabel = new System.Windows.Forms.Label();
            this.cyclesLabel = new System.Windows.Forms.Label();
            this.waveformTypeLabel = new System.Windows.Forms.Label();
            this.signalTypeComboBox = new System.Windows.Forms.ComboBox();
            this.amplitudeLabel = new System.Windows.Forms.Label();
            this.cyclesPerBuffer = new System.Windows.Forms.NumericUpDown();
            this.samplesPerBuffer = new System.Windows.Forms.NumericUpDown();
            this.amplitude = new System.Windows.Forms.NumericUpDown();
            this.startButton = new System.Windows.Forms.Button();
            this.TimingParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.frequencyTextBox = new System.Windows.Forms.TextBox();
            this.frequencyLabel = new System.Windows.Forms.Label();
            this.channelParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.physicalChannelComboBox = new System.Windows.Forms.ComboBox();
            this.maximumValueTextBox = new System.Windows.Forms.TextBox();
            this.minimumValueTextBox = new System.Windows.Forms.TextBox();
            this.maximumValueLabel = new System.Windows.Forms.Label();
            this.minimumValueLabel = new System.Windows.Forms.Label();
            this.physicalChannelLabel = new System.Windows.Forms.Label();
            this.triggerParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.triggerSourceInfo = new System.Windows.Forms.Label();
            this.triggerSourceInfoAsterisk = new System.Windows.Forms.Label();
            this.triggerLevelNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.triggerLevelLabel = new System.Windows.Forms.Label();
            this.fallingRadioButton = new System.Windows.Forms.RadioButton();
            this.risingRadioButton = new System.Windows.Forms.RadioButton();
            this.edgeLabel = new System.Windows.Forms.Label();
            this.triggerSourceTextBox = new System.Windows.Forms.TextBox();
            this.triggerSourceLabel = new System.Windows.Forms.Label();
            this.hysteresisLabel = new System.Windows.Forms.Label();
            this.hysteresisNumeric = new System.Windows.Forms.NumericUpDown();
            this.stopButton = new System.Windows.Forms.Button();
            this.functionGeneratorGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cyclesPerBuffer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerBuffer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.amplitude)).BeginInit();
            this.TimingParametersGroupBox.SuspendLayout();
            this.channelParametersGroupBox.SuspendLayout();
            this.triggerParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.triggerLevelNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hysteresisNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // functionGeneratorGroupBox
            // 
            this.functionGeneratorGroupBox.Controls.Add(this.samplesPerBufferLabel);
            this.functionGeneratorGroupBox.Controls.Add(this.cyclesLabel);
            this.functionGeneratorGroupBox.Controls.Add(this.waveformTypeLabel);
            this.functionGeneratorGroupBox.Controls.Add(this.signalTypeComboBox);
            this.functionGeneratorGroupBox.Controls.Add(this.amplitudeLabel);
            this.functionGeneratorGroupBox.Controls.Add(this.cyclesPerBuffer);
            this.functionGeneratorGroupBox.Controls.Add(this.samplesPerBuffer);
            this.functionGeneratorGroupBox.Controls.Add(this.amplitude);
            this.functionGeneratorGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.functionGeneratorGroupBox.Location = new System.Drawing.Point(296, 8);
            this.functionGeneratorGroupBox.Name = "functionGeneratorGroupBox";
            this.functionGeneratorGroupBox.Size = new System.Drawing.Size(240, 176);
            this.functionGeneratorGroupBox.TabIndex = 5;
            this.functionGeneratorGroupBox.TabStop = false;
            this.functionGeneratorGroupBox.Text = "Function Generator Parameters";
            // 
            // samplesPerBufferLabel
            // 
            this.samplesPerBufferLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.samplesPerBufferLabel.Location = new System.Drawing.Point(16, 96);
            this.samplesPerBufferLabel.Name = "samplesPerBufferLabel";
            this.samplesPerBufferLabel.Size = new System.Drawing.Size(96, 32);
            this.samplesPerBufferLabel.TabIndex = 4;
            this.samplesPerBufferLabel.Text = "Samples Per Buffer:";
            // 
            // cyclesLabel
            // 
            this.cyclesLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cyclesLabel.Location = new System.Drawing.Point(16, 62);
            this.cyclesLabel.Name = "cyclesLabel";
            this.cyclesLabel.Size = new System.Drawing.Size(104, 23);
            this.cyclesLabel.TabIndex = 2;
            this.cyclesLabel.Text = "Cycles Per Buffer:";
            // 
            // waveformTypeLabel
            // 
            this.waveformTypeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.waveformTypeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.waveformTypeLabel.Location = new System.Drawing.Point(16, 26);
            this.waveformTypeLabel.Name = "waveformTypeLabel";
            this.waveformTypeLabel.Size = new System.Drawing.Size(88, 14);
            this.waveformTypeLabel.TabIndex = 0;
            this.waveformTypeLabel.Text = "Waveform Type:";
            // 
            // signalTypeComboBox
            // 
            this.signalTypeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.signalTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.signalTypeComboBox.ItemHeight = 13;
            this.signalTypeComboBox.Location = new System.Drawing.Point(120, 24);
            this.signalTypeComboBox.Name = "signalTypeComboBox";
            this.signalTypeComboBox.Size = new System.Drawing.Size(104, 21);
            this.signalTypeComboBox.TabIndex = 1;
            // 
            // amplitudeLabel
            // 
            this.amplitudeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.amplitudeLabel.Location = new System.Drawing.Point(16, 136);
            this.amplitudeLabel.Name = "amplitudeLabel";
            this.amplitudeLabel.Size = new System.Drawing.Size(96, 16);
            this.amplitudeLabel.TabIndex = 6;
            this.amplitudeLabel.Text = "Amplitude:";
            // 
            // cyclesPerBuffer
            // 
            this.cyclesPerBuffer.DecimalPlaces = 1;
            this.cyclesPerBuffer.Location = new System.Drawing.Point(120, 56);
            this.cyclesPerBuffer.Minimum = new System.Decimal(new int[] {
                                                                            1,
                                                                            0,
                                                                            0,
                                                                            0});
            this.cyclesPerBuffer.Name = "cyclesPerBuffer";
            this.cyclesPerBuffer.Size = new System.Drawing.Size(104, 20);
            this.cyclesPerBuffer.TabIndex = 6;
            this.cyclesPerBuffer.Value = new System.Decimal(new int[] {
                                                                          5,
                                                                          0,
                                                                          0,
                                                                          0});
            // 
            // samplesPerBuffer
            // 
            this.samplesPerBuffer.DecimalPlaces = 1;
            this.samplesPerBuffer.Location = new System.Drawing.Point(120, 96);
            this.samplesPerBuffer.Maximum = new System.Decimal(new int[] {
                                                                             10000000,
                                                                             0,
                                                                             0,
                                                                             0});
            this.samplesPerBuffer.Minimum = new System.Decimal(new int[] {
                                                                             1,
                                                                             0,
                                                                             0,
                                                                             0});
            this.samplesPerBuffer.Name = "samplesPerBuffer";
            this.samplesPerBuffer.Size = new System.Drawing.Size(104, 20);
            this.samplesPerBuffer.TabIndex = 6;
            this.samplesPerBuffer.Value = new System.Decimal(new int[] {
                                                                           250,
                                                                           0,
                                                                           0,
                                                                           0});
            // 
            // amplitude
            // 
            this.amplitude.DecimalPlaces = 1;
            this.amplitude.Location = new System.Drawing.Point(120, 136);
            this.amplitude.Maximum = new System.Decimal(new int[] {
                                                                      10000000,
                                                                      0,
                                                                      0,
                                                                      0});
            this.amplitude.Minimum = new System.Decimal(new int[] {
                                                                      1,
                                                                      0,
                                                                      0,
                                                                      0});
            this.amplitude.Name = "amplitude";
            this.amplitude.Size = new System.Drawing.Size(104, 20);
            this.amplitude.TabIndex = 6;
            this.amplitude.Value = new System.Decimal(new int[] {
                                                                    2,
                                                                    0,
                                                                    0,
                                                                    0});
            // 
            // startButton
            // 
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.startButton.Location = new System.Drawing.Point(360, 296);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(120, 23);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // TimingParametersGroupBox
            // 
            this.TimingParametersGroupBox.Controls.Add(this.frequencyTextBox);
            this.TimingParametersGroupBox.Controls.Add(this.frequencyLabel);
            this.TimingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.TimingParametersGroupBox.Location = new System.Drawing.Point(8, 152);
            this.TimingParametersGroupBox.Name = "TimingParametersGroupBox";
            this.TimingParametersGroupBox.Size = new System.Drawing.Size(272, 64);
            this.TimingParametersGroupBox.TabIndex = 3;
            this.TimingParametersGroupBox.TabStop = false;
            this.TimingParametersGroupBox.Text = "Timing Parameters";
            // 
            // frequencyTextBox
            // 
            this.frequencyTextBox.Location = new System.Drawing.Point(136, 24);
            this.frequencyTextBox.Name = "frequencyTextBox";
            this.frequencyTextBox.Size = new System.Drawing.Size(120, 20);
            this.frequencyTextBox.TabIndex = 1;
            this.frequencyTextBox.Text = "1000";
            // 
            // frequencyLabel
            // 
            this.frequencyLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.frequencyLabel.Location = new System.Drawing.Point(16, 24);
            this.frequencyLabel.Name = "frequencyLabel";
            this.frequencyLabel.Size = new System.Drawing.Size(128, 24);
            this.frequencyLabel.TabIndex = 0;
            this.frequencyLabel.Text = "Signal Frequency (Hz):";
            // 
            // channelParametersGroupBox
            // 
            this.channelParametersGroupBox.Controls.Add(this.physicalChannelComboBox);
            this.channelParametersGroupBox.Controls.Add(this.maximumValueTextBox);
            this.channelParametersGroupBox.Controls.Add(this.minimumValueTextBox);
            this.channelParametersGroupBox.Controls.Add(this.maximumValueLabel);
            this.channelParametersGroupBox.Controls.Add(this.minimumValueLabel);
            this.channelParametersGroupBox.Controls.Add(this.physicalChannelLabel);
            this.channelParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.channelParametersGroupBox.Location = new System.Drawing.Point(8, 8);
            this.channelParametersGroupBox.Name = "channelParametersGroupBox";
            this.channelParametersGroupBox.Size = new System.Drawing.Size(272, 136);
            this.channelParametersGroupBox.TabIndex = 2;
            this.channelParametersGroupBox.TabStop = false;
            this.channelParametersGroupBox.Text = "Channel Parameters";
            // 
            // physicalChannelComboBox
            // 
            this.physicalChannelComboBox.Location = new System.Drawing.Point(136, 24);
            this.physicalChannelComboBox.Name = "physicalChannelComboBox";
            this.physicalChannelComboBox.Size = new System.Drawing.Size(121, 21);
            this.physicalChannelComboBox.TabIndex = 1;
            this.physicalChannelComboBox.Text = "Dev1/ao0";
            // 
            // maximumValueTextBox
            // 
            this.maximumValueTextBox.Location = new System.Drawing.Point(136, 96);
            this.maximumValueTextBox.Name = "maximumValueTextBox";
            this.maximumValueTextBox.Size = new System.Drawing.Size(120, 20);
            this.maximumValueTextBox.TabIndex = 5;
            this.maximumValueTextBox.Text = "10";
            // 
            // minimumValueTextBox
            // 
            this.minimumValueTextBox.Location = new System.Drawing.Point(136, 60);
            this.minimumValueTextBox.Name = "minimumValueTextBox";
            this.minimumValueTextBox.Size = new System.Drawing.Size(120, 20);
            this.minimumValueTextBox.TabIndex = 3;
            this.minimumValueTextBox.Text = "-10";
            // 
            // maximumValueLabel
            // 
            this.maximumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.maximumValueLabel.Location = new System.Drawing.Point(16, 96);
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
            // triggerParametersGroupBox
            // 
            this.triggerParametersGroupBox.Controls.Add(this.triggerSourceInfo);
            this.triggerParametersGroupBox.Controls.Add(this.triggerSourceInfoAsterisk);
            this.triggerParametersGroupBox.Controls.Add(this.triggerLevelNumericUpDown);
            this.triggerParametersGroupBox.Controls.Add(this.triggerLevelLabel);
            this.triggerParametersGroupBox.Controls.Add(this.fallingRadioButton);
            this.triggerParametersGroupBox.Controls.Add(this.risingRadioButton);
            this.triggerParametersGroupBox.Controls.Add(this.edgeLabel);
            this.triggerParametersGroupBox.Controls.Add(this.triggerSourceTextBox);
            this.triggerParametersGroupBox.Controls.Add(this.triggerSourceLabel);
            this.triggerParametersGroupBox.Controls.Add(this.hysteresisLabel);
            this.triggerParametersGroupBox.Controls.Add(this.hysteresisNumeric);
            this.triggerParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.triggerParametersGroupBox.Location = new System.Drawing.Point(8, 224);
            this.triggerParametersGroupBox.Name = "triggerParametersGroupBox";
            this.triggerParametersGroupBox.Size = new System.Drawing.Size(272, 216);
            this.triggerParametersGroupBox.TabIndex = 4;
            this.triggerParametersGroupBox.TabStop = false;
            this.triggerParametersGroupBox.Text = "Trigger Parameters";
            // 
            // triggerSourceInfo
            // 
            this.triggerSourceInfo.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.triggerSourceInfo.Location = new System.Drawing.Point(24, 152);
            this.triggerSourceInfo.Name = "triggerSourceInfo";
            this.triggerSourceInfo.Size = new System.Drawing.Size(240, 56);
            this.triggerSourceInfo.TabIndex = 8;
            this.triggerSourceInfo.Text = "APFI0 is the default Analog Trigger pin for M Series devices.  Please refer to you" +
                "r device documentation for information regarding valid Analog Triggers for your " +
                "device.";
            // 
            // triggerSourceInfoAsterisk
            // 
            this.triggerSourceInfoAsterisk.Location = new System.Drawing.Point(16, 152);
            this.triggerSourceInfoAsterisk.Name = "triggerSourceInfoAsterisk";
            this.triggerSourceInfoAsterisk.Size = new System.Drawing.Size(8, 8);
            this.triggerSourceInfoAsterisk.TabIndex = 7;
            this.triggerSourceInfoAsterisk.Text = "*";
            // 
            // triggerLevelNumericUpDown
            // 
            this.triggerLevelNumericUpDown.DecimalPlaces = 2;
            this.triggerLevelNumericUpDown.Location = new System.Drawing.Point(136, 88);
            this.triggerLevelNumericUpDown.Maximum = new System.Decimal(new int[] {
                                                                                      20,
                                                                                      0,
                                                                                      0,
                                                                                      0});
            this.triggerLevelNumericUpDown.Minimum = new System.Decimal(new int[] {
                                                                                      20,
                                                                                      0,
                                                                                      0,
                                                                                      -2147483648});
            this.triggerLevelNumericUpDown.Name = "triggerLevelNumericUpDown";
            this.triggerLevelNumericUpDown.TabIndex = 6;
            this.triggerLevelNumericUpDown.Value = new System.Decimal(new int[] {
                                                                                    5,
                                                                                    0,
                                                                                    0,
                                                                                    65536});
            // 
            // triggerLevelLabel
            // 
            this.triggerLevelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.triggerLevelLabel.Location = new System.Drawing.Point(16, 88);
            this.triggerLevelLabel.Name = "triggerLevelLabel";
            this.triggerLevelLabel.Size = new System.Drawing.Size(100, 16);
            this.triggerLevelLabel.TabIndex = 5;
            this.triggerLevelLabel.Text = "Trigger Level (V):";
            // 
            // fallingRadioButton
            // 
            this.fallingRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.fallingRadioButton.Location = new System.Drawing.Point(200, 56);
            this.fallingRadioButton.Name = "fallingRadioButton";
            this.fallingRadioButton.Size = new System.Drawing.Size(64, 24);
            this.fallingRadioButton.TabIndex = 4;
            this.fallingRadioButton.Text = "Falling";
            this.fallingRadioButton.CheckedChanged += new System.EventHandler(this.fallingRadioButton_CheckedChanged);
            // 
            // risingRadioButton
            // 
            this.risingRadioButton.Checked = true;
            this.risingRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.risingRadioButton.Location = new System.Drawing.Point(136, 56);
            this.risingRadioButton.Name = "risingRadioButton";
            this.risingRadioButton.Size = new System.Drawing.Size(64, 24);
            this.risingRadioButton.TabIndex = 3;
            this.risingRadioButton.TabStop = true;
            this.risingRadioButton.Text = "Rising";
            this.risingRadioButton.CheckedChanged += new System.EventHandler(this.risingRadioButton_CheckedChanged);
            // 
            // edgeLabel
            // 
            this.edgeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.edgeLabel.Location = new System.Drawing.Point(16, 56);
            this.edgeLabel.Name = "edgeLabel";
            this.edgeLabel.Size = new System.Drawing.Size(100, 16);
            this.edgeLabel.TabIndex = 2;
            this.edgeLabel.Text = "Trigger Slope:";
            // 
            // triggerSourceTextBox
            // 
            this.triggerSourceTextBox.Location = new System.Drawing.Point(136, 24);
            this.triggerSourceTextBox.Name = "triggerSourceTextBox";
            this.triggerSourceTextBox.Size = new System.Drawing.Size(120, 20);
            this.triggerSourceTextBox.TabIndex = 1;
            this.triggerSourceTextBox.Text = "/Dev1/APFI0";
            // 
            // triggerSourceLabel
            // 
            this.triggerSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.triggerSourceLabel.Location = new System.Drawing.Point(16, 24);
            this.triggerSourceLabel.Name = "triggerSourceLabel";
            this.triggerSourceLabel.Size = new System.Drawing.Size(128, 24);
            this.triggerSourceLabel.TabIndex = 0;
            this.triggerSourceLabel.Text = "Trigger Source:*";
            // 
            // hysteresisLabel
            // 
            this.hysteresisLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.hysteresisLabel.Location = new System.Drawing.Point(16, 120);
            this.hysteresisLabel.Name = "hysteresisLabel";
            this.hysteresisLabel.Size = new System.Drawing.Size(100, 16);
            this.hysteresisLabel.TabIndex = 5;
            this.hysteresisLabel.Text = "Hysteresis (V):";
            // 
            // hysteresisNumeric
            // 
            this.hysteresisNumeric.DecimalPlaces = 2;
            this.hysteresisNumeric.Location = new System.Drawing.Point(136, 120);
            this.hysteresisNumeric.Maximum = new System.Decimal(new int[] {
                                                                              20,
                                                                              0,
                                                                              0,
                                                                              0});
            this.hysteresisNumeric.Minimum = new System.Decimal(new int[] {
                                                                              20,
                                                                              0,
                                                                              0,
                                                                              -2147483648});
            this.hysteresisNumeric.Name = "hysteresisNumeric";
            this.hysteresisNumeric.TabIndex = 6;
            // 
            // stopButton
            // 
            this.stopButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.stopButton.Location = new System.Drawing.Point(360, 320);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(120, 23);
            this.stopButton.TabIndex = 1;
            this.stopButton.Text = "Stop";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(546, 448);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.triggerParametersGroupBox);
            this.Controls.Add(this.functionGeneratorGroupBox);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.TimingParametersGroupBox);
            this.Controls.Add(this.channelParametersGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Continuous Generate Voltage Internal Clock - Analog Start";
            this.functionGeneratorGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cyclesPerBuffer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerBuffer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.amplitude)).EndInit();
            this.TimingParametersGroupBox.ResumeLayout(false);
            this.channelParametersGroupBox.ResumeLayout(false);
            this.triggerParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.triggerLevelNumericUpDown)).EndInit();
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
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                double minV, maxV, triggerLevel;
                minV = Double.Parse(minimumValueTextBox.Text);
                maxV = Double.Parse(maximumValueTextBox.Text);
                triggerLevel = (double)triggerLevelNumericUpDown.Value;

                // Create the task and channel
                myTask = new Task();
                myTask.AOChannels.CreateVoltageChannel(
                    physicalChannelComboBox.Text,
                    "myChan",
                    minV,
                    maxV,
                    AOVoltageUnits.Volts);
                
                // Verify the task before doing the waveform calculations
                myTask.Control(TaskAction.Verify);

                // Calculate some waveform parameters and generate data
                FunctionGenerator fGen = new FunctionGenerator(
                    myTask.Timing,
                    frequencyTextBox.Text,
                    samplesPerBuffer.Text,
                    cyclesPerBuffer.Text,
                    signalTypeComboBox.Text,
                    amplitude.Text);

                // Configure the sample clock with the calculated rate
                myTask.Timing.ConfigureSampleClock(
                    "", // onboard clock
                    fGen.ResultingSampleClockRate,
                    SampleClockActiveEdge.Rising,
                    SampleQuantityMode.ContinuousSamples, 1000);

                // Setup the triggering
                myTask.Triggers.StartTrigger.ConfigureAnalogEdgeTrigger(triggerSourceTextBox.Text,
                    triggerSlope, triggerLevel);

                myTask.Triggers.StartTrigger.AnalogEdge.Hysteresis = Convert.ToDouble(hysteresisNumeric.Value);

                // Set up the Task Done event handler
                myTask.Done += new TaskDoneEventHandler(myTask_Done);

                // Write the data
                AnalogSingleChannelWriter writer = new AnalogSingleChannelWriter(myTask.Stream);
                writer.WriteMultiSample(false, fGen.Data);
                myTask.Start();

                // Set up UI
                startButton.Enabled = false;
                stopButton.Enabled = true;
                channelParametersGroupBox.Enabled = false;
                TimingParametersGroupBox.Enabled = false;
                triggerParametersGroupBox.Enabled = false;
                functionGeneratorGroupBox.Enabled = false;
            }
            catch (Exception exception)
            {
                myTask.Dispose();
                MessageBox.Show(exception.Message);

                startButton.Enabled = true;
                stopButton.Enabled = false;
                channelParametersGroupBox.Enabled = true;
                TimingParametersGroupBox.Enabled = true;
                triggerParametersGroupBox.Enabled = true;
                functionGeneratorGroupBox.Enabled = true;
            }
            Cursor.Current = Cursors.Default;
        }

        private void risingRadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            triggerSlope = AnalogEdgeStartTriggerSlope.Rising;      
        }

        private void fallingRadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            triggerSlope = AnalogEdgeStartTriggerSlope.Falling;
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
            TimingParametersGroupBox.Enabled = true;
            triggerParametersGroupBox.Enabled = true;
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
			TimingParametersGroupBox.Enabled = true;
			triggerParametersGroupBox.Enabled = true;
			functionGeneratorGroupBox.Enabled = true;
        }
    }
}
