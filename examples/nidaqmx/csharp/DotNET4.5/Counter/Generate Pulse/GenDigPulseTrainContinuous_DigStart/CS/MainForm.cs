/******************************************************************************
*
* Example program:
*   GenDigPulseTrainContinuous_DigStart
*
* Category:
*   CO
*
* Description:
*   This example demonstrates how to generate a continuous digital pulse train
*   from a counter output channel using a digital start trigger. The frequency,
*   duty cycle, and idle state are all configurable.This example shows how to
*   configure the pulse in terms of frequency and duty cycle, but it can easily
*   be modified to generate a pulse in terms of time or ticks.
*
* Instructions for running:
*   1.  Enter the physical channel which corresponds to the counter you want to
*       output your signal to on the DAQ device.
*   2.  Enter the frequency and duty cycle to define the pulse parameters. You
*       can also change the idle state to set the state the line will remain in
*       after the generation is stopped.
*   3.  Setup the trigger parameters.  The default in this example is a rising
*       edge digital trigger on PFI9 (the default gate of ctr0).
*
* Steps:
*   1.  Create a Task object. Create a COChannel object for pulse generation in
*       terms of frequency. If the idle state of the pulse is set to low the
*       first transition of the generated signal is from low to high.
*   2.  Configure the Triggers object to have pulse generation start on a
*       digital edge.
*   3.  Use the ConfigureImplicit() method to configure the duration of the
*       pulse generation.
*   4.  Call the Start() method to arm the counter and begin waiting for the
*       trigger to start the pulse train generation.
*   5.  Call the Stop() method to stop the task and Dispose() method to
*       de-allocate any resources used by the Task.
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

namespace NationalInstruments.Examples.GenDigPulseTrainContinuous_DigStart
{
    /// <summary>
    /// Summary description for MainForm.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        internal System.Windows.Forms.Button stopButton;
        internal System.Windows.Forms.GroupBox triggerParameterGroupBox;
        internal System.Windows.Forms.Label triggerSourceLabel;
        internal System.Windows.Forms.Label edgeLabel;
        internal System.Windows.Forms.TextBox triggerSourceTextBox;
        internal System.Windows.Forms.GroupBox channelParameterGroupBox;
        internal System.Windows.Forms.Label idleStateLabel;
        internal System.Windows.Forms.TextBox dutyCycleTextBox;
        internal System.Windows.Forms.Label dutyCycleLabel;
        internal System.Windows.Forms.TextBox frequencyTextBox;
        internal System.Windows.Forms.Label frequencyLabel;
        internal System.Windows.Forms.Label counterLabel;
        internal System.Windows.Forms.Button startButton;
        private System.Windows.Forms.RadioButton lowRadioButton;
        private System.Windows.Forms.RadioButton highRadioButton;
        private System.Windows.Forms.RadioButton risingRadioButton;
        private System.Windows.Forms.RadioButton fallingRadioButton;
        private Task myTask;
        private COPulseIdleState idleState;
        private DigitalEdgeStartTriggerEdge triggerEdge;
        private System.Windows.Forms.Timer statusCheckTimer;
        private System.Windows.Forms.ComboBox counterComboBox;
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
            triggerEdge = DigitalEdgeStartTriggerEdge.Rising;
            lowRadioButton.Checked = true;
            risingRadioButton.Checked = true;

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
            this.triggerParameterGroupBox = new System.Windows.Forms.GroupBox();
            this.fallingRadioButton = new System.Windows.Forms.RadioButton();
            this.risingRadioButton = new System.Windows.Forms.RadioButton();
            this.triggerSourceLabel = new System.Windows.Forms.Label();
            this.edgeLabel = new System.Windows.Forms.Label();
            this.triggerSourceTextBox = new System.Windows.Forms.TextBox();
            this.channelParameterGroupBox = new System.Windows.Forms.GroupBox();
            this.counterComboBox = new System.Windows.Forms.ComboBox();
            this.idleStateLabel = new System.Windows.Forms.Label();
            this.dutyCycleTextBox = new System.Windows.Forms.TextBox();
            this.dutyCycleLabel = new System.Windows.Forms.Label();
            this.frequencyTextBox = new System.Windows.Forms.TextBox();
            this.frequencyLabel = new System.Windows.Forms.Label();
            this.counterLabel = new System.Windows.Forms.Label();
            this.lowRadioButton = new System.Windows.Forms.RadioButton();
            this.highRadioButton = new System.Windows.Forms.RadioButton();
            this.startButton = new System.Windows.Forms.Button();
            this.statusCheckTimer = new System.Windows.Forms.Timer(this.components);
            this.triggerParameterGroupBox.SuspendLayout();
            this.channelParameterGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.stopButton.Location = new System.Drawing.Point(208, 224);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(120, 32);
            this.stopButton.TabIndex = 1;
            this.stopButton.Text = "Stop";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // triggerParameterGroupBox
            // 
            this.triggerParameterGroupBox.Controls.Add(this.fallingRadioButton);
            this.triggerParameterGroupBox.Controls.Add(this.risingRadioButton);
            this.triggerParameterGroupBox.Controls.Add(this.triggerSourceLabel);
            this.triggerParameterGroupBox.Controls.Add(this.edgeLabel);
            this.triggerParameterGroupBox.Controls.Add(this.triggerSourceTextBox);
            this.triggerParameterGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.triggerParameterGroupBox.Location = new System.Drawing.Point(192, 8);
            this.triggerParameterGroupBox.Name = "triggerParameterGroupBox";
            this.triggerParameterGroupBox.Size = new System.Drawing.Size(152, 160);
            this.triggerParameterGroupBox.TabIndex = 3;
            this.triggerParameterGroupBox.TabStop = false;
            this.triggerParameterGroupBox.Text = "Trigger Parameters";
            // 
            // fallingRadioButton
            // 
            this.fallingRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.fallingRadioButton.Location = new System.Drawing.Point(40, 120);
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
            this.risingRadioButton.Location = new System.Drawing.Point(40, 96);
            this.risingRadioButton.Name = "risingRadioButton";
            this.risingRadioButton.Size = new System.Drawing.Size(64, 24);
            this.risingRadioButton.TabIndex = 3;
            this.risingRadioButton.TabStop = true;
            this.risingRadioButton.Text = "Rising";
            this.risingRadioButton.CheckedChanged += new System.EventHandler(this.risingRadioButton_CheckedChanged);
            // 
            // triggerSourceLabel
            // 
            this.triggerSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.triggerSourceLabel.Location = new System.Drawing.Point(16, 24);
            this.triggerSourceLabel.Name = "triggerSourceLabel";
            this.triggerSourceLabel.Size = new System.Drawing.Size(96, 16);
            this.triggerSourceLabel.TabIndex = 0;
            this.triggerSourceLabel.Text = "Trigger Source:";
            // 
            // edgeLabel
            // 
            this.edgeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.edgeLabel.Location = new System.Drawing.Point(16, 72);
            this.edgeLabel.Name = "edgeLabel";
            this.edgeLabel.Size = new System.Drawing.Size(100, 16);
            this.edgeLabel.TabIndex = 2;
            this.edgeLabel.Text = "Trigger Edge:";
            // 
            // triggerSourceTextBox
            // 
            this.triggerSourceTextBox.Location = new System.Drawing.Point(16, 40);
            this.triggerSourceTextBox.Name = "triggerSourceTextBox";
            this.triggerSourceTextBox.Size = new System.Drawing.Size(120, 20);
            this.triggerSourceTextBox.TabIndex = 1;
            this.triggerSourceTextBox.Text = "/Dev1/PFI9";
            // 
            // channelParameterGroupBox
            // 
            this.channelParameterGroupBox.Controls.Add(this.counterComboBox);
            this.channelParameterGroupBox.Controls.Add(this.idleStateLabel);
            this.channelParameterGroupBox.Controls.Add(this.dutyCycleTextBox);
            this.channelParameterGroupBox.Controls.Add(this.dutyCycleLabel);
            this.channelParameterGroupBox.Controls.Add(this.frequencyTextBox);
            this.channelParameterGroupBox.Controls.Add(this.frequencyLabel);
            this.channelParameterGroupBox.Controls.Add(this.counterLabel);
            this.channelParameterGroupBox.Controls.Add(this.lowRadioButton);
            this.channelParameterGroupBox.Controls.Add(this.highRadioButton);
            this.channelParameterGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.channelParameterGroupBox.Location = new System.Drawing.Point(8, 8);
            this.channelParameterGroupBox.Name = "channelParameterGroupBox";
            this.channelParameterGroupBox.Size = new System.Drawing.Size(168, 272);
            this.channelParameterGroupBox.TabIndex = 2;
            this.channelParameterGroupBox.TabStop = false;
            this.channelParameterGroupBox.Text = "Channel Parameters";
            // 
            // counterComboBox
            // 
            this.counterComboBox.Location = new System.Drawing.Point(16, 32);
            this.counterComboBox.Name = "counterComboBox";
            this.counterComboBox.Size = new System.Drawing.Size(136, 21);
            this.counterComboBox.TabIndex = 1;
            this.counterComboBox.Text = "Dev1/ctr0";
            // 
            // idleStateLabel
            // 
            this.idleStateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.idleStateLabel.Location = new System.Drawing.Point(16, 184);
            this.idleStateLabel.Name = "idleStateLabel";
            this.idleStateLabel.Size = new System.Drawing.Size(64, 16);
            this.idleStateLabel.TabIndex = 6;
            this.idleStateLabel.Text = "Idle State:";
            // 
            // dutyCycleTextBox
            // 
            this.dutyCycleTextBox.Location = new System.Drawing.Point(16, 144);
            this.dutyCycleTextBox.Name = "dutyCycleTextBox";
            this.dutyCycleTextBox.Size = new System.Drawing.Size(136, 20);
            this.dutyCycleTextBox.TabIndex = 5;
            this.dutyCycleTextBox.Text = "0.5";
            // 
            // dutyCycleLabel
            // 
            this.dutyCycleLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.dutyCycleLabel.Location = new System.Drawing.Point(16, 128);
            this.dutyCycleLabel.Name = "dutyCycleLabel";
            this.dutyCycleLabel.Size = new System.Drawing.Size(100, 16);
            this.dutyCycleLabel.TabIndex = 4;
            this.dutyCycleLabel.Text = "Duty Cycle:";
            // 
            // frequencyTextBox
            // 
            this.frequencyTextBox.Location = new System.Drawing.Point(16, 88);
            this.frequencyTextBox.Name = "frequencyTextBox";
            this.frequencyTextBox.Size = new System.Drawing.Size(136, 20);
            this.frequencyTextBox.TabIndex = 3;
            this.frequencyTextBox.Text = "1.0";
            // 
            // frequencyLabel
            // 
            this.frequencyLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.frequencyLabel.Location = new System.Drawing.Point(16, 72);
            this.frequencyLabel.Name = "frequencyLabel";
            this.frequencyLabel.Size = new System.Drawing.Size(100, 16);
            this.frequencyLabel.TabIndex = 2;
            this.frequencyLabel.Text = "Frequency (Hz):";
            // 
            // counterLabel
            // 
            this.counterLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.counterLabel.Location = new System.Drawing.Point(16, 16);
            this.counterLabel.Name = "counterLabel";
            this.counterLabel.Size = new System.Drawing.Size(100, 16);
            this.counterLabel.TabIndex = 0;
            this.counterLabel.Text = "Counter(s):";
            // 
            // lowRadioButton
            // 
            this.lowRadioButton.Checked = true;
            this.lowRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lowRadioButton.Location = new System.Drawing.Point(40, 208);
            this.lowRadioButton.Name = "lowRadioButton";
            this.lowRadioButton.Size = new System.Drawing.Size(88, 24);
            this.lowRadioButton.TabIndex = 7;
            this.lowRadioButton.TabStop = true;
            this.lowRadioButton.Text = "Low";
            this.lowRadioButton.CheckedChanged += new System.EventHandler(this.lowRadioButton_CheckedChanged);
            // 
            // highRadioButton
            // 
            this.highRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.highRadioButton.Location = new System.Drawing.Point(40, 232);
            this.highRadioButton.Name = "highRadioButton";
            this.highRadioButton.Size = new System.Drawing.Size(88, 24);
            this.highRadioButton.TabIndex = 8;
            this.highRadioButton.Text = "High";
            this.highRadioButton.CheckedChanged += new System.EventHandler(this.highRadioButton_CheckedChanged);
            // 
            // startButton
            // 
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.startButton.Location = new System.Drawing.Point(208, 192);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(120, 32);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // statusCheckTimer
            // 
            this.statusCheckTimer.Tick += new System.EventHandler(this.statusCheckTimer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(354, 288);
            this.Controls.Add(this.triggerParameterGroupBox);
            this.Controls.Add(this.channelParameterGroupBox);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.stopButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gen Dig Pulse Train-Continuous-Dig Start";
            this.triggerParameterGroupBox.ResumeLayout(false);
            this.channelParameterGroupBox.ResumeLayout(false);
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
                    "PulseTrain", COPulseFrequencyUnits.Hertz, idleState, 0.0, 
                    Convert.ToDouble(frequencyTextBox.Text), 
                    Convert.ToDouble(dutyCycleTextBox.Text));
                
                myTask.Triggers.StartTrigger.Type = StartTriggerType.DigitalEdge;
                myTask.Triggers.StartTrigger.DigitalEdge.Edge = triggerEdge;
                myTask.Triggers.StartTrigger.DigitalEdge.Source = triggerSourceTextBox.Text;
                
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

        private void lowRadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            idleState = COPulseIdleState.Low;       
        }

        private void highRadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            idleState = COPulseIdleState.High;
        }

        private void risingRadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            triggerEdge = DigitalEdgeStartTriggerEdge.Rising;       
        }

        private void fallingRadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            triggerEdge = DigitalEdgeStartTriggerEdge.Falling;
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
