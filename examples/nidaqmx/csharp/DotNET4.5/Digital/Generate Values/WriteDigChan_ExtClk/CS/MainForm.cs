/******************************************************************************
*
* Example program:
*   WriteDigChan_ExtClk
*
* Category:
*   DO
*
* Description:
*   This example demonstrates how to write values to a digital output channel
*   using an external sample clock.
*
* Instructions for running:
*   1.  Select the physical channel on the DAQ device.
*   2.  Select the external clock source.
*   3.  Select the sample clock rate.
*   4.  Select the number of samples.
*
* Steps:
*   1.  Create a new digital output task.
*   2.  Create the digital output channel.
*   3.  Configure the task to use an external sample clock.
*   4.  Create a DigitalSingleChannelWriter and associate it with the task by
*       using the task's stream.
*   5.  Generate a waveform with random states to write to the channel.
*   6.  Call DigitalSingleChannelWriter.WriteWaveform to write the data to the
*       channel.
*   7.  Dispose the Task object to clean-up any resources associated with the
*       task.
*   8.  Handle any DaqExceptions, if they occur.
*
* I/O Connections Overview:
*   Make sure your signal output terminals match the physical channel text box. 
*   In this case wire your digital signals to the appropriate eight digital
*   lines on your DAQ Device.  For more information on the input and output
*   terminals for your device, open the NI-DAQmx Help, and refer to the NI-DAQmx
*   Device Terminals and Device Considerations books in the table of contents.
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

namespace NationalInstruments.Examples.WriteDigChan_ExtClk
{
    /// <summary>
    /// Summary description for Mainform.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Label physicalChannelsLabel;
        private System.Windows.Forms.Label clockSourceLabel;
        private System.Windows.Forms.TextBox clockSourceTextBox;
        private System.Windows.Forms.Label sampleClockRateLabel;
        private System.Windows.Forms.Label numberSamplesLabel;
        private System.Windows.Forms.Button writeButton;
        private System.Windows.Forms.NumericUpDown sampleClockRateNumeric;
        private System.Windows.Forms.NumericUpDown numberSamplesNumeric;
        private System.Windows.Forms.ComboBox physicalChannelComboBox;
        
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
            physicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DOLine | PhysicalChannelTypes.DOPort, PhysicalChannelAccess.External));
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainForm));
            this.physicalChannelsLabel = new System.Windows.Forms.Label();
            this.clockSourceLabel = new System.Windows.Forms.Label();
            this.clockSourceTextBox = new System.Windows.Forms.TextBox();
            this.sampleClockRateLabel = new System.Windows.Forms.Label();
            this.numberSamplesLabel = new System.Windows.Forms.Label();
            this.writeButton = new System.Windows.Forms.Button();
            this.sampleClockRateNumeric = new System.Windows.Forms.NumericUpDown();
            this.numberSamplesNumeric = new System.Windows.Forms.NumericUpDown();
            this.physicalChannelComboBox = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.sampleClockRateNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberSamplesNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // physicalChannelsLabel
            // 
            this.physicalChannelsLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.physicalChannelsLabel.Location = new System.Drawing.Point(8, 16);
            this.physicalChannelsLabel.Name = "physicalChannelsLabel";
            this.physicalChannelsLabel.Size = new System.Drawing.Size(112, 23);
            this.physicalChannelsLabel.TabIndex = 0;
            this.physicalChannelsLabel.Text = "Physical Channels:";
            // 
            // clockSourceLabel
            // 
            this.clockSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.clockSourceLabel.Location = new System.Drawing.Point(8, 53);
            this.clockSourceLabel.Name = "clockSourceLabel";
            this.clockSourceLabel.Size = new System.Drawing.Size(112, 23);
            this.clockSourceLabel.TabIndex = 2;
            this.clockSourceLabel.Text = "Clock Source:";
            // 
            // clockSourceTextBox
            // 
            this.clockSourceTextBox.Location = new System.Drawing.Point(144, 54);
            this.clockSourceTextBox.Name = "clockSourceTextBox";
            this.clockSourceTextBox.Size = new System.Drawing.Size(176, 20);
            this.clockSourceTextBox.TabIndex = 3;
            this.clockSourceTextBox.Text = "/Dev1/PFI0";
            // 
            // sampleClockRateLabel
            // 
            this.sampleClockRateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.sampleClockRateLabel.Location = new System.Drawing.Point(8, 90);
            this.sampleClockRateLabel.Name = "sampleClockRateLabel";
            this.sampleClockRateLabel.Size = new System.Drawing.Size(112, 23);
            this.sampleClockRateLabel.TabIndex = 4;
            this.sampleClockRateLabel.Text = "Sample Clock Rate:";
            // 
            // numberSamplesLabel
            // 
            this.numberSamplesLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.numberSamplesLabel.Location = new System.Drawing.Point(8, 127);
            this.numberSamplesLabel.Name = "numberSamplesLabel";
            this.numberSamplesLabel.Size = new System.Drawing.Size(112, 23);
            this.numberSamplesLabel.TabIndex = 6;
            this.numberSamplesLabel.Text = "Number of Samples:";
            // 
            // writeButton
            // 
            this.writeButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.writeButton.Location = new System.Drawing.Point(128, 160);
            this.writeButton.Name = "writeButton";
            this.writeButton.TabIndex = 8;
            this.writeButton.Text = "Write Data";
            this.writeButton.Click += new System.EventHandler(this.writeButton_Click);
            // 
            // sampleClockRateNumeric
            // 
            this.sampleClockRateNumeric.DecimalPlaces = 2;
            this.sampleClockRateNumeric.Location = new System.Drawing.Point(144, 91);
            this.sampleClockRateNumeric.Maximum = new System.Decimal(new int[] {
                                                                                   100000,
                                                                                   0,
                                                                                   0,
                                                                                   0});
            this.sampleClockRateNumeric.Minimum = new System.Decimal(new int[] {
                                                                                   2,
                                                                                   0,
                                                                                   0,
                                                                                   0});
            this.sampleClockRateNumeric.Name = "sampleClockRateNumeric";
            this.sampleClockRateNumeric.Size = new System.Drawing.Size(176, 20);
            this.sampleClockRateNumeric.TabIndex = 5;
            this.sampleClockRateNumeric.Value = new System.Decimal(new int[] {
                                                                                 1000,
                                                                                 0,
                                                                                 0,
                                                                                 0});
            // 
            // numberSamplesNumeric
            // 
            this.numberSamplesNumeric.Location = new System.Drawing.Point(144, 128);
            this.numberSamplesNumeric.Maximum = new System.Decimal(new int[] {
                                                                                 100000,
                                                                                 0,
                                                                                 0,
                                                                                 0});
            this.numberSamplesNumeric.Minimum = new System.Decimal(new int[] {
                                                                                 1,
                                                                                 0,
                                                                                 0,
                                                                                 0});
            this.numberSamplesNumeric.Name = "numberSamplesNumeric";
            this.numberSamplesNumeric.Size = new System.Drawing.Size(176, 20);
            this.numberSamplesNumeric.TabIndex = 7;
            this.numberSamplesNumeric.Value = new System.Decimal(new int[] {
                                                                               1000,
                                                                               0,
                                                                               0,
                                                                               0});
            // 
            // physicalChannelComboBox
            // 
            this.physicalChannelComboBox.Location = new System.Drawing.Point(144, 17);
            this.physicalChannelComboBox.Name = "physicalChannelComboBox";
            this.physicalChannelComboBox.Size = new System.Drawing.Size(176, 21);
            this.physicalChannelComboBox.TabIndex = 1;
            this.physicalChannelComboBox.Text = "Dev1/port0";
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(330, 192);
            this.Controls.Add(this.physicalChannelComboBox);
            this.Controls.Add(this.sampleClockRateNumeric);
            this.Controls.Add(this.writeButton);
            this.Controls.Add(this.clockSourceTextBox);
            this.Controls.Add(this.physicalChannelsLabel);
            this.Controls.Add(this.clockSourceLabel);
            this.Controls.Add(this.sampleClockRateLabel);
            this.Controls.Add(this.numberSamplesLabel);
            this.Controls.Add(this.numberSamplesNumeric);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Write Digital Channel - External Clock";
            ((System.ComponentModel.ISupportInitialize)(this.sampleClockRateNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberSamplesNumeric)).EndInit();
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
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                using (Task digitalWriteTask = new Task())
                {
                    // Create the digital output channel
                    digitalWriteTask.DOChannels.CreateChannel(physicalChannelComboBox.Text,"",
                        ChannelLineGrouping.OneChannelForAllLines);

                    // Verify the task so we can query the channel's properties
                    digitalWriteTask.Control(TaskAction.Verify);

                    // Create the data to write
                    int samples = (int)numberSamplesNumeric.Value;
                    int signals = (int)digitalWriteTask.DOChannels[0].NumberOfLines;
                    
                    // Set up the timing
                    digitalWriteTask.Timing.ConfigureSampleClock(clockSourceTextBox.Text, 
                        Convert.ToDouble(sampleClockRateNumeric.Value), 
                        SampleClockActiveEdge.Rising, SampleQuantityMode.FiniteSamples, 
                        Convert.ToInt32(numberSamplesNumeric.Value));

                    // Write the data
                    DigitalSingleChannelWriter writer = new DigitalSingleChannelWriter(digitalWriteTask.Stream);

                    // Loop through every sample
                    DigitalWaveform waveform = new DigitalWaveform(Convert.ToInt32(numberSamplesNumeric.Value),
                                                                    Convert.ToInt32(digitalWriteTask.DOChannels[0].NumberOfLines));
                    Random r = new Random();
                    for (int i = 0; i < samples; i++)
                    {
                        // Generate a random set of boolean values
                        for (int j = 0; j < signals; j++)
                        {
                            if (r.Next() % 2 == 0)
                                waveform.Samples[i].States[j] = DigitalState.ForceUp;
                            else
                                waveform.Samples[i].States[j] = DigitalState.ForceDown;
                        }
                    }

                    // Write those values
                    writer.WriteWaveform(true, waveform);

                    digitalWriteTask.WaitUntilDone();
                }
            }
            catch(DaqException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
    }
}
