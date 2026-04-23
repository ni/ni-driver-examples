/******************************************************************************
*
* Example program:
*   GenDigPulseTrain_Continuous
*
* Category:
*   CO
*
* Description:
*   This example demonstrates how to generate a continuous digital pulse train
*   from a counter output channel.  The frequency, duty cycle, and idle state
*   are all configurable.This example shows how to configure the pulse in terms
*   of frequency and duty cycle, but it can easily be modified to generate a
*   pulse in terms of time or ticks.
*
* Instructions for running:
*   1.  Enter the physical channel of the counter you want to output your signal
*       to on the DAQ device.
*   2.  Enter the frequency and duty cycle to define the pulse parameters.  You
*       can also change the idle state to set the state the line will remain in
*       after the generation is stopped.Note:  Use the CountDigEvents example to
*       verify you are outputting the pulse train on the DAQ device.
*
* Steps:
*   1.  Create a counter output channel to produce a pulse in terms of
*       frequency.  If the idle state of the pulse is set to low, the first
*       transition of the generated signal is from low to high.
*   2.  Use the Task.Timing.ConfigureImplicit method to configure the duration
*       of the pulse generation.
*   3.  Call the Task.Start method to arm the counter and begin the pulse train
*       generation.
*   4.  This example will continue to generate the pulse train until the Stop
*       button is pressed.
*   5.  Call the Task.Stop method to stop the function
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


namespace NationalInstruments.Examples.GenDigPulseTrain_Continuous
{
    /// <summary>
    /// Summary description for MainForm.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.GroupBox channelParameterGroupBox;
        private System.Windows.Forms.Label physicalChannelLabel;
        private System.Windows.Forms.TextBox frequencyTextBox;
        private System.Windows.Forms.Label frequencyLabel;
        private System.Windows.Forms.TextBox dutyCycleTextBox;
        private System.Windows.Forms.Label dutyCycleLabel;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private Task myTask;
        private COPulseIdleState idleState;
        private System.Windows.Forms.GroupBox idleStateGroupBox;
        private System.Windows.Forms.RadioButton highRadioButton;
        private System.Windows.Forms.RadioButton lowRadioButton;
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
            this.channelParameterGroupBox = new System.Windows.Forms.GroupBox();
            this.counterComboBox = new System.Windows.Forms.ComboBox();
            this.idleStateGroupBox = new System.Windows.Forms.GroupBox();
            this.highRadioButton = new System.Windows.Forms.RadioButton();
            this.lowRadioButton = new System.Windows.Forms.RadioButton();
            this.dutyCycleTextBox = new System.Windows.Forms.TextBox();
            this.dutyCycleLabel = new System.Windows.Forms.Label();
            this.frequencyTextBox = new System.Windows.Forms.TextBox();
            this.frequencyLabel = new System.Windows.Forms.Label();
            this.physicalChannelLabel = new System.Windows.Forms.Label();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.statusCheckTimer = new System.Windows.Forms.Timer(this.components);
            this.channelParameterGroupBox.SuspendLayout();
            this.idleStateGroupBox.SuspendLayout();
            this.SuspendLayout();
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
            this.channelParameterGroupBox.Size = new System.Drawing.Size(224, 184);
            this.channelParameterGroupBox.TabIndex = 2;
            this.channelParameterGroupBox.TabStop = false;
            this.channelParameterGroupBox.Text = "Channel Parameters:";
            // 
            // counterComboBox
            // 
            this.counterComboBox.Location = new System.Drawing.Point(112, 16);
            this.counterComboBox.Name = "counterComboBox";
            this.counterComboBox.Size = new System.Drawing.Size(100, 21);
            this.counterComboBox.TabIndex = 1;
            this.counterComboBox.Text = "Dev1/ctr0";
            // 
            // idleStateGroupBox
            // 
            this.idleStateGroupBox.Controls.Add(this.highRadioButton);
            this.idleStateGroupBox.Controls.Add(this.lowRadioButton);
            this.idleStateGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.idleStateGroupBox.Location = new System.Drawing.Point(8, 112);
            this.idleStateGroupBox.Name = "idleStateGroupBox";
            this.idleStateGroupBox.Size = new System.Drawing.Size(208, 64);
            this.idleStateGroupBox.TabIndex = 6;
            this.idleStateGroupBox.TabStop = false;
            this.idleStateGroupBox.Text = "Idle State:";
            // 
            // highRadioButton
            // 
            this.highRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.highRadioButton.Location = new System.Drawing.Point(112, 24);
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
            this.lowRadioButton.Location = new System.Drawing.Point(40, 24);
            this.lowRadioButton.Name = "lowRadioButton";
            this.lowRadioButton.Size = new System.Drawing.Size(64, 24);
            this.lowRadioButton.TabIndex = 0;
            this.lowRadioButton.TabStop = true;
            this.lowRadioButton.Text = "Low";
            this.lowRadioButton.CheckedChanged += new System.EventHandler(this.lowRadioButton_CheckedChanged);
            // 
            // dutyCycleTextBox
            // 
            this.dutyCycleTextBox.Location = new System.Drawing.Point(112, 80);
            this.dutyCycleTextBox.Name = "dutyCycleTextBox";
            this.dutyCycleTextBox.TabIndex = 5;
            this.dutyCycleTextBox.Text = "0.5";
            // 
            // dutyCycleLabel
            // 
            this.dutyCycleLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.dutyCycleLabel.Location = new System.Drawing.Point(8, 84);
            this.dutyCycleLabel.Name = "dutyCycleLabel";
            this.dutyCycleLabel.Size = new System.Drawing.Size(72, 16);
            this.dutyCycleLabel.TabIndex = 4;
            this.dutyCycleLabel.Text = "Duty Cycle:";
            // 
            // frequencyTextBox
            // 
            this.frequencyTextBox.Location = new System.Drawing.Point(112, 48);
            this.frequencyTextBox.Name = "frequencyTextBox";
            this.frequencyTextBox.TabIndex = 3;
            this.frequencyTextBox.Text = "1.0";
            // 
            // frequencyLabel
            // 
            this.frequencyLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.frequencyLabel.Location = new System.Drawing.Point(8, 53);
            this.frequencyLabel.Name = "frequencyLabel";
            this.frequencyLabel.Size = new System.Drawing.Size(88, 16);
            this.frequencyLabel.TabIndex = 2;
            this.frequencyLabel.Text = "Frequency (Hz):";
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
            this.startButton.Location = new System.Drawing.Point(240, 16);
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
            this.stopButton.Location = new System.Drawing.Point(240, 56);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(96, 32);
            this.stopButton.TabIndex = 1;
            this.stopButton.Text = "Stop";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // statusCheckTimer
            // 
            this.statusCheckTimer.Tick += new System.EventHandler(this.statusCheckTimer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(346, 200);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.channelParameterGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Generate Continuous Digital Pulse Train";
            this.channelParameterGroupBox.ResumeLayout(false);
            this.idleStateGroupBox.ResumeLayout(false);
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
                    "ContinuousPulseTrain", COPulseFrequencyUnits.Hertz, idleState, 0.0, 
                    Convert.ToDouble(frequencyTextBox.Text), 
                    Convert.ToDouble(dutyCycleTextBox.Text));

                myTask.Timing.ConfigureImplicit(SampleQuantityMode.ContinuousSamples, 1000);
                                                
                myTask.Start();

                startButton.Enabled = false;
                stopButton.Enabled = true;

                statusCheckTimer.Enabled = true;
            }
            catch(Exception exception)
            {
                MessageBox.Show(exception.Message);
                myTask.Dispose();
                startButton.Enabled = true;
                stopButton.Enabled = false;
                statusCheckTimer.Enabled = false;
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
                System.Windows.Forms.MessageBox.Show(ex.Message);
                statusCheckTimer.Enabled = false;
                myTask.Stop();
                myTask.Dispose();
                startButton.Enabled = true;
                stopButton.Enabled = false;
            }
        }
    }
}
