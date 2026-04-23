/******************************************************************************
*
* Example program:
*   GenDigPulseTrain_ContBuff_ExtClk
*
* Category:
*   CO
*
* Description:
*   This example demonstrates how to generate a continuous buffered sample
*   clocked digital pulse train from a Counter Output Channel. The Frequency,
*   Duty Cycle, and Idle State are all configurable. The default data generated
*   is a pulse train with a fixed frequency but a duty cycle that varies based
*   on the Duty Cycle Max/Min and the signal type. The duty cycle will update
*   with each sample clock edge.
*
* Instructions for running:
*   1.  Enter the physical channel of the counter you want to output your signal
*       to on the DAQ device.
*   2.  Enter the Sample Clock Rate, samples per channel and clock source to
*       configure timing. Note: The sample clock rate should be less than half
*       the output PWM Frequency to avoid an over run error.
*   3.  Enter the frequency and a minimum duty cycle to define the initial pulse
*       parameters.
*   4.  Enter the signal type, minimum and maximum duty cycle to define the
*       pulse train generated.
*
* Steps:
*   1.  Create a counter output channel to produce a pulse in terms of
*       frequency.  If the idle state of the pulse is set to low, the first
*       transition of the generated signal is from low to high.
*   2.  Use the Task.Timing.ConfigureImplicit method to configure the duration
*       of the pulse generation.
*   3.  Write the array of frequency and duty cycle specifications to the output
*       buffer.
*   4.  Start the task to arm the counter and begin the pulse train generation.
*   5.  For continuous generation, the counter will continuously generate the
*       pulse train until the Stop button is pressed.
*   6.  Dispose the Task object to clean-up any resources associated with the
*       task.
*   7.  Handle any DaqExceptions, if they occur.
*
* I/O Connections Overview:
*   This example will cause the counter to output the pulse on the output
*   terminal of the counter specified. The default counter terminal(s) depend on
*   the type of measurement being taken. For more information on the default
*   counter input and output terminals for your device, open the NI-DAQmx Help,
*   and refer to Counter Signal Connections found under the Device
*   Considerations book in the table of contents.
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
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using NationalInstruments.DAQmx;


namespace NationalInstruments.Examples.GenDigPulseTrain_ContBuff_ExtClk
{
    /// <summary>
    /// Summary description for MainForm.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.GroupBox channelParameterGroupBox;
        private System.Windows.Forms.Label physicalChannelLabel;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private Task myTask;
        private COPulseIdleState idleState;
        private System.Windows.Forms.GroupBox idleStateGroupBox;
        private System.Windows.Forms.RadioButton highRadioButton;
        private System.Windows.Forms.RadioButton lowRadioButton;
        private System.Windows.Forms.ComboBox counterComboBox;
        private GroupBox timingParametersGroupBox;
        internal Label clockSourceLabel;
        internal TextBox clockSourceTextBox;
        internal Label rateLabel;
        internal Label samplesLabel;
        internal NumericUpDown samplesPerChannelNumeric;
        internal NumericUpDown rateNumeric;
        private GroupBox pwmParametersGroupBox;
        private TextBox dutyCycleMaxTextBox;
        private Label dutyCycleMaxLabel;
        private TextBox dutyCycleMinTextBox;
        private Label dutyCycleMinLabel;
        private TextBox frequencyTextBox;
        private Label frequencyLabel;
        private System.ComponentModel.IContainer components;

        public MainForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            idleState = COPulseIdleState.Low;
            
            counterComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.CO, PhysicalChannelAccess.External));
            if (counterComboBox.Items.Count > 0)
                counterComboBox.SelectedIndex = 0;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.channelParameterGroupBox = new System.Windows.Forms.GroupBox();
            this.counterComboBox = new System.Windows.Forms.ComboBox();
            this.idleStateGroupBox = new System.Windows.Forms.GroupBox();
            this.highRadioButton = new System.Windows.Forms.RadioButton();
            this.lowRadioButton = new System.Windows.Forms.RadioButton();
            this.physicalChannelLabel = new System.Windows.Forms.Label();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.timingParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.clockSourceLabel = new System.Windows.Forms.Label();
            this.clockSourceTextBox = new System.Windows.Forms.TextBox();
            this.rateLabel = new System.Windows.Forms.Label();
            this.samplesLabel = new System.Windows.Forms.Label();
            this.samplesPerChannelNumeric = new System.Windows.Forms.NumericUpDown();
            this.rateNumeric = new System.Windows.Forms.NumericUpDown();
            this.pwmParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.dutyCycleMinTextBox = new System.Windows.Forms.TextBox();
            this.dutyCycleMinLabel = new System.Windows.Forms.Label();
            this.frequencyTextBox = new System.Windows.Forms.TextBox();
            this.frequencyLabel = new System.Windows.Forms.Label();
            this.dutyCycleMaxTextBox = new System.Windows.Forms.TextBox();
            this.dutyCycleMaxLabel = new System.Windows.Forms.Label();
            this.channelParameterGroupBox.SuspendLayout();
            this.idleStateGroupBox.SuspendLayout();
            this.timingParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerChannelNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rateNumeric)).BeginInit();
            this.pwmParametersGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // channelParameterGroupBox
            // 
            this.channelParameterGroupBox.Controls.Add(this.counterComboBox);
            this.channelParameterGroupBox.Controls.Add(this.idleStateGroupBox);
            this.channelParameterGroupBox.Controls.Add(this.physicalChannelLabel);
            this.channelParameterGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.channelParameterGroupBox.Location = new System.Drawing.Point(8, 8);
            this.channelParameterGroupBox.Name = "channelParameterGroupBox";
            this.channelParameterGroupBox.Size = new System.Drawing.Size(250, 117);
            this.channelParameterGroupBox.TabIndex = 2;
            this.channelParameterGroupBox.TabStop = false;
            this.channelParameterGroupBox.Text = "Channel Parameters:";
            // 
            // counterComboBox
            // 
            this.counterComboBox.Location = new System.Drawing.Point(112, 16);
            this.counterComboBox.Name = "counterComboBox";
            this.counterComboBox.Size = new System.Drawing.Size(132, 21);
            this.counterComboBox.TabIndex = 1;
            this.counterComboBox.Text = "Dev1/ctr0";
            // 
            // idleStateGroupBox
            // 
            this.idleStateGroupBox.Controls.Add(this.highRadioButton);
            this.idleStateGroupBox.Controls.Add(this.lowRadioButton);
            this.idleStateGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.idleStateGroupBox.Location = new System.Drawing.Point(6, 43);
            this.idleStateGroupBox.Name = "idleStateGroupBox";
            this.idleStateGroupBox.Size = new System.Drawing.Size(238, 64);
            this.idleStateGroupBox.TabIndex = 6;
            this.idleStateGroupBox.TabStop = false;
            this.idleStateGroupBox.Text = "Idle State:";
            // 
            // highRadioButton
            // 
            this.highRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.highRadioButton.Location = new System.Drawing.Point(123, 24);
            this.highRadioButton.Name = "highRadioButton";
            this.highRadioButton.Size = new System.Drawing.Size(64, 24);
            this.highRadioButton.TabIndex = 1;
            this.highRadioButton.Text = "High";
            this.highRadioButton.CheckedChanged += new System.EventHandler(this.highRadioButton_CheckedChanged);
            // 
            // lowRadioButton
            // 
            this.lowRadioButton.Checked = true;
            this.lowRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lowRadioButton.Location = new System.Drawing.Point(51, 24);
            this.lowRadioButton.Name = "lowRadioButton";
            this.lowRadioButton.Size = new System.Drawing.Size(64, 24);
            this.lowRadioButton.TabIndex = 0;
            this.lowRadioButton.TabStop = true;
            this.lowRadioButton.Text = "Low";
            this.lowRadioButton.CheckedChanged += new System.EventHandler(this.lowRadioButton_CheckedChanged);
            // 
            // physicalChannelLabel
            // 
            this.physicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.physicalChannelLabel.Location = new System.Drawing.Point(8, 23);
            this.physicalChannelLabel.Name = "physicalChannelLabel";
            this.physicalChannelLabel.Size = new System.Drawing.Size(72, 16);
            this.physicalChannelLabel.TabIndex = 0;
            this.physicalChannelLabel.Text = "Counter(s):";
            // 
            // startButton
            // 
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.startButton.Location = new System.Drawing.Point(264, 17);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(96, 32);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.stopButton.Location = new System.Drawing.Point(264, 55);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(96, 32);
            this.stopButton.TabIndex = 1;
            this.stopButton.Text = "Stop";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // timingParametersGroupBox
            // 
            this.timingParametersGroupBox.Controls.Add(this.clockSourceLabel);
            this.timingParametersGroupBox.Controls.Add(this.clockSourceTextBox);
            this.timingParametersGroupBox.Controls.Add(this.rateLabel);
            this.timingParametersGroupBox.Controls.Add(this.samplesLabel);
            this.timingParametersGroupBox.Controls.Add(this.samplesPerChannelNumeric);
            this.timingParametersGroupBox.Controls.Add(this.rateNumeric);
            this.timingParametersGroupBox.Location = new System.Drawing.Point(8, 131);
            this.timingParametersGroupBox.Name = "timingParametersGroupBox";
            this.timingParametersGroupBox.Size = new System.Drawing.Size(250, 114);
            this.timingParametersGroupBox.TabIndex = 3;
            this.timingParametersGroupBox.TabStop = false;
            this.timingParametersGroupBox.Text = "Timing Parameters:";
            // 
            // clockSourceLabel
            // 
            this.clockSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.clockSourceLabel.Location = new System.Drawing.Point(8, 21);
            this.clockSourceLabel.Name = "clockSourceLabel";
            this.clockSourceLabel.Size = new System.Drawing.Size(80, 16);
            this.clockSourceLabel.TabIndex = 6;
            this.clockSourceLabel.Text = "Clock Source:";
            // 
            // clockSourceTextBox
            // 
            this.clockSourceTextBox.Location = new System.Drawing.Point(112, 19);
            this.clockSourceTextBox.Name = "clockSourceTextBox";
            this.clockSourceTextBox.Size = new System.Drawing.Size(132, 20);
            this.clockSourceTextBox.TabIndex = 7;
            this.clockSourceTextBox.Text = "/Dev1/PFI7";
            // 
            // rateLabel
            // 
            this.rateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rateLabel.Location = new System.Drawing.Point(8, 85);
            this.rateLabel.Name = "rateLabel";
            this.rateLabel.Size = new System.Drawing.Size(64, 16);
            this.rateLabel.TabIndex = 10;
            this.rateLabel.Text = "Rate (Hz):";
            // 
            // samplesLabel
            // 
            this.samplesLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.samplesLabel.Location = new System.Drawing.Point(8, 53);
            this.samplesLabel.Name = "samplesLabel";
            this.samplesLabel.Size = new System.Drawing.Size(104, 16);
            this.samplesLabel.TabIndex = 8;
            this.samplesLabel.Text = "Samples / Channel:";
            // 
            // samplesPerChannelNumeric
            // 
            this.samplesPerChannelNumeric.Location = new System.Drawing.Point(112, 51);
            this.samplesPerChannelNumeric.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.samplesPerChannelNumeric.Name = "samplesPerChannelNumeric";
            this.samplesPerChannelNumeric.Size = new System.Drawing.Size(132, 20);
            this.samplesPerChannelNumeric.TabIndex = 9;
            this.samplesPerChannelNumeric.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // rateNumeric
            // 
            this.rateNumeric.DecimalPlaces = 2;
            this.rateNumeric.Location = new System.Drawing.Point(112, 83);
            this.rateNumeric.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.rateNumeric.Name = "rateNumeric";
            this.rateNumeric.Size = new System.Drawing.Size(132, 20);
            this.rateNumeric.TabIndex = 11;
            this.rateNumeric.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // pwmParametersGroupBox
            // 
            this.pwmParametersGroupBox.Controls.Add(this.dutyCycleMaxTextBox);
            this.pwmParametersGroupBox.Controls.Add(this.dutyCycleMaxLabel);
            this.pwmParametersGroupBox.Controls.Add(this.dutyCycleMinTextBox);
            this.pwmParametersGroupBox.Controls.Add(this.dutyCycleMinLabel);
            this.pwmParametersGroupBox.Controls.Add(this.frequencyTextBox);
            this.pwmParametersGroupBox.Controls.Add(this.frequencyLabel);
            this.pwmParametersGroupBox.Location = new System.Drawing.Point(8, 251);
            this.pwmParametersGroupBox.Name = "pwmParametersGroupBox";
            this.pwmParametersGroupBox.Size = new System.Drawing.Size(250, 110);
            this.pwmParametersGroupBox.TabIndex = 4;
            this.pwmParametersGroupBox.TabStop = false;
            this.pwmParametersGroupBox.Text = "Pulse-width Modulation Parameters:";
            // 
            // dutyCycleMinTextBox
            // 
            this.dutyCycleMinTextBox.Location = new System.Drawing.Point(112, 51);
            this.dutyCycleMinTextBox.Name = "dutyCycleMinTextBox";
            this.dutyCycleMinTextBox.Size = new System.Drawing.Size(132, 20);
            this.dutyCycleMinTextBox.TabIndex = 9;
            this.dutyCycleMinTextBox.Text = "0.5";
            // 
            // dutyCycleMinLabel
            // 
            this.dutyCycleMinLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.dutyCycleMinLabel.Location = new System.Drawing.Point(8, 55);
            this.dutyCycleMinLabel.Name = "dutyCycleMinLabel";
            this.dutyCycleMinLabel.Size = new System.Drawing.Size(104, 16);
            this.dutyCycleMinLabel.TabIndex = 8;
            this.dutyCycleMinLabel.Text = "Duty Cycle Min:";
            // 
            // frequencyTextBox
            // 
            this.frequencyTextBox.Location = new System.Drawing.Point(112, 19);
            this.frequencyTextBox.Name = "frequencyTextBox";
            this.frequencyTextBox.Size = new System.Drawing.Size(132, 20);
            this.frequencyTextBox.TabIndex = 7;
            this.frequencyTextBox.Text = "1000.0";
            // 
            // frequencyLabel
            // 
            this.frequencyLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.frequencyLabel.Location = new System.Drawing.Point(8, 24);
            this.frequencyLabel.Name = "frequencyLabel";
            this.frequencyLabel.Size = new System.Drawing.Size(120, 16);
            this.frequencyLabel.TabIndex = 6;
            this.frequencyLabel.Text = "Frequency (Hz):";
            // 
            // dutyCycleMaxTextBox
            // 
            this.dutyCycleMaxTextBox.Location = new System.Drawing.Point(112, 82);
            this.dutyCycleMaxTextBox.Name = "dutyCycleMaxTextBox";
            this.dutyCycleMaxTextBox.Size = new System.Drawing.Size(132, 20);
            this.dutyCycleMaxTextBox.TabIndex = 11;
            this.dutyCycleMaxTextBox.Text = "0.8";
            // 
            // dutyCycleMaxLabel
            // 
            this.dutyCycleMaxLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.dutyCycleMaxLabel.Location = new System.Drawing.Point(6, 86);
            this.dutyCycleMaxLabel.Name = "dutyCycleMaxLabel";
            this.dutyCycleMaxLabel.Size = new System.Drawing.Size(104, 16);
            this.dutyCycleMaxLabel.TabIndex = 10;
            this.dutyCycleMaxLabel.Text = "Duty Cycle Max:";
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(372, 370);
            this.Controls.Add(this.pwmParametersGroupBox);
            this.Controls.Add(this.timingParametersGroupBox);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.channelParameterGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Digital Pulse Train - Continuous - Buffered";
            this.channelParameterGroupBox.ResumeLayout(false);
            this.idleStateGroupBox.ResumeLayout(false);
            this.timingParametersGroupBox.ResumeLayout(false);
            this.timingParametersGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerChannelNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rateNumeric)).EndInit();
            this.pwmParametersGroupBox.ResumeLayout(false);
            this.pwmParametersGroupBox.PerformLayout();
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
            // This example uses the default source (or gate) terminal for 
            // the counter of your device.  To determine what the default 
            // counter pins for your device are or to set a different source 
            // (or gate) pin, refer to the Connecting Counter Signals topic
            // in the NI-DAQmx Help (search for "Connecting Counter Signals").

            int samplesPerChannel = Convert.ToInt32(samplesPerChannelNumeric.Value);
            double rate = Convert.ToDouble(rateNumeric.Value);
            double frequency = Convert.ToDouble(frequencyTextBox.Text);
            double dutyCycleMin = Convert.ToDouble(dutyCycleMinTextBox.Text);
            double dutyCycleMax = Convert.ToDouble(dutyCycleMaxTextBox.Text);
            double dutyStep = (dutyCycleMax - dutyCycleMin) / samplesPerChannel;

            try
            {
                CODataFrequency [] data = new CODataFrequency[samplesPerChannel];
                for (int i = 0; i < data.Length; i++)
                    data[i] = new CODataFrequency(frequency, dutyCycleMin + dutyStep * i);

                myTask = new Task();

                myTask.COChannels.CreatePulseChannelFrequency(counterComboBox.Text, 
                    "ContinuousPulseTrain", COPulseFrequencyUnits.Hertz, idleState, 0.0, 
                    frequency, 
                    dutyCycleMin);

                myTask.Timing.ConfigureSampleClock(clockSourceTextBox.Text, rate,
                    SampleClockActiveEdge.Rising, SampleQuantityMode.ContinuousSamples);

                CounterSingleChannelWriter writer = new CounterSingleChannelWriter(myTask.Stream);
                writer.WriteMultiSample(false, data);
                
                myTask.Done += OnTaskDone;
                myTask.Start();

                startButton.Enabled = false;
                stopButton.Enabled = true;
            }
            catch(Exception exception)
            {
                MessageBox.Show(exception.Message);
                myTask.Dispose();
                startButton.Enabled = true;
                stopButton.Enabled = false;
            }
        }
    
        private void stopButton_Click(object sender, System.EventArgs e)
        {
            myTask.Stop();
            myTask.Dispose();
            startButton.Enabled = true;
            stopButton.Enabled = false;
        }

        private void lowRadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            idleState = COPulseIdleState.Low;
        }

        private void highRadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            idleState = COPulseIdleState.High;
        }

        private void OnTaskDone(object sender, TaskDoneEventArgs e)
        {
            try
            {
                stopButton.Enabled = false;
                startButton.Enabled = true;
                e.CheckForException();
                myTask.Stop();
                myTask.Dispose();
                myTask = null;
            }
            catch (DaqException exception)
            {
                // Display Errors
                MessageBox.Show(exception.Message);
                myTask.Stop();
                myTask.Dispose();
                myTask = null;
            }
        }
    }
}
