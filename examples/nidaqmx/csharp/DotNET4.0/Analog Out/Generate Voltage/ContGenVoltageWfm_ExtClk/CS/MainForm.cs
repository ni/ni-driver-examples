/******************************************************************************
*
* Example program:
*   ContGenVoltageWfm_ExtClk
*
* Category:
*   AO
*
* Description:
*   This example demonstrates how to continuously output a periodic waveform
*   using an external clock.
*
* Instructions for running:
*   1.  Select the physical channel corresponding to where your signal is output
*       on the DAQ device.
*   2.  Enter the minimum and maximum voltage values.
*   3.  Specify the external sample clock source.
*   4.  Select the desired waveform type.
*   5.  The rest of the parameters in the Function Generator Parameters section
*       will affect the way the waveform is created, before it's sent to the
*       analog output of the board. Select the amplitude, number of samples per
*       buffer, and the number of cycles per buffer to be used as waveform data.
*
* Steps:
*   1.  Create a new task and an analog output voltage channel.
*   2.  Specify the external clock source, and define the sample mode for
*       continuous samples.
*   3.  Create a AnalogSingleChannelWriter and call the WriteMultiSample method
*       to write the waveform data to a buffer.
*   4.  Call Task.Start().
*   5.  When the user presses the stop button, stop the task.
*   6.  Dispose the Task object to clean-up any resources associated with the
*       task.
*   7.  Handle any DaqExceptions, if they occur.
*
* I/O Connections Overview:
*   Make sure your signal output terminal matches the text in physical channel
*   textbox. In this case the signal will output to the ao0 pin on your DAQ
*   Device. Wire the external sample clock to a PFI or RTSI pin of your choice
*   on the board. Specify the same terminal name as the argument to the
*   ConfigureSampleClock method. (PFI0 is used in this example).  For more
*   information on the input and output terminals for your device, open the
*   NI-DAQmx Help, and refer to the NI-DAQmx Device Terminals and Device
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

namespace NationalInstruments.Examples.ContGenVoltageWfm_ExtClk
{
    /// <summary>
    /// Summary description for MainForm.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Label frequencyLabel;
        private System.Windows.Forms.TextBox clockSourceTextBox;
        private System.Windows.Forms.Label clockSourceLabel;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.GroupBox channelParametersGroupBox;
        private System.Windows.Forms.Label maximumValueLabel;
        private System.Windows.Forms.Label minimumValueLabel;
        private System.Windows.Forms.Label physicalChannelLabel;
        private System.Windows.Forms.GroupBox timingParametersGroupBox;
        private System.Windows.Forms.GroupBox functionGeneratorGroupBox;
        internal System.Windows.Forms.Label amplitudeLabel;
        internal System.Windows.Forms.NumericUpDown amplitudeNumeric;
        private System.Windows.Forms.NumericUpDown samplesPerBufferNumeric;
        private System.Windows.Forms.Label cyclesPerBufferLabel;
        private System.Windows.Forms.NumericUpDown cyclesPerBufferNumeric;
        private System.Windows.Forms.Label signalTypeLabel;
        private System.Windows.Forms.ComboBox signalTypeComboBox;
        internal System.Windows.Forms.NumericUpDown frequencyNumeric;
        private System.Windows.Forms.NumericUpDown minimumNumeric;
        private System.Windows.Forms.NumericUpDown maximumNumeric;
        private System.Windows.Forms.Label samplesPerBufferLabel;
        private System.Windows.Forms.ComboBox physicalChannelComboBox;

        private Task myTask;
    
        public MainForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            FunctionGenerator.InitComboBox(signalTypeComboBox);
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
                if (myTask != null) 
                {
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainForm));
            this.startButton = new System.Windows.Forms.Button();
            this.timingParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.frequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.frequencyLabel = new System.Windows.Forms.Label();
            this.clockSourceTextBox = new System.Windows.Forms.TextBox();
            this.clockSourceLabel = new System.Windows.Forms.Label();
            this.stopButton = new System.Windows.Forms.Button();
            this.channelParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.physicalChannelComboBox = new System.Windows.Forms.ComboBox();
            this.minimumNumeric = new System.Windows.Forms.NumericUpDown();
            this.maximumValueLabel = new System.Windows.Forms.Label();
            this.minimumValueLabel = new System.Windows.Forms.Label();
            this.physicalChannelLabel = new System.Windows.Forms.Label();
            this.maximumNumeric = new System.Windows.Forms.NumericUpDown();
            this.functionGeneratorGroupBox = new System.Windows.Forms.GroupBox();
            this.amplitudeLabel = new System.Windows.Forms.Label();
            this.amplitudeNumeric = new System.Windows.Forms.NumericUpDown();
            this.samplesPerBufferNumeric = new System.Windows.Forms.NumericUpDown();
            this.cyclesPerBufferLabel = new System.Windows.Forms.Label();
            this.cyclesPerBufferNumeric = new System.Windows.Forms.NumericUpDown();
            this.signalTypeLabel = new System.Windows.Forms.Label();
            this.signalTypeComboBox = new System.Windows.Forms.ComboBox();
            this.samplesPerBufferLabel = new System.Windows.Forms.Label();
            this.timingParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.frequencyNumeric)).BeginInit();
            this.channelParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.minimumNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maximumNumeric)).BeginInit();
            this.functionGeneratorGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.amplitudeNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerBufferNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cyclesPerBufferNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // startButton
            // 
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.startButton.Location = new System.Drawing.Point(347, 192);
            this.startButton.Name = "startButton";
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // timingParametersGroupBox
            // 
            this.timingParametersGroupBox.Controls.Add(this.frequencyNumeric);
            this.timingParametersGroupBox.Controls.Add(this.frequencyLabel);
            this.timingParametersGroupBox.Controls.Add(this.clockSourceTextBox);
            this.timingParametersGroupBox.Controls.Add(this.clockSourceLabel);
            this.timingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.timingParametersGroupBox.Location = new System.Drawing.Point(8, 152);
            this.timingParametersGroupBox.Name = "timingParametersGroupBox";
            this.timingParametersGroupBox.Size = new System.Drawing.Size(256, 96);
            this.timingParametersGroupBox.TabIndex = 3;
            this.timingParametersGroupBox.TabStop = false;
            this.timingParametersGroupBox.Text = "Timing Parameters";
            // 
            // frequencyNumeric
            // 
            this.frequencyNumeric.Location = new System.Drawing.Point(152, 56);
            this.frequencyNumeric.Maximum = new System.Decimal(new int[] {
                                                                             100000,
                                                                             0,
                                                                             0,
                                                                             0});
            this.frequencyNumeric.Minimum = new System.Decimal(new int[] {
                                                                             1,
                                                                             0,
                                                                             0,
                                                                             0});
            this.frequencyNumeric.Name = "frequencyNumeric";
            this.frequencyNumeric.Size = new System.Drawing.Size(96, 20);
            this.frequencyNumeric.TabIndex = 1;
            this.frequencyNumeric.Value = new System.Decimal(new int[] {
                                                                           100,
                                                                           0,
                                                                           0,
                                                                           0});
            // 
            // frequencyLabel
            // 
            this.frequencyLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.frequencyLabel.Location = new System.Drawing.Point(16, 59);
            this.frequencyLabel.Name = "frequencyLabel";
            this.frequencyLabel.Size = new System.Drawing.Size(128, 14);
            this.frequencyLabel.TabIndex = 20;
            this.frequencyLabel.Text = "Desired Frequency (Hz):";
            // 
            // clockSourceTextBox
            // 
            this.clockSourceTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.clockSourceTextBox.Location = new System.Drawing.Point(152, 24);
            this.clockSourceTextBox.Name = "clockSourceTextBox";
            this.clockSourceTextBox.Size = new System.Drawing.Size(96, 20);
            this.clockSourceTextBox.TabIndex = 0;
            this.clockSourceTextBox.Text = "/Dev1/PFI0";
            // 
            // clockSourceLabel
            // 
            this.clockSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.clockSourceLabel.Location = new System.Drawing.Point(16, 27);
            this.clockSourceLabel.Name = "clockSourceLabel";
            this.clockSourceLabel.Size = new System.Drawing.Size(88, 14);
            this.clockSourceLabel.TabIndex = 16;
            this.clockSourceLabel.Text = "Clock Source:";
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.stopButton.Location = new System.Drawing.Point(347, 225);
            this.stopButton.Name = "stopButton";
            this.stopButton.TabIndex = 1;
            this.stopButton.Text = "Stop";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // channelParametersGroupBox
            // 
            this.channelParametersGroupBox.Controls.Add(this.physicalChannelComboBox);
            this.channelParametersGroupBox.Controls.Add(this.minimumNumeric);
            this.channelParametersGroupBox.Controls.Add(this.maximumValueLabel);
            this.channelParametersGroupBox.Controls.Add(this.minimumValueLabel);
            this.channelParametersGroupBox.Controls.Add(this.physicalChannelLabel);
            this.channelParametersGroupBox.Controls.Add(this.maximumNumeric);
            this.channelParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.channelParametersGroupBox.Location = new System.Drawing.Point(8, 8);
            this.channelParametersGroupBox.Name = "channelParametersGroupBox";
            this.channelParametersGroupBox.Size = new System.Drawing.Size(256, 136);
            this.channelParametersGroupBox.TabIndex = 2;
            this.channelParametersGroupBox.TabStop = false;
            this.channelParametersGroupBox.Text = "Channel Parameters";
            // 
            // physicalChannelComboBox
            // 
            this.physicalChannelComboBox.Location = new System.Drawing.Point(152, 24);
            this.physicalChannelComboBox.Name = "physicalChannelComboBox";
            this.physicalChannelComboBox.Size = new System.Drawing.Size(96, 21);
            this.physicalChannelComboBox.TabIndex = 1;
            this.physicalChannelComboBox.Text = "Dev1/ao0";
            // 
            // minimumNumeric
            // 
            this.minimumNumeric.Location = new System.Drawing.Point(152, 59);
            this.minimumNumeric.Maximum = new System.Decimal(new int[] {
                                                                           10,
                                                                           0,
                                                                           0,
                                                                           0});
            this.minimumNumeric.Minimum = new System.Decimal(new int[] {
                                                                           10,
                                                                           0,
                                                                           0,
                                                                           -2147483648});
            this.minimumNumeric.Name = "minimumNumeric";
            this.minimumNumeric.Size = new System.Drawing.Size(96, 20);
            this.minimumNumeric.TabIndex = 3;
            this.minimumNumeric.Value = new System.Decimal(new int[] {
                                                                         10,
                                                                         0,
                                                                         0,
                                                                         -2147483648});
            // 
            // maximumValueLabel
            // 
            this.maximumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.maximumValueLabel.Location = new System.Drawing.Point(16, 99);
            this.maximumValueLabel.Name = "maximumValueLabel";
            this.maximumValueLabel.Size = new System.Drawing.Size(111, 14);
            this.maximumValueLabel.TabIndex = 4;
            this.maximumValueLabel.Text = "Maximum Value (V):";
            // 
            // minimumValueLabel
            // 
            this.minimumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.minimumValueLabel.Location = new System.Drawing.Point(16, 62);
            this.minimumValueLabel.Name = "minimumValueLabel";
            this.minimumValueLabel.Size = new System.Drawing.Size(104, 14);
            this.minimumValueLabel.TabIndex = 2;
            this.minimumValueLabel.Text = "Minimum Value (V):";
            // 
            // physicalChannelLabel
            // 
            this.physicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.physicalChannelLabel.Location = new System.Drawing.Point(16, 27);
            this.physicalChannelLabel.Name = "physicalChannelLabel";
            this.physicalChannelLabel.Size = new System.Drawing.Size(95, 14);
            this.physicalChannelLabel.TabIndex = 0;
            this.physicalChannelLabel.Text = "Physical Channel:";
            // 
            // maximumNumeric
            // 
            this.maximumNumeric.Location = new System.Drawing.Point(152, 96);
            this.maximumNumeric.Maximum = new System.Decimal(new int[] {
                                                                           10,
                                                                           0,
                                                                           0,
                                                                           0});
            this.maximumNumeric.Minimum = new System.Decimal(new int[] {
                                                                           10,
                                                                           0,
                                                                           0,
                                                                           -2147483648});
            this.maximumNumeric.Name = "maximumNumeric";
            this.maximumNumeric.Size = new System.Drawing.Size(96, 20);
            this.maximumNumeric.TabIndex = 5;
            this.maximumNumeric.Value = new System.Decimal(new int[] {
                                                                         10,
                                                                         0,
                                                                         0,
                                                                         0});
            // 
            // functionGeneratorGroupBox
            // 
            this.functionGeneratorGroupBox.Controls.Add(this.amplitudeLabel);
            this.functionGeneratorGroupBox.Controls.Add(this.amplitudeNumeric);
            this.functionGeneratorGroupBox.Controls.Add(this.samplesPerBufferNumeric);
            this.functionGeneratorGroupBox.Controls.Add(this.cyclesPerBufferLabel);
            this.functionGeneratorGroupBox.Controls.Add(this.cyclesPerBufferNumeric);
            this.functionGeneratorGroupBox.Controls.Add(this.signalTypeLabel);
            this.functionGeneratorGroupBox.Controls.Add(this.signalTypeComboBox);
            this.functionGeneratorGroupBox.Controls.Add(this.samplesPerBufferLabel);
            this.functionGeneratorGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.functionGeneratorGroupBox.Location = new System.Drawing.Point(272, 8);
            this.functionGeneratorGroupBox.Name = "functionGeneratorGroupBox";
            this.functionGeneratorGroupBox.Size = new System.Drawing.Size(224, 168);
            this.functionGeneratorGroupBox.TabIndex = 4;
            this.functionGeneratorGroupBox.TabStop = false;
            this.functionGeneratorGroupBox.Text = "Function Generator Parameters";
            // 
            // amplitudeLabel
            // 
            this.amplitudeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.amplitudeLabel.Location = new System.Drawing.Point(16, 138);
            this.amplitudeLabel.Name = "amplitudeLabel";
            this.amplitudeLabel.Size = new System.Drawing.Size(64, 16);
            this.amplitudeLabel.TabIndex = 42;
            this.amplitudeLabel.Text = "Amplitude:";
            // 
            // amplitudeNumeric
            // 
            this.amplitudeNumeric.DecimalPlaces = 1;
            this.amplitudeNumeric.Increment = new System.Decimal(new int[] {
                                                                               1,
                                                                               0,
                                                                               0,
                                                                               65536});
            this.amplitudeNumeric.Location = new System.Drawing.Point(120, 136);
            this.amplitudeNumeric.Minimum = new System.Decimal(new int[] {
                                                                             1,
                                                                             0,
                                                                             0,
                                                                             0});
            this.amplitudeNumeric.Name = "amplitudeNumeric";
            this.amplitudeNumeric.Size = new System.Drawing.Size(96, 20);
            this.amplitudeNumeric.TabIndex = 3;
            this.amplitudeNumeric.Value = new System.Decimal(new int[] {
                                                                           2,
                                                                           0,
                                                                           0,
                                                                           0});
            // 
            // samplesPerBufferNumeric
            // 
            this.samplesPerBufferNumeric.Location = new System.Drawing.Point(120, 96);
            this.samplesPerBufferNumeric.Maximum = new System.Decimal(new int[] {
                                                                                    1000000,
                                                                                    0,
                                                                                    0,
                                                                                    0});
            this.samplesPerBufferNumeric.Minimum = new System.Decimal(new int[] {
                                                                                    1,
                                                                                    0,
                                                                                    0,
                                                                                    0});
            this.samplesPerBufferNumeric.Name = "samplesPerBufferNumeric";
            this.samplesPerBufferNumeric.Size = new System.Drawing.Size(96, 20);
            this.samplesPerBufferNumeric.TabIndex = 2;
            this.samplesPerBufferNumeric.Value = new System.Decimal(new int[] {
                                                                                  250,
                                                                                  0,
                                                                                  0,
                                                                                  0});
            // 
            // cyclesPerBufferLabel
            // 
            this.cyclesPerBufferLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cyclesPerBufferLabel.Location = new System.Drawing.Point(16, 61);
            this.cyclesPerBufferLabel.Name = "cyclesPerBufferLabel";
            this.cyclesPerBufferLabel.Size = new System.Drawing.Size(103, 16);
            this.cyclesPerBufferLabel.TabIndex = 34;
            this.cyclesPerBufferLabel.Text = "Cycles Per Buffer:";
            // 
            // cyclesPerBufferNumeric
            // 
            this.cyclesPerBufferNumeric.Location = new System.Drawing.Point(120, 59);
            this.cyclesPerBufferNumeric.Minimum = new System.Decimal(new int[] {
                                                                                   1,
                                                                                   0,
                                                                                   0,
                                                                                   0});
            this.cyclesPerBufferNumeric.Name = "cyclesPerBufferNumeric";
            this.cyclesPerBufferNumeric.Size = new System.Drawing.Size(96, 20);
            this.cyclesPerBufferNumeric.TabIndex = 1;
            this.cyclesPerBufferNumeric.Value = new System.Decimal(new int[] {
                                                                                 5,
                                                                                 0,
                                                                                 0,
                                                                                 0});
            // 
            // signalTypeLabel
            // 
            this.signalTypeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.signalTypeLabel.Location = new System.Drawing.Point(16, 26);
            this.signalTypeLabel.Name = "signalTypeLabel";
            this.signalTypeLabel.Size = new System.Drawing.Size(87, 16);
            this.signalTypeLabel.TabIndex = 31;
            this.signalTypeLabel.Text = "Waveform Type:";
            // 
            // signalTypeComboBox
            // 
            this.signalTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.signalTypeComboBox.ItemHeight = 13;
            this.signalTypeComboBox.Items.AddRange(new object[] {
                                                                    ""});
            this.signalTypeComboBox.Location = new System.Drawing.Point(121, 24);
            this.signalTypeComboBox.Name = "signalTypeComboBox";
            this.signalTypeComboBox.Size = new System.Drawing.Size(96, 21);
            this.signalTypeComboBox.TabIndex = 0;
            // 
            // samplesPerBufferLabel
            // 
            this.samplesPerBufferLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.samplesPerBufferLabel.Location = new System.Drawing.Point(16, 98);
            this.samplesPerBufferLabel.Name = "samplesPerBufferLabel";
            this.samplesPerBufferLabel.Size = new System.Drawing.Size(112, 16);
            this.samplesPerBufferLabel.TabIndex = 35;
            this.samplesPerBufferLabel.Text = "Samples Per Buffer:";
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(522, 257);
            this.Controls.Add(this.functionGeneratorGroupBox);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.timingParametersGroupBox);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.channelParametersGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(528, 289);
            this.MinimumSize = new System.Drawing.Size(528, 289);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Continuous Voltage Generation - External Clock";
            this.timingParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.frequencyNumeric)).EndInit();
            this.channelParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.minimumNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maximumNumeric)).EndInit();
            this.functionGeneratorGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.amplitudeNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerBufferNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cyclesPerBufferNumeric)).EndInit();
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
                // Create the task and channel
                myTask = new Task();
                myTask.AOChannels.CreateVoltageChannel(physicalChannelComboBox.Text,
                    "",
                    Convert.ToDouble(minimumNumeric.Value),
                    Convert.ToDouble(maximumNumeric.Value),
                    AOVoltageUnits.Volts);

                // Verify the task before doing the waveform calculations
                myTask.Control(TaskAction.Verify);

                // Calculate some waveform parameters and generate data
                FunctionGenerator fGen = new FunctionGenerator(
                    myTask.Timing,
                    frequencyNumeric.Value.ToString(),
                    samplesPerBufferNumeric.Value.ToString(),
                    cyclesPerBufferNumeric.Value.ToString(),
                    signalTypeComboBox.Text,
                    amplitudeNumeric.Value.ToString());
                
                // Configure the sample clock with the calculated rate and 
                // specified clock source
                myTask.Timing.ConfigureSampleClock(clockSourceTextBox.Text, 
                    fGen.ResultingSampleClockRate, 
                    SampleClockActiveEdge.Rising, 
                    SampleQuantityMode.ContinuousSamples, 1000);

                // Set up the Task Done event
                myTask.Done += new TaskDoneEventHandler(myTask_Done);

                // Write data to buffer
                AnalogSingleChannelWriter writer = 
                    new AnalogSingleChannelWriter(myTask.Stream);

                writer.WriteMultiSample(false, fGen.Data);

                // Start writing data
                myTask.Start(); 

                // Update UI
                startButton.Enabled = false;
                stopButton.Enabled = true;
                channelParametersGroupBox.Enabled = false;
                timingParametersGroupBox.Enabled = false;
                functionGeneratorGroupBox.Enabled = false;

            }
            catch (DaqException exception)
            {
                myTask.Dispose();
                MessageBox.Show(exception.Message);

                startButton.Enabled = true;
                stopButton.Enabled = false;
                channelParametersGroupBox.Enabled = true;
                timingParametersGroupBox.Enabled = true;
                functionGeneratorGroupBox.Enabled = true;
            }

            Cursor.Current = Cursors.Default;
        }

        private void stopButton_Click(object sender, System.EventArgs e)
        {
            if (myTask != null)
            {
				myTask.Stop();
                myTask.Dispose();
            }

            startButton.Enabled = true;
            stopButton.Enabled = false;
            channelParametersGroupBox.Enabled = true;
            timingParametersGroupBox.Enabled = true;
            functionGeneratorGroupBox.Enabled = true;
        }

        private void myTask_Done(object sender, TaskDoneEventArgs e)
        {
			if (e.Error != null)
			{
				MessageBox.Show(e.Error.Message);
			}

			if (myTask != null)
			{
				myTask.Dispose();
			}

			startButton.Enabled = true;
			stopButton.Enabled = false;
			channelParametersGroupBox.Enabled = true;
			timingParametersGroupBox.Enabled = true;
			functionGeneratorGroupBox.Enabled = true;
        }
    }
}
