/******************************************************************************
*
* Example program:
*   GenMultVoltUpdates_SWTimed
*
* Category:
*   AO
*
* Description:
*   This example demonstrates how to output multiple voltage updates (samples)
*   to an analog output channel in a software timed loop.
*
* Instructions for running:
*   1.  Select the physical channel corresponding to where your signal is output
*       on the DAQ device.
*   2.  Enter the minimum and maximum voltage values.
*   3.  Set the loop rate.  Note that the resolution of the timer is
*       approximately 10 ms.
*
* Steps:
*   1.  Create a new task and an analog output voltage channel.
*   2.  Enable the timer.
*   3.  Inside the timer event handler, create a AnalogSingleChannelWrite and
*       call the WriteSingleSample method to write one sine wave value to the
*       channel at a time.
*   4.  When the user hits the stop button, disable the timer and stop the task.
*   5.  Dispose the Task object to clean-up any resources associated with the
*       task.
*   6.  Handle any DaqExceptions, if they occur.
*
* I/O Connections Overview:
*   Make sure your signal output terminal matches the text in the physical
*   channel text box. In this case the signal will output to the ao0 pin on your
*   DAQ Device.  For more information on the input and output terminals for your
*   device, open the NI-DAQmx Help, and refer to the NI-DAQmx Device Terminals
*   and Device Considerations books in the table of contents.
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
using System.Threading;
using NationalInstruments.DAQmx;

namespace NationalInstruments.Examples.GenMultVoltUpdates_SWTimed
{
    /// <summary>
    /// Summary description for MainForm.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.TextBox maximumValueTextBox;
        private System.Windows.Forms.TextBox minimumValueTextBox;
        private System.Windows.Forms.Label maximumValueLabel;
        private System.Windows.Forms.Label minimumValueLabel;
        private System.Windows.Forms.Label physicalChannelLabel;
        private System.Windows.Forms.GroupBox channelParametersGroupBox;
        private System.Windows.Forms.NumericUpDown rateNumericUpDown;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.Timer timer;
        private AnalogSingleChannelWriter writer;
        private Task myTask;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.GroupBox timingParametersGroupBox;
        private System.Windows.Forms.Label rateLabel;
        private System.Windows.Forms.ComboBox physicalChannelComboBox;
        private int counter = 0;

        public MainForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            physicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AO, PhysicalChannelAccess.External));
            if (physicalChannelComboBox.Items.Count > 0)
                physicalChannelComboBox.SelectedIndex = 0;
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
            this.startButton = new System.Windows.Forms.Button();
            this.channelParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.physicalChannelComboBox = new System.Windows.Forms.ComboBox();
            this.maximumValueTextBox = new System.Windows.Forms.TextBox();
            this.minimumValueTextBox = new System.Windows.Forms.TextBox();
            this.maximumValueLabel = new System.Windows.Forms.Label();
            this.minimumValueLabel = new System.Windows.Forms.Label();
            this.physicalChannelLabel = new System.Windows.Forms.Label();
            this.rateNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.timingParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.rateLabel = new System.Windows.Forms.Label();
            this.stopButton = new System.Windows.Forms.Button();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.channelParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rateNumericUpDown)).BeginInit();
            this.timingParametersGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // startButton
            // 
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.startButton.Location = new System.Drawing.Point(40, 224);
            this.startButton.Name = "startButton";
            this.startButton.TabIndex = 0;
            this.startButton.Text = "&Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // channelParametersGroupBox
            // 
            this.channelParametersGroupBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.channelParametersGroupBox.Controls.Add(this.physicalChannelComboBox);
            this.channelParametersGroupBox.Controls.Add(this.maximumValueTextBox);
            this.channelParametersGroupBox.Controls.Add(this.minimumValueTextBox);
            this.channelParametersGroupBox.Controls.Add(this.maximumValueLabel);
            this.channelParametersGroupBox.Controls.Add(this.minimumValueLabel);
            this.channelParametersGroupBox.Controls.Add(this.physicalChannelLabel);
            this.channelParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.channelParametersGroupBox.Location = new System.Drawing.Point(8, 8);
            this.channelParametersGroupBox.Name = "channelParametersGroupBox";
            this.channelParametersGroupBox.Size = new System.Drawing.Size(336, 128);
            this.channelParametersGroupBox.TabIndex = 2;
            this.channelParametersGroupBox.TabStop = false;
            this.channelParametersGroupBox.Text = "Channel Parameters";
            // 
            // physicalChannelComboBox
            // 
            this.physicalChannelComboBox.Location = new System.Drawing.Point(152, 24);
            this.physicalChannelComboBox.Name = "physicalChannelComboBox";
            this.physicalChannelComboBox.Size = new System.Drawing.Size(176, 21);
            this.physicalChannelComboBox.TabIndex = 1;
            this.physicalChannelComboBox.Text = "Dev1/ao0";
            // 
            // maximumValueTextBox
            // 
            this.maximumValueTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.maximumValueTextBox.Location = new System.Drawing.Point(152, 96);
            this.maximumValueTextBox.Name = "maximumValueTextBox";
            this.maximumValueTextBox.Size = new System.Drawing.Size(176, 20);
            this.maximumValueTextBox.TabIndex = 5;
            this.maximumValueTextBox.Text = "10";
            // 
            // minimumValueTextBox
            // 
            this.minimumValueTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.minimumValueTextBox.Location = new System.Drawing.Point(152, 60);
            this.minimumValueTextBox.Name = "minimumValueTextBox";
            this.minimumValueTextBox.Size = new System.Drawing.Size(176, 20);
            this.minimumValueTextBox.TabIndex = 3;
            this.minimumValueTextBox.Text = "-10";
            // 
            // maximumValueLabel
            // 
            this.maximumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.maximumValueLabel.Location = new System.Drawing.Point(16, 96);
            this.maximumValueLabel.Name = "maximumValueLabel";
            this.maximumValueLabel.Size = new System.Drawing.Size(112, 16);
            this.maximumValueLabel.TabIndex = 4;
            this.maximumValueLabel.Text = "Maximum Value (V):";
            // 
            // minimumValueLabel
            // 
            this.minimumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.minimumValueLabel.Location = new System.Drawing.Point(16, 62);
            this.minimumValueLabel.Name = "minimumValueLabel";
            this.minimumValueLabel.Size = new System.Drawing.Size(104, 16);
            this.minimumValueLabel.TabIndex = 2;
            this.minimumValueLabel.Text = "Minimum Value (V):";
            // 
            // physicalChannelLabel
            // 
            this.physicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.physicalChannelLabel.Location = new System.Drawing.Point(16, 26);
            this.physicalChannelLabel.Name = "physicalChannelLabel";
            this.physicalChannelLabel.Size = new System.Drawing.Size(96, 16);
            this.physicalChannelLabel.TabIndex = 0;
            this.physicalChannelLabel.Text = "Physical Channel:";
            // 
            // rateNumericUpDown
            // 
            this.rateNumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.rateNumericUpDown.Location = new System.Drawing.Point(152, 24);
            this.rateNumericUpDown.Maximum = new System.Decimal(new int[] {
                                                                              1000000,
                                                                              0,
                                                                              0,
                                                                              0});
            this.rateNumericUpDown.Minimum = new System.Decimal(new int[] {
                                                                              10,
                                                                              0,
                                                                              0,
                                                                              0});
            this.rateNumericUpDown.Name = "rateNumericUpDown";
            this.rateNumericUpDown.Size = new System.Drawing.Size(176, 20);
            this.rateNumericUpDown.TabIndex = 1;
            this.rateNumericUpDown.Value = new System.Decimal(new int[] {
                                                                            10,
                                                                            0,
                                                                            0,
                                                                            0});
            this.rateNumericUpDown.ValueChanged += new System.EventHandler(this.rateNumericUpDown_ValueChanged);
            // 
            // timingParametersGroupBox
            // 
            this.timingParametersGroupBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.timingParametersGroupBox.Controls.Add(this.rateLabel);
            this.timingParametersGroupBox.Controls.Add(this.rateNumericUpDown);
            this.timingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.timingParametersGroupBox.Location = new System.Drawing.Point(8, 152);
            this.timingParametersGroupBox.Name = "timingParametersGroupBox";
            this.timingParametersGroupBox.Size = new System.Drawing.Size(336, 56);
            this.timingParametersGroupBox.TabIndex = 3;
            this.timingParametersGroupBox.TabStop = false;
            this.timingParametersGroupBox.Text = "Timing Parameters";
            // 
            // rateLabel
            // 
            this.rateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rateLabel.Location = new System.Drawing.Point(16, 24);
            this.rateLabel.Name = "rateLabel";
            this.rateLabel.Size = new System.Drawing.Size(136, 16);
            this.rateLabel.TabIndex = 0;
            this.rateLabel.Text = "Software Loop Time (ms):";
            // 
            // stopButton
            // 
            this.stopButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.stopButton.Enabled = false;
            this.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.stopButton.Location = new System.Drawing.Point(240, 224);
            this.stopButton.Name = "stopButton";
            this.stopButton.TabIndex = 1;
            this.stopButton.Text = "S&top";
            this.stopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(352, 257);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.timingParametersGroupBox);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.channelParametersGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(0, 10);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(600, 289);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(256, 289);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Generate Multiple Voltage Updates - SW Timed";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.OnClosing);
            this.channelParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rateNumericUpDown)).EndInit();
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
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                myTask = new Task();
                myTask.AOChannels.CreateVoltageChannel(physicalChannelComboBox.Text, "aoChannel",
                    Convert.ToDouble(minimumValueTextBox.Text), Convert.ToDouble(maximumValueTextBox.Text),
                    AOVoltageUnits.Volts);
                writer = new AnalogSingleChannelWriter(myTask.Stream);
                timer.Interval = Convert.ToInt32(rateNumericUpDown.Value);
                timer.Enabled = true;
                startButton.Enabled = false;
                stopButton.Enabled = true;
            }
            catch(DaqException ex)
            {
                MessageBox.Show(ex.Message);
                myTask.Dispose();
                myTask = null;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void StopButton_Click(object sender, System.EventArgs e)
        {
            try
            {
                timer.Enabled = false;
                myTask.Dispose();
                myTask = null;
                startButton.Enabled = true;
                stopButton.Enabled = false;
            }
            catch (DaqException ex)
            {
                MessageBox.Show(ex.Message);                
            }       
        }

        private void timer_Tick(object sender, System.EventArgs e)
        {
            if(myTask == null) return;
            try
            {
                double data = Math.Sin(Math.PI / 180.0 * 0.001 * 360 * ((counter++)%1000)); // Calculate sine wave (-1V to 1 V)                     
                writer.WriteSingleSample(true, data);
            }
            catch (DaqException ex)
            {
                timer.Enabled = false;
                MessageBox.Show(ex.Message);
                myTask.Dispose();
                myTask = null;
                startButton.Enabled = true;
                stopButton.Enabled = false;
            }
        }

        private void rateNumericUpDown_ValueChanged(object sender, System.EventArgs e)
        {
           timer.Interval = Convert.ToInt32(rateNumericUpDown.Value);
        }

        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(myTask != null)
                StopButton_Click(null,null);
        }
    }
}
