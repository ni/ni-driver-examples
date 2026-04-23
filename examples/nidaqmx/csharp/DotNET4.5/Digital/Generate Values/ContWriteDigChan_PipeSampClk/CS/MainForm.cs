/******************************************************************************
*
* Example program:
*   ContWriteDigChan_PipeSampClk
*
* Category:
*   DO
*
* Description:
*   This examples demostrates how to interface the NI 6536/7 to a synchronous
*   DAC with an output enable signal.
*
* Instructions for running:
*   1.  Select the Physical Channels that correspond to where yoursignal is
*       output on the device.
*   2.  Enter the number of Samples per Buffer. This is the number ofsamples
*       that will be downloaded to the device every time theDAQmx Write function
*       is called.
*   3.  Specify the Sample Clock Rate of the output waveform.
*   4.  Specify the Output Terminal for the Exported Sample Clock.
*   5.  Specify the Sample Clock Pulse Polarity. When set to ActiveHigh, the
*       data lines will toggle on the rising edge of thesample clock.
*   6.  Specify the data active parameters.  The Data Active Event will be
*       asserted when a valid sample is clocked out.
*
* Steps:
*   1.  Create a task.
*   2.  Create one Digital Output channel for each Digital Line in the Task.
*   3.  Configure the Task to use a pipelined sampled clock.
*   4.  Configure the exported sample clock and data active event.
*   5.  Disallow Regeneration. When regeneration is disallowed, thedata transfer
*       between the device and the DAQmx buffer willpause when the device has
*       emptied this buffer. It will resumewhen more data has been written into
*       the buffer.
*   6.  Generate a random DigitalWaveform for every digital channel in the Task.
*   7.  Create a DigitalMultiChannelWriter and associate it with the task
*       byusing the task's stream.
*   8.  Call DigitalMultiChannelWriter.BeginWriteWaveform to install a callback
*       and begin the asynchronous write operation.
*   9.  Inside the callback, call DigitalMultiChannelWriter.EndWrite to handle
*       the end of the asynchronous write operation.  
*   10. Call DigitalMultiChannelWriter.BeginWriteWaveform again inside the
*       callback to perform another write operation.
*   11. Handle any DaqExceptions, if they occur.
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
*   Connect the data active event to the DAC's output enable signal.  Connect
*   the exported clock to the DAC's clock signal.  Connect the data lines from
*   the NI 6536/7 to the DAC's data lines.
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

using NationalInstruments.DAQmx;
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace NationalInstruments.Examples.ContWriteDigChan_PipeSampClk
{
    /// <summary>
    /// Summary description for MainForm.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.GroupBox channelParametersGroupBox;
        private System.Windows.Forms.Label physicalChannelLabel;
        private System.Windows.Forms.ComboBox physicalChannelComboBox;
        private System.Windows.Forms.Label samplesPerBufferLabel;
        private System.Windows.Forms.NumericUpDown samplesPerBufferNumericUpDown;
        private System.Windows.Forms.GroupBox timingParametersGroupBox;
        private System.Windows.Forms.Label sampleClockOutputTerminalLabel;
        private System.Windows.Forms.Label sampleClockRateLabel;
        private System.Windows.Forms.NumericUpDown samplesClockRateNumericUpDown;
        private System.Windows.Forms.TextBox clockSourceTextBox;
        private System.Windows.Forms.ComboBox sampleClockPulsePolarityComboBox;
        private System.Windows.Forms.Label sampleCloclPulsePolarityLabel;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        
        private Task myTask;
        private Task runningTask;
        private DigitalWaveform[] waveform;
        private AsyncCallback digitalCallback;
        private DigitalMultiChannelWriter writer;
        private System.Windows.Forms.Label dataActiveOutputTerminalLabel;
        private System.Windows.Forms.TextBox dataActiveOutputTerminalTextBox;
        private System.Windows.Forms.ComboBox dataActiveActiveLevelComboxBox;
        private System.Windows.Forms.Label dataActiveActiveLevelLabel;
        private System.Windows.Forms.GroupBox dataActiveMembersGroupBox;

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

            dataActiveActiveLevelComboxBox.SelectedIndex = 1;
            sampleClockPulsePolarityComboBox.SelectedIndex = 0;

            startButton.Enabled = false;

            physicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DOLine, PhysicalChannelAccess.External));
            if (physicalChannelComboBox.Items.Count > 0)
            {
                physicalChannelComboBox.SelectedIndex = 0;
                startButton.Enabled = true;
            }

            stopButton.Enabled = false;
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
                    runningTask = null;
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
            this.channelParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.samplesPerBufferNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.samplesPerBufferLabel = new System.Windows.Forms.Label();
            this.physicalChannelComboBox = new System.Windows.Forms.ComboBox();
            this.physicalChannelLabel = new System.Windows.Forms.Label();
            this.timingParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.sampleClockPulsePolarityComboBox = new System.Windows.Forms.ComboBox();
            this.sampleCloclPulsePolarityLabel = new System.Windows.Forms.Label();
            this.clockSourceTextBox = new System.Windows.Forms.TextBox();
            this.samplesClockRateNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.sampleClockOutputTerminalLabel = new System.Windows.Forms.Label();
            this.sampleClockRateLabel = new System.Windows.Forms.Label();
            this.dataActiveMembersGroupBox = new System.Windows.Forms.GroupBox();
            this.dataActiveOutputTerminalTextBox = new System.Windows.Forms.TextBox();
            this.dataActiveActiveLevelComboxBox = new System.Windows.Forms.ComboBox();
            this.dataActiveActiveLevelLabel = new System.Windows.Forms.Label();
            this.dataActiveOutputTerminalLabel = new System.Windows.Forms.Label();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.channelParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerBufferNumericUpDown)).BeginInit();
            this.timingParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.samplesClockRateNumericUpDown)).BeginInit();
            this.dataActiveMembersGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // channelParametersGroupBox
            // 
            this.channelParametersGroupBox.Controls.Add(this.samplesPerBufferNumericUpDown);
            this.channelParametersGroupBox.Controls.Add(this.samplesPerBufferLabel);
            this.channelParametersGroupBox.Controls.Add(this.physicalChannelComboBox);
            this.channelParametersGroupBox.Controls.Add(this.physicalChannelLabel);
            this.channelParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.channelParametersGroupBox.Location = new System.Drawing.Point(8, 16);
            this.channelParametersGroupBox.Name = "channelParametersGroupBox";
            this.channelParametersGroupBox.Size = new System.Drawing.Size(368, 96);
            this.channelParametersGroupBox.TabIndex = 0;
            this.channelParametersGroupBox.TabStop = false;
            this.channelParametersGroupBox.Text = "Channel Parameters";
            // 
            // samplesPerBufferNumericUpDown
            // 
            this.samplesPerBufferNumericUpDown.Location = new System.Drawing.Point(241, 64);
            this.samplesPerBufferNumericUpDown.Maximum = new System.Decimal(new int[] {
                                                                                          1000000,
                                                                                          0,
                                                                                          0,
                                                                                          0});
            this.samplesPerBufferNumericUpDown.Minimum = new System.Decimal(new int[] {
                                                                                          1,
                                                                                          0,
                                                                                          0,
                                                                                          0});
            this.samplesPerBufferNumericUpDown.Name = "samplesPerBufferNumericUpDown";
            this.samplesPerBufferNumericUpDown.TabIndex = 3;
            this.samplesPerBufferNumericUpDown.Value = new System.Decimal(new int[] {
                                                                                        1000,
                                                                                        0,
                                                                                        0,
                                                                                        0});
            // 
            // samplesPerBufferLabel
            // 
            this.samplesPerBufferLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.samplesPerBufferLabel.Location = new System.Drawing.Point(8, 64);
            this.samplesPerBufferLabel.Name = "samplesPerBufferLabel";
            this.samplesPerBufferLabel.Size = new System.Drawing.Size(120, 23);
            this.samplesPerBufferLabel.TabIndex = 2;
            this.samplesPerBufferLabel.Text = "Samples Per Buffer:";
            // 
            // physicalChannelComboBox
            // 
            this.physicalChannelComboBox.Location = new System.Drawing.Point(240, 18);
            this.physicalChannelComboBox.Name = "physicalChannelComboBox";
            this.physicalChannelComboBox.Size = new System.Drawing.Size(121, 21);
            this.physicalChannelComboBox.TabIndex = 1;
            this.physicalChannelComboBox.Text = "Dev1/port0/line0:7";
            // 
            // physicalChannelLabel
            // 
            this.physicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.physicalChannelLabel.Location = new System.Drawing.Point(8, 24);
            this.physicalChannelLabel.Name = "physicalChannelLabel";
            this.physicalChannelLabel.TabIndex = 0;
            this.physicalChannelLabel.Text = "Physical Channel:";
            // 
            // timingParametersGroupBox
            // 
            this.timingParametersGroupBox.Controls.Add(this.sampleClockPulsePolarityComboBox);
            this.timingParametersGroupBox.Controls.Add(this.sampleCloclPulsePolarityLabel);
            this.timingParametersGroupBox.Controls.Add(this.clockSourceTextBox);
            this.timingParametersGroupBox.Controls.Add(this.samplesClockRateNumericUpDown);
            this.timingParametersGroupBox.Controls.Add(this.sampleClockOutputTerminalLabel);
            this.timingParametersGroupBox.Controls.Add(this.sampleClockRateLabel);
            this.timingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.timingParametersGroupBox.Location = new System.Drawing.Point(8, 128);
            this.timingParametersGroupBox.Name = "timingParametersGroupBox";
            this.timingParametersGroupBox.Size = new System.Drawing.Size(368, 136);
            this.timingParametersGroupBox.TabIndex = 1;
            this.timingParametersGroupBox.TabStop = false;
            this.timingParametersGroupBox.Text = "Timing Parameters";
            // 
            // sampleClockPulsePolarityComboBox
            // 
            this.sampleClockPulsePolarityComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sampleClockPulsePolarityComboBox.Items.AddRange(new object[] {
                                                                                  "Active High",
                                                                                  "Active Low"});
            this.sampleClockPulsePolarityComboBox.Location = new System.Drawing.Point(240, 96);
            this.sampleClockPulsePolarityComboBox.Name = "sampleClockPulsePolarityComboBox";
            this.sampleClockPulsePolarityComboBox.Size = new System.Drawing.Size(121, 21);
            this.sampleClockPulsePolarityComboBox.TabIndex = 5;
            // 
            // sampleCloclPulsePolarityLabel
            // 
            this.sampleCloclPulsePolarityLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.sampleCloclPulsePolarityLabel.Location = new System.Drawing.Point(8, 104);
            this.sampleCloclPulsePolarityLabel.Name = "sampleCloclPulsePolarityLabel";
            this.sampleCloclPulsePolarityLabel.Size = new System.Drawing.Size(148, 23);
            this.sampleCloclPulsePolarityLabel.TabIndex = 4;
            this.sampleCloclPulsePolarityLabel.Text = "Sample Clock Pulse Polarity:";
            // 
            // clockSourceTextBox
            // 
            this.clockSourceTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.clockSourceTextBox.Location = new System.Drawing.Point(241, 59);
            this.clockSourceTextBox.Name = "clockSourceTextBox";
            this.clockSourceTextBox.Size = new System.Drawing.Size(120, 20);
            this.clockSourceTextBox.TabIndex = 3;
            this.clockSourceTextBox.Text = "/Dev1/PFI4";
            // 
            // samplesClockRateNumericUpDown
            // 
            this.samplesClockRateNumericUpDown.Location = new System.Drawing.Point(241, 21);
            this.samplesClockRateNumericUpDown.Maximum = new System.Decimal(new int[] {
                                                                                          1000000,
                                                                                          0,
                                                                                          0,
                                                                                          0});
            this.samplesClockRateNumericUpDown.Minimum = new System.Decimal(new int[] {
                                                                                          1,
                                                                                          0,
                                                                                          0,
                                                                                          0});
            this.samplesClockRateNumericUpDown.Name = "samplesClockRateNumericUpDown";
            this.samplesClockRateNumericUpDown.TabIndex = 1;
            this.samplesClockRateNumericUpDown.Value = new System.Decimal(new int[] {
                                                                                        100000,
                                                                                        0,
                                                                                        0,
                                                                                        0});
            // 
            // sampleClockOutputTerminalLabel
            // 
            this.sampleClockOutputTerminalLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.sampleClockOutputTerminalLabel.Location = new System.Drawing.Point(8, 64);
            this.sampleClockOutputTerminalLabel.Name = "sampleClockOutputTerminalLabel";
            this.sampleClockOutputTerminalLabel.Size = new System.Drawing.Size(160, 23);
            this.sampleClockOutputTerminalLabel.TabIndex = 2;
            this.sampleClockOutputTerminalLabel.Text = "Sample Clock Output Terminal:";
            // 
            // sampleClockRateLabel
            // 
            this.sampleClockRateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.sampleClockRateLabel.Location = new System.Drawing.Point(8, 24);
            this.sampleClockRateLabel.Name = "sampleClockRateLabel";
            this.sampleClockRateLabel.Size = new System.Drawing.Size(120, 23);
            this.sampleClockRateLabel.TabIndex = 0;
            this.sampleClockRateLabel.Text = "Sample Clock Rate (Hz):";
            // 
            // dataActiveMembersGroupBox
            // 
            this.dataActiveMembersGroupBox.Controls.Add(this.dataActiveOutputTerminalTextBox);
            this.dataActiveMembersGroupBox.Controls.Add(this.dataActiveActiveLevelComboxBox);
            this.dataActiveMembersGroupBox.Controls.Add(this.dataActiveActiveLevelLabel);
            this.dataActiveMembersGroupBox.Controls.Add(this.dataActiveOutputTerminalLabel);
            this.dataActiveMembersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.dataActiveMembersGroupBox.Location = new System.Drawing.Point(8, 280);
            this.dataActiveMembersGroupBox.Name = "dataActiveMembersGroupBox";
            this.dataActiveMembersGroupBox.Size = new System.Drawing.Size(368, 96);
            this.dataActiveMembersGroupBox.TabIndex = 2;
            this.dataActiveMembersGroupBox.TabStop = false;
            this.dataActiveMembersGroupBox.Text = "Data Active Members";
            // 
            // dataActiveOutputTerminalTextBox
            // 
            this.dataActiveOutputTerminalTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.dataActiveOutputTerminalTextBox.Location = new System.Drawing.Point(240, 62);
            this.dataActiveOutputTerminalTextBox.Name = "dataActiveOutputTerminalTextBox";
            this.dataActiveOutputTerminalTextBox.Size = new System.Drawing.Size(120, 20);
            this.dataActiveOutputTerminalTextBox.TabIndex = 3;
            this.dataActiveOutputTerminalTextBox.Text = "/Dev1/PFI0";
            // 
            // dataActiveActiveLevelComboxBox
            // 
            this.dataActiveActiveLevelComboxBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dataActiveActiveLevelComboxBox.Items.AddRange(new object[] {
                                                                                "Active High",
                                                                                "Active Low"});
            this.dataActiveActiveLevelComboxBox.Location = new System.Drawing.Point(240, 19);
            this.dataActiveActiveLevelComboxBox.Name = "dataActiveActiveLevelComboxBox";
            this.dataActiveActiveLevelComboxBox.Size = new System.Drawing.Size(121, 21);
            this.dataActiveActiveLevelComboxBox.TabIndex = 1;
            // 
            // dataActiveActiveLevelLabel
            // 
            this.dataActiveActiveLevelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.dataActiveActiveLevelLabel.Location = new System.Drawing.Point(8, 24);
            this.dataActiveActiveLevelLabel.Name = "dataActiveActiveLevelLabel";
            this.dataActiveActiveLevelLabel.Size = new System.Drawing.Size(200, 23);
            this.dataActiveActiveLevelLabel.TabIndex = 0;
            this.dataActiveActiveLevelLabel.Text = "Data Active Active Level:";
            // 
            // dataActiveOutputTerminalLabel
            // 
            this.dataActiveOutputTerminalLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.dataActiveOutputTerminalLabel.Location = new System.Drawing.Point(8, 64);
            this.dataActiveOutputTerminalLabel.Name = "dataActiveOutputTerminalLabel";
            this.dataActiveOutputTerminalLabel.Size = new System.Drawing.Size(200, 23);
            this.dataActiveOutputTerminalLabel.TabIndex = 2;
            this.dataActiveOutputTerminalLabel.Text = "Data Active Output Terminal:";
            // 
            // startButton
            // 
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.startButton.Location = new System.Drawing.Point(86, 392);
            this.startButton.Name = "startButton";
            this.startButton.TabIndex = 3;
            this.startButton.Text = "Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.stopButton.Location = new System.Drawing.Point(214, 392);
            this.stopButton.Name = "stopButton";
            this.stopButton.TabIndex = 4;
            this.stopButton.Text = "Stop";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(384, 430);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.dataActiveMembersGroupBox);
            this.Controls.Add(this.channelParametersGroupBox);
            this.Controls.Add(this.timingParametersGroupBox);
            this.Controls.Add(this.stopButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Continous Write Digital Channel - Pipeline Samp Clock";
            this.channelParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerBufferNumericUpDown)).EndInit();
            this.timingParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.samplesClockRateNumericUpDown)).EndInit();
            this.dataActiveMembersGroupBox.ResumeLayout(false);
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
            stopButton.Enabled = true;
            try
            {
                DataActiveEventLevelActiveLevel activeLevel;
                SampleClockPulsePolarity pulsePolarity;

                if (dataActiveActiveLevelComboxBox.SelectedIndex == 0)
                    activeLevel = DataActiveEventLevelActiveLevel.ActiveHigh;
                else
                    activeLevel = DataActiveEventLevelActiveLevel.ActiveLow;

                if (sampleClockPulsePolarityComboBox.SelectedIndex == 0)
                    pulsePolarity = SampleClockPulsePolarity.ActiveHigh;
                else
                    pulsePolarity = SampleClockPulsePolarity.ActiveLow;
                
                // Create and configure DO channel
                myTask = new Task();
                myTask.DOChannels.CreateChannel(physicalChannelComboBox.Text, "", ChannelLineGrouping.OneChannelForEachLine);
                myTask.Timing.ConfigurePipelinedSampleClock("", (double)samplesClockRateNumericUpDown.Value, SampleClockActiveEdge.Rising, 
                                         SampleQuantityMode.ContinuousSamples, (int) samplesPerBufferNumericUpDown.Value);
               
                myTask.Control(TaskAction.Verify);

                // Set ExportSignals properties
                myTask.ExportSignals.SampleClockOutputTerminal = clockSourceTextBox.Text;
                myTask.ExportSignals.SampleClockPulsePolarity = pulsePolarity;
                myTask.ExportSignals.DataActiveEventOutputTerminal = dataActiveOutputTerminalTextBox.Text;
                myTask.ExportSignals.DataActiveEventLevelActiveLevel = activeLevel;
                
                // Do not allow regeneration of data.
                myTask.Stream.WriteRegenerationMode = WriteRegenerationMode.DoNotAllowRegeneration;

                int states = (int)samplesPerBufferNumericUpDown.Value;
                int signals = myTask.DOChannels.Count;

                // Loop through every sample
                waveform = new DigitalWaveform[signals];
               
                Random r = new Random();
                for (int i = 0; i < signals; i++)
                {
                    waveform[i] = new DigitalWaveform(Convert.ToInt32(samplesPerBufferNumericUpDown.Value), 1);
                    // Generate a random set of boolean values
                    for (int j = 0; j < states; j++)
                    {
                        if (r.Next() % 2 == 0)
                            waveform[i].Signals[0].States[j] = DigitalState.ForceUp;
                        else
                            waveform[i].Signals[0].States[j] = DigitalState.ForceDown;
                    }
                }

                runningTask = myTask;
                
                writer = new DigitalMultiChannelWriter(myTask.Stream);

                // Use SynchronizeCallbacks to specify that the object 
                // marshals callbacks across threads appropriately.
                myTask.SynchronizeCallbacks = true;

                digitalCallback = new AsyncCallback(DigitalCallback);

                // Set up callback
                writer.BeginWriteWaveform(true, waveform, digitalCallback, myTask);
            }
            catch (DaqException exception)
            {
                MessageBox.Show(exception.Message);

                runningTask = null;
                myTask.Dispose();
                startButton.Enabled = true;
                stopButton.Enabled = false;
            }
            Cursor.Current = Cursors.Default;
        }

        private void DigitalCallback(IAsyncResult ar)
        {
            try
            {
                if (runningTask != null && runningTask == ar.AsyncState)
                {
                    // End async write operation
                    writer.EndWrite(ar);

                    // Set up new callback
                    writer.BeginWriteWaveform(false, waveform, digitalCallback, myTask);
                }
            }
            catch (DaqException exception)
            {
                MessageBox.Show(exception.Message);

                runningTask = null;
                myTask.Dispose();
                startButton.Enabled = true;
                stopButton.Enabled = false;
            }
        }

        private void stopButton_Click(object sender, System.EventArgs e)
        {
            runningTask = null;
            myTask.Dispose();

            stopButton.Enabled = false;
            startButton.Enabled = true;
        }
    }
}
