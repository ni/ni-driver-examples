/******************************************************************************
*
* Example program:
*   Meas2EdgeSeparation_BufCont
*
* Category:
*   CI
*
* Description:
*   This example demonstrates how to perform a continuous number of two edge
*   separation measurements 
*   on a counter input channel.  The first edge, second edge, minimum value,
*   maximum value, and samples to 
*   read are all configurable. This example shows how to perform a two edge
*   separation measurement on the 
*   counter's default input terminals (refer to the I/O Connections Overview below
*   for more information), 
*   but could easily be expanded to measure two edge separation on any PFI, RTSI,
*   or internal signal.Refer 
*   to your device documentation to see if your device supports two edge
*   separation measurements.
*
* Instructions for running:
*   1.  Select the physical channel which corresponds to the counter on the DAQ
*       device you want to perform a two edge separation measurement on.
*   2.  Enter the first edge and second edge corresponding to the two edges you
*       want the counter to measure.  Enter the maximum and minimum value to
*       specify the range of your unknown two edge separation.  Additionally,
*       you can choose the first and second edge input terminals.Note:  It is
*       important to set the maximum and minimum values of your unknown two edge
*       separation as accurately as possible so the best internal timebase can
*       be chosen to minimize measurement error.  The default values specify a
*       range that can be measured by the counter using the 20MHzTimebase.
*   3.  Set the samples to read.  This will determine how many samples are read
*       during each asynchronous cycle.
*
* Steps:
*   1.  Create a Task.
*   2.  Create a CIChannel object by using the CreateTwoEdgeSeparationChannel
*       method.  The first and second edge parameters are used to specify the
*       rising or falling edge of one digital signal and the rising or falling
*       edge of another digital signal.  It is important to set the maximum and
*       minimum values of your unknown two edge separation as accurately as
*       possible so the best internal timebase can be chosen to minimize
*       measurement error.  The default values specify a range that can be
*       measured by the counter using the 20MHzTimebase.
*   3.  Call the ConfigureImplicit method to configure the sample mode to be
*       continuous.  Note: For time measurements with counters, the implicit
*       timing method is used because the signals being measured determine the
*       sample rate.
*   4.  Create a CounterReader object and use the BeginReadMultiSampleDouble
*       method to initiate the measurement and return the data.
*   5.  For continuous measurements, the counter will continually read all
*       available data until the Stop button is pressed.
*   6.  Dispose the Task object to clean-up any resources associated with the
*       task.
*   7.  Handle any DaqExceptions, if they occur.
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
*   The counter will perform a two edge separation measurement on the first and
*   second edge input terminals of the counter specified by the physical
*   channel.In this example the two edge separation will be measured on the
*   default input terminals on ctr0. For more information on the default counter
*   input and output terminals for your device, open the NI-DAQmx Help, and
*   refer to Counter Signal Connections found under the Device Considerations
*   book in the table of contents.
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

namespace NationalInstruments.Examples.Meas2EdgeSeparation_BufCont
{
    /// <summary>
    /// Summary description for MainForm.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        private Task myTask;
        private Task runningTask;
        private CounterReader counterInReader;
        private AsyncCallback asyncCB;
        private double[] data;
        private int numberOfSamples;
        private CITwoEdgeSeparationFirstEdge firstEdge;
        private CITwoEdgeSeparationSecondEdge secondEdge;
        private System.Windows.Forms.GroupBox channelParametersGroupBox;
        private System.Windows.Forms.Label maximumLabel;
        private System.Windows.Forms.Label minimumLabel;
        private System.Windows.Forms.Label physicalChannelLabel;
        private System.Windows.Forms.TextBox maximumTextBox;
        private System.Windows.Forms.TextBox minimumTextBox;
        private System.Windows.Forms.GroupBox acqResultGroupBox;
        private System.Windows.Forms.Label resultLabel;
        private System.Windows.Forms.ComboBox firstEdgeComboBox;
        private System.Windows.Forms.Label firstEdgeLabel;
        private System.Windows.Forms.Label secondEdgeLabel;
        private System.Windows.Forms.ComboBox secondEdgeComboBox;
        private System.Windows.Forms.TextBox acquisitionDataTextBox;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.ComboBox counterComboBox;
        private GroupBox timingGroupBox;
        private Label samplesLabel;
        private TextBox samplesTextBox;
        private Button stopButton;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private int actualNumberOfSamplesRead;

        public MainForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            firstEdgeComboBox.SelectedIndex = 0;
            secondEdgeComboBox.SelectedIndex = 1;

            counterComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.CI, PhysicalChannelAccess.External));
            if (counterComboBox.Items.Count > 0)
            {
                counterComboBox.SelectedIndex = 0;
                startButton.Enabled = true;
            }
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
                    runningTask = null;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.channelParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.counterComboBox = new System.Windows.Forms.ComboBox();
            this.secondEdgeLabel = new System.Windows.Forms.Label();
            this.secondEdgeComboBox = new System.Windows.Forms.ComboBox();
            this.firstEdgeLabel = new System.Windows.Forms.Label();
            this.maximumLabel = new System.Windows.Forms.Label();
            this.minimumLabel = new System.Windows.Forms.Label();
            this.physicalChannelLabel = new System.Windows.Forms.Label();
            this.maximumTextBox = new System.Windows.Forms.TextBox();
            this.minimumTextBox = new System.Windows.Forms.TextBox();
            this.firstEdgeComboBox = new System.Windows.Forms.ComboBox();
            this.acqResultGroupBox = new System.Windows.Forms.GroupBox();
            this.acquisitionDataTextBox = new System.Windows.Forms.TextBox();
            this.resultLabel = new System.Windows.Forms.Label();
            this.startButton = new System.Windows.Forms.Button();
            this.timingGroupBox = new System.Windows.Forms.GroupBox();
            this.samplesLabel = new System.Windows.Forms.Label();
            this.samplesTextBox = new System.Windows.Forms.TextBox();
            this.stopButton = new System.Windows.Forms.Button();
            this.channelParametersGroupBox.SuspendLayout();
            this.acqResultGroupBox.SuspendLayout();
            this.timingGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // channelParametersGroupBox
            // 
            this.channelParametersGroupBox.Controls.Add(this.counterComboBox);
            this.channelParametersGroupBox.Controls.Add(this.secondEdgeLabel);
            this.channelParametersGroupBox.Controls.Add(this.secondEdgeComboBox);
            this.channelParametersGroupBox.Controls.Add(this.firstEdgeLabel);
            this.channelParametersGroupBox.Controls.Add(this.maximumLabel);
            this.channelParametersGroupBox.Controls.Add(this.minimumLabel);
            this.channelParametersGroupBox.Controls.Add(this.physicalChannelLabel);
            this.channelParametersGroupBox.Controls.Add(this.maximumTextBox);
            this.channelParametersGroupBox.Controls.Add(this.minimumTextBox);
            this.channelParametersGroupBox.Controls.Add(this.firstEdgeComboBox);
            this.channelParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.channelParametersGroupBox.Location = new System.Drawing.Point(8, 8);
            this.channelParametersGroupBox.Name = "channelParametersGroupBox";
            this.channelParametersGroupBox.Size = new System.Drawing.Size(248, 212);
            this.channelParametersGroupBox.TabIndex = 0;
            this.channelParametersGroupBox.TabStop = false;
            this.channelParametersGroupBox.Text = "Channel Parameters:";
            // 
            // counterComboBox
            // 
            this.counterComboBox.Location = new System.Drawing.Point(136, 24);
            this.counterComboBox.Name = "counterComboBox";
            this.counterComboBox.Size = new System.Drawing.Size(96, 21);
            this.counterComboBox.TabIndex = 1;
            this.counterComboBox.Text = "Dev1/ctr0";
            // 
            // secondEdgeLabel
            // 
            this.secondEdgeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.secondEdgeLabel.Location = new System.Drawing.Point(12, 184);
            this.secondEdgeLabel.Name = "secondEdgeLabel";
            this.secondEdgeLabel.Size = new System.Drawing.Size(112, 16);
            this.secondEdgeLabel.TabIndex = 8;
            this.secondEdgeLabel.Text = "Second Edge:";
            // 
            // secondEdgeComboBox
            // 
            this.secondEdgeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.secondEdgeComboBox.Items.AddRange(new object[] {
            "Rising",
            "Falling"});
            this.secondEdgeComboBox.Location = new System.Drawing.Point(136, 182);
            this.secondEdgeComboBox.Name = "secondEdgeComboBox";
            this.secondEdgeComboBox.Size = new System.Drawing.Size(96, 21);
            this.secondEdgeComboBox.TabIndex = 9;
            // 
            // firstEdgeLabel
            // 
            this.firstEdgeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.firstEdgeLabel.Location = new System.Drawing.Point(12, 144);
            this.firstEdgeLabel.Name = "firstEdgeLabel";
            this.firstEdgeLabel.Size = new System.Drawing.Size(112, 16);
            this.firstEdgeLabel.TabIndex = 6;
            this.firstEdgeLabel.Text = "First Edge:";
            // 
            // maximumLabel
            // 
            this.maximumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.maximumLabel.Location = new System.Drawing.Point(12, 105);
            this.maximumLabel.Name = "maximumLabel";
            this.maximumLabel.Size = new System.Drawing.Size(120, 16);
            this.maximumLabel.TabIndex = 4;
            this.maximumLabel.Text = "Maximum Value (sec):";
            // 
            // minimumLabel
            // 
            this.minimumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.minimumLabel.Location = new System.Drawing.Point(12, 65);
            this.minimumLabel.Name = "minimumLabel";
            this.minimumLabel.Size = new System.Drawing.Size(120, 18);
            this.minimumLabel.TabIndex = 2;
            this.minimumLabel.Text = "Minimum Value (sec):";
            // 
            // physicalChannelLabel
            // 
            this.physicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.physicalChannelLabel.Location = new System.Drawing.Point(12, 26);
            this.physicalChannelLabel.Name = "physicalChannelLabel";
            this.physicalChannelLabel.Size = new System.Drawing.Size(96, 16);
            this.physicalChannelLabel.TabIndex = 0;
            this.physicalChannelLabel.Text = "Counter:";
            // 
            // maximumTextBox
            // 
            this.maximumTextBox.Location = new System.Drawing.Point(136, 103);
            this.maximumTextBox.Name = "maximumTextBox";
            this.maximumTextBox.Size = new System.Drawing.Size(96, 20);
            this.maximumTextBox.TabIndex = 5;
            this.maximumTextBox.Text = "0.838860750";
            // 
            // minimumTextBox
            // 
            this.minimumTextBox.Location = new System.Drawing.Point(136, 64);
            this.minimumTextBox.Name = "minimumTextBox";
            this.minimumTextBox.Size = new System.Drawing.Size(96, 20);
            this.minimumTextBox.TabIndex = 3;
            this.minimumTextBox.Text = "0.000000100";
            // 
            // firstEdgeComboBox
            // 
            this.firstEdgeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.firstEdgeComboBox.Items.AddRange(new object[] {
            "Rising",
            "Falling"});
            this.firstEdgeComboBox.Location = new System.Drawing.Point(136, 142);
            this.firstEdgeComboBox.Name = "firstEdgeComboBox";
            this.firstEdgeComboBox.Size = new System.Drawing.Size(96, 21);
            this.firstEdgeComboBox.TabIndex = 7;
            // 
            // acqResultGroupBox
            // 
            this.acqResultGroupBox.Controls.Add(this.acquisitionDataTextBox);
            this.acqResultGroupBox.Controls.Add(this.resultLabel);
            this.acqResultGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.acqResultGroupBox.Location = new System.Drawing.Point(264, 8);
            this.acqResultGroupBox.Name = "acqResultGroupBox";
            this.acqResultGroupBox.Size = new System.Drawing.Size(152, 96);
            this.acqResultGroupBox.TabIndex = 2;
            this.acqResultGroupBox.TabStop = false;
            this.acqResultGroupBox.Text = "Acquisition Results:";
            // 
            // acquisitionDataTextBox
            // 
            this.acquisitionDataTextBox.Location = new System.Drawing.Point(16, 56);
            this.acquisitionDataTextBox.Name = "acquisitionDataTextBox";
            this.acquisitionDataTextBox.ReadOnly = true;
            this.acquisitionDataTextBox.Size = new System.Drawing.Size(120, 20);
            this.acquisitionDataTextBox.TabIndex = 1;
            this.acquisitionDataTextBox.Text = "0.0";
            // 
            // resultLabel
            // 
            this.resultLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.resultLabel.Location = new System.Drawing.Point(16, 24);
            this.resultLabel.Name = "resultLabel";
            this.resultLabel.Size = new System.Drawing.Size(112, 32);
            this.resultLabel.TabIndex = 0;
            this.resultLabel.Text = "Measured Two Edge Separation (sec):";
            // 
            // startButton
            // 
            this.startButton.Enabled = false;
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.startButton.Location = new System.Drawing.Point(272, 231);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(128, 32);
            this.startButton.TabIndex = 3;
            this.startButton.Text = "Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // timingGroupBox
            // 
            this.timingGroupBox.Controls.Add(this.samplesLabel);
            this.timingGroupBox.Controls.Add(this.samplesTextBox);
            this.timingGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.timingGroupBox.Location = new System.Drawing.Point(8, 231);
            this.timingGroupBox.Name = "timingGroupBox";
            this.timingGroupBox.Size = new System.Drawing.Size(248, 63);
            this.timingGroupBox.TabIndex = 1;
            this.timingGroupBox.TabStop = false;
            this.timingGroupBox.Text = "Timing Parameters:";
            // 
            // samplesLabel
            // 
            this.samplesLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.samplesLabel.Location = new System.Drawing.Point(12, 25);
            this.samplesLabel.Name = "samplesLabel";
            this.samplesLabel.Size = new System.Drawing.Size(120, 18);
            this.samplesLabel.TabIndex = 0;
            this.samplesLabel.Text = "Samples to Read:";
            // 
            // samplesTextBox
            // 
            this.samplesTextBox.Location = new System.Drawing.Point(136, 24);
            this.samplesTextBox.Name = "samplesTextBox";
            this.samplesTextBox.Size = new System.Drawing.Size(96, 20);
            this.samplesTextBox.TabIndex = 1;
            this.samplesTextBox.Text = "1000";
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.stopButton.Location = new System.Drawing.Point(272, 269);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(128, 32);
            this.stopButton.TabIndex = 4;
            this.stopButton.Text = "Stop";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(426, 310);
            this.Controls.Add(this.timingGroupBox);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.acqResultGroupBox);
            this.Controls.Add(this.channelParametersGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Measure Two Edge Separation - Buffered Continuous";
            this.channelParametersGroupBox.ResumeLayout(false);
            this.channelParametersGroupBox.PerformLayout();
            this.acqResultGroupBox.ResumeLayout(false);
            this.acqResultGroupBox.PerformLayout();
            this.timingGroupBox.ResumeLayout(false);
            this.timingGroupBox.PerformLayout();
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
                // Determine rising or falling edges
                switch (firstEdgeComboBox.SelectedItem.ToString())
                {
                    case "Rising":
                        firstEdge = CITwoEdgeSeparationFirstEdge.Rising;
                        break;
                    case "Falling":
                        firstEdge = CITwoEdgeSeparationFirstEdge.Falling;
                        break;
                }

                switch (secondEdgeComboBox.SelectedItem.ToString())
                {
                    case "Rising":
                        secondEdge = CITwoEdgeSeparationSecondEdge.Rising;
                        break;
                    case "Falling":
                        secondEdge = CITwoEdgeSeparationSecondEdge.Falling;
                        break;
                }

                // Create the task
                myTask = new Task();
                
                // Create the two edge separation counter channel
                myTask.CIChannels.CreateTwoEdgeSeparationChannel(
                    counterComboBox.Text,"",
                    Convert.ToDouble(minimumTextBox.Text),
                    Convert.ToDouble(maximumTextBox.Text),
                    firstEdge, secondEdge, CITwoEdgeSeparationUnits.Seconds);

                // Determine the number of samples per read
                numberOfSamples = Convert.ToInt32(samplesTextBox.Text);

                // Configure the task for continuous implicit sampling
                myTask.Timing.ConfigureImplicit(SampleQuantityMode.ContinuousSamples, numberOfSamples);

                runningTask = myTask;

                // Create the reader
                counterInReader = new CounterReader(myTask.Stream);

                // Use SynchronizeCallbacks to specify that the object 
                // marshals callbacks across threads appropriately.
                counterInReader.SynchronizeCallbacks = true;

                // Memory Optimized Read method needs an initialized array.
                data = new double[numberOfSamples];
                // Create the callback and start the first read
                asyncCB = new AsyncCallback(CounterInCallback);
                counterInReader.BeginMemoryOptimizedReadMultiSampleDouble(numberOfSamples, asyncCB, myTask, data);

                // Disable UI
                startButton.Enabled = false;
                stopButton.Enabled = true;
                channelParametersGroupBox.Enabled = false;
                timingGroupBox.Enabled = false;
            }
            catch(DaqException exception)
            {
                MessageBox.Show(exception.Message);
                stopButton_Click(null, null);
            }
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            // Dispose task and enable UI
            runningTask = null;
            myTask.Dispose();
            startButton.Enabled = true;
            stopButton.Enabled = false;
            channelParametersGroupBox.Enabled = true;
            timingGroupBox.Enabled = true;
        }

        private void CounterInCallback(IAsyncResult ar)
        {
            try
            {
                // Prevent stale events
                if (runningTask != null && runningTask == ar.AsyncState)
                {
                    // Retrieve the data
                    data = counterInReader.EndMemoryOptimizedReadMultiSampleDouble(ar, out actualNumberOfSamplesRead);

                    // Display the data
                    acquisitionDataTextBox.Text = data[0].ToString();

                    // Start the next read
                    counterInReader.BeginMemoryOptimizedReadMultiSampleDouble(numberOfSamples, asyncCB, myTask, data);
                }
            }
            catch (DaqException exception)
            {
                MessageBox.Show(exception.Message);
                stopButton_Click(null, null);
            }
        }
    }
}
