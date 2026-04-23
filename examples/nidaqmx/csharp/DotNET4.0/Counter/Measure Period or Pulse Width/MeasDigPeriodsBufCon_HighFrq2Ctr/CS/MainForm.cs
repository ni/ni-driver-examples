/******************************************************************************
*
* Example program:
*   MeasDigPeriodsBufCon_HighFrq2Ctr
*
* Category:
*   CI
*
* Description:
*   This example demonstrates how to measure periods using two counters on a
*   counter input channel.  The measurement time, sample mode, and samples per
*   read are configurable.This example shows how to measure period on the
*   counters default input terminal, (see I/O Connections Overview below for
*   more information), , but could easily be expanded to measure periods on any
*   PFI, RTSI, or internal signal.  Additionally, this example could be extended
*   to measure period with other measurement methods for different frequency and
*   quantization error requirements.
*
* Instructions for running:
*   1.  Enter the physical channel which corresponds to the counter you want to
*       use to measure period on the DAQ device.
*   2.  Enter the measurement time to specify how often a period is calculated
*       by counting the number of edges that have passed in the elapsed time. 
*       Additionally, you can change the input terminal where the period is
*       measured using properties on the CIChannel object.Note: Use the
*       GenDigPulseTrain_Continuous example to verify that you are measuring
*       correctly on the DAQ device.
*
* Steps:
*   1.  Create a Task object. Create a CIChannel object for period measurement
*       by using the CreateCIPeriodChannel method. The edge parameter is used to
*       determine if the counter will begin measuring on a rising or falling
*       edge.  The measurement time specifies how often a period is calculated
*       by counting the number of edges that have passed in the elapsed time.
*       Note: The maximum and minimum values are not used when measuring period
*       using the High Frequency 2 Ctr method.
*   2.  Call the ConfigureImplicit method to configure the sample mode.Note: For
*       time measurements with counters, ConfigureImplicit is used because the
*       signal being measured itself determines the sample rate.  This is unlike
*       buffered event counting, where an external sample clock must be used.
*   3.  Create a CounterReader object and use the
*       CounterReader.BeginReadMultiSampleDouble method to read the data and
*       register an asynchronous callback to be called when the requested data
*       is available.
*   4.  Call the CounterReader.EndReadMultiSampleDouble method to get the data
*       in the asynchronous callback. Call BeginReadMultiSampleDouble again in
*       the callback to continue retrieving the data being acquired.
*   5.  Call the Task.Dispose method to stop the acquisition and free resources
*       used by the task.
*   6.  Handle any DaqExceptions and display a message for errors.
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

namespace NationalInstruments.Examples.MeasDigPeriodsBufCon_HighFrq2Ctr
{
    /// <summary>
    /// Summary description for MainForm.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        internal System.Windows.Forms.GroupBox dataGroupBox;
        internal System.Windows.Forms.ListBox periodListBox;
        internal System.Windows.Forms.Button stopButton;
        internal System.Windows.Forms.Button startButton;
        internal System.Windows.Forms.GroupBox channelParamGroupBox;
        internal System.Windows.Forms.Label counterLabel;
        internal System.Windows.Forms.Label measurementTimeLabel;
        internal System.Windows.Forms.TextBox measurementTimeTextBox;
        private Task myTask;
        private double maxValue;
        private double minValue;
        private long divisor;
        private int samplesPerRead;
        private double[] samples;
        private int actualNumberOfSamplesRead;
        private Task runningTask;
        private CounterReader myCounterReader;
        private System.Windows.Forms.GroupBox edgeGroupBox;
        private System.Windows.Forms.RadioButton risingRadioButton;
        private System.Windows.Forms.RadioButton fallingRadioButton;
        private CIPeriodStartingEdge edge;
        private AsyncCallback myCallBack;
        internal System.Windows.Forms.Label periodsLabel;
        private System.Windows.Forms.GroupBox timingParametersGroupBox;
        private System.Windows.Forms.TextBox samplesPerReadTextBox;
        private System.Windows.Forms.Label samplesPerReadLabel;
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
            maxValue = 0.838861;
            minValue = 0;
            divisor = 4;
            edge = CIPeriodStartingEdge.Rising;
            runningTask = null;

            counterComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.CI, PhysicalChannelAccess.External));
            if (counterComboBox.Items.Count > 0)
                counterComboBox.SelectedIndex = 0;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
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
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainForm));
            this.dataGroupBox = new System.Windows.Forms.GroupBox();
            this.periodListBox = new System.Windows.Forms.ListBox();
            this.periodsLabel = new System.Windows.Forms.Label();
            this.stopButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.channelParamGroupBox = new System.Windows.Forms.GroupBox();
            this.counterComboBox = new System.Windows.Forms.ComboBox();
            this.edgeGroupBox = new System.Windows.Forms.GroupBox();
            this.fallingRadioButton = new System.Windows.Forms.RadioButton();
            this.risingRadioButton = new System.Windows.Forms.RadioButton();
            this.counterLabel = new System.Windows.Forms.Label();
            this.measurementTimeLabel = new System.Windows.Forms.Label();
            this.measurementTimeTextBox = new System.Windows.Forms.TextBox();
            this.timingParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.samplesPerReadTextBox = new System.Windows.Forms.TextBox();
            this.samplesPerReadLabel = new System.Windows.Forms.Label();
            this.dataGroupBox.SuspendLayout();
            this.channelParamGroupBox.SuspendLayout();
            this.edgeGroupBox.SuspendLayout();
            this.timingParametersGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGroupBox
            // 
            this.dataGroupBox.Controls.Add(this.periodListBox);
            this.dataGroupBox.Controls.Add(this.periodsLabel);
            this.dataGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.dataGroupBox.Location = new System.Drawing.Point(224, 8);
            this.dataGroupBox.Name = "dataGroupBox";
            this.dataGroupBox.Size = new System.Drawing.Size(200, 216);
            this.dataGroupBox.TabIndex = 4;
            this.dataGroupBox.TabStop = false;
            this.dataGroupBox.Text = "Data:";
            // 
            // periodListBox
            // 
            this.periodListBox.Location = new System.Drawing.Point(8, 32);
            this.periodListBox.Name = "periodListBox";
            this.periodListBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.periodListBox.Size = new System.Drawing.Size(184, 173);
            this.periodListBox.TabIndex = 1;
            // 
            // periodsLabel
            // 
            this.periodsLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.periodsLabel.Location = new System.Drawing.Point(8, 16);
            this.periodsLabel.Name = "periodsLabel";
            this.periodsLabel.Size = new System.Drawing.Size(96, 16);
            this.periodsLabel.TabIndex = 0;
            this.periodsLabel.Text = "Periods (sec):";
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.stopButton.Location = new System.Drawing.Point(264, 272);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(128, 32);
            this.stopButton.TabIndex = 1;
            this.stopButton.Text = "Stop";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // startButton
            // 
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.startButton.Location = new System.Drawing.Point(264, 240);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(128, 32);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // channelParamGroupBox
            // 
            this.channelParamGroupBox.Controls.Add(this.counterComboBox);
            this.channelParamGroupBox.Controls.Add(this.edgeGroupBox);
            this.channelParamGroupBox.Controls.Add(this.counterLabel);
            this.channelParamGroupBox.Controls.Add(this.measurementTimeLabel);
            this.channelParamGroupBox.Controls.Add(this.measurementTimeTextBox);
            this.channelParamGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.channelParamGroupBox.Location = new System.Drawing.Point(8, 8);
            this.channelParamGroupBox.Name = "channelParamGroupBox";
            this.channelParamGroupBox.Size = new System.Drawing.Size(208, 216);
            this.channelParamGroupBox.TabIndex = 2;
            this.channelParamGroupBox.TabStop = false;
            this.channelParamGroupBox.Text = "Channel Parameters:";
            // 
            // counterComboBox
            // 
            this.counterComboBox.Location = new System.Drawing.Point(16, 40);
            this.counterComboBox.Name = "counterComboBox";
            this.counterComboBox.Size = new System.Drawing.Size(176, 21);
            this.counterComboBox.TabIndex = 1;
            this.counterComboBox.Text = "Dev1/ctr0";
            // 
            // edgeGroupBox
            // 
            this.edgeGroupBox.Controls.Add(this.fallingRadioButton);
            this.edgeGroupBox.Controls.Add(this.risingRadioButton);
            this.edgeGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.edgeGroupBox.Location = new System.Drawing.Point(16, 72);
            this.edgeGroupBox.Name = "edgeGroupBox";
            this.edgeGroupBox.Size = new System.Drawing.Size(176, 72);
            this.edgeGroupBox.TabIndex = 2;
            this.edgeGroupBox.TabStop = false;
            this.edgeGroupBox.Text = "Edge:";
            // 
            // fallingRadioButton
            // 
            this.fallingRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.fallingRadioButton.Location = new System.Drawing.Point(24, 40);
            this.fallingRadioButton.Name = "fallingRadioButton";
            this.fallingRadioButton.Size = new System.Drawing.Size(72, 24);
            this.fallingRadioButton.TabIndex = 1;
            this.fallingRadioButton.Text = "Falling";
            this.fallingRadioButton.CheckedChanged += new System.EventHandler(this.fallingRadioButton_CheckedChanged);
            // 
            // risingRadioButton
            // 
            this.risingRadioButton.Checked = true;
            this.risingRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.risingRadioButton.Location = new System.Drawing.Point(24, 16);
            this.risingRadioButton.Name = "risingRadioButton";
            this.risingRadioButton.Size = new System.Drawing.Size(72, 24);
            this.risingRadioButton.TabIndex = 0;
            this.risingRadioButton.TabStop = true;
            this.risingRadioButton.Text = "Rising";
            this.risingRadioButton.CheckedChanged += new System.EventHandler(this.risingRadioButton_CheckedChanged);
            // 
            // counterLabel
            // 
            this.counterLabel.BackColor = System.Drawing.SystemColors.Control;
            this.counterLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.counterLabel.Location = new System.Drawing.Point(16, 24);
            this.counterLabel.Name = "counterLabel";
            this.counterLabel.Size = new System.Drawing.Size(100, 16);
            this.counterLabel.TabIndex = 0;
            this.counterLabel.Text = "Counter(s):";
            // 
            // measurementTimeLabel
            // 
            this.measurementTimeLabel.BackColor = System.Drawing.SystemColors.Control;
            this.measurementTimeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.measurementTimeLabel.Location = new System.Drawing.Point(16, 160);
            this.measurementTimeLabel.Name = "measurementTimeLabel";
            this.measurementTimeLabel.Size = new System.Drawing.Size(136, 16);
            this.measurementTimeLabel.TabIndex = 3;
            this.measurementTimeLabel.Text = "Measurement Time (sec):";
            // 
            // measurementTimeTextBox
            // 
            this.measurementTimeTextBox.Location = new System.Drawing.Point(16, 176);
            this.measurementTimeTextBox.Name = "measurementTimeTextBox";
            this.measurementTimeTextBox.Size = new System.Drawing.Size(176, 20);
            this.measurementTimeTextBox.TabIndex = 4;
            this.measurementTimeTextBox.Text = "0.000100";
            // 
            // timingParametersGroupBox
            // 
            this.timingParametersGroupBox.Controls.Add(this.samplesPerReadTextBox);
            this.timingParametersGroupBox.Controls.Add(this.samplesPerReadLabel);
            this.timingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.timingParametersGroupBox.Location = new System.Drawing.Point(8, 232);
            this.timingParametersGroupBox.Name = "timingParametersGroupBox";
            this.timingParametersGroupBox.Size = new System.Drawing.Size(208, 80);
            this.timingParametersGroupBox.TabIndex = 3;
            this.timingParametersGroupBox.TabStop = false;
            this.timingParametersGroupBox.Text = "Timing Parameters:";
            // 
            // samplesPerReadTextBox
            // 
            this.samplesPerReadTextBox.Location = new System.Drawing.Point(16, 40);
            this.samplesPerReadTextBox.Name = "samplesPerReadTextBox";
            this.samplesPerReadTextBox.Size = new System.Drawing.Size(176, 20);
            this.samplesPerReadTextBox.TabIndex = 1;
            this.samplesPerReadTextBox.Text = "1000";
            // 
            // samplesPerReadLabel
            // 
            this.samplesPerReadLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.samplesPerReadLabel.Location = new System.Drawing.Point(16, 24);
            this.samplesPerReadLabel.Name = "samplesPerReadLabel";
            this.samplesPerReadLabel.Size = new System.Drawing.Size(128, 23);
            this.samplesPerReadLabel.TabIndex = 0;
            this.samplesPerReadLabel.Text = "Samples Per Read:";
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(434, 320);
            this.Controls.Add(this.timingParametersGroupBox);
            this.Controls.Add(this.dataGroupBox);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.channelParamGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Meas Dig Periods Buffered Continuous - High Freq 2 Ctr";
            this.dataGroupBox.ResumeLayout(false);
            this.channelParamGroupBox.ResumeLayout(false);
            this.edgeGroupBox.ResumeLayout(false);
            this.timingParametersGroupBox.ResumeLayout(false);
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

                myTask.CIChannels.CreatePeriodChannel(counterComboBox.Text, "", minValue,
                    maxValue, edge, CIPeriodMeasurementMethod.HighFrequencyTwoCounter,
                    Convert.ToDouble(measurementTimeTextBox.Text), divisor,
                    CIPeriodUnits.Seconds);

                myTask.Timing.ConfigureImplicit(SampleQuantityMode.ContinuousSamples, 1000);

                runningTask = myTask;
                myCounterReader = new CounterReader(myTask.Stream);

                // Use SynchronizeCallbacks to specify that the object 
                // marshals callbacks across threads appropriately.
                myCounterReader.SynchronizeCallbacks = true;

                samplesPerRead = Convert.ToInt32(samplesPerReadTextBox.Text);


                myCallBack = new AsyncCallback(CounterInCallback);

                // Memory Optimized Read method needs an initialized array.
                samples = new double[samplesPerRead];
                myCounterReader.BeginMemoryOptimizedReadMultiSampleDouble(samplesPerRead, myCallBack, myTask, samples);

                startButton.Enabled = false;
                stopButton.Enabled = true;
            }
            catch (Exception exception)
            {
                myTask.Dispose();
                stopButton.Enabled = false;
                runningTask = null;
                MessageBox.Show(exception.Message);
                startButton.Enabled = true;
            }
        }

        private void stopButton_Click(object sender, System.EventArgs e)
        {
            runningTask = null;
            stopButton.Enabled = false;
            myTask.Dispose();
            startButton.Enabled = true;
        }

        private void CounterInCallback(IAsyncResult ar)
        {
            try
            {
                if (runningTask != null && runningTask == ar.AsyncState)
                {
                    samples = myCounterReader.EndMemoryOptimizedReadMultiSampleDouble(ar, out actualNumberOfSamplesRead);

                    periodListBox.BeginUpdate();
                    periodListBox.Items.Clear();

                    //Display only the first 10 data points acquired
                    int samplesToDisplay = 10;
                    if (samples.Length < samplesToDisplay)
                        samplesToDisplay = samples.Length;

                    for (int i = 0; i < samplesToDisplay; ++i)
                        periodListBox.Items.Add(samples[i]);

                    periodListBox.EndUpdate();

                    myCounterReader.BeginMemoryOptimizedReadMultiSampleDouble(samplesPerRead, myCallBack, myTask, samples);
                }
            }
            catch (DaqException exception)
            {
                myTask.Dispose();
                stopButton.Enabled = false;
                runningTask = null;
                MessageBox.Show(exception.Message);
                startButton.Enabled = true;
            }
        }

        private void risingRadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            edge = CIPeriodStartingEdge.Rising;
        }

        private void fallingRadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            edge = CIPeriodStartingEdge.Falling;
        }
    }
}
