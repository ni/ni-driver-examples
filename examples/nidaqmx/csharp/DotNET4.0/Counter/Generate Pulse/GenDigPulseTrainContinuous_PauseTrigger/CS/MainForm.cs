/******************************************************************************
*
* Example program:
*   GenDigPulseTrainContinuous_PauseTrigger
*
* Category:
*   CO
*
* Description:
*   This example demonstrates how to generate a continuous digital pulse train
*   from a counter output channel and controlled by an external digital pause
*   trigger.  The frequency, duty cycle, and idle state are all
*   configurable.This example shows how to configure the pulse in terms of
*   frequency and duty cycle, but can easily be modified to generate a pulse in
*   terms of time or ticks.
*
* Instructions for running:
*   1.  Enter the physical channel which corresponds to the counter you want to
*       output your signal to on the DAQ device.
*   2.  Enter the frequency and duty cycle to define the pulse parameters. You
*       can also change the idle state to set the state the line will remain in
*       after the generation is stopped.
*   3.  Setup the pause trigger parameters.
*
* Steps:
*   1.  Create a counter output channel to produce a pulse in terms of
*       frequency.  If the idle state of the pulse is set to low the first
*       transition of the generated signal is from low to high.
*   2.  Use the PauseTrigger object properties to configure a pause trigger.
*   3.  Use the ConfigureImplicit() method to configure the duration of the
*       pulse generation.
*   4.  Call the Start() method to arm the counter and begin the pulse train
*       generation.
*   5.  This example will continue to generate the pulse train until the Stop
*       button is  pressed on the user interface.
*   6.  Call Stop() to stop the task and Dispose() to to clear any resources
*       allocated by the task.
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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using NationalInstruments.DAQmx;

namespace NationalInstruments.Examples.GenDigPulseTrainContinuous_PauseTrigger
{
    /// <summary>
    /// Summary description for MainForm.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.GroupBox channelParameterGroupBox;
        private System.Windows.Forms.TextBox dutyCycleTextBox;
        private System.Windows.Forms.Label dutyCycleLabel;
        private System.Windows.Forms.TextBox frequencyTextBox;
        private System.Windows.Forms.Label frequencyLabel;
        private System.Windows.Forms.Label physicalChannelLabel;
        private System.Windows.Forms.GroupBox triggerParameterGroupBox;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.Label triggerSourceLabel;
        private System.Windows.Forms.GroupBox idleStateGroupBox;
        private Task myTask;
        private System.Windows.Forms.TextBox pauseTrigSourceTextBox;
        private System.Windows.Forms.GroupBox pauseConditionGroupBox;
        private System.Windows.Forms.RadioButton idleStateHighRadioButton;
        private System.Windows.Forms.RadioButton idleStateLowRadioButton;
        private System.Windows.Forms.RadioButton pauseWhenHighRadioButton;
        private System.Windows.Forms.RadioButton pauseWhenLowRadioButton;
        private COPulseIdleState idleState;
        private System.Windows.Forms.Timer statusCheckTimer;
        private System.Windows.Forms.ComboBox counterComboBox;
        private DigitalLevelPauseTriggerCondition pauseCondition;

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
            pauseCondition = DigitalLevelPauseTriggerCondition.Low;

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
            this.components = new System.ComponentModel.Container();
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainForm));
            this.stopButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.channelParameterGroupBox = new System.Windows.Forms.GroupBox();
            this.counterComboBox = new System.Windows.Forms.ComboBox();
            this.idleStateGroupBox = new System.Windows.Forms.GroupBox();
            this.idleStateHighRadioButton = new System.Windows.Forms.RadioButton();
            this.idleStateLowRadioButton = new System.Windows.Forms.RadioButton();
            this.dutyCycleTextBox = new System.Windows.Forms.TextBox();
            this.dutyCycleLabel = new System.Windows.Forms.Label();
            this.frequencyTextBox = new System.Windows.Forms.TextBox();
            this.frequencyLabel = new System.Windows.Forms.Label();
            this.physicalChannelLabel = new System.Windows.Forms.Label();
            this.triggerParameterGroupBox = new System.Windows.Forms.GroupBox();
            this.pauseWhenHighRadioButton = new System.Windows.Forms.RadioButton();
            this.pauseWhenLowRadioButton = new System.Windows.Forms.RadioButton();
            this.triggerSourceLabel = new System.Windows.Forms.Label();
            this.pauseTrigSourceTextBox = new System.Windows.Forms.TextBox();
            this.pauseConditionGroupBox = new System.Windows.Forms.GroupBox();
            this.statusCheckTimer = new System.Windows.Forms.Timer(this.components);
            this.channelParameterGroupBox.SuspendLayout();
            this.idleStateGroupBox.SuspendLayout();
            this.triggerParameterGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.stopButton.Location = new System.Drawing.Point(240, 224);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(96, 32);
            this.stopButton.TabIndex = 1;
            this.stopButton.Text = "Stop";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // startButton
            // 
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.startButton.Location = new System.Drawing.Point(240, 192);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(96, 32);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // channelParameterGroupBox
            // 
            this.channelParameterGroupBox.Controls.Add(this.counterComboBox);
            this.channelParameterGroupBox.Controls.Add(this.idleStateGroupBox);
            this.channelParameterGroupBox.Controls.Add(this.dutyCycleTextBox);
            this.channelParameterGroupBox.Controls.Add(this.dutyCycleLabel);
            this.channelParameterGroupBox.Controls.Add(this.frequencyTextBox);
            this.channelParameterGroupBox.Controls.Add(this.frequencyLabel);
            this.channelParameterGroupBox.Controls.Add(this.physicalChannelLabel);
            this.channelParameterGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.channelParameterGroupBox.Location = new System.Drawing.Point(8, 8);
            this.channelParameterGroupBox.Name = "channelParameterGroupBox";
            this.channelParameterGroupBox.Size = new System.Drawing.Size(184, 270);
            this.channelParameterGroupBox.TabIndex = 2;
            this.channelParameterGroupBox.TabStop = false;
            this.channelParameterGroupBox.Text = "Channel Parameters:";
            // 
            // counterComboBox
            // 
            this.counterComboBox.Location = new System.Drawing.Point(16, 48);
            this.counterComboBox.Name = "counterComboBox";
            this.counterComboBox.Size = new System.Drawing.Size(152, 21);
            this.counterComboBox.TabIndex = 1;
            this.counterComboBox.Text = "Dev1/ctr0";
            // 
            // idleStateGroupBox
            // 
            this.idleStateGroupBox.Controls.Add(this.idleStateHighRadioButton);
            this.idleStateGroupBox.Controls.Add(this.idleStateLowRadioButton);
            this.idleStateGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.idleStateGroupBox.Location = new System.Drawing.Point(16, 192);
            this.idleStateGroupBox.Name = "idleStateGroupBox";
            this.idleStateGroupBox.Size = new System.Drawing.Size(152, 64);
            this.idleStateGroupBox.TabIndex = 6;
            this.idleStateGroupBox.TabStop = false;
            this.idleStateGroupBox.Text = "Idle State:";
            // 
            // idleStateHighRadioButton
            // 
            this.idleStateHighRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.idleStateHighRadioButton.Location = new System.Drawing.Point(19, 37);
            this.idleStateHighRadioButton.Name = "idleStateHighRadioButton";
            this.idleStateHighRadioButton.Size = new System.Drawing.Size(56, 16);
            this.idleStateHighRadioButton.TabIndex = 1;
            this.idleStateHighRadioButton.Text = "High";
            this.idleStateHighRadioButton.CheckedChanged += new System.EventHandler(this.idleStateHighRadioButton_CheckedChanged);
            // 
            // idleStateLowRadioButton
            // 
            this.idleStateLowRadioButton.Checked = true;
            this.idleStateLowRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.idleStateLowRadioButton.Location = new System.Drawing.Point(19, 21);
            this.idleStateLowRadioButton.Name = "idleStateLowRadioButton";
            this.idleStateLowRadioButton.Size = new System.Drawing.Size(56, 16);
            this.idleStateLowRadioButton.TabIndex = 0;
            this.idleStateLowRadioButton.TabStop = true;
            this.idleStateLowRadioButton.Text = "Low";
            this.idleStateLowRadioButton.CheckedChanged += new System.EventHandler(this.idleStateLowRadioButton_CheckedChanged);
            // 
            // dutyCycleTextBox
            // 
            this.dutyCycleTextBox.Location = new System.Drawing.Point(16, 160);
            this.dutyCycleTextBox.Name = "dutyCycleTextBox";
            this.dutyCycleTextBox.Size = new System.Drawing.Size(152, 20);
            this.dutyCycleTextBox.TabIndex = 5;
            this.dutyCycleTextBox.Text = "0.5";
            // 
            // dutyCycleLabel
            // 
            this.dutyCycleLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.dutyCycleLabel.Location = new System.Drawing.Point(16, 141);
            this.dutyCycleLabel.Name = "dutyCycleLabel";
            this.dutyCycleLabel.Size = new System.Drawing.Size(100, 16);
            this.dutyCycleLabel.TabIndex = 4;
            this.dutyCycleLabel.Text = "Duty Cycle:";
            // 
            // frequencyTextBox
            // 
            this.frequencyTextBox.Location = new System.Drawing.Point(16, 104);
            this.frequencyTextBox.Name = "frequencyTextBox";
            this.frequencyTextBox.Size = new System.Drawing.Size(152, 20);
            this.frequencyTextBox.TabIndex = 3;
            this.frequencyTextBox.Text = "1.0";
            // 
            // frequencyLabel
            // 
            this.frequencyLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.frequencyLabel.Location = new System.Drawing.Point(16, 85);
            this.frequencyLabel.Name = "frequencyLabel";
            this.frequencyLabel.Size = new System.Drawing.Size(100, 16);
            this.frequencyLabel.TabIndex = 2;
            this.frequencyLabel.Text = "Frequency (Hz):";
            // 
            // physicalChannelLabel
            // 
            this.physicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.physicalChannelLabel.Location = new System.Drawing.Point(16, 32);
            this.physicalChannelLabel.Name = "physicalChannelLabel";
            this.physicalChannelLabel.Size = new System.Drawing.Size(100, 16);
            this.physicalChannelLabel.TabIndex = 0;
            this.physicalChannelLabel.Text = "Counter(s):";
            // 
            // triggerParameterGroupBox
            // 
            this.triggerParameterGroupBox.Controls.Add(this.pauseWhenHighRadioButton);
            this.triggerParameterGroupBox.Controls.Add(this.pauseWhenLowRadioButton);
            this.triggerParameterGroupBox.Controls.Add(this.triggerSourceLabel);
            this.triggerParameterGroupBox.Controls.Add(this.pauseTrigSourceTextBox);
            this.triggerParameterGroupBox.Controls.Add(this.pauseConditionGroupBox);
            this.triggerParameterGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.triggerParameterGroupBox.Location = new System.Drawing.Point(200, 8);
            this.triggerParameterGroupBox.Name = "triggerParameterGroupBox";
            this.triggerParameterGroupBox.Size = new System.Drawing.Size(176, 160);
            this.triggerParameterGroupBox.TabIndex = 3;
            this.triggerParameterGroupBox.TabStop = false;
            this.triggerParameterGroupBox.Text = "Trigger Parameters:";
            // 
            // pauseWhenHighRadioButton
            // 
            this.pauseWhenHighRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.pauseWhenHighRadioButton.Location = new System.Drawing.Point(34, 117);
            this.pauseWhenHighRadioButton.Name = "pauseWhenHighRadioButton";
            this.pauseWhenHighRadioButton.Size = new System.Drawing.Size(56, 16);
            this.pauseWhenHighRadioButton.TabIndex = 4;
            this.pauseWhenHighRadioButton.Text = "High";
            this.pauseWhenHighRadioButton.CheckedChanged += new System.EventHandler(this.pauseWhenHighRadioButton_CheckedChanged);
            // 
            // pauseWhenLowRadioButton
            // 
            this.pauseWhenLowRadioButton.Checked = true;
            this.pauseWhenLowRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.pauseWhenLowRadioButton.Location = new System.Drawing.Point(34, 101);
            this.pauseWhenLowRadioButton.Name = "pauseWhenLowRadioButton";
            this.pauseWhenLowRadioButton.Size = new System.Drawing.Size(56, 16);
            this.pauseWhenLowRadioButton.TabIndex = 3;
            this.pauseWhenLowRadioButton.TabStop = true;
            this.pauseWhenLowRadioButton.Text = "Low";
            this.pauseWhenLowRadioButton.CheckedChanged += new System.EventHandler(this.pauseWhenLowRadioButton_CheckedChanged);
            // 
            // triggerSourceLabel
            // 
            this.triggerSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.triggerSourceLabel.Location = new System.Drawing.Point(16, 32);
            this.triggerSourceLabel.Name = "triggerSourceLabel";
            this.triggerSourceLabel.Size = new System.Drawing.Size(128, 16);
            this.triggerSourceLabel.TabIndex = 0;
            this.triggerSourceLabel.Text = "Pause Trigger Source:";
            // 
            // pauseTrigSourceTextBox
            // 
            this.pauseTrigSourceTextBox.Location = new System.Drawing.Point(16, 48);
            this.pauseTrigSourceTextBox.Name = "pauseTrigSourceTextBox";
            this.pauseTrigSourceTextBox.Size = new System.Drawing.Size(144, 20);
            this.pauseTrigSourceTextBox.TabIndex = 1;
            this.pauseTrigSourceTextBox.Text = "/Dev1/PFI0";
            // 
            // pauseConditionGroupBox
            // 
            this.pauseConditionGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.pauseConditionGroupBox.Location = new System.Drawing.Point(16, 80);
            this.pauseConditionGroupBox.Name = "pauseConditionGroupBox";
            this.pauseConditionGroupBox.Size = new System.Drawing.Size(144, 64);
            this.pauseConditionGroupBox.TabIndex = 2;
            this.pauseConditionGroupBox.TabStop = false;
            this.pauseConditionGroupBox.Text = "Pause When:";
            // 
            // statusCheckTimer
            // 
            this.statusCheckTimer.Tick += new System.EventHandler(this.statusCheckTimer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(386, 288);
            this.Controls.Add(this.triggerParameterGroupBox);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.channelParameterGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gen Dig Pulse Train-Continuous-Pause Trigger";
            this.channelParameterGroupBox.ResumeLayout(false);
            this.idleStateGroupBox.ResumeLayout(false);
            this.triggerParameterGroupBox.ResumeLayout(false);
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

            try
            {
                myTask = new Task();
                
                myTask.COChannels.CreatePulseChannelFrequency(counterComboBox.Text,
                    "PulseGenPauseTrigger", COPulseFrequencyUnits.Hertz, idleState, 0.0, 
                    Convert.ToDouble(frequencyTextBox.Text),
                    Convert.ToDouble(dutyCycleTextBox.Text));

                myTask.Triggers.PauseTrigger.ConfigureDigitalLevelTrigger(
                    pauseTrigSourceTextBox.Text, pauseCondition);
                
                myTask.Timing.ConfigureImplicit(SampleQuantityMode.ContinuousSamples, 1000);
                
                myTask.Start();

                startButton.Enabled = false;
                stopButton.Enabled = true;

                statusCheckTimer.Enabled = true;
            }
            catch(Exception exception)
            {
                statusCheckTimer.Enabled = false;
                MessageBox.Show(exception.Message);
                myTask.Dispose();
                startButton.Enabled = true;
                stopButton.Enabled = false;
            }
        }

        private void stopButton_Click(object sender, System.EventArgs e)
        {
            statusCheckTimer.Enabled = false;
            myTask.Stop();
            myTask.Dispose();   
            startButton.Enabled = true;
            stopButton.Enabled = false;
        }

        private void idleStateLowRadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            idleState = COPulseIdleState.Low;
        }

        private void idleStateHighRadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            idleState = COPulseIdleState.High;
        }

        private void pauseWhenLowRadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            pauseCondition = DigitalLevelPauseTriggerCondition.Low;
        }

        private void pauseWhenHighRadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            pauseCondition = DigitalLevelPauseTriggerCondition.High;
        }

        private void statusCheckTimer_Tick(object sender, System.EventArgs e)
        {
            try
            {
                // Getting myTask.IsDone also checks for errors that would prematurely
                // halt the continuous generation.
                if (myTask.IsDone)
                {
                    statusCheckTimer.Enabled = false;
                    myTask.Stop();
                    myTask.Dispose();
                    startButton.Enabled = true;
                    stopButton.Enabled = false;
                }
            }
            catch (DaqException ex)
            {
                statusCheckTimer.Enabled = false;
                System.Windows.Forms.MessageBox.Show(ex.Message);
                myTask.Stop();
                myTask.Dispose();
                startButton.Enabled = true;
                stopButton.Enabled = false;
            }
        }
    }
}
