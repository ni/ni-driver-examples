/******************************************************************************
*
* Example program:
*   ReadDigPort_ExtClk
*
* Category:
*   DI
*
* Description:
*   This example demonstrates how to read values from a digital port using an
*   external sample clock.
*
* Instructions for running:
*   1.  Select the physical channel on the DAQ device.Note: You must specify
*       exactly 8 lines in the physical channel text box.
*   2.  Select the external clock source.
*   3.  Select the number of samples per channel.
*   4.  Select the sample clock rate.
*
* Steps:
*   1.  Create a new digital input task.
*   2.  Create the digital input channel.
*   3.  Configure the task to use an external sample clock.
*   4.  Create a DigitalSingleChannelReader and associate it with the task by
*       using the task's stream.
*   5.  Call DigitalSingleChannelReader.ReadSingleSamplePortByte to read the
*       data from the channel.
*   6.  Dispose the Task object to clean-up any resources associated with the
*       task.
*   7.  Handle any DaqExceptions, if they occur.
*
* I/O Connections Overview:
*   Make sure your signal input terminals match the physical channel text box. 
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

namespace NationalInstruments.Examples.ReadDigPort_ExtClk
{
    /// <summary>
    /// Summary description for MainForm.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        private Task myTask;
        private DigitalSingleChannelReader reader;

        private System.Windows.Forms.GroupBox channelParametersGroupBox;
        private System.Windows.Forms.NumericUpDown samplesPerChannelNumeric;
        private System.Windows.Forms.TextBox clockSourceTextBox;
        private System.Windows.Forms.Label clockSourceLabel;
        private System.Windows.Forms.Label physicalChannelLabel;
        private System.Windows.Forms.Label samplesPerChannelLabel;
        private System.Windows.Forms.NumericUpDown sampleRateNumeric;
        private System.Windows.Forms.Label sampleRateLabel;
        private System.Windows.Forms.GroupBox digitalValuesGroupBox;
        private System.Windows.Forms.ListBox valuesListBox;
        private System.Windows.Forms.Button readButton;
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
            physicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DIPort, PhysicalChannelAccess.External));
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
            this.channelParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.physicalChannelComboBox = new System.Windows.Forms.ComboBox();
            this.samplesPerChannelNumeric = new System.Windows.Forms.NumericUpDown();
            this.clockSourceTextBox = new System.Windows.Forms.TextBox();
            this.clockSourceLabel = new System.Windows.Forms.Label();
            this.physicalChannelLabel = new System.Windows.Forms.Label();
            this.samplesPerChannelLabel = new System.Windows.Forms.Label();
            this.sampleRateNumeric = new System.Windows.Forms.NumericUpDown();
            this.sampleRateLabel = new System.Windows.Forms.Label();
            this.digitalValuesGroupBox = new System.Windows.Forms.GroupBox();
            this.valuesListBox = new System.Windows.Forms.ListBox();
            this.readButton = new System.Windows.Forms.Button();
            this.channelParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerChannelNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sampleRateNumeric)).BeginInit();
            this.digitalValuesGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // channelParametersGroupBox
            // 
            this.channelParametersGroupBox.Controls.Add(this.physicalChannelComboBox);
            this.channelParametersGroupBox.Controls.Add(this.samplesPerChannelNumeric);
            this.channelParametersGroupBox.Controls.Add(this.clockSourceTextBox);
            this.channelParametersGroupBox.Controls.Add(this.clockSourceLabel);
            this.channelParametersGroupBox.Controls.Add(this.physicalChannelLabel);
            this.channelParametersGroupBox.Controls.Add(this.samplesPerChannelLabel);
            this.channelParametersGroupBox.Controls.Add(this.sampleRateNumeric);
            this.channelParametersGroupBox.Controls.Add(this.sampleRateLabel);
            this.channelParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.channelParametersGroupBox.Location = new System.Drawing.Point(8, 8);
            this.channelParametersGroupBox.Name = "channelParametersGroupBox";
            this.channelParametersGroupBox.Size = new System.Drawing.Size(312, 176);
            this.channelParametersGroupBox.TabIndex = 0;
            this.channelParametersGroupBox.TabStop = false;
            this.channelParametersGroupBox.Text = "Channel Parameters";
            // 
            // physicalChannelComboBox
            // 
            this.physicalChannelComboBox.Location = new System.Drawing.Point(144, 33);
            this.physicalChannelComboBox.Name = "physicalChannelComboBox";
            this.physicalChannelComboBox.Size = new System.Drawing.Size(152, 21);
            this.physicalChannelComboBox.TabIndex = 1;
            this.physicalChannelComboBox.Text = "Dev1/port0";
            // 
            // samplesPerChannelNumeric
            // 
            this.samplesPerChannelNumeric.Location = new System.Drawing.Point(144, 105);
            this.samplesPerChannelNumeric.Maximum = new System.Decimal(new int[] {
                                                                                     100000,
                                                                                     0,
                                                                                     0,
                                                                                     0});
            this.samplesPerChannelNumeric.Minimum = new System.Decimal(new int[] {
                                                                                     1,
                                                                                     0,
                                                                                     0,
                                                                                     0});
            this.samplesPerChannelNumeric.Name = "samplesPerChannelNumeric";
            this.samplesPerChannelNumeric.Size = new System.Drawing.Size(152, 20);
            this.samplesPerChannelNumeric.TabIndex = 5;
            this.samplesPerChannelNumeric.Value = new System.Decimal(new int[] {
                                                                                   1000,
                                                                                   0,
                                                                                   0,
                                                                                   0});
            // 
            // clockSourceTextBox
            // 
            this.clockSourceTextBox.Location = new System.Drawing.Point(144, 69);
            this.clockSourceTextBox.Name = "clockSourceTextBox";
            this.clockSourceTextBox.Size = new System.Drawing.Size(152, 20);
            this.clockSourceTextBox.TabIndex = 3;
            this.clockSourceTextBox.Text = "/Dev1/PFI0";
            // 
            // clockSourceLabel
            // 
            this.clockSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.clockSourceLabel.Location = new System.Drawing.Point(16, 68);
            this.clockSourceLabel.Name = "clockSourceLabel";
            this.clockSourceLabel.TabIndex = 2;
            this.clockSourceLabel.Text = "Clock Source:";
            // 
            // physicalChannelLabel
            // 
            this.physicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.physicalChannelLabel.Location = new System.Drawing.Point(16, 32);
            this.physicalChannelLabel.Name = "physicalChannelLabel";
            this.physicalChannelLabel.TabIndex = 0;
            this.physicalChannelLabel.Text = "Physical Channel:";
            // 
            // samplesPerChannelLabel
            // 
            this.samplesPerChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.samplesPerChannelLabel.Location = new System.Drawing.Point(16, 104);
            this.samplesPerChannelLabel.Name = "samplesPerChannelLabel";
            this.samplesPerChannelLabel.Size = new System.Drawing.Size(120, 23);
            this.samplesPerChannelLabel.TabIndex = 4;
            this.samplesPerChannelLabel.Text = "Samples per Channel:";
            // 
            // sampleRateNumeric
            // 
            this.sampleRateNumeric.DecimalPlaces = 2;
            this.sampleRateNumeric.Location = new System.Drawing.Point(144, 141);
            this.sampleRateNumeric.Maximum = new System.Decimal(new int[] {
                                                                              100000,
                                                                              0,
                                                                              0,
                                                                              0});
            this.sampleRateNumeric.Minimum = new System.Decimal(new int[] {
                                                                              1,
                                                                              0,
                                                                              0,
                                                                              0});
            this.sampleRateNumeric.Name = "sampleRateNumeric";
            this.sampleRateNumeric.Size = new System.Drawing.Size(152, 20);
            this.sampleRateNumeric.TabIndex = 7;
            this.sampleRateNumeric.Value = new System.Decimal(new int[] {
                                                                            1000,
                                                                            0,
                                                                            0,
                                                                            0});
            // 
            // sampleRateLabel
            // 
            this.sampleRateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.sampleRateLabel.Location = new System.Drawing.Point(16, 140);
            this.sampleRateLabel.Name = "sampleRateLabel";
            this.sampleRateLabel.Size = new System.Drawing.Size(120, 23);
            this.sampleRateLabel.TabIndex = 6;
            this.sampleRateLabel.Text = "Sample Rate (Hz):";
            // 
            // digitalValuesGroupBox
            // 
            this.digitalValuesGroupBox.Controls.Add(this.valuesListBox);
            this.digitalValuesGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.digitalValuesGroupBox.Location = new System.Drawing.Point(8, 192);
            this.digitalValuesGroupBox.Name = "digitalValuesGroupBox";
            this.digitalValuesGroupBox.Size = new System.Drawing.Size(312, 184);
            this.digitalValuesGroupBox.TabIndex = 1;
            this.digitalValuesGroupBox.TabStop = false;
            this.digitalValuesGroupBox.Text = "Digital Values";
            // 
            // valuesListBox
            // 
            this.valuesListBox.Location = new System.Drawing.Point(8, 16);
            this.valuesListBox.Name = "valuesListBox";
            this.valuesListBox.Size = new System.Drawing.Size(296, 160);
            this.valuesListBox.TabIndex = 0;
            // 
            // readButton
            // 
            this.readButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.readButton.Location = new System.Drawing.Point(128, 384);
            this.readButton.Name = "readButton";
            this.readButton.TabIndex = 2;
            this.readButton.Text = "Read";
            this.readButton.Click += new System.EventHandler(this.readButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(328, 414);
            this.Controls.Add(this.readButton);
            this.Controls.Add(this.digitalValuesGroupBox);
            this.Controls.Add(this.channelParametersGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Read Digital Port - External Clock";
            this.channelParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerChannelNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sampleRateNumeric)).EndInit();
            this.digitalValuesGroupBox.ResumeLayout(false);
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

        private void readButton_Click(object sender, System.EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                // Create the task
                using (myTask = new Task())
                {
                    // Create the digital input channel
                    myTask.DIChannels.CreateChannel(physicalChannelComboBox.Text, "", 
                        ChannelLineGrouping.OneChannelForAllLines);

                    // Configure the external clock
                    myTask.Timing.ConfigureSampleClock(clockSourceTextBox.Text, 
                        Convert.ToDouble(sampleRateNumeric.Value),
                        SampleClockActiveEdge.Rising, SampleQuantityMode.FiniteSamples, Convert.ToInt32(samplesPerChannelNumeric.Value));

                    // Create a task reader
                    reader = new DigitalSingleChannelReader(myTask.Stream);
                    
                    // Read the data
                    byte[] data = reader.ReadMultiSamplePortByte(Convert.ToInt32(samplesPerChannelNumeric.Value));

                    // Update the UI
                    valuesListBox.Items.Clear();

                    for (int i = 0; i < data.Length; i++)
                    {
                        valuesListBox.Items.Add(data[i]);
                    }
                }                
            }
            catch (DaqException ex)
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
