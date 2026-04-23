/******************************************************************************
*
* Example program:
*   GenMultCurrentUpdates_IntClk
*
* Category:
*   AO
*
* Description:
*   This example demonstrates how to output a finite number of current samples
*   to an Analog Output Channel using an internal sample clock.
*
* Instructions for running:
*   1.  Select the physical channel corresponding to where your signal is
*       outputon the DAQ device.
*   2.  Enter the minimum and maximum voltage values.
*   3.  Set the signal frequency of the waveform to write.
*   4.  Set the function generator parameters, including the waveform
*       type,cycles per buffer, samples per buffer, and offset.
*
* Steps:
*   1.  Create a new task and an analog output voltage channel.
*   2.  Use the FunctionGenerator class to generate an array of data
*       pointsrepresenting a waveform.
*   3.  Set up the timing for the generation. This example uses the onboardclock
*       to write a finite number of samples.
*   4.  Create a AnalogSingleChannelWriter, add the Task Done event handler,and
*       call the WriteMultiSample method to write multiple samples to a single
*       channel on your DAQ device. The autoStart parameter is set to false, so
*       Task.Start() must be explicitly called to begin the voltage generation.
*   5.  Call Task.Start().
*   6.  Call Task.WaitUntilDone, with a time limit of 10s.
*   7.  Dispose the Task object to clean-up any resources associated with the
*       task.
*   8.  In the Task Done event check for any errors and Dispose the Task object
*       to clean-up any resources associated with the task.
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
using NationalInstruments;
using NationalInstruments.DAQmx;
using NationalInstruments.Examples;

namespace NationalInstruments.Examples.GenMultCurrentUpdates_IntClk
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.GroupBox TimingParametersGroupBox;
        private System.Windows.Forms.TextBox frequencyTextBox;
        private System.Windows.Forms.Label frequencyLabel;
        private System.Windows.Forms.GroupBox channelParametersGroupBox;
        private System.Windows.Forms.ComboBox physicalChannelComboBox;
        private System.Windows.Forms.TextBox maximumValueTextBox;
        private System.Windows.Forms.TextBox minimumValueTextBox;
        private System.Windows.Forms.Label maximumValueLabel;
        private System.Windows.Forms.Label minimumValueLabel;
        private System.Windows.Forms.Label physicalChannelLabel;
        private System.Windows.Forms.GroupBox functionGeneratorGroupBox;
        internal System.Windows.Forms.Label offsetLabel;
        internal System.Windows.Forms.NumericUpDown offsetNumericUpDown;
        internal System.Windows.Forms.Label amplitudeLabel;
        internal System.Windows.Forms.NumericUpDown amplitudeNumericUpDown;
        private System.Windows.Forms.NumericUpDown samplesPerBufferNumericUpDown;
        private System.Windows.Forms.Label samplesPerBufferLabel;
        private System.Windows.Forms.Label cyclesPerBufferLabel;
        private System.Windows.Forms.NumericUpDown cyclesPerBufferNumericUpDown;
        private System.Windows.Forms.Label waveformTypeLabel;
        private System.Windows.Forms.ComboBox signalTypeComboBox;
        private System.Windows.Forms.Button writeButton;
        private System.ComponentModel.IContainer components = null;

        private bool taskRunning = false;
        private Task myTask = null;

        public MainForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            writeButton.Enabled = false;
            FunctionGenerator.InitComboBox(signalTypeComboBox);

            physicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AO, PhysicalChannelAccess.External));
            if (physicalChannelComboBox.Items.Count > 0)
            {
                physicalChannelComboBox.SelectedIndex = 0;
                writeButton.Enabled = true;
            }
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.TimingParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.frequencyTextBox = new System.Windows.Forms.TextBox();
            this.frequencyLabel = new System.Windows.Forms.Label();
            this.channelParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.physicalChannelComboBox = new System.Windows.Forms.ComboBox();
            this.maximumValueTextBox = new System.Windows.Forms.TextBox();
            this.minimumValueTextBox = new System.Windows.Forms.TextBox();
            this.maximumValueLabel = new System.Windows.Forms.Label();
            this.minimumValueLabel = new System.Windows.Forms.Label();
            this.physicalChannelLabel = new System.Windows.Forms.Label();
            this.functionGeneratorGroupBox = new System.Windows.Forms.GroupBox();
            this.offsetLabel = new System.Windows.Forms.Label();
            this.offsetNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.amplitudeLabel = new System.Windows.Forms.Label();
            this.amplitudeNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.samplesPerBufferNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.samplesPerBufferLabel = new System.Windows.Forms.Label();
            this.cyclesPerBufferLabel = new System.Windows.Forms.Label();
            this.cyclesPerBufferNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.waveformTypeLabel = new System.Windows.Forms.Label();
            this.signalTypeComboBox = new System.Windows.Forms.ComboBox();
            this.writeButton = new System.Windows.Forms.Button();
            this.TimingParametersGroupBox.SuspendLayout();
            this.channelParametersGroupBox.SuspendLayout();
            this.functionGeneratorGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.offsetNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.amplitudeNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerBufferNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cyclesPerBufferNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // TimingParametersGroupBox
            // 
            this.TimingParametersGroupBox.Controls.Add(this.frequencyTextBox);
            this.TimingParametersGroupBox.Controls.Add(this.frequencyLabel);
            this.TimingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.TimingParametersGroupBox.Location = new System.Drawing.Point(8, 152);
            this.TimingParametersGroupBox.Name = "TimingParametersGroupBox";
            this.TimingParametersGroupBox.Size = new System.Drawing.Size(272, 64);
            this.TimingParametersGroupBox.TabIndex = 2;
            this.TimingParametersGroupBox.TabStop = false;
            this.TimingParametersGroupBox.Text = "Timing Parameters";
            // 
            // frequencyTextBox
            // 
            this.frequencyTextBox.Location = new System.Drawing.Point(136, 24);
            this.frequencyTextBox.Name = "frequencyTextBox";
            this.frequencyTextBox.Size = new System.Drawing.Size(120, 20);
            this.frequencyTextBox.TabIndex = 1;
            this.frequencyTextBox.Text = "1000";
            // 
            // frequencyLabel
            // 
            this.frequencyLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.frequencyLabel.Location = new System.Drawing.Point(16, 24);
            this.frequencyLabel.Name = "frequencyLabel";
            this.frequencyLabel.Size = new System.Drawing.Size(128, 24);
            this.frequencyLabel.TabIndex = 0;
            this.frequencyLabel.Text = "Signal Frequency (Hz):";
            // 
            // channelParametersGroupBox
            // 
            this.channelParametersGroupBox.Controls.Add(this.physicalChannelComboBox);
            this.channelParametersGroupBox.Controls.Add(this.maximumValueTextBox);
            this.channelParametersGroupBox.Controls.Add(this.minimumValueTextBox);
            this.channelParametersGroupBox.Controls.Add(this.maximumValueLabel);
            this.channelParametersGroupBox.Controls.Add(this.minimumValueLabel);
            this.channelParametersGroupBox.Controls.Add(this.physicalChannelLabel);
            this.channelParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.channelParametersGroupBox.Location = new System.Drawing.Point(8, 8);
            this.channelParametersGroupBox.Name = "channelParametersGroupBox";
            this.channelParametersGroupBox.Size = new System.Drawing.Size(272, 136);
            this.channelParametersGroupBox.TabIndex = 1;
            this.channelParametersGroupBox.TabStop = false;
            this.channelParametersGroupBox.Text = "Channel Parameters";
            // 
            // physicalChannelComboBox
            // 
            this.physicalChannelComboBox.Location = new System.Drawing.Point(136, 24);
            this.physicalChannelComboBox.Name = "physicalChannelComboBox";
            this.physicalChannelComboBox.Size = new System.Drawing.Size(121, 21);
            this.physicalChannelComboBox.TabIndex = 1;
            this.physicalChannelComboBox.Text = "Dev1/ao0";
            // 
            // maximumValueTextBox
            // 
            this.maximumValueTextBox.Location = new System.Drawing.Point(136, 96);
            this.maximumValueTextBox.Name = "maximumValueTextBox";
            this.maximumValueTextBox.Size = new System.Drawing.Size(120, 20);
            this.maximumValueTextBox.TabIndex = 5;
            this.maximumValueTextBox.Text = "0.02";
            // 
            // minimumValueTextBox
            // 
            this.minimumValueTextBox.Location = new System.Drawing.Point(136, 60);
            this.minimumValueTextBox.Name = "minimumValueTextBox";
            this.minimumValueTextBox.Size = new System.Drawing.Size(120, 20);
            this.minimumValueTextBox.TabIndex = 3;
            this.minimumValueTextBox.Text = "0";
            // 
            // maximumValueLabel
            // 
            this.maximumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.maximumValueLabel.Location = new System.Drawing.Point(16, 96);
            this.maximumValueLabel.Name = "maximumValueLabel";
            this.maximumValueLabel.Size = new System.Drawing.Size(112, 16);
            this.maximumValueLabel.TabIndex = 4;
            this.maximumValueLabel.Text = "Maximum Value (A):";
            // 
            // minimumValueLabel
            // 
            this.minimumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.minimumValueLabel.Location = new System.Drawing.Point(16, 62);
            this.minimumValueLabel.Name = "minimumValueLabel";
            this.minimumValueLabel.Size = new System.Drawing.Size(104, 16);
            this.minimumValueLabel.TabIndex = 2;
            this.minimumValueLabel.Text = "Minimum Value (A):";
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
            // functionGeneratorGroupBox
            // 
            this.functionGeneratorGroupBox.Controls.Add(this.offsetLabel);
            this.functionGeneratorGroupBox.Controls.Add(this.offsetNumericUpDown);
            this.functionGeneratorGroupBox.Controls.Add(this.amplitudeLabel);
            this.functionGeneratorGroupBox.Controls.Add(this.amplitudeNumericUpDown);
            this.functionGeneratorGroupBox.Controls.Add(this.samplesPerBufferNumericUpDown);
            this.functionGeneratorGroupBox.Controls.Add(this.samplesPerBufferLabel);
            this.functionGeneratorGroupBox.Controls.Add(this.cyclesPerBufferLabel);
            this.functionGeneratorGroupBox.Controls.Add(this.cyclesPerBufferNumericUpDown);
            this.functionGeneratorGroupBox.Controls.Add(this.waveformTypeLabel);
            this.functionGeneratorGroupBox.Controls.Add(this.signalTypeComboBox);
            this.functionGeneratorGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.functionGeneratorGroupBox.Location = new System.Drawing.Point(296, 8);
            this.functionGeneratorGroupBox.Name = "functionGeneratorGroupBox";
            this.functionGeneratorGroupBox.Size = new System.Drawing.Size(232, 208);
            this.functionGeneratorGroupBox.TabIndex = 3;
            this.functionGeneratorGroupBox.TabStop = false;
            this.functionGeneratorGroupBox.Text = "Function Generator Parameters";
            // 
            // offsetLabel
            // 
            this.offsetLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.offsetLabel.Location = new System.Drawing.Point(16, 168);
            this.offsetLabel.Name = "offsetLabel";
            this.offsetLabel.Size = new System.Drawing.Size(104, 23);
            this.offsetLabel.TabIndex = 8;
            this.offsetLabel.Text = "Offset:";
            // 
            // offsetNumericUpDown
            // 
            this.offsetNumericUpDown.DecimalPlaces = 3;
            this.offsetNumericUpDown.Increment = new System.Decimal(new int[] {
                                                                                  1,
                                                                                  0,
                                                                                  0,
                                                                                  196608});
            this.offsetNumericUpDown.Location = new System.Drawing.Point(120, 168);
            this.offsetNumericUpDown.Name = "offsetNumericUpDown";
            this.offsetNumericUpDown.Size = new System.Drawing.Size(104, 20);
            this.offsetNumericUpDown.TabIndex = 9;
            this.offsetNumericUpDown.Value = new System.Decimal(new int[] {
                                                                              1,
                                                                              0,
                                                                              0,
                                                                              131072});
            // 
            // amplitudeLabel
            // 
            this.amplitudeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.amplitudeLabel.Location = new System.Drawing.Point(16, 136);
            this.amplitudeLabel.Name = "amplitudeLabel";
            this.amplitudeLabel.Size = new System.Drawing.Size(104, 23);
            this.amplitudeLabel.TabIndex = 6;
            this.amplitudeLabel.Text = "Amplitude:";
            // 
            // amplitudeNumericUpDown
            // 
            this.amplitudeNumericUpDown.DecimalPlaces = 3;
            this.amplitudeNumericUpDown.Increment = new System.Decimal(new int[] {
                                                                                     1,
                                                                                     0,
                                                                                     0,
                                                                                     196608});
            this.amplitudeNumericUpDown.Location = new System.Drawing.Point(120, 136);
            this.amplitudeNumericUpDown.Name = "amplitudeNumericUpDown";
            this.amplitudeNumericUpDown.Size = new System.Drawing.Size(104, 20);
            this.amplitudeNumericUpDown.TabIndex = 7;
            this.amplitudeNumericUpDown.Value = new System.Decimal(new int[] {
                                                                                 1,
                                                                                 0,
                                                                                 0,
                                                                                 131072});
            // 
            // samplesPerBufferNumericUpDown
            // 
            this.samplesPerBufferNumericUpDown.Location = new System.Drawing.Point(120, 96);
            this.samplesPerBufferNumericUpDown.Maximum = new System.Decimal(new int[] {
                                                                                          10000000,
                                                                                          0,
                                                                                          0,
                                                                                          0});
            this.samplesPerBufferNumericUpDown.Minimum = new System.Decimal(new int[] {
                                                                                          1,
                                                                                          0,
                                                                                          0,
                                                                                          0});
            this.samplesPerBufferNumericUpDown.Name = "samplesPerBufferNumericUpDown";
            this.samplesPerBufferNumericUpDown.Size = new System.Drawing.Size(104, 20);
            this.samplesPerBufferNumericUpDown.TabIndex = 5;
            this.samplesPerBufferNumericUpDown.Value = new System.Decimal(new int[] {
                                                                                        250,
                                                                                        0,
                                                                                        0,
                                                                                        0});
            // 
            // samplesPerBufferLabel
            // 
            this.samplesPerBufferLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.samplesPerBufferLabel.Location = new System.Drawing.Point(16, 96);
            this.samplesPerBufferLabel.Name = "samplesPerBufferLabel";
            this.samplesPerBufferLabel.Size = new System.Drawing.Size(96, 32);
            this.samplesPerBufferLabel.TabIndex = 4;
            this.samplesPerBufferLabel.Text = "Samples Per Buffer:";
            // 
            // cyclesPerBufferLabel
            // 
            this.cyclesPerBufferLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cyclesPerBufferLabel.Location = new System.Drawing.Point(16, 62);
            this.cyclesPerBufferLabel.Name = "cyclesPerBufferLabel";
            this.cyclesPerBufferLabel.Size = new System.Drawing.Size(104, 23);
            this.cyclesPerBufferLabel.TabIndex = 2;
            this.cyclesPerBufferLabel.Text = "Cycles Per Buffer:";
            // 
            // cyclesPerBufferNumericUpDown
            // 
            this.cyclesPerBufferNumericUpDown.DecimalPlaces = 1;
            this.cyclesPerBufferNumericUpDown.Location = new System.Drawing.Point(120, 56);
            this.cyclesPerBufferNumericUpDown.Minimum = new System.Decimal(new int[] {
                                                                                         1,
                                                                                         0,
                                                                                         0,
                                                                                         0});
            this.cyclesPerBufferNumericUpDown.Name = "cyclesPerBufferNumericUpDown";
            this.cyclesPerBufferNumericUpDown.Size = new System.Drawing.Size(104, 20);
            this.cyclesPerBufferNumericUpDown.TabIndex = 3;
            this.cyclesPerBufferNumericUpDown.Value = new System.Decimal(new int[] {
                                                                                       50,
                                                                                       0,
                                                                                       0,
                                                                                       65536});
            // 
            // waveformTypeLabel
            // 
            this.waveformTypeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.waveformTypeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.waveformTypeLabel.Location = new System.Drawing.Point(16, 26);
            this.waveformTypeLabel.Name = "waveformTypeLabel";
            this.waveformTypeLabel.Size = new System.Drawing.Size(88, 14);
            this.waveformTypeLabel.TabIndex = 0;
            this.waveformTypeLabel.Text = "Waveform Type:";
            // 
            // signalTypeComboBox
            // 
            this.signalTypeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.signalTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.signalTypeComboBox.ItemHeight = 13;
            this.signalTypeComboBox.Location = new System.Drawing.Point(120, 24);
            this.signalTypeComboBox.Name = "signalTypeComboBox";
            this.signalTypeComboBox.Size = new System.Drawing.Size(104, 21);
            this.signalTypeComboBox.TabIndex = 1;
            // 
            // writeButton
            // 
            this.writeButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.writeButton.Location = new System.Drawing.Point(240, 224);
            this.writeButton.Name = "writeButton";
            this.writeButton.Size = new System.Drawing.Size(80, 23);
            this.writeButton.TabIndex = 0;
            this.writeButton.Text = "Write";
            this.writeButton.Click += new System.EventHandler(this.writeButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(536, 254);
            this.Controls.Add(this.channelParametersGroupBox);
            this.Controls.Add(this.functionGeneratorGroupBox);
            this.Controls.Add(this.writeButton);
            this.Controls.Add(this.TimingParametersGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Generate Multiple Current Updates - Internal Clock";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.MainForm_Closing);
            this.TimingParametersGroupBox.ResumeLayout(false);
            this.channelParametersGroupBox.ResumeLayout(false);
            this.functionGeneratorGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.offsetNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.amplitudeNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerBufferNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cyclesPerBufferNumericUpDown)).EndInit();
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

        private void writeButton_Click(object sender, System.EventArgs e)
        {
            writeButton.Enabled = false;
            taskRunning = true;

            try
            {
                // Create the task and channel
                myTask = new Task();

                myTask.AOChannels.CreateCurrentChannel(
                    physicalChannelComboBox.Text,
                    "",
                    Double.Parse(minimumValueTextBox.Text),
                    Double.Parse(maximumValueTextBox.Text),
                    AOCurrentUnits.Amps);

                // Verify the task before doing the waveform calculations
                myTask.Control(TaskAction.Verify);
                        
                // Calculate some waveform parameters and generate data
                FunctionGenerator fGen = new FunctionGenerator(
                    myTask.Timing,
                    frequencyTextBox.Text,
                    samplesPerBufferNumericUpDown.Value.ToString(),
                    cyclesPerBufferNumericUpDown.Value.ToString(),
                    signalTypeComboBox.Text,
                    amplitudeNumericUpDown.Value.ToString(),
                    offsetNumericUpDown.Value.ToString());

                // Configure the sample clock with calculated rate
                myTask.Timing.ConfigureSampleClock(
                    "",
                    fGen.ResultingSampleClockRate,
                    SampleClockActiveEdge.Rising,
                    SampleQuantityMode.FiniteSamples,
                    fGen.Data.Length);

                AnalogSingleChannelWriter writer = new AnalogSingleChannelWriter(myTask.Stream);

                AnalogWaveform<double> waveform = AnalogWaveform<double>.FromArray1D(fGen.Data);

                writer.WriteWaveform<double>(false, waveform);
                // Setup the Task Done event
                myTask.Done += new TaskDoneEventHandler(myTask_Done);

                myTask.Start();
                myTask.WaitUntilDone(10000);
            }
            catch (DaqException ex)
            {
                MessageBox.Show(ex.Message);

                if (myTask != null)
                    myTask.Dispose();

                taskRunning = false;
                writeButton.Enabled = true;
            }
        }

        private void myTask_Done(object sender, TaskDoneEventArgs e)
        {
            if (e.Error!= null)
            {
                MessageBox.Show(e.Error.Message);
            }

            if (myTask != null)
                myTask.Dispose();

            taskRunning = false;
            writeButton.Enabled = true;
        }

        private void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = taskRunning;
        }
    }
}
