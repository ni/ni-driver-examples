/******************************************************************************
*
* Example program:
*   CountDigEvents
*
* Category:
*   CI
*
* Description:
*   This example demonstrates how to count digital events on a Counter Input
*   Channel. The Initial Count, Count Direction, and Edge are all
*   configurable.This example shows how to count edges on the counter's default
*   source pin, but could easily be expanded to count edges on any PFI, RTSI, or
*   internal signal. Non-buffered event counting can also use a digital pause
*   trigger which could be added to this example by configuring the Trigger
*   object for the Task.
*
* Instructions for running:
*   1.  Enter the physical channel which corresponds to the counter you want to
*       use to count edges on the DAQ device.
*   2.  Enter the initial count, count direction, and measurement edge to
*       specify how you want the counter to count.Note:  Use the
*       GenDigPulseTrain_Continuous example to verify that you are counting
*       correctly on the DAQ device.
*
* Steps:
*   1.  Create the Task object. Create a CIChannel object with the correct
*       configuration to count events. The edge parameter is used to determine
*       if the counter will count on rising or falling edges of the input
*       signal.
*   2.  Call the Start() method to arm the counter and begin counting.  The
*       counter will be preloaded with the initial count.
*   3.  The counter will be continually polled until the Stop button is pressed
*       on the user interface.
*   4.  Dispose the Task object to clean-up any resources associated with the
*       task.
*   5.  Handle any DaqExceptions, if they occur.
*
* I/O Connections Overview:
*   This example will perform a measurement on the default terminal(s) of the
*   counter specified. The default counter terminal(s) depend on the type of
*   measurement being taken. For more information on the default counter input
*   and output terminals for your device, open the NI-DAQmx Help, and refer to
*   Counter Signal Connections found under the Device Considerations book in the
*   table of contents.
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

namespace NationalInstruments.Examples.CountDigEvents
{
    /// <summary>
    /// Summary description for MainForm.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        internal System.Windows.Forms.TextBox countTextBox;
        internal System.Windows.Forms.Button stopButton;
        internal System.Windows.Forms.Button startButton;
        internal System.Windows.Forms.GroupBox channelParameterGroupBox;
        internal System.Windows.Forms.Label countDirectionLabel;
        internal System.Windows.Forms.TextBox initialCountTextBox;
        internal System.Windows.Forms.Label initialCountLabel;
        internal System.Windows.Forms.Label counterLabel;
        internal System.Windows.Forms.ComboBox countDirectionComboBox;
        internal System.Windows.Forms.Timer loopTimer;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.RadioButton risingRadioButton;
        private System.Windows.Forms.RadioButton fallingRadioButton;
        private Task myTask;
        private CICountEdgesActiveEdge edgeType;
        internal System.Windows.Forms.Label countLabel;
        private System.Windows.Forms.GroupBox edgeGroupBox;
        internal System.Windows.Forms.GroupBox dataGroupBox;
        private CICountEdgesCountDirection countDirection;
        private CounterReader myCounterReader;
        private System.Windows.Forms.ComboBox counterComboBox;
        private uint reading;

        public MainForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            edgeType = CICountEdgesActiveEdge.Rising;
            countDirection = CICountEdgesCountDirection.Up;

            counterComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.CI, PhysicalChannelAccess.External));
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
            this.dataGroupBox = new System.Windows.Forms.GroupBox();
            this.countLabel = new System.Windows.Forms.Label();
            this.countTextBox = new System.Windows.Forms.TextBox();
            this.stopButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.channelParameterGroupBox = new System.Windows.Forms.GroupBox();
            this.counterComboBox = new System.Windows.Forms.ComboBox();
            this.edgeGroupBox = new System.Windows.Forms.GroupBox();
            this.fallingRadioButton = new System.Windows.Forms.RadioButton();
            this.risingRadioButton = new System.Windows.Forms.RadioButton();
            this.countDirectionLabel = new System.Windows.Forms.Label();
            this.initialCountTextBox = new System.Windows.Forms.TextBox();
            this.initialCountLabel = new System.Windows.Forms.Label();
            this.counterLabel = new System.Windows.Forms.Label();
            this.countDirectionComboBox = new System.Windows.Forms.ComboBox();
            this.loopTimer = new System.Windows.Forms.Timer(this.components);
            this.dataGroupBox.SuspendLayout();
            this.channelParameterGroupBox.SuspendLayout();
            this.edgeGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGroupBox
            // 
            this.dataGroupBox.Controls.Add(this.countLabel);
            this.dataGroupBox.Controls.Add(this.countTextBox);
            this.dataGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.dataGroupBox.Location = new System.Drawing.Point(264, 8);
            this.dataGroupBox.Name = "dataGroupBox";
            this.dataGroupBox.Size = new System.Drawing.Size(112, 96);
            this.dataGroupBox.TabIndex = 3;
            this.dataGroupBox.TabStop = false;
            this.dataGroupBox.Text = "Data";
            // 
            // countLabel
            // 
            this.countLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.countLabel.Location = new System.Drawing.Point(16, 24);
            this.countLabel.Name = "countLabel";
            this.countLabel.Size = new System.Drawing.Size(40, 16);
            this.countLabel.TabIndex = 0;
            this.countLabel.Text = "Count:";
            // 
            // countTextBox
            // 
            this.countTextBox.Location = new System.Drawing.Point(16, 48);
            this.countTextBox.Name = "countTextBox";
            this.countTextBox.ReadOnly = true;
            this.countTextBox.Size = new System.Drawing.Size(80, 20);
            this.countTextBox.TabIndex = 1;
            this.countTextBox.Text = "0";
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.stopButton.Location = new System.Drawing.Point(264, 144);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(112, 32);
            this.stopButton.TabIndex = 1;
            this.stopButton.Text = "Stop";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // startButton
            // 
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.startButton.Location = new System.Drawing.Point(264, 112);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(112, 32);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // channelParameterGroupBox
            // 
            this.channelParameterGroupBox.Controls.Add(this.counterComboBox);
            this.channelParameterGroupBox.Controls.Add(this.edgeGroupBox);
            this.channelParameterGroupBox.Controls.Add(this.countDirectionLabel);
            this.channelParameterGroupBox.Controls.Add(this.initialCountTextBox);
            this.channelParameterGroupBox.Controls.Add(this.initialCountLabel);
            this.channelParameterGroupBox.Controls.Add(this.counterLabel);
            this.channelParameterGroupBox.Controls.Add(this.countDirectionComboBox);
            this.channelParameterGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.channelParameterGroupBox.Location = new System.Drawing.Point(8, 9);
            this.channelParameterGroupBox.Name = "channelParameterGroupBox";
            this.channelParameterGroupBox.Size = new System.Drawing.Size(248, 167);
            this.channelParameterGroupBox.TabIndex = 2;
            this.channelParameterGroupBox.TabStop = false;
            this.channelParameterGroupBox.Text = "Channel Parameters";
            // 
            // counterComboBox
            // 
            this.counterComboBox.Location = new System.Drawing.Point(104, 24);
            this.counterComboBox.Name = "counterComboBox";
            this.counterComboBox.Size = new System.Drawing.Size(128, 21);
            this.counterComboBox.TabIndex = 1;
            this.counterComboBox.Text = "Dev1/ctr0";
            // 
            // edgeGroupBox
            // 
            this.edgeGroupBox.Controls.Add(this.fallingRadioButton);
            this.edgeGroupBox.Controls.Add(this.risingRadioButton);
            this.edgeGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.edgeGroupBox.Location = new System.Drawing.Point(8, 104);
            this.edgeGroupBox.Name = "edgeGroupBox";
            this.edgeGroupBox.Size = new System.Drawing.Size(224, 48);
            this.edgeGroupBox.TabIndex = 6;
            this.edgeGroupBox.TabStop = false;
            this.edgeGroupBox.Text = "Edge";
            // 
            // fallingRadioButton
            // 
            this.fallingRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.fallingRadioButton.Location = new System.Drawing.Point(120, 16);
            this.fallingRadioButton.Name = "fallingRadioButton";
            this.fallingRadioButton.Size = new System.Drawing.Size(80, 24);
            this.fallingRadioButton.TabIndex = 1;
            this.fallingRadioButton.Text = "Falling";
            this.fallingRadioButton.CheckedChanged += new System.EventHandler(this.fallingRadioButton_CheckedChanged);
            // 
            // risingRadioButton
            // 
            this.risingRadioButton.Checked = true;
            this.risingRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.risingRadioButton.Location = new System.Drawing.Point(32, 16);
            this.risingRadioButton.Name = "risingRadioButton";
            this.risingRadioButton.Size = new System.Drawing.Size(80, 24);
            this.risingRadioButton.TabIndex = 0;
            this.risingRadioButton.TabStop = true;
            this.risingRadioButton.Text = "Rising";
            this.risingRadioButton.CheckedChanged += new System.EventHandler(this.risingRadioButton_CheckedChanged);
            // 
            // countDirectionLabel
            // 
            this.countDirectionLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.countDirectionLabel.Location = new System.Drawing.Point(8, 74);
            this.countDirectionLabel.Name = "countDirectionLabel";
            this.countDirectionLabel.Size = new System.Drawing.Size(88, 16);
            this.countDirectionLabel.TabIndex = 4;
            this.countDirectionLabel.Text = "Count Direction:";
            // 
            // initialCountTextBox
            // 
            this.initialCountTextBox.Location = new System.Drawing.Point(104, 48);
            this.initialCountTextBox.Name = "initialCountTextBox";
            this.initialCountTextBox.Size = new System.Drawing.Size(128, 20);
            this.initialCountTextBox.TabIndex = 3;
            this.initialCountTextBox.Text = "0";
            // 
            // initialCountLabel
            // 
            this.initialCountLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.initialCountLabel.Location = new System.Drawing.Point(8, 50);
            this.initialCountLabel.Name = "initialCountLabel";
            this.initialCountLabel.Size = new System.Drawing.Size(72, 16);
            this.initialCountLabel.TabIndex = 2;
            this.initialCountLabel.Text = "Initial Count:";
            // 
            // counterLabel
            // 
            this.counterLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.counterLabel.Location = new System.Drawing.Point(8, 26);
            this.counterLabel.Name = "counterLabel";
            this.counterLabel.Size = new System.Drawing.Size(72, 16);
            this.counterLabel.TabIndex = 0;
            this.counterLabel.Text = "Counter(s):";
            // 
            // countDirectionComboBox
            // 
            this.countDirectionComboBox.Items.AddRange(new object[] {
                                                                        "Count Up",
                                                                        "Count Down",
                                                                        "Externally Controlled"});
            this.countDirectionComboBox.Location = new System.Drawing.Point(104, 72);
            this.countDirectionComboBox.Name = "countDirectionComboBox";
            this.countDirectionComboBox.Size = new System.Drawing.Size(128, 21);
            this.countDirectionComboBox.TabIndex = 5;
            this.countDirectionComboBox.Text = "Count Up";
            // 
            // loopTimer
            // 
            this.loopTimer.Tick += new System.EventHandler(this.loopTimer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(386, 184);
            this.Controls.Add(this.dataGroupBox);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.channelParameterGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Count Digital Events";
            this.dataGroupBox.ResumeLayout(false);
            this.channelParameterGroupBox.ResumeLayout(false);
            this.edgeGroupBox.ResumeLayout(false);
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
            
                switch (countDirectionComboBox.SelectedItem.ToString())
                {
                    case "Count Up":
                        countDirection = CICountEdgesCountDirection.Up;
                        break;
                    case "Count Down":
                        countDirection = CICountEdgesCountDirection.Down;
                        break;
                    case "Externally Controlled":
                        countDirection = CICountEdgesCountDirection.ExternallyControlled;
                        break;
                }   

                myTask.CIChannels.CreateCountEdgesChannel (counterComboBox.Text, "Count Edges", 
                    edgeType, Convert.ToInt64(initialCountTextBox.Text), countDirection);

                myCounterReader = new CounterReader(myTask.Stream);

                myTask.Start();
                loopTimer.Enabled = true;
            }
            catch(DaqException exception)
            {
                loopTimer.Enabled = false;
                MessageBox.Show(exception.Message);
                myTask.Dispose();
                return;
            }
            startButton.Enabled=false;
            stopButton.Enabled=true;

        }

        private void stopButton_Click(object sender, System.EventArgs e)
        {
            loopTimer.Enabled = false;
            startButton.Enabled = true;
            stopButton.Enabled = false;
            myTask.Stop();
            myTask.Dispose();
        }

        private void loopTimer_Tick(object sender, System.EventArgs e)
        {           
            try
            {
                reading = myCounterReader.ReadSingleSampleUInt32();
                countTextBox.Text = Convert.ToString(reading);
            }
            catch (DaqException exception)
            {
                MessageBox.Show(exception.Message);
                myTask.Dispose();
                loopTimer.Enabled = false;
                startButton.Enabled = true;
                stopButton.Enabled = false;
                return;
            }
        }

        private void risingRadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            edgeType = CICountEdgesActiveEdge.Rising;
        }

        private void fallingRadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            edgeType = CICountEdgesActiveEdge.Falling;
        }
    }
}
