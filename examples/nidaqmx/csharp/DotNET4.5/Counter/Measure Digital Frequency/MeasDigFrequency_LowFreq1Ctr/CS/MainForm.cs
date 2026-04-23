/******************************************************************************
*
* Example program:
*   MeasDigFrequency_LowFreq1Ctr
*
* Category:
*   CI
*
* Description:
*   This example demonstrates how to measure a frequency using one counter on a
*   counter input channel.  The starting edge, minimum value and maximum value
*   are all configurable. This example shows how to measure frequency on the
*   counter's default input terminal (see I/O Connections Overview below for
*   more information), but could easily be expanded to measure frequency on any
*   PFI, RTSI, or internal signal.  Additionally, this example could be extended
*   to measure frequency with two counters for different frequency and
*   quantization error requirements.
*
* Instructions for running:
*   1.  Enter the physical channel which corresponds to the counter you want to
*       use to measure the frequency on the DAQ device.
*   2.  Enter the measurement edge to specify the edge on which you want the
*       counter to start measuring.  Enter the maximum and minimum value to
*       specify the expected range of your unknown frequency. Note:  It is
*       important to set the maximum and minimum values of your unknown
*       frequency as accurately as possible so the best internal timebase can be
*       chosen to minimize measurement error.  The default values specify a
*       range that can be measured by the counter using the 20MHzTimebase. Use
*       the GenDigPulseTrain_Continuous example to verify that you are measuring
*       correctly on the DAQ device.
*
* Steps:
*   1.  Create a Task.
*   2.  Create a CIChannel by using the CreateFrequencyChannel method to measure
*       frequency.  The edge parameter is used to determine if the counter  will
*       begin measuring on a rising or falling edge.  It is important to set the
*       maximum and minimum values of your unknown signal as accurately as
*       possible so the best internal timebase can be chosen to minimize
*       measurement error.  The default values specify the range that can be
*       measured by the counter using the 20MHzTimebase.
*   3.  Create a CounterReader object and use the ReadSingleSampleDouble method
*       to read the data. The timeout is set by default to 10 seconds.
*   4.  Handle any DaqExceptions and display any error messages in a Message
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

namespace NationalInstruments.Examples.MeasDigFrequency_LowFreq1Ctr
{
    /// <summary>
    /// Summary description for MainForm.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        internal System.Windows.Forms.Button measureFrequencyButton;
        internal System.Windows.Forms.GroupBox channelParamGroupBox;
        internal System.Windows.Forms.Label physicalChannelLabel;
        internal System.Windows.Forms.TextBox minValueTextBox;
        internal System.Windows.Forms.Label minimumValueLabel;
        internal System.Windows.Forms.Label maximumValueLabel;
        internal System.Windows.Forms.TextBox maxValueTextBox;
        internal System.Windows.Forms.Label measuredFrequencyLabel;
        internal System.Windows.Forms.TextBox measuredFrequencyTextBox;
        private System.Windows.Forms.GroupBox startEdgeGroupBox;
        private System.Windows.Forms.RadioButton risingRadioButton;
        private CIFrequencyStartingEdge edges=CIFrequencyStartingEdge.Rising;
        private System.Windows.Forms.RadioButton fallingRadioButton;
        private double measureFrequency=0;
        private Task myTask;
        private CounterReader myCounterReader;
        private AsyncCallback myCallBack;
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
            this.measureFrequencyButton = new System.Windows.Forms.Button();
            this.measuredFrequencyLabel = new System.Windows.Forms.Label();
            this.measuredFrequencyTextBox = new System.Windows.Forms.TextBox();
            this.channelParamGroupBox = new System.Windows.Forms.GroupBox();
            this.counterComboBox = new System.Windows.Forms.ComboBox();
            this.startEdgeGroupBox = new System.Windows.Forms.GroupBox();
            this.fallingRadioButton = new System.Windows.Forms.RadioButton();
            this.risingRadioButton = new System.Windows.Forms.RadioButton();
            this.physicalChannelLabel = new System.Windows.Forms.Label();
            this.minValueTextBox = new System.Windows.Forms.TextBox();
            this.minimumValueLabel = new System.Windows.Forms.Label();
            this.maximumValueLabel = new System.Windows.Forms.Label();
            this.maxValueTextBox = new System.Windows.Forms.TextBox();
            this.channelParamGroupBox.SuspendLayout();
            this.startEdgeGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // measureFrequencyButton
            // 
            this.measureFrequencyButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.measureFrequencyButton.Location = new System.Drawing.Point(192, 192);
            this.measureFrequencyButton.Name = "measureFrequencyButton";
            this.measureFrequencyButton.Size = new System.Drawing.Size(120, 40);
            this.measureFrequencyButton.TabIndex = 0;
            this.measureFrequencyButton.Text = "Measure Frequency";
            this.measureFrequencyButton.Click += new System.EventHandler(this.measureFrequencyButton_Click);
            // 
            // measuredFrequencyLabel
            // 
            this.measuredFrequencyLabel.BackColor = System.Drawing.SystemColors.Control;
            this.measuredFrequencyLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.measuredFrequencyLabel.Location = new System.Drawing.Point(184, 16);
            this.measuredFrequencyLabel.Name = "measuredFrequencyLabel";
            this.measuredFrequencyLabel.Size = new System.Drawing.Size(144, 16);
            this.measuredFrequencyLabel.TabIndex = 2;
            this.measuredFrequencyLabel.Text = "Measured Frequency (Hz):";
            // 
            // measuredFrequencyTextBox
            // 
            this.measuredFrequencyTextBox.Location = new System.Drawing.Point(184, 32);
            this.measuredFrequencyTextBox.Name = "measuredFrequencyTextBox";
            this.measuredFrequencyTextBox.ReadOnly = true;
            this.measuredFrequencyTextBox.Size = new System.Drawing.Size(136, 20);
            this.measuredFrequencyTextBox.TabIndex = 3;
            this.measuredFrequencyTextBox.Text = "0.0";
            // 
            // channelParamGroupBox
            // 
            this.channelParamGroupBox.Controls.Add(this.counterComboBox);
            this.channelParamGroupBox.Controls.Add(this.startEdgeGroupBox);
            this.channelParamGroupBox.Controls.Add(this.physicalChannelLabel);
            this.channelParamGroupBox.Controls.Add(this.minValueTextBox);
            this.channelParamGroupBox.Controls.Add(this.minimumValueLabel);
            this.channelParamGroupBox.Controls.Add(this.maximumValueLabel);
            this.channelParamGroupBox.Controls.Add(this.maxValueTextBox);
            this.channelParamGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.channelParamGroupBox.Location = new System.Drawing.Point(8, 8);
            this.channelParamGroupBox.Name = "channelParamGroupBox";
            this.channelParamGroupBox.Size = new System.Drawing.Size(160, 256);
            this.channelParamGroupBox.TabIndex = 1;
            this.channelParamGroupBox.TabStop = false;
            this.channelParamGroupBox.Text = "Channel Parameters";
            // 
            // counterComboBox
            // 
            this.counterComboBox.Location = new System.Drawing.Point(16, 40);
            this.counterComboBox.Name = "counterComboBox";
            this.counterComboBox.Size = new System.Drawing.Size(128, 21);
            this.counterComboBox.TabIndex = 1;
            this.counterComboBox.Text = "Dev1/ctr0";
            // 
            // startEdgeGroupBox
            // 
            this.startEdgeGroupBox.Controls.Add(this.fallingRadioButton);
            this.startEdgeGroupBox.Controls.Add(this.risingRadioButton);
            this.startEdgeGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.startEdgeGroupBox.Location = new System.Drawing.Point(16, 168);
            this.startEdgeGroupBox.Name = "startEdgeGroupBox";
            this.startEdgeGroupBox.Size = new System.Drawing.Size(128, 72);
            this.startEdgeGroupBox.TabIndex = 6;
            this.startEdgeGroupBox.TabStop = false;
            this.startEdgeGroupBox.Text = "Edge:";
            // 
            // fallingRadioButton
            // 
            this.fallingRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.fallingRadioButton.Location = new System.Drawing.Point(16, 40);
            this.fallingRadioButton.Name = "fallingRadioButton";
            this.fallingRadioButton.Size = new System.Drawing.Size(77, 24);
            this.fallingRadioButton.TabIndex = 1;
            this.fallingRadioButton.Text = "Falling";
            this.fallingRadioButton.CheckedChanged += new System.EventHandler(this.fallingRadioButton_CheckedChanged);
            // 
            // risingRadioButton
            // 
            this.risingRadioButton.Checked = true;
            this.risingRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.risingRadioButton.Location = new System.Drawing.Point(16, 19);
            this.risingRadioButton.Name = "risingRadioButton";
            this.risingRadioButton.Size = new System.Drawing.Size(77, 24);
            this.risingRadioButton.TabIndex = 0;
            this.risingRadioButton.TabStop = true;
            this.risingRadioButton.Text = "Rising";
            this.risingRadioButton.CheckedChanged += new System.EventHandler(this.risingRadioButton_CheckedChanged);
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
            this.minValueTextBox.Location = new System.Drawing.Point(16, 88);
            this.minValueTextBox.Name = "minValueTextBox";
            this.minValueTextBox.Size = new System.Drawing.Size(128, 20);
            this.minValueTextBox.TabIndex = 3;
            this.minValueTextBox.Text = "1.192093";
            // 
            // minimumValueLabel
            // 
            this.minimumValueLabel.BackColor = System.Drawing.SystemColors.Control;
            this.minimumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.minimumValueLabel.Location = new System.Drawing.Point(16, 72);
            this.minimumValueLabel.Name = "minimumValueLabel";
            this.minimumValueLabel.Size = new System.Drawing.Size(120, 16);
            this.minimumValueLabel.TabIndex = 2;
            this.minimumValueLabel.Text = "Minimum value (Hz):";
            // 
            // maximumValueLabel
            // 
            this.maximumValueLabel.BackColor = System.Drawing.SystemColors.Control;
            this.maximumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.maximumValueLabel.Location = new System.Drawing.Point(16, 120);
            this.maximumValueLabel.Name = "maximumValueLabel";
            this.maximumValueLabel.Size = new System.Drawing.Size(120, 16);
            this.maximumValueLabel.TabIndex = 4;
            this.maximumValueLabel.Text = "Maximum Value (Hz):";
            // 
            // maxValueTextBox
            // 
            this.maxValueTextBox.Location = new System.Drawing.Point(16, 136);
            this.maxValueTextBox.Name = "maxValueTextBox";
            this.maxValueTextBox.Size = new System.Drawing.Size(128, 20);
            this.maxValueTextBox.TabIndex = 5;
            this.maxValueTextBox.Text = "10000000.0";
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(338, 272);
            this.Controls.Add(this.measureFrequencyButton);
            this.Controls.Add(this.measuredFrequencyLabel);
            this.Controls.Add(this.measuredFrequencyTextBox);
            this.Controls.Add(this.channelParamGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Meas Dig Frequency-Low Freq 1 Ctr";
            this.channelParamGroupBox.ResumeLayout(false);
            this.startEdgeGroupBox.ResumeLayout(false);
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

        private void measureFrequencyButton_Click(object sender, System.EventArgs e)
        {
            // This example uses the default source (or gate) terminal for 
            // the counter of your device.  To determine what the default 
            // counter pins for your device are or to set a different source 
            // (or gate) pin, refer to the Connecting Counter Signals topic
            // in the NI-DAQmx Help (search for "Connecting Counter Signals").

            measureFrequencyButton.Enabled = false;

            try
            {               
                myTask = new Task();
                
                myTask.CIChannels.CreateFrequencyChannel(counterComboBox.Text,
                    "Measure Dig Freq Low Frequency", 
                    Convert.ToDouble(minValueTextBox.Text), 
                    Convert.ToDouble(maxValueTextBox.Text), edges, 
                    CIFrequencyMeasurementMethod.LowFrequencyOneCounter, 0.001, 4,
                    CIFrequencyUnits.Hertz);
                
                myCounterReader = new CounterReader(myTask.Stream);

                // Use SynchronizeCallbacks to specify that the object 
                // marshals callbacks across threads appropriately.
                myCounterReader.SynchronizeCallbacks = true;

                myCallBack = new AsyncCallback(CounterInCallback);
                myCounterReader.BeginReadSingleSampleDouble(myCallBack,null);
            }

            catch(Exception exception)
            {
                MessageBox.Show(exception.Message);
                myTask.Dispose();
                measureFrequencyButton.Enabled = true;
            }
        }

        private void risingRadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            edges = CIFrequencyStartingEdge.Rising;
        }

        private void fallingRadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            edges = CIFrequencyStartingEdge.Falling;
        }

        private void CounterInCallback(IAsyncResult ar)
        {
            // Read the measured value
            try
            {                                
                measureFrequency = myCounterReader.EndReadSingleSampleDouble(ar);
                measuredFrequencyTextBox.Text = measureFrequency.ToString();
            }
            catch (DaqException exception)
            {
                MessageBox.Show(exception.Message);
            }
            finally
            {
                myTask.Dispose();
                measureFrequencyButton.Enabled = true;
            }
        }
    }
}
