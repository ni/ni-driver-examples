/******************************************************************************
*
* Example program:
*   CountDigEventsBuffContinuous_ExtClk
*
* Category:
*   CI
*
* Description:
*   This example demonstrates how to count buffered digital events on a Counter
*   Input channel.  
*   The initial count, count direction, edge, and sample clock source are all
*   configurable.  Edges are counted 
*   on the counter's default input terminal (see I/O Connections Overview below
*   for more information), but 
*   could easily be modified to count edges on a PFI or RTSI line.Note: For
*   buffered event counting, an external 
*   sample clock is necessary to signal when a sample should be inserted into the
*   buffer.  Specify the source 
*   terminal of the external clock in the clock source text box when you run the
*   example.
*
* Instructions for running:
*   1.  Enter the physical channel which corresponds to the counter you want to
*       use to count edges on the DAQ device.
*   2.  Enter the initial count, count direction, and measurement edge to
*       specify how you want the counter to count.
*   3.  Set the sample clock source. Note:  An external sample clock must be
*       used.  Counters do not have an internal sample clock available.  You can
*       use the GenDigPulseTrain_Continuous example to generate a pulse train on
*       another counter and connect it to the sample clock source you are using 
*       in this example.
*
* Steps:
*   1.  Create a counter input channel to count events.  The edge parameter is
*       used to determine if the counter will count rising or falling edges.
*   2.  Call ConfigureSampleClock to configure the external sample clock timing
*       parameters such as sample mode and sample clock source.  The sample
*       clock source determines when a sample will be inserted into the buffer. 
*       The edge parameter can be used to determine when a sample is taken.
*   3.  Call the CounterReader.BeginReadMultiSampleDouble method to arm the
*       counter and begin counting.  The counter will be preloaded with the
*       initial count.
*   4.  For continuous measurements, the counter will continually read new data
*       every time the set number of samples becomes available in the buffer.
*   5.  Call Task.Dispose to stop the task and de-allocate any resources used by
*       the task
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
*   This example will perform a measurement on the default terminal(s) of the
*   counter specified. In this example the two edge separation will be measured
*   on the default input terminals on ctr0. For more information on the default
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


namespace NationalInstruments.Examples.CountDigEventsBuffContinuous_ExtClk
{
    /// <summary>
    /// Summary description for MainForm.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private System.Windows.Forms.TextBox sampleClockTextBox;
        private System.Windows.Forms.Label clockSourceLabel;
        internal System.Windows.Forms.GroupBox channelParameterGroupBox;
        private System.Windows.Forms.GroupBox edgeGroupBox;
        private System.Windows.Forms.RadioButton fallingRadioButton;
        private System.Windows.Forms.RadioButton risingRadioButton;
        internal System.Windows.Forms.Label countDirectionLabel;
        internal System.Windows.Forms.TextBox initialCountTextBox;
        internal System.Windows.Forms.Label initialCountLabel;
        internal System.Windows.Forms.Label counterLabel;
        internal System.Windows.Forms.ComboBox countDirectionComboBox;
        internal System.Windows.Forms.GroupBox dataGroupBox;
        internal System.Windows.Forms.Label countLabel;
        internal System.Windows.Forms.Button stopButton;
        internal System.Windows.Forms.Button startButton;
        
        private CICountEdgesActiveEdge edgeType = CICountEdgesActiveEdge.Rising;
        private CounterReader myCounterReader;
        private Task counterReadTask;
        private CICountEdgesCountDirection countDirection = CICountEdgesCountDirection.Up;
        private System.Windows.Forms.GroupBox timingParamGroupBox;
        private System.Windows.Forms.Label rateLabel;
        private System.Windows.Forms.Label sampleReadLabel;
        private System.Windows.Forms.TextBox sampleToReadTextBox;
        private System.Windows.Forms.ListBox dataListBox;
        private System.Windows.Forms.TextBox rateTextBox;
        private AsyncCallback myCallBack;
        private System.Windows.Forms.ComboBox counterComboBox;
        private Task runningTask;
        double[] data;
        int actualNumberOFSamplesRead = 0;

        public MainForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            countDirectionComboBox.SelectedIndex=0;
            
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
                if (counterReadTask != null)
                {
                    runningTask = null;
                    counterReadTask.Dispose();
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
            this.sampleClockTextBox = new System.Windows.Forms.TextBox();
            this.timingParamGroupBox = new System.Windows.Forms.GroupBox();
            this.sampleToReadTextBox = new System.Windows.Forms.TextBox();
            this.sampleReadLabel = new System.Windows.Forms.Label();
            this.rateTextBox = new System.Windows.Forms.TextBox();
            this.rateLabel = new System.Windows.Forms.Label();
            this.clockSourceLabel = new System.Windows.Forms.Label();
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
            this.dataGroupBox = new System.Windows.Forms.GroupBox();
            this.dataListBox = new System.Windows.Forms.ListBox();
            this.countLabel = new System.Windows.Forms.Label();
            this.stopButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.timingParamGroupBox.SuspendLayout();
            this.channelParameterGroupBox.SuspendLayout();
            this.edgeGroupBox.SuspendLayout();
            this.dataGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // sampleClockTextBox
            // 
            this.sampleClockTextBox.Location = new System.Drawing.Point(8, 40);
            this.sampleClockTextBox.Name = "sampleClockTextBox";
            this.sampleClockTextBox.Size = new System.Drawing.Size(120, 20);
            this.sampleClockTextBox.TabIndex = 1;
            this.sampleClockTextBox.Text = "/Dev1/PFI9";
            // 
            // timingParamGroupBox
            // 
            this.timingParamGroupBox.Controls.Add(this.sampleToReadTextBox);
            this.timingParamGroupBox.Controls.Add(this.sampleReadLabel);
            this.timingParamGroupBox.Controls.Add(this.rateTextBox);
            this.timingParamGroupBox.Controls.Add(this.rateLabel);
            this.timingParamGroupBox.Controls.Add(this.sampleClockTextBox);
            this.timingParamGroupBox.Controls.Add(this.clockSourceLabel);
            this.timingParamGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.timingParamGroupBox.Location = new System.Drawing.Point(152, 8);
            this.timingParamGroupBox.Name = "timingParamGroupBox";
            this.timingParamGroupBox.Size = new System.Drawing.Size(136, 176);
            this.timingParamGroupBox.TabIndex = 3;
            this.timingParamGroupBox.TabStop = false;
            this.timingParamGroupBox.Text = "Timing Parameters";
            // 
            // sampleToReadTextBox
            // 
            this.sampleToReadTextBox.Location = new System.Drawing.Point(8, 136);
            this.sampleToReadTextBox.Name = "sampleToReadTextBox";
            this.sampleToReadTextBox.Size = new System.Drawing.Size(120, 20);
            this.sampleToReadTextBox.TabIndex = 5;
            this.sampleToReadTextBox.Text = "1000";
            // 
            // sampleReadLabel
            // 
            this.sampleReadLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.sampleReadLabel.Location = new System.Drawing.Point(8, 120);
            this.sampleReadLabel.Name = "sampleReadLabel";
            this.sampleReadLabel.Size = new System.Drawing.Size(96, 16);
            this.sampleReadLabel.TabIndex = 4;
            this.sampleReadLabel.Text = "Samples to Read:";
            // 
            // rateTextBox
            // 
            this.rateTextBox.Location = new System.Drawing.Point(8, 88);
            this.rateTextBox.Name = "rateTextBox";
            this.rateTextBox.Size = new System.Drawing.Size(120, 20);
            this.rateTextBox.TabIndex = 3;
            this.rateTextBox.Text = "1000.00";
            // 
            // rateLabel
            // 
            this.rateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rateLabel.Location = new System.Drawing.Point(8, 72);
            this.rateLabel.Name = "rateLabel";
            this.rateLabel.Size = new System.Drawing.Size(88, 16);
            this.rateLabel.TabIndex = 2;
            this.rateLabel.Text = "Rate:";
            // 
            // clockSourceLabel
            // 
            this.clockSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.clockSourceLabel.Location = new System.Drawing.Point(8, 24);
            this.clockSourceLabel.Name = "clockSourceLabel";
            this.clockSourceLabel.Size = new System.Drawing.Size(120, 16);
            this.clockSourceLabel.TabIndex = 0;
            this.clockSourceLabel.Text = "Sample Clock Source:";
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
            this.channelParameterGroupBox.Location = new System.Drawing.Point(8, 8);
            this.channelParameterGroupBox.Name = "channelParameterGroupBox";
            this.channelParameterGroupBox.Size = new System.Drawing.Size(136, 248);
            this.channelParameterGroupBox.TabIndex = 2;
            this.channelParameterGroupBox.TabStop = false;
            this.channelParameterGroupBox.Text = "Channel Parameters";
            // 
            // counterComboBox
            // 
            this.counterComboBox.Location = new System.Drawing.Point(8, 40);
            this.counterComboBox.Name = "counterComboBox";
            this.counterComboBox.Size = new System.Drawing.Size(121, 21);
            this.counterComboBox.TabIndex = 1;
            this.counterComboBox.Text = "Dev1/ctr0";
            // 
            // edgeGroupBox
            // 
            this.edgeGroupBox.Controls.Add(this.fallingRadioButton);
            this.edgeGroupBox.Controls.Add(this.risingRadioButton);
            this.edgeGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.edgeGroupBox.Location = new System.Drawing.Point(8, 168);
            this.edgeGroupBox.Name = "edgeGroupBox";
            this.edgeGroupBox.Size = new System.Drawing.Size(120, 72);
            this.edgeGroupBox.TabIndex = 6;
            this.edgeGroupBox.TabStop = false;
            this.edgeGroupBox.Text = "Edge";
            // 
            // fallingRadioButton
            // 
            this.fallingRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.fallingRadioButton.Location = new System.Drawing.Point(16, 40);
            this.fallingRadioButton.Name = "fallingRadioButton";
            this.fallingRadioButton.Size = new System.Drawing.Size(64, 24);
            this.fallingRadioButton.TabIndex = 1;
            this.fallingRadioButton.Text = "Falling";
            // 
            // risingRadioButton
            // 
            this.risingRadioButton.Checked = true;
            this.risingRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.risingRadioButton.Location = new System.Drawing.Point(16, 16);
            this.risingRadioButton.Name = "risingRadioButton";
            this.risingRadioButton.Size = new System.Drawing.Size(64, 24);
            this.risingRadioButton.TabIndex = 0;
            this.risingRadioButton.TabStop = true;
            this.risingRadioButton.Text = "Rising";
            // 
            // countDirectionLabel
            // 
            this.countDirectionLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.countDirectionLabel.Location = new System.Drawing.Point(8, 120);
            this.countDirectionLabel.Name = "countDirectionLabel";
            this.countDirectionLabel.Size = new System.Drawing.Size(88, 16);
            this.countDirectionLabel.TabIndex = 4;
            this.countDirectionLabel.Text = "Count Direction:";
            // 
            // initialCountTextBox
            // 
            this.initialCountTextBox.Location = new System.Drawing.Point(8, 88);
            this.initialCountTextBox.Name = "initialCountTextBox";
            this.initialCountTextBox.Size = new System.Drawing.Size(120, 20);
            this.initialCountTextBox.TabIndex = 3;
            this.initialCountTextBox.Text = "0";
            // 
            // initialCountLabel
            // 
            this.initialCountLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.initialCountLabel.Location = new System.Drawing.Point(8, 72);
            this.initialCountLabel.Name = "initialCountLabel";
            this.initialCountLabel.Size = new System.Drawing.Size(72, 16);
            this.initialCountLabel.TabIndex = 2;
            this.initialCountLabel.Text = "Initial Count:";
            // 
            // counterLabel
            // 
            this.counterLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.counterLabel.Location = new System.Drawing.Point(8, 24);
            this.counterLabel.Name = "counterLabel";
            this.counterLabel.Size = new System.Drawing.Size(100, 16);
            this.counterLabel.TabIndex = 0;
            this.counterLabel.Text = "Counter(s):";
            // 
            // countDirectionComboBox
            // 
            this.countDirectionComboBox.Items.AddRange(new object[] {
                                                                        "Count Up",
                                                                        "Count Down",
                                                                        "Externally Controlled"});
            this.countDirectionComboBox.Location = new System.Drawing.Point(8, 136);
            this.countDirectionComboBox.Name = "countDirectionComboBox";
            this.countDirectionComboBox.Size = new System.Drawing.Size(120, 21);
            this.countDirectionComboBox.TabIndex = 5;
            this.countDirectionComboBox.Text = "Count Up";
            this.countDirectionComboBox.SelectedIndexChanged += new System.EventHandler(this.countDirectionComboBox_SelectedIndexChanged);
            // 
            // dataGroupBox
            // 
            this.dataGroupBox.Controls.Add(this.dataListBox);
            this.dataGroupBox.Controls.Add(this.countLabel);
            this.dataGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.dataGroupBox.Location = new System.Drawing.Point(296, 8);
            this.dataGroupBox.Name = "dataGroupBox";
            this.dataGroupBox.Size = new System.Drawing.Size(128, 176);
            this.dataGroupBox.TabIndex = 4;
            this.dataGroupBox.TabStop = false;
            this.dataGroupBox.Text = "Data";
            // 
            // dataListBox
            // 
            this.dataListBox.Location = new System.Drawing.Point(8, 32);
            this.dataListBox.Name = "dataListBox";
            this.dataListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.dataListBox.Size = new System.Drawing.Size(112, 134);
            this.dataListBox.TabIndex = 1;
            // 
            // countLabel
            // 
            this.countLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.countLabel.Location = new System.Drawing.Point(8, 16);
            this.countLabel.Name = "countLabel";
            this.countLabel.Size = new System.Drawing.Size(40, 16);
            this.countLabel.TabIndex = 0;
            this.countLabel.Text = "Counts:";
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.stopButton.Location = new System.Drawing.Point(296, 216);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(128, 32);
            this.stopButton.TabIndex = 1;
            this.stopButton.Text = "Stop";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // startButton
            // 
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.startButton.Location = new System.Drawing.Point(160, 216);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(128, 32);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(434, 264);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.dataGroupBox);
            this.Controls.Add(this.channelParameterGroupBox);
            this.Controls.Add(this.timingParamGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Count Digital Events Buffered Continuous - External Clock";
            this.timingParamGroupBox.ResumeLayout(false);
            this.channelParameterGroupBox.ResumeLayout(false);
            this.edgeGroupBox.ResumeLayout(false);
            this.dataGroupBox.ResumeLayout(false);
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

        private void CounterReadCallback(IAsyncResult ar)
        {
            try
            {
                if (runningTask != null && runningTask == ar.AsyncState)
                {
                    data = myCounterReader.EndMemoryOptimizedReadMultiSampleDouble(ar, out actualNumberOFSamplesRead);
                

                    // Only display the first 10 samples acquired.
                    int samplesToShow; 
                    if (data.Length < 10)
                        samplesToShow = data.Length;
                    else
                        samplesToShow = 10;

                    dataListBox.BeginUpdate();
                    dataListBox.Items.Clear();
            
                    for (int i = 0; i < samplesToShow; ++i)
                    {
                        dataListBox.Items.Add(data[i]);
                    }

                    dataListBox.EndUpdate();

                    myCounterReader.BeginMemoryOptimizedReadMultiSampleDouble(
                        Convert.ToInt32(sampleToReadTextBox.Text),myCallBack,counterReadTask, data);
                }
            }
            catch(DaqException exception)
            {
                counterReadTask.Dispose();
                MessageBox.Show(exception.Message);
                runningTask = null;
                startButton.Enabled = true;
                stopButton.Enabled = false;
            }
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

                // Memory Optimized Read method needs an initialized array.
                data = new Double[Convert.ToInt32(sampleToReadTextBox.Text)];
                counterReadTask = new Task();

                counterReadTask.CIChannels.CreateCountEdgesChannel(counterComboBox.Text,
                    "",edgeType,Convert.ToInt64(initialCountTextBox.Text),countDirection);
                        
                counterReadTask.Timing.ConfigureSampleClock(sampleClockTextBox.Text,
                    Convert.ToDouble(rateTextBox.Text),SampleClockActiveEdge.Rising,
                    SampleQuantityMode.ContinuousSamples, 1000);

                runningTask = counterReadTask;
                myCounterReader = new CounterReader(counterReadTask.Stream);
        
                myCallBack = new AsyncCallback(CounterReadCallback);

                // Use SynchronizeCallbacks to specify that the object 
                // marshals callbacks across threads appropriately.
                myCounterReader.SynchronizeCallbacks = true;

                myCounterReader.BeginMemoryOptimizedReadMultiSampleDouble(
                        Convert.ToInt32(sampleToReadTextBox.Text), myCallBack, counterReadTask, data);

                startButton.Enabled = false;
                stopButton.Enabled = true;
                
            }
            catch(DaqException exception)
            {
                MessageBox.Show(exception.Message);
                counterReadTask.Dispose();
                startButton.Enabled = true;
                stopButton.Enabled = false;
                runningTask = null;
            }
        }

        private void stopButton_Click(object sender, System.EventArgs e)
        {
            runningTask = null;
            counterReadTask.Dispose();
            startButton.Enabled = true;
            stopButton.Enabled = false;
        }

        private void risingRadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            edgeType = CICountEdgesActiveEdge.Rising;
        }

        private void fallingRadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            edgeType = CICountEdgesActiveEdge.Falling;
        }

        private void countDirectionComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            switch  (countDirectionComboBox.SelectedIndex)
            {
                case 0 : countDirection = CICountEdgesCountDirection.Up;
                    break;
                case 1 : countDirection = CICountEdgesCountDirection.Down;
                    break;
                case 2 : countDirection = CICountEdgesCountDirection.ExternallyControlled;
                    break;
            }
        }
    }
}
