/******************************************************************************
*
* Example program:
*   ReadDigChan_IntClk_DigRef
*
* Category:
*   DI
*
* Description:
*   This example demonstrates how to acquire a finite amount of data (Waveform)
*   using a digital reference trigger.
*
* Instructions for running:
*   1.  Select the Physical Channel to correspond to where your signal is input
*       on the DAQ device.
*   2.  Select how many Samples to Acquire on Each Channel.
*   3.  Set the Rate of the Acquisition.
*   4.  Select the Source and Edge of the Digital Reference Trigger for the
*       acquisition.
*   5.  Set the number of Pre-Trigger samples.
*
* Steps:
*   1.  Create a new digital input task.
*   2.  Create a digital input channel. Use one channel of each line.
*   3.  Configure the sample clock, set the acquisition mode to finite.
*   4.  Use the ReferenceTrigger object properties to configure a digital edge
*       trigger.
*   5.  Start the task to begin the acquisition.
*   6.  Call the DigitalMultiChannelReader.ReadWaveform method to read the data
*       and then display the acquired data.
*   7.  Dispose the Task object to clean-up any resources associated with the
*       task.
*   8.  Handle any DaqExceptions, if they occur.
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

namespace NationalInstruments.Examples.ReadDigChan_IntClk_DigRef
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.GroupBox resultsGroupBox;
        private System.Windows.Forms.DataGrid resultsDataGrid;
        internal System.Windows.Forms.GroupBox triggerParametersGroupBox;
        internal System.Windows.Forms.GroupBox timingParametersGroupBox;
        internal System.Windows.Forms.NumericUpDown samplesPerChannelNumericUpDown;
        internal System.Windows.Forms.Label samplesPerChannelLabel;
        internal System.Windows.Forms.NumericUpDown samplesClockRateNumericUpDown;
        internal System.Windows.Forms.Label sampleClockRateLabel;
        internal System.Windows.Forms.GroupBox channelParametersGroupBox;
        internal System.Windows.Forms.ComboBox physicalChannelComboBox;
        internal System.Windows.Forms.Label physicalChannelLabel;
        internal System.Windows.Forms.Label edgeLabel;
        internal System.Windows.Forms.Label preTriggerSamplesLabel;
        internal System.Windows.Forms.NumericUpDown preTriggerSamplesNumericUpDown;
        internal System.Windows.Forms.ComboBox edgeComboBox;
        private System.Windows.Forms.Button readButton;
        internal System.Windows.Forms.Label triggerSourceLabel;
        internal System.Windows.Forms.ComboBox triggerSourceComboBox;

        private Task myTask;
        private NationalInstruments.DigitalWaveform[] waveform;
        private DigitalMultiChannelReader reader;
        private DataTable dataTable;
        private DataColumn[] dataColumn = null;

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
            triggerSourceComboBox.Items.AddRange(DaqSystem.Local.GetTerminals(TerminalTypes.All));
            
            if (physicalChannelComboBox.Items.Count > 0)
            {
                triggerSourceComboBox.SelectedIndex = 0;
                this.readButton.Enabled = true;             
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
            this.resultsGroupBox = new System.Windows.Forms.GroupBox();
            this.resultsDataGrid = new System.Windows.Forms.DataGrid();
            this.triggerParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.edgeComboBox = new System.Windows.Forms.ComboBox();
            this.preTriggerSamplesNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.edgeLabel = new System.Windows.Forms.Label();
            this.preTriggerSamplesLabel = new System.Windows.Forms.Label();
            this.triggerSourceComboBox = new System.Windows.Forms.ComboBox();
            this.triggerSourceLabel = new System.Windows.Forms.Label();
            this.timingParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.samplesPerChannelNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.samplesPerChannelLabel = new System.Windows.Forms.Label();
            this.samplesClockRateNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.sampleClockRateLabel = new System.Windows.Forms.Label();
            this.channelParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.physicalChannelComboBox = new System.Windows.Forms.ComboBox();
            this.physicalChannelLabel = new System.Windows.Forms.Label();
            this.readButton = new System.Windows.Forms.Button();
            this.resultsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resultsDataGrid)).BeginInit();
            this.triggerParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.preTriggerSamplesNumericUpDown)).BeginInit();
            this.timingParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerChannelNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesClockRateNumericUpDown)).BeginInit();
            this.channelParametersGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // resultsGroupBox
            // 
            this.resultsGroupBox.Controls.Add(this.resultsDataGrid);
            this.resultsGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.resultsGroupBox.Location = new System.Drawing.Point(312, 7);
            this.resultsGroupBox.Name = "resultsGroupBox";
            this.resultsGroupBox.Size = new System.Drawing.Size(272, 352);
            this.resultsGroupBox.TabIndex = 8;
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
            this.triggerParametersGroupBox.Controls.Add(this.edgeComboBox);
            this.triggerParametersGroupBox.Controls.Add(this.preTriggerSamplesNumericUpDown);
            this.triggerParametersGroupBox.Controls.Add(this.edgeLabel);
            this.triggerParametersGroupBox.Controls.Add(this.preTriggerSamplesLabel);
            this.triggerParametersGroupBox.Controls.Add(this.triggerSourceComboBox);
            this.triggerParametersGroupBox.Controls.Add(this.triggerSourceLabel);
            this.triggerParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.triggerParametersGroupBox.Location = new System.Drawing.Point(8, 191);
            this.triggerParametersGroupBox.Name = "triggerParametersGroupBox";
            this.triggerParametersGroupBox.Size = new System.Drawing.Size(288, 136);
            this.triggerParametersGroupBox.TabIndex = 7;
            this.triggerParametersGroupBox.TabStop = false;
            this.triggerParametersGroupBox.Text = "Trigger Parameters";
            // 
            // edgeComboBox
            // 
            this.edgeComboBox.Items.AddRange(new object[] {
                                                              "Rising",
                                                              "Falling"});
            this.edgeComboBox.Location = new System.Drawing.Point(160, 56);
            this.edgeComboBox.Name = "edgeComboBox";
            this.edgeComboBox.Size = new System.Drawing.Size(121, 21);
            this.edgeComboBox.TabIndex = 5;
            this.edgeComboBox.Text = "Rising";
            // 
            // preTriggerSamplesNumericUpDown
            // 
            this.preTriggerSamplesNumericUpDown.Location = new System.Drawing.Point(160, 96);
            this.preTriggerSamplesNumericUpDown.Maximum = new System.Decimal(new int[] {
                                                                                           1000000,
                                                                                           0,
                                                                                           0,
                                                                                           0});
            this.preTriggerSamplesNumericUpDown.Minimum = new System.Decimal(new int[] {
                                                                                           1,
                                                                                           0,
                                                                                           0,
                                                                                           0});
            this.preTriggerSamplesNumericUpDown.Name = "preTriggerSamplesNumericUpDown";
            this.preTriggerSamplesNumericUpDown.TabIndex = 6;
            this.preTriggerSamplesNumericUpDown.Value = new System.Decimal(new int[] {
                                                                                         100,
                                                                                         0,
                                                                                         0,
                                                                                         0});
            // 
            // edgeLabel
            // 
            this.edgeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.edgeLabel.Location = new System.Drawing.Point(8, 64);
            this.edgeLabel.Name = "edgeLabel";
            this.edgeLabel.Size = new System.Drawing.Size(100, 16);
            this.edgeLabel.TabIndex = 2;
            this.edgeLabel.Text = "Edge:";
            // 
            // preTriggerSamplesLabel
            // 
            this.preTriggerSamplesLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.preTriggerSamplesLabel.Location = new System.Drawing.Point(8, 96);
            this.preTriggerSamplesLabel.Name = "preTriggerSamplesLabel";
            this.preTriggerSamplesLabel.TabIndex = 4;
            this.preTriggerSamplesLabel.Text = "Pre-Trigger Samples:";
            // 
            // triggerSourceComboBox
            // 
            this.triggerSourceComboBox.Location = new System.Drawing.Point(160, 18);
            this.triggerSourceComboBox.Name = "triggerSourceComboBox";
            this.triggerSourceComboBox.Size = new System.Drawing.Size(121, 21);
            this.triggerSourceComboBox.TabIndex = 4;
            this.triggerSourceComboBox.Text = "/Dev1/PFI0";
            // 
            // triggerSourceLabel
            // 
            this.triggerSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.triggerSourceLabel.Location = new System.Drawing.Point(8, 24);
            this.triggerSourceLabel.Name = "triggerSourceLabel";
            this.triggerSourceLabel.Size = new System.Drawing.Size(128, 23);
            this.triggerSourceLabel.TabIndex = 0;
            this.triggerSourceLabel.Text = "Trigger Source:";
            // 
            // timingParametersGroupBox
            // 
            this.timingParametersGroupBox.Controls.Add(this.samplesPerChannelNumericUpDown);
            this.timingParametersGroupBox.Controls.Add(this.samplesPerChannelLabel);
            this.timingParametersGroupBox.Controls.Add(this.samplesClockRateNumericUpDown);
            this.timingParametersGroupBox.Controls.Add(this.sampleClockRateLabel);
            this.timingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.timingParametersGroupBox.Location = new System.Drawing.Point(8, 79);
            this.timingParametersGroupBox.Name = "timingParametersGroupBox";
            this.timingParametersGroupBox.Size = new System.Drawing.Size(288, 96);
            this.timingParametersGroupBox.TabIndex = 6;
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
            this.samplesPerChannelNumericUpDown.TabIndex = 2;
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
            this.channelParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.channelParametersGroupBox.Location = new System.Drawing.Point(8, 7);
            this.channelParametersGroupBox.Name = "channelParametersGroupBox";
            this.channelParametersGroupBox.Size = new System.Drawing.Size(288, 56);
            this.channelParametersGroupBox.TabIndex = 5;
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
            // readButton
            // 
            this.readButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.readButton.Location = new System.Drawing.Point(120, 336);
            this.readButton.Name = "readButton";
            this.readButton.TabIndex = 7;
            this.readButton.Text = "&Read";
            this.readButton.Click += new System.EventHandler(this.readButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(592, 366);
            this.Controls.Add(this.resultsGroupBox);
            this.Controls.Add(this.triggerParametersGroupBox);
            this.Controls.Add(this.timingParametersGroupBox);
            this.Controls.Add(this.channelParametersGroupBox);
            this.Controls.Add(this.readButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Read Digital Chan - Internal Clock - Digital Reference";
            this.resultsGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.resultsDataGrid)).EndInit();
            this.triggerParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.preTriggerSamplesNumericUpDown)).EndInit();
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
                myTask = new Task();

                myTask.DIChannels.CreateChannel(physicalChannelComboBox.Text, "", ChannelLineGrouping.OneChannelForEachLine);
                myTask.Timing.ConfigureSampleClock("", (double)samplesClockRateNumericUpDown.Value, SampleClockActiveEdge.Rising, 
                                                   SampleQuantityMode.FiniteSamples, (int) samplesPerChannelNumericUpDown.Value);
                
                DigitalEdgeReferenceTriggerEdge edge;

                if(edgeComboBox.Text == "Rising")
                {
                    edge = DigitalEdgeReferenceTriggerEdge.Rising;
                } 
                else 
                {
                    edge = DigitalEdgeReferenceTriggerEdge.Falling;
                }

                myTask.Control(TaskAction.Verify);

                myTask.Triggers.ReferenceTrigger.ConfigureDigitalEdgeTrigger(triggerSourceComboBox.Text, edge, (long) preTriggerSamplesNumericUpDown.Value);

                myTask.Start();

                InitializeDataTable(ref dataTable);
                resultsDataGrid.DataSource = dataTable;

                reader = new DigitalMultiChannelReader(myTask.Stream);

                waveform = reader.ReadWaveform((int) samplesPerChannelNumericUpDown.Value);

                dataToDataTable(waveform, ref dataTable);
            } 
            catch(DaqException exception)
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

        private void dataToDataTable(NationalInstruments.DigitalWaveform[] waveform, ref DataTable dataTable)
        {
            int currentLineIndex = 0;
            foreach (NationalInstruments.DigitalWaveform signal in waveform)
            {
                // We populate the DataGrid with maximum 10 samples
                for(int sample = 0; sample < signal.Signals[0].States.Count && sample < 10; ++sample)
                {
                    if (signal.Signals[0].States[sample] == NationalInstruments.DigitalState.ForceUp)
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