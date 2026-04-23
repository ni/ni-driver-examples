/******************************************************************************
*
* Example program:
*   WriteDigChan_WatchdogTimer
*
* Category:
*   DO
*
* Description:
*   This example demonstrates how to write values to a digital output channel,
*   using a watchdog timer.
*
* Instructions for running:
*   1.  Select the digital lines on the DAQ device to be written to.
*   2.  Select a value to write.
*
* Steps:
*   1.  Create an output task and a watchdog timer task.
*   2.  Create a digital output channel. Use one channel for all lines.
*   3.  Call Task.Start() on both tasks.
*   4.  Write the digital boolean array data. This write method writes a single
*       sample of digital data on demand, so no timeout is necessary. This
*       example uses an asynchronous write method and installs a callback.
*   5.  Inside the callback, reset the watchdog timer and perform another write.
*   6.  When the user presses the stop button, stop the tasks.
*   7.  Dispose the Task object to clean-up any resources associated with the
*       task.
*   8.  Handle any DaqExceptions, if they occur.
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
*   Make sure your signal output terminals match the Lines I/O Control. In this
*   case wire the item to receive the signal to the first eight digital lines on
*   your DAQ Device.  For more information on the input and output terminals for
*   your device, open the NI-DAQmx Help, and refer to the NI-DAQmx Device
*   Terminals and Device Considerations books in the table of contents.
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

namespace NationalInstruments.Examples.WriteDigChan_WatchdogTimer
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
	    private Task runningTask;
	    private Task outputTask = null;
	    private Task watchdogTask = null;
	    private DigitalSingleChannelWriter writer = null;
        private System.Windows.Forms.CheckBox line0;
        private System.Windows.Forms.CheckBox line1;
        private System.Windows.Forms.CheckBox line2;
        private System.Windows.Forms.CheckBox line3;
        private System.Windows.Forms.CheckBox line4;
        private System.Windows.Forms.CheckBox line5;
        private System.Windows.Forms.CheckBox line6;
        private System.Windows.Forms.CheckBox line7;
        private System.Windows.Forms.GroupBox dataToWriteGroupBox;
        private System.Windows.Forms.Label physicalChannelToWriteLabel;
        private System.Windows.Forms.Label warningText;
        private System.Windows.Forms.Label deviceNameLabel;
        private System.Windows.Forms.Label timeoutLabel;
        private System.Windows.Forms.TextBox timeout;
        private System.Windows.Forms.Label loopTimeLabel;
        private System.Windows.Forms.Label line0Label;
        private System.Windows.Forms.Label line1Label;
        private System.Windows.Forms.Label line2Label;
        private System.Windows.Forms.Label line3Label;
        private System.Windows.Forms.Label line4Label;
        private System.Windows.Forms.Label line5Label;
        private System.Windows.Forms.Label line6Label;
        private System.Windows.Forms.Label line7Label;
        private System.Windows.Forms.ComboBox watchdogExpirationState;
        private System.Windows.Forms.GroupBox watchdogSettingsGroupBox;
        private System.Windows.Forms.Label expirationStateLabel;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Label physicalChannelLabel;
        private System.Windows.Forms.NumericUpDown writeInterval;
        private System.Windows.Forms.ComboBox physicalChannelComboBox;
        private System.Windows.Forms.ComboBox deviceComboBox;
        private System.Windows.Forms.ComboBox watchdogPhysicalChannelComboBox;
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
            physicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DOLine, PhysicalChannelAccess.External));
            deviceComboBox.Items.AddRange(DaqSystem.Local.Devices);
            watchdogPhysicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DOLine | PhysicalChannelTypes.DOPort, PhysicalChannelAccess.External));
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
                if (outputTask != null)
                {
                    runningTask = null;
                    outputTask.Dispose();
                }
                if (watchdogTask != null)
                {
                    watchdogTask.Dispose();
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
            this.physicalChannelToWriteLabel = new System.Windows.Forms.Label();
            this.warningText = new System.Windows.Forms.Label();
            this.deviceNameLabel = new System.Windows.Forms.Label();
            this.timeoutLabel = new System.Windows.Forms.Label();
            this.timeout = new System.Windows.Forms.TextBox();
            this.loopTimeLabel = new System.Windows.Forms.Label();
            this.line0 = new System.Windows.Forms.CheckBox();
            this.line1 = new System.Windows.Forms.CheckBox();
            this.line2 = new System.Windows.Forms.CheckBox();
            this.line3 = new System.Windows.Forms.CheckBox();
            this.line4 = new System.Windows.Forms.CheckBox();
            this.line5 = new System.Windows.Forms.CheckBox();
            this.line6 = new System.Windows.Forms.CheckBox();
            this.line7 = new System.Windows.Forms.CheckBox();
            this.line0Label = new System.Windows.Forms.Label();
            this.line1Label = new System.Windows.Forms.Label();
            this.line2Label = new System.Windows.Forms.Label();
            this.line3Label = new System.Windows.Forms.Label();
            this.line4Label = new System.Windows.Forms.Label();
            this.line5Label = new System.Windows.Forms.Label();
            this.line6Label = new System.Windows.Forms.Label();
            this.line7Label = new System.Windows.Forms.Label();
            this.dataToWriteGroupBox = new System.Windows.Forms.GroupBox();
            this.watchdogExpirationState = new System.Windows.Forms.ComboBox();
            this.watchdogSettingsGroupBox = new System.Windows.Forms.GroupBox();
            this.watchdogPhysicalChannelComboBox = new System.Windows.Forms.ComboBox();
            this.expirationStateLabel = new System.Windows.Forms.Label();
            this.physicalChannelLabel = new System.Windows.Forms.Label();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.writeInterval = new System.Windows.Forms.NumericUpDown();
            this.physicalChannelComboBox = new System.Windows.Forms.ComboBox();
            this.deviceComboBox = new System.Windows.Forms.ComboBox();
            this.dataToWriteGroupBox.SuspendLayout();
            this.watchdogSettingsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.writeInterval)).BeginInit();
            this.SuspendLayout();
            // 
            // physicalChannelToWriteLabel
            // 
            this.physicalChannelToWriteLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.physicalChannelToWriteLabel.Location = new System.Drawing.Point(16, 16);
            this.physicalChannelToWriteLabel.Name = "physicalChannelToWriteLabel";
            this.physicalChannelToWriteLabel.Size = new System.Drawing.Size(160, 16);
            this.physicalChannelToWriteLabel.TabIndex = 2;
            this.physicalChannelToWriteLabel.Text = "Physical Channel to Write";
            // 
            // warningText
            // 
            this.warningText.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.warningText.Location = new System.Drawing.Point(16, 64);
            this.warningText.Name = "warningText";
            this.warningText.Size = new System.Drawing.Size(160, 32);
            this.warningText.TabIndex = 4;
            this.warningText.Text = "You must specify exactly eight lines in the channel string";
            // 
            // deviceNameLabel
            // 
            this.deviceNameLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.deviceNameLabel.Location = new System.Drawing.Point(16, 104);
            this.deviceNameLabel.Name = "deviceNameLabel";
            this.deviceNameLabel.Size = new System.Drawing.Size(112, 16);
            this.deviceNameLabel.TabIndex = 5;
            this.deviceNameLabel.Text = "Device Name";
            // 
            // timeoutLabel
            // 
            this.timeoutLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.timeoutLabel.Location = new System.Drawing.Point(16, 152);
            this.timeoutLabel.Name = "timeoutLabel";
            this.timeoutLabel.Size = new System.Drawing.Size(112, 16);
            this.timeoutLabel.TabIndex = 7;
            this.timeoutLabel.Text = "Timeout (seconds)";
            // 
            // timeout
            // 
            this.timeout.Location = new System.Drawing.Point(16, 168);
            this.timeout.Name = "timeout";
            this.timeout.Size = new System.Drawing.Size(152, 20);
            this.timeout.TabIndex = 8;
            this.timeout.Text = "0.8";
            // 
            // loopTimeLabel
            // 
            this.loopTimeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.loopTimeLabel.Location = new System.Drawing.Point(16, 200);
            this.loopTimeLabel.Name = "loopTimeLabel";
            this.loopTimeLabel.Size = new System.Drawing.Size(152, 16);
            this.loopTimeLabel.TabIndex = 9;
            this.loopTimeLabel.Text = "Write Interval (seconds)";
            // 
            // line0
            // 
            this.line0.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.line0.Location = new System.Drawing.Point(16, 24);
            this.line0.Name = "line0";
            this.line0.Size = new System.Drawing.Size(16, 24);
            this.line0.TabIndex = 1;
            // 
            // line1
            // 
            this.line1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.line1.Location = new System.Drawing.Point(48, 24);
            this.line1.Name = "line1";
            this.line1.Size = new System.Drawing.Size(16, 24);
            this.line1.TabIndex = 3;
            // 
            // line2
            // 
            this.line2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.line2.Location = new System.Drawing.Point(80, 24);
            this.line2.Name = "line2";
            this.line2.Size = new System.Drawing.Size(16, 24);
            this.line2.TabIndex = 5;
            // 
            // line3
            // 
            this.line3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.line3.Location = new System.Drawing.Point(112, 24);
            this.line3.Name = "line3";
            this.line3.Size = new System.Drawing.Size(16, 24);
            this.line3.TabIndex = 7;
            // 
            // line4
            // 
            this.line4.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.line4.Location = new System.Drawing.Point(144, 24);
            this.line4.Name = "line4";
            this.line4.Size = new System.Drawing.Size(16, 24);
            this.line4.TabIndex = 9;
            // 
            // line5
            // 
            this.line5.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.line5.Location = new System.Drawing.Point(176, 24);
            this.line5.Name = "line5";
            this.line5.Size = new System.Drawing.Size(16, 24);
            this.line5.TabIndex = 11;
            // 
            // line6
            // 
            this.line6.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.line6.Location = new System.Drawing.Point(208, 24);
            this.line6.Name = "line6";
            this.line6.Size = new System.Drawing.Size(16, 24);
            this.line6.TabIndex = 13;
            // 
            // line7
            // 
            this.line7.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.line7.Location = new System.Drawing.Point(240, 24);
            this.line7.Name = "line7";
            this.line7.Size = new System.Drawing.Size(16, 24);
            this.line7.TabIndex = 15;
            // 
            // line0Label
            // 
            this.line0Label.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.line0Label.Location = new System.Drawing.Point(16, 48);
            this.line0Label.Name = "line0Label";
            this.line0Label.Size = new System.Drawing.Size(16, 16);
            this.line0Label.TabIndex = 0;
            this.line0Label.Text = "0";
            // 
            // line1Label
            // 
            this.line1Label.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.line1Label.Location = new System.Drawing.Point(48, 48);
            this.line1Label.Name = "line1Label";
            this.line1Label.Size = new System.Drawing.Size(16, 16);
            this.line1Label.TabIndex = 2;
            this.line1Label.Text = "1";
            // 
            // line2Label
            // 
            this.line2Label.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.line2Label.Location = new System.Drawing.Point(80, 48);
            this.line2Label.Name = "line2Label";
            this.line2Label.Size = new System.Drawing.Size(16, 16);
            this.line2Label.TabIndex = 4;
            this.line2Label.Text = "2";
            // 
            // line3Label
            // 
            this.line3Label.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.line3Label.Location = new System.Drawing.Point(112, 48);
            this.line3Label.Name = "line3Label";
            this.line3Label.Size = new System.Drawing.Size(16, 16);
            this.line3Label.TabIndex = 6;
            this.line3Label.Text = "3";
            // 
            // line4Label
            // 
            this.line4Label.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.line4Label.Location = new System.Drawing.Point(144, 48);
            this.line4Label.Name = "line4Label";
            this.line4Label.Size = new System.Drawing.Size(16, 16);
            this.line4Label.TabIndex = 8;
            this.line4Label.Text = "4";
            // 
            // line5Label
            // 
            this.line5Label.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.line5Label.Location = new System.Drawing.Point(176, 48);
            this.line5Label.Name = "line5Label";
            this.line5Label.Size = new System.Drawing.Size(16, 16);
            this.line5Label.TabIndex = 10;
            this.line5Label.Text = "5";
            // 
            // line6Label
            // 
            this.line6Label.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.line6Label.Location = new System.Drawing.Point(208, 48);
            this.line6Label.Name = "line6Label";
            this.line6Label.Size = new System.Drawing.Size(16, 16);
            this.line6Label.TabIndex = 12;
            this.line6Label.Text = "6";
            // 
            // line7Label
            // 
            this.line7Label.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.line7Label.Location = new System.Drawing.Point(240, 48);
            this.line7Label.Name = "line7Label";
            this.line7Label.Size = new System.Drawing.Size(16, 16);
            this.line7Label.TabIndex = 14;
            this.line7Label.Text = "7";
            // 
            // dataToWriteGroupBox
            // 
            this.dataToWriteGroupBox.Controls.Add(this.line5Label);
            this.dataToWriteGroupBox.Controls.Add(this.line2Label);
            this.dataToWriteGroupBox.Controls.Add(this.line6);
            this.dataToWriteGroupBox.Controls.Add(this.line1Label);
            this.dataToWriteGroupBox.Controls.Add(this.line3);
            this.dataToWriteGroupBox.Controls.Add(this.line6Label);
            this.dataToWriteGroupBox.Controls.Add(this.line1);
            this.dataToWriteGroupBox.Controls.Add(this.line0Label);
            this.dataToWriteGroupBox.Controls.Add(this.line4);
            this.dataToWriteGroupBox.Controls.Add(this.line7Label);
            this.dataToWriteGroupBox.Controls.Add(this.line3Label);
            this.dataToWriteGroupBox.Controls.Add(this.line0);
            this.dataToWriteGroupBox.Controls.Add(this.line4Label);
            this.dataToWriteGroupBox.Controls.Add(this.line7);
            this.dataToWriteGroupBox.Controls.Add(this.line5);
            this.dataToWriteGroupBox.Controls.Add(this.line2);
            this.dataToWriteGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.dataToWriteGroupBox.Location = new System.Drawing.Point(192, 16);
            this.dataToWriteGroupBox.Name = "dataToWriteGroupBox";
            this.dataToWriteGroupBox.Size = new System.Drawing.Size(272, 80);
            this.dataToWriteGroupBox.TabIndex = 11;
            this.dataToWriteGroupBox.TabStop = false;
            this.dataToWriteGroupBox.Text = "Data to Write";
            // 
            // watchdogExpirationState
            // 
            this.watchdogExpirationState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.watchdogExpirationState.Items.AddRange(new object[] {
                                                                         "High",
                                                                         "Low",
                                                                         "Tristate",
                                                                         "No Change"});
            this.watchdogExpirationState.Location = new System.Drawing.Point(16, 112);
            this.watchdogExpirationState.Name = "watchdogExpirationState";
            this.watchdogExpirationState.Size = new System.Drawing.Size(152, 21);
            this.watchdogExpirationState.TabIndex = 3;
            // 
            // watchdogSettingsGroupBox
            // 
            this.watchdogSettingsGroupBox.Controls.Add(this.watchdogPhysicalChannelComboBox);
            this.watchdogSettingsGroupBox.Controls.Add(this.watchdogExpirationState);
            this.watchdogSettingsGroupBox.Controls.Add(this.expirationStateLabel);
            this.watchdogSettingsGroupBox.Controls.Add(this.physicalChannelLabel);
            this.watchdogSettingsGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.watchdogSettingsGroupBox.Location = new System.Drawing.Point(192, 104);
            this.watchdogSettingsGroupBox.Name = "watchdogSettingsGroupBox";
            this.watchdogSettingsGroupBox.Size = new System.Drawing.Size(184, 160);
            this.watchdogSettingsGroupBox.TabIndex = 12;
            this.watchdogSettingsGroupBox.TabStop = false;
            this.watchdogSettingsGroupBox.Text = "Expiration State (Watchdog)";
            // 
            // watchdogPhysicalChannelComboBox
            // 
            this.watchdogPhysicalChannelComboBox.Location = new System.Drawing.Point(16, 64);
            this.watchdogPhysicalChannelComboBox.Name = "watchdogPhysicalChannelComboBox";
            this.watchdogPhysicalChannelComboBox.Size = new System.Drawing.Size(152, 21);
            this.watchdogPhysicalChannelComboBox.TabIndex = 1;
            this.watchdogPhysicalChannelComboBox.Text = "Dev1/port0/line0:7";
            // 
            // expirationStateLabel
            // 
            this.expirationStateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.expirationStateLabel.Location = new System.Drawing.Point(16, 96);
            this.expirationStateLabel.Name = "expirationStateLabel";
            this.expirationStateLabel.Size = new System.Drawing.Size(88, 16);
            this.expirationStateLabel.TabIndex = 2;
            this.expirationStateLabel.Text = "Expiration State";
            // 
            // physicalChannelLabel
            // 
            this.physicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.physicalChannelLabel.Location = new System.Drawing.Point(16, 48);
            this.physicalChannelLabel.Name = "physicalChannelLabel";
            this.physicalChannelLabel.Size = new System.Drawing.Size(112, 16);
            this.physicalChannelLabel.TabIndex = 0;
            this.physicalChannelLabel.Text = "Physical Channel Name";
            // 
            // startButton
            // 
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.startButton.Location = new System.Drawing.Point(384, 112);
            this.startButton.Name = "startButton";
            this.startButton.TabIndex = 0;
            this.startButton.Text = "&Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.stopButton.Location = new System.Drawing.Point(384, 144);
            this.stopButton.Name = "stopButton";
            this.stopButton.TabIndex = 1;
            this.stopButton.Text = "S&top";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // writeInterval
            // 
            this.writeInterval.DecimalPlaces = 2;
            this.writeInterval.Increment = new System.Decimal(new int[] {
                                                                            1,
                                                                            0,
                                                                            0,
                                                                            131072});
            this.writeInterval.Location = new System.Drawing.Point(16, 216);
            this.writeInterval.Name = "writeInterval";
            this.writeInterval.Size = new System.Drawing.Size(152, 20);
            this.writeInterval.TabIndex = 10;
            this.writeInterval.Value = new System.Decimal(new int[] {
                                                                        5,
                                                                        0,
                                                                        0,
                                                                        131072});
            // 
            // physicalChannelComboBox
            // 
            this.physicalChannelComboBox.Location = new System.Drawing.Point(16, 32);
            this.physicalChannelComboBox.Name = "physicalChannelComboBox";
            this.physicalChannelComboBox.Size = new System.Drawing.Size(152, 21);
            this.physicalChannelComboBox.TabIndex = 3;
            this.physicalChannelComboBox.Text = "Dev1/port0/line0:7";
            // 
            // deviceComboBox
            // 
            this.deviceComboBox.Location = new System.Drawing.Point(16, 120);
            this.deviceComboBox.Name = "deviceComboBox";
            this.deviceComboBox.Size = new System.Drawing.Size(152, 21);
            this.deviceComboBox.TabIndex = 6;
            this.deviceComboBox.Text = "Dev1";
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(472, 270);
            this.Controls.Add(this.deviceComboBox);
            this.Controls.Add(this.physicalChannelComboBox);
            this.Controls.Add(this.writeInterval);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.watchdogSettingsGroupBox);
            this.Controls.Add(this.dataToWriteGroupBox);
            this.Controls.Add(this.warningText);
            this.Controls.Add(this.physicalChannelToWriteLabel);
            this.Controls.Add(this.timeout);
            this.Controls.Add(this.deviceNameLabel);
            this.Controls.Add(this.timeoutLabel);
            this.Controls.Add(this.loopTimeLabel);
            this.Controls.Add(this.stopButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Write Digital Channel - Watchdog Timer";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.dataToWriteGroupBox.ResumeLayout(false);
            this.watchdogSettingsGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.writeInterval)).EndInit();
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

        private void DimControls(bool startOrStop)
        {
            physicalChannelComboBox.Enabled = !startOrStop;
            deviceComboBox.Enabled = !startOrStop;
            timeout.Enabled = !startOrStop;
            watchdogPhysicalChannelComboBox.Enabled = !startOrStop;
            watchdogExpirationState.Enabled = !startOrStop;
            startButton.Enabled = !startOrStop;
            stopButton.Enabled = startOrStop;
        }

        private bool[] GetDataAsBoolArray()
        {
            return new bool[] {
                line0.Checked,
                line1.Checked,
                line2.Checked,
                line3.Checked,
                line4.Checked,
                line5.Checked,
                line6.Checked,
                line7.Checked
            };
        }

        private WatchdogDOExpirationState GetWatchdogExpirationState()
        {
            switch(watchdogExpirationState.Text)
            {
                case "High":
                    return WatchdogDOExpirationState.High;
                case "Low":
                    return WatchdogDOExpirationState.Low;
                case "Tristate":
                    return WatchdogDOExpirationState.Tristate;
                case "No Change":
                    return WatchdogDOExpirationState.NoChange;
            }
            throw new System.Exception("Unexpected value of watchdogExpiration field");
        }

        private void startButton_Click(object sender, System.EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                // Configure and start the output task
                outputTask = new Task();
                outputTask.DOChannels.CreateChannel(
                    physicalChannelComboBox.Text,
                    "myChannel",
                    ChannelLineGrouping.OneChannelForAllLines);
                outputTask.Start();

                // Configure and start the watchdog task
                watchdogTask = DaqSystem.Local.CreateWatchdogTimerTask(
                    "watchdogTask",
                    deviceComboBox.Text,
                    System.Double.Parse(timeout.Text),
                    new string[] { watchdogPhysicalChannelComboBox.Text },
                    new WatchdogDOExpirationState[] { GetWatchdogExpirationState() });
                watchdogTask.Start();

                writer = new DigitalSingleChannelWriter(outputTask.Stream);
                runningTask = outputTask;
                writer.BeginWriteSingleSampleMultiLine(true,
                    GetDataAsBoolArray(),
                    new AsyncCallback(OnDataWritten),
                    outputTask);
                
                DimControls(true);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                if(watchdogTask != null)
                {
                    watchdogTask.Dispose();
                    watchdogTask = null;
                }
                if(outputTask != null)
                {
                    outputTask.Dispose();
                    outputTask = null;
                }
            }
            Cursor.Current = Cursors.Default;
        }

        private void OnDataWritten(IAsyncResult result)
        {
            try
            {
                if (runningTask != null && runningTask == result.AsyncState)
                {
                    writer.EndWrite(result);
                    watchdogTask.Watchdog.ResetTimer();
                    System.Threading.Thread.Sleep((int)(writeInterval.Value * 1000));

                    writer.BeginWriteSingleSampleMultiLine(
                        true,
                        GetDataAsBoolArray(),
                        new AsyncCallback(OnDataWritten),
                        outputTask);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void stopButton_Click(object sender, System.EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                runningTask = null;
                outputTask.Stop();
                outputTask.Dispose();
                outputTask = null;
                watchdogTask.Stop();
                watchdogTask.Dispose();
                watchdogTask = null;
			}
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Cursor.Current = Cursors.Default;
            DimControls(false);
        }

        private void MainForm_Load(object sender, System.EventArgs e)
        {
            watchdogExpirationState.Text = "High";
        }
	}
}
