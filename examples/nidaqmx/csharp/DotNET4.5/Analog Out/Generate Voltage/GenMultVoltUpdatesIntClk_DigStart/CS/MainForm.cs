/******************************************************************************
*
* Example program:
*   GenMultVoltUpdatesIntClk_DigStart
*
* Category:
*   AO
*
* Description:
*   This example demonstrates how to output multiple voltage updates (samples)
*   to an analog output channel.  The generation starts when a digital trigger
*   is received.
*
* Instructions for running:
*   1.  Select the physical channel corresponding to where your signal is output
*       on the DAQ device.
*   2.  Enter the minimum and maximum voltage values.
*   3.  Define the digital start trigger source and ege. In this example, the
*       source defaults to /Dev1/PFI0.
*   4.  Set the signal frequency of the waveform to output.
*   5.  Set the function generator parameters, including the waveform type,
*       cycles per buffer, samples per bufrer, and amplitude.
*
* Steps:
*   1.  Create a new task and an analog output voltage channel.
*   2.  Set up the digital start trigger for the measurement using the
*       Task.Triggers property.
*   3.  Use the FunctionGenerator class to generate waveform data and calculate
*       the sample clock rate.
*   4.  Set up the timing for the measurement. In this example we use the
*       internal DAQ Device clock to take a finite number of samples.
*   5.  Create a AnalogSingleChannelWriter, add the Task Done event handler, and
*       call the WriteMultiSample method to write multiple samples to a single
*       channel on your DAQ device. The autoStart parameter is set to false, so
*       Task.Start() must be explicitly called to begin the voltage generation.
*   6.  Call Task.Start().
*   7.  Dispose the Task object to clean-up any resources associated with the
*       task.
*   8.  Handle any DaqExceptions, if they occur.
*   9.  In the Task Done event check for any errors and Dispose the Task object
*       to clean-up any resources associated with the task.
*
* I/O Connections Overview:
*   Make sure your signal output terminal matches the text in the physical
*   channel text box and your digital trigger signal is connected to the
*   terminal specified in the trigger source text box.  The default settings
*   will start output to the ao0 pin on your DAQ Device when a digital rising
*   edge occurs on Dev1/PFI0. For more information on the input and output
*   terminals for your device, open the NI-DAQmx Help, and refer to the NI-DAQmx
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
using System.Threading;
using NationalInstruments.DAQmx;


namespace NationalInstruments.Examples.GenMultVoltUpdatesIntClk_DigStart
{
    /// <summary>
    /// Summary description for MainForm.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        internal System.Windows.Forms.Button startButton;
        internal System.Windows.Forms.GroupBox channelParametersGroupBox;
        internal System.Windows.Forms.TextBox maximumValueTextBox;
        internal System.Windows.Forms.TextBox minimumValueTextBox;
        internal System.Windows.Forms.Label maximumValueLabel;
        internal System.Windows.Forms.Label minimumValueLabel;
        internal System.Windows.Forms.Label physicalChannelLabel;
        private System.Windows.Forms.GroupBox triggerParametersGroupBox;
        internal System.Windows.Forms.Label triggerSourceLabel;
        internal System.Windows.Forms.RadioButton fallingRadioButton;
        internal System.Windows.Forms.RadioButton risingRadioButton;
        internal System.Windows.Forms.Label edgeLabel;
        private System.Windows.Forms.GroupBox TimingParametersGroupBox;
        private System.Windows.Forms.TextBox frequencyTextBox;
        private System.Windows.Forms.Label frequencyLabel;
        private System.Windows.Forms.GroupBox functionGeneratorGroupBox;
        internal System.Windows.Forms.Label amplitudeLabel;
        internal System.Windows.Forms.NumericUpDown amplitudeNumericUpDown;
        private System.Windows.Forms.Label waveformTypeLabel;
        private System.Windows.Forms.ComboBox signalTypeComboBox;
        internal System.Windows.Forms.TextBox digitalTriggerTextBox;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private System.Windows.Forms.NumericUpDown samplesPerBufferNumericUpDown;
        private System.Windows.Forms.Label samplesPerBufferLabel;
        private System.Windows.Forms.Label cyclesPerBufferLabel;
        private System.Windows.Forms.NumericUpDown cyclesPerBufferNumericUpDown;

        private DigitalEdgeStartTriggerEdge edge = DigitalEdgeStartTriggerEdge.Rising;
        private bool taskRunning = false;
        private System.Windows.Forms.ComboBox physicalChannelComboBox;
        private Task myTask;

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
            this.channelParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.physicalChannelComboBox = new System.Windows.Forms.ComboBox();
            this.maximumValueTextBox = new System.Windows.Forms.TextBox();
            this.minimumValueTextBox = new System.Windows.Forms.TextBox();
            this.maximumValueLabel = new System.Windows.Forms.Label();
            this.minimumValueLabel = new System.Windows.Forms.Label();
            this.physicalChannelLabel = new System.Windows.Forms.Label();
            this.triggerParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.fallingRadioButton = new System.Windows.Forms.RadioButton();
            this.risingRadioButton = new System.Windows.Forms.RadioButton();
            this.edgeLabel = new System.Windows.Forms.Label();
            this.digitalTriggerTextBox = new System.Windows.Forms.TextBox();
            this.triggerSourceLabel = new System.Windows.Forms.Label();
            this.TimingParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.frequencyTextBox = new System.Windows.Forms.TextBox();
            this.frequencyLabel = new System.Windows.Forms.Label();
            this.functionGeneratorGroupBox = new System.Windows.Forms.GroupBox();
            this.amplitudeLabel = new System.Windows.Forms.Label();
            this.amplitudeNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.samplesPerBufferNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.samplesPerBufferLabel = new System.Windows.Forms.Label();
            this.cyclesPerBufferLabel = new System.Windows.Forms.Label();
            this.cyclesPerBufferNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.waveformTypeLabel = new System.Windows.Forms.Label();
            this.signalTypeComboBox = new System.Windows.Forms.ComboBox();
            this.channelParametersGroupBox.SuspendLayout();
            this.triggerParametersGroupBox.SuspendLayout();
            this.TimingParametersGroupBox.SuspendLayout();
            this.functionGeneratorGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.amplitudeNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerBufferNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cyclesPerBufferNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // startButton
            // 
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.startButton.Location = new System.Drawing.Point(376, 312);
            this.startButton.Name = "startButton";
            this.startButton.TabIndex = 0;
            this.startButton.Text = "&Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
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
            this.channelParametersGroupBox.TabIndex = 1;
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
            this.minimumValueLabel.Location = new System.Drawing.Point(16, 64);
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
            this.triggerParametersGroupBox.Controls.Add(this.fallingRadioButton);
            this.triggerParametersGroupBox.Controls.Add(this.risingRadioButton);
            this.triggerParametersGroupBox.Controls.Add(this.edgeLabel);
            this.triggerParametersGroupBox.Controls.Add(this.digitalTriggerTextBox);
            this.triggerParametersGroupBox.Controls.Add(this.triggerSourceLabel);
            this.triggerParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.triggerParametersGroupBox.Location = new System.Drawing.Point(8, 232);
            this.triggerParametersGroupBox.Name = "triggerParametersGroupBox";
            this.triggerParametersGroupBox.Size = new System.Drawing.Size(272, 104);
            this.triggerParametersGroupBox.TabIndex = 3;
            this.triggerParametersGroupBox.TabStop = false;
            this.triggerParametersGroupBox.Text = "Trigger Parameters";
            // 
            // fallingRadioButton
            // 
            this.fallingRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.fallingRadioButton.Location = new System.Drawing.Point(136, 72);
            this.fallingRadioButton.Name = "fallingRadioButton";
            this.fallingRadioButton.TabIndex = 4;
            this.fallingRadioButton.Text = "Falling";
            // 
            // risingRadioButton
            // 
            this.risingRadioButton.Checked = true;
            this.risingRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.risingRadioButton.Location = new System.Drawing.Point(136, 56);
            this.risingRadioButton.Name = "risingRadioButton";
            this.risingRadioButton.Size = new System.Drawing.Size(104, 16);
            this.risingRadioButton.TabIndex = 3;
            this.risingRadioButton.TabStop = true;
            this.risingRadioButton.Text = "Rising";
            this.risingRadioButton.CheckedChanged += new System.EventHandler(this.risingRadioButton_CheckedChanged);
            // 
            // edgeLabel
            // 
            this.edgeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.edgeLabel.Location = new System.Drawing.Point(16, 64);
            this.edgeLabel.Name = "edgeLabel";
            this.edgeLabel.Size = new System.Drawing.Size(100, 16);
            this.edgeLabel.TabIndex = 2;
            this.edgeLabel.Text = "Edge:";
            // 
            // digitalTriggerTextBox
            // 
            this.digitalTriggerTextBox.Location = new System.Drawing.Point(136, 24);
            this.digitalTriggerTextBox.Name = "digitalTriggerTextBox";
            this.digitalTriggerTextBox.Size = new System.Drawing.Size(120, 20);
            this.digitalTriggerTextBox.TabIndex = 1;
            this.digitalTriggerTextBox.Text = "/Dev1/PFI0";
            // 
            // triggerSourceLabel
            // 
            this.triggerSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.triggerSourceLabel.Location = new System.Drawing.Point(16, 24);
            this.triggerSourceLabel.Name = "triggerSourceLabel";
            this.triggerSourceLabel.Size = new System.Drawing.Size(128, 24);
            this.triggerSourceLabel.TabIndex = 0;
            this.triggerSourceLabel.Text = "Trigger Source:";
            // 
            // TimingParametersGroupBox
            // 
            this.TimingParametersGroupBox.Controls.Add(this.frequencyTextBox);
            this.TimingParametersGroupBox.Controls.Add(this.frequencyLabel);
            this.TimingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.TimingParametersGroupBox.Location = new System.Drawing.Point(8, 152);
            this.TimingParametersGroupBox.Name = "TimingParametersGroupBox";
            this.TimingParametersGroupBox.Size = new System.Drawing.Size(272, 72);
            this.TimingParametersGroupBox.TabIndex = 2;
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
            // functionGeneratorGroupBox
            // 
            this.functionGeneratorGroupBox.Controls.Add(this.amplitudeLabel);
            this.functionGeneratorGroupBox.Controls.Add(this.amplitudeNumericUpDown);
            this.functionGeneratorGroupBox.Controls.Add(this.samplesPerBufferNumericUpDown);
            this.functionGeneratorGroupBox.Controls.Add(this.samplesPerBufferLabel);
            this.functionGeneratorGroupBox.Controls.Add(this.cyclesPerBufferLabel);
            this.functionGeneratorGroupBox.Controls.Add(this.cyclesPerBufferNumericUpDown);
            this.functionGeneratorGroupBox.Controls.Add(this.waveformTypeLabel);
            this.functionGeneratorGroupBox.Controls.Add(this.signalTypeComboBox);
            this.functionGeneratorGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.functionGeneratorGroupBox.Location = new System.Drawing.Point(296, 8);
            this.functionGeneratorGroupBox.Name = "functionGeneratorGroupBox";
            this.functionGeneratorGroupBox.Size = new System.Drawing.Size(232, 168);
            this.functionGeneratorGroupBox.TabIndex = 4;
            this.functionGeneratorGroupBox.TabStop = false;
            this.functionGeneratorGroupBox.Text = "Function Generator Parameters";
            // 
            // amplitudeLabel
            // 
            this.amplitudeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.amplitudeLabel.Location = new System.Drawing.Point(16, 136);
            this.amplitudeLabel.Name = "amplitudeLabel";
            this.amplitudeLabel.Size = new System.Drawing.Size(104, 23);
            this.amplitudeLabel.TabIndex = 6;
            this.amplitudeLabel.Text = "Amplitude:";
            // 
            // amplitudeNumericUpDown
            // 
            this.amplitudeNumericUpDown.DecimalPlaces = 1;
            this.amplitudeNumericUpDown.Location = new System.Drawing.Point(120, 136);
            this.amplitudeNumericUpDown.Minimum = new System.Decimal(new int[] {
                                                                                   1,
                                                                                   0,
                                                                                   0,
                                                                                   0});
            this.amplitudeNumericUpDown.Name = "amplitudeNumericUpDown";
            this.amplitudeNumericUpDown.Size = new System.Drawing.Size(104, 20);
            this.amplitudeNumericUpDown.TabIndex = 7;
            this.amplitudeNumericUpDown.Value = new System.Decimal(new int[] {
                                                                                 2,
                                                                                 0,
                                                                                 0,
                                                                                 0});
            // 
            // samplesPerBufferNumericUpDown
            // 
            this.samplesPerBufferNumericUpDown.Location = new System.Drawing.Point(120, 96);
            this.samplesPerBufferNumericUpDown.Maximum = new System.Decimal(new int[] {
                                                                                          10000000,
                                                                                          0,
                                                                                          0,
                                                                                          0});
            this.samplesPerBufferNumericUpDown.Minimum = new System.Decimal(new int[] {
                                                                                          1,
                                                                                          0,
                                                                                          0,
                                                                                          0});
            this.samplesPerBufferNumericUpDown.Name = "samplesPerBufferNumericUpDown";
            this.samplesPerBufferNumericUpDown.Size = new System.Drawing.Size(104, 20);
            this.samplesPerBufferNumericUpDown.TabIndex = 5;
            this.samplesPerBufferNumericUpDown.Value = new System.Decimal(new int[] {
                                                                                        250,
                                                                                        0,
                                                                                        0,
                                                                                        0});
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
            // cyclesPerBufferLabel
            // 
            this.cyclesPerBufferLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cyclesPerBufferLabel.Location = new System.Drawing.Point(16, 64);
            this.cyclesPerBufferLabel.Name = "cyclesPerBufferLabel";
            this.cyclesPerBufferLabel.Size = new System.Drawing.Size(104, 18);
            this.cyclesPerBufferLabel.TabIndex = 2;
            this.cyclesPerBufferLabel.Text = "Cycles Per Buffer:";
            // 
            // cyclesPerBufferNumericUpDown
            // 
            this.cyclesPerBufferNumericUpDown.DecimalPlaces = 1;
            this.cyclesPerBufferNumericUpDown.Location = new System.Drawing.Point(120, 60);
            this.cyclesPerBufferNumericUpDown.Minimum = new System.Decimal(new int[] {
                                                                                         1,
                                                                                         0,
                                                                                         0,
                                                                                         0});
            this.cyclesPerBufferNumericUpDown.Name = "cyclesPerBufferNumericUpDown";
            this.cyclesPerBufferNumericUpDown.Size = new System.Drawing.Size(104, 20);
            this.cyclesPerBufferNumericUpDown.TabIndex = 3;
            this.cyclesPerBufferNumericUpDown.Value = new System.Decimal(new int[] {
                                                                                       50,
                                                                                       0,
                                                                                       0,
                                                                                       65536});
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
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(538, 344);
            this.Controls.Add(this.functionGeneratorGroupBox);
            this.Controls.Add(this.triggerParametersGroupBox);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.channelParametersGroupBox);
            this.Controls.Add(this.TimingParametersGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Generate Multiple Volt Updates IntClk-Digital Start";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.OnClosing);
            this.channelParametersGroupBox.ResumeLayout(false);
            this.triggerParametersGroupBox.ResumeLayout(false);
            this.TimingParametersGroupBox.ResumeLayout(false);
            this.functionGeneratorGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.amplitudeNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerBufferNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cyclesPerBufferNumericUpDown)).EndInit();
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
            taskRunning = true;

            try
            {
                // Create the task and channel
                myTask = new Task();
            
                myTask.AOChannels.CreateVoltageChannel(
                    physicalChannelComboBox.Text,
                    "aoChannel",
                    Double.Parse(minimumValueTextBox.Text),
                    Double.Parse(maximumValueTextBox.Text),
                    AOVoltageUnits.Volts);

                // Verify the task before doing the waveform calculations
                myTask.Control(TaskAction.Verify);

                // Calculate some waveform parameters and generate data
                FunctionGenerator fGen = new FunctionGenerator(
                    myTask.Timing,
                    Double.Parse(frequencyTextBox.Text),
                    (double)samplesPerBufferNumericUpDown.Value,
                    (double)cyclesPerBufferNumericUpDown.Value,
                    (WaveformType)signalTypeComboBox.SelectedIndex,
                    (double)amplitudeNumericUpDown.Value);

                // Configure the sample clock with the calculated rate
                myTask.Timing.ConfigureSampleClock(
                    "", // onboard clock
                    fGen.ResultingSampleClockRate,
                    SampleClockActiveEdge.Rising,
                    SampleQuantityMode.FiniteSamples,
                    fGen.Data.Length);

                // Setup the triggering
                myTask.Triggers.StartTrigger.ConfigureDigitalEdgeTrigger(digitalTriggerTextBox.Text, edge);
        
                AnalogSingleChannelWriter writer = new AnalogSingleChannelWriter(myTask.Stream);

                // Setup the Task Done event
                myTask.Done += new TaskDoneEventHandler(myTask_Done);

                writer.WriteMultiSample(false, fGen.Data);
                myTask.Start();
            }
            catch(Exception x)
            {
                MessageBox.Show(x.Message);

                if (myTask != null)
                {
                    myTask.Dispose();
                }
                
                taskRunning = false;
                startButton.Enabled = true;
            }
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

        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = taskRunning;
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

            taskRunning = false;
            startButton.Enabled = true;
        }
    }
}
