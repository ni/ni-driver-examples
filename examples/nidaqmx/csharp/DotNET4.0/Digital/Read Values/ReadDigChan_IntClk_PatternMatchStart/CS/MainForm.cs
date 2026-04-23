/******************************************************************************
*
* Example program:
*   ReadDigChan_IntClk_PatternMatchStart
*
* Category:
*   DI
*
* Description:
*   This example demonstrates how to acquire a finite amount of digital data
*   (Waveform) using a pattern match start trigger (i.e. the acquisition begins
*   when a specified pattern has been matched).
*
* Instructions for running:
*   1.  Select the Physical Channel to correspond to where your signal is input
*       on the DAQ device.
*   2.  Select how many Samples to Acquire on Each Channel.
*   3.  Set the Rate of the Acquisition.
*   4.  Select the Pattern Match Channels, Pattern, and Trigger When parameters
*       of the Pattern Match Start Trigger for the acquisition.
*
* Steps:
*   1.  Create a new digital input task.
*   2.  Create a digital input channel. Use one channel of each line.
*   3.  Configure the sample clock, set the acquisition mode to finite.
*   4.  Use the StartTrigger object properties to configure a pattern trigger.
*   5.  Start the task to begin the acquisition.
*   6.  Call the DigitalMultiChannelReader.ReadWaveform method to read the data
*       and then display the acquired data.
*   7.  Dispose the Task object to clean-up any resources associated with the
*       task.
*   8.  Handle any DaqExceptions, if they occur.
*
* I/O Connections Overview:
*   Make sure your signal input terminal matches the Physical Channel I/O
*   Control and the Pattern Match Channels control.  For further connection
*   information, refer to your hardware reference manual.
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

namespace NationalInstruments.Examples.ReadDigChan_IntClk_PatternMatchStart
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Button readButton;
        private System.Windows.Forms.GroupBox resultsGroupBox;
        private System.Windows.Forms.DataGrid resultsDataGrid;
        internal System.Windows.Forms.GroupBox triggerParametersGroupBox;
        private System.Windows.Forms.TextBox patternTextBox;
        internal System.Windows.Forms.Label patternLabel;
        internal System.Windows.Forms.ComboBox triggerWhenComboBox;
        internal System.Windows.Forms.Label triggerWhenLabel;
        internal System.Windows.Forms.Label patternMatchChannelLabel;
        internal System.Windows.Forms.GroupBox timingParametersGroupBox;
        internal System.Windows.Forms.NumericUpDown samplesPerChannelNumericUpDown;
        internal System.Windows.Forms.Label samplesPerChannelLabel;
        internal System.Windows.Forms.NumericUpDown samplesClockRateNumericUpDown;
        internal System.Windows.Forms.Label sampleClockRateLabel;
        internal System.Windows.Forms.GroupBox channelParametersGroupBox;
        internal System.Windows.Forms.ComboBox physicalChannelComboBox;
        internal System.Windows.Forms.Label physicalChannelLabel;

        private Task myTask;
        private DigitalWaveform[] waveform;
        private DigitalMultiChannelReader reader;
        private DataTable dataTable;
        private DataColumn[] dataColumn = null;
        internal System.Windows.Forms.ComboBox patternMatchChannelComboBox;

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

            readButton.Enabled = false;
            dataTable = new DataTable();

            physicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DILine, PhysicalChannelAccess.External));
            patternMatchChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DILine, PhysicalChannelAccess.External));
            if (physicalChannelComboBox.Items.Count > 0)
            {
                physicalChannelComboBox.SelectedIndex = 0;
                patternMatchChannelComboBox.SelectedIndex = 0;
                readButton.Enabled = true;
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainForm));
            this.readButton = new System.Windows.Forms.Button();
            this.resultsGroupBox = new System.Windows.Forms.GroupBox();
            this.resultsDataGrid = new System.Windows.Forms.DataGrid();
            this.triggerParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.patternTextBox = new System.Windows.Forms.TextBox();
            this.patternLabel = new System.Windows.Forms.Label();
            this.triggerWhenComboBox = new System.Windows.Forms.ComboBox();
            this.triggerWhenLabel = new System.Windows.Forms.Label();
            this.patternMatchChannelComboBox = new System.Windows.Forms.ComboBox();
            this.patternMatchChannelLabel = new System.Windows.Forms.Label();
            this.timingParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.samplesPerChannelNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.samplesPerChannelLabel = new System.Windows.Forms.Label();
            this.samplesClockRateNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.sampleClockRateLabel = new System.Windows.Forms.Label();
            this.channelParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.physicalChannelComboBox = new System.Windows.Forms.ComboBox();
            this.physicalChannelLabel = new System.Windows.Forms.Label();
            this.resultsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resultsDataGrid)).BeginInit();
            this.triggerParametersGroupBox.SuspendLayout();
            this.timingParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerChannelNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesClockRateNumericUpDown)).BeginInit();
            this.channelParametersGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // readButton
            // 
            this.readButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.readButton.Location = new System.Drawing.Point(104, 335);
            this.readButton.Name = "readButton";
            this.readButton.TabIndex = 3;
            this.readButton.Text = "Read";
            this.readButton.Click += new System.EventHandler(this.readButton_Click);
            // 
            // resultsGroupBox
            // 
            this.resultsGroupBox.Controls.Add(this.resultsDataGrid);
            this.resultsGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.resultsGroupBox.Location = new System.Drawing.Point(312, 7);
            this.resultsGroupBox.Name = "resultsGroupBox";
            this.resultsGroupBox.Size = new System.Drawing.Size(272, 352);
            this.resultsGroupBox.TabIndex = 4;
            this.resultsGroupBox.TabStop = false;
            this.resultsGroupBox.Text = "Results";
            // 
            // resultsDataGrid
            // 
            this.resultsDataGrid.DataMember = "";
            this.resultsDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.resultsDataGrid.Location = new System.Drawing.Point(8, 16);
            this.resultsDataGrid.Name = "resultsDataGrid";
            this.resultsDataGrid.Size = new System.Drawing.Size(256, 328);
            this.resultsDataGrid.TabIndex = 0;
            this.resultsDataGrid.TabStop = false;
            // 
            // triggerParametersGroupBox
            // 
            this.triggerParametersGroupBox.Controls.Add(this.patternTextBox);
            this.triggerParametersGroupBox.Controls.Add(this.patternLabel);
            this.triggerParametersGroupBox.Controls.Add(this.triggerWhenComboBox);
            this.triggerParametersGroupBox.Controls.Add(this.triggerWhenLabel);
            this.triggerParametersGroupBox.Controls.Add(this.patternMatchChannelComboBox);
            this.triggerParametersGroupBox.Controls.Add(this.patternMatchChannelLabel);
            this.triggerParametersGroupBox.Location = new System.Drawing.Point(8, 191);
            this.triggerParametersGroupBox.Name = "triggerParametersGroupBox";
            this.triggerParametersGroupBox.Size = new System.Drawing.Size(288, 136);
            this.triggerParametersGroupBox.TabIndex = 2;
            this.triggerParametersGroupBox.TabStop = false;
            this.triggerParametersGroupBox.Text = "Trigger Parameters";
            // 
            // patternTextBox
            // 
            this.patternTextBox.Location = new System.Drawing.Point(160, 64);
            this.patternTextBox.Name = "patternTextBox";
            this.patternTextBox.Size = new System.Drawing.Size(120, 20);
            this.patternTextBox.TabIndex = 3;
            this.patternTextBox.Text = "00XX 11XX";
            // 
            // patternLabel
            // 
            this.patternLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.patternLabel.Location = new System.Drawing.Point(8, 64);
            this.patternLabel.Name = "patternLabel";
            this.patternLabel.TabIndex = 2;
            this.patternLabel.Text = "Pattern:";
            // 
            // triggerWhenComboBox
            // 
            this.triggerWhenComboBox.Items.AddRange(new object[] {
                                                                     "Pattern Matches",
                                                                     "Pattern Does Not Match"});
            this.triggerWhenComboBox.Location = new System.Drawing.Point(160, 104);
            this.triggerWhenComboBox.Name = "triggerWhenComboBox";
            this.triggerWhenComboBox.Size = new System.Drawing.Size(121, 21);
            this.triggerWhenComboBox.TabIndex = 5;
            this.triggerWhenComboBox.Text = "Pattern Matches";
            // 
            // triggerWhenLabel
            // 
            this.triggerWhenLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.triggerWhenLabel.Location = new System.Drawing.Point(8, 104);
            this.triggerWhenLabel.Name = "triggerWhenLabel";
            this.triggerWhenLabel.TabIndex = 4;
            this.triggerWhenLabel.Text = "Trigger When:";
            // 
            // patternMatchChannelComboBox
            // 
            this.patternMatchChannelComboBox.Location = new System.Drawing.Point(160, 18);
            this.patternMatchChannelComboBox.Name = "patternMatchChannelComboBox";
            this.patternMatchChannelComboBox.Size = new System.Drawing.Size(121, 21);
            this.patternMatchChannelComboBox.TabIndex = 1;
            this.patternMatchChannelComboBox.Text = "Dev1/port0/line0:7";
            // 
            // patternMatchChannelLabel
            // 
            this.patternMatchChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.patternMatchChannelLabel.Location = new System.Drawing.Point(8, 24);
            this.patternMatchChannelLabel.Name = "patternMatchChannelLabel";
            this.patternMatchChannelLabel.Size = new System.Drawing.Size(128, 23);
            this.patternMatchChannelLabel.TabIndex = 0;
            this.patternMatchChannelLabel.Text = "Pattern Match Channel:";
            // 
            // timingParametersGroupBox
            // 
            this.timingParametersGroupBox.Controls.Add(this.samplesPerChannelNumericUpDown);
            this.timingParametersGroupBox.Controls.Add(this.samplesPerChannelLabel);
            this.timingParametersGroupBox.Controls.Add(this.samplesClockRateNumericUpDown);
            this.timingParametersGroupBox.Controls.Add(this.sampleClockRateLabel);
            this.timingParametersGroupBox.Location = new System.Drawing.Point(8, 79);
            this.timingParametersGroupBox.Name = "timingParametersGroupBox";
            this.timingParametersGroupBox.Size = new System.Drawing.Size(288, 96);
            this.timingParametersGroupBox.TabIndex = 1;
            this.timingParametersGroupBox.TabStop = false;
            this.timingParametersGroupBox.Text = "Timing Parameters";
            // 
            // samplesPerChannelNumericUpDown
            // 
            this.samplesPerChannelNumericUpDown.Location = new System.Drawing.Point(160, 24);
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
            this.samplesPerChannelNumericUpDown.TabIndex = 1;
            this.samplesPerChannelNumericUpDown.Value = new System.Decimal(new int[] {
                                                                                         1000,
                                                                                         0,
                                                                                         0,
                                                                                         0});
            // 
            // samplesPerChannelLabel
            // 
            this.samplesPerChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.samplesPerChannelLabel.Location = new System.Drawing.Point(8, 24);
            this.samplesPerChannelLabel.Name = "samplesPerChannelLabel";
            this.samplesPerChannelLabel.Size = new System.Drawing.Size(120, 23);
            this.samplesPerChannelLabel.TabIndex = 0;
            this.samplesPerChannelLabel.Text = "Samples per Channel:";
            // 
            // samplesClockRateNumericUpDown
            // 
            this.samplesClockRateNumericUpDown.Location = new System.Drawing.Point(160, 64);
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
            this.samplesClockRateNumericUpDown.TabIndex = 3;
            this.samplesClockRateNumericUpDown.Value = new System.Decimal(new int[] {
                                                                                        100000,
                                                                                        0,
                                                                                        0,
                                                                                        0});
            // 
            // sampleClockRateLabel
            // 
            this.sampleClockRateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.sampleClockRateLabel.Location = new System.Drawing.Point(8, 64);
            this.sampleClockRateLabel.Name = "sampleClockRateLabel";
            this.sampleClockRateLabel.Size = new System.Drawing.Size(120, 23);
            this.sampleClockRateLabel.TabIndex = 2;
            this.sampleClockRateLabel.Text = "Sample Clock Rate (Hz):";
            // 
            // channelParametersGroupBox
            // 
            this.channelParametersGroupBox.Controls.Add(this.physicalChannelComboBox);
            this.channelParametersGroupBox.Controls.Add(this.physicalChannelLabel);
            this.channelParametersGroupBox.Location = new System.Drawing.Point(8, 7);
            this.channelParametersGroupBox.Name = "channelParametersGroupBox";
            this.channelParametersGroupBox.Size = new System.Drawing.Size(288, 56);
            this.channelParametersGroupBox.TabIndex = 0;
            this.channelParametersGroupBox.TabStop = false;
            this.channelParametersGroupBox.Text = "Channel Parameters";
            // 
            // physicalChannelComboBox
            // 
            this.physicalChannelComboBox.Location = new System.Drawing.Point(160, 18);
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
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(592, 366);
            this.Controls.Add(this.readButton);
            this.Controls.Add(this.resultsGroupBox);
            this.Controls.Add(this.triggerParametersGroupBox);
            this.Controls.Add(this.timingParametersGroupBox);
            this.Controls.Add(this.channelParametersGroupBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Read Dig Chan - Internal Clock - Pattern Match Start";
            this.resultsGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.resultsDataGrid)).EndInit();
            this.triggerParametersGroupBox.ResumeLayout(false);
            this.timingParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerChannelNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesClockRateNumericUpDown)).EndInit();
            this.channelParametersGroupBox.ResumeLayout(false);
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
            readButton.Enabled = false;
            try
            {
                DigitalPatternStartTriggerCondition condition;
                if (triggerWhenComboBox.SelectedIndex == 0)
                    condition = DigitalPatternStartTriggerCondition.PatternMatches;
                else
                    condition = DigitalPatternStartTriggerCondition.PatternDoesNotMatch;

                // Create and configure the Task
                myTask = new Task();
                myTask.DIChannels.CreateChannel(physicalChannelComboBox.Text, "", ChannelLineGrouping.OneChannelForEachLine);
                myTask.Timing.ConfigureSampleClock("", (double) samplesClockRateNumericUpDown.Value,
                                    SampleClockActiveEdge.Rising, SampleQuantityMode.FiniteSamples, (int) samplesPerChannelNumericUpDown.Value);
                myTask.Triggers.StartTrigger.ConfigureDigitalPatternTrigger(patternMatchChannelComboBox.Text, patternTextBox.Text, condition);

                myTask.Start();

                InitializeDataTable(ref dataTable);
                resultsDataGrid.DataSource = dataTable;

                reader = new DigitalMultiChannelReader(myTask.Stream);

                waveform = reader.ReadWaveform((int) samplesPerChannelNumericUpDown.Value);

                dataToDataTable(waveform, ref dataTable);
            }
            catch (DaqException exception)
            {
                MessageBox.Show(exception.Message);
            }
            finally
            {
                myTask.Dispose();
                readButton.Enabled = true;
            }
            Cursor.Current = Cursors.Default;
        }


        private void dataToDataTable(DigitalWaveform[] waveform, ref DataTable dataTable)
        {
            // Iterate over channels
            int currentLineIndex = 0;
            foreach (DigitalWaveform signal in waveform)
            {
                for(int sample = 0; sample < signal.Signals[0].States.Count; ++sample)
                {
                    if (sample == 10)
                    {
                        break;
                    }
                    if (signal.Signals[0].States[sample] == DigitalState.ForceUp)
                    {
                        dataTable.Rows[sample][currentLineIndex] = 1;
                    }
                    else
                    {
                        dataTable.Rows[sample][currentLineIndex] = 0;
                    }
                }
                currentLineIndex++;
            }
        }

        private void InitializeDataTable(ref DataTable data)
        {
            int numOfLines = Convert.ToInt32(myTask.DIChannels.Count);
            data.Rows.Clear();
            data.Columns.Clear();
            dataColumn = new DataColumn[numOfLines];
            int numOfRows = 10;

            for (int currentLineIndex = 0; currentLineIndex < numOfLines; currentLineIndex++)
            {
                dataColumn[currentLineIndex] = new DataColumn();
                dataColumn[currentLineIndex].DataType = typeof(int);
                dataColumn[currentLineIndex].ColumnName = myTask.DIChannels[currentLineIndex].PhysicalName;
            }
            data.Columns.AddRange(dataColumn);

            for (int currentDataIndex = 0; currentDataIndex < numOfRows; currentDataIndex++)
            {
                object[] rowArr = new object[numOfLines];
                data.Rows.Add(rowArr);
            }
        }
    }
}
