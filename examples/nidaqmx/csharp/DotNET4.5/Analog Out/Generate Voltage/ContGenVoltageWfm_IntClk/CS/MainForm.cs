/******************************************************************************
*
* Example program:
*   ContGenVoltageWfm_IntClk
*
* Category:
*   AO
*
* Description:
*   This example demonstrates how to continuously output a periodic waveform
*   using an internal sample clock.
*
* Instructions for running:
*   1.  Select the physical channel corresponding to where your signal is output
*       on the DAQ device.
*   2.  Enter the minimum and maximum voltage values.
*   3.  Enter the desired rate for the generation. The onboard sample clock will
*       operate at this rate.
*   4.  Select the desired waveform type.
*   5.  The rest of the parameters in the Function Generator Parameters section
*       will affect the way the waveform is created, before it's sent to the
*       analog output of the board. Select the amplitude, number of samples per
*       buffer, and the number of cycles per buffer to be used as waveform data.
*
* Steps:
*   1.  Create a new task and an analog output voltage channel.
*   2.  Call the ConfigureSampleClock method to define the sample rate and a
*       continuous sample mode.
*   3.  Create a AnalogSingleChannelWriter and call the WriteMultiSample method
*       to write the waveform data to a buffer.
*   4.  Call Task.Start().
*   5.  When the user presses the stop button, stop the task.
*   6.  Dispose the Task object to clean-up any resources associated with the
*       task.
*   7.  Handle any DaqExceptions, if they occur.
*
* I/O Connections Overview:
*   Make sure your signal output terminal matches the physical channel control.
*   In this example, the signal will output to the ao0 pin on your DAQ device.
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
using NationalInstruments.Examples;

namespace NationalInstruments.Examples.ContGenVoltageWfm_IntClk
{
    /// <summary>
    /// Summary description for MainForm.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.GroupBox channelParametersGroupBox;
        private System.Windows.Forms.Label maximumLabel;
        private System.Windows.Forms.Label minimumLabel;
        private System.Windows.Forms.Label physicalChannelLabel;
        private System.Windows.Forms.TextBox maximumTextBox;
        private System.Windows.Forms.TextBox minimumTextBox;
        private System.Windows.Forms.GroupBox functionGeneratorGroupBox;
        private System.Windows.Forms.ComboBox signalTypeComboBox;
        private System.Windows.Forms.Label frequencyLabel;
        internal System.Windows.Forms.Label amplitudeLabel;
        private Task myTask;
        private System.Windows.Forms.NumericUpDown samplesPerBufferNumeric;
        private System.Windows.Forms.Label samplesperBufferLabel;
        private System.Windows.Forms.NumericUpDown cyclesPerBufferNumeric;
        internal System.Windows.Forms.NumericUpDown amplitudeNumeric;
        private System.Windows.Forms.GroupBox timingParametersGroupBox;
        private System.Windows.Forms.Label cyclesPerBufferLabel;
        private System.Windows.Forms.Label signalTypeLabel;
        private System.Windows.Forms.NumericUpDown frequencyNumeric;
        private System.Windows.Forms.Timer statusCheckTimer;
        private System.Windows.Forms.ComboBox physicalChannelComboBox;
        private System.ComponentModel.IContainer components;

        public MainForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
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
            this.stopButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.timingParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.frequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.frequencyLabel = new System.Windows.Forms.Label();
            this.channelParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.physicalChannelComboBox = new System.Windows.Forms.ComboBox();
            this.physicalChannelLabel = new System.Windows.Forms.Label();
            this.maximumTextBox = new System.Windows.Forms.TextBox();
            this.minimumTextBox = new System.Windows.Forms.TextBox();
            this.maximumLabel = new System.Windows.Forms.Label();
            this.minimumLabel = new System.Windows.Forms.Label();
            this.functionGeneratorGroupBox = new System.Windows.Forms.GroupBox();
            this.amplitudeLabel = new System.Windows.Forms.Label();
            this.amplitudeNumeric = new System.Windows.Forms.NumericUpDown();
            this.samplesPerBufferNumeric = new System.Windows.Forms.NumericUpDown();
            this.cyclesPerBufferLabel = new System.Windows.Forms.Label();
            this.cyclesPerBufferNumeric = new System.Windows.Forms.NumericUpDown();
            this.signalTypeLabel = new System.Windows.Forms.Label();
            this.signalTypeComboBox = new System.Windows.Forms.ComboBox();
            this.samplesperBufferLabel = new System.Windows.Forms.Label();
            this.statusCheckTimer = new System.Windows.Forms.Timer(this.components);
            this.timingParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.frequencyNumeric)).BeginInit();
            this.channelParametersGroupBox.SuspendLayout();
            this.functionGeneratorGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.amplitudeNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerBufferNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cyclesPerBufferNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.stopButton.Location = new System.Drawing.Point(184, 414);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(80, 24);
            this.stopButton.TabIndex = 1;
            this.stopButton.Text = "Stop";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // startButton
            // 
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.startButton.Location = new System.Drawing.Point(70, 414);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(80, 24);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // timingParametersGroupBox
            // 
            this.timingParametersGroupBox.Controls.Add(this.frequencyNumeric);
            this.timingParametersGroupBox.Controls.Add(this.frequencyLabel);
            this.timingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.timingParametersGroupBox.Location = new System.Drawing.Point(46, 150);
            this.timingParametersGroupBox.Name = "timingParametersGroupBox";
            this.timingParametersGroupBox.Size = new System.Drawing.Size(247, 64);
            this.timingParametersGroupBox.TabIndex = 3;
            this.timingParametersGroupBox.TabStop = false;
            this.timingParametersGroupBox.Text = "Timing Parameters";
            // 
            // frequencyNumeric
            // 
            this.frequencyNumeric.Location = new System.Drawing.Point(120, 24);
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
            this.frequencyNumeric.Size = new System.Drawing.Size(112, 20);
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
            this.frequencyLabel.Location = new System.Drawing.Point(16, 26);
            this.frequencyLabel.Name = "frequencyLabel";
            this.frequencyLabel.Size = new System.Drawing.Size(88, 16);
            this.frequencyLabel.TabIndex = 0;
            this.frequencyLabel.Text = "Frequency (Hz):";
            // 
            // channelParametersGroupBox
            // 
            this.channelParametersGroupBox.Controls.Add(this.physicalChannelComboBox);
            this.channelParametersGroupBox.Controls.Add(this.physicalChannelLabel);
            this.channelParametersGroupBox.Controls.Add(this.maximumTextBox);
            this.channelParametersGroupBox.Controls.Add(this.minimumTextBox);
            this.channelParametersGroupBox.Controls.Add(this.maximumLabel);
            this.channelParametersGroupBox.Controls.Add(this.minimumLabel);
            this.channelParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.channelParametersGroupBox.Location = new System.Drawing.Point(46, 14);
            this.channelParametersGroupBox.Name = "channelParametersGroupBox";
            this.channelParametersGroupBox.Size = new System.Drawing.Size(247, 128);
            this.channelParametersGroupBox.TabIndex = 2;
            this.channelParametersGroupBox.TabStop = false;
            this.channelParametersGroupBox.Text = "Channel Parameters";
            // 
            // physicalChannelComboBox
            // 
            this.physicalChannelComboBox.Location = new System.Drawing.Point(120, 24);
            this.physicalChannelComboBox.Name = "physicalChannelComboBox";
            this.physicalChannelComboBox.Size = new System.Drawing.Size(112, 21);
            this.physicalChannelComboBox.TabIndex = 1;
            this.physicalChannelComboBox.Text = "Dev1/ao0";
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
            // maximumTextBox
            // 
            this.maximumTextBox.Location = new System.Drawing.Point(120, 96);
            this.maximumTextBox.Name = "maximumTextBox";
            this.maximumTextBox.Size = new System.Drawing.Size(112, 20);
            this.maximumTextBox.TabIndex = 5;
            this.maximumTextBox.Text = "10";
            // 
            // minimumTextBox
            // 
            this.minimumTextBox.Location = new System.Drawing.Point(120, 60);
            this.minimumTextBox.Name = "minimumTextBox";
            this.minimumTextBox.Size = new System.Drawing.Size(112, 20);
            this.minimumTextBox.TabIndex = 3;
            this.minimumTextBox.Text = "-10";
            // 
            // maximumLabel
            // 
            this.maximumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.maximumLabel.Location = new System.Drawing.Point(16, 98);
            this.maximumLabel.Name = "maximumLabel";
            this.maximumLabel.Size = new System.Drawing.Size(112, 16);
            this.maximumLabel.TabIndex = 4;
            this.maximumLabel.Text = "Maximum Value (V):";
            // 
            // minimumLabel
            // 
            this.minimumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.minimumLabel.Location = new System.Drawing.Point(16, 62);
            this.minimumLabel.Name = "minimumLabel";
            this.minimumLabel.Size = new System.Drawing.Size(104, 16);
            this.minimumLabel.TabIndex = 2;
            this.minimumLabel.Text = "Minimum Value (V):";
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
            this.functionGeneratorGroupBox.Controls.Add(this.samplesperBufferLabel);
            this.functionGeneratorGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.functionGeneratorGroupBox.Location = new System.Drawing.Point(45, 222);
            this.functionGeneratorGroupBox.Name = "functionGeneratorGroupBox";
            this.functionGeneratorGroupBox.Size = new System.Drawing.Size(248, 176);
            this.functionGeneratorGroupBox.TabIndex = 4;
            this.functionGeneratorGroupBox.TabStop = false;
            this.functionGeneratorGroupBox.Text = "Function Generator Parameters";
            // 
            // amplitudeLabel
            // 
            this.amplitudeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.amplitudeLabel.Location = new System.Drawing.Point(16, 138);
            this.amplitudeLabel.Name = "amplitudeLabel";
            this.amplitudeLabel.Size = new System.Drawing.Size(56, 16);
            this.amplitudeLabel.TabIndex = 6;
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
            this.amplitudeNumeric.Size = new System.Drawing.Size(112, 20);
            this.amplitudeNumeric.TabIndex = 7;
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
                                                                                    10000000,
                                                                                    0,
                                                                                    0,
                                                                                    0});
            this.samplesPerBufferNumeric.Minimum = new System.Decimal(new int[] {
                                                                                    1,
                                                                                    0,
                                                                                    0,
                                                                                    0});
            this.samplesPerBufferNumeric.Name = "samplesPerBufferNumeric";
            this.samplesPerBufferNumeric.Size = new System.Drawing.Size(112, 20);
            this.samplesPerBufferNumeric.TabIndex = 5;
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
            this.cyclesPerBufferLabel.TabIndex = 2;
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
            this.cyclesPerBufferNumeric.Size = new System.Drawing.Size(112, 20);
            this.cyclesPerBufferNumeric.TabIndex = 3;
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
            this.signalTypeLabel.TabIndex = 0;
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
            this.signalTypeComboBox.Size = new System.Drawing.Size(112, 21);
            this.signalTypeComboBox.TabIndex = 1;
            // 
            // samplesperBufferLabel
            // 
            this.samplesperBufferLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.samplesperBufferLabel.Location = new System.Drawing.Point(16, 98);
            this.samplesperBufferLabel.Name = "samplesperBufferLabel";
            this.samplesperBufferLabel.Size = new System.Drawing.Size(112, 16);
            this.samplesperBufferLabel.TabIndex = 4;
            this.samplesperBufferLabel.Text = "Samples Per Buffer:";
            // 
            // statusCheckTimer
            // 
            this.statusCheckTimer.Tick += new System.EventHandler(this.statusCheckTimer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(338, 453);
            this.Controls.Add(this.functionGeneratorGroupBox);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.timingParametersGroupBox);
            this.Controls.Add(this.channelParametersGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Continuous Voltage Generation - Int Clk";
            this.timingParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.frequencyNumeric)).EndInit();
            this.channelParametersGroupBox.ResumeLayout(false);
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
                // create the task and channel
                myTask = new Task();
                myTask.AOChannels.CreateVoltageChannel(physicalChannelComboBox.Text,
                    "",
                    Convert.ToDouble(minimumTextBox.Text), 
                    Convert.ToDouble(maximumTextBox.Text),
                    AOVoltageUnits.Volts);

                // verify the task before doing the waveform calculations
                myTask.Control(TaskAction.Verify);

                // calculate some waveform parameters and generate data
                FunctionGenerator fGen = new FunctionGenerator(
                    myTask.Timing,
                    frequencyNumeric.Value.ToString(),
                    samplesPerBufferNumeric.Value.ToString(),
                    cyclesPerBufferNumeric.Value.ToString(),
                    signalTypeComboBox.Text,
                    amplitudeNumeric.Value.ToString());
                
                // configure the sample clock with the calculated rate
                myTask.Timing.ConfigureSampleClock("",
                    fGen.ResultingSampleClockRate,
                    SampleClockActiveEdge.Rising,
                    SampleQuantityMode.ContinuousSamples, 1000);


                AnalogSingleChannelWriter writer = 
                    new AnalogSingleChannelWriter(myTask.Stream);

                //write data to buffer
                writer.WriteMultiSample(false,fGen.Data);
                
                //start writing out data
                myTask.Start();
                
                startButton.Enabled = false;
                stopButton.Enabled = true;

                statusCheckTimer.Enabled = true;
            }
            catch(DaqException err)
            {
                statusCheckTimer.Enabled = false;
                MessageBox.Show(err.Message);
                myTask.Dispose();
            }
            Cursor.Current = Cursors.Default;
        }
        
        private void stopButton_Click(object sender, System.EventArgs e)
        {
            statusCheckTimer.Enabled = false;
            if (myTask != null)
            {
                try
                {
                    myTask.Stop();
                }
                catch(Exception x)
                {
                    MessageBox.Show(x.Message);
                }
                myTask.Dispose();
                myTask = null;
                startButton.Enabled = true;
                stopButton.Enabled = false;
            }
        }

        private void statusCheckTimer_Tick(object sender, System.EventArgs e)
        {
            try
            {
                // Getting myTask.IsDone also checks for errors that would prematurely
                // halt the continuous generation.
                if (myTask.IsDone)
                {
                    statusCheckTimer.Enabled = false;
                    myTask.Stop();
                    myTask.Dispose();
                    startButton.Enabled = true;
                    stopButton.Enabled = false;
                }
            }
            catch (DaqException ex)
            {
                statusCheckTimer.Enabled = false;
                System.Windows.Forms.MessageBox.Show(ex.Message);
                myTask.Dispose();
                startButton.Enabled = true;
                stopButton.Enabled = false;
            }
        }
    }
}
