/******************************************************************************
*
* Example program:
*   MeasPulseWidth
*
* Category:
*   CI
*
* Description:
*   This example demonstrates how to measure pulse width on a counter input
*   channel. The edge, minimum value and maximum value are all configurable.This
*   example shows how to measure pulse width on the counter's default input
*   terminal (see I/O Connections Overview below for more information), but
*   could easily be expanded to measure pulse width on any PFI, RTSI, or
*   internal signal.
*
* Instructions for running:
*   1.  Enter the physical channel which corresponds to the counter you want to
*       use to measure pulse width on the DAQ device.
*   2.  Enter the measurement edge to specify what type of pulse you want the
*       counter to measure.  Enter the maximum and minimum value to specify the
*       range of your unknown pulse width.Note:  It is important to set the
*       maximum and minimum values of your unknown pulse as accurately as
*       possible so the best internal timebase can be chosen to minimize
*       measurement error.  The default values specify the range that can be
*       measured by the counter using the 20MHzTimebase.  Use the GenDigPulse
*       example to verify that you are measuring correctly on the DAQ device.
*
* Steps:
*   1.  Create a Task.
*   2.  Create a CIChannel object using CreatePulseWidthChannel.  The edge
*       parameter is used to determine if the counter will measure high or low
*       pulses.  It is important to set the maximum and minimum values of your
*       unknown pulse as accurately as possible so the best internal timebase
*       can be chosen to minimize measurement error.  The default values specify
*       a range that can be measured by the counter using the 20MHzTimebase.
*   3.  Create a CounterReader object by passing it a stream to the task
*       created. Then use the ReadSingleSampleDouble method to return the next
*       pulse width measurement.The timeout is set to 10 seconds by default so
*       an error is returned if a pulse is not returned in the specified time
*       limit.
*   4.  Handle any DaqExceptions and display a message for errors
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

namespace NationalInstruments.Examples.MeasPulseWidth
{
    /// <summary>
    /// Summary description for MainForm.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Label physicalChannelLabel;
        private System.Windows.Forms.Label minimumValueLabel;
        private System.Windows.Forms.TextBox minValueTextBox;
        private System.Windows.Forms.Label maximumValueLabel;
        private System.Windows.Forms.TextBox maxValueTextBox;
        private System.Windows.Forms.GroupBox channelParamGroupBox;
        private System.Windows.Forms.TextBox measuredWidthTextBox;
        private System.Windows.Forms.Label measurePulseWidthLabel;
        private System.Windows.Forms.Button measureWidthButton;
        internal System.Windows.Forms.GroupBox startEdgeGroupBox;
        internal System.Windows.Forms.RadioButton fallingRadioButton;
        internal System.Windows.Forms.RadioButton risingRadioButton;
        private CIPulseWidthStartingEdge edge = CIPulseWidthStartingEdge.Rising;

        private NationalInstruments.DAQmx.Task myTask;
        private NationalInstruments.DAQmx.CounterReader myCounterReader;
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
            this.physicalChannelLabel = new System.Windows.Forms.Label();
            this.minimumValueLabel = new System.Windows.Forms.Label();
            this.minValueTextBox = new System.Windows.Forms.TextBox();
            this.maximumValueLabel = new System.Windows.Forms.Label();
            this.maxValueTextBox = new System.Windows.Forms.TextBox();
            this.channelParamGroupBox = new System.Windows.Forms.GroupBox();
            this.counterComboBox = new System.Windows.Forms.ComboBox();
            this.startEdgeGroupBox = new System.Windows.Forms.GroupBox();
            this.fallingRadioButton = new System.Windows.Forms.RadioButton();
            this.risingRadioButton = new System.Windows.Forms.RadioButton();
            this.measuredWidthTextBox = new System.Windows.Forms.TextBox();
            this.measurePulseWidthLabel = new System.Windows.Forms.Label();
            this.measureWidthButton = new System.Windows.Forms.Button();
            this.channelParamGroupBox.SuspendLayout();
            this.startEdgeGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // physicalChannelLabel
            // 
            this.physicalChannelLabel.BackColor = System.Drawing.SystemColors.Control;
            this.physicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.physicalChannelLabel.Location = new System.Drawing.Point(16, 24);
            this.physicalChannelLabel.Name = "physicalChannelLabel";
            this.physicalChannelLabel.Size = new System.Drawing.Size(100, 16);
            this.physicalChannelLabel.TabIndex = 0;
            this.physicalChannelLabel.Text = "Physical Channel:";
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
            // minValueTextBox
            // 
            this.minValueTextBox.Location = new System.Drawing.Point(16, 104);
            this.minValueTextBox.Name = "minValueTextBox";
            this.minValueTextBox.Size = new System.Drawing.Size(104, 20);
            this.minValueTextBox.TabIndex = 3;
            this.minValueTextBox.Text = "0.000000100";
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
            this.maxValueTextBox.Location = new System.Drawing.Point(16, 160);
            this.maxValueTextBox.Name = "maxValueTextBox";
            this.maxValueTextBox.Size = new System.Drawing.Size(104, 20);
            this.maxValueTextBox.TabIndex = 5;
            this.maxValueTextBox.Text = "0.830000000";
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
            this.channelParamGroupBox.Location = new System.Drawing.Point(16, 16);
            this.channelParamGroupBox.Name = "channelParamGroupBox";
            this.channelParamGroupBox.Size = new System.Drawing.Size(136, 296);
            this.channelParamGroupBox.TabIndex = 1;
            this.channelParamGroupBox.TabStop = false;
            this.channelParamGroupBox.Text = "Channel Parameters:";
            // 
            // counterComboBox
            // 
            this.counterComboBox.Location = new System.Drawing.Point(16, 48);
            this.counterComboBox.Name = "counterComboBox";
            this.counterComboBox.Size = new System.Drawing.Size(104, 21);
            this.counterComboBox.TabIndex = 1;
            this.counterComboBox.Text = "Dev1/ctr0";
            // 
            // startEdgeGroupBox
            // 
            this.startEdgeGroupBox.Controls.Add(this.fallingRadioButton);
            this.startEdgeGroupBox.Controls.Add(this.risingRadioButton);
            this.startEdgeGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.startEdgeGroupBox.Location = new System.Drawing.Point(16, 200);
            this.startEdgeGroupBox.Name = "startEdgeGroupBox";
            this.startEdgeGroupBox.Size = new System.Drawing.Size(104, 80);
            this.startEdgeGroupBox.TabIndex = 6;
            this.startEdgeGroupBox.TabStop = false;
            this.startEdgeGroupBox.Text = "Starting Edge:";
            // 
            // fallingRadioButton
            // 
            this.fallingRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.fallingRadioButton.Location = new System.Drawing.Point(16, 45);
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
            this.risingRadioButton.Location = new System.Drawing.Point(16, 24);
            this.risingRadioButton.Name = "risingRadioButton";
            this.risingRadioButton.Size = new System.Drawing.Size(80, 24);
            this.risingRadioButton.TabIndex = 0;
            this.risingRadioButton.TabStop = true;
            this.risingRadioButton.Text = "Rising";
            this.risingRadioButton.CheckedChanged += new System.EventHandler(this.risingRadioButton_CheckedChanged);
            // 
            // measuredWidthTextBox
            // 
            this.measuredWidthTextBox.Location = new System.Drawing.Point(168, 40);
            this.measuredWidthTextBox.Name = "measuredWidthTextBox";
            this.measuredWidthTextBox.ReadOnly = true;
            this.measuredWidthTextBox.Size = new System.Drawing.Size(144, 20);
            this.measuredWidthTextBox.TabIndex = 3;
            this.measuredWidthTextBox.Text = "0";
            // 
            // measurePulseWidthLabel
            // 
            this.measurePulseWidthLabel.BackColor = System.Drawing.SystemColors.Control;
            this.measurePulseWidthLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.measurePulseWidthLabel.Location = new System.Drawing.Point(168, 24);
            this.measurePulseWidthLabel.Name = "measurePulseWidthLabel";
            this.measurePulseWidthLabel.Size = new System.Drawing.Size(152, 16);
            this.measurePulseWidthLabel.TabIndex = 2;
            this.measurePulseWidthLabel.Text = "Measured Pulse Width (sec):";
            // 
            // measureWidthButton
            // 
            this.measureWidthButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.measureWidthButton.Location = new System.Drawing.Point(176, 248);
            this.measureWidthButton.Name = "measureWidthButton";
            this.measureWidthButton.Size = new System.Drawing.Size(128, 40);
            this.measureWidthButton.TabIndex = 0;
            this.measureWidthButton.Text = "Measure Pulse Width";
            this.measureWidthButton.Click += new System.EventHandler(this.measureWidthButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(328, 325);
            this.Controls.Add(this.measureWidthButton);
            this.Controls.Add(this.measurePulseWidthLabel);
            this.Controls.Add(this.measuredWidthTextBox);
            this.Controls.Add(this.channelParamGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Measure Pulse Width";
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

       
        private void measureWidthButton_Click(object sender, System.EventArgs e)
        {
            // This example uses the default source (or gate) terminal for 
            // the counter of your device.  To determine what the default 
            // counter pins for your device are or to set a different source 
            // (or gate) pin, refer to the Connecting Counter Signals topic
            // in the NI-DAQmx Help (search for "Connecting Counter Signals").

            try
            {  
                measureWidthButton.Enabled = false;

                myTask = new Task();

                myTask.CIChannels.CreatePulseWidthChannel(counterComboBox.Text, 
                    "Meas Pulse Width", Convert.ToDouble(minValueTextBox.Text), 
                    Convert.ToDouble(maxValueTextBox.Text), edge, 
                    CIPulseWidthUnits.Seconds);

                myCounterReader = new CounterReader(myTask.Stream);

                // Use SynchronizeCallbacks to specify that the object 
                // marshals callbacks across threads appropriately.
                myCounterReader.SynchronizeCallbacks = true;

                AsyncCallback myCallback = new AsyncCallback(MeasurementCallback);
                myCounterReader.BeginReadSingleSampleDouble(myCallback, null);
                
            }
            catch(Exception exception)
            {
                myTask.Dispose();
                MessageBox.Show(exception.Message);
                measureWidthButton.Enabled = true;
            }
        }

        private void risingRadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            edge = CIPulseWidthStartingEdge.Rising;
        }

        private void fallingRadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            edge = CIPulseWidthStartingEdge.Falling;
        }

        private void MeasurementCallback(IAsyncResult ar)
        {
            try
            {                                
                double pulseWidth = myCounterReader.EndReadSingleSampleDouble(ar);
                measuredWidthTextBox.Text = pulseWidth.ToString();
            }
            catch (DaqException exception)
            {
                MessageBox.Show(exception.Message);
            }
            finally
            {
                myTask.Dispose();
                measureWidthButton.Enabled = true;
            }
        }
    }
}
