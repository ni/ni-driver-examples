/******************************************************************************
*
* Example program:
*   MeasAngularPositionBufferedCont_ExtClk
*
* Category:
*   CI
*
* Description:
*   This example demonstrates how to measure angular position using a quadrature
*   encoder on a counter input channel.  The decoding type, pulses per
*   revolution, z-index enable, z-index phase, z-index value, and sample clock
*   source are all configurable. Position is measured on the counter's default
*   A, B, and Z input terminals (see I/O Connections Overview below for more
*   information).Note: For buffered position measurement, an external sample
*   clock is necessary to signal when a sample should be inserted into the
*   buffer.  This is set by the sample clock source.
*
* Instructions for running:
*   1.  Enter the physical channel which corresponds to the counter you want to
*       use to    measure position on the DAQ device.
*   2.  Enter the decoding type, pulses per revolution, z-index enable, z-index
*       phase, and z-index value to specify how you want the counter to measure
*       position.  Set the sample clock source.
*   3.  Set the samples to read control.  This will determine how many samples
*       are read each time.Note:  An external sample clock must be used. 
*       Counters do not have an internal sample clock available.  You can use
*       the Gen Dig Pulse Train-Continuous example to generate a pulse train on
*       another counter and connect it to the sample clock source you are using
*       n this example.
*
* Steps:
*   1.  Create a Task, then create a CIChannel using the
*       CreateAngularEncoderChannel method on CIChannels.  The decoding type,
*       pulses per revolution, z-index enable, z-index phase, and z-index value
*       parameters are used to determine how the counter should measure
*       position.
*   2.  Use the ConfigureSampleClock of Task.Timing to configure the external
*       sample clock timing parameters such as sample mode and sample clock
*       source.  The sample clock source determines when a sample will be
*       inserted into the buffer.  The 100kHz, 20MHz, and 80MHz timebases cannot
*       be used as the sample clock source. The edge parameter can be used to
*       determine when a sample is taken.
*   3.  Create a CounterReader and use BeginReadMultiSampleDouble() to initiate
*       an asynchronous read.  The counter will be preloaded with the initial
*       angle when the task is started by the initial read.
*   4.  For continuous measurements, the counter will continually read all
*       available data until the Stop button is pressed.
*   5.  Call Task.Dispose to stop the task and de-allocate the resources
*       allocated by the task.
*   6.  Handle any DaqExceptions and display any error messages.
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

namespace NationalInstruments.Examples.MeasAngularPositionBufferedCont_ExtClk
{
    /// <summary>
    /// Summary description for MainForm.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        private Task myTask;
        private CounterReader counterInReader;
        private AsyncCallback asyncCB;
        private CIEncoderDecodingType encoderType;
        private CIEncoderZIndexPhase encoderPhase;
        private bool zIndexEnable;
        private Task runningTask;
        private int numberOfSamples;
        private double[] data;
        private int actualNumberOfSamplesRead;

        private System.Windows.Forms.GroupBox channelParametersGroupBox;
        private System.Windows.Forms.Label physicalChannelLabel;
        private System.Windows.Forms.Label zIndexPhaseLabel;
        private System.Windows.Forms.ComboBox zIndexPhaseComboBox;
        private System.Windows.Forms.Label pulsesPerRevLabel;
        private System.Windows.Forms.TextBox pulsePerRevTextBox;
        private System.Windows.Forms.TextBox zIndexValueTextBox;
        internal System.Windows.Forms.GroupBox dataGroupBox;
        private System.Windows.Forms.TextBox samplesToReadTextBox;
        private System.Windows.Forms.Label samplesToReadLabel;
        private System.Windows.Forms.Label rateLabel;
        private System.Windows.Forms.TextBox rateTextBox;
        internal System.Windows.Forms.Button stopButton;
        internal System.Windows.Forms.Button startButton;
        internal System.Windows.Forms.ListBox dataListBox;
        private System.Windows.Forms.CheckBox zIndexEnabledCheckBox;
        private System.Windows.Forms.Label sampleClkSourceLabel;
        private System.Windows.Forms.TextBox sampleClkSourceTextBox;
        private System.Windows.Forms.Label decodingTypeLabel;
        private System.Windows.Forms.Label zIndexValueLabel;
        private System.Windows.Forms.ComboBox decodingTypeComboBox;
        private System.Windows.Forms.GroupBox timingParametersGroupBox;
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
            zIndexPhaseComboBox.SelectedIndex = 0;
            decodingTypeComboBox.SelectedIndex = 0;
            asyncCB = new AsyncCallback(CounterInCallback);

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
            this.channelParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.counterComboBox = new System.Windows.Forms.ComboBox();
            this.zIndexPhaseLabel = new System.Windows.Forms.Label();
            this.zIndexPhaseComboBox = new System.Windows.Forms.ComboBox();
            this.decodingTypeLabel = new System.Windows.Forms.Label();
            this.pulsesPerRevLabel = new System.Windows.Forms.Label();
            this.zIndexValueLabel = new System.Windows.Forms.Label();
            this.physicalChannelLabel = new System.Windows.Forms.Label();
            this.pulsePerRevTextBox = new System.Windows.Forms.TextBox();
            this.zIndexValueTextBox = new System.Windows.Forms.TextBox();
            this.decodingTypeComboBox = new System.Windows.Forms.ComboBox();
            this.zIndexEnabledCheckBox = new System.Windows.Forms.CheckBox();
            this.dataGroupBox = new System.Windows.Forms.GroupBox();
            this.dataListBox = new System.Windows.Forms.ListBox();
            this.timingParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.sampleClkSourceLabel = new System.Windows.Forms.Label();
            this.sampleClkSourceTextBox = new System.Windows.Forms.TextBox();
            this.samplesToReadTextBox = new System.Windows.Forms.TextBox();
            this.samplesToReadLabel = new System.Windows.Forms.Label();
            this.rateLabel = new System.Windows.Forms.Label();
            this.rateTextBox = new System.Windows.Forms.TextBox();
            this.stopButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.channelParametersGroupBox.SuspendLayout();
            this.dataGroupBox.SuspendLayout();
            this.timingParametersGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // channelParametersGroupBox
            // 
            this.channelParametersGroupBox.Controls.Add(this.counterComboBox);
            this.channelParametersGroupBox.Controls.Add(this.zIndexPhaseLabel);
            this.channelParametersGroupBox.Controls.Add(this.zIndexPhaseComboBox);
            this.channelParametersGroupBox.Controls.Add(this.decodingTypeLabel);
            this.channelParametersGroupBox.Controls.Add(this.pulsesPerRevLabel);
            this.channelParametersGroupBox.Controls.Add(this.zIndexValueLabel);
            this.channelParametersGroupBox.Controls.Add(this.physicalChannelLabel);
            this.channelParametersGroupBox.Controls.Add(this.pulsePerRevTextBox);
            this.channelParametersGroupBox.Controls.Add(this.zIndexValueTextBox);
            this.channelParametersGroupBox.Controls.Add(this.decodingTypeComboBox);
            this.channelParametersGroupBox.Controls.Add(this.zIndexEnabledCheckBox);
            this.channelParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.channelParametersGroupBox.Location = new System.Drawing.Point(8, 8);
            this.channelParametersGroupBox.Name = "channelParametersGroupBox";
            this.channelParametersGroupBox.Size = new System.Drawing.Size(272, 224);
            this.channelParametersGroupBox.TabIndex = 2;
            this.channelParametersGroupBox.TabStop = false;
            this.channelParametersGroupBox.Text = "Channel Parameters";
            // 
            // counterComboBox
            // 
            this.counterComboBox.Location = new System.Drawing.Point(136, 24);
            this.counterComboBox.Name = "counterComboBox";
            this.counterComboBox.Size = new System.Drawing.Size(121, 21);
            this.counterComboBox.TabIndex = 1;
            this.counterComboBox.Text = "Dev1/ctr0";
            // 
            // zIndexPhaseLabel
            // 
            this.zIndexPhaseLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.zIndexPhaseLabel.Location = new System.Drawing.Point(12, 160);
            this.zIndexPhaseLabel.Name = "zIndexPhaseLabel";
            this.zIndexPhaseLabel.Size = new System.Drawing.Size(92, 16);
            this.zIndexPhaseLabel.TabIndex = 7;
            this.zIndexPhaseLabel.Text = "Z Index Phase:";
            // 
            // zIndexPhaseComboBox
            // 
            this.zIndexPhaseComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.zIndexPhaseComboBox.Items.AddRange(new object[] {
                                                                     "A High B High",
                                                                     "A High B Low",
                                                                     "A Low B High",
                                                                     "A Low B Low"});
            this.zIndexPhaseComboBox.Location = new System.Drawing.Point(136, 160);
            this.zIndexPhaseComboBox.Name = "zIndexPhaseComboBox";
            this.zIndexPhaseComboBox.Size = new System.Drawing.Size(120, 21);
            this.zIndexPhaseComboBox.TabIndex = 8;
            // 
            // decodingTypeLabel
            // 
            this.decodingTypeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.decodingTypeLabel.Location = new System.Drawing.Point(12, 96);
            this.decodingTypeLabel.Name = "decodingTypeLabel";
            this.decodingTypeLabel.Size = new System.Drawing.Size(112, 16);
            this.decodingTypeLabel.TabIndex = 3;
            this.decodingTypeLabel.Text = "Decoding Type:";
            // 
            // pulsesPerRevLabel
            // 
            this.pulsesPerRevLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.pulsesPerRevLabel.Location = new System.Drawing.Point(12, 192);
            this.pulsesPerRevLabel.Name = "pulsesPerRevLabel";
            this.pulsesPerRevLabel.Size = new System.Drawing.Size(120, 16);
            this.pulsesPerRevLabel.TabIndex = 9;
            this.pulsesPerRevLabel.Text = "Pulses per Revolution:";
            // 
            // zIndexValueLabel
            // 
            this.zIndexValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.zIndexValueLabel.Location = new System.Drawing.Point(12, 128);
            this.zIndexValueLabel.Name = "zIndexValueLabel";
            this.zIndexValueLabel.Size = new System.Drawing.Size(120, 18);
            this.zIndexValueLabel.TabIndex = 5;
            this.zIndexValueLabel.Text = "Z Index Value:";
            // 
            // physicalChannelLabel
            // 
            this.physicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.physicalChannelLabel.Location = new System.Drawing.Point(12, 26);
            this.physicalChannelLabel.Name = "physicalChannelLabel";
            this.physicalChannelLabel.Size = new System.Drawing.Size(96, 16);
            this.physicalChannelLabel.TabIndex = 0;
            this.physicalChannelLabel.Text = "Counter(s):";
            // 
            // pulsePerRevTextBox
            // 
            this.pulsePerRevTextBox.Location = new System.Drawing.Point(136, 192);
            this.pulsePerRevTextBox.Name = "pulsePerRevTextBox";
            this.pulsePerRevTextBox.Size = new System.Drawing.Size(120, 20);
            this.pulsePerRevTextBox.TabIndex = 10;
            this.pulsePerRevTextBox.Text = "24";
            // 
            // zIndexValueTextBox
            // 
            this.zIndexValueTextBox.Location = new System.Drawing.Point(136, 128);
            this.zIndexValueTextBox.Name = "zIndexValueTextBox";
            this.zIndexValueTextBox.Size = new System.Drawing.Size(120, 20);
            this.zIndexValueTextBox.TabIndex = 6;
            this.zIndexValueTextBox.Text = "0";
            // 
            // decodingTypeComboBox
            // 
            this.decodingTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.decodingTypeComboBox.Items.AddRange(new object[] {
                                                                      "X1",
                                                                      "X2",
                                                                      "X4"});
            this.decodingTypeComboBox.Location = new System.Drawing.Point(136, 96);
            this.decodingTypeComboBox.Name = "decodingTypeComboBox";
            this.decodingTypeComboBox.Size = new System.Drawing.Size(120, 21);
            this.decodingTypeComboBox.TabIndex = 4;
            // 
            // zIndexEnabledCheckBox
            // 
            this.zIndexEnabledCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.zIndexEnabledCheckBox.Location = new System.Drawing.Point(136, 56);
            this.zIndexEnabledCheckBox.Name = "zIndexEnabledCheckBox";
            this.zIndexEnabledCheckBox.Size = new System.Drawing.Size(120, 24);
            this.zIndexEnabledCheckBox.TabIndex = 2;
            this.zIndexEnabledCheckBox.Text = "Z Index Enabled";
            // 
            // dataGroupBox
            // 
            this.dataGroupBox.Controls.Add(this.dataListBox);
            this.dataGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.dataGroupBox.Location = new System.Drawing.Point(288, 8);
            this.dataGroupBox.Name = "dataGroupBox";
            this.dataGroupBox.Size = new System.Drawing.Size(144, 224);
            this.dataGroupBox.TabIndex = 4;
            this.dataGroupBox.TabStop = false;
            this.dataGroupBox.Text = "Data";
            // 
            // dataListBox
            // 
            this.dataListBox.Location = new System.Drawing.Point(8, 16);
            this.dataListBox.Name = "dataListBox";
            this.dataListBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.dataListBox.Size = new System.Drawing.Size(128, 199);
            this.dataListBox.TabIndex = 0;
            // 
            // timingParametersGroupBox
            // 
            this.timingParametersGroupBox.Controls.Add(this.sampleClkSourceLabel);
            this.timingParametersGroupBox.Controls.Add(this.sampleClkSourceTextBox);
            this.timingParametersGroupBox.Controls.Add(this.samplesToReadTextBox);
            this.timingParametersGroupBox.Controls.Add(this.samplesToReadLabel);
            this.timingParametersGroupBox.Controls.Add(this.rateLabel);
            this.timingParametersGroupBox.Controls.Add(this.rateTextBox);
            this.timingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.timingParametersGroupBox.Location = new System.Drawing.Point(8, 240);
            this.timingParametersGroupBox.Name = "timingParametersGroupBox";
            this.timingParametersGroupBox.Size = new System.Drawing.Size(272, 112);
            this.timingParametersGroupBox.TabIndex = 3;
            this.timingParametersGroupBox.TabStop = false;
            this.timingParametersGroupBox.Text = "Timing Parameters";
            // 
            // sampleClkSourceLabel
            // 
            this.sampleClkSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.sampleClkSourceLabel.Location = new System.Drawing.Point(12, 80);
            this.sampleClkSourceLabel.Name = "sampleClkSourceLabel";
            this.sampleClkSourceLabel.Size = new System.Drawing.Size(120, 16);
            this.sampleClkSourceLabel.TabIndex = 4;
            this.sampleClkSourceLabel.Text = "Sample Clock Source:";
            // 
            // sampleClkSourceTextBox
            // 
            this.sampleClkSourceTextBox.Location = new System.Drawing.Point(136, 80);
            this.sampleClkSourceTextBox.Name = "sampleClkSourceTextBox";
            this.sampleClkSourceTextBox.Size = new System.Drawing.Size(120, 20);
            this.sampleClkSourceTextBox.TabIndex = 5;
            this.sampleClkSourceTextBox.Text = "/Dev1/PFI9";
            // 
            // samplesToReadTextBox
            // 
            this.samplesToReadTextBox.Location = new System.Drawing.Point(136, 48);
            this.samplesToReadTextBox.Name = "samplesToReadTextBox";
            this.samplesToReadTextBox.Size = new System.Drawing.Size(120, 20);
            this.samplesToReadTextBox.TabIndex = 3;
            this.samplesToReadTextBox.Text = "1000";
            // 
            // samplesToReadLabel
            // 
            this.samplesToReadLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.samplesToReadLabel.Location = new System.Drawing.Point(12, 48);
            this.samplesToReadLabel.Name = "samplesToReadLabel";
            this.samplesToReadLabel.Size = new System.Drawing.Size(98, 16);
            this.samplesToReadLabel.TabIndex = 2;
            this.samplesToReadLabel.Text = "Samples to Read:";
            // 
            // rateLabel
            // 
            this.rateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rateLabel.Location = new System.Drawing.Point(12, 24);
            this.rateLabel.Name = "rateLabel";
            this.rateLabel.Size = new System.Drawing.Size(56, 16);
            this.rateLabel.TabIndex = 0;
            this.rateLabel.Text = "Rate:";
            // 
            // rateTextBox
            // 
            this.rateTextBox.Location = new System.Drawing.Point(136, 16);
            this.rateTextBox.Name = "rateTextBox";
            this.rateTextBox.Size = new System.Drawing.Size(120, 20);
            this.rateTextBox.TabIndex = 1;
            this.rateTextBox.Text = "1000.00";
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.stopButton.Location = new System.Drawing.Point(312, 296);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(100, 32);
            this.stopButton.TabIndex = 1;
            this.stopButton.Text = "Stop";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // startButton
            // 
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.startButton.Location = new System.Drawing.Point(312, 264);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(100, 32);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(442, 360);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.timingParametersGroupBox);
            this.Controls.Add(this.dataGroupBox);
            this.Controls.Add(this.channelParametersGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Measure Angular Position Buffered Continuous - External Clock";
            this.channelParametersGroupBox.ResumeLayout(false);
            this.dataGroupBox.ResumeLayout(false);
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

                double zIndexValue = Convert.ToDouble(zIndexValueTextBox.Text);
                int pulsePerRev = Convert.ToInt32(pulsePerRevTextBox.Text);
                numberOfSamples = Convert.ToInt32(samplesToReadTextBox.Text);
                zIndexEnable = zIndexEnabledCheckBox.Checked;

                switch (decodingTypeComboBox.SelectedIndex)
                {
                    case 0: //X1
                        encoderType = CIEncoderDecodingType.X1;
                        break;
                    case 1: //X2
                        encoderType = CIEncoderDecodingType.X2;
                        break;
                    case 2: //X4
                        encoderType = CIEncoderDecodingType.X4;
                        break;
                }

                switch (decodingTypeComboBox.SelectedIndex)
                {
                    case 0: //A High B High
                        encoderPhase = CIEncoderZIndexPhase.AHighBHigh;
                        break;
                    case 1: //A High B Low
                        encoderPhase = CIEncoderZIndexPhase.AHighBLow;
                        break;
                    case 2: //A Low B High
                        encoderPhase = CIEncoderZIndexPhase.ALowBHigh;
                        break;
                    case 3: //A Low B Low
                        encoderPhase = CIEncoderZIndexPhase.ALowBLow;
                        break;
                }

                myTask.CIChannels.CreateAngularEncoderChannel(counterComboBox.Text,
                    "", encoderType, zIndexEnable, zIndexValue, encoderPhase, pulsePerRev,
                    0.0, CIAngularEncoderUnits.Degrees);

                myTask.Timing.ConfigureSampleClock(sampleClkSourceTextBox.Text, Convert.ToDouble(rateTextBox.Text),
                    SampleClockActiveEdge.Rising, SampleQuantityMode.ContinuousSamples, 1000);

                runningTask = myTask;

                counterInReader = new CounterReader(myTask.Stream);

                // Use SynchronizeCallbacks to specify that the object 
                // marshals callbacks across threads appropriately.
                counterInReader.SynchronizeCallbacks = true;
                // Memory Optimized Read method needs an initialized array.
                data = new Double[numberOfSamples];
                counterInReader.BeginMemoryOptimizedReadMultiSampleDouble(numberOfSamples, asyncCB, myTask, data);

                startButton.Enabled = false;
                stopButton.Enabled = true;

            }
            catch (DaqException exception)
            {
                myTask.Dispose();
                runningTask = null;
                MessageBox.Show(exception.Message);
            }

        }

        private void stopButton_Click(object sender, System.EventArgs e)
        {
            runningTask = null;
            myTask.Dispose();
            startButton.Enabled = true;
            stopButton.Enabled = false;
        }

        private void CounterInCallback(IAsyncResult ar)
        {
            try
            {
                if (runningTask != null && runningTask == ar.AsyncState)
                {
                    int samplesToDisplay;

                    data = counterInReader.EndMemoryOptimizedReadMultiSampleDouble(ar, out actualNumberOfSamplesRead);

                    //Display only the first 10 data points in the listbox. 
                    if (data.Length < 10)
                        samplesToDisplay = data.Length;
                    else
                        samplesToDisplay = 10;

                    dataListBox.BeginUpdate();
                    dataListBox.Items.Clear();

                    for (int i = 0; i < samplesToDisplay; i++)
                    {
                        dataListBox.Items.Add(data[i]);
                    }
                    dataListBox.EndUpdate();
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
