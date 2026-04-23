/******************************************************************************
*
* Example program:
*   MeasureGpsTimestamp
*
* Category:
*   CI
*
* Description:
*   This example demonstrates how to use a GPS counter to update the current
*   time.
*
* Instructions for running:
*   1.  Select the GPS counter.
*   2.  Select the GPS synchronization method.
*   3.  Select the GPS synchronization source.
*   4.  If applicable, set the initial time.Note:  The initial time is only
*       applicable when the synchronization method is PPS or None.
*
* Steps:
*   1.  Create a new counter input task.
*   2.  Create a GPS Timestamp channel on the task.
*   3.  Set the initial time if applicable.
*   4.  Create a CounterReader and associate it with the task by using the
*       task's stream.
*   5.  Start the task and the timer.
*   6.  Inside the timer callback, read the count value and update the GPS time.
*   7.  Dispose the Task object to clean-up any resources associated with the
*       task.
*   8.  Handle any DaqExceptions, if they occur.
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

namespace NationalInstruments.Examples.MeasureGpsTimestamp
{
    /// <summary>
    /// Summary description for MainForm.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        private Task myTask;
        private CounterReader myCounterReader;
        private double reading;
        
        internal System.Windows.Forms.Button stopButton;
        internal System.Windows.Forms.Button startButton;
        internal System.Windows.Forms.GroupBox channelParameterGroupBox;
        internal System.Windows.Forms.Label counterLabel;
        internal System.Windows.Forms.Timer loopTimer;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.ComboBox counterComboBox;
        internal System.Windows.Forms.GroupBox timeGroupBox;
        private System.Windows.Forms.DateTimePicker gpsDateTimePicker;
        internal System.Windows.Forms.Label syncMethodLabel;
        internal System.Windows.Forms.ComboBox syncMethodComboBox;
        internal System.Windows.Forms.ComboBox syncSourceComboBox;
        internal System.Windows.Forms.Label syncSourceLabel;
        internal System.Windows.Forms.GroupBox startingGroupBox;
        private System.Windows.Forms.DateTimePicker startingDateTimePicker;

        public MainForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            // Initialize UI
            counterComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.CI, PhysicalChannelAccess.External));
            if (counterComboBox.Items.Count > 0)
                counterComboBox.SelectedIndex = 0;

            syncSourceComboBox.Items.AddRange(DaqSystem.Local.GetTerminals(TerminalTypes.All));
            if (syncSourceComboBox.Items.Count > 0)
                syncSourceComboBox.SelectedIndex = 0;

            syncMethodComboBox.SelectedIndex = 0;

            startingDateTimePicker.Enabled = false;
            startingDateTimePicker.Value = new DateTime(DateTime.Today.Year, 1, 1);
            
            gpsDateTimePicker.Value = new DateTime(DateTime.Today.Year, 1, 1);
            gpsDateTimePicker.MinDate = gpsDateTimePicker.Value;
            gpsDateTimePicker.MaxDate = gpsDateTimePicker.Value + TimeSpan.FromMilliseconds(1);
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
            this.timeGroupBox = new System.Windows.Forms.GroupBox();
            this.gpsDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.stopButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.channelParameterGroupBox = new System.Windows.Forms.GroupBox();
            this.counterComboBox = new System.Windows.Forms.ComboBox();
            this.syncMethodLabel = new System.Windows.Forms.Label();
            this.counterLabel = new System.Windows.Forms.Label();
            this.syncMethodComboBox = new System.Windows.Forms.ComboBox();
            this.syncSourceComboBox = new System.Windows.Forms.ComboBox();
            this.syncSourceLabel = new System.Windows.Forms.Label();
            this.loopTimer = new System.Windows.Forms.Timer(this.components);
            this.startingGroupBox = new System.Windows.Forms.GroupBox();
            this.startingDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.timeGroupBox.SuspendLayout();
            this.channelParameterGroupBox.SuspendLayout();
            this.startingGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // timeGroupBox
            // 
            this.timeGroupBox.Controls.Add(this.gpsDateTimePicker);
            this.timeGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.timeGroupBox.Location = new System.Drawing.Point(8, 208);
            this.timeGroupBox.Name = "timeGroupBox";
            this.timeGroupBox.Size = new System.Drawing.Size(288, 64);
            this.timeGroupBox.TabIndex = 2;
            this.timeGroupBox.TabStop = false;
            this.timeGroupBox.Text = "GPS Time";
            // 
            // gpsDateTimePicker
            // 
            this.gpsDateTimePicker.CustomFormat = "MMMM dd, yyyy hh:mm:ss tt";
            this.gpsDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.gpsDateTimePicker.Location = new System.Drawing.Point(8, 24);
            this.gpsDateTimePicker.Name = "gpsDateTimePicker";
            this.gpsDateTimePicker.ShowUpDown = true;
            this.gpsDateTimePicker.Size = new System.Drawing.Size(272, 20);
            this.gpsDateTimePicker.TabIndex = 0;
            this.gpsDateTimePicker.Value = new System.DateTime(2005, 1, 1, 0, 0, 0, 0);
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.stopButton.Location = new System.Drawing.Point(165, 280);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(72, 24);
            this.stopButton.TabIndex = 4;
            this.stopButton.Text = "Stop";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // startButton
            // 
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.startButton.Location = new System.Drawing.Point(69, 280);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(72, 24);
            this.startButton.TabIndex = 3;
            this.startButton.Text = "Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // channelParameterGroupBox
            // 
            this.channelParameterGroupBox.Controls.Add(this.counterComboBox);
            this.channelParameterGroupBox.Controls.Add(this.syncMethodLabel);
            this.channelParameterGroupBox.Controls.Add(this.counterLabel);
            this.channelParameterGroupBox.Controls.Add(this.syncMethodComboBox);
            this.channelParameterGroupBox.Controls.Add(this.syncSourceComboBox);
            this.channelParameterGroupBox.Controls.Add(this.syncSourceLabel);
            this.channelParameterGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.channelParameterGroupBox.Location = new System.Drawing.Point(8, 9);
            this.channelParameterGroupBox.Name = "channelParameterGroupBox";
            this.channelParameterGroupBox.Size = new System.Drawing.Size(288, 119);
            this.channelParameterGroupBox.TabIndex = 0;
            this.channelParameterGroupBox.TabStop = false;
            this.channelParameterGroupBox.Text = "Channel Parameters";
            // 
            // counterComboBox
            // 
            this.counterComboBox.Location = new System.Drawing.Point(136, 24);
            this.counterComboBox.Name = "counterComboBox";
            this.counterComboBox.Size = new System.Drawing.Size(144, 21);
            this.counterComboBox.TabIndex = 1;
            this.counterComboBox.Text = "Dev1/gpsTimestampCtr0";
            // 
            // syncMethodLabel
            // 
            this.syncMethodLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.syncMethodLabel.Location = new System.Drawing.Point(8, 58);
            this.syncMethodLabel.Name = "syncMethodLabel";
            this.syncMethodLabel.Size = new System.Drawing.Size(120, 16);
            this.syncMethodLabel.TabIndex = 2;
            this.syncMethodLabel.Text = "Synchronization Method:";
            // 
            // counterLabel
            // 
            this.counterLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.counterLabel.Location = new System.Drawing.Point(8, 26);
            this.counterLabel.Name = "counterLabel";
            this.counterLabel.Size = new System.Drawing.Size(72, 16);
            this.counterLabel.TabIndex = 0;
            this.counterLabel.Text = "GPS Counter:";
            // 
            // syncMethodComboBox
            // 
            this.syncMethodComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.syncMethodComboBox.Items.AddRange(new object[] {
                                                                    "IRIG-B",
                                                                    "PPS",
                                                                    "None"});
            this.syncMethodComboBox.Location = new System.Drawing.Point(136, 56);
            this.syncMethodComboBox.Name = "syncMethodComboBox";
            this.syncMethodComboBox.Size = new System.Drawing.Size(144, 21);
            this.syncMethodComboBox.TabIndex = 3;
            this.syncMethodComboBox.SelectedIndexChanged += new System.EventHandler(this.syncMethodComboBox_SelectedIndexChanged);
            // 
            // syncSourceComboBox
            // 
            this.syncSourceComboBox.Location = new System.Drawing.Point(136, 88);
            this.syncSourceComboBox.Name = "syncSourceComboBox";
            this.syncSourceComboBox.Size = new System.Drawing.Size(144, 21);
            this.syncSourceComboBox.TabIndex = 5;
            this.syncSourceComboBox.Text = "/Dev1/PFI7";
            // 
            // syncSourceLabel
            // 
            this.syncSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.syncSourceLabel.Location = new System.Drawing.Point(8, 90);
            this.syncSourceLabel.Name = "syncSourceLabel";
            this.syncSourceLabel.Size = new System.Drawing.Size(120, 16);
            this.syncSourceLabel.TabIndex = 4;
            this.syncSourceLabel.Text = "Synchronization Source:";
            // 
            // loopTimer
            // 
            this.loopTimer.Tick += new System.EventHandler(this.loopTimer_Tick);
            // 
            // startingGroupBox
            // 
            this.startingGroupBox.Controls.Add(this.startingDateTimePicker);
            this.startingGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.startingGroupBox.Location = new System.Drawing.Point(8, 136);
            this.startingGroupBox.Name = "startingGroupBox";
            this.startingGroupBox.Size = new System.Drawing.Size(288, 64);
            this.startingGroupBox.TabIndex = 1;
            this.startingGroupBox.TabStop = false;
            this.startingGroupBox.Text = "Starting Time";
            // 
            // startingDateTimePicker
            // 
            this.startingDateTimePicker.CustomFormat = "MMMM dd, yyyy hh:mm:ss tt";
            this.startingDateTimePicker.Enabled = false;
            this.startingDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.startingDateTimePicker.Location = new System.Drawing.Point(8, 24);
            this.startingDateTimePicker.Name = "startingDateTimePicker";
            this.startingDateTimePicker.Size = new System.Drawing.Size(272, 20);
            this.startingDateTimePicker.TabIndex = 0;
            this.startingDateTimePicker.Value = new System.DateTime(2005, 1, 1, 0, 0, 0, 0);
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(306, 312);
            this.Controls.Add(this.timeGroupBox);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.channelParameterGroupBox);
            this.Controls.Add(this.startingGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Measure GPS Timestamp";
            this.timeGroupBox.ResumeLayout(false);
            this.channelParameterGroupBox.ResumeLayout(false);
            this.startingGroupBox.ResumeLayout(false);
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
                // Create the task
                myTask = new Task();

                // Determine the method
                CIGpsSyncMethod method;
                switch (syncMethodComboBox.SelectedItem.ToString())
                {
                    case "IRIG-B":
                        method = CIGpsSyncMethod.IrigB;
                        break;
                    case "PPS":
                        method = CIGpsSyncMethod.Pps;
                        break;
                    case "None":
                    default:
                        method = CIGpsSyncMethod.None;
                        break;
                }   

                // Create the channel
                CIChannel myChan = myTask.CIChannels.CreateGpsTimestampChannel(counterComboBox.Text, 
                    "", 
                    method,
                    CITimestampUnits.Seconds);

                // Set the synchronization source
                myChan.GpsSyncSource = syncSourceComboBox.Text;

                // Set start time
                if (syncMethodComboBox.Text != "IRIG-B")
                {
                    DateTime startOfYear = new DateTime(startingDateTimePicker.Value.Year, 1, 1);
                    TimeSpan startTime = startingDateTimePicker.Value - startOfYear;
                    myChan.TimestampInitialSeconds = (long)startTime.TotalSeconds;
                }

                // Create the reader
                myCounterReader = new CounterReader(myTask.Stream);

                // Setup UI
                startButton.Enabled = false;
                stopButton.Enabled = true;
                counterComboBox.Enabled = false;
                syncMethodComboBox.Enabled = false;
                syncSourceComboBox.Enabled = false;
                startingDateTimePicker.Enabled = false;

                // Begin loop
                loopTimer.Enabled = true;
            }
            catch(DaqException exception)
            {
                loopTimer.Enabled = false;
                myTask.Dispose();
                MessageBox.Show(exception.Message);

                startButton.Enabled = true;
                stopButton.Enabled = false;
                counterComboBox.Enabled = true;
                syncMethodComboBox.Enabled = true;
                syncSourceComboBox.Enabled = true;

                if (syncMethodComboBox.Text == "IRIG-B")
                    startingDateTimePicker.Enabled = false;
                else
                    startingDateTimePicker.Enabled = true;
            }
        }

        private void loopTimer_Tick(object sender, System.EventArgs e)
        {           
            try
            {
                // Get the number of seconds since midnight on January 1st
                reading = myCounterReader.ReadSingleSampleDouble();

                // Update time display
                DateTime today = DateTime.Today;
                DateTime startOfYear = new DateTime(today.Year, 1, 1);
                DateTime now = startOfYear + TimeSpan.FromSeconds(reading);

                // In order to prevent an exception from setting the DateTimePicker's
                // MinDate to be later than the MaxDate, first set the MinDate to
                // the earliest possible DateTime value, and the MaxDate to the latest
                // possible value.
                gpsDateTimePicker.MinDate = DateTimePicker.MinDateTime;
                gpsDateTimePicker.MaxDate = DateTimePicker.MaxDateTime;
                gpsDateTimePicker.Value = now;
                gpsDateTimePicker.MinDate = now;
                gpsDateTimePicker.MaxDate = now + TimeSpan.FromMilliseconds(1);
            }
            catch (DaqException exception)
            {
                loopTimer.Enabled = false;
                myTask.Dispose();
                MessageBox.Show(exception.Message);
                
                startButton.Enabled = true;
                stopButton.Enabled = false;
                counterComboBox.Enabled = true;
                syncMethodComboBox.Enabled = true;
                syncSourceComboBox.Enabled = true;

                if (syncMethodComboBox.Text == "IRIG-B")
                    startingDateTimePicker.Enabled = false;
                else
                    startingDateTimePicker.Enabled = true;
            }
        }

        private void stopButton_Click(object sender, System.EventArgs e)
        {
            loopTimer.Enabled = false;
            myTask.Stop();
            myTask.Dispose();

            startButton.Enabled = true;
            stopButton.Enabled = false;
            counterComboBox.Enabled = true;
            syncMethodComboBox.Enabled = true;
            syncSourceComboBox.Enabled = true;

            if (syncMethodComboBox.Text == "IRIG-B")
                startingDateTimePicker.Enabled = false;
            else
                startingDateTimePicker.Enabled = true;
        }

        private void syncMethodComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (syncMethodComboBox.Text == "IRIG-B")
            {
                startingDateTimePicker.Enabled = false;
            }
            else
            {
                startingDateTimePicker.Enabled = true;
            }
        }
    }
}
