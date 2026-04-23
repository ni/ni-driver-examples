/******************************************************************************
*
* Example program:
*   MeasGpsTimestamp_BuffFinite
*
* Category:
*   CI
*
* Description:
*   This example demonstrates how to use a finite buffereded task to measure
*   time using a GPS Timestamp Channel.  The Synchronization Method,
*   Synchronization Source, Sample Clock Source, and Samples per Channel are all
*   configurable.
*
* Instructions for running:
*   1.  Select the Physical Channel which corresponds to the GPS counter you
*       want to count edges on the DAQ device.
*   2.  Select the Synchronization Method and Synchronization Source to specify
*       which type of  GPS synchronization signal you want to use.
*   3.  Set the Sample Clock Source and Samples per Channel to configure timing
*       for the measurement.
*   4.  If applicable, set the initial time.Note:  The initial time is only
*       applicable when the synchronization method is PPS or None.
*   5.  Set the Rate of the Acquisition.
*   6.  Select the Source of the Digital Reference Trigger for the acquisition.
*
* Steps:
*   1.  Create a new counter input task.
*   2.  Create a GPS Timestamp channel on the task.
*   3.  Set the initial time if applicable.
*   4.  Create a CounterReader and associate it with the task by using the
*       task's stream.
*   5.  Configure the sample clock, set the acquisition mode to finite.
*   6.  Use the ReferenceTrigger object properties to configure a digital edge
*       trigger.
*   7.  Start the task.
*   8.  Read the count values and update the GPS time.
*   9.  Dispose the Task object to clean-up any resources associated with the
*       task.
*   10. Handle any DaqExceptions, if they occur.
*
* I/O Connections Overview:
*   Make sure your signal input terminals match the physical channel text box. 
*   For more information on the input and output terminals for your device, open
*   the NI-DAQmx Help, and refer to the NI-DAQmx Device Terminals and Device
*   Considerations books in the table of contents.
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

namespace NationalInstruments.Examples.MeasGpsTimestamp_BuffFinite
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
        private Task myTask;
        private CounterReader myCounterReader;

        internal System.Windows.Forms.Button startButton;
        internal System.Windows.Forms.GroupBox channelParameterGroupBox;
        private System.Windows.Forms.ComboBox counterComboBox;
        internal System.Windows.Forms.Label synchronizationMethodLabel;
        internal System.Windows.Forms.Label counterLabel;
        internal System.Windows.Forms.ComboBox synchronizationMethodComboBox;
        internal System.Windows.Forms.ComboBox synchronizationSourceComboBox;
        internal System.Windows.Forms.Label synchronizationSourceLabel;
        internal System.Windows.Forms.GroupBox startingGroupBox;
        private System.Windows.Forms.DateTimePicker startingDateTimePicker;
        private System.Windows.Forms.GroupBox timeGroupBox;
        internal System.Windows.Forms.GroupBox timingParametersGroupBox;
        internal System.Windows.Forms.Label samplesPerChannelLabel;
        internal System.Windows.Forms.Label rateLabel;
        internal System.Windows.Forms.Label sampleClockSourceLabel;
        internal System.Windows.Forms.ComboBox sampleClockSourceComboBox;
        internal System.Windows.Forms.NumericUpDown rateNumericUpDown;
        internal System.Windows.Forms.NumericUpDown samplesPerChannelNumericUpDown;
        private System.Windows.Forms.ListBox gpsTimeListBox;
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

            counterComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.CI, PhysicalChannelAccess.External));
            if (counterComboBox.Items.Count > 0)
                counterComboBox.SelectedIndex = 0;

            synchronizationMethodComboBox.SelectedIndex = 0;

            synchronizationSourceComboBox.Items.AddRange(DaqSystem.Local.GetTerminals(TerminalTypes.Basic));
            if (synchronizationSourceComboBox.Items.Count > 0)
                synchronizationSourceComboBox.SelectedIndex = 1;

            sampleClockSourceComboBox.Items.AddRange(DaqSystem.Local.GetTerminals(TerminalTypes.All));
           

            startingDateTimePicker.Enabled = false;
            startingDateTimePicker.Value = new DateTime(DateTime.Today.Year, 1, 1);
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
            this.startButton = new System.Windows.Forms.Button();
            this.channelParameterGroupBox = new System.Windows.Forms.GroupBox();
            this.counterComboBox = new System.Windows.Forms.ComboBox();
            this.synchronizationMethodLabel = new System.Windows.Forms.Label();
            this.counterLabel = new System.Windows.Forms.Label();
            this.synchronizationMethodComboBox = new System.Windows.Forms.ComboBox();
            this.synchronizationSourceComboBox = new System.Windows.Forms.ComboBox();
            this.synchronizationSourceLabel = new System.Windows.Forms.Label();
            this.startingGroupBox = new System.Windows.Forms.GroupBox();
            this.startingDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.timeGroupBox = new System.Windows.Forms.GroupBox();
            this.gpsTimeListBox = new System.Windows.Forms.ListBox();
            this.timingParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.samplesPerChannelNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.rateNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.samplesPerChannelLabel = new System.Windows.Forms.Label();
            this.rateLabel = new System.Windows.Forms.Label();
            this.sampleClockSourceComboBox = new System.Windows.Forms.ComboBox();
            this.sampleClockSourceLabel = new System.Windows.Forms.Label();
            this.channelParameterGroupBox.SuspendLayout();
            this.startingGroupBox.SuspendLayout();
            this.timeGroupBox.SuspendLayout();
            this.timingParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerChannelNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rateNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // startButton
            // 
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.startButton.Location = new System.Drawing.Point(224, 336);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(72, 24);
            this.startButton.TabIndex = 13;
            this.startButton.Text = "&Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // channelParameterGroupBox
            // 
            this.channelParameterGroupBox.Controls.Add(this.counterComboBox);
            this.channelParameterGroupBox.Controls.Add(this.synchronizationMethodLabel);
            this.channelParameterGroupBox.Controls.Add(this.counterLabel);
            this.channelParameterGroupBox.Controls.Add(this.synchronizationMethodComboBox);
            this.channelParameterGroupBox.Controls.Add(this.synchronizationSourceComboBox);
            this.channelParameterGroupBox.Controls.Add(this.synchronizationSourceLabel);
            this.channelParameterGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.channelParameterGroupBox.Location = new System.Drawing.Point(9, 9);
            this.channelParameterGroupBox.Name = "channelParameterGroupBox";
            this.channelParameterGroupBox.Size = new System.Drawing.Size(288, 119);
            this.channelParameterGroupBox.TabIndex = 1;
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
            // synchronizationMethodLabel
            // 
            this.synchronizationMethodLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.synchronizationMethodLabel.Location = new System.Drawing.Point(8, 58);
            this.synchronizationMethodLabel.Name = "synchronizationMethodLabel";
            this.synchronizationMethodLabel.Size = new System.Drawing.Size(120, 16);
            this.synchronizationMethodLabel.TabIndex = 2;
            this.synchronizationMethodLabel.Text = "S&ynchronization &Method:";
            // 
            // counterLabel
            // 
            this.counterLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.counterLabel.Location = new System.Drawing.Point(8, 26);
            this.counterLabel.Name = "counterLabel";
            this.counterLabel.Size = new System.Drawing.Size(72, 16);
            this.counterLabel.TabIndex = 0;
            this.counterLabel.Text = "&GPS Counter:";
            // 
            // synchronizationMethodComboBox
            // 
            this.synchronizationMethodComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.synchronizationMethodComboBox.Items.AddRange(new object[] {
                                                                    "IRIG-B",
                                                                    "PPS",
                                                                    "None"});
            this.synchronizationMethodComboBox.Location = new System.Drawing.Point(136, 56);
            this.synchronizationMethodComboBox.Name = "synchronizationMethodComboBox";
            this.synchronizationMethodComboBox.Size = new System.Drawing.Size(144, 21);
            this.synchronizationMethodComboBox.TabIndex = 3;
            this.synchronizationMethodComboBox.SelectedIndexChanged += new System.EventHandler(this.synchronizationMethodComboBox_SelectedIndexChanged);
            // 
            // synchronizationSourceComboBox
            // 
            this.synchronizationSourceComboBox.Location = new System.Drawing.Point(136, 88);
            this.synchronizationSourceComboBox.Name = "synchronizationSourceComboBox";
            this.synchronizationSourceComboBox.Size = new System.Drawing.Size(144, 21);
            this.synchronizationSourceComboBox.TabIndex = 5;
            this.synchronizationSourceComboBox.Text = "/Dev1/PFI7";
            // 
            // synchronizationSourceLabel
            // 
            this.synchronizationSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.synchronizationSourceLabel.Location = new System.Drawing.Point(8, 90);
            this.synchronizationSourceLabel.Name = "synchronizationSourceLabel";
            this.synchronizationSourceLabel.Size = new System.Drawing.Size(120, 16);
            this.synchronizationSourceLabel.TabIndex = 4;
            this.synchronizationSourceLabel.Text = "Sy&nchronization &Source:";
            // 
            // startingGroupBox
            // 
            this.startingGroupBox.Controls.Add(this.startingDateTimePicker);
            this.startingGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.startingGroupBox.Location = new System.Drawing.Point(9, 264);
            this.startingGroupBox.Name = "startingGroupBox";
            this.startingGroupBox.Size = new System.Drawing.Size(288, 64);
            this.startingGroupBox.TabIndex = 3;
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
            this.startingDateTimePicker.TabIndex = 12;
            this.startingDateTimePicker.Value = new System.DateTime(2005, 1, 1, 0, 0, 0, 0);
            // 
            // timeGroupBox
            // 
            this.timeGroupBox.Controls.Add(this.gpsTimeListBox);
            this.timeGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.timeGroupBox.Location = new System.Drawing.Point(312, 9);
            this.timeGroupBox.Name = "timeGroupBox";
            this.timeGroupBox.Size = new System.Drawing.Size(224, 351);
            this.timeGroupBox.TabIndex = 0;
            this.timeGroupBox.TabStop = false;
            this.timeGroupBox.Text = "GPS Time/Date";
            // 
            // gpsTimeListBox
            // 
            this.gpsTimeListBox.Location = new System.Drawing.Point(8, 16);
            this.gpsTimeListBox.Name = "gpsTimeListBox";
            this.gpsTimeListBox.Size = new System.Drawing.Size(208, 329);
            this.gpsTimeListBox.TabIndex = 0;
            // 
            // timingParametersGroupBox
            // 
            this.timingParametersGroupBox.Controls.Add(this.samplesPerChannelNumericUpDown);
            this.timingParametersGroupBox.Controls.Add(this.rateNumericUpDown);
            this.timingParametersGroupBox.Controls.Add(this.samplesPerChannelLabel);
            this.timingParametersGroupBox.Controls.Add(this.rateLabel);
            this.timingParametersGroupBox.Controls.Add(this.sampleClockSourceComboBox);
            this.timingParametersGroupBox.Controls.Add(this.sampleClockSourceLabel);
            this.timingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.timingParametersGroupBox.Location = new System.Drawing.Point(8, 136);
            this.timingParametersGroupBox.Name = "timingParametersGroupBox";
            this.timingParametersGroupBox.Size = new System.Drawing.Size(288, 119);
            this.timingParametersGroupBox.TabIndex = 2;
            this.timingParametersGroupBox.TabStop = false;
            this.timingParametersGroupBox.Text = "Timing Parameters";
            // 
            // samplesPerChannelNumericUpDown
            // 
            this.samplesPerChannelNumericUpDown.Location = new System.Drawing.Point(136, 56);
            this.samplesPerChannelNumericUpDown.Maximum = new System.Decimal(new int[] {
                                                                                    1000000,
                                                                                    0,
                                                                                    0,
                                                                                    0});
            this.samplesPerChannelNumericUpDown.Minimum = new System.Decimal(new int[] {
                                                                                    1,
                                                                                    0,
                                                                                    0,
                                                                                    0});
            this.samplesPerChannelNumericUpDown.Name = "samplesPerChannelNumericUpDown";
            this.samplesPerChannelNumericUpDown.Size = new System.Drawing.Size(144, 20);
            this.samplesPerChannelNumericUpDown.TabIndex = 9;
            this.samplesPerChannelNumericUpDown.Value = new System.Decimal(new int[] {
                                                                                  100,
                                                                                  0,
                                                                                  0,
                                                                                  0});
            // 
            // rateNumericUpDown
            // 
            this.rateNumericUpDown.DecimalPlaces = 2;
            this.rateNumericUpDown.Location = new System.Drawing.Point(136, 24);
            this.rateNumericUpDown.Maximum = new System.Decimal(new int[] {
                                                                       1000000,
                                                                       0,
                                                                       0,
                                                                       0});
            this.rateNumericUpDown.Minimum = new System.Decimal(new int[] {
                                                                       1,
                                                                       0,
                                                                       0,
                                                                       0});
            this.rateNumericUpDown.Name = "rateNumericUpDown";
            this.rateNumericUpDown.Size = new System.Drawing.Size(144, 20);
            this.rateNumericUpDown.TabIndex = 7;
            this.rateNumericUpDown.Value = new System.Decimal(new int[] {
                                                                     1000,
                                                                     0,
                                                                     0,
                                                                     0});
            // 
            // samplesPerChannelLabel
            // 
            this.samplesPerChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.samplesPerChannelLabel.Location = new System.Drawing.Point(8, 58);
            this.samplesPerChannelLabel.Name = "samplesPerChannelLabel";
            this.samplesPerChannelLabel.Size = new System.Drawing.Size(120, 16);
            this.samplesPerChannelLabel.TabIndex = 8;
            this.samplesPerChannelLabel.Text = "Samples per &Channel:";
            // 
            // rateLabel
            // 
            this.rateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rateLabel.Location = new System.Drawing.Point(8, 26);
            this.rateLabel.Name = "rateLabel";
            this.rateLabel.Size = new System.Drawing.Size(72, 16);
            this.rateLabel.TabIndex = 6;
            this.rateLabel.Text = "&Rate:";
            // 
            // sampleClockSourceComboBox
            // 
            this.sampleClockSourceComboBox.Location = new System.Drawing.Point(136, 88);
            this.sampleClockSourceComboBox.Name = "sampleClockSourceComboBox";
            this.sampleClockSourceComboBox.Size = new System.Drawing.Size(144, 21);
            this.sampleClockSourceComboBox.TabIndex = 11;
            this.sampleClockSourceComboBox.Text = "/Dev1/PFI9";
            // 
            // sampleClockSourceLabel
            // 
            this.sampleClockSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.sampleClockSourceLabel.Location = new System.Drawing.Point(8, 90);
            this.sampleClockSourceLabel.Name = "sampleClockSourceLabel";
            this.sampleClockSourceLabel.Size = new System.Drawing.Size(120, 16);
            this.sampleClockSourceLabel.TabIndex = 10;
            this.sampleClockSourceLabel.Text = "Sample C&lock Source:";
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(546, 367);
            this.Controls.Add(this.timingParametersGroupBox);
            this.Controls.Add(this.timeGroupBox);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.channelParameterGroupBox);
            this.Controls.Add(this.startingGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Measure GPS Timestamp - Buffered Finite";
            this.channelParameterGroupBox.ResumeLayout(false);
            this.startingGroupBox.ResumeLayout(false);
            this.timeGroupBox.ResumeLayout(false);
            this.timingParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerChannelNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rateNumericUpDown)).EndInit();
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
            Cursor.Current = Cursors.WaitCursor;
            startButton.Enabled = false;
            gpsTimeListBox.Items.Clear();

            try
            {
                myTask = new Task();
                
                CIGpsSyncMethod method;
                switch (synchronizationMethodComboBox.SelectedItem.ToString())
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

                CIChannel myChan = myTask.CIChannels.CreateGpsTimestampChannel(counterComboBox.Text, 
                    "", 
                    method,
                    CITimestampUnits.Seconds);

                myChan.GpsSyncSource = synchronizationSourceComboBox.Text;

                if (synchronizationMethodComboBox.Text != "IRIG-B")
                {
                    DateTime startOfYear = new DateTime(startingDateTimePicker.Value.Year, 1, 1);
                    TimeSpan startTime = startingDateTimePicker.Value - startOfYear;
                    myChan.TimestampInitialSeconds = (long)startTime.TotalSeconds;
                }

                myCounterReader = new CounterReader(myTask.Stream);

                myTask.Timing.ConfigureSampleClock(sampleClockSourceComboBox.Text,
                    (double)rateNumericUpDown.Value,
                    SampleClockActiveEdge.Rising,
                    SampleQuantityMode.FiniteSamples,
                    (int)samplesPerChannelNumericUpDown.Value);

                myTask.Start();

                double [] reading = myCounterReader.ReadMultiSampleDouble(-1);
                foreach(double readingSample in reading)
                {
                    DateTime today = DateTime.Today;
                    DateTime startOfYear = new DateTime(today.Year, 1, 1);
                    DateTime now = startOfYear + TimeSpan.FromSeconds(readingSample);

                    gpsTimeListBox.Items.Add(now);
                }
            } 
            catch(DaqException exception)
            {
                MessageBox.Show(exception.Message);
                if (synchronizationMethodComboBox.Text == "IRIG-B")
                {
                    startingDateTimePicker.Enabled = false;
                }
                else
                {
                    startingDateTimePicker.Enabled = true;
                }
            } 
            finally 
            {
                myTask.Dispose();
            }

            Cursor.Current = Cursors.Default;
            startButton.Enabled = true;
        }

        private void synchronizationMethodComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (synchronizationMethodComboBox.Text == "IRIG-B")
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
