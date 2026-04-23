/******************************************************************************
*
* Example program:
*   GenDigPulse
*
* Category:
*   CO
*
* Description:
*   This example demonstrates how to generate a single digital pulse from a
*   counter output channel.  The initial delay, high time, low time, and idle
*   state are all software configurable. This example shows how to configure the
*   pulse in terms of time, but can easily be modified to generate a pulse in
*   terms of frequency and duty cycle or ticks.
*
* Instructions for running:
*   1.  Enter the physical channel which corresponds to the counter you want to
*       use to output your signal to on the DAQ device.
*   2.  Enter the low time and high time (in seconds) to define the pulse
*       parameters. Additionally, you can set the initial delay (in seconds)
*       which will delay the beginning of the pulse from the start call. Also,
*       you can change the idle state to generate a high or low pulse.Note: Use
*       the MeasPulseWidth example to verify you are outputting a pulse on the
*       DAQ device.
*
* Steps:
*   1.  Create a counter output channel using
*       Task.COChannels.CreatePulseChannelTime to produce a pulse in terms of
*       time. If the idle state of the pulse is set to low the first transition
*       of the generated signal is from low to high.
*   2.  Call Task.Start to arm the counter and begin the pulse generation.  The
*       pulse will not begin until after the initial delay (in seconds) has
*       expired.
*   3.  Use Task.WaitUntilDone to ensure the entire pulse is generated before
*       ending the task.
*   4.  Dispose the Task object to clean-up any resources associated with the
*       task.
*   5.  Handle any DaqExceptions, if they occur.
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
using System.Threading;
using NationalInstruments.DAQmx;

namespace NationalInstruments.Examples.GenDigPulse
{
    public delegate void FormDelegate();

    /// <summary>
    /// Summary description for MainForm.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        private Task myTask;
        private COPulseIdleState idleStates;
        private FormDelegate myDelegate;

        private System.Windows.Forms.GroupBox channelParamGroupBox;
        internal System.Windows.Forms.Label physicalChannelLabel;
        private System.Windows.Forms.TextBox lowTimeTextBox;
        internal System.Windows.Forms.Label lowTimeLabel;
        internal System.Windows.Forms.Label highTimeLabel;
        private System.Windows.Forms.TextBox highTimeTextBox;
        private System.Windows.Forms.TextBox iniDelayTextBox;
        private System.Windows.Forms.GroupBox idleStateGroupBox;
        private System.Windows.Forms.RadioButton highRadioButton;
        private System.Windows.Forms.RadioButton lowRadioButton;
        internal System.Windows.Forms.Label iniDelayLabel;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.ComboBox counterComboBox;

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
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            idleStates = COPulseIdleState.Low;

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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainForm));
            this.channelParamGroupBox = new System.Windows.Forms.GroupBox();
            this.counterComboBox = new System.Windows.Forms.ComboBox();
            this.idleStateGroupBox = new System.Windows.Forms.GroupBox();
            this.highRadioButton = new System.Windows.Forms.RadioButton();
            this.lowRadioButton = new System.Windows.Forms.RadioButton();
            this.iniDelayLabel = new System.Windows.Forms.Label();
            this.iniDelayTextBox = new System.Windows.Forms.TextBox();
            this.highTimeLabel = new System.Windows.Forms.Label();
            this.highTimeTextBox = new System.Windows.Forms.TextBox();
            this.lowTimeLabel = new System.Windows.Forms.Label();
            this.lowTimeTextBox = new System.Windows.Forms.TextBox();
            this.physicalChannelLabel = new System.Windows.Forms.Label();
            this.startButton = new System.Windows.Forms.Button();
            this.channelParamGroupBox.SuspendLayout();
            this.idleStateGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // channelParamGroupBox
            // 
            this.channelParamGroupBox.Controls.Add(this.counterComboBox);
            this.channelParamGroupBox.Controls.Add(this.idleStateGroupBox);
            this.channelParamGroupBox.Controls.Add(this.iniDelayLabel);
            this.channelParamGroupBox.Controls.Add(this.iniDelayTextBox);
            this.channelParamGroupBox.Controls.Add(this.highTimeLabel);
            this.channelParamGroupBox.Controls.Add(this.highTimeTextBox);
            this.channelParamGroupBox.Controls.Add(this.lowTimeLabel);
            this.channelParamGroupBox.Controls.Add(this.lowTimeTextBox);
            this.channelParamGroupBox.Controls.Add(this.physicalChannelLabel);
            this.channelParamGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.channelParamGroupBox.Location = new System.Drawing.Point(16, 16);
            this.channelParamGroupBox.Name = "channelParamGroupBox";
            this.channelParamGroupBox.Size = new System.Drawing.Size(136, 304);
            this.channelParamGroupBox.TabIndex = 1;
            this.channelParamGroupBox.TabStop = false;
            this.channelParamGroupBox.Text = "Channel Parameters";
            // 
            // counterComboBox
            // 
            this.counterComboBox.Location = new System.Drawing.Point(16, 40);
            this.counterComboBox.Name = "counterComboBox";
            this.counterComboBox.Size = new System.Drawing.Size(104, 21);
            this.counterComboBox.TabIndex = 1;
            this.counterComboBox.Text = "Dev1/ctr0";
            // 
            // idleStateGroupBox
            // 
            this.idleStateGroupBox.Controls.Add(this.highRadioButton);
            this.idleStateGroupBox.Controls.Add(this.lowRadioButton);
            this.idleStateGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.idleStateGroupBox.Location = new System.Drawing.Point(16, 216);
            this.idleStateGroupBox.Name = "idleStateGroupBox";
            this.idleStateGroupBox.Size = new System.Drawing.Size(104, 72);
            this.idleStateGroupBox.TabIndex = 8;
            this.idleStateGroupBox.TabStop = false;
            this.idleStateGroupBox.Text = "Idle State:";
            // 
            // highRadioButton
            // 
            this.highRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.highRadioButton.Location = new System.Drawing.Point(16, 40);
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
            this.lowRadioButton.Location = new System.Drawing.Point(16, 16);
            this.lowRadioButton.Name = "lowRadioButton";
            this.lowRadioButton.Size = new System.Drawing.Size(64, 24);
            this.lowRadioButton.TabIndex = 0;
            this.lowRadioButton.TabStop = true;
            this.lowRadioButton.Text = "Low";
            this.lowRadioButton.CheckedChanged += new System.EventHandler(this.lowRadioButton_CheckedChanged);
            // 
            // iniDelayLabel
            // 
            this.iniDelayLabel.BackColor = System.Drawing.SystemColors.Control;
            this.iniDelayLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.iniDelayLabel.Location = new System.Drawing.Point(15, 168);
            this.iniDelayLabel.Name = "iniDelayLabel";
            this.iniDelayLabel.Size = new System.Drawing.Size(100, 16);
            this.iniDelayLabel.TabIndex = 6;
            this.iniDelayLabel.Text = "Initial Delay (sec):";
            // 
            // iniDelayTextBox
            // 
            this.iniDelayTextBox.Location = new System.Drawing.Point(15, 184);
            this.iniDelayTextBox.Name = "iniDelayTextBox";
            this.iniDelayTextBox.Size = new System.Drawing.Size(104, 20);
            this.iniDelayTextBox.TabIndex = 7;
            this.iniDelayTextBox.Text = "1.00";
            // 
            // highTimeLabel
            // 
            this.highTimeLabel.BackColor = System.Drawing.SystemColors.Control;
            this.highTimeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.highTimeLabel.Location = new System.Drawing.Point(15, 120);
            this.highTimeLabel.Name = "highTimeLabel";
            this.highTimeLabel.Size = new System.Drawing.Size(100, 16);
            this.highTimeLabel.TabIndex = 4;
            this.highTimeLabel.Text = "High Time (sec):";
            // 
            // highTimeTextBox
            // 
            this.highTimeTextBox.Location = new System.Drawing.Point(15, 136);
            this.highTimeTextBox.Name = "highTimeTextBox";
            this.highTimeTextBox.Size = new System.Drawing.Size(104, 20);
            this.highTimeTextBox.TabIndex = 5;
            this.highTimeTextBox.Text = "1.00";
            // 
            // lowTimeLabel
            // 
            this.lowTimeLabel.BackColor = System.Drawing.SystemColors.Control;
            this.lowTimeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lowTimeLabel.Location = new System.Drawing.Point(15, 72);
            this.lowTimeLabel.Name = "lowTimeLabel";
            this.lowTimeLabel.Size = new System.Drawing.Size(100, 16);
            this.lowTimeLabel.TabIndex = 2;
            this.lowTimeLabel.Text = "Low Time (sec):";
            // 
            // lowTimeTextBox
            // 
            this.lowTimeTextBox.Location = new System.Drawing.Point(15, 88);
            this.lowTimeTextBox.Name = "lowTimeTextBox";
            this.lowTimeTextBox.Size = new System.Drawing.Size(104, 20);
            this.lowTimeTextBox.TabIndex = 3;
            this.lowTimeTextBox.Text = "0.50";
            // 
            // physicalChannelLabel
            // 
            this.physicalChannelLabel.BackColor = System.Drawing.SystemColors.Control;
            this.physicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.physicalChannelLabel.Location = new System.Drawing.Point(15, 24);
            this.physicalChannelLabel.Name = "physicalChannelLabel";
            this.physicalChannelLabel.Size = new System.Drawing.Size(100, 16);
            this.physicalChannelLabel.TabIndex = 0;
            this.physicalChannelLabel.Text = "Counter(s):";
            // 
            // startButton
            // 
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.startButton.Location = new System.Drawing.Point(176, 24);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(104, 40);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Generate Pulse";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(298, 336);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.channelParamGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Generate Digital Pulse";
            this.channelParamGroupBox.ResumeLayout(false);
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

        private void lowRadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            idleStates = COPulseIdleState.Low;
        }

        private void highRadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            idleStates = COPulseIdleState.High;
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
                
                myTask.COChannels.CreatePulseChannelTime(counterComboBox.Text,
                    "GenerateDigitalPulse", COPulseTimeUnits.Seconds, idleStates, 
                    Convert.ToDouble(iniDelayTextBox.Text), 
                    Convert.ToDouble(lowTimeTextBox.Text), 
                    Convert.ToDouble(highTimeTextBox.Text));

                myTask.Start();
                    
                startButton.Enabled = false;

                //Wait for the task to complete in another thread so the UI
                //does not freeze.
                ThreadPool.QueueUserWorkItem(new WaitCallback(WaitMethod));
            }
            catch(DaqException exception)
            {
                MessageBox.Show(exception.Message);
                myTask.Dispose();
                startButton.Enabled = true;
            }
        }

        private void WaitMethod(Object obj)
        {
            myTask.WaitUntilDone(-1);
            myTask.Stop();
            myTask.Dispose();

            myDelegate = new FormDelegate(EnableStart);
            this.Invoke(myDelegate);
        }

        private void EnableStart()
        {
            startButton.Enabled = true;
        }
    }
}
