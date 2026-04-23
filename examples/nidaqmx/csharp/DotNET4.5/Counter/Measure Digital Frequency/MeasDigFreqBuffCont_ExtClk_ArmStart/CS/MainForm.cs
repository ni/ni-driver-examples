/******************************************************************************
*
* Example program:
*   MeasDigFreqBuffCont_ExtClk_ArmStart
*
* Category:
*   CI
*
* Description:
*   This example demonstrates how to continually measure the frequency on a
*   Counter Input Channel 
*   with a sample clock and arm start trigger. This example shows how to measure
*   frequency with a counter on any PFI, RTSI, or internal signal.
*
* Instructions for running:
*   1.  Enter the physical channel which corresponds to the counter you want to
*       use to measure frequency on the DAQ device.
*   2.  Enter the Maximum and Minimum value to specify the range or your unknown
*       pulse frequency.
*   3.  Select the frequency input channel.
*
* Steps:
*   1.  Create a Task.
*   2.  Create a counter input channel using the
*       Task.CIChannel.CreateCIFreqChannel method. The edge 
*       parameter is used to determine if the counter will begin measuring on a
*       rising or falling edge. The divisor 
*       specifies how many periods of the unknow signal are used to calculate the
*       frequency. The higher this 
*       is, the more accurate your measurement will be, it will also take longer
*       to perform each measurement.  
*       It is important to set the maximum and minimum values of your unknown
*       frequency as accurately as possible 
*       so the best internal timebase can be chosen to minimize measurement error.
*       The default values specify 
*       a range that can be measured by the counter using the 20MHzTimebase.
*   3.  Configure the Sample Clock of the Task. Note: If the sample clock
*       frequency is not half or less of the frequency measured, you may overrun
*       the buffer.
*   4.  Enable the Arm Start Trigger and set the source to be the sample clock.
*   5.  Create a CounterReader object, and set its SynchronizingObject property
*       to  the current instance of the form class.  Register an asynchronous
*       callback for the counter operation using
*       CounterReader.BeginReadMultiSampleDouble.
*   6.  The async callback is called everytime the required number of samples
*       has been acquired. Calling CounterReader.EndReadMultiSampleDouble
*       returns the acquired data. Calling the BeginReadMultiSampleDouble method
*       again makes this operation continuously return data.
*   7.  Call the Task.Dispose method to stop the task and de-allocate any
*       resources acquired by the task
*   8.  Handle any DaqExceptions and display any error messages in a Message
*       box.
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

namespace NationalInstruments.Examples.MeasDigFreqBuffContExtClkArmStart

{
    /// <summary>
    /// Summary description for MainForm.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        internal System.Windows.Forms.GroupBox channelParamGroupBox;
        internal System.Windows.Forms.Label physicalChannelLabel;
        internal System.Windows.Forms.TextBox minValueTextBox;
        internal System.Windows.Forms.Label minimumValueLabel;
        internal System.Windows.Forms.Label maximumValueLabel;
        internal System.Windows.Forms.TextBox maxValueTextBox;
        internal System.Windows.Forms.Label measureFrequencyLabel;
        private System.Windows.Forms.ListBox frequencyListBox;
        internal System.Windows.Forms.Button startButton;
        internal System.Windows.Forms.Button stopButton;
        private Task myTask;
        private Task runningTask;
        private int samplesPerRead;
        private double[] measuredData;
        private int actualNumberOfSamplesRead = 0;
        private CounterReader myReader;
        private AsyncCallback myCallBack;
        private System.Windows.Forms.GroupBox timingParametersGroupBox;
        private System.Windows.Forms.TextBox samplesPerReadTextBox;
        private System.Windows.Forms.Label samplesPerReadLabel;
        private System.Windows.Forms.GroupBox dataGroupBox;
        private System.Windows.Forms.ComboBox counterComboBox;
        private NumericUpDown rateNumeric;
        private Label rateLabel;
        private Label clockSourceLabel;
        private TextBox clockSourceTextBox;
        private Label frequencyInputTerminalLabel;
        private TextBox frequencyInputTerminalTextBox;

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
            this.startButton = new System.Windows.Forms.Button();
            this.measureFrequencyLabel = new System.Windows.Forms.Label();
            this.channelParamGroupBox = new System.Windows.Forms.GroupBox();
            this.frequencyInputTerminalLabel = new System.Windows.Forms.Label();
            this.frequencyInputTerminalTextBox = new System.Windows.Forms.TextBox();
            this.counterComboBox = new System.Windows.Forms.ComboBox();
            this.physicalChannelLabel = new System.Windows.Forms.Label();
            this.minValueTextBox = new System.Windows.Forms.TextBox();
            this.minimumValueLabel = new System.Windows.Forms.Label();
            this.maximumValueLabel = new System.Windows.Forms.Label();
            this.maxValueTextBox = new System.Windows.Forms.TextBox();
            this.frequencyListBox = new System.Windows.Forms.ListBox();
            this.stopButton = new System.Windows.Forms.Button();
            this.timingParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.rateNumeric = new System.Windows.Forms.NumericUpDown();
            this.rateLabel = new System.Windows.Forms.Label();
            this.clockSourceLabel = new System.Windows.Forms.Label();
            this.clockSourceTextBox = new System.Windows.Forms.TextBox();
            this.samplesPerReadTextBox = new System.Windows.Forms.TextBox();
            this.samplesPerReadLabel = new System.Windows.Forms.Label();
            this.dataGroupBox = new System.Windows.Forms.GroupBox();
            this.channelParamGroupBox.SuspendLayout();
            this.timingParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rateNumeric)).BeginInit();
            this.dataGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // startButton
            // 
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.startButton.Location = new System.Drawing.Point(232, 282);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(128, 40);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // measureFrequencyLabel
            // 
            this.measureFrequencyLabel.BackColor = System.Drawing.SystemColors.Control;
            this.measureFrequencyLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.measureFrequencyLabel.Location = new System.Drawing.Point(16, 24);
            this.measureFrequencyLabel.Name = "measureFrequencyLabel";
            this.measureFrequencyLabel.Size = new System.Drawing.Size(144, 16);
            this.measureFrequencyLabel.TabIndex = 0;
            this.measureFrequencyLabel.Text = "Measured Frequency (Hz):";
            // 
            // channelParamGroupBox
            // 
            this.channelParamGroupBox.Controls.Add(this.frequencyInputTerminalLabel);
            this.channelParamGroupBox.Controls.Add(this.frequencyInputTerminalTextBox);
            this.channelParamGroupBox.Controls.Add(this.counterComboBox);
            this.channelParamGroupBox.Controls.Add(this.physicalChannelLabel);
            this.channelParamGroupBox.Controls.Add(this.minValueTextBox);
            this.channelParamGroupBox.Controls.Add(this.minimumValueLabel);
            this.channelParamGroupBox.Controls.Add(this.maximumValueLabel);
            this.channelParamGroupBox.Controls.Add(this.maxValueTextBox);
            this.channelParamGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.channelParamGroupBox.Location = new System.Drawing.Point(8, 8);
            this.channelParamGroupBox.Name = "channelParamGroupBox";
            this.channelParamGroupBox.Size = new System.Drawing.Size(184, 202);
            this.channelParamGroupBox.TabIndex = 2;
            this.channelParamGroupBox.TabStop = false;
            this.channelParamGroupBox.Text = "Channel Parameters:";
            // 
            // frequencyInputTerminalLabel
            // 
            this.frequencyInputTerminalLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.frequencyInputTerminalLabel.Location = new System.Drawing.Point(16, 64);
            this.frequencyInputTerminalLabel.Name = "frequencyInputTerminalLabel";
            this.frequencyInputTerminalLabel.Size = new System.Drawing.Size(140, 16);
            this.frequencyInputTerminalLabel.TabIndex = 7;
            this.frequencyInputTerminalLabel.Text = "Frequency Input Terminal:";
            // 
            // frequencyInputTerminalTextBox
            // 
            this.frequencyInputTerminalTextBox.Location = new System.Drawing.Point(16, 83);
            this.frequencyInputTerminalTextBox.Name = "frequencyInputTerminalTextBox";
            this.frequencyInputTerminalTextBox.Size = new System.Drawing.Size(152, 20);
            this.frequencyInputTerminalTextBox.TabIndex = 8;
            this.frequencyInputTerminalTextBox.Text = "/Dev1/PFI0";
            // 
            // counterComboBox
            // 
            this.counterComboBox.Location = new System.Drawing.Point(16, 40);
            this.counterComboBox.Name = "counterComboBox";
            this.counterComboBox.Size = new System.Drawing.Size(152, 21);
            this.counterComboBox.TabIndex = 1;
            this.counterComboBox.Text = "Dev1/ctr0";
            // 
            // physicalChannelLabel
            // 
            this.physicalChannelLabel.BackColor = System.Drawing.SystemColors.Control;
            this.physicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.physicalChannelLabel.Location = new System.Drawing.Point(16, 24);
            this.physicalChannelLabel.Name = "physicalChannelLabel";
            this.physicalChannelLabel.Size = new System.Drawing.Size(100, 16);
            this.physicalChannelLabel.TabIndex = 0;
            this.physicalChannelLabel.Text = "Counter(s):";
            // 
            // minValueTextBox
            // 
            this.minValueTextBox.Location = new System.Drawing.Point(16, 125);
            this.minValueTextBox.Name = "minValueTextBox";
            this.minValueTextBox.Size = new System.Drawing.Size(152, 20);
            this.minValueTextBox.TabIndex = 4;
            this.minValueTextBox.Text = "100000.000000";
            // 
            // minimumValueLabel
            // 
            this.minimumValueLabel.BackColor = System.Drawing.SystemColors.Control;
            this.minimumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.minimumValueLabel.Location = new System.Drawing.Point(16, 106);
            this.minimumValueLabel.Name = "minimumValueLabel";
            this.minimumValueLabel.Size = new System.Drawing.Size(112, 16);
            this.minimumValueLabel.TabIndex = 3;
            this.minimumValueLabel.Text = "Minimum value (Hz):";
            // 
            // maximumValueLabel
            // 
            this.maximumValueLabel.BackColor = System.Drawing.SystemColors.Control;
            this.maximumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.maximumValueLabel.Location = new System.Drawing.Point(16, 148);
            this.maximumValueLabel.Name = "maximumValueLabel";
            this.maximumValueLabel.Size = new System.Drawing.Size(120, 16);
            this.maximumValueLabel.TabIndex = 5;
            this.maximumValueLabel.Text = "Maximum Value (Hz):";
            // 
            // maxValueTextBox
            // 
            this.maxValueTextBox.Location = new System.Drawing.Point(16, 167);
            this.maxValueTextBox.Name = "maxValueTextBox";
            this.maxValueTextBox.Size = new System.Drawing.Size(152, 20);
            this.maxValueTextBox.TabIndex = 6;
            this.maxValueTextBox.Text = "1000000.000000";
            // 
            // frequencyListBox
            // 
            this.frequencyListBox.Location = new System.Drawing.Point(16, 48);
            this.frequencyListBox.Name = "frequencyListBox";
            this.frequencyListBox.Size = new System.Drawing.Size(152, 186);
            this.frequencyListBox.TabIndex = 1;
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.stopButton.Location = new System.Drawing.Point(232, 328);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(128, 40);
            this.stopButton.TabIndex = 1;
            this.stopButton.Text = "Stop";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // timingParametersGroupBox
            // 
            this.timingParametersGroupBox.Controls.Add(this.rateNumeric);
            this.timingParametersGroupBox.Controls.Add(this.rateLabel);
            this.timingParametersGroupBox.Controls.Add(this.clockSourceLabel);
            this.timingParametersGroupBox.Controls.Add(this.clockSourceTextBox);
            this.timingParametersGroupBox.Controls.Add(this.samplesPerReadTextBox);
            this.timingParametersGroupBox.Controls.Add(this.samplesPerReadLabel);
            this.timingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.timingParametersGroupBox.Location = new System.Drawing.Point(8, 216);
            this.timingParametersGroupBox.Name = "timingParametersGroupBox";
            this.timingParametersGroupBox.Size = new System.Drawing.Size(184, 158);
            this.timingParametersGroupBox.TabIndex = 3;
            this.timingParametersGroupBox.TabStop = false;
            this.timingParametersGroupBox.Text = "Timing Parameters:";
            // 
            // rateNumeric
            // 
            this.rateNumeric.DecimalPlaces = 2;
            this.rateNumeric.Location = new System.Drawing.Point(16, 40);
            this.rateNumeric.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.rateNumeric.Name = "rateNumeric";
            this.rateNumeric.Size = new System.Drawing.Size(152, 20);
            this.rateNumeric.TabIndex = 7;
            this.rateNumeric.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            // 
            // rateLabel
            // 
            this.rateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rateLabel.Location = new System.Drawing.Point(16, 21);
            this.rateLabel.Name = "rateLabel";
            this.rateLabel.Size = new System.Drawing.Size(112, 16);
            this.rateLabel.TabIndex = 6;
            this.rateLabel.Text = "Rate (Hz):";
            // 
            // clockSourceLabel
            // 
            this.clockSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.clockSourceLabel.Location = new System.Drawing.Point(16, 107);
            this.clockSourceLabel.Name = "clockSourceLabel";
            this.clockSourceLabel.Size = new System.Drawing.Size(120, 16);
            this.clockSourceLabel.TabIndex = 4;
            this.clockSourceLabel.Text = "Sample Clock Source:";
            // 
            // clockSourceTextBox
            // 
            this.clockSourceTextBox.Location = new System.Drawing.Point(16, 126);
            this.clockSourceTextBox.Name = "clockSourceTextBox";
            this.clockSourceTextBox.Size = new System.Drawing.Size(152, 20);
            this.clockSourceTextBox.TabIndex = 5;
            this.clockSourceTextBox.Text = "/Dev1/PFI1";
            // 
            // samplesPerReadTextBox
            // 
            this.samplesPerReadTextBox.Location = new System.Drawing.Point(16, 82);
            this.samplesPerReadTextBox.Name = "samplesPerReadTextBox";
            this.samplesPerReadTextBox.Size = new System.Drawing.Size(152, 20);
            this.samplesPerReadTextBox.TabIndex = 1;
            this.samplesPerReadTextBox.Text = "10";
            // 
            // samplesPerReadLabel
            // 
            this.samplesPerReadLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.samplesPerReadLabel.Location = new System.Drawing.Point(16, 66);
            this.samplesPerReadLabel.Name = "samplesPerReadLabel";
            this.samplesPerReadLabel.Size = new System.Drawing.Size(112, 23);
            this.samplesPerReadLabel.TabIndex = 0;
            this.samplesPerReadLabel.Text = "Samples per Read:";
            // 
            // dataGroupBox
            // 
            this.dataGroupBox.Controls.Add(this.measureFrequencyLabel);
            this.dataGroupBox.Controls.Add(this.frequencyListBox);
            this.dataGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.dataGroupBox.Location = new System.Drawing.Point(200, 8);
            this.dataGroupBox.Name = "dataGroupBox";
            this.dataGroupBox.Size = new System.Drawing.Size(184, 248);
            this.dataGroupBox.TabIndex = 4;
            this.dataGroupBox.TabStop = false;
            this.dataGroupBox.Text = "Data:";
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(394, 386);
            this.Controls.Add(this.dataGroupBox);
            this.Controls.Add(this.timingParametersGroupBox);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.channelParamGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dig Freq-Buffered-Cont-Ext Clk-Arm Start";
            this.channelParamGroupBox.ResumeLayout(false);
            this.channelParamGroupBox.PerformLayout();
            this.timingParametersGroupBox.ResumeLayout(false);
            this.timingParametersGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rateNumeric)).EndInit();
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
            
                myTask.CIChannels.CreateFrequencyChannel(counterComboBox.Text, "Digital Frequency",
                    Convert.ToDouble(minValueTextBox.Text),
                    Convert.ToDouble(maxValueTextBox.Text), CIFrequencyStartingEdge.Rising, 
                    CIFrequencyMeasurementMethod.LowFrequencyOneCounter, 0.001,
                    1, CIFrequencyUnits.Hertz);


                myTask.CIChannels["Digital Frequency"].FrequencyTerminal = frequencyInputTerminalTextBox.Text;
                myTask.Timing.ConfigureSampleClock(clockSourceTextBox.Text, Convert.ToDouble(rateNumeric.Value),
                    SampleClockActiveEdge.Rising, SampleQuantityMode.ContinuousSamples, 1000);
                myTask.Triggers.ArmStartTrigger.Type = ArmStartTriggerType.DigitalEdge;
                myTask.Triggers.ArmStartTrigger.DigitalEdge.Source = clockSourceTextBox.Text;
                myTask.Triggers.ArmStartTrigger.DigitalEdge.Edge = DigitalEdgeArmStartTriggerEdge.Rising;
                

                runningTask = myTask;
                myReader = new CounterReader(myTask.Stream);

                // Use SynchronizeCallbacks to specify that the object 
                // marshals callbacks across threads appropriately.
                myReader.SynchronizeCallbacks = true;

                myCallBack = new AsyncCallback(CounterInCallback);
                samplesPerRead = Convert.ToInt32(samplesPerReadTextBox.Text);

                // Memory Optimized Read method needs an initialized array.
                measuredData = new double[samplesPerRead];
                myReader.BeginMemoryOptimizedReadMultiSampleDouble(samplesPerRead, myCallBack, myTask, measuredData);

                startButton.Enabled = false;
                stopButton.Enabled = true;
            }
            catch (Exception exception)
            {
                myTask.Dispose();
                MessageBox.Show(exception.Message);
                runningTask = null;
                startButton.Enabled = true;
                stopButton.Enabled = false;
            }
        }

        private void stopButton_Click(object sender, System.EventArgs e)
        {
            runningTask = null;
            stopButton.Enabled = false;
            myTask.Dispose();
            startButton.Enabled = true;
        }

        private  void CounterInCallback (IAsyncResult ar)
        {
            //Reads the returned data and updates the form control
            try
            {
                if (runningTask != null && runningTask == ar.AsyncState)
                {
                    measuredData = myReader.EndMemoryOptimizedReadMultiSampleDouble(ar, out actualNumberOfSamplesRead);
                    
                    //Display only the first 10 data points acquired
                    int samplesToDisplay = 10;
                    if (measuredData.Length < 10)
                        samplesToDisplay = measuredData.Length;

                    frequencyListBox.BeginUpdate();
                    frequencyListBox.Items.Clear();

                    for(int i = 0; i < samplesToDisplay; i++)
                    {
                        frequencyListBox.Items.Add(measuredData[i]);
                    }
                    
                    frequencyListBox.EndUpdate();

                    myReader.BeginMemoryOptimizedReadMultiSampleDouble(samplesPerRead, myCallBack, myTask, measuredData);
                }
            }
            catch (DaqException exception)
            {
                runningTask = null;
                myTask.Dispose();
                stopButton.Enabled = false;
                MessageBox.Show(exception.Message);
                startButton.Enabled = true;
            }
        }
    }
}
