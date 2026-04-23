/******************************************************************************
*
* Example program:
*   MeasBuffered_SemiPeriodFinite
*
* Category:
*   CI
*
* Description:
*   This example demonstrates how to measure semi-periods on a counter input
*   channel. The 
*   minimum value, maximum value, sample mode, and samples per channel are all
*   configurable.This example 
*   shows how to measure semi-period on the counter's default input terminal (see
*   I/O Conections Overview 
*   below for more information), but can easily be expanded to measure semi-period
*   on any PFI, RTSI, or internal 
*   signal by setting the properties on the CIChannel object.Semi-period
*   measurement differs from pulse width 
*   measurement in that it measures both the high and the low pulses of a given
*   signal.  So for every period, 
*   two data points will be returned.
*
* Instructions for running:
*   1.  Enter the physical channel which corresponds to the counter you want to
*       use to measure semi-periods on the DAQ device.
*   2.  Enter the maximum and minimum value to specify a range for your unknown
*       semi-periods.Note:  It is important to set the maximum and minimum
*       values of your unknown semi-period as accurately as possible so the best
*       internal timebase can be chosen to minimize measurement error.  The
*       default values specify the range that can be measured by the counter
*       using the 20MHzTimebase.  Use the GenDigPulseTrain_Continuous example to
*       verify that you are measuring correctly on the DAQ device.
*
* Steps:
*   1.  Create a counter input channel using
*       Task.CIChannel.CreateCISemiPeriodChannel. It is important to set the
*       maximum and minimum values of your unknown period as accurately as
*       possible so the best internal timebase can be chosen to minimize
*       measurement error.  The default values specify a range that can be
*       measured by the counter using the 20MHzTimebase.
*   2.  Call the ConfigureImplicit method to configure the sample mode and
*       samples per channel.  Note: For time measurements with counters, the
*       ConfigureImplicit method is used because the signal being measured
*       itself determines the sample rate.  This is unlike buffered event
*       counting, where an external sample clock must be used.
*   3.  Call the CounterReader.BeginReadMultiSampleDouble method to arm the
*       counter and begin measuring.
*   4.  Call the Task.Dispose method to stop the task and de-allocate any
*       resources used by the Task.
*   5.  Handle any DaqExceptions and display a message box if there are errors.
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
*   measurement being taken.  In this example the two edge separation will be
*   measured on the default input terminals on ctr0. For more information on the
*   default counter input and output terminals for your device, open the
*   NI-DAQmx Help, and refer to Counter Signal Connections found under the
*   Device Considerations book in the table of contents.
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

namespace NationalInstruments.Examples.MeasBuffered_SemiPeriodFinite
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
        private System.Windows.Forms.Label measuredPeriodLabel;
        private System.Windows.Forms.Button startButton;
        private System.ComponentModel.IContainer components=null;
        private System.Windows.Forms.ListBox periodListBox;
        private Task myTask;
        private double[] measureValue;
        private System.Windows.Forms.GroupBox timingParamGroupBox;
        internal System.Windows.Forms.TextBox samplesPerChannelTextBox;
        private CounterReader myCounterReader;
        internal System.Windows.Forms.Label samplesPerChannelLabel;
        private System.Windows.Forms.GroupBox dataGroupBox;
        private System.Windows.Forms.ComboBox counterComboBox;
        private AsyncCallback myCallBack;

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
            this.physicalChannelLabel = new System.Windows.Forms.Label();
            this.minValueTextBox = new System.Windows.Forms.TextBox();
            this.minimumValueLabel = new System.Windows.Forms.Label();
            this.maximumValueLabel = new System.Windows.Forms.Label();
            this.maxValueTextBox = new System.Windows.Forms.TextBox();
            this.measuredPeriodLabel = new System.Windows.Forms.Label();
            this.startButton = new System.Windows.Forms.Button();
            this.periodListBox = new System.Windows.Forms.ListBox();
            this.timingParamGroupBox = new System.Windows.Forms.GroupBox();
            this.samplesPerChannelLabel = new System.Windows.Forms.Label();
            this.samplesPerChannelTextBox = new System.Windows.Forms.TextBox();
            this.dataGroupBox = new System.Windows.Forms.GroupBox();
            this.channelParamGroupBox.SuspendLayout();
            this.timingParamGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // channelParamGroupBox
            // 
            this.channelParamGroupBox.Controls.Add(this.counterComboBox);
            this.channelParamGroupBox.Controls.Add(this.physicalChannelLabel);
            this.channelParamGroupBox.Controls.Add(this.minValueTextBox);
            this.channelParamGroupBox.Controls.Add(this.minimumValueLabel);
            this.channelParamGroupBox.Controls.Add(this.maximumValueLabel);
            this.channelParamGroupBox.Controls.Add(this.maxValueTextBox);
            this.channelParamGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.channelParamGroupBox.Location = new System.Drawing.Point(8, 8);
            this.channelParamGroupBox.Name = "channelParamGroupBox";
            this.channelParamGroupBox.Size = new System.Drawing.Size(144, 192);
            this.channelParamGroupBox.TabIndex = 1;
            this.channelParamGroupBox.TabStop = false;
            this.channelParamGroupBox.Text = "Channel Parameters:";
            // 
            // counterComboBox
            // 
            this.counterComboBox.Location = new System.Drawing.Point(16, 40);
            this.counterComboBox.Name = "counterComboBox";
            this.counterComboBox.Size = new System.Drawing.Size(112, 21);
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
            this.minValueTextBox.Location = new System.Drawing.Point(16, 96);
            this.minValueTextBox.Name = "minValueTextBox";
            this.minValueTextBox.Size = new System.Drawing.Size(112, 20);
            this.minValueTextBox.TabIndex = 3;
            this.minValueTextBox.Text = "0.00000010";
            // 
            // minimumValueLabel
            // 
            this.minimumValueLabel.BackColor = System.Drawing.SystemColors.Control;
            this.minimumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.minimumValueLabel.Location = new System.Drawing.Point(16, 80);
            this.minimumValueLabel.Name = "minimumValueLabel";
            this.minimumValueLabel.Size = new System.Drawing.Size(120, 16);
            this.minimumValueLabel.TabIndex = 2;
            this.minimumValueLabel.Text = "Minimum value (sec):";
            // 
            // maximumValueLabel
            // 
            this.maximumValueLabel.BackColor = System.Drawing.SystemColors.Control;
            this.maximumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.maximumValueLabel.Location = new System.Drawing.Point(16, 136);
            this.maximumValueLabel.Name = "maximumValueLabel";
            this.maximumValueLabel.Size = new System.Drawing.Size(120, 16);
            this.maximumValueLabel.TabIndex = 4;
            this.maximumValueLabel.Text = "Maximum Value (sec):";
            // 
            // maxValueTextBox
            // 
            this.maxValueTextBox.Location = new System.Drawing.Point(16, 152);
            this.maxValueTextBox.Name = "maxValueTextBox";
            this.maxValueTextBox.Size = new System.Drawing.Size(112, 20);
            this.maxValueTextBox.TabIndex = 5;
            this.maxValueTextBox.Text = "0.8388";
            // 
            // measuredPeriodLabel
            // 
            this.measuredPeriodLabel.BackColor = System.Drawing.SystemColors.Control;
            this.measuredPeriodLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.measuredPeriodLabel.Location = new System.Drawing.Point(176, 32);
            this.measuredPeriodLabel.Name = "measuredPeriodLabel";
            this.measuredPeriodLabel.Size = new System.Drawing.Size(128, 16);
            this.measuredPeriodLabel.TabIndex = 4;
            this.measuredPeriodLabel.Text = "Measured Periods (sec):";
            // 
            // startButton
            // 
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.startButton.Location = new System.Drawing.Point(184, 232);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(112, 40);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Measure Periods";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // periodListBox
            // 
            this.periodListBox.Location = new System.Drawing.Point(176, 48);
            this.periodListBox.Name = "periodListBox";
            this.periodListBox.Size = new System.Drawing.Size(120, 134);
            this.periodListBox.TabIndex = 5;
            // 
            // timingParamGroupBox
            // 
            this.timingParamGroupBox.Controls.Add(this.samplesPerChannelLabel);
            this.timingParamGroupBox.Controls.Add(this.samplesPerChannelTextBox);
            this.timingParamGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.timingParamGroupBox.Location = new System.Drawing.Point(8, 208);
            this.timingParamGroupBox.Name = "timingParamGroupBox";
            this.timingParamGroupBox.Size = new System.Drawing.Size(144, 80);
            this.timingParamGroupBox.TabIndex = 2;
            this.timingParamGroupBox.TabStop = false;
            this.timingParamGroupBox.Text = "Timing Parameters:";
            // 
            // samplesPerChannelLabel
            // 
            this.samplesPerChannelLabel.BackColor = System.Drawing.SystemColors.Control;
            this.samplesPerChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.samplesPerChannelLabel.Location = new System.Drawing.Point(16, 24);
            this.samplesPerChannelLabel.Name = "samplesPerChannelLabel";
            this.samplesPerChannelLabel.Size = new System.Drawing.Size(120, 16);
            this.samplesPerChannelLabel.TabIndex = 0;
            this.samplesPerChannelLabel.Text = "Samples Per Channel:";
            // 
            // samplesPerChannelTextBox
            // 
            this.samplesPerChannelTextBox.Location = new System.Drawing.Point(16, 40);
            this.samplesPerChannelTextBox.Name = "samplesPerChannelTextBox";
            this.samplesPerChannelTextBox.Size = new System.Drawing.Size(112, 20);
            this.samplesPerChannelTextBox.TabIndex = 1;
            this.samplesPerChannelTextBox.Text = "1000";
            // 
            // dataGroupBox
            // 
            this.dataGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.dataGroupBox.Location = new System.Drawing.Point(160, 8);
            this.dataGroupBox.Name = "dataGroupBox";
            this.dataGroupBox.Size = new System.Drawing.Size(152, 192);
            this.dataGroupBox.TabIndex = 3;
            this.dataGroupBox.TabStop = false;
            this.dataGroupBox.Text = "Data:";
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(322, 295);
            this.Controls.Add(this.timingParamGroupBox);
            this.Controls.Add(this.periodListBox);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.measuredPeriodLabel);
            this.Controls.Add(this.channelParamGroupBox);
            this.Controls.Add(this.dataGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Measure Buffered Semi-Period-Finite";
            this.channelParamGroupBox.ResumeLayout(false);
            this.timingParamGroupBox.ResumeLayout(false);
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
                
                myTask.CIChannels.CreateSemiPeriodChannel(counterComboBox.Text,
                    "SemiPeriodMeasure", Convert.ToDouble(minValueTextBox.Text),
                    Convert.ToDouble(maxValueTextBox.Text), CISemiPeriodUnits.Seconds);

                myTask.Timing.ConfigureImplicit(SampleQuantityMode.FiniteSamples,
                    Convert.ToInt32(samplesPerChannelTextBox.Text));
                
                myCounterReader = new CounterReader(myTask.Stream);

                // Use SynchronizeCallbacks to specify that the object 
                // marshals callbacks across threads appropriately.
                myCounterReader.SynchronizeCallbacks = true;

                myCallBack = new AsyncCallback(CounterInCallback);
                myCounterReader.BeginReadMultiSampleDouble(-1, myCallBack, null);
               
                startButton.Enabled = false;
            }
            catch(Exception exception)
            {
                myTask.Dispose();
                MessageBox.Show(exception.Message);
                startButton.Enabled = true;
            }
        }
        
        private void CounterInCallback (IAsyncResult ar )
        {
            //Reads the returned data and updates the form control
            try
            {   
                measureValue = myCounterReader.EndReadMultiSampleDouble(ar);
                
                //Display only the first 10 data points acquired
                int samplesToDisplay = 10;
                if (measureValue.Length < 10)
                    samplesToDisplay = measureValue.Length;
               
                periodListBox.BeginUpdate();
                periodListBox.Items.Clear();

                for(int i = 0; i < samplesToDisplay; i++)
                {
                    periodListBox.Items.Add(measureValue[i]);
                }

                periodListBox.EndUpdate();
            }
            catch (DaqException exception)
            {
                MessageBox.Show(exception.Message);
            }
            finally
            {
                myTask.Dispose();
                startButton.Enabled = true;  
            }
        }
    }
}
